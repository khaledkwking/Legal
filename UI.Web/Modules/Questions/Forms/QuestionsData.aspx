<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="QuestionsData.aspx.cs" Inherits="UI.Web.Modules.Questions.Forms.QuestionsData" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
	Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Src="~/UserControls/DeleteConfirm.ascx"  TagPrefix="uc"  TagName="DeleteConfirm" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">



	<style type="text/css">
		label {
			font-size: 14px;
		}

		.grdHead {
			background-color: #FFFFFF;
			color: #000000;
			/*font-family:Tahoma;
		font-size:11px;*/
			font-weight: bold;
		}

		.grdFoot {
			background-color: #666666;
			color: #FFFFFF;
			/*font-family:Tahoma;
		font-size:11px;*/
			font-weight: bold;
			border-top: solid 2px #000000;
		}

		.grd td {
			border-left: solid 1px #000000;
			border-right: solid 1px #000000;
			border-top: solid 0px #000000;
			border-bottom: solid 0px #000000;
		}

		.grdFoot td {
			border: solid 1px #666666;
		}

		.grdHead td {
			border: solid 1px #000000;
		}

		.grd {
			border-color: #000000;
			/*font-family:Tahoma;*/
			border: solid 1px #000000;
		}

		.grdPager /* Disabled Pager Style */ {
			background-color: #EBE9E9;
			color: #AAAAAA;
			/*font-family:Tahoma;
		font-size:11px;*/
			font-weight: bold;
		}



		.grdItem {
			/*font-family:Tahoma ;
		font-size:11px;*/
			color: #555555;
			border-color: #000000;
			border-left: solid 1px #000000;
			border-right: solid 1px #000000;
			border-top: solid 0px #000000;
			border-bottom: solid 0px #000000;
			background: #ffffff;
		}

			.grdItem td {
				border-left: solid 1px #000000;
				border-right: solid 1px #000000;
				border-top: solid 0px #000000;
				border-bottom: solid 0px #000000;
			}
	</style>
	<script>


		//var selectedPersonToValues = new Array();
		//selectedPersonToValues = [2, 3];
		//function CheckSelected() {



		//    // alert("Selected value is: " + $(".multiselect-filtering").val());
		//    //  alert(selectedPersonToValues);
		//    //$(".multiselect-filtering").val(selectedPersonToValues);
		//    $('#multiselect-filtering').select2('val', selectedPersonToValues);
		//    return false;
		//}

		function setselctedRequestedFrom() {
			document.getElementById("<%=hdnfilterRequestedFrom.ClientID %>").value = $("#<%=lstFilterPerson.ClientID %>").val();

		}

		function ValidateHeading() {
		  <%--  var txt = document.getElementById("<%=chkHasAnswer.ClientID %>")
			if (txt.checked) {
				document.getElementById("Answerdatecontainer").style.display = '';
			} else {
				document.getElementById("Answerdatecontainer").style.display = 'none';
			}--%>


		}
		function ControlGrid(imgName, rowIndex, rowID) {
			//alert("CONTROL GRID");
			// alert(imgName);
			// alert(rowIndex);
			//alert(rowID);
			rowIndex = rowIndex + 3;

			var myrow = "";
			if (rowIndex < 10)
				myrow = "ctl00_Main_grdQuestionsList_ctl0" + rowIndex;
			else
				myrow = "ctl00_Main_grdQuestionsList_ctl" + rowIndex;
			var row = document.getElementById(myrow);
			//  alert("IMG NAME: "+imgName+" and ROW INDEX: "+rowIndex+" ID: "+rowID);
			//alert("MYROW: "+myrow+" AND VALUE FOUND: "+row);
			if (row.style.display == "") {
				row.style.display = "none";
				document.getElementById(imgName).src = plus.src;
			}
			else {
				row.style.display = "";
				document.getElementById(imgName).src = minus.src;
			}
		}
		function Checklist(obj, list) {
			//  alert("CHECK SYSTEM: "+obj.checked);
			if (list != "") {
				var data = list.split(",");
				//alert("LIST IS: "+data.length)
				for (var i = 0; i < data.length; i++) {
					document.getElementById(data[i]).checked = obj.checked;
				}
			}
		}
		function ValidateAnswer() {
			var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ،احفظ بيانات السؤال اولا ");
				return false;
			}

			var txt = document.getElementById("<%=txtAnswerDate.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("   فضلا ، ادخل تاريخ الاجابة ");
				txt.focus();
				return false;
			}


			return true


		}

		function ValidateProcesdureadd() {
			var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ،احفظ بيانات الوسام اولا ");
				txt.focus();
				return false;
			}
			return true


		}

		function chkImage() {



<%--            var txt = document.getElementById("<%=txtQuestionInternalSerial.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، ادخل  رقم السؤال السؤال");
				txt.focus();
				return false;
			}--%>


			var txt = document.getElementById("<%=txtOma_Serial.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، ادخل رقم الصادر بملس الامة ");
				txt.focus();
				return false;
			}

		<%--    var txt = document.getElementById("<%=txtQoOmaYear.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، ادخل السنة");
				txt.focus();
				return false;
			}--%>

			var txt = document.getElementById("<%=txtQuestionDate.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ،  اختر تاريخ السؤال ");
				txt.focus();
				return false;
			}


			var txt = document.getElementById("<%=lstRequestTo.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ، اختر موجة إلى ");
				txt.focus();
				return false;
			}


			var txt = document.getElementById("<%=lstChapter.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ، اختر الفصل التشريعي  ");
				txt.focus();
				return false;
			}

			var txt = document.getElementById("<%=lstSession.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ، اختر  دور الانعقاد ");
				txt.focus();
				return false;
			}

	   <%--     var txt = document.getElementById("<%=lstassignedPersons.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ، اختر الموظف المختص");
				txt.focus();
				return false;
			}--%>

			var txt = document.getElementById("<%=lstStatus.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ، اختر الحاله ");
				txt.focus();
				return false;
			}
			var txt = document.getElementById("<%=txtQuestionSubject.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، ادخل  نص السؤال ");
				txt.focus();
				return false;
			}

			var lstRequestedFromPareon = document.getElementById("<%=lstRequestFrom.ClientID %>");// getObjById("lstRequestFrom").value;
			if (lstRequestedFromPareon.value == "" || lstRequestedFromPareon.value == "0") {
				new $.Zebra_Dialog("فضلا ، يرجي اضافة الشخص الموجة من");
				return false;
			}

			//var lstRequestedFromOrg = getObjById("lstRelatedOrgs").value;
			//if (lstRequestedFromOrg != "0") {
			//    new $.Zebra_Dialog("فضلا ، يرجي اضافة الجهة المعنية  ");
			//    return false;
			//}


			//Get selected Value
		   // alert($("#<%=lstRelatedOrgs.ClientID %>").val());

			document.getElementById("<%=hdnRelatedOrg.ClientID %>").value = $("#<%=lstRelatedOrgs.ClientID %>").val();

			return true;

		}

		function LinkAddClick() {
			// alert("in");

			return InsertItem();
		}

		function relatedOrgClick() {
			// alert("in");

			return ValidatedRelatedOrgs();
		}

		function ValidatedRelatedOrgs() {

			var lstRelatedOrgs = getObjById("lstRelatedOrgs").value;
			if (lstRelatedOrgs == "" || lstRelatedOrgs == "0") {
				new $.Zebra_Dialog("فضلا ،اختر الجهة المعنية ");
				return false;
			}
		}

		function InsertItem() {
			var lstRequestedFromPareon = getObjById("lstRequestedFromPareon").value;

			if (lstRequestedFromPareon == "" || lstRequestedFromPareon == "0") {
				new $.Zebra_Dialog("فضلا ،اختر موجة من    ");
				return false;
			}
		}

		function ValidateIncoming() {

			var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ،احفظ بيانات السؤال اولا ");
				return false;
			}

			var txt = document.getElementById("<%=txtDoc_Serial.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، ادخل رقم الوثيقة     ");
				txt.focus();
				return false;
			}
			var txt = document.getElementById("<%=lstIncomingOrg.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ، ادخل وارد من ");
				txt.focus();
				return false;
			}
			var txt = document.getElementById("<%=txtDoc_Subject.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، ادخل  الموضوع ");
				txt.focus();
				return false;
			}

			var txt = document.getElementById("<%=txtComingDate.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، ادخل  تاريخ الوثيقه ");
				txt.focus();
				return false;
			}

		}


		function Validateoutgoing() {

			var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ،احفظ بيانات السؤال اولا ");
				return false;
			}
			var txt = document.getElementById("<%=txtOutDocNo.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، ادخل رقم الوثيقة ");
				txt.focus();
				return false;
			}
			var txt = document.getElementById("<%=lstoutgoiningOrg.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ، ادخل صادر إلى ");
				txt.focus();
				return false;
			}
			var txt = document.getElementById("<%=txtOutSubject.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، ادخل  الموضوع ");
				txt.focus();
				return false;
			}

			var txt = document.getElementById("<%=txtoutDate.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، ادخل  تاريخ الوثيقه ");
				txt.focus();
				return false;
			}

		}
	  <%--  function validateFileAttachment() {
			var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
			if (txt.value == "" || txt.value == "0") {
				new $.Zebra_Dialog("فضلا ،احفظ بيانات السؤال اولا ");
				return false;
			}

			var txt = document.getElementById("<%=lstAttachmentType.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، اختر نوع المرفق ");
				txt.focus();
				return false;
			}
			var txt = document.getElementById("<%=txtAttachCreationDate.ClientID %>")
			  if (txt.value == "") {
				  new $.Zebra_Dialog("فضلا ، ادخل تاريخ الوثيقة  ");
				  txt.focus();
				  return false;
			  }
			  var txt = document.getElementById("<%=txtAttachRef.ClientID %>")
			if (txt.value == "") {
				new $.Zebra_Dialog("فضلا ، ادخل  المرجع ");
				txt.focus();
				return false;
			}

			return true;

		}--%>

		function setactiveTab(tabindex) {
			//alert("para:" + tabindex)
			var txt = document.getElementById("<%=hdnactivetab.ClientID %>");
			txt.value = tabindex;

		}


		function showscannerLoading() {

			if (ValidateAnswer()) {
				document.getElementById("scanLoading").style.display = "";
				return true;
			} else { return false; }
		}



		$(document).ready(function () {
			var selectedCodeWBs = [<%=QrelatedOrg%>];


			 //   $('#<%=lstRelatedOrgs.ClientID %>').select2().val(selectedCodeWBs).change();

			$('#<%=lstRelatedOrgs.ClientID %>').val(selectedCodeWBs).select2();


		});


	</script>

	<input id="hdnQScannerfilepath" runat="server" type="hidden" />
	<input id="hdnAnswerScannerfilepath" runat="server" type="hidden" />

	<input id="hdnIncomingScannerfilepath" runat="server" type="hidden" />
	<input id="hdnOutgoingScannerfilepath" runat="server" type="hidden" />

	<input id="hdnfilterRequestedFrom" runat="server" type="hidden" />
	<div class="row">
		<div class="col-lg-12">
			<div class="col-lg-8">
				<div class="page-title2">
					<h4>
						<i class="icon-grid position-left"></i>
						نظام الأسئلة البرلمانية
					</h4>
				</div>

			</div>

		</div>
	</div>



	<asp:UpdatePanel runat="server" ID="Updatepanel1" ChildrenAsTriggers="true" UpdateMode="conditional">
		<ContentTemplate>
		</ContentTemplate>
	</asp:UpdatePanel>
	<!--END TITLE & BREADCRUMB PAGE-->
	<!--BEGIN CONTENT-->

	<div class="row mbl" id="tblSearch" style="min-height: 450px;" runat="server">

		<div class="panel panel-flat">
			<div class="panel-heading">
				<div class="position-right" style="float: left; margin-left: 30px">

					<asp:LinkButton runat="server" ID="btnNew" class="btn btn-success btn-xs" OnClick="btnNew_Click1"><i class="fa fa-plus"></i>&nbsp; إضافة سؤال جديد&nbsp;</asp:LinkButton>

					<asp:LinkButton OnClientClick="return checkDelete();" runat="server" Visible="false" ID="btnDelete" class="btn btn-danger btn-xs" OnClick="btnDelete_Click"><i class="fa fa-times"></i>&nbsp;Delete Selected Data</asp:LinkButton>

				</div>
				<h5 class="panel-title">نطاق البحث</h5>



				<div class="panel-body">
					<div class="form-horizontal">

						<asp:Label runat="server" ID="lblerror"></asp:Label>

						<fieldset class="content-group">
							<legend class="text-semibold">
								<i class="icon-file-text2 position-left"></i>
								ادخل شروط البحث
											<%--<a class="control-arrow" data-toggle="collapse" data-target="#demo1">
												<i class="icon-circle-down2"></i>
											</a>--%>
							</legend>




							<div class="col-md-4">
								<div class="form-group" style="display: none">
									<label class="col-lg-3 control-label">
										رقم السؤال :</label>
									<div class="col-lg-9">
										<asp:TextBox ID="txtFilterserial" runat="server" class="form-control"></asp:TextBox>
									</div>
								</div>


								<div class="form-group">
									<label class="col-lg-3 control-label">تاريخ  السؤال من :</label>
									<div class="col-lg-9">
										<div class="input-group">
											<span class="input-group-addon"><i class="icon-calendar22"></i></span>
											<%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
											<asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
										</div>



									</div>
								</div>


								<div class="form-group">
									<label class="col-lg-3 control-label">
										رقم الصادر بمجلس الامة:

									</label>
									<div class="col-lg-3">
										<asp:TextBox ID="txtFilterInternalSerial" runat="server" class="form-control"></asp:TextBox>
									</div>
									<label class="col-lg-2 control-label">
										السنة:

									</label>
									<div class="col-lg-3">
										<asp:TextBox ID="txtFilterFileYear" runat="server" class="form-control"></asp:TextBox>
									</div>


								</div>


								<div class="form-group">
									<label class="col-lg-3 control-label">
										جزء من نص السؤال :
									</label>
									<div class="col-lg-9 autoDrop">
										<asp:TextBox ID="txtFilterSubject" runat="server" class="form-control"></asp:TextBox>

										<%--  <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1"
											runat="server" TargetControlID="txtFilterSubject"
											CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
											ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />--%>
									</div>



								</div>

								<div class="form-group">
									<label class="col-md-3 control-label" for="">سؤال موحد: </label>

									<div class="col-md-9">

										<asp:CheckBox ID="chkFilerIsGroup" runat="server" />


									</div>
								</div>

 
							</div>

							<div class="col-md-4">
								<div class="form-group">
									<label class="col-lg-3 control-label">الفصل التشريعي :</label>
									<div class="col-lg-9">
										<asp:DropDownList ID="lstFilterChapter" AutoPostBack="true" class="Select2Drop" runat="server" OnSelectedIndexChanged="lstFilterChapter_SelectedIndexChanged"></asp:DropDownList>
									</div>
								</div>

								<div class="form-group">
									<label class="col-lg-3 control-label">إلى  :</label>
									<div class="col-lg-9">

										<div class="input-group">
											<span class="input-group-addon"><i class="icon-calendar22"></i></span>
											<asp:TextBox ID="txtFilterDateTo" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>

										</div>

									</div>
								</div>
								<div class="form-group">
									<label class="col-lg-3 control-label">
										موجه من :
									</label>
									<div class="col-lg-9 autoDrop">
										<%--<asp:TextBox ID="txtFilterPersonName" runat="server" class="form-control"></asp:TextBox>--%>
										<asp:DropDownList ID="lstFilterPerson" class="form-control multiselect-filtering" multiple="multiple" Width="100%" runat="server"></asp:DropDownList>
									</div>
								</div>
								<div class="form-group">
									<label class="col-lg-3 control-label">
										الجهات المعنيه   :
									</label>
									<div class="col-lg-9 autoDrop">
										<%--<asp:TextBox ID="txtFilterOrg" runat="server" class="form-control"></asp:TextBox>--%>

										<asp:DropDownList ID="lstFilterRelatedOrgs" class="Select2Drop" runat="server"></asp:DropDownList>

									</div>
								</div>
							</div>

							<div class="col-md-4">
								<div class="form-group">
									<label class="col-lg-3 control-label">
										دور الانعقاد:</label>
									<div class="col-lg-9">
										<asp:DropDownList ID="lstFilterSession" class="Select2Drop" runat="server"></asp:DropDownList>
									</div>
								</div>
								<div class="form-group">
									<label class="col-lg-3 control-label">الإجابة :</label>
									<div class="col-lg-9">
										<asp:DropDownList ID="lstFilterStatus" class="Select2Drop" runat="server"></asp:DropDownList>
									</div>
								</div>
								<div class="form-group">
									<label class="col-lg-3 control-label">موجة إلى :</label>
									<div class="col-lg-9">
										<asp:DropDownList ID="lstFilterRequestTo" class="Select2Drop" runat="server"></asp:DropDownList>
									</div>
								</div>
								<div class="form-group" style="display:none">
									<label class="col-lg-3 control-label">الموظف المختص :</label>
									<div class="col-lg-9">
										<asp:DropDownList ID="lstFilterAssignedPerson" class="Select2Drop" runat="server"></asp:DropDownList>
									</div>
								</div>


							</div>


						</fieldset>
					</div>
					<div class="text-right">
						<asp:LinkButton ID="lnkSearch" OnClientClick="setselctedRequestedFrom();" class="btn btn-primary" runat="server" OnClick="lnkSearch_Click">&nbsp;&nbsp; بحث&nbsp;&nbsp; <i class="icon-search4 position-right"></i></asp:LinkButton>
					</div>

				</div>
			</div>


		</div>

	</div>
	<div class="row mbl">

		<div class="panel" id="tblshow" runat="server" visible="false">

			<div class="panel-body">
				<div class="row">

					<div class="panel-body">
						<div class="form-horizontal">

							<div class="col-lg-12">
								<div class="portlet box">
									<div class="portlet-header">

										<div class="portlet-body">
											<fieldset class="content-group">
												<legend class="text-semibold">
													<i class="icon-file-text2 position-left"></i>

													نتيجة البحث  <span style="color: #000; font-size: 14px;">(
														<asp:Label ID="lblSearchResultCount" runat="server"></asp:Label>)</span>


													<asp:LinkButton ID="lnkSearchback" CssClass="control-arrow" runat="server" OnClick="lnkSearchback_Click"> رجوع  <i class=" icon-backward2"></i></asp:LinkButton>

												</legend>




												<div class="datatable-scroll">


													<asp:DataGrid ID="grdQuestionsList" runat="server"
														DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="40" class="table datatable-basic dataTable no-footer"
														Width="100%" OnItemDataBound="grdQuestionsList_ItemDataBound" OnItemCommand="grdQuestionsList_ItemCommand">
														<SelectedItemStyle ForeColor="White" />
														<ItemStyle CssClass="grdItem" />
														<AlternatingItemStyle CssClass="grdItem" />
														<PagerStyle Visible="false" />
														<HeaderStyle CssClass="grdHead" BackColor="Black" ForeColor="White" Font-Bold="False" />
														<Columns>
															<asp:ButtonColumn HeaderText="Del." Text="<img border=0 src='images/delete.gif' alt='Delete'>" CommandName="Delete" Visible="false">
																<HeaderStyle></HeaderStyle>
																<ItemStyle HorizontalAlign="center" />
															</asp:ButtonColumn>
															<asp:TemplateColumn HeaderText="">
																<ItemStyle HorizontalAlign="Right" BackColor="#EEF0FA" />
																<ItemTemplate>

																	<div runat="server" id="divAnswer" class="panel panel-flat border-top-info border-bottom-info" style="border: 2px solid transparent; border-top-color: #40EE27  !important; border-bottom-color: #40EE27 !important; padding: 20px;">

																		<asp:Label Font-Bold="true" runat="server" ID="Label7" CssClass="black_Lable">
																					نص الاجابة
																		</asp:Label>
																		<asp:DataGrid ID="grdAnswers" runat="server"
																			class="table table-hover table-striped table-bordered table-advanced tablesorter"
																			AutoGenerateColumns="False"
																			BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
																			CellPadding="3" Width="100%" OnItemDataBound="grdUnits_ItemDataBound">
																			<SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
																			<ItemStyle CssClass="grdItem" />
																			<AlternatingItemStyle CssClass="grdItem" />
																			<HeaderStyle CssClass="grdHead" BackColor="Gray" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
																			<FooterStyle CssClass="grdFoot" />
																			<PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
																				PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
																			<Columns>
																				<asp:BoundColumn DataField="QuestionID" Visible="False"></asp:BoundColumn>
																				<asp:BoundColumn DataField="AnswerDate" HeaderText="تاريخ الاجابة " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
																				<asp:BoundColumn DataField="LastActionDate" HeaderText="اخر تحديث  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
																				<asp:BoundColumn DataField="AnswerText" HeaderText="نص الاجابة"></asp:BoundColumn>
																				<asp:TemplateColumn HeaderText="مرفقات">
																					<ItemStyle HorizontalAlign="Center" Width="5%" />
																					<HeaderStyle HorizontalAlign="Center" />
																					<ItemTemplate>
																						<a class="label border-left-primary label-striped iframe" target="_blank" href="<%# ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(Eval("QuestionID"))+"/answers/"+gets(Eval("Code"))+"/" + "&vfileList=["  + gets(Eval("Answerattachments"))+";]" %>" style="<%#showattachment(gets(Eval("Answerattachments")))%>">
																							<i class="icon-attachment"></i>&nbsp;
																							  نص الاجابة
																						</a>

																					</ItemTemplate>
																				</asp:TemplateColumn>






																			</Columns>
																		</asp:DataGrid>

																	</div>


																	<div class="panel panel-flat border-top-info border-bottom-info" style="border: 2px solid transparent; border-top-color: #00BCD4 !important; border-bottom-color: #00BCD4 !important; padding: 20px;" runat="server" id="divFollow">

																		<asp:Label Font-Bold="true" runat="server" ID="Label4" CssClass="black_Lable">
																					كتب المتابعه والردود
																		</asp:Label>
																		<asp:DataGrid runat="server" ID="grdFollow" AutoGenerateColumns="False"
																			AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemCommand="grdincoming_ItemCommand" OnEditCommand="grdincoming_EditCommand">
																			<PagerStyle Visible="False" />
																			<HeaderStyle BackColor="#efefef" Font-Bold="True" />
																			<Columns>
																				<asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>

																				<asp:TemplateColumn HeaderText="النوع">
																					<ItemStyle Width="3%" />
																					<ItemTemplate>
																						<%#fillDocType(gets(Eval("Doc_Type"))) %>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:BoundColumn DataField="Doc_Serial" HeaderText="رقم الكتاب"></asp:BoundColumn>
																				<asp:TemplateColumn HeaderText="الجهة">

																					<ItemTemplate>
																						<%# gets(Eval("Doc_Type")) =="1" ? gets(Eval("Doc_From")) :gets(Eval("Doc_To"))  %>
																					</ItemTemplate>
																				</asp:TemplateColumn>

																				<asp:BoundColumn DataField="SentDate" HeaderText="التاريخ  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
																				<asp:BoundColumn DataField="Doc_Subject" HeaderText="الموضوع "></asp:BoundColumn>
																				<asp:BoundColumn DataField="Doc_Notes" HeaderText="ملاحظات "></asp:BoundColumn>
																				<asp:BoundColumn DataField="NextFollowReminderDate" HeaderText="تاريخ التنبيه  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>


																				<asp:TemplateColumn HeaderText="عرض المرفق">
																					<ItemStyle HorizontalAlign="Center" Width="10%" />
																					<HeaderStyle HorizontalAlign="Center" />
																					<ItemTemplate>

																						<a class="label border-left-primary label-striped iframe" target="_blank" href="<%#  ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath +gets(Eval("RefDocID"))+"/"+ fillDocTypefolder(gets(Eval("Doc_Type")))  +"/"+ gets(Eval("Code"))+"/"+ "&vfileList=[" + gets(Eval("Filepath")) +";]"%>" style="<%#showattachment(gets(Eval("Filepath")))%>">
																							<i class="icon-attachment"></i>&nbsp; عرض المرفق


																						</a>



																					</ItemTemplate>
																				</asp:TemplateColumn>





																			</Columns>
																		</asp:DataGrid>

																	</div>

																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="">
																<ItemStyle HorizontalAlign="center" />
																<ItemTemplate>
																	<img style="cursor: pointer;" src="/layout/images/plus.gif" alt="" border="0" runat="server" id="imgControl" />
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn Visible="false" HeaderText="Code" DataField="code"></asp:BoundColumn>
															<asp:BoundColumn Visible="false" HeaderText="Q_RequestTo" DataField="Q_RequestTo"></asp:BoundColumn>
															<asp:BoundColumn Visible="false" HeaderText="StatusID" DataField="StatusID"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="رقم السؤال " DataField="Q_Serial" Visible="false"></asp:BoundColumn>
															<%-- <asp:TemplateColumn HeaderText="  رقم السؤال ">
																<ItemStyle HorizontalAlign="Center" />
																<HeaderStyle Wrap="False" HorizontalAlign="Center" />

																<ItemTemplate>
																	<%#Eval("Oma_Serial") %>/  <%#Eval("OmaYear") %>
																</ItemTemplate>
															</asp:TemplateColumn>--%>

															<asp:BoundColumn DataField="Q_Date" HeaderText="تاريخ السؤال" DataFormatString="{0:dd/MM/yyyy}">
																<HeaderStyle Wrap="false" />
															</asp:BoundColumn>

															<asp:BoundColumn DataField="Q_Text" HeaderText="السؤال">
																<HeaderStyle Wrap="false" />
																<ItemStyle Width="30%" />
															</asp:BoundColumn>
															<asp:BoundColumn DataField="RequestedFromNameAr" HeaderText="موجه من  ">
																<HeaderStyle Wrap="false" />
															</asp:BoundColumn>
															<asp:BoundColumn DataField="Q_RequestToNameAr" HeaderText="موجه إلى  ">
																<HeaderStyle Wrap="false" />
															</asp:BoundColumn>
															<asp:BoundColumn DataField="RelatedOrgs" HeaderText="الجهات المعنية   ">
																<HeaderStyle Wrap="false" />
															</asp:BoundColumn>

															<asp:BoundColumn DataField="ChapterNameAr" HeaderText="الفصل التشريعي">
																<HeaderStyle Wrap="false" />
															</asp:BoundColumn>


															<asp:BoundColumn DataField="SessionNameAr" HeaderText="دور الانعقاد ">
																<HeaderStyle Wrap="false" />
															</asp:BoundColumn>

															<asp:BoundColumn DataField="Oma_Serial" HeaderText="الصادر بمجلس الامة ">
																<HeaderStyle Wrap="false" />
															</asp:BoundColumn>


															<%--                                                            <asp:BoundColumn DataField="LastActionDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="تاريخ اخر تحديث ">
																<HeaderStyle Wrap="false" />
																<ItemStyle HorizontalAlign="Center" />
															</asp:BoundColumn>--%>

															<asp:TemplateColumn HeaderText="الحالة   ">
																<ItemStyle HorizontalAlign="left" />
																<HeaderStyle Wrap="False" HorizontalAlign="left" />

																<ItemTemplate>
																	<%# (GetQStatus(ZeroIntergerIFNull(Eval("StatusID").ToString()),gets(Eval("StatusNameAr")))) %>
																	<div style="margin: 0 auto; margin-top: 10px;">
																		<%# ( gets(Eval("StatusID"))=="4"? GetLastFollow(gets(Eval("Code"))):"") %>
																	</div>
																</ItemTemplate>
															</asp:TemplateColumn>



															<asp:TemplateColumn HeaderText="التفاصيل">
																<ItemStyle HorizontalAlign="right" Width="5%" />
																<HeaderStyle HorizontalAlign="Center" />
																<ItemTemplate>
																	<div style="text-align: right">

																		<a href="<%#ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath  +gets(Eval("Code"))+"/"+ "&vfileList=[" + gets(Eval("Q_Attachment"))+";]"%>" style="font-size: 12px; <%#showattachment(gets(Eval("Q_Attachment")))%>" class="label border-left-primary label-striped iframe">
																			<i class="icon-attachment"></i>&nbsp;
																		  نص السؤال
																		</a>
																		<div style="margin-top: 10px;">
																			<a class="label border-left-warning label-striped iframe" id="lnkAnswer" runat="server" target="_blank" style="padding-top: 10px;">
																				<i class="icon-attachment"></i>&nbsp;
																		  نص الاجابة
																			</a>
																		</div>


																	</div>
																	<div style="margin-top: 10px;">
																		<a href="QuestionsData.aspx?QuestionID=<%#Eval("Code")%>&editflag=1" class="label border-left-success label-striped" style="font-size: 12px;">
																			<i class="fa fa-file"></i>&nbsp;
																						   التفاصيل
																		</a>
																	</div>

																</ItemTemplate>
															</asp:TemplateColumn>


														 <asp:TemplateColumn>
														   <ItemStyle Width="5%" HorizontalAlign="Center" />
														   <HeaderStyle Wrap="False" HorizontalAlign="Center" />
														   <ItemTemplate>
															   <%--<asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('are you sure you want to delete selected items?');" CommandName="delete" runat="server">  <i class="fa fa-trash" style="color:##333"></i>&nbsp;</asp:LinkButton>--%>
														 <asp:LinkButton
															  ID="lnkDelete"
															  OnClientClick="return DeleteConfirm.show(this);"
															  CommandName="delete"
															  runat="server">
															  <i class="fa fa-trash" style="color:#333"></i>&nbsp;
														  </asp:LinkButton>
														 </ItemTemplate>
													   </asp:TemplateColumn>

														</Columns>
													</asp:DataGrid>
												</div>

												<div class="datatable-footer">
													<div class="dataTables_info" id="DataTables_Table_3_info" role="status" aria-live="polite">
														<asp:Label ID="lblcount" runat="server"></asp:Label>
													</div>
													<div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">


														<cc1:Pager CurrentIndex="1" OnCommand="pager_Command" ShowFirstLast="False" ID="pager1"
															runat="server" Width="100%" PageSize="40" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>


													</div>
												</div>
											</fieldset>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>


				</div>

			</div>
		</div>
	</div>

	<div class="row mbl">

		<div class="panel-heading" id="tblAdd" runat="server" visible="false">
			<div class="row">
				<asp:Label runat="server" ID="lblAdderror"></asp:Label>
				<div class="panel">

					<div class="panel-body">

						<div class="tabbable">
							<ul class="nav nav-tabs nav-tabs-highlight">
								<li class="<%=activeTab(1) %>" onclick="setactiveTab(1)"><a href="#badges-tab1" data-toggle="tab"><i class="icon-menu7 position-left"></i>بيانات السؤال</a></li>
								<li class="<%=activeTab(2) %>" onclick="setactiveTab(2)"><a href="#badges-tab2" data-toggle="tab">الاجابة <span class="badge badge-success  position-right">
									<asp:Label ID="lblAnswercount" runat="server" Text="0"></asp:Label></span></a></li>
								<li class="<%=activeTab(3) %>" onclick="setactiveTab(3)"><a href="#badges-tab3" data-toggle="tab">ردود الجهات <span class="badge badge-success  position-right">
									<asp:Label ID="lblComingalert" runat="server" Text="0"></asp:Label></span></a></li>
								<li class="<%=activeTab(4) %>" onclick="setactiveTab(4)"><a href="#badges-tab4" data-toggle="tab">كتب المتابعه <span class="badge badge-success  position-right">
									<asp:Label ID="lbloutAlert" runat="server" Text="0"></asp:Label></span></a></li>
							</ul>

							<div class="tab-content">
								<div class="tab-pane <%=activeTab(1) %>" id="badges-tab1">
									<div class="col-lg-12">
										<div class="portlet box portlet-blue">
											<div class="portlet-header">
												<div class="caption">
													<asp:Label runat="server" Visible="false" ID="lblSubTitle">إضافة جديد</asp:Label>
												</div>

											</div>
											<div class="portlet-body">
												<div role="form" class="form-horizontal">
													<input id="hdnMasterID" runat="server" type="hidden" />
													<input id="hdnRelatedOrg" runat="server" type="hidden" />


													<input id="hdnactivetab" runat="server" type="hidden" />
													<fieldset class="content-group">

														<legend class="text-semibold">
															<i class="icon-file-text2 position-left"></i>
															بيانات  السؤال


														</legend>

														<div class="row">

															<div class="col-md-4">
																<div class="form-group" style="display: none">
																	<label class="col-md-4 control-label" for="">رقم السؤال : <span class="text-danger">*</span> </label>
																	<div class="col-md-8">

																		<asp:TextBox ID="txtQuestionInternalSerial" class="form-control" runat="server"></asp:TextBox>

																	</div>


																</div>


																<div class="form-group">
																	<label class="col-lg-4 control-label" style="font-size: 14px;">
																		رقم الصادر بمجلس الامة  :<span class="text-danger">*</span>

																	</label>
																	<div class="col-lg-3">
																		<asp:TextBox ID="txtOma_Serial" class="form-control" runat="server"></asp:TextBox>
																	</div>
																	<label class="col-lg-2 control-label">
																		السنة:

																	</label>
																	<div class="col-lg-2">
																		<asp:TextBox ID="txtQoOmaYear" class="form-control" runat="server"></asp:TextBox>
																	</div>


																</div>

																<div class="form-group" style="display:none">
																	<label class="col-md-4 control-label" for="">الموظف المختص:<span class="text-danger">*</span> </label>

																	<div class="col-md-8">

																		<asp:DropDownList ID="lstassignedPersons" runat="server" class="Select2Drop"></asp:DropDownList>


																	</div>
																</div>
																	<div class="form-group">
																	<label class="col-md-3 control-label" for="">موجة من : <span class="text-danger">*</span></label>

																	<div class="col-md-9">
																		<asp:DropDownList ID="lstRequestFrom" runat="server" class="Select2Drop"></asp:DropDownList>
																	</div>
																</div>

															</div>
															<div class="col-md-4">

																<div class="form-group">
																	<label class="col-md-3 control-label" for="">تاريخ السؤال:<span class="text-danger">*</span>  </label>

																	<div class="col-md-9">

																		<div class="input-group">
																			<span class="input-group-addon"><i class="icon-calendar22"></i></span>

																			<asp:TextBox ID="txtQuestionDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

																		</div>

																	</div>
																</div>

															

																<div class="form-group">
																	<label class="col-md-3 control-label" for="">موجة إلى :<span class="text-danger">*</span> </label>

																	<div class="col-md-9">

																		<asp:DropDownList ID="lstRequestTo" runat="server" class="Select2Drop"></asp:DropDownList>


																	</div>
																</div>


															</div>

															<div class="col-md-4">

																<div class="form-group">
																	<label class="col-md-3 control-label" for="">الفصل التشريعي:<span class="text-danger">*</span>   </label>

																	<div class="col-md-9">

																		<asp:DropDownList ID="lstChapter" runat="server" class="Select2Drop" OnSelectedIndexChanged="lstChapter_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>


																	</div>
																</div>

																<div class="form-group">
																	<label class="col-md-3 control-label" for="">دور الانعقاد:<span class="text-danger">*</span> </label>

																	<div class="col-md-9">

																		<asp:DropDownList ID="lstSession" AutoPostBack="false" runat="server" class="Select2Drop"></asp:DropDownList>

																	</div>
																</div>

																<div class="form-group">
																	<label class="col-md-3 control-label" for="">الإجابة: <span class="text-danger">*</span> </label>

																	<div class="col-md-9">

																		<asp:DropDownList ID="lstStatus" runat="server" class="Select2Drop"></asp:DropDownList>


																	</div>
																</div>







															</div>

														</div>

														<div class="row">
															<div class="col-md-8">


																<div class="form-group">
																	<label class="col-lg-2 control-label">
																		الجهات المعنيه :
																	</label>
																	<div class="col-lg-10">

																		<asp:DropDownList ID="lstRelatedOrgs" class="select-border-color border-warning" multiple="multiple" Width="100%" runat="server"></asp:DropDownList>
																	</div>
																</div>

																<div class="form-group">
																	<label class="col-md-2 control-label" for="">نص السؤال <span class="text-danger">*</span>:</label>

																	<div class="col-md-10 autoDrop">
																		<asp:TextBox runat="server" ID="txtQuestionSubject" class="form-control" Rows="6" TextMode="MultiLine"></asp:TextBox>
																		<%-- <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
																			runat="server" TargetControlID="txtQuestionSubject"
																			CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
																			ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />--%>
																	</div>


																</div>

																<div class="form-group">
																	<label class="col-md-2 control-label" for="">ملاحظات:</label>

																	<div class="col-md-10">

																		<asp:TextBox runat="server" TextMode="MultiLine" ID="txtQuestionNote" class="form-control"></asp:TextBox>

																	</div>
																</div>

																<div class="form-group">
																	<label class="col-md-2 control-label">ملف السؤال:</label>

																	<div class="col-md-7">
																		<asp:Label ID="lblimage" runat="server"></asp:Label>
																		<asp:FileUpload ID="txtQImage" runat="server" class="file-styled" />
																		<span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
																	</div>
																	<div class="col-md-2">
																		<span class="help-block2">| Or | </span>
																		<asp:LinkButton runat="server" ID="lnkQScan" OnClientClick="return chkImage();" OnClick="lnkQScan_Click" class="btn btn-info btn-xs"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
																	</div>


																</div>



															</div>

															<div class="col-md-4">



																<div class="form-group">
																	<label class="col-md-3 control-label" for="">سؤال موحد: </label>

																	<div class="col-md-9">

																		<asp:CheckBox ID="chkIsGrouped" runat="server" />


																	</div>
																</div>




															</div>
														</div>




														<%--                                                        <legend class="text-semibold">
															<i class="icon-file-text2 position-left"></i>
															أسماء
														</legend>--%>


														<div class="col-md-6" style="display: none">
															<asp:UpdatePanel runat="server" ID="Updatepanel2" ChildrenAsTriggers="true" UpdateMode="conditional">
																<ContentTemplate>


																	<asp:Button UseSubmitBehavior="false" runat="server" ID="btnAddNewItem" Text="Add Item" Style="display: none;" OnClick="btnAddNewItem_Click" />

																	<div class="panel-heading">
																		<h5 class="panel-title">موجه من </h5>

																	</div>

																	<asp:DataGrid ID="grdprosecutor" runat="server"
																		class="table table-hover table-striped table-bordered table-advanced tablesorter"
																		AutoGenerateColumns="False"
																		BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
																		CellPadding="3" Width="100%" OnItemDataBound="grdprosecutor_ItemDataBound" OnItemCommand="grdprosecutor_ItemCommand">
																		<SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
																		<ItemStyle CssClass="grdItem" />
																		<AlternatingItemStyle CssClass="grdItem" />
																		<HeaderStyle CssClass="grdHead" BackColor="Gray" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
																		<FooterStyle CssClass="grdFoot" />
																		<PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
																			PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
																		<Columns>

																			<asp:TemplateColumn HeaderText="">
																				<HeaderStyle BackColor="#000000"></HeaderStyle>
																				<ItemStyle Width="1%" HorizontalAlign="center" />
																				<FooterStyle HorizontalAlign="center" />
																				<ItemTemplate>
																					<asp:LinkButton runat="server" ID="lnkEdit" CssClass="btn btn-default btn-xs" CommandName="Edit"> <img src="/Layout/RTL/assets/images/EditPerson.png"  /></asp:LinkButton>
																				</ItemTemplate>
																				<EditItemTemplate>
																					<table border="0" align="center">
																						<tr>
																							<td style="border: solid 0px #FFFFFF;">
																								<asp:LinkButton runat="server" ID="lnkAdd" CssClass="btn btn-default btn-xs" Visible="false" CommandName="AddNew"><img src="/Layout/RTL/assets/images/addPerson.png" /></asp:LinkButton></td>
																							<td style="border: solid 0px #FFFFFF;">
																								<asp:LinkButton runat="server" ID="lnkUpdate" CssClass="btn btn-default btn-xs" CommandName="Update">تحديث</asp:LinkButton>
																							</td>
																							<td style="border: solid 0px #FFFFFF;">
																								<asp:LinkButton runat="server" ID="lnkCancel" CssClass="btn btn-default btn-xs" CommandName="Cancel">الغاء</asp:LinkButton>
																							</td>
																						</tr>
																					</table>
																					<asp:Button UseSubmitBehavior="false" runat="server" ID="btnUpdateItem" CommandName="Update" Style="display: none;" />
																					<asp:Button UseSubmitBehavior="false" runat="server" ID="btnCancelItem" CommandName="Cancel" Style="display: none;" />
																				</EditItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn HeaderText="">
																				<HeaderStyle BackColor="#000000"></HeaderStyle>
																				<ItemStyle Width="1%" HorizontalAlign="center" />
																				<ItemTemplate>
																					<asp:LinkButton runat="server" ID="lnkDelete" CssClass="btn btn-default btn-xs" CommandName="Delete"><img src="/Layout/RTL/assets/images/DeletePerson.png" /></asp:LinkButton>
																				</ItemTemplate>
																				<EditItemTemplate>
																					&nbsp;
																				</EditItemTemplate>
																			</asp:TemplateColumn>



																			<asp:BoundColumn DataField="code" HeaderText="#" Visible="false">
																				<ItemStyle Width="2px" />
																			</asp:BoundColumn>
																			<asp:BoundColumn DataField="PartyType" HeaderText="PartyType" Visible="false"></asp:BoundColumn>

																			<asp:TemplateColumn HeaderText="م.">
																				<ItemStyle HorizontalAlign="center" Width="2px" />
																				<ItemTemplate>
																					<%#Convert.ToInt32(DataBinder.Eval(Container, "ItemIndex")) + 1%>
																				</ItemTemplate>
																			</asp:TemplateColumn>


																			<asp:TemplateColumn HeaderText="   الاسم">
																				<HeaderStyle Wrap="false" />
																				<%-- <ItemStyle Width="20%" />--%>
																				<ItemTemplate>
																					<%#Eval("PersonName") %>
																				</ItemTemplate>
																				<EditItemTemplate>
																					<asp:DropDownList ID="lstRequestedFromPareon" class="Select2Drop form-control" Width="100%" runat="server"></asp:DropDownList>
																					<input id="hdnrequestedfromPerson" value='<%#Eval("PersonID") %>' runat="server" type="hidden" />
																				</EditItemTemplate>

																			</asp:TemplateColumn>


																			<asp:TemplateColumn HeaderText="  الاسم" Visible="false">

																				<HeaderStyle HorizontalAlign="right" />
																				<ItemTemplate>
																					<%#Eval("PersonName") %>
																				</ItemTemplate>
																				<EditItemTemplate>
																					<asp:TextBox ID="txtname" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
																					<input id="hdnPerson_NameAr" value='<%#Eval("PersonName") %>' runat="server" type="hidden" />

																					<ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
																						runat="server" TargetControlID="txtname"
																						CompletionInterval="10" CompletionSetCount="10" ContextKey="<%#ZeroIntergerIFNull(lstChapter.SelectedValue)%>" MinimumPrefixLength="1"
																						ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="OmaPersonsAutoComplete" />

																				</EditItemTemplate>

																			</asp:TemplateColumn>


																			<asp:TemplateColumn HeaderText="  الرقم المدني" Visible="false">
																				<ItemStyle />
																				<HeaderStyle HorizontalAlign="right" />
																				<ItemTemplate>
																					<%#Eval("CivilID") %>
																				</ItemTemplate>
																				<EditItemTemplate>
																					<asp:TextBox ID="txtCivilID" MaxLength="12" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>

																					<input id="hdnCivilID" value='<%#Eval("CivilID") %>' runat="server" type="hidden" />
																				</EditItemTemplate>

																			</asp:TemplateColumn>
																			<asp:TemplateColumn HeaderText=" ملاحظات" Visible="false">

																				<HeaderStyle HorizontalAlign="right" />
																				<ItemTemplate><%#Eval("Notes") %></ItemTemplate>
																				<EditItemTemplate>

																					<asp:TextBox ID="txtNotes" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
																					<input id="hdnNotes" value='<%#Eval("Notes") %>' runat="server" type="hidden" />

																				</EditItemTemplate>


																			</asp:TemplateColumn>

																		</Columns>
																	</asp:DataGrid>

																</ContentTemplate>
															</asp:UpdatePanel>
														</div>

														<div class="col-md-6" style="display: none">

															<asp:UpdatePanel runat="server" ID="Updatepanel3" ChildrenAsTriggers="true" UpdateMode="conditional">
																<ContentTemplate>


																	<asp:Button UseSubmitBehavior="false" runat="server" ID="Button1" Text="Add Item" Style="display: none;" OnClick="btnAddNewItem_Click" />

																	<div class="panel-heading">
																		<h5 class="panel-title">الجهات المعنيه </h5>

																	</div>
																	<asp:DataGrid ID="grdDefendant" runat="server"
																		class="table table-hover table-striped table-bordered table-advanced tablesorter"
																		AutoGenerateColumns="False"
																		BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
																		CellPadding="3" Width="100%" OnItemDataBound="grdDefendant_ItemDataBound" OnItemCommand="grdDefendant_ItemCommand">
																		<SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
																		<ItemStyle CssClass="grdItem" />
																		<AlternatingItemStyle CssClass="grdItem" />
																		<HeaderStyle CssClass="grdHead" BackColor="Gray" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
																		<FooterStyle CssClass="grdFoot" />
																		<PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
																			PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
																		<Columns>

																			<asp:TemplateColumn HeaderText="">
																				<HeaderStyle BackColor="#000000"></HeaderStyle>
																				<ItemStyle Width="1%" HorizontalAlign="center" />
																				<FooterStyle HorizontalAlign="center" />
																				<ItemTemplate>
																					<asp:LinkButton runat="server" ID="lnkEdit" CssClass="btn btn-default btn-xs" CommandName="Edit"> <img src="/Layout/RTL/assets/images/EditPerson.png" /></asp:LinkButton>
																				</ItemTemplate>
																				<EditItemTemplate>
																					<table border="0" align="center">
																						<tr>
																							<td style="border: solid 0px #FFFFFF;">
																								<asp:LinkButton runat="server" ID="lnkAdd" CssClass="btn btn-default btn-xs" Visible="false" CommandName="AddNew"><img src="/Layout/RTL/assets/images/addPerson.png" /></asp:LinkButton></td>
																							<td style="border: solid 0px #FFFFFF;">
																								<asp:LinkButton runat="server" ID="lnkUpdate" CssClass="btn btn-default btn-xs" CommandName="Update">تحديث</asp:LinkButton>
																							</td>
																							<td style="border: solid 0px #FFFFFF;">
																								<asp:LinkButton runat="server" ID="lnkCancel" CssClass="btn btn-default btn-xs" CommandName="Cancel">الغاء</asp:LinkButton>
																							</td>
																						</tr>
																					</table>
																					<asp:Button UseSubmitBehavior="false" runat="server" ID="btnUpdateItem" CommandName="Update" Style="display: none;" />
																					<asp:Button UseSubmitBehavior="false" runat="server" ID="btnCancelItem" CommandName="Cancel" Style="display: none;" />
																				</EditItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn HeaderText="">
																				<HeaderStyle BackColor="#000000"></HeaderStyle>
																				<ItemStyle Width="1%" HorizontalAlign="center" />
																				<ItemTemplate>
																					<asp:LinkButton runat="server" ID="lnkDelete" CssClass="btn btn-default btn-xs" CommandName="Delete"><img src="/Layout/RTL/assets/images/DeletePerson.png" /></asp:LinkButton>
																				</ItemTemplate>
																				<EditItemTemplate>
																					&nbsp;
																				</EditItemTemplate>
																			</asp:TemplateColumn>



																			<asp:BoundColumn DataField="code" HeaderText="#" Visible="false">
																				<ItemStyle Width="2px" />
																			</asp:BoundColumn>
																			<asp:BoundColumn DataField="PartyType" HeaderText="PartyType" Visible="false"></asp:BoundColumn>

																			<asp:TemplateColumn HeaderText="م.">
																				<ItemStyle HorizontalAlign="center" Width="2px" />
																				<ItemTemplate>
																					<%#Convert.ToInt32(DataBinder.Eval(Container, "ItemIndex")) + 1%>
																				</ItemTemplate>
																			</asp:TemplateColumn>


																			<asp:TemplateColumn HeaderText=" الجهة">
																				<HeaderStyle Wrap="false" />
																				<%-- <ItemStyle Width="20%" />--%>
																				<ItemTemplate>
																					<%#Eval("PersonName") %>
																				</ItemTemplate>
																				<EditItemTemplate>
																					<asp:DropDownList ID="lstRelatedOrgs" class="Select2Drop form-control" Width="100%" runat="server"></asp:DropDownList>
																					<input id="hdnRelatedOrgs" value='<%#Eval("PersonID") %>' runat="server" type="hidden" />
																				</EditItemTemplate>

																			</asp:TemplateColumn>

																			<asp:TemplateColumn HeaderText="  الجهة" Visible="false">

																				<HeaderStyle HorizontalAlign="right" />
																				<ItemTemplate>
																					<%#Eval("PersonName") %>
																				</ItemTemplate>
																				<EditItemTemplate>
																					<asp:TextBox ID="txtDname" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
																					<input id="hdnPerson_NameAr" value='<%#Eval("PersonName") %>' runat="server" type="hidden" />

																					<ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
																						runat="server" TargetControlID="txtDname"
																						CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
																						ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="OrgAutoCompete" />

																				</EditItemTemplate>

																			</asp:TemplateColumn>


																			<%--    <asp:TemplateColumn HeaderText="  الرقم المدني">
																		<ItemStyle Width="15%" />
																		<HeaderStyle HorizontalAlign="right" />
																		<ItemTemplate>
																			<%#Eval("CivilID") %>
																		</ItemTemplate>
																		<EditItemTemplate>
																			<asp:TextBox ID="txtCivilID" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>

																			<input id="hdnCivilID" value='<%#Eval("CivilID") %>' runat="server" type="hidden" />
																		</EditItemTemplate>

																	</asp:TemplateColumn>--%>
																			<asp:TemplateColumn HeaderText=" ملاحظات" Visible="false">

																				<HeaderStyle HorizontalAlign="right" />
																				<ItemTemplate><%#Eval("Notes") %></ItemTemplate>
																				<EditItemTemplate>

																					<asp:TextBox ID="txtDNotes" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
																					<input id="hdnNotes" value='<%#Eval("Notes") %>' runat="server" type="hidden" />

																				</EditItemTemplate>


																			</asp:TemplateColumn>

																		</Columns>
																	</asp:DataGrid>

																</ContentTemplate>
															</asp:UpdatePanel>
														</div>
												</div>


												<div class="row" style="padding-top: 10px">
													<div class="col-md-12">
														<div class="form-actions">
															<div class="col-md-offset-6 col-md-12">


																<asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary" OnClick="btnSave_Click1"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>



																&nbsp;
															   <asp:Button runat="server" ID="btnCancel" class="btn btn-default" Text=" الغاء / رجوع  " OnClick="btnCancel_Click" />
																&nbsp;
															 <a id="anchorAttachment" visible="false" runat="server" href='#' class="btn btn-success btn-xs iframe">
																 <i class="icon-attachment"></i>&nbsp; عرض نص السؤال</a>

															</div>
														</div>
													</div>

												</div>



												</fieldset>

											</div>
										</div>
									</div>
								</div>

								<div class="tab-pane <%=activeTab(2) %>" id="badges-tab2">
									<%-- <asp:UpdatePanel runat="server" ID="Updatepanel7" ChildrenAsTriggers="true" UpdateMode="conditional">
										<ContentTemplate>--%>

									<div class="form-horizontal" id="DivAddAnswer" runat="server" visible="false">

										<div class="row">

											<div class="col-md-6">

												<div class="form-group">
													<label class="col-md-4 control-label">تاريخ الاجابة<span class="text-danger">*</span> </label>

													<div class="col-md-8">
														<div class="input-group">
															<span class="input-group-addon"><i class="icon-calendar22"></i></span>
															<asp:TextBox ID="txtAnswerDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
														</div>

													</div>

												</div>
												<div class="form-group">
													<label class="col-md-4 control-label">نص الاجابة<span class="text-danger">*</span> </label>

													<div class="col-md-8">
														<asp:TextBox ID="txtAnswerText" TextMode="MultiLine" class="form-control" runat="server"></asp:TextBox>

													</div>

												</div>
												<div class="form-group">
													<label class="col-md-4 control-label" for="">ملاحظات  </label>

													<div class="col-md-8">

														<asp:TextBox ID="txtAnswerNotes" TextMode="MultiLine" class="form-control" runat="server"></asp:TextBox>

													</div>
												</div>


												<div class="form-group">
													<label class="col-md-4 control-label">ملف الاجابة</label>

													<div class="col-md-5">
														<asp:Label ID="Label1" runat="server"></asp:Label>
														<asp:FileUpload ID="txtAnswerimage" runat="server" class="file-styled" />
														<span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
													</div>
													<div class="col-md-3">
														<span class="help-block2">| Or | </span>
														<asp:LinkButton runat="server" ID="btnAnswerScan" OnClientClick="return showscannerLoading();" OnClick="btnAnswerScan_Click" class="btn btn-info btn-xs"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
													</div>
												</div>


											</div>
										</div>


										<div class="col-md-12">
											<div class="form-actions">
												<div class="col-md-offset-9 col-md-12">
													<asp:LinkButton ID="lnkSaveAnswer" runat="server" class="btn btn-primary" OnClientClick="return ValidateAnswer()" OnClick="lnkSaveAnswer_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

													&nbsp;
											   <asp:Button runat="server" ID="lnkCancelAnswer" OnClick="lnkCancelAnswer_Click" class="btn btn-default" Text=" الغاء  " />

												</div>
											</div>
										</div>

									</div>
									<div class="row" id="divShowAnswer" runat="server">
										<div class="col-lg-12">
											<div class="portlet box">
												<div class="portlet-header">

													<div class="actions pull-right" style="margin-bottom: 10px;">

														<asp:LinkButton runat="server" ID="lnkAddAnswer" OnClick="lnkAddAnswer_Click" class="btn btn-info btn-xs"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

														<asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="lnkDeleteAnswer" OnClick="lnkDeleteAnswer_Click" class="btn btn-danger btn-xs"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

													</div>
												</div>
												<div class="portlet-body">
													<div class="datatable-scroll" style="padding-top: 20px;">

														<asp:DataGrid runat="server" ID="grdAnswerList" AutoGenerateColumns="False"
															AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemCommand="grdAnswerList_ItemCommand" OnEditCommand="grdAnswerList_EditCommand">
															<PagerStyle Visible="False" />
															<HeaderStyle BackColor="#efefef" Font-Bold="True" />
															<Columns>
																<asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
																<asp:BoundColumn DataField="CreationDate" HeaderText="تاريخ التسجيل  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
																<asp:BoundColumn DataField="AnswerDate" HeaderText=" تاريخ الاجابة  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
																<asp:BoundColumn DataField="AnswerText" HeaderText="نص الاجابة "></asp:BoundColumn>


																<%--<asp:BoundColumn DataField="AnswerNotes" HeaderText="ملاحظات "></asp:BoundColumn>--%>
																<asp:TemplateColumn HeaderText="عرض المرفق">
																	<ItemStyle HorizontalAlign="Center" Width="10%" />
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemTemplate>
																		<a target="_blank" href="<%# ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(Eval("QuestionID"))+"/answers/"+gets(Eval("Code"))+"/" + "&vfileList=["  + gets(Eval("Answerattachments"))+";]" %>" class="label border-left-primary label-striped iframe" style="<%#showattachment(gets(Eval("Answerattachments")))%>">
																			<i class="icon-attachment"></i>&nbsp;عرض المرفق
																		</a>

																	</ItemTemplate>
																</asp:TemplateColumn>

																<asp:TemplateColumn HeaderText="تعديل">
																	<ItemStyle HorizontalAlign="Center" Width="5%" />
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemTemplate>

																		<asp:LinkButton runat="server" ID="lnkEdit" CommandName="Edit" class="btn btn-default btn-xs">
												 <i class="fa fa-edit"></i>&nbsp;
												تعديل
																		</asp:LinkButton>
																	</ItemTemplate>
																</asp:TemplateColumn>

																<asp:TemplateColumn>
																	<ItemStyle Width="5%" HorizontalAlign="Center" />
																	<HeaderStyle Wrap="False" HorizontalAlign="Center" />
																	<HeaderTemplate>
																		<input id="chkAllItems" class="checkall" style="border-style: none;" type="checkbox" onclick="CheckAllDataGridCheckBoxes('chkItem', this.checked)" />
																	</HeaderTemplate>
																	<ItemTemplate>
																		<asp:CheckBox runat="server" ID="chkItem" CssClass="check" />
																	</ItemTemplate>
																</asp:TemplateColumn>
															</Columns>
														</asp:DataGrid>
														<div class="row mbm">
															<div class="col-lg-12">
																<div class="pagination-panel">
																</div>
															</div>
														</div>
													</div>


													<div class="datatable-footer">
														<div class="dataTables_info" id="DataTables_Table_3_info" role="status" aria-live="polite">
														</div>
														<div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">

															<cc1:Pager CurrentIndex="1" OnCommand="pager_Command5" ShowFirstLast="False" ID="pager5"
																runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>


														</div>
													</div>
												</div>
											</div>



										</div>
									</div>

									<%--   </ContentTemplate>
									</asp:UpdatePanel>--%>
								</div>

								<div class="tab-pane <%=activeTab(3) %>" id="badges-tab3">
									<%--  <asp:UpdatePanel runat="server" ID="Updatepanel4" ChildrenAsTriggers="true" UpdateMode="conditional">
										<ContentTemplate>--%>

									<div class="form-horizontal" id="divAddIncoming" runat="server" visible="false">

										<div class="row">

											<div class="col-md-4">

												<div class="form-group">
													<label class="col-md-3 control-label" for="">رقم الكتاب<span class="text-danger">*</span></label>

													<div class="col-md-9">

														<asp:TextBox ID="txtDoc_Serial" class="form-control" runat="server"></asp:TextBox>
													</div>

												</div>

												<div class="form-group">
													<label class="col-md-3 control-label" for="">الموضوع <span class="text-danger">*</span></label>

													<div class="col-md-9">

														<asp:TextBox ID="txtDoc_Subject" class="form-control" runat="server"></asp:TextBox>
													</div>

												</div>


												<div class="form-group">
													<label class="col-md-3 control-label" for="">ملاحظات  </label>

													<div class="col-md-9">

														<asp:TextBox ID="txtComingNotes" class="form-control" runat="server"></asp:TextBox>

													</div>
												</div>


												<div class="form-group">
													<label class="col-md-3 control-label">الملف  </label>

													<div class="col-md-5">
														<asp:Label ID="Label2" runat="server"></asp:Label>
														<asp:FileUpload ID="txtIncomingImge" runat="server" class="file-styled" />
														<span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
													</div>
													<div class="col-md-4">
														<span class="help-block2">| Or | </span>
														<asp:LinkButton runat="server" ID="btnIncomingScan" OnClientClick="return ValidateIncoming();" OnClick="btnIncomingScan_Click" class="btn btn-info btn-xs"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
													</div>
												</div>

												<%--   <div id="divProAttache" runat="server">
											إضافة مرفق
											<hr />
											  <div class="form-group">
													<label class="col-md-3 control-label" for="">نوع المرفق<span class="text-danger">*</span> </label>

													<div class="col-md-9">
														<asp:DropDownList ID="lstAttachmentType" class="form-control" runat="server"></asp:DropDownList>
													</div>
												</div>

												<div class="form-group">
													<label class="col-md-3 control-label">المرجع<span class="text-danger">*</span></label>

													<div class="col-md-9">

														<asp:TextBox runat="server" class="form-control" ID="txtRef"></asp:TextBox>


													</div>

												</div>

												<div class="form-group">
													<label class="col-md-3 control-label">الملف</label>

													<div class="col-md-9">
														<asp:Label ID="lblimage" runat="server"></asp:Label>
														<asp:FileUpload ID="txtImage" runat="server" Visible="false" />
														<asp:LinkButton runat="server" ID="lnkScan" class="btn btn-info btn-xs" OnClick="lnkScan_Click"  OnClientClick="return ValidateProcedures()" ><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>


													</div>

												</div>--%>
											</div>
											<div class="col-md-4">

												<div class="form-group">
													<label class="col-md-3 control-label" for="">وارد من  <span class="text-danger">*</span></label>

													<div class="col-md-9" style="z-index: 99">




														<asp:DropDownList ID="lstIncomingOrg" class="Select2Drop" runat="server"></asp:DropDownList>

														<%--  <asp:TextBox ID="txtFrom" class="form-control" runat="server"></asp:TextBox>

														<ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender3"
															runat="server" TargetControlID="txtFrom"
															CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
															ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="ArchOrgAutoCompete" />--%>
													</div>

												</div>

												<div class="form-group">
													<label class="col-md-3 control-label">التاريخ<span class="text-danger">*</span> </label>

													<div class="col-md-9">
														<div class="input-group">
															<span class="input-group-addon"><i class="icon-calendar22"></i></span>
															<asp:TextBox ID="txtComingDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
														</div>

													</div>

												</div>

												<div class="form-group" style="display: ">
													<label class="col-md-3 control-label">تاريخ التنبيه  </label>

													<div class="col-md-9">
														<div class="input-group">
															<span class="input-group-addon"><i class="icon-calendar22"></i></span>
															<asp:TextBox ID="txtComingReminderDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
														</div>

													</div>

												</div>
											</div>
											<div class="col-md-4">
											</div>

										</div>

										<div class="col-md-12">
											<div class="form-actions">
												<div class="col-md-offset-9 col-md-12">
													<asp:LinkButton ID="lnkSaveIncoming" runat="server" class="btn btn-primary" OnClientClick="return ValidateIncoming()" OnClick="lnkSaveIncoming_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

													&nbsp;
											   <asp:Button runat="server" ID="lnkCancelIncoming" class="btn btn-default" Text=" الغاء  " OnClick="lnkCancelIncoming_Click" />

												</div>
											</div>
										</div>

									</div>
									<div class="row" id="divshowincoming" runat="server">
										<div class="col-lg-12">
											<div class="portlet box">
												<div class="portlet-header">

													<div class="actions pull-right" style="margin-bottom: 10px;">

														<asp:LinkButton runat="server" ID="Lnkincoming" class="btn btn-info btn-xs" OnClick="Lnkincoming_Click"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

														<asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="lnkDeleteIncoming" class="btn btn-danger btn-xs" OnClick="lnkDeleteIncoming_Click"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

													</div>
												</div>
												<div class="portlet-body">
													<div class="datatable-scroll" style="padding-top: 20px;">

														<asp:DataGrid runat="server" ID="grdincoming" AutoGenerateColumns="False"
															AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemCommand="grdincoming_ItemCommand" OnEditCommand="grdincoming_EditCommand">
															<PagerStyle Visible="False" />
															<HeaderStyle BackColor="#efefef" Font-Bold="True" />
															<Columns>
																<asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>

																<asp:TemplateColumn HeaderText="النوع">
																	<ItemStyle Width="3%" />
																	<ItemTemplate>
																		<%#fillDocType(gets(Eval("Doc_Type"))) %>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:BoundColumn DataField="Doc_Serial" HeaderText="رقم الكتاب"></asp:BoundColumn>
																<asp:BoundColumn DataField="Doc_From" HeaderText="وارد من "></asp:BoundColumn>

																<asp:BoundColumn DataField="SentDate" HeaderText="التاريخ  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
																<asp:BoundColumn DataField="Doc_Subject" HeaderText="الموضوع "></asp:BoundColumn>
																<asp:BoundColumn DataField="Doc_Notes" HeaderText="ملاحظات "></asp:BoundColumn>
																<asp:BoundColumn DataField="NextFollowReminderDate" HeaderText="تاريخ التنبيه  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>


																<asp:TemplateColumn HeaderText="عرض المرفق">
																	<ItemStyle HorizontalAlign="Center" Width="10%" />
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemTemplate>

																		<a class="label border-left-primary label-striped iframe" target="_blank" href="<%#  ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath +gets(Eval("RefDocID"))+"/incoming/"+ gets(Eval("Code"))+"/"+ "&vfileList=[" + gets(Eval("Filepath")) +";]"%>" style="<%#showattachment(gets(Eval("Filepath")))%>">
																			<i class="icon-attachment"></i>&nbsp; عرض المرفق


																		</a>



																	</ItemTemplate>
																</asp:TemplateColumn>


																<asp:TemplateColumn HeaderText="تعديل">
																	<ItemStyle HorizontalAlign="Center" Width="5%" />
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemTemplate>

																		<asp:LinkButton runat="server" ID="lnkEdit" CommandName="Edit" class="btn btn-default btn-xs">
												 <i class="fa fa-edit"></i>&nbsp;
												تعديل
																		</asp:LinkButton>
																	</ItemTemplate>
																</asp:TemplateColumn>

																<asp:TemplateColumn>
																	<ItemStyle Width="5%" HorizontalAlign="Center" />
																	<HeaderStyle Wrap="False" HorizontalAlign="Center" />
																	<HeaderTemplate>
																		<input id="chkAllItems" class="checkall" style="border-style: none;" type="checkbox" onclick="CheckAllDataGridCheckBoxes('chkItem', this.checked)" />
																	</HeaderTemplate>
																	<ItemTemplate>
																		<asp:CheckBox runat="server" ID="chkItem" CssClass="check" />
																	</ItemTemplate>
																</asp:TemplateColumn>
															</Columns>
														</asp:DataGrid>
														<div class="row mbm">
															<div class="col-lg-12">
																<div class="pagination-panel">
																</div>
															</div>
														</div>
													</div>


													<div class="datatable-footer">
														<div class="dataTables_info" id="DataTables_Table_3_info" role="status" aria-live="polite">
															<asp:Label ID="lblComingCount" runat="server"></asp:Label>
														</div>
														<div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">
															<%-- <a class="paginate_button previous disabled" aria-controls="DataTables_Table_3" data-dt-idx="0" tabindex="0" id="DataTables_Table_3_previous">→</a>
						<span><a class="paginate_button current" aria-controls="DataTables_Table_3" data-dt-idx="1" tabindex="0">1</a>
							<a class="paginate_button " aria-controls="DataTables_Table_3" data-dt-idx="2" tabindex="0">2</a>


						</span>
						<a class="paginate_button next" aria-controls="DataTables_Table_3" data-dt-idx="3" tabindex="0" id="DataTables_Table_3_next">←</a>--%>


															<cc1:Pager CurrentIndex="1" OnCommand="pager_Command2" ShowFirstLast="False" ID="pager2"
																runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>


														</div>
													</div>
												</div>
											</div>



										</div>
									</div>

									<%--                                        </ContentTemplate>
									</asp:UpdatePanel>--%>
								</div>
								<div class="tab-pane <%=activeTab(4) %>" id="badges-tab4">
									<%--  <asp:UpdatePanel runat="server" ID="Updatepanel5" ChildrenAsTriggers="true" UpdateMode="conditional">
										<ContentTemplate>--%>

									<div class="form-horizontal" id="divOutgiongAdd" runat="server" visible="false">

										<div class="row">

											<div class="col-md-4">

												<div class="form-group">
													<label class="col-md-3 control-label" for="">رقم الكتاب<span class="text-danger">*</span></label>

													<div class="col-md-9">

														<asp:TextBox ID="txtOutDocNo" class="form-control" runat="server"></asp:TextBox>
													</div>

												</div>

												<div class="form-group">
													<label class="col-md-3 control-label" for="">الموضوع <span class="text-danger">*</span></label>

													<div class="col-md-9">

														<asp:TextBox ID="txtOutSubject" class="form-control" runat="server"></asp:TextBox>
													</div>

												</div>


												<div class="form-group">
													<label class="col-md-3 control-label" for="">ملاحظات  </label>

													<div class="col-md-9">

														<asp:TextBox ID="txtoutNotes" class="form-control" runat="server"></asp:TextBox>

													</div>
												</div>

												<div class="form-group">
													<label class="col-md-3 control-label">الملف  </label>

													<div class="col-md-5">
														<asp:Label ID="Label3" runat="server"></asp:Label>
														<asp:FileUpload ID="txtoutgoiningImage" runat="server" class="file-styled" />
														<span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
													</div>
													<div class="col-md-4">
														<span class="help-block2">| Or | </span>
														<asp:LinkButton runat="server" ID="btnOutgoingScan" OnClientClick="return Validateoutgoing();" OnClick="btnOutgoingScan_Click" class="btn btn-info btn-xs"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
													</div>
												</div>

											</div>
											<div class="col-md-4">

												<div class="form-group">
													<label class="col-md-3 control-label" for="">صادر إلى    <span class="text-danger">*</span></label>

													<div class="col-md-9" style="z-index: 99">


														<asp:DropDownList ID="lstoutgoiningOrg" class="Select2Drop" runat="server"></asp:DropDownList>

														<%--    <asp:TextBox ID="txtto" class="form-control" runat="server"></asp:TextBox>
														<ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender4"
															runat="server" TargetControlID="txtto"
															CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
															ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="ArchOrgAutoCompete" />--%>
													</div>

												</div>

												<div class="form-group">
													<label class="col-md-3 control-label">التاريخ<span class="text-danger">*</span> </label>

													<div class="col-md-9">
														<div class="input-group">
															<span class="input-group-addon"><i class="icon-calendar22"></i></span>
															<asp:TextBox ID="txtoutDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
														</div>

													</div>

												</div>

												<div class="form-group" style="display: ">
													<label class="col-md-3 control-label">تاريخ التنبيه  </label>

													<div class="col-md-9">
														<div class="input-group">
															<span class="input-group-addon"><i class="icon-calendar22"></i></span>
															<asp:TextBox ID="txtOutReminderDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
														</div>

													</div>

												</div>
											</div>
										</div>

										<div class="col-md-12">
											<div class="form-actions">
												<div class="col-md-offset-9 col-md-12">
													<asp:LinkButton ID="lnkSaveOut" runat="server" class="btn btn-primary" OnClientClick="return Validateoutgoing()" OnClick="lnkSaveOut_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

													&nbsp;
											   <asp:Button runat="server" ID="lnkCancelOut" class="btn btn-default" Text=" الغاء  " OnClick="lnkCancelOut_Click" />

												</div>
											</div>
										</div>

									</div>
									<div class="row" id="divoutgoingshow" runat="server">
										<div class="col-lg-12">
											<div class="portlet box">
												<div class="portlet-header">

													<div class="actions pull-right" style="margin-bottom: 10px;">

														<asp:LinkButton runat="server" ID="lnkAddNewOutGoing" class="btn btn-info btn-xs" OnClick="lnkAddNewOutGoing_Click"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

														<asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="lnkDeleteOutgoing" class="btn btn-danger btn-xs" OnClick="lnkDeleteOutgoing_Click"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

													</div>
												</div>
												<div class="portlet-body">
													<div class="datatable-scroll" style="padding-top: 20px;">

														<asp:DataGrid runat="server" ID="grdOutgoing" AutoGenerateColumns="False"
															AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemCommand="grdOutgoing_ItemCommand" OnEditCommand="grdOutgoing_EditCommand">
															<PagerStyle Visible="False" />
															<HeaderStyle BackColor="#efefef" Font-Bold="True" />
															<Columns>
																<asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>

																<asp:TemplateColumn HeaderText="النوع">
																	<ItemStyle Width="3%" />
																	<ItemTemplate>
																		<%#fillDocType(gets(Eval("Doc_Type"))) %>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:BoundColumn DataField="Doc_Serial" HeaderText="رقم الكتاب"></asp:BoundColumn>
																<asp:BoundColumn DataField="Doc_to" HeaderText="صادر إلى   "></asp:BoundColumn>

																<asp:BoundColumn DataField="SentDate" HeaderText="التاريخ  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
																<asp:BoundColumn DataField="Doc_Subject" HeaderText="الموضوع "></asp:BoundColumn>
																<asp:BoundColumn DataField="Doc_Notes" HeaderText="ملاحظات "></asp:BoundColumn>
																<asp:BoundColumn DataField="NextFollowReminderDate" HeaderText="تاريخ التنبيه  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
																<asp:TemplateColumn HeaderText="عرض المرفق">
																	<ItemStyle HorizontalAlign="Center" Width="10%" />
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemTemplate>
																		<%-- <% if (!(Eval("Filepath")).Equals("")) { %>--%>
																		<a class="label border-left-primary label-striped iframe" target="_blank" href="<%#  ScannerRepositoryViewer + "?targetpath=" +  _TargetUploadPath +gets(Eval("RefDocID"))+"/outgoing/"+ gets(Eval("Code"))+"/" + "&vfileList=[" + gets(Eval("Filepath"))+";]" %>" style="<%#showattachment(gets(Eval("Filepath")))%>">
																			<i class="icon-attachment"></i>&nbsp;عرض المرفق
																		</a>
																	</ItemTemplate>
																</asp:TemplateColumn>

																<asp:TemplateColumn HeaderText="تعديل">
																	<ItemStyle HorizontalAlign="Center" Width="5%" />
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemTemplate>

																		<asp:LinkButton runat="server" ID="lnkEdit" CommandName="Edit" class="btn btn-default btn-xs">
												 <i class="fa fa-edit"></i>&nbsp;
												تعديل
																		</asp:LinkButton>
																	</ItemTemplate>
																</asp:TemplateColumn>

																<asp:TemplateColumn>
																	<ItemStyle Width="5%" HorizontalAlign="Center" />
																	<HeaderStyle Wrap="False" HorizontalAlign="Center" />
																	<HeaderTemplate>
																		<input id="chkAllItems" class="checkall" style="border-style: none;" type="checkbox" onclick="CheckAllDataGridCheckBoxes('chkItem', this.checked)" />
																	</HeaderTemplate>
																	<ItemTemplate>
																		<asp:CheckBox runat="server" ID="chkItem" CssClass="check" />
																	</ItemTemplate>
																</asp:TemplateColumn>
															</Columns>
														</asp:DataGrid>
														<div class="row mbm">
															<div class="col-lg-12">
																<div class="pagination-panel">
																</div>
															</div>
														</div>
													</div>


													<div class="datatable-footer">
														<div class="dataTables_info" id="DataTables_Table_3_info" role="status" aria-live="polite">
															<asp:Label ID="lbloutgoingCount" runat="server"></asp:Label>
														</div>
														<div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">
															<%-- <a class="paginate_button previous disabled" aria-controls="DataTables_Table_3" data-dt-idx="0" tabindex="0" id="DataTables_Table_3_previous">→</a>
						<span><a class="paginate_button current" aria-controls="DataTables_Table_3" data-dt-idx="1" tabindex="0">1</a>
							<a class="paginate_button " aria-controls="DataTables_Table_3" data-dt-idx="2" tabindex="0">2</a>


						</span>
						<a class="paginate_button next" aria-controls="DataTables_Table_3" data-dt-idx="3" tabindex="0" id="DataTables_Table_3_next">←</a>--%>


															<cc1:Pager CurrentIndex="1" OnCommand="pager_Command3" ShowFirstLast="False" ID="pager3"
																runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>


														</div>
													</div>
												</div>
											</div>

										</div>
									</div>

									<%--                                        </ContentTemplate>
									</asp:UpdatePanel>--%>
								</div>

							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<!--END CONTENT-->
	<!--BEGIN FOOTER-->
	<div id="scanLoading" class="scanLoading" style="display: none">
		<img src="/Layout/images/scan-document.gif" />
	</div>
	    <uc:DeleteConfirm runat="server" />
</asp:Content>
