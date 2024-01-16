using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using UI.Web.Admin.Controller;

namespace UI.Web.Agreements.Forms
{


    public partial class AgreementAttachments : BaseFormAdmin
    {
        #region "Page Members"
       // WebClient client = new WebClient();
        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/";
        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public AgreementsRepository objRepository = IoC.Resolve<AgreementsRepository>();


        public string _PageTitle = "مرفقات الإجراء ";

        #endregion

        #region "Page Events"
        protected void Page_PreRender(object sender, EventArgs e)
        {

            applyUserPermission();
        }

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
                //Scanner Response Call
                UpdateScannedFile();
                //if ((Request.UrlReferrer == null))
                //{
                //    Response.Redirect("/admin/pages/main.aspx");
                //}
                FillDllwithoptional_ALL(objLookup.FillAttachmentTypes(),ref lstAttachmentType, "NameAr", "Code", "");
                ViewState["itemID"] = "0";
                ViewState["ProcedureID"] = "0";
                ViewState["AgreementCode"] = "0";


                if (Request.QueryString["ProcedureID"] != null)
                {
                    ViewState["ProcedureID"] = ZeroIntergerIFNull(Request.QueryString["ProcedureID"].ToString());
                }
                else if (Request.Form["ProcedureID"] != null)
                {
                    ViewState["ProcedureID"] = ZeroIntergerIFNull(Request.Form["ProcedureID"].ToString());

                }

                if (Request.QueryString["AgreementCode"] != null)
                {
                    ViewState["AgreementCode"] = ZeroIntergerIFNull(Request.QueryString["AgreementCode"].ToString());
                }
                else if (Request.Form["AgreementCode"] != null)
                {
                    ViewState["AgreementCode"] = ZeroIntergerIFNull(Request.Form["AgreementCode"].ToString());

                }

                if (ViewState["AgreementCode"].ToString() == "0" || ViewState["ProcedureID"].ToString() == "0")
                {
                    lblerror.Text = "Fail to Load Agreement Information";
                    string script = FormatpopupErrorMSG("Fail to load agreement Infromation", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    return;

                }


                FillGrid();
            }


            
           
            
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

