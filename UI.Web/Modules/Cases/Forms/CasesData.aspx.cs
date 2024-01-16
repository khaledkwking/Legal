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

namespace UI.Web.Modules.Cases.Forms
{
    public partial class CasesData : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public CasesRepository objRepository = IoC.Resolve<CasesRepository>();
        public string _PageTitle = " القضايا ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "CasesAttachments/Cases/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();


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
                    //  ClearForm();

                    string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                }



                fillLookups();


                if (Request.QueryString["DegreeID"] != null)
                {
                    filleCasesFilterResult();
                    return;
                }


                ViewState["SpEdit"] = "0";
                ViewState["NewDesc"] = "";
                ViewState["NewBar"] = "";
                ViewState["NewIsbn"] = "";
                ViewState["SPITEM"] = "";
                ViewState["NewPrice"] = "0";
                Session["ItemList"] = null;
                ViewState["itemID"] = "0";
                ViewState["CaseitemID"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["CasesArcID"] = "0";
                ViewState["AttachitemID"] = "0";
                ViewState["HearingCode"] = "0";

                Session["PersonsList"] = null;
                Session["objDefendant"] = null;
                Session["objprosecutor"] = null;



                if (Request.QueryString["FileID"] == null)
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


                    ViewState["itemID"] = Request.QueryString["FileID"].ToString();
                    FillCaseMasterInformation();
                    //Fill Agreemnt Details


                }

                SetPageTitle();

                ViewState["OutboundItemID"] = "0";

                // FillInboundItems();
                UpdateScannedFile();

            }

            //Bind Uploading path

        }

        protected void grdCasesList_ItemDataBound(object sender, DataGridItemEventArgs e)
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
                string Filecode = e.Item.Cells[3].Text;

                //SqlDataReader dr = SellMaster.ins.getInvoiceItemsReader(code);

                var objUnitList = objRepository.GetFileCases(ZeroIntergerIFNull(Filecode));
                if (objUnitList != null)
                {
                    DataGrid grd = ((DataGrid)(e.Item.Cells[1].FindControl("grdCases")));
                    grd.DataSource = objUnitList;
                    grd.Columns[13].Visible = userAccess.Delete;
                    grd.DataBind();



                    //switch (ItemType)
                    //{
                    //    case "1"://Cargo


                    //        grd.Columns[1].Visible = true;
                    //        grd.Columns[2].Visible = true;


                    //        grd.Columns[3].Visible = false;
                    //        grd.Columns[4].Visible = false;
                    //        grd.Columns[5].Visible = false;
                    //        grd.Columns[6].Visible = false;


                    //        break;
                    //    case "2"://Container
                    //        grd.Columns[1].Visible = true;
                    //        grd.Columns[2].Visible = true;

                    //        grd.Columns[3].Visible = false;
                    //        grd.Columns[4].Visible = false;
                    //        grd.Columns[5].Visible = false;
                    //        grd.Columns[6].Visible = false;



                    //        break;
                    //    case "3"://Vehicle 7-11

                    //        grd.Columns[1].Visible = false;
                    //        grd.Columns[2].Visible = false;

                    //        grd.Columns[3].Visible = true;
                    //        grd.Columns[4].Visible = true;
                    //        grd.Columns[5].Visible = true;
                    //        grd.Columns[6].Visible = true;

                    //        break;
                    //    default:
                    //        break;
                    //}


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

            //Hide Defult Dates

            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    if (e.Item.Cells[13].Text == "01/01/1990") e.Item.Cells[13].Text = "";
            //    if (e.Item.Cells[15].Text == "01/01/1990") e.Item.Cells[15].Text = "";
            //    if (e.Item.Cells[16].Text == "01/01/1990") e.Item.Cells[16].Text = "";



            //}
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
            Session["objprosecutor"] = null;
            Session["objDefendant"] = null;
            FillCaselPersons(0);
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
            FillCases();
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
            ViewState["CaseitemID"] = "0";
            ViewState["ProceduresitemID"] = "0";
            Session["PersonsList"] = null;
            Response.Redirect("/Modules/Cases/Forms/CasesData.aspx");

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillCases();
        }
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            SaveCaseMaster();
        }


        protected void lnkAddNewCase_Click(object sender, EventArgs e)
        {
            ClearCaseForm();
        }

        protected void btnHearingScan_Click(object sender, EventArgs e)
        {
            //Show Loadin div

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            int _TargetID = 0;                                                             //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }
            if (ViewState["CaseitemID"].ToString() != "0" && ViewState["CaseitemID"].ToString() != "")
            {
                _TargetID = SaveHearingInformation(ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Case Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["CaseitemID"].ToString() + "/hearing/" + _TargetID.ToString() + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnHearingScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnHearingScannerfilepath.Value + "]' />";

                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + ViewState["CaseitemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["itemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='2' />";
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

        protected void grdCasesList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                objRepository.Deletefile((Case_FileInformation)objRepository.GetFileDetails(ZeroIntergerIFNull(e.Item.Cells[3].Text)));
            }
            FillCases();
        }

        protected void grdCases_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "delete")
            {
                objRepository.DeleteCase((Cases_Data)objRepository.GetCaseDetails(ZeroIntergerIFNull(e.Item.Cells[0].Text)));
            }
            FillCases();
        }


        //}
        #endregion

        #region "Fill Information"


        public string ShowJudgmentresult(string Judgmentresult)
        {
            if (Judgmentresult == "")
            {
                return "";

            }
            if (Judgmentresult == "1")
            {
                return "<span class=\'label label-sm label-success\'>&nbsp;لصالح&nbsp;</span>";
            }
            else if (Judgmentresult == "2")
            {
                return "<span class=\'label label-sm label-danger\'>&nbsp;ضد&nbsp;</span>";
            }
            else { return ""; }

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

        private void FillCaselPersons(int CaseID)
        {
            var objprosecutor = objRepository.FillPersons(CaseID, 1);// المدعي
            if (Session["objprosecutor"] != null && ViewState["SpEdit"].ToString() == "1")
            {
                objprosecutor = (List<Case_parties>)Session["objprosecutor"];
            }
            else if (objprosecutor.Count > 0)
            {
                Session["objprosecutor"] = objprosecutor;
            }


            if (objprosecutor != null && objprosecutor.Count > 0)
            {

                grdprosecutor.DataSource = AddDefaultItems(objprosecutor);



                if (ViewState["SpEdit"].Equals("0"))
                {
                    grdprosecutor.EditItemIndex = objprosecutor.Count;
                }

                grdprosecutor.DataBind();



            }
            else
            {

                grdprosecutor.EditItemIndex = 0;
                grdprosecutor.DataSource = AddDefaultItems(objprosecutor);
                grdprosecutor.DataBind();

            }


            ///'//////////////////////////////////'

            var objDefendant = objRepository.FillPersons(CaseID, 2);//عليه المدعي
            if (Session["objDefendant"] != null && ViewState["SpEdit"].ToString() == "1")
            {
                objDefendant = (List<Case_parties>)Session["objDefendant"];
            }
            else if (objDefendant.Count > 0)
            {
                Session["objDefendant"] = objDefendant;
            }


            if (objDefendant != null && objDefendant.Count > 0)
            {

                grdDefendant.DataSource = AddDefaultItems(objDefendant);



                if (ViewState["SpEdit"].Equals("0"))
                {
                    grdDefendant.EditItemIndex = objDefendant.Count;
                }

                grdDefendant.DataBind();



            }
            else
            {

                grdDefendant.EditItemIndex = 0;
                grdDefendant.DataSource = AddDefaultItems(objDefendant);
                grdDefendant.DataBind();

            }


        }
        private List<Case_parties> AddDefaultItems(List<Case_parties> _SourceList)
        {
            List<Case_parties> _OutList = new List<Case_parties>();

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
                    _OutList.Add(new Case_parties());
                }

            }
            else
            {

                for (int i = 0; i < _SourceList.Count; i++)
                {
                    _OutList.Add(_SourceList[i]);
                }
                //  _OutList = _SourceList;
                _OutList.Add(new Case_parties());

            }

            return _OutList;
        }
        public string ViewPrivateParty()
        {
            return getBool(ReadSession("ViewPrivate")) ? "" : "none";
        }
        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            lnkAddNewCase.Visible = userAccess.Add;
            lnkAddHearing.Visible = userAccess.Add;

            Lnkincoming.Visible = userAccess.Add;
            lnkDeleteIncoming.Visible = userAccess.Add;

            lnkAddNewOutGoing.Visible = userAccess.Add;
            lnkDeleteOutgoing.Visible = userAccess.Add;



            btnSave.Visible = userAccess.Edit || userAccess.Add;
            lnkSaveAttachment.Visible = userAccess.Edit || userAccess.Add;
            lnkSaveHearing.Visible = userAccess.Edit || userAccess.Add;


            lnkSaveIncoming.Visible = userAccess.Edit || userAccess.Add;
            lnkSaveOut.Visible = userAccess.Edit || userAccess.Add;


            //btnDelete.Visible = userAccess.Delete;
            grdCasesList.Columns[12].Visible = userAccess.Delete;
            lnkDeleteAttachment.Visible = userAccess.Delete;
            lnkDeleteHearing.Visible = userAccess.Delete;



        }

        private string MapSearchKeys()
        {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                _keyList.Add("رقم المسلسل", txtFilterInternalSerial.Text);
                _keyList.Add("السنة ", txtFilterFileYear.Text);
                _keyList.Add(" تاريخ  ايداع القضية فى المحكمة من", txtFilterDatefrom.Text);
                _keyList.Add(" الي تاريخ", txtFilterDateTo.Text);
                _keyList.Add("رقم القضية", txtFSuitNum.Text + "/" + txtFSuitYear.Text + "/" + lstFiltertype.SelectedItem.Text + "/" + txtFSuitType.Text);
                _keyList.Add("الموضوع", txtFilterSubject.Text);
                _keyList.Add(" الدرجه", lstFCaseLevel.SelectedItem.Text);
                _keyList.Add("  المدعي علية  ", lstFilterPersonName.SelectedItem.Text);
                _keyList.Add("   الرقم الإلى ", txtFilterFileNUm.Text);
                _keyList.Add(" المدعي", lstFiltercaseParty.SelectedItem.Text);
                _keyList.Add(" الحكم", lstFilterDession.SelectedItem.Text);
                _keyList.Add(" الحالة", lstFilterStatus.SelectedItem.Text);
                _keyList.Add(" الموظف المختص", lstFilterAssignedPerson.SelectedItem.Text);
                _keyList.Add(" نتيجة الحكم", lstfilterJudgmentresult.SelectedItem.Text);
            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }

        private void FillCases()
        {
            var objList = objRepository.GetFileList(txtFilterFileNUm.Text, ZeroIntergerIFNull(txtFilterInternalSerial.Text), ZeroIntergerIFNull(txtFilterFileYear.Text), NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFiltertype.SelectedValue), ZeroIntergerIFNull(lstFilterStatus.SelectedValue),
                txtFilterSubject.Text, getBool(ReadSession("ViewPrivate")), txtFSuitType.Text, txtFSuitNum.Text, txtFSuitYear.Text,
                ZeroIntergerIFNull(lstFCaseLevel.SelectedValue),
                ZeroIntergerIFNull(lstFilterDession.SelectedValue), lstFilterPersonName.SelectedValue,
                lstFiltercaseParty.SelectedValue,
                ZeroIntergerIFNull(lstFilterAssignedPerson.SelectedValue), MapSearchKeys(), ZeroIntergerIFNull(lstfilterJudgmentresult.SelectedValue), ZeroIntergerIFNull(lstfiltermainType.SelectedValue));


            lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
            lblcount3.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));


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

          
            grdCasesList.DataSource = duplicatedList;
            grdCasesList.DataBind();

          
            pager1.ItemCount = duplicatedList.Count;

        }
        private void filleCasesFilterResult()
        {
            var objList = objRepository.getCasesResult(ZeroIntergerIFNull(Request.QueryString["DegreeID"]));


            lblcount2.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));


            //var duplicatedList = objList.SelectMany(t =>
            // Enumerable.Repeat(t, 2)).ToList();


            if (objList.Count > 0)
            {
                //btnSave.Visible = true;
                //lnkBack.Visible = true;

                tblshow2.Visible = true;
                tblSearch.Visible = false;
                pager6.Visible = true;

            }
            else
            {
                tblshow2.Visible = false;
                pager6.Visible = false;
                tblSearch.Visible = true;
                string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }




            grdCasesFilterResult.DataSource = objList;
            grdCasesFilterResult.DataBind();


            pager6.ItemCount = objList.Count;

        }



        private void ClearCaseForm()
        {


            ViewState["CaseitemID"] = "0";
            txtSuitType.Text = "";
            txtSuitNum.Text = "";
            lstCaseType.SelectedValue = "0";
            lstCaseLevel.SelectedValue = "0";
            lstcaseStatus.SelectedValue = "0";
            lstDession.SelectedValue = "0";
            lstJudgmentresult.SelectedValue = "0";
            // lstCourt.SelectedValue = "0";


            txtSuitYear.Text = "";

            txtCaseDate.Text = "";
            txtCaseNote.Text = "";
            txtCaseSubject.Text = "";

            Session["PersonsList"] = null;


            Session["objprosecutor"] = null;
            Session["objDefendant"] = null;
            FillCaselPersons(0);


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

        protected void pager_Command(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdCasesList.PageCount))
            {
                currnetPageIndx = (grdCasesList.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdCasesList.CurrentPageIndex = (currnetPageIndx - 1);
            FillCases();
        }
        protected void grdCasesFilterResult_pager_Command(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdCasesFilterResult.PageCount))
            {
                currnetPageIndx = (grdCasesFilterResult.PageCount - 1);
            }

            pager6.CurrentIndex = currnetPageIndx;
            grdCasesFilterResult.CurrentPageIndex = (currnetPageIndx - 1);
            filleCasesFilterResult();
        }

        #endregion

        #region "Helper Methods"


        private void FillCaseMasterInformation()
        {

            var objList = objRepository.FillFileDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {
                hdnMasterID.Value = gets(objList.Code);



                txtFileAutoNumber.Text = gets(objList.FileAutoNumber);
                if (gets(objList.FileAutoNumber).Equals("000000000000"))
                {
                    txtFileAutoNumber.Attributes.Add("disabled", "true");
                    chkhasautoNumber.Checked = true;

                }
                txtfilnum.Text = gets(objList.FileInternalSerial);
                txtFileInternalserial.Text = gets(objList.FileInternalSerial);
                txtSerialYear.Text = gets(objList.fileSerialyear);
                txtFileNotes.Text = gets(objList.FileNote);
                txtFileCreationDate.Text = NullDateifEmptyToText(objList.TransDate).ToString();
                chkIsPrivate.Checked = getBool(objList.isPrivate);
                lstassignedPersons.SelectedValue = gets(objList.assignedPersonID);

                //Get Case Information , Selected Or last Case Data
                var ObjCaseData = objRepository.GetFileLastCase(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                if (Request.QueryString["CaseID"] != null)
                {
                    if (Request.QueryString["CaseID"].ToString() != ObjCaseData.Code.ToString())
                    {
                        ObjCaseData = objRepository.GetCaseDetails(ZeroIntergerIFNull(Request.QueryString["CaseID"]));
                    }

                }

                if (ObjCaseData != null)
                {



                    //Set Case Information
                    ViewState["CaseitemID"] = gets(ObjCaseData.Code);
                    anchorAttachment.Visible = true;
                    //DocID=0&CaseID=<%#Eval("CaseID")%>&FileID=0

                    //Get Case Attachment Count
                    int CaseAttachmentCount = 0;

                    CaseAttachmentCount = objRepository.FillCaseDocsAttachemntCount(ObjCaseData.Code, 0, 0);

                    anchorAttachment.HRef = "CaseAttachments.aspx?DocID=0&FileID=0&CaseID=" + gets(ObjCaseData.Code);
                    anchorAttachment.InnerHtml = "<i class='icon-attachment'></i>&nbsp;  مرفقات القضية <b>[ " + CaseAttachmentCount.ToString() + " ]</b>";
                    txtSuitType.Text = gets(ObjCaseData.SuitType);
                    txtSuitNum.Text = gets(ObjCaseData.SuitNum);
                    txtSuitYear.Text = gets(ObjCaseData.SuitYear);

                    txtCaseInternalSerial.Text = gets(ObjCaseData.CaseInternalSerial);

                    txtCaseDate.Text = NullDateifEmptyToText(ObjCaseData.TransDate).ToString();

                    lstcaseStatus.SelectedValue = gets(ObjCaseData.CaseStatus);
                    lstCaseType.SelectedValue = gets(ObjCaseData.CaseType);

                    lstJudgmentresult.SelectedValue = gets(ObjCaseData.Judgmentresult);

                    // lstCourt.SelectedValue = gets(ObjCaseData.CourtID);
                    //FillDllwithoptional_ALL(objLookup.FillCourtBranches(ZeroIntergerIFNull(lstCourt.SelectedValue)), lstCourtBrach, "NameAr", "Code", "الكل");
                    lstDession.SelectedValue = gets(ObjCaseData.CaseDecisionID);
                    lstCaseLevel.SelectedValue = gets(ObjCaseData.DegreeID);

                    txtCaseSubject.Text = gets(ObjCaseData.CaseSubject);
                    txtCaseNote.Text = gets(ObjCaseData.CaseNotes);
                    try
                    {
                        lstCategory.SelectedValue = gets(ObjCaseData.CaseMainType);
                    }
                    catch (Exception)
                    {

                         
                    }



                    //Fill Person Information
                    FillCaselPersons(ObjCaseData.Code);
                    //Fill Arch
                    FillArcData(1, ObjCaseData.Code);
                    FillArcData(2, ObjCaseData.Code);
                    //Fill Attachments

                    FillFileAttachment(objList.Code);
                    FilLCaseHearing(ObjCaseData.Code);
                }

            }

            tblAdd.Visible = true;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private void SaveCaseMaster()
        {
            string script = "";
            if (!CheckPersonCount())
            {
                script = FormatpopupErrorMSG("عفوا ، ادخل الخصوم", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                return;

            }


            try
            {

                Case_FileInformation objFile = new Case_FileInformation();
                Cases_Data _obj = new Cases_Data();

                //Added To Validate on auto number
                if (txtFileAutoNumber.Text.ToString() == "")
                {
                    txtFileAutoNumber.Text = "000000000000";
                }



                if (ViewState["itemID"].Equals("0"))
                {//Save
                    if (objRepository.CheckCaseFileExistance(ZeroIntergerIFNull(txtFileInternalserial.Text), ZeroIntergerIFNull(txtSerialYear.Text), 0))
                    {

                        script = FormatpopupErrorMSG("رقم الملف مسجل من قبل [مسلسل / سنة]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return;
                    }

                    if (txtFileAutoNumber.Text.ToString() != "000000000000" && txtFileAutoNumber.Text.ToString() != "")
                    {

                        //script = FormatpopupErrorMSG(txtFileInternalserial.Text.ToString() +" - " + ( txtFileInternalserial.Text.ToString() != "000000000000").ToString(), "1");
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        //return;


                        if (objRepository.CheckFileAutoNumberExistance(txtFileAutoNumber.Text, 0))
                        {

                            script = FormatpopupErrorMSG("  الرقم الإلى مسجل من قبل", "1");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                            return;

                        }
                    }

                    objFile.TransDate = DateTime.Now;
                    objFile.CreationDate = DateTime.Now;
                    objFile.LastTransctionDate = DateTime.Now;
                    objFile.FileAutoNumber = txtFileAutoNumber.Text;
                    objFile.file_Serial = txtFileInternalserial.Text;
                    objFile.fileSerialyear = ZeroIntergerIFNull(txtSerialYear.Text);
                    //if (!txtfilnum.Text.Equals(""))
                    //{
                    //    string[] fn = txtfilnum.Text.Split('/');
                    //    if (fn.Length > 0)
                    //    {
                    //        objFile.CaseSerialNum = ZeroIntergerIFNull(fn[0]);
                    //        objFile.CaseSerialyea = ZeroIntergerIFNull(fn[1]);
                    //    }
                    //}

                    objFile.FileInternalSerial = ZeroIntergerIFNull(txtFileInternalserial.Text);

                    objFile.TransDate = NullDateifEmpty(txtFileCreationDate.Text);
                    objFile.FileNote = txtFileNotes.Text;
                    objFile.isPrivate = getBool(chkIsPrivate.Checked);
                    objFile.assignedPersonID = ZeroIntergerIFNull(lstassignedPersons.SelectedValue);
                    objFile.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                    objRepository.AddFile(objFile);
                    hdnMasterID.Value = gets(objFile.Code);
                    ViewState["itemID"] = gets(objFile.Code);


                    ////Save Case Informations
                    if (ViewState["CaseitemID"].Equals("0"))
                    {//Save

                        _obj.FileNumID = objFile.Code;
                        // _obj.CaseSerial = txtSuitType.Text + "-" + txtSuitNum.Text + "-" + txtSuitYear.Text;
                        _obj.CaseSerial = txtSuitNum.Text + "-" + txtSuitYear.Text;

                        _obj.CreationDate = DateTime.Now;
                        _obj.LastActionDate = DateTime.Now;
                        _obj.CaseType = ZeroIntergerIFNull(lstCaseType.SelectedValue);
                        _obj.DegreeID = ZeroIntergerIFNull(lstCaseLevel.SelectedValue);
                        //_obj.CourtID = ZeroIntergerIFNull(lstCourt.SelectedValue);
                        _obj.CaseDecisionID = ZeroIntergerIFNull(lstDession.SelectedValue);
                        _obj.SuitType = txtSuitType.Text;
                        _obj.SuitNum = txtSuitNum.Text;
                        _obj.SuitYear = txtSuitYear.Text;
                        _obj.CaseSubject = txtCaseSubject.Text;
                        _obj.CaseNotes = txtCaseNote.Text;
                        _obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                        _obj.CaseInternalSerial = txtCaseInternalSerial.Text;

                        _obj.Judgmentresult = ZeroIntergerIFNull(lstJudgmentresult.SelectedValue);
                        _obj.CaseMainType = ZeroIntergerIFNull(lstCategory.SelectedValue);


                        if (txtCaseDate.Text != "")
                        {
                            _obj.TransDate = NullDateifEmpty(txtCaseDate.Text);
                        }
                        else { _obj.TransDate = null; }


                        _obj.CaseStatus = ZeroIntergerIFNull(lstcaseStatus.SelectedValue);
                        objRepository.AddCase(_obj);
                    }
                    else
                    {//Update Case Infirmation
                        _obj = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));
                        _obj.FileNumID = objFile.Code;
                        //_obj.CaseSerial = txtSuitType.Text + "-" + txtSuitNum.Text + "-" + txtSuitYear.Text;
                        _obj.CaseSerial = txtSuitNum.Text + "-" + txtSuitYear.Text;



                        _obj.LastActionDate = DateTime.Now;
                        _obj.CaseType = ZeroIntergerIFNull(lstCaseType.SelectedValue);
                        _obj.DegreeID = ZeroIntergerIFNull(lstCaseLevel.SelectedValue);
                        //  _obj.CourtID = ZeroIntergerIFNull(lstCourt.SelectedValue);
                        _obj.CaseDecisionID = ZeroIntergerIFNull(lstDession.SelectedValue);

                        if (txtCaseDate.Text != "")
                        {
                            _obj.TransDate = NullDateifEmpty(txtCaseDate.Text);
                        }
                        else { _obj.TransDate = null; }

                        _obj.SuitType = txtSuitType.Text;
                        _obj.SuitNum = txtSuitNum.Text;
                        _obj.SuitYear = txtSuitYear.Text;
                        _obj.CaseSubject = txtCaseSubject.Text;
                        _obj.CaseNotes = txtCaseNote.Text;
                        _obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                        _obj.CaseInternalSerial = txtCaseInternalSerial.Text;
                        _obj.CaseStatus = ZeroIntergerIFNull(lstcaseStatus.SelectedValue);
                        _obj.CaseInternalSerial = txtCaseInternalSerial.Text;
                        _obj.Judgmentresult = ZeroIntergerIFNull(lstJudgmentresult.SelectedValue);
                        _obj.CaseMainType = ZeroIntergerIFNull(lstCategory.SelectedValue);

                        objRepository.UpdateCase(_obj);
                    }
                    // Save Persons

                }
                else
                { //Update


                    if (objRepository.CheckCaseFileExistance(ZeroIntergerIFNull(txtFileInternalserial.Text), ZeroIntergerIFNull(txtSerialYear.Text), ZeroIntergerIFNull(ViewState["itemID"].ToString())))
                    {
                        script = FormatpopupErrorMSG("رقم الملف مسجل من قبل [مسلسل / سنة]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return;
                    }
                    if (txtFileAutoNumber.Text.ToString() != "000000000000" && txtFileAutoNumber.Text.ToString() != "")
                    {
                        if (objRepository.CheckFileAutoNumberExistance(txtFileAutoNumber.Text, ZeroIntergerIFNull(ViewState["itemID"].ToString())))
                        {
                            script = FormatpopupErrorMSG("  الرقم الإلى مسجل من قبل", "1");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                            return;
                        }
                    }

                    hdnMasterID.Value = ViewState["itemID"].ToString();
                    objFile = objRepository.GetFileDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    objFile.LastTransctionDate = DateTime.Now;
                    objFile.FileAutoNumber = txtFileAutoNumber.Text;

                    objFile.FileInternalSerial = ZeroIntergerIFNull(txtFileInternalserial.Text);

                    objFile.file_Serial = txtFileInternalserial.Text;
                    objFile.fileSerialyear = ZeroIntergerIFNull(txtSerialYear.Text);


                    objFile.TransDate = NullDateifEmpty(txtFileCreationDate.Text);

                    objFile.FileNote = txtFileNotes.Text;
                    objFile.isPrivate = getBool(chkIsPrivate.Checked);
                    objFile.assignedPersonID = ZeroIntergerIFNull(lstassignedPersons.SelectedValue);
                    objFile.FileInternalSerial = ZeroIntergerIFNull(txtFileInternalserial.Text);


                    objFile.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                    objRepository.Updatefile(objFile);



                    //////Update Case Informations

                    //_obj.FileNumID = objFile.Code;
                    //_obj.CaseSerial = txtSuitType.Text + "-" + txtSuitNum.Text + "-" + txtSuitYear.Text;


                    //_obj.LastActionDate = DateTime.Now;
                    //_obj.CaseType = ZeroIntergerIFNull(lstCaseType.SelectedValue);
                    //_obj.DegreeID = ZeroIntergerIFNull(lstCaseLevel.SelectedValue);
                    //_obj.CourtID = ZeroIntergerIFNull(lstCourt.SelectedValue);
                    //_obj.SuitType = txtSuitType.Text;
                    //_obj.SuitNum = txtSuitNum.Text;
                    //_obj.SuitYear = txtSuitYear.Text;
                    //_obj.CaseSubject = txtCaseSubject.Text;
                    //_obj.CaseNotes = txtCaseNote.Text;
                    //_obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    //_obj.CaseStatus = ZeroIntergerIFNull(lstcaseStatus.SelectedValue);
                    //objRepository.UpdateCase(_obj);
                    ////Save Case Informations
                    if (ViewState["CaseitemID"].Equals("0"))
                    {//Save

                        _obj.FileNumID = objFile.Code;
                        // _obj.CaseSerial = txtSuitType.Text + "-" + txtSuitNum.Text + "-" + txtSuitYear.Text;
                        _obj.CaseSerial = txtSuitNum.Text + "-" + txtSuitYear.Text;

                        _obj.CreationDate = DateTime.Now;
                        _obj.LastActionDate = DateTime.Now;
                        _obj.CaseType = ZeroIntergerIFNull(lstCaseType.SelectedValue);
                        _obj.DegreeID = ZeroIntergerIFNull(lstCaseLevel.SelectedValue);
                        //   _obj.CourtID = ZeroIntergerIFNull(lstCourt.SelectedValue);
                        _obj.CaseDecisionID = ZeroIntergerIFNull(lstDession.SelectedValue);

                        if (txtCaseDate.Text != "")
                        {
                            _obj.TransDate = NullDateifEmpty(txtCaseDate.Text);
                        }
                        else { _obj.TransDate = null; }

                        _obj.SuitType = txtSuitType.Text;
                        _obj.SuitNum = txtSuitNum.Text;
                        _obj.SuitYear = txtSuitYear.Text;
                        _obj.CaseSubject = txtCaseSubject.Text;
                        _obj.CaseNotes = txtCaseNote.Text;

                        _obj.CaseInternalSerial = txtCaseInternalSerial.Text;

                        _obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                        _obj.CaseStatus = ZeroIntergerIFNull(lstcaseStatus.SelectedValue);

                        _obj.Judgmentresult = ZeroIntergerIFNull(lstJudgmentresult.SelectedValue);
                        _obj.CaseMainType = ZeroIntergerIFNull(lstCategory.SelectedValue);
                        objRepository.AddCase(_obj);
                    }
                    else
                    {//Update Case Infirmation

                        //Update Case Information
                        _obj = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));

                        _obj.FileNumID = objFile.Code;
                        _obj.CaseSerial = txtSuitNum.Text + "-" + txtSuitYear.Text;

                        _obj.LastActionDate = DateTime.Now;
                        _obj.CaseType = ZeroIntergerIFNull(lstCaseType.SelectedValue);
                        _obj.DegreeID = ZeroIntergerIFNull(lstCaseLevel.SelectedValue);
                        // _obj.CourtID = ZeroIntergerIFNull(lstCourt.SelectedValue);
                        _obj.CaseDecisionID = ZeroIntergerIFNull(lstDession.SelectedValue);

                        if (txtCaseDate.Text != "")
                        {
                            _obj.TransDate = NullDateifEmpty(txtCaseDate.Text);
                        }
                        else { _obj.TransDate = null; }
                        _obj.SuitType = txtSuitType.Text;
                        _obj.SuitNum = txtSuitNum.Text;
                        _obj.SuitYear = txtSuitYear.Text;
                        _obj.CaseSubject = txtCaseSubject.Text;
                        _obj.CaseNotes = txtCaseNote.Text;
                        _obj.CaseInternalSerial = txtCaseInternalSerial.Text;

                        _obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                        _obj.Judgmentresult = ZeroIntergerIFNull(lstJudgmentresult.SelectedValue);
                        _obj.CaseMainType = ZeroIntergerIFNull(lstCategory.SelectedValue);
                        _obj.CaseStatus = ZeroIntergerIFNull(lstcaseStatus.SelectedValue);
                        objRepository.UpdateCase(_obj);
                    }



                }

                ViewState["itemID"] = objFile.Code;

                anchorAttachment.Visible = true;
                anchorAttachment.HRef = "CaseMainAttachments.aspx?CaseMasterID=" + hdnMasterID.Value;

                //  ClearForm();
                //Save Person
                try
                {
                    if (_obj.Code == 0)
                    {
                        script = FormatpopupErrorMSG("خطأ  ، في ادخال بيانات القضية، يرجي المحاولة مره اخري ", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return;

                    }

                    SavePersons(_obj.Code);
                    Session["PersonsList"] = null;


                    script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);


                    Response.Redirect("CasesData.aspx?ss=1&FileID=" + gets(objFile.Code) + "&CaseID=" + gets(_obj.Code));


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

        }
        private void fillLookups()
        {



            //Session["MedalList"], (Dro
            //Session["MedalGradeList"],
            //Session["Grantreasons"], (

            //var MedalTypeList = objLookup.FillCasesTypes();
            //Session["MedalList"] = MedalTypeList;


            //var MedalGradeList = objLookup.FillJobGrade();
            //Session["MedalGradeList"] = MedalGradeList;


            //var Grantreasons = objLookup.FillGrantReason();
            //Session["Grantreasons"] = Grantreasons;

            FillDllwithoptional_ALL(objLookup.FillCasesMainTypes(), ref lstfiltermainType, "NameAr", "Code", "التصنيف");
            FillDllwithoptional_ALL(objLookup.FillCasesMainTypes(), ref lstCategory, "NameAr", "Code", "التصنيف");


            FillDllwithoptional_ALL(objLookup.FillCasesTypes(), ref lstCaseType, "NameAr", "Code", "الدائره");
            FillDllwithoptional_ALL(objLookup.FillCasesTypes(), ref lstFiltertype, "NameAr", "Code", "الدائره");


            FillDllwithoptional_ALL(objLookup.FillCasesStatus(), ref lstcaseStatus, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillCasesStatus(), ref lstFilterStatus, "NameAr", "Code", "الكل");


            //FillDllwithoptional_ALL(objLookup.FillCourt(), lstCourt, "NameAr", "Code", "اختر");
            //FillDllwithoptional_ALL(objLookup.FillCourt(), lstFilterCourt, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillLitigationDegree(), ref lstCaseLevel, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillLitigationDegree(), ref lstFCaseLevel, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillAttachmentType(), ref lstAttachmentType, "NameAr", "Code", "");

            FillDllwithoptional_ALL(objLookup.Filldession(), ref lstDession, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.Filldession(), ref lstFilterDession, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillCaseAssignedPersons(), ref lstFilterAssignedPerson, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillCaseAssignedPersons(), ref lstassignedPersons, "NameAr", "Code", "");

            FillDllwithoptional_ALL(objLookup.FillCaseAutopersons(), ref lstFilterPersonName, "NameAr", "NameAr", "الكل");
            FillDllwithoptional_Array(objLookup.FillCasePatires(), lstFiltercaseParty, "الكل");

            FillDllwithoptional_ALL(objLookup.FilldessionResult(), ref lstJudgmentresult, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FilldessionResult(), ref lstfilterJudgmentresult, "NameAr", "Code", "الكل");

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

        #region "Persons"

        private void SavePersons(int CaseCode)
        {

            if (Session["objprosecutor"] != null)
            {
                List<Case_parties> _PersonsList = new List<Case_parties>();

                _PersonsList = (List<Case_parties>)Session["objprosecutor"];

                for (int i = 0; i < _PersonsList.Count; i++)
                {


                    if (_PersonsList[i].Code == 0 || _PersonsList[i].Code == -1)
                    {//Insert
                        _PersonsList[i].CaseID = CaseCode;
                        //Adding Child Enitty

                        objRepository.AddPerson(_PersonsList[i]);

                    }
                    else

                    {//Update
                        var objForUpdate = objRepository.getcasePartyDetails(_PersonsList[i].Code);
                        objForUpdate.CaseID = CaseCode;
                        objForUpdate.FullName = _PersonsList[i].FullName;
                        objForUpdate.Notes = _PersonsList[i].Notes;
                        objRepository.UpdatePersons(objForUpdate);
                    }

                }


            }
            /**********************************************/

            if (Session["objDefendant"] != null)
            {
                List<Case_parties> _PersonsList = new List<Case_parties>();

                _PersonsList = (List<Case_parties>)Session["objDefendant"];

                for (int i = 0; i < _PersonsList.Count; i++)
                {


                    if (_PersonsList[i].Code == 0 || _PersonsList[i].Code == -1)
                    {//Insert
                        _PersonsList[i].CaseID = CaseCode;
                        //Adding Child Enitty

                        objRepository.AddPerson(_PersonsList[i]);

                    }
                    else

                    {//Update
                     // _PersonsList[i].CaseID = CaseCode;
                     // objRepository.UpdatePersons(_PersonsList[i]);

                        var objForUpdate = objRepository.getcasePartyDetails(_PersonsList[i].Code);
                        objForUpdate.CaseID = CaseCode;
                        objForUpdate.FullName = _PersonsList[i].FullName;
                        objForUpdate.Notes = _PersonsList[i].Notes;
                        objRepository.UpdatePersons(objForUpdate);
                    }

                }


            }

            Session["PersonsList"] = null;
            Session["objDefendant"] = null;
            Session["objprosecutor"] = null;
        }


        private bool CheckPersonCount()
        {

            if (Session["objprosecutor"] != null)
            {
                List<Case_parties> _PersonsList = new List<Case_parties>();

                _PersonsList = (List<Case_parties>)Session["objprosecutor"];

                if (_PersonsList.Count > 0)
                {
                    return true;
                }

            }
            else if (Session["objDefendant"] != null)
            {
                List<Case_parties> _PersonsList = new List<Case_parties>();

                _PersonsList = (List<Case_parties>)Session["objDefendant"];

                if (_PersonsList.Count > 0)
                {
                    return true;
                }


            }
            return false;
        }
        protected void btnAddNewItem_Click(object sender, EventArgs e)
        {

        }


        protected void grdprosecutor_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                ViewState["SpEdit"] = "0";

                List<Case_parties> objprosecutor = new List<Case_parties>();
                if (Session["objprosecutor"] != null)
                {
                    objprosecutor = (List<Case_parties>)Session["objprosecutor"];
                }

                Case_parties _personobj = new Case_parties();

                _personobj.Code = -1;
                _personobj.PartyType = 1;
                _personobj.AddDate = DateTime.Now;
                _personobj.LastUpdate = DateTime.Now;
                _personobj.FullName = gets(((TextBox)e.Item.FindControl("txtname")).Text);
                _personobj.Notes = gets(((TextBox)e.Item.FindControl("txtNotes")).Text);
                _personobj.CivilID = gets(((TextBox)e.Item.FindControl("txtCivilID")).Text);

                _personobj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());





                objprosecutor.Add(_personobj);
                Session["objprosecutor"] = objprosecutor;
                //  grdPersons.EditItemIndex = -1;

                grdprosecutor.EditItemIndex = objprosecutor.Count;

                grdprosecutor.DataSource = AddDefaultItems(objprosecutor);
                grdprosecutor.DataBind();
                //  FillMedalPersons(ZeroIntergerIFNull(hdnMasterID.Value));

                // FillMedalPersons(ZeroIntergerIFNull(Request.QueryString["id"].ToString()));
            }
            else if (e.CommandName.Equals("Edit"))
            {
                ViewState["SpEdit"] = 1;
                grdprosecutor.EditItemIndex = e.Item.ItemIndex;
                // grdPersons.DataBind();


                List<Case_parties> objprosecutor = new List<Case_parties>();
                if (Session["objprosecutor"] != null)
                {
                    objprosecutor = (List<Case_parties>)Session["objprosecutor"];
                }

                grdprosecutor.DataSource = AddDefaultItems(objprosecutor);
                grdprosecutor.DataBind();

            }
            else if (e.CommandName.Equals("Update"))
            {

                List<Case_parties> PersonsList = new List<Case_parties>();
                if (Session["objprosecutor"] != null)
                {
                    PersonsList = (List<Case_parties>)Session["objprosecutor"];
                }



                Case_parties _personobj = new Case_parties();
                _personobj = PersonsList[e.Item.ItemIndex];

                _personobj.FullName = gets(((TextBox)e.Item.FindControl("txtname")).Text);
                _personobj.Notes = gets(((TextBox)e.Item.FindControl("txtNotes")).Text);
                _personobj.LastUpdate = DateTime.Now;
                _personobj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                PersonsList[e.Item.ItemIndex] = _personobj;
                Session["objprosecutor"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;
                ViewState["SpEdit"] = 0;

                grdprosecutor.EditItemIndex = PersonsList.Count;

                grdprosecutor.DataSource = AddDefaultItems(PersonsList);
                grdprosecutor.DataBind();
            }
            else if (e.CommandName.Equals("Delete"))
            {
                string code = e.Item.Cells[2].Text.Replace("&nbsp;", " ").Trim();

                List<Case_parties> PersonsList = new List<Case_parties>();
                if (Session["objprosecutor"] != null)
                {
                    PersonsList = (List<Case_parties>)Session["objprosecutor"];
                }
                if (code.ToString() != "0" && code.ToString() != "-1")
                {
                    var PartiesToDelete = objRepository.getcasePartyDetails(ZeroIntergerIFNull(code));
                    objRepository.DeletePersons(PartiesToDelete);

                }

                PersonsList.RemoveAt(e.Item.ItemIndex);
                Session["objprosecutor"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;
                ViewState["SpEdit"] = 0;

                grdprosecutor.EditItemIndex = PersonsList.Count;

                grdprosecutor.DataSource = AddDefaultItems(PersonsList);
                grdprosecutor.DataBind();
            }
            else if (e.CommandName.Equals("Cancel"))
            {
                ViewState["SpEdit"] = 0;

                List<Case_parties> PersonsList = new List<Case_parties>();
                if (Session["objprosecutor"] != null)
                {
                    PersonsList = (List<Case_parties>)Session["objprosecutor"];
                }
                grdprosecutor.EditItemIndex = PersonsList.Count;

                grdprosecutor.DataSource = AddDefaultItems(PersonsList);
                grdprosecutor.DataBind();
            }
        }

        protected void grdprosecutor_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {

                TextBox txtname = (TextBox)e.Item.FindControl("txtname");
                TextBox txtNotes = (TextBox)e.Item.FindControl("txtNotes");
                TextBox txtCivilID = (TextBox)e.Item.FindControl("txtCivilID");


                HtmlInputHidden hdnPerson_NameAr = (HtmlInputHidden)e.Item.FindControl("hdnPerson_NameAr");
                //HtmlInputHidden hdnGradeID = (HtmlInputHidden)e.Item.FindControl("hdnGradeID");
                HtmlInputHidden hdnCivilID = (HtmlInputHidden)e.Item.FindControl("hdnCivilID");
                HtmlInputHidden hdnNoteText = (HtmlInputHidden)e.Item.FindControl("hdnNotes");


                if (hdnPerson_NameAr.Value != "")
                {
                    txtname.Text = hdnPerson_NameAr.Value;
                }


                if (hdnNoteText.Value != "" && hdnNoteText.Value != "0")
                {
                    txtNotes.Text = hdnNoteText.Value;
                }
                if (hdnCivilID.Value != "" && hdnCivilID.Value != "0")
                {
                    txtCivilID.Text = hdnCivilID.Value;
                }

                e.Item.Cells[1].Visible = false;
                e.Item.Cells[0].Attributes.Add("colspan", "2");

                LinkButton lnkUpdate = (LinkButton)(e.Item.Cells[0].FindControl("lnkUpdate"));
                LinkButton lnkAdd = (LinkButton)(e.Item.Cells[0].FindControl("lnkAdd"));
                LinkButton lnkCancel = (LinkButton)(e.Item.Cells[0].FindControl("lnkCancel"));

                lnkAdd.Attributes.Add("onclick", "return LinkAddClickgrdprosecutor();");
                if (ViewState["SpEdit"].ToString() == "0")//' Inseting New Row Mode
                {
                    lnkUpdate.Visible = false;
                    lnkCancel.Visible = false;
                    lnkAdd.Visible = true;
                    lnkAdd.Attributes.Add("onclick", "return LinkAddClickgrdprosecutor();");
                }
                else
                {
                    lnkUpdate.Visible = true;
                    lnkCancel.Visible = true;
                    lnkAdd.Visible = false;


                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string code = e.Item.Cells[2].Text.Replace("&nbsp;", " ").Trim();
                LinkButton lnkEdit = (LinkButton)(e.Item.Cells[0].FindControl("lnkEdit"));
                LinkButton lnkDelete = (LinkButton)(e.Item.Cells[0].FindControl("lnkDelete"));

                if (code.ToString().Equals("0"))
                {

                    lnkEdit.Visible = false;
                    lnkDelete.Visible = false;
                }
                else
                {
                    lnkEdit.Visible = true;
                    lnkDelete.Visible = true;



                }

            }

        }
        protected void grdDefendant_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {

                TextBox txtname = (TextBox)e.Item.FindControl("txtDname");
                TextBox txtNotes = (TextBox)e.Item.FindControl("txtNotes");


                HtmlInputHidden hdnPerson_NameAr = (HtmlInputHidden)e.Item.FindControl("hdnPerson_NameAr");
                //HtmlInputHidden hdnGradeID = (HtmlInputHidden)e.Item.FindControl("hdnGradeID");
                //HtmlInputHidden hdnCivilID = (HtmlInputHidden)e.Item.FindControl("hdnCivilID");
                HtmlInputHidden hdnNoteText = (HtmlInputHidden)e.Item.FindControl("hdnNotes");


                if (hdnPerson_NameAr.Value != "")
                {
                    txtname.Text = hdnPerson_NameAr.Value;
                }


                if (hdnNoteText.Value != "" && hdnNoteText.Value != "0")
                {
                    txtNotes.Text = hdnNoteText.Value;
                }


                e.Item.Cells[1].Visible = false;
                e.Item.Cells[0].Attributes.Add("colspan", "2");

                LinkButton lnkUpdate = (LinkButton)(e.Item.Cells[0].FindControl("lnkUpdate"));
                LinkButton lnkAdd = (LinkButton)(e.Item.Cells[0].FindControl("lnkAdd"));
                LinkButton lnkCancel = (LinkButton)(e.Item.Cells[0].FindControl("lnkCancel"));

                lnkAdd.Attributes.Add("onclick", "return LinkAddClick();");
                if (ViewState["SpEdit"].ToString() == "0")//' Inseting New Row Mode
                {
                    lnkUpdate.Visible = false;
                    lnkCancel.Visible = false;
                    lnkAdd.Visible = true;
                    lnkAdd.Attributes.Add("onclick", "return LinkAddClick();");
                }
                else
                {
                    lnkUpdate.Visible = true;
                    lnkCancel.Visible = true;
                    lnkAdd.Visible = false;


                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string code = e.Item.Cells[2].Text.Replace("&nbsp;", " ").Trim();
                LinkButton lnkEdit = (LinkButton)(e.Item.Cells[0].FindControl("lnkEdit"));
                LinkButton lnkDelete = (LinkButton)(e.Item.Cells[0].FindControl("lnkDelete"));

                if (code.ToString().Equals("0"))
                {

                    lnkEdit.Visible = false;
                    lnkDelete.Visible = false;
                }
                else
                {
                    lnkEdit.Visible = true;
                    lnkDelete.Visible = true;



                }

            }

        }

        protected void grdDefendant_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                ViewState["SpEdit"] = "0";

                List<Case_parties> PersonsList = new List<Case_parties>();
                if (Session["objDefendant"] != null)
                {
                    PersonsList = (List<Case_parties>)Session["objDefendant"];
                }




                Case_parties _personobj = new Case_parties();



                _personobj.Code = -1;
                _personobj.PartyType = 2;
                _personobj.FullName = gets(((TextBox)e.Item.FindControl("txtDname")).Text);
                _personobj.Notes = gets(((TextBox)e.Item.FindControl("txtDNotes")).Text);
                _personobj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                _personobj.AddDate = DateTime.Now;
                _personobj.LastUpdate = DateTime.Now;


                PersonsList.Add(_personobj);
                Session["objDefendant"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;

                grdDefendant.EditItemIndex = PersonsList.Count;

                grdDefendant.DataSource = AddDefaultItems(PersonsList);
                grdDefendant.DataBind();
                //  FillMedalPersons(ZeroIntergerIFNull(hdnMasterID.Value));

                // FillMedalPersons(ZeroIntergerIFNull(Request.QueryString["id"].ToString()));
            }
            else if (e.CommandName.Equals("Edit"))
            {
                ViewState["SpEdit"] = 1;
                grdDefendant.EditItemIndex = e.Item.ItemIndex;
                // grdPersons.DataBind();


                List<Case_parties> PersonsList = new List<Case_parties>();
                if (Session["objDefendant"] != null)
                {
                    PersonsList = (List<Case_parties>)Session["objDefendant"];
                }

                grdDefendant.DataSource = AddDefaultItems(PersonsList);
                grdDefendant.DataBind();

            }
            else if (e.CommandName.Equals("Update"))
            {

                List<Case_parties> PersonsList = new List<Case_parties>();
                if (Session["objDefendant"] != null)
                {
                    PersonsList = (List<Case_parties>)Session["objDefendant"];
                }



                Case_parties _personobj = new Case_parties();
                _personobj = PersonsList[e.Item.ItemIndex];




                _personobj.FullName = gets(((TextBox)e.Item.FindControl("txtDname")).Text);
                _personobj.Notes = gets(((TextBox)e.Item.FindControl("txtDNotes")).Text);
                _personobj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                _personobj.LastUpdate = DateTime.Now;

                PersonsList[e.Item.ItemIndex] = _personobj;
                Session["objDefendant"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;
                ViewState["SpEdit"] = 0;

                grdDefendant.EditItemIndex = PersonsList.Count;

                grdDefendant.DataSource = AddDefaultItems(PersonsList);
                grdDefendant.DataBind();
            }
            else if (e.CommandName.Equals("Delete"))
            {
                string code = e.Item.Cells[2].Text.Replace("&nbsp;", " ").Trim();

                List<Case_parties> PersonsList = new List<Case_parties>();
                if (Session["objDefendant"] != null)
                {
                    PersonsList = (List<Case_parties>)Session["objDefendant"];
                }

                if (code.ToString() != "0" && code.ToString() != "-1")
                {
                    var PartiesToDelete = objRepository.getcasePartyDetails(ZeroIntergerIFNull(code));
                    objRepository.DeletePersons(PartiesToDelete);

                }



                PersonsList.RemoveAt(e.Item.ItemIndex);




                Session["objDefendant"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;
                ViewState["SpEdit"] = 0;

                grdDefendant.EditItemIndex = PersonsList.Count;

                grdDefendant.DataSource = AddDefaultItems(PersonsList);
                grdDefendant.DataBind();
            }
            else if (e.CommandName.Equals("Cancel"))
            {
                ViewState["SpEdit"] = 0;

                List<Case_parties> PersonsList = new List<Case_parties>();
                if (Session["objDefendant"] != null)
                {
                    PersonsList = (List<Case_parties>)Session["objDefendant"];
                }
                grdDefendant.EditItemIndex = PersonsList.Count;
                grdDefendant.DataSource = AddDefaultItems(PersonsList);
                grdDefendant.DataBind();
            }
        }
        #endregion

        #region "Coming Letters"
        #region "Coming HElpers"
        private void FillinComingForm()
        {

            var objList = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["CasesArcID"].ToString()));
            if ((objList != null))
            {
                txtDoc_Serial.Text = gets(objList.Doc_Serial);
                txtDoc_Subject.Text = gets(objList.Doc_Subject);
                txtComingNotes.Text = gets(objList.Doc_Notes);

                txtFrom.Text = gets(objList.Doc_From);
                txtComingDate.Text = NullDateifEmptyToText(objList.SentDate);
                txtComingReminderDate.Text = NullDateifEmptyToText(objList.NextFollowReminderDate);

                chkHasHearing.Checked = getBool(objList.IsHearing);

                if (!gets(objList.Filepath).Equals(""))
                {
                    btnIncomingScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnIncomingScannerfilepath.Value = gets(objList.Filepath);
                }

                if (chkHasHearing.Checked)
                {
                    chkHasHearing.Enabled = false;

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

                if (ViewState["CasesArcID"].Equals("0"))
                {//Save

                    obj.TransactionDate = DateTime.Now;
                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.CasesModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtDoc_Serial.Text);
                    obj.Doc_Subject = gets(txtDoc_Subject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = txtFrom.Text;
                    obj.Doc_To = "CMGS";
                    obj.Doc_Notes = txtComingNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtComingDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtComingReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

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
                    obj.TargetModule = (int)ArcTargetModules.CasesModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtDoc_Serial.Text);
                    obj.Doc_Subject = gets(txtDoc_Subject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = txtFrom.Text;
                    obj.Doc_To = "CMGS";
                    obj.Doc_Notes = txtComingNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtComingDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtComingReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

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
                        if (obj.HearingID != null)
                        {
                            objHearing = objRepository.GetHearingDetails(obj.HearingID);
                            objHearing.CaseID = RefDocID;

                            objHearing.HearningDate = NullDateifEmpty(txtHeadingDate.Text);
                            objRepository.UpdateHearing(objHearing);
                        }

                    }

                    objRepository.UpdateArcData(obj);

                }



                ViewState["CasesArcID"] = obj.Code;

                string folderpath = obj.RefDocID.ToString() + "/" + (obj.Doc_Type == 1 ? "incoming/" : "outgoing/") + obj.Code.ToString() + "/";
                string _img = UploadFileoServer(txtIncomingImge, ScannerRepository + _TargetUploadPath + folderpath);
                if (_img != "")
                {
                    var objAcrForUpdate = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["CasesArcID"].ToString()));
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
            FilLCaseHearing(RefDocID);
            return obj.Code;
        }
        protected void grdincoming_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["CasesArcID"] = e.Item.Cells[0].Text;
            divAddIncoming.Visible = false;
            FillinComingForm();

        }

        #endregion

        protected void Lnkincoming_Click(object sender, EventArgs e)
        {
            ViewState["outgoingCode"] = "0";
            ViewState["CasesArcID"] = "0";

            divAddIncoming.Visible = true;
            divshowincoming.Visible = false;
        }

        protected void lnkSaveIncoming_Click(object sender, EventArgs e)
        {
            if (ViewState["CaseitemID"].ToString() != "0" && ViewState["CaseitemID"].ToString() != "")
            {
                SaveComingDocInformation(1, ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));
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
            FillArcData(1, ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));
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
                FillArcData(1, ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));

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

                if (ViewState["CasesArcID"].Equals("0"))
                {//Save

                    obj.TransactionDate = DateTime.Now;
                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.CasesModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtOutDocNo.Text);
                    obj.Doc_Subject = gets(txtOutSubject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = "CMGS";
                    obj.Doc_To = txtto.Text;
                    obj.Doc_Notes = txtoutNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtoutDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtOutReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                    objRepository.AddArcData(obj);

                    ViewState["CasesArcID"] = gets(obj.Code);


                }
                else
                { //Update


                    obj = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["CasesArcID"].ToString()));


                    obj.lastTransDate = DateTime.Now;
                    obj.TargetModule = (int)ArcTargetModules.CasesModules;
                    obj.Doc_Type = DirectionType;
                    obj.Doc_Serial = gets(txtOutDocNo.Text);
                    obj.Doc_Subject = gets(txtOutSubject.Text);
                    obj.RefDocID = RefDocID;
                    obj.Doc_From = "CMGS";
                    obj.Doc_To = txtto.Text;
                    obj.Doc_Notes = txtoutNotes.Text;
                    obj.SentDate = NullDateifEmpty(txtoutDate.Text);
                    obj.NextFollowReminderDate = NullDateifEmpty(txtOutReminderDate.Text);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    objRepository.UpdateArcData(obj);

                }

                ViewState["CasesArcID"] = obj.Code;

                string folderpath = obj.RefDocID.ToString() + "/" + (obj.Doc_Type == 1 ? "incoming/" : "outgoing/") + obj.Code.ToString() + "/";
                string _img = UploadFileoServer(txtoutgoiningImage, ScannerRepository + _TargetUploadPath + folderpath);
                if (_img != "")
                {
                    var objAcrForUpdate = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["CasesArcID"].ToString()));
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
            ViewState["CasesArcID"] = "0";



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
            ViewState["CasesArcID"] = "0";

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
                FillArcData(2, ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));

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
            if (ViewState["CaseitemID"].ToString() != "0" && ViewState["CaseitemID"].ToString() != "")
            {
                SaveOutDocInformation(2, ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));
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
            FillArcData(2, ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));
        }

        private void FillOutForm()
        {

            var objList = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["CasesArcID"].ToString()));
            if ((objList != null))
            {


                if (!gets(objList.Filepath).Equals(""))
                {
                    btnOutgoingScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnoutgiongScannerfilepath.Value = gets(objList.Filepath);
                }

                txtOutDocNo.Text = gets(objList.Doc_Serial);
                txtOutSubject.Text = gets(objList.Doc_Subject);
                txtoutNotes.Text = gets(objList.Doc_Notes);

                txtto.Text = gets(objList.Doc_To);
                txtoutDate.Text = NullDateifEmptyToText(objList.SentDate);
                txtOutReminderDate.Text = NullDateifEmptyToText(objList.NextFollowReminderDate);


            }

            divOutgiongAdd.Visible = true;
            divoutgoingshow.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        protected void grdOutgoing_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["CasesArcID"] = e.Item.Cells[0].Text;
            divOutgiongAdd.Visible = false;
            FillOutForm();
        }
        #endregion

        #region "Case Attachment"


        protected void Attachepager_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdCaseAttachment.PageCount))
            {
                currnetPageIndx = (grdCaseAttachment.PageCount - 1);
            }

            pager4.CurrentIndex = currnetPageIndx;
            grdCaseAttachment.CurrentPageIndex = (currnetPageIndx - 1);
            FillFileAttachment(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
        }
        protected void btnAttDelete_Click(object sender, System.EventArgs e)
        {
            try
            {

                arc_Attachments obj = new arc_Attachments();
                for (int i = 0; i <= grdCaseAttachment.Items.Count - 1; i++)
                {

                    if ((grdCaseAttachment.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdCaseAttachment.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteAttachment((arc_Attachments)objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(grdCaseAttachment.Items[i].Cells[0].Text)));
                        }
                    }
                }
                FillFileAttachment(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteDataFailed + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

        }

        protected void grdCaseAttachment_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;

            ViewState["AttachitemID"] = id;
            FillAttachmentForm();
            tblshow.Visible = false;
            tblAdd.Visible = true;
        }

        protected void grdCaseAttachment_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
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
        protected void btnAttachSave_Click(object sender, System.EventArgs e)
        {
            SaveAttachmentInformation();
        }

        protected void btnAttCancel_Click(object sender, System.EventArgs e)
        {
            ClearAttacheForm();
        }

        protected void btnNewAttachement_Click(object sender, EventArgs e)
        {

            this.ClearAttacheForm();
            divAddAttachment.Visible = true;
            DivAttachmentShow.Visible = false;
        }


        #region "Fill Information"
        private int SaveAttachmentInformation()
        {
            string script = "";
            arc_Attachments obj = new arc_Attachments();
            try
            {

                if (gets(ViewState["AttachitemID"]).Equals("0"))
                {//Save
                    obj.FileID = ZeroIntergerIFNull(ViewState["itemID"].ToString());
                    obj.TypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    obj.ReceiveDate = NullDateifEmpty(txtAttachCreationDate.Text);
                    obj.UploadDate = DateTime.Now;
                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    obj.AttacheSubject = txtAttachSubject.Text;
                    obj.AttachRef = txtAttachRef.Text;

                    objRepository.AddAttachement(obj);
                }
                else
                { //Update
                    obj = objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(ViewState["AttachitemID"].ToString()));

                    obj.FileID = ZeroIntergerIFNull(ViewState["itemID"].ToString());
                    obj.TypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    obj.ReceiveDate = NullDateifEmpty(txtAttachCreationDate.Text);
                    obj.AttacheSubject = txtAttachSubject.Text;
                    obj.AttachRef = txtAttachRef.Text;
                    obj.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;


                    objRepository.UpdateAttachment(obj);

                }
                //Upload LocalFile
                if (txtImage.FileName != "")
                {

                    string _img = UploadFileoServer(txtImage, ScannerRepository + _TargetUploadPath + gets(obj.Code) + "/");
                    if (_img != "")
                    {
                        var objForEdit = objRepository.GetAttachemtnDetails(obj.Code);
                        objForEdit.Filepath = _img;
                        objRepository.UpdateAttachment(objForEdit);
                    }
                }

                ClearAttacheForm();
                FillFileAttachment(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

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
        private void FillFileAttachment(int FileID)
        {
            var objList = objRepository.FillFileAttachemnt(FileID);
            lblAttachcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
            decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdCaseAttachment.PageSize));
            if ((c <= grdCaseAttachment.CurrentPageIndex))
            {
                grdCaseAttachment.CurrentPageIndex = 0;
            }

            grdCaseAttachment.DataSource = objList;
            grdCaseAttachment.DataBind();
            int _totalCount = objList.Count;
            pager1.ItemCount = _totalCount;

        }

        private void FillAttachmentForm()
        {
            var objList = objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(ViewState["AttachitemID"].ToString()));
            if ((objList != null))
            {


                lstAttachmentType.SelectedValue = gets(objList.TypeCode);
                txtAttachRef.Text = gets(objList.AttachRef);
                txtAttachSubject.Text = gets(objList.AttacheSubject);
                txtAttachCreationDate.Text = gets(objList.ReceiveDate);
                lblimage.Text = "<a target='_blank' href='" + ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList=[" + gets(objList.Filepath) + ";]'>View File</a>";
            }

            divAddAttachment.Visible = true;


        }
        private void ClearAttacheForm()
        {
            txtAttachCreationDate.Text = "";
            txtAttachRef.Text = "";
            txtAttachSubject.Text = "";

            ViewState["AttachitemID"] = 0;
            divAddAttachment.Visible = false;
            DivAttachmentShow.Visible = true;

        }
        public string showattachment(string hasattachment)
        {
            if (hasattachment != "")
            {
                return "";
            }
            return "display:none";

        }
        #endregion

        #region "Scanning"
        protected void lnkScan_Click(object sender, EventArgs e)
        {

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";

            int _TargetID = SaveAttachmentInformation();

            if (_TargetID != 0)
            {

                //ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + ViewState["DocID"].ToString() + "' />";
                //ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + ViewState["CaseID"].ToString() + "' />";
                //ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";

                //target='_blank'
                string ScannerPostFrom = "<form id='ScannerCalllerForm'   action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='1' />";
                ScannerPostFrom += "<input type='hidden' id='AttachID' name='AttachID' value='" + _TargetID + "' />";
                //ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";

                //ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                //ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                //ScannerPostFrom += "<input type='hidden' id='id' name='id' value='" + ZeroIntergerIFNull(ViewState["itemID"].ToString()).ToString() + "' />";


                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + ViewState["CaseitemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["itemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";


                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += "  CalllerForm.submit();";
                ScannerPostFrom += "</script>";

                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;

                //_TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&AttachID=" + _TargetID  + "&TargetID=" + _TargetID;
                //Response.Redirect(_TargetUrl);
            }

        }

        private void UpdateScannedFile()
        {

            //Parliament_Questions objQuestion = new Parliament_Questions();
            Cases_H_Hearing objHearing = new Cases_H_Hearing();

            arc_Data objArc = new arc_Data();
            if (Request.Form["fileList"] != null)
            {
                if (Request.Form["fileList"].ToString() != "")
                {

                    switch (Request.Form["ActiveTab"].ToString())
                    {
                        case "1":
                            {//Question Information


                                //ViewState["itemID"] = Request.Form["TargetID"];
                                ////  FillCaseMasterInformation();
                                //hdnactivetab.Value = "1";

                                //string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                //_ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                //string[] FileList = _ScannerFileLlisy.Split(';');

                                //if (Request.Form["TargetID"] != null)
                                //{
                                //    for (int i = 0; i < FileList.Length; i++)
                                //    {//Update Current Record With

                                //        objQuestion = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                //        objQuestion.Q_Attachment = FileList[i].Split(',')[0].ToString();
                                //        objRepository.UpdateQuestion(objQuestion);

                                //    }
                                //    Response.Redirect("CasesData.aspx?activetab=1&QuestionID=" + Request.Form["TargetID"]);
                                //    return;
                                //}


                                //else
                                //{
                                //    string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
                                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                //}


                                break;
                            }
                        case "2":
                            {// Hearing Details


                                ViewState["HearingCode"] = Request.Form["TargetID"];
                                ViewState["itemID"] = Request.Form["FileID"];
                                //  FillCaseMasterInformation();
                                hdnactivetab.Value = "2";

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
                                            objHearing = objRepository.GetHearingDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objHearing.HearingAttachment = null;
                                            objRepository.UpdateHearing(objHearing);
                                        }
                                        else
                                        {
                                            objHearing = objRepository.GetHearingDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objHearing.HearingAttachment = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateHearing(objHearing);
                                        }


                                    }
                                    Response.Redirect("CasesData.aspx?activetab=2&FileID=" + Request.Form["FileID"] + "&CaseID=" + Request.Form["CaseID"]);
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


                                ViewState["CasesArcID"] = Request.Form["TargetID"];
                                ViewState["itemID"] = Request.Form["FileID"];
                                //  FillCaseMasterInformation();
                                hdnactivetab.Value = "3";

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
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["CasesArcID"].ToString()));
                                            objArc.Filepath = null;
                                            objRepository.UpdateArcData(objArc);

                                        }
                                        else
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["CasesArcID"].ToString()));
                                            objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateArcData(objArc);
                                        }


                                    }

                                    Response.Redirect("CasesData.aspx?activetab=3&FileID=" + Request.Form["FileID"] + "&CaseID=" + Request.Form["CaseID"]);
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

                                ViewState["CasesArcID"] = Request.Form["TargetID"];
                                ViewState["itemID"] = Request.Form["FileID"];
                                //  FillCaseMasterInformation();
                                hdnactivetab.Value = "4";

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
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["CasesArcID"].ToString()));
                                            objArc.Filepath = null;
                                            objRepository.UpdateArcData(objArc);
                                        }
                                        else
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["CasesArcID"].ToString()));
                                            objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateArcData(objArc);
                                        }


                                    }
                                    Response.Redirect("CasesData.aspx?activetab=4&FileID=" + Request.Form["FileID"] + "&CaseID=" + Request.Form["CaseID"]);
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


            //----------------------------------------

            //arc_Attachments obj = new arc_Attachments();
            //if (Request.Form["fileList"] != null)
            //{
            //    if (Request.Form["fileList"].ToString() != "")
            //    {


            //        ViewState["itemID"] = Request.Form["FileID"];
            //      //  FillCaseMasterInformation();
            //        hdnactivetab.Value = "5";

            //        string _ScannerFileLlisy = Request.Form["fileList"].ToString();
            //        _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
            //        string[] FileList = _ScannerFileLlisy.Split(';');

            //        if (Request.Form["TargetID"] != null)
            //        {
            //            for (int i = 0; i < FileList.Length; i++)
            //            {//Update Current Record With

            //                obj = objRepository.GetAttachemtnDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
            //                obj.Filepath = FileList[i].Split(',')[0].ToString();
            //                objRepository.UpdateAttachment(obj);

            //            }

            //        }

            //        //string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
            //        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            //        Response.Redirect("CasesData.aspx?activetab=5&FileID="+ Request.Form["FileID"] + "&CaseID="+ Request.Form["CaseID"]);
            //    }
            //    else
            //    {
            //        string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            //    }


            //}


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
            if (ViewState["CaseitemID"].ToString() != "0" && ViewState["CaseitemID"].ToString() != "")
            {
                _TargetID = SaveComingDocInformation(1, ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Question Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["CaseitemID"].ToString() + "/incoming/" + _TargetID.ToString() + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnIncomingScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnIncomingScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + ViewState["CaseitemID"] + "' />";
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
            if (ViewState["CaseitemID"].ToString() != "0" && ViewState["CaseitemID"].ToString() != "")
            {
                _TargetID = SaveOutDocInformation(2, ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Question Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["CaseitemID"].ToString() + "/outgoing/" + _TargetID.ToString() + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnoutgiongScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnoutgiongScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + ViewState["CaseitemID"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["itemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='4' />";
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


        #endregion

        #endregion

        #region "Hearing"
        #region "Hearing HElpers"
        private void FillHearingForm()
        {

            var objList = objRepository.GetHearingDetails(ZeroIntergerIFNull(ViewState["HearingCode"].ToString()));
            if ((objList != null))
            {
                txtHearingNotes.Text = gets(objList.Note);
                txtHearingDate.Text = NullDateifEmptyToText(objList.HearningDate);

                txtHearingWriter.Text = gets(objList.HearingWriter);
                txtHearingDecisionText.Text = gets(objList.DecisionText);
                txthearingConsultant.Text = gets(objList.CaseConsulatName);


                if (!gets(objList.HearingAttachment).Equals(""))
                {
                    btnHearingScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnHearingScannerfilepath.Value = gets(objList.HearingAttachment);
                }

            }

            DivAddHearing.Visible = true;
            divShowHearing.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private void FilLCaseHearing(int DocRefID)
        {

            var objList = objRepository.FillCaseHearing(DocRefID);
            //  lblComingCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
            lblHearingcount.Text = objList.Count.ToString();
            decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdHraingList.PageSize));
            if ((c <= grdHraingList.CurrentPageIndex))
            {
                grdHraingList.CurrentPageIndex = 0;
            }

            //List Duplication
            //List<View_InboundItems> duplicatedList = new List<View_InboundItems>();
            //duplicatedList = DuplicatedList(objList);

            if (objList.Count > 0)
            {
                //btnSave.Visible = true;
                //lnkBack.Visible = true;

                divShowHearing.Visible = true;
                pager5.Visible = true;
                grdHraingList.Visible = true;

            }


            //var duplicatedList = objList.SelectMany(t =>
            //  Enumerable.Repeat(t, 2)).ToList();

            grdHraingList.DataSource = objList;
            grdHraingList.DataBind();
            pager5.ItemCount = objList.Count;
        }
        private void ClearHearingforms()
        {
            ViewState["HearingCode"] = "0";
            txtHearingDate.Text = "";
            txtHearingNotes.Text = "";


            DivAddHearing.Visible = false;
            divShowHearing.Visible = true;



        }
        private int SaveHearingInformation(int RefDocID)
        {
            string script = "";
            Cases_H_Hearing obj = new Cases_H_Hearing();
            try
            {

                if (ViewState["HearingCode"].Equals("0"))
                {//Save

                    obj.CreationDate = DateTime.Now;
                    obj.CaseID = RefDocID;
                    obj.Note = txtHearingNotes.Text;
                    obj.HearningDate = NullDateifEmpty(txtHearingDate.Text);

                    obj.HearingWriter = gets(txtHearingWriter.Text);
                    obj.DecisionText = gets(txtHearingDecisionText.Text);
                    obj.CaseConsulatName = gets(txthearingConsultant.Text);


                    objRepository.AddHearing(obj);

                }
                else
                { //Update


                    obj = objRepository.GetHearingDetails(ZeroIntergerIFNull(ViewState["HearingCode"].ToString()));
                    obj.CaseID = RefDocID;
                    obj.Note = txtHearingNotes.Text;
                    obj.HearningDate = NullDateifEmpty(txtHearingDate.Text);

                    obj.HearingWriter = gets(txtHearingWriter.Text);
                    obj.DecisionText = gets(txtHearingDecisionText.Text);
                    obj.CaseConsulatName = gets(txthearingConsultant.Text);


                    objRepository.UpdateHearing(obj);

                }

                ViewState["HearingCode"] = obj.Code;
                string folderpath = obj.CaseID.ToString() + "/hearing/" + obj.Code.ToString() + "/";
                string _img = UploadFileoServer(txtHearingimage, ScannerRepository + _TargetUploadPath + folderpath);
                if (_img != "")
                {
                    var objForUpdate = objRepository.GetHearingDetails(ZeroIntergerIFNull(ViewState["HearingCode"].ToString()));
                    objForUpdate.HearingAttachment = _img;
                    objRepository.UpdateArcData(objForUpdate);
                }



                ClearHearingforms();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            FilLCaseHearing(RefDocID);
            return obj.Code;
        }


        #endregion
        protected void grdHraingList_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["HearingCode"] = e.Item.Cells[0].Text;
            DivAddHearing.Visible = true;
            divShowHearing.Visible = false;
            FillHearingForm();

        }
        protected void lnkAddHearing_Click(object sender, EventArgs e)
        {
            ViewState["HearingCode"] = "0";
            DivAddHearing.Visible = true;
            divShowHearing.Visible = false;
        }

        protected void lnkSaveHearing_Click(object sender, EventArgs e)
        {
            if (ViewState["CaseitemID"].ToString() != "0" && ViewState["CaseitemID"].ToString() != "")
            {
                SaveHearingInformation(ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Case Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }


        }

        protected void lnkCancelHearing_Click(object sender, EventArgs e)
        {
            ClearHearingforms();
        }

        protected void pager_Command5(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdHraingList.PageCount))
            {
                currnetPageIndx = (grdHraingList.PageCount - 1);
            }

            pager5.CurrentIndex = currnetPageIndx;
            grdHraingList.CurrentPageIndex = (currnetPageIndx - 1);
            FilLCaseHearing(ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));
        }
        protected void grdHraingList_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }

        protected void lnkDeleteHearing_Click(object sender, EventArgs e)
        {
            try
            {

                Cases_H_Hearing obj = new Cases_H_Hearing();
                for (int i = 0; i <= grdHraingList.Items.Count - 1; i++)
                {

                    if ((grdHraingList.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdHraingList.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteHearing((Cases_H_Hearing)objRepository.GetHearingDetails(ZeroIntergerIFNull(grdHraingList.Items[i].Cells[0].Text)));
                        }
                    }
                }
                FilLCaseHearing(ZeroIntergerIFNull(ViewState["CaseitemID"].ToString()));

            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteDataFailed + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
        }

        #endregion

    }
}