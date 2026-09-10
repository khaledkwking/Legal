<%@ Page Title="" Language="C#" ClientIDMode="AutoID" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="PermissionsNew.aspx.cs" Inherits="UI.Web.Admin.Pages.PermissionsNew" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <script src="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/Basic/Basic.js"></script>
    <script language="javascript" type="text/javascript">
        <%=ViewState["Def"].ToString() %>
    </script>
    <script language="javascript" type="text/javascript">
        function ToggleSystemGroup(imgid, rowlist) {

            var img = document.getElementById(imgid);
            //alert(img);
            if (img.src.indexOf("plus") != -1) // the group is hidden, show it
            {
                img.src = minus.src;
                var data = rowlist.split(",");
                for (var i = 0; i < data.length; i++) {
                    document.getElementById(data[i]).style.display = "";
                    // alert(document.getElementById(data[i]));

                }
            }
            else // the group is shown, hide it
            {
                img.src = plus.src;
                var data = rowlist.split(",");
                for (var i = 0; i < data.length; i++) {
                    document.getElementById(data[i]).style.display = "none";
                }
            }
        }
        function CheckSystem(obj, list) {
            //alert("CHECK SYSTEM: "+obj.checked);
            if (list != "") {
                var data = list.split(",");
                //alert("LIST IS: "+data.length)
                for (var i = 0; i < data.length; i++) {
                    document.getElementById(data[i]).checked = obj.checked;
                }
            }
        }
        function KeepCheck(parent, list) {
            var check = true;
            if (list != "") {
                var data = list.split(",");
                for (var i = 0; i < data.length; i++) {
                    if (!document.getElementById(data[i]).checked) {
                        check = false;
                        break;
                    }
                }
            }
            document.getElementById(parent).checked = check;
        }

        var permissionHeaderToggleState = {
            chkShow: false,
            chkAdd: false,
            chkModify: false,
            chkDelete: false,
            chkAudit: false
        };

        function togglePermissionColumn(checkBoxIdSuffix) {
            var targetState = !permissionHeaderToggleState[checkBoxIdSuffix];
            permissionHeaderToggleState[checkBoxIdSuffix] = targetState;

            var selector = "#<%=grdResult.ClientID %> input[type='checkbox'][id$='" + checkBoxIdSuffix + "']";
            var list = document.querySelectorAll(selector);

            for (var i = 0; i < list.length; i++) {
                if (!list[i].disabled) {
                    list[i].checked = targetState;
                }
            }
        }

        function findHeaderRow(table) {
            if (!table || !table.rows || table.rows.length === 0) return null;
            return table.rows[0];
        }

        function bindHeaderCellClick(cell, suffix) {
            if (!cell) return;
            cell.style.cursor = "pointer";
            cell.title = "اضغط للتحديد/إلغاء التحديد";
            cell.onclick = function () {
                togglePermissionColumn(suffix);
                return false;
            };
        }

        function normalizeArabicText(txt) {
            if (!txt) return "";
            return txt.replace(/\s+/g, " ").trim();
        }

        function findHeaderCellByCaption(headerRow, captions) {
            if (!headerRow || !headerRow.cells) return null;

            for (var i = 0; i < headerRow.cells.length; i++) {
                var cellText = normalizeArabicText(headerRow.cells[i].innerText || headerRow.cells[i].textContent || "");
                for (var j = 0; j < captions.length; j++) {
                    if (cellText === captions[j]) {
                        return headerRow.cells[i];
                    }
                }
            }
            return null;
        }

        function bindPermissionHeaderClicks() {
            var grid = document.getElementById("<%=grdResult.ClientID %>");
            if (!grid) return;

            var headerRow = findHeaderRow(grid);
            if (!headerRow || !headerRow.cells || headerRow.cells.length === 0) return;

            // Bind by header caption (safer than fixed indexes when hidden columns exist)
            bindHeaderCellClick(findHeaderCellByCaption(headerRow, ["عرض"]), "chkShow");
            bindHeaderCellClick(findHeaderCellByCaption(headerRow, ["اضافة", "إضافة"]), "chkAdd");
            bindHeaderCellClick(findHeaderCellByCaption(headerRow, ["تعديل"]), "chkModify");
            bindHeaderCellClick(findHeaderCellByCaption(headerRow, ["حذف"]), "chkDelete");
            bindHeaderCellClick(findHeaderCellByCaption(headerRow, ["تدقيق"]), "chkAudit");
        }

        if (typeof (Sys) !== "undefined" && Sys.Application) {
            Sys.Application.add_load(function () {
                bindPermissionHeaderClicks();
            });
        } else {
            window.onload = function () {
                bindPermissionHeaderClicks();
            };
        }
    </script>


    <!--BEGIN TITLE & BREADCRUMB PAGE-->
    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                      إداره صلاحيات النظام
                    </h4>
                </div>

            </div>

        </div>
    </div>
    <!--END TITLE & BREADCRUMB PAGE-->

    
    <asp:UpdatePanel runat="server" ID="Updatepanel1" ChildrenAsTriggers="true" UpdateMode="conditional">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row mbl">

        <div class="col-lg-12">
            <div class="panel">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div style="padding-top: 10px; height: 100%;">


                                <div style="float: left">

                                    <asp:LinkButton ID="lnkSave" CssClass='btn btn-primary' runat="server"
                                        OnClick="lnkSave_Click"> حفظ التغيرات</asp:LinkButton>


                                    <asp:LinkButton ID="lbkColse" CssClass='btn btn-danger' runat="server"
                                        OnClick="lbkColse_Click">حذف الصلاحيات </asp:LinkButton>
                                </div>

                                <asp:Label runat="server" ID="lblError"></asp:Label>

                                <div id="AddDiv" align='left' style="display: block; padding-top: 5px;" runat="server">
                                    <table border="0" width="100%" class="form-group">
                                        <tr>
                                            <td style="width: 80px;">
                                                <asp:Label Style="white-space: nowrap;" ID="Label1" runat="server" CssClass="black_Lable">
			                    اختر نوع المستخدم:
                                                </asp:Label>
                                            </td>
                                            <td style="width: 200px" class="form-group">
                                                <asp:DropDownList runat="server" ID="lstJob" Width="250px" AutoPostBack="True" class="form-control" OnSelectedIndexChanged="lstJob_SelectedIndexChanged"
                                                  >
                                                </asp:DropDownList>
                                            </td>
                                            <td>&nbsp;&nbsp;</td>
                                            <td style="width: 90px">
                                                <asp:Label Style="white-space: nowrap;" ID="Label2" runat="server" CssClass="black_Lable">
			                        المستخدم:
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="lstMember" Width="250px" CssClass="form-control"
                                                    AutoPostBack="True" OnSelectedIndexChanged="lstMember_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <hr style="color: #DDDDDD;" />
                                               <div class="datatable-scroll">
                                                    <asp:DataGrid ID="grdResult" runat="server" ClientIDMode="AutoID"
                                                        CssClass="table datatable-basic dataTable no-footer" ShowFooter="True"
                                                        PageSize="15" AutoGenerateColumns="False" Font-Names="Tahoma" Width="100%"
                                                        OnItemDataBound="grdResult_ItemDataBound">
                                                        <ItemStyle />
                                                        <AlternatingItemStyle />
                                                        <HeaderStyle BackColor="#bdc7d5" Font-Bold="true" />
                                                        <FooterStyle BackColor="#bdc7d5" />
                                                        <PagerStyle Mode="NextPrev"
                                                            PrevPageText="&lt;&lt; Previous &nbsp;&nbsp;&nbsp;"
                                                            NextPageText="&nbsp;&nbsp;&nbsp;Next&gt;&gt;" />
                                                        <Columns>
                                                            <asp:BoundColumn DataField="SystemID" HeaderText="SystemID" Visible="false"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PageID" HeaderText="PageID" Visible="false"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PermissionID" HeaderText="PermissionID" Visible="false"></asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="System Title">
                                                                <HeaderStyle Wrap="false" Width="2px"></HeaderStyle>
                                                                <ItemStyle />
                                                                <ItemTemplate>
                                                                    <table border="0">
                                                                        <tr>
                                                                            <td style="border-style: none;">
                                                                                <img alt="" runat="server" id="imgSystem" src="/Layout/Assets/Basic/plus.gif" style="cursor: pointer;" />
                                                                            </td>
                                                                            <td style="border-style: none;" class="gg1">
                                                                                <a href="javascript:void(0);" style="color: #555555; text-decoration: none;" runat="server" id="lnkSystem"><%#Eval("SystemTitle")%></a>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="PageTitle" HeaderText="الصلاحية">
                                                                <HeaderStyle Wrap="false"></HeaderStyle>
                                                                <ItemStyle />
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="عرض">
                                                                <HeaderStyle Wrap="false" HorizontalAlign="center" Width="10%" />
                                                                <ItemStyle Wrap="false" HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox Style="border-style: none;" CssClass="check" runat="server" ID="chkShow" Checked='<%#getBool(DataBinder.Eval(Container.DataItem,"show")) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="اضافة ">
                                                                <HeaderStyle Wrap="false" HorizontalAlign="center" />
                                                                <ItemStyle Wrap="false" HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox Style="border-style: none;" CssClass="check" runat="server" ID="chkAdd" Checked='<%#getBool(DataBinder.Eval(Container.DataItem,"AddRecord")) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="تعديل" >
                                                                <HeaderStyle Wrap="false" HorizontalAlign="center" />
                                                                <ItemStyle Wrap="false" HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox Style="border-style: none;" CssClass="check" runat="server" ID="chkModify" Checked='<%#getBool(DataBinder.Eval(Container.DataItem,"modify")) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="حذف" >
                                                                <HeaderStyle Wrap="false" HorizontalAlign="center" />
                                                                <ItemStyle Wrap="false" HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox Style="border-style: none;" CssClass="check" runat="server" ID="chkDelete" Checked='<%#getBool(DataBinder.Eval(Container.DataItem,"DeleteRecord")) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="Date" Visible="false">
                                                                <HeaderStyle Wrap="false" HorizontalAlign="center" />
                                                                <ItemStyle Wrap="false" HorizontalAlign="center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox Style="border-style: none;" CssClass="check" runat="server" ID="chkDate" Checked='<%#getBool(DataBinder.Eval(Container.DataItem,"DateControl")) %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                             <asp:TemplateColumn HeaderText="تدقيق">
                                                             <HeaderStyle Wrap="false" HorizontalAlign="center" />
                                                             <ItemStyle Wrap="false" HorizontalAlign="center" />
                                                             <ItemTemplate>
                                                                 <asp:CheckBox Style="border-style: none;" CssClass="check" runat="server" ID="chkAudit" Checked='<%#getBool(DataBinder.Eval(Container.DataItem,"AuditControl")) %>' />
                                                             </ItemTemplate>
                                                         </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid>

                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                            </div>

                        </div>

                    </div>
                </div>
            </div>
        </div>



    </div>







</asp:Content>
