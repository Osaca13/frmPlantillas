<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="visordocumentshtml.ascx.vb" Inherits="frmPlantillasTest.visordocumentshtml" %>

<%@ Import Namespace="System.Data"  %>
<%@ Import Namespace="System.Data.OleDb" %>
<asp:Label id="lblDocs" runat="server"/>
<%
	Dim resultat as string=""
	Dim codiRelacio as integer =0
	Dim est as integer=0

		
	codiRelacio=session("sesCodiRelacio")
	
	Dim ambDataDescripcio as boolean = false
	
	IF session("ambDataDescripcio")=true THEN
		ambDataDescripcio=true
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
	Dim adreça as string=""

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
	'gaia.debug(objconn,codirelacio & ".. " & Now)
	IF HttpContext.Current.User.Identity.Name.length>0 THEN

		UsuariXarxa= GAIA.FormatejaUsuari(HttpContext.Current.User.Identity.Name)
		
		If proves THEN
			session("codiorg")=nothing
			session("nif")=nothing
		
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

	
	'Response.write(grupsAD)
	'Response.write(usuariXarxa+"-"+session("nif")+"-"+session("codiorg") & idUsuari)
	IF clsPermisos.tepermis(objconn,3,idusuari,idusuari,  rel,0,usuariXarxa,"")=1 THEN			
		txtAfegirDocuments = "<a href=""/gaia/aspx/documents/carregaDocuments.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("cr=" + codiRelacio.ToString())) + """ target=""_blank""><img src=""http://www.l-h.cat/img/afegirdoc.gif"" alt="""" border=""0""/></a>&nbsp;"
		txtAfegirEnllaços= "<a href=""/gaia/aspx/links/carregalinks.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("cr=" + codiRelacio.ToString())) + """ target=""_blank""><img src=""http://www.l-h.cat/img/afegirenll.gif"" alt="""" border=""0""/></a>&nbsp;"
	END IF


	txtCercador="<a href=""/GAIA/aspx/buscador/buscador.aspx?"+HttpUtility.UrlEncode(oCodParam.encriptar("codiRelacioInicial=" + codiRelacio.tostring() ))+""" target=""_blank""><img src=""http://www.l-h.cat/img/cercar.gif"" border=""0"" alt=""""/></a>"




IF (Request("senseTags") is nothing) THEN
	IF (NOT Request("cami") is nothing) OR (NOT Request("mp") is nothing)  THEN 

%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

<title>Intranet de l'Ajuntament de L'Hospitalet</title>

