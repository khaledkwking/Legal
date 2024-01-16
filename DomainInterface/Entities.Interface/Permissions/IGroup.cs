using System;
using System.Collections.Generic;
namespace DomainInterface
{
    public interface IGroup
    {
        string Description { get; set; }
        int ID { get; set; }
        bool? isActiveDir { get; set; }
        string Name { get; set; }
        int? ParentID { get; set; }
        ICollection<IPermissionPath> PermissionPaths { get; set; }
        ICollection<IUser> Users { get; set; }
        IList<int> UsersIDs { get; set; }
    }
}
