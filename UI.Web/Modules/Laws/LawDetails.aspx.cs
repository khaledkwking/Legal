using Infrastructure;
using Infrastructure.DAL;
using Infrastructure.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using UI.Web.Admin.Controller;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;

namespace UI.Web.Modules.Laws
{
    public partial class LawDetails : BaseFormAdmin
    {
        private const int PageSize = 20;

        private int _groupIndex;
        private Dictionary<string, int> _rootChildIndex = new Dictionary<string, int>();
        private class LawHierarchyResult
        {
            public int? Level { get; set; }
            public int? ParentDocId { get; set; }
            public int? ChildDocId { get; set; }
            public string Path { get; set; }
            public int? DocNum { get; set; }
            public int? DocYear { get; set; }
            public string DocSerial { get; set; }
            public string DocSubject { get; set; }
            public string DocDetails { get; set; }
            public string DocFilepath { get; set; }
            public string DocTypeName { get; set; }
            public string DocDescriptionHTML { get; set; }
            public string DocProceduresTypesNameAr { get; set; }
            public int? LastProcedureID { get; set; }
        }

        private class LawExtraDoc
        {
            public int Code { get; set; }
            public int? DocNum { get; set; }
            public int? DocYear { get; set; }
            public string DocSerial { get; set; }
            public string DocSubject { get; set; }
            public string DocDetails { get; set; }
            public string DocFilepath { get; set; }
            public string DocTypeName { get; set; }
        }

        private class LawTypeItem
        {
            public int Code { get; set; }
            public string NameAr { get; set; }
        }
        private class CategoryItem
        {
            public int Code { get; set; }
            public string NameAr { get; set; }
        }

        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LawsAttachments/";
        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();

        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();

        private int CurrentPageIndex
        {
            get { return ViewState["LawDetailsPageIndex"] != null ? (int)ViewState["LawDetailsPageIndex"] : 0; }
            set { ViewState["LawDetailsPageIndex"] = value; }
        }

        public int CurrentPageNumber
        {
            get { return CurrentPageIndex + 1; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnSectorId.Value = gets(Request.QueryString["sectorId"]);
                hdnTypeId.Value = gets(Request.QueryString["typeId"]);
                BindDocTypes();
                BindCategories();
                
                FillSearchResults();
            }
        }

