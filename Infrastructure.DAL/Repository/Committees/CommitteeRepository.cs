using Infrastructure.DAL.Model;
using Infrastructure.DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Infrastructure.DAL
{
    public partial class CommitteeRepository : BaseRepository
    {

        public CommitteeRepository(CMGS_DBEntities _context) : base(_context)
        {

        }

        #region "Committees master Data"

        #region List

        public List<viewCommitteeData> GetList(int SerialNum, int SerialYear,
          DateTime TransactionDatFrom, DateTime TransactionDatTo, string CommitteesSubject, string CommitteesDetails, string CommitteesDocSerial,
          DateTime ExpireDatFrom, DateTime ExpireDatTo, int MinisterRefId, int lastProcedureId,int CommiteeRefCode, string SearchKeys)
        {
            var result =
                (from obj in DC.viewCommitteeData
                     //orderby obj.Q_Serial descending    chnaged by Nada Request ib 12052018
                 orderby obj.committeeYear descending, obj.committeeNum descending //obj.DocDate
                 where 1 == 1
             && (CommitteesDocSerial != "" ? obj.committeeSerial == CommitteesDocSerial : 1 == 1)
             && (SerialNum != 0 ? obj.committeeNum == SerialNum : 1 == 1)
             && (SerialYear != 0 ? obj.committeeYear == SerialYear : 1 == 1)
             && (MinisterRefId != 0 ? obj.MinisterRefId == MinisterRefId : 1 == 1)
             && (lastProcedureId != 0 ? obj.LastProcedureID == lastProcedureId : 1 == 1)
             && (CommiteeRefCode != 0 ? obj.committeeRefCode == CommiteeRefCode : 1 == 1)
             && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.lastJoinDate >= TransactionDatFrom : 1 == 1) || (TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.lastJoinDate >= TransactionDatFrom : 1 == 1))
             && ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.lastJoinDate <= TransactionDatTo : 1 == 1) || (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.lastJoinDate <= TransactionDatTo : 1 == 1))
             && ((ExpireDatFrom != new DateTime(1990, 01, 01) ? obj.JoinExpireDate >= ExpireDatFrom : 1 == 1) || (ExpireDatFrom != new DateTime(1990, 01, 01) ? obj.JoinExpireDate >= ExpireDatFrom : 1 == 1))
             && ((ExpireDatTo != new DateTime(1990, 01, 01) ? obj.JoinExpireDate <= ExpireDatTo : 1 == 1) || (ExpireDatTo != new DateTime(1990, 01, 01) ? obj.JoinExpireDate <= ExpireDatTo : 1 == 1))
             && (CommitteesSubject != "" ? obj.LegalDocsDesc.Contains(CommitteesSubject) || obj.Notes.Contains(CommitteesSubject) ||  obj.committeeTitle.Contains(CommitteesSubject) : 1 == 1)
             && (CommitteesDetails != "" ? obj.LegalDocsDesc.Contains(CommitteesDetails) || obj.Notes.Contains(CommitteesDetails) || obj.committeeTitle.Contains(CommitteesDetails) : 1 == 1)

                 select obj);



            var _out = result.ToList<viewCommitteeData>();
            PostResultToAudit((int)SysModulesRef.Legislation, "CommitteeList", "Search Result /GetList", SearchKeys, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;


        }


        public Committees_Data GetDetails(int _Code)
        {
            var result =
                (from obj in DC.Committees_Data
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<Committees_Data>();
        }

        public Committees_Data GetDetailsWithProcedures(int _Code)
        {
            var result =
                (from obj in DC.Committees_Data
                 .Include("Committees_DocProcedures")
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<Committees_Data>();
        }

        public Committees_Data GetDetailsForEdit(int _Code)
        {
            var result =
                (from obj in DC.Committees_Data
                 where obj.Code == _Code
                 select obj);
            return result.FirstOrDefault<Committees_Data>();

        }

        public Committees_DocProcedures GetLastProcedure(int _DocCode)
        {


            var result =
                (from obj in DC.Committees_DocProcedures
                 where obj.DocRefID == _DocCode
                 orderby obj.ProcedureDate descending
                 select obj).FirstOrDefault<Committees_DocProcedures>();


            return result;



        }


        public bool CheckDocExitance(int DocNum,  int fileRefID)
        {


            var result =
                (from obj in DC.Committees_Data
                 where obj.committeeNum == DocNum   && obj.Code != fileRefID
                 select obj).FirstOrDefault<Committees_Data>();
            if (result != null)
            {
                return true;
            }


            return false;
        }


        #endregion

        #region "Add ,  Update  ,Delete" Committees Operation

        public int AddCommittees<T>(T item)
        {


            DC.Committees_Data.Add(item as Committees_Data);
            return DC.SaveChanges();

        }

        public int DeleteCommittees<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Committees_Data).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateCommittees<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Committees_Data).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region "Committees Child"

        //#region"Attachemnt"


        //public Committees_Attachments GetAttachemtnDetails(int _Code)
        //{


        //    var result =
        //        (from obj in DC.Committees_Attachments
        //         where obj.Code == _Code
        //         select obj);

        //    return result.FirstOrDefault<Committees_Attachments>();

        //}

        //public List<Committees_Attachments> FillCommitteesAttachemnt(int RefTypeID, int RefID)
        //{


        //    var result =
        //        (from obj in DC.Committees_Attachments
        //         .Include("Committees_AttachmentTypes")
        //         where obj.RefTypeID == RefTypeID && obj.RefID == RefID
        //         select obj);
        //    return result.ToList<Committees_Attachments>();

        //}



        //#region "Add ,  Update  ,Delete" Medal proceduee

        //public int AddAttachement<T>(T item)
        //{


        //    DC.Committees_Attachments.Add(item as Committees_Attachments);
        //    return DC.SaveChanges();

        //}

        //public int DeleteAttachment<T>(T item)
        //{


        //    //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
        //    DC.Entry(item as Committees_Attachments).State = System.Data.Entity.EntityState.Deleted;
        //    return DC.SaveChanges();

        //}

        //public int UpdateAttachment<T>(T item)
        //{


        //    // Mark entity as modified
        //    DC.Entry(item as Committees_Attachments).State = System.Data.Entity.EntityState.Modified;
        //    return DC.SaveChanges();

        //}


        //#endregion

        //#endregion


        //#region"Attachemnt Main"


        //public Medalmain_Attachments GetAttachemtnMainDetails(int _Code)
        //{


        //    var result =
        //        (from obj in DC.Medalmain_Attachments
        //         where obj.Code == _Code
        //         select obj);

        //    return result.FirstOrDefault<Medalmain_Attachments>();

        //}

        //public List<Medalmain_Attachments> FillMedalMainAttachemnt(int _Code)
        //{


        //    var result =
        //        (from obj in DC.Medalmain_Attachments
        //         .Include("Medal_M_AttachmentTypes")
        //         where obj.MedalMasterCode == _Code
        //         select obj);

        //    return result.ToList<Medalmain_Attachments>();

        //}

        //#region "Add ,  Update  ,Delete" Medal proceduee

        //public int AddAttachementmain<T>(T item)
        //{


        //    DC.Medalmain_Attachments.Add(item as Medalmain_Attachments);
        //    return DC.SaveChanges();

        //}

        //public int DeleteAttacjmentmain<T>(T item)
        //{


        //    //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
        //    DC.Entry(item as Medalmain_Attachments).State = System.Data.Entity.EntityState.Deleted;
        //    return DC.SaveChanges();

        //}

        //public int UpdateAttachmentMain<T>(T item)
        //{


        //    // Mark entity as modified
        //    DC.Entry(item as Medalmain_Attachments).State = System.Data.Entity.EntityState.Modified;
        //    return DC.SaveChanges();

        //}


        //#endregion

        //#endregion

        #region"Procedures"
        public Committees_DocProcedures GetProceduresDetails(int? _Code)
        {


            var result =
                (from obj in DC.Committees_DocProcedures
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Committees_DocProcedures>();

        }
        public List<Committees_DocProcedures> FillCommitteesProcedures(int RefDocID)
        {


            var result =
                (from obj in DC.Committees_DocProcedures
                 .Include("Committees_ProceduresTypes")
                 orderby obj.Committees_ProceduresTypes.D_Order.Value , obj.ProcedureDate.Value ascending
                 where obj.DocRefID == RefDocID
                 select obj);

            return result.ToList<Committees_DocProcedures>();

        }



        #region "Add ,  Update  ,Delete" Procedures proceduee

        public int AddProcedures<T>(T item)
        {


            DC.Committees_DocProcedures.Add(item as Committees_DocProcedures);
            return DC.SaveChanges();

        }

        public int DeleteProcedures<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Committees_DocProcedures).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateProcedures<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Committees_DocProcedures).State = System.Data.Entity.EntityState.Modified;
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





    }
}
