<%@ Page Language="vb" AutoEventWireup="false" Debug="true" EnableViewStateMac="false"  %>

<!DOCTYPE html>
<!--[if lt IE 7]>      
    <html class="no-js lt-ie9 lt-ie8 lt-ie7" lang="ca"> 
    <![endif]--><!--[if IE 7]>         
        <html class="no-js lt-ie9 lt-ie8" lang="ca"> <![endif]--><!--[if IE 8]>         
            <html class="no-js lt-ie9" lang="ca"> <![endif]--><!--[if gt IE 8]><!-->
<html runat="server" class="no-js" xmlns="http://www.w3.org/1999/xhtml" lang="ca">
    <!--<![endif]-->
    <head runat="server">
        <style type="text/css" media="screen">
            <!-- .body {margin:0px; font-size: 100%;} 
                 #contenidor{width: 800px; min-height:500px; margin-left:auto; margin-right:auto;} 
                 #contingut{clear:both}                                           
                 .wpx800{width: 800px;} .wpx458{width: 458px;} .wpx156{width: 156px;} .wpx282{width: 282px;} .wpx460{width: 460px;} .wpx320{width: 320px;} .wpx300{width: 300px;} -->

        </style>
<title>GAIA | Intranet</title>
        <asp:Literal id="lblhead" runat="server">

        </asp:Literal>
        <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
        <meta http-equiv="Content-Style-Type" content="text/css"/><meta http-equiv="Content-Language" content="ca"/>
        <meta http-equiv="Content-Script-Type" content="text/javascript"/><meta name="Author" content="Ajuntament de L'Hospitalet"/>
        <meta name="DC.Creator" content="Ajuntament de L'Hospitalet"/>
        <meta name="DC.Identifier" content="http://www.l-h.cat"/>
        <meta name="title" content="GAIA | Intranet" runat="server" id="metatitle"/>
        <meta name="DC.title" content="GAIA | Intranet"/>
        <link rel="schema.DC" href="http://purl.org/dc/elements/1.1/"/>
        <meta name="DC.Language" content="ca"/><meta name="Copyright" content="http://www.l-h.cat/gdocs/d6779062.pdf"/> 
        <!-- jQuery Library + ALL jQuery Tools -->
<%--        <script type="text/javascript" src="http://cdn.jquerytools.org/1.2.7/full/jquery.tools.min.js"></script>--%>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-tools/1.2.7/jquery.tools.min.js"> </script>

<!-- visor imatges -->
<script type="text/javascript" src="js/visorimatges/jquery.lightbox-0.5CAS.js"></script>
<script type="text/javascript" src="js/visorimatges/jquery.lightbox-0.5CAT.js"></script>
<script type="text/javascript" src="js/visorimatges/noscript.js"></script>
<!-- fi visor imatges -->
<meta name="robots" content="NOINDEX,NOFOLLOW"/><link rel="stylesheet" href="Styles/gaiaIntranet.css" type="text/css" media="screen"/>
        <link rel="stylesheet" href="Styles/intranet.css" type="text/css" media="screen"/>
        <link rel="stylesheet" href="Styles/jquery.lightbox-0.5.css" type="text/css" media="screen"/>
        <link rel="stylesheet" href="Styles/gaiaIntranetPrint.css" type="text/css" media="print"/>
 <script>
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-156874-14', 'auto');
  ga('send', 'pageview');

</script>


</head>
    <body id="cosPagina" > 
    <div id="contenidor"><div class="floatleft wpx800 ">
    <%@Register TagPrefix="visor" TagName="Documents" Src="~/js/App_LocalResources/visordocumentshtml.ascx"%>
    <div class="floatleft wpx800 ">
      <!--  #INCLUDE VIRTUAL="~/js/App_LocalResources/cap.aspx" -->
        <div  class="floatleft wpx800 "><div  class="floatleft wpx800 "><div  class="floatleft wpx800 "><div  class="floatleft wpx800 "><div  class="floatleft wpx800 "><div  class="floatleft wpx800 ">
   <%@ Register TagPrefix="menuG" TagName="menuG" Src="~/js/App_LocalResources/menu.ascx" %>
