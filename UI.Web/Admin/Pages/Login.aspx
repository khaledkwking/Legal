<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UI.Web.Admin.Pages.Login" %>

<!DOCTYPE html>
<html lang="en">

<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<title>CMGS | الأمانة العامة لمجلس الوزراء – دولة الكويت </title>

	<!-- Global stylesheets -->
	<link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css">
	<link href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/css/icons/icomoon/styles.css" rel="stylesheet" type="text/css">
	<link href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/css/bootstrap.css" rel="stylesheet" type="text/css">
	<link href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/css/core.css" rel="stylesheet" type="text/css">
	<link href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/css/components.css" rel="stylesheet" type="text/css">
	<link href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/css/colors.css" rel="stylesheet" type="text/css">
	<!-- /global stylesheets -->

	<!-- Core JS files -->
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/loaders/pace.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/core/libraries/jquery.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/core/libraries/bootstrap.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/loaders/blockui.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/ui/nicescroll.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/ui/drilldown.js"></script>
	<!-- /core JS files -->


	<!-- Theme JS files -->
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/core/app.js"></script>
	<!-- /theme JS files -->


    
        <!--LOADING STYLESHEET FOR Dialog-->

<%--     <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/examples/public/javascript/jquery-1.7.2.js"></script>--%>
    <link rel="stylesheet" href="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/public/css/zebra_dialog.css" type="text/css">
    <link rel="stylesheet" href="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/examples/public/css/style.css" type="text/css">
    <link rel="stylesheet" href="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/examples/libraries/highlight/public/css/ir_black.css" type="text/css">


    
    <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/examples/public/javascript/jquery-1.7.2.js"></script>
    <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/examples/libraries/highlight/public/javascript/highlight.js"></script>
    <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/public/javascript/zebra_dialog.js"></script>

    
        <script type="text/javascript">
        hljs.initHighlightingOnLoad();
        </script>


    <script>


          function chkImage() {

            var txt = document.getElementById("<%=txtUser.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ،ادخل المستخدم");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtPassword.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل كلمة المرور");
                txt.focus();
                return false;
            }


 
            return true;
        }
    </script>
</head>
<body style="background:url('/Layout/images/back.png')">
    <form id="form1" runat="server" autocomplete="off">

 

	<!-- Page container -->
	<div class="page-container login-container" >

		<!-- Page content -->
		<div class="page-content">

			<!-- Main content -->
			<div class="content-wrapper">

				<!-- Simple login form -->
				<div>
					<div class="panel panel-body login-form" style="background-color: rgba(80,165,241,.25) !important;">
						<div class="text-center">
							<i class="icon-reading2"><img src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/images/logosmall.png" /></i>
							<h5 class="content-group">مرحباَ بكم في النظم الآلية الخاصة بأمانة الشئون القانونية 
                                <%--<br/><small class="display-block" style="font-size:18pآx;color:#166dba;padding-top:20px">الامانة العامة لمجلس الوزراء</small>--%>

							</h5>
						</div>

						<div class="form-group has-feedback has-feedback-left">
						 
                             <asp:TextBox ID="txtUser" runat="server"  class="form-control" autocomplete="off" placeholder="Username"></asp:TextBox>

							<div class="form-control-feedback">
								<i class="icon-user text-muted"></i>
							</div>
						</div>

						<div class="form-group has-feedback has-feedback-left">
 

                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="form-control" placeholder="Password">></asp:TextBox>

							<div class="form-control-feedback">
								<i class="icon-lock2 text-muted"></i>
							</div>
						</div>

						<div class="form-group" style="float:;text-align:left">
						 
                           

                 <asp:LinkButton ID="LoginButton" runat="server" Text="Button"   OnClick="LoginButton_Click" CssClass="btn btn-success btn-labeled"  ><b><i class="icon-person"></i></b>   تسجيل الدخول</asp:LinkButton>


 
						</div>

						<div class="text-center">
							<a href="login_password_recover.html" style="display:none">Forgot password?</a>
						</div>
<div class="text-center">


      <asp:Label ID="lblStatus" runat="server" Visible="False"></asp:Label>
</div>

					</div>
				</div>
				<!-- /simple login form -->

			</div>
			<!-- /main content -->

		</div>
		<!-- /page content -->


		<!-- Footer -->
		<div class="footer text-muted">
			<a href="http://cmgs.gov.kw/" target="_blank">الامانة العامة لمجلس الوزراء</a>- <a href="#">مركز نظم المعلومات</a>   &copy; <%=DateTime.Now.Year.ToString() %>.
		</div>
		<!-- /footer -->

	</div>
	<!-- /page container -->



      
    </form>
</body>
</html>
