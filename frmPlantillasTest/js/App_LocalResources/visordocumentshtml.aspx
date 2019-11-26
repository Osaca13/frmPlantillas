<%@ Page Language="vb"    %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>

<% 

'*************************************************************************************
' PARAMETRES
'		mp: si = 1 no poso capcelera ni peu per&ograve; poso estils
'		  cami: si existeix el par&agrave;metre poso les molles de pa, la capcelera i el peu (excepte si mp=1 que no posar&eacute; cap i peu)
'			nomollespa=1: No mostrar&eacute; les molles de pa
'		  codiIdioma: Idioma en el que es mostrar&agrave; la info
'		  codiRelacio o relDocs (nom&eacute;s si es crida de llibreriadecodiweb): codi de relaci&oacute; des d'on mostrar&eacute; els docs.
'			senseTags: si nothing poso els <html><body>,etc..
'*************************************************************************************
	Dim est as integer=0
	
	IF 	NOT Request("est")=nothing THEN
		est=Request("est")
	END IF
	
	
	Dim codiRelacio as integer =0
	IF Request("relDocs") is nothing THEN
		IF NOT Request("codiRelacio") is nothing THEN
			codiRelacio=Request("codiRelacio")
		END IF
	ELSE
			est=0
			codiRelacio=Request("relDocs")
	END IF
	
	
	Dim rel as New clsRelacio

	
	Dim txtAfegirDocuments as string=string.empty
	Dim txtAfegirEnllaços as string=string.empty
	Dim txtCercador as string=string.empty
	dim oCodParam as new lhCodParam.lhCodParam
	Dim llistaNodes as string()
	Dim node as string
	Dim contCarpeta as integer=0
	Dim cont as integer=0
	Dim contDocument as integer=0
	Dim dataDocuments As Date 		
	Dim codiIdioma as integer
	Dim objconn as OleDbConnection
	Dim idusuari as integer=-1
	Dim cami as string, item as string, aCami as string(),herenciaCarpeta as string
	Dim DS As DataSet = new dataset, i as integer
	dim nouCami as string
	Dim llistaCarpetes as string=""
	Dim nomCarpeta as string=""
	Dim llistaDocuments as string=""
	Dim llistaCami as string=""
	Dim txtTitol as string=""
	Dim link as string=""
	Dim adreça as string
	Dim nodeCarpeta as integer=0
	Dim target as string="_blank"
	Dim grupsAD as string=""
	Dim usuariXarxa as string=string.empty
	Dim relCarpeta as integer=0
	Dim strRelacionsAmbPermis as string=string.empty
	Dim proves as boolean=false
	
    rel.bdget(objconn, codiRelacio)
	cami=""
	IF NOT Request("cami") is nothing THEN
		cami = Request("cami")
	END IF
	if trim(cami) <> "" then cami &= "|"
	IF codiRelacio<>0 THEN
		cami &= codiRelacio
	END IF	

proves=0

	IF NOT  Request("us") is nothing THEN
		Session("codiOrg")= Request("us")
	END IF

	IF HttpContext.Current.User.Identity.Name.length>0 THEN

		UsuariXarxa= GAIA.FormatejaUsuari(HttpContext.Current.User.Identity.Name)
		
		If proves THEN
			session("codiorg")=nothing
			session("nif")=nothing
			'usuariXarxa="jaguilar"
		END IF
		IF (Session("nif") is nothing) THEN			
			Session("nif") =GAIA.nifUsuari(objconn,UsuariXarxa).Trim()				
		END IF
		IF Session("codiOrg") is nothing THEN			
			session("CodiOrg")=GAIA.trobaNodeUsuari(objConn, Session("nif")).tostring().Trim()
		END IF
		idUsuari = Session("CodiOrg")
		grupsAD = clsPermisos.getGroupsAD(usuariXarxa)
	END IF
