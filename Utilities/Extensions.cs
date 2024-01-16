using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace Utilities
{


    public static class Extensions 
    {
        public static bool  ISNull(this object Value)
        {
            if (Value == null || (Value == System.DBNull.Value) ||
                string.IsNullOrEmpty(Value.ToString().Trim()) || Value.ToString().Trim().Length < 1)
            {
                return true;
            }
            return false;

        }
        public static DateTime MaxDate(this DateTime a, DateTime b)
        {
            return a > b ? a : b;
        }

        public static double Round(this double a)
        {
            return  Math.Round(a,2);
        }
    }
}
