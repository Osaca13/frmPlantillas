<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="visorPlantilla_GAIA2.aspx.vb" Inherits="frmPlantillasTest.WebForm1" EnableEventValidation="false"  EnableViewState="false" ValidateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="ca">
<head runat="server">
<!-- Required meta tags -->
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
<meta http-equiv="x-ua-compatible" content="ie=edge">
<!-- Bootstrap CSS -->
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous"/>
<link rel="stylesheet" href="../../css/formularisGaia.css"/>
<link href="../web/img/open-iconic/font/css/open-iconic-bootstrap.css" rel="stylesheet"/>
<link href="../../css/bootstrap-multiselect.css" rel="stylesheet" />
<!--[if IE]>
      <link href="https://cdn.jsdelivr.net/gh/coliff/bootstrap-ie8/css/bootstrap-ie9.min.css" rel="stylesheet">
      <script src="https://cdn.jsdelivr.net/g/html5shiv@3.7.3"></script>
    <![endif]-->
    <!--[if lt IE 9]>
	  <link href="https://cdn.jsdelivr.net/gh/coliff/bootstrap-ie8/css/bootstrap-ie8.min.css" rel="stylesheet">
<![endif]-->
<title>Manteniment web - GAIA2</title>
</head>
<body>
<div class="card h-100">      
    <div class="card-body" style="overflow-y: auto; position:relative; height:90vh">
    	<div class="p-2" style="position:absolute; top:0; bottom:0; left:0; right:0;">
		<!-- CONTAINER -->
		<div class="container-fluid">        
     
        <form runat="server" id="frm">
            <asp:textbox  runat="server" id="txtCodiNode" style="display: none;"/>
            <input type="text" runat="server" id="txtEst" style="display: none;" />
            <input type="hidden" runat="server" id="txtEstBD" />  
            <input type="text" runat="server" id="txtAtributs"  style="display: none;" />
            <input type="text" runat="server" id="nroId" style="display: none;" />
           
            <asp:Label ID="lblResultat" runat="server"/> 
                
          
          <div id="divEst" style="visibility:hidden; position:absolute;"></div>
          <div style="visibility:hidden; position:absolute;">
              <asp:literal runat="server" id="ltTitol"/>            
              <asp:Panel runat="server" ID="pnlcanviIdioma" CssClass="form-inline col-6 justify-content-end">
              <div class="input-group input-group-sm">
                	<div class="input-group-prepend">
                  	<label runat="server" for="lstCanviIdioma" class="input-group-text">Canvi d'idioma:</label>
                  	</div>
                      <asp:DropDownList ID="lstCanviIdioma" runat="server" AutoPostBack="true" OnSelectedIndexChanged="canviIdioma" CssClass="custom-select custom-select-sm">
                          <asp:ListItem Value="1">Catal&agrave;</asp:ListItem>
                          <asp:ListItem Value="2">Castell&agrave;</asp:ListItem>
                          <asp:ListItem Value="3">Angl&egrave;s</asp:ListItem>
                          <asp:ListItem Value="4">Franc&egrave;s</asp:ListItem>
                      </asp:DropDownList>
                  </div>
              </asp:Panel>           
          </div>
          <!-- TABS --><ul class="nav nav-tabs" id="myTab" role="tablist">
                <li class="nav-item"><a class="nav-link active" id="propietats" data-toggle="tab" href="#arbreProps" role="tab" aria-controls="arbreProps" aria-selected="true">Propietats</a></li>
                <li class="nav-item"><a class="nav-link" id="disseny" data-toggle="tab" href="#dis" role="tab" aria-controls="dis" aria-selected="false">Disseny</a></li>                
              </ul><!-- FI TABS --> 
              
          <!-- TAB-CONTENT --><div class="tab-content" id="myTabContent">
            <!-- PROPIETATS --><div class="tab-pane fade show active p-3" id="arbreProps" role="tabpanel" aria-labelledby="propietats">             	           
              <asp:placeholder runat="server" id="pnlArbreWeb" visible="false">
              <div class="form-group">
                <div class="form-row mb-3"> 
                    <label for="AWEDSTIT" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">* T&iacute;tol:</label>
                    <asp:TextBox runat="server" ID="AWEDSTIT" MaxLength="60" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="T&iacute;tol"/>
                    
                </div>
                <div class="form-row mb-3">
                	<label for="AWEDSNOM" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Nom de l'arbre:</label>
                	<asp:TextBox runat="server" ID="AWEDSNOM" MaxLength="60" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Nom de l'arbre"/>
                </div>
                <div class="form-row mb-3">
                	<label for="lstAWEDSSER" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">* Servidor FTP dest&iacute;:</label>
                	<asp:DropDownList id="lstAWEDSSER" runat="server" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Servidor FTP dest&iacute;"/>
                    
                </div>
                <div class="form-row mb-3">
                	<label for="AWEDSROT" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Cam&iacute; del disc:</label>
                    <asp:TextBox runat="server"  ID="AWEDSROT" MaxLength="60" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Cam&iacute; del disc"/>
                </div>
                <div class="form-row mb-3">
                	<label for="AWEDSDOC" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Dest&iacute; pels documents:</label>
                	<asp:TextBox runat="server" ID="AWEDSDOC" MaxLength="60" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Dest&iacute; pels documents"/>
                </div>
                <div class="form-row mb-3">
                	<label for="AWEDSEBO" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Estils body:</label>
                	<asp:TextBox runat="server"  ID="AWEDSEBO" MaxLength="60" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Estils body" />
                </div>
                <div class="form-row mb-3">
                	<label for="AWEDSMET" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Valors META:</label>
                	<asp:TextBox runat="server" ID="AWEDSMET" Rows="10" TextMode="MultiLine"  class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7"  placeholder="Valors META" />
                </div>
                <div class="form-row mb-3">
                	<label for="AWEDSPEU" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">HTML al peu:</label>
                	<asp:TextBox runat="server"  ID="AWEDSPEU" Rows="10" TextMode="MultiLine"  class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7"  placeholder="HTML al peu"/>
                </div>
                <div class="form-row mb-3">
                	<label for="AWEDSCSP" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">CSS per pantalla:</label>
                	<asp:TextBox runat="server"  ID="AWEDSCSP"  class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7"  placeholder="CSS per pantalla"/>
                </div>
                <div class="form-row mb-3">
                	<label for="AWEDSCSI" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">CSS per impressora:</label>
                	<asp:TextBox runat="server"  ID="AWEDSCSI" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="CSS per impressora"/>
                </div>
              </div>
              </asp:placeholder>  
              
              <asp:placeholder runat="server" id="pnlPlantilla"  visible="false">
              <div class="form-group">
                  <div class="form-row mb-3"> 
                    <label for="txtPLTDSTIT" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">T&iacute;tol:</label>
                    <asp:textbox runat="server" id="txtPLTDSTIT" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="T&iacute;tol"/>
                  </div>
                  <div class="form-row mb-3">
                      <label for="txtPLTDSOBS" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Breu Descripci&oacute;:</label>
                      <asp:textbox runat="server" id="txtPLTDSOBS" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Breu Descripci&oacute;"/>
                  </div>
                  <div class="form-row mb-3">
                      <label for="chkPLTSWVIS" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm form-check-label text-lg-right text-md-right text-sm-left">Mostrar imatges amb visor d'imatges:</label>
                      <div class="form-check"><asp:checkbox runat="server" id="chkPLTSWVIS"  placeholder="Mostrar imatges amb visor d'imatges" CssClass="form-check-input" ToolTip="Mostrar imatges amb visor d'imatges"/></div>
                  </div>
              </div>
              </asp:placeholder>
              
              <asp:placeholder runat="server" id="pnlFullaWeb" visible="false">
              	<div class="form-group">
                    <div class="form-row mb-3"> 
                      <label for="WEBDSTIT" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">T&iacute;tol:</label>
                      <asp:textbox runat="server" id="WEBDSTIT" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="T&iacute;tol"/>
                    </div>
                    <div class="form-row mb-3">
                      <label for="WEBDSDES" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Descripci&oacute;:</label>
                      <asp:textbox runat="server" id="WEBDSDES" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Descripci&oacute;"/>
                     </div>
                     <div class="form-row mb-3">
                      <label for="WEBDSPCL" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Paraules clau:</label>
                      <asp:textbox runat="server" id="WEBDSPCL" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Paraules clau"/>                 
                     </div>
                     <div class="form-row mb-3">
                        <label for="WEBTPBUS" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm form-check-label text-lg-right text-md-right text-sm-left">Incloure-la al cercador:</label>
                        <div class="form-check"><asp:checkbox runat="server" id="WEBTPBUS" CssClass="form-check-input"/></div>
                      </div>
                      <div class="form-row mb-3">
                      <label for="WEBDTPUB" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Data publicaci&oacute;:</label>
                      <asp:textbox runat="server" id="WEBDTPUB" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="DD/MM/AAAA"/>
                      </div>                  
                      <div class="form-row mb-3">
                      <label for="WEBDTCAD" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Data caducitat:</label>
                      <asp:textbox runat="server" id="WEBDTCAD" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="DD/MM/AAAA"/>
                      </div>
                      <div class="form-row mb-3">
                      <label for="WEBDSFIT" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Nom del fitxer:</label>
                      <asp:TextBox runat="server"  ID="WEBDSFIT" MaxLength="100" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7"  placeholder="Nom del fitxer"></asp:TextBox>
                      </div>
                      <div class="form-row mb-3">
                      <label for="WEBDSURL" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Url:</label>
                      <asp:TextBox runat="server"  ID="WEBDSURL" MaxLength="100" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Url"></asp:TextBox>
                      </div>
                      <div class="form-row mb-3">
                        <label for="WEBTPHER" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm form-check-label text-lg-right text-md-right text-sm-left">Hereta propietats:</label>
                        <div class="form-check"><asp:CheckBox runat="server" ID="WEBTPHER" class="form-check-input"></asp:CheckBox></div>
                      </div>
                      <div class="form-row mb-3">
                        <label for="WEBSWFRM" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm form-check-label text-lg-right text-md-right text-sm-left">T&eacute; un formulari de servidor?:</label>
                        <div class="form-check"><asp:CheckBox runat="server" ID="WEBSWFRM" class="form-check-input"></asp:CheckBox></div>
                      </div>
                      <div class="form-row mb-3">
                        <label for="WEBSWEML" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm form-check-label text-lg-right text-md-right text-sm-left">&Eacute;s un correu electr&ograve;nic?:</label>
                        <div class="form-check"><asp:CheckBox runat="server" ID="WEBSWEML" class="form-check-input"></asp:CheckBox></div>
                      </div>
                      <div class="form-row mb-3">
                        <label for="WEBSWSSL" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm form-check-label text-lg-right text-md-right text-sm-left">&Eacute;s una p&agrave;gina segura?:</label>                    
                        <span class="form-check"><asp:CheckBox runat="server" ID="WEBSWSSL" class="form-check-input"></asp:CheckBox></span>&nbsp;<small class="form-text text-muted">(https)</small>
                        
                      </div>
                      <div class="form-row mb-3">
                        <label for="WEBWNMTH" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Mida horitzontal m&agrave;xima:</label>                        
                        <asp:textBox runat="server" ID="WEBWNMTH" value="0" MaxLength="4" class="form-control form-control-sm  col-sm-1" placeholder="mida"></asp:textBox>&nbsp;px&nbsp;<small class="form-text text-muted">(nom&eacute;s si no hereta propietats)</small>                       
                      </div>
                      <div class="form-row mb-3">
                      <label for="WEBDSEBO" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Estils body:</label>
                      <asp:TextBox runat="server" ID="WEBDSEBO" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Estils body"></asp:TextBox>
                      </div>
                   </div>
              </asp:placeholder>
              
              <asp:placeholder runat="server" id="pnlNodeWeb" visible="false">
              <div class="form-group">
                <div class="form-row mb-3"> 
                	<label for="NWEDSTIT" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">T&iacute;tol:</label>
                  	<asp:textbox runat="server" id="NWEDSTIT" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="T&iacute;tol"/>
                </div>
                <div class="form-row mb-3"> 
                  <label for="NWEDSCAR" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Carpeta:</label>
                  <asp:textbox runat="server" id="NWEDSCAR" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Carpeta"/>
                </div>
                <div class="form-row mb-3">
                  <label for="NWEDSEBO" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Estils body:</label>
                  <asp:textbox runat="server" id="NWEDSEBO" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Estils body"/>
                </div>
                <div class="form-row mb-3">
                  <label for="NWEDSMET" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Valors META:</label>
                  <asp:TextBox runat="server" ID="NWEDSMET" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" Rows="10" TextMode="MultiLine" placeholder="Valors META"/>
                </div>
                <div class="form-row mb-3">
                  <label for="NWEDSPEU" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">HTML al peu:</label>
                  <asp:TextBox runat="server"  ID="NWEDSPEU" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" Rows="10" TextMode="MultiLine" placeholder="HTML al peu"/>
               </div>
               <div class="form-row mb-3">
                  <label for="NWEDSCSP" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">CSS per pantalla:</label>
                  <asp:TextBox runat="server"  ID="NWEDSCSP" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="CSS per pantalla" />
               </div>
               <div class="form-row mb-3">
                  <label for="NWEDSCSI" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">CSS per impressora:</label>
                  <asp:TextBox runat="server"  ID="NWEDSCSI" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7"  placeholder="CSS per impressora"/>                
                </div>
                </div>
              </asp:placeholder>
            </div><!-- FI PROPIETATAS -->
            
            
            <!-- DISSENY --><div class="tab-pane fade pt-4 pb-4" id="dis" role="tabpanel" aria-labelledby="disseny">               
                <!-- DISSENY CONTENIDORS--><div class="card bg-light mb-4">
                	<!--GRIDS--><div class="card-header">                        
                    	<div class="row no-gutters"> 
                        	<div class="col-auto text-nowrap mr-3">
                                <h6 class="font-weight-bold">CONTENIDORS</h6>   
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirContenidorAbans"><img src="/GAIA2/aspx/web/img/fila_abans.png" class="mr-1">Abans</button>
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirContenidorDespres"><img src="/GAIA2/aspx/web/img/fila_despres.png" class="mr-1">Despr&eacute;s</button>    
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirContenidorAbansAdins"><img src="/GAIA2/aspx/web/img/fila_abansdins.png" class="mr-1">Abans, a dins</button>
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirContenidorDespresAdins"><img src="/GAIA2/aspx/web/img/fila_despresdins.png" class="mr-1">Despr&eacute;s, a dins</button>
                            </div>
                            <div class="col-auto mr-3">
                                <h6 class="font-weight-bold">ROWS</h6>
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirFilaDinsAbans"><img src="/GAIA2/aspx/web/img/fila_abansdins.png" class="mr-1">Abans</button>
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirFilaDinsDespres"><img src="/GAIA2/aspx/web/img/fila_despresdins.png" class="mr-1">Despr&eacute;s</button>
                            </div>
                            <div class="col-auto mr-3">
                                <h6 class="font-weight-bold">COLS</h6>
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirColumnaDinsAbans"><img src="/GAIA2/aspx/web/img/columna_abans.png" class="mr-1">Abans</button>
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirColumnaDinsDespres"><img src="/GAIA2/aspx/web/img/columna_despres.png" class="mr-1">Despr&eacute;s</button>
                            </div>
                            
                            <asp:placeholder runat="server" id="pnlWebCamps1" visible="false">                        
                            <div class="col">                              
                                    
                                    <div class="form-check form-check-inline align-bottom">                                        
                                        <asp:checkbox runat="server" CssClass="form-check-input" id="chkWEBDSCND"/>
                                        <label for="chkWEBDSCND" class="form-check-label" data-toggle="tooltip" title="Mostrar text 'contingut no disponibe' si no hi ha contingut"><small>Contingut no disponible</small></label>
                                    </div>                               
                            </div>                       
                            </asp:placeholder>       
                        </div>                                                                       
					</div><!-- FI GRIDS --> 
                    <!-- ESTRUCTURA --><div class="card-body bg-white">
                    	<div class="row no-gutters">
                            <div class="col-12 mb-3">
                                <div id="htmlEst"><asp:literal runat="server" id="ltEst"/></div>
                            </div>
                            <div class="col-12 card-text text-center pt-0">
                                <button type="button" class="btn btn-sm btn-primary" id="btnModificarDades">Modificar</button>
                                <button type="button" class="btn btn-sm btn-success" id="btnModificarDadesCel" style="display: none;">Modificar columna</button>
                                <button type="button" class="btn btn-sm btn-danger" id="btnEsborrarCel">Esborrar</button>   
                                <button type="button" class="btn btn-sm btn-info" id="btnCopiar">Copiar estructura</button>                                    
                            </div>  
                        </div>
                    </div><!-- FI ESTRUCTURA -->                    
                    <!--MIDES CONTENIDOR--><div class="card-body border-top mt-0 pt-2 pb-0">
                    	<div class="row no-gutters">
                        	<div class="col-7">
                            	<h6 class="font-weight-bold">MIDES CONTENIDOR</h6>
                                <div class="form-inline">
                                    <div class="input-group input-group-sm mr-1 mb-2">
                                        <div class="input-group-prepend"><label for="ddlXs" class="input-group-text" id="inputGroup-sizing-xs">xs</label></div>
                                        <select id="ddlXs" class="custom-select custom-select-sm" aria-label="xs:" aria-describedby="inputGroup-sizing-xs">
                                        <option></option>
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                        <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                        <option>11</option>
                                        <option>12</option>
                                        <option>Tot</option>
                                        </select>
                                    </div>
                                    <div class="input-group input-group-sm mr-1 mb-2">
                                        <div class="input-group-prepend"><label for="ddlSm" class="input-group-text" id="inputGroup-sizing-sm">sm</label></div>                                
                                        <select  id="ddlSm" class="custom-select custom-select-sm" aria-label="sm:" aria-describedby="inputGroup-sizing-sm">
                                        <option></option>
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                        <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                        <option>11</option>
                                        <option>12</option>
                                        <option>Tot</option>
                                        </select>
                                    </div>
                                    <div class="input-group input-group-sm mr-1 mb-2">
                                        <div class="input-group-prepend"><label for="ddlMd" class="input-group-text" id="inputGroup-sizing-md">md</label></div>
                                        <select  id="ddlMd" class="custom-select custom-select-sm" aria-label="md:" aria-describedby="inputGroup-sizing-md">
                                        <option></option>
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                        <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                        <option>11</option>
                                        <option>12</option>
                                        <option>Tot</option>
                                        </select>
                                   </div>
                                   <div class="input-group input-group-sm mr-1 mb-2">
                                        <div class="input-group-prepend"><label for="ddlLg" class="input-group-text" id="inputGroup-sizing-lg">lg</label></div>
                                        <select  id="ddlLg" class="custom-select custom-select-sm" aria-label="lg:" aria-describedby="inputGroup-sizing-lg">
                                        <option></option>
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                        <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                        <option>11</option>
                                        <option>12</option>
                                        <option>Tot</option>
                                        </select>
                                   </div>
                                   <div class="input-group input-group-sm mr-1 mb-2">
                                        <div class="input-group-prepend"><label for="ddlXl" class="input-group-text" id="inputGroup-sizing-xl">xl</label></div>
                                        <select  id="ddlXl" class="custom-select custom-select-sm" aria-label="xl:" aria-describedby="inputGroup-sizing-xl">
                                        <option></option>
                                        <option>1</option>
                                        <option>2</option>
                                        <option>3</option>
                                        <option>4</option>
                                        <option>5</option>
                                        <option>6</option>
                                        <option>7</option>
                                        <option>8</option>
                                        <option>9</option>
                                        <option>10</option>
                                        <option>11</option>
                                        <option>12</option>
                                        <option>Tot</option>
                                        </select>
                                    </div>
                                </div>                                
                            </div>
                            <div class="col-2">                            
                                <div class="form-group mr-2">
                                	<h6 class="font-weight-bold"><label for="selectContingut" id="inputGroup-tipusCella">TIPUS CONTENIDOR</label></h6>
                                    <select class="custom-select custom-select-sm" id="selectContingut" aria-describedby="inputGroup-tipusCella">                                 
                                         <option></option>
                                         <option value="div">div</option>                            
                                         <option value="section">section</option>
                                         <option value="nav">nav</option>
                                         <option value="header">header</option>
                                         <option value="article">article</option>
                                         <option value="aside">aside</option>
                                         <option value="details">details</option>
                                         <option value="footer">footer</option>
                                         <option value="main">main</option>
                                         <option value="summary">summary</option>
                                         <option value="hr">hr</option>
                                         <option value="p">p</option>
                                         <option value="h1">h1</option>
                                         <option value="h2">h2</option>
                                         <option value="h3">h3</option>
                                         <option value="h4">h4</option>
                                         <option value="h5">h5</option>
                                         <option value="h6">h6</option>
										 <option value="address">address</option>
										 <option value="mark">mark</option>
										 <option value="small">small</option>
                                    </select>                                    
                                </div>
                            </div>
                            <div class="col-3">
                                <div class="form-group">
                                    <h6 class="font-weight-bold" id="nomCel">NOM CONTENIDOR</h6>                                    
                                    <input type="text" class="form-control form-control-sm" id="txtNomCel" placeholder="Nom cel&middot;la"/>
                                    <input type="text" id="txtIdCel" style="display: none;" value="0"/>
                                </div> 
                            </div>                        
                        </div>
                    </div><!-- FI MIDES CONTENIDOR -->                          
                    <!-- LLIBRERIES --><div class="card-footer">                    	                        
                    	<div class="row no-gutters">
                            <div class="col-12 pb-1">
                            	<h6 class="font-weight-bold d-inline-flex mb-0">LLIBRERIES</h6><a data-toggle="collapse" class="float-right" href="#collapseLlibreries" role="button" aria-expanded="true" aria-controls="collapseLlibreries"><i class="oi oi-caret-top text-dark"></i></a></div>
                        	                     
                            <div class="collapse show col-12" id="collapseLlibreries">
                            	<div class="row no-gutters mt-3">
                                    <asp:placeholder runat="server" id="pnlLCWA">
                                    <div class="col-4">
                                        <div class="card bg-light mr-2">
                                            <div class="card-header"><h6 class="font-weight-bold mb-0">ABANS DEL CONTINGUT</h6></div>
                                            <div class="card-body p-2">
                                                <div class="form-group mb-0">
                                                    <div class="bs-component text-center">
                                                    <input type="button" data-toggle="modal" data-target="#visorArbres" id="inputIframeAbans" value="Seleccionar" class="btn btn-sm btn-primary mb-2">
        
                                                    <input type="button" onClick="document.getElementById('gaiaCodiWebNodesAbans').value = ''; document.getElementById('gaiaCodiWebTxtAbans').value=''; return false;" id="eliminarCodiAbans" value="Esborrar"  class="btn btn-sm btn-danger mb-2">
                                                    <input type="button" data-toggle="modal" data-target="#editarLlibreries" id="btnEditaCodisAbans" value="Edita Llibreria" class="btn btn-sm btn-success mb-2">                                                   
                                                    
                                                    </div>
                                                    <asp:TextBox ID="gaiaCodiWebTxtAbans" runat="server" AutoPostBack="False"  Rows="3" ContentEditable="false" TextMode="MultiLine"  CssClass="form-control form-control-sm mb-2"/>
                                                    <asp:TextBox ID="gaiaCodiWebNodesAbans" runat="server"  CssClass="form-control form-control-sm" placeholder="Codi node de la llibreria"/>
                                                </div>
                                             </div>
                                        </div>
                                    </div>
                                    </asp:placeholder>
                                    <asp:placeholder runat="server" id="pnlLCW">
                                    <div class="col-4">
                                        <div class="card bg-light mx-2">
                                            <div class="card-header"><h6 class="font-weight-bold mb-0">DINS DEL CONTINGUT</h6></div>
                                            <div class="card-body p-2">
                                                <div class="form-group mb-0">
                                                    <div class="bs-component text-center">
                                                     <input type="button" data-toggle="modal" data-target="#visorArbres" id="inputIframeDins" value="Seleccionar" class="btn btn-sm btn-primary mb-2">

                                                    <input type="button" id="eliminarCodiDins" value="Esborrar" onClick="document.getElementById('gaiaCodiWebNodesDins').value = ''; document.getElementById('gaiaCodiWebTxtDins').value = ''; return false;" class="btn btn-sm btn-danger mb-2">
                                                    <input type="button"  onClick="window.open('/GAIA/aspx/fulles/editaLCW.htm?nodesSeleccionats=' + document.getElementById('gaiaCodiWebNodesDins').value, '_blank', 'location=0,height=800,width=400,scrollbars=yes,resizable=yes'); return false;" value="Edita Llibreria" id="btnEditaCodisDins" class="btn btn-sm btn-success mb-2" >
                                                    </div>
                                                    <asp:TextBox ID="gaiaCodiWebTxtDins" runat="server" AutoPostBack="False"  Rows="3" ContentEditable="false" TextMode="MultiLine"  CssClass="form-control form-control-sm mb-2"/>
                                                    <asp:TextBox ID="gaiaCodiWebNodesDins" runat="server"  CssClass="form-control form-control-sm" placeholder="Codi node de la llibreria"/>
                                                </div>
                                             </div>
                                        </div>
                                    </div>
                                    </asp:placeholder>
                                    <asp:placeholder runat="server" id="pnlLCW2">
                                    <div class="col-4">
                                        <div class="card bg-light ml-2">                
                                            <div class="card-header"><h6 class="font-weight-bold mb-0">DESPR&Eacute;S DEL CONTINGUT</h6></div>
                                            <div class="card-body p-2">
                                                <div class="form-group mb-0">
                                                    <div class="bs-component text-center">
                                                    <input type="button" data-toggle="modal" data-target="#visorArbres" id="inputIframeDespres" value="Seleccionar" class="btn btn-sm btn-primary mb-2">

                                                    <input type="button" id="eliminarCodiDespres" value="Esborrar" onClick="document.getElementById('gaiaCodiWebNodesDespres').value = ''; document.getElementById('gaiaCodiWebTxtDespres').value = ''; return false;" class="btn btn-sm btn-danger mb-2">
                                                    <input type="button"  onClick="window.open('/GAIA/aspx/fulles/editaLCW.htm?nodesSeleccionats=' + document.getElementById('gaiaCodiWebNodesDespres').value, '_blank', 'location=0,height=800,width=400,scrollbars=yes,resizable=yes'); return false;" value="Edita Llibreria" id="btnEditaCodis2" class="btn btn-sm btn-success mb-2">
                                                    </div>
                                                    <asp:TextBox ID="gaiaCodiWebTxtDespres" runat="server" AutoPostBack="False" Rows="3" ContentEditable="false" TextMode="MultiLine" CssClass="form-control form-control-sm mb-2"/>
                                                    <asp:TextBox ID="gaiaCodiWebNodesDespres" runat="server" CssClass="form-control form-control-sm" placeholder="Codi node de la llibreria"/>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </asp:placeholder>      
                                </div>             
                            </div>                    
						</div>
                    </div><!-- FI LLIBRERIES -->                           
                </div><!--FI DISSENY CONTENIDORS-->
                
                <!-- FULLA --><div class="card bg-light mb-4">                	
                	<div class="card-header pb-2">
                    	<h6 class="font-weight-bold d-inline-flex mr-2">FULLA</h6>
                        <div class="form d-inline-flex">
                            <div class="input-group input-group-sm">                            	
                                <div class="input-group-prepend"><label for="lstTipusFulla" class="input-group-text">Tipus de fulla:</label></div>
                                <asp:DropDownList ID="lstTipusFulla" runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            <%--<asp:literal ID="lstTipusFulla" runat="server"></asp:literal>--%>
                            </div>
                        </div>  
                        <a data-toggle="collapse" class="float-right" href="#collapseFulla" role="button" aria-expanded="true" aria-controls="collapseFulla"><i class="oi oi-caret-top text-dark"></i></a>                    
                    </div> 
                        	
                    <div class="collapse show" id="collapseFulla">
                        <div class="card-body">
                            <div class="row">
                                <!-- CAMPS I PLANTILLA SECUNDARIA --><div class="col">
                                	<asp:placeholder runat="server" id="pnlPltCamps1" visible="false">
                                    <div class="form-group mb-0">    	
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend"><label for="ddlPLTDSCMP" class="input-group-text">Camp:</label></div>
                                                <select name="ddlPLTDSCMP" id="ddlPLTDSCMP" class="custom-select custom-select-sm">
                                                  <option value=""></option>
                                                </select>
                                            </div>                                        
                                       
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend"><label for="ddlPLTDSLNK" class="input-group-text">Enlla&ccedil;:</label></div>            
                                                <select name="ddlPLTDSLNK"  id="ddlPLTDSLNK" class="custom-select custom-select-sm">
                                                  <option value=""></option>
                                                </select>
                                            </div>
                                        
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend"><label for="ddlPLTDSALT" class="input-group-text">Text alternatiu:</label></div>              
                                                <select name="ddlPLTDSALT"  id="ddlPLTDSALT" class="custom-select custom-select-sm">
                                                  <option value=""></option>
                                                </select>
                                            </div>
                                           
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend"><label for="ddlPLTDSIMG" class="input-group-text">&Eacute;s imatge?:</label></div>               
                                                <select name="ddlPLTDSIMG"  id="ddlPLTDSIMG" class="custom-select custom-select-sm">
                                                  <option value="0" selected>No</option>
                                                  <option value="1">Si</option>
                                                </select>
                                            </div>
                                       
                                            <div class="input-group input-group-sm mb-2">
                                                <div class="input-group-prepend"><label for="txtPLTDSNUM" class="input-group-text">N&ordm; elements</label></div>
                                                <input name="txtPLTDSNUM" runat="server" id="txtPLTDSNUM" type="text" value="0" maxlength="3" class="form-control form-control-sm">
                                            </div>                                        
                                    </div>
                                    </asp:placeholder>                                      
                                    <asp:placeholder runat="server" id="pnlPlt">
                                    <div class="card bg-light">
                                        <div class="card-header"><h6 class="font-weight-bold mb-0">PLANTILLA <asp:placeholder runat="server" id="pnlPltCamps2" visible="false">SECUND&Agrave;RIA</asp:placeholder></h6></div>
                                        <div class="card-body p-2">
                                            <div class="form-group mb-0">
                                                <div class="bs-component text-center">
                                                <input type="button" data-toggle="modal" data-target="#visorArbres" id="inputPlantilla" value="Seleccionar" class="btn btn-sm btn-primary mb-2">
                                                <%--<input type="button"  onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?arbre1=plantillaWeb&c=gaiaPltSec&nodesSeleccionats=' + document.getElementById('gaiaPltSecNodes').value, '_blank', 'location=0,height=800,width=460,scrollbars=yes,resizable=yes'); return false;" value="Seleccionar" id="btnCodis3" class="btn btn-sm btn-primary mb-2" >--%>
                                                <input type="button" id="btnEliminarPltSec" value="Esborrar" onClick="document.getElementById('gaiaPltSecNodes').value = ''; document.getElementById('gaiaPltSecTxt').value = ''; return false;"  class="btn btn-sm btn-danger mb-2">
                                                <input type="button" data-toggle="modal" data-target="#editarPlantilles" id="btnEditaPlantilla" value="Edita plantilla (Odalys)" class="btn btn-sm btn-success mb-2">
                                                
                                                <input type="button"  onClick="window.open('/GAIA/aspx/web/editaPlantilla.htm?nodesSeleccionats=' + document.getElementById('gaiaPltSecNodes').value, '_blank', 'location=0,height=800,width=400,scrollbars=yes,resizable=yes'); return false;" value="Edita Plantilla" id="btnEditaPlt2"  class="btn btn-sm btn-success mb-2" >
                                                </div>
                                                <asp:TextBox ID="gaiaPltSecTxt" runat="server" AutoPostBack="False" Rows="3" ContentEditable="false" TextMode="MultiLine"  CssClass="form-control form-control-sm mb-2"/>
                                                <asp:TextBox ID="gaiaPltSecNodes" runat="server" CssClass="form-control form-control-sm" placeholder="Codi node de la plantilla"/>
                                            </div>
                                        </div>
                                    </div>                            
                                    </asp:placeholder>                           
                                </div><!-- CAMPS I PLANTILLA SECUNDARIA -->
                                
                               <!-- PLANTILLA AUTOENLLAÇ --><asp:placeholder runat="server" id="pnlPltCamps3" visible="false">
                               <div class="col">     
                                    <div class="card bg-light">                
                                        <div class="card-header"><h6 class="font-weight-bold d-inline-flex mb-0">AUTO-ENLLA&Ccedil;</h6></div>
                                        <div class="card-body">
                                            <div class="row no-gutters">      	
                                                <div class="col-12">
                                                    <div class="form-group mb-0">
                    
                                                        <div class="input-group input-group-sm mb-2">
                                                            <div class="input-group-prepend"><label class="input-group-text">Adre&ccedil;a:</label></div>
                                                            <asp:TextBox runat="server" ID="txtPLTDSAAL" MaxLength="100" CssClass="form-control form-control-sm"/>
                                                        </div>
                    
                    
                                                         <div class="input-group input-group-sm mb-2">   
                                                            <div class="input-group-prepend"><label for="ddlb_PLTDSALF" class="input-group-text">Target:</label></div>                
                                                            <asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_PLTDSALF" runat="server"/>
                                                        </div>
                    
                    
                                                        <div class="input-group input-group-sm mb-2">
                                                            <div class="input-group-prepend"><label for="txtPLTDSALK" class="input-group-text">Text associat:</label></div>
                                                            <asp:TextBox runat="server" ID="txtPLTDSALK" TextMode="MultiLine" Rows="4" MaxLength="500" CssClass="form-control form-control-sm"/>                            
                                                        </div>
                                                   </div>
                    
                                                   <div class="card bg-light">
                                                        <div class="card-header">
                                                            <h6 class="font-weight-bold mb-0">PLANTILLA AUTO-ENLLA&Ccedil;</h6>
                                                        </div>
                                                        <div class="card-body p-2">
                                                            <div class="form-group mb-0">
                                                                <div class="bs-component mb-2 text-center">
                                                                <input name="button" type="button" id="btnCodis5" onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?arbre1=plantillaWeb&c=gaiaPLTCDPAL&nodesSeleccionats='+document.getElementById('gaiaPLTCDPALNodes').value,'_blank', 'location=0,height=800,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" class="btn btn-sm btn-primary">
                                                                <input name="button" type="button" id="btnEliminarPLTCDPAL" onClick="document.getElementById('gaiaPLTCDPALNodes').value='';document.getElementById('gaiaPLTCDPALTxt').value=''; return false;" value="Esborrar" class="btn btn-sm btn-danger">
                                                                <input type="button" class="btn btn-sm btn-success" onClick="window.open('/GAIA/aspx/web/editaPlantilla.htm?nodesSeleccionats='+document.getElementById('gaiaPLTCDPALNodes').value+'','_blank','location=0,height=800,width=600,scrollbars=yes,resizable=yes');return false;" value="Editar Plantilla" id="btnEditaPlantillesAL" >
                                                                </div>
                                        
                                                                <asp:TextBox ID="gaiaPLTCDPALTxt" runat="server" AutoPostBack="False" Rows="3" ContentEditable="false" TextMode="MultiLine" CssClass="form-control form-control-sm mb-2"/>
                                                                
                                                                <asp:TextBox ID="gaiaPLTCDPALNodes" runat="server" CssClass="form-control form-control-sm" placeholder="Codi node de la plantilla"/>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>       
                                        </div>
                                    </div>
                                </div>
                                </asp:placeholder> <!-- FI PLANTILLA AUTOENLLAÇ -->                          
                                
                            </div>
                        </div>  
                    </div> 
                    
                                    
                </div><!-- FI FULLA -->                          
                
                <!-- ESTILS --><asp:placeholder runat="server" id="pnlBootstrap">
                <div class="card bg-light mb-4">                	
                	<div class="card-header pb-3"><h6 class="font-weight-bold d-inline-flex mb-0">ESTILS</h6><a data-toggle="collapse" class="float-right" href="#collapseEstils" role="button" aria-expanded="true" aria-controls="collapseEstils"><i class="oi oi-caret-top text-dark"></i></a></div>
                    <div class="card-body collapse show" id="collapseEstils">
                    	<div class="form-inline row no-gutters align-items-start">
                        
                        	<h6 class="col-12 font-weight-bold">REIXETA</h6>  
                                                  
							<div class="col-4 align-self-start">                            
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_642" class="input-group-text">Disposici&oacute;:</label></div>
                                    <asp:DropDownList ID="ddlb_642" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                                 <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_637" class="input-group-text">Posici&oacute;:</label></div>
                                    <asp:DropDownList  ID="ddlb_637" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>           
                        	</div>
                        
                            <div class="col-4 align-self-start">	
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_648" class="input-group-text">Alineaci&oacute; H.:</label></div>
                                    <asp:DropDownList  ID="ddlb_648" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_636" class="input-group-text">Alineaci&oacute; V.:</label></div>
                                    <asp:DropDownList  ID="ddlb_636" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>                               
                            </div>
                        
                            <div class="col-4 align-self-start">                        	 	
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_630" class="input-group-text">Flex:</label></div>
                                    <asp:DropDownList  ID="ddlb_630" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_653" class="input-group-text">Alertes:</label></div>
                                    <asp:DropDownList  ID="ddlb_653" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        
                        	<h6 class="col-12 font-weight-bold text-uppercase mt-2">Texte</h6>
                        
                            <div class="col-4 align-self-start">					
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_638" class="input-group-text">Transformaci&oacute;:</label></div>
                                    <asp:DropDownList  ID="ddlb_638" runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                                </div>
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_633" class="input-group-text">Amplada:</label></div>
                                    <asp:DropDownList  ID="ddlb_633" runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                                </div>     
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_655" class="input-group-text">Familia:</label></div>
                                    <asp:DropDownList  ID="ddlb_655" runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                                </div>
                            </div>
                        
                            <div class="col-4 align-self-start">
                                 <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_632" class="input-group-text">Alineaci&oacute; H.:</label></div>
                                    <asp:DropDownList  ID="ddlb_632" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>                             
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_650" class="input-group-text">Alineaci&oacute; V.:</label></div>
                                    <asp:DropDownList  ID="ddlb_650" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_673" class="input-group-text">Decoraci&oacute;:</label></div>
                                    <asp:DropDownList  ID="ddlb_673" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            
                            <div class="col-4 align-self-start">                            
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_649" class="input-group-text">Estil font:</label></div>
                                    <asp:DropDownList  ID="ddlb_649" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_652" class="input-group-text">Tamany font:</label></div>
                                    <asp:DropDownList  ID="ddlb_652" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                                <!--<div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlPLTDSNIV" class="input-group-text">Nivell:</label></div>
                                    <asp:DropDownList ID="ddlPLTDSNIV" runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                                </div>-->
                            </div>
                        
                            <h6 class="col-12 font-weight-bold text-uppercase mt-2">Estils comuns</h6>	
                            
                            <div class="col-4 align-self-start">  
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_651" class="input-group-text">Voreres:</label></div>
                                    <asp:DropDownList  ID="ddlb_651" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>                      
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_641" class="input-group-text">Efectes:</label></div>
                                    <asp:DropDownList  ID="ddlb_641" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                             </div>
                             <div class="col-4 align-self-start">
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_639" class="input-group-text">Marge exterior:</label></div>
                                    <asp:DropDownList  ID="ddlb_639" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_635" class="input-group-text">Al&ccedil;ada:</label></div>
                                    <asp:DropDownList  ID="ddlb_635" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-4 align-self-start">  
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_631" class="input-group-text">Marge interior:</label></div>
                                    <asp:DropDownList  ID="ddlb_631" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>  
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_634" class="input-group-text">Amplada:</label></div>
                                    <asp:DropDownList  ID="ddlb_634" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>                                                    
                            </div>
                            <div class="col-4 align-self-start">
                            <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_654" class="input-group-text">Fons:</label></div>
                                    <asp:DropDownList  ID="ddlb_654" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <h6 class="col-12 font-weight-bold text-uppercase mt-2">Media</h6>
                            
                            <div class="col-6 align-self-start">
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_674" class="input-group-text">Videos:</label></div>
                                    <asp:DropDownList  ID="ddlb_674" CssClass="custom-select custom-select-sm" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-6 align-self-start">
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_675" class="input-group-text">Imatges:</label></div>
                                    <asp:DropDownList  ID="ddlb_675" runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                                </div>                             
                           </div>
                            <h6 class="col-12 font-weight-bold text-uppercase mt-2">Altres</h6>
                            
                            <div class="col-12 align-self-start">
                            <asp:placeholder runat="server" id="pnlEstilsDefinits">
                                <div class="input-group input-group-sm mr-2 mb-2">
                                    <div class="input-group-prepend"><label for="ddlb_117" class="input-group-text">Estils definits:</label></div>
                                    <asp:DropDownList ID="ddlb_117" runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                                </div>
                            </asp:placeholder>
                            </div>
                    	</div>
                    </div>
               	</div>
                </asp:placeholder><!-- FI ESTILS -->
             </div><!--FI DISSENY --> 

              </div><!--FI TAB-CONTENT -->          
                <asp:CheckBox ID="plantillaChecked" runat="server" style="display:none" Checked="false"/>
    		</form>
		
			</div><!-- FI CONTAINER -->
		</div>
	</div>
    <div class="card-footer text-center"><button type="button" class="btn btn-sm btn-success" id="btnGuardar" runat="server" onClick="guardar();" onserverclick="btnGuardar_ServerClick">Guardar <asp:literal runat="server" id="ltTitol2"/></button>
        
    </div>