'gaia.debug(objconn,grupsAD)
	'
	'
	IF clsPermisos.tepermis(objconn,3,idusuari,idusuari,  rel,0,usuariXarxa,"")=1 THEN			
		txtAfegirDocuments = "<a href=""/gaia/aspx/documents/carregaDocuments.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("cr=" + codiRelacio.ToString())) + """ target=""_blank""><img src=""http://www.l-h.cat/img/afegirdoc.gif"" border=""0""/></a>&nbsp;"
		txtAfegirEnllaços= "<a href=""/gaia/aspx/links/carregalinks.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("cr=" + codiRelacio.ToString())) + """ target=""_blank""><img src=""http://www.l-h.cat/img/afegirenll.gif"" border=""0""/></a>&nbsp;"
	END IF


	txtCercador="<a href=""/GAIA/aspx/buscador/buscador.aspx?" & HttpUtility.Urlencode(oCodParam.encriptar("codiRelacioInicial=" & codiRelacio)) & """ target=""_blank""><img src=""http://www.l-h.cat/img/cercar.gif"" border=""0""/></a>"




IF (Request("senseTags") is nothing) THEN
	IF (NOT Request("cami") is nothing) OR (NOT Request("mp") is nothing)  THEN 

%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

<title>Intranet de l'Ajuntament de L'Hospitalet</title>

<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<link rel="stylesheet" href="/css/gaiaIntranet.css" type="text/css"/>
<link rel="stylesheet" href="/css/intranet.css" type="text/css"/>
</head>
<body bgcolor="#FFFFFF" text="#000000" leftmargin="0" topmargin="0" marginheight="0" marginwidth="0">
<% IF ( Request("mp") is nothing) THEN %>
<style>
.h250 {height:250px}
</style>
<table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td height="60">
				<div class="paddinginf1 w100p fonsVisorDoc">
				<div class="blanc bold t150 gtxt floatleft"><img src="http://www.l-h.cat/img/carpeta_visor.gif" align="absmiddle"/>&nbsp;documents</div>
				<div class="floatright paddingsupinf10"><%=txtAfegirEnllaços%><%=txtAfegirDocuments%><%=txtcercador%>&nbsp;&nbsp;&nbsp;</div>
			</div>
		</td>
	</tr>
	<tr>
		<td valign="top">
<% END IF%>
<%	
	END IF
END IF%>
<%


	objConn = GAIA.bdIni()


	IF Request("nomollespa") is nothing THEN
	
		if trim(cami) <> ""  then
			i=0
			aCami=split(cami,"|")							
			llistaCami+="<div class=""paddingsupinf10""><div class=""t075 paddingsupinf3 fonsGris616161""><div class=""paddingDretaEsquerra10"">"
			nouCami=""
			
			for each item in aCami
				if i > 0 then 
					llistaCami+="<a class=""blanc bold nodeco"">&nbsp;>&nbsp;</a>"	
				END IF
				GAIA.bdr(objConn,"SELECT NODDSTXT FROM METLNOD LEFT JOIN METLREL ON RELINFIL = NODINNOD WHERE RELINCOD=" + item ,ds)
				
				if item <> codiRelacio then
					llistaCami+="<a href=""/gaia/aspx/llibreriacodiweb/documents/visordocumentshtml.aspx?codiRelacio=" & item & "&cami=" & nouCami 
					IF NOT (Request("mp") is nothing) THEN llistaCami+="&mp=1"
						if NOT (request("ed") is nothing) THEN llistaCami+="&ed=1"
						llistaCami+=  """ class=""blanc nodeco"">" + ds.tables(0).rows(0).item(0) + "</a>"
					else
						'actual
						nomCarpeta= ds.tables(0).rows(0).item(0)
						llistaCami+="<a href=""#"" class=""blanc bold nodeco"">" & nomCarpeta & "</a></div></div></div>"
					end if
				if trim(nouCami) <> "" then nouCami &= "|"
				nouCami &= item
	
				i += 1
			next item
		end if 
	END IF

IF Request("codiIdioma") is Nothing THEN
	codiIdioma = 1
ELSE
	codiIdioma = Request("codiIdioma")
END IF

datadocuments = DateAdd("d",-7,Now)

	Dim dbRow As DataRow   

	Dim relTMP as New clsRelacio
	DS = New DataSet()			
	
	
