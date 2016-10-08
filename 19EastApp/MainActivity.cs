using System;
using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Net;
using System.Json;
using System.Collections.Generic;
using System.Net.Http;


namespace _19EastApp
{
    [Activity(Label = "19East Gigs", MainLauncher = true, Icon = "@drawable/logo", Theme = "@style/MyCustomTheme")]
    public class MainActivity : Activity
    {
        private GigsListAdapter adapter;
        protected override async void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            List<Gig> Gigs = new List<Gig>();
            
            ListView listView = FindViewById<ListView>(Resource.Id.GigList);
            EditText filterList = FindViewById<EditText>(Resource.Id.filterList);

            try
            { 
                JsonValue json = await FetchGigsAsync(GetString(Resource.String.gigs_url));
                Console.WriteLine("jsonCount: " + json.Count);
                ParseAndDisplay(json, ref Gigs);


                adapter = new GigsListAdapter(this, Gigs);
                listView.Adapter = adapter;
            }
            catch (WebException ex)
            {
                Console.WriteLine("An Error in the internet connection has occured" + ex.Message);
                throw;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occured -->" + ex.Message);
                throw;
            }


            filterList.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                //adapter.Filter.InvokeFilter(e.Text.ToString().ToLower());
            };
            
        }


        // Gets weather data from the passed URL.  
        private async Task<JsonValue> FetchGigsAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(2000);
            
            client.MaxResponseContentBufferSize = 256000;
            try {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Parse(content));
                    return jsonDoc;
                } else
                {
                    Console.WriteLine("URL Error" + response.StatusCode.ToString());
                    Android.Widget.Toast.MakeText(this, "Error in fetching. Status code " + response.StatusCode, Android.Widget.ToastLength.Long).Show();
                    throw new Exception("Something wrong just hapenz");
                }
            } catch (TaskCanceledException ex) { 
                 Console.WriteLine("Error: " + ex.Message);
                 Console.WriteLine(ex.InnerException.Message);
                throw ex;
            } catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                return null;
            }
            finally
            {
                client.Dispose();
            }
            /*
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            
            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                 
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    //  Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                }
            }
            */
        }

        private void ParseAndDisplay(JsonValue json, ref List<Gig> gigs )
        {

            // Extract the array of name/value results for the field name "weatherObservation":
            // Note that there is no exception handling for when this field is not found.
           
            
            if (json.Count == 0)
            {
                Android.Widget.Toast.MakeText(this, "Unable to Fetch items", Android.Widget.ToastLength.Long).Show();
            }
            else
            {
                foreach (JsonObject item in json)
                {



                    gigs.Add(new Gig() { EventDate = DateTime.Parse(item["event_date"]) , Description = item["event_gig"] });

                }

              
            }

            // Extract the "stationName" (location string) and write it to the location TextBox:
            //Console.WriteLine(items[0].ToString());
        }

       


    }

}

