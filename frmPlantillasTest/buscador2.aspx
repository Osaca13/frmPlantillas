<identity impersonate="true"/>
<%@ Page Language="VB" Debug="true"  enableviewstateMAC="false"%>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data.Oledb" %>
<%@ Import Namespace="System.Xml" %>
<%@ Register TagPrefix="radT" Namespace="Telerik.WebControls" Assembly="RadTreeView" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>Cerca avançada de documents</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<link rel="stylesheet" href="/css/intranet.css" type="text/css">
</head>
<body bgcolor="#FFFFFF" text="#000000" leftmargin="10" topmargin="10">
<!--#INCLUDE VIRTUAL="/inc/cap.aspx" -->
<script language="javascript">
function UpdateAllChildren(nodes, checked)
{
//   var i;
 //  for (i=0; i<nodes.length; i++)
 //  {
//      checked ? nodes[i].Check() : nodes[i].UnCheck(); 
//      if (nodes[i].Nodes.length > 0)
//         UpdateAllChildren(nodes[i].Nodes, checked);   
//   }
}

function CheckChildNodes(node)
{
  // UpdateAllChildren(node.Nodes, node.Checked);
}
</script>


<form id="frmLlistat" runat="server" > 
      <%@ Register TagPrefix="menuG" TagName="menuG" Src="/gaia/aspx/llibreriacodiweb/intranet/gaia/menu.ascx" %>
    <menuG:menug ID="menuG" Text="Menú GAIA" runat="server"/> 

