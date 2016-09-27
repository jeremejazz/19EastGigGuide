using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Net;
using System.Json;
using System.IO;

namespace _19EastApp
{
    [Activity(Label = "19East Gigs", MainLauncher = true, Icon = "@drawable/logo")]
    public class MainActivity : Activity
    {
 

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            
        }

     



    }

}