</div>

<!-- MODAL VISOR LLIBRERIES/PLANTILLES -->
<div class="modal fade" id="visorArbres" tabindex="-1" role="dialog" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Visor d'arbres</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="closeButton">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body" style="height:80vh; min-height:80vh;">      	
        <iframe id="iframeLlibreria" class="w-100 h-100 border-0" src="/GAIA2/aspx/llibreria/visorArbresLite_GAIA2.aspx" scrolling="no"></iframe>
      </div>      
    </div>
  </div>
</div>
<!-- FI MODAL VISOR LLIBRERIES/PLANTILLES -->

<!-- MODAL EDITAR LLIBRERIES -->
<div class="modal fade" id="editarLlibreries" tabindex="-1" role="dialog" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title mr-3">Editar LLibreries</h5><div class="form-inline"><select id="selectLlibreria" class="form-control form-control-sm"></select></div>  
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body" style="height:85vh; min-height:85vh;"> 
          
        <iframe id="iframeEditarLlibreries" class="w-100 h-100 border-0" src="/GAIA2/aspx/llibreria/visorLlibreriaCodiWeb_GAIA2.aspx" scrolling="no"></iframe>
      </div>      
    </div>
  </div>
</div>
<!-- FI MODAL EDITAR LLIBRERIES-->

