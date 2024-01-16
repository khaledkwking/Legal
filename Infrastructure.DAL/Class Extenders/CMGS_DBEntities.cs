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

                foreach (var prop in change.OriginalValues.PropertyNames)
                {
                    var originalValue = change.OriginalValues[prop] != null ? change.OriginalValues[prop].ToString() : null;
                    var currentValue = change.CurrentValues[prop] != null ? change.CurrentValues[prop].ToString() : null;
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
                            AuditType="M",
                            UserId = Convert.ToInt32( HttpContext.Current.Session["userid"].ToString()),
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
                var primaryKey = GetPrimaryKeyValue(change);

                foreach (var prop in change.OriginalValues.PropertyNames)
                {
                    var originalValue = change.OriginalValues[prop] != null ? change.OriginalValues[prop].ToString() : null;
                    var currentValue ="";//change.CurrentValues[prop] != null ? change.CurrentValues[prop].ToString() : null;
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
                            AuditType = "D",
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
                var entityName = entityType.Name;
                //   var primaryKey = GetPrimaryKeyValue(added);

                foreach (var prop in added.CurrentValues.PropertyNames)
                {
                    var currentValue = added.CurrentValues[prop] != null ? added.CurrentValues[prop].ToString() : null;

                    log = new AuditLog()
                    {
                        TableName = entityName,
                        PK ="",
                        ColumnName = prop,
                        OldValue = "",
                        NewValue = currentValue,
                        Date = DateTime.Now,
                        AuditType = "A",
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


    }
}
