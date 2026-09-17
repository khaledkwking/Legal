<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LawSearch.aspx.cs" Inherits="UI.Web.Modules.Laws.Forms.LawSearch" %>

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
    /*color: #2980b9;*/ /* أزرق */
    color: #e74c3c;
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
    /*color: #3498db;*/ /* أزرق فاتح */
     color: #e74c3c;
}
.status-alt-blue::before {
    content: "\1F9E9 "; /* 🧩 */
}
.law-summary-item{
    font-size: 15px;
}
#cboxOverlay {
    background: #f443361f  !important;
}
 .highlight-search {
     background-color: #FFFF00;
     color: #000;
     font-weight: bold;
     padding: 3px 6px;
     border-radius: 3px;
     box-shadow: 0 0 3px rgba(255, 193, 7, 0.5);
 }

    .sort-radio-list input {
        margin-right: 20px;
    }

    .sort-radio-list label {
        margin-right: 10px;
    }


    </style>
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
                                <label class="col-lg-3 control-label">الترتيب:</label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:RadioButtonList 
                                        ID="RٍButtonSortList" 
                                        runat="server" 
                                        RepeatDirection="Horizontal"
                                        CssClass="sort-radio-list" AutoPostBack="True" OnSelectedIndexChanged="RٍButtonSortList_SelectedIndexChanged">

                                        <asp:ListItem Value="1" Selected="True">تنازليا</asp:ListItem>

                                        <asp:ListItem Value="2">تصاعديا </asp:ListItem>

                                    </asp:RadioButtonList>
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
        <div class="panel panel-flat" style="background-color:white !important;">
            <div class="panel-body">
                <asp:DataList ID="dlLawHierarchy" runat="server" RepeatLayout="Flow" OnItemDataBound="dlLawHierarchy_ItemDataBound">
                    <ItemTemplate>
                        <div class="law-summary-item" id="rowContainer" runat="server" style="padding-right:5%">
                 <a target="_blank" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath+gets(Eval("ChildDocId"))+"/" +"&vfileList=["+gets(Eval("DocFilepath"))+";]" %>' id="file1" runat="server">

                            <asp:Label ID="lblItemIndex" runat="server" ForeColor="Black"></asp:Label> -
                               <asp:Label 
                                ID="lblDocInfo" 
                                runat="server" 
                                ForeColor="#006ac2"
                                Text='<%# Eval("DocTypeName") + " رقم " + Eval("DocNum") + " لسنة " + Eval("DocYear") %>'>
                            </asp:Label>
                        &nbsp;&nbsp;-&nbsp;&nbsp;
                            <asp:Label 
                                ID="Label1" 
                                runat="server" 
                                ForeColor="Black"
                                Text='<%# Eval("DocSubject") %>'>
                            </asp:Label>
                            <asp:Literal ID="litDocDescription" runat="server" Text='<%# Eval("DocDescriptionHTML") %>' ></asp:Literal>  
                          
                    </a>
                               <a href="javascript:void(0);" onclick="openLawDetailsDocData('<%# Eval("LastProcedureID") %>',1); return false;">
                            <asp:Label
                                ID="Label2"
                                runat="server"
                                Text='<%# "- يحتوي على " + Eval("DocProceduresTypesNameAr") %>'
                                Visible='<%# Eval("DocProceduresTypesNameAr") != null &&
                                           Eval("DocProceduresTypesNameAr").ToString().Contains("استدراك") %>'
                                ForeColor="#e74c3c"
                                Font-Bold="true"
                                Font-Size="15px">
                            </asp:Label>
                    </a>
                     
                 <%--    <asp:Label 
                                ID="litDocProcedureTypeName" 
                                runat="server" 
                                Text='<%# "- يحتوي على " + Eval("DocProceduresTypesNameAr") %>'
                                Visible='<%# Eval("DocProceduresTypesNameAr") != null 
                                    && Eval("DocProceduresTypesNameAr").ToString().Contains("استدراك") %>'
                                ForeColor="#e74c3c" 
                                Font-Bold="true" 
                                Font-Size="15px">
                            </asp:Label>--%>
                   
                            &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                     <a href="#" 
                           class="btn btn-primary btn-sm shadow-sm rounded-pill px-4"
                           data-toggle="modal" 
                           data-target="#modal_<%#Eval("ChildDocId") %>">
                            <i class="fa fa-file-text-o"></i> نص الوثيقة
                        </a>
                                &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                            <asp:Label 
                                ID="Label3" 
                                runat="server" 
                                class="btn-sm shadow-sm rounded-pill px-4"
                                Text='<%# Eval("DocSerial") %>' BorderStyle="NotSet"  Font-Bold="True" ForeColor="black"  Font-Size="11">
                            </asp:Label>

            <div id="modal_<%#Eval("ChildDocId") %>" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h5 class="modal-title">تفاصيل الوثيقة</h5>
                        </div>

                        <div class="modal-body">
                            <p>
                                <%--<%#HighlightSearchText(getsDocDetails(Eval("ChildDocId")), txtDetails.Text) %>--%>
                               
                                  <%# HighlightSearchText(getsDocDetails(Eval("ChildDocId")), txtFilterDetails.Text) .Replace("\r\n", "<br />").Replace("\n", "<br />") %>

                                </p>

        
                        </div>

                        <div class="modal-footer">
                            <button type="button" class="btn btn-link" data-dismiss="modal">إغلاق</button>

                        </div>
                    </div>
                </div>
            </div>
                            <hr />
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
