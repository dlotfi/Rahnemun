/*!
** On demand validation for jQuery Validation Unobtrusive
*/

$.validator.addMethod("dummy", function () { return true; }, "");

(function ($) {
	$.fn.isValid = function () {
		// Check if not parsed yet
		if (!$(this).data("unobtrusiveValidation")) {
			$.validator.unobtrusive.parse(this);
		}

		// Check to see if validation status changes
		var settings = $(this).data('validator').settings;
		settings.showErrors = function (errorMap, errorList) {
			var form = $(this.currentForm);
			var lastInvalidState = form.data('invalstate1');
			if (!lastInvalidState && this.numberOfInvalids()) {
			    form.data('invalstate1', true);
				form.trigger('valstatechange', false);
			}
			else if (lastInvalidState && !this.numberOfInvalids()) {
			    form.data('invalstate1', false);
				form.trigger('valstatechange', true);
			}
			this.defaultShowErrors();
		};

		return $(this).valid();
	};

	$.fn.showErrors = function (errors, showModelError) {
		var validator = $(this).validate();
		// Add dummy (always valid) validation rule to all inputs in order to prevent 
		// error messages to stick to invalid inputs even after changing them
		$(this).find("input, select, textarea").each(function () {
			$(this).rules("add", "dummy");
		});

		// Check to see if validation status changes
		var form = $(this);
		var lastInvalidState = form.data('invalstate2');
		if (!lastInvalidState && !$.isEmptyObject(errors)) {
		    form.data('invalstate2', true);
			form.trigger('valstatechange', false);
		}
		else if (lastInvalidState && $.isEmptyObject(errors)) {
		    form.data('invalstate2', false);
			form.trigger('valstatechange', true);
		}
		if (errors && errors['']) {
			if (showModelError) showModelError(errors['']);
			delete errors[''];
		}

		validator.showErrors(errors);
		validator.focusInvalid();
	};
})(jQuery);