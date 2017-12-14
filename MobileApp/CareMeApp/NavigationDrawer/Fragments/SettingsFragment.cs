using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

using Android.Content;
using System;
using Android.Gms.Iid;
using Android.Preferences;

//Ambiguities
using Fragment = Android.App.Fragment;
using NavigationDrawer;

namespace edu.uncc.homework4
{
    internal class SettingsFragment : Fragment
    {
        Switch switchOnOff;
        ISharedPreferences prefs;

        public SettingsFragment()
        {
            // Empty constructor required for fragment subclasses
        }

        public static Fragment NewInstance()
        {
            Fragment fragment = new SettingsFragment();            
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
                                           Bundle savedInstanceState)
        {
            prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            View rootView = inflater.Inflate(Resource.Layout.fragment_settings, container, false);          
            
            return rootView;
        }

        private void OnCheckedChanged(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            InstanceID id = InstanceID.GetInstance(Context);
            if (e.IsChecked)
            {
                Intent intent = new Intent(Activity, typeof(RegistrationIntentService));
                Context.StartService(intent);
            }
            else
            {
                try
                {                   
                    id.DeleteInstanceID();
                    
                }catch(Exception exp)
                {
                    Console.Write(exp.Message);
                }
              
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            switchOnOff = Activity.FindViewById<Switch>(Resource.Id.switchOnOff);
            switchOnOff.Checked = prefs.GetBoolean(CONSTANTS.RECEIVE_NOTIFICATION_BOOLEAN, false);
            switchOnOff.CheckedChange += OnCheckedChanged;
        }
    }

}