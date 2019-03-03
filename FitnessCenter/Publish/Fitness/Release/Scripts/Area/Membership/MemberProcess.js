var memberprocess = function () {
    return {
        run: function () {
            MWizard.init($('#form_wizard_Transfer'), $('#formTransfer'));
            MWizard.init($('#form_wizard_1'), $('#formUpgrade'));
            var spinn = $('#TotalMonthFreeze');
            $('input[name="MemberType.DiscountPct"]')
                .keyup(function (e) {
                    if (/\D/g.test(this.value)) {
                        // Filter non-digits from input value.
                        this.value = this.value.replace(/\D/g, "");
                    }
                });
            var select2 = select2Control;
            const memberno = $("#Membership_tMember_MemberNO");

            select2.init("#Membership_tMember_MemberNO", "Member",
                memberno.data("url"),
                memberno.data("urllookup"),
                function (res) {
                    return `<div class="row">
                        <div class ="col-md-4">${res.id}</div>
                        <div class ="col-md-4">${res.note[1]}</div>
                        <div class ="col-md-4">${res.note[0]}</div></div>`;
                });

            //Initalize Payment Table
            var dtPaymentDetail = $("#dtPaymentDetail")
                .DataTable({
                    "serverSide": false,
                    "processing": true,
                    "autoWidth": false,
                    "deferRender": true,
                    "filter": true,
                    "lengthMenu": [5, 10, 25, 50, 75, 100],
                    "pageLength": 10,
                    "columns": [
                        {
                            "visible": false,
                        },
                        {
                            "orderable": false,
                        },
                        {
                            "orderable": false,
                        },
                        {
                            "orderable": false,
                        },
                        {
                            "orderable": false,
                        },
                        {
                            "orderable": false,
                        },
                        {
                            "orderable": false,
                        },
                        {
                            "orderable": false,
                        },
                        {
                            "orderable": false,
                        },
                        {
                            "orderable": false,
                        },
                        {
                            "orderable": false,
                        }
                    ],
                    "order": [[0, "asc"]]
                });

            select2.init("#MemberType_MemberType",
                "Tipe Member",
                "/Office/MemberType/GetMemberType",
                "/Office/MemberType/GetMemberTypeById");

            function resetSpinVal() {
                spinn.val(1);
                spinn.trigger("touchspin.updatesettings", { max: 10 });
            }

            function setOnChange() {
                resetSpinVal();
            }


            function setMask(lookUp) {
                $(lookUp)
                    .inputmask("decimal",
                    {
                        radixPoint: ",",
                        groupSeparator: ".",
                        autoGroup: true,
                        digits: 2,
                        digitsOptional: true
                    });
            }

            function getMaskVal(lookUp) {
                const val = $(lookUp).inputmask("unmaskedvalue");
                if (Globalize.locale() === undefined) {
                    return parseFloat(val === "" ? "0" : val);
                } else {
                    return Globalize.parseNumber((val === "" ? "0" : val));
                }
            }

            function reInitializeMaskEdit() {
                $('.mask-date').datepicker({
                    format: 'dd/mm/yyyy',
                    startDate: '+0d'
                });

                $(".mask-date").inputmask("dd/mm/yyyy",
                {
                    autoUnmask: false
                }); //direct mask
            }

            function reAmountMemberType() {
                const payment = getMaskVal('#MemberType_Payment');
                const discountPct = $('#MemberType_DiscountPct').val();
                const currMemberVal = getMaskVal('#Membership_Total');
                var totalAmount = 0;
                var discount = 0;
                var totalNewMembershipMustPay = 0;
                totalAmount = (payment);

                discount = totalAmount * discountPct / 100;

                totalAmount = totalAmount - discount;
                totalNewMembershipMustPay = totalAmount - currMemberVal;

                document.getElementById("DiscountMemberHelp")
                    .innerHTML = `Discount Total for this account (${discount.formatMoney(2, ",", ".")})`;
                $("#MemberType_Discount").val(discount);

                $("#MemberType_TotalAmount").val(totalAmount);

                $("#TotalNewMemberMustPay").val(totalNewMembershipMustPay);

            }

            //#region function2
            function reAmountDetail() {
                //$('.autoDetail').autoNumeric('init', { aSep: '.', aDec: ',' });

                setMask(".autoDetail");

                var totalSummaryPaymentList = 0;
                var totalOverPaymentDetail = 0;

                for (let i = 0; i <= dtPaymentDetail.rows().data().length - 1; i++) {
                    const currentRow = dtPaymentDetail.row(i);
                    totalSummaryPaymentList += getMaskVal(currentRow.nodes().to$().find(".paymentamountdetail"));
                }

                var total = getMaskVal('#TotalNewMemberMustPay');

                total -= totalSummaryPaymentList;
                if (total < 0) {
                    totalOverPaymentDetail = total * -1;
                    total = 0;
                }

                $("#TotalPaymentMustPay").val(total);
                $("#TotalSumPayment").val(totalSummaryPaymentList);
                $("#totalOverPaymentDetail").val(totalOverPaymentDetail);

            }

            function addGroupDetailPayment(paymentType, amount) {
                $.ajax({
                    url: "/Membership/FastMembership/AddDataPaymentDetail",
                    data: {
                        amount: amount,
                        paymentType: paymentType,
                        memberId: "",
                        membershipId: ""
                    }
                })
                    .done(function (row) {
                        dtPaymentDetail.row.add($(row)).draw();
                        setMask(".autoDetail");
                        reAmountDetail();

                    });
            };


            function reAmount() {
                //var discount = 0;
                var total = getMaskVal('#TotalNewMemberMustPay'); //$('#TotalNewMemberMustPay').val();

                var totalSumPayment = 0;
                dtPaymentDetail.clear().draw();
                addGroupDetailPayment('Cash', total);

                $('#TotalSumPayment').val(totalSumPayment);

            }

            function renewFreeze(month) {
                const startFreezeDate = $('#StartDate').val();

                $("#EndDate").val(moment(startFreezeDate, 'DD/MM/YYYY').add(month, 'months').format("DD/MM/YYYY"));
                const freezePrice = Globalize.parseNumber($('#FreezePrice').val());
                $('#TotalFreezePrice').val((freezePrice * month));
            }

            function reAmountPpt() {
                const subtotal = getMaskVal('#PT_PTSubtotal');
                const diskon = $('#DiscountPct').val();
                var totalAmount = 0;
                var discount = 0;

                totalAmount = subtotal;
                discount = totalAmount * diskon / 100;

                totalAmount = totalAmount - discount;

                document
                    .getElementById('DiscountMemberHelp')
                    .innerHTML = 'Discount Total for this account (' + discount.formatMoney(2, ',', '.') + ')';
                //$('#PT_PTTotal').autoNumeric('set', totalAmount);


                $('#PT_PTTotal').val(totalAmount);
                $('#PT_PTDiskon').val(discount);

            }


            $("#MemberType_MemberType").on("change", function () {
                var typeId = select2.select("#MemberType_MemberType").id;
                $.ajax({
                    url: "/Office/MemberType/GetMemberSharedById",
                    dataType: "json",
                    data: {
                        id: typeId
                    }
                }).done(function (data) {
                    $("#MemberType_Payment").val(data.Biaya);
                    if (data.IsPaket !== true) {
                        $("#hdnMemberProrate").addClass("hidden");
                        $("#MemberType_Prorate").val(0);
                    } else {
                        $("#hdnMemberProrate").removeClass("hidden");
                        $("#MemberType_Prorate").val(data.Prorate);
                    }

                    $("#MemberType_MemberTypeDef").val(data.MemberType);
                    $("#MemberType_Prefix").val(data.prFix);
                    $("#totalMonth").val(data.JmlBulan);
                    $("#MasterPayment_TotalMonth").val(data.JmlBulan);
                    reAmountMemberType();
                    reAmount();
                });
            });

            $('.autoPPT, #DiscountPct').on('change', function () {
                reAmountPpt();
            });

            $('.auto').on('change', function () {
                reAmount();
            });

            $('#PT_tPaketPTID').on('change', function () {
                var paketId = $(this).select2('val');
                $.ajax({
                    url: '/Office/PaketPT/GetPaketPTSharedById',
                    dataType: 'json',
                    data: {
                        id: paketId
                    }
                })
                    .done(function (data) {
                        $('#PT_PTSubtotal').val(data.PPTHarga);
                        $('#PT_SisaJam').val(data.PPTJam);
                        $('#PT_PTTotal').val(data.PPTHarga);
                    });
            });

            //Event Payment
            $("#dtPaymentDetail tbody")
                .on('click',
                    '.delPaymentList',
                    function () {
                        dtPaymentDetail.row($(this).parents('tr')).remove().draw();
                        reAmountDetail();
                    });


            $(".autoMemberType, #MemberType_DiscountPct")
                .on('change',
                    function () {
                        reAmountMemberType();
                    });

            //event add Payment Detail
            $(".btnAddPaymentList")
                .on('click',
                    function (ev) {
                        $.ajax({
                            url: $(this).data("url"),
                            data: {
                                amount: 0,
                                paymentType: 'Cash',
                                memberId: "",
                                membershipId: ""
                            }
                        })
                            .done(function (template) {
                                dtPaymentDetail.row.add($(template)).draw();
                                setMask(".autoDetail");
                                reInitializeMaskEdit();

                                Metronic.init();
                            });
                    });

            $(window)
                .load(function () {
                    spinn.TouchSpin({
                        min: 1,
                        max: 10,
                        mousewheel: true,
                        stepinterval: 50,
                        maxboostedstep: 10000000
                    });

                    setOnChange();
                    setMask(".autoMemberType");
                    setMask(".auto");
                    setMask(".autoFreeze");
                });

            $("#TotalMonthFreeze").on('change', function () {
                renewFreeze($(this).val());
            });

            $(document).on('change', '.paymentDetailType', function () {
                const payType = $(this).find('option:selected').text();
                console.log(payType);
                if (payType !== "Cash") {
                    $(this).closest("tr").find('.paymentDetailBank').removeClass('disabled');
                    $(this).closest("tr").find('.paidAmountDetail').removeClass('disabled');

                    $(this).closest("tr").find('.paymentDetailBank').prop("disabled", false);

                    $(this).closest("tr").find('.paidAmountDetail').prop('readonly', true);

                } else {
                    if (!$(this).closest("tr").find('.paymentDetailBank').hasClass('disabled')) {
                        $(this).closest("tr").find('.paymentDetailBank').addClass('disabled');
                        $(this).closest("tr").find('.paidAmountDetail').addClass('disabled');
                    }

                    $(this).closest("tr").find('.paidAmountDetail').prop("readonly", false);
                    $(this).closest("tr").find('.paymentDetailBank').prop("disabled", true);


                    $(this).closest("tr").find('.paymentDetailBank').val("");
                }
            });

            $(document).on("change", ".autoDetail", function () {
                reAmountDetail();
            });

            $("#Membership_tMember_MemberNO").on("change", function () {
                $.ajax({
                    url: $(this).data("urlsingle"),
                    data: {
                        memberNo: $(this).val()
                    }
                }).done(function (data) {
                    $("#Person_PNama").val(data.nama);
                    $("#Person_PGender").val(data.gender);
                    $("#Person_PIdentitas").val(data.identitas);
                    $("#Person_PPropinsi").val(data.propinsi);
                    $("#Person_PKota").val(data.kota);
                    $("#Person_PKelurahan").val(data.kelurahan);
                    $("#Person_PKecamatan").val(data.kecamatan);
                    $("#Person_PAlamat").val(data.alamat);
                    $("#Person_PHP1").val(data.hp1);
                    $("#Person_PHP2").val(data.hp2);
                    $("#Person_PPinBB").val(data.pinbb);
                    $("#Person_PEmail").val(data.email);

                    const date = data.tgllahir.split("-");
                    $(".PTglLahir").find(".thn").val(date[0]);
                    $(".PTglLahir").find(".bln").val(date[1]);
                    $(".PTglLahir").find(".tgl").val(date[2]);
                });
            });

            $("a[data-toggle='tab']").on("shown.bs.tab", function () {
                if ($("#liupgrade2").hasClass("active")) {
                    reAmountDetail();
                }
            });

        }
    }
}();