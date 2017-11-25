using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Content;
using Android.Util;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AndroidApp1
{
    [Activity(Label = "AndroidApp1", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Button _button;
        private ListView _listData;

        private List<Person> _listSource=new List<Person>();
        private DataBase _db;
        private string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);


        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Deal with dataBase
            _db = DataBase.getInstance(folder);
            _db.CreateDataBase();
            Log.Info("DB_PATH", folder);


            //initialize add button
            _button = FindViewById<Button>(Resource.Id.addBtn);
            
            //initialize ListView
            _listData = FindViewById<ListView>(Resource.Id.listView1);

            loadData();

            _button.Click += addBtnClicked;

            
            //Change the background color when the list view item is clicked
            _listData.ItemClick += (sender, e) =>
            {
                for (int i = 0; i < _listData.Count; i++)
                {
                    if (e.Position == i)
                    {
                        _listData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.DarkGray);
                    }
                    else
                    {
                        _listData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);
                    }

                }               
            };
          
            _listData.ItemLongClick += (sender, e) =>
            {
                _listSource = _db.selectTablePerson();
                PopupMenu menu = new PopupMenu(this, _listData.GetChildAt(e.Position));
                menu.Inflate(Resource.Layout.PopupMenu);

                menu.MenuItemClick += (sender1,e1)=> {
                    if (e1.Item.TitleFormatted.ToString().Equals("Delete"))
                    {
                        Person person = _listSource[e.Position];
                        _db.deleteTablePerson(person);
                    }
                    if (e1.Item.TitleFormatted.ToString().Equals("Edit"))
                    {
                        Intent intent = new Intent(this,typeof(EditActivity));
                        Person person = _listSource[e.Position];

                        long id = e.Id;
                        intent.PutExtra("id", id);
                        intent.PutExtra("person", JsonConvert.SerializeObject(person));
                       
                        StartActivity(intent);
                    }

                    loadData();
                };

                menu.Show();

            };

        }

       
        private void addBtnClicked(object sender, EventArgs e)
        {
            FragmentTransaction fragment = FragmentManager.BeginTransaction();
            AddFragment addFragment = new AddFragment(_db,_listData,this);
            addFragment.Show(fragment, "add contact dialog");
            _db = addFragment.GetDataBase();
        }

        private void loadData()
        {
            _listSource = _db.selectTablePerson();
            var adapter = new ListViewAdapter(this, _listSource);
            _listData.Adapter = adapter;
        }
    }
}

