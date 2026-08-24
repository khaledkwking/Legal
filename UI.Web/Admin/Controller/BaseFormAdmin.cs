
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
using UI.Web.Controler;
using Microsoft.Reporting.WebForms;
using System.Drawing;
using System.Drawing.Imaging;

namespace UI.Web.Admin.Controller
{

    public class BaseFormAdmin : System.Web.UI.Page
    {
        private static string _strConLogger;
      
        private CultureInfo _Culture;
        public static string _DateFormat = "dd/MM/yyyy";
        private bool permstatus;

        public string PageUrl = "";
        public Access userAccess;


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

        /// <summary>
        /// Gets mode in which the application will be opened.
        /// There are three modes: 1-EditOnly 2-ViewOnly 3-ManageAll.
        /// </summary>
        /// <returns>IPermission</returns>
        public IPermissionPath SessioPermissionMode
        {
            get
            {
                return Session["PermissionMode"] as IPermissionPath;
            }
            set
            {
                Session["PermissionMode"] = value;
            }
        }

        public void SetPermissionAtPageControls()
        {
            foreach (Control ctrl in this.Form.Controls)
            {
                if (ctrl is ContentPlaceHolder)
                {
                    ContentPlaceHolder chp = ((System.Web.UI.WebControls.ContentPlaceHolder)ctrl);
                    foreach (Control ctrl2 in chp.Controls)
                    {
                        if (ctrl2 is TextBox)
                        {
                            TextBox txt = ((System.Web.UI.WebControls.TextBox)ctrl2);
                            //Make Text ReadOnly by permission by user
                            //  4 = Read
                            if (SessioPermissionMode.PermissionID == 4 || SessioPermissionMode.Permission.Type == "Read")
                            {
                                permstatus = true;
                            }
                            else
                            {
                                permstatus = false;
                            }


                            txt.ReadOnly = !(permstatus);

                            //txt.ReadOnly = !((SessioPermissionMode.Type == "" )?? false);
                            // txt.Enabled = (SessioPermissionMode.IsUpdateEnabled ?? false);
                        }
                        if (ctrl2 is Button)
                        {
                            Button btn = ((System.Web.UI.WebControls.Button)ctrl2);
                            //Make Text ReadOnly by permission by user
                            // btn.Enabled = (SessioPermissionMode.IsUpdateEnabled ?? false);
                            //  4 = Read
                            if (SessioPermissionMode.PermissionID == 4 || SessioPermissionMode.Permission.Type == "Read")
                            {
                                permstatus = true;
                            }
                            else
                            {
                                permstatus = false;
                            }
                            btn.Visible = permstatus;
                            //btn.Visible = (SessioPermissionMode.IsUpdateEnabled ?? false);

                        }
                    }
                }

            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ///
        private Security_pr_admin user;
        // public IAdminRepository objRepository = IoC.Resolve<IAdminRepository>();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //if (!Page.IsPostBack)
            //{
                #region Old Code
                //IPermissionPath objPermission = PermissionClient.ValidedPermission(this.Page);

                ////Don't log the Main page to loges Table
                //if (!this.Page.Request.FilePath.Contains("Home.aspx"))
                //{
                //    SessioPermissionMode = objPermission;

                //    //  4 = Read
                //    if (objPermission != null)
                //    {
                //        if (objPermission.PermissionID == 4 || objPermission.Permission.Type == "Read")
                //        {
                //            permstatus = true;
                //        }
                //        else
                //        {
                //            permstatus = false;
                //        }
                //    }

                //    if (objPermission == null || !(permstatus))
                //    {
                //        //Not have permission at current page
                //        this.Page.Response.Redirect("Home.aspx");
                //    }

                //    if (objPermission != null)
                //    {
                //        //Set Permission at page control according to User permission
                //        SetPermissionAtPageControls();
                //    }

                //    //PageName = PageName.Substring(PageName.LastIndexOf('/') + 1);
                //    LogUserAction(UserAction.ViewPage);
                //}
                #endregion

                if (!MemberShip_Permission.isAuthenticationCookie())
                {
                    Response.Redirect("~/Admin/Pages/Login.aspx");
                    return;
                }

                if (PageUrl.Trim().Equals(""))
                { PageUrl = getCurrentUrl(); }
                user = MemberShipConstantUI.CurrentUser;
                LoadPagePermission(user, PageUrl, "CurrentPage");
                Access ac = new Access(Session["CurrentPage"].ToString());
                userAccess = new Access(Session["CurrentPage"].ToString());
                if (!ac.Show & !PageUrl.Trim().Equals("home.aspx") & !PageUrl.Trim().Equals("mainmenu.aspx"))
                {
                    Response.Redirect("~/Admin/Pages/AccessDenied.aspx?OUT=1&ReturnUrl=" + Request.RawUrl);
                }

            //}
        }


        public Boolean ShowHideButtonBool(string _function)
        {
            //switch (_function)
            //{

            //    case "Add":
            //        return userAccess.Add  ;
            //    case "Edit":
            //        return userAccess.Edit;
            //    case "Delete":
            //        return userAccess.Delete;
            //    default:
            //        return false;
            //}
            return false;
        }

        public string  ShowHideButton(string _function)
        {
            switch (_function)
            {

                case "Add":
                    return userAccess.Add?"": "hidden";
                case "Edit":
                    return userAccess.Edit ? "" : "hidden";
                case "Delete":
                    return userAccess.Delete ? "" : "hidden";
                default:
                    return "hidden";   
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override void OnPreInit(EventArgs e)
        {
            // Constant.SelectMasterForAdmin(this);
            base.OnPreInit(e);
        }

       


        #region "Utility Methods"
        public string GenerateBar(string id)
        {
            string bar = "";
            int zeroLength = 7;
            //  the length of digits
            int rest = (zeroLength - id.Length);
            bar = "0";
            for (int i = 0; (i <= (rest - 1)); i++)
            {
                bar += "0";
            }

            bar = (bar + id);
            return bar;
        }
        public object ReadSession(string sessionKey)
        {
            if (Session[sessionKey] == null)
            {
                return "0";

            }
            return Session[sessionKey];
        }

        //public string GetFileName(AjaxControlToolkit.AsyncFileUpload txtImage)
        //{
        //    string imgname = "";
        //    int inx = 0;
        //    string temp = "";
        //    string ext = "";
        //    string RandChar = "";
        //    string ValueString = "";
        //    Microsoft.VisualBasic.VBMath.Randomize();
        //    imgname = "";
        //    imgname = txtImage.PostedFile.FileName;
        //    imgname = imgname.Substring((imgname.LastIndexOf("\\") + 1));
        //    inx = imgname.LastIndexOf('.');
        //    temp = imgname.Substring(0, inx);
        //    ext = imgname.Substring((inx + 1));
        //    for (int i = 1; (i <= 10); i++)
        //    {
        //        RandChar = (string)(Microsoft.VisualBasic.Conversion.Int((26 * Microsoft.VisualBasic.VBMath.Rnd() + 65)).ToString());
        //        ValueString += RandChar;
        //    }
        //    imgname = (temp + (ValueString + ("." + ext)));
        //    return imgname;
        //}
        public string ItemUnitisout(string ItemUnitStatus)
        {
            if (ItemUnitStatus.Equals("0"))
            {
                return "<span class=\'label label-sm label-warning\'>Planned</span>";
            }
            else if (ItemUnitStatus.Equals("1"))
            {
                return "<span class=\'label label-sm label-success\'>Received</span>";
            }
            else if (ItemUnitStatus.Equals("2"))
            {
                return "<span class=\'label label-sm label-info\'>Addtional</span>";
            }
            else if (ItemUnitStatus.Equals("3"))
            {
                return "<span class=\'label label-sm label-danger\'>Delivered</span>";
            }
            else
            {
                return "";
            }


        }
        public string ShowYesNo(bool isYes)
        {
            if (isYes)
            {
                return "<span class=\'label label-sm label-success\'>&nbsp;YES&nbsp;</span>";
            }
            else
            {
                return "<span class=\'label label-sm label-danger\'>&nbsp;NO&nbsp;</span>";
            }

        }

        public string UploadFile(FileUpload txtImage, string uploadPath)
        {
            string imgname = "";
            int inx = 0;
            string temp = "";
            string ext = "";
            string RandChar = "";
            string ValueString = "";
            Microsoft.VisualBasic.VBMath.Randomize();
            imgname = "";

            if (txtImage.PostedFile != null && txtImage.PostedFile.FileName != "")
            {

                imgname = txtImage.PostedFile.FileName;
                imgname = imgname.Substring((imgname.LastIndexOf("\\") + 1));
                inx = imgname.LastIndexOf('.');
                temp = imgname.Substring(0, inx);
                ext = imgname.Substring((inx + 1));
                for (int i = 1; (i <= 10); i++)
                {
                    RandChar = (string)(Microsoft.VisualBasic.Conversion.Int((26 * Microsoft.VisualBasic.VBMath.Rnd() + 65)).ToString());
                    ValueString += RandChar;
                }
                imgname = ((ValueString + ("." + ext)));
                txtImage.PostedFile.SaveAs(Server.MapPath(uploadPath + imgname));
            }


            return imgname;
        }

        public string UploadFileoServer(FileUpload txtImage, string uploadPath)
        {
            string imgname = "";
            int inx = 0;
            string temp = "";
            string ext = "";
            string RandChar = "";
            string ValueString = "";
            Microsoft.VisualBasic.VBMath.Randomize();
            imgname = "";

            if (txtImage.PostedFile != null && txtImage.PostedFile.FileName != "")
            {

                imgname = txtImage.PostedFile.FileName;
                imgname = imgname.Substring((imgname.LastIndexOf("\\") + 1));
                inx = imgname.LastIndexOf('.');
                temp = imgname.Substring(0, inx);
                ext = imgname.Substring((inx + 1));
                for (int i = 1; (i <= 10); i++)
                {
                    RandChar = (string)(Microsoft.VisualBasic.Conversion.Int((26 * Microsoft.VisualBasic.VBMath.Rnd() + 65)).ToString());
                    ValueString += RandChar;
                }
                imgname = ((ValueString + ("." + ext)));

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                txtImage.PostedFile.SaveAs(uploadPath + imgname);
            }


            return imgname;
        }

        public bool DeleteFileFromServer(string filename, string uploadPath)
        {
            //try
            //{
            //    if (File.Exists(uploadPath + filename))
            //    {
            //        File.Delete(uploadPath + filename);
            //        return true;
            //    }
            //}
            //catch (Exception ex)
            //{

            //    return false;
            //}
            //return false;
            return true;

        }

        public bool getBool(object ch)
        {
            if (ch == null)
            {
                return false;

            }
            else if (ch == DBNull.Value)
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
            if (obj == null || object.ReferenceEquals(obj, DBNull.Value))
            {
                return "";
            }
            else if (obj.ToString().Equals("&nbsp;"))
            {
                return "";
            }
            else
            {
                return Convert.ToString(obj);
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
                        _out += "new $.Zebra_Dialog('<strong>" + Resources.Alerts.error + "</strong>," + msg + "', {";
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
                        //try
                        //{
                        //    if (msg.ToLower().Contains("The statement has been terminated"))
                        //    {
                        //        SendEmail("tarek.mosaad@yahoo.com", "CMGS Error Log", msg, "", "CMGS");
                        //    }

                        //}
                        //catch (Exception)
                        //{

                        //}


                        break;
                    case "2":
                        //Notation
                        // _out = "<div class='notificationdivstyle'><div style='float:left'><img src='/Assets/images/notification.png' alt='Error MSG'/>&nbsp;</div> <div style='color:#000000;float:left;padding-left:5px;padding-top:5px;'>" + msg + "</div></div>";

                        _out = "";
                        _out += "   $(document).ready(function () { ";
                        _out += "new $.Zebra_Dialog('" + msg + "', {";
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
                        _out += "new $.Zebra_Dialog('" + msg + "', {";
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
        //    string fromuser =   ConfigurationManager.AppSettings["FROM"];
        //    string frompass = ConfigurationManager.AppSettings["FROMPASS"];
        //    string server = ConfigurationManager.AppSettings["SMTPserver"];
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


        protected string NullDateifEmptyToText(object obj)
        {
            if (obj == null || obj.Equals("") || obj.ToString().Equals("1/1/0001 12:00:00 AM"))
            {
                return "";
            }
            else if (obj == null || obj.Equals("") || obj.ToString().Equals("1/1/1990 12:00:00 AM"))
            {
                return "";
            }
            else
            {
                try
                {
                    CultureInfo ci = new CultureInfo("ar-EG");
                    if (Convert.ToDateTime(GetDateTimeForDB(obj.ToString()), ci).ToString("dd/MM/yyyy").Equals("01/01/1990"))
                    {
                        return "";
                    }
                    else
                    { return Convert.ToDateTime(GetDateTimeForDB(obj.ToString()), ci).ToString("yyyy/MM/dd"); }

                    //return Convert.ToDateTime(GetDateTimeForDB(obj.ToString()));
                }
                catch
                {
                    try
                    {
                        CultureInfo ci = new CultureInfo("en-US");
                        if (Convert.ToDateTime(GetDateTimeForDB3(obj.ToString()), ci).ToString("dd/MM/yyyy").Equals("01/01/1990"))
                        {
                            return "";
                        }
                        else
                        { return Convert.ToDateTime(GetDateTimeForDB3(obj.ToString()), ci).ToString("yyyy/MM/dd"); }
                    }
                    catch (Exception)
                    {

                        return "";
                    }

                }

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
                    CultureInfo ci = new CultureInfo("ar-EG");//dd/MM/YYY

                    return Convert.ToDateTime(GetDateTimeForDB2(obj.ToString()), ci);
                    //return Convert.ToDateTime(GetDateTimeForDB(obj.ToString()));
                }
                catch (Exception)
                {
                    try
                    {
                        CultureInfo ci = new CultureInfo("ar-EG");//MM/dd/YYY

                        return Convert.ToDateTime(GetDateTimeForDB(obj.ToString()), ci);
                    }
                    catch (Exception)
                    {

                        try
                        {
                            CultureInfo ci = new CultureInfo("en-US");//dd/MM/yyyy

                            return Convert.ToDateTime(GetDateTimeForDB(obj.ToString()), ci);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                CultureInfo ci = new CultureInfo("en-US");//MM/dd/YYY

                                return Convert.ToDateTime(GetDateTimeForDB2(obj.ToString()), ci);

                            }
                            catch (Exception)
                            {

                                return Convert.ToDateTime(GetDateTimeForDB(new DateTime(1990, 01, 01).ToString()));
                            }

                        }
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

        protected int ZeroIntergerIFNullNew (string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
                return 0;

            return int.TryParse(obj, out int val) ? val : 0;
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
            int d = ZeroIntergerIFNull(bl[2]);
            int m = ZeroIntergerIFNull(bl[1]);
            int y = ZeroIntergerIFNull(bl[0]);

            return new DateTime(y, m, d, 0, 0, 0).ToString("dd/MM/yyyy");

        }


        protected string GetDateTimeForDB3(string dat)
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

            return new DateTime(y, m, d, 0, 0, 0).ToString("MM/dd/yyyy");

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


        #region "Image Processing"

        public string FillImage(string strimgName, string strimagFolder, int ReWidth, int ReHeight, string alt)
        {
            try
            {
                if ((strimgName == ""))
                {
                    // Return ""
                    // Warning!!! Optional parameters not supported
                    // Warning!!! Optional parameters not supported
                    strimgName = "no.gif";
                }

                if ((strimgName == "none"))
                {
                    // Return ""
                    strimgName = "no.gif";
                }

                string strwidth;
                strwidth = "width=\'";
                string strheight;
                strheight = "height=\'";
                //int fileheight;
                //int filewidth;
                //fileheight = 0;
                //filewidth = 0;
                // Dim Fs As FileStream
                strwidth = (strwidth
                            + (ReWidth + "\'"));
                strheight = (strheight
                            + (ReHeight + "\'"));

                return "<img  alt='" + alt + "'  src='" + strimagFolder + strimgName + "' " + strwidth + " " + strheight + " class='imgborder' align='center'>";
            }
            catch (Exception)
            {

                return "";
                // " + strimagFolder + "/noLogo.png'
            }

        }

        public string FillForceImage(string strimgName, string strimagFolder, int ReWidth, int ReHeight, string alt)
        {
            try
            {
                if ((strimgName == ""))
                {
                    // Return ""
                    // Warning!!! Optional parameters not supported
                    strimgName = "logo.gif";
                }

                if ((strimgName == "none"))
                {
                    // Return ""
                    strimgName = "logo.gif";
                }

                string strwidth;
                strwidth = "width=\'";
                string strheight;
                strheight = "height=\'";
                int fileHeight;
                int fileWidth;
                fileHeight = 0;
                fileWidth = 0;
                FileStream Fs;
                Fs = new FileStream(Server.MapPath((strimagFolder + strimgName)), FileMode.Open, FileAccess.Read, FileShare.Read);
                System.Drawing.Image image;
                image = System.Drawing.Image.FromStream(Fs);
                fileWidth = image.Width;
                fileHeight = image.Height;
                Fs.Close();
                Fs = null;
                if (((fileWidth > ReWidth)
                            || (fileHeight > ReHeight)))
                {
                    if ((fileWidth > fileHeight))
                    {
                        int wL;
                        int hL;
                        wL = ReWidth;
                        hL = ((wL * fileHeight)
                                    / fileWidth);
                        strwidth = (strwidth
                                    + (wL + "\'"));
                        strheight = (strheight
                                    + (hL + "\'"));
                        if ((hL > ReHeight))
                        {
                            strwidth = "width=\'";
                            strheight = "height=\'";
                            hL = ReHeight;
                            wL = ((hL * fileWidth)
                                        / fileHeight);
                            strwidth = ((strwidth
                                        + (wL + "\'"))).ToString();
                            strheight = (strheight
                                        + (hL + "\'"));
                        }

                    }
                    else
                    {
                        int wL;
                        int hL;
                        hL = ReHeight;
                        wL = ((hL * fileWidth)
                                    / fileHeight);
                        strwidth = ((strwidth
                                    + (wL + "\'"))).ToString();
                        strheight = (strheight
                                    + (hL + "\'"));
                    }

                }
                else
                {
                    strwidth = (strwidth
                                + (fileWidth + "\'"));
                    strheight = (strheight
                                + (fileHeight + "\'"));
                }

                return (("<img  border=0 alt=\'"
                            + (alt + "\' class=\'imgBorder\' src=\'"))
                            + (strimagFolder
                            + (strimgName + ("\' "
                            + (strwidth + (" "
                            + (strheight + " align=\'center\'>")))))));
            }
            catch (Exception)
            {
                return ("<img src=\'"
                            + (strimagFolder + ("noLogo.png\' width=\'"
                            + (ReWidth.ToString() + ("\' height=\'"
                            + (ReHeight.ToString() + "\' border=\'0\' alt=\'\'/>"))))));
            }

        }
        #endregion


        #region "Access Validation Methods"

        private string getCurrentUrl()
        {
            string url = "";

            url = Request.RawUrl.ToLower().Replace("_ar", "");
            url = url.Substring(url.LastIndexOf("/") + 1);
            if (!url.Contains("tablename"))
            {
                if (url.IndexOf("?") != -1)
                {
                    url = url.Substring(0, url.IndexOf("?"));
                }
            }


            return url;
        }

        public void LoadPagePermission(Security_pr_admin currentuser, string url, string key)
        {

            AdminRepository objRepository = new AdminRepository();

            ArrayList PermssionDetaile = new ArrayList();
            var ObjUserPermission = objRepository.GetMemberShipPagePermssion(currentuser.AdminType, currentuser.id, url);

            string info = "0,0,0,0,0,0";
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
                    if (Convert.ToBoolean(item.DeleteRecord))
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
                    if (Convert.ToBoolean(item.AuditControl))
                    {
                        info += ",1";
                    }
                    else
                    {
                        info += ",0";
                    }
                }


            }
            Session[key] = info;
        }
        #endregion


        #region "Helper Methods Fill DropDown"
        public void FillDll(object lstData, ref DropDownList ddl, string txtField, string valueField)
        {
            ddl.Items.Clear();
            ddl.SelectedIndex = -1;
            ddl.SelectedValue = null;
            ddl.ClearSelection();


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
        public void FillDllwithOutoptional_ALL(object lstData, ref DropDownList ddl, string txtField, string valueField)
        {
            ddl.Items.Clear();
            ddl.SelectedIndex = -1;
            ddl.SelectedValue = null;
            ddl.ClearSelection();

            ddl.DataSource = lstData;
            ddl.DataTextField = txtField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            
        }
        public void FillDllwithoptional(object lstData, DropDownList ddl, string txtField, string valueField)
        {

            ddl.Items.Clear();
            ddl.SelectedIndex = -1;
            ddl.SelectedValue = null;
            ddl.ClearSelection();

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


        public void FillDllwithoptional_ALL(object lstData, ref DropDownList ddl, string txtField, string valueField, string defaulttext)
        {
            ddl.Items.Clear();
            ddl.SelectedIndex = -1;
            ddl.SelectedValue = null;
            ddl.ClearSelection();

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


        public void FillDllwithoptional_Array(object lstData, DropDownList ddl, string defaulttext)
        {
            ddl.Items.Clear();
            ddl.DataSource = lstData;

            ddl.DataBind();
            ddl.Items.Add(new ListItem(defaulttext, ""));

            try
            {
                ddl.SelectedValue = "";
            }
            catch (Exception)
            {


            }
        }

        #endregion

        #region "List Processing"
        //public List<object> DuplicatedList(List<object> souceList)
        //{
        //    List<object> _outList = new List<object>();



        //}
        #endregion
        public string showHideAttachment(string hasattachment)
        {
            if (hasattachment != "")
            {
                return "";
            }
            return "display:none";

        }

        public bool CopyFile(string filename, string soucepath, string destpath)
        {
            try
            {


                if (File.Exists(soucepath + filename))
                {
                    if (!Directory.Exists(destpath))
                        Directory.CreateDirectory(destpath);

                    File.Copy(soucepath + filename, destpath + filename);
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        public string Export(LocalReport rpt, string filePath)
        {
            string ack = "";
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                using (FileStream stream = File.OpenWrite(filePath))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                return ack;
            }
            catch (Exception ex)
            {
                ack = ex.InnerException.Message;
                return ack;
            }
        }

        public void ExportPdf(ReportViewer rv, string fileName)
        {
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string encoding;
            string extension;

            //Export the RDLC Report to Byte Array.
            byte[] bytes = rv.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);

            // Open generated PDF.
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            // Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
            //Response.AddHeader("content-disposition", "filename=" + fileName + "." + extension);
          //  Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes);

            Response.Flush();
            Response.End();
        }
        public string getUserWatermarkImage(string userId, string userName)
        {
            string _filepath = "";
            //check file Existance
            if (File.Exists(Server.MapPath("~/Layout/uploads/Adminprofile/" + userId + ".png")))
            {
                _filepath =  new Uri(Server.MapPath("~/Layout/uploads/Adminprofile/" + userId + ".png")).AbsoluteUri;
               // _filepath = new Uri(Server.MapPath("~/Layout/uploads/Adminprofile/4.png")).AbsoluteUri;
            }
            else
            { //Create New Waterfile


                //Bitmap bitMapImage = new System.Drawing.Bitmap((Server.MapPath("~/Layout/uploads/Adminprofile/mask.png")));
                //Graphics graphicImage = Graphics.FromImage(bitMapImage);
                //graphicImage.DrawString(userName,  new Font("Arial", 20, FontStyle.Bold),
                //SystemBrushes.WindowText, new Point(0, 0));
                //bitMapImage.Save(Server.MapPath("~/Layout/uploads/Adminprofile/"+ userId + ".png"), ImageFormat.Png);
                //graphicImage.Dispose();
                //bitMapImage.Dispose();


                Bitmap bmp = new System.Drawing.Bitmap((Server.MapPath("~/Layout/uploads/Adminprofile/mask.png")));
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.TranslateTransform(bmp.Width / 2, bmp.Height / 2);
                    g.RotateTransform(30);
                   SizeF textSize = g.MeasureString(userName, new Font("Arial", 20, FontStyle.Bold));
                 //   g.DrawString(userName, new Font("Arial", 20, FontStyle.Bold), Brushes.Red, 0,0);
                    g.DrawString(userName , new Font("Arial", 20, FontStyle.Bold), Brushes.Silver, -(textSize.Width / 2), -(textSize.Height / 2));
                }

                bmp.Save(Server.MapPath("~/Layout/uploads/Adminprofile/" + userId + ".png"), ImageFormat.Png);
                bmp.Dispose();
                bmp.Dispose();
                _filepath = new Uri(Server.MapPath("~/Layout/uploads/Adminprofile/" + userId + ".png")).AbsoluteUri;

            }




            return _filepath;

        }

    }
}