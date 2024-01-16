<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="UI.Web._default" %>

<!DOCTYPE html>
<html lang="en">

 
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<title>GCS Centeral Dashboard</title>

	<!-- Global stylesheets -->
	<link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css">
	<link href="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/css/icons/icomoon/styles.css" rel="stylesheet" type="text/css">
	<link href="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/css/bootstrap.css" rel="stylesheet" type="text/css">
	<link href="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/css/core.css" rel="stylesheet" type="text/css">
	<link href="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/css/components.css" rel="stylesheet" type="text/css">
	<link href="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/css/colors.css" rel="stylesheet" type="text/css">
	<!-- /global stylesheets -->

	<!-- Core JS files -->
	<script type="text/javascript" src="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/js/plugins/loaders/pace.min.js"></script>
	<script type="text/javascript" src="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/js/core/libraries/jquery.min.js"></script>
	<script type="text/javascript" src="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/js/core/libraries/bootstrap.min.js"></script>
	<script type="text/javascript" src="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/js/plugins/loaders/blockui.min.js"></script>
	<script type="text/javascript" src="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/js/plugins/ui/nicescroll.min.js"></script>
	<script type="text/javascript" src="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/js/plugins/ui/drilldown.js"></script>
	<!-- /core JS files -->


	<!-- Theme JS files -->
	<script type="text/javascript" src="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/js/core/app.js"></script>
	<!-- /theme JS files -->

</head>
<body>
    <form id="form1" runat="server">
   	<!-- Main navbar -->
	<div class="navbar navbar-inverse">
		<div class="navbar-header">
			 <img src="<%=ConfigurationSettings.AppSettings["resourcesURL"]%>assets/images/GCS_Logo_small.png" alt=""> 
 
		</div>

		 
	</div>
	<!-- /main navbar -->


	<!-- Page container -->
	<div class="page-container login-container">

		<!-- Page content -->
		<div class="page-content">

			<!-- Main content -->
			<div class="content-wrapper">

				<!-- Simple login form -->
				<div>
					<div class="panel panel-body login-form">
						<div class="text-center">
							<div class="icon-object border-slate-300 text-slate-300"><i class="icon-reading"></i></div>
							<h5 class="content-group">Login to your account <small class="display-block">Enter your credentials below</small></h5>
						</div>

						<div class="form-group has-feedback has-feedback-left">
							<input type="text" class="form-control" placeholder="Username">
							<div class="form-control-feedback">
								<i class="icon-user text-muted"></i>
							</div>
						</div>

						<div class="form-group has-feedback has-feedback-left">
							<input type="text" class="form-control" placeholder="Password">
							<div class="form-control-feedback">
								<i class="icon-lock2 text-muted"></i>
							</div>
						</div>

						<div class="form-group">
							<button type="submit" class="btn btn-primary btn-block">Sign in <i class="icon-circle-right2 position-right"></i></button>
						</div>

						<div class="text-center">
							<a href="login_password_recover.html">Forgot password?</a>
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
			&copy; <%=DateTime.Now.Year.ToString() %>. <a href="#">Centeral Dashboard</a> by <a href="http://www.cmgs.gov.kw/" target="_blank">GCS - GLOBAL CLEARINGHOUSE SYSTEM</a>
		</div>
		<!-- /footer -->

	</div>
	<!-- /page container -->




</form>
</body>
</html>
