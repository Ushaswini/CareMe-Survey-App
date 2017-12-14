using Homework05.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Homework05.Models
{
    public class L_QuestionType
    {
        public int Id { get; set; }
        public string QuestionType { get; set; }
    }

    public enum QuestionType
    {
        TextEntry,
        Choice,
        Scale,
        Info,
        Reminder
    }

    public enum Frequency
    {
        Daily,
        Hourly,
        TwiceDaily
    }

    public enum SurveyType
    {
        Survey,
        Message
    }

    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Options { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double StepSize { get; set; }

    }

    public class Survey
    {
        public int Id { get; set; }
        public string SurveyName { get; set; }
        public SurveyType SurveyType { get; set; }

    }

    public class X_User_Group
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int StudyGroupId { get; set; }
        //Navigation properties
        public ApplicationUser User { get; set; }
        public StudyGroup StudyGroup { get; set; }
    }

    public class X_Coordinator_Group
    {
        public int Id { get; set; }
        public string CoordinatorId { get; set; }
        public int StudyGroupId { get; set; }
        //Navigation properties
        public ApplicationUser Coordinator { get; set; }
        public StudyGroup StudyGroup { get; set; }
    }

    public class StudyGroup
    {
        public int Id { get; set; }        
        public string StudyGroupName { get; set; }
        public string StudyGroupCreatedTime { get; set; }       
    }
   
    public class X_Survey_Question
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }

        //Navigation properties
        public Survey Survey { get; set; }
        public Question Question { get; set; }
    }

    public class X_Survey_Group
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int StudyGroupId { get; set; } 
        public string SurveyCreatedTime { get; set; }

        //If survey type is message;these are valid
        [EnumDataType(typeof(Frequency))]
        public Frequency FrequencyOfNotifications { get; set; }
        public string Time1 { get; set; }
        public string Time2 { get; set; }


        //Navigation properties
        public Survey Survey { get; set; }
        public StudyGroup StudyGroup { get; set; }
    }

    public class X_Survey_Question_Response
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }
        public int ResponseId { get; set; }
        //Navigation Properties
        public Question Question { get; set; }
        public SurveyResponse SurveyResponse { get; set; }
    }
  



    public class SurveyResponse
    {
        [Key]
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }        
        public string ResponseText { get; set; }
        public string ResponseReceivedTime { get; set; }

        //Navigation Properties
        public Survey Survey { get; set; }
        public Question Question { get; set; }
        public ApplicationUser User { get; set; }

    }


    public class SurveyPushNotification
    {
        [JsonProperty(propertyName: "registration_ids")]
        public List<string> RegisteredDeviceIds { get; set; }
        [JsonProperty(propertyName: "data")]
        public PushNotificationData Data { get; set; }
    }

    public class PushNotificationData
    {
        [JsonProperty(propertyName:"message")]
        public string Message { get; set; }
        [JsonProperty(propertyName: "time")]
        public string Time { get; set; }
    }

    public class SurveysForUser
    {
        public List<ResponseDTO> SurveysResponded { get; set; }
        public List<SurveyDTO> Surveys { get; set; }
    }

    public class Device
    {
        public string UserId { get; set; }
        [Key]
        public int Id { get; set; }
        public string DeviceId { get; set; }
    }
}