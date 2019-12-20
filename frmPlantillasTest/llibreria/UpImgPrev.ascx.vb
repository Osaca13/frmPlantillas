Imports System.Data
Imports System.IO
Imports System.Data.OleDb
Imports System.Web.UI.StateBag
Imports System.Web


Public Class UpImgPrev
    Inherits System.Web.UI.UserControl
    Protected imgwid As Integer = 1
    Protected imgCount As Integer = 0
    Protected dispHTML As String = ""
    Protected codSit As Integer = 96
    Protected idiPral As Integer = 1
    'Protected savePath As String = "http://lhintranet/aplics/GAIA/documents/docs.borrar!/"
    'Protected deletePath As String = "\\lhintranet\wwwroot\aplics\GAIA\documents\docs.borrar!\"
    Protected savePath As String = "/docs/"
    Protected deletePath As String = "e:\docs\GAIA\"
    Protected traducir As Boolean = False

    Public Property codiNode() As Integer
        Get
            Return CInt(ViewState("imgcodiNode"))
        End Get
        Set(ByVal value As Integer)
            ViewState("imgcodiNode") = Value
        End Set
    End Property

    Public Property Traduce() As Boolean
        Get
            Return traducir
        End Get
        Set(ByVal value As Boolean)
            traducir = Value
        End Set
    End Property

    Public Sub afegeixImatges()
        Dim cant As Integer = HttpContext.Current.Request.Files.Count
        Dim fileUp As httppostedfile
        Dim i As Integer
        Dim nom As String
        Dim txtNomFitxer As String = ""
        Dim txtTipusDocument As String = ""
        Dim txtTHor As String = ""
        Dim txtTver As String = ""
        Dim txtTamany As String = ""
        Dim objconn As OleDbConnection

        'Imprime lo que viene por post
        'Dim aux As Integer = Page.Request.Params.Count
        'Dim aux2 As Integer = 0
        'For aux2 = 0 To aux - 1
        'Page.Response.Output.Write(Page.Request.Params.GetKey(aux2))
        'Page.Response.Output.Write("=")
        'Page.Response.Output.Write(Page.Request.Params.Get(aux2))
        'Page.Response.Output.Write("<br>")
        'next
        Dim ds As New DataSet
        Dim megaSelect As String
        Dim codiIdioma As Integer = idiPral

        If Not (Request("lstIdioma") Is Nothing) or Request("lstIdioma")<>"" Then 
            codiIdioma = Request("lstIdioma")
        End If

        megaSelect = "SELECT DISTINCT d.DOCINNOD,d.DOCDSTIT,d.DOCDSALT,d.DOCDSFIT,d.DOCINIDI,d.DOCINTDO,d.DOCWNSIZ,d.DOCWNHOR,d.DOCWNVER,METLREL.RELCDORD FROM METLDOC d  WITH(NOLOCK), METLREL WITH(NOLOCK) " & _
           "WHERE METLREL.RELINPAR=" & Me.codiNode.toString() & " AND METLREL.RELCDSIT IN (95,96) AND METLREL.RELINFIL=d.DOCINNOD AND " & _
           "(d.DOCINIDI = " & codiIdioma.toString() & " OR (d.DOCINIDI = " & IdiPral.toString() & " AND " & _
           "(SELECT COUNT(*) FROM METLDOC j WITH(NOLOCK), METLREL WITH(NOLOCK) " & _
           "WHERE METLREL.RELINPAR= " & Me.codiNode.toString() & " AND METLREL.RELCDSIT IN (95,96) AND METLREL.RELINFIL=j.DOCINNOD AND d.DOCINNOD = j.DOCINNOD AND j.DOCINIDI=" & codiIdioma.toString() & ")=0)) " & _
           "ORDER BY METLREL.RELCDORD ASC"

        GAIA.bdR(objconn, megaSelect, ds)

        For i = 0 To cant - 1
            nom = HttpContext.Current.Request.Files.GetKey(i)
            If (nom.StartsWith("imgfitxer") And Integer.Parse(nom.Substring(9)) > ds.Tables(0).Rows.Count) Then
                fileUp = HttpContext.Current.Request.Files.Get(i)
                If (fileUp.FileName.Length() > 0) Then
                    GAIA.nouFitxer(txtNomFitxer, txtTipusDocument, txtTHor, txtTver, txtTamany, fileUp)

                    Dim num As String = nom.Substring(9)
                    Dim titol As String = HttpContext.Current.Request.Params.Get("ImgTitol" & num)
                    Dim alt As String = HttpContext.Current.Request.Params.Get("ImgAlt" & num)
                    Dim pos As Integer = HttpContext.Current.Request.Params.Get("imgpos" & num)
					Dim chkVisor as integer =  HttpContext.Current.Request.Params.Get("chkNoVisor" & num)
                    'Añadimos la imagen a la bd
                    Dim tipusNodeAux As Integer
                    Dim codiNodeAux As Integer
                    Dim txtCodiNodeAux As String
                    Dim relBuidaAux As New clsRelacio

                    tipusNodeAux = GAIA.tipusNodeByTxt(objConn, "fulla document")
                    codiNodeAux = GAIA.insertarNode(objConn, tipusNodeAux, titol, Session("codiOrg"))
                    txtCodiNodeAux = codiNodeAux