<!-- MODAL EDITAR PLANTILLES -->
<div class="modal fade" id="editarPlantilles" tabindex="-1" role="dialog" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title mr-3">Editar Plantilles</h5><div class="form-inline"><select id="selectPlantilla" class="form-control form-control-sm"></select></div>  
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body" style="height:85vh; min-height:85vh;">           
        <iframe id="iframeEditarPlantilles" class="w-100 h-100 border-0" scrolling="no"></iframe>
      </div>      
    </div>
  </div>
</div>
<!-- FI MODAL EDITAR PLANTILLES-->




    
 
</body>
</html>

<!-- Optional JavaScript -->    
<!-- jQuery first, then Tether, then Bootstrap JS. -->   
<asp:Label ID="lblCodi" runat="server"/> 
<script src="https://code.jquery.com/jquery-3.3.1.min.js" integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=" crossorigin="anonymous"></script>
<script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js" integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js" integrity="sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy" crossorigin="anonymous"></script>
<script type="text/javascript" src="../../js/bootstrap-multiselect.js"></script>			
<script>
$(function () {
  var nua = navigator.userAgent
  var isAndroid = (nua.indexOf('Mozilla/5.0') > -1 && nua.indexOf('Android ') > -1 && nua.indexOf('AppleWebKit') > -1 && nua.indexOf('Chrome') === -1)
  if (isAndroid) {
    $('select.form-control').removeClass('form-control').css('width', '100%')
  }
})
</script>
<script>
$(function () {
  $('[data-toggle="tooltip"]').tooltip()
})
</script>

