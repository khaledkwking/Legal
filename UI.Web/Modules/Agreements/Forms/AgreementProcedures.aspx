<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Empty.Master" AutoEventWireup="true" CodeBehind="AgreementProcedures.aspx.cs" Inherits="UI.Web.Agreements.Forms.AgreementProcedures" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">

    <script language="JavaScript" type="text/javascript">
        function chkImage() {
           <%-- var txt = document.getElementById("<%=lstAttachmentType.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل نوع المرفق");
                txt.focus();
                return false;
            }--%>


            return true;
        }
    </script>
       <input id="hdnMasterID" runat="server" type="hidden" />

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
                         <asp:Label runat="server" ID="lblerror"></asp:Label>
                      
                                    <div class="form-horizontal" id="divAddProcedure" runat="server" visible="false">

                                        <div class="row">

                                            <div class="col-md-6">

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">الاجراء<span class="text-danger">*</span></label>

                                                    <div class="col-md-9">

                                                        <asp:DropDownList ID="lstProcedureType" runat="server" class="form-control"  OnSelectedIndexChanged="lstProcedureType_SelectedIndexChanged1" AutoPostBack="True"  ></asp:DropDownList>
                                                    </div>

                                                </div>
                                                <div class="form-group" id="divRelatedOrg" runat="server" visible="false">
                                                    <label class="col-md-3 control-label" for="">الجهة </label>

                                                    <div class="col-md-9">

                                                        <asp:DropDownList ID="lstRelatedOrgs" runat="server" class="form-control"></asp:DropDownList>
                                                    </div>

                                                </div>
                                                 <div class="form-group" id="divDessionNum" runat="server" visible="false">
                                                      <div class="col-md-6">

                                                          <asp:RadioButtonList ID="rblPublishType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" >
                                                              <asp:ListItem   Value="1">&nbsp;&nbsp;صدر بقانون&nbsp;&nbsp;</asp:ListItem>
                                                              <asp:ListItem Value="2">&nbsp;&nbsp;صدر بمرسوم&nbsp;&nbsp;</asp:ListItem>

                                                          </asp:RadioButtonList>
                                                      </div>
                                                      

                                                    <label class="col-md-3 control-label" for="">رقم القانوان | المرسوم  </label>

                                                    <div class="col-md-3">

                                                        <asp:TextBox ID="txtDessionNum" class="form-control" runat="server"></asp:TextBox>

                                                    </div>
                                                </div>


                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">التاريخ<span class="text-danger">*</span> </label>

                                                    <div class="col-md-9">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                            <asp:TextBox ID="txtProcedureActionDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                </div>

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">ملاحظات  </label>

                                                    <div class="col-md-9">

                                                        <asp:TextBox ID="txtRemarks" class="form-control" runat="server"></asp:TextBox>

                                                    </div>
                                                </div>
                                                <div id="divProAttache" runat="server">
                                                    إضافة مرفق للإجراء
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
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblimage" runat="server"></asp:Label>
                                                            <asp:FileUpload ID="txtImage" runat="server" class="file-styled" />
                                                            <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <span class="help-block2">| Or | </span>
                                                            <asp:LinkButton runat="server" ID="lnkScan" class="btn btn-info btn-xs" OnClick="lnkScan_Click"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                        </div>

                                                    </div>
                                                </div>

                                            </div>




                                            <div class="col-md-12">
                                                <div class="form-actions">
                                                    <div class="col-md-offset-9 col-md-12">
                                                        <asp:LinkButton ID="lnkSaveProcesure" runat="server" class="btn btn-primary" OnClientClick="return ValidateProcedures()" OnClick="lnkSaveProcesure_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

                                                        &nbsp;
				                               <asp:Button runat="server" ID="lnkCancelProcedure" class="btn btn-default" Text=" الغاء  " OnClick="lnkCancelProcedure_Click" />

                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                    <div class="row" id="divshowProcesure" runat="server">
                                        <div class="col-lg-12">
                                            <div class="portlet box">
                                                <div class="portlet-header">

                                                    <div class="actions pull-right" style="margin-bottom: 10px;">

                                                        <asp:LinkButton runat="server" ID="lnkAddNewProcedurew" class="btn btn-info btn-xs" OnClick="lnkAddNewProcedurew_Click"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

                                                        <asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="lnkDeleteProcedure" class="btn btn-danger btn-xs" OnClick="lnkDeleteProcedure_Click"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

                                                    </div>
                                                </div>
                                                <div class="portlet-body">
                                                    <div class="datatable-scroll" style="padding-top: 20px;">
                                                        <asp:DataGrid runat="server" ID="grdProcedure" AutoGenerateColumns="False"
                                                            AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer"  OnItemDataBound="grdData_ItemDataBound" OnEditCommand="grdData_EditCommand">
                                                            <PagerStyle Visible="False" />
                                                            <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                                            <Columns>
                                                                <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
                                                                <asp:TemplateColumn HeaderText="الاجراء">

                                                                    <HeaderStyle HorizontalAlign="right" />
                                                                    <ItemTemplate>
                                                                        <%#Eval("Agreement_ProcedureTypes.NameAr") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Remarks" HeaderText="ملاحظات"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ActionDate" HeaderText="تاريخ التسجيل " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ProcedureDate" HeaderText="تاريخ الاجراء " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>



                                                                <asp:TemplateColumn HeaderText="مرفقات">
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>

                                                                        <a href="AgreementAttachments.aspx?ProcedureID=<%#Eval("code")%>&AgreementCode=<%#Eval("AgreementCode")%>" class="iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                            مرفقات          
                                                                        </a>
                                                                        <%--  <a href="dddd.aspx" class="btn btn-default btn-xs iframe">
                                                                            <i class="fa fa-file"></i>&nbsp;
                                                                            مرفقات          
                                                                        </a>--%>
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
