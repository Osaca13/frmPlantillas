<identity impersonate="true"/>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="visorLlibreriaCodiWeb_GAIA2.aspx.vb" Inherits="frmPlantillasTest.visorLlibreriaCodiWeb_GAIA2" ValidateRequest="false" Debug="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
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
    <title>Llibreria de codi web</title>
</head>
<body>
<form runat="server">

  <asp:Label ID="lbldebug" runat="server" CssClass="sr-only"/>
  <asp:Label ID="lblResultat" runat="server" CssClass="alert alert-success"/>
  <asp:textbox id="txtCodiNode" runat="server" CssClass="sr-only"></asp:textbox>
  
  <div class="card h-100">      
    <div class="card-body" style="overflow-y: auto; position:relative;">
    	<div class="p-2" style="position:absolute; top:0; bottom:0; left:0; right:0;">    	
                	               
                
        	<div class="form-row">
            	<div class="form-group col-12 mb-2">
                    <div class="input-group input-group-sm">
                    	<div class="input-group-prepend">
                        <label for="LCWDSTIT" class="input-group-text">T&iacute;tol:</label>
                        </div>
                        <asp:TextBox runat="server" ID="LCWDSTIT" MaxLength="100" CssClass="form-control"></asp:TextBox>    
                    </div>    
                </div>  
                <div class="form-group col-3 mb-2">
                        <label for="lblIdioma" class="col-form-label col-form-label-sm">Idioma:</label>
                        <asp:label runat="server" id="lblIdioma"></asp:label>   
                </div>                
                <div class="form-group col-3 mb-2">
                       <label for="LCWTPFOR" class="col-form-label col-form-label-sm">For&ccedil;ar execuci&oacute;:</label>
                        <asp:dropdownlist runat="server" id="LCWTPFOR" CssClass="custom-select custom-select-sm">
                        <asp:ListItem value="S">Si</asp:ListItem>
                        <asp:ListItem value="N">No</asp:ListItem>
                    	</asp:dropdownlist>             
                </div>                
                <div class="form-group col-6 mb-2">
                       <label for="LCWTPFOL" class="col-form-label col-form-label-sm">Executar si alguna llibreria &eacute;s buida</label>
                        <asp:dropdownlist runat="server" id="LCWTPFOL" CssClass="custom-select custom-select-sm">
                        <asp:ListItem value="S" >Si</asp:ListItem>
                        <asp:ListItem value="N" >No</asp:ListItem>
                    	</asp:dropdownlist>           
                </div>                 
                <div class="form-group col-12 mb-2">
                	<div class="input-group input-group-sm">
                        <div class="input-group-prepend">
                       <label for="LCWCDTIP" class="input-group-text">Tipus</label>
                        </div>
                        <asp:dropdownlist runat="server" id="LCWCDTIP" CssClass="custom-select custom-select-sm">
                        <asp:ListItem value="1">codi HTML</asp:ListItem>
                        <asp:ListItem value="2">Pre-proc&eacute;s abans de la publicaci&oacute;</asp:ListItem>
                        <asp:ListItem value="3">TEXT per afegir a la p&agrave;gina i executar després de la publicaci&oacute;</asp:ListItem>
                        <asp:ListItem value="4">FITXER per incloure i executar despr&eacute;s de la publicaci&oacute;</asp:ListItem>
                    	</asp:dropdownlist>
                    </div>
                </div>
                <div class="form-group col-12 mb-2">
                    <label for="LCWDSTXT" class="col-form-label col-form-label-sm">Codi</label>
                    <asp:TextBox runat="server" ID="LCWDSTXT" Rows="3" AutoPostBack="true" TextMode="MultiLine" CssClass="form-control form-control-sm"></asp:TextBox>  
                    <asp:label runat="server" id="lblEditarFitxer"></asp:label>                           
                </div>
                <div class="form-group col-12 mb-2">
                    <label for="LCWDSHLP" class="col-form-label col-form-label-sm">Qu&egrave; fa la llibreria?</label>
                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="LCWDSHLP" Rows="3" AutoPostBack="true" TextMode="MultiLine"></asp:TextBox>       
                </div>
                <div class="col-6 mb-2">
                	<div class="card bg-light">
                        <div class="card-header"><h6 class="font-weight-bold"><label for="RelacionsNodes" class="mb-0">Continguts que provoquen la publicaci&oacute;</label></h6></div>
                        <div class="card-body p-2">                            
                                <div class="text-center mb-2">
                                 <input type="button" class="btn btn-sm btn-primary" onClick="window.open('/GAIA2/aspx/visorarbreslite_GAIA2.aspx?perCodi=1&arbre1=198341&contingut=' + document.getElementById('RelacionsNodes').value + '&c=Relacions&nodesSeleccionats=' + document.getElementById('RelacionsNodes').value + '&separador=,', '_blank', 'location=0,height=400,width=460,scrollbars=yes,resizable=yes'); return false;" value="Seleccionar" id="btnRelacions">
  								 <input type="button" class="btn btn-sm btn-danger" id="eliminarRelacions" value="Esborrar" onClick="document.getElementById('RelacionsNodes').value='';document.getElementById('RelacionsTxt').value=''; return false;">  
                                </div> 
                                <asp:TextBox ID="RelacionsTxt" runat="server" AutoPostBack="False" Rows="4" ContentEditable="false" TextMode="MultiLine" CssClass="form-control form-control-sm mb-2"></asp:TextBox>
                                <asp:TextBox ID="RelacionsNodes" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>                            
                         </div>
                    </div>
                </div>
                <div class="col-6 mb-2">
                	<div class="card bg-light">
                        <div class="card-header"><h6 class="font-weight-bold"><label for="RelacionsPlantillesNodes" class="mb-0">Plantilles utilitzades dins la llibreria</label></h6></div>
                        <div class="card-body p-2">                            
                            <div class="text-center mb-2">
                             <input type="button" class="btn btn-sm btn-primary" onClick="window.open('/GAIA2/aspx/visorarbreslite_GAIA2.aspx?perCodi=1&arbre1=198337&c=RelacionsPlantilles&nodesSeleccionats='+document.getElementById('RelacionsPlantillesNodes').value+'&separador=,','_blank', 'location=0,height=400,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" id="btnPlantillesRelacions">
                  			<input type="button" id="eliminarPlantillesRelacions" value="Esborrar" class="btn btn-sm btn-danger" onClick="document.getElementById('RelacionsPlantillesNodes').value='';document.getElementById('RelacionsPlantillesTxt').value=''; return false;">  
                            </div>                             
                            <asp:TextBox ID="RelacionsPlantillesTxt" runat="server" AutoPostBack="False" Rows="4" ContentEditable="false" TextMode="MultiLine" CssClass="form-control form-control-sm mb-2"></asp:TextBox>  
                            <asp:TextBox ID="RelacionsPlantillesNodes" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>                        
                         </div>
                    </div>    
                </div> 
                <asp:placeholder id="pnlCel" runat="server" visible="false"></asp:placeholder>
                <div class="form-group col-12 mb-2">
                	<div class="input-group input-group-sm">
                        <div class="input-group-prepend">
                       <label for="ltCEL" class="input-group-text">Enlla&ccedil; darrera execuci&oacute;</label>
                        </div>
                       <asp:literal id="ltCEL" runat="server"/> 
                    </div> 
                </div>
                 
            </div>
   		</div>
   </div>
   <div class="card-footer text-center"><input type=button  id="btnInsert"  value="Afegir codi" OnServerClick="clickAfegirCodi" runat="server" class="btn btn-sm btn-success">
       <asp:CheckBox ID="checkedAfegirCodi" runat="server" Checked="false" style="display:none" />

   </div>
</div>
</form>
</body>
</html>
<script src="https://code.jquery.com/jquery-3.3.1.min.js" integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js" integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js" integrity="sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy" crossorigin="anonymous"></script>

