using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure.DAL.ViewModels
{
    public partial class CasesModel
    {

        public string StatusName { get; set; }
        public DateTime TransDate { get; set; }
        public int CasesCount { get; set; }
        public int degreecode { get; set; }
        public string partiesName { get; set; }
        public string DegreeDesc { get; set; }
  public int TransYear { get; set; }
    }
}