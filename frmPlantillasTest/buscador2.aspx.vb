	Private Sub Page_Load(sender As Object, e As System.EventArgs)  
		f_inicialitzar()
		If Not Page.IsPostBack Then
			carrega_idioma()
		End if
	
	End sub
	
	'procediment per carregar el desplegable d'idioma
  	private sub carrega_idioma()
		dim ds as new Dataset, qSQL as string
		qSQL="select IDICDIDI,IDIDSNOM from METLIDI"
		GAIA.bdR(objconn,qSQL,ds)			
		ddlb_idioma.datasource=ds
		ddlb_idioma.datatextfield="IDIDSNOM"
		ddlb_idioma.datavaluefield="IDICDIDI"
		ddlb_idioma.databind()
		
		'ddlb_idioma.Items.Insert(0, new ListItem("Tots",-1))
		ds.Dispose
	end sub

	'***************************************************************************************
	'procediment encarregat de carregar el grid segons els filtres especificats
	'**************************************************************************************
	Private Sub carregarGrid(ByVal sOrder as String, byval tipusSortida as string)
		carregarGrid(sorder,tipusSortida,"")
	End Sub
	
	Private Sub carregarGrid(ByVal sOrder as String, byval tipusSortida as string, byVal paramInicial as string)
		dim sSQL as string, bCercarContingut as boolean, sLlistaIS as string 
		Dim sSQLOrder as string=""
		Dim rel as new clsrelacio
		Dim paramUbicacio as string=string.empty
		'CERCA DINS EL CONTINGUT DELS FITXERS (INDEX SERVER)
		bCercarContingut=f_cercar_contingut()
		if bCercarContingut then
			sLlistaIS=f_cerca_IS()
		end if
		

		Dim selectSIT as string=""
		
		IF cstr(ddlb_estat.selectedvalue)="0" THEN
			selectSIT = " AND RELCDSIT IN (1,2,3) " 
		ELSE
			selectSIT = " AND RELCDSIT="+cstr(ddlb_estat.selectedvalue)
		END IF
		'CERCA PER IDIOMA (TAULA METLDOC)
		sSQL="SELECT DOCINNOD,DOCDSFIT,substring(DOCDSTIT,1,100) as NOM,DOCWNSIZ / 1024.0 as MIDA,DOCDTANY as CREACIO, RELINCOD, TDODSIMG, cast(RELINCOD as varchar) as Relacions " & _
		"FROM METLDOC LEFT JOIN METLREL ON METLREL.RELINFIL = METLDOC.DOCINNOD " & selectSit & _
		" LEFT JOIN METLTDO ON TDOCDTDO=DOCINTDO WHERE DOCINIDI = " & ddlb_idioma.selectedvalue
		
		'Cerca per data de creació
		if trim(dataIni.text <> "") then
			sSQL &= " AND DOCDTANY >= '" & dataIni.text & "'"
		end if
		if trim(dataFi.text <> "") then
			sSQL &= " AND DOCDTANY <= '" & dataFi.text & "'"
		end if

		'cerca per títol de document
		if trim(titol.text) <> "" then
			sSQL = sSQL & " AND UPPER(CAST(DOCDSTIT AS VARCHAR(8000))) LIKE '%" & ucase(replace(trim(titol.text),"'","''")) & "%'"
		end if
		IF Request("codiRelacioInicial")=nothing THEN
			Dim oCodParam as new lhCodParam.lhCodParam
			paramUbicacio=oCodParam.querystring(Request.querystring(0),"codiRelacioInicial")
			oCodparam=nothing
		ELSE
			paramUbicacio=Request.QueryString("codiRelacioInicial")			
		END IF
		
		IF paramUbicacio.length>0 THEN
			rel.bdget(objconn,paramUbicacio)
			ssql = ssql + " AND METLREL.RELCDHER like '"+rel.cdher+"_"+rel.infil.tostring()+"%' "
		END IF		
		
		


'response.write(sLlistaIS.length)
	
	
	
		If Trim(sOrder) <> "" then
			sSQLOrder = " ORDER BY " & sOrder 
		End If
		
		
		
		
		'si s'ha cercat pel contingut, afegir la restricció
		if bCercarContingut then
			Dim strTMP as string=""
			Dim sSQLTmp as string=""
			Dim pos as integer=-9
			'response.write("<br>" & sllistaIS & "<br>")
			strTMP =""
			While sLListaIS.length>5000
				pos=0
				IF strTMP.length>0 THEN
					sSQLTmp &= " | " 
				END IF
				strTMP= sLListaIS.substring(0,5000)
				pos = instrrev(strTMP, ",")
				
				strTMP = sLListaIS.substring(0,pos-1)
				sllistaIs = sllistaIs.substring(pos)
				'Response.write("<br>" & strTMP & "<br>")	
			
				sSQLTmp &= sSQL & " AND CAST(DOCDSFIT AS VARCHAR(8000)) IN (" & strTMP & ")"& sSQLOrder & " "
			END WHILE
			
			
			IF strTMP.length>0 THEN
				sSQLTmp &= " | "  & sSQL & " AND CAST(DOCDSFIT AS VARCHAR(8000)) IN (" & sllistaIS & ")"& sSQLOrder & " "	
				sSQL = sSQLTmp	
			END IF
			'No ha entrat al bucle, faig la sql normal
			IF pos=-9 THEN
				sSQL = sSQL & " AND CAST(DOCDSFIT AS VARCHAR(8000)) IN (" & sLlistaIS & ")" & sSQLOrder
			END IF
			
			
		ELSE
			sSQL = sSQL & sSQLOrder
		end if
		
