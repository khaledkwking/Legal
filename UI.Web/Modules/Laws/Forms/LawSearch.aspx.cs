using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using Infrastructure;
using Infrastructure.DAL;
using UI.Web.Admin.Controller;

namespace UI.Web.Modules.Laws.Forms
{
    public partial class LawSearch : BaseFormAdmin
    {
        private const int PageSize = 20;
        private int _groupIndex;
        private Dictionary<string, int> _rootChildIndex = new Dictionary<string, int>();
        public string _PageTitle = "  التشريعات وارتباطها  ";

        private class LawHierarchyResult
        {
            public int? Level { get; set; }
            public int? ParentDocId { get; set; }
            public int? ChildDocId { get; set; }
            public string Path { get; set; }
            public int? DocNum { get; set; }
            public int? DocYear { get; set; }
            public string DocSubject { get; set; }
            public string DocFilepath { get; set; }
            public string DocTypeName { get; set; }
            public string DocDescriptionHTML { get; set; }
            public string DocProceduresTypesNameAr { get; set; }
            
        }
        private class LawExtraDoc
        {
            public int Code { get; set; }
            public int? DocNum { get; set; }
            public int? DocYear { get; set; }
            public string DocSubject { get; set; }
            public string DocFilepath { get; set; }
            public string DocTypeName { get; set; }
        }
        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LawsAttachments/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();
        public string HighlightSearchText(string text, string searchText)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(searchText))
            {
                return text;
            }

