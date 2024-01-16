<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="UI.Web.Admin.Pages.AccessDenied" %>

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

</head>
<body>
   <form id="form1" runat="server">


        


	<!-- Page container -->
	<div class="page-container login-container" style="margin-top:10%">

		<!-- Page content -->
		<div class="page-content">

			<!-- Main content -->
			<div class="content-wrapper">

				<!-- Simple login form -->
				<div>
					<div class="panel panel-body login-form">
						<div class="text-center">
							<div class="icon-object border-slate-300 text-slate-300">
                                
                                <i class="">
                                    <img src="/Layout/Assets/img/no-access.png" />
                                </i></div>
							<h5 class="content-group"> Access Denied <small class="display-block">Authorization violation</small></h5>
						</div>

						<div class="form-group has-feedback has-feedback-left">
            <div class="email">
                You are not authorized to access this page, your session has been terminated to re-login
                            please click login.
            </div>
            

        </div>





        <div class="text-center">
            <asp:LinkButton runat="server" ID="lnkGo" class="form-control" OnClick="lnkGo_Click">Login</asp:LinkButton>
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
				&copy; <%=DateTime.Now.Year.ToString() %>.<a href="http://cmgs.gov.kw/" target="_blank">الامانة العامة لمجلس الوزراء</a>- <a href="#">مركز المعلومات</a>   
		</div>
		<!-- /footer -->

	</div>
	<!-- /page container -->



      
    </form>
</body>
</html>
