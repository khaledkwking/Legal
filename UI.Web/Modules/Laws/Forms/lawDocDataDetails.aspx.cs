using Infrastructure;
using Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UI.Web.Modules.Laws.Forms
{
    public partial class lawDocDataDetails : System.Web.UI.Page
    {
        private LawsRepository objRepository = IoC.Resolve<LawsRepository>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDocumentDetails();
            }
        }

        private void LoadDocumentDetails()
        {
            try
            {
                int lawDocId = 0;
                string searchText = string.Empty;

                // Get query string parameters
                if (Request.QueryString["LawDocID"] != null)
                {
                    int.TryParse(Request.QueryString["LawDocID"], out lawDocId);
                }

                if (Request.QueryString["txtsearch"] != null)
                {
                    searchText = Request.QueryString["txtsearch"].Trim();
                }

                if (lawDocId == 0)
                {
                    litDocDetails.Text = "<p class='alert alert-warning'>لم يتم تحديد وثيقة</p>";
                    return;
                }

                // Get document details from repository
                var docData = objRepository.GetDetails(lawDocId);

                if (docData == null)
                {
                    litDocDetails.Text = "<p class='alert alert-warning'>لم يتم العثور على الوثيقة</p>";
                    return;
                }

                // Populate document information
                lblDocSerial.Text = gets(docData.DocSerial);
                lblDocDate.Text = FormatDate(docData.DocDate);
                lblDocSubject.Text = gets(docData.DocSubject);

                // Get document details text
                string docDetailsText = gets(docData.DocDetails);

                // Highlight search text if provided
                if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(docDetailsText))
                {
                    docDetailsText = HighlightText(docDetailsText, searchText);
                }

                litDocDetails.Text = docDetailsText;
            }
            catch (Exception ex)
            {
                litDocDetails.Text = $"<p class='alert alert-danger'>خطأ: {ex.Message}</p>";
            }
        }

        /// <summary>
        /// Highlights search text in the document details with background color
        /// </summary>
        private string HighlightText(string text, string searchText)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(searchText))
            {
                return text;
            }

            try
            {
                // Escape special regex characters in the search text
                string pattern = System.Text.RegularExpressions.Regex.Escape(searchText);
                
                // Create replacement with highlight span
                string replacement = $"<span class='highlight-search'>{System.Web.HttpUtility.HtmlEncode(searchText)}</span>";

                // Replace with case-insensitive matching
                string highlightedText = System.Text.RegularExpressions.Regex.Replace(
                    text,
                    pattern,
                    replacement,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase
                );

                return highlightedText;
            }
            catch
            {
                // If regex fails, return original text
                return text;
            }
        }

        /// <summary>
        /// Formats date to DD/MM/YYYY format
        /// </summary>
        private string FormatDate(DateTime? dateTime)
        {
            if (!dateTime.HasValue || dateTime.Value == DateTime.MinValue)
            {
                return "-";
            }

            return dateTime.Value.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Helper method to safely get string values
        /// </summary>
        private string gets(object obj)
        {
            if (obj == null)
                return string.Empty;

            if (obj is DBNull)
                return string.Empty;

            return obj.ToString();
        }
    }
}