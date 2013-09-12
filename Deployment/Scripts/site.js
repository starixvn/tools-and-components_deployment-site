var Site = (function () {

    var context = {};

    context.backupAllSite = function () {

        var siteList = $(".site-id:checked").not(".checkbox-all");
        var siteCount = siteList.length;
        var success = true;

        $("#dialog").dialog({ modal: true });

        siteList.each(function (i, e) {
            if (!success) {
                return false;
            }

            var siteId = $(e).attr("data-site-id");
            var siteName = $(e).attr("data-site-name");
            var message = "Starting backup " + (i+1) + "/" + siteCount + " sites. Current site is: " + siteName;
            $("#dialog-text").text(message);
            $.ajax({
                type: "POST",
                url: Global.resolveClientUrl("Site/Backup"),
                async: false,
                data: { id: siteId },
                success: function (ajaxResult) {
                },
                complete: function () {
                }, error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                    success = false;
                }
            });

            message = "Finish backup " + (i + 1) + "/" + siteCount + " sites. Current site is: " + siteName;
            $("#dialog-text").text(message);
        });

        $("#dialog").dialog("close");
    };
    context.initEvent = function () {
        $("#backup-all").click(function () {
            context.backupAllSite();
        });
    };

    context.initialize = function () {
        $(".site-id").checkboxgroup();

        context.initEvent();
    };


    return context;

})();

$(document).ready(function () {
    Site.initialize();
});
