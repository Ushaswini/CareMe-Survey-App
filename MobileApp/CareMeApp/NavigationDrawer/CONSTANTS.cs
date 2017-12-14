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

namespace edu.uncc.homework4
{
    class CONSTANTS
    {
        public const string BASE_URL = "http://careme-surveypart2.azurewebsites.net";
        public const string LOGIN_URL = BASE_URL + "/oauth2/token";
        public const string GET_USERINFO_URL = BASE_URL + "/api/Account/UserInfo?token={0}";
        public const string GET_SURVEYS_URL = BASE_URL + "/api/Surveys/GetSurvey?userId={0}";
        public const string POST_DEVICEID_URL = BASE_URL + "/api/Users/UpdateDeviceId";
        public const string POST_RESPONSE_URL = BASE_URL + "/api/SurveyResponses";
        public const string LOGOUT_URL = BASE_URL + "/api/Account/Logout";


        public const string AUTH_HEADER = "token";
        public const string USERNAME = "username";
        public const string FULLNAME = "fullname";
        public const string USERID = "userid";
        public const string DEVICEID = "deviceid";
        public const string EMAIL = "email";
        public const string REGIONID = "regionid";


        public const string SENT_TOKEN_TO_SERVER = "sentTokenToServer";
        public const string REGISTRATION_COMPLETE = "registrationComplete";
        public const string RECEIVE_NOTIFICATION_BOOLEAN = "receive";
        public const string DATE_FORMAT = "0:MM/dd/yy H:mm:ss zzz";

    }
}