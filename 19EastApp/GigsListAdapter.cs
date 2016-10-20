using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Object = Java.Lang.Object;
using System;

namespace _19EastApp
{
    class GigsListAdapter : BaseAdapter<Gig>, IFilterable
    {
        List<Gig> items;
        Activity context;
        private List<Gig> _originalData;
        public GigsListAdapter(Activity context, List<Gig> items) 
            : base()
        {
            this.context = context;
            this.items = items;
            Filter = new GigsFilter(this);
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

        public Filter Filter
        {
            get; private set;
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


        private class GigsFilter : Filter
        {
            private readonly GigsListAdapter _adapter; 

            public GigsFilter( GigsListAdapter adapter)
            {
                _adapter = adapter;
            }

                
            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var returnObj = new FilterResults();

                var results = new List<Gig>();
                if(_adapter._originalData  == null)
                {
                    _adapter._originalData = _adapter.items;
                }

                if (constraint == null) return returnObj;

                if (_adapter._originalData != null && _adapter._originalData.Any() && constraint.ToString() != null)
                {
                    Console.WriteLine("constraint: " + constraint.ToString());
                    results.AddRange(
                        _adapter._originalData.Where(
                            gig => gig.Description.ToLower().Contains(constraint.ToString()))
                        );
                }

                returnObj.Values = FromArray(results.Select( r=> r.ToJavaObject()).ToArray());
                returnObj.Count = results.Count;

                constraint.Dispose();

                return returnObj;
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                    _adapter.items = values.ToArray<Object>()
                        .Select(r => r.ToNetObject<Gig>()).ToList();
                _adapter.NotifyDataSetChanged();

                constraint.Dispose();
                results.Dispose();
            }
        }

    }
}