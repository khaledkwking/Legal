<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LawDocSearch.aspx.cs" Inherits="UI.Web.Modules.Laws.Forms.LawDocSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet">
    <style>
   .status {
    font-weight: bold;
    padding: 0 20px;
    border-radius: 3px;
    margin-right: 2px;
    display: inline-block;
}

.status-court {
    color: #c0392b; /* أحمر */
}
.status-court::before {
    content: "\2696 "; /* ⚖️ */
}

.status-amend {
    color: #2980b9; /* أزرق */
}
.status-amend::before {
    content: "\270F "; /* ✏️ */
}

.status-alt-red {
    color: #e74c3c; /* أحمر فاتح */
}
.status-alt-red::before {
    content: "\1F9E9 "; /* 🧩 */
}

.status-alt-blue {
    color: #3498db; /* أزرق فاتح */
}
.status-alt-blue::before {
    content: "\1F9E9 "; /* 🧩 */
}
.law-summary-item{
    font-size: 15px;
}

    </style>
    <div class="row mbl">
        <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title">نطاق البحث</h5>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <asp:Label runat="server" ID="lblerror"></asp:Label>
                    <fieldset class="content-group">
                        <legend class="text-semibold">
                            <i class="icon-file-text2 position-left"></i>
                            ادخل شروط البحث
                        </legend>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-lg-3 control-label">رقم الوثيقة:</label>
                                <div class="col-lg-3">
                                    <asp:TextBox ID="txtFilterSerialNum" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <label class="col-lg-3 control-label">سنة الاصدار:</label>
                                <div class="col-lg-3">
                                    <asp:TextBox ID="txtFilterSerialYear" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">تاريخ  إصدار الوثيقة من :</label>
                                <div class="col-lg-9">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                        <asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">تاريخ  انتهاء الوثيقة من :</label>
                                <div class="col-lg-9">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                        <asp:TextBox ID="txtFilterExpireFrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">جزء من نص الوثيقة :</label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:TextBox ID="txtFilterDetails" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">العمل التحضيري :</label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:DropDownList ID="lstfilterProceduretype" runat="server" class="Select2Drop"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-lg-3 control-label">نوع الوثيقة   :</label>
                                <div class="col-lg-9">
                                    <asp:DropDownList ID="lstFilterType" AutoPostBack="true" OnSelectedIndexChanged="lstFilterType_SelectedIndexChanged" class="Select2Drop" runat="server"></asp:DropDownList>
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
                                        <asp:TextBox ID="txtFilterExpireTo" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
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
                                <asp:Label runat="server" class="col-lg-3 control-label" ID="lblFilterCatTitle" Text="التصنيف"> التصنيف    :</asp:Label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:DropDownList ID="lstFilterCategory" class="Select2Drop" Width="100%" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">جزء من الموضوع :</label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:TextBox ID="txtFilterSubject" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3 control-label" for="">نشر بالجريدة الرسمية    : </label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="lstFilterPublish" class="Select2Drop" runat="server">
                                        <asp:ListItem Value="0" Text="الكل"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="نعم"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="لا"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">جزء من الملاحظات :</label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:TextBox ID="txtFilterNotes" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <div class="text-right">
                        <asp:LinkButton ID="lnkSearch" class="btn btn-primary" runat="server" OnClick="lnkSearch_Click">&nbsp;&nbsp; بحث&nbsp;&nbsp; <i class="icon-search4 position-right"></i></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row mbl" id="divResult" runat="server" visible="false">
        <div class="panel-heading">
            <h5 class="panel-title">
                نتيجة البحث
                <span style="color: #000; font-size: 14px;">(<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
            </h5>
        </div>
        <div class="panel panel-flat">
            
            <div class="panel-body">
                <asp:DataList ID="dlLawSummary" runat="server" RepeatLayout="Flow" OnItemCommand="dlLawSummary_ItemCommand" OnItemDataBound="dlLawSummary_ItemDataBound">
                    <ItemTemplate>
                        <div class="law-summary-item">
                            <asp:LinkButton ID="lnkToggle" runat="server" CommandName="Toggle" CommandArgument='<%# Eval("DestDocId") %>' CssClass="btn btn-default btn-xs">+</asp:LinkButton>
                            <a href='LawDocSearchDetails.aspx?destdocid=<%# Eval("DestDocId") %>'>
                                <asp:Label ID="Literal3" runat="server" Text='<%# (PageOffset + Container.ItemIndex + 1)%>' ForeColor="Black"></asp:Label> -
                                <asp:Literal ID="litDocDescription" runat="server" Text='<%# Eval("DocDescriptionHTML") %>'></asp:Literal>
                            </a>
                            <br /><br />
                            <div class="law-summary-subject" style="font-size:12px">
                                <%# Eval("DocSubject") %>
                                <hr style="font-weight:bold;" />
                            </div>
                            <asp:Panel ID="pnlRelated" runat="server" Visible="false" CssClass="well well-sm" style="margin-top: 10px;">
                                <asp:DataList ID="dlRelated" runat="server" RepeatLayout="Flow">
                                    <ItemTemplate>
                                                 <div class="law-summary-item" style="font-size:16px;">
               <a target="_blank" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath+gets(Eval("code"))+"/" +"&vfileList=["+gets(Eval("DocFilepath"))+";]" %>' id="file1" runat="server">

               
              <div>
                     <asp:Label ID="Literal3" runat="server" Text='<%# (PageOffset + Container.ItemIndex + 1)%>' ForeColor="Black"></asp:Label> -

                     <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("DocDescriptionHTML") %>'></asp:Literal>
                  
            
                     <asp:Label ID="Literal2" runat="server" Text='<%# Eval("DocSubject") %>' ForeColor="Black"></asp:Label>
                  
                  <hr style="font-weight:bold;" />
              </div>
              </a>
          </div>
                                    </ItemTemplate>
                                </asp:DataList>
                            </asp:Panel>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <div class="text-center" style="margin-top: 10px;">
                    <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-default btn-xs" OnClick="btnPrev_Click">السابق</asp:LinkButton>
                    <asp:Repeater ID="rptPages" runat="server" OnItemCommand="rptPages_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPage" runat="server" CssClass='<%# ((int)Container.DataItem == CurrentPageNumber) ? "btn btn-primary btn-xs" : "btn btn-default btn-xs" %>' CommandName="Page" CommandArgument='<%# Container.DataItem %>'><%# Container.DataItem %></asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-default btn-xs" OnClick="btnNext_Click">التالي</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
