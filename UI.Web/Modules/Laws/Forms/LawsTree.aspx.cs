using Infrastructure.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Linq.Dynamic;
using UI.Web.Admin.Controller;
using Infrastructure.DAL;
using Infrastructure;
using Newtonsoft.Json;
using static iTextSharp.text.pdf.PdfReader;

namespace UI.Web.Modules.Laws.Forms
{
    public partial class LawsTree : BaseFormAdmin
    {
        public CMGS_DBEntities DC = new CMGS_DBEntities();
        #region "Page Members"
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();
        public AgreementsRepository agreemtnyRepository = IoC.Resolve<AgreementsRepository>();
        public string _PageTitle = "نظام التشريعات  ";

        public bool isCancelled = false;


        public string _TargetUrl = System.Configuration.ConfigurationManager.AppSettings["ScanningModuleURL"].ToString();
        public string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LawsAttachments/";

        public string ScannerRepositoryViewer = System.Configuration.ConfigurationManager.AppSettings["ScannerRepositoryViewer"].ToString();
        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();

        public string selectedChapter = "0";
        public string QrelatedOrg = "";
        #endregion


        protected void Page_PreRender(object sender, EventArgs e)
        {
            applyUserPermission();

        }
        protected void Page_PreInit(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillLookups();
                BindGrid();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            dgLaws.CurrentPageIndex = 0;
            BindGrid();
        }

        private void fillLookups()
        {
            FillDllwithoptional_ALL(objLookup.FillLaw_DocType(), ref lstFilterType, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.FillLaw_DocZeroCategory(), ref lstFilterCategory, "NameAr", "Code", "الكل");

            FillDllwithoptional_ALL(objLookup.Filllaw_DocProceduresTypes(), ref lstfilterProceduretype, "NameAr", "Code", "الكل");
        }
        private void applyUserPermission()
        {
        }
        private string MapSearchKeys()
        {
            Dictionary<string, string> _keyList = new Dictionary<string, string>();
            try
            {
                _keyList.Add(" رقم الوثيقة ", txtFilterSerialNum.Text);
                _keyList.Add("   سنة الاصدار   ", txtFilterSerialYear.Text);
                _keyList.Add(" تاريخ  إصدار الوثيقة من ", txtFilterDatefrom.Text);
                _keyList.Add(" الي تاريخ   ", txtFilterDateTo.Text);
                _keyList.Add(" جزء من نص الوثيقة    ", txtFilterDetails.Text);
                _keyList.Add(" نوع الوثيقة  ", lstFilterType.SelectedItem.Text);
                _keyList.Add(" قيد الدراسة    ", lstFilterIsUnderStudy.SelectedItem.Text);
                _keyList.Add(lblFilterCatTitle.Text, lstFilterCategory.SelectedItem.Text);
                _keyList.Add("  جزء من الموضوع ", txtFilterSubject.Text);
                _keyList.Add("نشر بالجريدة الرسمية", lstFilterPublish.Text);

                _keyList.Add(" تاريخ  انتهاء الوثيقة من ", txtFilterExpireFrom.Text);
                _keyList.Add(" الي   تاريخ  انتهاء  ", txtFilterExpireTo.Text);

            }
            catch (Exception)
            {

                return "";
            }


            return JsonConvert.SerializeObject(_keyList);
        }

        //private void BindGrid(string sortExpression = null, string direction = "ASC")
        //{
        //    //string keyword = txtSearch.Text.Trim();

        //    var query = DC.View_LawsDocs
        //       .Where(v => v.isDeleted == false || v.isDeleted == null);

