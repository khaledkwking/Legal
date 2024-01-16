using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainInterface;
using Permission.DAL.Repository;


namespace Infrastructure.DAL
{
    public partial class AdminRepository //: IAdminRepository
    {


        #region Administration MemberShip
        //public IList<Permission.DAL.Repository.Security_pr_admin> GetMemberShips()
        //{
        //    using (var DC = new CMGS_SECDBEntities())
        //    {
        //        var result =
        //            (from obj in DC.Security_pr_admin
        //             .Include("Rule")
        //             select obj);

        //        return result.ToList<Permission.DAL.Repository.Security_pr_admin>();
        //    }
        //}

        //public IList<DomainInterface.Security_pr_admin> GetMemberShipsForAdmin()
        //{
        //    using (var DC = new CMGS_SECDBEntities())
        //    {
        //        var result =
        //            (from obj in DC.MemberShips
        //              .Include("Rule")
        //             select obj);

        //        return result.ToList<DomainInterface.Security_pr_admin>();
        //    }
        //}

        //public Security_pr_admin GetNewMemberShip()
        //{
        //    return new MemberShip();
        //}

        //public IList<IKeyListItem> FindMemberShipKeys()
        //{
        //    using (var DC = new CMGS_SECDBEntities())
        //    {
        //        var varObjecive = (from item in DC.MemberShips
        //                            .Include("Rule")
        //                           select new KeyListItem
        //                           {
        //                               ID = item.ID,
        //                               Name = item.Name
        //                           });

        //        return varObjecive.AsEnumerable().Cast<IKeyListItem>().ToList<IKeyListItem>();
        //    }
        //}

        //public Security_pr_admin GetMemberShipByID(int ID)
        //{
        //    using (var DC = new CMGS_SECDBEntities())
        //    {
        //        var result =
        //            (from obj in DC.MemberShips
        //             .Include("Rule")
        //             where obj.ID == ID
        //             select obj);

        //        return result.FirstOrDefault();
        //    }
        //}

        //public IList<DomainInterface.Security_pr_admin> GetMemberShipByRuleID(int ID)
        //{
        //    using (var DC = new CMGS_SECDBEntities())
        //    {
        //        var result =
        //            (from obj in DC.MemberShips
        //              .Include("Rule")
        //             where obj.RuleID == ID
        //             select obj);

        //        return result.ToList<DomainInterface.Security_pr_admin>();
        //    }
        //}

        public Security_pr_admin GetMemberShipByName(string Name)
        {
            using (var DC = new CMGS_SECDBEntities())
            {
                var result =
                    (from obj in DC.Security_pr_admin
                     // .Include("Rule")
                     where obj.username == Name
                     select obj).FirstOrDefault<Security_pr_admin>();

                return result;
            }
        }
        public Security_pr_admin GetMemberShipById(int userCode)
        {
            using (var DC = new CMGS_SECDBEntities())
            {
                var result =
                    (from obj in DC.Security_pr_admin
                         // .Include("Rule")
                     where obj.id == userCode
                     select obj).FirstOrDefault<Security_pr_admin>();

                return result;
            }
        }

        public int? GetMemberShipPermssionCount(int? AdminType, int AdminID)
        {

            using (var DC = new CMGS_SECDBEntities())
            {
                var result =
                    (from obj in DC.Security_SP_getPermissionsCount(AdminType, AdminID)
                     select obj).FirstOrDefault();
                return result;
            }
        }

        public List<Security_SP_PRM_getJobPermissions_Result> GetMemberShipJobPermssion(int? AdminType)
        {

            using (var DC = new CMGS_SECDBEntities())
            {

                var result2 =
               (from obj in DC.Security_SP_PRM_getJobPermissions(AdminType)
                select obj).ToList();

                return result2;

            }



            
        }

        public List<Security_SP_PRM_getSystemPermission_Result> GetMemberShipSystemPermssion(int? AdminType,int adminID)
        {

            using (var DC = new CMGS_SECDBEntities())
            {

                var result2 =
               (from obj in DC.Security_SP_PRM_getSystemPermission(AdminType, adminID)
                select obj).ToList();

                return result2;

            }




        }


        public List<Security_SP_PRM_getUserPage_Result> GetMemberShipPagePermssion(int? AdminType, int AdminID,string pageURL)
        {
            using (var DC = new CMGS_SECDBEntities())
            {
                var result =
                    (from obj in DC.Security_SP_PRM_getUserPage(AdminType, AdminID, pageURL) 
                     select obj).ToList<Security_SP_PRM_getUserPage_Result>();

                return result;
            }
        }

        //#region "Add ,  Update  ,Delete" For Admin MemberShip

        //public int AddMemberShip<T>(T item)
        //{
        //    using (var DC = new CMGS_SECDBEntities())
        //    {
        //        return DC.Add<MemberShip>("MemberShips", item as MemberShip);
        //    }
        //}

        //public int DeleteMemberShip<T>(T item)
        //{
        //    using (var DC = new CMGS_SECDBEntities())
        //    {
        //        //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();

        //        return DC.Delete<MemberShip>("MemberShips", item as MemberShip);
        //    }
        //}

        //public int UpdateMemberShip<T>(T item)
        //{
        //    using (var DC = new CMGS_SECDBEntities())
        //    {
        //        return DC.Update<MemberShip>("MemberShips", item as MemberShip);
        //    }
        //}


        //#endregion
        #endregion

    }
}
