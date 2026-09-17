<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LawSearch.aspx.cs" Inherits="UI.Web.Modules.Laws.Forms.LawSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="/Layout/Assets/law/css/law-table.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet">
    <style>
.highlight-search {
     background-color: #FFFF00;
     color: #000;
     font-weight: bold;
     padding: 3px 6px;
     border-radius: 3px;
     box-shadow: 0 0 3px rgba(255, 193, 7, 0.5);
 }

    .sort-radio-list input {
        margin-right: 20px;
    }

    .sort-radio-list label {
        margin-right: 10px;
    }

#cboxOverlay {
    background: #f443361f  !important;
}
    </style>
    <div class="row">
    <div class="col-lg-12">
        <div class="col-lg-8">
            <div class="page-title2">
                <h4>
                    <i class="icon-grid position-left"></i>
                    <%= _PageTitle %>
                </h4>
            </div>

        </div>

    </div>
</div>
    <div class="row mbl">
        <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title">نطاق البحث</h5>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <asp:Label runat="server" ID="lblerror"></asp:Label>
                    <fieldset class="content-group">
                        <legend class="text-semibold">
                            <i class="icon-file-text2 position-left"></i>
                            ادخل شروط البحث
                        </legend>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-lg-3 control-label">رقم الوثيقة:</label>
                                <div class="col-lg-3">
                                    <asp:TextBox ID="txtFilterSerialNum" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <label class="col-lg-3 control-label">سنة الاصدار:</label>
                                <div class="col-lg-3">
                                    <asp:TextBox ID="txtFilterSerialYear" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">تاريخ  إصدار الوثيقة من :</label>
                                <div class="col-lg-9">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                        <asp:TextBox ID="txtFilterDatefrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">تاريخ  انتهاء الوثيقة من :</label>
                                <div class="col-lg-9">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                        <asp:TextBox ID="txtFilterExpireFrom" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">جزء من نص الوثيقة :</label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:TextBox ID="txtFilterDetails" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">الترتيب:</label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:RadioButtonList 
                                        ID="RButtonSortList" 
                                        runat="server" 
                                        RepeatDirection="Horizontal"
                                        CssClass="sort-radio-list" AutoPostBack="True" OnSelectedIndexChanged="RButtonSortList_SelectedIndexChanged">

                                        <asp:ListItem Value="1" Selected="True">تنازليا</asp:ListItem>

                                        <asp:ListItem Value="2">تصاعديا </asp:ListItem>

                                    </asp:RadioButtonList>
                                </div>
                            </div>

                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-lg-3 control-label">نوع الوثيقة   :</label>
                                <div class="col-lg-9">
                                    <asp:DropDownList ID="lstFilterType" AutoPostBack="true" OnSelectedIndexChanged="lstFilterType_SelectedIndexChanged" class="Select2Drop" runat="server"></asp:DropDownList>
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
                                        <asp:TextBox ID="txtFilterExpireTo" runat="server" class="form-control pickadate-selectors picker__input picker__input--active"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3 control-label" for="">قيد الدراسة: </label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="lstFilterIsUnderStudy" class="Select2Drop" runat="server">
                                        <asp:ListItem Value="0" Text="الكل"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="نعم"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="لا"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <asp:Label runat="server" class="col-lg-3 control-label" ID="lblFilterCatTitle" Text="التصنيف"> التصنيف    :</asp:Label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:DropDownList ID="lstFilterCategory" class="Select2Drop" Width="100%" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">جزء من الموضوع :</label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:TextBox ID="txtFilterSubject" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3 control-label" for="">نشر بالجريدة الرسمية    : </label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="lstFilterPublish" class="Select2Drop" runat="server">
                                        <asp:ListItem Value="0" Text="الكل"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="نعم"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="لا"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-3 control-label">جزء من الملاحظات :</label>
                                <div class="col-lg-9 autoDrop">
                                    <asp:TextBox ID="txtFilterNotes" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <div class="text-right">
                        <asp:LinkButton ID="lnkSearch" class="btn btn-primary" runat="server" OnClick="lnkSearch_Click">&nbsp;&nbsp; بحث&nbsp;&nbsp; <i class="icon-search4 position-right"></i></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row mbl" id="divResult" runat="server" visible="false">
        <div class="panel-heading">
            <h5 class="panel-title">
                نتيجة البحث
                <span style="color: #000; font-size: 14px;">(<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
            </h5>
        </div>
       <div class="panel panel-flat" style="background-color:white !important;">
    <div class="panel-body">

        <%--
            شجرة Master/Detail: القانون الأصلي (Master) يظهر بدون رقم، ويليه
            مباشرة تفاصيله (Detail) مرقّمة 1، 2، 3... أضيفي في ItemDataBound:

            protected void dlLawHierarchy_ItemDataBound(object sender, DataListItemEventArgs e)
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlGenericControl rowContainer = (HtmlGenericControl)e.Item.FindControl("rowContainer");
                    Label lblItemIndex = (Label)e.Item.FindControl("lblItemIndex");

                    bool isDetailRow = !string.IsNullOrWhiteSpace(lblItemIndex.Text)
                                        && int.TryParse(lblItemIndex.Text.Trim(), out _);

                    rowContainer.Attributes["class"] += isDetailRow
                        ? " law-child-item"
                        : " law-master-item";
                }
            }
        --%>

        <div class="law-table" id="lawTable">
            <asp:DataList ID="dlLawHierarchy" runat="server" RepeatLayout="Flow"
                OnItemDataBound="dlLawHierarchy_ItemDataBound"
                CssClass="law-table">
            <ItemTemplate>
               <article class="law-row" id="rowContainer" runat="server">
                   <div class="law-law-cell">
                        <asp:PlaceHolder ID="phMasterToggle" runat="server" Visible="false">
                            <button type="button" class="law-expand-btn" aria-expanded="true" aria-label="القانون الرئيسي" title="القانون الرئيسي" tabindex="-1">
                                <i class="fa fa-minus" aria-hidden="true"></i>
                            </button>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phChildMarker" runat="server" Visible="false">
                            <span class="law-tree-marker"></span>
                        </asp:PlaceHolder>
                        <span class="law-index-badge">
                            <asp:Label ID="lblItemIndex" runat="server"></asp:Label>
                        </span>
                        <a target="_blank" class="law-title-link" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath+gets(Eval("ChildDocId"))+"/" +"&vfileList=["+gets(Eval("DocFilepath"))+";]" %>' id="file1" runat="server">
                            <span class="law-title"><%# Eval("DocTypeName") + " رقم " + Eval("DocNum") + " لسنة " + Eval("DocYear") %></span>
                        </a>
                    </div>

                    <div class="law-subject-cell">
                        <a target="_blank" class="law-subject-link" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath+gets(Eval("ChildDocId"))+"/" +"&vfileList=["+gets(Eval("DocFilepath"))+";]" %>'>
                            <span class="law-subject"><%# Eval("DocSubject") %></span>
                            <span class="law-description">
                                <asp:Literal ID="litDocDescription" runat="server" Text='<%# Eval("DocDescriptionHTML") %>'></asp:Literal>
                            </span>
                        </a>

                        <div class="law-status">
                            <a href="javascript:void(0);" onclick="openLawDetailsDocData('<%# Eval("LastProcedureID") %>',1); return false;">
                            <asp:Literal
                                    ID="litDocProceduresSummary"
                                    runat="server"
                                    Mode="PassThrough"
                                    Text='<%# GetProcedureBadges(Eval("DocProceduresTypesNameAr")) %>'
                                    Visible='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("DocProceduresTypesNameAr"))) %>'>
                            </asp:Literal>
                            </a>
                        </div>
                    </div>

                    <div class="law-duplicate-cell">
                        <asp:Label
                            ID="Label3"
                            runat="server"
                            CssClass="law-duplicate-badge"
                            Text="مكرر"
                            Visible='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("DocSerial"))) && Convert.ToString(Eval("DocSerial")).Contains("مكرر") %>'
                        ></asp:Label>
                    </div>

                    <div class="law-document-cell">
                        <a href="#" class="law-document-btn" data-toggle="modal" data-target="#modal_<%#Eval("ChildDocId") %>">
                            <i class="fa fa-file-text-o"></i><span>نص الوثيقة</span>
                        </a>
                    </div>

                    <div id="modal_<%#Eval("ChildDocId") %>" class="modal fade law-modal">
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h5 class="modal-title">تفاصيل الوثيقة</h5>
                                </div>

                                <div class="modal-body">
                                    <p>
                                        <%# HighlightSearchText(getsDocDetails(Eval("ChildDocId")), txtFilterDetails.Text).Replace("\r\n", "<br />").Replace("\n", "<br />") %>
                                    </p>
                                </div>

                                <div class="modal-footer">
                                    <button type="button" class="btn btn-link" data-dismiss="modal">إغلاق</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </article>
            </ItemTemplate>
        </asp:DataList>
        </div>

        <nav class="law-pagination text-center" style="margin-top:10px;">
            <asp:LinkButton ID="btnPrev" runat="server" CssClass="law-page-btn" OnClick="btnPrev_Click">السابق</asp:LinkButton>
            <asp:Repeater ID="rptPages" runat="server" OnItemCommand="rptPages_ItemCommand">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkPage" runat="server" CssClass='<%# ((int)Container.DataItem == CurrentPageNumber) ? "law-page-btn active" : "law-page-btn" %>' CommandName="Page" CommandArgument='<%# Container.DataItem %>'><%# Container.DataItem %></asp:LinkButton>
                </ItemTemplate>
            </asp:Repeater>
            <asp:LinkButton ID="btnNext" runat="server" CssClass="law-page-btn" OnClick="btnNext_Click">التالي</asp:LinkButton>
        </nav>
    </div>
