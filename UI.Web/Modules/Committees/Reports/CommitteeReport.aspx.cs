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

namespace UI.Web.Modules.Committees.Reports
{
    public partial class CommitteeReport : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public CommitteeRepository objRepository = IoC.Resolve<CommitteeRepository>();
        public string _PageTitle = "نظام المجالس واللجان العليا ومجالس إدارات الجهات الحكومية  ";




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
                 

                if (Request.QueryString["ss"] != null && Request.QueryString["ss"].ToString() == "1")
                {

                    string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                }



                fillLookups();

               // FillCommittees();



                ViewState["SpChapterChanged"] = "0";
                ViewState["SpEdit"] = "0";
                ViewState["NewDesc"] = "";
                ViewState["NewBar"] = "";
                ViewState["NewIsbn"] = "";
                ViewState["SPITEM"] = "";
                ViewState["NewPrice"] = "0";
                Session["ItemList"] = null;
                ViewState["itemID"] = "0";
                ViewState["CommitteeitemID"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["CommitteeArcID"] = "0";
                ViewState["AttachitemID"] = "0";
                ViewState["ProcedureCode"] = "0";

                ViewState["linkedAgreement"] = "0";


                if (Request.QueryString["CommitteeID"] == null)
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
                   


                    ViewState["itemID"] = Request.QueryString["CommitteeID"].ToString();
                   

                }

                SetPageTitle();

                ViewState["OutboundItemID"] = "0";

               
 
            }

        }
        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            FillCommittees();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillCommittees();
        }
        #endregion

        #region "Fill Information"
        private string MapSearchKeys()
        {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                //_keyList.Add(" رقم الوثيقة ", txtFilterSerialNum.Text);
                //_keyList.Add("   سنة الاصدار   ", txtFilterSerialYear.Text);
                //_keyList.Add(" تاريخ  إصدار الوثيقة من ", txtFilterDatefrom.Text);
                //_keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                //_keyList.Add(" جزء من نص الوثيقة    ", txtFilterDetails.Text);
                //_keyList.Add(" العمل التحضيري", lstfilterProceduretype.SelectedItem.Text);
                //_keyList.Add(" نوع الوثيقة  ", lstFilterType.SelectedItem.Text);
                //_keyList.Add(" قيد الدراسة    ", lstFilterIsUnderStudy.SelectedItem.Text);
                //_keyList.Add(lblFilterCatTitle.Text, lstFilterCategory.SelectedItem.Text);
                //_keyList.Add("  جزء من الموضوع ", txtFilterSubject.Text);
                //_keyList.Add("نشر بالجريدة الرسمية", lstFilterPublish.Text);

                //_keyList.Add(" تاريخ  انتهاء الوثيقة من ", txtFilterExpireFrom.Text);
                //_keyList.Add(" الي   تاريخ  انتهاء  ", txtFilterExpireTo.Text);

            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }
        private void FillCommittees()
        {
            string reportTitle = "تقرير المجالس واللجان العليا ومجالس إدارات الجهات الحكومية ";


            var objList = objRepository.GetList(ZeroIntergerIFNull(txtFilterSerialNum.Text), 0,
                 NullDateifEmpty(txtFilterDatefrom.Text), NullDateifEmpty(txtFilterDateTo.Text), txtFilterSubject.Text, txtFilterDetails.Text, "",
                NullDateifEmpty(txtFilterExpireFrom.Text), NullDateifEmpty(txtFilterExpireTo.Text),ZeroIntergerIFNull(lstfilterminister.SelectedValue),
                ZeroIntergerIFNull(lstfilterProcedureType.SelectedValue), ZeroIntergerIFNull(lstFilterCommittee.SelectedValue),0, MapSearchKeys());


            
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
                        case "MinisterNameAr":
                            {
                                reportTitle += "على حسب - الوزير  ";
                                break;
                            }
                        case "LastProcedureID":
                            {
                                reportTitle += "على حسب - الاداة القانونية ";
                                break;
                            }
                         

                        default:
                            break;
                    }

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Committees/RDLC/rpt_CommitteeListgrouping.rdlc");
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("groupfield", lstgroupping.SelectedValue));
                }
                else
                { ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Committees/RDLC/rpt_CommitteeList.rdlc"); }

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", reportTitle));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("username", gets(ReadSession("AdminName"))));

                if (getBool(ReadSession("viewWaterMark")))
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("watermarkpath", getUserWatermarkImage(gets(ReadSession("userid")), gets(ReadSession("AdminName"))), false));



                TempDT.TableName = "ds_Committee";
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

        protected void lnkSearchback_Click(object sender, EventArgs e)
        {
            tblSearch.Visible = true;
            tblshow.Visible = false;
        }
        private void SetPageTitle()
        {
            if (Request.QueryString["d"] != null)
            {
                // lblSubTitle.Text = "Deposit Goods";

            }


        }

        #endregion

        #region "Helper Methods"
        private void fillLookups()
        {


            FillDllwithoptional_ALL(objLookup.fillMinisters(), ref lstfilterminister, "NameAr", "Code", "الكل");



            FillDllwithoptional_ALL(objLookup.fillCommittees_ProceduresTypes(), ref lstfilterProcedureType, "NameAr", "Code", "الكل");

            //FillDllwithoptional_ALL(objLookup.FillCommittees_DocProceduresTypes(), ref lstprocedureType, "NameAr", "Code", "إختر");
            //FillDllwithoptional_ALL(objLookup.FillCommittees_DocProceduresTypes(), ref lstfilterProceduretype, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillRptGrouoing((int)ArcTargetModules.Committee), ref lstgroupping, "NameAr", "GroupingFilde", " ");
            FillDllwithoptional_ALL(objLookup.FillCommitteeList(), ref lstFilterCommittee, "NameAr", "Code", "الكل");


        }
        #endregion
 
    }
}