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
    public partial class CommitteeDetails : BaseFormAdmin
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

            if (!IsPostBack)
            {

                 FillCommittees();

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
                    tblshow.Visible = true;


                    ViewState["itemID"] = Request.QueryString["CommitteeID"].ToString();
                 





                }

                SetPageTitle();

                ViewState["OutboundItemID"] = "0";


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
            int TypeID = 0;
            if (Request.QueryString["TypeID"] != null)
            {
                 TypeID = int.Parse(Request.QueryString["TypeID"].ToString());
                //var objCommitteeList = objLookup.FillCommitteeList().Where(c=>c.c);
            }
            int typeId = int.Parse(RadioButtonTypesList.SelectedValue.ToString ());
            int FinishedId = int.Parse(RadioButtonFinishedList.SelectedValue.ToString());
            List<viewCommitteeData> objList ;
            string CurDate = "";
            if (FinishedId ==2)
            {
                CurDate = DateTime.Today.AddDays(-1).ToShortDateString();
            }
         
            if(FinishedId == 0 || FinishedId == 2)
            {
                 objList = objRepository.GetList(0, 0, NullDateifEmpty(""), NullDateifEmpty(""), "", "", "", NullDateifEmpty(""), NullDateifEmpty(CurDate), 0, 0, 0, typeId, MapSearchKeys()).ToList();
            }
            else
            {
                objList = objRepository.GetList(0, 0, NullDateifEmpty(""), NullDateifEmpty(""), "", "", "", NullDateifEmpty(""), NullDateifEmpty(CurDate), 0, 0, 0, typeId, MapSearchKeys()).Where(obj=>
                obj.JoinExpireDate >= DateTime.Today).ToList();


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

            }
            else
            {
                tblshow.Visible = false;

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
        protected void grdProcedureList_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ViewState["ProcedureCode"] = e.Item.Cells[0].Text;
         

        }
        protected void lnkAddProcedure_Click(object sender, EventArgs e)
        {
            ViewState["ProcedureCode"] = "0";
      
        }


      
        protected void grdProcedureList_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }


        protected void grdCommitteesList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            FillCommittees();
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

   

        protected void RadioButtonFinishedList_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCommittees();

        }

        protected void RadioButtonTypesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCommittees();

        }
    }
}