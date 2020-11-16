var main = document.getElementById("main");
var editor = document.getElementById("editor");
var editorContainer = document.getElementById("editorContainer");
var editorResult = document.getElementById("editorResult");
var editorResultContainer = document.getElementById("editorResultContainer");
var body = document.body,
	html = document.documentElement;
var menuHeight = document.getElementById("containerMenu").offsetHeight;
var downloadCode = document.getElementById("downloadCode");
var run = document.getElementById("run");
var dragbar = document.getElementById("dragbar");
var iframeResult = document.getElementById("iframeResult");
var dimensions = document.getElementById("dimensions");
var selectedTheme = localStorage.getItem("codeMirrorSelectedTheme");
var selectedView = localStorage.getItem("codeMirrorSelectedView");
var editor = CodeMirror.fromTextArea(document.getElementById("editor"), {
	mode: "htmlmixed",
	theme: getEditorTheme(),
	lineNumbers: true,
	indentUnit: 4,
	extraKeys: {
		"Tab": function (cm) {
			cm.replaceSelection("    ", "end")
		}
	}
});
if (!selectedTheme) {
	localStorage.setItem("codeMirrorSelectedTheme", "white")
}
if (!selectedView) {
	localStorage.setItem("codeMirrorSelectedView", "cols")
}

function runCode() {
	var iframe = iframeResult;
	iframe = iframe.contentWindow ? iframe.contentWindow : iframe.contentDocument ? iframe.contentDocument.document : iframe.contentDocument;
	iframe.document.open();
	iframe.document.write(editor.getValue());
	iframe.document.close()
}

function loadAds() {
	var AD_HTML = '<script async src="https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>';
	if ($(window).width() >= 970) {
		AD_HTML += '<!-- Web Editor 970 x 90 -->' + '<ins class="adsbygoogle" style="display:inline-block;width:970px;height:90px" data-ad-client="ca-pub-1613148428630404" data-ad-slot="5975336166"></ins>'
	} else if ($(window).width() >= 728) {
		AD_HTML += '<!-- Web Editor 728 x 90 -->' + '<ins class="adsbygoogle" style="display:inline-block;width:728px;height:90px" data-ad-client="ca-pub-1613148428630404" data-ad-slot="7092976712"></ins>'
	} else if ($(window).width() >= 468) {
		AD_HTML += '<!-- Web Editor 468 x 59 -->' + '<ins class="adsbygoogle" style="display:inline-block;width:468px;height:59px" data-ad-client="ca-pub-1613148428630404" data-ad-slot="7113142103"></ins>'
	} else if ($(window).width() >= 320) {
		AD_HTML += '<!-- Web Editor 320 x 50 -->' + '<ins class="adsbygoogle" style="display:inline-block;width:320px;height:50px" data-ad-client="ca-pub-1613148428630404" data-ad-slot="4103835381"></ins>'
	}
	AD_HTML += '<script>(adsbygoogle = window.adsbygoogle || []).push({});</script>';
	$("#ads").html(AD_HTML)
}

function screenAdjustment() {
	var pageHeight = html.offsetHeight;
	var pageWidth = html.offsetWidth;
	var menuHeight = document.getElementById("containerMenu").offsetHeight;
	if ($(window).width() >= 728) {
		$("#header").height(140);
		$("#ads").height(92);
		$("#container").css("padding-top", "130px")
	} else if ($(window).width() >= 468) {
		$("#header").height(110);
		$("#ads").height(61);
		$("#container").css("padding-top", "100px")
	} else {
		$("#header").height(101);
		$("#ads").height(52);
		$("#container").css("padding-top", "91px")
	}
	$("#main").height($(body).height() - $("#header").height());
	if (selectedView == "rows") {
		editorContainer.style.marginBottom = "5px";
		editorContainer.style.width = "100%";
		editorResultContainer.style.width = "100%";
		editorContainer.style.height = main.offsetHeight / 2 - 10 + "px";
		editorResultContainer.style.height = main.offsetHeight / 2 - 5 + "px";
		$("#dragbar").css({
			cursor: "row-resize",
			top: $("#editorResultContainer").position().top - 5,
			left: 0,
			width: "100%",
			height: 5,
		})
	} else {
		editorContainer.style.marginRight = "5px";
		editorContainer.style.width = main.offsetWidth / 2 - 5 + "px";
		editorResultContainer.style.width = main.offsetWidth / 2 - 10 + "px";
		editorContainer.style.height = "100%";
		editorResultContainer.style.height = "100%";
		$("#dragbar").css({
			cursor: "col-resize",
			top: 0,
			left: $("#editorResultContainer").position().left - 5,
			height: "100%",
			width: 5,
		})
	}
	editor.setSize("100%", "100%");
	main.style.visibility = "visible";
	dimensions.style.display = $(window).width() < 500 ? "none" : "block";
	displayDimensions()
}