            try
            {
                // Escape special regex characters in the search text
                string pattern = System.Text.RegularExpressions.Regex.Escape(searchText.Trim());

                // Create replacement with highlight span
                string replacement = $"<span class='highlight-search'>{System.Web.HttpUtility.HtmlEncode(searchText.Trim())}</span>";

                // Replace with case-insensitive matching
                string highlightedText = System.Text.RegularExpressions.Regex.Replace(
                    text,
                    pattern,
                    replacement,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase
                );

                return highlightedText;
            }
            catch
            {
                // If regex fails, return original text
                return text;
            }
        }
        public string getsDocDetails(object code)
        {
            if (code == null || code == DBNull.Value)
                return string.Empty;

            var docData = objRepository.GetDetails((int)GetNullableInt(code.ToString()));

            if (docData == null)
                return string.Empty;

            return docData.DocDetails ?? string.Empty;
        }
        private int CurrentPageIndex
        {
            get { return ViewState["LawSearchPageIndex"] != null ? (int)ViewState["LawSearchPageIndex"] : 0; }
            set { ViewState["LawSearchPageIndex"] = value; }
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
            if (lstFilterType.SelectedValue == "3")//مرسوم
            {
                lblFilterCatTitle.Text = "تصنيف المرسوم <span class='text-danger'>*</span>:";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(ZeroIntergerIFNull(lstFilterType.SelectedValue)), ref lstFilterCategory, "NameAr", "Code", "الكل");

                lblFilterCatTitle.Visible = true;
                lstFilterCategory.Visible = true;
            }
            else if (lstFilterType.SelectedValue == "4")//امر اميرس
            {
                lblFilterCatTitle.Text = "تصنيف الأمر الأميري <span class='text-danger'>*</span>:";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(ZeroIntergerIFNull(lstFilterType.SelectedValue)), ref lstFilterCategory, "NameAr", "Code", "الكل");

                lblFilterCatTitle.Visible = false;
                lstFilterCategory.Visible = false;
            }
            else
            {
                lblFilterCatTitle.Text = "التصنيف<span class='text-danger'>*</span>: ";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocZeroCategory(), ref lstFilterCategory, "NameAr", "Code", "الكل");

                lblFilterCatTitle.Visible = true;
                lstFilterCategory.Visible = true;
            }
        }

        private void fillLookups()
        {
            FillDllwithoptional_ALL(objLookup.FillLaw_DocType(), ref lstFilterType, "NameAr", "Code", "الكل");
            FillDllwithoptional_ALL(objLookup.FillLaw_DocZeroCategory(), ref lstFilterCategory, "NameAr", "Code", "الكل");
        }
        string GetRoot(LawHierarchyResult x)
        {
            if (string.IsNullOrWhiteSpace(x.Path))
                return x.ChildDocId.ToString();

            return x.Path.Split('>')[0].Trim();
        }
        private void FillSearchResults()
        {
            Func<List<LawHierarchyResult>, List<LawHierarchyResult>> dedupeResults = items =>
            {
                var seen = new HashSet<string>();
                var filtered = new List<LawHierarchyResult>(items.Count);

                foreach (var item in items)
                {
                    if (!item.ParentDocId.HasValue)
                    {
                        filtered.Add(item);
                        continue;
                    }

                    var key = string.Join("|", GetRoot(item), item.DocNum, item.DocYear, item.DocTypeName);
                    if (seen.Add(key))
                    {
                        filtered.Add(item);
                    }
                }

                return filtered;
            };
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DocId", GetNullableInt(null)),
                new SqlParameter("@DocNum", GetNullableInt(txtFilterSerialNum.Text)),
                new SqlParameter("@DocYear", GetNullableInt(txtFilterSerialYear.Text)),
                new SqlParameter("@DocTypeID", GetNullableInt(lstFilterType.SelectedValue)),
                new SqlParameter("@DocCategoryID", GetNullableInt(lstFilterCategory.SelectedValue)),
                new SqlParameter("@UnderStudy", GetNullableBool(lstFilterIsUnderStudy.SelectedValue)),
                new SqlParameter("@isPublished", GetNullableBool(lstFilterPublish.SelectedValue)),
                new SqlParameter("@DocDateFrom", GetNullableDate(txtFilterDatefrom.Text)),
                new SqlParameter("@DocDateTo", GetNullableDate(txtFilterDateTo.Text)),
                new SqlParameter("@DocSubject", GetNullableString(txtFilterSubject.Text)),
                new SqlParameter("@DocNotes", GetNullableString(txtFilterNotes.Text)),
                new SqlParameter("@DocDetails", GetNullableString(txtFilterDetails.Text)),
                new SqlParameter("@ExpireDateFrom", GetNullableDate(txtFilterExpireFrom.Text)),
                new SqlParameter("@ExpireDateTo", GetNullableDate(txtFilterExpireTo.Text)),
                new SqlParameter("@Flag", 1)
            };

            var result = objRepository.DC.Database.SqlQuery<LawHierarchyResult>(
                "EXEC sp_SearchLawHierarchyV9 @DocId, @DocNum, @DocYear, @DocTypeID, @DocCategoryID, @UnderStudy, @isPublished, @DocDateFrom, @DocDateTo, @DocSubject, @DocNotes, @DocDetails, @ExpireDateFrom, @ExpireDateTo,@Flag",
                parameters.ToArray()).ToList();

            // get all unlinked 
            var extraDocs = LoadAllUnlinkedDocs();
            // get list of chlids
            var existingIds = new HashSet<int>(result.Where(item => item.ChildDocId.HasValue)
                .Select(item => item.ChildDocId.Value));

            foreach (var extra in extraDocs)
            {
                if (existingIds.Contains(extra.Code))
                {
                    continue;
                }
                // get if parent has no chlid
                result.Add(new LawHierarchyResult
                {
                    Level = 0,
                    ParentDocId = null,
                    ChildDocId = extra.Code,
                    DocNum = extra.DocNum,
                    DocYear = extra.DocYear,
                    DocSubject = extra.DocSubject,
                    DocFilepath = extra.DocFilepath,
                    DocTypeName = extra.DocTypeName,
                    DocDescriptionHTML = string.Empty,
                    DocProceduresTypesNameAr = string.Empty
                });
            }

            result = dedupeResults(result);
            //result = result
            //       .OrderBy(x => GetRoot(x))  // يجمع كل شجرة مع بعض
            //       .ThenBy(x =>
            //           x.ParentDocId == null
            //               ? 0
            //               : 1)                // Roots أولاً داخل كل group
            //       .ThenByDescending(x => x.DocYear)
            //       .ThenByDescending(x => x.DocNum)// ترتيب بالسنة فقط داخل كل الشجرة
            //       .ThenBy(x => x.Level)             // يحافظ على الهيكل
            //       .ThenBy(x => x.Path)              // stability فقط
            //       .ToList();
            // map للـ root مع السنة
            var rootYearMap = result
                .Where(x => x.ParentDocId == null)
                 .ToDictionary(
                        x => x.ChildDocId.ToString(),
                        x => new { x.DocYear, x.DocNum }
                    );

            // الترتيب
            result = result
                .Select(x => new
                {
                    Item = x,
                    Root = GetRoot(x)
                })
               .OrderByDescending(x =>
                    rootYearMap.ContainsKey(x.Root)
                        ? rootYearMap[x.Root].DocYear
                        : 0) // السنة أولاً

                .ThenByDescending(x =>
                    rootYearMap.ContainsKey(x.Root)
                        ? rootYearMap[x.Root].DocNum
                        : 0) // ثم DocNum للـ parent نفسه
                .ThenBy(x => x.Root) // grouping
                .ThenBy(x => x.Item.ParentDocId == null ? 0 : 1) // root الأول
                .ThenByDescending(x => x.Item.DocYear) // children بالأحدث
        
                .ThenByDescending(x => x.Item.DocNum)

                .Select(x => x.Item)

                .ToList();

            result = result
           .GroupBy(x => new { x.ParentDocId, x.ChildDocId })
           .Select(g => g.First())
           .ToList();

            Session["LawSearchResults"] = result;
            lblResultCount.Text = result.Count.ToString();
            divResult.Visible = result.Count > 0;
            BindPagedResults();
        }
        private List<LawExtraDoc> LoadAllUnlinkedDocs()
        {
            var query = @"SELECT d.Code, d.DocNum, d.DocYear, d.DocSubject, d.DocFilepath, t.NameAr AS DocTypeName
            FROM Law_DocData d

            LEFT JOIN View_LawsLinkedDocs l ON l.SouceDocID = d.Code OR l.DestDocId = d.Code
            LEFT JOIN Law_DocType t ON d.DocTypeID = t.Code
            WHERE l.Code IS NULL
              AND (@docTypeId IS NULL OR d.DocTypeID = @docTypeId)
              AND (@docCategoryId IS NULL OR d.DocCategoryID = @docCategoryId)
              AND (@docNum IS NULL OR d.DocNum = @docNum)
              AND (@docYear IS NULL OR d.DocYear = @docYear)

              AND (@UnderStudy IS NULL OR d.UnderStudy = @UnderStudy)
              AND (@isPublished IS NULL OR d.isPublished = @isPublished)
              AND (@DocDateFrom IS NULL OR d.DocDate >= @DocDateFrom)
              AND (@DocDateTo IS NULL OR d.DocDate <= @DocDateTo)
              AND (@ExpireDateFrom IS NULL OR ExpireDate >= @ExpireDateFrom)
              AND (@ExpireDateTo IS NULL OR ExpireDate <= @ExpireDateTo)
              AND (@DocSubject IS NULL OR d.DocSubject LIKE '%' + @DocSubject + '%')
              AND (@DocNotes IS NULL OR d.DocNotes LIKE '%' + @DocNotes + '%')
              AND (@DocDetails IS NULL OR d.DocDetails LIKE '%' + @DocDetails + '%')";

            return objRepository.DC.Database.SqlQuery<LawExtraDoc>(
                query,
                new SqlParameter("@DocNum", GetNullableInt(txtFilterSerialNum.Text)),
                new SqlParameter("@DocYear", GetNullableInt(txtFilterSerialYear.Text)),
                new SqlParameter("@DocTypeID", GetNullableInt(lstFilterType.SelectedValue)),
                new SqlParameter("@DocCategoryID", GetNullableInt(lstFilterCategory.SelectedValue)),
                new SqlParameter("@UnderStudy", GetNullableBool(lstFilterIsUnderStudy.SelectedValue)),
                new SqlParameter("@isPublished", GetNullableBool(lstFilterPublish.SelectedValue)),
                new SqlParameter("@DocDateFrom", GetNullableDate(txtFilterDatefrom.Text)),
                new SqlParameter("@DocDateTo", GetNullableDate(txtFilterDateTo.Text)),
                new SqlParameter("@DocSubject", GetNullableString(txtFilterSubject.Text)),
                new SqlParameter("@DocNotes", GetNullableString(txtFilterNotes.Text)),
                new SqlParameter("@DocDetails", GetNullableString(txtFilterDetails.Text)),
                new SqlParameter("@ExpireDateFrom", GetNullableDate(txtFilterExpireFrom.Text)),
                new SqlParameter("@ExpireDateTo", GetNullableDate(txtFilterExpireTo.Text))).ToList();
        }
    
        private void BindPagedResults()
        {
            var result = Session["LawSearchResults"] as List<LawHierarchyResult> ?? new List<LawHierarchyResult>();
            _groupIndex = 0;
            var startIndex = CurrentPageIndex * PageSize;
            _rootChildIndex = result
                .Take(startIndex)
                .Where(item => item.ParentDocId.HasValue)
                .GroupBy(item => GetRoot(item))
                .ToDictionary(group => group.Key, group => group.Count());
            var paged = new PagedDataSource
            {
                DataSource = result,
                AllowPaging = true,
                PageSize = PageSize,
                CurrentPageIndex = CurrentPageIndex
            };

            //_groupIndex = 0;
           

            dlLawHierarchy.DataSource = paged;
            dlLawHierarchy.DataBind();

            btnPrev.Enabled = !paged.IsFirstPage;
            btnNext.Enabled = !paged.IsLastPage;

            var totalPages = Math.Max(paged.PageCount, 1);
            var windowSize = 10;
            var startPage = ((CurrentPageNumber - 1) / windowSize) * windowSize + 1;
            var endPage = Math.Min(startPage + windowSize - 1, totalPages);
            var pages = Enumerable.Range(startPage, endPage - startPage + 1).ToList();
            rptPages.DataSource = pages;
            rptPages.DataBind();
        }

        protected void dlLawHierarchy_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            var dataItem = e.Item.DataItem as LawHierarchyResult;
            if (dataItem == null)
            {
                return;
            }

            var lblIndex = e.Item.FindControl("lblItemIndex") as Label;
            if (lblIndex == null)
            {
                return;
            }

            if (!dataItem.ParentDocId.HasValue)
            {

                lblIndex.Text = "";
                _groupIndex = 0;
            }
            else
            {
                var rootKey = GetRoot(dataItem);
                int currentIndex;
                _rootChildIndex.TryGetValue(rootKey, out currentIndex);
                currentIndex++;
                _rootChildIndex[rootKey] = currentIndex;
                lblIndex.Text = currentIndex.ToString();

            }
            var rowContainer = e.Item.FindControl("rowContainer") as System.Web.UI.HtmlControls.HtmlGenericControl;

            if (rowContainer != null)
            {
                int level = 0;

                if (dataItem.Level != null)
                    level = Convert.ToInt32(dataItem.Level);

                // اللون الافتراضي لو ParentDocId = null
                if (dataItem.ParentDocId == null)
                {
                    rowContainer.Style["background-color"] = "#719cda40";
                    rowContainer.Style["border-radius"] = "25px";
                    rowContainer.Style["padding-right"] = "2%";
                    rowContainer.Style["padding-top"] = "15px";
                }

                // لو مطابق للفيلتر
                if (!string.IsNullOrEmpty(txtFilterSerialNum.Text) &&
                    !string.IsNullOrEmpty(txtFilterSerialYear.Text) &&
                    dataItem.DocNum.HasValue &&
                    dataItem.DocYear.HasValue)
                {
                    if (dataItem.DocNum.Value == ZeroIntergerIFNull(txtFilterSerialNum.Text)
                        && dataItem.DocYear.Value == ZeroIntergerIFNull(txtFilterSerialYear.Text))
                    {
                        rowContainer.Style["background-color"] = "#fff3a0";
                        rowContainer.Style["border-radius"] = "25px";
                        rowContainer.Style["padding-top"] = "15px";
                    }
                }
            }
        }

        private object GetNullableInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return DBNull.Value;
            }

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
    }
}
