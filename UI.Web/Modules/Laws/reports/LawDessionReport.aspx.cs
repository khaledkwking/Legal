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
    public partial class LawDessionReport : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();
        public string _PageTitle = "نظام القرارات  ";


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
            if (getBool(ReadSession("ViewPrivate")))
            {
                privateFiles.Visible = true;
            }
            else { privateFiles.Visible = false; }

            if (!IsPostBack)
            {


                fillLookups();


                if (Request.QueryString["docTypeId"] != null)
                {
                    lstFilterType.SelectedValue = gets(Request.QueryString["docTypeId"]);
                   FillReport();

                }

                if (Request.QueryString["ispublish"] != null)
                {
                    lstFilterPublish.SelectedValue = "1";
                    FillReport();

                }

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
            ViewState["LawDocitemID"] = "0";
            ViewState["ProceduresitemID"] = "0";
             Session["PersonsList"] = null;
            // Response.Redirect("/Modules/laws/Forms/LawDessionData.aspx");
            FillReport();

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillReport();
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
        private string MapSearchKeys()
        {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                _keyList.Add("  مسلسل القرار   ", txtFilterSerialNum.Text);
                _keyList.Add("  رقم القرار", txtFilterSerialNum.Text);
                _keyList.Add(" سنة الاصدار  ", txtFilterSerialYear.Text);


                _keyList.Add("تاريخ  إصدار القرار من ", txtFilterDatefrom.Text);
                _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                _keyList.Add(" نوع القرار   ", lstFilterType.SelectedItem.Text);
                _keyList.Add(" نوع الوثيقة  ", lstFilterType.SelectedItem.Text);
                _keyList.Add(" قيد الدراسة    ", lstFilterIsUnderStudy.SelectedItem.Text);
                _keyList.Add("   جزء من نص القرار  ", txtFilterSubject.Text);
                _keyList.Add("نشر بالجريدة الرسمية", lstFilterPublish.Text);

            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }

        private void FillReport()
        {

            string reportTitle = " تقرير القرارات ";


            var objList = objRepository.GetDecisionsList(ZeroIntergerIFNull(txtFilterSerialNum.Text),
                ZeroIntergerIFNull(txtFilterSerialYear.Text),NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterType.SelectedValue),
                ZeroIntergerIFNull(lstFilterCategory.SelectedValue),
               ZeroIntergerIFNull(lstFilterIsUnderStudy.SelectedValue ),
               txtFilterSubject.Text, txtFilterDetails.Text,
               ZeroIntergerIFNull(lstFilterPublish.SelectedValue),txtFilterSerial.Text, MapSearchKeys(),0,
               getBool(ReadSession("ViewPrivate")),ZeroIntergerIFNull(lstFilterPrivate.SelectedValue));


            lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));



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
                        case "DocYear":
                            {
                                reportTitle += "على حسب - السنة  ";
                                break;
                            }

                        case "Law_DocTypeNameAr":
                            {
                                reportTitle += " على حسب - نوع القرار ";
                                break;
                            }
                        case "law_DocProceduresTypesNameAr":
                            {
                                reportTitle += " على حسب -   الاجراء";
                                break;
                            }

                        default:
                            break;
                    }

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/laws/RDLC/rpt_Dessionlawdocsgrouping.rdlc");
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("groupfield", lstgroupping.SelectedValue));


                }
                else
                { ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/laws/RDLC/rpt_Dessionlawdocs.rdlc"); }


                if (getBool(ReadSession("viewWaterMark")))
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("watermarkpath", getUserWatermarkImage(gets(ReadSession("userid")), gets(ReadSession("AdminName"))), false));




                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", reportTitle));

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("username", gets(ReadSession("AdminName"))));


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


        #endregion

         #region "Helper Methods"



        private void fillLookups()
        {
            FillDllwithoptional_ALL(objLookup.FillRptGrouoing((int)ArcTargetModules.DecisionModules), ref lstgroupping, "NameAr", "GroupingFilde", " ");

            FillDllwithoptional_ALL(objLookup.FillLaw_DessionType(), ref lstFilterType, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(), ref lstFilterCategory, "NameAr", "Code", "الكل");

        }

        #endregion


    }
}