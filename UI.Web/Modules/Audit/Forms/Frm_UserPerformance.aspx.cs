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
using Infrastructure.DAL.ViewModels;
using System.Data;
using Utilities;

namespace UI.Web.Modules.Audit.Forms
{
    public partial class Frm_UserPerformance : BaseFormAdmin
    {
        #region "Page Members"

        public AuditRepository objRepository = IoC.Resolve<AuditRepository>();

        #endregion

        #region "Page Events"

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";




            if (!IsPostBack)
            {

                fillLookups();
                ViewState["OutboundItemID"] = "0";

            }

        }

        protected void grdresult_ItemDataBound(object sender, DataGridItemEventArgs e)
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


        }
        protected void lnkBack_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            { Response.Redirect("OutboundOperrations.aspx?id=" + Request.QueryString["id"].ToString()); }
            else
            { Response.Redirect("OutboundOperrations.aspx"); }

        }
        protected void grdData_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }


        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            FillAudit();
        }

        protected void lnkSearchback_Click(object sender, EventArgs e)
        {
            tblSearch.Visible = true;
            tblshow.Visible = false;
        }


        #endregion

        #region "Fill Information"
        private void FillAudit()
        {

            List<searchkeys> _SuboutList = new List<searchkeys>();
            var objList = objRepository.GetuserStatisticList(ZeroIntergerIFNull(lstFilerUser.SelectedValue), NullDateifEmpty(txtFilterDatefrom.Text),
                NullDateifEmpty(txtFilterDateTo.Text)).Where(c=>c.Total >0).ToList();

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
                //pager1.Visible = true;

                //foreach (var item in objList)
                //{

                //    if (item.tages != null && item.tages != "{}" && item.tages != "")
                //    {

                //        var Result = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.tages);

                //        foreach (KeyValuePair<string, string> Keyitem in Result)
                //        {
                //            // Console.WriteLine(string.Format("Key: {0} Value: {1}", item.Key, item.Value));
                //            _SuboutList.Add(new searchkeys() { refRecordId = item.Code, key = Keyitem.Key, value = Keyitem.Value });
                //        }

                //    }

                    //}
                DataTable TempDT = ConvertHelper.ToDataTable(objList);
                //Bind Report And SubReport
                string selectedText = lstFilerUser.SelectedItem.Text;


                TempDT.TableName = "UserMonthlyStatistics";
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/modules/Audit/RDLC/rpt_UserAuditPerformance.rdlc");
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", " نموذج الانجازات الشهرية بالنظام    "));

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("reportSubTitle",  "  عن الفترة من   " + txtFilterDatefrom.Text + "  الي " + txtFilterDateTo.Text + " " ));

                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("UserName", selectedText));

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(TempDT.TableName, TempDT));

            }
            else
            {
                tblshow.Visible = false;
                pager1.Visible = false;
                tblSearch.Visible = true;
                string script = FormatpopupErrorMSG("لا يوجد نتيجة للبحث ", "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

            }

           // var duplicatedList = objList.SelectMany(t =>
           //Enumerable.Repeat(t, 2)).ToList();


            //grdresult.DataSource = objList;
            //grdresult.DataBind();


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
        protected void pager_Command(object sender, CommandEventArgs e)
        {
            //Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            //if ((currnetPageIndx <= 0))
            //{
            //    currnetPageIndx = 1;
            //}

            //if ((currnetPageIndx > grdresult.PageCount))
            //{
            //    currnetPageIndx = (grdresult.PageCount - 1);
            //}

            //pager1.CurrentIndex = currnetPageIndx;
            //grdresult.CurrentPageIndex = (currnetPageIndx - 1);
            //FillAudit();
        }


        #endregion

         #region "Helper Methods"
         private void fillLookups()
        {

             FillDllwithoptional_ALL(objRepository.fillSystemUsystemUsers(), ref lstFilerUser , "Name", "id", "الكل");
            //FillDllwithoptional_ALL(objRepository.fillSysmtesMOdule(),null, "NameAr", "Code", "الكل");



        }



        #endregion





    }
}