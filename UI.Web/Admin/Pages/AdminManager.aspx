<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="AdminManager.aspx.cs" Inherits="UI.Web.Admin.Pages.AdminManager" %>

<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">


    <script language="JavaScript" type="text/javascript" >
        function chkImage()
        {


            //alert("in");

            //return false;

            txt = document.getElementById("<%=lstadminType.ClientID %>")
        
            if(txt.value == "0")
            {
                alert("Please select Admin Type");
               // new $.Zebra_Dialog("Please select Admin Type");
                txt.focus();
                return false;
            }
        
            var txt = document.getElementById("<%=txtfullName.ClientID %>")
            if(txt.value == "")
            {
                // alert("in");
                alert("Please select Admin Name");
             //   new $.Zebra_Dialog("Please select Admin Name");
                txt.focus();
                return false;
            }
            
             var txt = document.getElementById("<%=txtName.ClientID %>")
            if(txt.value == "")
            {
                alert("Please select Admin User Name");
                //new $.Zebra_Dialog("Please select Admin User Name");
                txt.focus();
                return false;
            }
            
            txt = document.getElementById("<%=txtPassword.ClientID%>")
            if(txt.value == "")
            {
                alert("Please select Admin password");
              //  new $.Zebra_Dialog("Please select Admin password");

                txt.focus();
                return false;
            }

         
            
            return true;
        }
    </script>
 

    
     <!--BEGIN TITLE & BREADCRUMB PAGE-->
    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
