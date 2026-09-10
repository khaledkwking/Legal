using CutePager;
using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Enum;
using Infrastructure.DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using UI.Web.Admin.Controller;
using UI.Web.Helper;
using static iTextSharp.text.pdf.AcroFields;

namespace UI.Web.Modules.Laws
{
    public partial class LawDetailsDocData : BaseFormAdmin
    {
        private class LinkedStatusLookup
        {
            public int Code { get; set; }
            public string NameAr { get; set; }
        }
        private class Sectors
        {
            public int Code { get; set; }
            public string NameAr { get; set; }
            public string NameEn { get; set; }
        }
        private class Law_DocSectors
        {
            public int Code { get; set; }
            public int SectorId { get; set; }
            public int Law_DocId { get; set; }
        }
        private class LinkedStatusValue
        {
            public int? LinkedStatusID { get; set; }
        }

        #region "Page Members"

        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();
        public AgreementsRepository agreemtnyRepository = IoC.Resolve<AgreementsRepository>();
        public string _PageTitle = "نظام التشريعات  ";

        public bool isCancelled = false;


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "procedure/";

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


                if (Request.QueryString["ss"] != null && Request.QueryString["ss"].ToString() == "1")
                {

                    string script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
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


                if (Request.QueryString["ChildDocId"] != null)
                {
                    string listIds = Request.QueryString["ChildDocId"].ToString();
                    
                    FillLawDocs(listIds);
                    // Added Temporary Consignee
                    //string script = FormatpopupErrorMSG(Resources.Alerts.SorryFailToretriveData + " Query string Missing", "1");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    ViewState["itemID"] = "0";
                    //return;

                }
                else
                {
                    tblshow.Visible = false;



                    ViewState["itemID"] = Request.QueryString["ChildDocId"].ToString();


                }

                SetPageTitle();

                ViewState["OutboundItemID"] = "0";

                // FillInboundItems();


            }

        }

