var rahnemun = rahnemun || {}; //rahnemun namespace

Function.prototype.extends = function (base) {
	function surrogateCtor() { }
	surrogateCtor.prototype = base.prototype;
	this.prototype = new surrogateCtor(); // Object.create(base.prototype); requires JavaScript 1.8.5
	this.prototype.constructor = this;
	this.base = base.prototype;
};

rahnemun.route = function (routeName, routeData, success) {
	rahnemun.get("/routes/get-route", routeData, function (result) {
		success(result.url);
	});
};

rahnemun.message = rahnemun.message || function (message, title, type) {
	if (!title) {
		alert(title + ": \r\n" + message);
	} else {
		alert(message);
	}
};

rahnemun.confirm = rahnemun.confirm || function (message, title, ok, cancel) {
	ok = ok || function () { };
	cancel = cancel || function () { };
	if (!title) {
		confirm(title + ": \r\n" + message) ? ok() : cancel();
	} else {
		confirm(message) ? ok() : cancel();
	}
};

rahnemun.dialogsCache = {};
rahnemun.showDialogAdapter = rahnemun.showDialogAdapter || function(dialog, settings) {
	throw new Error("No dialog adapter has been provided.");
};
rahnemun.storeDialogAdapter = rahnemun.storeDialogAdapter || function (dialogId, dialogContent) {
	throw new Error("No dialog storage has been provided.");
};
rahnemun.showDialog = rahnemun.showDialog || function (dialogId, settings_title) {
	// settings can include: title, width, height, complete
	if (!dialogId)
		throw new Error("Dialog name should be specified.");
	var settings = typeof (settings_title) === "string"
		? { title: settings_title }
		: settings_title || {};

	var showDialog = function (dialog) {
		settings.opening = function(eventData) {
			$(dialog).trigger("dialogOpening", eventData);
		}
		rahnemun.showDialogAdapter(dialog, settings);
	}

	var addNewResources = function (resources) {
		if (!resources) return;
		var scripts = $("script");
		for (var i = 0; i < resources.length; i++) {
			var duplicate = false;
			for (var j = 0; j < scripts.length; j++)
				if (scripts[j].outerHTML === resources[i]) {
					duplicate = true;
					break;
				}
			if (!duplicate)
				$("head").append(resources[i]);
		}
	}

	if (rahnemun.dialogsCache[dialogId]) {
		showDialog(rahnemun.dialogsCache[dialogId]);
	} else {
		var dialog = $("#" + dialogId)[0];
		if (dialog) {
			showDialog(dialog);
			rahnemun.dialogsCache[dialogId] = dialog;
		}
		else {
			rahnemun.get("/dialog", { dialogId: dialogId }, function (result) {
				dialog = rahnemun.storeDialogAdapter(dialogId, result.content);
				addNewResources(result.resources);
				showDialog(dialog);
				rahnemun.dialogsCache[dialogId] = dialog;
			});
		}
	}
};



// ================================================ Ajax Helpers ================================================
rahnemun.ajax = function (settings, ajaxCaller, ajaxCallerTargetObject) {
	var jqSettings = settings,
		originalSuccess = settings.success,
		originalError = settings.error;

	jqSettings.success = function (data, textStatus, jqXHR) {
		if (typeof data.success === "undefined" || data.success) {
			if (originalSuccess) {
				originalSuccess(data, textStatus, jqXHR);
			}
		} else {
			if (settings.failure) {
				settings.failure(data, textStatus, jqXHR);
			}
			if (!settings.noAlertFailure && data.message) {
				rahnemun.message(data.message, null, "error");
			}
		}
	};

	if (!settings.noAlertError) {
		jqSettings.error = function (jqXHR, textStatus, errorThrown) {
			if (originalError) {
				originalError(jqXHR, textStatus, errorThrown);
			}
			rahnemun.message("متاسفانه خطایی اتفاق افتاده است. لطفا دوباره تلاش نمایید: " + (textStatus === "error" ? errorThrown : textStatus), null, "error");
		};
	}

	// Ensure call to be detected as an ajax call at server
	jqSettings.data = jqSettings.data || {};
	jqSettings.data["X-Requested-With"] = "XMLHttpRequest";
	
	return ajaxCaller ? ajaxCaller.call(ajaxCallerTargetObject, jqSettings) : $.ajax(jqSettings);
};

rahnemun.get = function (url, data, success, type) {
	return rahnemun.ajax({
		type: "GET",
		url: url,
		data: data,
		success: success,
		dataType: type
	});
};

rahnemun.post = function (url, data, success, type) {
	return rahnemun.ajax({
		type: "POST",
		url: url,
		data: data,
		success: success,
		dataType: type
	});
};


// ================================================ Format Helpers ================================================
rahnemun.isNullOrEmpty = function(str) {
	var s = (typeof str === "undefined" || str === null) ? "" : str.toString().trim();
	return (s.length == 0);
};

rahnemun.ellipsize = function (value, maxLength) {
	maxLength = maxLength || 100;
	if (value == null || value.length <= maxLength) {
		return value;
	}
	var text = value.toString().replace(/[\r\n]/g, ""),
		max = maxLength,
		min = maxLength - 1,
		elipsized = null;
		while (elipsized == null && min > 0) {
			var regex = new RegExp("^.{" + min + "," + max + "}\\s"),
				match = text.match(regex);
			if (match) {
				elipsized = match[0].replace(/\.?\s$/, "…");
			}
			//well, there were no \s between min and max
			min = min - 1;
		}
	return elipsized || text;
};

$(document).ready(function() {
	$(".open-dialog").click(function() {
		var dialogData = $(this).data("dialog").split(";");
		if (dialogData.length > 0) {
			rahnemun.showDialog(dialogData[0], dialogData.length > 1 ? dialogData[1] : null);
			return false;
		}
	});

	$("form [data-ajaxform]").each(function() {
		var $form = $(this);

	});
});