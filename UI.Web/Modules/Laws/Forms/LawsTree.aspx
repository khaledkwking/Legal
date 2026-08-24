<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LawsTree.aspx.cs" Inherits="UI.Web.Modules.Laws.Forms.LawsTree" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .row {
    margin-left: -10px;
    margin-right: -10px;
    padding-bottom: 20px !important;
}
        label {
    margin-bottom: 6px;
    font-weight: 400;
    font-size: 14px;
    height: 25px !important;
}
       .rowGrid{
           text-align:center;
       }
    	.child-row {
    		background-color: white !important;
    		padding: 8px 16px 8px 40px;
    		border-left: 4px solid #d6c26e;
    		font-size: 1em;
    	}

    		.child-row:hover td {
    			background-color: #d6c26e !important;
    		}

    	.search-container {
    		margin-top: 30px;
    		background-color: #fff;
    		padding: 20px;
    		border-radius: 12px;
    		box-shadow: 0 2px 10px rgba(0,0,0,0.5);
    	}

    	.table-hover tbody tr:hover {
    		background-color: #f5f5f5;
    	}
        .label-striped {
            background-color: white;
        }
        .btn-primary:focus, .btn-primary.focus, .btn-primary{
            background-color: #d6c26e;
            border-color: #d6c26e;
            color:black;
        }
        .btn-primary:focus, .btn-primary.focus, .btn-primary:hover {
            background-color: #d6c26ea6;
            border-color: #d6c26ea6;
            color:black;
        }
    	.btn-expand {
    		font-size: 1.2rem;
    		/*padding: 0 6px 0 6px;*/
    		cursor: pointer;
            background-color:#d6c26e !important;
    	}

    	.child-row.level-1 td{
    		background-color: #d6c26ea6 !important;
    		padding-right: 90px !important;
            font-size: 0.9em;
    	}

    	.child-row.level-2 td {
    		background-color: #d6c26e59 !important;
    		padding-right: 120px !important;
            font-size: 0.9em;
    	}

    	.child-row.level-3 td{
    		background-color: #d6c26e2b !important;
    		padding-right: 170px !important;
            font-size: 0.9em;
    	}

    	.child-row.level-4 td{
    		background-color: #d6c26e2b !important;
    		padding-right: 220px !important;
           font-size: 0.9em;
    	}

    	.child-row.level-5 td{
    		background-color: #d6c26e2b !important;
    		padding-right: 240px !important;
            font-size: 0.9em;
    	}

    	.level-indent {
    		padding-left: 20px;
    	}

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
    		background: #ffffff;
    	}

    		.grdItem td {
    			border-left: solid 1px #000000;
    			border-right: solid 1px #000000;
    			border-top: solid 0px #000000;
    			border-bottom: solid 0px #000000;
    		}

    	.pagination-container a, .pagination-container span {
    		display: inline-block;
    		padding: 6px 12px;
    		margin: 0 2px;
    		border: 1px solid #ddd;
    		border-radius: 4px;
    		background: #fff;
    		color: #007bff;
    		text-decoration: none;
    	}

    		.pagination-container a:hover {
    			background-color: #d6c26ea6;
    			color: black;
    		}

    	.pagination-container span {
    		background-color: #d6c26e;
    		color: black;
    	}

    	@media (min-width: 1200px) {
    		.container {
    			width: 95%;
    		}
    	}

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
    <div class="row">
	<div class="col-lg-12">
		<div class="col-lg-8">
			<div class="page-title2">
				<h4>
					<i class="icon-grid position-left"></i>
					التشريعات و ارتباطاتها
				</h4>
			</div>

		</div>

	</div>
</div>
    <div class="container search-container">
        						<fieldset class="content-group">

        <legend class="text-semibold">
				<i class="icon-file-text2 position-left"></i>
				ادخل شروط البحث
										<%--<a class="control-arrow" data-toggle="collapse" data-target="#demo1">
											<i class="icon-circle-down2"></i>
										</a>--%>
</legend>
                                    </fieldset>
        <div class="row mb-3">
    <!-- 🔹 أول صف فيه 4 أعمدة -->
    <div class="col-md-3">
        <label class="control-label">رقم الوثيقة:</label>
        <asp:TextBox ID="txtFilterSerialNum" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="col-md-3">
        <label class="control-label">سنة الإصدار:</label>
        <asp:TextBox ID="txtFilterSerialYear" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="col-md-3">
        <label class="control-label">نوع الوثيقة:</label>
        <asp:DropDownList ID="lstFilterType" AutoPostBack="true"
            OnSelectedIndexChanged="lstFilterType_SelectedIndexChanged"
            CssClass="Select2Drop form-control" runat="server"></asp:DropDownList>
    </div>
    <div class="col-md-3">
        <div style="height:30px;">
     <asp:Label runat="server" class="sheight"  ID="lblFilterCatTitle"> التصنيف :</asp:Label>

        </div>

      <asp:DropDownList ID="lstFilterCategory" runat="server" CssClass="Select2Drop form-control"></asp:DropDownList>
  </div>
  
