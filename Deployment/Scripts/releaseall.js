var ReleaseAll = (function () {

    var context = {};

    context.releaseAllSite = function () {

        var siteList = $(".site-id:checked").not(".checkbox-all"); ;
        var siteCount = siteList.length;
        var packageName = $("#release").attr("data-package-name");
        var packageId = $("#release").attr("data-package-id");
        var success = true;

        $("#dialog").dialog({ modal: true });

        siteList.each(function (i, e) {

            if (!success) {
                return false;
            }

            var siteId = $(e).attr("data-site-id");
            var siteName = $(e).attr("data-site-name");

            var message = "Starting release package " + packageName + " for " + (i + 1) + "/" + siteCount + " sites. Current site is: " + siteName;
            $("#dialog-text").text(message);
            $.ajax({
                type: "POST",
                url: Global.resolveClientUrl("Package/Release/" + packageId),
                async: false,
                data: { siteId: siteId, isBackup: true },
                success: function (ajaxResult) {
                },
                complete: function () {
                }, error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                    success = false;
                }
            });

            message = "Finish release package " + packageName + " for " + (i + 1) + "/" + siteCount + " sites. Current site is: " + siteName;
            $("#dialog-text").text(message);
        });

        $("#dialog").dialog("close");
    };
    context.initEvent = function () {
        $("#release").click(function () {
            context.releaseAllSite();
        });
    };

    context.initialize = function () {
        $(".site-id").checkboxgroup();
        $("#dialog").dialog({ modal: true });
        $("#dialog").dialog("close");
        context.initEvent();
    };


    return context;

})();



$(document).ready(function () {
    ReleaseAll.initialize();
});