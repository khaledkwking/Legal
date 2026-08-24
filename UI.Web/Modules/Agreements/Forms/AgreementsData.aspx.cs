using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using Newtonsoft.Json;
using UI.Web.Admin.Controller;

namespace UI.Web.Agreements.Forms
{
    public partial class AgreementsData : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public AgreementsRepository objRepository = IoC.Resolve<AgreementsRepository>();
        public string _PageTitle = "الاتفاقيات ";
        public string AgreementCode = "0";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/";
        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();


        






        public string _LawDocsTargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LawsAttachments/";


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
            lnkSaveProcesure.Attributes.Add("onclick", "return chkProcedure();");

            if (Request.QueryString["activetab"] != null)
            {
                hdnactivetab.Value = gets(Request.QueryString["activetab"]);
            }

            if (!IsPostBack)
            {
                UpdateScannedFile();

                if (Request.QueryString["ss"] != null && Request.QueryString["ss"].ToString() == "1")
                {
                    //  ClearForm();

                    string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);


                }

                fillLookups();

                if (Request.QueryString["ProTypeID"] != null)
                {
                    if (Request.QueryString["ProTypeID"].ToString() == "-1")
                    {
                        List<int> notInList = new List<int>();
                        notInList.Add(1);
                        notInList.Add(2);
                        notInList.Add(3);
                        notInList.Add(6);
                        notInList.Add(16);
                        FillAgreements(notInList);
                    }
                    else
                    {
                        lstFilterProcedures.SelectedValue = gets(Request.QueryString["ProTypeID"]);
                        FillAgreements();
                    }


                }

                if (Request.QueryString["current"] != null)
                {
                    FillCurrentAgreements();

                }
                ViewState["itemID"] = "0";
                ViewState["SpEdit"] = "0";
                ViewState["NewDesc"] = "";
                ViewState["NewBar"] = "";
                ViewState["NewIsbn"] = "";
                ViewState["SPITEM"] = "";
                ViewState["NewPrice"] = "0";
                Session["ItemList"] = null;
                ViewState["itemID"] = "0";
                ViewState["parentID"] = "0";
                ViewState["ProceduresitemID"] = "0";
                if (Request.QueryString["id"] != null)
                {
                    ViewState["itemID"] = Request.QueryString["id"].ToString();

                }
                else if (Request.Form["id"] != null)
                {
                    ViewState["itemID"] = Request.Form["id"].ToString();
                }

                if (ViewState["itemID"].ToString()!="0")
                {
                    tblshow.Visible = false;
                    tblSearch.Visible = false;
                    tblAdd.Visible = true;

                    btnProcedure.Visible = false;
                    //ViewState["itemID"] = Request.QueryString["id"].ToString();
                    FillAgreementMasterInformation();
                    //Fill Agreemnt Details
                    FillAgreementProcedures(ZeroIntergerIFNull(ViewState["itemID"].ToString()), hdnIsInitial.Value);

                }



                SetPageTitle();

                ViewState["OutboundItemID"] = "0";

