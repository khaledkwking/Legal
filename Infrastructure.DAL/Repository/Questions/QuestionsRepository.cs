using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainInterface;
using Infrastructure.DAL.Model;
using Infrastructure.DAL.Enum;
using Newtonsoft.Json;

namespace Infrastructure.DAL
{
    public partial class QuestionsRepository : BaseRepository
    {

        public QuestionsRepository(CMGS_DBEntities _context) : base(_context)
        {

        }


        #region "Question master Data"

        #region List
        public List<View_QuestionsList> GetList(int QType, int Q_Serial, string OmaSerial, int omaYear, DateTime TransactionDatFrom, DateTime TransactionDatTo,
            int ChapterID, int sessionID, int StatusID, List<int> selectedRequestedFrom,
            int RequestTo, string QuestionSubject, int RelatedOrgs, int assignedPerson, int ResultId, DateTime DiscussionDateFrom, DateTime DiscussionDateTo,Boolean IsGrouped, string searchkeys)
        {


            var result =
                (from obj in DC.View_QuestionsList
                     //orderby obj.Q_Serial descending    chnaged by Nada Request ib 12052018
                     orderby   obj.Q_Date descending
                 where obj.QType == QType
                 && (Q_Serial != 0 ? obj.Q_Serial == Q_Serial : 1 == 1)
                 && (OmaSerial != "" ? obj.Oma_Serial == OmaSerial : 1 == 1)
                 && (omaYear != 0 ? obj.OmaYear == omaYear : 1 == 1)
                 && (IsGrouped ==true ? obj.Is_grouped == true : 1 == 1)

                 && ((TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Q_Date >= TransactionDatFrom : 1 == 1) || (TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.Q_Date >= TransactionDatFrom : 1 == 1))
                 && ((TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Q_Date <= TransactionDatTo : 1 == 1) || (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.Q_Date <= TransactionDatTo : 1 == 1))


                  && ((DiscussionDateFrom != new DateTime(1990, 01, 01) ? obj.Q_DiscussionDate >= DiscussionDateFrom : 1 == 1) || (DiscussionDateFrom != new DateTime(1990, 01, 01) ? obj.Q_DiscussionDate >= DiscussionDateFrom : 1 == 1))
                 && ((DiscussionDateTo != new DateTime(1990, 01, 01) ? obj.Q_DiscussionDate <= DiscussionDateTo : 1 == 1) || (DiscussionDateTo != new DateTime(1990, 01, 01) ? obj.Q_DiscussionDate <= DiscussionDateTo : 1 == 1))


                 && (ChapterID != 0 ? obj.ChapterID == ChapterID : 1 == 1)
                 && (sessionID != 0 ? obj.SessionID == sessionID : 1 == 1)
                 && (assignedPerson != 0 ? obj.AssignedPersonID == assignedPerson : 1 == 1)
                 && (StatusID != 0 ? obj.StatusID == StatusID : 1 == 1)
                 && (RequestTo != 0 ? obj.Q_RequestTo == RequestTo : 1 == 1)
                 && (ResultId != 0 ? obj.Q_Result == ResultId : 1 == 1)

                 && (selectedRequestedFrom.Count > 0 ? selectedRequestedFrom.Contains(obj.Q_RequestFrom.Value) : 1 == 1)
                 && (QuestionSubject != "" ? obj.Q_Text.Contains(QuestionSubject) : 1 == 1)


                  //&& (RelatedOrgs.Count > 0 ? RelatedOrgs.Contains(obj.Q_RequestTo.Value) : 1 == 1)
                  && (RelatedOrgs != 0 ? DC.Parliament_Requestedby
                                           .Where(x => x.PersonID == RelatedOrgs)
                                           .Select(x => x.QuestionID).Contains(obj.code)
                                           : 1 == 1)

                     //    //  && (JObGradeID != 0 ? obj.Parliament_Requestedby.Any(sub => sub.GradeID == JObGradeID) : 1 == 1)
                     //    && (isPrivate != true ? obj.isPrivate == false : 1 == 1)

                     select obj);



            var _out = result.ToList<View_QuestionsList>();
            PostResultToAudit((int)SysModulesRef.questions, nameof(SysModulesRef.questions), "Search Result /GetList", searchkeys, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;


        }


        public List<View_QuestionsStatistics> FillStatistics(
          int ChapterID, int sessionID,int RequestTo)
        {


            var result =
                (from obj in DC.View_QuestionsStatistics

                 where obj.QType == (int)Questions_TypesEnum.Question

                 && (ChapterID != 0 ? obj.ChapterID == ChapterID : 1 == 1)
                 && (sessionID != 0 ? obj.SessionID == sessionID : 1 == 1)
                  && (RequestTo != 0 ? obj.Q_RequestTo == RequestTo : 1 == 1)
                 orderby obj.Qcount descending 

                 select obj);

            return result.ToList<View_QuestionsStatistics>();

        }

        public List<view_QuestionsFollow> ShowQuestionsAlerts()
        {

            var result =
                (from obj in DC.view_QuestionsFollow
                 select obj);

            return result.ToList<view_QuestionsFollow>();

        }

        public List<sp_ArcNotifications_Result> ShowArcAlert(int ModuleCode)
        {

            var result =
                (from obj in DC.sp_ArcNotifications(ModuleCode)
                 select obj);

            return result.ToList<sp_ArcNotifications_Result>();

        }
        public Parliament_Questions GetDetails(int _Code)
        {


            var result =
                (from obj in DC.Parliament_Questions
                 .Include("Parliament_legislativeChapter")
                 .Include("Parliament_legislativeSession")
                 .Include("Parliament_QuestionStatus")
                  .Include("Parliament_RequestedToPerson")
                  .Include("Parliament_AssignedPerson")

                 where obj.code == _Code
                 select obj);
            return result.FirstOrDefault<Parliament_Questions>();

        }

        public View_QuestionsList GetViewDetails(int _Code)
        {


            var result =
                (from obj in DC.View_QuestionsList

                 where obj.code == _Code
                 select obj);
            return result.FirstOrDefault<View_QuestionsList>();

        }

        public bool CheckQuestionExistance(string Oma_Serial, int ChapterID, int fileRefID, int QType)
        {



            var result =
                (from obj in DC.Parliament_Questions
                 where obj.Oma_Serial == Oma_Serial && obj.ChapterID == ChapterID && obj.code != fileRefID && obj.QType == QType
                 select obj).FirstOrDefault<Parliament_Questions>();
            if (result != null)
            {
                return true;
            }


            return false;
        }



        public Parliament_Questions GetDetailsForEdit(int _Code)
        {

            var result =
                (from obj in DC.Parliament_Questions
                 where obj.code == _Code
                 select obj);
            return result.FirstOrDefault<Parliament_Questions>();

        }

        #endregion

        #region "Add ,  Update  ,Delete" Question Operation

        public int AddQuestion<T>(T item)
        {

            DC.Parliament_Questions.Add(item as Parliament_Questions);
            return DC.SaveChanges();

        }

        public int DeleteQuestion<T>(T item)
        {

            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Parliament_Questions).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateQuestion<T>(T item)
        {

            // Mark entity as modified
            DC.Entry(item as Parliament_Questions).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion

        #region "Question Child"
        #region"Persons"

        public bool validateQuestionMadbata(int QuestionId)
        {


            var result =
                (from obj in DC.Parliament_Questions_Madbata
                 where obj.QuestionRefId == QuestionId
                 select obj).ToList<Parliament_Questions_Madbata>();

            if (result != null && result.Count > 0)
            {
                return true;
            }
            return false;

        }

        public int GetPersonIDbyName(string personName)
        {


            var result =
                (from obj in DC.Parliament_Persons

                 where obj.NameEn == personName
                 select obj).FirstOrDefault<Parliament_Persons>();

            if (result != null)
            {
                return result.Code;
            }
            return 0;

        }

        public List<Parliament_Requestedby> FillPersons(int _Code, int Type)
        {


            var result =
                (from obj in DC.Parliament_Requestedby

                 where obj.QuestionID == _Code && obj.PartyType == Type
                 select obj);

            return result.ToList<Parliament_Requestedby>();

        }


        public List<Parliament_RelatedOrgs> FillOrgs(int _Code)
        {


            var result =
                (from obj in DC.Parliament_RelatedOrgs

                 where obj.QuestionID == _Code
                 select obj);

            return result.ToList<Parliament_RelatedOrgs>();

        }

        #region "Add ,  Update  ,Delete" Medal Persons

        public int AddPerson<T>(T item)
        {


            DC.Parliament_Requestedby.Add(item as Parliament_Requestedby);
            return DC.SaveChanges();

        }

        public int DeletePersons<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Parliament_Requestedby).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdatePersons<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Parliament_Requestedby).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


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

