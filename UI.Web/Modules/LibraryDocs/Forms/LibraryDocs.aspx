<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LibraryDocs.aspx.cs" Inherits="UI.Web.LibraryDocs.Forms.LibraryDocs" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/DeleteConfirm.ascx"  TagPrefix="uc"  TagName="DeleteConfirm" %>
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

        function chkImage() {

            var txt = document.getElementById("<%=txtSubject.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ،ادخل الموضوع");
                txt.focus();
                return false;
            }

<%--            var txt = document.getElementById("<%=txtSerial.ClientID %>")
            if (txt.value == "") {
                new $.Zebra_Dialog("فضلا ، ادخل  المسلسل");
                txt.focus();
                return false;
            }--%>

            var txt = document.getElementById("<%=lstTypeCode.ClientID %>")
            if (txt.value == "" || txt.value == "0") {
                new $.Zebra_Dialog("فضلا ، ادخل اختر نوع الملف  ");
                txt.focus();
                return false;
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
                        <%= Resources.menu.Library %>
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

                            <div class="col-md-5">

                                <div class="form-group" style="display: none">
                                    <label class="col-lg-3 control-label">
                                        مسلسل :
                                    </label>
                                    <div class="col-lg-3">
                                        <asp:TextBox ID="txtFilterSerial" runat="server" class="form-control"></asp:TextBox>


                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">التصنيف :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstfilterDocCategory" AutoPostBack="true" OnSelectedIndexChanged="lstfilterDocCategory_SelectedIndexChanged" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>


                                <div class="form-group">
                                    <label class="col-lg-3 control-label">النوع :</label>
                                    <div class="col-lg-9">
                                        <asp:DropDownList ID="lstfilterDocType" class="Select2Drop" runat="server"></asp:DropDownList>
                                    </div>
                                </div>


                                <div class="form-group" style="display: none">
                                    <label class="col-lg-3 control-label">تاريخ الرفع من</label>
                                    <div class="col-lg-9">




                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <%--	<input class="form-control pickadate-selectors picker__input picker__input--active" value="03/18/2013" type="text">--%>
                                            <asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>



                                        </div>



                                    </div>
                                </div>

                                <div class="form-group" style="display: none">
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
                                        كلمة فى الموضوع
                                    </label>
                                    <div class="col-lg-9">
                                        <asp:TextBox ID="txtPartofName" runat="server" class="form-control"></asp:TextBox>


                                    </div>
                                </div>



                            </div>


                            <div class="col-md-4" id="divFilterSession" runat="server" visible="false">

                                <div class="form-group">
                                    <label class="col-md-3 control-label" for="">الفصل التشريعي:   </label>

                                    <div class="col-md-9">

                                        <asp:DropDownList ID="lstFilterChapter" runat="server" class="Select2Drop" OnSelectedIndexChanged="lstFilterChapter_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>


                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label" for="">دور الانعقاد: </label>

                                    <div class="col-md-9">

                                        <asp:DropDownList ID="lstFilterSession" AutoPostBack="false" runat="server" class="Select2Drop"></asp:DropDownList>

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
                                                        <asp:Label ID="lblSearchResultCount" runat="server"></asp:Label>)</span>

                                                    <asp:LinkButton ID="lnkSearchback" CssClass="control-arrow" runat="server" OnClick="lnkSearchback_Click"> رجوع  <i class=" icon-backward2"></i></asp:LinkButton>

                                                </legend>




                                                <div class="datatable-scroll">
                                                    <div class="dataTables_info" id="DataTables_Table_3_info" role="status" aria-live="polite">
                                                        <%--                                                        <asp:Label ID="lblCount2" runat="server"></asp:Label>--%>
                                                    </div>
                                                    <asp:DataGrid ID="grdInboundItems" runat="server"
                                                        DataKeyField="code" AllowPaging="True" AutoGenerateColumns="False" PageSize="20" class="table datatable-basic dataTable no-footer"
                                                        Width="100%" OnItemDataBound="grdInboundItems_ItemDataBound" OnItemCommand="grdInboundItems_ItemCommand">
                                                        <SelectedItemStyle ForeColor="White" />
                                                        <ItemStyle CssClass="grdItem" />
                                                        <AlternatingItemStyle CssClass="grdItem" />
                                                        <PagerStyle Visible="false" />
                                                        <HeaderStyle CssClass="grdHead" BackColor="Black" ForeColor="White" Font-Bold="False" />
                                                        <Columns>

                                                            <asp:BoundColumn Visible="false" HeaderText="Code" DataField="code"></asp:BoundColumn>

                                                            <%-- <asp:BoundColumn DataField="DocRef" HeaderText="مسلسل ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:BoundColumn DataField="Library_DocsCategoryNameAr" HeaderText="التصنيف  ">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="DocTyepNameAr" HeaderText="نوع الوثيقة">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="DocSubject" HeaderText=" الموضوع ">
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle Width="50%" />
                                                            </asp:BoundColumn>

                                                            <%--<asp:BoundColumn DataField="UploadDate" HeaderText="تاريخ الرفع" DataFormatString="{0:dd/MM/yyyy}">
                                                                <HeaderStyle Wrap="false" />
                                                            </asp:BoundColumn>--%>



                                                            <asp:TemplateColumn HeaderText="عرض  ">
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <a target="_blank" href="<%# ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath+gets(Eval("code"))+"/" + "&vfileList=["  + gets(Eval("Filepath"))+";]"%>" class="label border-left-primary label-striped iframe" style="<%#showattachment(gets(Eval("Filepath")))%>">
                                                                        <i class="icon-download"></i>&nbsp; عرض
                                                                    </a>

                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:TemplateColumn HeaderText="التفاصيل">
                                                                <ItemStyle HorizontalAlign="left" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="left" />

                                                                <ItemTemplate>
                                                                    <a href="LibraryDocs.aspx?id=<%#Eval("code") %>&editflag=1" class="btn btn-default btn-xs">
                                                                        <i class="fa fa-file"></i>&nbsp;
                                                                         التفاصيل
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:BoundColumn DataField="ReceiveDate" HeaderText="تاريخ الوثيقة    " DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>


