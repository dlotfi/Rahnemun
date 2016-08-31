$(document).ready(function () {
    // registers event handlers

    $("a.popup").click(function () {
        window.open(this.href, this.title, "width=600,height=400");
        return false;
    });

    $(":checkbox.toggle-others").each(function () {
        var toggleData = $(this).data("toggle").split(";");
        if (toggleData.length > 0) {
            var element = toggleData[0];
            var className = toggleData.length > 1 ? toggleData[1] : null;
            var title = toggleData.length > 2 ? toggleData[2] : null;
            var originalTitle = $(element).prop("title");

            // set initial status
            var checked = $(this).is(":checked");
            $(element).prop("disabled", !checked);
            $(element).toggleClass("disabled", !checked);
            if (className)
                $(element).toggleClass(className, !checked);
            if (title && !checked)
                $(element).prop("title", title);

            // toggle
            $(this).change(function() {
                $(element).prop("disabled", function (i, v) { return !v; });
                $(element).toggleClass("disabled");
                if (className)
                    $(element).toggleClass(className);
                if (title)
                    $(element).prop("title", function (i, v) { return v === title ? originalTitle : title; });
            });
        }
    });

    // auto hide page title to go with banner title
    if ($(".banner").length !== 0 && edreamer.device() === 1) {
        $("#header > h1").hide();
        $("#header").addClass("noshadow");
    }

    if ($("#header").hasClass("autohide") &&
        edreamer.device() === 1 &&
        $(".banner").length !== 0) {

        $(window).scroll({
            previousTop: 0
        }, function () {

            if ($("body").hasClass("offcanvas") == false &&
                $("#header").hasClass("extended") == false &&
                $(".actions").hasClass("active") == false) {

                var currentTop = $(window).scrollTop();

                if (currentTop <= 0) {
                    $("#header > h1").fadeOut("fast");
                    $("#header").addClass("noshadow");
                } else {
                    $("#header > h1").fadeIn();
                    $("#header").removeClass("noshadow");
                }

                this.previousTop = currentTop;

            }
        });
    }

    // reading mode trigger
    $(".readingmode-trigger").off("click");
    $(".readingmode-trigger").click(function () {
        edreamer.showDialog($(".readingmode"), "reading", { title: document.title.replace(" | رهنمون", "") });
        return false;
    });

    // show notification dialogs
    $("[data-notifications]").each(function () {
        var data = $(this).data("notifications");
        var title, icon;
        $.each(data, function(i, notification) {
            switch (notification.type) {
                case "Information": title = "اطلاعات"; icon = "info"; break;
                case "Success": title = "موفقیت"; icon = "success"; break;
                case "Error": title = "خطا"; icon = "error"; break;
                case "Warning": title = "اخطار"; icon = "warning"; break;
                default: throw new Error("Invalid notfication type!");
            }
            edreamer.showDialog(notification.message, "message", { title: title, icon: icon });
        });
    });

    // focus first invalid input on page
    var invalidFormGroup = $('.form-group.error:first');
    if (invalidFormGroup.length > 0) {
        var invalidInput = invalidFormGroup.find(".input-validation-error:first");
        if (invalidInput.length > 0)
            invalidInput.focus();
        else
            $(window).scrollTop(invalidFormGroup.offset().top);
        //$("html,body").animate({
        //    scrollTop: invalidFormGroup.offset().top
        //}, "normal");
    }
});

if ($.validator) {
    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest(".form-group").addClass("error");
        },
        unhighlight: function (element) {
            $(element).closest(".form-group").removeClass("error");
        }
    });
}

if (rahnemun) {
    rahnemun.confirm = function (message, title, ok, cancel) {
        edreamer.showDialog(message, "confirm", {
            title: title,
            icon: "question",
            closed: function (answer) {
            if (answer && ok)
                ok();
            else if (!answer && cancel)
                cancel();
            }
        });
    };

    rahnemun.message = function (message, title, type) {
        if (!title) {
            switch (type) {
                case "info": title = "اطلاعات"; break;
                case "error": title = "خطا"; break;
                case "warning": title = "اخطار"; break;
                case "success": title = "موفقیت"; break;
                case "question": title = "سوال"; break;
                case "waiting": title = "انتظار";
            }
        }
        edreamer.showDialog(message, "message", { title: title, icon: type });
    };

    rahnemun.showDialogAdapter = function (dialog, settings) {
        var dialogSettings = {
            title: settings.title,
            width: settings.width,
            height: settings.height,
            method: "move",
            opening: function (dialogContent, isFirstTime) {
                if (settings.opening) {
                    var eventData = { dialogContent: dialogContent, closeDialog: edreamer.closeDialog, settings: dialogSettings, firstInit: isFirstTime };
                    settings.opening(eventData);
                }
            },
            closed: function (answer) {
                if (settings.complete) settings.complete(answer);
            }
        };
        edreamer.showDialog(dialog, "inline", dialogSettings);
    };

    rahnemun.storeDialogAdapter = function (dialogId, dialogContent) {
        $(".dialogs-container").append(dialogContent);  
        return $(".dialogs-container > #" + dialogId)[0];
    };
}
