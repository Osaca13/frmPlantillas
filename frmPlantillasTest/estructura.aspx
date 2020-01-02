<identity impersonate="true"/>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="estructura.aspx.vb" Inherits="frmPlantillasTest.estructura" ValidateRequest="false" Debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="ca">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
   <!-- Required meta tags -->
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
<meta http-equiv="x-ua-compatible" content="ie=edge">
<!-- Bootstrap CSS -->
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
<link rel="stylesheet" href="../../css/formularisGaia.css">
<link href="img/open-iconic/font/css/open-iconic-bootstrap.css" rel="stylesheet">
<!--[if IE]>
      <link href="https://cdn.jsdelivr.net/gh/coliff/bootstrap-ie8/css/bootstrap-ie9.min.css" rel="stylesheet">
      <script src="https://cdn.jsdelivr.net/g/html5shiv@3.7.3"></script>
    <![endif]-->
<!--[if lt IE 9]>
	  <link href="https://cdn.jsdelivr.net/gh/coliff/bootstrap-ie8/css/bootstrap-ie8.min.css" rel="stylesheet">
<![endif]-->
<title>Estructura web</title>
</head>
       
<body onLoad="document.getElementById('divMain').style.visibility='visible';
    document.getElementById('divWait').style.visibility='hidden';">

<div id="divWait" style="visibility:hidden; vertical-align:middle; width:100%; margin:0 auto; color:#990000; height:100%; position:absolute; background-color:#fff; top:150px;">
  <center>
    <img src="/img/reloj2.gif"  alt="Treballant">
  </center>
