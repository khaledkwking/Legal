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

namespace UI.Web.Modules.Questions.Reports
{
    public partial class InterrogationReports : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public QuestionsRepository objRepository = IoC.Resolve<QuestionsRepository>();
        public string _PageTitle = "تقارير الاستجوابات   ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "QuestionsAttachments/";
        public string _MadbataTargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "Parliament_madbata/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        public string selectedChapter = "0";
        public string QrelatedOrg = "";
        #endregion

        #region "Page Events"
        public string viewMadabata()
        {
            return ViewState["itemID"] != null && ViewState["itemID"].ToString() != "0"  ? "": "none";
        }

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


                if (Request.QueryString["StatusID"] != null)
                {
                    lstFilterResult.SelectedValue = gets(Request.QueryString["StatusID"]);
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
                ViewState["QuestionitemID"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["QuestionArcID"] = "0";
                ViewState["AttachitemID"] = "0";
                ViewState["AnswerCode"] = "0";

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



        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillReport();
        }



        #endregion

        #region "Fill Information"
        public string viewMadbatafile(string filename)
        {
            return gets(filename).Equals("") ? "none" : "";
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
        private string MapSearchKeys()
        {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                _keyList.Add(" مسلسل   ", txtFilterserial.Text);

                _keyList.Add(" تاريخ  ورود الاستجواب من", txtFilterDatefrom.Text);
                _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                _keyList.Add(" تاريخ  جلسة المناقشة  من  ", txtFilterQ_DiscussionDateFrom.Text);
                _keyList.Add(" الي تاريخ   ", txtFilterQ_DiscussionDateTo.Text);


                _keyList.Add("رقم الصادر بمجلس الامة  ", txtFilterInternalSerial.Text);

                _keyList.Add("السنة  ", txtFilterFileYear.Text);
                _keyList.Add("  جزء من نص الاستجواب  ", txtFilterSubject.Text);
                _keyList.Add("إستجواب موحد ", chkFilerIsGroup.Checked ? "true" : "false");
                _keyList.Add(" الفصل التشريعي ", lstFilterChapter.SelectedItem.Text);
                _keyList.Add("    العضو مقدم الاستجواب ", lstFilterRelatedOrgs.SelectedItem.Text);
                _keyList.Add("    دور الانعقاد ", lstFilterSession.SelectedItem.Text);
                _keyList.Add("المستجوب", lstFilterRequestTo.SelectedItem.Text);
                _keyList.Add("  الموظف المختص ", lstFilterAssignedPerson.SelectedItem.Text);
                _keyList.Add("    نتيجة الاستجواب ", lstFilterResult.SelectedItem.Text);
            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }
        private void FillReport()
        {
            string reportTitle = "تقرير الاستجوابات ";

            List<int> selectedRequestedFrom= new List<int>();
            if (hdnfilterRequestedFrom.Value != "" && hdnfilterRequestedFrom.Value != "0")
            {
                string[] selected = hdnfilterRequestedFrom.Value.Split(',');
                for (int i = 0; i < selected.Length; i++)
                {
                    if (ZeroIntergerIFNull(selected[i]) != 0)
                    {
                        selectedRequestedFrom.Add(ZeroIntergerIFNull(selected[i]));
                    }

                }
            }


            //Get Selectd Persons

            var objList = objRepository.GetList((int)Questions_TypesEnum.Interrogation, ZeroIntergerIFNull(txtFilterserial.Text),  (txtFilterInternalSerial.Text), ZeroIntergerIFNull(txtFilterFileYear.Text) , NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterChapter.SelectedValue), ZeroIntergerIFNull(lstFilterSession.SelectedValue),
                0, selectedRequestedFrom,
                ZeroIntergerIFNull(lstFilterRequestTo.SelectedValue),txtFilterSubject.Text, ZeroIntergerIFNull(lstFilterRelatedOrgs.SelectedValue)
                ,ZeroIntergerIFNull(lstFilterAssignedPerson.SelectedValue), ZeroIntergerIFNull(lstFilterResult.SelectedValue),
                NullDateifEmpty(txtFilterQ_DiscussionDateFrom.Text), NullDateifEmpty(txtFilterQ_DiscussionDateTo.Text),false, MapSearchKeys());



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
                        case "Q_RequestToNameAr":
                            {
                                reportTitle += "على حسب - الوزير المستجوب";
                                break;
                            }
                        case "ResultNameAr":
                            {
                                reportTitle += "على حسب - النتيجة ";
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

                    
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Questions/RDLC/rpt_InterrrogationListgrouping.rdlc");
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("groupingfield", lstgroupping.SelectedValue));
                }
                else
                { ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Questions/RDLC/rpt_InterrrogationList.rdlc"); }

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", reportTitle));

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("username", gets(ReadSession("AdminName"))));

                if (getBool(ReadSession("viewWaterMark")))
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("watermarkpath", getUserWatermarkImage(gets(ReadSession("userid")), gets(ReadSession("AdminName"))), false));



                TempDT.TableName = "ds_QuestionsList";
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

            FillDllwithoptional_ALL(objLookup.FillRptGrouoing((int)ArcTargetModules.Interrogation), ref lstgroupping, "NameAr", "GroupingFilde", " ");


            FillDllwithoptional_ALL(objLookup.FillParliament_AssignedPerson(), ref lstFilterAssignedPerson, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillParliament_QuestionResult(), ref lstFilterResult, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeChapter(), ref lstFilterChapter , "NameAr", "Code", "الكل");
             FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterSession , "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillParliament_RequestedToPerson(), ref lstFilterRequestTo, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull( lstFilterChapter.SelectedValue)), ref lstFilterPerson, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterRelatedOrgs, "NameAr", "Code", "الكل");
            var relatedOrgs = objLookup.FillParliament_RelatedOrgs();
            Session["relatedOrgs"] = relatedOrgs;

        }

        #endregion

        protected void lstFilterChapter_SelectedIndexChanged(object sender, EventArgs e)
        {

            selectedChapter = lstFilterChapter.SelectedValue;
            ViewState["SpChapterChanged"] = "1";
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterSession, "NameAr", "Code", "الكل");

          // FillQuestionPersons(ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));

            FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterPerson, "NameAr", "Code", "الكل");
        }



    }
}