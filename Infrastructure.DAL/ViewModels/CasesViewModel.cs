using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure.DAL.ViewModels
{
    public partial class CasesViewModel
    {

        public int Code { get; set; }
        public string file_Serial { get; set; }
        public string FileNote { get; set; }
        public int? FileInternalSerial { get; set; }
        public int? fileSerialyear { get; set; }
        public string FileAutoNumber { get; set; }
        public DateTime? TransDate { get; set; }
        public DateTime? LastTransctionDate { get; set; }
        public bool isPrivate { get; set; }
    }
}