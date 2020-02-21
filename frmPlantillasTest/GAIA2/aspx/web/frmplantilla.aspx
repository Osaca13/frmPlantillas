<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmplantilla.aspx.vb" ValidateRequest="false" Inherits="frmPlantillasTest.frmplantilla" Debug="true" EnableEventValidation="false" %>

<!DOCTYPE html>

<html lang="ca">
<head>
<!-- Required meta tags -->
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
<meta http-equiv="x-ua-compatible" content="ie=edge">
<!-- Bootstrap CSS -->
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css" integrity="sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO" crossorigin="anonymous">
<%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">--%>
<link rel="stylesheet" href="../../../Styles/formularisGaia.css">
<link href="img/open-iconic/font/css/open-iconic-bootstrap.css" rel="stylesheet">
<!--[if IE]>
      <link href="https://cdn.jsdelivr.net/gh/coliff/bootstrap-ie8/css/bootstrap-ie9.min.css" rel="stylesheet">
      <script src="https://cdn.jsdelivr.net/g/html5shiv@3.7.3"></script>
    <![endif]-->
    <!--[if lt IE 9]>
	  <link href="https://cdn.jsdelivr.net/gh/coliff/bootstrap-ie8/css/bootstrap-ie8.min.css" rel="stylesheet">
<![endif]-->
<title>Plantilla web - GAIA2</title>
    <style>
        .activado{
            
            background-color: rgb(227, 252, 255);
        }
        .noactivado{
            background-color: rgb(255, 255, 255);
        }

    </style>
</head>
<body>
<!-- navegacio -->
  <nav class="navbar navbar-dark bg-info navbar-expand-lg">
 
  <a class="navbar-brand" href="/home.asp"><img src="../../../img/logo_hospinet.png"></a>
  <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#desplegar" aria-controls="desplegar" aria-expanded="false" aria-label="Toggle navigation">
    <span class="navbar-toggler-icon"></span>
  </button>
  
  <div class="collapse navbar-collapse" id="desplegar">
    <ul class="navbar-nav mr-auto">
        <li class="nav-item">
        <a class="nav-link" href="/home.asp"><span class="oi oi-home mr-1" title="oi-home" aria-hidden="true"></span>Inici</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" href="http://www.l-h.cat/" target="_blank"><span class="oi oi-globe mr-1" title="oi-globe" aria-hidden="true"></span>Ajuntament on-line
          <span class="sr-only">(inici)</span>
        </a>
      </li>
      <li class="nav-item">
        <a class="nav-link" href="/html/cgis/Llistitelefonic/Listintelefonico.aspx"><span class="oi oi-book mr-1" title="oi-book" aria-hidden="true"></span>Llist&iacute; telef&ograve;nic</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" href="https://www.l-h.cat/recullpremsa" target="_blank"><span class="oi oi-rss mr-1" title="oi-rss" aria-hidden="true"></span>Recull de premsa</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" href="../../../asp/areapersonal.aspx"><span class="oi oi-person mr-1" title="oi-person" aria-hidden="true"></span>Espai personal</a> 
        <% IF Session("login") THEN
        Response.write("<img src=""/img/common/ico_desconnectar.png"">&nbsp;<a href=""/asp/areapersonal.aspx?desconectar=1""><span class='oi oi-power-standby' title='oi-power-standby' aria-hidden='true'></></span>Desconnectar</a>")
        END IF%>
      </li>
    </ul>
    <form class="form-inline my-2 my-lg-0" name="busqueda" action="/utils/cercador/cercador.aspx" method="post">
    <div class="input-group">
      <input class="form-control form-control-sm" type="text" placeholder="Cercar text...">
      <button class="btn btn-dark btn-sm" type="submit" name="Cercar" id="Cercar" onClick="document.busqueda.submit()" onKeyPress="document.busqueda.submit()">Cercar</button>
      </div>
    </form>
  </div>
