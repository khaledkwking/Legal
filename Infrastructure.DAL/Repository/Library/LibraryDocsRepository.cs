using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainInterface;
using Infrastructure.DAL.Enum;
using Infrastructure.DAL.Model;
using Newtonsoft.Json;

namespace Infrastructure.DAL
{
    public partial class LibraryDocsRepository : BaseRepository
    {

        public LibraryDocsRepository(CMGS_DBEntities _context) :base(_context)
        {

        }

        #region "Library master Data"

        #region List
        public List<view_LibraryDocs> GetList(string Libraryerial, string DocSubject, DateTime TransactionDatFrom,
            DateTime TransactionDatTo, int DocType
            ,Boolean isPrivate, int DocCatID, int chapterId, int sessionId,string SearchKeys)
        {


                var result =
                    (from obj in DC.view_LibraryDocs
                     orderby obj.ReceiveDate descending
                     where 1 == 1
                   && (Libraryerial != "" ? obj.DocRef.Contains(Libraryerial) : 1 == 1)
                   && (TransactionDatFrom != new DateTime(1990, 01, 01) ? obj.UploadDate >= TransactionDatFrom : 1 == 1)
                   && (TransactionDatTo != new DateTime(1990, 01, 01) ? obj.UploadDate <= TransactionDatFrom : 1 == 1)
                   && (DocType != 0 ? obj.DocType == DocType : 1 == 1)
                    && (DocCatID != 0 ? obj.CatId == DocCatID : 1 == 1)

                    && (chapterId != 0 ? obj.ChapterID == chapterId : 1 == 1)
                    && (sessionId != 0 ? obj.SessionID == sessionId : 1 == 1)

                    && (DocSubject != "" ? obj.DocSubject.Contains(DocSubject) : 1 == 1)
                   && (isPrivate != true ? obj.isPrivate == false : 1 == 1)
                     select obj);




            var _out = result.ToList<view_LibraryDocs>();
            PostResultToAudit((int)SysModulesRef.libarary, nameof(SysModulesRef.libarary), "Search Result /GetList", SearchKeys, JsonConvert.SerializeObject(_out), _out.Count);
            return _out;

        }

         public view_LibraryDocs FillDetails(int _Code)
        {


                var result =
                    (from obj in DC.view_LibraryDocs
                   //  .Include("Library_DocsType")
                     where obj.Code == _Code
                     select obj);

                return result.FirstOrDefault<view_LibraryDocs>();
           // }
        }
        public Library_Documents GetDetails(int _Code)
        {


                var result =
                    (from obj in DC.Library_Documents
                     where obj.Code == _Code
                     select obj);

                return result.FirstOrDefault<Library_Documents>();

        }
        #endregion

        #region "Add ,  Update  ,Delete" LibraryDocs

        public int AddLibraryDocs<T>(T item)
        {


             DC.Library_Documents.Add(item as Library_Documents);
                return DC.SaveChanges();

        }

        public int DeleteLibraryDocs<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
             DC.Entry(item as Library_Documents).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateLibraryDocs<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Library_Documents).State = System.Data.Entity.EntityState.Modified;

                return DC.SaveChanges();

        }


        #endregion
        #endregion


        #region "Doc Types"
        public List<Library_DocsType> GetTypeList(int CatID)
        {


                var result =
                    (from obj in DC.Library_DocsType
                     .Include("Library_DocsCategory")
                     orderby obj.CatId descending
                     where 1 == 1
                 && (CatID != 0 ? obj.CatId== CatID : 1 == 1)

                     select obj);

                return result.ToList<Library_DocsType>();

        }


        public Library_DocsType GetTypeDetails(int _Code)
        {


                var result =
                    (from obj in DC.Library_DocsType
                     where obj.Code == _Code
                     select obj);

                return result.FirstOrDefault<Library_DocsType>();

        }

        #region "Add ,  Update  ,Delete" LibraryDocs  Type

        public int AddLibraryDocType<T>(T item)
        {


                DC.Library_DocsType.Add(item as Library_DocsType);
                return DC.SaveChanges();

        }

        public int DeleteLibraryType<T>(T item)
        {


                //var item = DC.News.Where(N => N.newsId == id).FirstOrDefault();
                DC.Entry(item as Library_DocsType).State = System.Data.Entity.EntityState.Deleted;
                return DC.SaveChanges();

        }

        public int UpdateLibrarytype<T>(T item)
        {


                // Mark entity as modified
                DC.Entry(item as Library_DocsType).State = System.Data.Entity.EntityState.Modified;
                return DC.SaveChanges();

        }


        #endregion
        #endregion
    }
}
