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

namespace UI.Web.cases.Forms
{
    public partial class CaseAttachments : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public CasesRepository objRepository = IoC.Resolve<CasesRepository>();
        public string _PageTitle = "مرفقات القضية ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "CasesAttachments/Cases/";
        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();


        #endregion

        #region "Page Events"
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "CasesData.aspx";
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
            // lnkScan.Attributes.Add("onclick", "return chkImage();");

            if (!IsPostBack)
            {
                UpdateScannedFile();
                //if ((Request.UrlReferrer == null))
                //{
                //    Response.Redirect("/admin/pages/main.aspx");
                //}
                FillDllwithoptional_ALL(objLookup.FillAttachmentType(), ref lstAttachmentType, "NameAr", "Code", "");
                ViewState["itemID"] = "0";

                ViewState["CaseID"] = "0";
                ViewState["DocID"] = "0";
                ViewState["FileID"] = "0";



                if (Request.QueryString["CaseID"] != null)
                {

                    ViewState["CaseID"] = ZeroIntergerIFNull(Request.QueryString["CaseID"].ToString());
                }
                else if (Request.Form["CaseID"] != null)
                {
                    ViewState["CaseID"] = ZeroIntergerIFNull(Request.Form["CaseID"].ToString());

                }

                if (Request.QueryString["DocID"] != null)
                {
                    ViewState["DocID"] = ZeroIntergerIFNull(Request.QueryString["DocID"].ToString());
                }
                else if (Request.Form["DocID"] != null)
                {
                    ViewState["DocID"] = ZeroIntergerIFNull(Request.Form["DocID"].ToString());

                }

                if (Request.QueryString["FileID"] != null)
                {
                    ViewState["FileID"] = ZeroIntergerIFNull(Request.QueryString["FileID"].ToString());
                }
                else if (Request.Form["FileID"] != null)
                {
                    ViewState["FileID"] = ZeroIntergerIFNull(Request.Form["FileID"].ToString());

                }


                FillGrid();
            }

