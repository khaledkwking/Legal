<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LawProjectData.aspx.cs" Inherits="UI.Web.Modules.Laws.Forms.LawProjectData" %>

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
            background: #ffffff;
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
        function ValidateProcedure() {
            var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات الوثيقة اولا ");
                return false;
            }
            var txt = document.getElementById("<%=lstprocedureType.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("   فضلا ، اختر نوع العمل ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtProcedureLetterNum.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("   فضلا ، ادخل رقم الكتاب   ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtProcedureDate.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("   فضلا ، ادخل تاريخ العمل التحضيري ");
                txt.focus();
                return false;
            }
             <%--  var txt = document.getElementById("<%=txtProcedureText.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("   فضلا ، ادخل الموضوع");
                txt.focus();
                return false;
            }--%>

            return true;


        }

        function ValidateProcesdureadd() {
            var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات الوسام اولا ");
                txt.focus();
                return false;
            }
            return true;


        }

        function chkImage() {



            var txt = document.getElementById("<%=txtSubject.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل الموضوع     ");
                txt.focus();
                return false;
            }



            var txt = document.getElementById("<%=txtDocSerialNum.ClientID %>");
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل رقم مرسوم الإحالة");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtDocSerialYear.ClientID %>");
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل السنة    ");
                txt.focus();
                return false;
            }



            var txt = document.getElementById("<%=txtDocDate.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،  ادخل تاريخ الإحالة   ");
                txt.focus();
                return false;
            }




            return true;

        }




        function setactiveTab(tabindex) {
            // alert("para:" + tabindex)
            var txt = document.getElementById("<%=hdnactivetab.ClientID %>");
            txt.value = tabindex
            //   alert(txt.value);

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

    <input id="hdnScannerfilepath" runat="server" type="hidden" />
    <input id="hdnpublishedScannerfilepath" runat="server" type="hidden" />
    <input id="hdnprocedureScannerfilepath" runat="server" type="hidden" />

    <input id="hdnfilterRequestedFrom" runat="server" type="hidden" />
    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        مشروعات قوانين محالة
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

    <div class="row mbl" id="tblSearch" style="min-height: 450px;" runat="server">




        <div class="panel panel-flat">
            <div class="panel-heading">
                <div class="position-right" style="float: left; margin-left: 30px">



                    <asp:LinkButton runat="server" ID="btnNew" class="btn btn-success btn-xs" OnClick="btnNew_Click1"><i class="fa fa-plus"></i>&nbsp; إضافة مشروع قانون جديد  &nbsp;</asp:LinkButton>

                    <asp:LinkButton OnClientClick="return checkDelete();" runat="server" Visible="false" ID="btnDelete" class="btn btn-danger btn-xs" OnClick="btnDelete_Click"><i class="fa fa-times"></i>&nbsp;Delete Selected Data</asp:LinkButton>


                      
                                                                
                                                                
                                                               


                </div>
                <h5 class="panel-title">نطاق البحث</h5>
                <div class="panel-body">
                    <div class="form-horizontal">

                        <asp:Label runat="server" ID="lblerror"></asp:Label>

                        <fieldset class="content-group">
                            <legend class="text-semibold">
                                <i class="icon-file-text2 position-left"></i>
                                ادخل شروط البحث
											<%--<a class="control-arrow" data-toggle="collapse" data-target="#demo1">
                                                <i class="icon-circle-down2"></i>
                                            </a>--%>
                            </legend>




                            <div class="col-md-4">

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        رقم مرسوم الإحالة:

                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterSerialNum" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <label class="col-lg-3 control-label">
                                        السنة  :

                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterSerialYear" runat="server" class="form-control"></asp:TextBox>
                                    </div>


                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">تاريخ الإحالة   :</label>
                                    <div class="col-lg-9">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
                                            <asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                        </div>



                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        جزء من الموضوع :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:TextBox ID="txtFilterSubject" runat="server" class="form-control"></asp:TextBox>

                                        <%--  <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1"
                                            runat="server" TargetControlID="txtFilterSubject"
                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />--%>
                                    </div>



                                </div>


                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <label class="col-md-3 control-label" for="">إسترداد  : </label>

                                    <div class="col-md-9">


                                        <asp:DropDownList ID="lstFilterIsUnderStudy" class="Select2Drop" runat="server">
                                            <asp:ListItem Value="0" Text="الكل"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="نعم"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="لا"></asp:ListItem>
                                        </asp:DropDownList>


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
                                    <label class="col-lg-3 control-label">
                                        العمل التحضيري :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:DropDownList ID="lstfilterProceduretype" runat="server" class="Select2Drop"></asp:DropDownList>

                                    </div>

                                </div>

                            </div>

                            <div class="col-md-4">
                                <div class="form-group" style="display:none">
                                    <label class="col-md-3 control-label" for="">تم الإحالة  : </label>

                                    <div class="col-md-9">


                                        <asp:DropDownList ID="lstFilterTransfere" class="Select2Drop" runat="server">
                                            <asp:ListItem Value="0" Text="الكل"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="نعم"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="لا" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>


                                    </div>
                                </div>
                            </div>

                        </fieldset>
                    </div>
                    <div class="text-right">
                        <asp:LinkButton ID="lnkSearch" class="btn btn-primary" runat="server" OnClick="lnkSearch_Click">&nbsp;&nbsp; بحث&nbsp;&nbsp; <i class="icon-search4 position-right"></i></asp:LinkButton>
                    </div>

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

                                                    <asp:LinkButton ID="lnkSearchback" CssClass="control-arrow" runat="server" OnClick="lnkSearchback_Click"> رجوع  <i class=" icon-backward2"></i></asp:LinkButton>

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
                                                            <asp:TemplateColumn HeaderText="" Visible="false">
                                                                <ItemStyle HorizontalAlign="Right" BackColor="#EEF0FA" />
                                                                <ItemTemplate>

                                                                    <div class="panel panel-flat border-top-info border-bottom-info" style="border: 2px solid transparent; border-top-color: #00BCD4 !important; border-bottom-color: #00BCD4 !important;" id="divRelated" runat="server">
                                                                        <div class="panel-heading">

                                                                            <div style="float: left">
                                                                                <a target="_blank" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath+gets(Eval("code"))+"/" +"&vfileList=["+gets(Eval("DocFilepath"))+";]" %>' id="file1" runat="server" class="btn bg-teal btn-xs">
                                                                                    <i class="icon-attachment"></i>&nbsp
                                                                                                  ملف الوثيقة
                                                                                </a>

                                                                                <a target="_blank" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath+gets(Eval("code"))+"/" +"&vfileList=["+gets(Eval("DocFilepath_published"))+";]" %>' id="file2" runat="server" class="btn btn-success btn-xs">
                                                                                    <i class="icon-attachment"></i>&nbsp;
                                                                                             ملف وثيقة الجريدة الرسمية
                                                                                </a>
                                                                            </div>
                                                                            <h6 class="panel-title">الأعمال التحضيرية       </h6>
                                                                        </div>

                                                                        <div class="panel-body">


                                                                            <asp:DataGrid ID="grdDocProcedures" runat="server"
                                                                                class="table table-hover table-striped table-bordered table-advanced tablesorter"
                                                                                AutoGenerateColumns="False"
                                                                                BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                                                                                CellPadding="3" Width="100%">
                                                                                <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                                <ItemStyle CssClass="grdItem" />
                                                                                <AlternatingItemStyle CssClass="grdItem" />
                                                                                <HeaderStyle CssClass="grdHead" BackColor="Gray" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                                <FooterStyle CssClass="grdFoot" />
                                                                                <PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
                                                                                    PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
                                                                                <Columns>
                                                                                    <asp:BoundColumn DataField="DocRefID" Visible="False"></asp:BoundColumn>

                                                                                    <asp:TemplateColumn HeaderText="العمل التحضيري">
                                                                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemTemplate>
                                                                                            <%#Eval("law_DocProceduresTypes.NameAr") %>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>

                                                                                    <asp:BoundColumn DataField="CreationDate" HeaderText="تاريخ التسجيل    " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="ProcedureDate" HeaderText="التاريخ   " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="ProcedureletterNum" HeaderText="رقم الكتاب"></asp:BoundColumn>
                                                                                    <%-- <asp:BoundColumn DataField="ProcedureSubject" HeaderText="الموضوع  "></asp:BoundColumn>--%>

                                                                                    <%-- <asp:TemplateColumn HeaderText="محال لمجلس الامة   ">
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                        <HeaderStyle Wrap="False" HorizontalAlign="left" />

                                                                                        <ItemTemplate>
                                                                                            <%#ShowYesNo(getBool(Eval("SendToOma"))) %>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>--%>
                                                                                    <asp:BoundColumn DataField="ProcedureNotes" HeaderText="ملاحظات  "></asp:BoundColumn>


                                                                                    <asp:TemplateColumn HeaderText="مرفقات">
                                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemTemplate>
                                                                                            <a class="label border-left-primary label-striped iframe" target="_blank" href="<%# ScannerRepositoryViewer + "?targetpath="  + _TargetUploadPath +gets(Eval("DocRefID"))+   "/procedure/" +gets(Eval("code"))+"/" + "&vfileList=["  + gets(Eval("Procedureattachments")) +";]"%>" style="<%#showattachment(gets(Eval("Procedureattachments")))%>">

                                                                                                <i class="icon-attachment"></i>&nbsp;
                                                                                              ملف الكتاب
                                                                                            </a>

                                                                                            <%--   <a href="AnwserAttachments.aspx?DocID=0&LawDocID=<%#Eval("LawDocID")%>&FileID=0" class="btn btn-default btn-xs iframe">
                                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                                           مرفقات
                                                                                        </a>--%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>






                                                                                </Columns>
                                                                            </asp:DataGrid>

                                                                        </div>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="" Visible="false">
                                                                <ItemStyle HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    <img style="cursor: pointer;" src="/layout/images/plus.gif" alt="" border="0" runat="server" id="imgControl" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="Code"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="DocFilepath"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="DocFilepath_published"></asp:BoundColumn>

                                                            <%--  <asp:BoundColumn DataField="Law_DocTypeNameAr" HeaderText="  نوع الوثيقة  ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>--%>



                                                            <asp:BoundColumn HeaderText="رقم مرسوم الإحالة " DataField="DocSerial">
                                                                <ItemStyle Width="8%" />
                                                            </asp:BoundColumn>


                                                            <asp:BoundColumn DataField="DocDate" HeaderText="تاريخ الإحالة  " DataFormatString="{0:dd/MM/yyyy}">
                                                                <ItemStyle Width="5%" />
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="   إسترداد   ">
                                                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />

                                                                <ItemTemplate>
                                                                    <%#ShowYesNo(getBool(Eval("UnderStudy"))) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:TemplateColumn HeaderText="الموضوع">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Right" />

                                                                <ItemTemplate>
                                                                    <a data-toggle="modal" data-target="#modal_<%#Eval("Code") %>">
                                                                        <%#Eval("DocSubject") %></a>
                                                                    <div id="modal_<%#Eval("Code") %>" class="modal fade">
                                                                        <div class="modal-dialog">
                                                                            <div class="modal-content">
                                                                                <div class="modal-header">
                                                                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                                                    <h5 class="modal-title">تفاصيل الوثيقة</h5>
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

                                                            <asp:TemplateColumn HeaderText="تم الإحالة إلى تشريع">
                                                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />

                                                                <ItemTemplate>
                                                                    <%#  getBool(Eval("isTransfered")) ?  "<a href='LawDocData.aspx?LawDocID="+Eval("refId")+"'> " + ShowYesNo(getBool(Eval("isTransfered")))+"</a>" : ShowYesNo(getBool(Eval("isTransfered"))) %>
                                                                    <div style="margin-top: 5px;">
                                                                        <%#  getBool(Eval("isTransfered")) ?  "<a class='label border-left-primary label-striped '  href='LawDocData.aspx?LawDocID="+Eval("refId")+"'> عرض التشريع <b><i class='glyphicon glyphicon-link'></i></b> </a>" :""%>
                                                                    </div>


                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="التفاصيل">
                                                                <ItemStyle HorizontalAlign="right" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div style="text-align: right">
                                                                        <a target="_blank" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath+gets(Eval("code"))+"/" +"&vfileList=["+gets(Eval("DocFilepath"))+";]" %>' style="font-size: 12px; <%#showattachment(gets(Eval("DocFilepath")))%>" class="label border-left-primary label-striped iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                                              ملف الوثيقة
                                                                        </a>

                                                                    </div>
                                                                    <div style="margin-top: 10px;">
                                                                        <a href="LawProjectData.aspx?LawDocID=<%#Eval("Code")%>" class="label border-left-success label-striped" style="font-size: 12px;">
                                                                            <i class="fa fa-file"></i>&nbsp;
                                                                                           التفاصيل
                                                                        </a>
                                                                    </div>



                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:TemplateColumn>
                                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('are you sure you want to delete selected items?');" CommandName="delete" runat="server">  <i class="fa fa-trash" style="color:##333"></i>&nbsp;</asp:LinkButton>
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

    <div class="row mbl">

        <div class="panel-heading" id="tblAdd" runat="server" visible="false">
            <div class="row">
                <asp:Label runat="server" ID="lblAdderror"></asp:Label>
                <div class="panel">

                    <div class="panel-body">

                        <div class="tabbable">
                            <ul class="nav nav-tabs nav-tabs-highlight">
                                <li class="<%=activeTab(1) %>" onclick="setactiveTab(1)"><a href="#badges-tab1" data-toggle="tab"><i class="icon-menu7 position-left"></i>بيانات  مشروع قانون محال</a></li>
                                <li class="<%=activeTab(2) %>" onclick="setactiveTab(2)"><a href="#badges-tab2" data-toggle="tab">الأعمال التحضيرية <span class="badge badge-success  position-right">
                                    <asp:Label ID="lblProcedureCount" runat="server" Text="0"></asp:Label></span></a></li>
                            </ul>

                            <div class="tab-content">
                                <div class="tab-pane <%=activeTab(1) %>" id="badges-tab1">
                                    <div class="col-lg-12">
                                        <div class="portlet box portlet-blue">
                                            <div class="portlet-header">
                                                <div class="caption">
                                                    <asp:Label runat="server" Visible="false" ID="lblSubTitle">إضافة جديد</asp:Label>
                                                </div>

                                            </div>
                                            <div class="portlet-body">
                                                <div role="form" class="form-horizontal">
                                                    <input id="hdnMasterID" runat="server" type="hidden" />
                                                    <input id="hdnRelatedOrg" runat="server" type="hidden" />


                                                    <input id="hdnactivetab" runat="server" type="hidden" />
                                                    <fieldset class="content-group">

                                                        <legend class="text-semibold">
                                                            <i class="icon-file-text2 position-left"></i>
                                                            بيانات  مرسوم الإحالة


                                                        </legend>
                                                        <div class="row" style="background: #efefef; padding: 10px">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label class="col-lg-4 control-label" style="font-size: 14px;">
                                                                        رقم مرسوم الإحالة <span class="text-danger">*</span>:

                                                                    </label>
                                                                    <div class="col-lg-3">
                                                                        <asp:TextBox ID="txtDocSerialNum" class="form-control" runat="server"></asp:TextBox>
                                                                    </div>
                                                                    <label class="col-lg-2 control-label">
                                                                        السنة     :

                                                                    </label>
                                                                    <div class="col-lg-3">
                                                                        <asp:TextBox ID="txtDocSerialYear" class="form-control" runat="server"></asp:TextBox>
                                                                    </div>


                                                                </div>


                                                            </div>





                                                        </div>
                                                        <legend class="text-semibold">
                                                            <i class="icon-file-text2 position-left"></i>
                                                            بيانات  مشروع قانون محال


                                                        </legend>
                                                        <div class="row">
                                                            <div class="col-md-8">
                                                                <div class="form-group">
                                                                    <label class="col-md-2 control-label" for="">الموضوع <span class="text-danger">*</span> :</label>

                                                                    <div class="col-md-10 autoDrop">
                                                                        <asp:TextBox runat="server" ID="txtSubject" class="form-control" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                                                        <%-- <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
                                                                            runat="server" TargetControlID="txtQuestionSubject"
                                                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />--%>
                                                                    </div>


                                                                </div>
                                                            </div>


                                                        </div>

                                                        <div class="row">


                                                            <div class="col-md-4">

                                                                <div class="form-group" style="display: none">
                                                                    <label class="col-md-4 control-label" for="">نوع الوثيقة  <span class="text-danger">*</span>  : </label>

                                                                    <div class="col-md-8">
                                                                        <asp:DropDownList ID="lstDocType" runat="server" class="Select2Drop"></asp:DropDownList>
                                                                    </div>
                                                                </div>



                                                                <div class="form-group">
                                                                    <label class="col-md-4 control-label" for="">تاريخ   الإحالة   :  </label>

                                                                    <div class="col-md-8">

                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>

                                                                            <asp:TextBox ID="txtDocDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

                                                                        </div>

                                                                    </div>
                                                                </div>


                                                                <div class="form-group">
                                                                    <label class="col-md-4 control-label" for="">إسترداد  : </label>

                                                                    <div class="col-md-8">

                                                                        <asp:CheckBox ID="chkIsUnderStudy" runat="server" />


                                                                    </div>
                                                                </div>


                                                            </div>


                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-8">

                                                                <div class="form-group ">
                                                                    <label class="col-md-2 control-label" for="">ملاحظات  :</label>

                                                                    <div class="col-md-10 autoDrop">

                                                                        <asp:TextBox runat="server" TextMode="MultiLine" Rows="3" ID="txtNotes" class="form-control"></asp:TextBox>

                                                                    </div>
                                                                </div>

                                                            </div>



                                                        </div>
                                                        <div class="row" style="display: none">
                                                            <div class="col-md-12">

                                                                <div class="form-group">
                                                                    <label class="col-md-1 control-label" for="">نص الوثيقة:</label>

                                                                    <div class="col-md-11" style="padding-right: 40px;">

                                                                        <asp:TextBox runat="server" TextMode="MultiLine" Rows="12" ID="txtDetails" class="form-control"></asp:TextBox>

                                                                    </div>
                                                                </div>

                                                            </div>



                                                        </div>
                                                        <div class="row">

                                                            <div class="col-md-8">

                                                                <div class="form-group" style="background-color: lavender; padding: 10px; border-radius: 3px;">
                                                                    <label class="col-md-2 control-label">ملف الوثيقة  :</label>

                                                                    <div class="col-md-7">
                                                                        <asp:Label ID="lblimage" runat="server"></asp:Label>
                                                                        <asp:FileUpload ID="txtDocImage" runat="server" class="file-styled" />
                                                                        <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <span class="help-block2">| Or | </span>
                                                                        <asp:LinkButton runat="server" ID="lnkDocScan" OnClientClick="return chkImage();" OnClick="lnkQScan_Click" class="btn btn-info btn-xs"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                                    </div>


                                                                </div>
                                                            </div>

                                                            <div class="col-md-4">
                                                            </div>
                                                        </div>

                                                </fieldset>

                                                </div>


                                                <div class="row" style="padding-top: 10px">
                                                    <div class="col-md-12">
                                                        <div class="form-actions">
                                                            <div class="col-md-offset-4 col-md-12">


                                                                <asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary" OnClick="btnSave_Click1"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>



                                                                &nbsp;
				                                               <asp:Button runat="server" ID="btnCancel" class="btn btn-default" Text=" الغاء / رجوع  " OnClick="btnCancel_Click" />
                                                                &nbsp;

                                                              
                                                                &nbsp;
                                                             <a id="anchorAttachment" visible="false" runat="server" href='javascript:void(0)' class="btn btn-success btn-xs iframe">
                                                                 <i class="icon-attachment"></i>&nbsp; عرض ملف الوثيقة</a>
            

  &nbsp;
                                                                
                    <div class="btn-group btn-group-animated" runat="server" id="divlinklaws" style="width:250px;" visible="false" >
                        <button type="button" class="btn btn-warning dropdown-toggle" data-toggle="dropdown" aria-expanded="false"><span class="glyphicon glyphicon-link"></span>&nbsp;التحويل إلى تشريع&nbsp; </button>
                        <ul class="dropdown-menu dropdown-menu-right">
                            <li>
                                 
                                <asp:LinkButton ID="lnkMoveToLawDoc" runat="server"  OnClientClick="return confirm('هل انت متأكد من تحويل مشروع القانون الي تشريع ؟  ');" OnClick="lnkMoveToLawDoc_Click"  >
                                    <i class='icon-cog5'></i>&nbsp; 
                                    
                                    تحويل المشروع إلى تشريع جديد   </asp:LinkButton>

                            </li>
                            <li>
                                 <a href="#" class="btn btn-primary btn-labeled iframe linkedpopup" id="lnkAddRelatedDoc" runat="server"><b><i class="glyphicon glyphicon-link"></i></b>ربط المشروع بتشريع مسجل   </a>

                            </li>

                        </ul>
                    </div>
                      &nbsp;
                <a href="#" class="btn btn-primary btn-labeled" id="lnkViewRelatedDoc" runat="server" visible="false"><b><i class="glyphicon glyphicon-link"></i></b>عرض التشريع المرتبط   </a>



                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>




                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="tab-pane <%=activeTab(2) %>" id="badges-tab2">
                                    <%-- <asp:UpdatePanel runat="server" ID="Updatepanel7" ChildrenAsTriggers="true" UpdateMode="conditional">
                                        <ContentTemplate>--%>

                                    <div class="form-horizontal" id="DivAddProcedure" runat="server" visible="false">

                                        <div class="row col-md-10">

                                            <div class="col-md-6">

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">العمل التحضيري<span class="text-danger">*</span> : </label>

                                                    <div class="col-md-9">
                                                        <asp:DropDownList ID="lstprocedureType" runat="server" class="Select2Drop"></asp:DropDownList>

                                                    </div>

                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">رقم الكتاب <span class="text-danger">*</span>   : </label>

                                                    <div class="col-md-9">
                                                        <asp:TextBox ID="txtProcedureLetterNum" class="form-control" runat="server"></asp:TextBox>

                                                    </div>

                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">التاريخ  <span class="text-danger">*</span> :</label>

                                                    <div class="col-md-9">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                            <asp:TextBox ID="txtProcedureDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                </div>

                                                <div class="form-group" style="display: none">
                                                    <label class="col-md-3 control-label">الموضوع  <span class="text-danger">*</span> : </label>

                                                    <div class="col-md-9">
                                                        <asp:TextBox ID="txtProcedureText" TextMode="MultiLine" class="form-control" runat="server"></asp:TextBox>

                                                    </div>

                                                </div>

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">ملاحظات : </label>

                                                    <div class="col-md-9">

                                                        <asp:TextBox ID="txtProcedureNotes" TextMode="MultiLine" class="form-control" runat="server"></asp:TextBox>

                                                    </div>
                                                </div>



                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group" style="display: none">
                                                    <label class="col-md-3 control-label" for="">محال لمجلس الامة  : </label>

                                                    <div class="col-md-9">

                                                        <asp:CheckBox ID="chkOmaSend" runat="server" />

                                                    </div>
                                                </div>

                                                <div class="form-group" style="display: none">
                                                    <label class="col-md-3 control-label">تاريخ الاحالة   : </label>

                                                    <div class="col-md-9">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                            <asp:TextBox ID="txtOmaDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                </div>

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">ملف الكتاب :</label>

                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                                        <asp:FileUpload ID="txtProcedureimage" runat="server" class="file-styled" />
                                                        <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <span class="help-block2">| Or | </span>
                                                        <asp:LinkButton runat="server" ID="btnprocedureScan" OnClientClick="return showscannerLoading();" OnClick="btnprocedureScan_Click" class="btn btn-info btn-xs"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>


                                        <div class="col-md-12">
                                            <div class="form-actions">
                                                <div class="col-md-offset-9 col-md-12">
                                                    <asp:LinkButton ID="lnkSaveProcedure" runat="server" class="btn btn-primary" OnClientClick="return ValidateProcedure()" OnClick="lnkSaveProcedure_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

                                                    &nbsp;
				                               <asp:Button runat="server" ID="lnkCancelProcedure" OnClick="lnkCancelProcedure_Click" class="btn btn-default" Text=" الغاء  " />

                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row" id="divShowProcedure" runat="server">
                                        <div class="col-lg-12">
                                            <div class="portlet box">
                                                <div class="portlet-header">

                                                    <div class="actions pull-right" style="margin-bottom: 10px;">

                                                        <asp:LinkButton runat="server" ID="lnkAddProcedure" OnClick="lnkAddProcedure_Click" class="btn btn-info btn-xs"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

                                                        <asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="lnkDeleteProcedure" OnClick="lnkDeleteProcedure_Click" class="btn btn-danger btn-xs"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

                                                    </div>
                                                </div>
                                                <div class="portlet-body">
                                                    <div class="datatable-scroll" style="padding-top: 20px;">

                                                        <asp:DataGrid runat="server" ID="grdProcedureList" AutoGenerateColumns="False"
                                                            AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemCommand="grdProcedureList_ItemCommand" OnEditCommand="grdProcedureList_EditCommand">
                                                            <PagerStyle Visible="False" />
                                                            <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                                            <Columns>
                                                                <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
                                                                <asp:TemplateColumn HeaderText="العمل التحضيري    ">
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <%#Eval("law_DocProceduresTypes.NameAr") %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <asp:BoundColumn DataField="CreationDate" HeaderText="تاريخ التسجيل    " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ProcedureDate" HeaderText="التاريخ   " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ProcedureletterNum" HeaderText="رقم الكتاب"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ProcedureSubject" Visible="false" HeaderText="الموضوع  "></asp:BoundColumn>


                                                                <asp:BoundColumn DataField="ProcedureNotes" HeaderText="ملاحظات  "></asp:BoundColumn>

                                                                <asp:TemplateColumn HeaderText="ملف الكتاب  ">
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <a target="_blank" href="<%# ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath+gets(Eval("DocRefID"))+"/procedure/"+gets(Eval("code"))+"/" + "&vfileList=["  + gets(Eval("Procedureattachments")) +";]"%>" class="label border-left-primary label-striped iframe" style="<%#showattachment(gets(Eval("Procedureattachments")))%>">
                                                                            <i class="icon-attachment"></i>&nbsp;عرض المرفق
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
                                                        </div>
                                                        <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">

                                                            <cc1:Pager CurrentIndex="1" OnCommand="pager_Command5" ShowFirstLast="False" ID="pager5"
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
        </div>
    </div>
    <!--END CONTENT-->
    <!--BEGIN FOOTER-->
    <div id="scanLoading" class="scanLoading" style="display: none">
        <img src="/Layout/images/scan-document.gif" />
    </div>
</asp:Content>
