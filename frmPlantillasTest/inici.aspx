
<!doctype html>
<html lang="ca" ><head runat="server">
    <meta charset="UTF-8"/><meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <asp:Literal id="lblhead" runat="server">

    </asp:Literal><meta http-equiv="Content-Style-Type" content="text/css"/>
    <meta http-equiv="Content-Language" content="ca"/>
    <meta http-equiv="Content-Script-Type" content="text/javascript"/>
    <meta name="Author" content="Ajuntament de L'Hospitalet"/>
    <meta name="DC.Creator" content="Ajuntament de L'Hospitalet"/>
    <meta name="DC.Identifier" content="http://www.l-h.cat"/>
    <meta id="metatitle" name="title" content="Inici" runat="server" />
    <meta name="DC.title" content="Inici"/><link rel="schema.DC" href="http://purl.org/dc/elements/1.1/"/>
    <meta name="DC.Language" content="ca"/><meta name="Copyright" content="http://www.l-h.cat/gdocs/d6779062.pdf"/>
    <meta name="DC.Description" content="Inici"/>
    <meta name="Description" content="Inici"/>
    <meta name="keywords" content="educa, cultura"/>
    <meta name="DC.subject" content="educa, cultura"/>
    <meta name="robots" content="INDEX,FOLLOW"/>
    <link rel="stylesheet" type="text/css" href="Styles/default.css" />
    <link rel="stylesheet" href="Styles/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
<!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
<!-- visor imatges -->
    <script type="text/javascript" src="js/jquery.tools.min.js"> </script>
    <script type="text/javascript" src="js/visorimatges/jquery.min.js"></script>
    <script type="text/javascript" src="js/visorimatges/jquery-1.6.4.min.js"></script>
<script type="text/javascript" src="js/visorimatges/jquery.lightbox-0.5CAS.js"></script>
<script type="text/javascript" src="js/visorimatges/jquery.lightbox-0.5CAT.js"></script>
<script type="text/javascript" src="js/visorimatges/noscript.js"></script>
<!-- fi visor imatges -->
    <title>Inici</title>
    <link rel="stylesheet" href="https://www.l-h.cat/css/barresLH/barraLH.css" type="text/css" media="screen"/>
    <link rel="stylesheet" href="Styles/culturaLHeduca.css" type="text/css" media="screen"/>
    <link rel="stylesheet" href="Styles/formulariCulturalheduca.css" type="text/css" media="screen"/>
    <link rel="stylesheet" href="Styles/jquery.lightbox-0.5.css" type="text/css" media="screen"/>

</head>
    <body > 
        <div><div class='row'>
            <div class='col' >
<nav role="navigation" class="navbar navbar-LH navbar-static-top">
      <div class="container-fluid">
    		<!-- Logo i boto expandir menu -->
    		<div class="navbar-header">
              <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#MainMenu">
                <span class="sr-only">Veure opcions</span>
                <span class="glyphicon glyphicon-option-horizontal"></span>
              </button>
              <a class="navbar-brand" href="~/inici.aspx?id=1" target="_blank" title="http://www.l-h.cat/"><img class="header-logo img-responsive" src="https://www.l-h.cat/img/lh12/common/logo_ajuntament_barra.png" alt="Ajuntament de L'Hospitalet"/></a>
    		</div>

    		<div class="collapse navbar-collapse" id="MainMenu">
                <!-- Menu-dret -->
                <ul class="nav navbar-nav navbar-right">
                    <li class="divider-vertical"><a href="http://www.l-h.cat/laciutat/265283_1.aspx?id=1" target="_blank" class="text-uppercase" title="Anar a La ciutat">La ciutat</a></li>
                    <li class="divider-vertical"><a href="http://www.l-h.cat/ajuntament/249727_1.aspx?id=1" target="_blank" class="text-uppercase" title="Anar a L'Ajuntament">L'Ajuntament</a></li>
                    <li class="divider-vertical-icon">
                            <ul>
                                <li><a href="http://www.l-h.cat/directori/directori.aspx?id=1" title="Anar a Directori" target="_blank" class="text-uppercase"><img src="https://www.l-h.cat/img/lh12/common/ico_directorip.png" alt="Directori"/></a></li>
                                <li><a href="https://seuelectronica.l-h.cat/171065_1.aspx?id=1" title="Anar a Tr&agrave;mits" target="_blank" class="text-uppercase"><img src="https://www.l-h.cat/img/lh12/common/ico_tramitsp.png" alt="Tr&agrave;mits"/></a></li>
                                <li><a href="http://www.l-h.cat/utils/guiaUrbana/planol.aspx?1%e2%80%9e%c2%be%11%c5%922%c3%ab%c3%98%c2%a9%c5%be%e2%80%94%c3%90%3d%c3%82%c3%ba%19%7d+t" title="Anar a Pl&agrave;nol de la ciutat" target="_blank" class="text-uppercase"><img src="https://www.l-h.cat/img/lh12/common/ico_planolp.png" alt="Pl&agrave;nol de la ciutat"/></a></li>
                            </ul>
                    </li>
                    <li><a href="/LHEduca/inici_2.aspx" title="Cambiar idioma a castellano" id="canviIdioma" hreflang="es" lang="es"><abbr>CAS</abbr></a></li>
                </ul>
    		</div>
      </div>
    </nav>
    
