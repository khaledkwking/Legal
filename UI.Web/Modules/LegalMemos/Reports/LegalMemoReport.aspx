<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LegalMemoReport.aspx.cs" Inherits="UI.Web.Modules.LegalMemos.Reports.LegalMemoReport" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">



    <input id="hdnIncomingScannedFilePath" runat="server" type="hidden" />
    <input id="hdnfilterRequestedFrom" runat="server" type="hidden" />
    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        <%= _PageTitle %>
                    </h4>
                </div>

            </div>

        </div>
    </div>


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




                            <div class="col-md-4">



                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        رقم الملف:

                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterSerialNum" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <label class="col-lg-3 control-label">
                                        السنة  :

                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterSerialYear" runat="server" class="form-control"></asp:TextBox>
                                    </div>


                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">تاريخ  إصدار الوثيقة  من :</label>
                                    <div class="col-lg-9">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
                                            <asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                        </div>



                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">تاريخ الإجراء من :</label>
                                    <div class="col-lg-9">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
                                            <asp:TextBox ID="txtFilterFollowDateFrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                        </div>



                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        الإجراءات :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:DropDownList ID="lstfilterProcedure" runat="server" class="Select2Drop"></asp:DropDownList>

                                    </div>

                                </div>
                            </div>

                            <div class="col-md-4">

                                <div class="form-group" style="display: none">
                                    <label class="col-lg-3 control-label">
                                        التصنيف :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:DropDownList ID="lstFilterCategory" class="Select2Drop" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        الرقم الاّلي  :

                                    </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="txtFilterAutoNum" runat="server" class="form-control"></asp:TextBox>
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
                                    <label class="col-lg-3 control-label">إلى  :</label>
                                    <div class="col-lg-9">

                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <asp:TextBox ID="txtFilterFollowDateTo" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>

                                        </div>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        الجهة :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:DropDownList ID="lstfilterOrg" class="Select2Drop" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                </div>



                                <div class="form-group" style="display: none">
                                    <label class="col-md-3 control-label" for="">قيد الدراسة: </label>

                                    <div class="col-md-9">


                                        <asp:DropDownList ID="lstFilterIsUnderStudy" class="Select2Drop" runat="server">
                                            <asp:ListItem Value="0" Text="الكل"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="نعم"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="لا"></asp:ListItem>
                                        </asp:DropDownList>


                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">




                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        جزء من الموضوع :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:TextBox ID="txtFilterSubject" runat="server" class="form-control"></asp:TextBox>


                                    </div>



                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        الموظف المختص :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:DropDownList ID="lstfilterAssignedEmployee" class="Select2Drop" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group" style="display:none">
                                    <label class="col-md-3 control-label" for="">المستشار القانوني : </label>

                                    <div class="col-md-9">
                                        <asp:DropDownList ID="lstFilterConsultant" runat="server" class="Select2Drop"></asp:DropDownList>
                                    </div>
                                </div>


                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        الحالة :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:DropDownList ID="lstFilterStatus" runat="server" class="Select2Drop"></asp:DropDownList>

                                    </div>

                                </div>


                                <div class="form-group" style="padding: 10px; background: #9dc5e2">
                                    <label class="col-lg-3 control-label">عرض حسب  :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstgroupping" class="Select2Drop" runat="server"></asp:DropDownList>
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
                                                    نتيجة البحث
											<asp:LinkButton ID="lnkSearchback" CssClass="control-arrow" runat="server" OnClick="lnkSearchback_Click"> رجوع  <i class=" icon-backward2"></i></asp:LinkButton>



                                                </legend>


                                                <div style="direction: ltr; background: #ffffff; padding: 10px; margin: 0 auto; width: 1150px" class="viewerbg">
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


</asp:Content>
