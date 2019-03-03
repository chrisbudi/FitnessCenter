// ================================================================
//  Description: Avatar Upload supporting script
//  License:     MIT - check License.txt file for details
//  Author:      Codative Corp. (https://www.codative.com/)
// ================================================================
var jcrop_api,
    boundx,
    boundy,
    xsize,
    ysize;


function initAvatarUpload() {
    $({
        beforeSend: function () {
            //updateProgress(0);
            $('#avatar-upload-form').addClass('hidden');
        },
        success: function (data) {
            //updateProgress(100);
            if (data.success === false) {
                $('#status').html(data.errorMessage);
            } else {
                $('#preview-pane .preview-container img').attr('src', data.fileName);

                var img = $('#CaptureImage');
                img.attr('src', data.fileName);

                $('#avatar-upload-box').addClass('hidden'); // ToDo - Remove if you want to keep the upload box
                $('#avatar-crop-box').removeClass('hidden');

                initAvatarCrop(img);
            }
        },
        complete: function (xhr) {
        }
    });
}

function initAvatarCrop(img) {
    $('#preview-pane .preview-container img').attr('src', img.src);

    img.Jcrop({
        onChange: updatePreviewPane,
        onSelect: updatePreviewPane,
        aspectRatio: xsize / ysize,
        minSize: [200, 200]
    }, function () {
        var bounds = this.getBounds();
        boundx = bounds[0];
        boundy = bounds[1];

        jcrop_api = this;
        jcrop_api.animateTo([100, 100, 300, 300]);
        jcrop_api.setOptions({ allowSelect: true });
        jcrop_api.setOptions({ allowMove: true });
        jcrop_api.setOptions({ allowResize: true });
        jcrop_api.setOptions({ aspectRatio: 1 });

        var pcnt = $('#preview-pane .preview-container');
        xsize = pcnt.width();
        ysize = pcnt.height();

        $('#preview-pane').appendTo(jcrop_api.ui.holder);


        jcrop_api.focus();
    });
}

function updatePreviewPane(c) {
    if (parseInt(c.w) > 0) {
        var rx = xsize / c.w;
        var ry = ysize / c.h;

        $('#preview-pane .preview-container img').css({
            width: Math.round(rx * boundx) + 'px',
            height: Math.round(ry * boundy) + 'px',
            marginLeft: '-' + Math.round(rx * c.x) + 'px',
            marginTop: '-' + Math.round(ry * c.y) + 'px'
        });
    }
}

function saveAvatar(imageId, imageSource) {
    var img = $('#preview-pane .preview-container img');
    $('#avatar-crop-box button').addClass('disabled');
    $.ajax({
        type: "POST",
        url: "/Tools/Camera/CaptureImage",
        dataType: 'json',
        data: {
            w: 350,
            h: 270,
            l: img.css('marginLeft'),
            t: img.css('marginTop'),
            sw: img.css('width'),
            sh: img.css('height'),
            imageSource: img.attr('src')
        },
        success: function (data) {
            $(imageId).attr('src', data.src);
            $(imageSource).attr('value', data.src);
        },
        error: function () {
            alert("error program show !");
        }
    });
}