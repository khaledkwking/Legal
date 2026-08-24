<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LawDocSearchDetails.aspx.cs" Inherits="UI.Web.Modules.Laws.Forms.LawDocSearchDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
    <div class="row mbl">
        <div class="panel panel-flat">
               <div class="panel-heading" style="font-size:18px;">
       
                   <a target="_blank" href="" id="ahrefDoc" runat="server">
       <h5 class="panel-title">
            * <asp:Label ID="lblMainDoc" runat="server" ForeColor="#166dba"></asp:Label>  - 
             <asp:Label ID="lblMainDocSubject" runat="server" ForeColor="Black"></asp:Label>  
   
           </h5>
                       </a>
            </div>
            

        </div>
              <div style="text-align:right;padding:20px;font-size:16px;">
                 «· ‘—Ì⁄«  «·„— »ÿ…
                  &nbsp;  (
                 <asp:Label ID="lblDetailsCount" runat="server"></asp:Label>
                 )
             
                </div>
        <div class="panel-body" style="padding-right:50px;border:1px solid;">
            <br />
    <asp:DataList ID="dlSourceLaws" runat="server" RepeatLayout="Flow">
        <ItemTemplate>
            <div class="law-summary-item" style="font-size:16px;">
                 <a target="_blank" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath+gets(Eval("code"))+"/" +"&vfileList=["+gets(Eval("DocFilepath"))+";]" %>' id="file1" runat="server">

                 
                <div>
                       <asp:Label ID="Literal3" runat="server" Text='<%# (PageOffset + Container.ItemIndex + 1)%>' ForeColor="Black"></asp:Label> -

                       <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("DocDescriptionHTML") %>'></asp:Literal>
                    
              
                       <asp:Label ID="Literal2" runat="server" Text='<%# Eval("DocSubject") %>' ForeColor="Black"></asp:Label>
                    
                    <hr style="font-weight:bold;" />
                </div>
                </a>
            </div>

        </ItemTemplate>
    </asp:DataList>
     
</div>
        <div class="text-center" style="margin-top: 10px;">
    <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-default btn-xs" OnClick="btnPrev_Click">«·”«»ﬁ</asp:LinkButton>
    <asp:Repeater ID="rptPages" runat="server" OnItemCommand="rptPages_ItemCommand">
        <ItemTemplate>
            <asp:LinkButton ID="lnkPage" runat="server" CssClass='<%# ((int)Container.DataItem == CurrentPageNumber) ? "btn btn-primary btn-xs" : "btn btn-default btn-xs" %>' CommandName="Page" CommandArgument='<%# Container.DataItem %>'><%# Container.DataItem %></asp:LinkButton>
        </ItemTemplate>
    </asp:Repeater>
    <asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-default btn-xs" OnClick="btnNext_Click">«· «·Ì</asp:LinkButton>
</div>
        <div style="text-align:left">
                       <asp:HyperLink 
                            ID="lnkSearch" 
                            runat="server" 
                            CssClass="btn btn-primary"
                            NavigateUrl="LawDocSearch.aspx">
                            &nbsp;&nbsp; —ÃÊ⁄&nbsp;&nbsp;
                            
                        </asp:HyperLink>


</div>
    </div>
    
</asp:Content>
