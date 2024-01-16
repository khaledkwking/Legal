using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace Infrastructure.DAL
{
 public class SuperBusiness
    {

        public string StringLimit(string str, int limit)
        {
            if ((str.Length <= limit))
            {
                return (str + " ");
            }

            string s = str.Substring(0, limit);
            if ((s.LastIndexOf(" ") != -1))
            {
                s = s.Substring(0, s.LastIndexOf(" "));
            }

            s += " ...";
            return s;
        }

        public bool GetBool(object st)
        {
            if ((st == DBNull.Value))
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(Convert.ToInt16(st));
            }

        }

        protected string gets(object obj)
        {
            if (obj==null || obj == DBNull.Value)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }

        }
    }
}
