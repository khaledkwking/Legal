<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Empty.Master" AutoEventWireup="true" CodeBehind="AgreementsDataLink.aspx.cs" Inherits="UI.Web.Agreements.Forms.AgreementsDataLink" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
    <script>

        function ControlGrid(imgName, rowIndex, rowID) {
            //alert("CONTROL GRID");
            //alert(imgName);
            //alert(rowIndex);
            //alert(rowID);
            rowIndex = rowIndex + 3;

            var myrow = "";
            if (rowIndex < 10)
                myrow = "ctl00_Main_grdInboundItems_ctl0" + rowIndex;
            else
                myrow = "ctl00_Main_grdInboundItems_ctl" + rowIndex;
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


        function setactiveTab(tabindex) {
            //alert("para:" + tabindex)
            var txt = document.getElementById("<%=hdnactivetab.ClientID %>");
            txt.value = tabindex;

        }

        function chkProcedure() {


            var txt = document.getElementById("<%=hdnMasterID.ClientID %>");
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات الاتفاقيه ");
                txt.focus();
                return false;
            }



            var txt = document.getElementById("<%=lstProcedureType.ClientID %>");
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ،اختر نوع الاجراء ");
                txt.focus();
                return false;
            }




            return true;

        }

        function ValidateProcesdureadd() {
            var txt = document.getElementById("<%=hdnMasterID.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات الاتفاقية اولا ");
                txt.focus();
                return false;
            }
            return true


        }

        function chkImage() {

            var txt = document.getElementById("<%=txtSubject.ClientID %>");
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ،ادخل عنوان الاتفاقية");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtSerialNUm.ClientID %>");
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  المسلسل");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtserialYear.ClientID %>");
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  المسلسل");
                txt.focus();
                return false;
            }




            var txt = document.getElementById("<%=lstTypeCode.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،   اختر نوع الاتفاقيه  ");
                txt.focus();
                return false;
            }

            <%--  var txt = document.getElementById("<%=txtCreationDate.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل اختر تاريخ الاتفاقية    ");
                txt.focus();
                return false;
            }--%>

            var txt = document.getElementById("<%=lstCats.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،   اختر التصنيف  ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=lstorg.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،   اختر الجهة/الدولة  ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=lstAgrRelatedOrgs.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر جهة الإختصاص    ");
                txt.focus();
                return false;
            }


          <%--  var txt = document.getElementById("<%=lstStatus.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل اختر الحالة   ");
                txt.focus();
                return false;
            }--%>
            return true;
        }
        function ValidateProScan1() {
            ValidateProcedures();

            var txt = document.getElementById("<%=txtRef.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،   ادخل رقم المرجع     ");
                txt.focus();
                return false;
            }

            return true;
        }

        function ValidateProcedures() {


            var txt = document.getElementById("<%=lstProcedureType.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،   اختر نوع الاجراء   ");
                txt.focus();
                return false;
            }


            var txt = document.getElementById("<%=txtProcedureActionDate.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،   اختر تاريخ المرفق    ");
                txt.focus();
                return false;
            }

          <%--  var txt = document.getElementById("<%=lstAttachmentType.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل اختر نوع المرفق     ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtRef.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل اختر المرجع      ");
                txt.focus();
                return false;
            }--%>


            return true;
        }
        function ValidateProScan2() {
            ValidateProcedures2();

            var txt = document.getElementById("<%=txtRef2.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل رقم المرجع    ");
                txt.focus();
                return false;
            }
            return true;

        }
        function ValidateProcedures2() {


            var txt = document.getElementById("<%=lstProcedureType2.ClientID %>");
             if (txt.value == "" || txt.value == "0") {
                 new $.Zebra_Dialog("فضلا ،   اختر نوع الاجراء   ");
                 txt.focus();
                 return false;
             }


             var txt = document.getElementById("<%=txtProcedureActionDate2.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،   اختر تاريخ المرفق    ");
                txt.focus();
                return false;
            }


            return true;
        }
    </script>

    <input id="hdnCMGSScannerfilepath" runat="server" type="hidden" />
    <input id="hdnCMGS2Scannerfilepath" runat="server" type="hidden" />

    <input id="hdnpublishScannerfilepath" runat="server" type="hidden" />
    <input id="hdnpublish2Scannerfilepath" runat="server" type="hidden" />



    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        الاتفاقيات
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




                            <div class="col-md-5">

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        مسلسل :
                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterNum" runat="server" class="form-control" placeholder="رقم"></asp:TextBox>


                                    </div>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterYear" runat="server" class="form-control" placeholder="سنة"></asp:TextBox>


                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        عنوان الاتفاقية :
                                    </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="txtPartofName" runat="server" class="form-control"></asp:TextBox>


                                    </div>
                                </div>




                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الجهة/الدولة:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterOrg" class="Select2Drop form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">جهة الإختصاص :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterAgrRelatedOrgs" class="Select2Drop form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">


                                <div class="form-group">
                                    <label class="col-lg-3 control-label">التصنيف:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterCats" class="Select2Drop form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">النوع:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterType" class="Select2Drop form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>


                                <%--<div class="form-group">
                                    <label class="col-lg-3 control-label">الحالة:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterStatusCode" class="Select2Drop form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>--%>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الاجراء:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterProcedures" AutoPostBack="true" OnSelectedIndexChanged="lstFilterProcedures_SelectedIndexChanged" class="Select2Drop form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">التاريخ من :</label>
                                    <div class="col-lg-9">




                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control pickadate-selectors picker__input picker__input--active" value="03/18/2013" type="text">--%>
                                            <asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>



                                        </div>



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
                                <div class="form-group" id="divFilterRelatedOrg" runat="server" visible="false">
                                    <label class="col-md-3 control-label" for="">محال الي جهة الإختصاص    </label>

                                    <div class="col-md-9">

                                        <asp:DropDownList ID="lstFilterRelatedOrgs" runat="server" class="Select2Drop form-control"></asp:DropDownList>
                                    </div>

                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label" for="">حالة الاتفاقية   </label>

                                    <div class="col-md-9">

                                        <asp:DropDownList ID="lstFilterStatus" runat="server" class="form-control">
                                            <asp:ListItem Text="الكل" Value="">
                                            </asp:ListItem>
                                            <asp:ListItem Text="مبدئية" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="نهائية" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                </div>

                            </div>


                        </fieldset>
                    </div>
                    <div class="text-right">


                        <asp:LinkButton ID="lnkSearch" class="btn btn-primary" runat="server" OnClick="lnkSearch_Click"> بحث <i class="icon-search4 position-right"></i></asp:LinkButton>


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
                                                        <asp:Label ID="lblcount2" runat="server"></asp:Label>)</span>

                                                    <asp:LinkButton ID="lnkSearchback" CssClass="control-arrow" runat="server" OnClick="lnkSearchback_Click"> بحث جديد  <i class=" icon-backward2"></i></asp:LinkButton>

                                                </legend>


                                                <div class="datatable-scroll">

                                                    <asp:DataGrid ID="grdInboundItems" runat="server"
                                                        DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="20" class="table datatable-basic dataTable no-footer"
                                                        Width="100%" OnItemDataBound="grdInboundItems_ItemDataBound" OnItemCommand="grdInboundItems_ItemCommand">
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
                                                                    <div class="panel panel-flat border-left-danger" style="border: 2px solid transparent; border-right-color: #F44336 !important;">
                                                                        <div class="panel-heading">
                                                                            <h6 class="panel-title">إجراءات الاتفاقية   <%# getAgreementType(getBool(Eval("isInitial").ToString())) %>   </h6>
                                                                        </div>

                                                                        <div class="panel-body">

                                                                            <asp:DataGrid ID="grdProcedures" runat="server" PageSize="10"
                                                                                class="table table-hover table-striped table-bordered table-advanced tablesorter"
                                                                                AutoGenerateColumns="False"
                                                                                BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                                                                                CellPadding="3" Width="100%" OnItemDataBound="grdUnits_ItemDataBound">
                                                                                <SelectedItemStyle Font-Bold="True" ForeColor="White" />
                                                                                <ItemStyle CssClass="grdItem" />
                                                                                <AlternatingItemStyle CssClass="grdItem" />
                                                                                <HeaderStyle CssClass="grdHead" BackColor="#66BB6A" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                                <FooterStyle CssClass="grdFoot" />
                                                                                <PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
                                                                                    PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
                                                                                <Columns>
                                                                                    <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
                                                                                    <asp:TemplateColumn HeaderText="الاجراء">

                                                                                        <HeaderStyle HorizontalAlign="right" />
                                                                                        <ItemTemplate>
                                                                                            <%#Eval("Agreement_ProcedureTypes.NameAr") %>
                                                                                            <%--   <br />
                                                                        <div class="label bg-green-400" style="<%#ViewDiv(gets(Eval("RelatedOrgName")))%>">
                                                                            <%#gets(Eval("RelatedOrgName")) %>  </div>

                                                                         <div class="label bg-green-400" style="<%#ViewDiv(gets(Eval("decisionNum")))%>" >
                                                                        <%#getPublishType(gets(Eval("publishType"))) %>
                                                                        <%# gets(Eval("decisionNum")) %></div>--%>
                                                                                            <div class="label bg-green-400" style="<%#ViewDiv(gets(Eval("publishDessionFile")))%>">
                                                                                                ملف :
                                                                         <%# (gets(Eval("publishDessionFile"))!=""? "<a target='_blank' href='"+  ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath +"&vfileList=["+gets(Eval("publishDessionFile"))  +";]'>View file</a>":"")%>
                                                                                            </div>

                                                                                            <div class="label bg-green-400" style="<%#ViewDiv(gets(Eval("CMGSDessionNum")))%>">
                                                                                                <i class="icon-attachment"></i>قرار مجلس الوزراء : رقم
                                                                       <%# (gets(Eval("CMGSDessionFile"))!=""? "<a target='_blank' href='"+  ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath +"&vfileList=["+gets(Eval("CMGSDessionFile"))  +";]'>"+gets(Eval("CMGSDessionNum"))+"</a>":gets(Eval("CMGSDessionNum")))  %>>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>

                                                                                    <asp:BoundColumn DataField="ActionDate" HeaderText="تاريخ التسجيل " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="ProcedureDate" HeaderText="تاريخ الاجراء " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Remarks" HeaderText="ملاحظات"></asp:BoundColumn>
                                                                                    <asp:TemplateColumn HeaderText="مرفقات">
                                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemTemplate>

                                                                                            <a href="AgreementAttachments.aspx?ProcedureID=<%#Eval("code") %>&AgreementCode=<%#Eval("AgreementCode") %>" class="btn btn-xs iframe">
                                                                                                <i class="icon-attachment"></i>&nbsp;
                                                                                                                  مرفقات الاجراء
                                                                                            </a>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>




                                                                                </Columns>
                                                                            </asp:DataGrid>

                                                                        </div>
                                                                    </div>

                                                                    <div class="panel panel-flat border-top-info border-bottom-info" style="border: 2px solid transparent; border-right-color: #00BCD4 !important;" id="divRelated" runat="server">
                                                                        <div class="panel-heading">
                                                                            <div style="float: left; display: none">
                                                                                <a href="AgreementsData.aspx?id=" id="relatedlnk" runat="server" class="btn bg-teal btn-xs">
                                                                                    <i class="fa fa-file"></i>&nbsp;
                                                                                            تفاصيل الاتفاقية النهائية
                                                                                </a>
                                                                            </div>
                                                                            <h6 class="panel-title">إجراءات الاتفاقية
                                                                                <asp:Label ID="lblRelatedInitial" runat="server"></asp:Label>
                                                                            </h6>
                                                                        </div>

                                                                        <div class="panel-body">


                                                                            <asp:DataGrid ID="grdProcedures2" runat="server" PageSize="10"
                                                                                class="table table-hover table-striped table-bordered table-advanced tablesorter"
                                                                                AutoGenerateColumns="False"
                                                                                BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                                                                                CellPadding="3" Width="100%" OnItemDataBound="grdUnits_ItemDataBound">
                                                                                <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                                <ItemStyle CssClass="grdItem" />
                                                                                <AlternatingItemStyle CssClass="grdItem" />
                                                                                <HeaderStyle CssClass="grdHead" BackColor="#FF7043" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                                <FooterStyle CssClass="grdFoot" />
                                                                                <PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
                                                                                    PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
                                                                                <Columns>
                                                                                    <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
                                                                                    <asp:TemplateColumn HeaderText="الاجراء">

                                                                                        <HeaderStyle HorizontalAlign="right" />
                                                                                        <ItemTemplate>
                                                                                            <%#Eval("Agreement_ProcedureTypes.NameAr") %>

                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>

                                                                                    <asp:BoundColumn DataField="ActionDate" HeaderText="تاريخ التسجيل " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="ProcedureDate" HeaderText="تاريخ الاجراء " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Remarks" HeaderText="ملاحظات"></asp:BoundColumn>
                                                                                    <asp:TemplateColumn HeaderText="مرفقات">
                                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemTemplate>

                                                                                            <a href="AgreementAttachments.aspx?ProcedureID=<%#Eval("code") %>&AgreementCode=<%#Eval("AgreementCode") %>" class="btn btn-xs iframe">
                                                                                                <i class="icon-attachment"></i>&nbsp;
                                                                                                                  مرفقات الاجراء
                                                                                            </a>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>




                                                                                </Columns>
                                                                            </asp:DataGrid>

                                                                            <%--<asp:DataGrid ID="grdRelatedAgreements" runat="server"
                                                                    DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="20" class="table datatable-basic dataTable no-footer"
                                                                    Width="100%" >
                                                                    <SelectedItemStyle ForeColor="White" />
                                                                    <ItemStyle CssClass="grdItem" />
                                                                    <AlternatingItemStyle CssClass="grdItem" />
                                                                    <PagerStyle Visible="false" />
                                                                    <HeaderStyle CssClass="grdHead" BackColor="#F44336 " ForeColor="White" Font-Bold="False" />
                                                                    <Columns>

                                                                        <asp:BoundColumn Visible="false" HeaderText="Code" DataField="code"></asp:BoundColumn>
                                                                        <asp:BoundColumn Visible="false" HeaderText="Agr_TypeCode" DataField="Agr_TypeCode"></asp:BoundColumn>

                                                                        <asp:BoundColumn Visible="false" HeaderText="Agr_CatID" DataField="Agr_CatID"></asp:BoundColumn>
                                                                        <asp:BoundColumn Visible="false" HeaderText="Agr_OrgID" DataField="Agr_OrgID"></asp:BoundColumn>
                                                                        <asp:BoundColumn Visible="false" HeaderText="Agr_OrgID" DataField="Agr_OrgID"></asp:BoundColumn>
                                                                        <asp:BoundColumn Visible="false" HeaderText="LastActionID" DataField="LastActionID"></asp:BoundColumn>


                                                                        <asp:BoundColumn DataField="Agr_Serial" HeaderText="مسلسل ">
                                                                            <HeaderStyle Wrap="false" />
                                                                        </asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="AgreementTypeNameAr" HeaderText="نوع الاتفاقية">
                                                                            <HeaderStyle Wrap="false" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="AgreementCatNameAr" HeaderText="التصنيف ">
                                                                            <HeaderStyle Wrap="false" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="AgreementOrgNameAr" HeaderText="الجهة/الدولة ">
                                                                            <HeaderStyle Wrap="false" />
                                                                        </asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="Agr_ReciveDate" HeaderText="التاريخ" DataFormatString="{0:dd/MM/yyyy}">
                                                                            <HeaderStyle Wrap="false" />
                                                                        </asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="Agr_Subject" HeaderText="عنوان الاتفاقية">
                                                                            <HeaderStyle Wrap="false" />
                                                                        </asp:BoundColumn>



                                                                        <asp:BoundColumn DataField="Agr_StartDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="من تاريخ">
                                                                            <HeaderStyle Wrap="false" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="Agr_EndDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="إلى تاريخ">
                                                                            <HeaderStyle Wrap="false" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundColumn>

                                                                        <asp:TemplateColumn HeaderText="الحالة ">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" />

                                                                            <ItemTemplate>
                                                                                <a href="AgreementProcedures.aspx?AgreementCode=<%#Eval("code") %>" class="iframe">
                                                                         <%# gets(Eval("LastProcedureNameAr")) %> </a>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>

                                                                        <asp:TemplateColumn HeaderText="النوع   ">
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" />

                                                                            <ItemTemplate>
                                                                                <%# getAgreementType(getBool(Eval("isInitial").ToString())) %>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>

                                                                        <asp:TemplateColumn HeaderText="مرفقات">
                                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemTemplate>
                                                                                <a href="AgreementMainAttachments.aspx?AgreementCode=<%#Eval("Code") %>" class="btn btn-default btn-xs iframe">
                                                                                    <i class="icon-attachment"></i>&nbsp;
                                                                       مرفقات
                                                                                </a>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>


                                                                        <asp:TemplateColumn HeaderText="التفاصيل">
                                                                            <ItemStyle HorizontalAlign="left" />
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="left" />

                                                                            <ItemTemplate>
                                                                                <a href="AgreementsData.aspx?id=<%#Eval("code") %>" class="btn btn-default btn-xs">
                                                                                    <i class="fa fa-file"></i>&nbsp;
                                                                                            التفاصيل
                                                                                </a>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>

                                                                    </Columns>
                                                                </asp:DataGrid>--%>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="">
                                                                <ItemStyle HorizontalAlign="center" Width="20px" />
                                                                <ItemTemplate>
                                                                    <img style="cursor: pointer;" src="/layout/images/plus.gif" alt="" border="0" runat="server" id="imgControl2" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="code"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Agr_TypeCode" DataField="Agr_TypeCode"></asp:BoundColumn>

                                                            <asp:BoundColumn Visible="false" HeaderText="Agr_CatID" DataField="Agr_CatID"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Agr_OrgID" DataField="Agr_OrgID"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Agr_OrgID" DataField="Agr_OrgID"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="LastActionID" DataField="LastActionID"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="isInitial" DataField="isInitial"></asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Agr_Serial" HeaderText="مسلسل ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="AgreementTypeNameAr" HeaderText="نوع الاتفاقية">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="AgreementCatNameAr" HeaderText="التصنيف ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="AgreementOrgNameAr" HeaderText="الجهة/الدولة ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Agr_ReciveDate" Visible="false" HeaderText="التاريخ" DataFormatString="{0:dd/MM/yyyy}">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Agr_Subject" HeaderText="عنوان الاتفاقية">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>



                                                            <asp:BoundColumn DataField="Agr_StartDate" Visible="false" DataFormatString="{0:dd/MM/yyyy}" HeaderText="من تاريخ">
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Agr_EndDate" Visible="false" DataFormatString="{0:dd/MM/yyyy}" HeaderText="إلى تاريخ">
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="الاجراء النهائي ">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Right" />

                                                                <ItemTemplate>
                                                                    <%--  <a href="AgreementProcedures.aspx?AgreementCode=<%#Eval("code") %>" class="iframe">--%>
                                                                    <%# gets(Eval("LastProcedureNameAr")) %>

                                                                    <%--</a>--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                            <asp:TemplateColumn HeaderText="الحالة   ">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />

                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAgreStatus" runat="server"></asp:Label>
                                                                    <%--   <%# getAgreementType(getBool(Eval("isInitial").ToString())) %>--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:BoundColumn DataField="RelatedOrgNameAr" HeaderText="جهة الإختصاص  ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText=" التفاصيل/ مرفقات" Visible="false">
                                                                <ItemStyle HorizontalAlign="left" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="left" />

                                                                <ItemTemplate>
                                                                    <a href="AgreementsData.aspx?id=<%#Eval("code") %>" class="btn btn-default btn-xs">
                                                                        <i class="fa fa-file"></i>&nbsp;
                                                                                            التفاصيل
                                                                    </a>
                                                                    <br />
                                                                    <div style="margin-top: 5px">
                                                                        <a href="AgreementMainAttachments.aspx?AgreementCode=<%#Eval("Code") %>" class="btn btn-default btn-xs iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                       مرفقات
                                                                        </a>
                                                                    </div>
                                                                    <div style="margin-top: 5px;display:<%#viewlinkedfile(gets(Eval("Law_DocDataRefId")))%>">
                                                                        <a href="/modules/laws/forms/LawDocDatalnk.aspx?agreementid=<%#Eval("Code") %>&lawDocRefId=<%#Eval("Law_DocDataRefId") %>" class="btn btn-warning btn-labeled btn-xs iframe">
                                                                            <b><i class="glyphicon glyphicon-link"></i> </b> قانون مرتبط
                                                                        </a>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                               <asp:TemplateColumn HeaderText="ربط">
                                                                <ItemStyle HorizontalAlign="right" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>


                                                                    <asp:LinkButton runat="server" id="lnkAgreementlink" CommandName="link" CssClass="btn btn-warning btn-labeled btn-sm">
                                                                       <b> <i class="glyphicon glyphicon-link"></i></b>
                                                                        ربط

                                                                    </asp:LinkButton>

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
                                                            runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>


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
                                <li class="<%=activeTab(1) %>" onclick="setactiveTab(1)"><a href="#badges-tab1" data-toggle="tab"><i class="icon-menu7 position-left"></i>بيانات الاتفاقية  </a></li>
                                <li class="<%=activeTab(2) %>" onclick="setactiveTab(2)"><a href="#badges-tab2" data-toggle="tab">إجراءات الاتفاقية  <%= getAgreementType(getBool(hdnIsInitial.Value)) %>  <span class="badge badge-success  position-right">
                                    <asp:Label ID="lblProcerduresCount2" runat="server" Text="0"></asp:Label></span></a></li>
                                <li class="<%=activeTab(3) %>" onclick="setactiveTab(3)" style="display: <%= showRelatedFilestab()%>"><a href="#badges-tab3" data-toggle="tab">إجراءات الاتفاقية
                                    <asp:Label ID="lbltrlatedType" runat="server"></asp:Label><span class="badge badge-success  position-right">
                                        <asp:Label ID="lblProcerdures2Count" runat="server" Text="0"></asp:Label></span></a></li>


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
                                                    <input id="hdnRelatedMaster" runat="server" type="hidden" />
                                                    <input id="hdnactivetab" runat="server" type="hidden" />
                                                    <input id="hdnIsInitial" runat="server" type="hidden" />

                                                    <fieldset class="content-group">
                                                        <%--  <legend class="text-semibold">
                                                <i class="icon-file-text2 position-left"></i>
                                                بيانات الاتفاقيه

                                            </legend>--%>




                                                        <div class="row" style="margin-bottom: 15px;">
                                                            <div class="col-md-12">

                                                                <label class="col-md-1 control-label" for="">عنوان الاتفاقية <span class="text-danger">*</span>  </label>

                                                                <div class="col-md-11">
                                                                    <asp:TextBox runat="server" ID="txtSubject" TextMode="MultiLine" class="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">

                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">مسلسل<span class="text-danger">*</span></label>

                                                                    <div class="col-md-3">

                                                                        <asp:TextBox ID="txtSerialNUm" class="form-control" placeholder="رقم" runat="server"></asp:TextBox>

                                                                    </div>

                                                                    <div class="col-md-6">
                                                                        <asp:TextBox ID="txtserialYear" class="form-control" placeholder="سنة" runat="server"></asp:TextBox>

                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">النوع<span class="text-danger">*</span></label>

                                                                    <div class="col-md-9">
                                                                        <asp:DropDownList ID="lstTypeCode" runat="server" class="Select2Drop form-control"></asp:DropDownList>

                                                                    </div>
                                                                </div>



                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label">ملف خاص </label>

                                                                    <div class="col-md-9">

                                                                        <asp:CheckBox ID="chkIsPrivate" runat="server" />
                                                                    </div>

                                                                </div>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">الجهة/الدولة<span class="text-danger">*</span></label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstorg" runat="server" class="Select2Drop form-control"></asp:DropDownList>

                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">جهة الإختصاص <span class="text-danger">*</span></label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstAgrRelatedOrgs" runat="server" class="Select2Drop form-control"></asp:DropDownList>

                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label">التصنيف<span class="text-danger">*</span> </label>

                                                                    <div class="col-md-9">
                                                                        <asp:DropDownList ID="lstCats" runat="server" class="Select2Drop form-control"></asp:DropDownList>



                                                                    </div>

                                                                </div>

                                                                <div class="form-group" style="display: none">
                                                                    <label class="col-md-3 control-label" for="">تاريخ الاتفاقية </label>

                                                                    <div class="col-md-9">

                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>

                                                                            <asp:TextBox ID="txtCreationDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

                                                                        </div>

                                                                    </div>
                                                                </div>



                                                                <%--   <div class="form-group">
                                                                    <label class="col-lg-3 control-label">الحالة<span class="text-danger">*</span>:</label>
                                                                    <div class="col-lg-9">
                                                                        <asp:DropDownList ID="lstStatus" class="Select2Drop form-control" runat="server"></asp:DropDownList>
                                                                    </div>
                                                                </div>--%>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group" style="" runat="server" id="initial1">
                                                                    <label class="col-md-3 control-label" for="">تاريخ توقيع الاتفاقية :   </label>

                                                                    <div class="col-md-9">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                                            <asp:TextBox ID="txtSignDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group" runat="server" id="initial2">
                                                                    <label class="col-md-4 control-label" for="">تاريخ السريان من : </label>

                                                                    <div class="col-md-8">




                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                                            <asp:TextBox ID="txtStartDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

                                                                        </div>

                                                                    </div>
                                                                </div>

                                                                <div class="form-group" runat="server" id="initial3">
                                                                    <label class="col-md-4 control-label" for="">إلى : </label>

                                                                    <div class="col-md-8">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>

                                                                            <asp:TextBox ID="txtEndDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

                                                                        </div>



                                                                    </div>
                                                                </div>


                                                            </div>
                                                            <div class="col-md-12" style="display: none">
                                                                <div class="col-md-5" style="background-color: aliceblue; padding: 5px; border-radius: 3px; height: 150px;" id="cmgsdivFile1" runat="server">
                                                                    <%= getAgreementType(getBool(hdnIsInitial.Value)) %>
                                                                    <hr />
                                                                    <div class="form-group">
                                                                        <label class="col-md-2 control-label" for="">رقم قرار مجلس الوزراء  </label>
                                                                        <div class="col-md-2">
                                                                            <asp:TextBox ID="TextBox1" class="form-control" runat="server"></asp:TextBox>
                                                                        </div>
                                                                        <label class="col-md-1 control-label" for="">صورة القرار  </label>
                                                                        <div class="col-md-3">
                                                                            <asp:FileUpload ID="FileUpload1" runat="server" class="file-styled" />

                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <span class="help-block2">| Or | </span>
                                                                            <asp:LinkButton runat="server" ID="lnkInitailScan" class="btn btn-info btn-xs" OnClick="lnkInitailScan_Click"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-1">
                                                                </div>
                                                                <div class="col-md-5" style="background-color: peachpuff; padding: 5px; border-radius: 3px; height: 150px; float: left" id="initial4" runat="server">
                                                                    <asp:Label ID="lbltrlatedType2" runat="server"></asp:Label>
                                                                    <hr />
                                                                    <div class="form-group">
                                                                        <label class="col-md-2 control-label" for="">رقم قرار مجلس الوزراء  </label>
                                                                        <div class="col-md-2">
                                                                            <asp:TextBox ID="TextBox2" class="form-control" runat="server"></asp:TextBox>
                                                                        </div>
                                                                        <label class="col-md-1 control-label" for="">صورة القرار  </label>
                                                                        <div class="col-md-3">
                                                                            <asp:FileUpload ID="FileUpload2" runat="server" class="file-styled" />

                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <span class="help-block2">| Or | </span>
                                                                            <asp:LinkButton runat="server" ID="lnkRelatedCMGS" class="btn btn-info btn-xs" OnClick="lnkRelatedCMGS_Click"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>



                                                            <div class="col-md-12" style="margin-top: 20px;">
                                                                <div class="form-actions" style="display:none">
                                                                    <div class="col-md-offset-6 col-md-12">


                                                                        <asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary" OnClick="btnSave_Click1"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

                                                                        &nbsp;

                                                                      <asp:LinkButton ID="btnProcedure" Visible="false" runat="server" OnClientClick="return ValidateProcesdureadd()" class="btn bg-brown" OnClick="btnProcedure_Click"><i class='icon-attachment'></i>&nbsp; الاجراءات والمرفقات  </asp:LinkButton>


                                                                        &nbsp;
				                                             <asp:Button runat="server" ID="btnCancel" class="btn btn-default" Text=" الغاء / رجوع  " OnClick="btnCancel_Click" />
                                                                        &nbsp;
                                                             <a id="anchorAgreementAttachemnt" visible="false" runat="server" href='#' class="btn btn-success btn-labeled iframe">
                                                                 <b><i class="icon-attachment"></i></b>
                                                                 مرفقات
                                                             </a>

                                                                        &nbsp;
                                                                          <asp:LinkButton ID="lnkMOvetoFinal" runat="server" class="btn btn-danger" OnClientClick="return confirm('هل انت متأكد من تحويل الملف لإتفاقية نهائية?');" OnClick="lnkMOvetoFinal_Click"><i class='icon-cog5'></i>&nbsp;   التحويل لاتفاقية نهائية </asp:LinkButton>

                                                                        <a href="#" class="btn btn-warning btn-labeled iframe linkedpopup" id="lnklawDoc" runat="server"><b><i class="glyphicon glyphicon-link"></i></b>إضافة قانون مرتبط</a>

                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>




                                                    </fieldset>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                </div>

                                <div class="tab-pane <%=activeTab(2) %>" id="badges-tab2">

                                    <div class="form-horizontal" id="divAddProcedure" runat="server" visible="false">

                                        <div class="row">

                                            <div class="col-md-6">

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">الاجراء<span class="text-danger">*</span></label>

                                                    <div class="col-md-9">

                                                        <asp:DropDownList ID="lstProcedureType" runat="server" class="form-control" OnSelectedIndexChanged="lstProcedureType_SelectedIndexChanged1" AutoPostBack="True"></asp:DropDownList>
                                                    </div>

                                                </div>
                                                <div class="form-group" id="divRelatedOrg" runat="server" visible="false">
                                                    <label class="col-md-3 control-label" for="">الجهة </label>

                                                    <div class="col-md-9">

                                                        <asp:DropDownList ID="lstRelatedOrgs" runat="server" class="Select2Drop form-control"></asp:DropDownList>
                                                    </div>

                                                </div>

                                                <div id="divDessionNum" runat="server" visible="false" style="background-color: aliceblue; padding: 20px; border-radius: 5px;">
                                                    <div class="form-group">
                                                        <div class="col-md-6">

                                                            <asp:RadioButtonList ID="rblPublishType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem Value="1">&nbsp;&nbsp;صدر بقانون&nbsp;&nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="2">&nbsp;&nbsp;صدر بمرسوم&nbsp;&nbsp;</asp:ListItem>

                                                            </asp:RadioButtonList>
                                                        </div>


                                                        <label class="col-md-3 control-label" for="">رقم القانون | المرسوم  </label>

                                                        <div class="col-md-3">

                                                            <asp:TextBox ID="txtDessionNum" class="form-control" runat="server"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label">الملف</label>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="lblViewDessionFile" runat="server"></asp:Label>
                                                            <asp:FileUpload ID="txtDessionFile" runat="server" class="file-styled" />
                                                            <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                        </div>
                                                      <%--  <div class="col-md-3">
                                                            <span class="help-block2">| Or | </span>
                                                            <asp:LinkButton runat="server" ID="btnScanDession" class="btn btn-info btn-xs" OnClick="btnScanDession_Click"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                        </div>--%>

                                                    </div>


                                                </div>
                                                <div id="divDessionNum4" runat="server" visible="false" style="background-color: aliceblue; padding: 20px; border-radius: 5px;">

                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label" for="">رقم قرار مجلس الوزراء  </label>
                                                        <div class="col-md-2">
                                                            <asp:TextBox ID="txtcmgsDession" class="form-control" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-1 control-label" for="">صورة القرار  </label>
                                                        <div class="col-md-3">
                                                            <asp:FileUpload ID="txtimage1" runat="server" class="file-styled" />

                                                        </div>
                                                       <%-- <div class="col-md-3">
                                                            <span class="help-block2">| Or | </span>
                                                            <asp:LinkButton runat="server" ID="lnkScanCMgs" class="btn btn-info btn-xs" OnClick="lnkScanCMgs_Click"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                        </div>--%>
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
                                                <div style="padding: 10px; background-color: aliceblue; border-radius: 3px;" id="proattchmet" runat="server">
                                                    <div id="divProAttache">
                                                        إضافة مرفق للإجراء
                                                      <hr />
                                                        <div class="form-group" style="display: none">
                                                            <label class="col-md-3 control-label" for="">نوع المرفق  </label>

                                                            <div class="col-md-9">
                                                                <asp:DropDownList ID="lstAttachmentType" class="form-control" runat="server"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label class="col-md-3 control-label">المرجع </label>

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
                                                           <%-- <div class="col-md-3">
                                                                <span class="help-block2">| Or | </span>
                                                                <asp:LinkButton runat="server" ID="lnkScan" class="btn btn-info btn-xs" OnClick="lnkScan_Click" OnClientClick="return ValidateProScan1()"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                            </div>--%>

                                                        </div>


                                                    </div>

                                                </div>
                                            </div>





