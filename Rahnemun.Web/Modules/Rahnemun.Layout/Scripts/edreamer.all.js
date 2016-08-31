/*!	SWFObject v2.2 <http://code.google.com/p/swfobject/> 
	is released under the MIT License <http://www.opensource.org/licenses/mit-license.php> 
*/

var audioplayer_swfobject = function() {
	
	var UNDEF = "undefined",
		OBJECT = "object",
		SHOCKWAVE_FLASH = "Shockwave Flash",
		SHOCKWAVE_FLASH_AX = "ShockwaveFlash.ShockwaveFlash",
		FLASH_MIME_TYPE = "application/x-shockwave-flash",
		EXPRESS_INSTALL_ID = "SWFObjectExprInst",
		ON_READY_STATE_CHANGE = "onreadystatechange",
		
		win = window,
		doc = document,
		nav = navigator,
		
		plugin = false,
		domLoadFnArr = [main],
		regObjArr = [],
		objIdArr = [],
		listenersArr = [],
		storedAltContent,
		storedAltContentId,
		storedCallbackFn,
		storedCallbackObj,
		isDomLoaded = false,
		isExpressInstallActive = false,
		dynamicStylesheet,
		dynamicStylesheetMedia,
		autoHideShow = true,
	
	/* Centralized function for browser feature detection
		- User agent string detection is only used when no good alternative is possible
		- Is executed directly for optimal performance
	*/	
	ua = function() {
		var w3cdom = typeof doc.getElementById != UNDEF && typeof doc.getElementsByTagName != UNDEF && typeof doc.createElement != UNDEF,
			u = nav.userAgent.toLowerCase(),
			p = nav.platform.toLowerCase(),
			windows = p ? /win/.test(p) : /win/.test(u),
			mac = p ? /mac/.test(p) : /mac/.test(u),
			webkit = /webkit/.test(u) ? parseFloat(u.replace(/^.*webkit\/(\d+(\.\d+)?).*$/, "$1")) : false, // returns either the webkit version or false if not webkit
			ie = !+"\v1", // feature detection based on Andrea Giammarchi's solution: http://webreflection.blogspot.com/2009/01/32-bytes-to-know-if-your-browser-is-ie.html
			playerVersion = [0,0,0],
			d = null;
		if (typeof nav.plugins != UNDEF && typeof nav.plugins[SHOCKWAVE_FLASH] == OBJECT) {
			d = nav.plugins[SHOCKWAVE_FLASH].description;
			if (d && !(typeof nav.mimeTypes != UNDEF && nav.mimeTypes[FLASH_MIME_TYPE] && !nav.mimeTypes[FLASH_MIME_TYPE].enabledPlugin)) { // navigator.mimeTypes["application/x-shockwave-flash"].enabledPlugin indicates whether plug-ins are enabled or disabled in Safari 3+
				plugin = true;
				ie = false; // cascaded feature detection for Internet Explorer
				d = d.replace(/^.*\s+(\S+\s+\S+$)/, "$1");
				playerVersion[0] = parseInt(d.replace(/^(.*)\..*$/, "$1"), 10);
				playerVersion[1] = parseInt(d.replace(/^.*\.(.*)\s.*$/, "$1"), 10);
				playerVersion[2] = /[a-zA-Z]/.test(d) ? parseInt(d.replace(/^.*[a-zA-Z]+(.*)$/, "$1"), 10) : 0;
			}
		}
		else if (typeof win.ActiveXObject != UNDEF) {
			try {
				var a = new ActiveXObject(SHOCKWAVE_FLASH_AX);
				if (a) { // a will return null when ActiveX is disabled
					d = a.GetVariable("$version");
					if (d) {
						ie = true; // cascaded feature detection for Internet Explorer
						d = d.split(" ")[1].split(",");
						playerVersion = [parseInt(d[0], 10), parseInt(d[1], 10), parseInt(d[2], 10)];
					}
				}
			}
			catch(e) {}
		}
		return { w3:w3cdom, pv:playerVersion, wk:webkit, ie:ie, win:windows, mac:mac };
	}(),
	
	/* Cross-browser onDomLoad
		- Will fire an event as soon as the DOM of a web page is loaded
		- Internet Explorer workaround based on Diego Perini's solution: http://javascript.nwbox.com/IEContentLoaded/
		- Regular onload serves as fallback
	*/ 
	onDomLoad = function() {
		if (!ua.w3) { return; }
		if ((typeof doc.readyState != UNDEF && doc.readyState == "complete") || (typeof doc.readyState == UNDEF && (doc.getElementsByTagName("body")[0] || doc.body))) { // function is fired after onload, e.g. when script is inserted dynamically 
			callDomLoadFunctions();
		}
		if (!isDomLoaded) {
			if (typeof doc.addEventListener != UNDEF) {
				doc.addEventListener("DOMContentLoaded", callDomLoadFunctions, false);
			}		
			if (ua.ie && ua.win) {
				doc.attachEvent(ON_READY_STATE_CHANGE, function() {
					if (doc.readyState == "complete") {
						doc.detachEvent(ON_READY_STATE_CHANGE, arguments.callee);
						callDomLoadFunctions();
					}
				});
				if (win == top) { // if not inside an iframe
					(function(){
						if (isDomLoaded) { return; }
						try {
							doc.documentElement.doScroll("left");
						}
						catch(e) {
							setTimeout(arguments.callee, 0);
							return;
						}
						callDomLoadFunctions();
					})();
				}
			}
			if (ua.wk) {
				(function(){
					if (isDomLoaded) { return; }
					if (!/loaded|complete/.test(doc.readyState)) {
						setTimeout(arguments.callee, 0);
						return;
					}
					callDomLoadFunctions();
				})();
			}
			addLoadEvent(callDomLoadFunctions);
		}
	}();
	
	function callDomLoadFunctions() {
		if (isDomLoaded) { return; }
		try { // test if we can really add/remove elements to/from the DOM; we don't want to fire it too early
			var t = doc.getElementsByTagName("body")[0].appendChild(createElement("span"));
			t.parentNode.removeChild(t);
		}
		catch (e) { return; }
		isDomLoaded = true;
		var dl = domLoadFnArr.length;
		for (var i = 0; i < dl; i++) {
			domLoadFnArr[i]();
		}
	}
	
	function addDomLoadEvent(fn) {
		if (isDomLoaded) {
			fn();
		}
		else { 
			domLoadFnArr[domLoadFnArr.length] = fn; // Array.push() is only available in IE5.5+
		}
	}
	
	/* Cross-browser onload
		- Based on James Edwards' solution: http://brothercake.com/site/resources/scripts/onload/
		- Will fire an event as soon as a web page including all of its assets are loaded 
	 */
	function addLoadEvent(fn) {
		if (typeof win.addEventListener != UNDEF) {
			win.addEventListener("load", fn, false);
		}
		else if (typeof doc.addEventListener != UNDEF) {
			doc.addEventListener("load", fn, false);
		}
		else if (typeof win.attachEvent != UNDEF) {
			addListener(win, "onload", fn);
		}
		else if (typeof win.onload == "function") {
			var fnOld = win.onload;
			win.onload = function() {
				fnOld();
				fn();
			};
		}
		else {
			win.onload = fn;
		}
	}
	
	/* Main function
		- Will preferably execute onDomLoad, otherwise onload (as a fallback)
	*/
	function main() { 
		if (plugin) {
			testPlayerVersion();
		}
		else {
			matchVersions();
		}
	}
	
	/* Detect the Flash Player version for non-Internet Explorer browsers
		- Detecting the plug-in version via the object element is more precise than using the plugins collection item's description:
		  a. Both release and build numbers can be detected
		  b. Avoid wrong descriptions by corrupt installers provided by Adobe
		  c. Avoid wrong descriptions by multiple Flash Player entries in the plugin Array, caused by incorrect browser imports
		- Disadvantage of this method is that it depends on the availability of the DOM, while the plugins collection is immediately available
	*/
	function testPlayerVersion() {
		var b = doc.getElementsByTagName("body")[0];
		var o = createElement(OBJECT);
		o.setAttribute("type", FLASH_MIME_TYPE);
		var t = b.appendChild(o);
		if (t) {
			var counter = 0;
			(function(){
				if (typeof t.GetVariable != UNDEF) {
					var d = t.GetVariable("$version");
					if (d) {
						d = d.split(" ")[1].split(",");
						ua.pv = [parseInt(d[0], 10), parseInt(d[1], 10), parseInt(d[2], 10)];
					}
				}
				else if (counter < 10) {
					counter++;
					setTimeout(arguments.callee, 10);
					return;
				}
				b.removeChild(o);
				t = null;
				matchVersions();
			})();
		}
		else {
			matchVersions();
		}
	}
	
	/* Perform Flash Player and SWF version matching; static publishing only
	*/
	function matchVersions() {
		var rl = regObjArr.length;
		if (rl > 0) {
			for (var i = 0; i < rl; i++) { // for each registered object element
				var id = regObjArr[i].id;
				var cb = regObjArr[i].callbackFn;
				var cbObj = {success:false, id:id};
				if (ua.pv[0] > 0) {
					var obj = getElementById(id);
					if (obj) {
						if (hasPlayerVersion(regObjArr[i].swfVersion) && !(ua.wk && ua.wk < 312)) { // Flash Player version >= published SWF version: Houston, we have a match!
							setVisibility(id, true);
							if (cb) {
								cbObj.success = true;
								cbObj.ref = getObjectById(id);
								cb(cbObj);
							}
						}
						else if (regObjArr[i].expressInstall && canExpressInstall()) { // show the Adobe Express Install dialog if set by the web page author and if supported
							var att = {};
							att.data = regObjArr[i].expressInstall;
							att.width = obj.getAttribute("width") || "0";
							att.height = obj.getAttribute("height") || "0";
							if (obj.getAttribute("class")) { att.styleclass = obj.getAttribute("class"); }
							if (obj.getAttribute("align")) { att.align = obj.getAttribute("align"); }
							// parse HTML object param element's name-value pairs
							var par = {};
							var p = obj.getElementsByTagName("param");
							var pl = p.length;
							for (var j = 0; j < pl; j++) {
								if (p[j].getAttribute("name").toLowerCase() != "movie") {
									par[p[j].getAttribute("name")] = p[j].getAttribute("value");
								}
							}
							showExpressInstall(att, par, id, cb);
						}
						else { // Flash Player and SWF version mismatch or an older Webkit engine that ignores the HTML object element's nested param elements: display alternative content instead of SWF
							displayAltContent(obj);
							if (cb) { cb(cbObj); }
						}
					}
				}
				else {	// if no Flash Player is installed or the fp version cannot be detected we let the HTML object element do its job (either show a SWF or alternative content)
					setVisibility(id, true);
					if (cb) {
						var o = getObjectById(id); // test whether there is an HTML object element or not
						if (o && typeof o.SetVariable != UNDEF) { 
							cbObj.success = true;
							cbObj.ref = o;
						}
						cb(cbObj);
					}
				}
			}
		}
	}
	
	function getObjectById(objectIdStr) {
		var r = null;
		var o = getElementById(objectIdStr);
		if (o && o.nodeName == "OBJECT") {
			if (typeof o.SetVariable != UNDEF) {
				r = o;
			}
			else {
				var n = o.getElementsByTagName(OBJECT)[0];
				if (n) {
					r = n;
				}
			}
		}
		return r;
	}
	
	/* Requirements for Adobe Express Install
		- only one instance can be active at a time
		- fp 6.0.65 or higher
		- Win/Mac OS only
		- no Webkit engines older than version 312
	*/
	function canExpressInstall() {
		return !isExpressInstallActive && hasPlayerVersion("6.0.65") && (ua.win || ua.mac) && !(ua.wk && ua.wk < 312);
	}
	
	/* Show the Adobe Express Install dialog
		- Reference: http://www.adobe.com/cfusion/knowledgebase/index.cfm?id=6a253b75
	*/
	function showExpressInstall(att, par, replaceElemIdStr, callbackFn) {
		isExpressInstallActive = true;
		storedCallbackFn = callbackFn || null;
		storedCallbackObj = {success:false, id:replaceElemIdStr};
		var obj = getElementById(replaceElemIdStr);
		if (obj) {
			if (obj.nodeName == "OBJECT") { // static publishing
				storedAltContent = abstractAltContent(obj);
				storedAltContentId = null;
			}
			else { // dynamic publishing
				storedAltContent = obj;
				storedAltContentId = replaceElemIdStr;
			}
			att.id = EXPRESS_INSTALL_ID;
			if (typeof att.width == UNDEF || (!/%$/.test(att.width) && parseInt(att.width, 10) < 310)) { att.width = "310"; }
			if (typeof att.height == UNDEF || (!/%$/.test(att.height) && parseInt(att.height, 10) < 137)) { att.height = "137"; }
			doc.title = doc.title.slice(0, 47) + " - Flash Player Installation";
			var pt = ua.ie && ua.win ? "ActiveX" : "PlugIn",
				fv = "MMredirectURL=" + win.location.toString().replace(/&/g,"%26") + "&MMplayerType=" + pt + "&MMdoctitle=" + doc.title;
			if (typeof par.flashvars != UNDEF) {
				par.flashvars += "&" + fv;
			}
			else {
				par.flashvars = fv;
			}
			// IE only: when a SWF is loading (AND: not available in cache) wait for the readyState of the object element to become 4 before removing it,
			// because you cannot properly cancel a loading SWF file without breaking browser load references, also obj.onreadystatechange doesn't work
			if (ua.ie && ua.win && obj.readyState != 4) {
				var newObj = createElement("div");
				replaceElemIdStr += "SWFObjectNew";
				newObj.setAttribute("id", replaceElemIdStr);
				obj.parentNode.insertBefore(newObj, obj); // insert placeholder div that will be replaced by the object element that loads expressinstall.swf
				obj.style.display = "none";
				(function(){
					if (obj.readyState == 4) {
						obj.parentNode.removeChild(obj);
					}
					else {
						setTimeout(arguments.callee, 10);
					}
				})();
			}
			createSWF(att, par, replaceElemIdStr);
		}
	}
	
	/* Functions to abstract and display alternative content
	*/
	function displayAltContent(obj) {
		if (ua.ie && ua.win && obj.readyState != 4) {
			// IE only: when a SWF is loading (AND: not available in cache) wait for the readyState of the object element to become 4 before removing it,
			// because you cannot properly cancel a loading SWF file without breaking browser load references, also obj.onreadystatechange doesn't work
			var el = createElement("div");
			obj.parentNode.insertBefore(el, obj); // insert placeholder div that will be replaced by the alternative content
			el.parentNode.replaceChild(abstractAltContent(obj), el);
			obj.style.display = "none";
			(function(){
				if (obj.readyState == 4) {
					obj.parentNode.removeChild(obj);
				}
				else {
					setTimeout(arguments.callee, 10);
				}
			})();
		}
		else {
			obj.parentNode.replaceChild(abstractAltContent(obj), obj);
		}
	} 

	function abstractAltContent(obj) {
		var ac = createElement("div");
		if (ua.win && ua.ie) {
			ac.innerHTML = obj.innerHTML;
		}
		else {
			var nestedObj = obj.getElementsByTagName(OBJECT)[0];
			if (nestedObj) {
				var c = nestedObj.childNodes;
				if (c) {
					var cl = c.length;
					for (var i = 0; i < cl; i++) {
						if (!(c[i].nodeType == 1 && c[i].nodeName == "PARAM") && !(c[i].nodeType == 8)) {
							ac.appendChild(c[i].cloneNode(true));
						}
					}
				}
			}
		}
		return ac;
	}
	
	/* Cross-browser dynamic SWF creation
	*/
	function createSWF(attObj, parObj, id) {
		var r, el = getElementById(id);
		if (ua.wk && ua.wk < 312) { return r; }
		if (el) {
			if (typeof attObj.id == UNDEF) { // if no 'id' is defined for the object element, it will inherit the 'id' from the alternative content
				attObj.id = id;
			}
			if (ua.ie && ua.win) { // Internet Explorer + the HTML object element + W3C DOM methods do not combine: fall back to outerHTML
				var att = "";
				for (var i in attObj) {
					if (attObj[i] != Object.prototype[i]) { // filter out prototype additions from other potential libraries
						if (i.toLowerCase() == "data") {
							parObj.movie = attObj[i];
						}
						else if (i.toLowerCase() == "styleclass") { // 'class' is an ECMA4 reserved keyword
							att += ' class="' + attObj[i] + '"';
						}
						else if (i.toLowerCase() != "classid") {
							att += ' ' + i + '="' + attObj[i] + '"';
						}
					}
				}
				var par = "";
				for (var j in parObj) {
					if (parObj[j] != Object.prototype[j]) { // filter out prototype additions from other potential libraries
						par += '<param name="' + j + '" value="' + parObj[j] + '" />';
					}
				}
				el.outerHTML = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"' + att + '>' + par + '</object>';
				objIdArr[objIdArr.length] = attObj.id; // stored to fix object 'leaks' on unload (dynamic publishing only)
				r = getElementById(attObj.id);	
			}
			else { // well-behaving browsers
				var o = createElement(OBJECT);
				o.setAttribute("type", FLASH_MIME_TYPE);
				for (var m in attObj) {
					if (attObj[m] != Object.prototype[m]) { // filter out prototype additions from other potential libraries
						if (m.toLowerCase() == "styleclass") { // 'class' is an ECMA4 reserved keyword
							o.setAttribute("class", attObj[m]);
						}
						else if (m.toLowerCase() != "classid") { // filter out IE specific attribute
							o.setAttribute(m, attObj[m]);
						}
					}
				}
				for (var n in parObj) {
					if (parObj[n] != Object.prototype[n] && n.toLowerCase() != "movie") { // filter out prototype additions from other potential libraries and IE specific param element
						createObjParam(o, n, parObj[n]);
					}
				}
				el.parentNode.replaceChild(o, el);
				r = o;
			}
		}
		return r;
	}
	
	function createObjParam(el, pName, pValue) {
		var p = createElement("param");
		p.setAttribute("name", pName);	
		p.setAttribute("value", pValue);
		el.appendChild(p);
	}
	
	/* Cross-browser SWF removal
		- Especially needed to safely and completely remove a SWF in Internet Explorer
	*/
	function removeSWF(id) {
		var obj = getElementById(id);
		if (obj && obj.nodeName == "OBJECT") {
			if (ua.ie && ua.win) {
				obj.style.display = "none";
				(function(){
					if (obj.readyState == 4) {
						removeObjectInIE(id);
					}
					else {
						setTimeout(arguments.callee, 10);
					}
				})();
			}
			else {
				obj.parentNode.removeChild(obj);
			}
		}
	}
	
	function removeObjectInIE(id) {
		var obj = getElementById(id);
		if (obj) {
			for (var i in obj) {
				if (typeof obj[i] == "function") {
					obj[i] = null;
				}
			}
			obj.parentNode.removeChild(obj);
		}
	}
	
	/* Functions to optimize JavaScript compression
	*/
	function getElementById(id) {
		var el = null;
		try {
			el = doc.getElementById(id);
		}
		catch (e) {}
		return el;
	}
	
	function createElement(el) {
		return doc.createElement(el);
	}
	
	/* Updated attachEvent function for Internet Explorer
		- Stores attachEvent information in an Array, so on unload the detachEvent functions can be called to avoid memory leaks
	*/	
	function addListener(target, eventType, fn) {
		target.attachEvent(eventType, fn);
		listenersArr[listenersArr.length] = [target, eventType, fn];
	}
	
	/* Flash Player and SWF content version matching
	*/
	function hasPlayerVersion(rv) {
		var pv = ua.pv, v = rv.split(".");
		v[0] = parseInt(v[0], 10);
		v[1] = parseInt(v[1], 10) || 0; // supports short notation, e.g. "9" instead of "9.0.0"
		v[2] = parseInt(v[2], 10) || 0;
		return (pv[0] > v[0] || (pv[0] == v[0] && pv[1] > v[1]) || (pv[0] == v[0] && pv[1] == v[1] && pv[2] >= v[2])) ? true : false;
	}
	
	/* Cross-browser dynamic CSS creation
		- Based on Bobby van der Sluis' solution: http://www.bobbyvandersluis.com/articles/dynamicCSS.php
	*/	
	function createCSS(sel, decl, media, newStyle) {
		if (ua.ie && ua.mac) { return; }
		var h = doc.getElementsByTagName("head")[0];
		if (!h) { return; } // to also support badly authored HTML pages that lack a head element
		var m = (media && typeof media == "string") ? media : "screen";
		if (newStyle) {
			dynamicStylesheet = null;
			dynamicStylesheetMedia = null;
		}
		if (!dynamicStylesheet || dynamicStylesheetMedia != m) { 
			// create dynamic stylesheet + get a global reference to it
			var s = createElement("style");
			s.setAttribute("type", "text/css");
			s.setAttribute("media", m);
			dynamicStylesheet = h.appendChild(s);
			if (ua.ie && ua.win && typeof doc.styleSheets != UNDEF && doc.styleSheets.length > 0) {
				dynamicStylesheet = doc.styleSheets[doc.styleSheets.length - 1];
			}
			dynamicStylesheetMedia = m;
		}
		// add style rule
		if (ua.ie && ua.win) {
			if (dynamicStylesheet && typeof dynamicStylesheet.addRule == OBJECT) {
				dynamicStylesheet.addRule(sel, decl);
			}
		}
		else {
			if (dynamicStylesheet && typeof doc.createTextNode != UNDEF) {
				dynamicStylesheet.appendChild(doc.createTextNode(sel + " {" + decl + "}"));
			}
		}
	}
	
	function setVisibility(id, isVisible) {
		if (!autoHideShow) { return; }
		var v = isVisible ? "visible" : "hidden";
		if (isDomLoaded && getElementById(id)) {
			getElementById(id).style.visibility = v;
		}
		else {
			createCSS("#" + id, "visibility:" + v);
		}
	}

	/* Filter to avoid XSS attacks
	*/
	function urlEncodeIfNecessary(s) {
		var regex = /[\\\"<>\.;]/;
		var hasBadChars = regex.exec(s) != null;
		return hasBadChars && typeof encodeURIComponent != UNDEF ? encodeURIComponent(s) : s;
	}
	
	/* Release memory to avoid memory leaks caused by closures, fix hanging audio/video threads and force open sockets/NetConnections to disconnect (Internet Explorer only)
	*/
	var cleanup = function() {
		if (ua.ie && ua.win) {
			window.attachEvent("onunload", function() {
				// remove listeners to avoid memory leaks
				var ll = listenersArr.length;
				for (var i = 0; i < ll; i++) {
					listenersArr[i][0].detachEvent(listenersArr[i][1], listenersArr[i][2]);
				}
				// cleanup dynamically embedded objects to fix audio/video threads and force open sockets and NetConnections to disconnect
				var il = objIdArr.length;
				for (var j = 0; j < il; j++) {
					removeSWF(objIdArr[j]);
				}
				// cleanup library's main closures to avoid memory leaks
				for (var k in ua) {
					ua[k] = null;
				}
				ua = null;
				for (var l in audioplayer_swfobject) {
					audioplayer_swfobject[l] = null;
				}
				audioplayer_swfobject = null;
			});
		}
	}();
	
	return {
		/* Public API
			- Reference: http://code.google.com/p/swfobject/wiki/documentation
		*/ 
		registerObject: function(objectIdStr, swfVersionStr, xiSwfUrlStr, callbackFn) {
			if (ua.w3 && objectIdStr && swfVersionStr) {
				var regObj = {};
				regObj.id = objectIdStr;
				regObj.swfVersion = swfVersionStr;
				regObj.expressInstall = xiSwfUrlStr;
				regObj.callbackFn = callbackFn;
				regObjArr[regObjArr.length] = regObj;
				setVisibility(objectIdStr, false);
			}
			else if (callbackFn) {
				callbackFn({success:false, id:objectIdStr});
			}
		},
		
		getObjectById: function(objectIdStr) {
			if (ua.w3) {
				return getObjectById(objectIdStr);
			}
		},
		
		embedSWF: function(swfUrlStr, replaceElemIdStr, widthStr, heightStr, swfVersionStr, xiSwfUrlStr, flashvarsObj, parObj, attObj, callbackFn) {
			var callbackObj = {success:false, id:replaceElemIdStr};
			if (ua.w3 && !(ua.wk && ua.wk < 312) && swfUrlStr && replaceElemIdStr && widthStr && heightStr && swfVersionStr) {
				setVisibility(replaceElemIdStr, false);
				addDomLoadEvent(function() {
					widthStr += ""; // auto-convert to string
					heightStr += "";
					var att = {};
					if (attObj && typeof attObj === OBJECT) {
						for (var i in attObj) { // copy object to avoid the use of references, because web authors often reuse attObj for multiple SWFs
							att[i] = attObj[i];
						}
					}
					att.data = swfUrlStr;
					att.width = widthStr;
					att.height = heightStr;
					var par = {}; 
					if (parObj && typeof parObj === OBJECT) {
						for (var j in parObj) { // copy object to avoid the use of references, because web authors often reuse parObj for multiple SWFs
							par[j] = parObj[j];
						}
					}
					if (flashvarsObj && typeof flashvarsObj === OBJECT) {
						for (var k in flashvarsObj) { // copy object to avoid the use of references, because web authors often reuse flashvarsObj for multiple SWFs
							if (typeof par.flashvars != UNDEF) {
								par.flashvars += "&" + k + "=" + flashvarsObj[k];
							}
							else {
								par.flashvars = k + "=" + flashvarsObj[k];
							}
						}
					}
					if (hasPlayerVersion(swfVersionStr)) { // create SWF
						var obj = createSWF(att, par, replaceElemIdStr);
						if (att.id == replaceElemIdStr) {
							setVisibility(replaceElemIdStr, true);
						}
						callbackObj.success = true;
						callbackObj.ref = obj;
					}
					else if (xiSwfUrlStr && canExpressInstall()) { // show Adobe Express Install
						att.data = xiSwfUrlStr;
						showExpressInstall(att, par, replaceElemIdStr, callbackFn);
						return;
					}
					else { // show alternative content
						setVisibility(replaceElemIdStr, true);
					}
					if (callbackFn) { callbackFn(callbackObj); }
				});
			}
			else if (callbackFn) { callbackFn(callbackObj);	}
		},
		
		switchOffAutoHideShow: function() {
			autoHideShow = false;
		},
		
		ua: ua,
		
		getFlashPlayerVersion: function() {
			return { major:ua.pv[0], minor:ua.pv[1], release:ua.pv[2] };
		},
		
		hasFlashPlayerVersion: hasPlayerVersion,
		
		createSWF: function(attObj, parObj, replaceElemIdStr) {
			if (ua.w3) {
				return createSWF(attObj, parObj, replaceElemIdStr);
			}
			else {
				return undefined;
			}
		},
		
		showExpressInstall: function(att, par, replaceElemIdStr, callbackFn) {
			if (ua.w3 && canExpressInstall()) {
				showExpressInstall(att, par, replaceElemIdStr, callbackFn);
			}
		},
		
		removeSWF: function(objElemIdStr) {
			if (ua.w3) {
				removeSWF(objElemIdStr);
			}
		},
		
		createCSS: function(selStr, declStr, mediaStr, newStyleBoolean) {
			if (ua.w3) {
				createCSS(selStr, declStr, mediaStr, newStyleBoolean);
			}
		},
		
		addDomLoadEvent: addDomLoadEvent,
		
		addLoadEvent: addLoadEvent,
		
		getQueryParamValue: function(param) {
			var q = doc.location.search || doc.location.hash;
			if (q) {
				if (/\?/.test(q)) { q = q.split("?")[1]; } // strip question mark
				if (param == null) {
					return urlEncodeIfNecessary(q);
				}
				var pairs = q.split("&");
				for (var i = 0; i < pairs.length; i++) {
					if (pairs[i].substring(0, pairs[i].indexOf("=")) == param) {
						return urlEncodeIfNecessary(pairs[i].substring((pairs[i].indexOf("=") + 1)));
					}
				}
			}
			return "";
		},
		
		// For internal usage only
		expressInstallCallback: function() {
			if (isExpressInstallActive) {
				var obj = getElementById(EXPRESS_INSTALL_ID);
				if (obj && storedAltContent) {
					obj.parentNode.replaceChild(storedAltContent, obj);
					if (storedAltContentId) {
						setVisibility(storedAltContentId, true);
						if (ua.ie && ua.win) { storedAltContent.style.display = "block"; }
					}
					if (storedCallbackFn) { storedCallbackFn(storedCallbackObj); }
				}
				isExpressInstallActive = false;
			} 
		}
	};
}();
var AudioPlayer = function () {
	var instances = [];
	var groups = {};
	var activePlayers = {};
	var playerURL = "";
	var defaultOptions = {};
	var currentVolume = -1;
	var requiredFlashVersion = "9";
	
	function getPlayer(playerID) {
		if (document.all && !window[playerID]) {
			for (var i = 0; i < document.forms.length; i++) {
				if (document.forms[i][playerID]) {
					return document.forms[i][playerID];
					break;
				}
			}
		}
		return document.all ? window[playerID] : document[playerID];
	}
	
	function addListener (playerID, type, func) {
		getPlayer(playerID).addListener(type, func);
	}
	
	return {
		setup: function (url, options) {
			playerURL = url;
			defaultOptions = options;
			if (audioplayer_swfobject.hasFlashPlayerVersion(requiredFlashVersion)) {
				audioplayer_swfobject.switchOffAutoHideShow();
				audioplayer_swfobject.createCSS("p.audioplayer_container span", "visibility:hidden;height:24px;overflow:hidden;padding:0;border:none;");
			}
		},

		getPlayer: function (playerID) {
			return getPlayer(playerID);
		},
		
		addListener: function (playerID, type, func) {
			addListener(playerID, type, func);
		},
		
		embed: function (elementID, options) {
			var instanceOptions = {};
			var key;
			
			var flashParams = {};
			var flashVars = {};
			var flashAttributes = {};
	
			// Merge default options and instance options
			for (key in defaultOptions) {
				instanceOptions[key] = defaultOptions[key];
			}
			for (key in options) {
				instanceOptions[key] = options[key];
			}
			
			if (instanceOptions.transparentpagebg == "yes") {
				flashParams.bgcolor = "#FFFFFF";
				flashParams.wmode = "transparent";
			} else {
				if (instanceOptions.pagebg) {
					flashParams.bgcolor = "#" + instanceOptions.pagebg;
				}
				flashParams.wmode = "opaque";
			}
			
			flashParams.menu = "false";
			
			for (key in instanceOptions) {
				if (key == "pagebg" || key == "width" || key == "transparentpagebg") {
					continue;
				}
				flashVars[key] = instanceOptions[key];
			}
			
			flashAttributes.name = elementID;
			flashAttributes.style = "outline: none";
			
			flashVars.playerID = elementID;
			
			audioplayer_swfobject.embedSWF(playerURL, elementID, instanceOptions.width.toString(), "24", requiredFlashVersion, false, flashVars, flashParams, flashAttributes);
			
			instances.push(elementID);
			
			if (options.group) {
				groups[elementID] = options.group;
			}
		},
		
		syncVolumes: function (playerID, volume) {	
			if (groups[playerID]) return;
			currentVolume = volume;
			for (var i = 0; i < instances.length; i++) {
				if (!groups[instances[i]] && instances[i] != playerID) {
					getPlayer(instances[i]).setVolume(currentVolume);
				}
			}
		},
		
		activate: function (playerID, info) {
			for (var activePlayerID in activePlayers) {
				if (activePlayerID == playerID) {
					continue;
				}
				if (groups[playerID] != groups[activePlayerID]) {
					this.close(activePlayerID);
					continue;
				}
				if (!(groups[playerID] || groups[activePlayerID])) {
					this.close(activePlayerID);
				}
			}
			activePlayers[playerID] = 1;
		},
		
		load: function (playerID, soundFile, titles, artists) {
			getPlayer(playerID).load(soundFile, titles, artists);
		},
		
		close: function (playerID) {
			getPlayer(playerID).close();
			if (playerID in activePlayers) {
				delete activePlayers[playerID];
			}
		},
		
		open: function (playerID, index) {
			if (index == undefined) {
				index = 1;
			}
			getPlayer(playerID).open(index == undefined ? 0 : index-1);
		},
		
		getVolume: function (playerID) {
			return currentVolume;
		}
		
	}
	
}();

/*! http://responsiveslides.com v1.54 by @viljamis */
(function(c,I,B){c.fn.responsiveSlides=function(l){var a=c.extend({auto:!0,speed:500,timeout:4E3,pager:!1,nav:!1,random:!1,pause:!1,pauseControls:!0,prevText:"Previous",nextText:"Next",maxwidth:"",navContainer:"",manualControls:"",namespace:"rslides",before:c.noop,after:c.noop},l);return this.each(function(){B++;var f=c(this),s,r,t,m,p,q,n=0,e=f.children(),C=e.size(),h=parseFloat(a.speed),D=parseFloat(a.timeout),u=parseFloat(a.maxwidth),g=a.namespace,d=g+B,E=g+"_nav "+d+"_nav",v=g+"_here",j=d+"_on",
w=d+"_s",k=c("<ul class='"+g+"_tabs "+d+"_tabs' />"),x={"float":"left",position:"relative",opacity:1,zIndex:2},y={"float":"none",position:"absolute",opacity:0,zIndex:1},F=function(){var b=(document.body||document.documentElement).style,a="transition";if("string"===typeof b[a])return!0;s=["Moz","Webkit","Khtml","O","ms"];var a=a.charAt(0).toUpperCase()+a.substr(1),c;for(c=0;c<s.length;c++)if("string"===typeof b[s[c]+a])return!0;return!1}(),z=function(b){a.before(b);F?(e.removeClass(j).css(y).eq(b).addClass(j).css(x),
n=b,setTimeout(function(){a.after(b)},h)):e.stop().fadeOut(h,function(){c(this).removeClass(j).css(y).css("opacity",1)}).eq(b).fadeIn(h,function(){c(this).addClass(j).css(x);a.after(b);n=b})};a.random&&(e.sort(function(){return Math.round(Math.random())-0.5}),f.empty().append(e));e.each(function(a){this.id=w+a});f.addClass(g+" "+d);l&&l.maxwidth&&f.css("max-width",u);e.hide().css(y).eq(0).addClass(j).css(x).show();F&&e.show().css({"-webkit-transition":"opacity "+h+"ms ease-in-out","-moz-transition":"opacity "+
h+"ms ease-in-out","-o-transition":"opacity "+h+"ms ease-in-out",transition:"opacity "+h+"ms ease-in-out"});if(1<e.size()){if(D<h+100)return;if(a.pager&&!a.manualControls){var A=[];e.each(function(a){a+=1;A+="<li><a href='#' class='"+w+a+"'>"+a+"</a></li>"});k.append(A);l.navContainer?c(a.navContainer).append(k):f.after(k)}a.manualControls&&(k=c(a.manualControls),k.addClass(g+"_tabs "+d+"_tabs"));(a.pager||a.manualControls)&&k.find("li").each(function(a){c(this).addClass(w+(a+1))});if(a.pager||a.manualControls)q=
k.find("a"),r=function(a){q.closest("li").removeClass(v).eq(a).addClass(v)};a.auto&&(t=function(){p=setInterval(function(){e.stop(!0,!0);var b=n+1<C?n+1:0;(a.pager||a.manualControls)&&r(b);z(b)},D)},t());m=function(){a.auto&&(clearInterval(p),t())};a.pause&&f.hover(function(){clearInterval(p)},function(){m()});if(a.pager||a.manualControls)q.bind("click",function(b){b.preventDefault();a.pauseControls||m();b=q.index(this);n===b||c("."+j).queue("fx").length||(r(b),z(b))}).eq(0).closest("li").addClass(v),
a.pauseControls&&q.hover(function(){clearInterval(p)},function(){m()});if(a.nav){g="<a href='#' class='"+E+" prev'>"+a.prevText+"</a><a href='#' class='"+E+" next'>"+a.nextText+"</a>";l.navContainer?c(a.navContainer).append(g):f.after(g);var d=c("."+d+"_nav"),G=d.filter(".prev");d.bind("click",function(b){b.preventDefault();b=c("."+j);if(!b.queue("fx").length){var d=e.index(b);b=d-1;d=d+1<C?n+1:0;z(c(this)[0]===G[0]?b:d);if(a.pager||a.manualControls)r(c(this)[0]===G[0]?b:d);a.pauseControls||m()}});
a.pauseControls&&d.hover(function(){clearInterval(p)},function(){m()})}}if("undefined"===typeof document.body.style.maxWidth&&l.maxwidth){var H=function(){f.css("width","100%");f.width()>u&&f.css("width",u)};H();c(I).bind("resize",function(){H()})}})}})(jQuery,this,0);

/*
	By Osvaldas Valutis, www.osvaldas.info
	Available for use under the MIT License
*/



;(function(e,t,n,r){e.fn.doubleTapToGo=function(r){if(!("ontouchstart"in t)&&!navigator.msMaxTouchPoints&&!navigator.userAgent.toLowerCase().match(/windows phone os 7/i))return false;this.each(function(){var t=false;e(this).on("click",function(n){var r=e(this);if(r[0]!=t[0]){n.preventDefault();t=r}});e(n).on("click touchstart MSPointerDown",function(n){var r=true,i=e(n.target).parents();for(var s=0;s<i.length;s++)if(i[s]==t[0])r=false;if(r)t=false})});return this}})(jQuery,window,document);
/*

SMINT V1.0 by Robert McCracken
SMINT V2.0 by robert McCracken with some awesome help from Ryan Clarke (@clarkieryan) and mcpacosy ‏(@mcpacosy)
SMINT V3.0 by robert McCracken with some awesome help from Ryan Clarke (@clarkieryan) and mcpacosy ‏(@mcpacosy)

SMINT is my first dabble into jQuery plugins!

http://www.outyear.co.uk/smint/

If you like Smint, or have suggestions on how it could be improved, send me a tweet @rabmyself

*/


(function(){


	$.fn.smint = function( options ) {

		var settings = $.extend({
			'scrollSpeed'  : 500
			//'mySelector'     : 'div'
		}, options);

		// adding a class to users div
		$(this).addClass('smint');


				
		
		//Set the variables needed
		var optionLocs = new Array(),
			lastScrollTop = 0,
			menuHeight = $(".smint").height(),
			smint = $('.smint'),
        	smintA = $('.smint a'),
        	myOffset = smint.height();


		if ( settings.scrollSpeed ) {
				var scrollSpeed = settings.scrollSpeed
			}

		//if ( settings.mySelector ) {
		//		var mySelector = settings.mySelector
		//};

	    //FIX: get length of a tags that are enable for smint
		    var length = smintA.not('.extLink').length;
	    //END_FIX

	    //FIX: get initial top of menu one time before fixed it to top / getting out of smintA loop
	    // get initial top offset for the menu 
		    var stickyTop = smint.offset().top;
	    //END_FIX


		return smintA.each( function(index) {
           
			var id = $(this).attr('href').split('#')[1];


		    //FIX: add rel attribute instead of id
			if (!$(this).hasClass("extLink")) {
				$(this).attr('rel', id);
			}
            //END_FIX
			
			//Fill the menu
			optionLocs.push(Array(
				$("."+id).position().top-menuHeight, 
				$("."+id).height()+$("."+id).position().top, id)
			);


			///////////////////////////////////

            // FIX: ***
		    // get initial top offset for the menu 
		    //.var stickyTop = smint.offset().top;	
		    // END FIX

			// check position and make sticky if needed
			var stickyMenu = function(direction){

				// current distance top
				var scrollTop = $(window).scrollTop()+myOffset; 

				// if we scroll more than the navigation, change its position to fixed and add class 'fxd', otherwise change it back to absolute and remove the class
				if (scrollTop > stickyTop+myOffset) { 
					smint.css({ 'position': 'fixed', 'top':0,'left':0 }).addClass('fxd');

					// add padding to the body to make up for the loss in heigt when the menu goes to a fixed position.
					// When an item is fixed, its removed from the flow so its height doesnt impact the other items on the page
					$('body').css('padding-top', menuHeight );	
				} else {
					smint.css( 'position', 'relative').removeClass('fxd'); 
					//remove the padding we added.
					$('body').css('padding-top', '0' );	
				}   

				// Check if the position is inside then change the menu
				// Courtesy of Ryan Clarke (@clarkieryan)
				if(optionLocs[index][0] <= scrollTop && scrollTop <= optionLocs[index][1]){	
				    if (direction == "up") {
				        //FIX: add rel attribute instead of id
					    $("[rel=" + id + "]").addClass("active");


					    // FIX: if is last child don't check for next sibling
					    if (index != length - 1) {
					        $("[rel=" + optionLocs[index + 1][2] + "]").removeClass("active");
					    }

					    // END_FIX

					}

					//FIX : check first child for activing first child in down direction and if is on first child don't check for previous sibling
					else if (index >= 0) {
					    //FIX: add rel attribute instead of id
					    $("[rel=" + id + "]").addClass("active");
					        if (index != 0)
					        {
					            $("#" + optionLocs[index - 1][2]).removeClass("active");
					        }
                        //END_FIX

					} else if (direction == undefined) {
					    //FIX: add rel attribute instead of id
					    $("[rel=" + id + "]").addClass("active");
                        //END_FIX
					}
					$.each(optionLocs, function(i){
						if(id != optionLocs[i][2]){
						    //FIX: add rel attribute instead of id
						    $("[rel=" + optionLocs[i][2] + "]").removeClass("active");
                            //END_FIX
						}
					});
				}
			};

			// run functions
			stickyMenu();

			// run function every time you scroll
			$(window).scroll(function() {
				//Get the direction of scroll
				var st = $(this).scrollTop()+myOffset;
				if (st > lastScrollTop) {
				    direction = "down";
				} else if (st < lastScrollTop ){
				    direction = "up";
				}
				lastScrollTop = st;
				stickyMenu(direction);

				// Check if at bottom of page, if so, add class to last <a> as sometimes the last div
				// isnt long enough to scroll to the top of the page and trigger the active state.

				if($(window).scrollTop() + $(window).height() == $(document).height()) {
	       			smintA.removeClass('active')
	       			$(".smint a:not('.extLink'):last").addClass('active')
	       			
				} else {
                    //FIX: for add active class to last child
   					//smintA.last().removeClass('active')
   				}
			});

			///////////////////////////////////////
        
        	$(this).on('click', function(e){
				// gets the height of the users div. This is used for off-setting the scroll so the menu doesnt overlap any content in the div they jst scrolled to
				var myOffset = smint.height();   

        		// stops hrefs making the page jump when clicked
				e.preventDefault();
				
				// get the hash of the button you just clicked
				var hash = $(this).attr('href').split('#')[1];

				

				var goTo =  $('.'+ hash).offset().top-myOffset;
				
				// Scroll the page to the desired position!
				$("html, body").stop().animate({ scrollTop: goTo }, scrollSpeed);
				
				// if the link has the '.extLink' class it will be ignored 
		 		// Courtesy of mcpacosy ‏(@mcpacosy)
				if ($(this).hasClass("extLink"))
                {
                    return false;
                }

			});	


			//This lets yo use links in body text to scroll. Just add the class 'intLink' to your button and it will scroll

			$('.intLink').on('click', function(e){
				var myOffset = smint.height();   

				e.preventDefault();
				
				var hash = $(this).attr('href').split('#')[1];

				if (smint.hasClass('fxd')) {
					var goTo =  $('.'+ hash).position().top-myOffset;
				} else {
					var goTo =  $('.'+ hash).position().top-myOffset*2;
				}
				
				$("html, body").stop().animate({ scrollTop: goTo }, scrollSpeed);

				if ($(this).hasClass("extLink"))
                {
                    return false;
                }

			});	
		});

	};

	$.fn.smint.defaults = { 'scrollSpeed': 500};
})(jQuery);
/*!
 * jQuery Cookie Plugin v1.4.0
 * https://github.com/carhartl/jquery-cookie
 *
 * Copyright 2013 Klaus Hartl
 * Released under the MIT license
 */
(function (factory) {
	if (typeof define === 'function' && define.amd) {
		// AMD
		define(['jquery'], factory);
	} else if (typeof exports === 'object') {
		// CommonJS
		factory(require('jquery'));
	} else {
		// Browser globals
		factory(jQuery);
	}
}(function ($) {

	var pluses = /\+/g;

	function encode(s) {
		return config.raw ? s : encodeURIComponent(s);
	}

	function decode(s) {
		return config.raw ? s : decodeURIComponent(s);
	}

	function stringifyCookieValue(value) {
		return encode(config.json ? JSON.stringify(value) : String(value));
	}

	function parseCookieValue(s) {
		if (s.indexOf('"') === 0) {
			// This is a quoted cookie as according to RFC2068, unescape...
			s = s.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, '\\');
		}

		try {
			// Replace server-side written pluses with spaces.
			// If we can't decode the cookie, ignore it, it's unusable.
			// If we can't parse the cookie, ignore it, it's unusable.
			s = decodeURIComponent(s.replace(pluses, ' '));
			return config.json ? JSON.parse(s) : s;
		} catch(e) {}
	}

	function read(s, converter) {
		var value = config.raw ? s : parseCookieValue(s);
		return $.isFunction(converter) ? converter(value) : value;
	}

	var config = $.cookie = function (key, value, options) {

		// Write

		if (value !== undefined && !$.isFunction(value)) {
			options = $.extend({}, config.defaults, options);

			if (typeof options.expires === 'number') {
				var days = options.expires, t = options.expires = new Date();
				t.setTime(+t + days * 864e+5);
			}

			return (document.cookie = [
				encode(key), '=', stringifyCookieValue(value),
				options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
				options.path    ? '; path=' + options.path : '',
				options.domain  ? '; domain=' + options.domain : '',
				options.secure  ? '; secure' : ''
			].join(''));
		}

		// Read

		var result = key ? undefined : {};

		// To prevent the for loop in the first place assign an empty array
		// in case there are no cookies at all. Also prevents odd result when
		// calling $.cookie().
		var cookies = document.cookie ? document.cookie.split('; ') : [];

		for (var i = 0, l = cookies.length; i < l; i++) {
			var parts = cookies[i].split('=');
			var name = decode(parts.shift());
			var cookie = parts.join('=');

			if (key && key === name) {
				// If second argument (value) is a function it's a converter...
				result = read(cookie, value);
				break;
			}

			// Prevent storing a cookie that we couldn't decode.
			if (!key && (cookie = read(cookie)) !== undefined) {
				result[name] = cookie;
			}
		}

		return result;
	};

	config.defaults = {};

	$.removeCookie = function (key, options) {
		if ($.cookie(key) === undefined) {
			return false;
		}

		// Must not alter options, thus extending a fresh object...
		$.cookie(key, '', $.extend({}, options, { expires: -1 }));
		return !$.cookie(key);
	};

}));