<!--[if IE]>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
<script>$('input, textarea').placeholder();</script>
<![endif]-->  

<script type="text/javascript">
  
	  $(document).ready(function () {
        var element;
        var nroId = parseInt($("#nroId").val());
       
        $('#ddlb_634').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_631').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_639').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_641').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_651').attr('title', 'multiselect').attr('multiple', 'multiple');
       
        $('#ddlb_642').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_648').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_632').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_635').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_637').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_636').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_650').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_649').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_653').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_673').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_674').attr('title', 'multiselect').attr('multiple', 'multiple');
        $('#ddlb_675').attr('title', 'multiselect').attr('multiple', 'multiple');
        
        var primerFil = $('#htmlEst').children().first();
        activado($(primerFil).attr('id'));
          readValues(primerFil);
		  var botonAccion;

          $('#inputIframeAbans').click(function (event) {
              AbrirIFrameVisor(event);
          });
          $('#inputIframeDins').click(function (event) {
              AbrirIFrameVisor(event);
          });
          $('#inputIframeDespres').click(function (event) {
              AbrirIFrameVisor(event);
          });
          $('#inputPlantilla').click(function (event) {
              AbrirIFrameVisor(event);
          });
          $('#inputPlantillaAuto').click(function (event) {
              AbrirIFrameVisor(event);
          });
          $('#btnEditaCodisAbans').click(function (event) {
              AbrirIFrameEditor(event);
          });
          $('#btnEditaCodisDins').click(function (event) {
              AbrirIFrameEditor(event);
          });
          $('#btnEditaCodisDespres').click(function (event) {
              AbrirIFrameEditor(event);
          });
          $('#btnEditaPlantilla').click(function (event) {
              AbrirIFrameEditor(event);
          });
          $('#btnEditaPlantillaAuto').click(function (event) {
              AbrirIFrameEditor(event);
          });

          function AbrirIFrameEditor(event) {
              botonAccion = event.target; //qué boton dió la orden de abrir modal           

              var LlibreriasTxt = "";
              var LlibreriasNod = "";
              $('select', 'div#editarLlibreries').find('option').remove();

              switch ($(botonAccion).attr('id')) {
                  case 'btnEditaCodisAbans':
                      LlibreriasTxt += $('#gaiaCodiWebTxtAbans').val();
                      LlibreriasNod += $('#gaiaCodiWebNodesAbans').val(); //id de la llibreria para editar
                      OpenLlibreriaModal(LlibreriasTxt, LlibreriasNod);
                      break;
                  case 'btnEditaCodisDins':
                      LlibreriasTxt += $('#gaiaCodiWebTxtDins').val();
                      LlibreriasNod += $('#gaiaCodiWebNodesDins').val(); //id de la llibreria para editar
                      OpenLlibreriaModal(LlibreriasTxt, LlibreriasNod);
                      break;
                  case 'btnEditaCodisDespres':
                      LlibreriasTxt += $('#gaiaCodiWebTxtDespres').val();
                      LlibreriasNod += $('#gaiaCodiWebNodesDespres').val(); //id de la llibreria para editar
                      OpenLlibreriaModal(LlibreriasTxt, LlibreriasNod);
                      break;
                  case 'btnEditaPlantilla':
                      LlibreriasTxt += $('#gaiaPltSecTxt').val();
                      LlibreriasNod += $('#gaiaPltSecNodes').val(); //id de la llibreria para editar
                      OpenPlantillaModal(LlibreriasTxt, LlibreriasNod);
                      break;
                  case 'btnEditaPlantillaAuto':
                      LlibreriasTxt += $('#gaiaPLTCDPALTxt').val();
                      LlibreriasNod += $('#gaiaPLTCDPALNodes').val(); //id de la llibreria para editar
                      OpenPlantillaModal(LlibreriasTxt, LlibreriasNod);
                      break;


              }
          };

          $("#selectLlibreria").change(function () {
              var sel = $("#selectLlibreria option:selected").val();
              if (sel == "") { sel = 0 };
              var idiArbre = 1; //parametro de idioma
              var urlIFrame = $('#iframeEditarLlibreries'); //iframe
              $(urlIFrame).attr('src', "/GAIA2/aspx/llibreria/visorLlibreriaCodiWeb_GAIA2.aspx" + "?id=" + sel + "&idiArbre=" + idiArbre);
          });

          function OpenLlibreriaModal(LlibreriasTxt, LlibreriasNod) {
              var idiArbre = 1; //parametro de idioma
              var arrayLlibreriasTxt = [];
              var arrayLlibreriasNod = [];
              if (!jQuery.isEmptyObject(LlibreriasNod)) {
                  if (LlibreriasNod.indexOf('|') >= 0) {
                      arrayLlibreriasTxt = LlibreriasTxt.split(',');
                      arrayLlibreriasNod = LlibreriasNod.split('|');
                      $('select', 'div#editarLlibreries').css('display', '');
                      $('select', 'div#editarLlibreries').append(new Option('Elige una llibreria', '', true, true));
                      arrayLlibreriasTxt.forEach(function (item, index) {
                          if (item != "") {
                              $('select', 'div#editarLlibreries').append(new Option(item, arrayLlibreriasNod[index]));
                          }
                      });
                      $('#iframeEditarLlibreries').attr('src', "/GAIA2/aspx/llibreria/visorLlibreriaCodiWeb_GAIA2.aspx" + "?id=" + 0 + "&idiArbre=" + idiArbre);
                      $('#editarLlibreries').modal('show');
                  } else {

                      $('select', 'div#editarLlibreries').css('display', 'none');
                      $('#iframeEditarLlibreries').attr('src', "/GAIA2/aspx/llibreria/visorLlibreriaCodiWeb_GAIA2.aspx" + "?id=" + LlibreriasNod + "&idiArbre=" + idiArbre);
                      $('#editarLlibreries').modal('show');
                  }
              }
          };

          $("#selectPlantilla").change(function () {
              var sel = $("#selectPlantilla option:selected").val();
              if (sel == "") { sel = 0 };
              var idiArbre = 1; //parametro de idioma
              var urlIFrame = $('#iframeEditarPlantilles'); //iframe
              $(urlIFrame).attr('src', "/GAIA2/aspx/web/visorPlantilla_GAIA2.aspx" + "?id=" + sel + "&idiArbre=" + idiArbre);
          });

          function OpenPlantillaModal(LlibreriasTxt, LlibreriasNod) {
              var idiArbre = 1; //parametro de idioma
              var arrayLlibreriasTxt = [];
              var arrayLlibreriasNod = [];
              if (!jQuery.isEmptyObject(LlibreriasNod)) {
                  if (LlibreriasNod.indexOf('|') >= 0) {
                      arrayLlibreriasTxt = LlibreriasTxt.split(',');
                      arrayLlibreriasNod = LlibreriasNod.split('|');
                      $('select', 'div#editarPlantilles').css('display', '');
                      $('select', 'div#editarPlantilles').append(new Option('Elige una plantilla', '', true, true));
                      arrayLlibreriasTxt.forEach(function (item, index) {
                          if (item != "") {
                              $('select', 'div#editarPlantilles').append(new Option(item, arrayLlibreriasNod[index]));
                          }
                      });
                      $('#iframeEditarPlantilles').attr('src', "/GAIA2/aspx/web/visorPlantilla_GAIA2.aspx" + "?id=" + 0 + "&idiArbre=" + idiArbre);
                      $('#editarPlantilles').modal('show');
                  } else {

                      $('select', 'div#editarPlantilles').css('display', 'none');
                      $('#iframeEditarPlantilles').attr('src', "/GAIA2/aspx/web/visorPlantilla_GAIA2.aspx" + "?id=" + LlibreriasNod + "&idiArbre=" + idiArbre);
                      $('#editarPlantilles').modal('show');
                  }
              }
          };


          function AbrirIFrameVisor(event) {
              botonAccion = event.target;
              var urlIFrame = $('#iframeLlibreria');
              var arbre1 = "codiWeb";
              var c = "gaiaCodiWeb";
              var separador = "|";
              switch ($(botonAccion).attr('id')) {
                  case 'inputIframeAbans':
                      var nodesSeleccionats = $('#gaiaCodiWebNodesAbans').val();
                      break;
                  case 'inputIframeDins':
                      var nodesSeleccionats = $('#gaiaCodiWebNodesDins').val();
                      break;
                  case 'inputIframeDespres':
                      var nodesSeleccionats = $('#gaiaCodiWebNodesDespres').val();
                      break;
                  case 'inputPlantilla':
                      arbre1 = "plantillaWeb";
                      c = "gaiaPltSec";
                      var nodesSeleccionats = $('#gaiaPltSecNodes').val();
                      break;
                  case 'inputPlantillaAuto':
                      arbre1 = "plantillaWeb";
                      c = "gaiaPLTCDPAL";
                      var nodesSeleccionats = $('gaiaPLTCDPALNodes').val();
                      break;
              }
              var urlIFra = "/GAIA2/aspx/llibreria/visorArbresLite_GAIA2.aspx" + "?arbre1=" + arbre1 + "&c=" + c + "&separador=" + separador + "&nodesSeleccionats=" + nodesSeleccionats;
              $(urlIFrame).attr('src', urlIFra);
          };

          $('#iframeLlibreria').on('load', function (event) {
              var frame = this.contentDocument;
              var doc = $(frame).find('body');
              var formDocumentoVisor = $(doc).children('form');
              var textNodes = $(formDocumentoVisor).find('#labelNodes').text();
              var nroNodes = $(formDocumentoVisor).find('#labelNroNodes').text();
              var retornarNodesButton = $(formDocumentoVisor).find('#retornarNodesChecked');
              switch ($(botonAccion).attr('id')) {
                  case 'inputIframeAbans':
                      $('#gaiaCodiWebTxtAbans').val(textNodes);
                      $('#gaiaCodiWebNodesAbans').val(nroNodes);
                      break;
                  case 'inputIframeDins':
                      $('#gaiaCodiWebTxtDins').val(textNodes);
                      $('#gaiaCodiWebNodesDins').val(nroNodes);
                      break;
                  case 'inputIframeDespres':
                      $('#gaiaCodiWebTxtDespres').val(textNodes);
                      $('#gaiaCodiWebNodesDespres').val(nroNodes);
                      break;
                  case 'inputPlantilla':
                      $('#gaiaPltSecTxt').val(textNodes);
                      $('#gaiaPltSecNodes').val(nroNodes);
                      break;
                  case 'inputPlantillaAuto':
                      $('#gaiaPLTCDPALTxt').val(textNodes);
                      $('#gaiaPLTCDPALNodes').val(nroNodes);
                      break;
              }
              if ($(retornarNodesButton).attr('Checked')) {
                  $('#closeButton').click();
                  $('textarea').change();

              }
          });

          $('#iframeEditarLlibreries').on('load', function (event) {
              var frame = this.contentDocument;
              var doc = $(frame).find('body').find('form');
              if ($(doc).find('#checkedAfegirCodi').attr('checked')) {
                  $('button', 'div#editarLlibreries').click();
                  $('textarea').change();
              }
          });
          

        function RowsAndColumns(type) {
            var contenidor = "";
            var id = 'd' + nroId.toString();
            // $('#htmlEst').find('.' + type).length; 
            switch (type) {
                case "row":
                    contenidor = '<div class= "row b-inicial inactivo" id="' + id + '"><span class="atributs" style="display: none;">'+id+'#'+type+'#####################################################################|</span></div>';
                    break;
                case "col":
                    contenidor = '<div class= "col b-inicial inactivo" id="' + id + '"><span class="atributs" style="display: none;">'+id+'#'+type+'#####################################################################|</span></div>';
                    break;
            };
            return contenidor;
        };

        function Tipus(nroId) {
            var nombre = "";
            var tipus = $('#selectContingut').children('option:selected').val();
            var id = 'd' + nroId.toString();
            var innerSpan = id + '#' + tipus + '#######################################################################|';
            switch (tipus) {
                case "":
                    alert("Seleccione un elemento");
                    break;
                case "div":
                    var nombre = '<div class = "b-inicial inactivo" id=' + id + '> <span class="atributs" style="display: none;">'+innerSpan+'</span></div>';
                    break;
                case "section":
                    var nombre = '<section class = "b-inicial inactivo" id=' + id + '> <span class="atributs" style="display: none;">'+innerSpan+'</span></section>';
                    break;
                case "nav":
                    var nombre = '<nav class = "b-inicial inactivo" id=' + id + '> <span class="atributs" style="display: none;">'+innerSpan+'</span></nav>';

                    break;
                case "header":
                    var nombre = '<header class = "b-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">'+innerSpan+'</span></header>';

                    break;
                case "article":
                    var nombre = '<article class = "b-inicial inactivo" id=' + id + '> <span class="atributs" style="display: none;">'+innerSpan+'</span></article>';

                    break;
                case "iframe":
                    var nombre = '<iframe class = "b-inicial  inactivo" id=' + id + '> <span class="atributs" style="display: none;">' + innerSpan + '</span></iframe>';

                case "video":
                    var nombre = '<video class = "b-inicial  inactivo" id=' + id + '> <span class="atributs" style="display: none;">' + innerSpan + '</span></video>';
														  

                    break;
                case "details":
                    var nombre = '<details class = "b-inicial  inactivo" id=' + id + ' > <span class="atributs" style="display: none;">'+innerSpan+'</span></details>';
                    break;
                case "footer":
                    var nombre = '<footer class = "b-inicial  inactivo" id=' + id + ' > <span class="atributs" style="display: none;">'+innerSpan+'</span></footer>';
                    break;
                case "main":
                    var nombre = '<main class = "b-inicial  inactivo" id=' + id + ' > <span class="atributs" style="display: none;">'+innerSpan+'</span></main>';
                    break;
                case "summary":
                    var nombre = '<summary class = "b-inicial  inactivo" id=' + id + '> <span class="atributs" style="display: none;">'+innerSpan+'</span></summary>';
                    break;
                case "hr":
                    var nombre = '<div class="hr-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">'+innerSpan+'</span></div>';
                    break;
                case "p":
                    var nombre = '<p class="b-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">' + innerSpan + '</span></p>';
                    break;
                case "h1":
                    var nombre = '<h1 class="b-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">' + innerSpan + '</span></h1>';
                    break;
                case "h2":
                    var nombre = '<h2 class="b-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">' + innerSpan + '</span></h2>';
                    break;
                case "h3":
                    var nombre = '<h3 class="b-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">' + innerSpan + '</span></h3>';
                    break;
                case "h4":
                    var nombre = '<h4 class="b-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">' + innerSpan + '</span></h4>';
                    break;
                case "h5":
                    var nombre = '<h5 class="b-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">' + innerSpan + '</span></h5>';
                    break;
                case "h6":
                    var nombre = '<h6 class="b-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">' + innerSpan + '</span></h6>';
                    break;
			    case "address":
                    var nombre = '<address class="b-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">' + innerSpan + '</span></address>';
                    break;
				case "mark":
                    var nombre = '<mark class="b-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">' + innerSpan + '</span></mark>';
                    break;
			    case "small":
                    var nombre = '<small class="b-inicial inactivo" id=' + id + '><span class="atributs" style="display: none;">' + innerSpan + '</span></small>';
                    break;
            };
            return nombre;
        };

        $("#btnAfegirContenidorAbans").click(function () {
            //gravo canvis en la cel·la actual
            //$("#btnModificarDades").click();
		   nroId++;
            element = $('#htmlEst').find('*').filter(function () {
                if ($(this).hasClass('activo')) {
                    return this;
                } else {
                    return null;
                }
            });
            if (!jQuery.isEmptyObject(element)) {
                var valor = Tipus(nroId);
                $(element).before(valor);
                $(element).removeClass('activo').addClass('inactivo');               
                var IdValor = $(valor).attr('id');
                $('#txtIdCel').removeAttr('value');
                $('#txtIdCel').val(IdValor);
                $('#' + IdValor).removeClass('inactivo').addClass('activo');
                readValues(valor);
                $('#btnModificarDades').click();
            } else {
                alert("Active un contenidor");
            };
            //guardo els canvis 
            $("#txtEst").val($("#htmlEst").html());
        });			
						
        $("#btnAfegirContenidorDespres").click(function () {
            //gravo canvis en la cel·la actual
            nroId++;
							

            element = $('#htmlEst').find('*').filter(function () {
                if ($(this).hasClass('activo')) {
                    return this;
                } else {
                    return null;
                }
            });

            if (!jQuery.isEmptyObject(element)) {
                var valor = Tipus(nroId);
                $(element).after(valor);
                $(element).removeClass('activo').addClass('inactivo');
                
                var IdValor = $(valor).attr('id');
                $('#txtIdCel').removeAttr('value');
                $('#txtIdCel').val(IdValor);
                $('#' + IdValor).removeClass('inactivo').addClass('activo');
                readValues(valor);
                $('#btnModificarDades').click();
            } else {
                alert("Active un contenidor");
            };
			 
            //guardo els canvis 
            $("#txtEst").val($("#htmlEst").html());
        });		
       $("#btnAfegirContenidorAbansAdins").click(function () {
            nroId++;
            //gravo canvis en la cel·la actual
            //$("#btnModificarDades").click();
            element = $('#htmlEst').find('*').filter(function () {
                if ($(this).hasClass('activo')) {
                    return this;
                } else {
                    return null;
                }
            });

            if (!jQuery.isEmptyObject(element)) {
                var lastSpan = $(element).children('span').last();
                var valor = Tipus(nroId);
                $(lastSpan).after(valor);
                $(element).removeClass('activo').addClass('inactivo');
                var IdValor = $(valor).attr('id');
                $('#txtIdCel').removeAttr('value');
                $('#txtIdCel').val(IdValor);
                $('#' + IdValor).removeClass('inactivo').addClass('activo');
                readValues(document.getElementById(IdValor));
                $('#btnModificarDades').click();

            } else {
                alert("Active un contenidor");
            };			 
            //guardo els canvis 
            $("#txtEst").val($("#htmlEst").html());
        });
		    
			$("#btnAfegirContenidorDespresAdins").click(function () {												 
            nroId++;
			element = $('#htmlEst').find('*').filter(function () {
                if ($(this).hasClass('activo')) {
                    return this;
                } else {
                    return null;
                }											   
            });	
            if (!jQuery.isEmptyObject(element)) {
                var valor = Tipus(nroId);
                $(element).append(valor);
                $(element).removeClass('activo').addClass('inactivo');
               
                var IdValor = $(valor).attr('id');
                $('#txtIdCel').removeAttr('value');
                $('#txtIdCel').val(IdValor);
                $('#' + IdValor).removeClass('inactivo').addClass('activo');
                readValues(valor);
                $('#btnModificarDades').click();
            } else {
                alert("Active un contenidor");
            };
            //guardo els canvis 		
            $("#txtEst").val($("#htmlEst").html());
        });
		
		//afegir fila en el nivell adins
        $("#btnAfegirFilaDinsDespres").click(function () {
            nroId++;
            //gravo canvis en la cel·la actual
			element = $('#htmlEst').find('*').filter(function () {
                if ($(this).hasClass('activo')) {
                    return this;
                } else {
                    return null;
                }												   
            });

            if (!jQuery.isEmptyObject(element)) {
                var elementToInsert = RowsAndColumns('row');
                $(element).append(elementToInsert);
                $(element).removeClass('activo').addClass('inactivo');				 
                
                var IdValor = $(elementToInsert).attr('id');
                $('#txtIdCel').removeAttr('value');
                $('#txtIdCel').val(IdValor);
                $('#' + IdValor).removeClass('inactivo').addClass('activo');
                readValues(elementToInsert);
                $('#btnModificarDades').click();
            } else {
                alert("Active un contenidor");
            };
            $("#txtEst").val($("#htmlEst").html());
	 
        });	

        //afegir fila en el nivell adins
        $("#btnAfegirFilaDinsAbans").click(function () {
            nroId++;
            //gravo canvis en la cel·la actual												 

            element = $('#htmlEst').find('*').filter(function () {
                if ($(this).hasClass('activo')) {
                    return this;																																																																																																																													 
                } else {
                    return null;
                }
			});

            if (!jQuery.isEmptyObject(element)) {
                var elementToInsert = RowsAndColumns('row');
                var lastSpan = $(element).children('span').last();
                $(lastSpan).after(elementToInsert);
                $(element).removeClass('activo').addClass('inactivo');
                
                var IdValor = $(elementToInsert).attr('id');
                $('#txtIdCel').removeAttr('value');
                $('#txtIdCel').val(IdValor);
                $('#' + IdValor).removeClass('inactivo').addClass('activo');
                readValues(elementToInsert);
                $('#btnModificarDades').click();
            } else {
                alert("Active un contenidor");
            };
            $("#txtEst").val($("#htmlEst").html());
        });
		
		        $("#btnAfegirColumnaDinsAbans").click(function () {
            nroId++;
            //gravo canvis en la cel·la actual												 

            element = $('#htmlEst').find('*').filter(function () {
                if ($(this).hasClass('activo')) {
                    return this;																																																																																																																													 
                } else {
                    return null;
                }
			});

             if (!jQuery.isEmptyObject(element)) {
                var elementToInsert = RowsAndColumns('col');
                if ($(element).hasClass('col')) {
                    $(element).before(elementToInsert);
                } else {
                    if ($(element).hasClass('row')) {
                        var lastSpan = $(element).children('span').last();
                        $(lastSpan).after(elementToInsert);
                    } else {
                        alert('primero inserte una fila');
                    }
                }
                $(element).removeClass('activo').addClass('inactivo');
               
                var IdValor = $(elementToInsert).attr('id');
                $('#txtIdCel').removeAttr('value');
                $('#txtIdCel').val(IdValor);
                $('#' + IdValor).removeClass('inactivo').addClass('activo');
                readValues(elementToInsert);
                $('#btnModificarDades').click();
            } else {
                alert("Active un contenidor");
            };
            $("#txtEst").val($("#htmlEst").html());
      });
	  
	          $("#btnAfegirColumnaDinsDespres").click(function () {
            nroId++;
           //gravo canvis en la cel·la actual											

            element = $('#htmlEst').find('*').filter(function () {
                if ($(this).hasClass('activo')) {
                    return this;
                } else {
                    return null;
                } 
			});
            if (!jQuery.isEmptyObject(element)) {
                var elementToInsert = RowsAndColumns('col');
                if (element.hasClass('col')) {
                    $(element).after(elementToInsert);
                } else {
                    if (element.hasClass('row')) {
                        var lastSpan = $(element).children('span').last();
                        $(lastSpan).after(elementToInsert);
                    } else {
                        alert('primero inserte una fila');
                    }
                }
                $(element).removeClass('activo').addClass('inactivo');
                var IdValor = $(elementToInsert).attr('id');
                $('#txtIdCel').removeAttr('value');
                $('#txtIdCel').val(IdValor);
                $('#' + IdValor).removeClass('inactivo').addClass('activo');
                readValues(elementToInsert);
                $('#btnModificarDades').click();
            } else {
                alert("Active un contenidor");
            };
			$("#txtEst").val($("#htmlEst").html());
        });
		
		 $("#btnEsborrarCel").click(function () {
            //buscar elemento activo para borrar																																																																																																																																																								  
            element = $('#htmlEst').find('*').filter(function () {
                if ($(this).hasClass('activo')) {
                    return this;
                } else {
                    return null;
                }						 
            });
            if (!jQuery.isEmptyObject(element)) {
                if ($(element).prop('tagName').toLowerCase() == 'span' && $(element).hasClass('text')) {
                    $(element).siblings('.divImg').remove();
                    $(element).remove();
                } else {																			
					  
                    $(element).remove();
                }
            } else {
                alert("No hay elemento activado");
            };
            //guardo els canvis 
            $("#txtEst").val($("#htmlEst").html());
        });

      $("#btnCopiar").click(function () {

            $("#divEst").html($("#htmlEst").html());

            //// limpio la estructura
            $("#divEst").find("span.divImg").remove();
            $("#divEst").find("span.atributs").remove();
            $("#divEst").find("span.text").remove();
                        
            $("#divEst").find("*").not('span').removeClass(" b-inicial activo inactivo");
																																   
            $("#txtEstBD").val($("#divEst").html());

            var copyText = $("#txtEstBD");

            /* Select the text field */
            copyText.select();

            /* Copy the text inside the text field */
            document.execCommand("copy");
        });

        $("#btnModificarDadesCel").click(function () {

            var strAtr = "";        
           
            element = $('#htmlEst').find('*').filter(function () {
                if ($(this).hasClass('activo')) {
                    return this;
                } else {
                    return null;
                }
            });
            if (!jQuery.isEmptyObject(element)) {
                //element.find("span.text").text($("#txtNomCel").val());																   

                strAtr += $("#txtIdCel").val() + "#" + $("#txtNomCel").val(); //1#2

                strAtr = ReadFromSelectDadesMides(strAtr, element);

                strAtr += "#";
                if ($("#lstTipusFulla").val() != null) {
                    strAtr += $.trim($("#lstTipusFulla").val()); //8
                }

                strAtr += "#";
                if ($("#ddlPLTDSCMP").val() != null) {
                    strAtr += $.trim($("#ddlPLTDSCMP").val()); //9
                }
                strAtr += "#";
                if ($("#ddlPLTDSLNK").val() != null) {
                    strAtr += $.trim($("#ddlPLTDSLNK").val()); //10
                }
                strAtr += "#";
                if ($("#ddlPLTDSALT").val() != null) {
                    strAtr += $.trim($("#ddlPLTDSALT").val()); //11
                }

                strAtr += "#";
                if ($("#ddlPLTDSIMG").val() != null) {
                    strAtr += $.trim($("#ddlPLTDSIMG").val()); //12
                }  
                strAtr = ReadFromSelectDadesLlibrerias(strAtr);

                strAtr += "#"
                if ($("#txtPLTDSNUM").val() != null) {
                    strAtr += $.trim($("#txtPLTDSNUM").val()); //21
                }

                strAtr += "#";
                if ($("#ddlPLTDSNIV").val() != null) {
                    strAtr += $.trim($("#ddlPLTDSNIV").val()); //22
                }

                strAtr += "#";
                if ($("#txtPLTDSAAL").val() != null) {
                    strAtr += $.trim($("#txtPLTDSAAL").val()); //23
                }

                strAtr += "#";
                if ($("#ddlb_PLTDSALF").val() != null) {
                    strAtr += $.trim($("#ddlb_PLTDSALF").val()); //24
                }

                strAtr += "#";
                if ($("#txtPLTDSALK").val() != null) {
                    strAtr += $.trim($("#txtPLTDSALK").val()); //25
                }

                strAtr += "#";
                if ($("#gaiaPLTCDPALTxt").val() != null) {
                    strAtr += $.trim($("#gaiaPLTCDPALTxt").val()); //26
                }

                strAtr += "#";
                if ($("#gaiaPLTCDPALNodes").val() != null) {
                    strAtr += $.trim($("#gaiaPLTCDPALNodes").val()); //27
                }

                

                strAtr += "#";
                strAtr += $("#chkWEBDSCND").is(":checked"); //28

                strAtr = ReadFromSelectDadesEstils(strAtr, element);

                modificaIcona(element);
                //afegeixo els atributs a l'array de propietats
                $(element).children("span.atributs").text(strAtr);

                //guardo els canvis 
                $("#txtEst").val($("#htmlEst").html());
            } else {
                alert('Selecciones un elemento');
            }
        });

        $("#btnModificarDades").click(function () {
            
            if (!jQuery.isEmptyObject(element)) {                
                $("#btnModificarDadesCel").click();
                return true;                
            }
        });
        function RemoveClassInElement(valor, patron) {
            $(valor).removeClass(function (index, classNames) {
                var current_classes = classNames.split(" "), // change the list into an array
                    classes_to_remove = []; // array of classes which are to be removed

                $.each(current_classes, function (index, class_name) {
                    // if the classname begins with bg add it to the classes_to_remove array
                    if (patron.test(class_name)) {
                        classes_to_remove.push(class_name);
                    }
                });
                // turn the array back into a string
                return classes_to_remove.join(" ");
            });

        };	

        function ReadFromSelectDadesLlibrerias(strAtr) {

            strAtr += "#";
            if ($("#gaiaCodiWebTxtAbans").val() != null) {
                strAtr += $.trim($("#gaiaCodiWebTxtAbans").val());//13
            }
            strAtr += "#";
            if ($("#gaiaCodiWebNodesAbans").val() != null) {
                strAtr += $.trim($("#gaiaCodiWebNodesAbans").val());//14
            }
            strAtr += "#";
            if ($("#gaiaCodiWebTxtDins").val() != null) {
                strAtr += $.trim($("#gaiaCodiWebTxtDins").val());//15
            }
            strAtr += "#";
            if ($("#gaiaCodiWebNodesDins").val() != null) {
                strAtr += $.trim($("#gaiaCodiWebNodesDins").val());//16
            }
            strAtr += "#";
            if ($("#gaiaCodiWebTxtDespres").val() != null) {
                strAtr += $.trim($("#gaiaCodiWebTxtDespres").val());//17
            }
            strAtr += "#";
            if ($("#gaiaCodiWebNodesDespres").val() != null) {
                strAtr += $.trim($("#gaiaCodiWebNodesDespres").val());//18
            }
            strAtr += "#";
            if ($("#gaiaPltSecTxt").val() != null) {
                strAtr += $.trim($("#gaiaPltSecTxt").val()); //19
            }
            strAtr += "#";
            if ($("#gaiaPltSecNodes").val() != null) {
                strAtr += $.trim($("#gaiaPltSecNodes").val()); //20
            }
            return strAtr;
        };

        function ReadFromSelectDadesMides(strAtr, element) {

            strAtr += "#";
            var data = $("#ddlXs").children("option:selected").text();
            if (!jQuery.isEmptyObject(data)) {
                //    contadorMides++;
                RemoveClassInElement(element, new RegExp("col-xs"));
                if (data != 'Tot') {
                    $(element).addClass("col-xs-" + data);
                    strAtr += data;  //#3
                }
                else {
                    $(element).addClass("col-xs");
                    strAtr += "0";
                }
            }
            strAtr += "#";
            var data = $("#ddlSm").children("option:selected").text();
            if (!jQuery.isEmptyObject(data)) {
                //$("#ddlSm").val() != '') {
                RemoveClassInElement(element, new RegExp("col-sm"));
                //   contadorMides++;
                if (data != 'Tot') {
                    $(element).addClass("col-sm-" + data);
                    strAtr += data; //4
                }
                else {
                    $(element).addClass("col-sm");
                    strAtr += "0";
                }
            }
            strAtr += "#";
            var data = $("#ddlMd").children("option:selected").text();
            if (!jQuery.isEmptyObject(data)) {
                RemoveClassInElement(element, new RegExp("col-md"));
                //  contadorMides++;
                if (data != 'Tot') {
                    $(element).addClass("col-md-" + data);
                    strAtr += data; //5
                }
                else {
                    $(element).addClass("col-md");
                    strAtr += "0";
                }
            }
            strAtr += "#";
            var data = $("#ddlLg").children("option:selected").text();
            if (!jQuery.isEmptyObject(data)) {
                RemoveClassInElement(element, new RegExp("col-lg"));
                //  contadorMides++;
                if (data != 'Tot') {
                    $(element).addClass("col-lg-" + data);
                    strAtr += data; //6
                }
                else {
                    $(element).addClass("col-lg");
                    strAtr += "0";
                }
            }
            strAtr += "#";
            var data = $("#ddlXl").children("option:selected").text();
            if (!jQuery.isEmptyObject(data)) {
                RemoveClassInElement(element, new RegExp("col-xl"));
                //  contadorMides++;
                if (data != 'Tot') {
                    $(element).addClass("col-xl-" + data);
                    strAtr += data; //7
                }
                else {
                    $(element).addClass("col-xl");
                    strAtr += "0";
                }
            }
            return strAtr;
        };


        function ReadFromSelectDadesEstils(strAtr, element) {

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_117', element);//29
            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_630', element);//30
            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_631', element);//31

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_632', element);//32
            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_633', element);//33
			
            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_634', element);//34

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_635', element);//35

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_636', element);//36

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_637', element);//37

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_638', element);//38

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_639', element);//39

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_641', element);//40

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_642', element);//41

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_648', element);//42

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_649', element);//43

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_650', element);//44

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_651', element);//45

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_652', element);//46

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_653', element);//47

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_654', element);//48

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_655', element);//49

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_673', element);//50

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_674', element);//51

            strAtr += "#";
            strAtr = getSelectedOptions(strAtr, 'ddlb_675', element);//52
											  
            strAtr += "|"
            return strAtr;
		
		};

	       function getSelectedOptions(strAtr, id, element) {
            var clase = [];
            var valor = [];
            var IdSelect = '#' + id;
            $(IdSelect).children('option:selected').map(function (index, item) {
                clase.push(item.text);
                valor.push(item.value);
            });
            if (!jQuery.isEmptyObject(element)) {
                $.map($(IdSelect).children('option'), function (e) {
                    if (!jQuery.isEmptyObject($(e).text())) {
                        RemoveClassInElement(element, new RegExp($(e).text()));
                    };
                });
                strAtr += valor.join(",");
                $.each(clase, function (i, item) { $(element).addClass(item); });
            }
            return strAtr;
        };       
        document.addEventListener('click', function (event) {           
            var elementInicial = event.target;
            
            var tagNameElement = $(elementInicial).prop('tagName');
            var multiselect = $(elementInicial).attr('title') == 'multiselect' ? true : false;
            if ($(elementInicial).parents('#htmlEst').length > 0) {
                switch ($(elementInicial).prop('tagName').toLowerCase()) {
                    case 'span':
                        break;
                    case 'img':
                        break;
                    default:
                        activado($(elementInicial).attr('id'));
                        readValues(elementInicial);
                        event.stopPropagation();
                        break;
                }
                
            };         
        }, true);

        document.addEventListener('change', function (event) {
            var elementInicial = event.target;            
            
            if ($(elementInicial).prop('tagName') == 'INPUT' && $(elementInicial).attr('id') == 'txtNomCel') {
                UpdateInput($(elementInicial).val());
            } else {
                if ($(elementInicial).attr('id') == 'lstTipusFulla') {
                    canviCampsDB($(elementInicial).val());                    
                }
                  GuardaValoresDeSelectCuandoCambien(elementInicial);    
            }
        });
		
		$('div.mydropdown').on('hidden.bs.dropdown', function (event) {
            var elementInicial = event.target;
            GuardaValoresDeSelectCuandoCambien($(elementInicial).siblings('select'));            
        });

        function GuardaValoresDeSelectCuandoCambien(valor) {
            var thisSelect = $(valor).attr('id');
            if ($(valor).attr('id') != 'selectContingut') {
                $('#btnModificarDadesCel').click();              
            }
        }																	 

        $('textarea').change(function () {           
                $('#btnModificarDadesCel').click();
                
            return false;
        });
       
        function UpdateInput(e) {
            element = $('#htmlEst').find('*').filter(function () {
                if ($(this).hasClass('activo')) {
                    return this;
                } else {
                    return null;
                }
            });													 
						 
            if (!jQuery.isEmptyObject(element)) {
                
               
                    if ($(element).is('span.text')) {
                        $(element).text(e);
                    } else {
                        if ($(element).children('span.text').length == 0) {
                            $(element).append('<span class="divImg" ></span>');
                            $(element).append('<span class="text" ></span>');
                            $(element).children('span.text').text(e);
                           
                        } else {
                            $(element).children('span.text').text(e);
                        }
                    }                
                $('#btnModificarDades').click();
                $("#txtEst").val($("#htmlEst").html());
            }            
            return false;
        };

      
    }); 												 
						  
    function setActiveLiToSelectedOption(id, valores) {
        $.map($('#' + id).children('option'), function (item) { $(item).removeAttr('selected').prop('selected', false) });
        var options = [];
        if (valores.indexOf(',') > -1) {
            options = valores.split(',');
        } else {
            $(options).push(valores);
        }
        if (options.length > 1) {

            $.each(options, function (index, item) {
                if (!jQuery.isEmptyObject(item)) {
                    var textSelectedOption = $('#' + id).children('option').filter(function () { return $(this).text().toLowerCase() === $.trim(item); });
                    $(textSelectedOption).attr("selected", "selected").prop('selected', true);
                }
            });
        } else {
            var textSelectedOption = $('#' + id).children('option').filter(function () { return $(this).text().toLowerCase() === $.trim(valores); });
            $(textSelectedOption).attr("selected", "selected").prop('selected', true);
        }
		
    };
    function selectClick(id) {
        element = $('#htmlEst').find('*').filter(function () {
            if ($(this).hasClass('activo')) {
                return this;
            } else {
                return null;
            }
        });
        if (element.length > 0) {
            var selectedElement = document.getElementById(id);
            $(selectedElement).multiselect({
                disableIfEmpty: true,
                maxHeight: 200,
                onChange: function (option, checked) {
                    if (checked) {
                        $(selectedElement).multiselect('select', option.val());
                    } else {
                        $(selectedElement).multiselect('deselect', option.val());
                    }
                },
                onDropdownHidden: function (event) {
                    var lista = [];
                    var data = event.relatedTarget;
                    var allActives = $(data).siblings('ul').find('li.active');
                    $.each(allActives, function (index, item) { lista.push($(item).text()); });
                    setActiveLiToSelectedOption(id, lista.join(','));
                    $('#btnModificarDades').click();
                }
            });	  
        };
    };

    function setSelectedOptions(id, valores, selectValue) {
        $.map($('#' + id).children('option'), function (item) { $(item).removeAttr('selected').prop('selected', false); });
        var options = [];
        if (valores.indexOf(',') > -1) {
            options = valores.split(',');
        } else {
            $(options).push(valores);
        }

        if (options.length > 1) {
            //if (arregloDeOpciones.length > 1) {
            // $('option[value="1"]', $('#example-refresh')).prop('selected', true);
            $.each(options, function (index, item) {
                $('option[value="' + item + '"]', $('#' + id)).attr('selected', 'selected').prop('selected', true);
                $('option[value="' + item + '"]', $('#' + id)).attr('selected', 'selected').prop('defaultSelected', true);
                //   $('#' + id).children('option[value=' + item + ']').attr("selected", "selected");
            });
        } else {
            $('option[value="' + valores + '"]', $('#' + id)).attr('selected', 'selected').prop('selected', true);
            $('option[value="' + valores + '"]', $('#' + id)).attr('selected', 'selected').prop('defaultSelected', true);
            //  $('#' + id).children('option[value=' + valores + ']').attr("selected", "selected");
        }
        $('#' + id).multiselect('refresh');
        selectClick(id);
    };

    function readValues(valor) {

        var strAtr = "";

        strAtr = $(valor).children("span.atributs").text();

        if (!jQuery.isEmptyObject(strAtr)) {

            var arrAtr = strAtr.substring(0, strAtr.length - 1).split("#");
            $("#txtIdCel").val(arrAtr[0]);
            $("#txtNomCel").val(arrAtr[1]);

            ReadFromSpanDadesMides(arrAtr)
            //continguts
            $("#lstTipusFulla").val(function () { if (arrAtr[7] != null) { canviCampsDB(arrAtr[7]); } return arrAtr[7]; });
            //<asp:Literal runat="server" id="ltCanviCampsDb" />
            $("#ddlPLTDSCMP").val(arrAtr[8]);
            $("#ddlPLTDSLNK").val(arrAtr[9]);
            $("#ddlPLTDSALT").val(arrAtr[10]);
            $("#ddlPLTDSIMG").val(arrAtr[11]);
            ReadFromSpanDadesLlibrerias(arrAtr);		  

            $("#txtPLTDSNUM").val(arrAtr[20]);
            $("#ddlPLTDSNIV").val(arrAtr[21]);
            $("#txtPLTDSAAL").val(arrAtr[22]);
            $("#ddlb_PLTDSALF").val(arrAtr[23]);
            $("#txtPLTDSALK").val(arrAtr[24]);
            $("#gaiaPLTCDPALTxt").val(arrAtr[25]);
            $("#gaiaPLTCDPALNodes").val(arrAtr[26]);
			
			
            if (arrAtr[28] == "true") {
                $("#chkWEBDSCND").attr("checked", true);
            }
            else {
                $("#chkWEBDSCND").attr("checked", false);
            }

            ReadFromSpanDadesEstils(arrAtr);

        }
        var icona = "";
        modificaIcona(valor);

        return false;
    };

    function modificaIcona(element) {
        var icona = ""											

            if ($("#lstTipusFulla").val() != null) {
                switch ($("#lstTipusFulla").val().trim()) {
                    case "45":
                        icona = 'ico_agenda.png';
                        break;
                    case "49":
                        icona = 'ico_link.png';
                        break;
                    case "48":
                        icona = '';
                        break;
                    case "31":
                        icona = 'ico_catalegserveis.png';
                        break;
                    case "40":
                        icona = 'ico_contractacio.png';
                        break;
                    case "3":
                        icona = 'ico_directori.png';
                        break;
                    case "5":
                        icona = 'ico_document.png';
                        break;
                    case "56":
                        icona = 'ico_info.png';
                        break;
                    case "4":
                        icona = 'ico_noticia.png';
                        break;
                    case "13":
                        icona = 'ico_organigrama.png';
                        break;
                    case "55":
                        icona = 'ico_projecte.png';
                        break;
                    case "51":
                        icona = 'ico_tramit.png';
                        break;
                    case "10":
                        icona = 'ico_web.png';
                        break;
                    case "35":
                        icona = 'folder.png';
                        break;
                    case "9":
                        icona = 'node_web.png';
                        break;
                    default:
            }
        } else {			

        }
        if (icona != "") {
            $(element).find("span.divImg").html("<img src='http://lhintranet/img/common/iconografia/" + icona + "' width='20px'/>");
            
        }
    };
				
   function ReadFromSpanDadesMides(arrAtr) {
        //mides 
        $("#ddlXs").val(arrAtr[2]);
        $("#ddlSm").val(arrAtr[3]);
        $("#ddlMd").val(arrAtr[4]);
        $("#ddlLg").val(arrAtr[5]);
        $("#ddlXl").val(arrAtr[6]);
    };

    function ReadFromSpanDadesLlibrerias(arrAtr) {
        $("#gaiaCodiWebTxtAbans").val(arrAtr[12]);
        $("#gaiaCodiWebNodesAbans").val(arrAtr[13]);
        $("#gaiaCodiWebTxtDins").val(arrAtr[14]);
        $("#gaiaCodiWebNodesDins").val(arrAtr[15]);
        $("#gaiaCodiWebTxtDespres").val(arrAtr[16]);
        $("#gaiaCodiWebNodesDespres").val(arrAtr[17]);
        $("#gaiaPltSecTxt").val(arrAtr[18]);
        $("#gaiaPltSecNodes").val(arrAtr[19]);
    };
    function ReadFromSpanDadesEstils(arrAtr) {
        //estils
        
        $("#ddlb_117").val(arrAtr[44]);
        $("#ddlb_630").val(arrAtr[48]);
        setSelectedOptions('ddlb_631', arrAtr[49], 'value');
        setSelectedOptions('ddlb_632', arrAtr[50], 'value');
        $("#ddlb_633").val(arrAtr[51]);
        setSelectedOptions('ddlb_634', arrAtr[52], 'value');
        setSelectedOptions('ddlb_635', arrAtr[53], 'value');
        setSelectedOptions('ddlb_636', arrAtr[54], 'value');
        setSelectedOptions('ddlb_637', arrAtr[55], 'value');
        $("#ddlb_638").val(arrAtr[56]);
        setSelectedOptions('ddlb_639', arrAtr[57], 'value');
        setSelectedOptions('ddlb_641', arrAtr[58], 'value');
        setSelectedOptions('ddlb_642', arrAtr[59], 'value');
        setSelectedOptions('ddlb_648', arrAtr[60], 'value');
        setSelectedOptions('ddlb_649', arrAtr[61], 'value');
        setSelectedOptions('ddlb_650', arrAtr[62], 'value');
        setSelectedOptions('ddlb_651', arrAtr[63], 'value');
        $("#ddlb_652").val(arrAtr[64]);
        setSelectedOptions('ddlb_653', arrAtr[65], 'value');
        $("#ddlb_654").val(arrAtr[66]);
        $("#ddlb_655").val(arrAtr[67]);
        setSelectedOptions('ddlb_673', arrAtr[68], 'value');
        setSelectedOptions('ddlb_674', arrAtr[69], 'value');
        setSelectedOptions('ddlb_675', arrAtr[70], 'value');
      
    };	

   function activado(valor) {

        $('#htmlEst').find('*').not('span').each(function () {
            if ($(this).attr('id') == valor) {
                if ($(this).hasClass('inactivo')) {
                    $(this).removeClass('inactivo').addClass('activo');
                    $(this).find('*').not('span').each(function () { $(this).removeClass('activo').addClass('inactivo') });
                    $('select#selectContingut').val(this.tagName.toLowerCase());
                } 
            } else {
                $(this).removeClass('activo').addClass('inactivo');			 
            }
        });
            $("#txtEst").val($("#htmlEst").html());
            return false;
    }; 
					   
    function guardar() {
       
        var i = 0;
		var atributs="";
		//poso i tracto en divEst l'estructura de la p&agrave;gina que guardaré en METLPLT.PLTDSEST
		//ho tracto en divEst i ho envio a txtEst, que és el camp que arribar&agrave; a l'vb
        var elementos = $('#htmlEst').find('*').not('span').not('img');
        $(elementos).each(function (index, item) {
            if ($(item).hasClass('hr-inicial')) {
                $(item).children('span').remove();
                $(item).removeClass('hr-inicial ').removeAttr('class');
                $(item).replaceWith('<hr>');
            } else {
                activado($(item).attr('id'));
                readValues(item);

                if (i < 10) {
                    $("#txtIdCel").val('d0' + i++);

                } else {
                    $("#txtIdCel").val('d' + i++);
                }
                $(item).attr('id', $("#txtIdCel").val());
                $("#btnModificarDadesCel").click();
            }
        });

		$("#divEst").html($("#htmlEst").html());	
      
        // guardo els atributs
        $('#divEst').find('*').not('span').each(function () {
            atributs += $(this).children('span.atributs').text();             
        });

		$("#txtAtributs").val(atributs);
        $("#divEst").find("span").remove();
        $('#divEst').find('*').removeClass('b-inicial hr-inicial activo inactivo');

		$("#txtEstBD").val($("#txtEst").val()); 
		$("#txtEst").val($("#divEst").html());	

    };


</script> 

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Web" %>
<script runat="server" src="visorPlantilla_GAIA2.aspx.vb" ></script>

