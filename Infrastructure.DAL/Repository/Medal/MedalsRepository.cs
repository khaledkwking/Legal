using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainInterface;
using Infrastructure.DAL.Enum;
using Infrastructure.DAL.Model;
using Infrastructure.DAL.ViewModels;
using Newtonsoft.Json;

namespace Infrastructure.DAL
{
    public partial class MedalsRepository : BaseRepository
    {
        public MedalsRepository(CMGS_DBEntities _context):base(_context)
        {
        }
        #region "Medals master Data"
        #region "Validationn"

        public bool CheckExistance(int SerialNUm, int serialYear, int fileRefID)
        {



                var result =
                    (from obj in DC.Medal_Data
                     where obj.FileSerial == SerialNUm && obj.FileYear == serialYear && obj.Code != fileRefID
                     select obj).FirstOrDefault<Medal_Data>();
                if (result != null)
                {
                    return true;
                }


            return false;
        }

        #endregion

        #region List
        public List<MedalViewModel> GetList(string Medalserial, DateTime TransactionDatFrom,
            DateTime TransactionDatTo, int MedalType, int Agr_CatID, int orgCodeID,
            int LastActionID,string FilterName, int JObGradeID,Boolean isPrivate, string SelectedFilterKeys,int MedalCatId )
        {


                var result =
                    (from obj in DC.Medal_Data

                     .Include("Medal_M_Organizations")
                     .Include("Medal_Persons.Medal_M_Types")
                       .Include("Medal_Persons")
                         //.Include("Medal_Persons.Medal_M_Types")
                         //    orderby obj.FileSerial descending, obj.FileYear descending
                     where 1 == 1// Stop Getting Data As per Osama request on 13112019
                   && (Medalserial != "" ? obj.FileNum == Medalserial : 1 == 1)
                   && (TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.TransDate >= TransactionDatFrom : 1 == 1)
                   && (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.TransDate <= TransactionDatFrom : 1 == 1)
                   && (MedalType != 0 ? obj.Medal_Persons.Any(a=>a.Medal_M_Types.Code == MedalType) : 1 == 1)
                   && (orgCodeID != 0 ? obj.Medal_OrgID == orgCodeID : 1 == 1)
                   // && (MedalType != 0 ? obj.Medal_Persons.FirstOrDefault().Medal_M_Types.Code == MedalType : 1 == 1)
                   && (LastActionID != 0 ? obj.LastActionID == LastActionID : 1 == 1)
                      && (MedalCatId != 0 ? obj.MedalCatId== MedalCatId: 1 == 1)
                   && (FilterName != "" ? obj.Medal_Persons.Any(sub => sub.Person_NameAr.Contains(FilterName)) : 1 == 1)
                   && (JObGradeID != 0 ? obj.Medal_Persons.Any(sub => sub.GradeID == JObGradeID) : 1 == 1)
                    && (isPrivate != true ? obj.isPrivate == false : 1 == 1)
                     //&& (ManifestNo != "" ? obj.ManifestNo == ManifestNo : 1 == 1)
                     //&& (DekiveryOrderNo != "" ? obj.DeliveryOrderNo == DekiveryOrderNo : 1 == 1)
                     //&& (Decalrationtype != 0 ? obj.DepositeDeclarationTypeCode == Decalrationtype : 1 == 1)
                     select obj

                     ).OrderByDescending(x => x.FileYear).ThenByDescending(x => x.FileSerial).Select(m=> new MedalViewModel {
                         Code=m.Code,
                         Medal_OrgID=m.Medal_OrgID,
                         LastActionID=m.LastActionID,
                         FileNum=m.FileNum,
                         MedalCatId = m.MedalCatId,
                         OrganizationsNameAr =m.Medal_M_Organizations.NameAr,
                         Medal_receivedDate=m.Medal_receivedDate.Value,
                         TransDate=m.TransDate.Value

                     });


            var _out = result.ToList<MedalViewModel>();
             //PostResultToAudit((int)SysModulesRef.medal, nameof(SysModulesRef.medal), "Search Result /GetList",
             // SelectedFilterKeys, JsonConvert.SerializeObject(_out, _settings), _out.Count);
            PostResultToAudit((int)SysModulesRef.medal, nameof(SysModulesRef.medal), "Search Result /GetList",
            SelectedFilterKeys, JsonConvert.SerializeObject(_out), _out.Count);

            return _out;

        }
        public Medal_Data FillDetails(int _Code)
        {


                var result =
                    (from obj in DC.Medal_Data
                     where obj.Code == _Code
                     select obj);

                return result.FirstOrDefault<Medal_Data>();

        }



