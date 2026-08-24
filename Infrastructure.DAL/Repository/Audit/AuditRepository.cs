using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainInterface;
using Infrastructure.DAL.Model;
using Infrastructure.DAL.Enum;
using Infrastructure.DAL.ViewModels;

namespace Infrastructure.DAL
{
    public partial class AuditRepository : BaseRepository
    {
  public AuditRepository(CMGS_DBEntities _context):base(_context)
        {
        }


        #region "AuditRepository master Data"

        #region List
        public List<ViewUser_AuditLog> GetUserTransactionAuditList(int userId, string TableName, string operationType, DateTime TransactionDatFrom, DateTime TransactionDatTo)
        {


            var result =
                (from obj in DC.ViewUser_AuditLog

                 orderby obj.Date ascending
                 where 1 == 1

                  && (userId != 0 ? obj.UserId == userId : 1 == 1)
                  && (TableName != "" && TableName != "0" ? obj.TableName == TableName : 1 == 1)
                  && (operationType != "" && operationType != "0" ? obj.AuditType == operationType : 1 == 1)
                 && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Date >= TransactionDatFrom : 1 == 1))
              && ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Date <= TransactionDatTo : 1 == 1)
              //&& (obj.TabaleNameAr !=null )
              )

                 select obj);

            return result.ToList<ViewUser_AuditLog>();

        }

        public List<View_AuditLog> GetUserAuditList(int userId, string TableName,string operationType, DateTime TransactionDatFrom, DateTime TransactionDatTo)
        {


            var result =
                (from obj in DC.View_AuditLog

                 orderby obj.Date ascending
                 where 1 == 1

                  && (userId != 0 ? obj.UserId == userId : 1 == 1)
                  && (TableName != "" && TableName != "0" ? obj.TableName == TableName : 1 == 1)
                  && (operationType != "" && operationType != "0" ? obj.AuditType == operationType : 1 == 1)
                 && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Date >= TransactionDatFrom : 1 == 1))
              && ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Date <= TransactionDatTo : 1 == 1))

                 select obj);

            return result.ToList<View_AuditLog>();

        }
        public List<View_AudiLogUserOperations> GetList(int userId, int moduleId, DateTime TransactionDatFrom, DateTime TransactionDatTo)
        {


                var result =
                    (from obj in DC.View_AudiLogUserOperations

                     orderby obj.TransDate ascending
                     where 1 == 1

                      && (userId != 0? obj.UserID == userId : 1 == 1)
                      && (moduleId != 0 ? obj.moduleCode == moduleId : 1 == 1)
                     && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.TransDate >= TransactionDatFrom : 1 == 1) )
                  && ( (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.TransDate <= TransactionDatTo : 1 == 1)  )

                     select obj);

                return result.ToList<View_AudiLogUserOperations>();

        }
        public List<SP_UserMonthlyStatistics_Result> GetuserStatisticList(int userId, DateTime TransactionDatFrom, DateTime TransactionDatTo)
        {


            var result = DC.SP_UserMonthlyStatistics(userId, TransactionDatFrom, TransactionDatTo);

              //   orderby obj.TransDate ascending
              //   where 1 == 1

              //    && (userId != 0 ? obj.UserID == userId : 1 == 1)
              //    && (moduleId != 0 ? obj.moduleCode == moduleId : 1 == 1)
              //   && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.TransDate >= TransactionDatFrom : 1 == 1))
              //&& ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.TransDate <= TransactionDatTo : 1 == 1))

              //   select obj);

            return result.ToList<SP_UserMonthlyStatistics_Result>();

        }

        public View_AudiLogUserOperations GetDetails(int _Code)
        {
                var result =
                    (from obj in DC.View_AudiLogUserOperations
                     orderby obj.TransDate ascending
                     where obj.Code == _Code
                     select obj);
                return result.FirstOrDefault<View_AudiLogUserOperations>();

        }

        public List<Security_pr_admin> fillSystemUsystemUsers()
        {
            var result =
                (from obj in DC.Security_pr_admin
                 select obj);

            return result.ToList<Security_pr_admin>();
        }
        public List<Security_pr_MainSystem> fillSysmtesMOdule()
        {
            var result =
                (from obj in DC.Security_pr_MainSystem
                 select obj);

            return result.ToList<Security_pr_MainSystem>();
        }

        public List<AuditLockupModel> FillAuditTables()
        {
            var result =
                (from obj in DC.AuditLog
                 select new AuditLockupModel {
                     Code=obj.TableName,
                     Name=obj.TableName,
                     NameAr=(from sub in DC.Security_pr_SystemTables where sub.RefTable== obj.TableName select sub).FirstOrDefault().TabaleNameAr

                 }).Distinct();
            result = result.Where(c => c.NameAr != null);
            return result.ToList<AuditLockupModel>();
        }

        #endregion

        #endregion


    }
}
