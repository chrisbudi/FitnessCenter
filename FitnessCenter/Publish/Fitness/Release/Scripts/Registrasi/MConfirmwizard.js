var MConfirmWizard = function () {

    return {
        //main function to initiate the module
        init: function (formwz, btnNext, btnPrev) {
            if (!jQuery().bootstrapWizard) {
                return;
            }

            var handleTitle = function (tab, navigation, index) {
                var total = navigation.find('li').length;
                var current = index + 1;
                // set wizard title
                $('.step-title', $('#' + formwz)).text('Step ' + (index + 1) + ' of ' + total);
                // set done steps
                jQuery('li', $('#' + formwz)).removeClass("done");
                var li_list = navigation.find('li');
                for (var i = 0; i < index; i++) {
                    jQuery(li_list[i]).addClass("done");
                }

                if (current == 1) {
                    $('#' + formwz).find('.' + btnPrev).hide();
                } else {
                    $('#' + formwz).find('.' + btnPrev).show();
                }

                if (current >= total) {
                    $('#' + formwz).find('.' + btnNext).hide();
                    //$('#' + formwz).find('.button-port-submit').show();
                } else {
                    $('#' + formwz).find('.' + btnNext).show();
                    //$('#' + formwz).find('.button-port-submit').hide();
                }
                Metronic.scrollTo($('.page-title'));
            }

            // default form wizard
            $('#' + formwz).bootstrapWizard({
                'nextSelector': '.' + btnNext,
                'previousSelector': '.' + btnPrev,
                onTabClick: function (tab, navigation, index, clickedIndex) {
                    handleTitle(tab, navigation, index);
                    return true;
                    /*
                    success.hide();
                    error.hide();
                    if (form.valid() == false) {
                        return false;
                    }
                    handleTitle(tab, navigation, clickedIndex);
                    */
                },
                onNext: function (tab, navigation, index) {
                    handleTitle(tab, navigation, index);
                },
                onPrevious: function (tab, navigation, index) {
                    handleTitle(tab, navigation, index);
                }
            });

            $('#' + formwz).find('.' + btnPrev).hide();
        }

    };

}();