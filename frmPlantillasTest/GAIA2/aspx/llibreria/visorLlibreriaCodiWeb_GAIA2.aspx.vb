Imports System.Data
Imports System.Data.OleDb

Public Class visorLlibreriaCodiWeb_GAIA2
    Inherits System.Web.UI.Page

	'**********************************************************************
	'**********************************************************************
	'			F R M L L I B R E R I A C O D I W E B 
	'**********************************************************************
	'**********************************************************************

	Public Shared nif As String
	Dim codiIdioma As Integer = 0
	Public Shared objconn As OleDbConnection

	Private Sub Page_UnLoad(sender As Object, e As System.EventArgs) Handles MyBase.Unload
		GAIA.bdFi(objconn)
	End Sub 'Page_UnLoad


	Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
		Dim DS As DataSet
		Dim dbRow As DataRow

		Dim cont As Integer = 0
		objconn = GAIA.bdIni()

		'If Not Request("idioma") Is Nothing Then
		'	codiIdioma = Request("idioma")
		'Else
		'	If Not Request("idiArbre") Is Nothing Then
		codiIdioma = Request("idiArbre")
		'	End If
		'End If

		'If HttpContext.Current.User.Identity.Name.Length > 0 Then
		'	If (Session("nif") Is Nothing) Then
		'		Session("nif") = GAIA.nifUsuari(objconn, HttpContext.Current.User.Identity.Name).Trim()
		'	End If
		'	If Session("codiOrg") Is Nothing Then
		'		Session("CodiOrg") = GAIA.trobaNodeUsuari(objconn, Session("nif")).ToString().Trim()
		'	End If
		'End If
		Session("codiOrg") = 346231

		If Session("codiOrg") = 49730 Or Session("codiOrg") = 49727 Or Session("codiOrg") = 56935 Or Session("codiOrg") = 80879 Or Session("codiOrg") = 297650 Or Session("codiOrg") = 313486 Or Session("codiOrg") = 346231 Or Session("codiOrg") = 297650 Then

			If Not Page.IsPostBack Then
				If Not Request("id") Is Nothing Then
					DS = New DataSet()
					GAIA.bdr(objconn, "SELECT LCWINIDI,LCWDSTIT,LCWDSTXT,LCWCDTIP, LCWTPFOR,LCWTPFOL, LCWDSHLP FROM METLLCW WHERE LCWINNOD=" + Request("id").ToString() + " AND LCWINIDI=" + codiIdioma.ToString(), DS)
					If DS.Tables(0).Rows.Count > 0 Then
						Dim llistaNodes As String()
						Dim strNodes As String
						Dim item As String
						dbRow = DS.Tables(0).Rows(0)
						LCWDSTIT.Text = dbRow("LCWDSTIT")
						LCWDSTXT.Text = dbRow("LCWDSTXT")
						LCWDSHLP.Text = dbRow("LCWDSHLP")
						codiIdioma = dbRow("LCWINIDI")
						btnInsert.Value = "Modificar codi web"
						LCWCDTIP.Items.FindByValue(dbRow("LCWCDTIP")).Selected = True
						LCWTPFOR.Items.FindByValue(dbRow("LCWTPFOR")).Selected = True
						LCWTPFOL.Items.FindByValue(dbRow("LCWTPFOL")).Selected = True
						txtCodiNode.Text = Request("id")

						carregarAss(41, "Relacions")
						carregarAss(104, "RelacionsPlantilles")

						If dbRow("LCWCDTIP") = 2 Or dbRow("LCWCDTIP") = 4 Then
							lblEditarFitxer.Text = ""
							cont = 1
							For Each item In dbRow("LCWDSTXT").split("|")
								item = item.Replace("\", "/")
								lblEditarFitxer.Text += IIf(cont = 1, "", " - ") & "<a href=""file://" & ("172.16.2.3/wwwroot/gaia/aspx/llibreriacodiweb/" & item.Substring(0, InStrRev(item, "/"))).Replace("//", "/") & """ class=""text-primary"">busca fitxer" & IIf(cont = 1, "", cont) & "</a>"
								cont = cont + 1
							Next item

						End If


						GAIA.bdr(objconn, "select TOP 1 CELINPAR FROM METLCEL WHERE CELINLCW=" & Request("id") & " AND CELINIDI=1 ORDER BY CELDTFEC DESC", DS)
						If DS.Tables(0).Rows.Count > 0 Then
							pnlCel.Visible = True
							ltCEL.Text = "<a href=""http://lhintranet/gaia/aspx/llibreriacodiweb/" & DS.Tables(0).Rows(0)("CELINPAR") & """ target=""_blank"">obrir </a>"
						End If

					End If
					DS.Dispose()
				End If

			Else
				pnlCel.Visible = False
			End If
			lblIdioma.Text = "<select name=""idioma"" class=""custom-select custom-select-sm"">" + GAIA.llistaIdiomes(objconn, codiIdioma).ToString() + "</select>"
		Else
			Server.Transfer("/gdocs/232511_1.aspx")
		End If
	End Sub 'Page_Load

	Protected Sub carregarAss(ByVal idAssociacio As Integer, ByVal control As String)
		Dim ds As New DataSet, sSQL As String
		Dim dbRow As DataRow, i As Integer
		Dim oControlNodes As TextBox, oControlTxt As TextBox
		oControlNodes = Page.FindControl(control & "Nodes")
		oControlTxt = Page.FindControl(control & "Txt")
		sSQL = "SELECT ASSCDNRL,NODDSTXT FROM METLASS LEFT JOIN METLNOD ON ASSCDNRL = NODINNOD " &
				"WHERE ASSCDNOD=" & txtCodiNode.Text & " AND ASSCDTPA = " & idAssociacio

		GAIA.bdr(objconn, sSQL, ds)
		i = 0
		For Each dbRow In ds.Tables(0).Rows
			If i > 0 Then
				oControlNodes.Text &= ","
				oControlTxt.Text &= ","
			End If
			oControlNodes.Text &= dbRow("ASSCDNRL").ToString()
			oControlTxt.Text &= dbRow("NODDSTXT").ToString()
			i += 1
		Next dbRow
	End Sub

	Protected Sub clickAfegirCodi(sender As Object, e As EventArgs)
		Dim tipusNode As Integer
		Dim codiNode As Integer

		'Si és un canvi d'idioma he de fer un delete del idioma nou i un insert
		If Not Request("idiArbre") Is Nothing Then
			If Request("idiArbre") <> Request("idioma") Then
				GAIA.bdSR(objconn, "DELETE FROM METLLCW WHERE LCWINIDI=" + Request("idioma").ToString() + " AND LCWINNOD=" + Request("id").ToString())
				GAIA.bdSR(objconn, "INSERT INTO METLLCW (LCWINNOD, LCWINIDI, LCWDSTIT,LCWDSUSR,LCWDSTXT,LCWDSHLP, LCWCDTIP,LCWTPFOR,LCWTPFOL) VALUES (" + Request("id").ToString() + "," + Request("idioma").ToString() + ",'" + LCWDSTIT.Text.ToString().Replace("'", "''") + "','" & Session("codiOrg") + "','" + LCWDSTXT.Text.ToString().Replace("'", "''") & "','" & LCWDSHLP.Text.ToString().Replace("'", "''") + "'," + LCWCDTIP.SelectedItem.Value.ToString() + ",'" + LCWTPFOR.SelectedItem.Value.ToString() + "','" + LCWTPFOL.SelectedItem.Value.ToString() + "')")
				btnInsert.Value = "Afegir idioma"
				lblResultat.Text = ""
				GAIA.escriuResultat(objconn, lblResultat, "Idioma afegit amb èxit.", "")
			End If
		End If

		Select Case btnInsert.Value
			Case "Modificar codi web"
				'Dim DS As DataSet
				'Dim dbRow As DataRow
				'Dim rel As New clsRelacio
				'DS = New DataSet()
				'Dim relbuida As New clsRelacio
				'If Request("idioma") = 1 Then 'Només actualitzo el node si la info està en català (idioma=1)			
				'	GAIA.bdSR(objconn, "UPDATE METLNOD SET NODDSTXT='" + LCWDSTIT.Text.Replace("'", "''").Replace("’", "''") + "' WHERE NODINNOD=" + Request("txtCodiNode").ToString())

				'End If

				'GAIA.bdSR(objconn, "DELETE FROM METLLCW WHERE LCWINNOD=" + Request("txtCodiNode").ToString() + " AND LCWINIDI=" + Request("idioma").ToString())
				'GAIA.bdSR(objconn, "INSERT INTO METLLCW (LCWINNOD, LCWINIDI, LCWDSTIT,LCWDSUSR,LCWDSTXT,LCWDSHLP, LCWCDTIP,LCWTPFOR,LCWTPFOL) VALUES (" + Request("txtCodiNode").ToString() + "," + Request("idioma").ToString() + ",'" + LCWDSTIT.Text.ToString().Replace("'", "''") + "','" + Session("codiOrg") + "','" + LCWDSTXT.Text.ToString().Replace("'", "''") & "','" & LCWDSHLP.Text.ToString().Replace("'", "''") & "'," + LCWCDTIP.SelectedItem.Value.ToString() + ",'" + LCWTPFOR.SelectedItem.Value.ToString() + "','" + LCWTPFOL.SelectedItem.Value.ToString() + "')")
				'lblResultat.Text = ""
				'GAIA.escriuResultat(objconn, lblResultat, "Codi web modificat amb èxit.", "<a href=""/GAIA/aspx/fulles/visorLlibreriaCodiWeb_GAIA2.aspx"" class=""txtRojo12Px"">&nbsp;Nou codi web</a>")

				''He d'afegir accions de manteniment a totes les pàgines web o plantilles que utilitzin aquesta llibreria de codi
				''Cerco primer dins de totes les plantilles
				''	Dim strPlant as string
				''	GAIA.bdR(objconn, "SELECT DISTINCT PLTINNOD FROM METLPLT WHERE (PLTDSLCW LIKE '%"+Request("txtCodiNode").toString() +"') OR (PLTDSLC2 LIKE '%"+Request("txtCodiNode").toString() +"%')", DS)
				''	For Each dbRow In  ds.Tables(0).Rows
				''		strPlant &=" OR WEBDSPLA LIKE '%" & dbRow("PLTINNOD")	& "%'"	
				''	Next dbRow

				''GAIA.bdR(objconn, "(SELECT DISTINCT METLREL.RELINCOD  FROM  METLREL LEFT OUTER JOIN METLWEB ON METLREL.RELINFIL = METLWEB.WEBINNOD WHERE     (METLREL.RELCDSIT <> 99) AND (WEBDSLCW LIKE '%"+Request("txtCodiNode").toString() +"%'" + strPlant +")) UNION (SELECT DISTINCT METLREL.RELINCOD FROM METLREL LEFT OUTER JOIN METLAWE ON METLREL.RELINFIL = METLAWE.AWEINNOD WHERE     (METLREL.RELCDSIT <> 99) AND (METLAWE.AWEDSLCW LIKE '%"+Request("txtCodiNode").toString()  +"%'))	UNION (SELECT DISTINCT METLREL.RELINCOD FROM METLREL LEFT OUTER JOIN METLNWE ON METLREL.RELINFIL = METLNWE.NWEINNOD WHERE  (METLREL.RELCDSIT <> 99) AND (METLNWE.NWEDSLCW LIKE '%"+Request("txtCodiNode").toString() +"%'))", DS)


				''For Each dbRow In  ds.Tables(0).Rows
				''	rel.bdGet(objconn, dbRow("RELINCOD"))
				'lblResultat.Text &= GAIA.afegeixAccioManteniment(objconn, relbuida, Request("txtCodiNode"), 99, Now, Now, relbuida)

				''NEXT	
				''borrem les associacions actuals
				'GAIA.bdSR(objconn, "DELETE FROM METLASS WHERE ASSCDNOD=" + txtCodiNode.Text)
				''guardem les noves associacions
				'guardarAss(41, RelacionsNodes.Text)
				'guardarAss(104, RelacionsPlantillesNodes.Text)

				'DS.Dispose()
			Case "Afegir codi"
				'tipusNode = GAIA.tipusNodeByTxt(objconn, "fulla codiWeb")
				''Inserto el node document
				'codiNode = GAIA.insertarNode(objconn, tipusNode, LCWDSTIT.Text, Session("codiOrg"))
				''em guardo el codi del node
				'txtCodiNode.Text = codiNode.ToString()
				''Inserto el nodo document en l'arbre personal de l'usuari
				'GAIA.insertaNodeArbrePersonal(objconn, GAIA.tipusNodeByTxt(objconn, "fulla codiWeb"), codiNode, Session("codiOrg"), "")
				''Creo la fulla codiWeb			
				'GAIA.bdSR(objconn, "INSERT INTO METLLCW (LCWINNOD, LCWINIDI, LCWDSTIT,LCWDSUSR,LCWDSTXT, LCWDSHLP, LCWCDTIP, LCWTPFOR, LCWTPFOL) VALUES (" + codiNode.ToString() + "," + Request("idioma").ToString() + ",'" + LCWDSTIT.Text.ToString().Replace("'", "''") + "','" + Session("codiOrg") + "','" + LCWDSTXT.Text.ToString().Replace("'", "''") & "','" & LCWDSHLP.Text.ToString().Replace("'", "''") & "'," + LCWCDTIP.SelectedItem.Value.ToString() + ",'" + LCWTPFOR.SelectedItem.Value.ToString() + "','" + LCWTPFOL.SelectedItem.Value.ToString() + "')")
				''guardem les noves associacions
				'guardarAss(41, RelacionsNodes.Text)
				'guardarAss(104, RelacionsPlantillesNodes.Text)
				lblResultat.Text = ""
				GAIA.escriuResultat(objconn, lblResultat, "Codi web afegit amb èxit.", "<a href=""/GAIA/aspx/fulles/visorLlibreriaCodiWeb_GAIA2.aspx"" class=""txtRojo12Px"">&nbsp;Nou codi web</a>")
		End Select
		btnInsert.Value = "Modificar codi web"
		checkedAfegirCodi.Checked = True
	End Sub

	Private Sub guardarAss(ByVal idAssociacio As Integer, ByVal llista As String)
		Dim aValors As String(), sSQL As String
		Dim item As String, sTipus As String
		If Trim(llista) = "" Then Exit Sub
		aValors = Split(llista, ",")
		'tipus: fulla tràmit
		sTipus = "51"

		For Each item In aValors

			sSQL = "INSERT INTO METLASS (ASSCDTPA,ASSCDNOD,ASSCDTIP,ASSCDNRL) VALUES (" &
			idAssociacio & "," &
			txtCodiNode.Text & "," &
			sTipus & "," &
			item & ")"

			GAIA.bdSR(objconn, sSQL)
		Next item
	End Sub

End Class