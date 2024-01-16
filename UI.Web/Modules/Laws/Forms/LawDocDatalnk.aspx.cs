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

namespace UI.Web.Modules.Laws.Forms
{
    public partial class LawDocDatalnk : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();
        public MedalsRepository MedalobjRepository = IoC.Resolve<MedalsRepository>();
        public AgreementsRepository agreemtnyRepository = IoC.Resolve<AgreementsRepository>();


        public string _PageTitle = "نظام التشريعات  ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LawsAttachments/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        public string selectedChapter = "0";
        public string QrelatedOrg = "";
        #endregion

        #region "Page Events"

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "LawDocData.aspx";
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


                if (Request.QueryString["docTypeId"] != null)
                {
                    lstFilterType.SelectedValue = gets(Request.QueryString["docTypeId"]);
                    FillLawDocs();

                }

                if (Request.QueryString["ispublish"] != null)
                {
                    lstFilterPublish.SelectedValue = "1";
                    FillLawDocs();

                }

                if (Request.QueryString["ProjectId"] != null && Request.QueryString["ProjectId"].ToString() != "0")
                {
                    lstFilterType.SelectedValue = "1";
                    //lstFilterType.Enabled = false;
                }
                if (Request.QueryString["MedalSourceId"] != null && Request.QueryString["MedalSourceId"].ToString() != "0")
                {
                    lstFilterType.SelectedValue = "4";
                    //lstFilterType.Enabled = false;
                }
                ViewState["SpChapterChanged"] = "0";
                ViewState["SpEdit"] = "0";
                ViewState["NewDesc"] = "";
                ViewState["NewBar"] = "";
                ViewState["NewIsbn"] = "";
                ViewState["SPITEM"] = "";
                ViewState["NewPrice"] = "0";
                Session["ItemList"] = null;
                ViewState["itemID"] = "0";
                ViewState["LawDocitemID"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["LawDocArcID"] = "0";
                ViewState["AttachitemID"] = "0";
                ViewState["ProcedureCode"] = "0";


                if (Request.QueryString["LawDocID"] == null)
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


                    ViewState["itemID"] = Request.QueryString["LawDocID"].ToString();
                    FillLawDocMasterInformation();


                }
                if (Request.QueryString["lawDocRefId"] != null && Request.QueryString["lawDocRefId"].ToString() != "0")
                {
                    tblshow.Visible = false;
                    tblSearch.Visible = false;
                    tblAdd.Visible = true;


                    ViewState["itemID"] = Request.QueryString["lawDocRefId"].ToString();
                    FillLawDocMasterInformation();
                }
                else
                {
                    tblshow.Visible = false;
                    tblSearch.Visible = true;
                    tblAdd.Visible = false;
                    // FillLawDocs();
                }



                SetPageTitle();

                ViewState["OutboundItemID"] = "0";