            if (ViewState["CaseID"].ToString() == "0")
            {
                lblerror.Text = "Fail to Load Case Information";
                string script = FormatpopupErrorMSG("Fail to load Case Infromation", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                return;

            }
            if (ViewState["CaseID"].ToString() != "")
            {
                _TargetUploadPath += ViewState["CaseID"].ToString() + "/";
            }

            if (ViewState["DocID"].ToString() != "" && ViewState["DocID"].ToString() != "0")
            {
                _TargetUploadPath += ViewState["DocID"].ToString() + "/";
            }


            // grdData.DataBind();

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

                arc_Attachments obj = new arc_Attachments();
                for (int i = 0; i <= grdData.Items.Count - 1; i++)
                {

                    if ((grdData.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdData.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteAttachment((arc_Attachments)objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(grdData.Items[i].Cells[0].Text)));
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
        private int SaveAttachmentInformation()
        {
            string script = "";

            arc_Attachments obj = new arc_Attachments();
            try
            {

                if (gets(ViewState["itemID"]).Equals("0"))
                {//Save

                    obj.CaseID = ZeroIntergerIFNull(ViewState["CaseID"].ToString());
                    obj.DocCode = ZeroIntergerIFNull(ViewState["DocID"].ToString());
                    obj.FileID = ZeroIntergerIFNull(ViewState["FileID"].ToString());
                    obj.TypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    if (txtCreationDate.Text != "")
                    {
                        obj.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);
                    }
                    obj.UploadDate = DateTime.Now;


                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject.Text;
                    obj.AttachRef = txtRef.Text;

                    objRepository.AddAttachement(obj);
                }
                else
                { //Update
                    obj = objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    obj.CaseID = ZeroIntergerIFNull(ViewState["CaseID"].ToString());
                    obj.DocCode = ZeroIntergerIFNull(ViewState["DocID"].ToString());
                    obj.FileID = ZeroIntergerIFNull(ViewState["FileID"].ToString());
                    obj.TypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    if (txtCreationDate.Text != "")
                    {
                        obj.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);
                    }


                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject.Text;
                    obj.AttachRef = txtRef.Text;

                    objRepository.UpdateAttachment(obj);

                }

                string _img = UploadFileoServer(txtImage, ScannerRepository + _TargetUploadPath + gets(obj.Code) + "/");
                if (_img != "")
                {
                    var objForEdit = objRepository.GetAttachemtnDetails(obj.Code);
                    obj.Filepath = _img;
                    objRepository.UpdateAttachment(objForEdit);
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

        private int SaveAttachmentInformation_scan()
        {
            string script = "";
            //string _img = UploadFileoServer(txtImage, ScannerRepository + _TargetUploadPath+gets(obj.code)+"/");
            arc_Attachments obj = new arc_Attachments();
            try
            {

                if (gets(ViewState["itemID"]).Equals("0"))
                {//Save

                    obj.CaseID = ZeroIntergerIFNull(ViewState["CaseID"].ToString());
                    obj.DocCode = ZeroIntergerIFNull(ViewState["DocID"].ToString());
                    obj.FileID = ZeroIntergerIFNull(ViewState["FileID"].ToString());
                    obj.TypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    if (txtCreationDate.Text != "")
                    {
                        obj.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);
                    }



                    obj.UploadDate = DateTime.Now;

                    //if (!_img.Equals(""))
                    //{
                    //    obj.Filepath = _img;

                    //}
                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject.Text;
                    obj.AttachRef = txtRef.Text;

                    objRepository.AddAttachement(obj);
                }
                else
                { //Update
                    obj = objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    obj.CaseID = ZeroIntergerIFNull(ViewState["CaseID"].ToString());
                    obj.DocCode = ZeroIntergerIFNull(ViewState["DocID"].ToString());
                    obj.FileID = ZeroIntergerIFNull(ViewState["FileID"].ToString());
                    obj.TypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    if (txtCreationDate.Text != "")
                    {
                        obj.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);
                    }

                    //if (!_img.Equals(""))
                    //{
                    //    obj.Filepath = _img;

                    //}

                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject.Text;
                    obj.AttachRef = txtRef.Text;

                    objRepository.UpdateAttachment(obj);

                }

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

            return obj.Code;
        }
        private void FillGrid()
        {
            int CaseID = 0;
            int DocID = 0;
            int FileID = 0;
            if (Request.QueryString["CaseID"] != null)
            {
                CaseID = ZeroIntergerIFNull(Request.QueryString["CaseID"].ToString());
            }
            else if (Request.Form["CaseID"] != null)
            {
                CaseID = ZeroIntergerIFNull(Request.Form["CaseID"].ToString());

            }

            if (Request.QueryString["DocID"] != null)
            {
                DocID = ZeroIntergerIFNull(Request.QueryString["DocID"].ToString());
            }
            else if (Request.Form["DocID"] != null)
            {
                DocID = ZeroIntergerIFNull(Request.Form["DocID"].ToString());

            }

            if (Request.QueryString["FileID"] != null)
            {
                FileID = ZeroIntergerIFNull(Request.QueryString["FileID"].ToString());
            }
            else if (Request.Form["FileID"] != null)
            {
                FileID = ZeroIntergerIFNull(Request.Form["FileID"].ToString());

            }


            var objList = objRepository.FillCaseDocsAttachemnt(CaseID, DocID, FileID);
            lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
            

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
            var objList = objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {

                if (!gets(objList.Filepath).Equals(""))
                {
                    lnkScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnScannerfilepath.Value = gets(objList.Filepath);
                }


                lstAttachmentType.SelectedValue = gets(objList.TypeCode);
                txtRef.Text = gets(objList.AttachRef);
                txtSubject.Text = gets(objList.AttacheSubject);
                txtCreationDate.Text = gets(objList.ReceiveDate);
                lblimage.Text = "<a target='_blank' href='" + ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList=[" + gets(objList.Filepath) + ";]'>View File</a>";
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
        }
        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            btnSave.Visible = userAccess.Edit || userAccess.Add;
            //btnDelete.Visible = userAccess.Delete;


        }
        #endregion

        #region "Scanning"
        protected void lnkScan_Click(object sender, EventArgs e)
        {
            //Show Loadin div

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
                                                                           //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }
            int _TargetID = SaveAttachmentInformation_scan();



            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + ViewState["DocID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + ViewState["CaseID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["FileID"].ToString() + "' />";
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

        private void UpdateScannedFile()
        {
            arc_Attachments obj = new arc_Attachments();
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

                                obj = objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                obj.Filepath = null;
                                objRepository.UpdateAttachment(obj);
                            }
                            else
                            {

                                obj = objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                obj.Filepath = FileList[i].Split(',')[0].ToString();
                                objRepository.UpdateAttachment(obj);
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