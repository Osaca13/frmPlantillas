<identity impersonate="true"/>
<%@ Page Language="vb" AutoEventWireup="false" ValidateRequest="false" EnableViewStateMac="false" Debug ="true" CodeBehind="frmFullaWeb.aspx.vb" Inherits="frmPlantillasTest.frmFullaWeb" %>

<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Pàgina web</title>

<link rel="stylesheet" href="../../Styles/intranet.css" type="text/css">
<link rel="stylesheet" href="../../Styles/gaiaIntranet.css" type="text/css">
<link href="css/Ajuda.css" rel="stylesheet" type="text/css"/>
<script language="JavaScript" type="text/javascript" src="js/Ajuda.js"></script>
</head>

<body bgcolor="#FFFFFF" text="#000000" leftmargin="10" topmargin="10">

<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<%@ Register TagPrefix="lh" TagName="Ajuda" Src="/js/App_LocalResources/Ajuda.ascx" %>
<!--#INCLUDE VIRTUAL="../../js/App_LocalResources/cap.inc" -->


  <script type="text/javascript">
	function obrePreview() {
		window.open("/GAIA/aspx/web/frmCSSPreview.aspx?estructura="+document.getElementById("estructura").value+"&thor="+document.getElementById("llistaTHor").value+"&tver="+document.getElementById("llistaTVer").value+"&tcontinguts="+document.getElementById("llistaTipusContinguts").value+"&llistaPlantilles="+document.getElementById("llistaNodes").value+"&llistaDescripcions="+document.getElementById("llistaDescripcions").value+"&autolink=0");
		
	}
	function activaCamps(activar)
	{
		if (activar) {
			document.getElementById('chkimp').disabled=false;		
			document.getElementById('chkCND').disabled=false;			
			document.getElementById('tVer').disabled=false;
			document.getElementById('tHor').disabled=false;
			document.getElementById('btnDivHor').disabled=false;
			document.getElementById('btnDivVer').disabled=false;
			document.getElementById('btnPlantilles').disabled=false;
			
			document.getElementById('tipusContingut').disabled=false;
			
	
						
			document.getElementById('btnDivEsb').disabled=false;
			//ini codi sara
			document.getElementById('WEBDSDEC').disabled=false;
			//fi codi sara
			document.getElementById('btnEditaPlantilles').disabled=false;	
			document.getElementById('btnEditaCodis').disabled=false;
		}
		else {
			document.getElementById('tVer').disabled=false;
			document.getElementById('tHor').disabled=false;
			document.getElementById('btnDivHor').disabled=true;			
			document.getElementById('btnPlantilles').disabled=true;
			
			document.getElementById('btnDivVer').disabled=true;
			document.getElementById('btnDivEsb').disabled=true;	
			//ini codi sara
			document.getElementById('WEBDSDEC').disabled=false;
			//fi codi sara		
			document.getElementById('btnEditaPlantilles').disabled=true;
			document.getElementById('btnEditaCodis').disabled=true;
		}
		return false;
	}
	function seleccionaCelda(celda,nomDivisio)
	{
	
			if  (document.getElementById("ultimaDivisio").value!="") {				
				var ultimaDivisio=document.getElementById("ultimaDivisio").value;
			}
			else {
				var ultimaDivisio="";
			}
			var arrayLlistaCSS2
			var arraychkimp = document.getElementById("llistachkimp").value.split(",");
			var arraychkCND = document.getElementById("llistachkCND").value.split(",");
			var arrayTVer = document.getElementById("llistaTVer").value.split(",");
			var arrayTHor = document.getElementById("llistaTHor").value.split(",");
			var arrayLlistaTipusContinguts = document.getElementById("llistaTipusContinguts").value.split(",");
			var arrayLlistaPlantilles = document.getElementById("llistaPlantilles").value.split("|");
			var arrayLlistaCodis= document.getElementById("llistaCodis").value.split("|");
			var arrayLlistaNodes = document.getElementById("llistaNodes").value.split(",");
			var arrayLlistaNodes2= document.getElementById("llistaNodes2").value.split(",");
			var arrayLlistaCSS = document.getElementById("llistaCSS").value.split("|");	
			//ini codi sara
			var arrayLlistaDescripcions = document.getElementById("llistaDescripcions").value.split("|");
			
			//fi codi sara
			var llistachkimp="";
			var llistachkCND="";			
			var llistaTVer="";
			var llistaTHor="";
			var llistaTipusContinguts="";
			var llistaNodes="";
			var llistaNodes2="";
			var llistaPlantilles="";
			var llistaCodis ="";
			var llistaCSS="";
			//ini codi sara
			var llistaDescripcions="";
			
			//fi  codi sara
			 
	
			if  (ultimaDivisio!="") {				
				if (celda.length==0) {
					celda = ultimaDivisio;
				}
				indexCelda = celda.substring(1);
				document.getElementById(ultimaDivisio).bgColor="";		
					
				if (document.getElementById("chkimp").checked) {
					arraychkimp[ultimaDivisio.substring(1)]='1';
				}
				else {
					arraychkimp[ultimaDivisio.substring(1)]='0';
				}
				if (document.getElementById("chkCND").checked) {
					arraychkCND[ultimaDivisio.substring(1)]='1';
				}
				else {
					arraychkCND[ultimaDivisio.substring(1)]='0';
				}


				index = ultimaDivisio.substring(1);
				
				arrayTVer[ultimaDivisio.substring(1)]=document.getElementById("tVer").value;
				arrayTHor[ultimaDivisio.substring(1)]=document.getElementById("tHor").value;
				
				arrayLlistaPlantilles[ultimaDivisio.substring(1)]=document.getElementById("gaiaPlantillaWebTxt").value;
				arrayLlistaNodes[ultimaDivisio.substring(1)]=document.getElementById("gaiaPlantillaWebNodes").value;
				arrayLlistaNodes2[ultimaDivisio.substring(1)]=document.getElementById("gaiaCodiWebNodes").value;
				arrayLlistaTipusContinguts[ultimaDivisio.substring(1)]=document.getElementById("tipusContingut")[document.getElementById("tipusContingut").selectedIndex].value;
				//arrayLlistaCSS[ultimaDivisio.substring(1)]=document.getElementById("estilsCSS")[document.getElementById("estilsCSS").selectedIndex].value;
			
			
				arrayLlistaCSS[index] = "";
				var arrayTipusCSS=new Array("23","24","25","26","27","28","103","105","108","109","110","111","112","122","114","115","123","117","118","119","124");
				for (i=0;i<arrayTipusCSS.length;i++) {
					if (i>0) {
						arrayLlistaCSS[index]+=",";
					}					
					arrayLlistaCSS[index] = arrayLlistaCSS[index] + document.getElementById("ddlb_" + arrayTipusCSS[i])[document.getElementById("ddlb_" + arrayTipusCSS[i]).selectedIndex].value ;				
				}
								
								
								
							
				arrayLlistaCodis[ultimaDivisio.substring(1)]=document.getElementById("gaiaCodiWebTxt").value;	
				//ini codi sara
				arrayLlistaDescripcions[ultimaDivisio.substring(1)]=document.getElementById("WEBDSDEC").value;				
				//fi codi sara				
			}
			var indexCelda=0;
			var valorMaxim=0;
			var cont=0;
			indexCelda = celda.substring(1);
				// 1. actualitzo la llista amb el valor que hi ha a "mida vertical" i "mida horitzontal"
				while (cont < arrayTVer.length) {
					if (cont>0) {
						llistachkimp+=",";
						llistachkCND+=",";						
						llistaTVer+=",";	
						llistaTHor+=",";
						llistaCSS+="|";
						llistaPlantilles+="|";
						llistaCodis+="|";
						llistaNodes+=",";
						llistaNodes2+=",";						
						llistaTipusContinguts+=",";
						//ini codi sara
						llistaDescripcions+="|";
						
						//fi codi sara
					}
					llistachkimp+=arraychkimp[cont];
					llistachkCND+=arraychkCND[cont];					
					llistaTVer+=arrayTVer[cont];
					llistaTHor+=arrayTHor[cont];
					llistaCSS+=arrayLlistaCSS[cont];
					llistaPlantilles+=arrayLlistaPlantilles[cont];
					llistaCodis+=arrayLlistaCodis[cont];
					llistaNodes+=arrayLlistaNodes[cont];
					llistaNodes2+=arrayLlistaNodes2[cont];					
					llistaTipusContinguts+=arrayLlistaTipusContinguts[cont];
					//ini codi sara
					llistaDescripcions+=arrayLlistaDescripcions[cont];
					
					//fi codi sara				
					cont++;
				}
				
				if (arraychkimp[indexCelda]=='0') {
					document.getElementById("chkimp").checked=false;
				}
				else {
					document.getElementById("chkimp").checked=true;
				}
				
				if (arraychkCND[indexCelda]=='0') {
					document.getElementById("chkCND").checked=false;
				}
				else {
					document.getElementById("chkCND").checked=true;
				}
				// 2. actualitzo el valor de "mida vertical" i "horitzontal" amb el valor de la llista apuntat per la divisió seleccionada	
				
				

				document.getElementById("tVer").value=arrayTVer[indexCelda];
				document.getElementById("tHor").value=arrayTHor[indexCelda];				
				document.getElementById("gaiaPlantillaWebNodes").value=arrayLlistaNodes[indexCelda];
				document.getElementById("gaiaPlantillaWebTxt").value=arrayLlistaPlantilles[indexCelda];
				document.getElementById("gaiaCodiWebNodes").value=arrayLlistaNodes2[indexCelda];
				document.getElementById("gaiaCodiWebTxt").value=arrayLlistaCodis[indexCelda];
				//ini codi sara
				document.getElementById("WEBDSDEC").value=arrayLlistaDescripcions[indexCelda];
				//fi codi sara
				cont=0
				while (cont < document.getElementById("tipusContingut").length) {
					if (arrayLlistaTipusContinguts[indexCelda]==" ") {
						index=0;
					}
					else {
						index = arrayLlistaTipusContinguts[indexCelda];
					}					
					if (document.getElementById("tipusContingut")[cont].value==index) {
						document.getElementById("tipusContingut")[cont].selected=true;
						cont =  document.getElementById("tipusContingut").length;
					}
					cont++;
				}
				
				//Poso els valors selected de cadascun del dropdownlist d'estils
				arrayLlistaCSS2= arrayLlistaCSS[indexCelda].split(",");
				var itemCSS ="";
				var arrayTipusCSS=new Array("23","24","25","26","27","28","103","105","108","109","110","111","112","122","114","115","123","117","118","119","124")
				for (var i=0;i<arrayTipusCSS.length;i++) {
					cont=0;
					objecte = document.getElementById('ddlb_' + arrayTipusCSS[i]);
					valorMaxim=objecte.length;
					while (cont < valorMaxim) {
						if (objecte[cont].value==arrayLlistaCSS2[i]) {
							objecte[cont].selected=true;
							cont =  valorMaxim;
						}
						cont++;
					}
				}				
			
				document.getElementById("llistachkimp").value=llistachkimp;
				document.getElementById("llistachkCND").value=llistachkCND;				
				document.getElementById("llistaTVer").value=llistaTVer;
				document.getElementById("llistaTHor").value=llistaTHor;				
				document.getElementById("llistaPlantilles").value=llistaPlantilles;								
				document.getElementById("llistaCodis").value=llistaCodis;
				document.getElementById("llistaTipusContinguts").value=llistaTipusContinguts;
				document.getElementById("llistaCSS").value=llistaCSS;
				document.getElementById("llistaNodes").value=llistaNodes;
				document.getElementById("llistaNodes2").value=llistaNodes2;
				//ini codi sara
				document.getElementById("llistaDescripcions").value=llistaDescripcions;
				
				//fi codi sara
				
				document.getElementById("ultimaDivisio").value=celda;
				document.getElementById(celda).bgColor="#CACAAA";
				if (nomDivisio.length>0) {
					document.getElementById("divSel").value=nomDivisio;
				}
								
			return false;
	}
