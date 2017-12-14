using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;

//Ambiguities
using Fragment = Android.App.Fragment;
using edu.uncc.homework4;
using Android.Preferences;
using System.Threading.Tasks;
using System.Net.Http;
using edu.uncc.homework4.Models;

namespace NavigationDrawer
{
	[Activity (Label = "@string/app_name", Icon = "@drawable/ic_launcher")]
	public class NavigationDrawerActivity : Activity, OptionsAdapter.OnItemClickListener
	{
		private DrawerLayout mDrawerLayout;
		private RecyclerView mDrawerList;
		private ActionBarDrawerToggle mDrawerToggle;

		private string mDrawerTitle;
		private String[] mOptionsTitles;

        ISharedPreferences prefs;
        ISharedPreferencesEditor prefEditor;
        Fragment fragment;

		protected override void OnCreate (Bundle savedInstanceState)
		{

			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.activity_navigation_drawer);

            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            prefEditor = prefs.Edit();

            mDrawerTitle = this.Title;
			mOptionsTitles = this.Resources.GetStringArray (Resource.Array.drawer_items_array);
			mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mDrawerList = FindViewById<RecyclerView> (Resource.Id.left_drawer);

			// set a custom shadow that overlays the main content when the drawer opens
			mDrawerLayout.SetDrawerShadow (Resource.Drawable.drawer_shadow, GravityCompat.Start);
			// improve performance by indicating the list if fixed size.
			mDrawerList.HasFixedSize = true;
			mDrawerList.SetLayoutManager (new LinearLayoutManager (this));

			// set up the drawer's list view with items and click listener
			mDrawerList.SetAdapter (new OptionsAdapter (mOptionsTitles, this));
			// enable ActionBar app icon to behave as action to toggle nav drawer
			this.ActionBar.SetDisplayHomeAsUpEnabled (true);
			this.ActionBar.SetHomeButtonEnabled (true);

			// ActionBarDrawerToggle ties together the the proper interactions
			// between the sliding drawer and the action bar app icon

			mDrawerToggle = new MyActionBarDrawerToggle (this, mDrawerLayout,
				Resource.Drawable.Drawer, 
				Resource.String.drawer_open, 
				Resource.String.drawer_close);

			mDrawerLayout.SetDrawerListener (mDrawerToggle);
			if (savedInstanceState == null) //first launch
				selectItem (0);