        //    //if (!string.IsNullOrEmpty(keyword))
        //    //{
        //        int SerialNum= ZeroIntergerIFNull(txtFilterSerialNum.Text); 
        //        int SerialYear = ZeroIntergerIFNull(txtFilterSerialYear.Text); 
        //        DateTime TransactionDatFrom = NullDateifEmpty(txtFilterDatefrom.Text); 
        //        DateTime TransactionDatTo = NullDateifEmpty(txtFilterDateTo.Text);
        //        int DocType = ZeroIntergerIFNull(lstFilterType.SelectedValue);
        //        int DocCategory = ZeroIntergerIFNull(lstFilterCategory.SelectedValue);
        //        int IsUnderStudy = ZeroIntergerIFNull(lstFilterIsUnderStudy.SelectedValue);
        //        string LawSubject = txtFilterSubject.Text;
        //        string LawDetails = txtFilterDetails.Text;
        //        int isPublished = ZeroIntergerIFNull(lstFilterPublish.SelectedValue);
        //        int lastProcedureId = ZeroIntergerIFNull(lstfilterProceduretype.SelectedValue);
        //        DateTime ExpireDatefrom = NullDateifEmpty(txtFilterExpireFrom.Text);
        //        DateTime ExpireDateTo = NullDateifEmpty(txtFilterExpireTo.Text);
        //        string SearchKyes = MapSearchKeys();

        //        query = from obj in DC.View_LawsDocs
        //        orderby obj.DocYear descending, obj.DocNum descending
        //        where true && ((SerialNum != 0) ? (obj.DocNum == (int?)SerialNum) : true)  && ((SerialYear != 0) ? (obj.DocYear == (int?)SerialYear) : true) && (((TransactionDatFrom != new DateTime(1990, 1, 1)) ? (obj.DocDate >= TransactionDatFrom) : true) || ((TransactionDatFrom != new DateTime(1990, 1, 1)) ? (obj.DocDate >= TransactionDatFrom) : true)) && (((TransactionDatTo != new DateTime(1990, 1, 1)) ? (obj.DocDate <= TransactionDatTo) : true) || ((TransactionDatTo != new DateTime(1990, 1, 1)) ? (obj.DocDate <= TransactionDatTo) : true)) && (((ExpireDatefrom != new DateTime(1990, 1, 1)) ? (obj.ExpireDate >= ExpireDatefrom) : true) || ((ExpireDatefrom != new DateTime(1990, 1, 1)) ? (obj.ExpireDate >= ExpireDatefrom) : true)) && (((ExpireDateTo != new DateTime(1990, 1, 1)) ? (obj.ExpireDate <= ExpireDateTo) : true) || ((ExpireDateTo != new DateTime(1990, 1, 1)) ? (obj.ExpireDate <= ExpireDateTo) : true)) && ((DocType != 0) ? (obj.DocTypeID == (int?)DocType) : (obj.DocTypeID != (int?)5 && obj.DocTypeID != (int?)6 && obj.DocTypeID != (int?)7)) && ((IsUnderStudy != 0) ? ((IsUnderStudy == 1) ? (obj.UnderStudy == (bool?)true) : (obj.UnderStudy == (bool?)false)) : true) && ((isPublished != 0) ? ((isPublished == 1) ? (obj.isPublished == (bool?)true) : (obj.isPublished == (bool?)false)) : true) && ((DocCategory != 0) ? (obj.DocCategoryID == (int?)DocCategory) : true) && ((lastProcedureId != 0) ? (obj.ProcedureTypeCode == (int?)lastProcedureId) : true) && ((LawSubject != "") ? obj.DocSubject.Contains(LawSubject) : true) && ((LawDetails != "") ? obj.DocDetails.Contains(LawDetails) : true)
        //        select obj;
        //       // query = query.Where(v => v.DocSubject.Contains(keyword) || v.Code.ToString().Contains(keyword) || v.DocYear.ToString().Contains(keyword));

