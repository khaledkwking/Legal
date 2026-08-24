<%@ Page Title="" Language="C#"  AutoEventWireup="true" ValidateRequest="false" CodeBehind="LawDetailsDocData.aspx.cs" Inherits="UI.Web.Modules.Laws.LawDetailsDocData" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
<link href="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/External/google/css.css" rel="stylesheet" />

	<link href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/css/icons/icomoon/styles.css" rel="stylesheet" type="text/css">
	<link href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/css/bootstrap.css" rel="stylesheet" type="text/css">
	<link href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/css/core.css" rel="stylesheet" type="text/css">
	<link href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/css/components.css" rel="stylesheet" type="text/css">
	<link href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/css/colors.css" rel="stylesheet" type="text/css">
    	<link href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/css/extras/animate.min.css" rel="stylesheet" type="text/css">


	<link rel="stylesheet" href="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/progress/jQuery-plugin-progressbar.css">


	<!-- /global stylesheets -->


	<!-- Core JS files -->
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/loaders/pace.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/core/libraries/jquery.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/core/libraries/bootstrap.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/loaders/blockui.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/ui/nicescroll.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/ui/drilldown.js"></script>
	<!-- /core JS files -->
      <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/examples/public/javascript/jquery-1.7.2.js"></script>

<!-- Theme JS files -->
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/notifications/jgrowl.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/ui/moment/moment.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/pickers/daterangepicker.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/pickers/anytime.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/pickers/pickadate/picker.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/pickers/pickadate/picker.date.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/pickers/pickadate/picker.time.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/pickers/pickadate/legacy.js"></script>

	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/core/app.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/pages/picker_date.js"></script>
	<!-- /theme JS files -->


       <!-- Theme JS files -->
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/core/libraries/jquery_ui/interactions.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/forms/styling/uniform.min.js"></script>

	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/pages/form_select2.js"></script>
    <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/pages/dashboard.js"></script>


    <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/pages/form_layouts.js"></script>
	<!-- /theme JS files -->
     <!--LOADING STYLESHEET FOR Dialog-->


    	<!-- Theme JS files -->
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/core/libraries/jasny_bootstrap.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/forms/styling/uniform.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/forms/inputs/autosize.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/forms/inputs/formatter.min.js"></script>
	 
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/forms/inputs/passy.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/forms/inputs/maxlength.min.js"></script>

	<script type="text/javascript" src="assets/js/core/app.js"></script>
	<script type="text/javascript" src="assets/js/pages/form_controls_extended.js"></script>
	<!-- /theme JS files -->

 <%--   <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/tables/datatables/extensions/responsive.min.js"></script>
   <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/pages/datatables_advanced.js"></script>
    <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/pages/datatables_responsive.js"></script>
