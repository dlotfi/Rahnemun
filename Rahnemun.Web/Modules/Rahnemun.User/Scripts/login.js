rahnemun.LoginDialogViewModel = function (closeDialog, url) {
    var self = this;
    // Public members
    self.processing = ko.observable(false);
    self.error = ko.observable(null);
    self.login = function (formElement) {
        if (self.processing() || !$(formElement).isValid()) return;
        self.processing(true);
        self.error(null);
        rahnemun.ajax({
            dataType: "JSON",
            url: url,
            type: "POST",
            success: function (result) {
                closeDialog();
                if (result.returnUrl)
                    window.location = result.returnUrl;
                else
                    location.reload();
            },
            complete: function () {
                self.processing(false);
            },
            failure: function (result) {
                $(formElement).showErrors(result.errors, function (msg) { self.error(msg); });
            }
        }, $.fn.ajaxSubmit, $(formElement));
    };
};

$(function () {
    $("#login-dialog").on("dialogOpening", null, null, function (event, dialog) {
        var loginUrl = $(event.target).data("login-url");
        dialog.settings.width = 400;
        dialog.settings.height = 420;
        if (dialog.firstInit) {
            var viewModel = new rahnemun.LoginDialogViewModel(dialog.closeDialog, loginUrl);
            ko.applyBindings(viewModel, event.target);
        } else {
            $(event.target).find("form").resetForm();
            ko.dataFor(event.target).error(null);
        }
    });
});