using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;
using System.Collections;
using UI.Web.Admin.Controller;
using Infrastructure.DAL;

using Microsoft.VisualBasic;
using System.Resources;
using Infrastructure;

namespace UI.Web.Modules.MasterData
{
    public partial class lookups : BaseFormAdmin
    {
         public string _PageTitle = "";
        public Boolean editflag = false;
        public string TargetTableName { get; set; }
        public LookupMaster objLookup = IoC.Resolve<LookupMaster>();

        [Serializable]
        private class FilterColumnInfo
        {
            public string ColumnName { get; set; }
            public bool IsForeignKey { get; set; }
            public string ParentSchema { get; set; }
            public string ParentTable { get; set; }
            public string ParentColumn { get; set; }
            public string DisplayColumn { get; set; }
        }

        private class LookupOption
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }

        private class LookupGridModel
        {
            public int Code { get; set; }
            public string NameEn { get; set; }
            public string NameAr { get; set; }
            public string TypeNameAr { get; set; }
        }

        private class LawDocCategoryValues
        {
            public int? TypeID { get; set; }
        }

        private class SectorValues
        {
            public string imgPath { get; set; }
        }

        private class Sectors
        {
            public int Code { get; set; }
            public string NameAr { get; set; }
            public string NameEn { get; set; }
            public string imgPath { get; set; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (btnNew != null)
            {
                btnNew.Click += btnNew_Click;
            }
            if (!string.IsNullOrWhiteSpace(TargetTableName))
            {
                BindFilterControls();
            }
        }
       
