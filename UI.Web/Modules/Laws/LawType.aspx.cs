using Infrastructure;
using Infrastructure.DAL;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web.Util;
using UI.Web.Admin.Controller;

namespace UI.Web.Modules.Laws
{
    public partial class LawType : BaseFormAdmin
    {
        private class LawTypeItem
        {
            public int Code { get; set; }
            public string NameAr { get; set; }
            public long RowIndex { get; set; }
        }
        public class SortItem
        {
            public int TypeCode { get; set; }        // نفس Code بتاع العنصر
            public int SortOrder { get; set; }   // الترتيب الجديد
        }
        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();
        public LawsRepository objRepository = IoC.Resolve<LawsRepository>();
        public CommitteeRepository objRepositoryComm = IoC.Resolve<CommitteeRepository>();
        public LibraryDocsRepository objRepositoryLib = IoC.Resolve<LibraryDocsRepository>();
        public LegalMemoRepository objRepositoryMemo= IoC.Resolve<LegalMemoRepository>();



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //hdnSectorId.Value = gets(Request.QueryString["sectorId"]);
                BindLawTypes();
            }
        }

        private void BindLawTypes()
        {
            var items = objLookup.DC.Database.SqlQuery<LawTypeItem>(
                "SELECT Code, NameAr, ROW_NUMBER() OVER (ORDER BY NameAr) AS RowIndex FROM Law_DocType").ToList();

            // ✅ إضافة عنصر ثابت
            items.Add(new LawTypeItem
            {
                Code = -2, // أي قيمة مميزة (مهم عشان تفرقها)
                NameAr = "مجالس إدارات الهيئات والمؤسسات العامةواللجان والمجالس العليا",
                RowIndex = items.Count + 1
            });
            //items.Add(new LawTypeItem
            //{
            //    Code = -2, // أي قيمة مميزة (مهم عشان تفرقها)
            //    NameAr = "اللجان والمجالس العليا",
            //    RowIndex = items.Count + 1
            //});
            items.Add(new LawTypeItem
            {
                Code = -1, // أي قيمة مميزة (مهم عشان تفرقها)
                NameAr = "التشكيلات الوزارية",
                RowIndex = items.Count + 1
            });
            //items.Add(new LawTypeItem
            //{
            //    Code = -3,
            //    NameAr = "كتب ومذكرات الرأي القانوني",
            //    RowIndex = items.Count + 1
            //});
            var sortList = objLookup.DC.Database
                .SqlQuery<SortItem>("SELECT TypeCode, SortOrder FROM LawTypeSort")
                .ToList();

           
            items = items
           .Where(x => x.Code != 1 && x.Code != 2)
           .ToList();

            var mergedRow = new LawTypeItem
            {
                Code = 1, // كود جديد
                NameAr = "مرسوم بقانون و قانون"
                // انسخ أي خصائص أخرى تحتاجها
            };

            items.Insert(0, mergedRow); // أو Add حسب المكان المطلوب

            items = items
               .Select(x => new
               {
                   Item = x,
                   SortOrder = sortList
                       .FirstOrDefault(s => s.TypeCode == x.Code)?.SortOrder ?? 999
               })
               .OrderBy(x => x.SortOrder)
               .ThenBy(x => x.Item.NameAr)
               .Select(x => x.Item)
               .ToList();


            var type48 = ZeroIntergerIFNull("48");
            var type7 = ZeroIntergerIFNull("7");
            var canViewPrivate = getBool(ReadSession("ViewPrivate"));

            var lawTypeCounts = objRepository.DC.View_LawsDocs
                .GroupBy(x => x.DocTypeID)
                .Select(g => new { Code = g.Key, Count = g.Count() })
                .ToDictionary(x => x.Code, x => x.Count);

            var decisionCounts = objRepository.DC.View_LawsDocs
                .Where(x => (x.DocTypeID == 5 || x.DocTypeID == 6) /*&& x.isPrivate == false*/)
                .GroupBy(x => x.DocTypeID)
                .Select(g => new { Code = g.Key, Count = g.Count() })
                .ToDictionary(x => x.Code, x => x.Count);

            //var committeeCounttype1= objRepositoryComm.DC.viewCommitteeData.ToList ().Count;
            //var committeeCounttype2 = objRepositoryComm.DC.viewCommitteeData.Where(c => c.committeeTypeID == 2).ToList().Count;

            var committeeCount = objRepositoryComm.DC.viewCommitteeData.Count();

            // كتب ومذكرات الرأي القانوني
            var libraryDocsQuery = objRepositoryLib.DC.view_LibraryDocs
                .Where(x => x.DocType == type48 && x.CatId == type7);
            if (!canViewPrivate)
            {
                libraryDocsQuery = libraryDocsQuery.Where(x => x.isPrivate == false);
            }
            var libraryDocsCount = libraryDocsQuery.Count();

            var legalMemoCount = objRepositoryMemo.DC.View_LegalMemosDocs.Count();

            Dictionary<int, int> lawCounts = new Dictionary<int, int>(items.Count);

            foreach (var item in items)
            {
                switch (item.Code)
                {
                    case -1:
                        lawCounts[item.Code] = libraryDocsCount;// legalMemoCount;
                       
                        break;
                    case -2:
                        lawCounts[item.Code] = committeeCount;// committeeCounttype1;// committeeCount;
                       // lawCounts[item.Code] = libraryDocsCount;
                        break;
                    case -3:
                        //lawCounts[item.Code] = committeeCount;// committeeCounttype2;// committeeCount;
                        //lawCounts[item.Code] = legalMemoCount;
                        break;
                    case 5:
                        lawCounts[item.Code] = decisionCounts.ContainsKey(item.Code) ? decisionCounts[item.Code] : 0;
                        break;
                    case 6:
                        lawCounts[item.Code] = decisionCounts.ContainsKey(item.Code) ? decisionCounts[item.Code] : 0;
                        break;
                    case 9:
                        lawCounts[item.Code] = 1;
                        break;
                    default:
                        if (item.Code == 1)
                        {
                            lawCounts[item.Code] = lawTypeCounts.ContainsKey(item.Code) ? lawTypeCounts[item.Code] + lawTypeCounts[2] : 0;
                        }
                        else
                        {
                            lawCounts[item.Code] = lawTypeCounts.ContainsKey(item.Code) ? lawTypeCounts[item.Code] : 0;
                        }
                        break;
                }
            }

            // نخزنها
            ViewState["LawCounts"] = lawCounts;
            rptLawTypes.DataSource = items;
            rptLawTypes.DataBind();
        }
        public int GetLawCount(object codeObj)
        {
            var dict = ViewState["LawCounts"] as Dictionary<int, int>;
            if (dict == null) return 0;

            int code = Convert.ToInt32(codeObj);

           
            return dict.ContainsKey(code) ? dict[code] : 0;
        }
        public string GetBackgroundImage(object rowIndex)
        {
            var index = 0L;
            long.TryParse(gets(rowIndex), out index);
            var fileName = index % 2 == 0 ? "RectangleV2.png" : "RectangleV1.png";
            return "/Layout/uploads/MedalFiles/" + fileName;
        }
    }
}
