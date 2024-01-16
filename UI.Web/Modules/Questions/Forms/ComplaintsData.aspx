<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="ComplaintsData.aspx.cs" Inherits="UI.Web.Modules.Questions.Forms.ComplaintsData" %>

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


        //var selectedPersonToValues = new Array();
        //selectedPersonToValues = [2, 3];
        //function CheckSelected() {



        //    // alert("Selected value is: " + $(".multiselect-filtering").val());
        //    //  alert(selectedPersonToValues);
        //    //$(".multiselect-filtering").val(selectedPersonToValues);
        //    $('#multiselect-filtering').select2('val', selectedPersonToValues);
        //    return false;
        //}

      <%--  function setselctedRequestedFrom() {
            document.getElementById("<%=hdnfilterRequestedFrom.ClientID %>").value = $("#<%=lstFilterPerson.ClientID %>").val();

        }--%>

        function ValidateHeading() {
          <%--  var txt = document.getElementById("<%=chkHasAnswer.ClientID %>")
            if (txt.checked) {
                document.getElementById("Answerdatecontainer").style.display = '';
            } else {
                document.getElementById("Answerdatecontainer").style.display = 'none';
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
                myrow = "ctl00_Main_grdCompaintsList_ctl0" + rowIndex;
            else
                myrow = "ctl00_Main_grdCompaintsList_ctl" + rowIndex;
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




        function chkImage() {





            var txt = document.getElementById("<%=txtCompaintDate.ClientID %>");
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،  اختر تاريخ الشكوى ");
                txt.focus();
                return false;
            }



            var txt = document.getElementById("<%=txtCompaintSubject.ClientID %>");
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  نص الشكوى ");
                txt.focus();
                return false;
            }



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

        function ValidateIncoming() {

            var txt = document.getElementById("<%=hdnMasterID.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،احفظ بيانات الشكوى اولا ");
                return false;
            }

            var txt = document.getElementById("<%=txtDoc_Serial.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل رقم الوثيقة     ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=lstIncomingOrg.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
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
                new $.Zebra_Dialog("فضلا ،احفظ بيانات الشكوى اولا ");
                return false;
            }
            var txt = document.getElementById("<%=txtOutDocNo.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل رقم الوثيقة ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=lstoutgoiningOrg.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
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


        function setactiveTab(tabindex) {
            //alert("para:" + tabindex)
            var txt = document.getElementById("<%=hdnactivetab.ClientID %>");
            txt.value = tabindex;

        }


        function showscannerLoading() {

            if (ValidateAnswer()) {
                document.getElementById("scanLoading").style.display = "";
                return true;
            } else { return false; }
        }



        $(document).ready(function () {
          <%--  var selectedCodeWBs = [<%=QrelatedOrg%>];
             //   $('#<%=lstRelatedOrgs.ClientID %>').select2().val(selectedCodeWBs).change();
                $('#<%=lstRelatedOrgs.ClientID %>').val(selectedCodeWBs).select2(); --%>


        });


    </script>

<input id="hdnQScannerfilepath" runat="server" type="hidden" />
    <input id="hdnAnswerScannerfilepath" runat="server" type="hidden" />

    <input id="hdnIncomingScannerfilepath" runat="server" type="hidden" />
    <input id="hdnOutgoingScannerfilepath" runat="server" type="hidden" />

      <input id="hdnfilterRequestedFrom" runat="server" type="hidden" />
    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        نظام الشكاوى والعرائض
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
                                <div class="form-group" style="display:">
                                    <label class="col-lg-3 control-label">
                                        رقم الشكوى :</label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="txtFilterserial" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>







                                 <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                       الجهة المعنية  :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <%--<asp:TextBox ID="txtFilterOrg" runat="server" class="form-control"></asp:TextBox>--%>

                                        <asp:DropDownList ID="lstFilterCMGSRelatedOrgs" class="Select2Drop" runat="server"></asp:DropDownList>

                                    </div>
                                </div>





                            </div>

                            <div class="col-md-4">
                             <div class="form-group">
                                    <label class="col-lg-3 control-label">تاريخ الشكوى من :</label>
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
                                        جزء من نص الشكوى :
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
                                        مقدم الشكوى :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:TextBox ID="txtFilterName" runat="server" class="form-control"></asp:TextBox>

                                        <%--  <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1"
                                            runat="server" TargetControlID="txtFilterSubject"
                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />--%>
                                    </div>



                                </div>

                            </div>


                        </fieldset>
                    </div>
                    <div class="text-right">
                        <asp:LinkButton ID="lnkSearch" OnClientClick="setselctedRequestedFrom();" class="btn btn-primary" runat="server" OnClick="lnkSearch_Click">&nbsp;&nbsp; بحث&nbsp;&nbsp; <i class="icon-search4 position-right"></i></asp:LinkButton>
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

                                                  نتيجة البحث  <span style="color:#000;font-size:14px;"> ( <asp:Label ID="lblSearchResultCount" runat="server"></asp:Label>)</span>


                                                <asp:LinkButton ID="lnkSearchback" CssClass="control-arrow" runat="server" OnClick="lnkSearchback_Click"> رجوع  <i class=" icon-backward2"></i></asp:LinkButton>

                                                </legend>




                                                <div class="datatable-scroll">


                                                    <asp:DataGrid ID="grdCompaintsList" runat="server"
                                                        DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="40" class="table datatable-basic dataTable no-footer"
                                                        Width="100%"   OnItemCommand="grdCompaintsList_ItemCommand" OnItemDataBound="grdCompaintsList_ItemDataBound">
                                                        <SelectedItemStyle ForeColor="White" />
                                                        <ItemStyle CssClass="grdItem" />
                                                        <AlternatingItemStyle CssClass="grdItem" />
                                                        <PagerStyle Visible="false" />
                                                        <HeaderStyle CssClass="grdHead" BackColor="Black" ForeColor="White" Font-Bold="False" />
                                                        <Columns>


                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="code"></asp:BoundColumn>


                                                               <asp:TemplateColumn HeaderText=""  >
                                                                <ItemStyle HorizontalAlign="Right" BackColor="#EEF0FA" />
                                                                <ItemTemplate>
                                                                     

                                                                    
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
                                                                    <img style="cursor: pointer;" src="/layout/images/plus.gif" alt="" border="0" runat="server" id="imgControl" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:BoundColumn DataField="Complaints_Serial" HeaderText="رقم الشكوى ">
                                                                <HeaderStyle Wrap="false" />
                                                                                                                                <ItemStyle Width="5%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Complaints_Date" HeaderText="تاريخ الشكوى" DataFormatString="{0:dd/MM/yyyy}">
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle Width="5%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Complaints_Subject" HeaderText="الشكوى">
                                                                <HeaderStyle Wrap="false" />

                                                            </asp:BoundColumn>




                                                             <asp:BoundColumn DataField="Complaints_OwnerName" HeaderText="مقدم الشكوى  ">
                                                                <HeaderStyle Wrap="false" />
                                                                   <ItemStyle Width="15%" />
                                                            </asp:BoundColumn>

                                                             <asp:BoundColumn DataField="RelatedOrgNameAr" HeaderText="   الجهة المعنية ">
                                                                <HeaderStyle Wrap="false" />
                                                                   <ItemStyle Width="15%" />
                                                            </asp:BoundColumn>




                                                            <asp:TemplateColumn HeaderText="التفاصيل">
                                                                <ItemStyle HorizontalAlign="right" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div style="text-align: right">
                                                                        <%--    <a href="CompaintAttachments.aspx?FileID=<%#Eval("Code") %>" class="btn btn-default btn-xs iframe">
                                                                        <i class="icon-attachment"></i>&nbsp;
                                                                       نص الشكوى
                                                                    </a>--%>
                                                                          <a href="<%#ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath+gets(Eval("Code"))+"/" + "&vfileList=[" + gets(Eval("Complaints_File"))+";]"%>" style="font-size: 12px; <%#showattachment(gets(Eval("Complaints_File")))%>" class="label border-left-primary label-striped iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                          نص الشكوى
                                                                        </a>


                                                                    </div>
                                                                    <div style="margin-top: 10px;">
                                                                        <a href="complaintsData.aspx?CompaintID=<%#Eval("Code")%>" class="label border-left-success label-striped" style="font-size: 12px;">
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
                                <li class="<%=activeTab(1) %>" onclick="setactiveTab(1)"><a href="#badges-tab1" data-toggle="tab"><i class="icon-menu7 position-left"></i>بيانات الشكوى</a></li>
                                <li class="<%=activeTab(2) %>" onclick="setactiveTab(2)"><a href="#badges-tab3" data-toggle="tab">الوارد <span class="badge badge-success  position-right">
                                    <asp:Label ID="lblComingalert" runat="server" Text="0"></asp:Label></span></a></li>
                                <li class="<%=activeTab(3) %>" onclick="setactiveTab(3)"><a href="#badges-tab4" data-toggle="tab">الصادر <span class="badge badge-success  position-right">
                                    <asp:Label ID="lbloutAlert" runat="server" Text="0"></asp:Label></span></a></li>
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
                                                            بيانات  الشكوى


                                                        </legend>

                                                        <div class="row">


                                                            <div class="col-md-4">
                                                                 <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">رقم الشكوى   : <span class="text-danger">*</span></label>

                                                                    <div class="col-md-9">
                                                                            <asp:TextBox ID="txtComplaintSerial" class="form-control" runat="server"></asp:TextBox>
                                                                    </div>
                                                                </div>


                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">مقدم الشكوى   : <span class="text-danger">*</span></label>

                                                                    <div class="col-md-9">
                                                                            <asp:TextBox ID="txtOwnerName" class="form-control" runat="server"></asp:TextBox>
                                                                    </div>
                                                                </div>




                                                            </div>

                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">تاريخ الشكوى:<span class="text-danger">*</span>  </label>

                                                                    <div class="col-md-9">

                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>

                                                                            <asp:TextBox ID="txtCompaintDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

                                                                        </div>

                                                                    </div>
                                                                </div>


                                                                 <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">  الجهة المعنية   :<span class="text-danger">*</span> </label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstCMGSRelatedOrg" runat="server" class="Select2Drop"></asp:DropDownList>


                                                                    </div>
                                                                </div>


                                                            </div>

                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-8">




                                                                <div class="form-group">
                                                                    <label class="col-md-2 control-label" for="">نص الشكوى <span class="text-danger">*</span>:</label>

                                                                    <div class="col-md-10 autoDrop">
                                                                        <asp:TextBox runat="server" ID="txtCompaintSubject" class="form-control" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                        <%-- <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
                                                                            runat="server" TargetControlID="txtCompaintSubject"
                                                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />--%>
                                                                    </div>


                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-2 control-label" for="">ملاحظات:</label>

                                                                    <div class="col-md-10">

                                                                        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtCompaintNote" class="form-control"></asp:TextBox>

                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-2 control-label">ملف الشكوى:</label>

                                                                    <div class="col-md-7">
                                                                        <asp:Label ID="lblimage" runat="server"></asp:Label>
                                                                        <asp:FileUpload ID="txtQImage" runat="server" class="file-styled" />
                                                                        <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <span class="help-block2">| Or | </span>
                                                                        <asp:LinkButton runat="server" ID="lnkQScan" OnClientClick="return chkImage();" OnClick="lnkQScan_Click" class="btn btn-info btn-xs"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                                    </div>

                                                                </div>

                                                            </div>

                                                            <div class="col-md-4">

                                                                <div class="form-group" style="display:none">
                                                                    <label class="col-md-3 control-label" for="">لعدم الإختصاص  : </label>

                                                                    <div class="col-md-9">
                                                                        <asp:CheckBox ID="chkIsGrouped" runat="server" />

                                                                    </div>
                                                                </div>

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
                                                                 <i class="icon-attachment"></i>&nbsp; عرض نص الشكوى</a>

                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                                </fieldset>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane <%=activeTab(2) %>" id="badges-tab3">
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




                                                        <asp:DropDownList ID="lstIncomingOrg" class="Select2Drop" runat="server"></asp:DropDownList>

                                                        <%--  <asp:TextBox ID="txtFrom" class="form-control" runat="server"></asp:TextBox>

                                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender3"
                                                            runat="server" TargetControlID="txtFrom"
                                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="ArchOrgAutoCompete" />--%>
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

                                                <div class="form-group" style="display:none ">
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
                                                                <asp:BoundColumn DataField="NextFollowReminderDate" Visible="false" HeaderText="تاريخ التنبيه  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>


                                                                <asp:TemplateColumn HeaderText="عرض المرفق">
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>

                                                                        <a class="label border-left-primary label-striped iframe" target="_blank" href="<%#  ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath +gets(Eval("RefDocID"))+"/incoming/"+ gets(Eval("Code"))+"/"  + "&vfileList=[" + gets(Eval("Filepath")) +";]"%>" style="<%#showattachment(gets(Eval("Filepath")))%>">
                                                                            <i class="icon-attachment"></i>&nbsp; عرض المرفق


                                                                        </a>



                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <%-- <asp:TemplateColumn HeaderText="مرفقات">
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>

                                                                        <a href="CompaintAttachments.aspx?DocID=<%#Eval("code") %>&FileID=<%#Eval("RefDocID") %>&fileID=0" class="btn btn-default btn-xs iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                مرفقات
                                                                        </a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>--%>

                                                                <%--  <asp:TemplateColumn HeaderText="صادر">
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>

                                                                        <a href="CompaintArcData.aspx?RefType=2&DocID=<%#Eval("code") %>&CompaintID=<%#Eval("RefDocID") %>" class="btn btn-default btn-xs iframe">
                                                                            <i class="fa fa-angle-double-right"></i>&nbsp;
                                                                                      صادر (<%# objRepository.getchildDocs(ZeroIntergerIFNull(Eval("code").ToString())) %>)
                                                                        </a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>--%>
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
                                <div class="tab-pane <%=activeTab(3) %>" id="badges-tab4">
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


                                                        <asp:DropDownList ID="lstoutgoiningOrg" class="Select2Drop" runat="server"></asp:DropDownList>

                                                        <%--    <asp:TextBox ID="txtto" class="form-control" runat="server"></asp:TextBox>
                                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender4"
                                                            runat="server" TargetControlID="txtto"
                                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="ArchOrgAutoCompete" />--%>
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

                                                <div class="form-group" style="display:none ">
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
                                                                <asp:BoundColumn DataField="Doc_to" HeaderText="صادر إلى   "></asp:BoundColumn>

                                                                <asp:BoundColumn DataField="SentDate" HeaderText="التاريخ  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Doc_Subject" HeaderText="الموضوع "></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Doc_Notes" HeaderText="ملاحظات "></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="NextFollowReminderDate" Visible="false" HeaderText="تاريخ التنبيه  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>


                                                                <asp:TemplateColumn HeaderText="عرض المرفق">
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <%-- <% if (!(Eval("Filepath")).Equals("")) { %>--%>
                                                                        <a class="label border-left-primary label-striped iframe" target="_blank" href="<%#  ScannerRepositoryViewer + "?targetpath=" +  _TargetUploadPath +gets(Eval("RefDocID"))+"/outgoing/"+ gets(Eval("Code"))+"/" + "&vfileList=[" + gets(Eval("Filepath"))+";]" %>" style="<%#showattachment(gets(Eval("Filepath")))%>">

                                                                            <i class="icon-attachment"></i>&nbsp;عرض المرفق

                                                                        </a>
                                                                        <%--  <%}%>--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <%-- <asp:TemplateColumn HeaderText="مرفقات">
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>

                                                                        <a href="CompaintAttachments.aspx?DocID=<%#Eval("code") %>&FileID=<%#Eval("RefDocID") %>&FileID=0" class="btn btn-default btn-xs iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                مرفقات
                                                                        </a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>--%>
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
