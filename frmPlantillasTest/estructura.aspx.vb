Imports System.Data
Imports System.Data.OleDb

Public Class estructura
    Inherits System.Web.UI.Page

    Public Shared nif As String
    Public Shared objconn As OleDbConnection


    Private Sub Page_UnLoad(sender As Object, e As System.EventArgs) Handles MyBase.Unload
        GAIA.bdFi(objconn)
    End Sub 'Page_UnLoad

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim relOrigen As New clsRelacio
        Dim relDesti As New clsRelacio
        Dim dbRow As DataRow
        Dim ds As DataSet, prefix As String
        objconn = GAIA.bdIni()

        If HttpContext.Current.User.Identity.Name.Length > 0 Then
            If (Session("nif") Is Nothing) Then
                Session("nif") = GAIA.nifUsuari(objconn, HttpContext.Current.User.Identity.Name).Trim()
            End If
            If Session("codiOrg") Is Nothing Then
                'Session("CodiOrg") = GAIA.trobaNodeUsuari(objconn, Session("nif")).ToString().Trim()
                Session("codiOrg") = "346231"

            End If
        End If
        'nif = Session("nif").Trim()
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
            'If clsPermisos.tepermis(objconn, 7, Session("codiOrg"), Session("codiOrg"), relDesti, 1, "", "", 0) <> 1 Then
            '    pnlError.Visible = True
            '    pnlEditar.Visible = False
            '    ltErr.Text = "No tens permisos per fer aquesta acció."
            '    pnlBotonsOk.Visible = False
            '    pnlBotonsSensePermis.Visible = True
            'Else
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
            'End If

        Else

            'no hago nada. 
        End If
    End Sub

    Private Function obtenirNomTaula(ByVal codiNode As String) As String
        Dim ds As DataSet, nom As String
        ds = New DataSet
        GAIA.bdr(objconn, "Select isnull(TBLDSTXT,'') as TBLDSTXT FROM METLNOD WITH(NOLOCK) LEFT JOIN METLTBL WITH(NOLOCK) ON NODCDTIP = TBLINTFU WHERE NODINNOD = " & codiNode, ds)
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
        Dim html As String()
        Dim i As Integer, plantillaActual As Integer
        Dim ddlb_plantilla As DropDownList

        Dim esGaia2 As Boolean = False
        ''		 gaia.debug(nothing, "aqui")
        qSQL = "SELECT ISNULL(RELINPLA,-1) AS RELINPLA FROM METLREL WITH(NOLOCK) WHERE RELINCOD = " & codiRelacioOrigen.Text
        GAIA.bdr(objconn, qSQL, ds)
        'gaia.debug(nothing,qsql)
        plantillaActual = ds.Tables(0).Rows(0).Item("RELINPLA")

        aPlantilles = Split(llistaPlantilles.Text, ",")

        If lblEstructura.Text <> "" Then
            ' Los id de los desplegables de las plantillas en GAIA empiezan por t y en GAIA2 por d
            html = Split(lblEstructura.Text, "id=")
            'gaia.debug(nothing, lblEstructura.text)
            Try
                If html.Length > 1 Then
                    If html(1).Substring(1, 1) = "d" Then esGaia2 = True
                Else
                    esGaia2 = True
                End If

            Catch ex As Exception
                Debug.WriteLine(ex.InnerException.Message)


            End Try
            'gaia.debug(nothing, esGaia2)
        End If

        i = 0
        For Each item In aPlantilles
            GAIA.debug(Nothing, lblEstructura.Text)
            GAIA.debug(Nothing, item)
            item = Replace(item, "|", ",")
            item = Trim(item)
            ddlb_plantilla = New DropDownList
            ' GAIA2
            If esGaia2 Then
                ddlb_plantilla.ID = "ddlb_plantilla" & html(i + 1).Substring(1, html(i + 1).IndexOf("'", 1) - 1)
            Else
                ddlb_plantilla.ID = "ddlb_plantillat" & i
            End If
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


        posicioEstructura = assignaAutomaticamentCella(objconn, tipusNodeAMoure, relDesti, 0, idioma, tmp, 0)

        relSuperior = GAIA.obtenirRelacioSuperior(objconn, relOrigen)
        If relSuperior.incod = codiRelacioDesti Then
            posicioEstructuraReal = relOrigen.cdest
        Else
            posicioEstructuraReal = 0
        End If
        txtPosicioEstructuraReal.Text = posicioEstructuraReal.ToString()
        txtposicioEstructura.Text = posicioEstructura.ToString()
        ultimaDivisio.Text = "d" & posicioEstructura.ToString()   ' Teresa para que si solo hay una opción la guarde.

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

                    GAIA.bdr(objconn, "SELECT WEBDSPLA,WEBDSEST,WEBDSATR,WEBDSTVER,WEBDSTHOR,WEBDSDEC,WEBDSPLA,WEBDSTCO FROM METLWEB WITH(NOLOCK) WHERE WEBINNOD=" & nroNodeDesti & " UNION SELECT WEBDSPLA, CAST (WEBDSHTM AS VARCHAR(8000)) AS WEBDSEST,'GAIA2' as WEBDSATR,'0' as WEBDSTVER,'0' as WEBDSTHOR,'' as WEBDSDEC,WEBDSPLA,WEBDSTCO  FROM METLWEB2 WITH(NOLOCK) WHERE WEBINNOD=" & nroNodeDesti, DS)
                    '						gaia.debug(nothing, "SELECT WEBDSPLA,WEBDSEST,WEBDSATR,WEBDSTVER,WEBDSTHOR,WEBDSDEC,WEBDSPLA,WEBDSTCO FROM METLWEB WITH(NOLOCK) WHERE WEBINNOD=" & nroNodeDesti  & " UNION SELECT WEBDSPLA, CAST (WEBDSEST AS VARCHAR(8000)) AS WEBDSEST,'' as WEBDSATR,0 as WEBDSTVER,0 as WEBDSTHOR,'' as WEBDSDEC,WEBDSPLA,WEBDSTCO  FROM METLWEB2 WITH(NOLOCK) WHERE WEBINNOD=" & nroNodeDesti)				
                    If DS.Tables(0).Rows.Count > 0 Then
                        dbRow = DS.Tables(0).Rows(0)
                        '							gaia.debug(nothing, "1")
                        llistaPlantilles.Text = dbRow("WEBDSPLA")
                        If posicioEstructura < 0 Then

                            If dbRow("WEBDSATR") = "GAIA2" Then   'Teresa
                                lblEstructura.Text = maquetaEstructura(dbRow("WEBDSEST"), tipusNodeAMoure)
                            Else ' Gaia anterior
                                Dim a As String()
                                a = Split(dbRow("WEBDSEST"), ",")
                                Array.Sort(a)
                                'ini codi sara								
                                lblEstructura.Text = GAIA.pintaEstructura(objconn, a, Split(dbRow("WEBDSATR"), ","), Split(dbRow("WEBDSTVER"), ","), Split(dbRow("WEBDSTHOR"), ","), 1, "", 1, "", Split(dbRow("WEBDSDEC"), "|"), Split(dbRow("WEBDSPLA"), ","), 0)
                                'fi codi sara
                            End If
                            lblCodi.Text = "<script>document.getElementById(""WEBDSTCO"").value=""" & dbRow("WEBDSTCO").ToString() & """;document.getElementById(""tipusNode"").value=""" & tipusNode.Text.ToString() & """;document.getElementById(""codiRelacioOrigen"").value=""" & codiRelacioOrigen.Text.ToString() & """;document.getElementById(""txtPosicioEstructuraReal"").value=""" & posicioEstructuraReal.ToString() & """;</script> "
                        Else 'Ya tiene una posición asignada
                            If dbRow("WEBDSATR") = "GAIA2" Then   'Teresa
                                lblEstructura.Text = maquetaEstructura(dbRow("WEBDSEST"), tipusNodeAMoure)
                            End If
                            lblCodi.Text = "<script>document.getElementById(""codiRelacioOrigen"").value=""" & codiRelacioOrigen.Text.ToString() & """;</script> "
                        End If

                    End If
                Case "node web"
                    lblTitol.Text = "Estructura del node web"
                    GAIA.bdr(objconn, "SELECT NWEDSPLA,NWEDSEST ,NWEDSATR,NWEDSTVER,NWEDSTHOR,NWEDSTCO FROM METLNWE WITH(NOLOCK) WHERE NWEINNOD=" & nroNodeDesti & " UNION SELECT NWEDSPLA,CAST (NWEDSEST AS VARCHAR(8000)) AS NWEDSEST ,'GAIA2' as NWEDSATR,'0' as NWEDSTVER,'0' as NWEDSTHOR,NWEDSTCO FROM METLNWE2 WITH(NOLOCK) WHERE NWEINNOD=" & nroNodeDesti, DS)
                    'gaia.debug(nothing, "SELECT NWEDSPLA,NWEDSEST ,NWEDSATR,NWEDSTVER,NWEDSTHOR,NWEDSTCO FROM METLNWE WITH(NOLOCK) WHERE NWEINNOD=" & nroNodeDesti & " UNION SELECT NWEDSPLA,CAST (NWEDSEST AS VARCHAR(8000)) AS NWEDSEST ,'GAIA2' as NWEDSATR,'0' as NWEDSTVER,'0' as NWEDSTHOR,NWEDSTCO FROM METLNWE2 WITH(NOLOCK) WHERE NWEINNOD=" & nroNodeDesti)
                    If DS.Tables(0).Rows.Count > 0 Then

                        dbRow = DS.Tables(0).Rows(0)

                        llistaPlantilles.Text = dbRow("NWEDSPLA")
                        'gaia.debug(nothing, posicioEstructura)
                        If posicioEstructura < 0 Then
                            If dbRow("NWEDSATR") = "GAIA2" Then  'Teresa                    
                                lblEstructura.Text = maquetaEstructura(dbRow("NWEDSEST"), tipusNodeAMoure)
                            Else
                                Dim a As String()
                                a = Split(dbRow("NWEDSEST"), ",")
                                Array.Sort(a)
                                lblEstructura.Text = GAIA.pintaEstructura(objconn, a, Split(dbRow("NWEDSATR"), ","), Split(dbRow("NWEDSTVER"), ","), Split(dbRow("NWEDSTHOR"), ","), 1, "", 1, "", arrayDescripcions, arrayBuit, 0)
                            End If
                            lblCodi.Text = "<script>document.getElementById(""WEBDSTCO"").value=""" & dbRow("NWEDSTCO").ToString() & """;document.getElementById(""tipusNode"").value=""" & tipusNode.Text.ToString() & """;document.getElementById(""codiRelacioOrigen"").value=""" & codiRelacioOrigen.Text.ToString() & """;document.getElementById(""txtPosicioEstructuraReal"").value=""" & posicioEstructuraReal.ToString() & """;</script> "
                        Else 'Ya tiene una posición asignada
                            If dbRow("NWEDSATR") = "GAIA2" Then   'Teresa
                                lblEstructura.Text = maquetaEstructura(dbRow("NWEDSEST"), tipusNodeAMoure)
                            End If
                        End If
                    End If
                Case "arbre web"
                    lblTitol.Text = "Estructura de l'arbre web"
                    GAIA.bdr(objconn, "SELECT AWEDSEST,AWEDSATR,AWEDSTVER,AWEDSTHOR,AWEDSTCO FROM METLAWE WITH(NOLOCK) WHERE AWEINNOD=" & nroNodeDesti & " UNION SELECT  CAST(AWEDSHTM AS VARCHAR(8000)) AS AWEDSEST,'GAIA2' as AWEDSATR,'0' as AWEDSTVER,'0' as AWEDSTHOR,AWEDSTCO FROM METLAWE2 WITH(NOLOCK) WHERE AWEINNOD=" & nroNodeDesti, DS)
                    'gaia.debug(nothing,"SELECT AWEDSEST,AWEDSATR,AWEDSTVER,AWEDSTHOR,AWEDSTCO FROM METLAWE WITH(NOLOCK) WHERE AWEINNOD=" & nroNodeDesti & " UNION SELECT  CAST(AWEDSEST AS VARCHAR(8000)) AS AWEDSEST,'GAIA2' as AWEDSATR,'0' as AWEDSTVER,'0' as AWEDSTHOR,AWEDSTCO FROM METLAWE2 WITH(NOLOCK) WHERE AWEINNOD=" & nroNodeDesti )
                    If DS.Tables(0).Rows.Count > 0 Then
                        dbRow = DS.Tables(0).Rows(0)
                        If posicioEstructura < 0 Then
                            If dbRow("AWEDSATR") = "GAIA2" Then  'Teresa
                                ''                                    gaia.debug(nothing, lblEstructura.text)
                                lblEstructura.Text = maquetaEstructura(dbRow("AWEDSEST"), tipusNodeAMoure)
                                ''                                            gaia.debug(nothing, lblEstructura.text)
                            Else
                                Dim a As String()
                                a = Split(dbRow("AWEDSEST"), ",")

                                Array.Sort(a)
                                lblEstructura.Text = GAIA.pintaEstructura(objconn, a, Split(dbRow("AWEDSATR"), ","), Split(dbRow("AWEDSTVER"), ","), Split(dbRow("AWEDSTHOR"), ","), 1, "", 1, "", arrayDescripcions, arrayBuit, 0)
                            End If
                            lblCodi.Text = "<script>document.getElementById(""WEBDSTCO"").value=""" & dbRow("AWEDSTCO").ToString() & """;document.getElementById(""tipusNode"").value=""" & tipusNode.Text.ToString() & """;document.getElementById(""codiRelacioOrigen"").value=""" & codiRelacioOrigen.Text.ToString() & """;document.getElementById(""txtPosicioEstructuraReal"").value=""" & posicioEstructuraReal.ToString() & """;</script> "
                        Else 'Ya tiene una posición asignada
                            If dbRow("AWEDSATR") = "GAIA2" Then   'Teresa
                                lblEstructura.Text = maquetaEstructura(dbRow("AWEDSEST"), tipusNodeAMoure)
                            End If
                        End If
                    End If
                Case Else
                    'No es una fulla web i he de identificar la plantilla per defecte que tindrà el tipus de contingut.
                    Dim codiRelacio As Integer
                    'codiRelacio=GAIA.obtenirRelacioSuperior(objConn,valorCodiRelacioOrigen)
                    Dim codiPlantilla As String = GAIA.plantillaPerDefecte(objconn, relDesti, 1)
                    GAIA.bdr(objconn, "SELECT  CAST(PLTDSEST AS VARCHAR(8000)) AS PLTDSEST,CAST(PLTDSATR AS VARCHAR(8000)) AS PLTDSATR,CAST(PLTDSVER AS VARCHAR(8000)) AS  PLTDSVER,CAST(PLTDSHOR AS VARCHAR(8000)) AS PLTDSHOR,CAST(PLTDSOBS AS VARCHAR(8000)) AS PLTDSOBS, CAST(PLTDSTCO AS VARCHAR(8000)) AS PLTDSTCO FROM METLPLT WITH(NOLOCK) WHERE PLTINNOD=" & codiPlantilla & " UNION SELECT CAST(PLTDSEST AS VARCHAR(8000)) AS PLTDSEST,'GAIA2' as PLTDSATR,'0' as PLTDSVER,'0' as PLTDSHOR,CAST(PLTDSOBS AS VARCHAR(8000)) AS PLTDSOBS,  CAST(PLTDSTCO AS VARCHAR(8000)) AS PLTDSTCO  FROM METLPLT2 WITH(NOLOCK) WHERE PLTINNOD=" & codiPlantilla, DS)
                    If DS.Tables(0).Rows.Count > 0 Then
                        dbRow = DS.Tables(0).Rows(0)
                        If posicioEstructura < 0 Then
                            If dbRow("PLTDSATR") = "GAIA2" Then  'Teresa
                                lblEstructura.Text = maquetaEstructura(dbRow("PLTDSEST"), tipusNodeAMoure)
                            Else
                                Dim a As String()
                                a = Split(dbRow("PLTDSEST"), ",")
                                Array.Sort(a)
                                lblEstructura.Text = GAIA.pintaEstructura(objconn, a, Split(dbRow("PLTDSATR"), ","), Split(dbRow("PLTDSVER"), ","), Split(dbRow("PLTDSHOR"), ","), 1, "", 1, "", arrayDescripcions, arrayBuit, 0)
                            End If
                            lblTitol.Text = "Estructura de la plantilla." + dbRow("PLTDSOBS")
                            lblCodi.Text = "<script>document.getElementById(""WEBDSTCO"").value=""" & dbRow("PLTDSTCO").ToString() & """;document.getElementById(""tipusNode"").value=""" & tipusNode.Text.ToString() & """;document.getElementById(""codiRelacioOrigen"").value=""" & codiRelacioOrigen.Text.ToString() & """;document.getElementById(""txtPosicioEstructuraReal"").value=""" & posicioEstructuraReal.ToString() & """;</script> "
                        Else 'Ya tiene una posición asignada
                            If dbRow("PLTDSATR") = "GAIA2" Then   'Teresa
                                lblEstructura.Text = maquetaEstructura(dbRow("PLTDSEST"), tipusNodeAMoure)
                            End If
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

        If accio.SelectedItem.Value = "moure" Then 'copiar al mateix nivell del node seleccionat
            Dim strnodePathNou As String
            If nodePathNou.Text.Length > 0 Then
                strnodePathNou = nodePathNou.Text.Substring(0, InStrRev(nodePathNou.Text, "_") - 1)
            End If

            fesMoviment(txtDragDrop.Text, nroArbreOrigen.Text, nroNodeOrigen.Text, relOrigen, strnodePathNou, ultimaDivisio.Text, nroArbreDesti.Text, relDesti.inpar, GAIA.obtenirRelacioSuperior(objconn, relDesti), txtposicioEstructura.Text)

        Else 'Insertar dins del node seleccionat
            fesMoviment(txtDragDrop.Text, nroArbreOrigen.Text, nroNodeOrigen.Text, relOrigen, nodePathNou.Text, ultimaDivisio.Text, nroArbreDesti.Text, nroNodeDesti.Text, relDesti, txtposicioEstructura.Text)
        End If


    End Sub 'clickModificaEstructura

    Protected Sub clickCancelar(sender As Object, e As EventArgs)
        Dim relOrigen As New clsRelacio
        Dim relSuperior As New clsRelacio
        relOrigen.bdget(objconn, Request("codiRelacioOrigen"))
        relSuperior = GAIA.obtenirRelacioSuperior(objconn, relOrigen)

        If Request("direccio") = "1" Or Request("direccio") = ">" Then

            lblCodi.Text = "<script>window.close()</script>"
        Else
            lblCodi.Text = "<script>window.close()</script>"

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


        If InStr(llistaPlantilles.Text, ",") > 0 Then
            'la cel·la conté diverses plantilles, agafarem la que l'usuari hagi escollit a través
            'del desplegable	


            '		plantillaRelacio = CType(Page.FindControl("ddlb_plantilla" & ultimaDivisio), DropDownList).SelectedValue()
            plantillaRelacio = Request.Form("ddlb_plantilla" & ultimaDivisio)

        Else
            'la cel·la només té una plantilla possible.
            plantillaRelacio = llistaPlantilles.Text
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
            lblCodi.Text = "<script>window.close()</script>"
        Else 'actualitzo l'arbre 2 perque és una còpia entre arbres o bé un dragdrop de l'arbre "1" a l'arbre "2" o bé un dragdrop dins de l'arbre 2						
            lblCodi.Text = "<script>window.close()</script>"
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

    '************************************************************************************************************
    '	Funció: GAIA.assignaAutomaticamentCella
    '	Entrada: 
    '						tipusNodeAMoure: el tipus del node que volem decidir on l'assignem  	
    '						CodiRelacio: codi relacio on RELINFIL apunta a nroNodeDesti
    '						forcarCella: Si 1 llavors assigno a la primera cel·la disponible del tipus "tipusNodeAMoure"

    '					Si el tipusNodeAMoure correspon a cap o només a un tipus de contingut de la illa 
    '					corresponent a nroNodeDesti llavors no he de fer la crida, i assignaré directament el codi de cel·la
    '					Si no hi ha un lloc clar on ubicar-ho retorno -1. (per exemple perque hi ha + d'una possició possible)
    '					Si no trobo cap plantilla retorno -2
    '************************************************************************************************************
    Public Shared Function assignaAutomaticamentCella(ByVal objConn As OleDbConnection, ByVal tipusNodeAMoure As Integer, ByVal rel As clsRelacio, ByVal forcarCella As Integer, ByVal idioma As Integer, ByRef cellaAutolink As Integer, ByVal posicioEstructura As Integer) As Integer

        'Dim assignaAutomaticamentCella as integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim descTipus, item As String
        Dim nroNodeDesti As Integer
        Dim cont As Integer
        Dim trobats As Integer
        Dim plantillaAL As Integer
        Dim assignaAutomaticamentCellaAL As Integer
        Dim codiRelacio As Integer
        Dim relPare As clsRelacio
        Dim strTmp As String = ""
        codiRelacio = rel.incod
        nroNodeDesti = rel.infil
        descTipus = rel.tipdsdes

        DS = New DataSet()
        trobats = 0
        assignaAutomaticamentCella = -1
        assignaAutomaticamentCellaAL = -1

        If (rel.cdsit = 96 Or rel.cdsit = 97 Or rel.cdsit = 95) Then
            assignaAutomaticamentCella = -1
        Else

            Select Case descTipus.Trim
                Case "arbre web"
                    GAIA.bdr(objConn, "SELECT AWEDSTCO as tipusContingut,''  as plantillaAutoLink, AWEDSEST as estructura  FROM METLAWE2 WITH(NOLOCK)  WHERE AWEINNOD=" + nroNodeDesti.ToString() + " AND AWEINIDI=" + idioma.ToString(), DS)
                Case "node web"
                    GAIA.bdr(objConn, "SELECT NWEDSTCO as tipusContingut,''  as plantillaAutoLink, NWEDSEST as estructura FROM METLNWE2  WITH(NOLOCK) WHERE NWEINNOD=" + nroNodeDesti.ToString() + " AND NWEINIDI=" + idioma.ToString(), DS)

                Case "fulla web"
                    GAIA.bdr(objConn, "SELECT WEBDSTCO as tipusContingut, ''  as plantillaAutoLink, WEBDSEST as estructura FROM METLWEB2 WITH(NOLOCK)  WHERE WEBINNOD=" + nroNodeDesti.ToString() + " AND WEBINIDI=" + idioma.ToString(), DS)
                Case Else ' si el destí no és una fulla web serà un objecte amb una plantilla per defecte o bé donada per una pag web.			
                    GAIA.bdr(objConn, "SELECT PLTDSTCO AS tipusContingut, PLTCDPAL as plantillaAutoLink FROM METLPLT2  WITH(NOLOCK) WHERE PLTINNOD=" + GAIA.plantillaPerDefecte(objConn, rel, idioma).ToString(), DS)
            End Select

            If DS.Tables(0).Rows.Count > 0 Then
                dbRow = DS.Tables(0).Rows(0)
                'TRACTO AUTOLINK
                'pot ser que en una cel·la hi hagi més d'una referència igual a una plantilla d'autolink. Agafo la primera. No tractaré el cas de més d'una plantilla diferent!
                For Each strTmp In dbRow("plantillaAutolink").split(",")
                    If strTmp.Trim().Length > 0 Then
                        Exit For
                    End If
                Next

                If strTmp.Trim().Length > 0 Then

                    plantillaAL = strTmp
                    Dim codiRelacioPare As Integer
                    Dim descTipusNodeSup As String
                    'Busco Plantilla de l'Autolink (només serà un dels valors a plantillaAutolink que tindrà el format ",,,nroplantilla,,", on
                    '	les ',' son celles sense autolink. He d'eliminar els ',' i quedarme amb el valor (si hi ha cap).		
                    'si el pare és una fulla web fem:		
                    relPare = GAIA.obtenirRelacioSuperior(objConn, rel)
                    codiRelacioPare = relPare.incod
                    descTipusNodeSup = relPare.tipdsdes

                    If descTipusNodeSup = "fulla web" Then
                        assignaAutomaticamentCellaAL = GAIA.trobaCellaAutolink(objConn, tipusNodeAMoure, relPare, forcarCella, idioma)
                    Else
                        'Si el pare no es una fulla web hem de fer:
                        trobats = 0
                        cont = 0
                        Dim DS2 As DataSet
                        Dim dbRow2 As DataRow
                        DS2 = New DataSet()
                        GAIA.bdr(objConn, "SELECT PLTDSTCO AS tipusContingut, PLTCDPAL as plantillaAutoLink FROM METLPLT2  WITH(NOLOCK) WHERE PLTINNOD=" + plantillaAL.ToString(), DS2)
                        dbRow2 = DS2.Tables(0).Rows(0)
                        For Each item In Split(dbRow2("tipusContingut"), ",")
                            'Només retorno la cel·la automaticament si hi ha 1, si no trobo cap o + d'un llavors retorno -1				

                            'item=54 --> tots els tipus de continguts són permessos
                            If item = tipusNodeAMoure.ToString() Or item = "54" Then
                                trobats += 1
                                If assignaAutomaticamentCellaAL = -1 Then
                                    assignaAutomaticamentCellaAL = cont
                                End If
                            End If
                            cont += 1
                        Next item


                        If trobats = 0 Then
                            assignaAutomaticamentCellaAL = -2
                        Else
                            If trobats > 1 And forcarCella = 0 Then
                                assignaAutomaticamentCellaAL = -1
                            End If
                        End If
                        DS2.Dispose()

                    End If
                End If

                'TRACTO EL CAS NORMAL		
                trobats = 0
                cont = 0


                For Each item In Split(dbRow("tipusContingut"), ",")
                    'Només retorno la cel·la automaticament si hi ha 1, si no trobo cap o + d'un llavors retorno -1				
                    'item=54 --> tots els tipus de continguts són permessos
                    If item = tipusNodeAMoure.ToString() Or item = "54" Then
                        trobats += 1
                        If assignaAutomaticamentCella = -1 Then
                            assignaAutomaticamentCella = cont
                        End If
                    End If
                    cont += 1
                Next item


                If trobats = 0 Then

                    assignaAutomaticamentCella = -2
                Else
                    If trobats > 1 And forcarCella = 0 Then
                        assignaAutomaticamentCella = -1

                        ' Teresa
                    ElseIf trobats = 1 Then

                        ' Localizar el id de la celda en esa posicion
                        Dim estructura As String = dbRow("estructura")
                        Dim array As String() = Split(estructura, "id='d")
                        cont = 0
                        Dim i As Integer = 0
                        For Each item In array
                            If cont > 0 Then
                                If i = assignaAutomaticamentCella Then
                                    assignaAutomaticamentCella = item.Substring(0, 1)
                                End If
                                i += 1
                            End If
                            cont += 1
                        Next  ' Fin Teresa
                    End If
                End If
            Else
                assignaAutomaticamentCella = -2
            End If


            cellaAutolink = assignaAutomaticamentCellaAL

        End If
        DS.Dispose()
    End Function 'assignaAutomaticamentCella

    Function maquetaEstructura(ByVal webdshtm As String, ByVal tipusNodeAMoure As Integer) As String
        Dim html As String
        Dim divs As String()
        Dim celdas As String()
        Dim atributs As String()
        Dim cont As Integer = 0
        Dim posicion As Integer
        Dim identificador As Integer
        Dim posicioEstructuraReal As String

        posicioEstructuraReal = txtPosicioEstructuraReal.Text

        divs = Split(webdshtm, "id=")
        html = divs(0) & "id="

        celdas = Split(webdshtm, "<span class='atributs' style='display: none;'>")

        For Each celda In celdas
            'Saltamos el primero
            If cont > 0 Then
                atributs = celda.Split("#")
                identificador = atributs(0)
                posicion = divs(cont).IndexOf(">", 0)
                If atributs(7) = tipusNodeAMoure.ToString() Then
                    ubicacionsSeleccionables.Text = "1"
                    If (identificador.ToString() = posicioEstructuraReal) Then
                        divs(cont) = divs(cont).Substring(0, posicion) & "' style='background-color: #E6F9FB;' onclick='seleccionaDiv($(this));activaCamps(true);return false;'" & divs(cont).Substring(posicion) & "id="
                        ultimaDivisio.Text = divs(cont).Substring(0, posicion).Replace("'", "")
                        ultimaPlantilla.Text = llistaPlantilles.Text.Split(",")(cont - 1)

                    Else
                        divs(cont) = divs(cont).Substring(0, posicion) & "' style='background-color: #ffffff;' onclick='seleccionaDiv($(this));activaCamps(true);return false;'" & divs(cont).Substring(posicion) & "id="
                    End If
                Else
                    divs(cont) = divs(cont).Substring(0, posicion) & "' style='background-color: #eeeeee;' " & divs(cont).Substring(posicion) & "id="
                End If
                html += divs(cont)

            End If
            cont += 1
        Next

        html = html.Substring(0, html.Length - 3)
        Return html
    End Function

End Class