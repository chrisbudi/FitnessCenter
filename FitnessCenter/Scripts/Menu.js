var menuLoad = function () {

    return {
        //Script untuk Link Auth
        init: function (controller, action, area, url) {

            var menuLoaded = false;
            $.ajax({
                url: '/Menu/SideBar',
                cache: false,
                data: {
                    controller: controller,
                    action: action,
                    url: url,
                    area: area
                },
                type: "GET",
                success: function (response, status, xhr) {
                    var nvContainer = $("#sidebar");
//                    console.log(response);
                    nvContainer.html(response);
                    menuLoaded = true;
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    var nvContainer = $("#sidebar");
                    nvContainer.html(errorThrown);
                }
            });

            //$.ajax({
            //    url: '/Menu/TopBar',
            //    data: {
            //        menu: controller,
            //        controller: controller
            //    },
            //    type: "GET",
            //    success: function (response, status, xhr) {
            //        var nvContainer = $("#topbar");
            //        nvContainer.html(response);
            //        menuLoaded = true;
            //        var top = $('#topbar .active').text();
            //        //alert(top);
            //        //loadSidebar(top.trim());
            //        loadHref();
            //    },
            //    error: function (xmlHttpRequest, textStatus, errorThrown) {
            //        var nvContainer = $(".navcontainer");
            //        nvContainer.html(errorThrown);
            //    }
            //});
        }
    }
}();