        protected void Page_PreInit(object sender, EventArgs e)
        {
            string Script = "";
            if (Request.QueryString["tableName"] != null)
            {
                if (Request.QueryString["tableName"] != "")
                {
                    TargetTableName = Request.QueryString["tableName"].ToString();

                    //ResourceManager rm = new ResourceManager(typeof(Resources.lockups));
                    //string someString =
                    //_PageTitle = (String)GetGlobalResourceObject(
                    // "lockups", TargetTableName); // Resources.Utilities.TargetTableName;
                    _PageTitle = (string)GetGlobalResourceObject("lockups", TargetTableName);
                    //  _PageTitle = "البيانات الاساسيه";
                }
                else
                {
                    Script = FormatpopupErrorMSG("Fail To Load Table Data, TableName Paramter", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                    lblerror.ForeColor = System.Drawing.Color.Red;
                    return;
                }

            }
            else
            {
                Script = FormatpopupErrorMSG("Fail To Load Table Data", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                lblerror.ForeColor = System.Drawing.Color.Red;
                return;
            }

        }

        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";
            if (btnCancel != null)
            {
                btnCancel.Attributes.Add("onclick", "Page_ValidationActive=false;");
            }
            if (btnSave != null)
            {
                btnSave.Attributes.Add("onclick", "return chkImage();"); 
            }
            if (!IsPostBack)
            {
                //if ((Request.UrlReferrer == null))
                //{
                //    Response.Redirect("/admin/pages/main.aspx");
                //}

                ViewState["Item"] = 0;
                ConfigureLawDocCategoryFields();
                FillGrid();
            }

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

            applyUserPermission();
            if(IsLawDocCategory()|| IsCommittees_RefList())
                ApplyLawDocCategoryGridColumns();

        }

        private void ConfigureLawDocCategoryFields()
        {
            var isLawDocCategory = IsLawDocCategory();
            var isCommittees_RefList = IsCommittees_RefList();
            if (divTypeId != null)
            {
                if (isLawDocCategory || isCommittees_RefList)
                {
                    divTypeId.Visible = true;
                }
                
                else
                {
                    divTypeId.Visible = false;

                }

            }


            if (isLawDocCategory)
            {
                BindLawDocCategoryLookups();
            }
            else if (isCommittees_RefList)
            {
                BindCommittees_RefListLookups();
            }

                ConfigureSectorFields();
        }

        private void ConfigureSectorFields()
        {
            if (divSectorImage != null)
            {
                divSectorImage.Visible = IsSectorsTable();
            }
        }

        private void ApplyLawDocCategoryGridColumns()
        {
            var isLawDocCategory = IsLawDocCategory();
            if (grdData != null && grdData.Columns.Count > 4)
            {
                if (isLawDocCategory || IsCommittees_RefList())
                {
                    grdData.Columns[2].Visible = true;

                    grdData.Columns[3].Visible = true;
                }
                else
                {
                    grdData.Columns[2].Visible = false;

                    grdData.Columns[3].Visible = false;
                }
               
            }
        }

        private void BindLawDocCategoryLookups()
        {
            if (ddlTypeId != null)
            {
                ddlTypeId.Items.Clear();
                ddlTypeId.Items.Add(new ListItem("الكل", ""));
                BindLookupItems(ddlTypeId, "Law_DocType", "Code", "NameAr");
            }
        }

        private void BindCommittees_RefListLookups()
        {
            if (ddlTypeId != null)
            {
                ddlTypeId.Items.Clear();
                ddlTypeId.Items.Add(new ListItem("الكل", ""));
                BindLookupItems(ddlTypeId, "Committees_Types", "Code", "NameAr");
            }
        }

        private void BindLookupItems(DropDownList ddl, string tableName, string valueColumn, string textColumn)
        {
            var query = "SELECT CONVERT(NVARCHAR(4000), [" + valueColumn + "]) AS Value, CONVERT(NVARCHAR(4000), [" + textColumn + "]) AS Text FROM " + GetFullTableName("dbo", tableName) + " ORDER BY [" + textColumn + "]";
            var items = objLookup.DC.Database.SqlQuery<LookupOption>(query).ToList();
            foreach (var item in items)
            {
                ddl.Items.Add(new ListItem(item.Text, item.Value));
            }
        }

        private bool IsLawDocCategory()
        {
            return string.Equals(NormalizeTableName(TargetTableName), "Law_DocCategory", StringComparison.OrdinalIgnoreCase);
        }
        private bool IsCommittees_RefList()
        {
            return string.Equals(NormalizeTableName(TargetTableName), "Committees_RefList", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsSectorsTable()
        {
            return string.Equals(NormalizeTableName(TargetTableName), "Sectors", StringComparison.OrdinalIgnoreCase);
        }

        private string NormalizeTableName(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return string.Empty;
            }

            var trimmed = tableName.Trim('[', ']');
            if (trimmed.Contains("."))
            {
                var parts = trimmed.Split('.');
                return parts[parts.Length - 1].Trim('[', ']');
            }

            return trimmed;
        }

        private void GetSchemaAndTable(string tableName, out string schemaName, out string normalizedTable)
        {
            schemaName = "dbo";
            normalizedTable = tableName;

            if (!string.IsNullOrWhiteSpace(tableName) && tableName.Contains("."))
            {
                var parts = tableName.Split('.');
                if (parts.Length >= 2)
                {
                    schemaName = parts[0].Trim('[', ']');
                    normalizedTable = parts[1].Trim('[', ']');
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(tableName))
            {
                normalizedTable = tableName.Trim('[', ']');
            }
        }
        private void FillGrid()
        {
            var masterList = GetFilteredItems();
            lblcount.Text = (Resources.Utilities.foundTotal + (masterList.Count.ToString() + Resources.Utilities.records));
            decimal c = System.Math.Floor(Convert.ToDecimal(masterList.Count / grdData.PageSize));
            if ((c < grdData.CurrentPageIndex))
            {
                grdData.CurrentPageIndex = 0;
            }

            grdData.DataSource = masterList;
            grdData.DataBind();
            int _totalCount = masterList.Count;
           // pager1.ItemCount = _totalCount;

        }

        private void BindFilterControls()
        {
            if (phFilters == null)
            {
                return;
            }

            var columns = GetFilterColumnInfos();
            phFilters.Controls.Clear();

            foreach (var column in columns)
            {
                string ColName = column.ColumnName;
                if (ColName == "TypeID")
                    ColName = "النوع";
                else if (ColName == "NameAr")
                    ColName = "الاسم بالعربيه";
                //else if (ColName == "OrgType")
                //    ColName = "الاسم بالعربيه";

                if (column.ColumnName != "imgPath" && ColName != "OrgType" && ColName != "CatID")
                    phFilters.Controls.Add(new LiteralControl("<div class=\"col-md-3\"><div class=\"form-group\"><span>" + ColName + ":</span>"));

                Control control;
                if (column.IsForeignKey)
                {
                    var ddl = new DropDownList
                    {
                        ID = GetFilterControlId(column.ColumnName),
                        CssClass = "table-group-action-select form-control input-inline",
                        AppendDataBoundItems = true
                    };
                    ddl.Items.Add(new ListItem("الكل", ""));
                    BindForeignKeyValues(ddl, column);
                    control = ddl;
                }
                else
                {
                    control = new TextBox
                    {
                        ID = GetFilterControlId(column.ColumnName),
                        CssClass = "table-group-action-select form-control input-inline"
                    };
                }
                if (column.ColumnName != "imgPath" && ColName != "OrgType" && ColName != "CatID")
                {
                    phFilters.Controls.Add(control);
                    phFilters.Controls.Add(new LiteralControl("</div></div>"));
                }


            }
        }

        private List<FilterColumnInfo> GetFilterColumnInfos()
        {
            return LoadFilterColumnInfos();
        }

        private List<FilterColumnInfo> LoadFilterColumnInfos()
        {
            var tableName = TargetTableName;
            var schemaName = "dbo";

            if (!string.IsNullOrWhiteSpace(TargetTableName) && TargetTableName.Contains("."))
            {
                var parts = TargetTableName.Split('.');
                if (parts.Length >= 2)
                {
                    schemaName = parts[0].Trim('[', ']');
                    tableName = parts[1].Trim('[', ']');
                }
            }
            else if (!string.IsNullOrWhiteSpace(TargetTableName))
            {
                tableName = TargetTableName.Trim('[', ']');
            }

            const string query = "SELECT c.COLUMN_NAME AS ColumnName, fk.REF_TABLE_SCHEMA AS ParentSchema, fk.REF_TABLE_NAME AS ParentTable, fk.REF_COLUMN_NAME AS ParentColumn " +
                                 "FROM INFORMATION_SCHEMA.COLUMNS c " +
                                 "LEFT JOIN (" +
                                 "    SELECT kcu.COLUMN_NAME, ccu.TABLE_SCHEMA AS REF_TABLE_SCHEMA, ccu.TABLE_NAME AS REF_TABLE_NAME, ccu.COLUMN_NAME AS REF_COLUMN_NAME " +
                                 "    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc " +
                                 "    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu ON rc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME " +
                                 "    INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu ON rc.UNIQUE_CONSTRAINT_NAME = ccu.CONSTRAINT_NAME " +
                                 "    WHERE kcu.TABLE_SCHEMA = @schemaName AND kcu.TABLE_NAME = @tableName" +
                                 ") fk ON c.COLUMN_NAME = fk.COLUMN_NAME " +
                                 "WHERE c.TABLE_SCHEMA = @schemaName AND c.TABLE_NAME = @tableName " +
                                 "ORDER BY c.ORDINAL_POSITION";

            var result = objLookup.DC.Database.SqlQuery<FilterColumnInfo>(query,
                new SqlParameter("@schemaName", schemaName),
                new SqlParameter("@tableName", tableName)).ToList();

            foreach (var column in result)
            {
                column.IsForeignKey = !string.IsNullOrWhiteSpace(column.ParentTable);
                if (column.IsForeignKey)
                {
                    column.DisplayColumn = GetParentDisplayColumn(column);
                }
            }

            return result.Where(column => !string.Equals(column.ColumnName, "code", StringComparison.OrdinalIgnoreCase) && !string.Equals(column.ColumnName, "NameEn", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private string GetParentDisplayColumn(FilterColumnInfo column)
        {
            if (string.IsNullOrWhiteSpace(column.ParentTable))
            {
                return column.ParentColumn;
            }

            if (column.ParentTable.Equals("Law_DocType", StringComparison.OrdinalIgnoreCase))
            {
                return "NameAr";
            }

            const string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = @schemaName AND TABLE_NAME = @tableName ORDER BY ORDINAL_POSITION";
            var columns = objLookup.DC.Database.SqlQuery<string>(query,
                new SqlParameter("@schemaName", column.ParentSchema),
                new SqlParameter("@tableName", column.ParentTable)).ToList();

            var nameAr = columns.FirstOrDefault(item => item.Equals("NameAr", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(nameAr))
            {
                return nameAr;
            }

            var nameEn = columns.FirstOrDefault(item => item.Equals("NameEn", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(nameEn))
            {
                return nameEn;
            }

            var first = columns.FirstOrDefault(item => !item.Equals("code", StringComparison.OrdinalIgnoreCase));
            return string.IsNullOrWhiteSpace(first) ? column.ParentColumn : first;
        }

        private void BindForeignKeyValues(DropDownList ddl, FilterColumnInfo column)
        {
            if (ddl == null || string.IsNullOrWhiteSpace(column.ParentTable) || string.IsNullOrWhiteSpace(column.ParentColumn))
            {
                return;
            }

            var displayColumn = column.DisplayColumn ?? column.ParentColumn;
            var safeValueColumn = GetSafeColumnName(column.ParentColumn);
            var safeDisplayColumn = GetSafeColumnName(displayColumn);
            var parentTable = GetFullTableName(column.ParentSchema, column.ParentTable);

            var query = "SELECT CONVERT(NVARCHAR(4000), " + safeValueColumn + ") AS Value, CONVERT(NVARCHAR(4000), " + safeDisplayColumn + ") AS Text FROM " + parentTable +
                        " ORDER BY " + safeDisplayColumn;
            var items = objLookup.DC.Database.SqlQuery<LookupOption>(query).ToList();

            foreach (var item in items)
            {
                ddl.Items.Add(new ListItem(item.Text, item.Value));
            }
        }

        private List<LookupGridModel> GetFilteredItems()
        {
            var columns = GetFilterColumnInfos();
            string schemaName;
            string tableName;
            GetSchemaAndTable(TargetTableName, out schemaName, out tableName);
            var isLawDocCategory = IsLawDocCategory();
            var isCommittees_RefList = IsCommittees_RefList();

            var baseTable = GetFullTableName(schemaName, tableName);
            var query = isLawDocCategory
                ? "SELECT c.*, dt.NameAr AS TypeNameAr FROM " + baseTable + " c LEFT JOIN " + GetFullTableName(schemaName, "Law_DocType") + " dt ON c.TypeID = dt.Code  WHERE 1=1": isCommittees_RefList?
                "SELECT c.*, dt.NameAr AS TypeNameAr FROM " + baseTable + " c LEFT JOIN " + GetFullTableName(schemaName, "Committees_Types") + " dt ON c.TypeID = dt.Code  WHERE 1=1"
                : "SELECT * FROM " + TargetTableName + " WHERE 1=1";

            var parameters = new List<SqlParameter>();
            var index = 0;

            foreach (var column in columns)
            {
                var controlId = GetFilterControlId(column.ColumnName);
                var safeColumn = isLawDocCategory
                    ? "c." + GetSafeColumnName(column.ColumnName)
                    : GetSafeColumnName(column.ColumnName);

                if (column.IsForeignKey)
                {
                    var ddl = phFilters.FindControl(controlId) as DropDownList;
                    if (ddl != null && !string.IsNullOrWhiteSpace(ddl.SelectedValue))
                    {
                        var paramName = "@p" + index++;
                        query += " AND " + safeColumn + " = " + paramName;
                        parameters.Add(new SqlParameter(paramName, ddl.SelectedValue));
                    }
                }
                else
                {
                    var txt = phFilters.FindControl(controlId) as TextBox;
                    if (txt != null && !string.IsNullOrWhiteSpace(txt.Text))
                    {
                        var paramName = "@p" + index++;
                        query += " AND CONVERT(NVARCHAR(4000), " + safeColumn + ") LIKE N'%' + " + paramName + " + '%'";
                        parameters.Add(new SqlParameter(paramName, txt.Text));
                    }
                }
            }

            var result = objLookup.DC.Database.SqlQuery<LookupGridModel>(query, parameters.ToArray());
            return result.ToList();
        }

        private string GetFilterControlId(string columnName)
        {
            return "flt_" + columnName;
        }

        private string GetSafeColumnName(string columnName)
        {
            var safe = columnName.Replace("]", "]]");
            return "[" + safe + "]";
        }

        private string GetFullTableName(string schema, string table)
        {
            var safeSchema = string.IsNullOrWhiteSpace(schema) ? "dbo" : schema.Replace("]", "]]");
            var safeTable = table.Replace("]", "]]");
            return "[" + safeSchema + "].[" + safeTable + "]";
        }



        //protected void pager_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        //{
        //    Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
        //    if ((currnetPageIndx <= 0))
        //    {
        //        currnetPageIndx = 1;
        //    }

        //    if ((currnetPageIndx > grdData.PageCount))
        //    {
        //        currnetPageIndx = (grdData.PageCount - 1);
        //    }

        //    pager1.CurrentIndex = currnetPageIndx;
        //    grdData.CurrentPageIndex = (currnetPageIndx - 1);
        //    this.FillGrid();
        //}

        protected void grdData_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            objLookup.Delete(TargetTableName, id);
            this.FillGrid();
        }

        protected void btnDelete_Click(object sender, System.EventArgs e)
        {
            string Script = "";
            string lst = "";
            for (int i = 0; (i
                        <= (grdData.Items.Count - 1)); i++)
            {
                string id = grdData.Items[i].Cells[0].Text;
                CheckBox check = ((CheckBox)(grdData.Items[i].FindControl("chkItem")));
                if (check.Checked)
                {
                    if (lst.Equals(""))
                    {
                        lst = (lst + id);
                    }
                    else
                    {
                        lst = (lst + ("," + id));
                    }

                }

            }

            if (!lst.Trim().Equals(""))
            {
                try
                {
                    objLookup.DeleteList(TargetTableName, lst);
                    this.FillGrid();

                    Script = FormatpopupErrorMSG("تم الحذف بنجاج", "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                }
                catch (Exception)
                {


                    string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteMaterDataFailed, "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    lblerror.ForeColor = System.Drawing.Color.Red;
                    return;
                }

            }

        }

        protected void grdData_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            this.ClearForm();
            ViewState["Item"] = id;
            this.FillForm();
            tblshow.Visible = false;
            tblAdd.Visible = true;
            editflag = true;
            //btnSave.Visible = userAccess.Edit;


        }

        protected void grdData_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='\'#f2d575\'';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='\'#FFFFFF\'';");
            }

            if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor='\'#f2d575\'';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='\'#FFFFFF\'';");
            }

        }

        private string GetTitle(bool isadd)
        {
            if (isadd)
            {
                return "";
            }
            else
            {
                return "";
            }

        }


        private void FillForm()
        {
           var masterList = objLookup.GetDetails(TargetTableName, ViewState["Item"].ToString());
            if (masterList!=null)
            {


                txtNameEn.Text = gets(masterList.NameEn);
                txtNameAr.Text = gets(masterList.NameAr);

                if (IsLawDocCategory())
                {
                    ConfigureLawDocCategoryFields();
                    var valuesQuery = "SELECT TypeID FROM " + TargetTableName + " WHERE Code = @code";
                    var values = objLookup.DC.Database.SqlQuery<LawDocCategoryValues>(valuesQuery,
                        new SqlParameter("@code", ViewState["Item"].ToString())).FirstOrDefault();

                    if (ddlTypeId != null)
                    {
                        ddlTypeId.SelectedValue = values != null && values.TypeID.HasValue ? values.TypeID.Value.ToString() : "";
                    }

                }
                if (IsCommittees_RefList())
                {
                    ConfigureLawDocCategoryFields();
                    var valuesQuery = "SELECT TypeID FROM " + TargetTableName + " WHERE Code = @code";
                    var values = objLookup.DC.Database.SqlQuery<LawDocCategoryValues>(valuesQuery,
                        new SqlParameter("@code", ViewState["Item"].ToString())).FirstOrDefault();

                    if (ddlTypeId != null)
                    {
                        ddlTypeId.SelectedValue = values != null && values.TypeID.HasValue ? values.TypeID.Value.ToString() : "";
                    }
         
                }

                if (IsSectorsTable())
                {
                    var valuesQuery = "SELECT imgPath FROM " + TargetTableName + " WHERE Code = @code";
                    var values = objLookup.DC.Database.SqlQuery<SectorValues>(valuesQuery,
                        new SqlParameter("@code", ViewState["Item"].ToString())).FirstOrDefault();

                    hdnSectorImagePath.Value = values != null ? gets(values.imgPath) : string.Empty;
                    if (imgSectorPreview != null)
                    {
                        var imagePath = GetSectorImagePath(hdnSectorImagePath.Value);
                        imgSectorPreview.ImageUrl = imagePath;
                        imgSectorPreview.Visible = !string.IsNullOrWhiteSpace(hdnSectorImagePath.Value);
                    }
                }

            }

            tblAdd.Visible = true;
            lblSubTitle.Text = this.GetTitle(false);

        }
        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            if (editflag)
            {
                btnSave.Visible = userAccess.Edit;
            }
            
            //btnDelete.Visible = userAccess.Delete;


        }


        private string getImage(ref FileUpload txtFile)
        {
            string imgname = "";
            string temp;
            string ext;
            int inx;
            int i;
            string RandChar;
            string ValueString;
            //  Microsoft.VisualBasic.VBMath.Randomize();
            imgname = "";
            ValueString = "";
            if (!(txtFile.PostedFile == null))
            {
                if ((txtFile.PostedFile.FileName != ""))
                {
                    imgname = txtFile.PostedFile.FileName;
                    imgname = imgname.Substring((imgname.LastIndexOf("\\") + 1));
                    inx = imgname.LastIndexOf(".");
                    temp = imgname.Substring(0, inx);
                    ext = imgname.Substring((inx + 1));
                    for (i = 1; (i <= 16); i++)
                    {
                        //  RandChar = (string)(Microsoft.VisualBasic.Conversion.Int((26 * Microsoft.VisualBasic.VBMath.Rnd() + 65)).ToString());

                        RandChar = (new Random().Next(i, 16)).ToString();
                        ValueString += RandChar;
                    }

                    imgname = (ValueString + ("." + ext));
                    txtFile.PostedFile.SaveAs(Server.MapPath(("/Layout/uploads/Adminprofile/" + imgname)));
                }

            }


            //////string imgname = "";
            //////int inx = 0;
            //////string temp = "";
            //////string ext = "";
            //////string RandChar = "";
            //////string ValueString = "";
            //////Microsoft.VisualBasic.VBMath.Randomize();
            //////imgname = "";
            //////imgname = txtImage.PostedFile.FileName;
            //////imgname = imgname.Substring((imgname.LastIndexOf("\\") + 1));
            //////inx = imgname.LastIndexOf('.');
            //////temp = imgname.Substring(0, inx);
            //////ext = imgname.Substring((inx + 1));
            //////for (int i = 1; (i <= 10); i++)
            //////{
            //////    RandChar = (string)(Microsoft.VisualBasic.Conversion.Int((26 * Microsoft.VisualBasic.VBMath.Rnd() + 65)).ToString());
            //////    ValueString += RandChar;
            //////}
            //////imgname = (temp + (ValueString + ("." + ext)));
            //////return imgname;
            return imgname;
        }

        private void ClearForm()
        {
            txtNameEn.Text = "";
            txtNameAr.Text = "";

            if (IsLawDocCategory()|| IsCommittees_RefList())
            {
                if (ddlTypeId != null)
                {
                    ddlTypeId.SelectedIndex = 0;
                }
             
            }

            if (IsSectorsTable())
            {
                hdnSectorImagePath.Value = string.Empty;
                if (imgSectorPreview != null)
                {
                    imgSectorPreview.ImageUrl = string.Empty;
                    imgSectorPreview.Visible = false;
                }
            }

            ViewState["Item"] = 0;
            tblAdd.Visible = false;
            tblshow.Visible = true;
            lblSubTitle.Text = this.GetTitle(true);
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            string Script = "";
            try
            {
                //Check Item Existance


                if (ViewState["Item"].ToString().Equals("0"))
                {

                    if (objLookup.checkTextExistance(TargetTableName , txtNameAr.Text))
                    {

                        Script = FormatpopupErrorMSG("Item already exist , repeating not allowed", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                        return;
                    }

                    if (IsLawDocCategory() || IsCommittees_RefList())
                    {
                        InsertLawDocCategory();
                    }
                   
                    else if (IsSectorsTable())
                    {
                        InsertSector();
                    }
                    else
                    {
                        objLookup.Insert(TargetTableName, txtNameEn.Text, txtNameAr.Text);
                    }
                }
                else
                {
                    if (IsLawDocCategory() || IsCommittees_RefList())
                    {
                        UpdateLawDocCategory();
                    }
                   
                    else if (IsSectorsTable())
                    {
                        UpdateSector();
                    }
                    else
                    {
                        objLookup.Update(TargetTableName, ViewState["Item"].ToString(), txtNameEn.Text, txtNameAr.Text);
                    }
                }

                Script = FormatpopupErrorMSG("تم حفظ البيانات بنجاح", "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                this.ClearForm();
                this.FillGrid();
            }
            catch (Exception ex)
            {
                Script = FormatpopupErrorMSG("خطأ فى حفظ البيانات", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                lblerror.ForeColor = System.Drawing.Color.Red;
                lblerror.Text = ("Error :" + ex.Message);
            }

        }

        private void InsertLawDocCategory()
        {
            var typeId = string.IsNullOrWhiteSpace(ddlTypeId.SelectedValue) ? (object)DBNull.Value : ddlTypeId.SelectedValue;
            

            var query = "INSERT INTO " + TargetTableName + " (NameEn, NameAr, TypeID) VALUES (@nameEn, @nameAr, @typeId)";
            objLookup.DC.Database.ExecuteSqlCommand(query,
                new SqlParameter("@nameEn", gets(txtNameEn.Text)),
                new SqlParameter("@nameAr", gets(txtNameAr.Text)),
                new SqlParameter("@typeId", typeId));
        }

        private void UpdateLawDocCategory()
        {
            var typeId = string.IsNullOrWhiteSpace(ddlTypeId.SelectedValue) ? (object)DBNull.Value : ddlTypeId.SelectedValue;
            

            var query = "UPDATE " + TargetTableName + " SET NameEn=@nameEn, NameAr=@nameAr, TypeID=@typeId  WHERE Code=@code";
            objLookup.DC.Database.ExecuteSqlCommand(query,
                new SqlParameter("@nameEn", gets(txtNameEn.Text)),
                new SqlParameter("@nameAr", gets(txtNameAr.Text)),
                new SqlParameter("@typeId", typeId),
                new SqlParameter("@code", ViewState["Item"].ToString()));
        }

        private void InsertSector()
        {
            var imagePath = SaveSectorImage();
            var query = "INSERT INTO " + TargetTableName + " (NameEn, NameAr, imgPath) VALUES (@nameEn, @nameAr, @imgPath)";
            objLookup.DC.Database.ExecuteSqlCommand(query,
                new SqlParameter("@nameEn", gets(txtNameEn.Text)),
                new SqlParameter("@nameAr", gets(txtNameAr.Text)),
                new SqlParameter("@imgPath", string.IsNullOrWhiteSpace(imagePath) ? (object)DBNull.Value : imagePath));
        }

        private void UpdateSector()
        {
            var imagePath = SaveSectorImage();
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                imagePath = hdnSectorImagePath.Value;
            }

            var query = "UPDATE " + TargetTableName + " SET NameEn=@nameEn, NameAr=@nameAr, imgPath=@imgPath WHERE Code=@code";
            objLookup.DC.Database.ExecuteSqlCommand(query,
                new SqlParameter("@nameEn", gets(txtNameEn.Text)),
                new SqlParameter("@nameAr", gets(txtNameAr.Text)),
                new SqlParameter("@imgPath", string.IsNullOrWhiteSpace(imagePath) ? (object)DBNull.Value : imagePath),
                new SqlParameter("@code", ViewState["Item"].ToString()));
        }

        private string SaveSectorImage()
        {
            if (txtSectorImage == null || !txtSectorImage.HasFile)
            {
                return string.Empty;
            }

            var extension = Path.GetExtension(txtSectorImage.FileName);
            var fileName = Guid.NewGuid().ToString("N") + extension;
            var folderPath = Server.MapPath("~/Layout/uploads/Sectors/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            txtSectorImage.SaveAs(Path.Combine(folderPath, fileName));
            return fileName;
        }

        private string GetSectorImagePath(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
            {
                return string.Empty;
            }

            return "/Layout/uploads/Sectors/" + imageName;
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.ClearForm();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.FillGrid();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            this.ClearForm();
            tblAdd.Visible = true;
            tblshow.Visible = false;
            editflag = false;
            ConfigureLawDocCategoryFields();
        }

        protected void grdData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            grdData.CurrentPageIndex = e.NewPageIndex;
            this.FillGrid();
        }
    }
}