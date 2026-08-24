using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Web.Helper
{
    public class LawSummaryDTO
    {
        // Extra columns from SP
        public int SourceDocId { get; set; }
        public int DestDocId { get; set; }
        public string DocDescriptionHTML { get; set; } // HTML with icons/colors
        public int RelatedLawsCount { get; set; }
        public int Level { get; set; }
        public int ParentDocId { get; set; }
        public int ChildDocId { get; set; }
        public string Path { get; set; }// number of related laws
           
        // All D.* columns
        public int Code { get; set; }                        // not nullable
        public int? DocTypeID { get; set; }
        public string DocSerial { get; set; }
        public int? DocNum { get; set; }
        public int? DocYear { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public string DocSubject { get; set; }
        public DateTime? DocDate { get; set; }
        public bool? UnderStudy { get; set; }
        public int? DocCategoryID { get; set; }
        public bool? isPublished { get; set; }
        public DateTime? PublishDate { get; set; }
        public string PublishVersion { get; set; }
        public string DocFilepath { get; set; }
        public string DocDetails { get; set; }
        public string DocNotes { get; set; }
        public int? LastProcedureID { get; set; }
        public int? aUser { get; set; }
        public string kng_Dession { get; set; }
        public string DocFilepath_published { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool? isDeleted { get; set; }
        public bool? isTransfered { get; set; }
        public int? RefId { get; set; }
        public bool? LawCancelled { get; set; }
        public DateTime? LawCancelDate { get; set; }
        public bool? isPrivate { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}