using Infrastructure.DAL.Enum;
using Infrastructure.DAL.Model;
using Infrastructure.DAL.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DAL
{
    public  class LawsRepository : BaseRepository
    {

        public LawsRepository(CMGS_DBEntities _context) : base(_context)
        {

        }

        #region "Law master Data"

        #region List

        public List<View_LawsDocs> GetDecisionsList(int SerialNum, int SerialYear,
          DateTime TransactionDatFrom, DateTime TransactionDatTo,
          int DocType, int DocCategory,
          int IsUnderStudy, string LawSubject, string LawDetails,
          int isPublished, string lawDocSerial, string SearchKeys, int lastProcedureId, Boolean isPrivate, int PrivateType)
        {


            var result =
                (from obj in DC.View_LawsDocs
                     //orderby obj.Q_Serial descending    chnaged by Nada Request ib 12052018
                 orderby obj.DocYear descending, obj.DocNum descending //obj.DocDate
                 where 1 == 1
             && (lawDocSerial != "" ? obj.DocSerial == lawDocSerial : 1 == 1)
             && (SerialNum != 0 ? obj.DocNum == SerialNum : 1 == 1)
             && (SerialYear != 0 ? obj.DocYear == SerialYear : 1 == 1)

             && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.DocDate >= TransactionDatFrom : 1 == 1) || (TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.DocDate >= TransactionDatFrom : 1 == 1))
             && ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.DocDate <= TransactionDatTo : 1 == 1) || (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.DocDate <= TransactionDatTo : 1 == 1))

             && (DocType != 0 ? obj.DocTypeID == DocType : (obj.DocTypeID == (int)LawDoc_TypesEnum.Dession || obj.DocTypeID == (int)LawDoc_TypesEnum.PmDession))

             && (IsUnderStudy != 0 ? IsUnderStudy == 1 ? obj.UnderStudy == true : obj.UnderStudy == false : 1 == 1)

             && (isPublished != 0 ? isPublished == 1 ? obj.isPublished == true : obj.isPublished == false : 1 == 1)
             && (DocCategory != 0 ? obj.DocCategoryID == DocCategory : 1 == 1)
             //&& (assignedPerson != 0 ? obj.AssignedPersonID == assignedPerson : 1 == 1)
             //&& (StatusID != 0 ? obj.StatusID == StatusID : 1 == 1)
             //&& (RequestTo != 0 ? obj.Q_RequestTo == RequestTo : 1 == 1)
             //&& (selectedRequestedFrom.Count > 0 ? selectedRequestedFrom.Contains(obj.Q_RequestFrom.Value) : 1 == 1)
             && (LawSubject != "" ? (obj.DocSubject != null && obj.DocSubject.Contains(LawSubject)) : 1 == 1)
             && (LawDetails != "" ? (obj.DocDetails != null && obj.DocDetails.Contains(LawDetails)) : 1 == 1)
             && (lastProcedureId != 0 ? obj.ProcedureTypeCode == lastProcedureId : 1 == 1)
             && (isPrivate != true ? obj.isPrivate == false : (PrivateType == 0) ? 1 == 1 : PrivateType == 1 ? obj.isPrivate == true : obj.isPrivate == false)
                 ////&& (RelatedOrgs.Count > 0 ? RelatedOrgs.Contains(obj.Q_RequestTo.Value) : 1 == 1)
                 //&& (RelatedOrgs != 0 ? DC.Parliament_Requestedby
                 //                         .Where(x => x.PersonID == RelatedOrgs)
                 //                         .Select(x => x.LawID).Contains(obj.code)
                 //                         : 1 == 1)
                 select obj);



            var _out = result.ToList<View_LawsDocs>();
            PostResultToAudit((int)SysModulesRef.Legislation, "DecisionsList", "Search Result /GetList", SearchKeys, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;


        }

        public List<View_LawsDocs> GetListBySerialNum(string docNumbers)
        {
            var numbers = docNumbers
                 .Split(',')
                 .Where(x => !string.IsNullOrWhiteSpace(x))
                 .Select(x => int.Parse(x.Trim()))
                 .ToList();

            var result =
                from obj in DC.View_LawsDocs
                orderby obj.DocYear descending, obj.DocNum descending
                where !numbers.Any() || numbers.Contains(obj.Code)
                select obj;

            //var result =
            //      (from obj in DC.View_LawsDocs
            //           //orderby obj.Q_Serial descending    chnaged by Nada Request ib 12052018
            //           //orderby obj.DocDate descending // obj.DocNum, obj.DocYear 
            //       orderby obj.DocYear descending, obj.DocNum descending
            //       where 1 == 1
            //       && (SerialNum != 0 ? obj.DocNum == SerialNum : 1 == 1)
            //       && (SerialYear != 0 ? obj.DocYear == SerialYear : 1 == 1)

              
            //       select obj);

            var _out = result.ToList<View_LawsDocs>();
           PostResultToAudit((int)SysModulesRef.Legislation, nameof(SysModulesRef.Legislation), "Search Result /GetList", numbers.ToString(), JsonConvert.SerializeObject(_out), _out.Count);
            return _out;

        }
        public List<View_LawsDocs> GetList(int SerialNum, int SerialYear,
            DateTime TransactionDatFrom, DateTime TransactionDatTo,
            int DocType, int DocCategory,
            int IsUnderStudy, string LawSubject, string LawNotes, string LawDetails, int isPublished, int lastProcedureId, DateTime ExpireDatefrom, DateTime ExpireDateTo, string SearchKyes)
        {
            var minDate = new DateTime(1990, 01, 01);

            IQueryable<View_LawsDocs> query = DC.View_LawsDocs;

            if (SerialNum != 0)
                query = query.Where(obj => obj.DocNum == SerialNum);

            if (SerialYear != 0)
                query = query.Where(obj => obj.DocYear == SerialYear);

            if (TransactionDatFrom != minDate)
                query = query.Where(obj => obj.DocDate >= TransactionDatFrom);

            if (TransactionDatTo != minDate)
                query = query.Where(obj => obj.DocDate <= TransactionDatTo);

            if (ExpireDatefrom != minDate)
                query = query.Where(obj => obj.ExpireDate >= ExpireDatefrom);

            if (ExpireDateTo != minDate)
                query = query.Where(obj => obj.ExpireDate <= ExpireDateTo);

            if (DocType != 0)
                query = query.Where(obj => obj.DocTypeID == DocType);
            else
                query = query.Where(obj => obj.DocTypeID != (int)LawDoc_TypesEnum.Dession
                    && obj.DocTypeID != (int)LawDoc_TypesEnum.PmDession
                    && obj.DocTypeID != (int)LawDoc_TypesEnum.LawProject);

            if (IsUnderStudy != 0)
                query = query.Where(obj => obj.UnderStudy == (IsUnderStudy == 1));

            if (isPublished != 0)
                query = query.Where(obj => obj.isPublished == (isPublished == 1));

            if (DocCategory != 0)
                query = query.Where(obj => obj.DocCategoryID == DocCategory);

            if (lastProcedureId != 0)
                query = query.Where(obj => obj.ProcedureTypeCode == lastProcedureId);

            if (LawSubject != "")
                query = query.Where(obj => obj.DocSubject.Contains(LawSubject));

            if (LawNotes != "")
                query = query.Where(obj => obj.DocSubject.Contains(LawNotes));

            if (LawDetails != "")
                query = query.Where(obj => obj.DocDetails.Contains(LawDetails));

            query = query.OrderByDescending(obj => obj.DocYear).ThenByDescending(obj => obj.DocNum);

            var _out = query.ToList<View_LawsDocs>();

            PostResultToAudit((int)SysModulesRef.Legislation, nameof(SysModulesRef.Legislation), "Search Result /GetList", SearchKyes, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;
        }


        public List<View_LawsDocs> GetlawProjects(int SerialNum, int SerialYear,
          DateTime TransactionDatFrom, DateTime TransactionDatTo,
          int DocType, int DocCategory,
          int IsUnderStudy, string LawSubject, string LawDetails, int lastProcedureId, string SearchKyes, int isTransfered)
        {


            var result =
                (from obj in DC.View_LawsDocs
                     //orderby obj.Q_Serial descending    chnaged by Nada Request ib 12052018
                 orderby obj.DocDate descending
                 where 1 == 1
             && (SerialNum != 0 ? obj.DocNum == SerialNum : 1 == 1)
             && (SerialYear != 0 ? obj.DocYear == SerialYear : 1 == 1)

             && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.DocDate >= TransactionDatFrom : 1 == 1) || (TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.DocDate >= TransactionDatFrom : 1 == 1))
             && ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.DocDate <= TransactionDatTo : 1 == 1) || (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.DocDate <= TransactionDatTo : 1 == 1))

             && (DocType != 0 ? obj.DocTypeID == DocType : (obj.DocTypeID == (int)LawDoc_TypesEnum.LawProject))

             && (IsUnderStudy != 0 ? IsUnderStudy == 1 ? obj.UnderStudy == true : obj.UnderStudy == false : 1 == 1)
             && (isTransfered != 0 ? isTransfered == 1 ? obj.isTransfered == true : (obj.isTransfered == false || obj.isTransfered == null) : 1 == 1)

             && (DocCategory != 0 ? obj.DocCategoryID == DocCategory : 1 == 1)
             && (lastProcedureId != 0 ? obj.ProcedureTypeCode == lastProcedureId : 1 == 1)

             && (LawSubject != "" ? (obj.DocSubject != null && obj.DocSubject.Contains(LawSubject)) : 1 == 1)
             && (LawDetails != "" ? (obj.DocDetails != null && obj.DocDetails.Contains(LawDetails)) : 1 == 1)

                 select obj);

            var _out = result.ToList<View_LawsDocs>();
            PostResultToAudit((int)SysModulesRef.Legislation, nameof(SysModulesRef.Legislation), "Search Result /GetList", SearchKyes, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;

        }
        public Law_DocData GetDetails(int _Code)
        {
            var result =
                (from obj in DC.Law_DocData
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<Law_DocData>();
        }
        public Medal_Data GetMedalDetails(int _Code)
        {
            var result =
                (from obj in DC.Medal_Data
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<Medal_Data>();
        }
        public Law_DocData GetDetailsByType(int DocTypeID,int DocCategoryID)
        {
            var result =
                (from obj in DC.Law_DocData
                 where obj.DocTypeID == DocTypeID && obj.DocCategoryID == DocCategoryID
                 select obj);
            return result.FirstOrDefault<Law_DocData>();
        }
        public Law_DocData GetDetailswithProcedures(int _Code)
        {
            var result =
                (from obj in DC.Law_DocData
                 .Include("Law_DocProcedures")
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<Law_DocData>();
        }

        public Law_DocData GetDetailsForEdit(int _Code)
        {
            var result =
                (from obj in DC.Law_DocData
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<Law_DocData>();

        }

        public Law_DocProcedures GetLastProcedureobj(int _DocCode)
        {


            var result =
                (from obj in DC.Law_DocProcedures
                 where obj.DocRefID == _DocCode
                 orderby obj.ProcedureDate descending
                 select obj).FirstOrDefault<Law_DocProcedures>();


            return result;



        }


        public bool CheckDocExistance(int DocNum, int DocYear, int DocTypeID, int fileRefID)
        {


            var result =
                (from obj in DC.Law_DocData
                 where obj.DocNum == DocNum && obj.DocYear == DocYear && obj.DocTypeID == DocTypeID && obj.Code != fileRefID
                 select obj).FirstOrDefault<Law_DocData>();
            if (result != null)
            {
                return true;
            }


            return false;
        }


        #endregion

        #region "Add ,  Update  ,Delete" Law Operation

        public int AddLaw<T>(T item)
        {


            DC.Law_DocData.Add(item as Law_DocData);
            return DC.SaveChanges();

        }

        public int DeleteLaw<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Law_DocData).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateLaw<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Law_DocData).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region "Law Child"

        #region"Attachemnt"


        public law_Attachments GetAttachemtnDetails(int _Code)
        {


            var result =
                (from obj in DC.law_Attachments
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<law_Attachments>();

        }

        public List<law_Attachments> FillLawAttachemnt(int RefTypeID, int RefID)
        {


            var result =
                (from obj in DC.law_Attachments
                 .Include("law_AttachmentTypes")
                 where obj.RefTypeID == RefTypeID && obj.RefID == RefID
                 select obj);
            return result.ToList<law_Attachments>();

        }



        #region "Add ,  Update  ,Delete" Medal proceduee

        public int AddAttachement<T>(T item)
        {


            DC.law_Attachments.Add(item as law_Attachments);
            return DC.SaveChanges();

        }

        public int DeleteAttachment<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as law_Attachments).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateAttachment<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as law_Attachments).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion


        #region"Attachemnt Main"


        public Medalmain_Attachments GetAttachemtnMainDetails(int _Code)
        {


            var result =
                (from obj in DC.Medalmain_Attachments
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Medalmain_Attachments>();

        }

        public List<Medalmain_Attachments> FillMedalMainAttachemnt(int _Code)
        {


            var result =
                (from obj in DC.Medalmain_Attachments
                 .Include("Medal_M_AttachmentTypes")
                 where obj.MedalMasterCode == _Code
                 select obj);

            return result.ToList<Medalmain_Attachments>();

        }

        #region "Add ,  Update  ,Delete" Medal proceduee

        public int AddAttachementmain<T>(T item)
        {


            DC.Medalmain_Attachments.Add(item as Medalmain_Attachments);
            return DC.SaveChanges();

        }

        public int DeleteAttacjmentmain<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Medalmain_Attachments).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateAttachmentMain<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Medalmain_Attachments).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region"Procedures"
        public Law_DocProcedures GetProceduresDetails(int? _Code)
        {


            var result =
                (from obj in DC.Law_DocProcedures
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Law_DocProcedures>();

        }
        public List<Law_DocProcedures> GetAllProceduresDetails(int? _Code)
        {


            var result =
                (from obj in DC.Law_DocProcedures
                 where obj.Code == _Code
                 select obj);

            return result.ToList ();

        }
        public List<Law_DocProcedures> FillLawProcedures(int RefDocID)
        {


            var result =
                (from obj in DC.Law_DocProcedures
                 .Include("law_DocProceduresTypes")
                 orderby obj.ProcedureDate descending
                 where obj.DocRefID == RefDocID
                 select obj);

            return result.ToList<Law_DocProcedures>();

        }



        #region "Add ,  Update  ,Delete" Procedures proceduee

        public int AddProcedures<T>(T item)
        {


            DC.Law_DocProcedures.Add(item as Law_DocProcedures);
            return DC.SaveChanges();

        }

        public int DeleteProcedures<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Law_DocProcedures).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateProcedures<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Law_DocProcedures).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion

        #endregion


        #region"Coming"
        public arc_Data GetArcDocDetails(int _Code)
        {


            var result =
                (from obj in DC.arc_Data
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<arc_Data>();

        }
        public List<arc_Data> FillArcDocs(int Doc_Type, int RefDocID, int parentid = 0)
        {


            var result =
                (from obj in DC.arc_Data

                 where obj.TargetModule == (int)ArcTargetModules.DecisionModules && obj.Doc_Type == Doc_Type && obj.RefDocID == RefDocID
                 && (parentid != 0 ? obj.ParentArcRefID == parentid : 1 == 1)
                 select obj);

            return result.ToList<arc_Data>();

        }

        public int getchildDocs(int parentid)
        {


            var result =
                (from obj in DC.arc_Data
                 where obj.TargetModule == (int)ArcTargetModules.DecisionModules && obj.ParentArcRefID == parentid

                 select obj).Count();

            return result;

        }
        #region "Add ,  Update  ,Delete" Arcs

        public int AddArcData<T>(T item)
        {


            DC.arc_Data.Add(item as arc_Data);
            return DC.SaveChanges();

        }

        public int DeleteArcData<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as arc_Data).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateArcData<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as arc_Data).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region "Linked Agreemrnt"
        public View_AgreeementsList GetlawDocLinkedAgreemt(int _LawDocCode)
        {


            var result =
                (from obj in DC.View_AgreeementsList
                 where obj.Law_DocDataRefId == _LawDocCode
                 select obj);
            return result.FirstOrDefault<View_AgreeementsList>();

        }

        #endregion


        #region "Linked Docs"


        public Law_DocData_Linked GetLinkDetails(int _Code)
        {
            var result =
                (from obj in DC.Law_DocData_Linked
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<Law_DocData_Linked>();
        }

        public List<View_LawsLinkedDocs> GetRelatedDocs(int _LawDocCode)
        {


            var result =
                (from obj in DC.View_LawsLinkedDocs
                 orderby obj.DocNum, obj.DocYear
                 where obj.SouceDocID == _LawDocCode
                 select obj);
            return result.ToList<View_LawsLinkedDocs>();

        }

        public bool CheckLinkExostance(int DouceDocID, int DestDocID)
        {
            var result =
                (from obj in DC.View_LawsLinkedDocs
                 where obj.SouceDocID == DouceDocID && obj.DestDocId == DestDocID
                 select obj).ToList();
            if (result != null && result.Count > 0)
            {
                return true;

            }
            return false;

        }


        public int DeleteDocLink<T>(T item)
        {


            DC.Entry(item as Law_DocData_Linked).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateDocLink<T>(T item)
        {


            DC.Entry(item as Law_DocData_Linked).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }
        public int AddLnkedDoc<T>(T item)
        {


            DC.Law_DocData_Linked.Add(item as Law_DocData_Linked);
            return DC.SaveChanges();

        }

        #endregion
    }
}
