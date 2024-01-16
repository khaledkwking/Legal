<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="userSystemsDeliveryForm.aspx.cs" Inherits="UI.Web.Admin.Pages.userSystemsDeliveryForm" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">



    <asp:UpdatePanel runat="server" ID="Updatepanel1" ChildrenAsTriggers="true" UpdateMode="conditional">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>



    <div class="row mbl">

        <div class="col-lg-12">
            <div class="panel">
                <div class="panel-body">



                    <div class="row">
                        <div class="col-lg-12">
                            <div class="portlet box">

                                <div class="portlet-body">

                                    <div style="direction: ltr; background: #ffffff; padding: 10px;margin:0 auto;width:900px" class="viewerbg">
                                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" ZoomMode="FullPage"
                                            Font-Names="Verdana" Font-Size="8pt" Width="100%" Height="900px" ProcessingMode="Local"
                                            ShowParameterPrompts="False" ShowCredentialPrompts="False"
                                            ShowFindControls="False" ShowZoomControl="true" CssClass="ReportViewer" BackColor="White" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" PageCountMode="Actual" SizeToReportContent="True" BorderColor="#CCCCCC">
                                        </rsweb:ReportViewer>
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
