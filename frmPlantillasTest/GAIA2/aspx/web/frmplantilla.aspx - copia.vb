﻿Imports System.Data.OleDb
Imports System.Data
Imports System.Web



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
    Public tipodePlantilla As String = "P"
    Public idPlantilla As String = "354860"
    ' idPlantilla corresponde a la variable Request("id")


    Private Sub Page_UnLoad(sender As Object, e As System.EventArgs) Handles MyBase.Unload
        GAIA.bdFi(objconn)
    End Sub 'Page_UnLoad
    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load

        'If HttpContext.Current.User.Identity.Name.Length > 0 Then

        '    If (Session("nif") Is Nothing) Then
        '        Session("nif") = GAIA.nifUsuari(objconn, HttpContext.Current.User.Identity.Name).Trim()
        '    End If

        '    If Session("codiOrg") Is Nothing Then
        '        Session("CodiOrg") = GAIA.trobaNodeUsuari(objconn, Session("nif")).ToString().Trim()
        '    End If


        'End If
        'nif = Session("nif").Trim()
        Session("codiOrg") = 346231


        If Session("codiOrg") = 49730 Or Session("codiOrg") = 49727 Or Session("codiOrg") = 56935 Or Session("codiOrg") = 80879 Or Session("codiOrg") = 48729 Or Session("codiOrg") = 297650 Or Session("codiOrg") = 302362 Or Session("codiOrg") = 313486 Or Session("codiOrg") = 346231 Then
            objconn = GAIA.bdIni()
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

        nroId.Value = 0
        tipodePlantilla = "P"
        'Request("tipus")



        'Select Case Request("tipus")
        Select Case tipodePlantilla

            Case "A"
                ltTitol.Text = "Arbre web"
                pnlArbreWeb.Visible = True
                pnlcanviIdioma.Visible = True
                pnlNodeWeb.Visible = False
                pnlFullaWeb.Visible = False
                pnlPlantilla.Visible = False
                pnlWebCamps1.Visible = False
                pnlPltCamps1.Visible = False
                pnlPltCamps2.Visible = False
                pnlPltCamps3.Visible = False
				pnlLCWA.Visible = true
                pnlLCW2.Visible = True
                pnlPlt.Visible = False
                pnlEstils.Visible = False
                inicialitzaLListaServidors()
            Case "N"
                ltTitol.Text = "Node web"
                pnlArbreWeb.Visible = False
                pnlcanviIdioma.Visible = True
                pnlNodeWeb.Visible = True
                pnlFullaWeb.Visible = False
                pnlPlantilla.Visible = False
                pnlWebCamps1.Visible = False
                pnlPltCamps1.Visible = False
                pnlPltCamps2.Visible = False
                pnlPltCamps3.Visible = False
				pnlLCWA.Visible = true
                pnlLCW2.Visible = False
                pnlPlt.Visible = True
                pnlEstils.Visible = True
            Case "W"
                ltTitol.Text = "Fulla web"
                pnlArbreWeb.Visible = False
                pnlcanviIdioma.Visible = True
                pnlNodeWeb.Visible = False
                pnlFullaWeb.Visible = True
                pnlPlantilla.Visible = False
                pnlWebCamps1.Visible = True
                pnlPltCamps1.Visible = False
                pnlPltCamps2.Visible = False
                pnlPltCamps3.Visible = False
				pnlLCWA.Visible = true
                pnlLCW2.Visible = True
                pnlPlt.Visible = True
                pnlEstils.Visible = True
            Case Else
                ltTitol.Text = "Plantilla web"
                pnlArbreWeb.Visible = False
                pnlcanviIdioma.Visible = False
                pnlNodeWeb.Visible = False
                pnlPlantilla.Visible = True
                pnlFullaWeb.Visible = False
                pnlWebCamps1.Visible = False
                pnlPltCamps1.Visible = True
                pnlPltCamps2.Visible = True
                pnlPltCamps3.Visible = True
				pnlLCWA.Visible = true
                pnlLCW2.Visible = True
                pnlPlt.Visible = True
                pnlEstils.Visible = True
                'ltCanviCampsDb.Text = "if ($('#lstTipusFulla').val()!= null) {canviCampsDB($('#lstTipusFulla').val()) ;}"
        End Select


        If idPlantilla = "" Then
            ltEst.Text = "<div class=""b-inicial p-inicial inactivo"" id=""d00""><span class=""atributs"" style=""display: none;"">div0#div############297693##############false#false#####################1720###1759,1760###########1763#####|</span></div>"
            txtEst.Value = ltEst.Text
        Else
            'Select Case Request("tipus")
            Select Case tipodePlantilla

                Case "W" 'fulla web
                    qSQL = "select * FROM METLWEB2 WITH(NOLOCK) WHERE WEBINNOD=" & idPlantilla & " AND WEBINIDI=" & codiIdioma
                    GAIA.bdr(objconn, qSQL, ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        dbRow = ds.Tables(0).Rows(0)
                        txtEst.Value = dbRow("WEBDSHTM").replace("''", "'")

                        ltEst.Text = txtEst.Value

                        'busco el nroId més gran			
                        Dim cont As Integer = 0
                        Dim idTmp As Integer = 0
                        arrayTmp = Regex.Split(dbRow("WEBDSHTM"), "id='d")
                        For Each item As String In arrayTmp
                            If cont > 0 Then
                                idTmp = item.Substring(0, InStr(item, "'") - 1)
                                If idTmp >= nroId.Value Then nroId.Value = idTmp
                            End If
                            cont += 1
                        Next item

                        WEBDSTIT.Text = dbRow("WEBDSTIT")
                        WEBDSDES.Text = dbRow("WEBDSDES")
                        WEBDSPCL.Text = dbRow("WEBDSPCL")

                        WEBTPBUS.Checked = IIf(dbRow("WEBTPBUS") = "S", True, False)
                        WEBDTPUB.Text = dbRow("WEBDTPUB")
                        WEBDTCAD.Text = dbRow("WEBDTCAD")
                        WEBDSFIT.Text = dbRow("WEBDSFIT")

                        WEBDSURL.Text = dbRow("WEBDSURL")
                        WEBTPHER.Checked = IIf(dbRow("WEBTPHER") = "S", True, False)

                        WEBSWFRM.Checked = IIf(dbRow("WEBSWFRM") = "S", True, False)
                        WEBSWEML.Checked = IIf(dbRow("WEBSWEML") = "S", True, False)
                        WEBSWSSL.Checked = IIf(dbRow("WEBSWSSL") = "S", True, False)

                        WEBDSEBO.Text = dbRow("WEBDSEBO")
                        WEBWNMTH.Text = dbRow("WEBWNMTH")

                    Else 'No he trobat l'idioma demanat i busco un altre
                        qSQL = "select * FROM METLWEB2 WITH(NOLOCK) WHERE WEBINNOD=" & idPlantilla & " ORDER BY WEBINIDI ASC "
                        GAIA.bdr(objconn, qSQL, ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            carregaCamps(ds.Tables(0).Rows(0)("WEBINIDI"))
                        End If
                    End If

                Case "N" 'node web
                    qSQL = "select * FROM METLNWE2 WITH(NOLOCK) WHERE NWEINNOD=" & idPlantilla & " AND NWEINIDI=" & codiIdioma
                    GAIA.bdr(objconn, qSQL, ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        dbRow = ds.Tables(0).Rows(0)
                        txtEst.Value = dbRow("NWEDSHTM")
                        ltEst.Text = dbRow("NWEDSHTM")

                        'busco el nroId més gran			
                        Dim cont As Integer = 0
                        Dim idTmp As Integer = 0
                        arrayTmp = Regex.Split(dbRow("NWEDSHTM"), "id='d")
                        For Each item As String In arrayTmp
                            If cont > 0 Then
                                idTmp = item.Substring(0, InStr(item, "'") - 1)
                                If idTmp >= nroId.Value Then nroId.Value = idTmp
                            End If
                            cont += 1
                        Next item
                        NWEDSTIT.Text = dbRow("NWEDSTIT")
                        NWEDSCAR.Text = dbRow("NWEDSCAR")
                        NWEDSEBO.Text = dbRow("NWEDSEBO")
                        NWEDSMET.Text = dbRow("NWEDSMET")
                        NWEDSPEU.Text = dbRow("NWEDSPEU")
                        NWEDSCSP.Text = dbRow("NWEDSCSP")
                        NWEDSCSI.Text = dbRow("NWEDSCSI")

                    Else 'No he trobat l'idioma demanat i busco un altre
                        qSQL = "select * FROM METLNWE2 WITH(NOLOCK) WHERE NWEINNOD=" & idPlantilla & " ORDER BY NWEINIDI ASC "
                        GAIA.bdr(objconn, qSQL, ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            carregaCamps(ds.Tables(0).Rows(0)("NWEINIDI"))
                        End If
                    End If

                Case "A" 'arbre web

                    qSQL = "select * FROM METLAWE2 WITH(NOLOCK) WHERE AWEINNOD=" & idPlantilla & " AND AWEINIDI=" & codiIdioma
                    GAIA.bdr(objconn, qSQL, ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        dbRow = ds.Tables(0).Rows(0)
                        txtEst.Value = dbRow("AWEDSHTM")
                        ltEst.Text = dbRow("AWEDSHTM")

                        'busco el nroId més gran			
                        Dim cont As Integer = 0
                        Dim idTmp As Integer = 0
                        arrayTmp = Regex.Split(dbRow("AWEDSHTM"), "id='d")
                        For Each item As String In arrayTmp
                            If cont > 0 Then
                                idTmp = item.Substring(0, InStr(item, "'") - 1)
                                If idTmp >= nroId.Value Then nroId.Value = idTmp
                            End If
                            cont += 1
                        Next item
                        AWEDSTIT.Text = dbRow("AWEDSTIT")
                        AWEDSNOM.Text = dbRow("AWEDSNOM")
                        AWEDSROT.Text = dbRow("AWEDSROT")
                        AWEDSDOC.Text = dbRow("AWEDSDOC")

                        AWEDSEBO.Text = dbRow("AWEDSEBO")
                        AWEDSMET.Text = dbRow("AWEDSMET")
                        AWEDSPEU.Text = dbRow("AWEDSPEU")
                        AWEDSCSP.Text = dbRow("AWEDSCSP")
                        AWEDSCSI.Text = dbRow("AWEDSCSI")
                        lstAWEDSSER.Items.FindByValue(dbRow("AWEDSSER")).Selected = True

                    Else 'No he trobat l'idioma demanat i busco un altre
                        qSQL = "select * FROM METLAWE2 WITH(NOLOCK) WHERE AWEINNOD=" & idPlantilla & " ORDER BY AWEINIDI ASC "
                        GAIA.bdr(objconn, qSQL, ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            carregaCamps(ds.Tables(0).Rows(0)("AWEINIDI"))
                        End If
                    End If

                Case Else   'plantilla
                    qSQL = "select * FROM METLPLT2 WITH(NOLOCK) WHERE PLTINNOD=" & idPlantilla
                    GAIA.bdr(objconn, qSQL, ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        dbRow = ds.Tables(0).Rows(0)
                        txtEst.Value = dbRow("PLTDSHTM")
                        ltEst.Text = dbRow("PLTDSHTM")

                        'busco el nroId més gran			
                        Dim cont As Integer = 0
                        Dim idTmp As Integer = 0
                        arrayTmp = Regex.Split(dbRow("PLTDSHTM"), "id='d")

                        For Each item As String In arrayTmp.Skip(1)
                            idTmp = Integer.Parse(item.Substring(0, 2))
                            If idTmp >= nroId.Value Then nroId.Value = idTmp
                        Next item

                        txtPLTDSTIT.Text = dbRow("PLTDSTIT")
                        txtPLTDSOBS.Text = dbRow("PLTDSOBS")
                        chkPLTSWVIS.Checked = dbRow("PLTSWVIS")
                    End If
            End Select
        End If

    End Sub


    Protected Sub carregallistaEstilsCSS()
        Dim ds As New DataSet, qSQL As String, dv As DataView
        Dim element As ListItem
        Dim item As DataRow
        qSQL = "select isnull(CSSDSCSS,'1') as CSSDSCSS,CSSINTIP,CSSINCOD,CSSDSTXT from METLCSS WITH(NOLOCK) ORDER BY CSSDSTXT"
        GAIA.bdr(objconn, qSQL, ds)
        'colors

        dv = ds.Tables(0).DefaultView


        For Each item In dv.Table.Rows
            If item("CSSINTIP") = 23 Then
                element = New ListItem(item("CSSDSTXT"), item("CSSINCOD"))
                If item("CSSDSCSS") <> "1" And item("CSSDSTXT") <> "blanc" Then
                    element.Attributes.Add("style", item("CSSDSCSS"))
                End If
                ddlb_23.Items.Add(element)
            End If
        Next item
        'ddlb_23.Items.Insert(0, new ListItem("",""))
        ddlb_23.Items.Insert(0, New ListItem("", ""))
        ' ddlb_23.Items(0).Selected = True


        dv.RowFilter = "CSSINTIP=25"
        ddlb_25.DataSource = dv
        ddlb_25.DataTextField = "CSSDSTXT"
        ddlb_25.DataValueField = "CSSINCOD"
        ddlb_25.DataBind()
        ddlb_25.Items.Insert(0, New ListItem("", 0))


        'font
        dv.RowFilter = "CSSINTIP=26"
        ddlb_26.DataSource = dv
        ddlb_26.DataTextField = "CSSDSTXT"
        ddlb_26.DataValueField = "CSSINCOD"
        ddlb_26.DataBind()
        ddlb_26.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=27"
        ddlb_27.DataSource = dv
        ddlb_27.DataTextField = "CSSDSTXT"
        ddlb_27.DataValueField = "CSSINCOD"
        ddlb_27.DataBind()
        ddlb_27.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=28"
        ddlb_28.DataSource = dv
        ddlb_28.DataTextField = "CSSDSTXT"
        ddlb_28.DataValueField = "CSSINCOD"
        ddlb_28.DataBind()
        ddlb_28.Items.Insert(0, New ListItem("", 0))


        dv.RowFilter = "CSSINTIP=103"
        ddlb_103.DataSource = dv
        ddlb_103.DataTextField = "CSSDSTXT"
        ddlb_103.DataValueField = "CSSINCOD"
        ddlb_103.DataBind()
        ddlb_103.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=105"
        ddlb_105.DataSource = dv
        ddlb_105.DataTextField = "CSSDSTXT"
        ddlb_105.DataValueField = "CSSINCOD"
        ddlb_105.DataBind()
        ddlb_105.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=108"
        ddlb_108.DataSource = dv
        ddlb_108.DataTextField = "CSSDSTXT"
        ddlb_108.DataValueField = "CSSINCOD"
        ddlb_108.DataBind()
        ddlb_108.Items.Insert(0, New ListItem("", 0))

        'dv.RowFilter="CSSINTIP=109"
        'ddlb_109.datasource=dv
        'ddlb_109.datatextfield="CSSDSTXT"
        'ddlb_109.datavaluefield="CSSINCOD"
        'ddlb_109.databind()
        'ddlb_109.Items.Insert(0, new ListItem("",0))

        dv.RowFilter = "CSSINTIP=110"
        ddlb_110.DataSource = dv
        ddlb_110.DataTextField = "CSSDSTXT"
        ddlb_110.DataValueField = "CSSINCOD"
        ddlb_110.DataBind()
        ddlb_110.Items.Insert(0, New ListItem("", 0))
		

        dv.RowFilter = "CSSINTIP=111"
        ddlb_111.DataSource = dv
        ddlb_111.DataTextField = "CSSDSTXT"
        ddlb_111.DataValueField = "CSSINCOD"
        ddlb_111.DataBind()
        ddlb_111.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=112"
        ddlb_112.DataSource = dv
        ddlb_112.DataTextField = "CSSDSTXT"
        ddlb_112.DataValueField = "CSSINCOD"
        ddlb_112.DataBind()
        ddlb_112.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=122"
        ddlb_122.DataSource = dv
        ddlb_122.DataTextField = "CSSDSTXT"
        ddlb_122.DataValueField = "CSSINCOD"
        ddlb_122.DataBind()
        ddlb_122.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=114"
        ddlb_114.DataSource = dv
        ddlb_114.DataTextField = "CSSDSTXT"
        ddlb_114.DataValueField = "CSSINCOD"
        ddlb_114.DataBind()
        ddlb_114.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=115"
        ddlb_115.DataSource = dv
        ddlb_115.DataTextField = "CSSDSTXT"
        ddlb_115.DataValueField = "CSSINCOD"
        ddlb_115.DataBind()
        ddlb_115.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=123"
        ddlb_123.DataSource = dv
        ddlb_123.DataTextField = "CSSDSTXT"
        ddlb_123.DataValueField = "CSSINCOD"
        ddlb_123.DataBind()
        ddlb_123.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=647"  'Nuevo grupo de estilos: G2-Estils definits
        ddlb_117.DataSource = dv
        ddlb_117.DataTextField = "CSSDSTXT"
        ddlb_117.DataValueField = "CSSINCOD"
        ddlb_117.DataBind()
        ddlb_117.Items.Insert(0, New ListItem("", 0))

        'fons

        ddlb_118.Items.Insert(0, New ListItem("", 0))
        For Each item In dv.Table.Rows
            If item("CSSINTIP") = 118 Then
                element = New ListItem(item("CSSDSTXT"), item("CSSINCOD"))
                If item("CSSDSCSS") <> "1" And InStr(item("CSSDSCSS"), "000000") = 0 Then
                    'element.Attributes.Add("style",item("CSSDSCSS"))
                End If
                ddlb_118.Items.Add(element)
            End If
        Next item



        dv.RowFilter = "CSSINTIP=119"
        ddlb_119.DataSource = dv
        ddlb_119.DataTextField = "CSSDSTXT"
        ddlb_119.DataValueField = "CSSINCOD"
        ddlb_119.DataBind()
        ddlb_119.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=124"
        ddlb_124.DataSource = dv
        ddlb_124.DataTextField = "CSSDSTXT"
        ddlb_124.DataValueField = "CSSINCOD"
        ddlb_124.DataBind()
        ddlb_124.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=630"
        ddlb_630.DataSource = dv
        ddlb_630.DataTextField = "CSSDSTXT"
        ddlb_630.DataValueField = "CSSINCOD"
        ddlb_630.DataBind()
        ddlb_630.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=631"
        ddlb_631.DataSource = dv
        ddlb_631.DataTextField = "CSSDSTXT"
        ddlb_631.DataValueField = "CSSINCOD"
        ddlb_631.DataBind()
        ddlb_631.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=632"
        ddlb_632.DataSource = dv
        ddlb_632.DataTextField = "CSSDSTXT"
        ddlb_632.DataValueField = "CSSINCOD"
        ddlb_632.DataBind()
        ddlb_632.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=633"
        ddlb_633.DataSource = dv
        ddlb_633.DataTextField = "CSSDSTXT"
        ddlb_633.DataValueField = "CSSINCOD"
        ddlb_633.DataBind()
        ddlb_633.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=634"
        ddlb_634.DataSource = dv
        ddlb_634.DataTextField = "CSSDSTXT"
        ddlb_634.DataValueField = "CSSINCOD"
        ddlb_634.DataBind()
        ddlb_634.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=635"
        ddlb_635.DataSource = dv
        ddlb_635.DataTextField = "CSSDSTXT"
        ddlb_635.DataValueField = "CSSINCOD"
        ddlb_635.DataBind()
        ddlb_635.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=636"
        ddlb_636.DataSource = dv
        ddlb_636.DataTextField = "CSSDSTXT"
        ddlb_636.DataValueField = "CSSINCOD"
        ddlb_636.DataBind()
        ddlb_636.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=637"
        ddlb_637.DataSource = dv
        ddlb_637.DataTextField = "CSSDSTXT"
        ddlb_637.DataValueField = "CSSINCOD"
        ddlb_637.DataBind()
        ddlb_637.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=638"
        ddlb_638.DataSource = dv
        ddlb_638.DataTextField = "CSSDSTXT"
        ddlb_638.DataValueField = "CSSINCOD"
        ddlb_638.DataBind()
        ddlb_638.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=639"
        ddlb_639.DataSource = dv
        ddlb_639.DataTextField = "CSSDSTXT"
        ddlb_639.DataValueField = "CSSINCOD"
        ddlb_639.DataBind()
        ddlb_639.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=641"
        ddlb_641.DataSource = dv
        ddlb_641.DataTextField = "CSSDSTXT"
        ddlb_641.DataValueField = "CSSINCOD"
        ddlb_641.DataBind()
        ddlb_641.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=642"
        ddlb_642.DataSource = dv
        ddlb_642.DataTextField = "CSSDSTXT"
        ddlb_642.DataValueField = "CSSINCOD"
        ddlb_642.DataBind()
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

        dv.RowFilter = "CSSINTIP=673"
        ddlb_673.DataSource = dv
        ddlb_673.DataTextField = "CSSDSTXT"
        ddlb_673.DataValueField = "CSSINCOD"
        ddlb_673.DataBind()
        ddlb_673.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=674"
        ddlb_674.DataSource = dv
        ddlb_674.DataTextField = "CSSDSTXT"
        ddlb_674.DataValueField = "CSSINCOD"
        ddlb_674.DataBind()
        ddlb_674.Items.Insert(0, New ListItem("", 0))

        dv.RowFilter = "CSSINTIP=675"
        ddlb_675.DataSource = dv
        ddlb_675.DataTextField = "CSSDSTXT"
        ddlb_675.DataValueField = "CSSINCOD"
        ddlb_675.DataBind()
        ddlb_675.Items.Insert(0, New ListItem("", 0))

        ds.Dispose
    End Sub 'carregallistaEstilsCSS


    Protected Sub carregaTipusContinguts()
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim DV As DataView

        GAIA.bdr(objconn, "SELECT TIPINTIP,TIPDSDES,TBLDSTXT FROM  METLTIP WITH(NOLOCK), METLTBL WITH(NOLOCK) WHERE TIPINTIP=TBLINTFU", DS)
        DV = DS.Tables(0).DefaultView

        If tipodePlantilla = "P" Or tipodePlantilla = "" Then

            
            lstTipusFulla.DataSource = DV
            lstTipusFulla.DataTextField = "TIPDSDES"
            lstTipusFulla.DataValueField = "TIPINTIP"
            lstTipusFulla.DataBind()
            lstTipusFulla.Items.Insert(0, New ListItem("", 0))
            lblCodi.Text = "<script language=""javascript"">"
            lblCodi.Text += "var longitud = document.getElementById('lstTipusFulla').options.length;"
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

            lblCodi.Text += "function canviCampsDB(valor) { 
               var temp=document.getElementById('ddlPLTDSCMP');
               var temp2=document.getElementById('ddlPLTDSLNK');
               var temp3=document.getElementById('ddlPLTDSALT');
                temp.options[0]=new Option("" "","" "");
                temp2.options[0]=new Option("" "","" "");
                temp3.options[0]=new Option("" "","" "");
                var x = 0;
                for (j=0;j<taules.length;j++) 
                  { if (taules[j]==valor) { x = j;	break;	}	}	
               for (m=temp.options.length-1;m>0; m--) 
                 { temp.options[m]=null;temp2.options[m]=null;temp3.options[m]=null;}
              for (i=1;i<camps[x].length;i++) 
            {	   temp.options[i]=new Option(camps[x][i].text,camps[x][i].value);
                   temp2.options[i]=new Option(camps[x][i].text,camps[x][i].value);
                   temp3.options[i]=new Option(camps[x][i].text,camps[x][i].value);	}
               temp.options[0].selected=true;
               temp2.options[0].selected=true;
               temp3.options[0].selected=true;}"
            lblCodi.Text += "</script>"
        Else
            lstTipusFulla.DataSource = DV
            lstTipusFulla.DataTextField = "TIPDSDES"
            lstTipusFulla.DataValueField = "TIPINTIP"
            lstTipusFulla.DataBind()
            lstTipusFulla.Items.Insert(0, New ListItem("", 0))
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

        GAIA.bdr(objconn, "SELECT TOP 1 * FROM " & taula, DS)
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

        Dim PLTDSCMP As String = String.Empty, PLTDSLNK As String = "", PLTDSALT As String = "", PLTDSEST As String = "", PLTDSCSS As String = "", PLTDSTCO As String = "", PLTDSLCA As String = "", PLTDSLCW As String = "", PLTDSLC2 As String = "", PLTDSIMG As String = "", PLTCDPAL As String = "", PLTDSAAL As String = "", PLTDSPLT As String = "", PLTDSALK As String = "", PLTDSFLW As String = "", PLTSWALT As Integer = 1, PLTDSNUM As String = "", PLTDSALF As String = "", PLTDSNIV As String = "", PLTSWVIS As Integer = 0, WEBDSIMP As String = "", WEBDSCND As String = "", PLTDSHTM As String = ""
        codiIdioma = lstCanviIdioma.SelectedValue
  
        txtEstBD.Value = txtEstBD.Value.Replace("""", "'").Replace("''", "'").Replace("'", "''")
        PLTDSHTM = txtEstBD.Value.Replace("celActiva", "").Replace("rowActiva", "").Replace("contenidorActiu", "")

        PLTDSEST = txtEst.Value.Replace("""", "'").Replace("''", "'").Replace("'", "''")
       
        Dim cssTmp As String = ""
        Dim cont As Integer = 0
        LeerAtributos(PLTDSCMP, PLTDSLNK, PLTDSALT, PLTDSCSS, PLTDSTCO, PLTDSLCA, PLTDSLCW, PLTDSLC2, PLTDSIMG, PLTCDPAL, PLTDSAAL, PLTDSPLT, PLTDSALK, PLTDSFLW, PLTDSNUM, PLTDSALF, PLTDSNIV, WEBDSIMP, WEBDSCND, cssTmp, cont)


        Select Case tipodePlantilla

            Case "W"

                If WEBDTCAD.Text = "" Then dataCaducitat = "01/01/1900" Else dataCaducitat = WEBDTCAD.Text
                If WEBTPBUS.Checked Then buscar = "S" Else buscar = "N"
                If WEBTPHER.Checked Then heretar = "S" Else heretar = "N"
                If WEBSWFRM.Checked Then esForm = "S" Else esForm = "N"
                If WEBSWEML.Checked Then esEML = "S" Else esEML = "N"
                If WEBSWSSL.Checked Then esSSL = "S" Else esSSL = "N"


                If Not idPlantilla Is Nothing Then
                    txtCodiNode.Text = idPlantilla.ToString()
                Else
                    'Inserto el node fulla web
                    txtCodiNode.Text = GAIA.insertarNode(objconn, 10, WEBDSTIT.Text.Replace("<p>", "").Replace("</p>", ""), Session("codiOrg"))

                    'Inserto el node fulla web en l'arbre personal de l'usuari		
                    GAIA.insertaNodeArbrePersonal(objconn, 10, txtCodiNode.Text, Session("codiOrg"), "")
                End If


                'per fer proves
                'txtCodiNode.text = 272518

                strSql &= "DELETE FROM METLWEB2 WHERE WEBINNOD=" & txtCodiNode.Text & " AND WEBINIDI=" & codiIdioma
                strSql &= ";INSERT INTO METLWEB2 (WEBINNOD, WEBINIDI,WEBDSTIT,WEBDSFIT, WEBDSURL, WEBDTPUB, WEBDTCAD, WEBDTANY, WEBDSUSR,WEBTPBUS,WEBDSTCO, WEBDSPLA, WEBDSEST, WEBDSHTM,  WEBDSLCW, WEBDSLC2, WEBDSCSS, WEBTPHER, WEBDSIMP,WEBDSCND, WEBWNMTH,WEBSWFRM, WEBSWEML,WEBDSEBO, WEBDSDES, WEBDSPCL, WEBSWSSL) VALUES (" & txtCodiNode.Text & "," & codiIdioma & ",'" & WEBDSTIT.Text.Replace("'", "''").Replace("<p>", "").Replace("</p>", "") & "','" & WEBDSFIT.Text.ToString().Replace("'", "''") & "','" & WEBDSURL.Text.ToString().Replace("'", "''") & "','" & WEBDTPUB.Text.ToString() & "','" & dataCaducitat & "',getDate(),'" & Session("codiOrg") & "','" & buscar.ToString() & "','" & PLTDSTCO & "','" & PLTDSPLT & "','" & PLTDSEST & "','" & PLTDSHTM & "','" & PLTDSLCW & "','" & PLTDSLC2 & "','" & PLTDSCSS & "', '" + heretar & "','" & WEBDSIMP & "','" & WEBDSCND & "'," & WEBWNMTH.Text & ",'" & esForm & "','" & esEML & "','" & WEBDSEBO.Text.Replace("'", "''") & "','" & WEBDSDES.Text.Replace("'", "''") & "','" & WEBDSPCL.Text.Replace("'", "''") & "','" & esSSL & "')"


                'IF Request("lstIdioma")=1 THEN
                strSql &= ";UPDATE METLNOD SET NODDSTXT='" & WEBDSTIT.Text.Replace("'", "''").Replace("<p>", "").Replace("</p>", "") & "' WHERE NODINNOD=" & txtCodiNode.Text

                'Si hi ha p&agrave;gines en altres idiomes, actualitzo tots els valors comuns
                'strsql &= ";UPDATE METLWEB2 SET  WEBDTPUB='"&WEBDTPUB.Text.Tostring()&"', WEBDTCAD='"&dataCaducitat.Tostring()&"', WEBDTANY=getDate(), WEBDSUSR='" & session("codiOrg") & "',WEBTPBUS='"& buscar &"',', WEBDSTCO='" & PLTDSTCO & "', WEBDSPLA='" & PLTDSPLT & "', WEBDSEST='" & PLTDSEST & "', WEBDSHTM='" & PLTDSHTM & "', WEBDSLCW='" & PLTDSLCW & "', WEBDSLC2='" & PLTDSLC2 & "', WEBDSCSS='" & PLTDSCSS & "',  WEBTPHER='" & heretar & "',  WEBDSIMP='" & WEBDSIMP &"',WEBDSCND='" & WEBDSCND & "', WEBWNMTH=" & WEBWNMTH.text & ",WEBSWFRM='" & esForm & "' , WEBSWEML='" & esEML & "',WEBDSEBO='" & WEBDSEBO.text.Replace("'","''") & "', WEBSWSSL='" & esSSL & "' WHERE WEBINIDI<>1 AND WEBINNOD=" & txtCodiNode.text			 
                'END IF
                lblResultat.Text = "<div class='alert alert-dismissible alert-success mt-2 mb-2'><button type='button' class='close' data-dismiss='alert'>x</button>P&agrave;gina Web modificada amb èxit.<br/><br/><a href=""http://lhintranet/GAIA/aspx/web/frmplantilla.aspx?tipus=W"" class=""btn btn-sm btn-primary"">Nova p&agrave;gina web</a></div>"
                'GAIA.escriuResultat(objConn,lblResultat , "P&agrave;gina Web modificada amb èxit.","<a href=""http://lhintranet/GAIA/aspx/web/frmplantilla.aspx?tipus=W"" class=""btn btn-sm btn-primary"">Nova p&agrave;gina web</a>")			

                GAIA.debug(Nothing, "est=" & PLTDSEST)
                GAIA.debug(Nothing, "htm=" & PLTDSHTM)
                GAIA.debug(Nothing, strSql)
                GAIA.bdSR(objconn, "BEGIN TRANSACTION " & strSql & "; COMMIT TRANSACTION")


            Case "N"

                If Not idPlantilla Is Nothing Then  'Tengo el id, modifico el nodo.
                    txtCodiNode.Text = idPlantilla.ToString()
                Else ' No tengo id, inserto un nuevo nodo.
                    'Inserto el node web    '9 node web
                    txtCodiNode.Text = GAIA.insertarNode(objconn, 9, NWEDSTIT.Text.Replace("<p>", "").Replace("</p>", ""), Session("codiOrg"))
                    ' Creo la relación y los permisos. Coloco el nodo el el meu arbre personal, sense classificar.
                    GAIA.insertaNodeArbrePersonal(objconn, 9, txtCodiNode.Text, Session("codiOrg"), "")
                End If



                'per fer proves
                'txtCodiNode.text = 272518

                strSql &= "DELETE FROM METLNWE2 WHERE NWEINNOD=" & txtCodiNode.Text & " AND NWEINIDI=" & codiIdioma

                strSql &= ";INSERT INTO METLNWE2 (NWEINNOD, NWEINIDI,NWEDSTIT,NWEDSCAR,NWEDSUSR, NWEDSTCO, NWEDSPLA, NWEDSEST, NWEDSHTM, NWEDSLCW, NWEDSCSS, NWEDSEBO, NWEDSMET, NWEDSPEU, NWEDSCSP, NWEDSCSI, NWEDTPUB, NWEDTCAD) VALUES (" & txtCodiNode.Text & "," & codiIdioma & ",'" & NWEDSTIT.Text.Replace("'", "''").Replace("<p>", "").Replace("</p>", "") & "','" & NWEDSCAR.Text.ToString().Replace("'", "''") & "','" & Session("codiOrg") & "','" & PLTDSTCO & "','" & PLTDSPLT & "','" & PLTDSEST & "','" & PLTDSHTM & "','" & PLTDSLCW & "','" & PLTDSCSS & "','" & NWEDSEBO.Text.Replace("'", "''") & "','" & NWEDSMET.Text.Replace("'", "''") & "','" & NWEDSPEU.Text.Replace("'", "''") & "','" & NWEDSCSP.Text.Replace("'", "''") & "','" & NWEDSCSI.Text.Replace("'", "''") & "',getdate(), '1/1/1900')"


                'If Request("lstIdioma") = 1 Then
                strSql &= ";UPDATE METLNOD SET NODDSTXT='" & NWEDSTIT.Text.Replace("'", "''").Replace("<p>", "").Replace("</p>", "") & "' WHERE NODINNOD=" & txtCodiNode.Text
                'Si hi ha p&agrave;gines en altres idiomes, actualitzo tots els valors comuns

                'strsql &= ";UPDATE METLNWE2  SET NWEDSTIT='" & NWEDSTIT.text.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "',NWEDSCAR='" & NWEDSCAR.text.tostring().Replace("'", "''") & "', NWEDSTCO = '" & PLTDSTCO & "', NWEDSPLA='" & PLTDSPLT & "', NWEDSEST='" & PLTDSEST & "',NWEDSHTM=' " & PLTDSHTM & "', NWEDSLCW='" & PLTDSLCW & "', NWEDSCSS='" & PLTDSCSS & "',NWEDSEBO=" & NWEDSEBO.text.Replace("'", "''") & "', NWEDSMET= '" & NWEDSMET.text.Replace("'", "''") & "',NWEDSPEU='" & NWEDSPEU.text.Replace("'", "''") & "', NWEDSCSP='" & NWEDSCSP.text.Replace("'", "''") & "', NWEDSCSI='" & NWEDSCSI.text.Replace("'", "''") & "' WHERE NWEINIDI<>1 AND NWEINNOD=" & txtCodiNode.text
                'End If
                lblResultat.Text = "<div class='alert alert-dismissible alert-success mt-2 mb-2'><button type='button' class='close' data-dismiss='alert'>x</button>Node Web modificat amb èxit.<br/><br/><a href=""http://lhintranet/GAIA2/aspx/web/frmplantilla.aspx?tipus=N"">&nbsp;Nova p&agrave;gina web</a></div>"
                'GAIA2.escriuResultat(objConn, lblResultat, "Node Web modificat amb èxit.", "<a href=""http://lhintranet/GAIA2/aspx/web/frmplantilla.aspx?tipus=N"">&nbsp;Nova p&agrave;gina web</a>")

                GAIA.bdSR(objconn, "BEGIN TRANSACTION " & strSql & "; COMMIT TRANSACTION")

            Case "A"

                If Not idPlantilla Is Nothing Then
                    txtCodiNode.Text = idPlantilla.ToString()
                Else
                    'Inserto el node web    '8 arbre web
                    txtCodiNode.Text = GAIA.insertarNode(objconn, 8, AWEDSTIT.Text.Replace("<p>", "").Replace("</p>", ""), Session("codiOrg"))
                    'Creo una relació del node amb si mateix perque és el primer
                    Dim rel As New clsRelacio
                    ' La funcion creaRelacio, crea también los permisos
                    ' el segundo argumento "8" crea un arbre tipo arbre web 
                    rel = GAIA.creaRelacio(objconn, 8, txtCodiNode.Text, txtCodiNode.Text, 0, "", -1, 1, -1, 1, False, Session("codiOrg"))
                End If

                strSql &= "DELETE FROM METLAWE2 WHERE AWEINNOD=" & txtCodiNode.Text & " AND AWEINIDI=" & codiIdioma

                'strSql &= ";INSERT INTO METLAWE2  VALUES (" & txtCodiNode.Text & "," & codiIdioma & ",'" & AWEDSTIT.Text.Replace("'", "''").Replace("<p>", "").Replace("</p>", "") & "'," & lstAWEDSSER.SelectedItem.Value & ",'" & AWEDSROT.Text & "'," & Session("codiOrg") & ",'" & PLTDSTCO & "','" & PLTDSPLT & "','" & PLTDSEST & "','" & PLTDSHTM & "','" & PLTDSLCW & "','" & PLTDSLC2 & "','" & AWEDSDOC.Text & "','" & AWEDSEBO.Text & "','" & AWEDSMET.Text & "','" & AWEDSPEU.Text & "','" & AWEDSCSP.Text & "','" & AWEDSCSI.Text & "','" & AWEDSNOM.Text & "')"
                strSql &= "; INSERT INTO METLAWE2 (AWEINNOD, AWEINIDI, AWEDSTIT, AWEDSSER, AWEDSROT, AWEDSUSR, AWEDSTCO, AWEDSPLA, AWEDSEST, AWEDSHTM, AWEDSLCW, AWEDSLC2, AWEDSDOC, AWEDSEBO, AWEDSMET, AWEDSPEU, AWEDSCSP, AWEDSCSI, AWEDSNOM) VALUES (" & txtCodiNode.Text & "," & codiIdioma & ",'" & AWEDSTIT.Text.Replace("'", "''").Replace("<p>", "").Replace("</p>", "") & "'," & lstAWEDSSER.SelectedItem.Value & ",'" & AWEDSROT.Text & "'," & Session("codiOrg") & ",'" & PLTDSTCO & "','" & PLTDSPLT & "','" & PLTDSEST & "','" & PLTDSHTM & "','" & PLTDSLCW & "','" & PLTDSLC2 & "','" & AWEDSDOC.Text & "','" & AWEDSEBO.Text & "','" & AWEDSMET.Text & "','" & AWEDSPEU.Text & "','" & AWEDSCSP.Text & "','" & AWEDSCSI.Text & "','" & AWEDSNOM.Text & "')"


                'IF Request("lstIdioma")=1 THEN
                strSql &= ";UPDATE METLNOD SET NODDSTXT='" & AWEDSTIT.Text.Replace("'", "''").Replace("<p>", "").Replace("</p>", "") & "' WHERE NODINNOD=" & txtCodiNode.Text
                'Si hi ha p&agrave;gines en altres idiomes, actualitzo tots els valors comuns

                'strsql &= ";UPDATE METLAWE2  SET AWEDSTIT='"& AWEDSTIT.text.Replace("'","''").replace("<p>","").replace("</p>","") & "', AWEDSSER=" & lstAWEDSSER.SelectedItem.Value & ",AWEDSROT='" & AWEDSROT.text  & "', AWEDSTCO='" & PLTDSTCO & "', AWEDSPLA='" & PLTDSPLT & "', AWEDSEST='" & PLTDSEST & "',AWEDSHTM='" & PLTDSHTM & "',AWEDSLCW='" &  PLTDSLCW & "',AWEDSLC2='" & PLTDSLC2 & "',AWEDSDOC='" & AWEDSDOC.text & "',AWEDSMET='" & AWEDSEBO.text  & "',AWEDSMET='" & AWEDSMET.text  & "',AWEDSPEU='" & AWEDSPEU.text  & "',AWEDSCSP='" & AWEDSCSP.text  & "',AWEDSCSI'" & AWEDSCSI.text  &  "' WHERE AWEINIDI<>1 AND AWEINNOD=" & txtCodiNode.text			 
                'END IF
                lblResultat.Text = "<div class='alert alert-dismissible alert-success mt-2 mb-2'><button type='button' class='close' data-dismiss='alert'>x</button>Arbre Web modificat amb èxit.<br/><br/><a href=""http://lhintranet/GAIA/aspx/web/frmplantilla.aspx?tipus=A"" class=""txtRojo12Px"">&nbsp;Nou arbre web</a></div>"
                'GAIA.escriuResultat(objConn,lblResultat , "Arbre Web modificat amb èxit.","<a href=""http://lhintranet/GAIA/aspx/web/frmplantilla.aspx?tipus=A"" class=""txtRojo12Px"">&nbsp;Nou arbre web</a>")			

                'MAX: falta crear relació, permisos... 
                GAIA.bdSR(objconn, "BEGIN TRANSACTION " & strSql & "; COMMIT TRANSACTION")

            Case Else 'plantilla
                If Not idPlantilla Is Nothing Then
                    txtCodiNode.Text = idPlantilla.ToString()
                Else
                    'Inserto el node fulla web
                    txtCodiNode.Text = GAIA.insertarNode(objconn, 24, txtPLTDSTIT.Text.Replace("<p>", "").Replace("</p>", ""), Session("codiOrg"))
                    GAIA.insertaNodeArbrePersonal(objconn, 24, txtCodiNode.Text, Session("codiOrg"), "")
                End If

                'PLTDSALT= PLTDSALT.replace(",","|")		
                strSql = "DELETE FROM METLPLT2 WHERE PLTINNOD=" & txtCodiNode.Text
                strSql &= ";INSERT INTO METLPLT2 VALUES(" & txtCodiNode.Text & ",'" & txtPLTDSTIT.Text & "',getdate(), " & Session("codiOrg") & ",'" & PLTDSCMP & "','" & PLTDSEST & "','" & PLTDSCSS & "','" & PLTDSLNK & "','" & PLTDSIMG & "','" & PLTDSTCO & "','" & PLTDSLCW & "','" & PLTDSLC2 & "','" & PLTDSALK & "','" & PLTCDPAL & "','" & PLTDSAAL & "','" & PLTDSALT & "','" & PLTDSPLT & "','" & PLTDSFLW & "','" & txtPLTDSOBS.Text & "'," & PLTSWALT & ",'" & PLTDSNUM & "','" & PLTDSALF & "','" & PLTDSNIV & "'," & IIf(chkPLTSWVIS.Checked, 1, 0) & ",'" & PLTDSHTM & "')"

                strSql &= ";UPDATE METLNOD SET NODDSTXT='" & txtPLTDSTIT.Text.Replace("'", "''").Replace("<p>", "").Replace("</p>", "") & "' WHERE NODINNOD=" & txtCodiNode.Text


                lblResultat.Text = "<div class='alert alert-dismissible alert-success mt-2 mb-2'><button type='button' class='close' data-dismiss='alert'>x</button>Plantilla Web modificada amb èxit.<br/><br/><a href=""http://lhintranet/GAIA/aspx/web/frmplantilla.aspx?tipus=P"" class=""txtRojo12Px"">&nbsp;Nova plantilla</a></div>"
                'GAIA.escriuResultat(objConn,lblResultat , "Plantilla modificada amb èxit.","<a href=""http://lhintranet/GAIA/aspx/web/frmplantilla.aspx?tipus=P"" class=""btn btn-sm btn-primary"">Nova plantilla</a>")			

                GAIA.bdSR(objconn, "BEGIN TRANSACTION " & strSql & "; COMMIT TRANSACTION")
        End Select

        txtEstBD.Value = txtEstBD.Value.Replace("''", """")

        carregaCamps()
    End Sub

    Private Sub LeerAtributos(ByRef PLTDSCMP As String, ByRef PLTDSLNK As String, ByRef PLTDSALT As String, ByRef PLTDSCSS As String, ByRef PLTDSTCO As String, ByRef PLTDSLCA As String, ByRef PLTDSLCW As String, ByRef PLTDSLC2 As String, ByRef PLTDSIMG As String, ByRef PLTCDPAL As String, ByRef PLTDSAAL As String, ByRef PLTDSPLT As String, ByRef PLTDSALK As String, ByRef PLTDSFLW As String, ByRef PLTDSNUM As String, ByRef PLTDSALF As String, ByRef PLTDSNIV As String, ByRef WEBDSIMP As String, ByRef WEBDSCND As String, ByRef cssTmp As String, ByRef cont As Integer)
        Dim item As String()
        Dim arrCel As String()
        arrCel = txtAtributs.Value.Split("|")
        Array.Resize(arrCel, arrCel.Length - 1)
        'GAIA.debug(Nothing, "txtAtributs=" + txtAtributs.Value.ToString())
        For Each cel As String In arrCel
            If Not String.IsNullOrEmpty(cel) Then

                item = cel.Split("#")
                If (item.Length > 1) Then
                    PLTDSTCO &= item(7) & "," 'Tipus de fulla
                    PLTDSCMP &= item(8) & ","
                    PLTDSLNK &= item(9) & ","
                    PLTDSALT &= item(10) & "|"
                    PLTDSIMG &= IIf(item(11) = "", 0, item(11)) & ","
                    PLTDSFLW &= "0" & ","
                    PLTDSLCA &= item(13).Replace(",", "|") & ","
                    PLTDSLCW &= item(15).Replace(",", "|") & ","
                    PLTDSLC2 &= item(17).Replace(",", "|") & ","
                    PLTDSPLT &= item(19).Replace(",", "|") & ","
                    PLTDSNUM &= IIf(item(20) = "", 0, item(20)) & ","
                    PLTDSNIV &= item(21) & ","
                    PLTDSAAL &= item(22) & "|"
                    PLTDSALF &= item(23) & ","
                    PLTDSALK &= item(24) & "|"
                    PLTCDPAL &= item(26) & ","

                    'Camps d'atributs de cel·les de web
                    WEBDSIMP &= item(27) & ","
                    WEBDSCND &= item(28) & ","
                    'Estils
                    cssTmp = ""
                    For i As Integer = 29 To 29
                        If cssTmp.Length > 0 Then cssTmp &= ","
                        If item(i).Length > 0 Then cssTmp &= item(i)
                    Next i
                    PLTDSCSS &= cssTmp & "|"

                End If
            End If
        Next cel
        PLTDSLCA = PLTDSLCA.Substring(0, PLTDSLCA.Length - 1)
        PLTDSLCW = PLTDSLCW.Substring(0, PLTDSLCW.Length - 1)
        PLTDSLC2 = PLTDSLC2.Substring(0, PLTDSLC2.Length - 1)
        PLTDSCSS = PLTDSCSS.Substring(0, PLTDSCSS.Length - 1)
        WEBDSIMP = WEBDSIMP.Substring(0, WEBDSIMP.Length - 1)
        WEBDSCND = WEBDSCND.Substring(0, WEBDSCND.Length - 1)
        PLTCDPAL = PLTCDPAL.Substring(0, PLTCDPAL.Length - 1)
        PLTDSALK = PLTDSALK.Substring(0, PLTDSALK.Length - 1)
        PLTDSALF = PLTDSALF.Substring(0, PLTDSALF.Length - 1)
        PLTDSTCO = PLTDSTCO.Substring(0, PLTDSTCO.Length - 1)
        PLTDSCMP = PLTDSCMP.Substring(0, PLTDSCMP.Length - 1)
        PLTDSALT = PLTDSALT.Substring(0, PLTDSALT.Length - 1)
        PLTDSIMG = PLTDSIMG.Substring(0, PLTDSIMG.Length - 1)
        PLTDSLNK = PLTDSLNK.Substring(0, PLTDSLNK.Length - 1)
        PLTDSFLW = PLTDSFLW.Substring(0, PLTDSFLW.Length - 1)
        PLTDSPLT = PLTDSPLT.Substring(0, PLTDSPLT.Length - 1)
        PLTDSNUM = PLTDSNUM.Substring(0, PLTDSNUM.Length - 1)
        PLTDSNIV = PLTDSNIV.Substring(0, PLTDSNIV.Length - 1)
        PLTDSAAL = PLTDSAAL.Substring(0, PLTDSAAL.Length - 1)

    End Sub

    Protected Sub inicialitzaLListaServidors()

        Dim DS As DataSet
        DS = New DataSet()
        GAIA.bdr(objconn, "SELECT SERINCOD, SERDSURL FROM METLSER WITH (NOLOCK)", DS)
        lstAWEDSSER.DataSource = DS.Tables(0).DefaultView
        lstAWEDSSER.DataTextField = "SERDSURL"
        lstAWEDSSER.DataValueField = "SERINCOD"
        lstAWEDSSER.DataBind()
        lstAWEDSSER.Items.Insert(0, New ListItem("Selecciona un servidor", "0"))
        DS.Dispose()
    End Sub 'inicialitzaLListaServidors

    Protected Sub canviIdioma(sender As Object, e As EventArgs)
        Dim codiIdioma As Integer
        codiIdioma = lstCanviIdioma.SelectedItem.Value

        carregaCamps(codiIdioma)

    End Sub 'canviIdioma

    'Protected Sub lstTipusFulla_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    Dim DS2 As New DataSet
    '    Dim valor As Integer = lstTipusFulla.SelectedItem.Value
    '    GAIA.bdr(objconn, "SELECT TBLDSTXT FROM (SELECT TIPINTIP,TIPDSDES,TBLDSTXT FROM  METLTIP WITH(NOLOCK), METLTBL WITH(NOLOCK) WHERE TIPINTIP=TBLINTFU) AS TMP WHERE TIPINTIP =" & valor, DS2)
    '    carregaNomCamps(lstTipusFulla.SelectedIndex.ToString(), DS2.Tables(0).Rows(0).Item(0))

    'End Sub
End Class