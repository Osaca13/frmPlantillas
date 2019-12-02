<identity impersonate="true"/>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmestructura.aspx.vb" Inherits="frmPlantillasTest.frmestructura" ValidateRequest="false" Debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

<title>Intranet de l'Ajuntament de L'Hospitalet</title>

<style>
body, html {height:100%;}
</style>
<link rel="stylesheet" href="/css/intranet.css" type="text/css">
<link rel="stylesheet" href="/css/gaiaIntranet.css" type="text/css">
<script type="text/javascript" src="js/jquery-1.10.2.js"></script>
</head>

<body bgcolor="#FFFFFF" text="#000000" leftmargin="10" topmargin="10" onLoad="document.getElementById('divMain').style.visibility='visible';document.getElementById('divWait').style.visibility='hidden';" >
<div id="divWait" style="visibility:hidden; vertical-align:middle; width:100%; margin:0 auto; color:#990000; height:100%; position:absolute; background-color:#fff; top:150;">
	<center><img src="img/reloj.gif"  alt="Treballant"></center>
</div>


<form id="frm" runat="server">

<div id="divMain"  style="visibility:visible"  >
<asp:textbox id="WEBDSTCO" width=0 runat="server"   style="visibility:hidden"></asp:textbox>
<asp:textbox id="tipusNode" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="ultimaDivisio" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="ultimaPlantilla" width=0 runat="server"  style="visibility:hidden"></asp:textbox>

<asp:textbox id="nroArbreOrigen" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="nroArbreDesti" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="nroNodeOrigen" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="nroNodeDesti" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="nodePathVell" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="nodePathNou" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="nroNodePareAnterior" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="codiRelacioOrigen" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="txtposicioEstructura"  width=0 runat="server"  style="visibility:hidden" ></asp:textbox>
<asp:textbox id="moureFills" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="codiRelacioDesti" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="txtDragDrop" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="txtPosicioEstructuraReal"  width=0  runat="server" /> 
<asp:textbox id="codiRelacioDestiInicial" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="llistaPlantilles" width=0 runat="server" style="visibility:hidden"></asp:textbox>
<asp:textbox id="nomTaula" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
<asp:textbox id="ubicacionsSeleccionables"  width=0 value="0" runat="server"   style="visibility:hidden" ></asp:textbox>


 <asp:panel runat="server" visible="false" id="pnlError" >
        <div class="missatgeErrorIntranet"> 
            <div class="topEsquerraBlanc"></div>
            <div class="topDretaBlanc"></div>
                <div class="icona"><asp:literal runat="server" id="ltErr"></asp:literal></div>
            <div class="bottomEsquerraBlanc"></div>
            <div class="bottomDretaBlanc"></div>
        </div>
	</asp:panel>
    
    
    
    <asp:panel runat="server" visible="true" id="pnlEditar">
