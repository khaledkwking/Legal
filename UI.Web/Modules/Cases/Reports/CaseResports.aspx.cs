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

namespace UI.Web.Modules.Cases.Reports
{
    public partial class CaseResports : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public CasesRepository objRepository = IoC.Resolve<CasesRepository>();
        public string _PageTitle = " تقارير القضايا ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "CasesAttachments/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();


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
                ViewState["HearingCode"] = "0";

                Session["PersonsList"] = null;
                Session["objDefendant"] = null;
                Session["objprosecutor"] = null;

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
            ViewState["CaseitemID"] = "0";
            ViewState["ProceduresitemID"] = "0";
            Session["PersonsList"] = null;
            Response.Redirect("/Modules/Cases/Forms/CasesData.aspx");

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillReport();
        }

        #endregion

        #region "Fill Information"


        public string ShowJudgmentresult(string Judgmentresult)
        {
            if (Judgmentresult == "")
            {
                return "";

            }
            if (Judgmentresult == "1")
            {
                return "<span class=\'label label-sm label-success\'>&nbsp;لصالح&nbsp;</span>";
            }
            else if (Judgmentresult == "2")
            {
                return "<span class=\'label label-sm label-danger\'>&nbsp;ضد&nbsp;</span>";
            }
            else { return ""; }

        }
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


        private List<Case_parties> AddDefaultItems(List<Case_parties> _SourceList)
        {
            List<Case_parties> _OutList = new List<Case_parties>();

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
                    _OutList.Add(new Case_parties());
                }

            }
            else
            {

                for (int i = 0; i < _SourceList.Count; i++)
                {
                    _OutList.Add(_SourceList[i]);
                }
                //  _OutList = _SourceList;
                _OutList.Add(new Case_parties());

            }

            return _OutList;
        }
        public string ViewPrivateParty()
        {
            return getBool(ReadSession("ViewPrivate")) ? "" : "none";
        }

        private void FillReport()
        {

            string reportTitle = "تقرير القضايا ";

            var objList = objRepository.GetList(txtFilterFileNUm.Text, ZeroIntergerIFNull(txtFilterInternalSerial.Text), ZeroIntergerIFNull(txtFilterFileYear.Text), NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFiltertype.SelectedValue), ZeroIntergerIFNull(lstFilterStatus.SelectedValue),
                txtFilterSubject.Text, getBool(ReadSession("ViewPrivate")), txtFSuitType.Text, txtFSuitNum.Text, txtFSuitYear.Text,
                ZeroIntergerIFNull(lstFCaseLevel.SelectedValue), ZeroIntergerIFNull(lstFilterDession.SelectedValue), lstFilterPersonName.SelectedValue, 
                lstFiltercaseParty.SelectedValue,
                ZeroIntergerIFNull(lstFilterAssignedPerson.SelectedValue),ZeroIntergerIFNull(lstfilterJudgmentresult.SelectedValue));

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
                        case "DecisionNameAr":
                            {
                                reportTitle += "على حسب - الحكم";
                                break;
                            }
                        case "LitigationDegreeAr":
                            {
                                reportTitle += "على حسب - الدرجة ";
                                break;
                            }
                        case "Judgmentresult":
                            {
                                reportTitle += " على حسب - نتيجة الحكم";
                                break;
                            }
                        case "CaseStatusAr":
                            {
                                reportTitle += " على حسب - الحالة";
                                break;
                            }

                        default:
                            break;
                    }

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Cases/RDLC/rpt_CasesListgrouping.rdlc");
                }
                else
                { ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Cases/RDLC/rpt_CasesListgrouping.rdlc"); }


                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", reportTitle));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("username", gets(ReadSession("AdminName"))));
              
                if (getBool(ReadSession("viewWaterMark")))
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("watermarkpath", getUserWatermarkImage(gets(ReadSession("userid")), gets(ReadSession("AdminName"))), false));



                TempDT.TableName = "ds_caseslist";
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

            FillDllwithoptional_ALL(objLookup.FillRptGrouoing((int)ArcTargetModules.CasesModules), ref lstgroupping, "NameAr", "GroupingFilde", " ");
            FillDllwithoptional_ALL(objLookup.FillCasesTypes(), ref lstFiltertype, "NameAr", "Code", "الدائره");
            FillDllwithoptional_ALL(objLookup.FillCasesStatus(), ref lstFilterStatus, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillLitigationDegree(), ref lstFCaseLevel, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.Filldession(), ref lstFilterDession, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillCaseAssignedPersons(), ref lstFilterAssignedPerson, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillCaseAutopersons(), ref lstFilterPersonName, "NameAr", "NameAr", "الكل");
            FillDllwithoptional_Array(objLookup.FillCasePatires(), lstFiltercaseParty, "الكل");
            FillDllwithoptional_ALL(objLookup.FilldessionResult(), ref lstfilterJudgmentresult, "NameAr", "Code", "الكل");
        }
    }

    #endregion

}
 