'***************************************************************************
' Tracto el primer nivell
'***************************************************************************	
	'Busco carpetes
	
	contCarpeta=0
	strRelacionsAmbPermis=  clsPermisos.FillsRelacioAmbPermis(objConn, 9,idUsuari, idUsuari,  rel, 0, usuariXarxa, grupsAD) 
	IF strRelacionsAmbPermis.length>0 THEN
	IF est<>0 THEN
		GAIA.BDR(objconn, "SELECT RELINPAR,RELINCOD,RELCDHER,RELINFIL,NODDSTXT,TIPDSIMG FROM METLREL  WITH(NOLOCK),METLNOD  WITH(NOLOCK),METLTIP  WITH(NOLOCK) WHERE METLREL.RELINCOD IN ("+strRelacionsAmbPermis + ")  AND RELCDSIT in (1,3) AND NODCDTIP in (35,47) AND NODINNOD=RELINFIL AND TIPINTIP=NODCDTIP AND RELCDEST =" + est.tostring() + " AND RELSWVIS=1 ORDER BY RELCDORD ",DS)
		

	ELSE
		GAIA.BDR(objconn, "SELECT RELINPAR,RELCDHER,RELINFIL,RELINCOD,NODDSTXT,TIPDSIMG FROM METLREL WITH(NOLOCK),METLNOD  WITH(NOLOCK),METLTIP   WITH(NOLOCK) WHERE METLREL.RELINCOD IN ("+strRelacionsAmbPermis + ")  AND RELCDSIT in (1,3) AND NODCDTIP in (35,47) AND NODINNOD=RELINFIL AND TIPINTIP=NODCDTIP  AND RELSWVIS=1 ORDER BY RELCDORD ",DS)
	

	END IF

	For each dbRow in ds.tables(0).Rows		

		'relTMP.bdget(objconn,dbrow("RELINCOD"))

	'IF clsPermisos.tepermis(objconn,9,idusuari,idusuari,  relTMP,0,usuariXarxa,"") THEN
			
			
			llistaCarpetes+="<div class=""clearboth paddingEsquerra5 paddingsupinf2"">"
			llistaCarpetes+="<img src=""/img/common/iconografia/"+dbROW("TIPDSIMG")+""" class=""floatleft paddingDreta2 printhide""/><a href=""/gaia/aspx/llibreriacodiweb/documents/visordocumentshtml.aspx?codiRelacio="+dbRow("RELINCOD").tostring()+"&cami=" & cami
			IF NOT (Request("mp") is nothing) THEN llistaCarpetes+="&mp=1"
			if NOT (request("ed") is nothing) THEN llistaCarpetes+="&ed=1"
						
			llistaCarpetes += """ class=""gtxt t075 bold negre nodeco floatleft esq paddingsup"">"+dbRow("NODDSTXT")+"</a>"
			llistaCarpetes+="</div>"	
			herenciaCarpeta= dbRow("RELCDHER")+"_"+dbROW("RELINFIL").tostring()
			nodeCarpeta=dbRow("RELINFIL")
			contCarpeta+=1
			nomCarpeta=dbRow("NODDSTXT")
			relCarpeta=dbrow("RELINCOD")
