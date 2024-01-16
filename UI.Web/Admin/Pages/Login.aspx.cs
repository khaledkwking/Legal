using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using UI.Web.Controler;

namespace UI.Web.Admin.Pages
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            LoginButton.Attributes.Add("onclick", "return chkImage();");


            if (!Page.IsPostBack)
            {
                HttpCookie authCookieUser = HttpContext.Current.Request.Cookies["LEGALUSER"];
                if (authCookieUser != null && !authCookieUser.Value.Trim().Equals("") && !authCookieUser.Value.Trim().Equals(null))
                {
                    string strUserName = authCookieUser["LEGALUSER"];
                   txtUser.Text = strUserName;  

                }


                if (Request.QueryString["inactive"] != null)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "<div class='alert alert-danger'>Error, You are not authorized to access this site!</div>";
                }

                if (Request.QueryString["out"] == null)
                {

                    if (MemberShip_Permission.isAuthenticationCookie())
                    {
                        

                        if (Request.QueryString["ReturnUrl"] != null)
                        {
                            Response.Redirect(Request.QueryString["ReturnUrl"]);
                        }
                        else
                        { Response.Redirect(Resources.Utilities.cutureRoute + "/Admin/Pages/Home.aspx"); }

                    }
                }
                else {
                    //logout , Clear all sessions
                    HttpCookie authCookie = null;
                    authCookie = HttpContext.Current.Request.Cookies["CMGSPORTAL"];
                    if (((authCookie != null)))
                    {
                        authCookie.Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(authCookie);
                    }
                }
            }
        }



        protected void LoginButton_Click(object sender, EventArgs e)
        {

            bool blnIsValidUser =
            MemberShip_Permission.IsValidUser(txtUser.Text.ToLower().TrimEnd(), txtPassword.Text);
            if (blnIsValidUser)
            {
                try
                {

                    
                    string UserInfo = txtUser.Text;
                    // Create the authetication ticket
                    FormsAuthenticationTicket authTicket =
                         new FormsAuthenticationTicket(1, txtUser.Text, DateTime.Now, DateTime.Now.AddDays(1),
                                                         true, UserInfo, FormsAuthentication.FormsCookiePath);
                    FormsIdentity identitiy = new FormsIdentity(authTicket);
                    // Now encrypt the ticket.
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    // Add the encrypted ticket to the cookie collection.
                    HttpCookie authCookie = new HttpCookie("CMGSPORTAL", encryptedTicket);
                    HttpCookie authCookieUser = new HttpCookie("LEGALUSER", UserInfo);
                    //Add Cookie With User Name 
                    authCookie["CMGSPORTAL"] = UserInfo;
                    authCookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(authCookie);


                    authCookieUser.Expires = DateTime.Now.AddDays(100);
                    Response.Cookies.Add(authCookieUser);

                    lblStatus.Visible = false;
                    lblStatus.Text = "";

                   

                    var ASPCookie = Request.Cookies["ASP.NET_SessionId"];
                    ASPCookie.Expires = DateTime.Now.AddDays(30);
                    Response.SetCookie(ASPCookie);

                    //check user Permission

                }

                catch (System.Exception)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "<div class='alert alert-danger'>Error, You are not authorized to access this site!</div>";
                }

                if (Request.QueryString["ReturnUrl"] != null)
                {
                    Response.Redirect(Request.QueryString["ReturnUrl"]);
                }
                else
                { Response.Redirect(Resources.Utilities.cutureRoute + "/Admin/Pages/Home.aspx"); }

            }
            else
            {
                lblStatus.Visible = true;
                lblStatus.Text = "<div class='alert alert-danger'>Error, " + txtUser.Text + " is invalid user Or Wrong Password</div>";
            }



        }
    }
}