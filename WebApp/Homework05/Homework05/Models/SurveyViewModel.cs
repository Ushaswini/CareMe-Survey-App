using Homework05.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Homework05.Models
{
    public class SurveyViewModel
    {
        [Required]
        public string SurveyName { get; set; }
        public SurveyType SurveyType { get; set; }
        [Required(ErrorMessage ="You Should select atleast one question")]
        public string QuestionIds_String { get; set; }

        private List<int> questionIds;
        public List<int> QuestionIds { get {
                if (QuestionIds_String == null || QuestionIds_String.Equals(""))
                    return null;

                return QuestionIds_String.TrimEnd(',') .Split(',').Select(s => Int32.Parse(s)).ToList();
            } set {
                this.questionIds = value;
            } }        
    }

    public class PublishSurveyViewModel
    {
        public int SurveyId { get; set; }
        public int StudyGroupId { get; set; }
        public string SurveyCreatedTime { get; set; }

        //If survey type is message;these are valid
        [EnumDataType(typeof(Frequency))]
        public Frequency FrequencyOfNotifications { get; set; }
        public string Time1 { get; set; }
        public string Time2 { get; set; }
    }

    public class SurveyResponseViewModel
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }        
        public string UserId { get; set; }      

        public List<QuestionResponseDTO> Responses { get; set; }
    }

    public class StudyGroupViewModel
    {
        public int Id { get; set; }
        [Required]
        public string StudyGroupName { get; set; }
        public string StudyGroupCreadtedTime { get; set; }

        public string StudyCoordinatorId { get; set; }

        //Navigation Properties
        
        public ApplicationUser StudyCoordinator { get; set; }
    }
}