<%
if  instr(Request.userAgent, "MSIE")=0 THEN 
	pnlAvis.visible=TRUE
	lblAVis.text = "Per raons d’eficiència i estalvi de recursos, GAIA s’ha desenvolupat per a que funcioni correctament en navegadors Microsoft Internet Explorer.<br />El navegador amb el que estàs obrint GAIA no compleix els nostres estàndards i pot ser que la usabilitat no sigui la prevista. "
else
	pnlAvis.visible=FALSE
end if
%>
<menuG:menug ID="menuG" Text="Menú GAIA" runat="server"/>
<asp:panel id="pnlAvis" runat="server" visible="false">
<div class="missatgeAvisIntranet"> <span class="topEsquerraBlanc"></span> <span class="topDretaBlanc"></span>
  <div class="icona">
    <asp:Label ID="lblAvis" runat="server" />
  </div>
  <span class="bottomEsquerraBlanc"></span> <span class="bottomDretaBlanc"></span> </div>
</asp:panel>

    
    </div>
            <div  class="floatleft wpx800 ">
                <div  class="floatleft wpx460 paddingDreta20 ">
                    <h3 class="titolApartatIntranet">Not&iacute;cies</h3>
                    <div class="llistatContinguts">
                        <ul>
                            <li>
                                <div class="borderRadius10  marginBottom10 border1Bot2Grise7e7e7 relativo fonsblanc displayInlineBlock ">
                                    <div class="topBloc"></div>
                                    <div class="contBloc">
                                        <div  class="wpx458 ">
                                            <div  class="floatleft wpx156 ">
                                                <div  class="floatleft imatgetamanymaximdisponible fotoContingut ">
                                                    <img src="/utils/obreFitxer.ashx?Fw9EVw48XS7I8hEkvZoPDAclqjbZXvRwQwqazCKBHSP720qazCZg81rslnzXBU9tQ9zYpwLglMFHqazC6rzoqazB" alt=""  class="border0 imatgetamanymaximdisponible fotoContingut "/></div>

                                            </div>
                                            <div  class="floatleft wpx282 padding10 ">
                                                <div  class="floatleft wpx282 ">
                                                    <div  class="floatleft wpx282 gris t075 arial ">24/10/2018</div></div>
                                                <div  class="floatleft wpx282 ">
                                                    <h3 class="floatleft wpx282 negre t12 marginBottom5 arialNarrow bold ">
 Nou formulari d&rsquo;agenda
</h3>
                                                    <div  class="floatleft wpx282 ">
                                                        <div  class="floatleft wpx282 gris t075 paddingBottom5 arial justificat ">S&rsquo;ha redissenyat el&nbsp; formulari d&rsquo;agenda de la intranet, accessible des de el men&uacute; de continguts de GAIA o des de l&rsquo;antic formulari d&rsquo;agenda. S&rsquo;han organitzat els diferents camps del formulari en seccions per aix&iacute; fer el proc&eacute;s d&rsquo;alta m&eacute;s entenedor i senzill.<br />
