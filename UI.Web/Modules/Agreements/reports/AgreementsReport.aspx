<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="AgreementsReport.aspx.cs" Inherits="UI.Web.Agreements.reports.AgreementsReport" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
  

    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        الاتفاقيات - التقارير  
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
											 
                            </legend>




                            <div class="col-md-5">

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        مسلسل :                                                       
                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterNum" runat="server" class="form-control" placeholder="رقم"></asp:TextBox>


                                    </div>
                                     <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterYear" runat="server" class="form-control" placeholder="سنة"></asp:TextBox>


                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        كلمة فى الموضوع
                                    </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="txtPartofName" runat="server" class="form-control"></asp:TextBox>


                                    </div>
                                </div>




                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الجهة/الدولة:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterOrg" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

<div class="form-group">
                                    <label class="col-lg-3 control-label">جهة الإختصاص :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterAgrRelatedOrgs" class="Select2Drop" runat="server"></asp:DropDownList>
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
                                    <label class="col-lg-3 control-label">التصنيف:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterCats" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">النوع:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterType" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group" >
                                    <label class="col-md-3 control-label" for="">حالة الاتفاقية   </label>

                                    <div class="col-md-9">

                                        <asp:DropDownList ID="lstFilterStatus" runat="server" class="form-control" >
                                            <asp:ListItem Text="الكل" Value="">                                         
                                            </asp:ListItem>
                                            <asp:ListItem Text="مبدئية" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="نهائية" Value="0"></asp:ListItem>
                                             <asp:ListItem Text="مبدئية >>> نهائية" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                </div>
<%--                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الحالة:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterStatusCode" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>--%>


                            </div>

                            <div class="col-md-4">

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">التاريخ من</label>
                                    <div class="col-lg-9">




                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control pickadate-selectors picker__input picker__input--active" value="03/18/2013" type="text">--%>
                                            <asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>


                                           
                                        </div>



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
                                    <label class="col-lg-3 control-label">الاجراء:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterProcedures" AutoPostBack="true" OnSelectedIndexChanged="lstFilterProcedures_SelectedIndexChanged" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>


                                <div class="form-group" id="divFilterRelatedOrg" runat="server" visible="false">
                                    <label class="col-md-3 control-label" for=""> محال إلى جهة الإختصاص  </label>

                                    <div class="col-md-9">

                                        <asp:DropDownList ID="lstFilterRelatedOrgs" runat="server" class="Select2Drop"></asp:DropDownList>
                                    </div>

                                </div>
                                  <div class="form-group">
                                    <label class="col-lg-3 control-label">الموظف المختص :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterAssignedPerson" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                            </div>


                        </fieldset>
                    </div>
                    <div class="text-right">


                        <asp:LinkButton ID="lnkSearch" class="btn btn-primary" runat="server" OnClick="lnkSearch_Click"> بحث <i class="icon-search4 position-right"></i></asp:LinkButton>


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



                                                 <div style="direction: ltr;background:#ffffff;padding:10px;margin:0 auto;width:1200px" class="viewerbg">
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