</nav>
 <!-- fi navegacio -->

    <!-- container -->
    <div class="container">         
     
        <form runat="server" id="frm">
            <asp:textbox  runat="server" id="txtCodiNode" style="display: none;"/>
            <input type="text" runat="server" id="txtEst" style="display: none;" />
            <input type="hidden" runat="server" id="txtEstBD" />  
            <input type="text" runat="server" id="txtAtributs"  style="display: none;" />
            <input type="text" runat="server" id="nroId" style="display: none;" />
           
          <div class="row"><div class="col-12"><asp:Label ID="lblResultat" runat="server"/></div></div>
          
          <div id="divEst" style="visibility:hidden; position:absolute;"></div>
          <div class="row mb-3">
              <h1 class="col-6"><asp:literal runat="server" id="ltTitol"/></h1>
              <div class="form-inline col-6 justify-content-end">
              	<div class="input-group input-group-sm select">
                	<div class="input-group-prepend">
                  	<label runat="server" id="pnlcanviIdioma" for="lstCanviIdioma" class="input-group-text">Canvi d'idioma: </label>
                  	</div>
                      <asp:DropDownList ID="lstCanviIdioma" runat="server" AutoPostBack="true" OnSelectedIndexChanged="canviIdioma">
                          <asp:ListItem Value="1">Catal&agrave;</asp:ListItem>
                          <asp:ListItem Value="2">Castell&agrave;</asp:ListItem>
                          <asp:ListItem Value="3">Angl&egrave;s</asp:ListItem>
                          <asp:ListItem Value="4">Franc&egrave;s</asp:ListItem>
                      </asp:DropDownList>
                  </div>
              </div>
          </div>
          
          <ul class="nav nav-tabs" id="myTab" role="tablist">
            <li class="nav-item"><a class="nav-link active" id="propietats" data-toggle="tab" href="#arbreProps" role="tab" aria-controls="arbreProps" aria-selected="true">Propietats</a></li>
            <li class="nav-item"><a class="nav-link" id="disseny" data-toggle="tab" href="#dis" role="tab" aria-controls="dis" aria-selected="false">Disseny</a></li>
          </ul>          
          <div class="tab-content" id="myTabContent">
          	<!--*******************************************************************************************
                PROPIETATS
            *******************************************************************************************-->
            <div class="tab-pane fade show active p-3" id="arbreProps" role="tabpanel" aria-labelledby="propietats">             	           
              <asp:placeholder runat="server" id="pnlArbreWeb" visible="false">
              <div class="form-group">
                <div class="form-row mb-3"> 
                    <label for="AWEDSTIT" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">* T&iacute;tol:</label>
                    <asp:TextBox runat="server" ID="AWEDSTIT" MaxLength="60" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="T&iacute;tol"/>
                    <asp:RequiredFieldValidator ID="rqTitol" runat="server" ErrorMessage="Camp obligatori" ControlToValidate="AWEDSTIT" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </div>
                <div class="form-row mb-3">
                	<label for="AWEDSNOM" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">Nom de l'arbre:</label>
                	<asp:TextBox runat="server" ID="AWEDSNOM" MaxLength="60" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Nom de l'arbre" ></asp:TextBox>
                    <%--OnTextChanged="AWEDSNOM_TextChanged"/>--%>
                </div>
                <div class="form-row mb-3">
                	<label for="lstAWEDSSER" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-lg-right text-md-right text-sm-left">* Servidor FTP dest&iacute;:</label>
                	<asp:DropDownList id="lstAWEDSSER" runat="server" class="form-control form-control-sm col-lg-7 col-md-7 col-sm-7" placeholder="Servidor FTP dest&iacute;"/>
                    <asp:RequiredFieldValidator ID="rqServidor" runat="server" ErrorMessage="Camp obligatori" InitialValue="0" ControlToValidate="lstAWEDSSER"></asp:RequiredFieldValidator>
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
            </div>
            
            <!--*******************************************************************************************
                ESTRUCTURA DE LA p&agrave;gina
            *******************************************************************************************-->
            <div class="tab-pane fade pt-4 pb-4" id="dis" role="tabpanel" aria-labelledby="disseny">
				<div class="card text-dark mb-3">
                    <div class="card-header">
                    	<div class="row"> 
                        	<div class="col-sm-6 ">
                                <h6 class="font-weight-bold">CONTENIDORS:</h6>   
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirContenidorAbans"><img src="img/fila_abans.png" class="mr-1">Abans</button>
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirContenidorDespres"><img src="img/fila_despres.png" class="mr-1">Despr&eacute;s</button>    
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirContenidorAbansAdins"><img src="img/fila_abans.png" class="mr-1">Abans, a dins</button>
                                <button type="button" class="btn btn-primary btn-sm" id="btnAfegirContenidorDespresAdins"><img src="img/fila_despres.png" class="mr-1">Despr&eacute;s, a dins</button>
                            </div>
                            <div class="col-sm-6 ">
                                <h6 class="font-weight-bold">TIPUS CONTENIDOR:</h6>   
                                <select class="custom-select custom-select-sm" id="selectContingut" >
                                 <option></option>
                                 <option value="div">div</option>                            
                                 <option value="section">section</option>
                                 <%--<option value="column">column</option>--%>
                                 <option value="nav">nav</option>
                                 <option value="header">header</option>
                                 <option value="article">article</option>
                                 <option value="aside">aside</option>
                                 <option value="details">details</option>
                                 <option value="footer">footer</option>
                                 <option value="main">main</option>
                                 <option value="summary">summary</option>
                                </select>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6 ">                           
                            <h6 class="font-weight-bold">FILES:</h6>                            
                            <button type="button" class="btn btn-primary btn-sm" id="btnAfegirFilaDinsAbans"><img src="img/fila_abansdins.png" class="mr-1">Abans, a dins</button>
                            <button type="button" class="btn btn-primary btn-sm" id="btnAfegirFilaDinsDespres"><img src="img/fila_despresdins.png" class="mr-1">Despr&eacute;s, a dins</button>
                            </div>
                            <div class="col-sm-6">
                            <h6 class="font-weight-bold">COLUMNES:</h6>
                            <button type="button" class="btn btn-primary btn-sm" id="btnAfegirColumnaDinsAbans"><img src="img/columna_abans.png" class="mr-1">Abans, a dins</button>
                            <button type="button" class="btn btn-primary btn-sm" id="btnAfegirColumnaDinsDespres"><img src="img/columna_despres.png" class="mr-1">Despr&eacute;s, a dins</button>
                            </div>
                        </div> 
                        <div class="row">
                            <div class="col-md-3 col-sm-12">
                                <div class="form-group m-0">
                                    <h6 class="font-weight-bold" id="nomCel">NOM:</h6>                                    
                                    <input type="text" class="form-control form-control-sm m-0" id="txtNomCel" placeholder="Nom cel&middot;la"/><input type="text" id="txtIdCel" style="display: none;" value="0"/>
                                </div> 
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div id="htmlEst" class="mr-3 ml-3"><asp:literal runat="server" id="ltEst"/></div>
                    </div>
                    <hr>
                    <div class="card-body text-center pt-0">
                    	<button type="button" class="btn btn-sm btn-success" id="btnModificarDades">Modificar</button>
                        <button type="button" class="btn btn-sm btn-success" id="btnModificarDadesCel" style="display: none;;">Modificar columna</button>
                        <button type="button" class="btn btn-sm btn-danger" id="btnEsborrarCel">Esborrar</button>   
                        <button type="button" class="btn btn-sm btn-info" id="btnCopiar">Copiar estructura</button>     
                    </div>
                    <div class="card-footer">
                    	<h6 class="font-weight-bold">MIDES</h6>                        
                        <div class="row">
                            <div class="col text-center">
                                <div class="form-inline">
                                    <div class="input-group input-group-sm mr-1 mb-2">
                                        <div class="input-group-prepend"><label for="ddlXs" class="input-group-text" id="inputGroup-sizing-sm">xs:</label></div>
                                        <select id="ddlXs" class="custom-select custom-select-sm" aria-label="xs:" aria-describedby="inputGroup-sizing-sm">
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
                                        <div class="input-group-prepend"><label for="ddlSm" class="input-group-text" id="inputGroup-sizing-sm">sm:</label></div>                                
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
                                        <div class="input-group-prepend"><label for="ddlMd" class="input-group-text" id="inputGroup-sizing-sm">md:</label></div>
                                        <select  id="ddlMd" class="custom-select custom-select-sm" aria-label="md:" aria-describedby="inputGroup-sizing-sm">
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
                                        <div class="input-group-prepend"><label for="ddlLg" class="input-group-text" id="inputGroup-sizing-sm">lg:</label></div>
                                        <select  id="ddlLg" class="custom-select custom-select-sm" aria-label="lg:" aria-describedby="inputGroup-sizing-sm">
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
                                        <div class="input-group-prepend"><label for="ddlXl" class="input-group-text" id="inputGroup-sizing-sm">xl:</label></div>
                                        <select  id="ddlXl" class="custom-select custom-select-sm" aria-label="xl:" aria-describedby="inputGroup-sizing-sm">
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
                        </div>
                    </div>                           
                </div>  
                
                <div class="card bg-light mb-4">                	
                	<div class="card-header">
                    	<h6 class="font-weight-bold d-inline-flex mr-2">CONTINGUT</h6>
                        <div class="form d-inline-flex">
                            <div class="input-group input-group-sm select">                            	
                                <div class="input-group-prepend"><label for="lblTipusFulla" class="input-group-text">Tipus de fulla:</label></div>
                                <asp:literal ID="lblTipusFulla" runat="server"></asp:literal>
                            </div>
                        </div>

                      
                    </div> 
                        	
                    <asp:placeholder runat="server" id="pnlPltCamps1" visible="false">
                    <div class="card-body">
                        <div class="form-inline">
                            <div class="col-4">
                                <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlPLTDSCMP" class="input-group-text">Camp:</label></div>
                                <select name="ddlPLTDSCMP" id="ddlPLTDSCMP" class="custom-select custom-select-sm">
                                  <option value=""></option>
                                </select>
                                </div>
                                
                                <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlPLTDSLNK" class="input-group-text">Enlla&ccedil;:</label></div>            
                                <select name="ddlPLTDSLNK"  id="ddlPLTDSLNK" class="custom-select custom-select-sm">
                                  <option value=""></option>
                                </select>
                                </div>
                             </div>
                             <div class="col-4">   
                                <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlPLTDSALT" class="input-group-text">Text alternatiu:</label></div>              
                                <select name="ddlPLTDSALT"  id="ddlPLTDSALT" class="custom-select custom-select-sm">
                                  <option value=""></option>
                                </select>
                                </div>
                                
                                <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlPLTDSIMG" class="input-group-text">&Eacute;s imatge?:</label></div>               
                                <select name="ddlPLTDSIMG"  id="ddlPLTDSIMG" class="custom-select custom-select-sm">
                                  <option value="0" selected>No</option>
                                  <option value="1">Si</option>
                                </select>
                                </div>
                             </div>
                             <div class="col-4">
                                
                                <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="txtPLTDSNUM" class="input-group-text">N&ordm; elements</label></div>
                                <input name="txtPLTDSNUM" runat="server" id="txtPLTDSNUM" type="text" value="0" maxlength="3" class="form-control form-control-sm">
                                </div>
                                
                                <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlPLTDSNIV" class="input-group-text">Nivell:</label></div>
                                <asp:DropDownList ID="ddlPLTDSNIV" runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                                </div>
                             </div>
                        </div>                    	
                    </div>  
                    </asp:placeholder>
                    <asp:placeholder runat="server" id="pnlWebCamps1" visible="false">
                    <div class="card-body">
                        <div class="form-inline">
                             <div class="col-4 form-group">
                                <div class="input-group input-group-sm mb-3">
                                    <label for="chkWEBDSIMP" class="form-check-label col-form-label-sm mr-2">Imprimir cel&middot;la?:</label>
                                    <div class="form-check form-check-input"><asp:checkbox runat="server" id="chkWEBDSIMP"/></div>
                                </div>
                                <div class="input-group input-group-sm mb-3">
                                    <label for="chkWEBDSCND" class="form-check-label col-form-label-sm mr-2">Mostrar text de contingut no disponible?:</label>
                                    <div class="form-check form-check-input"><asp:checkbox runat="server" id="chkWEBDSCND"/></div>
                                </div>
                             </div>
                        </div>
                     </div>
                    </asp:placeholder>                                          
                </div>

                <asp:placeholder runat="server" id="pnlBootstrap">
                <div class="card bg-light mb-4">                	
                	<div class="card-header"><h6 class="font-weight-bold">ESTILS BOOTSTRAP</h6></div>
                    <div class="card-body">
                    	<div class="form-inline">
                        
                        <h6 class="col-12 font-weight-bold text-uppercase">Reixeta</h6>
                        
						<div class="col-4 align-self-start">
                            
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_642" class="input-group-text">Disposici&oacute;:</label></div>
                                <asp:DropDownList  ID="ddlb_642"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>
                             <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_637" class="input-group-text">Posici&oacute; i comportament:</label></div>
                                <asp:DropDownList  ID="ddlb_637"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>
                                                       
                                                 
                        </div>
                        
                        <div class="col-4 align-self-start">	
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_648" class="input-group-text">Alineaci&oacute; H.:</label></div>
                                <asp:DropDownList  ID="ddlb_648"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_636" class="input-group-text">Alineaci&oacute; V.:</label></div>
                                <asp:DropDownList  ID="ddlb_636"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>
                           
                        </div>
                        
						<div class="col-4 align-self-start">
                        	 	
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_630" class="input-group-text">Flex:</label></div>
                                <asp:DropDownList  ID="ddlb_630"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>
                            <div class="input-group input-group-sm mr-2 mb-3">
                            	<div class="input-group-prepend"><label for="ddlb_653" class="input-group-text">Alertes:</label></div>
                            	<asp:DropDownList  ID="ddlb_653"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                        	</div>
                                                      
                            
						</div>
                        
                        <h6 class="col-12 font-weight-bold text-uppercase">Texte</h6>
                        
						<div class="col-4 align-self-start">					
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_638" class="input-group-text">Transformaci&oacute;:</label></div>
                                <asp:DropDownList  ID="ddlb_638"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_633" class="input-group-text">Amplada:</label></div>
                                <asp:DropDownList  ID="ddlb_633"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>     
                            <div class="input-group input-group-sm mr-2 mb-3">
                            	<div class="input-group-prepend"><label for="ddlb_655" class="input-group-text">Familia:</label></div>
                            	<asp:DropDownList  ID="ddlb_655"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                        	</div>                          
                        </div>
                        
                        <div class="col-4 align-self-start">
                        	 <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_632" class="input-group-text">Alineaci&oacute; H.:</label></div>
                                <asp:DropDownList  ID="ddlb_632"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>                             
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_650" class="input-group-text">Alineaci&oacute; V.:</label></div>
                                <asp:DropDownList  ID="ddlb_650"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>                           
						</div>
                        
                        <div class="col-4 align-self-start">                            
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_649" class="input-group-text">Estil font:</label></div>
                                <asp:DropDownList  ID="ddlb_649"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_652" class="input-group-text">Tamany font:</label></div>
                                <asp:DropDownList  ID="ddlb_652"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>
                        </div>
                        
                        <h6 class="col-12 font-weight-bold text-uppercase">Estils comuns</h6>	
                        
                        <div class="col-4 align-self-start">  
                        	<div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_651" class="input-group-text">Voreres:</label></div>
                                <asp:DropDownList  ID="ddlb_651"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>                      
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_641" class="input-group-text">Tipus voreres:</label></div>
                                <asp:DropDownList  ID="ddlb_641"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>
                            <asp:placeholder runat="server" id="pnlEstilsDefinits">
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_117" class="input-group-text">Estils definits:</label></div>
                                <asp:DropDownList ID="ddlb_117" runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>
                            </asp:placeholder>
                         </div>
                         <div class="col-4 align-self-start">
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_639" class="input-group-text">Marge exterior:</label></div>
                                <asp:DropDownList  ID="ddlb_639"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_635" class="input-group-text">Height:</label></div>
                                <asp:DropDownList  ID="ddlb_635"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div> 
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_654" class="input-group-text">Background:</label></div>
                                <asp:DropDownList  ID="ddlb_654"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>  
                        </div>
                        <div class="col-4 align-self-start">  
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_631" class="input-group-text">Marge interior:</label></div>
                                <asp:DropDownList  ID="ddlb_631"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>  
                            <div class="input-group input-group-sm mr-2 mb-3">
                                <div class="input-group-prepend"><label for="ddlb_634" class="input-group-text">Width:</label></div>
                                <asp:DropDownList  ID="ddlb_634"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                            </div>                                                    
                        </div>
                    </div>
                    </div>
               	</div>
                </asp:placeholder>

                
                
                
                
                <div class="row">
                    <div class="col">
                        <div class="card bg-light mb-4">
                            <div class="card-header"><h6 class="font-weight-bold">LLIBRERIA (ABANS DEL CONTINGUT)</h6></div>
                            <div class="card-body">
                                <div class="form-group">
                                <div class="bs-component text-center">
                                <input type="button"  onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?arbre1=codiWeb&c=gaiaCodiWeb&separador=|&nodesSeleccionats='+document.getElementById('gaiaCodiWebNodes').value,'_blank', 'location=0,height=800,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" id="btnCodis" class="btn btn-sm btn-primary mb-2">
                                <input type="button" id="eliminarCodi" value="Esborrar" onClick="document.getElementById('gaiaCodiWebNodes').value='';document.getElementById('gaiaCodiWebTxt').value=''; return false;" class="btn btn-sm btn-danger mb-2">
                                <input type="button"  onClick="window.open('/GAIA/aspx/fulles/editaLCW.htm?nodesSeleccionats='+document.getElementById('gaiaCodiWebNodes').value,'_blank', 'location=0,height=800,width=400,scrollbars=yes,resizable=yes');return false;" value="Edita Llibreria" id="btnEditaCodis" class="btn btn-sm btn-success mb-2" >
                                </div>
                                <asp:TextBox ID="gaiaCodiWebTxt" runat="server" AutoPostBack="False"  Rows="4" ContentEditable="false" TextMode="MultiLine"  CssClass="form-control form-control-sm mb-3"/>
                                <asp:TextBox ID="gaiaCodiWebNodes" runat="server"  CssClass="form-control form-control-sm" placeholder="Codi node de la llibreria"/>
                                </div>
                             </div>
                        </div>
                    </div>
                    
                    <asp:placeholder runat="server" id="pnlLCW2">
                    <div class="col">
                        <div class="card bg-light mb-4">                
                            <div class="card-header"><h6 class="font-weight-bold">LLIBRERIA (DESPR&Eacute;S DEL CONTINGUT)</h6></div>
                            <div class="card-body">
                                <div class="form-group">
                                    <div class="bs-component text-center">
                                    <input type="button"  onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?arbre1=codiWeb&c=gaiaCodiWeb2&separador=|&nodesSeleccionats='+document.getElementById('gaiaCodiWeb2Nodes').value,'_blank', 'location=0,height=800,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" id="btnCodis2" class="btn btn-sm btn-primary mb-2">
                                    <input type="button" id="eliminarCodi2" value="Esborrar" onClick="document.getElementById('gaiaCodiWeb2Nodes').value='';document.getElementById('gaiaCodiWeb2Txt').value=''; return false;" class="btn btn-sm btn-danger mb-2">
                                    <input type="button"  onClick="window.open('/GAIA/aspx/fulles/editaLCW.htm?nodesSeleccionats='+document.getElementById('gaiaCodiWeb2Nodes').value,'_blank', 'location=0,height=800,width=400,scrollbars=yes,resizable=yes');return false;" value="Edita Llibreria" id="btnEditaCodis2" class="btn btn-sm btn-success mb-2">
                                    </div>
                                    <div class="form-group">
                                    <asp:TextBox ID="gaiaCodiWeb2Txt" runat="server" AutoPostBack="False" Rows="4" ContentEditable="false" TextMode="MultiLine" CssClass="form-control form-control-sm mb-3"/>
                                    <asp:TextBox ID="gaiaCodiWeb2Nodes" runat="server" CssClass="form-control form-control-sm" placeholder="Codi node de la llibreria"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </asp:placeholder>
                    
                    <asp:placeholder runat="server" id="pnlPlt">
                    <div class="col">
                        <div class="card bg-light mb-4">
                            <div class="card-header"><h6 class="font-weight-bold">PLANTILLA <asp:placeholder runat="server" id="pnlPltCamps2" visible="false">SECUND&Agrave;RIA</asp:placeholder></h6></div>
                            <div class="card-body">
                                <div class="form-group">
                                    <div class="bs-component text-center">
                                    <input type="button"  onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?arbre1=plantillaWeb&c=gaiaPltSec&nodesSeleccionats='+document.getElementById('gaiaPltSecNodes').value,'_blank', 'location=0,height=800,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" id="btnCodis3" class="btn btn-sm btn-primary mb-2" >
                                    <input type="button" id="btnEliminarPltSec" value="Esborrar" onClick="document.getElementById('gaiaPltSecNodes').value='';document.getElementById('gaiaPltSecTxt').value=''; return false;"  class="btn btn-sm btn-danger mb-2">
                                    <input type="button"  onClick="window.open('/GAIA/aspx/web/editaPlantilla.htm?nodesSeleccionats='+document.getElementById('gaiaPltSecNodes').value,'_blank', 'location=0,height=800,width=400,scrollbars=yes,resizable=yes');return false;" value="Edita Plantilla" id="btnEditaPlt2"  class="btn btn-sm btn-success mb-2" >
                                    </div>
                                    <asp:TextBox ID="gaiaPltSecTxt" runat="server" AutoPostBack="False" Rows="4" ContentEditable="false" TextMode="MultiLine"  CssClass="form-control form-control-sm mb-3"/>
                                    <asp:TextBox ID="gaiaPltSecNodes" runat="server" CssClass="form-control form-control-sm" placeholder="Codi node de la llibreria"/>
                                </div>
                            </div>
                        </div>
                    </div>
                    </asp:placeholder> 
                </div>

                <asp:placeholder runat="server" id="pnlGeneric">
                <div class="card bg-light mb-4">                	
                	<div class="card-header"><h6 class="font-weight-bold">ESTILS GAIA I</h6></div>
                    <div class="card-body">
                    	<div class="form-inline">
						<div class="col-4 align-self-start">
                    	<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_115" class="input-group-text">Amplades i al&ccedil;ades:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_115" runat="server"></asp:DropDownList>
                        </div>
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_119" class="input-group-text">Impressi&oacute;:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_119" runat="server"></asp:DropDownList>
                        </div>
						</div>
						<div class="col-4 align-self-start">
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_118" class="input-group-text">Fons:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_118" runat="server"></asp:DropDownList>
                        </div>
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_103" class="input-group-text">Voreres:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_103" runat="server"></asp:DropDownList>
                        </div>
						</div>
                        
                        <asp:placeholder runat="server" id="pnlMarges">
                        <div class="col-4 align-self-start">
                        <div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_28" class="input-group-text">Marges Exteriors:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_28" runat="server"></asp:DropDownList>
                        </div>
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_25" class="input-group-text">Marges Interiors:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_25" runat="server"></asp:DropDownList>
                        </div>
                        </div>
                        </asp:placeholder>
                        <asp:placeholder runat="server" id="pnlCapses">
						<div class="col-4 align-self-start">
                    	<div class="input-group input-group-sm mr-2 mb-3">
                            <div class="input-group-prepend"><label for="ddlb_114" class="input-group-text">Comportaments:</label></div>
                            <asp:DropDownList  ID="ddlb_114"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                        </div>
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_105" class="input-group-text">Fluxes:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_105" runat="server"></asp:DropDownList>
                        </div>
						</div>
						<div class="col-4 align-self-start">
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_108" class="input-group-text">Posici&oacute;:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_108" runat="server"></asp:DropDownList>
                        </div>
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_122" class="input-group-text">Disposici&oacute;:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_122" runat="server"></asp:DropDownList>
                        </div>
						</div>
						<div class="col-4 align-self-start">
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_27" class="input-group-text">Justificaci&oacute;:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_27" runat="server"></asp:DropDownList>
                        </div>
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_26" class="input-group-text">Tipus:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_26" runat="server"></asp:DropDownList>
                        </div>
						</div>
                        </asp:placeholder>
                        
                        <asp:placeholder runat="server" id="pnlEstils">
						<div class="col-4 align-self-start">
                    	<div class="input-group input-group-sm mr-2 mb-3">
                            <div class="input-group-prepend"><label for="ddlb_23" class="input-group-text">Color:</label></div>
                            <asp:DropDownList  ID="ddlb_23"  runat="server" CssClass="custom-select custom-select-sm"></asp:DropDownList>
                        </div>
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_110" class="input-group-text">Font:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_110" runat="server"></asp:DropDownList>
                        </div>
						</div>
						<div class="col-4 align-self-start">
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_111" class="input-group-text">Transformaci&oacute;:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_111" runat="server"></asp:DropDownList>
                        </div>
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_123" class="input-group-text">Justificaci&oacute;:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_123" runat="server"></asp:DropDownList>
                        </div>
						</div>
						<div class="col-4 align-self-start">
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_124" class="input-group-text">Format:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_124" runat="server"></asp:DropDownList>
                        </div>
						<div class="input-group input-group-sm mr-2 mb-3">
							<div class="input-group-prepend"><label for="ddlb_112" class="input-group-text">Decoraci&oacute;:</label></div>
							<asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_112" runat="server"></asp:DropDownList>
                        </div>
						</div>
                		</asp:placeholder>
                        
                        </div>
                    </div>
               	</div>
                </asp:placeholder>	
        
                <asp:placeholder runat="server" id="pnlPltCamps3" visible="false">
                <div class="card bg-light">                
                	<div class="card-header"><h6 class="font-weight-bold">AUTOENLLA&Ccedil;</h6></div>
                    <div class="card-body">
                    	<div class="row">      	
                            <div class="col-8">
                            	<div class="form-group">

                                    <div class="input-group input-group-sm mb-3">
                                         <div class="input-group-prepend"><label class="input-group-text">Adre&ccedil;a:</label></div>
                                        <asp:TextBox runat="server" ID="txtPLTDSAAL" MaxLength="100" CssClass="form-control form-control-sm"/>
                                    </div>


                                     <div class="input-group input-group-sm mb-3">   
                                         <div class="input-group-prepend"><label for="ddlb_PLTDSALF" class="input-group-text">Target:</label></div>                
                                        <asp:DropDownList CssClass="custom-select custom-select-sm" ID="ddlb_PLTDSALF" runat="server"/>
                                    </div>


                                    <div class="input-group input-group-sm mb-3">
                                        <label for="txtPLTDSALK" class="col-lg-3 col-md-3 col-sm-5 col-form-label-sm text-right">Text associat:</label>
                                        <asp:TextBox runat="server" ID="txtPLTDSALK" TextMode="MultiLine" Rows="4" MaxLength="500" CssClass="form-control form-control-sm"/>                            
                                    </div>
                               </div>

                            </div>                            
                            <div class="col-4">
                            	<div class="form-group">
                                    <label for="btnCodis5">Plantilla:</label>
                                    <div class="bs-component mb-3 text-center">
                                    <input name="button" type="button" id="btnCodis5" onClick="window.open('/GAIA/aspx/visorArbresLite.aspx?arbre1=plantillaWeb&c=gaiaPLTCDPAL&nodesSeleccionats='+document.getElementById('gaiaPLTCDPALNodes').value,'_blank', 'location=0,height=800,width=460,scrollbars=yes,resizable=yes');return false;" value="Seleccionar" class="btn btn-sm btn-primary">
                                    <input name="button" type="button" id="btnEliminarPLTCDPAL" onClick="document.getElementById('gaiaPLTCDPALNodes').value='';document.getElementById('gaiaPLTCDPALTxt').value=''; return false;" value="Esborrar" class="btn btn-sm btn-danger">
                                    <input type="button" class="btn btn-sm btn-success" onClick="window.open('/GAIA/aspx/web/editaPlantilla.htm?nodesSeleccionats='+document.getElementById('gaiaPLTCDPALNodes').value+'','_blank','location=0,height=800,width=600,scrollbars=yes,resizable=yes');return false;" value="Edita Plantilles" id="btnEditaPlantillesAL" >
                                    </div>
            
                                    <asp:TextBox ID="gaiaPLTCDPALTxt" runat="server" AutoPostBack="False" Rows="5" ContentEditable="false" TextMode="MultiLine" CssClass="form-control form-control-sm mb-3"/>
                                    
                                    <asp:TextBox ID="gaiaPLTCDPALNodes" runat="server" CssClass="form-control form-control-sm" placeholder="Codi node de la llibreria"/>

                            	</div>
                         	</div>
                         </div>
                    </div>       
                </div>
                </asp:placeholder>
            </div>
            
            
            <div class="tab-pane fade" id="k"></div>
          </div>           
          <asp:Label ID="lblCodi" runat="server"/>       
        </form>
		<div class="form-group">
            <div class="bs-component mb-3 text-center">
            <button type="button" class="btn btn-success" id="btnGuardar" runat="server" onClick="guardar();" onserverclick="btnGuardar_ServerClick">Guardar</button>
            </div>
        </div>
     
    </div><!-- fi container -->
    
