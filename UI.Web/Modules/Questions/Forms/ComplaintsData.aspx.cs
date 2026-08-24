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
    public partial class ComplaintsData : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public CompaintsRepository objRepository = IoC.Resolve<CompaintsRepository>();
        public string _PageTitle = "نظام الاقتراحات برغبة     ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "suggestions/";

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



                ViewState["SpChapterChanged"] = "0";
                ViewState["SpEdit"] = "0";
                ViewState["NewDesc"] = "";
                ViewState["NewBar"] = "";
                ViewState["NewIsbn"] = "";
                ViewState["SPITEM"] = "";
                ViewState["NewPrice"] = "0";
                Session["ItemList"] = null;
                ViewState["itemID"] = "0";
                ViewState["ComplaintId"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["ComplaintArcID"] = "0";
                ViewState["AttachitemID"] = "0";
                ViewState["AnswerCode"] = "0";


                if (Request.QueryString["CompaintID"] == null)
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


                    ViewState["itemID"] = Request.QueryString["CompaintID"].ToString();
                    FillComplaintsMasterInformation();
                    //Fill Agreemnt Details


                }

                SetPageTitle();

                ViewState["OutboundItemID"] = "0";

                // FillInboundItems();
                UpdateScannedFile();

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
            Session["Complaints_prosecutor"] = null;
            Session["Complaints_Defendant"] = null;

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
            FillComplaints();
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
            ViewState["ComplaintId"] = "0";
            ViewState["ProceduresitemID"] = "0";
            Session["PersonsList"] = null;
            Response.Redirect("/Modules/Questions/Forms/complaintsData.aspx");

        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillComplaints();
        }
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            SaveComplaintsMaster();
        }
        protected void lnkAddNewComplaints_Click(object sender, EventArgs e)
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
                _keyList.Add(" رقم الشكوي   ", txtFilterserial.Text);

                _keyList.Add("تاريخ الشكوي من", txtFilterDatefrom.Text);
                _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                _keyList.Add(" جزء من نص الشكوي ", txtFilterSubject.Text);
                _keyList.Add("    مقدم الشكوي    ", txtFilterName.Text);

            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }
        private void FillComplaints()
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

            var objList = objRepository.GetList((int)Complaints_TypesEnum.Complaints, 0, 0, txtFilterserial.Text, NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), 0, 0, selectedRequestedFrom
                , txtFilterSubject.Text, ZeroIntergerIFNull(lstFilterCMGSRelatedOrgs.SelectedValue), txtFilterName.Text, MapSearchKeys());


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

            grdCompaintsList.DataSource = duplicatedList;
            grdCompaintsList.DataBind();

            pager1.ItemCount = objList.Count;

        }

        private void ClearCaseForm()
        {


            ViewState["ComplaintId"] = "0";

            txtCompaintSubject.Text = "";
            txtCompaintNote.Text = "";


            lstCMGSRelatedOrg.SelectedValue = "0";



            Session["PersonsList"] = null;


            Session["Complaints_prosecutor"] = null;
            Session["Complaints_Defendant"] = null;


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
            Lnkincoming.Visible = userAccess.Add;
            lnkAddNewOutGoing.Visible = userAccess.Add;


            //btnSave.Visible = userAccess.Edit || userAccess.Add;
            //lnkSaveIncoming.Visible = userAccess.Edit || userAccess.Add;
            //lnkSaveOut.Visible = userAccess.Edit || userAccess.Add;

            if (Request.QueryString["editflag"] != null)
            {
                string editflag = Request.QueryString["editflag"].ToString();

                btnSave.Visible = userAccess.Edit;

                lnkSaveIncoming.Visible = userAccess.Edit;
                lnkSaveOut.Visible = userAccess.Edit;
            }
            else
            {

                btnSave.Visible = userAccess.Edit || userAccess.Add;
                lnkSaveIncoming.Visible = userAccess.Edit || userAccess.Add;
                lnkSaveOut.Visible = userAccess.Edit || userAccess.Add;
            }


            //btnDelete.Visible = userAccess.Delete;
            lnkDeleteIncoming.Visible = userAccess.Delete;
            lnkDeleteOutgoing.Visible = userAccess.Delete;


            grdCompaintsList.Columns[7].Visible = userAccess.Delete;

        }


        protected void pager_Command(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdCompaintsList.PageCount))
            {
                currnetPageIndx = (grdCompaintsList.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdCompaintsList.CurrentPageIndex = (currnetPageIndx - 1);
            FillComplaints();
        }


        #endregion

        #region "Helper Methods"


        private void FillComplaintsMasterInformation()
        {

            var objList = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {
                hdnMasterID.Value = gets(objList.Code);
                ViewState["ComplaintId"] = gets(objList.Code);
                txtOwnerName.Text = gets(objList.Complaints_OwnerName);
                txtComplaintSerial.Text = gets(objList.Complaints_Serial);

                txtCompaintNote.Text = gets(objList.Notes);
                txtCompaintDate.Text = NullDateifEmptyToText(objList.Complaints_Date).ToString();

                //  lstChapter.SelectedValue = gets(objList.ChapterId);
                selectedChapter = gets(objList.ChapterId);

                //  lstChapter_SelectedIndexChanged(null, null);
                //selectedChapter = lstChapter.SelectedValue;
                //ViewState["SpChapterChanged"] = "1";
                //FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstChapter.SelectedValue)), lstSession, "NameAr", "Code", "اختر");
                //FillDllwithoptional_ALL(objLookup.FillOMaPerson(ZeroIntergerIFNull(lstChapter.SelectedValue)), lstRequestFrom, "NameAr", "Code", "اختر");
                //var requestedFromPersonList = objLookup.FillOMaPerson(ZeroIntergerIFNull(lstChapter.SelectedValue));
                //Session["RequestedFromPersonList"] = requestedFromPersonList;

                ////  lstSession.SelectedValue = gets(objList.SessionId);
                //  if (!gets(objList.OwnerId).Equals("0"))
                //  {
                //      lstRequestFrom.SelectedValue = gets(objList.OwnerId);
                //  }


                if (!gets(objList.RelatedOrgId).Equals("0"))
                {
                    lstCMGSRelatedOrg.SelectedValue = gets(objList.RelatedOrgId);
                }

                chkIsGrouped.Checked = getBool(objList.NoRelated);


                if (!gets(objList.Complaints_File).Equals(""))
                {
                    lnkQScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnQScannerfilepath.Value = gets(objList.Complaints_File);
                }

                txtCompaintSubject.Text = gets(objList.Complaints_Subject);
                if (!gets(objList.Complaints_File).Equals(""))
                {
                    anchorAttachment.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(objList.Code) + "/" + "&vfileList=[" + gets(objList.Complaints_File) + ";]";
                    anchorAttachment.Visible = true;
                }

                FillArcData(1, objList.Code);
                FillArcData(2, objList.Code);
                //Fill Attachments



            }

            tblAdd.Visible = true;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private int SaveComplaintsMaster(bool fromScan = false)
        {
            string script = "";
            //Upload LocalFile

            Complaints_Data objComplaint = new Complaints_Data();

            try
            {
                if (ViewState["itemID"].Equals("0"))
                {//Save

                    if (objRepository.checkComplainExistance((txtComplaintSerial.Text), 0, (int)Complaints_TypesEnum.Complaints))
                    {

                        script = FormatpopupErrorMSG(" [مسلسل الشكوي  مسجل من قبل  ]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return 0;
                    }


                    objComplaint.Complaints_Type = (int)Complaints_TypesEnum.Complaints;//1 Suggestiuon 2 Complains
                    objComplaint.CreationDate = DateTime.Now;
                    objComplaint.LastModificationDate = DateTime.Now;

                    objComplaint.Complaints_Serial = txtComplaintSerial.Text;
                    objComplaint.Complaints_Subject = txtCompaintSubject.Text;
                    objComplaint.Notes = txtCompaintNote.Text;

                    objComplaint.Complaints_Date = NullDateifEmpty(txtCompaintDate.Text);

                    //objComplaint.ChapterId = ZeroIntergerIFNull(lstChapter.SelectedValue);
                    //objComplaint.SessionId = ZeroIntergerIFNull(lstSession.SelectedValue);
                    //objComplaint.OwnerId = ZeroIntergerIFNull(lstRequestFrom.SelectedValue);

                    objComplaint.Complaints_OwnerName = (txtOwnerName.Text);

                    objComplaint.RelatedOrgId = ZeroIntergerIFNull(lstCMGSRelatedOrg.SelectedValue);

                    objComplaint.NoRelated = getBool(chkIsGrouped.Checked);


                    objComplaint.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    objRepository.AddCompaints(objComplaint);
                    hdnMasterID.Value = gets(objComplaint.Code);
                    ViewState["itemID"] = gets(objComplaint.Code);

                }
                else
                { //Update


                    if (objRepository.checkComplainExistance((txtComplaintSerial.Text), ZeroIntergerIFNull(ViewState["itemID"].ToString()), (int)Complaints_TypesEnum.Complaints))
                    {

                        script = FormatpopupErrorMSG(" [مسلسل الشكوي  مسجل من قبل  ]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return 0;
                    }

                    hdnMasterID.Value = ViewState["itemID"].ToString();
                    objComplaint = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                    objComplaint.LastModificationDate = DateTime.Now;

                    objComplaint.Complaints_Serial = txtComplaintSerial.Text;
                    objComplaint.Complaints_Subject = txtCompaintSubject.Text;
                    objComplaint.Notes = txtCompaintNote.Text;

                    objComplaint.Complaints_Date = NullDateifEmpty(txtCompaintDate.Text);


                    objComplaint.RelatedOrgId = ZeroIntergerIFNull(lstCMGSRelatedOrg.SelectedValue);
                    objComplaint.Complaints_OwnerName = (txtOwnerName.Text);
                    objComplaint.NoRelated = getBool(chkIsGrouped.Checked);

                    objComplaint.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                    objRepository.UpdateCompaints(objComplaint);
                }


                string _img = UploadFileoServer(txtQImage, ScannerRepository + _TargetUploadPath + gets(objComplaint.Code) + "/");
                if (_img != "")
                {
                    var objComplainForEdit = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                    objComplainForEdit.Complaints_File = _img;
                    objRepository.UpdateCompaints(objComplainForEdit);
                }

                ViewState["itemID"] = objComplaint.Code;

                anchorAttachment.Visible = true;
                anchorAttachment.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(objComplaint.Code) + "/" + "&vfileList=[" + gets(objComplaint.Complaints_File) + ";]";



                //  ClearForm();
                //Save Person
                try
                {

                    ////Get related Orgs

                    //if (hdnRelatedOrg.Value != "")
                    //{
                    //    //Delete Quextion Related Orgs
                    //    Session["Complaints_Defendant"] = null;
                    //    Session["Complaints_prosecutor"] = null;

                    //    //Set
                    //    string[] SelectedOrgs = hdnRelatedOrg.Value.Split(',');

                    //    List<Parliament_Requestedby> PersonsList = new List<Parliament_Requestedby>();
                    //    for (int i = 0; i < SelectedOrgs.Length; i++)
                    //    {


                    //        Parliament_Requestedby _personobj = new Parliament_Requestedby();
                    //        _personobj.Code = -1;
                    //        _personobj.PartyType = 2;
                    //        _personobj.PersonID = ZeroIntergerIFNull(SelectedOrgs[i]);
                    //        _personobj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                    //        _personobj.AddDate = DateTime.Now;
                    //        _personobj.LastUpdate = DateTime.Now;
                    //        PersonsList.Add(_personobj);
                    //    }

                    //    Session["Complaints_Defendant"] = PersonsList;
                    //    SavePersons(objComplaint.code);
                    //    Session["Complaints_Defendant"] = null;
                    //    Session["Complaints_prosecutor"] = null;
                    //}
                    //else {

                    //    Session["Complaints_Defendant"] = null;
                    //    Session["Complaints_prosecutor"] = null;
                    //    SavePersons(objComplaint.code);
                    //}


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
                Response.Redirect("complaintsData.aspx?ss=1&CompaintID=" + gets(objComplaint.Code));
                return 0;
            }
            return objComplaint.Code;
        }
        private void fillLookups()
        {


            FillDllwithoptional_ALL(objLookup.FillParliament_CMGSRelatedOrgs(), ref lstCMGSRelatedOrg, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillParliament_CMGSRelatedOrgs(), ref lstFilterCMGSRelatedOrgs, "NameAr", "Code", "الكل");




            FillDllwithoptional_ALL(objLookup.FillParliament_RelatedOrgs(), ref lstIncomingOrg, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillParliament_RelatedOrgs(), ref lstoutgoiningOrg, "NameAr", "Code", "اختر");

            try
            {
                lstoutgoiningOrg.SelectedValue = "30";// Requested by Nada 06102019
            }
            catch (Exception)
            {


            }
            // var requestedFromPersonList = objLookup.FillOMaPerson(ZeroIntergerIFNull( lstChapter.SelectedValue));
            // Session["RequestedFromPersonList"] = requestedFromPersonList;


            var relatedOrgs = objLookup.FillParliament_RelatedOrgs();
            Session["relatedOrgs"] = relatedOrgs;

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



        #region "Coming Letters"
        #region "Coming HElpers"
        private void FillinComingForm()
        {

            var objList = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["ComplaintArcID"].ToString()));
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


                if (ViewState["ComplaintArcID"].Equals("0"))
                {//Save

                    obj.TransactionDate = DateTime.Now;
                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.ComplaintsModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtDoc_Serial.Text);
                    obj.Doc_Subject = gets(txtDoc_Subject.Text);
                    obj.RefDocID = RefDocID;

                    obj.Doc_From = lstIncomingOrg.SelectedItem.Text;
                    obj.Doc_FromId = ZeroIntergerIFNull(lstIncomingOrg.SelectedValue);

                    obj.Doc_To = "CMGS";
                    obj.Doc_Notes = txtComingNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtComingDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtComingReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());



                    objRepository.AddArcData(obj);
                    ViewState["ComplaintArcID"] = gets(obj.Code);


                }
                else
                { //Update


                    obj = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["ComplaintArcID"].ToString()));


                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.ComplaintsModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtDoc_Serial.Text);
                    obj.Doc_Subject = gets(txtDoc_Subject.Text);
                    obj.RefDocID = RefDocID;



                    obj.Doc_From = lstIncomingOrg.SelectedItem.Text;
                    obj.Doc_FromId = ZeroIntergerIFNull(lstIncomingOrg.SelectedValue);


                    obj.Doc_To = "CMGS";
                    obj.Doc_Notes = txtComingNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtComingDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtComingReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    // Complaints_DataAnswers objAnswer = new Complaints_DataAnswers();


                    objRepository.UpdateArcData(obj);

                }
                string folderpath = obj.RefDocID.ToString() + "/" + (obj.Doc_Type == 1 ? "incoming/" : "outgoing/") + obj.Code.ToString() + "/";
                string _img = UploadFileoServer(txtIncomingImge, ScannerRepository + _TargetUploadPath + folderpath);
                if (_img != "")
                {
                    var objAcrForUpdate = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["ComplaintArcID"].ToString()));
                    objAcrForUpdate.Filepath = _img;
                    objRepository.UpdateArcData(objAcrForUpdate);
                }

                ViewState["ComplaintArcID"] = obj.Code;

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
        protected void grdincoming_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["ComplaintArcID"] = e.Item.Cells[0].Text;
            divAddIncoming.Visible = false;
            FillinComingForm();

        }

        #endregion

        protected void Lnkincoming_Click(object sender, EventArgs e)
        {
            ViewState["outgoingCode"] = "0";
            ViewState["ComplaintArcID"] = "0";

            divAddIncoming.Visible = true;
            divshowincoming.Visible = false;
        }

        protected void lnkSaveIncoming_Click(object sender, EventArgs e)
        {
            if (ViewState["ComplaintId"].ToString() != "0" && ViewState["ComplaintId"].ToString() != "")
            {
                SaveComingDocInformation(1, ZeroIntergerIFNull(ViewState["ComplaintId"].ToString()));
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
            FillArcData(1, ZeroIntergerIFNull(ViewState["ComplaintId"].ToString()));
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
                FillArcData(1, ZeroIntergerIFNull(ViewState["ComplaintId"].ToString()));

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

                if (ViewState["ComplaintArcID"].Equals("0"))
                {//Save

                    obj.TransactionDate = DateTime.Now;
                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.ComplaintsModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtOutDocNo.Text);
                    obj.Doc_Subject = gets(txtOutSubject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = "CMGS";



                    obj.Doc_To = lstoutgoiningOrg.SelectedItem.Text;
                    obj.Doc_ToId = ZeroIntergerIFNull(lstoutgoiningOrg.SelectedValue);

                    obj.Doc_Notes = txtoutNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtoutDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtOutReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    objRepository.AddArcData(obj);

                    ViewState["ComplaintArcID"] = gets(obj.Code);


                }
                else
                { //Update


                    obj = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["ComplaintArcID"].ToString()));


                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.ComplaintsModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtOutDocNo.Text);
                    obj.Doc_Subject = gets(txtOutSubject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = "CMGS";

                    obj.Doc_To = lstoutgoiningOrg.SelectedItem.Text;
                    obj.Doc_ToId = ZeroIntergerIFNull(lstoutgoiningOrg.SelectedValue);

                    obj.Doc_Notes = txtoutNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtoutDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtOutReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());



                    objRepository.UpdateArcData(obj);

                }
                string folderpath = obj.RefDocID.ToString() + "/" + (obj.Doc_Type == 1 ? "incoming/" : "outgoing/") + obj.Code.ToString() + "/";
                string _img = UploadFileoServer(txtoutgoiningImage, ScannerRepository + _TargetUploadPath + folderpath);
                if (_img != "")
                {
                    var objAcrForUpdate = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["ComplaintArcID"].ToString()));
                    objAcrForUpdate.Filepath = _img;
                    objRepository.UpdateArcData(objAcrForUpdate);
                }
                ViewState["ComplaintArcID"] = obj.Code;

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
            ViewState["ComplaintArcID"] = "0";



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
            ViewState["ComplaintArcID"] = "0";

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
                FillArcData(2, ZeroIntergerIFNull(ViewState["ComplaintId"].ToString()));

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
            if (ViewState["ComplaintId"].ToString() != "0" && ViewState["ComplaintId"].ToString() != "")
            {
                SaveOutDocInformation(2, ZeroIntergerIFNull(ViewState["ComplaintId"].ToString()));
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
            FillArcData(2, ZeroIntergerIFNull(ViewState["ComplaintId"].ToString()));
        }

        private void FillOutForm()
        {

            var objList = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["ComplaintArcID"].ToString()));
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
            ViewState["ComplaintArcID"] = e.Item.Cells[0].Text;
            divOutgiongAdd.Visible = false;
            FillOutForm();
        }
        #endregion

        #region "Scanning"
        //protected void lnkScan_Click(object sender, EventArgs e)
        //{

        //    string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
        //    string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
        //    //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";

        //    int _TargetID = SaveAttachmentInformation();

        //    if (_TargetID != 0)
        //    {

        //        //ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + ViewState["DocID"].ToString() + "' />";
        //        //ScannerPostFrom += "<input type='hidden' id='CompaintID' name='CompaintID' value='" + ViewState["CompaintID"].ToString() + "' />";
        //        //ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";

        //        //target='_blank'
        //        string ScannerPostFrom = "<form id='ScannerCalllerForm'   action='" + _TargetUrl + "' method='POST'>";
        //        ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
        //        ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
        //        ScannerPostFrom += "<input type='hidden' id='action' name='action' value='1' />";
        //        ScannerPostFrom += "<input type='hidden' id='AttachID' name='AttachID' value='" + _TargetID + "' />";
        //        //ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";

        //        //ScannerPostFrom += "<input type='hidden' id='CompaintID' name='CompaintID' value='" + _TargetID + "' />";
        //        //ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
        //        //ScannerPostFrom += "<input type='hidden' id='id' name='id' value='" + ZeroIntergerIFNull(ViewState["itemID"].ToString()).ToString() + "' />";


        //        ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
        //        ScannerPostFrom += "<input type='hidden' id='CompaintID' name='CompaintID' value='" + ViewState["ComplaintId"].ToString() + "' />";
        //        ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
        //        ScannerPostFrom += "<input type='hidden' id='CompaintID' name='CompaintID' value='" + ViewState["itemID"].ToString() + "' />";


        //        ScannerPostFrom += "</form>";
        //        ScannerPostFrom += "<script>";
        //        ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
        //        ScannerPostFrom += "  CalllerForm.submit();";
        //        ScannerPostFrom += "</script>";

        //        ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;

        //        //_TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&AttachID=" + _TargetID  + "&TargetID=" + _TargetID;
        //        //Response.Redirect(_TargetUrl);
        //    }

        //}

        private void UpdateScannedFile()
        {
            Complaints_Data objComplaint = new Complaints_Data();

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
                                //  FillComplaintsMasterInformation();
                                hdnactivetab.Value = "1";

                                string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                string[] FileList = _ScannerFileLlisy.Split(';');

                                if (Request.Form["TargetID"] != null)
                                {
                                    for (int i = 0; i < FileList.Length; i++)
                                    {//Update Current Record With
                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objComplaint = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objComplaint.Complaints_File = null;
                                            objRepository.UpdateCompaints(objComplaint);
                                        }
                                        else
                                        {
                                            objComplaint = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objComplaint.Complaints_File = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateCompaints(objComplaint);
                                        }



                                    }
                                    Response.Redirect("complaintsData.aspx?activetab=1&CompaintID=" + Request.Form["TargetID"]);
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
                            {//  Related Details


                                //ViewState["AnswerCode"] = Request.Form["TargetID"];
                                //ViewState["itemID"] = Request.Form["FileID"];
                                ////  FillComplaintsMasterInformation();
                                //hdnactivetab.Value = "2";

                                //string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                //_ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                //string[] FileList = _ScannerFileLlisy.Split(';');

                                //if (Request.Form["TargetID"] != null)
                                //{
                                //    for (int i = 0; i < FileList.Length; i++)
                                //    {//Update Current Record With
                                //        //if (File.Exists(Server.MapPath(ScannerRepository + _TargetUploadPath + FileList[i].Split(',')[0].ToString())))
                                //        //{
                                //        //    try
                                //        //    {
                                //        //        File.Delete(Server.MapPath(ScannerRepository + _TargetUploadPath + FileList[i].Split(',')[0].ToString()));
                                //        //    }
                                //        //    catch (Exception)
                                //        //    {

                                //        //    }

                                //        //}
                                //        objAnswer = objRepository.GetAnswersDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                //        objAnswer.Answerattachments = FileList[i].Split(',')[0].ToString();
                                //        objRepository.UpdateAnswers(objAnswer);
                                //    }
                                //    Response.Redirect("complaintsData.aspx?activetab=2&CompaintID=" + Request.Form["FileID"]);
                                //    return;
                                //}


                                //else
                                //{
                                //    string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
                                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                //}

                                break;
                            }
                        case "3":
                            {//Incoming


                                ViewState["ComplaintArcID"] = Request.Form["TargetID"];
                                ViewState["itemID"] = Request.Form["FileID"];
                                //  FillComplaintsMasterInformation();
                                hdnactivetab.Value = "2";

                                string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                string[] FileList = _ScannerFileLlisy.Split(';');

                                if (Request.Form["TargetID"] != null)
                                {
                                    for (int i = 0; i < FileList.Length; i++)
                                    {//Update Current Record With


                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["ComplaintArcID"].ToString()));
                                            objArc.Filepath = null;
                                            objRepository.UpdateArcData(objArc);

                                        }
                                        else
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["ComplaintArcID"].ToString()));
                                            objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateArcData(objArc);

                                        }


                                    }
                                    Response.Redirect("complaintsData.aspx?activetab=3&CompaintID=" + Request.Form["FileID"]);
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

                                ViewState["ComplaintArcID"] = Request.Form["TargetID"];
                                ViewState["itemID"] = Request.Form["FileID"];
                                //  FillComplaintsMasterInformation();
                                hdnactivetab.Value = "3";

                                string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                string[] FileList = _ScannerFileLlisy.Split(';');

                                if (Request.Form["TargetID"] != null)
                                {
                                    for (int i = 0; i < FileList.Length; i++)
                                    {//Update Current Record With
                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["ComplaintArcID"].ToString()));
                                            objArc.Filepath = null;
                                            objRepository.UpdateArcData(objArc);
                                        }
                                        else
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["ComplaintArcID"].ToString()));
                                            objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateArcData(objArc);
                                        }


                                    }
                                    Response.Redirect("complaintsData.aspx?activetab=4&CompaintID=" + Request.Form["FileID"]);
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
            int _TargetID = SaveComplaintsMaster(true);



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
            if (ViewState["ComplaintId"].ToString() != "0" && ViewState["ComplaintId"].ToString() != "")
            {
                _TargetID = SaveComingDocInformation(1, ZeroIntergerIFNull(ViewState["ComplaintId"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Question Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["ComplaintId"].ToString() + "/incoming/" + _TargetID.ToString() + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnIncomingScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnIncomingScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["ComplaintId"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='3' />";
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
            if (ViewState["ComplaintId"].ToString() != "0" && ViewState["ComplaintId"].ToString() != "")
            {
                _TargetID = SaveOutDocInformation(2, ZeroIntergerIFNull(ViewState["ComplaintId"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Question Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["ComplaintId"].ToString() + "/outgoing/" + _TargetID.ToString() + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnOutgoingScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnOutgoingScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["ComplaintId"].ToString() + "' />";
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


        protected void grdCompaintsList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                objRepository.DeleteCompaints((Complaints_Data)objRepository.GetDetails(ZeroIntergerIFNull(e.Item.Cells[0].Text)));
            }
            FillComplaints();
        }

        protected void grdCompaintsList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
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




            }
            else if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string rowID = e.Item.ClientID;
                string Filecode = e.Item.Cells[0].Text;

                /***********************************************/

                DataGrid grdIncoming = ((DataGrid)(e.Item.Cells[1].FindControl("grdIncoming")));
                DataGrid grdOutgoing = ((DataGrid)(e.Item.Cells[1].FindControl("grdOutgoing")));


                var _arcIncoming = objRepository.FillArcDocs(1, ZeroIntergerIFNull(Filecode));
                var _arcOutgoing = objRepository.FillArcDocs(2, ZeroIntergerIFNull(Filecode));

                if (_arcIncoming != null)
                {
                    grdIncoming.DataSource = _arcIncoming;
                    grdIncoming.DataBind();
                }

                if (grdOutgoing != null)
                {
                    grdOutgoing.DataSource = _arcOutgoing;
                    grdOutgoing.DataBind();
                }

                /**************************************************/


                for (int i = 2; i <= (e.Item.Cells.Count - 1); i++)
                {
                    e.Item.Cells[i].Visible = false;
                }

                //e.Item.Cells[0].Controls[0].Visible = false;
                e.Item.Cells[1].Attributes.Add("colspan", ((e.Item.Cells.Count - 2)).ToString());
                e.Item.Attributes.Add("style", "display:none");
            }

            if (!(e.Item.ItemType == ListItemType.AlternatingItem))
            {
                e.Item.Cells[1].Visible = false;


            }

            

            
        }
    }
}