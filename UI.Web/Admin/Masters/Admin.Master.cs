using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DomainInterface;
using UI.Web.Controler;
using Permission.DAL.Repository;
using System.Text;
using System.Configuration;
using System.Collections;
using Permission.DAL.Entities;
using System.Data;
using System.Runtime.Remoting.Contexts;
using Infrastructure.DAL;
using Infrastructure;
using Infrastructure.DAL.Enum;

namespace UI.Web.Admin.Masters
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        #region "Private Members"
        private Security_pr_admin user;
        public string mainMenu = "";
        public StringBuilder strmenu = new StringBuilder();

        public string AdminName = "";
        public string PrfilePhoto = "";
        //public string _NewMessagesCount = "0";
        public int _NewMessagesCount = 0;
        public QuestionsRepository objRepository = IoC.Resolve<QuestionsRepository>();
        #endregion

        #region Data Elements


        string applicationPath;
        private const string
            strLangEnglish = "en-US",
            strLangArabic = "ar-EG";

        #endregion Data Elements

        #region "Events Handlers"
        protected void Page_Init(object sender, EventArgs e)
        {
            if (MemberShip_Permission.isAuthenticationCookie())
            {
                user = MemberShipConstantUI.CurrentUser;
                ViewState["userid"] = user.id.ToString();
                Session["userid"] = user.id.ToString();

                ViewState["AdminName"] = user.name.ToString();
                Session["AdminName"] = user.name.ToString();
                Session["ViewPrivate"] = user.isOperation.ToString();
                Session["viewWaterMark"] = user.ProtectedOut.ToString();
                AdminName = user.name.ToString();
                PrfilePhoto = Resources.Utilities.Assetspath + "uploads/Adminprofile/" + user.AdminPhoto.ToString();
                FillPermissions(user.AdminType, user.id);
                ShowAlerts();
                FillMenu();
            }
            else { Response.Redirect("~/Admin/Pages/Login.aspx"); }


            //    SetModuleDefultpagelnk();

        }

        private void SetModuleDefultpagelnk()
        {
            lnk_home.HRef = Resources.Utilities.cutureRoute + "/admin/pages/home.aspx";

            if (Session["System"] != null)
            {


                Hashtable tbl = ((Hashtable)(ViewState["System"]));
                ArrayList PermitedModules = new ArrayList();
                //check if table has TRUE Key of vaild Module , if it has more than one return Defult
                //Else check Module KEY

                //Get Permited Module
                for (int i = 1; i <= tbl.Count; i++)
                {
                    if (tbl[i.ToString()] != null)
                    {
                        if (Convert.ToBoolean(tbl[i.ToString()]))
                        {
                            PermitedModules.Add(i.ToString());
                        }
                    }
                }


                var moduleKeys = tbl.Keys;

                if (PermitedModules.Count == 0 || PermitedModules.Count > 1)
                {
                    lnk_home.HRef = Resources.Utilities.cutureRoute + "/admin/pages/home.aspx";
                }
                else
                {
                    switch (PermitedModules[0].ToString())
                    {
                        case "2"://CTOS
                            {
                                lnk_home.HRef = Resources.Utilities.cutureRoute + "/Modules/CTOS/Panels/CTOS_Default.aspx";
                                break;
                            }

                        case "3"://KPI
                            {
                                lnk_home.HRef = Resources.Utilities.cutureRoute + "/Modules/RTKPI/Panels/KPI_Default.aspx";
                                break;
                            }
                        case "4"://GDB
                            {
                                lnk_home.HRef = Resources.Utilities.cutureRoute + "/Modules/GDB/Panels/GDB_Default.aspx";
                                break;
                            }

                        default:
                            lnk_home.HRef = Resources.Utilities.cutureRoute + "/admin/pages/home.aspx";
                            break;
                    }


                }
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lnk_home.HRef = Resources.Utilities.cutureRoute + "/admin/pages/home.aspx";
            SetCulture();
            string URLPath;
            applicationPath = (Request.ApplicationPath.Length == 1) ? "" : Request.ApplicationPath;

            try
            {
                //Label2.Text = Page.Title;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                URLPath = Request.AppRelativeCurrentExecutionFilePath;


                Session["URLPath"] = URLPath;

                string UrlPathName = URLPath.Replace("~", "");
                UrlPathName = UrlPathName.Replace(".aspx", "");



            }
            catch (Exception)
            {
                // Updating the Error details into Log file
                //dbErrorHandler.ErrorMessage = exp.Message;
                //objLogController.LogErrordetails(CommonProperties.User, dbErrorHandler, Request.AppRelativeCurrentExecutionFilePath + ".Page_Load", Request.ServerVariables["LOGON_USER"] + " - " + Request.UserHostAddress);
            }
        }
        #endregion

        #region "Local Methods"


        private void SetCulture()
        {
            //applicationPath = (Request.ApplicationPath.Length == 1) ? "" : Request.ApplicationPath;
            //if (Session["Language"] == null || Session["Language"].ToString() == "")
            //{
            //    Response.Redirect(applicationPath + "/Default.aspx", false);
            //    return;
            //}
            //string culture = Session["Language"].ToString();
            //System.Globalization.CultureInfo MyCltr = new System.Globalization.CultureInfo(culture);
            //System.Threading.Thread.CurrentThread.CurrentCulture = MyCltr;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = MyCltr;
            //if (Session["Language"].ToString() == strLangEnglish)
            //{
            //   // MasterPageID.Style[HtmlTextWriterStyle.Direction] = "ltr";
            //    menuTree.Style[HtmlTextWriterStyle.Direction] = "ltr";
            //    lblUserName.Text = CommonProperties.User.FirstNameEN + " " + CommonProperties.User.LastNameAR;
            //    imgGoogle.ImageUrl = "~/Images/Google.gif";
            //    imgMyprofile.ImageUrl = "~/Images/myprofile-on.gif";
            //    imgAbountUs.ImageUrl = "~/Images/aboutus-on.gif";
            //    imgContactus.ImageUrl = "~/Images/contactus-on.gif";
            //    imgHelp.ImageUrl = "~/Images/help-on.gif";
            //    imgArabic.ImageUrl = "~/Images/arabi-on.gif";

            //}
            //else
            //{
            //   // MasterPageID.Style[HtmlTextWriterStyle.Direction] = "rtl";
            //    menuTree.Style.Add(HtmlTextWriterStyle.Direction, "rtl");
            //    lblUserName.Text = CommonProperties.User.FirstNameEN + " " + CommonProperties.User.LastNameAR;
            //    imgGoogle.ImageUrl = "~/Images/Google-a.gif";
            //    imgMyprofile.ImageUrl = "~/Images/myprofile-on-a.gif";
            //    imgAbountUs.ImageUrl = "~/Images/aboutus-on-a.gif";
            //    imgContactus.ImageUrl = "~/Images/contactus-on-a.gif";
            //    imgHelp.ImageUrl = "~/Images/help-on-a.gif";
            //    imgArabic.ImageUrl = "~/Images/arabi-on-a.gif";
            //}

            //lblDateTime.Text = System.DateTime.Now.ToString("dd-MM-yyyy h:mm tt");
        }

        //public string GetmoduleDefaultTheme()
        //{
        //    string _classname = "";

        //    DataSet ds = clsModules.ins.getModuleDetailbyPage(getCurrentUrl());
        //    if (ds != null && ds.Tables[0].Rows.Count != 0)
        //    {
        //        _classname = gets(ds.Tables[0].Rows[0]["className"]);

        //        if (gets(ds.Tables[0].Rows[0]["Defult_calture"]) != "")
        //        {
        //            //////string culture = gets(ds.Tables[0].Rows[0]["Defult_calture"]);
        //            //////System.Globalization.CultureInfo MyCltr = new System.Globalization.CultureInfo(culture);
        //            //////System.Threading.Thread.CurrentThread.CurrentCulture = MyCltr;
        //            //////System.Threading.Thread.CurrentThread.CurrentUICulture = MyCltr;


        //            //string applicationPath = Request.ApplicationPath;
        //            //if (applicationPath == "/")
        //            //{
        //            //    applicationPath = string.Empty;
        //            //}
        //            //string path = Request.Url.AbsolutePath.Substring(applicationPath.Length);


        //            //string[] strArray = path.Trim(new char[] { '/' }).Split(new char[] { '/' });
        //            //string name = "en-US";
        //            //if ((strArray.Length > 1) && (strArray[0].Length > 0))
        //            //{
        //            //    if (strArray[0].ToLower().Equals("en-us") || strArray[0].ToLower().Equals("ar-kw"))
        //            //    {

        //            //        Context.RewritePath(culture + path.Replace(strArray[0], ""));
        //            //    }
        //            //    else
        //            //    { Response.Redirect("/"+culture + path); }

        //            //}

        //        }
        //    }

        //    return _classname;

        //}
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



        public void FillMenu()
        {
            if (ShowSystem("14"))
            {

                strmenu.Append("<li class='dropdown'>");
                strmenu.Append("<a href = '#' class='dropdown-toggle' data-toggle='dropdown'><i class='icon-law position-left'></i>" + Resources.menu.LawDocData + "<span class='caret'></span></a>");
                strmenu.Append("<ul class='dropdown-menu width-250'>");

                if (ShowSystem("15"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-display4'></i>  " + Resources.menu.LawDocDataMaster + "  </a>");


                    strmenu.Append("<ul class='dropdown-menu width-250'>");


                    if (ShowPage("lookups.aspx?tableName=Law_DocType"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Law_DocType'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Law_DocType + "</span></a></li>"));
                    }



                    if (ShowPage("lookups.aspx?tableName=Law_DocCategory"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Law_DocCategory'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Law_DocCategory + "</span></a></li>"));
                    }

                    if (ShowPage("lookups.aspx?tableName=Law_DocCategory"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookupsfixedCat.aspx?tableName=Law_DocCategory&refname=typeid&refvalue=3'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Law_DocCategoryMarsoom + "</span></a></li>"));
                    }
                    if (ShowPage("lookups.aspx?tableName=Law_DocCategory"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookupsfixedCat.aspx?tableName=Law_DocCategory&refname=typeid&refvalue=4'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Law_DocCategoryPrinceOrder + "</span></a></li>"));
                    }

                    if (ShowPage("lookups.aspx?tableName=law_DocProceduresTypes"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=law_DocProceduresTypes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.law_DocProceduresTypes + "</span></a></li>"));
                    }
                    if (ShowPage("lookups.aspx?tableName=law_AttachmentTypes"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=law_AttachmentTypes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.law_AttachmentTypes + " </span></a></li>"));


                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Parliament_Orgs'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                           ">" + Resources.menu.law_RelatedOrgs + " </span></a></li>"));
                    }

                    if (ShowPage("lookups.aspx?tableName=law_AttachmentTypes"))
                    {

                    }



                    //strmenu.Append("<li class='dropdown-header' style='padding:0px;margin:0px;'><hr style='padding:0px;margin:0px;'/></li>");
                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");

                }


                if (ShowSystem("20"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-drag-right'></i>  " + Resources.menu.LawsReports + "  </a>");


                    strmenu.Append("<ul class='dropdown-menu width-250'>");



                    if (ShowPage("LawDocDataReport.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/laws/reports/LawDocDataReport.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.LawDocDataReport + "  </span></a></li>"));

                    }

                    if (ShowPage("LawDessionReport.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/laws/reports/LawDessionReport.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.LawDessionReport + "  </span></a></li>"));

                    }
                    if (ShowPage("LawProjectReport.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/laws/reports/LawProjectReport.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.LawProjectReport + "  </span></a></li>"));

                    }


                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");
                }


                if (ShowPage("LawDocData.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/laws/Forms/LawDocData.aspx' ><i class='icon-law'></i>  " + Resources.menu.LawDocData + "  </a></li>");
                }

                if (ShowPage("LawDessionData.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/laws/Forms/LawDessionData.aspx' ><i class='icon-stack-text'></i>  " + Resources.menu.LawDession + "  </a></li>");
                }

                if (ShowPage("LawProjectData.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/laws/Forms/LawProjectData.aspx' ><i class='icon-display4'></i>  " + Resources.menu.LawProjectData + "  </a></li>");
                }

                strmenu.Append("</ul>");
                strmenu.Append("</li>");


            }



            if (ShowSystem("8"))
            {

                strmenu.Append("<li class='dropdown'>");
                strmenu.Append("<a href = '#' class='dropdown-toggle' data-toggle='dropdown'><i class='icon-question3 position-left'></i>" + Resources.menu.Questions + "<span class='caret'></span></a>");
                strmenu.Append("<ul class='dropdown-menu width-250'>");

                if (ShowSystem("9"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-display4'></i>  " + Resources.menu.QuestionsMaster + "  </a>");


                    strmenu.Append("<ul class='dropdown-menu width-250'>");


                    if (ShowPage("lookups.aspx?tableName=Parliament_legislativeChapter"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookupsOrder.aspx?tableName=Parliament_legislativeChapter'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Parliament_legislativeChapter + "</span></a></li>"));
                    }



                    if (ShowPage("QuestionSession.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/Questions/forms/QuestionSession.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Parliament_legislativeSession + "</span></a></li>"));
                    }
                    if (ShowPage("lookups.aspx?tableName=Parliament_AssignedPerson"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Parliament_AssignedPerson'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Parliament_AssignedPerson + "</span></a></li>"));
                    }

                    if (ShowPage("lookups.aspx?tableName=Parliament_QuestionStatus"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Parliament_QuestionStatus'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Parliament_QuestionStatus + "</span></a></li>"));
                    }


                    if (ShowPage("lookups.aspx?tableName=Parliament_QuestionResult"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Parliament_QuestionResult'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Parliament_QuestionResult + "</span></a></li>"));
                    }

                    if (ShowPage("lookups.aspx?tableName=Parliament_RequestedToPerson"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Parliament_RequestedToPerson'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Parliament_RequestedToPerson + " </span></a></li>"));
                    }

                    //if (ShowPage("lookups.aspx?tableName=arc_AttachmentsTypes"))
                    //{
                    //    strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=arc_AttachmentsTypes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                    //        ">" + Resources.menu.AgreementAttachmentTypes + " </span></a></li>"));
                    //}

                    //strmenu.Append("<li class='dropdown-header'>" + Resources.menu.Autocomp + "</li>");
                    strmenu.Append("<li class='dropdown-header' style='padding:0px;margin:0px;'><hr style='padding:0px;margin:0px;'/></li>");

                    if (ShowPage("QuestionOmaPerson.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/Questions/Forms/QuestionOmaPerson.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Parliament_Persons + " </span></a></li>"));
                    }



                    if (ShowPage("lookups.aspx?tableName=Parliament_Orgs"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Parliament_Orgs'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Parliament_Orgs + " </span></a></li>"));



                    }


                    if (ShowPage("lookups.aspx?tableName=Parliament_CMGSRelatedOrgs"))
                    {

                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Parliament_CMGSRelatedOrgs'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                           ">" + Resources.menu.Parliament_CMGSRelatedOrgs + " </span></a></li>"));

                    }

                    strmenu.Append("<li class='dropdown-header' style='padding:0px;margin:0px;'><hr style='padding:0px;margin:0px;'/></li>");

                    if (ShowPage("lookups.aspx?tableName=Complaints_Committees"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Complaints_Committees'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Complaints_Committees + " </span></a></li>"));
                    }
                    //if (ShowPage("lookups.aspx?tableName=Complaints_RelatedOrg"))
                    //{
                    //    strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Complaints_RelatedOrg'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                    //        ">" + Resources.menu.Complaints_RelatedOrg + " </span></a></li>"));
                    //}


                    strmenu.Append("</ul>");



                    strmenu.Append("</li>");

                }

                if (ShowSystem("10"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-drag-right'></i>  " + Resources.menu.QuestionsQuery + "  </a>");


                    strmenu.Append("<ul class='dropdown-menu width-250'>");



                    if (ShowPage("QuestionsReports.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/Questions/reports/QuestionsReports.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.QuestionsReports + " </span></a></li>"));
                    }

                    if (ShowPage("QuestionsReports.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/Questions/reports/QuestionsStatistics.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.QuestionsStatistics + " </span></a></li>"));
                    }


                    if (ShowPage("InterrogationReports.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><hr/><a href=\'/Modules/Questions/reports/InterrogationReports.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.InterrogationReports + " </span></a></li>"));
                    }

                    if (ShowPage("OmaMadbataReports.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><hr/><a href=\'/Modules/Questions/reports/OmaMadbataReports.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.OmaMadbataReports + " </span></a></li>"));
                    }


                    if (ShowPage("ComplaintsReport.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><hr/><a href=\'/Modules/Questions/reports/ComplaintsReport.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.ComplaintsReport + " </span></a></li>"));
                    }

                    if (ShowPage("suggestionReport.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><hr/><a href=\'/Modules/Questions/reports/suggestionReport.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.suggestionReport + " </span></a></li>"));
                    }



                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");
                }
                if (ShowPage("QuestionsData.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/Questions/Forms/QuestionsData.aspx' ><i class='icon-question3'></i>  " + Resources.menu.QuestionsData + "  </a></li>");
                }

                if (ShowPage("QuestionsData.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/Questions/Forms/InterrogationData.aspx' ><i class='icon-files-empty'></i>  " + Resources.menu.InterrogationData + "  </a></li>");
                }

                if (ShowPage("QuestionsData.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/Questions/Forms/OmaMadbata.aspx' ><i class='icon-question3'></i>  " + Resources.menu.QuestionsMadbata + "  </a></li>");
                }
                if (ShowPage("suggestionData.aspx"))
                {

                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/Questions/Forms/suggestionData.aspx' ><i class='icon-flip-vertical3'></i>  " + Resources.menu.suggestionData + "  </a></li>");
                }

                if (ShowPage("complaintsData.aspx"))
                {

                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/Questions/Forms/complaintsData.aspx' ><i class='icon-pencil3'></i>  " + Resources.menu.complaintsData + "  </a></li>");
                }



                strmenu.Append("</ul>");
                strmenu.Append("</li>");


            }

            if (ShowSystem("23"))
            {

                strmenu.Append("<li class='dropdown'>");
                strmenu.Append("<a href = '#' class='dropdown-toggle' data-toggle='dropdown'><i class='icon-notebook position-left'></i>" + Resources.menu.LegalMemo + "<span class='caret'></span></a>");
                strmenu.Append("<ul class='dropdown-menu width-250'>");

                if (ShowSystem("22"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-display4'></i>  " + Resources.menu.LegalMemoMaster + "  </a>");


                    strmenu.Append("<ul class='dropdown-menu width-250'>");


                    //if (ShowPage("lookups.aspx?tableName=LegalMemo_Category"))
                    //{
                    //    strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=LegalMemo_Category'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                    //        ">" + Resources.menu.LegalMemo_Category + "</span></a></li>"));
                    //}


                    if (ShowPage("lookups.aspx?tableName=LegalMemo_OrgCat"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=LegalMemo_OrgCat'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.LegalMemo_OrgCat + "</span></a></li>"));
                    }

                    if (ShowPage("LegalMemoOrg.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/LegalMemos/Forms/LegalMemoOrg.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.LegalMemo_Org + "</span></a></li>"));
                    }

                    if (ShowPage("lookups.aspx?tableName=LegalMemo_Procedure"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=LegalMemo_Procedure'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.LegalMemo_Procedure + "</span></a></li>"));
                    }
                    if (ShowPage("lookups.aspx?tableName=LegalMemo_AssignedPersons"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=LegalMemo_AssignedPersons'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.LegalMemo_AssignedPersons + "</span></a></li>"));
                    }
                    if (ShowPage("lookups.aspx?tableName=LegalMemo_Status"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=LegalMemo_Status'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.LegalMemo_Status + "</span></a></li>"));
                    }
                    //if (ShowPage("lookups.aspx?tableName=LegalMemo_Consultant"))
                    //{
                    //    strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=LegalMemo_Consultant'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                    //        ">" + Resources.menu.LegalMemo_Consultant + "</span></a></li>"));
                    //}


                    //if (ShowPage("lookups.aspx?tableName=law_AttachmentTypes"))
                    //{
                    //    strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=law_AttachmentTypes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                    //        ">" + Resources.menu.law_AttachmentTypes + " </span></a></li>"));


                    //    strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Parliament_Orgs'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                    //       ">" + Resources.menu.law_RelatedOrgs + " </span></a></li>"));
                    //}





                    //strmenu.Append("<li class='dropdown-header' style='padding:0px;margin:0px;'><hr style='padding:0px;margin:0px;'/></li>");
                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");

                }


                //if (ShowSystem("20"))
                //{
                //    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-drag-right'></i>  " + Resources.menu.LawsReports + "  </a>");


                //    strmenu.Append("<ul class='dropdown-menu width-250'>");



                //    if (ShowPage("LawDocDataReport.aspx"))
                //    {
                //        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/laws/reports/LawDocDataReport.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                //            ">" + Resources.menu.LawDocDataReport + "  </span></a></li>"));

                //    }

                //    if (ShowPage("LawDessionReport.aspx"))
                //    {
                //        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/laws/reports/LawDessionReport.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                //            ">" + Resources.menu.LawDessionReport + "  </span></a></li>"));

                //    }
                //    if (ShowPage("LawProjectReport.aspx"))
                //    {
                //        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/laws/reports/LawProjectReport.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                //            ">" + Resources.menu.LawProjectReport + "  </span></a></li>"));

                //    }


                //    strmenu.Append("</ul>");
                //    strmenu.Append("</li>");
                //}

                if (ShowSystem("24"))
                {

                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-drag-right'></i>  " + Resources.menu.LegalMemoReport + "  </a>");
                    strmenu.Append("<ul class='dropdown-menu width-250'>");
                    strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/LegalMemos/reports/LegalMemoReport.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'>" + Resources.menu.LegalMemoReportList + "  </span></a></li>"));
                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");

                }


                if (ShowPage("LegalMemoData.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/LegalMemos/Forms/LegalMemoData.aspx' ><i class='icon-law'></i>  " + Resources.menu.LegalMemo + "  </a></li>");

                }

                strmenu.Append("</ul>");
                strmenu.Append("</li>");


            }
            //if (ShowSystem("6"))
            //{

            //    strmenu.Append("<li class='dropdown'>");
            //    strmenu.Append("<a href = '#' class='dropdown-toggle' data-toggle='dropdown'><i class='icon-briefcase position-left'></i>" + Resources.menu.Cases + "<span class='caret'></span></a>");
            //    strmenu.Append("<ul class='dropdown-menu width-250'>");

            //    if (ShowSystem("7"))
            //    {
            //        strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-display4'></i>  " + Resources.menu.CasesMaster + "  </a>");


            //        strmenu.Append("<ul class='dropdown-menu width-250'>");


            //        //  strmenu.Append("<li class='dropdown-header'>" + Resources.menu.CasesMasterData + "</li>");


            //        //' Shared master Filtes

            //        //if (ShowPage("lookups.aspx?tableName=Medal_M_Category"))
            //        //{
            //        //    strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Medal_M_Category'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //        //        ">" + Resources.menu.Medal_M_Category + "</span></a></li>"));
            //        //}

            //        //if (ShowPage("lookups.aspx?tableName=Cases_M_Court"))
            //        //    {
            //        //        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Cases_M_Court'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //        //            ">" + Resources.menu.Cases_M_Court + "</span></a></li>"));
            //        //    }

            //        if (ShowPage("lookups.aspx?tableName=Cases_M_Court"))
            //        {
            //            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Case_LitigationDegree'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //                ">" + Resources.menu.Case_LitigationDegree + "</span></a></li>"));
            //        }


            //        //if (ShowPage("lookups.aspx?tableName=Cases_M_CaseTypes"))
            //        //{
            //        //    strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Cases_M_CaseTypes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //        //        ">" + Resources.menu.Cases_M_CaseTypes + "</span></a></li>"));
            //        //}


            //        if (ShowPage("lookups.aspx?tableName=Cases_M_CaseTypes"))
            //        {
            //            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/Cases/Forms/CaseTypes.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //                ">" + Resources.menu.Cases_M_CaseTypes + "</span></a></li>"));
            //        }



            //        if (ShowPage("lookups.aspx?tableName=Cases_M_Status"))
            //        {
            //            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Cases_M_Status'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //                ">" + Resources.menu.Cases_M_Status + "</span></a></li>"));
            //        }

            //        if (ShowPage("lookups.aspx?tableName=Cases_decision"))
            //        {
            //            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Cases_decision'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //                ">" + Resources.menu.Cases_decision + "</span></a></li>"));
            //        }
            //        if (ShowPage("lookups.aspx?tableName=Cases_M_DecisionResut"))
            //        {
            //            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Cases_M_DecisionResut'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //                ">" + Resources.menu.Cases_M_DecisionResut + "</span></a></li>"));
            //        }

            //        if (ShowPage("lookups.aspx?tableName=Case_AssignedPersons"))
            //        {
            //            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Case_AssignedPersons'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //                ">" + Resources.menu.AssignedPersons + " </span></a></li>"));
            //        }
            //        if (ShowPage("lookups.aspx?tableName=arc_AttachmentsTypes"))
            //        {
            //            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=arc_AttachmentsTypes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //                ">" + Resources.menu.AgreementAttachmentTypes + " </span></a></li>"));
            //        }

            //        //strmenu.Append("<li class='dropdown-header'>" + Resources.menu.Autocomp + "</li>");
            //        strmenu.Append("<li class='dropdown-header' style='padding:0px;margin:0px;'><hr style='padding:0px;margin:0px;'/></li>");

            //        if (ShowPage("lookups.aspx?tableName=Case_AutoSubject"))
            //        {
            //            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Case_AutoSubject'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //                ">" + Resources.menu.Case_AutoSubject + " </span></a></li>"));
            //        }
            //        if (ShowPage("lookups.aspx?tableName=Case_AutoPersons"))
            //        {
            //            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Case_AutoPersons'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //                ">" + Resources.menu.Case_AutoPersons + " </span></a></li>"));
            //        }
            //        if (ShowPage("lookups.aspx?tableName=Case_AutoArcSubjects"))
            //        {
            //            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Case_AutoArcSubjects'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //                ">" + Resources.menu.Case_AutoArcSubjects + " </span></a></li>"));
            //        }

            //        strmenu.Append("</ul>");



            //        strmenu.Append("</li>");

            //    }


            //    if (ShowSystem("18"))
            //    {
            //        strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-drag-right'></i>  " + Resources.menu.CasesReport + "  </a>");


            //        strmenu.Append("<ul class='dropdown-menu width-250'>");



            //        if (ShowPage("CaseResports.aspx"))
            //        {
            //            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/Cases/reports/CaseResports.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
            //                ">" + Resources.menu.CaseResportsList + "  </span></a></li>"));

            //        }


            //        strmenu.Append("</ul>");
            //        strmenu.Append("</li>");
            //    }

            //    if (ShowPage("CasesData.aspx"))
            //    {
            //        strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/Cases/Forms/CasesData.aspx' ><i class='icon-calendar'></i>  " + Resources.menu.CaseMenu + "  </a></li>");
            //    }


            //    strmenu.Append("</ul>");
            //    strmenu.Append("</li>");


            //}
            if (ShowSystem("26"))
            {

                strmenu.Append("<li class='dropdown'>");
                strmenu.Append("<a href = '#' class='dropdown-toggle' data-toggle='dropdown'><i class='icon-briefcase position-left'></i>" + Resources.menu.Committee + "<span class='caret'></span></a>");
                strmenu.Append("<ul class='dropdown-menu width-250'>");

                if (ShowSystem("25"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-display4'></i>  " + Resources.menu.CommitteeMaster + "  </a>");
                    strmenu.Append("<ul class='dropdown-menu width-250'>");

                    if (ShowPage("lookups.aspx?tableName=Committees_RefList"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Committees_RefList'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Committees_RefList + "</span></a></li>"));
                    }

                    if (ShowPage("lookups.aspx?tableName=Committees_Ministers"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Committees_Ministers'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Committees_Ministers + "</span></a></li>"));
                    }

                   
                    if (ShowPage("lookups.aspx?tableName=Committees_ProceduresTypes"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Committees_ProceduresTypes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Committees_ProceduresTypes + "</span></a></li>"));
                    }

                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");
                }

                if (ShowSystem("27"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-drag-right'></i>  " + Resources.menu.Committeesreports + "  </a>");


                    strmenu.Append("<ul class='dropdown-menu width-250'>");

                    if (ShowPage("CommitteeReport.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/Committees/reports/CommitteeReport.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.CommitteeReport + "  </span></a></li>"));

                    }

                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");
                }


                if (ShowPage("CommitteeData.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/Committees/Forms/CommitteeData.aspx' ><i class='icon-book3'></i>  " + Resources.menu.CommitteeData + "  </a></li>");
                }
                strmenu.Append("</ul>");
                strmenu.Append("</li>");
            }

          



            if (ShowSystem("17"))
            {

                strmenu.Append("<li class='dropdown'>");
                strmenu.Append("<a href = '#' class='dropdown-toggle' data-toggle='dropdown'><i class='icon-envelop2 position-left'></i>" + Resources.menu.pm + "<span class='caret'></span></a>");
                strmenu.Append("<ul class='dropdown-menu width-250'>");

                if (ShowSystem("18"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-display4'></i>  " + Resources.menu.pmMaster + "  </a>");
                    strmenu.Append("<ul class='dropdown-menu width-250'>");
                    if (ShowPage("lookups.aspx?tableName=Library_DocsCategory"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Pm_Letters_Categories'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Pm_DocsCategory + "</span></a></li>"));
                    }

                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");
                }


                if (ShowSystem("21"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-drag-right'></i>  " + Resources.menu.pmreports + "  </a>");


                    strmenu.Append("<ul class='dropdown-menu width-250'>");

                    if (ShowPage("Frm_Pm_LettersReports.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/pm/reports/Frm_Pm_LettersReports.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Frm_Pm_LettersReports + "  </span></a></li>"));

                    }

                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");
                }


                if (ShowPage("Frm_Pm_Letters.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/pm/Forms/Frm_Pm_Letters.aspx' ><i class='icon-popout'></i>  " + Resources.menu.pmLetters + "  </a></li>");
                }
                strmenu.Append("</ul>");
                strmenu.Append("</li>");
            }
            if (ShowSystem("3"))
            {

                strmenu.Append("<li class='dropdown'>");
                strmenu.Append("<a href = '#' class='dropdown-toggle' data-toggle='dropdown'><i class='icon-make-group position-left'></i>" + Resources.menu.Agreements + " <span class='caret'></span></a>");
                strmenu.Append("<ul class='dropdown-menu width-250'>");


                //strmenu.Append("<li class='dropdown-header'>" + Resources.menu.GDBAnalytics + "</li>");

                if (ShowSystem("2"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-display4'></i>  " + Resources.menu.AgreementMaster + "  </a>");


                    strmenu.Append("<ul class='dropdown-menu width-250'>");


                    //strmenu.Append("<li class='dropdown-header'>" + Resources.menu.AgreementsMasters + "</li>");
                    // strmenu.Append("<li class='dropdown-header' style='padding:0px;margin:0px;'><hr style='padding:0px;margin:0px;'/></li>");


                    //' Shared master Filtes
                    if (ShowPage("lookups.aspx?tableName=Agreement_TypeCodes"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Agreement_TypeCodes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Agreementtypes + "</span></a></li>"));
                    }




                    if (ShowPage("lookups.aspx?tableName=Agreement_Category"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Agreement_Category'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.AgreementCategory + " </span></a></li>"));
                    }


                    if (ShowPage("lookups.aspx?tableName=Agreement_Orgs"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Agreement_Orgs'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Org + " </span></a></li>"));
                    }


                    if (ShowPage("lookups.aspx?tableName=Agreement_ProcedureRelatedOrgs"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Agreement_ProcedureRelatedOrgs'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Agreement_ProcedureRelatedOrgs + " </span></a></li>"));
                    }

                    if (ShowPage("lookups.aspx?tableName=Agreement_ProcedureTypes"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Agreement_ProcedureTypes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Agreement_ProcedureTypes + " </span></a></li>"));
                    }

                    if (ShowPage("lookups.aspx?tableName=AgreementAttachmentTypes"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=AgreementAttachmentTypes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.AgreementAttachmentTypes + " </span></a></li>"));
                    }

                    if (ShowPage("lookups.aspx?tableName=Agreement_StatusCodes"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Agreement_StatusCodes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.StatusCodes + " </span></a></li>"));
                    }

                    if (ShowPage("lookups.aspx?tableName=Agreement_AssignedPersons"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Agreement_AssignedPersons'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Agreement_AssignedPersons + " </span></a></li>"));
                    }
                    strmenu.Append("</ul>");



                    strmenu.Append("</li>");

                }

                if (ShowSystem("13"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-drag-right'></i>  " + Resources.menu.AgreementsReports + "  </a>");


                    strmenu.Append("<ul class='dropdown-menu width-250'>");



                    if (ShowPage("AgreementsReport.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/Agreements/reports/AgreementsReport.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Agreementslistreport + "  </span></a></li>"));

                    }

                    if (ShowPage("AgreementsReport.aspx"))
                    {

                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/Agreements/reports/AgreementsStatistics.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                               ">" + Resources.menu.AgreementsStatistics + " </span></a></li>"));
                    }
                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");
                }


                if (ShowPage("AgreementsData.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/Agreements/Forms/AgreementsData.aspx' ><i class='icon-calendar'></i>  " + Resources.menu.AgreementsData + "  </a></li>");
                }
                if (ShowSystem("4"))
                {


                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-stars position-left'></i> " + Resources.menu.Medal + " </a>");
                    strmenu.Append("<ul class='dropdown-menu width-250'>");

                    if (ShowSystem("5"))
                    {
                        strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-display4'></i>  " + Resources.menu.MedalMasterData + "  </a>");


                        strmenu.Append("<ul class='dropdown-menu width-250'>");



                        if (ShowPage("lookups.aspx?tableName=Medal_M_Types"))
                        {
                            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Medal_M_Types'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                                ">" + Resources.menu.Medal_M_Types + "</span></a></li>"));
                        }




                        if (ShowPage("lookups.aspx?tableName=Medal_M_Organizations"))
                        {
                            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Medal_M_Organizations'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                                ">" + Resources.menu.Medal_M_Organizations + " </span></a></li>"));
                        }


                        //if (ShowPage("lookups.aspx?tableName=medal_M_Grantreasons"))
                        //{
                        //    strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=medal_M_Grantreasons'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                        //        ">" + Resources.menu.medal_M_Grantreasons + " </span></a></li>"));
                        //}

                        if (ShowPage("lookups.aspx?tableName=Medal_M_jobGrade"))
                        {
                            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Medal_M_jobGrade'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                                ">" + Resources.menu.Medal_M_jobGrade + " </span></a></li>"));
                        }

                        if (ShowPage("lookups.aspx?tableName=Medal_M_ProcedureType"))
                        {
                            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Medal_M_ProcedureType'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                                ">" + Resources.menu.Medal_M_ProcedureType + " </span></a></li>"));
                        }

                        if (ShowPage("lookups.aspx?tableName=Medal_M_AttachmentTypes"))
                        {
                            strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Medal_M_AttachmentTypes'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                                ">" + Resources.menu.Medal_M_AttachmentTypes + " </span></a></li>"));
                        }


                        strmenu.Append("</ul>");



                        strmenu.Append("</li>");

                    }

                    if (ShowPage("MedalsData.aspx"))
                    {
                        strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/Medals/Forms/MedalsData.aspx' ><i class='icon-calendar'></i>  " + Resources.menu.MedalsData + "  </a></li>");
                    }


                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");


                }

                strmenu.Append("</ul>");
                strmenu.Append("</li>");


            }

            if (ShowSystem("12"))
            {

                strmenu.Append("<li class='dropdown'>");
                strmenu.Append("<a href = '#' class='dropdown-toggle' data-toggle='dropdown'><i class='icon-archive position-left'></i>" + Resources.menu.Library + "<span class='caret'></span></a>");
                strmenu.Append("<ul class='dropdown-menu width-250'>");

                if (ShowSystem("11"))
                {
                    strmenu.Append("<li class='dropdown-submenu' ><a href='#' ><i class='icon-display4'></i>  " + Resources.menu.LibraryMaster + "  </a>");
                    strmenu.Append("<ul class='dropdown-menu width-250'>");
                    if (ShowPage("lookups.aspx?tableName=Library_DocsCategory"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookupswithImage.aspx?tableName=Library_DocsCategory'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Library_DocsCategory + "</span></a></li>"));
                    }
                    if (ShowPage("LibraryDocsType.aspx"))
                    {
                        strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/LibraryDocs/Forms/LibraryDocsType.aspx'><i class=\'fa fa-angle-left\'></i><span class=\'submenu-title\'" +
                            ">" + Resources.menu.Library_DocsType + "</span></a></li>"));
                    }

                    strmenu.Append("</ul>");
                    strmenu.Append("</li>");
                }

                if (ShowPage("LibraryDocs.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/LibraryDocs/Forms/LibraryDocs.aspx' ><i class='icon-book2'></i>  " + Resources.menu.LibraryDocs + "  </a></li>");
                }
                if (ShowPage("LibraryDocs.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/LibraryDocs/Forms/LibraryDocsview3.aspx' ><i class='icon-book3'></i>  " + Resources.menu.LibraryDocs2 + "  </a></li>");
                }
                strmenu.Append("</ul>");
                strmenu.Append("</li>");
            }

       

            if (ShowSystem("1"))
            {
                strmenu.Append("<li class='dropdown'>");
                strmenu.Append("<a href = '#' class='dropdown-toggle' data-toggle='dropdown'><i class='icon-users'></i> " + Resources.menu.UserManagement + " <span class='caret'></span></a>");
                strmenu.Append("<ul class='dropdown-menu width-250'>");


                //  strmenu.Append("<li class='dropdown-header'>"+Resources.menu.PortalAccess + "</li>");

                if (ShowPage("lookups.aspx?tableName=Security_pr_Department"))
                {
                    strmenu.Append(("\r\n" + "  <li><a href=\'/Modules/MasterData/lookups.aspx?tableName=Security_pr_Department'><i class=\'icon-files-empty\'></i><span class=\'submenu-title\'" +
                        ">" + Resources.menu.Security_pr_Department + "</span></a></li>"));
                }


                if (ShowPage("AdminManager.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/admin/pages/AdminManager.aspx' ><i class='icon-user-lock'></i> " + Resources.menu.AccessUsersList + "</a></li>");
                }
                if (ShowPage("ModuleManager.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/admin/pages/ModuleManager.aspx' ><i class='icon-popout'></i> " + Resources.menu.ModulesList + "</a></li>");
                }

                if (ShowPage("Frm_UserOperaions.aspx"))
                {
                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/audit/Forms/Frm_UserOperaions.aspx' ><i class='icon-popout'></i> " + Resources.menu.Useroperations + "</a></li>");

                    strmenu.Append("<li ><a href='" + Resources.Utilities.cutureRoute + "/Modules/audit/Forms/Frm_UserOperaionAudit.aspx' ><i class='icon-popout'></i> " + Resources.menu.UseroperationsAudit + "</a></li>");
                }



                if (ShowPage("PermissionsNew.aspx"))
                {
                    strmenu.Append(("\r\n" + "  <li><a href='" + Resources.Utilities.cutureRoute + "/admin/pages/PermissionsNew.aspx\'><i class='icon-lock2'></i> " + Resources.menu.userPermission + "</a></li>"));
                }

                strmenu.Append("</ul>");
                strmenu.Append("</li>");
            }

        }
        public string FIllPublicAnnouncements()
        {
            string _out = "";
            //string CompanyID = "0";
            //if ((!(Session("CompanyID") == null)
            //            && !Session("CompanyID").Equals("0"))) {
            //    CompanyID = Session("CompanyID");
            //}

            //DataSet ds = NewsData.ins.GetItems(CompanyID, ,, "1");
            //if ((!(ds == null)
            //            && !(ds.Tables[0].Rows.Count == 0))) {
            //    for (int i = 0; (i
            //                <= (ds.Tables[0].Rows.Count - 1)); i++) {
            //        _out = (_out + ("\r\n" + ("<li>"
            //                    + (gets(ds.Tables[0].Rows[i]["titleEn"]) + "</li>"))));
            //    }

            //}
            //else {
            //    _out = (_out + ("\r\n" + " <li>Welcome to �Portal - Restaurant Management System</li>"));
            //    _out = (_out + ("\r\n" + "  <li>You can manage your business in simple way .</li>"));
            //}
            //////_out +=" <li>Welcome to   Portal - Port Management System</li>";
            //////  _out +="<li>You can manage your business in simple way .</li>";
            return _out;
        }

        private void FillPermissions(int? adminType, int adminID)
        {
            ArrayList UserPermssionDetails = MemberShip_Permission.ins.SetUserPermission(adminType, adminID);


            Hashtable tbl = new Hashtable();
            Hashtable sys = new Hashtable();

            ViewState["Permission"] = (Hashtable)UserPermssionDetails[0];
            Session["Permission"] = (Hashtable)UserPermssionDetails[0];
            ViewState["System"] = (Hashtable)UserPermssionDetails[1];
            Session["System"] = (Hashtable)UserPermssionDetails[1];
        }

        public string ShowAlerts()
        {
            string _outList = "";

            var objList = objRepository.ShowQuestionsAlerts();
            if ((objList != null) && objList.Count > 0)
            {
                _NewMessagesCount += objList.Count;

                foreach (var item in objList)
                {
                    _outList += "<li class='media'>";
                    _outList += "<div class='media-left'>";
                    _outList += "<a href = '/Modules/Questions/Forms/QuestionsData.aspx?QuestionID=" + item.RefDocID + (item.Doc_Type == 1 ? "&activetab=3" : "&activetab=4") + "' class='btn " + (item.Doc_Type == 1 ? "border-primary text-primary" : "border-warning text-warning") + " btn-flat btn-rounded btn-icon btn-sm'>";

                    if (item.Doc_Type == 1)
                    { _outList += "<i class='icon-file-download'></i>"; }
                    else { _outList += "<i class='icon-file-upload'></i>"; }

                    _outList += "</a>";
                    _outList += "</div>";
                    _outList += "<div class='media-body'>" + item.Doc_Subject;
                    _outList += " <div class='media-annotation'>" + item.NextFollowReminderDate.Value.ToShortDateString() + "</div>";
                    _outList += "</div>";
                    _outList += "</li>";
                }



            }

            var objListArcAlert = objRepository.ShowArcAlert((int)ArcTargetModules.LegalMemo);

            if ((objListArcAlert != null) && objListArcAlert.Count > 0)
            {
                _NewMessagesCount += objListArcAlert.Count;

                foreach (var item in objListArcAlert)
                {
                    _outList += "<li class='media'>";
                    _outList += "<div class='media-left'>";
                    _outList += "<a href = '/Modules/LegalMemos/Forms/LegalMemoData.aspx?targetCode=" + item.Code + "&LawDocID=" + item.RefDocID + (item.Doc_Type == 1 ? "&activetab=3" : "&activetab=4") + "' class='btn " + (item.Doc_Type == 1 ? "border-primary text-primary" : "border-warning text-warning") + " btn-flat btn-rounded btn-icon btn-sm'>";

                    if (item.Doc_Type == 1)
                    { _outList += "<i class='icon-file-download'></i>"; }
                    else { _outList += "<i class='icon-file-upload'></i>"; }

                    _outList += "</a>";
                    _outList += "</div>";
                    _outList += "<div class='media-body'>" + item.Doc_Subject;
                    _outList += " <div class='media-annotation'>" + item.NextFollowReminderDate.Value.ToShortDateString() + "</div>";
                    _outList += "</div>";
                    _outList += "</li>";
                }



            }
            return _outList;


        }


        protected string gets(object obj)
        {
            if ((obj == DBNull.Value))
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }

        }

        public bool ShowPage(string url)
        {
            Hashtable tbl = ((Hashtable)(ViewState["Permission"]));
            url = url.ToLower();
            //if ((url.IndexOf("?") != -1))
            //{
            //    url = url.Substring(0, url.IndexOf("?"));
            //}

            if ((tbl[url] == null))
            {
                return false;
            }

            return bool.Parse(tbl[url].ToString());
        }

        public bool ShowSystem(string systemid)
        {
            Hashtable tbl = ((Hashtable)(ViewState["System"]));
            if ((tbl[systemid] == null))
            {
                return false;
            }

            return bool.Parse(tbl[systemid].ToString());
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

        protected void lnkEnglish_Click(object sender, EventArgs e)
        {

            Response.Redirect("/" + Resources.Utilities.langrouter + Request.Url.PathAndQuery);

        }

        protected void lnkArabic_Click(object sender, EventArgs e)
        {

            Response.Redirect("/" + Resources.Utilities.langrouter + Request.Url.PathAndQuery);
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
        #endregion
    }
}