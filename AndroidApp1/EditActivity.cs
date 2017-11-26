using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace AndroidApp1
{
    [Activity(Label = "EditActivity")]
    public class EditActivity : Activity
    {
        private Person person;
        private DataBase database;

        private EditText firstName;
        private EditText lastName;
        private EditText phoneNumber;

        private Button cancelButton;
        private Button submitButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EditLayout);

            //pass the person object and its id to this activity
            person = JsonConvert.DeserializeObject<Person>(Intent.GetStringExtra("person"));
            long id = Intent.GetLongExtra("id",0);

            string folder= System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            database = DataBase.getInstance(folder);

            //initialize the texts
            firstName = FindViewById<EditText>(Resource.Id.FirstNameTxt);
            lastName = FindViewById<EditText>(Resource.Id.LastNameTxt);
            phoneNumber = FindViewById<EditText>(Resource.Id.PhoneNumberTxt);

            //initialize the buttons
            cancelButton = FindViewById<Button>(Resource.Id.btnCancel);
            submitButton = FindViewById<Button>(Resource.Id.btnSubmit);


            firstName.Text = person.FirstName;
            firstName.Tag = id;
            lastName.Text = person.LastName;
            phoneNumber.Text = person.PhoneNumber;

            cancelButton.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };

            submitButton.Click += (sender, e) =>
            {
                Regex letterRegex = new Regex("^[a-zA-z]+$$");
                Regex numRegex = new Regex("^(0|[1-9][0-9]*)$");
                if (letterRegex.IsMatch(firstName.Text) && letterRegex.IsMatch(lastName.Text) && numRegex.IsMatch(phoneNumber.Text))
                {
                    Person person1 = new Person()
                    {
                        Id = int.Parse(firstName.Tag.ToString()),
                        FirstName = firstName.Text,
                        LastName = lastName.Text,
                        PhoneNumber = phoneNumber.Text
                    };
                    bool b = database.updateTablePerson(person1);
                    Console.WriteLine(b);
                    Intent intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                }
                else
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this, AlertDialog.ThemeHoloLight);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("Title");
                    alert.SetMessage("Please enter a valid name and phone number");
                    alert.SetButton("OK", (c, ev) =>
                    {
                        alert.Dismiss();
                    });
                    alert.Show();

                }
            };
        }
    }
}