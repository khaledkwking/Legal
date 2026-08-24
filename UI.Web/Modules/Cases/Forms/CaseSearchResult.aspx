<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Empty.Master" AutoEventWireup="true" CodeBehind="CaseSearchResult.aspx.cs" Inherits="UI.Web.cases.Forms.CaseSearchResult" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>

<%@ Register Src="~/UserControls/DeleteConfirm.ascx"  TagPrefix="uc"  TagName="DeleteConfirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">

     


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
                     

                    <div class="row" id="tblshow" runat="server">
                        <div class="col-lg-12">
                            <div class="portlet box">
                                
                                <div class="portlet-body">


                                    <div class="datatable-scroll">
                                            <asp:DataGrid ID="grdCases" runat="server"
                                                                            class="table table-hover table-striped table-bordered table-advanced tablesorter"
                                                                            AutoGenerateColumns="False"
                                                                            BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                                                                            CellPadding="3" Width="100%" OnItemDataBound="grdData_ItemDataBound" AllowPaging="true" >
                                                                            <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                            <ItemStyle CssClass="grdItem" />
                                                                            <AlternatingItemStyle CssClass="grdItem" />
                                                                            <HeaderStyle CssClass="grdHead" BackColor="Gray" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                            <FooterStyle CssClass="grdFoot" />
                                                                            <PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
                                                                                PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="CaseID" Visible="False"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CaseSerial" HeaderText="رقم القضية"></asp:BoundColumn>
                                                                                <%--<asp:BoundColumn DataField="CaseInternalSerial" HeaderText="الرقم الإلى  "></asp:BoundColumn>--%>
                                                                                <asp:BoundColumn DataField="CaseTransDate" HeaderText="تاريخ القضية " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="LastActionDate" HeaderText="اخر تحديث  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>

                                                                                <asp:TemplateColumn HeaderText="الدائره">


                                                                                    <ItemTemplate>

                                                                                        <%#Eval("CasesTypeAr")%>/<%#Eval("SuitType")  %>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>

                                                                                <asp:BoundColumn DataField="CasesTypeAr" HeaderText="الدائره"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="LitigationDegreeAr" HeaderText="الدرجه"></asp:BoundColumn>
                                                                                <%--<asp:BoundColumn DataField="CaseSubject" HeaderText="الموضوع"></asp:BoundColumn>--%>
                                                                                <asp:BoundColumn DataField="CaseStatusAr" HeaderText="الحاله"></asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="DecisionNameAr" HeaderText="الحكم"></asp:BoundColumn>

                                                                                <asp:TemplateColumn HeaderText="نتيجة الحكم ">
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>

                                                                                         <%#ShowJudgmentresult(gets(Eval("Judgmentresult"))) %>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>

                                                                                <asp:TemplateColumn HeaderText="مرفقات القضية">
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>

                                                                                        <a href="CaseAttachments.aspx?DocID=0&CaseID=<%#Eval("CaseID")%>&FileID=0" class="btn btn-default btn-xs iframe">
                                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                                                             مرفقات القضية <b>[<%#Eval("attachmentCount")%>]</b>
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>

                                                                                <asp:TemplateColumn HeaderText="التفاصيل">
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>

                                                                                        <a href="CasesData.aspx?FileID=<%#Eval("FileNumID") %>&CaseID=<%#Eval("CaseID") %>" class="btn btn-default btn-xs">
                                                                                            <i class="fa fa-file"></i>&nbsp;
                                                التفاصيل
                                                                                        </a>


                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>


                                                                              <%--  <asp:TemplateColumn>
                                                                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('are you sure you want to delete selected items?');" CommandName="delete" runat="server">  <i class="fa fa-trash" style="color:##333"></i>&nbsp;</asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>--%>
                                                                                            <%-- </asp:TemplateColumn>--%>

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
                                                runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>


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

     <uc:DeleteConfirm runat="server" />
 </asp:Content>
