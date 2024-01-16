    <%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Empty.Master" AutoEventWireup="true" CodeBehind="operationKeysDetails.aspx.cs" Inherits="UI.Web.Modules.Audit.Forms.operationKeysDetails" %>

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



        function ValidateHeading() {



        }
        function ControlGrid(imgName, rowIndex, rowID) {

            rowIndex = rowIndex + 3;

            var myrow = "";
            if (rowIndex < 10)
                myrow = "ctl00_Main_grdresult_ctl0" + rowIndex;
            else
                myrow = "ctl00_Main_grdresult_ctl" + rowIndex;
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




        function LinkAddClick() {
            // alert("in");

            return InsertItem();
        }

        function relatedOrgClick() {
            // alert("in");

            return ValidatedRelatedOrgs();
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
                        نظام تتبع العمليات
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

    <div class="row mbl">

        <div class="panel" id="tblshow" runat="server" visible="true">

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
                                                    تفاصيل العملية    <span style="color:#000;font-size:14px;"> ( <asp:Label ID="lblSearchResultCount" runat="server"></asp:Label>)</span>

                                                    <a href="javascript:history.go(-1)" class="control-arrow">رجوع <i class=" icon-backward2"></i></a>
                                                </legend>
                                                <div class="col-md-5">
                                 <div class="form-group">
                                    <label class="col-lg-3 control-label">  النظام :</label>
                                    <div class="col-lg-9">
                                        <%=SystemName %>

                                        </div>
                                     </div>

                                                    <div class="form-group">
                                    <label class="col-lg-3 control-label">  المستخدم :</label>
                                    <div class="col-lg-9">
                                         <%=userName %>

                                        </div>
                                     </div>

                                                      <div class="form-group">
                                    <label class="col-lg-3 control-label">  تاريخ العملية :</label>
                                    <div class="col-lg-9">
                                         <%=TransDate %>

                                        </div>
                                     </div>



                                                      <div class="form-group">
                                    <label class="col-lg-3 control-label">  شروط البحث   :</label>
                                    <div class="col-lg-9" style="color:red">
                                         <%=SearchKeys %>

                                        </div>
                                     </div>
                                                    </div>



                                                <div class="datatable-scroll" style="width:100%;">


                                                    <asp:DataGrid ID="grdresult" runat="server"
                                                         AllowPaging="false" AutoGenerateColumns="true"  PageSize="40" class="table datatable-basic dataTable no-footer"
                                                        Width="50%" OnItemDataBound="grdresult_ItemDataBound" >
                                                        <SelectedItemStyle ForeColor="White" />
                                                        <ItemStyle CssClass="grdItem" />
                                                        <AlternatingItemStyle CssClass="grdItem" />
                                                        <PagerStyle Visible="false" />
                                                        <HeaderStyle CssClass="grdHead" BackColor="Black" ForeColor="White" Font-Bold="False" />

                                                    </asp:DataGrid>
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

    <!--END CONTENT-->
    <!--BEGIN FOOTER-->
    <div id="scanLoading" class="scanLoading" style="display: none">
        <img src="/Layout/images/scan-document.gif" />
    </div>
</asp:Content>
