<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="pdfCompress.aspx.cs" Inherits="UI.Web.Admin.Pages.pdfCompress" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
    <div class="row">

        <div class="col-lg-12">
            <div class="portlet box portlet-blue" id="tblAdd">

                <div class="portlet-body">
                    <div role="form" class="form-horizontal">
                        <div class="row">

                            <div class="col-md-6" style="direction: ltr !important">
                                <div class="form-group">
                                    <label class="col-md-3 control-label" for="">Souce Location   </label>

                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtSouceLocation" placeholder="Enter Souce Location  " class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label" for="">Target Location </label>

                                    <div class="col-md-9">
                                        <asp:TextBox runat="server" ID="txtTargetLocation" placeholder="Enter Target Location  " class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12">

                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnSave" class="btn btn-primary" Text="   Compress Files " OnClick="btnSave_Click" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnAgreementMigation" class="btn btn-primary" Text="   Agreements Files migration " OnClick="btnAgreementMigation_Click" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnAgreementMigation2" class="btn btn-primary" Text="   Agreements Files migration 2" OnClick="btnAgreementMigation2_Click" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnMedal" class="btn btn-primary" Text="   Medal Files migration " OnClick="btnMedal_Click" />
                                </div>

                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnMedal2" class="btn btn-primary" Text="   Medal Files migration 2 " OnClick="btnMedal2_Click" />
                                </div>

                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnCases" class="btn btn-primary" Text="   cases Files migration " OnClick="btnCases_Click" />
                                </div>

                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnQuestions" class="btn btn-primary" Text=" Questions Files migration " OnClick="btnQuestions_Click" />
                                </div>

                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnMadbata" class="btn btn-primary" Text="Madbata migration " OnClick="btnMadbata_Click" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnSuggestion" class="btn btn-primary" Text="Suggestion migration " OnClick="btnSuggestion_Click" />
                                </div>

                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnLib" class="btn btn-primary" Text="Lib migration " OnClick="btnLib_Click" />
                                </div>

                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btnPm" class="btn btn-primary" Text="PM migration " OnClick="btnPm_Click" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button runat="server" ID="btbLawsDocs" class="btn btn-primary" Text="Laws migration " OnClick="btbLawsDocs_Click" />
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lblSucess" ForeColor="Green"></asp:Label></div>
                        <div class="col-md-6">
                            <asp:Label runat="server" ID="lblerror" ForeColor="Red"></asp:Label></div>

                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