</script>


<form runat="server" class="padding0 margin0">
<%@ Register TagPrefix="menuG" TagName="menuG" Src="../../js/App_LocalResources/menu.ascx" %>
<menuG:menug ID="menuG" Text="Menú GAIA" runat="server"/> 
<asp:Label ID="lbldebug" runat="server"/>
<asp:Label ID="lblResultat" runat="server"/>
<asp:textbox id="txtCodiNode" runat="server"  width="0"></asp:textbox>
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="background-color: #E0E0FE;color:#000066; font-weight:bold;">
  <tr valign="middle">
    <td width="41"><img src="/img/gaia/logoGaiaP.gif" alt="logo GAIA"  vspace="5" hspace="5"></td>
    <td><span>Manteniment de p&agrave;gines web </span></td>
	<td width="213"><span class="txtNeg12px">Canvi d'idioma: 
       <asp:DropDownList runat="server" ID="lstCanviIdioma" AutoPostBack="true" OnSelectedIndexChanged="canviIdioma">
      <asp:ListItem Value="1">Catal&agrave;</asp:ListItem>
      <asp:ListItem Value="2">Castell&agrave;</asp:ListItem>
      <asp:ListItem Value="3">Angl&egrave;s</asp:ListItem>
      <asp:ListItem Value="4">Franc&egrave;s</asp:ListItem>
    </asp:DropDownList>
    </span></td>
  </tr>	   
