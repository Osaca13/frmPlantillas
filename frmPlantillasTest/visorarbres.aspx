<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VisorArbres.aspx.vb" Inherits="frmPlantillasTest.VisorArbres" ValidateRequest="false" EnableViewStateMac="false" Debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
    <meta http-equiv="Page-Enter" content="revealTrans(Duration=0,Transition=5)">
    <%@ Register TagPrefix="radT" Namespace="Telerik.WebControls" Assembly="RadTreeView" %>
    <script type="text/JavaScript">
    
        function MM_reloadPage(init) {  //reloads the window if Nav4 resized
          if (init==true) with (navigator) {if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
            document.MM_pgW=innerWidth; document.MM_pgH=innerHeight; onresize=MM_reloadPage; }}
          else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) location.reload();
        }
    MM_reloadPage(true);
    </script>
    <style type="text/css">

.ContextItem
{
	font-family: Tahoma;
	font-size: 9pt;
	color: black;

}

.ContextItemOver
{
	font-family: Tahoma;
	font-size: 9pt;
	color: black;
	background-color: #FF8080;
	cursor: hand;
}
.unnamed1 {
	border: 10px outset;
	margin: 20px;
}
body{
    background: #ffffff;
    color: #000000;
    margin-left: 10px;
    margin-top: 10px

}
</style>
    <title>Visor d'arbres</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
    <link href="Styles/intranet.css" rel="stylesheet" type="text/css"/>
    <link href="Styles/gaiaIntranet.css" rel="stylesheet" type="text/css"/>
</head>
<body >
<!--#INCLUDE VIRTUAL="~/js/App_LocalResources/cap.aspx" -->

 <%  	
    If 2 = 4 And Session("login") <> True Then
%>
     <script>
     window.location.href="~/asp/areaPersonal.asp"
    </script>
 <%End IF%>
<script type="text/javascript">

function ContextClicked(node, itemText) 
{ 
	if (itemText=="Cercar") 
	{
		document.getElementById("arbreTreeView").value= node.TreeView.ID;
		
		document.getElementById("capaCercarNode").style.visibility = "visible";
		document.getElementById("cerca").focus();
	
	}
	if (itemText=="Insertar") 
	{
		document.getElementById("arbreTreeView").value= node.TreeView.ID;
		document.getElementById("capaInsertarNode").style.visibility = 'visible';
		document.getElementById("tipusNode").focus();
	}    
	
	if (itemText== "Esborrar") 
	{
		if (!confirm("S'esborrar� el contingut nom�s de la ubicaci� seleccionada. Segur que ho vols fer?")) 
        { 
            return(false); 
           // sender.get_contextMenus()[0].hide(); 
        } 
	}
	if (itemText== "Esborrar tots") 
	{
		if (!confirm("S'eliminar� el contingut de totes les ubicacions on es trobi. Segur que ho vols fer?")) 
        { 
            return(false); 
           // sender.get_contextMenus()[0].hide(); 
        } 
	}
	if (itemText== "Caducar") 
	{
		if (!confirm("El contingut es marcar� com caducat i les p�gines on aparegui s'actualitzaran per reflectir el canvi. Segur que ho vols fer?")) 
        { 
            return(false); 
           // sender.get_contextMenus()[0].hide(); 
        } 
	}
}


function seleccionaNodes(Node)
{			
	var arbre,separador,inici;		
    inici = 0;
    if (Node != null) {
        separador = Node.Value.indexOf("-", inici);
        document.getElementById('nodes').value = Node.Value.substr(inici, separador - inici);
        //alert(event.keyCode);
    } else
    {
        document.getElementById('nodes').value = "226870"
    }
	
}
function abansDrop(Node)
{
//alert(event.keyCode);
}
</script>
   
 <script runat = "server" >
     Public Function RequestNodes() As String
         RequestNodes = Page.Request("nodes")
         If RequestNodes Is Nothing Then
             RequestNodes = "321973"
         End If

     End Function

</script>
 <script runat="server">
     Public Function RequestArbre1() As String
         RequestArbre1 = Page.Request("arbre1")
         If RequestNodes() Is Nothing Then
             RequestArbre1 = "elMeuEspai"
         End If
     End Function