</div>
<div class="container-fluid" id="divMain" style="visibility:visible">
  <div class="row">
    <div class="col">
      <form id="frm" runat="server">
          <asp:textbox id="WEBDSTCO" width=0 runat="server"   style="visibility:hidden"></asp:textbox>
          <asp:textbox id="tipusNode" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="ultimaDivisio" width=0 runat="server" style="visibility:hidden" ></asp:textbox>
          <asp:textbox id="ultimaPlantilla" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="nroArbreOrigen" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="nroArbreDesti" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="nroNodeOrigen" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="nroNodeDesti" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="nodePathVell" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="nodePathNou" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="nroNodePareAnterior" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="codiRelacioOrigen" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="txtposicioEstructura"  width=0 runat="server"  style="visibility:hidden" ></asp:textbox>
          <asp:textbox id="moureFills" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="codiRelacioDesti" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="txtDragDrop" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="txtPosicioEstructuraReal"  width=0  runat="server" style="visibility:hidden"/>
          <asp:textbox id="codiRelacioDestiInicial" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="llistaPlantilles" width=0 runat="server" style="visibility:hidden"></asp:textbox>
          <asp:textbox id="nomTaula" width=0 runat="server"  style="visibility:hidden"></asp:textbox>
          <asp:textbox id="ubicacionsSeleccionables"  width=0 value="0" runat="server"   style="visibility:hidden" ></asp:textbox>   
          
          
          
          <asp:panel runat="server" visible="false" id="pnlError">
            <div class="alert alert-danger">
              <asp:literal runat="server" id="ltErr"></asp:literal>
            </div>
           </asp:panel>

          
          
          	<asp:panel runat="server" visible="true" id="pnlEditar">
            <div class="card mb-3">
            <% IF lblEstructura.text.length>0 THEN %>
            <script>
                top.resizeTo(1200, 900);
            </script> 
            
              <div class="card-header">
                <h6 class="card-title"><asp:Label ID="lblTitol" runat="server" CssClass="font-weight-bold"></asp:Label>. Selecciona l'ubicaci&oacute; del contingut</h6>
              </div>
              <div class="card-body">
              	<div class="container-fluid">
                <asp:literal ID="lblEstructura" runat="server"></asp:literal>
                </div>
              </div>                        
              <% END IF%>
              <div class="card-footer">
                <div class="form-group">
                    <div class="row">
                        <legend class="col-form-label-sm col-4 pt-0 text-right">Quin tipus de moviment desitges fer?:</legend>
                        <div class="col">
                            <asp:RadioButtonList ID="accio" runat="server" CssClass="form-check form-control-sm" OnSelectedIndexChanged="canviarNodeDesti" AutoPostBack="true">
                              <asp:ListItem  runat="server" Text="A dins del node seleccionat" Value="insertar" selected />
                              <asp:ListItem  runat="server" Text="Al mateix nivell del node seleccionat" Value="moure"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div> 
                <div class="form-group row">
                    <label class="col-4 col-form-label-sm text-right" for="lstOrdre">Moure a la posici&oacute;:</label>
                    <div class="col">
                         <asp:ListBox ID="lstOrdre" runat="server" Rows="1" CssClass="form-control form-control-sm">
                          <asp:ListItem Value="0">&Uacute;ltim de la llista</asp:ListItem>
                          <asp:ListItem Value="1">Primer de la llista</asp:ListItem>
                          <asp:ListItem Value="3">Ordre alfab&egrave;tic</asp:ListItem>
                          <asp:ListItem Value="2" >A sota de la posici&oacute; seleccionada</asp:ListItem>
                        </asp:ListBox>
                    </div>
                </div>
                <div class="form-group row">
                    <label class="col-4 col-form-label-sm text-right" for="plantillesPH" id="trPlantilla1" style="display:none">Plantilla a utilizar:</label>	
                    <div class="col select" id="trPlantilla2" style="display:none">
                         <asp:PlaceHolder runat="server" id="plantillesPH"/>
                    </div>
                </div>
              </div>
            </div>
              
            <div class="row"> 
                <div id="tbDates" visible="false" runat="server" class="col">
                    <div class="card bg-light mb-3">
                        <div class="card-header">
                          <h6 class="card-title font-weight-bold">Publicaci&oacute; / Caducitat</h6>
                        </div>
                        <div runat="server" id="tbDates_cat" class="card-body" visible="false">
                          <asp:RequiredFieldValidator ID="REIDTPUBReqVal"
                                                        ControlToValidate="REIDTPUB"
                                                        enabled="false"
                                                        ErrorMessage="Cal indicar la data de publicaci&oacute;"
                                                        EnableClientScript="true"
                                                        Display="None"
                                                        InitialValue="" runat=server></asp:RequiredFieldValidator>
                          <asp:CompareValidator ID="REIDTCADCompareVal" runat="server" Display="None"
                                                        ErrorMessage="La data de caducitat ha de ser posterior a la data de publicaci&oacute;" 
                                                        EnableClientScript="true"
                                                        enabled="false"
                                                        ControlToValidate="REIDTCAD" ControlToCompare="REIDTPUB" Type="Date"
                                                        Operator="GreaterThan"></asp:CompareValidator>
                          <asp:RangeValidator id="REIDTPUBrangeValDate"
                                                        Type="Date"
                                                        EnableClientScript="true"
                                                        ControlToValidate="REIDTPUB"
                                                        ErrorMessage="Format incorrecte de la data de publicaci&oacute; (DD/MM/AAAA)"
                                                        Display="None"
                                                        enabled="false"
                                                        MaximumValue="1/1/2100"
                                                        MinimumValue="1/1/1900"
                                                        runat="server"/>
                          <div class="form-row">
                            <div class="form-group col-6">                         
                                <label for="REIDTPUB" class="col-form-label-sm">* Data publicaci&oacute;:</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="REIDTPUB" MaxLength="10" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <div class="input-group-prepend">
                                      <div class="input-group-text"><a href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=REIDTPUB','calendar_window','width=150,height=188');calendar_window.focus()"><img src="/img/common/iconografia/ico_calendari.png" border="0" align="top"></a></div>
                                    </div>
                                </div>  
                            </div>  
                            <div class="form-group col-6">
                                <label for="horaPublicacio" class="col-form-label-sm">Hora publicaci&oacute;:</label>
                                <asp:TextBox runat="server" ID="horaPublicacio" MaxLength="5" CssClass="form-control form-control-sm" aria-describedby="horapub"></asp:TextBox><small class="form-text text-muted" id="horapub">(hh:mm)</small>
                            </div> 
                            <div class="form-group col-6">                         
                                <label for="REIDTCAD" class="col-form-label-sm">Data caducitat:</label>
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="REIDTCAD" MaxLength="10" CssClass="form-control form-control-sm"></asp:TextBox>
                                    <div class="input-group-prepend">
                                      <div class="input-group-text"><a href="javascript:calendar_window=window.open('/GAIA/aspx/calendari.aspx?camp=REIDTCAD','calendar_window','width=150,height=188');calendar_window.focus()"><img src="/img/common/iconografia/ico_calendari.png" border="0" align="top" /></a></div>
                                    </div>
                                </div>  
                            </div>  
                            <div class="form-group col-6">
                                <label for="horaCaducitat" class="col-form-label-sm">Hora caducitat:</label>
                                <asp:TextBox runat="server"  ID="horaCaducitat" MaxLength="5" CssClass="form-control form-control-sm" aria-describedby="horacad"></asp:TextBox><small class="form-text text-muted" id="horacad">(hh:mm)</small>
                            </div> 
                                                   
                          </div>
                        </div>
                    </div>
                </div>                    
                
                <asp:panel runat="server" id="pnlVisibilitat" visible="true" CssClass="col">
                    <div class="card bg-light mb-3">
                        <div class="card-header">
                          <h6 class="font-weight-bold">Visibilitat del contingut</h6>
                        </div>
                        <div class="TRvisible card-body">
                        	<div class="form-group form-check">                              
                              <asp:CheckBox runat="server" id="chkVisibleInternet" CssClass="form-check-input"/>
                              <asp:label id="lblVisible" runat="server" CssClass="form-check-label" associatedControlID="chkVisibleInternet"/>
                          </div>
                        </div>
                    </div>
                </asp:panel>
            </div>
		</asp:panel>
            
        <div class="form-group text-center">
            <asp:panel runat="server" id="pnlBotonsOk">
              <input class="btn btn-success" type=button id="btnInsert"  value="Acceptar"  runat="server"  OnServerClick="clickModificaEstructura"  onClick="if (comprovarUbicacioSeleccionada()) { document.getElementById('divMain').style.visibility = 'hidden'; document.getElementById('divWait').style.visibility = 'visible' } else { return (false) }">
              <input class="btn btn-warning" type=button id="btnCancelar"  value="Cancel&middot;lar"  runat="server"  OnServerClick="clickCancelar">
             </asp:panel>
            <asp:panel runat="server" id="pnlBotonsSensePermis"  visible="false">
             <input class="btn btn-warning" type=button id="btnTornar"  value="Tornar"  runat="server"  OnServerClick="clickCancelar">
             </asp:panel>
        </div>
        
        
        
        
        <asp:label id="lblCodi" runat="server"/>           
        

      </form>
    </div>
  </div>