</table>
<input type="hidden" id="ultimaDivisio" value="" name="ultimaDivisio">
<input type="hidden" id="estructura" value="" name="estructura">
<input type="hidden" id="atributs" value="" name="atributs">
<input type="hidden" id="llistaTHor" value="" name="llistaTHor">
<input type="hidden" id="llistachkimp" value="" name="llistachkimp">
<input type="hidden" id="llistachkCND" value="" name="llistachkCND">
<input type="hidden" id="llistaTVer" value="" name="llistaTVer">
<input type="hidden" value="" id="llistaPlantilles" name="llistaPlantilles">
<input type="hidden"  id="llistaCodis" value="" name="llistaCodis">
<input type="hidden" id="llistaTipusContinguts" value="" name="llistaTipusContinguts">
<input type="hidden" id="llistaNodes" value="" name="llistaNodes">
<input type="hidden" id="llistaNodes2" value="" name="llistaNodes2">
<input type="hidden" id="llistaCSS" value="" name="llistaCSS">
<input type="hidden" id="llistaDescripcions" value="" name="llistaDescripcions">



<asp:label id="lblCodi" runat="server"/>
<table width="100%" border="1" cellspacing="1" cellpadding="1"  class="tablabordepeqroj" bordercolor="#FFCDCA">
  <tr>
    <td colspan="4" bgcolor="#E6E6E6" class="txtrojo12px">PAS 1. DADES DE P&Agrave;GINA WEB 
    </td>
  </tr>
  <tr>
    <td height="156">
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td  class="txtneg12px"><div align="right">* T&iacute;tol&nbsp;&nbsp;</div></td>
          <td colspan="2"><FCKeditorV2:FCKeditor id="WEBDSTIT" width="500" runat="server" height="60"
                                    basepath=" /GAIA/aspx/FCKeditor.v2/"></FCKeditorV2:FCKeditor>   
          </td>
        </tr>
        <tr>
          <td class="txtneg12px"><div align="right">Idioma&nbsp;</div></td>
          <td colspan=2>  <asp:DropDownList runat="server" ID="lstIdioma" AutoPostBack="false" >
              <asp:ListItem Value="1">Catal&agrave;</asp:ListItem>
              <asp:ListItem Value="2">Castell&agrave;</asp:ListItem>
              <asp:ListItem Value="3">Angl&egrave;s</asp:ListItem>
              <asp:ListItem Value="4">Franc&egrave;s</asp:ListItem>
            </asp:DropDownList>
          </td>
        </tr>
         <tr>
          <td  class="txtneg12px"><div align="right"> Descripció&nbsp;&nbsp;</div></td>
          <td colspan="2"><FCKeditorV2:FCKeditor id="WEBDSDES" width="500" runat="server" height="80"
                                    basepath=" /GAIA/aspx/FCKeditor.v2/"></FCKeditorV2:FCKeditor> 
          </td>
        </tr>
         <tr>
          <td  class="txtneg12px"><div align="right"> Paraules Clau&nbsp;&nbsp;</div></td>
          <td colspan="2"><asp:TextBox runat="server"  ID="WEBDSPCL" Columns="60" MaxLength="300"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td class="txtneg12px"><div align="right">Incloure-la al cercador?&nbsp; </div></td>
          <td colspan=2><asp:CheckBox ID="WEBTPBUS" runat="server" /></td>
        </tr>
        <tr>
          <td class="txtneg12px"><div align="right">* Data publicaci&oacute;&nbsp;</div></td>
          <td width="1"><asp:TextBox runat="server"  ID="WEBDTPUB" Columns="10" MaxLength="10"></asp:TextBox></td>
          <td width="427"><a href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=WEBDTPUB','calendar_window','width=150,height=188');calendar_window.focus()"><img src="/img/common/iconografia/ico_calendari.png" border="0"></a></td>
        </tr>
        <tr>
          <td class="txtneg12px"><div align="right">* Data caducitat&nbsp;</div></td>
          <td><asp:TextBox runat="server"  ID="WEBDTCAD" Columns="10" MaxLength="10" ></asp:TextBox></td>
          <td><a href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=WEBDTCAD','calendar_window','width=150,height=188');calendar_window.focus()"><img src="/img/common/iconografia/ico_calendari.png" border="0"></a></td>
        </tr>
        <tr>
          <td class="txtneg12px"><div align="right">* Nom del fitxer for&ccedil;at&nbsp;&nbsp;</div></td>
          <td colspan="2"><asp:TextBox runat="server"  ID="WEBDSFIT" Columns="60" MaxLength="100"></asp:TextBox></td>
          </tr>
					  <tr>
          <td class="txtneg12px"><div align="right">* Url forçada&nbsp;&nbsp;</div></td>
          <td colspan="2"><div style="float:left"><asp:TextBox runat="server"  ID="WEBDSURL" Columns="80" MaxLength="100"></asp:TextBox></div><lh:Ajuda Text="URL, sense http. ex: www.l-h.cat/inici.aspx" runat="server" /> </td>
          </tr>
		  <tr>
          <td class="txtneg12px"><div align="right">Hereta propietats?&nbsp;</div></td>
          <td colspan="2"><asp:CheckBox runat="server" ID="WEBTPHER"></asp:CheckBox></td>				
          </tr>
		    <tr>
          <td class="txtneg12px"><div align="right">Té un formulari de servidor?&nbsp;</div></td>
          <td colspan="2"><asp:CheckBox runat="server" ID="WEBSWFRM" ></asp:CheckBox></td>				
          </tr>
		     <tr>
          <td class="txtneg12px"><div align="right">És un correu electrònic?&nbsp;</div></td>
          <td colspan="2"><asp:CheckBox runat="server" ID="WEBSWEML" ></asp:CheckBox></td>				
          </tr>
            <tr>
          <td class="txtneg12px"><div align="right">És una pàgina segura? (https)&nbsp;</div></td>
          <td colspan="2"><asp:CheckBox runat="server" ID="WEBSWSSL" ></asp:CheckBox></td>				
          </tr>
					 <tr>
          <td class="txtneg12px"><div align="right">Mida  horitzontal màxima <br>
            (només si no hereta propietats)&nbsp;</div></td>
          <td colspan="2" class="txtneg12px">&nbsp;<asp:textBox runat="server" ID="WEBWNMTH" value="0"  Width="35" MaxLength="4"></asp:textBox> px</td>					
          </tr>
		 <tr>
          <td class="txtneg12px"><div align="right">Estils body &nbsp;&nbsp;</div></td>
          <td colspan="2" ><asp:TextBox runat="server"  ID="WEBDSEBO" Columns="60"></asp:TextBox></td>
         </tr>
		  <tr>
          <td class="txtneg12px"><div align="right">Auto enllaç&nbsp;</div></td>
          <td colspan="2">
		  	<asp:TextBox runat="server" ID="gaiaAutoenllacNodes" Text="0" Width="0" Height="0"></asp:TextBox>
				<asp:TextBox ID="gaiaAutoenllacTxt" runat="server" AutoPostBack="False" Rows="3" Columns="58" ContentEditable="false"></asp:TextBox>
				<input type="button" onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?trobaRelacio=1&arbre1=Ajuntament on-line&c=gaiaAutoenllac&nodesSeleccionats='+document.getElementById('gaiaAutoenllacNodes').value,'_blank', 'location=0,height=800,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" id="btnAutoenllac">
				<input type="button" id="eliminarAutoenllac" value="Esborrar" onClick="document.getElementById('gaiaAutoenllacTxt').value='';document.getElementById('gaiaAutoenllacNodes').value='0'; return false;">
		  </td>
		  <td></td>
          </tr>
		  
      </table></td>
  </tr>
