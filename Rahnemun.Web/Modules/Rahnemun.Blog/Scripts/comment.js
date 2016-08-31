rahnemun.CommentFormViewModel = function (form, postUrl, repliedCommentId, afterPostComment) {
    var self = this;

    // Copy "form" properties
    for (var p in form)
        if (form.hasOwnProperty(p)) self[p] = form[p];
    // Other public members
    self.processing = ko.observable(false);
    self.text = ko.observable(null);
    self.isReply = repliedCommentId;
    self.postComment = function (formElement) {
        //if (self.processing() || !$(formElement).isValid()) return;
        self.processing(true);
        rahnemun.ajax({
            url: postUrl, type: "POST", dataType: "JSON",
            data: { repliedCommentId: repliedCommentId, name: self.name(), email: self.email(), text: self.text() },
            success: function (result) {
                self.isUserLoggedin(result.isUserLoggedin);
                afterPostComment(result.comment);
                self.text(null);
            },
            complete: function () {
                self.processing(false);
            },
            failure: function (result) {
                self.isUserLoggedin(result.isUserLoggedin);
                $(formElement).showErrors(result.errors, function (msg) { rahnemun.message(msg, null, "error"); });
            }
        });
    };
}

rahnemun.CommentViewModel = function (comment, form, postUrl) {
    var self = this;
    var afterPostComment = function(newComment) {
        self.newComments.push(new rahnemun.CommentViewModel(newComment, form, postUrl));
        if (comment) // If it's the container comment, don't hide the form after posting comment 
            self.displayForm(false);
    };

    // Copy "comment" properties
    for (var p in comment || {})
        if (comment.hasOwnProperty(p)) self[p] = comment[p];
    // Other public members
    self.newComments = ko.observableArray([]);
    // Because of Knockout limitation to call afterAdd and beforeRemove for if binding,
    // observableArray and foreach binding should be used as a workaround
    self.formModels = ko.observableArray([]);
    self.displayForm = ko.pureComputed({
        read: function() {
            return self.formModels().length > 0;
        },
        write: function(value) {
            if (value && self.formModels().length === 0)
                self.formModels.push(new rahnemun.CommentFormViewModel(form, postUrl, self.id, afterPostComment));
            else
                self.formModels.pop();
        }
    });
    self.toggleForm = function () {
        self.displayForm(!self.displayForm());
    };
    self.animateForm = function(element) {
        $(element).slideToggle('slow');
    }
};

$(function () {
    $("[data-comments]").each(function () {
        $(this).find("li.reply").off("click");
        var data = $(this).data("comments");
        var postUrl = data.postCommentUrl;
        var globalformViewModel = {
            isUserLoggedin: ko.observable(data.isUserLoggedin),
            name: ko.observable(data.name),
            email: ko.observable(data.email)
        };
        var containerViewModel = new rahnemun.CommentViewModel(null, globalformViewModel, postUrl);
        containerViewModel.displayForm(true);
        ko.applyBindings(containerViewModel, this);

        $(this).find("[data-comment]").each(function() {
            var data = $(this).data("comment");
            var commentViewModel = new rahnemun.CommentViewModel({ id: data.id }, globalformViewModel, postUrl);
            ko.applyBindings(commentViewModel, this);
        });
    });
});

ko.bindingHandlers.notBindToParent = {
    init: function () {
        return { controlsDescendantBindings: true };
    }
};
ko.virtualElements.allowedBindings.notBindToParent = true;