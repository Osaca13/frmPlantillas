if (window.attachEvent) window.attachEvent("onload", IniciaAjuda);
else if (window.addEventListener) window.addEventListener("load", function (){IniciaAjuda2 (window)}, false);	

function IniciaAjuda() {
	
	//per explorer
	var Ajudes = GetElementsWithClassName("div", "Ajuda");

	var NumeroAjudes = Ajudes.length;
	for (var i = 0; i < NumeroAjudes; i++) {
		var AjudaActual = Ajudes[i];
		var EnllacActual = AjudaActual.getElementsByTagName("a")[0];
		EnllacActual.attachEvent("onmouseover", AjudaEnllac_OnMouseOver);
		EnllacActual.attachEvent("onmouseout", AjudaEnllac_OnMouseOut);
	}
}


function IniciaAjuda2() {
	//per chrome, firefox, etc..
	var Ajudes = GetElementsWithClassName("div", "Ajuda");
	var NumeroAjudes = Ajudes.length;
	for (var i = 0; i < NumeroAjudes; i++) {
		var AjudaActual = Ajudes[i];
		var EnllacActual = AjudaActual.getElementsByTagName("a")[0];
		EnllacActual.addEventListener("mouseover", function (){AjudaEnllac_OnMouseOver()},false);
		EnllacActual.addEventListener("mouseout", function (){AjudaEnllac_OnMouseOut()},false);

	}
}

function AjudaEnllac_OnMouseOver() {
	var AjudaActual = window.event.srcElement.parentNode.parentNode;
	addClass(AjudaActual, "AjudaVisible");
	
	var DivActual = AjudaActual.getElementsByTagName("div")[0];
	addClass(DivActual, "Visible");
}

function AjudaEnllac_OnMouseOut() {
	var AjudaActual = window.event.srcElement.parentNode.parentNode;
	removeClass(AjudaActual, "AjudaVisible");
	
	var DivActual = AjudaActual.getElementsByTagName("div")[0];
	removeClass(DivActual, "Visible");
}

function hasClass(object, className) {
	if (!object.className) return false;
	return (object.className.search('(^|\\s)' + className + '(\\s|$)') != -1);
}

function removeClass(object,className) {
	if (!object) return;
	object.className = object.className.replace(new RegExp('(^|\\s)'+className+'(\\s|$)'), RegExp.$1+RegExp.$2);
}

function addClass(object,className) {
	if (!object || hasClass(object, className)) return;
	if (object.className) {
		object.className += ' '+className;
	} else {
		object.className = className;
	}
}

function GetElementsWithClassName(elementName,className) {
	var allElements = document.getElementsByTagName(elementName);
	var elemColl = new Array();
	for (var i = 0; i< allElements.length; i++) {
		if (hasClass(allElements[i], className)) {
			elemColl[elemColl.length] = allElements[i];
		}
	}
	return elemColl;
}