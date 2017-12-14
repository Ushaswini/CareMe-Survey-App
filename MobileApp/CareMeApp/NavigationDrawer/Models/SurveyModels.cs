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

namespace NavigationDrawer.Models
{
    

    public class SurveysForUser
    {
        public List<Message> SurveysResponded { get; set; }
        public List<Message> Surveys { get; set; }
    }

    public class Message 
    {
        //Unique identifiers 
        public string ResponseId { get; set; }
        public string SurveyId { get; set; }
        public string QuestionId { get; set; }
        public string UserName { get; set; }
        public string StudyGroupName { get; set; }

        //Input properties
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Options { get; set; }
        public string QuestionFrequency { get; set; }

        //Response properties
        public string ResponseText { get; set; }
        public string ResponseReceivedTime { get; set; }       

        public string StudyGroupId { get; set; }
        public string SurveyCreatedTime { get; set; }

        public string Time1 { get; set; }
        public string Time2 { get; set; }

        public string TimeToDisplay
        {
            get
            {
                if(this.ResponseId != null)
                {
                    return ResponseReceivedTime;
                }
                else
                {
                    return SurveyCreatedTime;
                }
            }
        }
    }

    public enum QuestionType
    {
        TextEntry,
        Choice,
        Message
    }

    public class SurveyResponse
    {
        public string SurveyId { get; set; }
        public string UserId { get; set; }
        public string SurveyResponseId { get; set; }
        public string StudyGroupId { get; set; }
        public string UserResponseText { get; set; }
        public string SurveyResponseReceivedTime { get; set; }

        public Dictionary<string, string> ToDict()
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("SurveyId", this.SurveyId);
            parameters.Add("UserId", this.UserId);            
            parameters.Add("StudyGroupId", this.StudyGroupId);
            parameters.Add("UserResponseText", this.UserResponseText);
            parameters.Add("SurveyResponseReceivedTime", this.SurveyResponseReceivedTime);
            return parameters;
        }


    }

    public class Response
    {
        //Unique identifiers 
        public string ResponseId { get; set; }

        public string SurveyId { get; set; }

        public string QuestionId { get; set; }


        public string UserName { get; set; }
        public string StudyGroupName { get; set; }

        //Input properties
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Options { get; set; }
        public string QuestionFrequency { get; set; }

        //Response properties
        public string ResponseText { get; set; }

        public string ResponseReceivedTime { get; set; }

    }

    public class Survey
    {
        public string SurveyId { get; set; }
        
        public string StudyGroupId { get; set; }
        public string SurveyCreatedTime { get; set; }
        public string StudyGroupName { get; set; }


        public string QuestionId { get; set; }
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Options { get; set; }

        public string QuestionFrequency { get; set; }
        public string Time1 { get; set; }
        public string Time2 { get; set; }
    }
}