<script type="text/javascript">
	<!--
   var url = window.location.href;
   var paramsStart = url.indexOf("?");  
   if(paramsStart != -1){
		   	if (1==1) {
				url=url.replace("\?1","\?2");
			}
			else {
				url=url.replace("\?2","\?1");	
			}				
			
	 		if (url.indexOf("id=1",paramsStart + 1) >0) {
	 		document.getElementById("canviIdioma").href=document.getElementById("canviIdioma").href + "?"+url.substr(paramsStart + 1).replace("id=1","id=2");

			}
			else {
			document.getElementById("canviIdioma").href=document.getElementById("canviIdioma").href + "?"+url.substr(paramsStart + 1).replace("id=2","id=1");
			}
		
		}
	//-->
</script>
</div></div><div class='row'><div class='col' ><div class='row'><div class='col' >
<div class="capPortada">
<h1>CulturaLH educa</h1>
</div></div></div><div class='row'><div class='col' >


    <div class="llistatRedsSocials">
        <ul>
        <li class="youtube"><a href="http://intranet/LHEduca/1710922_1.aspx" target="_blank" title="YouTube" ></a></li>
            <li class="facebook"><a href="https://www.facebook.com/CulturaLH" target="_blank" title="Facebook" ></a></li>
            <li class="twitter"><a href="https://twitter.com/LHCultura" target="_blank" title="Twitter" ></a></li>
            <li class="issuu"><a href="https://issuu.com/culturalh/stacks" target="_blank" title="Issuu" ></a></li>
      </ul>
    </div>
    
</div><div class='col-md-3' >


<form action="cercador.aspx" method="post">
<div class="input-group">
    <input type="text" class="form-control" placeholder="text a cercar..."  id="buscaText" name="buscaText">
    <div class="input-group-btn">
      <button class="btn btn-default" type="submit">Cercar</button>
    </div>
  </div>
</form> 

 


</div></div><div class='row'><div class='col' ><div class='row'><div class='col-md-9 col-lg-9' > 
    <!-- Para que no se mueva solo data-interval="false" -->
<div id="myCarousel" class="carousel slide" data-ride="carousel" data-interval="false">
  <!-- Indicators -->
  <ol class="carousel-indicators" style="bottom:10em;">
    	 <asp:Literal ID="ltIndicators" runat="server"></asp:Literal>
  </ol>

  <!-- Wrapper for slides -->
  <div class="carousel-inner">
<asp:Literal ID="ltcontingut" runat="server" />
</div>

    <!-- Left and right controls -->
  <a class="carousel-control-prev" href="#myCarousel" data-slide="prev">
    <span class="carousel-control-prev-icon"></span>
  </a>
  <a class="carousel-control-next" href="#myCarousel" data-slide="next">
    <span class="carousel-control-next-icon"></span>
  </a>
    </div> 
    
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Page Language="vb" debug="TRUE"%>  

