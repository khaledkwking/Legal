<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LawDetails.aspx.cs" Inherits="UI.Web.Modules.Laws.LawDetails" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
<link href="../../Layout/RTL/assets/css/bootstrap.css" rel="stylesheet" />
<%--  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">--%>

<%--  <link rel="preconnect" href="https://fonts.googleapis.com">
  <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>--%>

 <%-- <link href="https://fonts.googleapis.com/css2?family=Cairo:wght@400;500;600;700&display=swap" rel="stylesheet">--%>
    <link href="/Layout/Assets/law/css/law-table.css" rel="stylesheet" />

    <style type="text/css">
              .page-container {
    position: relative;
    padding: 20px 10px;
    padding-bottom: 40px;
}
     .sectors-banner {
        background-image: url('/Layout/uploads/MedalFiles/Banner.png');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: top center;
        width: 100%;                   /* full width of the screen */
    height: 900px;
    max-height: 80vh;
    min-height: 320px;
        position: relative;
        margin-bottom: 20px;
    }

    /* Optional: for very small screens */
    @media (max-width: 768px) {
        .sectors-banner {
        height: 360px;
        }
    }

.sectors-banner-text {
    position: absolute;
    top: 50%;
    right: 0;
    transform: translate(-50%, -50%);
    color: #ffffff;
    font-size: 60px;
    font-weight: bold;
    text-shadow: 0 2px 4px rgba(0,0,0,.5);
}
        .law-summary-item {
            font-size: 15px;
        }

    #<%= rblDocType.ClientID %> label {
        margin-left: 25px;
    }
        #<%= RadioButtonRelatedList.ClientID %> label {
        margin-left: 25px;
    }
       
.tooltip {
    position: relative;
    display: inline-block;
    cursor: pointer;
    font-family: 'Segoe UI', Arial, sans-serif;
}

.tooltip .tooltiptext {
    visibility: hidden;
    width: 220px;
    background-color: #FFD700; /* Yellow */
    color: #333333;           /* Elegant dark font */
    text-align: center;
    border-radius: 8px;
    padding: 8px 12px;
    font-size: 14px;
    font-weight: 500;
    box-shadow: 0 4px 10px rgba(0,0,0,0.15);

    position: absolute;
    z-index: 1;
    bottom: 125%;
    left: 50%;
    transform: translateX(-50%);
}

.tooltip:hover .tooltiptext {
    visibility: visible;
}

