using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using UI.Web.Admin.Controller;

namespace UI.Web.Agreements.Forms
{
    public partial class AgreementMainAttachments : BaseFormAdmin
    {
        #region "Page Members"

        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public AgreementsRepository objRepository = IoC.Resolve<AgreementsRepository>();
        public string _PageTitle = "مرفقات الاتفاقية ";

        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/";
        public string _TargetUploadPath2 = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();


        #endregion

        #region "Page Events"

        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "AgreementsData.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";
            btnCancel.Attributes.Add("onclick", "Page_ValidationActive=false;");
            btnSave.Attributes.Add("onclick", "return chkImage();");
            lnkScan.Attributes.Add("onclick", "return chkImage();");

            if (!IsPostBack)
            {

                //if ((Request.UrlReferrer == null))
                //{
                //    Response.Redirect("/admin/pages/main.aspx");
                //}
                FillDllwithoptional_ALL(objLookup.FillAttachmentTypes(), ref lstAttachmentType, "NameAr", "Code", "");
                FillDllwithoptional_ALL(objLookup.FillAttachmentTypes(), ref lstAttachmentType2, "NameAr", "Code", "");


                ViewState["itemID"] = "0";
                ViewState["AgreementCode"] = "0";


                if (Request.QueryString["AgreementCode"] != null)
                {
                    ViewState["AgreementCode"] = ZeroIntergerIFNull(Request.QueryString["AgreementCode"].ToString());
                }
                else if (Request.Form["AgreementCode"] != null)
                {
                    ViewState["AgreementCode"] = ZeroIntergerIFNull(Request.Form["AgreementCode"].ToString());

                }
                if (Request.Form["ParentID"] != null)
                {
                    ViewState["AgreementCode"] = ZeroIntergerIFNull(Request.Form["ParentID"].ToString());
                }




              

                //Scanner Response Call
                UpdateScannedFile();
                FillGrid();
            }

