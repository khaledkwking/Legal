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
using Newtonsoft.Json;

namespace UI.Web.Modules.Laws.Reports
{
    public partial class LawProjectReport : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();
        public AgreementsRepository agreemtnyRepository = IoC.Resolve<AgreementsRepository>();
        


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LawsAttachments/";

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
                ViewState["LawDocitemID"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["LawDocArcID"] = "0";
                ViewState["AttachitemID"] = "0";
                ViewState["ProcedureCode"] = "0";

                ViewState["linkedAgreement"] = "0";

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

            tblshow.Visible = false;
            tblSearch.Visible = false;
            ViewState["LawDocitemID"] = "0";
            ViewState["ProceduresitemID"] = "0";
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

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
            ViewState["LawDocitemID"] = "0";
            ViewState["ProceduresitemID"] = "0";
             Session["PersonsList"] = null;
            // Response.Redirect("/Modules/laws/Forms/LawDocData.aspx");
            FillReport();

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillReport();
        }

        
        #endregion

        #region "Helper Methods"

        private string MapSearchKeys()
        {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                _keyList.Add(" رقم مرسوم الاحالة ", txtFilterSerialNum.Text);
                _keyList.Add("   السنة     ", txtFilterSerialYear.Text);
                _keyList.Add(" تاريخ  الاحالة     ", txtFilterDatefrom.Text);
                _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                _keyList.Add(" جزء من نص الوثيقة    ", txtFilterDetails.Text);
                _keyList.Add(" نوع الوثيقة  ", lstFilterType.SelectedItem.Text);
                _keyList.Add(" إسترداد      ", lstFilterIsUnderStudy.SelectedItem.Text);
                _keyList.Add("  جزء من الموضوع ", txtFilterSubject.Text);
 
            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }


        private void FillReport()
        {

            string reportTitle = " تقرير مشروعات قوانين محالة ";

            //Get Selectd Persons

            var objList = objRepository.GetlawProjects(ZeroIntergerIFNull(txtFilterSerialNum.Text), ZeroIntergerIFNull(txtFilterSerialYear.Text),
                 NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterType.SelectedValue), 0,
               ZeroIntergerIFNull(lstFilterIsUnderStudy.SelectedValue), txtFilterSubject.Text, txtFilterDetails.Text,
                ZeroIntergerIFNull(lstfilterProceduretype.SelectedValue), MapSearchKeys(),ZeroIntergerIFNull(lstFilterTransfere.SelectedValue));


            lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));

            if (objList.Count > 0)
            {
                //btnSave.Visible = true;
                //lnkBack.Visible = true;

                tblshow.Visible = true;
                tblSearch.Visible = false;

                DataTable TempDT = ConvertHelper.ToDataTable(objList);
                ReportViewer1.LocalReport.EnableExternalImages = true;


                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/laws/RDLC/rpt_lawProjectdocs.rdlc");  

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", reportTitle));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("username", gets(ReadSession("AdminName"))));

                if (getBool(ReadSession("viewWaterMark")))
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("watermarkpath", getUserWatermarkImage(gets(ReadSession("userid")), gets(ReadSession("AdminName"))), false));



                TempDT.TableName = "ds_LawDocsList";
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
        private void fillLookups()
        {
             FillDllwithoptional(objLookup.FillLaw_project(),  lstFilterType, "NameAr", "Code");
            FillDllwithoptional_ALL(objLookup.Filllaw_DocProceduresTypes(), ref lstfilterProceduretype, "NameAr", "Code","الكل");

        }



        #endregion



    }
}