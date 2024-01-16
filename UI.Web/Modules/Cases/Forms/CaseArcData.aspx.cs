using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class CaseArcData : BaseFormAdmin
    {
        #region "Page Members"
        public CasesRepository objRepository = IoC.Resolve<CasesRepository>();
        public string _PageTitle = " السجل ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "CasesAttachments/";
        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();


        #endregion

        #region "Page Events"
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "CasesData.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";
            btnCancel.Attributes.Add("onclick", "Page_ValidationActive=false;");
          //  btnSave.Attributes.Add("onclick", "return chkImage();");
           

            if (!IsPostBack)
            {
                UpdateScannedFile();
                if (Request.QueryString["RefType"]!=null)
                {
                    if (Request.QueryString["RefType"].ToString() == "1")
                    {
                        txtTo.Text = "CMGS";
                    }
                    else
                    { txtFrom.Text = "CMGS"; }

                }
              
                ViewState["CasesArcID"] = "0";

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

            if ((currnetPageIndx > grdincoming.PageCount))
            {
                currnetPageIndx = (grdincoming.PageCount - 1);
            }

            pager2.CurrentIndex = currnetPageIndx;
            grdincoming.CurrentPageIndex = (currnetPageIndx - 1);
            this.FillGrid();
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
                FillGrid();

            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteDataFailed + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
        }

        protected void grdincoming_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            ClearComingDoc();
            ViewState["CasesArcID"] = id;
            this.FillForm();
            divshowincoming.Visible = false;
            divAddIncoming.Visible = true;
        }
        protected void grdincoming_ItemCommand(object source, DataGridCommandEventArgs e)
        {

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
            FillGrid();
        }

        protected void grdincoming_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
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
        protected void lnkSaveIncoming_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["CaseID"]!=null)
            {
                SaveComingDocInformation( ZeroIntergerIFNull(Request.QueryString["RefType"].ToString()), ZeroIntergerIFNull(Request.QueryString["CaseID"].ToString()), ZeroIntergerIFNull(Request.QueryString["DocID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Case Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }


        }
        protected void lnkCancelIncoming_Click(object sender, EventArgs e)
        {
            ClearComingDoc();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillGrid();
        }
        protected void Lnkincoming_Click(object sender, EventArgs e)
        {
            
            ViewState["CasesArcID"] = "0";

            divAddIncoming.Visible = true;
            divshowincoming.Visible = false;
        }

        #endregion

        #region "Fill Information"
        private int SaveComingDocInformation(int DirectionType, int RefDocID,int DocParentID)
        {
            string script = "";
            int _TragetID = 0;
            try
            {
                arc_Data obj = new arc_Data();
                if (ViewState["CasesArcID"].Equals("0"))
                {//Save

                    obj.TransactionDate = DateTime.Now;
                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = 1;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtDoc_Serial.Text);
                    obj.Doc_Subject = gets(txtDoc_Subject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = txtFrom.Text;
                    obj.Doc_To =txtTo.Text;
                    obj.Doc_Notes = txtComingNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtComingDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtComingReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                    obj.ParentArcRefID = ZeroIntergerIFNull(Request.QueryString["DocID"].ToString());

                    obj.IsHearing = chkHasHearing.Checked;

                    if (chkHasHearing.Checked)
                    {// Ad New Hearing
                        Cases_H_Hearing objHearing = new Cases_H_Hearing();
                        objHearing.CaseID = RefDocID;
                        objHearing.CreationDate = DateTime.Now;
                        objHearing.HearningDate = NullDateifEmpty(txtHeadingDate.Text);
                        objRepository.AddHearing(objHearing);


                        obj.HearingID = objHearing.Code;//Ref Arc Hearing
                    }

                    objRepository.AddArcData(obj);
                    ViewState["CasesArcID"] = gets(obj.Code);


                }
                else
                { //Update 


                    obj = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["CasesArcID"].ToString()));


                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = 1;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtDoc_Serial.Text);
                    obj.Doc_Subject = gets(txtDoc_Subject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = txtFrom.Text;
                    obj.Doc_To =txtTo.Text;
                    obj.Doc_Notes = txtComingNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtComingDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtComingReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                    obj.ParentArcRefID = ZeroIntergerIFNull(Request.QueryString["DocID"].ToString());

                    Cases_H_Hearing objHearing = new Cases_H_Hearing();
                    if (chkHasHearing.Checked && obj.HearingID != null)
                    {// Ad New Hearing

                        objHearing.CaseID = RefDocID;
                        objHearing.CreationDate = DateTime.Now;
                        objHearing.HearningDate = NullDateifEmpty(txtHeadingDate.Text);
                        objRepository.AddHearing(objHearing);


                        obj.HearingID = objHearing.Code;//Ref Arc Hearing
                    }
                    else
                    {//Update Hearing Information
                        objHearing = objRepository.GetHearingDetails(obj.HearingID);
                        objHearing.CaseID = RefDocID;

                        objHearing.HearningDate = NullDateifEmpty(txtHeadingDate.Text);
                        objRepository.UpdateHearing(objHearing);
                    }

                    objRepository.UpdateArcData(obj);

                }

                ViewState["CasesArcID"] = obj.Code;

                ClearComingDoc();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            FillGrid();
             
            return _TragetID;
        }
        private void FillGrid()
        {
            var objList = objRepository.FillArcDocs(ZeroIntergerIFNull( Request.QueryString["RefType"].ToString()), ZeroIntergerIFNull(Request.QueryString["CaseID"].ToString()), ZeroIntergerIFNull(Request.QueryString["DocID"].ToString()));
            lblComingCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
           

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
            var objList = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["CasesArcID"].ToString()));
            if ((objList != null))
            {
                txtDoc_Serial.Text = gets(objList.Doc_Serial);
                 txtDoc_Subject.Text = gets(objList.Doc_Subject);
                txtComingNotes.Text = gets(objList.Doc_Notes);

                txtFrom.Text= gets(objList.Doc_From);
                txtComingDate.Text = NullDateifEmptyToText(objList.SentDate);
                txtComingReminderDate.Text = NullDateifEmptyToText(objList.NextFollowReminderDate);

                chkHasHearing.Checked = getBool(objList.IsHearing);

                if (chkHasHearing.Checked)
                {
                    chkHasHearing.Enabled = false;

                }
               
            }

            divAddIncoming.Visible = true;
            divshowincoming.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private void ClearComingDoc()
        {
            txtDoc_Serial.Text = "";
            txtDoc_Subject.Text = "";
            txtComingDate.Text = "";
            txtComingReminderDate.Text = "";

            divAddIncoming.Visible = false;
            divshowincoming.Visible = true;
 

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
        //        _TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&DocID=" + Request.QueryString["DocID"].ToString() + "&CaseID=" + Request.QueryString["CaseID"].ToString() + "&TargetID=" + _TargetID;
        //        Response.Redirect(_TargetUrl);
        //    }

        //}

        private void UpdateScannedFile()
        {
            //arc_Attachments obj = new arc_Attachments();
            //if (Request.QueryString["fileList"] != null)
            //{
            //    if (Request.QueryString["fileList"].ToString() != "")
            //    {
            //        string _ScannerFileLlisy = Request.QueryString["fileList"].ToString();
            //        _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
            //        string[] FileList = _ScannerFileLlisy.Split(';');

            //        if (Request.QueryString["TargetID"] != null)
            //        {
            //            for (int i = 0; i < FileList.Length; i++)
            //            {//Update Current Record With 

            //                obj = objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(Request.QueryString["TargetID"].ToString()));
            //                obj.Filepath = FileList[i].Split(',')[0].ToString();
            //                objRepository.UpdateAttachment(obj);

            //            }

            //        }

            //        string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            //    }
            //    else
            //    {
            //        string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            //    }


            //}


        }

        #endregion



    }
}