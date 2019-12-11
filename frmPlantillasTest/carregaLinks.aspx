<%@ Page Language="vb" AutoEventWireup="false" ValidateRequest="false" Debug="true" %>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<%@ Register TagPrefix="radT" Namespace="Telerik.WebControls" Assembly="RadTreeView" %>
<%@ Register TagPrefix="UpImgPrev" TagName="UpImgPrev" Src="../llibreria/UpImgPrev.ascx" %>
<%@ Register TagPrefix="UpDocPrev" TagName="UpDocPrev" Src="../llibreria/UpDocPrev.ascx" %>
<%@ Register TagPrefix="lh" TagName="Ajuda" Src="../Ajuda/Ajuda.ascx" %>
<html>
<head>
<title>Enllaç</title>
<script language="JavaScript">
  window.onload = function(){ IniciDoc(); IniciImg(); };
    </script>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<link rel="stylesheet" href="/css/intranet.css" type="text/css">
<link rel="stylesheet" href="/css/gaiaIntranet.css" type="text/css">
<link href="../Ajuda/Ajuda.css" rel="stylesheet" type="text/css" />
<script language="JavaScript" type="text/javascript" src="../Ajuda/Ajuda.js"></script>
</head>
<body bgcolor="#FFFFFF" text="#000000" leftmargin="10" topmargin="10">
<!--#INCLUDE VIRTUAL="/inc/cap.aspx" -->
<script>
function blanquejarCampAlternatiu() {
	/*if (document.getElementById("LNKDSLNK").value != "") {
		document.getElementById("linkNodes").value="";
		document.getElementById("linkTxt").value="";

	}
	*/
}
    </script>