<table>
	<tr>
		<td width="50%" valign="top">
			<table width="100%" border="1" cellspacing="1" cellpadding="1"   class="tablabordepeqroj" bordercolor="#FFCDCA">
			  <tr>
				<td bgcolor="#E6E6E6" class="txtrojo12px">Quin tipus de moviment
				  desitges fer?</td>
			  </tr>
			  <tr>
				<td height="1"><asp:RadioButtonList ID="accio" runat="server" CssClass="txtNeg12px" OnSelectedIndexChanged="canviarNodeDesti" AutoPostBack="true">
					<asp:ListItem  runat="server" Text="A dins del node seleccionat" Value="insertar" selected   />
					<asp:ListItem  runat="server" Text="Al mateix nivell del node seleccionat" Value="moure"></asp:ListItem>
				  </asp:RadioButtonList>
				
				</td>
			  </tr>
			  <tr>
				<td bgcolor="#E6E6E6" class="txtrojo12px">Moure a la posici&oacute;: </td>			  
			  </tr>
			  <tr>
				<td>&nbsp;&nbsp;
				  <asp:ListBox ID="lstOrdre" runat="server" Rows="1">
				  <asp:ListItem Value="0">Últim de la llista</asp:ListItem>
				  <asp:ListItem Value="1">Primer de la llista</asp:ListItem>    
				  <asp:ListItem Value="3">Ordre alfabètic</asp:ListItem>
					  <asp:ListItem Value="2" >A sota de la posició seleccionada</asp:ListItem>
				</asp:ListBox></td>			  
			  </tr>
			  <tr id="trPlantilla1" style="display:none">
				<td colspan="2" bgcolor="#E6E6E6" class="txtrojo12px">Plantilla a utilizar:</td>
			  </tr>
			  <tr id="trPlantilla2" style="display:none">
				<td colspan="2" width="100%" height="1" >&nbsp;&nbsp;
				<asp:PlaceHolder runat="server" id="plantillesPH"/>
				</td>
			  </tr>  
			</table>
			<table runat="server" id="tbDates" visible="false" width="100%" border="1" cellspacing="1" cellpadding="1"   class="tablabordepeqroj" bordercolor="#FFCDCA">
			  <tr>
				<td colspan="2" bgcolor="#E6E6E6" class="txtrojo12px">Publicació / Caducitat</td>
			  </tr>
			  <tr runat="server" id="tbDates_cat"  visible = "false">
				<td class="txtneg12px">				
					<table>
						<tr>
							<td class="txtneg12px"><asp:RequiredFieldValidator ID="REIDTPUBReqVal"
										ControlToValidate="REIDTPUB"
										enabled="false"
										ErrorMessage="Cal indicar la data de publicació"
										EnableClientScript="true"
										Display="None"
										InitialValue="" runat=server></asp:RequiredFieldValidator>
									<asp:CompareValidator ID="REIDTCADCompareVal" runat="server" Display="None"
										ErrorMessage="La data de caducitat ha de ser posterior a la data de publicació" 
										EnableClientScript="true"
										enabled="false"
										ControlToValidate="REIDTCAD" ControlToCompare="REIDTPUB" Type="Date"
										Operator="GreaterThan"></asp:CompareValidator>
									<asp:RangeValidator id="REIDTPUBrangeValDate"
										Type="Date"
										EnableClientScript="true"
										ControlToValidate="REIDTPUB"
										ErrorMessage="Format incorrecte de la data de publicaci&oacute; (DD/MM/AAAA)"
										Display="None"
										enabled="false"
										MaximumValue="1/1/2100"
										MinimumValue="1/1/1900"
										runat="server"/><span class="txtrojo12px">* </span>Data publicació:</td>
							<td>
								<asp:TextBox runat="server"  ID="REIDTPUB" Columns="10" MaxLength="10"></asp:TextBox>                        
								<a href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=REIDTPUB','calendar_window','width=150,height=188');calendar_window.focus()"><img src="/img/common/iconografia/ico_calendari.png" border="0" align="top"></a>
							</td>
						</tr>
						<tr>
							<td class="txtNeg12pxFF" align="left">&nbsp;&nbsp;Hora publicaci&oacute;:
							</td>
							<td class="txtNeg12px" align="left"><asp:TextBox runat="server"  ID="horaPublicacio" Columns="5" MaxLength="5"></asp:TextBox>&nbsp;(hh:mm)
							</td>
							<td class="txtNeg12pxFF" align="left">&nbsp;</td>				
						  </tr>
						<tr>
							<td class="txtneg12px">
								&nbsp;&nbsp;Data caducitat:
							</td>
							<td>
								<asp:TextBox runat="server"  ID="REIDTCAD" Columns="10" MaxLength="10"></asp:TextBox>                        
								<a href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=REIDTCAD','calendar_window','width=150,height=188');calendar_window.focus()"><img src="/img/common/iconografia/ico_calendari.png" border="0" align="top"></a>
							</td>
						</tr>
						<tr>
							<td class="txtNeg12pxFF" align="left">&nbsp;&nbsp;Hora caducitat:
							</td>
							<td class="txtNeg12px" align="left"><asp:TextBox runat="server"  ID="horaCaducitat" Columns="5" MaxLength="5"></asp:TextBox>&nbsp;(hh:mm)
							</td>
							<td class="txtNeg12pxFF" align="left">
							
							</td>
							
						</tr>			
					</table>
				</td>
			  </tr>
			 
			</table>
             <asp:panel runat="server" id="pnlVisibilitat" visible="true">
            <table  width="100%" border="1" cellspacing="1" cellpadding="2"   class="tablabordepeqroj" bordercolor="#FFCDCA">
			
              <tr>
				<td colspan="2" bgcolor="#E6E6E6" class="txtrojo12px" style="padding:5">Visibilitat del contingut</td>
			  </tr>
			  <tr class="TRvisible" >
				<td class="txtneg12px" style="padding:5">		
                <asp:label id="lblVisible" runat="server"/> <asp:CheckBox runat="server" id="chkVisibleInternet" />
                </td></tr></table>
                </asp:panel>
		</td>
