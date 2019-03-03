var pageReporting = function () {
    return {

        //Script untuk Link Auth
        init: function (url, currentUrl, title, modalLink) {
            $(modalLink).on('click', function (e) {
                if ($(this).hasClass('disabled')) {
                    return false;
                }

                e.preventDefault();
                $(this).attr('data-target', '#staticReport');
                $(this).attr('data-toggle', 'modal');
                $.ajax({
                    url: url,
                    //contentType: 'application/html',
                    type: 'GET',
                    //dataType: 'html',
                    data: {
                        link: currentUrl,
                        title : title
                    }
                }).success(function (result) {
                    $('.modal-content-Report').html(result);
                });
            });

            $('body').on('click', '.modal-close-btn', function () {
                $('#staticReport').modal('hide');
            });

            $('#staticReport').on('hidden.bs.modal', function () {
                $(this).removeData('bs.modal');
            });

            $('#CancelModal').on('click', function () {
                return false;
            });

            var toggle = $('#staticReport');
            $('#btn-default').click(function () {
                toggle.modal('hide');
            });

        }
    }
}();
// End  Script untuk Link Auth