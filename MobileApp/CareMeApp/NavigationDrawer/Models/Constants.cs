namespace edu.uncc.homework4
{
    public static class Constants
    {
        public static readonly string UUID = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";
        public static readonly string DISCOUNT_REGION_IDENTIFIER = "Discount_Regions";
        public static readonly string TAG = "demo";

        public static readonly string GROCERY_IDENTIFIER = "Grocery";
        public static readonly int MAJOR_GROCERY = 15212;
        public static readonly int MINOR_GROCERY = 31506;

        public static readonly string LIFESTYLE_IDENTIFIER = "Lifestyle";
        public static readonly int MAJOR_LIFESTYLE = 48071;
        public static readonly int MINOR_LIFESTYLE = 25324;

        public static readonly string PRODUCE_IDENTIFIER = "Produce";
        public static readonly int MAJOR_PRODUCE = 26535;
        public static readonly int MINOR_PRODUCE = 44799;

       
        public static readonly string BASE_URL = "http://discountnotifier1.azurewebsites.net";
        public static readonly string ALL_URL = BASE_URL + "/api/Discounts";
        public static readonly string REGION_URL = BASE_URL + "/api/Discounts?regionName={0}";
        public static readonly string LOGIN_URL = BASE_URL + "/oauth2/token";

        public static readonly string GETUSERINFO_URL = BASE_URL + "/api/Account/UserInfo?token={0}";

        public static readonly string AUTH_HEADER = "token";
        public static readonly string USERNAME = "username";
        public static readonly string FULLNAME = "fullname";
        public static readonly string USERID = "userid";
        public static readonly string DEVICEID = "deviceid";
        public static readonly string EMAIL = "email";
        public static readonly string REGIONID = "regionid";

        public static readonly string REGISTERUSER_URL = BASE_URL + "/api/Account/Register";
        public static readonly string LOGOUTUSER_URL = BASE_URL + "/api/Account/Logout";
        public static readonly string UPDATE_DEVICEID_URL = BASE_URL + "/api/Users/UpdateDeviceId";
        public static readonly string GETREGIONS_URL = BASE_URL + "/api/Regions";
        public static readonly string UPDATE_REGIONID_URL = BASE_URL + "/api/Users/UpdateRegionId";

    }
}