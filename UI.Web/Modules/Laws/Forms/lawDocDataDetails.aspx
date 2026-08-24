<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Empty.Master" AutoEventWireup="true" CodeBehind="lawDocDataDetails.aspx.cs" Inherits="UI.Web.Modules.Laws.Forms.lawDocDataDetails" %>
<%@ Register TagPrefix="cc1" Namespace="CutePager" Assembly="ASPnetPagerV2netfx2_0" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style type="text/css">
        .law-doc-container {
            background-color: #ffffff;
            border-radius: 4px;
            padding: 20px;
            margin: 20px auto;
            box-shadow: 0 1px 3px rgba(0,0,0,0.12);
        }

        .doc-header {
            border-bottom: 2px solid #00BCD4;
            padding-bottom: 15px;
            margin-bottom: 20px;
        }

        .doc-header h3 {
            color: #333;
            margin: 0;
            font-weight: bold;
        }

        .doc-content {
            line-height: 1.8;
            color: #555;
            font-size: 14px;
            white-space: pre-wrap;
            word-wrap: break-word;
        }

        .highlight-search {
            background-color: #FFFF00;
            color: #000;
            font-weight: bold;
            padding: 3px 6px;
            border-radius: 3px;
            box-shadow: 0 0 3px rgba(255, 193, 7, 0.5);
        }

        .doc-info {
            background-color: #f5f5f5;
            border-left: 4px solid #00BCD4;
            padding: 15px;
            margin-bottom: 20px;
            border-radius: 4px;
        }

        .doc-info p {
            margin: 8px 0;
            font-size: 13px;
        }

        .doc-info label {
            font-weight: bold;
            color: #333;
            display: inline-block;
            min-width: 120px;
        }

        .btn-back {
            margin-bottom: 20px;
        }

        .close-btn {
            float: left;
        }

        @media print {
            .btn-back, .close-btn {
                display: none;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
     <div class="law-doc-container">
     <div class="btn-back">
         <a href="javascript:parent.$.colorbox.close();" class="btn btn-default close-btn">
             <i class="fa fa-times"></i>&nbsp; إغلاق
         </a>
     </div>

     <div class="doc-header">
         <h3>
             <i class="icon-file-text2 position-left"></i>
             تفاصيل الوثيقة
         </h3>
     </div>

     <div class="doc-info">
         <p>
             <label>رقم الوثيقة:</label>
             <span>
                 <asp:Label ID="lblDocSerial" runat="server"></asp:Label>
             </span>
         </p>
         <p>
             <label>تاريخ الإصدار:</label>
             <span>
                 <asp:Label ID="lblDocDate" runat="server"></asp:Label>
             </span>
         </p>
         <p>
             <label>الموضوع:</label>
             <span>
                 <asp:Label ID="lblDocSubject" runat="server"></asp:Label>
             </span>
         </p>
     </div>

    <div class="doc-content" style="height: 80vh; overflow-y: auto;">
    <asp:Literal ID="litDocDetails" runat="server"></asp:Literal>
</div>
 </div>

 <script type="text/javascript">
     $(document).ready(function() {
         // Auto-adjust iframe height
         if (parent !== window) {
             try {
                 var height = document.body.scrollHeight + 50;
                 parent.$('#cboxLoadedContent').height(height);
             } catch (e) {
                 // Handle cross-domain issues
             }
         }
     });
 </script>
</asp:Content>
