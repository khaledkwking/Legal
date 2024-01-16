using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure.DAL.ViewModels
{
    public partial class LawsModel
    {
  public int DocTypeID { get; set; }
        public string StatusName { get; set; }
        public DateTime TransDate { get; set; }
        public int DocCount { get; set; }
      public int TransYear { get; set; }
        public string Law_Doctypenamear { get; set; }
        public int Code { get; set; }

    }
}