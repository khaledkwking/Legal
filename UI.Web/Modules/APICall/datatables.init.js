 

function AddTHEAD(tableName) {
    var table = document.getElementById(tableName);
   // alert(table);
    if (table != null) {
        var head = document.createElement("thead");
       // head.style.display = "table-header-group";
        head.appendChild(table.rows[0]);
        table.insertBefore(head, table.childNodes[0]);
    }
}


$(function () {


    // Table setup
    // ------------------------------

    // Setting datatable defaults
    $.extend($.fn.dataTable.defaults, {
        autoWidth: false,
        responsive: true,
        columnDefs: [{
            orderable: false,
            width: '100px',
            targets: [5]
        }],
        dom: '<"datatable-header"fl><"datatable-scroll-wrap"t><"datatable-footer"ip>',

        language: {
            search: '<span>ÈÍË:</span> _INPUT_',
            lengthMenu: '<span>ÚÑÖ:</span> _MENU_',
            paginate: { 'first': 'First', 'last': 'Last', 'next': '&larr;', 'previous': '&rarr;' }
        },
        drawCallback: function () {
            $(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').addClass('dropup');
        },
        preDrawCallback: function () {
            $(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').removeClass('dropup');
        }
    });


    var _customizeExcelOptions = function (xlsx) {
        var sheet = xlsx.xl.worksheets['sheet1.xml'];
        var numrows = 2;
        var clR = $('row', sheet);

        //update Row
        clR.each(function () {
            var attr = $(this).attr('r');
            var ind = parseInt(attr);
            ind = ind + numrows;
            $(this).attr("r", ind);
        });

        // Create row before data
        $('row c ', sheet).each(function () {
            var attr = $(this).attr('r');
            var pre = attr.substring(0, 1);
            var ind = parseInt(attr.substring(1, attr.length));
            ind = ind + numrows;
            $(this).attr("r", pre + ind);
        });

        function Addrow(index, data) {
            var msg = '<row r="' + index + '">'
            for (var i = 0; i < data.length; i++) {
                var key = data[i].key;
                var value = data[i].value;
                msg += '<c t="inlineStr" r="' + key + index + '">';
                msg += '<is>';
                msg += '<t>' + value + '</t>';
                msg += '</is>';
                msg += '</c>';
            }
            msg += '</row>';
            return msg;
        }


        //insert
        var r1 = Addrow(1, [{ key: 'C', value: "CMGS PORTAL" }]);

        sheet.childNodes[0].childNodes[1].innerHTML = r1 + sheet.childNodes[0].childNodes[1].innerHTML;

    }


    // Basic responsive configuration


    $('.datatable-responsive').each(function () {
        $(this).prepend('<thead></thead>')
        $(this).find('thead').append($(this).find("tr:eq(0)"));
    })


    $('.datatable-responsive').DataTable({

        responsive: true,
        //dom: "<'row''<'col-sm-4'B><'col-sm-1'l><'col-sm-7'f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        lengthChange: true,
        ordering: false,
        info: false,
        autoWidth: true,
        stateSave: true,

        colReorder: true,
        language: {
            processing: "<div class='overlay' >  <i class='fa fa-refresh fa-spin'></i> </div>"

        },
        processing: true,
        paging: true

    });

    // Column controlled child rows
    $('.datatable-responsive-column-controlled').DataTable({
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                className: 'control',
                orderable: false,
                targets: 0
            },
            {
                width: "100px",
                targets: [6]
            },
            {
                orderable: false,
                targets: [6]
            }
        ],
        order: [1, 'asc']
    });


    // Control position
    $('.datatable-responsive-control-right').DataTable({
        responsive: {
            details: {
                type: 'column',
                target: -1
            }
        },
        columnDefs: [
            {
                className: 'control',
                orderable: false,
                targets: -1
            },
            {
                width: "100px",
                targets: [5]
            },
            {
                orderable: false,
                targets: [5]
            }
        ]
    });


    // Whole row as a control
    $('.datatable-responsive-row-control').DataTable({
        responsive: {
            details: {
                type: 'column',
                target: 'tr'
            }
        },
        columnDefs: [
            {
                className: 'control',
                orderable: false,
                targets: 0
            },
            {
                width: "100px",
                targets: [6]
            },
            {
                orderable: false,
                targets: [6]
            }
        ],
        order: [1, 'asc']
    });



    // External table additions
    // ------------------------------

    // Add placeholder to the datatable filter option
    $('.dataTables_filter input[type=search]').attr('placeholder', 'Type to filter...');


    // Enable Select2 select for the length option
    $('.dataTables_length select').select2({
        minimumResultsForSearch: Infinity,
        width: 'auto'
    });

});



  
   


    

 