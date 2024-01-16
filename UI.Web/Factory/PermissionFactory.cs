using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.DAL;
using Permission.DAL;
using DomainInterface;

namespace UI.Web  
{
    public partial class PermissionFactory// : PermissionRepository 
    {
       
        private static IPermissionRepository objPermissionRepository = null;

        private static object synLock = new object();

        public static IPermissionRepository GetObject()
        {
            if (objPermissionRepository == null)
            {
                lock (synLock)
                {
                    if (objPermissionRepository == null)
                    {
                        objPermissionRepository = new PermissionRepository();
                    }

                }
            }
            return objPermissionRepository;
        }
    }
}