        public string GetProcedureBadges(object proceduresObj)
        {
            string proc = Convert.ToString(proceduresObj);
            if (string.IsNullOrWhiteSpace(proc)) return string.Empty;

            bool hasIstadraq = proc.IndexOf("استدراك", StringComparison.OrdinalIgnoreCase) >= 0;
            bool hasCourt = proc.IndexOf("حكم محم", StringComparison.OrdinalIgnoreCase) >= 0 || proc.IndexOf("حكم محكمة", StringComparison.OrdinalIgnoreCase) >= 0;
            bool hasAmend = proc.IndexOf("تعديل", StringComparison.OrdinalIgnoreCase) >= 0;
            bool hasRepeal = proc.IndexOf("إلغاء", StringComparison.OrdinalIgnoreCase) >= 0 || proc.IndexOf("الغاء", StringComparison.OrdinalIgnoreCase) >= 0 || proc.IndexOf("إبطال", StringComparison.OrdinalIgnoreCase) >= 0;
            bool hasIsMaraslat = proc.IndexOf("كتب ومراسلات", StringComparison.OrdinalIgnoreCase) >= 0;
            bool hasParliament = proc.IndexOf("وارد رئيس مجلس الأمة", StringComparison.OrdinalIgnoreCase) >= 0 || proc.IndexOf("وارد رئيس", StringComparison.OrdinalIgnoreCase) >= 0;

            string result = string.Empty;
            if (hasIstadraq)
            {
                result += "<span class=\"status status-alt-red\"><i class=\"fa fa-exclamation-circle\"></i> يحتوي على استدراك</span>";
            }
            if (hasCourt)
            {
                if (!string.IsNullOrEmpty(result)) result += " ";
                result += "<span class=\"status status-court\"><i class=\"fa fa-bookmark-o\"></i> يحتوي على حكم محكمة</span>";
            }
            if (hasAmend)
            {
                if (!string.IsNullOrEmpty(result)) result += " ";
                result += "<span class=\"status status-amend\"><i class=\"fa fa-pencil\"></i> يحتوي على تعديل قانون</span>";
            }
            if (hasRepeal)
            {
                if (!string.IsNullOrEmpty(result)) result += " ";
                result += "<span class=\"status status-alt-blue\"><i class=\"fa fa-ban\"></i> يحتوي على إلغاء قانون</span>";
            }
            if (hasIsMaraslat)
            {
                if (!string.IsNullOrEmpty(result)) result += " ";
                result += "<span class=\"status status-court\"><i class=\"fa fa-ban\"></i> يحتوي على كتب ومراسلات</span>";
            }
            if (hasParliament)
            {
                if (!string.IsNullOrEmpty(result)) result += " ";
                result += "<span class=\"status status-parliament\"><i class=\"fa fa-envelope\"></i> يحتوي على وارد رئيس مجلس الأمة</span>";
            }

            // if neither matched, fall back to displaying the original text
            if (string.IsNullOrEmpty(result))
            {
                return "يحتوي على " + proc;
            }

            return result;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
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
       
        protected void dlLawDetails_ItemDataBound(object sender, DataListItemEventArgs e)
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

            var phMasterToggle = e.Item.FindControl("phMasterToggle") as PlaceHolder;
            var phChildMarker = e.Item.FindControl("phChildMarker") as PlaceHolder;

            if (!dataItem.ParentDocId.HasValue)
            {
                lblIndex.Text = "";
                lblIndex.Visible = false;
                if (phMasterToggle != null) phMasterToggle.Visible = true;
                if (phChildMarker != null) phChildMarker.Visible = false;
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
                lblIndex.Visible = true;
                if (phMasterToggle != null) phMasterToggle.Visible = false;
                if (phChildMarker != null) phChildMarker.Visible = true;
                
            }

            var rowContainer = e.Item.FindControl("rowContainer") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (rowContainer != null)
            {
                var rootKey = dataItem.ParentDocId.HasValue ? GetRoot(dataItem) : gets(dataItem.ChildDocId);
                rowContainer.Attributes["data-root-key"] = rootKey;

                if (dataItem.ParentDocId == null)
                {
                    rowContainer.Attributes["class"] = "law-row law-master-item";
                    rowContainer.Attributes["data-law-master"] = "true";
                    rowContainer.Attributes["aria-expanded"] = "true";
                    rowContainer.Attributes["tabindex"] = "0";
                }
                else
                {
                    rowContainer.Attributes["class"] = "law-row law-child-item";
                    rowContainer.Attributes.Remove("data-law-master");
                }

                var level = dataItem.Level ?? 0;
                //rowContainer.Style["padding-right"] = (level * 30) + "px";
                if (dataItem.ParentDocId == null)
                {
                    rowContainer.Style["background-color"] = "#719cda40";
                    rowContainer.Style["border-radius"] = "25px";
                    rowContainer.Style["padding-right"] = "1%";
                    rowContainer.Style["padding-top"] = "15px";
                }

                var docNumFilter = ZeroIntergerIFNull(txtDocNum.Text);
                var docYearFilter = ZeroIntergerIFNull(txtYearFrom.Text);
                if (docNumFilter != 0
                    && dataItem.DocNum.HasValue
                    && dataItem.DocNum.Value == docNumFilter
                    && (docYearFilter == 0 || (dataItem.DocYear.HasValue && dataItem.DocYear.Value == docYearFilter)))
                {
                    rowContainer.Style["background-color"] = "#fff3a0";
                    rowContainer.Style["border-radius"] = "25px";
                    rowContainer.Style["padding-right"] = "1%";
                    rowContainer.Style["padding-top"] = "15px";
                }
            }
        }
        private void BindDocTypes()
        {

            if (int.Parse(GetNullableInt(hdnTypeId.Value).ToString()) == 1)
            {
                var DocTypes = objRepository.DC.Database.SqlQuery<LawTypeItem>(
               "SELECT Code, NameAr FROM Law_DocType WHERE Code = @p0 or Code=@p1", Constant.Law_DocTypeQanuan, Constant.Law_DocTypeQanuanMarsum).ToList();
                DocTypeDiv.Visible = true;


                rblDocType.Items.Clear();

                //rblDocType.Items.Add(new ListItem("قانون", Constant.Law_DocTypeQanuan.ToString()));
                //rblDocType.Items.Add(new ListItem("مرسوم بقانون", Constant.Law_DocTypeQanuanMarsum.ToString ()));


                //foreach (var Type in DocTypes)
                //{
                //    rblDocType.Items.Add(new System.Web.UI.WebControls.ListItem(Type.NameAr, Type.Code.ToString()));

                //}
                //rblDocType.SelectedValue = "1";
                rblDocType.Items.Add(
                    new System.Web.UI.WebControls.ListItem("الكل", "0")
                );
                foreach (var type in DocTypes)
                {
                    rblDocType.Items.Add(
                        new System.Web.UI.WebControls.ListItem( type.NameAr,  type.Code.ToString()
                        )
                    );
                }
                
                rblDocType.SelectedValue = "0";

            }
            else
            {
                DocTypeDiv.Visible = false;
            }
        }

