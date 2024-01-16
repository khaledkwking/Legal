using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Infrastructure.DAL.ViewModels
{
   public class searchkeys
    {
        public long refRecordId { get; set; }
        public string key { get; set; }
        public string value { get; set; }

    }
}
