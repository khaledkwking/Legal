<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="CommitteeDetails.aspx.cs" Inherits="UI.Web.Modules.Laws.CommitteeDetails" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
 
    
    <style type="text/css">
    label {
        font-size: 14px;
    }

    .grdHead {
        <svg width="24px" height="25px" viewBox="0 0 24 25"><text x="-0.177446411929171" y="-0.925384615384615" fill="#000000" font-family="Arial" font-size="23.3034188034188" alignment-baseline="text-before-edge"> أو</text> <text x="15.0829450139795" y="5.20128205128205" fill="#000000" font-family="Calibri" font-size="16.1196581196581" alignment-baseline="text-before-edge"> </text> </svg> background-color: #FFFFFF;
        color: #000000;
        /*font-family:Tahoma;
	font-size:11px;*/
        font-weight: bold;
    }

    .grdFoot {
        background-color: #666666;
        color: #FFFFFF;
        /*font-family:Tahoma;
	font-size:11px;*/
        font-weight: bold;
        border-top: solid 2px #000000;
    }

    .grd td {
        border-left: solid 1px #000000;
        border-right: solid 1px #000000;
        border-top: solid 0px #000000;
        border-bottom: solid 0px #000000;
    }

    .grdFoot td {
        border: solid 1px #666666;
    }

    .grdHead td {
        border: solid 1px #000000;
    }

    .grd {
        border-color: #000000;
        /*font-family:Tahoma;*/
        border: solid 1px #000000;
    }

    .grdPager /* Disabled Pager Style */ {
        background-color: #EBE9E9;
        color: #AAAAAA;
        /*font-family:Tahoma;
	font-size:11px;*/
        font-weight: bold;
    }

    .grdItem {
        /*font-family:Tahoma ;
	font-size:11px;*/
        color: #555555;
        border-color: #000000;
        border-left: solid 1px #000000;
        border-right: solid 1px #000000;
        border-top: solid 0px #000000;
        border-bottom: solid 0px #000000;
        background: #ffffff;
    }

        .grdItem td {
            border-left: solid 1px #000000;
            border-right: solid 1px #000000;
            border-top: solid 0px #000000;
            border-bottom: solid 0px #000000;
        }
       
 /* المسافة بين الأزرار */
.smart-view-display {
    margin-top: 16px;
    margin-bottom: 16px;
    display: flex;
    flex-wrap: wrap;
    gap: 18px;
    align-items: center;
}

/* إخفاء الشكل الافتراضي للراديو */
.smart-view-display input[type="radio"] {
    position: absolute;
    opacity: 0;
    width: 0;
    height: 0;
}

/* الـ label يبقى فيه دائرة مرسومة بالـ CSS + النص */
.smart-view-display label {
    position: relative;
    display: inline-flex;
    align-items: center;
    padding-inline-start: 30px;
    cursor: pointer;
    font-size: 14px;
    color: #333;
    user-select: none;
    min-height: 22px;
}

/* الدائرة الخارجية */
.smart-view-display label::before {
    content: "";
    position: absolute;
    inset-inline-start: 0;
    top: 50%;
    transform: translateY(-50%);
    width: 20px;
    height: 20px;
    border-radius: 50%;
    border: 2px solid #adb5bd;
    background: #fff;
    transition: all 0.2s ease-in-out;
}

/* النقطة الداخلية (تظهر عند الاختيار) */
.smart-view-display label::after {
    content: "";
    position: absolute;
    inset-inline-start: 6px;
    top: 50%;
    transform: translateY(-50%) scale(0);
    width: 8px;
    height: 8px;
    border-radius: 50%;
    background: #0d6efd;
    transition: transform 0.15s ease-in-out;
}

/* حالة الاختيار */
.smart-view-display input[type="radio"]:checked + label::before {
    border-color: #0d6efd;
    box-shadow: 0 0 0 3px rgba(13, 110, 253, 0.15);
}

.smart-view-display input[type="radio"]:checked + label::after {
    transform: translateY(-50%) scale(1);
}