        //       //var query = objRepository.GetList(ZeroIntergerIFNull(txtFilterSerialNum.Text), ZeroIntergerIFNull(txtFilterSerialYear.Text),
        //       //  NullDateifEmpty(txtFilterDatefrom.Text),
        //       // NullDateifEmpty(txtFilterDateTo.Text), ZeroIntergerIFNull(lstFilterType.SelectedValue), ZeroIntergerIFNull(lstFilterCategory.SelectedValue),
        //       //ZeroIntergerIFNull(lstFilterIsUnderStudy.SelectedValue), txtFilterSubject.Text, txtFilterDetails.Text,
        //       //ZeroIntergerIFNull(lstFilterPublish.SelectedValue), ZeroIntergerIFNull(lstfilterProceduretype.SelectedValue), NullDateifEmpty(txtFilterExpireFrom.Text), NullDateifEmpty(txtFilterExpireTo.Text), MapSearchKeys());
        //   // }

        //    var laws = query.Select(v => new LawRow
        //    {
        //        Code = v.Code,
        //        DocSubject = v.DocSubject,
        //        DocYear = v.DocYear,
        //        Law_DocTypeNameAr = v.Law_DocTypeNameAr,
        //        Law_DocCategoryNameAr = v.Law_DocCategoryNameAr,
        //        Level = 0,
        //        ParentCode = null,
        //        IsExpanded = false
        //    }).ToList();

        //    // Sorting
        //    if (!string.IsNullOrEmpty(sortExpression))
        //    {
        //        switch (sortExpression)
        //        {
        //            case "DocSubject":
        //                laws = direction == "ASC" ? laws.OrderBy(x => x.DocSubject).ToList() : laws.OrderByDescending(x => x.DocSubject).ToList();
        //                break;
        //            case "DocYear":
        //                laws = direction == "ASC" ? laws.OrderBy(x => x.DocYear).ToList() : laws.OrderByDescending(x => x.DocYear).ToList();
        //                break;
        //            case "Law_DocTypeNameAr":
        //                laws = direction == "ASC" ? laws.OrderBy(x => x.Law_DocTypeNameAr).ToList() : laws.OrderByDescending(x => x.Law_DocTypeNameAr).ToList();
        //                break;
        //            case "Law_DocCategoryNameAr":
        //                laws = direction == "ASC" ? laws.OrderBy(x => x.Law_DocCategoryNameAr).ToList() : laws.OrderByDescending(x => x.Law_DocCategoryNameAr).ToList();
        //                break;
        //        }
        //    }

        //    ViewState["AllLaws"] = laws;
        //    dgLaws.DataSource = laws;
        //    dgLaws.DataBind();
        //}
        //protected void dgLaws_ItemDataBound(object sender, DataGridItemEventArgs e)
        //{
        //    if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
        //        return;

        //    var data = (LawRow)e.Item.DataItem;

        //    // ✅ Apply style to <td> cells if it's a child row
        //    if (data.Level > 0)
        //    {
        //        foreach (TableCell cell in e.Item.Cells)
        //        {
        //            cell.CssClass = $"child-row level-{data.Level}";
        //        }
        //    }

        //    Button btnExpand = (Button)e.Item.FindControl("btnExpand");
        //    if (btnExpand != null)
        //    {
        //        int code = data.Code;

        //        bool hasChildren = DC.Law_DocData_Linked.Any(l => l.SouceDocID == code);
        //        btnExpand.Visible = hasChildren;

        //        // ✅ Change button text based on IsExpanded property
        //        if (data.IsExpanded)
        //        {
        //            btnExpand.Text = "–"; // or "_"
        //            btnExpand.CssClass = "btn btn-sm btn-outline-danger";
        //        }
        //        else
        //        {
        //            btnExpand.Text = "+";
        //            btnExpand.CssClass = "btn btn-sm btn-outline-secondary";
        //        }
        //    }
        //    //int code = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Code"));

        //    //bool hasChildren = DC.Law_DocData_Linked.Any(l => l.SouceDocID == code);
        //    //if (!hasChildren && btnExpand != null)
        //    //    btnExpand.Visible = false;
        //}
        private List<int> ExpandedRows
        {
            get
            {
                if (ViewState["ExpandedRows"] == null)
                    ViewState["ExpandedRows"] = new List<int>();
                return (List<int>)ViewState["ExpandedRows"];
            }
            set
            {
                ViewState["ExpandedRows"] = value;
            }
        }
     


      