        private void BindCategories()
        {
            if (!string.IsNullOrEmpty(rblDocType.SelectedValue))
            {
                hdnTypeId.Value = rblDocType.SelectedValue.ToString();
            }
            var typeId = ZeroIntergerIFNull(hdnTypeId.Value);
            var sectorId = ZeroIntergerIFNull(hdnSectorId.Value);

            var qTypes = objRepository.DC.Database.SqlQuery<LawTypeItem>(
                    "SELECT Code, NameAr FROM Law_DocType WHERE Code = @p0", typeId).ToList();

            if (qTypes.Count > 0)
                if (qTypes[0].Code == Constant.Law_DocTypeQanuan)
                {
                    lblTitle.Text = Constant.Law_DocTypeQanuanDesc;
                }
                else
                {
                    lblTitle.Text = qTypes[0].NameAr;
                }

            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem("الكل", "0"));
            if (sectorId == 0)
            {
                var categories = objRepository.DC.Database.SqlQuery<CategoryItem>(
                    "SELECT Code, NameAr FROM Law_DocCategory WHERE (@p0 = 0 OR TypeID = @p0) ORDER BY NameAr",
                   typeId).ToList();
                foreach (var category in categories)
                {
                    ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem(category.NameAr, category.Code.ToString()));
                }
            }
            else
            {
                var categories = objRepository.DC.Database.SqlQuery<CategoryItem>(
                        @"SELECT DISTINCT CS.Law_DocCategoryId AS Code, C.NameAr
                      FROM Law_DocCategorySectors CS
                      INNER JOIN Law_DocCategory C 
                          ON CS.Law_DocCategoryId = C.Code
                      WHERE ( CS.SectorId = @p0)
                      ORDER BY C.NameAr",
                         sectorId).ToList();
                foreach (var category in categories)
                {
                    ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem(category.NameAr, category.Code.ToString()));
                }
            }
           
        }
        string GetRoot(LawHierarchyResult x)
        {
            if (string.IsNullOrWhiteSpace(x.Path))
                return x.ChildDocId.ToString();

            return x.Path.Split('>')[0].Trim();
        }
        private void FillSearchResults()
        {
            int RelatedFlag = 0; // display all related if =1 or 0 non related

            if (!string.IsNullOrEmpty(RadioButtonRelatedList.SelectedValue))
            {
                RelatedFlag = int.Parse(RadioButtonRelatedList.SelectedValue);
            }
           
            if (!string.IsNullOrEmpty(rblDocType.SelectedValue))
            {
                hdnTypeId.Value = rblDocType.SelectedValue.ToString();
            }
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
            var docYearParam = GetExactYearParam();
            if (RelatedFlag == 1) // مرتبط
            {
               
              

             var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DocId", DBNull.Value),
                new SqlParameter("@DocNum", GetNullableInt(txtDocNum.Text)),
                new SqlParameter("@DocYear", docYearParam),
                //new SqlParameter("@DocTypeID", GetNullableInt(hdnTypeId.Value)),
                new SqlParameter( "@DocTypeID", hdnTypeId.Value.ToString() != "0" ? (object)GetNullableInt(hdnTypeId.Value): DBNull.Value),
                new SqlParameter("@DocCategoryID", GetNullableInt(ddlCategory.SelectedValue)),
                new SqlParameter("@UnderStudy", DBNull.Value),
                new SqlParameter("@isPublished", DBNull.Value),
                new SqlParameter("@DocDateFrom", DBNull.Value),
                new SqlParameter("@DocDateTo", DBNull.Value),
                new SqlParameter("@DocSubject", txtDocSubject.Text),
                new SqlParameter("@DocNotes", DBNull.Value),
                new SqlParameter("@DocDetails", txtDetails.Text),
                new SqlParameter("@ExpireDateFrom", DBNull.Value),
                new SqlParameter("@ExpireDateTo", DBNull.Value),
                new SqlParameter("@Flag", RelatedFlag)

            };


                var result = objRepository.DC.Database.SqlQuery<LawHierarchyResult>(
                    "EXEC sp_SearchLawHierarchyV9 @DocId, @DocNum, @DocYear, @DocTypeID, @DocCategoryID, @UnderStudy, @isPublished, @DocDateFrom, @DocDateTo, @DocSubject, @DocNotes, @DocDetails, @ExpireDateFrom, @ExpireDateTo,@Flag",
                    parameters.ToArray()
                ).ToList();



                var sectorId = ZeroIntergerIFNull(hdnSectorId.Value);

                if (sectorId != 0)
                {
                    // 2️⃣ Docs linked to sector
                    var sectorDocIds = new HashSet<int>(
                        objRepository.DC.Database.SqlQuery<int>(
                            "SELECT Law_DocId FROM Law_DocSectors WHERE SectorId = @p0",
                            sectorId
                        )
                    );

                    var all = result.ToList();

                    // 3️⃣ Build Parent-Child maps
                    var parentToChildren = all
                        .Where(x => x.ParentDocId.HasValue && x.ChildDocId.HasValue)
                        .ToLookup(x => x.ParentDocId.Value, x => x.ChildDocId.Value);

                    var childToParent = all
                  .Where(x => x.ParentDocId.HasValue && x.ChildDocId.HasValue)
                  .GroupBy(x => x.ChildDocId.Value)
                  .ToDictionary(
                      g => g.Key,
                      g => g.First().ParentDocId.Value
                  );

                    // 4️⃣ Find ROOT of each node
                    int FindRoot(int id)
                    {
                        while (childToParent.ContainsKey(id))
                            id = childToParent[id];

                        return id;
                    }

                    // 5️⃣ Group nodes by root
                    var rootsToNodes = new Dictionary<int, HashSet<int>>();

                    foreach (var docId in sectorDocIds)
                    {
                        int root = FindRoot(docId);

                        if (!rootsToNodes.ContainsKey(root))
                            rootsToNodes[root] = new HashSet<int>();

                        rootsToNodes[root].Add(docId);
                    }

                    // 6️⃣ Collect full trees for those roots only
                    var allowedNodes = new HashSet<int>();

                    void CollectTree(int id)
                    {
                        if (!allowedNodes.Add(id))
                            return;

                        foreach (var child in parentToChildren[id])
                            CollectTree(child);
                    }

                    foreach (var root in rootsToNodes.Keys)
                    {
                        CollectTree(root);
                    }

                    // 7️⃣ Keep only nodes related to the sector tree
                    result = all.Where(x =>
                        (x.ChildDocId.HasValue && allowedNodes.Contains(x.ChildDocId.Value)) ||
                        (x.ParentDocId.HasValue && allowedNodes.Contains(x.ParentDocId.Value))
                    ).ToList();



                    // 8️⃣ Include unlinked docs for this sector as well
                    var extraDocs = LoadSectorUnlinkedDocs(sectorId, docYearParam);
                    var existingIds = new HashSet<int>(result.Where(item => item.ChildDocId.HasValue)
                        .Select(item => item.ChildDocId.Value));

                    foreach (var extra in extraDocs)
                    {
                        if (existingIds.Contains(extra.Code))
                        {
                            continue;
                        }

                        result.Add(new LawHierarchyResult
                        {
                            Level = 0,
                            ParentDocId = null,
                            ChildDocId = extra.Code,
                            DocNum = extra.DocNum,
                            DocYear = extra.DocYear,
                            DocSerial = extra.DocSerial,
                            DocSubject = extra.DocSubject,
                            DocFilepath = extra.DocFilepath,
                            DocTypeName = extra.DocTypeName,
                            DocDescriptionHTML = string.Empty,
                            DocProceduresTypesNameAr = string.Empty
                            
                        });
                    }

                    result = dedupeResults(result);
                }
                else
                {
                    var extraDocs = LoadAllUnlinkedDocs(docYearParam);
                    var existingIds = new HashSet<int>(result.Where(item => item.ChildDocId.HasValue)
                        .Select(item => item.ChildDocId.Value));

                    foreach (var extra in extraDocs)
                    {
                        if (existingIds.Contains(extra.Code))
                        {
                            continue;
                        }

                        result.Add(new LawHierarchyResult
                        {
                            Level = 0,
                            ParentDocId = null,
                            ChildDocId = extra.Code,
                            DocNum = extra.DocNum,
                            DocYear = extra.DocYear,
                            DocSerial = extra.DocSerial,
                            DocSubject = extra.DocSubject,
                            DocFilepath = extra.DocFilepath,
                            DocTypeName = extra.DocTypeName,
                            DocDescriptionHTML = string.Empty,
                            DocProceduresTypesNameAr = string.Empty
                        });
                    }

                    result = dedupeResults(result);
                }

                // filter th result
                var yearFrom = ZeroIntergerIFNull(txtYearFrom.Text);
                var yearTo = ZeroIntergerIFNull(txtYearTo.Text);
                var hasDocNum = !string.IsNullOrWhiteSpace(txtDocNum.Text);
                var hasExactYear = (yearFrom != 0 && (yearTo == 0 || yearTo == yearFrom)) || (yearTo != 0 && yearFrom == 0);
                var applyYearFilter = !hasDocNum && !hasExactYear;

                if (applyYearFilter && yearFrom != 0)
                {
                    result = result.Where(item => item.DocYear.HasValue && item.DocYear.Value >= yearFrom).ToList();
                }

                if (applyYearFilter && yearTo != 0)
                {
                    result = result.Where(item => item.DocYear.HasValue && item.DocYear.Value <= yearTo).ToList();
                }
                if (!string.IsNullOrWhiteSpace(txtDocSubject.Text))
                {
                    result = result.Where(item => item.DocSubject.Contains(txtDocSubject.Text)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(txtDetails.Text))
                {
                    result = result.Where(item => item.DocDetails != "" ? (item.DocDetails != null && item.DocDetails.Contains(txtDetails.Text)) : 1 == 1).ToList();

                        //item => item.DocDetails.Contains(txtDetails.Text)).ToList();
                }
                var rootYearMap = result
                       .Where(x => x.ParentDocId == null)
                        .ToDictionary(
                               x => x.ChildDocId.ToString(),
                               x => new { x.DocYear, x.DocNum }
                           );

                // الترتيب
                if (RButtonSortList.SelectedValue != null)
                {
                    if (RButtonSortList.SelectedValue.ToString() == "1")
                    {
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
                    }
                    else if (RButtonSortList.SelectedValue.ToString() == "2")
                    {
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
                            .ThenBy(x => x.Item.DocYear) // children بالأحدث

                            .ThenBy(x => x.Item.DocNum)

                            .Select(x => x.Item)

                            .ToList();
                    }
                }
                    
                result = result
                    .GroupBy(x => new { x.ParentDocId, x.ChildDocId })
                    .Select(g => g.First())
                    .ToList();



                Session["LawDetailsResults"] = result;
                lblResultCount.Text = result.Count.ToString();
                divResult.Visible = result.Count > 0;
                BindPagedResults();
            }
            else // غير مرتبط
            {
                int SectorId = 0;
                if (!string.IsNullOrEmpty(hdnSectorId.Value.ToString()))
                {
                    SectorId = int.Parse(hdnSectorId.Value.ToString());
                }
               var extraDocs = LoadAllwitoutRelatedDocs(SectorId, docYearParam);
                if (!string.IsNullOrWhiteSpace(txtDetails.Text))
                {
                    extraDocs = extraDocs.Where(item => string.IsNullOrWhiteSpace(txtDetails.Text) ||
                   (!string.IsNullOrEmpty(item.DocDetails) &&
                    item.DocDetails.Contains(txtDetails.Text))).ToList();


                    //item => item.DocDetails != "" ? (item.DocDetails != null && item.DocDetails.Contains(txtDetails.Text)) : 1 == 1).ToList();

                    //item => item.DocDetails.Contains(txtDetails.Text)).ToList();
                }
                //var existingIds = new HashSet<int>(result.Where(item => item.ChildDocId.HasValue)
                //    .Select(item => item.ChildDocId.Value));
                List<LawHierarchyResult> NoRelatedList = new List<LawHierarchyResult>();
                foreach (var extra in extraDocs)
                {
                    //if (existingIds.Contains(extra.Code))
                    //{
                    //    continue;
                    //}
                    NoRelatedList.Add(new LawHierarchyResult
                    {
                        Level = 0,
                        ParentDocId = null,
                        ChildDocId = extra.Code,
                        DocNum = extra.DocNum,
                        DocYear = extra.DocYear,
                        DocSerial = extra.DocSerial,
                        DocSubject = extra.DocSubject,
                        DocFilepath = extra.DocFilepath,
                        DocTypeName = extra.DocTypeName,
                        DocDescriptionHTML = string.Empty,
                        DocProceduresTypesNameAr = string.Empty
                    });
                }
                NoRelatedList=NoRelatedList.OrderByDescending(c => c.DocYear).ToList();
                NoRelatedList = dedupeResults(NoRelatedList);
                Session["LawDetailsResults"] = NoRelatedList;
                lblResultCount.Text = NoRelatedList.Count.ToString();
                divResult.Visible = NoRelatedList.Count > 0;
                BindPagedResults();
            }
            
            
        }

        private void BindPagedResults()
        {
            var result = Session["LawDetailsResults"] as List<LawHierarchyResult> ?? new List<LawHierarchyResult>();
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

            dlLawDetails.DataSource = paged;
            dlLawDetails.DataBind();

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

        private object GetExactYearParam()
        {
            var yearFrom = ZeroIntergerIFNull(txtYearFrom.Text);
            var yearTo = ZeroIntergerIFNull(txtYearTo.Text);

            if (yearFrom == 0 && yearTo == 0)
            {
                return DBNull.Value;
            }

            if (yearFrom != 0 && (yearTo == 0 || yearTo == yearFrom))
            {
                return yearFrom;
            }

            return DBNull.Value;
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

        public string GetAttachmentUrl(object docId, object docFilepath)
        {
            var fileName = gets(docFilepath);
            var idValue = gets(docId);
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(idValue))
            {
                return "#";
            }

            return ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + idValue + "/" + "&vfileList=[" + fileName + ";]";
        }

        private List<LawExtraDoc> LoadSectorUnlinkedDocs(int sectorId, object docYearParam)
        {
            var query = @"SELECT d.Code, d.DocNum, d.DocYear, d.DocSubject,d.DocSerial, d.DocFilepath, t.NameAr AS DocTypeName
            FROM Law_DocData d
            INNER JOIN Law_DocSectors s ON s.Law_DocId = d.Code AND s.SectorId = @sectorId
            LEFT JOIN View_LawsLinkedDocs l ON l.SouceDocID = d.Code OR l.DestDocId = d.Code
            LEFT JOIN Law_DocType t ON d.DocTypeID = t.Code
            WHERE l.Code IS NULL
              AND (@docTypeId IS NULL OR d.DocTypeID = @docTypeId)
              AND (@docCategoryId IS NULL OR d.DocCategoryID = @docCategoryId)
              AND (@docNum IS NULL OR d.DocNum = @docNum)
              AND (@docYear IS NULL OR d.DocYear = @docYear)";

            if (hdnTypeId.Value.ToString() == "0")
            {
                query = @"SELECT d.Code, d.DocNum, d.DocYear, d.DocSubject,d.DocSerial, d.DocFilepath, t.NameAr AS DocTypeName
                    FROM Law_DocData d
                    INNER JOIN Law_DocSectors s ON s.Law_DocId = d.Code AND s.SectorId = @sectorId
                    LEFT JOIN View_LawsLinkedDocs l ON l.SouceDocID = d.Code OR l.DestDocId = d.Code
                    LEFT JOIN Law_DocType t ON d.DocTypeID = t.Code
                    WHERE l.Code IS NULL
                      AND (d.DocTypeID = @docTypeId1 or d.DocTypeID = @docTypeId2)
                      AND (@docCategoryId IS NULL OR d.DocCategoryID = @docCategoryId)
                      AND (@docNum IS NULL OR d.DocNum = @docNum)
                      AND (@docYear IS NULL OR d.DocYear = @docYear)";
                return objRepository.DC.Database.SqlQuery<LawExtraDoc>(
               query,
               new SqlParameter("@sectorId", sectorId),
               new SqlParameter("@docTypeId1", Constant.Law_DocTypeQanuan),
               new SqlParameter("@docTypeId2", Constant.Law_DocTypeQanuanMarsum),
               new SqlParameter("@docCategoryId", GetNullableInt(ddlCategory.SelectedValue)),
               new SqlParameter("@docNum", GetNullableInt(txtDocNum.Text)),
               new SqlParameter("@docYear", docYearParam)).ToList();
            }
            else
            {

                return objRepository.DC.Database.SqlQuery<LawExtraDoc>(
                    query,
                    new SqlParameter("@sectorId", sectorId),
                    new SqlParameter("@docTypeId", GetNullableInt(hdnTypeId.Value)),
                    new SqlParameter("@docCategoryId", GetNullableInt(ddlCategory.SelectedValue)),
                    new SqlParameter("@docNum", GetNullableInt(txtDocNum.Text)),
                    new SqlParameter("@docYear", docYearParam)).ToList();
            }
        }
        private List<LawExtraDoc> LoadAllUnlinkedDocs( object docYearParam)
        {


            var query = @"SELECT d.Code, d.DocNum, d.DocYear, d.DocSubject,d.DocSerial, d.DocFilepath, t.NameAr AS DocTypeName
            FROM Law_DocData d

            LEFT JOIN View_LawsLinkedDocs l ON l.SouceDocID = d.Code OR l.DestDocId = d.Code
            LEFT JOIN Law_DocType t ON d.DocTypeID = t.Code
            WHERE l.Code IS NULL
              AND (@docTypeId IS NULL OR d.DocTypeID = @docTypeId)
              AND (@docCategoryId IS NULL OR d.DocCategoryID = @docCategoryId)
              AND (@docNum IS NULL OR d.DocNum = @docNum)
              AND (@docYear IS NULL OR d.DocYear = @docYear)";

            if (hdnTypeId.Value.ToString() == "0")
            {
                 query = @"SELECT d.Code, d.DocNum, d.DocYear, d.DocSubject,d.DocSerial, d.DocFilepath, t.NameAr AS DocTypeName
                FROM Law_DocData d

                LEFT JOIN View_LawsLinkedDocs l ON l.SouceDocID = d.Code OR l.DestDocId = d.Code
                LEFT JOIN Law_DocType t ON d.DocTypeID = t.Code
                WHERE l.Code IS NULL
                  AND (d.DocTypeID = @docTypeId1 or d.DocTypeID = @docTypeId2)
                  AND (@docCategoryId IS NULL OR d.DocCategoryID = @docCategoryId)
                  AND (@docNum IS NULL OR d.DocNum = @docNum)
                  AND (@docYear IS NULL OR d.DocYear = @docYear)";

                return objRepository.DC.Database.SqlQuery<LawExtraDoc>(
                query,
                    new SqlParameter("@docTypeId1", Constant.Law_DocTypeQanuan),
                    new SqlParameter("@docTypeId2", Constant.Law_DocTypeQanuanMarsum),
                new SqlParameter("@docCategoryId", GetNullableInt(ddlCategory.SelectedValue)),
                new SqlParameter("@docNum", GetNullableInt(txtDocNum.Text)),
                new SqlParameter("@docYear", docYearParam)).ToList();

            }
            {
                return objRepository.DC.Database.SqlQuery<LawExtraDoc>(
                query,
                new SqlParameter("@docTypeId", GetNullableInt(hdnTypeId.Value)),
                new SqlParameter("@docCategoryId", GetNullableInt(ddlCategory.SelectedValue)),
                new SqlParameter("@docNum", GetNullableInt(txtDocNum.Text)),
                new SqlParameter("@docYear", docYearParam)).ToList();
            }
        }

        private List<LawExtraDoc> LoadAllwitoutRelatedDocs(int sectorId, object docYearParam)
        {
            if (sectorId > 0)
            {
                var query = @"SELECT d.Code, d.DocNum, d.DocYear,d.DocSerial, d.DocSubject,d.DocDetails, d.DocFilepath, t.NameAr AS DocTypeName
            FROM Law_DocData d
            INNER JOIN Law_DocSectors s ON s.Law_DocId = d.Code AND s.SectorId = @sectorId
            LEFT JOIN Law_DocType t ON d.DocTypeID = t.Code
            WHERE (@docTypeId IS NULL OR d.DocTypeID = @docTypeId)
              AND (@docCategoryId IS NULL OR d.DocCategoryID = @docCategoryId)
              AND (@docNum IS NULL OR d.DocNum = @docNum)
              AND (@docSubject IS NULL OR d.docSubject = @docSubject)
              AND (@docYear IS NULL OR d.DocYear = @docYear)";

                if (hdnTypeId.Value.ToString() == "0")
                {
                    query = @"SELECT d.Code, d.DocNum, d.DocYear,d.DocSerial, d.DocSubject,d.DocDetails, d.DocFilepath, t.NameAr AS DocTypeName
                    FROM Law_DocData d
                    INNER JOIN Law_DocSectors s ON s.Law_DocId = d.Code AND s.SectorId = @sectorId
                    LEFT JOIN Law_DocType t ON d.DocTypeID = t.Code
                    WHERE  (d.DocTypeID = @docTypeId1 or d.DocTypeID = @docTypeId2)
                      AND (@docCategoryId IS NULL OR d.DocCategoryID = @docCategoryId)
                      AND (@docNum IS NULL OR d.DocNum = @docNum)
                      AND (@docSubject IS NULL OR d.docSubject = @docSubject)
                      AND (@docYear IS NULL OR d.DocYear = @docYear)";

                    return objRepository.DC.Database.SqlQuery<LawExtraDoc>(
                        query,
                        new SqlParameter("@sectorId", sectorId),
                        new SqlParameter("@docTypeId1", Constant.Law_DocTypeQanuan),
                        new SqlParameter("@docTypeId2", Constant.Law_DocTypeQanuanMarsum),
                        new SqlParameter("@docCategoryId", GetNullableInt(ddlCategory.SelectedValue)),
                        new SqlParameter("@docNum", GetNullableInt(txtDocNum.Text)),
                         new SqlParameter("@docSubject", GetNullableInt(txtDocSubject.Text)),
                        new SqlParameter("@docYear", docYearParam)).ToList();
                }
                else
                {

                    return objRepository.DC.Database.SqlQuery<LawExtraDoc>(
                        query,
                        new SqlParameter("@sectorId", sectorId),
                        new SqlParameter("@docTypeId", GetNullableInt(hdnTypeId.Value)),
                        new SqlParameter("@docCategoryId", GetNullableInt(ddlCategory.SelectedValue)),
                        new SqlParameter("@docNum", GetNullableInt(txtDocNum.Text)),
                         new SqlParameter("@docSubject", GetNullableInt(txtDocSubject.Text)),
                        new SqlParameter("@docYear", docYearParam)).ToList();
                }
            }
            
            else
            {

                var query = @"SELECT d.Code, d.DocNum, d.DocYear, d.DocSubject,d.DocDetails, d.DocFilepath, t.NameAr AS DocTypeName
                FROM Law_DocData d
                LEFT JOIN Law_DocType t ON d.DocTypeID = t.Code
                WHERE (@docTypeId IS NULL OR d.DocTypeID = @docTypeId)
                  AND (@docCategoryId IS NULL OR d.DocCategoryID = @docCategoryId)
                  AND (@docNum IS NULL OR d.DocNum = @docNum)
                  AND (@docSubject IS NULL OR d.docSubject = @docSubject)
                  AND (@docYear IS NULL OR d.DocYear = @docYear)";
                if (hdnTypeId.Value.ToString() == "0")
                {
                     query = @"SELECT d.Code, d.DocNum, d.DocYear, d.DocSubject,d.DocDetails, d.DocFilepath, t.NameAr AS DocTypeName
                    FROM Law_DocData d
                    LEFT JOIN Law_DocType t ON d.DocTypeID = t.Code
                      WHERE  (d.DocTypeID = @docTypeId1 or d.DocTypeID = @docTypeId2)
                      AND (@docCategoryId IS NULL OR d.DocCategoryID = @docCategoryId)
                      AND (@docNum IS NULL OR d.DocNum = @docNum)
                      AND (@docSubject IS NULL OR d.docSubject = @docSubject)
                      AND (@docYear IS NULL OR d.DocYear = @docYear)";

                    return objRepository.DC.Database.SqlQuery<LawExtraDoc>(
                    query,
                    new SqlParameter("@sectorId", sectorId),
                    new SqlParameter("@docTypeId1", Constant.Law_DocTypeQanuan),
                    new SqlParameter("@docTypeId2", Constant.Law_DocTypeQanuanMarsum),
                    new SqlParameter("@docCategoryId", GetNullableInt(ddlCategory.SelectedValue)),
                    new SqlParameter("@docNum", GetNullableInt(txtDocNum.Text)),
                    new SqlParameter("@docSubject", GetNullableInt(txtDocSubject.Text)),
                    new SqlParameter("@docYear", docYearParam)).ToList();
                }
                else
                { 
                    return objRepository.DC.Database.SqlQuery<LawExtraDoc>(
                    query,
                    new SqlParameter("@sectorId", sectorId),
                    new SqlParameter("@docTypeId", GetNullableInt(hdnTypeId.Value)),
                    new SqlParameter("@docCategoryId", GetNullableInt(ddlCategory.SelectedValue)),
                    new SqlParameter("@docNum", GetNullableInt(txtDocNum.Text)),
                    new SqlParameter("@docSubject", GetNullableInt(txtDocSubject.Text)),
                    new SqlParameter("@docYear", docYearParam)).ToList();
            }

        
     }
            }

        protected void rblDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            FillSearchResults();
        }

        protected void RadioButtonRelatedList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            FillSearchResults();
        }
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

        protected void RButtonSortList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPageIndex = 0;
            FillSearchResults();
        }
    }
}
