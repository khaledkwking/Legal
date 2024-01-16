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
using Permission.DAL.Repository;

namespace UI.Web.Admin.Pages
{
    public partial class PermissionsNew : BaseFormAdmin
    {
      

        public string def = "";
        protected void Page_PreRender(object sender, System.EventArgs e)
        {
          //  Access ac = new Access(ViewState["CurrentPage"].ToString());
            lbkColse.Attributes.Add("onclick", "return confirm('Are you sure you want to clear all permissions for this job/user?');");

        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            lblError.Text = "";
            if (!IsPostBack)
            {


                FillDllwithoptional_ALL(Security_Users.ins.fillJobs(), ref lstJob, "namear", "id", "الكل");
                FillDllwithoptional_ALL(Security_Users.ins.GetUserbyType(ZeroIntergerIFNull(lstJob.SelectedValue)), ref lstMember, "name", "id", "Any User");


                FillPermissions();
            }
        }

        #region "Lists Filling"
       
      

        protected void lstJob_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillDllwithoptional_ALL(Security_Users.ins.GetUserbyType(ZeroIntergerIFNull(lstJob.SelectedValue)), ref lstMember, "name", "id", "Any User");
            FillPermissions();
        }

        protected void lstMember_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FillPermissions();
        }
        #endregion

        #region "Permissions Area"
        private void FillPermissions()
        {
            ViewState["Def"] = "";
            def = "";
            ViewState["Group1"] = new ArrayList();
            ViewState["Group2"] = new ArrayList();
            ViewState["Group3"] = new ArrayList();
            ViewState["Group4"] = new ArrayList();
            ViewState["Group5"] = new ArrayList();
            ViewState["RowGroup"] = new ArrayList();


            var userPermissions = Security_Users.ins.getUserpermision(ZeroIntergerIFNull( lstJob.SelectedValue),ZeroIntergerIFNull( lstMember.SelectedValue));
            grdResult.DataSource = userPermissions;
            grdResult.DataBind();

            string sysid = "0";
            if ((ViewState["LastSystem"] != null))
            {
                sysid = Convert.ToString(ViewState["LastSystem"]);
            }

            CheckBox par = default(CheckBox);
            if ((Session["LastParent1"] != null))
            {
                def += "var gr_" + sysid + "Show = \"" + SerializeArray((ArrayList)ViewState["Group1"]) + "\";";
                par = (CheckBox)Session["LastParent1"];
                par.Attributes.Add("onclick", "CheckSystem(this,gr_" + sysid + "Show)");
            }
            if ((Session["LastParent2"] != null))
            {
                def += "var gr_" + sysid + "Modify = \"" + SerializeArray((ArrayList)ViewState["Group2"]) + "\";";
                par = (CheckBox)Session["LastParent2"];
                par.Attributes.Add("onclick", "CheckSystem(this,gr_" + sysid + "Modify)");
            }
            if ((Session["LastParent3"] != null))
            {
                def += "var gr_" + sysid + "Add = \"" + SerializeArray((ArrayList)ViewState["Group3"]) + "\";";
                par = (CheckBox)Session["LastParent3"];
                par.Attributes.Add("onclick", "CheckSystem(this,gr_" + sysid + "Add)");
            }
            if ((Session["LastParent4"] != null))
            {
                def += "var gr_" + sysid + "Delete = \"" + SerializeArray((ArrayList)ViewState["Group4"]) + "\";";
                par = (CheckBox)Session["LastParent4"];
                par.Attributes.Add("onclick", "CheckSystem(this,gr_" + sysid + "Delete)");
            }
            if ((Session["LastParent5"] != null))
            {
                def += "var gr_" + sysid + "Date = \"" + SerializeArray((ArrayList)ViewState["Group5"]) + "\";";
                par = (CheckBox)Session["LastParent5"];
                par.Attributes.Add("onclick", "CheckSystem(this,gr_" + sysid + "Date)");
            }

            ViewState["Group1"] = null;
            ViewState["Group2"] = null;
            ViewState["Group3"] = null;
            ViewState["Group4"] = null;
            ViewState["Group5"] = null;
            ViewState["LastSystem"] = null;
            Session["LastParent1"] = null;
            Session["LastParent2"] = null;
            Session["LastParent3"] = null;
            Session["LastParent4"] = null;
            Session["LastParent5"] = null;
            ViewState["Def"] = def;

            //Rows hidden/visible functionality
            if ((Session["RowImage"] != null) & (Session["RowLink"] != null))
            {

                System.Web.UI.HtmlControls.HtmlAnchor l = (System.Web.UI.HtmlControls.HtmlAnchor)Session["RowLink"];
                System.Web.UI.HtmlControls.HtmlImage im = (System.Web.UI.HtmlControls.HtmlImage)Session["RowImage"];
                ArrayList arr = (ArrayList)ViewState["RowGroup"];
                l.Attributes.Add("onclick", "ToggleSystemGroup('" + im.ClientID + "','" + SerializeArray(arr) + "')");
                im.Attributes.Add("onclick", "ToggleSystemGroup('" + im.ClientID + "','" + SerializeArray(arr) + "')");
            }
        }
        private string SerializeArray(ArrayList arr)
        {
            string ser = "";
            for (int i = 0; i <= arr.Count - 1; i++)
            {
                if (ser.Equals(""))
                {
                    ser = "" + Convert.ToString(arr[i]) + "";
                }
                else
                {
                    ser += "," + Convert.ToString(arr[i]) + "";
                }
            }
            return ser;
        }
        #endregion

        protected void grdResult_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header | e.Item.ItemType == ListItemType.Footer)
            {
                e.Item.Cells[3].Text = "&nbsp;";
            }
            if (e.Item.ItemType == ListItemType.AlternatingItem | e.Item.ItemType == ListItemType.Item)
            {
                string sysid = e.Item.Cells[0].Text.Trim();
                string pgid = e.Item.Cells[1].Text.Trim();

                if (!pgid.Trim().Equals("0"))
                {
                    // page permission
                    e.Item.Cells[3].Text = "&nbsp;";
                    CheckBox c1 = (CheckBox)e.Item.Cells[5].FindControl("chkShow");
                    CheckBox c2 = (CheckBox)e.Item.Cells[7].FindControl("chkModify");
                    CheckBox c3 = (CheckBox)e.Item.Cells[6].FindControl("chkAdd");
                    CheckBox c4 = (CheckBox)e.Item.Cells[8].FindControl("chkDelete");
                    CheckBox c5 = (CheckBox)e.Item.Cells[9].FindControl("chkDate");

                    CheckBox par = default(CheckBox);
                    if ((Session["LastParent1"] != null))
                    {
                        par = (CheckBox)Session["LastParent1"];
                        c1.Attributes.Add("onclick", "KeepCheck('" + par.ClientID + "',gr_" + sysid + "Show);");
                    }
                    if ((Session["LastParent2"] != null))
                    {
                        par = (CheckBox)Session["LastParent2"];
                        c2.Attributes.Add("onclick", "KeepCheck('" + par.ClientID + "',gr_" + sysid + "Modify);");
                    }
                    if ((Session["LastParent3"] != null))
                    {
                        par = (CheckBox)Session["LastParent3"];
                        c3.Attributes.Add("onclick", "KeepCheck('" + par.ClientID + "',gr_" + sysid + "Add);");
                    }
                    if ((Session["LastParent4"] != null))
                    {
                        par = (CheckBox)Session["LastParent4"];
                        c4.Attributes.Add("onclick", "KeepCheck('" + par.ClientID + "',gr_" + sysid + "Delete);");
                    }
                    if ((Session["LastParent5"] != null))
                    {
                        par = (CheckBox)Session["LastParent5"];
                        c5.Attributes.Add("onclick", "KeepCheck('" + par.ClientID + "',gr_" + sysid + "Date);");
                    }
                    ArrayList arr = (ArrayList)ViewState["Group1"];
                    arr.Add(c1.ClientID);
                    ViewState["Group1"] = arr;
                    arr = (ArrayList)ViewState["Group2"];
                    arr.Add(c2.ClientID);
                    ViewState["Group2"] = arr;
                    arr = (ArrayList)ViewState["Group3"];
                    arr.Add(c3.ClientID);
                    ViewState["Group3"] = arr;
                    arr = (ArrayList)ViewState["Group4"];
                    arr.Add(c4.ClientID);
                    ViewState["Group4"] = arr;
                    arr = (ArrayList)ViewState["Group5"];
                    arr.Add(c5.ClientID);
                    ViewState["Group5"] = arr;

                    //Rows hidden/visible functionality
                    arr = (ArrayList)ViewState["RowGroup"];
                   arr.Add(e.Item.ClientID);
                  e.Item.Style["display"] = "none";
                    ViewState["RowGroup"] = arr;
                   // e.Item.Visible = false;
                }
                else
                {
                    // system permissionB

                    CheckBox par = default(CheckBox);
                    if ((Session["LastParent1"] != null))
                    {
                        def += "var gr_" + Convert.ToString(ViewState["LastSystem"]) + "Show = \"" + SerializeArray((ArrayList)ViewState["Group1"]) + "\";";
                        par = (CheckBox)Session["LastParent1"];
                        par.Attributes.Add("onclick", "CheckSystem(this,gr_" + Convert.ToString(ViewState["LastSystem"]) + "Show)");
                    }
                    if ((Session["LastParent2"] != null))
                    {
                        def += "var gr_" + Convert.ToString(ViewState["LastSystem"]) + "Modify = \"" + SerializeArray((ArrayList)ViewState["Group2"]) + "\";";
                        par = (CheckBox)Session["LastParent2"];
                        par.Attributes.Add("onclick", "CheckSystem(this,gr_" + Convert.ToString(ViewState["LastSystem"]) + "Modify)");
                    }
                    if ((Session["LastParent3"] != null))
                    {
                        def += "var gr_" + Convert.ToString(ViewState["LastSystem"]) + "Add = \"" + SerializeArray((ArrayList)ViewState["Group3"]) + "\";";
                        par = (CheckBox)Session["LastParent3"];
                        par.Attributes.Add("onclick", "CheckSystem(this,gr_" + Convert.ToString(ViewState["LastSystem"]) + "Add)");
                    }
                    if ((Session["LastParent4"] != null))
                    {
                        def += "var gr_" + Convert.ToString(ViewState["LastSystem"]) + "Delete = \"" + SerializeArray((ArrayList)ViewState["Group4"]) + "\";";
                        par = (CheckBox)Session["LastParent4"];
                        par.Attributes.Add("onclick", "CheckSystem(this,gr_" + Convert.ToString(ViewState["LastSystem"]) + "Delete)");
                    }
                    if ((Session["LastParent5"] != null))
                    {
                        def += "var gr_" + Convert.ToString(ViewState["LastSystem"]) + "Date = \"" + SerializeArray((ArrayList)ViewState["Group5"]) + "\";";
                        par = (CheckBox)Session["LastParent5"];
                        par.Attributes.Add("onclick", "CheckSystem(this,gr_" + Convert.ToString(ViewState["LastSystem"]) + "Date)");
                    }
                    e.Item.Cells[4].Visible = false;
                    e.Item.Cells[3].ColumnSpan = 2;

                //    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#bdc7d5");
                    e.Item.Font.Bold = true;
                    CheckBox c1 = (CheckBox)e.Item.Cells[5].FindControl("chkShow");
                    CheckBox c2 = (CheckBox)e.Item.Cells[7].FindControl("chkModify");
                    CheckBox c3 = (CheckBox)e.Item.Cells[6].FindControl("chkAdd");
                    CheckBox c4 = (CheckBox)e.Item.Cells[8].FindControl("chkDelete");
                    CheckBox c5 = (CheckBox)e.Item.Cells[9].FindControl("chkDate");

                    ViewState["Group1"] = new ArrayList();
                    ViewState["Group2"] = new ArrayList();
                    ViewState["Group3"] = new ArrayList();
                    ViewState["Group4"] = new ArrayList();
                    ViewState["Group5"] = new ArrayList();
                    Session["LastParent1"] = c1;
                    Session["LastParent2"] = c2;
                    Session["LastParent3"] = c3;
                    Session["LastParent4"] = c4;
                    Session["LastParent5"] = c5;
                    ViewState["LastSystem"] = sysid;
                    //Rows hidden/visible functionality
                    if ((Session["RowImage"] != null) & (Session["RowLink"] != null))
                    {
                        System.Web.UI.HtmlControls.HtmlAnchor l = (System.Web.UI.HtmlControls.HtmlAnchor)Session["RowLink"];
                        System.Web.UI.HtmlControls.HtmlImage im = (System.Web.UI.HtmlControls.HtmlImage)Session["RowImage"];
                        ArrayList arr = (ArrayList)ViewState["RowGroup"];
                        l.Attributes.Add("onclick", "ToggleSystemGroup('" + im.ClientID + "','" + SerializeArray(arr) + "')");
                        im.Attributes.Add("onclick", "ToggleSystemGroup('" + im.ClientID + "','" + SerializeArray(arr) + "')");
                    }
                    System.Web.UI.HtmlControls.HtmlAnchor lnk = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.Cells[3].FindControl("lnkSystem");
                    System.Web.UI.HtmlControls.HtmlImage img = (System.Web.UI.HtmlControls.HtmlImage)e.Item.Cells[3].FindControl("imgSystem");
                    Session["RowImage"] = img;
                    Session["RowLink"] = lnk;
                    ViewState["RowGroup"] = new ArrayList();
                }
            }

        }

        private string bit(bool chk)
        {
            if (chk)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        protected void lnkSave_Click(object sender, System.EventArgs e)
	{

            Security_pr_Permission prObj = new Security_pr_Permission();


            Security_Users.ins.DeleteUserpermssion(ZeroIntergerIFNull(lstJob.SelectedValue), ZeroIntergerIFNull(lstMember.SelectedValue));



        for (int i = 0; i <= grdResult.Items.Count - 1; i++) {
			string sysid = grdResult.Items[i].Cells[0].Text.Replace("&nbsp;", " ").Trim();
			string pageid = grdResult.Items[i].Cells[1].Text.Replace("&nbsp;", " ").Trim();
			string prid = grdResult.Items[i].Cells[2].Text.Replace("&nbsp;", " ").Trim();
			CheckBox check = default(CheckBox);
			check = (CheckBox)grdResult.Items[i].Cells[5].FindControl("chkShow");
			string show = bit(check.Checked);

			check = (CheckBox)grdResult.Items[i].Cells[7].FindControl("chkModify");
			string modify = bit(check.Checked);

			check = (CheckBox)grdResult.Items[i].Cells[6].FindControl("chkAdd");
			string @add = bit(check.Checked);

			check = (CheckBox)grdResult.Items[i].Cells[8].FindControl("chkDelete");
			string delete = bit(check.Checked);

			check = (CheckBox)grdResult.Items[i].Cells[9].FindControl("chkDate");
			string dateControl = bit(check.Checked);

                prObj = new Security_pr_Permission();
                prObj.jobid = ZeroIntergerIFNull(lstJob.SelectedValue);
                prObj.userid = ZeroIntergerIFNull(lstMember.SelectedValue);
                prObj.sysid =ZeroIntergerIFNull( sysid);
                prObj.pageid =ZeroIntergerIFNull( pageid);
                prObj.show = getBool(show);
                prObj.modify = getBool(modify);
                prObj.AddRecord = getBool(@add);
                prObj.DeleteRecord = getBool(delete);
                prObj.DateControl = getBool(dateControl);

                Security_Users.ins.AddPermission(prObj);





              //  SitePermissions.ins.Insert(lstJob.SelectedValue, lstMember.SelectedValue, sysid, pageid, show, modify, @add, delete, dateControl);
			//If prid.Trim().Equals("0") Then ' this permission isn't stored yet!
			//    NewPermissions.ins.Insert( _
			//    lstJob.SelectedValue, lstMember.SelectedValue, sysid, _
			//    pageid, show, modify, add, delete, dateControl)
			//Else ' this permission is already stored, update it !!
			//    NewPermissions.ins.Update(prid, _
			//    lstJob.SelectedValue, lstMember.SelectedValue, sysid, _
			//    pageid, show, modify, add, delete, dateControl)
			//End If
		}

		FillPermissions();
		lblError.ForeColor  = System.Drawing.Color.Blue;
       string  script = FormatpopupErrorMSG(Resources.Alerts.DataSavedSuccessfully, "3");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Updatepanel1", script, true);

	}

        protected void lbkColse_Click(object sender, System.EventArgs e)
	{
            Security_Users.ins.DeleteUserpermssion(ZeroIntergerIFNull(lstJob.SelectedValue), ZeroIntergerIFNull(lstMember.SelectedValue));
            FillPermissions();
	}
        public PermissionsNew()
        {
            Load += Page_Load;
            PreRender += Page_PreRender;
        }

        protected void lstJob_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }
    }
}