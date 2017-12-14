package edu.uncc.homework4;


/**
 * Vinnakota Venkata Ratna Ushaswini
 * Constants
 * 06/12/2017
 */

public  class Constants {

    public static final String BASE_URL = "http://careme-surveypart2.azurewebsites.net";
    public static final String LOGIN_URL = BASE_URL + "/oauth2/token";
    public static final String GET_USERINFO_URL = BASE_URL + "/api/Account/UserInfo?token";
    public static final String GET_SURVEYS_URL = BASE_URL + "/api/Surveys/GetSurvey?userId=";
    public static final String POST_DEVICEID_URL = BASE_URL + "/api/Users/UpdateDeviceId";
    public static final String POST_RESPONSE_URL = BASE_URL + "/api/SurveyResponses";
    public static final String LOGOUT_URL = BASE_URL + "/api/Account/Logout";


    public static final String AUTH_HEADER = "token";
    public static final String USERNAME = "username";
    public static final String FULLNAME = "fullname";
    public static final String USERID = "userid";
    public static final String DEVICEID = "deviceid";
    public static final String EMAIL = "email";
    public static final String REGIONID = "regionid";


    public static final String SENT_TOKEN_TO_SERVER = "sentTokenToServer";
    public static final String REGISTRATION_COMPLETE = "registrationComplete";
    public static final String RECEIVE_NOTIFICATION_BOOLEAN = "receive";
    public static final String DATE_FORMAT = "EEE MMM dd HH:mm:ss zzz yyyy";
    public  static final String PREFS = "prefs";
}