</script>
     
 <form  name="form1" id="form1" runat="server" >
        <input id="nodes" name="nodes" type="hidden" value="<% =RequestNodes() %>" />
        <input name="arbre1" type="hidden" value="<%=RequestArbre1() %>" />	

 <%@ Register TagPrefix="menuG" TagName="menuG" Src="~/js/App_LocalResources/menu.ascx" %>
    <menuG:menug ID="menuG" Text="Menu GAIA" runat="server"/>

              <table width="100%" height="1" border="0" cellpadding="0" cellspacing="0">
                <tr>
                  <td  width="50%"   valign="top">
									
                  <table width="100%"  cellpadding="0" cellspacing="0"  >
                      <tr height="35px">
                        <td width="13%" valign="bottom" bgcolor="#E0E0FE"  class="txtNeg14px"  ><img src="img/logoGAIAP.gif" alt="GAIA" hspace="0" vspace="0" border="0">&nbsp;</td>
                        <td width="87%" align="right"  valign="baseline" bgcolor="#E0E0FE"  class="txtNeg12px" style="padding: 10">
                        <div align="right" >
                            <asp:DropDownList runat='server' ID="llistaArbres" DataTextField="TIPDSDES" DataValueField="TIPINTIP" OnSelectedIndexChanged="llistaArbres_canviArbre" AutoPostBack="true" ></asp:DropDownList>
                            <% If (Request("arbre2") Is Nothing) THEN %>
                            <% IF llistaArbres2_1.SelectedIndex = 0 THEN%>                
                           <asp:DropDownList runat='server' ID="llistaArbres2_1" DataTextField="TIPDSDES" DataValueField="TIPINTIP" OnSelectedIndexChanged="llistaArbres2_1_canviArbre" autopostback="true"></asp:DropDownList> 
                            <%END IF%>
                            <%END IF%>
                        </span></div></td>
                      </tr>
                      <tr>
                        <td height="4" colspan="2" bgcolor="#5F61C0"></td>
                      </tr>
                   <tr>
                   
                   <td colspan="2" class="TreeContext">             
                    <RADT:RADTREEVIEW ID="RadTree1"
											runat="server"
											RetainScrollPosition="True" 
											AutoPostBack="false" 
											CheckBoxes="False" 
											ImagesBaseDir="~/img/"										
											BeforeClientClick="seleccionaNodes" 
											BeforeClientDrop="abansDrop" 
											MultipleSelect="True" 
											DragAndDrop="True" 
											OnNodeDrop="tractaDragDrop"	 
											ContextMenuContentFile="~/js/App_LocalResources/contextmenusNou.xml"
											BeforeClientContextClick="ContextClicked" 
											OnNodeContextClick="ContextClicked"
											Skin="gaia"> </RADT:RADTREEVIEW>

</td></tr>  <tr>
                        <td height="4" colspan="2" bgcolor="#5F61C0"></td>
                      </tr></table></td>
<% IF NOT (Request("arbre2") Is Nothing) Or llistaArbres2_1.SelectedItem.Value.Substring(0, 1) <> "#" THEN %>
             <td valign="top" width=50% >
					<table width="100%" cellpadding="0" cellspacing="0"  >
                      <tr valign="bottom" bgcolor="#666666"  style="padding: 10">
                        <td width="" height="35"  class="txtNeg14px"><div align="left">                          
                            <asp:Button ID="Button1" runat="server" OnClick="clickCopiarNode" Text=">> Copiar"></asp:Button>
                                      </div>
												</td>
                        <td width="1" height="35" ><div align="right"><asp:DropDownList runat='server' ID="llistaArbres2_2" DataTextField="TIPDSDES" DataValueField="TIPINTIP" OnSelectedIndexChanged="llistaArbres2_2_canviArbre" autopostback="true"></asp:DropDownList></div></td>
                      </tr>
                      <tr bgcolor="#999999" style="padding: 10">
                        <td height="4" colspan="2"></td>
                      </tr>
                      <tr >
                        <td colspan="2" class="TreeContext"><RADT:RADTREEVIEW  
											ID="RadTree2" 
											runat="server" 
											RetainScrollPosition="True" 
											AutoPostBack="false" 
											CheckBoxes="False" 
											ImagesBaseDir="~/img/"
											BeforeClientClick="seleccionaNodes" 
											BeforeClientDrop="abansDrop" 
											MultipleSelect="True" 
											DragAndDrop="True" 
											OnNodeDrop="tractaDragDrop"	 
											ContextMenuContentFile="~/js/App_LocalResources/contextmenusNou.xml" 
											BeforeClientContextClick="ContextClicked" 
											OnNodeContextClick="ContextClicked"
											Skin="gaia" ></RADT:RADTREEVIEW>
												</td>
                      </tr>
                      <tr>
                        <td height="4" colspan="2" bgcolor="#AAAAAA"></td>
                      </tr>
                      <tr height="100%">
                        <td colspan="2" bgcolor="#FFFFFF" class="txtRojo12px">
                          <asp:Button ID="Button2" runat="server" Text=">> Copiar" OnClick="clickCopiarNode" ></asp:Button>
                        </td>
                      </tr>
                    </table> </td>
