<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="Frm_Pm_Letters.aspx.cs" Inherits="UI.Web.Modules.PM.Forms.Frm_Pm_Letters" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/DeleteConfirm.ascx"  TagPrefix="uc"  TagName="DeleteConfirm" %>

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



        }
        function ControlGrid(imgName, rowIndex, rowID) {

            rowIndex = rowIndex + 3;

            var myrow = "";
            if (rowIndex < 10)
                myrow = "ctl00_Main_grdLetter_ctl0" + rowIndex;
            else
                myrow = "ctl00_Main_grdLetter_ctl" + rowIndex;
            var row = document.getElementById(myrow);

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

            if (list != "") {
                var data = list.split(",");

                for (var i = 0; i < data.length; i++) {
                    document.getElementById(data[i]).checked = obj.checked;
                }
            }
        }



        function chkImage() {



            var txt = document.getElementById("<%=txtLetterSerial.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  رقم الكتاب ");
                txt.focus();
                return false;
            }



            var txt = document.getElementById("<%=txtLetterDate.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ،  اختر تاريخ الكتاب ");
                txt.focus();
                return false;
            }




            var txt = document.getElementById("<%=lstCategory.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، اختر الفصل التشريعي  ");
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




    </script>

    <input id="hdnScannerfilepath" runat="server" type="hidden" />

      <input id="hdnfilterRequestedFrom" runat="server" type="hidden" />
    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        نظام كتب ديوان سمو رئيس مجلس الوزراء
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

                    <asp:LinkButton runat="server" ID="btnNew" class="btn btn-success btn-xs" OnClick="btnNew_Click1"><i class="fa fa-plus"></i>&nbsp; إضافة كتاب  &nbsp;</asp:LinkButton>

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
                                        رقم الكتاب :</label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="txtFilterserial" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                     <div class="form-group" style="display:none">
                                                                    <label class="col-md-4 control-label" for="">  مسلسل : <span class="text-danger">*</span>

                                                                    </label>
                                                                    <div class="col-md-8">

                                                                        <asp:TextBox ID="txtFilterNum" class="form-control" runat="server"></asp:TextBox>

                                                                    </div>


                                                                </div>
                                                                <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">  السنة :

                                                                    </label>
                                                                    <div class="col-md-9">

                                                                        <asp:TextBox ID="txtFilterYear" class="form-control" runat="server"></asp:TextBox>

                                                                    </div>


                                                                </div>
                                 <div class="form-group">
                                                                    <label class="col-md-3 control-label" for="">الفصل التشريعي:   </label>

                                                                    <div class="col-md-9">

                                                                        <asp:DropDownList ID="lstFilterChapter" runat="server" class="Select2Drop"></asp:DropDownList>


                                                                    </div>
                                                                </div>





                            </div>

                            <div class="col-md-4">



                                  <div class="form-group">
                                    <label class="col-lg-3 control-label">  التصنيف :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstFilterCategory"  class="Select2Drop" runat="server" ></asp:DropDownList>
                                    </div>
                                </div>
                                  <div class="form-group">
                                    <label class="col-lg-3 control-label">تاريخ  الكتاب من :</label>
                                    <div class="col-lg-9">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control daterange-single" value="03/18/2013" type="text">--%>
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


                            </div>

                            <div class="col-md-4">




                            </div>




                        </fieldset>
                    </div>
                    <div class="text-right">
                        <asp:LinkButton ID="lnkSearch"  class="btn btn-primary" runat="server" OnClick="lnkSearch_Click">&nbsp;&nbsp; بحث&nbsp;&nbsp; <i class="icon-search4 position-right"></i></asp:LinkButton>
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


                                                    <asp:DataGrid ID="grdLetter" runat="server"
                                                        DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="40" class="table datatable-basic dataTable no-footer"
                                                        Width="100%" OnItemDataBound="grdLetter_ItemDataBound" OnItemCommand="grdLetter_ItemCommand">
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


                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="code"></asp:BoundColumn>
                                                             <asp:BoundColumn HeaderText="  مسلسل " DataField="Arc_Num" Visible="false"></asp:BoundColumn>
                                                             <asp:BoundColumn HeaderText="السنة   " DataField="Arc_Year"></asp:BoundColumn>
                                                            <asp:BoundColumn HeaderText="رقم الكتاب " DataField="Arc_Serial">
                                                                <ItemStyle  Width="150px"/>
                                                            </asp:BoundColumn>
                                                              <asp:BoundColumn DataField="Arc_Subject" HeaderText="الموضوع   ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Arc_Date" HeaderText="تاريخ الكتاب" DataFormatString="{0:dd/MM/yyyy}">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="CategoryNameAr" HeaderText="التصنيف  ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ChapterNameAr" HeaderText="الفصل التشريعي">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                         <%--   <asp:BoundColumn DataField="Arc_notes" HeaderText="ملاحظات   ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn HeaderText="  ملف الكتاب">
                                                                <ItemStyle HorizontalAlign="right" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                          <a href="<%#ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + gets(Eval("Code")) + "/" + "&vfileList=[" + gets(Eval("Arc_Attachment"))+";]"%>" style="font-size: 12px; <%#showattachment(gets(Eval("Arc_Attachment")))%>" class="label border-left-primary label-striped iframe">
                                                                            <i class="icon-attachment"></i>&nbsp;
                                                                          ملف الكتاب
                                                                        </a>

                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                            <asp:TemplateColumn HeaderText="التفاصيل">
                                                                <ItemStyle HorizontalAlign="right" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                        <a href="Frm_Pm_Letters.aspx?LetterID=<%#Eval("Code")%>&editflag=1" class="label border-left-success label-striped" style="font-size: 12px;">
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

                                                          <%--  <asp:TemplateColumn>
                                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('are you sure you want to delete selected items?');" CommandName="delete" runat="server">  <i class="fa fa-trash" style="color:##333"></i>&nbsp;</asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>--%>

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
                                <li class="<%=activeTab(1) %>" onclick="setactiveTab(1)"><a href="#badges-tab1" data-toggle="tab"><i class="icon-menu7 position-left"></i>بيانات الكتاب</a></li>

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


                                                        <div class="row">
                                                            <div class="col-md-4">

                                                                <div class="form-group" style="display:none">
                                                                    <label class="col-md-4 control-label" for="">  مسلسل : <span class="text-danger">*</span>

                                                                    </label>
                                                                    <div class="col-md-8">

                                                                        <asp:TextBox ID="txtNum" class="form-control" runat="server"></asp:TextBox>

                                                                    </div>


                                                                </div>
                                                                <div class="form-group">
                                                                    <label class="col-md-4 control-label" for="">  السنة : <span class="text-danger">*</span>

                                                                    </label>
                                                                    <div class="col-md-8">

                                                                        <asp:TextBox ID="txtYear" class="form-control" runat="server"></asp:TextBox>

                                                                    </div>


                                                                </div>




                                                                  <div class="form-group">
                                                                    <label class="col-md-4 control-label" for="">التصنيف  :<span class="text-danger">*</span>   </label>

                                                                    <div class="col-md-8">

                                                                        <asp:DropDownList ID="lstCategory" runat="server" class="Select2Drop"  ></asp:DropDownList>


                                                                    </div>
                                                                </div>
                                                                 <div class="form-group">
                                                                    <label class="col-md-4 control-label" for="">الفصل التشريعي:<span class="text-danger">*</span>   </label>

                                                                    <div class="col-md-8">

                                                                        <asp:DropDownList ID="lstChapter" runat="server" class="Select2Drop" ></asp:DropDownList>


                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">

                                                                <div class="form-group">
                                                                    <label class="col-md-4 control-label" for="">رقم الكتاب : <span class="text-danger">*</span>

                                                                    </label>
                                                                    <div class="col-md-8">

                                                                        <asp:TextBox ID="txtLetterSerial" class="form-control" runat="server"></asp:TextBox>

                                                                    </div>


                                                                </div>
                                                                  <div class="form-group">
                                                                    <label class="col-md-4 control-label" for="">تاريخ الكتاب:<span class="text-danger">*</span>  </label>

                                                                    <div class="col-md-8">

                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>

                                                                            <asp:TextBox ID="txtLetterDate" class="form-control pickadate-selectors picker__input picker__input--active" runat="server"></asp:TextBox>

                                                                        </div>

                                                                    </div>
                                                                </div>




                                                            </div>




                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-8">

                                                                <div class="form-group">
                                                                    <label class="col-md-2 control-label" for="">الموضوع:</label>

                                                                    <div class="col-md-10">

                                                                        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtSubject" class="form-control"></asp:TextBox>

                                                                    </div>
                                                                </div>



                                                                <div class="form-group">
                                                                    <label class="col-md-2 control-label" for="">ملاحظات:</label>

                                                                    <div class="col-md-10">

                                                                        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtLetterNote" class="form-control"></asp:TextBox>

                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <label class="col-md-2 control-label">ملف  الكتاب:</label>

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
                                                                 <i class="icon-attachment"></i>&nbsp; عرض ملف الكتاب  </a>

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
    </div>
    <!--END CONTENT-->
    <!--BEGIN FOOTER-->
    <div id="scanLoading" class="scanLoading" style="display: none">
        <img src="/Layout/images/scan-document.gif" />
    </div>
         <uc:DeleteConfirm runat="server" />
</asp:Content>
