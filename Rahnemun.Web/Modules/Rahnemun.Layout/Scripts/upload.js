$(function () {
	$("form").on("click", ".form-photo a.remove", null, function () {
		var elem = $(this);
		rahnemun.confirm("آیا مایل به حذف این فایل هستید؟", "تایید",
			function () {
				elem.siblings("a").remove();
				elem.siblings("img").remove();
				elem.siblings("input[type = hidden]").remove();
				elem.remove();
			}
		);
		return false;
	});
});