<% IF lblEstructura.text.length>0 THEN %>
				<script>
					top.resizeTo(800,600);
				</script>
		<td width="50%">
			<table width="100%" border="1" cellspacing="1" cellpadding="1"   class="tablabordepeqroj" bordercolor="#FFCDCA" >
			  <tr>
				<td colspan="4" bgcolor="#E6E6E6" class="txtrojo12px"><asp:label runat="server" id="lblTitol"></asp:label>
				  . Selecciona l'ubicaci&oacute; del contingut
				 </td>
			  </tr>
			  <tr>
				<td height="156"><asp:label runat="server" id="lblEstructura"></asp:label>     </td>
			  </tr>
			</table> 
		</td>
<% END IF%>
	</tr>	
</table>	
</asp:panel>
<div align="center">
<asp:panel runat="server" id="pnlBotonsOk">
	  <input type=button id="btnInsert"  value="Acceptar"  runat="server"  OnServerClick="clickModificaEstructura"  onClick="if (comprovarUbicacioSeleccionada()) { document.getElementById('divMain').style.visibility='hidden';document.getElementById('divWait').style.visibility='visible' } else { return(false)}">	

		 <input type=button id="btnCancelar"  value="Cancel·lar"  runat="server"  OnServerClick="clickCancelar">
</asp:panel>
<asp:panel runat="server" id="pnlBotonsSensePermis"  visible="false">   
	 <input type=button id="btnTornar"  value="Tornar"  runat="server"  OnServerClick="clickCancelar">
