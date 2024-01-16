using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using System.Web;

using System.IO;


namespace Utilities
{

    public class KeyListItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class NextGlobal
    {
        public static DateTime? GetDateFromtimestamp(double timestamp)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            date = date.AddSeconds(timestamp);
            string strShortDate = date.ToShortDateString();
            DateTime dateShortDate = Convert.ToDateTime(strShortDate);
            return dateShortDate;
        }

        public enum ExchangeType
        {
            MS,
            PC
        }
        public static DateTime STARTSNAPSHOTDATE = DateTime.Parse("01/01/2009");
    }

    public enum PermissionObjectType
    {
        Page,
        Object
    }

    public enum enumTxType
    {
        STM1,
        E3,
        T3,
        E1,
        Ethernet,
        Fastethernet,
        GBEthernet,
        GBEthernet10,
        STM4,
        DSL_Ports,
        Extender,
        DF,
        NO_TX,
        STM16
    }

}