        protected void grdLawDocsList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {




            if ((e.Item.ItemType == ListItemType.Item))
            {
                //
                HtmlImage im = ((HtmlImage)(e.Item.Cells[2].FindControl("imgControl")));
                if (im != null)
                {
                    string imname = im.ClientID;
                    string rowindex = (e.Item.ItemIndex + 1).ToString();
                    string rowID = e.Item.ClientID;
                    im.Attributes.Add("onclick", ("ControlGrid(\'" + (imname + ("\'," + (rowindex + (",\'" + (rowID + "\')")))))));
                }



            }
            else if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string rowID = e.Item.ClientID;
                string Filecode = e.Item.Cells[3].Text;



                //var objUnitList = objRepository.FillLawProcedures(ZeroIntergerIFNull(Filecode));
                //if (objUnitList != null)
                //{
                //    DataGrid grd = ((DataGrid)(e.Item.Cells[1].FindControl("grdDocProcedures")));
                //    grd.DataSource = objUnitList;
                //    grd.DataBind();



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
                //if (e.Item.Cells[13].Text == "01/01/1990") e.Item.Cells[13].Text = "";
                //if (e.Item.Cells[15].Text == "01/01/1990") e.Item.Cells[15].Text = "";
                //if (e.Item.Cells[16].Text == "01/01/1990") e.Item.Cells[16].Text = "";

                HtmlAnchor file1 = ((HtmlAnchor)(e.Item.Cells[2].FindControl("file1")));
                HtmlAnchor file2 = ((HtmlAnchor)(e.Item.Cells[2].FindControl("file2")));
                if (e.Item.Cells[4].Text == "" || e.Item.Cells[4].Text == "&nbsp;")
                {
                    if (file1 != null)
                    {
                        file1.Visible = false;
                    }
                }
                if (e.Item.Cells[5].Text == "" || e.Item.Cells[5].Text == "&nbsp;")
                {
                    //file2.Visible = false;

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


        private void FillLawDocs(string listIds)
        {


            if (Request.QueryString["Type"].ToString () == "0")
            {


                var objList = objRepository.GetListBySerialNum(listIds);


                lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
              
                if (objList.Count > 0)
                {
                    //btnSave.Visible = true;
                    //lnkBack.Visible = true;

                    tblshow.Visible = true;
                    pager1.Visible = true;
                    tblProcedure.Visible = false;
                    pager2.Visible = false;
           


                }
                else
                {
                    tblshow.Visible = false;
                    pager1.Visible = false;
                    tblProcedure.Visible = false;
                    pager2.Visible = false;

                    string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                }

                var duplicatedList = objList.SelectMany(t =>
                 Enumerable.Repeat(t, 2)).ToList();


                grdLawDocsList.DataSource = duplicatedList;
                grdLawDocsList.DataBind();
                pager1.ItemCount = duplicatedList.Count;

            }
            else
            {
                var numbers = listIds
                 .Split(',')
                 .Where(x => !string.IsNullOrWhiteSpace(x))
                 .Select(x => int.Parse(x.Trim()))
                 .ToList();
                var objProcedureList = objRepository.GetAllProceduresDetails(numbers[0]);

                lblcount.Text = (Resources.Utilities.foundTotal + (objProcedureList.Count + Resources.Utilities.records));

                if (objProcedureList != null)
                {
                    //btnSave.Visible = true;
                    //lnkBack.Visible = true;

                    tblshow.Visible = false;
                    pager1.Visible = false;
                    tblProcedure.Visible = true;
                    pager2.Visible = true;
                  


                }
                else
                {
                    tblshow.Visible = false;
                    pager1.Visible = false;
                    tblProcedure.Visible = false;
                    pager2.Visible = false;

                    string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

                }
                grdLawProcedureList.DataSource = objProcedureList;
                grdLawProcedureList.DataBind();
                pager1.ItemCount = objProcedureList.Count;
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
            if (Request.QueryString["ChildDocId"] != null)
            {
                string listIds = Request.QueryString["ChildDocId"].ToString();
                FillLawDocs(listIds);
            }
        }


        #endregion

        #region "Helper Methods"




        private void FilLLawDocProcedures(int DocRefID)
        {

            var objList = objRepository.FillLawProcedures(DocRefID);
            //  lblComingCount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() + Resources.Utilities.records));
            //lblProcedureCount.Text = objList.Count.ToString();
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

                //divShowProcedure.Visible = true;
                //pager5.Visible = true;
                //grdProcedureList.Visible = true;

            }


            //var duplicatedList = objList.SelectMany(t =>
            //  Enumerable.Repeat(t, 2)).ToList();


        }



        #endregion


        


        protected void grdLawDocsList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            //if (e.CommandName == "delete")
            //{




            //    objRepository.DeleteLaw((Law_DocData)objRepository.GetDetails(ZeroIntergerIFNull(e.Item.Cells[3].Text)));
            //}
           // FillLawDocs();
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


                    //litScript.Text = "parent.$.fn.colorbox.close();";
                }
            }
        }


        #region "Linked Docs"

        private void FillLinkedDocs(int DocRefID)
        {

            var objList = objRepository.GetRelatedDocs(DocRefID);

            //lblLinkCount.Text = objList.Count.ToString();

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

        protected void grdLinkedDocs_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            var ddl = e.Item.FindControl("lstLinkedStatus") as DropDownList;
            if (ddl == null)
            {
                return;
            }

            var statuses = objLookup.DC.Database.SqlQuery<LinkedStatusLookup>(
                "SELECT Code, NameAr FROM Law_DocData_Linked_Status ORDER BY NameAr").ToList();

            ddl.DataSource = statuses;
            ddl.DataTextField = "NameAr";
            ddl.DataValueField = "Code";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("--- اختر ---", "0"));

            var linkedId = ZeroIntergerIFNull(e.Item.Cells[0].Text);
            ddl.Attributes["data-linked-id"] = linkedId.ToString();

            var statusValue = objRepository.DC.Database.SqlQuery<LinkedStatusValue>(
                "SELECT LinkedStatusID FROM Law_DocData_Linked WHERE Code = @code",
                new SqlParameter("@code", linkedId)).FirstOrDefault();

            var selectedId = statusValue != null && statusValue.LinkedStatusID.HasValue ? statusValue.LinkedStatusID.Value : 0;
            if (selectedId != 0)
            {
                var selectedValue = selectedId.ToString();
                if (ddl.Items.FindByValue(selectedValue) != null)
                {
                    ddl.SelectedValue = selectedValue;
                }
            }
        }

        protected void lstLinkedStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as DropDownList;
            if (ddl == null)
            {
                return;
            }

            var linkedId = ZeroIntergerIFNull(ddl.Attributes["data-linked-id"]);
            if (linkedId == 0)
            {
                return;
            }
            if (ddl.SelectedValue != "0")
            {
                var statusId = ZeroIntergerIFNull(ddl.SelectedValue);
                objRepository.DC.Database.ExecuteSqlCommand(
                    "UPDATE Law_DocData_Linked SET LinkedStatusID = @statusId WHERE Code = @code",
                    new SqlParameter("@statusId", statusId),
                    new SqlParameter("@code", linkedId));
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


        private object GetNullableInt(string value)
        {
            var parsed = ZeroIntergerIFNull(value);
            return parsed == 0 ? (object)DBNull.Value : parsed;
        }

    }

    public class Law_DocCategoryNew
    {
        public int Code { get; set; }

        public string NameEn { get; set; }

        public string NameAr { get; set; }

        public int? TypeID { get; set; }
    }
}