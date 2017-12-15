using Homework05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework05.DTOs
{
    public class ResponseDTO
    {
        //Unique identifiers 
       // public int ResponseId { get; set; }
        public int SurveyId { get; set; }        
        public string UserName { get; set; }
        public string StudyGroupName { get; set; }
        public string SurveyName { get; set; }
        public int StudyGroupId { get; set; }

        public string StudyCoordinatorName { get; set; }

        public SurveyType SurveyType { get; set; }

        public List<QuestionResponseDTO> QuestionResponses { get; set; }

       

    }

    public class QuestionResponseDTO
    {
        //Input properties
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Options { get; set; }
        public string QuestionFrequency { get; set; }

        //Response properties
        public string ResponseText { get; set; }

        public string ResponseReceivedTime { get; set; }
    }
}