                Agreement_Attachments obj = new Agreement_Attachments();
                for (int i = 0; i <= grdData.Items.Count - 1; i++)
                {

                    if ((grdData.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdData.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteAttacjment((Agreement_Attachments)objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(grdData.Items[i].Cells[0].Text)));
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
            
            tblAdd.Visible = true;
            tblshow.Visible = false;
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
                if (e.Item.Cells[1].Text == "01/01/1990") e.Item.Cells[1].Text = "";
                if (e.Item.Cells[2].Text == "01/01/1990") e.Item.Cells[2].Text = "";

                if (e.Item.Cells[6].Text == "" || e.Item.Cells[6].Text == "&nbsp;")
                {
                    e.Item.Cells[7].Text = "";
                    e.Item.Cells[8].Text = "";
                }


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
        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            btnSave.Visible = userAccess.Edit ||  userAccess.Add;
            //btnDelete.Visible = userAccess.Delete;


        }

        public string getPublishType(string publishType)
        {
            string _out = "";
            if (publishType == "1")
            {
                _out = "صدر بقانون";
            }
            else if (publishType == "2")
            {
                _out = "صدر بمرسوم";
            }


            return _out;

        }
        private void FillGrid()
        {


            var objList = objRepository.FillAgreementAttachemnt(ZeroIntergerIFNull(ViewState["ProcedureID"].ToString()));
            //Fill Agreement Procedure
            //Check Procedure Dettila
            if (ViewState["ProcedureID"].ToString() !="0")
            {
                var ProcedureDetails = objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(ViewState["ProcedureID"].ToString()));
                if (ProcedureDetails!=null)
                {
                    if (ProcedureDetails.CMGSDessionFile!=null && ProcedureDetails.CMGSDessionFile != "")
                    {
                        objList.Add(new Agreement_Attachments()
                        {
                           Filepath= ProcedureDetails.CMGSDessionFile,
                            UploadDate= ProcedureDetails.ProcedureDate,
                            AttachRef="قرار مجلس الوزراء رقم :   " + ProcedureDetails.CMGSDessionNum

                        });

                    }


                    if (ProcedureDetails.publishDessionFile != null && ProcedureDetails.publishDessionFile != "")
                    {
                        objList.Add(new Agreement_Attachments()
                        {
                            Filepath = ProcedureDetails.publishDessionFile,
                            UploadDate = ProcedureDetails.ProcedureDate,
                            AttachRef = getPublishType(ProcedureDetails.publishType.ToString())  + " رقم " +  ProcedureDetails.decisionNum

                        });

                    }

                }

            }
            _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/" + ViewState["AgreementCode"].ToString() + "/" + ViewState["ProcedureID"].ToString() + "/";


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


                lstAttachmentType.SelectedValue = gets(objList.AttachmenttypeCode);
                txtRef.Text = gets(objList.AttachRef);
                txtSubject.Text = gets(objList.AttacheSubject);
                txtCreationDate.Text = gets(objList.ReceiveDate);
                _TargetUploadPath = _TargetUploadPath + objList.AgreementCode.ToString() + "/" + objList.ProcedureID.ToString() + "/";

                lblimage.Text = "<a target='_blank' href='" + ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList=[" + gets(objList.Filepath) + ";]'>View File</a>";
            }

          
           

        }
        private void ClearForm()
        {
            txtCreationDate.Text = "";
            txtRef.Text = "";
            txtSubject.Text = "";

            //ViewState["itemID"] = 0;
            tblAdd.Visible = false;
            tblshow.Visible = true;
           
        }
        private int SaveAttachmentInformation()
        {
            string script = "";
         
            Agreement_Attachments obj = new Agreement_Attachments();
            try
            {
                _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/" + ViewState["AgreementCode"].ToString() + "/" + ViewState["ProcedureID"].ToString() + "/";
                string _img = UploadFileoServer(txtImage, ScannerRepository + _TargetUploadPath );
                if (gets(ViewState["itemID"]).Equals("0"))
                {//Save

                    obj.AgreementCode = ZeroIntergerIFNull(ViewState["AgreementCode"].ToString());

                    obj.ProcedureID = ZeroIntergerIFNull(ViewState["ProcedureID"].ToString()); ;
                    obj.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    obj.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);

                    obj.UploadDate = DateTime.Now;

                    if (!_img.Equals(""))
                    {
                        obj.Filepath = _img;

                    }
                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject.Text;
                    obj.AttachRef = txtRef.Text;

                    objRepository.AddAttachement(obj);
                }
                else
                { //Update
                    obj = objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    obj.AgreementCode = ZeroIntergerIFNull(ViewState["AgreementCode"].ToString());
                    obj.ProcedureID = ZeroIntergerIFNull(ViewState["ProcedureID"].ToString()); ;
                    obj.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    obj.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);

                    if (!_img.Equals(""))
                    {
                        obj.Filepath = _img;

                    }

                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtSubject.Text;
                    obj.AttachRef = txtRef.Text;

                    objRepository.UpdateAttachment(obj);

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
            _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/" + ViewState["AgreementCode"].ToString() + "/" + ViewState["ProcedureID"].ToString() + "/";


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
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnScannerfilepath.Value + "]' />";

                ScannerPostFrom += "<input type='hidden' id='AgreementCode' name='AgreementCode' value='" + ViewState["AgreementCode"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='ProcedureID' name='ProcedureID' value='" + (ViewState["ProcedureID"].ToString()) + "' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";
                     //_TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&AgreementCode=" + Request.QueryString["AgreementCode"].ToString() + "&TargetID=" + _TargetID;
                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += "  CalllerForm.submit();";
                ScannerPostFrom += "</script>";

                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;

                 //TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&AgreementCode=" + Request.QueryString["AgreementCode"].ToString() + "&ProcedureID=" + Request.QueryString["ProcedureID"].ToString() + "&TargetID=" + _TargetID;
               // Response.Redirect(_TargetUrl);
            }

        }

        private void UpdateScannedFile()
        {
            Agreement_Attachments obj = new Agreement_Attachments();
            if (Request.Form["fileList"]!=null)
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

                                obj = objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                obj.Filepath =null;//Clear File
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