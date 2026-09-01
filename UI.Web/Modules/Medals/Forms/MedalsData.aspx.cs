using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ExcelDataReader;
using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using Newtonsoft.Json;
using UI.Web.Admin.Controller;

namespace UI.Web.Medals.Forms
{
    public partial class MedalsData : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public MedalsRepository objRepository = IoC.Resolve<MedalsRepository>();
        public string _PageTitle = "الانواط والاوسمه ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "MedalAttachments/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();


        #endregion

        #region "Page Events"

        protected void Page_PreRender(object sender, EventArgs e)
        {
            AduptPersonGrd();
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
            lnkScan.Attributes.Add("onclick", "return chkProcedure();");


            if (!IsPostBack)
            {


                if (Request.QueryString["activetab"] != null)
                {
                    hdnactivetab.Value = gets(Request.QueryString["activetab"]);
                }

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
                    lstFilterProcedures.SelectedValue = gets(Request.QueryString["ProTypeID"]);
                    FillMedals();

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
                ViewState["ProceduresitemID"] = "0";

                if (Request.QueryString["id"] != null)
                {
                    ViewState["itemID"] = Request.QueryString["id"].ToString();

                }
                else if (Request.Form["id"] != null)
                {
                    ViewState["itemID"] = Request.Form["id"].ToString();
                }

                if (ViewState["itemID"].ToString() != "0")
                {
                    tblshow.Visible = false;
                    tblSearch.Visible = false;
                    // ViewState["itemID"] = Request.QueryString["id"].ToString();
                    FillMedalMasterInformation();
                    //Fill Agreemnt Details
                    FillMedalProcedure(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                    FillMedalPersons(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                }
                else
                {
                    lnkAddRelatedDoc.Visible = false;
                    lnkViewRelatedDoc.Visible = false;
                }

                SetPageTitle();

                ViewState["OutboundItemID"] = "0";

                // FillInboundItems();


            }

        }

        protected void grdInboundItems_ItemDataBound(object sender, DataGridItemEventArgs e)
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
                string code = e.Item.Cells[3].Text;
                string ItemType = e.Item.Cells[4].Text;
                //SqlDataReader dr = SellMaster.ins.getInvoiceItemsReader(code);

                var objpersonsList = objRepository.FillPersons(ZeroIntergerIFNull(code));

                if (objpersonsList != null)
                {
                    DataGrid grd = ((DataGrid)(e.Item.Cells[1].FindControl("grdPersons")));
                    grd.DataSource = objpersonsList;

                    if (e.Item.Cells[6].Text == "1")
                    {
                        grd.Columns[3].Visible = true;
                        grd.Columns[4].Visible = true;
                        grd.Columns[5].Visible = true;

                        grd.Columns[6].Visible = false;
                        grd.Columns[7].Visible = false;

                    }
                    else
                    {


                        grd.Columns[3].Visible = false;
                        grd.Columns[4].Visible = false;
                        grd.Columns[5].Visible = false;

                        grd.Columns[6].Visible = true;
                        grd.Columns[7].Visible = true;
                    }

                    grd.DataBind();

                }


                var objUnitList = objRepository.FillMedalProcedures(ZeroIntergerIFNull(code));
                if (objUnitList != null)
                {
                    DataGrid grd = ((DataGrid)(e.Item.Cells[1].FindControl("grdProcedures")));
                    grd.DataSource = objUnitList;





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
        }

        protected void grdPersons2_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (Session["fileName"] != null)
                {
                    if (e.Item.Cells[2].Text.Contains(gets(Session["fileName"])))
                    {
                        e.Item.BackColor = System.Drawing.Color.Navy;
                    }
                }

            }
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
            Session["PersonsList"] = null;
            FillMedalPersons(0);
            tblAdd.Visible = true;
            tblshow.Visible = false;
            tblSearch.Visible = false;
            PersonsAll.Visible = false;
            personDiv.Visible = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void lnkAddNewProcedurew_Click(object sender, EventArgs e)
        {
            divAddProcedure.Visible = true;
            divshowProcesure.Visible = false;
            ViewState["ProceduresitemID"] = "0";
        }

        protected void lnkDeleteProcedure_Click(object sender, EventArgs e)
        {


            try
            {

                Medalmain_Attachments obj = new Medalmain_Attachments();
                for (int i = 0; i <= grdAttachemnt.Items.Count - 1; i++)
                {

                    if ((grdAttachemnt.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdAttachemnt.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteAttacjmentmain((Medalmain_Attachments)objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(grdAttachemnt.Items[i].Cells[0].Text)));
                        }
                    }
                }


                FillMedalProcedure(ZeroIntergerIFNull(hdnMasterID.Value));

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
            divProAttache.Visible = false;
            FillProcedureFrom();

        }

        protected void grdData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }

        protected void lnkSaveProcesure_Click(object sender, EventArgs e)
        {

            SaveAttachmentInformation();


        }

        protected void lnkCancelProcedure_Click(object sender, EventArgs e)
        {
            divAddProcedure.Visible = false;
            divshowProcesure.Visible = true;
            ViewState["ProceduresitemID"] = "0";
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            // Clear previous search results for detail view
            ViewState["itemID"] = "0";
            ViewState["SpEdit"] = "1";
            Session["PersonsList"] = null;

            FillMedals();
        }

        protected void lnkSearchback_Click(object sender, EventArgs e)
        {
            tblSearch.Visible = true;
            tblshow.Visible = false;
        }

