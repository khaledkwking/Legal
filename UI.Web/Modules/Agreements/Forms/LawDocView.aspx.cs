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
    public partial class LawDocView : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();
        public AgreementsRepository agreemtnyRepository = IoC.Resolve<AgreementsRepository>();
        public string _PageTitle = "نظام التشريعات  ";

        public bool isCancelled = false;


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

                if (Request.QueryString["expire"] != null)
                {
                    txtFilterExpireFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFilterExpireTo.Text = DateTime.Now.AddMonths(6).ToString("dd/MM/yyyy");
                    FillLawDocs();
                
                }
                if (Request.QueryString["projectId"] != null)
                {
                    //Load Project Details
                    var ProjectDetails = objRepository.GetDetails(ZeroIntergerIFNull(Request.QueryString["projectId"].ToString()));
                    if (ProjectDetails != null)
                    {
                        txtSubject.Text = gets(ProjectDetails.DocSubject);
                    }
                    //Load Project Procedures
                    var objProjectProcedureList = objRepository.FillLawProcedures(ZeroIntergerIFNull(Request.QueryString["projectId"]));
                    if (objProjectProcedureList != null)
                    {
                        Session["ProjectProcedireList"] = objProjectProcedureList;
                        grdProcedureList.DataSource = objProjectProcedureList;
                        grdProcedureList.DataBind();
                        lblProcedureCount.Text = objProjectProcedureList.Count.ToString();
                    }


                    tblshow.Visible = false;
                    tblSearch.Visible = false;
                    tblAdd.Visible = true;

                    try
                    {
                        lstDocType.SelectedValue = "1"; //قانون
                        lstCategory.SelectedValue = "133";//قانون

                    }
                    catch (Exception)
                    {

                    }

                }

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

                ViewState["linkedAgreement"] = "0";


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

                if (getBool(gets(e.Item.Cells[6].Text)))
                {//Law Cancelled
                    e.Item.BackColor = System.Drawing.Color.FromArgb(242, 178, 171);
                }


                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                if (getBool(gets(e.Item.Cells[6].Text)))
                { e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#f2b2ab\';"); }
                else { e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';"); }



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
        public string GetStatus(int StatusID, string StatusName)
        {
            string _out = "";
            if (StatusID == 13 || StatusID == 8 || StatusID == 14) // استرداد | استدراك 
            {
                _out = "<span class='label bg-warning-400' style='white-space: normal;'>" + StatusName + " </span>";
            }
            //switch (StatusID)
            //{
            //    case 0:
            //        {
            //            _out = "";
            //            break;
            //        }
            //    case 1:
            //        {
            //            _out = "<span class='label bg-success-400' style='white-space: normal;'>" + StatusName + "  </span>";
            //            break;
            //        }
            //    case 2:
            //        {
            //            _out = "<span class='label bg-warning-400' style='white-space: normal;'>" + StatusName + " </span>";
            //            break;
            //        }
            //    case 3:
            //        {
            //            _out = "<span class='label bg-blue-400' style='white-space: normal;'>" + StatusName + "</span>";
            //            break;
            //        }
            //    case 4:
            //        {
            //            _out = "<span class='label bg-grey-400' style='white-space: normal;'>" + StatusName + "</span>";
            //            break;
            //        }

            //    case 5:
            //        {
            //            _out = "<span class='label bg-success-400' style='white-space: normal;'>" + StatusName + "  </span>";
            //            break;
            //        }
            //    case 6:
            //        {
            //            _out = "<span class='label bg-warning-400' style='white-space: normal;'>" + StatusName + " </span>";
            //            break;
            //        }
            //    case 7:
            //        {
            //            _out = "<span class='label bg-blue-400' style='white-space: normal;'>" + StatusName + "</span>";
            //            break;
            //        }
            //    case 8:
            //        {
            //            _out = "<span class='label bg-grey-400' style='white-space: normal;'>" + StatusName + "</span>";
            //            break;
            //        }

            //    default:
            //        {
            //            _out = "<span class='label bg-grey-400' style='white-space: normal;'> " + StatusName + "</span>";
            //            break;
            //        }
            //}
            return _out;
        }
        public string GetDocCats(int StatusID, string StatusName)
        {
            string _out = "";
            if (StatusID == 141) // مراسيم استرداد  |   
            {
                _out = "<span class='label bg-warning-400' style='white-space: normal;'>" + StatusName + " </span>";
            }
            else if (StatusID == 145)
            {
                _out = "<span class='label bg-blue-400' style='white-space: normal;color:#fff'>مرسوم رد </span>";

            }

            return _out;
        }

        public string viewlinkedfile(string filename)
        {
            return gets(filename).Equals("") ? "none" : "";
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
                _keyList.Add(" العمل التحضيري", lstfilterProceduretype.SelectedItem.Text);
                _keyList.Add(" نوع الوثيقة  ", lstFilterType.SelectedItem.Text);
                _keyList.Add(" قيد الدراسة    ", lstFilterIsUnderStudy.SelectedItem.Text);
                _keyList.Add(lblFilterCatTitle.Text, lstFilterCategory.SelectedItem.Text);
                _keyList.Add("  جزء من الموضوع ", txtFilterSubject.Text);
                _keyList.Add("نشر بالجريدة الرسمية", lstFilterPublish.Text);

                _keyList.Add(" تاريخ  انتهاء الوثيقة من ", txtFilterExpireFrom.Text);
                _keyList.Add(" الي   تاريخ  انتهاء  ", txtFilterExpireTo.Text);

            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }
        private void FillLawDocs()
        {



            var objList = objRepository.GetList(ZeroIntergerIFNull(txtFilterSerialNum.Text), ZeroIntergerIFNull(txtFilterSerialYear.Text),
                 NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterType.SelectedValue), ZeroIntergerIFNull(lstFilterCategory.SelectedValue),
               ZeroIntergerIFNull(lstFilterIsUnderStudy.SelectedValue), txtFilterSubject.Text,"", txtFilterDetails.Text,
               ZeroIntergerIFNull(lstFilterPublish.SelectedValue), ZeroIntergerIFNull(lstfilterProceduretype.SelectedValue), NullDateifEmpty(txtFilterExpireFrom.Text), NullDateifEmpty(txtFilterExpireTo.Text), MapSearchKeys());


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

            if (lstFilterType.SelectedValue == "1")// قانون
            {
                grdLawDocsList.Columns[13].Visible = false;
                grdLawDocsList.Columns[16].Visible = false;
                grdLawDocsList.Columns[15].Visible = true;
            }
            else if (lstFilterType.SelectedValue == "3")// مرسوم
            {
                grdLawDocsList.Columns[15].Visible = false;
                grdLawDocsList.Columns[16].Visible = true;
                grdLawDocsList.Columns[13].Visible = true;
            }
            else { grdLawDocsList.Columns[16].Visible = false;
                grdLawDocsList.Columns[13].Visible = true;
                grdLawDocsList.Columns[15].Visible = true;
            }
            grdLawDocsList.DataBind();
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
            txtEffectiveDate.Text = "";
            txtExpireDate.Text = "";
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
        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            lnkAddProcedure.Visible = userAccess.Add;
            lnkAddRelatedDoc.Visible = userAccess.Add;
            lnkAgreementLink.Visible = userAccess.Add;


            btnSave.Visible = userAccess.Edit || userAccess.Add;
            lnkCancelLawDoc.Visible = userAccess.Edit || userAccess.Add;


            grdLawDocsList.Columns[18].Visible = userAccess.Delete;
            lnkDeleteProcedure.Visible = userAccess.Delete;

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


                if (Request.QueryString["LawDocRefID"] != null && !gets(objList.DocFilepath_published).Equals(""))
                {//Redirect to 
                    Response.Redirect(ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + ViewState["itemID"].ToString() + "/" + "&vfileList=[" + gets(objList.DocFilepath_published) + ";]");
                    return;
                }


                hdnMasterID.Value = gets(objList.Code);
                ViewState["LawDocitemID"] = gets(objList.Code);

                txtDocSerialNum.Text = gets(objList.DocNum);
                txtDocSerialYear.Text = gets(objList.DocYear);

                txtNotes.Text = gets(objList.DocNotes);
                txtDetails.Text = gets(objList.DocDetails);

                txtSubject.Text = gets(objList.DocSubject);

                txtDocDate.Text = NullDateifEmptyToText(objList.DocDate).ToString();
                txtEffectiveDate.Text = NullDateifEmptyToText(objList.effectiveDate).ToString();
                txtExpireDate.Text = NullDateifEmptyToText(objList.ExpireDate).ToString();
                txtPublishDate.Text = NullDateifEmptyToText(objList.PublishDate).ToString();

                lstDocType.SelectedValue = gets(objList.DocTypeID);

                isCancelled = getBool(objList.LawCancelled);
                if (getBool(objList.LawCancelled))
                {
                    lnkCancelLawDoc.Visible = false;
                }



                if (gets(objList.DocTypeID) == "3")//مرسوم
                {
                    lblcattitle.Text = "تصنيف المرسوم <span class='text-danger'>*</span>:";
                    FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(ZeroIntergerIFNull(lstDocType.SelectedValue)), ref lstCategory, "NameAr", "Code", "إختر");
                }
                else
                {
                    lblcattitle.Text = "التصنيف <span class='text-danger'>*</span>:";
                    FillDllwithoptional_ALL(objLookup.FillLaw_DocZeroCategory(), ref lstCategory, "NameAr", "Code", "إختر");
                }

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
                    anchorAttachment.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(objList.Code) + "/" + "&vfileList=[" + gets(objList.DocFilepath) + ";]"; // "LawsAttachments.aspx?FileID=" + gets(objList.code);
                                                                                                                                                                                         //anchorAttachment.Attributes.Add("click", "openScannerViewer('"+ _TargetUrl + "',3,"+ objList.Code + ",2,'"+ _TargetUploadPath + "','','["+ gets(objList.DocFilepath) + "]','"+ System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "')");
                                                                                                                                                                                         //  anchorAttachment.Attributes.Add("onclick", "openDocFile();");
                    anchorAttachment.Visible = true;
                }



                if (!gets(objList.DocFilepath).Equals(""))
                {
                    lnkDocScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل ملف الوثيقة";
                    hdnScannerfilepath.Value = gets(objList.DocFilepath);
                    lnkDeleteFile.Visible = true;
                }
                else { lnkDocScan.Text = "<i class='icon-images2'></i>&nbsp;تصوير    "; lnkDeleteFile.Visible = false; }



                if (!gets(objList.DocFilepath_published).Equals(""))
                {
                    anchorAttachment2.HRef = ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(objList.Code) + "/" + "&vfileList=[" + gets(objList.DocFilepath_published) + ";]"; // "LawsAttachments.aspx?FileID=" + gets(objList.code);
                    //anchorAttachment2.Attributes.Add("click", "openScannerViewer('" + _TargetUrl + "',3," + objList.Code + ",2,'" + _TargetUploadPath + "','','[" + gets(objList.DocFilepath) + "]','" + System.Configuration.ConfigurationManager.AppSettings["systemprofile"].ToString() + "')");
                    anchorAttachment2.Visible = true;
                }
                else { anchorAttachment2.Visible = false; }


                if (!gets(objList.DocFilepath_published).Equals(""))
                {
                    btnScan2.Text = "<i class='icon-images2'></i>&nbsp;تعديل ملف الوثيقة";
                    hdnpublishedScannerfilepath.Value = gets(objList.DocFilepath_published);
                    lnkDeletePublished.Visible = true;
                }
                else { btnScan2.Text = "<i class='icon-images2'></i>&nbsp; تصوير  "; lnkDeletePublished.Visible = false; }

                //FillFileAttachment(objList.Code);
                FilLLawDocProcedures(objList.Code);
                FillLinkedDocs(objList.Code);

                lnkAddRelatedDoc.Visible = true;
                lnkAddRelatedDoc.InnerHtml = "<b><i class='glyphicon glyphicon-link'></i></b>إضافة تشريع مرتبط   ";
                lnkAddRelatedDoc.HRef = "/modules/laws/forms/LawDocDatalnk.aspx?SouceID=" + gets(objList.Code) + "&lawDocRefId=0";




                //Check Law Doc Agreement Link
                var LinkedAgreement = objRepository.GetlawDocLinkedAgreemt(objList.Code);
                if (LinkedAgreement != null)
                {
                    lnkAgreementLink.InnerHtml = "<b><i class='glyphicon glyphicon-link'></i></b>عرض إتفاقية مرتبطة";
                    lnkAgreementLink.HRef = "/modules/Agreements/forms/AgreementsDataLink.aspx?linkedId=" + gets(objList.Code) + "&AgreementRefId=" + LinkedAgreement.Code;
                    btnUnlinkAgreement.Visible = true;

                    ViewState["linkedAgreement"] = LinkedAgreement.Code.ToString();

                }
                else
                {
                    btnUnlinkAgreement.Visible = false;
                    lnkAgreementLink.InnerHtml = "<b><i class='glyphicon glyphicon-link'></i></b>إضافة إتفاقية مرتبطة ";
                    lnkAgreementLink.HRef = "/modules/Agreements/forms/AgreementsDataLink.aspx?linkedId=" + gets(objList.Code) + "&lawDocRefId=0";

                    ViewState["linkedAgreement"] = "0";
                }




            }
            else
            { lnkAgreementLink.Visible = false; }
            tblAdd.Visible = true;
            tblshow.Visible = false;
            tblSearch.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private int SaveLawDocMaster(bool fromScan = false)
        {
            string script = "";
            //Upload LocalFile


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
                    objLawDoc.effectiveDate = NullDateifEmpty(txtEffectiveDate.Text);
                    objLawDoc.ExpireDate = NullDateifEmpty(txtExpireDate.Text);
                    objLawDoc.DocCategoryID = ZeroIntergerIFNull(lstCategory.SelectedValue);

                    objLawDoc.UnderStudy = getBool(chkIsUnderStudy.Checked);
                    objLawDoc.isPublished = getBool(chkIspublished.Checked);
                    objLawDoc.PublishDate = NullDateifEmpty(txtPublishDate.Text);
                    objLawDoc.PublishVersion = txtVersionNum.Text;

                    objLawDoc.DocNotes = gets(txtNotes.Text);

                    objLawDoc.DocDetails = gets(txtDetails.Text);
                    objLawDoc.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    objLawDoc.kng_Dession = gets(lstkngDession.Text);
                    objRepository.AddLaw(objLawDoc);
                    hdnMasterID.Value = gets(objLawDoc.Code);
                    ViewState["itemID"] = gets(objLawDoc.Code);

                    // Add Linked Project
                    if (Request.QueryString["projectId"] != null)
                    {


                        if (Session["ProjectProcedireList"] != null)
                        {
                            var ImportedProcedures = (List<Law_DocProcedures>)Session["ProjectProcedireList"];
                            if (ImportedProcedures != null && ImportedProcedures.Count > 0)
                            {
                                Law_DocProcedures obj = new Law_DocProcedures();
                                foreach (var item in ImportedProcedures)
                                {

                                    obj = new Law_DocProcedures();

                                    obj.DocRefID = objLawDoc.Code;
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
                                        if (File.Exists(ScannerRepository + _TargetUploadPath + gets(Request.QueryString["projectId"]) + "/procedure/" + gets(item.Code) + "/" + item.Procedureattachments))
                                        {
                                            if (!CopyFile(gets(item.Procedureattachments), ScannerRepository + _TargetUploadPath + gets(Request.QueryString["projectId"]) + "/procedure/" + gets(item.Code) + "/", ScannerRepository + _TargetUploadPath + gets(objLawDoc.Code) + "/procedure/" + gets(obj.Code) + "/"))
                                            {
                                                fromScan = true;
                                                lblerror2.Text += "Copy faild ";
                                                lblerror2.Text += " <br/>Source : " + ScannerRepository + _TargetUploadPath + gets(Request.QueryString["projectId"]) + "/procedure/" + gets(item.Code) + "/";
                                                lblerror2.Text += " <br/>Dest : " + ScannerRepository + _TargetUploadPath + gets(objLawDoc.Code) + "/procedure/" + gets(obj.Code) + "/";
                                                lblerror2.Text += "--------------------";
                                            }
                                        }
                                        else
                                        {
                                            fromScan = true;
                                            lblerror2.Text += "File NOt Exist";
                                            lblerror2.Text += " <br/>Source : " + ScannerRepository + _TargetUploadPath + gets(Request.QueryString["projectId"]) + "/procedure/" + gets(item.Code) + "/" + item.Procedureattachments;
                                        }

                                    }



                                }
                            }

                        }

                        //Update Linked Project
                        var ProjectDetails = objRepository.GetDetails(ZeroIntergerIFNull(Request.QueryString["projectId"].ToString()));
                        if (ProjectDetails != null)
                        {
                            ProjectDetails.RefId = objLawDoc.Code;
                            ProjectDetails.isTransfered = true;
                            objRepository.UpdateLaw(ProjectDetails);
                        }
                        Session.Remove("ProjectProcedireList");
                    }
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
                    objLawDoc.effectiveDate = NullDateifEmpty(txtEffectiveDate.Text);
                    objLawDoc.ExpireDate = NullDateifEmpty(txtExpireDate.Text);
                    objLawDoc.DocCategoryID = ZeroIntergerIFNull(lstCategory.SelectedValue);

                    objLawDoc.UnderStudy = getBool(chkIsUnderStudy.Checked);
                    objLawDoc.isPublished = getBool(chkIspublished.Checked);
                    objLawDoc.PublishDate = NullDateifEmpty(txtPublishDate.Text);
                    objLawDoc.PublishVersion = txtVersionNum.Text;

                    objLawDoc.DocNotes = gets(txtNotes.Text);
                    objLawDoc.DocDetails = gets(txtDetails.Text);

                    objLawDoc.kng_Dession = gets(lstkngDession.Text);


                    objLawDoc.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());




                    objRepository.UpdateLaw(objLawDoc);
                    objRepository.UpdateLaw(objLawDoc);
                }


                string _img = UploadFileoServer(txtDocImage, ScannerRepository + _TargetUploadPath + gets(objLawDoc.Code) + "/");
                string _img2 = UploadFileoServer(txtDocImage2, ScannerRepository + _TargetUploadPath + gets(objLawDoc.Code) + "/");
                var objForEdit = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                if (_img != "" || _img2 != "")
                {

                    if (_img != "")
                    {
                        objForEdit.DocFilepath = _img;
                    }
                    if (_img2 != "")
                    {
                        objLawDoc.DocFilepath_published = _img2;

                    }
                    objRepository.UpdateLaw(objForEdit);
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


        private void UpdateDocLastProcedure()
        {
            try
            {
                // Update Agreement With Last procedure Based On Date

                var LastProcedurepbj = objRepository.GetLastProcedureobj(ZeroIntergerIFNull(hdnMasterID.Value));
                if (LastProcedurepbj != null)
                {
                    Law_DocData objDocData = new Law_DocData();
                    objDocData = objRepository.GetDetails(ZeroIntergerIFNull(hdnMasterID.Value));
                    objDocData.LastProcedureID = LastProcedurepbj.Code;
                    objRepository.UpdateLaw(objDocData);
                }
                else
                {

                    Law_DocData objDocData = new Law_DocData();
                    objDocData = objRepository.GetDetails(ZeroIntergerIFNull(hdnMasterID.Value));
                    objDocData.LastProcedureID = 0;
                    objRepository.UpdateLaw(objDocData);
                }

            }
            catch (Exception)
            {

                throw;
            }


        }
        private void fillLookups()
        {


            FillDllwithoptional_ALL(objLookup.FillLaw_DocType(), ref lstDocType, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillLaw_DocType(), ref lstFilterType, "NameAr", "Code", "الكل");


            FillDllwithoptional_ALL(objLookup.FillLaw_DocZeroCategory(), ref lstCategory, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillLaw_DocZeroCategory(), ref lstFilterCategory, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.Filllaw_DocProceduresTypes(), ref lstprocedureType, "NameAr", "Code", "إختر");
            FillDllwithoptional_ALL(objLookup.Filllaw_DocProceduresTypes(), ref lstfilterProceduretype, "NameAr", "Code", "الكل");


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

                                            if (Request.Form["emptyFile"] != null)
                                            {

                                                objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                                DeleteFileFromServer(objLawDoc.DocFilepath_published, ScannerRepository + _TargetUploadPath + gets(objLawDoc.Code) + "/");
                                                objLawDoc.DocFilepath_published = null;
                                                objRepository.UpdateLaw(objLawDoc);


                                            }
                                            else
                                            {
                                                objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                                DeleteFileFromServer(objLawDoc.DocFilepath_published, ScannerRepository + _TargetUploadPath + gets(objLawDoc.Code) + "/");
                                                objLawDoc.DocFilepath_published = FileList[i].Split(',')[0].ToString();
                                                objRepository.UpdateLaw(objLawDoc);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < FileList.Length; i++)
                                        {//Update Current Record With
                                            if (Request.Form["emptyFile"] != null)
                                            {
                                                objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));

                                                DeleteFileFromServer(objLawDoc.DocFilepath, ScannerRepository + _TargetUploadPath + gets(objLawDoc.Code) + "/");
                                                objLawDoc.DocFilepath = null;
                                                objRepository.UpdateLaw(objLawDoc);
                                            }
                                            else
                                            {
                                                objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                                DeleteFileFromServer(objLawDoc.DocFilepath, ScannerRepository + _TargetUploadPath + gets(objLawDoc.Code) + "/");
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
                                    string script = FormatpopupErrorMSG("Failed to save Scanned Files", "1");
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
                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objProcedure = objRepository.GetProceduresDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            DeleteFileFromServer(objProcedure.Procedureattachments, ScannerRepository + _TargetUploadPath + objProcedure.DocRefID + "/procedure/" + gets(objProcedure.Code) + "/");
                                            objProcedure.Procedureattachments = null;
                                            objRepository.UpdateProcedures(objProcedure);
                                        }
                                        else
                                        {
                                            objProcedure = objRepository.GetProceduresDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                            DeleteFileFromServer(objProcedure.Procedureattachments, ScannerRepository + _TargetUploadPath + objProcedure.DocRefID + "/procedure/" + gets(objProcedure.Code) + "/");
                                            objProcedure.Procedureattachments = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateProcedures(objProcedure);
                                        }
                                    }
                                    Response.Redirect("LawDocData.aspx?activetab=2&LawDocID=" + Request.Form["FileID"]);
                                    return;
                                }


                                else
                                {
                                    string script = FormatpopupErrorMSG("Failed to save Scanned Files", "1");
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
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + gets(_TargetID) + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnScannerfilepath.Value + "]' />";
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
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + gets(_TargetID) + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnpublishedScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnpublishedScannerfilepath.Value + "]' />";
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
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["LawDocitemID"].ToString() + "/procedure/" + gets(_TargetID) + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnprocedureScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnprocedureScannerfilepath.Value + "]' />";
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



                if (!gets(objList.Procedureattachments).Equals(""))
                {
                    btnprocedureScan.Text = "<i class='icon-images2'></i>&nbsp;تعديل الوثيقة";
                    hdnprocedureScannerfilepath.Value = gets(objList.Procedureattachments);
                }





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
            //decimal c = System.Math.Ceiling(Convert.ToDecimal(objList.Count / grdProcedureList.PageSize));
            //if ((c <= grdProcedureList.CurrentPageIndex))
            //{
            //    grdProcedureList.CurrentPageIndex = 0;
            //}

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
            txtProcedureLetterNum.Text = "";
            lstprocedureType.SelectedValue = "0";

            DivAddProcedure.Visible = false;
            divShowProcedure.Visible = true;



        }
        private int SaveProcedureInformation(int RefDocID)
        {
            string script = "";
            Law_DocProcedures obj = new Law_DocProcedures();
            try
            {



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

                    objRepository.AddProcedures(obj);
                    ViewState["ProcedureCode"] = obj.Code;

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



                    objRepository.UpdateProcedures(obj);

                }

                string _img = UploadFileoServer(txtProcedureimage, ScannerRepository + _TargetUploadPath + gets(RefDocID) + "/procedure/" + gets(obj.Code) + "/");
                if (_img != "")
                {
                    var objForEdit = objRepository.GetProceduresDetails(ZeroIntergerIFNull(ViewState["ProcedureCode"].ToString()));
                    objForEdit.Procedureattachments = _img;
                    objRepository.UpdateProcedures(objForEdit);
                }

                ViewState["ProcedureCode"] = obj.Code;

                ClearProcedureforms();
                UpdateDocLastProcedure();


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
            FillLawDocs();
        }

        protected void lstFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {



            if (lstFilterType.SelectedValue == "3")//مرسوم
            {
                lblFilterCatTitle.Text = "تصنيف المرسوم <span class='text-danger'>*</span>:";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(ZeroIntergerIFNull(lstFilterType.SelectedValue)), ref lstFilterCategory, "NameAr", "Code", "الكل");
            }
            else if (lstFilterType.SelectedValue == "4")//امر اميرس
            {
                lblFilterCatTitle.Text = "تصنيف الأمر الأميري <span class='text-danger'>*</span>:";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(ZeroIntergerIFNull(lstFilterType.SelectedValue)), ref lstFilterCategory, "NameAr", "Code", "الكل");
            }
            else
            {
                lblFilterCatTitle.Text = "التصنيف<span class='text-danger'>*</span>: ";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocZeroCategory(), ref lstFilterCategory, "NameAr", "Code", "الكل");
            }



        }

        protected void lstDocType_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (lstDocType.SelectedValue == "3")//مرسوم
            {
                lblcattitle.Text = "تصنيف المرسوم <span class='text-danger'>*</span>:";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(ZeroIntergerIFNull(lstDocType.SelectedValue)), ref lstCategory, "NameAr", "Code", "إختر");
            }
            else if (lstDocType.SelectedValue == "4")//امر اميرس
            {
                lblcattitle.Text = "تصنيف الأمر الأميري <span class='text-danger'>*</span>:";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(ZeroIntergerIFNull(lstDocType.SelectedValue)), ref lstCategory, "NameAr", "Code", "إختر");
            }
            else
            {
                lblcattitle.Text = "التصنيف <span class='text-danger'>*</span>:";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocZeroCategory(), ref lstCategory, "NameAr", "Code", "إختر");
            }



        }

        protected void btnUnlinkAgreement_Click(object sender, EventArgs e)
        {
            // Get Agreement Data
            if (ViewState["linkedAgreement"].ToString() != "0")
            {
                //Update Agreemrnt]
                AgreementData objagreement = new AgreementData();
                objagreement = agreemtnyRepository.GetDetails(ZeroIntergerIFNull(ViewState["linkedAgreement"].ToString()));

                if (objagreement != null)
                {
                    objagreement.Law_DocDataRefId = 0;
                    agreemtnyRepository.UpdateAgreement(objagreement);

                    string script = FormatpopupErrorMSG("تم فك الربط بنجاح", "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);


                    //Colse Popup
                    FillLawDocMasterInformation();

                    //litScript.Text = "parent.$.fn.colorbox.close();";
                }
            }
        }


        #region "Linked Docs"

        private void FillLinkedDocs(int DocRefID)
        {

            var objList = objRepository.GetRelatedDocs(DocRefID);

            lblLinkCount.Text = objList.Count.ToString();

            //if (objList.Count > 0)
            //{

            //    tblshow.Visible = true;
            //    tblSearch.Visible = false;
            //    pager1.Visible = true;

            //}
            //else
            //{
            //    tblshow.Visible = false;
            //    pager1.Visible = false;
            //    tblSearch.Visible = true;
            //    string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            //}

            //var duplicatedList = objList.SelectMany(t =>
            // Enumerable.Repeat(t, 2)).ToList();

            grdLinkedDocs.DataSource = objList;
            grdLinkedDocs.DataBind();

        }
        #endregion
        protected void lnkDeleteLawLink_Click(object sender, EventArgs e)
        {

        }

        protected void lnkAddLink_Click(object sender, EventArgs e)
        {

        }

        protected void grdLinkedDocs_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                var objToDelete = (Law_DocData_Linked)objRepository.GetLinkDetails(ZeroIntergerIFNull(e.Item.Cells[0].Text));
                if (objToDelete != null)
                {
                    objRepository.DeleteDocLink(objToDelete);

                }
                FillLinkedDocs(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

            }

        }

        protected void lnkCancelLawDoc_Click(object sender, EventArgs e)
        {
            if (!ViewState["itemID"].Equals("0"))
            {//Cancel Law Doc

                var objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                objLawDoc.LawCancelDate = DateTime.Now;
                objLawDoc.LawCancelled = true;
                objRepository.UpdateLaw(objLawDoc);

                string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);



            }
            else
            {
                string script = FormatpopupErrorMSG("عفوا ، خطأ في الحفظ ", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }


        }

        protected void lnkDeletePublished_Click(object sender, EventArgs e)
        {
            if (!ViewState["itemID"].Equals("0"))
            {//Delete 

                var objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                objLawDoc.DocFilepath_published = null;
                objRepository.UpdateLaw(objLawDoc);
                FillLawDocMasterInformation();

                string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);


            }
            else
            {
                string script = FormatpopupErrorMSG("عفوا ، خطأ في الحفظ ", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }


        }

        protected void lnkDeleteFile_Click(object sender, EventArgs e)
        {
            if (!ViewState["itemID"].Equals("0"))
            {//Delete 

                var objLawDoc = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                objLawDoc.DocFilepath = null;
                objRepository.UpdateLaw(objLawDoc);
                FillLawDocMasterInformation();

                string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);


            }
            else
            {
                string script = FormatpopupErrorMSG("عفوا ، خطأ في الحفظ ", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
        }



        protected void txtEffectiveDate_TextChanged(object sender, EventArgs e)
        {
            if (txtEffectiveDate.Text != "" && lstCategory.SelectedValue == "134") // مرسوم قيادين
            {
                txtExpireDate.Text = NullDateifEmptyToText(NullDateifEmpty(txtEffectiveDate.Text).AddYears(4));

            }

        }


        public string checkExpiration(DateTime _effectiveDate, DateTime _expireDate)
        {
            string _out = "";


            double Percentage = 0;
            string ExecutionColorHex = "#ccc";
            if (_expireDate != null && NullDateifEmptyToText(_expireDate) != "")
            {
                Percentage = (_expireDate.Date - DateTime.Now.Date).TotalDays <= 0 ? 100 :
                                Math.Floor((DateTime.Now.Date - _effectiveDate.Date).TotalDays / (_expireDate.Date - _effectiveDate.Date).TotalDays * 100);
                if (Percentage == 0)
                {
                    Percentage = 1;

                }

                if (Percentage <= 50)
                {
                    ExecutionColorHex = "#08a711"; //greeen
                    _out = "<div class='circle-bar position' style='margin-top:5px;' data-percent='" + Percentage + "' data-color='#ccc," + ExecutionColorHex + "'></div>";
                }
                else if (Percentage > 50 && Percentage <= 87.5)
                {
                    ExecutionColorHex = "#e3ad24";// yellow
                    _out = "<div class='circle-bar position' style='margin-top:5px;' data-percent='" + Percentage + "' data-color='#ccc," + ExecutionColorHex + "'></div>";
                }
                else if (Percentage > 87.5 && Percentage <= 99.9)
                {
                    ExecutionColorHex = "#e32442";// Red
                    _out = "<div class='circle-bar position' style='margin-top:5px;' data-percent='" + Percentage + "' data-color='#ccc," + ExecutionColorHex + "'></div>";
                }
                else
                {
                    _out = "<span class='label bg-danger-400'>انتهى </span>";
                }



            }
            else
            {
                _out = "";

            }






            return _out;

        }
    }
}