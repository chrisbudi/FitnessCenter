var globalization = function() {
    function initGlobalization() {

        $.when(
                $.get("/Scripts/cldr/data/supplemental/likelySubtags.json"),
                $.get("/Scripts/cldr/data/supplemental/numberingSystems.json"),
                $.get("/Scripts/cldr/data/supplemental/timeData.json"),
                $.get("/Scripts/cldr/data/supplemental/weekData.json"),
                $.get("/Scripts/cldr/data/main/id/numbers.json"),
                $.get("/Scripts/cldr/data/main/id/ca-gregorian.json"),
                $.get("/Scripts/cldr/data/main/id/timeZoneNames.json")
            )
            .done(function(result1, result2, result3, result4, result5, result6, result7) {
                Globalize.load(result1[0]); //contains data of 1 executed request
                Globalize.load(result2[0]); //contains data of 2 executed request
                Globalize.load(result3[0]); //contains data of 3 executed request
                Globalize.load(result4[0]); //contains data of 4 executed request
                Globalize.load(result5[0]); //contains data of 5 executed request
                Globalize.load(result6[0]); //contains data of 6 executed request
                Globalize.load(result7[0]); //contains data of 7 executed request

                // set current language
                Globalize.locale("id");
//                var globFormatter = Globalize.numberFormatter({ maximumFractionDigits: 2 });
//                console.log(globFormatter(Math.PI));
//                console.log(Math.PI);
            });

    }

    return{
        init: function() {
            initGlobalization();
        }
    }
}();
//jQuery(document).ready(function () {
//    globalization.init(); // init metronic core componets
//});