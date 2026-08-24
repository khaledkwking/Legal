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
                var context = HttpContext.Current;
                if (context == null || context.Session == null)
                {
                    return null;
                }

                return (IUser)context.Session["IUser"];
            }
            set
            {
                var context = HttpContext.Current;
                if (context == null || context.Session == null)
                {
                    return;
                }

                context.Session["IUser"] = value;
            }
        }
    }
}
