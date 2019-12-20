<%@ Control Language="vb" AutoEventWireup="false" Src="UpImgPrev.ascx.vb" Inherits="UpImgPrev" %>

<script language="JavaScript">

// VALORES: 0-Recien creada, 1-Previsualizada, 2-Editado	

  var selected = '';
  
  function IniciImg() 
  {
  	AfegirElement(0);
  }
  
  function Previsualitzar(num)
  {
    

    var filename = document.getElementById('imgfitxer'+num).value;
	
    if (!esImatge(filename)) {
        var nuevoFile = document.createElement('input');
		nuevoFile.setAttribute('type','file');
		var nodaux = document.getElementById('imgfitxer' + num);
		var parentnodaux = nodaux.parentNode;
		parentnodaux.replaceChild(nuevoFile, document.getElementById('imgfitxer' + num));
		nuevoFile.setAttribute('id','imgfitxer' + num);
		nuevoFile.setAttribute('name', 'imgfitxer' + num);
		if ( nuevoFile.attachEvent ){
			nuevoFile.attachEvent("onchange", function changeaux(){ Previsualitzar(num);});
		}else{
			nuevoFile.addEventListener("change", function changeaux(){ Previsualitzar(num); }, false);
		}
        alert("Error: L'arxiu introduït no és una imatge. Per arxius que nos són imatges utilitzi l'apartat de documents.");
        return;
    }
    
    var imgC = document.getElementById('ImgCount');
	imgC.value = parseInt(imgC.value) + 1;
	

    var Img = new Image();
	//alert("file:\/\/" + filename + "1");
  	var fileOk = filename.replace(/\\/g,"/");
	
    Img.src = "file:/\\" + fileOk;
	//alert(Img.src);
	
    document.getElementById('imgprev'+num).src = Img.src.replace("///","//");
	//alert(document.getElementById('imgprev'+num).src);
	document.getElementById('imgprev'+num).style.display = 'inline'; 
	document.getElementById('imgfitxer'+num).style.display = 'none';
	document.getElementById('ImgbElim'+num).style.display = 'inline';
	document.getElementById('ImgbEdit'+num).style.display = 'inline';
	document.getElementById('ImgTitol'+num).style.display = 'inline';
	document.getElementById('ImgAlt'+num).style.display = 'inline';
	document.getElementById('ImgTextAlt'+num).style.display = 'inline';
	document.getElementById('ImgTextTit'+num).style.display = 'inline';
	document.getElementById('ImgbElime'+num).style.display = 'none';

	if (document.getElementById('ImgbDesfer' + num)) document.getElementById('ImgbDesfer' + num).style.display = 'none';	
	
	if(document.getElementById('imgstate'+num).value == 0)
	{
		AfegirElement(0);
	}
	
	document.getElementById('imgstate'+num).value = 1;
 
  }
  
  function esImatge(filename)
  {
    var path = filename.toUpperCase();
	var posExt = path.lastIndexOf(".");
	var Ext = path.substr(posExt);
	switch(Ext)
	{
	  case ".GIF":
		case ".SWF":
		case ".BMP":
		case ".PNG":
		case ".JPG":
		case ".TIFF":
		case ".RAW":
		case ".JPEG":
		case ".MP4":
	
		    return true;
		    break;
		default:
	}
	return false;
  }   
  
  function AfegirElement(pos, b)
  {
  //Si pos = 0, se inserta al final, sino pos es la posicion a insertar
	var capa = document.getElementById('ImgPrevDiv');
	var imgwid = document.getElementById('imgwid');
	var num;
	if(pos == 0){
		num = parseInt(imgwid.value);
		var nuevoDiv = document.createElement('div');
		var divNom = 'imgr'+num;
		nuevoDiv.setAttribute('id',divNom);
		nuevoDiv.innerHTML ='<table style="display:inline-block" width="auto" cellpadding="5"><tr>'+
	'<td valign="top" style="margin-right:5px;"><input type=hidden id="imgstate'+num+'" value=0 />'+
	'<input type="hidden" id="imgpos'+num+'" name="imgpos'+num+'" value='+num+' />'+
	'<img id="imgprev'+num+'" onclick="seleccion(\'' + divNom + '\')" style="display:none;" width="110" height="110" onclick="seleccion(' + nuevoDiv.id + ')" />'+	
	
	'<input type="button" style="display:none;" id="ImgbElime'+num+'" value="Eliminar" onClick="EliminarElement('+num+')" />'+
	'</td>'+
	'<td valign="top" width="450">'+			
	'<a id="ImgTextTit'+num+'" style="display:none;" class="txtNeg12px">'+
	'<strong>Títol: </strong><span class="gris878787">(Descripció breu de la imatge. Només s´ha de posar quan aquesta imatge no sigui merament decorativa)</a>'+
	'</span><br>'+
	'<input type="text" style="display:none; width:450px;" id="ImgTitol'+num+'" name="ImgTitol'+num+'" onclick="seleccion(\'' + nuevoDiv.id + '\')" /><br>'+	
	'<a id="ImgTextAlt'+num+'" style="display:none;" class="txtNeg12px">'+
	'<strong>Descripció llarga: </strong><span class="gris878787">(Només s&rsquo;ha d&rsquo;omplir quan la imatge, a més de no ser merament decorativa, conté informació de text o numèrica. Caldrà descriure la informació que apareix a la imatge)</span>'+
	'</a><br>'+
	'<input type="text" style="display:none; width:450px;" id="ImgAlt'+num+'" name="ImgAlt'+num+'" onclick="seleccion(\'' + nuevoDiv.id + '\')" />'+
	
	'<input type="button" style="display:none; margin-right:5px" id="ImgbEdit'+num+'" value="Canviar imatge" onClick="AfegirElement('+num+')" />'+
	'<input type="file" multiple="multiple" id="imgfitxer'+num+'" name="imgfitxer'+num+'" onChange="Previsualitzar('+num+')"/>'+		
	'<input type="button" style="display:none;" id="ImgbElim'+num+'" value="Eliminar" onClick="EliminarElement('+num+')" />'+
	
	'</td><td width="100">&nbsp;</td>'+
	'</tr></table>';
		capa.appendChild(nuevoDiv);
		imgwid.value = parseInt(num) + 1;
	}else{
		num = pos;
		if (b == 1) {
			document.getElementById('ImgbDesfer' + num).style.display = 'inline';
		}
		var nuevoFile = document.createElement('input');
		nuevoFile.setAttribute('type','file');
		var nodaux = document.getElementById('imgfitxer' + num);
		var parentnodaux = nodaux.parentNode;
		parentnodaux.replaceChild(nuevoFile, document.getElementById('imgfitxer' + num));
		nuevoFile.setAttribute('id','imgfitxer' + num);
		nuevoFile.setAttribute('name', 'imgfitxer' + num);
		if ( nuevoFile.attachEvent ){
			nuevoFile.attachEvent("onchange", function changeaux(){ Previsualitzar(num);});
		}else{
			nuevoFile.addEventListener("change", function changeaux(){ Previsualitzar(num); }, false);
		}
		var imgC = document.getElementById('ImgCount');
		imgC.value = parseInt(imgC.value) - 1 ;
		document.getElementById('imgstate'+num).value = 2;
		document.getElementById('imgprev'+num).style.display = 'none'; 
		document.getElementById('imgfitxer'+num).style.display = 'inline';
		document.getElementById('imgfitxer'+num).style.marginBottom = '5px';
		document.getElementById('imgfitxer'+num).style.marginRight = '5px';
		document.getElementById('ImgbElim'+num).style.display = 'inline';
		document.getElementById('ImgbElim'+num).style.marginRight = '5px';
		document.getElementById('ImgbEdit'+num).style.display = 'none';
		document.getElementById('ImgbEdit'+num).style.marginRight = '5px';
		document.getElementById('ImgTitol'+num).style.display = 'inline';
		document.getElementById('ImgAlt'+num).style.display = 'inline';
		document.getElementById('ImgTextAlt'+num).style.display = 'inline';
		document.getElementById('ImgTextTit'+num).style.display = 'inline';
		document.getElementById('ImgbElime'+num).style.display = 'none';
		document.getElementById('ImgbElime'+num).style.marginRight = '5px';
	}
	deseleccionar();
  }
  
  function EliminarElement(num)
  {
  	var conf = confirm("Està segur que desitja eliminar la imatge?");
	if ( conf )
	{
  		var capa = document.getElementById('ImgPrevDiv');
		var imgC = document.getElementById('ImgCount');
		var imgP = document.getElementById('imgr'+num);
			
		if(document.getElementById('imgstate'+num).value != 2)
		{	
			imgC.value = parseInt(imgC.value) - 1 ;
		}
		deseleccionar();
 		var divPos = document.getElementById('imgdivPos');
		capa.appendChild(divPos);
		capa.removeChild(imgP);
	}
  }
  
  //---------------------------------------------------------------------
  //------Funciones de Orden---------------------------------------------
  //---------------------------------------------------------------------

