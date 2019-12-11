<%@ Import Namespace="System.Data"  %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="lhCodParam" %>
<%@ Control Language="vb" AutoEventWireup="false" debug="true" %>
<%@ OutputCache Duration="100" VaryByParam="none" VaryByCustom="userName" %>
<asp:panel runat="server" id="pnlMenuGaia" visible="false">
<script type="text/javascript">

sfHover = function() {
	var sfEls = document.getElementById("navegacio").getElementsByTagName("li");
	for (var i=0; i<sfEls.length; i++) {
		sfEls[i].onmouseover=function() {
			this.className+=" sfhover";
		}
		sfEls[i].onmouseout=function() {
			this.className=this.className.replace(new RegExp(" sfhover\\b"), "");
		}
	}
}
if (window.attachEvent) window.attachEvent("onload", sfHover);
//--><!]]></script>
<script type="text/javascript">
<!--
	function esborrar(e) {
		if (e.value=="text a cercar" || e.value=="texto a buscar")  {
			e.value = "";
		}
	}
	function canviEstil(id, nouCSS) {
		identity=document.getElementById(id);
		identity.className=nouCSS;
	}
//-->
</script>
<link href="../../Styles/black.css" rel="stylesheet" type="text/css" />
<link href="../../Styles/blue.css" rel="stylesheet" type="text/css" />
<link href="../../Styles/orange_light.css" rel="stylesheet" type="text/css" />
<link href="../../Styles/red.css" rel="stylesheet" type="text/css" />
<link href="../../Styles/green.css" rel="stylesheet" type="text/css" />
<link href="../../Styles/light_blue.css" rel="stylesheet" type="text/css" />
<link href="../../Styles/grey.css" rel="stylesheet" type="text/css" />
<link href="../../Styles/white.css" rel="stylesheet" type="text/css" />
<link href="../../Styles/dcmegamenu.css" rel="stylesheet" type="text/css" />
  <script src="js/jquery-1.10.2.js"></script>
<script type='text/javascript' src="js/jquery.hoverIntent.minified.js"></script>
<script type='text/javascript' src="js/jquery.dcmegamenu.1.3.3.js"></script>
<script type="text/javascript">
$(document).ready(function($){
	$('#mega-menu-2').dcMegaMenu({
		rowItems: '3',
		speed: 'fast',
		effect: 'fade'
	});	
});
</script>
<div> <!-- hem tret: class="wrap" -->
<div class="demo-container">
<div class="white">
<ul id="mega-menu-2" class="mega-menu">	  
    <li>
        <a href="../../VisorArbres.aspx?arbre1=elMeuEspai" >Arbres GAIA</a>
    </li>
	<asp:literal runat="server" id="ltMenuContinguts"></asp:literal>
		<asp:PlaceHolder id="pnlAdm" runat="server" visible="false">
    <li><a href="#">Administració</a>    	
        <ul>   
           <asp:literal runat="server" id="ltMenuDissenyWeb"></asp:literal>
           <asp:literal runat="server" id="ltMenuGestio"></asp:literal>
           <asp:literal runat="server" id="ltMenuLlistats"></asp:literal>
		   <asp:literal runat="server" id="ltMenuBackoffice"></asp:literal>
         </ul>                                    
		</li>
    </asp:PlaceHolder>
    <asp:literal runat="server" id="ltUtilitats"></asp:literal>         
    <asp:literal runat="server" id="ltCercadors"></asp:literal>        
    <li class="logoGaia"><a href="../../232511_1.aspx" title="Anar a inici" class="logo"><img src="img/logo_gaia.png" class="border0"/></a></li>
