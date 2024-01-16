using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainInterface;
using Permission.DAL.Repository;

namespace UI.Web.Controler
{
    public class MemberShipConstantUI
    {
        public static Security_pr_admin CurrentUser
        {
            get
            {
                return (Security_pr_admin)HttpContext.Current.Session["IMemberShip"];
            }
            set
            {
                HttpContext.Current.Session["IMemberShip"] = value;
            }
        }
    }
}