</table> 
<table width="100%" border="1" cellspacing="1" cellpadding="1"   class="tablabordepeqroj" bordercolor="#FFCDCA">
  <tr>
    <td bgcolor="#E6E6E6" class="txtrojo12px">PAS 2. ESTRUCTURA DE
      LA P&Agrave;GINA </td>
  </tr>
  <tr>
    <td >
      <table width="100%" border="0" cellspacing="0" cellpadding="0" class="negre">
	  	 <tr  height="200px">
			<td>
				<asp:Label runat="server" ID="lblEstructura"></asp:Label>
			</td>
		</tr>
		<tr height="5px">
			<td bgcolor="#CCCCCC">
			</td>
		</tr>
        <tr>
          <td class="txtneg12px">
		  	<table width="100%" height="200px" border="0">
              <tr>
                <td valign="top">                  
					<table width="100%">
             
				  	<tr>
                    <td colspan="3"><span class="txtRojo14px">Accions</span></td>
                  </tr>
                  <tr>
                    <td height="1" colspan="3" bgcolor="#CCCCCC"></td>
                  </tr>
                  <tr>
                    <td colspan="3" align="center"><input type=button id="btnDivHor"  value="Divisió horitzontal"  onserverclick="clickDividirHoritzontalment" runat="server" disabled onClick="seleccionaCelda('','1');">
                        <input type=button id="btnDivVer"  value="Divisió vertical"  onserverclick="clickDividirVerticalment" runat="server" disabled onClick="seleccionaCelda('','1');">
                        <input type=button id="btnDivEsb"  value="Esborrar divisió"  onserverclick="clickEsborrarDivisio" runat="server" disabled onClick="seleccionaCelda('','1');"><br><br></td>
                  </tr>
					  <tr>
						<td class="txtRojo14px" align="left" width="24%"><input type="hidden" id="divSel" name="divSel" value="Sense selecció">Descripci&oacute;</td>
						<td class="txtRojo14px" align="left" width="43%">Propietats de la cel·la</td>
						<td class="txtRojo14px" align="left" width="33%">Tipus de contingut</td>
					  </tr>
					  <tr>
						<td height="1" bgcolor="#CCCCCC"></td>
						 <td height="1" bgcolor="#CCCCCC"></td>
						  <td height="1" bgcolor="#CCCCCC"></td>
					  </tr>
					  <tr>
						<td align="left">
							
							<asp:TextBox id="WEBDSDEC" name="WEBDSDEC" Text="" runat="server" AutoPostBack="true" OnTextChanged="pintaDescripcio" Enabled="false"></asp:TextBox>						</td>
						<td >
							<table width="100%" >
								<tr>
									<td ><div align="right" class="txtNeg12px">Mida vertical&nbsp; </div></td>
									<td  class="txtNeg12px"><input name="tVer" id="tVer" type="text" class="inputTextrojo12px"  value="100" size="7" maxlength="7" onBlur="seleccionaCelda('','');" disabled>
									%</td>
								</tr>
								<tr>
									<td><div align="right" class="txtNeg12px">Mida horitzontal&nbsp;</div></td>
									<td class="txtNeg12px"><input name="tHor" id="tHor" type="text" class="inputTextrojo12px"  value="100" size="7" maxlength="7" onBlur="seleccionaCelda('','');" disabled>
									%</td>
								</tr>
									<tr>
									<td><div align="right" class="txtNeg12px">Imprimir cel&middot;la? &nbsp;</div></td>
									<td class="txtNeg12px"><input name="chkimp" id="chkimp" type="checkbox" class="inputTextrojo12px" onBlur="seleccionaCelda('','');" disabled></td>
								</tr>
									<tr>
									<td><div align="right" class="txtNeg12px">Mostrar text de contingut no disponible? &nbsp;</div></td>
									<td class="txtNeg12px"><input name="chkCND" id="chkCND" type="checkbox" class="inputTextrojo12px" onBlur="seleccionaCelda('','');" disabled></td>
								</tr>
							</table>						</td>
						<td align="left">
							<asp:Label runat="server" ID="lblTipusFulla"></asp:Label>
							
							
							<br>
							<br></td>
					  </tr>
					</table></td></tr>
                  <tr>
                  	<td colspan="3">
						<table width="100%" >
							<tr>
								<td class="txtRojo14px">
						      		Estil								</td>
		                    </tr>
                      		<tr>
								<td height="1" bgcolor="#CCCCCC">								</td>
                      		</tr>
						</table>					</td>
				  </tr>
	   			  <tr>
							<td colspan="3">
                            
                            <table width="100%" >
                       <tr>
                          <td colspan="2"><span class="txtRojo14px">Capses</span></td>                         
                        </tr>
						<tr>
                          <td colspan="2" bgcolor="#CCCCCC"></td>
                        </tr>
						<tr>
							<td colspan="2">
                            
                            
								<table width="100%">
									<tr>
										<td class="txtneg12px" width="20%" align="right">Comportaments:</td>
										<td width="30%" align="left"><asp:DropDownList CssClass="control" ID="ddlb_114" runat="server"></asp:DropDownList></td>
										<td width="30%" align="right" class="txtneg12px">Fluxes:</td>
										<td width="30%" align="left"><asp:DropDownList CssClass="control" ID="ddlb_105" runat="server"></asp:DropDownList></td>
										<td width="20%" class="txtneg12px"  align="right">Posició:</td>
										<td width="30%" align="left"><asp:DropDownList CssClass="control" ID="ddlb_108" runat="server"></asp:DropDownList></td>
									</tr>
									<tr>
										<td class="txtneg12px" align="right">Disposicio:</td>
										<td align="left"><asp:DropDownList CssClass="control" ID="ddlb_122" runat="server"></asp:DropDownList></td>
										<td  class="txtneg12px"  align="right">Justificació:</td>
										<td align="left"><asp:DropDownList CssClass="control" ID="ddlb_27" runat="server"></asp:DropDownList></td>
										<td class="txtneg12px" align="right">Tipus:</td>
										<td align="left"><asp:DropDownList CssClass="control" ID="ddlb_26" runat="server"></asp:DropDownList></td>
									</tr>
							</table>							</td>
                        </tr>
                      </table>
                      <table width="100%" >
                       <tr>
                          <td colspan="2"><span class="txtRojo14px">Text</span></td>                         
                        </tr>
						<tr>
                          <td colspan="2" bgcolor="#CCCCCC"></td>
                        </tr>
						<tr>
							<td colspan="2">
                            
                            
								<table width="100%">
									<tr>
										<td  width="12%" class="txtneg12px" align="right">Alineaci&oacute;:</td>
										<td width="15%" align="left"><asp:DropDownList CssClass="control" ID="ddlb_109" runat="server"></asp:DropDownList></td>
										<td width="18%"  class="txtneg12px" align="right">Font:</td>
									  <td width="14%" align="left"><asp:DropDownList CssClass="control" ID="ddlb_110" runat="server"></asp:DropDownList></td>
										<td width="19%" class="txtneg12px"  align="right">Transformaci&oacute;:</td>
									  <td width="22%" align="left"><asp:DropDownList CssClass="control" ID="ddlb_111" runat="server"></asp:DropDownList></td>
									</tr>
									<tr>
										<td class="txtneg12px" align="right">Color:</td>
										<td align="left"><asp:DropDownList CssClass="control" ID="ddlb_23" runat="server"></asp:DropDownList></td>
										<td class="txtneg12px" align="right">Justificaci&oacute;:</td>
										<td align="left"><asp:DropDownList CssClass="control" ID="ddlb_123" runat="server"></asp:DropDownList></td>
									  <td class="txtneg12px" align="right">Format:</td>
									  <td align="left"><asp:DropDownList CssClass="control" ID="ddlb_124" runat="server"></asp:DropDownList></td>
								  </tr>
									<tr>
										<td class="txtneg12px" align="right">Decoraci&oacute;:</td>
										<td align="left"><asp:DropDownList CssClass="control" ID="ddlb_112" runat="server"></asp:DropDownList></td>
										<td class="txtneg12px" align="right">Mida:</td>
										<td align="left"><asp:DropDownList CssClass="control" ID="ddlb_24" runat="server"></asp:DropDownList></td>
										<td class="txtneg12px" align="right">&nbsp;</td>
									    <td align="left">&nbsp;</td>
								  </tr>
							</table>							</td>
                        </tr>
                      </table><table width="100%" >
                       <tr>
                          <td colspan="2"><span class="txtRojo14px">Gen&egrave;ric</span></td>                         
                        </tr>
						<tr>
                          <td colspan="2" bgcolor="#CCCCCC"></td>
                        </tr>
						<tr>
							<td colspan="2">
                            
                            
								<table width="100%">
									<tr>
										<td class="txtneg12px" width="20%" align="right">Amplades i al&ccedil;ades:</td>
										<td width="30%" align="left"><asp:DropDownList CssClass="control" ID="ddlb_115" runat="server"></asp:DropDownList></td>
										<td width="20%" class="txtneg12px"  align="right">Impressi&oacute;:</td>
										<td width="30%" align="left"><asp:DropDownList CssClass="control" ID="ddlb_119" runat="server"></asp:DropDownList></td>
									</tr>
									<tr>
										<td class="txtneg12px" align="right">Fons:</td>
										<td align="left"><asp:DropDownList CssClass="control" ID="ddlb_118" runat="server"></asp:DropDownList></td>
										<td class="txtneg12px" align="right">Voreres:</td>
										<td align="left"><asp:DropDownList CssClass="control" ID="ddlb_103" runat="server"></asp:DropDownList></td>
									</tr>
							</table>							</td>
                        </tr>
                      </table>
                      <table width="100%" >
                       <tr>
                          <td colspan="2"><span class="txtRojo14px">Marges</span></td>                         
                        </tr>
						<tr>
                          <td colspan="2" bgcolor="#CCCCCC"></td>
                        </tr>
						<tr>
							<td colspan="2">
                            
                            
								<table width="100%">
									<tr>
										<td class="txtneg12px" width="20%" align="right">Exteriors:</td>
									  <td width="30%" align="left"><asp:DropDownList CssClass="control" ID="ddlb_28" runat="server"></asp:DropDownList></td>
										<td width="20%" class="txtneg12px"  align="right">Interiors:</td>
										<td width="30%" align="left"><asp:DropDownList CssClass="control" ID="ddlb_25" runat="server"></asp:DropDownList></td>
									</tr>
							</table>							</td>
                        </tr>
                      </table>
                      <table width="100%" >
                       <tr>
                          <td colspan="2"><span class="txtRojo14px">Estils definits:</span></td>                         
                        </tr>
						<tr>
                          <td colspan="2" bgcolor="#CCCCCC"></td>
                        </tr>
						<tr>
							<td colspan="2">
                            
                            
								<table width="100%">
									<tr>
										<td width="80%" align="left"><asp:DropDownList CssClass="control" ID="ddlb_117" runat="server"></asp:DropDownList></td>
									</tr>
							</table>							</td>
                        </tr>
                      </table>
                            
                            </td>
                 		</tr>                 
                     
                  <tr>
                    <td colspan="3"><table width="100%" ><tr><td><span class="txtRojo14px"> Plantilles</span></td>
                    <td><span class="txtRojo14px">Llibreria de codi web </span></td>
                    </tr><tr><td height="1" colspan="3" bgcolor="#CCCCCC"></td></tr>
                        <tr valign="top">
                          <td width="49%" rowspan="2" align="center"  bgcolor="#EEEEEE">
                           <input type="button"  onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?arbre1=plantillaWeb&c=gaiaPlantillaWeb&nodesSeleccionats='+document.getElementById('gaiaPlantillaWebNodes').value+'&separador=|','_blank', 'location=0,height=800,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" id="btnPlantilles" disabled>
                            <input type="button" id="eliminarPlantilla" value="Esborrar" onClick="document.getElementById('gaiaPlantillaWebNodes').value='';document.getElementById('gaiaPlantillaWebTxt').value=''; return false;">
                            <input type="button"  onClick="window.open('/GAIA/aspx/web/editaPlantilla.htm?nodesSeleccionats='+document.getElementById('gaiaPlantillaWebNodes').value+'','_blank','location=0,height=800,width=600,scrollbars=yes,resizable=yes');return false;" value="Edita Plantilles" id="btnEditaPlantilles" disabled>
                            <asp:TextBox	 ID="gaiaPlantillaWebTxt" runat="server" AutoPostBack="False"  Rows="3" columns=45 ContentEditable="false" TextMode="MultiLine"></asp:TextBox><br><asp:TextBox ID="gaiaPlantillaWebNodes" runat="server"></asp:TextBox>

						</td>                  				
                        <td width="51%" valign="top" bgcolor="#EEEEEE" align="center">
  <input type="button"  onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?arbre1=codiWeb&separador=|&c=gaiaCodiWeb&nodesSeleccionats='+document.getElementById('gaiaCodiWebNodes').value,'_blank', 'location=0,height=800,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" id="btnCodis" >
  <input type="button" id="eliminarLlibreria" value="Esborrar" onClick="document.getElementById('gaiaCodiWebNodes').value='';document.getElementById('gaiaCodiWebTxt').value=''; return false;">   <input type="button"  onClick="window.open('/GAIA/aspx/fulles/editaLCW.htm?nodesSeleccionats='+document.getElementById('gaiaCodiWebNodes').value,'_blank', 'location=0,height=800,width=400,scrollbars=yes,resizable=yes');return false;" value="Edita LCW" id="btnEditaCodis" disabled >
  <asp:TextBox	 ID="gaiaCodiWebTxt" runat="server" AutoPostBack="False"  Rows="3" columns=45 ContentEditable="false" TextMode="MultiLine" ></asp:TextBox><br><asp:TextBox ID="gaiaCodiWebNodes" runat="server" ></asp:TextBox>

  													</td>
                        </tr>
                      </table></td>
                  </tr>
                </table></td>
              </tr>
            </table></td>
          </tr>
  
   
