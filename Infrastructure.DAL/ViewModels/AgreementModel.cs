using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure.DAL.ViewModels
{
    public partial  class AgreementModel
    {

        public string StatusName { get; set; }

        public DateTime TransDate { get; set; }
        public int AgreementCount { get; set; }
        public int statuscode { get; set; }

        public Boolean isInitial { get; set; }
        public int hasparent { get; set; }
      


    }
}