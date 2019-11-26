function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload !== 'function') {
        window.onload = func;
    } else {
        window.onload = function () {
            if (oldonload) {
                oldonload();
            }
            func();
        };
    }
}

addLoadEvent(function () { 
/* more code to run on page load */ 
});
function senseJS(elemento)
{

    if (document.removeChild) 
    {
		
        var div = document.getElementById(elemento);
		try {
        	div.parentNode.removeChild(div);
		}
		catch(err) {
		}
		
    }  
    else if (document.getElementById)
    {
			
        document.getElementById(elemento).style.display = "none"; 
    }
	
}