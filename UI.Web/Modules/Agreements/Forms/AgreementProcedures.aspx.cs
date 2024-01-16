using System;
using System.Collections.Generic;
using System.Configuration;
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


    public partial class AgreementProcedures : BaseFormAdmin
    {
        #region "Page Members"
       // WebClient client = new WebClient();
        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/";
        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public AgreementsRepository objRepository = IoC.Resolve<AgreementsRepository>();
        public string _PageTitle = "إجراءات الاتفاقية ";

        #endregion

        #region "Page Events"
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "AgreementsData.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";
            lnkCancelProcedure.Attributes.Add("onclick", "Page_ValidationActive=false;");
            lnkSaveProcesure.Attributes.Add("onclick", "return chkImage();");
            lnkScan.Attributes.Add("onclick", "return chkImage();");


            if (!IsPostBack)
            {

                FillDllwithoptional_ALL(objLookup.FillProcedureTypes(), ref lstProcedureType, "NameAr", "Code", "");
                FillDllwithoptional_ALL(objLookup.FillAttachmentTypes(), ref lstAttachmentType, "NameAr", "Code", "");
                FillDllwithoptional_ALL(objLookup.FillAgreementRelatedOrgs(), ref lstRelatedOrgs, "NameAr", "Code", "");

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
                    hdnMasterID.Value =  Request.QueryString["AgreementCode"].ToString();
                }
                else if (Request.Form["AgreementCode"] != null)
                {
                    ViewState["AgreementCode"] = ZeroIntergerIFNull(Request.Form["AgreementCode"].ToString());
                    hdnMasterID.Value =  (Request.Form["AgreementCode"].ToString());

                }


                FillAgreementProcedures(ZeroIntergerIFNull(hdnMasterID.Value));
            }

        }

        protected void lnkDeleteProcedure_Click(object sender, EventArgs e)
        {


            try
            {

                Agreement_procedureHistory obj = new Agreement_procedureHistory();
                for (int i = 0; i <= grdProcedure.Items.Count - 1; i++)
                {

                    if ((grdProcedure.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdProcedure.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteProceduret((Agreement_procedureHistory)objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(grdProcedure.Items[i].Cells[0].Text)));
                        }
                    }
                }


                FillAgreementProcedures(ZeroIntergerIFNull(hdnMasterID.Value));
                //TODO Update Aggreement with last Procedure Taken

                //// Update Agreement With last procesdure Taken
                //var LastProcesdureTaken = objRepository.FillAgreementProcedures(ZeroIntergerIFNull(hdnMasterID.Value)).First();
                //if (LastProcesdureTaken != null)
                //{
                //    UpdateAgreementLastProcedure(LastProcesdureTaken.ProcedureTypeCode.Value);
                //}
                //else { UpdateAgreementLastProcedure(0); }
                UpdateAgreementLastProcedure();
            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteDataFailed + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }


        }

        protected void grdData_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["ProceduresitemID"] = e.Item.Cells[0].Text;
            divProAttache.Visible = false;
            FillProcedureFrom();

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




        #endregion

        #region "Procedure"
        private void FillAgreementProcedures(int AgreementID)
        {
            var objList = objRepository.FillAgreementProcedures(AgreementID);
           // lblProcerduresCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));


            decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdProcedure.PageSize));
            if ((c <= grdProcedure.CurrentPageIndex))
            {
                grdProcedure.CurrentPageIndex = 0;
            }

            //List Duplication
            //List<View_InboundItems> duplicatedList = new List<View_InboundItems>();
            //duplicatedList = DuplicatedList(objList);

            if (objList.Count > 0)
            {
                //btnSave.Visible = true;
                //lnkBack.Visible = true;

                divshowProcesure.Visible = true;

                divAddProcedure.Visible = false;

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

            grdProcedure.DataSource = objList;
            grdProcedure.DataBind();



        }
        protected void lnkSaveProcesure_Click(object sender, EventArgs e)
        {
            SaveProcedureInformation();

        }
        private void CleaProcesure()
        {


            ViewState["ProceduresitemID"] = "0";
            txtProcedureActionDate.Text = "";
            txtRemarks.Text = "";
            lstAttachmentType.SelectedValue = "0";
            txtDessionNum.Text = "";
            lstRelatedOrgs.SelectedValue = "0";
            txtRef.Text = "";

            //BlblSubTitle.Text = this.GetTitle(true);
        }

        private void FillProcedureFrom()
        {

            var objList = objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(ViewState["ProceduresitemID"].ToString()));
            if ((objList != null))
            {


                txtRemarks.Text = gets(objList.Remarks);

                lstProcedureType.SelectedValue = gets(objList.ProcedureTypeCode);
                txtProcedureActionDate.Text = NullDateifEmptyToText(objList.ProcedureDate);
                if (objList.RelatedOrgID != 0 && objList.RelatedOrgID != null)
                {
                    lstRelatedOrgs.SelectedValue = gets(objList.RelatedOrgID);
                    divRelatedOrg.Visible = true;
                }
                else
                { divRelatedOrg.Visible = false; }

                if (objList.decisionNum != "")
                {
                    //divDessionNum.Visible = true;
                    txtDessionNum.Text = objList.decisionNum;
                }
                else
                { //divDessionNum.Visible = false;
                }

                if (objList.publishType != 0 && objList.publishType != null)
                {
                    rblPublishType.SelectedValue = objList.publishType.ToString();

                }
                else { rblPublishType.SelectedValue = null; }

                if (objList.ProcedureTypeCode == 6)
                {
                    divDessionNum.Visible = true;

                }
                else { divDessionNum.Visible = false; }



            }

            divAddProcedure.Visible = true;
            divshowProcesure.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }

        private int SaveProcedureInformation()
        {
            string script = "";
            int _TragetID = 0;
            try
            {
                Agreement_procedureHistory obj = new Agreement_procedureHistory();
                if (ViewState["ProceduresitemID"].Equals("0"))
                {//Save

                    obj.ActionDate = DateTime.Now;

                    obj.Remarks = txtRemarks.Text;
                    obj.ProcedureDate = NullDateifEmpty(txtProcedureActionDate.Text);

                    obj.AgreementCode = ZeroIntergerIFNull(hdnMasterID.Value);
                    obj.ProcedureTypeCode = ZeroIntergerIFNull(lstProcedureType.SelectedValue);
                    obj.Actionby = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    obj.RelatedOrgID = ZeroIntergerIFNull(lstRelatedOrgs.SelectedValue);
                    obj.RelatedOrgName = gets(lstProcedureType.SelectedItem.Text);
                    obj.decisionNum = txtDessionNum.Text;
                    if (rblPublishType.SelectedValue != null)
                    {
                        obj.publishType = ZeroIntergerIFNull(rblPublishType.SelectedValue);
                    }

                    objRepository.AddProcedure(obj);

                    ViewState["ProceduresitemID"] = gets(obj.Code);

                    // Save Documenting Status

                    //Agreement_procedureHistory objStatus = new Agreement_procedureHistory();
                    //if (ViewState["StatusTrackingitemID"].Equals("0"))
                    //{//Save


                    //    objStatus.AgreementCode = obj.Code;
                    //    objStatus.ActionDate = DateTime.Now;
                    //    objStatus.Remarks = "New Request Documenting Status";
                    //    objStatus.DepositeStatusTypeCode = 1;


                    //    objRepository.AddProcedure(objStatus);
                    //}
                    // Add Agreemrnt Data



                    // Update Agreement master Data

                    //AgreementData objAgreementData = new AgreementData();
                    //objAgreementData = objRepository.GetDetails(ZeroIntergerIFNull(hdnMasterID.Value));
                    //objAgreementData.LastActionID = ZeroIntergerIFNull(lstProcedureType.SelectedValue);
                    //objRepository.UpdateAgreement(objAgreementData);



                    // Add Procesures Attachments

                    if (lstAttachmentType.SelectedValue != "0")
                    {

                       

                        Agreement_Attachments objAttachment = new Agreement_Attachments();


                        objAttachment.AgreementCode = ZeroIntergerIFNull(hdnMasterID.Value);

                        objAttachment.ProcedureID = obj.Code;
                        objAttachment.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                        objAttachment.ReceiveDate = NullDateifEmpty(txtProcedureActionDate.Text);

                        objAttachment.UploadDate = DateTime.Now;

                        //if (!_img.Equals(""))
                        //{
                        //    objAttachment.Filepath = _img;

                        //}
                        if (rblPublishType.SelectedValue != null)
                        {
                            obj.publishType = ZeroIntergerIFNull(rblPublishType.SelectedValue);
                        }

                        objAttachment.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                        objAttachment.AttacheSubject = txtRemarks.Text;
                        objAttachment.AttachRef = txtRef.Text;
                        objRepository.AddAttachement(objAttachment);

                        _TragetID = objAttachment.Code;
                    }



                }
                else
                { //Update


                    obj = objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(ViewState["ProceduresitemID"].ToString()));

                    obj.Remarks = txtRemarks.Text;
                    obj.ProcedureDate = NullDateifEmpty(txtProcedureActionDate.Text);
                    obj.AgreementCode = ZeroIntergerIFNull(hdnMasterID.Value);
                    obj.ProcedureTypeCode = ZeroIntergerIFNull(lstProcedureType.SelectedValue);
                    obj.Actionby = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    obj.RelatedOrgID = ZeroIntergerIFNull(lstRelatedOrgs.SelectedValue);
                    obj.RelatedOrgName = gets(lstProcedureType.SelectedItem.Text);
                    obj.decisionNum = txtDessionNum.Text;


                    objRepository.UpdateProcedure(obj);


                    // Update Agreement With last procesdure Taken
                    //var LastProcesdureTaken = objRepository.FillAgreementProcedures(ZeroIntergerIFNull(hdnMasterID.Value)).First();
                    //if (LastProcesdureTaken != null)
                    //{
                    //    UpdateAgreementLastProcedure(LastProcesdureTaken.ProcedureTypeCode.Value);
                    //}
                    //else { UpdateAgreementLastProcedure(0); }



                }



                //string _img = UploadFileoServer(txtImage, ScannerRepository + _TargetUploadPath + gets(obj.Code) + "/");
                //if (_img != "")
                //{
                //    var objForEdit = objRepository.GetprOCEDUREDetails(obj.Code);
                //    objForEdit.publishDessionFile = _img;
                //    objRepository.UpdateProcedure(objForEdit);
                //}

                UpdateAgreementLastProcedure();

                ViewState["ProceduresitemID"] = obj.Code;

                CleaProcesure();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            FillAgreementProcedures(ZeroIntergerIFNull(hdnMasterID.Value));
            return _TragetID;
        }
        private void UpdateAgreementLastProcedure()
        {
            try
            {
                // Update Agreement With Last procedure Based On Date

                var ProcedureObj = objRepository.GetAgreementLastProcedureobj(ZeroIntergerIFNull(hdnMasterID.Value));
                if (ProcedureObj != null)
                {
                    AgreementData objAgreementData = new AgreementData();
                    objAgreementData = objRepository.GetDetails(ZeroIntergerIFNull(hdnMasterID.Value));
                    objAgreementData.LastActionID = ProcedureObj.ProcedureTypeCode;
                    objAgreementData.LastProcedureRefId = ProcedureObj.Code;
                    objRepository.UpdateAgreement(objAgreementData);
                }
                else
                {
                    AgreementData objAgreementData = new AgreementData();
                    objAgreementData = objRepository.GetDetails(ZeroIntergerIFNull(hdnMasterID.Value));
                    objAgreementData.LastActionID = 0;
                    objAgreementData.LastProcedureRefId = 0;
                    objRepository.UpdateAgreement(objAgreementData);

                }

            }
            catch (Exception)
            {

                throw;
            }


        }
        protected void lnkCancelProcedure_Click(object sender, EventArgs e)
        {
            divAddProcedure.Visible = false;
            divshowProcesure.Visible = true;
            ViewState["ProceduresitemID"] = "0";
        }
        protected void lstProcedureType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (lstProcedureType.SelectedValue == "3")//الاحالو لجهة الاختصاص
            {
                divRelatedOrg.Visible = true;
            }
            else { divRelatedOrg.Visible = false; }

            if (lstProcedureType.SelectedValue == "4" || lstProcedureType.SelectedValue == "5" || lstProcedureType.SelectedValue == "6")//صدر بمرسوم
            {
                divDessionNum.Visible = true;
            }
            else { divDessionNum.Visible = false; }

        }

        protected void lnkAddNewProcedurew_Click(object sender, EventArgs e)
        {
            divAddProcedure.Visible = true;
            divshowProcesure.Visible = false;
            divProAttache.Visible = true;
            ViewState["ProceduresitemID"] = "0";
        }



        #endregion

        #region "Scanning"
        protected void lnkScan_Click(object sender, EventArgs e)
        {

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }

            //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";

            int _TargetID = SaveProcedureInformation();

            if (_TargetID != 0)
            {

                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='1' />";
                ScannerPostFrom += "<input type='hidden' id='id' name='id' value='" + ViewState["itemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";
                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += "  CalllerForm.submit();";
                ScannerPostFrom += "</script>";

                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;


                //_TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&id=" + Request.QueryString["id"].ToString() + "&TargetID=" + _TargetID;
                //Response.Redirect(_TargetUrl);
            }

        }

        private void UpdateScannedFile()
        {
            Agreement_Attachments obj = new Agreement_Attachments();
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