</div>




<!-- Optional JavaScript --> 
<!-- jQuery first, then Tether, then Bootstrap JS. --> 
<script src="https://code.jquery.com/jquery-3.3.1.min.js" integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=" crossorigin="anonymous"></script>
<%--<script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>--%>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script> 
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
<script language="javascript">

      var aPlantilles = (document.getElementById('llistaPlantilles').value).split(",");
      iniPantalla(aPlantilles);
      if (document.getElementById("WEBDSTCO").value != "") {
          var arrayTCO = document.getElementById("WEBDSTCO").value.split(",");
          var cont = 0;
        
          // Teresa: las celdas(t..) empiezan en 1 y los arrays (aPlantilles y arrayTCO) en 0.
          while (cont < arrayTCO.length) {
            
              if ((arrayTCO[cont] != document.getElementById("tipusNode").value) && (arrayTCO[cont] != 54)) {
                  document.getElementById("t" + cont).disabled = true;
                  document.getElementById("t" + cont).bgColor = "#eeeeee";
              }
              else {
                  document.getElementById("ubicacionsSeleccionables").value = 1;
                  document.getElementById("t" + cont).disabled = false;
                  if (document.getElementById("txtPosicioEstructuraReal").value == cont) {
                      document.getElementById("t" + cont).bgColor = "#CACAAA";
                      //document.getElementById("ultimaDivisio").value = "t" + cont;
                      document.getElementById("ultimaPlantilla").value = aPlantilles[cont];
                      
                  }
                  else {
                      document.getElementById("t" + cont).bgColor = "#ffffff";
                  }
              }
              cont++;
          }
           if ($("#ultimaPlantilla").val() != "" && $("#ultimaDivisio").val() != "") {
              mostrarDesplegable($("#ultimaPlantilla").val(), $("#ultimaDivisio").val());
              }
      }
      else {
          var ncelda = document.getElementById("txtposicioEstructura").value;
          if (ncelda >= 0 && document.getElementById('llistaPlantilles').value != "") {
              if (document.getElementById("ultimaDivisio") != null)
              {

                  document.getElementById("ultimaDivisio").value = "t" + ncelda;
              }
              document.getElementById("ultimaPlantilla").value = aPlantilles[ncelda];
              if (aPlantilles[ncelda]) {
                  mostrarDesplegable(aPlantilles[ncelda], "t" + ncelda);
              }
          }
      }

      function iniPantalla(aPlantilles) {
          for (i = 0; i < $("#trPlantilla2 select").length; i++) {
              if (document.getElementById('ddlb_plantillad' + i) != null)
              {
                 document.getElementById('ddlb_plantillad' + i).style.display = 'none';
              }
             
              $("#trPlantilla2 select")[i].style.display='none';
          }
          
      }

      // aquesta funciï¿½ existeix nomï¿½s per compatibilitat amb frmplantillaV2.asx
      function activaCamps(activar) {
          return true;
      }
      function comprovarUbicacioSeleccionada() {
          if ((document.getElementById("ubicacionsSeleccionables").value >= 1) && ((document.getElementById("txtposicioEstructura").value == -1) && (document.getElementById("txtposicioEstructuraReal").value == -1))) {
              if (!confirm("No hi ha cap cel&middot;la seleccionada i el contingut no ser&agrave; visible. Vols continuar?")) {
                  return (false);

              }
          }
          return (true);
      }

      function seleccionaCelda(celda, nomDivisio) {

          if (document.getElementById(celda).disabled == false) {

              if (document.getElementById("ultimaDivisio").value != "") {

                  var ultimaDivisio = document.getElementById("ultimaDivisio").value;

                  document.getElementById(ultimaDivisio).bgColor = "#FFFFFF";
                  //amaguem el desplegable corresponent a la celï¿½la anterior
                  if (document.getElementById('ddlb_plantilla' + ultimaDivisio)) {
                      document.getElementById('ddlb_plantilla' + ultimaDivisio).style.display = 'none';
                  }
              }
              document.getElementById("txtposicioEstructura").value = celda.substring(1, celda.length);
              document.getElementById(celda).bgColor = "#CACAAA";


              document.getElementById("ultimaDivisio").value = celda;
              var aPlantilles = (document.getElementById('llistaPlantilles').value).split(",");

              document.getElementById('ultimaPlantilla').value = aPlantilles[celda.substring(1)];

              if (aPlantilles[celda.substring(1)]) {
                  mostrarDesplegable(aPlantilles[celda.substring(1)], celda);
              }
          }
      }

      // GAIA2
      function seleccionaDiv(div) {

          //marco de color blanco la celda anterior
          if (document.getElementById("ultimaDivisio").value != "") {

              var ultimaDivisio = document.getElementById("ultimaDivisio").value;
              $("#" + ultimaDivisio).css("background-color", "#FFFFFF");
              
              //amaguem el desplegable corresponent a la celï¿½la anterior
              if (document.getElementById('ddlb_plantilla' + ultimaDivisio)) {
                  document.getElementById('ddlb_plantilla' + ultimaDivisio).style.display = 'none';
              }
          }

          // marco de color verde la nueva celda y cargo sus valores
          div.css("background-color","#E6F9FB");
          $("#ultimaDivisio").val(div.attr("id"));
          $("#txtposicioEstructura").val( div.attr("id").substring(1, div.attr("id").length));
      }

      function mostrarDesplegable(sPlantilla, celda) {
          if (sPlantilla.indexOf("|") >= 0) {
              document.getElementById('trPlantilla1').style.display = '';
              document.getElementById('trPlantilla2').style.display = '';
              document.getElementById('ddlb_plantilla' + celda).style.display = '';
          }
          else {
              document.getElementById('trPlantilla1').style.display = 'none';
              document.getElementById('trPlantilla2').style.display = 'none';
          }
      }

      $.fn.multiline = function (text) {
          this.text(text);
          this.html(this.html().replace(/\n/g, '<br/>'));
          return this;
      }
      $(document).ready(function () {

          if (document.getElementById('chkVisibleInternet').checked) {
              $('.TRvisible').css("background-color", "#D9F5E0");
              $("#lblVisible").html("El contingut podria ser visible en alguna p&agrave;gina de web o intranet.<br>Desmarca el camp per canviar-ho:")
             
          }
          else {
              $('.TRvisible').css("background-color", "#F6D2D5");
              $("#lblVisible").html("El contingut podria no ser visible en alguna p&agrave;gina de web o intranet.<br>Marca el camp per canviar-ho:")
          }
          $('.TRvisible').click(function () {
              if (document.getElementById('chkVisibleInternet').checked) {
                  $("#lblVisible").html("El contingut podria ser visible en alguna p&agrave;gina de web o intranet.<br>Desmarca el camp per canviar-ho:")
                  $(this).css("background-color", "#D9F5E0");
              }
              else {
                  $("#lblVisible").html("El contingut podria no ser visible en alguna p&agrave;gina de web o intranet.<br>Marca el camp per canviar-ho:")
                  $(this).css("background-color", "#F3C6CA");
              }

          });
       
      });

</script> 

</body>
</html>
