
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using Utilities;
using DomainInterface;
using Permission.DAL;
using Infrastructure;
using Permission.DAL.Repository;
using Infrastructure.DAL;
using System.Collections;

namespace UI.Web.Admin.Controller
{

    public class BaseControlAdmin : System.Web.UI.UserControl
    {
        private static string _strConLogger;
        private CultureInfo _Culture;
        public static string _DateFormat = "dd/MM/yyyy";
        public string PageUrl = "";
        protected enum UserAction
        {
            ViewPage,
            Write,
            ExportGrid,
            SaveInstalation,
            TransferTOTOPM,
            GetInformationFromKam

        }

        public CultureInfo CultureWEB
        {
            get
            {
                if (_Culture == null)
                {
                    _Culture = CultureInfo.CreateSpecificCulture("en-US");
                    _Culture.DateTimeFormat.ShortDatePattern = _DateFormat;

                }
                return _Culture;
            }
        }

        public string Decrypt(string cipherText)
        {
            string plainText = "";

            if (cipherText != "" && cipherText != null)
            {
                //Key metro
                byte[] key = ASCIIEncoding.ASCII.GetBytes("whitecenter");

                //Encryption algrithm
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();

                //Memory stream containing cipher text
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cipherText));

                //Decrpt
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                    cryptoProvider.CreateDecryptor(key, key), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);

