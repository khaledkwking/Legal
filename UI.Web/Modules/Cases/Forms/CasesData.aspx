<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="CasesData.aspx.cs" Inherits="UI.Web.Modules.Cases.Forms.CasesData" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
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



        function ValidateHeading() {
            var txt = document.getElementById("<%=chkHasHearing.ClientID %>")
            if (txt.checked) {
                document.getElementById("hearingdatecontainer").style.display = '';
            } else {
                document.getElementById("hearingdatecontainer").style.display = 'none';
            }


        }
        function ControlGrid(imgName, rowIndex, rowID) {
            //alert("CONTROL GRID");
            // alert(imgName);
            // alert(rowIndex);
            //alert(rowID);
            rowIndex = rowIndex + 3;

            var myrow = "";
            if (rowIndex < 10)
                myrow = "ctl00_Main_grdCasesList_ctl0" + rowIndex;
            else
                myrow = "ctl00_Main_grdCasesList_ctl" + rowIndex;
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
        function ValidateHearing() {
            var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات القضية اولا ");
                return false;
            }

            var txt = document.getElementById("<%=txtHearingDate.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل تاريخ الجلسة ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txthearingConsultant.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل اسم القاضي");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtHearingWriter.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل اسم كاتب الجلسة");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtHearingDecisionText.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل نص الحكم");
                txt.focus();
                return false;
            }

            return true


        }

        function ValidateProcesdureadd() {
            var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات القضية اولا ");
                txt.focus();
                return false;
            }
            return true


        }

        function chkImage() {

            var txt = document.getElementById("<%=txtFileNotes.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  الموضوع ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtFileInternalserial.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  مسلسل الملف");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtSerialYear.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل السنة");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtFileCreationDate.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،  اختر تاريخ الملف ");
                txt.focus();
                return false;
            }


            var txt = document.getElementById("<%=lstassignedPersons.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر الموظف المختص");
                txt.focus();
                return false;
            }


            var txt = document.getElementById("<%=txtFileAutoNumber.ClientID %>")
            var chkhasautoNumber = document.getElementById("<%=chkhasautoNumber.ClientID %>")

            if (txt.value == "") {
                if (!chkhasautoNumber.checked) {
                    new $.Zebra_Dialog("فضلا ،  ادخل الرقم الإلى ");
                    txt.focus();
                    return false;
                }

            }


            var txt = document.getElementById("<%=lstCategory.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر التصنيف  ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=lstCaseType.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر الدائره  ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtSuitType.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل  رقم القضية");
                txt.focus();
                return false;
            }

           <%-- var txt = document.getElementById("<%=txtCaseDate.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  تاريخ ايداع القضية فى المحكمة  ");
                txt.focus();
                return false;
            }--%>
            var txt = document.getElementById("<%=lstCaseLevel.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر الدرجه ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=lstcaseStatus.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر الحالة ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=lstDession.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر الحكم ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=lstJudgmentresult.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر نتيجة الحكم ");
                txt.focus();
                return false;
            }


            return true;
        }

        function hasAutoNumber() {
            var txt = document.getElementById("<%=txtFileAutoNumber.ClientID %>");
            var chkhasautoNumber = document.getElementById("<%=chkhasautoNumber.ClientID %>");

            if (chkhasautoNumber.checked) {
                txt.value = "000000000000";
                txt.disabled = true;
            } else {
                txt.value = "";
                txt.disabled = false;
            }
        }

        function LinkAddClick() {
            // alert("in");

            // return InsertItem();
        }

        function LinkAddClickgrdprosecutor() {
            // alert("in");

            // return InsertItem();
        }



        function InsertItem() {


            var txtname = getObjById("txtname").value;

            if (txtname == "") {
                new $.Zebra_Dialog("فضلا ،ادخل الاسم ");
                return false;
            }


        }

        function ValidateIncoming() {

            var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات القضية اولا ");
                return false;
            }

            var txt = document.getElementById("<%=txtDoc_Serial.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل رقم الوثيقة     ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtFrom.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل وارد من ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtDoc_Subject.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  الموضوع ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtComingDate.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  تاريخ الوثيقه ");
                txt.focus();
                return false;
            }

        }


        function Validateoutgoing() {

            var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات القضية اولا ");
                return false;
            }
            var txt = document.getElementById("<%=txtOutDocNo.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل رقم الوثيقة ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtto.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل صادر إلى ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtOutSubject.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  الموضوع ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=txtoutDate.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  تاريخ الوثيقه ");
                txt.focus();
                return false;
            }

        }
        function validateFileAttachment() {
            var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات القضية اولا ");
                return false;
            }

            var txt = document.getElementById("<%=lstAttachmentType.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، اختر نوع المرفق ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtAttachCreationDate.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل تاريخ الوثيقة  ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtAttachRef.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  المرجع ");
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


        function showscannerLoading() {

            if (validateFileAttachment()) {
                document.getElementById("scanLoading").style.display = "";
                return true;
            } else { return false; }
        }


    </script>




    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        القضايا
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

                    <asp:LinkButton runat="server" ID="btnNew" class="btn btn-success btn-xs" OnClick="btnNew_Click1"><i class="fa fa-plus"></i>&nbsp; إضافة ملف جديد&nbsp;</asp:LinkButton>

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




                            <div class="col-md-6">

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        رقم المسلسل  :

                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterInternalSerial" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <label class="col-lg-2 control-label">
                                        السنة  :

                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterFileYear" runat="server" class="form-control"></asp:TextBox>
                                    </div>


                                </div>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">تاريخ  ايداع القضية فى المحكمة من :</label>
                                    <div class="col-lg-3">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
                                            <asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                        </div>
                                    </div>
                                    <label class="col-lg-2 control-label">إلى :</label>

                                    <div class="col-lg-3">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <asp:TextBox ID="txtFilterDateTo" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>

                                        </div>
                                    </div>

                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label" for="">تصنيف القضايا     : </label>



                                    <div class="col-md-9">
                                        <asp:DropDownList ID="lstfiltermainType" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>


                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label" for="">رقم القضية : </label>

                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtFSuitNum" class="form-control" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtFSuitYear" class="form-control" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="lstFiltertype" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">

                                        <asp:TextBox ID="txtFSuitType" class="form-control" runat="server"></asp:TextBox>

                                    </div>
                                </div>


                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        الموضوع :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:TextBox ID="txtFilterSubject" runat="server" class="form-control"></asp:TextBox>

                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1"
                                            runat="server" TargetControlID="txtFilterSubject"
                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />

                                    </div>



                                </div>




                            </div>

                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الدرجه:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFCaseLevel" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <%--
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">المحكمه:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterCourt" class="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>--%>

                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        المدعي علية   :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <%--  <asp:TextBox ID="txtFilterPersonName" runat="server" class="form-control"></asp:TextBox>--%>
                                        <asp:DropDownList ID="lstFilterPersonName" class="Select2Drop" runat="server"></asp:DropDownList>

                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        الرقم الإلى:
                                    </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="txtFilterFileNUm" runat="server" class="form-control"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="form-group" style='display: <%=ViewPrivateParty()%>'>
                                    <label class="col-lg-3 control-label">
                                        المدعي     :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:DropDownList ID="lstFiltercaseParty" class="Select2Drop" runat="server"></asp:DropDownList>

                                    </div>
                                </div>

                            </div>

                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الحكم :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterDession" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الحالة :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterStatus" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الموظف المختص :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterAssignedPerson" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label" for="" style="text-align: left">نتيجة الحكم: </label>

                                    <div class="col-md-9">

                                        <asp:DropDownList ID="lstfilterJudgmentresult" runat="server" class="Select2Drop">
                                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="لصالح المدعي  " Value="1"></asp:ListItem>
                                            <asp:ListItem Text="لصالح المدعي عليه" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="لم يصدر حكم" Value="3"></asp:ListItem>
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
                                                        <asp:Label ID="lblcount3" runat="server"></asp:Label>)</span>

                                                    <asp:LinkButton ID="lnkSearchback" CssClass="control-arrow" runat="server" OnClick="lnkSearchback_Click"> رجوع  <i class=" icon-backward2"></i></asp:LinkButton>

                                                </legend>




                                                <div class="datatable-scroll">


                                                    <asp:DataGrid ID="grdCasesList" runat="server"
                                                        DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="40" class="table datatable-basic dataTable no-footer"
                                                        Width="100%" OnItemDataBound="grdCasesList_ItemDataBound" OnItemCommand="grdCasesList_ItemCommand">
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

                                                                    <div style="padding-right: 30px">
                                                                        <asp:Label Font-Bold="true" runat="server" ID="Label7" CssClass="black_Lable">
				                          القضايا
                                                                        </asp:Label>


                                                                        <asp:DataGrid ID="grdCases" runat="server"
                                                                            class="table table-hover table-striped table-bordered table-advanced tablesorter"
                                                                            AutoGenerateColumns="False"
                                                                            BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                                                                            CellPadding="3" Width="100%" OnItemDataBound="grdUnits_ItemDataBound" OnItemCommand="grdCases_ItemCommand">
                                                                            <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                            <ItemStyle CssClass="grdItem" />
                                                                            <AlternatingItemStyle CssClass="grdItem" />
                                                                            <HeaderStyle CssClass="grdHead" BackColor="Gray" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                            <FooterStyle CssClass="grdFoot" />
                                                                            <PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
                                                                                PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="CaseID" Visible="False"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CasesMainTypeAr" HeaderText="تصنيف القضية  "></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="CaseSerial" HeaderText="رقم القضية"></asp:BoundColumn>
                                                                                <%--<asp:BoundColumn DataField="CaseInternalSerial" HeaderText="الرقم الإلى  "></asp:BoundColumn>--%>
                                                                                <asp:BoundColumn DataField="CaseTransDate" HeaderText="تاريخ ايداع القضية فى المحكمة " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="LastActionDate" HeaderText="اخر تحديث  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>

                                                                                <asp:TemplateColumn HeaderText="الدائره">


                                                                                    <ItemTemplate>

                                                                                        <%#Eval("CasesTypeAr")%>/<%#Eval("SuitType")  %>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>

                                                                                <asp:BoundColumn DataField="CasesTypeAr" HeaderText="الدائره"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="LitigationDegreeAr" HeaderText="الدرجه"></asp:BoundColumn>
                                                                                <%--<asp:BoundColumn DataField="CaseSubject" HeaderText="الموضوع"></asp:BoundColumn>--%>
                                                                                <asp:BoundColumn DataField="CaseStatusAr" HeaderText="الحاله"></asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="DecisionNameAr" HeaderText="الحكم"></asp:BoundColumn>

                                                                                <asp:TemplateColumn HeaderText="نتيجة الحكم ">
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>

                                                                                        <%#gets(Eval("Judgmentresult")) %>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>

                                                                                <asp:TemplateColumn HeaderText="مرفقات القضية">
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>

                                                                                        <a href="CaseAttachments.aspx?DocID=0&CaseID=<%#Eval("CaseID")%>&FileID=0" class="btn btn-default btn-xs iframe">
                                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                                                             مرفقات القضية <b>[<%#Eval("attachmentCount")%>]</b>
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>

                                                                                <asp:TemplateColumn HeaderText="التفاصيل">
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>

                                                                                        <a href="CasesData.aspx?FileID=<%#Eval("FileNumID") %>&CaseID=<%#Eval("CaseID") %>" class="btn btn-default btn-xs">
                                                                                            <i class="fa fa-file"></i>&nbsp;
                                                التفاصيل
                                                                                        </a>


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
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="التفاصيل">
                                                                <ItemStyle HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    <img style="cursor: pointer;" src="/layout/images/plus.gif" alt="" border="0" runat="server" id="imgControl" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="code"></asp:BoundColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="file_Serial" DataField="file_Serial"></asp:BoundColumn>

                                                            <asp:BoundColumn DataField="FileNote" HeaderText="الموضوع">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="  رقم الملف ">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />

                                                                <ItemTemplate>
                                                                    <%#Eval("FileInternalSerial") %>/  <%#Eval("fileSerialyear") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="FileAutoNumber" HeaderText="الرقم الإلى">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <%--  <asp:BoundColumn DataField="file_Serial" HeaderText="الرقم الإلى ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:BoundColumn DataField="TransDate" HeaderText="تاريخ ورود القضيه للأمانة    " DataFormatString="{0:dd/MM/yyyy}">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="LastTransctionDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="تاريخ اخر تحديث ">
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="ملف خاص ">
                                                                <ItemStyle HorizontalAlign="left" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="left" />

                                                                <ItemTemplate>
                                                                    <%#ShowYesNo(getBool(Eval("isPrivate"))) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                            <asp:TemplateColumn HeaderText="مرفقات" Visible="false">
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <a href="CaseAttachments.aspx?DocID=0&CaseID=0&FileID=<%#Eval("Code") %>" class="btn btn-default btn-xs iframe">
                                                                        <i class="icon-attachment"></i>&nbsp;
                                                                       مرفقات
                                                                    </a>
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

        <div class="panel" id="tblshow2" runat="server" visible="false">

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
                                                    نتيجة البحث



                                                </legend>




                                                <div class="datatable-scroll">


                                                    <asp:DataGrid ID="grdCasesFilterResult" runat="server"
                                                        class="table table-hover table-striped table-bordered table-advanced tablesorter"
                                                        AutoGenerateColumns="False" AllowPaging="true" PageSize="20"
                                                        BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                                                        CellPadding="3" Width="100%" OnItemDataBound="grdUnits_ItemDataBound" OnItemCommand="grdCases_ItemCommand">
                                                        <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                        <ItemStyle CssClass="grdItem" />
                                                        <AlternatingItemStyle CssClass="grdItem" />
                                                        <HeaderStyle CssClass="grdHead" BackColor="Black" ForeColor="White" Font-Bold="False" />
                                                        <FooterStyle CssClass="grdFoot" />
                                                        <PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev" Visible="false"
                                                            PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
                                                        <Columns>
                                                            <asp:BoundColumn DataField="CaseID" Visible="False"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CaseSerial" HeaderText="رقم القضية"></asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="CaseInternalSerial" HeaderText="الرقم الإلى  "></asp:BoundColumn>--%>
                                                            <asp:BoundColumn DataField="CaseTransDate" HeaderText="تاريخ ايداع القضية فى المحكمة " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="LastActionDate" HeaderText="اخر تحديث  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="الدائره">


                                                                <ItemTemplate>

                                                                    <%#Eval("CasesTypeAr")%>/<%#Eval("SuitType")  %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                            <asp:BoundColumn DataField="CasesTypeAr" HeaderText="الدائره"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="LitigationDegreeAr" HeaderText="الدرجه"></asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="CaseSubject" HeaderText="الموضوع"></asp:BoundColumn>--%>
                                                            <asp:BoundColumn DataField="CaseStatusAr" HeaderText="الحاله"></asp:BoundColumn>

                                                            <asp:BoundColumn DataField="DecisionNameAr" HeaderText="الحكم"></asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="نتيجة الحكم ">
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>

                                                                    <%#ShowJudgmentresult(gets(Eval("Judgmentresult"))) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                            <asp:TemplateColumn HeaderText="مرفقات القضية">
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>

                                                                    <a href="CaseAttachments.aspx?DocID=0&CaseID=<%#Eval("CaseID")%>&FileID=0" class="btn btn-default btn-xs iframe">
                                                                        <i class="icon-attachment"></i>&nbsp;
                                                                                                             مرفقات القضية <b>[<%#Eval("attachmentCount")%>]</b>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                            <asp:TemplateColumn HeaderText="التفاصيل">
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>

                                                                    <a href="CasesData.aspx?FileID=<%#Eval("FileNumID") %>&CaseID=<%#Eval("CaseID") %>" class="btn btn-default btn-xs">
                                                                        <i class="fa fa-file"></i>&nbsp;
                                                التفاصيل
                                                                    </a>


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
                                                        <asp:Label ID="lblcount2" runat="server"></asp:Label>
                                                    </div>
                                                    <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">


                                                        <cc1:Pager CurrentIndex="1" OnCommand="grdCasesFilterResult_pager_Command" ShowFirstLast="False" ID="pager6"
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
                                <li class="<%=activeTab(1) %>" onclick="setactiveTab(1)"><a href="#badges-tab1" data-toggle="tab"><i class="icon-menu7 position-left"></i>بيانات القضية</a></li>
                                <li class="<%=activeTab(2) %>" onclick="setactiveTab(2)"><a href="#badges-tab2" data-toggle="tab">الأحكام <span class="badge badge-success  position-right">
                                    <asp:Label ID="lblHearingcount" runat="server" Text="0"></asp:Label></span></a></li>
                                <li class="<%=activeTab(3) %>" onclick="setactiveTab(3)"><a href="#badges-tab3" data-toggle="tab">الوارد <span class="badge badge-success  position-right">
                                    <asp:Label ID="lblComingalert" runat="server" Text="0"></asp:Label></span></a></li>
                                <li class="<%=activeTab(4) %>" onclick="setactiveTab(4)"><a href="#badges-tab4" data-toggle="tab">الصادر <span class="badge badge-success  position-right">
                                    <asp:Label ID="lbloutAlert" runat="server" Text="0"></asp:Label></span></a></li>
                                <li class="<%=activeTab(5) %>" onclick="setactiveTab(5)" style="display: none"><a href="#badges-tab5" data-toggle="tab">مرفقات الملف <i class="icon-attachment position-left"></i></a></li>

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
                                                    <input id="hdnHearingScannerfilepath" runat="server" type="hidden" />
                                                    <input id="hdnIncomingScannerfilepath" runat="server" type="hidden" />
                                                    <input id="hdnoutgiongScannerfilepath" runat="server" type="hidden" />

                                                    <fieldset class="content-group">
                                                        <legend class="text-semibold">
                                                            <i class="icon-file-text2 position-left"></i>
                                                            بيانات  الملف

                                                        </legend>
                                                        <div style="background: #efefef; padding: 10px">
                                                            <div class="row">

                                                                <div class="col-md-8">
                                                                    <div class="form-group">
                                                                        <label class="col-md-2 control-label" for="">موضوع القضية :  <span class="text-danger">*</span></label>

                                                                        <div class="col-md-10" style="z-index: 99;">
                                                                            <asp:TextBox runat="server" ID="txtFileNotes" class="form-control"></asp:TextBox>
                                                                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender5"
                                                                                runat="server" TargetControlID="txtFileNotes"
                                                                                CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                                                ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">

                                                                <div class="col-md-4">
                                                                    <div class="form-group" style="display: none">
                                                                        <label class="col-md-3 control-label" for="">الرقم الإلى : <span class="text-danger">*</span></label>

                                                                        <div class="col-md-9">

                                                                            <asp:TextBox ID="txtfilnum" class="form-control" runat="server"></asp:TextBox>

                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label class="col-md-4 control-label" for="">مسلسل :<span class="text-danger">*</span> </label>
                                                                        <div class="col-md-3">

                                                                            <asp:TextBox ID="txtFileInternalserial" class="form-control" runat="server"></asp:TextBox>

                                                                        </div>
                                                                        <label class="col-md-2 control-label" for="" style="text-align: left">السنة:<span class="text-danger">*</span> </label>
                                                                        <div class="col-md-3">

                                                                            <asp:TextBox ID="txtSerialYear" class="form-control" runat="server"></asp:TextBox>

                                                                        </div>

                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label class="col-md-4 control-label" for="">الموظف المختص :<span class="text-danger">*</span> </label>

                                                                        <div class="col-md-8">

                                                                            <asp:DropDownList ID="lstassignedPersons" runat="server" class="Select2Drop"></asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">

                                                                    <div class="form-group">
                                                                        <label class="col-md-4 control-label" for="">تاريخ ورود القضيه للأمانة : <span class="text-danger">*</span></label>
                                                                        <div class="col-md-8">




                                                                            <div class="input-group">
                                                                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>

                                                                                <asp:TextBox ID="txtFileCreationDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

                                                                            </div>

                                                                        </div>
                                                                    </div>


                                                                    <div class="form-group">
                                                                        <label class="col-md-4 control-label" for="">الرقم الإلى : <span class="text-danger">*</span> </label>
                                                                        <div class="col-md-5">
                                                                            <asp:TextBox ID="txtFileAutoNumber" class="form-control" runat="server"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-1" style="padding-left: 0px; padding-right: 0px;">
                                                                            <asp:CheckBox ID="chkhasautoNumber" onclick="hasAutoNumber()" class="form-control" runat="server" />
                                                                        </div>
                                                                        <label class="col-md-2 control-label">لا يوجد</label>
                                                                    </div>

                                                                </div>
                                                                <div class="col-md-3">
                                                                    <div class="form-group">


                                                                        <div class="col-md-2" style="padding-left: 0px; padding-right: 0px;">

                                                                            <asp:CheckBox ID="chkIsPrivate" class="form-control" runat="server" />
                                                                        </div>

                                                                        <label class="col-md-4 control-label">ملف خاص </label>

                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <legend class="text-semibold">
                                                            <i class="icon-file-text2 position-left"></i>
                                                            بيانات  القضية
                                                             <div class="position-right" style="float: left; margin-left: 30px">

                                                                 <asp:LinkButton runat="server" ID="lnkAddNewCase" class="btn btn-success btn-xs" OnClick="lnkAddNewCase_Click"><i class="fa fa-plus"></i>&nbsp; إضافة درجة تقاضي  &nbsp;</asp:LinkButton>



                                                             </div>

                                                        </legend>

                                                        <div class="row">

                                                            <div class="col-md-5">

                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">تصنيف القضية :<span class="text-danger">*</span> </label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstCategory" runat="server" class="Select2Drop"></asp:DropDownList>


                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">رقم القضية بالمحكمة : <span class="text-danger">*</span></label>

                                                                    <div class="col-md-2">

                                                                        <asp:TextBox ID="txtSuitNum" class="form-control" runat="server"></asp:TextBox>

                                                                    </div>
                                                                    <div class="col-md-2">

                                                                        <asp:TextBox ID="txtSuitYear" class="form-control" runat="server"></asp:TextBox>

                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <asp:DropDownList ID="lstCaseType" runat="server" class="Select2Drop"></asp:DropDownList>
                                                                    </div>

                                                                    <div class="col-md-2">

                                                                        <asp:TextBox ID="txtSuitType" class="form-control" runat="server"></asp:TextBox>

                                                                    </div>
                                                                </div>

                                                                <div class="form-group" style="display: none">
                                                                    <label class="col-md-3 control-label" for="">الموضوع:</label>

                                                                    <div class="col-md-9 autoDrop">
                                                                        <asp:TextBox runat="server" ID="txtCaseSubject" class="form-control"></asp:TextBox>
                                                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
                                                                            runat="server" TargetControlID="txtCaseSubject"
                                                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />

                                                                    </div>


                                                                </div>

                                                                <div class="form-group" style="display: none">
                                                                    <label class="col-md-3 control-label" for="">الرقم الإلى : </label>
                                                                    <div class="col-md-9">

                                                                        <asp:TextBox ID="txtCaseInternalSerial" class="form-control" runat="server"></asp:TextBox>

                                                                    </div>


                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">ملاحظات:</label>

                                                                    <div class="col-md-9">

                                                                        <asp:TextBox runat="server" ID="txtCaseNote" class="form-control"></asp:TextBox>

                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="col-md-4">

                                                                <div class="form-group">
                                                                    <label class="col-md-5 control-label" for="">تاريخ ايداع القضية فى المحكمة  :  </label>

                                                                    <div class="col-md-7">




                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>

                                                                            <asp:TextBox ID="txtCaseDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label class="col-md-5 control-label" for="">الحالة: <span class="text-danger">*</span> </label>

                                                                    <div class="col-md-7">

                                                                        <asp:DropDownList ID="lstcaseStatus" runat="server" class="Select2Drop"></asp:DropDownList>


                                                                    </div>
                                                                </div>


                                                            </div>

                                                            <div class="col-md-3">

                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="" style="text-align: left">الدرجة: <span class="text-danger">*</span>  </label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstCaseLevel" runat="server" class="Select2Drop"></asp:DropDownList>


                                                                    </div>
                                                                </div>
                                                                <%-- <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">المحكمه </label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstCourt" runat="server" class="form-control"></asp:DropDownList>


                                                                    </div>
                                                                </div>--%>
                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="" style="text-align: left">الحكم:<span class="text-danger">*</span> </label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstDession" runat="server" class="Select2Drop"></asp:DropDownList>


                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="" style="text-align: left">نتيجة الحكم:<span class="text-danger">*</span> </label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstJudgmentresult" runat="server" class="Select2Drop">
                                                                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="لصالح المدعي " Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="لصالح المدعي عليه" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="لم يصدر حكم" Value="3"></asp:ListItem>
                                                                        </asp:DropDownList>


                                                                    </div>
                                                                </div>

                                                            </div>


                                                        </div>

                                                        <div class="row">
                                                        </div>


                                                        <legend class="text-semibold">
                                                            <i class="icon-file-text2 position-left"></i>
                                                            أسماء الخصوم
                                                        </legend>


                                                        <div class="col-md-6">
                                                            <asp:UpdatePanel runat="server" ID="Updatepanel2" ChildrenAsTriggers="true" UpdateMode="conditional">
                                                                <ContentTemplate>


                                                                    <asp:Button UseSubmitBehavior="false" runat="server" ID="btnAddNewItem" Text="Add Item" Style="display: none;" OnClick="btnAddNewItem_Click" />

                                                                    <div class="panel-heading">
                                                                        <h5 class="panel-title">المدعي</h5>

                                                                    </div>

                                                                    <asp:DataGrid ID="grdprosecutor" runat="server"
                                                                        class="table table-hover table-striped table-bordered table-advanced tablesorter"
                                                                        AutoGenerateColumns="False"
                                                                        BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                                                                        CellPadding="3" Width="100%" OnItemDataBound="grdprosecutor_ItemDataBound" OnItemCommand="grdprosecutor_ItemCommand">
                                                                        <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                        <ItemStyle CssClass="grdItem" />
                                                                        <AlternatingItemStyle CssClass="grdItem" />
                                                                        <HeaderStyle CssClass="grdHead" BackColor="Gray" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                        <FooterStyle CssClass="grdFoot" />
                                                                        <PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
                                                                            PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
                                                                        <Columns>

                                                                            <asp:TemplateColumn HeaderText="">
                                                                                <HeaderStyle BackColor="#000000"></HeaderStyle>
                                                                                <ItemStyle Width="1%" HorizontalAlign="center" />
                                                                                <FooterStyle HorizontalAlign="center" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="lnkEdit" CssClass="btn btn-default btn-xs" CommandName="Edit"> <img src="/Layout/RTL/assets/images/EditPerson.png"  /></asp:LinkButton>
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
                                                                                    <asp:LinkButton runat="server" ID="lnkDelete" CssClass="btn btn-default btn-xs" CommandName="Delete"><img src="/Layout/RTL/assets/images/DeletePerson.png" /></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    &nbsp;
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateColumn>



                                                                            <asp:BoundColumn DataField="code" HeaderText="#" Visible="false">
                                                                                <ItemStyle Width="2px" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="PartyType" HeaderText="PartyType" Visible="false"></asp:BoundColumn>

                                                                            <asp:TemplateColumn HeaderText="م.">
                                                                                <ItemStyle HorizontalAlign="center" Width="2px" />
                                                                                <ItemTemplate>
                                                                                    <%#Convert.ToInt32(DataBinder.Eval(Container, "ItemIndex")) + 1%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>



                                                                            <asp:TemplateColumn HeaderText="  الاسم">

                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate>
                                                                                    <%#Eval("FullName") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtname" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                                                    <input id="hdnPerson_NameAr" value='<%#Eval("FullName") %>' runat="server" type="hidden" />

                                                                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
                                                                                        runat="server" TargetControlID="txtname"
                                                                                        CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                                                        ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="PersonsAutoCompete" />

                                                                                </EditItemTemplate>

                                                                            </asp:TemplateColumn>


                                                                            <asp:TemplateColumn HeaderText="  الرقم المدني" Visible="false">
                                                                                <ItemStyle />
                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate>
                                                                                    <%#Eval("CivilID") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtCivilID" MaxLength="12" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>

                                                                                    <input id="hdnCivilID" value='<%#Eval("CivilID") %>' runat="server" type="hidden" />
                                                                                </EditItemTemplate>

                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderText=" ملاحظات">

                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate><%#Eval("Notes") %></ItemTemplate>
                                                                                <EditItemTemplate>

                                                                                    <asp:TextBox ID="txtNotes" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                                                    <input id="hdnNotes" value='<%#Eval("Notes") %>' runat="server" type="hidden" />

                                                                                </EditItemTemplate>


                                                                            </asp:TemplateColumn>

                                                                        </Columns>
                                                                    </asp:DataGrid>

                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>

                                                        <div class="col-md-6">

                                                            <asp:UpdatePanel runat="server" ID="Updatepanel3" ChildrenAsTriggers="true" UpdateMode="conditional">
                                                                <ContentTemplate>


                                                                    <asp:Button UseSubmitBehavior="false" runat="server" ID="Button1" Text="Add Item" Style="display: none;" OnClick="btnAddNewItem_Click" />

                                                                    <div class="panel-heading">
                                                                        <h5 class="panel-title">المدعي عليه </h5>

                                                                    </div>
                                                                    <asp:DataGrid ID="grdDefendant" runat="server"
                                                                        class="table table-hover table-striped table-bordered table-advanced tablesorter"
                                                                        AutoGenerateColumns="False"
                                                                        BackColor="White" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                                                                        CellPadding="3" Width="100%" OnItemDataBound="grdDefendant_ItemDataBound" OnItemCommand="grdDefendant_ItemCommand">
                                                                        <SelectedItemStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                        <ItemStyle CssClass="grdItem" />
                                                                        <AlternatingItemStyle CssClass="grdItem" />
                                                                        <HeaderStyle CssClass="grdHead" BackColor="Gray" ForeColor="White" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                                        <FooterStyle CssClass="grdFoot" />
                                                                        <PagerStyle CssClass="grdPager" HorizontalAlign="center" Mode="NextPrev"
                                                                            PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;" NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
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
                                                                                    <asp:LinkButton runat="server" ID="lnkDelete" CssClass="btn btn-default btn-xs" CommandName="Delete"><img src="/Layout/RTL/assets/images/DeletePerson.png" /></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    &nbsp;
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateColumn>



                                                                            <asp:BoundColumn DataField="code" HeaderText="#" Visible="false">
                                                                                <ItemStyle Width="2px" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="PartyType" HeaderText="PartyType" Visible="false"></asp:BoundColumn>

                                                                            <asp:TemplateColumn HeaderText="م.">
                                                                                <ItemStyle HorizontalAlign="center" Width="2px" />
                                                                                <ItemTemplate>
                                                                                    <%#Convert.ToInt32(DataBinder.Eval(Container, "ItemIndex")) + 1%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>



                                                                            <asp:TemplateColumn HeaderText="  الاسم">

                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate>
                                                                                    <%#Eval("FullName") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtDname" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                                                    <input id="hdnPerson_NameAr" value='<%#Eval("FullName") %>' runat="server" type="hidden" />

                                                                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
                                                                                        runat="server" TargetControlID="txtDname"
                                                                                        CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                                                        ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="PersonsAutoCompete" />

                                                                                </EditItemTemplate>

                                                                            </asp:TemplateColumn>


                                                                            <%--    <asp:TemplateColumn HeaderText="  الرقم المدني">
                                                                        <ItemStyle Width="15%" />
                                                                        <HeaderStyle HorizontalAlign="right" />
                                                                        <ItemTemplate>
                                                                            <%#Eval("CivilID") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtCivilID" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>

                                                                            <input id="hdnCivilID" value='<%#Eval("CivilID") %>' runat="server" type="hidden" />
                                                                        </EditItemTemplate>

                                                                    </asp:TemplateColumn>--%>
                                                                            <asp:TemplateColumn HeaderText=" ملاحظات">

                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate><%#Eval("Notes") %></ItemTemplate>
                                                                                <EditItemTemplate>

                                                                                    <asp:TextBox ID="txtDNotes" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                                                    <input id="hdnNotes" value='<%#Eval("Notes") %>' runat="server" type="hidden" />

                                                                                </EditItemTemplate>


                                                                            </asp:TemplateColumn>

                                                                        </Columns>
                                                                    </asp:DataGrid>

                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
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
                                                                 <i class="icon-attachment"></i>&nbsp; مرفقات القضية</a>

                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>



                                                </fieldset>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="tab-pane <%=activeTab(2) %>" id="badges-tab2">
                                    <%-- <asp:UpdatePanel runat="server" ID="Updatepanel7" ChildrenAsTriggers="true" UpdateMode="conditional">
                                        <ContentTemplate>--%>

                                    <div class="form-horizontal" id="DivAddHearing" runat="server" visible="false">

                                        <div class="row">

                                            <div class="col-md-4">

                                                <div class="form-group">
                                                    <label class="col-md-4 control-label">تاريخ الجلسه<span class="text-danger">*</span> </label>

                                                    <div class="col-md-8">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                            <asp:TextBox ID="txtHearingDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-4 control-label">نص الحكم<span class="text-danger">*</span> </label>

                                                    <div class="col-md-8">
                                                        <asp:TextBox ID="txtHearingDecisionText" TextMode="MultiLine" class="form-control" runat="server"></asp:TextBox>

                                                    </div>

                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-4 control-label" for="">ملاحظات  </label>

                                                    <div class="col-md-8">

                                                        <asp:TextBox ID="txtHearingNotes" class="form-control" runat="server"></asp:TextBox>

                                                    </div>
                                                </div>


                                                <div class="form-group">
                                                    <label class="col-md-4 control-label">ملف الحكم</label>

                                                    <div class="col-md-4">
                                                        <asp:Label ID="Label4" runat="server"></asp:Label>
                                                        <asp:FileUpload ID="txtHearingimage" runat="server" class="file-styled" />
                                                        <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <span class="help-block2">| Or | </span>
                                                        <asp:LinkButton runat="server" ID="btnHearingScan" OnClientClick="return ValidateHearing();" OnClick="btnHearingScan_Click" class="btn btn-info btn-xs"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="col-md-4">

                                                <div class="form-group">
                                                    <label class="col-md-4 control-label">اسم القاضي  <span class="text-danger">*</span> </label>

                                                    <div class="col-md-8">
                                                        <asp:TextBox ID="txthearingConsultant" class="form-control" runat="server"></asp:TextBox>

                                                    </div>

                                                </div>

                                                <div class="form-group">
                                                    <label class="col-md-4 control-label" for="">كاتب الجلسة  </label>

                                                    <div class="col-md-8">

                                                        <asp:TextBox ID="txtHearingWriter" class="form-control" runat="server"></asp:TextBox>

                                                    </div>
                                                </div>

                                            </div>


                                        </div>


                                        <div class="col-md-12">
                                            <div class="form-actions">
                                                <div class="col-md-offset-9 col-md-12">
                                                    <asp:LinkButton ID="lnkSaveHearing" runat="server" class="btn btn-primary" OnClientClick="return ValidateHearing()" OnClick="lnkSaveHearing_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

                                                    &nbsp;
				                               <asp:Button runat="server" ID="lnkCancelHearing" OnClick="lnkCancelHearing_Click" class="btn btn-default" Text=" الغاء  " />

                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row" id="divShowHearing" runat="server">
                                        <div class="col-lg-12">
                                            <div class="portlet box">
                                                <div class="portlet-header">

                                                    <div class="actions pull-right" style="margin-bottom: 10px;">

                                                        <asp:LinkButton runat="server" ID="lnkAddHearing" OnClick="lnkAddHearing_Click" class="btn btn-info btn-xs"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

                                                        <asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="lnkDeleteHearing" OnClick="lnkDeleteHearing_Click" class="btn btn-danger btn-xs"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

                                                    </div>
                                                </div>
                                                <div class="portlet-body">
                                                    <div class="datatable-scroll" style="padding-top: 20px;">

                                                        <asp:DataGrid runat="server" ID="grdHraingList" AutoGenerateColumns="False"
                                                            AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemCommand="grdHraingList_ItemCommand" OnEditCommand="grdHraingList_EditCommand">
                                                            <PagerStyle Visible="False" />
                                                            <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                                            <Columns>
                                                                <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="CreationDate" HeaderText="تاريخ التسجيل  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="HearningDate" HeaderText=" تاريخ الجلسه  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="DecisionText" HeaderText="نص الحكم "></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="CaseConsulatName" HeaderText="  القاضي "></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="HearingWriter" HeaderText="  كاتب الجلسة "></asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Note" HeaderText="ملاحظات "></asp:BoundColumn>

                                                                <asp:TemplateColumn HeaderText="عرض المرفق">
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <a target="_blank" href="<%#  ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath +gets(Eval("CaseID"))+"/Hearing/"+ gets(Eval("Code"))+"/"  + "&vfileList=[" + gets(Eval("HearingAttachment")) +";]"%>">عرض المرفق

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
                                                        </div>
                                                        <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">
                                                            <%-- <a class="paginate_button previous disabled" aria-controls="DataTables_Table_3" data-dt-idx="0" tabindex="0" id="DataTables_Table_3_previous">→</a>
                        <span><a class="paginate_button current" aria-controls="DataTables_Table_3" data-dt-idx="1" tabindex="0">1</a>
                            <a class="paginate_button " aria-controls="DataTables_Table_3" data-dt-idx="2" tabindex="0">2</a>


                        </span>
                        <a class="paginate_button next" aria-controls="DataTables_Table_3" data-dt-idx="3" tabindex="0" id="DataTables_Table_3_next">←</a>--%>


                                                            <cc1:Pager CurrentIndex="1" OnCommand="pager_Command5" ShowFirstLast="False" ID="pager5"
                                                                runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>


                                                        </div>
                                                    </div>
                                                </div>
                                            </div>



                                        </div>
                                    </div>

                                    <%--   </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </div>

                                <div class="tab-pane <%=activeTab(3) %>" id="badges-tab3">
                                    <%--  <asp:UpdatePanel runat="server" ID="Updatepanel4" ChildrenAsTriggers="true" UpdateMode="conditional">
                                        <ContentTemplate>--%>

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


                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">الملف  </label>

                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label2" runat="server"></asp:Label>
                                                        <asp:FileUpload ID="txtIncomingImge" runat="server" class="file-styled" />
                                                        <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <span class="help-block2">| Or | </span>
                                                        <asp:LinkButton runat="server" ID="btnIncomingScan" OnClientClick="return ValidateIncoming();" OnClick="btnIncomingScan_Click" class="btn btn-info btn-xs"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
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

                                                    <div class="col-md-9" style="z-index: 99">

                                                        <asp:TextBox ID="txtFrom" class="form-control" runat="server"></asp:TextBox>

                                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender3"
                                                            runat="server" TargetControlID="txtFrom"
                                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="ArchOrgAutoCompete" />


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

                                                <div class="form-group" style="display: none">
                                                    <label class="col-md-3 control-label">تاريخ التنبيه  </label>

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


                                                <div class="form-group" id="hearingdatecontainer" style="display: none">
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
                                                    <asp:LinkButton ID="lnkSaveIncoming" runat="server" class="btn btn-primary" OnClientClick="return ValidateIncoming()" OnClick="lnkSaveIncoming_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

                                                    &nbsp;
				                               <asp:Button runat="server" ID="lnkCancelIncoming" class="btn btn-default" Text=" الغاء  " OnClick="lnkCancelIncoming_Click" />

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
                                                                <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>

                                                                <asp:TemplateColumn HeaderText="النوع">
                                                                    <ItemStyle Width="3%" />
                                                                    <ItemTemplate>
                                                                        <%#fillDocType(gets(Eval("Doc_Type"))) %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Doc_Serial" HeaderText="رقم الكتاب"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Doc_From" HeaderText="وارد من "></asp:BoundColumn>

                                                                <asp:BoundColumn DataField="SentDate" HeaderText="التاريخ  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Doc_Subject" HeaderText="الموضوع "></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Doc_Notes" HeaderText="ملاحظات "></asp:BoundColumn>
                                                                <%--<asp:BoundColumn DataField="NextFollowReminderDate" HeaderText="تاريخ التنبيه  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>--%>

                                                                <asp:TemplateColumn HeaderText="جلسة  ">
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="left" />

                                                                    <ItemTemplate>
                                                                        <%#ShowYesNo(getBool(Eval("IsHearing"))) %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>


                                                                <asp:TemplateColumn HeaderText="عرض المرفق">
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <%-- <% if (!(Eval("Filepath")).Equals("")) { %>--%>
                                                                        <a target="_blank" href="<%#  ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath +gets(Eval("RefDocID"))+"/incoming/"+ gets(Eval("Code"))+"/"  + "&vfileList=[" + gets(Eval("Filepath")) +";]"%>">عرض المرفق

                                                 <i class="icon-attachment"></i>&nbsp;
                                                                        </a>
                                                                        <%--  <%}%>--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <%--<asp:TemplateColumn HeaderText="مرفقات">
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>

                                                                        <a href="CaseAttachments.aspx?DocID=<%#Eval("code") %>&CaseID=<%#Eval("RefDocID") %>&fileID=0" class="btn btn-default btn-xs iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                مرفقات
                                                                        </a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>--%>

                                                                <asp:TemplateColumn HeaderText="صادر" Visible="false">
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>

                                                                        <a href="CaseArcData.aspx?RefType=2&DocID=<%#Eval("code") %>&CaseID=<%#Eval("RefDocID") %>" class="btn btn-default btn-xs iframe">
                                                                            <i class="fa fa-angle-double-right"></i>&nbsp;
                                                                                      صادر (<%# objRepository.getchildDocs(ZeroIntergerIFNull(Eval("code").ToString())) %>)
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

                                    <%--                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </div>
                                <div class="tab-pane <%=activeTab(4) %>" id="badges-tab4">
                                    <%--  <asp:UpdatePanel runat="server" ID="Updatepanel5" ChildrenAsTriggers="true" UpdateMode="conditional">
                                        <ContentTemplate>--%>

                                    <div class="form-horizontal" id="divOutgiongAdd" runat="server" visible="false">

                                        <div class="row">

                                            <div class="col-md-4">

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">رقم الكتاب<span class="text-danger">*</span></label>

                                                    <div class="col-md-9">

                                                        <asp:TextBox ID="txtOutDocNo" class="form-control" runat="server"></asp:TextBox>
                                                    </div>

                                                </div>

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">الموضوع <span class="text-danger">*</span></label>

                                                    <div class="col-md-9">

                                                        <asp:TextBox ID="txtOutSubject" class="form-control" runat="server"></asp:TextBox>
                                                    </div>

                                                </div>

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">ملاحظات  </label>

                                                    <div class="col-md-9">

                                                        <asp:TextBox ID="txtoutNotes" class="form-control" runat="server"></asp:TextBox>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">الملف  </label>

                                                    <div class="col-md-5">
                                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                                        <asp:FileUpload ID="txtoutgoiningImage" runat="server" class="file-styled" />
                                                        <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <span class="help-block2">| Or | </span>
                                                        <asp:LinkButton runat="server" ID="btnOutgoingScan" OnClientClick="return Validateoutgoing();" OnClick="btnOutgoingScan_Click" class="btn btn-info btn-xs"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="col-md-4">

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label" for="">صادر إلى    <span class="text-danger">*</span></label>

                                                    <div class="col-md-9" style="z-index: 99">

                                                        <asp:TextBox ID="txtto" class="form-control" runat="server"></asp:TextBox>
                                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender4"
                                                            runat="server" TargetControlID="txtto"
                                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="ArchOrgAutoCompete" />
                                                    </div>

                                                </div>

                                                <div class="form-group">
                                                    <label class="col-md-3 control-label">التاريخ<span class="text-danger">*</span> </label>

                                                    <div class="col-md-9">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                            <asp:TextBox ID="txtoutDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                </div>

                                                <div class="form-group" style="display: none">
                                                    <label class="col-md-3 control-label">تاريخ التنبيه  </label>

                                                    <div class="col-md-9">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                            <asp:TextBox ID="txtOutReminderDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12">
                                            <div class="form-actions">
                                                <div class="col-md-offset-9 col-md-12">
                                                    <asp:LinkButton ID="lnkSaveOut" runat="server" class="btn btn-primary" OnClientClick="return Validateoutgoing()" OnClick="lnkSaveOut_Click"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>

                                                    &nbsp;
				                               <asp:Button runat="server" ID="lnkCancelOut" class="btn btn-default" Text=" الغاء  " OnClick="lnkCancelOut_Click" />

                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row" id="divoutgoingshow" runat="server">
                                        <div class="col-lg-12">
                                            <div class="portlet box">
                                                <div class="portlet-header">

                                                    <div class="actions pull-right" style="margin-bottom: 10px;">

                                                        <asp:LinkButton runat="server" ID="lnkAddNewOutGoing" class="btn btn-info btn-xs" OnClick="lnkAddNewOutGoing_Click"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

                                                        <asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="lnkDeleteOutgoing" class="btn btn-danger btn-xs" OnClick="lnkDeleteOutgoing_Click"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

                                                    </div>
                                                </div>
                                                <div class="portlet-body">
                                                    <div class="datatable-scroll" style="padding-top: 20px;">

                                                        <asp:DataGrid runat="server" ID="grdOutgoing" AutoGenerateColumns="False"
                                                            AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnItemCommand="grdOutgoing_ItemCommand" OnEditCommand="grdOutgoing_EditCommand">
                                                            <PagerStyle Visible="False" />
                                                            <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                                            <Columns>
                                                                <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>

                                                                <asp:TemplateColumn HeaderText="النوع">
                                                                    <ItemStyle Width="3%" />
                                                                    <ItemTemplate>
                                                                        <%#fillDocType(gets(Eval("Doc_Type"))) %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Doc_Serial" HeaderText="رقم الكتاب"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Doc_to" HeaderText=" صادر الي "></asp:BoundColumn>

                                                                <asp:BoundColumn DataField="SentDate" HeaderText="التاريخ  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Doc_Subject" HeaderText="الموضوع "></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Doc_Notes" HeaderText="ملاحظات "></asp:BoundColumn>
                                                                <%--<asp:BoundColumn DataField="NextFollowReminderDate" HeaderText="تاريخ التنبيه  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>--%>
                                                                <%--  <asp:TemplateColumn HeaderText="مرفقات">
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>

                                                                        <a href="CaseAttachments.aspx?DocID=<%#Eval("code") %>&CaseID=<%#Eval("RefDocID") %>&FileID=0" class="btn btn-default btn-xs iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                مرفقات
                                                                        </a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>--%>


                                                                <asp:TemplateColumn HeaderText="عرض المرفق">
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <%-- <% if (!(Eval("Filepath")).Equals("")) { %>--%>
                                                                        <a target="_blank" href="<%#  ScannerRepositoryViewer + "?targetpath=" +  _TargetUploadPath +gets(Eval("RefDocID"))+"/outgoing/"+ gets(Eval("Code"))+"/" + "&vfileList=[" + gets(Eval("Filepath"))+";]" %>" style="<%#showattachment(gets(Eval("Filepath")))%>">عرض المرفق

                                                 <i class="icon-attachment"></i>&nbsp;
                                                                        </a>
                                                                        <%--  <%}%>--%>
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
                                                            <asp:Label ID="lbloutgoingCount" runat="server"></asp:Label>
                                                        </div>
                                                        <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">
                                                            <%-- <a class="paginate_button previous disabled" aria-controls="DataTables_Table_3" data-dt-idx="0" tabindex="0" id="DataTables_Table_3_previous">→</a>
                        <span><a class="paginate_button current" aria-controls="DataTables_Table_3" data-dt-idx="1" tabindex="0">1</a>
                            <a class="paginate_button " aria-controls="DataTables_Table_3" data-dt-idx="2" tabindex="0">2</a>


                        </span>
                        <a class="paginate_button next" aria-controls="DataTables_Table_3" data-dt-idx="3" tabindex="0" id="DataTables_Table_3_next">←</a>--%>


                                                            <cc1:Pager CurrentIndex="1" OnCommand="pager_Command3" ShowFirstLast="False" ID="pager3"
                                                                runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>


                                                        </div>
                                                    </div>
                                                </div>
                                            </div>



                                        </div>
                                    </div>

                                    <%--                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </div>
                                <div class="tab-pane <%=activeTab(5) %>" id="badges-tab5">
                                    <%--  <asp:UpdatePanel runat="server" ID="Updatepanel6" ChildrenAsTriggers="true" UpdateMode="conditional">
                                        <ContentTemplate>--%>
                                    <div class="panel-body">

                                        <div class="row">
                                            <asp:Label runat="server" ID="Label1"></asp:Label>
                                            <div class="col-lg-12">
                                                <div class="portlet box portlet-blue" id="divAddAttachment" runat="server" visible="false">
                                                    <div class="portlet-header">
                                                        <div class="caption">
                                                            <asp:Label runat="server" ID="lblAttachmentError">إضافة جديد</asp:Label>
                                                        </div>

                                                    </div>
                                                    <div class="portlet-body">
                                                        <div role="form" class="form-horizontal">
                                                            <div class="row">

                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label class="col-md-3 control-label" for="">نوع المرفق</label>

                                                                        <div class="col-md-9">
                                                                            <asp:DropDownList ID="lstAttachmentType" class="form-control Select2Drop" runat="server"></asp:DropDownList>
                                                                        </div>
                                                                    </div>



                                                                    <div class="form-group">
                                                                        <label class="col-md-3 control-label" for="">تاريخ الوثيقة</label>

                                                                        <div class="col-md-9">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                                                                <%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
                                                                                <asp:TextBox ID="txtAttachCreationDate" runat="server" class="form-control pickadate-selectors"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label class="col-md-3 control-label">المرجع</label>

                                                                        <div class="col-md-9">

                                                                            <asp:TextBox runat="server" class="form-control" ID="txtAttachRef"></asp:TextBox>


                                                                        </div>

                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label class="col-md-3 control-label">الموضوع</label>

                                                                        <div class="col-md-9">

                                                                            <asp:TextBox runat="server" class="form-control" ID="txtAttachSubject"></asp:TextBox>


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
                                                                            <asp:LinkButton runat="server" OnClientClick="return showscannerLoading();" ID="lnkScan" class="btn btn-info btn-xs" OnClick="lnkScan_Click"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                                        </div>


                                                                    </div>


                                                                </div>

                                                                <div class="col-md-12">
                                                                    <div class="form-actions">
                                                                        <div class="col-md-offset-3 col-md-9">
                                                                            <asp:LinkButton ID="lnkSaveAttachment" runat="server" class="btn btn-primary" OnClientClick="return validateFileAttachment();" OnClick="btnAttachSave_Click"><i class='fa fa-save'></i>&nbsp;  حفظ البيانات </asp:LinkButton>

                                                                            &nbsp;
				                               <asp:Button runat="server" ID="LnkCancelAttachment" class="btn btn-default" Text=" الغاء " OnClick="btnAttCancel_Click" />

                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="row" id="DivAttachmentShow" runat="server">
                                            <div class="col-lg-12">
                                                <div class="portlet box">
                                                    <div class="portlet-header">

                                                        <div class="actions pull-right" style="margin-bottom: 10px;">

                                                            <asp:LinkButton runat="server" ID="lnkNewAttachment" class="btn btn-info btn-xs" OnClick="btnNewAttachement_Click"><i class="fa fa-plus"></i>&nbsp; إضافة جديد&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="lnkDeleteAttachment" class="btn btn-danger btn-xs" OnClick="btnAttDelete_Click"><i class="fa fa-times"></i>&nbsp;حذف الببانات المختاره</asp:LinkButton>

                                                        </div>
                                                    </div>
                                                    <div class="portlet-body">


                                                        <div class="datatable-scroll">
                                                            <asp:DataGrid runat="server" ID="grdCaseAttachment" AutoGenerateColumns="False"
                                                                AllowPaging="True" PageSize="20" class="table datatable-basic dataTable no-footer" OnEditCommand="grdCaseAttachment_EditCommand" OnItemDataBound="grdCaseAttachment_ItemDataBound">
                                                                <PagerStyle Visible="False" />
                                                                <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>


                                                                    <asp:BoundColumn DataField="ReceiveDate" HeaderText="تاريخ المرفق" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="UploadDate" HeaderText="تاريخ الرفع" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>

                                                                    <asp:BoundColumn DataField="AttacheSubject" HeaderText=" الموضوع"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="AttachRef" HeaderText="مرجع"></asp:BoundColumn>

                                                                    <asp:TemplateColumn HeaderText="عرض المرفق">
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemTemplate>
                                                                            <%--   <a target="_blank" href="<%# _TargetUrl.Substring(0, _TargetUrl.LastIndexOf ('/')) + _TargetUploadPath +gets(Eval("Filepath")) %>">عرض المرفق
                                                                                    <i class="icon-attachment"></i>&nbsp;
                                                                                    </a>--%>


                                                                            <a target="_blank" href="<%#  ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList=["  + gets(Eval("Filepath"))+";]" %>">عرض المرفق

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
                                                                <asp:Label ID="lblAttachcount" runat="server"></asp:Label>
                                                            </div>
                                                            <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_3_paginate">

                                                                <cc1:Pager CurrentIndex="1" OnCommand="Attachepager_Command" ShowFirstLast="False" ID="pager4"
                                                                    runat="server" Width="100%" PageSize="20" AlternativeTextEnabled="False" BackToFirstClause="" BackToPageClause="" EnableSmartShortCuts="True" EnableTheming="True" FirstClause="" FromClause="" GoClause="" GoToLastClause="" LastClause="" NextClause="التالي" OfClause="من" PageClause="صفحة" PreviousClause="السابق" RTL="True" ShowingResultClause="" ShowResultClause=""></cc1:Pager>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--  </ContentTemplate>
                                    </asp:UpdatePanel>--%>
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
