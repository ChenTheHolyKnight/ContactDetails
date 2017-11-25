using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace AndroidApp1
{
    class ListViewAdapter : BaseAdapter
    {
        private List<Person> _listSource;
        private Activity _activity;

        

        public ListViewAdapter(Activity activity,List<Person> listSource)
        {
            _listSource = listSource;
            _activity = activity;
        }
        public override int Count
        {
            get {
                return _listSource.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return _listSource[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (convertView == null) {
                row = _activity.LayoutInflater.Inflate(Resource.Layout.ListViewRow, parent, false);
            }

            var firstName = row.FindViewById<TextView>(Resource.Id.firstNameTextView);
            var lastName = row.FindViewById<TextView>(Resource.Id.lastNameTextView);
            var phone = row.FindViewById<TextView>(Resource.Id.phoneNumTextView);

            firstName.Text = _listSource[position].FirstName;
            lastName.Text = _listSource[position].LastName;
            phone.Text = _listSource[position].PhoneNumber;

            return row;
        }
    }
}