</div>

<!-- 🔹 باقي الصفوف 3 أعمدة -->
<div class="row mb-3">
    <div class="col-md-4">
        <label class="control-label">تاريخ إصدار الوثيقة من:</label>
        <div class="input-group">
            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
            <asp:TextBox ID="txtFilterDatefrom" runat="server" CssClass="form-control pickadate-selectors"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-4">
        <label class="control-label">إلى:</label>
        <div class="input-group">
            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
            <asp:TextBox ID="txtFilterDateTo" runat="server" CssClass="form-control pickadate-selectors"></asp:TextBox>
        </div>
    </div>
      <div class="col-md-4">
      <label class="control-label">جزء من نص الوثيقة:</label>
      <asp:TextBox ID="txtFilterDetails" runat="server" CssClass="form-control"></asp:TextBox>
  </div>
  
</div>

<div class="row mb-3">
      <div class="col-md-4">
      <label class="control-label">تاريخ انتهاء الوثيقة من:</label>
      <div class="input-group">
          <span class="input-group-addon"><i class="icon-calendar22"></i></span>
          <asp:TextBox ID="txtFilterExpireFrom" runat="server" CssClass="form-control pickadate-selectors"></asp:TextBox>
      </div>
  </div>
    <div class="col-md-4">
        <label class="control-label">إلى:</label>
        <div class="input-group">
            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
            <asp:TextBox ID="txtFilterExpireTo" runat="server" CssClass="form-control pickadate-selectors"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-4">
        <label class="control-label">جزء من الموضوع:</label>
        <asp:TextBox ID="txtFilterSubject" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
</div>

<div class="row mb-3">
    <div class="col-md-4">
        <label class="control-label">العمل التحضيري:</label>
        <asp:DropDownList ID="lstfilterProceduretype" runat="server" CssClass="Select2Drop form-control"></asp:DropDownList>
    </div>
    <div class="col-md-4">
        <label class="control-label">قيد الدراسة:</label>
        <asp:DropDownList ID="lstFilterIsUnderStudy" runat="server" CssClass="Select2Drop form-control">
            <asp:ListItem Value="0" Text="الكل"></asp:ListItem>
            <asp:ListItem Value="1" Text="نعم"></asp:ListItem>
            <asp:ListItem Value="2" Text="لا"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="col-md-4">
        <label class="control-label">نشر بالجريدة الرسمية:</label>
        <asp:DropDownList ID="lstFilterPublish" runat="server" CssClass="Select2Drop form-control">
            <asp:ListItem Value="0" Text="الكل"></asp:ListItem>
            <asp:ListItem Value="1" Text="نعم"></asp:ListItem>
            <asp:ListItem Value="2" Text="لا"></asp:ListItem>
        </asp:DropDownList>
    </div>
</div>

<div class="row mb-3">
  
    <div class="col-md-4">
        <label class="control-label">التشريعات التي لها ارتباطات :</label>
        <asp:CheckBox ID="chkHasRelated" runat="server" CssClass="form-control" Checked="true"></asp:CheckBox>
    </div>
      <div class="col-md-8">
        <label class="control-label"></label>
          <br />
      <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary w-100" Width="100px"  Text="بحث" OnClick="btnSearch_Click" />
  </div>
