using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure.DAL.ViewModels
{
    public partial class LegalMemoModel
    {

        public int RecordCount { get; set; }
        public int DocYear { get; set; }
        public string DocStatus { get; set; }
    }
}