<br />
Aquest nou formulari incorpora noves funcionalitats, com per exemple, la geolocalitzaci&oacute; d&rsquo;activitats i/o cursos o la millora en la traducci&oacute; amb la incorporaci&oacute; de Apertium, que &eacute;s un sistema de traducci&oacute; autom&agrave;tica que fan servir webs com Softcatala. Tamb&eacute; s&rsquo;ha millorat la secci&oacute; de preus i aforaments m&uacute;ltiples.<br />
<br />
El nou formulari&nbsp; estar&agrave; durant un temps en per&iacute;ode de proves. Durant aquest per&iacute;ode ser&agrave; possible crear i editar actes d&rsquo;agenda amb el dos formularis, l&rsquo;antic i el nou.</div><div  class="floatleft wpx282 "><div  class="floatleft wpx282 "><div  class="floatleft wpx282 negre t075 paddingTop10 arial dre bold "><a href="/detallNoticia.aspx?1D5C1VqazAYh9X7O9irAd6xk92iziIgwqazCDL9qazAIYEfGBImpgqazB"  class="negre paddingTop10 arial dre bold " target="_self" title="Llegir m&eacute;s de  Nou formulari d&rsquo;agenda">m&eacute;s informaci&oacute; [+]<span class="visibilidadoculta">(Llegir m&eacute;s de  Nou formulari d&rsquo;agenda)</span></a></div></div></div></div></div></div></div></div><div class="botBloc"></div></div></li><li><div class="borderRadius10  marginBottom10 border1Bot2Grise7e7e7 relativo fonsblanc displayInlineBlock "><div class="topBloc"></div><div class="contBloc"><div  class="wpx458 "><div  class="floatleft wpx156 "><div  class="floatleft imatgetamanymaximdisponible fotoContingut "><img src="/utils/obreFitxer.ashx?Fw9EVw48XS4U68d3IbfPG9IFwwdFOV6ja4zv6ppQmbjl1z1ruzp7V10AOzFKDAGTt1yoUwk5qc0qazB" alt=""  class="border0 imatgetamanymaximdisponible fotoContingut "/></div></div><div  class="floatleft wpx282 padding10 "><div  class="floatleft wpx282 "><div  class="floatleft wpx282 gris t075 arial ">12/07/2016</div></div><div  class="floatleft wpx282 "><h3 class="floatleft wpx282 negre t12 marginBottom5 arialNarrow bold ">
 Les Llistes de distribuci&oacute; presenten noves funcionalitats i disseny
</h3>
    <div  class="floatleft wpx282 "><div  class="floatleft wpx282 gris t075 paddingBottom5 arial justificat ">El sistema de Llistes de distribuci&oacute; ha experimentat una millora important amb la introducci&oacute; d&rsquo;un disseny acurat i de noves funcionalitats, que inclouen un hist&ograve;ric dels enviaments de cada llista de distribuci&oacute;, un hist&ograve;ric dels subscriptors de les llistes (altes i baixes) i un control de la recepci&oacute; dels correus (qui els ha obert i quan).<br />
<br />
La p&agrave;gina d&rsquo;entrada mostra les Llistes administrades amb el nom de cada llista, el remitent i les opcions que s&rsquo;hi poden aplicar: gesti&oacute; de la llista, enviar correus, hist&ograve;ric d&rsquo;enviaments i donar de baixa.<br />
</div><div  class="floatleft wpx282 "><div  class="floatleft wpx282 "><div  class="floatleft wpx282 negre t075 paddingTop10 arial dre bold "><a href="/detallNoticia.aspx?11Hq6yjHNICi1o4ohpavxKFtp4l66pqaLDkqaEp9FqBwqazB"  class="negre paddingTop10 arial dre bold " target="_self" title="Llegir m&eacute;s de  Les Llistes de distribuci&oacute; presenten noves funcionalitats i disseny">m&eacute;s informaci&oacute; [+]<span class="visibilidadoculta">(Llegir m&eacute;s de  Les Llistes de distribuci&oacute; presenten noves funcionalitats i disseny)</span></a></div></div></div></div></div></div></div></div><div class="botBloc"></div></div></li><li><div class="borderRadius10  marginBottom10 border1Bot2Grise7e7e7 relativo fonsblanc displayInlineBlock "><div class="topBloc"></div><div class="contBloc"><div  class="wpx458 "><div  class="floatleft wpx282 padding10 "><div  class="floatleft wpx282 "><div  class="floatleft wpx282 gris t075 arial ">22/04/2016</div></div><div  class="floatleft wpx282 "><h3 class="floatleft wpx282 negre t12 marginBottom5 arialNarrow bold ">
 El nou cercador de la Intranet permet buscar actes d&rsquo;Agenda caducats