'					gaia.debug(objconn, "INSERT INTO METLDOC (DOCINNOD,DOCINIDI,DOCINTDO,DOCDSTIT,DOCDSFIT,DOCDSFON, DOCDSAUT, DOCDSISB,DOCDSDES,DOCDTANY,DOCDTCAD,DOCDTPUB,DOCWNHOR, DOCWNVER,DOCDSALT, DOCDSLNK, DOCWNSIZ,DOCWNUPD, DOCWNBOR) VALUES (" + codiNodeAux.toString() + "," + IdiPral.toString() + "," + txtTipusDocument.toString() + ",'" + titol.toString().Replace("'", "''") + "','" + txtNomFitxer.toString().Replace("'", "''") + "','','','','',getDate(),'1/1/1900','1/1/1900'," + txtThor.tostring() + "," + txtTver.tostring() + ",'" + alt.toString().Replace("'", "''") + "',''," + txtTamany.tostring() + ",1,'S')")
					try
                    GAIA.bdSR(objConn, "INSERT INTO METLDOC (DOCINNOD,DOCINIDI,DOCINTDO,DOCDSTIT,DOCDSFIT,DOCDSFON, DOCDSAUT, DOCDSISB,DOCDSDES,DOCDTANY,DOCDTCAD,DOCDTPUB,DOCWNHOR, DOCWNVER,DOCDSALT, DOCDSLNK, DOCWNSIZ,DOCWNUPD, DOCWNBOR) VALUES (" + codiNodeAux.toString() + "," + IdiPral.toString() + "," + txtTipusDocument.toString() + ",'" + titol.toString().Replace("'", "''") + "','" + txtNomFitxer.toString().Replace("'", "''") + "','','','','',getDate(),'1/1/1900','1/1/1900'," + txtThor.tostring() + "," + txtTver.tostring() + ",'" + alt.toString().Replace("'", "''") + "',''," + txtTamany.tostring() + ",1,'S')")
					catch
					end try
                    GAIA.generarDatesPublicacio(objConn, txtCodiNodeAux, IdiPral, "'1/1/1900'", "'1/1/1900'")

                    If (Request("lstIdioma") <> IdiPral) Then
                        try
							GAIA.bdSR(objConn, "INSERT INTO METLDOC (DOCINNOD,DOCINIDI,DOCINTDO,DOCDSTIT,DOCDSFIT,DOCDSFON, DOCDSAUT, DOCDSISB,DOCDSDES,DOCDTANY,DOCDTCAD,DOCDTPUB,DOCWNHOR, DOCWNVER,DOCDSALT, DOCDSLNK, DOCWNSIZ,DOCWNUPD, DOCWNBOR) VALUES (" + codiNodeAux.toString() + "," + Request("lstIdioma") + "," + txtTipusDocument.toString() + ",'" + titol.toString().Replace("'", "''") + "','" + txtNomFitxer.toString().Replace("'", "''") + "','','','','',getDate(),'1/1/1900','1/1/1900'," + txtThor.tostring() + "," + txtTver.tostring() + ",'" + alt.toString().Replace("'", "''") + "',''," + txtTamany.tostring() + ",1,'S')")
						catch
						end try
                        GAIA.generarDatesPublicacio(objConn, txtCodiNodeAux, Request("lstIdioma"), "'1/1/1900'", "'1/1/1900'")
                    End If

                    Gaia.log(objconn, relbuidaAux, Session("codiOrg"), titol.replace("'", "''"), 9, txtCodiNodeAux)

            


	'Poso 1000+i perque primer gravem els documents que ja existeixen a la funció modificar fitxer i després els nous. Com que els primers es graven amb un ordre a partir del 100, els seguents els poso amb 1000 per que no es solapi. Tot això ho faig per que el camp "pos" no està funcionant bé i no tinc gaire temps per mirar-ho. Amb això funcionarà bé.
					
				
                    GAIA.creaRelacioPerNode(objconn, codiNode, codiNodeAux, -1, iif(chkVisor=1, 95, 96), 1000+i,Session("codiOrg"))
                    'Reiniciamos variables
                    txtNomFitxer = ""
                    txtTipusDOcument = ""
                    txtTHor = ""
                    txtTver = ""
                    txtTamany = ""
                End If

            End If
        Next
    End Sub

    Sub modificarImatges()
        Dim ds As New DataSet
        Dim dbRow As DataRow
        Dim objconn As OleDbConnection
        Dim megaSelect As String
        Dim codiIdioma As Integer = idiPral
        Dim relBuida As New clsRelacio
        Dim num As Integer
        Dim i As Integer
        Dim j As Integer
        Dim txtNomFitxer As String = ""
        Dim txtTipusDocument As String = ""
        Dim txtTHor As String = ""
        Dim txtTver As String = ""
        Dim txtTamany As String = ""
        Dim nom As String
        Dim cant As Integer = HttpContext.Current.Request.Files.Count
        Dim fileUp As httppostedfile
        Dim nuevaImagen As Boolean = False
		Dim chkNoVisor as integer = 0
        If Not (Request("lstIdioma") Is Nothing) Then
            codiIdioma = Request("lstIdioma")
        End If

        megaSelect = "SELECT DISTINCT d.DOCINNOD,d.DOCDSTIT,d.DOCDSALT,d.DOCDSFIT,d.DOCINIDI,d.DOCINTDO,d.DOCWNSIZ,d.DOCWNHOR,d.DOCWNVER,METLREL.RELCDORD,METLREL.RELCDSIT FROM METLDOC d WITH(NOLOCK), METLREL WITH(NOLOCK) " & _
           " WHERE METLREL.RELINPAR=" & Me.codiNode.toString() & " AND METLREL.RELCDSIT IN (95,96) AND METLREL.RELINFIL=d.DOCINNOD AND " & _
           "(d.DOCINIDI = " & codiIdioma.toString() & " OR (d.DOCINIDI = " & IdiPral.toString() & " AND " & _
           "(SELECT COUNT(*) FROM METLDOC j WITH(NOLOCK), METLREL  WITH(NOLOCK) " & _
           "WHERE METLREL.RELINPAR= " & Me.codiNode.toString() & " AND METLREL.RELCDSIT IN (95,96) AND METLREL.RELINFIL=j.DOCINNOD AND d.DOCINNOD = j.DOCINNOD AND j.DOCINIDI=" & codiIdioma.toString() & ")=0)) " & _
           "ORDER BY METLREL.RELCDORD ASC"

        GAIA.bdR(objconn, megaSelect, ds)

        num = ds.Tables(0).Rows.Count

        For i = 1 To num
            nuevaImagen = False
            If Page.Request.Params.Get("ImgTitol" & i.toString()) Is Nothing Then
                'borrar
                GAIA.esborrarNode(objconn, ds.Tables(0).Rows(i - 1).Item("DOCINNOD"), relBuida, 1, session("CodiOrg"), 0, 99)
            Else
                txtNomFitxer = ds.Tables(0).Rows(i - 1).Item("DOCDSFIT").tostring()
                txtTipusDOcument = ds.Tables(0).Rows(i - 1).Item("DOCINTDO").tostring()
                txtTHor = ds.Tables(0).Rows(i - 1).Item("DOCWNHOR").tostring()
                txtTver = ds.Tables(0).Rows(i - 1).Item("DOCWNVER").tostring()
                txtTamany = ds.Tables(0).Rows(i - 1).Item("DOCWNSIZ").tostring()
				
				
                For j = 0 To cant - 1
                    nom = HttpContext.Current.Request.Files.GetKey(j)
                    If (nom.Equals("imgfitxer" & i.toString())) Then
                        fileUp = HttpContext.Current.Request.Files.Get(j)
                        If (fileUp.FileName.Length() > 0) Then

                            Dim ds2 As New DataSet
                            GAIA.bdR(objconn, "SELECT COUNT (*) FROM METLDOC  WITH(NOLOCK) WHERE DOCDSFIT='" & txtNomFitxer & "'", ds2)

                            If (ds2.Tables(0).Rows(0).Item(0) <= 1) And System.IO.File.Exists(deletePath & txtNomFitxer) Then
                                System.IO.File.Delete(deletePath & txtNomFitxer)
                                If System.IO.File.Exists(deletePath & txtNomFitxer.Insert(txtNomFitxer.Length - 4, "P")) Then
                                    System.IO.File.Delete(deletePath & txtNomFitxer.Insert(txtNomFitxer.Length - 4, "P"))
                                End If
                            End If

                            txtNomFitxer = ""
                            txtTipusDOcument = ""
                            txtTHor = ""
                            txtTver = ""
                            txtTamany = ""

                            GAIA.nouFitxer(txtNomFitxer, txtTipusDocument, txtTHor, txtTver, txtTamany, fileUp)
                            nuevaImagen = True

                        End If

                        Exit For
                    End If
                Next

                If nuevaImagen Or Not (Page.Request.Params.Get("ImgTitol" & i.toString()).equals(ds.Tables(0).Rows(i - 1).Item("DOCDSTIT")) And Page.Request.Params.Get("ImgAlt" & i.toString()).equals(ds.Tables(0).Rows(i - 1).Item("DOCDSALT"))) Then
                    'update o insert
                    If (ds.Tables(0).Rows(i - 1).Item("DOCINIDI") <> codiIdioma) Then
                        Dim sqlstr As String = "INSERT INTO METLDOC (DOCINNOD,DOCINIDI,DOCINTDO,DOCDSTIT,DOCDSFIT,DOCDSFON, DOCDSAUT, DOCDSISB,DOCDSDES,DOCDTANY,DOCDTCAD,DOCDTPUB,DOCWNHOR, DOCWNVER,DOCDSALT, DOCDSLNK, DOCWNSIZ,DOCWNUPD, DOCWNBOR) VALUES (" & _
                        ds.Tables(0).Rows(i - 1).Item("DOCINNOD").toString() & "," & codiIdioma.toString() & "," & txtTipusDOcument & ",'" & Page.Request.Params.Get("ImgTitol" & i.toString()).toString().Replace("'", "''").Replace("’", "''") & "','" & txtNomFitxer & _
                         "','','','','',getDate(),'1/1/1900','1/1/1900'," & txtTHor & "," & txtTver & ",'" & Page.Request.Params.Get("ImgAlt" & i.toString()).toString().Replace("'", "''").Replace("’", "''") & "',''," & txtTamany & ",1,'S')"
                        GAIA.bdSR(objConn, sqlstr)

                        GAIA.generarDatesPublicacio(objConn, ds.Tables(0).Rows(i - 1).Item("DOCINNOD").toString(), codiIdioma, "'1/1/1900'", "'1/1/1900'")
                    Else
                        Dim sqlstr As String = "UPDATE METLDOC SET DOCDSTIT='" & Page.Request.Params.Get("ImgTitol" & i.toString()).toString().Replace("'", "''").Replace("’", "''") & "', DOCDSALT='" & _
                         Page.Request.Params.Get("ImgAlt" & i.toString()).toString().Replace("'", "''").Replace("’", "''") & _
                         "',DOCDSFIT='" & txtNomFitxer & "',DOCINTDO='" & txtTipusDocument & "',DOCWNHOR='" & txtTHor & "',DOCWNVER='" & txtTver & "',DOCWNSIZ='" & txtTamany & "',DOCWNUPD=1" & _
                          " WHERE DOCINNOD=" & ds.Tables(0).Rows(i - 1).Item("DOCINNOD") & " AND DOCINIDI=" & codiIdioma.toString()
                        GAIA.bdSR(objConn, sqlstr)


                    End If
                End If

				'Actualitzo el codi de relació
  				If (ds.Tables(0).Rows(i - 1).Item("RELCDSIT")=95 AND Page.Request.Params.Get("chkNoVisor" & i.toString())=0) OR (ds.Tables(0).Rows(i - 1).Item("RELCDSIT")=96 AND Page.Request.Params.Get("chkNoVisor" & i.toString())=1) THEN
			
					IF Page.Request.Params.Get("chkNoVisor" & i.toString())=0 THEN
						chkNoVisor=96
					ELSE
						chkNoVisor=95
					END IF
					GAIA.bdSR(objconn, "UPDATE METLREL SET RELCDSIT=" & chkNoVisor & " WHERE RELINFIL="  & ds.Tables(0).Rows(i - 1).Item("DOCINNOD"))	
				END IF
				If Not (Page.Request.Params.Get("imgpos" & i.toString())=ds.Tables(0).Rows(i - 1).Item("RELCDORD")) Then
                 GAIA.bdSR(objconn, "UPDATE METLREL SET RELCDORD=" & Page.Request.Params.Get("imgpos" & i.toString()) & " WHERE RELINFIL="  & ds.Tables(0).Rows(i - 1).Item("DOCINNOD"))
                End If


            End If

        Next i

        afegeixImatges()
    End Sub

    Sub previsualitzarImatges()
        Dim ds As New DataSet
        Dim dbRow As DataRow
        Dim objconn As OleDbConnection
        Dim megaSelect As String
        Dim codiIdioma As Integer = idiPral
        Dim titulo As String
        Dim alt As String
		Dim docAnt as integer=0
        dispHTML = ""
        imgwid = 1
        imgCount = 0

		IF ME.codiNode<>0 THEN
			If Not (Request("lstCanviIdioma") Is Nothing) Then
				codiIdioma = Request("lstCanviIdioma")
			End If
	
			megaSelect = "SELECT DISTINCT d.DOCINNOD,d.DOCDSTIT,d.DOCDSALT,d.DOCDSFIT, METLREL.RELCDORD, METLREL.RELCDSIT FROM METLDOC d WITH(NOLOCK), METLREL WITH(NOLOCK) " & _
			   "WHERE METLREL.RELINPAR=" & Me.codiNode.toString() & " AND METLREL.RELCDSIT IN (95,96) AND METLREL.RELINFIL=d.DOCINNOD AND " & _
			   "(d.DOCINIDI = " & codiIdioma.toString() & " OR (d.DOCINIDI = " & IdiPral.toString() & " AND " & _
			   "(SELECT COUNT(*) FROM METLDOC j, METLREL " & _
			   "WHERE METLREL.RELINPAR= " & Me.codiNode.toString() & " AND METLREL.RELCDSIT IN (95,96) AND METLREL.RELINFIL=j.DOCINNOD AND d.DOCINNOD = j.DOCINNOD AND j.DOCINIDI=" & codiIdioma.toString() & ")=0)) " & _
			  "ORDER BY METLREL.RELCDORD ASC"
			
			GAIA.bdR(objconn, megaSelect, ds)
	
			For Each dbRow In ds.Tables(0).Rows
				IF docAnt<>dbrow("DOCINNOD") THEN
					docAnt=dbrow("DOCINNOD")
					titulo = dbRow("DOCDSTIT")
					alt = dbRow("DOCDSALT")
					Traduccio(titulo, alt)
		
					dispHTML = dispHTML & "<div style=""display:block; padding:15px; overflow:hidden; position:relative; width:auto;"" id=""imgr" & imgwid & """ >" & _
					   "<div style=""display:inline-block; margin-right:5px; float:left"">" & _
					   "<input type=hidden class=""txtNeg12px"" id=""imgstate" & imgwid & """ value=1 />" & _
					   "<input type=hidden class=""txtNeg12px"" id=""imgpos" & imgwid & """ name=""imgpos" & imgwid & """ value=""" & dbRow("RELCDORD") & """ />" & _
					   "<a id=""Imgurlfit" & imgwid & """ href=""" & savePath & dbRow("DOCDSFIT") & """ target=""blank"" class=""txtNeg12px""><img id=""imgprev" & imgwid & """ src="" " & savePath & dbRow("DOCDSFIT") & " "" onclick=""seleccion('imgr" & imgwid & "')"" style=""display:inline;"" width=""110"" height=""110"" /></a>" & _
					   "<input type=file id=""imgfitxer" & imgwid & """ name=""imgfitxer" & imgwid & """ style=""display:none"" onChange=""Previsualitzar(" & imgwid & ")"" />" & _
					   "&nbsp;" & _
					   "<input type=button style=""display:none"" class=""txtNeg12px"" id=""ImgbElime" & imgwid & """ value=""Eliminar"" onClick=""EliminarElement(" & imgwid & ")"" />" & _
					  "</div>" & _
					  "<div style=""display:inline-block; vertical-align:top"">" & _
					   "<table>" & _
					   "<tr><td width=""450"">" & _
						"<a id=""ImgTextTit" & imgwid & """ style=""display:inline"" class=""txtNeg12px bold"">" & _
						 "Títol:" & _
						"</a>" & _
						"<br>" & _
					"<input type=""text"" class=""txtNeg12px"" style=""display:inline; width:450px;"" id=""ImgTitol" & imgwid & """ name=""ImgTitol" & imgwid & """ value=""" & titulo & """ onclick=""seleccion('imgr" & imgwid & "')"" />" & _
					   "</td></tr>" & _
					   "<tr><td>" & _
						"<a id=""ImgTextAlt" & imgwid & """ style=""display:inline""  class=""txtNeg12px bold"">" & _
						 "Text alternatiu:" & _
						"</a>" & _
						"<br>" & _
						"<input type=""text"" class=""txtNeg12px"" style=""display:inline; width:450px;"" id=""ImgAlt" & imgwid & """ name=""ImgAlt" & imgwid & """ value=""" & alt & """ onclick=""seleccion('imgr" & imgwid & "')"" />" & _
					   "<br><input name=""chkNoVisor" & imgwid & """ type=""checkbox"" value=""1"" " & iif(dbrow("RELCDSIT")=95, " checked " ,"") & " >No mostrar en visor d'imatges" & _
					   "</td></tr>" & _
					   "<tr><td>" & _
						"<input type=button style=""display:inline"" id=""ImgbEdit" & imgwid & """ value=""Canviar imatge""  class=""txtNeg12px"" onClick=""AfegirElement(" & imgwid & ", 1)"" />" & _
						"<input type=button style=""display:none"" id=""ImgbDesfer" & imgwid & """ value=""Desfer Canvi"" class=""txtNeg12px"" onClick=""desferCanviImg(" & imgwid & ")"" />" & _
						"&nbsp;" & _
						"<input type=button style=""display:inline"" id=""ImgbElim" & imgwid & """ value=""Eliminar""  class=""txtNeg12px"" onClick=""EliminarElement(" & imgwid & ")"" />" & _
					   "</td></tr>" & _
					   "</table>" & _
					   "</div>" & _
					   "<br>" & _
					   "</div>"
		
					imgwid = imgwid + 1
					
					imgCount += 1
				END IF
			Next dbRow
		end if
        traducir = False
    End Sub

    Private Sub Traduccio(ByRef titulo As String, ByRef alt As String)
        'Dim clTradAuto As wsTradAuto = New wsTradAuto()
        Dim text As String
        Dim direccio As String

        If traducir And Request("lstIdioma") <> Request("IDIOMATRADUCCIO") And Request("lstIdioma") <> 0 And Request("IDIOMATRADUCCIO") <> 0 Then
            Select Case Request("lstIdioma")
                Case 1
                    direccio = "CA"
                Case 2
                    direccio = "ES"
                Case 3
                    direccio = "EN"
            End Select
            Select Case Request("IDIOMATRADUCCIO")
                Case 1
                    direccio += "_CA"
                Case 2
                    direccio += "_ES"
                Case 3
                    direccio += "_EN"
            End Select
        End If
        If titulo.length > 0 Then
            ' clTradAuto.ferTraduccio(direccio, titulo)
        End If
        If alt.length > 0 Then
            ' clTradAuto.ferTraduccio(direccio, alt)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Page.IsPostBack Then
            previsualitzarImatges()
        End If
    End Sub

End Class
