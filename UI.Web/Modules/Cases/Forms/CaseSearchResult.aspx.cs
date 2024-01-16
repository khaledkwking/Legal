using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using UI.Web.Admin.Controller;

namespace UI.Web.cases.Forms
{
    public partial class CaseSearchResult : BaseFormAdmin
    {
        #region "Page Members"
        public CasesRepository objRepository = IoC.Resolve<CasesRepository>();
        public string _PageTitle = " قائمة القضايا ";


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "CasesAttachments/";
        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();


        #endregion

        #region "Page Events"
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "CasesData.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
 
            if (!IsPostBack)
            {
               
                if (Request.QueryString["DegreeID"] != null)
                {
                    FillGrid();
                }
                  
            }

        }
        protected void pager_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdCases.PageCount))
            {
                currnetPageIndx = (grdCases.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdCases.CurrentPageIndex = (currnetPageIndx - 1);
            this.FillGrid();
        }
         
       

        protected void grdData_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
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
        
        
        #endregion

        #region "Fill Information"
        
      
        private void FillGrid()
        { 
 
            var objList = objRepository.getCasesResult(ZeroIntergerIFNull(Request.QueryString["DegreeID"]));
            lblcount.Text = (Resources.Utilities.foundTotal + (objList.Count.ToString() +  Resources.Utilities.records));
           

            grdCases.DataSource = objList;
            grdCases.DataBind();
            int _totalCount = objList.Count;
            pager1.ItemCount = _totalCount;

        }

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


        #endregion





    }
}