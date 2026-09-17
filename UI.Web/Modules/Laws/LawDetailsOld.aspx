<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LawDetailsOld.aspx.cs" Inherits="UI.Web.Modules.Laws.LawDetailsOld"  ValidateRequest="false"%>
<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LawDetails.aspx.cs" Inherits="UI.Web.Modules.Laws.LawDetails" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
              .page-container {
    position: relative;
    padding: 20px 10px;
    padding-bottom: 40px;
}
                    .sectors-banner {
        background-image: url('/Layout/uploads/MedalFiles/Banner.png');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: top center;
        width: 100%;                   /* full width of the screen */
    height: 900px;
    max-height: 80vh;
    min-height: 320px;
        position: relative;
        margin-bottom: 20px;
    }

    /* Optional: for very small screens */
    @media (max-width: 768px) {
        .sectors-banner {
        height: 360px;
        }
    }

.sectors-banner-text {
    position: absolute;
    top: 50%;
    right: 0;
    transform: translate(-50%, -50%);
    color: #ffffff;
    font-size: 60px;
    font-weight: bold;
    text-shadow: 0 2px 4px rgba(0,0,0,.5);
}
        .law-summary-item {
            font-size: 15px;
        }
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


    #<%= rblDocType.ClientID %> label {
        margin-left: 25px;
    }
        #<%= RadioButtonRelatedList.ClientID %> label {
        margin-left: 25px;
    }
       
.tooltip {
    position: relative;
    display: inline-block;
    cursor: pointer;
    font-family: 'Segoe UI', Arial, sans-serif;
}

.tooltip .tooltiptext {
    visibility: hidden;
    width: 220px;
    background-color: #FFD700; /* Yellow */
    color: #333333;           /* Elegant dark font */
    text-align: center;
    border-radius: 8px;
    padding: 8px 12px;
    font-size: 14px;
    font-weight: 500;
    box-shadow: 0 4px 10px rgba(0,0,0,0.15);

    position: absolute;
    z-index: 1;
    bottom: 125%;
    left: 50%;
    transform: translateX(-50%);
}

