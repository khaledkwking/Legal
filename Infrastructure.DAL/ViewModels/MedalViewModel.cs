using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Infrastructure.DAL.ViewModels
{
   public class MedalViewModel
    {
        public int? Code { get; set; }
        public int? Medal_OrgID { get; set; }
        public int? LastActionID { get; set; }
        public Nullable<int> MedalCatId { get; set; }

        [DisplayName("رقم الملف")]
        public string FileNum { get; set; }
        public string OrganizationsNameAr { get; set; }
        public DateTime Medal_receivedDate { get; set; }
        public DateTime TransDate { get; set; }
    }
}
