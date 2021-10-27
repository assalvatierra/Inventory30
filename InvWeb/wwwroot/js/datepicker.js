// Daterangepicker
// Reference: https://www.daterangepicker.com


$(document).ready(() => {
    
        $('.timepicker').val(moment().format("hh:mm A"));
        $('.datetimepicker').val(moment().format("MMM DD YYYY HH:mm A"));
        $('.datepicker').val(moment().format("MMM DD YYYY"));
    
});


$(function () {
    $('.datepicker').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        minYear: 2000,
        maxYear: parseInt(moment().format('YYYY'), 20),
        locale: {
            format: 'MMM DD YYYY'
        }
    }, function (start, end, label) {

    });
});

$(function () {
    $('.datetimepicker').daterangepicker({
        timePicker: true,
        timePickerIncrement: 1,
        timePicker24Hour: false,
        singleDatePicker: true,
        showDropdowns: true,
        minYear: 2000,
        maxYear: parseInt(moment().format('YYYY'), 20),
        locale: {
            format: 'MMM DD YYYY hh:mm A'
        }
    }, function (start, end, label) {

    });
});

$(function () {

    $('.timepicker').daterangepicker({
        timePicker: true,
        singleDatePicker: true,
        timePicker24Hour: false,
        timePickerIncrement: 1,
        timePickerSeconds: false,
        locale: {
            format: 'hh:mm A'
        }
    }).on('show.daterangepicker', function (ev, picker) {
        picker.container.find(".calendar-table").hide();

    });

})