</table> 
<table width="100%" border="1" cellspacing="1" cellpadding="1"   class="tablabordepeqroj" bordercolor="#FFCDCA" >
  <tr>
    <td colspan="3" bgcolor="#E6E6E6" class="txtrojo12px">PAS 3. CICLE DE VIDA
      DE LA P&Agrave;GINA WEB </td>
  </tr>
  <tr>
    <td ><table width="100%" ><tr>
      <td><span class="txtRojo14px"> Circuit de publicaci&oacute; </span></td>
                    <td><span class="txtRojo14px">Circuit de caducitat </span></td>
                    </tr><tr><td height="1" colspan="2" bgcolor="#CCCCCC"></td></tr>
                        <tr valign="top">
                          <td width="49%" rowspan="2">  <div align="center">
													 <asp:TextBox ID="gaiaCircuitPublicacioNodes" runat="server"  Height="0" Width="0" ></asp:TextBox>													
											      <input type="button"  onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?trobaRelacio=1&arbre1=circuit&c=gaiaCircuitPublicacio&nodesSeleccionats='+document.getElementById('gaiaCircuitPublicacioNodes').value,'_blank', 'location=0,height=800,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" id="btnPlantilles" ><input type="button" id="eliminarCircuitPublicacio" value="Esborrar" onClick="document.getElementById('gaiaCircuitPublicacioNodes').value='';document.getElementById('gaiaCircuitPublicacioTxt').value=''; return false;">
													      <br>
													    </div>
													    <div align="center">
													      
													      <asp:TextBox	 ID="gaiaCircuitPublicacioTxt" runat="server" AutoPostBack="False"  Rows="3" Columns=45 ContentEditable="false" TextMode="MultiLine" ></asp:TextBox>
													    </div></td>                  				
			<td width="51%" valign="top" bgcolor="#EEEEEE">
