using Homework05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework05.DTOs
{
    public class SurveyDTO
    {
        public int SurveyId { get; set; }        
        public string SurveyName { get; set; }
        public SurveyType SurveyType { get; set; }
        public List<QuestionDTO> Questions { get; set; }

        public int StudyGroupId { get; set; }
        public string StudyGroupName { get; set; }
        public string StudyCoordinatorId { get; set; }
        public string StudyCoordinatorName { get; set; }
    }

    public class QuestionDTO
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Options { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double StepSize { get; set; }
    }

    public class StudyGroupDTO
    {
        public int Id { get; set; }
        public string CoordinatorId { get; set; }
        public string StudyGroupName { get; set; }
        public string CreatedTime { get; set; }
    }
}