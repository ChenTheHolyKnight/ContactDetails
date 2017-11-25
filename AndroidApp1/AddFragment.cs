using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Text.RegularExpressions;

namespace AndroidApp1
{
    public class AddFragment : DialogFragment
    {
        private Button _cancelBtn;
        private Button _submitBtn;

        private EditText _firstNameTxt;
        private EditText _lastNameTxt;
        private EditText _phoneNumberTxt;

        private DataBase _dataBase;
        private List<Person> _listPeople;
        private ListView _listView;
        private Activity _mainActivity;

        public AddFragment(DataBase dataBase,ListView listView,Activity activity)
        {
            _dataBase = dataBase;
            _mainActivity = activity;
            _listView = listView;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.AddFragment, container);

            //initialize buttons
            _cancelBtn = view.FindViewById<Button>(Resource.Id.cancelBtn);
            _submitBtn = view.FindViewById<Button>(Resource.Id.submitBtn);

            //initialize texts
            _firstNameTxt = view.FindViewById<EditText>(Resource.Id.firstNametxt);
            _lastNameTxt = view.FindViewById<EditText>(Resource.Id.LastNametxt);
            _phoneNumberTxt = view.FindViewById<EditText>(Resource.Id.phoneNumtxt);


            _cancelBtn.Click += (sender, e) =>
            {
                Dismiss();
            };
            _submitBtn.Click += (sender, e) =>
             {
                 Regex letterRegex = new Regex("^[a - zA - z] +$$");
                 Regex numRegex = new Regex("^(0|[1-9][0-9]*)$");
                 if (letterRegex.IsMatch(_firstNameTxt.Text) && letterRegex.IsMatch(_lastNameTxt.Text) && numRegex.IsMatch(_phoneNumberTxt.Text))
                 {
                     Person person = new Person() { FirstName = _firstNameTxt.Text, LastName = _lastNameTxt.Text, PhoneNumber = _phoneNumberTxt.Text };
                     _dataBase.insertIntoTablePerson(person);

                     _listPeople = _dataBase.selectTablePerson();
                     var adapter = new ListViewAdapter(_mainActivity, _listPeople);
                     _listView.Adapter = adapter;

                     Dismiss();
                 }
                 else {
                     AlertDialog.Builder dialog = new AlertDialog.Builder(view.Context,  AlertDialog.ThemeHoloLight);
                     AlertDialog alert = dialog.Create();
                     alert.SetTitle("Title");
                     alert.SetMessage("Please enter a valid name and a phone number");
                     alert.SetButton("OK", (c, ev) =>
                     {
                         Dismiss();  
                     });
                     alert.Show();
                 }

                
             };

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);            
        }

        public DataBase GetDataBase() {
            return _dataBase;
        }

        
    }
}