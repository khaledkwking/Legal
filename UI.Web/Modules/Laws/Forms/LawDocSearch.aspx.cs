using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using Infrastructure;
using Infrastructure.DAL;
using UI.Web.Admin.Controller;
using UI.Web.Helper;

namespace UI.Web.Modules.Laws.Forms
{
    public partial class LawDocSearch : BaseFormAdmin
    {
        private const int PageSize = 20;

        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();
        public string _PageTitle = "‰Ÿ«„ «· ‘—Ì⁄«   ";

        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LawsAttachments/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        private int CurrentPageIndex
        {
            get { return ViewState["LawDocSearchPageIndex"] != null ? (int)ViewState["LawDocSearchPageIndex"] : 0; }
            set { ViewState["LawDocSearchPageIndex"] = value; }
        }

        protected int CurrentPageNumber
        {
            get { return CurrentPageIndex + 1; }
        }

        protected int PageOffset
        {
            get { return CurrentPageIndex * PageSize; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillLookups();
            }
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            FillSearchResults();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPageIndex > 0)
            {
                CurrentPageIndex--;
            }

            BindPagedResults();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPageIndex++;
            BindPagedResults();
        }

        protected void rptPages_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                var pageIndex = ZeroIntergerIFNull(e.CommandArgument.ToString()) - 1;
                if (pageIndex >= 0)
                {
                    CurrentPageIndex = pageIndex;
                    BindPagedResults();
                }
            }
        }

        protected void lstFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFilterType.SelectedValue == "3")//„—”Ê„
            {
                lblFilterCatTitle.Text = " ’‰Ì› «·„—”Ê„ <span class='text-danger'>*</span>:";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(ZeroIntergerIFNull(lstFilterType.SelectedValue)), ref lstFilterCategory, "NameAr", "Code", "«·ﬂ·");

                lblFilterCatTitle.Visible = true;
                lstFilterCategory.Visible = true;
            }
            else if (lstFilterType.SelectedValue == "4")//«„— «„Ì—”
            {
                lblFilterCatTitle.Text = " ’‰Ì› «·√„— «·√„Ì—Ì <span class='text-danger'>*</span>:";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(ZeroIntergerIFNull(lstFilterType.SelectedValue)), ref lstFilterCategory, "NameAr", "Code", "«·ﬂ·");

                lblFilterCatTitle.Visible = false;
                lstFilterCategory.Visible = false;
            }
            else
            {
                lblFilterCatTitle.Text = "«· ’‰Ì›<span class='text-danger'>*</span>: ";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocZeroCategory(), ref lstFilterCategory, "NameAr", "Code", "«·ﬂ·");

                lblFilterCatTitle.Visible = true;
                lstFilterCategory.Visible = true;
            }
        }

        protected void dlLawSummary_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName != "Toggle")
            {
                return;
            }

            var destId = ZeroIntergerIFNull(e.CommandArgument.ToString());
            if (destId == 0)
            {
                return;
            }

            var expanded = GetExpandedDestIds();
            if (expanded.Contains(destId))
            {
                expanded.Remove(destId);
            }
            else
            {
                expanded.Add(destId);
            }

            SetExpandedDestIds(expanded);
            BindPagedResults();
        }

        protected void dlLawSummary_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            var dataItem = e.Item.DataItem as LawSummaryDTO;
            if (dataItem == null)
            {
                return;
            }

            var expanded = GetExpandedDestIds();
            var panel = e.Item.FindControl("pnlRelated") as Panel;
            var toggle = e.Item.FindControl("lnkToggle") as LinkButton;
            var relatedList = e.Item.FindControl("dlRelated") as DataList;
            if (panel == null || toggle == null || relatedList == null)
            {
                return;
            }

            if (expanded.Contains(dataItem.DestDocId))
            {
                toggle.Text = "-";
                panel.Visible = true;
                relatedList.DataSource = GetRelatedDocs(dataItem.DestDocId);
                relatedList.DataBind();
            }
            else
            {
                toggle.Text = "+";
                panel.Visible = false;
            }
        }

        private void fillLookups()
        {
            FillDllwithoptional_ALL(objLookup.FillLaw_DocType(), ref lstFilterType, "NameAr", "Code", "«·ﬂ·");
            FillDllwithoptional_ALL(objLookup.FillLaw_DocZeroCategory(), ref lstFilterCategory, "NameAr", "Code", "«·ﬂ·");
            FillDllwithoptional_ALL(objLookup.Filllaw_DocProceduresTypes(), ref lstfilterProceduretype, "NameAr", "Code", "«·ﬂ·");
        }

        private void FillSearchResults()
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DocNum", GetNullableInt(txtFilterSerialNum.Text)),
                new SqlParameter("@DocYear", GetNullableInt(txtFilterSerialYear.Text)),
                new SqlParameter("@DocDateFrom", GetNullableDate(txtFilterDatefrom.Text)),
                new SqlParameter("@DocDateTo", GetNullableDate(txtFilterDateTo.Text)),
                new SqlParameter("@DocTypeID", GetNullableInt(lstFilterType.SelectedValue)),
                new SqlParameter("@DocCategoryID", GetNullableInt(lstFilterCategory.SelectedValue)),
                new SqlParameter("@UnderStudy", GetNullableBool(lstFilterIsUnderStudy.SelectedValue)),
                new SqlParameter("@DocSubject", GetNullableString(txtFilterSubject.Text)),
                new SqlParameter("@DocNotes", GetNullableString(txtFilterNotes.Text)),
                new SqlParameter("@DocDetails", GetNullableString(txtFilterDetails.Text)),
                new SqlParameter("@isPublished", GetNullableBool(lstFilterPublish.SelectedValue)),
                new SqlParameter("@ProcedureTypeID", GetNullableInt(lstfilterProceduretype.SelectedValue)),
                new SqlParameter("@ExpireDateFrom", GetNullableDate(txtFilterExpireFrom.Text)),
                new SqlParameter("@ExpireDateTo", GetNullableDate(txtFilterExpireTo.Text))
            };

            var result = objRepository.DC.Database.SqlQuery<LawSummaryDTO>(
                "EXEC sp_GetLinkedLawsSummary_HTML @DocNum, @DocYear, @DocDateFrom, @DocDateTo, @DocTypeID, @DocCategoryID, @UnderStudy, @DocSubject, @DocNotes, @DocDetails, @isPublished, @ProcedureTypeID, @ExpireDateFrom, @ExpireDateTo",
                parameters.ToArray()).ToList();

            Session["LawDocSearchResults"] = result;
            ViewState["LawDocSearchExpanded"] = null;
            lblResultCount.Text = result.Count.ToString();

            if (result.Count > 0)
                divResult.Visible = true;
            else
                divResult.Visible = false;
            BindPagedResults();
        }

        private void BindPagedResults()
        {
            var result = Session["LawDocSearchResults"] as List<LawSummaryDTO> ?? new List<LawSummaryDTO>();
            var paged = new PagedDataSource
            {
                DataSource = result,
                AllowPaging = true,
                PageSize = PageSize,
                CurrentPageIndex = CurrentPageIndex
            };

            dlLawSummary.DataSource = paged;
            dlLawSummary.DataBind();

            btnPrev.Enabled = !paged.IsFirstPage;
            btnNext.Enabled = !paged.IsLastPage;

            var pages = Enumerable.Range(1, Math.Max(paged.PageCount, 1)).ToList();
            rptPages.DataSource = pages;
            rptPages.DataBind();
        }

        private object GetNullableInt(string value)
        {
            var parsed = ZeroIntergerIFNull(value);
            return parsed == 0 ? (object)DBNull.Value : parsed;
        }

        private object GetNullableBool(string value)
        {
            switch (ZeroIntergerIFNull(value))
            {
                case 1:
                    return true;
                case 2:
                    return false;
                default:
                    return DBNull.Value;
            }
        }

        private object GetNullableDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return DBNull.Value;
            }

            return NullDateifEmpty(value);
        }

        private object GetNullableString(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value;
        }

        private List<LawSummaryDTO> GetRelatedDocs(int destDocId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DestDocID", destDocId)
            };

            return objRepository.DC.Database.SqlQuery<LawSummaryDTO>(
                "EXEC sp_GetLinkedLawsSummary_HTML_Details @DestDocID",
                parameters.ToArray()).ToList();
        }

        private HashSet<int> GetExpandedDestIds()
        {
            var list = ViewState["LawDocSearchExpanded"] as List<int> ?? new List<int>();
            return new HashSet<int>(list);
        }

        private void SetExpandedDestIds(HashSet<int> expanded)
        {
            ViewState["LawDocSearchExpanded"] = expanded.ToList();
        }
    }
}
