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

namespace UI.Web.Modules.LegalMemos.Forms
{
    public partial class LegalMemoData : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LegalMemoRepository objRepository = IoC.Resolve<LegalMemoRepository>();
        public string _PageTitle = "  كتب ومذكرات الرأي القانوني  ";

        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LegalMemos/";

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

            if (Request.QueryString["isAdmin"] != null)
            {
                divAutoNum.Visible = true;
            }


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
                ViewState["LawDocitemID"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["LawDocArcID"] = "0";
                ViewState["AttachitemID"] = "0";
                ViewState["ProcedureCode"] = "0";


                if (Request.QueryString["LawDocID"] == null)
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


                    ViewState["itemID"] = Request.QueryString["LawDocID"].ToString();
                    FillMemoMasterInformation();


                }

                SetPageTitle();

                ViewState["OutboundItemID"] = "0";

                // FillInboundItems();
                UpdateScannedFile();

            }

        }

        protected void grdLawDocsList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {


            if ((e.Item.ItemType == ListItemType.Item))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
            }

            if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
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




            }
            else if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string rowID = e.Item.ClientID;
                int DocRefID = ZeroIntergerIFNull(e.Item.Cells[3].Text);

                //SqlDataReader dr = SellMaster.ins.getInvoiceItemsReader(code);

                DataGrid grdIncoming = ((DataGrid)(e.Item.Cells[1].FindControl("grdIncoming")));
                DataGrid grdOutgoing = ((DataGrid)(e.Item.Cells[1].FindControl("grdOutgoing")));


                var _arcIncoming = objRepository.FillArcDocs(1, DocRefID);
                var _arcOutgoing = objRepository.FillArcDocs(2, DocRefID);

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


        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            { Response.Redirect("OutboundOperrations.aspx?id=" + Request.QueryString["id"].ToString()); }
            else
            { Response.Redirect("OutboundOperrations.aspx"); }

        }

        protected void btnNew_Click1(object sender, EventArgs e)
        {
            tblAdd.Visible = true;
            tblshow.Visible = false;
            tblSearch.Visible = false;
            ViewState["LawDocitemID"] = "0";
            ViewState["ProceduresitemID"] = "0";

            chkIsAudit.Checked = false;
            chkIsAudit.Visible = false;
            DivAudit.Visible = false;
            Auidtlbl.Visible = false;
            // txtautoNum.Text = GenerateAutoSerial();
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
            FillMemos();
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
            ViewState["LawDocitemID"] = "0";
            ViewState["ProceduresitemID"] = "0";
            Session["PersonsList"] = null;
            // Response.Redirect("/Modules/laws/Forms/LegalMemoData.aspx");
            FillMemos();

        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillMemos();
        }
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            SaveLawDocMaster();
        }
        protected void lnkAddNewLawDoc_Click(object sender, EventArgs e)
        {
            ClearCaseForm();
        }
        protected void lstOrgCategory_SelectedIndexChanged(object sender, EventArgs e)

        {

            if (Request.QueryString["editflag"] != null) // edit mode
            {
                FillDllwithoptional_ALL(objLookup.fill_LegalMemo_OrgByCat(ZeroIntergerIFNull(lstOrgCategory.SelectedValue)), ref lstOrg, "NameAr", "Code", "اختر");


            }
            else // add mode
            {
                List<LegalMemo_Org> list = objLookup.fill_LegalMemo_OrgByCat(ZeroIntergerIFNull(lstOrgCategory.SelectedValue)).Where(c => !c.isDeleted.HasValue || c.isDeleted.Value == false).ToList();

                FillDllwithoptional_ALL(list, ref lstOrg, "NameAr", "Code", "اختر");

            }

            

        }

        #endregion

        #region "Fill Information"

        public string GetLastFollow(string QCode)
        {
            string _out = "";

            var objLastFollow = objRepository.FillLastFollow(ZeroIntergerIFNull(QCode));
            if (objLastFollow != null)
            {
                double Percentage = 0;
                string ExecutionColorHex = "#ccc";
                if (objLastFollow.NextFollowReminderDate != null && NullDateifEmptyToText(objLastFollow.NextFollowReminderDate) != "")
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


                    _out += "<div class='circle-bar position' style='margin-top:5px;' data-percent='" + Percentage + "' data-color='#ccc," + ExecutionColorHex + "'></div>";
                }
                else
                {

                }



            }


            return _out;

        }


        public string GetDocCats(int StatusID, string StatusName)
        {
            string _out = "";
            if (StatusID == 141) // مراسيم استرداد  |   
            {
                _out = "<span class='label bg-warning-400' style='white-space: normal;'>" + StatusName + " </span>";
            }
            else if (StatusID == 145)
            {
                _out = "<span class='label bg-blue-400' style='white-space: normal;color:#fff'>مرسوم رد </span>";

            }

            return _out;
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

        private void FillMemos()
        {
            //Get Selectd Persons

            var objList = objRepository.GetList(txtFilterAutoNum.Text, ZeroIntergerIFNull(txtFilterSerialNum.Text), ZeroIntergerIFNull(txtFilterSerialYear.Text), NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterCategory.SelectedValue), ZeroIntergerIFNull(lstfilterOrg.SelectedValue),
                ZeroIntergerIFNull(lstfilterAssignedEmployee.SelectedValue), ZeroIntergerIFNull(lstfilterProcedure.SelectedValue),
               ZeroIntergerIFNull(lstFilterIsUnderStudy.SelectedValue),
               txtFilterSubject.Text, MapSearchKeys(), NullDateifEmpty(txtFilterFollowDateFrom.Text),
                NullDateifEmpty(txtFilterFollowDateTo.Text), ZeroIntergerIFNull(lstFilterStatus.SelectedValue), ZeroIntergerIFNull(lstFilterConsultant.SelectedValue));


            lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
            lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));





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



            grdLawDocsList.DataSource = duplicatedList;
            grdLawDocsList.DataBind();
            pager1.ItemCount = duplicatedList.Count;

        }

        private void ClearCaseForm()
        {
            ViewState["LawDocitemID"] = "0";
            txtDocSerialNum.Text = "";
            txtDocSerialYear.Text = "";
            txtSubject.Text = "";
            txtNotes.Text = "";

            lstOrg.SelectedValue = "0";
            lstCategory.SelectedValue = "0";
            lstAssignedEmployee.SelectedValue = "0";
            lstProcedure.SelectedValue = "0";

            txtDocDate.Text = "";



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
                        _out = "<span class='label bg-warning-400'>" + StatusName + "  </span>";


                        break;
                    }
                case 2:
                    {
                        _out = "<span class='label bg-success-400'>" + StatusName + " </span>";
                        break;
                    }
                case 3:
                    {
                        _out = "<span class='label bg-grey-400'> " + StatusName + "</span>";
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
            lnkAddNewOutGoing.Visible = userAccess.Add;
            Lnkincoming.Visible = userAccess.Add;
            DivAudit.Visible = userAccess.AuditControl;

            if (Request.QueryString["editflag"] != null)
            {
                string editflag = Request.QueryString["editflag"].ToString();

                btnSave.Visible = userAccess.Edit;
            }
            else
            {

                btnSave.Visible = userAccess.Edit || userAccess.Add;
                //chkIsAudit.Checked = false;
                //chkIsAudit.Visible = false;

            }

            grdLawDocsList.Columns[16].Visible = userAccess.Delete;
            lnkDeleteIncoming.Visible = userAccess.Delete;
            lnkDeleteOutgoing.Visible = userAccess.Delete;

        }

        protected void pager_Command(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdLawDocsList.PageCount))
            {
                currnetPageIndx = (grdLawDocsList.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdLawDocsList.CurrentPageIndex = (currnetPageIndx - 1);
            FillMemos();
        }


        #endregion

        #region "Helper Methods"


        private void FillMemoMasterInformation()
        {

            var objList = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {
                hdnMasterID.Value = gets(objList.Code);
                ViewState["LawDocitemID"] = gets(objList.Code);

                txtDocSerialNum.Text = gets(objList.FileNum);
                txtDocSerialYear.Text = gets(objList.DocYear);
                txtautoNum.Text = gets(objList.DocSerial);

                txtNotes.Text = gets(objList.DocNotes);

                txtSubject.Text = gets(objList.DocSubject);

                txtDocDate.Text = NullDateifEmptyToText(objList.DocDate).ToString();

                //chkIsAudit.Checked = getBool(objList.isAudited);

                //if (Request.QueryString["editflag"] != null)
                //{
                    chkIsAudit.Checked = getBool(objList.isAudited);
                //}
                //else
                //{
                //    chkIsAudit.Checked = false;
                //    chkIsAudit.Visible = false;
                //}


                try
                {
                    lstCategory.SelectedValue = gets(objList.DocCategoryId);
                    lstAssignedEmployee.SelectedValue = gets(objList.assignedEmp);
                    lstProcedure.SelectedValue = gets(objList.ProcedureId);
                    lstOrgCategory.SelectedValue = gets(objList.LegalMemo_Org.catId);

                    FillDllwithoptional_ALL(objLookup.fill_LegalMemo_OrgByCat(ZeroIntergerIFNull(lstOrgCategory.SelectedValue)), ref lstOrg, "NameAr", "Code", "اختر");



                    lstOrg.SelectedValue = gets(objList.OrgId);

                    lstStatus.SelectedValue = gets(objList.DocStatusId);

                    if (gets(objList.ConsultantId) != "" && gets(objList.ConsultantId) != "0")
                    {
                        lstConsultant.SelectedValue = gets(objList.ConsultantId);
                    }
                }
                catch (Exception)
                {


                }

                if (!gets(objList.DocFilepath).Equals(""))
                {

                    lnkDocScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";

                    anchorAttachment.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(objList.Code) + "/" + "&vfileList=[" + gets(objList.DocFilepath) + ";]"; // "LawsAttachments.aspx?FileID=" + gets(objList.code);
                                                                                                                                                                                         //anchorAttachment.Attributes.Add("click", "openScannerViewer('"+ _TargetUrl + "',3,"+ objList.Code + ",2,'"+ _TargetUploadPath + "','','["+ gets(objList.DocFilepath) + "]','"+ System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "')");
                                                                                                                                                                                         //  anchorAttachment.Attributes.Add("onclick", "openDocFile();");
                    hdnScannerfilepath.Value = gets(objList.DocFilepath);
                    anchorAttachment.Visible = true;
                }


                //Fill Arch
                FillArcData(1, objList.Code);
                FillArcData(2, objList.Code);
            }
            tblAdd.Visible = true;
            tblshow.Visible = false;
            tblSearch.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }

        private string GenerateAutoSerial()
        {
            return gets(objRepository.getMemoCountForCurrentYear(NullDateifEmpty(txtDocDate.Text).Year) + 1) + "/" + NullDateifEmpty(txtDocDate.Text).Year.ToString();
        }


        private int SaveLawDocMaster(bool fromScan = false)
        {
            string script = "";
            //Upload LocalFile

            LegalMemo objLawDoc = new LegalMemo();

            try
            {
                if (ViewState["itemID"].Equals("0"))
                {//Save


                    //if (objRepository.CheckDocExistance(ZeroIntergerIFNull(txtDocSerialNum.Text), ZeroIntergerIFNull(txtDocSerialYear.Text), ZeroIntergerIFNull(lstDocType.SelectedValue), 0))
                    //{

                    //    script = FormatpopupErrorMSG(" [ رقم الوثيقة   مسجل من قبل  ]", "1");
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    //    return 0;
                    //}


                    objLawDoc.CreationDate = DateTime.Now;
                    objLawDoc.LastModificationDate = DateTime.Now;


                    //objLawDoc.DocSerial = gets(txtDocSerialYear.Text) + "/" + gets(txtDocSerialNum.Text);
                    objLawDoc.DocSerial = GenerateAutoSerial();
                    objLawDoc.FileNum = ZeroIntergerIFNull(txtDocSerialNum.Text);
                    objLawDoc.DocYear = ZeroIntergerIFNull(txtDocSerialYear.Text);


                    objLawDoc.DocSubject = txtSubject.Text;
                    objLawDoc.DocDate = NullDateifEmpty(txtDocDate.Text);
                    objLawDoc.DocCategoryId = ZeroIntergerIFNull(lstCategory.SelectedValue);

                    objLawDoc.OrgId = ZeroIntergerIFNull(lstOrg.SelectedValue);
                    objLawDoc.assignedEmp = ZeroIntergerIFNull(lstAssignedEmployee.SelectedValue);
                    objLawDoc.ProcedureId = ZeroIntergerIFNull(lstProcedure.SelectedValue);

                    objLawDoc.DocStatusId = ZeroIntergerIFNull(lstStatus.SelectedValue);
                    //objLawDoc.UnderStudy = getBool(chkIsUnderStudy.Checked);

                    objLawDoc.DocNotes = gets(txtNotes.Text);
                    objLawDoc.isAudited = getBool(chkIsAudit.Checked);
                    if (getBool(chkIsAudit.Checked) && chkIsAudit.Visible == true)
                    {
                        objLawDoc.LastAuditDate = DateTime.Now;
                        objLawDoc.LastAuditBy = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
                    }

                    objLawDoc.ConsultantId = ZeroIntergerIFNull(lstConsultant.SelectedValue);
                    objLawDoc.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                    objRepository.AddLegalMemo(objLawDoc);
                    hdnMasterID.Value = gets(objLawDoc.Code);
                    ViewState["itemID"] = gets(objLawDoc.Code);

                }
                else
                { //Update


                    //if (objRepository.CheckDocExistance(ZeroIntergerIFNull(txtDocSerialNum.Text), ZeroIntergerIFNull(txtDocSerialYear.Text), ZeroIntergerIFNull(lstDocType.SelectedValue), ZeroIntergerIFNull(ViewState["itemID"].ToString())))
                    //{
                    //    script = FormatpopupErrorMSG(" [ رقم الوثيقة   مسجل من قبل  ]", "1");
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    //    return 0;
                    //}
                    Boolean dateChanged = false;
                    int targetYear = 0;


                    hdnMasterID.Value = ViewState["itemID"].ToString();
                    objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                    objLawDoc.LastModificationDate = DateTime.Now;
                    objLawDoc.DocSerial = txtautoNum.Text;
                    objLawDoc.FileNum = ZeroIntergerIFNull(txtDocSerialNum.Text);
                    objLawDoc.DocYear = ZeroIntergerIFNull(txtDocSerialYear.Text);
                    //Reset Count if Date Changed
                    if (objLawDoc.DocDate != NullDateifEmpty(txtDocDate.Text))
                    {
                        dateChanged = true;
                        targetYear = objLawDoc.DocDate.Value.Year;
                        objLawDoc.DocSerial = GenerateAutoSerial();

                    }

                    objLawDoc.DocSubject = txtSubject.Text;
                    objLawDoc.DocDate = NullDateifEmpty(txtDocDate.Text);
                    objLawDoc.DocCategoryId = ZeroIntergerIFNull(lstCategory.SelectedValue);
                    objLawDoc.OrgId = ZeroIntergerIFNull(lstOrg.SelectedValue);
                    objLawDoc.assignedEmp = ZeroIntergerIFNull(lstAssignedEmployee.SelectedValue);
                    objLawDoc.ProcedureId = ZeroIntergerIFNull(lstProcedure.SelectedValue);

                    objLawDoc.DocStatusId = ZeroIntergerIFNull(lstStatus.SelectedValue);

                    objLawDoc.DocNotes = gets(txtNotes.Text);
                    objLawDoc.isAudited = getBool(chkIsAudit.Checked);
                    if (getBool(chkIsAudit.Checked) && chkIsAudit.Visible == true)
                    {
                        objLawDoc.LastAuditDate = DateTime.Now;
                        objLawDoc.LastAuditBy = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
                    }

                    objLawDoc.ConsultantId = ZeroIntergerIFNull(lstConsultant.SelectedValue);
                    objLawDoc.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                    objRepository.UpdateLegalMemo(objLawDoc);
                    if (dateChanged)
                    {
                        // Reset Year Serial
                        //resetYearSerial(targetYear);
                        objRepository.ResetDocSerials();

                    }
                }


                string _img = UploadFileoServer(txtDocImage, ScannerRepository + _TargetUploadPath + gets(objLawDoc.Code) + "/");

                if (_img != "")
                {
                    var objForEdit = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                    objForEdit.DocFilepath = _img;
                    objRepository.UpdateLegalMemo(objForEdit);
                }

                ViewState["itemID"] = objLawDoc.Code;

                //anchorAttachment.Visible = true;
                //anchorAttachment.HRef = "LawsAttachments.aspx?FileID=" + hdnMasterID.Value;

                //  ClearForm();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

            if (!fromScan)
            {
                Response.Redirect("LegalMemoData.aspx?ss=1&LawDocID=" + gets(objLawDoc.Code));
                return 0;
            }
            return objLawDoc.Code;
        }
        private void fillLookups()
        {




            FillDllwithoptional_ALL(objLookup.fillLLegalMemo_Category(), ref lstCategory, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.fillLLegalMemo_Category(), ref lstFilterCategory, "NameAr", "Code", "الكل");
            try
            {
                lstCategory.SelectedValue = "2";
            }
            catch (Exception)
            {

            }


            FillDllwithoptional_ALL(objLookup.fill_LegalMemo_Procedure(), ref lstProcedure, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.fill_LegalMemo_Procedure(), ref lstfilterProcedure, "NameAr", "Code", "");


            FillDllwithoptional_ALL(objLookup.fill_LegalMemo_OrgCat(), ref lstOrgCategory, "NameAr", "Code", "اختر");


       
            FillDllwithoptional_ALL(objLookup.fill_LegalMemo_OrgByCat(ZeroIntergerIFNull(lstOrgCategory.SelectedValue)), ref lstOrg, "NameAr", "Code", "اختر");
               

       

            FillDllwithoptional_ALL(objLookup.fill_LegalMemo_Org(), ref lstfilterOrg, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.fill_LegalMemo_AssignedPersons(), ref lstAssignedEmployee, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.fill_LegalMemo_AssignedPersons(), ref lstfilterAssignedEmployee, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.fillConsultant(), ref lstConsultant, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.fillConsultant(), ref lstFilterConsultant, "NameAr", "Code", "الكل");



            FillDllwithoptional_ALL(objLookup.FillParliament_RelatedOrgs(), ref lstIncomingOrg, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillParliament_RelatedOrgs(), ref lstoutgoiningOrg, "NameAr", "Code", "اختر");
            //FillDllwithoptional_ALL(objLookup.FillParliament_RelatedOrgs(), ref lstoutgoiningOrgFrom, "NameAr", "Code", "اختر");


            FillDllwithoptional_ALL(objLookup.FillLegalMemoStatus(), ref lstStatus, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillLegalMemoStatus(), ref lstFilterStatus, "NameAr", "Code", "الكل");


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

        #region "Scanning"


        private void UpdateScannedFile()
        {
            LegalMemo objLawDoc = new LegalMemo();
            arc_Data objArc = new arc_Data();
            if (Request.Form["fileList"] != null)
            {
                if (Request.Form["fileList"].ToString() != "")
                {

                    switch (Request.Form["ActiveTab"].ToString())
                    {
                        case "1":
                            {//LawDoc Information


                                ViewState["itemID"] = Request.Form["TargetID"];
                                //  FillMemoMasterInformation();
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
                                            objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objLawDoc.DocFilepath = null;
                                            objRepository.UpdateLegalMemo(objLawDoc);
                                        }
                                        else
                                        {
                                            objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objLawDoc.DocFilepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateLegalMemo(objLawDoc);
                                        }
                                    }

                                    Response.Redirect("LegalMemoData.aspx?activetab=1&LawDocID=" + Request.Form["TargetID"].ToString());
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


                                ViewState["DocumentArcID"] = Request.Form["TargetID"];
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

                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
                                            objArc.Filepath = null;
                                            objRepository.UpdateArcData(objArc);
                                        }
                                        else
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
                                            objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateArcData(objArc);
                                        }
                                    }
                                    Response.Redirect("LegalMemoData.aspx?activetab=3&LawDocID=" + Request.Form["FileID"]);
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

                                ViewState["DocumentArcID"] = Request.Form["TargetID"];
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

                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
                                            objArc.Filepath = null;
                                            objRepository.UpdateArcData(objArc);
                                        }
                                        else
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
                                            objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateArcData(objArc);
                                        }
                                    }
                                    Response.Redirect("LegalMemoData.aspx?activetab=4&LawDocID=" + Request.Form["FileID"]);
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
            int _TargetID = SaveLawDocMaster(true);



            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + gets(_TargetID) + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnScannerfilepath.Value + "]' />";
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
            if (ViewState["itemID"].ToString() != "0" && ViewState["itemID"].ToString() != "")
            {
                _TargetID = SaveComingDocInformation(1, ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Question Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["itemID"].ToString() + "/incoming/" + gets(_TargetID) + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnIncomingScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnIncomingScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["itemID"].ToString() + "' />";
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
            if (ViewState["itemID"].ToString() != "0" && ViewState["itemID"].ToString() != "")
            {
                _TargetID = SaveOutDocInformation(2, ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Question Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {



                //string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                //ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["itemID"].ToString() + "/outgoing/" + gets(_TargetID) + "/" + "' />";
                //ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                //ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                //ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnIncomingScannerfilepath.Value == "" ? "1" : "3") + "' />";
                //ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                //ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                //ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                //ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["itemID"].ToString() + "' />";
                //ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='4' />";
                //ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";
                //ScannerPostFrom += "</form>";
                //ScannerPostFrom += "<script>";
                //ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                //ScannerPostFrom += "  CalllerForm.submit();";
                //ScannerPostFrom += "</script>";

                //((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;


                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["itemID"].ToString() + "/outgoing/" + gets(_TargetID) + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnoutgoingScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnoutgoingScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["itemID"].ToString() + "' />";
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

        protected void lnkQScan_Click2(object sender, EventArgs e)
        {
            //Show Loadin div

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
                                                                           //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }
            int _TargetID = SaveLawDocMaster(true);



            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + gets(_TargetID) + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='1' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileType' name='FileType' value='2' />";
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
        protected void grdLawDocsList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                //Delete Related Data


                //for (int i = 0; i <= grdProcedureList.Items.Count - 1; i++)
                //{

                //    if ((grdProcedureList.Items[i].FindControl("chkItem") != null))
                //    {
                //        CheckBox check = (CheckBox)grdProcedureList.Items[i].FindControl("chkItem");

                //        if (check.Checked)
                //        {
                //            objRepository.DeleteProcedures((Law_DocProcedures)objRepository.GetProceduresDetails(ZeroIntergerIFNull(grdProcedureList.Items[i].Cells[0].Text)));
                //        }
                //    }
                //


                // Delete Related Docs
                var RelatedArcDocs = objRepository.FillArcDocs(ZeroIntergerIFNull(e.Item.Cells[3].Text));
                if (RelatedArcDocs != null)
                {
                    foreach (var item in RelatedArcDocs)
                    {
                        objRepository.DeleteArcData(item);

                    }

                }


                objRepository.DeleteLegalMemo((LegalMemo)objRepository.GetDetails(ZeroIntergerIFNull(e.Item.Cells[3].Text)));
            }
            FillMemos();
        }

        #endregion

        #region "Coming Letters"
        #region "Coming HElpers"
        private void FillinComingForm()
        {

            var objList = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
            if ((objList != null))
            {
                txtDoc_Serial.Text = gets(objList.Doc_Serial);
                txtDoc_Subject.Text = gets(objList.Doc_Subject);
                txtComingNotes.Text = gets(objList.Doc_Notes);

                // txtFrom.Text= gets(objList.Doc_From);

                lstIncomingOrg.SelectedValue = gets(objList.Doc_FromId);
                txtComingDate.Text = NullDateifEmptyToText(objList.SentDate);
                txtComingReminderDate.Text = NullDateifEmptyToText(objList.NextFollowReminderDate);
                if (!gets(objList.Filepath).Equals(""))
                {
                    hdnIncomingScannerfilepath.Value = gets(objList.Filepath);
                    btnIncomingScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                }



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


                if (ViewState["DocumentArcID"].Equals("0"))
                {//Save

                    obj.TransactionDate = DateTime.Now;
                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.LegalMemo;
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
                    ViewState["DocumentArcID"] = gets(obj.Code);


                }
                else
                { //Update


                    obj = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));


                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.LegalMemo;
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


                    // Parliament_DocumentsAnswers objAnswer = new Parliament_DocumentsAnswers();


                    objRepository.UpdateArcData(obj);

                }
                ViewState["DocumentArcID"] = obj.Code;
                string folderpath = obj.RefDocID.ToString() + "/" + (obj.Doc_Type == 1 ? "incoming/" : "outgoing/") + obj.Code.ToString() + "/";
                string _img = UploadFileoServer(txtIncomingImge, ScannerRepository + _TargetUploadPath + folderpath);
                if (_img != "")
                {
                    var objAcrForUpdate = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
                    objAcrForUpdate.Filepath = _img;
                    objRepository.UpdateArcData(objAcrForUpdate);
                }


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
            ViewState["DocumentArcID"] = e.Item.Cells[0].Text;
            divAddIncoming.Visible = false;
            FillinComingForm();

        }

        #endregion

        protected void Lnkincoming_Click(object sender, EventArgs e)
        {
            ViewState["outgoingCode"] = "0";
            ViewState["DocumentArcID"] = "0";

            divAddIncoming.Visible = true;
            divshowincoming.Visible = false;
        }

        protected void lnkSaveIncoming_Click(object sender, EventArgs e)
        {
            if (ViewState["itemID"].ToString() != "0" && ViewState["itemID"].ToString() != "")
            {
                SaveComingDocInformation(1, ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Document Information First", "2");
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
            FillArcData(1, ZeroIntergerIFNull(ViewState["itemID"].ToString()));
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
                FillArcData(1, ZeroIntergerIFNull(ViewState["itemID"].ToString()));

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

            //string _img = UploadFileoServer(txtoutgoiningImage, ScannerRepository + _TargetUploadPath);
            arc_Data obj = new arc_Data();
            try
            {

                if (ViewState["DocumentArcID"].Equals("0"))
                {//Save

                    obj.TransactionDate = DateTime.Now;
                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.LegalMemo;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtOutDocNo.Text);
                    obj.Doc_Subject = gets(txtOutSubject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = lstoutgoiningOrgFrom.SelectedItem.Text;
                    //obj.Doc_FromId = ZeroIntergerIFNull(lstoutgoiningOrgFrom.SelectedValue);



                    obj.Doc_To = lstoutgoiningOrg.SelectedItem.Text;
                    obj.Doc_ToId = ZeroIntergerIFNull(lstoutgoiningOrg.SelectedValue);

                    obj.Doc_Notes = txtoutNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtoutDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtOutReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    //if (_img != "")
                    //{
                    //    obj.Filepath = _img;
                    //}
                    objRepository.AddArcData(obj);

                    ViewState["DocumentArcID"] = gets(obj.Code);


                }
                else
                { //Update


                    obj = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));


                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.LegalMemo;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtOutDocNo.Text);
                    obj.Doc_Subject = gets(txtOutSubject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = lstoutgoiningOrgFrom.SelectedItem.Text;
                    //obj.Doc_FromId = ZeroIntergerIFNull(lstoutgoiningOrgFrom.SelectedValue);

                    obj.Doc_To = lstoutgoiningOrg.SelectedItem.Text;
                    obj.Doc_ToId = ZeroIntergerIFNull(lstoutgoiningOrg.SelectedValue);

                    obj.Doc_Notes = txtoutNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtoutDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtOutReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                    //if (_img != "")
                    //{
                    //    obj.Filepath = _img;
                    //}

                    objRepository.UpdateArcData(obj);

                }
                ViewState["DocumentArcID"] = obj.Code;
                string folderpath = obj.RefDocID.ToString() + "/" + (obj.Doc_Type == 1 ? "incoming/" : "outgoing/") + obj.Code.ToString() + "/";
                string _img = UploadFileoServer(txtoutgoiningImage, ScannerRepository + _TargetUploadPath + folderpath);
                if (_img != "")
                {
                    var objAcrForUpdate = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
                    objAcrForUpdate.Filepath = _img;
                    objRepository.UpdateArcData(objAcrForUpdate);
                }


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
            ViewState["DocumentArcID"] = "0";



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
            ViewState["DocumentArcID"] = "0";

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
                FillArcData(2, ZeroIntergerIFNull(ViewState["itemID"].ToString()));

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
            if (ViewState["itemID"].ToString() != "0" && ViewState["itemID"].ToString() != "")
            {
                SaveOutDocInformation(2, ZeroIntergerIFNull(ViewState["itemID"].ToString()));
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
            FillArcData(2, ZeroIntergerIFNull(ViewState["itemID"].ToString()));
        }

        private void FillOutForm()
        {

            var objList = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
            if ((objList != null))
            {
                txtOutDocNo.Text = gets(objList.Doc_Serial);
                txtOutSubject.Text = gets(objList.Doc_Subject);
                txtoutNotes.Text = gets(objList.Doc_Notes);

                // txtto.Text = gets(objList.Doc_To);
                try
                {
                    lstoutgoiningOrg.SelectedValue = gets(objList.Doc_ToId);
                    lstoutgoiningOrgFrom.SelectedValue = gets(objList.Doc_FromId);
                }
                catch (Exception)
                {


                }



                txtoutDate.Text = NullDateifEmptyToText(objList.SentDate);
                txtOutReminderDate.Text = NullDateifEmptyToText(objList.NextFollowReminderDate);

                if (!gets(objList.Filepath).Equals(""))
                {
                    hdnoutgoingScannerfilepath.Value = gets(objList.Filepath);
                    btnOutgoingScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                }



            }

            divOutgiongAdd.Visible = true;
            divoutgoingshow.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        protected void grdOutgoing_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["DocumentArcID"] = e.Item.Cells[0].Text;
            divOutgiongAdd.Visible = false;
            FillOutForm();
        }

        #endregion

        protected void grdOutgoing_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (Request.QueryString["targetCode"] != null)
                {
                    if (ZeroIntergerIFNull(Request.QueryString["targetCode"]) == ZeroIntergerIFNull(e.Item.Cells[0].Text))
                    {
                        //e.Item.Attributes.Add("backgroundColor", "this.style.backgroundColor=\'#f2d575\';");
                        e.Item.BackColor = System.Drawing.Color.Yellow;

                    }

                }

            }
        }

        protected void txtDocDate_TextChanged(object sender, EventArgs e)
        {
            txtDocSerialNum.Text = gets(objRepository.getMemoCountForCurrentYear(NullDateifEmpty(txtDocDate.Text).Year) + 1);
            txtDocSerialYear.Text = NullDateifEmpty(txtDocDate.Text).Year.ToString();
            if (NullDateifEmpty(txtDocDate.Text).Year >= 2023)
            {
                txtDocSerialNum.Enabled = false;
                txtDocSerialYear.Enabled = false;

            }
            else
            {
                txtDocSerialNum.Enabled = true;
                txtDocSerialYear.Enabled = true;
            }


        }
    }
}