<!-- Optional JavaScript -->    
<!-- jQuery first, then Tether, then Bootstrap JS. -->   
<%--<script src="https://code.jquery.com/jquery-3.3.1.min.js" integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=" crossorigin="anonymous"></script>--%>
<%--<script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>--%>
<%--<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>--%>
<%--<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>--%>
<script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js" integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js" integrity="sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy" crossorigin="anonymous"></script>
<script>
$(function () {
  var nua = navigator.userAgent
  var isAndroid = (nua.indexOf('Mozilla/5.0') > -1 && nua.indexOf('Android ') > -1 && nua.indexOf('AppleWebKit') > -1 && nua.indexOf('Chrome') === -1)
  if (isAndroid) {
    $('select.form-control').removeClass('form-control').css('width', '100%')
  }
})
</script>
  <!--[if IE]>
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
	<script>$('input, textarea').placeholder();</script>
		<![endif]-->   
</body>
</html>



<script>
    $(document).ready(function () {
        var element;
       
        var nroId = parseInt($("#nroId").val()); 

        $("div").on("click",".cel", function () {
            //gravo canvis en la cel·la actual
            element = $(this);
            

            //carrego dades en la cel·la nova
            var strAtr = "";
            $("div").removeClass("contenidorActiu");
            $("div").removeClass("rowActiva");
            $("div").removeClass("celActiva");

            $(this).addClass("celActiva");

            element = $(this);

            //carrego els camps d'atributs de cel·les
            strAtr = element.find("span.atributs").text();
            var arrAtr = strAtr.substring(0, strAtr.length - 1).split("#");
            
            $("#txtIdCel").val(arrAtr[0]);
            
            $("#txtNomCel").val(arrAtr[1]);
            
            DadesMides(arrAtr)
            //continguts
            $("#lstTipusFulla").val(arrAtr[7]);

            //<asp:Literal runat="server" id="ltCanviCampsDb" />

            $("#ddlPLTDSCMP").val(arrAtr[8]);
            $("#ddlPLTDSLNK").val(arrAtr[9]);
            $("#ddlPLTDSALT").val(arrAtr[10]);
            $("#ddlPLTDSIMG").val(arrAtr[11]);
            DadesLlibrerias(arrAtr);

           
            $("#txtPLTDSNUM").val(arrAtr[18]);
            $("#ddlPLTDSNIV").val(arrAtr[19]);
            $("#txtPLTDSAAL").val(arrAtr[20]);
            $("#ddlb_PLTDSALF").val(arrAtr[21]);
            $("#txtPLTDSALK").val(arrAtr[22]);
            $("#gaiaPLTCDPALTxt").val(arrAtr[23]);
            $("#gaiaPLTCDPALNodes").val(arrAtr[24]);

            if (arrAtr[25] == "true") {
                $("#chkWEBDSIMP").attr("checked", true);
            }
            else {
                $("#chkWEBDSIMP").attr("checked", false);
            }
            if (arrAtr[26] == "true") {
                $("#chkWEBDSCND").attr("checked", true);
            }
            else {
                $("#chkWEBDSCND").attr("checked", false);
            }

            DadesEstils(arrAtr);

            var icona = "";
            modificaIcona(element);

            return false;
        });

                //$("div#htmlEst").on("click", ".row", function () {

        //$('div').on('click', '.row',  function() {
        //    // gravo els cambis
        //    element = $(this);
        //    if ($(this).parents('div#htmlEst').length > 0) {
        //        element = $(this);
            
        //    //carrego dades en la cel·la nova
        //        var strAtr = "";
        //    $("div").removeClass("contenidorActiu");
        //    $("div").removeClass("rowActiva");
        //    $("div").removeClass("celActiva");

        //    $(element).addClass("rowActiva");

        //    //carrego els camps d'atributs de cel·les
        //    strAtr = element.find("span.rowAtributs").text();
        //        var arrAtr = strAtr.substring(0, strAtr.length - 1).split("#");

        //    DadesMides(arrAtr);
        //    DadesLlibrerias(arrAtr);
        //    DadesEstils(arrAtr);
        //    }
            
        //    return false;
        //});

        function DadesLlibrerias(arrAtr) {
            $("#gaiaCodiWebTxt").val(arrAtr[12]);
            $("#gaiaCodiWebNodes").val(arrAtr[13]);
            $("#gaiaCodiWeb2Txt").val(arrAtr[14]);
            $("#gaiaCodiWeb2Nodes").val(arrAtr[15]);
            $("#gaiaPltSecTxt").val(arrAtr[16]);
            $("#gaiaPltSecNodes").val(arrAtr[17]);
        }

        function DadesMides(arrAtr) {
            //mides 
            $("#ddlXs").val(arrAtr[2]);
            $("#ddlSm").val(arrAtr[3]);
            $("#ddlMd").val(arrAtr[4]);
            $("#ddlLg").val(arrAtr[5]);
            $("#ddlXl").val(arrAtr[6]);
        }

        function DadesEstils(arrAtr) {

            //estils
            $("#ddlb_23").val(arrAtr[27]);
            $("#ddlb_25").val(arrAtr[28]);
            $("#ddlb_26").val(arrAtr[29]);
            $("#ddlb_27").val(arrAtr[30]);
            $("#ddlb_28").val(arrAtr[31]);
            $("#ddlb_103").val(arrAtr[32]);
            $("#ddlb_105").val(arrAtr[33]);
            $("#ddlb_108").val(arrAtr[34]);
            $("#ddlb_110").val(arrAtr[35]);
            $("#ddlb_111").val(arrAtr[36]);
            $("#ddlb_112").val(arrAtr[37]);
            $("#ddlb_122").val(arrAtr[38]);
            $("#ddlb_114").val(arrAtr[39]);
            $("#ddlb_115").val(arrAtr[40]);
            $("#ddlb_123").val(arrAtr[41]);
            $("#ddlb_117").val(arrAtr[42]);
            $("#ddlb_118").val(arrAtr[43]);
            $("#ddlb_119").val(arrAtr[44]);
            $("#ddlb_124").val(arrAtr[45]);
            $("#ddlb_630").val(arrAtr[46]);
            $("#ddlb_631").val(arrAtr[47]);
            $("#ddlb_632").val(arrAtr[48]);
            $("#ddlb_633").val(arrAtr[49]);
            $("#ddlb_634").val(arrAtr[50]);
            $("#ddlb_635").val(arrAtr[51]);
            $("#ddlb_636").val(arrAtr[52]);
            $("#ddlb_637").val(arrAtr[53]);
            $("#ddlb_638").val(arrAtr[54]);
            $("#ddlb_639").val(arrAtr[55]);
            $("#ddlb_641").val(arrAtr[56]);
            $("#ddlb_642").val(arrAtr[57]);
            $("#ddlb_648").val(arrAtr[58]);
            $("#ddlb_649").val(arrAtr[59]);
            $("#ddlb_650").val(arrAtr[60]);
            $("#ddlb_651").val(arrAtr[61]);
            $("#ddlb_652").val(arrAtr[62]);
            $("#ddlb_653").val(arrAtr[63]);
            $("#ddlb_654").val(arrAtr[64]);
            $("#ddlb_655").val(arrAtr[65]);
        }


                //$("div#htmlEst").on("click", ".contenidor", function () {

        //$('div').on('click', '.contenidor', function () {
        //    // gravo els cambis
        //    element = $(this);
        //    if ($(this).parents('div#htmlEst').length > 0) {
        //        element = $(this).parent('div#htmlEst');

        //        $('div').removeClass("contenidorActiu");
        //        $('div').removeClass("rowActiva");
        //        $('div').removeClass("celActiva");

        //        $(element).addClass("contenidorActiu");

        //        //carrego els camps d'atributs de cel·les
        //        strAtr = element.find("span.contenidorAtributs").text();
        //        var arrAtr = strAtr.substring(0, strAtr.length - 1).split("#");

        //        DadesMides(arrAtr);
        //        DadesLlibrerias(arrAtr);
        //        DadesEstils(arrAtr);
        //    }
        //    return false;
        //});

        $("#btnAfegirContenidorAbans").click(function () {
            //gravo canvis en la cel·la actual
            //$("#btnModificarDades").click();
            
            nroId++;
            
            element = $('#htmlEst').find('*').filter(function () {                
                if (this.style.backgroundColor == 'rgb(227, 252, 255)') {
                    return this;
                }
            });
            if (!jQuery.isEmptyObject(element)) {                 
                $(element).before(Tipus(nroId));   
                $(element).css('background-color', function () { return 'rgb(255, 255, 255)';}); 
            }else{
                alert("Active un contenidor");
            };


            //if (element.length > 0) {
            //    var element = $('#htmlEst').find('.activado');
            //     $(element).after(Tipus());                        
                
            //}else{
            //    alert("Active un contenidor");
            //};


            //if (jQuery.isEmptyObject()) {
                
            // }
            //else {
            //    var elementoNuevo = $('#selectContingut').children('option:selected').val();

            //     var nombre = '<' +  elementoNuevo + ' class = "border border-primary" onclick = "activado(this)"> <span class="badge-light">' + elementoNuevo + '</span> <span class= "'+  elementoNuevo +'Atributs" style="display: none;">#################################################################|</span>' +
            //         '</' +  elementoNuevo + '>';
            //    $('section').append(nombre);
            //    $('section').css('align-items', 'center');
            //     var cambiaElemento = $('section').find(elementoNuevo);
            //     $(cambiaElemento).css('margin', '5%');
            // }         
                
            
            //guardo els canvis 
            $("#txtEst").val($("#htmlEst").html());
        });

        function RowsAndColumns(type) {
            var contenidor = ""; 
            var id = type + $('#htmlEst').find('.' + type).length; 
            switch (type) {
                case true, "row":                   
                    contenidor = '<div class= "row border p-2" id="' + id + '" style="background-color: rgb(255, 255, 255);" >row<span class= "rowAtributs" style="display: none;">#################################################################|</span></div>';                   
                    break;
                case "col":                    
                    contenidor = '<div class= "col border p-2" id="' + id + '" style="background-color: rgb(255, 255, 255);">col<span class= "colAtributs" style="display: none;">#################################################################|</span></div>';                    
                    break;            
            };           
            return contenidor;
        };

       function Tipus(nroId){
               var nombre = "";
               var id = $('#selectContingut').children('option:selected').val() + nroId;
               $('#txtNomCel').val(id);
               switch ($('#selectContingut').children('option:selected').val()) {
                    case "":
                        alert("Seleccione un elemento");
                       break;
                   case "div":
                       var nombre = '<div class = "container border p-2 pr-4 pl-4" id=' + id + ' >div <span class= "divAtributs" style="display: none;">#################################################################|</span>' +
                           '</div>';
                       break;
                    case "section":
                        var nombre = '<section class = "container border p-2 pr-4 pl-4" id='+id+' ><span class="badge-light small">section</span> <span class= "sectionAtributs" style="display: none;">#################################################################|</span>' +
                            '</section>';                       
                        break;
                    case "nav":
                        var nombre = '<nav class = "container navbar border p-2 pr-4 pl-4" id='+id+' > <span class="badge-light small">nav</span> <span class= "navAtributs" style="display: none;">#################################################################|</span></nav>';
                      
                        break;
                    case "header":
                        var nombre = '<header class = "container page-header border p-2 pr-4 pl-4" id='+id+' onclick = "activado(this.id)"> <span class="badge-light small">header</span> <span class= "headerAtributs" style="display: none;">#################################################################|</span></header>';
                      
                        break;
                    case "article":
                        var nombre = '<article class = "container col-md-4 border p-2 pr-4 pl-4" id='+id+' onclick = "activado(this.id)"> <span class="badge-light small">article</span> <span class= "articleAtributs" style="display: none;">#################################################################|</span></article>';
                      
                        break;
                    case "aside":
                        var nombre = '<aside class = "container col-md-4 border p-2  pr-4 pl-4" id='+id+' onclick = "activado(this.id)"> <span class="badge-light small">aside</span> <span class= "headerAtributs" style="display: none;">#################################################################|</span></aside>';
                   
                        break;
                    case "details":
                        var nombre = '<details class = "container border p-2  pr-4 pl-4" id='+id+' onclick = "activado(this.id)"> <span class="badge-light small">details</span> <span class= "detailsAtributs" style="display: none;">#################################################################|</span></details>';                   
                        break;
                    case "footer":
                        var nombre = '<footer class = "container border p-2 pr-4 pl-4" id='+id+' onclick = "activado(this.id)"> <span class="badge-light small">footer</span> <span class= "footerAtributs" style="display: none;">#################################################################|</span></footer>';                      
                       break;
                   case "main":
                        var nombre = '<main class = "container border p-2 pr-4 pl-4" id='+id+' onclick = "activado(this.id)"> <span class="badge-light small">main</span> <span class= "footerAtributs" style="display: none;">#################################################################|</span></main>';                      
                       break;
                   case "summary":
                        var nombre = '<summary class = "container border p-2 pr-4 pl-4" id='+id+' onclick = "activado(this.id)"> <span class="badge-light small">summary</span> <span class= "footerAtributs" style="display: none;">#################################################################|</span></summary>';                      
                        break;
                };
            return nombre;
       };


        $("#btnAfegirContenidorDespres").click(function () {
            //gravo canvis en la cel·la actual
            nroId++;
            
             //if (jQuery.isEmptyObject( $('#selectContingut').children('option:selected').val())) {
             //    alert("Seleccione un elemento");
             //}
             //else {
             //    var elementoNuevo = $('#selectContingut').children('option:selected').val();
             //    var nombre = '<' + elementoNuevo + ' class = "border border-primary" onclick = "activado(this)"><span class="badge-light">' + elementoNuevo + '</span> <span class= "'+ elementoNuevo +'Atributs" style="display: none;">#################################################################|</span>' +
             //        '</' +  elementoNuevo + '>';
             //    $('section').append(nombre);
             //    $('section').css('align-items', 'center');
             //    var cambiaElemento = $('section').find(elementoNuevo);
             //    $(cambiaElemento).css('margin', '5%');

             //}   
            element = $('#htmlEst').find('*').filter(function () {                
                if (this.style.backgroundColor == 'rgb(227, 252, 255)') {
                    return this;
                }
            });

            if (!jQuery.isEmptyObject(element)) {
                $(element).after(Tipus( nroId));                        
                $(element).css('background-color', function () { return 'rgb(255, 255, 255)';}); 
            }else{
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
                if (this.style.backgroundColor == 'rgb(227, 252, 255)') {
                    return this;
                }
            });

            if (!jQuery.isEmptyObject(element)) {
                var lastSpan = $(element).children('span').last();
                $(lastSpan).after(Tipus(nroId));                       
                $(element).css('background-color', function () { return 'rgb(255, 255, 255)';}); 
            }else{
                alert("Active un contenidor");
            };

            //if (!jQuery.isEmptyObject(element)) {
            //    nroId++;
            //    element.parent(".contenidor").prepend("<div class='row border border-secondary p-2'><span class='rowAtributs' style='display: none;'>#################################################################|</span><div class='col cel border border-secondary p-2' id='d" + nroId + "'><span class='divImg'></span><span class='text'>cel</span><span class='atributs' style='display: none;'>" + nroId + "#cel################################################################|</span></div></div>");

            //}
            //guardo els canvis 
            $("#txtEst").val($("#htmlEst").html());
        });

        $("#btnAfegirContenidorDespresAdins").click(function () {
            nroId++;
            //gravo canvis en la cel·la actual
            //$("#btnModificarDadesCel").click();

            //if (!jQuery.isEmptyObject(element)) {
            //    nroId++;
            //    element.parent(".contenidor").append("<div class='row border border-secondary p-2'><span class='rowAtributs' style='display: none;'>#################################################################|</span><div class='col cel border border-secondary p-2' id='d" + nroId + "'><span class='divImg'></span><span class='text'>cel</span><span class='atributs' style='display: none;'>" + nroId + "#cel################################################################|</span></div></div>");
            //}
            element = $('#htmlEst').find('*').filter(function () {                
                if (this.style.backgroundColor == 'rgb(227, 252, 255)') {
                    return this;
                }
            });

            if (!jQuery.isEmptyObject(element)) {
                $(element).append(Tipus(nroId));                        
                 $(element).css('background-color', function () { return 'rgb(255, 255, 255)';}); 
            }else{
                alert("Active un contenidor");
            };
            //guardo els canvis 		
            $("#txtEst").val($("#htmlEst").html());
        });

        //afegir fila en el mateix nivell
        $("#btnAfegirFilaDinsDespres").click(function () {
            nroId++;
            //gravo canvis en la cel·la actual
            //$("#btnModificarDades").click();

            //if (!jQuery.isEmptyObject(element)) {
            //    nroId++;
            //    var tmp = element.wrap("<p/>").parent().html().trim();
            //    element.parent().html("<div class='col'><div class='row border border-secondary p-2'><span class='rowAtributs' style='display: none;'>#################################################################|</span>" + tmp + "</div><div class='row border border-secondary p-2'><span class='rowAtributs' style='display: none;'>#################################################################|</span><div class='col cel border border-secondary p-2' id='d" + nroId + "'><span class='divImg'></span><span class='text'>cel</span><span class='atributs' style='display: none;'>" + nroId + "#cel################################################################|</span></div></div></div>")
            //    $("#htmlEst p").replaceWith(function () { return $(this).contents(); });
            //}
           element = $('#htmlEst').find('*').filter(function () {                
                if (this.style.backgroundColor == 'rgb(227, 252, 255)') {
                   return this;
                }
            });

            if (!jQuery.isEmptyObject(element)) {                
                var elementToInsert = RowsAndColumns('row');
                $(element).append(elementToInsert); 
                $(element).css('background-color', function () { return 'rgb(255, 255, 255)';});                              
            }else{
                alert("Active un contenidor");
            };
            //guardo els canvis 		
            $("#txtEst").val($("#htmlEst").html());            
        });

        //afegir fila en el mateix nivell
        $("#btnAfegirFilaDinsAbans").click(function () {
            nroId++;
            //gravo canvis en la cel·la actual
            //$("#btnModificarDades").click();

            //if (!jQuery.isEmptyObject(element)) {
            //    nroId++;
            //    var tmp = element.wrap("<p/>").parent().html().trim();
            //    element.parent().html("<div class='col'><div class='row border border-secondary p-2'><span class='rowAtributs' style='display: none;'>#################################################################|</span><div class='col cel border border-secondary p-2' id='d" + nroId + "'><span class='divImg'></span><span class='text'>cel</span><span class='atributs' style='display: none;'>" + nroId + "#cel##########################|</span></div></div><div class='row border border-secondary p-2'><span class='rowAtributs' style='display: none;'>#################################################################|</span>" + tmp + "</div></div>")
            //    $("#htmlEst p").replaceWith(function () { return $(this).contents(); });
            //}
            element = $('#htmlEst').find('*').filter(function () {                
                if (this.style.backgroundColor == 'rgb(227, 252, 255)') {
                    return this;
                }
            });
              
            if (!jQuery.isEmptyObject(element)) {                
                var elementToInsert = RowsAndColumns('row');
                var lastSpan = $(element).children('span').last();
                $(lastSpan).after(elementToInsert); 
              
                $(element).css('background-color', function () { return 'rgb(255, 255, 255)';});                              
            }else{
                alert("Active un contenidor");
            };
            //guardo els canvis 		
            $("#txtEst").val($("#htmlEst").html());
            //element = $(".celActiva")
        });

        $("#btnAfegirColumnaDinsAbans").click(function () {
            nroId++;
            //gravo canvis en la cel·la actual
            //$("#btnModificarDades").click();

            //if (!jQuery.isEmptyObject(element)) {
            //    nroId++;
            //    if (element.attr("class").indexOf('col') > -1) {
            //        element.before("<div class='col cel border border-secondary p-2' id='d" + nroId + "'><span class='divImg'></span><span class='text'>cel</span><span class='atributs' style='display: none;'>" + nroId + "#cel################################################################|</span></div>");
            //    }
            //    else {
            //        $("#lblResultat").html("<div class='alert alert-dismissible alert-success mt-2 mb-2'><button type='button' class='close' data-dismiss='alert'>x</button>Per inserir columnes cal seleccionar una columna</div>");
            //    }
            //}

            element = $('#htmlEst').find('*').filter(function () {                
                if (this.style.backgroundColor == 'rgb(227, 252, 255)') {
                    return this;
                }
            });

            if (!jQuery.isEmptyObject(element)) {  
                var elementToInsert = RowsAndColumns('col');
                if (element.hasClass('col')) {
                    $(element).before(elementToInsert);                    
                } else {
                    if (element.hasClass('row')) {
                        var lastSpan = $(element).children('span').last();
                        $(lastSpan).after(elementToInsert);
                    } else {
                        alert('primero inserte una fila');
                    }
                }
                $(element).css('background-color', function () { return 'rgb(255, 255, 255)'; });                                    
            }else{
                alert("Active un contenidor");
            };

            //if ($('#htmlEst').find('.activado').length > 0) {
            //     var element = $('#htmlEst').find('.activado');
            //     var elementToInsert = RowsAndColumns(element, 'col');
            //     $(element).before(elementToInsert);                        
                
            //}else{
            //    alert("Active un contenidor");
            //};
            //guardo els canvis 
            $("#txtEst").val($("#htmlEst").html());
        })


        $("#btnAfegirColumnaDinsDespres").click(function () {
            nroId++;
            //gravo canvis en la cel·la actual
            //$("#btnModificarDades").click();

            //if (!jQuery.isEmptyObject(element)) {
            //    nroId++;
            //    if (element.attr("class").indexOf('col') > -1) {
            //        element.after("<div class='col cel border border-secondary p-2' id='d" + nroId + "'><span class='divImg'></span><span class='text'>cel</span><span class='atributs' style='display: none;'>" + nroId + "#cel################################################################|</span></div>");
            //    }
            //    else {
            //        $("#lblResultat").html("<div class='alert alert-dismissible alert-success mt-2 mb-2'><button type='button' class='close' data-dismiss='alert'>x</button>Per inserir columnes cal seleccionar una columna</div>");
            //    }
            //}
            element = $('#htmlEst').find('*').filter(function () {                
                if (this.style.backgroundColor == 'rgb(227, 252, 255)') {
                    return this;
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
                $(element).css('background-color', function () { return 'rgb(255, 255, 255)';});                              
            }else{
                alert("Active un contenidor");
            };
            // if ($('#htmlEst').find('.activado').length > 0) {
            //     var element = $('#htmlEst').find('.activado');
            //     var elementToInsert = RowsAndColumns(element, 'col');
            //     $(element).after(elementToInsert);                        
                
            //}else{
            //    alert("Active un contenidor");
            //};
            //guardo els canvis 
            $("#txtEst").val($("#htmlEst").html());
        })

        $("#btnEsborrarCel").click(function () {
            nroId++;
            //gravo canvis en la cel·la actual
           // $("#btnModificarDadesCel").click();

            //if (!jQuery.isEmptyObject(element)) {
            //    if (!jQuery.isEmptyObject(element.siblings("div").html())) {
            //        element.remove();
            //    }
            //    else {
            //        element.parent().remove();
            //    }
            //}
            element = $('#htmlEst').find('*').filter(function () {                
                if (this.style.backgroundColor == 'rgb(227, 252, 255)') {
                    return this;
                }
            });
            if (!jQuery.isEmptyObject(element)) {                
               
                $(element).remove();                          
            }else{
                alert("No hay elemento activado");
            };

            //if ($('#htmlEst').find('.activado').length > 0) {
            //    var element = $('#htmlEst').find('.activado');
            //    $(element).remove();                      
                
            //}else{
            //    alert("Active un contenidor");
            //};

            //guardo els canvis 
            $("#txtEst").val($("#htmlEst").html());
        })

        $("#btnCopiar").click(function () {

            $("#divEst").html($("#htmlEst").html());

            //// limpio la estructura
            $("#divEst").find("span.divImg").remove();
            $("#divEst").find("span.atributs").remove();
            $("#divEst").find("span.rowAtributs").remove();
            $("#divEst").find("span.contenidorAtributs").remove();
            $("#divEst").find("div").removeAttr("style");
            $("#divEst").find("div").removeClass("cel celActiva p-2 border border-secondary rowActiva contenidorActiu contenidor");

            $("#txtEstBD").val($("#divEst").html());

            var copyText = $("#txtEstBD");

            /* Select the text field */
            copyText.select();

            /* Copy the text inside the text field */
            document.execCommand("copy");

        })
              

        $("#btnModificarDadesCel").click(function () {

            var strAtr = "";
            
            //var contadorMides = 0;

            if (!jQuery.isEmptyObject(element)) {
                element.find("span.text").text($("#txtNomCel").val());
                strAtr += $("#txtIdCel").val() + "#" + $("#txtNomCel").val(); //1#2
                //elimino les classes de mida de cel.la
                //element.removeClass(function (index, className) { return (className.match (/(^|\s)col-\S+/g) || []).join(' ');});
                element.removeClass();
                element.addClass("cel celActiva border border-secondary p-2");
                //afegeixo les classes de bootstrap

                strAtr = DadesComunsMides(strAtr, element);

                //if (contadorMides == 0) {
                    element.addClass("col");
                //}

                strAtr += "#";
                if ($("#lstTipusFulla").val() != null) {
                    strAtr += $("#lstTipusFulla").val().trim(); //8
                }
                
                strAtr += "#";
                if ($("#ddlPLTDSCMP").val() != null) {
                    strAtr += $("#ddlPLTDSCMP").val().trim(); //9
                }
                strAtr += "#";
                if ($("#ddlPLTDSLNK").val() != null) {
                    strAtr += $("#ddlPLTDSLNK").val().trim(); //10
                }
                strAtr += "#";
                if ($("#ddlPLTDSALT").val() != null) {
                    strAtr += $("#ddlPLTDSALT").val().trim(); //11
                }

                strAtr += "#";
                if ($("#ddlPLTDSIMG").val() != null) {
                    strAtr += $("#ddlPLTDSIMG").val().trim(); //12
                }

                strAtr = DadesComunsLlibrerias(strAtr);

               

                strAtr += "#"
                if ($("#txtPLTDSNUM").val() != null) {
                    strAtr += $("#txtPLTDSNUM").val().trim(); //19
                }

                strAtr += "#";
                if ($("#ddlPLTDSNIV").val() != null) {
                    strAtr += $("#ddlPLTDSNIV").val().trim(); //20
                }

                strAtr += "#";
                if ($("#txtPLTDSAAL").val() != null) {
                    strAtr += $("#txtPLTDSAAL").val().trim(); //21
                }

                strAtr += "#";
                if ($("#ddlb_PLTDSALF").val() != null) {
                    strAtr += $("#ddlb_PLTDSALF").val().trim(); //22
                }


                strAtr += "#";
                if ($("#txtPLTDSALK").val() != null) {
                    strAtr += $("#txtPLTDSALK").val().trim(); //23
                }

                strAtr += "#";
                if ($("#gaiaPLTCDPALTxt").val() != null) {
                    strAtr += $("#gaiaPLTCDPALTxt").val().trim(); //24
                }

                strAtr += "#";
                if ($("#gaiaPLTCDPALNodes").val() != null) {
                    strAtr += $("#gaiaPLTCDPALNodes").val().trim(); //25
                }

                strAtr += "#";
                strAtr += $("#chkWEBDSIMP").is(":checked"); //26

                strAtr += "#";
                strAtr += $("#chkWEBDSCND").is(":checked"); //27

                strAtr = DadesComunsEstils(strAtr, element);

                modificaIcona(element);
                //afegeixo els atributs a l'array de propietats
                element.find("span.atributs").text(strAtr);

                //guardo els canvis 
                $("#txtEst").val($("#htmlEst").html());
            }
        })

        $("#btnModificarDades").click(function () {

            var strAtr = "";

            if (!jQuery.isEmptyObject(element)) {

                if (element.hasClass('cel')) {   // element: el ultimo div que se ha clicado
                    $("#btnModificarDadesCel").click();
                    return true;
                }
                else if (element.hasClass('row')) {
                    ModificarDadesRow();

                }
                 //else if (element.attr("class").indexOf('contenidor') > -1) {

                else if (element.hasClass('contenidor')) {
                    ModificarDadesContenidor();
                }

               //guardo els canvis 
                $("#txtEst").val($("#htmlEst").html());
            }
        });

        function ModificarDadesContenidor() {
            
            element.removeClass();
            element.addClass("contenidor contenidorActiu border border-secondary p-2 pr-4 pl-4");
            element.find("span.contenidorAtributs").text("");

            var strAtr = ComposicionTextoAtributos();

            element.find("span.contenidorAtributs").text(strAtr);
            //guardo els canvis 
            $("#txtEst").val($("#htmlEst").html());

        }

        function ModificarDadesRow() {
            element.removeClass();
            element.addClass("row rowActiva border border-secondary p-2");
            element.find("span.rowAtributs").text("");

            var strAtr = ComposicionTextoAtributos();

            element.find("span.rowAtributs").text(strAtr);
            //guardo els canvis 
           $("#txtEst").val($("#htmlEst").html());

        }

        function ComposicionTextoAtributos(){
            var strAtr = "#";
            strAtr = DadesComunsMides(strAtr, element);
            strAtr += "#####";
            strAtr = DadesComunsLlibrerias(strAtr);
            strAtr +="#########"
            strAtr = DadesComunsEstils(strAtr, element);

            return strAtr;
        }

        function DadesComunsLlibrerias(strAtr) {

            strAtr += "#";
            if ($("#gaiaCodiWebTxt").val() != null) {
                strAtr += $("#gaiaCodiWebTxt").val().trim();//13
            }
            strAtr += "#";
            if ($("#gaiaCodiWebNodes").val() != null) {
                strAtr += $("#gaiaCodiWebNodes").val().trim();//14
            }
            strAtr += "#";
            if ($("#gaiaCodiWeb2Txt").val() != null) {
                strAtr += $("#gaiaCodiWeb2Txt").val().trim();//15
            }
            strAtr += "#";
            if ($("#gaiaCodiWeb2Nodes").val() != null) {
                strAtr += $("#gaiaCodiWeb2Nodes").val().trim();//16
            }
             strAtr += "#";
            if ($("#gaiaPltSecTxt").val() != null) {
               strAtr += $("#gaiaPltSecTxt").val().trim(); //17
            }
            strAtr += "#";
            if ($("#gaiaPltSecNodes").val() != null) {
               strAtr += $("#gaiaPltSecNodes").val().trim(); //18
            }
            return strAtr;
        }

        function DadesComunsMides(strAtr, element) {

            strAtr += "#";
            if ($("#ddlXs").val() != '') {
            //    contadorMides++;
                if ($("#ddlXs").val() != 'Tot') {
                    element.addClass("col-xs-" + $("#ddlXs").val());
                    strAtr += $("#ddlXs").val()  //#3
                }
                else {
                    element.addClass("col-xs");
                    strAtr += "0";

                }
            }
            strAtr += "#";
            if ($("#ddlSm").val() != '') {
             //   contadorMides++;
                if ($("#ddlSm").val() != 'Tot') {
                    element.addClass("col-sm-" + $("#ddlSm").val());
                    strAtr += $("#ddlSm").val() //4
                }
                else {
                    element.addClass("col-sm");
                    strAtr += "0";
                }
            }
            strAtr += "#";
            if ($("#ddlMd").val() != '') {
              //  contadorMides++;
                if ($("#ddlMd").val() != 'Tot') {
                    element.addClass("col-md-" + $("#ddlMd").val());
                    strAtr += $("#ddlMd").val() //5
                }
                else {
                    element.addClass("col-md");
                    strAtr += "0";
                }
            }
            strAtr += "#";
            if ($("#ddlLg").val() != '') {
              //  contadorMides++;
                if ($("#ddlLg").val() != 'Tot') {
                    element.addClass("col-lg-" + $("#ddlLg").val());
                    strAtr += $("#ddlLg").val() //6
                }
                else {
                    element.addClass("col-lg");
                    strAtr += "0";
                }
            }
            strAtr += "#";
            if ($("#ddlXl").val() != '') {
              //  contadorMides++;
                if ($("#ddlXl").val() != 'Tot') {
                    element.addClass("col-xl-" + $("#ddlXl").val());
                    strAtr += $("#ddlXl").val() //7
                }
                else {
                    element.addClass("col-xl");
                    strAtr += "0";
                }
            }
            return strAtr;
        }


        function DadesComunsEstils(strAtr, element) {

            strAtr += "#";
            if ($("#ddlb_23").val() != null) {
                strAtr += $("#ddlb_23").val();
                element.addClass($("#ddlb_23 option:selected").text());//28
            }

            strAtr += "#";
            if ($("#ddlb_25").val() != null) {
                strAtr += $("#ddlb_25").val();
                element.addClass($("#ddlb_25 option:selected").text());//29
            }

            strAtr += "#";
            if ($("#ddlb_26").val() != null) {
                strAtr += $("#ddlb_26").val();
                element.addClass($("#ddlb_26 option:selected").text());//30
            }

            strAtr += "#";
            if ($("#ddlb_27").val() != null) {
                strAtr += $("#ddlb_27").val();
                element.addClass($("#ddlb_27 option:selected").text());//31
            }

            strAtr += "#";
            if ($("#ddlb_28").val() != null) {
                strAtr += $("#ddlb_28").val();
                element.addClass($("#ddlb_28 option:selected").text());//32
            }
            strAtr += "#";
            if ($("#ddlb_103").val() != null) {
                strAtr += $("#ddlb_103").val();
                element.addClass($("#ddlb_103 option:selected").text());//33
            }

            strAtr += "#";
            if ($("#ddlb_105").val() != null) {
                strAtr += $("#ddlb_105").val();
                element.addClass($("#ddlb_105 option:selected").text());//34
            }

            strAtr += "#";
            if ($("#ddlb_108").val() != null) {
                strAtr += $("#ddlb_108").val();
                element.addClass($("#ddlb_108 option:selected").text());//35
            }

            strAtr += "#";
            if ($("#ddlb_110").val() != null) {
                strAtr += $("#ddlb_110").val();
                element.addClass($("#ddlb_110 option:selected").text());//36
            }

            strAtr += "#";
            if ($("#ddlb_111").val() != null) {
                strAtr += $("#ddlb_111").val();
                element.addClass($("#ddlb_111 option:selected").text());//37
            }
            strAtr += "#";
            if ($("#ddlb_112").val() != null) {
                strAtr += $("#ddlb_112").val();
                element.addClass($("#ddlb_112 option:selected").text());//38
            }

            strAtr += "#";
            if ($("#ddlb_122").val() != null) {
                strAtr += $("#ddlb_122").val();
                element.addClass($("#ddlb_122 option:selected").text());//39
            }

            strAtr += "#";
            if ($("#ddlb_114").val() != null) {
                strAtr += $("#ddlb_114").val();
                element.addClass($("#ddlb_114 option:selected").text());//40
            }

            strAtr += "#";
            if ($("#ddlb_115").val() != null) {
                strAtr += $("#ddlb_115").val();
                element.addClass($("#ddlb_115 option:selected").text());//41
            }

            strAtr += "#";
            if ($("#ddlb_123").val() != null) {
                strAtr += $("#ddlb_123").val();
                element.addClass($("#ddlb_123 option:selected").text());//42
            }
            strAtr += "#";
            if ($("#ddlb_117").val() != null) {
                strAtr += $("#ddlb_117").val();
                element.addClass($("#ddlb_117 option:selected").text());//43
            }

            strAtr += "#";
            if ($("#ddlb_118").val() != null) {
                strAtr += $("#ddlb_118").val();
                element.addClass($("#ddlb_118 option:selected").text());//44
            }

            strAtr += "#";
            if ($("#ddlb_119").val() != null) {
                strAtr += $("#ddlb_119").val();
                element.addClass($("#ddlb_119 option:selected").text());//45
            }

            strAtr += "#";
            if ($("#ddlb_124").val() != null) {
                strAtr += $("#ddlb_124").val();
                element.addClass($("#ddlb_124 option:selected").text());//46
            }

            strAtr += "#";
            if ($("#ddlb_630").val() != null) {
                strAtr += $("#ddlb_630").val();
                element.addClass($("#ddlb_630 option:selected").text());//47
            }

            strAtr += "#";
            if ($("#ddlb_631").val() != null) {
                strAtr += $("#ddlb_631").val();
                element.addClass($("#ddlb_631 option:selected").text());//48
            }

            strAtr += "#";
            if ($("#ddlb_632").val() != null) {
                strAtr += $("#ddlb_632").val();
                element.addClass($("#ddlb_632 option:selected").text());//49
            }

            strAtr += "#";
            if ($("#ddlb_633").val() != null) {
                strAtr += $("#ddlb_633").val();
                element.addClass($("#ddlb_633 option:selected").text());//50
            }

            strAtr += "#";
            if ($("#ddlb_634").val() != null) {
                strAtr += $("#ddlb_634").val();
                element.addClass($("#ddlb_634 option:selected").text());//51
            }

            strAtr += "#";
            if ($("#ddlb_635").val() != null) {
                strAtr += $("#ddlb_635").val();
                element.addClass($("#ddlb_635 option:selected").text());//52
            }

            strAtr += "#";
            if ($("#ddlb_636").val() != null) {
                strAtr += $("#ddlb_636").val();
                element.addClass($("#ddlb_636 option:selected").text());//53
            }

            strAtr += "#";
            if ($("#ddlb_637").val() != null) {
                strAtr += $("#ddlb_637").val();
                element.addClass($("#ddlb_637 option:selected").text());//54
            }

            strAtr += "#";
            if ($("#ddlb_638").val() != null) {
                strAtr += $("#ddlb_638").val();
                element.addClass($("#ddlb_638 option:selected").text());//55
            }

            strAtr += "#";
            if ($("#ddlb_639").val() != null) {
                strAtr += $("#ddlb_639").val();
                element.addClass($("#ddlb_639 option:selected").text());//56
            }

            strAtr += "#";
            if ($("#ddlb_641").val() != null) {
                strAtr += $("#ddlb_641").val();
                element.addClass($("#ddlb_641 option:selected").text());//57
            }

            strAtr += "#";
            if ($("#ddlb_642").val() != null) {
                strAtr += $("#ddlb_642").val();
                element.addClass($("#ddlb_642 option:selected").text());//58
            }

            strAtr += "#";
            if ($("#ddlb_648").val() != null) {
                strAtr += $("#ddlb_648").val();
                element.addClass($("#ddlb_648 option:selected").text());//59
            }

            strAtr += "#";
            if ($("#ddlb_649").val() != null) {
                strAtr += $("#ddlb_649").val();
                element.addClass($("#ddlb_649 option:selected").text());//60

            }

            strAtr += "#";
            if ($("#ddlb_650").val() != null) {
                strAtr += $("#ddlb_650").val();
                element.addClass($("#ddlb_650 option:selected").text());//61
            }

            strAtr += "#";
            if ($("#ddlb_651").val() != null) {
                strAtr += $("#ddlb_651").val();
                element.addClass($("#ddlb_651 option:selected").text());//62
            }

            strAtr += "#";
            if ($("#ddlb_652").val() != null) {
                strAtr += $("#ddlb_652").val();
                element.addClass($("#ddlb_652 option:selected").text());//63
            }

            strAtr += "#";
            if ($("#ddlb_653").val() != null) {
                strAtr += $("#ddlb_653").val();
                element.addClass($("#ddlb_653 option:selected").text());//64
            }

            strAtr += "#";
            if ($("#ddlb_654").val() != null) {
                strAtr += $("#ddlb_654").val();
                element.addClass($("#ddlb_654 option:selected").text());//65
            }

            strAtr += "#";
            if ($("#ddlb_655").val() != null) {
                strAtr += $("#ddlb_655").val();
                element.addClass($("#ddlb_655 option:selected").text());//66
            }

            strAtr += "|"
            return strAtr;

        }


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
            }
            if (icona != "") {
                element.find("span.divImg").html("<img src='img/" + icona + "'/>");
            }
        };    

        document.addEventListener('click', function (event) {
            element = event.toElement;           
            if ($(element).parents('#htmlEst').length > 0) {
                activado($(element).attr('id'));
                event.stopPropagation();
                 //$(hijos).css('background-color', 'rgb(255, 255, 255)');
                var hijos = $(element).find('.row', '.col');
                $(hijos).css('background-color', 'rgb(255, 255, 255)');
            };           
        }, true);
    });

    function activado(element) {
         
        $('#htmlEst').find('*').each(function () {
            if ($(this).attr('id') == element) {
                if ( $(this).css('background-color') == 'rgb(255, 255, 255)') {
                    this.style.backgroundColor ='rgb(227, 252, 255)';
                } else {
                    this.style.backgroundColor ='rgb(255, 2555, 255)'; 
                }                
                $('#selectContingut').children('option').find(this.tagName);
            } else {
                this.style.backgroundColor = 'rgb(255, 255, 255)';  
            }
        }); 
          return false;
     };   

    function guardar() {
       
		//gravo canvis en la cel·la actual
		$("#btnModificarDades").click();

		var atributs="";
		//poso i tracto en divEst l'estructura de la p&agrave;gina que guardaré en METLPLT.PLTDSEST
		//ho tracto en divEst i ho envio a txtEst, que és el camp que arribar&agrave; a l'vb
		$("#divEst").html($("#htmlEst").html());	
      
        // guardo els atributs
            var div = $("#divEst").find('div.contenidor');

            $.each(div, function (index, value) {
                atributs += 'Contenidor:';
                console.log(index + ": " + value);
                atributs += $(this).find("span.contenidorAtributs").html();
            
                var fila = $(this).children('div.row');

                $.each(fila, function (index, value) {
                console.log( index + ": " + value );
                atributs += 'Row:';
                atributs += $(this).find("span.rowAtributs").html();

                $(this).children('div.cel').each(function (index, value) {
                    console.log( index + ": " + value );
                    atributs += 'Cel:';
                    atributs += $(this).find("span.atributs").html();
		        });	            
		    });	            
        });	
       
			
		$("#txtAtributs").val(atributs);

		$("#divEst").find("span").remove();
		$("#divEst").find("div").removeAttr("style");
		$("#divEst").find("div").removeClass("cel celActiva p-2 pr-4 pl-4 border border-secondary rowActiva contenidor contenidorActiu");

		$("#txtEstBD").val($("#txtEst").val()); 

		$("#txtEst").val($("#divEst").html());
		
		// $("#frm").submit();  Esto es lo que provocava que se insertasen 2 registros cada vez.

	};	

</script> 

