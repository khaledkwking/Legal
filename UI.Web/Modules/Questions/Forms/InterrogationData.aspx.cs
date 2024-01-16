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
    public partial class InterrogationData : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public QuestionsRepository objRepository = IoC.Resolve<QuestionsRepository>();
        public string _PageTitle = "نظام الأسئلة البرلمانية   ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "Interrogation/";
        public string _MadbataTargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "Parliament_madbata/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        public string selectedChapter = "0";
        public string QrelatedOrg = "";

        public bool hasMadbata = false;
        #endregion

        #region "Page Events"
        public string viewMadabata()
        {
            return ViewState["itemID"] != null && ViewState["itemID"].ToString() != "0"  ? "": "none";
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

            applyUserPermission();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "QuestionsData.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";
            btnCancel.Attributes.Add("onclick", "Page_ValidationActive=false;");
            btnSave.Attributes.Add("onclick", "return chkImage();");



            if (!IsPostBack)
            {
                vote1.Style.Add("display", "none");
                vote2.Style.Add("display", "none");
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
                    lstFilterResult.SelectedValue = gets(Request.QueryString["StatusID"]);
                    FillInterrogation();

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


            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))

            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
            }


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
            FillInterrogation();
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
            Response.Redirect("/Modules/Questions/Forms/InterrogationData.aspx");

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillInterrogation();
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

        private void FillQuestionPersons(int QuestionID)
        {
            var objprosecutor = objRepository.FillPersons(QuestionID,1);// موجة من
            if ((Session["Question_prosecutor"] != null && ViewState["SpEdit"].ToString() == "1") ||(Session["Question_prosecutor"] != null && ViewState["SpChapterChanged"].ToString() == "1") )
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
                    QrelatedOrg += item.PersonID.ToString() +",";
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
        private void FillInterrogation()
        {

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
                ZeroIntergerIFNull(lstFilterRequestTo.SelectedValue),txtFilterSubject.Text,ZeroIntergerIFNull(lstFilterRelatedOrgs.SelectedValue)
                ,ZeroIntergerIFNull(lstFilterAssignedPerson.SelectedValue), ZeroIntergerIFNull(lstFilterResult.SelectedValue),
                NullDateifEmpty(txtFilterQ_DiscussionDateFrom.Text), NullDateifEmpty(txtFilterQ_DiscussionDateTo.Text),false, MapSearchKeys());


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

            // var duplicatedList = objList.SelectMany(t =>
            //Enumerable.Repeat(t, 2)).ToList();

            

            grdQuestionsList.DataSource = objList;
            grdQuestionsList.DataBind();
            pager1.ItemCount = objList.Count;

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
            lstResult.SelectedValue = "0";



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
                case 2:
                    {
                        _out = "<span class='label bg-danger-400'>"+ StatusName + "  </span>";
                        break;
                    }
                case 3 :
                case 4:
                case 5:
                    {
                        _out = "<span class='label bg-warning-400'>"+ StatusName + " </span>";
                        break;
                    }
               
                case 6:
                case 7:
                case 8:
                    {
                        _out = "<span class='label bg-grey-400'>"+ StatusName + "</span>";
                        break;
                    }
                case 9:
                    {
                        _out = "<span class='label bg-success-400'>" + StatusName + "  </span>";
                        break;
                    }
                case 10:
                    {
                        _out = "<span class='label bg-blue-400'>" + StatusName + "</span>";
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
        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;


            btnSave.Visible = userAccess.Edit ||  userAccess.Add;


            //btnDelete.Visible = userAccess.Delete;
            grdQuestionsList.Columns[14].Visible = userAccess.Delete;

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
            FillInterrogation();
        }


        #endregion

         #region "Helper Methods"


        private void FillQuestionMasterInformation()
        {

            var objList = objRepository.GetViewDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {
                hdnMasterID.Value = gets(objList.code);
                ViewState["QuestionitemID"] = gets(objList.code);

                txtQuestionInternalSerial.Text = gets(objList.Q_Serial);
                txtQoOmaYear.Text = gets(objList.OmaYear);
                txtOma_Serial.Text = gets(objList.Oma_Serial);
                txtQuestionNote.Text = gets(objList.Q_Notes);
                txtQuestionDate.Text = NullDateifEmptyToText(objList.Q_Date).ToString();
                txtQ_DiscussionDate.Text = NullDateifEmptyToText(objList.Q_DiscussionDate).ToString();


                lstChapter.SelectedValue = gets(objList.ChapterID);
                selectedChapter = gets(objList.ChapterID);



                txtVoteWith.Text = objList.Q_Result_VoteWith.ToString();
                txtVoteabsent.Text = objList.Q_Result_absent.ToString();
                txtVoteAgenest.Text = objList.Q_Result_VoteAgainst.ToString();
                txtVoteVoid.Text = objList.Q_Result_Void.ToString();




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

                lstResult.SelectedValue = gets(objList.Q_Result);

                if (objList.Q_Result == 1)
                {
                    vote1.Style.Add("display", "");
                    vote2.Style.Add("display", "");

                }
                else
                {
                    vote1.Style.Add("display", "none");
                    vote2.Style.Add("display", "none");
                }

                chkIsGrouped.Checked = getBool(objList.Is_grouped);

                txtQuestionSubject.Text = gets(objList.Q_Text);
                if (!gets(objList.Q_Attachment).Equals(""))
                {
                    anchorAttachment.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath+gets(objList.code) +"/" + "&vfileList=[" + gets(objList.Q_Attachment) + ";]";
                    anchorAttachment.Visible = true;
                    hasMadbata = true;

                }

                if (!gets(objList.Q_Attachment).Equals(""))
                {
                    lnkQScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل نص الاستجواب";
                    hdnQScannerfilepath.Value = gets(objList.Q_Attachment);
                }

                if (!gets(objList.M_Attachment).Equals(""))
                {
                    hdnMadbataFile.Value = gets(objList.M_Attachment);
                }

                //Fill Person Information
                FillQuestionPersons(objList.code);
            }

            tblAdd.Visible = true;
            ////blSubTitle.Text = this.GetTitle(false);
            //if (hdnMadbataFile.Value != "")
            //{
            //    lnkMadbata.Visible = true;
            //    lnkMadbata.HRef = ScannerRepositoryViewer + "?targetpath=" + _MadbataTargetUploadPath + "&vfileList=[" + hdnMadbataFile.Value + ";]";
            //}
            //else {
            //    lnkMadbata.Visible = false;
            //}


        }
        private int SaveQuestionMaster(bool fromScan=false)
        {
            string script = "";

            Parliament_Questions objQuestion = new Parliament_Questions();

            try
            {
                if (ViewState["itemID"].Equals("0"))
                {//Save


                    objQuestion.QType = 2;
                    objQuestion.CreationDate = DateTime.Now;
                    objQuestion.LastActionDate = DateTime.Now;

                    objQuestion.Q_Serial = ZeroIntergerIFNull(txtQuestionInternalSerial.Text);
                    objQuestion.Oma_Serial = gets(txtOma_Serial.Text);
                    objQuestion.OmaYear = ZeroIntergerIFNull(txtQoOmaYear.Text);


                    objQuestion.Q_Text = txtQuestionSubject.Text;
                    objQuestion.Q_Notes = txtQuestionNote.Text;

                    objQuestion.Q_Date = NullDateifEmpty(txtQuestionDate.Text);

                    objQuestion.Q_DiscussionDate = NullDateifEmpty(txtQ_DiscussionDate.Text);


                    objQuestion.ChapterID = ZeroIntergerIFNull(lstChapter.SelectedValue);
                    objQuestion.SessionID = ZeroIntergerIFNull(lstSession.SelectedValue);
                    objQuestion.Q_RequestTo = ZeroIntergerIFNull(lstRequestTo.SelectedValue);

                    //Set from With the first person in list
                    if (hdnRelatedOrg.Value!="")
                    {
                        string[] SelectedOrgs = hdnRelatedOrg.Value.Split(',');
                        objQuestion.Q_RequestFrom = ZeroIntergerIFNull(SelectedOrgs[0]);

                    }

                    objQuestion.Q_Result = ZeroIntergerIFNull(lstResult.SelectedValue);
                    objQuestion.Is_grouped = getBool(chkIsGrouped.Checked);



                    objQuestion.Q_Result_VoteWith = ZeroIntergerIFNull( txtVoteWith.Text);
                    objQuestion.Q_Result_VoteAgainst = ZeroIntergerIFNull(txtVoteAgenest.Text);
                    objQuestion.Q_Result_Void = ZeroIntergerIFNull(txtVoteVoid.Text);
                    objQuestion.Q_Result_absent = ZeroIntergerIFNull(txtVoteabsent.Text);



                    objQuestion.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    objRepository.AddQuestion(objQuestion);
                    hdnMasterID.Value = gets(objQuestion.code);
                    ViewState["itemID"] = gets(objQuestion.code);

                }
                else
                { //Update


                    hdnMasterID.Value = ViewState["itemID"].ToString();
                    objQuestion = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                    objQuestion.LastActionDate = DateTime.Now;

                    objQuestion.Q_Serial = ZeroIntergerIFNull(txtQuestionInternalSerial.Text);
                    objQuestion.Oma_Serial = gets(txtOma_Serial.Text);
                    objQuestion.OmaYear = ZeroIntergerIFNull(txtQoOmaYear.Text);


                    objQuestion.Q_Text = txtQuestionSubject.Text;
                    objQuestion.Q_Notes = txtQuestionNote.Text;

                    objQuestion.Q_Date = NullDateifEmpty(txtQuestionDate.Text);
                    objQuestion.Q_DiscussionDate = NullDateifEmpty(txtQ_DiscussionDate.Text);
                    // objQuestion.AssignedPersonID = ZeroIntergerIFNull(lstassignedPersons.SelectedValue);

                    objQuestion.ChapterID = ZeroIntergerIFNull(lstChapter.SelectedValue);
                    objQuestion.SessionID = ZeroIntergerIFNull(lstSession.SelectedValue);
                    objQuestion.Q_RequestTo = ZeroIntergerIFNull(lstRequestTo.SelectedValue);
                    //Set from With the first person in list
                    if (hdnRelatedOrg.Value != "")
                    {
                        string[] SelectedOrgs = hdnRelatedOrg.Value.Split(',');
                        objQuestion.Q_RequestFrom = ZeroIntergerIFNull(SelectedOrgs[0]);

                    }
                    objQuestion.Q_Result = ZeroIntergerIFNull(lstResult.SelectedValue);



                    objQuestion.Q_Result_VoteWith = ZeroIntergerIFNull(txtVoteWith.Text);
                    objQuestion.Q_Result_VoteAgainst = ZeroIntergerIFNull(txtVoteAgenest.Text);
                    objQuestion.Q_Result_Void = ZeroIntergerIFNull(txtVoteVoid.Text);
                    objQuestion.Q_Result_absent = ZeroIntergerIFNull(txtVoteabsent.Text);
                    objQuestion.Is_grouped = getBool(chkIsGrouped.Checked);
                    objQuestion.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                    objRepository.UpdateQuestion(objQuestion);
                }


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
                    anchorAttachment.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(objQuestion.code)+"/"+ "&vfileList=[" + gets(objQuestion.Q_Attachment) + ";]";
                    anchorAttachment.Visible = true;
                    hasMadbata = true;

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
                    else {

                        Session["Question_Defendant"] = null;
                        Session["Question_prosecutor"] = null;
                        SavePersons(objQuestion.code);
                    }


                    script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

            if (!fromScan)
            {
                Response.Redirect("InterrogationData.aspx?ss=1&QuestionID=" + gets(objQuestion.code));
                return 0;
            }
            return objQuestion.code;
        }
        private void fillLookups()
        {


            FillDllwithoptional_ALL(objLookup.FillParliament_AssignedPerson(),ref lstassignedPersons, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillParliament_AssignedPerson(), ref lstFilterAssignedPerson, "NameAr", "Code", "الكل");


            FillDllwithoptional_ALL(objLookup.FillParliament_QuestionResult(), ref lstResult, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillParliament_QuestionResult(), ref lstFilterResult, "NameAr", "Code", "الكل");

            FillDll(objLookup.FillParliament_legislativeChapter(), ref lstChapter, "NameAr", "Code");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeChapter(), ref lstFilterChapter , "NameAr", "Code", "الكل");


            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull( lstChapter.SelectedValue)), ref lstSession, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterSession , "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillParliament_RequestedToPerson(), ref lstFilterRequestTo, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillParliament_RequestedToPerson(), ref  lstRequestTo, "NameAr", "Code", "اختر");

            FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull( lstFilterChapter.SelectedValue)), ref lstFilterPerson, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstChapter.SelectedValue)), ref lstRequestFrom, "NameAr", "Code", "اختر");


            FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterRelatedOrgs, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstChapter.SelectedValue)), ref lstRelatedOrgs, "NameAr", "Code", "اختر");
            lstRelatedOrgs.SelectedIndex = -1;



            var requestedFromPersonList = objLookup.FillOMaPerson(ZeroIntergerIFNull( lstChapter.SelectedValue));
            Session["RequestedFromPersonList"] = requestedFromPersonList;


            var relatedOrgs = objLookup.FillParliament_RelatedOrgs();
            Session["relatedOrgs"] = relatedOrgs;

        }

        public string activeTab(int tabindex)
        {
            string _out = "";
            if (hdnactivetab.Value =="" && tabindex == 1)
            {
                _out = "active";
            } else if (ZeroIntergerIFNull( hdnactivetab.Value) == tabindex)
            {
                _out = "active";
            }


            return _out;

        }
        public string ValidateMadbataExistance(int QuestionID)
        {
            return !objRepository.validateQuestionMadbata(QuestionID) ? "none" : "";

        }

        #endregion

        #region "Procedure Methods"



        #endregion

        #region "Persons"

        private void SavePersons(int QuestionCode)
        {
            // Delete Reated persons

            var objprosecutor = objRepository.FillPersons(QuestionCode, 2);// موجة من
            if (objprosecutor!=null)
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
            if (objprosecutor.Where(i=>i.PersonID==personID).ToList().Count>0)
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
            if (objprosecutor.Where(i=>i.PersonID== OrgID).ToList().Count>0)
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
                                    {//Update Current Record With
                                     //if (File.Exists(Server.MapPath(ScannerRepository + _TargetUploadPath + FileList[i].Split(',')[0].ToString())))
                                     //{
                                     //    try
                                     //    {
                                     //        File.Delete(Server.MapPath(ScannerRepository + _TargetUploadPath + FileList[i].Split(',')[0].ToString()));
                                     //    }
                                     //    catch (Exception)
                                     //    {

                                        //    }

                                        //}

                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objQuestion = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objQuestion.Q_Attachment =null;
                                            objRepository.UpdateQuestion(objQuestion);
                                        }
                                        else
                                        {
                                            objQuestion = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objQuestion.Q_Attachment = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateQuestion(objQuestion);

                                        }


                                      

                                    }
                                    Response.Redirect("InterrogationData.aspx?activetab=1&QuestionID=" + Request.Form["TargetID"] );
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
                                    {//Update Current Record With
                                     //if (File.Exists(Server.MapPath(ScannerRepository + _TargetUploadPath + FileList[i].Split(',')[0].ToString())))
                                     //{
                                     //    try
                                     //    {
                                     //        File.Delete(Server.MapPath(ScannerRepository + _TargetUploadPath + FileList[i].Split(',')[0].ToString()));
                                     //    }
                                     //    catch (Exception)
                                     //    {

                                        //    }

                                        //}

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
                                    Response.Redirect("InterrogationData.aspx?activetab=2&QuestionID=" + Request.Form["FileID"]);
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
                                    {//Update Current Record With
                                     //if (File.Exists(Server.MapPath(ScannerRepository + _TargetUploadPath + FileList[i].Split(',')[0].ToString())))
                                     //{
                                     //    try
                                     //    {
                                     //        File.Delete(Server.MapPath(ScannerRepository + _TargetUploadPath + FileList[i].Split(',')[0].ToString()));
                                     //    }
                                     //    catch (Exception)
                                     //    {

                                        //    }

                                        //}

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
                                    Response.Redirect("InterrogationData.aspx?activetab=3&QuestionID=" + Request.Form["FileID"]);
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
                                    {//Update Current Record With
                                     //if (File.Exists(Server.MapPath(ScannerRepository + _TargetUploadPath + FileList[i].Split(',')[0].ToString())))
                                     //{
                                     //    try
                                     //    {
                                     //        File.Delete(Server.MapPath(ScannerRepository + _TargetUploadPath + FileList[i].Split(',')[0].ToString()));
                                     //    }
                                     //    catch (Exception)
                                     //    {

                                        //    }

                                        //}

                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));
                                            objArc.Filepath =null;
                                            objRepository.UpdateArcData(objArc);
                                        }
                                        else
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["QuestionArcID"].ToString()));
                                            objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateArcData(objArc);
                                        }


                                      
                                    }
                                    Response.Redirect("InterrogationData.aspx?activetab=4&QuestionID=" + Request.Form["FileID"]);
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



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + gets(_TargetID) + "/" + "' />";
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


                //_TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&DocID=" + Request.QueryString["DocID"].ToString() + "&CaseID=" + Request.QueryString["CaseID"].ToString() + "&TargetID=" + _TargetID;
                //Response.Redirect(_TargetUrl);
            }

        }


        #endregion

        //#endregion

        protected void lstFilterChapter_SelectedIndexChanged(object sender, EventArgs e)
        {

            selectedChapter = lstFilterChapter.SelectedValue;
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
          FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstChapter.SelectedValue)), ref lstRelatedOrgs, "NameAr", "Code", "اختر");

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
            if (e.CommandName== "delete")
            {
                objRepository.DeleteQuestion((Parliament_Questions)objRepository.GetDetails(ZeroIntergerIFNull(e.Item.Cells[3].Text)));
            }
            FillInterrogation();
        }

        //protected void lstResult_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (lstResult.SelectedValue == "1")
        //    {
        //        vote1.Visible = true;
        //        vote2.Visible = true;
        //    }
        //    else {
        //        vote1.Visible = false;
        //        vote2.Visible = false;
        //    }


        //}
    }
}