                plainText = reader.ReadToEnd();
            }

            return plainText;

        }
         
        #region "Utility Methods"

        public string ShowYesNo(bool isYes)
        {
            if (isYes)
            {
                return "<span class=\'label label-sm label-success\'>Active</span>";
            }
            else
            {
                return "<span class=\'label label-sm label-danger\'>Not Active</span>";
            }

        }
        public bool getBool(object ch)
        {
            if (object.ReferenceEquals(ch, DBNull.Value))
            {
                return false;

            }
            else if (ch.ToString().Equals(""))
            {
                return false;
            }
            else if (ch.ToString().Equals("0"))
            {
                return false;
            }
            else if (ch.ToString().Equals("1"))
            {
                return true;
            }

            else
            {
                return Convert.ToBoolean(ch.ToString());
            }
        }

        public string getBit(object ch)
        {
            if (object.ReferenceEquals(ch, DBNull.Value))
            {
                return "0";

            }
            else if (ch.ToString().Equals(""))
            {
                return "0";
            }
            else if (Convert.ToBoolean(ch) == false)
            {
                return "0";
            }
            else if (Convert.ToBoolean(ch) == true)
            {
                return "1";
            }
            return "0";

        }


        protected string gets(object obj)
        {
            if (object.ReferenceEquals(obj, DBNull.Value))
            {
                return "";
            }
            else
            {
                return Convert.ToString(obj);
            }
        }
        public void FillDll(object lstData, DropDownList ddl, string txtField, string valueField)
        {

            ddl.DataSource = lstData;
            ddl.DataTextField = txtField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Add(new ListItem("", "0"));

            //try
            //{
            //    ddl.SelectedValue = "0";
            //}
            //catch (Exception)
            //{


            //}
        }

        public void FillDllwithoptional(object lstData, DropDownList ddl, string txtField, string valueField)
        {

            ddl.DataSource = lstData;
            ddl.DataTextField = txtField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Add(new ListItem("", "0"));

            //try
            //{
            //    ddl.SelectedValue = "0";
            //}
            //catch (Exception)
            //{


            //}
        }


        public void FillDllwithoptional_ALL(object lstData, DropDownList ddl, string txtField, string valueField, string defaulttext)
        {

            ddl.DataSource = lstData;
            ddl.DataTextField = txtField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Add(new ListItem(defaulttext, "0"));

            try
            {
                ddl.SelectedValue = "0";
            }
            catch (Exception)
            {


            }
        }
        public string FormatpopupErrorMSG(string msg, string ErrorType)
        {
            string _out = "";
            if ((msg != null))
            {
                switch (ErrorType)
                {
                    case "1":
                        //Error
                        //  _out = "<div class='Errordivstyle'><div style='float:left'><img src='/Assets/images/error.png' alt='Error MSG'/></div> <div style='color:#000000;float:left;padding-left:5px;padding-top:5px;'>" + msg + "</div></div>";

                        _out = "";
                        _out += "   $(document).ready(function () { ";
                        _out += "new $.Zebra_Dialog('<strong>Error</strong>," + msg + "', {";
                        _out += "'buttons': false,";
                        _out += "'modal': false,";
                        _out += "'type':'error',";

                        if (Resources.Utilities.cuture == "en-US")
                        {
                            _out += "'position': ['right - 40', 'top + 80']";
                        }
                        else { _out += "'position': ['left + 40', 'top + 80']"; }


                        _out += ",'auto_close': 5000";
                        _out += "});";
                        _out += "});";


                        // Send Error meesage
                        try
                        {
                            if (msg.ToLower().Contains("The statement has been terminated"))
                            {
                             //   SendEmail("tarek.mosaad@yahoo.com", "HR Error Log", msg, "", "Qortuba");
                            }

                        }
                        catch (Exception)
                        {


                        }


                        break;
                    case "2":
                        //Notation
                        // _out = "<div class='notificationdivstyle'><div style='float:left'><img src='/Assets/images/notification.png' alt='Error MSG'/>&nbsp;</div> <div style='color:#000000;float:left;padding-left:5px;padding-top:5px;'>" + msg + "</div></div>";

                        _out = "";
                        _out += "   $(document).ready(function () { ";
                        _out += "new $.Zebra_Dialog('<strong>Information</strong>," + msg + "', {";
                        _out += "'buttons': false,";
                        _out += "'modal': false,";
                        _out += "'type':'information',";

                        if (Resources.Utilities.cuture == "en-US")
                        {
                            _out += "'position': ['right - 40', 'top + 80']";
                        }
                        else { _out += "'position': ['left + 40', 'top + 80']"; }


                        _out += ",'auto_close': 3000";
                        _out += "});";
                        _out += "});";







                        break;
                    case "3":
                        //Success
                        // _out = "<div class='ZebraDialog_Body ZebraDialog_Icon ZebraDialog_Information'><img src='/Assets/images/success.png' alt='Error MSG'/>&nbsp;<span style='color:#000000;'>" + msg + "</span></div>";



                        _out = "";
                        _out += "   $(document).ready(function () { ";
                        _out += "new $.Zebra_Dialog('<strong>Sucess</strong>," + msg + "', {";
                        _out += "'buttons': false,";
                        _out += "'modal': false,";
                        _out += "'type':'confirmation',";

                        if (Resources.Utilities.cuture == "en-US")
                        {
                            _out += "'position': ['right - 40', 'top + 80']";
                        }
                        else { _out += "'position': ['left + 40', 'top + 80']"; }


                        _out += ",'auto_close': 3000";
                        _out += "});";
                        _out += "});";


                        break;
                }

            }

            return _out;
        }


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
        //public void SendEmail(string target, string subject, string message, string from, string fromName)
        //{
        //    string fromuser = "webmaster@gcskw.com";// ConfigurationManager.AppSettings["FROM"];
        //    string frompass = "Web@2012";//ConfigurationManager.AppSettings["FROMNAME"];
        //    string server = "mail.gcskw.com";//ConfigurationManager.AppSettings["SMTPserver"];
        //    System.Web.Mail.MailMessage m = new System.Web.Mail.MailMessage();
        //    if (from.Equals(""))
        //        from = fromuser;

        //    m.From = fromuser;
        //    m.To = target;
        //    m.Body = message;
        //    m.Subject = subject;
        //    m.BodyFormat = System.Web.Mail.MailFormat.Html;
        //    System.Web.Mail.SmtpMail.SmtpServer = server;
        //    m.BodyEncoding = System.Text.Encoding.GetEncoding("windows-1256");
        //    m.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
        //    m.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] = fromuser;
        //    m.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] = frompass;
        //    System.Web.Mail.SmtpMail.Send(m);
        //}

        protected object NullifEmpty(string obj)
        {
            if (obj.Equals(""))
            {
                return DBNull.Value;
            }
            else
            {
                return obj;
            }
        }

        protected DateTime NullDateifEmpty(object obj)
        {
            if (obj == null || obj.Equals("") || obj.ToString().Equals("1/1/0001 12:00:00 AM"))
            {
                return new DateTime(1990, 01, 01);
            }
            else
            {
                try
                {
                    CultureInfo ci = new CultureInfo("ar-EG");

                    return Convert.ToDateTime(GetDateTimeForDB(obj.ToString()), ci);
                    //return Convert.ToDateTime(GetDateTimeForDB(obj.ToString()));
                }
                catch
                {
                    try
                    {
                        CultureInfo ci = new CultureInfo("en-US");

                        return Convert.ToDateTime(GetDateTimeForDB2(obj.ToString()), ci);
                    }
                    catch (Exception)
                    {

                        return Convert.ToDateTime(GetDateTimeForDB(new DateTime(1990, 01, 01).ToString()));
                    }

                }

            }
        }
        protected DateTime NullDateifEmptywithoutformat(object obj)
        {
            if (obj == null || obj.Equals("") || obj.ToString().Equals("1/1/0001 12:00:00 AM"))
            {
                return new DateTime(1990, 01, 01);
            }
            else
            {
                try
                {
                    CultureInfo ci = new CultureInfo("ar-EG");

                    return Convert.ToDateTime((obj.ToString()), ci);
                    //return Convert.ToDateTime(GetDateTimeForDB(obj.ToString()));
                }
                catch
                {
                    try
                    {
                        CultureInfo ci = new CultureInfo("en-US");

                        return Convert.ToDateTime((obj.ToString()), ci);
                    }
                    catch (Exception)
                    {

                        return Convert.ToDateTime((new DateTime(1990, 01, 01).ToString()));
                    }

                }

            }
        }

        protected DateTime NullDateFromDB(object obj)
        {
            if (obj == null || obj.Equals("") || obj.ToString().Equals("1/1/0001 12:00:00 AM"))
            {
                return new DateTime(1990, 01, 01);
            }
            else
            {
                try
                {
                    CultureInfo ci = new CultureInfo("ar-EG");

                    return Convert.ToDateTime(ReadFromDb2(obj.ToString()), ci);
                    //return Convert.ToDateTime(GetDateTimeForDB(obj.ToString()));
                }
                catch
                {
                    try
                    {
                        CultureInfo ci = new CultureInfo("en-US");
                        return Convert.ToDateTime(ReadFromDb(obj.ToString()), ci);
                    }
                    catch (Exception)
                    {

                        return Convert.ToDateTime(ReadFromDb2(new DateTime(1990, 01, 01).ToString()));
                    }


                }

            }
        }
        protected double ZeroIFNull(string obj)
        {
            if (obj.Equals(""))
            {
                return 0.0;
            }
            else
            {
                return Convert.ToDouble(obj);
            }
        }

        protected Int32 ZeroIntergerIFNull(string obj)
        {
            if (obj.Equals(""))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }


        protected string GetDateTimeForDB(string dat)
        {
            if (dat.Trim().Equals(""))
            {
                return "";
            }
            int h = 0;
            int min = 0;
            if (dat.Trim().IndexOf(" ") != -1)
            {
                string[] data = dat.Split(' ');
                dat = data[0];
                string[] time = data[1].Split(':');
                h = ZeroIntergerIFNull(time[0]);
                min = ZeroIntergerIFNull(time[1]);
            }
            string[] bl = dat.Split('/');
            int d = ZeroIntergerIFNull(bl[0]);
            int m = ZeroIntergerIFNull(bl[1]);
            int y = ZeroIntergerIFNull(bl[2]);

            return new DateTime(y, m, d, 0, 0, 0).ToString("dd/MM/yyyy");
        }

        protected string GetDateTimeForDB2(string dat)
        {
            if (dat.Trim().Equals(""))
            {
                return "";
            }
            int h = 0;
            int min = 0;
            if (dat.Trim().IndexOf(" ") != -1)
            {
                string[] data = dat.Split(' ');
                dat = data[0];
                string[] time = data[1].Split(':');
                h = ZeroIntergerIFNull(time[0]);
                min = ZeroIntergerIFNull(time[1]);
            }
            string[] bl = dat.Split('/');
            int d = ZeroIntergerIFNull(bl[1]);
            int m = ZeroIntergerIFNull(bl[0]);
            int y = ZeroIntergerIFNull(bl[2]);

            return new DateTime(y, m, d, 0, 0, 0).ToString("dd/MM/yyyy");

        }
        protected string ReadFromDb(string dat)
        {
            if (dat.Trim().Equals(""))
            {
                return "";
            }
            int h = 0;
            int min = 0;
            if (dat.Trim().IndexOf(" ") != -1)
            {
                string[] data = dat.Split(' ');
                dat = data[0];
                string[] time = data[1].Split(':');
                h = ZeroIntergerIFNull(time[0]);
                min = ZeroIntergerIFNull(time[1]);
            }
            string[] bl = dat.Split('/');
            int d = ZeroIntergerIFNull(bl[1]);
            int m = ZeroIntergerIFNull(bl[0]);
            int y = ZeroIntergerIFNull(bl[2]);

            return new DateTime(y, m, d, 0, 0, 0).ToString("dd/MM/yyyy");
        }

        protected string ReadFromDb2(string dat)
        {
            if (dat.Trim().Equals(""))
            {
                return "";
            }
            int h = 0;
            int min = 0;
            if (dat.Trim().IndexOf(" ") != -1)
            {
                string[] data = dat.Split(' ');
                dat = data[0];
                string[] time = data[1].Split(':');
                h = ZeroIntergerIFNull(time[0]);
                min = ZeroIntergerIFNull(time[1]);
            }
            string[] bl = dat.Split('/');
            int d = ZeroIntergerIFNull(bl[0]);
            int m = ZeroIntergerIFNull(bl[1]);
            int y = ZeroIntergerIFNull(bl[2]);

            return new DateTime(y, m, d, 0, 0, 0).ToString("dd/MM/yyyy");
        }

        protected string getDBDate(object obj)
        {
            if (object.ReferenceEquals(obj, DBNull.Value))
            {
                return "";
            }
            else
            {
                return Convert.ToDateTime(obj).ToString("dd/MM/yyyy");
            }
        }

        protected string getDBString(object obj)
        {
            if (object.ReferenceEquals(obj, DBNull.Value))
            {
                return "";
            }
            else
            {
                return Convert.ToString(obj);
            }
        }

        protected double getDBDecimal(object obj)
        {
            if (object.ReferenceEquals(obj, DBNull.Value))
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(obj);
            }
        }


        public string FormatErrorMSG(string msg, string ErrorType, [System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute("")]  // ERROR: Optional parameters aren't supported in C#
string imagepath)
        {
            string _out = "";
            if ((msg != null))
            {
                switch (ErrorType)
                {
                    case "1":
                        //Error
                        _out = "<div class='Errordivstyle'><table><tr><td><img src='" + imagepath + "/UIResouces/images/error.png' alt='Error MSG'/></td><td style='color:#ffffff;'>" + msg + "</td></tr></table></div>";
                        break;
                    case "2":
                        //Notation
                        _out = "<div class='notificationdivstyle'><table><tr><td><img src='" + imagepath + "/UIResouces/images/notification.png' alt='Error MSG'/></td><td style='color:#000000;'>" + msg + "</td></tr></table></div>";
                        //_out = "<div class='notificationdivstyle'><span><img src='" + imagepath + "/UIResouces/images/notification.png' alt='Error MSG'/></span><span style='color:#000000'>" + msg + "</span></div>";
                        break;
                    case "3":
                        //Success
                        _out = "<div class='successdivstyle'><table><tr><td><img src='" + imagepath + "/UIResouces/images/success.png' alt='Error MSG'/></td><td style='color:#ffffff;'>" + msg + "</td></tr></table></div>";
                        //_out = "<div class='successdivstyle'><span><img src='" + imagepath + "/UIResouces/images/success.png' alt='Error MSG'/></span><span style='color:#ffffff'>" + msg + "</span></div>";
                        break;



                }

            }

            return _out;
        }



        #endregion


        #region "Access Validation Methods"

        private string getCurrentUrl()
        {
            string url = "";

            url = Request.RawUrl.ToLower().Replace("_ar", "");
            url = url.Substring(url.LastIndexOf("/") + 1);
            if (url.IndexOf("?") != -1)
            {
                url = url.Substring(0, url.IndexOf("?"));
            }

            return url;
        }

        public void LoadPagePermission(Security_pr_admin currentuser, string url, string key)
        {

            AdminRepository objRepository = new AdminRepository();

            ArrayList PermssionDetaile = new ArrayList();


            var ObjUserPermission = objRepository.GetMemberShipPagePermssion(currentuser.AdminType, currentuser.id, url);



            string info = "0,0,0,0,0";
            if (ObjUserPermission != null && ObjUserPermission.Count != 0)
            {
                foreach (var item in ObjUserPermission)
                {

                                    info = "";
                    if (Convert.ToBoolean(item.show))
                    {
                        info += "1";
                    }
                    else
                    {
                        info += "0";
                    }
                    if (Convert.ToBoolean(item.AddRecord))
                    {
                        info += ",1";
                    }
                    else
                    {
                        info += ",0";
                    }
                    if (Convert.ToBoolean(item.modify))
                    {
                        info += ",1";
                    }
                    else
                    {
                        info += ",0";
                    }
                    if (Convert.ToBoolean(item.DeleteRecord ))
                    {
                        info += ",1";
                    }
                    else
                    {
                        info += ",0";
                    }
                    if (Convert.ToBoolean(item.DateControl))
                    {
                        info += ",1";
                    }
                    else
                    {
                        info += ",0";
                    }
                }


            }
            ViewState[key] = info;
        }
        #endregion
    }
}