ادارة مستخدمي النظام                    </h4>
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

                <div class="col-lg-12">
                    <div class="panel">
                        <div class="panel-body">

        <div class="row">
            <asp:Label runat="server" ID="lblerror"></asp:Label>
            <div class="col-lg-12">
                <div class="portlet box portlet-blue" id="tblAdd" runat="server" visible="false">
                    <div class="portlet-header">
                        <div class="caption">
                            <asp:Label runat="server" ID="lblSubTitle">إضافة جديد</asp:Label>
                        </div>
                      
                    </div>
                    <div class="portlet-body">
                        <div role="form" class="form-horizontal">
                            <div class="row">
                             
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-md-3 control-label" for="">الادارة   </label>

                                        <div class="col-md-9">

                                            <asp:DropDownList ID="lstDepatements" runat="server" class="form-control">
                                              
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3 control-label" for="">نوع المسنخدم </label>

                                        <div class="col-md-9">

                                            <asp:DropDownList ID="lstadminType" runat="server" class="form-control">
                                                <asp:ListItem Value="1">Super Administrator</asp:ListItem>
                                             <%--   <asp:ListItem Value="2">CT Board</asp:ListItem>
                                                <asp:ListItem Value="3">RTKPI  Board</asp:ListItem>
                                                <asp:ListItem Value="4">GDB  Board</asp:ListItem>--%>

                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-3 control-label" for="">الاسم </label>

                                        <div class="col-md-9">
                                            <asp:TextBox runat="server" ID="txtfullName" placeholder="Enter Full Name" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">اسم المستخدم </label>

                                        <div class="col-md-9">
                                            <div class="input-icon"><i class="fa fa-user"></i>


                                            <asp:TextBox runat="server" placeholder="Enter User Name" class="form-control" ID="txtName"></asp:TextBox></div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                                                            <label class="col-md-3 control-label">كلمة المرور </label>

                                                                            <div class="col-md-9">
                                                                                 <div class="input-icon"><i class="fa fa-fa-chain"></i>
                                                                                
                                                                                     <asp:TextBox runat="server" TextMode="Password" placeholder="Enter Password" class="form-control" id="txtPassword"></asp:TextBox>
                                                                                     </div> 
                                                                            </div>
                                                                        </div>
                                     <div class="form-group">
                                        <label class="col-md-3 control-label">صوره</label>

                                        <div class="col-md-9">
                                            <div class="input-group">
                                                <asp:Label ID="lblimage" runat="server"></asp:Label>
                                                <asp:FileUpload ID="txtImage" runat="server" />


                                            </div>
                                        </div>
                                    </div>

                                  



                                    

                                </div>
                                    <div class="col-md-6">

                                          <div class="form-group">
                                        <label class="col-md-3 control-label">البريد الالكتروني</label>

                                        <div class="col-md-9">
                                             <div class="input-icon"><i class="fa fa-envelope"></i>
                                            <asp:TextBox runat="server" placeholder="email@yourcompany.com" class="form-control" ID="txtEmail"></asp:TextBox>
                                                 </div> 
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">الموبيل</label>

                                        <div class="col-md-9">
                                            <asp:TextBox runat="server" placeholder="Enter User Contact mobile" class="form-control" ID="txtmobile"></asp:TextBox>
                                        </div>
                                    </div>
                                     <div class="form-group">
                                        <label class="col-md-3 control-label">العنوان</label>

                                        <div class="col-md-9">
                                            <asp:TextBox runat="server" placeholder="Enter User Address"  TextMode="MultiLine"  class="form-control" ID="txtaddress"></asp:TextBox>
                                        </div>
                                    </div>

                                    
                                    <div class="form-group">
                                       <label class="col-md-3 control-label"> فعال</label>

                                               <div class="col-md-9">
                                                   <asp:CheckBox ID="chkisactive" runat="server" />
                                               </div>
                                    </div>

                                         <div class="form-group" >
                                       <label class="col-md-3 control-label"> عرض الملفات الخاصة</label>

                                               <div class="col-md-9">
                                                   <asp:CheckBox ID="chkOperation" runat="server" />
                                                   <span class="text-warning mts help-block-right">Has permission to View Private Records</span>
                                               </div>
                                    </div>

                                          <div class="form-group" >
                                       <label class="col-md-3 control-label"> عرض علامة مائية   </label>

                                               <div class="col-md-9">
                                                   <asp:CheckBox ID="chkViewWaterMark" runat="server" />
                                                   
                                               </div>
                                    </div>
                                        </div>
                                    <div class="col-md-12">
                                        <div class="form-actions">
                                        <div class="col-md-offset-3 col-md-9">

                                            <asp:Button runat="server" ID="btnSave" class="btn btn-primary" Text=" حفظ البيانات " OnClick="btnSave_Click" />
                                            &nbsp;
				                <asp:Button runat="server" ID="btnCancel" class="btn btn-default" Text=" الغاء " OnClick="btnCancel_Click" />

                                        </div>
                                    </div>
                                        </div> 

                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div class="row" id="tblshow" runat="server">
            <div class="col-lg-12">
                <div class="portlet box">
                    <div class="portlet-header">
                    
                        <div class="actions pull-right">

                            <asp:LinkButton runat="server" ID="btnNew" class="btn btn-info btn-xs" OnClick="btnNew_Click1"><i class="fa fa-plus"></i>&nbsp;  إضافة جديد&nbsp;</asp:LinkButton>

                            <asp:LinkButton OnClientClick="return checkDelete();" runat="server" ID="btnDelete" class="btn btn-danger btn-xs" OnClick="btnDelete_Click"><i class="fa fa-times"></i>&nbsp;حذف البيانات</asp:LinkButton>

                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="row mbm" style="margin-bottom:20px">

                            <div class="col-md-3">
                                <div class="form-group">
                                    <span>الاإدارة   :</span>
                                    <asp:DropDownList runat="server" ID="lstFilterDept" class="table-group-action-select form-control input-inline">
                                        
                                    </asp:DropDownList>

                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span>نوع المستخدم :</span>
                                    <asp:DropDownList runat="server" ID="LstFilterAdminType" class="table-group-action-select form-control input-inline">
                                        <asp:ListItem Value="0">All </asp:ListItem>
                                        <asp:ListItem Value="1">Super Administrator</asp:ListItem>
                                      <%--  <asp:ListItem Value="2">CT Board</asp:ListItem>
                                        <asp:ListItem Value="3">RTKPI  Board</asp:ListItem>
                                        <asp:ListItem Value="4">GDB  Board</asp:ListItem>--%>
                                    </asp:DropDownList>

                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="form-group">
                                    <span>جزء من الاسم</span>
                                    <asp:TextBox ID="txtPArtOfName" runat="server" class="table-group-action-select form-control input-inline"></asp:TextBox>

                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="form-group" style="margin-top:20px;">
                                    <asp:LinkButton runat="server" ID="btnFilter" class="btn btn-success dropdown-toggle" OnClick="btnFilter_Click"><i class="fa fa-search"></i>&nbsp;
                                                تصفيه</asp:LinkButton>
                                </div>
                            </div>



                        </div>


                        <asp:DataGrid runat="server" ID="grdData" AutoGenerateColumns="False" PageSize="20" class="table table-hover table-striped table-bordered table-advanced tablesorter" OnDeleteCommand="grdData_DeleteCommand" OnEditCommand="grdData_EditCommand" OnItemDataBound="grdData_ItemDataBound">
                            <PagerStyle Visible="False" />
                            <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                            <Columns>
				                        <asp:BoundColumn DataField="id" Visible="False"></asp:BoundColumn>

                                 <asp:TemplateColumn HeaderText="الادارة">
                                      
                                            <ItemTemplate>
                                               <%#Eval("Security_pr_Department.namear")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="نوع المستخدم">
                                      
                                            <ItemTemplate>
                                               <%#Eval("Security_pr_AdminType.namear")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>

				                         
                                        
                                         
				                        <asp:BoundColumn DataField="name" HeaderText="الاسم ">
				                           
				                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="username" HeaderText=" اسم المستخدم"></asp:BoundColumn>
                                        <%--<asp:BoundColumn DataField="Password" HeaderText="Password"></asp:BoundColumn>--%>
                                        <asp:TemplateColumn HeaderText="فعال">
                                      
                                            <ItemTemplate>
                                               <%#ShowYesNo(getBool(Eval("IsActive")))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                         <asp:TemplateColumn HeaderText="عرض الملفات الخاصة">
                                      
                                            <ItemTemplate>
                                               <%#ShowYesNo(getBool(Eval("isOperation")))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                   <asp:TemplateColumn HeaderText="عرض العلامة المائيه">
                                      
                                            <ItemTemplate>
                                               <%#ShowYesNo(getBool(Eval("ProtectedOut")))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
				                        <asp:TemplateColumn HeaderText="تعديل">
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                
                                               <asp:LinkButton runat="server" ID="lnkEdit" CommandName="Edit" class="btn btn-default btn-xs">
                                                 <i class="fa fa-edit"></i>&nbsp;
                                                تعديل
                                        </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>

 <asp:TemplateColumn HeaderText="نموذج استلام">
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                
                                                <a href="../reports/userSystemsDeliveryForm.aspx?uid=<%#Eval("id") %>" target="_blank" style="white-space:nowrap"  >نموذج استلام النظام</a>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                              
                                        <asp:TemplateColumn >
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                            <input id="chkAllItems"   class="checkall" style="border-style:none;" type="checkbox" onclick="CheckAllDataGridCheckBoxes('chkItem', this.checked)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkItem" CssClass="check" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
				                    </Columns>
                        </asp:DataGrid>
                        <div class="row mbm">
                            <div class="col-lg-12">


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
</asp:Content>