/*
    eDreamer CSS framework 2.5.0 (94/6/20): Global Java Script.
    Copyright 2014 eDreamer Design, all rights reserved.
    http://www.edreamer.ir
*/
var edreamer = {}; edreamer.device = function () { var n = $(".container").outerWidth(); return n == 1170 ? 4 : n == 970 ? 3 : n == 750 ? 2 : n < 750 ? 1 : null }; edreamer.deviceOS = function () { return navigator.platform.indexOf("iPhone") != -1 ? 2 : null }; edreamer.showOverlay = function (n) { $(".overlay").length == 0 && $("body").append("<div class='overlay'><\/div>"); $("body").css({ overflow: "hidden" }); (typeof n == "undefined" || n === null || n < 0 || n > 100) && (n = "50"); n == 100 ? $(".overlay").css({ opacity: "1" }) : $(".overlay").css({ opacity: "0." + n }); $(".overlay").fadeIn("slow") }; edreamer.hideOverlay = function () { $("body").css({ overflow: "auto" }); $(".overlay").stop().animate({ opacity: 0 }, "fast", function () { $(".overlay").remove() }) }; edreamer.dialogData = {}; edreamer.showDialog = function (n, t, i) { var r, u, e, o, f; if (!n) throw "'Content' is not defined!"; if (i = i || {}, edreamer.dialogData.closing = i.closing, edreamer.dialogData.closed = i.closed, r = n, u = !1, t === "inline" && (r = i.method !== "move" ? $(n).clone(!0, !0) : $(n), u = !$(n).data("openedBefore")), edreamer.dialogData.dialogContent = r, edreamer.dialogData.isFirstTime = u, i.opening && (e = i.opening(r, u), typeof e == "boolean" && !e)) { edreamer.dialogData = {}; return } (typeof i.overly == "undefined" || i.overly !== !1) && edreamer.showOverlay(i.overlyOpacity); $(".dialogbox").length !== 0 && $(".dialogbox").remove(); $("body").append("<div class='dialogbox'><div class='container'><\/div><\/div>"); i.customClass && $(".dialogbox").addClass(i.customClass); i.title && $(".dialogbox .container").append("<h1 class='title'>" + i.title + "<\/h1>"); i.okLabel = i.okLabel || "تایید"; i.cancelLabel = i.cancelLabel || "انصراف"; switch (t) { case "message": default: $(".dialogbox").addClass("message"); $(".dialogbox .container").append("<div class='content'><p>" + r + "<\/p><\/div><button class='ok button default focus'>" + i.okLabel + "<\/button>"); break; case "confirm": o = "<button class='cancel button'\">" + i.cancelLabel + "<\/button><button class='ok button default focus'\">" + i.okLabel + "<\/button>"; $(".dialogbox").addClass("confirm"); $(".dialogbox .container").append("<div class='content'><p>" + r + "<\/p><\/div>" + o); break; case "inline": $(".overlay").addClass("loading"); $(".dialogbox").addClass("inline"); r.length !== 0 ? (r.css("display", "block"), i.method === "move" ? (edreamer.dialogData.placeholder = $("<div class='dialog-placeholder'><\/div>").insertBefore(r), r.wrap("<div class='content'><\/div>").parent().appendTo(".dialogbox .container")) : r.wrap("<div class='content'><\/div>").parent().appendTo(".dialogbox .container")) : $(".dialogbox .container").append("<div class='content'><p>محتوی درخواستی یافت نشد!<\/p><\/div>"); break; case "reading": $(".overlay").addClass("loading"); $(".dialogbox").addClass("reading"); $(r).length !== 0 ? ($(".dialogbox .container").append("<div class='content'>" + $(r).wrap("<div>").parent().html() + "<\/div>"), $(r).unwrap("<div>")) : $(".dialogbox .container").append("<div class='content'><p>محتوی درخواستی یافت نشد!<\/p><\/div>"); break; case "url": $(".overlay").addClass("loading"); $(".dialogbox").addClass("url"); $(".dialogbox .container").append("<div class='content'><iframe frameborder='0' hspace='0' src='" + r + "' title='Dialog' scrolling='auto' id='dialogboxframe' name='dialogboxframe" + Math.round(Math.random() * 1e3) + "' onload='self.parent.edreamer.showiframecontent()' > <\/iframe><\/div>"); break; case "photo": $(".overlay").addClass("loading"); $(".dialogbox").addClass("photo"); $(".dialogbox .container").append("<div class='content'><img alt='" + i.title + "' src='" + r + "' /><\/div>"); break; case "video": $(".overlay").addClass("loading"); $(".dialogbox").addClass("video"); $(".dialogbox .container").append("<div class='content'><div class='video-container video-16by9'><video class='video-item' controls='controls' preload='auto' autoplay poster='/videos/poster.jpg'><source src='" + r + "' type='video/mp4' /><object class='video-item' type='application/x-shockwave-flash' data='http://releases.flowplayer.org/swf/flowplayer-3.2.1.swf'><param name='movie' value='http://releases.flowplayer.org/swf/flowplayer-3.2.1.swf' /><param name='allowFullScreen' value='true' /><param name='wmode' value='transparent' /><param name='flashVars' value=\"config={'playlist':['/videos/poster.jpg',{'url':'" + r + "','autoPlay':true,'autobuffering':true}]}\" /><\/object><\/video><\/div><\/div>"); break; case "audio": $(".overlay").addClass("loading"); $(".dialogbox").addClass("audio"); f = document.createElement("audio"); !!f.canPlayType && "no" != f.canPlayType("audio/mpeg") && "" != f.canPlayType("audio/mpeg") ? $(".dialogbox .container").append("<div class='content'><audio id='audioplayer' preload='auto' autoplay='autoplay' controls='controls'><source src='" + r + "' type='audio/mpeg'><\/audio>") : ($(".dialogbox .container").append("<div class='content'><div id='audioplayer'><\/div>"), AudioPlayer.setup("scripts/player.swf", { width: "100%", rtl: "yes", animation: "no" }), AudioPlayer.embed("audioplayer", { soundFile: r, titles: i.title, autostart: "yes" })) } if (i.icon != null && i.icon != "") switch (i.icon) { case "info": $(".dialogbox").addClass("info"); break; case "error": $(".dialogbox").addClass("error"); break; case "warning": $(".dialogbox").addClass("warning"); break; case "success": $(".dialogbox").addClass("success"); break; case "question": $(".dialogbox").addClass("question"); break; case "waiting": $(".dialogbox").addClass("waiting") } i.modal || ($(".dialogbox").after("<span class='cancel'><\/span>"), $(".overlay").click(function () { return edreamer.closeDialog(!1), !1 }), $(document).keyup(function (n) { return n.keyCode == 27 ? (edreamer.closeDialog(!1), !1) : !1 })); t == "reading" && ($(".dialogbox").after("<span class='plus' title='افزودن اندازه'><\/span>"), $(".dialogbox").after("<span class='minus' title='کاهش اندازه'><\/span>"), $(".dialogbox").after("<span class='night' title='سوئیچ به حالت شب'><\/span>"), $(".dialogbox").after("<span class='day' title='سوئیچ به حالت روز'><\/span>"), $("span.plus").click(function () { return curentSize = parseInt($(".dialogbox.reading .content *").css("font-size")) + 2, curentSize < 26 && $(".dialogbox.reading .content *").css("font-size", curentSize), !1 }), $("span.minus").click(function () { return curentSize = parseInt($(".dialogbox.reading .content *").css("font-size")) - 2, curentSize >= 12 && $(".dialogbox.reading .content *").css("font-size", curentSize), !1 }), $("span.night").click(function () { return $("span.day").fadeIn(), $("span.night").fadeOut(), $(".dialogbox").addClass("night"), $.cookie("nightmodeEnabled", "1", { expires: 365, path: "/" }), !1 }), $("span.day").click(function () { return $("span.day").fadeOut(), $("span.night").fadeIn(), $(".dialogbox").removeClass("night"), $.removeCookie("nightmodeEnabled", { expires: 365, path: "/" }), !1 }), $.cookie("nightmodeEnabled") == 1 && ($("span.day").show(), $("span.night").hide(), $(".dialogbox").addClass("night"))); $("span.cancel, .dialogbox .cancel").click(function () { return edreamer.closeDialog(!1), !1 }); $(".dialogbox .ok").click(function () { return edreamer.closeDialog(!0), !1 }); window.onresize = function () { edreamer.centerDialog() }; i.width != "" && i.width != null && ($(".dialogbox").width(i.width), $(".dialogbox").css("max-width", "none"), $(".dialogbox > .container").css("width", "100%")); i.height != "" && i.height != null && ($(".dialogbox").height(i.height), $(".dialogbox").css("max-height", "none"), $(".dialogbox > .container").css("width", "100%")); edreamer.centerDialog(); $(".overlay").removeClass("loading"); $(".dialogbox").fadeIn("slow", "linear", function () { t === "inline" && $(n).data("openedBefore", !0); i.opened && i.opened(r, u) }); $(".dialogbox .focus").focus(); $(".dialogbox").hasClass("photo") && $(".dialogbox .content img").width() < $(".container").width() && ($(".dialogbox.photo .container").css({ width: "auto" }), edreamer.centerDialog()) }; edreamer.centerDialog = function () { $(".dialogbox").css({ "max-height": $(window).height() / 1 }); $(".dialogbox").css({ "margin-left": -$(".dialogbox").width() / 2 }); $(".dialogbox").css({ "margin-top": -$(".dialogbox").height() / 2 }) }; edreamer.closeDialog = function (n) { var i, t; return edreamer.dialogData.closing && (i = edreamer.dialogData.closing(n, edreamer.dialogData.dialogContent, edreamer.dialogData.isFirstTime), typeof i == "boolean" && !i) ? null : ($(".dialogbox").hasClass("message") || $(".dialogbox").hasClass("confirm") ? $(".dialogbox").fadeOut("slow", "linear", function () { $(".dialogbox").remove(); edreamer.dialogData.closed && edreamer.dialogData.closed(n, edreamer.dialogData.isFirstTime); edreamer.dialogData = {} }) : (t = edreamer.dialogData.placeholder, $(".dialogbox").animate({ top: "+=100%", opacity: 0 }, "normal", function () { if (t) { var i = $(".dialogbox .container .content > :first-child"); i.css("display", "none"); i.insertAfter(t); t.remove() } $(".dialogbox").remove(); edreamer.dialogData.closed && edreamer.dialogData.closed(n, edreamer.dialogData.isFirstTime); edreamer.dialogData = {} })), edreamer.hideOverlay(), $("span.cancel").remove(), $("span.plus").remove(), $("span.minus").remove(), $("span.night").remove(), $("span.day").remove(), $(".overlay").unbind("click"), $(window).unbind("resize"), $(document).unbind("keyup"), n) }; edreamer.showIframeContent = function () { $("#dialogboxframe").fadeIn("fast"); edreamer.centerDialog() }; edreamer.hideBlocks = function (n, t) { n.each(function () { $(this).offset().top > $(window).scrollTop() + $(window).height() * t && $(this).addClass("state-a") }) }; edreamer.showBlocks = function (n, t) { n.each(function () { $(this).offset().top <= $(window).scrollTop() + $(window).height() * t && $(this).hasClass("state-a") && $(this).removeClass("state-a").addClass("state-b") }) }; edreamer.setUIPreference = function () { if ($.each($.cookie(), function (n, t) { n.match("^#") && t == 1 && $(n).addClass("collapsed") }), edreamer.device() != 1) { var n = $(".animate"), t = .8; edreamer.hideBlocks(n, t); $(window).on("scroll", function () { window.requestAnimationFrame ? window.requestAnimationFrame(function () { edreamer.showBlocks(n, t) }) : setTimeout(function () { edreamer.showBlocks(n, t) }, 100) }) } }; $(document).ready(function () { function t() { return $(".thumbnail.inlineview article").removeClass("selected"), $(".thumbnail.inlineview .inlineview-container").remove(), $(window).unbind("resize"), $(document).unbind("keyup"), !0 } var r, u, f, i, n; $("a.photo").click(function () { return edreamer.showDialog($(this).attr("href"), "photo", { title: $(this).attr("title") }), !1 }); $("a.video").click(function () { return edreamer.showDialog($(this).attr("href"), "video", { title: $(this).attr("title") }), !1 }); $("a.audio").click(function () { return edreamer.showDialog($(this).attr("href"), "audio", { title: $(this).attr("title") }), !1 }); $(".readingmode-trigger").click(function () { return edreamer.showDialog($(".readingmode"), "reading", { title: document.title }), !1 }); edreamer.device() !== 1 ? $(".status, [class^='status-']").delay(1e3).slideDown("slow") : $(".status, [class^='status-']").show(); $(".status a.close, [class^='status-'] a.close").click(function () { return $(this).parent().slideUp("slow"), !1 }); $("#header li.more").click(function () { return $(this).find("ul").fadeIn("fast"), $(document).bind("click", function () { $("#header li.more > ul").fadeOut("slow"); $(document).unbind("click") }), !1 }); $("#header li.search").click(function () { return edreamer.showOverlay(), $(".userbar .search").fadeIn("fast"), $(".userbar .search input").focus(), $(".overlay").click(function () { return $(".userbar .search a").trigger("click"), !1 }), !1 }); $(".userbar .search a").click(function () { return edreamer.hideOverlay(), $(".userbar .search").fadeOut("slow"), !1 }); $("#navigation li:has(ul)").doubleTapToGo(); $(".sidebar nav li:has(ul)").doubleTapToGo(); $(".thumbnail article > a").doubleTapToGo(); $(".actions > li").length > 2 && $(".actions").doubleTapToGo(); $(".slider > ul").length && $(".slider > ul").responsiveSlides({ pager: !0, nav: !0, pause: !0, prevText: "<span>قبلی<\/span>", nextText: "<span>بعدی<\/span>" }); $("#tab1 > ul:last").length && $("#tab1 > ul:last").responsiveSlides({ auto: !1, pager: !0, nav: !1, manualControls: "#tab1 > ul:first" }); $("#tab2").length && $("#tab2 > ul:last").responsiveSlides({ auto: !1, pager: !0, nav: !1, manualControls: "#tab2 > ul:first" }); edreamer.device() == 1 && $(".slider > ul.rslides > li").each(function () { $(this).attr("data-mobile-image") != undefined && $(this).css("background-image", "url('" + $(this).attr("data-mobile-image") + "')") }); $(".panel .header").click(function () { var t = $(this).parent().parent(), n = $(this).parent(); return t.hasClass("megapanel") && t.find(".panel").addClass("collapsed"), n.hasClass("collapsed") ? (n.removeClass("collapsed"), n.attr("id") && $.removeCookie("#" + n.attr("id"), { expires: 365, path: "/" }), !1) : (n.addClass("collapsed"), n.attr("id") && $.cookie("#" + n.attr("id"), "1", { expires: 365, path: "/" }), !1) }); edreamer.device() == 1 && $("#footer h5").click(function () { var n = $(this).parent(); return n.hasClass("collapsed") ? (n.removeClass("collapsed"), n.attr("id") && $.removeCookie("#" + n.attr("id"), { expires: 365, path: "/" }), !1) : (n.addClass("collapsed"), n.attr("id") && $.cookie("#" + n.attr("id"), "1", { expires: 365, path: "/" }), !1) }); $(".external").click(function () { return window.open(this.href), !1 }); window.location.hash != "" && (r = $("." + window.location.hash.replace("#", "")), r && (u = $(r), u.length != 0 && (f = u.offset().top, $("html,body").stop().animate({ scrollTop: f }, 1e3)))); $("a[href*=#].jumper").click(function () { var n, t; return location.pathname.replace(/^\//, "") == this.pathname.replace(/^\//, "") && location.hostname == this.hostname && this.hash.length != 0 && (n = $(this.hash), n = n.length && n || $("." + this.hash.slice(1)), n.length) ? (t = n.offset().top, $("html,body").stop().animate({ scrollTop: t }, 1e3), !1) : !1 }); edreamer.device() != 1 && $("#navigation-onepage").length && $("#navigation-onepage").smint({ scrollSpeed: 1e3 }); $(".nav-trigger").click(function () { return $(this).hasClass("back") ? (history.back(), !1) : $("body").hasClass("offcanvas") ? ($("body").removeClass("offcanvas"), $(".overlay").unbind("click"), document.onkeyup = "", edreamer.hideOverlay(), !1) : ($("body").addClass("offcanvas"), edreamer.showOverlay(), $(".overlay").click(function () { return $(".nav-trigger").click(), !1 }), document.onkeyup = function (n) { var t; return (t = n == null ? event.keyCode : n.which, t == 27) ? ($(".nav-trigger").click(), !1) : !1 }, !1) }); $(".nav-offcanvas-trigger").click(function () { return $("html").hasClass("offcanvas") ? ($("html").removeClass("offcanvas"), $(".overlay").unbind("click"), document.onkeyup = "", edreamer.hideOverlay(), !1) : ($("html").addClass("offcanvas"), edreamer.showOverlay(25), $(".overlay").click(function () { return $(".nav-offcanvas-trigger").click(), !1 }), document.onkeyup = function (n) { var t; return t = n == null ? event.keyCode : n.which, t == 27 && $(".nav-offcanvas-trigger").click(), !1 }, !1) }); i = document.createElement("audio"); !!i.canPlayType && "no" != i.canPlayType("audio/mpeg") && "" != i.canPlayType("audio/mpeg") || (AudioPlayer.setup("scripts/player.swf", { width: "100%", rtl: "yes", animation: "no" }), AudioPlayer.embed("audioplayer1", { soundFile: "videos/audio.mp3", autostart: "yes" })); $(".tooltip").hover(function () { var n = $(this).attr("title"), t, i; $(this).data("tipText", n).removeAttr("title"); t = $(this).offset().top + ($(this).outerHeight() + 30); $('<div class="tooltip-container"><\/div>').text(n).appendTo("body"); i = $(this).offset().left - ($(".tooltip-container").outerWidth() - $(this).outerWidth()) / 2; $(".tooltip-container").css({ top: t, left: i }); $(".tooltip-container").animate({ opacity: 1, top: "-=15" }) }, function () { $(this).attr("title", $(this).data("tipText")); $(".tooltip-container").remove() }); $(".debug").dblclick(function () { $('link[rel=stylesheet][href~="stylesheets/debug.css"]').remove() }); n = $(".category > ul"); n.length != 0 && (n[0].scrollWidth > n.innerWidth() && n.find("li.more").addClass("visible"), n.scroll(function () { n.find("li.more").hide(); n.onscroll = "" })); $(".map").click(function () { $(".map iframe").css("pointer-events", "auto") }); $(".map").mouseleave(function () { $(".map iframe").css("pointer-events", "none") }); $(".list.comment .contents .reply").click(function () { return $(this).toggleClass("opened"), $(this).parent().parent().next().slideToggle("slow"), !1 }); $("#header").hasClass("autohide") && $(window).scroll({ previousTop: 0 }, function () { if ($("body").hasClass("offcanvas") == !1 && $("#header").hasClass("extended") == !1 && $(".actions").hasClass("active") == !1) { var n = $(window).scrollTop(); n < this.previousTop ? ($("#header").removeClass("outview"), $("ul.actions").removeClass("outview"), $(".feedback").length && $(".feedback").removeClass("outview")) : ($("#header").addClass("outview"), $("ul.actions").addClass("outview"), $(".feedback").length && $(".feedback").hasClass("expanded") == !1 && $(".feedback").addClass("outview")); $("#header li.more > ul").css("display") === "block" && ($("#header li.more > ul").fadeOut("slow"), $(document).unbind("click")); this.previousTop = n; n <= 0 && ($("#header").removeClass("outview"), $("ul.actions").removeClass("outview")) } }); $(".actions").hover(function () { return $(".actions > li.trigger").hide(), $(".actions").addClass("active"), $(".actions > li").length > 2 ? (edreamer.showOverlay(), $("body").css({ overflow: "auto" }), $(".overlay").css({ "z-index": "98" }), $(this).find("li").not(":last-child").stop().fadeIn()) : $(this).find("li").not(":last-child").show(), !1 }, function () { return $(".actions > li").length > 2 && edreamer.hideOverlay(), $(this).find("li").not(":last-child").stop().hide(), $(".actions").removeClass("active"), $(".actions > li.trigger").show(), !1 }); $("span.validator").on({ mouseenter: function () { return $(this).find("span").stop().attr("style", "display:block !important").css({ opacity: "0", top: "-65px" }).animate({ opacity: 1, top: "-35px" }, "normal"), !1 }, mouseleave: function () { return $(this).find("span").stop().animate({ opacity: 0, top: "-=10px" }, "slow"), !1 } }); if (edreamer.device() === 1) $("form").on({ focus: function () { return $(this).next(".validator").find("span").attr("style", "display:block !important").css({ opacity: "0", top: "-65px" }).stop().animate({ opacity: 1, top: "-35px" }, "normal"), !1 }, blur: function () { return $(this).next(".validator").find("span").stop().animate({ opacity: 0, top: "-=10px" }, "slow"), !1 } }, ".form-group.error .form-control, .form-group.warning .form-control, .form-group.success .form-control"); $(".thumbnail.inlineview article > .contents").on("click", function () { var r = $(this).parent().find(".inlineview"); if (r.length != 0) { if ($(this).parent().hasClass("selected")) return t(), !1; t(); $(this).parent().addClass("selected"); var n = 4, u = 1, o = $(this).parent().parent().find("article").length, s = $(this).parent().index() + 1; switch (edreamer.device()) { default: case 4: case 3: n = 4; break; case 2: n = 3; break; case 1: n = 1 } u = Math.ceil(s / n); u * n < o ? $(this).parent().parent().find("article:nth-of-type(" + u * n + ")").after("<div class='inlineview-container clearfix'><span class='close' title='بستن'><\/span>" + r.html() + "<\/div>") : $(this).parent().parent().find("article:last-child").after("<div class='inlineview-container clearfix'><span class='close' title='بستن'><\/span>" + r.html() + "<\/div>"); $("div.inlineview-container").fadeIn(); var i = Math.round($(this).offset().top), f = Math.round($(window).scrollTop()), e = 0; edreamer.device() != 1 && $("#navigation-onepage").length != 0 && (e = $("#navigation-onepage").height()); f = f + e; i != f && (i = i - e, $("html,body").stop().animate({ scrollTop: i }, 1e3)); $(".inlineview-container .close").on("click", function () { return t(), !1 }); return $(window).bind("resize", function () { t() }), $(document).keyup(function (n) { n.keyCode == 27 && t() }), !1 } return !0 }); $(".feedback h2").click(function () { return $(this).parent().hasClass("expanded") ? ($(this).parent().removeClass("expanded"), edreamer.hideOverlay(), $(this).next(".contents").toggle("slow"), $(".overlay").unbind("click"), $(window).unbind("resize"), document.onkeyup = "") : ($(this).parent().addClass("expanded"), edreamer.showOverlay(), $(this).next(".contents").toggle("slow").css({ "max-height": $(window).height() / 1 - $(this).outerHeight() }), $(window).bind("resize", function () { $(".feedback .contents").css({ "max-height": $(window).height() / 1 - $(".feedback > h2").outerHeight() }) }), $(".feedback .contents input:text").first().focus(), $(".overlay").click(function () { return $(".feedback h2").click(), !1 }), document.onkeyup = function (n) { var t; return t = n == null ? event.keyCode : n.which, t == 27 && $(".feedback h2").click(), !1 }), !1 }); edreamer.showSnackbar = function (n, t, i, r, u) { $(".snackbar").length != 0 && $(".snackbar").remove(); $("body").append("<div class='snackbar'><div class='contents'><\/div><\/div>"); u != null && u != "" && $(".snackbar").prepend("<a title=" + r + " href=" + i + "><img alt=" + r + " src=" + u + " /><\/a>"); n != null && n != "" && $(".snackbar .contents").append("<h2 class='snackbar-title'>" + n + "<\/h2>"); t != null && t != "" && $(".snackbar .contents").append("<p>" + t + "<\/p>"); i != null && i != "" && r != null && r != "" && $(".snackbar .contents").append("<a class='snackbar-link' title=" + r + " href=" + i + ">" + r + "<\/a>"); var f = $(".snackbar").outerHeight(); edreamer.device() == 1 ? $(".snackbar").css({ bottom: -f }).animate({ opacity: 1, bottom: "+0px" }, "slow").delay("3500").animate({ opacity: 1, bottom: -f }, "normal") : $(".snackbar").css({ bottom: -f }).animate({ opacity: 1, bottom: "+30px" }, "slow").delay("3500").animate({ opacity: 0, bottom: -f }, "normal") }; $(".help-trigger").click(function () { return edreamer.showDialog($(this).next(".help-contents"), "inline", { customClass: "help" }), !1 }); $(".help-feedback .yes").click(function () { return $(".help-feedback-question").slideUp(), $(".help-feedback-thanks").slideDown(), $(".help-feedback").delay("2500").slideUp(), !1 }); $(".help-feedback .no").click(function () { return $(".help-feedback-question").slideUp(), $(".help-feedback-form").slideDown(), $(".help-feedback-form textarea").focus(), !1 }); $(".links-signin").click(function () { return edreamer.showDialog($(".user-signin"), "inline", { title: $(".user-signin").find("legend").text(), width: 400, height: 340, customClass: "signin" }), !1 }); $(".links-signup").click(function () { return edreamer.showDialog($(".user-signup"), "inline", { title: $(".user-signup").find("legend").text(), width: 450, height: 480, customClass: "signup" }), !1 }); $(".pager .print a").click(function () { return parent.print(), !1 }); $(".pager .back a").click(function () { return history.back(), !1 }); edreamer.setUIPreference() }); window.console.info("eDreamer CSS Framework 2.0, Copyright 2015 eDreamer Design, all rights reserved. http://www.royagar.com");