'		response.write(ssql.length)

		
		
		
		'response.write(ssql)
		f_filtrar_i_mostrar(sSQL,"DOCINNOD",tipusSortida)			
	End Sub
	
	'cerca en el contingut dels fitxers utilitzant l'index server.
	private function f_cerca_IS() as string
		dim ds As New Dataset("resultats"), da as new OleDbDataAdapter, i as integer, sLlistaIS as string
		dim s_query as string, s_query_algunes as string, s_query_totes as string, s_query_sense as string, s_query_tipus as string
		dim Q as Object = Server.CreateObject("ixsso.Query")
		dim Util as Object = Server.CreateObject("ixsso.Util")
		'consulta a l'índex server
		'ordenació per rellevància (descendent)
		Q.SortBy = "rank[d]"
		'camp a recollir
		Q.Columns = "DocTitle, vpath, path, filename, size, write, characterization"
		'Catàleg, identificat pel path físic on es troba
		Q.Catalog = "c:\IndexServer\WEB06"
		'Directori on buscar
		util.AddScopeToQuery(Q, "e:\docs\GAIA\", "deep")
		'A la variable s_query anirem montant la consulta que enviarem a l'Index Server
		's_query = "(#filename *.shtm OR #filename *.htm OR #filename *.html OR #filename *.asp OR #filename *.pdf OR #filename *.xls OR #filename *.doc OR #filename *.ppt OR #filename *.pps)"
		s_query_tipus = "NOT (#filename *.shtm OR #filename *.shtml OR #filename *.htm OR #filename *.html OR #filename *.asp OR #filename *.aspx)"		
		if trim(paraules_totes.text) <> "" then		
			s_query = "@contents " & Replace(Trim(paraules_totes.text)," ","* and @contents ") & "* AND "
		end if
		if trim(paraules_alguna.text) <> "" then
			s_query = s_query & "(@contents %" & Replace(Trim(paraules_alguna.text)," ","% OR @contents %") & "%) AND "
		end if
		if trim(paraules_sense.text) <> "" then
			if s_query = "" then s_query = " (@size > 0) AND "
			s_query = s_query & "NOT @contents " & Replace(Trim(paraules_sense.text)," ","* AND NOT @contents ") & "* AND "
		end if
		s_query = s_query & s_query_tipus
		'response.write(s_query)
		Q.Query = s_query
		'L'objecte ixsso.Query retorna un recordset. A través de la instrucció següent
		'aconseguim obtenir-lo i carregar-lo en un dataset, que ens servirà per carregar 
		'posteriorment el datagrid
		da.Fill(ds, Q.CreateRecordset("nonsequential"),"resultats")
		for i=0 to ds.tables(0).rows.count - 1
			if sLlistaIS <> "" then sLlistaIS = sLlistaIS & ","
			sLlistaIS = sLlistaIS & "'" & ds.tables(0).rows(i).item("FILENAME") & "'"
		next i
		if trim(sLlistaIS) = "" then sLlistaIS = "'NOMATCH'"
		return sLlistaIS
	end function

	'funció que en indica si cal buscar en el contingut del fitxer, depenent dels filtres entrats per l'usuari.
	private function f_cercar_contingut() as boolean
		if trim(paraules_totes.text) <> "" or trim(paraules_alguna.text) <> "" or trim(paraules_sense.text) <> ""   then
			return true
		else
			return false
		end if
	end function
	
	'mètode alternatiu per filtrar les entrades segons el resultat de l'Index Server
	'drs=ds.tables(0).select("DOCDSFIT IN (" & sLlistaIS & ")")
	'response.write("Long: " & drs.getLength(0))

	'Afegir al dataset una columna calculada, ja que l'Index Server ens retorna la mida en bytes
	'i nosaltres la volem mostrar en Kbytes.
	'dc.columnname = "midaKb"
	'dc.datatype = System.Type.GetType("System.Decimal")
	'dc.expression = "size/1024"
	'ds.tables(0).Columns.Add(dc)
	'instrucció de debug per si volem mostrar el query que s'envia a l'Index Server
	'response.write("Query:" & s_query)
	'dgResultats.DataSource = ds.tables(0)
