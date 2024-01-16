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

namespace UI.Web.Modules.Questions.Forms
{
    public partial class QuestionsData : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public QuestionsRepository objRepository = IoC.Resolve<QuestionsRepository>();
        public string _PageTitle = "نظام الأسئلة البرلمانية   ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "QuestionsAttachments/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        public string selectedChapter = "0";
        public string QrelatedOrg = "";
        #endregion

        #region "Page Events"

        protected void Page_PreRender(object sender, EventArgs e)
        {

            applyUserPermission();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {

        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";
            btnCancel.Attributes.Add("onclick", "Page_ValidationActive=false;");
            btnSave.Attributes.Add("onclick", "return chkImage();");



            if (!IsPostBack)
            {
                if (Request.QueryString["activetab"] != null)
                {
                    hdnactivetab.Value = gets(Request.QueryString["activetab"]);
                }

                if (Request.QueryString["ss"] != null && Request.QueryString["ss"].ToString() == "1")
                {

                    string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                }



                fillLookups();


                if (Request.QueryString["StatusID"] != null)
                {
                    lstFilterStatus.SelectedValue = gets(Request.QueryString["StatusID"]);
                    lstFilterChapter.SelectedValue = gets(Request.QueryString["chapter"]);
                    FillQuestions();

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


                if (Request.QueryString["QuestionID"] == null)
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
                    tblAdd.Visible = true;


                    ViewState["itemID"] = Request.QueryString["QuestionID"].ToString();
                    FillQuestionMasterInformation();
                    //Fill Agreemnt Details


                }

                SetPageTitle();

                ViewState["OutboundItemID"] = "0";

                // FillInboundItems();
                UpdateScannedFile();

            }

        }

        protected void grdQuestionsList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {


            if ((e.Item.ItemType == ListItemType.Item))
            {

                //if (e.Item.Cells[4].Text == "1")
                //{
                //    e.Item.Attributes.Add("style", "background:#86b2d5;color:#ffffff");

                //    e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                //    e.Item.Attributes.Add("onmouseout", "this.style.color=\'#ffffff\';this.style.backgroundColor=\'#86b2d5\';");

                //}
                //else
                //{  e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                //    e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
                //}
            }

            if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                //e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                //e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
            }


            if ((e.Item.ItemType == ListItemType.Item))
            {
                //
                HtmlImage im = ((HtmlImage)(e.Item.Cells[2].FindControl("imgControl")));
                string imname = im.ClientID;
                string rowindex = (e.Item.ItemIndex + 1).ToString();
                string rowID = e.Item.ClientID;
                im.Attributes.Add("onclick", ("ControlGrid(\'" + (imname + ("\'," + (rowindex + (",\'" + (rowID + "\')")))))));
                //LinkButton lnk = ((LinkButton)(e.Item.Cells[0].Controls[0]));
                //lnk.Attributes.Add("onclick", "return confirm(\'Are you sure you want to delete this Invoice?\');");
                HtmlAnchor lnkAnswer = (HtmlAnchor)e.Item.FindControl("lnkAnswer");
                string Filecode = e.Item.Cells[3].Text;
                var objUnitList = objRepository.FillQuestionAnswers(ZeroIntergerIFNull(Filecode));
                if (objUnitList != null && objUnitList.Count > 0)
                {
                    if (objUnitList[0] != null)
                    {

                        lnkAnswer.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + objUnitList[0].QuestionID + "/answers/" + objUnitList[0].Code.ToString() + "/" + "&vfileList=[" + gets(objUnitList[0].Answerattachments) + ";]";
                        lnkAnswer.Visible = true;
                    }
                    else { lnkAnswer.Visible = false; }

                }
                else { lnkAnswer.Visible = false; }


            }
            else if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string rowID = e.Item.ClientID;
                string Filecode = e.Item.Cells[3].Text;
                string QStatus = e.Item.Cells[5].Text;

                var objUnitList = objRepository.FillQuestionAnswers(ZeroIntergerIFNull(Filecode));
                DataGrid grd = ((DataGrid)(e.Item.Cells[1].FindControl("grdAnswers")));
                if (objUnitList != null && objUnitList.Count > 0)
                {
                    grd.Visible = true;
                    grd.DataSource = objUnitList;
                    grd.DataBind();

                }
                else
                {
                    // lnkAnswer.Visible = false;
                    grd.Visible = false;
                    ((HtmlGenericControl)(e.Item.Cells[1].FindControl("divAnswer"))).Visible = false;
                }


                // Fill FOllow

                var objFollowList = objRepository.FillArcDocsInOut(ZeroIntergerIFNull(Filecode));
                DataGrid grdfollow = ((DataGrid)(e.Item.Cells[1].FindControl("grdFollow")));
                if (objFollowList != null && objFollowList.Count > 0)
                {
                    grdfollow.Visible = true;
                    grdfollow.DataSource = objFollowList;
                    grdfollow.DataBind();

                }
                else
                {
                    // lnkAnswer.Visible = false;
                    grdfollow.Visible = false;
                    ((HtmlGenericControl)(e.Item.Cells[1].FindControl("divFollow"))).Visible = false;
                }






                for (int i = 2; i <= (e.Item.Cells.Count - 1); i++)
                {
                    e.Item.Cells[i].Visible = false;
                }

                e.Item.Cells[0].Controls[0].Visible = false;
                e.Item.Cells[1].Attributes.Add("colspan", ((e.Item.Cells.Count - 2)).ToString());
                e.Item.Attributes.Add("style", "display:none");
            }

            if (!(e.Item.ItemType == ListItemType.AlternatingItem))
            {
                e.Item.Cells[1].Visible = false;


            }

            //Hide Defult Dates

            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    if (e.Item.Cells[13].Text == "01/01/1990") e.Item.Cells[13].Text = "";
            //    if (e.Item.Cells[15].Text == "01/01/1990") e.Item.Cells[15].Text = "";
            //    if (e.Item.Cells[16].Text == "01/01/1990") e.Item.Cells[16].Text = "";



