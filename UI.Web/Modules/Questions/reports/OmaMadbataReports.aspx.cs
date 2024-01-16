using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using Infrastructure.DAL.Enum;
using UI.Web.Admin.Controller;
using Utilities;

namespace UI.Web.Modules.Questions.Reports
{
    public partial class OmaMadbataReports : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public MadbataRepository objRepository = IoC.Resolve<MadbataRepository>();
        public string _PageTitle = "نظام مضابط مجلس الامة     ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "Parliament_madbata/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        public string selectedChapter = "0";
        public string QrelatedOrg = "";
        #endregion

        #region "Page Events"

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "QuestionsData.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";

            if (!IsPostBack)
            {
                fillLookups();
                 

                 ViewState["SpChapterChanged"] = "0";
                ViewState["SpEdit"] = "0";
                ViewState["NewDesc"] = "";
                ViewState["NewBar"] = "";
                ViewState["NewIsbn"] = "";
                ViewState["SPITEM"] = "";
                ViewState["NewPrice"] = "0";
                Session["ItemList"] = null;
                ViewState["itemID"] = "0";
                ViewState["MadbataItemID"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["MadbataArcID"] = "0";
                ViewState["AttachitemID"] = "0";
                ViewState["AnswerCode"] = "0";
               

                if (Request.QueryString["MadbataID"] == null)
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
                    ViewState["itemID"] = Request.QueryString["MadbataID"].ToString();
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
       
      
        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            FillReport();
        }

        protected void lnkSearchback_Click(object sender, EventArgs e)
        {
            tblSearch.Visible = true;
            tblshow.Visible = false;
        }

     

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
         
            tblSearch.Visible = true;
        
         
            ViewState["SpEdit"] = "0";
            ViewState["NewDesc"] = "";
            ViewState["NewBar"] = "";
            ViewState["NewIsbn"] = "";
            ViewState["SPITEM"] = "";
            ViewState["NewPrice"] = "0";
            Session["ItemList"] = null;
            ViewState["itemID"] = "0";
            ViewState["itemID"] = "0";
            ViewState["MadbataItemID"] = "0";
            ViewState["ProceduresitemID"] = "0";
             Session["PersonsList"] = null;
            Response.Redirect("/Modules/Questions/Forms/OmaMadbata.aspx");

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillReport();
        }
      
 
        //}
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

      
        private List<Parliament_Requestedby> AddDefaultItems(List<Parliament_Requestedby> _SourceList)
        {
            List<Parliament_Requestedby> _OutList = new List<Parliament_Requestedby>();

            int TargetCount = 5;
            int _RoundCount = TargetCount - _SourceList.Count;

            if (_RoundCount > 0)
            {
                //  _OutList = _SourceList;
                for (int i = 0; i < _SourceList.Count; i++)
                {
                    _OutList.Add(_SourceList[i]);
                }

                for (int i = 0; i < _RoundCount; i++)
                {
                    _OutList.Add(new Parliament_Requestedby());
                }

            }
            else
            {

                //  _OutList = _SourceList;
                for (int i = 0; i < _SourceList.Count; i++)
                {
                    _OutList.Add(_SourceList[i]);
                }

                //  _OutList = _SourceList;
                _OutList.Add(new Parliament_Requestedby());

            }

            return _OutList;
        }

        private void FillReport()
        {
            string reportTitle = "تقرير مضابط مجلس الامة  ";

            var objList = objRepository.GetList((txtFilterserial.Text),   NullDateifEmpty(txtFilterDatefrom.Text), 
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterChapter.SelectedValue), ZeroIntergerIFNull(lstFilterSession.SelectedValue) );




            if (objList.Count > 0)
            {
                //btnSave.Visible = true;
                //lnkBack.Visible = true;

                tblshow.Visible = true;
                tblSearch.Visible = false;

                DataTable TempDT = ConvertHelper.ToDataTable(objList);

                ReportViewer1.LocalReport.EnableExternalImages = true;


                if (lstgroupping.SelectedValue != "0")
                {
                    for (int i = 0; i < TempDT.Rows.Count; i++)
                    {
                        //TempDT.Rows[i]["GroupName"] = TempDT.Rows[i]["fld_name_en"];
                        TempDT.Rows[i]["GroupName"] = TempDT.Rows[i][lstgroupping.SelectedValue];
                    }

                    switch (lstgroupping.SelectedValue)
                    {
                        case "M_date":
                            {
                                reportTitle += "على حسب - تاريخ الجلسة ";
                                break;
                            }
                       
                        case "ChapterNameAr":
                            {
                                reportTitle += " على حسب - الفصل التشريعي";
                                break;
                            }
                        case "SessionNameAr":
                            {
                                reportTitle += " على حسب - دور الانعقاد";
                                break;
                            }

                        default:
                            break;
                    }

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Questions/RDLC/rpt_MadbataListgrouping.rdlc");
                }
                else
                { ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Questions/RDLC/rpt_MadbataList.rdlc"); }

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", reportTitle));

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("username", gets(ReadSession("AdminName"))));

                if (getBool(ReadSession("viewWaterMark")))
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("watermarkpath", getUserWatermarkImage(gets(ReadSession("userid")), gets(ReadSession("AdminName"))), false));




                TempDT.TableName = "ds_MadbataList";
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(TempDT.TableName, TempDT));


            }
            else
            {
                tblshow.Visible = false;

                tblSearch.Visible = true;
                string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }

        }
       
        #endregion

         #region "Helper Methods"
         
        private void fillLookups()
        {
            FillDllwithoptional_ALL(objLookup.FillRptGrouoing((int)ArcTargetModules.Madbata), ref lstgroupping, "NameAr", "GroupingFilde", " ");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeChapter(), ref lstFilterChapter , "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterSession , "NameAr", "Code", "الكل");

        }
 
        #endregion
         
        protected void lstFilterChapter_SelectedIndexChanged(object sender, EventArgs e)
        {

            selectedChapter = lstFilterChapter.SelectedValue;
            ViewState["SpChapterChanged"] = "1";
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterSession, "NameAr", "Code", "الكل");
 
             
        }

        
       

      
    }
}