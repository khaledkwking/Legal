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
using Infrastructure.DAL;

using Microsoft.VisualBasic;
using System.Resources;

using Infrastructure;

namespace UI.Web.Modules.MasterData
{
    public partial class lookupsOrder : BaseFormAdmin
    {
         public string _PageTitle = "";
        public string TargetTableName { get; set; }
        public LookupMaster objLookup = IoC.Resolve<LookupMaster>();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "lookups.aspx?tableName=Parliament_legislativeChapter";

            string Script = "";
            if (Request.QueryString["tableName"] != null)
            {
                if (Request.QueryString["tableName"] != "")
                {
                    TargetTableName = Request.QueryString["tableName"].ToString();

                    //ResourceManager rm = new ResourceManager(typeof(Resources.lockups));
                    //string someString = 
                    //_PageTitle = (String)GetGlobalResourceObject(
                    // "lockups", TargetTableName); // Resources.Utilities.TargetTableName;
                    _PageTitle = (string)GetGlobalResourceObject("lockups", TargetTableName);
                    //  _PageTitle = "البيانات الاساسيه";
                }
                else
                {
                    Script = FormatpopupErrorMSG("Fail To Load Table Data, TableName Paramter", "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                    lblerror.ForeColor = System.Drawing.Color.Red;
                    return;
                }

            }
            else
            {
                Script = FormatpopupErrorMSG("Fail To Load Table Data", "1");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                lblerror.ForeColor = System.Drawing.Color.Red;
                return;
            }

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

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
                //    Response.Redirect("/admin/pages/main.aspx");
                //}

                ViewState["Item"] = 0;
                FillGrid();
            }

        }

        private void FillGrid()
        {
            var masterList = objLookup.GetItems(TargetTableName, txtPArtOfName.Text,"1");
            lblcount.Text = (Resources.Utilities.foundTotal + (masterList.Count.ToString() + Resources.Utilities.records));
            decimal c = System.Math.Floor(Convert.ToDecimal(masterList.Count / grdData.PageSize));
            if ((c < grdData.CurrentPageIndex))
            {
                grdData.CurrentPageIndex = 0;
            }

            grdData.DataSource = masterList;
            grdData.DataBind();
            int _totalCount = masterList.Count;
           // pager1.ItemCount = _totalCount;

        }

        protected void grdData_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            objLookup.Delete(TargetTableName, id);
            this.FillGrid();
        }

        protected void btnDelete_Click(object sender, System.EventArgs e)
        {
            string Script = "";
            string lst = "";
            for (int i = 0; (i
                        <= (grdData.Items.Count - 1)); i++)
            {
                string id = grdData.Items[i].Cells[0].Text;
                CheckBox check = ((CheckBox)(grdData.Items[i].FindControl("chkItem")));
                if (check.Checked)
                {
                    if (lst.Equals(""))
                    {
                        lst = (lst + id);
                    }
                    else
                    {
                        lst = (lst + ("," + id));
                    }

                }

            }

            if (!lst.Trim().Equals(""))
            {
                try
                {
                    objLookup.DeleteList(TargetTableName, lst);
                    this.FillGrid();

                    Script = FormatpopupErrorMSG("Data Deleted Successfully", "3");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                }
                catch (Exception)
                {


                    string script = FormatpopupErrorMSG(Resources.Alerts.SorryDeleteMaterDataFailed, "1");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);
                    lblerror.ForeColor = System.Drawing.Color.Red;
                    return;
                }

            }

        }

        protected void grdData_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            this.ClearForm();
            ViewState["Item"] = id;
            this.FillForm();
            tblshow.Visible = false;
            tblAdd.Visible = true;
        }

        protected void grdData_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
            }

            if ((e.Item.ItemType == ListItemType.AlternatingItem))
            {
                e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor=\'#f2d575\';");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=\'#FFFFFF\';");
            }

        }

        private string GetTitle(bool isadd)
        {
            if (isadd)
            {
                return "";
            }
            else
            {
                return "";
            }

        }


        private void FillForm()
        {
           var masterDetails = objLookup.GetDetails(TargetTableName, ViewState["Item"].ToString());
            if (masterDetails !=null)
            {


                txtNameEn.Text = gets(masterDetails.NameEn);
                txtNameAr.Text = gets(masterDetails.NameAr);
                txtOrder.Text = gets(masterDetails.d_order);

            }

            tblAdd.Visible = true;
            lblSubTitle.Text = this.GetTitle(false);

        }
        private void applyUserPermission()
        {

            btnNew.Visible = userAccess.Add;
            btnSave.Visible = userAccess.Edit ||  userAccess.Add;
            //btnDelete.Visible = userAccess.Delete;


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
            txtNameEn.Text = "";
            txtNameAr.Text = "";
            txtOrder.Text = "0";

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
                //Check Item Existance
              

                if (ViewState["Item"].ToString().Equals("0"))
                {

                    if (objLookup.checkTextExistance(TargetTableName , txtNameAr.Text))
                    {

                        Script = FormatpopupErrorMSG("Item already exist , repeating not allowed", "1");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                        return;
                    }

                    objLookup.Insert(TargetTableName, txtNameEn.Text, txtNameAr.Text,txtOrder.Text);
                }
                else
                {
                    objLookup.Update(TargetTableName, ViewState["Item"].ToString(), txtNameEn.Text, txtNameAr.Text, txtOrder.Text);
                }

                Script = FormatpopupErrorMSG("Data Saved Successfully", "3");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", Script, true);
                this.ClearForm();
                this.FillGrid();
            }
            catch (Exception ex)
            {
                Script = FormatpopupErrorMSG("Fail To Save Data", "1");
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



        protected void btnNew_Click(object sender, EventArgs e)
        {

            this.ClearForm();
            tblAdd.Visible = true;
            tblshow.Visible = false;
        }

        protected void grdData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            grdData.CurrentPageIndex = e.NewPageIndex  ;
            this.FillGrid();
        }
    }
}