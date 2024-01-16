using System;
namespace DomainInterface
{
   public  interface IPermissionPath
    {
        IGroup Group { get; set; }
        int? GroupID { get; set; }
        //System.Data.Objects.DataClasses.EntityReference<Infrastructure.DAL.Group> GroupReference { get; set; }
        int ID { get; set; }
        int ObjectID { get; set; }
        int ObjectType { get; set; }
        IPermission Permission { get; set; }
        int PermissionID { get; set; }
        //System.Data.Objects.DataClasses.EntityReference<Infrastructure.DAL.Permission> PermissionReference { get; set; }
        IUser User { get; set; }
        int? UserID { get; set; }
        //System.Data.Objects.DataClasses.EntityReference<Infrastructure.DAL.User> UserReference { get; set; }
    }
}