<%--                                            <div class="col-md-12">
                                                <div class="form-actions">
                                                    <div class="col-md-offset-9 col-md-12">
                                                        <asp:LinkButton ID="lnkSaveProcesure" runat="server" class="btn btn-primary" OnClientClick="return ValidateProcedures()" OnClick="lnkSaveProcesure_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

                                                        &nbsp;
				                               <asp:Button runat="server" ID="lnkCancelProcedure" class="btn btn-default" Text=" الغاء  " OnClick="lnkCancelProcedure_Click" />

                                                    </div>
                                                </div>
                                            </div>--%>

                                        </div>

                                    </div>
                                    <div class="row" id="divshowProcesure" runat="server">
                                        <div class="col-lg-12">
                                            <div class="portlet box">
                                                <div class="portlet-header" style="display:none">

                                                    <div class="actions pull-right" style="margin-bottom: 10px;">

                                                        <asp:LinkButton runat="server" ID="lnkAddNewProcedurew" class="btn btn-info btn-xs" OnClick="lnkAddNewProcedurew_Click"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

                                                        <asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="lnkDeleteProcedure" class="btn btn-danger btn-xs" OnClick="lnkDeleteProcedure_Click"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

                                                    </div>
                                                </div>
                                                <div class="portlet-body">
                                                    <div class="datatable-scroll" style="padding-top: 20px;">
                                                        <asp:DataGrid runat="server" ID="grdProcedure" AutoGenerateColumns="False"
                                                            AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemDataBound="grdData_ItemDataBound" OnEditCommand="grdData_EditCommand">
                                                            <PagerStyle Visible="False" />
                                                            <HeaderStyle BackColor="#66BB6A" ForeColor="White" Font-Bold="True" />
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

                                                                <asp:TemplateColumn HeaderText="تعديل" Visible="false">
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
                                                            <asp:Label ID="lblProcerduresCount" runat="server"></asp:Label>
                                                        </div>
                                                        <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">
                                                            <%-- <a class="paginate_button previous disabled" aria-controls="DataTables_Table_3" data-dt-idx="0" tabindex="0" id="DataTables_Table_3_previous">→</a>
                        <span><a class="paginate_button current" aria-controls="DataTables_Table_3" data-dt-idx="1" tabindex="0">1</a>
                            <a class="paginate_button " aria-controls="DataTables_Table_3" data-dt-idx="2" tabindex="0">2</a>


                        </span>
                        <a class="paginate_button next" aria-controls="DataTables_Table_3" data-dt-idx="3" tabindex="0" id="DataTables_Table_3_next">←</a>--%>


                                                            <cc1:Pager CurrentIndex="1" OnCommand="pager_Command" ShowFirstLast="False" ID="pager2"
                                                                runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>


                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                </div>

                                <div class="tab-pane <%=activeTab(3) %>" id="badges-tab3">

                                    <div class="form-horizontal" id="divAddProcedure2" runat="server" visible="false">

                                        <div class="row">

                                            <div class="col-md-6">

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">الاجراء<span class="text-danger">*</span></label>

                                                    <div class="col-md-9">

                                                        <asp:DropDownList ID="lstProcedureType2" runat="server" class="form-control" OnSelectedIndexChanged="lstProcedureType2_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>

                                                </div>
                                                <div class="form-group" id="divRelatedOrg2" runat="server" visible="false">
                                                    <label class="col-md-3 control-label" for="">الجهة </label>

                                                    <div class="col-md-9">

                                                        <asp:DropDownList ID="lstRelatedOrgs2" runat="server" class="Select2Drop form-control"></asp:DropDownList>
                                                    </div>

                                                </div>
                                                <div id="divDessionNum2" runat="server" visible="false" style="background-color: aliceblue; padding: 20px; border-radius: 5px;">
                                                    <div class="form-group">
                                                        <div class="col-md-6">

                                                            <asp:RadioButtonList ID="rblPublishType2" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem Value="1">&nbsp;&nbsp;صدر بقانون&nbsp;&nbsp;</asp:ListItem>
                                                                <asp:ListItem Value="2">&nbsp;&nbsp;صدر بمرسوم&nbsp;&nbsp;</asp:ListItem>

                                                            </asp:RadioButtonList>
                                                        </div>


                                                        <label class="col-md-3 control-label" for="">رقم القانون | المرسوم  </label>

                                                        <div class="col-md-3">

                                                            <asp:TextBox ID="txtDessionNum2" class="form-control" runat="server"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label">الملف</label>
                                                        <div class="col-md-6">
                                                            <asp:Label ID="Label2" runat="server"></asp:Label>
                                                            <asp:FileUpload ID="txtDessionFile2" runat="server" class="file-styled" />
                                                            <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                        </div>
                                                      <%--  <div class="col-md-3">
                                                            <span class="help-block2">| Or | </span>
                                                            <asp:LinkButton runat="server" ID="lnkDessionScan2" class="btn btn-info btn-xs" OnClick="lnkDessionScan2_Click"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                        </div>--%>

                                                    </div>




                                                </div>

                                                <div id="divDessionNum3" runat="server" visible="false" style="background-color: aliceblue; padding: 20px; border-radius: 5px;">

                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label" for="">رقم قرار مجلس الوزراء  </label>
                                                        <div class="col-md-2">
                                                            <asp:TextBox ID="txtcmgsDession2" class="form-control" runat="server"></asp:TextBox>
                                                        </div>
                                                        <label class="col-md-1 control-label" for="">صورة القرار  </label>
                                                        <div class="col-md-3">
                                                            <asp:FileUpload ID="txtcmgsimage2" runat="server" class="file-styled" />

                                                        </div>
                                                        <div class="col-md-3">
                                                            <span class="help-block2">| Or | </span>
                                                            <asp:LinkButton runat="server" ID="btnCMGSScan2" class="btn btn-info btn-xs" OnClick="btnCMGSScan2_Click"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                        </div>
                                                    </div>

                                                </div>


                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">التاريخ<span class="text-danger">*</span> </label>

                                                    <div class="col-md-9">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                            <asp:TextBox ID="txtProcedureActionDate2" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                </div>

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">ملاحظات  </label>

                                                    <div class="col-md-9">

                                                        <asp:TextBox ID="txtRemarks2" class="form-control" runat="server"></asp:TextBox>

                                                    </div>
                                                </div>
                                                <div style="padding: 10px; background-color: aliceblue; border-radius: 3px;" id="proAttachment2" runat="server">
                                                    <div>
                                                        إضافة مرفق للإجراء
                                            <hr />
                                                        <div class="form-group" style="display: none">
                                                            <label class="col-md-3 control-label" for="">نوع المرفق  </label>

                                                            <div class="col-md-9">
                                                                <asp:DropDownList ID="lstAttachmentType2" class="form-control" runat="server"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label class="col-md-3 control-label">المرجع </label>

                                                            <div class="col-md-9">

                                                                <asp:TextBox runat="server" class="form-control" ID="txtRef2"></asp:TextBox>


                                                            </div>

                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-3 control-label">الملف</label>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                                                <asp:FileUpload ID="txtImage2" runat="server" class="file-styled" />
                                                                <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <span class="help-block2">| Or | </span>
                                                                <asp:LinkButton runat="server" ID="lnkScan2" OnClientClick="return ValidateProScan2();" class="btn btn-info btn-xs" OnClick="lnkScan2_Click"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                            </div>

                                                        </div>
                                                    </div>

                                                </div>
                                            </div>





                                            <div class="col-md-12">
                                                <div class="form-actions">
                                                    <div class="col-md-offset-9 col-md-12">
                                                        <asp:LinkButton ID="lnkSaveProcesure2" runat="server" class="btn btn-primary" OnClientClick="return ValidateProcedures2()" OnClick="lnkSaveProcesure2_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

                                                        &nbsp;
				                               <asp:Button runat="server" ID="lnkCancelProcedure2" class="btn btn-default" Text=" الغاء  " OnClick="lnkCancelProcedure2_Click" />

                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                    <div class="row" id="divshowProcesure2" runat="server">
                                        <div class="col-lg-12">
                                            <div class="portlet box">
                                                <div class="portlet-header" style="display:none">

                                                    <div class="actions pull-right" style="margin-bottom: 10px;">

                                                        <asp:LinkButton runat="server" ID="lnkAddProcedure2" class="btn btn-info btn-xs" OnClick="lnkAddProcedure2_Click"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

                                                        <asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="lnkDeleteProcedure2" class="btn btn-danger btn-xs" OnClick="lnkDeleteProcedure2_Click"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

                                                    </div>
                                                </div>
                                                <div class="portlet-body">
                                                    <div class="datatable-scroll" style="padding-top: 20px;">
                                                        <asp:DataGrid runat="server" ID="grdProcedure2" AutoGenerateColumns="False"
                                                            AllowPaging="false" PageSize="20" class="table datatable-basic dataTable no-footer" OnEditCommand="grdProcedure2_EditCommand">
                                                            <PagerStyle Visible="False" />
                                                            <HeaderStyle BackColor="#FF7043" ForeColor="White" Font-Bold="True" />
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

                                                                <asp:TemplateColumn HeaderText="تعديل" Visible="false">
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
       <script type="text/javascript">

<asp:Literal id="litScript" runat="server" />
</script>
</asp:Content>
