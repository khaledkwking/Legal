using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;
using System.Collections;
using UI.Web.Admin.Controller;
using Permission.DAL.Entities;
using Infrastructure.DAL;
using Microsoft.VisualBasic;
using Permission.DAL.Repository;

namespace UI.Web.Admin.Pages
{
    public partial class AdminManager : BaseFormAdmin
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //AduptPersonGrd();
            applyUserPermission();

        }

        protected void Page_Load(object sender, System.EventArgs e)
        {

            lblerror.Text = "";

            btnCancel.Attributes.Add("onclick", "Page_ValidationActive=false;");
            btnSave.Attributes.Add("onclick", "return chkImage();");
            if (!IsPostBack)
            {
                //if ((Request.UrlReferrer == null))
                //{
                //    Response.Redirect("Default.aspx");
                //}

                FillDll(Security_Users.ins.fillUserDepartments(), ref lstDepatements, "Namear", "Code");
                FillDllwithoptional_ALL(Security_Users.ins.fillUserDepartments(), ref lstFilterDept, "NameAr", "Code", "الكل");


                FillDllwithoptional_ALL(Security_Users.ins.fillJobs(), ref lstadminType, "namear", "id", "إختر");
                FillDllwithoptional_ALL(Security_Users.ins.fillJobs(), ref LstFilterAdminType, "namear", "id", "الكل");

                ViewState["Item"] = 0;
                FillGrid();
            }

        }
        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            btnSave.Visible = userAccess.Edit;
            btnDelete.Visible = userAccess.Delete;

        }
        private void FillGrid()
        {
            var userList = Security_Users.ins.GetItems(ZeroIntergerIFNull( LstFilterAdminType.SelectedValue), txtPArtOfName.Text,ZeroIntergerIFNull( lstFilterDept.SelectedValue));

            if (userList!=null)
            {
                lblcount.Text = (Resources.Utilities.foundTotal + (userList.Count.ToString() + Resources.Utilities.records));
                decimal c = System.Math.Ceiling(Convert.ToDecimal(userList.Count / grdData.PageSize));
                if ((c <= grdData.CurrentPageIndex))
                {
                    grdData.CurrentPageIndex = 0;
                }

                grdData.DataSource = userList;
                grdData.DataBind();


            }

        }

        protected void pager_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            Int32 currnetPageIndx = ((Int32)(e.CommandArgument));
            if ((currnetPageIndx <= 0))
            {
                currnetPageIndx = 1;
            }

            if ((currnetPageIndx > grdData.PageCount))
            {
                currnetPageIndx = (grdData.PageCount - 1);
            }

            pager1.CurrentIndex = currnetPageIndx;
            grdData.CurrentPageIndex = (currnetPageIndx - 1);
            this.FillGrid();
        }

        protected void grdData_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;

            Security_Users.ins.Delete((Security_pr_admin)Security_Users.ins.GetDetails(ZeroIntergerIFNull(id)));
            this.FillGrid();
        }

        protected void btnDelete_Click(object sender, System.EventArgs e)
        {
             
            for (int i = 0; (i
                        <= (grdData.Items.Count - 1)); i++)
            {
                string id = grdData.Items[i].Cells[0].Text;
                CheckBox check = ((CheckBox)(grdData.Items[i].FindControl("chkItem")));
                if (check.Checked)
                {
                    Security_Users.ins.Delete((Security_pr_admin)Security_Users.ins.GetDetails(ZeroIntergerIFNull(id)));
                }

            }

            
                this.FillGrid();
             

        }

        protected void grdData_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            this.ClearForm();
            ViewState["Item"] = id;
            this.FillForm();
            tblshow.Visible = false;
        }

        protected void grdData_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#DA9CF1\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
            }

            if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#DA9CF1\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#EFEFEF\';");
            }

        }

        private string GetTitle(bool isadd)
        {
            if (isadd)
            {
                return "Add New Record Information";
            }
            else
            {
                return "Edit Record Information";
            }

        }


        private void FillForm()
        {

            var userDetails = Security_Users.ins.GetDetails(ZeroIntergerIFNull(ViewState["Item"].ToString()));

            if (userDetails !=null)
            {


                txtName.Text = userDetails.username;
                txtPassword.Text = userDetails.password;
                txtPassword.Attributes.Add("Value", gets(userDetails.password));
                txtfullName.Text = userDetails.name;
                chkisactive.Checked =  getBool(userDetails.IsActive);
                chkOperation.Checked = getBool( userDetails.isOperation);
                chkViewWaterMark.Checked = getBool( userDetails.ProtectedOut);
                txtmobile.Text = userDetails.mobile;
                txtEmail.Text = userDetails.Email;
                txtaddress.Text = userDetails.Address;

                if (userDetails.DeptID!=null && userDetails.DeptID != 0)
                {
                    lstDepatements.SelectedValue = gets(userDetails.DeptID);
                }
                lstadminType.SelectedValue =gets( userDetails.AdminType.Value);

            }

             
            tblAdd.Visible = true;
            lblSubTitle.Text = this.GetTitle(false);

        }

        private string getImage(ref FileUpload txtFile)
        {
            string imgname = "";
            string temp;
            string ext;
            int inx;
            int i;
            string RandChar;
            string ValueString;
            //  Microsoft.VisualBasic.VBMath.Randomize();
            imgname = "";
            ValueString = "";
            if (!(txtFile.PostedFile == null))
            {
                if ((txtFile.PostedFile.FileName != ""))
                {
                    imgname = txtFile.PostedFile.FileName;
                    imgname = imgname.Substring((imgname.LastIndexOf("\\") + 1));
                    inx = imgname.LastIndexOf(".");
                    temp = imgname.Substring(0, inx);
                    ext = imgname.Substring((inx + 1));
                    for (i = 1; (i <= 16); i++)
                    {
                        //  RandChar = (string)(Microsoft.VisualBasic.Conversion.Int((26 * Microsoft.VisualBasic.VBMath.Rnd() + 65)).ToString());

                        RandChar = (new Random().Next(i, 16)).ToString();
                        ValueString += RandChar;
                    }

                    imgname = (ValueString + ("." + ext));
                    txtFile.PostedFile.SaveAs(Server.MapPath(("/Layout/uploads/Adminprofile/" + imgname)));
                }

            }


            //////string imgname = "";
            //////int inx = 0;
            //////string temp = "";
            //////string ext = "";
            //////string RandChar = "";
            //////string ValueString = "";
            //////Microsoft.VisualBasic.VBMath.Randomize();
            //////imgname = "";
            //////imgname = txtImage.PostedFile.FileName;
            //////imgname = imgname.Substring((imgname.LastIndexOf("\\") + 1));
            //////inx = imgname.LastIndexOf('.');
            //////temp = imgname.Substring(0, inx);
            //////ext = imgname.Substring((inx + 1));
            //////for (int i = 1; (i <= 10); i++)
            //////{
            //////    RandChar = (string)(Microsoft.VisualBasic.Conversion.Int((26 * Microsoft.VisualBasic.VBMath.Rnd() + 65)).ToString());
            //////    ValueString += RandChar;
            //////}
            //////imgname = (temp + (ValueString + ("." + ext)));
            //////return imgname;
            return imgname;
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtfullName.Text = "";
            chkOperation.Checked = false;
            chkViewWaterMark.Checked = false;
            txtPassword.Text = "";
            ViewState["Item"] = 0;
            tblAdd.Visible = false;

            tblshow.Visible = true;
            lblSubTitle.Text = this.GetTitle(true);
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            string Script = "";
            try
            {
                //    string img1 = "";
                string img1 = this.getImage(ref txtImage);
                Security_pr_admin obj = new Security_pr_admin();
                if (ViewState["Item"].ToString().Equals("0"))
                {
                    // CHeck User Existance
                    if (Security_Users.ins.CheckUserNameExitance(txtName.Text))
                    {
                        Script = FormatpopupErrorMSG("Sorry, User Name Not Vaild please enter another User Name ", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                        return;
                    }

                    obj.name = txtfullName.Text;
                    obj.username = txtName.Text;
                    obj.password = txtPassword.Text;
                    obj.AdminType = ZeroIntergerIFNull(lstadminType.SelectedValue);
                    obj.IsActive = chkisactive.Checked;
                    obj.isOperation = chkOperation.Checked;
                    obj.ProtectedOut = chkViewWaterMark.Checked;
                    obj.mobile = txtmobile.Text;
                    obj.Email = txtEmail.Text;
                    obj.Address = txtaddress.Text;
                    obj.AdminPhoto = img1;
                    obj.DeptID = ZeroIntergerIFNull(lstDepatements.SelectedValue);
                    obj.RegisterDate = DateTime.Now;
                    Security_Users.ins.Add(obj);
                     
                }
                else
                {
                    obj = Security_Users.ins.GetDetails(ZeroIntergerIFNull(ViewState["Item"].ToString()));
                    obj.name = txtfullName.Text;
                    obj.username = txtName.Text;
                    obj.password = txtPassword.Text;
                    obj.AdminType = ZeroIntergerIFNull(lstadminType.SelectedValue);
                    obj.IsActive = chkisactive.Checked;
                    obj.isOperation = chkOperation.Checked;
                    obj.ProtectedOut = chkViewWaterMark.Checked;
                    obj.mobile = txtmobile.Text;
                    obj.Email = txtEmail.Text;
                    obj.Address = txtaddress.Text;
                    obj.AdminPhoto = img1;
                    obj.DeptID = ZeroIntergerIFNull(lstDepatements.SelectedValue);


                    Security_Users.ins.Update(obj);
                }

                Script = FormatpopupErrorMSG("تم حفظ البيانات بنجاح", "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                this.ClearForm();
                this.FillGrid();
            }
            catch (Exception ex)
            {
                Script = FormatpopupErrorMSG("خطأ فى حفظ البيانات", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                lblerror.ForeColor = System.Drawing.Color.Red;
                lblerror.Text = ("Error :" + ex.Message);
            }

        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.ClearForm();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.FillGrid();
        }



        protected void btnNew_Click1(object sender, EventArgs e)
        {

            this.ClearForm();
            tblAdd.Visible = true;
            tblshow.Visible = false;
        }
    }
}