<div align="center">



<asp:TextBox ID="gaiaCircuitCaducitatNodes" runat="server"  Height=0 Width=0 ></asp:TextBox>													
 <input type="button"  onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?trobaRelacio=1&arbre1=circuit&c=gaiaCircuitCaducitat&nodesSeleccionats='+document.getElementById('gaiaCircuitCaducitatNodes').value,'_blank', 'location=0,height=800,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" id="btnPlantilles" >										       
 <input type="button" id="eliminarCircuitCaducitat" value="Esborrar" onClick="document.getElementById('gaiaCircuitCaducitatNodes').value='';document.getElementById('gaiaCircuitCaducitatTxt').value=''; return false;">
													      <br>
													    </div>
													    <div align="center">   
													      <asp:TextBox	 ID="gaiaCircuitCaducitatTxt" runat="server" AutoPostBack="False"  Rows="3" Columns=45 ContentEditable="false" TextMode="MultiLine" ></asp:TextBox>
</div>


  													</td>
                        </tr>
                      </table></td>
  </tr>
</table>     
	<div align="center">
	  <input type=button id="btnInsert"  value="Crear pàgina web" onClick="seleccionaCelda('t0','t0');" onserverclick="clickAfegirPaginaWeb" runat="server">
	  <a href="#" onClick="obrePreview()">Veure Estructura</a>
	  <input name="button"  type=button id="button" onClick="seleccionaCelda('t0','t0');"  value="Esborrar Idioma" onserverclick="clickEsborrarIdioma" runat="server" class="blancSobreVermell">
	</div>
	
</form>


<!-- #INCLUDE VIRTUAL="js/App_LocalResources/peu.aspx" -->
</body>


</html>
