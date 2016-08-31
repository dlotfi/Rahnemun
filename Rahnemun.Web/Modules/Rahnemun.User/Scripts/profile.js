// The view model constructor
rahnemun.ProfileEditViewModel = function (submitUrl, resultMessage, resultMessageTitle, updatePending, timestamp) {
    var self = this;

    // Public members
    self.processing = ko.observable(false);
    self.error = ko.observable(null);
    self.updatePending = ko.observable(updatePending);
    self.save = function (formElement) {
        if (self.processing() || !$(formElement).isValid()) return;
        self.processing(true);
        rahnemun.ajax({
            url: submitUrl, type: "POST", dataType: "JSON",
            data: { timestamp: timestamp },
            success: function (result) {
                rahnemun.message(resultMessage, resultMessageTitle, "success");
                timestamp = result.timestamp;
                self.updatePending(true);
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
    $("[data-profile]").each(function () {
        var data = $(this).data("profile");
        var viewModel = new rahnemun.ProfileEditViewModel(data.submitUrl, data.resultMessage, data.resultMessageTitle, data.updatePending, data.timestamp);
        ko.applyBindings(viewModel, this);
    });
});