<form enctype="multipart/form-data" runat="server">
  <%@ Register TagPrefix="menuG" TagName="menuG" Src="/gaia/aspx/llibreriacodiweb/intranet/gaia/menu.ascx" %>
  <menuG:menug ID="menuG" Text="Menú GAIA" runat="server"/>
  <asp:Label ID="lbldebug" runat="server"/>
   <asp:panel id="pnlAvis" runat="server" visible="false">
       <div class="missatgeAvisIntranet"> 
            <span class="topEsquerraBlanc"></span>
            <span class="topDretaBlanc"></span>
                <div class="icona"><asp:Label ID="lblAvis" runat="server" /></div>
            <span class="bottomEsquerraBlanc"></span>
            <span class="bottomDretaBlanc"></span>
        </div>
	</asp:panel>
      <asp:panel runat="server" id="pnlOK" visible="false">
        <div class="missatgeOkIntranet"> 
            <span class="topEsquerraBlanc"></span>
            <span class="topDretaBlanc"></span>
                <div class="icona"><asp:literal runat="server" id="lblOk"></asp:literal></div>
            <span class="bottomEsquerraBlanc"></span>
            <span class="bottomDretaBlanc"></span>
        </div>
        </asp:panel>
 
 <%-- <label id="errorAfegir" runat="server" class="botoEsborrar"></label>--%>
  <asp:PlaceHolder ID="errorPermisos" Visible="false" runat="server">
    <asp:Label ID="Label3" Text="Error d'acc&eacute;s: No t&eacute; permisos." ForeColor="red" Font-Names="Verdana" Font-Size="10" runat="server" />
  </asp:PlaceHolder>
  <asp:PlaceHolder ID="contenedor" runat="server">
    <asp:TextBox ID="txtTraduccio" runat="server" Width="0" class="visibilidadOculta"></asp:TextBox>
    <asp:TextBox ID="txtCodiNode" runat="server" Width="0" class="visibilidadOculta"></asp:TextBox>
    <asp:ValidationSummary ID="valSum" runat="server" DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" HeaderText="Escriu un valor en els següents camps:" Font-Names="verdana" Font-Size="10" />
    <table border="0" width="100%" cellpadding="0" cellspacing="0" style="background-color: #E0E0FE;
            color: #000066; font-weight: bold;">
      <tr valign="middle">
        <td width="41"><img src="/img/gaia/logoGaiaP.gif" alt="logo GAIA" vspace="5" hspace="5"></td>
        <td width="682"><span>Manteniment d'enlla&ccedil;os</span></td>
        <td width="213"><span class="txtNeg12px">Canvi d'idioma:
          <asp:DropDownList runat="server" ID="lstCanviIdioma" AutoPostBack="true" OnSelectedIndexChanged="canviIdioma">
            <asp:ListItem Value="1">Catal&agrave;</asp:ListItem>
            <asp:ListItem Value="2">Castell&agrave;</asp:ListItem>
            <asp:ListItem Value="3">Angl&egrave;s</asp:ListItem>
            <asp:ListItem Value="4">Franc&egrave;s</asp:ListItem>
          </asp:DropDownList>
          </span> </td>
      </tr>
      <tr>
        <td colspan="3" height="1" bgcolor="#CCCCCC"></td>
      </tr>
    </table>
    <br>
        <img src="../img/cuadre_asterisc.png" alt="" style="margin-right: .5em;" /><asp:Label ID="lblOutput" Text="Camps obligatoris (La informació no es gravarà fins que no estiguin completats)" CssClass="arial bold negre t075" runat="server" />
  <br><img src="../img/cuadre_vermellfluix.gif" alt="" style="margin-right: .5em;" /><asp:Label ID="Label1" CssClass="arial bold negre t075" Text="Camps no visibles a Internet." runat="server" />
  <br><br>
    <table width="100%" border="1" cellspacing="1" cellpadding="1" class="tablabordepeqroj" bordercolor="#D3D3D3">
      <tr>
        <td colspan="4" bgcolor="#E4E4E4" class="negre bold t09 arial"> PAS 1. DADES DE L'ENLLA&Ccedil;</td>
      </tr>
      <tr>
        <td>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="20%" class="txtneg12px paddingDreta5" valign="middle" align="right">Adre&ccedil;a:</td>
              <td><asp:TextBox runat="server" ID="LNKDSLNK" Columns="60" OnChange="javascript:blanquejarCampAlternatiu();" onBlur="blanquejarCampAlternatiu();"></asp:TextBox></td>
              <td><lh:Ajuda ID="Ajuda1" Text="Per enlla&ccedil;os a webs externs poseu l'adreça on voleu enlla&ccedil;ar amb el prefix: 'http://'<br>  Per exemple: http://www.socUnWebExtern.cat" runat="server" /></td>
            </tr>
            <tr>
              <td class="txtneg12px paddingDreta5" valign="middle" align="right">Node relacionat:</td>
              <td><asp:TextBox ContentEditable="false" runat="server" ID="linkTxt" Columns="40" MaxLength="200"></asp:TextBox>
                <input type="button" value="Seleccionar" onClick="javascript:window.open('visorarbreslinks.aspx?c=link','_blank', 'location=0,height=900,width=560,scrollbars=yes,resizable=yes');" id="Seleccionar"><input type="button" id="eliminar" value="Esborrar" onClick="document.getElementById('linkNodes').value='';document.getElementById('linkTxt').value=''; return false;"><asp:TextBox runat="server" ID="linkNodes" Columns="4" MaxLength="200" CssClass="t60"></asp:TextBox></td>
              <td><lh:Ajuda ID="Ajuda2" Text="Per enlla&ccedil;os a continguts o p&agrave;gines de GAIA, premeu 'Seleccionar' per cercar la p&agrave;gina o contingut on voleu enlla&ccedil;ar" runat="server" /></td>
            </tr>
            <tr>
              <td class="txtneg12px paddingDreta5" valign="top" align="right"><img src="../img/cuadre_asterisc.png" alt="" style="margin-right: .5em;" />T&iacute;tol (text que normalment utilitzem per fer l'enlla&ccedil;):</td>
              <td><FCKeditorV2:FCKeditor id="LNKDSTXT" width="500" runat="server" height="80" basepath="/GAIA/aspx/FCKeditor.v2/"></FCKeditorV2:FCKeditor></td>
              <td><lh:Ajuda ID="Ajuda3" Text="Text volem que aparegui com enlla&ccedil;" runat="server" /></td>
            </tr>
            <tr>
              <td class="txtneg12px paddingDreta5" valign="top" align="right">Descripci&oacute;:</td>
              <td><FCKeditorV2:FCKeditor id="LNKDSDES" width="500" runat="server" height="100" basepath=" /GAIA/aspx/FCKeditor.v2/"></FCKeditorV2:FCKeditor></td>
              <td><lh:Ajuda ID="Ajuda4" Text="Text explicatiu que apareix sota el text d'enlla&ccedil;" runat="server" /></td>
            </tr>
            <tr>
              <td class="txtneg12px paddingDreta5" valign="middle" align="right">Dest&iacute;:</td>
              <td>
                <asp:DropDownList runat="server" ID="LNKWNTIP" CssClass="txtNeg12px">
                  <asp:ListItem Value="0">Mateixa finestra</asp:ListItem>
                  <asp:ListItem Value="1">Nova Finestra</asp:ListItem>
                </asp:DropDownList>
               </td>
              <td><lh:Ajuda ID="Ajuda5" Text="Indiqueu on voleu que s'obri l'enlla&ccedil;, en la mateixa finestra o en una nova finestra." runat="server" /></td>
            </tr>
            <tr>
              <td class="txtneg12px paddingDreta5" valign="middle" align="right">Idioma:</td>
              <td>
                <asp:DropDownList runat="server" ID="lstIdioma" CssClass="txtNeg12px">
                  <asp:ListItem Value="1">Catal&agrave;</asp:ListItem>
                  <asp:ListItem Value="2">Castell&agrave;</asp:ListItem>
                  <asp:ListItem Value="3">Angl&egrave;s</asp:ListItem>
                  <asp:ListItem Value="4">Franc&egrave;s</asp:ListItem>
                </asp:DropDownList></td>
              <td><lh:Ajuda ID="Ajuda6" Text="Indica en quina versi&oacute; d'idioma estem visualitzant i introduint la informaci&oacute;" runat="server" /></td>
            </tr>
            
            <tr class="fonsVermellFCECE7 paddingTop10 paddingBottom10">
              <td class="txtneg12px paddingDreta5" valign="middle" align="right"><asp:RequiredFieldValidator ID="LNKDTPUBReqVal" ControlToValidate="LNKDTPUB" ErrorMessage="Data publicaci&oacute; " Display="Static" InitialValue="" runat="server"><img src="../img/cuadre_asterisc.png" alt="" style="margin-right: .5em;" /></asp:RequiredFieldValidator>
Data publicaci&oacute;:</td>
              <td class="txtneg12px"><asp:TextBox runat="server" ID="LNKDTPUB" Columns="10" MaxLength="10"></asp:TextBox><a href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=LNKDTPUB','calendar_window','width=150,height=188');calendar_window.focus()"><img src="/img/common/iconografia/ico_calendari.png" border="0" align="absmiddle"></a><asp:RangeValidator ID="LNKDTPUBrangeValDate" Type="Date" ControlToValidate="LNKDTPUB" ErrorMessage="Format incorrecte de data publicaci&oacute; (DD/MM/AAAA) " Display="None" MaximumValue="1/1/2100" MinimumValue="1/1/1900" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Hora
                <asp:TextBox CssClass="txtNeg12px" ID="HoraPUB" Columns="5" MaxLength="5" Text="00:00" runat="server" TextMode="SingleLine"></asp:TextBox></td>
              <td><lh:Ajuda ID="Ajuda7" Text="Data i hora que volem que aquest contingut aparegui autom&agrave;ticament a Internet" runat="server" />
              </td>
            </tr>
            <tr class="fonsVermellFCECE7 paddingBottom10">
              <td class="txtneg12px paddingDreta5" align="right" valign="middle"><asp:RangeValidator ID="LNKDTCADrangeValDate" Type="Date" ControlToValidate="LNKDTCAD" ErrorMessage="Format incorrecte de data caducitat (DD/MM/AAAA) " Display="None" MaximumValue="1/1/2100" MinimumValue="1/1/1900" runat="server" />
Data caducitat:</td>
              <td class="txtneg12px"><asp:TextBox runat="server" ID="LNKDTCAD" Columns="10" MaxLength="10"></asp:TextBox><a href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=LNKDTCAD','calendar_window','width=150,height=188');calendar_window.focus()"><img src="/img/common/iconografia/ico_calendari.png" border="0" align="absmiddle"></a>
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Hora</span><asp:TextBox CssClass="txtNeg12px" ID="HoraCAD" Columns="5" MaxLength="5" Text="00:00" runat="server" TextMode="SingleLine"></asp:TextBox></td>
              <td><lh:Ajuda ID="Ajuda8" Text="Data i hora que volem que aquest contingut desaparegui autom&agrave;ticament d'Internet" runat="server" /></td>
            </tr>
             <tr>
              <td class="txtneg12px paddingDreta5" valign="top" align="right">Enllaç a vídeo:</td>
              <td><asp:textbox runat="server" id="LNKDSVID" Rows="7" Columns="55" MaxLength="1000" TextMode="MultiLine" Wrap="true"></asp:textbox> </td>
              <td><lh:Ajuda  Text="Codi web per inscrustar un visor de vídeos de youtube, vimeo, etc." runat="server" /></td>
            </tr>
          </table>
          </td>
      </tr>
    </table>
    <table width="100%" border="1" cellspacing="1" cellpadding="1" class="tablabordepeqroj"
            bordercolor="#D3D3D3">
      <tr>
        <td colspan="5" bgcolor="#E4E4E4" class="negre bold t09 arial"> PAS 2. AFEGIR IMATGES AL CONTINGUT (per a tots els idiomes) </td>
      </tr>
      <tr>
        <td><span style="display: inline; margin-top: 0px; float: right;">
          <lh:Ajuda ID="Ajuda12" Text="<u>T&iacute;tol:</u> Descripci&oacute; breu de la imatge<br> <u>Text alternatiu:</u> Text que expliqui que es veu a la imatge per a persones amb defici&egrave;ncies visuals. Nom&eacute;s &eacute;s necessari en cas que la imatge sigui significativa."
                            runat="server" />
          </span>
          <UpImgPrev:UpImgPrev ID="Prev" name="Prev" runat="server" EnableViewState="True"></UpImgPrev:UpImgPrev>
        </td>
      </tr>
    </table>
    <table width="100%" border="1" cellspacing="1" cellpadding="1" class="tablabordepeqroj"
            bordercolor="#D3D3D3">
      <tr>
        <td colspan="5" bgcolor="#E4E4E4" class="negre bold t09 arial"> PAS 3. AFEGIR DOCUMENTS AL CONTINGUT (per a tots els idiomes) </td>
      </tr>
      <tr>
        <td><span style="display: inline; margin-top: 0px; float: right;">
          <lh:Ajuda ID="Ajuda13" Text="<u>T&iacute;tol:</u> Nom del document. <br> <u>Text alternatiu:</u> Descripci&oacute; breu del document per a persones amb defici&egrave;ncies visuals. Nom&eacute;s &eacute;s necessari si el t&iacute;tol no &eacute;s autoexplicatiu."
                            runat="server" />
          </span>
          <UpDocPrev:UpDocPrev ID="docPrev" name="docPrev" runat="server" EnableViewState="True"> </UpDocPrev:UpDocPrev>
        </td>
      </tr>
    </table>
    
    <asp:Panel Visible="false" runat="server" ID="panelTraduccio">
      <table width="100%" border="1" cellspacing="1" cellpadding="1" class="tablabordepeqroj"
                bordercolor="#D3D3D3">
        <tr>
          <td colspan="4" bgcolor="#E4E4E4" class="negre bold t09 arial"> PAS 4. TRADUCCI&Oacute; DEL CONTINGUT</td>
        </tr>
        <tr>
          <td height="1"><span class="txtNeg12px">Selecciona l'idioma de traducci&oacute;: </span>
            <asp:Label runat="server" ID="lblIdiomaTraduccio"></asp:Label>
            <input type="button" id="traduccio" value="Traducció" onserverclick="clickTraduccio" runat="server" onClick="document.getElementById('txtTraduccio').value='1';">
            &nbsp;</td>
        </tr>
      </table>
      </asp:Panel>
    
      
	  <center>
      <input type="button" id="upload" value="Afegir enllaç" onserverclick="clickAfegirLink" runat="server" class="botoAcceptarModificar">
      <asp:panel runat="server" id="pnlEsborrar" visible="true" CssClass="valignMiddle displayInline">     
     <input type="button" class="botoEsborrar" id="esborrarContingut" value="esborrar contingut" onserverclick="clickEsborrar" onClick="if (!confirm('S´eliminarà el contingut de totes les ubicacions on es trobi')) { return false;}" runat="server" visible="true">
     </asp:panel>
      </center>

  </asp:placeholder>
</form>
<asp:Label ID="lblCodi" runat="server" />
<!--#INCLUDE VIRTUAL="/inc/peu.aspx" -->
</body>
</html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="wsTradAuto" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="System.Web" %>
<script runat="server" src="../llibreria/frmcarregalinks.aspx.vb"></script>
