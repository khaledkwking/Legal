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

namespace UI.Web.Modules.PM.Forms
{
    public partial class Frm_Pm_Letters : BaseFormAdmin
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
            applyUserPermission();
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
                ViewState["letterId"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["LetterArcID"] = "0";
                ViewState["AttachitemID"] = "0";
                ViewState["AnswerCode"] = "0";


                if (Request.QueryString["LetterID"] == null)
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


                    ViewState["itemID"] = Request.QueryString["LetterID"].ToString();
                    FillLetterMasterInformation();
                    //Fill Agreemnt Details


                }

                SetPageTitle();

                ViewState["OutboundItemID"] = "0";

                // FillInboundItems();
                UpdateScannedFile();

            }

        }

        protected void grdLetter_ItemDataBound(object sender, DataGridItemEventArgs e)
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
            FillLetters();
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
            ViewState["letterId"] = "0";
            ViewState["ProceduresitemID"] = "0";
             Session["PersonsList"] = null;
            Response.Redirect("/Modules/pm/Forms/Frm_Pm_Letters.aspx");

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillLetters();
        }
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            SaveLetterMaster();
        }


        protected void lnkAddNewLetter_Click(object sender, EventArgs e)
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
        private void FillLetters()
        {


            var objList = objRepository.GetList(ZeroIntergerIFNull(txtFilterNum.Text ), ZeroIntergerIFNull(txtFilterYear.Text), (txtFilterserial.Text),   NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterCategory.SelectedValue) , ZeroIntergerIFNull(lstFilterChapter.SelectedValue), MapSearchKeys());

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

           


            grdLetter.DataSource = objList;
         
            grdLetter.DataBind();


            pager1.ItemCount = objList.Count;

        }

        private void ClearCaseForm()
        {


            ViewState["letterId"] = "0";
            txtLetterSerial.Text = "";
            txtLetterNote.Text = "";

            lstCategory.SelectedValue = "0";



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

        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
           

            if (Request.QueryString["editflag"] != null)
            {
                string editflag = Request.QueryString["editflag"].ToString();

                btnSave.Visible = userAccess.Edit;
                               
            }
            else
            {

                btnSave.Visible = userAccess.Edit || userAccess.Add;
                
            }

            grdLetter.Columns[11].Visible = userAccess.Delete;

        }

        protected void pager_Command(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdLetter.PageCount))
            {
                currnetPageIndx = (grdLetter.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdLetter.CurrentPageIndex = (currnetPageIndx - 1);
            FillLetters();
        }


        #endregion

         #region "Helper Methods"


        private void FillLetterMasterInformation()
        {

            var objList = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {
                hdnMasterID.Value = gets(objList.code);
                ViewState["letterId"] = gets(objList.code);

                txtLetterSerial.Text = gets(objList.Arc_Serial);

                txtLetterNote.Text = gets(objList.Arc_Notes);
                txtLetterDate.Text = NullDateifEmptyToText(objList.Arc_Date).ToString();

                lstCategory.SelectedValue = gets(objList.CategoryID);

                lstChapter.SelectedValue = gets(objList.ChapterId);
                selectedChapter = gets(objList.CategoryID);


                txtSubject.Text = gets(objList.Arc_Subject);
                txtNum.Text = gets(objList.Arc_Num);
                txtYear.Text = gets(objList.Arc_Year);



                if (!gets(objList.Arc_Attachment).Equals(""))
                {
                    anchorAttachment.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath+gets(objList.code)+"/" + "&vfileList=[" + gets(objList.Arc_Attachment)+";]";
                    anchorAttachment.Visible = true;
                }


                if (!gets(objList.Arc_Attachment).Equals(""))
                {
                    lnkQScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnScannerfilepath.Value = gets(objList.Arc_Attachment);
                }


            }

            tblAdd.Visible = true;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private int SaveLetterMaster(bool fromScan=false)
        {
            string script = "";
            //Upload LocalFile

            Pm_Letters obj = new Pm_Letters();

            try
            {
                if (ViewState["itemID"].Equals("0"))
                {//Save

                    if (objRepository.CheckletterExistance((txtLetterSerial.Text), ZeroIntergerIFNull(txtYear.Text), ZeroIntergerIFNull(lstCategory.SelectedValue), 0))
                    {

                        script = FormatpopupErrorMSG(" [مسلسل الكتاب مسجل من قبل - لنفس التصنيف]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return 0;
                    }

                    obj.CreationDate = DateTime.Now;
                    obj.LastActionDate = DateTime.Now;

                    obj.Arc_Serial =  ( txtLetterSerial.Text);
                    obj.Arc_Notes = txtLetterNote.Text;


                    obj.Arc_Subject = txtSubject.Text;
                    obj.Arc_Num =ZeroIntergerIFNull( txtNum.Text);
                    obj.Arc_Year = ZeroIntergerIFNull(txtYear.Text);


                    obj.Arc_Date = NullDateifEmpty(txtLetterDate.Text);

                    obj.CategoryID   = ZeroIntergerIFNull(lstCategory.SelectedValue);

                    obj.ChapterId = ZeroIntergerIFNull(lstChapter.SelectedValue);



                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    objRepository.Addletter(obj);
                    hdnMasterID.Value = gets(obj.code);
                    ViewState["itemID"] = gets(obj.code);

                }
                else
                { //Update

                    if (objRepository.CheckletterExistance((txtLetterSerial.Text), ZeroIntergerIFNull(txtYear.Text), ZeroIntergerIFNull(lstCategory.SelectedValue), ZeroIntergerIFNull(ViewState["itemID"].ToString())))
                    {

                        script = FormatpopupErrorMSG(" [مسلسل الكتاب مسجل من قبل - لنفس التصنيف ]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return 0;
                    }

                    hdnMasterID.Value = ViewState["itemID"].ToString();
                    obj = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                    obj.LastActionDate = DateTime.Now;

                    obj.Arc_Subject = txtSubject.Text;
                    obj.Arc_Num = ZeroIntergerIFNull(txtNum.Text);
                    obj.Arc_Year = ZeroIntergerIFNull(txtYear.Text);


                    obj.Arc_Serial = (txtLetterSerial.Text);
                    obj.Arc_Notes = txtLetterNote.Text;
                    obj.Arc_Date = NullDateifEmpty(txtLetterDate.Text);

                    obj.CategoryID = ZeroIntergerIFNull(lstCategory.SelectedValue);
                    obj.ChapterId = ZeroIntergerIFNull(lstChapter.SelectedValue);

                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                    objRepository.Updateletter(obj);
                }

                string _img = UploadFileoServer(txtQImage, ScannerRepository + _TargetUploadPath+gets(obj.code)+"/");
                if (_img != "")
                    {
                    var objForEdit = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                    objForEdit.Arc_Attachment = _img;
                    objRepository.Updateletter(objForEdit);
                    }

                ViewState["itemID"] = obj.code;

                anchorAttachment.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath+gets(obj.code)+"/" + "&vfileList=[" + gets(obj.Arc_Attachment)+";]";
                anchorAttachment.Visible = true;



            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

            if (!fromScan)
            {
                Response.Redirect("Frm_Pm_Letters.aspx?ss=1&LetterID=" + gets(obj.code));
                return 0;
            }
            return obj.code;
        }
        private void fillLookups()
        {

            FillDll(objLookup.FillPmLetters_Categories(), ref lstCategory, "NameAr", "Code");
            FillDllwithoptional_ALL(objLookup.FillPmLetters_Categories(), ref lstFilterCategory , "NameAr", "Code", "الكل");


            FillDll(objLookup.FillParliament_legislativeChapter(), ref lstChapter, "NameAr", "Code");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeChapter(), ref lstFilterChapter, "NameAr", "Code", "الكل");



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

        #endregion

        #region "Procedure Methods"



        #endregion








        #region "Scanning"


        private void UpdateScannedFile()
        {
            Pm_Letters obj = new Pm_Letters();

            if (Request.Form["fileList"] != null)
            {
                if (Request.Form["fileList"].ToString() != "")
                {

                    switch (Request.Form["ActiveTab"].ToString())
                    {
                        case "1":
                            {//Letter Information


                                ViewState["itemID"] = Request.Form["TargetID"];
                                //  FillLetterMasterInformation();
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
                                            obj = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            obj.Arc_Attachment = null;
                                            objRepository.Updateletter(obj);
                                        }
                                        else
                                        {
                                            obj = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            obj.Arc_Attachment = FileList[i].Split(',')[0].ToString();
                                            objRepository.Updateletter(obj);
                                        }


                                      

                                    }
                                    Response.Redirect("Frm_Pm_Letters.aspx?activetab=1&LetterID=" + Request.Form["TargetID"] );
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
            int _TargetID = SaveLetterMaster(true);



            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath+gets(_TargetID)+"/" + "' />";
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


        #endregion

        //#endregion





        protected void grdLetter_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName== "delete")
            {
                objRepository.Deleteletter((Pm_Letters)objRepository.GetDetails(ZeroIntergerIFNull(e.Item.Cells[1].Text)));
            }
            FillLetters();
        }
    }
}