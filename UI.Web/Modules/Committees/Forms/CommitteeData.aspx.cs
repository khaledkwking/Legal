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

namespace UI.Web.Modules.Committees.Forms
{
    public partial class CommitteeData : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public CommitteeRepository objRepository = IoC.Resolve<CommitteeRepository>();
        public string _PageTitle = "نظام المجالس واللجان العليا ومجالس إدارات الجهات الحكومية  ";

        public bool isCancelled = false;


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "CommitteeAttachments/";

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

               // FillCommittees();



                ViewState["SpChapterChanged"] = "0";
                ViewState["SpEdit"] = "0";
                ViewState["NewDesc"] = "";
                ViewState["NewBar"] = "";
                ViewState["NewIsbn"] = "";
                ViewState["SPITEM"] = "";
                ViewState["NewPrice"] = "0";
                Session["ItemList"] = null;
                ViewState["itemID"] = "0";
                ViewState["CommitteeitemID"] = "0";
                ViewState["ProceduresitemID"] = "0";
                ViewState["IncommingCode"] = "0";
                ViewState["outgoingCode"] = "0";
                ViewState["CommitteeArcID"] = "0";
                ViewState["AttachitemID"] = "0";
                ViewState["ProcedureCode"] = "0";

                ViewState["linkedAgreement"] = "0";


                if (Request.QueryString["CommitteeID"] == null)
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


