using Infrastructure.DAL.Model;
using Infrastructure.DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Infrastructure.DAL
{
    public partial class LegalMemoRepository : BaseRepository
    {

        public LegalMemoRepository(CMGS_DBEntities _context) : base(_context)
        {

        }

        #region "LegalMemo master Data"

        #region List

        public arc_Data FillLastFollow(int RefDocID)
        {


            var result =
                (from obj in DC.arc_Data
                 orderby obj.NextFollowReminderDate descending
                 where obj.TargetModule == (int)ArcTargetModules.LegalMemo && obj.RefDocID == RefDocID
                 && (obj.NextFollowReminderDate != null || obj.NextFollowReminderDate != default(DateTime))
                 select obj).FirstOrDefault();

            return result;

        }
        public List<View_LegalMemosDocs> GetList(string AutoNum, int SerialNum, int SerialYear,
            DateTime TransactionDatFrom, DateTime TransactionDatTo,
            int DocCategory, int OrgId, int assignedPersons, int procedureId,
            int IsUnderStudy, string LegalMemoSubject, string SearchKyes, 
            DateTime FollowDateFrom, DateTime FollowDateTo, int StatusId, int ConsultantId)
        {


            var result =
                (from obj in DC.View_LegalMemosDocs
                 orderby obj.DocDate descending // obj.DocNum, obj.DocYear 
                 //orderby obj.DocYear descending, obj.FileNum descending
                 where 1 == 1
                 && (SerialNum != 0 ? obj.FileNum == SerialNum : 1 == 1)
                 && (SerialYear != 0 ? obj.DocYear == SerialYear : 1 == 1)
                 && (AutoNum != "" ? obj.DocSerial == AutoNum : 1 == 1)

                 && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.DocDate >= TransactionDatFrom : 1 == 1))
                 && ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.DocDate <= TransactionDatTo : 1 == 1))
                 && (IsUnderStudy != 0 ? IsUnderStudy == 1 ? obj.UnderStudy == true : obj.UnderStudy == false : 1 == 1)


                 && (DocCategory != 0 ? obj.DocCategoryId == DocCategory : 1 == 1)
                 && (procedureId != 0 ? obj.ProcedureId == procedureId : 1 == 1)
                 && (assignedPersons != 0 ? obj.assignedEmp == assignedPersons : 1 == 1)
                 && (OrgId != 0 ? obj.OrgId == OrgId : 1 == 1)

                 && (StatusId != 0 ? obj.DocStatusId == StatusId : 1 == 1)
                  && (ConsultantId != 0 ? obj.ConsultantId == ConsultantId : 1 == 1)

                 && (LegalMemoSubject != "" ? obj.DocSubject.Contains(LegalMemoSubject) : 1 == 1)

                 && ((FollowDateFrom != new DateTime(1990, 01, 01) ? (from arc in DC.arc_Data where arc.TargetModule == (int)ArcTargetModules.LegalMemo && arc.SentDate >= FollowDateFrom select arc.RefDocID).ToList().Contains(obj.Code) : 1 == 1))
                 && ((FollowDateTo != new DateTime(1990, 01, 01) ? (from arc in DC.arc_Data where arc.TargetModule == (int)ArcTargetModules.LegalMemo && arc.SentDate <= FollowDateTo select arc.RefDocID).ToList().Contains(obj.Code) : 1 == 1))

                 select obj);

            var _out = result.ToList<View_LegalMemosDocs>();
            PostResultToAudit((int)SysModulesRef.Legislation, nameof(SysModulesRef.Legislation), "Search Result /GetList", SearchKyes, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;
        }


        public int getMemoCountForCurrentYear(int DocYear)
        {
            var result =
                (from obj in DC.LegalMemo
                 where obj.DocDate.Value.Year == DocYear
                 select obj);
            return result.Count();
        }



        public LegalMemo GetDetails(int _Code)
        {
            var result =
                (from obj in DC.LegalMemo
                 .Include("LegalMemo_Org")
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<LegalMemo>();
        }
        public Medal_Data GetMedalDetails(int _Code)
        {
            var result =
                (from obj in DC.Medal_Data
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<Medal_Data>();
        }
        public LegalMemo GetDetailsForEdit(int _Code)
        {
            var result =
                (from obj in DC.LegalMemo
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<LegalMemo>();

        }

        public bool CheckDocExistance(int DocNum, int DocYear, int DocTypeID, int fileRefID)
        {


            var result =
                (from obj in DC.LegalMemo
                 where obj.FileNum == DocNum && obj.DocYear == DocYear && obj.DocCategoryId == DocTypeID && obj.Code != fileRefID
                 select obj).FirstOrDefault<LegalMemo>();
            if (result != null)
            {
                return true;
            }


            return false;
        }
        public int ResetDocSerials()
        {
            return DC.sp_LegalMemoResetSerial();
        }

        #region "Orgs"

        public bool checkItemExistance(int ChapterId, string itemName, int RefId)
        {

            var result =
                (from obj in DC.LegalMemo_Org
                 where obj.CatId == ChapterId && obj.NameAr == itemName && obj.Code != RefId
                 select obj).FirstOrDefault();

            if (result != null)
            {
                return true;
            }

            return false;
        }


        public List<LegalMemo_Org> FilterLegalMemo_Org(int catId)
        {


            var result =
                (from obj in DC.LegalMemo_Org
                 .Include("LegalMemo_OrgCat")
                     //orderby obj.DocYear descending, obj.FileNum descending
                 where 1 == 1
                 && (catId != 0 ? obj.CatId == catId : 1 == 1)
                 select obj);

            var _out = result.ToList<LegalMemo_Org>();
            return _out;
        }

        public LegalMemo_Org GetOrgDetails(int _Code)
        {
            var result =
                (from obj in DC.LegalMemo_Org
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<LegalMemo_Org>();
        }


        #region "Add ,  Update  ,Delete" LegalMemo Operation

        public int AddLegalMemoOrg<T>(T item)
        {


            DC.LegalMemo_Org.Add(item as LegalMemo_Org);
            return DC.SaveChanges();

        }

        public int DeleteLegalMemoOrg<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as LegalMemo_Org).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateLegalMemoOrg<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as LegalMemo_Org).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion


        #endregion



        #endregion

        #region "Add ,  Update  ,Delete" LegalMemo Operation

        public int AddLegalMemo<T>(T item)
        {


            DC.LegalMemo.Add(item as LegalMemo);
            return DC.SaveChanges();

        }

        public int DeleteLegalMemo<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as LegalMemo).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateLegalMemo<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as LegalMemo).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region "LegalMemo Child"

        //#region"Attachemnt"


        //public LegalMemo_Attachments GetAttachemtnDetails(int _Code)
        //{


        //        var result =
        //            (from obj in DC.LegalMemo_Attachments
        //             where obj.Code == _Code
        //             select obj);

        //        return result.FirstOrDefault<LegalMemo_Attachments>();

        //}

        //public List<LegalMemo_Attachments> FillLegalMemoAttachemnt(int RefTypeID, int RefID)
        //{


        //        var result =
        //            (from obj in DC.LegalMemo_Attachments
        //             .Include("LegalMemo_AttachmentTypes")
        //             where obj.RefTypeID == RefTypeID && obj.RefID == RefID
        //             select obj);
        //        return result.ToList<LegalMemo_Attachments>();

        //}



        //#region "Add ,  Update  ,Delete" Medal proceduee

        //public int AddAttachement<T>(T item)
        //{


        //        DC.LegalMemo_Attachments.Add(item as LegalMemo_Attachments);
        //        return DC.SaveChanges();

        //}

        //public int DeleteAttachment<T>(T item)
        //{


        //        //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
        //        DC.Entry(item as LegalMemo_Attachments).State = System.Data.Entity.EntityState.Deleted;
        //        return DC.SaveChanges();

        //}

        //public int UpdateAttachment<T>(T item)
        //{


        //        // Mark entity as modified
        //        DC.Entry(item as LegalMemo_Attachments).State = System.Data.Entity.EntityState.Modified;
        //        return DC.SaveChanges();

        //}


        //#endregion

        //#endregion


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
                 orderby obj.SentDate descending
                 where obj.TargetModule == (int)ArcTargetModules.LegalMemo && obj.Doc_Type == Doc_Type && obj.RefDocID == RefDocID
                 && (parentid != 0 ? obj.ParentArcRefID == parentid : 1 == 1)
                 select obj);

            return result.ToList<arc_Data>();

        }
        public List<arc_Data> FillArcDocs(int RefDocID)
        {


            var result =
                (from obj in DC.arc_Data
                 orderby obj.SentDate descending
                 where obj.TargetModule == (int)ArcTargetModules.LegalMemo && obj.RefDocID == RefDocID

                 select obj);

            return result.ToList<arc_Data>();

        }

        public int getchildDocs(int parentid)
        {


            var result =
                (from obj in DC.arc_Data
                 where obj.TargetModule == (int)ArcTargetModules.LegalMemo && obj.ParentArcRefID == parentid

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

    }
}