<%
END IF
%>
                 
                </tr>
              </table>
              
    
              <div id="capaInsertarNode" style="position:absolute; overflow: hidden; width:500; height:300; z-index:1; left: 100px; top: 300px; background-color: #CCCCCC;  border: 2px outset #990000; visibility: hidden; margin: 20px;">
             <div style="background-color:#990000" class="txtblanco18px" align="center">Insercio de nodes</div>
 <img src="img/fletxaroj.gif" width="5" height="8"> <span class="txtNeg14px">Escolliu el tipus de node que voleu inserir i poseu el text associat. </span><br>
 <br>
   <table width=100%  border="0" cellpadding="0" cellspacing="0">
                    <tr bgcolor="#DDDDDD" class="txtNeg14px">
                      <td width="10%">Tipus</td>
                      <td width="90%">Text</td>
     </tr>
                    <tr bgcolor="#000000" class="txtNeg14px">
                      <td colspan="2" height="1"></td>
                    </tr>
                    <tr>
                      <td><asp:DropDownList runat='server' ID="tipusNode" DataTextField="TIPDSDES" DataValueField="TIPINTIP"></asp:DropDownList></td>
                      <td><asp:TextBox runat="server"  ID="noddstxt" Columns="30" onKeyDown="
			if (window.event.keyCode==13) {		
				event.returnValue=false;
                event.cancel = true;
				document.getElementById('InsertarNode').click();

			}"></asp:TextBox></td>
                    </tr>
</table>
                  <br>   <div align="center">
                  <asp:button runat="server" OnClick="clickInsertarNode" Text="Acceptar" id="InsertarNode"></asp:button>
                  <input type="submit" OnClick="document.getElementById('capaInsertarNode').style.visibility = 'hidden';return false;" value="Tancar" >
                </div>
             
              </div>

<div id="capaCercarNode" style="position:absolute; overflow: hidden; width:500; height:300; z-index:1; left: 100px; top: 300px; background-color: #CCCCCC;  border: 2px outset #990000; visibility: hidden; margin: 20px;" >
             <div style="background-color:#990000" class="txtblanco18px" align="center">Cerca de nodes</div>
 <img src="/img/fletxaroj.gif" width="5" height="8"> <span class="txtNeg14px">Escolliu el filtre per fer la cerca:</span><br>
 <br>
              <table  border="0">
                  <tr bgcolor="#DDDDDD">
                    <td width="27">Text</td>
                    <td width="386"><asp:TextBox runat="server"  ID="cerca" Columns="70" MaxLength="100"  onKeyDown="
			if (window.event.keyCode==13) {		
				event.returnValue=false;
        event.cancel = true;
				document.getElementById('CercarNode').click();

			}"></asp:TextBox></td>
                  </tr>
                </table>
                <div align="center">
                  <asp:button runat="server" OnClick="clickCercarNode" Text="Acceptar" id="CercarNode"></asp:button>
                  <input type="submit" onclick="document.getElementById('capaCercarNode').style.visibility = 'hidden';return false;" value="Tancar">
             
								
                </div>
						  </div>
<asp:textbox id="arbreTreeView" runat="server" BorderWidth="0" Width="0"></asp:textbox><asp:textbox id="actualitzaNode" BorderWidth="0" runat="server" width="0"></asp:textbox><asp:textbox id="actualitzaNodeArbre2" runat="server" width="0" BorderWidth="0"></asp:textbox><asp:label id="lblDebug" runat="server"></asp:label><asp:label id="lblCodi" runat="server"></asp:label>
</form>

 <!-- #INCLUDE VIRTUAL="~/js/App_LocalResources/peu.aspx" -->

</body>
</html>