                    ViewState["itemID"] = Request.QueryString["CommitteeID"].ToString();
                    FillCommitteeMasterInformation();









                }

                SetPageTitle();

                ViewState["OutboundItemID"] = "0";

                // FillInboundItems();
                UpdateScannedFile();

            }

        }

        protected void grdCommitteesList_ItemDataBound(object sender, DataGridItemEventArgs e)
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

                //LinkButton lnkDelete = ((LinkButton)(e.Item.FindControl("lnkDelete")));
                //if (lnkDelete != null)
                //{
                //    string deleteMessage = GetGlobalResourceObject("Alerts", "DeleteAlert").ToString();
                //    lnkDelete.Attributes.Add("onclick", "return confirm('" + deleteMessage + "');");
                //}



            }
            else if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string rowID = e.Item.ClientID;
                string Filecode = e.Item.Cells[3].Text;

                //SqlDataReader dr = SellMaster.ins.getInvoiceItemsReader(code);

                var objUnitList = objRepository.FillCommitteesProcedures(ZeroIntergerIFNull(Filecode));
                if (objUnitList != null)
                {
                    DataGrid grd = ((DataGrid)(e.Item.Cells[1].FindControl("grdDocProcedures")));
                    grd.DataSource = objUnitList;
                    grd.DataBind();

 

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

                //if (getBool(gets(e.Item.Cells[6].Text)))
                //{//Law Cancelled
                //    e.Item.BackColor = System.Drawing.Color.FromArgb(242, 178, 171);
                //}


                //e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                //if (getBool(gets(e.Item.Cells[6].Text)))
                //{ e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#f2b2ab\';"); }
                //else { e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';"); }



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
            ViewState["CommitteeitemID"] = "0";
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
            FillCommittees();
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
            ViewState["CommitteeitemID"] = "0";
            ViewState["ProceduresitemID"] = "0";
            Session["PersonsList"] = null;
            // Response.Redirect("/Modules/laws/Forms/CommitteeData.aspx");
            FillCommittees();

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillCommittees();
        }
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            SaveCommitteeMaster();
        }


        protected void lnkAddNewCommittee_Click(object sender, EventArgs e)
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
                //_keyList.Add(" رقم الوثيقة ", txtFilterSerialNum.Text);
                //_keyList.Add("   سنة الاصدار   ", txtFilterSerialYear.Text);
                //_keyList.Add(" تاريخ  إصدار الوثيقة من ", txtFilterDatefrom.Text);
                //_keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                //_keyList.Add(" جزء من نص الوثيقة    ", txtFilterDetails.Text);
                //_keyList.Add(" العمل التحضيري", lstfilterProceduretype.SelectedItem.Text);
                //_keyList.Add(" نوع الوثيقة  ", lstFilterType.SelectedItem.Text);
                //_keyList.Add(" قيد الدراسة    ", lstFilterIsUnderStudy.SelectedItem.Text);
                //_keyList.Add(lblFilterCatTitle.Text, lstFilterCategory.SelectedItem.Text);
                //_keyList.Add("  جزء من الموضوع ", txtFilterSubject.Text);
                //_keyList.Add("نشر بالجريدة الرسمية", lstFilterPublish.Text);

                //_keyList.Add(" تاريخ  انتهاء الوثيقة من ", txtFilterExpireFrom.Text);
                //_keyList.Add(" الي   تاريخ  انتهاء  ", txtFilterExpireTo.Text);

            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }
        private void FillCommittees()
        {
            int typeId = int.Parse(RadioButtonTypesList.SelectedValue.ToString());
            int FinishedId = int.Parse(RadioButtonFinishedList.SelectedValue.ToString());
            List<viewCommitteeData> objList;
            string CurDate = "";
            if (FinishedId == 2)
            {
                CurDate = DateTime.Today.AddDays(-1).ToShortDateString();
            }

            if (FinishedId == 0 || FinishedId == 2)
            {
                objList = objRepository.GetList(ZeroIntergerIFNull(txtFilterSerialNum.Text), 0,
                 NullDateifEmpty(txtFilterDatefrom.Text), NullDateifEmpty(txtFilterDateTo.Text), txtFilterSubject.Text, txtFilterDetails.Text, "",
                NullDateifEmpty(""), NullDateifEmpty(CurDate), ZeroIntergerIFNull(lstfilterminister.SelectedValue),
                ZeroIntergerIFNull(lstfilterProcedureType.SelectedValue), ZeroIntergerIFNull(lstFilterCommittee.SelectedValue), typeId, MapSearchKeys()).ToList();
                //objList = objRepository.GetList(0, 0, NullDateifEmpty(""), NullDateifEmpty(""), "", "", "", NullDateifEmpty(""), NullDateifEmpty(CurDate), 0, 0, 0, typeId, MapSearchKeys()).ToList();
            }
            else
            {
                //objList = objRepository.GetList(0, 0, NullDateifEmpty(""), NullDateifEmpty(""), "", "", "", NullDateifEmpty(""), NullDateifEmpty(CurDate), 0, 0, 0, typeId, MapSearchKeys()).Where(obj =>
                //obj.JoinExpireDate >= DateTime.Today).ToList();
                objList = objRepository.GetList(ZeroIntergerIFNull(txtFilterSerialNum.Text), 0,
                     NullDateifEmpty(txtFilterDatefrom.Text), NullDateifEmpty(txtFilterDateTo.Text), txtFilterSubject.Text, txtFilterDetails.Text, "",
                   NullDateifEmpty(""), NullDateifEmpty(CurDate), ZeroIntergerIFNull(lstfilterminister.SelectedValue),
                    ZeroIntergerIFNull(lstfilterProcedureType.SelectedValue), ZeroIntergerIFNull(lstFilterCommittee.SelectedValue), typeId, MapSearchKeys()).ToList();

                objList= objList.Where(obj =>obj.JoinExpireDate >= DateTime.Today).ToList();

            }

            
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


            grdCommitteesList.DataSource = duplicatedList;
            grdCommitteesList.DataBind();
            pager1.ItemCount = duplicatedList.Count;

            
            grdCommitteesList.DataBind();
        }

        private void ClearCaseForm()
        {
            ViewState["CommitteeitemID"] = "0";
            txtDocSerialNum.Text = "";
            
            txtSubject.Text = "";
            txtNotes.Text = "";

            txtDocDate.Text = "";
            txtExpireDate.Text = "";


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
          
            btnSave.Visible = userAccess.Edit || userAccess.Add;

            if (Request.QueryString["editflag"] != null)
            {
                string editflag = Request.QueryString["editflag"].ToString();

                btnSave.Visible = userAccess.Edit;
               
            }
            else
            {

                btnSave.Visible = userAccess.Edit || userAccess.Add;
               
            }



            grdCommitteesList.Columns[12].Visible = userAccess.Delete;
            lnkDeleteProcedure.Visible = userAccess.Delete;

        }


        protected void pager_Command(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdCommitteesList.PageCount))
            {
                currnetPageIndx = (grdCommitteesList.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdCommitteesList.CurrentPageIndex = (currnetPageIndx - 1);
            FillCommittees();
        }


        #endregion

        #region "Helper Methods"


        private void FillCommitteeMasterInformation()
        {

            var objList = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {


                //if (Request.QueryString["CommitteeRefID"] != null && !gets(objList.DocFilepath_published).Equals(""))
                //{//Redirect to 
                //    Response.Redirect(ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + ViewState["itemID"].ToString() + "/" + "&vfileList=[" + gets(objList.DocFilepath_published) + ";]");
                //    return;
                //}


                hdnMasterID.Value = gets(objList.Code);
                ViewState["CommitteeitemID"] = gets(objList.Code);

                txtDocSerialNum.Text = gets(objList.committeeNum);
                
                txtNotes.Text = gets(objList.Notes);
                txtSubject.Text = gets(objList.LegalDocsDesc);
                lstCommittee.SelectedValue = gets(objList.committeeRefCode);
                

                txtDocDate.Text = NullDateifEmptyToText(objList.lastJoinDate).ToString();
                txtExpireDate.Text = NullDateifEmptyToText(objList.JoinExpireDate).ToString();

               lstMinister.SelectedValue = gets(objList.MinisterRefId);



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








                //FillFileAttachment(objList.Code);
                FilLCommitteeProcedures(objList.Code);

                


            }
            else
            {   }
            tblAdd.Visible = true;
            tblshow.Visible = false;
            tblSearch.Visible = false;
            //blSubTitle.Text = this.GetTitle(false);

        }
        private int SaveCommitteeMaster(bool fromScan = false)
        {
            string script = "";
            //Upload LocalFile


            Committees_Data objCommittee = new Committees_Data();


            try
            {
                if (ViewState["itemID"].Equals("0"))
                {//Save


                    if (objRepository.CheckDocExitance(ZeroIntergerIFNull(txtDocSerialNum.Text),   0))
                    {

                        script = FormatpopupErrorMSG(" [ رقم الوثيقة   مسجل من قبل  ]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return 0;
                    }


                    objCommittee.CreationDate = DateTime.Now;
                    objCommittee.LastModificationDate = DateTime.Now;
                    // objCommittee.DocTypeID = ZeroIntergerIFNull(lstDocType.SelectedValue);

                    objCommittee.committeeSerial =  gets(txtDocSerialNum.Text);
                    objCommittee.committeeNum = ZeroIntergerIFNull(txtDocSerialNum.Text);

                    objCommittee.committeeRefCode = ZeroIntergerIFNull( lstCommittee.SelectedValue);
                    objCommittee.committeeTitle = lstCommittee.SelectedItem.Text;

                    Committees_RefList objCommittees_RefList = new Committees_RefList(); // الجهات
                    if (objCommittee.committeeRefCode !=null)
                    {
                        objCommittees_RefList= objLookup.FillCommitteeList().Where(c => c.Code == objCommittee.committeeRefCode).FirstOrDefault();
                        objCommittee.committeeTypeID = objCommittees_RefList.TypeID;
                    }
                    

                    objCommittee.LegalDocsDesc = txtSubject.Text;
                    objCommittee.lastJoinDate = NullDateifEmpty(txtDocDate.Text);
                    objCommittee.JoinExpireDate = NullDateifEmpty(txtExpireDate.Text);
                    objCommittee.MinisterRefId = ZeroIntergerIFNull(lstMinister.SelectedValue);



                    objCommittee.Notes = gets(txtNotes.Text);

                
                    objCommittee.CreatedBy = ZeroIntergerIFNull(ReadSession("userid").ToString());
                    objCommittee.CreatedAt = DateTime.Now;

                    objRepository.AddCommittees(objCommittee);
                    hdnMasterID.Value = gets(objCommittee.Code);
                    ViewState["itemID"] = gets(objCommittee.Code);

                    // Add Linked Project

                }
                else
                { //Update


                    if (objRepository.CheckDocExitance(ZeroIntergerIFNull(txtDocSerialNum.Text),    ZeroIntergerIFNull(ViewState["itemID"].ToString())))
                    {
                        script = FormatpopupErrorMSG(" [ رقم الوثيقة   مسجل من قبل  ]", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                        return 0;
                    }


                    hdnMasterID.Value = ViewState["itemID"].ToString();
                    objCommittee = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    objCommittee.LastModificationDate = DateTime.Now;

                    objCommittee.committeeSerial =   gets(txtDocSerialNum.Text);
                    objCommittee.committeeNum = ZeroIntergerIFNull(txtDocSerialNum.Text);

                    objCommittee.committeeRefCode = ZeroIntergerIFNull(lstCommittee.SelectedValue);
                    objCommittee.committeeTitle = lstCommittee.SelectedItem.Text;

                    objCommittee.LegalDocsDesc = txtSubject.Text;
                    objCommittee.lastJoinDate = NullDateifEmpty(txtDocDate.Text);

                    objCommittee.JoinExpireDate = NullDateifEmpty(txtExpireDate.Text);
                    objCommittee.MinisterRefId = ZeroIntergerIFNull(lstMinister.SelectedValue);

                    Committees_RefList objCommittees_RefList = new Committees_RefList(); // الجهات
                    if (objCommittee.committeeRefCode != null)
                    {
                        objCommittees_RefList = objLookup.FillCommitteeList().Where(c => c.Code == objCommittee.committeeRefCode).FirstOrDefault();
                        objCommittee.committeeTypeID = objCommittees_RefList.TypeID;
                    }

                    objCommittee.Notes = gets(txtNotes.Text);




                    objCommittee.CreatedBy = ZeroIntergerIFNull(ReadSession("userid").ToString());
                    objCommittee.CreatedAt = DateTime.Now;



                    objRepository.UpdateCommittees(objCommittee);
                }


                string _img = UploadFileoServer(txtDocImage, ScannerRepository + _TargetUploadPath + gets(objCommittee.Code) + "/");
                //  string _img2 = UploadFileoServer(txtDocImage2, ScannerRepository + _TargetUploadPath + gets(objCommittee.Code) + "/");
                var objForEdit = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
                if (_img != "")
                {

                    if (_img != "")
                    {
                        objForEdit.DocFilepath = _img;
                    }

                    objRepository.UpdateCommittees(objForEdit);
                }

                ViewState["itemID"] = objCommittee.Code;

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
                Response.Redirect("CommitteeData.aspx?ss=1&CommitteeID=" + gets(objCommittee.Code));
                return 0;
            }
            return objCommittee.Code;
        }


        private void UpdateDocLastProcedure()
        {
            try
            {
                // Update Agreement With Last procedure Based On Date

                var LastProcedurepbj = objRepository.GetLastProcedure(ZeroIntergerIFNull(hdnMasterID.Value));
                if (LastProcedurepbj != null)
                {
                    Committees_Data objDocData = new Committees_Data();
                    objDocData = objRepository.GetDetails(ZeroIntergerIFNull(hdnMasterID.Value));
                    objDocData.LastProcedureID = LastProcedurepbj.Code;
                    objRepository.UpdateCommittees(objDocData);
                }
                else
                {

                    Committees_Data objDocData = new Committees_Data();
                    objDocData = objRepository.GetDetails(ZeroIntergerIFNull(hdnMasterID.Value));
                    objDocData.LastProcedureID = 0;
                    objRepository.UpdateCommittees(objDocData);
                }

            }
            catch (Exception)
            {

                throw;
            }


        }
        private void fillLookups()
        {


            FillDllwithoptional_ALL(objLookup.fillMinisters(), ref lstMinister, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.fillMinisters(), ref lstfilterminister, "NameAr", "Code", "الكل");



            FillDllwithoptional_ALL(objLookup.fillCommittees_ProceduresTypes(), ref lstprocedureType, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.fillCommittees_ProceduresTypes(), ref lstfilterProcedureType, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillCommitteeList(), ref lstCommittee, "NameAr", "Code", "إختر");
           FillDllwithoptional_ALL(objLookup.FillCommitteeList(), ref lstFilterCommittee, "NameAr", "Code", "الكل");


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
            Committees_Data objCommittee = new Committees_Data();
            Committees_DocProcedures objProcedure = new Committees_DocProcedures();

            arc_Data objArc = new arc_Data();
            if (Request.Form["fileList"] != null)
            {
                if (Request.Form["fileList"].ToString() != "")
                {

                    switch (Request.Form["ActiveTab"].ToString())
                    {
                        case "1":
                            {//Committee Information


                                ViewState["itemID"] = Request.Form["TargetID"];
                                //  FillCommitteeMasterInformation();
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

                                                objCommittee = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                                DeleteFileFromServer(objCommittee.DocFilepath, ScannerRepository + _TargetUploadPath + gets(objCommittee.Code) + "/");
                                                objCommittee.DocFilepath = null;
                                                objRepository.UpdateCommittees(objCommittee);


                                            }
                                            else
                                            {
                                                objCommittee = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                                DeleteFileFromServer(objCommittee.DocFilepath, ScannerRepository + _TargetUploadPath + gets(objCommittee.Code) + "/");
                                                objCommittee.DocFilepath = FileList[i].Split(',')[0].ToString();
                                                objRepository.UpdateCommittees(objCommittee);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < FileList.Length; i++)
                                        {//Update Current Record With
                                            if (Request.Form["emptyFile"] != null)
                                            {
                                                objCommittee = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));

                                                DeleteFileFromServer(objCommittee.DocFilepath, ScannerRepository + _TargetUploadPath + gets(objCommittee.Code) + "/");
                                                objCommittee.DocFilepath = null;
                                                objRepository.UpdateCommittees(objCommittee);
                                            }
                                            else
                                            {
                                                objCommittee = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                                DeleteFileFromServer(objCommittee.DocFilepath, ScannerRepository + _TargetUploadPath + gets(objCommittee.Code) + "/");
                                                objCommittee.DocFilepath = FileList[i].Split(',')[0].ToString();
                                                objRepository.UpdateCommittees(objCommittee);
                                            }
                                        }

                                    }

                                    Response.Redirect("CommitteeData.aspx?activetab=1&CommitteeID=" + Request.Form["TargetID"]);
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
                                //  FillCommitteeMasterInformation();
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
                                    Response.Redirect("CommitteeData.aspx?activetab=2&CommitteeID=" + Request.Form["FileID"]);
                                    return;
                                }


                                else
                                {
                                    string script = FormatpopupErrorMSG("Failed to save Scanned Files", "1");
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                                }

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
            int _TargetID = SaveCommitteeMaster(true);



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
            int _TargetID = SaveCommitteeMaster(true);



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
            if (ViewState["CommitteeitemID"].ToString() != "0" && ViewState["CommitteeitemID"].ToString() != "")
            {
                _TargetID = SaveProcedureInformation(ZeroIntergerIFNull(ViewState["CommitteeitemID"].ToString()));
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Committee Information First", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }





            if (_TargetID != 0)
            {



                string ScannerPostFrom = "<form id='ScannerCalllerForm' action='" + _TargetUrl + "' method='POST'>";
                ScannerPostFrom += "<input type='hidden' id='Targetpath' name='Targetpath' value='" + _TargetUploadPath + ViewState["CommitteeitemID"].ToString() + "/procedure/" + gets(_TargetID) + "/" + "' />";
                ScannerPostFrom += "<input type='hidden' id='CallbackURL' name='CallbackURL' value='" + _CallBackUrl + "' />";
                ScannerPostFrom += "<input type='hidden' id='calleruser' name='calleruser' value='" + Session["userid"] + "' />";
                ScannerPostFrom += "<input type='hidden' id='action' name='action' value='" + (hdnprocedureScannerfilepath.Value == "" ? "1" : "3") + "' />";
                ScannerPostFrom += "<input type='hidden' id='vfilelist' name='vfilelist' value='[" + hdnprocedureScannerfilepath.Value + "]' />";
                ScannerPostFrom += "<input type='hidden' id='TargetID' name='TargetID' value='" + _TargetID + "' />";
                ScannerPostFrom += "<input type='hidden' id='FileID' name='FileID' value='" + ViewState["CommitteeitemID"].ToString() + "' />";
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
        private void FilLCommitteeProcedures(int DocRefID)
        {

            var objList = objRepository.FillCommitteesProcedures(DocRefID);
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
            Committees_DocProcedures obj = new Committees_DocProcedures();
            try
            {



                if (ViewState["ProcedureCode"].Equals("0"))
                {//Save
                    obj.CreationDate = DateTime.Now;
                    obj.LastActionDate = DateTime.Now;

                    obj.DocRefID = RefDocID;
                    obj.ProcedureNotes = txtProcedureNotes.Text;
                    obj.ProcedureDate = NullDateifEmpty(txtProcedureDate.Text);
                  
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
            FilLCommitteeProcedures(RefDocID);
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
            if (ViewState["CommitteeitemID"].ToString() != "0" && ViewState["CommitteeitemID"].ToString() != "")
            {
                SaveProcedureInformation(ZeroIntergerIFNull(ViewState["CommitteeitemID"].ToString()));
                string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
            else
            {

                string script = FormatpopupErrorMSG("Please save Committee Information First", "2");
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
            FilLCommitteeProcedures(ZeroIntergerIFNull(ViewState["CommitteeitemID"].ToString()));
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
                            objRepository.DeleteProcedures((Committees_DocProcedures)objRepository.GetProceduresDetails(ZeroIntergerIFNull(grdProcedureList.Items[i].Cells[0].Text)));
                        }
                    }
                }
                FilLCommitteeProcedures(ZeroIntergerIFNull(ViewState["CommitteeitemID"].ToString()));

            }
            catch (Exception ex)
            {


                string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteDataFailed + ex.Message.ToString(), "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
            }
        }

        #endregion



        protected void grdCommitteesList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                //Delete Related Data

                var objList = objRepository.FillCommitteesProcedures(ZeroIntergerIFNull(e.Item.Cells[3].Text));
                foreach (var item in objList)
                {
                    objRepository.DeleteProcedures(item);
                }
                objRepository.DeleteCommittees((Committees_Data)objRepository.GetDetails(ZeroIntergerIFNull(e.Item.Cells[3].Text)));
            }
            FillCommittees();
        }

      


        #region "Linked Docs"


        #endregion
        protected void lnkDeleteLawLink_Click(object sender, EventArgs e)
        {

        }

        protected void lnkAddLink_Click(object sender, EventArgs e)
        {

        }




        protected void lnkDeleteFile_Click(object sender, EventArgs e)
        {
            if (!ViewState["itemID"].Equals("0"))
            {//Delete 

                var objCommittee = objRepository.GetDetailsForEdit(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                objCommittee.DocFilepath = null;
                objRepository.UpdateCommittees(objCommittee);
                FillCommitteeMasterInformation();

                string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);


            }
            else
            {
                string script = FormatpopupErrorMSG("عفوا ، خطأ في الحفظ ", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }
        }



        protected void txtDocDate_TextChanged(object sender, EventArgs e)
        {
            if (txtDocDate.Text != "") 
            {
                txtExpireDate.Text = NullDateifEmptyToText(NullDateifEmpty(txtDocDate.Text).AddYears(4));

            }

        }


        public string checkExpiration(DateTime _JoinJoinExpireDate, DateTime _JoinExpireDate)
        {
            string _out = "";


            double Percentage = 0;
            string ExecutionColorHex = "#ccc";
            if (_JoinExpireDate != null && NullDateifEmptyToText(_JoinExpireDate) != "")
            {
                Percentage = (_JoinExpireDate.Date - DateTime.Now.Date).TotalDays <= 0 ? 100 :
                                Math.Floor((DateTime.Now.Date - _JoinJoinExpireDate.Date).TotalDays / (_JoinExpireDate.Date - _JoinJoinExpireDate.Date).TotalDays * 100);
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

        protected void RadioButtonTypesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCommittees();
        }

        protected void RadioButtonFinishedList_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCommittees();
        }
    }
}