<script runat="server">
Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)  Handles Me.Load
                
                
                Dim codiIdioma as string
                IF Request("id") is Nothing THEN
                    codiIdioma="1"		
                ELSE
                    if Request("id").toString().equals("") then
                        codiIdioma="1"				
                    else
                        if cint(Request("id"))>3 then
                            codiIdioma="1"				
                        else
                            codiIdioma = Request("id").toString()					
                        end if 
                    end if
                END IF
            
                Dim GAIA As GAIAws
                GAIA = New GAIAws
    
                Dim ds As New DataSet
                Dim dbRow As DataRow  		
                Dim carousel As String = ""
        		Dim contador As Integer
				Dim imagen As String = ""
        		Dim propImagen As String()
                
				ds= GAIA.wsUltimesFullesInfoDS(-1, codiIdioma, 0, 0, 5, "1698712", -1)
                If ds.Tables(0).Rows.Count > 0 Then
                
					For Each dbRow In  ds.Tables(0).Rows
					
						imagen = GAIA.wsUltimesImatges(1, 0, dbRow("RELINCOD"), "")
                		propImagen = imagen.Split(",")
						
						carousel &= "<div class=""carousel-item " & IIf(contador = 0, "active", "") & """>"
                	  	carousel &= "<img src=""" & propImagen(2) & """ alt=""" & propImagen(1) & """ style=""width:100%;""/>"
                		carousel &= "<div class=""carousel-caption""><h3>" & dbRow("INFDSTIT") & "</h3>"
                		carousel &= "" & dbRow("INFDSTXT") & "<br/></div></div>"
                		contador += 1
					Next dbrow 
                End If
                
				ds.dispose()                
                GAIA.dispose()
                
				ltcontingut.text=carousel	
				
				For i = 0 To contador - 1
            		ltIndicators.Text &= "<li data-target=""#myCarousel"" data-slide-to=""" & i & """ class=""" & IIf(i = 0, "active", "") & """ style=""background-color:darkRed;""></li>"
        		Next
                
            End Sub
</script>     </div><div class='col-md-3' >


<script src="js/modernizr.custom.js"></script>
<div class="main clearfix">
	<nav id="menu" class="navPortada">

<ul><li ><a href="http://intranet/LHEduca/1707924_1.aspx?id=1" target="_self" title="Anar a Presentaci&oacute;">Presentaci&oacute;</a></li>
    <li><a href="http://intranet/LHEduca/1708817_1.aspx?id=1" target="_self" title="Anar a Activitats per a les escoles">Activitats per a les escoles</a></li>
    <li><a href="http://intranet/LHEduca/1711234_1.aspx?id=1" target="_self" title="Anar a Cursos i tallers">Cursos i tallers</a></li>
    <li><a href="http://intranet/LHEduca/1711260_1.aspx?id=1" target="_self" title="Anar a Projectes singulars">Projectes singulars</a></li>
    <li><a href="http://intranet/LHEduca/1711268_1.aspx?id=1" target="_self" title="Anar a Fem Tàndem L&rsquo;H">Fem Tàndem L&rsquo;H</a></li></ul>

  </nav>
</div>

		<script>
			//  The function to change the class
			var changeClass = function (r,className1,className2) {
				var regex = new RegExp("(?:^|\\s+)" + className1 + "(?:\\s+|$)");
				if( regex.test(r.className) ) {
					r.className = r.className.replace(regex,' '+className2+' ');
			    }
			    else{
					r.className = r.className.replace(new RegExp("(?:^|\\s+)" + className2 + "(?:\\s+|$)"),' '+className1+' ');
			    }
			    return r.className;
			};	

			//  Creating our button in JS for smaller screens
			var menuElements = document.getElementById('menu');
			menuElements.insertAdjacentHTML('afterBegin','<button type="button" id="menutoggle" class="navtoogle" aria-hidden="true"><i aria-hidden="true" class="icon-menu"> </span> Menu</button>');

			//  Toggle the class on click to show / hide the menu
			document.getElementById('menutoggle').onclick = function() {
				changeClass(this, 'navtoogle active', 'navtoogle');
			}

			// http://tympanus.net/codrops/2013/05/08/responsive-retina-ready-menu/comment-page-2/#comment-438918
			document.onclick = function(e) {
				var mobileButton = document.getElementById('menutoggle'),
					buttonStyle =  mobileButton.currentStyle ? mobileButton.currentStyle.display : getComputedStyle(mobileButton, null).display;

				if(buttonStyle === 'block' && e.target !== mobileButton && new RegExp(' ' + 'active' + ' ').test(' ' + mobileButton.className + ' ')) {
					changeClass(mobileButton, 'navtoogle active', 'navtoogle');
				}
			}
		</script></div></div> </div></div> </div></div> </div>
        <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script></body></html>