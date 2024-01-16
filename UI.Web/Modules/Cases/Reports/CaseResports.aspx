<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="CaseResports.aspx.cs" Inherits="UI.Web.Modules.Cases.Reports.CaseResports" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">



    <style type="text/css">
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
              background:#ffffff;
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
                myrow = "ctl00_Main_grdCasesList_ctl0" + rowIndex;
            else
                myrow = "ctl00_Main_grdCasesList_ctl" + rowIndex;
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
        

       

        function LinkAddClick() {
            // alert("in");

            // return InsertItem();
        }

        function LinkAddClickgrdprosecutor() {
            // alert("in");

            // return InsertItem();
        }



       
 

        function showscannerLoading() {

            if (validateFileAttachment()) {
                document.getElementById("scanLoading").style.display = "";
                return true;
            } else { return false; }
        }


    </script>




    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        القضايا
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




                            <div class="col-md-6">

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        رقم المسلسل  :    
                                                                                         
                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterInternalSerial" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <label class="col-lg-2 control-label">
                                        السنة  :    
                                                                                         
                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterFileYear" runat="server" class="form-control"></asp:TextBox>
                                    </div>


                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">تاريخ  ايداع القضية فى المحكمة من :</label>
                                    <div class="col-lg-3">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
                                            <asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                        </div>
                                    </div>
                                    <label class="col-lg-2 control-label">إلى :</label>

                                    <div class="col-lg-3">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <asp:TextBox ID="txtFilterDateTo" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>

                                        </div>
                                    </div>

                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label" for="">رقم القضية : </label>

                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtFSuitNum" class="form-control" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtFSuitYear" class="form-control" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="lstFiltertype" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">

                                        <asp:TextBox ID="txtFSuitType" class="form-control" runat="server"></asp:TextBox>

                                    </div>
                                </div>


                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        الموضوع :                                                        
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:TextBox ID="txtFilterSubject" runat="server" class="form-control"></asp:TextBox>

                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1"
                                            runat="server" TargetControlID="txtFilterSubject"
                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />

                                    </div>



                                </div>


                                <div class="form-group" style="padding:10px;background:#9dc5e2">
                                    <label class="col-lg-3 control-label"> عرض حسب  :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstgroupping" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الدرجه:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFCaseLevel" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <%-- 
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">المحكمه:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterCourt" class="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>--%>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        المدعي علية   :                                                     
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <%--  <asp:TextBox ID="txtFilterPersonName" runat="server" class="form-control"></asp:TextBox>--%>
                                        <asp:DropDownList ID="lstFilterPersonName" class="Select2Drop" runat="server"></asp:DropDownList>

                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        الرقم الإلى:                                                
                                    </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="txtFilterFileNUm" runat="server" class="form-control"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="form-group" style='display: <%=ViewPrivateParty()%>'>
                                    <label class="col-lg-3 control-label">
                                        المدعي     :                                                     
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:DropDownList ID="lstFiltercaseParty" class="Select2Drop" runat="server"></asp:DropDownList>

                                    </div>
                                </div>

                            </div>

                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الحكم :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterDession" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الحالة :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterStatus" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الموظف المختص :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterAssignedPerson" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label" for="" style="text-align: left">نتيجة الحكم: </label>

                                    <div class="col-md-9">

                                        <asp:DropDownList ID="lstfilterJudgmentresult" runat="server" class="Select2Drop">
                                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="لصالح المدعي " Value="1"></asp:ListItem>
                                            <asp:ListItem Text="لصالح المدعي عليه" Value="2"></asp:ListItem>
                                              <asp:ListItem Text="لم يصدر حكم" Value="3"></asp:ListItem>
                                        </asp:DropDownList>


                                    </div>
                                </div>
                            </div>


                        </fieldset>
                    </div>
                    <div class="text-right">


                        <asp:LinkButton ID="lnkSearch" class="btn btn-primary" runat="server" OnClick="lnkSearch_Click">&nbsp;&nbsp; بحث&nbsp;&nbsp; <i class="icon-search4 position-right"></i></asp:LinkButton>


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
                                                    نتيجة البحث
											<asp:LinkButton ID="lnkSearchback" CssClass="control-arrow" runat="server" OnClick="lnkSearchback_Click"> رجوع  <i class=" icon-backward2"></i></asp:LinkButton>

                                                 

                                                </legend>


                                                 <div style="direction: ltr;background:#ffffff;padding:10px;margin:0 auto;width:1150px" class="viewerbg">
                                                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" ZoomMode="FullPage"
                                                        Font-Names="Verdana" Font-Size="8pt" Width="100%" Height="900px" ProcessingMode="Local"
                                                        ShowParameterPrompts="False" ShowCredentialPrompts="False"
                                                        ShowFindControls="False" ShowZoomControl="true" CssClass="ReportViewer" BackColor="White" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" PageCountMode="Actual" SizeToReportContent="True" BorderColor="#CCCCCC">
                                                    </rsweb:ReportViewer>
    
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

     
    <!--END CONTENT-->
    <!--BEGIN FOOTER-->
    <div id="scanLoading" class="scanLoading" style="display: none">
        <img src="/Layout/images/scan-document.gif" />
    </div>
</asp:Content>
