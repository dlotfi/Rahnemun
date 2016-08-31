// The view model constructor
rahnemun.SessionViewModel = function (sourceUrl, sendUrl, setSeenUrl, stopUrl) {
    var self = this;

    // Private members
    var processing = false,
        lastMessageTime = null,
        stickDividers = function () {
            //stick dividers to top of page when scrolling
            $(".chatbox .list .divider").stick_in_parent({
                sticky_class: "affix",
                offset_top: 0
            });
        },
        scrollToMessage = function(message) {
            //scroll to message if provided, otherwise scroll to sendbox
            var elementToScroll = message ? $("#msg-" + message.id) : $("div.sendbox");
            if (elementToScroll.length !== 0) // when session is stopped sendbox does not exist
                $(window).scrollTop(elementToScroll.offset().top);
        },
        updateModel = function(newData) {
            ko.utils.arrayPushAll(self.messages, newData.messages);
            lastMessageTime = newData.lastMessageTime;
            if (newData.elapsedTime)
                self.elapsedTime(newData.elapsedTime);
            if (newData.stopped)
                self.stopped(newData.stopped);
            rahnemun.post(setSeenUrl, { lastMessageTime: lastMessageTime });
            stickDividers();
        },
        load = function () {
            if (processing) return;
            processing = true;
            rahnemun.ajax({
                url: sourceUrl, type: "GET", dataType: "JSON",
                data: { lastMessageTime: lastMessageTime },
                success: function (result) {
                    updateModel(result);
                    if (!self.initialized()) {
                        self.initialized(true);
                        var firstUnseenMessage = ko.utils.arrayFirst(self.messages, function (message) { return message.unseen; });
                        scrollToMessage(firstUnseenMessage);
                    }
                },
                complete: function() {
                    processing = false;
                }
            });
        };

    // Public members
    self.initialized = ko.observable(false);
    self.elapsedTime = ko.observable(0);
    self.messages = ko.observableArray([]);
    self.stopped = ko.observable(false);
    self.send = function (formElement) {
        if (processing || !$(formElement).isValid()) return;
        processing = true;
        rahnemun.ajax({
            iframe: true, url: sendUrl, type: "POST", dataType: "JSON",
            data: { lastMessageTime: lastMessageTime },
            success: function (result) {
                updateModel(result);
                var lastMessage = result.messages[result.messages.length - 1];
                scrollToMessage(lastMessage);
                $(formElement).resetForm();
            },
            complete: function () {
                processing = false;
            },
            failure: function (result) {
                $(formElement).showErrors(result.errors, function (msg) { rahnemun.message(msg, null, "error"); });
            }
        }, $.fn.ajaxSubmit, $(formElement));
    };
    self.stop = function () {
        rahnemun.confirm("آیا مایل به اتمام این جلسه هستید؟", "اعلام اتمام جلسه", function () {
            if (processing) return;
            processing = true;
            rahnemun.ajax({
                url: stopUrl, type: "POST", dataType: "JSON",
                data: { lastMessageTime: lastMessageTime },
                success: function (result) {
                    updateModel(result);
                },
                complete: function() {
                    processing = false;
                }
            });
        });
    };

    // Initialization
    load();
    
    // Set poll interval
    setInterval(function () { 
        if (!self.stopped()) load();
    }, 15000);
};

$(function () {
    $("[data-session]").each(function () {
        var data = $(this).data("session");
        var viewModel = new rahnemun.SessionViewModel(data.sourceUrl, data.sendUrl, data.setSeenUrl, data.stopUrl);
        ko.applyBindings(viewModel, this);
    });
});
