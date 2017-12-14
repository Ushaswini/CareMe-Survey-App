using Android.App;
using Android.Preferences;
using Android.Gms.Iid;
using Android;
using Android.Gms.Gcm;
using Android.Util;
using Android.Support.V4.Content;
using System;
using Android.Content;
using NavigationDrawer;
using edu.uncc.homework4.Models;
using System.Net.Http;
using Android.Widget;

namespace edu.uncc.homework4
{
	[Service (Exported = false)]
	public class RegistrationIntentService : IntentService
	{
		const string TAG = "RegIntentService";
		static readonly string[] TOPICS = {"global"};
        ISharedPreferences prefs;

		public RegistrationIntentService() : base (TAG) {
           // prefs = PreferenceManager.GetDefaultSharedPreferences(this);
        }

		protected override void OnHandleIntent (Intent intent)
		{
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);

			try {
				lock (TAG) {
					var instanceID = InstanceID.GetInstance(this);
					var token = instanceID.GetToken(GetString(NavigationDrawer.Resource.String.gcm_defaultSenderId),
						GoogleCloudMessaging.InstanceIdScope, null);
					Log.Info(TAG, "GCM Registration Token: " + token);
					SendRegistrationToServer(token);
                    //SubscribeTopics(token);
                    prefs.Edit().PutBoolean(CONSTANTS.RECEIVE_NOTIFICATION_BOOLEAN, true).Apply();
				}
			} catch (Exception e) {
				Log.Debug(TAG, "Failed to complete token refresh", e);
                prefs.Edit().PutBoolean(CONSTANTS.RECEIVE_NOTIFICATION_BOOLEAN, false).Apply();
			}
			var registrationComplete = new Intent(CONSTANTS.REGISTRATION_COMPLETE);
			LocalBroadcastManager.GetInstance(this).SendBroadcast(registrationComplete);
		}

		void SendRegistrationToServer(string token) {
            // Add custom implementation, as needed.
            string deviceId = prefs.GetString(Constants.DEVICEID, "");

            if (!deviceId.Equals(token))
            {
                var model = new DeviceIdModel
                {
                    UserId = prefs.GetString(CONSTANTS.USERID,""),
                    DeviceId = token
                };

                try
                {
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + prefs.GetString(CONSTANTS.AUTH_HEADER, ""));
                    client.PostAsync(CONSTANTS.POST_DEVICEID_URL, new FormUrlEncodedContent(model.ToDict())).ContinueWith((response) => {

                        Console.Write(response);
                    });
                    
                }catch(Exception e)
                {
                    Toast.MakeText(this, e.Message, ToastLength.Short).Show();

                }               
                    
                //post
                Console.Write("token is " + token);
            }
        }

		void SubscribeTopics(string token) {
			foreach (var topic in TOPICS) {
				var pubSub = GcmPubSub.GetInstance(this);
				pubSub.Subscribe(token, "/topics/" + topic, null);
			}
		}
	}
}

