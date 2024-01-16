using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using DomainInterface;
using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using Utilities;

namespace UI.Web
{

    public class Global : System.Web.HttpApplication
    {
        //IAdminRepository Reposatory = IoC.Resolve<IAdminRepository>();
       // public LoggerRepository _logger = IoC.Resolve<LoggerRepository>();
        protected void Application_Start(object sender, EventArgs e)
        {
            //initialize IOC
            IoC.InitializeWith(new DependencyResolverFactory());

            // IAdminRepository objRepository = IoC.Resolve<IAdminRepository>();

            try
            {
                //var ObjVisitor = objRepository.GetTAW_Access_Levels();
                //if (ObjVisitor != null)
                //{
                //    if (ObjVisitor.Count == null)
                //    {
                //        ObjVisitor.Count = 0;
                //        objRepository.UpdateTAW_Access_Level(ObjVisitor);
                //    }
                //    else
                //    {
                //        ObjVisitor.Count = ObjVisitor.Count + 1;
                //        objRepository.UpdateTAW_Access_Level(ObjVisitor);
                //    }

                //}
            }
            catch (System.Exception)
            { }



        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Session["masterpage"] = "~/Masters/WHSite.Master";
            //System.Globalization.CultureInfo CI =
            //   new System.Globalization.CultureInfo("en-US");
            //Settings.Language = CI;


        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Try to log the user enter at this page
            try
            {
                System.Exception _Exception = Server.GetLastError();
                string strErrorr = string.Empty;
                string strErrorrInner = string.Empty;
                if (_Exception != null && _Exception.InnerException != null)
                {
                    strErrorr = _Exception.InnerException.Message;
                    strErrorrInner = _Exception.InnerException.ToString();
                }

                HttpContext ctx = HttpContext.Current;
                string PageName = ctx.Request.Url.ToString();

                PageName = PageName.Substring(PageName.LastIndexOf('/') + 1);
               

                string strUser = string.Empty;
                if (ConstantUI.CurrentUser != null)
                    strUser = ConstantUI.CurrentUser.Name;
                LoggerException _log = new LoggerException();
                _log.PagePath = PageName;   
                _log.UserName = strUser;  
                _log.Message = strErrorr;
                _log.InnerException = strErrorrInner;
                //_logger.AddLoggere(_log);

                //   if (strUser != string.Empty)
                string strLang = Settings.Language.Name;

                if (strLang.Equals("ar"))
                {
                    //Response.Redirect(@"~\UIPages\ARError.aspx");
                }
                else
                {
                    // Response.Redirect(@"~\UIPages\Error.aspx");
                }

            }
            catch (System.Exception)
            { }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies["strCookieeLanguage"] != null)
                HttpContext.Current.Request.Cookies["strCookieeLanguage"].Value = null;


            //if (HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies["CMGSPORTAL"] != null)
            //    HttpContext.Current.Request.Cookies["CMGSPORTAL"].Value = null;

        }
    }
}