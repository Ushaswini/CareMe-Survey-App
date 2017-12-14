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
using Fragment = Android.App.Fragment;
using Android.Preferences;
using NavigationDrawer;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Support.V7.Widget;
using NavigationDrawer.Models;
using Message = NavigationDrawer.Models.Message;
using Newtonsoft.Json;

namespace edu.uncc.homework4
{
    internal class DashboardFragment : Fragment, SurveysAdapter.OnSendClickListener
    {
        private RecyclerView mSurveysRv;
        private ISharedPreferences prefs;
        private SurveysAdapter mAdapter;
        private List<Message> mData;
        private LinearLayoutManager layoutManager;

        public DashboardFragment()
        {
            // Empty constructor required for fragment subclasses
        }

        public static Fragment NewInstance()
        {
            Fragment fragment = new DashboardFragment();
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
                                           Bundle savedInstanceState)
        {
            prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
            View rootView = inflater.Inflate(Resource.Layout.fragment_dashboard, container, false);
            return rootView;
        }


        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            mData = new List<Message>();
            mSurveysRv = Activity.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            mAdapter = new SurveysAdapter(mData, Context, this);
            layoutManager = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);
            mSurveysRv.SetLayoutManager(layoutManager);
            mSurveysRv.SetAdapter(mAdapter);
            GetMessagesAsync();

        }


        public async Task GetMessagesAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    string id = prefs.GetString(CONSTANTS.USERID, "");
                    string url = String.Format(CONSTANTS.GET_SURVEYS_URL, prefs.GetString(CONSTANTS.USERID, ""));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + prefs.GetString(CONSTANTS.AUTH_HEADER, ""));
                    var data = await client.GetAsync(String.Format(CONSTANTS.GET_SURVEYS_URL, prefs.GetString(CONSTANTS.USERID, "")));
                    var jsonData = await data.Content.ReadAsStringAsync();
                    var surveys = JsonConvert.DeserializeObject<SurveysForUser>(jsonData);
                    List<Message> msgs = new List<Message>();
                    msgs.AddRange(surveys.Surveys);
                    msgs.AddRange(surveys.SurveysResponded);

                    Activity.RunOnUiThread(() =>
                    {
                        mAdapter.UpdateData(msgs);
                    });

                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }

            }
        }

        void SurveysAdapter.OnSendClickListener.OnResponseSendClicked(int position, int checkedId, string reply)
        {
            Console.Write("in clicked" + position);
            var data = mData[position];
            string replyText = "";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + prefs.GetString(CONSTANTS.AUTH_HEADER, ""));
            switch (data.QuestionType)
            {
                case QuestionType.Choice:
                    {
                        if (checkedId == Resource.Id.radioButtonYes) replyText = "Yes";
                        else replyText = "No";

                        break;
                    }
                case QuestionType.TextEntry:
                    {
                        replyText = reply;
                        break;
                    }
            }
            var responseToSend = new SurveyResponse
            {
                UserId = prefs.GetString(CONSTANTS.USERID, ""),
                SurveyId = data.SurveyId,
                StudyGroupId = data.StudyGroupId,
                UserResponseText = replyText,
                SurveyResponseReceivedTime = DateTime.Now.ToString(CONSTANTS.DATE_FORMAT)
            };
            client.PostAsync(CONSTANTS.POST_RESPONSE_URL, new FormUrlEncodedContent(responseToSend.ToDict()))
                            .ContinueWith((response) =>
                            {
                                Console.Write(response.Status);
                                if (response.IsCompleted)
                                {
                                    GetMessagesAsync();
                                }
                                else
                                {
                                    Console.Write(response.Result);
                                }
                            });




        }
    }
}