<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="LawType.aspx.cs" Inherits="UI.Web.Modules.Laws.LawType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .lawtypes-container {
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    gap: 15px;

    background: white;
    padding: 25px;
    border-radius: 8px;

    box-shadow: 0 4px 10px rgba(0,0,0,0.5);

    width: 100%;
    margin: auto;

    position: relative;
    top: -220px;
    z-index: 10;
}

/* ===== Card ===== */
.card {
    width: 100%;
    height: 180px;

    border: 0.65px solid rgb(12 71 107 / 40%);
    background: #fff;

    position: relative;

    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;

    text-align: center;

    overflow: hidden;
    transition: 0.4s ease;
}

/* ===== Gradient overlay ===== */
.card::before {
    content: "";
    position: absolute;
    inset: 0;

    /* 🔥 من أبيض فاتح جدًا إلى لونك الأساسي */
    background: linear-gradient(
        to bottom,
        rgba(255, 255, 255, 0.95) 0%,
        rgba(255, 255, 255, 0.6) 30%,
        rgba(12, 71, 107, 0.4) 70%,
        rgba(12, 71, 107, 0.9) 100%
    );

    opacity: 0;
    transition: 0.4s ease;
    z-index: 1;
}

/* ===== Icon ===== */
.card img {
    width: 65px;
    position: absolute;
    top: 10px;
    right: 10px;

    opacity: 0.6;
    z-index: 2;

    transition: 0.3s ease;
}

/* ===== Title (CENTER always) ===== */
.card h2 {
    font-size: 20px;
    font-weight: bold;
    color: #0C476B;

    position: absolute;
    top: 50%;
    transform: translateY(-50%);

    margin: 0;
    z-index: 2;

    transition: 0.4s ease;
}

/* ===== Number (hidden by default) ===== */
.card-count {
    font-size: 35px;
    font-weight: bold;
    color: #fff;

    opacity: 0;

    position: absolute;
    bottom: 35px;

    z-index: 2;

    transition: 0.4s ease;
}
.card-count .count-number {
    font-size: 35px;
    font-weight: bold;
}

/* النص تحت الرقم */
.card-count .count-text {
    font-size: 12px;
    margin-top: 2px;
}
/* ===== Hover ===== */
.card:hover::before {
    opacity: 1;
}

/* Title stays visible + turns white + moves up a bit */
.card:hover h2 {
    color: #0C476B;
    top: 20%;
    transform: translateY(0);
}

/* Show number */
.card:hover .card-count {
    opacity: 1;
    bottom: 5%;
}

/* Hide icon */
.card:hover img {
    opacity: 0;
}

/* Lift effect */
.card:hover {
    transform: translateY(-6px);
}

/* links */
.lawtypes-container a {
    text-decoration: none;
}

/* responsive */
@media (max-width: 1200px) {
    .lawtypes-container {
        grid-template-columns: repeat(3, 1fr);
    }
}

@media (max-width: 768px) {
    .lawtypes-container {
        grid-template-columns: repeat(2, 1fr);
        top: -60px;
    }
}

@media (max-width: 480px) {
    .lawtypes-container {
        grid-template-columns: 1fr;
    }
}
.page-container {
    position: relative;
    padding: 20px 10px;
    padding-bottom: 40px;
}
                    .sectors-banner {
        background-image: url('/Layout/uploads/MedalFiles/Banner.png');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: top center;
        width: 100%;                   /* full width of the screen */
    height: 900px;
    max-height: 80vh;
    min-height: 320px;
        position: relative;
        margin-bottom: 20px;
    }

    /* Optional: for very small screens */
    @media (max-width: 768px) {
        .sectors-banner {
        height: 360px;
        }
    }

.sectors-banner-text {
    position: absolute;
    top: 50%;
    right: 0;
    transform: translate(-50%, -50%);
    color: #ffffff;
    font-size: 60px;
    font-weight: bold;
    text-shadow: 0 2px 4px rgba(0,0,0,.5);
}
        .law-summary-item {
            font-size: 15px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
    <div class="sectors-banner">
        <div class="sectors-banner-text">
            تشريعات دولة الكويت
        </div>
   </div>
  <div class="row">
    <div class="col-md-10 col-md-offset-1 col-sm-12" style="">

        <div class="lawtypes-container">

            <asp:Repeater ID="rptLawTypes" runat="server">
                <ItemTemplate>

                    <a href='<%# 
                        Eval("NameAr").ToString().Contains("اللجان والمجالس العليا")
                            ? "CommitteeDetails.aspx?TypeID=1"
                            :Eval("NameAr").ToString().Contains("مجالس إدارات الهيئات والمؤسسات العامة")
                             ? "CommitteeDetails.aspx?TypeID=2"
                             : Eval("NameAr").ToString().Contains("التشكيلات الوزارية")
                                ? "/Modules/LibraryDocs/Forms/LibraryDocsview3.aspx?catid=7&typeid=48"
                                     //: Eval("NameAr").ToString().Contains("كتب ومذكرات الرأي القانوني")
                                     //    ? "/Modules/LegalMemos/Forms/LegalMemoData.aspx"
                                     : Eval("NameAr").ToString()==("الدستور")
                                         ? "/Modules/LibraryDocs/Forms/LibraryDocsview3.aspx?catid=4"
                                         : Eval("NameAr").ToString().Contains("سمو رئيس مجلس")
                                            ? "/Modules/laws/DessionData.aspx?T=6"
                                            : Eval("NameAr").ToString().Contains("مجلس الوزراء")
                                            ? "/Modules/laws/DessionData.aspx?T=5"
                                                : (Eval("NameAr").ToString() == "قانون" || Eval("NameAr").ToString() == "مرسوم بقانون" || Eval("NameAr").ToString() == "مرسوم بقانون و قانون")
                                                    ? "Sectors.aspx?typeId=" + Eval("Code")
                                                    : "LawDetails.aspx?typeId=" + Eval("Code")
                    %>'>

                        <div class="card">

                            <img src="/Layout/uploads/MedalFiles/Layer1.png" alt="icon" />

                            <h2><%# Eval("NameAr") %></h2>

                        <div class="card-count">
                            <div class="count-number"><%# GetLawCount(Eval("Code")) %></div>
                            <div class="count-text">تشريع</div>
                        </div>

                        </div>

                    </a>

                </ItemTemplate>
            </asp:Repeater>

        </div>

    </div>
</div>
</asp:Content>