</div>

        <asp:UpdatePanel ID="updGrid" runat="server">
            <ContentTemplate>
                <asp:DataGrid ID="dgLaws" runat="server" AutoGenerateColumns="False" 
                    CssClass="table datatable-basic dataTable no-footer"
                    OnItemDataBound="dgLaws_ItemDataBound"
                    OnItemCommand="dgLaws_ItemCommand"
                    AllowPaging="True" PageSize="25" OnPageIndexChanged="dgLaws_PageIndexChanged"
                    AllowSorting="True" OnSortCommand="dgLaws_SortCommand">

                    <SelectedItemStyle ForeColor="White" />
                    <ItemStyle CssClass="grdItem" />
                    <AlternatingItemStyle CssClass="grdItem" />
                    <PagerStyle Mode="NumericPages"
                        CssClass="pagination-container"
                        HorizontalAlign="Center"
                        BackColor="#f5f5f5"
                        ForeColor="#000"
                        Font-Bold="True" />
                    <HeaderStyle CssClass="grdHead" BackColor="Black" ForeColor="White" Font-Bold="False" />

                    <Columns>
                        <asp:TemplateColumn HeaderText="+" HeaderStyle-CssClass="rowGrid" HeaderStyle-Width="2%" ItemStyle-CssClass="rowGrid">
                            <ItemTemplate>
                                <asp:Button ID="btnExpand" runat="server" CssClass="btn btn-sm btn-outline-secondary btn-expand"
                                    Text="+" CommandName="Expand" CommandArgument='<%# Eval("Code") %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
						
                        <asp:BoundColumn DataField="DocNum" HeaderText="رقم التشريع <i class='fa fa-sort'></i>" SortExpression="DocNum" ItemStyle-CssClass="rowGrid" HeaderStyle-CssClass="rowGrid" HeaderStyle-Width="5%"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DocYear" HeaderText="سنة التشريع <i class='fa fa-sort'></i>" SortExpression="DocYear" ItemStyle-CssClass="rowGrid" HeaderStyle-CssClass="rowGrid" HeaderStyle-Width="5%" />

                        <asp:BoundColumn DataField="DocSubject" HeaderText="عنوان التشريع <i class='fa fa-sort'></i>" SortExpression="DocSubject" HeaderStyle-Width="40%"  />
                        <asp:BoundColumn DataField="Law_DocTypeNameAr" HeaderText="نوع التشريع <i class='fa fa-sort'></i>" SortExpression="Law_DocTypeNameAr" ItemStyle-CssClass="rowGrid" HeaderStyle-CssClass="rowGrid" HeaderStyle-Width="10%"  />
                        <asp:BoundColumn DataField="Law_DocCategoryNameAr" HeaderText="التصنيف <i class='fa fa-sort'></i>" SortExpression="Law_DocCategoryNameAr" ItemStyle-CssClass="rowGrid" HeaderStyle-CssClass="rowGrid" HeaderStyle-Width="10%" />
                        	<asp:TemplateColumn HeaderText="التفاصيل">
								<ItemStyle HorizontalAlign="right" Width="5%" />
								<HeaderStyle HorizontalAlign="Center" />
								<ItemTemplate>
												<div style="text-align: right">
																<a target="_blank" href='<%# ScannerRepositoryViewer+"?targetpath=" + _TargetUploadPath+gets(Eval("code"))+"/" +"&vfileList=["+gets(Eval("DocFilepath"))+";]" %>' style="font-size: 12px; <%#showattachment(gets(Eval("DocFilepath")))%>" class="label border-left-primary label-striped">
																	<i class="icon-attachment"></i>&nbsp;
																					  ملف الوثيقة
																</a>

												</div>
												<div style="margin-top: 10px;">
																<a href="LawDocData.aspx?LawDocID=<%#Eval("Code")%>&&DocRelated=true" class="label border-left-success label-striped" style="font-size: 12px;">
																	<i class="fa fa-file"></i>&nbsp;
																				   التفاصيل
																</a>
												</div>

												<div style="margin-top: 5px; display: <%#viewlinkedfile(gets(Eval("relatedAgreement")))%>">
																<a href="/modules/Agreements/forms/AgreementsDataLink.aspx?AgreementRefId=<%#Eval("relatedAgreement") %>" class="btn btn-warning btn-labeled btn-xs iframe">
																	<b><i class="glyphicon glyphicon-link"></i></b>إتفاقية مرتبطة

																</a>
												</div>

								</ItemTemplate>
				</asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </ContentTemplate>
        </asp:UpdatePanel>
      <%--  <asp:UpdatePanel ID="updGrid" runat="server">
            <ContentTemplate>
                <asp:DataGrid ID="dgLaws" runat="server" AutoGenerateColumns="False" class="table datatable-basic dataTable no-footer"
                    OnItemDataBound="dgLaws_ItemDataBound"
                    OnItemCommand="dgLaws_ItemCommand"
                    AllowPaging="True" PageSize="30" OnPageIndexChanged="dgLaws_PageIndexChanged"
                    AllowSorting="True" OnSortCommand="dgLaws_SortCommand">
                    <SelectedItemStyle ForeColor="White" />
                    <ItemStyle CssClass="grdItem" />
                    <AlternatingItemStyle CssClass="grdItem" />
                    <PagerStyle Mode="NumericPages"
                        CssClass="pagination-container"
                        HorizontalAlign="Center"
                        BackColor="#f5f5f5"
                        ForeColor="#000"
                        Font-Bold="True" />
                    <HeaderStyle CssClass="grdHead" BackColor="Black" ForeColor="White" Font-Bold="False" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="+">
                            <ItemTemplate>
                                <asp:Button ID="btnExpand" runat="server" CssClass="btn btn-sm btn-outline-secondary btn-expand"
                                    Text="+" CommandName="Expand" CommandArgument='<%# Eval("Code") %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="DocSubject" HeaderText="عنوان القانون" SortExpression="DocSubject" />
                        <asp:BoundColumn DataField="DocYear" HeaderText="السنة" SortExpression="DocYear" />
                        <asp:BoundColumn DataField="Law_DocTypeNameAr" HeaderText="نوع القانون" SortExpression="Law_DocTypeNameAr" />
                        <asp:BoundColumn DataField="Law_DocCategoryNameAr" HeaderText="التصنيف" SortExpression="Law_DocCategoryNameAr" />
                    </Columns>
                </asp:DataGrid>
            </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
</asp:Content>
