<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Empty.Master" AutoEventWireup="true" CodeBehind="AgreementMainAttachments.aspx.cs" Inherits="UI.Web.Agreements.Forms.AgreementMainAttachments" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">

    <script language="JavaScript" type="text/javascript">
        function chkImage() {
            var txt = document.getElementById("<%=lstAttachmentType.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل نوع المرفق");
                txt.focus();
                return false;
            }


            return true;
        }

        function setactiveTab(tabindex) {
            //alert("para:" + tabindex)
            var txt = document.getElementById("<%=hdnactivetab.ClientID %>");
            txt.value = tabindex;

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
                    <input id="hdnAgreementCode" runat="server" type="hidden" />
                     <input id="hdnRelatedAgreement" runat="server" type="hidden" />
                    <input id="hdnactivetab" runat="server" type="hidden" />
                    <input id="hdnIsInitial" runat="server" type="hidden" />

            <input id="hdnScannerfilepath" runat="server" type="hidden" />
                    <input id="hdnScannerfilepath2" runat="server" type="hidden" />


                    <div class="tabbable">
                        <ul class="nav nav-tabs nav-tabs-highlight">
                            <li class="<%=activeTab(1) %>" onclick="setactiveTab(1)"><a href="#badges-tab1" data-toggle="tab">مرفقات الاتفاقية  <%= getAgreementType(getBool(hdnIsInitial.Value)) %><span class="badge badge-success  position-right">
                                <asp:Label ID="lblAttchmentCount" runat="server" Text="0"></asp:Label></span></a></li>
                            <li class="<%=activeTab(2) %>" onclick="setactiveTab(2)" style="display: <%= showRelatedFiles()%>"><a href="#badges-tab2" data-toggle="tab">مرفقات الاتفاقية
                                <asp:Label ID="lbltrlatedType" runat="server"></asp:Label><span class="badge badge-success  position-right">
                                    <asp:Label ID="lblAttchment2Count" runat="server" Text="0"></asp:Label></span></a></li>



                        </ul>
                        <div class="tab-content">
                            <div class="tab-pane <%=activeTab(1) %>" id="badges-tab1">

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
                                                                <label class="col-md-3 control-label" for="">نوع المرفق</label>

                                                                <div class="col-md-9">
                                                                    <asp:DropDownList ID="lstAttachmentType" class="form-control" runat="server"></asp:DropDownList>
                                                                </div>
                                                            </div>



                                                            <div class="form-group">
                                                                <label class="col-md-3 control-label" for="">تاريخ الوثيقة</label>

                                                                <div class="col-md-9">



                                                                    <div class="input-group">
                                                                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                                        <%--	<input class="form-control pickadate-selectors picker__input picker__input--active" value="03/18/2013" type="text">--%>
                                                                        <asp:TextBox ID="txtCreationDate" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                                                    </div>


                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-md-3 control-label">المرجع</label>

                                                                <div class="col-md-9">

                                                                    <asp:TextBox runat="server" class="form-control" ID="txtRef"></asp:TextBox>


                                                                </div>

                                                            </div>
                                                            <div class="form-group" style="display: none">
                                                                <label class="col-md-3 control-label">الموضوع</label>

                                                                <div class="col-md-9">

                                                                    <asp:TextBox runat="server" class="form-control" ID="txtSubject"></asp:TextBox>


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

                                                        <div class="col-md-12">
                                                            <div class="form-actions">
                                                                <div class="col-md-offset-3 col-md-9">
                                                                    <asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary" OnClick="btnSave_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

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
                                                    <asp:DataGrid runat="server" ID="grdData" AutoGenerateColumns="False"
                                                        AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemDataBound="grdData_ItemDataBound" OnEditCommand="grdData_EditCommand">
                                                        <PagerStyle Visible="False" />
                                                        <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                                        <Columns>
                                                            <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
  <asp:TemplateColumn HeaderText="نوع المرفق">

                                                                <HeaderStyle HorizontalAlign="right" />
                                                                <ItemTemplate>
                                                                    <%#Eval("AgreementAttachmentTypes.NameAr") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="ReceiveDate" HeaderText="تاريخ المرفق" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="UploadDate" HeaderText="تاريخ الرفع" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>

                                                            <asp:BoundColumn DataField="AttacheSubject" Visible="false" HeaderText=" الموضوع"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="AttachRef" HeaderText="مرجع"></asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="عرض المرفق">
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                 <%--   <%#Eval("Filepath") %>--%>
                                                                    <a target="_blank" href="<%#ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath +"&vfileList=["+gets(Eval("Filepath"))+";]" %>">عرض المرفق

                                                 <i class="icon-attachment"></i>&nbsp;
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

                            <div class="tab-pane <%=activeTab(2) %>" id="badges-tab2">


                                <div class="row">
                                    <asp:Label runat="server" ID="Label1"></asp:Label>
                                    <div class="col-lg-12">
                                        <div class="portlet box portlet-blue" id="tblAdd2" runat="server" visible="false">
                                            <div class="portlet-header">
                                                <div class="caption">
                                                    <asp:Label runat="server" ID="Label2">إضافة جديد</asp:Label>
                                                </div>

                                            </div>
                                            <div class="portlet-body">
                                                <div role="form" class="form-horizontal">
                                                    <div class="row">

                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label class="col-md-3 control-label" for="">نوع المرفق</label>

                                                                <div class="col-md-9">
                                                                    <asp:DropDownList ID="lstAttachmentType2" class="form-control" runat="server"></asp:DropDownList>
                                                                </div>
                                                            </div>



                                                            <div class="form-group">
                                                                <label class="col-md-3 control-label" for="">تاريخ الوثيقة</label>

                                                                <div class="col-md-9">



                                                                    <div class="input-group">
                                                                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                                        <%--	<input class="form-control pickadate-selectors picker__input picker__input--active" value="03/18/2013" type="text">--%>
                                                                        <asp:TextBox ID="txtCreationDate2" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                                                    </div>


                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-md-3 control-label">المرجع</label>

                                                                <div class="col-md-9">

                                                                    <asp:TextBox runat="server" class="form-control" ID="txtRef2"></asp:TextBox>


                                                                </div>

                                                            </div>
                                                            <div class="form-group" style="display: none">
                                                                <label class="col-md-3 control-label">الموضوع</label>

                                                                <div class="col-md-9">

                                                                    <asp:TextBox runat="server" class="form-control" ID="txtSubject2"></asp:TextBox>


                                                                </div>

                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-md-3 control-label">الملف</label>

                                                                <div class="col-md-6">
                                                                    <asp:Label ID="lblimage2" runat="server"></asp:Label>
                                                                    <asp:FileUpload ID="txtImage2" runat="server" class="file-styled" />
                                                                    <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <span class="help-block2">| Or | </span>
                                                                    <asp:LinkButton runat="server" ID="lnkScan2" class="btn btn-info btn-xs" OnClick="lnkScan2_Click"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                                </div>

                                                            </div>


                                                        </div>

                                                        <div class="col-md-12">
                                                            <div class="form-actions">
                                                                <div class="col-md-offset-3 col-md-9">
                                                                    <asp:LinkButton ID="btnSave2" runat="server" class="btn btn-primary" OnClick="btnSave2_Click1"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

                                                                    &nbsp;
				                               <asp:Button runat="server" ID="btnCancel2" class="btn btn-default" Text=" الغاء " OnClick="btnCancel2_Click" />

                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="row" id="tblshow2" runat="server">
                                    <div class="col-lg-12">
                                        <div class="portlet box">
                                            <div class="portlet-header">

                                                <div class="actions pull-right" style="margin-bottom: 10px;">

                                                    <asp:LinkButton runat="server" ID="btnNew2" class="btn btn-info btn-xs" OnClick="btnNew2_Click"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

                                                    <asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="btnDelete2" class="btn btn-danger btn-xs" OnClick="btnDelete2_Click"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

                                                </div>
                                            </div>
                                            <div class="portlet-body">


                                                <div class="datatable-scroll">
                                                    <asp:DataGrid runat="server" ID="grdData2" AutoGenerateColumns="False"
                                                        AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemDataBound="grdData2_ItemDataBound" OnEditCommand="grdData2_EditCommand">
                                                        <PagerStyle Visible="False" />
                                                        <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                                        <Columns>
                                                            <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="نوع المرفق">

                                                                <HeaderStyle HorizontalAlign="right" />
                                                                <ItemTemplate>
                                                                    <%#Eval("AgreementAttachmentTypes.NameAr") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="ReceiveDate" HeaderText="تاريخ المرفق" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="UploadDate" HeaderText="تاريخ الرفع" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>

                                                            <asp:BoundColumn DataField="AttacheSubject" Visible="false" HeaderText=" الموضوع"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="AttachRef" HeaderText="مرجع"></asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="عرض المرفق">
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <a target="_blank" href="<%#ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath2 +"&vfileList=["+gets(Eval("Filepath"))+";]" %>" style="<%#showHideAttachment(gets(Eval("Filepath")))%>">عرض المرفق

                                                 <i class="icon-attachment"></i>&nbsp;
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

                                                </div>
                                                <div class="datatable-footer">
                                                    <div class="dataTables_info" id="DataTables_Table_3_info" role="status" aria-live="polite">
                                                        <asp:Label ID="lblcount2" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">

                                                        <cc1:Pager CurrentIndex="1" OnCommand="pager2_Command" ShowFirstLast="False" ID="pager2"
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
            </div>


        </div>

        <!--END CONTENT-->
        <!--BEGIN FOOTER-->

       
</asp:Content>
