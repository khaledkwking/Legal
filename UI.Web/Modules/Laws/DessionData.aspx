<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="DessionData.aspx.cs" Inherits="UI.Web.Modules.Laws.DessionData" %>

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
            background-color: #FFFFFF;
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
              background:#ffffff;
        }

            .grdItem td {
                border-left: solid 1px #000000;
                border-right: solid 1px #000000;
                border-top: solid 0px #000000;
                border-bottom: solid 0px #000000;
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
                myrow = "ctl00_Main_grdLawDocsList_ctl0" + rowIndex;
            else
                myrow = "ctl00_Main_grdLawDocsList_ctl" + rowIndex;
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


<%--var txt = document.getElementById("<%=txtSerial.ClientID %>");
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل مسلسل القرار");
                txt.focus();
                return false;
            }--%>
   

         

           <%-- var txt = document.getElementById("<%=lstCategory.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر   تصنيف القرار    ");
                txt.focus();
                return false;
            }--%>

       


            return true;

        }

        function LinkAddClick() {
            // alert("in");

            return InsertItem();
        }

        function relatedOrgClick() {
            // alert("in");

            return ValidatedRelatedOrgs();
        }

        function ValidatedRelatedOrgs() {

            var lstRelatedOrgs = getObjById("lstRelatedOrgs").value;
            if (lstRelatedOrgs == "" || lstRelatedOrgs == "0") {
                new $.Zebra_Dialog("فضلا ،اختر الجهة المعنية ");
                return false;
            }
        }

        function InsertItem() {
            var lstRequestedFromPareon = getObjById("lstRequestedFromPareon").value;

            if (lstRequestedFromPareon == "" || lstRequestedFromPareon == "0") {
                new $.Zebra_Dialog("فضلا ،اختر موجة من    ");
                return false;
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
            //  var iframe = $('#cboxLoadedContent iframe');
            // var form = $('<form>').attr({ name:'pdfpost', action: url, method: 'POST',target:"_blank"  });
            // $(createFormInputsFromObject(postData)).appendTo(form);
            // $('body').append(form);
            //form.submit();


        }






    </script>
    <input id="hdnIncomingScannedFilePath" runat="server" type="hidden" />
    <input id="hdnfilterRequestedFrom" runat="server" type="hidden" />
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


                                                    <asp:DataGrid ID="grdLawDocsList" runat="server"
                                                        DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="40" class="table datatable-basic dataTable no-footer"
                                                        Width="100%" OnItemDataBound="grdLawDocsList_ItemDataBound" OnItemCommand="grdLawDocsList_ItemCommand">
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
                                                            <asp:TemplateColumn HeaderText="" Visible="false" >
                                                                <ItemStyle HorizontalAlign="Right" BackColor="#EEF0FA" />
                                                                <ItemTemplate>
                                                                    <div class="panel panel-flat border-top-info border-bottom-info" style="border: 2px solid transparent; border-top-color: #00BCD4 !important; border-bottom-color: #00BCD4 !important;" id="divRelated" runat="server">
                                                                        <div class="panel-heading">

                                                                            <div style="float: left">
                                                                                <a target="_blank" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath + gets(Eval("code"))+"/" +"&vfileList=["+gets(Eval("DocFilepath"))+";]" %>' id="file1" runat="server" class="btn bg-teal btn-xs iframe " style="color:#fff">
                                                                                    <i class="icon-attachment"></i>&nbsp
                                                                                                  ملف القرار
                                                                                </a>

                                                                               
                                                                            </div>
                                                                            <h6 class="panel-title">الأعمال التحضيرية       </h6>
                                                                        </div>

                                                                        <div class="panel-body">
                                                                            
                                                                        </div>
                                                                    </div>

                                                                    
                                                                    <div class="panel panel-flat border-top-info border-bottom-success" style="border: 2px solid transparent; border-top-color: #54a207 !important; border-bottom-color: #54a207 !important;" id="div1" runat="server">
                                                                        <div class="panel-heading">
                                                                            <h6 class="panel-title">الوارد         </h6>
                                                                        </div>
                                                                        <div class="panel-body">
                                                                            <asp:DataGrid runat="server" ID="grdIncoming" AutoGenerateColumns="False"
                                                                                AllowPaging="false" PageSize="20" class="table datatable-basic dataTable no-footer">
                                                                                <PagerStyle Visible="False" />
                                                                                <SelectedItemStyle Font-Bold="True" ForeColor="White" />
                                                                                <ItemStyle CssClass="grdItem" />
                                                                                <AlternatingItemStyle CssClass="grdItem" />
                                                                                <HeaderStyle CssClass="grdHead" BackColor="#66BB6A" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                                <Columns>
                                                                                    <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
                                                                                    <%-- <asp:TemplateColumn HeaderText="النوع">
                                                                                        <ItemStyle Width="3%" />
                                                                                        <ItemTemplate>
                                                                                            <%#fillDocType(gets(Eval("Doc_Type"))) %>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>--%>
                                                                                    <asp:BoundColumn DataField="Doc_Serial" HeaderText="رقم الكتاب"></asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Doc_From" HeaderText="وارد من "></asp:BoundColumn>

                                                                                    <asp:BoundColumn DataField="SentDate" HeaderText="التاريخ  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                    <%--<asp:BoundColumn DataField="Doc_Subject" HeaderText="الموضوع "></asp:BoundColumn>--%>
                                                                                    <asp:BoundColumn DataField="Doc_Notes" HeaderText="ملاحظات "></asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="NextFollowReminderDate" HeaderText="تاريخ التنبيه  " DataFormatString="{0:dd/MM/yyyy}" Visible="false"></asp:BoundColumn>
                                                                                    <asp:TemplateColumn HeaderText="عرض المرفق">
                                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemTemplate>
                                                                                            <a class="label border-left-primary label-striped iframe" target="_blank" href="<%#  ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath +gets(Eval("RefDocID"))+"/incoming/"+ gets(Eval("Code"))+"/"+ "&vfileList=[" + gets(Eval("Filepath")) +";]"%>" style="<%#showattachment(gets(Eval("Filepath")))%>">
                                                                                                <i class="icon-attachment"></i>&nbsp; عرض المرفق
                                                                                            </a>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                </Columns>
                                                                            </asp:DataGrid>
                                                                        </div>
                                                                    </div>

                                                                    <div class="panel panel-flat border-top-info border-bottom-warning" style="border: 2px solid transparent; border-top-color: #00BCD4 !important; border-bottom-color: #00BCD4 !important;" id="div2" runat="server">
                                                                        <div class="panel-heading">
                                                                            <h6 class="panel-title">الصادر         </h6>
                                                                        </div>
                                                                        <div class="panel-body">
                                                                            <asp:DataGrid runat="server" ID="grdOutgoing" AutoGenerateColumns="False"
                                                                                AllowPaging="false" PageSize="20" class="table datatable-basic dataTable no-footer">
                                                                                <PagerStyle Visible="False" />
                                                                                <SelectedItemStyle Font-Bold="True" ForeColor="White" />
                                                                                <ItemStyle CssClass="grdItem" />
                                                                                <AlternatingItemStyle CssClass="grdItem" />
                                                                                <HeaderStyle CssClass="grdHead" BackColor="#00BCD4" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                                <Columns>
                                                                                    <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>

                                                                                    <%--    <asp:TemplateColumn HeaderText="النوع">
                                                                                        <ItemStyle Width="3%" />
                                                                                        <ItemTemplate>
                                                                                            <%#fillDocType(gets(Eval("Doc_Type"))) %>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>--%>
                                                                                    <asp:BoundColumn DataField="Doc_Serial" HeaderText="رقم الكتاب"></asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Doc_from" HeaderText="صادر من   "></asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Doc_to" HeaderText="صادر إلى   "></asp:BoundColumn>

                                                                                    <asp:BoundColumn DataField="SentDate" HeaderText="التاريخ  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                    <%--<asp:BoundColumn DataField="Doc_Subject" HeaderText="الموضوع "></asp:BoundColumn>--%>
                                                                                    <asp:BoundColumn DataField="Doc_Notes" HeaderText="ملاحظات "></asp:BoundColumn>


                                                                                    <asp:TemplateColumn HeaderText="تاريخ التنبيه ">

                                                                                        <ItemTemplate>
                                                                                            <%# NullDateifEmptyToText(gets(Eval("NextFollowReminderDate"))) %>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>

                                                                                    <asp:TemplateColumn HeaderText="عرض المرفق">
                                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemTemplate>
                                                                                            <a class="label border-left-primary label-striped iframe" target="_blank" href="<%#  ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath +gets(Eval("RefDocID"))+"/outgoing/"+ gets(Eval("Code"))+"/"+ "&vfileList=[" + gets(Eval("Filepath")) +";]"%>" style="<%#showattachment(gets(Eval("Filepath")))%>">
                                                                                                <i class="icon-attachment"></i>&nbsp; عرض المرفق
                                                                                            </a>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>


                                                                                </Columns>
                                                                            </asp:DataGrid>
                                                                        </div>
                                                                    </div>


                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="" >
                                                                <ItemStyle HorizontalAlign="center" width="1%"  />
                                                                <ItemTemplate>
                                                                                         <span>-</span>


                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="Code"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="DocFilepath"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="DocFilepath_published"></asp:BoundColumn>

                                                         

                                                                   <asp:TemplateColumn HeaderText="رقم القرار ">
                                                                <ItemStyle HorizontalAlign="Center" width="3%"  />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <%#Eval("DocSerial") %>
                                                                    <br />
                                                                       <%# getPrivate(getBool(gets(Eval("isPrivate")))) %>

                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>




                                                            

                                                      <%--      <asp:BoundColumn DataField="Law_DocTypeNameAr" HeaderText="  نوع القرار  ">
                                                                <ItemStyle Width="7%" />
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>--%>

                                                            
                                                            <asp:TemplateColumn HeaderText=" نوع القرار     ">
                                                                <ItemStyle HorizontalAlign="Center" width="7%"  />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <%#Eval("Law_DocTypeNameAr") %>
                                                                    <br />
                                                                       <%# (GetStatus(ZeroIntergerIFNull(gets(Eval("ProcedureTypeCode")).ToString()),gets(Eval("law_DocProceduresTypesNameAr")))) %>

                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:BoundColumn DataField="DocDate" HeaderText="تاريخ إصدار القرار" DataFormatString="{0:dd/MM/yyyy}">
                                                                <ItemStyle Width="3%" />
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText=" تحت الدراسة" Visible="false">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />

                                                                <ItemTemplate>
                                                                    <%#ShowYesNo(getBool(Eval("UnderStudy"))) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:TemplateColumn HeaderText="الموضوع">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <HeaderStyle Wrap="False" Width="30%" HorizontalAlign="Right" />

                                                                <ItemTemplate>
                                                                   
                                                                        <span style="color:#1E88E5"><%#Eval("DocSubject") %></span>
                                                                    <div id="modal_<%#Eval("Code") %>" class="modal fade">
                                                                        <div class="modal-dialog">
                                                                            <div class="modal-content">
                                                                                <div class="modal-header">
                                                                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                                                    <h5 class="modal-title">تفاصيل القرار</h5>
                                                                                </div>

                                                                                <div class="modal-body">
                                                                                    <p>
                                                                                        <%#Eval("DocDetails") %>
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

                                                            <%--  <asp:BoundColumn DataField="DocSubject" HeaderText="الموضوع">
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle Width="30%" />
                                                            </asp:BoundColumn>--%>

                                                            <asp:BoundColumn DataField="Law_DocCategoryNameAr" HeaderText="  التصنيف  " Visible="false">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>


                                                            <asp:TemplateColumn HeaderText=" نشر بالجريدة الرسمية " Visible="false">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />

                                                                <ItemTemplate>
                                                                    <%#ShowYesNo(getBool(Eval("isPublished"))) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="kng_Dession" HeaderText="  قرار مجلس الامة  " Visible="false">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>




                                                            <asp:TemplateColumn HeaderText="التفاصيل">
                                                                <ItemStyle HorizontalAlign="right" Width="2%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                 
                                                                    <div style="margin-top: 10px;">

                                                                    <a   href='<%#ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(Eval("code"))+"/"+ "&vfileList=[" + gets(Eval("DocFilepath"))+";]"%>' style='font-size: 12px; <%#showattachment(gets(Eval("DocFilepath")))%>' class="label border-left-warning label-sucess  label-striped iframe">
                                                                 <i class="icon-attachment"></i>&nbsp;  ملف القرار</a>
</div>
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

    
    <!--END CONTENT-->
    <!--BEGIN FOOTER-->
    <div id="scanLoading" class="scanLoading" style="display: none">
        <img src="/Layout/images/scan-document.gif" />
    </div>
</asp:Content>
