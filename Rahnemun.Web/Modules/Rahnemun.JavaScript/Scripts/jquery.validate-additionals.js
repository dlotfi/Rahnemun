﻿$.validator.addMethod("accept", function (value, element, param) {
    param = typeof param === "string" ? param.replace(/,/g, "|") : "png|jpe?g|gif";
    return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i"));
}, $.validator.format("Please enter a value with a valid extension."));