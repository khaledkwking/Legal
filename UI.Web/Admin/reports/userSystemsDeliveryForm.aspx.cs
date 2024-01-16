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
using Utilities;

namespace UI.Web.Admin.Pages
{
    public partial class userSystemsDeliveryForm : BaseFormAdmin
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            PageUrl = "AdminManager.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
 fillreport();
            }
                
          

        }

        private void fillreport()
        {
            var userModules = Security_Users.ins.getUserSystems(ZeroIntergerIFNull( Request.QueryString["uid"].ToString()));

            
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/admin/RDLC/UserSystemsDeliveryForm.rdlc");
            // ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("report_title", "إحصائية الاسئلة لاعضاء مجلس الامة"));
            DataTable TempDT = ConvertHelper.ToDataTable(userModules);
            TempDT.TableName = "View_UserSystems";
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(TempDT.TableName, TempDT));
        }

        
    }
}