function seleccion(objeto)
{
	if ( document.getElementById(objeto) == null ) return;
	if ( document.getElementById('imgstate' + objeto.substring(4, objeto.length)).value == 0) return;

	deseleccionar();
	
	if (selected != objeto){
	 	selected = objeto;
	 	document.getElementById(objeto).style.borderWidth = '2px';
		document.getElementById(objeto).style.backgroundColor = 'white';
	 	document.getElementById(objeto).style.borderColor = '#D6DDE5';
	 	document.getElementById(objeto).style.borderStyle = 'Solid';
		document.getElementById(objeto).style.marginBottom = '15px';
		document.getElementById(objeto).style.padding = '5px';
		document.getElementById(objeto).style.display = 'block';
	 
	 	var divPos = document.getElementById('imgdivPos');
	 	divPos.style.display = 'block';
		divPos.style.top = '-155px';
		if ( document.getElementById('imgfitxer' + objeto.substring(4, objeto.length)).style.display != 'none'){
		    divPos.style.display = 'none';
		}	 	
		document.getElementById(objeto).appendChild(divPos);
	}
}

function deseleccionar()
{
	if (selected != '')	
		{
			if ( document.getElementById(selected) != null )
			{
				document.getElementById(selected).style.borderWidth = '0';
				document.getElementById(selected).style.backgroundColor = 'transparent';
	 	document.getElementById(selected).style.borderColor = '#D6DDE5';
	 	document.getElementById(selected).style.borderStyle = 'Solid';
		document.getElementById(selected).style.marginBottom = '15px';
		document.getElementById(selected).style.padding = '5px';
							}
			document.getElementById('imgdivPos').style.display = 'none';
		}
	selected = "";	
}


