Imports System.Data
Imports System.Data.OleDb

Public Class frmestructura
    Inherits System.Web.UI.Page

    Public Shared nif As String
    Public Shared objconn As OleDbConnection


    Private Sub Page_UnLoad(sender As Object, e As System.EventArgs) Handles MyBase.Unload
        GAIA.bdFi(objconn)
    End Sub 'Page_UnLoad

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        Dim relOrigen As New clsRelacio
        Dim relDesti As New clsRelacio
        Dim dbRow As DataRow
        Dim ds As DataSet, prefix As String
        objconn = GAIA.bdIni()

        If HttpContext.Current.User.Identity.Name.Length > 0 Then
            If (Session("nif") Is Nothing) Then
                'Session("nif") = GAIA.nifUsuari(objconn, HttpContext.Current.User.Identity.Name).Trim()
            End If
            If Session("codiOrg") Is Nothing Then
                'Session("CodiOrg") = GAIA.trobaNodeUsuari(objconn, Session("nif")).ToString().Trim()
            End If
        End If
        'nif = Session("nif").Trim()
        'idUsuari = "346231"
        Session("codiOrg") = "346231"
        If Not Page.IsPostBack Then
            nroArbreOrigen.Text = Request("nroArbreOrigen")
            nroArbreDesti.Text = Request("nroArbreDesti")
            nroNodeOrigen.Text = Request("nroNodeOrigen")
            nroNodeDesti.Text = Request("nroNodeDesti")
            nodePathVell.Text = Request("nodePathVell")
            nodePathNou.Text = Request("nodePathNou")
            nroNodePareAnterior.Text = Request("nroNodePareAnterior")
            codiRelacioOrigen.Text = Request("codiRelacioOrigen")
            codiRelacioDesti.Text = Request("codiRelacioDesti")
            codiRelacioDestiInicial.Text = Request("codiRelacioDesti")
            txtDragDrop.Text = Request("dragdrop")
            relOrigen.bdget(objconn, codiRelacioOrigen.Text)
            relDesti.bdget(objconn, codiRelacioDesti.Text)


            'Comprovo el permís, si no en té no deixo copiar.
            If clsPermisos.tepermis(objconn, 7, Session("codiOrg"), Session("codiOrg"), relDesti, 1, "", "", 0) <> 1 Then
                pnlError.Visible = True
                pnlEditar.Visible = False
                ltErr.Text = "No tens permisos per fer aquesta acció."
                pnlBotonsOk.Visible = False
                pnlBotonsSensePermis.Visible = True
            Else
                pnlBotonsOk.Visible = True
                pnlBotonsSensePermis.Visible = False

                InicialitzaEstructura(relOrigen, relDesti, 1)
                chkVisibleInternet.Checked = relOrigen.swvis
                nomTaula.Text = obtenirNomTaula(nroNodeOrigen.Text)
                If relDesti.tipdsdes = "fulla web" Or relDesti.tipdsdes = "node estructura" Then
                    tbDates.Visible = True
                    carrega_dates()

                End If

                If InStr(relDesti.cdher, "_5286") > 0 Then
                    pnlVisibilitat.Visible = False
                End If
                carrega_plantilles()
            End If
        Else


            'no hago nada. 
        End If

    End Sub 'Page_Load

    Private Function obtenirNomTaula(ByVal codiNode As String) As String
        Dim ds As DataSet, nom As String
        ds = New DataSet
        GAIA.bdr(objconn, "SELECT isnull(TBLDSTXT,'') as TBLDSTXT FROM METLNOD WITH(NOLOCK) LEFT JOIN METLTBL WITH(NOLOCK) ON NODCDTIP = TBLINTFU WHERE NODINNOD = " & codiNode, ds)
        If ds.Tables(0).Rows.Count > 0 Then
            nom = Trim(ds.Tables(0).Rows(0).Item(0))
        End If
        Return nom
    End Function




    Private Sub carrega_dates()
        If nomTaula.Text <> "" Then
            Dim ds As New DataSet, prefix As String
            Dim horaP As String, horaC As String
            tbDates_cat.Visible = True

            GAIA.bdr(objconn, "SELECT REIDTPUB,REIDTCAD FROM METLREI WITH(NOLOCK) WHERE REIINCOD = " & codiRelacioOrigen.Text & " AND REIINIDI = 1", ds)
            'si no hem trobat les dades a METLREI les busquem a la taula principal del node

            If ds.Tables(0).Rows.Count = 0 Then
                prefix = Right(nomTaula.Text, 3)
                ds = New DataSet

                GAIA.bdr(objconn, "SELECT " & prefix & "DTPUB, " & prefix & "DTCAD FROM " & nomTaula.Text & " WITH(NOLOCK) WHERE " & prefix & "INNOD = " & nroNodeOrigen.Text, ds)

            End If
            If ds.Tables(0).Rows.Count > 0 Then
                horaP = ds.Tables(0).Rows(0).Item(0).ToString()
                horaC = ds.Tables(0).Rows(0).Item(1).ToString()
                REIDTPUB.Text = horaP.Substring(0, 10)
                If Not horaC.Substring(0, 10).Equals("01/01/1900") Then
                    REIDTCAD.Text = horaC.Substring(0, 10)
                Else
                    REIDTCAD.Text = ""
                End If

                If Len(horaP) < 11 Then
                    horaPublicacio.Text = ""
                Else
                    horaPublicacio.Text = horaP.Substring(11, horaP.LastIndexOf(":") - 11)
                End If
                If Len(horaC) < 11 Then
                    horaCaducitat.Text = ""
                Else
                    horaCaducitat.Text = horaC.Substring(11, horaC.LastIndexOf(":") - 11)
                End If
            End If
        End If
    End Sub


    'procediment per carregar el desplegable de plantilles
    Private Sub carrega_plantilles()

        Dim ds As New DataSet, qSQL As String, item As String, aPlantilles As String()
        Dim i As Integer, plantillaActual As Integer
        Dim ddlb_plantilla As DropDownList

        qSQL = "SELECT ISNULL(RELINPLA,-1) AS RELINPLA FROM METLREL WITH(NOLOCK) WHERE RELINCOD = " & codiRelacioOrigen.Text
        GAIA.bdr(objconn, qSQL, ds)

        plantillaActual = ds.Tables(0).Rows(0).Item("RELINPLA")

        aPlantilles = Split(llistaPlantilles.Text, ",")

        i = 0
        For Each item In aPlantilles

            item = Replace(item, "|", ",")
            item = Trim(item)
            ddlb_plantilla = New DropDownList
            ddlb_plantilla.ID = "ddlb_plantillat" & i
            If Trim(item & "") <> "" Then
                'he vist que a vegades arriba un element amb format  codiplantilla1|codiplantilla2| (amb la barra al final). L'elimino.
                If item.Substring(item.Length - 1) = "," Then item = item.Substring(0, item.Length - 1)

                qSQL = "SELECT PLTINNOD,CAST(PLTDSOBS AS VARCHAR(8000)) AS PLTDSOBS FROM METLPLT WITH(NOLOCK) WHERE PLTINNOD IN (" & item & ")  UNION SELECT PLTINNOD,CAST(PLTDSOBS AS VARCHAR(8000)) AS PLTDSOBS FROM METLPLT2 WITH(NOLOCK) WHERE PLTINNOD IN (" & item & ") "
                '				gaia.debug(nothing, qsql)
                GAIA.bdr(objconn, qSQL, ds)
                ddlb_plantilla.DataSource = ds
                ddlb_plantilla.DataTextField = "PLTDSOBS"
                ddlb_plantilla.DataValueField = "PLTINNOD"
                ddlb_plantilla.DataBind()
                If ddlb_plantilla.Items.Count > 1 Then
                    ddlb_plantilla.Items.Add(New ListItem("Plantilles alternatives", 9))
                End If
                If Trim(txtPosicioEstructuraReal.Text) <> "" Then
                    If i = CInt(txtPosicioEstructuraReal.Text) And plantillaActual <> -1 Then
                        Try
                            ddlb_plantilla.SelectedValue = plantillaActual
                        Catch
                        End Try
                    End If
                End If
            End If
            ddlb_plantilla.Visible = True
            plantillesPH.Controls.Add(ddlb_plantilla)
            i += 1
        Next item
        ds.Dispose()
    End Sub

    'borra els desplegables de plantilles generats dinàmicament
    'la utilitzarem per poder generar de nou els desplegables quan es canvia de "moure" a "inserir" i vicerversa
    Protected Sub borra_plantilles()
        Dim aPlantilles As String(), i As Integer, item As String
        Dim ddlb_plantilla As DropDownList

        aPlantilles = Split(llistaPlantilles.Text, ",")
        i = 0
        For Each item In aPlantilles
            ddlb_plantilla = Page.FindControl("ddlb_plantillat" & i)
            plantillesPH.Controls.Remove(ddlb_plantilla)
            i += 1
        Next item
    End Sub

    'Prepara la crida a GAIA.pintaEstructura 
    Protected Sub InicialitzaEstructura(ByVal relOrigen As clsRelacio, ByVal relDesti As clsRelacio, ByVal idioma As Integer)
        Dim nomTipusNroNodeDesti, nomTipusNroNodeAMoure As String
        Dim nroNodeDesti As Integer
        Dim tipusNodeDesti, tipusNodeAMoure As Integer
        Dim posicioEstructura As Integer
        Dim posicioEstructuraReal As Integer
        Dim tmp As Integer
        Dim valorCodiRelacioOrigen, codiRelacioDesti As Integer
        Dim relSuperior As New clsRelacio
        'ini codi sara
        Dim arrayDescripcions As String()
        Dim arrayBuit As String()
        'fi codi sara
        valorCodiRelacioOrigen = relOrigen.incod
        codiRelacioDesti = relDesti.incod
        lblCodi.Text = "<script>document.getElementById(""WEBDSTCO"").value="""";</script> "
        'Inicialitzo a blancs l'estructura
        lblEstructura.Text = ""
        tipusNodeAMoure = relOrigen.tipintip
        tipusNode.Text = tipusNodeAMoure
        tipusNodeDesti = relDesti.tipintip
        nroNodeDesti = relDesti.infil


        posicioEstructura = GAIA.assignaAutomaticamentCella(objconn, tipusNodeAMoure, relDesti, 0, idioma, tmp, 0)

        relSuperior = GAIA.obtenirRelacioSuperior(objconn, relOrigen)
        If relSuperior.incod = codiRelacioDesti Then
            posicioEstructuraReal = relOrigen.cdest
        Else
            posicioEstructuraReal = 0
        End If
        txtPosicioEstructuraReal.Text = posicioEstructuraReal.ToString()
        txtposicioEstructura.Text = posicioEstructura.ToString()

        moureFills.Text = Request("moureFills")
        If codiRelacioOrigen.Text.Length > 0 Then

            Dim DS As DataSet
            Dim dbRow As DataRow
            Dim descTipus As String
            Dim tipus As Integer
            '				tipus  = GAIA.tipusNodebyNro(objConn,nroNodeDesti, descTipus)
            descTipus = relDesti.tipdsdes
            DS = New DataSet()
            Select Case descTipus
                Case "fulla web"

                    lblTitol.Text = "Estructura de la pàgina web"

                    GAIA.bdr(objconn, "SELECT WEBDSPLA,WEBDSEST,WEBDSATR,WEBDSTVER,WEBDSTHOR,WEBDSDEC,WEBDSPLA,WEBDSTCO FROM METLWEB WITH(NOLOCK) WHERE WEBINNOD=" & nroNodeDesti & " UNION SELECT WEBDSPLA, CAST (WEBDSEST AS VARCHAR(8000)) AS WEBDSEST,'' as WEBDSATR,'0' as WEBDSTVER,'0' as WEBDSTHOR,'' as WEBDSDEC,WEBDSPLA,WEBDSTCO  FROM METLWEB2 WITH(NOLOCK) WHERE WEBINNOD=" & nroNodeDesti, DS)
                    '						gaia.debug(nothing, "SELECT WEBDSPLA,WEBDSEST,WEBDSATR,WEBDSTVER,WEBDSTHOR,WEBDSDEC,WEBDSPLA,WEBDSTCO FROM METLWEB WITH(NOLOCK) WHERE WEBINNOD=" & nroNodeDesti  & " UNION SELECT WEBDSPLA, CAST (WEBDSEST AS VARCHAR(8000)) AS WEBDSEST,'' as WEBDSATR,0 as WEBDSTVER,0 as WEBDSTHOR,'' as WEBDSDEC,WEBDSPLA,WEBDSTCO  FROM METLWEB2 WITH(NOLOCK) WHERE WEBINNOD=" & nroNodeDesti)				
                    If DS.Tables(0).Rows.Count > 0 Then
                        dbRow = DS.Tables(0).Rows(0)
                        '							gaia.debug(nothing, "1")
                        llistaPlantilles.Text = dbRow("WEBDSPLA")
                        If posicioEstructura < 0 Then
                            Dim a As String()
                            a = Split(dbRow("WEBDSEST"), ",")
                            Array.Sort(a)
                            'ini codi sara								

                            lblEstructura.Text = GAIA.pintaEstructura(objconn, a, Split(dbRow("WEBDSATR"), ","), Split(dbRow("WEBDSTVER"), ","), Split(dbRow("WEBDSTHOR"), ","), 1, "", 1, "", Split(dbRow("WEBDSDEC"), "|"), Split(dbRow("WEBDSPLA"), ","), 0)
                            'fi codi sara
                            lblCodi.Text = "<script>document.getElementById(""WEBDSTCO"").value=""" & dbRow("WEBDSTCO").ToString() & """;document.getElementById(""tipusNode"").value=""" & tipusNode.Text.ToString() & """;document.getElementById(""codiRelacioOrigen"").value=""" & codiRelacioOrigen.Text.ToString() & """;document.getElementById(""txtPosicioEstructuraReal"").value=""" & posicioEstructuraReal.ToString() & """;</script> "
                        Else
                            lblCodi.Text = "<script>document.getElementById(""codiRelacioOrigen"").value=""" & codiRelacioOrigen.Text.ToString() & """;</script> "
                        End If

                    End If
                Case "node web"
                    lblTitol.Text = "Estructura del node web"
                    GAIA.bdr(objconn, "SELECT NWEDSPLA,NWEDSEST ,NWEDSATR,NWEDSTVER,NWEDSTHOR,NWEDSTCO FROM METLNWE WITH(NOLOCK) WHERE NWEINNOD=" & nroNodeDesti & " UNION SELECT NWEDSPLA,CAST (NWEDSEST AS VARCHAR(8000)) AS NWEDSEST ,'' as NWEDSATR,'0' as NWEDSTVER,'0' as NWEDSTHOR,NWEDSTCO FROM METLNWE2 WITH(NOLOCK) WHERE NWEINNOD=" & nroNodeDesti, DS)
                    '						gaia.debug(nothing, "SELECT NWEDSPLA,NWEDSEST ,NWEDSATR,NWEDSTVER,NWEDSTHOR,NWEDSTCO FROM METLNWE WITH(NOLOCK) WHERE NWEINNOD="&nroNodeDesti  & " UNION SELECT NWEDSPLA,CAST (NWEDSEST AS VARCHAR(8000)) AS NWEDSEST ,'' as NWEDSATR,0 as NWEDSTVER,0 as NWEDSTHOR,NWEDSTCO FROM METLNWE2 WITH(NOLOCK) WHERE NWEINNOD="&nroNodeDesti)
                    If DS.Tables(0).Rows.Count > 0 Then

                        dbRow = DS.Tables(0).Rows(0)

                        llistaPlantilles.Text = dbRow("NWEDSPLA")

                        If posicioEstructura < 0 Then
                            Dim a As String()
                            a = Split(dbRow("NWEDSEST"), ",")
                            Array.Sort(a)
                            lblEstructura.Text = GAIA.pintaEstructura(objconn, a, Split(dbRow("NWEDSATR"), ","), Split(dbRow("NWEDSTVER"), ","), Split(dbRow("NWEDSTHOR"), ","), 1, "", 1, "", arrayDescripcions, arrayBuit, 0)
                            lblCodi.Text = "<script>document.getElementById(""WEBDSTCO"").value=""" & dbRow("NWEDSTCO").ToString() & """;document.getElementById(""tipusNode"").value=""" & tipusNode.Text.ToString() & """;document.getElementById(""codiRelacioOrigen"").value=""" & codiRelacioOrigen.Text.ToString() & """;document.getElementById(""txtPosicioEstructuraReal"").value=""" & posicioEstructuraReal.ToString() & """;</script> "
                        End If
                    End If
                Case "arbre web"
                    lblTitol.Text = "Estructura de l'arbre web"
                    GAIA.bdr(objconn, "SELECT AWEDSEST,AWEDSATR,AWEDSTVER,AWEDSTHOR,AWEDSTCO FROM METLAWE WITH(NOLOCK) WHERE AWEINNOD=" & nroNodeDesti & " UNION SELECT  CAST(AWEDSEST AS VARCHAR(8000)) AS AWEDSEST,'' as AWEDSATR,'0' as AWEDSTVER,'0' as AWEDSTHOR,AWEDSTCO FROM METLAWE2 WITH(NOLOCK) WHERE AWEINNOD=" & nroNodeDesti, DS)
                    If DS.Tables(0).Rows.Count > 0 Then
                        dbRow = DS.Tables(0).Rows(0)
                        If posicioEstructura < 0 Then
                            Dim a As String()
                            a = Split(dbRow("AWEDSEST"), ",")
                            Array.Sort(a)
                            lblEstructura.Text = GAIA.pintaEstructura(objconn, a, Split(dbRow("AWEDSATR"), ","), Split(dbRow("AWEDSTVER"), ","), Split(dbRow("AWEDSTHOR"), ","), 1, "", 1, "", arrayDescripcions, arrayBuit, 0)
                            lblCodi.Text = "<script>document.getElementById(""WEBDSTCO"").value=""" & dbRow("AWEDSTCO").ToString() & """;document.getElementById(""tipusNode"").value=""" & tipusNode.Text.ToString() & """;document.getElementById(""codiRelacioOrigen"").value=""" & codiRelacioOrigen.Text.ToString() & """;document.getElementById(""txtPosicioEstructuraReal"").value=""" & posicioEstructuraReal.ToString() & """;</script> "
                        End If
                    End If
                Case Else
                    'No es una fulla web i he de identificar la plantilla per defecte que tindrà el tipus de contingut.
                    Dim codiRelacio As Integer
                    'codiRelacio=GAIA.obtenirRelacioSuperior(objConn,valorCodiRelacioOrigen)
                    Dim codiPlantilla As String = GAIA.plantillaPerDefecte(objconn, relDesti, 1)
                    GAIA.bdr(objconn, "SELECT  CAST(PLTDSEST AS VARCHAR(8000)) AS PLTDSEST,CAST(PLTDSATR AS VARCHAR(8000)) AS PLTDSATR,CAST(PLTDSVER AS VARCHAR(8000)) AS  PLTDSVER,CAST(PLTDSHOR AS VARCHAR(8000)) AS PLTDSHOR,CAST(PLTDSOBS AS VARCHAR(8000)) AS PLTDSOBS, CAST(PLTDSTCO AS VARCHAR(8000)) AS PLTDSTCO FROM METLPLT WITH(NOLOCK) WHERE PLTINNOD=" & codiPlantilla & " UNION SELECT CAST(PLTDSEST AS VARCHAR(8000)) AS PLTDSEST,'' as PLTDSATR,'0' as PLTDSVER,'0' as PLTDSHOR,CAST(PLTDSOBS AS VARCHAR(8000)) AS PLTDSOBS,  CAST(PLTDSTCO AS VARCHAR(8000)) AS PLTDSTCO  FROM METLPLT2 WITH(NOLOCK) WHERE PLTINNOD=" & codiPlantilla, DS)
                    If DS.Tables(0).Rows.Count > 0 Then
                        dbRow = DS.Tables(0).Rows(0)
                        If posicioEstructura < 0 Then
                            Dim a As String()
                            a = Split(dbRow("PLTDSEST"), ",")
                            Array.Sort(a)
                            lblEstructura.Text = GAIA.pintaEstructura(objconn, a, Split(dbRow("PLTDSATR"), ","), Split(dbRow("PLTDSVER"), ","), Split(dbRow("PLTDSHOR"), ","), 1, "", 1, "", arrayDescripcions, arrayBuit, 0)
                            lblTitol.Text = "Estructura de la plantilla." + dbRow("PLTDSOBS")
                            lblCodi.Text = "<script>document.getElementById(""WEBDSTCO"").value=""" & dbRow("PLTDSTCO").ToString() & """;document.getElementById(""tipusNode"").value=""" & tipusNode.Text.ToString() & """;document.getElementById(""codiRelacioOrigen"").value=""" & codiRelacioOrigen.Text.ToString() & """;document.getElementById(""txtPosicioEstructuraReal"").value=""" & posicioEstructuraReal.ToString() & """;</script> "
                        End If
                    End If
            End Select

            DS.Dispose()
        End If
    End Sub 'InicialitzaEstructura


    Protected Sub clickModificaEstructura(sender As Object, e As EventArgs)
        Dim relOrigen As New clsRelacio
        Dim relDesti As New clsRelacio
        relOrigen.bdget(objconn, codiRelacioOrigen.Text)
        relDesti.bdget(objconn, codiRelacioDesti.Text)

        If accio.SelectedItem.Value = "moure" Then 'copiar
            Dim strnodePathNou As String
            If nodePathNou.Text.Length > 0 Then
                strnodePathNou = nodePathNou.Text.Substring(0, InStrRev(nodePathNou.Text, "_") - 1)
            End If

            fesMoviment(txtDragDrop.Text, nroArbreOrigen.Text, nroNodeOrigen.Text, relOrigen, strnodePathNou, ultimaDivisio.Text, nroArbreDesti.Text, relDesti.inpar, GAIA.obtenirRelacioSuperior(objconn, relDesti), txtposicioEstructura.Text)

        Else 'Insertar
            fesMoviment(txtDragDrop.Text, nroArbreOrigen.Text, nroNodeOrigen.Text, relOrigen, nodePathNou.Text, ultimaDivisio.Text, nroArbreDesti.Text, nroNodeDesti.Text, relDesti, txtposicioEstructura.Text)
        End If


    End Sub 'clickModificaEstructura

    Protected Sub clickCancelar(sender As Object, e As EventArgs)
        Dim relOrigen As New clsRelacio
        Dim relSuperior As New clsRelacio
        relOrigen.bdget(objconn, Request("codiRelacioOrigen"))
        relSuperior = GAIA.obtenirRelacioSuperior(objconn, relOrigen)

        If Request("direccio") = "1" Or Request("direccio") = ">" Then

            lblCodi.Text = "<script>opener.document.getElementById(""actualitzaNode"").value=""" + relSuperior.incod.ToString() + """;opener.document.forms[""form1""].submit();window.close()</script>"
        Else
            lblCodi.Text = "<script>opener.document.getElementById(""actualitzaNodeArbre2"").value=""" + relSuperior.incod.ToString() + """;opener.document.forms[""form1""].submit();window.close()</script>"

        End If
    End Sub 'clickCancelar


    'Si l'usuari canvia el tipus de moviment que vol fer, he de trobar el nou node destí per poder oferir-li
    'a quina posició de l'estructura dessitja afegir el node.
    Protected Sub canviarNodeDesti(sender As Object, e As EventArgs)
        Dim relOrigen As New clsRelacio
        Dim relDesti As New clsRelacio
        borra_plantilles()
        relOrigen.bdget(objconn, codiRelacioOrigen.Text)
        relDesti.bdget(objconn, codiRelacioDesti.Text)
        ultimaDivisio.Text = ""
        WEBDSTCO.Text = ""
        If accio.SelectedItem.Value = "moure" Then
            InicialitzaEstructura(relOrigen, GAIA.obtenirRelacioSuperior(objconn, relDesti), 1)
        Else
            InicialitzaEstructura(relOrigen, relDesti, 1)
        End If
        carrega_plantilles()
    End Sub





    Protected Sub fesMoviment(ByVal moure As String, ByVal nroArbreOrigen As Integer, ByVal nroNodeOrigen As Integer, ByVal relOrigen As clsRelacio, ByVal nodePathNou As String, ByVal ultimaDivisio As String, ByVal nroArbreDesti As Integer, ByVal nroNodeDesti As Integer, ByVal relDesti As clsRelacio, ByVal txtposicioEstructura As String)


        Dim tipusMoviment As Integer
        Dim moureFills As Integer
        Dim codiRelacioDestiInicial As String
        Dim novaRelacio As Integer
        Dim direccio As String
        Dim codiRelacioOrigen, codiRelacioDesti As Integer
        Dim relNova As New clsRelacio
        Dim relTmp As New clsRelacio
        Dim relDestiInicial As New clsRelacio
        Dim relDestiSuperior As New clsRelacio
        Dim plantillaRelacio As String = ""
        Dim sqlUpdate As String = ""


        If InStr(ultimaPlantilla.Text, "|") > 0 Then
            'la cel·la conté diverses plantilles, agafarem la que l'usuari hagi escollit a través
            'del desplegable	


            '		plantillaRelacio = CType(Page.FindControl("ddlb_plantilla" & ultimaDivisio), DropDownList).SelectedValue()
            plantillaRelacio = Request.Form("ddlb_plantilla" & ultimaDivisio)

        Else
            'la cel·la només té una plantilla possible.
            plantillaRelacio = ultimaPlantilla.Text
        End If


        If Trim(plantillaRelacio) = "" Or plantillaRelacio = "undefined" Then plantillaRelacio = "NULL"

        codiRelacioOrigen = relOrigen.incod
        codiRelacioDesti = relDesti.incod
        relDestiInicial.bdget(objconn, Request("codiRelacioDestiInicial"))
        direccio = Request("direccio")
        codiRelacioDestiInicial = relDestiInicial.incod
        '*************************************************
        '*************************************************
        'Copia de nodes
        '*************************************************
        '*************************************************
        Dim relbuida As New clsRelacio

        If moure = "0" Then 'copiar
            Dim nodesOrg(), permisos() As Integer
            Dim nroNodesOrg, cont, cont2 As Integer
            Dim debug As String

            If ultimaDivisio.ToString().Length > 0 Then
                relNova = GAIA.creaRelacio(objconn, nroArbreDesti, nroNodeDesti, nroNodeOrigen, 0, nodePathNou, ultimaDivisio.Substring(1), relOrigen.cdsit, relOrigen.cdord, relOrigen.swvis, False, Session("codiOrg"))

            Else
                relNova = GAIA.creaRelacio(objconn, nroArbreDesti, nroNodeDesti, nroNodeOrigen, 0, nodePathNou, txtposicioEstructura, relOrigen.cdsit, relOrigen.cdord, relOrigen.swvis, False, Session("codiOrg"))
            End If
            If relNova.incod <> 0 Then
                novaRelacio = relNova.incod

                'establim la plantilla de la relació i la visibilitat
                sqlUpdate = " RELINPLA=" & plantillaRelacio & ", RELSWVIS=" & IIf(chkVisibleInternet.Checked, 1, 0) & sqlUpdate

                GAIA.bdSR(objconn, "UPDATE METLREL SET " & sqlUpdate & " WHERE RELINCOD=" & novaRelacio)

                If relDesti.tipdsdes = "fulla web" Or relDesti.tipdsdes = "node estructura" Then
                    actualitzar_dates(relNova.incod)
                End If
                GAIA.log(objconn, relNova, Session("codiOrg"), "", GAIA.TAINSERTAR)
                '**************************************************************				
                'Afegeixo la informació pel manteniment automàtic
                '**************************************************************		

                GAIA.afegeixAccioManteniment(objconn, relNova, 0, 99, Now, Now, relbuida, txtposicioEstructura, 0, True)

                nroNodesOrg = -1
                ' Assigno els permisos manuals en la nova ubicació				

                '				clsPermisos.permisosManuals(objconn,relOrigen, nodesOrg, permisos, nroNodesOrg)
                '				For cont=0 to nroNodesOrg				
                'clsPermisos.assignarPermisos(objconn,permisos(cont), nodesOrg(cont),  nroNodeOrigen, relNova, 0, debug,0 )								
                '					clsPermisos.assignarPermisos(objconn,permisos(cont), nodesOrg(cont),   relNova.infil, 0 )	
                '				Next

                '				clsPermisos.assignaPermisLlistarNode(objconn, 0, relNova)	
                ' Si el node que copio no està expandit, llavors he de copiar, també, tots els fills
                If UCase(Request("moureFills")) = "TRUE" Then
                    'Trobo la llista de fills per la relació on es troba el node 

                    Dim nodesFills() As Integer
                    Dim nroFills, nroNodePare As Integer
                    Dim relacions() As Integer
                    Dim RelacionsDesti() As Integer
                    Dim relacio As Integer
                    Dim posicioEstructuraFills As Integer
                    Dim nodePath As String
                    Dim nroNodeDestiFill As Integer
                    ReDim nodesFills(0)
                    ReDim relacions(0)
                    ReDim RelacionsDesti(0)
                    nroFills = 0
                    cont = 0
                    GAIA.trobaFills(objconn, relOrigen, nroNodeOrigen, nodesFills, nroFills, relacions, 0, 0)

                    If nroFills > 0 Then
                        nodePath = relNova.cdher
                        nroNodeDestiFill = relNova.infil
                        For Each relacio In relacions
                            relTmp.bdget(objconn, relacio)

                            posicioEstructuraFills = relTmp.cdest
                            fesMoviment(moure, nroArbreOrigen, nodesFills(cont), relTmp, nodePath + "_" + nroNodeDestiFill.ToString(), ultimaDivisio, nroArbreDesti, nroNodeDestiFill, relNova, posicioEstructuraFills)
                            cont += 1
                        Next relacio
                    End If
                    'Assigno els permisos heretats a la nova ubicació			
                    clsPermisos.tractaPermisosHeretats(objconn, "", nroNodeDestiFill, "", "", 0)
                    'moure node, insertarAlFinal=0
                End If
                GAIA.canviOrdre(objconn, relNova, relDesti, 0, lstOrdre.SelectedItem.Value, relDestiInicial)
            End If
            '*************************************************
            '*************************************************
            'Moviment de nodes
            '*************************************************
            '*************************************************
        Else
            'Faig el moviment del node i l'assigno a l'ultima posició de l'ordre de nodes que penjen de 
            If accio.SelectedItem.Value = "insertar" Then
                tipusMoviment = 1
            Else 'moure
                tipusMoviment = 0
            End If
            If UCase((Request("moureFills"))) = "TRUE" Then
                moureFills = 1
            Else
                moureFills = 0
            End If

            GAIA.log(objconn, relOrigen, Session("codiOrg"), "", GAIA.TAMOURE1)
            Dim posEst As Integer = 0
            If ultimaDivisio.Length > 0 Then
                posEst = ultimaDivisio.Substring(1)
            End If
            Try
                'gaia.debug(objconn, "usu=" & session("CodiOrg") & " - relOrigen=" & relOrigen.incod & ", relDesti=" & relDesti.incod & ", moureFills=" & moureFills & ",posEst=" &   posEst & ", tipusMoviment=" & tipusMoviment & ",relDestiInicial= " & relDestiInicial.incod & ", lstOrdre.selectedItem.value=" & lstOrdre.selectedItem.value)		
            Catch
                GAIA.debug(objconn, "problema al moure")
            End Try



            GAIA.moureNode(objconn, relOrigen, relDesti, moureFills, posEst, tipusMoviment, relDestiInicial, lstOrdre.SelectedItem.Value)

            GAIA.log(objconn, relDesti, Session("codiOrg"), "", GAIA.TAMOURE2)
            If relDesti.tipdsdes = "fulla web" Or relDesti.tipdsdes = "node estructura" Then
                actualitzar_dates(codiRelacioOrigen.ToString())
            End If

            ' Faig el canvi de cel·la de l'estructura

            If ultimaDivisio.ToString().Length > 0 Then
                sqlUpdate = " ,RELCDEST=" & ultimaDivisio.Substring(1)
            End If
            'EVS 16/01/2006		
            'establim la plantilla de la relació i la visibilitat
            sqlUpdate = " RELINPLA=" & plantillaRelacio & ", RELSWVIS=" & IIf(chkVisibleInternet.Checked, 1, 0) & sqlUpdate

            GAIA.bdSR(objconn, "UPDATE METLREL SET " & sqlUpdate & " WHERE RELINCOD=" & codiRelacioOrigen.ToString())





        End If

        'Actualitzo el canvi a la pantalla				
        If Request("dragdrop") = 1 And (direccio = "1" Or direccio = "<") Then 'actualitzo l'arbre 1 perque és un dragdrop al mateix arbre o bé de l'arbre 2 a l'arbre 1			
            lblCodi.Text = "<script>opener.document.getElementById(""actualitzaNode"").value=""" + relDesti.incod.ToString() + """;opener.document.forms[""form1""].submit();window.close()</script>"
        Else 'actualitzo l'arbre 2 perque és una còpia entre arbres o bé un dragdrop de l'arbre "1" a l'arbre "2" o bé un dragdrop dins de l'arbre 2						
            lblCodi.Text = "<script>opener.document.getElementById(""actualitzaNodeArbre2"").value=""" + relDesti.incod.ToString() + """;opener.document.forms[""form1""].submit();window.close()</script>"
        End If

    End Sub 'fesMoviment

    Private Sub actualitzar_dates(ByVal codiRelacio As String)
        Dim prefix As String
        Dim sSQL As String
        Dim sqlDataIni, sqlDataFi As String
        prefix = Right(nomTaula.Text, 3)
        Dim dataIni, dataFI As DateTime
        Dim rel As New clsRelacio
        rel.bdget(objconn, codiRelacio)
        GAIA.bdSR(objconn, "DELETE FROM METLREI  WHERE REIINCOD = " & codiRelacio)
        If tbDates.Visible Then

            sqlDataIni = REIDTPUB.Text & " " & horaPublicacio.Text
            sqlDataFi = REIDTCAD.Text & " " & horaCaducitat.Text
            If REIDTPUB.Text = "" Or REIDTCAD.Text = "" Then
                GAIA.datesPublicacio(objconn, rel, 1, dataIni, dataFI)
                If REIDTPUB.Text = "" Then
                    sqlDataIni = dataIni
                End If
                If REIDTCAD.Text = "" Then
                    sqlDataFi = dataFI
                End If
            End If
            'Català
            sSQL = "INSERT INTO METLREI (REIINCOD,REIINIDI,REIDTPUB,REIDTCAD,REIDSFIT) VALUES " &
            "(" & codiRelacio & "," & "1,'" & CDate(sqlDataIni) & "','" &
            CDate(sqlDataFi) & "','')"
            GAIA.bdSR(objconn, sSQL)

            'Castellà
            sSQL = "INSERT INTO METLREI (REIINCOD,REIINIDI,REIDTPUB,REIDTCAD,REIDSFIT) VALUES " &
            "(" & codiRelacio & "," & "2,'" & CDate(sqlDataIni) & "','" &
            CDate(sqlDataFi) & "','')"
            GAIA.bdSR(objconn, sSQL)


            If CDate(sqlDataFi) < Now And sqlDataFi > CDate("01/01/1900") Then
                GAIA.bdSR(objconn, "UPDATE METLREL SET RELCDSIT=98 WHERE RELINCOD=" & rel.incod)
            End If

        End If

        If sqlDataIni = "" Then sqlDataIni = Now
        If sqlDataFi = "" Then sqlDataFi = Now
        'Faig el manteniment per les dates de caducitat i publicació.
        'En realitat, ja s'ha fet el manteniment en el crearRelacio al fer el moviment. El correcte és que els canvis en METLREI s'haguessin fet abans de cridar al
        'afegeixacciomanteniment dins del crearrelacio, però no tinc temps per fer-ho...

        GAIA.afegeixAccioManteniment(objconn, rel, 0, 99, sqlDataIni, sqlDataFi, rel, 1, 0, True)



    End Sub


End Class