                // FillInboundItems();
                UpdateScannedFile();

            }

        }

        protected void grdLawDocsList_ItemDataBound(object sender, DataGridItemEventArgs e)
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

                var objUnitList = objRepository.FillLawProcedures(ZeroIntergerIFNull(Filecode));
                if (objUnitList != null)
                {
                    DataGrid grd = ((DataGrid)(e.Item.Cells[1].FindControl("grdDocProcedures")));
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

            //Hide Defult Dates

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //if (e.Item.Cells[13].Text == "01/01/1990") e.Item.Cells[13].Text = "";
                //if (e.Item.Cells[15].Text == "01/01/1990") e.Item.Cells[15].Text = "";
                //if (e.Item.Cells[16].Text == "01/01/1990") e.Item.Cells[16].Text = "";

                HtmlAnchor file1 = ((HtmlAnchor)(e.Item.Cells[2].FindControl("file1")));
                HtmlAnchor file2 = ((HtmlAnchor)(e.Item.Cells[2].FindControl("file2")));
                if (e.Item.Cells[4].Text == "" || e.Item.Cells[4].Text == "&nbsp;")
                {
                    file1.Visible = false;

                }
                if (e.Item.Cells[5].Text == "" || e.Item.Cells[5].Text == "&nbsp;")
                {
                    file2.Visible = false;

                }




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
            ViewState["LawDocitemID"] = "0";
            ViewState["ProceduresitemID"] = "0";
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
            FillLawDocs();
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
            ViewState["LawDocitemID"] = "0";
            ViewState["ProceduresitemID"] = "0";
            Session["PersonsList"] = null;
            // Response.Redirect("/Modules/laws/Forms/LawDocData.aspx");
            FillLawDocs();

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillLawDocs();
        }
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            SaveLawDocMaster();
        }


        protected void lnkAddNewLawDoc_Click(object sender, EventArgs e)
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
        private string MapSearchKeys()
        {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                _keyList.Add(" رقم الوثيقة ", txtFilterSerialNum.Text);
                _keyList.Add("   سنة الاصدار   ", txtFilterSerialYear.Text);
                _keyList.Add(" تاريخ  إصدار الوثيقة من ", txtFilterDatefrom.Text);
                _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                _keyList.Add(" جزء من نص الوثيقة    ", txtFilterDetails.Text);
                _keyList.Add(" نوع الوثيقة  ", lstFilterType.SelectedItem.Text);
                _keyList.Add(" قيد الدراسة    ", lstFilterIsUnderStudy.SelectedItem.Text);
                _keyList.Add("  جزء من الموضوع ", txtFilterSubject.Text);
                _keyList.Add("نشر بالجريدة الرسمية", lstFilterPublish.Text);

            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }

        private void FillLawDocs()
        {


            //Get Selectd Persons

            var objList = objRepository.GetList(ZeroIntergerIFNull(txtFilterSerialNum.Text), ZeroIntergerIFNull(txtFilterSerialYear.Text),
                 NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterType.SelectedValue), ZeroIntergerIFNull(lstFilterCategory.SelectedValue),
               ZeroIntergerIFNull(lstFilterIsUnderStudy.SelectedValue), txtFilterSubject.Text,
               txtFilterDetails.Text, ZeroIntergerIFNull(lstFilterPublish.SelectedValue), ZeroIntergerIFNull(lstFilterprocedureType.SelectedValue), NullDateifEmpty(txtFilterExpireDateFrom.Text), NullDateifEmpty(txtFilterExpireDateTo.Text), MapSearchKeys());


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

            var duplicatedList = objList.SelectMany(t =>
           Enumerable.Repeat(t, 2)).ToList();
          

            grdLawDocsList.DataSource = duplicatedList;
            grdLawDocsList.DataBind();
            pager1.ItemCount = duplicatedList.Count;

        }

        private void ClearCaseForm()
        {
            ViewState["LawDocitemID"] = "0";
            txtDocSerialNum.Text = "";
            txtDocSerialYear.Text = "";
            txtSubject.Text = "";
            txtNotes.Text = "";
            txtDetails.Text = "";
            txtVersionNum.Text = "";
            lstDocType.SelectedValue = "0";
            lstCategory.SelectedValue = "0";

            txtDocDate.Text = "";
            txtPublishDate.Text = "";


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
            if (hasattachment != "")
            {
                return "";
            }
            return "display:none";

        }

        public string GetQStatus(int StatusID, string StatusName)
        {
            string _out = "";
            switch (StatusID)
            {
                case 1:
                    {
                        _out = "<span class='label bg-success-400'>" + StatusName + "  </span>";
                        break;
                    }
                case 2:
                    {
                        _out = "<span class='label bg-warning-400'>" + StatusName + " </span>";
                        break;
                    }
                case 3:
                    {
                        _out = "<span class='label bg-blue-400'>" + StatusName + "</span>";
                        break;
                    }
                case 4:
                    {
                        _out = "<span class='label bg-grey-400'>" + StatusName + "</span>";
                        break;
                    }

                default:
                    {
                        _out = "<span class='label bg-grey-400'> " + StatusName + "</span>";
                        break;
                    }
            }
            return _out;
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

            if ((currnetPageIndx > grdLawDocsList.PageCount))
            {
                currnetPageIndx = (grdLawDocsList.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdLawDocsList.CurrentPageIndex = (currnetPageIndx - 1);
            FillLawDocs();
        }


        #endregion

        #region "Helper Methods"


        private void FillLawDocMasterInformation()
        {

            var objList = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {
                hdnMasterID.Value = gets(objList.Code);
                ViewState["LawDocitemID"] = gets(objList.Code);

                txtDocSerialNum.Text = gets(objList.DocNum);
                txtDocSerialYear.Text = gets(objList.DocYear);

                txtNotes.Text = gets(objList.DocNotes);
                txtDetails.Text = gets(objList.DocDetails);

                txtSubject.Text = gets(objList.DocSubject);

                txtDocDate.Text = NullDateifEmptyToText(objList.DocDate).ToString();
                txtPublishDate.Text = NullDateifEmptyToText(objList.PublishDate).ToString();

                lstDocType.SelectedValue = gets(objList.DocTypeID);
                lstCategory.SelectedValue = gets(objList.DocCategoryID);
                try
                {
                    lstkngDession.SelectedValue = gets(objList.kng_Dession);
                }
                catch (Exception)
                {


                }




                chkIspublished.Checked = getBool(objList.isPublished);
                chkIsUnderStudy.Checked = getBool(objList.UnderStudy);

                txtVersionNum.Text = gets(objList.PublishVersion);
                if (!gets(objList.DocFilepath).Equals(""))
                {
                    anchorAttachment.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList=[" + gets(objList.DocFilepath) + ";]"; // "LawsAttachments.aspx?FileID=" + gets(objList.code);
                                                                                                                                                              //anchorAttachment.Attributes.Add("click", "openScannerViewer('"+ _TargetUrl + "',3,"+ objList.Code + ",2,'"+ _TargetUploadPath + "','','["+ gets(objList.DocFilepath) + "]','"+ System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "')");
                                                                                                                                                              //  anchorAttachment.Attributes.Add("onclick", "openDocFile();");
                    anchorAttachment.Visible = true;
                }


                if (!gets(objList.DocFilepath_published).Equals(""))
                {
                    anchorAttachment2.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList=[" + gets(objList.DocFilepath_published) + ";]"; // "LawsAttachments.aspx?FileID=" + gets(objList.code);
                    //anchorAttachment2.Attributes.Add("click", "openScannerViewer('" + _TargetUrl + "',3," + objList.Code + ",2,'" + _TargetUploadPath + "','','[" + gets(objList.DocFilepath) + "]','" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "')");
                    anchorAttachment2.Visible = true;
                }

                //FillFileAttachment(objList.Code);
                FilLLawDocProcedures(objList.Code);

            }
            tblAdd.Visible = true;
            tblshow.Visible = false;
            tblSearch.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private int SaveLawDocMaster(bool fromScan = false)
        {
            string script = "";
            //Upload LocalFile
            string _img = UploadFileoServer(txtDocImage, ScannerRepository + _TargetUploadPath);
            string _img2 = UploadFileoServer(txtDocImage2, ScannerRepository + _TargetUploadPath);
            Law_DocData objLawDoc = new Law_DocData();

            try
            {
                if (ViewState["itemID"].Equals("0"))
                {//Save


                    if (objRepository.CheckDocExistance(ZeroIntergerIFNull(txtDocSerialNum.Text), ZeroIntergerIFNull(txtDocSerialYear.Text), ZeroIntergerIFNull(lstDocType.SelectedValue), 0))
                    {

                        script = FormatpopupErrorMSG(" [ رقم الوثيقة   مسجل من قبل  ]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return 0;
                    }


                    objLawDoc.CreationDate = DateTime.Now;
                    objLawDoc.LastModificationDate = DateTime.Now;
                    objLawDoc.DocTypeID = ZeroIntergerIFNull(lstDocType.SelectedValue);

                    objLawDoc.DocSerial = gets(txtDocSerialYear.Text) + "/" + gets(txtDocSerialNum.Text);
                    objLawDoc.DocNum = ZeroIntergerIFNull(txtDocSerialNum.Text);
                    objLawDoc.DocYear = ZeroIntergerIFNull(txtDocSerialYear.Text);


                    objLawDoc.DocSubject = txtSubject.Text;
                    objLawDoc.DocDate = NullDateifEmpty(txtDocDate.Text);
                    objLawDoc.DocCategoryID = ZeroIntergerIFNull(lstCategory.SelectedValue);

                    objLawDoc.UnderStudy = getBool(chkIsUnderStudy.Checked);
                    objLawDoc.isPublished = getBool(chkIspublished.Checked);
                    objLawDoc.PublishDate = NullDateifEmpty(txtPublishDate.Text);
                    objLawDoc.PublishVersion = txtVersionNum.Text;

                    objLawDoc.DocNotes = gets(txtNotes.Text);

                    objLawDoc.DocDetails = gets(txtDetails.Text);
                    objLawDoc.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                    if (_img != "")
                    {
                        objLawDoc.DocFilepath = _img;

                    }

                    if (_img2 != "")
                    {
                        objLawDoc.DocFilepath_published = _img2;

                    }
                    objLawDoc.kng_Dession = gets(lstkngDession.Text);
                    objRepository.AddLaw(objLawDoc);
                    hdnMasterID.Value = gets(objLawDoc.Code);
                    ViewState["itemID"] = gets(objLawDoc.Code);

                }
                else
                { //Update


                    if (objRepository.CheckDocExistance(ZeroIntergerIFNull(txtDocSerialNum.Text), ZeroIntergerIFNull(txtDocSerialYear.Text), ZeroIntergerIFNull(lstDocType.SelectedValue), ZeroIntergerIFNull(ViewState["itemID"].ToString())))
                    {
                        script = FormatpopupErrorMSG(" [ رقم الوثيقة   مسجل من قبل  ]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return 0;
                    }


                    hdnMasterID.Value = ViewState["itemID"].ToString();
                    objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    objLawDoc.LastModificationDate = DateTime.Now;
                    objLawDoc.DocTypeID = ZeroIntergerIFNull(lstDocType.SelectedValue);

                    objLawDoc.DocSerial = gets(txtDocSerialYear.Text) + "/" + gets(txtDocSerialNum.Text);
                    objLawDoc.DocNum = ZeroIntergerIFNull(txtDocSerialNum.Text);
                    objLawDoc.DocYear = ZeroIntergerIFNull(txtDocSerialYear.Text);


                    objLawDoc.DocSubject = txtSubject.Text;
                    objLawDoc.DocDate = NullDateifEmpty(txtDocDate.Text);
                    objLawDoc.DocCategoryID = ZeroIntergerIFNull(lstCategory.SelectedValue);

                    objLawDoc.UnderStudy = getBool(chkIsUnderStudy.Checked);
                    objLawDoc.isPublished = getBool(chkIspublished.Checked);
                    objLawDoc.PublishDate = NullDateifEmpty(txtPublishDate.Text);
                    objLawDoc.PublishVersion = txtVersionNum.Text;

                    objLawDoc.DocNotes = gets(txtNotes.Text);
                    objLawDoc.DocDetails = gets(txtDetails.Text);

                    objLawDoc.kng_Dession = gets(lstkngDession.Text);


                    objLawDoc.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    if (_img != "")
                    {
                        objLawDoc.DocFilepath = _img;
                    }

                    if (_img2 != "")
                    {
                        objLawDoc.DocFilepath_published = _img2;

                    }

                    objRepository.UpdateLaw(objLawDoc);
                }

                ViewState["itemID"] = objLawDoc.Code;

                anchorAttachment.Visible = true;
                anchorAttachment.HRef = "LawsAttachments.aspx?FileID=" + hdnMasterID.Value;

                //  ClearForm();

                script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }

            if (!fromScan)
            {
                Response.Redirect("LawDocData.aspx?ss=1&LawDocID=" + gets(objLawDoc.Code));
                return 0;
            }
            return objLawDoc.Code;
        }
        private void fillLookups()
        {


            FillDllwithoptional_ALL(objLookup.FillLaw_DocType(), ref lstDocType, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillLaw_DocType(), ref lstFilterType, "NameAr", "Code", "الكل");


            FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(), ref lstCategory, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(), ref lstFilterCategory, "NameAr", "Code", "الكل");



            FillDllwithoptional_ALL(objLookup.Filllaw_DocProceduresTypes(), ref lstprocedureType, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.Filllaw_DocProceduresTypes(), ref lstFilterprocedureType, "NameAr", "Code", "الكل");


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


        #region "Scanning"
        //protected void lnkScan_Click(object sender, EventArgs e)
        //{

        //    string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
        //    string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
        //    //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";

        //    int _TargetID = SaveAttachmentInformation();

        //    if (_TargetID != 0)
        //    {

        //        //ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + ViewState["DocID"].ToString() + "' />";
        //        //ScannerPostFrom += "<input type='hidden' id='LawDocID' name='LawDocID' value='" + ViewState["LawDocID"].ToString() + "' />";
        //        //ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";

        //        //target='_blank'
        //        string ScannerPostFrom = "<form id='ScannerCalllerForm'   action='" + _TargetUrl + "' method='POST'>";
        //        ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
        //        ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
        //        ScannerPostFrom += "<input type='hidden' id='action' name='action' value='1' />";
        //        ScannerPostFrom += "<input type='hidden' id='AttachID' name='AttachID' value='" + _TargetID + "' />";
        //        //ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";

        //        //ScannerPostFrom += "<input type='hidden' id='LawDocID' name='LawDocID' value='" + _TargetID + "' />";
        //        //ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
        //        //ScannerPostFrom += "<input type='hidden' id='id' name='id' value='" + ZeroIntergerIFNull(ViewState["itemID"].ToString()).ToString() + "' />";


        //        ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
        //        ScannerPostFrom += "<input type='hidden' id='LawDocID' name='LawDocID' value='" + ViewState["LawDocitemID"].ToString() + "' />";
        //        ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
        //        ScannerPostFrom += "<input type='hidden' id='LawDocID' name='LawDocID' value='" + ViewState["itemID"].ToString() + "' />";


        //        ScannerPostFrom += "</form>";
        //        ScannerPostFrom += "<script>";
        //        ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
        //        ScannerPostFrom += "  CalllerForm.submit();";
        //        ScannerPostFrom += "</script>";

        //        ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;

        //        //_TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&AttachID=" + _TargetID  + "&TargetID=" + _TargetID;
        //        //Response.Redirect(_TargetUrl);
        //    }

        //}

        private void UpdateScannedFile()
        {
            Law_DocData objLawDoc = new Law_DocData();
            Law_DocProcedures objProcedure = new Law_DocProcedures();

            arc_Data objArc = new arc_Data();
            if (Request.Form["fileList"] != null)
            {
                if (Request.Form["fileList"].ToString() != "")
                {

                    switch (Request.Form["ActiveTab"].ToString())
                    {
                        case "1":
                            {//LawDoc Information


                                ViewState["itemID"] = Request.Form["TargetID"];
                                //  FillLawDocMasterInformation();
                                hdnactivetab.Value = "1";

                                string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                string[] FileList = _ScannerFileLlisy.Split(';');

                                if (Request.Form["TargetID"] != null)
                                {

                                    if (Request.Form["FileType"] != null)
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
                                                objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                                objLawDoc.DocFilepath_published = null;
                                                objRepository.UpdateLaw(objLawDoc);
                                            }
                                            else
                                            {
                                                objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                                objLawDoc.DocFilepath_published = FileList[i].Split(',')[0].ToString();
                                                objRepository.UpdateLaw(objLawDoc);
                                            }

                                        }
                                    }
                                    else
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
                                                objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                                objLawDoc.DocFilepath = null;
                                                objRepository.UpdateLaw(objLawDoc);
                                            }
                                            else
                                            {
                                                objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                                objLawDoc.DocFilepath = FileList[i].Split(',')[0].ToString();
                                                objRepository.UpdateLaw(objLawDoc);
                                            }



                                        }

                                    }

                                    Response.Redirect("LawDocData.aspx?activetab=1&LawDocID=" + Request.Form["TargetID"]);
                                    return;
                                }


                                else
                                {
                                    string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                }


                                break;
                            }
                        case "2":
                            {// Procedure Details


                                ViewState["ProcedureCode"] = Request.Form["TargetID"];
                                ViewState["itemID"] = Request.Form["FileID"];
                                //  FillLawDocMasterInformation();
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
                                            objProcedure = objRepository.GetProceduresDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objProcedure.Procedureattachments = null;
                                            objRepository.UpdateProcedures(objProcedure);
                                        }
                                        else
                                        {
                                            objProcedure = objRepository.GetProceduresDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            objProcedure.Procedureattachments = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateProcedures(objProcedure);
                                        }


                                    }
                                    Response.Redirect("LawDocData.aspx?activetab=2&LawDocID=" + Request.Form["FileID"]);
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


                                //ViewState["LawDocArcID"] = Request.Form["TargetID"];
                                //ViewState["itemID"] = Request.Form["FileID"];
                                ////  FillLawDocMasterInformation();
                                //hdnactivetab.Value = "3";

                                //string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                //_ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                //string[] FileList = _ScannerFileLlisy.Split(';');

                                //if (Request.Form["TargetID"] != null)
                                //{
                                //    for (int i = 0; i < FileList.Length; i++)
                                //    {//Update Current Record With

                                //        objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["LawDocArcID"].ToString()));
                                //        objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                //        objRepository.UpdateArcData(objArc);
                                //    }
                                //    Response.Redirect("LawDocData.aspx?activetab=3&LawDocID=" + Request.Form["FileID"]);
                                //    return;
                                //}


                                //else
                                //{
                                //    string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
                                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                //}

                                break;
                            }
                        case "4":
                            {//Outgoinung

                                //ViewState["LawDocArcID"] = Request.Form["TargetID"];
                                //ViewState["itemID"] = Request.Form["FileID"];
                                ////  FillLawDocMasterInformation();
                                //hdnactivetab.Value = "4";

                                //string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                //_ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                //string[] FileList = _ScannerFileLlisy.Split(';');

                                //if (Request.Form["TargetID"] != null)
                                //{
                                //    for (int i = 0; i < FileList.Length; i++)
                                //    {//Update Current Record With

                                //        objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["LawDocArcID"].ToString()));
                                //        objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                //        objRepository.UpdateArcData(objArc);
                                //    }
                                //    Response.Redirect("LawDocData.aspx?activetab=4&LawDocID=" + Request.Form["FileID"]);
                                //    return;
                                //}


                                //else
                                //{
                                //    string script = FormatpopupErrorMSG("Faild to save Scanned Files", "1");
                                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                //}

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
            int _TargetID = SaveLawDocMaster(true);



            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='1' />";
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


                //_TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&DocID=" + Request.QueryString["DocID"].ToString() + "&CaseID=" + Request.QueryString["CaseID"].ToString() + "&TargetID=" + _TargetID;
                //Response.Redirect(_TargetUrl);
            }

        }


        protected void lnkQScan_Click2(object sender, EventArgs e)
        {
            //Show Loadin div

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
                                                                           //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }
            int _TargetID = SaveLawDocMaster(true);



            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='1' />";
                ScannerPostFrom += "<input type='hidden' id='DocID' name='DocID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='CaseID' name='CaseID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileType' name='FileType' value='2' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='1' />";
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
        protected void btnprocedureScan_Click(object sender, EventArgs e)
        {

            //Show Loadin div

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
            int _TargetID = 0;                                                             //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }
            if (ViewState["LawDocitemID"].ToString() != "0" && ViewState["LawDocitemID"].ToString() != "")
            {
                _TargetID = SaveProcedureInformation(ZeroIntergerIFNull(ViewState["LawDocitemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save LawDoc Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='1' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["LawDocitemID"].ToString() + "' />";
                ScannerPostFrom += "<input type='hidden' id='ActiveTab' name='ActiveTab' value='2' />";
                ScannerPostFrom += "<input type='hidden' name='systemprofile' value='" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "' />";
                ScannerPostFrom += "</form>";
                ScannerPostFrom += "<script>";
                ScannerPostFrom += " var CalllerForm = document.forms['ScannerCalllerForm'];";
                ScannerPostFrom += " CalllerForm.submit();";
                ScannerPostFrom += "</script>";

                ((Literal)this.Master.FindControl("lScannerForm")).Text = ScannerPostFrom;


                //_TargetUrl += "?Targetpath=" + _TargetUploadPath + "&CallbackURL=" + _CallBackUrl + "&action=1&DocID=" + Request.QueryString["DocID"].ToString() + "&CaseID=" + Request.QueryString["CaseID"].ToString() + "&TargetID=" + _TargetID;
                //Response.Redirect(_TargetUrl);
            }


        }

        #endregion

        //#endregion


        #region "Procedure Methods"

        #region "Procedure HElpers"
        private void FillProcedureForm()
        {

            var objList = objRepository.GetProceduresDetails(ZeroIntergerIFNull(ViewState["ProcedureCode"].ToString()));
            if ((objList != null))
            {
                txtProcedureNotes.Text = gets(objList.ProcedureNotes);
                txtProcedureText.Text = gets(objList.ProcedureSubject);
                txtProcedureDate.Text = NullDateifEmptyToText(objList.ProcedureDate);
                txtProcedureLetterNum.Text = gets(objList.ProcedureletterNum);

                lstprocedureType.SelectedValue = gets(objList.ProcedureTypeID);

                txtOmaDate.Text = NullDateifEmptyToText(objList.SendDate);
                chkOmaSend.Checked = getBool(objList.SendToOma);



            }

            DivAddProcedure.Visible = true;
            divShowProcedure.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private void FilLLawDocProcedures(int DocRefID)
        {

            var objList = objRepository.FillLawProcedures(DocRefID);
            //  lblComingCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
            lblProcedureCount.Text = objList.Count.ToString();
            decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdProcedureList.PageSize));
            if ((c <= grdProcedureList.CurrentPageIndex))
            {
                grdProcedureList.CurrentPageIndex = 0;
            }

            //List Duplication
            //List<View_InboundItems> duplicatedList = new List<View_InboundItems>();
            //duplicatedList = DuplicatedList(objList);

            if (objList.Count > 0)
            {
                //btnSave.Visible = true;
                //lnkBack.Visible = true;

                divShowProcedure.Visible = true;
                pager5.Visible = true;
                grdProcedureList.Visible = true;

            }


            //var duplicatedList = objList.SelectMany(t =>
            //  Enumerable.Repeat(t, 2)).ToList();

            grdProcedureList.DataSource = objList;
            grdProcedureList.DataBind();
            pager5.ItemCount = objList.Count;
        }
        private void ClearProcedureforms()
        {
            ViewState["ProcedureCode"] = "0";
            txtProcedureDate.Text = "";
            txtOmaDate.Text = "";
            txtProcedureNotes.Text = "";
            txtProcedureText.Text = "";

            DivAddProcedure.Visible = false;
            divShowProcedure.Visible = true;



        }
        private int SaveProcedureInformation(int RefDocID)
        {
            string script = "";
            Law_DocProcedures obj = new Law_DocProcedures();
            try
            {

                string _img = UploadFileoServer(txtProcedureimage, ScannerRepository + _TargetUploadPath);

                if (ViewState["ProcedureCode"].Equals("0"))
                {//Save
                    obj.CreationDate = DateTime.Now;
                    obj.LastActionDate = DateTime.Now;

                    obj.DocRefID = RefDocID;
                    obj.ProcedureNotes = txtProcedureNotes.Text;
                    obj.ProcedureDate = NullDateifEmpty(txtProcedureDate.Text);
                    obj.SendDate = NullDateifEmpty(txtOmaDate.Text);
                    obj.SendToOma = getBool(chkOmaSend.Checked);
                    obj.ProcedureSubject = gets(txtProcedureText.Text);

                    obj.ProcedureletterNum = gets(txtProcedureLetterNum.Text);
                    obj.ProcedureTypeID = ZeroIntergerIFNull(lstprocedureType.SelectedValue);

                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    if (_img != "")
                    {
                        obj.Procedureattachments = _img;
                    }
                    objRepository.AddProcedures(obj);

                }
                else
                { //Update


                    obj = objRepository.GetProceduresDetails(ZeroIntergerIFNull(ViewState["ProcedureCode"].ToString()));
                    obj.LastActionDate = DateTime.Now;

                    obj.DocRefID = RefDocID;
                    obj.ProcedureNotes = txtProcedureNotes.Text;
                    obj.ProcedureDate = NullDateifEmpty(txtProcedureDate.Text);
                    obj.SendDate = NullDateifEmpty(txtOmaDate.Text);
                    obj.SendToOma = getBool(chkOmaSend.Checked);
                    obj.ProcedureSubject = gets(txtProcedureText.Text);

                    obj.ProcedureletterNum = gets(txtProcedureLetterNum.Text);
                    obj.ProcedureTypeID = ZeroIntergerIFNull(lstprocedureType.SelectedValue);


                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    if (_img != "")
                    {
                        obj.Procedureattachments = _img;
                    }

                    objRepository.UpdateProcedures(obj);

                }

                ViewState["ProcedureCode"] = obj.Code;

                ClearProcedureforms();



            }
            catch (Exception ex)
            {


                script = FormatpopupErrorMSG(Resources.Alerts.FailToSaveData + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            FilLLawDocProcedures(RefDocID);
            return obj.Code;
        }


        #endregion
        protected void grdProcedureList_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["ProcedureCode"] = e.Item.Cells[0].Text;
            DivAddProcedure.Visible = true;
            divShowProcedure.Visible = false;
            FillProcedureForm();

        }
        protected void lnkAddProcedure_Click(object sender, EventArgs e)
        {
            ViewState["ProcedureCode"] = "0";
            DivAddProcedure.Visible = true;
            divShowProcedure.Visible = false;
        }

        protected void lnkSaveProcedure_Click(object sender, EventArgs e)
        {
            if (ViewState["LawDocitemID"].ToString() != "0" && ViewState["LawDocitemID"].ToString() != "")
            {
                SaveProcedureInformation(ZeroIntergerIFNull(ViewState["LawDocitemID"].ToString()));
                string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save LawDoc Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }


        }

        protected void lnkCancelProcedure_Click(object sender, EventArgs e)
        {
            ClearProcedureforms();

        }

        protected void pager_Command5(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdProcedureList.PageCount))
            {
                currnetPageIndx = (grdProcedureList.PageCount - 1);
            }

            pager5.CurrentIndex = currnetPageIndx;
            grdProcedureList.CurrentPageIndex = (currnetPageIndx - 1);
            FilLLawDocProcedures(ZeroIntergerIFNull(ViewState["LawDocitemID"].ToString()));
        }
        protected void grdProcedureList_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }

        protected void lnkDeleteProcedure_Click(object sender, EventArgs e)
        {
            try
            {


                for (int i = 0; i <= grdProcedureList.Items.Count - 1; i++)
                {

                    if ((grdProcedureList.Items[i].FindControl("chkItem") != null))
                    {
                        CheckBox check = (CheckBox)grdProcedureList.Items[i].FindControl("chkItem");

                        if (check.Checked)
                        {
                            objRepository.DeleteProcedures((Law_DocProcedures)objRepository.GetProceduresDetails(ZeroIntergerIFNull(grdProcedureList.Items[i].Cells[0].Text)));
                        }
                    }
                }
                FilLLawDocProcedures(ZeroIntergerIFNull(ViewState["LawDocitemID"].ToString()));

            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteDataFailed + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
        }

        #endregion



        protected void grdLawDocsList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                //Delete Related Data


                //for (int i = 0; i <= grdProcedureList.Items.Count - 1; i++)
                //{

                //    if ((grdProcedureList.Items[i].FindControl("chkItem") != null))
                //    {
                //        CheckBox check = (CheckBox)grdProcedureList.Items[i].FindControl("chkItem");

                //        if (check.Checked)
                //        {
                //            objRepository.DeleteProcedures((Law_DocProcedures)objRepository.GetProceduresDetails(ZeroIntergerIFNull(grdProcedureList.Items[i].Cells[0].Text)));
                //        }
                //    }
                //




                objRepository.DeleteLaw((Law_DocData)objRepository.GetDetails(ZeroIntergerIFNull(e.Item.Cells[3].Text)));
            }
            else if (e.CommandName == "link")
            {
                if (Request.QueryString["agreementid"] != null)
                {
                    //Update Agreemrnt]
                    AgreementData objagreement = new AgreementData();
                    objagreement = agreemtnyRepository.GetDetails(ZeroIntergerIFNull(Request.QueryString["agreementid"].ToString()));

                    if (objagreement != null)
                    {
                        objagreement.Law_DocDataRefId = ZeroIntergerIFNull(e.Item.Cells[3].Text);
                        agreemtnyRepository.UpdateAgreement(objagreement);

                        string script = FormatpopupErrorMSG("تم الربط بنجاح", "3");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);


                        //Colse Popup


                        litScript.Text = "parent.$.fn.colorbox.close();";
                    }

                }
            }

            else if (e.CommandName == "LawDoclink")
            {
                if (Request.QueryString["SouceID"] != null)
                {
                    //Update Agreemrnt] SouceID
                    Law_DocData_Linked objlink = new Law_DocData_Linked();
                    if (!objRepository.CheckLinkExostance(ZeroIntergerIFNull(Request.QueryString["SouceID"].ToString()), ZeroIntergerIFNull(e.Item.Cells[3].Text)))
                    {
                        objlink.SouceDocID = ZeroIntergerIFNull(Request.QueryString["SouceID"].ToString());
                        objlink.DestDocId = ZeroIntergerIFNull(e.Item.Cells[3].Text);
                        objlink.TransDate = DateTime.Now;
                        objlink.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());
                        objRepository.AddLnkedDoc(objlink);

                        string script = FormatpopupErrorMSG("تم الربط بنجاح", "3");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        //Colse Popup

                        litScript.Text = "parent.$.fn.colorbox.close();";

                    }
                    else
                    {
                        string script = FormatpopupErrorMSG("عفوا تم ربط الوثيقة من قبل ", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    }




                }


                if (Request.QueryString["ProjectId"] != null)
                {// Link Project By Law Doc
                    int DestDocId = ZeroIntergerIFNull(e.Item.Cells[3].Text);

                    var ProjectDetails = objRepository.GetDetails(ZeroIntergerIFNull(Request.QueryString["ProjectId"].ToString()));
                    if (ProjectDetails != null)
                    {

                        //Load Project Procedures
                        var objProjectProcedureList = objRepository.FillLawProcedures(ZeroIntergerIFNull(Request.QueryString["ProjectId"]));
                        if (objProjectProcedureList != null)
                        {
                            if (objProjectProcedureList != null && objProjectProcedureList.Count > 0)
                            {

                                Law_DocProcedures obj = new Law_DocProcedures();
                                foreach (var item in objProjectProcedureList)
                                {


                                    obj = new Law_DocProcedures();

                                    obj.DocRefID = DestDocId;
                                    obj.CreationDate = item.CreationDate;
                                    obj.LastActionDate = item.LastActionDate;
                                    obj.ProcedureNotes = item.ProcedureNotes;
                                    obj.ProcedureDate = item.ProcedureDate;
                                    obj.SendDate = item.SendDate;
                                    obj.SendToOma = item.SendToOma;
                                    obj.ProcedureSubject = item.ProcedureSubject;
                                    obj.ProcedureletterNum = item.ProcedureletterNum;
                                    obj.ProcedureTypeID = item.ProcedureTypeID;
                                    obj.aUser = item.aUser;
                                    obj.Procedureattachments = item.Procedureattachments;
                                    objRepository.AddProcedures(obj);


                                    //Copy Attachemnts
                                   
                                    if (item.Procedureattachments != null && gets(item.Procedureattachments) != "")
                                    {
                                        if (!CopyFile(gets(item.Procedureattachments), ScannerRepository + _TargetUploadPath + gets(Request.QueryString["projectId"]) + "/procedure/" + gets(item.Code) + "/", ScannerRepository + _TargetUploadPath + gets(DestDocId) + "/procedure/" + gets(obj.Code) + "/"))
                                        {
                                            lblerror.Text += "Copy faild <br/>";
                                            lblerror.Text += "Source : " + ScannerRepository + _TargetUploadPath + gets(Request.QueryString["projectId"]) + "/procedure/" + gets(item.Code) + "/";
                                            lblerror.Text += "Dest : " + ScannerRepository + _TargetUploadPath + gets(DestDocId) + "/procedure/" + gets(obj.Code) + "/";

                                        }


                                    }



                                }
                            }
                        }

                        //Update Linked Project
                        ProjectDetails.RefId = DestDocId;
                        ProjectDetails.isTransfered = true;
                        objRepository.UpdateLaw(ProjectDetails);


                        litScript.Text = "parent.$.fn.colorbox.close();";

                    }
                    else
                    {

                        string script = FormatpopupErrorMSG("عفوا المشروع غير موجود ", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                    }


                }

                if (Request.QueryString["MedalSourceId"] != null)
                {// Link Project By Law Doc
                    int DestDocId = ZeroIntergerIFNull(e.Item.Cells[3].Text);

                    var MedalDetails = MedalobjRepository.GetDetails(ZeroIntergerIFNull(Request.QueryString["MedalSourceId"].ToString()));
                    if (MedalDetails != null)
                    {
                        //Update Linked Project
                        MedalDetails.RelatedLawDocRefId = DestDocId;
                        MedalDetails.LinkDate = DateTime.Now;
                        MedalobjRepository.UpdateMedal(MedalDetails);

                        litScript.Text = "parent.$.fn.colorbox.close();";

                    }
                    else
                    {

                        string script = FormatpopupErrorMSG("عفوا الوسام غير موجود ", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                    }


                }


            }

            FillLawDocs();
        }
    }
}