using System;
using System.Collections.Generic;
namespace DomainInterface
{
   public interface IPermission
    {
        int ID { get; set; }
        string Type { get; set; }
        ICollection<IPermissionPath> PermissionPaths { get; set; }
    }
}
