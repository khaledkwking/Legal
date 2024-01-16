using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
using Newtonsoft.Json;
using System.Data;
using Utilities;

namespace UI.Web.Modules.LegalMemos.Reports
{
    public partial class LegalMemoReport : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LegalMemoRepository objRepository = IoC.Resolve<LegalMemoRepository>();
        public string _PageTitle = "  مذكرات الرأي القانوني  ";

        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LegalMemos/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        public string selectedChapter = "0";
        public string QrelatedOrg = "";
        #endregion

        #region "Page Events"

        
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "LegalMemoData.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";
            if (!IsPostBack)
            { 
                fillLookups();
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
            //Dictionary<string, string> _keyList = new Dictionary<string, string>();
            //try
            //{
            //    _keyList.Add("  مسلسل القرار   ", txtFilterSerialNum.Text);
            //    _keyList.Add("  رقم القرار", txtFilterSerialNum.Text);
            //    _keyList.Add(" سنة الاصدار  ", txtFilterSerialYear.Text);


            //    _keyList.Add("تاريخ  إصدار القرار من ", txtFilterDatefrom.Text);
            //    _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
            //    _keyList.Add(" نوع القرار   ", lstFilterType.SelectedItem.Text);
            //    _keyList.Add(" نوع الوثيقة  ", lstFilterType.SelectedItem.Text);
            //    _keyList.Add(" قيد الدراسة    ", lstFilterIsUnderStudy.SelectedItem.Text);
            //    _keyList.Add("   جزء من نص القرار  ", txtFilterSubject.Text);
            //    _keyList.Add("نشر بالجريدة الرسمية", lstFilterPublish.Text);

            //}
            //catch (Exception)
            //{

            //    return "";
            //}


            //return JsonConvert.SerializeObject(_keyList);
            return "";
        }

   


        private void FillReport()
        {

            string reportTitle = "تقرير  مذكرات الرأي القانوني ";
            var objList = objRepository.GetList(txtFilterAutoNum.Text, ZeroIntergerIFNull(txtFilterSerialNum.Text), ZeroIntergerIFNull(txtFilterSerialYear.Text),
                NullDateifEmpty(txtFilterDatefrom.Text),NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterCategory.SelectedValue), 
                ZeroIntergerIFNull(lstfilterOrg.SelectedValue),ZeroIntergerIFNull(lstfilterAssignedEmployee.SelectedValue), ZeroIntergerIFNull(lstfilterProcedure.SelectedValue),
                         ZeroIntergerIFNull(lstFilterIsUnderStudy.SelectedValue), txtFilterSubject.Text, MapSearchKeys(), NullDateifEmpty(txtFilterFollowDateFrom.Text),
                          NullDateifEmpty(txtFilterFollowDateTo.Text), ZeroIntergerIFNull(lstFilterStatus.SelectedValue), ZeroIntergerIFNull(lstFilterConsultant.SelectedValue));

            if (objList.Count > 0)
            {


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
                        case "OrgId":
                            {
                                reportTitle += "على حسب - الجهة";
                                break;
                            }
                        case "DocStatusId":
                            {
                                reportTitle += "على حسب - الحالة ";
                                break;
                            }
                        case "assignedEmp":
                            {
                                reportTitle += " على حسب - الموظف المختص ";
                                break;
                            }
                        case "ProcedureId":
                            {
                                reportTitle += " على حسب - الاجراء";
                                break;
                            }

                        default:
                            break;
                    }

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/LegalMemos/RDLC/rpt_LegalMemogrouping.rdlc");
                }
                else
                { ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/LegalMemos/RDLC/rpt_LegalMemogrouping.rdlc"); }


                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", reportTitle));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("username", gets(ReadSession("AdminName"))));

                if (getBool(ReadSession("viewWaterMark")))
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("watermarkpath", getUserWatermarkImage(gets(ReadSession("userid")), gets(ReadSession("AdminName"))), false));



                TempDT.TableName = "ds_LegalMemos";
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
        public string showattachment(string hasattachment)
        {
            if (hasattachment != "")
            {
                return "";
            }
            return "display:none";

        }
        private void SetPageTitle()
        {
            if (Request.QueryString["d"] != null)
            {
                // lblSubTitle.Text = "Deposit Goods";

            }


        }

        private void ClearForm()
        {


            //ViewState["itemID"] = "0";
            //txtfilnum.Text = "";
            // txtMedalNotes.Text = "";
            //txtMedalDate.Text = "";


            //BlblSubTitle.Text = this.GetTitle(true);
        }
        #endregion

        #region "Helper Methods"
        private void fillLookups()
        {
            FillDllwithoptional_ALL(objLookup.FillRptGrouoing((int)ArcTargetModules.LegalMemo), ref lstgroupping, "NameAr", "GroupingFilde", " ");
            FillDllwithoptional_ALL(objLookup.fillLLegalMemo_Category(), ref lstFilterCategory, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.fill_LegalMemo_Procedure(), ref lstfilterProcedure, "NameAr", "Code", "");
            FillDllwithoptional_ALL(objLookup.fill_LegalMemo_Org(), ref lstfilterOrg, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.fill_LegalMemo_AssignedPersons(), ref lstfilterAssignedEmployee, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillLegalMemoStatus(), ref lstFilterStatus, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.fillConsultant(), ref lstFilterConsultant, "NameAr", "Code", "الكل");

        }
        #endregion
        protected void grdOutgoing_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType==ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (Request.QueryString["targetCode"] !=null)
                {
                    if (ZeroIntergerIFNull( Request.QueryString["targetCode"]) ==ZeroIntergerIFNull( e.Item.Cells[0].Text))
                    {
                        //e.Item.Attributes.Add("backgroundColor", "this.style.backgroundColor=\'#f2d575\';");
                        e.Item.BackColor = System.Drawing.Color.Yellow;

                    }

                    }

            }
        }
    }
}