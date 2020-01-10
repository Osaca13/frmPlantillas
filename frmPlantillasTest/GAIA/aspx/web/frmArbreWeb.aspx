<identity impersonate="true"/>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmArbreWeb.aspx.vb" Inherits="frmPlantillasTest.frmArbreWeb" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<!-- #BeginEditable "doctitle" --> 
<title>Intranet de l'Ajuntament de L'Hospitalet</title>
<!-- #EndEditable -->

<link rel="stylesheet" href="/css/intranet.css" type="text/css">
<link rel="stylesheet" href="/css/gaiaIntranet.css" type="text/css">
</head>

<body bgcolor="#FFFFFF" text="#000000" leftmargin="10" topmargin="10">
<!--#INCLUDE VIRTUAL="~/js/App_LocalResources/cap.inc" -->
<!-- #BeginEditable "Content" -->

  <script language="javascript">
	function activaCamps(activar)
	{
		if (activar) {
			document.getElementById('tVer').disabled=false;
			document.getElementById('tHor').disabled=false;
			document.getElementById('btnDivHor').disabled=false;
			document.getElementById('btnDivVer').disabled=false;			
			document.getElementById('btnCodis').disabled=false;
			document.getElementById('tipusContingut').disabled=false;
			document.getElementById('btnDivEsb').disabled=false;
		    document.getElementById('btnEditaCodis').disabled=false;
		}
		else {
			document.getElementById('tVer').disabled=false;
			document.getElementById('tHor').disabled=false;
			document.getElementById('btnDivHor').disabled=true;
			//document.getElementById('tipusContingut').disabled=true;
			//document.getElementById('btnCodis').disabled=true;
			document.getElementById('btnDivVer').disabled=true;
			document.getElementById('btnDivEsb').disabled=true;
			//document.getElementById('btnEditaCodis').disabled=true;

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
			var arrayTVer = document.getElementById("llistaTVer").value.split(",");
			var arrayTHor = document.getElementById("llistaTHor").value.split(",");
			var arrayLlistaTipusContinguts = document.getElementById("llistaTipusContinguts").value.split(",");
	
			var arrayLlistaCodis= document.getElementById("llistaCodis").value.split("|");

			var arrayLlistaNodes2= document.getElementById("llistaNodes2").value.split(",");
			
			var llistaTVer="";
			var llistaTHor="";
			var llistaTipusContinguts="";
			
			var llistaNodes2="";

			var llistaCodis ="";

	
			if  (ultimaDivisio!="") {				
				if (celda.length==0) {
					celda = ultimaDivisio;
				}
				document.getElementById(ultimaDivisio).bgColor="";							
				arrayTVer[ultimaDivisio.substring(1)]=document.getElementById("tVer").value;
				arrayTHor[ultimaDivisio.substring(1)]=document.getElementById("tHor").value;
				
				
				arrayLlistaNodes2[ultimaDivisio.substring(1)]=document.getElementById("gaiaCodiWebNodes").value;
				arrayLlistaTipusContinguts[ultimaDivisio.substring(1)]=document.getElementById("tipusContingut")[document.getElementById("tipusContingut").selectedIndex].value;
				arrayLlistaCodis[ultimaDivisio.substring(1)]=document.getElementById("gaiaCodiWebTxt").value;								
			}
			var cont=0;
				// 1. actualitzo la llista amb el valor que hi ha a "tamany vertical" i "tamany horitzontal"
				while (cont < arrayTVer.length) {
					if (cont>0) {
						llistaTVer+=",";	
						llistaTHor+=",";						
						llistaCodis+="|";
						llistaNodes2+=",";						
						llistaTipusContinguts+=",";
					}					
					llistaTVer+=arrayTVer[cont];
					llistaTHor+=arrayTHor[cont];
					llistaCodis+=arrayLlistaCodis[cont];
					llistaNodes2+=arrayLlistaNodes2[cont];					
					llistaTipusContinguts+=arrayLlistaTipusContinguts[cont];				
					cont++;
				}
			
				// 2. actualitzo el valor de "tamany vertical" i "horitzontal" amb el valor de la llista apuntat per la divisió seleccionada

        document.getElementById("tVer").value=arrayTVer[celda.substring(1)];
				document.getElementById("tHor").value=arrayTHor[celda.substring(1)];
				document.getElementById("gaiaCodiWebNodes").value=arrayLlistaNodes2[celda.substring(1)];
				document.getElementById("gaiaCodiWebTxt").value=arrayLlistaCodis[celda.substring(1)];
				
				
				cont=0
				while (cont < document.getElementById("tipusContingut").length) {
					if (arrayLlistaTipusContinguts[celda.substring(1)]==" ") {
						index=0;
					}
					else {
						index = arrayLlistaTipusContinguts[celda.substring(1)];
					}
					
					if (document.getElementById("tipusContingut")[cont].value==index) {
						document.getElementById("tipusContingut")[cont].selected=true;
						cont =  document.getElementById("tipusContingut").length;
					}
					cont++;
				}
		
				document.getElementById("llistaTVer").value=llistaTVer;
				document.getElementById("llistaTHor").value=llistaTHor;				
				document.getElementById("llistaCodis").value=llistaCodis;
				document.getElementById("llistaTipusContinguts").value=llistaTipusContinguts;
				document.getElementById("llistaNodes2").value=llistaNodes2;
							
				document.getElementById("ultimaDivisio").value=celda;
				document.getElementById(celda).bgColor="#CACAAA";
				if (nomDivisio.length>0) {
					document.getElementById("divSel").value=nomDivisio;
				}
			return false;
	}
</script>


<form runat="server">
      <%@ Register TagPrefix="menuG" TagName="menuG" Src="~/GAIA/llibreriacodiweb/intranet/gaia/menu2.ascx" %>
    <menuG:menug ID="menuG" Text="Menú GAIA" runat="server"/> 

<asp:Label ID="lbldebug" runat="server"/>
<asp:Label ID="lblResultat" runat="server"/>
<table border="0" width="100%" cellpadding="0" cellspacing="0"  style="background-color: #E0E0FE;color:#000066; font-weight:bold; " >
  <tr  valign="middle">
    <td width="41"><img src="/img/gaia/logoGaiaP.gif" alt="logo GAIA"  vspace="5" hspace="5"></td>
    <td><span>Manteniment d'arbres web</span></td>
  </tr>
  <tr>
    <td colspan=2 height=1 bgcolor="#CCCCCC"></td>
  </tr>
</table>
<input type="hidden" id="ultimaDivisio" value="" name="ultimaDivisio">
<input type="hidden" id="estructura" value="" name="estructura">
<input type="hidden" id="atributs" value="" name="atributs">
<input type="hidden" id="llistaTHor" value="" name="llistaTHor">
<input type="hidden" id="llistaTVer" value="" name="llistaTVer">
<input type="hidden"  id="llistaCodis" value="" name="llistaCodis">
<input type="hidden" id="llistaTipusContinguts" value="" name="llistaTipusContinguts">
<input type="hidden" id="llistaNodes2" value="" name="llistaNodes2">

<asp:label id="lblCodi" runat="server"/>
<table width="100%" border="1" cellspacing="1" cellpadding="1" class="tablabordepeqroj" bordercolor="#FFCDCA">
  <tr>
    <td colspan="4" bgcolor="#E6E6E6" class="txtrojo12px">PAS 1. DADES DE L'ARBRE WEB </td>
  </tr>
  <tr>
    <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td class="txtneg12px"><div align="right">Idioma&nbsp;&nbsp;</div></td>
          <td ><asp:Label runat="server" ID="lblIdioma"></asp:Label></td>
        </tr>
        <tr>
          <td width="25%"  class="txtneg12px"><div align="right">T&iacute;tol&nbsp;&nbsp;</div></td>
          <td width="75%" ><asp:TextBox runat="server"  ID="AWEDSTIT" Columns="60" MaxLength="60"></asp:TextBox></td>
        </tr>
          <tr>
          <td width="25%"  class="txtneg12px"><div align="right">Nom de l'arbre&nbsp;&nbsp;</div></td>
          <td width="75%" ><asp:TextBox runat="server"  ID="AWEDSNOM" Columns="60" MaxLength="60"></asp:TextBox></td>
        </tr>
        <tr>
          <td class="txtneg12px"><div align="right">Servidor
              FTP dest&iacute;:&nbsp;&nbsp;</div></td>
          <td >
					<asp:DropDownList id="lstAWEDSSER" runat="server"></asp:DropDownList>
					</td>
        </tr>
        <tr>
          <td  class="txtneg12px"><div align="right">Cam&iacute; del disc:&nbsp;&nbsp; </div></td>
          <td ><asp:TextBox runat="server"  ID="AWEDSROT" Columns="60" MaxLength="60"></asp:TextBox></td>
        </tr>
				  <tr>
          <td  class="txtneg12px"><div align="right">Destí pels documents : </div></td>
          <td ><asp:TextBox runat="server"  ID="AWEDSDOC" Columns="60" MaxLength="60"></asp:TextBox></td>
        </tr>
        <tr>
          <td  class="txtneg12px"><div align="right">Mida Horitzontal&nbsp;&nbsp;</div></td>
          <td ><asp:TextBox runat="server"  ID="AWEDSHOR" Columns="4" MaxLength="4"></asp:TextBox>
            <span class="txtNeg12px">px.</span></td>
        </tr>
        <tr>
          <td  class="txtneg12px"><div align="right">Mida Vertical&nbsp;&nbsp;</div></td>
          <td ><asp:TextBox runat="server"  ID="AWEDSVER" Columns="4" MaxLength="4"></asp:TextBox>
            <span class="txtNeg12px">px.</span></td>
        </tr>
				      <tr>
          <td  class="txtneg12px"><div align="right">Estils body&nbsp;&nbsp;</div></td>
          <td ><asp:TextBox runat="server"  ID="AWEDSEBO" Columns="60" MaxLength="60"></asp:TextBox>
            </td>
        </tr>
				      <tr>
          <td  class="txtneg12px"><div align="right">Valors META&nbsp;&nbsp;</div></td>
          <td><asp:TextBox runat="server" ID="AWEDSMET" Columns="98" Rows="10" TextMode="MultiLine" CssClass="txtNeg11px"></asp:TextBox>
            </td>
        </tr>
		<tr>
			<td  class="txtneg12px"><div align="right">HTML al peu:&nbsp;&nbsp;</div></td>
			<td ><asp:TextBox runat="server"  ID="AWEDSPEU" Columns="98" Rows="10" TextMode="MultiLine" CssClass="txtNeg11px"></asp:TextBox></td>
		</tr>
		<tr>
			<td  class="txtneg12px"><div align="right">CSS per pantalla:&nbsp;&nbsp;</div></td>
			<td ><asp:TextBox runat="server"  ID="AWEDSCSP" Columns="80" ></asp:TextBox></td>
		</tr>
		<tr>
			<td  class="txtneg12px"><div align="right">CSS per impressora:&nbsp;&nbsp;</div></td>
			<td ><asp:TextBox runat="server"  ID="AWEDSCSI" Columns="80" ></asp:TextBox></td>
		</tr>
		
    </table></td>
  </tr>
</table>
<table width="100%" border="1" cellspacing="1" cellpadding="1"   class="tablabordepeqroj" bordercolor="#FFCDCA">
  <tr>
    <td colspan="4" bgcolor="#E6E6E6" class="txtrojo12px">PAS 2. ESTRUCTURA DE
      LA P&Agrave;GINA </td>
  </tr>
  <tr>
    <td >
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td colspan="2" class="txtneg12px"><table width="752" height="200px" border="0">
              <tr>
                <td width="307" heigth=400px><asp:Label runat="server" ID="lblEstructura"></asp:Label></td>
								<td width="1" bgcolor="#CCCCCC"></td>
                <td width="421" valign="top">                  <table width="100%">
                  <tr>
                    <td colspan="3"><span class="txtRojo14px">Divisi&oacute;: </span>
                        <input type="text" name="divSel" id="divSel" value="Sense selecció" onFocus="this.blur();" class="negroSobreBlanco14px">
                    </td>
                  </tr>
                  <tr>
                    <td height="1" colspan="3" bgcolor="#CCCCCC"></td>
                  </tr>
                  <tr>
                    <td colspan="3"  class="txtNeg14px" ><div align="left">Propietats de la Cel·la </div></td>
                  </tr>
                  <tr>
                    <td width="29%" ><table width="100%" >
                      <tr>
                        <td width="33%"><div align="right" class="txtNeg12px">Mida Vertical&nbsp; </div></td>
                        <td width="67%"><input name="tVer"  id="tVer" type="text" class="inputTextrojo12px"  value="100" size="1" maxlength="3" onBlur="seleccionaCelda('','');" disabled>
                      %</td>
                      </tr>
                      <tr>
                        <td><div align="right" class="txtNeg12px">Mida
                        Horitzontal&nbsp;</div></td>
                        <td><input name="tHor" id="tHor" type="text" class="inputTextrojo12px"  value="100" size="1" maxlength="3" onBlur="seleccionaCelda('','');" disabled>
%</td>
                      </tr>
                    </table></td>
                   
                  </tr>
                 
                 
                  <tr>
                    <td ><table width="100%" ><tr><td><span class="txtRojo14px">
      Tipus de continguts</span></td>
                    </tr>
                      <tr><td height="1" bgcolor="#CCCCCC"></td></tr>
                        <tr valign="top">
                          <td rowspan="2"><asp:Label runat="server" ID="lblTipusFulla"></asp:Label>
                          <asp:TextBox ID="gaiaCodiWebNodes" runat="server"  Height=0 Width=0 ></asp:TextBox></td>                  				
                        </tr>
                      </table></td>
                  </tr>
                  <tr>
                    <td colspan="3"><table width="100%" ><tr><td><span class="txtRojo14px"> </span><span class="txtRojo14px">Llibreria de codi web<br> 
                      <asp:TextBox	 ID="gaiaCodiWebTxt" runat="server" AutoPostBack="False"  Rows="3" Columns=30 ContentEditable="false" TextMode="MultiLine" ></asp:TextBox>
                    </span></td>
                    </tr><tr><td height="1" bgcolor="#CCCCCC"></td></tr>
                        <tr valign="top">
                          <td rowspan="2">  <div align="center">
    <input type="button"  onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?arbre1=codiWeb&c=gaiaCodiWeb&nodesSeleccionats='+document.getElementById('gaiaCodiWebNodes').value,'_blank', 'location=0,height=400,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" id="btnCodis" enabled>
    <input type="button" id="eliminarCodi" value="Esborrar" onClick="document.getElementById('gaiaCodiWebNodes').value='';document.getElementById('gaiaCodiWebTxt').value=''; return false;">
 <input type="button"  onClick="window.open('/GAIA/aspx/fulles/editaLCW.htm?nodesSeleccionats='+document.getElementById('gaiaCodiWebNodes').value,'_blank', 'location=0,height=400,width=400,scrollbars=yes,resizable=yes');return false;" value="Edita LCW" id="btnEditaCodis" enabled >
    <br>
                          </div></td>                  				
                        </tr>
                      </table></td>
                  </tr>
                  <tr>
                    <td colspan="3" bgcolor="#CCCCCC" heigth="1"></td>
                  </tr>
                  <tr>
                    <td colspan="3"> 									
										</td>
                  </tr>
                  <tr>
                    <td colspan="3"><span class="txtRojo14px">
      Accions</span></td>
                  </tr>
                  <tr>
                    <td height="1" colspan="3" bgcolor="#CCCCCC"></td>
                  </tr>
                  <tr>
                    <td colspan="3"><input type=button id="btnDivHor"  value="Divisió horitzontal"  onserverclick="clickDividirHoritzontalment" runat="server" disabled onClick="seleccionaCelda('','1');">
                        <input type=button id="btnDivVer"  value="Divisió vertical"  onserverclick="clickDividirVerticalment" runat="server" disabled onClick="seleccionaCelda('','1');">
                        <input type=button id="btnDivEsb"  value="Esborrar divisió"  onserverclick="clickEsborrarDivisio" runat="server" disabled onClick="seleccionaCelda('','1');"></td>
                  </tr>
                </table></td>
              </tr>
            </table></td>
          </tr>
  
    </table>
      </td>
  </tr>
</table>          
	<div align="center">
	  <input type=button id="btnInsert"  value="Modificar arbre web" onClick="seleccionaCelda('t0','t0');" onserverclick="clickModificarArbreWeb" runat="server">
	
	</div>
</form>

<!-- #EndEditable -->
<!-- #INCLUDE VIRTUAL="~/js/App_LocalResources/peu.aspx" -->
</body>
<!-- #EndTemplate -->

</html>