                 where obj.TargetModule == (int)ArcTargetModules.QuestionsModules && obj.Doc_Type == Doc_Type && obj.RefDocID == RefDocID
                 && (parentid != 0 ? obj.ParentArcRefID == parentid : 1 == 1)
                 select obj);

            return result.ToList<arc_Data>();

        }


        public List<arc_Data> FillArcDocsInOut(int RefDocID, int parentid = 0)
        {


            var result =
                (from obj in DC.arc_Data

                 where obj.TargetModule == (int)ArcTargetModules.QuestionsModules && obj.RefDocID == RefDocID
                 && (parentid != 0 ? obj.ParentArcRefID == parentid : 1 == 1)
                 select obj);

            return result.ToList<arc_Data>();

        }

        public arc_Data FillLastFollow(int RefDocID)
        {


            var result =
                (from obj in DC.arc_Data
                 orderby obj.NextFollowReminderDate descending
                 where obj.TargetModule == (int)ArcTargetModules.QuestionsModules && obj.RefDocID == RefDocID
                 && (obj.NextFollowReminderDate !=null  ||   obj.NextFollowReminderDate !=default(DateTime))
                 select obj).FirstOrDefault();

            return result;

        }



        public int getchildDocs(int parentid)
        {


            var result =
                (from obj in DC.arc_Data
                 where obj.TargetModule == (int)ArcTargetModules.QuestionsModules && obj.ParentArcRefID == parentid

                 select obj).Count();

            return result;

        }
        #region "Add ,  Update  ,Delete" Medal proceduee

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


        #region"Attachemnt"


        public Question_Attachments GetAttachemtnDetails(int _Code)
        {


            var result =
                (from obj in DC.Question_Attachments
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Question_Attachments>();

        }

        public List<Question_Attachments> FillQuestionAttachemnt(int _Code)
        {


            var result =
                (from obj in DC.Question_Attachments
                 .Include("Question_AttachmentsTypes")
                 where obj.QuestionID == _Code
                 select obj);

            return result.ToList<Question_Attachments>();

        }
        public List<Question_Attachments> FillFileAttachemnt(int _FileCode)
        {


            var result =
                (from obj in DC.Question_Attachments
                 .Include("Question_AttachmentsTypes")
                 where obj.FileID == _FileCode
                 select obj);

            return result.ToList<Question_Attachments>();

        }
        public List<Question_Attachments> FillQuestionDocsAttachemnt(int QuestionID, int DocID, int fileID)
        {


            var result =
                (from obj in DC.Question_Attachments
                 .Include("Question_AttachmentsTypes")
                 where 1 == 1
           && (DocID != 0 ? obj.DocCode == DocID : 1 == 1)
           && (QuestionID != 0 ? obj.QuestionID == QuestionID : 1 == 1)
           && (fileID != 0 ? obj.FileID == fileID : 1 == 1)
                 select obj);

            return result.ToList<Question_Attachments>();

        }

        #region "Add ,  Update  ,Delete" Medal proceduee

        public int AddAttachement<T>(T item)
        {


            DC.Question_Attachments.Add(item as Question_Attachments);
            return DC.SaveChanges();

        }

        public int DeleteAttachment<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Question_Attachments).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateAttachment<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Question_Attachments).State = System.Data.Entity.EntityState.Modified;
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

        #region"Answers"
        public Parliament_QuestionsAnswers GetAnswersDetails(int? _Code)
        {


            var result =
                (from obj in DC.Parliament_QuestionsAnswers
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Parliament_QuestionsAnswers>();

        }
        public List<Parliament_QuestionsAnswers> FillQuestionAnswers(int RefDocID)
        {


            var result =
                (from obj in DC.Parliament_QuestionsAnswers

                 where obj.QuestionID == RefDocID
                 select obj);

            return result.ToList<Parliament_QuestionsAnswers>();

        }



        #region "Add ,  Update  ,Delete" Answers proceduee

        public int AddAnswers<T>(T item)
        {


            DC.Parliament_QuestionsAnswers.Add(item as Parliament_QuestionsAnswers);
            return DC.SaveChanges();

        }

        public int DeleteAnswers<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Parliament_QuestionsAnswers).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateAnswers<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Parliament_QuestionsAnswers).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();
            // }
        }


        #endregion

        #endregion


        #region "Oma Person"

        #region "Add ,  Update  ,Delete" Medal proceduee




        public List<Parliament_Persons> FillOmaPersons(int ChapterID)
        {


            var result =
                (from obj in DC.Parliament_Persons
                   .Include("Parliament_legislativeChapter")
                 where 1 == 1
             && (ChapterID != 0 ? obj.ChapterID == ChapterID : 1 == 1)
                 select obj);

            return result.ToList<Parliament_Persons>();
            //  }
        }

        public Parliament_Persons getPersonDetails(int PersonID)
        {


            var result =
                (from obj in DC.Parliament_Persons
                   .Include("Parliament_legislativeChapter")
                 where obj.Code == PersonID
                 select obj);

            return result.FirstOrDefault<Parliament_Persons>();
            // }
        }
        public Parliament_Persons FillPersonDetails(int PersonID)
        {


            var result =
                (from obj in DC.Parliament_Persons
                 .Include("Parliament_legislativeChapter")
                 where obj.Code == PersonID
                 select obj);

            return result.FirstOrDefault<Parliament_Persons>();
            //  }
        }

        public bool checkTextExistance(String personName, int ChapterID)
        {


            var result =
                (from obj in DC.Parliament_Persons
                 where obj.ChapterID == ChapterID && obj.NameAr == personName
                 select obj);

            if (result.ToList().Count > 0)
            {
                return true;
            }
            return false;
            // }
        }

        public bool checkTextExistanceUpdate(String personName, int ChapterID, int recordID)
        {


            var result =
                (from obj in DC.Parliament_Persons
                 where obj.Code != recordID && obj.ChapterID == ChapterID && obj.NameAr == personName
                 select obj);

            if (result.ToList().Count > 0)
            {
                return true;
            }
            return false;

        }
        public int AddOmaPerson<T>(T item)
        {


            DC.Parliament_Persons.Add(item as Parliament_Persons);
            return DC.SaveChanges();

        }

        public int DeleteOmaPerson<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Parliament_Persons).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();
            // }
        }

        public int UpdateOmaPerson<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Parliament_Persons).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }


        #endregion

        #endregion



        public List<Parliament_legislativeSession> FillChapterSession(int ChapterID)
        {


            var result =
                (from obj in DC.Parliament_legislativeSession
                   .Include("Parliament_legislativeChapter")
                 where 1 == 1
             && (ChapterID != 0 ? obj.ChapterID == ChapterID : 1 == 1)
                 orderby obj.sessionOrder ascending
                 select obj);

            return result.ToList<Parliament_legislativeSession>();

        }


        public Parliament_legislativeSession FillSessionDetails(int SessionID)
        {


            var result =
                (from obj in DC.Parliament_legislativeSession
                 .Include("Parliament_legislativeChapter")
                 where obj.Code == SessionID
                 select obj);

            return result.FirstOrDefault<Parliament_legislativeSession>();

        }

        public Parliament_legislativeSession getSessionDetails(int SessionID)
        {


            var result =
                (from obj in DC.Parliament_legislativeSession
                   .Include("Parliament_legislativeChapter")
                 where obj.Code == SessionID
                 select obj);

            return result.FirstOrDefault<Parliament_legislativeSession>();

        }

        public bool checkItemExistance(int ChapterId, string itemName, int RefId)
        {


            var result =
                (from obj in DC.Parliament_legislativeSession
                 where obj.ChapterID == ChapterId && obj.NameAr == itemName && obj.Code != RefId
                 select obj).FirstOrDefault();

            if (result != null)
            {
                return true;
            }

            return false;
        }


        public int AddSession<T>(T item)
        {


            DC.Parliament_legislativeSession.Add(item as Parliament_legislativeSession);
            return DC.SaveChanges();

        }

        public int Deletesession<T>(T item)
        {


            //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
            DC.Entry(item as Parliament_legislativeSession).State = System.Data.Entity.EntityState.Deleted;
            return DC.SaveChanges();

        }

        public int UpdateSession<T>(T item)
        {


            // Mark entity as modified
            DC.Entry(item as Parliament_legislativeSession).State = System.Data.Entity.EntityState.Modified;
            return DC.SaveChanges();

        }

        #endregion
    }
}
