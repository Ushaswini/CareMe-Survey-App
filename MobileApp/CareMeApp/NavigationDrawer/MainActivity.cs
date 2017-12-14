
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
using Android.Preferences;
using Android.Net;
using System.Net.Http;
using edu.uncc.homework4.Models;
using Newtonsoft.Json;
using edu.uncc.homework4;

namespace NavigationDrawer
{
	[Activity (Label = "@string/app_name",MainLauncher =true)]			
	/*public class MainActivity : Activity, AdapterView.IOnItemClickListener
	{
		internal Sample[] mSamples;
		internal GridView mGridView;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.activity_main);

			// Prepare list of samples in this dashboard.
			mSamples = new Sample[] {
				new Sample (Resource.String.navigationdraweractivity_title, 
					Resource.String.navigationdraweractivity_description,
					this,
					typeof(NavigationDrawerActivity)),
			};

			// Prepare the GridView
			mGridView = FindViewById<GridView> (Android.Resource.Id.List);
			mGridView.Adapter = new SampleAdapter (this);
			mGridView.OnItemClickListener = this;
		}

		public void OnItemClick (AdapterView container, View view, int position, long id)
		{
			StartActivity (mSamples [position].intent);
		}
	}

	internal class SampleAdapter : BaseAdapter
	{
		private MainActivity owner;

		public SampleAdapter (MainActivity owner) : base ()
		{
			this.owner = owner;
		}

		public override int Count {
			get {
				return owner.mSamples.Length;
			}
		}


		public override Java.Lang.Object GetItem (int position)
		{
			return owner.mSamples [position];
		}

		public override long GetItemId (int position)
		{
			return (long)owner.mSamples [position].GetHashCode ();
		}

		public override View GetView (int position, View convertView, ViewGroup container)
		{
			if (convertView == null) {
				convertView = owner.LayoutInflater.Inflate (Resource.Layout.sample_dashboard_item, container, false);
			}
			convertView.FindViewById<TextView> (Android.Resource.Id.Text1).SetText (owner.mSamples [position].titleResId);
			convertView.FindViewById<TextView> (Android.Resource.Id.Text2).SetText (owner.mSamples [position].descriptionResId);
			return convertView;
		}
	}

	internal class Sample : Java.Lang.Object
	{
		internal int titleResId;
		internal int descriptionResId;
		internal Intent intent;

		public Sample (int titleResId, int descriptionResId, Intent intent)
		{
			Initialize (titleResId, descriptionResId, intent);
		}

		public Sample (int titleResId, int descriptionResId, Context c, Type t)
		{
			Initialize (titleResId, descriptionResId, new Intent (c, t));
		}

		private void Initialize (int titleResId, int descriptionResId, Intent intent)
		{
			this.intent = intent;
			this.titleResId = titleResId;
			this.descriptionResId = descriptionResId;
		}
	}*/


    public class MainActivity : Activity
    {
        EditText etUsername;
        EditText etPassword;
        Button btnLogin;
        Button btnRegister;
        ISharedPreferences prefs;
        ISharedPreferencesEditor prefEditor;
        ProgressDialog _progressDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Console.Write(Resource.String.gcm_defaultSenderId);
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            string token = prefs.GetString(CONSTANTS.AUTH_HEADER, "");
            if (!token.Equals(""))
            {
                Intent intent = new Intent(this, typeof(NavigationDrawerActivity));
                intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                StartActivity(intent);
            }
            SetContentView(Resource.Layout.activity_main);
            Init();
        }

        protected override void OnResume()
        {
            base.OnResume();
            
        }

        private void Init()
        {
            etUsername = FindViewById<EditText>(Resource.Id.tvUsername);
            etPassword = FindViewById<EditText>(Resource.Id.tvPassword);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            
            
            btnLogin.Click += OnLoginClicked;
            prefEditor = prefs.Edit();
        }

        
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (!IsInValidInput())
            {
                ConnectivityManager service = (ConnectivityManager)GetSystemService(ConnectivityService);
                NetworkInfo info = service.ActiveNetworkInfo;
                if (info != null)
                {
                    HttpClient client = new HttpClient();
                    var loginData = new LoginModel
                    {
                        UserName = etUsername.Text,
                        Password = etPassword.Text,
                        GrantType = "password"
                    };

                    try
                    {
                        HttpResponseMessage result = await client.PostAsync(CONSTANTS.LOGIN_URL, new FormUrlEncodedContent(loginData.ToDict()));

                        if (result.IsSuccessStatusCode)
                        {
                            string jsonResult = await result.Content.ReadAsStringAsync();
                            // TokenResult is a custom model class for deserialization of the Token Endpoint

                            var resultObject = JsonConvert.DeserializeObject<TokenModel>(jsonResult);


                            client.DefaultRequestHeaders.Add("Authorization", "Bearer" + resultObject.Access_Token);
                            var profile = await client.GetAsync(String.Format(CONSTANTS.GET_USERINFO_URL, resultObject.Access_Token));

                            var jsonProfile = await profile.Content.ReadAsStringAsync();

                            var user = JsonConvert.DeserializeObject<UserInfoModel>(jsonProfile);

                            user.DeviceId = prefs.GetString(CONSTANTS.DEVICEID, "");


                            var model = new DeviceIdModel
                            {
                                UserId = user.Id,
                                DeviceId = user.DeviceId
                            };


                            await client.PostAsync(CONSTANTS.POST_DEVICEID_URL, new FormUrlEncodedContent(model.ToDict()));

                            prefEditor.PutString(CONSTANTS.AUTH_HEADER, resultObject.Access_Token);
                            prefEditor.PutString(CONSTANTS.USERID, user.Id);
                            prefEditor.PutString(CONSTANTS.USERNAME, user.Username);
                            prefEditor.PutString(CONSTANTS.EMAIL, user.Email);
                            prefEditor.PutInt(CONSTANTS.REGIONID, user.RegionId);
                            prefEditor.PutString(CONSTANTS.DEVICEID, user.DeviceId);
                            prefEditor.PutString(CONSTANTS.FULLNAME, user.Fullname);
                            prefEditor.Apply();

                            //move to discount page
                            Intent intent = new Intent(this, typeof(NavigationDrawerActivity));
                            intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                            StartActivity(intent);

                        }
                        else
                        {
                            Toast.MakeText(this, result.ReasonPhrase, ToastLength.Short).Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        string debugBreak = ex.ToString();
                        Toast.MakeText(this, debugBreak, ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, "Invalid inputs", ToastLength.Short).Show();
                }
            }
            else
            {
                //Snackbar snackBar = Snackbar.Make((Button)sender, "No nternet Connection", Snackbar.LengthIndefinite);
                //Show the snackbar
               // snackBar.Show();
            }
        }

        private bool IsInValidInput()
        {
            bool isInvalid = false;


            if (etPassword.Text == null)
            {
                isInvalid = true;
                etPassword.Error = "Password cannot be empty";
            }
            if (etUsername.Text == null)
            {
                isInvalid = true;
                etUsername.Error = "User name cannot be empty";
            }

            return isInvalid;

        }
        private void ShowProgress(string message)
        {
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetMessage(message);
            _progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progressDialog.SetCancelable(false);
            _progressDialog.Show();
        }

        private void EndProgress()
        {
            _progressDialog.Dismiss();
        }
    }
}