        //protected void dgLaws_ItemCommand(object source, DataGridCommandEventArgs e)
        //{
        //    if (e.CommandName != "Expand") return;

        //    int code = Convert.ToInt32(e.CommandArgument);
        //    List<LawRow> all = (List<LawRow>)ViewState["AllLaws"];

        //    var parentRow = all.FirstOrDefault(r => r.Code == code);
        //    if (parentRow == null) return;

        //    if (parentRow.IsExpanded)
        //        CollapseRows(all, parentRow);
        //    else
        //        ExpandRows(all, parentRow);

        //    parentRow.IsExpanded = !parentRow.IsExpanded;

        //    dgLaws.DataSource = all.Where(r => r.Level == 0 || r.ParentCode != null).ToList();
        //    dgLaws.DataBind();
        //}

    
        protected void lstFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {



            if (lstFilterType.SelectedValue == "3")//مرسوم
            {
                lblFilterCatTitle.Text = "تصنيف المرسوم:";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(ZeroIntergerIFNull(lstFilterType.SelectedValue)), ref lstFilterCategory, "NameAr", "Code", "الكل");

                lblFilterCatTitle.Visible = true;
                lstFilterCategory.Visible = true;

            }
            else if (lstFilterType.SelectedValue == "4")//امر اميرس
            {
                lblFilterCatTitle.Text = "تصنيف الأمر الأميري:";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocCategory(ZeroIntergerIFNull(lstFilterType.SelectedValue)), ref lstFilterCategory, "NameAr", "Code", "الكل");

