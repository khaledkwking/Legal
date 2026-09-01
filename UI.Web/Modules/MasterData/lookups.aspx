<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="lookups.aspx.cs" Inherits="UI.Web.Modules.MasterData.lookups" %>


<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>

<%@ Register Src="~/UserControls/DeleteConfirm.ascx"  TagPrefix="uc"  TagName="DeleteConfirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">


    <script language="JavaScript" type="text/javascript">
        function chkImage() {
            var txt = document.getElementById("<%=txtNameEn.ClientID %>")
            //if (txt.value == "") {
            //    new $.Zebra_Dialog("فضلا ، ادخل الاسم بالانجليزيه");
            //    txt.focus();
            //    return false;
            //}

            var txt = document.getElementById("<%=txtNameAr.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل الاسم بالعربيه");
                txt.focus();
                return false;
            }
            return true;
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
    <uc:DeleteConfirm ID="DeleteConfirm1" runat="server" />
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


                                                <div class="form-group" style="display:none">
                                                    <label class="col-md-3 control-label" for="">الاسم بالانجليزيه  </label>

                                                    <div class="col-md-9">
                                                        <asp:TextBox runat="server" ID="txtNameEn" placeholder="Enter English title" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">الاسم بالعربيه </label>

                                                    <div class="col-md-9">



                                                        <asp:TextBox runat="server" placeholder="Enter Arabic Title" class="form-control" ID="txtNameAr"></asp:TextBox>
                                                    </div>

                                                </div>
                                                <div class="form-group" id="divTypeId" runat="server" visible="false">
                                                    <label class="col-md-3 control-label">النوع</label>
                                                    <div class="col-md-9">
                                                        <asp:DropDownList runat="server" ID="ddlTypeId" class="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group" id="divSectorImage" runat="server" visible="false">
                                                    <label class="col-md-3 control-label">صورة القطاع</label>
                                                    <div class="col-md-9">
                                                        <asp:FileUpload runat="server" ID="txtSectorImage" class="file-styled" />
                                                        <asp:HiddenField runat="server" ID="hdnSectorImagePath" />
                                                        <asp:Image runat="server" ID="imgSectorPreview" CssClass="img-thumbnail" Style="max-height: 120px; margin-top: 8px; display: block;" />
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="col-md-12">
                                                <div class="form-actions">
                                                    <div class="col-md-offset-3 col-md-9">
                                                        <asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary" OnClick="btnSave_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

                                                        &nbsp;
				                               <asp:Button runat="server" ID="btnCancel" class="btn btn-default" Text=" الغاء  " OnClick="btnCancel_Click" /> 


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

                                    <div class="actions pull-right" style="margin-bottom:30px;">

                                        <asp:LinkButton runat="server" ID="btnNew" class="btn btn-info btn-xs"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

                                        <asp:LinkButton OnClientClick="return DeleteConfirm.show(this);" runat="server" ID="btnDelete" class="btn btn-danger btn-xs" OnClick="btnDelete_Click"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>


                                    </div>
                                </div>
                                <div class="portlet-body">
                                    <div class="row mbm" style="margin-bottom: 20px">

                                        <asp:PlaceHolder ID="phFilters" runat="server" />
                                        <div class="col-md-1">
                                            <div class="form-group" style="margin-top: 20px;">
                                                <asp:LinkButton runat="server" ID="btnFilter" class="btn btn-success dropdown-toggle" OnClick="btnFilter_Click"><i class="fa fa-search"></i>&nbsp;
                                                بحث</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="datatable-scroll">
                                <%-- <table cellspacing="0" rules="all" class="table table-striped table-bordered" border="1" id="grdData" style="border-collapse: collapse; display: none">
                              
                            </table>--%>

                                <asp:DataGrid runat="server" ID="grdData" AutoGenerateColumns="False"  
                                    AllowPaging="True" PageSize="20" class="table datatable-responsive" OnDeleteCommand="grdData_DeleteCommand" OnItemDataBound="grdData_ItemDataBound" OnEditCommand="grdData_EditCommand" OnPageIndexChanged="grdData_PageIndexChanged">
                                    <PagerStyle Visible="true" Mode="NumericPages" BackColor="#EFEFEF" />
                                    <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                    <Columns>
                                        <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Code" HeaderText="الكود" Visible="True"></asp:BoundColumn>
                                        <%--<asp:BoundColumn DataField="NameEn" Visible="false" HeaderText="الاسم بالانجليزيه"></asp:BoundColumn>--%>
                                        <asp:BoundColumn DataField="NameAr" HeaderText="الاسم بالعربيه "></asp:BoundColumn>
                                        <asp:BoundColumn DataField="TypeNameAr" Visible="false" HeaderText="النوع"></asp:BoundColumn>


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
                                    <asp:Label ID="lblcount" runat="server"></asp:Label></div>
                                <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">
                                    <%-- <a class="paginate_button previous disabled" aria-controls="DataTables_Table_3" data-dt-idx="0" tabindex="0" id="DataTables_Table_3_previous">→</a>
                        <span><a class="paginate_button current" aria-controls="DataTables_Table_3" data-dt-idx="1" tabindex="0">1</a>
                            <a class="paginate_button " aria-controls="DataTables_Table_3" data-dt-idx="2" tabindex="0">2</a>


                        </span>
                        <a class="paginate_button next" aria-controls="DataTables_Table_3" data-dt-idx="3" tabindex="0" id="DataTables_Table_3_next">←</a>--%>


                                     <%-- <cc1:Pager CurrentIndex="1" OnCommand="pager_Command" ShowFirstLast="False" ID="pager1"
                                        runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>
                                    --%>
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



   
</asp:Content>
