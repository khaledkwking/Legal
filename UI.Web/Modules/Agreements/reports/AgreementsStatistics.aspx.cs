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

namespace UI.Web.Agreements.reports
{
    public partial class AgreementsStatistics : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public AgreementsRepository objRepository = IoC.Resolve<AgreementsRepository>();
        public string _PageTitle = "الاتفاقيات ";
        public string AgreementCode = "0";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/";
        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();



        #endregion

        #region "Page Events"


        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "AgreementsReport.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";

            if (!IsPostBack)
            {


                fillLookups();

                if (Request.QueryString["ProTypeID"] != null)
                {
                    lstFilterProcedures.SelectedValue = gets(Request.QueryString["ProTypeID"]);
                    FillReport();

                }



                if (Request.QueryString["id"] != null)
                {
                    ViewState["itemID"] = Request.QueryString["id"].ToString();

                }
                else if (Request.Form["id"] != null)
                {
                    ViewState["itemID"] = Request.Form["id"].ToString();
                }


            }

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
            tblshow.Visible = false;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillReport();
        }

        //}
        #endregion

        #region "Fill Information"


        private void FillReport()
        {
            string reportTitle = " الاتفاقيات والمعاهدات الدولية";



            var objList = objRepository.GetList(0,
              ZeroIntergerIFNull(txtFilterYear.Text), NullDateifEmpty(""),
              NullDateifEmpty(""), 0, 0,0, 0, ZeroIntergerIFNull(lstFilterProcedures.SelectedValue),"", getBool(ReadSession("ViewPrivate")), new List<int>(), "", ZeroIntergerIFNull(lstFilterRelatedOrgs.SelectedValue), 0, "",0);

            var objRemovedParent = objList.Where(x => x.ParentID != 0).ToList();
            var results = objList.Except(objRemovedParent).ToList();


            if (results.Count > 0)
            {


                tblshow.Visible = true;
                tblSearch.Visible = false;

                DataTable TempDT = ConvertHelper.ToDataTable(results);
                ReportViewer1.LocalReport.EnableExternalImages = true;


                reportTitle +=   txtFilterYear.Text != "" ? (" لسنة " + txtFilterYear.Text) : "";

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Agreements/RDLC/rpt_AgreementsStatistics.rdlc");

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", reportTitle));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("username", gets(ReadSession("AdminName"))));

                if (getBool(ReadSession("viewWaterMark")))
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("watermarkpath", getUserWatermarkImage(gets(ReadSession("userid")), gets(ReadSession("AdminName"))), false));





                TempDT.TableName = "ds_Agreements";
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
            FillDllwithoptional_ALL(objLookup.FillProcedureTypes(), ref lstFilterProcedures, "NameAr", "Code", "الكل");

             FillDllwithoptional_ALL(objLookup.FillAgreementRelatedOrgs(), ref lstFilterRelatedOrgs, "NameAr", "Code", "الكل");

        }




        #endregion

        protected void lstFilterProcedures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFilterProcedures.SelectedValue == "3")//الاحاله لجهة الاختصاص
            {
                divFilterRelatedOrg.Visible = true;
            }
            else
            {
                divFilterRelatedOrg.Visible = false;
                try
                {
                    lstFilterRelatedOrgs.SelectedValue = "0";
                }
                catch (Exception)
                {


                }

            }
        }
    }
}