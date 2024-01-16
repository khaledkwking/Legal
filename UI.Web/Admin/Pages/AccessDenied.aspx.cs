using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UI.Web.Admin.Pages
{
    public partial class AccessDenied : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (Request.QueryString["ReturnUrl"] == null)
            //{B
            //    Response.Redirect("~/admin/pages/login.aspx");
            //} else
            //{ Response.Redirect("~/admin/pages/login.aspx?ReturnUrl=" + Request.QueryString["ReturnUrl"]); }

        }

        protected void lnkGo_Click(object sender, EventArgs e)
        {

            //if (Request.QueryString["ReturnUrl"] == null)
            //{
                
                Response.Redirect("~/admin/pages/login.aspx?OUT=1");
            //}
            //else
            //{ Response.Redirect("~/admin/pages/login.aspx?ReturnUrl=" + Request.QueryString["ReturnUrl"]); }

        }
    }
}