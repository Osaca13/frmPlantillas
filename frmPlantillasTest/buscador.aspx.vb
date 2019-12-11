Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Xml

Public Class buscador
    Inherits System.Web.UI.Page

    Public objconn As OleDbConnection

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'f_inicialitzar()
        If Not Page.IsPostBack Then
            carrega_idioma()
        End If

    End Sub

    'procediment per carregar el desplegable d'idioma
    Private Sub carrega_idioma()
        Dim ds As New DataSet, qSQL As String
        qSQL = "select IDICDIDI,IDIDSNOM from METLIDI"


        GAIA.bdr(objconn, qSQL, ds)
        ddlb_idioma.DataSource = ds
        ddlb_idioma.DataTextField = "IDIDSNOM"
        ddlb_idioma.DataValueField = "IDICDIDI"
        ddlb_idioma.DataBind()

        'ddlb_idioma.Items.Insert(0, new ListItem("Tots",-1))
        ds.Dispose()
    End Sub

    '***************************************************************************************
    'procediment encarregat de carregar el grid segons els filtres especificats
    '**************************************************************************************
    Private Sub carregarGrid(ByVal sOrder As String, ByVal tipusSortida As String)
        carregarGrid(sOrder, tipusSortida, "")
    End Sub

    Private Sub carregarGrid(ByVal sOrder As String, ByVal tipusSortida As String, ByVal paramInicial As String)
        Dim sSQL As String, bCercarContingut As Boolean, sLlistaIS As String
        Dim sSQLOrder As String = ""
        Dim rel As New clsRelacio
        Dim paramUbicacio As String = String.Empty
        'CERCA DINS EL CONTINGUT DELS FITXERS (INDEX SERVER)
        bCercarContingut = f_cercar_contingut()
        If bCercarContingut Then
            sLlistaIS = f_cerca_IS()
        End If


        Dim selectSIT As String = ""

        If CStr(ddlb_estat.SelectedValue) = "0" Then
            selectSIT = " AND RELCDSIT IN (1,2,3) "
        Else
            selectSIT = " AND RELCDSIT=" + CStr(ddlb_estat.SelectedValue)
        End If
        'CERCA PER IDIOMA (TAULA METLDOC)
        sSQL = "SELECT DOCINNOD,DOCDSFIT,substring(DOCDSTIT,1,100) as NOM,DOCWNSIZ / 1024.0 as MIDA,DOCDTANY as CREACIO, RELINCOD, TDODSIMG, cast(RELINCOD as varchar) as Relacions " &
        "FROM METLDOC LEFT JOIN METLREL ON METLREL.RELINFIL = METLDOC.DOCINNOD " & selectSIT &
        " LEFT JOIN METLTDO ON TDOCDTDO=DOCINTDO WHERE DOCINIDI = " & ddlb_idioma.SelectedValue

        'Cerca per data de creació
        If Trim(dataIni.Text <> "") Then
            sSQL &= " AND DOCDTANY >= '" & dataIni.Text & "'"
        End If
        If Trim(dataFi.Text <> "") Then
            sSQL &= " AND DOCDTANY <= '" & dataFi.Text & "'"
        End If

        'cerca per títol de document
        If Trim(titol.Text) <> "" Then
            sSQL = sSQL & " AND UPPER(CAST(DOCDSTIT AS VARCHAR(8000))) LIKE '%" & UCase(Replace(Trim(titol.Text), "'", "''")) & "%'"
        End If
        If Request("codiRelacioInicial") = Nothing Then
            Dim oCodParam As New lhCodParam.lhCodParam
            paramUbicacio = oCodParam.queryString(Request.QueryString(0), "codiRelacioInicial")
            oCodParam = Nothing
        Else
            paramUbicacio = Request.QueryString("codiRelacioInicial")
        End If

        If paramUbicacio.Length > 0 Then
            rel.bdget(objconn, paramUbicacio)
            sSQL = sSQL + " AND METLREL.RELCDHER like '" + rel.cdher + "_" + rel.infil.ToString() + "%' "
        End If




        'response.write(sLlistaIS.length)



        If Trim(sOrder) <> "" Then
            sSQLOrder = " ORDER BY " & sOrder
        End If




        'si s'ha cercat pel contingut, afegir la restricció
        If bCercarContingut Then
            Dim strTMP As String = ""
            Dim sSQLTmp As String = ""
            Dim pos As Integer = -9
            'response.write("<br>" & sllistaIS & "<br>")
            strTMP = ""
            While sLlistaIS.Length > 5000
                pos = 0
                If strTMP.Length > 0 Then
                    sSQLTmp &= " | "
                End If
                strTMP = sLlistaIS.Substring(0, 5000)
                pos = InStrRev(strTMP, ",")

                strTMP = sLlistaIS.Substring(0, pos - 1)
                sLlistaIS = sLlistaIS.Substring(pos)
                'Response.write("<br>" & strTMP & "<br>")	

                sSQLTmp &= sSQL & " AND CAST(DOCDSFIT AS VARCHAR(8000)) IN (" & strTMP & ")" & sSQLOrder & " "
            End While


            If strTMP.Length > 0 Then
                sSQLTmp &= " | " & sSQL & " AND CAST(DOCDSFIT AS VARCHAR(8000)) IN (" & sLlistaIS & ")" & sSQLOrder & " "
                sSQL = sSQLTmp
            End If
            'No ha entrat al bucle, faig la sql normal
            If pos = -9 Then
                sSQL = sSQL & " AND CAST(DOCDSFIT AS VARCHAR(8000)) IN (" & sLlistaIS & ")" & sSQLOrder
            End If


        Else
            sSQL = sSQL & sSQLOrder
        End If

        '		response.write(ssql.length)

        'response.write(ssql)
        'f_filtrar_i_mostrar(sSQL, "DOCINNOD", tipusSortida)
    End Sub

    'cerca en el contingut dels fitxers utilitzant l'index server.
    Private Function f_cerca_IS() As String
        Dim ds As New DataSet("resultats"), da As New OleDbDataAdapter, i As Integer, sLlistaIS As String
        Dim s_query As String, s_query_algunes As String, s_query_totes As String, s_query_sense As String, s_query_tipus As String
        Dim Q As Object = Server.CreateObject("ixsso.Query")
        Dim Util As Object = Server.CreateObject("ixsso.Util")
        'consulta a l'índex server
        'ordenació per rellevància (descendent)
        Q.SortBy = "rank[d]"
        'camp a recollir
        Q.Columns = "DocTitle, vpath, path, filename, size, write, characterization"
        'Catàleg, identificat pel path físic on es troba
        Q.Catalog = "c:\IndexServer\WEB06"
        'Directori on buscar
        Util.AddScopeToQuery(Q, "e:\docs\GAIA\", "deep")
        'A la variable s_query anirem montant la consulta que enviarem a l'Index Server
        's_query = "(#filename *.shtm OR #filename *.htm OR #filename *.html OR #filename *.asp OR #filename *.pdf OR #filename *.xls OR #filename *.doc OR #filename *.ppt OR #filename *.pps)"
        s_query_tipus = "NOT (#filename *.shtm OR #filename *.shtml OR #filename *.htm OR #filename *.html OR #filename *.asp OR #filename *.aspx)"
        If Trim(paraules_totes.Text) <> "" Then
            s_query = "@contents " & Replace(Trim(paraules_totes.Text), " ", "* and @contents ") & "* AND "
        End If
        If Trim(paraules_alguna.Text) <> "" Then
            s_query = s_query & "(@contents %" & Replace(Trim(paraules_alguna.Text), " ", "% OR @contents %") & "%) AND "
        End If
        If Trim(paraules_sense.Text) <> "" Then
            If s_query = "" Then s_query = " (@size > 0) AND "
            s_query = s_query & "NOT @contents " & Replace(Trim(paraules_sense.Text), " ", "* AND NOT @contents ") & "* AND "
        End If
        s_query = s_query & s_query_tipus
        'response.write(s_query)
        Q.Query = s_query
        'L'objecte ixsso.Query retorna un recordset. A través de la instrucció següent
        'aconseguim obtenir-lo i carregar-lo en un dataset, que ens servirà per carregar 
        'posteriorment el datagrid
        da.Fill(ds, Q.CreateRecordset("nonsequential"), "resultats")
        For i = 0 To ds.Tables(0).Rows.Count - 1
            If sLlistaIS <> "" Then sLlistaIS = sLlistaIS & ","
            sLlistaIS = sLlistaIS & "'" & ds.Tables(0).Rows(i).Item("FILENAME") & "'"
        Next i
        If Trim(sLlistaIS) = "" Then sLlistaIS = "'NOMATCH'"
        Return sLlistaIS
    End Function

    'funció que en indica si cal buscar en el contingut del fitxer, depenent dels filtres entrats per l'usuari.
    Private Function f_cercar_contingut() As Boolean
        If Trim(paraules_totes.Text) <> "" Or Trim(paraules_alguna.Text) <> "" Or Trim(paraules_sense.Text) <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

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


End Class