            if (!prefs.GetBoolean(CONSTANTS.RECEIVE_NOTIFICATION_BOOLEAN, false))
            {
                ShowDialog("Allow Care-Me to send notifications. You can always change this settings in profile.");
            }

					
		}
        private void ShowDialog(string title)
        {

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Alert!");
            builder.SetMessage(title);

            builder.SetPositiveButton("Yes", (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(RegistrationIntentService));
                StartService(intent);
            });
            builder.SetNegativeButton("No", (sender, e) => { });           

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }
        internal class MyActionBarDrawerToggle : ActionBarDrawerToggle
		{
			NavigationDrawerActivity owner;

			public MyActionBarDrawerToggle (NavigationDrawerActivity activity, DrawerLayout layout, int imgRes, int openRes, int closeRes)
				: base (activity, layout, imgRes, openRes, closeRes)
			{
				owner = activity;
			}

			public override void OnDrawerClosed (View drawerView)
			{
				owner.ActionBar.Title = owner.Title;
				owner.InvalidateOptionsMenu ();
			}

			public override void OnDrawerOpened (View drawerView)
			{
				owner.ActionBar.Title = owner.mDrawerTitle;
				owner.InvalidateOptionsMenu ();
			}
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			// Inflate the menu; this adds items to the action bar if it is present.
			this.MenuInflater.Inflate (Resource.Menu.navigation_drawer, menu);
			return true;
		}

		/* Called whenever we call invalidateOptionsMenu() */
		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			// If the nav drawer is open, hide action items related to the content view
			bool drawerOpen = mDrawerLayout.IsDrawerOpen (mDrawerList);
			menu.FindItem (Resource.Id.action_refresh).SetVisible (!drawerOpen);
			return base.OnPrepareOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			// The action bar home/up action should open or close the drawer.
			// ActionBarDrawerToggle will take care of this.
			if (mDrawerToggle.OnOptionsItemSelected (item)) {
				return true;
			}
            switch (item.ItemId)
            {
                case Resource.Id.action_refresh:
                    ((DashboardFragment)fragment).GetMessagesAsync();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }

            
			
		}

		/* The click listener for RecyclerView in the navigation drawer */
		public void OnClick (View view, int position)
		{
			selectItem (position);
		}

		private void selectItem (int position)
		{
            switch (position)
            {
                case 0:
                    {
                        //Dashboard
                        fragment = DashboardFragment.NewInstance();
                        var fragmentManager = this.FragmentManager;
                        var ft = FragmentManager.BeginTransaction();
                        ft.Replace(Resource.Id.content_frame, fragment);
                        ft.Commit();
                        break;
                    }
                case 1:
                    {
                        //Settings
                        var fragment = SettingsFragment.NewInstance();
                        var fragmentManager = this.FragmentManager;
                        var ft = FragmentManager.BeginTransaction();
                        ft.Replace(Resource.Id.content_frame, fragment);
                        ft.Commit();
                        break;
                    }
                    
                case 2:
                    //Logout
                    LogoutAsync();
                    break;
            }
			
			// update selected item title, then close the drawer
			Title = mOptionsTitles [position];
			mDrawerLayout.CloseDrawer (mDrawerList);
		}

        private async Task LogoutAsync()
        {
            try
            {
                using(var client = new HttpClient()){

                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + prefs.GetString(CONSTANTS.AUTH_HEADER, ""));

                    var model = new DeviceIdModel
                    {
                        UserId = prefs.GetString(CONSTANTS.USERID, ""),
                        DeviceId = ""
                    };
                    //TODO:delete from device id table
                    await client.PostAsync(CONSTANTS.POST_DEVICEID_URL, new FormUrlEncodedContent(model.ToDict()));
                    var result = await client.PostAsync(Constants.LOGOUTUSER_URL, null);

                    if (result.IsSuccessStatusCode)
                    {
                        prefEditor.Remove(CONSTANTS.AUTH_HEADER);
                        prefEditor.Remove(CONSTANTS.DEVICEID);
                        prefEditor.Remove(CONSTANTS.USERID);
                        prefEditor.Remove(CONSTANTS.USERNAME);
                        prefEditor.Remove(CONSTANTS.EMAIL);
                        prefEditor.Remove(CONSTANTS.FULLNAME);
                        prefEditor.Remove(CONSTANTS.RECEIVE_NOTIFICATION_BOOLEAN);


                        prefEditor.Apply();

                        Intent intent = new Intent(this, typeof(MainActivity));
                        intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                        StartActivity(intent);
                    }
                    else
                    {
                        Toast.MakeText(this, "Logout unsuccessful", ToastLength.Short).Show();
                    }
                }
            }
            catch (Exception exp)
            {
                Toast.MakeText(this, exp.Message, ToastLength.Short).Show();
            }
        }

        protected override void OnTitleChanged (Java.Lang.ICharSequence title, Android.Graphics.Color color)
		{
			//base.OnTitleChanged (title, color);
			this.ActionBar.Title = title.ToString ();
		}

		/**
	     * When using the ActionBarDrawerToggle, you must call it during
	     * onPostCreate() and onConfigurationChanged()...
	     */

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			// Sync the toggle state after onRestoreInstanceState has occurred.
			mDrawerToggle.SyncState ();
		}

		public override void OnConfigurationChanged (Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			// Pass any configuration change to the drawer toggls
			mDrawerToggle.OnConfigurationChanged (newConfig);
		}

		/**
	     * Fragment that appears in the "content_frame", shows a planet
	     */
		
	
	}
}


