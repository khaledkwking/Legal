using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure.DAL.ViewModels
{
    public partial  class QuestionsModel
    {

        public string StatusName { get; set; }
        public DateTime TransDate { get; set; }
        public int QuestionCount { get; set; }
        public int statuscode { get; set; }
         public string ChapterName { get; set; }
        public int Chaptercode { get; set; }

    }
}