.tooltip:hover .tooltiptext {
    visibility: visible;
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

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
    <asp:HiddenField ID="hdnSectorId" runat="server" />
    <asp:HiddenField ID="hdnTypeId" runat="server" />
         <div class="sectors-banner">
     <div class="sectors-banner-text">
         <asp:Label ID="lblTitle" runat="server"></asp:Label>
     </div>
 </div>
    <div style="position: relative; top: -180px;">
    <div class="row mbl">
        <div class="panel panel-flat" style="background-color:white !important;">
    <%--        <div class="panel-heading">
                <h5 class="panel-title">نطاق البحث</h5>
            </div>--%>
            <div class="panel-body" >
                <div class="form-horizontal">
                    <asp:Label runat="server" ID="lblerror"></asp:Label>
                    <fieldset class="content-group">
                        <legend class="text-semibold">
                            <i class="icon-file-text2 position-left"></i>
                            ادخل شروط البحث
                        </legend>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="col-lg-3 control-label">رقم الوثيقة:</label>
                                <div class="col-lg-9">
                                    <asp:TextBox ID="txtDocNum" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-lg-3 control-label">سنة الاصدار من:</label>
                                <div class="col-lg-9">
                                    <asp:TextBox ID="txtYearFrom" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display:none">
                                <label class="col-lg-3 control-label">سنة الاصدار إلى:</label>
                                <div class="col-lg-9">
                                    <asp:TextBox ID="txtYearTo" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <label class="col-lg-3 control-label">التصنيف:</label>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="ddlCategory" runat="server" class="Select2Drop" Width="100%"></asp:DropDownList>
                                </div>
                              
                            </div>
                        </div>
                    </fieldset>
                     <fieldset class="content-group">
                         <div class="col-md-6">
                              
                           <label class="col-lg-2 control-label">جزء من النص:</label>
                          <div class="col-lg-10">
                              <asp:TextBox ID="txtDetails" runat="server" class="form-control"></asp:TextBox>
                          </div>
                             </div>
                                 <div id="DocTypeDiv" runat="server">
                                <label class="col-lg-1 control-label">النوع:</label>
                                 <div class="col-lg-5">
                                    <%--<asp:DropDownList ID="ddlDocTypes" runat="server" class="Select2Drop" Width="100%"></asp:DropDownList>--%>
                                    <asp:RadioButtonList ID="rblDocType" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"  RepeatLayout="Flow" OnSelectedIndexChanged="rblDocType_SelectedIndexChanged">

                                    </asp:RadioButtonList>
                                </div>

                                   </div>
                        
                         </fieldset>
                      <fieldset class="content-group">
                         <div class="col-lg-6">
                                <label class="col-lg-2 control-label">الموضوع:</label>
                                <div class="col-lg-10">
                                    <asp:TextBox ID="txtDocSubject" runat="server" class="form-control"></asp:TextBox>
                                </div>
                             </div>
                            <div class="col-lg-3">
                          <asp:RadioButtonList ID="RadioButtonRelatedList" runat="server" RepeatDirection="Horizontal"  RepeatLayout="Flow" CssClass="radio-space" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonRelatedList_SelectedIndexChanged">
                               <asp:ListItem Text="مرتبط" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="الجميع بدون اظهار الارتباط" Value="0"></asp:ListItem>
                          </asp:RadioButtonList>
                      </div>
                          <div class="col-lg-3">
                                                     
                                <label class="col-lg-3 control-label">الترتيب:</label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:RadioButtonList 
                                        ID="RButtonSortList" 
                                        runat="server" 
                                        RepeatDirection="Horizontal"
                                        CssClass="sort-radio-list" AutoPostBack="True" OnSelectedIndexChanged="RٍButtonSortList_SelectedIndexChanged">

                                        <asp:ListItem Value="1" Selected="True">تنازليا</asp:ListItem>

                                        <asp:ListItem Value="2">تصاعديا </asp:ListItem>

                                    </asp:RadioButtonList>
                                </div>
                         </div>

                       <div class="text-right">
                         <asp:LinkButton ID="btnSearch" class="btn btn-primary" runat="server" OnClick="btnSearch_Click">&nbsp;&nbsp; بحث&nbsp;&nbsp; <i class="icon-search4 position-right"></i></asp:LinkButton>
                     </div>
                        
                       </fieldset>
                   
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
                <asp:DataList ID="dlLawDetails" runat="server" RepeatLayout="Flow" OnItemDataBound="dlLawDetails_ItemDataBound">
                    <ItemTemplate>
                        <div class="law-summary-item" id="rowContainer" runat="server" style="padding-right:5%">
                            <a target="_blank" href='<%# ((UI.Web.Modules.Laws.LawDetails)Page).GetAttachmentUrl(Eval("ChildDocId"), Eval("DocFilepath")) %>' id="file1" runat="server">
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

                            <%-- <a target="_blank" href='<%# GetAttachmentUrl(Eval("ChildDocId"), Eval("DocFilepath")) %>' id="A1" runat="server">
                                <asp:Label 
                                    ID="litDocProcedureTypeName" 
                                    runat="server" 
                                    Text='<%# "- يحتوي على " + Eval("DocProceduresTypesNameAr") %>'
                                    Visible='<%# Eval("DocProceduresTypesNameAr") != null 
                                        && Eval("DocProceduresTypesNameAr").ToString().Contains("تدراك") %>'
                                    ForeColor="#e74c3c" 
                                    Font-Bold="true" 
                                    Font-Size="15px">
                                </asp:Label>
                            </a>--%> &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
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
                            Text='<%# Eval("DocSerial") %>' BorderStyle="NotSet"  Font-Bold="True" ForeColor="white" BackColor="#990033" Font-Size="11">
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
                               
                                  <%# ((UI.Web.Modules.Laws.LawDetails)Page).HighlightSearchText(((UI.Web.Modules.Laws.LawDetails)Page).getsDocDetails(Eval("ChildDocId")), txtDetails.Text).Replace("\r\n", "<br />").Replace("\n", "<br />") %>

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
                            <asp:LinkButton ID="lnkPage" runat="server" CssClass='<%# ((int)Container.DataItem == ((UI.Web.Modules.Laws.LawDetails)Page).CurrentPageNumber) ? "btn btn-primary btn-xs" : "btn btn-default btn-xs" %>' CommandName="Page" CommandArgument='<%# Container.DataItem %>'><%# Container.DataItem %></asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-default btn-xs" OnClick="btnNext_Click">التالي</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
        </div>
   
    <script>
      
        function openLawDetailsDocData(childDocId, type) {
            //alert(childDocId);
            //alert(type);
            type = type || 0;
                $.colorbox({
                    href: '/Modules/Laws/LawDetailsDocData.aspx?ChildDocId='
                        + childDocId
                        + '&Type=' + type,
                    iframe: true,
                    width: '60%',
                    height: '75%'
                });

            return false;
    }
    
    </script>
</asp:Content>
