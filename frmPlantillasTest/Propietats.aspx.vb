Imports System.Data
Imports System.Data.OleDb


Public Class Propietats
    Inherits System.Web.UI.Page

    '**********************************************************************
    '**********************************************************************
    '			F R M P R O P I E T A T S  (GAIA)
    '**********************************************************************
    '**********************************************************************

    Dim session2 As String
    Dim nif As String
    Public Shared objconn As OleDbConnection

    Private Sub Page_UnLoad(sender As Object, e As System.EventArgs) Handles MyBase.Unload
        GAIA.bdFi(objconn)
    End Sub 'Page_UnLoad

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load

        objconn = GAIA.bdIni()
        'If HttpContext.Current.User.Identity.Name.Length > 0 Then
        '    If (Session("nif") Is Nothing) Then
        '        Session("nif") = GAIA.nifUsuari(objconn, HttpContext.Current.User.Identity.Name).Trim()
        '    End If
        '    If Session("codiOrg") Is Nothing Then
        '        Session("CodiOrg") = GAIA.trobaNodeUsuari(objconn, Session("nif")).ToString().Trim()
        '    End If
        'End If
        'nif = Session("nif").Trim()



        session2 = "346231"
        If Not Page.IsPostBack Then
            dataIni.Text = DateAdd(DateInterval.Day, -30, Now).ToString("dd/MM/yyyy")
            dataFi.Text = Now.ToString("dd/MM/yyyy")


            carregaPropietats(Request("RELINCOD"))
            carregaUbicacions()


            botonsAccions(Request("RELINCOD"), session2)

        End If


    End Sub 'Page_Load

    Public Sub btnCanviarPeriode_Click()
        calcularVisites(Request("RELINCOD"))

    End Sub



    Protected Sub botonsAccions(ByVal codiRelacio As Integer, ByVal usuari As Integer)
        Dim rel As New clsRelacio
        rel.bdget(Nothing, codiRelacio)

        If clsPermisos.tepermis(Nothing, 3, session2, session2, rel, 0, "", "", 0) Then
            lblEditar.Text = "<a href=""" & GAIA.editarContingut(Nothing, rel.infil, 1, rel.incod, rel.incod, rel.incod, 0) & """ target=""_blank"">Editar</a>"
        End If

    End Sub

    Protected Sub carregaPropietats(ByVal codiRelacio As String)
        Dim prelacions As Integer = Request("RELINCOD")
        Dim DS As DataSet
        Dim DS2 As DataSet
        Dim dbRow As DataRow
        Dim strsql As String = ""
        Dim cont As Integer
        Dim cont2 As Integer = 0
        Dim strOrderBy As String
        If codiRelacio > 0 Then
            DS = New DataSet()
            DS2 = New DataSet()
            GAIA.bdr(objconn, "SELECT  RELCDSIT,RELINCOD,node.NODINNOD,node.NODDSTXT as nomNode,FORDSTIT,TIPDSDES,node.NODDTTIM,RELCDVER,RELCDORD,  TIPINTIP FROM METLREL WITH(NOLOCK), METLNOD as node  WITH(NOLOCK), METLFOR   WITH(NOLOCK),METLTIP  WITH(NOLOCK) WHERE RELINFIL=node.NODINNOD AND RELINCOD=" & codiRelacio & " AND node.NODCDTIP=TIPINTIP AND RELCDSIT<>99 AND (node.NODCDUSR=FORINNOD OR node.NODCDUSR like '9999')", DS)

            If DS.Tables(0).Rows.Count > 0 Then
                dbRow = DS.Tables(0).Rows(0)
                lblNode.Text = HttpUtility.HtmlDecode(dbRow("nomNode").Trim())
                lblTipusNode.Text = dbRow("TIPDSDES").Trim()
                lblUsr.Text = dbRow("FORDSTIT").Trim()
                lblTim.Text = dbRow("NODDTTIM").ToString()

                lblCodi.Text = dbRow("NODINNOD").ToString()
                lblRelacio.Text = dbRow("RELINCOD").ToString()
                lblOrdre.Text = dbRow("RELCDORD")
                Select Case dbRow("RELCDSIT")
                    Case 1
                        lblSituacio.Text = "Inicial"
                    Case 2
                        lblSituacio.Text = "Pendent de publicar"
                    Case 3
                        lblSituacio.Text = "Publicat"
                    Case 98
                        lblSituacio.Text = "Caducat"
                    Case 99
                        lblSituacio.Text = "Esborrat"
                End Select


                Select Case dbRow("TIPINTIP")
                    Case 40 'contractació		    
                        PanelHistorico.Visible = True
                        Dim oCodParam As New lhCodParam.lhCodParam
                        ctrlHistoricoCatala.NavigateUrl = "http://intranet/GAIA/aspx/contractacio/ContractesHistorico.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codigo=" & dbRow("NODINNOD") & "&idioma=1&codigosup=" & dbRow("RELINCOD")))
                        ctrlHistoricoCastella.NavigateUrl = "http://intranet/GAIA/aspx/contractacio/ContractesHistorico.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codigo=" & dbRow("NODINNOD") & "&idioma=2&codigosup=" & dbRow("RELINCOD")))
                    Case 51 'tràmits
                        PanelHistorico.Visible = True
                        Dim oCodParam As New lhCodParam.lhCodParam
                        ctrlHistoricoCatala.NavigateUrl = "/GAIA/aspx/propietats/historic.aspx?" & HttpUtility.UrlEncode(oCodParam.encriptar("codigo=" & dbRow("NODINNOD") & "&idioma=1"))
                        ctrlHistoricoCastella.NavigateUrl = "/GAIA/aspx/propietats/historic.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codigo=" & dbRow("NODINNOD") & "&idioma=2"))

                    Case Else
                        'no poso històric
                End Select
                If dbRow("TIPINTIP") = 45 Or dbRow("TIPINTIP") = 4 Or dbRow("TIPINTIP") = 51 Then

                    calcularVisites(Request("RELINCOD"))

                End If
            End If



            Try

                Dim rel As New clsRelacio
                Dim strResultat As String = ""

                rel.bdget(objconn, codiRelacio)
                GAIA.obtenirPaginesAfectadesPerCanvi(objconn, rel, rel.infil, 1, Now, Now, rel, 1, strResultat, 0, Nothing, False)


                Dim llistaPaginesNodes As String = ""
                Dim llistaPaginesRelacions As String = ""
                For Each grupCanvis As String In strResultat.Split(",")
                    Dim arrCanvis As String() = grupCanvis.Split("|")

                    Try
                        If Not String.IsNullOrEmpty(grupCanvis) Then

                            If (arrCanvis(0) = "0") Then 'tinc un node
                                If arrCanvis(1) <> rel.infil Then clsPermisos.afegirElement(arrCanvis(1), llistaPaginesNodes)
                            Else
                                If arrCanvis(0) <> rel.incod Then clsPermisos.afegirElement(arrCanvis(0), llistaPaginesRelacions)
                            End If
                        End If
                    Catch
                    End Try
                Next grupCanvis

                strResultat = ""
                strsql = ""
                If Not String.IsNullOrEmpty(llistaPaginesRelacions) Then
                    strsql &= " RELINCOD  in (" & llistaPaginesRelacions & ") "
                End If
                If Not String.IsNullOrEmpty(llistaPaginesNodes) Then
                    If Not String.IsNullOrEmpty(strsql) Then strsql &= " OR "
                    strsql &= "  RELINFIL IN (" & llistaPaginesNodes & ")"
                End If
                cont2 = 0
                GAIA.bdr(objconn, "select TIPDSIMG, RELCDHER, RELINFIL FROM METLNOD WITH(NOLOCK) ,METLREL WITH(NOLOCK) , METLTIP WITH(NOLOCK) WHERE (" & strsql & ") AND NODINNOD=RELINFIL AND RELCDSIT<98 AND NOT RELCDHER LIKE '_5286%' AND NODCDTIP=TIPINTIP AND NODCDTIP<>9", DS)
                For Each dbRow In DS.Tables(0).Rows
                    If String.IsNullOrEmpty(dbRow("RELCDHER")) Then
                        strsql = dbRow("RELINFIL")

                    Else
                        strsql = dbRow("RELCDHER").substring(1).replace("_", ",") & "," & dbRow("RELINFIL")
                        cont = 0
                        strOrderBy = ""
                        For Each item As Integer In (dbRow("RELCDHER").substring(1) & "_" & dbRow("RELINFIL")).Split("_")
                            strOrderBy &= " WHEN " & item & " THEN " & cont
                            cont += 1
                        Next item
                        strOrderBy = " ORDER by CASE NODINNOD " & strOrderBy & " END"

                    End If



                    GAIA.bdr(objconn, "SELECT NODDSTXT, NODINNOD FROM METLNOD WITH(NOLOCK) WHERE NODINNOD IN (" & strsql & ")  " & strOrderBy, DS2)
                    cont = 0

                    For Each dbrow2 As DataRow In DS2.Tables(0).Rows
                        If cont = 0 Then
                            strResultat &= "<li class=""" & IIf(cont2 Mod 2 = 0, "color1", "color2") & """><img src=""http://www.l-h.cat/img/" & dbRow("TIPDSIMG") & """ class="""" hspace=""5"" align=""absmiddle""/>"
                        Else
                            strResultat &= " > "
                        End If
                        cont = cont + 1
                        strResultat &= dbrow2("NODDSTXT")
                    Next dbrow2
                    strResultat &= "</li>"
                    cont2 = cont2 + 1
                Next dbRow
                strResultat = "<ul>" & strResultat & "</ul>"

                lblPaginesUs.Text = HttpUtility.HtmlDecode(strResultat)
                PanelUsPagines.Visible = True
            Catch

            End Try

            DS.Dispose()
            DS2.Dispose()
        End If

    End Sub 'carregaPropietats

    'carrega en el datagrid les ubicacions de les relacions passades per paràmetre
    Protected Sub carregaUbicacions()
        Dim ptipus As String = ""
        Dim prelacions As Integer = Request("RELINCOD")
        Dim codiant As String = ""
        Dim ds As DataSet, ds2 As DataSet, i As Integer, j As Integer, nomArbre As String
        Dim dbRow As DataRow, sCami As String, aCami As String(), sNodText As String, aResultats As String()
        ds = New DataSet()
        ds2 = New DataSet()
        If InStr(prelacions, ",") <= 0 Then
            GAIA.bdr(objconn, "SELECT  DISTINCT RELCDHER, RELINCOD, RELINFIL FROM METLREL WITH(NOLOCK) WHERE RELCDSIT<98 AND RELINFIL IN (SELECT RELINFIL FROM METLREL WITH(NOLOCK) WHERE RELCDSIT<98  AND RELINCOD IN (" & prelacions & "))   AND NOT (RELCDHER LIKE '_201940%' OR RELCDHER LIKE '_5286%') ORDER BY RELINCOD  ", ds)


            'GAIA.bdR(objconn, "SELECT RELCDHER,RELINCOD,RELINFIL FROM METLREL WHERE RELINCOD IN (" & pRelacions & ")", DS)
        Else
            'obtenim les ubicacions de les diferents relacions a través del camp RELCDHER, en un string en format (_ubicacio)*
            GAIA.bdr(objconn, "SELECT DISTINCT RELCDHER,RELINCOD,RELINFIL FROM METLREL WITH(NOLOCK)  WHERE RELCDSIT<98 AND RELINCOD IN (" & prelacions & ") AND NOT (RELCDHER LIKE '_201940%' OR RELCDHER LIKE '_5286%') ORDER BY RELINCOD", ds)
        End If

        ReDim aResultats(ds.Tables(0).Rows.Count - 1)
        Dim cont As Integer = 0
        For i = 0 To ds.Tables(0).Rows.Count - 1
            dbRow = ds.Tables(0).Rows(i)
            If dbRow("RELCDHER") <> codiant Then
                codiant = dbRow("RELCDHER")
                sCami = dbRow("RELCDHER")
                'dividim l'string i ho posem en un array per poder-ho tractar
                aCami = Split(sCami, "_")
                If ptipus = "N" Then
                    aResultats(cont) = "<a target=""_blank"" href=""/gdocs/aspx/noticia.aspx?cr=" & dbRow("RELINCOD") & "&pl=56676&id=1&us=0"" class=""color1"">"
                Else
                    nomArbre = f_obtenirNomArbre(dbRow("RELINCOD"))
                    aResultats(cont) = "&nbsp;&nbsp; <a target=""_blank"" href=""VisorArbres.aspx?arbre1=" & nomArbre & "&nodesSeleccionats=" & dbRow("RELINFIL") & """ class=""color1"">"
                End If
                'per cada ubicació anem a buscar la seva descripció
                For j = 1 To UBound(aCami)
                    GAIA.bdr(objconn, "SELECT NODDSTXT FROM METLNOD WITH(NOLOCK) WHERE NODINNOD = " & aCami(j), ds2)
                    dbRow = ds2.Tables(0).Rows(0)
                    If j > 1 Then aResultats(cont) &= " > "
                    aResultats(cont) &= dbRow("NODDSTXT")

                Next j
                If ptipus = "N" Then
                    aResultats(cont) &= "</a>"
                Else
                    aResultats(cont) &= "</a>"
                End If
                cont = cont + 1
            End If
        Next i

        Dim sortida As String = ""
        cont = 0
        For Each item As String In aResultats
            sortida &= "<li  class=""" & IIf(cont Mod 2 = 0, "color1", "color2") & """>" & item & "</li>"
            cont = cont + 1

        Next item
        sortida = "<ul>" & sortida & "</ul>"
        lblUbicacions.Text = sortida
        'carreguem el vector de descripcions en un datagrid per tal de mostrar-ho per pantalla.
        'dgResultats.dataSource = aResultats
        'dgResultats.databind()
        ds.Dispose()
        ds2.Dispose()
    End Sub 'carregaUbicacions

    Private Function f_obtenirNomArbre(ByVal codiRelacio As Integer) As String
        Dim ds As New DataSet, nomArbre As String, sHer As String, nodeArbre As String
        GAIA.bdr(objconn, "SELECT RELCDHER FROM METLREL WITH(NOLOCK) WHERE RELCDSIT<98 AND RELINCOD = " & codiRelacio, ds)
        If ds.Tables(0).Rows.Count > 0 Then

            sHer = ds.Tables(0).Rows(0).Item("RELCDHER")
            If sHer.Length > 0 Then
                nodeArbre = sHer.Substring(1).Split("_")(0)

                ds = New DataSet
                GAIA.bdr(objconn, "SELECT NODDSTXT FROM METLNOD WITH(NOLOCK) WHERE NODINNOD = " & nodeArbre, ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    nomArbre = ds.Tables(0).Rows(0).Item("NODDSTXT")
                    nomArbre = Trim(Replace(nomArbre, "arbre ", ""))
                End If

            End If
        End If
        ds.Dispose()
        Return nomArbre
    End Function


    Protected Sub calcularVisites(ByVal codiRelacio As Integer)
        'gaia.bdr(objconn, "SELECT count(*) AS total FROM METLHIT WITH(NOLOCK) WHERE HITCDREL IN (select rel1.RELINCOD FROM METLREL as rel1 WITH(NOLOCK), METLREL as rel2 WITH(NOLOCK) WHERE rel2.RELINCOD=" & codiRelacio & " AND rel2.RELCDSIT<99 AND rel2.RELINFIL=rel1.RELINFIL AND rel1.RELCDSIT<99 ) AND DATEDIFF(dd,HITDTDAT, GETDATE())<=30 AND HITWNROB=0 ", DS)

        Dim DS As DataSet
        DS = New DataSet()
        GAIA.bdr(objconn, "SELECT count(*) AS total FROM METLHIT WITH(NOLOCK) WHERE HITCDREL IN (select rel1.RELINCOD FROM METLREL as rel1 WITH(NOLOCK), METLREL as rel2 WITH(NOLOCK) WHERE rel2.RELINCOD=" & codiRelacio & " AND rel2.RELCDSIT<99 AND rel2.RELINFIL=rel1.RELINFIL AND rel1.RELCDSIT<99 ) AND DATEDIFF(mi,'" & dataIni.Text & "',HITDTDAT )>=0 AND DATEDIFF(mi,HITDTDAT,  '" & dataFi.Text & "')>=0 AND HITWNROB=0 ", DS)


        If DS.Tables(0).Rows.Count > 0 Then
            lblVisites.Text = DS.Tables(0).Rows(0)("total")
        Else
            lblVisites.Text = 0
        End If
        pnlVisites.Visible = True
        DS.Dispose()
    End Sub




End Class