        public Medal_Data GetDetails(int _Code)
        {


                var result =
                    (from obj in DC.Medal_Data
                     where obj.Code == _Code
                     select obj);

                return result.FirstOrDefault<Medal_Data>();

        }

        public List<Medal_ProcedureHistory> FillMedalProcedures(int _Code)
        {


                var result =
                    (from obj in DC.Medal_ProcedureHistory
                     .Include("Medal_M_ProcedureType")
                     orderby obj.ProcedureDate descending
                     where obj.MedalMasterID == _Code
                     select obj);

                return result.ToList<Medal_ProcedureHistory>();

        }


        public List<View_MedalData> FillMedalMater(int _Code)
        {


                var result =
                    (from obj in DC.View_MedalData
                     where obj.Code == _Code
                     select obj);

                return result.ToList<View_MedalData>();

        }

        #endregion

        #region "Add ,  Update  ,Delete" Medal Operation

        public int AddMedal<T>(T item)
        {


                DC.Medal_Data.Add(item as Medal_Data);
                return DC.SaveChanges();

        }




        public int DeleteMedal<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as Medal_Data).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateMedal<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Medal_Data).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();

        }


        #endregion


        #endregion


        #region "Medal Child"
        #region"Persons"
  public List<Medal_Persons> FillPersons(int _Code)
        {


                var result =
                    (from obj in DC.Medal_Persons
                     .Include("Medal_M_Types")
                     //.Include("medal_M_Grantreasons")
                     .Include("Medal_M_jobGrade")
                     //orderby obj.Code
                     where obj.MedalmasterID == _Code
                     select obj);

                return result.ToList<Medal_Persons>();

        }

        public Medal_Persons getpersonDetails(int _Code)
        {
            var result =
                (from obj in DC.Medal_Persons
                 where obj.Code == _Code
                 select obj);

            return result.FirstOrDefault<Medal_Persons>();

        }

        #region "Add ,  Update  ,Delete" Medal Persons

        public int AddPerson<T>(T item)
        {


                DC.Medal_Persons.Add(item as Medal_Persons);
                return DC.SaveChanges();

        }

        public int DeletePersons<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as Medal_Persons).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdatePersons<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Medal_Persons).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();

        }


        #endregion
        #endregion

        #region"Procedure"
        public Medal_ProcedureHistory GetprOCEDUREDetails(int _Code)
        {


                var result =
                    (from obj in DC.Medal_ProcedureHistory
                     where obj.Code == _Code
                     select obj);

                return result.FirstOrDefault<Medal_ProcedureHistory>();

        }

        #region "Add ,  Update  ,Delete" Medal proceduee

        public int AddProcedure<T>(T item)
        {


                DC.Medal_ProcedureHistory.Add(item as Medal_ProcedureHistory);
                return DC.SaveChanges();

        }

        public int DeleteProceduret<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as Medal_ProcedureHistory).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateProcedure<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Medal_ProcedureHistory).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();

        }


        #endregion

        #endregion


        #region"Attachemnt"


        public Medal_Attachments GetAttachemtnDetails(int _Code)
        {


                var result =
                    (from obj in DC.Medal_Attachments
                     where obj.Code == _Code
                     select obj);

                return result.FirstOrDefault<Medal_Attachments>();

        }

        public List<Medal_Attachments> FillMedalAttachemnt(int _Code)
        {


                var result =
                    (from obj in DC.Medal_Attachments
                     .Include("Medal_M_AttachmentTypes")
                     where obj.ProcedureID == _Code
                     select obj);

                return result.ToList<Medal_Attachments>();

        }

        #region "Add ,  Update  ,Delete" Medal proceduee

        public int AddAttachement<T>(T item)
        {


                DC.Medal_Attachments.Add(item as Medal_Attachments);
                return DC.SaveChanges();

        }

        public int DeleteAttacjment<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as Medal_Attachments).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateAttachment<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Medal_Attachments).State = System.Data.Entity.EntityState.Modified;
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

        #endregion
    }
}
