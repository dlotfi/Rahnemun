$(function () {
	$("form").on({
	    click: function () {
	        var src = $(this).attr("src").split("?")[0];
		    $(this).hide().attr("src", src + "?" + Math.random()).fadeIn();
		}
	}, ".antibot-image");
});
