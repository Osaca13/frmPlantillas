<%@ Page Language="vb" AutoEventWireup="false" debug="false"%>
<%@ Register TagPrefix="radT" Namespace="Telerik.WebControls" Assembly="RadTreeView" %>
<%@ Register TagPrefix="lh" TagName="Ajuda" Src="../Ajuda/Ajuda.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">

<!-- DW6 -->
<head>
 
    <title>Document</title>
   
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
   <link rel="stylesheet" href="/css/intranet.css" type="text/css">
    <link rel="stylesheet" type="text/css" href="/css/gaiaIntranet.css" />
    <link href="../Ajuda/Ajuda.css" rel="stylesheet" type="text/css"/>

    <script language="JavaScript" type="text/javascript" src="../Ajuda/Ajuda.js"></script>
    <script src="lhintranet/js/jquery.min.js" type="text/javascript"></script>

</head>
<body bgcolor="#FFFFFF" text="#000000" leftmargin="10" topmargin="10">
    <!--#INCLUDE VIRTUAL="~/js/App_LocalResources/cap.aspx" -->

    <form runat="server" enctype="multipart/form-data">
       <%@ Register TagPrefix="menuG" TagName="menuG" Src="~/js/App_LocalResources/menu.ascx" %>
    <menuG:menug ID="menuG" Text="Menú GAIA" runat="server"/>
        <asp:Label ID="lbldebug" runat="server" />
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
                <div class="icona">
                <asp:literal runat="server" id="lblOk"></asp:literal>
             </div>
            <span class="bottomEsquerraBlanc"></span>
            <span class="bottomDretaBlanc"></span>
        </div>
        </asp:panel>
        <asp:TextBox ID="txtCodiNode" runat="server" Width="0" class="visibilidadOculta"></asp:TextBox>
        <asp:TextBox ID="txtNomFitxer" runat="server" Width="0" class="visibilidadOculta"></asp:TextBox>
        <asp:TextBox ID="txtTipusDocument" runat="server" Width="0" class="visibilidadOculta"></asp:TextBox>
        <asp:TextBox ID="txtTHor" runat="server" Width="0" class="visibilidadOculta"></asp:TextBox>
        <asp:TextBox ID="txtTVer" runat="server" Width="0" class="visibilidadOculta"></asp:TextBox>
        <asp:TextBox ID="txtTamany" runat="server" Width="0" class="visibilidadOculta"></asp:TextBox>
        <asp:TextBox ID="txtTraduccio" runat="server" Width="0" class="visibilidadOculta"></asp:TextBox>
        <asp:TextBox ID="afegit" runat="server" Width="0" class="visibilidadOculta"></asp:TextBox>
        <asp:ValidationSummary ID="valSum" runat="server" DisplayMode="BulletList" ShowMessageBox="true"
            ShowSummary="false" HeaderText="Escriu un valor en els següents camps:" Font-Names="verdana"
            Font-Size="10" />
        <asp:PlaceHolder ID="errorPermisos" Visible="false" runat="server">
            <br>
            <asp:Label ID="Label3" Text="Error d'acc&eacute;s: No t&eacute; permisos." ForeColor="red"
                Font-Names="Verdana" Font-Size="20" runat="server" />
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="contenedor" runat="server">
   <asp:Label ID="Label2" Text="" CssClass="blancSobreVermell" runat="server" />
   <br>
   <img src="../img/cuadre_asterisc.png" alt="Color de fons per als camps no visibles a Internet" style="margin-right: .5em;" /><asp:Label ID="lblOutput" Text="Camps obligatoris (La informació no es gravarà fins que no estiguin completats)" CssClass="arial bold negre t075" runat="server" />
  <br><img src="../img/cuadre_vermellfluix.gif" alt="Color de fons per als camps no visibles a Internet" style="margin-right: .5em;" /><asp:Label ID="Label1" CssClass="arial bold negre t075" Text="Camps no visibles a Internet." runat="server" />
  <br><br>
            <table border="0" width="100%" cellpadding="0" cellspacing="0" style="background-color: #E0E0FE;
                color: #000066; font-weight: bold;">
                <tr valign="middle">
                    <td width="46">
                        <img src="/img/gaia/logoGaiaP.gif" alt="logo GAIA" vspace="5" hspace="5"></td>
              <td width="633">
                        <span>Manteniment de documents </span>                    </td>
              <td width="348">
                        <span class="txtNeg12px">Canvi d'idioma:
                  <asp:DropDownList runat="server" ID="lstCanviIdioma" AutoPostBack="true" OnSelectedIndexChanged="canviIdioma">
                                <asp:ListItem Value="1">Catal&agrave;</asp:ListItem>
                                <asp:ListItem Value="2">Castell&agrave;</asp:ListItem>
                                <asp:ListItem Value="3">Angl&egrave;s</asp:ListItem>
                                <asp:ListItem Value="4">Franc&egrave;s</asp:ListItem>
                            </asp:DropDownList>
                        </span>
                    </td>
              </tr>
            </table>
            <table width="100%" border="1" cellspacing="1" cellpadding="1" class="tablabordepeqroj"
                bordercolor="#D3D3D3">
                <tr>
                    <td colspan="4" bgcolor="#E4E4E4" class="negre bold t09 arial">
                        PAS 1. DOCUMENTS
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="25%" class="txtneg12px">
                                    <div align="right">
                                        <asp:RequiredFieldValidator ID="DOCDSTITReqVal" ControlToValidate="DOCDSTIT" ErrorMessage="Títol"
                                            Display="Static" InitialValue="" runat="server"><img src="../img/cuadre_asterisc.png" alt="Color de fons per als camps no visibles a Internet" style="margin-right: .5em;" /></asp:RequiredFieldValidator>
                                        T&iacute;tol&nbsp;&nbsp;</div>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="DOCDSTIT" Columns="60" MaxLength="600"></asp:TextBox></td>
                                <td>
                                    <lh:Ajuda ID="Ajuda1" Text="Nom del document. En cas de legislaci&oacute; s'aconsella afegir aqu&iacute; la refer&egrave;ncia num&egrave;rica de la llei, la data d'aprovaci&oacute; i l'estat d'aprovaci&oacute; (Aprovaci&oacute; inicial, definitiva o en vigor)"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr class="fonsVermellFCECE7">
                                <td class="txtneg12px">
                                    <div align="right">
                                        Idioma&nbsp;&nbsp;</div>
                                </td>
                                <td colspan="2">
                                    <span class="txtNeg12px">
                                        <asp:DropDownList runat="server" ID="lstIdioma" AutoPostBack="false">
                                            <asp:ListItem Value="1">Catal&agrave;</asp:ListItem>
                                            <asp:ListItem Value="2">Castell&agrave;</asp:ListItem>
                                            <asp:ListItem Value="3">Angl&egrave;s</asp:ListItem>
                                            <asp:ListItem Value="4">Franc&egrave;s</asp:ListItem>
                                        </asp:DropDownList>
                                    </span>
                                </td>
                                <td>
                                    <lh:Ajuda ID="Ajuda2" Text="Indica en quina versi&oacute; d'idioma estem visualitzant i introduint la informaci&oacute;"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="txtneg12px">
                                    <div align="right">
                                        Autor&nbsp;&nbsp;</div>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="DOCDSAUT" Columns="60" MaxLength="100"></asp:TextBox></td>
                                <td>
                                    <lh:Ajuda ID="Ajuda3" Text="Autor del document" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="txtneg12px">
                                    <div align="right">
                                        Font&nbsp;&nbsp;</div>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="DOCDSFON" Columns="60" MaxLength="100"></asp:TextBox></td>
                                <td>
                                    <lh:Ajuda ID="Ajuda4" Text="Proced&egrave;ncia del document" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="txtneg12px">
                                    <div align="right">
                                        ISBN&nbsp;&nbsp;</div>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="DOCDSISB" Columns="60" MaxLength="100"></asp:TextBox></td>
                                <td>
                                    <lh:Ajuda ID="Ajuda5" Text="«N&uacute;mero Internacional Est&agrave;ndard del Llibre». &Eacute;s un identificador &uacute;nic per cada edici&oacute; d'un llibre o publicaci&oacute; "
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="txtneg12px" valign="top">
                                    <div align="right">
                                        Descripci&oacute; / Paraules clau&nbsp;&nbsp;
                                    </div>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="DOCDSDES" Rows="5" Columns="62" MaxLength="1000" TextMode="MultiLine" Wrap="true"/></td>
                                <td>
                                    <lh:Ajuda ID="Ajuda6" Text="S&oacute;n paraules que fan refer&egrave;ncia al contingut del document per fer m&eacute;s r&agrave;pides les cerques"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="txtneg12px">
                                    <div align="right">
                                        Text alternatiu&nbsp;
                                    </div>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="DOCDSALT" Columns="60" MaxLength="100" /></td>
                                <td>
                                    <lh:Ajuda ID="Ajuda8" Text="Descripci&oacute; breu del document per a persones amb defici&egrave;ncies visuals. Nom&eacute;s &eacute;s necessari si el t&iacute;tol no &eacute;s autoexplicatiu o es tracta d'una imatge significativa per entendre la resta d'informaci&oacute;"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="txtneg12px">
                                    <div align="right">
                                        Enlla&ccedil; del document &nbsp;&nbsp;
                                    </div>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox runat="server" ID="DOCDSLNK" Columns="60"/></td>
                                <td>
                                    <lh:Ajuda ID="Ajuda7" Text="Per imatges, permet associar un enlla&ccedil; a la imatge. Al  fer clic sobre la imatge s'executar&agrave; l'enlla&ccedil;"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr class="fonsVermellFCECE7">
                                <td class="txtneg12px">
                                    <div align="right">
                                        <asp:RangeValidator ID="DOCDTANYrangeValDate" Type="Date" ControlToValidate="DOCDTANY"
                                            ErrorMessage="Format incorrecte de data de creació(DD/MM/AAAA) " Display="None"
                                            MaximumValue="1/1/2100" MinimumValue="1/1/1900" runat="server" />
                                        Data creaci&oacute;&nbsp;&nbsp;</div>
                                </td>
                                <td width="9%">
                                    <asp:TextBox runat="server" ID="DOCDTANY" Columns="10" MaxLength="10"></asp:TextBox></td>
                                <td width="66%">
                                    <a href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=DOCDTANY','calendar_window','width=150,height=188');calendar_window.focus()">
                                        <img src="/img/common/iconografia/ico_calendari.png" border="0"></a>
                                </td>
                                <td>
                                    <lh:Ajuda ID="Ajuda9" Text="Data que es guarda per mantenir un control intern dels continguts per&ograve; no afecta a la seva publicaci&oacute; o caducitat"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr class="fonsVermellFCECE7">
                                <td class="txtneg12px">
                                    <div align="right">
                                        <asp:RangeValidator ID="DOCDTPUBrangeValDate" Type="Date" ControlToValidate="DOCDTPUB"
                                            ErrorMessage="Format incorrecte de data document actiu(DD/MM/AAAA) " Display="None"
                                            MaximumValue="1/1/2100" MinimumValue="1/1/1900" runat="server" />
                                        Data document actiu &nbsp;</div>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="DOCDTPUB" Columns="10" MaxLength="10"></asp:TextBox></td>
                                <td>
                                    <a href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=DOCDTPUB','calendar_window','width=150,height=188');calendar_window.focus()">
                                        <img src="/img/common/iconografia/ico_calendari.png" border="0"></a></td>
                                <td>
                                    <lh:Ajuda ID="Ajuda10" Text="Data en que volem que aquest document sigui consultable"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr class="fonsVermellFCECE7">
                                <td class="txtneg12px">
                                    <div align="right">
                                        <asp:RangeValidator ID="DOCDTCADrangeValDate" Type="Date" ControlToValidate="DOCDTCAD"
                                            ErrorMessage="Format incorrecte de data caducitat(DD/MM/AAAA) " Display="None"
                                            MaximumValue="1/1/2100" MinimumValue="1/1/1900" runat="server" />
                                        Data caducitat &nbsp;</div>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="DOCDTCAD" Columns="10" MaxLength="10"></asp:TextBox></td>
                                <td>
                                    <a href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=DOCDTCAD','calendar_window','width=150,height=188');calendar_window.focus()">
                                        <img src="/img/common/iconografia/ico_calendari.png" border="0"></a></td>
                                <td>
                                    <lh:Ajuda ID="Ajuda11" Text="Data en que volem que aquest document deixi de ser consultable"
                                        runat="server" />
                                </td>
                            </tr>
                            <asp:PlaceHolder runat="server" ID="copiaArbrePers">
                                <tr class="fonsVermellFCECE7">
                                    <td width="100%" height="1" valign="top" class="txtneg12px" align="right">
                                        Guardar c&ograve;pia del document a&nbsp;<br>
                                        a l'arbre personal?&nbsp;</td>
                                    <td colspan="2">
                                        <asp:CheckBox runat="server" Text="" Checked="true" ID="DOCGRDAP"></asp:CheckBox></td>
                                    <td>
                                        <lh:Ajuda ID="Ajuda12" Text="Si es marca, la not&iacute;cia quedar&agrave; emmagatzemada al vostre arbre 'elMeuEspai' . En cas de no codificar la not&iacute;cia aquesta opci&oacute; es marcar&agrave; autom&agrave;ticament."
                                            runat="server" />
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <tr class="fonsVermellFCECE7" style="padding-top: 5px;">
                                <td height="58" valign="top" class="txtneg12px" align="right">
                                    Esborrar definitivament el document una vegada caducat?
                                </td>
                                <td colspan="2">
                                    <div class="floatleft">
                                        <asp:CheckBox runat="server" Text="" Checked="false" ID="chkDOCWNBOR"></asp:CheckBox>&nbsp;&nbsp;</div>
                                </td>
                                <td>
                                    <lh:Ajuda ID="Ajuda13" Text="Atenci&oacute;: GAIA sempre guarda una c&ograve;pia dels documents encara que hagin caducat. Amb aquesta opci&oacute; els documents s'esborraran definitivament i no es podran recuperar, cal utilitzar-la amb compte."
                                        runat="server" />
                                </td>
                            </tr>
                            <asp:Panel Visible="false" runat="server" ID="panelCanviDocument">
                                <tr>
                                    <td class="txtneg12px">
                                        <div align="right">
                                            Document actual&nbsp;&nbsp;</div>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label runat="server" ID="LBLenllaçDocument"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="txtneg12px">
                                        <div align="right">
                                            Nou Document &nbsp;&nbsp;</div>
                                    </td>
                                    <td colspan="2">
                                        <input id="nouUploadedFile" type="file" runat="server" size="60" src=""></td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel Visible="true" runat="server" ID="panelNouDocument">
                                <tr>
                                    <td class="txtneg12px">
                                        <div align="right">
                                            <img src="../img/cuadre_asterisc.png" alt="Color de fons per als camps no visibles a Internet" style="margin-right: .5em;" /> Document&nbsp;&nbsp;</div>
                                    </td>
                                    <td colspan="2">
                                        <input id="uploadedFile" type="file" runat="server" size="60" src="" /></td>
                                    <td>
                                        <lh:Ajuda ID="Ajuda14" Text="Cerqueu el document al vostre PC. L'arxiu es copiar&agrave; en el servidor, podreu esborrar o modificar el document en la seva ubicaci&oacute; original sense que afecti aqu&iacute;"
                                            runat="server" />
                                    </td>
                                </tr>
                            </asp:Panel>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%" border="1" cellspacing="1" cellpadding="1" class="tablabordepeqroj"
                bordercolor="#D3D3D3">
                <tr>
                    <td colspan="5" bgcolor="#E4E4E4" class="negre bold t09 arial">
                        PAS 2. CODIFICACI&Oacute; DEL CONTINGUT (per a tots els idiomes)
                    </td>
                </tr>
                <tr class="fonsVermellFCECE7">
                    <td width="100%" height="1" valign="top">
                        <span style="display: inline; margin-top: 0px; float: right;">
                            <lh:Ajuda ID="Ajuda15" Text="Permet codificar el document segons la classificaci&oacute; de l'arbre de Documents marcant  els elements de l'arbre que considereu convenients."
                                runat="server" />
                        </span>
                        <radT:RADTREEVIEW contentFile="../codificacio/arbre12367.xml" id="RadTree1" runat="server"
                            RetainScrollPosition="True" AutoPostBack="false" CheckBoxes="True" ImagesBaseDir="/img/common/iconografia/"
                            CssFile=" /GAIA/aspx/Examples/Advanced/LoadOnDemand/tree.css" MultipleSelect="True"
                            BeforeClientClick="return false;">
                        </radT:RADTREEVIEW>
                    </td>
                </tr>
            </table>
            <br>
            <br>
            <div align="center">
                <input type="button" id="upload" value="Afegir document" onclick="if (!Validator()) return false;" onserverclick="clickAfegirDocument"
                    runat="server">
                <br>
                <asp:Panel Visible="false" runat="server" ID="panelTraduccio">
                    <table width="100%" border="1" cellspacing="1" cellpadding="1" class="tablabordepeqroj"
                        bordercolor="#D3D3D3">
                        <tr>
                            <td colspan="4" bgcolor="#E4E4E4" class="negre bold t09 arial">
                                PAS 3. TRADUCCI&Oacute; DEL CONTINGUT
                            </td>
                        </tr>
                        <tr>
                            <td height="1">
                                <span class="txtNeg12px">Selecciona l'idioma de traducci&oacute;: </span>
                                <asp:Label runat="server" ID="lblIdiomaTraduccio"></asp:Label>
                                <input type="button" id="traduccio" value="Traducci&oacute;" onserverclick="clickTraduccio"
                                    runat="server" onClick="document.getElementById('txtTraduccio').value='1';">
                                &nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
					<asp:panel runat="server" id="pnlEsborrar" visible="true">
		 <table width="100%" border="1" cellspacing="1" cellpadding="1" class="tablabordepeqroj" bordercolor="#D3D3D3">
		<tr><td colspan="4" bgcolor="#E4E4E4" class="negre bold t09 arial">ESBORRAR CONTINGUT</td></tr>
	<tr><td align="center"><input type="button" Class="blancSobreVermell" id="esborrarContingut" value="esborrar contingut" onserverclick="clickEsborrar" onClick="if (!confirm('S´eliminarà el contingut de totes les ubicacions on es trobi')) { return false;}" runat="server" visible="true"></td></tr>
		 </table>
		</asp:panel>
            </div>
        </asp:PlaceHolder>
    </form>
    <br>
    <br>
   <!--#INCLUDE VIRTUAL="/inc/peu.aspx" -->
    <script type="text/javascript" >
       function Validator() {
           var filename = $("#uploadedFile").val();
           var noufilename = $("#nouUploadedFile").val();
           var enlace = $("#LBLenllaçDocument").html();
           if (!filename && !noufilename && !enlace) {
               alert("Cap document seleccionat");

               return false;
           }
           else { return true; }
       }

   </script>
</body>

</html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="Telerik.WebControls" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Drawing.Imaging" %>

<script runat="server" src="../llibreria/frmCarregaDocuments.aspx.vb"></script>