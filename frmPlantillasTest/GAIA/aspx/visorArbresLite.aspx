<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="visorArbresLite.aspx.vb" Inherits="frmPlantillasTest.visorArbresLite" ValidateRequest="false" Debug="true" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Page-Enter" content="revealTrans(Duration=0,Transition=5)">

  

    <%@ register tagprefix="radT" namespace="Telerik.WebControls" assembly="RadTreeView" %>
    <title>Visor llibreries</title>
    <meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
	<meta http-equiv="x-ua-compatible" content="ie=edge">
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous"/>
    <!--[if IE]>
          <link href="https://cdn.jsdelivr.net/gh/coliff/bootstrap-ie8/css/bootstrap-ie9.min.css" rel="stylesheet">
          <script src="https://cdn.jsdelivr.net/g/html5shiv@3.7.3"></script>
        <![endif]-->
        <!--[if lt IE 9]>
          <link href="https://cdn.jsdelivr.net/gh/coliff/bootstrap-ie8/css/bootstrap-ie8.min.css" rel="stylesheet">
    <![endif]-->
    <style>
	input[type=checkbox], input[type=radio] { margin-right:.3rem}
	</style>
</head>
<body>   
<script type="text/javascript">
    function ContextClicked(node, itemText) {
        if (itemText == "Cercar") {
            document.getElementById("arbreTreeView").value = node.TreeView.ID;
            document.getElementById("capaCercarNode").style.visibility = "visible";
            document.getElementById("cerca").focus();

        }
        if (itemText == "Insertar") {

        }
    }

    function BeforeClickHandler(node) {
        return false;
    }

    function seleccionaNodes(Node) {
        var arbre, separador, inici;
        inici = 0;
        separador = Node.Value.indexOf("-", inici);
        document.getElementById('nodes').value = Node.Value.substr(inici, separador - inici);
        //alert(event.keyCode);
    }
    var oldNode;
    function marcaNode(Node) {
        oldNodeChecked = Node.Checked
        if (1 ==<%=IIf(Request("nU") Is Nothing, "0", Request("nU"))%>) {
            for (var i = 0; i < RadTree1.AllNodes.length; i++) {
                RadTree1.AllNodes[i].UnCheck();
            }
        }
        if (oldNodeChecked == true) {

            Node.Check();
        }
    }


    function abansDrop(Node) {
        //alert(event.keyCode);
    }
