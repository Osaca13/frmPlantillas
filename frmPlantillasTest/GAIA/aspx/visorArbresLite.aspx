<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="visorArbresLite.aspx.vb" Inherits="frmPlantillasTest.visorArbresLite" ValidateRequest="false" Debug="true" %>

<!DOCTYPE html>
<html>

<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta http-equiv="Page-Enter" content="revealTrans(Duration=0,Transition=5)"/>
    <title>Documento sin titulo</title>
<%@ Register tagprefix="radT" namespace="Telerik.WebControls" assembly="RadTreeView" %>
    <style type="text/css">
<!--
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
-->
</style>
</head>
<body>
   

<script  lang ="javascript">

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
		
	}    
}

  function BeforeClickHandler(node)
  {
   return false;
  }

function seleccionaNodes(Node)
{			
	var arbre,separador,inici;		
	inici=0;				
	separador = Node.Value.indexOf("-",inici);						
	document.getElementById('nodes').value =  Node.Value.substr(inici,separador-inici);	
	//alert(event.keyCode);
}
   var oldNode;
function marcaNode(Node)
{
	oldNodeChecked = Node.Checked
	if (1==<%=iif(Request("nU") is nothing,"0",Request("nU"))%>) {
		for (var i=0; i<RadTree1.AllNodes.length; i++)
		 {
		  RadTree1.AllNodes[i].UnCheck(); 
		 }
	}
	if (oldNodeChecked==true) {	
		Node.Check();		
	}
}


function abansDrop(Node)
{
//alert(event.keyCode);
}
</script>

    <form name="form1" runat="server">
        <asp:Label ID="lblDebug" runat="server"></asp:Label>
        <asp:Label ID="lblCodi" runat="server"></asp:Label>
        <asp:TextBox ID="arbreTreeView" runat="server" Width="0" ></asp:TextBox>
        <asp:TextBox ID="actualitzaNode" runat="server" Width="0" ></asp:TextBox>
        <asp:TextBox ID="actualitzaNodeArbre2" runat="server" Width="0" ></asp:TextBox>
        <asp:DropDownList runat="server" ID="idioma">
        <asp:ListItem Value="1">Català</asp:ListItem>
        <asp:ListItem Value="2">Castellà</asp:ListItem>
        <asp:ListItem Value="3">Anglès</asp:ListItem>
        </asp:DropDownList>
        <asp:dropdownlist runat="server" id="llistaArbres" visible="true" runat="server" OnSelectedIndexChanged="llistaArbres_canviArbre" AutoPostBack="true"></asp:dropdownlist>	
        <input name="nodes" type="hidden" value="<%=Request("nodes")%>">
        <input name="arbre1" type="hidden" value="<%=Request("arbre1")%>">
        <table height="55" width="387">
            <tbody>
                <tr>
                    <td width="379" valign="top">
                        <div align="center">
                            <input type="submit" value="Cercar" onClick="document.getElementById('capaCercarNode').style.visibility = 'visible';return false;"
                                class="INPUTTEXTROJO">
                            <asp:Button ID="RetornarNodes" runat="server" Text="Retornar nodes" OnClick="clickRetornarNodes">
                            </asp:Button>
                            <asp:Label ID="lblIdioma" runat="server"></asp:Label>
                        </div>
                        <hr>
                        <radT:RADTREEVIEW id="RadTree1" runat="server" RetainScrollPosition="True" AutoPostBack="false"
                          ContextMenuContentFile="/GAIA/aspx/ContextMenusNou.xml"   CheckBoxes="True" ImagesBaseDir="/img/common/iconografia/" CssFile=" /GAIA/aspx/Examples/Advanced/LoadOnDemand/tree.css" BeforeClientContextClick="ContextClicked" OnClick="UpdateStatus" OnNodeContextClick="ContextClicked"
											Skin="gaia" 
                            MultipleSelect="True" AfterClientCheck="marcaNode" BeforeClientClick="BeforeClickHandler">
                        </radT:RADTREEVIEW>
                        <hr>
                        <div align="center">
                            <input type="submit" value="Cercar" onClick="document.getElementById('capaCercarNode').style.visibility = 'visible';return false;"
                                class="INPUTTEXTROJO">
                            <asp:Button ID="RetornarNodes2" runat="server" Text="Retornar nodes" OnClick="clickRetornarNodes">
                            </asp:Button>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="capaCercarNode" style="position: absolute; width: 350px; height: 115px;
            z-index: 1; left: 9px; top: 26px; background-color: #FFCC99; layer-background-color: #FFCC99;
            border: 1px none #000000; visibility: hidden;">
            <div align="center">
                <br>
                <table border="0">
                    <tr bgcolor="#DDDDDD">
                        <td>
                            Text</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="cerca" Columns="35" MaxLength="100"></asp:TextBox></td>
                    </tr>
                </table>
                <br>
            </div>
            <div align="center">
                <asp:Button runat="server" OnClick="clickCercarNode" Text="Acceptar"></asp:Button>
                <input type="submit" onClick="document.getElementById('capaCercarNode').style.visibility = 'hidden';return false;"
                    value="Tancar">
            </div>
        </div>
    </form>
    <% 
        ' FAIG això només per poder declarar radtree2, no ho utilitzo però em fa falta per compartir el vb amb visorarbres.aspx

        If False Then %>
<radT:RADTREEVIEW withevents="" id="RadTree2" runat="server" RetainScrollPosition="True"
    AutoPostBack="True" CheckBoxes="False" LicenseFile=" /GAIA/aspx/LicenseFile.xml"
    ImagesBaseDir="/img/common/iconografia/" CssFile="~/Examples/Advanced/LoadOnDemand/tree.css" />
<asp:dropdownlist runat='server' id="tipusNode" datatextfield="TIPDSDES" datavaluefield="TIPINTIP"></asp:dropdownlist>
<asp:textbox runat="server" id="noddstxt" columns="70" maxlength="100"></asp:textbox>
<% END IF%>

<asp:dropdownlist runat="server" id="llistaArbres2_1" visible="false"></asp:dropdownlist>
<asp:dropdownlist runat="server" id="llistaArbres2_2" visible="false"></asp:dropdownlist>


</body>



</html>
