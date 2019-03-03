var MWizard = function () {
    return {

        initializeValidation: function () {
        },
        //main function to initiate the module
        init: function (wizardId, formId) {
            if (!jQuery().bootstrapWizard) {
                return;
            }

            function format(state) {
                if (!state.id) return state.text; // optgroup
                return "<img class='flag' src='../../assets/global/img/flags/" + state.id.toLowerCase() + ".png'/>&nbsp;&nbsp;" + state.text;
            }

            var form = formId;//$('#submit_form');
            var error = $('.alert-danger', form);
            var success = $('.alert-success', form);

            form.validate({
                doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
                errorElement: 'span', //default input error message container
                errorClass: 'help-block help-block-error', // default input error message class
                focusInvalid: false, // do not focus the last invalid input

                messages: {

                },

                errorPlacement: function ($errorLabel, $element) { // render error placement for each input type


                    var $elementToInsertAfter = $element;
                    if ($element.prop("type") === "radio") {
                        $elementToInsertAfter = $element.closest(".controls");
                    }

                    $errorLabel.insertAfter($elementToInsertAfter);

                },
                invalidHandler: function (event, validator) { //display error alert on form submit
                    success.hide();
                    error.show();
                    Metronic.scrollTo(error, -200);
                },
                highlight: function (element) { // hightlight error inputs
                    $(element)
                        .closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
                },
                unhighlight: function (element) { // revert the change done by hightlight
                    $(element)
                        .closest('.form-group').removeClass('has-error'); // set error class to the control group
                },
                success: function (label) {
                    if (label.attr("for") === "gender") { // for checkboxes and radio buttons, no need to show OK icon
                        label.closest('.form-group').removeClass('has-error').addClass('has-success');
                        label.remove(); // remove error label here
                    } else { // display success icon for other inputs
                        label
                            .addClass('valid') // mark the current input as valid and display OK icon
                            .closest('.form-group').removeClass('has-error').addClass('has-success'); // set success class to the control group
                    }
                },
                submitHandler: function (form) {
                    success.show();
                    error.hide();
                    //add here some ajax code to submit your form or just call form.submit() if you want to submit the form without ajax
                    $.ajax({
                        cache: false,
                        async: true,
                        type: "POST",
                        url: form.attr('action'),
                        data: form.serialize(),
                    });
                }

            });


            var displayConfirm = function () {
                $('#tab4 .form-control-static', form).each(function () {
                    var input = $('[name="' + $(this).attr("data-display") + '"]', form);
                    if (input.is(":radio")) {
                        input = $('[name="' + $(this).attr("data-display") + '"]:checked', form);
                    }
                    if (input.is(":text") || input.is("textarea")) {
                        $(this).html(input.val());
                    } else if (input.is("select")) {
                        $(this).html(input.find('option:selected').text());
                    } else if (input.is(":radio") && input.is(":checked")) {
                        $(this).html(input.attr("data-title"));
                    } else if ($(this).attr("data-display") == 'payment') {
                        var payment = [];
                        $('[name="payment[]"]:checked').each(function () {
                            payment.push($(this).attr('data-title'));
                        });
                        $(this).html(payment.join("<br>"));
                    }
                });
            }

            var handleTitle = function (tab, navigation, index) {


                var total = navigation.find('li').length;
                var current = index + 1;
                // set wizard title
                $('.step-title', wizardId).text('Step ' + (index + 1) + ' of ' + total);
                // set done steps
                jQuery('li', wizardId).removeClass("done");
                var liList = navigation.find('li');
                for (var i = 0; i < index; i++) {
                    jQuery(liList[i]).addClass("done");
                }

                if (current == 1) {
                    wizardId.find('.button-previous').hide();
                } else {
                    wizardId.find('.button-previous').show();
                }

                if (current >= total) {
                    wizardId.find('.button-next').hide();
                    wizardId.find('.button-submit').show();
                    displayConfirm();
                } else {
                    wizardId.find('.button-next').show();
                    wizardId.find('.button-submit').hide();
                }
                Metronic.scrollTo($('.page-title'));
            }
            var validationRadioButton = function() {

                //function domAttrModified(obj) {
                //    //$obj = $(obj);
                //    $(obj).bind('DOMAttrModified', function () {
                //        if ($(obj).attr('class').indexOf('input-validation-error') != -1)
                //            $(obj).parent().addClass('input-validation-error');
                //        else
                //            $(obj).parent().removeClass('input-validation-error');
                //    });
                //}

                //$(function () {
                //    $('input[type=radio]').each(function () {
                //        domAttrModified(this);
                //    });
                //});
            }

            // default form wizard
            wizardId.bootstrapWizard({
                'nextSelector': '.button-next',
                'previousSelector': '.button-previous',
                onTabClick: function (tab, navigation, index, clickedIndex) {
                    success.hide();
                    error.hide();
                    validationRadioButton();
                    if (form.valid() === false) {
                        return false;
                    }
                    handleTitle(tab, navigation, clickedIndex);
                },
                onNext: function (tab, navigation, index) {
                    success.hide();
                    error.hide();
                    validationRadioButton();
                    if (form.valid() === false) {
                        return false;
                    }

                    handleTitle(tab, navigation, index);
                },
                onPrevious: function (tab, navigation, index) {
                    success.hide();
                    error.hide();

                    handleTitle(tab, navigation, index);
                },
                onTabShow: function (tab, navigation, index) {
                    var total = navigation.find('li').length;
                    var current = index + 1;
                    var $percent = (current / total) * 100;
                    wizardId.find('.progress-bar').css({
                        width: $percent + '%'
                    });
                }
            });

            wizardId.find('.button-previous').hide();
            wizardId.find('.button-submit').click(function () {
                //a lert('Finished! Hope you like it :)');
            }).hide();
        }

    };

}();