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

namespace UI.Web.Modules.PM.Reports
{
    public partial class Frm_Pm_LettersReports : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public PmLettersRepository objRepository = IoC.Resolve<PmLettersRepository>();
        public string _PageTitle = "نظام كتب ديوان سمو رئيس مجلس الوزراء          ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "Pm_Letters/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        public string selectedChapter = "0";
        public string QrelatedOrg = "";
        #endregion

        #region "Page Events"

        protected void Page_PreRender(object sender, EventArgs e)
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
                ViewState["letterId"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["LetterArcID"] = "0";
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



        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
           ClearForm();

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
            ViewState["letterId"] = "0";
            ViewState["ProceduresitemID"] = "0";
             Session["PersonsList"] = null;
            Response.Redirect("/Modules/pm/Forms/Frm_Pm_Letters.aspx");

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
        private string MapSearchKeys()
        {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                _keyList.Add(" رقم الكتاب", txtFilterserial.Text);
                _keyList.Add("  مسلسل  ", txtFilterNum.Text);
                _keyList.Add(" السنة    ", txtFilterYear.Text);
                _keyList.Add("تاريخ  الكتاب من", txtFilterDatefrom.Text);
                _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                _keyList.Add(" الفصل التشريعي     ", lstFilterChapter.SelectedItem.Text);
                _keyList.Add(" التصنيف    ", lstFilterCategory.SelectedItem.Text);

            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }
        private void FillReport()
        {
            string reportTitle = "تقرير كتب ديوان الرئيس ";

            var objList = objRepository.GetList(ZeroIntergerIFNull(txtFilterNum.Text ), ZeroIntergerIFNull(txtFilterYear.Text), (txtFilterserial.Text),   NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterCategory.SelectedValue) , ZeroIntergerIFNull(lstFilterChapter.SelectedValue), MapSearchKeys());


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
                        case "Arc_Year":
                            {
                                reportTitle += "على حسب - السنة  ";
                                break;
                            }
                        case "CategoryNameAr":
                            {
                                reportTitle += "على حسب - التصنيف ";
                                break;
                            }
                        case "ChapterNameAr":
                            {
                                reportTitle += " على حسب - الفصل التشريعي";
                                break;
                            }

                        default:
                            break;
                    }

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/pm/RDLC/rpt_PmListgrouping.rdlc");
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("groupfield", lstgroupping.SelectedValue));
                }
                else
                { ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/pm/RDLC/rpt_PmList.rdlc"); }

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", reportTitle));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("username", gets(ReadSession("AdminName"))));
                
                if (getBool(ReadSession("viewWaterMark")))
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("watermarkpath", getUserWatermarkImage(gets(ReadSession("userid")), gets(ReadSession("AdminName"))), false));



                TempDT.TableName = "ds_PmList";
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
            if (hasattachment!="")
            {
                return "";
            }
            return "display:none";

        }

        public string GetQStatus(int StatusID,string StatusName)
        {
            string _out = "";
            switch (StatusID)
            {
                case 1:
                    {
                        _out = "<span class='label bg-success-400'>"+ StatusName + "  </span>";
                        break;
                    }
                case 2:
                    {
                        _out = "<span class='label bg-warning-400'>"+ StatusName + " </span>";
                        break;
                    }
                case 3:
                    {
                        _out = "<span class='label bg-blue-400'>" + StatusName + "</span>";
                        break;
                    }
                case 4:
                    {
                        _out = "<span class='label bg-grey-400'>"+ StatusName + "</span>";
                        break;
                    }

                default:
                    {
                        _out = "<span class='label bg-grey-400'> "+ StatusName + "</span>";
                        break;
                    }
            }
            return _out;
        }

    private void SetPageTitle()
        {
            if (Request.QueryString["d"] !=null)
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
            FillDllwithoptional_ALL(objLookup.FillRptGrouoing((int)ArcTargetModules.PM), ref lstgroupping, "NameAr", "GroupingFilde", " ");
            FillDllwithoptional_ALL(objLookup.FillPmLetters_Categories(), ref lstFilterCategory , "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeChapter(), ref lstFilterChapter, "NameAr", "Code", "الكل");

        }



        #endregion



    }
}