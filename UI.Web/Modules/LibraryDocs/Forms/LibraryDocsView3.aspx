<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LibraryDocsView3.aspx.cs" Inherits="UI.Web.LibraryDocs.Forms.LibraryDocsView3" %>

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



    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <%--<i class=""><img src="/Modules/LibraryDocs/Forms/lib_img/ico.png" width="30px" /></i>--%>
                         <i class=""><img src="/layout/rtl/assets/images/lib_ico.png" /></i>
                        <%= Resources.menu.Library %>
                    </h4>
                </div>

            </div>

        </div>
    </div>


<%--    <div class="row">
        <div class="col-lg-12">
            <table cellpadding="0" cellspacing="0" style="border:0px;">
               <tr>
                   <td style="background:url('/Modules/LibraryDocs/Forms/lib_img/shelves_right.png') repeat-y top center;margin:0px;padding:0px;width:52px;"></td>
                   <td style="background:url('/Modules/LibraryDocs/Forms/lib_img/shelves_m.png') top center;margin:0px;padding:0px;height:400px;width:1122px"></td>
                   <td style="background:url('/Modules/LibraryDocs/Forms/lib_img/shelves_left.png') repeat-y top center;margin:0px;padding:0px;width:50px;"></td>

               </tr>

            </table>

        </div>
    </div>--%>




    <asp:UpdatePanel runat="server" ID="Updatepanel1" ChildrenAsTriggers="true" UpdateMode="conditional">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!--END TITLE & BREADCRUMB PAGE-->
    <!--BEGIN CONTENT-->

    <div class="row mbl" id="tblSearch" style="" runat="server">
          <div class="panel panel-flat" style="min-height:750px;background:#fff">
            <div class="">
                    <div class="pull-left" id="divback" runat="server" style="position:absolute;z-index:9999"> <a href="javascript:void(0)"  runat="server" id="libback"><img src="/Modules/LibraryDocs/Forms/lib_img/back.png" /></a></div>
                <div  class="pull-right" id="divtxtSearch" runat="server" style="position:absolute;left:20px;z-index:9999">
                    <div class="form-group has-feedback has-feedback-left" style="float:right">
                            <asp:TextBox ID="txtpartName" runat="server" class="form-control" placeholder="كلمة فى اسم الوثيقة"></asp:TextBox>
                        <div class="form-control-feedback">
                            <i class="icon-search4 text-size-base"></i>
                        </div>



                    </div>
                    <div style="float:left">  <asp:LinkButton ID="lnkSearch" class="btn btn-primary" runat="server" OnClick="lnkSearch_Click1"> بحث  </asp:LinkButton></div>
                 </div>
                  </div>


                <div class="panel-body" style="min-height:400px">
         <div class="form-horizontal">

                        <asp:Label runat="server" ID="lblerror"></asp:Label>

                        <fieldset class="content-group" style="display: none">
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


                        <div class="category-content">
                            <div class="row">

                                <asp:Repeater runat="server" ID="rptCategory">
                                    <ItemTemplate>
                                        <div class="col-xs-4"  >
                                            <a  style="min-height:110px;"  href="LibraryDocsview3.aspx?catid=<%# Eval("code")%>">
                                             <%--<div style="text-align:center;min-height:70px"> <img src="/layout/rtl/assets/images/CatIco.png"  /></div>--%>
                                            <div style="text-align: center;min-height: 150px;padding:10px;border-radius: 5px;width: 70%;margin: 0 auto;margin-bottom:20px;"> <img src="/Modules/LibraryDocs/Forms/lib_img/<%#(gets(Eval("img")).Equals("")?"cmgs.jpg":Eval("img"))%>" style="max-width:150px;" /></div>
                                            <div style="font-size:26px;font-weight:bold;text-align:center;color:#0d3159;margin-bottom:30px;min-height:57px;">

                                            <%#Eval("NameAr") %></div></a>

                                        </div>
                                    </ItemTemplate>

                                </asp:Repeater>



                                <asp:Repeater runat="server" ID="rptTypes">
                                    <ItemTemplate>
                                        <div class="col-xs-3" style="margin-top: 5px;">
                                            <a  style="min-height:110px;" class="" href="LibraryDocsview3.aspx?catid=<%#Eval("catid") %>&typeid=<%#Eval("code") %>">

                                                <div class="" style="text-align:center;min-height:70px"><img src="/Modules/LibraryDocs/Forms/lib_img/<%#(gets(Eval("img")).Equals("")?"cmgs.jpg":Eval("img"))%>"   style="margin-bottom:20px;max-width:150px;height:200px;" /></div>  <div style="font-size:16px;text-align:center;color:#0d3159;min-height:57px;"><%#Eval("NameAr") %></div></a></div>
                                    </ItemTemplate>

                                </asp:Repeater>


                                <asp:Repeater runat="server" ID="rptDocs">
                                    <ItemTemplate>
                                        <div class="col-xs-2" style="margin-top: 5px;min-height:300px;height:300px">
                                            <a class="iframe" href="<%# ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "/"+gets(Eval("Code"))+"/&vfileList=["  + gets(Eval("Filepath"))+";]"%>" style="min-height:130px;<%#showattachment(gets(Eval("Filepath")))%>"><div class="" style="text-align:center"> <img src="<%#(gets(Eval("file_Img")).Equals("")?"/Layout/RTL/assets/images/Icons/doc.png":"/Modules/LibraryDocs/Forms/lib_img/"+ Eval("file_Img"))%>"  style="max-width:150px;" /></div><div style="color:#0d3159;text-align:center;margin-top:20px;">
                                                  <%--<p style="color:#045588"> تاريخ الوثيقة : <%#NullDateifEmptyToText( Eval("ReceiveDate") )%></p>--%>
                                                <%#Eval("DocSubject") %>



                                            </div></a>
                                        </div>
                                    </ItemTemplate>

                                </asp:Repeater>


                            </div>
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
                                                                    <a target="_blank" href="<%# ScannerRepositoryViewer + "?targetpath=" + _TargetUploadPath + "&vfileList=["  + gets(Eval("Filepath"))+";]"%>" class="label border-left-primary label-striped iframe" style="<%#showattachment(gets(Eval("Filepath")))%>">
                                                                        <i class="icon-download"></i>&nbsp; عرض
                                                                    </a>

                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>


                                                            <asp:TemplateColumn HeaderText="التفاصيل">
                                                                <ItemStyle HorizontalAlign="left" />
                                                                <HeaderStyle Wrap="False" HorizontalAlign="left" />

                                                                <ItemTemplate>
                                                                    <a href="LibraryDocs.aspx?id=<%#Eval("code") %>" class="btn btn-default btn-xs">
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




                                            <div class="row" style="margin-bottom: 15px;">
                                                <div class="col-md-12">

                                                    <label class="col-md-1 control-label" for="">الموضوع <span class="text-danger">*</span>  </label>

                                                    <div class="col-md-11">
                                                        <asp:TextBox runat="server" ID="txtSubject" TextMode="MultiLine" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">

                                                <div class="col-md-5">
                                                    <div class="form-group" style="display: none">
                                                        <label class="col-md-3 control-label" for="">مسلسل<span class="text-danger">*</span></label>

                                                        <div class="col-md-9">

                                                            <asp:TextBox ID="txtSerial" class="form-control" runat="server"></asp:TextBox>

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

</asp:Content>
