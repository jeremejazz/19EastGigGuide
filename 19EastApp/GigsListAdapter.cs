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

namespace _19EastApp
{
    class GigsListAdapter : BaseAdapter<Gig>
    {
        List<Gig> items;
        Activity context;
        public GigsListAdapter(Activity context, List<Gig> items) 
            : base()
        {
            this.context = context;
            this.items = items;

        }
        public override Gig this[int position]
        {
            get
            {
                return items[position];
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.event_row, null);

            view.FindViewById<TextView>(Resource.Id.event_description).Text = item.Description;
            view.FindViewById<TextView>(Resource.Id.event_date).Text = item.EventDate.ToLongDateString();
            
 

            return view;
        }
    }
}