/* تأثير خفيف عند المرور بالماوس */
.smart-view-display label:hover::before {
    border-color: #0d6efd;
}

</style>
    <script>

        function ValidateHeading() {
	  <%--  var txt = document.getElementById("<%=chkHasProcedure.ClientID %>")
		if (txt.checked) {
			document.getElementById("Proceduredatecontainer").style.display = '';
		} else {
			document.getElementById("Proceduredatecontainer").style.display = 'none';
		}--%>


        }
        function ControlGrid(imgName, rowIndex, rowID) {
            //alert("CONTROL GRID");
            // alert(imgName);
            // alert(rowIndex);
            //alert(rowID);
            rowIndex = rowIndex + 3;

            var myrow = "";
            if (rowIndex < 10)
                myrow = "ctl00_Main_grdCommitteesList_ctl0" + rowIndex;
            else
                myrow = "ctl00_Main_grdCommitteesList_ctl" + rowIndex;
            var row = document.getElementById(myrow);
            //  alert("IMG NAME: "+imgName+" and ROW INDEX: "+rowIndex+" ID: "+rowID);
            //alert("MYROW: "+myrow+" AND VALUE FOUND: "+row);
            if (row.style.display == "") {
                row.style.display = "none";
                document.getElementById(imgName).src = plus.src;
            }
            else {
                row.style.display = "";
                document.getElementById(imgName).src = minus.src;
            }
        }
        function Checklist(obj, list) {
            //  alert("CHECK SYSTEM: "+obj.checked);
            if (list != "") {
                var data = list.split(",");
                //alert("LIST IS: "+data.length)
                for (var i = 0; i < data.length; i++) {
                    document.getElementById(data[i]).checked = obj.checked;
                }
            }
        }
      


   



  

        function showscannerLoading() {

            if (ValidateProcedure()) {
                document.getElementById("scanLoading").style.display = "";
                return true;
            } else { return false; }
        }


        function createFormInputsFromObject(data, prefix) {
            prefix = prefix || '';
            var inputs = '';

            jQuery.each(data, function (name, value) {
                if (prefix !== '') name = prefix + '[' + name + ']';
                if (Array.isArray(value) || value instanceof Object) {
                    inputs += createFormInputsFromObject(value, name);
                }
                else {
                    inputs += jQuery('<input>').attr({ type: 'hidden', name: name, value: value }).prop('outerHTML');
                }
            });

            return inputs;
        }
        function showPdf(url, postData) {
            $.colorbox({
                iframe: true,
                href: 'about:blank',
                width: '90%',
                height: '90%',
                onComplete: function () {

                    var iframe = $('#cboxLoadedContent iframe');

                    var form = $('<form>').attr({ action: url, method: 'POST', target: iframe.attr("name") });
                    if (!$.isEmptyObject(postData)) {
                        $(createFormInputsFromObject(postData)).appendTo(form);
                    }
                    form.appendTo(iframe)
                        .submit();

                }
            });



        }


    </script>
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
     <asp:UpdatePanel runat="server" ID="Updatepanel1" ChildrenAsTriggers="true" UpdateMode="conditional">
     <ContentTemplate>
     </ContentTemplate>
 </asp:UpdatePanel>
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
                
                      
                            <div class="form-group">
                            
                                                           
                                 <label class="col-lg-1 control-label">النوع:</label>
                        
 <div class="col-lg-5">
  <asp:RadioButtonList ID="RadioButtonTypesList" runat="server" 
      RepeatDirection="Horizontal" RepeatLayout="Flow" 
      CssClass="radio-space smart-view-display" AutoPostBack="True" 
      OnSelectedIndexChanged="RadioButtonTypesList_SelectedIndexChanged">
      <asp:ListItem Text="عرض الكل" Value="0" Selected="True"></asp:ListItem>
      <asp:ListItem Text="اللجان والمجالس العليا" Value="1" ></asp:ListItem>
      <asp:ListItem Text="مجالس إدارات الهيئات والمؤسسات العامة" Value="2"></asp:ListItem>
  </asp:RadioButtonList>