<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<link rel="stylesheet" href="../../Styles/gaiaIntranet.css" type="text/css"/>
<link rel="stylesheet" href="../../Styles/intranet.css" type="text/css"/>

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
				<div class="blanc bold t150 arial floatleft"><img src="http://www.l-h.cat/img/carpeta_visor.gif" align="absmiddle"/>&nbsp;documents</div>
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
			llistaCami+="<div class=""displayBlock marginBottom10""><div class=""t09 padding5 arial fonsGris616161""><div class=""paddingDretaEsquerra10"">"
			nouCami=""
			
			for each item in aCami
				if i > 0 then 
					llistaCami+="<a class=""blanc bold nodeco"">&nbsp;>&nbsp;</a>"	
				END IF
				GAIA.bdr(objConn,"SELECT NODDSTXT FROM METLNOD with(NOLOCK) LEFT JOIN METLREL with(NOLOCK)  ON RELINFIL = NODINNOD WHERE RELINCOD=" + item ,ds)
				
				if item <> codiRelacio then
					llistaCami+="<a href=""/gaia/aspx/llibreriacodiweb/documents/visordocumentshtml.aspx?codiRelacio=" & item & "&cami=" & nouCami 
					IF NOT (Request("mp") is nothing) THEN llistaCami+="&mp=1"
						if NOT (Session("ed") is nothing) THEN llistaCami+="&ed=1"
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

	'gaia.debug(objconn, "2" & Now) 
	IF strRelacionsAmbPermis.length>0 THEN
	IF est<>0 THEN
		GAIA.BDR(objconn, "SELECT RELINPAR,RELINCOD,RELCDHER,RELINFIL,NODDSTXT,TIPDSIMG FROM METLREL with(NOLOCK) ,METLNOD with(NOLOCK) ,METLTIP with(NOLOCK)  WHERE METLREL.RELINCOD IN ("+strRelacionsAmbPermis + ")  AND RELCDSIT in (1,3) AND NODCDTIP in (35,47) AND NODINNOD=RELINFIL AND TIPINTIP=NODCDTIP AND RELCDEST =" + est.tostring() + " ORDER BY RELCDORD ",DS)
	ELSE
		GAIA.BDR(objconn, "SELECT RELINPAR,RELCDHER,RELINFIL,RELINCOD,NODDSTXT,TIPDSIMG FROM METLREL with(NOLOCK) ,METLNOD with(NOLOCK) ,METLTIP with(NOLOCK)  WHERE METLREL.RELINCOD IN ("+strRelacionsAmbPermis + ")  AND RELCDSIT in (1,3) AND NODCDTIP in (35,47) AND NODINNOD=RELINFIL AND TIPINTIP=NODCDTIP ORDER BY RELCDORD ",DS)
	END IF

	For each dbRow in ds.tables(0).Rows		
			llistaCarpetes+="<div class=""displayBlock marginBottom5"">"
			llistaCarpetes+="<img src=""/img/common/iconografia/"+dbROW("TIPDSIMG")+""" class=""floatleft paddingDreta2""/><a href=""/gaia/aspx/llibreriacodiweb/documents/visordocumentshtml.aspx?codiRelacio="+dbRow("RELINCOD").tostring()+"&cami=" & cami
			IF NOT (Request("mp") is nothing) THEN llistaCarpetes+="&mp=1"
			if NOT (Session("ed") is nothing) THEN llistaCarpetes+="&ed=1"
						
			llistaCarpetes += """ class=""arial t08 negre arialNarrow bold nodeco"">"+dbRow("NODDSTXT")+"</a>"
			llistaCarpetes+="</div>"	
			herenciaCarpeta= dbRow("RELCDHER")+"_"+dbROW("RELINFIL").tostring()
			nodeCarpeta=dbRow("RELINFIL")
			contCarpeta+=1
			nomCarpeta=dbRow("NODDSTXT")
			relCarpeta=dbrow("RELINCOD")
	Next
	END IF
	'Busco documents
	contDocument=0
	
	IF strRelacionsAmbPermis.length>0 THEN
		IF est<>0 THEN		
			GAIA.BDR(objconn, "SELECT  DOCDSDES,DOCDTANY,DOCINIDI,METLREL.RELCDORD, METLREL.RELINCOD, NODDSTXT,DOCINNOD,isNull(DOCDSFIT,'') as DOCDSFIT , DOCDSTIT, DOCDSDES,  TDODSIMG FROM METLREL with(NOLOCK) INNER JOIN METLNOD with(NOLOCK) ON METLREL.RELINFIL = METLNOD.NODINNOD INNER JOIN METLDOC with(NOLOCK) ON METLREL.RELINFIL = METLDOC.DOCINNOD AND METLDOC.DOCINIDI=1 INNER JOIN METLTDO with(NOLOCK) ON METLDOC.DOCINTDO = METLTDO.TDOCDTDO WHERE METLREL.RELINCOD IN (" + strRelacionsAmbPermis+") AND METLREL.RELCDSIT IN (1, 3) AND (METLNOD.NODCDTIP = 5) AND RELCDEST="+est.tostring()+" ORDER BY METLREL.RELCDORD",DS)
		ELSE
			GAIA.BDR(objconn, "SELECT  DOCDSDES,DOCDTANY,DOCINIDI,METLREL.RELCDORD, METLREL.RELINCOD, NODDSTXT,DOCINNOD,  isNull(DOCDSFIT,'') as DOCDSFIT, DOCDSTIT, DOCDSDES, TDODSIMG FROM METLREL with(NOLOCK) INNER JOIN METLNOD with(NOLOCK) ON METLREL.RELINFIL = METLNOD.NODINNOD INNER JOIN METLDOC ON METLREL.RELINFIL = METLDOC.DOCINNOD AND METLDOC.DOCINIDI=1 INNER JOIN METLTDO ON METLDOC.DOCINTDO = METLTDO.TDOCDTDO WHERE METLREL.RELINCOD IN (" + strRelacionsAmbPermis+") AND METLREL.RELCDSIT IN (1, 3) AND (METLNOD.NODCDTIP = 5) ORDER BY METLREL.RELCDORD",DS)
		
		END IF	
		For each dbRow in ds.tables(0).Rows		
			relTMP.bdGet(objconn,dbrow("RELINCOD"))
			
			IF NOT dbROW("DOCDSFIT")="" andalso (instr(dbrow("DOCDSFIT"),".mp")>0  OR (instr(dbrow("DOCDSFIT"),".wm")>0)) THEN
				adreça = "<a href="""" onclick=""window.open('/gaia/aspx/documents/visorvideos.aspx?v="+dbRow("DOCDSFIT")+"','_blank','width=640,height=570');return false;"" class=""arial t09 negre valignMiddle bold floatleft"" target=""_blank""><img src=""/img/common/iconografia/"+dbROW("TDODSIMG")+""" class=""valignMiddle border0"" target=""_blank"" alt=""""/>&nbsp;"+dbRow("DOCDSTIT")+"</a>"
			ELSE
				link = "/GAIA/ASPX/llibreriaCodiWeb/webMunicipal/utilitats/obreFitxer/obreFitxer.aspx?"+HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" + dbrow("DOCINNOD").tostring() + "&codiIdioma=1"))
				adreça = "<a href=""" & link & """ class=""arial t08 negre bold"" target=""_blank"">" & dbRow("DOCDSTIT") & "</a>"
			END IF
				
			
			if ambDataDescripcio THEN
				llistaDocuments &= "<div class=""displayBlock paddingsupinf3 border1BottomDotted846540"">"
				llistaDocuments &= "<div class=""displayInline gris arial t08 paddingsupinf2 w60"">" & dbrow("DOCDTANY") & "</div><div class=""displayInline paddingEsquerra5""><img src=""/img/common/iconografia/" & dbROW("TDODSIMG") & """ target=""_blank"" class=""border0"" align=""middle"" alt=""""/></div>"				
				llistaDocuments &= "<div class=""displayInline paddingEsquerra5 paddingsupinf2"">" & adreça & "</div><div class=""paddingsupinf2 displayInline arial t075 gris paddingEsquerra10"">" & dbrow("DOCDSDES") & "</div><div class=""displayInline paddingEsquerra10 paddingsupinf2"">"
				
			
				
			ELSE
				llistaDocuments &= "<div class=""clearboth paddingsupinf2"">"	
				llistaDocuments &=  "<a href=""" + link + """ class=""arial t075 negre valignMiddle displayInlineBlock"" target=""_blank"" valign=""middle""><img src=""/img/common/iconografia/" + dbRow("TDODSIMG") + """ target=""_blank"" class=""border0 valignMiddle"" alt=""""/>&nbsp;" + dbRow("DOCDSTIT") + "</a>"
			
			END IF
			'Poso el botor d'editar
				IF   not ( Session("ed") is nothing) THEN 
					IF NOT isdbNull(dbrow("RELINCOD")) THEN
						
						IF clsPermisos.tepermis(objconn,3,idusuari,idusuari,  relTMP,0,usuariXarxa,"") THEN
					
							llistaDocuments &= "<a href=""/gaia/aspx/documents/carregaDocuments.aspx?" & HttpUtility.UrlEncode(oCodParam.encriptar("id=" & dbRow("DOCINNOD").ToString() & "&idiarbre=" & dbRow("DOCINIDI").ToString())) & """ target=""_blank"" class=""valignMiddle displayInline marginEsquerra5""><img src=""http://www.l-h.cat/img/boton_editar.gif"" border=""0"" align=""absmiddle"" alt=""Editar""/></a>"
							
							
						END IF
					END IF
				END IF
			IF ambDataDescripcio THEN
				llistaDocuments &= "</div></div>"
			ELSE
				llistaDocuments &= "</div>"
			END IF
			
			contDocument+=1
			
		Next
	
		'Tamb&eacute; afegeixo la llista de p&agrave;gines web	
		IF est<>0 THEN
			GAIA.bdR(objconn,"SELECT   relContingut.RELINCOD as relacio,METLLNK.LNKWNTIP,METLLNK.LNKDSLNK,METLLNK.LNKDSTXT, relContingut.RELDSFIT, METLREI.REIDSFIT FROM   METLREL relMenu with(NOLOCK) LEFT OUTER JOIN METLLNK with(NOLOCK) ON relMenu.RELINFIL = METLLNK.LNKINNOD LEFT OUTER JOIN METLREI with(NOLOCK) ON METLLNK.LNKCDREL = METLREI.REIINCOD AND METLREI.REIINIDI ="+ codiIdioma.tostring()+" LEFT OUTER JOIN METLREL relContingut with(NOLOCK) ON METLLNK.LNKCDREL = relContingut.RELINCOD WHERE (relMenu.RELINPAR= "+ rel.infil.tostring()+" ) AND (METLLNK.LNKINIDI ="+ codiIdioma.tostring()+") AND (relMenu.RELCDSIT <98)  AND relMenu.RELCDEST =" + est.tostring() + " ORDER BY relMenu.RELCDORD" ,DS)		
		ELSE
			GAIA.bdR(objconn,"SELECT     relContingut.RELINCOD as relacio,METLLNK.LNKINIDI, METLLNK.LNKINNOD, METLLNK.LNKWNTIP,METLLNK.LNKDSLNK,METLLNK.LNKDSTXT, relContingut.RELDSFIT, METLREI.REIDSFIT FROM   METLREL relMenu with(NOLOCK) LEFT OUTER JOIN METLLNK with(NOLOCK) ON relMenu.RELINFIL = METLLNK.LNKINNOD LEFT OUTER JOIN METLREI with(NOLOCK) ON METLLNK.LNKCDREL = METLREI.REIINCOD AND METLREI.REIINIDI ="+ codiIdioma.tostring()+" LEFT OUTER JOIN METLREL relContingut with(NOLOCK) ON METLLNK.LNKCDREL = relContingut.RELINCOD WHERE (relMenu.RELINPAR= "+ rel.infil.tostring()+" ) AND (METLLNK.LNKINIDI ="+ codiIdioma.tostring()+") AND (relMenu.RELCDSIT <98)   ORDER BY relMenu.RELCDORD" ,DS)	
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
				llistaDocuments+="<div class=""paddingsupinf2 paddingEsquerra5 esq clearboth"">"
	'			llistaDocuments+="<div class=""paddingSup2Dre5Esq15 esq"">"
				llistaDocuments+="<div class=""floatleft paddingEsquerra5"" align=""absmiddle""><img src=""/img/common/iconografia/ic_html.gif"" target=""_blank"" class=""border0 valignMiddle"" alt=""""/>&nbsp;</div><div align=""absmiddle""><a href="""+link.REplace("GAIA/","")+""" class=""nodeco over negre bold t075 floatleft valignMiddle"" target="""+target+""">"+dbROW("LNKDSTXT")+"</a>"	
					IF   not ( Session("ed") is nothing) THEN 
						IF NOT isdbNull(dbrow("relacio")) THEN
							relTMP.bdGet(objconn,dbrow("relacio"))
	
							IF clsPermisos.tepermis(objconn,3,idusuari,idusuari,  relTMP,0,usuariXarxa,"") THEN
								llistaDocuments +="<a href=""/gaia/aspx/links/carregalinks.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("id=" + dbRow("LNKINNOD").ToString() + "&idiarbre=" + dbRow("LNKINIDI").ToString())) + """ target=""_blank"" class=""valignMiddle""><img src=""http://www.l-h.cat/img/boton_editar.gif"" border=""0"" align=""absmiddle"" alt=""Editar""/></a>"
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
		If session("desplegarCarpetes")=1 AND (contDocument=0 and contCarpeta=1) THEN 
	
		'Poso el nom de la carpeta		
		txtTitol+="<div class=""paddingEsquerra10 blanc arial t08 bold fonsBlauTurquesa mayusculas paddingsupinf3"">"+nomCarpeta+"</div>"		
		llistaCarpetes=""				
		contCarpeta=0

		rel.bdget(objconn,relCarpeta)
		strRelacionsAmbPermis=  clsPermisos.FillsRelacioAmbPermis(objConn, 9,idUsuari, idUsuari,  rel, 0, usuariXarxa, grupsAD) 		
		IF strRelacionsAmbPermis.length>0 THEN		
			GAIA.BDR(objconn, "SELECT  DOCINIDI,METLREL.RELCDORD, METLREL.RELINCOD, NODDSTXT, isNull(DOCDSFIT,'') as DOCDSFIT,DOCDSTIT, DOCWNSIZ, DOCDTANY,  TDODSIMG, TIPDSIMG, DOCINNOD, DOCDSDES FROM METLREL with(NOLOCK) INNER JOIN METLNOD with(NOLOCK) ON METLREL.RELINFIL = METLNOD.NODINNOD LEFT OUTER JOIN METLTIP with(NOLOCK) ON METLTIP.TIPINTIP=METLNOD.NODCDTIP LEFT OUTER JOIN METLDOC with(NOLOCK) ON METLREL.RELINFIL = METLDOC.DOCINNOD AND METLDOC.DOCINIDI=1 LEFT OUTER JOIN METLTDO with(NOLOCK) ON METLDOC.DOCINTDO = METLTDO.TDOCDTDO WHERE METLREL.RELINCOD IN  ("+ strRelacionsAmbPermis+") AND (METLREL.RELCDSIT IN (1, 3)) AND (METLNOD.NODCDTIP IN (5,35,47)) ORDER BY METLREL.RELCDORD",DS)



			For each dbRow in ds.tables(0).Rows		
				relTMP.bdGet(objconn,dbrow("RELINCOD"))
				IF instr(dbrow("DOCDSFIT"),".mp")>0  OR (instr(dbrow("DOCDSFIT"),".wm")>0) THEN
					adreça = "<a href="""" onclick=""window.open('/gaia/aspx/documents/visorvideos.aspx?v="+dbRow("DOCDSFIT")+"','_blank','width=640,height=570');return false;"" class=""arial t075 negre valignMiddle bold floatleft"" target=""_blank""><img src=""/img/common/iconografia/"+dbROW("TDODSIMG")+""" class=""valignMiddle border0"" target=""_blank"" alt=""""/>&nbsp;"+dbRow("DOCDSTIT")+"</a>"
				ELSE
					link = "/GAIA/ASPX/llibreriaCodiWeb/webMunicipal/utilitats/obreFitxer/obreFitxer.aspx?"+HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" + dbrow("DOCINNOD").tostring() + "&codiIdioma=1"))
					adreça = "<a href=""" & link & """ class=""arial t075 negre bold"" target=""_blank"">" & dbRow("DOCDSTIT") & "</a>"
				END IF
				
				
				if ambDataDescripcio THEN
					llistaDocuments &= "<div class=""floatleft paddingsupinf3 border1BottomDotted846540"">"
					llistaDocuments &= "<div class=""floatleft gris arial t075 paddingsupinf2 w60"">" & dbrow("DOCDTANY") & "</div><div class=""floatleft paddingEsquerra5""><img src=""/img/common/iconografia/" & dbROW("TDODSIMG") & """ target=""_blank"" class=""border0"" align=""middle"" alt=""""/></div>"				
					llistaDocuments &= "<div class=""floatleft paddingEsquerra5 paddingsupinf2 w250"">" & adreça & "</div><div class=""paddingsupinf2 floatleft arial t08 gris paddingEsquerra10 w380"">" & dbrow("DOCDSDES") & "</div><div class=""floatleft paddingEsquerra10 paddingsupinf2"">"
					
					'Poso el botor d'editar
					IF   not ( Session("ed") is nothing) THEN 
						IF NOT isdbNull(dbrow("RELINCOD")) THEN
							
							IF clsPermisos.tepermis(objconn,3,idusuari,idusuari,  relTMP,0,usuariXarxa,"") THEN								
								llistaDocuments &="<a href=""/gaia/aspx/documents/carregaDocuments.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("id=" + dbRow("DOCINNOD").ToString() + "&idiarbre=" + dbRow("DOCINIDI").ToString())) + """ target=""_blank"" class=""valignMiddle""><img src=""http://www.l-h.cat/img/boton_editar.gif"" border=""0"" align=""absmiddle"" alt=""Editar""/></a>"
							END IF
						END IF
					END IF
					llistaDocuments &= "</div></div>"
				ELSE
					llistaDocuments &= "<div class=""clearboth paddingsupinf2"">"	
					llistaDocuments &=  "<a href=""" + link + """ class=""arial t075 negre valignMiddle bold"" target=""_blank"" valign=""middle""><img src=""/img/common/iconografia/" + dbRow("TDODSIMG") + """ target=""_blank"" class=""border0 valignMiddle"" alt=""""/>&nbsp;" + dbRow("DOCDSTIT") + "</a>"
					llistaDocuments &= "</div>"	
				END IF
				contDocument+=1
			Next
		END IF	
	'També afegeixo la llista de pàgines web
	
		GAIA.bdR(objconn,"SELECT     METLLNK.LNKWNTIP, METLLNK.LNKDSLNK,METLLNK.LNKDSTXT, relContingut.RELDSFIT, METLREI.REIDSFIT, relMenu.RELINCOD as relacio FROM   METLREL relMenu with(NOLOCK) LEFT OUTER JOIN METLLNK with(NOLOCK) ON relMenu.RELINFIL = METLLNK.LNKINNOD LEFT OUTER JOIN METLREI with(NOLOCK) ON METLLNK.LNKCDREL = METLREI.REIINCOD AND METLREI.REIINIDI ="+ codiIdioma.tostring()+" LEFT OUTER JOIN METLREL relContingut with(NOLOCK) ON METLLNK.LNKCDREL = relContingut.RELINCOD WHERE relMenu.RELINCOD in  ("+ strRelacionsAmbPermis +" ) AND (METLLNK.LNKINIDI ="+ codiIdioma.tostring()+") AND (relMenu.RELCDSIT<98) ORDER BY relMenu.RELCDORD" ,DS)	
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
					llistaDocuments+="<div class=""clearboth paddingsupinf2"">"
					llistaDocuments+="<div class=""floatleft paddingEsquerra5""><img src=""/img/common/iconografia/ic_html.gif"" border=""0"" target=""_blank"" align=""absmiddle""/>&nbsp;</div><div class=""textneg paddingBottom1 floatleft""><a href="""+link.REplace("GAIA/","")+""" class=""nodeco over negre t075"" target="""+target+""">"+dbROW("LNKDSTXT")+"</a></div>"			
					llistaDocuments+="</div>"
					contDocument+=1	
				END IF
		Next	
	END IF
	END IF
	ds.dispose()	

	GAIA.bdFi(objConn)

	session("ambDataDescripcio")=false
	Response.write(llistaCami)
	Response.write(txtTitol)
	Response.write(llistaCarpetes)
	Response.Write(llistaDocuments)	
		'gaia.debug(objconn,codirelacio & ".. " & Now)
%>

<%IF (Request("senseTags") is nothing) THEN
  IF (NOT Request("cami") is nothing) AND ( Request("mp") is nothing)  THEN 
%>
<%--#INCLUDE VIRTUAL="/inc/peuDocuments.inc" --%>
</body>
</html>


<% END IF
END IF
oCodParam=nothing		
%>