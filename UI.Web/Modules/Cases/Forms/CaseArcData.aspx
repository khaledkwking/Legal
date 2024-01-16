<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Empty.Master" AutoEventWireup="true" CodeBehind="CaseArcData.aspx.cs" Inherits="UI.Web.cases.Forms.CaseArcData" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">

    <script language="JavaScript" type="text/javascript">
        function chkImage() {
         


            return true;
        }
          function ValidateIncoming() {
            var txt = document.getElementById("<%=txtDoc_Serial.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل رقم الوثيقة     ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtFrom.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل وارد من       ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtDoc_Subject.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  الموضوع     ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtComingDate.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  تاريخ الوثيقه     ");
                txt.focus();
                return false;
            }

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
    <div class="row mbl">
        <asp:Label ID="lblerror" runat="server" ></asp:Label>
         
                                            <div class="form-horizontal" id="divAddIncoming" runat="server" visible="false">

                                                <div class="row">

                                                    <div class="col-md-4">

                                                        <div class="form-group">
                                                            <label class="col-md-3 control-label" for="">رقم الكتاب<span class="text-danger">*</span></label>

                                                            <div class="col-md-9">

                                                                <asp:TextBox ID="txtDoc_Serial" class="form-control" runat="server"></asp:TextBox>
                                                            </div>

                                                        </div>

                                                        <div class="form-group">
                                                            <label class="col-md-3 control-label" for="">الموضوع <span class="text-danger">*</span></label>

                                                            <div class="col-md-9">

                                                                <asp:TextBox ID="txtDoc_Subject" class="form-control" runat="server"></asp:TextBox>
                                                            </div>

                                                        </div>


                                                        <div class="form-group">
                                                            <label class="col-md-3 control-label" for="">ملاحظات  </label>

                                                            <div class="col-md-9">

                                                                <asp:TextBox ID="txtComingNotes" class="form-control" runat="server"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <%--   <div id="divProAttache" runat="server">
                                            إضافة مرفق  
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

                                                    <div class="col-md-9">
                                                        <asp:Label ID="lblimage" runat="server"></asp:Label>
                                                        <asp:FileUpload ID="txtImage" runat="server" Visible="false" />
                                                        <asp:LinkButton runat="server" ID="lnkScan" class="btn btn-info btn-xs" OnClick="lnkScan_Click"  OnClientClick="return ValidateProcedures()" ><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>


                                                    </div>

                                                </div>--%>
                                                    </div>
                                                    <div class="col-md-4">

                                                        <div class="form-group">
                                                            <label class="col-md-3 control-label" for="">وارد من  <span class="text-danger">*</span></label>

                                                            <div class="col-md-9">

                                                                <asp:TextBox ID="txtFrom" class="form-control" runat="server"></asp:TextBox>
                                                            </div>

                                                        </div>
                                                         <div class="form-group">
                                                            <label class="col-md-3 control-label" for=""> صادر إلى  <span class="text-danger">*</span></label>

                                                            <div class="col-md-9">

                                                                <asp:TextBox ID="txtTo" class="form-control" runat="server"></asp:TextBox>
                                                            </div>

                                                        </div>

                                                        <div class="form-group">
                                                            <label class="col-md-3 control-label">التاريخ<span class="text-danger">*</span> </label>

                                                            <div class="col-md-9">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                                    <asp:TextBox ID="txtComingDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
                                                                </div>

                                                            </div>

                                                        </div>

                                                        <div class="form-group">
                                                            <label class="col-md-3 control-label">تاريخ التذكيير  </label>

                                                            <div class="col-md-9">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                                    <asp:TextBox ID="txtComingReminderDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
                                                                </div>

                                                            </div>

                                                        </div>
                                                    </div>
                                                      <div class="col-md-4">

                                                        <div class="form-group">
                                                            <label class="col-md-4 control-label" for="">تسجيل جلسة  </label>

                                                            <div class="col-md-2">

                                                                <asp:CheckBox ID="chkHasHearing" class="form-control" onclick="ValidateHeading()" runat="server" />
                                                            </div>

                                                        </div>
 

                                                        <div class="form-group" id="hearingdatecontainer" style="display:none">
                                                            <label class="col-md-4 control-label">تاريخ الجلسه<span class="text-danger">*</span> </label>

                                                            <div class="col-md-8">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                                    <asp:TextBox ID="txtHeadingDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
                                                                </div>

                                                            </div>

                                                        </div>
                                                    </div>

                                                </div>

                                                <div class="col-md-12">
                                                    <div class="form-actions">
                                                        <div class="col-md-offset-9 col-md-12">
                                                            <asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary" OnClientClick="return ValidateIncoming()" OnClick="lnkSaveIncoming_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

                                                            &nbsp;
				                                                   <asp:Button runat="server" ID="btnCancel" class="btn btn-default" Text=" الغاء  " OnClick="lnkCancelIncoming_Click" />

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="row" id="divshowincoming" runat="server">
                                                <div class="col-lg-12">
                                                    <div class="portlet box">
                                                        <div class="portlet-header">

                                                            <div class="actions pull-right" style="margin-bottom: 10px;">

                                                                <asp:LinkButton runat="server" ID="Lnkincoming" class="btn btn-info btn-xs" OnClick="Lnkincoming_Click"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

                                                                <asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="lnkDeleteIncoming" class="btn btn-danger btn-xs" OnClick="lnkDeleteIncoming_Click"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

                                                            </div>
                                                        </div>
                                                        <div class="portlet-body">
                                                            <div class="datatable-scroll" style="padding-top: 20px;">

                                                                <asp:DataGrid runat="server" ID="grdincoming" AutoGenerateColumns="False"
                                                                    AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemCommand="grdincoming_ItemCommand" OnEditCommand="grdincoming_EditCommand">
                                                                    <PagerStyle Visible="False" />
                                                                    <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                                                    <Columns>
                                                                           
                                                        <%--    <asp:TemplateColumn HeaderText="جلسة  ">
                                                                <ItemStyle HorizontalAlign="left" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="left" />

                                                                <ItemTemplate>
                                                                    <%#ShowYesNo(getBool(Eval("IsHearing"))) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>--%>
                                                                        <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>

                                                                        <asp:TemplateColumn HeaderText="النوع">
                                                                            <ItemStyle Width="3%" />
                                                                            <ItemTemplate>
                                                                                <%#fillDocType(gets(Eval("Doc_Type"))) %>
                                                                                
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="Doc_Serial" HeaderText="رقم الكتاب"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Doc_From" HeaderText=" من "></asp:BoundColumn>
                                                                         <asp:BoundColumn DataField="Doc_To" HeaderText=" إلى "></asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="SentDate" HeaderText="التاريخ  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Doc_Subject" HeaderText="الموضوع "></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Doc_Notes" HeaderText="ملاحظات "></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="NextFollowReminderDate" HeaderText="تاريخ التذكيير  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                           
                                                                        <asp:TemplateColumn HeaderText="مرفقات">
                                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemTemplate>

                                                                                <a href="CaseAttachments.aspx?DocID=<%#Eval("code") %>&CaseID=<%#Eval("RefDocID") %>&fileID=0" class="btn btn-default btn-xs">
                                                                                    <i class="icon-attachment"></i>&nbsp;
                                                مرفقات
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
                                                                <div class="row mbm">
                                                                    <div class="col-lg-12">
                                                                        <div class="pagination-panel">
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                            <div class="datatable-footer">
                                                                <div class="dataTables_info" id="DataTables_Table_3_info" role="status" aria-live="polite">
                                                                    <asp:Label ID="lblComingCount" runat="server"></asp:Label>
                                                                </div>
                                                                <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">
                                                                    <%-- <a class="paginate_button previous disabled" aria-controls="DataTables_Table_3" data-dt-idx="0" tabindex="0" id="DataTables_Table_3_previous">→</a>
                        <span><a class="paginate_button current" aria-controls="DataTables_Table_3" data-dt-idx="1" tabindex="0">1</a>
                            <a class="paginate_button " aria-controls="DataTables_Table_3" data-dt-idx="2" tabindex="0">2</a>


                        </span>
                        <a class="paginate_button next" aria-controls="DataTables_Table_3" data-dt-idx="3" tabindex="0" id="DataTables_Table_3_next">←</a>--%>


                                                                    <cc1:Pager CurrentIndex="1" OnCommand="pager_Command2" ShowFirstLast="False" ID="pager2"
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
    

    <!--END CONTENT-->
    <!--BEGIN FOOTER-->
</asp:Content>