</div>

    </div>
      <script>

        function buildMasterChildTree() {
            var table = document.getElementById('lawTable');
            if (!table) return;

            // already grouped
            if (table.querySelector('.law-children')) return;

            // Snapshot every law-row in document order, regardless of whether the
            // DataList (RepeatLayout="Flow") wrapped it in a <span> or not.
            // querySelectorAll returns a static NodeList, so it's safe to convert to array.
            var rows = Array.prototype.slice.call(table.querySelectorAll('.law-row'));
            if (!rows.length) return;

            var rebuilt = [];
            var currentMaster = null;
            var currentChildren = null;
            var hasChildrenForCurrentMaster = false;

            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];
                var isMaster = row.classList.contains('law-master-item');
                var isChild = row.classList.contains('law-child-item');

                if (isMaster) {
                    // Only push the previous children container if it has children
                    if (currentMaster && currentChildren && hasChildrenForCurrentMaster) {
                        rebuilt.push(currentChildren);
                    }

                    currentMaster = row;
                    currentMaster.setAttribute('data-law-master', 'true');
                    currentMaster.setAttribute('aria-expanded', 'true');
                    currentMaster.setAttribute('tabindex', '0');

                    currentChildren = document.createElement('div');
                    currentChildren.className = 'law-children';
                    currentChildren.hidden = false;
                    hasChildrenForCurrentMaster = false;

                    rebuilt.push(currentMaster);
                }
                else if (isChild && currentChildren) {
                    // Remove any inline display:none style from child rows
                    row.style.display = '';
                    row.style.visibility = 'visible';
                    currentChildren.appendChild(row);
                    hasChildrenForCurrentMaster = true;
                }
                else {
                    rebuilt.push(row);
                }
            }

            // Push the last children container if it has children
            if (currentMaster && currentChildren && hasChildrenForCurrentMaster) {
                rebuilt.push(currentChildren);
            }

            while (table.firstChild) {
                table.removeChild(table.firstChild);
            }
            for (var j = 0; j < rebuilt.length; j++) {
                table.appendChild(rebuilt[j]);
            }

            var masters = table.querySelectorAll(".law-master-item[data-law-master='true']");
            for (var m = 0; m < masters.length; m++) {
                var master = masters[m];
                var children = master.nextElementSibling;
                var btn = master.querySelector('.law-expand-btn');

                if (!btn || !children || !children.classList.contains('law-children') || children.children.length === 0) {
                    if (btn) btn.style.visibility = 'hidden';
                    continue;
                }

                btn.style.visibility = 'visible';
            }
        }


        function forceMastersExpandedOnLoad() {
            var masters = document.querySelectorAll("#lawTable .law-master-item[data-law-master='true']");
            for (var i = 0; i < masters.length; i++) {
                var master = masters[i];
                var children = master.nextElementSibling;
                var btn = master.querySelector('.law-expand-btn');
                var rootKey = master.getAttribute('data-root-key');

                // Get child rows from the law-children container first
                var childRows = [];
                if (children && children.classList && children.classList.contains('law-children')) {
                    // Get all .law-row elements inside law-children
                    var containerChildRows = children.querySelectorAll('.law-row');
                    for (var cr = 0; cr < containerChildRows.length; cr++) {
                        childRows.push(containerChildRows[cr]);
                    }
                } else if (rootKey) {
                    // Fallback: look for law-child-item rows outside container
                    var fallbackRows = document.querySelectorAll('#lawTable .law-child-item[data-root-key="' + rootKey + '"]');
                    for (var fr = 0; fr < fallbackRows.length; fr++) {
                        childRows.push(fallbackRows[fr]);
                    }
                }

                var hasChildrenGroup = !!(children && children.classList && children.classList.contains('law-children') && children.children.length > 0);
                var hasChildrenRows = childRows && childRows.length > 0;

                if (!btn || (!hasChildrenGroup && !hasChildrenRows)) {
                    if (btn) btn.style.visibility = 'hidden';
                    continue;
                }

                btn.style.visibility = 'visible';

                master.classList.add('is-expanded');
                master.setAttribute('aria-expanded', 'true');
                btn.setAttribute('aria-expanded', 'true');
                btn.setAttribute('aria-label', 'إغلاق القوانين التابعة');
                btn.setAttribute('title', 'إغلاق القوانين التابعة');
                var icon = btn.querySelector('i');
                if (icon) {
                    icon.classList.remove('fa-plus');
                    icon.classList.add('fa-minus');
                }

                // Show children group
                if (hasChildrenGroup) {
                    children.hidden = false;
                    // Also clear display style from child rows inside the container
                    for (var cr = 0; cr < childRows.length; cr++) {
                        childRows[cr].style.display = '';
                    }
                }

                // Show any fallback child rows
                if (hasChildrenRows && !hasChildrenGroup) {
                    for (var c = 0; c < childRows.length; c++) {
                        childRows[c].style.display = '';
                    }
                }
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            buildMasterChildTree();
            forceMastersExpandedOnLoad();

            // jQuery delegated toggle (stable with dynamic DOM and WebForms rendering)
            var $table = $('#lawTable');
            $table.off('click.lawtoggle', '.law-expand-btn');
            $table.on('click.lawtoggle', '.law-expand-btn', function (e) {
                e.preventDefault();
                e.stopPropagation();

                var $btn = $(this);
                var $master = $btn.closest('.law-master-item');
                var rootKey = $master.attr('data-root-key');
                var $children = $master.next('.law-children');

                // fallback if grouping container is missing: collect child rows by root key
                var $childRows = rootKey
                    ? $table.find('.law-child-item[data-root-key="' + rootKey + '"]')
                    : $();

                if ($children.length === 0 && $childRows.length === 0) return;

                var isExpanded = $master.attr('aria-expanded') === 'true';
                var nextExpanded = !isExpanded;

                $master.attr('aria-expanded', nextExpanded ? 'true' : 'false');
                $master.toggleClass('is-expanded', nextExpanded);

                $btn.attr('aria-expanded', nextExpanded ? 'true' : 'false');
                $btn.attr('aria-label', nextExpanded ? 'إغلاق القوانين التابعة' : 'فتح القوانين التابعة');
                $btn.attr('title', nextExpanded ? 'إغلاق القوانين التابعة' : 'فتح القوانين التابعة');

                var $icon = $btn.find('i.fa').first();
                $icon.toggleClass('fa-minus', nextExpanded);
                $icon.toggleClass('fa-plus', !nextExpanded);

                if ($children.length > 0) {
                    $children.prop('hidden', !nextExpanded);
                }
                if ($childRows.length > 0) {
                    $childRows.toggle(nextExpanded);
                }
            });

            // Optional: click on master row (except links/buttons) toggles too
            $table.off('click.lawtoggleRow', '.law-master-item');
            $table.on('click.lawtoggleRow', '.law-master-item', function (e) {
                if ($(e.target).closest('a,button').length > 0) return;
                var $btn = $(this).find('.law-expand-btn').first();
                if ($btn.length) $btn.trigger('click');
            });
        });

        function openLawDetailsDocData(childDocId, type) {
            type = type || 0;
            $.colorbox({
                href: '/Modules/Laws/LawDetailsDocData.aspx?ChildDocId=' + childDocId + '&Type=' + type,
                iframe: true,
                width: '60%',
                height: '75%'
            });

            return false;
        }

      </script>
</asp:Content>