        protected void grdPersons_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {

                DropDownList lstMedalType = (DropDownList)e.Item.FindControl("lstMedalType");
                DropDownList lstJobGrade = (DropDownList)e.Item.FindControl("lstJobGrade");
                DropDownList lstGrantreasons = (DropDownList)e.Item.FindControl("lstGrantreasons");
                TextBox txtname = (TextBox)e.Item.FindControl("txtname");
                TextBox txtCivilID = (TextBox)e.Item.FindControl("txtCivilID");
                TextBox txtGrantreasons = (TextBox)e.Item.FindControl("txtGrantreasons");


                DropDownList lstCountryId = (DropDownList)e.Item.FindControl("lstCountryId");
                TextBox txtMilitaryNum = (TextBox)e.Item.FindControl("txtMilitaryNum");
                TextBox txtPersonTitle = (TextBox)e.Item.FindControl("txtPersonTitle");

                HtmlInputHidden hdnMilitaryNum = (HtmlInputHidden)e.Item.FindControl("hdnMilitaryNum");
                HtmlInputHidden hdnCountryId = (HtmlInputHidden)e.Item.FindControl("hdnCountryId");
                HtmlInputHidden hdnPersonTitle = (HtmlInputHidden)e.Item.FindControl("hdnPersonTitle");
                HtmlInputHidden hdnAdditionalData = (HtmlInputHidden)e.Item.FindControl("hdnAdditionalData");
                TextBox txtAdditionalData = (TextBox)e.Item.FindControl("txtAdditionalData");




                HtmlInputHidden hdnMedalType = (HtmlInputHidden)e.Item.FindControl("hdnMedalType");
                HtmlInputHidden hdnPerson_NameAr = (HtmlInputHidden)e.Item.FindControl("hdnPerson_NameAr");
                HtmlInputHidden hdnGradeID = (HtmlInputHidden)e.Item.FindControl("hdnGradeID");
                HtmlInputHidden hdnCivilID = (HtmlInputHidden)e.Item.FindControl("hdnCivilID");
                HtmlInputHidden hdnGrantReasonText = (HtmlInputHidden)e.Item.FindControl("hdnGrantReasonText");



                lstMedalType.Items.Clear();
                lstJobGrade.Items.Clear();
                lstGrantreasons.Items.Clear();
                lstCountryId.Items.Clear();

                FillDllwithoptional_ALL(Session["MedalList"], ref lstMedalType, "NameAr", "Code", "");
                FillDllwithoptional_ALL(Session["MedalGradeList"], ref lstJobGrade, "NameAr", "Code", "");
                FillDllwithoptional_ALL(Session["Grantreasons"], ref lstGrantreasons, "NameAr", "Code", "");
                FillDllwithoptional_ALL(Session["Countries"], ref lstCountryId, "NameAr", "Code", "");


                if (hdnMedalType.Value != "" && hdnMedalType.Value != "0")
                {
                    lstMedalType.SelectedValue = hdnMedalType.Value;
                }


                if (hdnPerson_NameAr.Value != "")
                {
                    txtname.Text = hdnPerson_NameAr.Value;
                }

                if (hdnGradeID.Value != "" && hdnGradeID.Value != "0")
                {
                    lstJobGrade.SelectedValue = hdnGradeID.Value;
                }

                if (hdnCivilID.Value != "" && hdnCivilID.Value != "0")
                {
                    txtCivilID.Text = hdnCivilID.Value;
                }


                if (hdnGrantReasonText.Value != "" && hdnGrantReasonText.Value != "0")
                {
                    txtGrantreasons.Text = hdnGrantReasonText.Value;
                }


                if (hdnCountryId.Value != "" && hdnCountryId.Value != "0")
                {
                    lstCountryId.SelectedValue = hdnCountryId.Value;
                }

                if (hdnPersonTitle.Value != "" && hdnPersonTitle.Value != "0")
                {
                    txtPersonTitle.Text = hdnPersonTitle.Value;
                }

                if (hdnAdditionalData.Value != "" && hdnAdditionalData.Value != "0")
                {
                    txtAdditionalData.Text = hdnAdditionalData.Value;
                }

                if (hdnMilitaryNum.Value != "" && hdnMilitaryNum.Value != "0")
                {
                    txtMilitaryNum.Text = hdnMilitaryNum.Value;
                }



                e.Item.Cells[1].Visible = false;
                e.Item.Cells[0].Attributes.Add("colspan", "2");



                LinkButton lnkUpdate = (LinkButton)(e.Item.Cells[0].FindControl("lnkUpdate"));

                LinkButton lnkAdd = (LinkButton)(e.Item.Cells[0].FindControl("lnkAdd"));
                LinkButton lnkCancel = (LinkButton)(e.Item.Cells[0].FindControl("lnkCancel"));
                //LinkButton lnkUpdate = (LinkButton)(e.Item.Cells[0].FindControl("lnkUpdate"));
                //LinkButton lnkUpdate = (LinkButton)(e.Item.Cells[0].FindControl("lnkUpdate"));

                //    Dim lnkAdd As LinkButton = CType(e.Item.Cells(0).FindControl("lnkAdd"), LinkButton)
                //Dim lnkCancel As LinkButton = CType(e.Item.Cells(0).FindControl("lnkCancel"), LinkButton)
                //Dim txtQuantity As TextBox = CType(e.Item.Cells(11).FindControl("txtQuantity"), TextBox)
                //Dim txtFooterQuantity As TextBox = CType(e.Item.Cells(11).FindControl("txtFooterQuantity"), TextBox)
                //Dim txtOld As HtmlInputHidden = CType(e.Item.Cells(11).FindControl("txtOld"), HtmlInputHidden)
                //Dim txtContent As HtmlInputHidden = CType(e.Item.Cells(11).FindControl("txtContent"), HtmlInputHidden)

                //Dim txtBalance As HtmlInputHidden = CType(e.Item.Cells(11).FindControl("txtBalance"), HtmlInputHidden)
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

                e.Item.BackColor = System.Drawing.Color.FromArgb(231,246,247);




            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string code = e.Item.Cells[2].Text.Replace("&nbsp;", " ").Trim();
                LinkButton lnkEdit = (LinkButton)(e.Item.Cells[0].FindControl("lnkEdit"));
                LinkButton lnkDelete = (LinkButton)(e.Item.Cells[0].FindControl("lnkDelete"));

                if (code.ToString().Equals("0"))
                {
                    //e.Item.Cells[0].Controls[0].Visible = false;
                    //e.Item.Cells[0].Controls[1].Visible = false;
                    //e.Item.Cells[1].Controls[0].Visible = false;
                    //e.Item.Cells[1].Controls[1].Visible = false;

                    //e.Item.Cells[0].Controls[0].Visible = true;
                    //e.Item.Cells[0].Controls[1].Visible = true;
                    //e.Item.Cells[1].Controls[0].Visible = true;
                    //e.Item.Cells[1].Controls[1].Visible = true;

                    lnkEdit.Visible = false;
                    lnkDelete.Visible = false;
                }
                else
                {
                    lnkEdit.Visible = true;
                    lnkDelete.Visible = true;



                }


                //LinkButton lnkUpdate = (LinkButton)(e.Item.Cells[0].FindControl("lnkUpdate"));

                //LinkButton lnkAdd = (LinkButton)(e.Item.Cells[0].FindControl("lnkAdd"));
                //LinkButton lnkCancel = (LinkButton)(e.Item.Cells[0].FindControl("lnkCancel"));

                //if (ViewState["SpEdit"].ToString() == "0")//' Inseting New Row Mode
                //{
                //    lnkUpdate.Visible = false;
                //    lnkCancel.Visible = false;
                //    lnkAdd.Visible = true;
                //    lnkAdd.Attributes.Add("onclick", "return LinkAddClick();");
                //}


            }

        }

        protected void grdPersons_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName.Equals("AddNew"))
            {
                ViewState["SpEdit"] = "0";

                List<Medal_Persons> PersonsList = new List<Medal_Persons>();
                if (Session["PersonsList"] != null)
                {
                    PersonsList = (List<Medal_Persons>)Session["PersonsList"];
                }

                // Get the controls from edit row
                TextBox txtname = ((TextBox)e.Item.FindControl("txtname"));
                TextBox txtMilitaryNum = ((TextBox)e.Item.FindControl("txtMilitaryNum"));
                DropDownList lstMedalType = ((DropDownList)e.Item.FindControl("lstMedalType"));
                DropDownList lstJobGrade = ((DropDownList)e.Item.FindControl("lstJobGrade"));
                DropDownList lstGrantreasons = ((DropDownList)e.Item.FindControl("lstGrantreasons"));
                DropDownList lstCountryId = ((DropDownList)e.Item.FindControl("lstCountryId"));

                // Validation: Check if required fields are not empty
                if (string.IsNullOrWhiteSpace(txtname.Text))
                {
                    string errorScript = FormatpopupErrorMSG("فضلاً، يجب إدخال الاسم", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", errorScript, true);
                    return;
                }

                if (ZeroIntergerIFNull(lstMedalType.SelectedValue) == 0)
                {
                    string errorScript = FormatpopupErrorMSG("فضلاً، يجب اختيار نوع الوسام", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", errorScript, true);
                    return;
                }

                if (ZeroIntergerIFNull(lstJobGrade.SelectedValue) == 0)
                {
                    string errorScript = FormatpopupErrorMSG("فضلاً، يجب اختيار الرتبة", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", errorScript, true);
                    return;
                }

                // Military Number is optional - removed validation

                Medal_Persons _personobj = new Medal_Persons();

                Medal_M_Types _Medal_M_Types = new Medal_M_Types();
                Medal_M_jobGrade _Medal_M_jobGrade = new Medal_M_jobGrade();
                medal_M_Grantreasons _medal_M_Grantreasons = new medal_M_Grantreasons();
                Agreement_Orgs _Countries = new Agreement_Orgs();


                _Medal_M_Types.Code = ZeroIntergerIFNull(lstMedalType.SelectedValue);
                _Medal_M_Types.NameEn = gets(lstMedalType.SelectedItem.Text);
                _Medal_M_Types.NameAr = gets(lstMedalType.SelectedItem.Text);



                _Medal_M_jobGrade.Code = ZeroIntergerIFNull(lstJobGrade.SelectedValue);
                _Medal_M_jobGrade.NameEn = gets(lstJobGrade.SelectedItem.Text);
                _Medal_M_jobGrade.NameAr = gets(lstJobGrade.SelectedItem.Text);


                _Countries.Code = ZeroIntergerIFNull(lstCountryId.SelectedValue);
                _Countries.NameEn = gets(lstCountryId.SelectedItem.Text);
                _Countries.NameAr = gets(lstCountryId.SelectedItem.Text);




                //_medal_M_Grantreasons.Code = ZeroIntergerIFNull(lstGrantreasons.SelectedValue);
                //_medal_M_Grantreasons.NameEn = gets(lstGrantreasons.SelectedItem.Text);
                //_medal_M_Grantreasons.NameAr = gets(lstGrantreasons.SelectedItem.Text);




                _personobj.Code = -1;
                _personobj.MedalType = ZeroIntergerIFNull(lstMedalType.SelectedValue);
                _personobj.GradeID = ZeroIntergerIFNull(lstJobGrade.SelectedValue);
                _personobj.GrantReason = ZeroIntergerIFNull(lstGrantreasons.SelectedValue);
                _personobj.CountryId = ZeroIntergerIFNull(lstCountryId.SelectedValue);

                _personobj.Person_NameEn = gets(((TextBox)e.Item.FindControl("txtname")).Text);
                _personobj.Person_NameAr = gets(((TextBox)e.Item.FindControl("txtname")).Text);
                _personobj.CivilID = gets(((TextBox)e.Item.FindControl("txtCivilID")).Text);
                _personobj.GrantReasonText = gets(((TextBox)e.Item.FindControl("txtGrantreasons")).Text);


                _personobj.PersonTitle = gets(((TextBox)e.Item.FindControl("txtPersonTitle")).Text);
                _personobj.AdditionalData = gets(((TextBox)e.Item.FindControl("txtAdditionalData")).Text);
                _personobj.MilitaryNum = gets(((TextBox)e.Item.FindControl("txtMilitaryNum")).Text);
                _personobj.CountryName = gets(lstCountryId.SelectedItem.Text);



                _personobj.Medal_M_Types = _Medal_M_Types;
                _personobj.Medal_M_jobGrade = _Medal_M_jobGrade;
                //  _personobj.medal_M_Grantreasons = _medal_M_Grantreasons;



                PersonsList.Add(_personobj);
                Session["PersonsList"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;

                // grdPersons.EditItemIndex = PersonsList.Count;
                grdPersons.EditItemIndex = 0;

                grdPersons.DataSource = AddDefaultItems(PersonsList);
                grdPersons.DataBind();

                // Update pager
                if (pagerPersons != null)
                {
                    pagerPersons.ItemCount = PersonsList.Count;
                    pagerPersons.CurrentIndex = 1;
                }
                grdPersons.CurrentPageIndex = 0;

                string script = FormatpopupErrorMSG("Person Added Successfully ", "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            else if (e.CommandName.Equals("Edit"))
            {
                ViewState["SpEdit"] = 1;
                grdPersons.EditItemIndex = e.Item.ItemIndex;
                // grdPersons.DataBind();


                List<Medal_Persons> PersonsList = new List<Medal_Persons>();
                if (Session["PersonsList"] != null)
                {
                    PersonsList = (List<Medal_Persons>)Session["PersonsList"];
                }

                grdPersons.DataSource = AddDefaultItems(PersonsList);
                grdPersons.DataBind();

            }
            else if (e.CommandName.Equals("Update"))
            {

                int selectedIndex = grdPersons.Items.Count > 10 ? e.Item.ItemIndex - 1 : (grdPersons.Items.Count - e.Item.ItemIndex) - 1;
                List<Medal_Persons> PersonsList = new List<Medal_Persons>();
                if (Session["PersonsList"] != null)
                {
                    PersonsList = (List<Medal_Persons>)Session["PersonsList"];
                }

                DropDownList lstMedalType = ((DropDownList)e.Item.FindControl("lstMedalType"));
                DropDownList lstJobGrade = ((DropDownList)e.Item.FindControl("lstJobGrade"));
                DropDownList lstGrantreasons = ((DropDownList)e.Item.FindControl("lstGrantreasons"));
                DropDownList lstCountryId = ((DropDownList)e.Item.FindControl("lstCountryId"));


                Medal_M_Types _Medal_M_Types = new Medal_M_Types();
                Medal_M_jobGrade _Medal_M_jobGrade = new Medal_M_jobGrade();



                Medal_Persons _personobj = new Medal_Persons();
                _personobj = PersonsList[selectedIndex]; // Get Item Index From End Of List

                //_personobj.Medal_M_Types.Code = ZeroIntergerIFNull(lstMedalType.SelectedValue);
                //_personobj.Medal_M_Types.NameEn = gets(lstMedalType.SelectedItem.Text);
                //_personobj.Medal_M_Types.NameAr = gets(lstMedalType.SelectedItem.Text);

                //_personobj.Medal_M_jobGrade.Code = ZeroIntergerIFNull(lstJobGrade.SelectedValue);
                //_personobj.Medal_M_jobGrade.NameEn = gets(lstJobGrade.SelectedItem.Text);
                //_personobj.Medal_M_jobGrade.NameAr = gets(lstJobGrade.SelectedItem.Text);


                _Medal_M_Types.Code = ZeroIntergerIFNull(lstMedalType.SelectedValue);
                _Medal_M_Types.NameEn = gets(lstMedalType.SelectedItem.Text);
                _Medal_M_Types.NameAr = gets(lstMedalType.SelectedItem.Text);



                _Medal_M_jobGrade.Code = ZeroIntergerIFNull(lstJobGrade.SelectedValue);
                _Medal_M_jobGrade.NameEn = gets(lstJobGrade.SelectedItem.Text);
                _Medal_M_jobGrade.NameAr = gets(lstJobGrade.SelectedItem.Text);




                _personobj.Medal_M_Types = _Medal_M_Types;
                _personobj.Medal_M_jobGrade = _Medal_M_jobGrade;



                //_personobj.Medal_M_Types = null;
                //_personobj.Medal_M_jobGrade = null;

                _personobj.MedalType = ZeroIntergerIFNull(lstMedalType.SelectedValue);


                _personobj.GradeID = ZeroIntergerIFNull(lstJobGrade.SelectedValue);
                _personobj.GrantReason = ZeroIntergerIFNull(lstGrantreasons.SelectedValue);

                _personobj.Person_NameEn = gets(((TextBox)e.Item.FindControl("txtname")).Text);
                _personobj.Person_NameAr = gets(((TextBox)e.Item.FindControl("txtname")).Text);
                _personobj.CivilID = gets(((TextBox)e.Item.FindControl("txtCivilID")).Text);
                _personobj.GrantReasonText = gets(((TextBox)e.Item.FindControl("txtGrantreasons")).Text);


                _personobj.MilitaryNum = gets(((TextBox)e.Item.FindControl("txtMilitaryNum")).Text);
                _personobj.PersonTitle = gets(((TextBox)e.Item.FindControl("txtPersonTitle")).Text);
                _personobj.CountryName = gets(lstCountryId.SelectedItem.Text);



                PersonsList[selectedIndex] = _personobj;
                Session["PersonsList"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;
                ViewState["SpEdit"] = 0;

                // grdPersons.EditItemIndex = PersonsList.Count;
                grdPersons.EditItemIndex = 0;

                grdPersons.DataSource = AddDefaultItems(PersonsList);
                grdPersons.DataBind();

                // Update pager
                if (pagerPersons != null)
                {
                    pagerPersons.ItemCount = PersonsList.Count;
                    pagerPersons.CurrentIndex = 1;
                }
                grdPersons.CurrentPageIndex = 0;

                string script = FormatpopupErrorMSG("Person Updated Successfully ", "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            else if (e.CommandName.Equals("Delete"))
            {
                string code = e.Item.Cells[2].Text.Replace("&nbsp;", " ").Trim();

                List<Medal_Persons> PersonsList = new List<Medal_Persons>();
                if (Session["PersonsList"] != null)
                {
                    PersonsList = (List<Medal_Persons>)Session["PersonsList"];
                }
                if (code.ToString() != "0" && code.ToString() != "-1")
                {
                    //Get Person Delatils
                    var objforDelete = objRepository.getpersonDetails(ZeroIntergerIFNull(code));
                    objRepository.DeletePersons(objforDelete);

                }

                PersonsList.RemoveAt(e.Item.ItemIndex);




                Session["PersonsList"] = PersonsList;
                //  grdPersons.EditItemIndex = -1;
                ViewState["SpEdit"] = 0;

                // grdPersons.EditItemIndex = PersonsList.Count;
                grdPersons.EditItemIndex = 0;

                grdPersons.DataSource = AddDefaultItems(PersonsList);
                grdPersons.DataBind();

                // Update pager after delete
                if (pagerPersons != null)
                {
                    pagerPersons.ItemCount = PersonsList.Count;

                    // Validate current page index doesn't exceed page count
                    decimal pageCount = Math.Ceiling((decimal)PersonsList.Count / grdPersons.PageSize);
                    if (grdPersons.CurrentPageIndex >= pageCount)
                    {
                        grdPersons.CurrentPageIndex = (int)pageCount - 1;
                    }

                    pagerPersons.CurrentIndex = grdPersons.CurrentPageIndex + 1;
                }
            }
            else if (e.CommandName.Equals("Cancel"))
            {
                ViewState["SpEdit"] = 0;

                List<Medal_Persons> PersonsList = new List<Medal_Persons>();
                if (Session["PersonsList"] != null)
                {
                    PersonsList = (List<Medal_Persons>)Session["PersonsList"];
                }
                //  grdPersons.EditItemIndex = PersonsList.Count;
                grdPersons.EditItemIndex = 0;

                grdPersons.DataSource = AddDefaultItems(PersonsList);
                grdPersons.DataBind();

                // Update pager
                if (pagerPersons != null)
                {
                    pagerPersons.ItemCount = PersonsList.Count;
                    pagerPersons.CurrentIndex = grdPersons.CurrentPageIndex + 1;
                }
            }
        }

        protected void btnAddNewItem_Click(object sender, EventArgs e)
        {

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
            ViewState["ProceduresitemID"] = "0";
            Session["PersonsList"] = null;

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillMedals();
        }
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            SaveMedalMaster();
        }


        protected void lstMedalCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            AduptPersonGrd();



        }

        protected void lstorg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstorg.SelectedValue == "6") //الديوان الاميري
            {
                divMedalCategory.Visible = false;
                lstMedalCat.SelectedValue = "2";
                UploadPersonsList.Visible = false;

            }
            else
            {
                divMedalCategory.Visible = false;
                lstMedalCat.SelectedValue = "1";
                UploadPersonsList.Visible = true;
            }

        }

        protected void grdAttachemnt_EditCommand(object source, DataGridCommandEventArgs e)
        {
            divshowProcesure.Visible = false;

            string id = e.Item.Cells[0].Text;
            this.CleaProcesure();
            ViewState["ProceduresitemID"] = id;
            FillProcedureFrom();
            divshowProcesure.Visible = false;
            divAddProcedure.Visible = true;

        }

        protected void grdAttachemnt_ItemDataBound(object sender, DataGridItemEventArgs e)
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

        protected void lnkUploadPersons_Click(object sender, EventArgs e)
        {
            //Validate File Extention and Format
            string _uploadeFileName = UploadFileoServer(txtUploadPersons, "/Layout/uploads/MedalFiles");
            if (_uploadeFileName != null && _uploadeFileName != "")
            {
                try
                {


                    // Read and Parse File 
                    DataTable PersonList = ReadExcelFile("/Layout/uploads/MedalFiles" + _uploadeFileName);

                    if (PersonList != null && PersonList.Rows.Count > 0)
                    {
                        if (validateTemplate(PersonList))
                        {
                            for (int i = 1; i < PersonList.Rows.Count; i++)
                            {
                                if (gets(PersonList.Rows[i][0]) != "" && gets(PersonList.Rows[i][1]) != "" && gets(PersonList.Rows[i][2]) != "" && gets(PersonList.Rows[i][3]) != "")
                                {
                                    string medalType = gets(PersonList.Rows[i][0]);
                                    string personName = gets(PersonList.Rows[i][1]);
                                    string grade = gets(PersonList.Rows[i][2]);
                                    string militaryNum = gets(PersonList.Rows[i][3]);
                                    string additionalData = PersonList.Columns.Count > 4 ? gets(PersonList.Rows[i][4]) : "";

                                    AppendPersonFromExcel(personName, medalType, grade, militaryNum, additionalData);
                                }
                                else
                                {
                                    string script = FormatpopupErrorMSG("Row Indx:" + i.ToString() + " Data Missed ", "1");
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                                }

                            }


                            File.Delete("/Layout/uploads/MedalFiles" + _uploadeFileName);

                            // Show success message - Select2 will be initialized automatically
                            string successScript = FormatpopupErrorMSG("تم إضافة الأشخاص بنجاح", "3");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", successScript, true);
                        }
                        else
                        {
                            string script = FormatpopupErrorMSG("فضلا تأكد من خانات الملف المرفوع", "1");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                            // Delete Excel File
                            File.Delete("/Layout/uploads/MedalFiles" + _uploadeFileName);
                        }
                    }
                }
                catch (Exception ex)
                {

                    string script = FormatpopupErrorMSG("عفوا ، خطأ اثناء قراءة الاسماء ، فضلا حاول مرة اخرى  ", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                }

            }
        }
        private bool validateTemplate(DataTable dt)
        {
            if (gets(dt.Rows[0][0]) != "النوط")
            {
                return false;

            }
            if (gets(dt.Rows[0][1]) != "الاســـــــــــم")
            {
                return false;

            }
            if (gets(dt.Rows[0][2]) != "الرتـبة")
            {
                return false;

            }
            if (gets(dt.Rows[0][3]) != "الرقم العســكري")
            {
                return false;

            }

            return true;
        }

        private DataTable ReadExcelFile(string storePath)
        {



            FileStream stream = File.Open(storePath, FileMode.Open, FileAccess.Read);

            string fileExtension = Path.GetExtension(storePath);
            IExcelDataReader excelReader = null;
            if (fileExtension == ".xls")
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (fileExtension == ".xlsx")
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            DataSet result = excelReader.AsDataSet();
            var test = result.Tables[0];
            return result.Tables[0];

        }

        private void AppendPersonFromExcel(string Person_NameEn, string medalType, string grade, string MilitaryNum, string AdditionalData = "")
        {

            ViewState["SpEdit"] = "0";

            List<Medal_Persons> PersonsList = new List<Medal_Persons>();
            if (Session["PersonsList"] != null)
            {
                PersonsList = (List<Medal_Persons>)Session["PersonsList"];
            }

            Medal_Persons _personobj = new Medal_Persons();

            Medal_M_Types _Medal_M_Types = new Medal_M_Types();
            Medal_M_jobGrade _Medal_M_jobGrade = new Medal_M_jobGrade();
            medal_M_Grantreasons _medal_M_Grantreasons = new medal_M_Grantreasons();
            Agreement_Orgs _Countries = new Agreement_Orgs();

            _Medal_M_Types = getMeadalTypeByCode(medalType);
            _Medal_M_jobGrade = getGrdeByCode(grade);

            _personobj.Code = -1;
            _personobj.MedalType = _Medal_M_Types.Code;
            _personobj.GradeID = _Medal_M_jobGrade.Code;

            _personobj.Person_NameEn = Person_NameEn;
            _personobj.Person_NameAr = Person_NameEn;
            _personobj.MilitaryNum = MilitaryNum;

            if (!string.IsNullOrEmpty(AdditionalData))
            {
                _personobj.AdditionalData = AdditionalData;
            }


            _personobj.Medal_M_Types = _Medal_M_Types;
            _personobj.Medal_M_jobGrade = _Medal_M_jobGrade;


            PersonsList.Add(_personobj);
            Session["PersonsList"] = PersonsList;

            grdPersons.EditItemIndex = 0;

            grdPersons.DataSource = AddDefaultItems(PersonsList);
            grdPersons.DataBind();
            lblPersonCount.Text = "<span style='color:red'>(" + PersonsList.Count.ToString() + ")</span>";

            // Set pager item count for Excel upload
            if (pagerPersons != null)
            {
                pagerPersons.ItemCount = PersonsList.Count;
                pagerPersons.CurrentIndex = 1;
            }

            // Reset to first page
            grdPersons.CurrentPageIndex = 0;
        }
        private Medal_M_Types getMeadalTypeByCode(string medalTypeCode)
        {
            if (!string.IsNullOrEmpty(medalTypeCode))
            {
                var MedalTypeList = objLookup.FillMedalsTypeByCode(medalTypeCode);
                if (MedalTypeList != null)
                {
                    return MedalTypeList;
                }
                else
                {
                    //Add New Recode and 
                    Medal_M_Types objnew = new Medal_M_Types();
                    objnew.NameEn = medalTypeCode;
                    objnew.NameAr = medalTypeCode;
                    objLookup.AddMedal_M_Types(objnew);
                    return objnew;

                }

            }
            return null;
        }
        private Medal_M_jobGrade getGrdeByCode(string GradeCode)
        {
            if (!string.IsNullOrEmpty(GradeCode))
            {
                var medalGrade = objLookup.getGradeByCode(GradeCode);
                if (medalGrade != null)
                {
                    return medalGrade;
                }
                else
                {
                    //Add New Recode and 
                    Medal_M_jobGrade objnew = new Medal_M_jobGrade();
                    objnew.NameEn = GradeCode;
                    objnew.NameAr = GradeCode;
                    objLookup.AddMedalGrade(objnew);
                    return objnew;

                }

            }
            return null;
        }


        //}
        #endregion

        #region "Fill Information"

        private void FillMedalPersons(int MedalID)
        {
            var objpersonsList = objRepository.FillPersons(MedalID);
            if (Session["PersonsList"] != null && ViewState["SpEdit"].ToString() == "1")
            {
                objpersonsList = (List<Medal_Persons>)Session["PersonsList"];
            }
            else if (objpersonsList.Count > 0)
            {
                Session["PersonsList"] = objpersonsList;
            }

            if (objpersonsList != null && objpersonsList.Count > 0)
            {
                var dataSource = AddDefaultItems(objpersonsList);

                // Set CurrentPageIndex FIRST before setting DataSource
                // This ensures the page index is set before DataBind processes it
                int currentPageIndex = grdPersons.CurrentPageIndex;
                if (currentPageIndex >= dataSource.Count / grdPersons.PageSize)
                {
                    currentPageIndex = Math.Max(0, (dataSource.Count / grdPersons.PageSize) - 1);
                }

                grdPersons.CurrentPageIndex = currentPageIndex;
                grdPersons.DataSource = dataSource;

                // Always set edit index to 0 (for the blank row) in add mode (SpEdit = "0")
                if (ViewState["SpEdit"].Equals("0"))
                {
                    grdPersons.EditItemIndex = 0;
                    // Show upload section in add mode
                    UploadPersonsList.Visible = true;
                }
                else
                {
                    // In details view mode (SpEdit = "1"), don't set edit index
                    // so rows show as read-only
                    grdPersons.EditItemIndex = -1;
                    // Hide upload section in details view
                    UploadPersonsList.Visible = false;
                }

                // DataBind should preserve the CurrentPageIndex that was just set
                grdPersons.DataBind();
            }
            else
            {
                grdPersons.EditItemIndex = 0;
                grdPersons.DataSource = AddDefaultItems(objpersonsList);
                grdPersons.DataBind();

                // Show upload section when adding new
                UploadPersonsList.Visible = (ViewState["SpEdit"].ToString() == "0");
            }

            lblPersonCount.Text = "(" + objpersonsList.Count().ToString() + ")";

            // Set pager item count and current index
            // Use the actual dataSource count for paging calculations (which includes placeholder rows)
            if (pagerPersons != null)
            {
                // Only count non-placeholder rows for the pager ItemCount
                // The pager should show the number of actual data rows, not including blanks
                pagerPersons.ItemCount = objpersonsList.Count;

                // Set pager current index based on grid's current page
                pagerPersons.CurrentIndex = grdPersons.CurrentPageIndex + 1;
            }
        }

        /// <summary>
        /// Determines if a row is a placeholder (empty) row.
        /// </summary>
        public bool IsPlaceholderRow(object dataItem)
        {
            if (dataItem == null)
                return true;

            var row = dataItem as Medal_Persons;
            if (row == null)
                return true;

            // A row is considered a placeholder if it has no name and no medal type
            return string.IsNullOrWhiteSpace(row.Person_NameAr) && row.MedalType == 0;
        }

        /// <summary>
        /// Counts placeholder rows before the given index to adjust serial numbering.
        /// </summary>
        public int GetPlaceholderRowsBefore(int itemIndex)
        {
            int count = 0;
            if (grdPersons.DataSource is List<Medal_Persons> dataSource)
            {
                for (int i = 0; i < itemIndex && i < dataSource.Count; i++)
                {
                    if (IsPlaceholderRow(dataSource[i]))
                        count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Gets the correct row number accounting for placeholder rows and pagination
        /// </summary>
        public int GetRowNumber(int itemIndex)
        {
            // Initialize serial number based on current page and items per page
            // Count non-placeholder items up to current position
            int serialNumber = 1;
            int itemsPerPage = grdPersons.PageSize;
            int currentPageStart = grdPersons.CurrentPageIndex * itemsPerPage;

            if (grdPersons.DataSource is List<Medal_Persons> dataSource)
            {
                // Count non-placeholder rows from the beginning to the start of current page
                for (int i = 0; i < currentPageStart && i < dataSource.Count; i++)
                {
                    if (!IsPlaceholderRow(dataSource[i]))
                    {
                        serialNumber++;
                    }
                }

                // Count non-placeholder rows from current page start to current item
                for (int i = currentPageStart; i < currentPageStart + itemIndex && i < dataSource.Count; i++)
                {
                    if (!IsPlaceholderRow(dataSource[i]))
                    {
                        serialNumber++;
                    }
                }
            }
            else
            {
                // Fallback to simple calculation if DataSource is not available
                serialNumber = (grdPersons.CurrentPageIndex * itemsPerPage) + itemIndex + 1;
            }

            return serialNumber;
        }

        private List<Medal_Persons> AddDefaultItems(List<Medal_Persons> _SourceList)
        {
            List<Medal_Persons> _OutList = new List<Medal_Persons>();

            int TargetCount = 10; // Minimum rows per page for empty grids

            // Only add blank rows if we have fewer items than TargetCount
            if (_SourceList.Count < TargetCount)
            {
                // Add a blank row at the beginning (for add mode or details view)
                _OutList.Add(new Medal_Persons());

                // Add actual data
                for (int i = 0; i < _SourceList.Count; i++)
                {
                    _OutList.Add(_SourceList[i]);
                }

                // Add empty rows for remaining count to reach TargetCount
                int _RoundCount = TargetCount - _SourceList.Count - 1; // -1 because we added one blank row
                if (_RoundCount > 0)
                {
                    for (int i = 0; i < _RoundCount; i++)
                    {
                        _OutList.Add(new Medal_Persons());
                    }
                }
            }
            else
            {
                // For large datasets, don't add placeholder rows
                // Just add the actual data
                for (int i = 0; i < _SourceList.Count; i++)
                {
                    _OutList.Add(_SourceList[i]);
                }
            }

            return _OutList;
        }
        private string MapSearchKeys()
        {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                _keyList.Add("رقم الوارد", txtFilterFileSerial.Text);
                _keyList.Add("السنة", txtFilterFileYear.Text);
                _keyList.Add("رقم الملف", txtFilterFileNUm.Text);
                _keyList.Add("جزء من الاسم", txtFilterName.Text);
                _keyList.Add("تاريخ المنح من", txtFilterDatefrom.Text);
                _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                _keyList.Add("الرتبة  ", lstFilterJobGrade.SelectedItem.Text);
                _keyList.Add("التصنيف  ", lstMedalCat.SelectedItem.Text);
                _keyList.Add(" نوع الوسام", lstFilterType.SelectedItem.Text);
                _keyList.Add("الاجراء ", lstFilterProcedures.SelectedItem.Text);
                _keyList.Add(" الجهة ", lstFilterOrg.SelectedItem.Text);
            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }
        private void FillMedals()
        {
            if (txtFilterName.Text == "")
            {
                Session["fileName"] = null;
                var objList = objRepository.GetList(NullDateifEmpty(txtFilterDatefrom.Text),
                    NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterType.SelectedValue),
                    0, ZeroIntergerIFNull(lstFilterOrg.SelectedValue),
                    ZeroIntergerIFNull(lstFilterProcedures.SelectedValue), txtFilterName.Text,
                    ZeroIntergerIFNull(lstFilterJobGrade.SelectedValue), getBool(ReadSession("ViewPrivate")), MapSearchKeys(), ZeroIntergerIFNull(lstFilterMedalCat.SelectedValue),
                    ZeroIntergerIFNull(txtFilterFileSerial.Text), ZeroIntergerIFNull(txtFilterFileYear.Text));
                lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
                lblcount2.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));

                if (!txtFilterName.Text.Equals(""))
                {
                    Session["fileName"] = txtFilterName.Text;
                }

                var duplicatedList = objList.SelectMany(t =>
               Enumerable.Repeat(t, 2)).ToList();

                //decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdInboundItems.PageSize));
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
                personDiv.Visible = false;
                PersonsAll.Visible = false;
                grdInboundItems.Visible = true;

                grdInboundItems.DataSource = duplicatedList;
                grdInboundItems.DataBind();
                pager1.ItemCount = duplicatedList.Count;
            }
            else if (txtFilterName.Text != "")
            {
                var objpersonsList = objRepository.GetPersonList(NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterType.SelectedValue),
                0, ZeroIntergerIFNull(lstFilterOrg.SelectedValue),
                ZeroIntergerIFNull(lstFilterProcedures.SelectedValue), txtFilterName.Text,
                ZeroIntergerIFNull(lstFilterJobGrade.SelectedValue), getBool(ReadSession("ViewPrivate")), MapSearchKeys(), ZeroIntergerIFNull(lstFilterMedalCat.SelectedValue),
                ZeroIntergerIFNull(txtFilterFileSerial.Text), ZeroIntergerIFNull(txtFilterFileYear.Text));
                PersonsAll.DataSource = objpersonsList;
                PersonsAll.DataBind();

                personDiv.Visible = true;
                PersonsAll.Visible = true;

                tblshow.Visible = false;
                pager1.Visible = false;
                grdInboundItems.Visible = false;
                
               
            }
           
        }


        private void FillMedalProcedure(int MedalMasterID)
        {

            var objList = objRepository.FillMedalMainAttachemnt(MedalMasterID);
            //  lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
            decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdAttachemnt.PageSize));

            lblProcedureCount.Text = objList.Count.ToString();
            if ((c <= grdAttachemnt.CurrentPageIndex))
            {
                grdAttachemnt.CurrentPageIndex = 0;
            }

            grdAttachemnt.DataSource = objList;
            grdAttachemnt.DataBind();
            int _totalCount = objList.Count;
            pager2.ItemCount = _totalCount;

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
                // divshowProcesure.Visible = false;
                //pager2.Visible = false;
                //string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

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
            if (Request.QueryString["d"] != null)
            {
                // lblSubTitle.Text = "Deposit Goods";

            }


        }

        private void ClearForm()
        {


            ViewState["itemID"] = "0";
            txtSerialNUm.Text = "";
            txtserialYear.Text = "";
            txtMedalNotes.Text = "";
            txtMedalDate.Text = "";


            //BlblSubTitle.Text = this.GetTitle(true);
        }
        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            lnkAddNewProcedurew.Visible = userAccess.Add;
            
            
            //btnDelete.Visible = userAccess.Delete;
            grdInboundItems.Columns[14].Visible = userAccess.Delete;
            lnkDeleteProcedure.Visible = userAccess.Delete;
            lnkSaveProcesure.Visible = userAccess.Edit ||  userAccess.Add;

            if (Request.QueryString["editflag"] != null)
            {
                string editflag = Request.QueryString["editflag"].ToString();

                btnSave.Visible = userAccess.Edit;

                lnkAddRelatedDoc.Visible = userAccess.Edit;
                
            }
            else
            {

                btnSave.Visible = userAccess.Edit || userAccess.Add;
                lnkAddRelatedDoc.Visible = userAccess.Edit || userAccess.Add;
            }



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
            FillMedals();
        }


        #endregion

        #region "Helper Methods"

        private void AduptPersonGrd()
        {
            if (lstMedalCat.SelectedValue == "1")
            {
                grdPersons.Columns[13].Visible = false;
                grdPersons.Columns[14].Visible = false;


                grdPersons.Columns[10].Visible = true;
                grdPersons.Columns[11].Visible = false;//person
                grdPersons.Columns[12].Visible = true;

            }
            else
            {

                grdPersons.Columns[10].Visible = false;
                grdPersons.Columns[11].Visible = false;
                grdPersons.Columns[12].Visible = false;


                grdPersons.Columns[13].Visible = true;
                grdPersons.Columns[14].Visible = true;
            }

        }

        private void FillMedalMasterInformation()
        {

            var objList = objRepository.FillDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {
                hdnMasterID.Value = gets(objList.Code);

                anchorAttachment.Visible = true;
                anchorAttachment.HRef = "MedalsMainAttachments.aspx?MedalMasterID=" + hdnMasterID.Value;


                txtSerialNUm.Text = gets(objList.FileSerial);
                txtserialYear.Text = gets(objList.FileYear);


                lstorg.SelectedValue = gets(objList.Medal_OrgID);
                lstMedalCat.SelectedValue = gets(objList.MedalCatId);

 


                if (gets(objList.Medal_OrgID) == "6") //الديوان الاميري
                {
                    divMedalCategory.Visible = false;
                    UploadPersonsList.Visible = false;

                }
                else
                {
                    divMedalCategory.Visible = false;
                    UploadPersonsList.Visible = true;
                }



                txtMedalDate.Text = NullDateifEmptyToText(objList.Medal_receivedDate).ToString();

                txtMedalNotes.Text = gets(objList.Remarks);

                chkIsPrivate.Checked = getBool(objList.isPrivate);

                if (objList.RelatedLawDocRefId != null && objList.RelatedLawDocRefId != 0)
                {
                    lnkAddRelatedDoc.Visible = false;
                    lnkViewRelatedDoc.Visible = true;
                    lnkViewRelatedDoc.HRef = "/modules/laws/Forms/LawDocData.aspx?LawDocID=" + gets(objList.RelatedLawDocRefId);


                }
                else
                {
                    lnkAddRelatedDoc.Visible = true;
                    lnkViewRelatedDoc.Visible = false;
                    lnkAddRelatedDoc.HRef = "/modules/laws/forms/LawDocDatalnk.aspx?MedalSourceId=" + gets(objList.Code) + "&lawDocRefId=0";


                }
            }

            tblAdd.Visible = true;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private void SaveMedalMaster()
        {

            string script = "";
            try
            {
                Medal_Data obj = new Medal_Data();
                if (ViewState["itemID"].Equals("0"))
                {//Save



                    if (objRepository.CheckExistance(ZeroIntergerIFNull(txtSerialNUm.Text), ZeroIntergerIFNull(txtserialYear.Text), 0))
                    {

                        script = FormatpopupErrorMSG("  رقم الملف مسجل من قبل [مسلسل / سنة]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return;
                    }


                    obj.TransDate = DateTime.Now;


                    obj.FileNum = txtSerialNUm.Text + "/" + txtserialYear.Text;

                    obj.FileSerial = ZeroIntergerIFNull(txtSerialNUm.Text);
                    obj.FileYear = ZeroIntergerIFNull(txtserialYear.Text);


                    //obj.FileNum = txtfilnum.Text;
                    //if (!txtfilnum.Text.Equals(""))
                    //{
                    //    if (txtfilnum.Text.IndexOf('/')==-1)
                    //    {

                    //        script = FormatpopupErrorMSG("فضلا ، ادخل  رقم الملف بشكل صحيح ", "3");
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    //        return;
                    //    }

                    //    string[] fn = txtfilnum.Text.Split('/');
                    //    if (fn.Length > 0)
                    //    {
                    //        obj.FileSerial = ZeroIntergerIFNull(fn[0]);
                    //        obj.FileYear = ZeroIntergerIFNull(fn[1]);
                    //    }

                    //}


                    obj.Medal_receivedDate = NullDateifEmpty(txtMedalDate.Text);
                    obj.Medal_OrgID = ZeroIntergerIFNull(lstorg.SelectedValue);
                    obj.Remarks = txtMedalNotes.Text;
                    obj.isPrivate = getBool(chkIsPrivate.Checked);
                    obj.MedalCatId = ZeroIntergerIFNull(lstMedalCat.SelectedValue);

                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());


                    objRepository.AddMedal(obj);
                    hdnMasterID.Value = gets(obj.Code);
                    ViewState["itemID"] = gets(obj.Code);

                    // Save Documenting Status

                    //Medal_ProcedureHistory objStatus = new Medal_ProcedureHistory();
                    //if (ViewState["StatusTrackingitemID"].Equals("0"))
                    //{//Save


                    //    objStatus.AgreementCode = obj.Code;
                    //    objStatus.ActionDate = DateTime.Now;
                    //    objStatus.Remarks = "New Request Documenting Status";
                    //    objStatus.DepositeStatusTypeCode = 1;


                    //    objRepository.AddProcedure(objStatus);
                    //}

                }
                else
                { //Update


                    if (objRepository.CheckExistance(ZeroIntergerIFNull(txtSerialNUm.Text), ZeroIntergerIFNull(txtserialYear.Text), ZeroIntergerIFNull(ViewState["itemID"].ToString())))
                    {
                        script = FormatpopupErrorMSG("رقم الملف  مسجل من قبل [مسلسل / سنة]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return;
                    }


                    hdnMasterID.Value = ViewState["itemID"].ToString();
                    obj = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    obj.LastUpdate = DateTime.Now;

                    //obj.FileNum = txtfilnum.Text;
                    //if (!txtfilnum.Text.Equals(""))
                    //{
                    //    string[] fn = txtfilnum.Text.Split('/');
                    //    if (fn.Length > 0)
                    //    {
                    //        obj.FileSerial = ZeroIntergerIFNull(fn[0]);
                    //        obj.FileYear = ZeroIntergerIFNull(fn[1]);
                    //    }
                    //}


                    obj.FileNum = txtSerialNUm.Text + "/" + txtserialYear.Text;

                    obj.FileSerial = ZeroIntergerIFNull(txtSerialNUm.Text);
                    obj.FileYear = ZeroIntergerIFNull(txtserialYear.Text);

                    obj.Medal_receivedDate = NullDateifEmpty(txtMedalDate.Text);
                    obj.Medal_OrgID = ZeroIntergerIFNull(lstorg.SelectedValue);
                    obj.Remarks = txtMedalNotes.Text;
                    obj.isPrivate = getBool(chkIsPrivate.Checked);
                    obj.MedalCatId = ZeroIntergerIFNull(lstMedalCat.SelectedValue);

                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    objRepository.UpdateMedal(obj);

                }

                ViewState["itemID"] = obj.Code;

                anchorAttachment.Visible = true;
                anchorAttachment.HRef = "MedalsMainAttachments.aspx?MedalMasterID=" + hdnMasterID.Value;

                //  ClearForm();
                //Save Person
                try
                {
                    SavePersons(obj.Code);
                    Session["PersonsList"] = null;


                    script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);


                    Response.Redirect("MedalsData.aspx?ss=1&id=" + gets(obj.Code));


                }
                catch (Exception)
                {

                    //throw;
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

            var MedalTypeList = objLookup.FillMedalsTypes();
            Session["MedalList"] = MedalTypeList;


            var MedalGradeList = objLookup.FillJobGrade();
            Session["MedalGradeList"] = MedalGradeList;


            var Grantreasons = objLookup.FillGrantReason();
            Session["Grantreasons"] = Grantreasons;

            var Countries = objLookup.FillOrganization();
            Session["Countries"] = Countries;


            FillDllwithoptional_ALL(objLookup.FillMedalsTypes(), ref lstFilterType, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillMedalOrgs(), ref lstFilterOrg, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillJobGrade(), ref lstFilterJobGrade, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillMedalProcedureType(), ref lstFilterProcedures, "NameAr", "Code", "الكل");

            //FillDllwithoptional_ALL(objLookup.FillAgreementTYpes(), lstTypeCode, "NameAr", "Code", "");
            //FillDllwithoptional_ALL(objLookup.FillAgreementCategories(), lstCats, "NameAr", "Code", "");
            FillDllwithoptional_ALL(objLookup.FillMedalOrgs(), ref lstorg, "NameAr", "Code", "");


            FillDllwithoptional_ALL(objLookup.FillMedalattachmentTyps(), ref lstAttachmentType, "NameAr", "Code", "");




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
        public string viewMedalCat(string catId)
        {
            if (catId == "1")
            {
                return "<span class='label label-sm label-success'>داخلي</span>";
            }
            else
            {
                return "<span class='label label-sm label-warning'>خارجي</span>";
            }

        }


        #endregion

        #region "Procedure Methods"

        private void CleaProcesure()
        {


            ViewState["ProceduresitemID"] = "0";
            txtCreationDate.Text = "";
            txtRef.Text = "";

            lstAttachmentType.SelectedValue = "0";
            txtRef.Text = "";

            //BlblSubTitle.Text = this.GetTitle(true);
        }

        private void FillProcedureFrom()
        {

            var objList = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(ViewState["ProceduresitemID"].ToString()));
            if ((objList != null))
            {


                if (!gets(objList.Filepath).Equals(""))
                {
                    lnkScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnScannerfilepath.Value = gets(objList.Filepath);
                }

                lstAttachmentType.SelectedValue = gets(objList.AttachmenttypeCode);
                txtRef.Text = gets(objList.AttachRef);
                txtCreationDate.Text = gets(objList.ReceiveDate);
                lblimage.Text = "<a target='_blank' href='" + ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList=[" + gets(objList.Filepath) + ";]'>View File</a>";
            }

            divAddProcedure.Visible = true;
            divshowProcesure.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private int SaveAttachmentInformation()
        {
            string script = "";
            int _TragetID = 0;
            try
            {
                Medalmain_Attachments objAttachment = new Medalmain_Attachments();
               

                if (ViewState["ProceduresitemID"].Equals("0"))
                {//Save

                    objAttachment.MedalMasterCode = ZeroIntergerIFNull(hdnMasterID.Value);
                    objAttachment.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    objAttachment.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);

                    objAttachment.UploadDate = DateTime.Now;
                   
                    objAttachment.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    objAttachment.AttachRef = txtRef.Text;

                    objRepository.AddAttachementmain(objAttachment);
                    _TragetID = objAttachment.Code;

                }
                else
                { //Update


                    objAttachment = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(ViewState["ProceduresitemID"].ToString()));

                    objAttachment.MedalMasterCode = ZeroIntergerIFNull(hdnMasterID.Value);

                    objAttachment.AttachmenttypeCode = ZeroIntergerIFNull(lstAttachmentType.SelectedValue);
                    objAttachment.ReceiveDate = NullDateifEmpty(txtCreationDate.Text);

                   
                    objAttachment.Uploadedby = ZeroIntergerIFNull(ReadSession("userid").ToString()); ;
                    objAttachment.AttachRef = txtRef.Text;
                    _TragetID = objAttachment.Code;
                    objRepository.UpdateAttachmentMain(objAttachment);

                }

                ViewState["ProceduresitemID"] = objAttachment.Code;
                //Upload LocalFile
                if (txtImage.FileName != "")
                {
                    string _img = UploadFileoServer(txtImage, ScannerRepository + _TargetUploadPath + gets(objAttachment.Code) + "/");
                    if (_img != "")
                    {
                        var objForEdit = objRepository.GetAttachemtnMainDetails(objAttachment.Code);
                        objForEdit.Filepath = _img;
                        objRepository.UpdateAttachment(objForEdit);
                    }
                }
                CleaProcesure();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            FillMedalProcedure(ZeroIntergerIFNull(hdnMasterID.Value));

            return _TragetID;

        }

        #endregion

        #region "Persons"

        private void SavePersons(int MedalCode)
        {
            if (Session["PersonsList"] != null)
            {
                List<Medal_Persons> _PersonsList = new List<Medal_Persons>();
                _PersonsList = (List<Medal_Persons>)Session["PersonsList"];

                // Separate into new and existing persons
                List<Medal_Persons> newPersons = new List<Medal_Persons>();
                List<Medal_Persons> existingPersons = new List<Medal_Persons>();

                // Filter out placeholder rows and categorize
                foreach (var person in _PersonsList)
                {
                    // Skip placeholder rows (empty rows without data)
                    if (IsPlaceholderRow(person))
                    {
                        continue;
                    }

                    if (person.Code == 0 || person.Code == -1)
                    {
                        // New person
                        person.MedalmasterID = MedalCode;
                        person.Medal_M_Types = null;
                        person.Medal_M_jobGrade = null;
                        newPersons.Add(person);
                    }
                    else
                    {
                        // Existing person - prepare for update
                        person.Medal_M_jobGrade = null;
                        person.Medal_M_Types = null;
                        existingPersons.Add(person);
                    }
                }

                // Batch size for processing
                int batchSize = 200;

                // Add new persons in batches
                if (newPersons.Count > 0)
                {
                    for (int i = 0; i < newPersons.Count; i += batchSize)
                    {
                        var batch = newPersons.Skip(i).Take(batchSize).ToList();
                        objRepository.AddPersonsBatch(batch);
                    }
                }

                // Update existing persons in batches
                if (existingPersons.Count > 0)
                {
                    // First, fetch all existing records to update
                    List<Medal_Persons> personsToUpdate = new List<Medal_Persons>();

                    foreach (var person in existingPersons)
                    {
                        var objforupdate = objRepository.getpersonDetails(person.Code);
                        if (objforupdate != null)
                        {
                            objforupdate.Medal_M_jobGrade = null;
                            objforupdate.Medal_M_Types = null;
                            objforupdate.CivilID = person.CivilID;
                            objforupdate.GradeID = person.GradeID;
                            objforupdate.GrantReason = person.GrantReason;
                            objforupdate.GrantReasonText = person.GrantReasonText;
                            objforupdate.Person_NameAr = person.Person_NameAr;
                            objforupdate.Person_NameEn = person.Person_NameEn;
                            objforupdate.MedalType = person.MedalType;
                            objforupdate.MilitaryNum = person.MilitaryNum;
                            objforupdate.PersonTitle = person.PersonTitle;
                            objforupdate.CountryName = person.CountryName;
                            objforupdate.CountryId = person.CountryId;

                            personsToUpdate.Add(objforupdate);
                        }
                    }

                    // Update in batches
                    for (int i = 0; i < personsToUpdate.Count; i += batchSize)
                    {
                        var batch = personsToUpdate.Skip(i).Take(batchSize).ToList();
                        objRepository.UpdatePersonsBatch(batch);
                    }
                }

                // Final garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        protected void grdInboundItems_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                objRepository.DeleteMedal((Medal_Data)objRepository.GetDetails(ZeroIntergerIFNull(e.Item.Cells[3].Text)));
            }
            FillMedals();
        }

        /// <summary>
        /// Handle pager command for grdPersons pagination
        /// </summary>
        protected void pagerPersons_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = ((int)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdPersons.PageCount))
            {
                currnetPageIndx = (grdPersons.PageCount - 1);
            }

            // Set both pager and grid indices BEFORE FillMedalPersons
            pagerPersons.CurrentIndex = currnetPageIndx;
            grdPersons.CurrentPageIndex = (currnetPageIndx - 1);

            // Reload the data for the new page
            FillMedalPersons(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
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
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='MedalMasterID' name='MedalMasterID' value='" + hdnMasterID.Value + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";

                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += "  CalllerForm.submit();";
                ScannerPostFrom += "</script>";

                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;

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
                                if (obj!=null)
                                {
                                    obj.Filepath = null;
                                    objRepository.UpdateAttachmentMain(obj);
                                }
                            }
                              
                            else
                            {
                                obj = objRepository.GetAttachemtnMainDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                if (obj != null)
                                {
                                    obj.Filepath = FileList[i].Split(',')[0].ToString();
                                    objRepository.UpdateAttachmentMain(obj);
                                }
                              
                            }




                        }

                        Response.Redirect("/Modules/Medals/Forms/MedalsData.aspx?activetab=2&id=" + Request.Form["MedalMasterID"]);

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

        #region "Linked Docs"

        //private void FillLinkedDocs(int DocRefID)
        //{

        //    var objList = objRepository.GetRelatedDocs(DocRefID);

        //    lblLinkCount.Text = objList.Count.ToString();



        //    grdLinkedDocs.DataSource = objList;
        //    grdLinkedDocs.DataBind();

        //}
        #endregion

    }
}
