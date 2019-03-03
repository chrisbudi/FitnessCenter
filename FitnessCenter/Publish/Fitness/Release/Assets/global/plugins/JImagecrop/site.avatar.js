﻿// ================================================================
//  Description: Avatar Upload supporting script
//  License:     MIT - check License.txt file for details
//  Author:      Codative Corp. (http://www.codative.com/)
// ================================================================
var jcrop_api,
    boundx,
    boundy,
    xsize,
    ysize;


function initAvatarUpload() {
    $({
        beforeSend: function () {
            updateProgress(0);
            $('#avatar-upload-form').addClass('hidden');
        },
        success: function (data) {
            updateProgress(100);
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

function updateProgress(percentComplete) {
    $('.upload-percent-bar').width(percentComplete + '%');
    $('.upload-percent-value').html(percentComplete + '%');
    if (percentComplete === 0) {
        $('#upload-status').empty();
        $('.upload-progress').removeClass('hidden');
    }
}

function initAvatarCrop(img) {
    $('#preview-pane .preview-container img').attr('src', img.src);

    img.Jcrop({
        onChange: updatePreviewPane,
        onSelect: updatePreviewPane,
        aspectRatio: xsize / ysize
    }, function () {
        var bounds = this.getBounds();
        boundx = bounds[0];
        boundy = bounds[1];

        jcrop_api = this;
        //jcrop_api.animateTo([100, 100, 300, 300]);
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

function saveAvatar(urlType) {
    var img = $('#preview-pane .preview-container img');
    //$("#imageToSave").attr("src", img.attr('src'));

    //var img = $("#canvas");
    $('#avatar-crop-box button').addClass('disabled');
    $.ajax({
        type: "POST",
        url: "/General/Foto/CaptureImage/" + urlType,
        dataType: 'json',
        data: {
            w: img.css('width'),
            h: img.css('height'),
            l: img.css('marginLeft'),
            t: img.css('marginTop'),
            imageSource: img.attr('src'),
            imageFileName: $("#personId").val()
        },
        success: function (data) {
            $('#imageToSave').attr('src', data.src);
        },
        error: function () {
            alert("error program show !");
        }
    });
}