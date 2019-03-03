var pageCamera = function () {
    return {

        //Script untuk Link Auth
        init: function (modalLink, cameraObj, cameraSource) {
            $(modalLink).on('click', function (e) {
                if ($(this).hasClass('disabled')) {
                    return false;
                }

                e.preventDefault();
                $(this).attr('data-target', '#staticCamera');
                $(this).attr('data-toggle', 'modal');
                $.ajax({
                    url: '/Tools/Camera/Camera',
                    //contentType: 'application/html',
                    type: 'GET',
                    data: {
                        camera: cameraObj,
                        cameraSource: cameraSource
                    }
                }).success(function (result) {
                    $('.modal-content-camera').html(result);
                });
            });

            //$('body').on('click', '.modal-close-btn', function () {
            //    $('#staticCamera').modal('hide');
            //});

            //$('#staticCamera').on('hidden.bs.modal', function () {
            //    $(this).removeData('bs.modal');
            //});

            //$('#CancelModal').on('click', function () {
            //    return false;
            //});

            //var toggle = $('#staticCamera');
            //$('#btn-default').click(function () {
            //    toggle.modal('hide');
            //});

        }
    }
}();
// End  Script untuk Link Auth