<%--                                                            <asp:TemplateColumn>
                                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('are you sure you want to delete selected items?');" CommandName="delete" runat="server">  <i class="fa fa-trash" style="color:##333"></i>&nbsp;</asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>--%>
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

        <div class="panel-heading" id="tblAdd" runat="server" visible="false">



            <div class="row">
                <asp:Label runat="server" ID="lblAdderror"></asp:Label>
                <div class="panel">

                    <div class="panel-body">
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
                                        <fieldset class="content-group">
                                            <legend class="text-semibold">
                                                <i class="icon-file-text2 position-left"></i>
                                                بيانات الملف
											<%--<a class="control-arrow" data-toggle="collapse" data-target="#demo1">
                                                <i class="icon-circle-down2"></i>
                                            </a>--%>
                                            </legend>




                                           
                                            <div class="row">

                                                <div class="col-md-5">
                                                    <div class="form-group" style="display: none">
                                                        <label class="col-md-3 control-label" for="">مسلسل<span class="text-danger">*</span></label>

                                                        <div class="col-md-9">

                                                            <asp:TextBox ID="txtSerial" class="form-control" runat="server"></asp:TextBox>

                                                        </div>
                                                    </div>

                                                    
                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label" for="">الموضوع<span class="text-danger">*</span></label>

                                                        <div class="col-md-9">
                                                              <CKEditor:CKEditorControl ID="txtSubject" runat="server" ContentsLangDirection="Rtl"></CKEditor:CKEditorControl>

                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label" for="">التصنيف<span class="text-danger">*</span></label>

                                                        <div class="col-md-9">
                                                            <asp:DropDownList ID="lstDocCategory" AutoPostBack="true" OnSelectedIndexChanged="lstDocCategory_SelectedIndexChanged" runat="server" class="Select2Drop"></asp:DropDownList>

                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label" for="">النوع<span class="text-danger">*</span></label>

                                                        <div class="col-md-9">
                                                            <asp:DropDownList ID="lstTypeCode" runat="server" class="Select2Drop"></asp:DropDownList>

                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label class="col-lg-3 control-label">تاريخ الوثيقة  </label>
                                                        <div class="col-lg-9">




                                                            <div class="input-group">
                                                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>

                                                                <asp:TextBox ID="txtReciveDate" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>



                                                            </div>



                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label">الملف  </label>

                                                        <div class="col-md-5">
                                                            <asp:Label ID="Label3" runat="server"></asp:Label>
                                                            <asp:FileUpload ID="txtImage" runat="server" class="file-styled" />
                                                            <span class="help-block">Accepted formats: gif, png, jpg,pdf,doc.</span>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <span class="help-block2">| Or | </span>
                                                            <asp:LinkButton runat="server" ID="btnScan" OnClientClick="return Validateoutgoing();" OnClick="btnOutgoingScan_Click" class="btn btn-info btn-xs"><i class="icon-images2"></i>&nbsp;  تصوير&nbsp;</asp:LinkButton>
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label">الصورة:</label>

                                                        <div class="col-md-9">
                                                            <asp:Label ID="lblimage" runat="server"></asp:Label>
                                                            <asp:FileUpload ID="lblDocImage" runat="server" class="file-styled" />
                                                            <span class="help-block">Accepted formats: gif, png, jpg</span>
                                                        </div>


                                                    </div>


                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label">ملف خاص </label>

                                                        <div class="col-md-9">

                                                            <asp:CheckBox ID="chkIsPrivate" runat="server" />
                                                        </div>

                                                    </div>

                                                </div>


                                                <div class="col-md-3" id="divChapterSessions" runat="server" visible="false">

                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label" for="">الفصل التشريعي:<span class="text-danger">*</span>   </label>

                                                        <div class="col-md-9">

                                                            <asp:DropDownList ID="lstChapter" runat="server" class="Select2Drop" OnSelectedIndexChanged="lstChapter_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>


                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-3 control-label" for="">دور الانعقاد:<span class="text-danger">*</span> </label>

                                                        <div class="col-md-9">

                                                            <asp:DropDownList ID="lstSession" AutoPostBack="false" runat="server" class="Select2Drop"></asp:DropDownList>

                                                        </div>
                                                    </div>

                                                </div>


                                                <div class="col-md-12">
                                                    <div class="form-actions">
                                                        <div class="col-md-offset-6 col-md-12">


                                                            <asp:LinkButton ID="btnSave" runat="server" class="btn btn-primary" OnClick="btnSave_Click1"><i class='fa fa-save'></i>&nbsp; حفظ البيانات </asp:LinkButton>


                                                            &nbsp;
				                               <asp:Button runat="server" ID="btnCancel" class="btn btn-default" Text=" الغاء / رجوع  " OnClick="btnCancel_Click" />

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

    <!--END CONTENT-->
    <!--BEGIN FOOTER-->
       <uc:DeleteConfirm runat="server" />
</asp:Content>
