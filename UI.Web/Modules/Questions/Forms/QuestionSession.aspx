<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="QuestionSession.aspx.cs" Inherits="UI.Web.Questions.Forms.QuestionSession" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">

    <script language="JavaScript" type="text/javascript">
        function chkImage() {
            var txt = document.getElementById("<%=lstChapter.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر الفصل التشريعي ");
                txt.focus();
                return false;
            }


            var txt = document.getElementById("<%=txtNameAr.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل الاسم بالعربيه");
                txt.focus();
                return false;
            }
           // showscannerLoading();

            return true;
        }

        function showscannerLoading() {
            if (chkImage()) {
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
                        <%=_PageTitle %>
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
    <div class="row mbl">

        <div class="col-lg-12">
            <div class="panel">
                <div class="panel-body">

                    <div class="row">
                        <asp:Label runat="server" ID="lblerror"></asp:Label>
                        <div class="col-lg-12">
                            <div class="portlet box portlet-blue" id="tblAdd" runat="server" visible="false">
                                <div class="portlet-header">
                                    <div class="caption">
                                        <asp:Label runat="server" ID="lblSubTitle">إضافة جديد</asp:Label>
                                    </div>

                                </div>
                                <div class="portlet-body">
                                    <div role="form" class="form-horizontal">
                                        <div class="row">

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">  الفصل  التشريعي</label>

                                                    <div class="col-md-9">
                                                        <asp:DropDownList ID="lstChapter" AutoPostBack="false"   class="form-control" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                   




                                               
                                               
                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">الاسم بالعربية</label>

                                                    <div class="col-md-9">

                                                        <asp:TextBox runat="server" class="form-control" ID="txtNameAr"></asp:TextBox>


                                                    </div>

                                                </div>
                                                
                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">ترتيب العرض  </label>

                                                    <div class="col-md-9">

                                                        <asp:TextBox runat="server" class="form-control" ID="txtOrder" Text="0"></asp:TextBox>


                                                    </div>

                                                </div>

                                                 <div class="form-group" style="display:none">
                                                    <label class="col-md-3 control-label">الاسم بالانجليزية</label>

                                                    <div class="col-md-9">

                                                        <asp:TextBox runat="server" class="form-control" ID="txtNameEn"></asp:TextBox>


                                                    </div>

                                                </div>

                                            </div>

                                            <div class="col-md-12">
                                                <div class="form-actions">
                                                    <div class="col-md-offset-3 col-md-9">
                                                        <asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary" OnClick="btnSave_Click"><i class='fa fa-save'></i>&nbsp;  حفظ البيانات </asp:LinkButton>

                                                        &nbsp;
				                               <asp:Button runat="server" ID="btnCancel" class="btn btn-default" Text=" الغاء " OnClick="btnCancel_Click" />

                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="row" id="tblshow" runat="server">
                        <div class="col-lg-12">
                            <div class="portlet box">
                                <div class="portlet-header">

                                  <div class="actions pull-right" style="margin-bottom: 10px;">

                                        <asp:LinkButton runat="server" ID="btnNew" class="btn btn-info btn-xs" OnClick="btnNew_Click"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

                                        <asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="btnDelete" class="btn btn-danger btn-xs" OnClick="btnDelete_Click"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

                                    </div>
                                </div>
                                <div class="portlet-body">


                                    <div class="datatable-scroll">
                                        <div class="portlet-body">
                                    <div class="row mbm" style="margin-bottom: 20px">

                                        <div class="col-md-3">
                                            <div class="form-group">

                                                <span>    الفصل التشريعي:</span>
                                                <asp:DropDownList ID="lstFilterChapter"  AutoPostBack="false" class="form-control" runat="server"></asp:DropDownList>
                                            </div>

                                            
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group" style="margin-top: 20px;">
                                                <asp:LinkButton runat="server" ID="btnFilter" class="btn btn-success dropdown-toggle" OnClick="btnFilter_Click"><i class="fa fa-search"></i>&nbsp;
                                                تصفيه</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                        

                                        <asp:DataGrid runat="server" ID="grdData" AutoGenerateColumns="False"
                                            AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemDataBound="grdData_ItemDataBound" OnEditCommand="grdData_EditCommand" OnPageIndexChanged="grdData_PageIndexChanged1">
                                            <PagerStyle Visible="true"  Mode="NumericPages"/>
                                            <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                            <Columns>
                                                <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>

                                                <asp:TemplateColumn HeaderText="الفصل التشريعي">

                                                    <HeaderStyle HorizontalAlign="right" />
                                                    <ItemTemplate>
                                                        <%#Eval("Parliament_legislativeChapter.NameAr") %>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Namear" HeaderText=" دور الانعقاد "></asp:BoundColumn>

                                                <asp:BoundColumn DataField="NameEn" Visible="false" HeaderText=" الاسم بالانجليزية"></asp:BoundColumn>

                                                <asp:BoundColumn DataField="sessionOrder" HeaderText=" ترتيب العرض  "></asp:BoundColumn>
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

                                    </div>
                                    <div class="datatable-footer">
                                        <div class="dataTables_info" id="DataTables_Table_3_info" role="status" aria-live="polite">
                                            <asp:Label ID="lblcount" runat="server"></asp:Label>
                                        </div>
                                        <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">

<%--                                            <cc1:Pager CurrentIndex="1" OnCommand="pager_Command" ShowFirstLast="False" ID="pager1"
                                                runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>--%>


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

    <div id="scanLoading" class="scanLoading" style="display:none"><img src="/Layout/images/scan-document.gif" /></div>
</asp:Content>
