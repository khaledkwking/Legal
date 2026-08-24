using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Infrastructure.DAL;
using UI.Web.Admin.Controller;

namespace UI.Web.Modules.Laws
{
    public partial class Sectors : BaseFormAdmin
    {
        private class SectorItem
        {
            public int Code { get; set; }
            public string NameAr { get; set; }
            public string imgPath { get; set; }
            public int DocCount { get; set; }
        }

        private class SectorCountItem
        {
            public int SectorId { get; set; }
            public int DocCount { get; set; }
        }

        public LooksUpsRepository objLookup = IoC.Resolve<LooksUpsRepository>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnSectorId.Value = gets(Request.QueryString["typeId"]);
                BindSectors();
            }
        }

        private void BindSectors()
        {
            var typeId = ZeroIntergerIFNull(hdnSectorId.Value);
            var sectors = objLookup.DC.Database.SqlQuery<SectorItem>(
                "SELECT Code, NameAr, imgPath FROM Sectors ORDER BY NameAr").ToList();

            var sectorCounts = objLookup.DC.Database.SqlQuery<SectorCountItem>(
                    "SELECT s.SectorId, COUNT(DISTINCT s.Law_DocId) AS DocCount FROM Law_DocSectors s INNER JOIN Law_DocData d ON d.Code = s.Law_DocId WHERE (@p0 = 0 OR d.DocTypeID = @p0) GROUP BY s.SectorId",
                    typeId).ToList();

            if (typeId==1)
            {
                 
                var sectorCountsCode2 = objLookup.DC.Database.SqlQuery<SectorCountItem>(
                    "SELECT s.SectorId, COUNT(DISTINCT s.Law_DocId) AS DocCount FROM Law_DocSectors s INNER JOIN Law_DocData d ON d.Code = s.Law_DocId WHERE (@p0 = 0 OR d.DocTypeID = @p0) GROUP BY s.SectorId",
                    Constant.Law_DocTypeQanuanMarsum).ToList();

                sectorCounts.AddRange(sectorCountsCode2);

                sectorCounts = sectorCounts
                    .GroupBy(x => x.SectorId)
                    .Select(g => new SectorCountItem
                    {
                        SectorId = g.Key,
                        DocCount = g.Sum(x => x.DocCount)
                    })
                    .ToList();

            }
           

            var countsBySector = sectorCounts.ToDictionary(item => item.SectorId, item => item.DocCount);
            foreach (var sector in sectors)
            {
                int count;
                sector.DocCount = countsBySector.TryGetValue(sector.Code, out count) ? count : 0;
            }

            rptSectors.DataSource = sectors;
            rptSectors.DataBind();
        }

        public string GetSectorImageUrl(object imgPath)
        {
            var imageName = gets(imgPath);
            return string.IsNullOrWhiteSpace(imageName)
                ? string.Empty
                : "/Layout/uploads/Sectors/" + imageName;
        }
    }
}
