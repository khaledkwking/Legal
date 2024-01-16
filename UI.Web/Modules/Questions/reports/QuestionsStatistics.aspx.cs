using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using UI.Web.Admin.Controller;
using Utilities;

namespace UI.Web.Modules.Questions.Forms
{
    public partial class QuestionsStatistics : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public QuestionsRepository objRepository = IoC.Resolve<QuestionsRepository>();
        public string _PageTitle = "نظام الأسئلة البرلمانية   ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "QuestionsAttachments/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();


        #endregion

        #region "Page Events"

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "QuestionsReports.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";



            if (!IsPostBack)
            {


                if (Request.QueryString["ss"] != null && Request.QueryString["ss"].ToString() == "1")
                {

                    string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                }



                fillLookups();


                if (Request.QueryString["ProTypeID"] != null)
                {
                    //lstFilterProcedures.SelectedValue = gets(Request.QueryString["ProTypeID"]);
                    //FillQuestions();

                }


                ViewState["SpEdit"] = "0";
                ViewState["NewDesc"] = "";
                ViewState["NewBar"] = "";
                ViewState["NewIsbn"] = "";
                ViewState["SPITEM"] = "";
                ViewState["NewPrice"] = "0";
                Session["ItemList"] = null;
                ViewState["itemID"] = "0";
                ViewState["CaseitemID"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["CasesArcID"] = "0";
                ViewState["AttachitemID"] = "0";
                ViewState["AnswerCode"] = "0";


                if (Request.QueryString["QuestionID"] == null)
                {

                    // Added Temporary Consignee 
                    //string script = FormatpopupErrorMSG(Resources.Alerts.SorryFailToretriveData + " Query string Missing", "1");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    ViewState["itemID"] = "0";
                    //return;

                }
                else
                {
                    tblshow.Visible = false;
                    tblSearch.Visible = false;



                    ViewState["itemID"] = Request.QueryString["QuestionID"].ToString();

                    //Fill Agreemnt Details


                }


                ViewState["OutboundItemID"] = "0";


            }

        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            { Response.Redirect("OutboundOperrations.aspx?id=" + Request.QueryString["id"].ToString()); }
            else
            { Response.Redirect("OutboundOperrations.aspx"); }

        }

        protected void btnNew_Click1(object sender, EventArgs e)
        {
            Session["objprosecutor"] = null;
            Session["objDefendant"] = null;

            tblshow.Visible = false;
            tblSearch.Visible = false;
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            FillQuestions();
        }

        protected void lnkSearchback_Click(object sender, EventArgs e)
        {
            tblSearch.Visible = true;
            tblshow.Visible = false;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillQuestions();
        }

        #endregion

        #region "Fill Information"
        public string fillDocType(string DocType)
        {
            string _out = "";
            if (DocType.Equals("1"))
            {
                _out = "<img src='/Layout/Assets/images/coming.png' alt='Coming'  />";
            }
            else if (DocType.Equals("2"))
            {
                _out = "<img src='/Layout/Assets/images/outgoing.png' alt='outgoing'  />";
            }


            return _out;

        }

        private void FillQuestions()
        {

            var objList = objRepository.FillStatistics(ZeroIntergerIFNull(lstFilterChapter.SelectedValue), ZeroIntergerIFNull(lstFilterSession.SelectedValue), ZeroIntergerIFNull(lstFilterRequestTo.SelectedValue));

            if (objList.Count > 0)
            {

                tblshow.Visible = true;
                tblSearch.Visible = false;


            }
            else
            {
                tblshow.Visible = false;

                tblSearch.Visible = true;
                string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }

            DataTable TempDT = ConvertHelper.ToDataTable(objList);

            ReportViewer1.LocalReport.EnableExternalImages = true;


            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Questions/RDLC/rpt_QuestionsStatistics.rdlc");
            string _ReportTitle = "";
            if (lstFilterRequestTo.SelectedValue == "0" || lstFilterRequestTo.SelectedValue == "")
            {
                _ReportTitle = " إحصائية مفصلة للأسئلة البرلمانية بأسماء أعضاء مجلس الأمة  ";
            }
            else {
                _ReportTitle = "إحصائية مفصلة للأسئلة البرلمانية بأسماء أعضاء مجلس الأمة الموجهة ل"+ lstFilterRequestTo.SelectedItem.Text ;
            }


            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", _ReportTitle));

            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("username", gets(ReadSession("AdminName"))));

            if (getBool(ReadSession("viewWaterMark")))
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("watermarkpath", getUserWatermarkImage(gets(ReadSession("userid")), gets(ReadSession("AdminName"))), false));

            TempDT.TableName = "view_QuestionsStatistics";
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(TempDT.TableName, TempDT));
        }

        private string GetTitle(bool isadd)
        {
            if (isadd)
            {
                return "Add New Record Information";
            }
            else
            {
                return "Edit Record Information";
            }

        }

        #endregion

        #region "Helper Methods"

        private void fillLookups()
        {
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeChapter(), ref lstFilterChapter, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterSession, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillParliament_RequestedToPerson(), ref lstFilterRequestTo, "NameAr", "Code", "الكل");
        }

        #endregion

        protected void lstFilterChapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterSession, "NameAr", "Code", "الكل");

        }


    }
}