                // FillInboundItems();


            }

        }
        protected void grdInboundItems_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            List<View_AgreeementsList> RelatedAgreements = new List<View_AgreeementsList>() ;

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
                string code = e.Item.Cells[3].Text;
                RelatedAgreements = objRepository.getRelatedAgreement(ZeroIntergerIFNull(code));

                //
                HtmlImage im = ((HtmlImage)(e.Item.Cells[2].FindControl("imgControl2")));
                string imname = im.ClientID;
                string rowindex = (e.Item.ItemIndex + 1).ToString();
                string rowID = e.Item.ClientID;
                im.Attributes.Add("onclick", ("ControlGrid(\'" + (imname + ("\'," + (rowindex + (",\'" + (rowID + "\')")))))));
                //LinkButton lnk = ((LinkButton)(e.Item.Cells[0].Controls[0]));
                //lnk.Attributes.Add("onclick", "return confirm(\'Are you sure you want to delete this Invoice?\');");

                if (RelatedAgreements != null && RelatedAgreements.Count > 0)
                {
                    Label lblStatus = ((Label)(e.Item.FindControl("lblAgreStatus")));
                    lblStatus.Text = getAgreementType(getBool(e.Item.Cells[9].Text)) + " <i class='icon-arrow-down16' style='margin-top:5px;'></i> " + getAgreementType(getBool(RelatedAgreements[0].isInitial)); ;

                }
                else { //im.Visible = false;

                    Label lblStatus = ((Label)(e.Item.FindControl("lblAgreStatus")));
                    lblStatus.Text = getAgreementType(getBool(e.Item.Cells[9].Text));
                }


                }
            else if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string rowID = e.Item.ClientID;
                string code = e.Item.Cells[3].Text;
                string ItemType = e.Item.Cells[4].Text;
                //SqlDataReader dr = SellMaster.ins.getInvoiceItemsReader(code);

                var objUnitList = objRepository.FillAgreementProcedures(ZeroIntergerIFNull(code));
                if (objUnitList != null)
                {
                    DataGrid grd = ((DataGrid)(e.Item.Cells[1].FindControl("grdProcedures")));
                    if (getBool(e.Item.Cells[9].Text))
                    {
                        grd.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#66BB6A");
                    }
                    else { grd.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF7043");}

                    grd.DataSource = objUnitList;
                    grd.DataBind();

                }


                var RelatedProcedures = objRepository.FillRelatedAgreementProcedures(ZeroIntergerIFNull(code));

                if (RelatedProcedures != null && RelatedProcedures.Count > 0)
                {
                    DataGrid grd2 = ((DataGrid)(e.Item.Cells[1].FindControl("grdProcedures2")));
                    grd2.DataSource = RelatedProcedures;
                    grd2.DataBind();

                    RelatedAgreements = objRepository.getRelatedAgreement(ZeroIntergerIFNull(code));
                    HtmlAnchor relatedlnk = ((HtmlAnchor)(e.Item.Cells[2].FindControl("relatedlnk")));
                    relatedlnk.HRef = "AgreementsData.aspx?id=" + RelatedAgreements[0].Code;
                    Label lblRelatedInitial = ((Label)(e.Item.Cells[1].FindControl("lblRelatedInitial")));
                    lblRelatedInitial.Text = getAgreementType(RelatedAgreements[0].isInitial);


                }
                else
                {
                    ////hide plus
                    //HtmlImage img=((HtmlImage)(e.Item.FindControl("imgControl2")));
                    //img.Visible = false;
                    //img.Attributes.Add("style", "display:none");
                    //e.Item.Cells[2].Controls.Clear();

                    HtmlGenericControl div = ((HtmlGenericControl)(e.Item.Cells[2].FindControl("divRelated")));
                    div.Visible = false;
                }




                //RelatedAgreements = objRepository.getRelatedAgreement(ZeroIntergerIFNull(code));
                //if (RelatedAgreements != null && RelatedAgreements.Count>0)
                //{
                //    DataGrid grd2 = ((DataGrid)(e.Item.Cells[1].FindControl("grdRelatedAgreements")));
                //    grd2.DataSource = RelatedAgreements;
                //    grd2.DataBind();
                //}
                //else {
                //    ////hide plus
                //    //HtmlImage img=((HtmlImage)(e.Item.FindControl("imgControl2")));
                //    //img.Visible = false;
                //    //img.Attributes.Add("style", "display:none");
                //    //e.Item.Cells[2].Controls.Clear();


                //    HtmlGenericControl div =((HtmlGenericControl)(e.Item.Cells[2].FindControl("divRelated")));
                //    div.Visible = false;
                //}









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

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[14].Text == "01/01/1990") e.Item.Cells[14].Text = "";
                if (e.Item.Cells[16].Text == "01/01/1990") e.Item.Cells[16].Text = "";
                if (e.Item.Cells[17].Text == "01/01/1990") e.Item.Cells[17].Text = "";



            }
        }

        protected void grdUnits_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            //if (e.Item.ItemType==ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
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

        protected void lnkAddNewProcedurew_Click(object sender, EventArgs e)
        {
            divAddProcedure.Visible = true;
            divshowProcesure.Visible = false;
            ViewState["ProceduresitemID"] = "0";
            CleaProcesure();
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


                FillAgreementProcedures(ZeroIntergerIFNull(hdnMasterID.Value), hdnIsInitial.Value);
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

        protected void grdData_DeleteCommand(object source, DataGridCommandEventArgs e)
        {

        }

        protected void grdData_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["ProceduresitemID"] = e.Item.Cells[0].Text;
           // divProAttache.Visible = false;
            FillProcedureFrom();

        }

        protected void grdData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }

        protected void lnkSaveProcesure_Click(object sender, EventArgs e)
        {
            SaveProcedureInformation(1);

        }

        protected void lnkCancelProcedure_Click(object sender, EventArgs e)
        {
            divAddProcedure.Visible = false;
            divshowProcesure.Visible = true;
            ViewState["ProceduresitemID"] = "0";
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            hdnactivetab.Value = "3";// Set Agreement Detafult TABl
            FillAgreements();
        }

        protected void lnkSearchback_Click(object sender, EventArgs e)
        {
            tblSearch.Visible = true;
            tblshow.Visible = false;
        }

        protected void btnProcedure_Click(object sender, EventArgs e)
        {
            //divProcedures.Visible = true;
            tblAdd.Visible = false;
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            // this.ClearForm();

            tblSearch.Visible = true;
            tblAdd.Visible = false;
            tblshow.Visible = false;
           // divProcedures.Visible = false;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillAgreements();
        }
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            SaveAgreementMaster();
        }


        protected void grdInboundItems_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                objRepository.DeleteAgreement((AgreementData)objRepository.GetDetails(ZeroIntergerIFNull(e.Item.Cells[3].Text)));
            }
            FillAgreements();
        }

        protected void lnkAddNewAgreement1_Click(object sender, EventArgs e)
        {
            ClearForm();
            tblAdd.Visible = true;
            tblshow.Visible = false;
            tblSearch.Visible = false;
            hdnactivetab.Value = "1";
            hdnIsInitial.Value = "1";
            //FillDllwithoptional_ALL(objLookup.FillAgreementTypes(23), ref lstTypeCode, "NameAr", "Code", "");
            //try
            //{
            //    lstTypeCode.SelectedValue = "23";
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

            if (hdnIsInitial.Value == "1" && ViewState["itemID"].ToString() != "0" )//اتفاقية مبئية
            {
                lnkMOvetoFinal.Visible = true;

            }
            else { lnkMOvetoFinal.Visible = false;  }

            if (hdnIsInitial.Value == "1")
            { showHideinitial(false); }
            else { showHideinitial(true); }

            FillDllwithoptional_ALL(objLookup.FillInitailProcedureTypes(), ref lstProcedureType, "NameAr", "Code", "");



        }

        protected void lnkAddNewAgreement2_Click(object sender, EventArgs e)
        {
            ClearForm();
            tblAdd.Visible = true;
            tblshow.Visible = false;
            tblSearch.Visible = false;
            hdnactivetab.Value = "1";
            hdnIsInitial.Value = "0";

            if (hdnIsInitial.Value == "1" && ViewState["itemID"].ToString() != "0")//اتفاقية مبئية
            { lnkMOvetoFinal.Visible = true; }
            else { lnkMOvetoFinal.Visible = false;  }

            if (hdnIsInitial.Value == "1")
            { showHideinitial(false); }
            else { showHideinitial(true); }

            //FillDllwithoptional_ALL(objLookup.FillAgreementExecludedTypes(23), ref lstTypeCode, "NameAr", "Code", "");
            FillDllwithoptional_ALL(objLookup.FillProcedureTypes(), ref lstProcedureType, "NameAr", "Code", "");
        }

        protected void lstProcedureType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (lstProcedureType.SelectedValue == "3")//الاحالو لجهة الاختصاص
            {
                divRelatedOrg.Visible = true;


            }
            else
            {
                divRelatedOrg.Visible = false;


            }

            if (lstProcedureType.SelectedValue == "4" || lstProcedureType.SelectedValue == "5" || lstProcedureType.SelectedValue == "6")//صدر بمرسوم
            {
                divDessionNum.Visible = true;

            }
            else
            {
                divDessionNum.Visible = false;

            }


            if (lstProcedureType.SelectedValue == "16")//صدر بمرسوم
            {
                divDessionNum4.Visible = true;

            }
            else { divDessionNum4.Visible = false; }

            if (lstProcedureType.SelectedValue != "6" && lstProcedureType.SelectedValue != "16")
            {
                proattchmet.Visible = true;
            }
            else { proattchmet.Visible = false; }

        }

        protected void lnkMOvetoFinal_Click(object sender, EventArgs e)
        {
            int OriginalAgreementID = ZeroIntergerIFNull(hdnMasterID.Value);
            lnkAddNewAgreement2_Click(null, null);
            //Fill Aggreemetn info
            ViewState["parentID"] = OriginalAgreementID;
            var objList = objRepository.FillDetails(OriginalAgreementID);
            if ((objList != null))
            {
                hdnactivetab.Value = "1";
                hdnMasterID.Value = gets(objList.Code);
                anchorAgreementAttachemnt.Visible = true;
                anchorAgreementAttachemnt.HRef = "AgreementMainAttachments.aspx?AgreementCode=" + hdnMasterID.Value;
                txtSerialNUm.Text = gets(objList.Agr_SerialNum);
                txtserialYear.Text = gets(objList.Agr_SerialYear);

                lstTypeCode.SelectedValue = gets(objList.Agr_TypeCode);

                lstorg.SelectedValue = gets(objList.Agr_OrgID);

                lstCats.SelectedValue = gets(objList.Agr_CatID);
                //lstStatus.SelectedValue = gets(objList.StatusCodeID);


                txtCreationDate.Text = NullDateifEmptyToText(objList.Agr_ReciveDate).ToString();

                txtSubject.Text = gets(objList.Agr_Subject);

                txtStartDate.Text = NullDateifEmptyToText(objList.Agr_StartDate);
                txtEndDate.Text = NullDateifEmptyToText(objList.Agr_EndDate);
                txtSignDate.Text = NullDateifEmptyToText(objList.Agr_PublishDate);

                chkIsPrivate.Checked = getBool(objList.isPrivate);
                lnkMOvetoFinal.Visible = false;
                lbltrlatedType2.Text = getAgreementType(false);
                //Hide Related
                cmgsdivFile1.Visible = false;

                txtSubject.Enabled = false;
                txtserialYear.Enabled = false;
                txtSerialNUm.Enabled = false;

                FillAgreementProcedures(0, hdnIsInitial.Value);
            }

            tblAdd.Visible = true;


        }

        protected void lnkSaveProcesure2_Click(object sender, EventArgs e)
        {
            if (hdnRelatedMaster.Value != "" && hdnRelatedMaster.Value != "0")
            {
                SaveProcedureInformation2(ZeroIntergerIFNull(hdnRelatedMaster.Value), 1);
            }

        }

        private int SaveProcedureInformation2(int AgreementCode, int returentype)
        {
            string script = "";
            int _TragetID = 0;
            try
            {
                Agreement_procedureHistory obj = new Agreement_procedureHistory();
                if (ViewState["ProceduresitemID"].Equals("0"))
                {//Save

                    obj.ActionDate = DateTime.Now;

                    obj.Remarks = txtRemarks2.Text;
                    obj.ProcedureDate = NullDateifEmpty(txtProcedureActionDate2.Text);

                    obj.AgreementCode = AgreementCode;
                    obj.ProcedureTypeCode = ZeroIntergerIFNull(lstProcedureType2.SelectedValue);
                    obj.Actionby = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    obj.RelatedOrgID = ZeroIntergerIFNull(lstRelatedOrgs2.SelectedValue);
                    obj.RelatedOrgName = gets(lstRelatedOrgs2.SelectedItem.Text);
                    obj.decisionNum = txtDessionNum2.Text;
                    if (rblPublishType2.SelectedValue != null)
                    {
                        obj.publishType = ZeroIntergerIFNull(rblPublishType2.SelectedValue);
                    }

                    string _imgCMGS = UploadFileoServer(txtcmgsimage2, ScannerRepository + _TargetUploadPath);
                    if (_imgCMGS != "")
                    {
                        obj.CMGSDessionFile = _imgCMGS;
                    }
                    string _imgDessionFile = UploadFileoServer(txtDessionFile2, ScannerRepository + _TargetUploadPath);
                    if (_imgDessionFile != "")
                    {
                        obj.publishDessionFile = _imgDessionFile;
                    }

                    obj.CMGSDessionNum = txtcmgsDession2.Text;


                    objRepository.AddProcedure(obj);
                    _TragetID = obj.Code;
                    ViewState["ProceduresitemID"] = gets(obj.Code);


                    // Add Procesures Attachments
                    Agreement_Attachments objAttachment = new Agreement_Attachments();
                    string _Attchimg = UploadFileoServer(txtImage2, ScannerRepository + _TargetUploadPath);
                    if (_Attchimg != "" || txtRef2.Text != "")
                    {

                        objAttachment.AgreementCode = AgreementCode;

                        objAttachment.ProcedureID = obj.Code;
                        objAttachment.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType2.SelectedValue);
                        objAttachment.ReceiveDate = NullDateifEmpty(txtProcedureActionDate2.Text);

                        objAttachment.UploadDate = DateTime.Now;

                        if (!_Attchimg.Equals(""))
                        {
                            objAttachment.Filepath = _Attchimg;

                        }
                        if (rblPublishType2.SelectedValue != null)
                        {
                            obj.publishType = ZeroIntergerIFNull(rblPublishType2.SelectedValue);
                        }

                        objAttachment.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                        objAttachment.AttacheSubject = txtRemarks2.Text;
                        objAttachment.AttachRef = txtRef2.Text;
                        objRepository.AddAttachement(objAttachment);

                        _TragetID = objAttachment.Code;
                    }

                    if (returentype == 1)
                    {
                        _TragetID = obj.Code;

                    }
                    else { _TragetID = objAttachment.Code; }


                }
                else
                { //Update


                    obj = objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(ViewState["ProceduresitemID"].ToString()));

                    obj.Remarks = txtRemarks2.Text;
                    obj.ProcedureDate = NullDateifEmpty(txtProcedureActionDate2.Text);
                    obj.AgreementCode = AgreementCode;
                    obj.ProcedureTypeCode = ZeroIntergerIFNull(lstProcedureType2.SelectedValue);
                    obj.Actionby = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    obj.RelatedOrgID = ZeroIntergerIFNull(lstRelatedOrgs2.SelectedValue);
                    obj.RelatedOrgName = gets(lstRelatedOrgs2.SelectedItem.Text);
                    obj.decisionNum = txtDessionNum2.Text;

                    string _imgCMGS = UploadFileoServer(txtcmgsimage2, ScannerRepository + _TargetUploadPath);
                    if (_imgCMGS != "")
                    {
                        obj.CMGSDessionFile = _imgCMGS;
                    }
                    obj.CMGSDessionNum = txtcmgsDession2.Text;

                    objRepository.UpdateProcedure(obj);
                    _TragetID = obj.Code;
                    // Add Procesures Attachments
                    Agreement_Attachments objAttachment = new Agreement_Attachments();
                    string _img = UploadFileoServer(txtImage2, ScannerRepository + _TargetUploadPath);
                    if (_img != "" || txtRef2.Text != "")
                    {

                        objAttachment.AgreementCode = AgreementCode;

                        objAttachment.ProcedureID = obj.Code;
                        objAttachment.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType2.SelectedValue);
                        objAttachment.ReceiveDate = NullDateifEmpty(txtProcedureActionDate2.Text);

                        objAttachment.UploadDate = DateTime.Now;

                        if (!_img.Equals(""))
                        {
                            objAttachment.Filepath = _img;

                        }
                        if (rblPublishType2.SelectedValue != null)
                        {
                            obj.publishType = ZeroIntergerIFNull(rblPublishType2.SelectedValue);
                        }

                        objAttachment.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                        objAttachment.AttacheSubject = txtRemarks2.Text;
                        objAttachment.AttachRef = txtRef2.Text;
                        objRepository.AddAttachement(objAttachment);

                        _TragetID = objAttachment.Code;
                    }
                    if (returentype == 1)
                    {
                        _TragetID = obj.Code;

                    }
                    else { _TragetID = objAttachment.Code; }

                }


                UpdateAgreementLastProcedure();

                ViewState["ProceduresitemID"] = obj.Code;

              //  CleaProcesure();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            FillAgreementProcedures(ZeroIntergerIFNull(hdnMasterID.Value), hdnIsInitial.Value);
            return _TragetID;
        }

        protected void lnkCancelProcedure2_Click(object sender, EventArgs e)
        {
            divAddProcedure2.Visible = false;
            divshowProcesure2.Visible = true;
            ViewState["ProceduresitemID"] = "0";
        }

        protected void lnkScan2_Click(object sender, EventArgs e)
        {
            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }


            int _TargetID = SaveProcedureInformation2(ZeroIntergerIFNull(hdnRelatedMaster.Value), 2);

            if (_TargetID != 0)
            {
                _TargetUploadPath = _TargetUploadPath + ZeroIntergerIFNull(hdnRelatedMaster.Value) + "/" + gets(ViewState["ProceduresitemID"]) + "/";

                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='1' />";
                ScannerPostFrom += "<input type='hidden' id='id' name='id' value='" + ViewState["itemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='attachetype' name='attachetype' value='1' />";
                ScannerPostFrom += "<input type='hidden' id='profiletype' name='profiletype' value='1' />";

                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='3' />";
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

        protected void lstProcedureType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstProcedureType2.SelectedValue == "3")//الاحالو لجهة الاختصاص
            {
                divRelatedOrg2.Visible = true;
            }
            else { divRelatedOrg2.Visible = false; }

            if (lstProcedureType2.SelectedValue == "4" || lstProcedureType2.SelectedValue == "5" || lstProcedureType2.SelectedValue == "6")//صدر بمرسوم
            {
                divDessionNum2.Visible = true;
            }
            else { divDessionNum2.Visible = false; }


            if (lstProcedureType2.SelectedValue == "16")//صدر بمرسوم
            {
                divDessionNum3.Visible = true;
            }
            else { divDessionNum3.Visible = false; }
            if (lstProcedureType2.SelectedValue != "6" && lstProcedureType2.SelectedValue != "16")
            {
                proAttachment2.Visible = true;
            }
            else { proAttachment2.Visible = false; }
        }

        protected void grdProcedure2_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["ProceduresitemID"] = e.Item.Cells[0].Text;
            // divProAttache2.Visible = false;
            FillProcedureFrom2();
        }

        protected void btnScan2_Click(object sender, EventArgs e)
        {
            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }

            //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";

            int _TargetID = SaveProcedureInformation(2);

            if (_TargetID != 0)
            {

                _TargetUploadPath = _TargetUploadPath + ZeroIntergerIFNull(hdnMasterID.Value) + "/" + gets( ViewState["ProceduresitemID"] )+ "/";

                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='1' />";
                ScannerPostFrom += "<input type='hidden' id='id' name='id' value='" + ViewState["itemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='profiletype' name='profiletype' value='1' />";
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

        protected void lnkInitailScan_Click(object sender, EventArgs e)
        {

        }

        protected void lnkRelatedCMGS_Click(object sender, EventArgs e)
        {

        }

        protected void lnkScanCMgs_Click(object sender, EventArgs e)
        {
            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }

            //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";

            int _TargetID = SaveProcedureInformation(1);

            if (_TargetID != 0)
            {

                _TargetUploadPath = _TargetUploadPath + ZeroIntergerIFNull(hdnMasterID.Value) + "/" + _TargetID.ToString() + "/";

                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnCMGSScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnCMGSScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='id' name='id' value='" + ViewState["itemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='profiletype' name='profiletype' value='2' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='2' />";
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

        protected void btnCMGSScan2_Click(object sender, EventArgs e)
        {
            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }


            int _TargetID = SaveProcedureInformation2(ZeroIntergerIFNull(hdnRelatedMaster.Value), 1);

            if (_TargetID != 0)
            {
                _TargetUploadPath = _TargetUploadPath + ZeroIntergerIFNull(hdnRelatedMaster.Value) + "/" + _TargetID.ToString() + "/";
                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnCMGS2Scannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnCMGS2Scannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='id' name='id' value='" + ViewState["itemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='attachetype' name='attachetype' value='2' />";
                ScannerPostFrom += "<input type='hidden' id='profiletype' name='profiletype' value='2' />";

                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='3' />";
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

        protected void btnScanDession_Click(object sender, EventArgs e)
        {
            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }

            //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";

            int _TargetID = SaveProcedureInformation(1);

            if (_TargetID != 0)
            {
                _TargetUploadPath = _TargetUploadPath + ZeroIntergerIFNull(hdnRelatedMaster.Value) + "/" + _TargetID.ToString() + "/";

                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnpublishScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnpublishScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='id' name='id' value='" + ViewState["itemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='profiletype' name='profiletype' value='3' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='2' />";
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

        protected void lnkDessionScan2_Click(object sender, EventArgs e)
        {
            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }


            int _TargetID = SaveProcedureInformation2(ZeroIntergerIFNull(hdnRelatedMaster.Value), 1);

            if (_TargetID != 0)
            {
                _TargetUploadPath = _TargetUploadPath + ZeroIntergerIFNull(hdnRelatedMaster.Value) + "/" + _TargetID.ToString() + "/";

                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnpublish2Scannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnpublish2Scannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='id' name='id' value='" + ViewState["itemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='attachetype' value='3' />";
                ScannerPostFrom += "<input type='hidden' id='profiletype' name='profiletype' value='3' />";

                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='3' />";
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

        protected void lstFilterProcedures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFilterProcedures.SelectedValue == "3")//الاحالو لجهة الاختصاص
            {
                divFilterRelatedOrg.Visible = true;
            }
            else
            {
                divFilterRelatedOrg.Visible = false;
                try
                {
                    lstFilterRelatedOrgs.SelectedValue = "0";
                }
                catch (Exception)
                {


                }

            }
        }
        protected void lnklaeUnlink_Click(object sender, EventArgs e)
        {
            AgreementData obj = new AgreementData();

            obj = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if (obj != null)
            {
                obj.Law_DocDataRefId = 0;
                objRepository.UpdateAgreement(obj);

                string script = FormatpopupErrorMSG("تم فك الارتباط بنجاح", "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                //FillAgreementMasterInformation();

                lnklawDoc.InnerHtml = "<b><i class='glyphicon glyphicon-link'></i></b>إضافة قانون مرتبط";
                lnklawDoc.HRef = "/modules/laws/forms/LawDocDatalnk.aspx?agreementid=" + gets(obj.Code) + "&lawDocRefId=0";
                lnklaeUnlink.Visible = false;

            }


        }

        //}
        #endregion

        #region "Fill Information"
        public Boolean showContainer()
        { 
            return false;   
        }
        public string viewlinkedfile(string filename)
        {
            return gets(filename).Equals("") || gets(filename).Equals("0") ? "none" : "";
        }


        private string  MapSearchKeys() {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                _keyList.Add("مسلسل", txtFilterNum.Text);
                _keyList.Add("السنة", txtFilterYear.Text);
                _keyList.Add("تاريخ الاتفاقية من ", txtFilterDatefrom.Text);
                _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                _keyList.Add("نوع الاتفاقية", lstFilterType.SelectedItem.Text);
                _keyList.Add("تصنيف الاتفاقية", lstFilterCats.SelectedItem.Text);
                _keyList.Add("الجهة/الدولة  ", lstFilterOrg.SelectedItem.Text);
                _keyList.Add("الإجراء ", lstFilterProcedures.SelectedItem.Text);
                _keyList.Add(" جزء من موضوع الاتفاقية ", txtPartofName.Text);
                _keyList.Add("حالة الاتفاقية ", lstFilterStatus.SelectedItem.Text);
                _keyList.Add("محال الي جهة الإختصاص   ", lstFilterRelatedOrgs.SelectedItem.Text);
                _keyList.Add("  جهة الإختصاص", lstFilterAgrRelatedOrgs.SelectedItem.Text);
            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }
        private void FillAgreements(List<int> notInList=null)
        {
            if (notInList==null)
            {
                notInList = new List<int>();
            }
            //   Session["ViewPrivate"]
            hdnactivetab.Value = "3";
            var objList = objRepository.GetList(ZeroIntergerIFNull(txtFilterNum.Text),
                ZeroIntergerIFNull(txtFilterYear.Text), NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterType.SelectedValue),
                ZeroIntergerIFNull(lstFilterCats.SelectedValue), ZeroIntergerIFNull(lstFilterOrg.SelectedValue),
               0, ZeroIntergerIFNull(lstFilterProcedures.SelectedValue),
                txtPartofName.Text, getBool(ReadSession("ViewPrivate")), notInList,
                lstFilterStatus.SelectedValue,ZeroIntergerIFNull(lstFilterRelatedOrgs.SelectedValue),
                ZeroIntergerIFNull(lstFilterAgrRelatedOrgs.SelectedValue),MapSearchKeys(),ZeroIntergerIFNull(lstFilterAssignedPerson.SelectedValue));



            var InitailAgreements = objList.Where(c =>  c.ParentID == 0).SelectMany(t =>
               Enumerable.Repeat(t, 2)).ToList();

            //var InitailAgreements = objList.SelectMany(t =>
            //  Enumerable.Repeat(t, 2)).ToList();


            // var FinalAgreements = objList.Where(c => c.isInitial == false).SelectMany(t =>
            //Enumerable.Repeat(t, 2)).ToList();



            if (objList.Count > 0)
            {


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
          


            grdInboundItems.DataSource = InitailAgreements;
            grdInboundItems.DataBind();

            //grdAgreement2.DataSource = FinalAgreements;
            //grdAgreement2.DataBind();

            pager1.ItemCount = InitailAgreements.Count ;
           // pager3.ItemCount = FinalAgreements.Count  ;


            lblcount.Text = (Resources.Utilities.foundTotal + ((InitailAgreements.Count/2).ToString() + Resources.Utilities.records));
            lblcount2.Text = (Resources.Utilities.foundTotal + ((InitailAgreements.Count/2).ToString() + Resources.Utilities.records));


          //  lblInitialCount.Text = (InitailAgreements.Count / 2).ToString();


            //lblFinalCount2.Text = (Resources.Utilities.foundTotal + ((FinalAgreements.Count / 2).ToString() + Resources.Utilities.records));
            //lblFinalCount.Text =(FinalAgreements.Count / 2).ToString();

        }

        private void FillCurrentAgreements()
        {


            //   Session["ViewPrivate"]
            var objList = objRepository.GetCurrentAgreementList();
            lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));


            var duplicatedList = objList.SelectMany(t =>
           Enumerable.Repeat(t, 2)).ToList();

            //decimal c = System.Math.Ceiling(Convert.ToDecimal(duplicatedList.Count / grdInboundItems.PageSize));
            //if ((c <= grdInboundItems.CurrentPageIndex))
            //{
            //    grdInboundItems.CurrentPageIndex = 0;
            //}

            //List Duplication
            //List<View_InboundItems> duplicatedList = new List<View_InboundItems>();
            //duplicatedList = DuplicatedList(objList);

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
            decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdInboundItems.PageSize));
            if ((c <= grdInboundItems.CurrentPageIndex))
            {
                grdInboundItems.CurrentPageIndex = 0;
            }



            grdInboundItems.DataSource = duplicatedList;
            grdInboundItems.DataBind();
           
            pager1.ItemCount = duplicatedList.Count;

        }
        private void FillAgreementProcedures(int AgreementID,string Agreementtype )
        {
            var objList = objRepository.FillAgreementProcedures(AgreementID);
            lblProcerduresCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() +  Resources.Utilities.records));
            lblProcerduresCount2.Text = objList.Count.ToString();

            decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdProcedure.PageSize));
            if ((c <= grdProcedure.CurrentPageIndex))
            {
                grdProcedure.CurrentPageIndex = 0;
            }

            if (objList.Count > 0)
            {
                //btnSave.Visible = true;
                //lnkBack.Visible = true;




                divshowProcesure.Visible = true;
                pager2.Visible = true;
                divAddProcedure.Visible = false;

            }
            else
            {
            }

            if (getBool(Agreementtype))
            {
                grdProcedure.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#66BB6A");
            }
            else { grdProcedure.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF7043"); }



            grdProcedure.DataSource = objList;
            grdProcedure.DataBind();
            pager2.ItemCount = objList.Count;



            //Fill Related Procedures

            var objListRelated = objRepository.FillRelatedAgreementProcedures(AgreementID);
            if (objListRelated != null && objListRelated.Count > 0)
            {
                //hdnRelatedMaster.Value = objListRelated[0].AgreementCode.ToString();
                grdProcedure2.Visible = true;
                grdProcedure2.DataSource = objListRelated;
                grdProcedure2.DataBind();
                lblProcerdures2Count.Text = objListRelated.Count.ToString();

            }
            else { //hdnRelatedMaster.Value = "0";
            }



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

        private void SetPageTitle()
        {
            if (Request.QueryString["d"] !=null)
            {
               // lblSubTitle.Text = "Deposit Goods";

            }


        }

        private void ClearForm()
        {


            ViewState["itemID"] = "0";
            txtSerialNUm.Text = "";
            txtserialYear.Text = "";
            txtSubject.Text = "";
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            txtCreationDate.Text = "";

            lstorg.SelectedValue = "0";
            lstTypeCode.SelectedValue = "0";
            lstCats.SelectedValue = "0";
           // lstStatus.SelectedValue = "0";
            hdnactivetab.Value ="0";

            ViewState["parentID"] = "0";


            //BlblSubTitle.Text = this.GetTitle(true);
        }

        protected void pager_Command(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdInboundItems.PageCount))
            {
                currnetPageIndx = (grdInboundItems.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdInboundItems.CurrentPageIndex = (currnetPageIndx - 1);


            if (Request.QueryString["current"] != null)
            {
                FillCurrentAgreements();

            }
            else
            { FillAgreements(); }


        }
        //protected void pager_Command2(object sender, CommandEventArgs e)
        //{
        //    Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
        //    if ((currnetPageIndx <= 0))
        //    {
        //        currnetPageIndx = 1;
        //    }

        //    if ((currnetPageIndx > grdAgreement2.PageCount))
        //    {
        //        currnetPageIndx = (grdAgreement2.PageCount - 1);
        //    }

        //    pager3.CurrentIndex = currnetPageIndx;
        //    grdAgreement2.CurrentPageIndex = (currnetPageIndx - 1);


        //    if (Request.QueryString["current"] != null)
        //    {
        //        FillCurrentAgreements();

        //    }
        //    else
        //    { FillAgreements(); }

        //    hdnactivetab.Value = "4";

        //}


        private void applyUserPermission()
        {
            
            divAddControl.Visible = userAccess.Add;
            lnkAddNewProcedurew.Visible = userAccess.Add;
            lnkAddProcedure2.Visible = userAccess.Add;

            grdInboundItems.Columns[22].Visible = userAccess.Delete;
            lnkDeleteProcedure.Visible = userAccess.Delete;
            lnkDeleteProcedure2.Visible = userAccess.Delete;


            //btnSave.Visible = userAccess.Edit ||  userAccess.Add;
            //lnklaeUnlink.Visible = userAccess.Edit ||  userAccess.Add;
                
            //lnkSaveProcesure.Visible = userAccess.Edit ||  userAccess.Add;
            //lnkSaveProcesure2.Visible = userAccess.Edit ||  userAccess.Add;
            if (Request.QueryString["editflag"] != null)
            {
                string editflag = Request.QueryString["editflag"].ToString();

                btnSave.Visible = userAccess.Edit;

                lnkSaveProcesure.Visible = userAccess.Edit;
                lnkSaveProcesure2.Visible = userAccess.Edit;
            }
            else
            {

                btnSave.Visible = userAccess.Edit || userAccess.Add;
                lnkSaveProcesure.Visible = userAccess.Edit || userAccess.Add;
                lnkSaveProcesure2.Visible = userAccess.Edit || userAccess.Add;
            }


        }


        #endregion

        #region "Helper Methods"
        public string showattachment(string hasattachment)
        {
            if (hasattachment != "")
            {
                return "";
            }
            return "display:none";

        }
        private void showHideinitial(bool show)
        {
            initial1.Visible = show;
            initial2.Visible = show;
            initial3.Visible = show;
            initial4.Visible = show;
        }

        public string showRelatedFiles()
        {
            string _out = "none";
            if (ZeroIntergerIFNull(hdnMasterID.Value) != 0)
            {
                var objRelatedAgreement = objRepository.getRelatedAgreement(ZeroIntergerIFNull(hdnMasterID.Value));
                if (objRelatedAgreement != null && objRelatedAgreement.Count > 0)
                {
                    lnkMOvetoFinal.Visible = false;
                    lbltrlatedType.Text = getAgreementType(objRelatedAgreement[0].isInitial);
                    lbltrlatedType2.Text = getAgreementType(objRelatedAgreement[0].isInitial);
                    hdnRelatedMaster.Value = objRelatedAgreement[0].Code.ToString();
                    _out = "";
                    //show Related Informatrion

                    txtStartDate.Text = NullDateifEmptyToText(objRelatedAgreement[0].Agr_StartDate);
                    txtEndDate.Text = NullDateifEmptyToText(objRelatedAgreement[0].Agr_EndDate);
                    txtSignDate.Text = NullDateifEmptyToText(objRelatedAgreement[0].Agr_PublishDate);

                    if (getBool(objRelatedAgreement[0].isInitial))
                    {
                        showHideinitial(false);
                        FillDllwithoptional_ALL(objLookup.FillInitailProcedureTypes(), ref lstProcedureType2, "NameAr", "Code", "");


                    }
                    else
                    {
                        showHideinitial(true);
                        FillDllwithoptional_ALL(objLookup.FillProcedureTypes(), ref lstProcedureType2, "NameAr", "Code", "");
                    }



                }
                else
                { lbltrlatedType.Text = ""; }
            }


            return _out;
        }

        public string showRelatedFilestab()
        {
            string _out = "none";
            if (ZeroIntergerIFNull(hdnMasterID.Value) != 0)
            {
                var objRelatedAgreement = objRepository.getRelatedAgreement(ZeroIntergerIFNull(hdnMasterID.Value));
                if (objRelatedAgreement != null && objRelatedAgreement.Count > 0)
                {

                    _out = "";

                }
                else
                { lbltrlatedType.Text = ""; }
            }


            return _out;
        }
        public string getPublishType(string publishType)
        {
            string _out = "";
            if (publishType =="1")
            {
                _out = "صدر بقانون";
            }
            else if (publishType=="2")
            {
                _out = "صدر بمرسوم";
            }


            return _out;

        }

        public string ViewDiv(string inputstr)
        {
            string _out = "";
            if (inputstr == "")
            {
                _out = "display:none";
            }
            else if (inputstr != "")
            {
                _out = "display:";
            }


            return _out;

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

        private void FillAgreementMasterInformation()
        {

            var objList = objRepository.FillDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {
                hdnactivetab.Value = "1";

                anchorAgreementAttachemnt.Visible = true;
                anchorAgreementAttachemnt.HRef = "AgreementMainAttachments.aspx?AgreementCode=" + gets(objList.Code);
                txtSerialNUm.Text = gets(objList.Agr_SerialNum);
                txtserialYear.Text = gets(objList.Agr_SerialYear);



                lstorg.SelectedValue = gets(objList.Agr_OrgID);
                lstTypeCode.SelectedValue = gets(objList.Agr_TypeCode);
                lstCats.SelectedValue = gets(objList.Agr_CatID);
               // lstStatus.SelectedValue = gets(objList.StatusCodeID);

                lstAgrRelatedOrgs.SelectedValue = gets(objList.Agr_RelatedOrgId);


                txtCreationDate.Text = NullDateifEmptyToText(objList.Agr_ReciveDate).ToString();

                txtSubject.Text = gets(objList.Agr_Subject);

                txtStartDate.Text = NullDateifEmptyToText(objList.Agr_StartDate);
                txtEndDate.Text = NullDateifEmptyToText(objList.Agr_EndDate);
                txtSignDate.Text = NullDateifEmptyToText(objList.Agr_PublishDate);

               chkIsPrivate.Checked = getBool( objList.isPrivate);
                hdnIsInitial.Value = getBit(objList.isInitial);
                if (hdnIsInitial.Value=="1" && ViewState["itemID"].ToString() !="0")//اتفاقية مبئية
                { lnkMOvetoFinal.Visible = true; }
                else { lnkMOvetoFinal.Visible = false;  }

                if (hdnIsInitial.Value == "1")
                { showHideinitial(false);
                    FillDllwithoptional_ALL(objLookup.FillInitailProcedureTypes(), ref lstProcedureType, "NameAr", "Code", "");


                } else {
                    FillDllwithoptional_ALL(objLookup.FillProcedureTypes(), ref lstProcedureType, "NameAr", "Code", "");
                    showHideinitial(true); }

                //var objRelatedAgreement = objRepository.getRelatedAgreement(objList.Code);
                //if (objRelatedAgreement!=null && objRelatedAgreement.Count>0)
                //{
                //    hdnRelatedMaster.Value = objRelatedAgreement[0].Code.ToString();
                //}


                hdnMasterID.Value = gets(objList.Code);
                hdnIsInitial.Value = gets(objList.isInitial);
                showRelatedFiles();

                //Control Lnk
                if (objList.Law_DocDataRefId !=null && objList.Law_DocDataRefId != 0 && gets(objList.Law_DocDataRefId )!= "")
                {
                    lnklawDoc.InnerHtml = "<b><i class='glyphicon glyphicon-link'></i></b>عرض قانون مرتبط";
                    lnklawDoc.HRef = "/modules/laws/forms/LawDocDatalnk.aspx?agreementid=" + gets(objList.Code) + "&lawDocRefId=" + objList.Law_DocDataRefId;
                    lnklaeUnlink.Visible = true;
                }
                else
                {
                    lnklawDoc.InnerHtml = "<b><i class='glyphicon glyphicon-link'></i></b>إضافة قانون مرتبط";
                    lnklawDoc.HRef = "/modules/laws/forms/LawDocDatalnk.aspx?agreementid=" + gets(objList.Code) + "&lawDocRefId=0";
                    lnklaeUnlink.Visible = false;


                }
                try
                {
                    lstassignedPersons.SelectedValue = gets(objList.assignedPersonID);
                }
                catch (Exception)
                {

                    throw;
                }

            }

            tblAdd.Visible = true;
           //blSubTitle.Text = this.GetTitle(false);

        }
        private void SaveAgreementMaster()
        {

            string script = "";
            try
            {
                AgreementData obj = new AgreementData();
                if (ViewState["itemID"].Equals("0"))
                {//Save


                    if (objRepository.CheckAgreementExistance(ZeroIntergerIFNull(txtSerialNUm.Text), ZeroIntergerIFNull(txtserialYear.Text), getBool(hdnIsInitial.Value), 0))
                    {

                        script = FormatpopupErrorMSG("مسلسل الاتفاقيه مسجل من قبل [مسلسل / سنة]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return;
                    }


                    obj.TransDate = DateTime.Now;

                    obj.Agr_Serial =    txtserialYear.Text+ "/" + txtSerialNUm.Text ;

                    obj.Agr_SerialNum = ZeroIntergerIFNull(txtSerialNUm.Text);
                    obj.Agr_SerialYear = ZeroIntergerIFNull(txtserialYear.Text);

                    //if (!txtSerial.Text.Equals(""))
                    //{
                    //    string[] fn = txtSerial.Text.Split('/');
                    //    if (fn.Length > 0)
                    //    {
                    //        obj.Agr_SerialNum = ZeroIntergerIFNull(fn[0]);
                    //        obj.Agr_SerialYear  = ZeroIntergerIFNull(fn[1]);
                    //    }
                    //}

                    obj.Agr_ReciveDate = NullDateifEmpty(txtCreationDate.Text);
                    obj.Agr_PublishDate = NullDateifEmpty(txtSignDate.Text);


                    obj.Agr_OrgID = ZeroIntergerIFNull(lstorg.SelectedValue);
                    obj.Agr_TypeCode = ZeroIntergerIFNull(lstTypeCode.SelectedValue);
                   // obj.StatusCodeID = ZeroIntergerIFNull(lstStatus.SelectedValue);
                    obj.Agr_CatID = ZeroIntergerIFNull(lstCats.SelectedValue);

                    obj.Agr_RelatedOrgId = ZeroIntergerIFNull(lstAgrRelatedOrgs.SelectedValue);


                    obj.Agr_Subject = txtSubject.Text;
                    obj.Agr_SubjectEn = txtSubject.Text;

                    obj.Agr_StartDate = NullDateifEmpty(txtStartDate.Text);
                    obj.Agr_EndDate = NullDateifEmpty(txtEndDate.Text);

                    obj.isPrivate = getBool(chkIsPrivate.Checked);

                    // obj.StatusCodeID = 0;

                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                    obj.isInitial = getBool(hdnIsInitial.Value);


                    obj.ParentID =ZeroIntergerIFNull(ViewState["parentID"].ToString());

                    obj.assignedPersonID = ZeroIntergerIFNull(lstassignedPersons.SelectedValue);

                    objRepository.AddAgreement(obj);
                    hdnMasterID.Value = gets(obj.Code);
                    ViewState["itemID"] = gets(obj.Code);

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
                   // FillAgreementMasterInformation();

                }
                else
                { //Update


                    if (objRepository.CheckAgreementExistance(ZeroIntergerIFNull(txtSerialNUm.Text), ZeroIntergerIFNull(txtserialYear.Text), getBool(hdnIsInitial.Value), ZeroIntergerIFNull(ViewState["itemID"].ToString())))
                    {
                        script = FormatpopupErrorMSG("مسلسل الاتفاقيه مسجل من قبل [مسلسل / سنة]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return;
                    }


                    hdnMasterID.Value = ViewState["itemID"].ToString();
                    obj = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    obj.LastUpdate = DateTime.Now;

                    //obj.Agr_Serial = txtSerial.Text;

                    //if (!txtSerial.Text.Equals(""))
                    //{
                    //    string[] fn = txtSerial.Text.Split('/');
                    //    if (fn.Length > 0)
                    //    {
                    //        obj.Agr_SerialNum = ZeroIntergerIFNull(fn[0]);
                    //        obj.Agr_SerialYear = ZeroIntergerIFNull(fn[1]);
                    //    }
                    //}

                    obj.Agr_Serial = txtserialYear.Text + "/" + txtSerialNUm.Text;

                    obj.Agr_SerialNum = ZeroIntergerIFNull(txtSerialNUm.Text);
                    obj.Agr_SerialYear = ZeroIntergerIFNull(txtserialYear.Text);

                    obj.Agr_ReciveDate = NullDateifEmpty(txtCreationDate.Text);
                    obj.Agr_PublishDate = NullDateifEmpty(txtSignDate.Text);


                    obj.Agr_OrgID = ZeroIntergerIFNull(lstorg.SelectedValue);
                    obj.Agr_TypeCode = ZeroIntergerIFNull(lstTypeCode.SelectedValue);
                    obj.Agr_CatID = ZeroIntergerIFNull(lstCats.SelectedValue);
                    // obj.StatusCodeID = ZeroIntergerIFNull(lstStatus.SelectedValue);


                    obj.Agr_RelatedOrgId = ZeroIntergerIFNull(lstAgrRelatedOrgs.SelectedValue);


                    obj.Agr_Subject = txtSubject.Text;
                    obj.Agr_SubjectEn = txtSubject.Text;

                    obj.Agr_StartDate = NullDateifEmpty(txtStartDate.Text);
                    obj.Agr_EndDate = NullDateifEmpty(txtEndDate.Text);

                    obj.isPrivate = getBool(chkIsPrivate.Checked);
                    obj.assignedPersonID = ZeroIntergerIFNull(lstassignedPersons.SelectedValue);


                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    objRepository.UpdateAgreement(obj);

                }

                ViewState["itemID"] = obj.Code;
                anchorAgreementAttachemnt.Visible = true;
                anchorAgreementAttachemnt.HRef = "AgreementMainAttachments.aspx?AgreementCode=" + hdnMasterID.Value;
                //  ClearForm();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                if (gets(ViewState["parentID"]) != "0")
                {
                    Response.Redirect("AgreementsData.aspx?ss=1&id=" + gets(ViewState["parentID"]));
                }
                else { Response.Redirect("AgreementsData.aspx?ss=1&id=" + hdnMasterID.Value); }


            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

        }
        private void fillLookups()
        {

            FillDllwithoptional_ALL(objLookup.FillAgreementTypes(), ref lstTypeCode, "NameAr", "Code", "");
            FillDllwithoptional_ALL(objLookup.FillAgreementTypes(), ref lstFilterType, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillAgreementCategories(), ref lstCats, "NameAr", "Code", "");
            FillDllwithoptional_ALL(objLookup.FillAgreementCategories(), ref lstFilterCats, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillOrganization(), ref lstorg, "NameAr", "Code", "");
            FillDllwithoptional_ALL(objLookup.FillOrganization(), ref lstFilterOrg, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillProcedureTypes(), ref lstFilterProcedures, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillInitailProcedureTypes(), ref lstProcedureType, "NameAr", "Code", "");
            FillDllwithoptional_ALL(objLookup.FillProcedureTypes(), ref lstProcedureType2, "NameAr", "Code", "");

            //FillDllwithoptional_ALL(objLookup.FillStatusCode(), ref lstFilterStatusCode, "NameAr", "Code", "الكل");
            //FillDllwithoptional_ALL(objLookup.FillStatusCode(), ref lstStatus, "NameAr", "Code", "");

            FillDllwithoptional_ALL(objLookup.FillAttachmentTypes(), ref lstAttachmentType, "NameAr", "Code", "");
            FillDllwithoptional_ALL(objLookup.FillAttachmentTypes(), ref lstAttachmentType2, "NameAr", "Code", "");

            FillDllwithoptional_ALL(objLookup.FillAgreementRelatedOrgs(), ref lstRelatedOrgs, "NameAr", "Code", "");
            FillDllwithoptional_ALL(objLookup.FillAgreementRelatedOrgs(), ref lstRelatedOrgs2, "NameAr", "Code", "");

            FillDllwithoptional_ALL(objLookup.FillAgreementRelatedOrgs(), ref lstAgrRelatedOrgs, "NameAr", "Code", "");
            FillDllwithoptional_ALL(objLookup.FillAgreementRelatedOrgs(), ref lstFilterAgrRelatedOrgs, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillAgreementRelatedOrgs(), ref lstFilterRelatedOrgs, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillAgreementsAssignedPersons(), ref lstFilterAssignedPerson, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillAgreementsAssignedPersons(), ref lstassignedPersons, "NameAr", "Code", "");



        }

        private void UpdateAgreementLastProcedure()
        {
            try
            {
                // Update Agreement With Last procedure Based On Date

                var  LastProcedurepbj = objRepository.GetAgreementLastProcedureobj(ZeroIntergerIFNull(hdnMasterID.Value));
                if (LastProcedurepbj != null)
                {
                    AgreementData objAgreementData = new AgreementData();
                    objAgreementData = objRepository.GetDetails(ZeroIntergerIFNull(hdnMasterID.Value));
                    objAgreementData.LastActionID = LastProcedurepbj.ProcedureTypeCode;
                    objAgreementData.LastProcedureRefId = LastProcedurepbj.Code;

                    objRepository.UpdateAgreement(objAgreementData);
                }
                else {

                    AgreementData objAgreementData = new AgreementData();
                    objAgreementData = objRepository.GetDetails(ZeroIntergerIFNull(hdnMasterID.Value));
                    objAgreementData.LastActionID =0;
                    objAgreementData.LastProcedureRefId = 0;
                    objRepository.UpdateAgreement(objAgreementData);
                }

            }
            catch (Exception)
            {

                throw;
            }


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
        public string getAgreementProcedureHeader(Boolean isInitial)
        {

            string _out = "";
            switch (isInitial)
            {
                case true:
                    {
                        _out = "#669999";
                        break;
                    }
                case false:
                    {
                        _out = "#669999";
                        break;
                    }

            }
            return _out;


        }

        private int SaveProcedureInformation(int scanType)
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
                    obj.RelatedOrgName = gets(lstRelatedOrgs.SelectedItem.Text);
                    obj.decisionNum = txtDessionNum.Text;
                    if (rblPublishType.SelectedValue!=null)
                    {
                        obj.publishType = ZeroIntergerIFNull(rblPublishType.SelectedValue);
                    }

                    string _imgCMGS = UploadFileoServer(txtimage1, ScannerRepository + _TargetUploadPath);
                    if (_imgCMGS != "")
                    {
                        obj.CMGSDessionFile = _imgCMGS;
                    }

                    string _imgDessionFile = UploadFileoServer(txtDessionFile, ScannerRepository + _TargetUploadPath);
                    if (_imgDessionFile != "")
                    {
                        obj.publishDessionFile = _imgDessionFile;
                    }

                    obj.CMGSDessionNum = txtcmgsDession.Text;

                        objRepository.AddProcedure(obj);

                    ViewState["ProceduresitemID"] = gets(obj.Code);

                   


                    // Add Procesures Attachments
                    Agreement_Attachments objAttachment = new Agreement_Attachments();
                    string _img = UploadFileoServer(txtImage, ScannerRepository + _TargetUploadPath );
                    if (_img != "" || txtRef.Text !="")
                    {

                        objAttachment.AgreementCode = ZeroIntergerIFNull(hdnMasterID.Value);

                        objAttachment.ProcedureID = obj.Code;
                        objAttachment.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                        objAttachment.ReceiveDate = NullDateifEmpty(txtProcedureActionDate.Text);

                        objAttachment.UploadDate = DateTime.Now;

                        if (!_img.Equals(""))
                        {
                            objAttachment.Filepath = _img;

                        }
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
                    if (scanType == 1)
                    {
                        _TragetID = obj.Code;

                    }
                    else { _TragetID = objAttachment.Code; }



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
                    obj.RelatedOrgName = gets(lstRelatedOrgs.SelectedItem.Text);
                    obj.decisionNum = txtDessionNum.Text;

                    string _imgCMGS = UploadFileoServer(txtimage1, ScannerRepository + _TargetUploadPath);
                    if (_imgCMGS != "")
                    {
                        obj.CMGSDessionFile = _imgCMGS;
                    }
                    if (rblPublishType.SelectedValue != null)
                    {
                        obj.publishType = ZeroIntergerIFNull(rblPublishType.SelectedValue);
                    }
                    obj.CMGSDessionNum = txtcmgsDession.Text;

                    objRepository.UpdateProcedure(obj);
                    _TragetID = obj.Code;

                    // Update Agreement With last procesdure Taken
                    //var LastProcesdureTaken = objRepository.FillAgreementProcedures(ZeroIntergerIFNull(hdnMasterID.Value)).First();
                    //if (LastProcesdureTaken != null)
                    //{
                    //    UpdateAgreementLastProcedure(LastProcesdureTaken.ProcedureTypeCode.Value);
                    //}
                    //else { UpdateAgreementLastProcedure(0); }


                    // Add Procesures Attachments
                    Agreement_Attachments objAttachment = new Agreement_Attachments();
                    string _img = UploadFileoServer(txtImage, ScannerRepository + _TargetUploadPath );
                    if (_img != "" || txtRef.Text != "")
                    {
                        objAttachment.AgreementCode = ZeroIntergerIFNull(hdnMasterID.Value);

                        objAttachment.ProcedureID = obj.Code;
                        objAttachment.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                        objAttachment.ReceiveDate = NullDateifEmpty(txtProcedureActionDate.Text);

                        objAttachment.UploadDate = DateTime.Now;

                        if (!_img.Equals(""))
                        {
                            objAttachment.Filepath = _img;

                        }
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


                    if (scanType == 1)
                    {
                        _TragetID = obj.Code;

                    }
                    else { _TragetID = objAttachment.Code; }

                }

                UpdateAgreementLastProcedure();

                ViewState["ProceduresitemID"] = obj.Code;

                //CleaProcesure();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            FillAgreementProcedures(ZeroIntergerIFNull(hdnMasterID.Value), hdnIsInitial.Value);
            return _TragetID;
        }

        #endregion

        #region "Procedure Methods"
        private void FillProcedureFrom2()
        {

            var objList = objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(ViewState["ProceduresitemID"].ToString()));
            if ((objList != null))
            {


                txtRemarks2.Text = gets(objList.Remarks);

                lstProcedureType2.SelectedValue = gets(objList.ProcedureTypeCode);
                lstProcedureType2_SelectedIndexChanged(null, null);
                txtProcedureActionDate2.Text = NullDateifEmptyToText(objList.ProcedureDate);
                if (objList.RelatedOrgID != 0 && objList.RelatedOrgID != null)
                {
                    lstRelatedOrgs2.SelectedValue = gets(objList.RelatedOrgID);
                    divRelatedOrg2.Visible = true;
                }
                else
                { divRelatedOrg2.Visible = false; }

                if (objList.decisionNum != "")
                {
                    //divDessionNum.Visible = true;
                    txtDessionNum2.Text = objList.decisionNum;
                }
                else
                { //divDessionNum.Visible = false;
                }

                if (objList.publishType != 0 && objList.publishType != null)
                {
                    rblPublishType2.SelectedValue = objList.publishType.ToString();

                }
                else { rblPublishType2.SelectedValue = null; }

                if (objList.ProcedureTypeCode == 6)
                {
                    divDessionNum2.Visible = true;

                }
                else { divDessionNum2.Visible = false; }
                if (objList.ProcedureTypeCode == 16)
                {
                    divDessionNum3.Visible = true;

                }
                else { divDessionNum3.Visible = false; }

                if (objList.CMGSDessionNum != "")
                {
                    //divDessionNum.Visible = true;
                    txtcmgsDession2.Text = objList.CMGSDessionNum;
                }


                if (!gets(objList.CMGSDessionFile).Equals(""))
                {
                    btnCMGSScan2.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnCMGS2Scannerfilepath.Value = gets(objList.CMGSDessionFile);
                }
                if (!gets(objList.publishDessionFile).Equals(""))
                {
                    lnkDessionScan2.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnpublish2Scannerfilepath.Value = gets(objList.publishDessionFile);
                }


            }

            divAddProcedure2.Visible = true;
            divshowProcesure2.Visible = false;

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
           // divProAttache.Visible = true;


            ViewState["ProceduresitemID"] = "0";
            txtProcedureActionDate2.Text = "";
            txtRemarks2.Text = "";
            lstAttachmentType2.SelectedValue = "0";
            txtDessionNum2.Text = "";
            lstRelatedOrgs2.SelectedValue = "0";
            txtRef2.Text = "";
          //  divProAttache2.Visible = true;

            divAddProcedure2.Visible = false;
            divshowProcesure2.Visible = true;

            //BlblSubTitle.Text = this.GetTitle(true);
        }

        private void FillProcedureFrom()
        {

            var objList = objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(ViewState["ProceduresitemID"].ToString()));
            if ((objList != null))
            {


                txtRemarks.Text = gets(objList.Remarks);

                lstProcedureType.SelectedValue = gets(objList.ProcedureTypeCode);
                lstProcedureType_SelectedIndexChanged1(null, null);
                  txtProcedureActionDate.Text = NullDateifEmptyToText(objList.ProcedureDate);

                if (objList.RelatedOrgID != 0 && objList.RelatedOrgID!=null)
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
                else { //divDessionNum.Visible = false;
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

                if (objList.ProcedureTypeCode == 16)
                {
                    divDessionNum4.Visible = true;

                }
                else { divDessionNum4.Visible = false; }

                if (objList.CMGSDessionNum != "")
                {
                    //divDessionNum.Visible = true;
                    txtcmgsDession.Text = objList.CMGSDessionNum;
                }


            }


            if (!gets(objList.CMGSDessionFile).Equals(""))
            {
                lnkScanCMgs.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                hdnCMGSScannerfilepath.Value = gets(objList.CMGSDessionFile);
            }

            if (!gets(objList.publishDessionFile).Equals(""))
            {
               // lnkDessionScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                hdnpublishScannerfilepath.Value = gets(objList.publishDessionFile);
            }


            divAddProcedure.Visible = true;
            divshowProcesure.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        #endregion

        #region "Scanning"
        protected void lnkScan_Click(object sender, EventArgs e)
        {

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?")==-1)
            {
                _CallBackUrl = _URI;

            }

            //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";

            int _TargetID = SaveProcedureInformation(2);

            if (_TargetID != 0)
            {

                _TargetUploadPath = _TargetUploadPath + ZeroIntergerIFNull(hdnMasterID.Value) + "/" + gets(ViewState["ProceduresitemID"]) + "/";
                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='1' />";
                ScannerPostFrom += "<input type='hidden' id='id' name='id' value='" + ViewState["itemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='profiletype' name='profiletype' value='1' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='2' />";
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

            if (Request.Form["ActiveTab"]!=null)
            {
                hdnactivetab.Value = gets(Request.Form["ActiveTab"]);
            }
            if (Request.Form["fileList"] != null)
            {
                if (Request.Form["fileList"].ToString() != "")
                {
                    string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                    _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                    string[] FileList = _ScannerFileLlisy.Split(';');
                    if (Request.Form["profiletype"] != null && Request.Form["profiletype"] == "1")
                    {
                        if (Request.Form["TargetID"] != null)
                        {
                            for (int i = 0; i < FileList.Length; i++)
                            {//Update Current Record With
                             //TODO delete Existing files
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
                    }
                    else if (Request.Form["profiletype"] != null && Request.Form["profiletype"] == "2")
                    {
                        if (Request.Form["TargetID"] != null)
                        {
                            Agreement_procedureHistory objPro = new Agreement_procedureHistory();
                            for (int i = 0; i < FileList.Length; i++)
                            {//Update Current Record With
                             //Check FIle Existance and Delete File if exist
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
                                    objPro = objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                    objPro.CMGSDessionFile = null;
                                    objRepository.UpdateProcedure(objPro);
                                }
                                else
                                {

                                    objPro = objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                    objPro.CMGSDessionFile = FileList[i].Split(',')[0].ToString();
                                    objRepository.UpdateProcedure(objPro);
                                }
                            }

                        }

                        string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    }
                    else if (Request.Form["profiletype"] != null && Request.Form["profiletype"] == "3")
                    {
                        if (Request.Form["TargetID"] != null)
                        {
                            Agreement_procedureHistory objPro = new Agreement_procedureHistory();
                            for (int i = 0; i < FileList.Length; i++)
                            {//Update Current Record With
                                if (Request.Form["emptyFile"] != null)
                                {
                                    objPro = objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                    objPro.publishDessionFile = null;
                                    objRepository.UpdateProcedure(objPro);
                                }
                                else
                                {
                                    objPro = objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                    objPro.publishDessionFile = FileList[i].Split(',')[0].ToString();
                                    objRepository.UpdateProcedure(objPro);
                                }
                            }
                            string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        }
                    }
                    else
                    {
                        string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                    }


                }
            }

        }

        #endregion

        #region "Procedure2"
        protected void lnkDeleteProcedure2_Click(object sender, EventArgs e)
        {
            try
            {

                Agreement_procedureHistory obj = new Agreement_procedureHistory();
                for (int i = 0; i <= grdProcedure2.Items.Count - 1; i++)
                {

                    if ((grdProcedure2.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdProcedure2.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteProceduret((Agreement_procedureHistory)objRepository.GetprOCEDUREDetails(ZeroIntergerIFNull(grdProcedure2.Items[i].Cells[0].Text)));
                        }
                    }
                }


                FillAgreementProcedures(ZeroIntergerIFNull(hdnMasterID.Value), hdnIsInitial.Value);
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
        protected void lnkAddProcedure2_Click(object sender, EventArgs e)
        {
            CleaProcesure();
            divAddProcedure2.Visible = true;
            divshowProcesure2.Visible = false;
            ViewState["ProceduresitemID"] = "0";

        }
        #endregion

    
    }
}