            //}


        }

        public string GetLastFollow(string QCode)
        {
            string _out = "";

            var objLastFollow = objRepository.FillLastFollow(ZeroIntergerIFNull(QCode));
            if (objLastFollow != null)
            {
                double Percentage = 0;
                string ExecutionColorHex = "#ccc";
                if (objLastFollow.NextFollowReminderDate != null)
                {
                    Percentage = (objLastFollow.NextFollowReminderDate.Value.Date - DateTime.Now.Date).TotalDays <= 0 ? 100 :
                                    Math.Floor((DateTime.Now.Date - objLastFollow.SentDate.Value.Date).TotalDays / (objLastFollow.NextFollowReminderDate.Value.Date - objLastFollow.SentDate.Value.Date).TotalDays * 100);
                    if (Percentage == 0)
                    {
                        Percentage = 1;

                    }
                    if (Percentage <= 50)
                    {
                        ExecutionColorHex = "#08a711"; //greeen
                    }
                    else if (Percentage > 50 && Percentage <= 75)
                    {
                        ExecutionColorHex = "#e3ad24";// yellow
                    }
                    else
                    {
                        ExecutionColorHex = "#e32442";// Red
                    }


                    _out += "<div class='circle-bar position' data-percent='" + Percentage + "' data-color='#ccc," + ExecutionColorHex + "'></div>";
                }
                else
                {

                }



            }


            return _out;

        }


        protected void lnkBack_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            { Response.Redirect("OutboundOperrations.aspx?id=" + Request.QueryString["id"].ToString()); }
            else
            { Response.Redirect("OutboundOperrations.aspx"); }

        }

        protected void grdUnits_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    if (gets(e.Item.Cells[9].Text).Equals("3"))
            //    {
            //        ((CheckBox)e.Item.FindControl("chkItem")).Visible = false;
            //    }

            //    if (gets(e.Item.Cells[9].Text).Equals("1"))
            //    {
            //        ((CheckBox)e.Item.FindControl("chkItem")).Checked = true;
            //    }

            //}
        }

        protected void btnNew_Click1(object sender, EventArgs e)
        {
            Session["Question_prosecutor"] = null;
            Session["Question_Defendant"] = null;
            FillQuestionPersons(0);
            tblAdd.Visible = true;
            tblshow.Visible = false;
            tblSearch.Visible = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }



        protected void grdData_DeleteCommand(object source, DataGridCommandEventArgs e)
        {

        }



        protected void grdData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }


        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            FillQuestions();
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
            tblAdd.Visible = false;

            ViewState["SpEdit"] = "0";
            ViewState["NewDesc"] = "";
            ViewState["NewBar"] = "";
            ViewState["NewIsbn"] = "";
            ViewState["SPITEM"] = "";
            ViewState["NewPrice"] = "0";
            Session["ItemList"] = null;
            ViewState["itemID"] = "0";
            ViewState["itemID"] = "0";
            ViewState["QuestionitemID"] = "0";
            ViewState["ProceduresitemID"] = "0";
            Session["PersonsList"] = null;
            Response.Redirect("/Modules/Questions/Forms/QuestionsData.aspx");

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillQuestions();
        }
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            SaveQuestionMaster();
        }


        protected void lnkAddNewQuestion_Click(object sender, EventArgs e)
        {
            ClearCaseForm();
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

public string fillDocTypefolder(string DocType)
        {
            string _out = "";
            if (DocType.Equals("1"))
            {
                _out = "incoming";
            }
            else if (DocType.Equals("2"))
            {
                _out = "outgoing";
            }


            return _out;

        }


        private void FillQuestionPersons(int QuestionID)
        {
            var objprosecutor = objRepository.FillPersons(QuestionID, 1);// موجة من
            if ((Session["Question_prosecutor"] != null && ViewState["SpEdit"].ToString() == "1") || (Session["Question_prosecutor"] != null && ViewState["SpChapterChanged"].ToString() == "1"))
            {
                objprosecutor = (List<Parliament_Requestedby>)Session["Question_prosecutor"];
            }
            else if (objprosecutor.Count > 0)
            {
                Session["Question_prosecutor"] = objprosecutor;
            }


            if (objprosecutor != null && objprosecutor.Count > 0)
            {

                grdprosecutor.DataSource = AddDefaultItems(objprosecutor);



                if (ViewState["SpEdit"].Equals("0"))
                {
                    grdprosecutor.EditItemIndex = objprosecutor.Count;
                }

                grdprosecutor.DataBind();



            }
            else
            {

                grdprosecutor.EditItemIndex = 0;
                grdprosecutor.DataSource = AddDefaultItems(objprosecutor);
                grdprosecutor.DataBind();

            }


            ///'//////////////////////////////////'

            var objDefendant = objRepository.FillPersons(QuestionID, 2);//  جهات ذات صلة
            if (Session["Question_Defendant"] != null && ViewState["SpEdit"].ToString() == "1")
            {
                objDefendant = (List<Parliament_Requestedby>)Session["Question_Defendant"];
            }
            else if (objDefendant.Count > 0)
            {
                Session["Question_Defendant"] = objDefendant;
            }


            if (objDefendant != null && objDefendant.Count > 0)
            {
                hdnRelatedOrg.Value = "";

                foreach (var item in objDefendant)
                {
                    QrelatedOrg += item.PersonID.ToString() + ",";
                }
                QrelatedOrg = QrelatedOrg.Substring(0, QrelatedOrg.Length - 1);

                hdnRelatedOrg.Value = QrelatedOrg;
                grdDefendant.DataSource = AddDefaultItems(objDefendant);



                if (ViewState["SpEdit"].Equals("0"))
                {
                    grdDefendant.EditItemIndex = objDefendant.Count;
                }

                grdDefendant.DataBind();



            }
            else
            {

                grdDefendant.EditItemIndex = 0;
                grdDefendant.DataSource = AddDefaultItems(objDefendant);
                grdDefendant.DataBind();

            }


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
                _keyList.Add(" رقم السؤال ", txtFilterserial.Text);

                _keyList.Add("تاريخ  السؤال من", txtFilterDatefrom.Text);
                _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                _keyList.Add(" رقم الصادر بمجلس الامة  ", txtFilterInternalSerial.Text);
                _keyList.Add("السنة  ", txtFilterFileYear.Text);
                _keyList.Add("  جزء من نص السؤال  ", txtFilterSubject.Text);
                _keyList.Add("سؤال موحد ", chkFilerIsGroup.Checked ? "true" : "false");
                _keyList.Add(" الفصل التشريعي ", lstFilterChapter.SelectedItem.Text);
                _keyList.Add(" موجه من ", lstFilterPerson.SelectedItem.Text);
                _keyList.Add("   الجهات المعنيه    ", lstFilterRelatedOrgs.SelectedItem.Text);
                _keyList.Add("    دور الانعقاد ", lstFilterSession.SelectedItem.Text);
                _keyList.Add("الحالة", lstFilterStatus.SelectedItem.Text);
                _keyList.Add("موجة إلى ", lstFilterRequestTo.SelectedItem.Text);
                _keyList.Add("  الموظف المختص ", lstFilterAssignedPerson.SelectedItem.Text);

            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }
        private void FillQuestions()
        {

            List<int> selectedRequestedFrom = new List<int>();
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

            var objList = objRepository.GetList((int)Questions_TypesEnum.Question, ZeroIntergerIFNull(txtFilterserial.Text), (txtFilterInternalSerial.Text), ZeroIntergerIFNull(txtFilterFileYear.Text), NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterChapter.SelectedValue), ZeroIntergerIFNull(lstFilterSession.SelectedValue),
                ZeroIntergerIFNull(lstFilterStatus.SelectedValue), selectedRequestedFrom,
                ZeroIntergerIFNull(lstFilterRequestTo.SelectedValue), txtFilterSubject.Text, ZeroIntergerIFNull(lstFilterRelatedOrgs.SelectedValue)
                 , ZeroIntergerIFNull(lstFilterAssignedPerson.SelectedValue), 0, new DateTime(1990, 01, 01), new DateTime(1990, 01, 01),chkFilerIsGroup.Checked, MapSearchKeys());

            if (objList != null && objList.Count > 0)
            {


                lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
                lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));


                //var duplicatedList = objList.SelectMany(t =>
                // Enumerable.Repeat(t, 2)).ToList();


                if (objList.Count > 0)
                {
                    //btnSave.Visible = true;
                    //lnkBack.Visible = true;

                    tblshow.Visible = true;
                    tblSearch.Visible = false;
                    pager1.Visible = true;

                }
                else
                {
                    tblshow.Visible = false;
                    pager1.Visible = false;
                    tblSearch.Visible = true;
                    string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                }

                var duplicatedList = objList.SelectMany(t =>
               Enumerable.Repeat(t, 2)).ToList();
                

                grdQuestionsList.DataSource = duplicatedList;
             
                grdQuestionsList.DataBind();


               

                pager1.ItemCount = duplicatedList.Count;
            }
        }

        private void ClearCaseForm()
        {


            ViewState["QuestionitemID"] = "0";
            txtOma_Serial.Text = "";
            txtQoOmaYear.Text = "";
            txtQuestionSubject.Text = "";
            txtQuestionNote.Text = "";
            txtQuestionInternalSerial.Text = "";
            txtQoOmaYear.Text = "";
            lstChapter.SelectedValue = "0";
            lstSession.SelectedValue = "0";
            lstStatus.SelectedValue = "0";



            Session["PersonsList"] = null;


            Session["Question_prosecutor"] = null;
            Session["Question_Defendant"] = null;
            FillQuestionPersons(0);


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

        public string GetQStatus(int StatusID, string StatusName)
        {
            string _out = "";
            switch (StatusID)
            {
                case 1:
                    {
                        _out = "<span class='label bg-success-400'>" + StatusName + "  </span>";
                        break;
                    }
                case 2:
                    {
                        _out = "<span class='label bg-warning-400'>" + StatusName + " </span>";
                        break;
                    }
                case 3:
                    {
                        _out = "<span class='label bg-blue-400'>" + StatusName + "</span>";
                        break;
                    }
                case 4:
                    {
                        _out = "<span class='label bg-grey-400'>" + StatusName + "</span>";
                        break;
                    }

                default:
                    {
                        _out = "<span class='label bg-grey-400'> " + StatusName + "</span>";
                        break;
                    }
            }
            return _out;
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

        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            lnkAddAnswer.Visible = userAccess.Add;
            Lnkincoming.Visible = userAccess.Add;
            lnkAddNewOutGoing.Visible = userAccess.Add;


            btnSave.Visible = userAccess.Edit ||  userAccess.Add;
            lnkSaveAnswer.Visible = userAccess.Edit ||  userAccess.Add;
            lnkSaveIncoming.Visible = userAccess.Edit ||  userAccess.Add;
            lnkSaveOut.Visible = userAccess.Edit ||  userAccess.Add;


            //btnDelete.Visible = userAccess.Delete;
            lnkDeleteAnswer.Visible = userAccess.Delete;

            lnkDeleteIncoming.Visible = userAccess.Delete;
            lnkDeleteOutgoing.Visible = userAccess.Delete;

            grdQuestionsList.Columns[17].Visible = userAccess.Delete;


        }

        protected void pager_Command(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdQuestionsList.PageCount))
            {
                currnetPageIndx = (grdQuestionsList.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdQuestionsList.CurrentPageIndex = (currnetPageIndx - 1);
            FillQuestions();
        }


        #endregion

        #region "Helper Methods"


        private void FillQuestionMasterInformation()
        {

            var objList = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {
                hdnMasterID.Value = gets(objList.code);
                ViewState["QuestionitemID"] = gets(objList.code);

                txtQuestionInternalSerial.Text = gets(objList.Q_Serial);
                txtQoOmaYear.Text = gets(objList.OmaYear);
                txtOma_Serial.Text = gets(objList.Oma_Serial);
                txtQuestionNote.Text = gets(objList.Q_Notes);
                txtQuestionDate.Text = NullDateifEmptyToText(objList.Q_Date).ToString();

                lstChapter.SelectedValue = gets(objList.ChapterID);
                selectedChapter = gets(objList.ChapterID);

                lstChapter_SelectedIndexChanged(null, null);
                //selectedChapter = lstChapter.SelectedValue;
                //ViewState["SpChapterChanged"] = "1";
                //FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstChapter.SelectedValue)), lstSession, "NameAr", "Code", "اختر");
                //FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstChapter.SelectedValue)), lstRequestFrom, "NameAr", "Code", "اختر");
                //var requestedFromPersonList = objLookup.FillOMaPerson(ZeroIntergerIFNull(lstChapter.SelectedValue));
                //Session["RequestedFromPersonList"] = requestedFromPersonList;

                lstSession.SelectedValue = gets(objList.SessionID);
                if (!gets(objList.Q_RequestFrom).Equals("0"))
                {
                    lstRequestFrom.SelectedValue = gets(objList.Q_RequestFrom);
                }

                lstRequestTo.SelectedValue = gets(objList.Q_RequestTo);
                if (!gets(objList.AssignedPersonID).Equals("0"))
                {
                    lstassignedPersons.SelectedValue = gets(objList.AssignedPersonID);
                }

                lstStatus.SelectedValue = gets(objList.StatusID);

                chkIsGrouped.Checked = getBool(objList.Is_grouped);


                if (!gets(objList.Q_Attachment).Equals(""))
                {
                    lnkQScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل نص السؤال";
                    hdnQScannerfilepath.Value = gets(objList.Q_Attachment);
                }

                txtQuestionSubject.Text = gets(objList.Q_Text);
                if (!gets(objList.Q_Attachment).Equals(""))
                {
                    anchorAttachment.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + objList.code.ToString() + "/" + "&vfileList=[" + gets(objList.Q_Attachment) + ";]";
                    anchorAttachment.Visible = true;
                }

                //Fill Person Information
                FillQuestionPersons(objList.code);
                //Fill Arch
                FillArcData(1, objList.code);
                FillArcData(2, objList.code);
                //Fill Attachments

                //FillFileAttachment(objList.Code);
                FilLQuestionAnswer(objList.code);


            }

            tblAdd.Visible = true;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private int SaveQuestionMaster(bool fromScan = false)
        {
            string script = "";
            //Upload LocalFile

            Parliament_Questions objQuestion = new Parliament_Questions();

            try
            {
                if (ViewState["itemID"].Equals("0"))
                {//Save

                    if (objRepository.CheckQuestionExistance(txtOma_Serial.Text, ZeroIntergerIFNull(lstChapter.SelectedValue), 0, 1))
                    {

                        script = FormatpopupErrorMSG(" [السؤال مسجل من قبل - فى الفصل التشريعي]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return 0;
                    }


                    objQuestion.QType = 1;//1 Questions 2 Estgwap
                    objQuestion.CreationDate = DateTime.Now;
                    objQuestion.LastActionDate = DateTime.Now;

                    objQuestion.Q_Serial = ZeroIntergerIFNull(txtQuestionInternalSerial.Text);
                    objQuestion.Oma_Serial = gets(txtOma_Serial.Text);
                    objQuestion.OmaYear = ZeroIntergerIFNull(txtQoOmaYear.Text);


                    objQuestion.Q_Text = txtQuestionSubject.Text;
                    objQuestion.Q_Notes = txtQuestionNote.Text;

                    objQuestion.Q_Date = NullDateifEmpty(txtQuestionDate.Text);

                    objQuestion.AssignedPersonID = ZeroIntergerIFNull(lstassignedPersons.SelectedValue);

                    objQuestion.ChapterID = ZeroIntergerIFNull(lstChapter.SelectedValue);
                    objQuestion.SessionID = ZeroIntergerIFNull(lstSession.SelectedValue);
                    objQuestion.Q_RequestTo = ZeroIntergerIFNull(lstRequestTo.SelectedValue);
                    objQuestion.Q_RequestFrom = ZeroIntergerIFNull(lstRequestFrom.SelectedValue);
                    objQuestion.StatusID = ZeroIntergerIFNull(lstStatus.SelectedValue);
                    objQuestion.Is_grouped = getBool(chkIsGrouped.Checked);

                    objQuestion.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    objRepository.AddQuestion(objQuestion);
                    hdnMasterID.Value = gets(objQuestion.code);
                    ViewState["itemID"] = gets(objQuestion.code);

                }
                else
                { //Update

                    //if (objRepository.CheckQuestionExistance(ZeroIntergerIFNull(txtDoc_Serial.Text), ZeroIntergerIFNull(lstChapter.SelectedValue), ZeroIntergerIFNull(ViewState["itemID"].ToString()),1))
                    //{

                    //    script = FormatpopupErrorMSG(" [مسلسل السؤال مسجل من قبل - فى الفصل التشريعي]", "1");
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    //    return 0;
                    //}

                    hdnMasterID.Value = ViewState["itemID"].ToString();
                    objQuestion = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                    objQuestion.LastActionDate = DateTime.Now;

                    objQuestion.Q_Serial = ZeroIntergerIFNull(txtQuestionInternalSerial.Text);
                    objQuestion.Oma_Serial = gets(txtOma_Serial.Text);
                    objQuestion.OmaYear = ZeroIntergerIFNull(txtQoOmaYear.Text);


                    objQuestion.Q_Text = txtQuestionSubject.Text;
                    objQuestion.Q_Notes = txtQuestionNote.Text;

                    objQuestion.Q_Date = NullDateifEmpty(txtQuestionDate.Text);

                    objQuestion.AssignedPersonID = ZeroIntergerIFNull(lstassignedPersons.SelectedValue);

                    objQuestion.ChapterID = ZeroIntergerIFNull(lstChapter.SelectedValue);
                    objQuestion.SessionID = ZeroIntergerIFNull(lstSession.SelectedValue);
                    objQuestion.Q_RequestTo = ZeroIntergerIFNull(lstRequestTo.SelectedValue);
                    objQuestion.Q_RequestFrom = ZeroIntergerIFNull(lstRequestFrom.SelectedValue);
                    objQuestion.StatusID = ZeroIntergerIFNull(lstStatus.SelectedValue);


                    objQuestion.Is_grouped = getBool(chkIsGrouped.Checked);


                    objQuestion.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());



                    objRepository.UpdateQuestion(objQuestion);
                }


                //Save Related Files

                string _img = UploadFileoServer(txtQImage, ScannerRepository + _TargetUploadPath + objQuestion.code.ToString() + "/");
                if (_img != "")
                {
                    var ObjQuestionForUpdate = objRepository.GetDetailsForEdit(objQuestion.code);
                    ObjQuestionForUpdate.Q_Attachment = _img;
                    objRepository.UpdateQuestion(ObjQuestionForUpdate);
                }

                ViewState["itemID"] = objQuestion.code;


                if (!gets(objQuestion.Q_Attachment).Equals(""))
                {
                    anchorAttachment.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + objQuestion.code.ToString() + "/" + "&vfileList=[" + gets(objQuestion.Q_Attachment) + ";]";
                    anchorAttachment.Visible = true;
                }


                //  ClearForm();
                //Save Person
                try
                {

                    //Get related Orgs

                    if (hdnRelatedOrg.Value != "")
                    {
                        //Delete Quextion Related Orgs
                        Session["Question_Defendant"] = null;
                        Session["Question_prosecutor"] = null;

                        //Set
                        string[] SelectedOrgs = hdnRelatedOrg.Value.Split(',');

                        List<Parliament_Requestedby> PersonsList = new List<Parliament_Requestedby>();
                        for (int i = 0; i < SelectedOrgs.Length; i++)
                        {


                            Parliament_Requestedby _personobj = new Parliament_Requestedby();
                            _personobj.Code = -1;
                            _personobj.PartyType = 2;
                            _personobj.PersonID = ZeroIntergerIFNull(SelectedOrgs[i]);
                            _personobj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                            _personobj.AddDate = DateTime.Now;
                            _personobj.LastUpdate = DateTime.Now;
                            PersonsList.Add(_personobj);
                        }

                        Session["Question_Defendant"] = PersonsList;
                        SavePersons(objQuestion.code);
                        Session["Question_Defendant"] = null;
                        Session["Question_prosecutor"] = null;
                    }
                    else
                    {

                        Session["Question_Defendant"] = null;
                        Session["Question_prosecutor"] = null;
                        SavePersons(objQuestion.code);
                    }


                    script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                }
                catch (Exception)
                {

                    throw;
                }

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

            if (!fromScan)
            {
                Response.Redirect("QuestionsData.aspx?ss=1&QuestionID=" + gets(objQuestion.code));
                return 0;
            }
            return objQuestion.code;
        }
        private void fillLookups()
        {


            FillDllwithoptional_ALL(objLookup.FillParliament_AssignedPerson(), ref lstassignedPersons, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillParliament_AssignedPerson(), ref lstFilterAssignedPerson, "NameAr", "Code", "الكل");


            FillDllwithoptional_ALL(objLookup.FillParliament_QuestionStatus(), ref lstStatus, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillParliament_QuestionStatus(), ref lstFilterStatus, "NameAr", "Code", "الكل");

            FillDll(objLookup.FillParliament_legislativeChapter(), ref lstChapter, "NameAr", "Code");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeChapter(), ref lstFilterChapter, "NameAr", "Code", "الكل");


            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstChapter.SelectedValue)), ref lstSession, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterSession, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillParliament_RequestedToPerson(), ref lstFilterRequestTo, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillParliament_RequestedToPerson(), ref lstRequestTo, "NameAr", "Code", "اختر");

            FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterPerson, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstChapter.SelectedValue)), ref lstRequestFrom, "NameAr", "Code", "اختر");

            FillDllwithoptional_ALL(objLookup.FillParliament_RelatedOrgs(), ref lstFilterRelatedOrgs, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillParliament_RelatedOrgs(), ref lstRelatedOrgs, "NameAr", "Code", "");
            lstRelatedOrgs.SelectedIndex = -1;


            FillDllwithoptional_ALL(objLookup.FillParliament_RelatedOrgs(), ref lstIncomingOrg, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillParliament_RelatedOrgs(), ref lstoutgoiningOrg, "NameAr", "Code", "اختر");

            var requestedFromPersonList = objLookup.FillOMaPerson(ZeroIntergerIFNull(lstChapter.SelectedValue));
            Session["RequestedFromPersonList"] = requestedFromPersonList;


            var relatedOrgs = objLookup.FillParliament_RelatedOrgs();
            Session["relatedOrgs"] = relatedOrgs;


            try
            {
                lstassignedPersons.SelectedValue = "8";
            }
            catch (Exception ex)
            {
            }

        }

        public string activeTab(int tabindex)
        {
            string _out = "";
            if (hdnactivetab.Value == "" && tabindex == 1)
            {
                _out = "active";
            }
            else if (ZeroIntergerIFNull(hdnactivetab.Value) == tabindex)
            {
                _out = "active";
            }


            return _out;

        }

        #endregion

        #region "Procedure Methods"



        #endregion

        #region "Persons"

        private void SavePersons(int QuestionCode)
        {
            // Delete Reated persons

            var objprosecutor = objRepository.FillPersons(QuestionCode, 2);// موجة من
            if (objprosecutor != null)
            {
                foreach (var item in objprosecutor)
                {
                    objRepository.DeletePersons(item);

                }

            }


            if (Session["Question_prosecutor"] != null)
            {
                List<Parliament_Requestedby> _PersonsList = new List<Parliament_Requestedby>();

                _PersonsList = (List<Parliament_Requestedby>)Session["Question_prosecutor"];

                for (int i = 0; i < _PersonsList.Count; i++)
                {


                    if (_PersonsList[i].Code == 0 || _PersonsList[i].Code == -1)
                    {//Insert
                        _PersonsList[i].QuestionID = QuestionCode;
                        //Adding Child Enitty
                        // Set _PersonsList[i] person ID
                        objRepository.AddPerson(_PersonsList[i]);

                    }
                    else

                    {//Update
                        _PersonsList[i].QuestionID = QuestionCode;
                        objRepository.UpdatePersons(_PersonsList[i]);
                    }

                }


            }
            /**********************************************/

            if (Session["Question_Defendant"] != null)
            {
                List<Parliament_Requestedby> _PersonsList = new List<Parliament_Requestedby>();

                _PersonsList = (List<Parliament_Requestedby>)Session["Question_Defendant"];

                for (int i = 0; i < _PersonsList.Count; i++)
                {


                    if (_PersonsList[i].Code == 0 || _PersonsList[i].Code == -1)
                    {//Insert
                        _PersonsList[i].QuestionID = QuestionCode;
                        //Adding Child Enitty

                        objRepository.AddPerson(_PersonsList[i]);

                    }
                    else

                    {//Update
                        _PersonsList[i].QuestionID = QuestionCode;
                        objRepository.UpdatePersons(_PersonsList[i]);
                    }

                }


            }


        }
        private bool checkpersonExistance(int? personID)
        {

            List<Parliament_Requestedby> objprosecutor = new List<Parliament_Requestedby>();
            if (Session["Question_prosecutor"] != null)
            {
                objprosecutor = (List<Parliament_Requestedby>)Session["Question_prosecutor"];
            }
            if (objprosecutor.Where(i => i.PersonID == personID).ToList().Count > 0)
            {
                return true;
            }

            return false;
        }

        private bool checkOrgExistance(int? OrgID)
        {

            List<Parliament_Requestedby> objprosecutor = new List<Parliament_Requestedby>();
            if (Session["Question_Defendant"] != null)
            {
                objprosecutor = (List<Parliament_Requestedby>)Session["Question_Defendant"];
            }
            if (objprosecutor.Where(i => i.PersonID == OrgID).ToList().Count > 0)
            {
                return true;
            }

            return false;
        }

        protected void btnAddNewItem_Click(object sender, EventArgs e)
        {

        }


        protected void grdprosecutor_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                ViewState["SpEdit"] = "0";

                List<Parliament_Requestedby> objprosecutor = new List<Parliament_Requestedby>();
                if (Session["Question_prosecutor"] != null)
                {
                    objprosecutor = (List<Parliament_Requestedby>)Session["Question_prosecutor"];
                }

                Parliament_Requestedby _personobj = new Parliament_Requestedby();

                _personobj.Code = -1;
                _personobj.PartyType = 1;
                _personobj.AddDate = DateTime.Now;
                _personobj.LastUpdate = DateTime.Now;
                //_personobj.PersonName = gets(((TextBox)e.Item.FindControl("txtname")).Text);
                //_personobj.PersonID = objRepository.GetPersonIDbyName(gets(((TextBox)e.Item.FindControl("txtname")).Text).ToLower());


                _personobj.PersonName = gets(((DropDownList)e.Item.FindControl("lstRequestedFromPareon")).SelectedItem.Text);
                _personobj.PersonID = ZeroIntergerIFNull(((DropDownList)e.Item.FindControl("lstRequestedFromPareon")).SelectedValue);

                //Remove Selected ID From List



                _personobj.Notes = gets(((TextBox)e.Item.FindControl("txtNotes")).Text);
                _personobj.CivilID = gets(((TextBox)e.Item.FindControl("txtCivilID")).Text);

                _personobj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                //Check person Existance
                if (!checkpersonExistance(_personobj.PersonID))
                {
                    objprosecutor.Add(_personobj);
                    Session["Question_prosecutor"] = objprosecutor;
                }
                else
                {
                    string script = FormatpopupErrorMSG("تم اضافة الشخص من قبل", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    return;
                }



                //  grdPersons.EditItemIndex = -1;

                grdprosecutor.EditItemIndex = objprosecutor.Count;

                grdprosecutor.DataSource = AddDefaultItems(objprosecutor);
                grdprosecutor.DataBind();
                //  FillMedalPersons(ZeroIntergerIFNull(hdnMasterID.Value));

                // FillMedalPersons(ZeroIntergerIFNull(Request.QueryString["id"].ToString()));
            }
            else if (e.CommandName.Equals("Edit"))
            {
                ViewState["SpEdit"] = 1;
                grdprosecutor.EditItemIndex = e.Item.ItemIndex;
                // grdPersons.DataBind();


                List<Parliament_Requestedby> objprosecutor = new List<Parliament_Requestedby>();
                if (Session["Question_prosecutor"] != null)
                {
                    objprosecutor = (List<Parliament_Requestedby>)Session["Question_prosecutor"];
                }

                grdprosecutor.DataSource = AddDefaultItems(objprosecutor);
                grdprosecutor.DataBind();

            }
            else if (e.CommandName.Equals("Update"))
            {

                List<Parliament_Requestedby> PersonsList = new List<Parliament_Requestedby>();
                if (Session["Question_prosecutor"] != null)
                {
                    PersonsList = (List<Parliament_Requestedby>)Session["Question_prosecutor"];
                }



                Parliament_Requestedby _personobj = new Parliament_Requestedby();
                _personobj = PersonsList[e.Item.ItemIndex];

                //_personobj.PersonName = gets(((TextBox)e.Item.FindControl("txtname")).Text);

                //Check person Existance
                if (checkpersonExistance(_personobj.PersonID) && _personobj.PersonID != ZeroIntergerIFNull(((DropDownList)e.Item.FindControl("lstRequestedFromPareon")).SelectedValue))
                {
                    string script = FormatpopupErrorMSG("تم اضافة الشخص من قبل", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    return;
                }


                _personobj.PersonName = gets(((DropDownList)e.Item.FindControl("lstRequestedFromPareon")).SelectedItem.Text);
                _personobj.PersonID = ZeroIntergerIFNull(((DropDownList)e.Item.FindControl("lstRequestedFromPareon")).SelectedValue);





                _personobj.Notes = gets(((TextBox)e.Item.FindControl("txtNotes")).Text);
                _personobj.LastUpdate = DateTime.Now;
                _personobj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                PersonsList[e.Item.ItemIndex] = _personobj;
                Session["Question_prosecutor"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;
                ViewState["SpEdit"] = 0;

                grdprosecutor.EditItemIndex = PersonsList.Count;

                grdprosecutor.DataSource = AddDefaultItems(PersonsList);
                grdprosecutor.DataBind();
            }
            else if (e.CommandName.Equals("Delete"))
            {
                string code = e.Item.Cells[2].Text.Replace("&nbsp;", " ").Trim();

                List<Parliament_Requestedby> PersonsList = new List<Parliament_Requestedby>();
                if (Session["Question_prosecutor"] != null)
                {
                    PersonsList = (List<Parliament_Requestedby>)Session["Question_prosecutor"];
                }
                if (code.ToString() != "0" && code.ToString() != "-1")
                {
                    objRepository.DeletePersons(PersonsList[e.Item.ItemIndex]);

                }

                PersonsList.RemoveAt(e.Item.ItemIndex);
                Session["Question_prosecutor"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;
                ViewState["SpEdit"] = 0;

                grdprosecutor.EditItemIndex = PersonsList.Count;

                grdprosecutor.DataSource = AddDefaultItems(PersonsList);
                grdprosecutor.DataBind();
            }
            else if (e.CommandName.Equals("Cancel"))
            {
                ViewState["SpEdit"] = 0;

                List<Parliament_Requestedby> PersonsList = new List<Parliament_Requestedby>();
                if (Session["Question_prosecutor"] != null)
                {
                    PersonsList = (List<Parliament_Requestedby>)Session["Question_prosecutor"];
                }
                grdprosecutor.EditItemIndex = PersonsList.Count;

                grdprosecutor.DataSource = AddDefaultItems(PersonsList);
                grdprosecutor.DataBind();
            }
        }

        protected void grdprosecutor_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {

                TextBox txtname = (TextBox)e.Item.FindControl("txtname");
                TextBox txtNotes = (TextBox)e.Item.FindControl("txtNotes");
                TextBox txtCivilID = (TextBox)e.Item.FindControl("txtCivilID");

                DropDownList lstRequestedFromPareon = (DropDownList)e.Item.FindControl("lstRequestedFromPareon");
                HtmlInputHidden hdnrequestedfromPerson = (HtmlInputHidden)e.Item.FindControl("hdnrequestedfromPerson");

                HtmlInputHidden hdnPerson_NameAr = (HtmlInputHidden)e.Item.FindControl("hdnPerson_NameAr");
                //HtmlInputHidden hdnGradeID = (HtmlInputHidden)e.Item.FindControl("hdnGradeID");
                HtmlInputHidden hdnCivilID = (HtmlInputHidden)e.Item.FindControl("hdnCivilID");
                HtmlInputHidden hdnNoteText = (HtmlInputHidden)e.Item.FindControl("hdnNotes");


                if (hdnPerson_NameAr.Value != "")
                {
                    txtname.Text = hdnPerson_NameAr.Value;
                }


                if (hdnNoteText.Value != "" && hdnNoteText.Value != "0")
                {
                    txtNotes.Text = hdnNoteText.Value;
                }
                if (hdnCivilID.Value != "" && hdnCivilID.Value != "0")
                {
                    txtCivilID.Text = hdnCivilID.Value;
                }


                lstRequestedFromPareon.Items.Clear();
                FillDllwithoptional_ALL(Session["RequestedFromPersonList"], ref lstRequestedFromPareon, "NameAr", "Code", "");
                if (hdnrequestedfromPerson.Value != "" && hdnrequestedfromPerson.Value != "0")
                {
                    lstRequestedFromPareon.SelectedValue = hdnrequestedfromPerson.Value;
                }



                e.Item.Cells[1].Visible = false;
                e.Item.Cells[0].Attributes.Add("colspan", "2");

                LinkButton lnkUpdate = (LinkButton)(e.Item.Cells[0].FindControl("lnkUpdate"));
                LinkButton lnkAdd = (LinkButton)(e.Item.Cells[0].FindControl("lnkAdd"));
                LinkButton lnkCancel = (LinkButton)(e.Item.Cells[0].FindControl("lnkCancel"));

                lnkAdd.Attributes.Add("onclick", "return LinkAddClick();");
                if (ViewState["SpEdit"].ToString() == "0")//' Inseting New Row Mode
                {
                    lnkUpdate.Visible = false;
                    lnkCancel.Visible = false;
                    lnkAdd.Visible = true;
                    lnkAdd.Attributes.Add("onclick", "return LinkAddClick();");
                }
                else
                {
                    lnkUpdate.Visible = true;
                    lnkCancel.Visible = true;
                    lnkAdd.Visible = false;


                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string code = e.Item.Cells[2].Text.Replace("&nbsp;", " ").Trim();
                LinkButton lnkEdit = (LinkButton)(e.Item.Cells[0].FindControl("lnkEdit"));
                LinkButton lnkDelete = (LinkButton)(e.Item.Cells[0].FindControl("lnkDelete"));

                if (code.ToString().Equals("0"))
                {

                    lnkEdit.Visible = false;
                    lnkDelete.Visible = false;
                }
                else
                {
                    lnkEdit.Visible = true;
                    lnkDelete.Visible = true;



                }

            }

        }
        protected void grdDefendant_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {

                TextBox txtname = (TextBox)e.Item.FindControl("txtname");
                TextBox txtNotes = (TextBox)e.Item.FindControl("txtNotes");


                HtmlInputHidden hdnPerson_NameAr = (HtmlInputHidden)e.Item.FindControl("hdnPerson_NameAr");
                //HtmlInputHidden hdnGradeID = (HtmlInputHidden)e.Item.FindControl("hdnGradeID");
                //HtmlInputHidden hdnCivilID = (HtmlInputHidden)e.Item.FindControl("hdnCivilID");
                HtmlInputHidden hdnNoteText = (HtmlInputHidden)e.Item.FindControl("hdnNotes");


                DropDownList lstRelatedOrgs = (DropDownList)e.Item.FindControl("lstRelatedOrgs");
                HtmlInputHidden hdnRelatedOrgs = (HtmlInputHidden)e.Item.FindControl("hdnRelatedOrgs");


                if (hdnPerson_NameAr.Value != "")
                {
                    txtname.Text = hdnPerson_NameAr.Value;
                }


                if (hdnNoteText.Value != "" && hdnNoteText.Value != "0")
                {
                    txtNotes.Text = hdnNoteText.Value;
                }



                lstRelatedOrgs.Items.Clear();
                FillDllwithoptional_ALL(Session["relatedOrgs"], ref lstRelatedOrgs, "NameAr", "Code", "");
                if (hdnRelatedOrgs.Value != "" && hdnRelatedOrgs.Value != "0")
                {
                    lstRelatedOrgs.SelectedValue = hdnRelatedOrgs.Value;
                }



                e.Item.Cells[1].Visible = false;
                e.Item.Cells[0].Attributes.Add("colspan", "2");

                LinkButton lnkUpdate = (LinkButton)(e.Item.Cells[0].FindControl("lnkUpdate"));
                LinkButton lnkAdd = (LinkButton)(e.Item.Cells[0].FindControl("lnkAdd"));
                LinkButton lnkCancel = (LinkButton)(e.Item.Cells[0].FindControl("lnkCancel"));

                lnkAdd.Attributes.Add("onclick", "return relatedOrgClick();");
                if (ViewState["SpEdit"].ToString() == "0")//' Inseting New Row Mode
                {
                    lnkUpdate.Visible = false;
                    lnkCancel.Visible = false;
                    lnkAdd.Visible = true;
                    lnkAdd.Attributes.Add("onclick", "return relatedOrgClick();");
                }
                else
                {
                    lnkUpdate.Visible = true;
                    lnkCancel.Visible = true;
                    lnkAdd.Visible = false;


                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string code = e.Item.Cells[2].Text.Replace("&nbsp;", " ").Trim();
                LinkButton lnkEdit = (LinkButton)(e.Item.Cells[0].FindControl("lnkEdit"));
                LinkButton lnkDelete = (LinkButton)(e.Item.Cells[0].FindControl("lnkDelete"));

                if (code.ToString().Equals("0"))
                {

                    lnkEdit.Visible = false;
                    lnkDelete.Visible = false;
                }
                else
                {
                    lnkEdit.Visible = true;
                    lnkDelete.Visible = true;



                }

            }

        }

        protected void grdDefendant_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                ViewState["SpEdit"] = "0";

                List<Parliament_Requestedby> PersonsList = new List<Parliament_Requestedby>();
                if (Session["Question_Defendant"] != null)
                {
                    PersonsList = (List<Parliament_Requestedby>)Session["Question_Defendant"];
                }




                Parliament_Requestedby _personobj = new Parliament_Requestedby();



                _personobj.Code = -1;
                _personobj.PartyType = 2;
                //   _personobj.PersonName = gets(((TextBox)e.Item.FindControl("txtDname")).Text);



                _personobj.PersonName = gets(((DropDownList)e.Item.FindControl("lstRelatedOrgs")).SelectedItem.Text);
                _personobj.PersonID = ZeroIntergerIFNull(((DropDownList)e.Item.FindControl("lstRelatedOrgs")).SelectedValue);




                _personobj.Notes = gets(((TextBox)e.Item.FindControl("txtDNotes")).Text);
                _personobj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                _personobj.AddDate = DateTime.Now;
                _personobj.LastUpdate = DateTime.Now;
                //Check person Existance
                if (checkOrgExistance(_personobj.PersonID))
                {
                    string script = FormatpopupErrorMSG("تم اضافة الجهة من قبل", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    return;
                }


                PersonsList.Add(_personobj);
                Session["Question_Defendant"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;

                grdDefendant.EditItemIndex = PersonsList.Count;

                grdDefendant.DataSource = AddDefaultItems(PersonsList);
                grdDefendant.DataBind();
                //  FillMedalPersons(ZeroIntergerIFNull(hdnMasterID.Value));

                // FillMedalPersons(ZeroIntergerIFNull(Request.QueryString["id"].ToString()));
            }
            else if (e.CommandName.Equals("Edit"))
            {
                ViewState["SpEdit"] = 1;
                grdDefendant.EditItemIndex = e.Item.ItemIndex;
                // grdPersons.DataBind();


                List<Parliament_Requestedby> PersonsList = new List<Parliament_Requestedby>();
                if (Session["Question_Defendant"] != null)
                {
                    PersonsList = (List<Parliament_Requestedby>)Session["Question_Defendant"];
                }

                grdDefendant.DataSource = AddDefaultItems(PersonsList);
                grdDefendant.DataBind();

            }
            else if (e.CommandName.Equals("Update"))
            {

                List<Parliament_Requestedby> PersonsList = new List<Parliament_Requestedby>();
                if (Session["Question_Defendant"] != null)
                {
                    PersonsList = (List<Parliament_Requestedby>)Session["Question_Defendant"];
                }



                Parliament_Requestedby _personobj = new Parliament_Requestedby();
                _personobj = PersonsList[e.Item.ItemIndex];

                //Check person Existance
                if (checkpersonExistance(_personobj.PersonID) && _personobj.PersonID != ZeroIntergerIFNull(((DropDownList)e.Item.FindControl("lstRequestedFromPareon")).SelectedValue))
                {
                    string script = FormatpopupErrorMSG("تم اضافة الجهة من قبل", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    return;
                }


                //   _personobj.PersonName = gets(((TextBox)e.Item.FindControl("txtDname")).Text);


                _personobj.PersonName = gets(((DropDownList)e.Item.FindControl("lstRelatedOrgs")).SelectedItem.Text);
                _personobj.PersonID = ZeroIntergerIFNull(((DropDownList)e.Item.FindControl("lstRelatedOrgs")).SelectedValue);

                _personobj.Notes = gets(((TextBox)e.Item.FindControl("txtDNotes")).Text);
                _personobj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                _personobj.LastUpdate = DateTime.Now;

                PersonsList[e.Item.ItemIndex] = _personobj;
                Session["Question_Defendant"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;
                ViewState["SpEdit"] = 0;

                grdDefendant.EditItemIndex = PersonsList.Count;

                grdDefendant.DataSource = AddDefaultItems(PersonsList);
                grdDefendant.DataBind();
            }
            else if (e.CommandName.Equals("Delete"))
            {
                string code = e.Item.Cells[2].Text.Replace("&nbsp;", " ").Trim();

                List<Parliament_Requestedby> PersonsList = new List<Parliament_Requestedby>();
                if (Session["Question_Defendant"] != null)
                {
                    PersonsList = (List<Parliament_Requestedby>)Session["Question_Defendant"];
                }
                if (code.ToString() != "0" && code.ToString() != "-1")
                {
                    objRepository.DeletePersons(PersonsList[e.Item.ItemIndex]);

                }

                PersonsList.RemoveAt(e.Item.ItemIndex);




                Session["Question_Defendant"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;
                ViewState["SpEdit"] = 0;

                grdDefendant.EditItemIndex = PersonsList.Count;

                grdDefendant.DataSource = AddDefaultItems(PersonsList);
                grdDefendant.DataBind();
            }
            else if (e.CommandName.Equals("Cancel"))
            {
                ViewState["SpEdit"] = 0;

                List<Parliament_Requestedby> PersonsList = new List<Parliament_Requestedby>();
                if (Session["Question_Defendant"] != null)
                {
                    PersonsList = (List<Parliament_Requestedby>)Session["Question_Defendant"];
                }
                grdDefendant.EditItemIndex = PersonsList.Count;
                grdDefendant.DataSource = AddDefaultItems(PersonsList);
                grdDefendant.DataBind();
            }
        }
        #endregion

        #region "Coming Letters"
        #region "Coming HElpers"
        private void FillinComingForm()
        {

            var objList = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));
            if ((objList != null))
            {
                txtDoc_Serial.Text = gets(objList.Doc_Serial);
                txtDoc_Subject.Text = gets(objList.Doc_Subject);
                txtComingNotes.Text = gets(objList.Doc_Notes);

                // txtFrom.Text= gets(objList.Doc_From);

                if (!gets(objList.Filepath).Equals(""))
                {
                    btnIncomingScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnIncomingScannerfilepath.Value = gets(objList.Filepath);
                }


                lstIncomingOrg.SelectedValue = gets(objList.Doc_FromId);
                txtComingDate.Text = NullDateifEmptyToText(objList.SentDate);
                txtComingReminderDate.Text = NullDateifEmptyToText(objList.NextFollowReminderDate);


            }

            divAddIncoming.Visible = true;
            divshowincoming.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private void FillArcData(int Doc_Type, int DocRefID)
        {
            if (Doc_Type == 1)
            {
                var objList = objRepository.FillArcDocs(Doc_Type, DocRefID);
                lblComingCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
                lblComingalert.Text = objList.Count.ToString();
                decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdincoming.PageSize));
                if ((c <= grdincoming.CurrentPageIndex))
                {
                    grdincoming.CurrentPageIndex = 0;
                }

                //List Duplication
                //List<View_InboundItems> duplicatedList = new List<View_InboundItems>();
                //duplicatedList = DuplicatedList(objList);

                if (objList.Count > 0)
                {
                    //btnSave.Visible = true;
                    //lnkBack.Visible = true;

                    divshowincoming.Visible = true;
                    pager2.Visible = true;
                    grdincoming.Visible = true;

                }
                else
                {
                    // divshowProcesure.Visible = false;
                    //pager2.Visible = false;
                    //string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                }

                //var duplicatedList = objList.SelectMany(t =>
                //  Enumerable.Repeat(t, 2)).ToList();

                grdincoming.DataSource = objList;
                grdincoming.DataBind();
                pager2.ItemCount = objList.Count;
            }
            else
            {

                var objList = objRepository.FillArcDocs(Doc_Type, DocRefID);
                lbloutgoingCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
                lbloutAlert.Text = objList.Count.ToString();
                decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdOutgoing.PageSize));
                if ((c <= grdOutgoing.CurrentPageIndex))
                {
                    grdOutgoing.CurrentPageIndex = 0;
                }

                //List Duplication
                //List<View_InboundItems> duplicatedList = new List<View_InboundItems>();
                //duplicatedList = DuplicatedList(objList);

                if (objList.Count > 0)
                {
                    //btnSave.Visible = true;
                    //lnkBack.Visible = true;

                    divoutgoingshow.Visible = true;
                    pager3.Visible = true;
                    grdOutgoing.Visible = true;

                }
                else
                {
                    // divshowProcesure.Visible = false;
                    //pager2.Visible = false;
                    //string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                }

                //var duplicatedList = objList.SelectMany(t =>
                //  Enumerable.Repeat(t, 2)).ToList();

                grdOutgoing.DataSource = objList;
                grdOutgoing.DataBind();
                pager3.ItemCount = objList.Count;

            }


        }
        private void ClearComingDoc()
        {
            txtDoc_Serial.Text = "";
            txtDoc_Subject.Text = "";
            txtComingDate.Text = "";
            txtComingReminderDate.Text = "";

            txtOutDocNo.Text = "";
            txtOutSubject.Text = "";
            txtoutDate.Text = "";
            txtOutReminderDate.Text = "";



            divAddIncoming.Visible = false;
            divshowincoming.Visible = true;

            divOutgiongAdd.Visible = false;
            divoutgoingshow.Visible = true;


        }
        private int SaveComingDocInformation(int DirectionType, int RefDocID)
        {
            string script = "";
            arc_Data obj = new arc_Data();
            try
            {


                if (ViewState["QuestionArcID"].Equals("0"))
                {//Save

                    obj.TransactionDate = DateTime.Now;
                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.QuestionsModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtDoc_Serial.Text);
                    obj.Doc_Subject = gets(txtDoc_Subject.Text);
                    obj.RefDocID = RefDocID;

                    obj.Doc_From = lstIncomingOrg.SelectedItem.Text;
                    obj.Doc_FromId = ZeroIntergerIFNull(lstIncomingOrg.SelectedValue);

                    obj.Doc_To = "CMGS";
                    obj.Doc_Notes = txtComingNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtComingDate.Text);
                    if (txtComingReminderDate.Text != "")
                    {
                        obj.NextFollowReminderDate = NullDateifEmpty(txtComingReminderDate.Text);
                    }

                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());



                    objRepository.AddArcData(obj);
                    ViewState["QuestionArcID"] = gets(obj.Code);


                }
                else
                { //Update


                    obj = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));


                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.QuestionsModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtDoc_Serial.Text);
                    obj.Doc_Subject = gets(txtDoc_Subject.Text);
                    obj.RefDocID = RefDocID;



                    obj.Doc_From = lstIncomingOrg.SelectedItem.Text;
                    obj.Doc_FromId = ZeroIntergerIFNull(lstIncomingOrg.SelectedValue);


                    obj.Doc_To = "CMGS";
                    obj.Doc_Notes = txtComingNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtComingDate.Text);
                    if (txtComingReminderDate.Text != "")
                    {
                        obj.NextFollowReminderDate = NullDateifEmpty(txtComingReminderDate.Text);
                    }
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                    // Parliament_QuestionsAnswers objAnswer = new Parliament_QuestionsAnswers();


                    objRepository.UpdateArcData(obj);

                }
                string folderpath = obj.RefDocID.ToString() + "/" + (obj.Doc_Type == 1 ? "incoming/" : "outgoing/") + obj.Code.ToString() + "/";
                string _img = UploadFileoServer(txtIncomingImge, ScannerRepository + _TargetUploadPath + folderpath);
                if (_img != "")
                {
                    var objAcrForUpdate = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));
                    objAcrForUpdate.Filepath = _img;
                    objRepository.UpdateArcData(objAcrForUpdate);
                }


                ViewState["QuestionArcID"] = obj.Code;

                ClearComingDoc();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            FillArcData(DirectionType, RefDocID);
            FilLQuestionAnswer(RefDocID);
            return obj.Code;
        }
        protected void grdincoming_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["QuestionArcID"] = e.Item.Cells[0].Text;
            divAddIncoming.Visible = false;
            FillinComingForm();

        }

        #endregion

        protected void Lnkincoming_Click(object sender, EventArgs e)
        {
            ViewState["outgoingCode"] = "0";
            ViewState["QuestionArcID"] = "0";

            divAddIncoming.Visible = true;
            divshowincoming.Visible = false;
        }

        protected void lnkSaveIncoming_Click(object sender, EventArgs e)
        {
            if (ViewState["QuestionitemID"].ToString() != "0" && ViewState["QuestionitemID"].ToString() != "")
            {
                SaveComingDocInformation(1, ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Question Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }


        }

        protected void lnkCancelIncoming_Click(object sender, EventArgs e)
        {
            ClearComingDoc();
        }

        protected void pager_Command2(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdincoming.PageCount))
            {
                currnetPageIndx = (grdincoming.PageCount - 1);
            }

            pager2.CurrentIndex = currnetPageIndx;
            grdincoming.CurrentPageIndex = (currnetPageIndx - 1);
            FillArcData(1, ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));
        }
        protected void grdincoming_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }

        protected void lnkDeleteIncoming_Click(object sender, EventArgs e)
        {
            try
            {

                arc_Data obj = new arc_Data();
                for (int i = 0; i <= grdincoming.Items.Count - 1; i++)
                {

                    if ((grdincoming.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdincoming.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteArcData((arc_Data)objRepository.GetArcDocDetails(ZeroIntergerIFNull(grdincoming.Items[i].Cells[0].Text)));
                        }
                    }
                }
                FillArcData(1, ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));

            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteDataFailed + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
        }

        #endregion

        #region "Outgiong"

        private int SaveOutDocInformation(int DirectionType, int RefDocID)
        {
            string script = "";


            arc_Data obj = new arc_Data();
            try
            {

                if (ViewState["QuestionArcID"].Equals("0"))
                {//Save

                    obj.TransactionDate = DateTime.Now;
                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.QuestionsModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtOutDocNo.Text);
                    obj.Doc_Subject = gets(txtOutSubject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = "CMGS";



                    obj.Doc_To = lstoutgoiningOrg.SelectedItem.Text;
                    obj.Doc_ToId = ZeroIntergerIFNull(lstoutgoiningOrg.SelectedValue);

                    obj.Doc_Notes = txtoutNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtoutDate.Text);


                    if (txtOutReminderDate.Text != "")
                    {
                        obj.NextFollowReminderDate = NullDateifEmpty(txtOutReminderDate.Text);
                    }

                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                    objRepository.AddArcData(obj);

                    ViewState["QuestionArcID"] = gets(obj.Code);


                }
                else
                { //Update


                    obj = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));


                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.QuestionsModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtOutDocNo.Text);
                    obj.Doc_Subject = gets(txtOutSubject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = "CMGS";

                    obj.Doc_To = lstoutgoiningOrg.SelectedItem.Text;
                    obj.Doc_ToId = ZeroIntergerIFNull(lstoutgoiningOrg.SelectedValue);

                    obj.Doc_Notes = txtoutNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtoutDate.Text);
                    if (txtOutReminderDate.Text != "")
                    {
                        obj.NextFollowReminderDate = NullDateifEmpty(txtOutReminderDate.Text);
                    }
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());




                    objRepository.UpdateArcData(obj);

                }

                string folderpath = obj.RefDocID.ToString() + "/" + (obj.Doc_Type == 1 ? "incoming/" : "outgoing/") + obj.Code.ToString() + "/";
                string _img = UploadFileoServer(txtoutgoiningImage, ScannerRepository + _TargetUploadPath + folderpath);
                if (_img != "")
                {
                    var objAcrForUpdate = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));
                    objAcrForUpdate.Filepath = _img;
                    objRepository.UpdateArcData(objAcrForUpdate);
                }

                ViewState["QuestionArcID"] = obj.Code;

                ClearComingDoc();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            FillArcData(DirectionType, RefDocID);
            return obj.Code;
        }
        protected void ClearComingDoc(object sender, EventArgs e)
        {
            ViewState["outgoingCode"] = "0";
            ViewState["QuestionArcID"] = "0";



            divAddIncoming.Visible = false;
            divshowincoming.Visible = false;

            divOutgiongAdd.Visible = true;
            divoutgoingshow.Visible = false;
        }

        protected void lnkCancelOut_Click(object sender, EventArgs e)
        {
            ClearComingDoc();
        }


        protected void lnkAddNewOutGoing_Click(object sender, EventArgs e)
        {
            ViewState["outgoingCode"] = "0";
            ViewState["QuestionArcID"] = "0";

            divOutgiongAdd.Visible = true;
            divoutgoingshow.Visible = false;
        }
        protected void lnkDeleteOutgoing_Click(object sender, EventArgs e)
        {
            try
            {

                arc_Data obj = new arc_Data();
                for (int i = 0; i <= grdOutgoing.Items.Count - 1; i++)
                {

                    if ((grdOutgoing.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdOutgoing.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteArcData((arc_Data)objRepository.GetArcDocDetails(ZeroIntergerIFNull(grdOutgoing.Items[i].Cells[0].Text)));
                        }
                    }
                }
                FillArcData(2, ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));

            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteDataFailed + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
        }

        protected void grdOutgoing_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }

        protected void lnkSaveOut_Click(object sender, EventArgs e)
        {
            if (ViewState["QuestionitemID"].ToString() != "0" && ViewState["QuestionitemID"].ToString() != "")
            {
                SaveOutDocInformation(2, ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Case Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

        }
        protected void pager_Command3(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdOutgoing.PageCount))
            {
                currnetPageIndx = (grdOutgoing.PageCount - 1);
            }

            pager3.CurrentIndex = currnetPageIndx;
            grdOutgoing.CurrentPageIndex = (currnetPageIndx - 1);
            FillArcData(2, ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));
        }

        private void FillOutForm()
        {

            var objList = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));
            if ((objList != null))
            {
                txtOutDocNo.Text = gets(objList.Doc_Serial);
                txtOutSubject.Text = gets(objList.Doc_Subject);
                txtoutNotes.Text = gets(objList.Doc_Notes);

                // txtto.Text = gets(objList.Doc_To);

                lstoutgoiningOrg.SelectedValue = gets(objList.Doc_ToId);

                txtoutDate.Text = NullDateifEmptyToText(objList.SentDate);
                txtOutReminderDate.Text = NullDateifEmptyToText(objList.NextFollowReminderDate);

                if (!gets(objList.Filepath).Equals(""))
                {
                    btnOutgoingScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnOutgoingScannerfilepath.Value = gets(objList.Filepath);
                }
            }

            divOutgiongAdd.Visible = true;
            divoutgoingshow.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        protected void grdOutgoing_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["QuestionArcID"] = e.Item.Cells[0].Text;
            divOutgiongAdd.Visible = false;
            FillOutForm();
        }
        #endregion


        #region "Scanning"


        private void UpdateScannedFile()
        {
            Parliament_Questions objQuestion = new Parliament_Questions();
            Parliament_QuestionsAnswers objAnswer = new Parliament_QuestionsAnswers();

            arc_Data objArc = new arc_Data();
            if (Request.Form["fileList"] != null)
            {
                if (Request.Form["fileList"].ToString() != "")
                {

                    switch (Request.Form["ActiveTab"].ToString())
                    {
                        case "1":
                            {//Question Information


                                ViewState["itemID"] = Request.Form["TargetID"];
                                //  FillQuestionMasterInformation();
                                hdnactivetab.Value = "1";

                                string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                string[] FileList = _ScannerFileLlisy.Split(';');

                                if (Request.Form["TargetID"] != null)
                                {
                                    for (int i = 0; i < FileList.Length; i++)
                                    {

                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objQuestion = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objQuestion.Q_Attachment = null;
                                            objRepository.UpdateQuestion(objQuestion);
                                        }
                                        else
                                        {
                                            objQuestion = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objQuestion.Q_Attachment = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateQuestion(objQuestion);

                                        }




                                    }
                                    Response.Redirect("QuestionsData.aspx?activetab=1&QuestionID=" + Request.Form["TargetID"]);
                                    return;
                                }


                                else
                                {
                                    string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                }


                                break;
                            }
                        case "2":
                            {// Answer Details


                                ViewState["AnswerCode"] = Request.Form["TargetID"];
                                ViewState["itemID"] = Request.Form["FileID"];
                                //  FillQuestionMasterInformation();
                                hdnactivetab.Value = "2";

                                string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                string[] FileList = _ScannerFileLlisy.Split(';');

                                if (Request.Form["TargetID"] != null)
                                {
                                    for (int i = 0; i < FileList.Length; i++)
                                    {

                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objAnswer = objRepository.GetAnswersDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objAnswer.Answerattachments = null;
                                            objRepository.UpdateAnswers(objAnswer);
                                        }
                                        else
                                        {
                                            objAnswer = objRepository.GetAnswersDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objAnswer.Answerattachments = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateAnswers(objAnswer);

                                        }


                                    }
                                    Response.Redirect("QuestionsData.aspx?activetab=2&QuestionID=" + Request.Form["FileID"]);
                                    return;
                                }


                                else
                                {
                                    string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                }

                                break;
                            }
                        case "3":
                            {//Incoming


                                ViewState["QuestionArcID"] = Request.Form["TargetID"];
                                ViewState["itemID"] = Request.Form["FileID"];
                                //  FillQuestionMasterInformation();
                                hdnactivetab.Value = "3";

                                string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                string[] FileList = _ScannerFileLlisy.Split(';');

                                if (Request.Form["TargetID"] != null)
                                {
                                    for (int i = 0; i < FileList.Length; i++)
                                    {

                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));
                                            objArc.Filepath = null;
                                            objRepository.UpdateArcData(objArc);
                                        }
                                        else
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));
                                            objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateArcData(objArc);
                                        }


                                    }
                                    Response.Redirect("QuestionsData.aspx?activetab=3&QuestionID=" + Request.Form["FileID"]);
                                    return;
                                }


                                else
                                {
                                    string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                }

                                break;
                            }
                        case "4":
                            {//Outgoinung

                                ViewState["QuestionArcID"] = Request.Form["TargetID"];
                                ViewState["itemID"] = Request.Form["FileID"];
                                //  FillQuestionMasterInformation();
                                hdnactivetab.Value = "4";

                                string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                string[] FileList = _ScannerFileLlisy.Split(';');

                                if (Request.Form["TargetID"] != null)
                                {
                                    for (int i = 0; i < FileList.Length; i++)
                                    {
                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));
                                            objArc.Filepath = null;
                                            objRepository.UpdateArcData(objArc);
                                        }
                                        else
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));
                                            objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateArcData(objArc);
                                        }


                                    }
                                    Response.Redirect("QuestionsData.aspx?activetab=4&QuestionID=" + Request.Form["FileID"]);
                                    return;
                                }


                                else
                                {
                                    string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                }

                                break;
                            }

                    }

                }


            }
        }

        protected void lnkQScan_Click(object sender, EventArgs e)
        {
            //Show Loadin div

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
                                                                           //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }
            int _TargetID = SaveQuestionMaster(true);



            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST' >";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + _TargetID.ToString() + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnQScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnQScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='1' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";
                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += "  CalllerForm.submit();";
                ScannerPostFrom += "</script>";

                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;



            }

        }
        protected void btnAnswerScan_Click(object sender, EventArgs e)
        {

            //Show Loadin div

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            int _TargetID = 0;                                                             //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }
            if (ViewState["QuestionitemID"].ToString() != "0" && ViewState["QuestionitemID"].ToString() != "")
            {
                _TargetID = SaveAnswerInformation(ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Question Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["QuestionitemID"].ToString() + "/answers/" + _TargetID.ToString() + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnAnswerScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnAnswerScannerfilepath.Value + "]' />";

                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["QuestionitemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='2' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";
                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += " CalllerForm.submit();";
                ScannerPostFrom += "</script>";




                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;



            }


        }
        protected void btnIncomingScan_Click(object sender, EventArgs e)
        {

            //Show Loadin div

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            int _TargetID = 0;                                                             //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }
            if (ViewState["QuestionitemID"].ToString() != "0" && ViewState["QuestionitemID"].ToString() != "")
            {
                _TargetID = SaveComingDocInformation(1, ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Question Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["QuestionitemID"].ToString() + "/incoming/" + _TargetID.ToString() + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnIncomingScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnIncomingScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["QuestionitemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='3' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";
                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += "  CalllerForm.submit();";
                ScannerPostFrom += "</script>";

                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;


            }
        }
        protected void btnOutgoingScan_Click(object sender, EventArgs e)
        {

            //Show Loadin div

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            int _TargetID = 0;                                                             //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }
            if (ViewState["QuestionitemID"].ToString() != "0" && ViewState["QuestionitemID"].ToString() != "")
            {
                _TargetID = SaveOutDocInformation(2, ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Question Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {

                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["QuestionitemID"].ToString() + "/outgoing/" + _TargetID.ToString() + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnOutgoingScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnOutgoingScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["QuestionitemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='4' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";
                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += "  CalllerForm.submit();";
                ScannerPostFrom += "</script>";

                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;


            }
        }

        #endregion

        #region "Answer"
        #region "Answer HElpers"
        private void FillAnswerForm()
        {

            var objList = objRepository.GetAnswersDetails(ZeroIntergerIFNull(ViewState["AnswerCode"].ToString()));
            if ((objList != null))
            {
                txtAnswerNotes.Text = gets(objList.AnswerNotes);
                txtAnswerDate.Text = NullDateifEmptyToText(objList.AnswerDate);
                txtAnswerText.Text = gets(objList.AnswerText);

                if (!gets(objList.Answerattachments).Equals(""))
                {
                    btnAnswerScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل نص الاجابة";
                    hdnAnswerScannerfilepath.Value = gets(objList.Answerattachments);
                }


            }

            DivAddAnswer.Visible = true;
            divShowAnswer.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private void FilLQuestionAnswer(int DocRefID)
        {

            var objList = objRepository.FillQuestionAnswers(DocRefID);
            //  lblComingCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
            lblAnswercount.Text = objList.Count.ToString();
            decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdAnswerList.PageSize));
            if ((c <= grdAnswerList.CurrentPageIndex))
            {
                grdAnswerList.CurrentPageIndex = 0;
            }

            //List Duplication
            //List<View_InboundItems> duplicatedList = new List<View_InboundItems>();
            //duplicatedList = DuplicatedList(objList);

            if (objList.Count > 0)
            {
                //btnSave.Visible = true;
                //lnkBack.Visible = true;

                divShowAnswer.Visible = true;
                pager5.Visible = true;
                grdAnswerList.Visible = true;

            }


            //var duplicatedList = objList.SelectMany(t =>
            //  Enumerable.Repeat(t, 2)).ToList();

            grdAnswerList.DataSource = objList;
            grdAnswerList.DataBind();
            pager5.ItemCount = objList.Count;
        }
        private void ClearAnswerforms()
        {
            ViewState["AnswerCode"] = "0";
            txtAnswerDate.Text = "";
            txtAnswerNotes.Text = "";
            txtAnswerText.Text = "";

            DivAddAnswer.Visible = false;
            divShowAnswer.Visible = true;



        }
        private int SaveAnswerInformation(int RefDocID)
        {
            string script = "";
            Parliament_QuestionsAnswers obj = new Parliament_QuestionsAnswers();
            try
            {



                if (ViewState["AnswerCode"].Equals("0"))
                {//Save
                    obj.CreationDate = DateTime.Now;
                    obj.LastActionDate = DateTime.Now;

                    obj.QuestionID = RefDocID;
                    obj.AnswerNotes = txtAnswerNotes.Text;
                    obj.AnswerDate = NullDateifEmpty(txtAnswerDate.Text);

                    obj.AnswerText = gets(txtAnswerText.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                    objRepository.AddAnswers(obj);

                    //// update Question Status to  Answered
                    //var QuestionDetails = objRepository.GetDetails(RefDocID);
                    //if (QuestionDetails != null)
                    //{
                    //    QuestionDetails.StatusID = 1;
                    //    objRepository.UpdateQuestion(QuestionDetails);
                    //}

                }
                else
                { //Update


                    obj = objRepository.GetAnswersDetails(ZeroIntergerIFNull(ViewState["AnswerCode"].ToString()));
                    obj.LastActionDate = DateTime.Now;
                    obj.QuestionID = RefDocID;
                    obj.AnswerNotes = txtAnswerNotes.Text;
                    obj.AnswerDate = NullDateifEmpty(txtAnswerDate.Text);

                    obj.AnswerText = gets(txtAnswerText.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());



                    objRepository.UpdateAnswers(obj);

                }

                string _img = UploadFileoServer(txtAnswerimage, ScannerRepository + _TargetUploadPath + RefDocID + "/answers/" + obj.Code.ToString() + "/");
                if (_img != "")
                {
                    var objAnswerForUpdate = objRepository.GetAnswersDetails(obj.Code);
                    objAnswerForUpdate.Answerattachments = _img;
                    objRepository.UpdateAnswers(objAnswerForUpdate);
                }
                ViewState["AnswerCode"] = obj.Code;

                ClearAnswerforms();



            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            FilLQuestionAnswer(RefDocID);
            return obj.Code;
        }


        #endregion
        protected void grdAnswerList_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["AnswerCode"] = e.Item.Cells[0].Text;
            DivAddAnswer.Visible = true;
            divShowAnswer.Visible = false;
            FillAnswerForm();

        }
        protected void lnkAddAnswer_Click(object sender, EventArgs e)
        {
            ViewState["AnswerCode"] = "0";
            DivAddAnswer.Visible = true;
            divShowAnswer.Visible = false;
        }

        protected void lnkSaveAnswer_Click(object sender, EventArgs e)
        {
            if (ViewState["QuestionitemID"].ToString() != "0" && ViewState["QuestionitemID"].ToString() != "")
            {
                SaveAnswerInformation(ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));
                string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Question Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }


        }

        protected void lnkCancelAnswer_Click(object sender, EventArgs e)
        {
            ClearAnswerforms();
        }

        protected void pager_Command5(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdAnswerList.PageCount))
            {
                currnetPageIndx = (grdAnswerList.PageCount - 1);
            }

            pager5.CurrentIndex = currnetPageIndx;
            grdAnswerList.CurrentPageIndex = (currnetPageIndx - 1);
            FilLQuestionAnswer(ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));
        }
        protected void grdAnswerList_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }

        protected void lnkDeleteAnswer_Click(object sender, EventArgs e)
        {
            try
            {

                Parliament_QuestionsAnswers obj = new Parliament_QuestionsAnswers();
                for (int i = 0; i <= grdAnswerList.Items.Count - 1; i++)
                {

                    if ((grdAnswerList.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdAnswerList.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteAnswers((Parliament_QuestionsAnswers)objRepository.GetAnswersDetails(ZeroIntergerIFNull(grdAnswerList.Items[i].Cells[0].Text)));
                        }
                    }
                }
                FilLQuestionAnswer(ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));

            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteDataFailed + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
        }

        #endregion

        protected void lstFilterChapter_SelectedIndexChanged(object sender, EventArgs e)
        {

            selectedChapter = lstChapter.SelectedValue;
            ViewState["SpChapterChanged"] = "1";
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterSession, "NameAr", "Code", "الكل");

            // FillQuestionPersons(ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));

            FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterPerson, "NameAr", "Code", "الكل");
        }

        protected void lstChapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedChapter = lstChapter.SelectedValue;
            ViewState["SpChapterChanged"] = "1";
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstChapter.SelectedValue)), ref lstSession, "NameAr", "Code", "اختر");
            // FillQuestionPersons(ZeroIntergerIFNull(ViewState["QuestionitemID"].ToString()));


            FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstChapter.SelectedValue)), ref lstRequestFrom, "NameAr", "Code", "اختر");

            //Fill person List [Drop List]
            var requestedFromPersonList = objLookup.FillOMaPerson(ZeroIntergerIFNull(lstChapter.SelectedValue));
            Session["RequestedFromPersonList"] = requestedFromPersonList;
        }



        protected void lstSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedChapter = lstChapter.SelectedValue;
            ViewState["SpChapterChanged"] = "1";
        }

        protected void grdQuestionsList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                objRepository.DeleteQuestion((Parliament_Questions)objRepository.GetDetails(ZeroIntergerIFNull(e.Item.Cells[3].Text)));
            }
            FillQuestions();
        }
    }
}