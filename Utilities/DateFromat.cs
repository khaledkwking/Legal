using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Utilities
{
  public   class DateFromat
    {

        #region "Date Format "
        private const int startGreg = 1900;
        private const int endGreg = 2100;
        private string[] allFormats = { "yyyy/MM/dd", "yyyy/M/d", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yyyy", "d-M-yyyy",
    "dd-M-yyyy", "d-MM-yyyy", "yyyy MM dd", "yyyy M d", "dd MM yyyy", "d M yyyy", "dd M yyyy", "d MM yyyy", "MM-dd-yyyy", "MM/dd/yyyy"
     };
        private CultureInfo arCul = new CultureInfo("ar-SA");
        private static CultureInfo enCul = new CultureInfo("en-US");

        private bool ISDateORNull(object Value)
        {

            if (!string.IsNullOrEmpty(Value.ToString().Trim()) && ((!object.ReferenceEquals(Value, System.DBNull.Value))))
            {
                try
                {
                    // DateTime.ParseExact(Value.ToString().Trim(), allFormats, enCul.DateTimeFormat,DateTimeStyles.AllowWhiteSpaces);
                    DateTime.ParseExact(Value.ToString(), allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                }
                catch (System.FormatException)
                {
                    try
                    {
                        DateTime.ParseExact(DateTime.Parse(Value.ToString()).ToString("dd-MM-yyyy", enCul), allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                    }
                    catch (System.FormatException)
                    {
                        return false;
                    }
                    catch (System.Exception)
                    {
                        return false;
                    }
                }
                catch (System.Exception)
                {
                    return false;
                }
            }


            return true;
        }

        //return DMY format to match with sql
        public static string AdjustDateToSQL(object value)
        {
            try
            {
                if (ISNull(value))
                {
                    return "NULL";
                }
                if (DateTime.Parse(value.ToString())  < DateTime.Parse("01/01/1900"))
                {
                    return DateTime.Parse("01/01/1900").ToString("dd/MM/yyyy", enCul.DateTimeFormat);
                }
                return DateTime.Parse(value.ToString()).ToString("dd/MM/yyyy", enCul.DateTimeFormat);
            }
            catch (System.Exception)
            {
                return value.ToString();
            }
        }
        //return DMY format to match with sql
        //public static string AdjustDateToUser(object value)
        //{
        //    try
        //    {
        //        if (ISNull(value))
        //        {
        //            return "NULL";
        //        }
        //        if (DateTime.Parse(value.ToString()) < DateTime.Parse("01/01/1900"))
        //        {
        //            return DateTime.Parse("01/01/1900").ToString("dd/MM/yyyy", enCul.DateTimeFormat);
        //        }
        //        return DateTime.Parse(value.ToString()).ToString("dd/MM/yyyy", enCul.DateTimeFormat);
        //    }
        //    catch (System.Exception generatedExceptionName)
        //    {
        //        return value.ToString();
        //    }
        //}
        //return date formate accorrding to user calendar
        //Private Function AdjustDateToUser(ByVal value As Object) As String
        //    Try
        //        value = value.ToString().Trim()
        //        Dim ResultDate As String
        //        Dim tempDate As DateTime
        //        Try
        //            tempDate = DateTime.ParseExact(value.ToString(), allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces)
        //        Catch generatedExceptionName As FormatException

        //            tempDate = DateTime.ParseExact(DateTime.Parse(value.ToString()).ToString("dd/MM/yyyy", enCul), allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces)
        //        End Try
        //        If SAConfigs.Calendar = CInt(Calendars.Hijri) Then
        //            If tempDate.Year >= startGreg AndAlso tempDate.Year <= endGreg Then
        //                ResultDate = tempDate.ToString("dd/MM/yyyy", arCul.DateTimeFormat)
        //            Else
        //                'return Hijri from Greg
        //                ResultDate = tempDate.ToShortDateString()
        //                'return Hijri from Hijri
        //            End If
        //        Else
        //            If tempDate.Year >= startGreg AndAlso tempDate.Year <= endGreg Then
        //                ResultDate = tempDate.ToShortDateString()
        //            Else
        //                ' return Greg  from Greg
        //                Try


        //                    tempDate = DateTime.ParseExact(value.ToString(), allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces)
        //                Catch generatedExceptionName As FormatException
        //                    tempDate = DateTime.ParseExact(DateTime.Parse(value.ToString()).ToString("dd/MM/yyyy", arCul), allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces)
        //                End Try
        //                'return Greg  from Hijri
        //                ResultDate = tempDate.ToString("dd/MM/yyyy", enCul.DateTimeFormat)
        //            End If
        //        End If

        //        Return ResultDate
        //    Catch generatedExceptionName As System.ArgumentException
        //        Return value.ToString()
        //    Catch generatedExceptionName As System.Exception
        //        Return value.ToString()
        //    End Try
        //End Function

        //return date formate Greg to use at application
        private string AdjustDateToApplication(object value)
        {
            try
            {
                if (ISNull(value))
                {
                    return value.ToString();
                }
                DateTime tempDate = default(DateTime);
                value = value.ToString().Trim();
                string ResultDate = null;
                try
                {

                    tempDate = DateTime.ParseExact(value.ToString(), allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                }
                catch (FormatException)
                {
                    tempDate = DateTime.ParseExact(DateTime.Parse(value.ToString()).ToString("dd/MM/yyyy", enCul), allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                }
                if (tempDate.Year >= startGreg && tempDate.Year <= endGreg)
                {
                    // return Greg  from Greg
                    ResultDate = tempDate.ToShortDateString();
                }
                else
                {
                    try
                    {
                        tempDate = DateTime.ParseExact(value.ToString(), allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                        //return Greg  from Hijri
                        ResultDate = tempDate.ToShortDateString();
                    }
                    catch (FormatException)
                    {
                        tempDate = DateTime.ParseExact(DateTime.Parse(value.ToString()).ToString("dd/MM/yyyy", enCul), allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);

                        ResultDate = tempDate.ToShortDateString();
                    }
                }


                return ResultDate;
            }
            catch (System.FormatException)
            {
                return value.ToString();
            }
            catch (System.Exception)
            {
                return value.ToString();
            }
        }


        private static object GetStrNull(object value)
        {
            if (!ISNull(value))
            {
                return value;
            }
            else
            {
                return "NULL";
            }
        }
        private static bool ISNull(object value)
        {
            if (value == null || (object.ReferenceEquals(value, System.DBNull.Value)) || string.IsNullOrEmpty(value.ToString().Trim()) || value.ToString().Trim().Length < 1)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}