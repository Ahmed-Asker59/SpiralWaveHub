$(document).ready(function () {
    $('.js-datepicker').daterangepicker({
        singleDatePicker: true,
        maxDate: new Date()
    });
    $('#TestPic').on('change', function () {
        var selectedFile = $(this).val().split('\\').pop();
        var testContainer = $('#TestPicContainer');
        var img = window.URL.createObjectURL(this.files[0]);
        testContainer.removeClass('d-none');
        testContainer.find('img').attr('src', img);

    });
});