</h3><div  class="floatleft wpx282 "><div  class="floatleft wpx282 gris t075 paddingBottom5 arial justificat ">Des del nou cercador de la Intranet, accessible des de la cap&ccedil;alera de qualsevol de les seves p&agrave;gines, &eacute;s possible ara buscar actes d&rsquo;Agenda caducats. Nom&eacute;s cal seleccionar la pestanya Agenda, clicar al bot&oacute; &ldquo;M&eacute;s opcions&rdquo; i seleccionar &ldquo;Veure actes caducats&rdquo;.<br />
<br />
Una vegada feta la cerca, prement sobre el bot&oacute; &ldquo;+&rdquo; es pot editar la informaci&oacute; de l&rsquo;acte (si tenim perm&iacute;s d&rsquo;edici&oacute;), canviar-ne les dates d&rsquo;inici i final, i modificar-ne la data de publicaci&oacute; perqu&egrave; es torni a activar l&rsquo;acte d&rsquo;Agenda editat.<br />
</div><div  class="floatleft wpx282 "><div  class="floatleft wpx282 "><div  class="floatleft wpx282 negre t075 paddingTop10 arial dre bold "><a href="/detallNoticia.aspx?1IKlvDvrv0TXJTtpaxQhNoQwDdTWB90Uzdr3KqazC6d911UqazB"  class="negre paddingTop10 arial dre bold " target="_self" title="Llegir m&eacute;s de  El nou cercador de la Intranet permet buscar actes d&rsquo;Agenda caducats">m&eacute;s informaci&oacute; [+]<span class="visibilidadoculta">(Llegir m&eacute;s de  El nou cercador de la Intranet permet buscar actes d&rsquo;Agenda caducats)</span></a></div></div></div></div></div></div></div></div><div class="botBloc"></div></div></li><li><div class="borderRadius10  marginBottom10 border1Bot2Grise7e7e7 relativo fonsblanc displayInlineBlock "><div class="topBloc"></div><div class="contBloc"><div  class="wpx458 "><div  class="floatleft wpx156 "><div  class="floatleft imatgetamanymaximdisponible fotoContingut "><img src="/utils/obreFitxer.ashx?Fw9EVw48XS6cIjMt71XYqazCPPbzA6rkxWqQAAfD1GM7b3rDHtl7cpUKwk3oAgVDVqazAOqazADarac3gokcqazB" alt=""  class="border0 imatgetamanymaximdisponible fotoContingut "/></div></div><div  class="floatleft wpx282 padding10 "><div  class="floatleft wpx282 "><div  class="floatleft wpx282 gris t075 arial ">26/11/2015</div></div><div  class="floatleft wpx282 "><h3 class="floatleft wpx282 negre t12 marginBottom5 arialNarrow bold ">
 M&eacute;s opcions per eliminar continguts
