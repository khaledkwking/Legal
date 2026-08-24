<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Admin.Master" AutoEventWireup="true" CodeBehind="Sectors.aspx.cs" Inherits="UI.Web.Modules.Laws.Sectors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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

    /*    .sectors-grid {
            display: flex;
            flex-wrap: wrap;
        }

        .sector-item {
            width: 20%;
            padding: 15px;
            box-sizing: border-box;
            text-align: center;
            border:1px solid black;
        }

        .sector-item img {
            max-width: 100%;
            height: auto;
            display: block;
            margin: 0 auto 10px;
        }

        @media (max-width: 1200px) {
            .sector-item { width: 25%; }
        }

        @media (max-width: 992px) {
            .sector-item { width: 33.3333%; }
        }

        @media (max-width: 768px) {
            .sector-item { width: 50%; }
        }

        @media (max-width: 480px) {
            .sector-item { width: 100%; }
        }*/
        .sectors-grid {
            padding-left: 0;
            padding-right: 0;
            position: relative;
            top: -250px;
            z-index: 10;
        }
        .sectors-container {
            display: grid;
            grid-template-columns: repeat(5, 1fr);
            gap: 15px;
            background: white;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 4px 10px rgb(0 0 0 / 12%);
            width: 80%;
            margin: auto;
        }

        .card {
            width: 100%;
            aspect-ratio: 5 / 5;
            position: relative;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            text-align: center;
            overflow: hidden;
            transition: 0.4s ease;
            box-shadow: 0 4px 10px rgb(0 0 0 / 12%);
            transform-style: preserve-3d;
        }

        .card-image {
            position: absolute;
            top: 0;
            left: 6%;
            right: 6%;
            height: 60%;
            background-size: contain;
            background-position: center;
            background-repeat: no-repeat;
            z-index: 1;
            transition: 0.4s ease;
        }

        .card-image::before {
            content: "";
            position: absolute;
            inset: 0;
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

        .card-number {
            position: absolute;
            top: 30%;
            left: 50%;
            transform: translate(-50%, -50%);
            color: #fff;
            font-size: 50px;
            font-weight: bold;
            opacity: 0;
            z-index: 2;
            transition: 0.4s ease;
            text-shadow: 0 2px 6px rgba(0,0,0,0.35);
        }

        .card-number span {
            display: block;
            font-size: 14px;
            font-weight: normal;
            margin-top: 4px;
        }

        .card h2 {
            font-size: 18px;
            font-weight: bold;
            color: rgba(182, 163, 119, 1);
            position: absolute;
            bottom: 0px;
            left: 0;
            right: 0;
            margin: 0;
            background: #fff;
            padding: 4px 12px 4px;
            text-align: right;
            z-index: 2;
            transition: 0.4s ease;
        }

        .card h2 span {
            display: block;
            font-size: 12px;
            font-weight: bold;
            color: #0C476B;
            margin-top: 15px;
            margin-bottom: 30px;
        }

        .card h2 span::after {
            content: " \2190";
            margin-right: 6px;
        }



        .card:hover .card-image::before {
            opacity: 1;
        }

        .card:hover {
            box-shadow: 0 4px 10px rgb(0 0 0 / 12%);
        }

        .card:hover .card-image {
            transform: translateY(-6px) rotateX(2deg) rotateY(2deg);
        }

        .card:hover .card-number {
            opacity: 1;
        }

        .sectors-container a {
            text-decoration: none;
        }
        @media (max-width: 1200px) {
            .sectors-container {
                grid-template-columns: repeat(3, 1fr);
            }
        }

        @media (max-width: 768px) {
            .sectors-container {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 480px) {
            .sectors-container {
                grid-template-columns: 1fr;
            }
        }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
  
    <div class="sectors-banner">
        <div class="sectors-banner-text">القطاعات</div>
    </div>
    <asp:HiddenField ID="hdnSectorId" runat="server" />
    <div class="sectors-grid">
        <div class="sectors-container">
            <asp:Repeater ID="rptSectors" runat="server">
                <ItemTemplate>
                    <a href="LawDetails.aspx?typeId=<%= hdnSectorId.Value %>&sectorId=<%# Eval("Code") %>">
                        <div class="card">
                            <div class="card-number">
                                <%# Eval("DocCount") %>
                                <span>تشريع</span>
                            </div>
                            <div class="card-image" style='background-image: url("<%# GetSectorImageUrl(Eval("imgPath")) %>");'></div>

                            <h2>
                                <%# Eval("NameAr") %>
                                <span>اقرا المزيد</span>
                            </h2>
                        </div>
                    </a>
                </ItemTemplate>
            </asp:Repeater>
</div>    

    </div>
</asp:Content>
