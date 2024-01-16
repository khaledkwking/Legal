using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure.DAL.ViewModels
{
    public partial  class MedalModel
    {

        public string StatusName { get; set; }
      
        public DateTime TransDate { get; set; }
        public int MedalCount { get; set; }
        public int statuscode { get; set; }

        
    }
}