<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="InterrogationData.aspx.cs" Inherits="UI.Web.Modules.Questions.Forms.InterrogationData" %>

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

        function setselctedRequestedFrom() {
            document.getElementById("<%=hdnfilterRequestedFrom.ClientID %>").value = $("#<%=lstFilterPerson.ClientID %>").val();

        }

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
                myrow = "ctl00_Main_grdQuestionsList_ctl0" + rowIndex;
            else
                myrow = "ctl00_Main_grdQuestionsList_ctl" + rowIndex;
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


           <%-- var txt = document.getElementById("<%=txtQuestionInternalSerial.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  مسلسل الاستجواب");
                txt.focus();
                return false;
            }--%>



            var txt = document.getElementById("<%=txtQuestionDate.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،  اختر تاريخ ورود الاستجواب ");
                txt.focus();
                return false;
            }


            var txt = document.getElementById("<%=lstRequestTo.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر المستجوب  ");
                txt.focus();
                return false;
            }


            var txt = document.getElementById("<%=lstChapter.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر الفصل التشريعي  ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=lstSession.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر  دور الانعقاد ");
                txt.focus();
                return false;
            }

            var txt = document.getElementById("<%=lstResult.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر نتيجة الاستجواب ");
                txt.focus();
                return false;
            }
            var txt = document.getElementById("<%=txtQuestionSubject.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  نص الاستجواب ");
                txt.focus();
                return false;
            }

         <%--   var lstRequestedFromPareon = document.getElementById("<%=lstRequestFrom.ClientID %>");// getObjById("lstRequestFrom").value;
            if (lstRequestedFromPareon.value == "" || lstRequestedFromPareon.value == "0") {
                new $.Zebra_Dialog("فضلا ، يرجي إختيار مقدم الاستجواب  ");
                return false;
            }--%>

            //var lstRequestedFromOrg = getObjById("lstRelatedOrgs").value;
            //if (lstRequestedFromOrg != "0") {
            //    new $.Zebra_Dialog("فضلا ، يرجي اضافة الجهة المعنية  ");
            //    return false;
            //}


            //Get selected Value
         //  alert($("#<%=lstRelatedOrgs.ClientID %>").val());

            document.getElementById("<%=hdnRelatedOrg.ClientID %>").value = $("#<%=lstRelatedOrgs.ClientID %>").val();

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
                new $.Zebra_Dialog("فضلا ،اختر العضو مقدم الاسنجواب      ");
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
            var selectedCodeWBs = [<%=QrelatedOrg%>];


             //   $('#<%=lstRelatedOrgs.ClientID %>').select2().val(selectedCodeWBs).change();

                $('#<%=lstRelatedOrgs.ClientID %>').val(selectedCodeWBs).select2();



        });

        function viewresult() {

            var txt = document.getElementById("<%=lstResult.ClientID %>");
            if (txt.value == "1") {
                document.getElementById("<%=vote1.ClientID %>").style.display = "";
                document.getElementById("<%=vote2.ClientID %>").style.display="";
            } else {

                  document.getElementById("<%=vote1.ClientID %>").style.display = "none";
                document.getElementById("<%=vote2.ClientID %>").style.display = "none";

                document.getElementById("<%=txtVoteWith.ClientID %>").value = "";
                document.getElementById("<%=txtVoteAgenest.ClientID %>").value = "";
                document.getElementById("<%=txtVoteVoid.ClientID %>").value = "";
                document.getElementById("<%=txtVoteabsent.ClientID %>").value = "";

            }
        }


    </script>
    <input id="hdnQScannerfilepath" runat="server" type="hidden" />


      <input id="hdnfilterRequestedFrom" runat="server" type="hidden" />
          <input id="hdnMadbataFile" runat="server" type="hidden" />

    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        نظام الاستجوابات
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

                    <asp:LinkButton runat="server" ID="btnNew" class="btn btn-success btn-xs" OnClick="btnNew_Click1"><i class="fa fa-plus"></i>&nbsp; إضافة إستجواب جديد&nbsp;</asp:LinkButton>

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
                                <div class="form-group" style="display:none">
                                    <label class="col-lg-3 control-label">
                                        مسلسل :</label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="txtFilterserial" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="form-group">
                                    <label class="col-lg-3 control-label">تاريخ  ورود الاستجواب من :</label>
                                    <div class="col-lg-9">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
                                            <asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                        </div>



                                    </div>
                                </div>

                                 <div class="form-group">
                                    <label class="col-lg-3 control-label">تاريخ  جلسة المناقشة  من :</label>
                                    <div class="col-lg-9">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
                                            <asp:TextBox ID="txtFilterQ_DiscussionDateFrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                        </div>



                                    </div>
                                </div>


                                <div class="form-group" style="display:none">
                                    <label class="col-lg-3 control-label">
                                        رقم الصادر بمجلس الامة:

                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterInternalSerial" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <label class="col-lg-2 control-label">
                                        السنة:

                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterFileYear" runat="server" class="form-control"></asp:TextBox>
                                    </div>


                                </div>


                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        جزء من نص الاستجواب :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <asp:TextBox ID="txtFilterSubject" runat="server" class="form-control"></asp:TextBox>

                                        <%--  <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1"
                                            runat="server" TargetControlID="txtFilterSubject"
                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />--%>
                                    </div>



                                </div>

                                <div class="form-group" style="display:none">
                                    <label class="col-md-3 control-label" for="">إستجواب موحد: </label>

                                    <div class="col-md-9">

                                        <asp:CheckBox ID="chkFilerIsGroup" runat="server" />


                                    </div>
                                </div>


                                <%--  <div class="form-group">
										<label><span class="text-semibold">Filtering</span> option</label>
										<div class="multi-select-full">
											<select class="multiselect-filtering" multiple="multiple">
												<option value="cheese">Cheese</option>
												<option value="tomatoes">Tomatoes</option>
												<option value="mozarella">Mozzarella</option>
												<option value="mushrooms">Mushrooms</option>
											</select>
										</div>
									</div>--%>
                            </div>

                            <div class="col-md-5">
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">الفصل التشريعي :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterChapter" AutoPostBack="true" class="Select2Drop" runat="server" OnSelectedIndexChanged="lstFilterChapter_SelectedIndexChanged"></asp:DropDownList>
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
                                            <asp:TextBox ID="txtFilterQ_DiscussionDateTo" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>

                                        </div>

                                    </div>
                                </div>

                                <div class="form-group" style="display:none">
                                    <label class="col-lg-3 control-label">
                                         جهات مرتبطة    :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <%--<asp:TextBox ID="txtFilterPersonName" runat="server" class="form-control"></asp:TextBox>--%>
                                        <asp:DropDownList ID="lstFilterRelatedOrgs" class="form-control multiselect-filtering" multiple="multiple" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group" style="display:">
                                    <label class="col-lg-3 control-label">
                                       العضو مقدم الاستجواب :
                                    </label>
                                    <div class="col-lg-9 autoDrop">
                                        <%--<asp:TextBox ID="txtFilterOrg" runat="server" class="form-control"></asp:TextBox>--%>

                                        <asp:DropDownList ID="lstFilterPerson" class="Select2Drop" runat="server"></asp:DropDownList>

                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">
                                        دور الانعقاد:</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterSession" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">نتيجة الاستجواب :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterResult" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">  المستجوب :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterRequestTo" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group" style="display:none">
                                    <label class="col-lg-3 control-label">الموظف المختص :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterAssignedPerson" class="Select2Drop" runat="server"></asp:DropDownList>
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


                                                    <asp:DataGrid ID="grdQuestionsList" runat="server"
                                                        DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="40" class="table datatable-basic dataTable no-footer"
                                                        Width="100%" OnItemDataBound="grdQuestionsList_ItemDataBound" OnItemCommand="grdQuestionsList_ItemCommand">
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

                                                                    <div style="padding-right: 30px">
                                                                        <asp:Label Font-Bold="true" runat="server" ID="Label7" CssClass="black_Lable">
				                          نص الاجابة
                                                                        </asp:Label>
                                                                        <asp:DataGrid ID="grdAnswers" runat="server"
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
                                                                                <asp:BoundColumn DataField="QuestionID" Visible="False"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AnswerDate" HeaderText="تاريخ الاجابة " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="LastActionDate" HeaderText="اخر تحديث  " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="AnswerText" HeaderText="نص الاجابة"></asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="مرفقات">
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>
                                                                                        <a class="label border-left-primary label-striped iframe" target="_blank" href="<%# ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList=["  + gets(Eval("Answerattachments"))+";]" %>" style="<%#showattachment(gets(Eval("Answerattachments")))%>">

                                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                                              نص الاجابة
                                                                                        </a>

                                                                                        <%--   <a href="AnwserAttachments.aspx?DocID=0&QuestionID=<%#Eval("QuestionID")%>&FileID=0" class="btn btn-default btn-xs iframe">
                                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                                           مرفقات
                                                                                        </a>--%>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateColumn>






                                                                            </Columns>
                                                                        </asp:DataGrid>

                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="" Visible="false">
                                                                <ItemStyle HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    <img style="cursor: pointer;" src="/layout/images/plus.gif" alt="" border="0" runat="server" id="imgControl" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="code"></asp:BoundColumn>
                                                              <asp:BoundColumn Visible="false" HeaderText="Q_RequestTo" DataField="Q_RequestTo"></asp:BoundColumn>
                                                            <asp:BoundColumn HeaderText="مسلسل " DataField="Q_Serial" Visible="false"></asp:BoundColumn>
                                                            <%-- <asp:TemplateColumn HeaderText="  رقم الاستجواب ">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />

                                                                <ItemTemplate>
                                                                    <%#Eval("Oma_Serial") %>/  <%#Eval("OmaYear") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>--%>

                                                            <asp:BoundColumn DataField="Q_Date" HeaderText="تاريخ ورود الاستجواب" DataFormatString="{0:dd/MM/yyyy}">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Q_Text" HeaderText="الاستجواب">
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle Width="30%" />
                                                            </asp:BoundColumn>
                                                             <%--   <asp:BoundColumn DataField="RequestedFromNameAr" HeaderText="مقدم الاستجواب  ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:BoundColumn DataField="Q_RequestToNameAr" HeaderText="موجه إلى  ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>
                                                             <asp:BoundColumn DataField="RequestedFrompersons" HeaderText="مقدمي الاستجواب     ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="ChapterNameAr" HeaderText="الفصل التشريعي">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>


                                                            <asp:BoundColumn DataField="SessionNameAr" HeaderText="دور الانعقاد ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>



                                                            <%--                                                            <asp:BoundColumn DataField="LastActionDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="تاريخ اخر تحديث ">
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundColumn>--%>

                                                            <asp:TemplateColumn HeaderText="النتيجة  ">
                                                                <ItemStyle HorizontalAlign="left" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="left" />

                                                                <ItemTemplate>
                                                                    <%# (GetQStatus(ZeroIntergerIFNull(gets(Eval("Q_Result")).ToString()),gets(Eval("ResultNameAr")))) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>



                                                            <asp:TemplateColumn HeaderText="التفاصيل">
                                                                <ItemStyle HorizontalAlign="right" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div style="text-align: right">

                                                                          <a href="<%#ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath+gets(Eval("Code"))+"/" + "&vfileList=[" + gets(Eval("Q_Attachment"))+";]"%>" style="font-size: 12px; <%#showattachment(gets(Eval("Q_Attachment")))%>" class="label border-left-primary label-striped iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                          نص الاستجواب
                                                                        </a>

                                                                    </div>



                                                                     <div style="margin-top: 10px;display:<%#ValidateMadbataExistance(ZeroIntergerIFNull(gets(Eval("Code"))))%>" > <a href="/Modules/Questions/Forms/OmaMadbataLink.aspx?targetid=<%#Eval("Code") %>" style="font-size: 12px; " class="label bg-blue border-left-primary label-striped iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                            مضابط مرتبطة
                                                                        </a>
                                                                    </div>




                                                                    <div style="margin-top: 10px;">
                                                                        <a href="InterrogationData.aspx?QuestionID=<%#Eval("Code")%>" class="label border-left-success label-striped" style="font-size: 12px;">
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
                                <li class="<%=activeTab(1) %>" onclick="setactiveTab(1)"><a href="#badges-tab1" data-toggle="tab"><i class="icon-menu7 position-left"></i>بيانات الاستجواب</a></li>
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
                                                            بيانات  الاستجواب


                                                        </legend>

                                                        <div class="row">

                                                            <div class="col-md-4">
                                                                <div class="form-group" style="display:none">
                                                                    <label class="col-md-4 control-label" for="">مسلسل : <span class="text-danger">*</span> </label>
                                                                    <div class="col-md-8">

                                                                        <asp:TextBox ID="txtQuestionInternalSerial" class="form-control" runat="server"></asp:TextBox>

                                                                    </div>


                                                                </div>


                                                                <div class="form-group" style="display:none">
                                                                    <label class="col-lg-4 control-label" style="font-size: 14px;">
                                                                        رقم الصادر بمجلس الامة  :

                                                                    </label>
                                                                    <div class="col-lg-3">
                                                                        <asp:TextBox ID="txtOma_Serial" class="form-control" runat="server"></asp:TextBox>
                                                                    </div>
                                                                    <label class="col-lg-2 control-label">
                                                                        السنة:

                                                                    </label>
                                                                    <div class="col-lg-2">
                                                                        <asp:TextBox ID="txtQoOmaYear" class="form-control" runat="server"></asp:TextBox>
                                                                    </div>


                                                                </div>


                                                                <div class="form-group">
                                                                    <label class="col-md-4 control-label" for="">الفصل التشريعي:<span class="text-danger">*</span>   </label>

                                                                    <div class="col-md-8">

                                                                        <asp:DropDownList ID="lstChapter" runat="server" class="Select2Drop" OnSelectedIndexChanged="lstChapter_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>


                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-4 control-label" for="">دور الانعقاد:<span class="text-danger">*</span> </label>

                                                                    <div class="col-md-8">

                                                                        <asp:DropDownList ID="lstSession" AutoPostBack="false" runat="server"  class="Select2Drop" ></asp:DropDownList>

                                                                    </div>
                                                                </div>


                                                            </div>
                                                            <div class="col-md-5">

                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">تاريخ ورود الاستجواب:<span class="text-danger">*</span>  </label>

                                                                    <div class="col-md-9">

                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>

                                                                            <asp:TextBox ID="txtQuestionDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

                                                                        </div>

                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">الاعضاء مقدمي الاستجواب    : <span class="text-danger">*</span></label>

                                                                    <div class="col-md-9">
                                                                        <asp:DropDownList ID="lstRequestFrom" Visible="false" runat="server" class="Select2Drop"></asp:DropDownList>
                                                                           <asp:DropDownList ID="lstRelatedOrgs" class="select-border-color border-warning" multiple="multiple"   Width="100%"   runat="server"></asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">المستجوب   :<span class="text-danger">*</span> </label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstRequestTo" runat="server" class="Select2Drop"></asp:DropDownList>


                                                                    </div>
                                                                </div>


                                                            </div>

                                                            <div class="col-md-3">


                                                                 <div class="form-group" style="display:none">
                                                                    <label class="col-md-3 control-label" for="">الموظف المختص:<span class="text-danger">*</span> </label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstassignedPersons" runat="server" class="Select2Drop"></asp:DropDownList>


                                                                    </div>
                                                                </div>

                                                                  <div class="form-group">
                                                                    <label class="col-md-4 control-label" for="">تاريخ جلسة المناقشة   :   </label>

                                                                    <div class="col-md-8">

                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>

                                                                            <asp:TextBox ID="txtQ_DiscussionDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                                 <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">نتيجة الاستجواب: <span class="text-danger">*</span> </label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstResult" runat="server" class="Select2Drop" onchange="viewresult()" ></asp:DropDownList>


                                                                    </div>
                                                                </div>





                                                                 <div class="form-group" id="vote1" runat="server" style="display:none">

                                                                     <label class="col-md-3 control-label" for="">تصويت مع   :   </label>

                                                                    <div class="col-md-3">

                                                                         <asp:TextBox ID="txtVoteWith" class="form-control" runat="server"></asp:TextBox>


                                                                    </div>
                                                                    <label class="col-md-3 control-label" for="">تصويت ضد   :   </label>

                                                                    <div class="col-md-3">

                                                                         <asp:TextBox ID="txtVoteAgenest" class="form-control" runat="server"></asp:TextBox>


                                                                    </div>


                                                                </div>

                                                                 <div class="form-group"  id="vote2" runat="server" style="display:none">

                                                                       <label class="col-md-3 control-label" for="">  ممتنعين   :   </label>

                                                                    <div class="col-md-3">

                                                                         <asp:TextBox ID="txtVoteVoid" class="form-control" runat="server"></asp:TextBox>


                                                                    </div>

                                                                    <label class="col-md-3 control-label" for="">  متغيبين   :   </label>

                                                                    <div class="col-md-3">

                                                                         <asp:TextBox ID="txtVoteabsent" class="form-control" runat="server"></asp:TextBox>


                                                                    </div>
                                                                </div>






                                                            </div>

                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-8">



                                                                <div class="form-group">
                                                                    <label class="col-md-2 control-label" for="">محاور الاستجواب <span class="text-danger">*</span>:</label>

                                                                    <div class="col-md-10 autoDrop">
                                                                        <asp:TextBox runat="server" ID="txtQuestionSubject" class="form-control" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                        <%-- <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
                                                                            runat="server" TargetControlID="txtQuestionSubject"
                                                                            CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                                            ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="SubjectAutoCompete" />--%>
                                                                    </div>


                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-2 control-label" for="">ملاحظات:</label>

                                                                    <div class="col-md-10">

                                                                        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtQuestionNote" class="form-control"></asp:TextBox>

                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-2 control-label">ملف الاستجواب:</label>

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
                                                         <div class="col-md-4" style="display:<%=viewMadabata() %>" >
                                                        <div class="form-group">
                                                   <%--     <label class="col-md-3 control-label" for="">   : </label>--%>

                                                        <div class="col-md-8">



                                                        </div>
                                                            <div class="col-md-4">





                                                            </div>
                                                        </div>

                                                         </div>


                                                            <div class="col-md-4" style="display:none">

                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">إستجواب موحد: </label>

                                                                    <div class="col-md-9">

                                                                        <asp:CheckBox ID="chkIsGrouped" runat="server" />


                                                                    </div>
                                                                </div>




                                                            </div>
                                                        </div>




                                                        <%--                                                        <legend class="text-semibold">
                                                            <i class="icon-file-text2 position-left"></i>
                                                            أسماء
                                                        </legend>--%>


                                                        <div class="col-md-6" style="display:none">
                                                            <asp:UpdatePanel runat="server" ID="Updatepanel2" ChildrenAsTriggers="true" UpdateMode="conditional">
                                                                <ContentTemplate>


                                                                    <asp:Button UseSubmitBehavior="false" runat="server" ID="btnAddNewItem" Text="Add Item" Style="display: none;" OnClick="btnAddNewItem_Click" />

                                                                    <div class="panel-heading">
                                                                        <h5 class="panel-title">مقدم الاستجواب </h5>

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


                                                                            <asp:TemplateColumn HeaderText="   الاسم">
                                                                                <HeaderStyle Wrap="false" />
                                                                                <%-- <ItemStyle Width="20%" />--%>
                                                                                <ItemTemplate>
                                                                                    <%#Eval("PersonName") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:DropDownList ID="lstRequestedFromPareon" class="Select2Drop form-control" Width="100%" runat="server"></asp:DropDownList>
                                                                                    <input id="hdnrequestedfromPerson" value='<%#Eval("PersonID") %>' runat="server" type="hidden" />
                                                                                </EditItemTemplate>

                                                                            </asp:TemplateColumn>


                                                                            <asp:TemplateColumn HeaderText="  الاسم" Visible="false">

                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate>
                                                                                    <%#Eval("PersonName") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtname" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                                                    <input id="hdnPerson_NameAr" value='<%#Eval("PersonName") %>' runat="server" type="hidden" />

                                                                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
                                                                                        runat="server" TargetControlID="txtname"
                                                                                        CompletionInterval="10" CompletionSetCount="10" ContextKey="<%#ZeroIntergerIFNull(lstChapter.SelectedValue)%>" MinimumPrefixLength="1"
                                                                                        ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="OmaPersonsAutoComplete" />

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
                                                                            <asp:TemplateColumn HeaderText=" ملاحظات" Visible="false">

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

                                                        <div class="col-md-6" style="display:none">

                                                            <asp:UpdatePanel runat="server" ID="Updatepanel3" ChildrenAsTriggers="true" UpdateMode="conditional">
                                                                <ContentTemplate>


                                                                    <asp:Button UseSubmitBehavior="false" runat="server" ID="Button1" Text="Add Item" Style="display: none;" OnClick="btnAddNewItem_Click" />

                                                                    <div class="panel-heading">
                                                                        <h5 class="panel-title">الجهات المعنيه </h5>

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


                                                                            <asp:TemplateColumn HeaderText=" الجهة">
                                                                                <HeaderStyle Wrap="false" />
                                                                                <%-- <ItemStyle Width="20%" />--%>
                                                                                <ItemTemplate>
                                                                                    <%#Eval("PersonName") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:DropDownList ID="lstRelatedOrgs" class="Select2Drop form-control" Width="100%" runat="server"></asp:DropDownList>
                                                                                    <input id="hdnRelatedOrgs" value='<%#Eval("PersonID") %>' runat="server" type="hidden" />
                                                                                </EditItemTemplate>

                                                                            </asp:TemplateColumn>

                                                                            <asp:TemplateColumn HeaderText="  الجهة" Visible="false">

                                                                                <HeaderStyle HorizontalAlign="right" />
                                                                                <ItemTemplate>
                                                                                    <%#Eval("PersonName") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtDname" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                                                    <input id="hdnPerson_NameAr" value='<%#Eval("PersonName") %>' runat="server" type="hidden" />

                                                                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2"
                                                                                        runat="server" TargetControlID="txtDname"
                                                                                        CompletionInterval="10" CompletionSetCount="10" MinimumPrefixLength="1"
                                                                                        ServicePath="/modules/autocomplete/Services/TextAutoComplete.asmx" ServiceMethod="OrgAutoCompete" />

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
                                                                            <asp:TemplateColumn HeaderText=" ملاحظات" Visible="false">

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
                                                            <div class="col-md-offset-4 col-md-12">


                                                                <asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary" OnClick="btnSave_Click1"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>



                                                                &nbsp;
				                                               <asp:Button runat="server" ID="btnCancel" class="btn btn-default" Text=" الغاء / رجوع  " OnClick="btnCancel_Click" />
                                                                &nbsp;
                                                             <a id="anchorAttachment" visible="false" runat="server" href='#' class="btn btn-success btn-xs iframe">
                                                                 <i class="icon-attachment"></i>&nbsp; عرض نص الاستجواب</a>

                                                                 &nbsp;
                                                                <a href="OmaMadbataLink.aspx?targetid=<%=ViewState["itemID"] %>" class="iframe linkedpopup btn btn-danger btn-labeled">
                                                                    <%= hasMadbata?"مضابط مرتبطه":" ربط الاستجواب بمضبطة" %>

                                                                <b><i class="glyphicon glyphicon-link"></i></b>

                                                            </a>


                                                                  &nbsp;

                                                                    <a id="lnkMadbata" runat="server" visible="false" href="#" style="font-size: 12px;display: <%=viewMadbatafile(gets(hdnMadbataFile.Value))%>" class="btn btn-success btn-labeled iframe">
                                                                        <b><i class="icon-attachment"></i></b>
                                                                          ملف الجلسة
                                                                    </a>


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