<input type="hidden" id="id_buscador" runat="server" value="Doc"/>
  <div> 
    <p class="txtRojo_Titulo_PagInterior18px">Cerca avançada de documents</p> 
	<asp:ValidationSummary runat="server" id="vs1" DisplayMode="List"  ShowSummary="true" ShowMessageBox="false"></asp:ValidationSummary>
	<asp:panel id="pnlCriterisCerca" visible="true" runat="server">
	
	<table width="759" border="1" cellspacing="1" cellpadding="1"   class="tablabordepeqroj" bordercolor="#FFCDCA">
    	<tr>
    		<td colspan="4" bgcolor="#E6E6E6" class="txtrojo12px">Criteris de Cerca
			</td>
    	</tr>
        <tr>
	        <td colspan="4">
    	        <table width="100%" border="0" cellspacing="4" cellpadding="0">
					<tr>
						<td>
							<table>
								<tr>
									<td class="txtneg12px">
										<u>Contingut del document</u>
									</td>
								</tr>
								<tr>
									<td width="28%" class="txtneg12px">
										&nbsp;&nbsp;&nbsp;- Amb totes les paraules:
									</td>
									<td width="35%">
										<asp:TextBox runat="server" class="text" ID="paraules_totes" Columns="25" MaxLength="50"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td class="txtneg12px">
										&nbsp;&nbsp;&nbsp;- Amb alguna de les paraules:
									</td>
									<td>
										<asp:TextBox runat="server" class="text" ID="paraules_alguna" Columns="25" MaxLength="50"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td class="txtneg12px">
										&nbsp;&nbsp;&nbsp;- Sense les paraules:
									</td>
									<td>
										<asp:TextBox runat="server" class="text" ID="paraules_sense" Columns="25" MaxLength="50"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td class="txtneg12px">
										Títol:
									</td>
									<td>
										<asp:TextBox runat="server" class="text" ID="titol" Columns="20" MaxLength="50"></asp:TextBox>
									</td>
								</tr>
							</table>
						</td>
						<td>
							<br>
							<table>
								<tr>
									<td class="txtneg12px">
										Estat:
									</td>
									<td>
										<asp:DropDownList CssClass="control" ID="ddlb_estat" runat="server">
											<asp:ListItem Value="0">Tots</asp:ListItem>
											<asp:ListItem Value="2" >Actiu</asp:ListItem>									
											<asp:ListItem Value="98">Caducat</asp:ListItem>
											<asp:ListItem Value="99" >Esborrat</asp:ListItem>		
										</asp:DropDownList>
									</td>
								</tr>								
								<tr>
									<td class="txtneg12px">
										Idioma:
									</td>
									<td>
										<asp:DropDownList CssClass="control" ID="ddlb_idioma" runat="server"></asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td class="txtneg12px">
										Data de creació inicial: 
									 </td>
									 <td >
										<asp:TextBox runat="server" class="text" ID="dataIni" Columns="10" MaxLength="10"></asp:TextBox>
										<asp:RangeValidator id="RVDataIni" Display="Static" Type="Date" ControlToValidate="dataIni" ErrorMessage="Format incorrecte en la data inicial(DD/MM/AAAA)" MaximumValue="1/1/2100" MinimumValue="1/1/1900" runat="server">*</asp:RangeValidator>
										<a style="vertical-align:middle;" href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=dataIni','calendar_window','width=150,height=188');calendar_window.focus()"><img src="/img/common/iconografia/ico_calendari.png" border="0" /></a> </span>
									</td>
								</tr>
								<tr>
									<td class="txtneg12px">
										Data de creació final: 
									 </td>
									 <td >
										<asp:TextBox runat="server" class="text" ID="dataFi" Columns="10" MaxLength="10"></asp:TextBox>
										<asp:RangeValidator id="RVDataFi" Display="Static" Type="Date" ControlToValidate="dataFi" ErrorMessage="Format incorrecte en la data final(DD/MM/AAAA)" MaximumValue="1/1/2100" MinimumValue="1/1/1900" runat="server">*</asp:RangeValidator>
										<a style="vertical-align:middle;" href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=dataFi','calendar_window','width=150,height=188');calendar_window.focus()"><img src="/img/common/iconografia/ico_calendari.png" border="0" /></a> </span>
									</td>
								 </tr>
							</table>						
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="4" bgcolor="#E6E6E6">
				<table width="100%" cellpadding="0" cellspacing="0">
					<tr>
						<td width="60%"  class="txtrojo12px">
							Codificació del contingut
						</td>
						<td width="40%" class="txtrojo12px">
							
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="4">
				<table width="100%">
					<tr>
						<td width="30%" height="1" valign="top">
						<RADT:RADTREEVIEW contentFile="../codificacio/arbre12367.xml" withevents id="RadTree1" runat="server" RetainScrollPosition="True" AutoPostBack="false" CheckBoxes="True"  ImagesBaseDir="/img/common/iconografia/" CssFile="/treeview/Examples/Advanced/LoadOnDemand/tree.css"  MultipleSelect="True" AfterClientCheck="CheckChildNodes"  BeforeClientClick="return false;" > </RADT:RADTREEVIEW>
						
						
						</td>
						<td style="border-right-style:solid; border-right-width:thin; border-right-color:#FFCDCA" width="30%"   valign="top" ><RADT:RADTREEVIEW withevents id="RadTree2" runat="server" RetainScrollPosition="True" AutoPostBack="false" CheckBoxes="True"  ImagesBaseDir="/img/common/iconografia/" CssFile="/treeview/Examples/Advanced/LoadOnDemand/tree.css"  MultipleSelect="True" AfterClientCheck="CheckChildNodes"  BeforeClientClick="return false;" > </RADT:RADTREEVIEW></td>
						<td width="40%" valign="top" ><RADT:RADTREEVIEW id="RadTree3" runat="server" RetainScrollPosition="True" AutoPostBack="false" CheckBoxes="True"  ImagesBaseDir="/img/common/iconografia/" CssFile=" /GAIA/aspx/Examples/Advanced/LoadOnDemand/tree.css"  MultipleSelect="True" AfterClientCheck="CheckChildNodes"  BeforeClientClick="return false;" > </RADT:RADTREEVIEW></td>						
					</tr>
				</table>
			</td>
		</tr>		
	</table>
  	
	   <asp:Button OnClick="mostrarClick" id="Mostrar" runat="server" Text="Cercar" CssClass="boto"></asp:Button>
		<asp:Button OnClick="amagarCriterisCerca" id="btnAmagarCriterisCerca" runat="server" Text="Amagar criteris de cerca" CssClass="boto"></asp:Button>
 </asp:panel>
 <asp:panel id="pnlBtnMostrarCriterisCerca" runat="server" Visible="false">
	<div>
	 <div class="paddingTop20 floatleft" ><asp:Button OnClick="mostrarCriterisCerca" id="btnMostrarCriterisCerca" runat="server" Text="Mostrar criteris de cerca" CssClass="boto"></asp:Button></div>


 	</div>
 </asp:panel>
	
	<input type=hidden runat=server id=ultimOrderCrit> 
	<input type=hidden runat=server id=ultimOrderDir> 
    <br>
    <br>
  </div> 

  <ASP:DataGrid id="dgResultats" 
	runat="server"
	Width="759"
	BackColor="#FFFFFF" 
	BorderColor="black"
	CellPadding=3
	CellSpacing=2
	Font-Name="Arial"
	Font-Size="10pt"
	GridLines="None"
	HeaderStyle-BackColor="#DDDDDD"
	HeaderStyle-CssClass="txtrojo12px"
	AutoGenerateColumns="false"
	AllowPaging="true"	
	PageSize="10"
	OnPageIndexChanged="dgPaginar"
	PagerStyle-Mode="NumericPages"
  	AllowSorting="True"  OnItemCreated="evNovaFila"
	OnSortCommand="evhOnSortCommand" OnItemCommand="evhUbicacio">
	<columns>
		<asp:BoundColumn DataField="DOCINNOD" Visible="false"/>
		<asp:BoundColumn DataField="Relacions" Visible="false"/>
		<asp:TemplateColumn HeaderText = "Tipus" ItemStyle-HorizontalAlign="center" SortExpression="CAST(TDODSIMG AS VARCHAR(8000))">
        	<ItemTemplate>
            	<asp:Image runat=server ImageUrl='<%# "~/img/" & DataBinder.Eval(Container.DataItem, "TDODSIMG") %>'
				/>
				
		    </ItemTemplate>
		</asp:TemplateColumn>	
		<asp:HyperLinkColumn HeaderText="Nom del Document" sortexpression="NOM"
	  		DataNavigateUrlFormatString="/docs/{0}"
   			DataNavigateUrlField="DOCDSFIT"
			DataTextField="NOM"
			Target="_blank"
		/>							
		<asp:boundcolumn HeaderText="Mida" SortExpression="MIDA" DataField="MIDA" DataFormatString="{0:F2} KB"/>
		<asp:boundcolumn HeaderText="Data Creació" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CREACIO" DataField="CREACIO"/>
		<asp:HyperLinkColumn
	  		DataNavigateUrlFormatString="~/carregaDocuments.aspx?id={0}&idiArbre=1"
   			DataNavigateUrlField="DOCINNOD" 
			text = "Editar"
			Target="_blank"
		/>													
		<asp:ButtonColumn ButtonType="LinkButton" Text="Ubicació" CommandName="Ubicacio" />
	</columns>	
	<AlternatingItemStyle BackColor="#FFF3EC"></AlternatingItemStyle>
  </ASP:DataGrid>
  <asp:Label id="lbPagines" Visible="false" runat="server" CssClass="txtRojo12px">Pàgines de resultats:&nbsp;</asp:Label>
  <asp:LinkButton id="lnkAnterior" Visible="false" Font-Size="12px"
  	runat="server" width=60 OnClick="clickAnterior"  style="text-align:center">Anterior
  </asp:LinkButton>
  <asp:LinkButton id="lnkSeguent" Visible="false" Font-Size="12px"
  	runat="server" width=70 OnClick="clickSeguent" style="text-align:center"> Següent
  </asp:LinkButton>  
  <asp:Label ID="lbResultats" BackColor="#DDDDDD" width="759" class="txtneg12px" runat="server" />    
<br>
<table width="100%" runat="server" id="btnExcel">
	<tr>
		<td class="txtneg12px" align="left">
			<img src="/img/excel.gif"><asp:LinkButton runat="server" Text="Descarregar en format Excel" OnClick="descExcel"></asp:LinkButton>
	  	</td>
  	</tr>
</table>
</form> 
<br>
	
<!--#INCLUDE VIRTUAL="/inc/peu.aspx" -->
</body>
</html>
<script runat="server"  src="buscador_comu.aspx.vb"></script>
<script runat="server"  src="buscador.aspx.vb"></script>
