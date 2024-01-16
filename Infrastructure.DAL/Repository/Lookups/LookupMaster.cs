using Infrastructure.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data;
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

    }

    public class LookupMaster : BaseRepository
    {


        public LookupMaster(CMGS_DBEntities _context) : base(_context)
        {

        }


        //  public static LookupMaster ins = new LookupMaster();

        public List<LookupModel> GetItems(string TableName, string FilterStr, string d_order = "", string refname = "", string refvalue = "")
        {
            string query = "SELECT * ";
            query += " FROM " + TableName;
            query += " where 1=1";

            if (FilterStr != "" && FilterStr != "0")
            {
                query += " and ( NameEn like N'%" + FilterStr + "%' or NameAr like N'%" + FilterStr + "%')";
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
            string q = ("delete from " + TableName + " where code=" + id);

            DC.Database.ExecuteSqlCommand(q);

        }
        public void DeleteList(string TableName, string list)
        {
            string q = "delete from " + TableName + " where code in (" + list + ")";
            DC.Database.ExecuteSqlCommand(q);
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