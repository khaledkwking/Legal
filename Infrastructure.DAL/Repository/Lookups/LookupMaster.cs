using Infrastructure.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Infrastructure.DAL
{
    public class LookupModel
    {
        public int Code { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int d_order { get; set; }
        public string img { get; set; }
        public int? TypeID { get; set; }
        public int? DocCategoryMainId { get; set; }
        public string TypeNameAr { get; set; }
        public string DocCategoryMainNameAr { get; set; }

    }

    public class LookupMaster : BaseRepository
    {


        public LookupMaster(CMGS_DBEntities _context) : base(_context)
        {

        }


        //  public static LookupMaster ins = new LookupMaster();

        public List<LookupModel> GetItems(string TableName, string FilterStr, string d_order = "", string refname = "", string refvalue = "", string filterColumn = "")
        {
            string query = "SELECT * ";
            query += " FROM " + TableName;
            query += " where 1=1";

            if (FilterStr != "" && FilterStr != "0")
            {
                if (filterColumn != "")
                {
                    var safeColumn = filterColumn.Replace("]", "]]" );
                    query += " and ( CONVERT(NVARCHAR(4000), [" + safeColumn + "]) like N'%" + FilterStr + "%')";
                }
                else
                {
                    query += " and ( NameEn like N'%" + FilterStr + "%' or NameAr like N'%" + FilterStr + "%')";
                }
            }

            if (refvalue != "" && refvalue != "0")
            {
                query += " and (" + refname + "=" + refvalue + ")";
            }
            if (d_order != "")
            {
                query += " order by  d_order asc";
            }
            var result = DC.Database.SqlQuery<LookupModel>(query);
            return result.ToList<LookupModel>();
        }

        public List<string> GetFilterColumns(string TableName)
        {
            var tableName = TableName;
            var schemaName = "dbo";

            if (!string.IsNullOrWhiteSpace(TableName) && TableName.Contains("."))
            {
                var parts = TableName.Split('.');
                if (parts.Length >= 2)
                {
                    schemaName = parts[0].Trim('[', ']');
                    tableName = parts[1].Trim('[', ']');
                }
            }
            else if (!string.IsNullOrWhiteSpace(TableName))
            {
                tableName = TableName.Trim('[', ']');
            }

            string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = @schemaName AND TABLE_NAME = @tableName ORDER BY ORDINAL_POSITION";
            var result = DC.Database.SqlQuery<string>(query,
                new SqlParameter("@schemaName", schemaName),
                new SqlParameter("@tableName", tableName)).ToList();

            return result.Where(column => !string.Equals(column, "code", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<LookupModel> FillLookup(string TableName)
        {
            string query = "select * from " + TableName;// " order by  NameEn";
                                                        // return ABOBasic.ins.ExecuteDs(query);
            var result = DC.Database.SqlQuery<LookupModel>(query);
            return result.ToList<LookupModel>();
        }

        public bool checkTextExistance(string TableName, string TextToCompair)
        {
            string query = ("select * from " + TableName + " where NameAr like N'" + TextToCompair + "'");

            var result = DC.Database.SqlQuery<LookupModel>(query).ToList();
            // result.ToList<LookupModel>();

            if (result != null && result.Count > 0)
            {
                return true;
            }

            return false;
        }

        public LookupModel GetDetails(string TableName, string code)
        {
            string query = ("select * from " + TableName + " where code=" + code);


            var result = DC.Database.SqlQuery<LookupModel>(query);
            return result.FirstOrDefault<LookupModel>();

        }

        public void Insert(string TableName, string NameEn, string NameAr, string D_Order = "", string refName = "", string refvalue = "", string img = "")
        {
            string q = "insert into " + TableName + "(NameEn,NameAr " + (D_Order != "" ? ",D_Order" : "") + (refName != "" ? "," + refName + "" : "")+(img != "" ? ",img" : "") + ")";
            q += " values(N'" + FixString(NameEn) + "',N'" + FixString(NameAr) + "'" + (D_Order != "" ? "," + D_Order : "") + (refvalue != "" ? "," + refvalue : "") + (img != "" ? ",'" + img+"'" : "") + ")";

            DC.Database.ExecuteSqlCommand(q);
        }

        public void Update(string TableName, string code, string NameEn, string NameAr, string D_Order = "", string img = "")
        {
            string q = "update " + TableName + " set NameEn=N'" + FixString(NameEn) + "',NameAr=N'" + FixString(NameAr) + "'" + (D_Order != "" ? ",D_Order=" + D_Order : "") + (img != "" ? ",img='" + img+"'" : "");
            q += " where code = " + code;

            DC.Database.ExecuteSqlCommand(q);

        }
        public void Delete(string TableName, string id)
        {
            try
            {
                // Handle foreign key constraints by deleting related records first
                if (TableName.ToLower() == "law_docdata")
                {
                    // Delete related Law_DocSectors records
                    string deleteSectorsQuery = "DELETE FROM Law_DocSectors WHERE Law_DocId = " + id;
                    DC.Database.ExecuteSqlCommand(deleteSectorsQuery);
                }

                string q = ("delete from " + TableName + " where code=" + id);
                DC.Database.ExecuteSqlCommand(q);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting record from " + TableName + ": " + ex.Message, ex);
            }
        }

        public void DeleteList(string TableName, string list)
        {
            try
            {
                // Handle foreign key constraints by deleting related records first
                if (TableName.ToLower() == "law_docdata")
                {
                    // Delete related Law_DocSectors records for all items in the list
                    string deleteSectorsQuery = "DELETE FROM Law_DocSectors WHERE Law_DocId IN (" + list + ")";
                    DC.Database.ExecuteSqlCommand(deleteSectorsQuery);
                }

                string q = "delete from " + TableName + " where code in (" + list + ")";
                DC.Database.ExecuteSqlCommand(q);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting records from " + TableName + ": " + ex.Message, ex);
            }
        }
        private string FixString(string per)
        {
            if (per.Equals("") | per.Equals("0"))
            {
                return "0";
            }
            else
            {
                return per;
            }
        }

    }
}