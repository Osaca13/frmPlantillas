<identity impersonate="true"/>
<%@ Page Language="vb" AutoEventWireup="false"   %>
<html><!-- #BeginTemplate "../../../Templates/p_interior01.dwt" --><!-- DW6 -->
<head>
<!-- #BeginEditable "doctitle" --> 
<title>Intranet de l'Ajuntament de L'Hospitalet</title>
<!-- #EndEditable -->
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<link rel="stylesheet" href="/css/intranet.css" type="text/css">
<link rel="stylesheet" href="/css/gaiaIntranet.css" type="text/css">
</head>

<body bgcolor="#FFFFFF" text="#000000" leftmargin="10" topmargin="10">
<!--#INCLUDE VIRTUAL="/inc/cap.inc" -->
<!-- #BeginEditable "Content" -->
<form runat="server">
<asp:Label ID="lbldebug" runat="server"/>
<asp:Label ID="lblResultat" runat="server"/>


           <table width="100%" border="1" cellspacing="1" cellpadding="1"   class="tablabordepeqroj" bordercolor="#FFCDCA">
                <tr>       
	        	<td colspan="4" bgcolor="#E6E6E6" class="txtrojo12px">Missatges</td>
                </tr>
                <tr> 
                  <td><table width="100%" >
                    <tr  class="txtrojo12px">
                      <td width="13%">Rebut</td>
                      <td width="30%">Títol</td>
                      <td width="57%">Contingut al que fa referència</td>
                      <td width="0%">&nbsp;</td>
                    </tr>
										
										<asp:Label ID="lblLlistatMissatges" runat="server"/>
                  </table></td>
                </tr></table>              
            
</form>

<!-- #EndEditable -->
<!--#INCLUDE VIRTUAL="/inc/peu.aspx" -->
</body>
<!-- #EndTemplate --></html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="Telerik.WebControls"%>

<script runat="server"  src="../llibreria/visorNodeMissatges.aspx.vb"></script>