function displayDimensions() {
	dimensions.innerHTML = editorResult.offsetWidth + " × " + editorResult.offsetHeight
}
$(document).ready(function () {
	$("#downloadCode").click(function () {
		var textFile = null,
			makeTextFile = function (text) {
				var data = new Blob([text], {
					type: "text/plain"
				});
				if (textFile !== null) {
					window.URL.revokeObjectURL(textFile)
				}
				textFile = window.URL.createObjectURL(data);
				return textFile
			};
		var create = document.getElementById("downloadCode");
		a = document.createElement("a");
		document.body.appendChild(a);
		a.style = "display: none";
		a.href = makeTextFile(editor.getValue());
		a.setAttribute("download", "harmash_webeditor.html");
		a.click()
	});
	$("#run").click(function () {
		runCode()
	});
	$("#toggleView").click(function () {
		localStorage.setItem("codeMirrorSelectedView", selectedView == "rows" ? "cols" : "rows", );
		editor.setOption("codeMirrorSelectedView", localStorage.getItem("codeMirrorSelectedView"), );
		selectedView = localStorage.getItem("codeMirrorSelectedView");
		screenAdjustment()
	});
	$("#toggleTheme").click(function () {
		localStorage.setItem("codeMirrorSelectedTheme", selectedTheme == "white" ? "dark" : "white", );
		editor.setOption("theme", getEditorTheme());
		selectedTheme = localStorage.getItem("codeMirrorSelectedTheme")
	});
	$("#dragbar");
	screenAdjustment();
	runCode()
});
$(window).load(function () {
	loadAds()
});

function getEditorTheme() {
	if (localStorage.getItem("codeMirrorSelectedTheme") == "white") return "default";
	else return "panda-syntax"
}

function dragElement(elmnt) {
	var pos1 = 0,
		pos2 = 0,
		pos3 = 0,
		pos4 = 0;
	if (document.getElementById(elmnt.id + "header")) {
		document.getElementById(elmnt.id + "header", ).onmousedown = dragMouseDown
	} else {
		elmnt.onmousedown = dragMouseDown
	}

	function dragMouseDown(e) {
		$("#iframeFix").css("display", "block");
		e = e || window.event;
		e.preventDefault();
		pos3 = e.clientX;
		pos4 = e.clientY;
		document.onmouseup = closeDragElement;
		document.onmousemove = elementDrag
	}

	function elementDrag(e) {
		e = e || window.event;
		e.preventDefault();
		pos1 = pos3 - e.clientX;
		pos2 = pos4 - e.clientY;
		pos3 = e.clientX;
		pos4 = e.clientY;
		if (selectedView == "rows") {
			if (pos4 < $("#header").height() + 30 || pos4 > body.offsetHeight - 30) {
				return
			}
			elmnt.style.top = elmnt.offsetTop - pos2 + "px";
			elmnt.style.left = "0px";
			editorContainer.style.height = elmnt.offsetTop - 5 + "px";
			editorResultContainer.style.height = main.offsetHeight - elmnt.offsetTop - 10 + "px"
		} else {
			if (pos3 < 40 || pos3 > body.offsetWidth - 30) {
				return
			}
			elmnt.style.top = "0px";
			elmnt.style.left = elmnt.offsetLeft - pos1 + "px";
			editorContainer.style.width = elmnt.offsetLeft - 5 + "px";
			editorResultContainer.style.width = main.offsetWidth - elmnt.offsetLeft - 10 + "px"
		}
		displayDimensions()
	}

	function closeDragElement() {
		$("#iframeFix").css("display", "none");
		document.onmouseup = null;
		document.onmousemove = null
	}
}
dragElement(dragbar);