</div>
                                 <label class="col-lg-1 control-label">حالة التشكيل:</label>
<div class="col-lg-5">
  <asp:RadioButtonList ID="RadioButtonFinishedList" runat="server" 
      RepeatDirection="Horizontal" RepeatLayout="Flow" 
      CssClass="radio-space smart-view-display" AutoPostBack="True" 
      OnSelectedIndexChanged="RadioButtonFinishedList_SelectedIndexChanged">
      <asp:ListItem Text="عرض الكل" Value="0" Selected="True"></asp:ListItem>
      <asp:ListItem Text="الحالية" Value="1" ></asp:ListItem>
      <asp:ListItem Text="المنتهية" Value="2"></asp:ListItem>
  </asp:RadioButtonList>
</div>

                                    </div>
                           
                    </fieldset>
                 
                   
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
                                                نتيجة البحث  <span style="color: #000; font-size: 14px;">(
													<asp:Label ID="lblSearchResultCount" runat="server"></asp:Label>)</span>
                                               
                                            </legend>

                                            <div class="datatable-scroll">
                                                <asp:DataGrid ID="grdCommitteesList" runat="server"
                                                    DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="40" class="table datatable-basic dataTable no-footer"
                                                    Width="100%" OnItemDataBound="grdCommitteesList_ItemDataBound" OnItemCommand="grdCommitteesList_ItemCommand">
                                                    <SelectedItemStyle ForeColor="White" />
                                                    <ItemStyle CssClass="grdItem" />
                                                    <AlternatingItemStyle CssClass="grdItem" />
                                                    <PagerStyle Visible="false" />
                                                    <HeaderStyle CssClass="grdHead" BackColor="Black" ForeColor="White" Font-Bold="False" />
                                                    <Columns>
                                                        <asp:ButtonColumn HeaderText="Del." Text="<img border=0 src='images/delete.gif' alt='Delete'>" CommandName="Delete" Visible="false">
                                                            <HeaderStyle></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="center" />
                                                        </asp:ButtonColumn>
                                                        <asp:TemplateColumn HeaderText="">
                                                            <ItemStyle HorizontalAlign="Right" BackColor="#EEF0FA" />
                                                            <ItemTemplate>

                                                                <div class="panel panel-flat border-top-info border-bottom-info" style="border: 2px solid transparent; border-top-color: #00BCD4 !important; border-bottom-color: #00BCD4 !important;" id="divRelated" runat="server">
                                                                    <div class="panel-heading">

                                                                        <div style="float: left">
                                                                            <a target="_blank" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath+gets(Eval("code"))+"/" +"&vfileList=["+gets(Eval("DocFilepath"))+";]" %>' id="file1" runat="server" class="btn bg-teal btn-xs">
                                                                                <i class="icon-attachment"></i>&nbsp
																							  ملف الوثيقة
                                                                            </a>


                                                                        </div>
                                                                        <h6 class="panel-title">الأداة القانونية للإنشاء </h6>
                                                                    </div>

                                                                    <div class="panel-body">


                                                                        <asp:DataGrid ID="grdDocProcedures" runat="server"
                                                                            class="table table-hover table-striped table-bordered table-advanced tablesorter"
                                                                            AutoGenerateColumns="False"
                                                                            BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                                                                            CellPadding="3" Width="100%" OnItemDataBound="grdUnits_ItemDataBound">
                                                                            <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                            <ItemStyle CssClass="grdItem" />
                                                                            <AlternatingItemStyle CssClass="grdItem" />
                                                                            <HeaderStyle CssClass="grdHead" BackColor="Gray" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                            <FooterStyle CssClass="grdFoot" />
                                                                            <PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
                                                                                PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="DocRefID" Visible="False"></asp:BoundColumn>

                                                                                <asp:TemplateColumn HeaderText="نوع الأداة القانونية">
                                                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>
                                                                                        <%#Eval("Committees_ProceduresTypes.NameAr") %>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="ProcedureNotes" HeaderText="الموضوع  "></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ProcedureDate" HeaderText="تاريخ الوثيقة   " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ProcedureletterNum" HeaderText="رقم الوثيقة "></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CreationDate" HeaderText="تاريخ التسجيل    " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                              


                                                                                <asp:TemplateColumn HeaderText="مرفقات">
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>
                                                                                        <a class="label border-left-primary label-striped iframe" target="_blank" href="<%# ScannerRepositoryViewer + "?targetpath="  + _TargetUploadPath +gets(Eval("DocRefID"))+   "/procedure/" +gets(Eval("code"))+"/" + "&vfileList=["  + gets(Eval("Procedureattachments")) +";]"%>" style="<%#showattachment(gets(Eval("Procedureattachments")))%>">

                                                                                            <i class="icon-attachment"></i>&nbsp;
																						  ملف الوثيقة
                                                                                        </a>

                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>






                                                                            </Columns>
                                                                        </asp:DataGrid>

                                                                    </div>
                                                                </div>

                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="">
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <ItemTemplate>
                                                                <img style="cursor: pointer;" src="/layout/images/plus.gif" alt="" border="0" runat="server" id="imgControl" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>


                                                        <asp:BoundColumn Visible="false" HeaderText="Code" DataField="Code"></asp:BoundColumn>
                                                        <asp:BoundColumn Visible="false" HeaderText="Code" DataField="DocFilepath"></asp:BoundColumn>


                                                        <%--     <asp:TemplateColumn HeaderText=" نوع الوثيقة   ">
															<ItemStyle HorizontalAlign="Center" Width="100px" />
															<HeaderStyle Wrap="False" HorizontalAlign="Center" />
															<ItemTemplate>
																<%#Eval("Law_DocTypeNameAr") %>
																<br />

																<%# GetDocCats(ZeroIntergerIFNull(gets(Eval("DocCategoryID")).ToString()),"إسترداد") %>
																<%# (GetStatus(ZeroIntergerIFNull(gets(Eval("ProcedureTypeCode")).ToString()),gets(Eval("law_DocProceduresTypesNameAr")))) %>
															</ItemTemplate>
														</asp:TemplateColumn>--%>

                                                        <asp:BoundColumn HeaderText="مسلسل " DataField="committeeSerial"></asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="اسم المجلس أو اللجنة">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle Wrap="False" Width="30%" HorizontalAlign="Right" />
                                                            <ItemTemplate>
                                                                <a data-toggle="modal" data-target="#modal_<%#Eval("Code") %>">
                                                                    <%#Eval("committeeTitle") %></a>
                                                                <div id="modal_<%#Eval("Code") %>" class="modal fade">
                                                                    <div class="modal-dialog">
                                                                        <div class="modal-content">
                                                                            <div class="modal-header">
                                                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                                                <h5 class="modal-title">تفاصيل الوثيقة</h5>
                                                                            </div>

                                                                            <div class="modal-body">
                                                                                <p>
                                                                                    <%#Eval("LegalDocsDesc") %>
                                                                                </p>
                                                                            </div>

                                                                            <div class="modal-footer">
                                                                                <button type="button" class="btn btn-link" data-dismiss="modal">إغلاق</button>

                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn HeaderText="الوزير المختص " DataField="MinisterNameAr"></asp:BoundColumn>
                                                          <asp:BoundColumn  HeaderText="نوع اللجنة" DataField="CommitteesTypeDesc"></asp:BoundColumn>

                                                        <asp:BoundColumn DataField="lastJoinDate" HeaderText="تاريخ التشكيل  " DataFormatString="{0:dd/MM/yyyy}">
                                                            <HeaderStyle Wrap="false" />
                                                        </asp:BoundColumn>

                                                        <asp:TemplateColumn HeaderText="تاريخ انتهاء التشكيل ">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <%# checkExpiration(NullDateifEmpty(gets(Eval("lastJoinDate"))),NullDateifEmpty(gets(Eval("JoinExpireDate")))) %>
                                                                <b><%# NullDateifEmptyToText(gets(Eval("JoinExpireDate"))) %></b>
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
                                                        runat="server" Width="100%" PageSize="40" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>


                                                </div>
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
