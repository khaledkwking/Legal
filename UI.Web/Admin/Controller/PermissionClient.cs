using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using DomainInterface;
using Infrastructure;
using UI.Web.Admin.WebPages;

namespace UI.Web
{
    public class PermissionClient
    {
        //public IPermissionRepository objRepository = IoC.Resolve<IPermissionRepository>();

        static internal bool isAuthenticationCookie()
        {
            bool Exists = false;
            string cookieName = System.Web.Security.FormsAuthentication.FormsCookieName;
           // HttpCookie authCookie = null;
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
            //authCookie = null;
            if (((authCookie != null)))
            {
                IUser objUser = ConstantUI.CurrentUser;

                if (objUser == null)
                {
                    string strUserName = authCookie["UserName"];
                    objUser = PermissionFactory.GetObject().FindUserByName(strUserName);
                        //PermissionCFactory.getController().FindUserByName(strUserName);
                    ConstantUI.CurrentUser = objUser;
                }

                // There is authentication cookie.
                if (!authCookie.Value.Trim().Equals(""))
                {
                    Exists = true;
                }

            }
            else
            {
                Exists = false;
            }
            return Exists;
        }

        public static string getHashPassword(string strPassword)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(strPassword, "md5");
        }

        public static bool IsValidUser(string strUserName, string strPassword)
        {
            bool blnResult = false;

            IUser user = PermissionFactory.GetObject().FindUserByName(strUserName);
            if (user != null)
            {
                strPassword = getHashPassword(strPassword);
                if (user.Password == strPassword)
                {
                    ConstantUI.CurrentUser = user;
                    blnResult = true;
                }
            }

            return blnResult;
        }

        public static IPermissionPath ValidedPermission(System.Web.UI.Page _senderPage)
        {
            IPermissionPath blnHavePermission = null;
            if (!isAuthenticationCookie())
            {
                _senderPage.Response.Redirect("Login.aspx");
            }
            else if (!_senderPage.Request.FilePath.Contains("Home.aspx"))
            {
                string PageName = _senderPage.Request.FilePath;
                try
                {
                    PageName = _senderPage.Request.FilePath.Substring(_senderPage.Request.FilePath.LastIndexOf('/') + 1);
                }
                catch (System.Exception exc)
                { }
                IUser objUser = ConstantUI.CurrentUser;
// To Do
                Permission_Services objPermission_Services = new Permission_Services();

                blnHavePermission = objPermission_Services.IsUserHavePermissionAtPage(objUser, PageName);
            }
            return blnHavePermission;
        }

    }
}
