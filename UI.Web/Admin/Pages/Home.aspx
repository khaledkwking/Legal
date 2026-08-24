<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Masters/Tempmaster.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="UI.Web.Admin.Pages.Home" %>





<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">




    <%--DX--%>
    <link href="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/DX/dx.common.css" rel="stylesheet" />
    <link href="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/DX/dx.dark.css" rel="stylesheet" />
    <link href="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/DX/dx.light.css" rel="stylesheet" />
    <%--<link href="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/DX/dx.spa.css" rel="stylesheet" /> --%>

    <%--   <script type="text/javascript" src="<%= GetGlobalResourceObject("Utilities", "resourcespath")%>assets/js/core/libraries/jquery.min.js"></script>--%>

    <script src="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/DX_JS/globalize.min.js"></script>
    <script src="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/DX_JS/dx.all.js"></script>
    <script src="<%= GetGlobalResourceObject("Utilities", "Assetspath")%>Assets/DX_JS/dx.chartjs.js"></script>
    <%--END-DX--%>


    <div class="row">
        <div class="col-lg-3 col-xs-3 pull-right mr-20">

            <div class="pull-left">

                <%--  <button type="button" class="btn btn-info daterange-ranges heading-btn text-semibold">
										<i class="icon-calendar3 position-left"></i> <span></span> <b class="caret"></b>
									</button>--%>

                <div class="form-group pull-right" style="width: 100%">

                    <button type="button" class="btn btn-danger daterange-ranges">
                        <i class="icon-calendar22 position-left"></i><span id="spanreportFilter" runat="server"></span><b class="caret"></b>
                    </button>
                </div>



                <%--                        <div class="form-group">
							<div class="input-group">
								<span class="input-group-addon"><i class="icon-calendar22"></i></span>
								<asp:TextBox ID="txtFilterDate" runat="server" class="form-control daterange-basic"></asp:TextBox>
							</div>
						</div>--%>
            </div>
            <div class="pull-left">
                <input id="hdnDateRange" runat="server" type="hidden" />
                <asp:LinkButton ID="btnfilter" CssClass="btn btn-success btn-labeled" runat="server" OnClientClick="setDateRangehidden();" OnClick="btnfilter_Click"> <%= GetGlobalResourceObject("menu", "Reload")%><b><i class="icon-search4"></i></b></asp:LinkButton>
                <asp:LinkButton ID="btnReload" CssClass="btn btn-success hidden" runat="server"><%= GetGlobalResourceObject("menu", "Reload")%>&nbsp;&nbsp;<i class="fa fa-search"></i></asp:LinkButton>
                <script>
                    function setDateRangehidden() {
                        spanDaterange = document.getElementById("<%=spanreportFilter.ClientID %>")
                        //alert(spanDaterange.innerHTML);
                        hdnvalue = document.getElementById("<%=hdnDateRange.ClientID %>")
                        if (spanDaterange.innerHTML != "") {
                            hdnvalue.value = spanDaterange.innerHTML;
                        }
                        //alert(hdnvalue.value);
                        //return false;
                    }

                </script>
            </div>

        </div>
    </div>
    <div class="clearfix"></div>

    <script>
        function setactiveTab(tabindex) {
            // alert("para:" + tabindex)
            var txt = document.getElementById("<%=hdnactivetab.ClientID %>");
            txt.value = tabindex;
						// alert(txt.value);
						// var hangoutButton = document.getElementById("<%=btnReload.ClientID %>");
            // hangoutButton.click(); // this will trigger the click event

            // $("#QuestionStatusCount").dxPieChart("instance")._render();

        }
    </script>
    <input id="hdnactivetab" runat="server" type="hidden" />


    
    <% if (ShowSystem("6"))
	{ %>

    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8 col-xs-12">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        لوحة مؤشرات التشريعات

                    </h4>
                </div>

            </div>

            <div class="clearfix"></div>
        </div>
    </div>

    <div class="row">

        <div class="col-lg-2 col-xs-12">

            <!-- Today's revenue -->
            <div class="panel bg-soft-primary">
                <div class="panel-body">
                    <div class="heading-elements  icon-pen" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/laws/Forms/LawDocData.aspx?docTypeId=1" style="color: #000"><%=_LawDocType1%></a></h3>
                    <b>قانون</b>
                    <%--  <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalCasesDegree1Str %>  </div>--%>
                </div>


            </div>
            <!-- /today's revenue -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-secondary">
                <div class="panel-body">
                    <div class="heading-elements icon-checkmark3" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/laws/Forms/LawDocData.aspx?docTypeId=2" style="color: #000"><%=_LawDocType2%></a> </h3>
                    <b>مرسوم بقانون</b>
                    <%--  <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalMedalToMinisterStr %> </div>--%>
                </div>


            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-success">
                <div class="panel-body">
                    <div class="heading-elements icon-stamp" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/laws/Forms/LawDocData.aspx?docTypeId=3" style="color: #000"><%=_LawDocType3%></a> </h3>
                    <b>مرسوم أميري</b>
                    <%--   <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalMedalToDewanStr%> </div>--%>
                </div>

            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-info">
                <div class="panel-body">
                    <div class="heading-elements icon-chess-king" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/laws/Forms/LawDocData.aspx?docTypeId=4" style="color: #000"><%=_LawDocType4%></a> </h3>
                    <b>أمر أميري</b>
                    <%--<div class="text-muted text-size-mini"><%=Resources.CTOS.MedalsaderStr%> </div>--%>
                </div>

            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-warning">
                <div class="panel-body">
                    <div class="heading-elements icon-stack-empty" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/laws/Forms/LawDessionData.aspx?docTypeId=5" style="color: #000"><%=_LawDocType5%></a> </h3>
                    <b>قرار مجلس الوزراء</b>

                </div>

            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-danger">
                <div class="panel-body">
                    <div class="heading-elements icon-stack-plus" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/laws/Forms/LawDessionData.aspx?docTypeId=6" style="color: #000"><%=_LawDocType6%></a> </h3>
                    <b>قرار سمو رئيس مجلس الوزراء</b>

                </div>

            </div>
            <!-- /current server load -->

        </div>


    </div>

    <div class="row mbl">

        <div class="col-md-2">


            <div class="panel text-center bg-soft-warning">
                <div class="panel-body" style="min-height: 330px">
                    <div class="heading-elements">
                    </div>

                    <div class="content-group-sm svg-center position-relative" style="text-align: center;">

                        <div class="icon-newspaper" style="font-size: 40px; margin-bottom: 20px">
                        </div>

                        <h3 class="no-margin"><a href="/Modules/laws/Forms/LawDocData.aspx?ispublish=1" style="color: #000"><%=_LawDocPublished%></a> </h3>
                        <b>تم النشر</b>
                        <div class="text-muted text-size-large">التشريعات التي تم نشرها    </div>

                    </div>
                    <hr class="text-info" />

                      <div class="content-group-sm svg-center position-relative " style="text-align: center;">

                        <div class="icon-bell3 blink text-danger" style="font-size: 40px; margin-bottom: 20px">
                        </div>

                        <h3 class="no-margin text-danger"><a href="/Modules/laws/Forms/LawDocData.aspx?expire=1" ><%=_LawDocExpireCount%></a> </h3>
                        <div class="text-danger text-size-large">التشريعات التي سيتنهي العمل بها خلال 6 اشهر       </div>

                    </div>

                     


                </div>


            </div>
        </div>

        <div class="col-lg-10">
            <div class="panel">
                <div class="panel-body">
                    <div class="row">



                        <div class="col-lg-12 col-md-12">

                            <div id="_LawsTransYear" style="min-height: 350px;"></div>

                            <script>

                                var _LawsTransYear = [<%=_LawsTransYear%>];

                                $(function () {
                                    $("#_LawsTransYear").dxChart({
                                        dataSource: _LawsTransYear,
                                        //rotated:true,
                                        //equalBarWidth:false,
                                        commonSeriesSettings: {
                                            argumentField: "TransYear",
                                            valueField: "CasesCount",
                                            type: "bar",
                                            hoverMode: "allArgumentPoints",
                                            selectionMode: "allArgumentPoints",

                                            label: {
                                                visible: false,
                                                format: "fixedPoint",
                                                precision: 0,
                                                alignment: 'center',
                                                position: 'inside',
                                                rotationAngle: 90
                                            }
                                        },
                                        seriesTemplate: {
                                            nameField: "DegreeDesc",
                                            valueField: "CasesCount",
                                            type: "bar",
                                            smallValuesGrouping: {
                                                mode: "topN",
                                                topCount: 3
                                            }

                                        },

                                        title: "إحصائية التشريعات طبقا لسنة الإصدار ",
                                        palette: "ocean",
                                        crosshair: {
                                            enabled: true,
                                            color: "#949494",
                                            width: 3,
                                            dashStyle: "dot",
                                            label: {
                                                visible: true,
                                                backgroundColor: "#949494",
                                                font: {
                                                    color: "#fff",
                                                    size: 12,
                                                }
                                            }
                                        },
                                        legend: {
                                            verticalAlignment: "bottom",
                                            horizontalAlignment: "center",
                                            visible: false

                                        }, argumentAxis: {
                                            tickInterval: 5,
                                            label: {
                                                overlappingBehavior: { mode: 'rotate', rotationAngle: 45 },
                                                font: { size: 12 }
                                            },
                                            valueMarginsEnabled: false,
                                            discreteAxisDivisionMode: "crossLabels",
                                            grid: {
                                                visible: true
                                            },

                                            //onSeriesClick: function (e) {
                                            //    var series = e.target;
                                            //    series.isVisible() ? series.hide() : series.show();
                                            //},
                                        },
                                        //onSeriesClick: function (e) {
                                        //    var series = e.target;
                                        //    series.isVisible() ? series.hide() : series.show();
                                        //},
                                        tooltip: {
                                            enabled: true,
                                            customizeTooltip: function (arg) {
                                                return {
                                                    text: arg.value
                                                };
                                            }
                                        }
                                    });


                                });

                            </script>
                        </div>





                    </div>
                </div>
            </div>
        </div>

    </div>
    <%} %>

   

     <%--Question --%>
    <% if (ShowSystem("8"))
    { %>
    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8 col-xs-12">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        <%=Resources.CTOS.QuestionsDashborad %>

                    </h4>
                </div>

            </div>

            <div class="clearfix"></div>
        </div>
    </div>

    <!-- Quick stats boxes -->
    <div class="row" style="display: none">

        <div class="col-lg-2 col-xs-12">
            <!-- Today's revenue -->
            <div class="panel bg-green-300">
                <div class="panel-body">
                    <div class="heading-elements icon-checkmark3" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=1" style="color: #000"><%=_TotalQuestion_Relied%></a></h3>
                    <b><%=Resources.CTOS._TotalQuestion_Reliedstr %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_ReliedDesc %>  </div>

                </div>


            </div>
            <!-- /today's revenue -->

        </div>
        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-danger-300">
                <div class="panel-body">
                    <div class="heading-elements icon-warning22" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=2" style="color: #000"><%=_TotalQuestion_Delaied%></a> </h3>
                    <b><%=Resources.CTOS._TotalQuestion_DelaiedStr %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_DelaiedDesc %>  </div>

                </div>

            </div>
            <!-- /current server load -->

        </div>
        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-info-300">
                <div class="panel-body">
                    <div class="heading-elements icon-question3" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=3" style="color: #000"><%=_TotalQuestion_NoAnswer%></a> </h3>
                    <b><%=Resources.CTOS._TotalQuestion_NoAnswerStr %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_NoAnswerDesc%> </div>

                </div>

            </div>
            <!-- /current server load -->

        </div>
        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-info-300">
                <div class="panel-body">
                    <div class="heading-elements icon-alarm-check" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=4" style="color: #000"><%=_TotalQuestion_Estifaa%></a> </h3>
                    <b><%=Resources.CTOS._TotalQuestion_Estifaa %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_Estifaadesc%> </div>

                </div>

            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-teal-300">
                <div class="panel-body">
                    <div class="heading-elements icon-file-check2" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=5" style="color: #000"><%=_TotalQuestion_partial%></a> </h3>
                    <b><%=Resources.CTOS._TotalQuestion_partial %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_partialdesc%> </div>

                </div>

            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-violet-300" style="height: 150px;">
                <div class="panel-body">
                    <div class="heading-elements icon-wall" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=0" style="color: #000"><%=_TotalQuestion_All%></a> </h3>
                    <b><%=Resources.CTOS._TotalQuestion_AllStr %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_AllDesc%> </div>

                </div>

            </div>
            <!-- /current server load -->

        </div>
    </div>

    <div class="row">

        <div class="panel panel-flat bg-soft-info">
            <div class="panel-heading">
                <h6 class="panel-title"><%=Resources.CTOS.QuestionsDashborad2 %>  <%=ChapterName %> </h6>
            </div>

            <%--  <div class="table-responsive">--%>
            <table class="table">
                <tbody>
                    <tr>
                        <td>
                            <div class="media-left" style="vertical-align: middle;">
                                <div class="icon-checkmark3" style="font-size: 30px">
                                </div>
                            </div>
                            <div class="media-left">
                                <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=1&chapter=<%=Chapterid %>" style="color: #000"><%=_TotalQuestion_Relied2%></a></h3>
                                <b><%=Resources.CTOS._TotalQuestion_Reliedstr %></b>
                                <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_ReliedDesc %>  </div>
                            </div>
                        </td>


                        <td>
                            <div class="media-left" style="vertical-align: middle;">
                                <div class="icon-warning22" style="font-size: 30px">
                                </div>
                            </div>
                            <div class="media-left">
                                <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=2&chapter=<%=Chapterid %>" style="color: #000"><%=_TotalQuestion_Delaied2%></a> </h3>
                                <b><%=Resources.CTOS._TotalQuestion_DelaiedStr %></b>
                                <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_DelaiedDesc %>  </div>
                            </div>
                        </td>
                        <td>
                            <div class="media-left" style="vertical-align: middle;">
                                <div class="icon-question3" style="font-size: 30px">
                                </div>
                            </div>
                            <div class="media-left">
                                <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=3&chapter=<%=Chapterid %>" style="color: #000"><%=_TotalQuestion_NoAnswer2%></a> </h3>
                                <b><%=Resources.CTOS._TotalQuestion_NoAnswerStr %></b>
                                <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_NoAnswerDesc%> </div>
                            </div>
                        </td>
                        <td>
                            <div class="media-left" style="vertical-align: middle;">
                                <div class="icon-alarm-check" style="font-size: 30px">
                                </div>
                            </div>
                            <div class="media-left">
                                <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=4&chapter=<%=Chapterid %>" style="color: #000"><%=_TotalQuestion_Estifaa2%></a> </h3>
                                <b><%=Resources.CTOS._TotalQuestion_Estifaa %></b>
                                <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_Estifaadesc%> </div>
                            </div>
                        </td>
                        <td>
                            <div class="media-left" style="vertical-align: middle;">
                                <div class="icon-file-check2" style="font-size: 30px">
                                </div>
                            </div>
                            <div class="media-left">
                                <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=5&chapter=<%=Chapterid %>" style="color: #000"><%=_TotalQuestion_partial2%></a> </h3>
                                <b><%=Resources.CTOS._TotalQuestion_partial %></b>
                                <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_partialdesc%> </div>
                            </div>
                        </td>
                        <td>
                            <div class="media-left" style="vertical-align: middle;">
                                <div class="icon-file-check2" style="font-size: 30px">
                                </div>
                            </div>
                            <div class="media-left">
                                <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=6&chapter=<%=Chapterid %>" style="color: #000"><%=_TotalQuestion_Unlegal%></a> </h3>
                                <b>مخالف للأحكام الدستورية</b>
                                <div class="text-muted text-size-mini">عدد الأسئلة الأستجوبات المخالفة للأحكام الدستورية </div>
                            </div>
                        </td>
                        <td>
                            <div class="media-left" style="vertical-align: middle;">
                                <div class="icon-wall" style="font-size: 30px">
                                </div>
                            </div>
                            <div class="media-left">
                                <h3 class="no-margin"><a href="/Modules/questions/Forms/questionsData.aspx?StatusID=0&chapter=<%=Chapterid %>" style="color: #000"><%=_TotalQuestion_All2%></a> </h3>
                                <b><%=Resources.CTOS._TotalQuestion_AllStr %></b>
                                <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalQuestion_AllDesc%> </div>
                            </div>
                        </td>



                    </tr>
                </tbody>
            </table>
            <%--  </div>--%>
        </div>
    </div>

    <div class="row mbl">
        <div class="col-lg-12">
            <div class="panel">
                <div class="panel-body">
                    <div class="col-lg-6 col-md-12">

                        <div id="QuestionStatusCount" style="width: 100%; height: 400px"></div>
                        <script>

                            var QuestionStatusCount = [<%=_QuestionStatusCount%>];

                            $("#QuestionStatusCount").dxPieChart({
                                dataSource: QuestionStatusCount,
                                title: "<%=Resources.CTOS.QuestionStstusCOunt%>",
                                //  palette: ['#0072C6', 'OrangeRed', 'Orange', '#B78C9B', '#F2CA84', '#A7CA74'],
                                palette: "Pastel",
                                resolveLabelOverlapping: 'shift',
                                legend: {
                                    horizontalAlignment: "center",
                                    verticalAlignment: "bottom"
                                },
                                series: [{
                                    argumentField: "StatusName",
                                    valueField: "CasesCount",
                                    label: {
                                        visible: true,
                                        connector: {
                                            visible: true,
                                            width: 0.5
                                        },
                                        format: "fixedPoint",
                                        customizeText: function (point) {
                                            return point.argumentText + ":" + "(" + point.percentText + ") " + point.valueText;
                                        }
                                    },
                                    //  palette: ["#00ced1", "#008000", "#ffd700", "#ff7f50"],

                                    onPointClick: function (e) {
                                        var point = e.target;
                                        toggleVisibility(point);
                                    },
                                    onLegendClick: function (e) {
                                        var arg = e.target;
                                        toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
                                    }
                                    ,
                                    //smallValuesGrouping: {
                                    //    mode: "smallValueThreshold",
                                    //    threshold: 2
                                    //},
                                    tooltip: {
                                        enabled: true,
                                        customizeTooltip: function (arg) {
                                            return {
                                                text: arg.seriesName + " : " + arg.percentText + " - " + arg.valueText
                                            };
                                        }
                                    }


                                }]


                            });

                            function toggleVisibility(item) {
                                item.isVisible() ? item.hide() : item.show();
                            }

                        </script>

                    </div>

                    <div class="col-lg-6 col-md-12">

                        <div id="QuestionStatusCountTarget" style="width: 100%; height: 400px"></div>
                        <script>

                            var QuestionStatusCount = [<%=_QuestionTargetCount%>];

                            $("#QuestionStatusCountTarget").dxPieChart({
                                dataSource: QuestionStatusCount,
                                resolveLabelOverlapping: 'shift',
                                title: {
                                    text:"<%=Resources.CTOS.QuestionTargetCOunt%> <%=ChapterName %>",
                                    font: { size: 20 },
                                    subtitle: { text: "الأسئلة الموجهة لسمو رئيس مجلس الوزراء ووزير الدولة  " }
                                },



                                palette: ['#B78C9B', '#F2CA84', '#A7CA74', '#0072C6', 'OrangeRed', 'Orange',],
                                // palette: "Pastel",
                                legend: {
                                    horizontalAlignment: "center",
                                    verticalAlignment: "bottom"
                                },
                                series: [{
                                    argumentField: "StatusName",
                                    valueField: "CasesCount",
                                    label: {
                                        visible: true,
                                        connector: {
                                            visible: true,
                                            width: 0.5
                                        },
                                        format: "fixedPoint",
                                        customizeText: function (point) {
                                            return point.argumentText + ":" + "(" + point.percentText + ") " + point.valueText;
                                        }
                                    },
                                    //  palette: ["#00ced1", "#008000", "#ffd700", "#ff7f50"],

                                    onPointClick: function (e) {
                                        var point = e.target;
                                        toggleVisibility(point);
                                    },
                                    onLegendClick: function (e) {
                                        var arg = e.target;
                                        toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
                                    }
                                    ,
                                    //smallValuesGrouping: {
                                    //    mode: "smallValueThreshold",
                                    //    threshold: 2
                                    //},
                                    tooltip: {
                                        enabled: true,
                                        customizeTooltip: function (arg) {
                                            return {
                                                text: arg.seriesName + " : " + arg.percentText + " - " + arg.valueText
                                            };
                                        }
                                    }


                                }]


                            });

                            function toggleVisibility(item) {
                                item.isVisible() ? item.hide() : item.show();
                            }

                        </script>

                    </div>
                </div>
            </div>
        </div>
    </div>

      <div class="row mbl">

        <div class="col-md-2">


            <div class="panel text-center bg-soft-warning">
                <div class="panel-body" style="min-height: 330px">
                    <div class="heading-elements">
                    </div>

                    <div class="content-group-sm svg-center position-relative" style="text-align: center;">

                        <div class="icon-newspaper" style="font-size: 40px; margin-bottom: 20px">
                        </div>

                        <h3 class="no-margin"><a href="/Modules/Questions/Forms/QuestionsData.aspx?all=1" style="color: #000"><%=gets(_ALLQuestionCount)%></a> </h3>
                        <b>  إجمالي الأسئلة </b>
                        <div class="text-muted text-size-large">    منذ بداية الحياة البرلمانية      </div>

                    </div>
                </div>


            </div>
        </div>

        <div class="col-lg-10">
            <div class="panel">
                <div class="panel-body">
                    <div class="row">



                        <div class="col-lg-12 col-md-12">

                            <div id="_QuestionALLCount" style="min-height: 300px;"></div>

                            <script>

                                var _QuestionALLCount = [<%=_QuestionALLCount%>];

                                $(function () {
                                    $("#_QuestionALLCount").dxChart({
                                        dataSource: _QuestionALLCount,
                                        //rotated:true,
                                        //equalBarWidth:false,
                                        series: {
                                            argumentField: "ChapterName",
                                            valueField: "QuestionCount",
                                            type: "bar",
                                            hoverMode: "allArgumentPoints",
                                            selectionMode: "allArgumentPoints",

                                            label: {
                                                visible: false,
                                                format: "fixedPoint",
                                                precision: 0,
                                                alignment: 'center',
                                                position: 'inside',
                                                rotationAngle: 90
                                            }
                                        },
                                        //seriesTemplate: {
                                        //    nameField: "ChapterName",
                                        //    valueField: "QuestionCount",
                                        //    type: "bar",
                                        //    smallValuesGrouping: {
                                        //        mode: "topN",
                                        //        topCount: 3
                                        //    }

                                        //},

                                        title: "إحصائية الأسئلة البرلمانية منذ بداية الحياة البرلمانية  ",
                                        palette: "ocean",
                                        crosshair: {
                                            enabled: true,
                                            color: "#949494",
                                            width: 3,
                                            dashStyle: "dot",
                                            label: {
                                                visible: true,
                                                backgroundColor: "#949494",
                                                font: {
                                                    color: "#fff",
                                                    size: 12,
                                                }
                                            }
                                        },
                                        legend: {
                                            verticalAlignment: "bottom",
                                            horizontalAlignment: "center",
                                            visible: false

                                        }, argumentAxis: {
                                            tickInterval: 5,
                                            label: {
                                                overlappingBehavior: { mode: 'rotate', rotationAngle: 45 },
                                                font: { size: 12 }
                                            },
                                            valueMarginsEnabled: false,
                                            discreteAxisDivisionMode: "crossLabels",
                                            grid: {
                                                visible: true
                                            },

                                            //onSeriesClick: function (e) {
                                            //    var series = e.target;
                                            //    series.isVisible() ? series.hide() : series.show();
                                            //},
                                        },
                                        //onSeriesClick: function (e) {
                                        //    var series = e.target;
                                        //    series.isVisible() ? series.hide() : series.show();
                                        //},
                                        tooltip: {
                                            enabled: true,
                                            customizeTooltip: function (arg) {
                                                return {
                                                    text: arg.value
                                                };
                                            }
                                        }
                                    });


                                });

                            </script>
                        </div>





                    </div>
                </div>
            </div>
        </div>

    </div>

    <%} %>

    
    <%--Legal Memo--%>
    <% if (ShowSystem("23"))
    { %>

    <div class="page-title2">
        <h4>
            <i class="icon-grid position-left"></i>
            <%=Resources.menu.LegalMemoTitle %>

        </h4>
    </div>
    <div class="row mbl">

        <div class="col-md-2">


            <div class="panel text-center bg-soft-info">
                <div class="panel-body" style="min-height: 330px">
                    <div class="heading-elements">
                    </div>

                    <div class="content-group-sm svg-center position-relative" style="text-align: center;">

                        <div class="icon-newspaper" style="font-size: 40px; margin-bottom: 20px">
                        </div>

                        <h3 class="no-margin"><a href="/Modules/LegalMemos/Forms/LegalMemoData.aspx?all=1" style="color: #000"><%=gets(_AllLegalMemo)%></a> </h3>
                     <%--   <b>  إجمالي الأسئلة </b>--%>
                        <div class="text-muted text-size-large"> إجمالي كتب ومذكرات الرأي القانوني </div>

                    </div>
                </div>


            </div>
        </div>

        <div class="col-lg-10">
            <div class="panel">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-6 col-md-12">

                            <div id="legalMemopie" style="width: 100%; height: 250px"></div>

                            <script>

                                var legalMemmoProcedure= [<%=_legalMemmoProcedure%>];

                                $("#legalMemopie").dxPieChart({
                                    dataSource: legalMemmoProcedure,
                                    title: "كتب ومذكرات الرأي القانوني طبقا للإجراء",

                                    legend: {
                                        horizontalAlignment: "right",
                                        verticalAlignment: "top",
                                        itemTextPosition: "left",
                                        visible: false
                                    },
                                    series: [{
                                        argumentField: "DocStatus",
                                        valueField: "RecordCount",
                                        label: {
                                            visible: true,
                                            connector: {
                                                visible: true,
                                                width: 0.2
                                            },
                                            // format: "fixedPoint",
                                            customizeText: function (point) {
                                                return point.argumentText + ":" + "(" + point.percentText + ") " + point.valueText;
                                            }
                                        },

                                        onPointClick: function (e) {
                                            var point = e.target;
                                            toggleVisibility(point);
                                        },
                                        onLegendClick: function (e) {
                                            var arg = e.target;
                                            toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
                                        }
                                        ,
                                        smallValuesGrouping: {
                                            mode: "smallValueThreshold",
                                            threshold: 1
                                        },
                                        tooltip: {
                                            enabled: true,
                                            customizeTooltip: function (arg) {
                                                return {
                                                    text: arg.seriesName + " : " + arg.percentText + " - " + arg.valueText
                                                };
                                            }
                                        }


                                    }]


                                });

                                function toggleVisibility(item) {
                                    item.isVisible() ? item.hide() : item.show();
                                }

                            </script>
                        </div>


                        <div class="col-lg-6 col-md-12">

                            <div id="legalMemoBar" style="min-height: 300px;"></div>

                            <script>

                                var _LegalMemoStat = [<%=_LegalMemoPerYear%>];

                                $(function () {
                                    $("#legalMemoBar").dxChart({
                                        dataSource: _LegalMemoStat,
                                        //rotated:true,
                                        //equalBarWidth:false,
                                        commonSeriesSettings: {
                                            argumentField: "TransYear",
                                            valueField: "RecordCount",
                                            type: "stackedBar",
                                            hoverMode: "allArgumentPoints",
                                            selectionMode: "allArgumentPoints",

                                            label: {
                                                visible: false,
                                                format: "fixedPoint",
                                                precision: 0,
                                                alignment: 'center',
                                                position: 'inside',
                                                rotationAngle: 90
                                            }
                                        },
                                        seriesTemplate: {
                                            nameField: "DocStatus",
                                            valueField: "RecordCount",
                                            type: "bar",
                                            smallValuesGrouping: {
                                                mode: "topN",
                                                topCount: 3
                                            }

                                        },

                                        title: "إحصائية كتب ومذكرات الرأي القانوني  طبقا للسنة والحالة    ",
                                        palette: ["#02b6ad", "#ffd365", "#b34446", "#ff7f50"],
                                        //palette: ["#e884aa", "#9f5287", "#ffd365", "#02b6ad","#b34446","#5cb85c"],
                                        legend: {
                                            verticalAlignment: "bottom",
                                            horizontalAlignment: "center"
                                        }, argumentAxis: {
                                            tickInterval: 5,
                                            label: {
                                                overlappingBehavior: { mode: 'rotate', rotationAngle: 45 },
                                                font: { size: 12 }
                                            },
                                            valueMarginsEnabled: false,
                                            discreteAxisDivisionMode: "crossLabels",
                                            grid: {
                                                visible: true
                                            },

                                            onSeriesClick: function (e) {
                                                var series = e.target;
                                                series.isVisible() ? series.hide() : series.show();
                                            },
                                        },
                                        tooltip: {
                                            enabled: true,
                                            customizeTooltip: function (arg) {
                                                return {
                                                    /*     text: arg.value*/
                                                    text: arg.seriesName + " :  " + arg.value
                                                };
                                            }
                                        },
                                        onSeriesClick: function (e) {
                                            var series = e.target;
                                            series.isVisible() ? series.hide() : series.show();
                                        },
                                    });


                                });

                            </script>
                        </div>





                    </div>
                </div>
            </div>
        </div>

    </div>
     
     
    <%} %>

       <%--Cases DashBord--%>

    <% if (ShowSystem("6"))
	 { %>
    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8 col-xs-12">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        <%=Resources.CTOS.CasesDashbord %>

                    </h4>
                </div>

            </div>

            <div class="clearfix"></div>
        </div>
    </div>
    <div class="row">

        <div class="col-lg-1 col-xs-12">

            <!-- Today's revenue -->
            <div class="panel bg-soft-success">
                <div class="panel-body">
                    <div class="heading-elements icon-office" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Cases/Forms/CasesData.aspx?DegreeID=1" style="color: #000"><%=_TotalCasesDegree1%></a></h3>
                    <b><%=Resources.CTOS._TotalCasesDegree1 %></b>
                    <%--  <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalCasesDegree1Str %>  </div>--%>
                </div>


            </div>
            <!-- /today's revenue -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-secondary">
                <div class="panel-body">
                    <div class="heading-elements icon-alarm-check" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Cases/Forms/CasesData.aspx?DegreeID=2" style="color: #000"><%=_TotalCasesDegree2%></a> </h3>
                    <b><%=Resources.CTOS._TotalCasesDegree2 %></b>
                    <%--  <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalMedalToMinisterStr %> </div>--%>
                </div>


            </div>
            <!-- /current server load -->

        </div>
        <div class="col-lg-1 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-secondary">
                <div class="panel-body">
                    <div class="heading-elements icon-menu-close2" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Cases/Forms/CasesData.aspx?DegreeID=8" style="color: #000"><%=_TotalCasesDegree8%></a> </h3>
                    <b><%=Resources.CTOS._TotalCasesDegree8 %></b>
                    <%--<div class="text-muted text-size-mini"><%=Resources.CTOS.MedalsaderStr%> </div>--%>
                </div>

            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-info">
                <div class="panel-body">
                    <div class="heading-elements icon-stamp" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Cases/Forms/CasesData.aspx?DegreeID=3" style="color: #000"><%=_TotalCasesDegree3%></a> </h3>
                    <b><%=Resources.CTOS._TotalCasesDegree3 %></b>
                    <%--   <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalMedalToDewanStr%> </div>--%>
                </div>

            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-warning">
                <div class="panel-body">
                    <div class="heading-elements icon-stack-text" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Cases/Forms/CasesData.aspx?DegreeID=4" style="color: #000"><%=_TotalCasesDegree4%></a> </h3>
                    <b><%=Resources.CTOS._TotalCasesDegree4 %></b>
                    <%--<div class="text-muted text-size-mini"><%=Resources.CTOS.MedalsaderStr%> </div>--%>
                </div>

            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-1 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-danger">
                <div class="panel-body">
                    <div class="heading-elements icon-file-check2" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Cases/Forms/CasesData.aspx?DegreeID=5" style="color: #000"><%=_TotalCasesDegree5%></a> </h3>
                    <b><%=Resources.CTOS._TotalCasesDegree5 %></b>
                    <%--<div class="text-muted text-size-mini"><%=Resources.CTOS.MedalsaderStr%> </div>--%>
                </div>

            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-pink">
                <div class="panel-body">
                    <div class="heading-elements icon-menu-close2" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Cases/Forms/CasesData.aspx?DegreeID=6" style="color: #000"><%=_TotalCasesDegree6%></a> </h3>
                    <b><%=Resources.CTOS._TotalCasesDegree6 %></b>
                    <%--<div class="text-muted text-size-mini"><%=Resources.CTOS.MedalsaderStr%> </div>--%>
                </div>

            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-1 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-primary">
                <div class="panel-body">
                    <div class="heading-elements icon-file-check2" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Cases/Forms/CasesData.aspx?DegreeID=7" style="color: #000"><%=_TotalCasesDegree7%></a> </h3>
                    <b><%=Resources.CTOS._TotalCasesDegree7 %></b>
                    <%--<div class="text-muted text-size-mini"><%=Resources.CTOS.MedalsaderStr%> </div>--%>
                </div>

            </div>
            <!-- /current server load -->

        </div>




    </div>

    <div class="row mbl">
        <div class="col-lg-12">
            <div class="panel">
                  <div class="panel-body">
                    <div class="col-lg-12 col-md-12">
                        <div id="_CasesTransYear" style="min-height: 500px;"></div>

                        <script>
                            var _CasesTransYear = [<%=_CasesTransYear%>];
                            $(function () {
                                $("#_CasesTransYear").dxChart({
                                    dataSource: _CasesTransYear,
                                    //rotated:true,
                                    //equalBarWidth:false,
                                    commonSeriesSettings: {
                                        argumentField: "TransYear",
                                        valueField: "CasesCount",
                                        type: "bar",
                                        hoverMode: "allArgumentPoints",
                                        selectionMode: "allArgumentPoints",

                                        label: {
                                            visible: false,
                                            format: "fixedPoint",
                                            precision: 0,
                                            alignment: 'center',
                                            position: 'inside',
                                            rotationAngle: 90
                                        }
                                    },
                                    seriesTemplate: {
                                        nameField: "DegreeDesc",
                                        valueField: "CasesCount",
                                        type: "bar",
                                        smallValuesGrouping: {
                                            mode: "topN",
                                            topCount: 3
                                        }

                                    },

                                    title: "إحصائية القضايا طبقا للسنة ودرجة التقاضي ",
                                    palette: ["#02b6ad", "#ffd365", "#b34446", "#ff7f50"],
                                    //palette: ["#e884aa", "#9f5287", "#ffd365", "#02b6ad","#b34446","#5cb85c"],
                                    legend: {
                                        verticalAlignment: "bottom",
                                        horizontalAlignment: "center"
                                    }, argumentAxis: {
                                        tickInterval: 5,
                                        label: {
                                            overlappingBehavior: { mode: 'rotate', rotationAngle: 45 },
                                            font: { size: 12 }
                                        },
                                        valueMarginsEnabled: false,
                                        discreteAxisDivisionMode: "crossLabels",
                                        grid: {
                                            visible: true
                                        },

                                        onSeriesClick: function (e) {
                                            var series = e.target;
                                            series.isVisible() ? series.hide() : series.show();
                                        },
                                    },
                                    tooltip: {
                                        enabled: true,
                                        customizeTooltip: function (arg) {
                                            return {
                                                /*     text: arg.value*/
                                                text: arg.seriesName + " :  " + arg.value
                                            };
                                        }
                                    },
                                    onSeriesClick: function (e) {
                                        var series = e.target;
                                        series.isVisible() ? series.hide() : series.show();
                                    },
                                });


                            });

                        </script>


                    </div>
                       </div>

                </div>
            </div>
         

        <%--<div class="col-lg-4">
            <div class="panel">


                <div class="panel-body" style="min-height: 530px;">

                    <div class="col-lg-12 col-md-12">

                        <div id="_CasesJudgmentresult" style="width: 100%; height: 400px;"></div>

                        <script>

                            var VesselVisits = [<%=_CasesJudgmentresult%>];

                            $("#_CasesJudgmentresult").dxPieChart({
                                dataSource: VesselVisits,
                                resolveLabelOverlapping: 'shift',
                                title: {
                                    text:"<%=Resources.CTOS.Judgmentresult%>",
                                    font: { size: 20 },
                                },


                                legend: {
                                    horizontalAlignment: "right",
                                    verticalAlignment: "top",
                                    itemTextPosition: "left",
                                    visible: false
                                },
                                series: [{
                                    argumentField: "StatusName",
                                    valueField: "CasesCount",
                                    label: {
                                        visible: true,
                                        connector: {
                                            visible: true,
                                            width: 0.2
                                        },
                                        // format: "fixedPoint",
                                        customizeText: function (point) {
                                            return point.argumentText + ":" + "(" + point.percentText + ") " + point.valueText;
                                        }
                                    },

                                    onPointClick: function (e) {
                                        var point = e.target;
                                        toggleVisibility(point);
                                    },
                                    onLegendClick: function (e) {
                                        var arg = e.target;
                                        toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
                                    }
                                    ,
                                    smallValuesGrouping: {
                                        mode: "smallValueThreshold",
                                        threshold: 1
                                    },
                                    tooltip: {
                                        enabled: true,
                                        customizeTooltip: function (arg) {
                                            return {
                                                text: arg.seriesName + " : " + arg.percentText + " - " + arg.valueText
                                            };
                                        }
                                    }


                                }]


                            });

                            function toggleVisibility(item) {
                                item.isVisible() ? item.hide() : item.show();
                            }

                        </script>
                    </div>

                </div>
            </div>
        </div>--%>
   
</div>
    <%} %>


    <%--Aggreements--%>
    <% if (ShowSystem("3"))
    { %>

    <div class="page-title2">
        <h4>
            <i class="icon-grid position-left"></i>
            <%=Resources.menu.summeryBoardTitle %>

        </h4>
    </div>
    <!-- Quick stats boxes -->
    <div class="row">

        <div class="col-lg-2 col-xs-12">

            <!-- Today's revenue -->
            <div class="panel bg-soft-success">
                <div class="panel-body">
                    <div class="heading-elements icon-pen-minus" style="font-size: 30px;">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Agreements/Forms/AgreementsData.aspx?ProTypeID=1" style="color: #000"><%=_TotalUnderStudy%></a></h3>
                    <b><%=Resources.CTOS._TotalUnderStudy %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalUnderStudyStr %></div>

                </div>


            </div>
            <!-- /today's revenue -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-info">
                <div class="panel-body">
                    <div class="heading-elements  icon-users4" style="font-size: 30px;">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Agreements/Forms/AgreementsData.aspx?ProTypeID=2" style="color: #000"><%=_TotalToMinistery%></a> </h3>
                    <b><%=Resources.CTOS._TotalToMinistery %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalToMinisterystr %> </div>

                </div>


            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-danger">
                <div class="panel-body">
                    <div class="heading-elements icon-library2" style="font-size: 30px;">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Agreements/Forms/AgreementsData.aspx?ProTypeID=3" style="color: #000"><%=_TotalToLaw%></a> </h3>
                    <b><%=Resources.CTOS._TotalToLaw %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalToLawStr %> </div>

                </div>

            </div>
            <!-- /current server load -->

        </div>



        <div class="col-lg-2 col-xs-12">

            <!-- Today's revenue -->
            <div class="panel bg-soft-secondary">
                <div class="panel-body">
                    <div class="heading-elements icon-stamp" style="font-size: 30px;">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Agreements/Forms/AgreementsData.aspx?ProTypeID=16" style="color: #000"><%=_TotalCMGS%></a></h3>
                    <b><%=Resources.CTOS._TotalCMGS %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalCMGSDESC %>  </div>

                </div>


            </div>
            <!-- /today's revenue -->

        </div>
        <div class="col-lg-2 col-xs-12">


            <div class="panel bg-soft-warning">
                <div class="panel-body" style="padding: 12px">
                    <div class="heading-elements" style="font-family: 'Droid Arabic Kufi'; top: 10px; font-size: 16px;">
                        <b><%=Resources.CTOS._TotalPublished %></b>
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Agreements/Forms/AgreementsData.aspx?ProTypeID=6" style="color: #000"><%=_TotalPublished%></a>


                    </h3>

                    <%-- <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalPublishedSfd %>    </div>--%>

                    <div class="row text-center">
                        <div class="col-md-4">
                            <div class="">
                                <h6 class="text-bold no-margin"><i class="icon-clipboard3 text-warning position-left"></i><%=_TotalPublished_Law %></h6>
                                <span class="text-muted text-size-small">صدر بقانون</span>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="">
                                <h6 class="text-bold no-margin"><i class="icon-stamp text-success position-left"></i><%=_TotalPublished_Marsoom %></h6>
                                <span class="text-muted text-size-small">صدر بمرسوم</span>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="">
                                <h6 class="text-bold no-margin"><i class="icon-history text-warning position-left"></i><%=_TotalPublished_Unknown %></h6>
                                <span class="text-muted text-size-small">غير معرف</span>
                            </div>
                        </div>
                    </div>


                </div>

            </div>


        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Today's revenue -->
            <div class="panel bg-soft-primary">
                <div class="panel-body">
                    <div class="heading-elements icon-drag-right" style="font-size: 30px;">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Agreements/Forms/AgreementsData.aspx?ProTypeID=-1" style="color: #000"><%=_TotalOther%></a></h3>
                    <b><%=Resources.CTOS._TotalOther %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalOtherStr %>  </div>

                </div>


            </div>
            <!-- /today's revenue -->

        </div>

    </div>
    <div class="row mbl">

        <div class="col-md-2">


            <div class="panel text-center bg-soft-secondary">
                <div class="panel-body" style="min-height: 330px">
                    <div class="heading-elements">
                    </div>

                    <div class="content-group-sm svg-center position-relative" style="text-align: center;">

                        <div class="icon-make-group" style="font-size: 40px">
                        </div>

                        <h3 class="no-margin"><%=_TotalAgreementAll%> </h3>
                        إجمالي الإتفاقيات
                        

                    </div>

                    <hr />
                    <div class="row text-center">
                        <div class="col-md-12">
                            <div class="mt-10">
                                <h6 class="text-bold no-margin"><i class="icon-clipboard3 text-info position-left"></i><%=_TotalInitial %></h6>
                                <span class="text-muted text-size-small">إتفاقيات مبدئية</span>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="mt-10">
                                <h6 class="text-bold no-margin"><i class="icon-stamp text-success position-left"></i><%=_TotalFinal %></h6>
                                <span class="text-muted text-size-small">إتفاقيات نهائية</span>
                            </div>
                        </div>
                        <%--  <hr />
                        <div class="col-md-12">
                            <div class="mt-10">
                                <h6 class="text-bold no-margin"><i class="icon-history text-warning position-left"></i><%=_TotalFinalFromInitial %></h6>
                                <span class="text-muted text-size-small">إتفاقيات مبدئية تحولت نهائية  </span>
                            </div>
                        </div>--%>
                    </div>

                </div>


            </div>
        </div>

        <div class="col-lg-10">
            <div class="panel">


                <div class="panel-body">
                    <div class="row">



                        <div class="col-lg-6 col-md-12">

                            <div id="CasesStatusCount" style="width: 100%; height: 250px"></div>

                            <script>

                                var VesselVisits = [<%=_TicketMonthlyRate%>];

                                $("#CasesStatusCount").dxPieChart({
                                    dataSource: VesselVisits,
                                    title: "<%=Resources.CTOS.ticketRate%>",

                                    legend: {
                                        horizontalAlignment: "right",
                                        verticalAlignment: "top",
                                        itemTextPosition: "left",
                                        visible: false
                                    },
                                    series: [{
                                        argumentField: "StatusName",
                                        valueField: "CasesCount",
                                        label: {
                                            visible: true,
                                            connector: {
                                                visible: true,
                                                width: 0.2
                                            },
                                            // format: "fixedPoint",
                                            customizeText: function (point) {
                                                return point.argumentText + ":" + "(" + point.percentText + ") " + point.valueText;
                                            }
                                        },

                                        onPointClick: function (e) {
                                            var point = e.target;
                                            toggleVisibility(point);
                                        },
                                        onLegendClick: function (e) {
                                            var arg = e.target;
                                            toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
                                        }
                                        ,
                                        smallValuesGrouping: {
                                            mode: "smallValueThreshold",
                                            threshold: 1
                                        },
                                        tooltip: {
                                            enabled: true,
                                            customizeTooltip: function (arg) {
                                                return {
                                                    text: arg.seriesName + " : " + arg.percentText + " - " + arg.valueText
                                                };
                                            }
                                        }


                                    }]


                                });

                                function toggleVisibility(item) {
                                    item.isVisible() ? item.hide() : item.show();
                                }

                            </script>
                        </div>

                        <div class="col-lg-6 col-md-12">

                            <div id="CasesTypeCount" style="width: 100%; height: 300px"></div>

                            <script>


                                var TicketTypes = [<%=_TicketTypesCount%>];
                                $("#CasesTypeCount").dxChart({
                                    dataSource: TicketTypes,
                                    commonSeriesSettings: {

                                        argumentField: "StatusName",
                                        type: "bar",
                                        label: {
                                            visible: true,
                                            format: "fixedPoint",
                                            precision: 0,
                                            alignment: 'center',
                                            position: 'outside',
                                            //rotationAngle: 90
                                        },
                                    },
                                    //rotated: true,
                                    series: [
                                        { valueField: "CasesCount", name: " ", color: '#5cb85c' }

                                    ],
                                    legend: {
                                        verticalAlignment: "bottom",
                                        horizontalAlignment: "center",
                                        itemTextPosition: "right",
                                        visible: false
                                    },
                                    title: {
                                        text: "عدد الإتفاقيات حسب النوع",
                                        font: { size: 20 }
                                    },

                                    argumentAxis: {
                                        tickInterval: 5,
                                        label: {
                                            overlappingBehavior: { mode: 'rotate', rotationAngle: 45 },
                                            font: { size: 15 }
                                        }
                                    },

                                    tooltip: {
                                        enabled: true,
                                        customizeTooltip: function (arg) {
                                            return {
                                                text: arg.seriesName + " : " + arg.valueText
                                            };
                                        }
                                    }
                                });

                            </script>
                        </div>
                        <div class="col-lg-6" style="display: none">
                            <div class="panel">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="table-responsive">

                                                <asp:DataGrid runat="server" ID="grdData" AutoGenerateColumns="False" PageSize="20" class="table text-nowrap" Visible="false">
                                                    <PagerStyle Visible="False" />
                                                    <HeaderStyle BackColor="#efefef" Font-Bold="True" />
                                                    <Columns>
                                                        <asp:BoundColumn DataField="code" Visible="False"></asp:BoundColumn>


                                                        <asp:BoundColumn DataField="CaseRefNum" HeaderText="Ref#"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TransDate" HeaderText="post Date" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="OwnerName" HeaderText="User Name"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="mobile" HeaderText="mobile"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Email" HeaderText="Email"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TypeNameEn" HeaderText="Type"></asp:BoundColumn>


                                                        <asp:TemplateColumn HeaderText="Status">

                                                            <ItemTemplate>
                                                                <a href="CaseStatusHistory.aspx?caseID=<%#Eval("code") %>" class="iframe"><%#Eval("StatusNameEn") %></a>
                                                                <%--      <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.StatusNameEn") %>'></asp:Label>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>


                                                        <asp:TemplateColumn HeaderText="Details">
                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>

                                                                <asp:LinkButton runat="server" ID="lnkEdit" CommandName="Edit" class="btn btn-default btn-xs">
																					 <i class="fa fa-edit"></i>&nbsp;
																					Details
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>


                                                    </Columns>
                                                </asp:DataGrid>




                                                <table class="table text-nowrap">
                                                    <thead>
                                                        <tr>
                                                            <th><%=Resources.CTOS.Ticket %></th>
                                                            <th class="col-md-2"><%=Resources.CTOS.Email %></th>
                                                            <th class="col-md-2"><%=Resources.CTOS.OwnerName %></th>
                                                            <th class="col-md-2"><%=Resources.CTOS.mobile %></th>
                                                            <th class="col-md-2"><%=Resources.CTOS.Quick %></th>
                                                            <th class="text-center" style="width: 20px;"><i class="icon-arrow-down12"></i></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <%-- <%=FillGrid() %>--%>
                                                    </tbody>
                                                </table>
                                            </div>

                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>



                    </div>
                </div>
            </div>
        </div>

    </div>
    <%} %>


    <%--Medal --%>
    <div style="display:none">
    <% if (ShowSystem("4")) 
        { %>
    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-8 col-xs-12">
                <div class="page-title2">
                    <h4>
                        <i class="icon-grid position-left"></i>
                        <%=Resources.CTOS.MedalDashboard %>

                    </h4>
                </div>

            </div>

            <div class="clearfix"></div>
        </div>
    </div>
    <!-- Quick stats boxes -->
    <div class="row">

        <div class="col-lg-2 col-xs-12">

            <!-- Today's revenue -->
            <div class="panel bg-soft-success">
                <div class="panel-body">
                    <div class="heading-elements icon-office" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Medals/Forms/MedalsData.aspx?ProTypeID=1" style="color: #000"><%=_TotalMedalCompleteData%></a></h3>
                    <%=Resources.CTOS._TotalMedalCompleteData %>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalMedalCompleteDataStr %>  </div>

                </div>


            </div>
            <!-- /today's revenue -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-info">
                <div class="panel-body">
                    <div class="heading-elements icon-alarm-check" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Medals/Forms/MedalsData.aspx?ProTypeID=2" style="color: #000"><%=_TotalMedalToMinister%></a> </h3>
                    <b><%=Resources.CTOS._TotalMedalToMinister %></b>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalMedalToMinisterStr %> </div>

                </div>


            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-2 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-danger">
                <div class="panel-body">
                    <div class="heading-elements icon-stamp" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Medals/Forms/MedalsData.aspx?ProTypeID=3" style="color: #000"><%=_TotalMedalToDewan%></a> </h3>
                    <%=Resources.CTOS._TotalMedalToDewan %>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS._TotalMedalToDewanStr%> </div>

                </div>

            </div>
            <!-- /current server load -->

        </div>

        <div class="col-lg-3 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-secondary">
                <div class="panel-body">
                    <div class="heading-elements icon-file-check2" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Medals/Forms/MedalsData.aspx?ProTypeID=4" style="color: #000"><%=_TotalMedalSader%></a> </h3>
                    <%=Resources.CTOS.Medalsader %>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS.MedalsaderStr%> </div>

                </div>

            </div>
            <!-- /current server load -->

        </div>
        <div class="col-lg-3 col-xs-12">

            <!-- Current server load -->
            <div class="panel bg-soft-warning">
                <div class="panel-body">
                    <div class="heading-elements icon-file-eye2" style="font-size: 30px">
                    </div>

                    <h3 class="no-margin"><a href="/Modules/Medals/Forms/MedalsData.aspx?ProTypeID=5" style="color: #000"><%=_TotalMedalEkhtar%></a> </h3>
                    <%=Resources.CTOS.MedalEkhtar %>
                    <div class="text-muted text-size-mini"><%=Resources.CTOS.MedalEkhtarStr%> </div>

                </div>

            </div>
            <!-- /current server load -->

        </div>
    </div>
    <div class="row mbl">
        <div class="col-lg-12">
            <div class="panel">


                <div class="panel-body">

                    <div class="col-lg-6 col-md-12" style="display:none">

                        <div id="MedalOrgPendingProcedures" style="width: 100%; height: 300px"></div>

                        <script>
                            var MedalStatusTypes = [<%=_MedalStatusTypes%>];

                            $("#MedalOrgPendingProcedures").dxPieChart({
                                dataSource: MedalStatusTypes,
                                title: "<%=Resources.CTOS.MedalStatus%>",
                                palette: ['#7CBAB4', '#92C7E2', '#75B5D6', '#B78C9B', '#F2CA84', '#A7CA74'],
                                legend: {
                                    horizontalAlignment: "center",
                                    verticalAlignment: "bottom"
                                },
                                series: [{
                                    argumentField: "StatusName",
                                    valueField: "CasesCount",
                                    label: {
                                        visible: true,
                                        connector: {
                                            visible: true,
                                            width: 0.5
                                        },
                                        format: "fixedPoint",
                                        customizeText: function (point) {
                                            return point.argumentText + ":" + "(" + point.percentText + ") " + point.valueText;
                                        }
                                    },
                                    //   palette: ["#00ced1", "#008000", "#ffd700", "#ff7f50"],
                                    // palette: ['#7CBAB4', '#92C7E2', '#75B5D6', '#B78C9B', '#F2CA84', '#A7CA74'],
                                    onPointClick: function (e) {
                                        var point = e.target;
                                        toggleVisibility(point);
                                    },
                                    onLegendClick: function (e) {
                                        var arg = e.target;
                                        toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
                                    }
                                    ,
                                    //smallValuesGrouping: {
                                    //    mode: "smallValueThreshold",
                                    //    threshold: 2
                                    //},
                                    tooltip: {
                                        enabled: true,
                                        customizeTooltip: function (arg) {
                                            return {
                                                text: arg.seriesName + " : " + arg.percentText + " - " + arg.valueText
                                            };
                                        }
                                    }


                                }]


                            });

                            function toggleVisibility(item) {
                                item.isVisible() ? item.hide() : item.show();
                            }

                        </script>

                        <%--  <script>
								var MedalOrgPendingProcedure = [<%=_MedalOrgPendingProcedure%>];
								$("#MedalOrgPendingProcedures").dxChart({
									rotated: true,
									dataSource: MedalOrgPendingProcedure,
									commonSeriesSettings: {
										argumentField: "OrgName",
										type: "StackedBar"
									},
									series: [

										{ valueField: "MedalCount_ToOrg", name: "إعادة للجهة لاستكمال البيانات", color: '#02b6ad' },
										{ valueField: "MedalCount_ToMinister", name: "التحويل إلى وزير الدوله", color: '#ffd365' },
										{ valueField: "MedalCount_ToDewan", name: "التحويل إلى وزير الديوان", color: '#b34446' }
									],
									palette: ["#00ced1", "#008000", "#ffd700", "#ff7f50"],
									onSeriesClick: function (e) {
										var series = e.target;
										series.isVisible() ? series.hide() : series.show();
									},
									legend: {
										verticalAlignment: "top",
										horizontalAlignment: "center",
										itemTextPosition: "right"
									},
									title: {
										title: "<%=Resources.CTOS.MedalOrgTypes%>",
										font: { size: 20 },
										subtitle: { text: "" }
									},

									argumentAxis: {
										tickInterval: 5,
										label: {
											overlappingBehavior: { mode: 'rotate', rotationAngle: 45 },
											font: { size: 12 }
										}
									},
									tooltip: {
										enabled: true,
										customizeTooltip: function (arg) {
											return {
												text: arg.seriesName + " : " + arg.percentText + " - " + arg.valueText
											};
										}
									}
								});
							</script>--%>
                    </div>



                    <div class="col-lg-12 col-md-12">

                        <div id="MedalOrgCount" style="width: 100%; height: 300px"></div>


                        <script>

                            var MedalOrgTypes = [<%=_MedalOrgTypes%>];

                            $("#MedalOrgCount").dxPieChart({
                                dataSource: MedalOrgTypes,
                                title: "<%=Resources.CTOS.MedalOrgTypes%>",
                                //  palette: ['#0072C6', 'OrangeRed', 'Orange', '#B78C9B', '#F2CA84', '#A7CA74'],
                                palette: "Pastel",
                                legend: {
                                    horizontalAlignment: "center",
                                    verticalAlignment: "bottom"
                                },
                                series: [{
                                    argumentField: "StatusName",
                                    valueField: "CasesCount",
                                    label: {
                                        visible: true,
                                        connector: {
                                            visible: true,
                                            width: 0.5
                                        },
                                        format: "fixedPoint",
                                        customizeText: function (point) {
                                            return point.argumentText + ":" + "(" + point.percentText + ") " + point.valueText;
                                        }
                                    },
                                    //  palette: ["#00ced1", "#008000", "#ffd700", "#ff7f50"],

                                    onPointClick: function (e) {
                                        var point = e.target;
                                        toggleVisibility(point);
                                    },
                                    onLegendClick: function (e) {
                                        var arg = e.target;
                                        toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
                                    }
                                    ,
                                    //smallValuesGrouping: {
                                    //    mode: "smallValueThreshold",
                                    //    threshold: 2
                                    //},
                                    tooltip: {
                                        enabled: true,
                                        customizeTooltip: function (arg) {
                                            return {
                                                text: arg.seriesName + " : " + arg.percentText + " - " + arg.valueText
                                            };
                                        }
                                    }


                                }]


                            });

                            function toggleVisibility(item) {
                                item.isVisible() ? item.hide() : item.show();
                            }

                        </script>





                    </div>




                </div>
            </div>
        </div>
    </div>

    <%} %>
        </div>
    <asp:HiddenField runat="server" ID="SelectedDate" ClientIDMode="Static" />
</asp:Content>