</asp:panel>      
  </div>

 <asp:label id="lblCodi" runat="server"/>

  <script language="javascript">
	
	var aPlantilles = (document.getElementById('llistaPlantilles').value).split(",");
	iniPantalla(aPlantilles);
	if (document.getElementById("WEBDSTCO").value!="")
	{
		var arrayTCO = document.getElementById("WEBDSTCO").value.split(",");
			var cont=0;
			//alert( arrayTCO.length);
		while (cont < arrayTCO.length) {		

			if ((arrayTCO[cont]!=document.getElementById("tipusNode").value) && (arrayTCO[cont]!=54)) {			
				document.getElementById("t"+cont).disabled=true;
				document.getElementById("t"+cont).bgColor="#eeeeee";
			}
			else {	
				document.getElementById("ubicacionsSeleccionables").value=1;
				document.getElementById("t"+cont).disabled=false;
				if (document.getElementById("txtPosicioEstructuraReal").value==cont) {
					document.getElementById("t"+cont).bgColor="#CACAAA";
					document.getElementById("ultimaDivisio").value="t"+cont;
					document.getElementById("ultimaPlantilla").value=aPlantilles[cont];
					if (aPlantilles[cont]) {
						mostrarDesplegable(aPlantilles[cont],"t"+cont);
					}
				}
				else {
					document.getElementById("t"+cont).bgColor="#ffffff";
				}
			}
			cont++;
		}
	}
	else
	{
		var ncelda=document.getElementById("txtposicioEstructura").value;
		if (ncelda >= 0 && document.getElementById('llistaPlantilles').value != "") {
			document.getElementById("ultimaDivisio").value="t"+ncelda;		
			document.getElementById("ultimaPlantilla").value=aPlantilles[ncelda];
			if (aPlantilles[ncelda]) {
				mostrarDesplegable(aPlantilles[ncelda],"t"+ncelda);	
			}
		}
	}
	
	function iniPantalla(aPlantilles) {
		//var aPlantilles = (document.getElementById('llistaPlantilles').value).split(",");
		for (i=0;i<aPlantilles.length;i++) {
				document.getElementById('ddlb_plantillat' + i).style.display='none';
		}
	}
	
	// aquesta funció existeix només per compatibilitat amb frmplantillaV2.asx
	function activaCamps(activar) {
		return true;
	}
	function comprovarUbicacioSeleccionada() {
		if ((document.getElementById("ubicacionsSeleccionables").value >= 1) && ((document.getElementById("txtposicioEstructura").value==-1) && (document.getElementById("txtposicioEstructuraReal").value==-1) ))
		{
			if (!confirm("No hi ha cap cel·la seleccionada i el contingut no serà visible. Vols continuar?")) 
			{ 
				return(false); 
			  
			} 
		} 
		return(true);
	}
	
	function seleccionaCelda(celda,nomDivisio)
	{	

		if (document.getElementById(celda).disabled==false) {

			if  (document.getElementById("ultimaDivisio").value!="") {	
						
					var ultimaDivisio=document.getElementById("ultimaDivisio").value;

					document.getElementById(ultimaDivisio).bgColor="#FFFFFF";
					//amaguem el desplegable corresponent a la cel·la anterior
					if (document.getElementById('ddlb_plantilla' + ultimaDivisio)) {
						document.getElementById('ddlb_plantilla' + ultimaDivisio).style.display='none';
					}
			}											
			document.getElementById("txtposicioEstructura").value=celda.substring(1,celda.length);
			document.getElementById(celda).bgColor="#CACAAA";


			document.getElementById("ultimaDivisio").value=celda;
			var aPlantilles = (document.getElementById('llistaPlantilles').value).split(",");

			document.getElementById('ultimaPlantilla').value=aPlantilles[celda.substring(1)];

			if (aPlantilles[celda.substring(1)]) {
				mostrarDesplegable(aPlantilles[celda.substring(1)],celda);
			}
		}
	}
	
	function mostrarDesplegable(sPlantilla,celda) {
		if (sPlantilla.indexOf("|") >= 0) {
			document.getElementById('trPlantilla1').style.display='';
			document.getElementById('trPlantilla2').style.display='';
			document.getElementById('ddlb_plantilla' + celda).style.display='';
		}
		else
		{
			document.getElementById('trPlantilla1').style.display='none';
			document.getElementById('trPlantilla2').style.display='none';
		}	
	}
	
	$.fn.multiline = function(text){
    this.text(text);
    this.html(this.html().replace(/\n/g,'<br/>'));
    return this;
}
	$(document).ready(function(){
		 if (document.getElementById('chkVisibleInternet').checked) {
		 		$('.TRvisible').css("background-color", "#92ce07");
				$("#lblVisible").multiline("El contingut podria ser visible en alguna pàgina de web o intranet.\nDesmarca el camp per canviar-ho:")	
			}
			else {
		 		$('.TRvisible').css("background-color", "#f48484");
				$("#lblVisible").multiline("El contingut podria no ser visible en alguna pàgina de web o intranet.\nMarca el camp per canviar-ho:")
			}
		  $('.TRvisible').click(function(){
			if (document.getElementById('chkVisibleInternet').checked) {
				$("#lblVisible").multiline("El contingut podria ser visible en alguna pàgina de web o intranet.\nDesmarca el camp per canviar-ho:")			
		 		$(this).css("background-color", "#92ce07");
			}
			else {
				$("#lblVisible").multiline("El contingut podria no ser visible en alguna pàgina de web o intranet.\nMarca el camp per canviar-ho:")
		 		$(this).css("background-color", "#f48484");
			}
			
		  }); 

	});

</script>

</div>
</form>

</body>
</html>