</script>

    <form name="form1" runat="server">
        <asp:Label ID="lblDebug" runat="server" CssClass="sr-only"></asp:Label>
        <asp:Label ID="lblCodi" runat="server" CssClass="sr-only"></asp:Label>
        <asp:TextBox ID="arbreTreeView" runat="server" CssClass="sr-only"></asp:TextBox>
        <asp:TextBox ID="actualitzaNode" runat="server" CssClass="sr-only"></asp:TextBox>
        <asp:TextBox ID="actualitzaNodeArbre2" runat="server" CssClass="sr-only"></asp:TextBox>
        <input name="nodes" type="hidden" value="<%=Request("nodes")%>" class="sr-only">
        <input name="arbre1" type="hidden" value="<%=Request("arbre1")%>" class="sr-only">
        
        
        
        
  		<div class="card h-100">  
        	<div class="card-header form-inline p-2">
                <div class="input-group input-group-sm mr-2 sr-only">
                    <div class="input-group-prepend">
                    <label for="idioma" class="input-group-text">Idioma</label>
                    </div>
                    <asp:DropDownList runat="server" ID="idioma" CssClass="custom-select custom-select-sm">
                        <asp:ListItem Value="1">Catal&agrave;</asp:ListItem>
                        <asp:ListItem Value="2">Castell&agrave;</asp:ListItem>
                        <asp:ListItem Value="3">Angl&egrave;s</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="input-group input-group-sm">
                    <div class="input-group-prepend">
                    <label for="idioma" class="input-group-text">Arbre</label>
                    </div>
                    <asp:dropdownlist runat="server" id="llistaArbres" visible="true" OnSelectedIndexChanged="llistaArbres_SelectedIndexChanged" AutoPostBack="true" CssClass="custom-select custom-select-sm"></asp:dropdownlist>	
                </div>
        	</div> 
                        
            <div class="card-body" style="overflow-y: auto; position:relative; height:600px">
            	<div style="position:absolute; top:0; bottom:0; left:0; right:0;">                 
            <radT:RADTREEVIEW id="RadTree1" runat="server" RetainScrollPosition="True" AutoPostBack="false" ContextMenuContentFile="/GAIA/aspx/ContextMenusNou.xml" CheckBoxes="True" ImagesBaseDir="/img/common/iconografia/" CssFile=" /GAIA/aspx/Examples/Advanced/LoadOnDemand/tree.css" BeforeClientContextClick="ContextClicked" OnClick="UpdateStatus" OnNodeContextClick="ContextClicked" Skin="gaia" MultipleSelect="True" AfterClientCheck="marcaNode" BeforeClientClick="BeforeClickHandler"></radT:RADTREEVIEW></div>
            </div>
                            
            <div class="card-footer text-center">              
                <input type="submit" data-toggle="modal" data-target="#capaCercarNode" value="Cercar" onClick="document.getElementById('capaCercarNode').style.visibility = 'visible'; return false;" class="btn btn-primary btn-sm">
                <asp:Button ID="RetornarNodes2" runat="server" Text="Retornar nodes" OnClick="clickRetornarNodes" CssClass="btn btn-success btn-sm"></asp:Button>
            </div>
              <asp:Label ID="labelNodes" runat="server" style="display:none" ></asp:Label>
              <asp:Label ID="labelNroNodes" runat="server" style="display:none"></asp:Label>
              <asp:CheckBox ID="retornarNodesChecked" runat="server" style="display:none" Checked="false"></asp:CheckBox>
                  
        </div>        
        
        
        <!-- Modal -->
        <div class="modal fade" id="capaCercarNode" tabindex="-1" role="dialog" aria-hidden="true">
          <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="proteccioDadesTitle">Cercar text</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body"> 
              	<div class="input-group input-group-sm">
                    <div class="input-group-prepend">
                    <label for="cerca" class="input-group-text">Text</label>
                    </div>
                    <asp:TextBox runat="server" ID="cerca" CssClass="form-control"></asp:TextBox>
                    <div class="input-group-append">
                    	<asp:Button runat="server" OnClick="clickCercarNode" Text="Cercar" CssClass="btn btn-primary"></asp:Button>
                    </div>
                </div>                
              </div>
            </div>
          </div>
        </div>
        <!-- Fi Modal -->
        
        
        
        
        
        
        
        
    </form>
</body>
</html>
<script src="https://code.jquery.com/jquery-3.3.1.min.js" integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js" integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js" integrity="sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy" crossorigin="anonymous"></script>
<% 
    ' FAIG això només per poder declarar radtree2, no ho utilitzo però em fa falta per compartir el vb amb visorarbres.aspx

    If False Then %>
<radT:RADTREEVIEW withevents id="RadTree2" runat="server" RetainScrollPosition="True"
    AutoPostBack="True" CheckBoxes="False" LicenseFile=" /GAIA/aspx/LicenseFile.xml"
    ImagesBaseDir="/img/common/iconografia/" CssFile="~/Examples/Advanced/LoadOnDemand/tree.css" />
<asp:dropdownlist runat='server' id="tipusNode" datatextfield="TIPDSDES" datavaluefield="TIPINTIP"></asp:dropdownlist>
<asp:textbox runat="server" id="noddstxt" columns="70" maxlength="100"></asp:textbox>
<% END IF%>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="Telerik.WebControls" %>
<%@ Import Namespace="System.Data.OleDb" %>

<asp:dropdownlist runat="server" id="llistaArbres2_1" visible="false"></asp:dropdownlist>
<asp:dropdownlist runat="server" id="llistaArbres2_2" visible="false"></asp:dropdownlist>