'END IF
	Next
	END IF
	'Busco documents
	contDocument=0
	
	strRelacionsAmbPermis=  clsPermisos.FillsRelacioAmbPermis(objConn, 9,idUsuari, idUsuari,  rel, 0, usuariXarxa, grupsAD) 
	IF strRelacionsAmbPermis.length>0 THEN
		IF est<>0 THEN		
			GAIA.BDR(objconn, "SELECT  DOCINIDI,METLREL.RELCDORD, METLREL.RELINCOD, NODDSTXT,DOCINNOD, isNull(DOCDSFIT,'') as DOCDSFIT, DOCDSTIT, DOCDSDES,  TDODSIMG FROM METLREL INNER JOIN METLNOD ON METLREL.RELINFIL = METLNOD.NODINNOD INNER JOIN METLDOC ON METLREL.RELINFIL = METLDOC.DOCINNOD  INNER JOIN METLTDO ON METLDOC.DOCINTDO = METLTDO.TDOCDTDO WHERE METLREL.RELINCOD IN (" + strRelacionsAmbPermis+") AND METLREL.RELCDSIT IN (1, 3) AND (METLNOD.NODCDTIP = 5) AND RELCDEST="+est.tostring()+" AND RELSWVIS=1 ORDER BY METLREL.RELCDORD",DS)
		
		ELSE
			GAIA.BDR(objconn, "SELECT  DOCINIDI,METLREL.RELCDORD, METLREL.RELINCOD, NODDSTXT,DOCINNOD,  isNull(DOCDSFIT,'') as DOCDSFIT, DOCDSTIT, DOCDSDES, TDODSIMG FROM METLREL INNER JOIN METLNOD ON METLREL.RELINFIL = METLNOD.NODINNOD INNER JOIN METLDOC ON METLREL.RELINFIL = METLDOC.DOCINNOD INNER JOIN METLTDO ON METLDOC.DOCINTDO = METLTDO.TDOCDTDO WHERE METLREL.RELINCOD IN (" + strRelacionsAmbPermis+") AND METLREL.RELCDSIT IN (1, 3) AND (METLNOD.NODCDTIP = 5)  AND RELSWVIS=1  ORDER BY METLREL.RELCDORD",DS)
			
		END IF	
		
		'busco quines documents es poden editar
		Dim llistaPendents as string="",llistaambpermisos as string="", llistasensepermisos as string=""
		
		For each dbRow in ds.tables(0).Rows
			clsPermisos.afegirElement(dbrow("RELINCOD"), llistaPendents)						
		Next dbrow
		clsPermisos.trobaPermisLlistaRelacions(objconn, 3, llistaPendents, idusuari, usuariXarxa, 0, llistaambpermisos, llistasensepermisos)
		

		Dim docant as integer=0
		For each dbRow in ds.tables(0).Rows		
			IF docAnt<>dbrow("DOCINNOD") THEN
				docAnt = dbrow("DOCINNOD")
				relTMP.bdGet(objconn,dbrow("RELINCOD"))
				llistaDocuments+="<div class=""clearboth paddingEsquerra5 paddingsupinf"">"	
			
				IF instr(dbrow("DOCDSFIT"),".mpg")>0  OR (instr(dbrow("DOCDSFIT"),".wm")>0) THEN	
					llistaDocuments+="<a href="""" onclick=""window.open('/gaia/aspx/documents/visorvideos.aspx?v="+dbRow("DOCDSFIT")+"','_blank','width=640,height=570');return false;"" class=""gtxt t075 negre"" target=""_blank"" valign=""middle""><img src=""/img/common/iconografia/"+dbROW("TDODSIMG")+""" border=""0"" target=""_blank"" align=""absmiddle"" class=""printhide""/>&nbsp;"+dbRow("DOCDSTIT")+"</a>"
				ELSE
					adreça="/utils/obreFitxer.ashx?"+HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" + dbrow("DOCINNOD").tostring() + "&codiIdioma=1"))
					llistaDocuments += "<a href=""" + adreça + """ class=""gtxt t075 negre"" target=""_blank"" valign=""middle""><img src=""/img/common/iconografia/" + dbRow("TDODSIMG") + """ border=""0"" target=""_blank"" align=""absmiddle"" class=""printhide""/>&nbsp;" + dbRow("NODDSTXT") + "</a>&nbsp;"
				
				'desactivo l'opció d'editar continguts

					IF   not ( Request("ed") is nothing) THEN 
						IF clsPermisos.existeixElement(dbrow("RELINCOD"), llistaambpermisos) THEN
