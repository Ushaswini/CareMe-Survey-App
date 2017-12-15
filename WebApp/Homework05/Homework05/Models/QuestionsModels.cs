using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework05.Models
{

    public class QuestionViewModel
    {
        public bool Active { get; set; }
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public string Options { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int StepSize { get; set; }

        private string range;
        public string Range
        {
            get
            {
                return Minimum + ":" + Maximum + ":" + StepSize;
            }
            set
            {

                this.range = value;
            }
        }

    }

}