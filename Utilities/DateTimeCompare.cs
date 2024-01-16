using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

/// <summary>
/// Summary description for DateTimeCompare
/// </summary>
public class DateTimeCompare
{
	public DateTimeCompare()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// Returns -1 if StartDate less than EndDate, 
    /// 0 if Equal.
    /// 1 if StartDate greater than EndDate  
    public static int CompareDates(string strStartDate, string strEndDate)
    {
        try
        {
            // Creates and initializes the CultureInfo which uses the international sort.
            //I have used English (United Kingdom) cultural inforamtion to convert data into dd/MM/yyyy format 
            CultureInfo cultInfo = new CultureInfo("en-GB", true);
            DateTimeFormatInfo formatInfo = cultInfo.DateTimeFormat;

            formatInfo.ShortDatePattern = "dd/MM/yy";
            formatInfo.ShortDatePattern = "dd/MM/yyyy";
            formatInfo.LongDatePattern = "dd MMMM yyyy";
            formatInfo.FullDateTimePattern = "dd MMMM yyyy HH:mm:ss";

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            //Convert strStartDate which is passed as string argument into date
            if (!String.IsNullOrEmpty(strStartDate))
                startDate = System.Convert.ToDateTime(strStartDate, formatInfo);

            //Convert strEndDate which is passed as string argument into date
            if (!String.IsNullOrEmpty(strEndDate))
                endDate = System.Convert.ToDateTime(strEndDate, formatInfo);

            return DateTime.Compare(startDate, endDate);


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