#cboxOverlay {
    background: #f443361f  !important;
}
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

     .law-details-view .law-match-item {
         box-shadow: 0 0 0 2px #ffd54f inset;
     }



    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
    <asp:HiddenField ID="hdnSectorId" runat="server" />
    <asp:HiddenField ID="hdnTypeId" runat="server" />
         <div class="sectors-banner">
     <div class="sectors-banner-text">
         <asp:Label ID="lblTitle" runat="server"></asp:Label>
     </div>
 </div>
    <div style="position: relative; top: -180px; margin-bottom: -180px;">
    <div class="row mbl">
        <div class="panel panel-flat" style="background-color:white !important;">
    <%--        <div class="panel-heading">
                <h5 class="panel-title">نطاق البحث</h5>
            </div>--%>
            <div class="panel-body" >
                <div class="form-horizontal">
                    <asp:Label runat="server" ID="lblerror"></asp:Label>
                    <fieldset class="content-group">
                        <legend class="text-semibold">
                            <i class="icon-file-text2 position-left"></i>
                            ادخل شروط البحث
                        </legend>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="col-lg-3 control-label">رقم الوثيقة:</label>
                                <div class="col-lg-9">
                                    <asp:TextBox ID="txtDocNum" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="col-lg-3 control-label">سنة الاصدار من:</label>
                                <div class="col-lg-9">
                                    <asp:TextBox ID="txtYearFrom" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display:none">
                                <label class="col-lg-3 control-label">سنة الاصدار إلى:</label>
                                <div class="col-lg-9">
                                    <asp:TextBox ID="txtYearTo" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <label class="col-lg-3 control-label">التصنيف:</label>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="ddlCategory" runat="server" class="Select2Drop" Width="100%"></asp:DropDownList>
                                </div>
                              
                            </div>
                        </div>
                    </fieldset>
                     <fieldset class="content-group">
                         <div class="col-md-6">
                              
                           <label class="col-lg-2 control-label">جزء من النص:</label>
                          <div class="col-lg-10">
                              <asp:TextBox ID="txtDetails" runat="server" class="form-control"></asp:TextBox>
                          </div>
                             </div>
                                 <div id="DocTypeDiv" runat="server">
                                <label class="col-lg-1 control-label">النوع:</label>
                                 <div class="col-lg-5">
                                    <%--<asp:DropDownList ID="ddlDocTypes" runat="server" class="Select2Drop" Width="100%"></asp:DropDownList>--%>
                                    <asp:RadioButtonList ID="rblDocType" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"  RepeatLayout="Flow" OnSelectedIndexChanged="rblDocType_SelectedIndexChanged">

                                    </asp:RadioButtonList>
                                </div>

                                   </div>
                        
                         </fieldset>
                      <fieldset class="content-group">
                         <div class="col-lg-6">
                                <label class="col-lg-2 control-label">الموضوع:</label>
                                <div class="col-lg-10">
                                    <asp:TextBox ID="txtDocSubject" runat="server" class="form-control"></asp:TextBox>
                                </div>
                             </div>
                            <div class="col-lg-3">
                          <asp:RadioButtonList ID="RadioButtonRelatedList" runat="server" RepeatDirection="Horizontal"  RepeatLayout="Flow" CssClass="radio-space" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonRelatedList_SelectedIndexChanged">
                               <asp:ListItem Text="مرتبط" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="الجميع بدون اظهار الارتباط" Value="0"></asp:ListItem>
                          </asp:RadioButtonList>

                     
                      </div>
                         
                    
                       <div class="text-right">
                         <asp:LinkButton ID="btnSearch" class="btn btn-primary" runat="server" OnClick="btnSearch_Click">&nbsp;&nbsp; بحث&nbsp;&nbsp; <i class="icon-search4 position-right"></i></asp:LinkButton>
                     </div>
                        
                       </fieldset>
                        <fieldset class="content-group">
                        <div class="col-lg-6">
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

                        </fieldset>
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
        <div class="panel panel-flat law-details-view" style="background-color:white !important;">
            <div class="panel-body">
                <main class="law-page">
                    <section class="law-table-shell" aria-label="التشريعات والقوانين المرتبطة">
                        <div class="law-table" id="lawTable">
                            <asp:DataList ID="dlLawDetails" runat="server" RepeatLayout="Flow" OnItemDataBound="dlLawDetails_ItemDataBound">
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
                                            <a target="_blank" class="law-title-link" href='<%# ((UI.Web.Modules.Laws.LawDetails)Page).GetAttachmentUrl(Eval("ChildDocId"), Eval("DocFilepath")) %>' id="file1" runat="server">
                                                <span class="law-title"><%# Eval("DocTypeName") + " رقم " + Eval("DocNum") + " لسنة " + Eval("DocYear") %></span>
                                            </a>
                                        </div>

                                        <div class="law-subject-cell">
                                            <a target="_blank" class="law-subject-link" href='<%# ((UI.Web.Modules.Laws.LawDetails)Page).GetAttachmentUrl(Eval("ChildDocId"), Eval("DocFilepath")) %>'>
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
                                                        Text='<%# ((UI.Web.Modules.Laws.LawDetails)Page).GetProcedureBadges(Eval("DocProceduresTypesNameAr")) %>'
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
                                                            <%# ((UI.Web.Modules.Laws.LawDetails)Page).HighlightSearchText(((UI.Web.Modules.Laws.LawDetails)Page).getsDocDetails(Eval("ChildDocId")), txtDetails.Text).Replace("\r\n", "<br />").Replace("\n", "<br />") %>
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
                                    <asp:LinkButton ID="lnkPage" runat="server" CssClass='<%# ((int)Container.DataItem == ((UI.Web.Modules.Laws.LawDetails)Page).CurrentPageNumber) ? "law-page-btn active" : "law-page-btn" %>' CommandName="Page" CommandArgument='<%# Container.DataItem %>'><%# Container.DataItem %></asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:LinkButton ID="btnNext" runat="server" CssClass="law-page-btn" OnClick="btnNext_Click">التالي</asp:LinkButton>
                        </nav>
                    </section>
                </main>
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