</h3><div  class="floatleft wpx282 "><div  class="floatleft wpx282 gris t075 paddingBottom5 arial justificat ">S&rsquo;hi han afegit dues opcions noves per eliminar continguts dins dels arbres de GAIA, &ldquo;Esborrar tots&rdquo; i &ldquo;Caducar&rdquo;, que completen l&rsquo;&uacute;nica opci&oacute; que hi havia fins ara, &ldquo;Esborrar&rdquo;. S&rsquo;hi accedeix al men&uacute; d&rsquo;aquestes opcions, que incorpora icones noves, clicant amb el bot&oacute; dret del ratol&iacute; al contingut escollit. Aquestes s&oacute;n les seves caracter&iacute;stiques.<br />
<br />
<strong>Esborrar</strong>: elimina el contingut &uacute;nicament de la branca on es troba seleccionat.<br />
<strong>Esborrar tots</strong>: marca el contingut com a esborrat en totes les ubicacions on hi apareix.<br />
<strong>Caducar</strong>: marca el contingut com a caducat en totes les ubicacions on hi apareix.<br />
<br />
A &ldquo;m&eacute;s informaci&oacute;&rdquo; trobareu quan s&rsquo;ha de fer servir cada opci&oacute;.<br />
</div><div  class="floatleft wpx282 "><div  class="floatleft wpx282 "><div  class="floatleft wpx282 negre t075 paddingTop10 arial dre bold "><a href="/detallNoticia.aspx?1eKVFpcCnGjxpplpgpZRHdN0bB0FRwSbKU0szCkoVRQEqazB"  class="negre paddingTop10 arial dre bold " target="_self" title="Llegir m&eacute;s de  M&eacute;s opcions per eliminar continguts">m&eacute;s informaci&oacute; [+]<span class="visibilidadoculta">(Llegir m&eacute;s de  M&eacute;s opcions per eliminar continguts)</span></a></div></div></div></div></div></div></div></div><div class="botBloc"></div></div></li><li><div class="borderRadius10  marginBottom10 border1Bot2Grise7e7e7 relativo fonsblanc displayInlineBlock "><div class="topBloc"></div><div class="contBloc"><div  class="wpx458 "><div  class="floatleft wpx282 padding10 "><div  class="floatleft wpx282 "><div  class="floatleft wpx282 gris t075 arial ">22/07/2015</div></div><div  class="floatleft wpx282 "><h3 class="floatleft wpx282 negre t12 marginBottom5 arialNarrow bold ">
 Nova funcionalitat per saber on s&rsquo;hi utilitza un contingut
</h3><div  class="floatleft wpx282 "><div  class="floatleft wpx282 gris t075 paddingBottom5 arial justificat ">GAIA ha incorporat una nova funcionalitat que permet con&egrave;ixer en quines p&agrave;gines del Web, la Seu electr&ograve;nica o la Intranet s&rsquo;hi utilitza un contingut creat (not&iacute;cia, acte d&rsquo;agenda, tr&agrave;mit...) o amb quin altre contingut est&agrave; relacionat.<br />
<br />
Per accedir-hi, a l&rsquo;arbre de GAIA s&rsquo;ha de clicar amb el bot&oacute; dret del ratol&iacute; al contingut escollit i, dins del men&uacute; que s&rsquo;obre, anar a Propietats, full on es troba aquesta informaci&oacute; amb el nom &ldquo;On s&rsquo;utilitza el contingut?&rdquo;.<br />
</div><div  class="floatleft wpx282 "><div  class="floatleft wpx282 "><div  class="floatleft wpx282 negre t075 paddingTop10 arial dre bold "><a href="/detallNoticia.aspx?1AgvgluMqqazCDcoYiI8344BOtnGNvzSRFu72SqazCLLrhU9ZUqazB"  class="negre paddingTop10 arial dre bold " target="_self" title="Llegir m&eacute;s de  Nova funcionalitat per saber on s&rsquo;hi utilitza un contingut">m&eacute;s informaci&oacute; [+]<span class="visibilidadoculta">(Llegir m&eacute;s de  Nova funcionalitat per saber on s&rsquo;hi utilitza un contingut)</span></a></div></div></div></div></div></div></div></div><div class="botBloc"></div></div></li><li><div class="borderRadius10  marginBottom10 border1Bot2Grise7e7e7 relativo fonsblanc displayInlineBlock "><div class="topBloc"></div><div class="contBloc"><div  class="wpx458 "><div  class="floatleft wpx282 padding10 "><div  class="floatleft wpx282 "><div  class="floatleft wpx282 gris t075 arial ">01/10/2014</div></div><div  class="floatleft wpx282 "><h3 class="floatleft wpx282 negre t12 marginBottom5 arialNarrow bold ">
 Nova funcionalitat per con&egrave;ixer el nombre de visites dels continguts
