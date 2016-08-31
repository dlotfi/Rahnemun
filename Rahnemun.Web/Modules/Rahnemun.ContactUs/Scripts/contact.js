// The view model constructor
rahnemun.ContactUsViewModel = function (addUrl, isUserLoggedin, resultMessage, resultMessageTitle) {
    var self = this;

    // Public members
    self.processing = ko.observable(false);
    self.isUserLoggedin = ko.observable(isUserLoggedin);
    self.send = function (formElement) {
        if (self.processing() || !$(formElement).isValid()) return;
        self.processing(true);
        rahnemun.ajax({
            url: addUrl, type: "POST", dataType: "JSON",
            success: function (result) {
                self.isUserLoggedin(result.isUserLoggedin);
                rahnemun.message(resultMessage, resultMessageTitle, "success");
                $(formElement).resetForm();
            },
            complete: function () {
                // Referesh captcha
                $(formElement).find(".antibot-image").trigger("click");
                self.processing(false);
            },
            failure: function (result) {
                self.isUserLoggedin(result.isUserLoggedin);
                $(formElement).find("#Captcha").val("");
                $(formElement).showErrors(result.errors, function (msg) { rahnemun.message(msg, null, "error"); });
            }
        }, $.fn.ajaxSubmit, $(formElement));
    };
};

$(function () {
    $("[data-customermessage]").each(function () {
        var data = $(this).data("customermessage");
        var viewModel = new rahnemun.ContactUsViewModel(data.addUrl, data.isUserLoggedin, data.resultMessage, data.resultMessageTitle);
        ko.applyBindings(viewModel, this);
    });
    // feedback
    $(".feedback h2").click(function () {
        if (!$(this).hasClass("expanded"))
            $(this).next(".contents").find(".antibot-image").trigger("click");
    });
});
