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
using Newtonsoft.Json;
using UI.Web.Admin.Controller;

namespace UI.Web.LibraryDocs.Forms
{
    public partial class LibraryDocsView2 : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LibraryDocsRepository objRepository = IoC.Resolve<LibraryDocsRepository>();
        public string _PageTitle = "المكتبة الإلكترونية ";
        public string AgreementCode = "0";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LibraryDocs/";
        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();



        #endregion

        #region "Page Events"

        protected void Page_PreRender(object sender, EventArgs e)
        {


        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "LibraryDocs.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";
            btnCancel.Attributes.Add("onclick", "Page_ValidationActive=false;");
            btnSave.Attributes.Add("onclick", "return chkImage();");


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



                ViewState["itemID"] = "0";

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
                    FillLibraryDocsInfo();

                }

                SetPageTitle();
                ViewState["OutboundItemID"] = "0";
                fillGallery();
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

            ////Hide Defult Dates

            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    if (e.Item.Cells[13].Text == "01/01/1990") e.Item.Cells[13].Text = "";
            //    if (e.Item.Cells[15].Text == "01/01/1990") e.Item.Cells[15].Text = "";
            //    if (e.Item.Cells[16].Text == "01/01/1990") e.Item.Cells[16].Text = "";



            //}
        }


        public string showattachment(string hasattachment)
        {
            if (hasattachment != "")
            {
                return "";
            }
            return "display:none";

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


        protected void grdData_DeleteCommand(object source, DataGridCommandEventArgs e)
        {

        }

        //protected void grdData_EditCommand(object source, DataGridCommandEventArgs e)
        //{
        //    ViewState["ProceduresitemID"] = e.Item.Cells[0].Text;
        //    divProAttache.Visible = false;
        //    FillProcedureFrom();

        //}

        protected void grdData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }




        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            FillLibraryDocs();
        }

        protected void lnkSearchback_Click(object sender, EventArgs e)
        {
            tblSearch.Visible = true;
            tblshow.Visible = false;
        }


        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            // this.ClearForm();

            tblSearch.Visible = true;
            tblAdd.Visible = false;
            tblshow.Visible = false;

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillLibraryDocs();
        }
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            SaveLibraryDocs();
        }




        //}
        #endregion

        #region "Fill Information"
        private string MapSearchKeys()
        {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                _keyList.Add("مسلسل", txtFilterSerial.Text);
                _keyList.Add(" التصنيف ", lstfilterDocCategory.SelectedItem.Text);
                _keyList.Add(" النوع    ", lstfilterDocType.SelectedItem.Text);
                _keyList.Add("تاريخ الرفع من", txtFilterDatefrom.Text);
                _keyList.Add("التاريخ الي", txtFilterDateTo.Text);
                _keyList.Add("    كلمة فى الموضوع  ", txtPartofName.Text);
                _keyList.Add(" الفصل التشريعي", lstFilterChapter.SelectedItem.Text);
                _keyList.Add(" دور الانعقاد  ", lstFilterSession.SelectedItem.Text);

            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }

        private void FillLibraryDocs()
        {


            //   Session["ViewPrivate"]
            var objList = objRepository.GetList(txtFilterSerial.Text,txtPartofName.Text,
                NullDateifEmpty(txtFilterDatefrom.Text), NullDateifEmpty(txtFilterDateTo.Text),
                ZeroIntergerIFNull(lstfilterDocType.SelectedValue),
                getBool(ReadSession("ViewPrivate")), ZeroIntergerIFNull(lstfilterDocCategory.SelectedValue),
                ZeroIntergerIFNull(lstFilterChapter.SelectedValue),
                ZeroIntergerIFNull(lstFilterSession.SelectedValue), MapSearchKeys());


            lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));

            lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));


            // var duplicatedList = objList.SelectMany(t =>
            //Enumerable.Repeat(t, 2)).ToList();

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



            grdInboundItems.DataSource = objList;
            grdInboundItems.DataBind();

            pager1.ItemCount = objList.Count;

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
            txtSerial.Text = "";
            txtSubject.Text = "";
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


            FillLibraryDocs();


        }


        #endregion

        #region "Helper Methods"


        private void FillLibraryDocsInfo()
        {

            var objList = objRepository.FillDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));
            if ((objList != null))
            {
                hdnMasterID.Value = gets(objList.Code);

                txtSerial.Text = gets(objList.DocRef);

                lstDocCategory.SelectedValue = gets(objList.CatId);
                FillDllwithoptional_ALL(objLookup.FillDocTypes(ZeroIntergerIFNull(lstDocCategory.SelectedValue)), ref lstTypeCode, "NameAr", "Code", "اختر");
                lstTypeCode.SelectedValue = gets(objList.DocType);
                txtSubject.Text = gets(objList.DocSubject);
                chkIsPrivate.Checked = getBool(objList.isPrivate);


                if (objList.CatId == 5)
                {
                    divChapterSessions.Visible = true;
                    lstChapter.SelectedValue = gets(objList.ChapterID);
                    FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstChapter.SelectedValue)), ref lstSession, "NameAr", "Code", "اختر");
                    lstSession.SelectedValue = gets(objList.SessionID);

                }
                else
                { divChapterSessions.Visible = false; }
            }

            tblAdd.Visible = true;
           //blSubTitle.Text = this.GetTitle(false);

        }
        private int SaveLibraryDocs(bool fromScan = false)
        {
            Library_Documents obj = new Library_Documents();
           
            string script = "";
            try
            {

                if (ViewState["itemID"].Equals("0"))
                {//Save

                    obj.UploadDate = DateTime.Now;
                    obj.ReceiveDate = DateTime.Now;

                    obj.DocRef = txtSerial.Text;

                    obj.DocType = ZeroIntergerIFNull(lstTypeCode.SelectedValue);
                    obj.DocSubject = txtSubject.Text;
                    obj.Doc_Meta = txtSubject.Text;

                    obj.isPrivate = getBool(chkIsPrivate.Checked);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    obj.ChapterID = ZeroIntergerIFNull(lstChapter.SelectedValue);
                    obj.SessionID = ZeroIntergerIFNull(lstSession.SelectedValue);

                    objRepository.AddLibraryDocs(obj);
                    hdnMasterID.Value = gets(obj.Code);
                    ViewState["itemID"] = gets(obj.Code);



                }
                else
                { //Update

                    hdnMasterID.Value = ViewState["itemID"].ToString();
                    obj = objRepository.GetDetails(ZeroIntergerIFNull(ViewState["itemID"].ToString()));

                    obj.LastUpdate = DateTime.Now;
                    obj.DocSubject = txtSubject.Text;
                    obj.Doc_Meta = txtSubject.Text;
                    obj.DocRef = txtSerial.Text;
                    obj.DocType = ZeroIntergerIFNull(lstTypeCode.SelectedValue);
                   
                    obj.isPrivate = getBool(chkIsPrivate.Checked);
                    obj.aUser = ZeroIntergerIFNull(ReadSession("userid").ToString());

                    obj.ChapterID = ZeroIntergerIFNull(lstChapter.SelectedValue);
                    obj.SessionID = ZeroIntergerIFNull(lstSession.SelectedValue);

                    objRepository.UpdateLibraryDocs(obj);

                }
                //Upload LocalFile
                if (txtImage.FileName != "")
                {

                    string _img = UploadFileoServer(txtImage, ScannerRepository + _TargetUploadPath + gets(obj.Code) + "/");
                    if (_img != "")
                    {
                        var objForEdit = objRepository.GetDetails(obj.Code);
                        objForEdit.Filepath = _img;
                        objRepository.UpdateLibraryDocs(objForEdit);
                    }
                }

                ViewState["itemID"] = obj.Code;

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
                Response.Redirect("LibraryDocs.aspx?ss=1&id=" + gets(obj.Code));
                return 0;
            }
            return obj.Code;
        }
        private void fillLookups()
        {


            FillDllwithoptional_ALL(objLookup.Fill_LibDocCategory(), ref lstfilterDocCategory, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.Fill_LibDocCategory(), ref lstDocCategory, "NameAr", "Code", "اختر");


            FillDllwithoptional_ALL(objLookup.FillDocTypes(ZeroIntergerIFNull(lstfilterDocCategory.SelectedValue)), ref lstfilterDocType, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillDocTypes(ZeroIntergerIFNull(lstDocCategory.SelectedValue)), ref lstTypeCode, "NameAr", "Code", "اختر");


            FillDll(objLookup.FillParliament_legislativeChapter(), ref lstChapter, "NameAr", "Code");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeChapter(), ref lstFilterChapter, "NameAr", "Code","الكل");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstChapter.SelectedValue)), ref lstSession, "NameAr", "Code", "اختر");
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterSession, "NameAr", "Code", "الكل");

        }

         private void fillGallery()
        {

            var categories = objLookup.Fill_LibDocCategory();
            rptCategory.DataSource = categories;
            rptCategory.DataBind();

            if (Request.QueryString["catid"] != null)
            {
                rptCategory.Visible = false;
                rptTypes.Visible = true;
                var Subcategories = objLookup.FillDocTypes(ZeroIntergerIFNull(Request.QueryString["catid"]));
                rptTypes.DataSource = Subcategories;
                rptTypes.DataBind();
            }
            else { rptCategory.Visible = true;
                rptTypes.Visible = false;
            }

            if (Request.QueryString["catid"] != null && Request.QueryString["typeid"] != null)
            {
                rptCategory.Visible = false;
                rptTypes.Visible = false;
                rptDocs.Visible = true;

              var docs=  objRepository.GetList(txtFilterSerial.Text, txtPartofName.Text,
                   NullDateifEmpty(txtFilterDatefrom.Text), NullDateifEmpty(txtFilterDateTo.Text),
                   ZeroIntergerIFNull(Request.QueryString["typeid"]),
                   getBool(ReadSession("ViewPrivate")), ZeroIntergerIFNull(Request.QueryString["catid"]),
                   ZeroIntergerIFNull(lstFilterChapter.SelectedValue),
                   ZeroIntergerIFNull(lstFilterSession.SelectedValue), MapSearchKeys());

                rptDocs.DataSource = docs;
                rptDocs.DataBind();

            }





            }
        protected void lstChapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstChapter.SelectedValue)), ref lstSession, "NameAr", "Code", "اختر");

        }

        #endregion

        #region "Scanning"

        private void UpdateScannedFile()
        {
            Library_Documents obj = new Library_Documents();
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

                                obj = objRepository.GetDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                obj.Filepath = null;
                                objRepository.UpdateLibraryDocs(obj);
                            }
                            else
                            {
                                obj = objRepository.GetDetails(ZeroIntergerIFNull(Request.Form["TargetID"].ToString()));
                                obj.Filepath = FileList[i].Split(',')[0].ToString();
                                objRepository.UpdateLibraryDocs(obj);

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

        protected void grdInboundItems_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                objRepository.DeleteLibraryDocs((Library_Documents)objRepository.GetDetails(ZeroIntergerIFNull(e.Item.Cells[0].Text)));
            }
            FillLibraryDocs();
        }

        protected void btnOutgoingScan_Click(object sender, EventArgs e)
        {
            //Show Loadin div

            string _URI = HttpContext.Current.Request.Url.AbsoluteUri;
            string _CallBackUrl = _URI.Substring(0, _URI.IndexOf("?") + 1);// + "/modules/Agreements/forms/AgreementAttachments.aspx";
                                                                           //string _CallBackUrl = HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.RawUrl + "/modules/Agreements/forms/AgreementAttachments.aspx";
            if (_URI.IndexOf("?") == -1)
            {
                _CallBackUrl = _URI;

            }
            int _TargetID = SaveLibraryDocs(true);

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

        protected void lstDocCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            FillDllwithoptional_ALL(objLookup.FillDocTypes(ZeroIntergerIFNull(lstDocCategory.SelectedValue)), ref lstTypeCode, "NameAr", "Code", "اختر");

            if (lstDocCategory.SelectedValue == "5")
            {
                divChapterSessions.Visible = true;
            }
            else { divChapterSessions.Visible = false; }
        }

        protected void lstfilterDocCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDllwithoptional_ALL(objLookup.FillDocTypes(ZeroIntergerIFNull(lstfilterDocCategory.SelectedValue)), ref lstfilterDocType, "NameAr", "Code", "الكل");

 if (lstfilterDocCategory.SelectedValue == "5")
            {
                divFilterSession.Visible = true;
            }
            else { divFilterSession.Visible = false; }

        }

        protected void lstFilterChapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDllwithoptional_ALL(objLookup.FillParliament_legislativeSession(ZeroIntergerIFNull(lstFilterChapter.SelectedValue)), ref lstFilterSession, "NameAr", "Code", "الكل");

        }
    }
}