<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="MedalsData.aspx.cs" Inherits="UI.Web.Medals.Forms.MedalsData" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/DeleteConfirm.ascx"  TagPrefix="uc"  TagName="DeleteConfirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">



    <style type="text/css">
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
        function chkProcedure() {


            var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات الوسام ");
                txt.focus();
                return false;
            }






            return true;

        }

        function ValidateProcesdureadd() {
            var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات الوسام اولا ");
                txt.focus();
                return false;
            }
            return true


        }

        function chkImage() {


         <%--   var txt = document.getElementById("<%=txtfilnum.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  رقم الملف");
                txt.focus();
                return false;
            }--%>

            var txt = document.getElementById("<%=txtSerialNUm.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  رقم وارد الجهة");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtserialYear.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  رقم وارد الجهة");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtMedalDate.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،  اختر تاريخ المنح  ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=lstorg.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،  اختر الجهة  ");
                txt.focus();
                return false;
            }



            return true;
        }


        function LinkAddClick() {
            // alert("in");

            return InsertItem();
        }

        var civilIds = [];
        function InsertItem() {

            //  alert(getObjById("lstMedalType"));

            var lstMedalType = getObjById("lstMedalType").value;


            var txtname = getObjById("txtname").value;

            var lstJobGrade = getObjById("lstJobGrade").value;

            var txtCivilID = getObjById("txtCivilID").value;
            //  alert(txtCivilID);
            // var lstGrantreasons = getObjById("lstGrantreasons").value;
            // alert(lstMedalType);


            if (lstMedalType == "" || lstMedalType == "0") {
                //alert("فضلا ،اختر نوع الوسام");
                new $.Zebra_Dialog("فضلا ،اختر نوع الوسام  ");
                return false;
            }
            if (txtname == "") {
                new $.Zebra_Dialog("فضلا ،ادخل الاسم   ");
                return false;
            }

            if (lstJobGrade == "" || lstJobGrade == "0") {
                new $.Zebra_Dialog("فضلا ،اختر الرتبة   ");
                return false;
            }


            //  alert((lstJobGrade != "اخري" && lstJobGrade != "اخرى"));
            //if (txtCivilID == "" && (lstJobGrade != "اخري" && lstJobGrade != "اخرى")) {
            //    new $.Zebra_Dialog("فضلا ،ادخل الرقم المدني   ");
            //    return false;
            //}

            if (txtCivilID != "" && (txtCivilID.length < 12)) {
                new $.Zebra_Dialog("فضلا ،ادخل الرقم المدني من 12 رقم   ");
                return false;
            }
            ////Check CicilID Existance
            //if (txtCivilID != "") {
            //    //Get Added Arrray
            //    if (civilIds.indexOf(txtCivilID) > -1) {
            //        new $.Zebra_Dialog("الرقم المدني مسجل من قبل");
            //        return false;
            //    }

            //}

            //civilIds.push(txtCivilID);
          <%--  var btnAdd = document.getElementById("<%=btnAddNewItem.ClientID %>")
            btnAdd.click();--%>
        }

        function getObjById(id) {
            // alert("ddd");
            for (var i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i]
                if (elm.id.indexOf(id) != -1) {
                    return elm;
                }
            }
            return null;
        }
        function setactiveTab(tabindex) {
            //alert("para:" + tabindex)
            var txt = document.getElementById("<%=hdnactivetab.ClientID %>");
               txt.value = tabindex;

        }



        function validateFileUpladed() {
           
            var txt = document.getElementById("<%=txtUploadPersons.ClientID %>");
            
            if (txt.value === "" ) {
                new $.Zebra_Dialog("فضلا ،  اختر ملف قائمة الاسماء  ");
                txt.focus();
                txt.select();
                return false;
            }

            if (txt.value != "") {
                var im = txt.value;
                if (im.indexOf(".") == -1) {
                    new $.Zebra_Dialog("فضلا ،  اختر ملف صحيح     ");
                    txt.focus();
                    txt.select();
                    return false;
                }
                var ext = im.substr(im.lastIndexOf(".") + 1)
                ext = ext.toLowerCase();
                if (ext != "xls" && ext != "xlsx"  ) {
                    new $.Zebra_Dialog("  File Format Supported are: xls,xlsx ");
                    txt.focus();
                    txt.select();
                    return false;
                }
            }
            return true;
        }
    </script>


     <input id="hdnScannerfilepath" runat="server" type="hidden" />

    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        الأنواط والأوسمة
                    </h4>
                </div>

            </div>

        </div>
    </div>



    <asp:UpdatePanel runat="server" ID="Updatepanel1" ChildrenAsTriggers="true" UpdateMode="conditional">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row mbl" id="tblSearch" style="min-height: 450px;" runat="server">

        <div class="panel panel-flat">
            <div class="panel-heading">
                <div class="position-right" style="float: left; margin-left: 30px">

                    <asp:LinkButton runat="server" ID="btnNew" class="btn btn-success btn-xs" OnClick="btnNew_Click1"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

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
                                        رقم الوارد :
                                    </label>
                                    <div class="col-lg-3">

                                        <asp:TextBox ID="txtFilterFileSerial" runat="server" class="form-control"></asp:TextBox>


                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        السنة :
                                    </label>
                                    <div class="col-lg-3">

                                        <asp:TextBox ID="txtFilterFileYear" runat="server" class="form-control"></asp:TextBox>


                                    </div>
                                </div>

                                <div class="form-group" style="display:none">
                                    <label class="col-lg-3 control-label">
                                        رقم الملف :
                                    </label>
                                    <div class="col-lg-3">

                                        <asp:TextBox ID="txtFilterFileNUm" runat="server" class="form-control"></asp:TextBox>


                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        جزء من الاسم
                                    </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="txtFilterName" runat="server" class="form-control"></asp:TextBox>


                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الرتبة:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterJobGrade" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-4">

                                
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">نوع الوسام:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterType" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>



                                <div class="form-group" style="display:none">
                                    <label class="col-lg-3 control-label">الاجراء:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterProcedures" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الجهة:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterOrg" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-4">

                                <div class="form-group">
                                    <label class="col-lg-4 control-label">التاريخ المنح من</label>
                                    <div class="col-lg-8">




                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
                                            <asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                        </div>



                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-lg-4 control-label">إلى  :</label>
                                    <div class="col-lg-8">

                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <asp:TextBox ID="txtFilterDateTo" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>

                                        </div>

                                    </div>
                                </div>
                                
                                <div class="form-group"  style="display:none">
                                    <label class="col-lg-3 control-label">التصنيف:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterMedalCat" class="Select2Drop" runat="server">
                                            <asp:ListItem Value="0" Text="الكل" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="داخلي"  ></asp:ListItem>
                                            <asp:ListItem Value="2" Text="خارجي" ></asp:ListItem>

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
                                                        <asp:Label ID="lblcount2" runat="server"></asp:Label>)</span>

                                                    <asp:LinkButton ID="lnkSearchback" CssClass="control-arrow" runat="server" OnClick="lnkSearchback_Click"> رجوع  <i class=" icon-backward2"></i></asp:LinkButton>

                                                </legend>




                                                <div class="datatable-scroll">

                                                    <asp:DataGrid ID="grdInboundItems" runat="server"
                                                        DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="20" class="table datatable-basic dataTable no-footer"
                                                        Width="100%" OnItemDataBound="grdInboundItems_ItemDataBound" OnItemCommand="grdInboundItems_ItemCommand">
                                                        <SelectedItemStyle ForeColor="White" />
                                                        <ItemStyle CssClass="grdItem" />
                                                        <AlternatingItemStyle CssClass="grdItem" />
                                                        <PagerStyle Visible="false" />
                                                        <HeaderStyle BackColor="Black" ForeColor="White" Font-Bold="False" />
                                                        <Columns>
                                                            <asp:ButtonColumn HeaderText="Del." Text="<img border=0 src='images/delete.gif' alt='Delete'>" CommandName="Delete" Visible="false">
                                                                <HeaderStyle></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:ButtonColumn>
                                                            <asp:TemplateColumn HeaderText="">
                                                                <ItemStyle HorizontalAlign="Right" BackColor="#EEF0FA" />
                                                                <ItemTemplate>
                                                                    <div class="panel panel-flat border-top-info border-bottom-info" style="border: 2px solid transparent; border-top-color: #00BCD4 !important; border-bottom-color: #00BCD4 !important;padding:20px;"  >
                                                                     
                                                                    <div style="display:none">
                                                                           الاجراءات
                                                                        <asp:DataGrid ID="grdProcedures" runat="server"
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
                                                                                <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="الاجراء">

                                                                                    <HeaderStyle HorizontalAlign="right" />
                                                                                    <ItemTemplate>
                                                                                        <%#Eval("Medal_M_ProcedureType.NameAr") %>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="Remarks" HeaderText="الموضوع"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ActionDate" HeaderText="تاريخ التسجيل " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ProcedureDate" HeaderText="تاريخ الاجراء " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>

                                                                                <asp:TemplateColumn HeaderText="مرفقات">
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>

                                                                                        <a href="MedalsAttachments.aspx?ProcedureID=<%#Eval("code") %>&MedalMasterID=<%#Eval("MedalMasterID") %>" class="btn btn-default btn-xs iframe">
                                                                                            <i class="icon-attachment"></i>&nbsp;
                                                مرفقات
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>




                                                                            </Columns>
                                                                        </asp:DataGrid>
                                                                    </div>
                                                                        <%--  <hr />--%>
                                                                        <br />
                                                                        <br />
                                                                    الشخصيات
                                                                    <div>
                                                                        <asp:DataGrid ID="grdPersons" runat="server"
                                                                            class="table table-hover table-striped table-bordered table-advanced tablesorter"
                                                                            AutoGenerateColumns="False" AllowPaging="false"
                                                                            BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                                                                            CellPadding="3" Width="100%" OnItemDataBound="grdPersons2_ItemDataBound">
                                                                            <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                            <ItemStyle CssClass="grdItem" />
                                                                            <AlternatingItemStyle CssClass="grdItem" />
                                                                            <HeaderStyle CssClass="grdHead" BackColor="#ece0a4" ForeColor="Black" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                            <FooterStyle CssClass="grdFoot" />
                                                                            <PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
                                                                                PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="م">
                                                                                    <HeaderStyle HorizontalAlign="center" Width="30px" />
                                                                                    <ItemStyle HorizontalAlign="center" Width="30px" />
                                                                                    <ItemTemplate>
                                                                                        <%# (Container.ItemIndex + 1).ToString() %>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="نوع  الوسام">

                                                                                    <HeaderStyle HorizontalAlign="right" />
                                                                                    <ItemTemplate>
                                                                                        <%#Eval("Medal_M_Types.NameAr") %>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="Person_NameEn" HeaderText="الاسم"></asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="  الرتبه">

                                                                                    <HeaderStyle HorizontalAlign="right" />
                                                                                    <ItemTemplate>
                                                                                        <%#Eval("Medal_M_jobGrade.NameAr") %>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="CivilID" HeaderText="الرقم المدني" Visible="false"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MilitaryNum" HeaderText="الرقم العسكري "></asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="PersonTitle" HeaderText="المسمى   "></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CountryName" HeaderText="  الدولة "></asp:BoundColumn>

                                                                                <asp:TemplateColumn HeaderText="ملاحظات">

                                                                                    <HeaderStyle HorizontalAlign="right" />
                                                                                    <ItemTemplate>
                                                                                        <%#Eval("GrantReasonText") %>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>


                                                                            </Columns>
                                                                        </asp:DataGrid>
                                                                    </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="">
                                                                <ItemStyle HorizontalAlign="center" Width="30px" />
                                                                <ItemTemplate>
                                                                    <img style="cursor: pointer;" src="/layout/images/plus.gif" alt="" border="0" runat="server" id="imgControl" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="code"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Medal_OrgID" DataField="Medal_OrgID"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="LastActionID" DataField="LastActionID"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="MedalCatId" DataField="MedalCatId"></asp:BoundColumn>


                                                            <asp:BoundColumn DataField="FileNum" HeaderText="رقم وارد الجهة ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="الجهة">

                                                                <HeaderStyle HorizontalAlign="right" />
                                                                <ItemTemplate>
                                                                    <%#Eval("OrganizationsNameAr") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:BoundColumn DataField="Medal_receivedDate" HeaderText="تاريخ المنح" DataFormatString="{0:dd/MM/yyyy}">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="TransDate" HeaderText="تاريخ التسجيل" DataFormatString="{0:dd/MM/yyyy}">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="التصنيف" Visible="false">

                                                                <HeaderStyle HorizontalAlign="right" />
                                                                <ItemTemplate>
                                                                    <%# viewMedalCat(gets(Eval("MedalCatId"))) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>



                                                            <%--       <asp:TemplateColumn HeaderText="الحالة ">
                                                                <ItemStyle HorizontalAlign="left" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="left" />

                                                                <ItemTemplate>
                                                                    <%#Eval("StatusNameAr") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>--%>

                                                            <asp:TemplateColumn HeaderText="مرفقات">
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <a href="MedalsMainAttachments.aspx?MedalMasterID=<%#Eval("Code") %>" class="btn btn-default btn-xs iframe">
                                                                        <i class="icon-attachment"></i>&nbsp;
                                                                       مرفقات
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>



                                                            <asp:TemplateColumn HeaderText="التفاصيل ">
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="left" />

                                                                <ItemTemplate>
                                                                    <a href="MedalsData.aspx?id=<%#Eval("code") %>&editflag=1" class="btn btn-default btn-xs">
                                                                        <i class="fa fa-file"></i>&nbsp;
                                                التفاصيل
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                        
                                                            <asp:TemplateColumn>
                                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                            <%--<asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('are you sure you want to delete selected items?');" CommandName="delete" runat="server">  <i class="fa fa-trash" style="color:##333"></i>&nbsp;</asp:LinkButton>--%>
                                                          <asp:LinkButton
                                                                           ID="lnkDelete"
                                                                           OnClientClick="return DeleteConfirm.show(this);"
                                                                           CommandName="delete"
                                                                           runat="server">
                                                                           <i class="fa fa-trash" style="color:#333"></i>&nbsp;
                                                           </asp:LinkButton>
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
                                                        <%-- <a class="paginate_button previous disabled" aria-controls="DataTables_Table_3" data-dt-idx="0" tabindex="0" id="DataTables_Table_3_previous">→</a>
                                                            <span><a class="paginate_button current" aria-controls="DataTables_Table_3" data-dt-idx="1" tabindex="0">1</a>
                                                                <a class="paginate_button " aria-controls="DataTables_Table_3" data-dt-idx="2" tabindex="0">2</a>


                                                            </span>
                                                            <a class="paginate_button next" aria-controls="DataTables_Table_3" data-dt-idx="3" tabindex="0" id="DataTables_Table_3_next">←</a>--%>


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

        <div class="panel" id="personDiv" runat="server" visible="false">

            <div class="panel-body">
                <div class="row">

                    <div class="panel-body">
                        <div class="form-horizontal">

                            <div class="col-lg-12">
                                <div class="portlet box">
                                    <div class="portlet-header">

                                        <div class="portlet-body">
                                                                                              <div class="datatable-scroll">
                                                     <asp:DataGrid ID="PersonsAll" runat="server" Visible="false"
                         DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="20" class="table datatable-basic dataTable no-footer" Width="100%" OnPageIndexChanged="PersonsAll_PageIndexChanged">

    <SelectedItemStyle ForeColor="White" />
 <ItemStyle CssClass="grdItem" />
 <AlternatingItemStyle CssClass="grdItem" />

 <HeaderStyle BackColor="Black" ForeColor="White" Font-Bold="False" />

   <%-- <HeaderStyle CssClass="grdHead" BackColor="#ece0a4" ForeColor="Black" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />--%>
    <FooterStyle CssClass="grdFoot" />
    <PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
        PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
    <Columns>
        <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="م">
            <HeaderStyle HorizontalAlign="center" Width="30px" />
            <ItemStyle HorizontalAlign="center" Width="30px" />
            <ItemTemplate>
                <%# (Container.ItemIndex + 1).ToString() %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="نوع  الوسام">

            <HeaderStyle HorizontalAlign="right" />
            <ItemTemplate>
                <%#Eval("Medal_M_TypesNameAr") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="Person_NameEn" HeaderText="الاسم"></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="  الرتبه">

            <HeaderStyle HorizontalAlign="right" />
            <ItemTemplate>
                <%#Eval("NameAr") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="CivilID" HeaderText="الرقم المدني" Visible="false"></asp:BoundColumn>
        <asp:BoundColumn DataField="FileNum" HeaderText="رقم الوارد "></asp:BoundColumn>

        <asp:BoundColumn DataField="OrgNameAr" HeaderText="الجهة"></asp:BoundColumn>
        <asp:BoundColumn DataField="TransDate" HeaderText=" التاريخ "></asp:BoundColumn>

        <%--<asp:TemplateColumn HeaderText="ملاحظات">

            <HeaderStyle HorizontalAlign="right" />
            <ItemTemplate>
                <%#Eval("GrantReasonText") %>
            </ItemTemplate>
        </asp:TemplateColumn>--%>


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
    <div class="row mbl">

        <div class="panel-heading" id="tblAdd" runat="server" visible="false">

            <div class="row">
                <asp:Label runat="server" ID="lblAdderror"></asp:Label>
                <div class="panel">

                    <div class="panel-body">

                        <div class="tabbable">
                            <ul class="nav nav-tabs nav-tabs-highlight">
                                <li class="<%=activeTab(1) %>" onclick="setactiveTab(1)"><a href="#badges-tab1" data-toggle="tab"><i class="icon-menu7 position-left"></i>بيانات الأنواط والأوسمة </a></li>
                                <li class="<%=activeTab(2) %>" onclick="setactiveTab(2)"><a href="#badges-tab2" data-toggle="tab">المرفقات <span class="badge badge-success  position-right">
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
                                                      <input id="hdnactivetab" runat="server" type="hidden" />
                                                    <fieldset class="content-group">
                                                        <legend class="text-semibold">
                                                            <i class="icon-file-text2 position-left"></i>
                                                            بيانات الأنواط والأوسمة
										 
                                                        </legend>

                                                        <div class="row">

                                                            <div class="col-md-4">
                                                                   <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">الجهة</label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstorg" runat="server" class="Select2Drop" AutoPostBack="true" OnSelectedIndexChanged="lstorg_SelectedIndexChanged"></asp:DropDownList>

                                                                    </div>
                                                                </div>
                                                                <div class="form-group" runat="server" id="divMedalCategory" visible="false">
                                                                    <label class="col-md-3 control-label" for="">التصنيف</label>

                                                                    <div class="col-md-9">

                                                                        <asp:RadioButtonList ID="lstMedalCat" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="lstMedalCat_SelectedIndexChanged"  >
                                                                            <asp:ListItem Value="1" Text="داخلي" Selected="True"> </asp:ListItem>
                                                                            <asp:ListItem Value="2" Text="حارجي"> </asp:ListItem>

                                                                        </asp:RadioButtonList>

                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">رقم وارد الجهة</label>

                                                                    <div class="col-md-9">

                                                                        <div class="form-group">


                                                                            <div class="col-md-3">

                                                                                <asp:TextBox ID="txtSerialNUm" class="form-control" placeholder="رقم" runat="server"></asp:TextBox>

                                                                            </div>

                                                                            <div class="col-md-6">
                                                                                <asp:TextBox ID="txtserialYear" class="form-control" placeholder="سنة" runat="server"></asp:TextBox>

                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">الموضوع</label>

                                                                    <div class="col-md-9">

                                                                        <asp:TextBox runat="server" ID="txtMedalNotes" class="form-control"></asp:TextBox>

                                                                    </div>
                                                                </div>




                                                            </div>
                                                            <div class="col-md-4">

                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">التاريخ </label>

                                                                    <div class="col-md-9">




                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>

                                                                            <asp:TextBox ID="txtMedalDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

                                                                        </div>

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

                                                             
                                                            </div>

                                                        </div>

                                                        <div class="row">
                                                        </div>

                                                        <fieldset class="content-group">
                                                            <legend class="text-semibold">
                                                                <i class="icon-file-text2 position-left"></i>
                                                                بيانات  الشخصيات
                                                                <asp:Label ID="lblPersonCount" runat="server" ></asp:Label>
                                                            </legend>

                                                            <div class="col-md-5 col-md-offset-3 pull-left" runat="server" id="UploadPersonsList">

                                                                <div class="form-group">
                                                                    <label class="col-md-2 control-label">إختر الملف  </label>

                                                                    <div class="col-md-6">
                                                                        <asp:FileUpload ID="txtUploadPersons" runat="server" class="file-styled" />
                                                                        <span class="help-block">Accepted formats: xls,xlsx</span>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:LinkButton runat="server" ID="lnkUploadPersons" class="btn btn-primary btn-labeled " OnClientClick="return validateFileUpladed();" OnClick="lnkUploadPersons_Click"><b><i class="icon-users"></i></b>إضافة قائمة الاشخاص  </asp:LinkButton>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                                <div class="row" style="padding-top: 10px">
                                                               <div class="col-md-12">
                                                                   <div class="form-actions">
                                                                       <div class="col-md-offset-6 col-md-12">


                                                                           <asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary" OnClick="btnSave_Click1"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>



                                                                           &nbsp;
                                                                          <asp:Button runat="server" ID="btnCancel" class="btn btn-default" Text=" الغاء / رجوع  " OnClick="btnCancel_Click" />
                                                                           &nbsp;
                                                                        <a id="anchorAttachment" visible="false" runat="server" href='#' class="btn btn-success btn-xs iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                           مرفقات
                                                                        </a>

                                                                             &nbsp;
                                                                                        <a href="#" class="btn btn-primary btn-labeled iframe linkedpopup" id="lnkAddRelatedDoc" runat="server"><b><i class="glyphicon glyphicon-link"></i></b>إضافة تشريع مرتبط </a>

                                                                           &nbsp;
                                                                                        <a href="#" class="btn btn-primary btn-labeled" id="lnkViewRelatedDoc" runat="server"><b><i class="glyphicon glyphicon-link"></i></b>عرض التشريع المرتبط   </a>


                                                                       </div>
                                                                   </div>
                                                               </div>

                                                           </div>
                                                        </fieldset>


                                                            <asp:UpdatePanel runat="server" ID="Updatepanel2" ChildrenAsTriggers="true" UpdateMode="conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="pagerPersons" EventName="Command" />
                                                                </Triggers>
                                                                <ContentTemplate>


                                                                    <asp:Button UseSubmitBehavior="false" runat="server" ID="btnAddNewItem" Text="Add Item" Style="display: none;" OnClick="btnAddNewItem_Click" />

                                                                    <asp:DataGrid ID="grdPersons" runat="server"
                                                                        class="table table-hover table-striped table-bordered table-advanced tablesorter"
                                                                        DataKeyField="Code" AllowPaging="True" AutoGenerateColumns="False" PageSize="20"
                                                                        BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                                                                        CellPadding="3" Width="100%" OnItemDataBound="grdPersons_ItemDataBound" OnItemCommand="grdPersons_ItemCommand">
                                                                        <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                        <ItemStyle CssClass="grdItem" />
                                                                        <AlternatingItemStyle CssClass="grdItem" />
                                                                        <HeaderStyle CssClass="grdHead" BackColor="Gray" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                        <FooterStyle CssClass="grdFoot" />
                                                                        <PagerStyle Visible="false" />
                                                                        <Columns>

                                                                            <asp:TemplateColumn HeaderText="">
                                                                                <HeaderStyle BackColor="#000000"></HeaderStyle>
                                                                                <ItemStyle Width="1%" HorizontalAlign="center" />
                                                                                <FooterStyle HorizontalAlign="center" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="lnkEdit" CssClass="btn btn-default btn-xs" CommandName="Edit"> <img src="/Layout/RTL/assets/images/EditPerson.png" /></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <table border="0" align="center">
                                                                                        <tr>
                                                                                            <td style="border: solid 0px #FFFFFF;">
                                                                                                <asp:LinkButton runat="server" ID="lnkAdd" CssClass="btn btn-default btn-xs" Visible="false" CommandName="AddNew"><img src="/Layout/RTL/assets/images/addPerson.png" /></asp:LinkButton></td>
                                                                                            <td style="border: solid 0px #FFFFFF;">
                                                                                                <asp:LinkButton runat="server" ID="lnkUpdate" CssClass="btn btn-default btn-xs" CommandName="Update">تحديث</asp:LinkButton>
                                                                                            </td>
                                                                                            <td style="border: solid 0px #FFFFFF;">
                                                                                                <asp:LinkButton runat="server" ID="lnkCancel" CssClass="btn btn-default btn-xs" CommandName="Cancel">الغاء</asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:Button UseSubmitBehavior="false" runat="server" ID="btnUpdateItem" CommandName="Update" Style="display: none;" />
                                                                                    <asp:Button UseSubmitBehavior="false" runat="server" ID="btnCancelItem" CommandName="Cancel" Style="display: none;" />
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText="">
                                                                                <HeaderStyle BackColor="#000000"></HeaderStyle>
                                                                                <ItemStyle Width="1%" HorizontalAlign="center" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="lnkDelete" OnClientClick="return checkDeleteone();" CssClass="btn btn-default btn-xs" CommandName="Delete"><img src="/Layout/RTL/assets/images/DeletePerson.png" /></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    &nbsp;
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateColumn>



                                                                            <asp:BoundColumn DataField="code" HeaderText="#" Visible="false">
                                                                                <ItemStyle Width="2px" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="MedalType" HeaderText="MedalType" Visible="false"></asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="MedalTypeCatID" HeaderText="MedalTypeCatID" Visible="false"></asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="GrantReason" HeaderText="GrantReason" Visible="false"></asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="GradeID" HeaderText="GradeID" Visible="false"></asp:BoundColumn>

                                                                            <asp:TemplateColumn HeaderText="م.">
                                                                                <ItemStyle HorizontalAlign="center" Width="2px" />
                                                                                <ItemTemplate>
                                                                                    <%# !IsPlaceholderRow(Container.DataItem) ? GetRowNumber(Container.ItemIndex).ToString() : "" %>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>

                                                                            <asp:TemplateColumn HeaderText="نوع  الوسام">
                                                                                <HeaderStyle Wrap="false" />
                                                                                <ItemStyle Width="20%" />
                                                                                <ItemTemplate>
                                                                                    <%#Eval("Medal_M_Types.NameAr") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:DropDownList ID="lstMedalType" class="Select2Drop" runat="server"></asp:DropDownList>
                                                                                    <input id="hdnMedalType" value='<%#Eval("MedalType") %>' runat="server" type="hidden" />
                                                                                </EditItemTemplate>

                                                                            </asp:TemplateColumn>

                                                                            <asp:TemplateColumn HeaderText="  الاسم">

                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate>
                                                                                    <%#Eval("Person_NameAr") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtname" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                                                    <input id="hdnPerson_NameAr" value='<%#Eval("Person_NameAr") %>' runat="server" type="hidden" />
                                                                                       <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
                                                                                        runat="server" TargetControlID="txtname"
                                                                                        CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1" 
                                                                                        ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="MedalPersonsAutoCompete" />

                                                                                </EditItemTemplate>

                                                                            </asp:TemplateColumn>
                                                                         
                                                                           
                                                                            <asp:TemplateColumn HeaderText="  الرتبه">

                                                                                <HeaderStyle HorizontalAlign="right" />

                                                                                <ItemTemplate>
                                                                                    <%#Eval("Medal_M_jobGrade.NameAr") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:DropDownList ID="lstJobGrade" class="Select2Drop" Width="100%" runat="server"></asp:DropDownList>

                                                                                    <input id="hdnGradeID" value='<%#Eval("GradeID") %>' runat="server" type="hidden" />

                                                                                </EditItemTemplate>

                                                                            </asp:TemplateColumn>

                                                                            <asp:TemplateColumn HeaderText="  الرقم المدني" Visible="false">
                                                                                <ItemStyle Width="15%" />
                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate>
                                                                                    <%#Eval("CivilID") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtCivilID" CssClass="form-control" MaxLength="12" Width="100%" runat="server"></asp:TextBox>

                                                                                    <input id="hdnCivilID" value='<%#Eval("CivilID") %>' runat="server" type="hidden" />
                                                                                </EditItemTemplate>

                                                                            </asp:TemplateColumn>

                                                                             <asp:TemplateColumn HeaderText="  الرقم العسكري">
                                                                                <ItemStyle Width="15%" />
                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate>
                                                                                    <%#Eval("MilitaryNum") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtMilitaryNum" CssClass="form-control" MaxLength="12" Width="100%" runat="server"></asp:TextBox>

                                                                                    <input id="hdnMilitaryNum" value='<%#Eval("MilitaryNum") %>' runat="server" type="hidden" />
                                                                                </EditItemTemplate>

                                                                            </asp:TemplateColumn>

                                                                             <asp:TemplateColumn HeaderText="الدولة">
                                                                                <HeaderStyle Wrap="false" />
                                                                                <ItemStyle Width="20%" />
                                                                                <ItemTemplate>
                                                                                    <%#Eval("CountryName") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:DropDownList ID="lstCountryId" class="Select2Drop" runat="server"></asp:DropDownList>
                                                                                    <input id="hdnCountryId" value='<%#Eval("CountryId") %>' runat="server" type="hidden" />
                                                                                </EditItemTemplate>

                                                                            </asp:TemplateColumn>


                                                                            <asp:TemplateColumn HeaderText="المسمى">

                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate><%#Eval("PersonTitle") %></ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtPersonTitle" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                                                    <input id="hdnPersonTitle" value='<%#Eval("PersonTitle") %>' runat="server" type="hidden" />

                                                                                </EditItemTemplate>


                                                                            </asp:TemplateColumn>

                                                                            <asp:TemplateColumn HeaderText="بيانات إضافية">

                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate><%#Eval("AdditionalData") %></ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtAdditionalData" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                                                    <input id="hdnAdditionalData" value='<%#Eval("AdditionalData") %>' runat="server" type="hidden" />

                                                                                </EditItemTemplate>


                                                                            </asp:TemplateColumn>

                                                                            <asp:TemplateColumn HeaderText="ملاحظات">

                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate><%#Eval("GrantReasonText") %></ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:DropDownList ID="lstGrantreasons" CssClass="form-control" Visible="false" Width="100%" runat="server"></asp:DropDownList>
                                                                                    <asp:TextBox ID="txtGrantreasons" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                                                    <input id="hdnGrantReasonText" value='<%#Eval("GrantReasonText") %>' runat="server" type="hidden" />

                                                                                </EditItemTemplate>


                                                                            </asp:TemplateColumn>

                                                                                     </Columns>
                                                                                </asp:DataGrid>

                                                                                <!-- Pagination Footer for grdPersons -->
                                                                                <div class="row mbm">
                                                                                    <div class="col-lg-12">
                                                                                        <div class="pagination-panel">
                                                                                        </div>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="datatable-footer">
                                                                                    <div class="dataTables_info" role="status" aria-live="polite">
                                                                                        <asp:Label ID="lblPersonsCount" runat="server"></asp:Label>
                                                                                    </div>
                                                                                    <div class="dataTables_paginate paging_simple_numbers">
                                                                                        <cc1:Pager CurrentIndex="1" OnCommand="pagerPersons_Command" ShowFirstLast="False" ID="pagerPersons"
                                                                                            runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>
                                                                                    </div>
                                                                                </div>

                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
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

                                               

                                                <div id="divProAttache" runat="server">
                                                    
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label" for="">نوع المرفق</label>

                                                        <div class="col-md-9">
                                                            <asp:DropDownList ID="lstAttachmentType" class="Select2Drop" runat="server"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                       <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">تاريخ الوثيقة</label>

                                                    <div class="col-md-9">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                            <%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
                                                            <asp:TextBox ID="txtCreationDate" runat="server" class="form-control pickadate-selectors"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>


                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label">المرجع</label>

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
                                                        <asp:LinkButton ID="lnkSaveProcesure" runat="server" class="btn btn-primary" OnClick="lnkSaveProcesure_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

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


                                                        <asp:DataGrid runat="server" ID="grdAttachemnt" AutoGenerateColumns="False"
                                            AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemDataBound="grdAttachemnt_ItemDataBound" OnEditCommand="grdAttachemnt_EditCommand">
                                            <PagerStyle Visible="False" />
                                            <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                            <Columns>
                                                <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>

                                                <asp:TemplateColumn HeaderText="نوع المرفق">

                                                    <HeaderStyle HorizontalAlign="right" />
                                                    <ItemTemplate>
                                                        <%#Eval("Medal_M_AttachmentTypes.NameAr") %>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="ReceiveDate" HeaderText="تاريخ المرفق" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="UploadDate" HeaderText="تاريخ الرفع" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>

                                                 
                                                <asp:BoundColumn DataField="AttachRef" HeaderText="مرجع"></asp:BoundColumn>

 <asp:TemplateColumn HeaderText="عرض المرفق">
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>

                                                        <a style="<%#showHideAttachment(gets(Eval("Filepath")))%>" target="_blank" href="<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath +"&vfileList=[" +gets(Eval("Filepath"))+";]" %>">عرض المرفق

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
                                                           

                                                            <cc1:Pager CurrentIndex="1" OnCommand="pager_Command" ShowFirstLast="False" ID="pager2"
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

       <uc:DeleteConfirm runat="server" />

    <script type="text/javascript">
        // Initialize Select2 for dropdown lists
        function initializeSelect2() {
            $('.Select2Drop').select2({
                language: 'ar',
                allowClear: true,
                dir: 'rtl',
                minimumInputLength: 0
            });
        }

        // Initialize on page load
        $(document).ready(function () {
            initializeSelect2();
        });

        // Re-initialize after partial postback
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function () {
                initializeSelect2();
            });
        }
    </script>
</asp:Content>
