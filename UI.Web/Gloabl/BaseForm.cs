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
namespace UI.Web 
{ 

    public class BaseForm : System.Web.UI.Page
    {
        private static string _strConLogger;
        
         private  CultureInfo _Culture;
        public static string  _DateFormat = "dd/MM/yyyy";

      protected   enum UserAction
        {
            ViewPage ,
            Write ,
            ExportGrid ,
            SaveInstalation ,
            TransferTOTOPM ,
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
                //Key
                byte[] key = ASCIIEncoding.ASCII.GetBytes("metro");

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
        //public IPermission SessioPermissionMode
        //{ get 
        //   { 
        //       return Session["PermissionMode"] as IPermission;
        //    }
        //    set
        //    {
        //        Session["PermissionMode"] = value;
        //    }
        //}


        //public void SetPermissionAtPageControls()
        //{
        //    foreach (Control ctrl in this.Form.Controls)
        //    {
        //        if (ctrl is ContentPlaceHolder)
        //        {
        //            ContentPlaceHolder chp = ((System.Web.UI.WebControls.ContentPlaceHolder)ctrl);
        //            foreach (Control ctrl2 in chp.Controls)
        //            {
        //                if (ctrl2 is TextBox)
        //                {
        //                    TextBox txt = ((System.Web.UI.WebControls.TextBox)ctrl2);
        //                    //Make Text ReadOnly by permission by user
        //                    txt.ReadOnly = !(SessioPermissionMode.IsUpdateEnabled ?? false);
        //                   // txt.Enabled = (SessioPermissionMode.IsUpdateEnabled ?? false);
        //                }
        //                if (ctrl2 is Button)
        //                {
        //                    Button btn = ((System.Web.UI.WebControls.Button)ctrl2);
        //                    //Make Text ReadOnly by permission by user                          
        //                   // btn.Enabled = (SessioPermissionMode.IsUpdateEnabled ?? false);
        //                    btn.Visible = (SessioPermissionMode.IsUpdateEnabled ?? false);

        //                }
        //            }
        //        }

        //    } 
        //}


        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
              //IPermission objPermission = PermissionClient.ValidedPermission(this.Page);
                 
              //  //Don't log the Main page to loges Table
              //  if (!this.Page.Request.FilePath.Contains("MainPage.aspx"))
              //  {
              //   SessioPermissionMode = objPermission;

              //    if (objPermission == null || !(objPermission.IsReadEnabled ?? false))
              //    {
              //      //Not have permission at current page
              //      this.Page.Response.Redirect("MainPage.aspx");
              //    }

              //      if (objPermission != null)
              //      {
              //          //Set Permission at page control according to User permission
              //          SetPermissionAtPageControls();
              //      }

              //      //PageName = PageName.Substring(PageName.LastIndexOf('/') + 1);
              //      LogUserAction(UserAction.ViewPage);

              //  }

            }
         
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        protected override void OnPreInit(EventArgs e)
        {    
            Constant.SelectMaster(this);
            base.OnPreInit(e);
        }
     
    }
}