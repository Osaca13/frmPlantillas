Imports System.Data.OleDb

Public Class frmplantilla
    Inherits System.Web.UI.Page
    '**********************************************************************
    '**********************************************************************
    '			F R M P L A N T I L L A
    '**********************************************************************
    '**********************************************************************
    Public Shared nif As String = ""
    Public Shared objconn As OleDbConnection
    Public codiIdioma As Integer = 1
    Private Sub Page_UnLoad(sender As Object, e As System.EventArgs) Handles MyBase.Unload
        gaia2.bdFi(objconn)
    End Sub 'Page_UnLoad
    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load




        If HttpContext.Current.User.Identity.Name.Length > 0 Then

            If (Session("nif") Is Nothing) Then
                Session("nif") = GAIA2.nifUsuari(objconn, HttpContext.Current.User.Identity.Name).Trim()
            End If

            If Session("codiOrg") Is Nothing Then
                Session("CodiOrg") = GAIA2.trobaNodeUsuari(objconn, Session("nif")).tostring().Trim()
            End If


        End If
        nif = Session("nif").Trim()

        If Session("codiOrg") = 49730 Or Session("codiOrg") = 49727 Or Session("codiOrg") = 56935 Or Session("codiOrg") = 80879 Or Session("codiOrg") = 48729 Or Session("codiOrg") = 297650 Or Session("codiOrg") = 302362 Or Session("codiOrg") = 313486 Or Session("codiOrg") = 346231 Then
            objconn = gaia2.bdIni()
            If Request("idioma") = 2 Then codiIdioma = 2

            If Not Page.IsPostBack Then
                lblResultat.Text = "&nbsp"
                carregaTipusContinguts()
                carregallistaEstilsCSS()
                carregaDdls()
                carregaCamps()

                'Falta carregar llibreries i plantilles	
            End If
        End If
    End Sub 'Page_Load

    Protected Sub carregaDdls()
        'Carrego el listbox de "target"
        ddlb_PLTDSALF.Items.Insert(0, New ListItem("Mateixa finestra", 0))
        ddlb_PLTDSALF.Items.Insert(1, New ListItem("Nova finestra", 1))
        'Carrego els nivells de text d'una cel.la (h1, h2, h3..etc)
        ddlPLTDSNIV.Items.Insert(0, New ListItem("", 0))
        Dim cont As Integer
        For cont = 1 To 7
            ddlPLTDSNIV.Items.Insert(cont, New ListItem("H" & cont, cont))
        Next cont

    End Sub

    Protected Sub carregaCamps(Optional ByVal codiIdioma As Integer = 1)
        Dim ds As New DataSet, qSQL As String
        Dim dbRow As DataRow
        Dim arrayTmp As String()

        nroId.value = 0

        Select Case Request("tipus")
            Case "A"
                ltTitol.text = "Arbre web"
                pnlArbreWeb.visible = True
                pnlcanviIdioma.Visible = True
                pnlNodeWeb.Visible = False
                pnlFullaWeb.visible = False
                pnlPlantilla.visible = False
                pnlWebCamps1.visible = False
                pnlPltCamps1.visible = False
                pnlPltCamps2.visible = False
                pnlPltCamps3.visible = False
                pnlLCW2.Visible = True
                pnlPlt.visible = False
                pnlEstils.visible = False
                inicialitzaLListaServidors()
            Case "N"
                ltTitol.text = "Node web"
                pnlArbreWeb.visible = False
                pnlcanviIdioma.Visible = True
                pnlNodeWeb.Visible = True
                pnlFullaWeb.visible = False
                pnlPlantilla.visible = False
                pnlWebCamps1.visible = False
                pnlPltCamps1.visible = False
                pnlPltCamps2.visible = False
                pnlPltCamps3.visible = False
                pnlLCW2.Visible = False
                pnlPlt.visible = True
                pnlEstils.visible = True
            Case "W"
                ltTitol.text = "Fulla web"
                pnlArbreWeb.visible = False
                pnlcanviIdioma.Visible = True
                pnlNodeWeb.Visible = False
                pnlFullaWeb.visible = True
                pnlPlantilla.visible = False
                pnlWebCamps1.visible = True
                pnlPltCamps1.visible = False
                pnlPltCamps2.visible = False
                pnlPltCamps3.visible = False
                pnlLCW2.Visible = True
                pnlPlt.visible = True
                pnlEstils.visible = True
            Case Else
                ltTitol.text = "Plantilla web"
                pnlArbreWeb.visible = False
                pnlcanviIdioma.Visible = False
                pnlNodeWeb.Visible = False
                pnlPlantilla.visible = True
                pnlFullaWeb.visible = False
                pnlWebCamps1.visible = False
                pnlPltCamps1.visible = True
                pnlPltCamps2.visible = True
                pnlPltCamps3.visible = True
                pnlLCW2.Visible = True
                pnlPlt.visible = True
                pnlEstils.visible = True
                ltCanviCampsDb.text = "if ($('#lstTipusFulla').val()!= null) {canviCampsDB($('#lstTipusFulla').val()) ;}"
        End Select


        If Request("id") = "" Then
            ltEst.text = "<div class=""contenidor border border-secondary p-2 pr-4 pl-4""><span class=""contenidorAtributs"" style=""display:none"">###########################|</span><div class=""row border border-secondary p-2""><span class=""rowAtributs"" style=""display:none"">###########################|</span><div class=""col cel border border-secondary p-2"" id=""d0""><span class=""divId"" style=""display:none"">0</span><span class=""divImg""></span><span class=""text"">Cel&middot;la inicial</span><span class=""atributs"" style=""display:none"">0#Cel&middot;la inicial##########################|</span></div></div></div> "
            txtEst.Value = ltEst.Text
        Else
            Select Case Request("tipus")
                Case "W" 'fulla web
                    qSQL = "select * FROM METLWEB2 WITH(NOLOCK) WHERE WEBINNOD=" & Request("id") & " AND WEBINIDI=" & codiIdioma
                    gaia2.bdR(objconn, qSQL, ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        dbRow = ds.Tables(0).Rows(0)
                        txtEst.value = dbRow("WEBDSHTM").replace("''", "'")

                        ltEst.text = txtEst.value

                        'busco el nroId més gran			
                        Dim cont As Integer = 0
                        Dim idTmp As Integer = 0
                        arrayTmp = Regex.Split(dbRow("WEBDSHTM"), "id='d")
                        For Each item As String In arrayTmp
                            If cont > 0 Then
                                idTmp = item.Substring(0, InStr(item, "'") - 1)
                                If idTmp >= nroId.value Then nroId.value = idTmp
                            End If
                            cont += 1
                        Next item

                        WEBDSTIT.text = dbRow("WEBDSTIT")
                        WEBDSDES.text = dbRow("WEBDSDES")
                        WEBDSPCL.text = dbRow("WEBDSPCL")

                        WEBTPBUS.checked = IIf(dbRow("WEBTPBUS") = "S", True, False)
                        WEBDTPUB.text = dbRow("WEBDTPUB")
                        WEBDTCAD.text = dbRow("WEBDTCAD")
                        WEBDSFIT.text = dbRow("WEBDSFIT")

                        WEBDSURL.text = dbRow("WEBDSURL")
                        WEBTPHER.checked = IIf(dbRow("WEBTPHER") = "S", True, False)

                        WEBSWFRM.checked = IIf(dbRow("WEBSWFRM") = "S", True, False)
                        WEBSWEML.checked = IIf(dbRow("WEBSWEML") = "S", True, False)
                        WEBSWSSL.checked = IIf(dbRow("WEBSWSSL") = "S", True, False)

                        WEBDSEBO.text = dbRow("WEBDSEBO")
                        WEBWNMTH.text = dbRow("WEBWNMTH")

                    Else 'No he trobat l'idioma demanat i busco un altre
                        qSQL = "select * FROM METLWEB2 WITH(NOLOCK) WHERE WEBINNOD=" & Request("id") & " ORDER BY WEBINIDI ASC "
                        gaia2.bdr(objconn, qSQL, ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            carregaCamps(ds.Tables(0).Rows(0)("WEBINIDI"))
                        End If
                    End If

                Case "N" 'node web
                    qSQL = "select * FROM METLNWE2 WITH(NOLOCK) WHERE NWEINNOD=" & Request("id") & " AND NWEINIDI=" & codiIdioma
                    gaia2.bdR(objconn, qSQL, ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        dbRow = ds.Tables(0).Rows(0)
                        txtEst.value = dbRow("NWEDSHTM")
                        ltEst.text = dbRow("NWEDSHTM")

                        'busco el nroId més gran			
                        Dim cont As Integer = 0
                        Dim idTmp As Integer = 0
                        arrayTmp = Regex.Split(dbRow("NWEDSHTM"), "id='d")
                        For Each item As String In arrayTmp
                            If cont > 0 Then
                                idTmp = item.Substring(0, InStr(item, "'") - 1)
                                If idTmp >= nroId.value Then nroId.value = idTmp
                            End If
                            cont += 1
                        Next item
                        NWEDSTIT.text = dbRow("NWEDSTIT")
                        NWEDSCAR.text = dbRow("NWEDSCAR")
                        NWEDSEBO.text = dbRow("NWEDSEBO")
                        NWEDSMET.text = dbRow("NWEDSMET")
                        NWEDSPEU.text = dbRow("NWEDSPEU")
                        NWEDSCSP.text = dbRow("NWEDSCSP")
                        NWEDSCSI.text = dbRow("NWEDSCSI")

                    Else 'No he trobat l'idioma demanat i busco un altre
                        qSQL = "select * FROM METLNWE2 WITH(NOLOCK) WHERE NWEINNOD=" & Request("id") & " ORDER BY NWEINIDI ASC "
                        GAIA2.bdr(objconn, qSQL, ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            carregaCamps(ds.Tables(0).Rows(0)("NWEINIDI"))
                        End If
                    End If

                Case "A" 'arbre web

                    qSQL = "select * FROM METLAWE2 WITH(NOLOCK) WHERE AWEINNOD=" & Request("id") & " AND AWEINIDI=" & codiIdioma
                    gaia2.bdR(objconn, qSQL, ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        dbRow = ds.Tables(0).Rows(0)
                        txtEst.value = dbRow("AWEDSHTM")
                        ltEst.text = dbRow("AWEDSHTM")

                        'busco el nroId més gran			
                        Dim cont As Integer = 0
                        Dim idTmp As Integer = 0
                        arrayTmp = Regex.Split(dbRow("AWEDSHTM"), "id='d")
                        For Each item As String In arrayTmp
                            If cont > 0 Then
                                idTmp = item.Substring(0, InStr(item, "'") - 1)
                                If idTmp >= nroId.value Then nroId.value = idTmp
                            End If
                            cont += 1
                        Next item
                        AWEDSTIT.text = dbRow("AWEDSTIT")
                        AWEDSNOM.text = dbRow("AWEDSNOM")
                        AWEDSROT.text = dbRow("AWEDSROT")
                        AWEDSDOC.text = dbRow("AWEDSDOC")

                        AWEDSEBO.text = dbRow("AWEDSEBO")
                        AWEDSMET.text = dbRow("AWEDSMET")
                        AWEDSPEU.text = dbRow("AWEDSPEU")
                        AWEDSCSP.text = dbRow("AWEDSCSP")
                        AWEDSCSI.text = dbRow("AWEDSCSI")
                        lstAWEDSSER.Items.FindByValue(dbRow("AWEDSSER")).Selected = True

                    Else 'No he trobat l'idioma demanat i busco un altre
                        qSQL = "select * FROM METLAWE2 WITH(NOLOCK) WHERE AWEINNOD=" & Request("id") & " ORDER BY AWEINIDI ASC "
                        GAIA2.bdr(objconn, qSQL, ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            carregaCamps(ds.Tables(0).Rows(0)("AWEINIDI"))
                        End If
                    End If

                Case Else   'plantilla
                    qSQL = "select * FROM METLPLT2 WITH(NOLOCK) WHERE PLTINNOD=" & Request("id")
                    gaia2.bdR(objconn, qSQL, ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        dbRow = ds.Tables(0).Rows(0)
                        txtEst.value = dbRow("PLTDSHTM")
                        ltEst.text = dbRow("PLTDSHTM")

                        'busco el nroId més gran			
                        Dim cont As Integer = 0
                        Dim idTmp As Integer = 0
                        arrayTmp = Regex.Split(dbRow("PLTDSHTM"), "id='d")
                        For Each item As String In arrayTmp
                            If cont > 0 Then
                                idTmp = item.Substring(0, InStr(item, "'") - 1)
                                If idTmp >= nroId.value Then nroId.value = idTmp
                            End If
                            cont += 1
                        Next item

                        txtPLTDSTIT.text = dbRow("PLTDSTIT")
                        txtPLTDSOBS.text = dbRow("PLTDSOBS")
                        chkPLTSWVIS.checked = dbRow("PLTSWVIS")
                    End If
            End Select
        End If

    End Sub


    Protected Sub carregallistaEstilsCSS()
        Dim ds As New DataSet, qSQL As String, dv As DataView
        Dim element As ListItem
        Dim item As DataRow
        qSQL = "select isnull(CSSDSCSS,'1') as CSSDSCSS,CSSINTIP,CSSINCOD,CSSDSTXT from METLCSS WITH(NOLOCK) ORDER BY CSSDSTXT"
        gaia2.bdR(objconn, qSQL, ds)
        'colors

        dv = ds.Tables(0).DefaultView


        For Each item In dv.Table.Rows
            If item("CSSINTIP") = 23 Then
                element = New ListItem(item("CSSDSTXT"), item("CSSINCOD"))
                If item("CSSDSCSS") <> "1" And item("CSSDSTXT") <> "blanc" Then
                    element.Attributes.Add("style", item("CSSDSCSS"))
                End If
                ddlb_23.Items.add(element)
            End If
        Next item
        'ddlb_23.Items.Insert(0, new ListItem("",""))
        ddlb_23.items.insert(0, New ListItem("", ""))
        ddlb_23.items(0).selected = True


        dv.RowFilter = "CSSINTIP=25"
        ddlb_25.datasource = dv
        ddlb_25.datatextfield = "CSSDSTXT"
        ddlb_25.datavaluefield = "CSSINCOD"
        ddlb_25.databind()
        ddlb_25.Items.Insert(0, New ListItem("", 0))


        'font
        dv.RowFilter = "CSSINTIP=26"
        ddlb_26.datasource = dv
        ddlb_26.datatextfield = "CSSDSTXT"
        ddlb_26.datavaluefield = "CSSINCOD"
        ddlb_26.databind()
        ddlb_26.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=27"
        ddlb_27.datasource = dv
        ddlb_27.datatextfield = "CSSDSTXT"
        ddlb_27.datavaluefield = "CSSINCOD"
        ddlb_27.databind()
        ddlb_27.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=28"
        ddlb_28.datasource = dv
        ddlb_28.datatextfield = "CSSDSTXT"
        ddlb_28.datavaluefield = "CSSINCOD"
        ddlb_28.databind()
        ddlb_28.Items.Insert(0, New ListItem("", 0))


        dv.RowFilter = "CSSINTIP=103"
        ddlb_103.datasource = dv
        ddlb_103.datatextfield = "CSSDSTXT"
        ddlb_103.datavaluefield = "CSSINCOD"
        ddlb_103.databind()
        ddlb_103.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=105"
        ddlb_105.datasource = dv
        ddlb_105.datatextfield = "CSSDSTXT"
        ddlb_105.datavaluefield = "CSSINCOD"
        ddlb_105.databind()
        ddlb_105.Items.Insert(0, New ListItem("", 0))





        dv.RowFilter = "CSSINTIP=108"
        ddlb_108.datasource = dv
        ddlb_108.datatextfield = "CSSDSTXT"
        ddlb_108.datavaluefield = "CSSINCOD"
        ddlb_108.databind()
        ddlb_108.Items.Insert(0, New ListItem("", 0))

        'dv.RowFilter="CSSINTIP=109"
        'ddlb_109.datasource=dv
        'ddlb_109.datatextfield="CSSDSTXT"
        'ddlb_109.datavaluefield="CSSINCOD"
        'ddlb_109.databind()
        'ddlb_109.Items.Insert(0, new ListItem("",0))

        dv.RowFilter = "CSSINTIP=110"
        ddlb_110.datasource = dv
        ddlb_110.datatextfield = "CSSDSTXT"
        ddlb_110.datavaluefield = "CSSINCOD"
        ddlb_110.databind()
        ddlb_110.Items.Insert(0, New ListItem("", 0))
        dv.RowFilter = "CSSINTIP=111"
        ddlb_111.datasource = dv
        ddlb_111.datatextfield = "CSSDSTXT"
        ddlb_111.datavaluefield = "CSSINCOD"
        ddlb_111.databind()
        ddlb_111.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=112"
        ddlb_112.datasource = dv
        ddlb_112.datatextfield = "CSSDSTXT"
        ddlb_112.datavaluefield = "CSSINCOD"
        ddlb_112.databind()
        ddlb_112.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=122"
        ddlb_122.datasource = dv
        ddlb_122.datatextfield = "CSSDSTXT"
        ddlb_122.datavaluefield = "CSSINCOD"
        ddlb_122.databind()
        ddlb_122.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=114"
        ddlb_114.datasource = dv
        ddlb_114.datatextfield = "CSSDSTXT"
        ddlb_114.datavaluefield = "CSSINCOD"
        ddlb_114.databind()
        ddlb_114.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=115"
        ddlb_115.datasource = dv
        ddlb_115.datatextfield = "CSSDSTXT"
        ddlb_115.datavaluefield = "CSSINCOD"
        ddlb_115.databind()
        ddlb_115.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=123"
        ddlb_123.datasource = dv
        ddlb_123.datatextfield = "CSSDSTXT"
        ddlb_123.datavaluefield = "CSSINCOD"
        ddlb_123.databind()
        ddlb_123.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=647"  'Nuevo grupo de estilos: G2-Estils definits
        ddlb_117.datasource = dv
        ddlb_117.datatextfield = "CSSDSTXT"
        ddlb_117.datavaluefield = "CSSINCOD"
        ddlb_117.databind()
        ddlb_117.Items.Insert(0, New ListItem("", 0))



        'fons

        ddlb_118.Items.Insert(0, New ListItem("", 0))
        For Each item In dv.Table.Rows
            If item("CSSINTIP") = 118 Then
                element = New ListItem(item("CSSDSTXT"), item("CSSINCOD"))
                If item("CSSDSCSS") <> "1" And InStr(item("CSSDSCSS"), "000000") = 0 Then
                    'element.Attributes.Add("style",item("CSSDSCSS"))
                End If
                ddlb_118.Items.add(element)
            End If
        Next item



        dv.RowFilter = "CSSINTIP=119"
        ddlb_119.datasource = dv
        ddlb_119.datatextfield = "CSSDSTXT"
        ddlb_119.datavaluefield = "CSSINCOD"
        ddlb_119.databind()
        ddlb_119.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=124"
        ddlb_124.datasource = dv
        ddlb_124.datatextfield = "CSSDSTXT"
        ddlb_124.datavaluefield = "CSSINCOD"
        ddlb_124.databind()
        ddlb_124.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=630"
        ddlb_630.datasource = dv
        ddlb_630.datatextfield = "CSSDSTXT"
        ddlb_630.datavaluefield = "CSSINCOD"
        ddlb_630.databind()
        ddlb_630.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=631"
        ddlb_631.datasource = dv
        ddlb_631.datatextfield = "CSSDSTXT"
        ddlb_631.datavaluefield = "CSSINCOD"
        ddlb_631.databind()
        ddlb_631.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=632"
        ddlb_632.datasource = dv
        ddlb_632.datatextfield = "CSSDSTXT"
        ddlb_632.datavaluefield = "CSSINCOD"
        ddlb_632.databind()
        ddlb_632.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=633"
        ddlb_633.datasource = dv
        ddlb_633.datatextfield = "CSSDSTXT"
        ddlb_633.datavaluefield = "CSSINCOD"
        ddlb_633.databind()
        ddlb_633.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=634"
        ddlb_634.datasource = dv
        ddlb_634.datatextfield = "CSSDSTXT"
        ddlb_634.datavaluefield = "CSSINCOD"
        ddlb_634.databind()
        ddlb_634.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=635"
        ddlb_635.datasource = dv
        ddlb_635.datatextfield = "CSSDSTXT"
        ddlb_635.datavaluefield = "CSSINCOD"
        ddlb_635.databind()
        ddlb_635.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=636"
        ddlb_636.datasource = dv
        ddlb_636.datatextfield = "CSSDSTXT"
        ddlb_636.datavaluefield = "CSSINCOD"
        ddlb_636.databind()
        ddlb_636.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=637"
        ddlb_637.datasource = dv
        ddlb_637.datatextfield = "CSSDSTXT"
        ddlb_637.datavaluefield = "CSSINCOD"
        ddlb_637.databind()
        ddlb_637.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=638"
        ddlb_638.datasource = dv
        ddlb_638.datatextfield = "CSSDSTXT"
        ddlb_638.datavaluefield = "CSSINCOD"
        ddlb_638.databind()
        ddlb_638.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=639"
        ddlb_639.datasource = dv
        ddlb_639.datatextfield = "CSSDSTXT"
        ddlb_639.datavaluefield = "CSSINCOD"
        ddlb_639.databind()
        ddlb_639.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=641"
        ddlb_641.datasource = dv
        ddlb_641.datatextfield = "CSSDSTXT"
        ddlb_641.datavaluefield = "CSSINCOD"
        ddlb_641.databind()
        ddlb_641.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=642"
        ddlb_642.datasource = dv
        ddlb_642.datatextfield = "CSSDSTXT"
        ddlb_642.datavaluefield = "CSSINCOD"
        ddlb_642.databind()
        ddlb_642.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=648"
        ddlb_648.DataSource = dv
        ddlb_648.DataTextField = "CSSDSTXT"
        ddlb_648.DataValueField = "CSSINCOD"
        ddlb_648.DataBind()
        ddlb_648.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=649"
        ddlb_649.DataSource = dv
        ddlb_649.DataTextField = "CSSDSTXT"
        ddlb_649.DataValueField = "CSSINCOD"
        ddlb_649.DataBind()
        ddlb_649.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=650"
        ddlb_650.DataSource = dv
        ddlb_650.DataTextField = "CSSDSTXT"
        ddlb_650.DataValueField = "CSSINCOD"
        ddlb_650.DataBind()
        ddlb_650.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=651"
        ddlb_651.DataSource = dv
        ddlb_651.DataTextField = "CSSDSTXT"
        ddlb_651.DataValueField = "CSSINCOD"
        ddlb_651.DataBind()
        ddlb_651.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=652"
        ddlb_652.DataSource = dv
        ddlb_652.DataTextField = "CSSDSTXT"
        ddlb_652.DataValueField = "CSSINCOD"
        ddlb_652.DataBind()
        ddlb_652.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=653"
        ddlb_653.DataSource = dv
        ddlb_653.DataTextField = "CSSDSTXT"
        ddlb_653.DataValueField = "CSSINCOD"
        ddlb_653.DataBind()
        ddlb_653.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=654"
        ddlb_654.DataSource = dv
        ddlb_654.DataTextField = "CSSDSTXT"
        ddlb_654.DataValueField = "CSSINCOD"
        ddlb_654.DataBind()
        ddlb_654.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=655"
        ddlb_655.DataSource = dv
        ddlb_655.DataTextField = "CSSDSTXT"
        ddlb_655.DataValueField = "CSSINCOD"
        ddlb_655.DataBind()
        ddlb_655.Items.Insert(0, New ListItem("", 0))

        ds.Dispose()
    End Sub 'carregallistaEstilsCSS


    Protected Sub carregaTipusContinguts()
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()

        gaia2.bdR(objconn, "SELECT TIPINTIP,TIPDSDES,TBLDSTXT FROM  METLTIP WITH(NOLOCK), METLTBL WITH(NOLOCK) WHERE TIPINTIP=TBLINTFU", DS)


        If Request("tipus") = "P" Or Request("tipus") = "" Then
            lblCodi.Text = "<script language=""javascript"">"

            lblCodi.Text += "var longitud = document.getElementById('lstTipusFulla').options.length;"

            lblTipusFulla.Text = "<select name=""lstTipusFulla"" id=""lstTipusFulla"" onChange='canviCampsDB(this[this.selectedIndex].value);'><option value="" ""></option>"
            For Each dbRow In DS.Tables(0).Rows
                lblTipusFulla.Text += "<option value=" + dbRow("TIPINTIP").ToString() + ">" + dbRow("TIPDSDES").Trim() + "</option>"
            Next dbRow
            lblTipusFulla.Text += "</select>"

            lblCodi.Text += "var taules = new Array(longitud);" & Chr(13) + Chr(10)
            lblCodi.Text += "var camps = new Array(longitud);" & Chr(13) + Chr(10)
            lblCodi.Text += "taules[0] = "" "";" & Chr(13) + Chr(10)

            lblCodi.Text += "for (i=0; i<longitud; i++) " & Chr(13) + Chr(10)
            lblCodi.Text += "camps[i]=new Array(); " & Chr(13) + Chr(10)
            lblCodi.Text += "camps[0][0]=new Option("" "","" "");" & Chr(13) + Chr(10)
            Dim x As Integer = 1
            For Each dbRow In DS.Tables(0).Rows
                lblCodi.Text += "taules[" & x.ToString() & "]=""" & dbRow("TIPINTIP") & """;" & Chr(13) + Chr(10)
                lblCodi.Text += carregaNomCamps(x, dbRow("TBLDSTXT")) & Chr(13) + Chr(10)
                x = x + 1
            Next dbRow

            lblCodi.Text += "function canviCampsDB(valor) { var temp=document.getElementById('ddlPLTDSCMP'); var temp2=document.getElementById('ddlPLTDSLNK'); var temp3=document.getElementById('ddlPLTDSALT');temp.options[0]=new Option("" "","" "");temp2.options[0]=new Option("" "","" "");temp3.options[0]=new Option("" "","" "");for (x=0;x<taules.length;x++) {		if (taules[x]==valor) {			break;	}	}	for (m=temp.options.length-1;m>0;m--) {		temp.options[m]=null;temp2.options[m]=null;temp3.options[m]=null;		} 	for (i=1;i<camps[x].length;i++) {		temp.options[i]=new Option(camps[x][i].text,camps[x][i].value);temp2.options[i]=new Option(camps[x][i].text,camps[x][i].value);temp3.options[i]=new Option(camps[x][i].text,camps[x][i].value);	} temp.options[0].selected=true;temp2.options[0].selected=true;temp3.options[0].selected=true;}"
            lblCodi.Text += "</script>"
        Else
            lblTipusFulla.Text = "<select name=""lstTipusFulla"" id=""lstTipusFulla"" ><option value="" ""></option>"
            For Each dbRow In DS.Tables(0).Rows
                lblTipusFulla.Text += "<option value=" + dbRow("TIPINTIP").ToString() + ">" + dbRow("TIPDSDES").Trim() + "</option>"
            Next dbRow
            lblTipusFulla.Text += "</select>"
        End If

        DS.Dispose()
    End Sub 'carregaTipusCodis


    Protected Function carregaNomCamps(ByVal index As String, ByVal taula As String) As String
        Dim DS As DataSet
        Dim index2 As Integer

        DS = New DataSet()
        Dim strCamps As String
        strCamps = ""
        index2 = 0

        gaia2.bdR(objconn, "SELECT TOP 1 * FROM " & taula, DS)
        For index2 = 0 To DS.Tables(0).Columns.Count - 1
            strCamps += "camps[" + index.ToString() + "][" + index2.ToString() + "]= new Option(""" + DS.Tables(0).Columns(index2).ColumnName.Trim() + """,""" + taula.Trim() + "." + DS.Tables(0).Columns(index2).ColumnName.Trim() + """);"
        Next index2


        strCamps += "camps[" + index.ToString() + "][" + (index2).ToString() + "]= new Option(""Auto-Enllaç"",""AUTO-ENLLAÇ"");"
        If taula.Trim() = "METLINF" Or taula.Trim() = "METLNOT" Or taula.Trim() = "METLCON" Or taula.Trim() = "METLAGD" Or taula.Trim() = "METLLNK" Or taula.Trim() = "METLDIR" Or taula.Trim() = "METLFPR" Then
            index2 += 1
            strCamps += "camps[" + index.ToString() + "][" + (index2).ToString() + "]= new Option(""Imatge"",""IMATGE"");"
        End If
        If taula.Trim() = "METLINF" Or taula.Trim() = "METLNOT" Or taula.Trim() = "METLCON" Or taula.Trim() = "METLAGD" Or taula.Trim() = "METLLNK" Or taula.Trim() = "METLDIR" Or taula.Trim() = "METLFPR" Then
            index2 += 1
            strCamps += "camps[" + index.ToString() + "][" + (index2).ToString() + "]= new Option(""Document"",""DOCUMENT"");"
            '+"]= new Option(""Auto-Enllaç"",""METLREL.RELDSFIT"");"
        End If
        carregaNomCamps = strCamps
        DS.Dispose()

    End Function 'CarregaNomCamps

    Protected Sub btnGuardar_ServerClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim dataCaducitat As String
        Dim buscar As String
        Dim heretar As String
        Dim esForm, esEML, esSSL As String
        Dim strSql As String = ""

        'en txtEst.text tinc l'estructura neta per gravar.
        ''	response.write(txtEst.value)
        'Preparo l'insert de la plantilla
        'txtatributs.value té una estructura tipus 
        ' id cel(0)#nom cel·la(1)#mida xa[2]#mida sm[3]#mida md[4]#mida lg[5]#mida xl[6]#tipuscontingut[7]#camp[8]#enllaç[9]#Text Alt[10]#color[11]|
        '|0#Cel·la inicial########||1#cel########|

        '***************************************************************************************
        ' Faig el tractament dels camps comuns'
        '***************************************************************************************		
        Dim item() As String
        Dim arrCel() As String
        Dim PLTDSCMP As String = "", PLTDSLNK As String = "", PLTDSALT As String = "", PLTDSEST As String = "", PLTDSCSS As String = "", PLTDSTCO As String = "", PLTDSLCW As String = "", PLTDSLC2 As String = "", PLTDSIMG As String = "", PLTCDPAL As String = "", PLTDSAAL As String = "", PLTDSPLT As String = "", PLTDSALK As String = "", PLTDSFLW As String = "", PLTSWALT As Integer = 1, PLTDSNUM As String = "", PLTDSALF As String = "", PLTDSNIV As String = "", PLTSWVIS As Integer = 0, WEBDSIMP As String = "", WEBDSCND As String = "", PLTDSHTM As String = ""
        codiIdioma = lstCanviIdioma.SelectedValue

        txtEstBD.value = txtEstBD.value.replace("""", "'").replace("''", "'").replace("'", "''")
        PLTDSHTM = txtEstBD.value.replace("celActiva", "").Replace("rowActiva", "").Replace("contenidorActiu", "")

        PLTDSEST = txtEst.value.replace("""", "'").replace("''", "'").replace("'", "''")

        '					gaia2.debug(nothing, "despres=" & PLTDSEST)
        Dim cssTmp As String = ""
        Dim cont As Integer = 0
        arrCel = txtAtributs.value.split("|")
        For Each cel As String In arrCel
            item = cel.Split("#")

            If item.Length > 1 Then
                If cont > 0 Then
                    PLTDSTCO &= ","
                    PLTDSCMP &= ","
                    PLTDSLNK &= ","
                    PLTDSALT &= "|"
                    PLTDSIMG &= ","
                    PLTDSALK &= "|"
                    PLTDSAAL &= "|"
                    PLTCDPAL &= ","
                    PLTDSFLW &= ","
                    PLTDSNUM &= ","
                    PLTDSALF &= ","
                    PLTDSNIV &= ","
                    PLTDSLCW &= ","
                    PLTDSLC2 &= ","
                    PLTDSPLT &= ","
                    PLTDSCSS &= "|"
                    WEBDSIMP &= ","
                    WEBDSCND &= ","
                End If

                PLTDSTCO &= item(7)
                PLTDSCMP &= item(8)
                PLTDSLNK &= item(9)
                PLTDSALT &= item(10)
                PLTDSIMG &= IIf(item(11) = "", 0, item(11))

                PLTDSFLW &= "0"



                PLTDSLCW &= item(13).Replace(",", "|")
                PLTDSLC2 &= item(15).Replace(",", "|")
                PLTDSPLT &= item(17).Replace(",", "|")
                PLTDSNUM &= IIf(item(18) = "", 0, item(18))
                PLTDSNIV &= item(19)

                PLTDSAAL &= item(20)
                PLTDSALF &= item(21)
                PLTDSALK &= item(22)
                PLTCDPAL &= item(24)



                'Camps d'atributs de cel·les de web
                WEBDSIMP &= item(25)
                WEBDSCND &= item(26)

                'Estils
                cssTmp = ""
                For i As Integer = 27 To 27
                    If cssTmp.Length > 0 Then cssTmp &= ","
                    If item(i).Length > 0 Then cssTmp &= item(i)

                Next i
                PLTDSCSS &= cssTmp

            End If
            cont = cont + 1
        Next cel

        Select Case Request("tipus")
            Case "W"
                If WEBDTCAD.Text = "" Then dataCaducitat = "01/01/1900" Else dataCaducitat = WEBDTCAD.Text
                If WEBTPBUS.checked Then buscar = "S" Else buscar = "N"
                If WEBTPHER.checked Then heretar = "S" Else heretar = "N"
                If WEBSWFRM.checked Then esForm = "S" Else esForm = "N"
                If WEBSWEML.checked Then esEML = "S" Else esEML = "N"
                If WEBSWSSL.checked Then esSSL = "S" Else esSSL = "N"


                If Not Request("id") Is Nothing Then
                    txtcodiNode.text = Request("id").ToString()
                Else
                    'Inserto el node fulla web
                    txtcodiNode.text = GAIA2.insertarNode(objconn, 10, WEBDSTIT.text.replace("<p>", "").replace("</p>", ""), Session("codiOrg"))

                    'Inserto el node fulla web en l'arbre personal de l'usuari		
                    GAIA2.insertaNodeArbrePersonal(objconn, 10, txtcodiNode.text, Session("codiOrg"), "")
                End If


                'per fer proves
                'txtCodiNode.text = 272518

                strSql &= "DELETE FROM METLWEB2 WHERE WEBINNOD=" & txtcodiNode.text & " AND WEBINIDI=" & codiIdioma
                strSql &= ";INSERT INTO METLWEB2 (WEBINNOD, WEBINIDI,WEBDSTIT,WEBDSFIT, WEBDSURL, WEBDTPUB, WEBDTCAD, WEBDTANY, WEBDSUSR,WEBTPBUS,WEBDSTCO, WEBDSPLA, WEBDSEST, WEBDSHTM,  WEBDSLCW, WEBDSLC2, WEBDSCSS, WEBTPHER, WEBDSIMP,WEBDSCND, WEBWNMTH,WEBSWFRM, WEBSWEML,WEBDSEBO, WEBDSDES, WEBDSPCL, WEBSWSSL) VALUES (" & txtcodiNode.text & "," & codiIdioma & ",'" & WEBDSTIT.text.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "','" & WEBDSFIT.text.tostring().Replace("'", "''") & "','" & WEBDSURL.text.tostring().Replace("'", "''") & "','" & WEBDTPUB.Text.Tostring() & "','" & dataCaducitat & "',getDate(),'" & Session("codiOrg") & "','" & buscar.ToString() & "','" & PLTDSTCO & "','" & PLTDSPLT & "','" & PLTDSEST & "','" & PLTDSHTM & "','" & PLTDSLCW & "','" & PLTDSLC2 & "','" & PLTDSCSS & "', '" + heretar & "','" & WEBDSIMP & "','" & WEBDSCND & "'," & WEBWNMTH.text & ",'" & esForm & "','" & esEML & "','" & WEBDSEBO.text.Replace("'", "''") & "','" & WEBDSDES.text.Replace("'", "''") & "','" & WEBDSPCL.text.Replace("'", "''") & "','" & esSSL & "')"


                'IF Request("lstIdioma")=1 THEN
                strSql &= ";UPDATE METLNOD SET NODDSTXT='" & WEBDSTIT.text.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "' WHERE NODINNOD=" & txtcodiNode.text

                'Si hi ha p&agrave;gines en altres idiomes, actualitzo tots els valors comuns
                'strsql &= ";UPDATE METLWEB2 SET  WEBDTPUB='"&WEBDTPUB.Text.Tostring()&"', WEBDTCAD='"&dataCaducitat.Tostring()&"', WEBDTANY=getDate(), WEBDSUSR='" & session("codiOrg") & "',WEBTPBUS='"& buscar &"',', WEBDSTCO='" & PLTDSTCO & "', WEBDSPLA='" & PLTDSPLT & "', WEBDSEST='" & PLTDSEST & "', WEBDSHTM='" & PLTDSHTM & "', WEBDSLCW='" & PLTDSLCW & "', WEBDSLC2='" & PLTDSLC2 & "', WEBDSCSS='" & PLTDSCSS & "',  WEBTPHER='" & heretar & "',  WEBDSIMP='" & WEBDSIMP &"',WEBDSCND='" & WEBDSCND & "', WEBWNMTH=" & WEBWNMTH.text & ",WEBSWFRM='" & esForm & "' , WEBSWEML='" & esEML & "',WEBDSEBO='" & WEBDSEBO.text.Replace("'","''") & "', WEBSWSSL='" & esSSL & "' WHERE WEBINIDI<>1 AND WEBINNOD=" & txtCodiNode.text			 
                'END IF
                lblResultat.text = "<div class='alert alert-dismissible alert-success mt-2 mb-2'><button type='button' class='close' data-dismiss='alert'>x</button>P&agrave;gina Web modificada amb èxit.<br/><br/><a href=""http://lhintranet/GAIA2/aspx/web/frmplantilla.aspx?tipus=W"" class=""btn btn-sm btn-primary"">Nova p&agrave;gina web</a></div>"
                'GAIA2.escriuResultat(objConn,lblResultat , "P&agrave;gina Web modificada amb èxit.","<a href=""http://lhintranet/GAIA2/aspx/web/frmplantilla.aspx?tipus=W"" class=""btn btn-sm btn-primary"">Nova p&agrave;gina web</a>")			

                gaia2.debug(Nothing, "est=" & PLTDSEST)
                gaia2.debug(Nothing, "htm=" & PLTDSHTM)
                gaia2.debug(Nothing, strSql)
                gaia2.bdsr(objconn, "BEGIN TRANSACTION " & strSql & "; COMMIT TRANSACTION")

            Case "N"

                If Not Request("id") Is Nothing Then  'Tengo el id, modifico el nodo.
                    txtcodiNode.text = Request("id").ToString()
                Else ' No tengo id, inserto un nuevo nodo.
                    'Inserto el node web    '9 node web
                    txtcodiNode.text = GAIA2.insertarNode(objconn, 9, NWEDSTIT.text.replace("<p>", "").replace("</p>", ""), Session("codiOrg"))
                    ' Creo la relación y los permisos. Coloco el nodo el el meu arbre personal, sense classificar.
                    GAIA2.insertaNodeArbrePersonal(objconn, 9, txtCodiNode.Text, Session("codiOrg"), "")
                End If



                'per fer proves
                'txtCodiNode.text = 272518

                strSql &= "DELETE FROM METLNWE2 WHERE NWEINNOD=" & txtcodiNode.text & " AND NWEINIDI=" & codiIdioma

                strSql &= ";INSERT INTO METLNWE2 (NWEINNOD, NWEINIDI,NWEDSTIT,NWEDSCAR,NWEDSUSR, NWEDSTCO, NWEDSPLA, NWEDSEST, NWEDSHTM, NWEDSLCW, NWEDSCSS, NWEDSEBO, NWEDSMET, NWEDSPEU, NWEDSCSP, NWEDSCSI, NWEDTPUB, NWEDTCAD) VALUES (" & txtcodiNode.text & "," & codiIdioma & ",'" & NWEDSTIT.text.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "','" & NWEDSCAR.text.tostring().Replace("'", "''") & "','" & Session("codiOrg") & "','" & PLTDSTCO & "','" & PLTDSPLT & "','" & PLTDSEST & "','" & PLTDSHTM & "','" & PLTDSLCW & "','" & PLTDSCSS & "','" & NWEDSEBO.text.Replace("'", "''") & "','" & NWEDSMET.text.Replace("'", "''") & "','" & NWEDSPEU.text.Replace("'", "''") & "','" & NWEDSCSP.text.Replace("'", "''") & "','" & NWEDSCSI.text.Replace("'", "''") & "',getdate(), '1/1/1900')"


                'If Request("lstIdioma") = 1 Then
                strSql &= ";UPDATE METLNOD SET NODDSTXT='" & NWEDSTIT.text.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "' WHERE NODINNOD=" & txtcodiNode.text
                'Si hi ha p&agrave;gines en altres idiomes, actualitzo tots els valors comuns

                'strsql &= ";UPDATE METLNWE2  SET NWEDSTIT='" & NWEDSTIT.text.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "',NWEDSCAR='" & NWEDSCAR.text.tostring().Replace("'", "''") & "', NWEDSTCO = '" & PLTDSTCO & "', NWEDSPLA='" & PLTDSPLT & "', NWEDSEST='" & PLTDSEST & "',NWEDSHTM=' " & PLTDSHTM & "', NWEDSLCW='" & PLTDSLCW & "', NWEDSCSS='" & PLTDSCSS & "',NWEDSEBO=" & NWEDSEBO.text.Replace("'", "''") & "', NWEDSMET= '" & NWEDSMET.text.Replace("'", "''") & "',NWEDSPEU='" & NWEDSPEU.text.Replace("'", "''") & "', NWEDSCSP='" & NWEDSCSP.text.Replace("'", "''") & "', NWEDSCSI='" & NWEDSCSI.text.Replace("'", "''") & "' WHERE NWEINIDI<>1 AND NWEINNOD=" & txtCodiNode.text
                'End If
                lblResultat.text = "<div class='alert alert-dismissible alert-success mt-2 mb-2'><button type='button' class='close' data-dismiss='alert'>x</button>Node Web modificat amb èxit.<br/><br/><a href=""http://lhintranet/GAIA2/aspx/web/frmplantilla.aspx?tipus=N"">&nbsp;Nova p&agrave;gina web</a></div>"
                'GAIA2.escriuResultat(objConn, lblResultat, "Node Web modificat amb èxit.", "<a href=""http://lhintranet/GAIA2/aspx/web/frmplantilla.aspx?tipus=N"">&nbsp;Nova p&agrave;gina web</a>")

                gaia2.bdsr(objconn, "BEGIN TRANSACTION " & strSql & "; COMMIT TRANSACTION")

            Case "A"

                If Not Request("id") Is Nothing Then
                    txtcodiNode.text = Request("id").ToString()
                Else
                    'Inserto el node web    '8 arbre web
                    txtcodiNode.text = GAIA2.insertarNode(objconn, 8, AWEDSTIT.text.replace("<p>", "").replace("</p>", ""), Session("codiOrg"))
                    'Creo una relació del node amb si mateix perque és el primer
                    Dim rel As New clsRelacio
                    ' La funcion creaRelacio, crea también los permisos
                    rel = GAIA2.creaRelacio(objconn, 8, txtCodiNode.Text, txtCodiNode.Text, 0, "", -1, 1, -1, 1, False, Session("codiOrg"))
                End If



                strSql &= "DELETE FROM METLAWE2 WHERE AWEINNOD=" & txtcodiNode.text & " AND AWEINIDI=" & codiIdioma

                strSql &= ";INSERT INTO METLAWE2  VALUES (" & txtcodiNode.text & "," & codiIdioma & ",'" & AWEDSTIT.text.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "'," & lstAWEDSSER.SelectedItem.Value & ",'" & AWEDSROT.text & "'," & Session("codiOrg") & ",'" & PLTDSTCO & "','" & PLTDSPLT & "','" & PLTDSEST & "','" & PLTDSHTM & "','" & PLTDSLCW & "','" & PLTDSLC2 & "','" & AWEDSDOC.text & "','" & AWEDSEBO.text & "','" & AWEDSMET.text & "','" & AWEDSPEU.text & "','" & AWEDSCSP.text & "','" & AWEDSCSI.text & "','" & AWEDSNOM.text & "')"


                'IF Request("lstIdioma")=1 THEN
                strSql &= ";UPDATE METLNOD SET NODDSTXT='" & AWEDSTIT.text.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "' WHERE NODINNOD=" & txtcodiNode.text
                'Si hi ha p&agrave;gines en altres idiomes, actualitzo tots els valors comuns

                'strsql &= ";UPDATE METLAWE2  SET AWEDSTIT='"& AWEDSTIT.text.Replace("'","''").replace("<p>","").replace("</p>","") & "', AWEDSSER=" & lstAWEDSSER.SelectedItem.Value & ",AWEDSROT='" & AWEDSROT.text  & "', AWEDSTCO='" & PLTDSTCO & "', AWEDSPLA='" & PLTDSPLT & "', AWEDSEST='" & PLTDSEST & "',AWEDSHTM='" & PLTDSHTM & "',AWEDSLCW='" &  PLTDSLCW & "',AWEDSLC2='" & PLTDSLC2 & "',AWEDSDOC='" & AWEDSDOC.text & "',AWEDSMET='" & AWEDSEBO.text  & "',AWEDSMET='" & AWEDSMET.text  & "',AWEDSPEU='" & AWEDSPEU.text  & "',AWEDSCSP='" & AWEDSCSP.text  & "',AWEDSCSI'" & AWEDSCSI.text  &  "' WHERE AWEINIDI<>1 AND AWEINNOD=" & txtCodiNode.text			 
                'END IF
                lblResultat.text = "<div class='alert alert-dismissible alert-success mt-2 mb-2'><button type='button' class='close' data-dismiss='alert'>x</button>Arbre Web modificat amb èxit.<br/><br/><a href=""http://lhintranet/GAIA2/aspx/web/frmplantilla.aspx?tipus=A"" class=""txtRojo12Px"">&nbsp;Nou arbre web</a></div>"
                'GAIA2.escriuResultat(objConn,lblResultat , "Arbre Web modificat amb èxit.","<a href=""http://lhintranet/GAIA2/aspx/web/frmplantilla.aspx?tipus=A"" class=""txtRojo12Px"">&nbsp;Nou arbre web</a>")			

                'MAX: falta crear relació, permisos... 
                gaia2.bdsr(objconn, "BEGIN TRANSACTION " & strSql & "; COMMIT TRANSACTION")

            Case Else 'plantilla
                If Not Request("id") Is Nothing Then
                    txtcodiNode.text = Request("id").ToString()
                Else
                    'Inserto el node fulla web
                    txtcodiNode.text = GAIA2.insertarNode(objconn, 24, txtPLTDSTIT.text.replace("<p>", "").replace("</p>", ""), Session("codiOrg"))
                    GAIA2.insertaNodeArbrePersonal(objconn, 24, txtCodiNode.Text, Session("codiOrg"), "")
                End If

                'PLTDSALT= PLTDSALT.replace(",","|")		
                strSql = "DELETE FROM METLPLT2 WHERE PLTINNOD=" & txtcodiNode.text
                strSql &= ";INSERT INTO METLPLT2 VALUES(" & txtcodiNode.text & ",'" & txtPLTDSTIT.text & "',getdate(), " & Session("codiOrg") & ",'" & PLTDSCMP & "','" & PLTDSEST & "','" & PLTDSCSS & "','" & PLTDSLNK & "','" & PLTDSIMG & "','" & PLTDSTCO & "','" & PLTDSLCW & "','" & PLTDSLC2 & "','" & PLTDSALK & "','" & PLTCDPAL & "','" & PLTDSAAL & "','" & PLTDSALT & "','" & PLTDSPLT & "','" & PLTDSFLW & "','" & txtPLTDSOBS.text & "'," & PLTSWALT & ",'" & PLTDSNUM & "','" & PLTDSALF & "','" & PLTDSNIV & "'," & IIf(chkPLTSWVIS.checked, 1, 0) & ",'" & PLTDSHTM & "')"

                lblResultat.text = "<div class='alert alert-dismissible alert-success mt-2 mb-2'><button type='button' class='close' data-dismiss='alert'>x</button>Plantilla Web modificada amb èxit.<br/><br/><a href=""http://lhintranet/GAIA2/aspx/web/frmplantilla.aspx?tipus=P"" class=""txtRojo12Px"">&nbsp;Nova plantilla</a></div>"
                'GAIA2.escriuResultat(objConn,lblResultat , "Plantilla modificada amb èxit.","<a href=""http://lhintranet/GAIA2/aspx/web/frmplantilla.aspx?tipus=P"" class=""btn btn-sm btn-primary"">Nova plantilla</a>")			

                gaia2.bdsr(objconn, "BEGIN TRANSACTION " & strSql & "; COMMIT TRANSACTION")
        End Select

        txtEstBD.value = txtEstBD.value.replace("''", """")

        carregaCamps()
    End Sub


    Protected Sub inicialitzaLListaServidors()

        Dim DS As DataSet
        DS = New DataSet()
        GAIA2.bdR(objconn, "SELECT SERINCOD, SERDSURL FROM METLSER WITH (NOLOCK)", DS)
        lstAWEDSSER.DataSource = DS.Tables(0).DefaultView
        lstAWEDSSER.datatextfield = "SERDSURL"
        lstAWEDSSER.datavaluefield = "SERINCOD"
        lstAWEDSSER.DataBind()
        lstAWEDSSER.Items.insert(0, New ListItem("Selecciona un servidor", "0"))
        DS.Dispose()
    End Sub 'inicialitzaLListaServidors

    Protected Sub canviIdioma(sender As Object, e As EventArgs)
        Dim codiIdioma As Integer
        codiIdioma = lstCanviIdioma.SelectedItem.Value

        carregaCamps(codiIdioma)

    End Sub 'canviIdioma
End Class