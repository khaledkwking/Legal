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
using Infrastructure.DAL.Enum;
using UI.Web.Admin.Controller;
using Utilities;
using Newtonsoft.Json;

namespace UI.Web.Agreements.reports
{
    public partial class AgreementsReport : BaseFormAdmin
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
        private string MapSearchKeys()
        {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                if (ZeroIntergerIFNull(txtFilterNum.Text) > 0)
                { _keyList.Add("مسلسل", txtFilterNum.Text); }

                if (ZeroIntergerIFNull(txtFilterYear.Text) > 0)
                { _keyList.Add("السنة", txtFilterYear.Text); }
                if (txtFilterDatefrom.Text != "")
                { _keyList.Add("تاريخ الاتفاقية من ", txtFilterDatefrom.Text); }

                if (txtFilterDateTo.Text != "")
                { _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text); }

                if (ZeroIntergerIFNull(lstFilterType.SelectedValue) > 0)
                { _keyList.Add("نوع الاتفاقية", lstFilterType.SelectedItem.Text); }

                if (ZeroIntergerIFNull(lstFilterCats.SelectedValue) > 0)
                { _keyList.Add("تصنيف الاتفاقية", lstFilterCats.SelectedItem.Text); }


                if (ZeroIntergerIFNull(lstFilterOrg.SelectedValue) > 0)
                { _keyList.Add("الجهة/الدولة  ", lstFilterOrg.SelectedItem.Text); }

                if (ZeroIntergerIFNull(lstFilterProcedures.SelectedValue) > 0)
                { _keyList.Add("الإجراء ", lstFilterProcedures.SelectedItem.Text); }

                if (txtPartofName.Text != "")
                { _keyList.Add(" جزء من موضوع الاتفاقية ", txtPartofName.Text); }

                if (ZeroIntergerIFNull(lstFilterStatus.SelectedValue) > 0)
                { _keyList.Add("حالة الاتفاقية ", lstFilterStatus.SelectedItem.Text); }

                if (ZeroIntergerIFNull(lstFilterRelatedOrgs.SelectedValue) > 0)
                { _keyList.Add("محال الي جهة الإختصاص   ", lstFilterRelatedOrgs.SelectedItem.Text); }

                if (ZeroIntergerIFNull(lstFilterAgrRelatedOrgs.SelectedValue) > 0)
                { _keyList.Add("  جهة الإختصاص", lstFilterAgrRelatedOrgs.SelectedItem.Text); }


            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }

        private void FillReport()
        {
            string reportTitle = "تقرير الاتفاقيات ";

            //   Session["ViewPrivate"]
            //var objList = objRepository.GetList(ZeroIntergerIFNull(txtFilterNum.Text), ZeroIntergerIFNull(txtFilterYear.Text),
            //    NullDateifEmpty(txtFilterDatefrom.Text), NullDateifEmpty(txtFilterDateTo.Text),
            //    ZeroIntergerIFNull(lstFilterType.SelectedValue), ZeroIntergerIFNull(lstFilterCats.SelectedValue),
            //    ZeroIntergerIFNull(lstFilterOrg.SelectedValue), ZeroIntergerIFNull(lstFilterStatusCode.SelectedValue),
            //    ZeroIntergerIFNull(lstFilterProcedures.SelectedValue), txtPartofName.Text,
            //    getBool(ReadSession("ViewPrivate")),new List<int>());


            var objList = objRepository.GetList(ZeroIntergerIFNull(txtFilterNum.Text),
              ZeroIntergerIFNull(txtFilterYear.Text), NullDateifEmpty(txtFilterDatefrom.Text),
              NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterType.SelectedValue),
              ZeroIntergerIFNull(lstFilterCats.SelectedValue), ZeroIntergerIFNull(lstFilterOrg.SelectedValue),
             0, ZeroIntergerIFNull(lstFilterProcedures.SelectedValue),
              txtPartofName.Text, getBool(ReadSession("ViewPrivate")), new List<int>(),
              lstFilterStatus.SelectedValue, ZeroIntergerIFNull(lstFilterRelatedOrgs.SelectedValue),
              ZeroIntergerIFNull(lstFilterAgrRelatedOrgs.SelectedValue), MapSearchKeys(),ZeroIntergerIFNull(lstFilterAssignedPerson.SelectedValue));


            var objRemovedParent = objList.Where(x => x.ParentID != 0).ToList();
            var results = objList.Except(objRemovedParent).ToList();


            // var duplicatedList = objList.SelectMany(t =>
            //Enumerable.Repeat(t, 2)).ToList();

            if (results.Count > 0)
            {


                tblshow.Visible = true;
                tblSearch.Visible = false;

                DataTable TempDT = ConvertHelper.ToDataTable(results);
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
                        case "AgreementOrgNameAr":
                            {
                                reportTitle += " على حسب الجهة - الدولة";
                                break;
                            }
                        case "AgreementTypeNameAr":
                            {
                                reportTitle += " على حسب نوع الاتفاقية";
                                break;
                            }
                        case "AgreementCatNameAr":
                            {
                                reportTitle += " على حسب تصنيف الاتفاقية";
                                break;
                            }
                        case "StatusNameAr":
                            {
                                reportTitle += " على حسب الإجراء";
                                break;
                            }

                        default:
                            break;
                    }

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Agreements/RDLC/rpt_AgreementsList.rdlc");
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("groupingfield", lstgroupping.SelectedValue));
                }
                else
                { ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Agreements/RDLC/rpt_AgreementsListwithoutGroup.rdlc"); }


                if (getBool(ReadSession("viewWaterMark")))
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("watermarkpath", getUserWatermarkImage(gets(ReadSession("userid")), gets(ReadSession("AdminName"))), false));

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", reportTitle));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("username", gets(ReadSession("AdminName"))));

                

                TempDT.TableName = "ds_AgreementsList";
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(TempDT.TableName, TempDT));

                ExportPdf(ReportViewer1,"Agreement_"+ ReadSession("userid")+"_"+DateTime.Now.ToShortDateString());
                 

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
            FillDllwithoptional_ALL(objLookup.FillAgreementTypes(), ref lstFilterType, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillAgreementCategories(), ref lstFilterCats, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillOrganization(), ref lstFilterOrg, "NameAr", "Code", "الكل");
            //   FillDllwithoptional_ALL(objLookup.FillStatusCode(), ref lstFilterStatusCode, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillProcedureTypes(), ref lstFilterProcedures, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillRptGrouoing((int)ArcTargetModules.AgreementModules), ref lstgroupping, "NameAr", "GroupingFilde", " ");
            FillDllwithoptional_ALL(objLookup.FillAgreementRelatedOrgs(), ref lstFilterRelatedOrgs, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillAgreementRelatedOrgs(), ref lstFilterAgrRelatedOrgs, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillCaseAssignedPersons(), ref lstFilterAssignedPerson, "NameAr", "Code", "الكل");
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