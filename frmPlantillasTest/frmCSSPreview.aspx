<identity impersonate="true"/>
<%@ Page Language="vb" AutoEventWireup="false"   validateRequest=false%>
<!DOCTYPE HTML>
<html lang="es">

<head>

<title>Intranet de l'Ajuntament de L'Hospitalet</title>

<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<link rel="stylesheet" href="/css/intranet.css" type="text/css">
</head>

<body bgcolor="#FFFFFF" text="#000000" leftmargin="10" topmargin="10" >

<form runat="server">
      <%@ Register TagPrefix="menuG" TagName="menuG" Src="js/App_LocalResources/menu.ascx" %>
    <menuG:menug ID="menuG" Text="Menú GAIA" runat="server"/> 

<asp:label runat="server" id="lblEstructura"></asp:label> 
</form>
</body>
</html>