</h3><div  class="floatleft wpx282 "><div  class="floatleft wpx282 gris t075 paddingBottom5 arial justificat ">GAIA ha incorporat una nova funcionalitat que permet con&egrave;ixer el nombre de visites des del Web que ha rebut un contingut (not&iacute;cia, acte d&rsquo;agenda o tr&agrave;mit) en els darrers 30 dies.<br />
<br />
Per accedir-hi, a l&rsquo;arbre de GAIA s&rsquo;ha de clicar amb el bot&oacute; dret del ratol&iacute; al contingut escollit i, dins del men&uacute; que s&rsquo;obre, anar a Propietats, full on es troba la informaci&oacute; de les visites.<br />
<br />
Per nombre de visites s&rsquo;ent&eacute;n el nombre de vegades que els usuaris han seguit l&rsquo;enlla&ccedil; &ldquo;llegir m&eacute;s&rdquo; del contingut (en el cas de Not&iacute;cies i Agenda) i, per tant, no es comptabilitzen quan es mostra en mode resum (p. e., portada del Web); d&rsquo;aquesta manera podem con&egrave;ixer l&rsquo;inter&egrave;s real de l&rsquo;usuari pel contingut. Tampoc hi compten les visites fetes per robots de cercadors.<br />
<br />
A m&eacute;s del nombre de visites, al full Propietats d&rsquo;un tr&agrave;mit tamb&eacute; es troba ara la informaci&oacute; de l&rsquo;hist&ograve;ric dels canvis que s&rsquo;hi han fet.<br />
</div><div  class="floatleft wpx282 "><div  class="floatleft wpx282 "><div  class="floatleft wpx282 negre t075 paddingTop10 arial dre bold "><a href="/detallNoticia.aspx?17qazCURogHkLeZ0zxbofUuV3xcxEWglfYYfPGLLrk6G2MoqazB"  class="negre paddingTop10 arial dre bold " target="_self" title="Llegir m&eacute;s de  Nova funcionalitat per con&egrave;ixer el nombre de visites dels continguts">m&eacute;s informaci&oacute; [+]<span class="visibilidadoculta">(Llegir m&eacute;s de  Nova funcionalitat per con&egrave;ixer el nombre de visites dels continguts)</span></a></div></div></div></div></div></div></div></div><div class="botBloc"></div></div></li></ul></div></div><div  class="floatleft wpx320 "><div  class="floatleft wpx320 "><h3 class="titolApartatIntranet">Enlla&ccedil;os</h3></div><div  class="wpx300 negre t08 padding10 borderRadius15 relativo arial fonsgris02 displayBlock "><div  class="iconaDocument ">

<img src="img/ico_pdf.png" alt="Document"/>

<a href="/utils/obreFitxer.ashx?Fw9EVw48XS4NXAAkoDOS36itw3CFUz8kWFysVSjjAjOtDqTJHkygssuqazCgUOrwd0I"  class="iconaDocument " target="_blank" title="Manual d&rsquo;ús del gestor de continguts GAIA (nova finestra) ">Manual d'ús del gestor de continguts GAIA</a></div><div  class="iconaDocument ">

<img src="img/ico_pdf.png" alt="Document"/>

<a href="/utils/obreFitxer.ashx?Fw9EVw48XS4NtxvH2mkDBaTOcBN1omXe2sAWhFcjtbqazAojFo07bxAAQfNKOTmessC"  class="iconaDocument " target="_blank" title="Manual d&rsquo;ús del web municipal (nova finestra) ">Manual d'ús del web municipal</a></div><div  class="iconaDocument ">

<img src="img/ico_pdf.png" alt="Document"/>

<a href="/utils/obreFitxer.ashx?Fw9EVw48XS61Qe1WWAhPq1eEt7eeXfL7Vz5yCKMGcqazCe8oYV9Ptq1YC3cFMyzqazARvp"  class="iconaDocument " target="_blank" title="Guia d&rsquo;ús i estil. Categorització i codificació d&rsquo;actes d&rsquo;Agenda als webs municipals  (nova finestra) ">Guia d'ús i estil. Categorització i codificació d'actes d'Agenda als webs municipals </a></div></div></div></div></div></div></div></div></div></div><div class="floatleft wpx800 ">
  <!--  #INCLUDE VIRTUAL="~/js/App_LocalResources/peu.aspx" -->
    </div></div>
 </div>
</body>

</html>