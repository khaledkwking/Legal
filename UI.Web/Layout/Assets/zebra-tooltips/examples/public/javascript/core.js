$(document).ready(function() {

    new $.Zebra_Tooltips($('.zebra_tips1'), {'position': 'right'});

    new $.Zebra_Tooltips($('.zebra_tips2'), {
        'background_color': '#C40000',
        'color':            '#FFF'
    });

    var zt = new $.Zebra_Tooltips($('.zebra_tips3'));
    zt.show($('.zebra_tips3'), true);

    new $.Zebra_Tooltips($('.zebra_tips4'), {
        'position': 'left'
    });

    new $.Zebra_Tooltips($('.zebra_tips5'), {
        'position': 'right'
    });

});
