using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainInterface;

namespace UI.Web
{
    public class ConstantUI
    {
        public static IUser CurrentUser
        {
            get
            {
                return (IUser)HttpContext.Current.Session["IUser"];
            }
            set
            {
                HttpContext.Current.Session["IUser"] = value;
            }
        }
    }
}