                lblFilterCatTitle.Visible = false;
                lstFilterCategory.Visible = false;

            }
            else
            {
                lblFilterCatTitle.Text = "التصنيف: ";
                FillDllwithoptional_ALL(objLookup.FillLaw_DocZeroCategory(), ref lstFilterCategory, "NameAr", "Code", "الكل");

                lblFilterCatTitle.Visible = true;
                lstFilterCategory.Visible = true;

            }



        }

        // -------------------- BindGrid --------------------
        private void BindGrid(string sortExpression = null, string direction = "ASC")
        {
            bool onlyWithRelated = chkHasRelated.Checked;

            var query = DC.View_LawsDocs.Where(v => v.isDeleted == false || v.isDeleted == null);

            // --- Apply filters (example, replace with your controls) ---
            int SerialNum = ZeroIntergerIFNull(txtFilterSerialNum.Text);
            int SerialYear = ZeroIntergerIFNull(txtFilterSerialYear.Text);
            DateTime TransactionDatFrom = NullDateifEmpty(txtFilterDatefrom.Text);
            DateTime TransactionDatTo = NullDateifEmpty(txtFilterDateTo.Text);
            int DocType = ZeroIntergerIFNull(lstFilterType.SelectedValue);
            int DocCategory = ZeroIntergerIFNull(lstFilterCategory.SelectedValue);
            int IsUnderStudy = ZeroIntergerIFNull(lstFilterIsUnderStudy.SelectedValue);
            string LawSubject = txtFilterSubject.Text;
            string LawDetails = txtFilterDetails.Text;
            int isPublished = ZeroIntergerIFNull(lstFilterPublish.SelectedValue);
            int lastProcedureId = ZeroIntergerIFNull(lstfilterProceduretype.SelectedValue);
            DateTime ExpireDatefrom = NullDateifEmpty(txtFilterExpireFrom.Text);
            DateTime ExpireDateTo = NullDateifEmpty(txtFilterExpireTo.Text);

            query = from obj in query
                    where
                        ((SerialNum != 0) ? obj.DocNum == SerialNum : true) &&
                        ((SerialYear != 0) ? obj.DocYear == SerialYear : true) &&
                        ((TransactionDatFrom != new DateTime(1990, 1, 1)) ? obj.DocDate >= TransactionDatFrom : true) &&
                        ((TransactionDatTo != new DateTime(1990, 1, 1)) ? obj.DocDate <= TransactionDatTo : true) &&
                        ((ExpireDatefrom != new DateTime(1990, 1, 1)) ? obj.ExpireDate >= ExpireDatefrom : true) &&
                        ((ExpireDateTo != new DateTime(1990, 1, 1)) ? obj.ExpireDate <= ExpireDateTo : true) &&
                        ((DocType != 0) ? obj.DocTypeID == DocType : true) &&
                        ((DocCategory != 0) ? obj.DocCategoryID == DocCategory : true) &&
                        ((IsUnderStudy != 0) ? ((IsUnderStudy == 1) ? obj.UnderStudy == true : obj.UnderStudy == false) : true) &&
                        ((isPublished != 0) ? ((isPublished == 1) ? obj.isPublished == true : obj.isPublished == false) : true) &&
                        ((lastProcedureId != 0) ? obj.ProcedureTypeCode == lastProcedureId : true) &&
                        ((LawSubject != "") ? obj.DocSubject.Contains(LawSubject) : true) &&
                        ((LawDetails != "") ? obj.DocDetails.Contains(LawDetails) : true) &&
                        (!onlyWithRelated || DC.Law_DocData_Linked.Any(l => l.SouceDocID == obj.Code))
                    select obj;

            var baseLaws = query.Select(v => new LawRow
            {
                Code = v.Code,
                DocNum = v.DocNum,
                DocFilepath = v.DocFilepath,
                relatedAgreement = v.relatedAgreement,
                DocSubject = v.DocSubject,
                DocYear = v.DocYear,
                Law_DocTypeNameAr = v.Law_DocTypeNameAr,
                Law_DocCategoryNameAr = v.Law_DocCategoryNameAr,
                Level = 0,
                ParentCode = null,
                IsExpanded = false,
                IsChild = false,
                IsVisible = true
            }).OrderByDescending(o=> o.DocYear).ToList();

            // Sorting
            if (!string.IsNullOrEmpty(sortExpression))
            {
                switch (sortExpression)
                {
                    case "DocSubject":
                        baseLaws = (direction == "ASC") ? baseLaws.OrderBy(x => x.DocSubject).ToList() : baseLaws.OrderByDescending(x => x.DocSubject).ToList();
                        break;
                    case "DocYear":
                        baseLaws = (direction == "ASC") ? baseLaws.OrderBy(x => x.DocYear).ToList() : baseLaws.OrderByDescending(x => x.DocYear).ToList();
                        break;
                    case "DocNum":
                        baseLaws = (direction == "ASC") ? baseLaws.OrderBy(x => x.DocNum).ToList() : baseLaws.OrderByDescending(x => x.DocNum).ToList();
                        break;
                    case "Law_DocTypeNameAr":
                        baseLaws = (direction == "ASC") ? baseLaws.OrderBy(x => x.Law_DocTypeNameAr).ToList() : baseLaws.OrderByDescending(x => x.Law_DocTypeNameAr).ToList();
                        break;
                    case "Law_DocCategoryNameAr":
                        baseLaws = (direction == "ASC") ? baseLaws.OrderBy(x => x.Law_DocCategoryNameAr).ToList() : baseLaws.OrderByDescending(x => x.Law_DocCategoryNameAr).ToList();
                        break;
                }
            }
            ViewState["AllLaws"] = baseLaws;

            dgLaws.VirtualItemCount = baseLaws.Count; // total top-level items for pager
            dgLaws.CurrentPageIndex = 0; // first page
            dgLaws.DataSource = baseLaws; // bind all top-level rows
            dgLaws.DataBind();
        }
        public string showattachment(string hasattachment)
        {
            if (hasattachment != "")
            {
                return "";
            }
            return "display:none";

        }
        public string viewlinkedfile(string filename)
        {
            return gets(filename).Equals("") ? "none" : "";
        }
        // -------------------- Paging --------------------
        //private void BindGridPage(int pageIndex)
        //{
        //    int pageSize = dgLaws.PageSize;
        //    var all = ViewState["AllLaws"] as List<LawRow>;
        //    if (all == null) return;

        //    var visibleRows = all.Where(r => r.Level == 0 || r.IsChild && r.IsVisible).ToList();

        //    dgLaws.CurrentPageIndex = pageIndex;
        //    dgLaws.DataSource = visibleRows.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        //    dgLaws.DataBind();
        //}
        private void BindGridPage(int pageIndex)
        {
            int pageSize = dgLaws.PageSize;
            var all = ViewState["AllLaws"] as List<LawRow>;
            if (all == null) return;

            // Only top-level items for paging
            var topLevel = all.Where(r => r.Level == 0).ToList();


            var pageRows = topLevel.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            dgLaws.CurrentPageIndex = pageIndex;
            dgLaws.DataSource = pageRows;
            dgLaws.DataBind();
        }



        // Helper to check if all parents are expanded
        private bool IsParentExpanded(LawRow child, List<LawRow> all)
        {
            var parent = all.FirstOrDefault(r => r.Code == child.ParentCode);
            if (parent == null) return true;
            if (!parent.IsExpanded) return false;
            if (parent.Level == 0) return true;
            return IsParentExpanded(parent, all); // recursive check
        }

        protected void dgLaws_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgLaws.CurrentPageIndex = e.NewPageIndex;

            var allLaws = ViewState["AllLaws"] as List<LawRow>;
            if (allLaws == null) return;

            dgLaws.DataSource = allLaws; // bind all top-level rows
            dgLaws.DataBind();
        }

        // -------------------- Expand / Collapse --------------------
        private void ExpandRows(List<LawRow> all, LawRow parent)
        {
            parent.IsHighlighted = true;   // ⭐ Highlight parent row

            var children = (from link in DC.Law_DocData_Linked
                            join v in DC.View_LawsDocs on link.DestDocId equals v.Code
                            where link.SouceDocID == parent.Code
                            select new LawRow
                            {
                                Code = v.Code,
                                DocNum = v.DocNum,
                                DocFilepath = v.DocFilepath,
                                relatedAgreement = v.relatedAgreement,
                                DocSubject = v.DocSubject,
                                DocYear = v.DocYear,
                                Law_DocTypeNameAr = v.Law_DocTypeNameAr,
                                Law_DocCategoryNameAr = v.Law_DocCategoryNameAr,
                                Level = parent.Level + 1,
                                ParentCode = parent.Code,
                                IsExpanded = false,
                                IsChild = true,
                                IsVisible = true,

                            }).OrderByDescending(o => o.DocYear).ToList();

            int index = all.IndexOf(parent) + 1;
            all.InsertRange(index, children);
        }

        private void CollapseRows(List<LawRow> all, LawRow parent)
        {
            var children = all.Where(r => r.ParentCode == parent.Code).ToList();

            foreach (var child in children)
            {
                if (child.IsExpanded)
                {
                    child.IsExpanded = false;
                    CollapseRows(all, child);
                }
                all.Remove(child);
            }
        }

        // -------------------- ItemCommand --------------------
        protected void dgLaws_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName != "Expand")
            {
                ViewState["ParentCode"] = "";
                return;
            }

            int code = Convert.ToInt32(e.CommandArgument);
            var all = ViewState["AllLaws"] as List<LawRow>;
            if (all == null) return;

            var parent = all.FirstOrDefault(r => r.Code == code);
            if (parent == null) return;

            if (parent.IsExpanded)
            {
                CollapseRows(all, parent);
                parent.IsExpanded = false;
                parent.IsHighlighted = false; // إزالة اللون عند Collapse
            }
            else
            {
                ExpandRows(all, parent);
                parent.IsExpanded = true;
                parent.IsHighlighted = true; // ⭐ اللون الأحمر عند Expand

            }

            // Update visibility: show top-level + expanded children
            foreach (var row in all)
            {
                row.IsVisible = row.Level == 0 || row.ParentCode == null || (row.ParentCode != null && all.Any(p => p.Code == row.ParentCode && p.IsExpanded));
            }

            ViewState["AllLaws"] = all;

            // ✅ Bind all top-level rows; DataGrid will handle paging
            dgLaws.DataSource = all.Where(r => r.Level == 0 || r.IsVisible).ToList();
            dgLaws.DataBind();
        }


        // -------------------- ItemDataBound --------------------
        protected void dgLaws_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            // Only process data rows
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            // Cast safely
            var data = e.Item.DataItem as LawRow;
        

            //if (data.IsHighlighted)
            //    e.Item.BackColor = System.Drawing.Color.Red;
            //else
            //    e.Item.BackColor = System.Drawing.Color.White;

            // ------------------ Expand/Collapse Button ------------------
            Button btnExpand = (Button)e.Item.FindControl("btnExpand");
            if (btnExpand != null)
            {
                // Check if this law has children
                bool hasChildren = DC.Law_DocData_Linked.Any(l => l.SouceDocID == data.Code);
                btnExpand.Visible = hasChildren;

                if (data.IsExpanded)
                {
                    if(data.Level==0)
                        e.Item.Attributes["style"] = "background-color: #d6c26e !important;";

                    btnExpand.Text = "–"; // Collapse sign
                    btnExpand.CssClass = "btn btn-sm btn-outline-danger";
                }
                else
                {
                    btnExpand.Text = "+";
                    btnExpand.CssClass = "btn btn-sm btn-outline-secondary btn-expand";
                }
            }

            // ------------------ Apply CSS Class for Level ------------------
            e.Item.CssClass = "child-row level-" + data.Level;

            // ------------------ Apply Padding Right per Level ------------------
            int padding = data.Level+1 * 20; // increase per level
            foreach (TableCell cell in e.Item.Cells)
            {
                cell.Style["padding-right"] = padding + "px";
            }

            // ------------------ Fix Paging by marking visible rows ------------------
            // This ensures the DataGrid calculates total rows correctly
            if (data.Level == 0)
            {
                data.IsVisible = true; // top-level always visible
            }
            else
            {
                // child row is visible only if its parent is expanded
                var all = ViewState["AllLaws"] as List<LawRow>;
                if (all != null && data.ParentCode.HasValue)
                {
                    var parent = all.FirstOrDefault(r => r.Code == data.ParentCode.Value);
                    data.IsVisible = parent != null && parent.IsExpanded && parent.IsVisible;
                }
            }
        }


        protected void dgLaws_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string direction = "ASC";

            var all = ViewState["AllLaws"] as List<LawRow>;
            if (all == null) return;

            // Toggle direction
            if (ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() == sortExpression)
            {
                direction = ViewState["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";
            }

            ViewState["SortExpression"] = sortExpression;
            ViewState["SortDirection"] = direction;

            BindGrid(sortExpression, direction);
        }
    }

    [Serializable]
    public class LawRow
    {
        public int Code { get; set; }
        public int? DocNum { get; set; }
        public string DocSubject { get; set; }
        public int? DocYear { get; set; }
        public string Law_DocTypeNameAr { get; set; }
        public string Law_DocCategoryNameAr { get; set; }
        public string DocFilepath { get; set; }
        public int? relatedAgreement { get; set; }

        public int Level { get; set; } = 0;
        public int? ParentCode { get; set; } = null;

        public bool IsExpanded { get; set; } = false;
        public bool IsChild { get; set; } = false;
        public bool IsVisible { get; set; } = true;
        public bool IsHighlighted { get; set; }

    }
    //public class LawRow
    //{
    //    public int Code { get; set; }
    //    public string DocSubject { get; set; }
    //    public int? DocYear { get; set; }
    //    public string Law_DocTypeNameAr { get; set; }
    //    public string Law_DocCategoryNameAr { get; set; }

    //    public int Level { get; set; } = 0;
    //    public int? ParentCode { get; set; } = null;

    //    public bool IsExpanded { get; set; } = false;
    //    public bool IsChild { get; set; } = false;
    //}


}