function desferCanviImg(imgBx){
    document.getElementById('imgprev' + imgBx).style.display = 'inline'; 
    document.getElementById('ImgbDesfer' + imgBx).style.display = 'none';
    document.getElementById('ImgbEdit' + imgBx).style.display = 'inline';
    document.getElementById('imgfitxer' + imgBx).style.display = 'none';
    var imgbox = document.getElementById('imgbox' + imgBx);
    if (imgbox) imgbox.selectedIndex = imgbox.title;
}

function cambiarOrden(b)
{
	var dv1 = document.getElementById(selected);
	var papa = document.getElementById('ImgPrevDiv');
	var dv2;
	if (b == 'up'){
		dv2 = document.getElementById(selected).previousSibling;
		
		while(dv2 != null && dv2.tagName != 'DIV'){
			dv2 = dv2.previousSibling;}
		if ( dv2 != null )
		{
			//CAMBIAR ORDEN
			var aux = document.getElementById('imgpos' + dv1.id.substring(4, dv1.id.length)).value;
			document.getElementById('imgpos' + dv1.id.substring(4, dv1.id.length)).value = document.getElementById('imgpos' + dv2.id.substring(4, dv2.id.length)).value;
			document.getElementById('imgpos' + dv2.id.substring(4, dv2.id.length)).value = aux;
			
			papa.removeChild(dv1);
			papa.insertBefore(dv1,dv2);
		}
	}else if (b == 'down'){
  		dv2 = document.getElementById(selected).nextSibling;
		
		while(dv2 != null && dv2.tagName != 'DIV'){
			dv2 = dv2.nextSibling;}
		
		if ( dv2 != null )
		{
			//CAMBIAR ORDEN
			var aux = document.getElementById('imgpos' + dv1.id.substring(4, dv1.id.length)).value;
			document.getElementById('imgpos' + dv1.id.substring(4, dv1.id.length)).value = document.getElementById('imgpos' + dv2.id.substring(4, dv2.id.length)).value;
			document.getElementById('imgpos' + dv2.id.substring(4, dv2.id.length)).value = aux;
			
			papa.removeChild(dv2);
			papa.insertBefore(dv2,dv1);
		}
	}
}
 
</script>
<div id="ImgPrevDiv" style="width:100%; display:block; position:relative; overflow:hidden">
    <div id="imgdivPos" style="display:none; position:relative; float:right; right:0; top:-155px; width:100px;">
        <span class="bold paddingBottom10 txtNeg12px">Canvi d'ordre</span><br />
        <a onclick="cambiarOrden('up')" class="txtNeg12px" style="cursor:pointer; text-decoration:none;"><img src="/img/gaia/arriba.png" onclick="cambiarOrden('up')" align="absmiddle" style="margin-right:.5em;">Pujar</a><br />
        <a onclick="cambiarOrden('down')" class="txtNeg12px" style="cursor:pointer; text-decoration:none;"><img src="/img/gaia/abajo.png" onclick="cambiarOrden('down')" align="absmiddle" style="margin-right:.5em;">Baixar</a>
    </div>
    <input type="hidden" id="imgwid" value="<%=imgwid%>" />
    <input type="hidden" id="ImgCount" value="<%=imgCount%>" />
    <%=dispHTML%>
</div>
