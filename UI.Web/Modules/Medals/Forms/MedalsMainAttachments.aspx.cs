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

namespace UI.Web.Medals.Forms
{
    public partial class MedalsMainAttachments : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public MedalsRepository objRepository = IoC.Resolve<MedalsRepository>();
        public string _PageTitle = "مرفقات الوسام ";

        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "MedalAttachments/";
        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();


        #endregion

        #region "Page Events"
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "MedalsData.aspx";
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

            applyUserPermission();
        }
        
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";
            btnCancel.Attributes.Add("onclick", "Page_ValidationActive=false;");
            btnSave.Attributes.Add("onclick", "return chkImage();");
            lnkScan.Attributes.Add("onclick", "return chkImage();");



            if (!IsPostBack)
            {

                ViewState["itemID"] = "0";
                ViewState["MedalMasterID"] = "0";
                UpdateScannedFile();

                //if ((Request.UrlReferrer == null))
                //{
                //    Response.Redirect("/admin/pages/main.aspx");
                //}
                FillDllwithoptional_ALL(objLookup.FillMedalattachmentTyps(), ref lstAttachmentType, "NameAr", "Code", "");


                if (Request.QueryString["MedalMasterID"] != null)
                {
                    ViewState["MedalMasterID"] = ZeroIntergerIFNull(Request.QueryString["MedalMasterID"].ToString());
                }
                else if (Request.Form["MedalMasterID"] != null)
                {
                    ViewState["MedalMasterID"] = ZeroIntergerIFNull(Request.Form["MedalMasterID"].ToString());
                }



                FillGrid();
            }
            if (ViewState["MedalMasterID"].ToString() == "0")
            {
                lblerror.Text = "Fail to Load Medal Attachment Information";
                string script = FormatpopupErrorMSG("Fail to load Medal Attachment Infromation", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                return;

            }

            _TargetUploadPath  = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "MedalAttachments/"+ ViewState["MedalMasterID"].ToString() + "/";
         //   grdData.DataBind();

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

                Medalmain_Attachments obj = new Medalmain_Attachments();
                for (int i = 0; i <= grdData.Items.Count - 1; i++)
                {

                    if ((grdData.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdData.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteAttacjmentmain((Medalmain_Attachments)objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(grdData.Items[i].Cells[0].Text)));
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
        }
        #endregion

        #region "Fill Information"
        private void FillGrid()
        {



            int MedalMasterID = 0;
            if (Request.QueryString["MedalMasterID"] != null)
            {
                MedalMasterID = ZeroIntergerIFNull(Request.QueryString["MedalMasterID"].ToString());
            }
            else if (Request.Form["MedalMasterID"] != null)
            {
                MedalMasterID = ZeroIntergerIFNull(Request.Form["MedalMasterID"].ToString());
            }

            var objList = objRepository.FillMedalMainAttachemnt(MedalMasterID);
            lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() +  Resources.Utilities.records));
            decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdData.PageSize));
            if ((c <= grdData.CurrentPageIndex))
            {
                grdData.CurrentPageIndex = 0;
            }

            grdData.DataSource = objList;
            grdData.DataBind();
            int _totalCount = objList.Count;
            pager1.ItemCount = _totalCount;

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
                lblimage.Text = "<a target='_blank' href='" + ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList=[" + gets(objList.Filepath) + ";]'>View File</a>";
            }

           
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
        }
        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            btnSave.Visible = userAccess.Edit ||  userAccess.Add;
            //btnDelete.Visible = userAccess.Delete;


        }
        private int SaveAttachmentInformation()
        {
            string script = "";
            Medalmain_Attachments obj = new Medalmain_Attachments();
            try
            {

                if (gets(ViewState["itemID"]).Equals("0"))
                {//Save

                    obj.MedalMasterCode = ZeroIntergerIFNull(Request.QueryString["MedalMasterID"].ToString());


                    obj.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    obj.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);

                    obj.UploadDate = DateTime.Now;
                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject.Text;
                    obj.AttachRef = txtRef.Text;

                    objRepository.AddAttachementmain(obj);
                }
                else
                { //Update
                    obj = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    obj.MedalMasterCode = ZeroIntergerIFNull(Request.QueryString["MedalMasterID"].ToString());

                    obj.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    obj.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);
                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject.Text;
                    obj.AttachRef = txtRef.Text;

                    objRepository.UpdateAttachmentMain(obj);

                }

                //Upload LocalFile
                if (txtImage.FileName != "")
                {
                    string _img = UploadFileoServer(txtImage, ScannerRepository + _TargetUploadPath );
                    if (_img != "")
                    {
                        var objForEdit = objRepository.GetAttachemtnDetails(obj.Code);
                        objForEdit.Filepath = _img;
                        objRepository.UpdateAttachment(objForEdit);
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
            {

                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath  + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='MedalMasterID' name='MedalMasterID' value='" + Request.QueryString["MedalMasterID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";

                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += "  CalllerForm.submit();";
                ScannerPostFrom += "</script>";

                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;


                //_TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&MedalMasterID=" + Request.QueryString["MedalMasterID"].ToString() + "&TargetID=" + _TargetID;
                //Response.Redirect(_TargetUrl);
            }

        }

        private void UpdateScannedFile()
        {
            Medalmain_Attachments obj = new Medalmain_Attachments();
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
                                obj = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                obj.Filepath = null;
                                objRepository.UpdateAttachmentMain(obj);

                            }
                            else
                            {
                                obj = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                obj.Filepath = FileList[i].Split(',')[0].ToString();
                                objRepository.UpdateAttachmentMain(obj);

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
    }
}