<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmObrirFulla.aspx.vb" Inherits="frmPlantillasTest.frmObrirFulla" ValidateRequest="false" Debug="true" EnableViewStateMac="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
   <title>GAIA.Publicacio web</title>

<link rel="stylesheet" type="text/css" href="/css/intranet.css"/>
<link rel="stylesheet" type="text/css" href="/css/gaiaIntranet.css"/>
<link rel="stylesheet" type="text/css" href="/css/frmTramits.css"/>
</head>
<body>
<!--#INCLUDE VIRTUAL="~/js/App_LocalResources/cap.aspx" -->
<%@ Register TagPrefix="menuG" TagName="menuG" Src="~/js/App_LocalResources/menu.ascx" %>
    <menuG:menug ID="menuG" Text="Menu GAIA" runat="server"/>
     <table width="100%" border="0" cellpadding="0" cellspacing="0" style="background-color: #E0E0FE;
            color: #000066; font-weight: bold;">
            <tr valign="middle">
                <td width="41"><img src="/img/logoGaiaP.gif" alt="logo GAIA" hspace="5" vspace="5"></td>
                <td >Publicaci� de p�gines</td>
               
            </tr>
            <tr>
                <td colspan="2" height="1" bgcolor="#CCCCCC">
                </td>
            </tr>
        </table><br />
    
<asp:label id="lblCodi" runat="server"></asp:label> 
<asp:label id="lblResultat" runat="server" ></asp:label>

<script language="javascript" type="text/javascript">
/*
markup = new String(document.body.innerHTML);
document.body.innerHTML = markup.replace("/gDocs", "/aplics/GAIA/documents");
*/
</script>
<!--#INCLUDE VIRTUAL="~/js/App_LocalResources/peu.aspx" -->
</body>

</html>