</ul>
</div>
</div>
</div>
</asp:panel>
<script runat="server">
    Dim objconn as OleDbConnection
    Dim usuariXarxa as string=string.empty
    Dim grupsAD as string=""
    Dim idusuari as integer=-1



    public  function GetVaryByCustomString( context as HttpContext ,  arg as string) as string
        if (arg = "userName") THEN
            return context.User.Identity.Name
        END IF
        return string.Empty
    end function


    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        Dim usuariActiu as boolean= False
        IF session("menuGAIA")="" Then
            'IF HttpContext.Current.User.Identity.Name.length>0 THEN
            '    UsuariXarxa= GAIA.FormatejaUsuari(HttpContext.Current.User.Identity.Name)



            If (Session("nif") Is Nothing) Then
                Session("nif") = GAIA.nifUsuari(objconn, usuariXarxa, usuariActiu).Trim()
            End If

            If True Then 'usuariActiu THEN

                If Session("codiOrg") Is Nothing Then
                    Session("CodiOrg") = GAIA.trobaNodeUsuari(objconn, Session("nif")).ToString().Trim()
                End If
                'idUsuari = Session("CodiOrg")
                'grupsAD = clsPermisos.getGroupsAD(usuariXarxa)
                idusuari = "346231"
                Session("codiOrg") = "346231"

                pnlMenuGaia.Visible = True


                Dim ds As DataSet
                ds = New DataSet()
                ds = llistaRelacionsAmbPermis()
                'ltMenuArbres.text = menuArbres(ds)
                ltMenuContinguts.Text = menuLinks(231018, 1, ds)
                ltMenuGestio.Text = menuLinks(231023, 1, ds)
                ltMenuDissenyWeb.Text = menuLinks(231022, 1, ds)
                ltMenuLlistats.Text = menuLinks(231024, 1, ds)
                ltMenuBackoffice.Text = menuLinks(270711, 1, ds)



                If (idusuari = 49730 Or idusuari = 49727 Or idusuari = 56935 Or idusuari = 80879 Or idusuari = 144886 Or idusuari = 156704 Or idusuari = 297650 Or idusuari = 313486 Or idusuari = 346231) And (ltMenuGestio.Text.Length > 0 Or ltMenuDissenyWeb.Text.Length > 0 Or ltMenuLlistats.Text.Length > 0 Or ltMenuBackoffice.Text.Length > 0) Then

                    pnlAdm.Visible = True
                Else

                    pnlAdm.Visible = False
                End If


                ltUtilitats.Text = menuLinks(231021, 1, ds)



                ds.Dispose()
            End If

        End If
        'End IF
        'END IF
    End sub


    Protected Function llistaRelacionsAmbPermis() as dataset


        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New dataset()

        Dim rel as new clsRelacio

        Dim llistaPendents as string=""
        Dim llistaAmbPermisos as string=""
        Dim llistaSensePermisos as string=""
        GAIA.bdR(objConn, "select * FROM METLREL WHERE (RELCDHER LIKE '%_163944%' ) AND RELCDSIT<96", DS)
        For Each dbRow In ds.Tables(0).Rows
            if llistaPendents.length>0 THEN
                llistaPendents &= ","
            END IF
            llistaPendents &= dbrow("RELINCOD")
        Next dbRow
        clsPermisos.trobaPermisLlistaRelacions(objconn, 9, llistaPendents, idUsuari, grupsAD, "", llistaAmbPermisos, llistaSensePermisos, usuariXarxa)
        if llistaAmbPermisos.length=0 THEN
            llistaAmbPermisos="-1"
        END IF
        GAIA.bdR(objconn,"SELECT DISTINCT  len(relMenu.RELCDHER),relMenu.RELCDHER,isnull(METLREI.REIINCOD, 0) AS REIINCOD,nodeMenu.NODDSTXT as nomMenu, nodePareMenu.NODDSTXT as nomPare, relMenu.RELINFIL, relMenu.RELINPAR, relMenu.RELCDRSU, relMenu.RELINCOD as relacio, LNKINNOD, METLLNK.LNKWNTIP, METLLNK.LNKDSLNK,METLLNK.LNKDSTXT, relContingut.RELDSFIT, METLREI.REIDSFIT, relMenu.RELCDORD FROM METLREL relMenu WITH(NOLOCK) INNER JOIN METLNOD nodePareMenu WITH(NOLOCK) on nodePareMenu.NODINNOD=relMenu.RELINPAR INNER JOIN METLNOD nodeMenu WITH(NOLOCK) on nodeMenu.NODINNOD=relMenu.RELINFIL  LEFT OUTER JOIN METLLNK WITH(NOLOCK) ON relMenu.RELINFIL = METLLNK.LNKINNOD AND (METLLNK.LNKINIDI =1) LEFT OUTER JOIN METLREI WITH(NOLOCK) ON METLLNK.LNKCDREL = METLREI.REIINCOD AND METLREI.REIINIDI =1 LEFT OUTER JOIN METLREL relContingut WITH(NOLOCK) ON METLLNK.LNKCDREL = relContingut.RELINCOD  AND relContingut.RELCDSIT<98 WHERE   relMenu.RELINCOD IN (" & llistaAmbPermisos & " )  AND (relMenu.RELCDSIT< 98) ORDER BY len(relMenu.RELCDHER), relMenu.RELCDORD" ,DS)


        return (ds)


    End Function


    Protected Function menuArbres(ds as dataset) as String
        Dim menuArbresTemp As String = String.Empty

        For Each dbRow In ds.Tables(0).Rows
            IF dbrow("RELINFIL")=dbrow("RELINPAR") Then
                menuArbresTemp &= "<li><a href=""~/VisorArbres.aspx?ca=" & dbRow("relacio") & """ title=""Anar a " & dbRow("nomMenu") & """>" & dbRow("nomMenu").replace("arbre ", "") & "</a></li>"
            End IF
        Next dbRow

        DS.dispose()
        Return menuArbresTemp
    End Function


    Private function menuLinks(byVal codiRelacio as integer, byVal codiIdioma as integer, byVal llistaAmbPermisos as dataset) as string
        Dim DS As DataSet
        Dim dbRow as DataRow
        DS = New DataSet()
        Dim strMenu as string=""
        Dim strLink as string
        Dim txtNovaFinestra as string = "nova finestra"
        Dim cont as integer = 0
        Dim strImatgeFinestraNova As String = " <img class=""finestraNova"" alt=""Obrir enllaç"" src=""img/finestra_nova.png""/>"
        Dim rel as new clsrelacio()

        rel.bdget(objconn, codiRelacio)

        For each dbRow in llistaAmbPermisos.tables(0).Rows
            IF (instr(dbrow("RELCDHER"), rel.infil)>0) AND NOT isdbnull(dbrow("LNKINNOD")) THEN
                if cont=0 THEN
                    strMenu &= "<li><a href=""#"">" & dbrow("nomPare") & "</a><ul>"
                END IF
                strLink= iif(isdbnull(dbrow("REIDSFIT")) OR dbrow("REIINCOD")=0,iif(isdbnull(dbRow("RELDSFIT")),dbROW("LNKDSLNK"),dbrow("RELDSFIT")),dbrow("REIDSFIT"))
                strMenu &= "<li><a href=""" & strLink & """ target=""" & iif(dbrow("LNKWNTIP")=1, "_blank", "_self") & """ title=""Anar a " & dbrow("LNKDSTXT") & iif(dbrow("LNKWNTIP")=1, txtNovaFinestra,"") & """>"  & dbrow("LNKDSTXT") & iif(dbrow("LNKWNTIP")=1, strImatgeFinestraNova,"") & "</a>"  & "</li>"
                cont = cont + 1
            END IF
        Next dbrow

        if cont>0 THEN
            strMenu &= "</ul></li>"
            'strMenu =  iif(cont>1, "<ul><li>","") & strMenu  & iif(cont>1, "</li></ul>","") 


        END IF

        return strMenu

    End function
</script>