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

namespace UI.Web.Modules.Laws
{
    public partial class DessionData : BaseFormAdmin
    {
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();
        public string _PageTitle = "نظام القرارات  ";
        public int RowNo = 0;

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

        }
        protected void Page_Load(object sender, System.EventArgs e)
        {




            if (!IsPostBack)
            {
                if (Request.QueryString["T"] != null && (Request.QueryString["T"]=="5")|| Request.QueryString["T"] == "6")
                {
                    ViewState["Type"] = Request.QueryString["T"].ToString();

                 

                    FillLawDocs();

                }
                else
                    Response.Redirect("/ar-KW/admin/pages/home.aspx");

                    if (Request.QueryString["ss"] != null && Request.QueryString["ss"].ToString() == "1")
                {

                    string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                }



                if (Request.QueryString["docTypeId"] != null)
                {
                    FillLawDocs();

                }

                if (Request.QueryString["ispublish"] != null)
                {
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
                    tblshow.Visible = true;


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
                string rowindex = (e.Item.ItemIndex + 1).ToString();
                string rowID = e.Item.ClientID;
                //LinkButton lnk = ((LinkButton)(e.Item.Cells[0].Controls[0]));
                //lnk.Attributes.Add("onclick", "return confirm(\'Are you sure you want to delete this Invoice?\');");




            }
            else if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string rowID = e.Item.ClientID;
                string Filecode = e.Item.Cells[3].Text;

             

                /***********************************************/

                DataGrid grdIncoming = ((DataGrid)(e.Item.Cells[1].FindControl("grdIncoming")));
                DataGrid grdOutgoing = ((DataGrid)(e.Item.Cells[1].FindControl("grdOutgoing")));


                var _arcIncoming = objRepository.FillArcDocs(1, ZeroIntergerIFNull(Filecode));
                var _arcOutgoing = objRepository.FillArcDocs(2, ZeroIntergerIFNull(Filecode));

                if (_arcIncoming != null)
                {
                    grdIncoming.DataSource = _arcIncoming;
                    grdIncoming.DataBind();
                }

                if (grdOutgoing != null)
                {
                    grdOutgoing.DataSource = _arcOutgoing;
                    grdOutgoing.DataBind();
                }

                /**************************************************/


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

        public string getPrivate(Boolean isPrivate)
        {
            string _out = "";
            if (isPrivate == true)
            {
                _out = "<span class='label bg-warning-400' style='white-space: normal;'>ملف سري </span>";
            }


            return _out;
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


        private void FillLawDocs()
        {
            if (Request.QueryString["T"] == "5")
                _PageTitle = "قرارات مجلس الوزراء ";
            else if (Request.QueryString["T"] == "6")
                _PageTitle = "قرارات سمو رئيس مجلس الوزراء ";
            //List<int> selectedRequestedFrom= new List<int>();
            //if (hdnfilterRequestedFrom.Value != "" && hdnfilterRequestedFrom.Value != "0")
            //{
            //    string[] selected = hdnfilterRequestedFrom.Value.Split(',');
            //    for (int i = 0; i < selected.Length; i++)
            //    {
            //        if (ZeroIntergerIFNull(selected[i]) != 0)
            //        {
            //            selectedRequestedFrom.Add(ZeroIntergerIFNull(selected[i]));
            //        }

            //    }
            //}


            //Get Selectd Persons
            int TypeId = ZeroIntergerIFNull(ViewState["Type"].ToString());
            if(TypeId==0)
                Response.Redirect("/ar-KW/admin/pages/home.aspx");

            var objList = objRepository.GetDecisionsList(ZeroIntergerIFNull(""),
                ZeroIntergerIFNull(""), NullDateifEmpty(""),       
                NullDateifEmpty(""), TypeId,
                ZeroIntergerIFNull("0"),
               ZeroIntergerIFNull("0"),
               "", "",
               ZeroIntergerIFNull("0"), "", "",
               ZeroIntergerIFNull("0"), false, ZeroIntergerIFNull("2"));


            lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
            lblSearchResultCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));


            //var duplicatedList = objList.SelectMany(t =>
            // Enumerable.Repeat(t, 2)).ToList();


            if (objList.Count > 0)
            {
                //btnSave.Visible = true;
                //lnkBack.Visible = true;

                tblshow.Visible = true;
                pager1.Visible = true;

            }
            else
            {

                tblshow.Visible = true;
                pager1.Visible = true;
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
                ViewState["LawDocitemID"] = gets(objList.Code);


            }
            //blSubTitle.Text = this.GetTitle(false);

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

                                    Response.Redirect("LawDessionData.aspx?activetab=1&LawDocID=" + Request.Form["TargetID"].ToString());
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
                                    Response.Redirect("LawDessionData.aspx?activetab=2&LawDocID=" + Request.Form["FileID"]);
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


                                ViewState["DocumentArcID"] = Request.Form["TargetID"];
                                ViewState["itemID"] = Request.Form["FileID"];
                                //  FillQuestionMasterInformation();

                                string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                string[] FileList = _ScannerFileLlisy.Split(';');

                                if (Request.Form["TargetID"] != null)
                                {
                                    for (int i = 0; i < FileList.Length; i++)
                                    {//Update Current Record With

                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
                                            objArc.Filepath = null;
                                            objRepository.UpdateArcData(objArc);
                                        }
                                        else
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
                                            objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateArcData(objArc);
                                        }
                                    }
                                    Response.Redirect("LawDessionData.aspx?activetab=3&LawDocID=" + Request.Form["FileID"]);
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

                                ViewState["DocumentArcID"] = Request.Form["TargetID"];
                                ViewState["itemID"] = Request.Form["FileID"];
                                //  FillQuestionMasterInformation();

                                string _ScannerFileLlisy = Request.Form["fileList"].ToString();
                                _ScannerFileLlisy = _ScannerFileLlisy.Substring(1, _ScannerFileLlisy.Length - 3);
                                string[] FileList = _ScannerFileLlisy.Split(';');

                                if (Request.Form["TargetID"] != null)
                                {
                                    for (int i = 0; i < FileList.Length; i++)
                                    {//Update Current Record With

                                        if (Request.Form["emptyFile"] != null)
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
                                            objArc.Filepath = null;
                                            objRepository.UpdateArcData(objArc);
                                        }
                                        else
                                        {
                                            objArc = objRepository.GetArcDocDetails(ZeroIntergerIFNull(ViewState["DocumentArcID"].ToString()));
                                            objArc.Filepath = FileList[i].Split(',')[0].ToString();
                                            objRepository.UpdateArcData(objArc);
                                        }
                                    }
                                    Response.Redirect("LawDessionData.aspx?activetab=4&LawDocID=" + Request.Form["FileID"]);
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
        }



        #endregion

        //#endregion


        #region "Procedure Methods"

        #region "Procedure HElpers"
      
     

        #endregion
        protected void grdProcedureList_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["ProcedureCode"] = e.Item.Cells[0].Text;
   

        }
        protected void lnkAddProcedure_Click(object sender, EventArgs e)
        {
            ViewState["ProcedureCode"] = "0";
        }


        protected void pager_Command5(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }
        }

        protected void grdProcedureList_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }

       void grdincoming_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["DocumentArcID"] = e.Item.Cells[0].Text;
          

        }

        #endregion

        protected void Lnkincoming_Click(object sender, EventArgs e)
        {
            ViewState["outgoingCode"] = "0";
            ViewState["DocumentArcID"] = "0";

        }

        protected void pager_Command2(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

        }
        protected void grdincoming_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }


  
        protected void grdOutgoing_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }

    
        protected void pager_Command3(object sender, CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

        }

        protected void grdOutgoing_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["DocumentArcID"] = e.Item.Cells[0].Text;
        }

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
    }
}