'						IF clsPermisos.tepermis(objconn,3,idusuari,idusuari,  relTMP,0,usuariXarxa,"") THEN	
							llistaDocuments +="<a href=""/gaia/aspx/documents/carregaDocuments.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("id=" + dbRow("DOCINNOD").ToString() + "&idiarbre=" + dbRow("DOCINIDI").ToString())) + """ target=""_blank""><img src=""http://www.l-h.cat/img/boton_editar.gif"" border=""0"" align=""absmiddle"" style="" alt=""Editar""/></a>"
						END IF
					END IF
				END IF		
				llistaDocuments+="</div>"	
				contDocument+=1
			END IF
		Next
	
		'Tamb&eacute; afegeixo la llista de p&agrave;gines web	
		IF est<>0 THEN
			GAIA.bdR(objconn,"SELECT   relContingut.RELINCOD as relacio,METLLNK.LNKWNTIP,METLLNK.LNKDSLNK,METLLNK.LNKDSTXT, relContingut.RELDSFIT, METLREI.REIDSFIT FROM   METLREL relMenu LEFT OUTER JOIN METLLNK ON relMenu.RELINFIL = METLLNK.LNKINNOD LEFT OUTER JOIN METLREI ON METLLNK.LNKCDREL = METLREI.REIINCOD AND METLREI.REIINIDI ="+ codiIdioma.tostring()+" LEFT OUTER JOIN METLREL relContingut ON METLLNK.LNKCDREL = relContingut.RELINCOD WHERE (relMenu.RELINPAR= "+ rel.infil.tostring()+" ) AND (METLLNK.LNKINIDI ="+ codiIdioma.tostring()+") AND (relMenu.RELCDSIT <98)  AND relMenu.RELCDEST =" + est.tostring() + " ORDER BY relMenu.RELCDORD" ,DS)		
		ELSE
			GAIA.bdR(objconn,"SELECT     relContingut.RELINCOD as relacio,METLLNK.LNKINIDI, METLLNK.LNKINNOD, METLLNK.LNKWNTIP,METLLNK.LNKDSLNK,METLLNK.LNKDSTXT, relContingut.RELDSFIT, METLREI.REIDSFIT FROM   METLREL relMenu LEFT OUTER JOIN METLLNK ON relMenu.RELINFIL = METLLNK.LNKINNOD LEFT OUTER JOIN METLREI ON METLLNK.LNKCDREL = METLREI.REIINCOD AND METLREI.REIINIDI ="+ codiIdioma.tostring()+" LEFT OUTER JOIN METLREL relContingut ON METLLNK.LNKCDREL = relContingut.RELINCOD WHERE (relMenu.RELINPAR= "+ rel.infil.tostring()+" ) AND (METLLNK.LNKINIDI ="+ codiIdioma.tostring()+") AND (relMenu.RELCDSIT <98)   ORDER BY relMenu.RELCDORD" ,DS)	
		END IF	
		For each dbRow in ds.tables(0).Rows			
	
			link= iif(isdbnull(dbrow("REIDSFIT")),iif(isdbnull(dbRow("RELDSFIT")),dbROW("LNKDSLNK"),dbrow("RELDSFIT")),dbrow("REIDSFIT").toString())
			target=iif(isdbnull(dbrow("LNKWNTIP")),"_self",iif(dbrow("LNKWNTIP")=0,"_self","_blank"))
			IF Instr(link, "_"+codiIdioma.tostring()+".")>0 OR isdbnull(dbrow("RELDSFIT"))  THEN			
				IF Instr(link,".asp") AND Instr(link,"id=")<0 THEN
					IF Instr(link,"?") THEN
						link+= "&id="+codiIdioma.tostring()
					ELSE
						link+="?id="+codiIdioma.tostring()
					END IF
				END IF
				llistaDocuments+="<div class=""paddingsupinf1 paddingEsquerra5 esq clearboth"">"
	'			llistaDocuments+="<div class=""paddingSup2Dre5Esq15 esq"">"
				llistaDocuments+="<div class=""floatleft t075 paddingEsquerra5 "" align=""absmiddle""><img src=""/img/common/iconografia/ic_html.gif"" border=""0"" target=""_blank"" align=""absmiddle"" class=""printhide""/>&nbsp;</div><div class=""textneg paddingBottom1"" align=""absmiddle""><a href="""+link.REplace("GAIA/","")+""" class=""nodeco over negre bold t075 floatleft"" target="""+target+""">"+dbROW("LNKDSTXT")+"</a>"	
					IF   not ( Request("ed") is nothing) THEN 
						IF NOT isdbNull(dbrow("relacio")) THEN
							relTMP.bdGet(objconn,dbrow("relacio"))
	
							IF clsPermisos.tepermis(objconn,3,idusuari,idusuari,  relTMP,0,usuariXarxa,"") THEN
								llistaDocuments +="<a href=""/gaia/aspx/links/carregalinks.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("id=" + dbRow("LNKINNOD").ToString() + "&idiarbre=" + dbRow("LNKINIDI").ToString())) + """ target=""_blank"" align=""absmiddle""><img src=""http://www.l-h.cat/img/boton_editar.gif"" border=""0"" align=""absmiddle"" style=""alt=""Editar""/></a>"
							END IF
						END IF
					END IF
				llistaDocuments+="</div></div>"		
				contDocument+=1	
			END IF
		Next	

	
	'**********************************************************************************************************
	'Si no trobo documents al primer nivell i nom&eacute;s hi ha una carpeta, poso els documents de dins de la carpeta
	'**********************************************************************************************************
		If contDocument=0 and contCarpeta=1 THEN 
	
		'Poso el nom de la carpeta		
		txtTitol+="<div class=""paddingEsquerra10 blanc gtxt t075 bold fonsgris70 mayusculas marginsupinf10 paddingsupinf5"">" & nomCarpeta & "</div>"		
		llistaCarpetes=""				
		contCarpeta=0

		rel.bdget(objconn,relCarpeta)
		strRelacionsAmbPermis=  clsPermisos.FillsRelacioAmbPermis(objConn, 9,idUsuari, idUsuari,  rel, 0, usuariXarxa, grupsAD) 
		IF strRelacionsAmbPermis.length>0 THEN	
		
			GAIA.BDR(objconn, "SELECT  DOCINIDI,METLREL.RELCDORD, METLREL.RELINCOD, NODDSTXT,  isNull(DOCDSFIT,'') as DOCDSFIT,DOCDSTIT, DOCWNSIZ, DOCDTANY,  TDODSIMG, TIPDSIMG, DOCINNOD, DOCDSDES FROM METLREL INNER JOIN METLNOD ON METLREL.RELINFIL = METLNOD.NODINNOD LEFT OUTER JOIN METLTIP ON METLTIP.TIPINTIP=METLNOD.NODCDTIP LEFT OUTER JOIN METLDOC ON METLREL.RELINFIL = METLDOC.DOCINNOD AND METLDOC.DOCINIDI=1 LEFT OUTER JOIN METLTDO ON METLDOC.DOCINTDO = METLTDO.TDOCDTDO WHERE METLREL.RELINCOD IN  ("+ strRelacionsAmbPermis+") AND (METLREL.RELCDSIT IN (1, 3)) AND (METLNOD.NODCDTIP IN (5,35,47)) AND RELSWVIS=1 ORDER BY METLREL.RELCDORD",DS)

			For each dbRow in ds.tables(0).Rows		
				llistaDocuments+="<div class=""clearboth paddingsupinf"">"		
				IF NOT dbROW("DOCDSFIT")="" THEN
					IF instr(dbrow("DOCDSFIT"),".mp")>0  OR (instr(dbrow("DOCDSFIT"),".wmv")>0) THEN	
						llistaDocuments+="<div class=""floatleft t075 paddingEsquerra5""><img src=""/img/common/iconografia/"+dbROW("TDODSIMG")+""" border=""0"" target=""_blank"" align=""absmiddle""/>&nbsp;</div><div class=""floatleft paddingBottom5""><a href="""" onclick=""window.open('/gaia/aspx/documents/visorvideos.aspx?v="+dbRow("DOCDSFIT")+"','_blank','width=640,height=570'); return false;"" class=""gtxt t075 negre left"" target=""_blank"" valign=""middle"">"+dbRow("DOCDSTIT")+"&nbsp;&nbsp;</a><!--<span class=""t60 gtxt vermell"">("+dbROW("DOCDTANY")+")</span>--></div>"			
					ELSE
					adreça="/utils/obreFitxer.ashx?"+HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" + dbrow("DOCINNOD").tostring() + "&codiIdioma=1"))
	
					
					llistaDocuments += "<div class=""floatleft t075 paddingEsquerra5""><img src=""/img/common/iconografia/" + dbRow("TDODSIMG") + """ border=""0"" target=""_blank"" align=""absmiddle"" class=""printhide""/>&nbsp;</div><div class=""floatleft paddingBottom5""><a href=""" + adreça + """ class=""gtxt t075 negre left"" target=""_blank"" valign=""middle"">" + dbRow("NODDSTXT") + "&nbsp;&nbsp;</a>&nbsp;"
					
						IF   not ( Request("ed") is nothing) THEN 
							relTmp.bdget(objconn,dbrow("RELINCOD"))
							IF clsPermisos.tepermis(objconn,3,idusuari,idusuari,  relTMP,0,usuariXarxa,"") THEN
								llistaDocuments +=	"<a href=""/gaia/aspx/documents/carregaDocuments.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("id=" + dbRow("DOCINNOD").ToString() + "&idiarbre=" + dbRow("DOCINIDI").ToString())) + """ target=""_blank""><img src=""http://www.l-h.cat/img/boton_editar.gif"" border=""0"" align=""absmiddle"" alt=""Editar"" /></a><!--<span class=""t60 gtxt vermell"">(" + dbRow("DOCDTANY") + ")</span>-->"
							END IF
						END IF
						llistadocuments+="</div>"
				
					END IF
				ELSE
					llistaDocuments+="<div class=""clearboth paddingEsquerra5"">"					
					llistaDocuments+= "<a href=""/gaia/aspx/llibreriacodiweb/documents/visordocumentshtml.aspx?codiRelacio="+dbRow("RELINCOD").tostring()+"&cami=" & cami
					if NOT (request("ed") is nothing) THEN llistaDocuments+="&ed=1"
					IF NOT (Request("mp") is nothing) THEN llistaDocuments+="&mp=1"
					llistaDocuments += """ class=""gtxt t075 bold negre nodeco paddingEsquerra5""><img src=""/img/common/iconografia/"+dbROW("TIPDSIMG")+""" border=""0""  align=""absmiddle""/>&nbsp;"+dbRow("NODDSTXT")+"</a>"
					llistaDocuments+="</div>"	
					herenciaCarpeta= dbRow("RELCDHER")+"_"+dbROW("RELINFIL").tostring()
					contCarpeta+=1			
				END IF		
				llistaDocuments+="</div>"
				contDocument+=1
			Next
		
	'Tamb&eacute; afegeixo la llista de p&agrave;gines web

		GAIA.bdR(objconn,"SELECT     METLLNK.LNKWNTIP, METLLNK.LNKDSLNK,METLLNK.LNKDSTXT, relContingut.RELDSFIT, METLREI.REIDSFIT, relMenu.RELINCOD as relacio FROM   METLREL relMenu LEFT OUTER JOIN METLLNK ON relMenu.RELINFIL = METLLNK.LNKINNOD LEFT OUTER JOIN METLREI ON METLLNK.LNKCDREL = METLREI.REIINCOD AND METLREI.REIINIDI ="+ codiIdioma.tostring()+" LEFT OUTER JOIN METLREL relContingut ON METLLNK.LNKCDREL = relContingut.RELINCOD WHERE relMenu.RELINCOD in  ("+ strRelacionsAmbPermis +" ) AND (METLLNK.LNKINIDI ="+ codiIdioma.tostring()+") AND (relMenu.RELCDSIT<98) ORDER BY relMenu.RELCDORD" ,DS)	
		For each dbRow in ds.tables(0).Rows			
			relTMP.bdGet(objconn,dbrow("relacio"))

			link= iif(isdbnull(dbrow("REIDSFIT")),iif(isdbnull(dbRow("RELDSFIT")),dbROW("LNKDSLNK"),dbrow("RELDSFIT")),dbrow("REIDSFIT").toString())
			target=iif(isdbnull(dbrow("LNKWNTIP")),"_self",iif(dbrow("LNKWNTIP")=0,"_self","_blank"))
				IF Instr(link, "_"+codiIdioma.tostring()+".")>0 OR isdbnull(dbrow("RELDSFIT"))  THEN			
					IF Instr(link,".asp") AND Instr(link,"id=")<0 THEN
						IF Instr(link,"?") THEN
							link+= "&id="+codiIdioma.tostring()
						ELSE
							link+="?id="+codiIdioma.tostring()
						END IF
					END IF		
					llistaDocuments+="<div class=""clearboth paddingsupinf"">"
					llistaDocuments+="<div class=""floatleft t075 paddingEsquerra5""><img src=""/img/common/iconografia/ic_html.gif"" border=""0"" target=""_blank"" align=""absmiddle"" class=""printhide""/>&nbsp;</div><div class=""textneg paddingBottom1 floatleft""><a href="""+link.REplace("GAIA/","")+""" class=""nodeco over negre t075"" target="""+target+""">"+dbROW("LNKDSTXT")+"</a></div>"			
					llistaDocuments+="</div>"
					contDocument+=1	
				END IF
		Next	
		END IF	
	END IF
	END IF
	ds.dispose()	

	GAIA.bdFi(objConn)


	Response.write(llistaCami)
	Response.write(txtTitol)
	Response.write(llistaCarpetes)
	Response.Write(llistaDocuments)	
%>

<%IF (Request("senseTags") is nothing) THEN
  IF (NOT Request("cami") is nothing) AND ( Request("mp") is nothing)  THEN 
%>
<!--#INCLUDE VIRTUAL="/inc/peuDocuments.inc" -->
</body>
</html>


<% END IF
END IF
oCodParam=nothing		
%>
