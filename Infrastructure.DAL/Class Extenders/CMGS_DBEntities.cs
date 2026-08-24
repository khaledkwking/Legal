using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;

namespace Infrastructure.DAL.Model
{
    public class CustomLog
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
        public string ColumnName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }

    }

    public class Audit
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }

    public partial class CMGS_DBEntities : DbContext
    {
        public override int SaveChanges()
        {

            System.Collections.Generic.IEnumerable<DbEntityEntry> addedAuditedEntities = ChangeTracker.Entries()
              .Where(p => p.State == EntityState.Added).ToList();
            //.OfType<IAuditedEntity>();

            System.Collections.Generic.IEnumerable<DbEntityEntry> modifiedAuditedEntities = ChangeTracker.Entries()
              .Where(p => p.State == EntityState.Modified)
              .ToList();


            System.Collections.Generic.IEnumerable<DbEntityEntry> deletedAuditedEntities = ChangeTracker.Entries()
              .Where(p => p.State == EntityState.Deleted).ToList();
            
            AuditLog log;

            foreach (var change in modifiedAuditedEntities)
            {
                Type entityType = change.Entity.GetType();
                if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                    entityType = entityType.BaseType;

                var entityName = entityType.Name;
                var primaryKey = GetPrimaryKeyValue(change);
                string ModifiedDocNo = GetDocNo(change);
                string ModuleName = GetModule(entityName, change);

                foreach (var prop in change.OriginalValues.PropertyNames)
                {
                    var originalValue = change.OriginalValues[prop] != null ? change.OriginalValues[prop].ToString() : null;
                    var currentValue = change.CurrentValues[prop] != null ? change.CurrentValues[prop].ToString() : null;
                    string AuditType = "M";
                    if (change.CurrentValues[prop] != null)
                    {
                        if ( prop == "isAudited")
                        {
                            if (change.CurrentValues[prop].ToString() == "True")
                            {
                                AuditType = "U";
                            }
                            else
                            {
                                AuditType = "M";
                            }
                        }
                    }
                    if (originalValue != currentValue)
                    {
                        log = new AuditLog()
                        {
                            TableName = entityName,
                            PK = primaryKey.ToString(),
                            ColumnName = prop,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            Date = DateTime.Now,
                            AuditType = AuditType, // "M",
                            No = ModifiedDocNo,
                            ModuleName = ModuleName,
                            UserId = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()),
                            // Operation = Environment.MachineName + ";" + UserSetting.ApplicationVersion,
                            Id = Guid.NewGuid()

                        };
                        AuditLog.Add(log);

                    }
                }
                if (change.CurrentValues.PropertyNames.Contains("LastModifiedAt"))
                {

                    DbPropertyEntry LastModifiedAtprop = change.Property("LastModifiedAt");
                    LastModifiedAtprop.CurrentValue = DateTime.Now;
                }
                if (change.CurrentValues.PropertyNames.Contains("LastModifiedBy"))
                {
                    DbPropertyEntry LastModifiedByprop = change.Property("LastModifiedBy");
                    LastModifiedByprop.CurrentValue = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
                }
            }

            foreach (var change in deletedAuditedEntities)
            {
                Type entityType = change.Entity.GetType();
                if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                    entityType = entityType.BaseType;

                var entityName = entityType.Name;

                // Handle deletion of related records before deleting the parent
                if (entityName == "Law_DocData")
                {
                    try
                    {
                        var primaryKey = GetPrimaryKeyValue(change);
                        // Delete related Law_DocSectors records first to avoid FK constraint violation
                        var relatedSectors = Law_DocSectors.Where(s => s.Law_DocId == (int)primaryKey).ToList();
                        foreach (var sector in relatedSectors)
                        {
                            Law_DocSectors.Remove(sector);
                        }
                        var relatedLaw_Linked = Law_DocData_Linked.Where(s => s.SouceDocID == (int)primaryKey)
                                .ToList();

                        foreach (var law in relatedLaw_Linked)
                        {
                            Law_DocData_Linked.Remove(law);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the error, but continue with the delete operation
                        System.Diagnostics.Debug.WriteLine("Error deleting related Law_DocSectors: " + ex.Message);
                    }
                }
                else if (entityName == "Parliament_Questions")
                {
                    var primaryKey = GetPrimaryKeyValue(change);
                    // Delete related Law_DocSectors records first to avoid FK constraint violation
                    var relatedParliament = Parliament_Requestedby.Where(s => s.QuestionID == (int)primaryKey)  .ToList();

                    foreach (var row in relatedParliament)
                    {
                        Parliament_Requestedby.Remove(row);
                    }
                }
                else if (entityName == "AgreementData")
                {
                    var primaryKey = GetPrimaryKeyValue(change);
                    // Delete related Law_DocSectors records first to avoid FK constraint violation
                    var relatedAgreement = Agreement_procedureHistory.Where(s => s.AgreementCode == (int)primaryKey).ToList();

                    foreach (var Newrow in relatedAgreement)
                    {
                        Agreement_procedureHistory.Remove(Newrow);
                    }
                    var relatedAttachments = Agreement_Attachments.Where(s => s.AgreementCode == (int)primaryKey).ToList();

                    foreach (var Attrow in relatedAttachments)
                    {
                        Agreement_Attachments.Remove(Attrow);
                    }

                    
                }
                else if (entityName == "Medal_Data")
                {
                    var primaryKey = GetPrimaryKeyValue(change);
                    // Delete related Law_DocSectors records first to avoid FK constraint violation
                    var relatedParliament = Parliament_Requestedby.Where(s => s.QuestionID == (int)primaryKey).ToList();

                    foreach (var row in relatedParliament)
                    {
                        Parliament_Requestedby.Remove(row);
                    }
                }

                var primaryKeyValue = GetPrimaryKeyValue(change);
                string DeletedDocNo = GetDocNo(change);

                string ModuleName = GetModule(entityName, change);

                foreach (var prop in change.OriginalValues.PropertyNames)
                {
                    var originalValue = change.OriginalValues[prop] != null ? change.OriginalValues[prop].ToString() : null;
                    var currentValue ="";//change.CurrentValues[prop] != null ? change.CurrentValues[prop].ToString() : null;
                    if (originalValue != currentValue)
                    {
                        log = new AuditLog()
                        {
                            TableName = entityName,
                            PK = primaryKeyValue.ToString(),
                            ColumnName = prop,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            Date = DateTime.Now,
                            AuditType = "D",
                            No = DeletedDocNo,
                            ModuleName = ModuleName,
                            UserId = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()),
                            // Operation = Environment.MachineName + ";" + UserSetting.ApplicationVersion,
                            Id = Guid.NewGuid()

                        };
                        AuditLog.Add(log);

                    }
                }
                //if (change.CurrentValues.PropertyNames.Contains("LastModifiedAt"))


                //    DbPropertyEntry LastModifiedAtprop = change.Property("LastModifiedAt");
                //    LastModifiedAtprop.CurrentValue = DateTime.Now;

                //if (change.CurrentValues.PropertyNames.Contains("LastModifiedBy"))

                //    DbPropertyEntry LastModifiedByprop = change.Property("LastModifiedBy");
                //    LastModifiedByprop.CurrentValue = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());

            }
            int changes = base.SaveChanges();

            foreach (DbEntityEntry added in addedAuditedEntities)
            {
                string addDocNo;
                if (added.CurrentValues.PropertyNames.Contains("CreatedBy"))
                {
                    DbPropertyEntry CreatedByprop = added.Property("CreatedBy");
                    CreatedByprop.CurrentValue = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
                }
                if (added.CurrentValues.PropertyNames.Contains("CreatedAt"))
                {
                    DbPropertyEntry CreatedAtprop = added.Property("CreatedAt");
                    CreatedAtprop.CurrentValue = DateTime.Now;
                }
                Type entityType = added.Entity.GetType();
                if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                    entityType = entityType.BaseType;

                var entityName = entityType.Name;

                addDocNo =GetDocNo(added);
                string ModuleName = GetModule(entityName, added);

                var primaryKey = GetPrimaryKeyValue(added);

                foreach (var prop in added.CurrentValues.PropertyNames)
                {
                    var currentValue = added.CurrentValues[prop] != null ? added.CurrentValues[prop].ToString() : null;

                    log = new AuditLog()
                    {
                        TableName = entityName,
                        PK = primaryKey.ToString(),
                        ColumnName = prop,
                        OldValue = "",
                        NewValue = currentValue,
                        Date = DateTime.Now,
                        AuditType = "A",
                        No = addDocNo,
                        ModuleName = ModuleName,
                        UserId = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()),
                        // Operation = Environment.MachineName + ";" + UserSetting.ApplicationVersion,
                        Id = Guid.NewGuid()

                    };
                    AuditLog.Add(log);


                }
            }
            base.SaveChanges();

            return changes;
        }
        private string GetDocNo(DbEntityEntry entry)
        {
            string[] properties =
            {
        "CaseSerial",
        "DocSerial",
        "committeeSerial",
        "FileNum",
        "M_Serial",
        "Agr_Serial"
    };

            var entity = entry.State == EntityState.Deleted
                ? entry.OriginalValues.ToObject()
                : entry.Entity;

            foreach (var property in properties)
            {
                var prop = entity.GetType().GetProperty(property);
                if (prop == null)
                    continue;

                var value = prop.GetValue(entity);

                if (value != null)
                    return value.ToString();
            }

            return string.Empty;
        }





        object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }


        //public override int SaveChanges()
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        var addedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
        //        var modifiedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified).ToList();

        //        foreach (var entry in modifiedEntries)
        //        {
        //            ApplyAuditLog(entry);
        //        }

        //        int changes = base.SaveChanges();
        //        foreach (var entry in addedEntries)
        //        {
        //            ApplyAuditLog(entry, LogOperation.CreateEntity);
        //        }

        //        base.SaveChanges();
        //        scope.Complete();
        //        return changes;
        //    }
        //}

        //private void ApplyAuditLog(DbEntityEntry entry)
        //{
        //    LogOperation operation;
        //    switch (entry.State)
        //    {
        //        case EntityState.Added:
        //            operation = LogOperation.CreateEntity;
        //            break;
        //        case EntityState.Deleted:
        //            operation = LogOperation.DeleteEntity;
        //            break;
        //        case EntityState.Modified:
        //            operation = LogOperation.UpdateEntity;
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }

        //    ApplyAuditLog(entry, operation);
        //}

        //private void ApplyAuditLog(DbEntityEntry entry, LogOperation logOperation)
        //{
        //    ILog entity = entry.Entity as ILog;

        //    if (entity != null)
        //    {
        //        AuditLog log = new AuditLog
        //        {
        //            Created = DateTime.Now,
        //            Entity = entry.Entity.GetType().Name,
        //            EntityId = entity.Id,
        //            Operation = logOperation,
        //        };
        //        AuditLog.Add(log);
        //    }
        //}
        //string GetModule(string TableName, DbEntityEntry entry)
        //{
        //    string ModuleName = "";
        //    if (entry != null)
        //    {


        //        switch (TableName)
        //        {
        //            case "Law_DocData":
        //            string DocTypeID = "";
        //            if (entry.CurrentValues.PropertyNames.Contains("DocTypeID"))
        //            {
        //                if (entry.Property("DocTypeID").CurrentValue != null)
        //                {
        //                    DocTypeID = entry.Property("DocTypeID").CurrentValue.ToString();
        //                }
        //            }

        //            switch (DocTypeID)
        //            {
        //                case "1":
        //                    ModuleName = "قانون";
        //                    break;
        //                case "2":
        //                    ModuleName = "مرسوم بقانون";
        //                    break;
        //                case "3":
        //                    ModuleName = "مرسوم";
        //                    break;
        //                case "4":
        //                    ModuleName = "أمر أميري";
        //                    break;
        //                case "5":
        //                    ModuleName = "قرار مجلس الوزراء";
        //                    break;
        //                case "6":
        //                    ModuleName = "قرار سمو رئيس مجلس الوزراء";
        //                    break;
        //                case "7":
        //                    ModuleName = "مشروع قانون محال";
        //                    break;
        //                case "8":
        //                    ModuleName = "حكم المحكمة الدستورية";
        //                    break;
        //                case "9":
        //                    ModuleName = "الدستور";
        //                    break;
        //            }
        //                break;
        //            case "LegalMemo":
        //                ModuleName = "كتب و مذكرات";
        //                break;
        //            case "Committees_Data":
        //                ModuleName = "المجالس واللجان العليا ومجالس إدارات الجهات";
        //                break;
        //            case "Parliament_Questions":
        //                ModuleName = "الاسئلة البرلمانية";
        //                break;
        //            case "Cases_Data":
        //                ModuleName = "مواضيع القضايا";
        //                break;
        //            case "Parliament_QuestionsAnswers":
        //                ModuleName = "الاستجوابات البرلمانية";
        //                break;
        //            case "AgreementData":
        //                ModuleName = "الاتفاقيات";
        //                break;

        //        }


        //    }
        //    return ModuleName;
        //}
        private object GetPropertyValue(DbEntityEntry entry, string propertyName)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                    return entry.CurrentValues.PropertyNames.Contains(propertyName)
                        ? entry.CurrentValues[propertyName]
                        : null;

                case EntityState.Deleted:
                    return entry.OriginalValues.PropertyNames.Contains(propertyName)
                        ? entry.OriginalValues[propertyName]
                        : null;

                default:
                    return null;
            }
        }
        private string GetModule(string tableName, DbEntityEntry entry)
        {
            switch (tableName)
            {
                case "Law_DocData":

                    var docType = GetPropertyValue(entry, "DocTypeID")?.ToString();

                    switch (docType)
                    {
                        case "1": return "قانون";
                        case "2": return "مرسوم بقانون";
                        case "3": return "مرسوم";
                        case "4": return "أمر أميري";
                        case "5": return "قرار مجلس الوزراء";
                        case "6": return "قرار سمو رئيس مجلس الوزراء";
                        case "7": return "مشروع قانون محال";
                        case "8": return "حكم المحكمة الدستورية";
                        case "9": return "الدستور";
                        default: return "";
                    }

                case "LegalMemo":
                    return "كتب و مذكرات";

                case "Committees_Data":
                    return "المجالس واللجان العليا ومجالس إدارات الجهات";

                case "Parliament_Questions":
                    return "الأسئلة البرلمانية";

                case "Cases_Data":
                    return "مواضيع القضايا";

                case "Parliament_QuestionsAnswers":
                    return "الاستجوابات البرلمانية";

                case "AgreementData":
                    return "الاتفاقيات";

                default:
                    return "";
            }
        }

    }
    
}
