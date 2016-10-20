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
using System.Threading.Tasks;
using Android.Util;
using System.Net;

namespace _19EastApp
{
    [Activity( Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() => {
                //Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
                Task.Delay(2000);  // Simulate a bit of startup work.
 
                //Log.Debug(TAG, "Working in the background - important stuff.");

            });

            startupWork.ContinueWith(t => {
                if (CheckInternetConnection()) { 
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                }else
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Connection Error");
                    alert.SetMessage("Unable to connect. Please make sure you are connected to the internet");
                    alert.SetPositiveButton("Retry", (senderAlert, args) => {
                        this.Recreate();
                    });

                    alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                        System.Environment.Exit(0);
                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }


        public bool CheckInternetConnection()
        {
            string CheckUrl = GetString(Resource.String.gigs_url);

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

                iNetRequest.Timeout = 5000;

                WebResponse iNetResponse = iNetRequest.GetResponse();

                // Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
                iNetResponse.Close();

                return true;

            }
            catch (WebException ex)
            {
                Android.Widget.Toast.MakeText(this, "Connection Error. Please make sure you are connected to the internet", Android.Widget.ToastLength.Short).Show();
                Console.WriteLine("WebException" + ex.Message);

                return false;
            }
        }
    }
}