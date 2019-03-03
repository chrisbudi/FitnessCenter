
(function ($) {
    $.validator.addMethod('date',
    function (value, element) {
        return true;
    });

    $.fn.getDateFromTrippleDdls = function () {
        var year = this.find('select:nth(2)').val();
        var month = this.find('select:nth(1)').val();
        var day = this.find('select:nth(0)').val();

        //alert(year + ' - ' + month + ' - ' + day);
        if (year === '' || month === '' || day === '') {
            return NaN;
        }

        var y = parseInt(year, 10);
        var m = parseInt(month, 10);
        var d = parseInt(day, 10);

        var date = new Date(y, m - 1, d);
        var isValidDate = date.getFullYear() === y && date.getMonth() + 1 === m && date.getDate() === d;
        if (isValidDate) {
            return date;
        }

        return NaN;
    };

    $.validator.unobtrusive.adapters.add('trippleddldate', [], function (options) {
        options.rules["trippleddldate"] = true;
        options.rules['trippleddldate'] = options.params;
        if (options.message) {
            options.messages['trippleddldate'] = options.message;
        }
    });

    $.validator.addMethod('trippleddldate', function (value, element, params) {
        var parent = $(element).closest('.trippleddldatetime');
        var date = parent.getDateFromTrippleDdls();
        //console.log(date);
        return !isNaN(date);
    }, '');

    $.validator.unobtrusive.adapters.add('minage', ['min'], function (options) {
        options.rules['minage'] = options.params;
        if (options.message) {
            options.messages['minage'] = options.message;
        }
    });

    $.validator.addMethod('minage', function (value, element, params) {
        var parent = $(element).closest('.trippleddldatetime');
        var birthDate = parent.getDateFromTrippleDdls();
        if (isNaN(birthDate)) {
            return false;
        }

        var today = new Date();
        var age = today.getFullYear() - birthDate.getFullYear();
        var m = today.getMonth() - birthDate.getMonth();
        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        return age >= parseInt(params.min, 10);
    }, '');

})(jQuery);