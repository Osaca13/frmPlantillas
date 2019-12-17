<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Propietats.aspx.vb" Inherits="frmPlantillasTest.Propietats" Debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
 <title>GAIA. Propietats</title>
 <%@ Register TagPrefix="radT" Namespace="Telerik.WebControls" Assembly="RadTreeView" %>
<link rel="stylesheet" href="/css/gaiaIntranet_1024.css" type="text/css" media="screen"/>
<link rel="stylesheet" href="/css/intranet_1024.css" type="text/css" media="screen"/>
<link rel="stylesheet" type="text/css" href="/css/jquery-ui.css">
<link rel="stylesheet" type="text/css" href="/scripts/jquery.timepicker.css" />
<script src="/js/jquery-1.10.2.js"></script>
<script src="/js/jquery-ui-1.11.4/jquery-ui.js"></script>
<script type="text/javascript" src="/Scripts/jquery.datepick-ca.js"></script>
<script type="text/javascript" src="/scripts/jquery.timepicker.js"></script> 
<script type="text/javascript" src="/scripts/jquery-ui.multidatespicker.js"></script> 
</head>
<body>

<script language="javascript">
 $(document).ready(function() {
 	
 		$("#dataIni").datepicker({
		  		 showOn: "both",
	             buttonImage: "/img/common/ico_calendar.png",
				 buttonImageOnly: true,
	            changeMonth: true,
	            changeYear: true
	        });
			$("#dataFi").datepicker({
		  		 showOn: "both",
	             buttonImage: "/img/common/ico_calendar.png",
				 buttonImageOnly: true,
	            changeMonth: true,
	            changeYear: true
	        });
	});
	
		
</script>
<form runat="server" class="margin0 padding0" id="frmPropietats" >

<table cellpadding="0" cellspacing="10" border="0" width="100%">
<tr>
<td>
<!--#INCLUDE VIRTUAL="js/App_LocalResources/cap_intranet.inc" -->
<table width="100%" border="0" cellpadding="5" cellspacing="0" class="fonsgris05">
  <tr>
    <td bgcolor="#5d5d5d" colspan="2">
    	<table width="100%" cellpadding="0" cellspacing="0" border="0"><tr><td><asp:Label ID="lblNode" runat="server" CssClass="t1 blanc arial bold mayusculas"/></td><td align="right"><asp:Label ID="lblEditar" runat="server" CssClass="botoEditar"/></td></tr></table>			
    </td>    
  </tr>
	<tr>
    <td align="right" valign="top" width="25%" class="bold arial t075 border1BottomDotted846540">Tipus contingut:</td>
    <td class="arial t075 blauTurquesa border1BottomDotted846540"><asp:Label ID="lblTipusNode" runat="server"/></td>
  </tr>
	<tr>
    <td align="right" valign="top" class="bold arial t075 border1BottomDotted846540">Usuari creador:</td>
    <td class="arial t075 blauTurquesa border1BottomDotted846540"><asp:Label ID="lblUsr" runat="server"/></td>
  </tr>
	<tr>
    <td align="right" valign="top" class="bold arial t075 border1BottomDotted846540">Data creaci&oacute;:</td>
    <td class="arial t075 blauTurquesa border1BottomDotted846540"><asp:Label ID="lblTim" runat="server"/></td>
  </tr>
 <tr>
    <td align="right" valign="top" class="bold arial t075 border1BottomDotted846540">Codis:</td>
    <td class="arial t075 blauTurquesa border1BottomDotted846540">Node: <asp:Label ID="lblCodi" runat="server"/><br/>Relaci&oacute;: <asp:Label ID="lblRelacio" runat="server"/></td>
  </tr>	
  <asp:panel runat="server" id="pnlOrdre" visible="false">
  <tr>
    <td align="right" valign="top" class="bold arial t075 border1BottomDotted846540">Ordre:</td>
    <td class="arial t075 blauTurquesa border1BottomDotted846540"><asp:Label ID="lblOrdre" runat="server"/></td>
  </tr>
  </asp:panel>
  <tr>
    <td align="right" valign="top" class="bold arial t075 border1BottomDotted846540">Estat:</td>
    <td class="arial t075 blauTurquesa border1BottomDotted846540"><asp:Label ID="lblSituacio" runat="server"/></td>
  </tr>
  <asp:panel id="pnlVisites" runat="server" visible="false">
  <tr>
    <td align="right" valign="top" class="bold arial t075 border1BottomDotted846540">Visites als webs en el període <br />del <asp:textbox runat="server"  id="dataIni" /><br />al <asp:textbox runat="server" id="dataFi"  name="dataFi"/><br />  
   
	<button type="submit"  ID="btnCanviarPeriode"   runat="server" onserverclick="btnCanviarPeriode_Click">Calcular</button>
    </td>
    <td class="arial t075 blauTurquesa border1BottomDotted846540"><asp:Label ID="lblVisites" runat="server"/></td>
  </tr>
  </asp:panel>
  <asp:Panel ID="PanelHistorico" Visible="false" runat="server">
   <tr>
      <td align="right" valign="top" class="bold arial t075 border1BottomDotted846540">Hist&ograve;ric:</td>
      <td class="arial t075 blauTurquesa border1BottomDotted846540"><asp:HyperLink ID="ctrlHistoricoCatala"  runat="server" Text="Català"  Font-Underline="true"></asp:HyperLink><br />
      <asp:HyperLink ID="ctrlHistoricoCastella" runat="server" Text="Castellà"  Font-Underline="true"></asp:HyperLink>
      </td>
   </tr>
  </asp:Panel>
   <tr>
      <td align="right" valign="top" class="bold arial t075 border1BottomDotted846540">Ubicaci&oacute; del contingut dins dels arbres:</td>
      <td class="border1BottomDotted846540"><div class="llistatPuntBlau"><asp:label runat="server" id="lblUbicacions"/></div></td>
   </tr> 
   <asp:Panel ID="PanelUsPagines" Visible="false" runat="server">
   <tr>
      <td align="right" valign="top" class="bold arial t075 border1BottomDotted846540">On s'utilitza el contingut?:</td>
      <td class="border1BottomDotted846540"><div class="llistatContinguts t075"><asp:label runat="server" id="lblPaginesUs"/></div></td>
   </tr>    
   </asp:Panel>
</table>
<!--#INCLUDE VIRTUAL="js/App_LocalResources/peu_intranet.inc" -->
</td>
</tr>
</table>
</form>	
</body>

</html>
