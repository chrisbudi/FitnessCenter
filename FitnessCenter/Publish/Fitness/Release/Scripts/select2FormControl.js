var select2Control = function () {
    return {
        init: function (lookupCode, placeHolder, urlFetch, urlLookUp, selectFormat, id) {
            var pageSize = 20;
            $(lookupCode).select2({
                placeholder: placeHolder,
                minimumInputLength: 0,
                multiple: false,
                quietMillis: 400,
                allowClear: true,
                dropdownAutoWidth: true,
                ajax: {
                    url: urlFetch,
                    dataType: "json",
                    type: "POST",
                    data: function (term, page) {
                        return {
                            pageSize: pageSize,
                            pageNum: page,
                            searchTerm: term
                        };
                    },
                    results: function (data, page) {
//                        console.log("init selection data", data);
                        const more = (page * pageSize) < data.Total;
                        return {
                            results: data.Results,
                            more: more
                        };
                    }
                },
                initSelection: function (item, callback) {
                    if (urlLookUp !== "")
                        if (item.val() !== "0") {
                            $.ajax({
                                url: urlLookUp,
                                data: {
                                    id: item.val()
                                }
                            }).done(function (data) {
//                                console.log("init selecttion data", data.Results[0]);
                                if (data.Results[0] !== undefined) {
                                    var obj = { id: data.Results[0].id, text: data.Results[0].text };
                                    console.log(obj);
                                    callback(obj);
                                } else {
                                    console.log(data.Results[0]);
                                }
                                //                                item.val(obj.id);

                            });
                        }
                },
                formatResult: function (item) {
//                    console.log(selectFormat);
                    if (selectFormat != undefined) {
                        return selectFormat(item);
                    }

                    return (`<div>${item.id} - ${item.text}</div>`);
                },
                formatSelection: function (item) {
                    if (jQuery.isEmptyObject(item)) {
                        return ("");
                    }
                    //if (lookupDescription) {
                    //    $(lookupDescription).val(item.text);
                    //}
                    return (item.text);
                },
                dropdownCssClass: "bigdrop",
                escapeMarkup: function (m) { return m; }
            }).on("change", function (e) {
                //if ((!jQuery.isEmptyObject(e.removed) && (!e.added)) && (lookupDescription)) {
                //    $(lookupDescription).val('');
                //}
            });
        },
        readonly: function (lookupCode, readonlyStatus) {
            $(lookupCode).attr('readonly', readonlyStatus);
        },
        clear: function (lookupCode) {
            $(lookupCode).select2("data", { id: "0", text: "" });
        },
        select: function (lookupCode) {
            return $(lookupCode).select2('data');
        }
    }
}();