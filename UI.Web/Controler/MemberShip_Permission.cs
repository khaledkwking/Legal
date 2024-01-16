using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DomainInterface;

using Infrastructure;
using Permission.DAL.Repository;
using Infrastructure.DAL;
using System.Collections;

namespace UI.Web.Controler
{
    public class MemberShip_Permission : SuperBusiness
    {
        //public IPermissionRepository objRepository = IoC.Resolve<IPermissionRepository>();
        public static MemberShip_Permission ins = new MemberShip_Permission();


        static internal bool isAuthenticationCookie()
        {
             AdminRepository objRepository = new AdminRepository();
            Security_pr_admin objUser = MemberShipConstantUI.CurrentUser;

            if (MemberShipConstantUI.CurrentUser == null)
            {

                if (objUser == null && HttpContext.Current.Request.Form["calleruser"] != null)
                {
                    objUser = objRepository.GetMemberShipById(int.Parse(HttpContext.Current.Request.Form["calleruser"]));
                    MemberShipConstantUI.CurrentUser = objUser;
                    return true; //Featch User Data and Returen User Status
                }


                HttpCookie authCookie = HttpContext.Current.Request.Cookies["CMGSPORTAL"];
                if (authCookie != null && !authCookie.Value.Trim().Equals("") && !authCookie.Value.Trim().Equals(null))
                {
                    string strUserName = authCookie["CMGSPORTAL"];
                    objUser = objRepository.GetMemberShipByName(strUserName);
                    MemberShipConstantUI.CurrentUser = objUser;
                    return true;

                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidUser(string strUserName, string strPassword)
        {
            //AdminRepository objRepository = IoC.Resolve<AdminRepository>();
            AdminRepository objRepository = new AdminRepository();
            bool blnResult = false;

            Security_pr_admin user = objRepository.GetMemberShipByName(strUserName);
            if (user != null)
            {
                // strPassword = getHashPassword(strPassword);

                if (user.password == strPassword && user.IsActive == true)
                {
                    MemberShipConstantUI.CurrentUser = user;
                    blnResult = true;
                }
            }

            return blnResult;
        }

        //public static IPermissionPath ValidedPermission(System.Web.UI.Page _senderPage)
        //{
        //    IPermissionPath blnHavePermission = null;
        //    if (!isAuthenticationCookie())
        //    {
        //        _senderPage.Response.Redirect("Login.aspx");
        //    }
        //    else if (!_senderPage.Request.FilePath.Contains("Home.aspx"))
        //    {
        //        string PageName = _senderPage.Request.FilePath;
        //        try
        //        {
        //            PageName = _senderPage.Request.FilePath.Substring(_senderPage.Request.FilePath.LastIndexOf('/') + 1);
        //        }
        //        catch (System.Exception exc)
        //        { }
        //        IUser objUser = ConstantUI.CurrentUser;
        //        // To Do
        //        Permission_Services objPermission_Services = new Permission_Services();

        //        blnHavePermission = objPermission_Services.IsUserHavePermissionAtPage(objUser, PageName);
        //    }
        //    return blnHavePermission;
        //}

        public ArrayList SetUserPermission(int? adminType, int AdminID)
        {

            AdminRepository objRepository = new AdminRepository();
            Hashtable tbl = new Hashtable();
            Hashtable sys = new Hashtable();
            ArrayList PermssionDetaile = new ArrayList();
            string last = "";
            bool lshow = false;
            var ObjUserPermission = new List<Security_SP_PRM_getSystemPermission_Result>();
            var ObjUserPermission_job = new List<Security_SP_PRM_getJobPermissions_Result>();
            var ObjUserPermissionCount = objRepository.GetMemberShipPermssionCount(adminType, AdminID);


            if (ObjUserPermissionCount == 0)
            {
                //Set Job Permission
                ObjUserPermission_job = objRepository.GetMemberShipJobPermssion(adminType);


                if (ObjUserPermission_job != null)

                {

                    for (int i = 0; i < ObjUserPermission_job.Count; i++)
                    {
                        bool show = GetBool(ObjUserPermission_job[i].show);
                        string url = gets(ObjUserPermission_job[i].url).ToLower();
                        if (!url.Trim().Equals(""))
                        {
                            tbl.Add(url, show);
                        }

                        string systemid = gets(ObjUserPermission_job[i].SystemID).ToLower();
                        if (!last.Trim().Equals(systemid))
                        {
                            if ((i != 0))
                            {
                                sys.Add(last, lshow);
                            }

                            last = systemid;
                            lshow = false;
                        }
                        else if (show)
                        {
                            lshow = true;
                        }

                    }

                }



            }
            else
            {
                ObjUserPermission = objRepository.GetMemberShipSystemPermssion(adminType, AdminID);



                if (ObjUserPermission != null)

                {

                    for (int i = 0; i < ObjUserPermission.Count; i++)
                    {
                        bool show = GetBool(ObjUserPermission[i].show);
                        string url = gets(ObjUserPermission[i].url).ToLower();
                        if (!url.Trim().Equals(""))
                        {
                            tbl.Add(url, show);
                        }

                        string systemid = gets(ObjUserPermission[i].SystemID).ToLower();
                        if (!last.Trim().Equals(systemid))
                        {
                            if ((i != 0))
                            {
                                sys.Add(last, lshow);
                            }

                            last = systemid;
                            lshow = false;
                        }
                        else if (show)
                        {
                            lshow = true;
                        }

                    }

                }
            }





            sys.Add(last, lshow);
            PermssionDetaile.Add(tbl);
            PermssionDetaile.Add(sys);








            return PermssionDetaile;
        }


        #region "Logged user Permission"



        #endregion


    }
}