            if (ViewState["AgreementCode"].ToString() == "0")
            {
                lblerror.Text = "Fail to Load Agreement Information";
                string script = FormatpopupErrorMSG("Fail to load agreement Infromation", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                return;

            }

            //_TargetUploadPath += ViewState["AgreementCode"].ToString() + "/";
            //if (hdnRelatedAgreement.Value!="")
            //{
            //    _TargetUploadPath += hdnRelatedAgreement.Value.ToString() + "/";
            //}


           // grdData.DataBind();
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

            applyUserPermission();
        }
        protected void pager_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdData.PageCount))
            {
                currnetPageIndx = (grdData.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdData.CurrentPageIndex = (currnetPageIndx - 1);
            this.FillGrid();
        }
        protected void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {

                AgreementMain_Attachments obj = new AgreementMain_Attachments();
                for (int i = 0; i <= grdData.Items.Count - 1; i++)
                {

                    if ((grdData.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdData.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteAttacjmentMain((AgreementMain_Attachments)objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(grdData.Items[i].Cells[0].Text)));
                        }
                    }
                }
                FillGrid();

            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteDataFailed + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

        }

        protected void grdData_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            this.ClearForm();
            ViewState["itemID"] = id;
            this.FillForm();
            tblshow.Visible = false;
            tblAdd.Visible = true;
        }

        protected void grdData_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#DA9CF1\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
            }

            if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#DA9CF1\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#EFEFEF\';");
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[2].Text == "01/01/1990") e.Item.Cells[2].Text = "";
                if (e.Item.Cells[3].Text == "01/01/1990") e.Item.Cells[3].Text = "";




            }

        }
        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            SaveAttachmentInformation();

        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.ClearForm();
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.FillGrid();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {

            this.ClearForm();
            tblAdd.Visible = true;
            tblshow.Visible = false;

           // Response.Redirect("/Doc_Scanner/online_demo_scan.aspx");
        }
        #endregion

        #region "Fill Information"
        private void FillGrid()
        {
            int AgreementCode = 0;
            if (Request.QueryString["AgreementCode"] != null)
            {
                AgreementCode = ZeroIntergerIFNull(Request.QueryString["AgreementCode"].ToString());
                hdnAgreementCode.Value= (Request.QueryString["AgreementCode"].ToString());
            }
            else if (Request.Form["AgreementCode"] != null)
            {
                AgreementCode = ZeroIntergerIFNull(Request.Form["AgreementCode"].ToString());
                hdnAgreementCode.Value = Request.Form["AgreementCode"].ToString();
            }

            if (Request.Form["ParentID"] != null)
            {
                AgreementCode = ZeroIntergerIFNull(Request.Form["ParentID"].ToString());
                hdnAgreementCode.Value = Request.Form["ParentID"].ToString();

            }

           _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/"+ AgreementCode.ToString()+"/";
           



        var agreementDetails = objRepository.GetDetails(ZeroIntergerIFNull(hdnAgreementCode.Value));
            if (agreementDetails!=null )
            {
                hdnIsInitial.Value = agreementDetails.isInitial.ToString();
            }


            var objList = objRepository.FillAgreementMainAttachemnt(ZeroIntergerIFNull(hdnAgreementCode.Value));
            lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() +  Resources.Utilities.records));
            lblAttchmentCount.Text = objList.Count.ToString();
            decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdData.PageSize));
            if ((c <= grdData.CurrentPageIndex))
            {
                grdData.CurrentPageIndex = 0;
            }

            grdData.DataSource = objList;
            grdData.DataBind();
            int _totalCount = objList.Count;
            pager1.ItemCount = _totalCount;


            //Fill Related Attachment

            var objRelatedAgreement = objRepository.getRelatedAgreement(ZeroIntergerIFNull(hdnAgreementCode.Value));
            if (objRelatedAgreement != null && objRelatedAgreement.Count > 0)
            {
                lbltrlatedType.Text = getAgreementType(objRelatedAgreement[0].isInitial);
                hdnRelatedAgreement.Value = objRelatedAgreement[0].Code.ToString();

                _TargetUploadPath2 = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/"+ objRelatedAgreement[0].Code.ToString()+"/";
                //Fill Related Attachment


                objList = objRepository.FillAgreementMainAttachemnt(objRelatedAgreement[0].Code);
                lblcount2.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
                  c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdData.PageSize));
                if ((c <= grdData2.CurrentPageIndex))
                {
                    grdData2.CurrentPageIndex = 0;
                }
                lblAttchment2Count.Text = objList.Count.ToString();
                grdData2.DataSource = objList;
                grdData2.DataBind();
                  _totalCount = objList.Count;
                pager2.ItemCount = _totalCount;




            }
            else
            { lbltrlatedType.Text = ""; }


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
        private void FillForm()
        {
            var objList = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {


                if (!gets(objList.Filepath).Equals(""))
                {
                    lnkScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnScannerfilepath.Value = gets(objList.Filepath);
                }

                lstAttachmentType.SelectedValue = gets(objList.AttachmenttypeCode);
                txtRef.Text = gets(objList.AttachRef);
                txtSubject.Text = gets(objList.AttacheSubject);
                txtCreationDate.Text = gets(objList.ReceiveDate);
                lblimage.Text = "<a target='_blank' href='"+ ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList=[" + gets(objList.Filepath) + ";]'>View File</a>";
            }

            tblAdd.Visible = true;
            lblSubTitle.Text = this.GetTitle(false);

        }
        private void ClearForm()
        {
            txtCreationDate.Text = "";
            txtRef.Text = "";
            txtSubject.Text = "";

            ViewState["itemID"] = 0;
            tblAdd.Visible = false;
            tblshow.Visible = true;
            lblSubTitle.Text = this.GetTitle(true);



             txtCreationDate2.Text = "";
            txtRef2.Text = "";
            txtSubject2.Text = "";

            ViewState["itemID"] = 0;
            tblAdd2.Visible = false;
            tblshow2.Visible = true;

        }

        private int SaveAttachmentInformation()
        {
            string script = "";
            
            AgreementMain_Attachments obj = new AgreementMain_Attachments();
            try
            {

                if (gets(ViewState["itemID"]).Equals("0"))
                {//Save

                    obj.AgreementCode = ZeroIntergerIFNull(ViewState["AgreementCode"].ToString());
                    obj.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    obj.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);

                    obj.UploadDate = DateTime.Now;
                   
                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject.Text;
                    obj.AttachRef = txtRef.Text;

                    objRepository.AddAttachementMain(obj);
                }
                else
                { //Update
                    obj = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                    obj.AgreementCode = ZeroIntergerIFNull(ViewState["AgreementCode"].ToString());
                    obj.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    obj.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);
 

                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject.Text;
                    obj.AttachRef = txtRef.Text;

                    objRepository.UpdateAttachmentmain(obj);

                }
                //Upload LocalFile
                if (txtImage.FileName != "")
                {
                    string _img = UploadFileoServer(txtImage, ScannerRepository + _TargetUploadPath + gets(ViewState["AgreementCode"].ToString()) + "/");
                    if (_img != "")
                    {
                        var objForEdit = objRepository.GetAttachemtnMainDetails(obj.Code);
                        objForEdit.Filepath = _img;
                        objRepository.UpdateAttachmentmain(objForEdit);
                    }
                }

                ClearForm();
                FillGrid();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            return obj.Code;
        }
        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            btnSave.Visible = userAccess.Edit ||  userAccess.Add;
            //btnDelete.Visible = userAccess.Delete;


        }

        #endregion

        #region "Scanning"
        protected void lnkScan_Click(object sender, EventArgs e)
        {

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
                                                                           //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }


            int _TargetID = SaveAttachmentInformation();

            if (_TargetID != 0)
            {// Here we Will Create and Submit From
                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='"+ _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath  + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnScannerfilepath.Value + "]' />";

                ScannerPostFrom += "<input type='hidden' id='AgreementCode' name='AgreementCode' value='" + ViewState["AgreementCode"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";

                //_TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&AgreementCode=" + Request.QueryString["AgreementCode"].ToString() + "&TargetID=" + _TargetID;
                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += "  CalllerForm.submit();";
                ScannerPostFrom += "</script>";

                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;

             //   Response.Redirect(_TargetUrl);
            }

        }

        private void UpdateScannedFile()
        {
            AgreementMain_Attachments obj = new AgreementMain_Attachments();
            if (Request.Form["fileList"] != null)
            {
                if (Request.Form["fileList"].ToString() != "")
                {
                    string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                    _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                    string[] FileList = _ScannerFileLlisy.Split(';');

                    if (Request.Form["TargetID"] != null)
                    {
                        for (int i = 0; i < FileList.Length; i++)
                        {//Update Current Record With

                            if (Request.Form["emptyFile"] != null)
                            {
                                obj = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                obj.Filepath = null;
                                objRepository.UpdateAttachmentmain(obj);
                            }
                            else
                            {

                                obj = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                obj.Filepath = FileList[i].Split(',')[0].ToString();
                                objRepository.UpdateAttachmentmain(obj);
                            }
                        }

                    }

                    string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                }
                else
                {
                    string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                }


            }


        }

        #endregion

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
        public string showRelatedFiles()
        {


            int AgreementCode = 0;
            if (Request.QueryString["AgreementCode"] != null)
            {
                AgreementCode = ZeroIntergerIFNull(Request.QueryString["AgreementCode"].ToString());
                hdnAgreementCode.Value = Request.QueryString["AgreementCode"].ToString();


            }
            else if (Request.Form["AgreementCode"] != null)
            {
                AgreementCode = ZeroIntergerIFNull(Request.Form["AgreementCode"].ToString());

                hdnAgreementCode.Value = (Request.Form["AgreementCode"].ToString());

            }

            if (Request.Form["ParentID"] != null)
            {
                AgreementCode = ZeroIntergerIFNull(Request.Form["ParentID"].ToString());
                hdnAgreementCode.Value = (Request.Form["ParentID"].ToString());
            }


            string _out = "none";
            var objRelatedAgreement = objRepository.getRelatedAgreement(ZeroIntergerIFNull(hdnAgreementCode.Value));
            if (objRelatedAgreement != null && objRelatedAgreement.Count > 0)
            {
                lbltrlatedType.Text = getAgreementType(objRelatedAgreement[0].isInitial);
                hdnRelatedAgreement.Value = objRelatedAgreement[0].Code.ToString();
;                _out = "";

                //Fill Related Attachment





                var objList = objRepository.FillAgreementMainAttachemnt(objRelatedAgreement[0].Code);
                lblcount2.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
                decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdData.PageSize));
                if ((c <= grdData2.CurrentPageIndex))
                {
                    grdData2.CurrentPageIndex = 0;
                }

                grdData2.DataSource = objList;
                grdData2.DataBind();
                int _totalCount = objList.Count;
                pager2.ItemCount = _totalCount;




            }
            else
            { lbltrlatedType.Text = ""; }

            return _out;
        }
        public string getAgreementType(bool? isInitial)
        {

            string _out = "";
            switch (isInitial)
            {
                case true:
                    {
                        _out = "<span class='label bg-success-400'>مبدئية  </span>";
                        break;
                    }
                case false:
                    {
                        _out = "<span class='label bg-warning-400'>نهائية </span>";
                        break;
                    }

            }
            return _out;


        }

        protected void lnkScan2_Click(object sender, EventArgs e)
        {
            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
                                                                           //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }


            int _TargetID = SaveAttachmentInformation2();

            if (_TargetID != 0)
            {// Here we Will Create and Submit From
                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath2 + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnScannerfilepath2.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnScannerfilepath2.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='AgreementCode' name='AgreementCode' value='" + hdnRelatedAgreement.Value.ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='AgreementCode' name='ParentID' value='" + ViewState["AgreementCode"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";

                //_TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&AgreementCode=" + Request.QueryString["AgreementCode"].ToString() + "&TargetID=" + _TargetID;
                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += "  CalllerForm.submit();";
                ScannerPostFrom += "</script>";

                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;

                //   Response.Redirect(_TargetUrl);
            }

        }

        protected void btnSave2_Click(object sender, EventArgs e)
        {
            SaveAttachmentInformation2();
        }

        protected void btnCancel2_Click(object sender, EventArgs e)
        {
            this.ClearForm();
        }

        protected void btnSave2_Click1(object sender, EventArgs e)
        {
            SaveAttachmentInformation2();
        }

        protected void grdData2_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#DA9CF1\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
            }

            if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#DA9CF1\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#EFEFEF\';");
            }
        }

        protected void grdData2_EditCommand(object source, DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            this.ClearForm();
            ViewState["itemID"] = id;
            this.FillForm2();
            tblshow2.Visible = false;
            tblAdd2.Visible = true;
        }

        protected void pager2_Command(object sender, CommandEventArgs e)
        {

        }

        protected void btnDelete2_Click(object sender, EventArgs e)
        {
            try
            {

                AgreementMain_Attachments obj = new AgreementMain_Attachments();
                for (int i = 0; i <= grdData2.Items.Count - 1; i++)
                {

                    if ((grdData2.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdData2.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteAttacjmentMain((AgreementMain_Attachments)objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(grdData2.Items[i].Cells[0].Text)));
                        }
                    }
                }
                FillGrid();

            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteDataFailed + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
        }


        private int SaveAttachmentInformation2()
        {
            string script = "";
            string _img = UploadFileoServer(txtImage2, ScannerRepository + _TargetUploadPath);
            AgreementMain_Attachments obj = new AgreementMain_Attachments();
            try
            {

                if (gets(ViewState["itemID"]).Equals("0"))
                {//Save

                    obj.AgreementCode =ZeroIntergerIFNull( hdnRelatedAgreement.Value);
                    obj.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType2.SelectedValue);
                    obj.ReceiveDate = NullDateifEmpty(txtCreationDate2.Text);

                    obj.UploadDate = DateTime.Now;
                    if (!_img.Equals(""))
                    {
                        obj.Filepath = _img;

                    }
                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject2.Text;
                    obj.AttachRef = txtRef2.Text;

                    objRepository.AddAttachementMain(obj);
                }
                else
                { //Update
                    obj = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                    obj.AgreementCode = ZeroIntergerIFNull(hdnRelatedAgreement.Value);
                    obj.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType2.SelectedValue);
                    obj.ReceiveDate = NullDateifEmpty(txtCreationDate2.Text);

                    if (!_img.Equals(""))
                    {
                        obj.Filepath = _img;

                    }

                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject2.Text;
                    obj.AttachRef = txtRef2.Text;

                    objRepository.UpdateAttachmentmain(obj);

                }

                ClearForm();
                FillGrid();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            return obj.Code;
        }
        private void FillForm2()
        {
            var objList = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {

                if (!gets(objList.Filepath).Equals(""))
                {
                    lnkScan2.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnScannerfilepath2.Value = gets(objList.Filepath);
                }




                lstAttachmentType2.SelectedValue = gets(objList.AttachmenttypeCode);
                txtRef2.Text = gets(objList.AttachRef);
                txtSubject2.Text = gets(objList.AttacheSubject);
                txtCreationDate2.Text = gets(objList.ReceiveDate);
                lblimage2.Text = "<a target='_blank' href='" + ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList='[" + gets(objList.Filepath) + ";]'>View File</a>";
            }

            tblAdd2.Visible = true;
           // lblSubTitle2.Text = this.GetTitle(false);

        }
        protected void btnNew2_Click(object sender, EventArgs e)
        {
            this.ClearForm();
            tblAdd2.Visible = true;
            tblshow2.Visible = false;
        }
    }
}