--%>



      <!-- DataTables -->

        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/jquery.dataTables.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/dataTables.bootstrap.js"></script>

        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/dataTables.buttons.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/buttons.bootstrap.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/jszip.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/pdfmake.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/vfs_fonts.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/buttons.html5.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/buttons.print.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/dataTables.fixedHeader.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/dataTables.keyTable.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/dataTables.responsive.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/responsive.bootstrap.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/dataTables.scroller.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/dataTables.colVis.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/datatables/dataTables.fixedColumns.min.js"></script>
        <script src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/plugins/bootstrap-touchspin/js/jquery.bootstrap-touchspin.min.js" type="text/javascript"></script>




        <script src="/Modules/APICall/datatables.init.js"></script>


    <link rel="stylesheet" href="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/public/css/zebra_dialog.css" type="text/css">
    <link rel="stylesheet" href="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/examples/public/css/style.css" type="text/css">
    <link rel="stylesheet" href="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/examples/libraries/highlight/public/css/ir_black.css" type="text/css">


    <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/examples/libraries/highlight/public/javascript/highlight.js"></script>
    <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/zebra-dialog/public/javascript/zebra_dialog.js"></script>


   
    <!-- Theme JS files -->
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/forms/styling/uniform.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/notifications/pnotify.min.js"></script>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/plugins/forms/selects/bootstrap_multiselect.js"></script>

	<%--<script type="text/javascript" src="assets/js/core/app.js"></script>--%>
	<script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/pages/form_multiselect.js"></script>
    <title></title>
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


     function ControlGrid(imgName, rowIndex, rowID) {
         //alert("CONTROL GRID");
         // alert(imgName);
         // alert(rowIndex);
         //alert(rowID);
         rowIndex = rowIndex + 3;

         var myrow = "";
         if (rowIndex < 10)
             myrow = "ctl00_Main_grdLawDocsList_ctl0" + rowIndex;
         else
             myrow = "ctl00_Main_grdLawDocsList_ctl" + rowIndex;
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


     function showscannerLoading() {

         if (ValidateProcedure()) {
             document.getElementById("scanLoading").style.display = "";
             return true;
         } else { return false; }
     }


     function createFormInputsFromObject(data, prefix) {
         prefix = prefix || '';
         var inputs = '';

         jQuery.each(data, function (name, value) {
             if (prefix !== '') name = prefix + '[' + name + ']';
             if (Array.isArray(value) || value instanceof Object) {
                 inputs += createFormInputsFromObject(value, name);
             }
             else {
                 inputs += jQuery('<input>').attr({ type: 'hidden', name: name, value: value }).prop('outerHTML');
             }
         });

         return inputs;
     }
     function showPdf(url, postData) {
         $.colorbox({
             iframe: true,
             href: 'about:blank',
             width: '90%',
             height: '90%',
             onComplete: function () {

                 var iframe = $('#cboxLoadedContent iframe');

                 var form = $('<form>').attr({ action: url, method: 'POST', target: iframe.attr("name") });
                 if (!$.isEmptyObject(postData)) {
                     $(createFormInputsFromObject(postData)).appendTo(form);
                 }
                 form.appendTo(iframe)
                     .submit();

             }
         });



     }



 </script>
</head>
<body class="law-page">
    <form runat ="server">
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

                                                   <%-- <asp:LinkButton ID="lnkSearchback" CssClass="control-arrow" runat="server" OnClick="lnkSearchback_Click"> رجوع  <i class=" icon-backward2"></i></asp:LinkButton>--%>

                                                </legend>




                                                <div class="datatable-scroll">


                                                    <asp:DataGrid ID="grdLawDocsList" runat="server"
                                                        DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="40" class="table datatable-basic dataTable no-footer"
                                                        Width="100%" OnItemDataBound="grdLawDocsList_ItemDataBound" OnItemCommand="grdLawDocsList_ItemCommand">
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
                                                                <ItemStyle HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    <img style="cursor: pointer;" src="/layout/images/plus.gif" alt="" border="0" runat="server" id="imgControl" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="Code"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="DocFilepath"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="DocFilepath_published"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="LawCancelled"></asp:BoundColumn>
                                                             <%--<asp:BoundColumn Visible="false" HeaderText="Code" DataField="isAudited"></asp:BoundColumn>--%>

                                                            <%--  <asp:BoundColumn DataField="Law_DocTypeNameAr" HeaderText="  نوع الوثيقة  ">
																<HeaderStyle Wrap="false" />
															</asp:BoundColumn>--%>


                                                            <asp:TemplateColumn HeaderText=" نوع الوثيقة   ">
                                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Law_DocTypeNameAr") %>
                                                                    <br />

                                                                    <%# GetDocCats(ZeroIntergerIFNull(gets(Eval("DocCategoryID")).ToString()),"إسترداد") %>
                                                                    <%# (GetStatus(ZeroIntergerIFNull(gets(Eval("ProcedureTypeCode")).ToString()),gets(Eval("law_DocProceduresTypesNameAr")))) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                            <asp:BoundColumn HeaderText="رقم الوثيقة " DataField="DocSerial"></asp:BoundColumn>


                                                            <asp:BoundColumn DataField="DocDate" HeaderText="تاريخ إصدار الوثيقة" DataFormatString="{0:dd/MM/yyyy}">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="effectiveDate" Visible="false" HeaderText="تاريخ العمل بالوثيقة" DataFormatString="{0:dd/MM/yyyy}">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                         
                                                            <asp:BoundColumn DataField="Law_DocCategoryNameAr" HeaderText="  التصنيف  ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>


                                                         
                                                            <asp:BoundColumn DataField="kng_Dession" HeaderText="  قرار مجلس الامة  ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                       


                                                            <asp:TemplateColumn HeaderText="التفاصيل">
                                                                <ItemStyle HorizontalAlign="right" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div style="text-align: right">
                                                                        <a target="_blank" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath+gets(Eval("code"))+"/" +"&vfileList=["+gets(Eval("DocFilepath"))+";]" %>' style="font-size: 12px; <%#showattachment(gets(Eval("DocFilepath")))%>" class="label border-left-primary label-striped">
                                                                            <i class="icon-attachment"></i>&nbsp;
																							  ملف الوثيقة
                                                                        </a>

                                                                    </div>
                                                                 

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

    <div class="panel" id="tblProcedure" runat="server" visible="false">

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
													<asp:Label ID="Label1" runat="server"></asp:Label>)</span>

                                               <%-- <asp:LinkButton ID="lnkSearchback" CssClass="control-arrow" runat="server" OnClick="lnkSearchback_Click"> رجوع  <i class=" icon-backward2"></i></asp:LinkButton>--%>

                                            </legend>




                                            <div class="datatable-scroll">


                                                <asp:DataGrid ID="grdLawProcedureList" runat="server"
                                                    DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="40" class="table datatable-basic dataTable no-footer"
                                                    Width="100%" >
                                                    <SelectedItemStyle ForeColor="White" />
                                                    <ItemStyle CssClass="grdItem" />
                                                    <AlternatingItemStyle CssClass="grdItem" />
                                                    <PagerStyle Visible="false" />
                                                    <HeaderStyle CssClass="grdHead" BackColor="Black" ForeColor="White" Font-Bold="False" />
                                                   <Columns>

    <asp:BoundColumn Visible="false" DataField="Code" HeaderText="Code" />

    <asp:BoundColumn
        DataField="ProcedureletterNum"
        HeaderText="رقم الكتاب">
        <HeaderStyle Wrap="false" />
    </asp:BoundColumn>

    <asp:BoundColumn
        DataField="ProcedureDate"
        HeaderText="تاريخ الإجراء"
        DataFormatString="{0:dd/MM/yyyy}">
        <HeaderStyle Wrap="false" />
    </asp:BoundColumn>

    <asp:BoundColumn
        DataField="LastActionDate"
        HeaderText="آخر إجراء"
        DataFormatString="{0:dd/MM/yyyy}">
        <HeaderStyle Wrap="false" />
    </asp:BoundColumn>

   <%-- <asp:BoundColumn
        DataField="SendDate"
        HeaderText="تاريخ الإرسال"
        DataFormatString="{0:dd/MM/yyyy}">
        <HeaderStyle Wrap="false" />
    </asp:BoundColumn>--%>

    <asp:BoundColumn
        DataField="ProcedureSubject"
        HeaderText="الموضوع">
        <HeaderStyle Wrap="false" />
        <ItemStyle Width="35%" />
    </asp:BoundColumn>

   <%-- <asp:BoundColumn
        DataField="ProcedureNotes"
        HeaderText="الملاحظات">
        <HeaderStyle Wrap="false" />
        <ItemStyle Width="25%" />
    </asp:BoundColumn>--%>

    <asp:TemplateColumn HeaderText="التفاصيل">
        <ItemStyle HorizontalAlign="Center" Width="120px" />
        <ItemTemplate>

            <a target="_blank"
               href='<%# ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(Eval("Code")) + "/" + "&vfileList=[" + gets(Eval("Procedureattachments")) + ";]" %>'
               style='<%# showattachment(gets(Eval("Procedureattachments"))) %>'
               class="label border-left-primary label-striped">

                <i class="icon-attachment"></i>
                ملف الإجراء

            </a>

        </ItemTemplate>
    </asp:TemplateColumn>

</Columns>
                                                </asp:DataGrid>
                                            </div>

                                            <div class="datatable-footer">
                                                <div class="dataTables_info" id="DataTables_Table_3_info" role="status" aria-live="polite">
                                                    <asp:Label ID="Label2" runat="server"></asp:Label>
                                                </div>
                                                <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">


                                                    <cc1:Pager CurrentIndex="1" OnCommand="pager_Command" ShowFirstLast="False" ID="pager2"
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
</form>
</body>
</html>



   

  
   



