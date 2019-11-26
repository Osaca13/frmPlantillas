﻿Imports System.Xml
Imports System.Data.OleDb
Imports Telerik.WebControls
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports KCommon.Net.FTP
Imports System.Drawing.Imaging
Imports System.DirectoryServices


'Necessari per la funció DameLonLatDeCoordenadals

Imports System.Convert
Imports System.Drawing
Imports SharpMap.CoordinateSystems
Imports SharpMap.CoordinateSystems.Transformations
Imports System.Data


'******************************************************************************************+
'******************************************************************************************+
'******************************************************************************************+
'******************************************************************************************+
'
'				C L A S S    P E R M I S 
'
''*****************************************************************************************+
'******************************************************************************************+
'******************************************************************************************+
'******************************************************************************************+
Public MustInherit Class clsPermisos
    Inherits System.Web.UI.Page
    Protected WithEvents RadTree1 As RadTreeView
    Protected lblNode As Label
    Protected cerca As TextBox
    Protected lblPersonesAmbPermis As Label
    Protected lblpersones As Label
    Protected lblpermisos, lblpermisosHerencia As Label
    Protected persona() As HtmlAnchor
    Protected btnClickLlistaAmpliada As Button

    Public Shared nropersones As Integer
    Public Shared strValor As String
    Public Shared nif As String
    Public Shared idUsuari As Integer

    Public Shared hOrganigrama As Dictionary(Of Integer, String)

    'Carrego el organigrama en una taula de hash on la clau són tots els nodes organigrama i el valor són els nodes i fulles organigrama inclosos


    Public Shared Sub carregaOrganigrama()
        Dim objconn As OleDbConnection
        objconn = GAIA2.bdIni()
        Dim ds As New DataSet()
        Dim dbrow As DataRow

        clsPermisos.hOrganigrama = New Dictionary(Of Integer, String)
        Dim codiAnt As Integer = 0
        Dim llistaNodes As String = ""
        GAIA2.bdr(objconn, "select DISTINCT nodPare.NODINNOD, relPare.RELINCOD FROM METLNOD as nodPare with (nolock), METLREL as relPare with (nolock) WHERE relPare.RELCDHER LIKE '_56261_115969%' AND relPare.RELINFIL=nodPare.NODINNOD AND relPare.RELCDSIT<98 AND nodPare.NODCDTIP=12 AND (select count(relFill.RELINCOD) FROM METLREL as relFill with (nolock), METLNOD AS nodFill with (nolock) WHERE relFill.RELCDRSU=relPare.RELINCOD AND relFill.RELCDHER LIKE '_56261_115969%' AND relFill.RELCDSIT<98 AND nodFill.NODCDTIP=12 AND relFill.RELINFIL=nodFill.NODINNOD ) >0 ", ds)
        For Each dbrow In ds.Tables(0).Rows
            llistaNodes = clsPermisos.carregaOrganigramaFills(objconn, dbrow("RELINCOD"))
            If llistaNodes.Length > 0 Then
                Try
                    clsPermisos.hOrganigrama.Add(dbrow("NODINNOD"), llistaNodes)
                Catch
                    'ja existeix el node organigrama i no l'he d'afegir
                End Try

            End If
            llistaNodes = ""
        Next
        ds.Dispose()
    End Sub

    Public Shared Function carregaOrganigramaFills(ByVal objConn As OleDbConnection, ByVal relPare As Integer) As String
        Dim llistaNodes As String = ""
        Dim llistaNodesFills As String = ""
        objConn = GAIA2.bdIni()
        Dim ds As New DataSet()
        Dim dbrow As DataRow
        GAIA2.bdr(objConn, "select DISTINCT NODINNOD, RELINCOD FROM METLNOD WITH(NOLOCK), METLREL WITH(NOLOCK) WHERE RELCDRSU=" & relPare & "  AND RELINFIL=NODINNOD AND RELCDSIT<98 AND NODCDTIP=12", ds)
        For Each dbrow In ds.Tables(0).Rows

            'Poso el node que he trobat
            If llistaNodes.Length > 0 Then
                llistaNodes &= ","
            End If
            llistaNodes &= dbrow("NODINNOD")

            'Poso els nodes fills
            llistaNodesFills = clsPermisos.carregaOrganigramaFills(objConn, dbrow("RELINCOD"))
            If llistaNodesFills.Length > 0 And llistaNodes.Length > 0 Then
                llistaNodes &= ","
            End If
            llistaNodes &= llistaNodesFills
            llistaNodesFills = ""
        Next

        ds.Dispose()

        Return (llistaNodes)
    End Function



    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Dim objconn As OleDbConnection
        objconn = GAIA2.bdIni()
        Dim rel As New clsRelacio
        Dim relbuida As New clsRelacio

        'If HttpContext.Current.User.Identity.Name.Length > 0 Then
        '    If (Session("nif") Is Nothing) Then
        '        Session("nif") = GAIA2.nifUsuari(objconn, HttpContext.Current.User.Identity.Name).Trim()
        '    End If
        '    If Session("codiOrg") Is Nothing Then
        '        Session("CodiOrg") = GAIA2.trobaNodeUsuari(objconn, Session("nif")).ToString().Trim()
        '    End If
        'End If
        idUsuari = 346231
        'Session("CodiOrg")
        nropersones = 0
        ' Carrego la llista de tipus de nodes
        lblNode.Text = GAIA2.descNode(objconn, Request("nodes"))
        If Not Page.IsPostBack Then
            lblpersones.Text = ""
            lblpermisos.Text = ""
            lblpermisosHerencia.Text = ""
            lblPersonesAmbPermis.Text = ""
            rel.bdget(objconn, Request("relincod"))

            carregaPersonesAmbPermis(objconn, rel, 0, lblPersonesAmbPermis.Text, lblpersones.Text, lblpermisos.Text, lblpermisosHerencia.Text, -1)
            GAIA2.generaArbre(objconn, RadTree1, 9, idUsuari, relbuida, Request("arbre1"), 0, 3)
        End If
        GAIA2.bdFi(objconn)
    End Sub 'Page_Load


    Private Sub generaArbre(ByVal objConn As OleDbConnection)
        Dim DS As DataSet
        Dim node As RadTreeNode
        Dim dbRow As DataRow
        Dim rel As New clsRelacio
        DS = New DataSet()
        RadTree1.RetainScrollPosition = False

        GAIA2.bdr(objConn, "SELECT NODDSTXT,NODINNOD,NODCDTIP,TIPDSDES,RELINCOD, RELCDSIT FROM METLTIP WITH(NOLOCK), METLNOD WITH(NOLOCK),METLREL WITH(NOLOCK) WHERE TIPCDVER > 0 AND NODCDTIP = TIPINTIP AND NODDSTXT LIKE 'arbre " & Request("arbre1").Trim & "%' AND RELINFIL=NODINNOD AND RELINPAR=NODINNOD AND RELCDSIT<98 ORDER BY NODINNOD DESC", DS)
        For Each dbRow In DS.Tables(0).Rows
            rel.bdget(objConn, dbRow("RELINCOD"))

            node = GAIA2.creaNodePantalla(objConn, dbRow("NODDSTXT"), dbRow("NODINNOD").ToString() & "-" & dbRow("NODCDTIP").ToString() & "_" & dbRow("RELINCOD"), "arbre", "", 0, idUsuari, rel, 0, 0)

            RadTree1.AddNode(node)
            RadTree1.AutoPostBack = False
        Next dbRow
        DS.Dispose()
    End Sub


    Private Sub RadTree1_NodeExpand(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles RadTree1.NodeExpand
        Dim nodo, arbre As String
        Dim objconn As OleDbConnection
        objconn = GAIA2.bdIni()
        nodo = e.NodeClicked.Value.Substring(0, e.NodeClicked.Value.IndexOf("-"))
        arbre = GAIA2.llegirCodiArbrePantalla(objconn, e.NodeClicked)
        If (e.NodeClicked.Nodes.Count = 0) Then
            GAIA2.afegeixNodesFillsPantallaLlista(objconn, nodo, e.NodeClicked, arbre, 0, 1, 0, 0, 1)
        End If
        GAIA2.bdFi(objconn)
    End Sub


    Protected Sub clickCercarNode(ByVal sender As Object, ByVal e As EventArgs)
        Dim objconn As OleDbConnection
        Dim selNode As RadTreeNode

        objconn = GAIA2.bdIni()
        selNode = RadTree1.SelectedNode()
        GAIA2.cercarNodeByTxt(objconn, RadTree1, cerca.Text, GAIA2.llegirCodiArbrePantalla(objconn, RadTree1.Nodes.Item(0)), idUsuari, 1, GAIA2.obtenirPathRelacioPantalla(objconn, selNode) + "_" + GAIA2.obtenirNodePantalla(objconn, selNode))
        GAIA2.bdFi(objconn)

    End Sub 'clickCercarNode


    ' Botó "aplicar" del formulari de permisos.
    ' Procés: grava tots els permisos i recalcula els permisos manuals

    Sub clickAplicarPermisos(ByVal sender As Object, ByVal e As EventArgs)
        Dim item As String
        Dim personas() As String

        Dim objconn As OleDbConnection
        Dim llistaPersonesAmbCanviPermis As String = ""
        Dim permisDescendent As Boolean
        Dim cont As Integer = 0
        Dim cont2 As Integer = 0
        objconn = GAIA2.bdIni()
        Dim llistaPersonesAmbCanviPermisDirecte As String = ""

        Dim rel As New clsRelacio
        rel.bdget(objconn, Request("relincod"))

        If Not Request("persona") Is Nothing Then
            personas = Request("persona").Split(",")
            For Each item In personas
                If Not Request("permisDescendent" & item) Is Nothing Then
                    permisDescendent = Request("permisDescendent" & item)
                Else
                    permisDescendent = False
                End If
                If Request("modificat" & item.ToString()) = "1" Then
                    assignarPermisos(objconn, Request("permis" & item), item, rel.infil, permisDescendent)
                    assignarPermisos2(objconn, Request("permis" & item), item, rel.infil, permisDescendent)
                    If cont > 0 Then
                        llistaPersonesAmbCanviPermis += ","
                    End If
                    llistaPersonesAmbCanviPermis += item
                    cont = cont + 1
                Else
                    If permisDescendent Then
                        assignarPermisos(objconn, Request("permis" & item), item, rel.infil, permisDescendent)
                        assignarPermisos2(objconn, Request("permis" & item), item, rel.infil, permisDescendent)
                        If cont2 > 0 Then
                            llistaPersonesAmbCanviPermisDirecte += ","
                        End If
                        llistaPersonesAmbCanviPermisDirecte += item
                        cont2 = cont2 + 1
                    End If
                End If
            Next item
            ' Inicialitzo la llista de persones o grups amb permis
            lblpersones.Text = ""
            lblpermisos.Text = ""
            lblpermisosHerencia.Text = ""
            lblPersonesAmbPermis.Text = ""

            If (btnClickLlistaAmpliada.Text = "llista ampliada") Then
                carregaPersonesAmbPermis(objconn, rel, 1, lblPersonesAmbPermis.Text, lblpersones.Text, lblpermisos.Text, lblpermisosHerencia.Text, -1)
            Else
                carregaPersonesAmbPermis(objconn, rel, 0, lblPersonesAmbPermis.Text, lblpersones.Text, lblpermisos.Text, lblpermisosHerencia.Text, -1)
            End If
        End If
        'Actualitzo els permisos heretats

        If llistaPersonesAmbCanviPermis <> "" Then
            '            clsPermisos.tractaPermisosHeretats(objconn, "", Request("nodes"), llistaPersonesAmbCanviPermis, "", 0)			
            clsPermisos.tractaPermisosHeretats2(objconn, Nothing, Request("nodes"), llistaPersonesAmbCanviPermis, 1, 0)
        End If
        If llistaPersonesAmbCanviPermisDirecte <> "" Then
            '            clsPermisos.tractaPermisosHeretats(objconn, 0, Request("nodes"), llistaPersonesAmbCanviPermisDirecte, "", 1)
            clsPermisos.tractaPermisosHeretats2(objconn, Nothing, Request("nodes"), llistaPersonesAmbCanviPermisDirecte, 1, 1)
        End If

        GAIA2.bdFi(objconn)
    End Sub

    Protected Sub clickLlistaAmpliada(ByVal sender As Object, ByVal e As EventArgs)
        Dim objconn As OleDbConnection
        objconn = GAIA2.bdIni()

        Dim rel As New clsRelacio
        rel.bdget(objconn, Request("relincod"))


        lblpersones.Text = ""
        lblpermisos.Text = ""
        lblpermisosHerencia.Text = ""
        lblPersonesAmbPermis.Text = ""
        If (btnClickLlistaAmpliada.Text = "llista ampliada") Then
            carregaPersonesAmbPermis(objconn, rel, 1, lblPersonesAmbPermis.Text, lblpersones.Text, lblpermisos.Text, lblpermisosHerencia.Text, -1)
            btnClickLlistaAmpliada.Text = "llista resumida"
        Else
            carregaPersonesAmbPermis(objconn, rel, 0, lblPersonesAmbPermis.Text, lblpersones.Text, lblpermisos.Text, lblpermisosHerencia.Text, -1)
            btnClickLlistaAmpliada.Text = "llista ampliada"
        End If
        GAIA2.bdFi(objconn)
    End Sub 'clickLlistaAmpliada





    Protected Sub clickAfegirPersones(ByVal sender As Object, ByVal e As EventArgs)
        Dim treenode As RadTreeNode
        Dim nodo As String
        Dim path As String
        Dim objconn As OleDbConnection
        objconn = GAIA2.bdIni()
        If Not Request("persona") Is Nothing Then
            lblpermisos.Text = lblpermisos.Text & "<script>"
            Dim personas() As String
            personas = Request("persona").Split(",")
            For Each nodo In personas
                lblpermisos.Text = lblpermisos.Text & "document.getElementById('permis" & nodo.ToString().Trim() & "').value=" & Request("permis" & nodo.ToString().Trim()) & ";"
            Next nodo
            lblpermisos.Text = lblpermisos.Text & "</script>"
        End If
        path = "/img/common/iconografia"
        For Each treenode In RadTree1.SelectedNodes
            nodo = GAIA2.descNode(objconn, treenode.Value.Substring(0, treenode.Value.IndexOf("-")))
            If lblPersonesAmbPermis.Text.IndexOf(nodo) < 0 Then
                If (treenode.Text.IndexOf("fulla organigrama")) > 0 Then
                    lblPersonesAmbPermis.Text = lblPersonesAmbPermis.Text & ("<image src='" & path & "/ico_organigrama.png'>")
                Else
                    If (treenode.Text.IndexOf("arbre organigrama")) > 0 Then
                        lblPersonesAmbPermis.Text = lblPersonesAmbPermis.Text & ("<image src='" & path & "/arbre.png'>")
                    Else
                        lblPersonesAmbPermis.Text = lblPersonesAmbPermis.Text & ("<image src='" & path & "/node_organigrama.png'>")
                    End If
                End If
                lblPersonesAmbPermis.Text = lblPersonesAmbPermis.Text & "<a href='#' name='persona'  class='txtNeg12px'  onclick='canviEstil(this);posaPermis(" & treenode.Value.Substring(0, treenode.Value.IndexOf("-")) & ",document.forms(0).permis" & treenode.Value.Substring(0, treenode.Value.IndexOf("-")) & ",document.forms(0).permisHerencia" & treenode.Value.Substring(0, treenode.Value.IndexOf("-")) & ");return false;'>" & nodo.ToString() & "</a>  <br/>"
                nropersones = nropersones + 1
                lblpersones.Text = lblpersones.Text & "<input type=""hidden"" name=""persona"" value=""" & treenode.Value.Substring(0, treenode.Value.IndexOf("-")) & """/>"
                lblpermisos.Text = lblpermisos.Text & "<input type=""hidden"" name=""permis" & treenode.Value.Substring(0, treenode.Value.IndexOf("-")) & """ value="

                If Request("permis" & treenode.Value.Substring(0, treenode.Value.IndexOf("-"))) >= 0 Then
                    lblpermisos.Text = lblpermisos.Text & "0"
                Else
                    lblpermisos.Text = lblpermisos.Text & Request("permis" & treenode.Value.Substring(0, treenode.Value.IndexOf("-")))
                End If
                lblpermisos.Text = lblpermisos.Text & "/>"
            End If
            lblpermisosHerencia.Text = lblpermisosHerencia.Text & "<input type=""hidden"" name=""permisHerencia" & treenode.Value.Substring(0, treenode.Value.IndexOf("-")) & """/>"



            lblpermisosHerencia.Text = lblpermisosHerencia.Text & "<input type=""hidden"" name=""modificat" & treenode.Value.Substring(0, treenode.Value.IndexOf("-")) & """ value=0/>"
        Next treenode
        GAIA2.bdFi(objconn)
    End Sub

    Public Shared Sub carregaPersonesAmbPermis(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal llistaAmpliada As Integer, ByRef strPersonesAmbPermis As String, ByRef lblpersones As String, ByRef lblpermisos As String, ByRef lblPermisosHerencia As String, ByVal permisos As String)
        carregaPersonesAmbPermis(objConn, rel, llistaAmpliada, strPersonesAmbPermis, lblpersones, lblpermisos, lblPermisosHerencia, permisos, 10)

    End Sub
    '******************************************************************
    '	Funció: carregaPersonesAmbPermis
    '	Entrada: 
    '		numNode:	Codi del Node sobre el que cercarem els nodes organigrama amb permís
    '		codiArbre: codi de l'arbre del node
    '		rel : objecte relació
    '		permisos: llista separada per comes dels permisos que volem trobar
    '	nronivells: només buscarà el permís a nodes superiors el nro de nivells indicat
    '	Procés: 	
    '		Selecciona les persones amb permis manual sobre el node o bé les persones amb permis heretat sobre node/arbre
    '******************************************************************
    Public Shared Sub carregaPersonesAmbPermis(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal llistaAmpliada As Integer, ByRef strPersonesAmbPermis As String, ByRef lblpersones As String, ByRef lblpermisos As String, ByRef lblPermisosHerencia As String, ByVal permisos As String, ByVal nronivells As Integer)
        'per eliminar
    End Sub  'carregaPersonesAmbPermis



    Public Shared Function llistaPersonesAmbPermis(ByVal objConn As OleDbConnection, ByVal node As Integer, ByVal rel As clsRelacio) As String
        'per eliminar
        Return ""
    End Function


    '******************************************************************
    '	Funció: trobaPermisNegatiu
    '	Entrada: 
    '		permis: tipus de permis
    '	Procés: 	
    '		Retorna el permis negatiu del donat.
    '	Sortida:
    '		codi del permis negatiu
    '******************************************************************
    Public Shared Function trobaPermisNegatiu(ByVal permis As Integer) As Integer
        'per eliminar
        Return 0
    End Function


    '******************************************************************
    '	Funció: assignarPermisos
    '	Entrada: 
    '		permisos: els 10 permisos bàsics es codifiquen en un byte. de forma que permis 1 = 2^0, permis2= 2*^1, permis3=2^2, etc.
    '		numNodeOrganigrama	: Codi del Node organigrama al que assignarem el permís
    '		numNode:	Codi del Node del que assignarem el permis
    '		rel : objecte relació on assignarem el permis. En aquesta relació, RELINFIL és numNode.
    '		herencia : 0 si es una assignacio manual, <>0 si automàtica
    '	Procés: 	
    '		Assigna el permis "tipusPermis" entre el node i el numNodeOrganigrama
    '	 	Si numNodeOrganigrama= arbre Organigrama , creo només un permis entre arbre Organigrama i numNode, sense herència
    '******************************************************************

    Public Shared Sub assignarPermisos(ByVal objConn As OleDbConnection, ByVal permisosIni As Integer, ByVal numNodeOrganigrama As Integer, ByVal codiNode As Integer, ByVal recursiu As Boolean)

        'per eliminar
    End Sub 'assignarPermisos



    Public Shared Sub assignarPermisos2(ByVal objConn As OleDbConnection, ByVal permisosIni As Integer, ByVal numNodeOrganigrama As Integer, ByVal codiNode As Integer, ByVal recursiu As Boolean)

        Dim arrPermis(9) As Integer
        Dim cont As Integer
        Dim ds, ds2 As DataSet
        ds = New DataSet()
        ds2 = New DataSet()
        Dim rel As New clsRelacio

        Dim permisos As Integer

        Dim dbrow As DataRow

        'msv:19/5/2011 faig un bucle per assignar els permisos a totes les relacions on aparegui el node
        'amb això assigno permisos directes al node i no a la relació.
        GAIA2.bdr(objConn, "SELECT RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELINFIL= " & codiNode & " AND RELCDSIT<95", ds)
        For Each dbrow In ds.Tables(0).Rows
            rel.bdget(objConn, dbrow("RELINCOD"))
            permisos = permisosIni

            ' decodifico el byte de permisos
            For cont = 9 To 0 Step -1
                If permisos >= Math.Pow(2, cont) Then
                    permisos = permisos - Math.Pow(2, cont)
                    arrPermis(cont) = 1
                Else
                    arrPermis(cont) = -1
                End If
            Next cont

            clsPermisos.gravaPermis2(objConn, numNodeOrganigrama, rel, 0, arrPermis(0), arrPermis(1), arrPermis(2), arrPermis(3), arrPermis(4), arrPermis(5), arrPermis(6), arrPermis(7), arrPermis(8), arrPermis(9), True)
        Next dbrow


        'Si he de gravar permis directe cap avall, faig el tractament

        If recursiu Then
            'ho faig per a tots els elements que penjen amb independència de RELCDSIT, per evitar problemes si hi ha canvi de permisos i després es recupera un node
            'tampoc treballo amb relcdsit 96,97 per que és un procés molt lent
            GAIA2.bdr(objConn, "SELECT RELINFIL FROM METLREL WITH(NOLOCK) WHERE RELCDSIT<95 AND RELCDRSU=" & rel.incod, ds)
            For Each dbrow In ds.Tables(0).Rows
                'reltmp.bdget(objconn,dbrow("RELINCOD"))				 
                assignarPermisos2(objConn, permisosIni, numNodeOrganigrama, dbrow("RELINFIL"), recursiu)
            Next dbrow
        End If

        ds.Dispose()
        ds2.Dispose()
    End Sub 'assignarPermisos2







    '************************************************************************************************************
    '	Funció: tePermisEdicio
    '	Entrada: 		
    '		numNodeOrganigrama: codi del node de l'organigrama
    '		node: Objecte del que volem saber si l'usuari té permís
    '	Procés: 	
    '		Comprova si existeix un permis d'edició  (3) o superior sobre el node. Com que pot apareixer a
    '		més d'una relació, comprovem que tingui permís a TOTES les relacions. 
    '		Si té permis a TOTES les relacions:
    '				tePermisEdicio=1
    '				strPermisos= ""
    '		En cas contrari :	
    '				tePermisEdicio=0
    '				strPermisos = Texte amb totes les relacions sobre les que NO te permisos
    '
    '************************************************************************************************************	

    Public Shared Function tePermisEdicio(ByVal objConn As OleDbConnection, ByVal numNodeOrganigrama As Integer, ByVal node As Integer, ByRef strPermisos As String) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim permis As Integer = 0
        Dim rel As New clsRelacio
        DS = New DataSet()
        tePermisEdicio = 0
        strPermisos = ""

        GAIA2.bdr(objConn, "SELECT RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELINFIL=" + node.ToString() + " and RELCDSIT<98", DS)

        For Each dbRow In DS.Tables(0).Rows
            If tePermisEdicio = 1 Then
                'res
            Else
                rel.bdget(objConn, dbRow("RELINCOD"))
                If False Then
                    tePermisEdicio = clsPermisos.tepermis(objConn, 3, numNodeOrganigrama, numNodeOrganigrama, rel, 0)
                End If



                tePermisEdicio = clsPermisos.tepermis2(objConn, 3, numNodeOrganigrama, numNodeOrganigrama, rel, 0, "", "", 0)
                If tePermisEdicio = 1 Then
                    strPermisos = rel.noddstxt
                End If
            End If
        Next dbRow


        DS.Dispose()
    End Function




    '*******************************************************************************************************
    ' Max - Març 2008
    ' Donat un node i l'herencia (indica on està), actualitzo tots els permisos heretats 
    ' des d'ell fins tots els nodes inferiors.
    ' Si node=-1, la funció s'ha cridat desde un "tractaPermisosHeretats" superior i caldrà tractar-ho de forma especial.
    ' ex: per actualitar tots els permisosHeretats de la intranet:   clsPermisos.tractaPermisosHeretats(objconn,"_55785",-1)
    '*******************************************************************************************************
    Public Shared Sub tractaPermisosHeretats(ByVal objconn As OleDbConnection, ByVal herenciaNode As String, ByVal node As Integer, ByVal llistaPersonesInicial As String, ByVal llistaPersones As String, ByVal tractarFillsORG As Integer)
        tractaPermisosHeretats(objconn, herenciaNode, node, llistaPersonesInicial, llistaPersones, tractarFillsORG, False)
    End Sub


    Public Shared Sub tractaPermisosHeretats(ByVal objconn As OleDbConnection, ByVal herenciaNode As String, ByVal node As Integer, ByVal llistaPersonesInicial As String, ByVal llistaPersones As String, ByVal tractarFillsORG As Integer, ByVal tractarPermisosEnDiferit As Boolean)


        '29/6/2015: Msv, Gravo en METLMAP2  per saber el que hauré de canviar després de proves
        '	GAIA.bdSR(objconn, "INSERT INTO METLMAP2 (MAPINNOD, MAPCDREL, MAPDSLPE, MAPDTTIM, MAPCDERR) VALUES (" & node & ",0,'" & llistaPersones & "', GETDATE(),0)")

        'per eliminar
    End Sub



    Private Shared Sub assignaPermisosHeretats(ByVal objconn As OleDbConnection, ByVal rel As clsRelacio, ByVal llistaPersones As String, ByVal tractarFillsORG As Integer)
        'per eliminar
    End Sub



    Private Shared Sub eliminaPermisosComplementaris(ByRef ds As DataSet)

        For Each dr As DataRow In ds.Tables(0).Rows
            If dr("PERCDTIP") <> 0 Then
                For Each dr2 As DataRow In ds.Tables(0).Rows

                    If dr("PERINORG") = dr2("PERINORG") Then
                        If clsPermisos.trobaPermisNegatiu(dr("PERCDTIP")) = dr2("PERCDTIP") Then
                            dr2("PERCDTIP") = 0 'marco el permis amb un 0 per no tractar-lo
                        End If
                    End If
                Next
            End If
        Next
        ds.Tables(0).AcceptChanges()


    End Sub




    ' Fa el manteniment de totes les peticions de tractament de permisos heretats que hi ha a la taula METLMAP 
    Public Shared Sub mantenimentPermisosHeretats()
        Dim objconn As OleDbConnection
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        objconn = GAIA2.bdIni()
        GAIA2.bdr(objconn, "SELECT * FROM METLMAP2 WITH(NOLOCK) WHERE MAPCDERR=0 ORDER BY MAPINCOD ASC", DS)
        For Each dbRow In DS.Tables(0).Rows
            clsPermisos.tractaPermisosHeretats2(objconn, Nothing, dbRow("MAPINNOD"), dbRow("MAPDSLPE"), 1, 0, True)
            GAIA2.bdSR(objconn, "UPDATE METLMAP2 SET MAPCDERR=2 WHERE  MAPINNOD=" & dbRow("MAPINNOD"))
        Next dbRow


        '26/3/2018, MSV: Hi ha un problema amb l'arbre "el meu espai". No sabem el motiu però apareixen permisos de lectura al usuari TOTS
        GAIA2.bdr(objconn, "UPDATE METLPER2 SET PERCDPE9=0,PERCDPE10=1 WHERE PERINORG=115969 AND PERCDREL IN (select RELINCOD FROM METLREL WITH(NOLOCK),METLPER2 WITH(NOLOCK) WHERE RELCDRSU=198336 AND PERCDREL=RELINCOD AND RELCDSIT<98 AND PERINORG=115969 AND PERCDPE10=0)", DS)


        DS.Dispose()
    End Sub 'mantenimentPermisosHeretats




    '******************************************************************
    '	Funció: getGroupsAD
    '	Entrada: 
    '		usariXarxa: nom de l'usuari que cercarem a l'active directory

    '	Procés: 	
    '		retorna una llista separada per | amb tots els grups als que pertany l'usuari
    '******************************************************************	
    Public Shared Function GetGroupsAD(ByVal usuariXarxa As String) As String
        Return GetGroupsAD(usuariXarxa, "")
    End Function


    Public Shared Function GetGroupsAD(ByVal usuariXarxa As String, ByVal pwd As String) As String

        Dim usuariAdm As String
        If pwd = "" Then
            pwd = "secopc"
            usuariAdm = "adminbackup"
        Else
            usuariAdm = usuariXarxa
        End If
        Try
            Dim pathLDAP As String = String.Empty
            Dim GroupString As String = String.Empty
            Dim oDE As New DirectoryEntry("LDAP://l-h.es", usuariAdm, pwd)

            Dim oDSearcher As New DirectorySearcher(oDE)
            oDSearcher.Filter = "sAMAccountName=" & usuariXarxa
            oDSearcher.PropertiesToLoad.Add("memberOf")

            Dim propertyCount As Integer
            Dim oResultSearch As DirectoryServices.SearchResult = oDSearcher.FindOne()

            propertyCount = oResultSearch.Properties("memberOf").Count

            Dim dn As String
            Dim equalsIndex, commaIndex As String

            For i As Integer = 0 To propertyCount - 1
                dn = oResultSearch.Properties("memberOf")(i)
                equalsIndex = dn.IndexOf("=", 1)
                commaIndex = dn.IndexOf(",", 1)
                If equalsIndex = -1 Then
                    Return "TOTS"
                End If

                GroupString += dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1) & "|"

            Next

            GroupString = (GroupString + "|" + "TOTS").Replace("||", "|")
            Return GroupString + "|" + "TOTS"

        Catch ex As Exception
            Return "TOTS"
        End Try








    End Function

    '******************************************************************
    '	Funció: tePermis
    '	Entrada: 
    '		permis: tipus de permis
    '		numNodeOrganigrama: codi del node de l'organigrama
    '		rel: codi de la relacio que apunta a un contingut sobre la que comprovarem si hi ha permís.


    '	Procés: 	
    '		Comprova si existeix un permis de tipus "permis" o bé un permís superior
    ' entre numnodeorganigrama i numnode. 

    '	També accepto que el permís estigui sobre el node "arbre organigrama"
    ' 	Si existeix retorna true. 
    '		Si no existeix retorna fals
    '******************************************************************	
    Public Shared Function tepermis(ByVal objConn As OleDbConnection, ByVal permis As Integer, ByVal numNodeOrganigramaini As Integer, ByVal numNodeOrganigrama As Integer, ByVal rel As clsRelacio, ByRef heretat As Integer) As Integer
        Return (tepermis(objConn, permis, numNodeOrganigramaini, numNodeOrganigrama, rel, heretat, "", "", 0))
    End Function


    Public Shared Function tepermis(ByVal objConn As OleDbConnection, ByVal permis As Integer, ByVal numNodeOrganigramaini As Integer, ByVal numNodeOrganigrama As Integer, ByVal rel As clsRelacio, ByRef heretat As Integer, ByVal usuariXarxa As String, ByVal grupsAD As String) As Integer
        Return (tepermis(objConn, permis, numNodeOrganigramaini, numNodeOrganigrama, rel, heretat, usuariXarxa, grupsAD, 0))
    End Function
    'Si cerco el permís per a un node, buscaré si hi ha permís positiu en alguna de les relacions d'on penja. Si és així dono OK al permís.
    Public Shared Function tepermis(ByVal objConn As OleDbConnection, ByVal permis As Integer, ByVal numNodeOrganigramaini As Integer, ByVal numNodeOrganigrama As Integer, ByVal rel As clsRelacio, ByRef heretat As Integer, ByVal usuariXarxa As String, ByVal grupsAD As String, ByVal node As Integer) As Integer




        'Faig el canvi al nou sistema de permisos
        Return (clsPermisos.tepermis2(objConn, permis, numNodeOrganigramaini, numNodeOrganigrama, rel, heretat, usuariXarxa, grupsAD, node))







    End Function

    'afegeix un element a una llista separada per comes (l'element pot ser una altra llista)
    Public Shared Sub afegirElement(ByVal element As String, ByRef llista As String)
        If Trim(element) <> "" Then
            If InStr(element, ",") = 0 Then
                'afegim un sol element, només l'afegirem si no existeix
                If Not existeixElement(element, llista) Then
                    If Trim(llista) <> "" Then llista &= ","
                    llista &= element
                End If
            Else
                'afegim una llista, no es comproben duplicats
                If Trim(llista) <> "" Then llista &= ","
                llista &= element
            End If
        End If
    End Sub

    Public Shared Function existeixElement(ByVal element As String, ByRef llista As String) As Boolean


        If llista = element Or InStr(llista, "," & element & ",") > 0 Or InStr(llista, element & ",") > 0 Or InStr(llista, "," & element) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function



    'elimina un element d'una llista separada per comes
    Public Shared Sub eliminarElement(ByVal element As String, ByRef llista As String)

        'elimino si està pel mig
        llista = Replace(llista, "," & element & ",", ",")
        'elimino si està al final



        llista = Replace(llista, "," & element, "")
        'elimino si està al començament
        If InStr(llista, element) = 1 Then
            llista = Replace(llista, element & ",", "")
        End If

        'elimino si només queda aquest valor
        If llista = element Then
            llista = ""
        End If
    End Sub

    Public Shared Function FillsRelacioAmbPermis(ByVal objConn As OleDbConnection, ByVal permis As Integer, ByVal numNodeOrganigramaini As Integer, ByVal usuariGAIA As Integer, ByVal relPare As clsRelacio, ByRef heretat As Integer, ByVal usuariXarxa As String, ByRef grupsAD As String) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim item As String = ""

        Dim llistaAmbPermisos As String = "", llistaSensePermisos As String = "", llistaPendents As String = ""

        GAIA2.bdr(objConn, "select RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELCDRSU=" & relPare.incod & " AND RELCDSIT<98", DS)
        For Each dbRow In DS.Tables(0).Rows
            If llistaPendents.Length > 0 Then llistaPendents &= ","
            llistaPendents &= dbRow("RELINCOD")
        Next
        trobaPermisLlistaRelacions(objConn, permis, llistaPendents, usuariGAIA, grupsAD, "", llistaAmbPermisos, llistaSensePermisos, usuariXarxa)
        DS.Dispose()
        Return llistaAmbPermisos

    End Function 'FillsRelacioAmbPermis

    Public Shared Sub trobaPermisLlistaRelacions(ByVal objConn As OleDbConnection, ByVal permis As Integer, ByRef llistaPendents As String, ByVal usuariGAIA As Integer, ByRef grupsAD As String, ByVal herenciaORG As String, ByRef llistaAmbPermisos As String, ByRef llistaSensePermisos As String)
        trobaPermisLlistaRelacions(objConn, permis, llistaPendents, usuariGAIA, grupsAD, herenciaORG, llistaAmbPermisos, llistaSensePermisos, "")
    End Sub





    Public Shared Sub trobaPermisLlistaRelacions(ByVal objConn As OleDbConnection, ByVal permis As Integer, ByRef llistaPendents As String, ByVal usuariGAIA As Integer, ByRef grupsAD As String, ByVal herenciaORG As String, ByRef llistaAmbPermisos As String, ByRef llistaSensePermisos As String, ByVal usuariXarxa As String, Optional ByVal pendentsSonNodes As Boolean = False, Optional ByRef llistaNodesSensePermis As String = "")


        clsPermisos.trobaPermisLlistaRelacions2(objConn, permis, llistaPendents, usuariGAIA, grupsAD, herenciaORG, llistaAmbPermisos, llistaSensePermisos, usuariXarxa, pendentsSonNodes, llistaNodesSensePermis)

    End Sub


    '******************************************************************
    '	Funció: tePermisLecturaNode
    '	Entrada: 

    '		numNodeOrganigrama: codi del node de l'organigrama
    '		rel: codi de la relació que volem comprovar si hi ha permis per mostrar el node a la pantalla
    '		NodeRoot: codi de relació que apunta al root de l'arbre. Si hi ha permis entre el root de numNodeOrganigrama i noderoot llavors retorno cert. Si nodeRoot=0 llavors el busco.
    '	Procés: 	
    '		Comprova si existeix qualsevol permís (heretat per qualsevol permis d'un altre node o ell mateix) de lectura de node
    ' entre numnodeorganigrama i la relació o bé entre el node inicial de l'arbre i 'arbreorganigrama'.
    ' 	Si existeix retorna true. 
    '		Si no existeix retorna fals
    '******************************************************************	
    Public Shared Function tePermisLecturaNode(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal numNodeOrganigrama As Integer, ByVal nodeRoot As Integer) As Integer


        If clsPermisos.tepermis2(objConn, 9, numNodeOrganigrama, numNodeOrganigrama, rel, 0) = 1 Then
            Return True
        Else
            Return False
        End If


        If False Then

            If clsPermisos.tepermis(objConn, 11, numNodeOrganigrama, numNodeOrganigrama, rel, 0) = 1 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function 'tePermisLecturaNode


    Public Shared Function tePermisLecturaNode(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal numNodeOrganigrama As Integer, ByVal nodeRoot As Integer, ByVal usuariXarxa As String) As Integer

        If clsPermisos.tepermis2(objConn, 9, numNodeOrganigrama, numNodeOrganigrama, rel, 0, usuariXarxa, "") = 1 Then
            Return True
        Else
            Return False
        End If







        If False Then
            If clsPermisos.tepermis(objConn, 11, numNodeOrganigrama, numNodeOrganigrama, rel, 0, usuariXarxa, "") = 1 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function



    '******************************************************************
    '	Funció: gravaPermis
    '	Entrada: 
    '		codiNodePermis: Tipus de permís
    '		numNodeOrganigrama	: Codi del Node organigrama al que assignarem el permís
    '		numNode:	Codi del Node del que assignarem el permis
    '		herencia: =0 si permís assignat manualment.
    '							>0 si és un permis heredat. El valor és el número de node del que hereda el permís.
    '		rel: Relació ó on assignarè el permís. Si rel.incod=0 ho faré a totes les relacions.
    '		recursiu: assigna al permis directe a tots els nodes inferiors
    '	Procés: 	
    '		Assigna el permis "tipusPermis" entre el node1 i el numNodeOrganigrama
    '		Si el permís ja existia modifica el camp herencia sempre i quan, pasi de PERCDHER > 0 a percdheR =0 .
    '	Sortida:
    '		>0 ->  error
    '		0 -> ok
    '******************************************************************
    Public Shared Sub gravaPermis(ByVal objConn As OleDbConnection, ByVal codiNodePermis As Integer, ByVal numNodeOrganigrama As Integer, ByVal herencia As Integer, ByVal herencia2 As Integer, ByVal rel As clsRelacio)
        'per eliminar
    End Sub


    Public Shared Function completarLlistaAmbParesOrganigrama(ByVal objconn As OleDb.OleDbConnection, ByVal usuariGaia As String, ByVal posaTots As Boolean) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()

        completarLlistaAmbParesOrganigrama = usuariGaia
        'Busco els nodes organigrama superiors per si hi ha un permís directe a nivell de grup
        GAIA2.bdr(Nothing, "SELECT RELINPAR FROM METLREL WITH(NOLOCK) WHERE RELINFIL=" & usuariGaia & " AND RELCDHER LIKE '_56261%' AND RELCDSIT<98 ", DS)

        For Each dbRow In DS.Tables(0).Rows
            clsPermisos.afegirElement(dbRow("RELINPAR"), completarLlistaAmbParesOrganigrama)
        Next dbRow

        If posaTots Then
            clsPermisos.afegirElement("115969", completarLlistaAmbParesOrganigrama)
        End If
        DS.Dispose()
        Return (completarLlistaAmbParesOrganigrama)
    End Function


#Region "Permisos 2"

    '**************************************************************************************************
    ' BLOC AMB NOVA VERSIÓ DE FUNCIONS DE GESTIö DE PERMISOS
    '**************************************************************************************************





    '****************************************************************************************************************************
    ' Calculo els permisos que he de gravar
    '-1: per esborrar un permís directe!
    ' 0: per obtenir permisos heretats superiors. En cas de 0,2, o 0,1 és per assignar permisos negatius
    ' 1: per assignar permisos directes
    ' 2: per assignar permisos heretats
    '****************************************************************************************************************************

    Private Shared Sub calculaPermisNou(ByVal objConn As OleDbConnection, ByVal llistaParesOrg As String, ByVal rel As clsRelacio, ByVal permisNou1 As Integer, ByVal permisNou2 As Integer, ByRef permisActual1 As Integer, ByRef permisActual2 As Integer, ByVal tipusPermis As Integer)


        Select Case permisNou1
            Case -1
                If permisNou2 = 1 Then
                    permisActual1 = 0
                    permisActual2 = 1
                Else
                    If clsPermisos.trobaPermisDirecteSuperior(objConn, llistaParesOrg, rel, 1) = 1 Then
                        permisActual1 = 2
                        permisActual2 = 0
                    Else
                        permisActual1 = 0
                        permisActual2 = 2
                    End If
                End If

            Case 0
                If permisNou2 = 1 Then
                    permisActual2 = 1
                    permisActual1 = 0
                Else
                    Select Case permisActual1
                        Case 0
                            permisActual1 = 0
                            permisActual2 = permisNou2
                        Case 1
                            permisActual1 = 1
                            permisActual2 = 0
                        Case 2
                            If clsPermisos.trobaPermisDirecteSuperior(objConn, llistaParesOrg, rel, 1) = 1 Then
                                permisActual1 = 2
                                permisActual2 = 0
                            Else
                                permisActual1 = 0
                                permisActual2 = 2
                            End If

                    End Select
                End If
            Case 1
                permisActual1 = 1
                permisActual2 = 0
            Case 2
                permisActual1 = IIf(permisActual1 = 1, 1, 2)
                permisActual2 = 0
        End Select
    End Sub



    'numnodeOrganigrama: usuari o grup d'usuaris sobre el que apliquem el permís
    'rel: apuntador al contingut sobre el que aplicarem permisos
    'heretat=true: si els permisos que es volen gravar venen d'un objecte superior
    'permis1 a 10, permisos amb valors 0: no té permís, 1: permís directe (si heretat=true,és un permís directe del node superior), 2: heretat 
    'Si assignem un permis positiu o negatiu però directe a un nivell concret, els positius o negatius inferiors tindran el mateix valor a permisNouX. Per exemple: permisNou3=1 --> permisNou5=1, permisNou7=1, permisNou9=1


    Public Shared Sub gravaPermis2(ByVal objConn As OleDbConnection, ByVal numNodeOrganigrama As Integer, ByVal rel As clsRelacio, ByVal heretat As Boolean, ByVal permisNou1 As Integer, ByVal permisNou2 As Integer, ByVal permisNou3 As Integer, ByVal permisNou4 As Integer, ByVal permisNou5 As Integer, ByVal permisNou6 As Integer, ByVal permisNou7 As Integer, ByVal permisNou8 As Integer, ByVal permisNou9 As Integer, ByVal permisNou10 As Integer, ByVal tractaPermisosEndiferit As Boolean, Optional ByVal posarPermisosAscendents As Boolean = True)
        If rel.incod <> 0 Then
            Dim DS As DataSet
            Dim dbRow As DataRow
            DS = New DataSet()
            Dim DS2 As DataSet
            Dim dbRow2 As DataRow
            DS2 = New DataSet()
            Dim tipusHerencia As String = "2"
            ' Creo que no hay que hacer este bloque. los permisos deberían calcularse de nodos superiores siempre


            Dim permisActual1, permisActual2, permisActual3, permisActual4, permisActual5, permisActual6, permisActual7, permisActual8, permisActual9, permisActual10 As Integer
            Dim permisTMP As Integer = 0

            Dim llistaParesOrg As String = numNodeOrganigrama
            'GAIA.bdr(objConn, "(SELECT RELINPAR FROM METLREL WHERE RELINFIL=" & numNodeOrganigrama & " and RELCDSIT<98 and RELCDHER like '_56261_115969%' )", DS2)
            GAIA2.bdr(objConn, "(SELECT RELINPAR FROM METLREL WHERE RELINFIL=" & numNodeOrganigrama & "  and RELCDHER like '_56261_115969%' )", DS2)
            For Each dbRow2 In DS2.Tables(0).Rows
                clsPermisos.afegirElement(dbRow2("RELINPAR"), llistaParesOrg)
            Next dbRow2
            clsPermisos.afegirElement(115969, llistaParesOrg)

            '***************************************************************
            'aplico la condició d'herència als permisos
            '***************************************************************
            If heretat Then
                If permisNou1 = 1 Then permisNou1 = 2
                If permisNou2 = 1 Then permisNou2 = 2
                If permisNou3 = 1 Then permisNou3 = 2
                If permisNou4 = 1 Then permisNou4 = 2
                If permisNou5 = 1 Then permisNou5 = 2
                If permisNou6 = 1 Then permisNou6 = 2
                If permisNou7 = 1 Then permisNou7 = 2
                If permisNou8 = 1 Then permisNou8 = 2
                If permisNou9 = 1 Then permisNou9 = 2
                If permisNou10 = 1 Then permisNou10 = 2
            End If

            '********************************************************************************************************************
            'aplico els permisos nous heretats segons permisos menys restrictius. Si admin, també escriptor, lector, etc.
            'si el permis és directe, s'aplica permis directe a la resta de permisos. Si admin directe,escriptor directe tb.
            '********************************************************************************************************************
            If permisNou1 = 1 Then
                permisNou2 = 0
                permisNou3 = 1
                permisNou4 = 0
                permisNou5 = 1
                permisNou6 = 0
                permisNou7 = 1
                permisNou8 = 0
                permisNou9 = 1
                permisNou10 = 0
            End If

            If permisNou3 = 1 Then
                permisNou4 = 0
                permisNou5 = 1
                permisNou6 = 0
                permisNou7 = 1
                permisNou8 = 0
                permisNou9 = 1
                permisNou10 = 0
            End If

            If permisNou5 = 1 Then
                permisNou6 = 0
                permisNou7 = 1
                permisNou8 = 0
                permisNou9 = 1
                permisNou10 = 0
            End If

            If permisNou7 = 1 Then
                permisNou8 = 0
                permisNou9 = 1
                permisNou10 = 0
            End If

            If permisNou9 = 1 Then
                permisNou10 = 0
            End If

            '********************************************************************************************************************
            ' Busco els permisos actuals de la relació/usuari i els tracto
            '********************************************************************************************************************
            GAIA2.bdr(objConn, "SELECT  * FROM METLPER2 WITH(NOLOCK) WHERE PERINORG=" & numNodeOrganigrama & " AND PERCDREL=" & rel.incod, DS)
            If DS.Tables(0).Rows.Count > 0 Then
                'Si hi ha permisos, els adapto als nous
                dbRow = DS.Tables(0).Rows(0)
                permisActual1 = dbRow("PERCDPE1")
                permisActual2 = dbRow("PERCDPE2")
                permisActual3 = dbRow("PERCDPE3")
                permisActual4 = dbRow("PERCDPE4")
                permisActual5 = dbRow("PERCDPE5")
                permisActual6 = dbRow("PERCDPE6")
                permisActual7 = dbRow("PERCDPE7")
                permisActual8 = dbRow("PERCDPE8")
                permisActual9 = dbRow("PERCDPE9")
                permisActual10 = dbRow("PERCDPE10")


                '****************************************************************************************************************************
                ' Calculo els permisos que he de gravar
                '-1: per esborrar un permís directe!
                ' 0: per obtenir permisos heretats superiors. En cas de 0,2, o 0,1 és per assignar permisos negatius
                ' 1: per assignar permisos directes
                ' 2: per assignar permisos heretats
                '****************************************************************************************************************************
                calculaPermisNou(objConn, llistaParesOrg, rel, permisNou1, permisNou2, permisActual1, permisActual2, 1)
                calculaPermisNou(objConn, llistaParesOrg, rel, permisNou3, permisNou4, permisActual3, permisActual4, 3)
                calculaPermisNou(objConn, llistaParesOrg, rel, permisNou5, permisNou6, permisActual5, permisActual6, 5)
                calculaPermisNou(objConn, llistaParesOrg, rel, permisNou7, permisNou8, permisActual7, permisActual8, 7)
                calculaPermisNou(objConn, llistaParesOrg, rel, permisNou9, permisNou10, permisActual9, permisActual10, 9)


                GAIA2.bdSR(objConn, "if NOT EXISTS (select PERINORG FROM METLPER2 WITH(NOLOCK) WHERE PERCDREL=" & rel.incod & " AND PERINORG=" & numNodeOrganigrama & " AND PERCDPE1=" & permisActual1 & " AND PERCDPE2=" & permisActual2 & "  AND PERCDPE3=" & permisActual3 & " AND PERCDPE4=" & permisActual4 & " AND PERCDPE5=" & permisActual5 & " AND PERCDPE6=" & permisActual6 & " AND PERCDPE7=" & permisActual7 & " AND PERCDPE8=" & permisActual8 & " AND PERCDPE9=" & permisActual9 & " AND PERCDPE10=" & permisActual10 & ")   UPDATE METLPER2 SET PERCDPE1=" & permisActual1 & ",PERCDPE2=" & permisActual2 & ",PERCDPE3=" & permisActual3 & ",PERCDPE4=" & permisActual4 & ",PERCDPE5=" & permisActual5 & ",PERCDPE6=" & permisActual6 & ",PERCDPE7=" & permisActual7 & ",PERCDPE8=" & permisActual8 & ",PERCDPE9=" & permisActual9 & ",PERCDPE10=" & permisActual10 & " WHERE PERINORG=" & numNodeOrganigrama & " AND PERCDREL=" & rel.incod)
            Else
                'Si no hi ha permisos, els genero
                If Not (permisNou1 = 0 And permisNou2 = 0 And permisNou3 = 0 And permisNou4 = 0 And permisNou5 = 0 And permisNou6 = 0 And permisNou7 = 0 And permisNou8 = 0 And permisNou9 = 0 And permisNou10 = 0) Then
                    ' GAIA.bdSR(objConn, "INSERT INTO METLPER2 VALUES (" & rel.incod & "," & numNodeOrganigrama & "," & rel.infil & "," & permisNou1 & "," & permisNou2 & "," & permisNou3 & "," & permisNou4 & "," & permisNou5 & "," & permisNou6 & "," & permisNou7 & "," & permisNou8 & "," & permisNou9 & "," & permisNou10 & ")")
                    If permisNou1 = -1 Then permisNou1 = 0
                    If permisNou2 = -1 Then permisNou2 = 0
                    If permisNou3 = -1 Then permisNou3 = 0
                    If permisNou4 = -1 Then permisNou4 = 0
                    If permisNou5 = -1 Then permisNou5 = 0
                    If permisNou6 = -1 Then permisNou6 = 0
                    If permisNou7 = -1 Then permisNou7 = 0
                    If permisNou8 = -1 Then permisNou8 = 0
                    If permisNou9 = -1 Then permisNou9 = 0
                    If permisNou10 = -1 Then permisNou10 = 0
                    GAIA2.bdSR(objConn, "if NOT EXISTS (select PERINORG FROM METLPER2 WITH(NOLOCK) WHERE PERCDREL=" & rel.incod & " AND PERINORG=" & numNodeOrganigrama & " AND PERCDPE1=" & permisNou1 & " AND PERCDPE2=" & permisNou2 & "  AND PERCDPE3=" & permisNou3 & " AND PERCDPE4=" & permisNou4 & " AND PERCDPE5=" & permisNou5 & " AND PERCDPE6=" & permisNou6 & " AND PERCDPE7=" & permisNou7 & " AND PERCDPE8=" & permisNou8 & " AND PERCDPE9=" & permisNou9 & " AND PERCDPE10=" & permisNou10 & ")  INSERT INTO METLPER2 VALUES (" & rel.incod & "," & numNodeOrganigrama & "," & rel.infil & "," & permisNou1 & "," & permisNou2 & "," & permisNou3 & "," & permisNou4 & "," & permisNou5 & "," & permisNou6 & "," & permisNou7 & "," & permisNou8 & "," & permisNou9 & "," & permisNou10 & ")")
                End If
            End If
            DS.Dispose()
            DS2.Dispose()
        End If

        'Tracto les herències de nodes organigrama dins del contingut i descendentment per l'arbre de continguts
        If heretat = 0 Then
            clsPermisos.tractaPermisosHeretats2(objConn, rel, rel.infil, numNodeOrganigrama, 1, tractaPermisosEndiferit, False)
        End If

    End Sub 'gravaPermis2


    'Busco el primer permis directe superior (positiu o negatiu), seguint l'ordre dels usuaris que hi ha a llistaUsuaris

    Public Shared Function trobaPermisDirecteSuperior(ByVal objconn As OleDb.OleDbConnection, ByVal llistaUsuaris As String, ByVal rel As clsRelacio, ByVal tipusPermis As Integer) As Integer
        Dim permis As Integer = 0
        Dim strsql As String
        Dim relPare As New clsRelacio

        relPare.bdget(objconn, rel.cdrsu)
        If relPare.incod <> 0 Then
            Dim DS As DataSet
            Dim dbRow As DataRow
            DS = New DataSet()
            For Each usuari As String In llistaUsuaris.Split(",")

                strsql = "select METLPER2.PERCDPE" & tipusPermis & ", METLPER2.PERCDPE" & (tipusPermis + 1) & " FROM METLREL WITH(NOLOCK), METLPER2 WITH(NOLOCK) WHERE RELINCOD= " & IIf(relPare.incod = 0, -1, relPare.incod) & " AND PERCDREL=RELINCOD AND PERINORG=" & usuari & " AND (PERCDPE" & tipusPermis & "=1 OR PERCDPE" & tipusPermis + 1 & "=1) "
                GAIA2.bdr(objconn, strsql, DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    If dbRow("PERCDPE" & tipusPermis) = 1 Then
                        permis = 1
                        Exit For
                    End If
                    If dbRow("PERCDPE" & (tipusPermis + 1)) = 1 Then
                        permis = 0
                        Exit For
                    End If
                    DS.Dispose()
                Else
                    DS.Dispose()
                    permis = trobaPermisDirecteSuperior(objconn, usuari, relPare, tipusPermis)
                    If permis <> 0 Then Exit For
                End If
            Next usuari
        End If

        'si encara no he trobat un permis directe, retorno 0
        Return (permis)
    End Function


    Public Shared Sub tractaPermisosHeretats2(ByVal objconn As OleDbConnection, ByVal rel As clsRelacio, ByVal node As Integer, ByVal llistaPersonesInicial As String, ByVal tractarFillsORG As Integer, ByVal tractarPermisosEnDiferit As Boolean, Optional ByVal posarPermisosAscendents As Boolean = False)


        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()


        If tractarPermisosEnDiferit Then
            GAIA2.bdSR(objconn, "  INSERT INTO METLMAP2 (MAPINNOD, MAPCDREL, MAPDSLPE, MAPDTTIM, MAPCDERR)  VALUES (" & rel.infil & "," & rel.incod & ",'" & llistaPersonesInicial & "', GETDATE(),0)")
        Else
            Dim strsql As String = ""


            If IsNothing(rel) Then
                Dim rel2 As New clsRelacio()
                GAIA2.bdr(objconn, "SELECT RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELCDSIT<99 AND RELINFIL=" & node, DS)
                For Each dbRow In DS.Tables(0).Rows
                    rel2.bdget(objconn, dbRow("RELINCOD"))
                    tractaPermisosHeretats2(objconn, rel2, rel2.infil, llistaPersonesInicial, tractarFillsORG, tractarPermisosEnDiferit, posarPermisosAscendents)
                Next dbRow
            Else
                'Carrego l'estructura d'organigrama per reutilitzar-la
                If clsPermisos.hOrganigrama Is Nothing Then
                    clsPermisos.carregaOrganigrama()
                End If

                strsql = " Select * FROM METLPER2 WITH(NOLOCK) WHERE PERCDREL=" & rel.cdrsu
                If Not String.IsNullOrEmpty(llistaPersonesInicial) Then
                    strsql &= " AND PERINORG IN (" & llistaPersonesInicial & ")"
                End If

                GAIA2.bdr(objconn, strsql, DS)
                Dim llistaPersones As String = ""
                Dim llistaFills As String = ""

                If tractarFillsORG = 1 Then
                    'Si l'usuari TOTS està a la llista d'usuaris inicial, el afegeixo
                    ' If clsPermisos.existeixElement("115969", llistaPersonesInicial) Then llistaPersones = "115969"
                    'Amplio la llista d'usuaris amb permís amb els fills dels nodes organigrama que hi hagin
                    For Each dbRow In DS.Tables(0).Rows
                        If Not clsPermisos.existeixElement(dbRow("PERINORG"), llistaPersonesInicial) Then
                            clsPermisos.afegirElement(dbRow("PERINORG"), llistaPersones)
                            'posar -1 ?
                            clsPermisos.gravaPermis2(objconn, dbRow("PERINORG"), rel, 1, dbRow("PERCDPE1"), dbRow("PERCDPE2"), dbRow("PERCDPE3"), dbRow("PERCDPE4"), dbRow("PERCDPE5"), dbRow("PERCDPE6"), dbRow("PERCDPE7"), dbRow("PERCDPE8"), dbRow("PERCDPE9"), dbRow("PERCDPE10"), tractarPermisosEnDiferit, posarPermisosAscendents)
                        End If

                        'Si no és l'usuari TOTS, busco els nodes organigrama fills del que estem tractant per assignar permisos
                        If dbRow("PERINORG") <> 115969 Then
                            If clsPermisos.hOrganigrama.ContainsKey(dbRow("PERINORG")) Then
                                For Each persona As String In clsPermisos.hOrganigrama.Item(dbRow("PERINORG")).Split(",")
                                    If Not clsPermisos.existeixElement(persona, llistaPersones) Then
                                        clsPermisos.gravaPermis2(objconn, persona, rel, 1, dbRow("PERCDPE1"), dbRow("PERCDPE2"), dbRow("PERCDPE3"), dbRow("PERCDPE4"), dbRow("PERCDPE5"), dbRow("PERCDPE6"), dbRow("PERCDPE7"), dbRow("PERCDPE8"), dbRow("PERCDPE9"), dbRow("PERCDPE10"), tractarPermisosEnDiferit, False)
                                    End If
                                    clsPermisos.afegirElement(persona, llistaPersones)
                                Next persona
                            End If
                        End If

                    Next

                End If
                '*********************************************************************************************
                ' Avanço a nivells inferiors de continguts, si hi ha, i intento propagar permisos
                '*********************************************************************************************                    
                assignaPermisosHeretats2(objconn, rel, llistaPersonesInicial, tractarFillsORG)
            End If
        End If
        DS.Dispose()
    End Sub


    '*********************************************************************************************
    ' Propago permisos cap a nivells inferiors de l'arbre de continguts
    '*********************************************************************************************              

    Private Shared Sub assignaPermisosHeretats2(ByVal objconn As OleDbConnection, ByVal rel As clsRelacio, ByVal llistaPersones As String, ByVal tractarFillsORG As Integer)
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim strsql As String = ""

        If Not String.IsNullOrEmpty(llistaPersones) Then

            ' busco els permisos directes o heretats que té el node superior en la relació o bé els permisos directes de la relació
            ' si llistaPersones valor<>"" busco només els permisos d'aquests usuaris. Això és correcte perque a la relació superior ja tenim calculats els permisos.


            GAIA2.bdr(objconn, "SELECT * FROM METLREL WITH(NOLOCK) WHERE RELCDRSU=" & rel.incod & " AND (RELCDSIT<96  OR RELCDSIT =98)", DS)

            Dim DS2 As DataSet
            Dim dbRow2 As DataRow
            DS2 = New DataSet()

            If DS.Tables(0).Rows.Count > 0 Then
                strsql = "SELECT * FROM METLPER2 WITH(NOLOCK) WHERE PERCDREL=" & rel.incod
                If llistaPersones.Length > 0 Then
                    strsql += " AND PERINORG IN (" + llistaPersones + ")"
                End If
                GAIA2.bdr(objconn, strsql, DS2)


                For Each dbRow In DS.Tables(0).Rows
                    Dim relFill As New clsRelacio()

                    For Each dbRow2 In DS2.Tables(0).Rows
                        relFill.bdget(objconn, dbRow("RELINCOD"))
                        gravaPermis2(objconn, dbRow2("PERINORG"), relFill, 1, dbRow2("PERCDPE1"), dbRow2("PERCDPE2"), dbRow2("PERCDPE3"), dbRow2("PERCDPE4"), dbRow2("PERCDPE5"), dbRow2("PERCDPE6"), dbRow2("PERCDPE7"), dbRow2("PERCDPE8"), dbRow2("PERCDPE9"), dbRow2("PERCDPE10"), 0, False)
                        clsPermisos.tractaPermisosHeretats2(objconn, relFill, relFill.infil, llistaPersones, tractarFillsORG, False, False)
                    Next dbRow2


                Next dbRow
                DS2.Dispose()
            End If
            DS.Dispose()
        End If
    End Sub 'assignaPermisosHeretats2






    Public Shared Sub trobaPermisLlistaRelacions2(ByVal objConn As OleDbConnection, ByVal permis As Integer, ByRef llistaPendents As String, ByVal usuariGAIA As Integer, ByRef grupsAD As String, ByVal herenciaORG As String, ByRef llistaAmbPermisos As String, ByRef llistaSensePermisos As String, ByVal usuariXarxa As String, Optional ByVal pendentsSonNodes As Boolean = False, Optional ByRef llistaNodesSensePermis As String = "")

        Dim cstUsuariTots As Integer = 115969
        Dim llistaUsuarisGaia As String = ""
        Dim llistaGRUPSAD As String = ""
        llistaAmbPermisos = ""
        llistaSensePermisos = ""
        Dim strOrderBy As String = ""
        Dim DS As DataSet
        Dim dbRow As DataRow

        If Not String.IsNullOrEmpty(llistaPendents) Then
            DS = New DataSet()

            If usuariXarxa = "" Then
                If usuariGAIA = 0 Or usuariGAIA = cstUsuariTots Then
                    '******************************************************************************************************************************
                    'Si no hi ha usr xarxa, no puc mostrar continguts que no siguin de web o d'arbre codificació (a més hauré de comprovar els permisos)
                    '******************************************************************************************************************************				
                    If Not pendentsSonNodes Then
                        GAIA2.bdr(Nothing, "SELECT RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELINCOD IN (" & llistaPendents & ") AND (RELCDARB<>8 AND NOT RELCDHER LIKE '_57135%')", DS)
                        For Each dbRow In DS.Tables(0).Rows
                            clsPermisos.eliminarElement(dbRow("RELINCOD"), llistaPendents)
                            clsPermisos.afegirElement(dbRow("RELINCOD"), llistaSensePermisos)

                        Next dbRow
                    Else 'llistapendents són nodes
                        'només moure a "sensepermis" els nodes que no tinguin alguna relació en web(relcdarb=8) o codificació (RELCDHER='_57135%')


                        GAIA2.bdr(Nothing, "SELECT RELINCOD, RELINFIL, RELCDHER FROM METLREL WITH(NOLOCK) WHERE RELINFIL IN (" & llistaPendents & ") AND (RELCDARB=8 OR RELCDHER LIKE '_57135%')", DS)
                        llistaSensePermisos = llistaPendents
                        llistaPendents = ""
                        For Each dbRow In DS.Tables(0).Rows
                            clsPermisos.afegirElement(dbRow("RELINFIL"), llistaPendents)
                            clsPermisos.eliminarElement(dbRow("RELINFIL"), llistaSensePermisos)
                        Next dbRow
                    End If

                End If
            Else
                If usuariGAIA <> cstUsuariTots And usuariGAIA <> 0 Then
                    'afegeixo l'usuari GAIA si hi ha
                    GAIA2.bdr(objConn, "SELECT FORDSWIN FROM METLFOR WITH(NOLOCK) WHERE FORINNOD=" & usuariGAIA & " AND FORINNOD IN (SELECT RELINFIL FROM METLREL WITH(NOLOCK) WHERE RELCDHER LIKE '_56261_115969%' AND RELCDSIT<98)", DS)
                    If DS.Tables(0).Rows.Count > 0 Then
                        usuariXarxa = DS.Tables(0).Rows(0)("FORDSWIN").trim()
                    End If
                End If

            End If



            If Not String.IsNullOrEmpty(llistaPendents) Then


                Dim strSqlCamp As String
                If pendentsSonNodes Then
                    strSqlCamp = "PERINNOD"
                Else
                    strSqlCamp = "PERCDREL"
                End If


                If usuariGAIA > 0 Then
                    llistaUsuarisGaia = usuariGAIA
                    'Busco els nodes organigrama superiors per si hi ha un permís directe a nivell de grup
                    GAIA2.bdr(objConn, "SELECT RELINPAR FROM METLREL WITH(NOLOCK) WHERE RELINFIL=" & usuariGAIA & " AND RELCDHER LIKE '_56261%' AND RELCDSIT<98 ", DS)
                    'GAIA.bdr(objConn, "exec sp_executesql @statement = N'SELECT RELINPAR FROM METLREL WITH(NOLOCK) WHERE RELINFIL=@P1 AND RELCDHER LIKE ''_56261%'' AND RELCDSIT<98', @parameters =N'@P1 varchar(100)',@P1=N'" & usuariGAIA & "'", DS)
                    For Each dbRow In DS.Tables(0).Rows
                        clsPermisos.afegirElement(dbRow("RELINPAR"), llistaUsuarisGaia)
                    Next dbRow
                End If
                'Afegeixo els grups als que pertany segons pertinença a ActiveDirectory
                If usuariXarxa <> "" Then
                    Try
                        grupsAD = clsPermisos.GetGroupsAD(usuariXarxa)
                    Catch
                        grupsAD = ""
                    End Try
                End If



                'Quan un usuari no té usuari GAIA
                'I pertany a més d’un grup AD amb permisos contradictoris.. s’ha de tenir en compte la query que cerca permisos..
                'Hauria de llegirse tots els registres que estiguin al mateix nivell i quedar-se amb el MENYS restrictiu.
                'Si hi ha un grup d’AD que li dona permisos, hauria de poder-se entrar

                'Guardo la llista de nodes organigrama que he de validar per que l'usuari pertany al grup d'AD. Quan calculi permisos, si no hi ha permis directe al usuari, em quedaré amb el menys restrictiu d'aquesta llista
                Dim llistaNORambAD As String = ""

                If Not String.IsNullOrEmpty(grupsAD) Then
                    llistaGRUPSAD = grupsAD.Replace("'", "''").Replace("|TOTS", "").Replace("TOTS", "")
                    llistaGRUPSAD = "'" & llistaGRUPSAD.Replace("|", "','") & "'"

                    If llistaGRUPSAD <> "''" Then
                        GAIA2.bdr(objConn, "SELECT pare.NORDSTIT, isNull(pare.NORINNOD, 0) as norPare, fill.NORDSTIT,fill.NORINNOD as norFill FROM METLNOR as fill with(NOLOCK) INNER JOIN METLREL WITH(NOLOCK) ON RELCDSIT<98 AND RELINFIL=fill.NORINNOD LEFT OUTER JOIN  METLNOR as pare WITH(NOLOCK) ON RELINPAR=pare.NORINNOD WHERE fill.NORDSGAD IN (" & llistaGRUPSAD & ") ", DS)

                        For Each dbRow In DS.Tables(0).Rows
                            clsPermisos.afegirElement(dbRow("norFill"), llistaUsuarisGaia)
                            clsPermisos.afegirElement(dbRow("norFill"), llistaNORambAD)

                            clsPermisos.afegirElement(dbRow("norPare"), llistaUsuarisGaia)
                            clsPermisos.afegirElement(dbRow("norPare"), llistaNORambAD)
                        Next dbRow
                    End If
                End If

                'Afegeixo l'usuari TOTS
                clsPermisos.afegirElement(cstUsuariTots, llistaUsuarisGaia)
                Dim cont As Integer = 1
                For Each item As Integer In llistaUsuarisGaia.Split(",")
                    strOrderBy &= " WHEN " & item & " THEN " & cont
                    cont += 1
                Next item
                If Not String.IsNullOrEmpty(strOrderBy) Then strOrderBy = " ORDER BY " & strSqlCamp & ", CASE PERINORG " & strOrderBy & " END "



                GAIA2.bdr(objConn, "select  * FROM METLPER2 WITH(NOLOCK) WHERE PERINORG IN (" & llistaUsuarisGaia & ") AND " & strSqlCamp & " IN (" & llistaPendents & ") " & strOrderBy, DS)
                Dim relAnt As Integer = 0
                Dim usuAnt As Integer = 0
                Dim permisAnteriorNegatiu As Integer = 0
                For Each dbRow In DS.Tables(0).Rows
                    If relAnt <> dbRow(strSqlCamp) Then
                        'tracto permis
                        permisAnteriorNegatiu = 0
                        relAnt = dbRow(strSqlCamp)
                        clsPermisos.eliminarElement(dbRow(strSqlCamp), llistaPendents)

                        If dbRow("PERCDPE" & permis) >= 1 Then
                            If permis Mod 2 = 1 Then
                                'permis positu
                                clsPermisos.afegirElement(dbRow(strSqlCamp), llistaAmbPermisos)
                            Else
                                'permis negatiu
                                clsPermisos.afegirElement(dbRow(strSqlCamp), llistaSensePermisos)
                                permisAnteriorNegatiu = dbRow("PERINORG")
                            End If
                        Else
                            permisAnteriorNegatiu = dbRow("PERINORG")
                            clsPermisos.afegirElement(dbRow(strSqlCamp), llistaSensePermisos)
                        End If
                    Else 'tinc un altre permis sobre el que ja he tractat. Si és de la llista llistaNORambAD, i el permís que ja he trobat és negatiu i el nou permís és positiu, he de donar permis positiu. Només activo la condició permisAnteriorNegatiu si era un permis sobre un grup i no directe a la persona
                        ' Si el permís ja era positiu, no ho he de tractar
                        If permisAnteriorNegatiu Then
                            If clsPermisos.existeixElement(dbRow("PERINORG"), llistaNORambAD) Or dbRow("PERINORG") = permisAnteriorNegatiu Then
                                If permis Mod 2 = 1 And dbRow("PERCDPE" & permis) >= 1 Then
                                    clsPermisos.eliminarElement(dbRow(strSqlCamp), llistaSensePermisos)
                                    clsPermisos.afegirElement(dbRow(strSqlCamp), llistaAmbPermisos)
                                    permisAnteriorNegatiu = 0
                                End If
                            End If
                        End If
                    End If
                Next dbRow


                'Tots els elements de llistaPendents els assigno a llistaSensePermisos
                clsPermisos.afegirElement(llistaPendents, llistaSensePermisos)
                llistaPendents = ""

                If pendentsSonNodes Then llistaNodesSensePermis = llistaSensePermisos
            End If
            DS.Dispose()
        End If
    End Sub

    Public Shared Function tepermis2(ByVal objConn As OleDbConnection, ByVal permis As Integer, ByVal numNodeOrganigramaini As Integer, ByVal numNodeOrganigrama As Integer, ByVal rel As clsRelacio, ByRef heretat As Integer, Optional ByVal usuariXarxa As String = "", Optional ByVal grupsAD As String = "", Optional ByVal node As Integer = 0) As Integer


        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim llistaAmbPermisos As String = "", llistaSensePermisos As String = "", llistaPendents As String = ""


        tepermis2 = 0

        'cerco permís per node
        If node > 0 Then
            GAIA2.bdr(objConn, "SELECT RELINCOD FROM METLREL With(NOLOCK) WHERE RELINFIL=" & node & " AND RELCDSIT<99 ORDER BY RELCDSIT ASC", DS)
            For Each dbRow In DS.Tables(0).Rows
                If Not String.IsNullOrEmpty(llistaPendents) Then llistaPendents &= ","
                llistaPendents &= dbRow("RELINCOD")
            Next
            DS.Dispose()
        Else
            'cerco permís per relació
            llistaPendents = rel.incod
        End If



        clsPermisos.trobaPermisLlistaRelacions2(objConn, permis, llistaPendents, numNodeOrganigramaini, grupsAD, "", llistaAmbPermisos, llistaSensePermisos, usuariXarxa)
        If llistaAmbPermisos.Length > 0 Then
            tepermis2 = 1
        End If
        Return tepermis2
    End Function




#End Region

End Class


'******************************************************************************************+
'******************************************************************************************+
'******************************************************************************************+
'******************************************************************************************+
'
'				C L A S E     G  A  I  A
'
''******************************************************************************************+
'******************************************************************************************+
'******************************************************************************************+
'******************************************************************************************+



Public Class GAIA2

    Inherits System.Web.UI.Page

    Public Shared llistatErrors As String = ""
    Public Shared llistatWarnings As String = ""
    '*******************************************************+
    ' DECLARACIÓ DE CONSTANTS
    '*******************************************************+
    'Tipus d'accio. Es correspon amb les dades a METLCOD.CODINCOD amb CODINTIP=2
    Public Const TAPUBLICAR As Integer = 8
    Public Const TAINSERTAR As Integer = 9
    Public Const TAESBORRAR As Integer = 10
    Public Const TAMOURE1 As Integer = 11
    Public Const TAMOURE2 As Integer = 19
    Public Const TACANVIESTAT As Integer = 12
    Public Const TACANVIPERMISOS As Integer = 14
    Public Const TACADUCAR As Integer = 20
    Public Const TAAPROVAR As Integer = 21
    Public Const TADENEGAR As Integer = 21
    Public Const TAMODIFICAR As Integer = 41
    Public Const TAREVISAR As Integer = 42
    Public Const TALLEGIR As Integer = 43

    'Tipus d'estat d'una relació
    Public Const ctINICIAL As Integer = 1
    Public Const ctPENDENTPUBLICAR As Integer = 2 's'ha demanat la publicació i està a l'espera de que arribi la data de publicació.
    Public Const ctPUBLICAR As Integer = 3 'Publicat o preparat per a publicar 

    Public Const ctPENDENTCADUCAR As Integer = 4     ' = a pendent de caducar
    Public Const ctESBORRATCADUCAT As Integer = 98
    Public Const ctESBORRATMANUAL As Integer = 99
    Public Const ctIMATGE As Integer = 96
    Public Const ctDOCUMENT As Integer = 97

    Public Const ctESBORRATMANUALIMATGE As Integer = 9997
    Public Const ctESBORRATMANUALDOCUMENT As Integer = 9996
    Public Const ctESBORRATCADUCATIMATGE As Integer = 9897
    Public Const ctESBORRATCADUCATDOCUMENT As Integer = 9896





    Public Const ctPENDENTPUBLICARCIRCUIT As Integer = 6 's'ha demanat la publicació i està a l'espera d'una aprovació
    Public Const ctPUBLICARCIRCUIT As Integer = 7 'Preparat per a publicar. S'ha aprovat dins d'un circuit)
    Public Const ctPENDENTCADUCARCIRCUIT As Integer = 8  ' = a pendent de caducar, pendent d'una aprovació de caducitat

    Public Shared fitxerXML As StreamWriter



    'Funció  sense el paràmetre "tipus"
    Public Shared Sub generaArbre(ByVal objconn As OleDbConnection, ByVal objArbre As RadTreeView, ByVal nroArbre As Integer, ByVal idUsuari As Integer, ByVal rel As clsRelacio, ByVal nomArbre As String, ByVal recursiu As Integer, ByVal nivells As Integer)
        GAIA2.generaArbre(objconn, objArbre, nroArbre, idUsuari, rel, nomArbre, 0, recursiu, nivells, 0)

    End Sub


    Public Shared Sub generaArbre(ByVal objconn As OleDbConnection, ByVal objArbre As RadTreeView, ByVal nroArbre As Integer, ByVal idUsuari As Integer, ByRef rel As clsRelacio, ByVal nomArbre As String, ByVal tipus As Integer, ByVal recursiu As Integer, ByVal nivells As Integer)
        GAIA2.generaArbre(objconn, objArbre, nroArbre, idUsuari, rel, nomArbre, 0, recursiu, nivells, 0)
    End Sub
    '***************************************************************************************************
    '	Funció: GAIA.generaArbre
    '	Entrada: 
    '				objconn: connexió a la bd.
    '				objArbre: objecte que apunta a l'arbre de la pantalla
    '				nroArbre: Nro d'arbres en pantalla, 1 o 2.
    '				idUsuari: Usuari que vol obrir l'arbre
    '				codiRelacio: Codi de la relació que apunta al node on volem obrir un arbre
    '				nomArbre: nom de l'arbre que volem obrir (si codirelacio=0 llavors utilitzem el nom de l'arbre)
    '				recursiu: 1 si volem obrir l'arbre sencer
    '				tipus: Tipus de nodes que vull afegir. Si =0 afegeixo qualsevol 
    '				novisibles: si 1 no es mostraran els continguts no visibles (RELSWVIS=0)
    '						
    '	Procés: 
    '***************************************************************************************************	

    Public Shared Sub generaArbre(ByVal objconn As OleDbConnection, ByVal objArbre As RadTreeView, ByVal nroArbre As Integer, ByVal idUsuari As Integer, ByRef rel As clsRelacio, ByVal nomArbre As String, ByVal tipus As Integer, ByVal recursiu As Integer, ByVal nivells As Integer, ByVal novisibles As Boolean)
        Dim strErr As String = ""

        Dim DS As DataSet
        Dim node As RadTreeNode
        Dim dbRow As DataRow
        Dim codiRelacio As Integer
        Dim relTmp As New clsRelacio

        DS = New DataSet()
        codiRelacio = rel.incod

        If codiRelacio = 0 Then
            If nomArbre = "organigramacserveis" Then
                nomArbre = "organigrama cataleg de serveis"

            End If
            GAIA2.bdr(objconn, "SELECT RELINCOD FROM METLREL WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE (NODDSTXT like 'arbre " + nomArbre.ToString() + "' OR NODDSTXT like '" + nomArbre.ToString() + "') AND RELINPAR=NODINNOD AND RELINPAR=RELINFIL AND RELCDSIT<98 " + " AND RELCDSIT<>" + ctESBORRATCADUCAT.ToString() & IIf(novisibles, " AND RELSWVIS=1", ""), DS)

            If DS.Tables(0).Rows.Count > 0 Then
                dbRow = DS.Tables(0).Rows(0)
                rel.bdget(objconn, dbRow("RELINCOD"))
                codiRelacio = rel.incod
            End If
        End If

        objArbre.Nodes.Clear
        If nroArbre = 1 Then
            objArbre.RetainScrollPosition = True
        Else
            objArbre.RetainScrollPosition = False
        End If
        GAIA2.bdr(objconn, "SELECT * FROM METLNOD WITH(NOLOCK), METLREL WITH(NOLOCK) WHERE RELINCOD=" & codiRelacio & " AND RELINFIL=NODINNOD AND RELCDSIT<98  AND RELCDSIT<>" & ctESBORRATCADUCAT & IIf(novisibles, " AND RELSWVIS=1", "") & " ORDER BY NODINNOD DESC", DS)

        'GAIA.bdr(objconn, "exec sp_executesql @statement = N'SELECT * FROM METLNOD WITH(NOLOCK), METLREL WITH(NOLOCK) WHERE RELINCOD=@P1 AND RELINFIL=NODINNOD AND RELCDSIT<98   ORDER BY NODINNOD DESC ', @parameters =N'@P1 varchar(100)',@P1=N'" & codiRelacio & "'", DS)		
        For Each dbRow In DS.Tables(0).Rows

            relTmp.bdget(objconn, dbRow("RELINCOD"))
            If clsPermisos.tePermisLecturaNode(objconn, rel, idUsuari, dbRow("NODINNOD")) Then
                node = GAIA2.creaNodePantalla(objconn, dbRow("NODDSTXT"), dbRow("NODINNOD").ToString() & "-" & dbRow("RELCDARB").ToString() & "_" & dbRow("RELINCOD"), "arbre", "", 0, idUsuari, relTmp, 0, 0)
                If recursiu Then
                    node.ExpandMode = ExpandMode.ClientSide

                Else
                    If nivells > 1 Then
                        node.ExpandMode = ExpandMode.ClientSide
                    Else
                        node.ExpandMode = ExpandMode.ServerSide
                    End If
                End If
                objArbre.AddNode(node)
                'Afegeixo nodes 
                If recursiu = 1 Then

                    GAIA2.afegeixNodesFillsPantallaLlista(objconn, dbRow("NODINNOD"), node, dbRow("RELCDARB"), idUsuari, nroArbre, tipus, recursiu, nivells, False, novisibles)

                    objArbre.CollapseAllNodes()
                Else
                    If nivells > 1 Then

                        GAIA2.afegeixNodesFillsPantallaLlista(objconn, dbRow("NODINNOD"), node, dbRow("RELCDARB"), idUsuari, nroArbre, tipus, recursiu, nivells - 1, False, novisibles)
                        objArbre.CollapseAllNodes()
                    End If
                End If
            End If
        Next dbRow
        If (objArbre.IsEmpty) Then
            objArbre.Visible = False
        Else
            objArbre.Visible = True
        End If
        DS.Dispose()
    End Sub 'generaArbre





    Public Shared Sub generaArbre2(ByVal objconn As OleDbConnection, ByVal objArbre As RadTreeView, ByVal nroArbre As Integer, ByVal idUsuari As Integer, ByRef rel As clsRelacio, ByVal nomArbre As String, ByVal tipus As Integer, ByVal recursiu As Integer, ByVal nivells As Integer, ByVal novisibles As Boolean)
        Dim strErr As String = ""

        Dim DS As DataSet

        Dim strTmp As String = ""

        Dim dbRow As DataRow
        Dim codiRelacio As String
        Dim relTmp As New clsRelacio
        Dim cont As Integer = 1
        DS = New DataSet()
        codiRelacio = rel.incod

        If codiRelacio = "0" Then
            If nomArbre = "organigramacserveis" Then
                nomArbre = "organigrama cataleg de serveis"
            End If
            GAIA2.bdr(objconn, "SELECT RELINCOD FROM METLREL WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE (NODDSTXT like 'arbre " + nomArbre.ToString() + "' OR NODDSTXT like '" + nomArbre.ToString() + "') AND RELINPAR=NODINNOD AND RELINPAR=RELINFIL AND RELCDSIT<98 " + " AND RELCDSIT<>" + ctESBORRATCADUCAT.ToString() & IIf(novisibles, " AND RELSWVIS=1", ""), DS)

            If DS.Tables(0).Rows.Count > 0 Then
                dbRow = DS.Tables(0).Rows(0)
                rel.bdget(objconn, dbRow("RELINCOD"))
                codiRelacio = rel.incod
            End If
        End If

        objArbre.Nodes.Clear
        If nroArbre = 1 Then
            objArbre.RetainScrollPosition = True
        Else
            objArbre.RetainScrollPosition = False
        End If

        GAIA2.bdr(objconn, "SELECT * FROM METLNOD WITH(NOLOCK), METLREL WITH(NOLOCK), METLTIP WITH(NOLOCK) WHERE RELINCOD=" & codiRelacio & " AND RELINFIL=NODINNOD AND RELCDSIT<98   AND TIPINTIP=NODCDTIP ORDER BY NODINNOD DESC", DS)
        For Each dbRow In DS.Tables(0).Rows
            GAIA2.ObteFillsNodeAmbPermisos2(objconn, objArbre, idUsuari, rel, nomArbre, tipus, recursiu, nivells, dbRow("NODINNOD"), dbRow("TIPDSDES"))

        Next dbRow
        If (objArbre.IsEmpty) Then
            objArbre.Visible = False
        Else
            objArbre.Visible = True
        End If
        DS.Dispose()
    End Sub 'generaArbre2



    '******************************************************************************************************************
    ' fa una filtre dels nodes segons permisos i retorna la part d'arbre amb els nivells demanats
    '******************************************************************************************************************

    Public Shared Sub ObteFillsNodeAmbPermisos2(ByVal objconn As OleDbConnection, ByVal objArbre As RadTreeView, ByVal idUsuari As Integer, ByRef rel As clsRelacio, ByVal nomArbre As String, ByVal tipus As Integer, ByVal recursiu As Integer, ByVal nivells As Integer, ByVal codiNode As Integer, ByVal tipdsdes As String)
        Dim strErr As String = ""

        Dim DS As DataSet

        Dim strTmp As String = ""
        Dim node As RadTreeNode
        Dim dbRow, dbrowTmp As DataRow
        Dim codiRelacio As String
        Dim relTmp As New clsRelacio
        Dim cont As Integer = 1
        DS = New DataSet()
        codiRelacio = rel.incod


        GAIA2.bdr(objconn, "select METLREL.RELINCOD FROM METLREL WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE (METLREL.RELINCOD=" & codiRelacio & " OR METLREL.RELCDHER LIKE '%_" & codiNode & "%') AND METLREL.RELCDSIT<95 AND RELINFIL=NODINNOD " & IIf(tipus > 0, " AND NODCDTIP=" & tipus, "") & " AND ((LEN(RELCDHER)-LEN(replace(RELCDHER,'_','')))<" & nivells + (rel.cdher.Split("_").Length) & ")", DS)

        Dim llistaRel As String = ""
        Dim llistaAmbPermisos As String = ""
        Dim hsNodes As New Hashtable()
        For Each dbRow In DS.Tables(0).Rows
            If llistaRel.Length > 0 Then llistaRel &= ","
            llistaRel &= dbRow("RELINCOD")
        Next dbRow
        clsPermisos.trobaPermisLlistaRelacions(objconn, 9, llistaRel, idUsuari, "", 0, llistaAmbPermisos, "")
        If llistaAmbPermisos.Length > 0 Then
            GAIA2.bdr(objconn, "select DISTINCT METLREL.RELCDRSU,METLREL.RELINFIL, METLREL.RELINCOD, METLREL.RELCDARB, METLREL.RELCDORD, METLNOD.NODINNOD, METLNOD.NODDSTXT FROM METLREL WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE (METLREL.RELINCOD IN (" & llistaAmbPermisos & ")  OR RELINCOD=" & rel.incod & ") AND RELINFIL=NODINNOD ORDER BY RELCDORD  ", DS)
            For Each dbRow In DS.Tables(0).Rows
                If hsNodes.Contains(dbRow("RELCDRSU").ToString()) Then
                    strTmp = hsNodes(dbRow("RELCDRSU").ToString())
                    hsNodes.Remove(dbRow("RELCDRSU").ToString())
                    hsNodes.Add(dbRow("RELCDRSU").ToString(), strTmp & "," & dbRow("RELINCOD"))
                Else
                    hsNodes.Add(dbRow("RELCDRSU").ToString(), dbRow("RELINCOD"))
                End If
            Next dbRow

            'en hsNodes tinc una relació de RELINPAR amb els RELINFIL que he de representar en l'arbre
            'Començo per la relació que hi ha a "codirelacio" i baixo el nro de nivells demanat

            If hsNodes.Contains(codiRelacio.ToString()) Then
                dbrowTmp = DS.Tables(0).Select("RELINCOD=" & codiRelacio)(0)
                relTmp.bdget(objconn, codiRelacio)
                node = GAIA2.creaNodePantalla(objconn, dbrowTmp("NODDSTXT"), dbrowTmp("NODINNOD").ToString() & "-" & dbrowTmp("RELCDARB").ToString() & "_" & dbrowTmp("RELINCOD"), tipdsdes, "", 0, idUsuari, relTmp, 0, 0)

                GAIA2.afegeixNodesFillsPantallaLlista2(objconn, codiRelacio, node, dbrowTmp("RELCDARB"), recursiu, nivells, hsNodes, DS, idUsuari, 1, 0)
                objArbre.AddNode(node)
            End If

        End If
        DS.Dispose()
    End Sub 'ObteFillsNodeAmbPermisos2




    Public Shared Sub generaArbre_NP(ByVal objconn As OleDbConnection, ByVal objArbre As RadTreeView, ByVal nroArbre As Integer, ByVal idUsuari As Integer, ByRef rel As clsRelacio, ByVal nomArbre As String, ByVal tipus As Integer, ByVal recursiu As Integer, ByVal nivells As Integer, ByVal novisibles As Boolean)
        Dim strErr As String = ""

        Dim DS As DataSet
        Dim node As RadTreeNode
        Dim dbRow As DataRow
        Dim codiRelacio As Integer
        Dim relTmp As New clsRelacio
        DS = New DataSet()
        codiRelacio = rel.incod

        If codiRelacio = 0 Then
            If nomArbre = "organigramacserveis" Then
                nomArbre = "organigrama cataleg de serveis"

            End If
            GAIA2.bdr(objconn, "SELECT RELINCOD FROM METLREL WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE (NODDSTXT like 'arbre " + nomArbre.ToString() + "' OR NODDSTXT like '" + nomArbre.ToString() + "') AND RELINPAR=NODINNOD AND RELINPAR=RELINFIL AND RELCDSIT<98 " + " AND RELCDSIT<>" + ctESBORRATCADUCAT.ToString() & IIf(novisibles, " AND RELSWVIS=1", ""), DS)

            If DS.Tables(0).Rows.Count > 0 Then
                dbRow = DS.Tables(0).Rows(0)
                rel.bdget(objconn, dbRow("RELINCOD"))
                codiRelacio = rel.incod
            End If
        End If

        objArbre.Nodes.Clear
        If nroArbre = 1 Then
            objArbre.RetainScrollPosition = True
        Else
            objArbre.RetainScrollPosition = False
        End If
        GAIA2.bdr(objconn, "SELECT * FROM METLNOD WITH(NOLOCK), METLREL WITH(NOLOCK) WHERE RELINCOD=" & codiRelacio & " AND RELINFIL=NODINNOD AND RELCDSIT<98  AND RELCDSIT<>" & ctESBORRATCADUCAT & IIf(novisibles, " AND RELSWVIS=1", "") & " ORDER BY NODINNOD DESC", DS)

        'GAIA.bdr(objconn, "exec sp_executesql @statement = N'SELECT * FROM METLNOD WITH(NOLOCK), METLREL WITH(NOLOCK) WHERE RELINCOD=@P1 AND RELINFIL=NODINNOD AND RELCDSIT<98   ORDER BY NODINNOD DESC ', @parameters =N'@P1 varchar(100)',@P1=N'" & codiRelacio & "'", DS)		
        For Each dbRow In DS.Tables(0).Rows

            relTmp.bdget(objconn, dbRow("RELINCOD"))
            If clsPermisos.tePermisLecturaNode(objconn, rel, idUsuari, dbRow("NODINNOD")) Then
                node = GAIA2.creaNodePantalla(objconn, dbRow("NODDSTXT"), dbRow("NODINNOD").ToString() & "-" & dbRow("RELCDARB").ToString() & "_" & dbRow("RELINCOD"), "arbre", "", 0, idUsuari, relTmp, 0, 0)
                If recursiu Then
                    node.ExpandMode = ExpandMode.ClientSide

                Else
                    If nivells > 1 Then
                        node.ExpandMode = ExpandMode.ClientSide
                    Else
                        node.ExpandMode = ExpandMode.ServerSide
                    End If
                End If
                objArbre.AddNode(node)
                'Afegeixo nodes 
                If recursiu = 1 Then

                    GAIA2.afegeixNodesFillsPantallaLlista_NP(objconn, dbRow("NODINNOD"), node, dbRow("RELCDARB"), idUsuari, nroArbre, tipus, recursiu, nivells, False, novisibles)

                    objArbre.CollapseAllNodes()
                Else
                    If nivells > 1 Then

                        GAIA2.afegeixNodesFillsPantallaLlista_NP(objconn, dbRow("NODINNOD"), node, dbRow("RELCDARB"), idUsuari, nroArbre, tipus, recursiu, nivells - 1, False, novisibles)
                        objArbre.CollapseAllNodes()
                    End If
                End If
            End If
        Next dbRow
        If (objArbre.IsEmpty) Then
            objArbre.Visible = False
        Else
            objArbre.Visible = True
        End If
        DS.Dispose()
    End Sub 'generaArbre


    '******************************************************************************************************************
    ' fa una filtre dels nodes segons permisos i retorna la part d'arbre amb els nivells demanats
    '******************************************************************************************************************

    Public Shared Sub ObteFillsNodeAmbPermisos_NP(ByVal objconn As OleDbConnection, ByVal objArbre As RadTreeView, ByVal objNode As RadTreeNode, ByVal idUsuari As Integer, ByRef rel As clsRelacio, ByVal nomArbre As String, ByVal tipus As Integer, ByVal recursiu As Integer, ByVal nivells As Integer, ByVal codiNode As Integer, ByVal tipdsdes As String)
        Dim strErr As String = ""

        Dim DS As DataSet

        Dim strTmp As String = ""
        Dim node As RadTreeNode
        Dim dbRow, dbrowTmp As DataRow
        Dim codiRelacio As String
        Dim relTmp As New clsRelacio
        Dim cont As Integer = 1
        DS = New DataSet()
        codiRelacio = rel.incod


        GAIA2.bdr(objconn, "select METLREL.RELINCOD FROM METLREL WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE (METLREL.RELINCOD=" & codiRelacio & " OR METLREL.RELCDHER LIKE '%_" & codiNode & "%') AND METLREL.RELCDSIT<95 AND RELINFIL=NODINNOD " & IIf(tipus > 0, " AND NODCDTIP=" & tipus, "") & " AND ((LEN(RELCDHER)-LEN(replace(RELCDHER,'_','')))<" & nivells & (rel.cdher.Split("_").Length) & ")", DS)

        Dim llistaRel As String = ""
        Dim llistaAmbPermisos As String = ""
        Dim hsNodes As New Hashtable()
        For Each dbRow In DS.Tables(0).Rows
            If llistaRel.Length > 0 Then llistaRel &= ","
            llistaRel &= dbRow("RELINCOD")
        Next dbRow

        clsPermisos.trobaPermisLlistaRelacions2(objconn, 9, llistaRel, idUsuari, "", 0, llistaAmbPermisos, "", "")
        If llistaAmbPermisos.Length > 0 Then
            GAIA2.bdr(objconn, "select DISTINCT METLREL.RELCDRSU,METLREL.RELINFIL, METLREL.RELINCOD, METLREL.RELCDARB, METLNOD.NODINNOD, METLNOD.NODDSTXT FROM METLREL WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE (METLREL.RELINCOD IN (" & llistaAmbPermisos & ")  OR RELINCOD=" & rel.incod & ") AND RELINFIL=NODINNOD ", DS)
            For Each dbRow In DS.Tables(0).Rows
                If hsNodes.Contains(dbRow("RELCDRSU").ToString()) Then
                    strTmp = hsNodes(dbRow("RELCDRSU").ToString())
                    hsNodes.Remove(dbRow("RELCDRSU").ToString())
                    hsNodes.Add(dbRow("RELCDRSU").ToString(), strTmp & "," & dbRow("RELINCOD"))
                Else
                    hsNodes.Add(dbRow("RELCDRSU").ToString(), dbRow("RELINCOD"))
                End If
            Next dbRow

            'en hsNodes tinc una relació de RELINPAR amb els RELINFIL que he de representar en l'arbre
            'Començo per la relació que hi ha a "codirelacio" i baixo el nro de nivells demanat

            If hsNodes.Contains(codiRelacio.ToString()) Then
                dbrowTmp = DS.Tables(0).Select("RELINCOD=" & codiRelacio)(0)
                relTmp.bdget(objconn, codiRelacio)
                node = GAIA2.creaNodePantalla(objconn, dbrowTmp("NODDSTXT"), dbrowTmp("NODINNOD").ToString() & "-" & dbrowTmp("RELCDARB").ToString() & "_" & dbrowTmp("RELINCOD"), tipdsdes, "", 0, idUsuari, relTmp, 0, 0)

                GAIA2.afegeixNodesFillsPantallaLlista2(objconn, codiRelacio, node, dbrowTmp("RELCDARB"), recursiu, nivells, hsNodes, DS, idUsuari, 1, 0)
                If objNode Is Nothing Then
                    objArbre.AddNode(node)
                Else
                    objNode.AddNode(node)
                End If
            End If

        End If
        DS.Dispose()
    End Sub 'ObteFillsNodeAmbPermisos_NP





    Public Shared Sub afegeixNodesFillsPantallaLlista2(ByVal objConn As OleDbConnection, ByVal codiRelacio As String, ByRef nodePare As RadTreeNode, ByVal codiArbre As Integer, ByVal recursiu As Integer, ByVal nivells As Integer, ByVal hsNodes As Hashtable, ByVal ds As DataSet, ByVal idUsuari As Integer, ByVal veureTots As Integer, ByVal nroMaxim As Integer)
        ' If nivells > 0 Or recursiu Then
        Dim node As RadTreeNode
        Dim dbrowTmp As DataRow
        Dim rel As New clsRelacio()
        Dim relaciofill As Integer

        Dim cont As Integer = 0

        If hsNodes.Contains(codiRelacio) Then
            For Each relaciofill In hsNodes(codiRelacio).ToString().Split(",")  'he d'agregar tots els fills al node pare
                dbrowTmp = ds.Tables(0).Select("RELINCOD=" & relaciofill)(0)
                rel.bdget(objConn, relaciofill)
                node = GAIA2.creaNodePantalla(objConn, dbrowTmp("NODDSTXT"), dbrowTmp("NODINNOD").ToString() & "-" & dbrowTmp("RELCDARB").ToString() & "_" & dbrowTmp("RELINCOD"), rel.tipdsdes, "", 0, idUsuari, rel, 0, 150)
                If recursiu = 1 Then
                    GAIA2.afegeixNodesFillsPantallaLlista2(objConn, relaciofill, node, dbrowTmp("RELCDARB"), recursiu, nivells - 1, hsNodes, ds, idUsuari, veureTots, nroMaxim)
                    node.Expanded = False
                    'expand d'un node no fa acció al servidor
                    node.ExpandMode = ExpandMode.ClientSide
                Else
                    If nivells > 1 Then
                        GAIA2.afegeixNodesFillsPantallaLlista2(objConn, relaciofill, node, dbrowTmp("RELCDARB"), recursiu, nivells - 1, hsNodes, ds, idUsuari, veureTots, nroMaxim)
                        node.ExpandMode = ExpandMode.ClientSide
                        node.Expanded = False
                    Else
                        node.ExpandMode = ExpandMode.ServerSide
                    End If
                End If


                nodePare.AddNode(node)
                cont += 1
                If nroMaxim > 0 And cont > nroMaxim And veureTots = 0 Then
                    Exit For
                End If
            Next relaciofill
        End If
        '  End If

    End Sub

    '******************************************************************
    '	Funció: afegeixNodesFillsPantalla
    '	Entrada: 
    '		nodePare: codi del node que volem expandir
    '		node: estructura que apunta al node que hi ha a la pantalla
    '		codiarbre: codi de l'arbre que hi ha a la pantalla
    '		idUsuari: usuari que vol expandir el node. Si idUsuari =0 llavors obriré el node sense mirar els permisos.
    '							Això ho utilitzaré, de moment, per obrir l'arbre dintre de la pantalla de permisos.
    '		nroArbre: arbre de la pantalla on afegiré els fills
    '		tipus: Tipus de nodes que vull afegir. Si =0 afegeixo qualsevol 

    '		nivells: nro de nivells que volem obrir. per defecte=0 --> 1 nivell
    '	Procés: 	
    '		Expandeix el node seleccionat.
    '	Sortida:
    '		Codi del tipus de node
    '******************************************************************
    'Public Shared  Sub afegeixNodesFillsPantalla(byVal objConn as OleDbConnection, byVal nodePare As integer, ByRef node As RadTreeNode, byVal codiArbre as integer, byVal idUsuari as Integer, byVal nroArbre as integer, byVal tipus as integer,byVal nivells as integer)
    '	GAIA.afegeixNodesFillsPantalla(objconn, nodePare, node, codiArbre, idUsuari,nroArbre,tipus,0,nivells)
    'End Sub 


    Public Shared Sub afegeixNodesFillsPantalla(ByVal objConn As OleDbConnection, ByVal nodePare As Integer, ByRef node As RadTreeNode, ByVal codiArbre As Integer, ByVal idUsuari As Integer, ByVal nroArbre As Integer, ByVal tipus As Integer, ByVal recursiu As Integer, ByVal nivells As Integer)
        afegeixNodesFillsPantallaLlista(objConn, nodePare, node, codiArbre, idUsuari, nroArbre, tipus, recursiu, nivells, False, False)


    End Sub 'afegeixNodesFillsPantalla

    'funció a partir de "afegeixNodesFillsPantalla" per tal de treballar els permisos en llista, en comptes d'un a un.

    Public Shared Sub afegeixNodesFillsPantallaLlista(ByVal objConn As OleDbConnection, ByVal nodePare As Integer, ByRef node As RadTreeNode, ByVal codiArbre As Integer, ByVal idUsuari As Integer, ByVal nroArbre As Integer, ByVal tipus As Integer, ByVal recursiu As Integer, ByVal nivells As Integer)
        afegeixNodesFillsPantallaLlista(objConn, nodePare, node, codiArbre, idUsuari, nroArbre, tipus, recursiu, nivells, False, False)
    End Sub

    Public Shared Sub afegeixNodesFillsPantallaLlista(ByVal objConn As OleDbConnection, ByVal nodePare As Integer, ByRef node As RadTreeNode, ByVal codiArbre As Integer, ByVal idUsuari As Integer, ByVal nroArbre As Integer, ByVal tipus As Integer, ByVal recursiu As Integer, ByVal nivells As Integer, ByVal veuretots As Boolean)
        afegeixNodesFillsPantallaLlista(objConn, nodePare, node, codiArbre, idUsuari, nroArbre, tipus, recursiu, nivells, veuretots, False)
    End Sub

    Public Shared Sub afegeixNodesFillsPantallaLlista(ByVal objConn As OleDbConnection, ByVal nodePare As Integer, ByRef node As RadTreeNode, ByVal codiArbre As Integer, ByVal idUsuari As Integer, ByVal nroArbre As Integer, ByVal tipus As Integer, ByVal recursiu As Integer, ByVal nivells As Integer, ByVal veuretots As Boolean, ByVal novisibles As Boolean)

        Dim DS As DataSet
        Dim dbRow As DataRow

        Dim strSql As String

        DS = New DataSet()
        '		Dim tmpstr as String
        Dim relacioPare As Integer
        Dim pathRelacioPare As String
        Dim nodeRoot As Integer
        Dim relTMP As New clsRelacio
        Dim grupsAD As String = ""
        Dim llistaRel As String
        Dim item As String
        Dim aRel As String()
        Dim usuariXarxa As String = ""
        Dim llistaSensePermisos As String = ""
        Dim llistaAmbPermisos As String = ""
        Dim nroMaxim As Integer = 150
        Dim cont As Integer = 0

        Dim relPare As New clsRelacio
        Dim heretat As Integer = 0
        relacioPare = GAIA2.obtenirRelacioPantalla(node)
        relPare.bdget(objConn, relacioPare)
        pathRelacioPare = relPare.cdher
        ' poso els fills del node seleccionat	

        strSql = "SELECT NODSWSIT,RELINCOD, NODCDTIP FROM  METLREL WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE  RELINPAR<>RELINFIL AND RELCDRSU=" & relPare.incod & " AND (RELCDSIT<95  OR RELCDSIT=98 AND NODSWSIT=1) " & IIf(novisibles, " AND RELSWVIS=1", "") & " AND NODINNOD=RELINPAR "
        If tipus > 0 Then
            strSql &= " AND NODCDTIP=" & tipus
        End If
        strSql += " ORDER BY RELCDORD ASC"

        GAIA2.bdr(objConn, strSql, DS)
        If pathRelacioPare.Length = 0 Then
            nodeRoot = nodePare
        Else
            If pathRelacioPare.LastIndexOf("_") = 0 Then
                nodeRoot = pathRelacioPare.Substring(1)
            Else
                nodeRoot = pathRelacioPare.Substring(1, pathRelacioPare.IndexOf("_", 1) - 1)
            End If
        End If
        llistaRel = ""
        For Each dbRow In DS.Tables(0).Rows
            If llistaRel <> "" Then llistaRel &= ","
            llistaRel &= dbRow("RELINCOD")
        Next dbRow
        'response.write("llista Inicial: " & llistaRel)
        Dim llistaRelTmp As String
        llistaRelTmp = llistaRel
        If idUsuari <> 0 Then
            clsPermisos.trobaPermisLlistaRelacions(objConn, 9, llistaRel, idUsuari, grupsAD, 0, llistaAmbPermisos, llistaSensePermisos)
            llistaRel = llistaAmbPermisos
        End If

        If Trim(llistaRel) <> "" Then
            aRel = Split(llistaRel, ",")
            DS = New DataSet
            strSql = "SELECT RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELINCOD IN (" & llistaRel & ") ORDER BY RELCDORD"
            GAIA2.bdr(objConn, strSql, DS)

            'Si estic en l'arbre codificació i només per notícies, agafo només les darreres 50 codificades
            cont = 0
            If Not veuretots Then
                If nroMaxim < DS.Tables(0).Rows.Count Then
                    cont = DS.Tables(0).Rows.Count - nroMaxim
                End If
            Else
                nroMaxim = 0
            End If
            Dim i As Integer

            For i = cont To DS.Tables(0).Rows.Count - 1

                dbRow = DS.Tables(0).Rows(i)
                item = dbRow("RELINCOD")
                relTMP.bdget(objConn, item)
                Dim childNode As RadTreeNode = GAIA2.creaNodePantalla(objConn, relTMP.noddstxt, relTMP.infil & "-" & relTMP.cdarb & "_" & relTMP.incod, relTMP.tipdsdes, "", GAIA2.nroFills(objConn, relTMP), idUsuari, relTMP, relTMP.pcrincod.ToString(), nroMaxim)

                If recursiu = 1 Then

                    afegeixNodesFillsPantallaLlista(objConn, relTMP.infil, childNode, codiArbre, idUsuari, nroArbre, tipus, recursiu, nivells, veuretots, novisibles)
                    childNode.Expanded = False
                    'expand d'un node no fa acció al servidor
                    childNode.ExpandMode = ExpandMode.ClientSide
                Else
                    If nivells > 1 Then
                        afegeixNodesFillsPantallaLlista(objConn, relTMP.incod, childNode, codiArbre, idUsuari, nroArbre, tipus, -1, nivells - 1, veuretots, novisibles)
                        childNode.ExpandMode = ExpandMode.ClientSide
                        childNode.Expanded = False
                    Else
                        childNode.ExpandMode = ExpandMode.ServerSide
                    End If
                End If
                node.AddNode(childNode)
            Next i
        End If

        DS.Dispose()
    End Sub 'afegeixNodesFillsPantallaLlista



    Public Shared Sub afegeixNodesFillsPantallaLlista_NP(ByVal objConn As OleDbConnection, ByVal nodePare As Integer, ByRef node As RadTreeNode, ByVal codiArbre As Integer, ByVal idUsuari As Integer, ByVal nroArbre As Integer, ByVal tipus As Integer, ByVal recursiu As Integer, ByVal nivells As Integer, ByVal veuretots As Boolean, ByVal novisibles As Boolean, Optional ByVal veureCaducats As Boolean = False)

        Dim DS As DataSet
        Dim dbRow As DataRow

        Dim strSql As String

        DS = New DataSet()
        '		Dim tmpstr as String
        Dim relacioPare As Integer
        Dim pathRelacioPare As String
        Dim nodeRoot As Integer
        Dim relTMP As New clsRelacio
        Dim grupsAD As String = ""
        Dim llistaRel As String
        Dim item As String
        Dim aRel As String()
        Dim usuariXarxa As String = ""
        Dim llistaSensePermisos As String = ""
        Dim llistaAmbPermisos As String = ""
        Dim nroMaxim As Integer = 150
        Dim cont As Integer = 0

        Dim relPare As New clsRelacio
        Dim heretat As Integer = 0
        relacioPare = GAIA2.obtenirRelacioPantalla(node)
        relPare.bdget(objConn, relacioPare)
        pathRelacioPare = relPare.cdher
        ' poso els fills del node seleccionat	

        strSql = "SELECT NODSWSIT,RELINCOD, NODCDTIP FROM  METLREL WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE  RELINPAR<>RELINFIL AND RELCDRSU=" & relPare.incod & " AND (RELCDSIT<95  OR (RELCDSIT=98 AND NODSWSIT=1) " & IIf(veureCaducats, " or (RELCDSIT=98)", "") & ")" & IIf(novisibles, " AND RELSWVIS=1", "") & " AND NODINNOD=RELINPAR "
        If tipus > 0 Then
            strSql &= " AND NODCDTIP=" & tipus
        End If
        strSql += "ORDER BY RELCDORD ASC"

        GAIA2.bdr(objConn, strSql, DS)
        If pathRelacioPare.Length = 0 Then
            nodeRoot = nodePare
        Else
            If pathRelacioPare.LastIndexOf("_") = 0 Then
                nodeRoot = pathRelacioPare.Substring(1)
            Else
                nodeRoot = pathRelacioPare.Substring(1, pathRelacioPare.IndexOf("_", 1) - 1)
            End If
        End If
        llistaRel = ""
        For Each dbRow In DS.Tables(0).Rows
            If llistaRel <> "" Then llistaRel &= ","
            llistaRel &= dbRow("RELINCOD")
        Next dbRow
        'response.write("llista Inicial: " & llistaRel)
        Dim llistaRelTmp As String
        llistaRelTmp = llistaRel
        If idUsuari <> 0 Then


            clsPermisos.trobaPermisLlistaRelacions2(objConn, 9, llistaRel, idUsuari, grupsAD, 0, llistaAmbPermisos, llistaSensePermisos, "")
            llistaRel = llistaAmbPermisos
        End If

        If Trim(llistaRel) <> "" Then
            aRel = Split(llistaRel, ",")
            DS = New DataSet
            strSql = "SELECT RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELINCOD IN (" & llistaRel & ") ORDER BY RELCDORD"
            GAIA2.bdr(objConn, strSql, DS)

            'Si estic en l'arbre codificació i només per notícies, agafo només les darreres 50 codificades
            cont = 0
            If Not veuretots Then
                If nroMaxim < DS.Tables(0).Rows.Count Then
                    cont = DS.Tables(0).Rows.Count - nroMaxim
                End If
            Else
                nroMaxim = 0
            End If
            Dim i As Integer

            For i = cont To DS.Tables(0).Rows.Count - 1

                dbRow = DS.Tables(0).Rows(i)
                item = dbRow("RELINCOD")
                relTMP.bdget(objConn, item)
                Dim childNode As RadTreeNode = GAIA2.creaNodePantalla(objConn, relTMP.noddstxt, relTMP.infil & "-" & relTMP.cdarb & "_" & relTMP.incod, relTMP.tipdsdes, "", GAIA2.nroFills(objConn, relTMP), idUsuari, relTMP, relTMP.pcrincod.ToString(), nroMaxim)

                If recursiu = 1 Then

                    afegeixNodesFillsPantallaLlista(objConn, relTMP.infil, childNode, codiArbre, idUsuari, nroArbre, tipus, recursiu, nivells, veuretots, novisibles)
                    childNode.Expanded = False
                    'expand d'un node no fa acció al servidor
                    childNode.ExpandMode = ExpandMode.ClientSide
                Else
                    If nivells > 1 Then
                        afegeixNodesFillsPantallaLlista(objConn, relTMP.incod, childNode, codiArbre, idUsuari, nroArbre, tipus, -1, nivells - 1, veuretots, novisibles)
                        childNode.ExpandMode = ExpandMode.ClientSide
                        childNode.Expanded = False
                    Else
                        childNode.ExpandMode = ExpandMode.ServerSide
                    End If
                End If
                node.AddNode(childNode)
            Next i
        End If

        DS.Dispose()
    End Sub 'afegeixNodesFillsPantallaLlista_NP

    '************************** Funcions per generar arbres de codificacio en XML ***********************************
    Public Shared Sub generarArbresCodificacio()
        Dim objConn As OleDbConnection
        objConn = GAIA2.bdIni()

        'creo xmls de codificació pel formulari de tràmits
        'temps de resposta
        GAIA2.generaCodificacioXml(objConn, 184496)

        'organ de resolucio
        GAIA2.generaCodificacioXml(objConn, 184523)
        'Competencia
        GAIA2.generaCodificacioXml(objConn, 184470)
        'Tipus procediment
        GAIA2.generaCodificacioXml(objConn, 184476)
        'Repercussio Economica
        GAIA2.generaCodificacioXml(objConn, 184492)
        'Canal
        GAIA2.generaCodificacioXml(objConn, 184509)
        'Instancia

        GAIA2.generaCodificacioXml(objConn, 184513)
        'Volumetria
        GAIA2.generaCodificacioXml(objConn, 184516)
        'tipus usuari
        GAIA2.generaCodificacioXml(objConn, 187152)
        'NivellProteccio LOPD
        GAIA2.generaCodificacioXml(objConn, 187165)
        'Efectes silenci administratiu
        GAIA2.generaCodificacioXml(objConn, 184503)
        'tràmit/servei

        GAIA2.generaCodificacioXml(objConn, 192011)

        'intern/extern
        GAIA2.generaCodificacioXml(objConn, 360085)

        GAIA2.generaArbreXml(objConn, 12367, "47,35")
        GAIA2.generaArbreXml(objConn, 12364, "47")
        GAIA2.generaArbreXml(objConn, 12365, "47")
        'directori
        GAIA2.generaArbreXml(objConn, 50349, "47")
        'tramits
        GAIA2.generaArbreXml(objConn, 69762, "47")
        'Agenda
        GAIA2.generaArbreXml(objConn, 12391, "47")
        GAIA2.generaArbreXml(objConn, 12392, "47")

        'Projectes
        GAIA2.generaArbreXml(objConn, 102072, "47")
        'Contractació
        GAIA2.generaArbreXml(objConn, 114392, "47")
        'Contractació per La Farga
        GAIA2.generaArbreXml(objConn, 130846, "47")



        GAIA2.generaCodificacioXml(objConn, 854771)
        GAIA2.generaCodificacioXml(objConn, 854772)

        GAIA2.bdFi(objConn)
    End Sub
    Public Shared Sub generaCodificacioXml(ByVal objconn As OleDbConnection, ByRef codiRelacio As Integer)
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim text As String, valor As String
        DS = New DataSet
        GAIA2.bdr(objconn, "SELECT NCODSTIT, NCOINNOD FROM METLNCO WITH(NOLOCK),METLREL WITH(NOLOCK) WHERE RELCDRSU=" & codiRelacio & " AND NCOINIDI=1 AND RELCDSIT<98 AND RELINFIL=NCOINNOD", DS)
        fitxerXML = New StreamWriter("c:\Inetpub\wwwroot\GAIA\aspx\codificacio\codificacio" & codiRelacio & ".xml", False, Encoding.Default)
        fitxerXML.WriteLine("<?xml version=""1.0"" encoding=""ISO-8859-1"" ?>")
        fitxerXML.WriteLine("<Codificacio><Node Text="""" Value=""-1""></Node>")

        For Each dbRow In DS.Tables(0).Rows
            text = HttpUtility.HtmlEncode(GAIA2.netejaHTML(dbRow("NCODSTIT"))).Replace("''", "'").Replace("´´", "'").Replace("&rsquo;", "'")
            valor = dbRow("NCOINNOD")
            fitxerXML.WriteLine("<Node Text=""" & text & """ Value=""" & valor & """></Node>")
        Next dbRow

        fitxerXML.WriteLine("</Codificacio>")
        fitxerXML.Close()
        DS.Dispose()
    End Sub


    'Genero un xml que utilitzaré en els formularis de fulles, nodes web i plantilles per mostrar els estils que es poden seleccionar per a cada cel·la
    Public Shared Sub generaCodisEstils(ByVal objconn As OleDbConnection)
        Dim fitxerXML As StreamWriter
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim text As String, valor As String = ""
        DS = New DataSet
        GAIA2.bdr(objconn, "SELECT isNUll(CSSINCOD, 0) as Identificador, CODDSTXT as nomGrup, isNUll(CSSDSTXT,'') as NomCamp, isNull(CODINCOD,'') as grup  FROM METLCOD WITH(NOLOCK) LEFT OUTER JOIN METLCSS WITH(NOLOCK) ON CSSINTIP=CODINCOD WHERE CODINTIP=4   order by   CSSDSTXT,CSSINTIP", DS)
        fitxerXML = New StreamWriter("c:\Inetpub\wwwroot\GAIA\aspx\codificacio\codificacioEstils.xml", False, Encoding.Default)
        fitxerXML.WriteLine("<?xml version=""1.0"" encoding=""ISO-8859-1"" ?>")
        fitxerXML.WriteLine("<Codificacio>")
        Dim grup As Integer = 0
        Dim strTmp As String = ""
        For Each dbRow In DS.Tables(0).Rows
            If grup <> dbRow("grup") Then
                grup = dbRow("grup")
                If Not String.IsNullOrEmpty(strTmp) Then
                    strTmp &= "</Node>"
                End If
                strTmp &= "<Node Text=""" & dbRow("nomGrup") & """ Value="""">"
            End If

            text = HttpUtility.HtmlEncode(GAIA2.netejaHTML(dbRow("NomCamp")))
            valor = dbRow("Identificador")
            strTmp &= "<Node Text=""" & text & """ Value=""" & valor & """></Node>"
        Next dbRow

        If strTmp.Length > 0 Then
            strTmp &= "</Node>"
        End If
        fitxerXML.WriteLine(strTmp)
        fitxerXML.WriteLine("</Codificacio>")
        fitxerXML.Close()
        DS.Dispose()
    End Sub

    Public Shared Sub generaArbreXml(ByVal objconn As OleDbConnection, ByRef codiRelacio As Integer, ByVal tipus As String)
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim imatge As String = ""
        Dim relTmp As New clsRelacio, text As String, valor As String
        fitxerXML = New StreamWriter("c:\Inetpub\wwwroot\GAIA\aspx\codificacio\arbre" & codiRelacio & ".xml", False, Encoding.Default)
        fitxerXML.WriteLine("<?xml version=""1.0"" encoding=""ISO-8859-1"" ?>")
        fitxerXML.WriteLine("<Tree>")
        DS = New DataSet
        GAIA2.bdr(objconn, "SELECT * FROM METLNOD WITH(NOLOCK), METLREL WITH(NOLOCK) WHERE RELINCOD=" + codiRelacio.ToString() + " AND RELINFIL=NODINNOD AND RELCDSIT<98 " + "  AND RELCDSIT<>" + ctESBORRATCADUCAT.ToString() + " AND RELCDSIT<>" + ctIMATGE.ToString() + " AND RELCDSIT<>" + ctDOCUMENT.ToString() + " ORDER BY NODINNOD DESC", DS)
        For Each dbRow In DS.Tables(0).Rows
            If dbRow("RELSWVIS") = 1 Then
                imatge = "node_codificacio.png"
            Else
                imatge = "node_codificacio5.png"
            End If
            relTmp.bdget(objconn, dbRow("RELINCOD"))
            text = GAIA2.netejaHTML(dbRow("NODDSTXT")).Replace("''", "'").Replace("´´", "'")

            valor = dbRow("NODINNOD").ToString() & "-" & dbRow("RELCDARB") & "_" & dbRow("RELINCOD")
            fitxerXML.WriteLine("<Node Text=""" & text & """ Image=""" & imatge & """ Value=""" & valor & """>")
            'Afegeixo nodes 
            afegeixNodesFillsPantallaXml(objconn, dbRow("NODINNOD"), codiRelacio, dbRow("RELCDARB"), tipus)
            fitxerXML.WriteLine("</Node>")
        Next dbRow
        fitxerXML.WriteLine("</Tree>")
        fitxerXML.Close()
        DS.Dispose()
    End Sub 'generaArbreXml

    Public Shared Sub afegeixNodesFillsPantallaXml(ByVal objConn As OleDbConnection, ByVal nodePare As Integer, ByVal codiRelacio As Integer, ByVal codiArbre As Integer, ByVal tipus As String)
        Dim DS As DataSet, dbRow As DataRow, strSql As String = ""
        Dim pathRelacioPare As String, relTMP As New clsRelacio
        Dim relPare As New clsRelacio
        Dim text As String, valor As String, fills As Integer
        Dim imatge As String = ""
        DS = New DataSet

        relPare.bdget(objConn, codiRelacio)

        pathRelacioPare = GAIA2.obtenirPathRelacio(objConn, codiRelacio)


        '        strSql = "SELECT RELSWVIS,PCRINCOD, RELINCOD FROM  METLREL WITH(NOLOCK),METLPCR WITH(NOLOCK),METLNOD WITH(NOLOCK) WHERE  RELINPAR<>RELINFIL AND RELCDRSU=" & relPare.incod & "  AND RELINCOD*=PCRINCOD AND RELCDSIT<95  AND NODINNOD=RELINFIL "
        '
        strSql = "SELECT RELSWVIS,PCRINCOD, RELINCOD FROM  METLREL WITH(NOLOCK) INNER JOIN METLNOD WITH(NOLOCK) ON NODINNOD=RELINFIL "
        If tipus <> "0" Then
            strSql += " AND NODCDTIP in (" & tipus & ")"

        End If

        strSql &= " LEFT OUTER JOIN METLPCR WITH(NOLOCK) ON RELINCOD=PCRINCOD WHERE  RELINPAR<>RELINFIL AND RELCDRSU=" & relPare.incod & " AND RELCDSIT<95 "
        strSql += "ORDER BY RELCDORD ASC"
        GAIA2.bdr(objConn, strSql, DS)
        For Each dbRow In DS.Tables(0).Rows
            If dbRow("RELSWVIS") = 1 Then
                imatge = "node_codificacio.png"
            Else
                imatge = "node_codificacio5.png"
            End If

            relTMP.bdget(objConn, dbRow("RELINCOD"))
            'tmpstr = relPare.cdher
            ' If nodePare.ToString().Length > 0 Then
            '     tmpstr += "_" + nodePare.ToString()
            ' End If
            'If relTMP.infil.ToString().Length > 0 Then
            '    tmpstr += "_" + relTMP.infil.ToString()
            'End If
            fills = GAIA2.nroFills(objConn, relTMP)
            text = GAIA2.netejaHTML(HttpUtility.HtmlDecode(relTMP.noddstxt)).Replace("&rsquo;", "'").Replace("’’", "'")

            If fills > 0 Then
                text &= " (" + fills.ToString() + ")"
            End If
            valor = relTMP.infil.ToString() & "-" & relTMP.cdarb & "_" & relTMP.incod
            fitxerXML.WriteLine("<Node Text=""" & text & """ Image=""" + imatge + """ Value=""" & valor & """>")
            afegeixNodesFillsPantallaXml(objConn, relTMP.infil, relTMP.incod, codiArbre, tipus)
            fitxerXML.WriteLine("</Node>")
        Next dbRow
        DS.Dispose()
    End Sub 'afegeixNodesFillsPantallaXml



    ''<summary>
    'Retorna un dataset amb nom Contingut, link visualització, tipus contingut amb data de revisió inferior a la data que es pasa com a paràmetre (dataRevisio)		
    ''</summary>
    ''<param name="dataRevisio">Data. Cercarem els continguts pendents de revisió amb data de revisió inferior a dataRevisio</param>
    ''<param name="tipus">Integer, Tipus de contingut del que generarem la llista</param>
    ''<returns>dataset amb codi Contingut, nom Contingut, data manteniment, link d'edició, tipus Contingut</returns>
    Public Shared Function llistaContingutsPendentsRevisioDS(ByVal datarevisio As Date) As DataSet
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim tipus As Integer = 0
        'Fulla INFo. Si és bitàcola, cerco els continguts pendents de manteniment. 
        GAIA2.bdr(Nothing, "(select DISTINCT INFINNOD as codi, INFDSTIT as nom, INFDTMAN as dataManteniment, '56' as tipus, '' as edicio,'' as icona FROM METLREL WITH(NOLOCK),METLINF WITH(NOLOCK) WHERE RELCDHER LIKE '_55785%' AND RELINFIL=INFINNOD AND INFWNBAU=1 AND INFDTMAN<= '" & datarevisio & "' AND RELCDSIT<98 AND INFDTMAN<>'1/1/1900') UNION (select DISTINCT FTRINNOD as codi, FTRDSNOM as nom, FTRDTMAN as dataManteniment, '51' as tipus, '' as edicio,'' as icona  FROM METLFTR WITH(NOLOCK),METLREL WITH(NOLOCK) WHERE RELCDHER LIKE '%_103017%' AND RELINFIL=FTRINNOD AND FTRDTMAN<='" & datarevisio & "' AND FTRDTMAN<>'1/1/1900' AND RELCDSIT<98 AND FTRINIDI=1) UNION (select DISTINCT DIRINNOD as codi, DIRDSNOM as nom, DIRDTMAN as dataManteniment, '3' as tipus, '' as edicio,'' as icona  FROM METLDIR WITH(NOLOCK),METLREL WITH(NOLOCK) WHERE RELCDHER LIKE '%_90106%' AND RELINFIL=DIRINNOD AND DIRDTMAN<='" & datarevisio & "' AND DIRDTMAN<>'1/1/1900' AND RELCDSIT<98 AND DIRINIDI=1) order by dataManteniment, codi", DS)
        For cont As Integer = 0 To DS.Tables(0).Rows.Count - 1
            dbRow = DS.Tables(0).Rows(cont)
            tipus = dbRow("tipus")
            Select Case tipus

                Case 56 'bitàcola
                    dbRow("edicio") = "http://lhintranet/GAIA/aspx/noticies/CarregaInfo.aspx?id=" & DS.Tables(0).Rows(cont)("codi") & "&idiArbre=1"
                    dbRow("icona") = "http://lhintranet/gaia/aspx/img/ico_noticia.png"
                Case 51 'tràmits
                    dbRow("edicio") = "http://lhintranet/GAIA/aspx/tramits/frmTramits.aspx?id=" & DS.Tables(0).Rows(cont)("codi") & "&idiArbre=1"
                    dbRow("icona") = "http://lhintranet/gaia/aspx/img/ico_tramit.png"
                Case 3 'directori
                    dbRow("edicio") = "http://lhintranet/GAIA/aspx/directori/frmDirectori.aspx?id=" & DS.Tables(0).Rows(cont)("codi") & "&idiArbre=1"
                    dbRow("icona") = "http://lhintranet/gaia/aspx/img/ico_directori.png"
            End Select
        Next cont
        DS.Tables(0).AcceptChanges()
        Return DS
    End Function




    ''<summary>
    'Retorna un dataset amb la llista de fulles d'informació de tipus bitàcola amb manteniments de backoffice pendetns		
    ''</summary>
    ''<param name="dataRevisio">Data. Cercarem els continguts pendents de backoffice amb data < que dataRevisio</param>
    ''<param name="tipus">Integer, Tipus de contingut del que generarem la llista</param>
    ''<returns>dataset amb codi contingut, data manteniment, nom Contingut, link d'edició </returns>
    Public Shared Function bitacolaLlistaBackofficesPendentsDS(ByVal datarevisio As Date) As DataSet
        Dim DS As DataSet

        DS = New DataSet()
        'Fulla INFo. Si és bitàcola, cerco els continguts pendents de manteniment. 

        GAIA2.bdr(Nothing, "(select DISTINCT INFINNOD as codi, INFDSTIT as nom, INFDTPBK as dataManteniment, cast(INFDSPBK as varchar(8000)) as descripcio, '' as edicio, '56' as tipus, '' as icona FROM METLREL WITH(NOLOCK),METLINF WITH(NOLOCK) WHERE RELCDHER LIKE '_55785%' AND RELINFIL=INFINNOD AND INFWNBAU=1 AND INFINIDI=1 AND INFDTPBK<='" & datarevisio & "' AND RELCDSIT<98 AND INFDTPBK<>'1/1/1900') order by dataManteniment, codi", DS)

        For cont As Integer = 0 To DS.Tables(0).Rows.Count - 1
            DS.Tables(0).Rows(cont)("edicio") = "http://lhintranet/GAIA/aspx/noticies/CarregaInfo.aspx?id=" & DS.Tables(0).Rows(cont)("codi") & "&idiArbre=1"
            DS.Tables(0).Rows(cont)("icona") = "http://lhintranet/gaia/aspx/img/ico_noticia.png"
        Next cont
        DS.Tables(0).AcceptChanges()
        Return DS
    End Function


    '************************** Fi funcions per generar arbres de codificació en XML ********************************	


    Public Shared Function seleccionaMenuContextual(ByVal objConn As OleDbConnection, ByVal idUsuari As Integer, ByVal rel As clsRelacio, ByVal descTipus As String, ByVal nroArbre As Integer) As String
        Return (seleccionaMenuContextual(objConn, idUsuari, rel, descTipus, nroArbre, False))
    End Function
    '****************************************************************************************************************
    '	Funció: seleccionaMenuContextual
    '	Entrada: 
    '		idusuari: Nro del node que apunta al node organigrama al que li retornarem el menu contextual
    '		codiRelacio: nro de relació que apunta al node fill sobre el que volem veure el nivell de permisos
    '		descTipus: string amb la descripció del tipus de node. Si no és web (arbre|node|fulla) no mostraré l'opció "PUBLICAR"
    '		nroArbre: nro del arbre on està el node al que volem posar el menu. Pot ser 1 o 2.
    '		superamaxim: booleà que indica si hi ha més nodes del màxim que es mostren per defecte (ex:100). En aquests cas es crida a un menú amb nom +
    '					que mostrarà tots
    '	Procés: 	
    '		Retorna el nom d'un menu contextual (dels que es troben a "contextmenus.xml") segons el nivell de permisos
    '		que es tinguin.
    '	Sortida:
    '		string amb el nom del menú contextual.
    '****************************************************************************************************************
    Public Shared Function seleccionaMenuContextual(ByVal objConn As OleDbConnection, ByVal idUsuari As Integer, ByVal rel As clsRelacio, ByVal descTipus As String, ByVal nroArbre As Integer, ByVal superamaxim As Boolean) As String
        Dim menu As String
        Dim codiRelacio As Integer
        codiRelacio = rel.incod

        menu = ""
        seleccionaMenuContextual = ""
        If descTipus = "node estructura" Then
            menu = "menuBuit"
        Else
            If idUsuari <> 0 Then
                'Tengo que buscar si el arbol al que pertenece el nodo es de tipo web			
                'IF clsPermisos.tePermis(objconn, 1, idUsuari, idUsuari, rel,heretat)=1 THEN				

                Select Case UCase(descTipus.Trim())

                    Case "FULLA WEB"
                        menu = "menuAW"
                    Case "FULLA LINK"
                        menu = "menuAD"
                    Case "NODE WEB"
                        menu = "menuAW"



                    Case "FULLA DOCUMENT"
                        menu = "menuAD"
                    Case "FULLA INFO"
                        menu = "menuAD"
                    Case "FULLA NOTICIA"
                        menu = "menuAD"
                    Case "FULLA TRAMIT"
                        menu = "menuAD"
                    Case "FULLA PLANTILLAWEB"
                        menu = "menuAD"
                    Case "FULLA CODIWEB"
                        menu = "menuAD"
                    Case Else
                        menu = "menuA"
                End Select



            Else
                menu = "menuBuit"
            End If
            If superamaxim Then
                menu = menu & "+"
            End If
            seleccionaMenuContextual = menu
            If nroArbre = 2 Then
                menu = menu + "2"
            End If
        End If


    End Function 'seleccionaMenuContextual



    Public Shared Function nroFills(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        'return(0)
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT COUNT(*) as nrofills FROM METLREL WITH(NOLOCK) WHERE RELCDRSU=" & rel.incod & " AND RELCDSIT<95", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            nroFills = dbRow("nrofills")
        Else
            nroFills = 0
        End If
        DS.Dispose()
    End Function 'nroFills

    Public Shared Function esPotExpandir(ByVal objConn As OleDbConnection, descTipusNodePare As String, descTipusNOde As String) As Integer
        'IF descTipusNodePare<>"fulla catalegServeis" THEN
        'esPotExpandir=TRUE
        'ELSE
        'IF descTipusNOde="fulla catalegServeis" THEN
        'esPotExpandir=TRUE
        'ELSE
        esPotExpandir = True
        'END IF

        'END IF	
    End Function 'esPotExpandir


    'Si imatge=null llavors afegeixo la imatge que hi ha a "nomImatge" 

    Public Shared Function creaNodePantalla(ByVal objConn As OleDbConnection, text As String, valor As String, imatge As String, nomImatge As String, fills As Integer, ByVal idUsuari As Integer, ByVal rel As clsRelacio, ByVal pasDeCircuit As String, ByVal nroMaxim As Integer) As RadTreeNode
        Dim DS As DataSet
        Dim textAlt As String = ""
        DS = New DataSet()
        Dim codiRelacio, nroArbre, situacio As Integer
        codiRelacio = rel.incod
        nroArbre = rel.cdarb
        situacio = rel.cdsit
        Dim superoNumeroMaxim As Boolean = False
        Dim limit As Integer
        If fills > 0 Then
            If fills > nroMaxim And nroMaxim <> 0 Then
                text &= " (" & nroMaxim & "+)"
                superoNumeroMaxim = True
            Else
                text &= " (" & fills & ")"
            End If
        End If



        text = GAIA2.netejaHTML(HttpUtility.HtmlDecode(text.Replace("´´", "´")))

        text = HttpUtility.HtmlDecode(text)
        textAlt = text
        limit = text.Length
        If limit > 55 Then
            limit = 55
        End If
        text = GAIA2.tallaText(text, limit)

        Dim node As New RadTreeNode(text)

        node.ToolTip = textAlt
        node.Value = valor
        'node.Expanded = False
        'node.ExpandMode= ExpandMode.Serverside
        If imatge.Length > 0 Then
            Select Case imatge.Trim
                Case "arbre"
                    node.Image = "arbre.png"
                Case "fulla organigrama"
                    node.Image = "ico_organigrama.png"
                Case "fulla directori"
                    node.Image = "ico_directori5.png"
                    If rel.swvis = 1 Then
                        GAIA2.bdr(objConn, "select DIRSWVIS FROM METLDIR WITH(NOLOCK) WHERE DIRINNOD=" + rel.infil.ToString(), DS)
                        If DS.Tables(0).Rows.Count > 0 Then
                            If DS.Tables(0).Rows(0)("DIRSWVIS") = 1 Then
                                node.Image = "ico_directori.png"
                            End If
                        End If
                    End If
                Case "node estructura"
                    node.Image = "node_estructura.png"
                Case "node directori"
                    node.Image = "node directori.png"
                Case "fulla tramit"
                    node.Image = "ico_tramit5.png"
                    If rel.swvis = 1 Then
                        GAIA2.bdr(objConn, "select FTRSWVIS FROM METLFTR WITH(NOLOCK) WHERE FTRINNOD=" + rel.infil.ToString(), DS)
                        If DS.Tables(0).Rows.Count > 0 Then
                            If DS.Tables(0).Rows(0)("FTRSWVIS") = 1 Then
                                node.Image = "ico_tramit.png"
                            End If
                        End If
                    End If
                Case "node tramit"
                    node.Image = "node_tramits.png"
                Case "node organigrama"
                    node.Image = "node_organigrama.png"
                Case "node elMeuEspai"
                    node.Image = "ico_elmeuespai.png"
                Case "fulla web"
                    node.Image = "ico_web" & situacio & ".png"
                Case "fulla info"
                    If rel.swvis = 1 Then
                        node.Image = "ico_info" & situacio & ".png"
                    Else
                        node.Image = "ico_info5.png"
                    End If

                Case "node codificacio"
                    If rel.swvis = 1 Then
                        node.Image = "node_codificacio.png"
                    Else
                        node.Image = "node_codificacio5.png"
                    End If


                    GAIA2.bdr(objConn, "select top 1 NCOSWCOD FROM METLNCO WITH(NOLOCK) WHERE NCOINIDI=1 AND NCOINNOD=" + rel.infil.ToString(), DS)
                    If DS.Tables(0).Rows.Count > 0 Then
                        If DS.Tables(0).Rows(0)("NCOSWCOD") = "S" Then
                            node.Checkable = True
                        Else
                            node.Checkable = True
                        End If
                    End If

                Case "fulla codificacio"
                    If rel.swvis = 1 Then
                        node.Image = "node_codificacio.png"
                    Else
                        node.Image = "node_codificacio5.png"
                    End If
                Case "fulla document"
                    If rel.swvis = 1 Then

                        GAIA2.bdr(objConn, "select TDODSIMG FROM METLTDO WITH(NOLOCK),METLDOC WITH(NOLOCK) WHERE DOCINTDO=TDOCDTDO and DOCINNOD=" & valor.Substring(0, valor.IndexOf("-")), DS)
                        If DS.Tables(0).Rows.Count > 0 Then
                            node.Image = DS.Tables(0).Rows(0)(0).Replace(".png", situacio.ToString() + ".png")
                        Else
                            node.Image = "ico_document.png"
                        End If

                    Else
                        node.Image = "ico_document5.png"
                    End If
                Case "node web"
                    node.Image = "node_web.png"
                Case "node plantillaWeb"
                    node.Image = "node_plantilla.png"
                Case "fulla plantillaWeb"
                    node.Image = "ico_plantilla.png"
                Case "fulla noticia"
                    If rel.swvis = 1 Then
                        node.Image = "ico_noticia" & situacio & ".png"
                    Else
                        node.Image = "ico_noticia5.png"
                    End If
                Case "fulla projecte"
                    If rel.swvis = 1 Then
                        node.Image = "ico_projecte" & situacio & ".png"
                    Else
                        node.Image = "ico_projecte5.png"
                    End If


                Case "fulla link"
                    If rel.swvis = 1 Then
                        node.Image = "ico_link" & situacio & ".png"
                    Else
                        node.Image = "ico_link5.png"
                    End If


                Case "node document"
                    node.Image = "folder.png"
                Case "node catalegServeis"
                    node.Image = "node_catalegserveis.png"
                Case "fulla catalegServeis"
                    node.Image = "ico_catalegserveis.png"
                Case "fulla codiWeb"
                    node.Image = "ico_llibreria.png"
                Case "node codiWeb"
                    node.Image = "node_llibreria.png"
                Case "node circuit"
                    node.Image = "node_circuit.png"
                Case "fulla circuit"
                    node.Image = "ico_circuit.png"
                Case "fulla missatge"
                    node.Image = "fullaMissatges.gif"
                Case "node missatge"
                    node.Image = "nodeMissatges.gif"
                Case "fulla contractació"
                    node.Image = "ico_contractacio" & situacio & ".png"
                Case "fulla agenda"
                    node.Image = "ico_agenda5.png"
                    If rel.swvis = 1 Then
                        GAIA2.bdr(objConn, "select AGDSWVIS FROM METLAGD WITH(NOLOCK) WHERE AGDINNOD=" + rel.infil.ToString(), DS)
                        If DS.Tables(0).Rows.Count > 0 Then
                            If DS.Tables(0).Rows(0)("AGDSWVIS") = 1 Then
                                node.Image = "ico_agenda" & situacio & ".png"
                            End If
                        End If
                    End If
                Case "node agenda"
                    node.Image = "node_agenda.png"

                Case "node tramit"
                    node.Image = "node_tramits.png"
            End Select

            'Tracto els casos especials
            If text.IndexOf("Sense classificar") = 0 Then
                node.Image = "folder.png"
            Else
                If text.IndexOf("Missatges") = 0 Then
                    node.Image = "node_missatges.png"
                End If
            End If
        Else
            node.Image = nomImatge
            imatge = nomImatge

        End If
        'if (pasdecircuit.length>0 and pasdecircuit<>"0") THEN
        '	node.Image= node.Image.Replace(".gif","p.gif")
        'END IF
        node.ContextMenuName = GAIA2.seleccionaMenuContextual(objConn, idUsuari, rel, imatge, nroArbre, superoNumeroMaxim)
        DS.Dispose()
        Return node
    End Function 'creaNodePantalla


    Public Shared Function bdIni() As OleDbConnection
        Dim objconn As New OleDbConnection
        '	Try
        '	 	objConn.ConnectionString = "Provider=SQLOLEDB;Data Source=sql;Initial Catalog=SILH; pwd=secopc; user id=CPDsa"';Min Pool Size=100;"
        '		objConn.Open()		
        '	Catch
        '					GAIA.f_logError(objconn, "Base de dades", "BDINI. "+err.number.tostring(),  Err.Description)
        'objconn = GAIA.bdIni()
        '	END TRY		
        Return objconn
    End Function

    Public Shared Sub bdFi(objConn As OleDbConnection)
        'IF NOT objconn is nothing THEN

        'objconn.dispose()
        'END IF
    End Sub


    '************************************************************************************************************
    '	Funció: Executa acció a bd sense resultats
    '************************************************************************************************************

    Public Shared Function bdSR(ByRef objConn As OleDbConnection, ByVal strSql As String) As Integer
        'bdSR(objConn, strSql, "sql4", "SILHTEST")
        Return (bdSR(objConn, strSql, "SQL6", "SILH"))
    End Function


    Public Shared Function bdSR(ByRef objConn As OleDbConnection, ByVal strSql As String, ByVal dataSource As String, ByVal bd As String) As Integer
        Dim strUsr As String = ""
        Dim strPwd As String = ""
        objConn = New OleDbConnection
        Dim myCommand As New OleDbCommand
        Dim resultat As Integer = 0
        If UCase(dataSource) = "SQL" Then
            dataSource = "SQL6"
        End If
        'objConn.ConnectionString = "Provider=SQLOLEDB;Data Source=" + dataSource + ";Initial Catalog=" + bd + "; pwd=secopc; user id=CPDsa" ';Min Pool Size=100;"
        ' objConn.ConnectionString = "Provider=SQLOLEDB;Data Source=" + dataSource + ";Initial Catalog=" + bd + "; pwd=sa12roKa; user id=CPDsa" ';Min Pool Size=100;"

        Select Case UCase(dataSource)
            Case "SQL6" 'web
                strPwd = "sa12roKa"
                strUsr = "CPDsa"
            Case "SQLBI" 'bussiness				
                strPwd = "Tjk8%sQ"
                strUsr = "usr_weblh"
            Case "SQL2" 'cartografia
                strPwd = "Tjk8%sQ;"
                strUsr = "usr_weblh"
            Case Else
                strPwd = "sa12roKa"
                strUsr = "CPDsa"
        End Select

        objConn.ConnectionString = "Provider=SQLOLEDB;Data Source=" + dataSource + ";Initial Catalog=" + bd + "; pwd=" & strPwd & "; user id=" & strUsr & ";Current Language=Spanish"
        ' objConn.ConnectionString = "Provider=SQLOLEDB;Data Source=" + dataSource + ";Initial Catalog=" + bd + "; pwd=sa12roKa; user id=CPDsa;Current Language=Spanish"

        Try
            objConn.Open()
            myCommand = New OleDbCommand(strSql, objConn)
            myCommand.ExecuteNonQuery()
            myCommand.Dispose()
        Catch ex As SqlException

            GAIA2.f_logError(objConn, "Base de dades", "BDSR", "error nro: " & ex.ErrorCode & " / " & ex.Message & "/" & strSql)
            If ex.Number = 1205 Then 'deadlock.. si es produeix, intento fer-ho de nou
                resultat = bdSR(objConn, strSql, dataSource, bd)
            End If
            resultat = ex.Number
        End Try


        objConn.Close()
        objConn.Dispose()
        Return (resultat)
    End Function


    '************************************************************************************************************
    '	Funció: Executa acció a bd sense resultats i retorna el camp identitat (si existeix)
    '************************************************************************************************************
    Public Shared Function bdSR_id(ByRef objConn As OleDbConnection, ByVal strSql As String) As Integer
        Return bdSR_id(objConn, strSql, "SQL6", "SILH")
    End Function
    Public Shared Function bdSR_id(ByRef objConn As OleDbConnection, ByVal strSql As String, ByVal dataSource As String, ByVal bd As String) As Integer



        Dim strUsr As String = ""

        Dim strPwd As String = ""

        If UCase(dataSource) = "SQL" Then
            dataSource = "SQL6"
        End If

        Select Case UCase(dataSource)
            Case "SQL6" 'web
                strPwd = "sa12roKa"
                strUsr = "CPDsa"
            Case "SQLBI" 'bussiness				
                strPwd = "Tjk8%sQ"
                strUsr = "usr_weblh"
            Case "SQL2" 'cartografia
                strPwd = "Tjk8%sQ;"
                strUsr = "usr_weblh"
            Case Else
                strPwd = "sa12roKa"
                strUsr = "CPDsa"
        End Select


        Dim identitat As Integer
        objConn = New OleDbConnection
        objConn.ConnectionString = "Provider=SQLOLEDB;Data Source=" + dataSource + ";Initial Catalog=" + bd + "; pwd=" & strPwd & "; user id=" & strUsr
        'objConn.ConnectionString = "Provider=SQLOLEDB;Data Source=" + dataSource + ";Initial Catalog=" + bd + "; pwd=secopc; user id=CPDsa"
        objConn.Open()

        Dim myCommand As New OleDbCommand
        Try
            myCommand = New OleDbCommand(strSql, objConn)
            myCommand.ExecuteNonQuery()
            myCommand.CommandText = "SELECT SCOPE_IDENTITY()"
            identitat = myCommand.ExecuteScalar
        Catch ex As SqlException
            GAIA2.f_logError(objConn, "Base de dades", "BDSR_ID.", "error nro: " & ex.ErrorCode & " / " & ex.Message & "/" & strSql)
        End Try


        objConn.Close()
        objConn.Dispose()
        Return identitat
    End Function

    '************************************************************************************************************
    '	Funció: Executa acció a bd amb resultats i ho retorna a l'array pasat per referencia
    '************************************************************************************************************

    Public Shared Function bdr(ByRef objConn As OleDbConnection, ByVal strSql As String, ByRef DS As DataSet) As Integer
        ' bdR(objConn, strSql, DS, "sql", "SILH")
        Return (bdR(objConn, strSql, DS, "SQL6", "SILH"))
    End Function


    Public Shared Function bdR(ByRef objConn As OleDbConnection, ByVal strSql As String, ByRef DS As DataSet, ByVal dataSource As String, ByVal bd As String) As Integer
        Dim resultat As Integer = 0
        Dim strUsr As String = ""
        Dim strPwd As String = ""
        DS.Clear()
        If UCase(dataSource) = "SQL" Then
            dataSource = "SQL6"
        End If


        Select Case UCase(dataSource)
            Case "SQL6" 'web
                strPwd = "sa12roKa"
                strUsr = "CPDsa"
            Case "SQLBI" 'bussiness				
                strPwd = "sa12roKa"
                strUsr = "CPDsa"
            Case "SQL2" 'cartografia
                strPwd = "Tjk8%sQ;"
                strUsr = "usr_weblh"
            Case "SQL7" 'AUPAC
                strPwd = "ACYTtXBzo5"
                strUsr = "aupaclh"
            Case Else
                strPwd = "sa12roKa"
                strUsr = "CPDsa"
        End Select


        objConn = New OleDbConnection
        ' objConn.ConnectionString = "Provider=SQLOLEDB;Data Source=" + dataSource + ";Initial Catalog=" + bd + "; pwd=secopc; user id=CPDsa"
        objConn.ConnectionString = "Provider=SQLOLEDB;Data Source=" + dataSource + ";Initial Catalog=" + bd + "; pwd=" & strPwd & "; user id=" & strUsr
        objConn.Open()
        Dim MyCommand As New OleDbDataAdapter()
        Try
            MyCommand.SelectCommand = New OleDbCommand(strSql, objConn)
            MyCommand.SelectCommand.CommandTimeout = 200
            MyCommand.Fill(DS)

            'GAIA.bdSR(objConn, "INSERT INTO METLDEB (TEXT,data) VALUES ('" + strSql.Replace("'", "''") + "','kkpp - " & Now & "')")
        Catch ex As SqlException
            GAIA2.f_logError(objConn, "Base de dades", "BDR", Err.Description + ".." + strSql)
            resultat = ex.Number
        End Try

        objConn.Close()
        objConn.Dispose()
        MyCommand.Dispose()

        Return (resultat)
    End Function



    '******************************************************************
    '	Funció: tipusNodebyNro
    '	Entrada: 
    '		nroNode: Nro del node
    '	Procés: 	
    '		Cerca dins de la taula de nodes i retorna el tipus al que pertany
    '	Sortida:
    '		Codi del tipus de node
    '******************************************************************
    Public Shared Function tipusNodebyNro(ByVal objConn As OleDbConnection, ByVal nroNode As Integer, ByRef descTipus As String) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        tipusNodebyNro = ""
        GAIA2.bdr(objConn, "SELECT  NODCDTIP,TIPDSDES FROM METLNOD WITH(NOLOCK), METLTIP WITH(NOLOCK) WHERE TIPINTIP=NODCDTIP  AND NODINNOD=" & nroNode.ToString.Trim() & " ORDER BY  TIPCDVER DESC ", DS)
        tipusNodebyNro = 0
        descTipus = ""

        For Each dbRow In DS.Tables(0).Rows
            tipusNodebyNro = dbRow("NODCDTIP")
            descTipus = dbRow("TIPDSDES").Trim
        Next dbRow
        DS.Dispose()
    End Function


    '******************************************************************
    '	Funció: tipusNodeFillbyRelacio
    '	Entrada: 
    '		codiRelacio: codi de relacio
    '	Procés: 	
    '		Cerca el tipus del fill de la relacio
    '	Sortida:
    '		Codi del tipus de node
    '******************************************************************
    Public Shared Function tipusNodeFillbyRelacio(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer, ByRef descTipus As String) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()

        GAIA2.bdr(objConn, "SELECT  NODCDTIP,TIPDSDES FROM METLNOD WITH(NOLOCK), METLTIP WITH(NOLOCK), METLREL WITH(NOLOCK) WHERE RELINFIL=NODINNOD AND TIPINTIP=NODCDTIP  AND RELINCOD=" + codiRelacio.ToString.Trim() + " AND RELCDSIT<98 " + " ORDER BY  TIPCDVER DESC ", DS)
        tipusNodeFillbyRelacio = 0
        descTipus = ""
        For Each dbRow In DS.Tables(0).Rows
            tipusNodeFillbyRelacio = dbRow("NODCDTIP")
            descTipus = dbRow("TIPDSDES").Trim
        Next dbRow
        DS.Dispose()
    End Function 'tipusNodeFillbyRelacio



    '******************************************************************
    '	Funció: tipusNodeByTxt
    '	Entrada: 
    '		descNode: nom del tipus de node
    '	Procés: 	
    '		Cerca dins de la taula de tipus de nodes i retorna el codi del tipus associat a la descripció
    '	Sortida:
    '		codi tipusNode
    '******************************************************************
    Public Shared Function tipusNodeByTxt(ByVal objConn As OleDbConnection, ByVal descTipusNode As String) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        tipusNodeByTxt = 0
        GAIA2.bdr(objConn, "SELECT TIPINTIP FROM METLTIP WITH(NOLOCK) WHERE TIPDSDES LIKE '%" & descTipusNode & "%' ORDER BY  TIPCDVER DESC", DS)
        For Each dbRow In DS.Tables(0).Rows
            tipusNodeByTxt = dbRow("TIPINTIP")
        Next dbRow
        DS.Dispose()
    End Function

    '******************************************************************
    '	Funció: codiNodeByTxt
    '	Entrada: 
    '		nomTipus: nom del tipus de node
    '		descNode: descripció del node
    '	Procés: 	
    '		Cerca dins de la taula de nodes i retorna el codi del node associat a la descripció i a l'arbre
    '	Sortida:
    '		codi Node o 0 si no trobat
    '******************************************************************
    Public Shared Function codiNodeByTxt(ByVal objConn As OleDbConnection, ByVal nomTipus As String, ByVal descNode As String) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        codiNodeByTxt = 0
        GAIA2.bdr(objConn, "SELECT NODINNOD FROM METLNOD WITH(NOLOCK),METLTIP WITH(NOLOCK),METLREL WITH(NOLOCK) WHERE NODDSTXT like '%" + descNode.Trim() + "%' AND NODCDTIP=TIPINTIP AND TIPDSDES like '%" + nomTipus.ToString() + "%'  AND RELINFIL=NODINNOD  AND RELCDSIT<>99 ORDER BY  NODINNOD DESC ", DS)

        For Each dbRow In DS.Tables(0).Rows
            codiNodeByTxt = dbRow("NODINNOD")
        Next dbRow
        DS.Dispose()
        Return codiNodeByTxt
    End Function


    '******************************************************************
    '	Funció: codiNodeElMeuEspaiByTxtPropietari
    '	Entrada: 
    '		descNode: descripció del node
    '		codiOrg: codi organigrama propietaria del node

    '	Procés: 	

    '		Cerca dins de la taula de nodes de ElMeuEspai i cerca per nom i propietari el codi de node
    '	Sortida:
    '		codi Node o 0 si no trobat
    '******************************************************************
    Public Shared Function codiNodeElMeuEspaiByTxtPropietari(ByVal objConn As OleDbConnection, ByVal descNode As String, ByVal codiOrg As Integer) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()


        codiNodeElMeuEspaiByTxtPropietari = 0
        GAIA2.bdr(objConn, "SELECT NESINNOD FROM METLNES WITH(NOLOCK),METLREL WITH(NOLOCK) WHERE NESDSTIT like '%" + descNode.Trim() + "%' and 	NESCDORG=" + codiOrg.ToString() + " AND NESINIDI=1 AND RELINFIL=NESINNOD AND RELCDSIT<>99", DS)

        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            codiNodeElMeuEspaiByTxtPropietari = dbRow("NESINNOD")
        End If



        DS.Dispose()
    End Function


    '******************************************************************
    '	Funció: trobaFills
    '	Entrada: 
    '		codiRelacio: codi de la relació
    '		codiNode	: Codi del Node pare
    '		nodesFills:	array on guardo els fills trobats
    '		nroFills:  Nro de fills trobats
    '		relacions: array de les relacions on el RELINFIL es descendent de codiNode
    '		nroOrg: 	En cas de nroOrg=0 llavors no comprovarem els permisos, només retornarem els nodes sobre els que
    '							l'usuari nodeOrganigrama tingui permís de lectura
    '		tipus: Tipus de fill que volem trobar. Si tipus=0 llavors retornem tots els fills
    '	Procés: 	
    '		Si el codi de Relació=0 llavors, codiNode és l'arrel i buscaré tots els nodes que penjen.
    '		Obté tots els fills de codiNode per a la relació codiRelacio. 
    '	Sortida:
    '******************************************************************
    Public Shared Sub trobaFills(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiNode As Integer, ByRef nodesFills() As Integer, ByRef nroFills As Integer, ByRef relacions() As Integer, ByVal nroOrg As Integer, ByVal tipus As Integer)

        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim nodeRoot, permis As Integer
        nroFills = 0
        Dim reltmp As New clsRelacio

        ReDim nodesFills(0)
        ReDim relacions(0)


        DS = New DataSet()

        If tipus > 0 Then

            GAIA2.bdr(objConn, "SELECT RELINCOD, RELINFIL,RELCDHER,RELINPAR FROM METLREL WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE RELINCOD<>" & rel.incod & " AND RELCDRSU=" & rel.incod & "  AND RELCDSIT<98  AND RELINFIL=NODINNOD AND NODCDTIP=" + tipus.ToString() + " ORDER BY DATALENGTH(RELCDHER), RELCDORD asc", DS)
        Else
            GAIA2.bdr(objConn, "SELECT RELINCOD, RELINFIL,RELCDHER,RELINPAR FROM METLREL WITH(NOLOCK) WHERE RELINCOD<>" & rel.incod & " AND RELCDRSU=" & rel.incod & "  AND RELCDSIT<98   ORDER BY DATALENGTH(RELCDHER), RELCDORD asc", DS)
        End If

        For Each dbRow In DS.Tables(0).Rows
            If nroOrg = 0 Then
                Dim strTmp As String = ""
                If dbRow("RELINPAR") = dbRow("RELINFIL") Then
                    nodeRoot = dbRow("RELINCOD")
                Else
                    strTmp = dbRow("RELCDHER").SubString(1)
                    If strTmp.IndexOf("_") > 0 Then
                        strTmp = strTmp.Substring(0, strTmp.IndexOf("_"))

                    End If
                End If
                nodeRoot = GAIA2.obtenirRoot(objConn, strTmp)
            End If
            permis = 0
            If nroOrg = 0 Then
                permis = 1
            Else
                reltmp.bdget(objConn, dbRow("RELINCOD"))
                If clsPermisos.tePermisLecturaNode(objConn, reltmp, nroOrg, nodeRoot) Then
                    permis = 1
                End If
            End If
            If permis = 1 Then
                ReDim Preserve nodesFills(nroFills)
                nodesFills(nroFills) = dbRow("RELINFIL")
                ReDim Preserve relacions(nroFills)
                relacions(nroFills) = dbRow("RELINCOD")
                nroFills = nroFills + 1
            End If
        Next dbRow
        'END IF
        DS.Dispose()
    End Sub


    '******************************************************************
    '	Funció: obtenirRoot
    '	Entrada: 
    '		codiNode: Code del node que apunta al primer node de l'arbre
    '	Procés: 	
    '		Cerca la relació que defineix la primera relació d'un arbre
    '	Sortida:
    '		codi relació o 0 si no trobat
    '******************************************************************
    Public Shared Function obtenirRoot(ByVal objConn As OleDbConnection, ByVal codiNode As String) As Integer


        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        obtenirRoot = 0

        GAIA2.bdr(objConn, "SELECT RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELINPAR=RELINFIL AND RELINPAR=" + codiNode + " AND RELCDSIT<98 ", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirRoot = dbRow("RELINCOD")
        End If

        DS.Dispose()
    End Function





    '******************************************************************
    '	Funció: obtenirTarget
    '	Entrada: 
    '		rel: relació que apunta al contingut de tipus METLLNK
    '	Procés: 	
    '		Retorna el valor LNKWNTIP si existeix, en cas contrari retorna 0
    '******************************************************************
    Public Shared Function obtenirTarget(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        obtenirTarget = 0
        GAIA2.bdr(objConn, "SELECT LNKWNTIP FROM METLLNK WITH(NOLOCK) WHERE LNKINNOD=" & rel.infil, DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirTarget = dbRow("LNKWNTIP")

        End If


        DS.Dispose()
    End Function





    '************************************************************************************
    '	Funció: buscaParesRel
    '	Entrada: 
    '		Arbre: codi de l'arbre
    '		nodeFill:	Node del q volem trobar tots els pares
    '		nodosPerObrir:  array de nodes on afegiré recursivament tots els pares.
    '	Procés: 	
    '		troba tots els nodes que hi ha al camí  d'on penja nodefill fins l'arrel
    '************************************************************************************
    Public Shared Sub buscaParesRel(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer, ByRef nodosPerObrir() As String)
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim existe As Integer
        Dim item As String

        Dim tmp As String
        Dim path As String
        'cont = nodosPerObrir.length+1
        GAIA2.bdr(objConn, "SELECT * FROM METLREL WITH(NOLOCK) WHERE RELINCOD=" + codiRelacio.ToString(), DS)
        'Per a totes les relacions on nodeFill sigui Fill, busco els pares, els afegeixo a un array sense repetits i ho retorno.
        For Each dbRow In DS.Tables(0).Rows
            If dbRow("RELCDHER").ToString().Length > 0 Then
                path = dbRow("RELCDHER").Trim().substring(1) & ""
                While path.Length > 0
                    existe = False
                    If path.IndexOf("_") >= 0 Then
                        tmp = path.Substring(0, path.IndexOf("_"))
                        path = path.Substring(path.IndexOf("_") + 1)
                    Else
                        tmp = path.Substring(0)
                        path = ""
                    End If
                    For Each item In nodosPerObrir
                        If tmp = item Then
                            existe = True
                            Exit For
                        End If
                    Next item
                    If Not existe Then
                        ReDim Preserve nodosPerObrir(nodosPerObrir.Length)
                        ' SELECT RELINCOD FROM METLREL WHERE RELCDHER LIKE '"+path+"'"
                        nodosPerObrir(nodosPerObrir.Length - 1) = tmp
                    End If

                End While
            End If
            'array.sort(nodosPerObrir)		
        Next dbRow

        'ultim cas
        'SELECT RELINCOD FROM METLREL WHERE RELINPAR=RELINFIL AND RELINPAR= 'a')

        DS.Dispose()
    End Sub 'buscaParesRel

    'Busca el node dins de l'arbre actiu a la pantalla (pot apareixer + d'un cop)
    Public Shared Function buscaNodeArbrePantalla(ByVal objConn As OleDbConnection, ByVal treeview1 As RadTreeView, ByVal node As Integer) As RadTreeNode()
        Dim nodeTrobat As RadTreeNode
        Dim cont As Integer
        Dim llistaNodesTrobats() As RadTreeNode = {}
        cont = 0

        For Each nodeTrobat In treeview1.GetAllNodes()
            If nodeTrobat.Value.Substring(0, nodeTrobat.Value.ToString().IndexOf("-")) = node.ToString() Then
                ReDim Preserve llistaNodesTrobats(cont)
                llistaNodesTrobats(cont) = nodeTrobat

                cont = cont + 1
            End If
        Next nodeTrobat
        If cont = 0 Then
            ReDim Preserve llistaNodesTrobats(1)
            llistaNodesTrobats(1) = Nothing
        End If
        Return llistaNodesTrobats

    End Function 'buscaNodeArbrePantalla


    'Busca la relació dins de l'arbre actiu a la pantalla 
    Public Shared Function buscaRelacioArbrePantalla(ByVal objConn As OleDbConnection, ByVal tree As RadTreeView, ByVal codiRelacio As Integer) As RadTreeNode
        buscaRelacioArbrePantalla = New RadTreeNode
        Dim item As RadTreeNode
        For Each item In tree.GetAllNodes()
            If item.Value.Substring(item.Value.ToString().IndexOf("_") + 1) = codiRelacio.ToString() Then
                buscaRelacioArbrePantalla = item
            End If
        Next item

    End Function 'buscaRelacioArbrePantalla


    '******************************************************************
    '	Funció:	Esborrar Node
    '	Entrada: 
    '		arbre: nro d'arbre al que pertany el node que es vol esborrar
    '		codiNode: node a esborrar. si =0 llavors el trobo a partir de RELINFIL de codirelacio
    '		rel: relació que apunta al node
    '		esborrarDocumentPublicat: si 1: si el node apunta a un document, esborro el document en el servidor on s'ha publicat el document, sempre i quan no l'utilitzi ningú més.
    '		esborrarRecursiu: si =1 esborro el node  i tots els fills, si 0 esborro el node i poso els fills al mateix nivell
    '	Sortida:	 0 Si ok , >0 si error
    '	Procés: 	
    '		Esborra el node. 
    ' No deixo esborrar un node de tipus arbre.
    '	0- Esborro els permisos que té node
    ' 1- Si es un node de tipus "node": Poso tots els fills dels diferents arbres al mateix nivell
    ' 2- esborro totes les relacions del node
    '
    ' 	3.1- esborro el node
    ' 	3.2- esborro la fulla
    ' 		Si es un node fulla i hi han documents relacionats no faig res amb els documents.
    '			Eliminant les relacions amb ells hi ha prou.
    '******************************************************************


    Public Shared Function esborrarNode(ByVal objConn As OleDbConnection, ByVal codiNode As Integer, ByVal rel As clsRelacio, ByVal esborrarDocumentPublicat As Integer, ByVal codiUsuari As String, ByVal esborrarRecursiu As Integer, ByVal tipusEsborrat As Integer) As String

        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim relTMP As New clsRelacio
        Dim tipusLog As Integer
        Dim resultat As String = ""
        esborrarNode = ""


        If codiNode <> 0 Then 'demanen esborrar d'una única ubicació
            'esborro totes les entrades en METLASS (associacions de continguts)
            GAIA2.bdr(objConn, "select DISTINCT ASSCDNOD FROM METLASS  WHERE ASSCDNRL=" & codiNode, DS)
            Dim relvoid As New clsRelacio()
            For Each dbRow In DS.Tables(0).Rows
                GAIA2.bdSR(objConn, "DELETE FROM METLASS WHERE ASSCDNRL=" & codiNode)
                GAIA2.esborrarCelles(Nothing, "CELCDNOD=" & dbRow("ASSCDNOD"))
                GAIA2.afegeixAccioManteniment(objConn, relvoid, dbRow("ASSCDNOD"), 99, Now, Now, relvoid, 0, 0)
            Next dbRow
        End If


        If tipusEsborrat = ctESBORRATCADUCAT Or tipusEsborrat = ctESBORRATCADUCATIMATGE Or tipusEsborrat = ctESBORRATCADUCATDOCUMENT Then
            tipusLog = TACADUCAR
        Else
            tipusLog = TAESBORRAR
        End If
        If codiUsuari = "" Then
            codiUsuari = "9999"
        End If
        If rel.incod = 0 Then
            If codiNode <> 0 Then
                GAIA2.bdr(objConn, "SELECT * FROM METLREL WITH(NOLOCK) WHERE RELINFIL=" & codiNode & " AND RELCDSIT<99", DS)
                For Each dbRow In DS.Tables(0).Rows
                    relTMP.bdget(objConn, dbRow("RELINCOD"))
                    esborrarNode(objConn, 0, relTMP, esborrarDocumentPublicat, codiUsuari, esborrarRecursiu, tipusEsborrat)



                Next dbRow
            End If
        Else

            If codiNode = 0 Then
                codiNode = rel.infil
            End If

            'tipus= GAIA.tipusNodebyNro(objconn, codiNode, descTipus)
            If rel.tipdsdes.Length = 0 Then
            Else
                ' No deixo esborrar un node de tipus arbre.		
                If rel.tipdsdes.Substring(0, 5) = "arbre" Then
                    esborrarNode = "Error: no es poden esborrar arbres"
                Else
                    If esborrarRecursiu = 1 Then
                        GAIA2.log(objConn, rel, codiUsuari, "", tipusLog)
                        GAIA2.esborrarRelacions(objConn, rel, 1, tipusEsborrat)
                        'Esborro el contingut inicial
                        GAIA2.esborrarNode(objConn, 0, rel, 0, codiUsuari, 0, tipusEsborrat)



                        'MSV, 12/5/15 No crido a esborrarNode per a cada fill per que pot ser molt costós en temps. Marco només les relacions amb RELCDSIT=100
                        GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDSIT=100 WHERE RELCDHER LIKE '" & rel.cdher & "_" & rel.infil & "' AND RELCDSIT<95")


                    Else 'esborrarRecursiu<>1
                        Select Case rel.tipdsdes
                            Case "fulla document"
                                If esborrarDocumentPublicat Then esborrarFullaDocument(objConn, rel, codiUsuari)
                            Case "fulla web"
                                'cal tractar-ho
                            Case Else
                        End Select
                        'Esborro les relacions	
                        esborrarRelacions(objConn, rel, esborrarRecursiu, tipusEsborrat)
                        GAIA2.log(objConn, rel, codiUsuari, "", tipusLog)
                    End If 'Fi de EsborrarRecursiu=1		

                    'GAIA.bdSR(objConn, "delete FROM METLLCE WHERE LCEINLCW LIKE '%node=" & rel.inpar & "%' OR LCEINLCW LIKE '%node=" & rel.infil & "%'")
                    GAIA2.esborrarCelles(Nothing, "CELCDNOD=" & rel.inpar & " OR CELCDNOD=" & rel.infil)
                    GAIA2.bdSR(objConn, "delete FROM METLCEL WHERE CELCDNOD=" & rel.inpar & " OR CELCDNOD=" & rel.infil)
                    'Si s'està esborrant un node organigrama, he d'esborrar els permisos
                    'però no ho faig, per poder recuperar nodes esborrats per error								
                End If
            End If
            If ((tipusEsborrat = ctESBORRATMANUAL) Or (tipusEsborrat = ctESBORRATCADUCAT)) And rel.cdsit < 95 Then
                'Miro de esborrar on toqui.

                GAIA2.afegeixAccioManteniment(objConn, rel, 0, 99, Now, Now, rel, 1, 0, True)


            End If
        End If




        DS.Dispose()



    End Function 'esborrarNode

    '******************************************************************

    '	Funció: Esborra relacions

    '	Entrada: 
    ' codiRelació: codi de la relació que vull esborrar
    '	Procés: 	
    '		esborra les relacions del node.

    '	Si esborro les relacions com a pare, he de modificar totes les relacions cap avall per treure el node
    ' del path.!!!!
    '******************************************************************
    Public Shared Sub esborrarRelacions(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal esborrarRecursiu As Integer, ByVal tipusEsborrat As Integer)

        Dim nouEstat As Integer = 1

        If rel.cdsit = 96 Or rel.cdsit = 97 Or rel.cdsit = 95 Then
            nouEstat = (tipusEsborrat * 100) + rel.cdsit 'poso els tipus 9996, 9997, 9896, 9897
        Else
            nouEstat = tipusEsborrat
        End If


        GAIA2.bdSR(objConn, "UPDATE  METLREL SET RELCDSIT=" + nouEstat.ToString() + " WHERE RELINCOD=" + rel.incod.ToString())


        ' Si no s'esborraran els fills he de pujar-los un nivell (el del pare)
        If esborrarRecursiu = 0 Then
            'Poso els fills al mateix nivell del pare
            'GAIA.modificaRelacio(objconn, rel, rel.cdher, rel.cdarb, rel.inpar,0,0,-1,0)		
        Else
            Dim DS As DataSet
            Dim dbRow As DataRow
            DS = New DataSet()
            Dim sit As Integer = 0
            GAIA2.bdr(objConn, "SELECT * FROM METLREL WITH(NOLOCK) WHERE RELCDHER  LIKE '" + rel.cdher + "_" + rel.infil.ToString() + "%'", DS)

            For Each dbRow In DS.Tables(0).Rows
                sit = dbRow("RELCDSIT")
                Select Case sit
                    Case 95
                        nouEstat = (tipusEsborrat * 100) + sit
                    Case 96
                        nouEstat = (tipusEsborrat * 100) + sit
                    Case 97
                        nouEstat = (tipusEsborrat * 100) + sit
                    Case 9895
                        nouEstat = sit
                    Case 9995
                        nouEstat = sit
                    Case 9896
                        nouEstat = sit
                    Case 9996
                        nouEstat = sit
                    Case 9897
                        nouEstat = sit
                    Case 9997
                        nouEstat = sit

                    Case Else
                        nouEstat = tipusEsborrat

                End Select



                GAIA2.bdSR(objConn, "UPDATE  METLREL SET RELCDSIT=" + nouEstat.ToString() + " WHERE RELINCOD=" + dbRow("RELINCOD").ToString())
            Next dbRow
            DS.Dispose()





        End If

    End Sub
    '******************************************************************
    '	Funció: esborrarFullaCatalegServeis
    '	Entrada: 
    '		codiNode: node a esborrar
    '	Procés: 	
    '		Esborra la fulla del catàleg de serveis
    '******************************************************************
    Public Shared Sub esborrarFullaCatalegServeis(ByVal objConn As OleDbConnection, ByVal codiNode As Integer)
        GAIA2.bdSR(objConn, "DELETE  FROM CSTLRAC WHERE RACINCOD=" & codiNode)
        GAIA2.bdSR(objConn, "DELETE  FROM CSTLACC WHERE ACCINNOD=" & codiNode)
    End Sub 'esborrarFullaCatalegServeis

    '******************************************************************
    '	Funció: esborrarFullaNoticia
    '	Entrada: 
    '		codiNode: node a esborrar
    '	Procés: 	
    '		Esborra la fulla "noticia"
    ' Falta fer comprovació d'error
    '******************************************************************
    Public Shared Sub esborrarFullaNoticia(ByVal objConn As OleDbConnection, ByVal codiNode As Integer)
        GAIA2.bdSR(objConn, "DELETE  FROM METLNOT WHERE NOTINNOD=" & codiNode)
    End Sub

    '******************************************************************
    '	Funció: esborrarFullaLink
    '	Entrada: 
    '		codiNode: node a esborrar
    '	Procés: 	
    '		Esborra la fulla "link"
    ' Falta fer comprovació d'error
    '******************************************************************
    Public Shared Sub esborrarFullaLink(ByVal objConn As OleDbConnection, ByVal codiNode As Integer)
        GAIA2.bdSR(objConn, "DELETE  FROM METLLNK WHERE LNKINNOD=" & codiNode)
    End Sub


    '******************************************************************
    '	Funció: esborrarFullaWeb
    '	Entrada: 
    '		codiNode: fulla a esborrar
    '	Procés: 	
    '		Esborra la fulla "web"
    ' 
    '******************************************************************
    Public Shared Sub esborrarFullaWeb(ByVal objConn As OleDbConnection, ByVal codiNode As Integer)
        GAIA2.bdSR(objConn, "DELETE FROM METLWEB WHERE WEBINNOD=" & codiNode)

    End Sub



    '******************************************************************
    '	Funció: esborrarFullaPlantillaWeb
    '	Entrada: 
    '		codiNode: fulla a esborrar
    '	Procés: 	
    '		Esborra la fulla "web"
    ' 
    '******************************************************************
    Public Shared Sub esborrarFullaPlantillaWeb(ByVal objConn As OleDbConnection, ByVal codiNode As Integer)
        GAIA2.bdSR(objConn, "DELETE FROM METLPLT WHERE PLTINNOD=" & codiNode)
    End Sub 'esborrarFullaPlantillaWeb



    '******************************************************************
    '	Funció: esborrarFullaCodiWeb
    '	Entrada: 
    '		codiNode: fulla a esborrar
    '	Procés: 	
    '		Esborra la fulla "codiWeb"
    ' 
    '******************************************************************
    Public Shared Sub esborrarFullaCodiWeb(ByVal objConn As OleDbConnection, ByVal codiNode As Integer)

        GAIA2.bdSR(objConn, "DELETE FROM METLLCW WHERE LCWINNOD=" & codiNode)
    End Sub 'esborrarFullaCodiWeb



    '******************************************************************
    '	Funció: esborrarFullaCircuit
    '	Entrada: 
    '		codiNode: fulla a esborrar
    '	Procés: 	
    '		Esborra la fulla "circuit"
    ' 
    '******************************************************************
    Public Shared Sub esborrarFullaCircuit(ByVal objConn As OleDbConnection, ByVal codiNode As Integer)
        GAIA2.bdSR(objConn, "DELETE FROM METLFCI WHERE FCIINNOD=" & codiNode)
    End Sub 'esborrarFullaCircuit

    '******************************************************************
    '	Funció: esborrarNodeOrganigrama
    '	Entrada: 
    '		codiNode: fulla a esborrar
    '	Procés: 	
    '		Esborra la fulla "nodeOrganigrama"
    ' 
    '******************************************************************
    Public Shared Sub esborrarNodeOrganigrama(ByVal objConn As OleDbConnection, ByVal codiNode As Integer)
        GAIA2.bdSR(objConn, "DELETE FROM METLNOR WHERE NORINNOD=" & codiNode)
    End Sub 'esborrarNodeOrganigrama



    '******************************************************************
    '	Funció: esborrarFullaOrganigrama
    '	Entrada: 
    '		codiNode: fulla a esborrar
    '	Procés: 	
    '		Esborra la fulla "fullaOrganigrama"
    ' 

    '******************************************************************
    Public Shared Sub esborrarFullaOrganigrama(ByVal objConn As OleDbConnection, ByVal codiNode As Integer)
        GAIA2.bdSR(objConn, "DELETE FROM METLFOR WHERE FORINNOD=" & codiNode)
    End Sub 'fullaOrganigrama



    '******************************************************************
    '	Funció: esborrarFullaMissatge
    '	Entrada: 
    '		codiNode: fulla a esborrar
    '	Procés: 	
    '		Esborra la fulla "fullaMissatge"
    ' 
    '******************************************************************
    Public Shared Sub esborrarFullaMissatge(ByVal objConn As OleDbConnection, ByVal codiNode As Integer)
        GAIA2.bdSR(objConn, "DELETE FROM METLMIS WHERE MISINNOD=" & codiNode)
    End Sub 'fullaOrganigrama



    '******************************************************************
    '	Funció: esborrarNodeElMeuEspai
    '	Entrada: 
    '		codiNode: node a esborrar
    '	Procés: 	
    '		Esborra la fulla "nodeElMeuEspai"
    ' 
    '******************************************************************
    Public Shared Sub esborrarNodeElMeuEspai(ByVal objConn As OleDbConnection, ByVal codiNode As Integer)
        GAIA2.bdSR(objConn, "DELETE FROM METLNES WHERE NESINNOD=" & codiNode)
    End Sub 'esborrarNodeElMeuEspai




    '******************************************************************
    '	Funció: esborrarFullaDocument
    '	Entrada: 
    '		codiRelacio: codi de la relació que apuntava al document que volem esborrar
    '		codiUsuari: codiUsuari GAIA que ha iniciat l'acció
    '	Procés: 	
    '		Esborra la fulla "document" i esborro el document físicament del servidor
    ' Falta fer comprovació d'error
    '******************************************************************
    Public Shared Sub esborrarFullaDocument(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiUsuari As Integer)
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim fitxer As String
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT DOCWNBOR, DOCDSFIT, isnull(RELDSFIT, '') as RELDSFIT, RELCDHER FROM METLREL WITH(NOLOCK),METLDOC WITH(NOLOCK) WHERE RELINCOD=" & rel.incod & "  AND DOCINNOD=RELINFIL ", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            If dbRow("RELDSFIT") = "" Then
                fitxer = dbRow("DOCDSFIT")
            Else
                fitxer = dbRow("RELDSFIT")
            End If


            Dim path As String()
            path = Split(dbRow("RELCDHER"), "_")
            If path.Length > 0 Then
                'Busco si el fitxer s'està utilitzant a més d'una pàgina																			
                Dim port, URL, servidor, usuariFTP, pwdFTP, pathDestiArrel, pathDocsDesti As String
                pathDocsDesti = ""
                pathDestiArrel = ""
                servidor = ""
                usuariFTP = ""
                pwdFTP = ""
                port = ""
                URL = ""
                GAIA2.trobaServidorDesti(objConn, rel, servidor, usuariFTP, pwdFTP, pathDestiArrel, pathDocsDesti, port, URL)
                GAIA2.ftpEsborrarFitxer(objConn, servidor, usuariFTP, pwdFTP, pathDocsDesti & "/" & fitxer, codiUsuari, rel, dbRow("DOCWNBOR"))

            End If
        End If
        DS.Dispose()
    End Sub 'esborrarFullaDocument



    '******************************************************************
    '	Funció: descTipusNode
    '	Entrada: 
    '		tipusNode: tipus de node
    '	Procés: 	
    '		Cerca dins de la taula de tipus de nodes i retorna el nom del tipus
    '	Sortida:
    '		desc tipusNode
    '******************************************************************
    Public Shared Function descTipusNode(ByVal objConn As OleDbConnection, ByVal tipusNode As Integer) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        descTipusNode = ""
        GAIA2.bdr(objConn, "SELECT TIPDSDES FROM METLTIP WITH(NOLOCK)   WHERE TIPINTIP=" & tipusNode.ToString() & " ORDER BY  TIPCDVER DESC ", DS)
        For Each dbRow In DS.Tables(0).Rows
            descTipusNode = dbRow("TIPDSDES").Trim

        Next dbRow
        DS.Dispose()
    End Function

    '******************************************************************
    '	Funció: descNode
    '	Entrada: 
    '		codiNode: identificador del node
    '	Procés: 	

    '		Cerca dins de la taula de nodes i retorna la descripció
    '	Sortida:
    '		descripció del node
    '******************************************************************
    Public Shared Function descNode(ByVal objConn As OleDbConnection, ByVal codiNode As Integer) As String
        Dim DS As DataSet

        Dim dbRow As DataRow
        DS = New DataSet()
        descNode = ""
        GAIA2.bdr(objConn, "SELECT NODDSTXT FROM METLNOD WITH(NOLOCK)   WHERE NODINNOD=" & codiNode.ToString(), DS)
        For Each dbRow In DS.Tables(0).Rows
            descNode = GAIA2.netejaHTML(dbRow("NODDSTXT").Trim)
        Next dbRow
        DS.Dispose()
    End Function




    '******************************************************************
    '	Funció: Insertar Node
    '	Entrada: 
    '		tipusNode 	: Tipus de node
    '		nomNode		: Nom del Node
    '		codiOrg	: String amb el nif del creador del node	
    '	Procés: 	
    '		Inserta el node en la taula de nodes d'informació
    '	Sortida:
    '		Codi del node insertat
    '******************************************************************
    Public Shared Function insertarNode(ByVal objConn As OleDbConnection, ByVal tipusNode As Integer, ByVal nomNode As String, ByVal codiOrg As String, Optional ByVal nodswsit As Integer = 0, Optional ByVal noddtpub As DateTime = Nothing, Optional ByVal noddtcad As DateTime = Nothing) As Integer
        Dim noddttim As DateTime
        noddttim = Now
        If noddtpub = Nothing Then noddtpub = CDate("1/1/1900")
        If noddtcad = Nothing Then noddtcad = CDate("1/1/1900")
        insertarNode = GAIA2.bdSR_id(objConn, "INSERT INTO METLNOD (NODCDTIP,NODDSTXT,NODCDUSR,NODDTTIM,NODSWSIT,NODDTPUB,NODDTCAD) VALUES (" & tipusNode.ToString() & ",'" & GAIA2.netejaHTML(nomNode.Replace("'", "''")) & "','" & codiOrg & "','" & noddttim & "'," & nodswsit & ",'" & noddtpub & "','" & noddtcad & "')")

    End Function


    '******************************************************************
    '	Funció: moureNode
    '	Entrada: 
    '		nroArbreOrigen 	: Arbre on s'ubica el node que volem moure

    '		nroArbreDesti		: Arbre on ubicarem el node

    '		nroNodeOrigen	: Codi del node que volem moure
    '		nroNodeDesti : Codi del node d'on penjarem el "nroNodeOrigen"
    '		moureFills: true si he de moure el node i tots els fills que penjen. False, només he de moure el node demanat i
    '								els seus fills penjaran del node pare del nroNodeOrigen.
    '		nodePath : pare del nroNodeDestí. Amb aixó busco la relació entre nroNodeDestí i nodePath i 
    '							resulta un path cap l'arrel que propagaré al nou node.
    '		posicioEstructura: cel·la on ubicarem el node dins de l'estructura
    '		tipusMoviment: 	0: Insertar node
    '										1: Moure node.
    '	Procés: 	
    '		Si moureFills=False:
    '			Mou el node origen d'un arbre origen a un arbre i node destí.
    '			Alhora eliminem els permisos heretats que pogués tenir en l'anterior ubicació.
    '	 		(i les herències de permisos que això implica) i els recreo en el nou destí.
    ' 		
    '		Si moureFills=1:
    '			Mou el node origen i tots els seus fills d'un arbre origen a un arbre i node destí.
    '			Si pot canvia les posicions de les relacions dels fills segons la nova plantilla heretada.
    '			Elimino els permisos heretats que pogués tenir en la ubicació original i li assigno els nous
    '			de l'ubicació destí.
    '			Faig el mateix per a tots els seus fills'
    '
    '		 Si tipusMoviment=1 => Insertar Node
    '				He de penjar el node origen del node desti
    '		 Si tipusMoviment=0 => Moure NOde
    '				He de penjar el node origen del pare del node destí i a pel que fa a l'ordre de nodes, a continuació del node desti

    'Canvi d'ordre

    ' Si el node destí es el pare del node origen, he d'assignar al node origen el valor RELCDORD=1 i a tots els altres incrementaré en 1 el valor de l'ordre, alhora de fer aquest increment, elimino els forats en la seqüència de l'ordre
    ' Si el node destí no es el pare del node origen, he d'assignar al node origen el valor de RELCDORD del nodeDestí + 1 i a tots els altres incremetaré en 1 el valor de RELCDORD, alhora de fer aquest increment, elimino els forats en la seqüència de l'ordre
    '	

    '	Sortida:
    '******************************************************************


    Public Shared Sub moureNode(ByVal objConn As OleDbConnection, ByRef relOrigen As clsRelacio, ByVal relDesti As clsRelacio, ByVal moureFills As Integer, ByVal posicioEstructura As Integer, ByVal tipusMoviment As Integer, ByVal relDestiInicial As clsRelacio, ByVal tipusOrdre As Integer)



        Dim NroNodePareAnterior As Integer
        Dim nroArbreOrigen, nroArbreDesti, nroNodeOrigen, nroNodeDesti As Integer
        Dim nodePathNou, nodePathVell As String
        '**************************************************************************************************************************************
        '	Desactivo l'opció moureNode sense moure fills. Provoca errors als usuaris als usuaris i millor no fer-ho per ara. MaxSV. 15/11/06
        '**************************************************************************************************************************************


        'Poso l'estat de la relacio a 1: estat inicial
        If relOrigen.cdsit <> 96 And relOrigen.cdsit <> 97 And relOrigen.cdsit <> 95 Then
            GAIA2.canviEstatRelacio(objConn, relOrigen, 1, 0)
        End If
        nroArbreOrigen = relOrigen.cdarb
        nroNodeOrigen = relOrigen.infil
        nodePathVell = relOrigen.cdher
        NroNodePareAnterior = relOrigen.inpar
        nroArbreDesti = relDesti.cdarb
        nroNodeDesti = relDesti.infil
        nodePathNou = relDesti.cdher + "_" + relDesti.infil.ToString()

        'si he de moure el node , he de canviar el node destí, que pasa a ser el node pare apuntat per la relacio: codiRelacioDesti
        ' Només faig un moviment si el tipusmoviment=0 i el node a on volem moure no és l'arrel de l'arbre

        'Modifico la relació del node que vull moure.
        GAIA2.modificaRelacio(objConn, relOrigen, nodePathNou, nroArbreDesti, nroNodeDesti, 0, posicioEstructura, -1, 1)

        'Assigno els permisos heretats a la nova ubicació		
        'clsPermisos.tractaPermisosHeretats(objConn, nodePathNou, nroNodeOrigen, "", "", 1)
        clsPermisos.tractaPermisosHeretats2(objConn, relDesti, relDesti.infil, "", True, True)


        ' Faig el manteniment de l'ordre de les relacions

        GAIA2.canviOrdre(objConn, relOrigen, relDesti, moureFills, tipusOrdre, relDestiInicial)

        'Arregla estructura de nodes segons plantilla		
        If relOrigen.cdsit <> 96 And relOrigen.cdsit <> 95 And relOrigen.cdsit <> 97 Then
            GAIA2.corregirErrorsEstructura(objConn, relOrigen, 1, posicioEstructura)
        End If
    End Sub 'moureNode




    'Fa una copia d'un contingut i retorna la nova relació creada
    Public Shared Function ferCopiaContingut(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiUsuari As Integer, Optional ByRef prefix As String = "", Optional ByVal reldesti As clsRelacio = Nothing) As Integer
        Dim docAnt As String = ""
        Dim codiNode As Integer
        Dim nomDocument As String = ""
        Dim savePath As String = "e:\docs\GAIA\"
        'Dim savePath As String = "y:\"
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim novaRel As New clsRelacio
        DS = New DataSet()


        If prefix = "" Then
            prefix = "C. de "
        End If
        If prefix = "-1" Then
            prefix = ""
        End If
        '1- creo node (METLNOD)
        '2- Per a tots el idiomes
        '2.1- creo copia de fitxer 
        '2.2- creo METLDOC amb dades de l'original (tot menys DOCINNOD i DOCDSFIT)

        '3- Per a totes les relacions on apareixia rel.infil creo METLREL i copio permisos

        '1
        codiNode = GAIA2.insertarNode(objConn, rel.tipintip, prefix + rel.noddstxt, codiUsuari)

        '2

        If reldesti Is Nothing Then
            reldesti = New clsRelacio()

            reldesti.bdget(objConn, rel.cdrsu)
        End If

        Select Case UCase(rel.tipdsdes.Trim())
            Case "NODE WEB"

                GAIA2.bdr(objConn, "SELECT NWEINNOD, NWEINIDI, isNull(NWEDSTIT,'') as NWEDSTIT, NWEDSUSR,NWEDSTHOR, NWEDSTVER, NWEDSTCO,NWEDSPLA, NWEDSEST, NWEDSATR, NWEDSLCW, NWEDSCAR, isnull(NWEDSCSS,'') as NWEDSCSS,NWEDSEBO,isnull(NWEDSMET,'') as NWEDSMET,isnull(NWEDSPEU,'') as NWEDSPEU, isnull(NWEDSCSP,'') as NWEDSCSP, isnull(NWEDSCSI,'') as NWEDSCSI FROM METLNWE  WITH(NOLOCK) WHERE NWEINNOD=" & rel.infil, DS)
                For Each dbRow In DS.Tables(0).Rows
                    GAIA2.bdSR(objConn, "INSERT INTO METLNWE (NWEINNOD, NWEINIDI,NWEDSTIT, NWEDSUSR,NWEDSTHOR, NWEDSTVER, NWEDSTCO,NWEDSPLA, NWEDSEST, NWEDSATR, NWEDSLCW, NWEDSCAR, NWEDSCSS,NWEDSEBO,NWEDSMET, NWEDSPEU, NWEDSCSP, NWEDSCSI) VALUES (" & codiNode & "," & dbRow("NWEINIDI") & ",'" & dbRow("NWEDSTIT").Replace("'", "''") & "','" & dbRow("NWEDSUSR") & "','" & dbRow("NWEDSTHOR") & "','" & dbRow("NWEDSTVER") & "','" & dbRow("NWEDSTCO") & "','" & dbRow("NWEDSPLA") & "','" & dbRow("NWEDSEST") & "','" & dbRow("NWEDSATR") & "','" & dbRow("NWEDSLCW") & "','" & dbRow("NWEDSCAR") & "','" & dbRow("NWEDSCSS").replace("'", "''") & "','" & dbRow("NWEDSEBO") & "','" & dbRow("NWEDSMET").Replace("'", "''") & "','" & dbRow("NWEDSPEU").Replace("'", "''") & "','" & dbRow("NWEDSCSP").Replace("'", "''") & "','" & dbRow("NWEDSCSI").Replace("'", "''") & "')")
                Next dbRow

                novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)

            Case "NODE DOCUMENT"
                GAIA2.bdr(objConn, "SELECT NDOINNOD,NDOINIDI,isNull(NDODSTIT,'') as NDODSTIT, NDODSUSR, isnull(NDODSOBS,'') as NDODSOBS FROM METLNDO  WITH(NOLOCK) WHERE NDOINNOD=" & rel.infil, DS)
                For Each dbRow In DS.Tables(0).Rows
                    GAIA2.bdSR(objConn, "INSERT INTO METLNDO (NDOINNOD,NDOINIDI,NDODSTIT, NDODSUSR,NDODSOBS) VALUES (" & codiNode & "," & dbRow("NDOINIDI") & ",'" & dbRow("NDODSTIT").Replace("'", "''") & "','" & dbRow("NDODSUSR") & "','" & dbRow("NDODSOBS").Replace("'", "''") + "')")
                Next dbRow
                novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)
            Case "FULLA DOCUMENT"
                Dim r As New Random()
                Dim hDocs As New Dictionary(Of String, String)


                Dim pos As Integer
                Dim nomVell As String = ""
                ' Creo objectes METLDOC

                GAIA2.bdr(objConn, "select isnull(REIDSFI2,'') as REIDSFI2, METLDOC.* FROM METLDOC  WITH(NOLOCK) INNER JOIN METLREL  WITH(NOLOCK) ON RELINFIL=DOCINNOD LEFT OUTER JOIN METLREI  WITH(NOLOCK) ON REIINCOD=RELINCOD WHERE DOCINNOD=" & rel.infil & " ORDER BY DOCDSFIT", DS)

                docAnt = ""
                Dim docAct As String = ""
                For Each dbRow In DS.Tables(0).Rows
                    If dbRow("REIDSFI2") <> "" Then
                        nomVell = dbRow("REIDSFI2")
                    Else
                        nomVell = dbRow("DOCDSFIT")
                    End If
                    docAct = dbRow("DOCDSFIT")
                    If docAnt <> docAct Then
                        docAnt = docAct
                        pos = InStrRev(nomVell, ".")
                        nomDocument = "d" & r.Next(9999999).ToString() & nomVell.Substring(pos - 1)
                        While File.Exists(savePath & nomDocument)
                            nomDocument = "d" & r.Next(9999999).ToString() & nomVell.Substring(pos - 1)
                        End While

                        If Not (hDocs.ContainsKey(nomVell)) Then
                            hDocs.Add(nomVell, nomDocument)
                        End If
                        GAIA2.bdSR(objConn, "INSERT INTO METLDOC (DOCINNOD,DOCINIDI,DOCINTDO,DOCDSTIT,DOCDSFIT,DOCDSFON, DOCDSAUT, DOCDSISB,DOCDSDES,DOCDTANY,DOCDTCAD,DOCDTPUB,DOCWNHOR, DOCWNVER,DOCDSALT, DOCDSLNK, DOCWNSIZ,DOCWNUPD, DOCWNBOR) VALUES (" + codiNode.ToString() + "," + dbRow("DOCINIDI").ToString() + "," + dbRow("DOCINTDO").ToString() + ",'" & prefix + dbRow("DOCDSTIT").Replace("'", "''") + "','" + nomDocument + "','" + dbRow("DOCDSFON").Replace("'", "''") + "','" + dbRow("DOCDSAUT").Replace("'", "''") + "','" + dbRow("DOCDSISB").Replace("'", "''") + "','" + dbRow("DOCDSDES").Replace("'", "''") + "','" + dbRow("DOCDTANY").ToString() + "','" + dbRow("DOCDTCAD").ToString() + "','" + dbRow("DOCDTPUB").ToString() + "'," + dbRow("DOCWNHOR").ToString() + "," + dbRow("DOCWNVER").ToString() + ",'" + dbRow("DOCDSALT").Replace("'", "''") + "','" + dbRow("DOCDSLNK") + "'," + dbRow("DOCWNSIZ").ToString() + ",1,'" + dbRow("DOCWNBOR").ToString() + "')")
                        '******************************************************+
                        'Faig copia de fitxers 
                        '******************************************************+

                        File.Copy(savePath & nomVell, savePath & nomDocument)

                        If InStr(nomVell, ".jpg") > 0 Then
                            'si és una imatge he de copiar el nomVell, el P i el P100 
                            File.Copy(savePath & nomVell.Replace(".jpg", "P.jpg"), savePath & nomDocument.Replace(".jpg", "P.jpg"))
                            File.Copy(savePath & nomVell.Replace(".jpg", "P100.jpg"), savePath & nomDocument.Replace(".jpg", "P100.jpg"))


                        End If
                    End If
                Next dbRow

                '********!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                If reldesti Is Nothing Then
                    reldesti.bdget(objConn, rel.cdrsu)
                End If
                'copio relacions
                GAIA2.bdr(objConn, "SELECT *	FROM METLREL  WITH(NOLOCK) WHERE RELCDRSU=" & rel.cdrsu & " AND RELCDSIT<99", DS)
                For Each dbRow In DS.Tables(0).Rows
                    novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)

                Next dbRow
                'copio METLREI
                GAIA2.bdr(objConn, "SELECT * FROM METLREI  WITH(NOLOCK) WHERE REIINCOD=" & rel.incod, DS)
                For Each dbRow In DS.Tables(0).Rows
                    Try

                        GAIA2.bdSR(objConn, "INSERT INTO METLREI VALUES (" & novaRel.incod & "," & dbRow("REIINIDI") & ",'" & CDate(dbRow("REIDTPUB")) & "','" & CDate(dbRow("REIDTCAD")) & "','/gdocs/" & hDocs.Item(nomVell) & "','" & hDocs.Item(nomVell) & "','" & dbRow("REIDSHAS") & "')")
                    Catch ex As Exception
                        'no faig res si ja existia
                    End Try

                Next dbRow

            Case "FULLA PLANTILLAWEB"
                GAIA2.bdr(objConn, "SELECT * FROM METLPLT  WITH(NOLOCK) WHERE PLTINNOD=" & rel.infil, DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    GAIA2.bdSR(objConn, "INSERT INTO METLPLT (PLTINNOD, PLTDSTIT,PLTDSOBS, PLTDTANY, PLTDSUSR,PLTDSHOR,PLTDSVER,PLTDSNUM, PLTDSCMP,PLTDSEST,PLTDSATR,PLTDSCSS, PLTDSLNK,PLTDSALT, PLTDSIMG, PLTDSFLW, PLTDSTCO,PLTDSLCW, PLTDSLC2, PLTDSPLT, PLTDSALK, PLTCDPAL, PLTDSAAL,PLTDSALF, PLTDSNIV, PLTSWVIS) VALUES (" & codiNode & ",'" & dbRow("PLTDSTIT") & "','" & dbRow("PLTDSOBS") & "',getdate(),'" & codiUsuari & "','" & dbRow("PLTDSHOR") & "','" & dbRow("PLTDSVER") & "','" & dbRow("PLTDSNUM") & "','" & dbRow("PLTDSCMP") & "','" & dbRow("PLTDSEST") & "','" & dbRow("PLTDSATR") & "','" & dbRow("PLTDSCSS") & "','" & dbRow("PLTDSLNK") & "','" & dbRow("PLTDSALT") & "','" & dbRow("PLTDSIMG") & "','" & dbRow("PLTDSFLW") & "','" & dbRow("PLTDSTCO") & "','" & dbRow("PLTDSLCW") & "','" & dbRow("PLTDSLC2") & "','" & dbRow("PLTDSPLT") & "','" & dbRow("PLTDSALK") & "','" & dbRow("PLTCDPAL") & "','" & dbRow("PLTDSAAL") & "','" & dbRow("PLTDSALF") & "','" & dbRow("PLTDSNIV") & "'," & dbRow("PLTSWVIS") & ")")
                    novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)

                End If
            Case "FULLA WEB"
                GAIA2.bdr(objConn, "SELECT * FROM METLWEB  WITH(NOLOCK) WHERE WEBINNOD=" & rel.infil, DS)
                For Each dbRow In DS.Tables(0).Rows

                    GAIA2.bdSR(objConn, "INSERT INTO METLWEB (WEBINNOD, WEBINIDI,WEBDSTIT,WEBDSFIT, WEBDSURL, WEBDTPUB, WEBDTCAD, WEBDTANY, WEBDSUSR,WEBTPBUS,WEBDSTHOR, WEBDSTVER, WEBDSTCO, WEBDSPLA, WEBDSEST, WEBDSATR, WEBDSLCW, WEBDSCSS, WEBDSCIP, WEBDSCIC, WEBTPHER, WEBDSDEC, WEBCDRAL, WEBDSIMP,WEBDSCND, WEBWNMTH,WEBSWFRM, WEBSWEML,WEBDSEBO, WEBDSDES, WEBDSPCL, WEBSWSSL) VALUES (" & codiNode & "," & dbRow("WEBINIDI") & ",'" & dbRow("WEBDSTIT").replace("'", "''") & "','','','" & dbRow("WEBDTPUB") & "','" & dbRow("WEBDTCAD") & "','" & dbRow("WEBDTANY") & "','" & dbRow("WEBDSUSR") & "','" & dbRow("WEBTPBUS") & "','" & dbRow("WEBDSTHOR") & "','" & dbRow("WEBDSTVER") & "','" & dbRow("WEBDSTCO") & "','" & dbRow("WEBDSPLA") & "','" & dbRow("WEBDSEST") & "','" & dbRow("WEBDSATR") & "','" & dbRow("WEBDSLCW") & "','" & dbRow("WEBDSCSS") & "'," & dbRow("WEBDSCIP") & "," & dbRow("WEBDSCIC") & ", '" & dbRow("WEBTPHER") & "','" & dbRow("WEBDSDEC").Replace("'", "''") & "'," & dbRow("WEBCDRAL") & ",'" & dbRow("WEBDSIMP") & "','" & dbRow("WEBDSCND") & "'," & dbRow("WEBWNMTH") & ",'" & dbRow("WEBSWFRM") & "','" & dbRow("WEBSWEML") & "','" & dbRow("WEBDSEBO") & "','" & dbRow("WEBDSDES").replace("'", "''") & "','" & dbRow("WEBDSPCL").replace("'", "''") & "','" & dbRow("WEBSWSSL") & "')")

                Next dbRow
                novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)
            Case "FULLA TRAMIT"
                GAIA2.bdr(objConn, "SELECT * FROM METLFTR  WITH(NOLOCK) WHERE FTRINNOD=" & rel.infil, DS)


                For Each dbRow In DS.Tables(0).Rows
                    'GAIA.bdSR(objConn, "INSERT INTO METLFTR  (FTRDSNOM, FTRDSEEX, FTRDSDES, FTRDSQUI, FTRDSHOW, FTRDSPRS, FTRDSTEL, FTRDSWEB, FTRDSQUA, FTRDSOBS, FTRINIDI, FTRDSPRE, FTRDSTRE, FTRDSLEG, FTRDTPUB, FTRDTCAD, FTRDTMAN, FTRDSPCL, FTRDSCOM, FTRSWVIS, FTRDSCOR, FTRINUNI,FTRCDRTE, FTRINNOD, FTRDSALE, FTRDTALI, FTRDTALF, FTRSWCAR, FTRSWAOC, FTRCDSIL , FTRDSFAJ, FTRDSFDO,) VALUES ('" & dbRow("FTRDSNOM").replace("'", "''") & "','" & dbRow("FTRDSEEX").replace("'", "''") & "', '" & dbRow("FTRDSDES").replace("'", "''") & "', '" & dbRow("FTRDSQUI").replace("'", "''") & "', '" & dbRow("FTRDSHOW").replace("'", "''") & "', '" & dbRow("FTRDSPRS").replace("'", "''") & "', '" & dbRow("FTRDSTEL").replace("'", "''") & "', '" & dbRow("FTRDSWEB").replace("'", "''") & "', '" & dbRow("FTRDSQUA").replace("'", "''") & "', '" & dbRow("FTRDSOBS").replace("'", "''") & "', " & dbRow("FTRINIDI") & ", '" & dbRow("FTRDSPRE").replace("'", "''") & "', '" & dbRow("FTRDSTRE").replace("'", "''") & "', '" & dbRow("FTRDSLEG").replace("'", "''") & "', '" & dbRow("FTRDTPUB") & "', '" & dbRow("FTRDTCAD") & "', '" & dbRow("FTRDTMAN") & "', '" & dbRow("FTRDSPCL").replace("'", "''") & "', '" & dbRow("FTRDSCOM").replace("'", "''") & "', 0, '" & dbRow("FTRDSCOR").replace("'", "''") & "', " & dbRow("FTRINUNI") & ", '" & dbRow("FTRCDRTE") & "', " & codiNode & ", '" & dbRow("FTRDSALE").replace("'", "''") & "', '" & dbRow("FTRDTALI") & "', '" & dbRow("FTRDTALF") & "','" & dbRow("FTRSWCAR") & "', '" & dbRow("FTRSWAOC") & "', '" & dbRow("FTRCDSIL") & "', '" & dbRow("FTRDSFAJ").replace("'", "''") & "', '" & dbRow("FTRDSFDO").replace("'", "''") & "')")
                    GAIA2.bdSR(objConn, "INSERT INTO [METLFTR]  ([FTRINNOD],[FTRDSNOM], [FTRDSEEX], [FTRDSDES], [FTRDSQUI], [FTRDSPRS], [FTRDSTEL], [FTRDSWEB], [FTRDSQUA], [FTRDSOBS], [FTRINIDI], [FTRDSPRE], [FTRDSTRE], [FTRDSLEG], [FTRDTPUB], [FTRDTCAD], [FTRDTMAN], [FTRDSPCL], [FTRDSCOM], [FTRSWVIS],[FTRSWVSE],[FTRSWVIN],[FTRSWVWE],[FTRDSCOR], [FTRINUNI],[FTRCDRTE], [FTRDSALE], [FTRDTALI], [FTRDTALF], [FTRSWCAR], [FTRSWAOC], [FTRDSFAJ], [FTRDSFDO],[FTRDSTDO],[FTRDSPDO], [FTRDSDEC], [FTRCDORG], [FTRDSTER], [FTRSWNOT], [FTRSWTPV],  [FTRSWREG], [FTRSWIDN], [FTRDSTEC], [FTRDSOBJ], [FTRDSETP], [FTRDSIND] , [FTRSWVUD], [FTRDSQDC], [FTRDSAPD], [FTRDSFWE],[FTRCDTRA],[FTRCDTTE],[FTRCDTTT],[FTRCDTTS],[FTRCDUTS],	[FTRDSNOC],[FTRDSVAL],[FTRDSRAP],[FTRDSRAT],[FTRDSRAI],[FTRDSDFG],[FTRDSDIP],[FTRDSDIT],[FTRDSDII])  VALUES(" & codiNode & ", " & FormataCadena(dbRow("FTRDSNOM")) & ", " & FormataCadena(dbRow("FTRDSEEX")) & ", " & FormataCadena(dbRow("FTRDSDES")) & ", " & FormataCadena(dbRow("FTRDSQUI")) & ", " & FormataCadena(dbRow("FTRDSPRS")) & ", " & FormataCadena(dbRow("FTRDSTEL")) & ", " & FormataCadena(dbRow("FTRDSWEB")) & ", " & FormataCadena(dbRow("FTRDSQUA")) & ", " & FormataCadena(dbRow("FTRDSOBS")) & ", " & dbRow("FTRINIDI") & ", " & FormataCadena(dbRow("FTRDSPRE")) & ", " & FormataCadena(dbRow("FTRDSTRE")) & ", " & FormataCadena(dbRow("FTRDSLEG")) & ", " & FormataData(TractaNullCadena(dbRow("FTRDTPUB"))) & ", " & FormataData(TractaNullCadena(dbRow("FTRDTCAD"))) & ", " & FormataData(TractaNullCadena(dbRow("FTRDTMAN"))) & ", " & FormataCadena(dbRow("FTRDSPCL")) & ", " & FormataCadena(dbRow("FTRDSCOM")) & ", " & formataSwitchbd(dbRow("FTRSWVIS")) & ", " & formataSwitchbd(dbRow("FTRSWVSE")) & ", " & formataSwitchbd(dbRow("FTRSWVIN")) & ", " & formataSwitchbd(dbRow("FTRSWVWE")) & ", " & FormataCadena(dbRow("FTRDSCOR")) & ", " & dbRow("FTRINUNI") & ", '" & dbRow("FTRCDRTE") & "', " & FormataCadena(dbRow("FTRDSALE")) & ", " & FormataData(TractaNullCadena(dbRow("FTRDTALI"))) & ", " & FormataData(TractaNullCadena(dbRow("FTRDTALF"))) & ", " & formataSwitchbd(dbRow("FTRSWCAR")) & ", " & formataSwitchbd(dbRow("FTRSWAOC")) & ", " & FormataCadena(dbRow("FTRDSFAJ")) & ", " & FormataCadena(dbRow("FTRDSFDO")) & ", " & FormataCadena(dbRow("FTRDSTDO")) & ", " & FormataCadena(dbRow("FTRDSPDO")) & ", " & FormataCadena(dbRow("FTRDSDEC")) & ", " & FormataInteger(dbRow("FTRCDORG")) & ", " & FormataCadena(dbRow("FTRDSTER")) & ", " & formataSwitchbd(dbRow("FTRSWNOT")) & ", " & formataSwitchbd(dbRow("FTRSWTPV")) & ", " & formataSwitchbd(dbRow("FTRSWREG")) & ", " & formataSwitchbd(dbRow("FTRSWIDN")) & ", " & FormataCadena(dbRow("FTRDSTEC")) & ", " & FormataCadena(dbRow("FTRDSOBJ")) & ", " & FormataCadena(dbRow("FTRDSETP")) & ", " & FormataCadena(dbRow("FTRDSIND")) & ", " & formataSwitchbd(dbRow("FTRSWVUD")) & ", " & FormataCadena(dbRow("FTRDSQDC")) & ", " & FormataCadena(dbRow("FTRDSAPD")) & ", " & FormataCadena(dbRow("FTRDSFWE")) & ", " & TractaNullInteger(dbRow("FTRCDTRA")) & ", " & FormataCadena(dbRow("FTRCDTTE")) & ", " & FormataCadena(dbRow("FTRCDTTT")) & ", " & FormataCadena(dbRow("FTRCDTTS")) & ", " & TractaNullInteger(dbRow("FTRCDUTS")) & "," & FormataCadena(dbRow("FTRDSNOC")) & "," & FormataCadena(dbRow("FTRDSVAL")) & "," & FormataCadena(dbRow("FTRDSRAP")) & "," & FormataCadena(dbRow("FTRDSRAT")) & "," & FormataCadena(dbRow("FTRDSRAI")) & "," & FormataCadena(dbRow("FTRDSDFG")) & "," & FormataCadena(dbRow("FTRDSDIP")) & "," & FormataCadena(dbRow("FTRDSDIT")) & "," & FormataCadena(dbRow("FTRDSDII")) & ")")

                Next dbRow
                novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)
                'copio associacions
                Dim DS2 As DataSet
                Dim dbRow2 As DataRow
                DS2 = New DataSet()
                GAIA2.bdr(objConn, "SELECT * FROM METLASS WHERE ASSCDNOD=" & rel.infil & " AND ASSCDTIP=51", DS2)
                For Each dbRow2 In DS2.Tables(0).Rows
                    GAIA2.bdSR(objConn, "INSERT INTO METLASS (ASSCDTPA,ASSCDNOD,ASSCDTIP,ASSCDNRL) VALUES (" & dbRow2("ASSCDTPA") & "," & codiNode & ", 51," & dbRow2("ASSCDNRL") & ")")
                Next dbRow2
                DS2.Dispose()

            Case "FULLA CODIWEB"
                GAIA2.bdr(objConn, "SELECT * FROM METLLCW  WITH(NOLOCK) WHERE LCWINNOD=" & rel.infil, DS)

                For Each dbRow In DS.Tables(0).Rows
                    GAIA2.bdSR(objConn, "INSERT INTO METLLCW (LCWINNOD, LCWINIDI, LCWDSTIT,LCWDSUSR,LCWDSTXT, LCWCDTIP,LCWTPFOR,LCWTPFOL) VALUES (" & codiNode & "," & dbRow("LCWINIDI") & ",'" & dbRow("LCWDSTIT").replace("'", "''") & "','" & dbRow("LCWDSUSR") & "','" & dbRow("LCWDSTXT").replace("'", "''") & "'," & dbRow("LCWCDTIP") & ",'" & dbRow("LCWTPFOR") & "','" & dbRow("LCWTPFOL") & "')")
                Next dbRow
                novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)

                'copio totes les relacions afectades
                GAIA2.bdr(objConn, "SELECT * FROM METLASS  WITH(NOLOCK) WHERE  ASSCDNOD=" & rel.infil, DS)
                For Each dbRow In DS.Tables(0).Rows
                    GAIA2.bdSR(objConn, "INSERT INTO METLASS (ASSCDTPA,ASSCDNOD,ASSCDTIP,ASSCDNRL) VALUES (" & dbRow("ASSCDTPA") & "," & codiNode & ",51," & dbRow("ASSCDNRL") & ")")
                Next dbRow


            Case "FULLA LINK"
                GAIA2.bdr(objConn, "SELECT * FROM METLLNK  WITH(NOLOCK) WHERE LNKINNOD=" & rel.infil, DS)
                For Each dbRow In DS.Tables(0).Rows
                    GAIA2.bdSR(objConn, "INSERT INTO METLLNK (LNKINNOD, LNKINIDI, LNKDSTXT, LNKWNTIP, LNKDTPUB, LNKDTCAD, LNKDSUSR,  LNKCDREL, LNKDSDES, LNKDSLNK, LNKDSVID) VALUES (" & codiNode & "," & dbRow("LNKINIDI") & ",'" & dbRow("LNKDSTXT").replace("'", "''") & "'," & dbRow("LNKWNTIP") & ",'" & dbRow("LNKDTPUB") & "','" & dbRow("LNKDTCAD") & "','" & dbRow("LNKDSUSR") & "'," & IIf(IsDBNull(dbRow("LNKCDREL")), "null", dbRow("LNKCDREL")) & ",'" & dbRow("LNKDSDES").replace("'", "''") & "', '" & dbRow("LNKDSLNK") & "','" & dbRow("LNKDSVID") & "')")
                Next dbRow
                novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)
                'copio METLREI
                GAIA2.bdr(objConn, "SELECT * FROM METLREI WITH(NOLOCK)  WHERE REIINCOD=" & rel.incod, DS)
                For Each dbRow In DS.Tables(0).Rows
                    Try
                        GAIA2.bdSR(objConn, "INSERT INTO METLREI VALUES (" & novaRel.incod & "," & dbRow("REIINIDI") & ",'" & dbRow("REIDTPUB") & "','" & dbRow("REIDTCAD") & "','" & dbRow("REIDSFIT") & "','" & dbRow("REIDSFI2") & "','" & dbRow("REIDSHAS") & "')")
                    Catch
                        'no faig res si ja existia
                    End Try

                Next dbRow

            Case "FULLA NOTICIA"
                GAIA2.bdr(objConn, "SELECT * FROM METLNOT  WITH(NOLOCK) WHERE NOTINNOD=" & rel.infil, DS)
                For Each dbRow In DS.Tables(0).Rows
                    GAIA2.bdSR(objConn, "insert into METLNOT (NOTINNOD, NOTINIDI,NOTDSTIT, NOTDSSUB, NOTDSAVN, NOTDSTXT, NOTDSRES,  NOTDTPUB, NOTDTCAD, NOTDTANY, NOTDSUSR,NOTDSLNK,NOTDSVID)  VALUES (" & codiNode & "," & dbRow("NOTINIDI") & ",'" & dbRow("NOTDSTIT").replace("'", "''") & "','" & dbRow("NOTDSSUB").replace("'", "''") & "','" & dbRow("NOTDSAVN").replace("'", "''") & "','" & dbRow("NOTDSTXT").replace("'", "''") & "','" & dbRow("NOTDSRES").replace("'", "''") & "','" & dbRow("NOTDTPUB") & "','" & dbRow("NOTDTCAD") & "','" & dbRow("NOTDTANY") & "','" & dbRow("NOTDSUSR") & "','" & dbRow("NOTDSLNK") & "','" & dbRow("NOTDSVID") & "')")
                Next dbRow
                novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)
                'copio METLREI
                GAIA2.bdr(objConn, "SELECT * FROM METLREI  WITH(NOLOCK) WHERE REIINCOD=" & rel.incod, DS)
                For Each dbRow In DS.Tables(0).Rows
                    Try
                        GAIA2.bdSR(objConn, "INSERT INTO METLREI VALUES (" & novaRel.incod & "," & dbRow("REIINIDI") & ",'" & dbRow("REIDTPUB") & "','" & dbRow("REIDTCAD") & "','" & dbRow("REIDSFIT") & "','" & dbRow("REIDSFI2") & "','" & dbRow("REIDSHAS") & "')")
                    Catch
                        'no faig res si ja existia
                    End Try

                Next dbRow

            Case "FULLA AGENDA"
                GAIA2.bdr(objConn, "SELECT * FROM METLAGD WITH(NOLOCK) WHERE AGDINNOD=" & rel.infil, DS)
                For Each dbRow In DS.Tables(0).Rows

                    GAIA2.bdSR(objConn, "INSERT INTO METLAGD (AGDINNOD, AGDINIDI, AGDDSTIT, AGDDSSUB, AGDDSRES, AGDDSDES, AGDDTINI,  AGDDSINS, AGDDTPUB, AGDDTCAD, AGDDSUSR, AGDDSCOM, AGDDTTIM, AGDDSEQP, AGDDSORG, AGDDSOBS, AGDIMPRE, AGDINENT,  AGDDSDUM, AGDDSDUH, AGDDTFIN, AGDDTINS, AGDDTINF, AGDSWVIS, AGDDSHOR,AGDCDJOV, AGDSWVAL, AGDSWSUS, AGDDSLNK, AGDDSVID) VALUES (" & codiNode & "," & dbRow("AGDINIDI") & ",'" & dbRow("AGDDSTIT") & "','" & dbRow("AGDDSSUB").replace("'", "''") & "','" & dbRow("AGDDSRES").replace("'", "''") & "','" & dbRow("AGDDSDES").replace("'", "''") & "','" & dbRow("AGDDTINI") & "','" & dbRow("AGDDSINS").replace("'", "''") & "','" & dbRow("AGDDTPUB") & "','" & dbRow("AGDDTCAD") & "','" & dbRow("AGDDSUSR") & "','" & dbRow("AGDDSCOM").replace("'", "''") & "','" & dbRow("AGDDTTIM") & "','" & dbRow("AGDDSEQP").replace("'", "''") & "','" & dbRow("AGDDSORG").replace("'", "''") & "','" & dbRow("AGDDSOBS").replace("'", "''") & "'," & IIf(IsDBNull(dbRow("AGDIMPRE")), 0, dbRow("AGDIMPRE")) & "," & IIf(IsDBNull(dbRow("AGDINENT")), 1, dbRow("AGDINENT")) & ",'" & dbRow("AGDDSDUM") & "','" & dbRow("AGDDSDUH") & "','" & dbRow("AGDDTFIN") & "','" & dbRow("AGDDTINS") & "','" & dbRow("AGDDTINF") & "'," & dbRow("AGDSWVIS") & ",'" & dbRow("AGDDSHOR") & "'," & dbRow("AGDCDJOV") & "," & dbRow("AGDSWVAL") & "," & dbRow("AGDSWSUS") & ",'" & dbRow("AGDDSLNK").replace("'", "''") & "','" & dbRow("AGDDSVID") & "')")

                Next dbRow

                novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)
                'copio METLREI
                GAIA2.bdr(objConn, "SELECT * FROM METLREI  WITH(NOLOCK) WHERE REIINCOD=" & rel.incod, DS)
                For Each dbRow In DS.Tables(0).Rows
                    Try

                        GAIA2.bdSR(objConn, "INSERT INTO METLREI VALUES (" & novaRel.incod & "," & dbRow("REIINIDI") & ",'" & dbRow("REIDTPUB") & "','" & dbRow("REIDTCAD") & "','" & dbRow("REIDSFIT") & "','" & dbRow("REIDSFI2") & "','" & dbRow("REIDSHAS") & "')")

                    Catch
                        'no faig res si ja existia
                    End Try

                Next dbRow






            Case "FULLA INFO"
                GAIA2.bdr(objConn, "SELECT * FROM METLINF  WITH(NOLOCK) WHERE INFINNOD=" & rel.infil, DS)
                For Each dbRow In DS.Tables(0).Rows
                    GAIA2.bdSR(objConn, "insert into METLINF (INFINNOD, INFINIDI, INFDSTIT, INFDSSUB, INFDSAVN, INFDSTXT, INFDSRES,  INFDTPUB, INFDTCAD, INFDTANY, INFDSUSR,INFDSLNK, INFDSVID, INFCDTIP, INFWNBAU, INFDSFON, INFDSPCL, INFDTPBK, INFDSPBK, INFWNREV, INFWNOAC, INFWN010)  VALUES (" & codiNode & "," & dbRow("INFINIDI") & ",'" & dbRow("INFDSTIT").replace("'", "''") & "','" & dbRow("INFDSSUB").replace("'", "''") & "','" & dbRow("INFDSAVN").replace("'", "''") & "','" & dbRow("INFDSTXT").replace("'", "''") & "','" & dbRow("INFDSRES").replace("'", "''") & "','" & dbRow("INFDTPUB") & "','" & dbRow("INFDTCAD") & "','" & dbRow("INFDTANY") & "','" & dbRow("INFDSUSR") & "','" & dbRow("INFDSLNK") & "','" & dbRow("INFDSVID") & "'," & dbRow("INFCDTIP") & "," & dbRow("INFWNBAU") & ",'" & dbRow("INFDSFON") & "','" & dbRow("INFDSPCL") & "','" & dbRow("INFDTPBK") & "','" & dbRow("INFDSPBK") & "'," & dbRow("INFWNREV") & ",'" & dbRow("INFWNOAC") & "','" & dbRow("INFWN010") & "')")
                Next dbRow
                novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)
                'copio METLREI
                GAIA2.bdr(objConn, "SELECT * FROM METLREI  WITH(NOLOCK) WHERE REIINCOD=" & rel.incod, DS)
                For Each dbRow In DS.Tables(0).Rows
                    Try
                        GAIA2.bdSR(objConn, "INSERT INTO METLREI VALUES (" & novaRel.incod & "," & dbRow("REIINIDI") & ",'" & dbRow("REIDTPUB") & "','" & dbRow("REIDTCAD") & "','" & dbRow("REIDSFIT") & "','" & dbRow("REIDSFI2") & "','" & dbRow("REIDSHAS") & "')")
                    Catch
                        'no faig res si ja existia
                    End Try
                Next dbRow
                'copio els equipaments relacionats
                GAIA2.bdr(objConn, "SELECT * FROM METLEQP WITH(NOLOCK) WHERE EQPINACT=" & rel.infil, DS)
                For Each dbRow In DS.Tables(0).Rows
                    GAIA2.bdSR(objConn, "INSERT INTO METLEQP VALUES (" & codiNode & "," & dbRow("EQPINDIR") & ")")
                Next dbRow
                'COPIO IMATGES I DOCUMENTS
                '    GAIA.bdR(objConn, "SELECT * FROM METLREL WHERE RELCDRSU=" & rel.incod & " AND RELCDSIT IN (96,97) ", DS)
                '   For Each dbRow In DS.Tables(0).Rows
                '
                'Next dbRow

            Case Else
                'és un node que no té taula associada. Creant el node i la relació és suficient
                novaRel = GAIA2.creaRelacio(objConn, reldesti.cdarb, reldesti.infil, codiNode, 0, reldesti.cdher & "_" & reldesti.infil, rel.cdest, rel.cdsit, rel.cdord + 1000, 1, False, codiUsuari)
        End Select

        'Si no és una imatge faig copia dels continguts que pengin:
        If Not UCase(rel.tipdsdes) = "FULLA DOCUMENT" Then
            Dim reltmp As New clsRelacio
            GAIA2.bdr(objConn, "SELECT * FROM METLREL  WITH(NOLOCK) WHERE RELCDRSU=" & rel.incod & " AND RELCDSIT <99 ", DS)
            For Each dbRow In DS.Tables(0).Rows
                reltmp.bdget(objConn, dbRow("RELINCOD"))
                ferCopiaContingut(objConn, reltmp, codiUsuari, "-1", novaRel)
            Next dbRow
        End If
        ferCopiaContingut = novaRel.incod
        DS.Dispose()
    End Function 'ferCopiaContingut






    Public Shared Function FormataCadena(ByVal Text As Object) As String
        If IsDBNull(Text) Then
            Return "''"
        Else
            If Text.Trim = "" Then
                Return "''"
            Else
                Return "'" & Text.Trim.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "'"
            End If
        End If
    End Function

    Public Shared Function FormataInteger(ByVal Text As Object) As String
        If IsDBNull(Text) Then
            Return "''"
        Else
            Return Text
        End If
    End Function

    Public Shared Function FormataData(ByVal Text As String) As String
        If Text.Trim = "" Then
            Return "1/1/1900"
        Else
            Return "'" & Text.Trim.Replace("'", "''") & "'"
        End If
    End Function

    Public Shared Function FormataSwitch(ByVal Valor As Boolean) As String
        If Valor Then
            Return "'1'"
        Else
            Return "'0'"
        End If

    End Function

    Public Shared Function formataSwitchbd(ByVal Valor As Object) As String
        Valor = Trim(Valor)
        If IsDBNull(Valor) Then
            Return "'0'"
        Else
            If Valor = "1" Then
                Return "'1'"
            Else
                Return "'0'"
            End If
        End If
    End Function

    Public Shared Function TractaNullData(ByVal Valor As Object) As String
        If Convert.IsDBNull(Valor) Then
            Return ""
        Else
            If Valor.ToString() = "" Then
                Return ""
            Else
                If CDate(Valor) = CDate("1/1/1900") Then
                    Return ""
                Else
                    Return CType(Valor, DateTime).Date
                End If

            End If
        End If
    End Function

    Public Shared Function TractaNullHora(ByVal Valor As Object) As String
        If Convert.IsDBNull(Valor) Or Valor.ToString() = "" Then
            Return ""
        Else
            Return CType(Valor.ToString().Replace(".", ":"), DateTime).TimeOfDay.ToString
        End If
    End Function

    Public Shared Function TractaNullCadena(ByVal Valor As Object) As String
        If Convert.IsDBNull(Valor) Then
            Return ""
        Else
            Return CStr(Valor)
        End If
    End Function


    Public Shared Function TractaNullInteger(ByVal Valor As Object) As String
        If Convert.IsDBNull(Valor) Then
            Return "0"
        Else
            If (Valor.ToString() & "") = "" Then
                Return "0"
            Else
                Return Valor.ToString()
            End If
        End If
    End Function


    '******************************************************************
    '	Funció: corregirErrorsEstructura
    '	Entrada:
    '			codiRelacio: Codi de la relació en la que RELINFIL apunta al node 
    '									del que comprobarem els errors	
    '
    '			Probo d'assignar la cel·la automàticament per a tots els fills que penjen de codiRelacio.
    '			Si no hi ha dubte ho faig (posicioEstructura>=0)
    '******************************************************************


    Public Shared Sub corregirErrorsEstructura(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal idioma As Integer, ByVal posicioEstructura As Integer)
        Dim nodesFills As Integer() = {}
        Dim relacions As Integer() = {}
        Dim nroFills, item, tipusNode As Integer
        Dim posicioEstructuraNova As Integer

        Dim posicioAL As Integer
        Dim sql As String
        Dim codiRElacio As Integer
        Dim relSuperior As New clsRelacio
        Dim reltmp As New clsRelacio
        Dim DS As DataSet


        DS = New DataSet()
        codiRElacio = rel.incod
        sql = ""

        GAIA2.trobaFills(objConn, rel, rel.infil, nodesFills, nroFills, relacions, 0, 0)
        For Each item In relacions
            If item <> 0 Then
                reltmp.bdget(objConn, item)

                posicioEstructuraNova = GAIA2.assignaAutomaticamentCella(objConn, reltmp.tipintip, rel, 0, idioma, posicioAL, posicioEstructura)

                If posicioEstructuraNova >= 0 Then
                    sql = " RELCDEST=" + posicioEstructuraNova.ToString()
                End If
                If posicioAL >= 0 Then
                    If sql <> "" Then
                        sql += ","
                    End If
                    sql += " RELCDEAL=" + posicioAL.ToString()
                End If
                If sql <> "" Then
                    GAIA2.bdSR(objConn, "UPDATE METLREL SET " + sql + " WHERE RELINCOD=" + item.ToString())

                End If
            End If
        Next item

        'Faig el mateix per a codiRelacio
        sql = ""
        tipusNode = rel.tipintip

        relSuperior.bdget(objConn, rel.cdrsu)

        posicioEstructuraNova = GAIA2.assignaAutomaticamentCella(objConn, tipusNode, relSuperior, 0, idioma, posicioAL, posicioEstructura)
        If posicioEstructuraNova >= 0 Then
            sql = " RELCDEST=" + posicioEstructuraNova.ToString()
        End If
        If posicioAL >= 0 Then
            If sql <> "" Then
                sql += ","
            End If
            sql = " RELCDEAL=" + posicioAL.ToString()
        End If
        If sql <> "" Then
            GAIA2.bdSR(objConn, "UPDATE METLREL SET " + sql + " WHERE RELINCOD=" + codiRElacio.ToString())
        End If

        DS.Dispose()
    End Sub 'corregirErrorsEstructura



    '******************************************************************
    '	Funció: canviOrdre
    '	Entrada:
    '			rel: relació en la que RELINFIL apunta al node que volem canviar d'ordre
    '			relDesti: relació que tindrà ordre superior a rel
    '			moureFills: Si TRUE llavors farem el tractament de moure els fills.
    '			insertarAlFinal: Si True insertaré al final de la llista de nodes que penjen de RELINPAR apuntat per codiRelacio.
    '			tipusOrdre:		0: al final de la llista
    '										1: al inici de la llista
    '										2: segons la posició seleccionada (a continuació de "reldestiInicial"
    '										3: ordre alfabètic
    '			relDestiInicial: En cas de moure al mateix nivell, relDestiInicial apunta al node desti on hem mogut el node
    '										Només ho utilitzaré en cas de tipusOrdre=2
    '											
    '
    '******************************************************************
    Public Shared Sub canviOrdre(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal relDesti As clsRelacio, ByVal moureFills As Integer, ByVal tipusOrdre As Integer, ByVal relDestiInicial As clsRelacio)

        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim codiRelacio, codiRelacioDesti As Integer
        codiRelacio = rel.incod
        codiRelacioDesti = relDesti.incod
        Dim cont, ordreCanviat As Integer
        ordreCanviat = 0
        cont = 1
        If tipusOrdre = 3 Then
            GAIA2.bdr(objConn, "SELECT * FROM METLREL  WITH(NOLOCK) ,METLNOD WITH(NOLOCK)  WHERE RELCDHER LIKE '" + relDesti.cdher + "_" + relDesti.infil.ToString() + "' AND RELINFIL=NODINNOD AND RELCDSIT < 98  ORDER BY NODDSTXT ", DS)
        Else
            GAIA2.bdr(objConn, "SELECT * FROM METLREL WITH(NOLOCK)  WHERE RELCDHER LIKE '" + relDesti.cdher + "_" + relDesti.infil.ToString() + "' AND RELCDSIT < 98  ORDER BY RELCDORD", DS)
        End If

        For Each dbRow In DS.Tables(0).Rows
            Select Case tipusOrdre
                Case 1 'poso rel el primer de la llista
                    If dbRow("RELINCOD") = rel.incod Then

                        'no faig res
                    Else
                        If cont = 1 Then
                            GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDORD=1 WHERE RELINCOD=" + rel.incod.ToString())
                            GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDORD=2 WHERE RELINCOD=" + dbRow("RELINCOD").ToString())
                            cont = 3
                        Else
                            GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDORD=" + cont.ToString() + " WHERE RELINCOD=" + dbRow("RELINCOD").ToString())
                            cont = cont + 1
                        End If
                    End If
                Case 3 'ordre alfabètic
                    GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDORD=" + cont.ToString() + " WHERE RELINCOD=" + dbRow("RELINCOD").ToString())
                    cont = cont + 1
                Case 2 'possició seleccionada
                    If dbRow("RELINCOD") = rel.incod Then
                        'no faig res
                    Else
                        If dbRow("RELINCOD") = relDestiInicial.incod Then
                            'tinc el desti, el poso i a continuació poso també el node que estic movent			
                            GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDORD=" + cont.ToString() + " WHERE RELINCOD=" + dbRow("RELINCOD").ToString())
                            cont = cont + 1
                            GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDORD=" + cont.ToString() + " WHERE RELINCOD=" + rel.incod.ToString())
                            cont = cont + 1
                        Else
                            GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDORD=" + cont.ToString() + " WHERE RELINCOD=" + dbRow("RELINCOD").ToString())
                            cont = cont + 1
                        End If

                    End If
                Case 0 'últim de la llista
                    If dbRow("RELINCOD") = rel.incod Then
                        'no faig res, el posaré al final
                        'cont=cont+1
                    Else
                        GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDORD=" + cont.ToString() + " WHERE RELINCOD=" + dbRow("RELINCOD").ToString())
                        cont = cont + 1
                    End If
            End Select
        Next
        If tipusOrdre = 0 Then
            GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDORD=" + cont.ToString() + " WHERE RELINCOD=" + rel.incod.ToString())
        End If
        DS.Dispose()
    End Sub 'canviOrdre



    '**********************************************************************************************************************
    '	Funció: Crea Relació
    '	Entrada: 
    '		pare		: node pare d'on volem penjar el node
    '		fill	  : node fill
    '		posicioEstructura: Posició dins d'una pàgina web on es representarà el node.
    '		situacioRelacio: Estat del fill apuntat per RELINFIL.
    '											Posibles estats: 1: Inicial, 2: Pendent de publicar(publicable), 3: publicat, 4: caducar.....
    '		Ordre: posició en la que volem crear la relació
    '	Procés:
    '		Crea tantes relacions com instàncies del node pare. Es a dir, cerco totes les relacions que tenen al node pare i els hi penjo el fill.
    '**********************************************************************************************************************
    Public Shared Function creaRelacioPerNode(ByVal objConn As OleDbConnection, ByVal pare As Integer, ByVal fill As Integer, ByVal posicioEstructura As Integer, ByVal situacioRelacio As Integer, ByVal ordre As Integer, ByVal codiusuari As Integer) As Integer
        Dim cont As Integer = 0

        Dim DS As DataSet
        Dim dbRow As DataRow

        DS = New DataSet()
        Dim herencia As String

        GAIA2.bdr(objConn, "SELECT RELCDHER,RELCDARB,RELINPAR,RELINFIL 	FROM METLREL  WITH(NOLOCK) WHERE RELINFIL=" + pare.ToString() + " AND RELCDSIT<98", DS)
        For Each dbRow In DS.Tables(0).Rows
            herencia = dbRow("RELCDHER") + "_" + dbRow("RELINFIL").ToString()

            GAIA2.creaRelacio(objConn, dbRow("RELCDARB"), pare, fill, 0, herencia, posicioEstructura, situacioRelacio, ordre, 1, False, codiusuari)
            cont += 1
        Next dbRow
        DS.Dispose()
        Return (cont)
    End Function 'creaRelacioPerNode

    '******************************************************************
    '	Funció: Crea Relació
    '	Entrada: 
    '		arbre 	: arbre de relacions
    '		pare		: node pare
    '		fill	  : node fill
    '		versió : versió de l'arbre. En cas d'una relació fulla/fulla el valor=0
    '		herènciaPare: camí desde el node "pare" fins l'arrel. null si node "pare" és l'arrel
    '		posicioEstructura: Posició dins d'una pàgina web on es representarà el node.
    '		situacioRelacio: Estat del fill apuntat per RELINFIL.
    '		Posibles estats: 1: Inicial, 2: Pendent de publicar(publicable), 3: publicat, 4: caducar
    '		Ordre: posició en la que volem crear la relació
    '	Sortida:	 0 Si ok , >0 si error
    '	Procés: 	
    '		Crea una relació pare/fill
    '		Si la relació ja existia no la crea.
    '		Finalment assigna els permisos heretats pel node a la seva nova ubicació (codiArbre)
    '******************************************************************

    Public Shared Function creaRelacio(ByVal objConn As OleDbConnection, ByVal arbre As Integer, ByVal pare As Integer, ByVal fill As Integer, ByVal versio As Integer, ByVal herencia As String, ByVal posicioEstructura As Integer, ByVal situacioRelacio As Integer) As clsRelacio
        Return creaRelacio(objConn, arbre, pare, fill, versio, herencia, posicioEstructura, situacioRelacio, -1, 1, False, 0)
    End Function



    Public Shared Function creaRelacio(ByVal objConn As OleDbConnection, ByVal arbre As Integer, ByVal pare As Integer, ByVal fill As Integer, ByVal versio As Integer, ByVal herencia As String, ByVal posicioEstructura As Integer, ByVal situacioRelacio As Integer, ByVal ordre As Integer) As clsRelacio
        Return creaRelacio(objConn, arbre, pare, fill, versio, herencia, posicioEstructura, situacioRelacio, ordre, 1, False, 0)
    End Function

    Public Shared Function creaRelacio(ByVal objConn As OleDbConnection, ByVal arbre As Integer, ByVal pare As Integer, ByVal fill As Integer, ByVal versio As Integer, ByVal herencia As String, ByVal posicioEstructura As Integer, ByVal situacioRelacio As Integer, ByVal ordre As Integer, ByVal contingutEsVisibleAInternet As Integer) As clsRelacio
        Return creaRelacio(objConn, arbre, pare, fill, versio, herencia, posicioEstructura, situacioRelacio, ordre, contingutEsVisibleAInternet, False, 0)
    End Function

    Public Shared Function creaRelacio(ByVal objConn As OleDbConnection, ByVal arbre As Integer, ByVal pare As Integer, ByVal fill As Integer, ByVal versio As Integer, ByVal herencia As String, ByVal posicioEstructura As Integer, ByVal situacioRelacio As Integer, ByVal ordre As Integer, ByVal contingutEsVisibleAInternet As Integer, ByVal tractarPermisosEnDiferit As Boolean) As clsRelacio
        Return creaRelacio(objConn, arbre, pare, fill, versio, herencia, posicioEstructura, situacioRelacio, ordre, contingutEsVisibleAInternet, tractarPermisosEnDiferit, 0)
    End Function

    Public Shared Function creaRelacio(ByVal objConn As OleDbConnection, ByVal arbre As Integer, ByVal pare As Integer, ByVal fill As Integer, ByVal versio As Integer, ByVal herencia As String, ByVal posicioEstructura As Integer, ByVal situacioRelacio As Integer, ByVal ordre As Integer, ByVal contingutEsVisibleAInternet As Integer, ByVal tractarPermisosEnDiferit As Boolean, ByVal usuari As Integer) As clsRelacio
        Dim herenciapare As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim codiRElacio As Integer
        Dim rel As New clsRelacio
        Dim relSuperior As New clsRelacio
        Dim sqlUpdate As String = ""

        'La visibilitat d'un document a dins d'un contingut es controla des del propi contingut 	
        If situacioRelacio = 96 Or situacioRelacio = 97 Or situacioRelacio = 95 Then
            contingutEsVisibleAInternet = 1
            posicioEstructura = -1
        Else
            contingutEsVisibleAInternet = contingutVisibleAInternet(objConn, fill)
        End If


        GAIA2.bdr(objConn, "SELECT RELINCOD, RELSWVIS FROM METLREL  WITH(NOLOCK)  WHERE  RELCDVER =" & versio & " AND RELINPAR=" & pare & " AND RELINFIL=" & fill & " AND RELCDHER LIKE '" & herencia & "%'  AND RELCDSIT<99 ORDER BY RELINCOD DESC", DS)

        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            ' La relació ja existia, actualitzo la propietat de "visible a internet" i retorno rel.incod=0
            rel.bdget(objConn, dbRow("RELINCOD"))
            relSuperior = GAIA2.obtenirRelacioSuperior(objConn, rel)

            If contingutEsVisibleAInternet = 0 Then
                If rel.swvis = 1 Then
                    sqlUpdate = "RELSWVIS=0"
                End If
            Else
                If relSuperior.swvis <> rel.swvis Then
                    sqlUpdate = "RELSWVIS=" & relSuperior.swvis
                End If
            End If
            'Actualitzo l'ordre
            If ordre <> -1 Then
                If sqlUpdate.Length > 0 Then
                    sqlUpdate += " , "
                End If
                sqlUpdate = "RELCDORD=" & ordre
            End If

            If sqlUpdate.Length > 0 Then
                GAIA2.bdSR(objConn, "UPDATE METLREL SET " & sqlUpdate & " WHERE RELINCOD=" & rel.incod)
            End If


            rel.incod = 0
        Else ' no existeix, vaig a buscar l'ordre
            If ordre = -1 Then
                'Busco l'ultim node de la llista
                GAIA2.bdr(objConn, "SELECT TOP 1 RELCDORD, RELINCOD FROM METLREL  WITH(NOLOCK) WHERE RELCDHER like '" & herencia & "' AND RELCDVER=" & versio & " AND RELINPAR=" & pare & " AND RELCDSIT<99  ORDER BY RELCDORD DESC", DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    ordre = dbRow("RELCDORD") + 1
                Else
                    ordre = 1

                End If
            End If

            If InStr(herencia, "_") > 0 Then
                herenciapare = herencia.Substring(0, InStrRev(herencia, "_") - 1)
            Else
                herenciapare = herencia
            End If


            GAIA2.bdr(objConn, "SELECT RELSWVIS FROM METLREL WITH(NOLOCK)  WHERE RELCDHER LIKE '" & herenciapare & "' AND RELINFIL=" & pare, DS)

            If DS.Tables(0).Rows.Count > 0 Then
                contingutEsVisibleAInternet = DS.Tables(0).Rows(0)("RELSWVIS")
            End If

            'Inserto
            codiRElacio = GAIA2.bdSR_id(objConn, "INSERT INTO METLREL (RELCDARB, RELCDVER,RELINPAR, RELINFIL, RELCDHER,RELCDEST,RELCDORD,RELCDSIT, RELSWVIS) VALUES (" + arbre.ToString() + "," + versio.ToString() + "," + pare.ToString() + "," + fill.ToString() + ",'" + herencia.ToString() + "'," + posicioEstructura.ToString() & "," & ordre & "," & situacioRelacio & "," & contingutEsVisibleAInternet & ")")

            'Faig l'herència del valor RELSWVIS
            rel.bdget(objConn, codiRElacio)

            relSuperior = GAIA2.obtenirRelacioSuperior(objConn, rel)
            rel.cdrsu = relSuperior.incod
            'Actualitzo el codi de relació pare i hereto la propietat de visible a internet

            'GAIA.bdSR(Objconn, "UPDATE METLREL SET RELSWVIS=" & relSuperior.swvis & ", RELCDRSU=" & relSuperior.incod & " WHERE RELINCOD=" & rel.incod)

            If contingutEsVisibleAInternet = 0 Then
                'RELSWVIS=0, ja ho he posat per defecte
                'només actualitzo RELCDRSU
                GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDRSU=" & relSuperior.incod & " WHERE RELINCOD=" & rel.incod)
            Else
                If relSuperior.swvis = 1 Then
                    GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDRSU=" & relSuperior.incod & ", RELSWVIS=" & relSuperior.swvis & " WHERE RELINCOD=" & rel.incod)
                Else
                    GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDRSU=" & relSuperior.incod & " WHERE RELINCOD=" & rel.incod)
                End If
            End If


        End If
        creaRelacio = rel
        If usuari <> 0 Then
            'Assigno permisos a l'usuari
            clsPermisos.gravaPermis2(objConn, usuari, rel, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, False)
        End If

        'Una vegada he creat la relació l'afegiré els permisos que s'hereten des dels nodes superiors
        If herencia = "" Or fill = 0 Then
            'no faig res
        Else
            'clsPermisos.tractaPermisosHeretats(objConn, herencia, fill, "", "", tractarPermisosEnDiferit)						
            clsPermisos.tractaPermisosHeretats2(objConn, rel, rel.infil, "", 1, 0)
        End If
        DS.Dispose()
    End Function 'creaRelacio



    Public Shared Function cercarNodeByTxt(ByVal objConn As OleDbConnection, ByVal radtree1 As RadTreeView, ByVal text As String, ByVal codiArbre As Integer, ByVal idusuari As Integer, ByVal nroArbre As Integer) As String
        Return cercarNodeByTxt(objConn, radtree1, text, codiArbre, idusuari, nroArbre, "")
    End Function

    Public Shared Function cercarNodeByTxt(ByVal objConn As OleDbConnection, ByVal radtree1 As RadTreeView, ByVal text As String, ByVal codiArbre As Integer, ByVal idusuari As Integer, ByVal nroArbre As Integer, ByVal herencia As String) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim nodos(0) As Integer
        Dim rels(0) As Integer
        Dim cont As Integer
        Dim nodo As Integer
        Dim rel As Integer
        Dim nodosPerObrir(0) As String
        Dim nodeArrel As Integer
        Dim treenode As RadTreeNode
        Dim i As Integer
        Dim item As String
        Dim strSql As String = ""
        Dim trobat As Integer = 0


        Dim cadenaCerca As String()
        cadenaCerca = Split(text, " ")
        For Each item In cadenaCerca
            If strSql.Length > 0 Then
                strSql += " AND	 "
            End If
            strSql += " UPPER(NODDSTXT) LIKE '%" + UCase(item.Replace("'", "''")) + "%'"
        Next item
        If strSql = "" Then
            strSql += " UPPER(NODDSTXT) LIKE '%" + UCase(text.Replace("'", "''")) + "%'"
        End If
        'minimitzo l'arbre
        If herencia <> "" Then
            If strSql.Length > 0 Then
                strSql += " AND	 "
            End If
            strSql += " RELCDHER LIKE '" + herencia + "%'"
        End If
        'RadTree1.collapseAllNodes()
        For Each treenode In radtree1.SelectedNodes
            treenode.Selected = False
        Next treenode
        'busquem el número de node del node arrel
        nodeArrel = Mid(radtree1.Nodes.Item(0).Value, 1, InStr(radtree1.Nodes.Item(0).Value, "-") - 1)
        'Busco els nodes que pertanyen a la cerca
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT  distinct RELINCOD,NODINNOD FROM METLREL WITH(NOLOCK) , METLNOD WITH(NOLOCK)  WHERE RELCDHER LIKE '%_" & nodeArrel & "%' AND (" + strSql.ToString() + ") AND NODINNOD = RELINFIL AND RELCDSIT<98 " + " AND RELCDSIT <>" + ctESBORRATCADUCAT.ToString() + " ORDER BY NODINNOD,RELINCOD", DS)

        cont = 0
        For Each dbRow In DS.Tables(0).Rows
            ReDim Preserve nodos(cont)
            ReDim Preserve rels(cont)
            rels(cont) = dbRow("RELINCOD")
            nodos(cont) = dbRow("NODINNOD")
            cont += 1
        Next dbRow


        cercarNodeByTxt = ""

        For Each rel In rels
            'Busco tots els pares

            GAIA2.buscaParesRel(objConn, rel, nodosPerObrir)
        Next rel

        'per a tots els trobats faig un recorregut cap amunt seguint les relacions de l'arbre
        ' amb aquests nodes i en ordre invers al recorregut faig un GAIA.afegeixNodesFillsPantalla
        ' finalment selecciono els que he cercat	

        For i = 0 To (nodosPerObrir.Length - 1)
            'Cadascun d'aquests nodes s'ha d'obrir/expandir si no ho estava ja (i en aquest ordre)				
            For Each treenode In GAIA2.buscaNodeArbrePantalla(objConn, radtree1, nodosPerObrir(i))
                If (Not treenode Is Nothing) Then
                    If treenode.Nodes.Count = 0 Then

                        GAIA2.afegeixNodesFillsPantallaLlista(objConn, nodosPerObrir(i), treenode, GAIA2.llegirCodiArbrePantalla(objConn, treenode), idusuari, nroArbre, 0, 0, 1)
                    End If
                    treenode.Expanded = True


                End If
            Next treenode
        Next

        For Each nodo In nodos
            For Each treenode In GAIA2.buscaNodeArbrePantalla(objConn, radtree1, nodo)
                If treenode Is Nothing Then
                Else
                    treenode.Selected = True
                    trobat = 1
                End If
            Next treenode
        Next nodo
        If trobat = 0 Then
            cercarNodeByTxt = "No s'ha trobat cap coincidència"
        End If
        DS.Dispose()
    End Function


    '******************************************************************
    '	Funció: cercarNodesPantallaByCodi
    '	Entrada: 

    '		radtree1: arbre de la pantalla on volem cercar
    '		codi		: codis dels nodes que volem seleccionar. Format: codi1, codi2, codi3,... ,codiN
    '		idUsuari			: Persona que vol fer la cerca 
    '		codiArbre: codi de l'arbre on buscarem (NO S'UTILITZA!, paràmetre a eliminar)
    '		nroArbre: 1 si arbre de l'esquerra, 2 si arbre de la dreta
    '		trobaRelacio: si 1, llavors "codis" és una llista de relacions, sino és una llista de codis.
    '	Procés: 	
    '		Cerca tots els codis en l'arbre seleccionat i obre l'arbre de la pantalla 
    '		seleccionant els nodes trobats.
    '****************************************************************** 	 	
    Public Shared Sub cercarNodesPantallaByCodi(ByVal objConn As OleDbConnection, ByVal radtree1 As RadTreeView, ByVal codis As String, ByVal codiArbre As Integer, ByVal idUsuari As Integer, ByVal nroArbre As Integer, ByVal trobaRelacio As Integer)
        Dim DS As DataSet

        Dim dbRow As DataRow

        Dim nodos(0) As Integer
        Dim rels(0) As Integer
        Dim cont As Integer
        Dim nodo As Integer
        Dim nodeArrel As Integer
        Dim rel As Integer
        Dim nodosPerObrir(0) As String
        Dim treenode As RadTreeNode
        Dim i As Integer

        codis = codis.Replace("|", ",")
        ' hi ha algun codi de forma "xxxx|,xxxx), i amb això faig neteja per que no falli el sql
        codis = codis.Replace(",,", ",")
        'minimitzo l'arbre
        radtree1.CollapseAllNodes()
        For Each treenode In radtree1.SelectedNodes
            treenode.Selected = False
        Next treenode

        'Busco els nodes que pertanyen a la cerca
        DS = New DataSet()

        nodeArrel = Mid(radtree1.Nodes.Item(0).Value, 1, InStr(radtree1.Nodes.Item(0).Value, "-") - 1)

        If trobaRelacio = 1 Then
            GAIA2.bdr(objConn, " SELECT  distinct RELINCOD , NODINNOD FROM METLREL WITH(NOLOCK) , METLNOD WITH(NOLOCK)  WHERE RELCDHER LIKE '%_" & nodeArrel & "%'  AND RELINCOD IN (" & codis.ToString() & ") AND  (NODINNOD = RELINFIL) AND RELINPAR<>RELINFIL   AND RELCDSIT<98 " + " AND RELCDSIT<>" + ctESBORRATCADUCAT.ToString(), DS)
        Else
            GAIA2.bdr(objConn, " SELECT  distinct RELINCOD , NODINNOD FROM METLREL WITH(NOLOCK) , METLNOD WITH(NOLOCK)  WHERE RELCDHER LIKE '%_" & nodeArrel & "%' AND NODINNOD IN (" & codis.ToString() & ") AND  (NODINNOD = RELINFIL) AND RELINPAR<>RELINFIL  AND RELCDSIT<98 " + " AND RELCDSIT<>" + ctESBORRATCADUCAT.ToString(), DS)
        End If

        cont = 0
        For Each dbRow In DS.Tables(0).Rows
            ReDim Preserve nodos(cont)
            ReDim Preserve rels(cont)
            nodos(cont) = dbRow("NODINNOD")
            rels(cont) = dbRow("RELINCOD")
            cont += 1
        Next dbRow

        For Each rel In rels
            GAIA2.buscaParesRel(objConn, rel, nodosPerObrir)
        Next rel


        For i = 0 To (nodosPerObrir.Length - 1)
            'Cadascun d'aquests nodes s'ha d'obrir/expandir si no ho estava ja (i en aquest ordre)
            For Each treenode In GAIA2.buscaNodeArbrePantalla(objConn, radtree1, nodosPerObrir(i))
                If (Not treenode Is Nothing) Then
                    If treenode.Nodes.Count = 0 Then
                        GAIA2.afegeixNodesFillsPantallaLlista(objConn, nodosPerObrir(i), treenode, GAIA2.llegirCodiArbrePantalla(objConn, treenode), idUsuari, nroArbre, 0, 0, 1)
                    End If
                    treenode.Expanded = True
                End If

            Next treenode
        Next

        If radtree1.CheckBoxes = True Then
            For Each nodo In nodos
                For Each treenode In GAIA2.buscaNodeArbrePantalla(objConn, radtree1, nodo)
                    If treenode Is Nothing Then
                    Else
                        treenode.Checked = True
                    End If
                Next treenode
            Next nodo
        Else
            For Each nodo In nodos
                For Each treenode In GAIA2.buscaNodeArbrePantalla(objConn, radtree1, nodo)
                    If treenode Is Nothing Then
                    Else
                        treenode.Selected = True
                    End If
                Next treenode
            Next nodo
        End If
        DS.Dispose()
    End Sub 'cercarNodesPantallaByCodi



    '******************************************************************
    '	Funció: Inserta Node Arbre Personal
    '	Entrada: 
    '		tipusFulla 	: tipus de node que volem insertar
    '		codiFulla	: codi del node que volem insertar
    '		codiOrg	 : Persona que crea el node 
    '		nomCarpetaBasica: Nom de la carpeta en la que volem insertar el node. 
    '												per defecte: "sense classificar"
    '												possibles valors:"Missatges","sense classificar"
    '	Sortida:
    '		clsRelacio: retorna l'objecte relació que apunta al node insertat.
    '	Procés: 	
    '		Inserta el node de l'arbre personal que de moment anomeno "el meu espai"
    '		Ho penja del node  amb nom:   tipusfulla de nif,  on tipusFulla és el nom del tipus de fulla,
    '		per exemple: documents, noticies, etc. 
    ' 	Així l'usuari tindrà el que acaba de crear per publicar-ho on vulgui.
    '		Si no existeixen aquests nodes d'on ho penjem, els creo.
    '****************************************************************** 

    Public Shared Function insertaNodeArbrePersonal(ByVal objConn As OleDbConnection, ByVal tipusFulla As Integer, ByRef codiFulla As Integer, ByVal codiOrg As String, ByVal nomCarpetaBasica As String) As clsRelacio
        'Creo la relació inicial entre l'arbre de "el meu espai" i un node de tipus "elMeuEspai" per a la persona. Si ja existeix no la creo.

        '
        Dim codiArbre As Integer = GAIA2.tipusNodeByTxt(objConn, "arbre elMeuEspai")
        Dim tipusNodeElMeuEspai As Integer = GAIA2.tipusNodeByTxt(objConn, "node elMeuEspai")
        Dim codiNodePare As Integer = 0
        Dim resultat As Integer = 0
        Dim codiNodeElMeuEspai As Integer = 0
        Dim codiNodeSenseClassificar As Integer = 0
        'Dim DS As DataSet
        Dim nomUsuari As String
        Dim codiRelacio As Integer
        Dim nodeFullaOrg As Integer
        Dim codiNodeDesti As Integer
        Dim rel As New clsRelacio
        Dim relArrel As New clsRelacio
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        nomUsuari = GAIA2.obtenirNomNode(objConn, codiOrg)
        nodeFullaOrg = codiNodeByTxt(objConn, "fulla organigrama", codiOrg.ToString())
        '********************************************************************
        ' Cerco el node del "elmeuespai" de l'usuari que crea el node
        '********************************************************************
        codiNodeElMeuEspai = codiNodeElMeuEspaiByTxtPropietari(objConn, "El meu espai de ", codiOrg)
        codiNodePare = codiNodeByTxt(objConn, "arbre elMeuEspai", "arbre elMeuEspai")

        If codiNodeElMeuEspai = 0 Then        'no he trobat el node i el creo				
            codiNodeElMeuEspai = insertarNode(objConn, tipusNodeElMeuEspai, "El meu espai de " + nomUsuari, codiOrg)
            relArrel = creaRelacio(objConn, codiArbre, codiNodePare, codiNodeElMeuEspai, 0, "_" + codiNodePare.ToString(), -1, 1, -1, 1, False, codiOrg)

            codiRelacio = relArrel.incod
            GAIA2.bdSR(objConn, "INSERT INTO METLNES (NESINNOD,NESINIDI,NESDSTIT, NESDSUSR,NESCDORG) VALUES (" + codiNodeElMeuEspai.ToString() + ",1, 'El meu espai de " + nomUsuari + "',0," + codiOrg.ToString() + ")")
            ' Gravo el permis manual, amb arbre=0 perque és manual
            'clsPermisos.gravaPermis(objConn, 3, codiOrg, 0, 0, rel)
            'clsPermisos.gravaPermis(objConn, 10, 115969, 0, 0, rel)


            clsPermisos.gravaPermis2(objConn, codiOrg, relArrel, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, False)

            clsPermisos.gravaPermis2(objConn, 115969, relArrel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, False)

            clsPermisos.gravaPermis2(objConn, 49730, relArrel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, False)
            '*************************************************************************
            ' Poso l'usuari en l'arbre d'usuaris, a la carpeta de sense classificar
            '*************************************************************************	
            creaRelacio(objConn, 11, 151203, codiFulla, 0, "_56261_115969_151203", -1, 1, -1, 1, False, codiOrg)
        End If
        ' tinc el node de "elmeuespai" de la persona (codiNode)  i a l'arbre "elMeuEspai" (nodePare)			

        '*************************************************************************			
        'Cerco el node "sense classificar" que ha de penjar de codiNodeElMeuEspai
        '*************************************************************************			

        codiNodeSenseClassificar = codiNodeElMeuEspaiByTxtPropietari(objConn, "Sense classificar", codiOrg)

        ' Si no existeix el node "sense classificar" el creo
        If codiNodeSenseClassificar = 0 Then

            codiNodeSenseClassificar = insertarNode(objConn, tipusNodeElMeuEspai, "Sense classificar", codiOrg)
            rel = creaRelacio(objConn, codiArbre, codiNodeElMeuEspai, codiNodeSenseClassificar, 0, "_" + codiNodePare.ToString() + "_" + codiNodeElMeuEspai.ToString(), -1, 1, -1, 1, False, codiOrg)
            codiRelacio = rel.incod
            'clsPermisos.gravaPermis(objConn, 3, codiOrg, 0, 0, rel)
            'clsPermisos.gravaPermis(objConn, 10, 115969, 0, 0, rel)


            clsPermisos.gravaPermis2(objConn, codiOrg, rel, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, False)
            clsPermisos.gravaPermis2(objConn, 115969, rel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, False)


            GAIA2.bdSR(objConn, "INSERT INTO METLNES (NESINNOD,NESINIDI,NESDSTIT, NESDSUSR,NESCDORG) VALUES (" + codiNodeSenseClassificar.ToString() + ",1,'Sense classificar','0'," + codiOrg.ToString() + ")")
        End If
        If nomCarpetaBasica = "" Or nomCarpetaBasica = "Sense classificar" Then
            codiNodeDesti = codiNodeSenseClassificar
        End If


        '*************************************************************************
        ' Afegeixo el contingut a l'arbre personal
        '*************************************************************************						
        If codiFulla <> 0 And tipusFulla <> 13 Then 'una fulla organigrama no la posaré al sense classificar del creador
            'Relaciono la fulla que volen insertar amb el node "sense classificar"
            rel = creaRelacio(objConn, codiArbre, codiNodeDesti, codiFulla, 0, "_" + codiNodePare.ToString() + "_" + codiNodeElMeuEspai.ToString() + "_" + codiNodeDesti.ToString(), -1, 1, -1, 1, False, codiOrg)
            codiRelacio = rel.incod
            insertaNodeArbrePersonal = rel
            'si és un tràmit només dono permís d'editor, a l'usuari i als grups d'organigrama als que pertany
            If tipusFulla = 51 Then
                'clsPermisos.gravaPermis(objConn, 3, codiOrg, 0, 0, rel)
                clsPermisos.gravaPermis2(objConn, codiOrg, rel, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, False)
                clsPermisos.gravaPermis2(objConn, 115969, rel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, False)
                clsPermisos.gravaPermis2(objConn, 49730, relArrel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, False)

                'el grup genéric "tràmits"=178621 no l'agafo. 
                GAIA2.bdr(objConn, " SELECT RELINPAR as pareOrg FROM METLREL WITH(NOLOCK)  WHERE RELCDHER LIKE '_56261%' AND RELINPAR<>178621 AND RELINFIL=" & codiOrg, DS)
                For Each dbRow In DS.Tables(0).Rows
                    'clsPermisos.gravaPermis(objConn, 3, dbRow("pareOrg"), 0, 0, rel)
                    clsPermisos.gravaPermis2(objConn, dbRow("pareOrg"), rel, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, False)

                Next dbRow
            Else
                'clsPermisos.gravaPermis(objConn, 1, codiOrg, 0, 0, rel)

                clsPermisos.gravaPermis2(objConn, codiOrg, rel, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, False)

            End If
            GAIA2.log(objConn, rel, codiOrg, "", TAINSERTAR)
        Else
            GAIA2.log(objConn, rel, codiOrg, "Creació de l'usuari: " + nomUsuari, TAINSERTAR)
        End If
        insertaNodeArbrePersonal = rel


        clsPermisos.gravaPermis2(objConn, 115969, relArrel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, False)
        clsPermisos.gravaPermis2(objConn, 49730, relArrel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, False)


        DS.Dispose()
    End Function    'InsertaNodeArbrePersonal



    Public Shared Function formatejaUsuari(ByVal usuari As String) As String
        If usuari.IndexOf("\") > 0 Then
            usuari = usuari.Substring(usuari.IndexOf("\") + 1)
        End If
        If usuari.IndexOf("/") > 0 Then
            usuari = usuari.Substring(usuari.IndexOf("/") + 1)
        End If
        Return usuari
    End Function



    'obté les dades del 'usuari. Si no existeix a GAIA es crea mitjançant dades que es trobin a la bd de personal
    Public Shared Sub UsuariGaia(ByVal objconn As OleDbConnection, ByVal usuari As String, ByRef nif As String, ByRef codiOrg As String, ByRef nom As String, Optional ByVal crearUsuari As Boolean = False)

        Dim DS As DataSet
        Dim dbRow As DataRow
        nif = ""
        codiOrg = ""
        nom = ""

        usuari = formatejaUsuari(usuari)
        If usuari = "adminbackup" Then
            usuari = "msoria"
        End If

        DS = New DataSet()
        GAIA2.bdr(objconn, "SELECT FORDSNIF, FORDSTIT, FORINNOD FROM METLFOR WITH(NOLOCK) ,METLREL WITH(NOLOCK)  WHERE FORDSWIN like '" & usuari & "' AND FORINIDI=1 AND RELCDSIT<98 AND FORINNOD=RELINFIL", DS)

        'si trobo l'usuari a GAIA
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            nif = dbRow("FORDSNIF")
            codiOrg = dbRow("FORINNOD")
            nom = dbRow("FORDSTIT")
        End If
        DS.Dispose()
        'No he trobat l'usuari a GAIA, el creo a l'arbre organigrama usuarisGAIA, dintre del node organigrama "nous, sense classificar" i retorno les dades
        If nom = "" And crearUsuari Then
            Dim cn As OleDbConnection
            Dim cmd As OleDbCommand
            Dim dr As OleDbDataReader

            Try

                cn = New OleDbConnection("Provider=IBMDADB2;DSN=DBPERS;UID=PE;PWD=4t./sR2C")
                cn.Open()
                'cal seleccionar només el primer --> top 1 no em serveix i FETCH FIRST 1 ROW ONLY no funciona??
                cmd = New OleDbCommand("SELECT   PERINPER, PERCDNIF, PERDSNOM, PERDSCO1, PERDSCO2 FROM PE.PETLPER   WHERE PERDSUSU like '" & usuari & "' ORDER BY PERINPER DESC WITH UR   ", cn)
                dr = cmd.ExecuteReader()
                While dr.Read
                    nif = dr("PERCDNIF")
                    nom = dr("PERDSNOM").trim() & " " & dr("PERDSCO1").trim() & " " & dr("PERDSCO2").trim()
                End While

                dr.Close()
                cn.Close()
                'He trobat a la persona, la afegeixo a GAIA i retorno també el número de organigrama								
                If nom <> "" Then
                    Dim tipusNode As Integer
                    'Creo la fulla organigrama						
                    tipusNode = GAIA2.tipusNodeByTxt(objconn, "fulla organigrama")
                    'Inserto el node "fulla organigrama " amd codiUsuari=9999 per que és un procés automàtic
                    Dim codinode As Integer = GAIA2.insertarNode(objconn, tipusNode, nom, 9999)
                    codiOrg = codinode
                    'Inserto el node fulla organigrama en l'arbre personal de l'usuari	
                    GAIA2.insertaNodeArbrePersonal(objconn, tipusNode, codinode, codinode, "")


                    'Creo la fulla  "fulla organigrama"
                    GAIA2.bdSR(objconn, "INSERT INTO METLFOR (FORINNOD,FORINIDI,FORDSTIT, FORDSUSR,FORDSCOD, FORDSNIF, FORDSWIN) VALUES (" & codinode & ",1,'" & nom.Replace("''", "'") & "','9999','0','" & nif & "','" & usuari & "')")

                    'oUtil.enviaMail("webAjuntament@l-h.cat", "msoria@l-h.cat", "", "","GAIA. Creació automàtica d'usuaris",  "S'ha creat l'usuari: " & usuari & " automàticament des d'un formulari GAIA. Cal classificar-ho dins de l'arbre d'usuaris de GAIA.", Nothing)
                End If
            Catch ex As Exception

                f_logError(objconn, "G01", "GAIA. Creació automàtica d'usuaris -- " & ex.Source, "Ha fallat el procés de creació automàtica d'un usuari de GAIA. -- " & ex.Message)
            End Try
        End If
    End Sub  'UsuariGaia



    Public Shared Function nifUsuari(ByVal objConn As OleDbConnection, ByVal usuari As String, Optional ByRef usuariActiu As Boolean = False) As String
        nifUsuari = ""
        usuari = formatejaUsuari(usuari)
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        If usuari = "adminbackup" Then
            usuari = "msoria"
        End If
        If usuari = "" Then
            nifUsuari = "99999999"
        Else
            GAIA2.bdr(objConn, "SELECT top 1 FORDSNIF, RELCDSIT FROM METLFOR  WITH(NOLOCK), METLREL WITH(NOLOCK)  WHERE RELCDSIT<98 AND RELINFIL=FORINNOD AND FORDSWIN like '" + usuari + "' AND FORINIDI=1 ORDER BY RELCDSIT ASC", DS)
            If DS.Tables(0).Rows.Count > 0 Then
                dbRow = DS.Tables(0).Rows(0)
                nifUsuari = dbRow("FORDSNIF")
                usuariActiu = True
            End If
        End If
        DS.Dispose()
    End Function        'NIF Usuari

    Public Shared Function nomUsuari(ByVal objConn As OleDbConnection, ByVal nif As String) As String

        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        nomUsuari = "99999999"

        GAIA2.bdr(objConn, "SELECT FORDSTIT FROM METLFOR WITH(NOLOCK)  WHERE FORDSNIF like '" + nif.Trim() + "' AND FORINIDI=1", DS)

        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            nomUsuari = dbRow("FORDSTIT")
        End If

        DS.Dispose()

    End Function        'NIF Usuari

    '******************************************************************
    '	Funció: trobaNodeUsuari
    '	Entrada: 
    '		strNif: Nif de l'usuari
    '	Procés: 	
    '		Cerca dins de la taula de fulles organigrama (METLNES) el valor de FORINNOD apuntat per FORDSNIF
    '	Sortida:
    '		Codi del node
    '******************************************************************
    Public Shared Function trobaNodeUsuari(ByVal objConn As OleDbConnection, ByVal strNif As String) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        trobaNodeUsuari = 0
        GAIA2.bdr(objConn, "SELECT FORINNOD FROM METLFOR WITH(NOLOCK)  WHERE FORDSNIF like '" + strNif + "' AND FORINIDI=1", DS)

        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            trobaNodeUsuari = dbRow("FORINNOD")
        End If
        DS.Dispose()
    End Function 'trobaNodeUsuari


    '******************************************************************
    '	Funció: trobaNomFitxerPublicat
    '	Entrada: 
    '		codi relacio
    '	Procés: 	
    '		Cerca dins de la taula de relacions (METLREL) el camp RELDSFIT

    '******************************************************************
    Public Shared Function trobaNomFitxerPublicat(ByVal objConn As OleDbConnection, ByVal codirelacio As Integer) As String
        Dim DS As DataSet
        Dim dbRow As DataRow

        DS = New DataSet()
        trobaNomFitxerPublicat = ""
        Dim pos As Integer
        GAIA2.bdr(objConn, "SELECT RELDSFIT FROM METLREL  WITH(NOLOCK) WHERE RELINCOD=" + codirelacio.ToString() + " AND NOT (RELDSFIT IS NULL)", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)

            pos = 0
            If (InStr(dbRow("RELDSFIT"), "GAIA/") > 0) Then
                trobaNomFitxerPublicat = dbRow("RELDSFIT").Substring(5)
            Else
                If (InStr(dbRow("RELDSFIT"), "/GAIA/") > 0) Then
                    trobaNomFitxerPublicat = dbRow("RELDSFIT").Substring(6)
                Else
                    If (InStr(dbRow("RELDSFIT"), "GAIA06/") > 0) Then
                        trobaNomFitxerPublicat = dbRow("RELDSFIT").Substring(7)
                    Else
                        trobaNomFitxerPublicat = dbRow("RELDSFIT")
                    End If
                End If
            End If
        End If
        DS.Dispose()
    End Function 'trobaNomFitxerPublicat


    Public Shared Function trobaNomFitxerPublicatAmbIdioma(ByVal objConn As OleDbConnection, ByVal codirelacio As Integer, ByVal codiIdioma As Integer) As String
        Dim DS As DataSet
        Dim dbRow As DataRow

        DS = New DataSet()
        trobaNomFitxerPublicatAmbIdioma = ""
        Dim pos As Integer
        GAIA2.bdr(objConn, "SELECT REIDSFIT FROM METLREI WITH(NOLOCK)  WHERE REIINCOD=" + codirelacio.ToString() + " AND REIINIDI=" + codiIdioma.ToString(), DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)

            pos = 0
            If (InStr(dbRow("REIDSFIT"), "GAIA/") > 0) Then
                trobaNomFitxerPublicatAmbIdioma = dbRow("REIDSFIT").Substring(5)
            Else
                If (InStr(dbRow("REIDSFIT"), "/GAIA/") > 0) Then
                    trobaNomFitxerPublicatAmbIdioma = dbRow("REIDSFIT").Substring(6)
                Else
                    If (InStr(dbRow("REIDSFIT"), "GAIA06/") > 0) Then
                        trobaNomFitxerPublicatAmbIdioma = dbRow("REIDSFIT").Substring(7)
                    Else
                        trobaNomFitxerPublicatAmbIdioma = dbRow("REIDSFIT")
                    End If
                End If
            End If
        End If
        DS.Dispose()
    End Function 'trobaNomFitxerPublicatAmbIdioma


    '******************************************************************
    '	Funció: trobaTipusDocument
    '	Entrada: 
    '		strTipusMime: Tipus mime del document
    '	Procés: 	
    '		Cerca dins de la taula de tipus de documents (METLTDO) el tipus de node. 
    '		Si no troba cap tipus igual crea un.
    '	Sortida:
    '		Codi del tipus de document
    '******************************************************************
    Public Shared Function trobaTipusDocument(ByVal objConn As OleDbConnection, ByVal strTipusMime As String) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        trobaTipusDocument = ""

        GAIA2.bdr(objConn, "SELECT  TDOCDTDO FROM METLTDO WITH(NOLOCK)  WHERE TDODSNOM like '" & strTipusMime & "'", DS)
        If DS.Tables(0).Rows.Count = 0 Then
            GAIA2.bdSR(objConn, "INSERT INTO METLTDO (TDODSNOM, TDODSIMG) VALUES ('" & strTipusMime & "','ico_document.png')")
            trobaTipusDocument = GAIA2.trobaTipusDocument(objConn, strTipusMime)
        Else
            dbRow = DS.Tables(0).Rows(0)
            trobaTipusDocument = dbRow("TDOCDTDO")
        End If

        DS.Dispose()
    End Function


    Public Shared Function trobaTipusDocumentPerExtensioFitxer(ByVal objConn As OleDbConnection, ByVal extensio As String) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        trobaTipusDocumentPerExtensioFitxer = 1
        GAIA2.bdr(objConn, "SELECT  TDOCDTDO FROM METLTDO WITH(NOLOCK)  WHERE TDODSEXT like '%" & LCase(extensio) & "%'", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            trobaTipusDocumentPerExtensioFitxer = dbRow("TDOCDTDO")
        End If
        DS.Dispose()
    End Function





    '************************************************************************************************************
    '	Funció: llistaNodes
    '	Entrada: 
    '			codiArbre = codi del arbre a mostrar
    '			nroNode = node de 
    '			txt = texte per afegir a la descripció
    '	Sortida:  <option>nodes</option>
    ' 	Procés: Funció recursiva que escriu una llista dels nodes d'un arbre en un select de html
    '************************************************************************************************************

    Public Shared Function llistaNodes(ByVal objConn As OleDbConnection, ByVal codiArbre As Integer, ByVal nroNode As Integer, ByVal txt As String) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        llistaNodes = ""
        GAIA2.bdr(objConn, "SELECT * FROM METLREL WITH(NOLOCK) , METLNOD WITH(NOLOCK) , METLTIP  WITH(NOLOCK) WHERE RELCDARB =" & codiArbre & " AND RELINPAR = " & nroNode & " AND RELINFIL=NODINNOD AND TIPDSDES LIKE 'node%' AND TIPINTIP=NODCDTIP AND RELINPAR<>RELINFIL AND RELCDSIT<98 ORDER BY RELINFIL ", DS)
        For Each dbRow In DS.Tables(0).Rows
            llistaNodes = llistaNodes & "<option value=" & dbRow("RELINFIL") & ">" & txt.ToString() & dbRow("NODDSTXT").ToString() & "</option>" & llistaNodes(objConn, codiArbre, dbRow("RELINFIL"), "&nbsp;&nbsp;" & txt.ToString())
        Next dbRow
        DS.Dispose()
    End Function

    '************************************************************************************************************
    '	Funció: llistaTipus
    '	Entrada: 
    '			strTipus = tipus a llistar (per exemple : 'fulla'
    '	Sortida:  <option>nodes</option>
    ' 	Procés: Funció que escriu una llista dels tipus de nodes que continguin la cadena "strTipus" 
    '************************************************************************************************************

    Public Shared Function llistaTipus(ByVal objConn As OleDbConnection, ByVal strTipus As String) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        llistaTipus = ""
        GAIA2.bdr(objConn, "SELECT TIPINTIP, TIPDSDES FROM  METLTIP  WITH(NOLOCK) WHERE TIPDSDES LIKE '%" + strTipus + "%' AND TIPINTIP<>2 ORDER BY TIPDSDES", DS)
        For Each dbRow In DS.Tables(0).Rows
            llistaTipus += "<option value=" + dbRow("TIPINTIP").ToString().Trim() + ">" + dbRow("TIPDSDES").ToString().Trim() + "</option>"
        Next dbRow
        DS.Dispose()
    End Function 'llistaTipus


    '************************************************************************************************************
    '	Funció: llistaIdiomes
    '	Entrada: 
    '	Sortida:  <option>nodes</option>
    ' 	Procés: Escriu una llista dels idiomes disponibles
    '************************************************************************************************************

    Public Shared Function llistaIdiomes(ByVal objConn As OleDbConnection, ByVal codiIdioma As Integer) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        llistaIdiomes = ""
        GAIA2.bdr(objConn, "SELECT * FROM METLIDI WITH(NOLOCK)  ORDER BY IDICDIDI asc", DS)
        For Each dbRow In DS.Tables(0).Rows
            llistaIdiomes = llistaIdiomes + "<option value=" + dbRow("IDICDIDI").ToString()
            If codiIdioma.ToString() = dbRow("IDICDIDI") Then
                llistaIdiomes = llistaIdiomes & " selected "
            End If
            llistaIdiomes = llistaIdiomes & ">" & dbRow("IDIDSNOM").Trim() & "</option>"
        Next dbRow
        DS.Dispose()

    End Function



    '************************************************************************************************************

    '	Funció: carregaDirectori
    '	Entrada: 
    '	Sortida:  <option>nodes</option>
    ' 	Procés: recull de la base de dades RECULLNOU la informació  sobre directori i la carrega a GAIA.
    '						Si una de les entrades de directori ja existeix, o bé no té els mateixos pares o fills la funció 
    '						avisa del fet.
    '************************************************************************************************************
    Public Shared Sub carregaDirectori()


        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()

        Dim MyConnection As SqlConnection
        Dim MyCommand As SqlDataAdapter
        MyConnection = New SqlConnection("server=recullnou;Database=SILH;Integrated Security=false; pwd=secopc; user id=cpdsa")
        MyCommand = New SqlDataAdapter("SELECT CODIGO, DESCAMPCAT FROM SICODIFICACION WHERE CODIGO like '03300000'", MyConnection)
        MyCommand.Fill(DS)

        MyConnection.Close()
        MyCommand.Dispose()

        For Each dbRow In DS.Tables(0).Rows
        Next dbRow
        DS.Dispose()
    End Sub

    '************************************************************************************************************
    '	Funció: escriuResultat
    '	Entrada:  lbl: etiqueta on representaré el resultat
    '						text: Text del resultat

    '						enllac: link



    '	Procés: Escriu el resultat (text) alhora d'afegir un enllac (link)
    '************************************************************************************************************
    Public Shared Sub escriuResultat(ByVal objConn As OleDbConnection, ByRef lbl As Label, ByVal text As String, ByVal enllaç As String)
        GAIA2.escriuResultat(objConn, lbl, text, enllaç, 1)

    End Sub

    Public Shared Sub escriuResultat(ByVal objConn As OleDbConnection, ByRef lbl As Label, ByVal text As String, ByVal enllaç As String, ByVal esborraResultatAnterior As Integer)


        If esborraResultatAnterior = 1 Then
            lbl.Text = ""
        End If
        lbl.Text &= "<div class="""



        If GAIA2.llistatWarnings.Length > 0 Then
            lbl.Text &= "missatgeAvis"
        Else
            If GAIA2.llistatErrors.Length > 0 Then
                lbl.Text &= "missatgeError"
            Else
                lbl.Text &= "missatgeOk"
            End If
        End If
        lbl.Text &= """><span class=""topGris""><span></span></span><div class=""missatgeCont gtxt""><div class="""
        If GAIA2.llistatErrors.Length = 0 Then
            lbl.Text &= "ok"
        Else
            lbl.Text &= "error"
        End If
        lbl.Text &= "ok"" style=""text-align:left"">" & text

        If enllaç.Length > 0 Then
            lbl.Text = lbl.Text & "<br/><br/>&nbsp;" & enllaç
        End If
        lbl.Text &= "</div></div><span class=""bottomGris""><span></span></span></div>"



    End Sub




    'Calcula si un contingut que té el camp xxSWVIS té el valor 1 o 0. Si no existeix el camp (cas de contractes, per exemple) retorna 1.
    Public Shared Function contingutVisibleAInternet(ByVal objConn As OleDbConnection, ByVal codiNode As Integer) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim descTipus As String = ""
        Dim tipus As String = ""
        DS = New DataSet()
        'comprovo si el contingut és visible a internet
        contingutVisibleAInternet = 0

        tipus = tipusNodebyNro(objConn, codiNode, descTipus)
        If tipus = 3 Or tipus = 45 Or tipus = 51 Then
            Select Case tipus
                Case 3
                    GAIA2.bdr(objConn, "select DIRSWVIS as VISIBLE FROM METLNOD  WITH(NOLOCK) ,METLDIR  WITH(NOLOCK) where NODINNOD=" & codiNode & " and DIRINNOD=NODINNOD AND DIRINIDI=1 ", DS)

                Case 45
                    GAIA2.bdr(objConn, "select  AGDSWVIS as VISIBLE FROM METLNOD  WITH(NOLOCK) ,METLAGD  WITH(NOLOCK) where NODINNOD=" & codiNode & " and AGDINNOD=NODINNOD AND AGDINIDI=1", DS)
                Case 51

                    GAIA2.bdr(objConn, "select FTRSWVIS as VISIBLE FROM METLNOD  WITH(NOLOCK) ,METLFTR  WITH(NOLOCK) where NODINNOD=" & codiNode & "  and FTRINNOD=NODINNOD AND FTRINIDI=1 ", DS)
            End Select

            If DS.Tables(0).Rows.Count > 0 Then
                dbRow = DS.Tables(0).Rows(0)
                contingutVisibleAInternet = dbRow("visible")

            End If
        Else
            contingutVisibleAInternet = 1
        End If
        DS.Dispose()
    End Function 'contingutVisibleAInternet




    '***************************************************************************************************

    '	Funció: modificaRelacio
    '	Entrada:  nodepath1: path inicial
    '						nodePath2: nou path
    '						codiArbre1: arbre
    '						codiArbre2: nou arbre per a la relació
    '						nodePare1: node pare anterior
    '					  nodePare2: nou node pare
    '						nodeFill1: node fill anterior
    '						nodeFill2: nou node fill
    '						posicioEstructura: posició que ocupará el node en l'estructura definida pel pare (en el cas
    '																d'una pàgina web serà la cel·la on s'ubicarà el contingut)

    '						relacioOrigen: Codi de la relacio que apunta a la posició original del node. Aquest codi es mantindrà a la nova 
    '														ubicació del node
    '						estatRelacio: Estat en que es vol posar la relació (el contingut apuntat per relinfil).
    '													Si = -1 llavors no la modifiquem
    '	Procés: Funció que fa una modificació d'una relació determinada per (codiarbreOrigen, nodepare1, nodefill1, nodepath1)
    '					Si codiArbreOrigen=0 llavors faré la modificació a totes les relacions de tots els arbres.
    '					Si posicioEstructura=-1 no modifico la cel·la on s'ubica el contingut.
    '					Si relacioOrigen=0 llavors no modificaré la relació a la que apuntan la resta de camps, només ho faré pels seus
    '															fills. Això ho utilitzaré per esborrar el node i moure els seus fills.
    '***************************************************************************************************



    Public Shared Sub modificaRelacio(ByVal objConn As OleDbConnection, ByRef relOrigen As clsRelacio, ByVal nodePathNou As String, ByVal codiArbreNou As Integer, ByVal nodePareNou As Integer, ByVal nodeFillNou As Integer, ByVal posicioEstructuraNova As Integer, ByVal estatRelacioNova As Integer, ByVal moureFills As Integer)
        Dim DS As DataSet
        Dim nodePath As String
        Dim dbRow As DataRow
        Dim posicio As Integer = 0
        Dim relDesti As New clsRelacio
        Dim est As Integer
        DS = New DataSet()
        Dim sql As String
        Dim noupare As String
        Dim relSuperior As New clsRelacio
        Dim visible As Integer = 0
        Dim contingutEsVisibleAInternet As Integer
        If relOrigen.cdher <> nodePathNou Or Not moureFills Then

            If moureFills = 1 Then
                ' Primer canvio les relacions Filles
                sql = "SELECT * FROM METLREL WITH(NOLOCK)   WHERE RELCDSIT<98  AND RELCDHER LIKE '" & relOrigen.cdher & "_" & relOrigen.infil & "%'"
                GAIA2.bdr(objConn, sql, DS)
                For Each dbRow In DS.Tables(0).Rows

                    If dbRow("RELCDSIT") = 97 Or dbRow("RELCDSIT") = 96 Or dbRow("RELCDSIT") = 95 Then
                        contingutEsVisibleAInternet = 1
                    Else
                        contingutEsVisibleAInternet = contingutVisibleAInternet(objConn, dbRow("RELINFIL")) And relDesti.swvis
                    End If
                    nodePath = dbRow("RELCDHER").Replace(relOrigen.cdher, nodePathNou)
                    sql = " RELCDHER='" + nodePath + "' "
                    relDesti.bdget(objConn, dbRow("RELINCOD"))
                    GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDARB=" & codiArbreNou & ",RELCDHER='" & nodePath & "',RELSWVIS=" & contingutEsVisibleAInternet & " WHERE RELINCOD=" & dbRow("RELINCOD"))
                    If relDesti.cdsit < 95 Then
                        GAIA2.afegeixAccioManteniment(objConn, relDesti, 0, 99, Now, Now, relDesti, 1, posicioEstructuraNova, True)


                    End If


                Next dbRow
            Else
                'Les relacions filles pujen un nivell a relcdher i canvia el pare
                sql = "SELECT * FROM METLREL  WITH(NOLOCK)  WHERE RELCDHER LIKE '" + relOrigen.cdher + "_" + relOrigen.infil.ToString() + "%'"
                GAIA2.bdr(objConn, sql, DS)

                For Each dbRow In DS.Tables(0).Rows

                    nodePath = dbRow("RELCDHER").Replace("_" + relOrigen.infil.ToString(), "")
                    sql = " RELCDHER='" + nodePath + "' "
                    If dbRow("RELINPAR") = relOrigen.infil Then
                        noupare = relOrigen.inpar
                    Else
                        noupare = dbRow("RELINPAR")
                    End If
                    If dbRow("RELCDSIT") = 97 Or dbRow("RELCDSIT") = 96 Or dbRow("RELCDSIT") = 95 Then
                        contingutEsVisibleAInternet = 1


                    Else
                        contingutEsVisibleAInternet = contingutVisibleAInternet(objConn, dbRow("RELINFIL")) And relOrigen.swvis
                    End If

                    GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDHER='" & nodePath & "', RELINPAR=" & noupare & ", RELCDRSU = " & relOrigen.incod & ", RELSWVIS=" & contingutEsVisibleAInternet & " WHERE RELINCOD=" & dbRow("RELINCOD"))
                    relDesti.bdget(objConn, dbRow("RELINCOD"))
                    If Not (dbRow("RELCDSIT") = 96 Or dbRow("RELCDSIT") = 97 Or dbRow("RELCDSIT") = 95) Then
                        est = plantillaPerDefecte(objConn, relDesti, 1)
                        relSuperior = GAIA2.obtenirRelacioSuperior(objConn, relDesti)
                        est = assignaAutomaticamentCella(objConn, relDesti.tipintip, relSuperior, 1, 1, 0, 0)
                        If est <> relDesti.cdest Then
                            GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDEST=" + est.ToString() + " WHERE RELINCOD=" + relDesti.incod.ToString())
                            relDesti.bdget(objConn, relDesti.incod)
                        End If
                    Else

                        GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDEST=-1 WHERE RELINCOD=" + relDesti.incod.ToString())
                    End If
                    If relDesti.cdsit < 95 Then
                        GAIA2.afegeixAccioManteniment(objConn, relDesti, 0, 99, "", "", relDesti, 0, posicioEstructuraNova)
                    End If
                Next dbRow

            End If
            'Ara faig el canvi per a relOrigen	
            sql = " RELCDHER='" + nodePathNou + "' "
            If relOrigen.incod = 0 Then
                sql = sql + ", RELCDORD=" + relOrigen.cdord.ToString() + " "
            End If
            If codiArbreNou > 0 Then
                sql = sql + ", RELCDARB=" + codiArbreNou.ToString() + " "
            End If
            If nodePareNou > 0 Then
                sql = sql + ", RELINPAR=" + nodePareNou.ToString() + " "
            End If
            If nodeFillNou > 0 Then
                sql = sql + ", RELINFIL=" + nodeFillNou.ToString() + " "
            End If
            If estatRelacioNova <> -1 Then
                If (relOrigen.cdsit = 96 Or relOrigen.cdsit = 97 Or relOrigen.cdsit = 95) Then
                    If estatRelacioNova = 98 Or estatRelacioNova = 99 Then
                        estatRelacioNova = (estatRelacioNova * 100) + relOrigen.cdsit 'poso els tipus 9996, 9997, 9896, 9897
                    Else
                        estatRelacioNova = relOrigen.cdsit
                    End If
                End If

                sql = sql + ", RELCDSIT=" + estatRelacioNova.ToString() + " "
            End If

            relSuperior = GAIA2.obtenirRelacio(objConn, nodePareNou, nodePathNou.Substring(0, InStrRev(nodePathNou, "_" & nodePareNou) - 1))
            'IF relSuperior.tipintip=35 THEN	
            '	contingutEsVisibleAInternet=true
            'ELSE
            contingutEsVisibleAInternet = contingutVisibleAInternet(objConn, relOrigen.infil) And relSuperior.swvis
            'END IF

            sql = sql + ", RELSWVIS=" & contingutEsVisibleAInternet
            sql = sql & ", RELCDRSU = " & relSuperior.incod

            If relOrigen.cdsit = 98 Then
                estatRelacioNova = 98
            End If

            bdSR(objConn, " UPDATE METLREL SET " & sql.ToString() & " WHERE RELINCOD=" & relOrigen.incod)
            If relOrigen.cdsit < 95 Then

                GAIA2.afegeixAccioManteniment(objConn, relOrigen, 0, 99, Now, Now, relOrigen, 1, posicioEstructuraNova, True)
                If relOrigen.cdrsu <> relSuperior.incod Then
                    GAIA2.afegeixAccioManteniment(objConn, relSuperior, 0, 99, Now, Now, relSuperior, 1, posicioEstructuraNova, True)
                End If

            End If
        End If

        If estatRelacioNova = 98 Or estatRelacioNova = 99 Then
            GAIA2.esborrarCelles(Nothing, "CELCDNOD=" & relDesti.infil)
            'gaia.bdsr(objconn, "DELETE FROM METLCEL WHERE CELCDNOD=" & relDesti.infil)
        End If
        'he de tractar els permisos heretats a la relació de destí, que és on s'han arrossegat els continguts.
        'clsPermisos.tractaPermisosHeretats(objConn, "", relDesti.infil, "", "", 1)
        clsPermisos.tractaPermisosHeretats2(objConn, relOrigen, relOrigen.infil, "", 1, 1)

        DS.Dispose()
    End Sub 'modificaRelacio

    '************************************************************************************************************
    '	Funció: GAIA.llegirCodiArbrePantalla
    '	Entrada:  node: RadTreeNode
    '						
    '	Procés: Recull el valor de l'arbre. Si no hi ha informació del path serà desde "-" fins el final. Si no, és
    '				 	des de "-" fins el primer "_"
    '************************************************************************************************************

    Public Shared Function llegirCodiArbrePantalla(ByVal objConn As OleDbConnection, ByVal node As RadTreeNode) As String

        If node.Value.IndexOf("_") >= 0 Then
            llegirCodiArbrePantalla = node.Value.Substring(node.Value.IndexOf("-") + 1, node.Value.IndexOf("_") - 1 - node.Value.IndexOf("-"))
        Else
            llegirCodiArbrePantalla = node.Value.Substring(node.Value.IndexOf("-") + 1)
        End If

    End Function 'llegirCodiArbrePantalla


    '***************************************************************************************************
    '	Funció: GAIA.obtenirRelacioPantalla
    '	Entrada:  node: RadTreeNode
    '						
    '	Procés: Recull el valor de la relació del el node seleccionat a la pantalla
    '***************************************************************************************************
    Public Shared Function obtenirRelacioPantalla(ByVal node As RadTreeNode) As String
        obtenirRelacioPantalla = ""
        'IF not (node is nothing) THEN
        If node.Value.IndexOf("_") > 0 Then
            obtenirRelacioPantalla = node.Value.Substring(node.Value.IndexOf("_") + 1)
        End If
        'END IF
    End Function         'obtenirRelacioPantalla		



    '***************************************************************************************************
    '	Funció: GAIA.obtenirPathRelacioPantalla
    '	Entrada:  node: RadTreeNode
    '						
    '	Procés: Recull el valor de la relació on el node seleccionat a la pantalla fa de fill i cerca el path associat.
    '					La informació s'ha de trobar a partir del primer "_"
    '***************************************************************************************************
    Public Shared Function obtenirPathRelacioPantalla(ByVal objConn As OleDbConnection, ByVal node As RadTreeNode) As String
        Dim codiRelacio As Integer
        obtenirPathRelacioPantalla = ""
        If Not (node Is Nothing) Then
            If node.Value.IndexOf("_") > 0 Then
                codiRelacio = node.Value.Substring(node.Value.IndexOf("_") + 1)
                obtenirPathRelacioPantalla = obtenirPathRelacio(objConn, codiRelacio)
            End If
        End If
    End Function         'obtenirPathRelacioPantalla		


    '***************************************************************************************************
    '	Funció: GAIA.obtenirNodePantalla
    '	Entrada:  node: RadTreeNode

    '						
    '	Procés: Recull el valor del node (número abans de "-"
    '***************************************************************************************************
    Public Shared Function obtenirNodePantalla(ByVal objConn As OleDbConnection, ByVal node As RadTreeNode) As String

        obtenirNodePantalla = ""
        If Not (node Is Nothing) Then
            If node.Value.IndexOf("-") > 0 Then
                obtenirNodePantalla = node.Value.Substring(0, node.Value.IndexOf("-"))
            End If
        End If
    End Function         'obtenirPathRelacioPantalla						

    '************************************************************************************************************
    '	Funció: GAIA.obtenirPathRelacio
    '	Entrada:  codiRelacio d'on retornarem el path
    '						
    '	Procés: Obté el path  de la relació
    '************************************************************************************************************
    Public Shared Function obtenirPathRelacio(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer) As String

        Dim DS As DataSet
        Dim dbRow As DataRow
        obtenirPathRelacio = ""

        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT RELCDHER FROM METLREL  WITH(NOLOCK) WHERE RELINCOD=" & codiRelacio.ToString(), DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirPathRelacio = dbRow("RELCDHER") & ""
        End If
        DS.Dispose()

    End Function         'obtenirPathRelacio						

    '************************************************************************************************************
    '	Funció: GAIA.obtenirNomNode
    '	Entrada:  codi del node del que volem obtenir el camp NODDSTXT
    '						
    '	Procés: Obté el path  de la relació

    '************************************************************************************************************
    Public Shared Function obtenirNomNode(ByVal objConn As OleDbConnection, ByVal codiNode As Integer) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        obtenirNomNode = ""
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT NODDSTXT FROM METLNOD  WITH(NOLOCK) WHERE NODINNOD=" & codiNode.ToString(), DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirNomNode = HttpUtility.HtmlDecode(GAIA2.netejaHTML(dbRow("NODDSTXT"))) + ""
        End If
        DS.Dispose()
    End Function         'obtenirNomNode						

    '************************************************************************************************************
    '	Funció: GAIA.obtenirNomPlantilla
    '	Entrada:  codi del node de la plantilla de la que obtindrem el valor PLTDSOBS

    '************************************************************************************************************
    Public Shared Function obtenirNomPlantilla(ByVal objConn As OleDbConnection, ByVal codiNode As Integer) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        obtenirNomPlantilla = ""
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT PLTDSOBS FROM METLPLT  WITH(NOLOCK) WHERE PLTINNOD=" & codiNode.ToString(), DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirNomPlantilla = dbRow("PLTDSOBS") + ""
        End If
        DS.Dispose()
    End Function         'obtenirNomPlantilla				



    '************************************************************************************************************
    '	Funció: GAIA.obtenirRelacioSuperior
    '	Entrada:  codirelacio:  relacio de la que retornarem la relacio superior
    '						
    '	Procés: Obté el la relacio a la que pertany el pare de "codiRelacio"
    '************************************************************************************************************
    Public Shared Function obtenirRelacioSuperior(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio) As clsRelacio

        Dim relTMp As New clsRelacio
        If rel.infil = rel.inpar Then
            relTMp.bdget(objConn, 0)
        Else
            If rel.cdrsu <> 0 Then
                relTMp.bdget(objConn, rel.cdrsu)
                If relTMp.inpar = rel.infil And relTMp.infil = rel.inpar Then
                    Dim objutil As New lhUtil.lhUtil

                    'hi ha una relació creuada.. s'ha de corregir. Envio un mail i retorno rel=0 per que pugui continuar

                    objutil.enviaMail("webintranet@l-h.cat", "msoria@l-h.cat", "", "", "Referència creuada dins d'obtenirRelacioSuperior", rel.incod & "-" & relTMp.incod, Nothing)
                    relTMp.bdget(objConn, 0)
                End If




            Else
                Dim DS As DataSet
                Dim dbRow As DataRow
                DS = New DataSet()
                GAIA2.bdr(objConn, "SELECT relacioSuperior.RELINCOD as relacio FROM METLREL as relacioSuperior  WITH(NOLOCK) INNER JOIN METLREL as relacioInferior  WITH(NOLOCK) ON relacioInferior.RELINCOD=" & rel.incod & " AND relacioInferior.RELCDHER LIKE (relacioSuperior.RELCDHER+'_'+CAST(relacioSuperior.RELINFIL AS VARCHAR))  AND relacioInferior.RELCDSIT<98 WHERE  relacioSuperior.RELCDSIT<98 ", DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    relTMp.bdget(objConn, dbRow("relacio"))

                Else
                    relTMp.bdget(objConn, 0)
                End If
                DS.Dispose()
            End If
        End If
        Return (relTMp)
    End Function 'obtenirRelacioSuperior




    '************************************************************************************************************
    '	Funció: GAIA.obtenirRelacioAL
    '	Entrada:  codirelacio:  relacio on el fill apunta a una fulla web de la que volem obtenir el camp WEBCDRAL

    '************************************************************************************************************
    Public Shared Function obtenirRelacioAL(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio) As clsRelacio
        Dim DS As DataSet
        Dim dbRow As DataRow
        obtenirRelacioAL = New clsRelacio

        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT WEBCDRAL FROM METLWEB WITH(NOLOCK) , METLREL WITH(NOLOCK)  WHERE RELINCOD=" + rel.incod.ToString() + " AND RELINFIL=WEBINNOD", DS)

        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirRelacioAL.bdget(objConn, dbRow("WEBCDRAL"))
        End If
        DS.Dispose()
    End Function 'obtenirRelacioAL



    '************************************************************************************************************
    '	Funció: GAIA.obtenirIconaTipusContingutPerRelacio
    '	Entrada: objecte relació d'on volem trobar la icona
    '						
    '	Procés: 
    '************************************************************************************************************
    Public Shared Function obtenirIconaTipusContingutPerRelacio(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio) As String
        Dim DS As DataSet
        obtenirIconaTipusContingutPerRelacio = ""

        DS = New DataSet()
        If rel.tipdsdes = "fulla document" Then
            GAIA2.bdr(objConn, "select TDODSIMG FROM METLTDO WITH(NOLOCK) ,METLDOC WITH(NOLOCK)  WHERE DOCINTDO=TDOCDTDO and DOCINNOD=" & rel.infil, DS)

            If DS.Tables(0).Rows.Count > 0 Then

                obtenirIconaTipusContingutPerRelacio = DS.Tables(0).Rows(0)(0).Replace(".png", "1.png")
            Else
                obtenirIconaTipusContingutPerRelacio = "ico_document.png"
            End If

        Else
            GAIA2.bdr(objConn, "SELECT TIPDSIMG FROM METLTIP  WITH(NOLOCK) WHERE TIPINTIP=" & rel.tipintip.ToString(), DS)
            If DS.Tables(0).Rows.Count > 0 Then
                obtenirIconaTipusContingutPerRelacio = DS.Tables(0).Rows(0)("TIPDSIMG")
            End If
        End If
        DS.Dispose()
    End Function         'obtenirIconaTipusContingut						


    '************************************************************************************************************
    '	Funció: GAIA.obtenirIconaTipusContingut
    '	Entrada:  tipusContingut as integer
    '						
    '	Procés: Obté la icona del document
    '************************************************************************************************************
    Public Shared Function obtenirIconaTipusContingut(ByVal objConn As OleDbConnection, ByVal tipintip As String) As String

        Dim DS As DataSet
        Dim dbRow As DataRow
        obtenirIconaTipusContingut = ""
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT TIPDSIMG FROM METLTIP  WITH(NOLOCK) WHERE TIPINTIP=" & tipintip, DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirIconaTipusContingut = dbRow("TIPDSIMG") & ""
        End If
        DS.Dispose()
    End Function         'obtenirIconaTipusContingut						


    '************************************************************************************************************
    '	Funció: GAIA.obtenirOrdreRelacio
    '	Entrada:  node: RadTreeNode
    '						
    '	Procés: Obté l'ordre  de la relació
    '***************************************************************************************************
    Public Shared Function obtenirOrdreRelacio(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer) As Integer

        Dim DS As DataSet
        Dim dbRow As DataRow
        obtenirOrdreRelacio = 0
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT RELCDORD FROM METLREL  WITH(NOLOCK) WHERE RELINCOD=" + codiRelacio.ToString() + " AND RELCDSIT<98 ", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirOrdreRelacio = dbRow("RELCDORD")
        End If
        DS.Dispose()
    End Function         'obtenirOrdreRelacio					



    '***************************************************************************************************
    '	Funció: GAIA.obtenirPareRelacio
    '	Entrada:  node: RadTreeNode
    '						
    '	Procés: Obté el pare  de la relació
    '***************************************************************************************************
    Public Shared Function obtenirPareRelacio(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer) As Integer

        Dim DS As DataSet
        Dim dbRow As DataRow
        obtenirPareRelacio = 0
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT RELINPAR FROM METLREL WITH(NOLOCK)  WHERE RELINCOD=" & codiRelacio.ToString() + " AND RELCDSIT<98 ", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirPareRelacio = dbRow("RELINPAR")
        End If
        DS.Dispose()
    End Function         'obtenirPareRelacio						


    '***************************************************************************************************
    '	Funció: GAIA.obtenirRelacio
    '	Entrada:  node: codi del node fill
    '						path: path que identifica el node dins de l'arbre
    '	Procés: A partir d'un node troba la relacio on es troba
    '***************************************************************************************************
    Public Shared Function obtenirRelacio(ByVal objConn As OleDbConnection, ByVal codiNode As Integer, ByVal path As String) As clsRelacio

        Dim DS As DataSet
        Dim dbRow As DataRow

        obtenirRelacio = New clsRelacio

        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT RELINCOD FROM METLREL  WITH(NOLOCK) WHERE RELCDHER like '" + path + "' AND RELINFIL=" + codiNode.ToString() + " AND RELCDSIT<98", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirRelacio.bdget(objConn, dbRow("RELINCOD"))
        End If
        DS.Dispose()
    End Function         'obtenirRelacio				


    '************************************************************************************************************
    '	Funció: GAIA.obtenirFillRelacio
    '	Entrada:  node: RadTreeNode
    '						
    '	Procés: Obté el node fill de la relació
    '************************************************************************************************************
    Public Shared Function obtenirFillRelacio(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer) As Integer

        Dim DS As DataSet
        Dim dbRow As DataRow
        obtenirFillRelacio = 0
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT RELINFIL FROM METLREL  WITH(NOLOCK) WHERE RELINCOD=" + codiRelacio.ToString() + " AND RELCDSIT<98 ", DS)

        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirFillRelacio = dbRow("RELINFIL")
        End If
        DS.Dispose()
    End Function         'obtenirFillRelacio					


    '************************************************************************************************************
    '	Funció: GAIA.obtenirNomTaula
    '	Entrada:  codi de tipus de contingut
    '						
    '	Procés: Obté el nom de la taula
    '************************************************************************************************************
    Public Shared Function obtenirNomTaula(ByVal objConn As OleDbConnection, ByVal codiTCO As Integer) As String

        Dim DS As DataSet
        Dim dbRow As DataRow


        DS = New DataSet()

        obtenirNomTaula = ""
        GAIA2.bdr(objConn, "SELECT TBLDSTXT FROM METLTBL  WITH(NOLOCK) WHERE TBLINTFU =" + codiTCO.ToString(), DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirNomTaula = dbRow("TBLDSTXT").Trim()
        End If
        DS.Dispose()
    End Function         'obtenirFillRelacio				





    '************************************************************************************************************
    '	Funció: GAIA.existeixContingut
    '	Entrada:  codi de node i codiIdioma del contingut que volem comprovar
    '						
    '	Procés: retorna 1 si trobat, 0 si no trobat
    '************************************************************************************************************
    Public Shared Function existeixContingut(ByVal objConn As OleDbConnection, ByVal codiNode As Integer, ByVal codiRelacio As Integer, ByVal codiIdioma As Integer) As Integer

        Dim DS As DataSet
        Dim dbRow As DataRow

        DS = New DataSet()
        existeixContingut = 0

        If codiRelacio <> 0 Then
            GAIA2.bdr(objConn, "SELECT * FROM METLREL WITH(NOLOCK) ,METLTBL WITH(NOLOCK) ,METLNOD WITH(NOLOCK)  WHERE RELINCOD=" + codiRelacio.ToString() + " AND NODINNOD=RELINFIL AND  NODCDTIP=TBLINTFU AND RELCDSIT<98", DS)
        Else
            GAIA2.bdr(objConn, "SELECT * FROM METLNOD WITH(NOLOCK) ,METLTBL  WITH(NOLOCK) WHERE NODINNOD=" + codiNode.ToString() + " AND NODCDTIP=TBLINTFU", DS)
        End If
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            codiNode = dbRow("NODINNOD")
            Dim seleccioIdioma As String = ""
            If codiIdioma <> 99 Then
                seleccioIdioma = dbRow("TBLDSTXT").substring(4, 3) & "INIDI=" & codiIdioma & " AND "
            End If
            GAIA2.bdr(objConn, "SELECT * FROM " + dbRow("TBLDSTXT") + " WITH(NOLOCK)  WHERE " & seleccioIdioma & dbRow("TBLDSTXT").substring(4, 3) + "INNOD=" + codiNode.ToString(), DS)
            If DS.Tables(0).Rows.Count > 0 Then
                existeixContingut = 1
            End If
        End If
        DS.Dispose()
    End Function         'existeixContingut						






    '************************************************************************************************************
    '	Funció: GAIA.obtenirFillRelacio
    '	Entrada:  codiRelacio
    '						
    '	Procés: retorna 1 si la pàgina web hereta les propietats dels nodes web i arbre web superiors,
    '					en cas contrari retorna 0
    '************************************************************************************************************
    Public Shared Function heretarPropietatsWeb(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer, ByRef width As Integer) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        heretarPropietatsWeb = 1
        DS = New DataSet()

        GAIA2.bdr(objConn, "SELECT WEBTPHER,WEBWNMTH FROM METLWEB WITH(NOLOCK) , METLREL WITH(NOLOCK)  WHERE RELINFIL=WEBINNOD AND RELINCOD=" + codiRelacio.ToString(), DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            If dbRow("WEBTPHER") = "S" Then
                heretarPropietatsWeb = 1

            Else
                heretarPropietatsWeb = 0

                width = dbRow("WEBWNMTH")


                'no modifico el width
            End If
        End If
        DS.Dispose()
    End Function



    Public Shared Function heretarPropietatsWeb(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer) As Integer
        Dim width As Integer = 0
        Return (GAIA2.heretarPropietatsWeb(objConn, codiRelacio, width))
    End Function         'heretarPropietatsWeb			



    '************************************************************************************************************
    '	Funció: GAIA.obtenirCodiArbreDeRelacio
    '	Entrada:  node: RadTreeNode
    '						
    '	Procés: Obté el codiArbre  de la relació
    '************************************************************************************************************
    Public Shared Function obtenirCodiArbreDeRelacio(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        obtenirCodiArbreDeRelacio = 0
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT RELCDARB FROM METLREL  WITH(NOLOCK) WHERE RELINCOD=" & codiRelacio & " AND RELCDSIT<98 ", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirCodiArbreDeRelacio = dbRow("RELCDARB")
        End If
        DS.Dispose()
    End Function         'obtenirCodiArbreDeRelacio										



    '***************************************************************************************************
    '	Funció: GAIA.debug
    '	Entrada:  texto: text que volem escriure a la bd
    '						
    '	Procés: Escriu a la taula METLDEB (debug) el text pasat com a paràmetre
    '***************************************************************************************************					
    Public Shared Sub debug(ByVal objConn As OleDbConnection, ByVal texto As String)
        GAIA2.bdSR(objConn, "INSERT INTO METLDEB (TEXT,data) VALUES ('" + texto.Replace("'", "''") + "','" + Now + "')")
    End Sub 'debug			



    'modeEdicio: si modeEdicio=1 llavors ensenyem les etiquetes sense descripció (div x.y.z)


    Public Shared Function pintaEstructura(ByVal objConn As OleDbConnection, ByRef arrayEstructura As String(), ByVal arrayAtributs As String(), ByVal arrayTVer As String(), ByVal arrayTHor As String(), ByVal veureEstructura As Integer, ByVal descripcio As String, ByVal border As Integer, ByVal sufix As String, ByVal arrayDescripcions As String(), ByVal arrayPlt As String(), ByVal modeEdicio As Integer) As String
        Dim res As String = ""
        Dim item, hreftxt1, hreftxt2, txt, nomDivisio, selectPlantilles As String
        Dim cont, i, orden As Integer
        Dim idxAtributs, idxAtributsSeguent As Integer
        Dim pintarDesc As Boolean = False
        Dim PlantillesPerCelda As String()
        Dim stronclick As String = ""
        Dim alturaCelda As Integer = 20
        hreftxt2 = ""
        hreftxt1 = ""
        Dim HeightTotal As Integer = 0

        If arrayEstructura.Length = 1 Then
            If veureEstructura = 1 Then
                If (Not arrayDescripcions Is Nothing) Then
                    If (arrayDescripcions.Length > 0) Then
                        If (arrayDescripcions(0).Trim.Length > 0) Then
                            res = "<table border=" + border.ToString() + " width=""100%"" height=""100%"" class=""tablabordepeqroj"" bordercolor=""#FFCDCA"" cellspacing=""0"" cellpadding=""0"" onclick=""seleccionaCelda('t0','P&agrave;gina en blanc');activaCamps(true);return false""><tr><td id=""t0""><div align=""center""><a href=""#"" onclick=""seleccionaCelda('t0','P&agrave;gina en blanc');activaCamps(true);return false"" class=""txtneg12px"">" + descripcio + arrayDescripcions(0) + sufix + "</a></div></td></tr></table>"
                            pintarDesc = True
                        End If

                    End If
                End If
                If Not pintarDesc Then
                    res = "<table border=" + border.ToString() + " width=""100%"" height=""100%"" class=""tablabordepeqroj"" bordercolor=""#FFCDCA"" cellspacing=""0"" cellpadding=""0"" onclick=""seleccionaCelda('t0','Pàgina en blanc');activaCamps(true);return false;""><tr><td id=""t0""><div align=""center""><a href=""#"" onclick=""seleccionaCelda('t0','P&agrave;gina en blanc');activaCamps(true);return false;"" class=""txtneg12px"">" + descripcio + "Pàgina en blanc" + sufix + "</a></div></td></tr></table>"
                End If
            Else
                res = "<table border=" + border.ToString() + " width=100% height=100% class=tablabordepeqroj bordercolor=#FFCDCA cellspacing=0 cellpadding=0><tr><td id=""t0""><div align=""center""><a href=""#"" class=""txtneg12px"">" + descripcio + "0" + sufix + "</div></td></tr></table>"
            End If
        Else
            For cont = 0 To arrayEstructura.Length - 1
                stronclick = ""
                item = arrayEstructura(cont)
                txt = item.Substring(0, item.Length - 3)
                idxAtributs = item.Substring(item.Length - 3)
                idxAtributsSeguent = 0
                'ini codi sara
                orden = item.Substring(item.Length - 3, 3)
                'fi codi sara

                If cont < arrayEstructura.Length - 1 Then
                    idxAtributsSeguent = arrayEstructura(cont + 1).Substring(arrayEstructura(cont + 1).Length - 3)
                End If
                nomDivisio = ""
                For i = 0 To txt.Length - 1
                    nomDivisio += txt.Chars(i) + "."
                Next

                If veureEstructura Then
                    pintarDesc = False
                    If (Not arrayDescripcions Is Nothing) Then
                        If (arrayDescripcions.Length > 0) Then
                            If (arrayDescripcions(orden).Trim.Length > 0) Then
                                'faig un filtre de condicions per mostrar o no una cel·la"
                                stronclick = " onclick=""seleccionaCelda('t" + idxAtributs.ToString() + "','" + nomDivisio + "');activaCamps(true);return false;"""
                                hreftxt1 = "<div align=""center""><a href=""#"" onclick=""seleccionaCelda('t" + idxAtributs.ToString() + "','" + nomDivisio + "');activaCamps(false);return false;"" class=""text06 t90"
                                hreftxt2 = "<div align=""center""><a href=""#"" onclick=""seleccionaCelda('t" + idxAtributs.ToString() + "','" + nomDivisio + "');activaCamps(true);return false;"" class=""text06 t90"
                                If modeEdicio <> 1 Then
                                    hreftxt1 += " vermell"
                                    hreftxt2 += " vermell"
                                End If
                                hreftxt1 += """>" + descripcio + arrayDescripcions(orden) + sufix + "</a></div>"
                                hreftxt2 += """>" + descripcio + arrayDescripcions(orden) + sufix + "</a></div>"

                                pintarDesc = True
                                '	END IF
                            End If
                        End If
                    End If
                    If Not pintarDesc Then
                        'IF  modeEdicio=1 THEN			
                        hreftxt1 = "<div align=""center""><a href=""#"" onclick=""seleccionaCelda('t" + idxAtributs.ToString() + "','" + nomDivisio + "');activaCamps(false);return false;"" class=""text06 t90"">" + descripcio + "Div " + nomDivisio.Replace("0.", "") + sufix + "</a></div>"
                        hreftxt2 = "<div align=""center""><a href=""#"" onclick=""seleccionaCelda('t" + idxAtributs.ToString() + "','" + nomDivisio + "');activaCamps(true);return false;"" class=""text06 t90"">" + descripcio + "Div " + nomDivisio.Replace("0.", "") + sufix + "</a></div>"
                        stronclick = " onclick=""seleccionaCelda('t" + idxAtributs.ToString() + "','" + nomDivisio + "');activaCamps(false);return false;"""

                    End If
                Else


                    hreftxt1 = descripcio + nomDivisio + sufix
                    hreftxt2 = descripcio + nomDivisio + sufix
                End If
                selectPlantilles = ""
                If (Not arrayPlt Is Nothing) Then
                    If (arrayPlt.Length > 0) Then
                        If (arrayPlt(orden).Trim.Length > 0) Then
                            PlantillesPerCelda = Split(arrayPlt(orden), "|")
                            For Each item In PlantillesPerCelda
                                If Trim(selectPlantilles) <> "" Then selectPlantilles += "<br/>"
                                selectPlantilles += GAIA2.obtenirNomPlantilla(objConn, item)
                            Next item
                        End If
                    End If
                End If


                If arrayAtributs(idxAtributs).IndexOf("i") >= 0 Then
                    If cont <> 0 Then
                        res += "<table width=""100%"" height=""100%"" border=" + border.ToString() + " class=""tablabordepeqroj"" bordercolor=""#FFCDCA"" cellspacing=""0"" cellpadding=""0"">"
                    End If
                    res += "<tr valign=top height=""" & alturaCelda & "px""><td  id=""t" + (idxAtributs).ToString() + """ valign=""top"" "
                    HeightTotal += alturaCelda
                    If modeEdicio <> 1 Then
                        res += stronclick
                    End If
                    res += ">"
                    If cont < arrayEstructura.Length - 1 Then
                        If arrayAtributs(idxAtributsSeguent).IndexOf("i") < 0 Then
                            res += hreftxt2
                        Else
                            res += hreftxt1
                        End If
                    Else

                        res += hreftxt2
                    End If
                Else
                    If arrayAtributs(idxAtributs).IndexOf("v") >= 0 Then
                        res += "</td><td id=""t" + (idxAtributs).ToString() + """ height=""" & alturaCelda & "px"" "
                        If modeEdicio <> 1 Then
                            res += stronclick
                        End If
                        res += ">"
                    Else  'divisió horitzontal
                        res += "</td></tr><tr valign=""top"" height=""" & alturaCelda & "px""><td id=""t" + (idxAtributs).ToString() + """  "
                        HeightTotal += alturaCelda
                        If modeEdicio <> 1 Then
                            res += stronclick
                        End If
                        res += ">"
                    End If
                    If cont < arrayEstructura.Length - 1 Then
                        If arrayAtributs(idxAtributsSeguent).IndexOf("i") < 0 Then
                            res += hreftxt2
                        Else
                            res += hreftxt1
                        End If
                    Else
                        res += hreftxt2
                    End If
                    If cont < arrayEstructura.Length - 1 Then
                        For i = 1 To arrayEstructura(cont).Length - arrayEstructura(cont + 1).Length
                            res += "</td></tr></table>"
                        Next i
                    End If
                End If
            Next cont


            For i = 1 To arrayEstructura(cont - 1).Length - 3
                res += "</td></tr></table>"
            Next i
        End If
        res = "<table width=""100%"" height=""" & (HeightTotal + (HeightTotal / alturaCelda) * 30) & "px"" border=" + border.ToString() + " class=""tablabordepeqroj"" bordercolor=""#FFCDCA"" cellspacing=""0"" cellpadding=""0"">" & res



        pintaEstructura = res
    End Function 'GAIA.pintaEstructura



    '***********************************************************************************************************
    '	Funció: GAIA.dibuixaPreview
    '	Entrada:  
    '					estructura: llista, separada per comes, de codis que representen l'estructura. 
    '											cada codi té el següent format:  aaaNNN  
    '																				aaa: codificació del contingut dintre d'un arbre.
    '																				Ex: si una pàgina es divideix en dos parts, tindrem: 01, 011, 012,
    '																						on 01 és el continent principal, 011 i 012 són fills de 01.
    '																				NNN: ordre del codi dins de l'estructura
    '					thor: 	llista, separada per comes, de tamanys relatius (%) horitzontals
    '					tver: 	llista, separada per comes, de tamanys relatius (%) verticals
    '					tcont: 	llista, separada per comes, del codis que representen el tipus de contingut
    '					tplant: llista, separada per comes, del codis que representen la plantilla que conté
    '					html: 	string per referència on guardaré el html resultant.
    '					css: 	string per referència on guardaré el css resultant per la pàgina
    '					tMaxHor: Tamany Horitzontal màxim de la pàgina generada
    '					tMaxVer: Tamany Vertical màxim de la pàgina generada
    '					prefix: String que afegiré als id de cada div
    '					rel: Relació on el fill apunta al node que volem representar
    '					
    '					estilCSS: llista, sepearada per comes, dels codis dels estils CSS que he d'afegir al <div>
    '					relacioInicial: codi de la relació des d'on he d'anar heretan les propietats de l'arbre o nodes
    '					dataSimulacio: string amb la data que volem simular com es veurà la pàgina. Si "" la data serà l'actual
    '					codiUsuari: codi de l'usuari GAIA que inicia l'acció
    '					autolink: si True llavors miraré les relacions dins de les cel·les que apareixen als camps d'estructura d'autolinks (RELCDESTLK)
    '					plantillesSecundaries: Si una cel·la d'una plantilla no té una taula.camp llavors intento obrir la plantilla
    '					heretaPropietatsWeb: booleà que indica si he de representar la fulla independentment de la seva herència o no.
    ' 				estilFLW: string amb llista de booleans separats per ",". Si element amb valor=1 llavors el contingut ha de fluir amb 
    '										el contingut de la cel·la adjacent (esquerra o dreta)
    '					titol: string amb el text que vaig construint amb el títol de la pàgina web.
    ' iteracioInicial: si una fulla web hereta propietats, primer haurà de pujar al nivell de l'arbre web i baixar de nou fins trobar la fulla web. 
    '                       quan iteracioInicial=false no aniré a cercar elements superiors
    '	Procés: 

    '					Prepara la crida a la funció recursiva "dibuixax" i retorna el contingut dins de "html" i "css"
    '***********************************************************************************************************				

    Public Shared Sub dibuixaPreview(ByVal objConn As OleDbConnection, ByVal plantilla As clsPlantilla, ByVal estructura As String, ByVal formaEstructura As String(), ByVal thor As String, ByVal tver As String, ByVal tcont As String, ByVal tplant As String, ByRef html As String, ByRef css As String, ByVal tMaxhor As Double, ByVal tMaxVer As Double, ByVal prefix As String, ByVal rel As clsRelacio, ByVal pathRelacio As String, ByRef fDesti As String, ByRef urlDesti As String, ByRef nrocontingut As Integer, ByRef llistaDocuments As String(), ByRef publicar As Integer, ByVal idioma As Integer, ByVal estilCSS As String, ByVal relIni As clsRelacio, ByVal dataSimulacio As DateTime, ByVal codiUsuari As Integer, ByVal autolink As Integer, ByVal plantillesSecundaries As String, ByVal heretaPropietatsWeb As Boolean, ByVal estilFLW As String, ByRef titol As String, ByRef nivell As Integer, ByRef estilbody As String, ByVal llistaCND As String, ByRef esForm As String, ByRef esEML As String, ByRef esSSL As String, ByRef tagsMeta As String, ByRef htmlPeu As String, ByRef strCSSPantalla As String, ByRef strCSSImpressora As String, ByVal llistaNivellsTextos As String, Optional ByVal pareTeEnllac As Boolean = False)


        Dim tanca As Integer
        Dim r As Random = New Random()
        Dim hEstructura As New Hashtable()
        Dim arrayEstructura As String()
        Dim item As String
        Dim cont As Integer = 0
        Dim cssini As Integer
        Dim codiRelacio As Integer
        Dim tancaVoreres As Integer = 0
        Dim htmlIni As String



        htmlIni = html
        codiRelacio = rel.incod

        cssini = (css.Length = 0)
        tanca = 0
        If cssini Then
            If (tMaxhor <> 0) And prefix <> "p" And prefix <> "f" Then
                css += "<style type=""text/css"" media=""screen""><!-- .body {margin:0px; font-size: 100%;} #contenidor{width: " + tMaxhor.ToString() + "px; min-height:" + tMaxVer.ToString() + "px; margin-left:auto; margin-right:auto;} #contingut{clear:both}"
            Else
                css += "<style type=""text/css""><!--"
            End If
        End If


        arrayEstructura = Split(estructura, ",")
        'tracto el cas del primer element a l'estructura		
        Dim arraythor As String()
        Dim width As Double
        arraythor = Split(thor, ",")

        If arrayEstructura.Length >= 0 Then
            hEstructura.Add("", "0")
            cont = 1
            If arrayEstructura(0) <> 100 Then
                width = CInt((arraythor(arrayEstructura(0)) * tMaxhor) \ 100)
                If (((arraythor(arrayEstructura(0)) * tMaxhor) \ 100) - width) >= 0.5 Then
                    width = width + 1
                End If
                tMaxhor = width

            End If

            dibuixaPreviewRec(objConn, plantilla, "", "", hEstructura, formaEstructura, html, css, arraythor, Split(tver, ","), r, tMaxhor, tMaxVer, tplant, prefix, rel, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, estilCSS, relIni, dataSimulacio, codiUsuari, autolink, tcont, plantillesSecundaries, heretaPropietatsWeb, 1, estilFLW, titol, nivell, estilbody, llistaCND, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, llistaNivellsTextos, tancaVoreres, pareTeEnllac)
            tanca = 1
        End If

        cont = 0
        hEstructura = New Hashtable()
        If arrayEstructura.Length > 1 Then
            For Each item In arrayEstructura
                If cont > 0 Then
                    hEstructura.Add(item.Substring(1, item.Length - 4), cont.ToString())
                End If
                cont += 1
            Next item

            dibuixaPreviewRec(objConn, plantilla, "", "1", hEstructura, formaEstructura, html, css, arraythor, Split(tver, ","), r, tMaxhor, tMaxVer, tplant, prefix, rel, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, estilCSS, relIni, dataSimulacio, codiUsuari, autolink, tcont, plantillesSecundaries, heretaPropietatsWeb, 0, estilFLW, titol, nivell, estilbody, llistaCND, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, llistaNivellsTextos, 0, pareTeEnllac)
            dibuixaPreviewRec(objConn, plantilla, "", "2", hEstructura, formaEstructura, html, css, arraythor, Split(tver, ","), r, tMaxhor, tMaxVer, tplant, prefix, rel, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, estilCSS, relIni, dataSimulacio, codiUsuari, autolink, tcont, plantillesSecundaries, heretaPropietatsWeb, 0, estilFLW, titol, nivell, estilbody, llistaCND, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, llistaNivellsTextos, 0, pareTeEnllac)


        End If

        If cssini Then
            css += "--></style>"
        End If


        For i As Integer = 0 To tancaVoreres - 1

            If esEML = "N" Then
                html &= "</div></div><div class=""botBloc""></div>"
            Else
                'html &= "</t2>"
            End If


        Next i

        If tanca > 0 Then

            If esEML = "S" Then
                If prefix <> "f" Then
                    'If prefix = "e" Or prefix = "n" Or prefix = "c" Then
                    html &= "</td></tr></table>"
                End If

            Else
                html &= "</div>"

            End If
            tanca = 0


        End If
        If False Then

            If esEML = "S" Then
                'msv,12/2/14: hi ha un cas que no es tanquen els <table> en cas de cridar dins de pintarContingut al previewrec. De moment comprovo el que falta i ho poso aquí
                Dim nroTablesOberts As Integer = Regex.Split(html, "<table").Length - 1
                If nroTablesOberts > Regex.Split(html, "</table").Length - 1 Then
                    html &= "</td></tr></table>"
                End If

            End If
        End If



        hEstructura = Nothing
    End Sub 'dibuixaPreview


    Public Shared Function strcount(ByVal text As String, ByVal find As String) As Integer
        Return Regex.Split(text, find).Length - 1
    End Function

    '************************************************************************************************************
    '	Funció: GAIA.dibuixaPreviewRec
    '	Entrada:  
    '					Index: string que apunta a la estructura que estic tractant. (com que l'estructura és un arbre
    '									binari, un pare sempre tindrà com a màxim 2 fills. Ex: 01, 011,012, 0121, 0122, etc.)
    '					estructura: estructura de hash on he emmagatzemat la estructura , de codis que representen l'estructura.
    '											ho faig així per poder accedir aleatoriament als camps (com si fos un diccionari)
    '					html: 	string per referència on guardaré el html resultant.
    '					css: 	string per referència on guardaré el css resultant per la pàgina
    '					thor: 	array de tamanys relatius (%) horitzontals
    '					tver: 	array de tamanys relatius (%) verticals
    '					r:	 valor random, ho utilitzo per fer el preview amb colors diferents.. (això s'eliminarà)
    '					tMaxHor: Tamany Horitzontal màxim de la pàgina generada
    '					tMaxVer: Tamany Vertical màxim de la pàgina generada
    '					tplant: array  del codis que representen la plantilla que conté
    '					prefix: String que afegiré als id de cada div
    '					rel: Relació on el fill apunta al node que volem representar
    '					pathRelacio: codi de la relació des d'on he d'anar heretan les propietats de l'arbre o nodes
    '					estilCSS: string amb els codis dels estilsCSS que he d'afegir als <div>
    '					relacioInicial: codi de la relació que ha iniciat el procés de dibuixarPreview
    '					dataSimulacio: string amb la data que volem simular com es veurà la pàgina. Si "" la data serà l'actual
    '					codiusuari: codi de l'usuari que inicia l'acció
    '					autolink: si true llavors miraré les relacions dins de les cel·les que apareixen als camps d'estructura d'autolinks (RELCDESTLK)
    '					celdaInicial: si 1 llavors no tancare el <div> per que es farà a dibuixapreview. 
    '					tcont : llista separada per , amb tipus de continguts
    '					plantillesSecundaries: llista separada per "," amb plantilles secundaries per a una cel·la sense taula.camp
    '					heretaPropietatsWeb: booleà que indica si he de representar la fulla independentment de la seva herència o no.
    '					estilFLW: string amb llista de booleans separats per ",". Si element amb valor=1 llavors el contingut ha de fluir amb 

    '										el contingut de la cel·la adjacent (esquerra o dreta)
    '					titol: titol de la pàgina web

    '					nivell: nivell dintre de l'ordre de continguts. En un futur es podria utilitzar per <hx></hx>
    '					llistaCND: llista de valors que indiquen si en una cel·la sense contingut s'ha de mostrar el text "contingut no disponible"
    '	Procés: 
    '					Funció recursiva que genera una estructura ACCESSIBLE (sense l'ús de taules),
    '					on insertaré els continguts.
    '************************************************************************************************************				
    Public Shared Function dibuixaPreviewRec(ByVal objConn As OleDbConnection, ByVal plantilla As clsPlantilla, ByVal index As String, ByVal subindex As String, ByVal estructura As Hashtable, ByVal formaEstructura As String(), ByRef html As String, ByRef css As String, ByVal thor As String(), ByVal tver As String(), ByVal r As Random, ByRef tMaxHor As Double, ByVal tMaxVer As Double, ByVal strTplant As String, ByVal prefix As String, ByVal rel As clsRelacio, ByRef pathRelacio As String, ByRef fDesti As String, ByRef urlDesti As String, ByRef nrocontingut As Integer, ByRef llistaDocuments As String(), ByRef publicar As Integer, ByVal idioma As Integer, ByVal estilCSS As String, ByVal relIni As clsRelacio, ByVal dataSimulacio As DateTime, ByVal codiUsuari As Integer, ByVal autolink As Integer, ByVal tcont As String, ByVal plantillesSecundaries As String, ByVal heretaPropietatsWeb As Boolean, ByVal celdaInicial As Integer, ByVal estilFLW As String, ByRef titol As String, ByVal nivell As Integer, ByRef estilbody As String, ByVal llistaCND As String, ByRef esForm As String, ByRef esEml As String, ByRef esSSL As String, ByRef tagsMeta As String, ByRef htmlPeu As String, ByRef strCSSPantalla As String, ByRef strCSSImpressora As String, ByVal llistaNivellsTextos As String, Optional ByRef tancavoreres As Integer = 0, Optional ByVal pareTeEnllac As Boolean = False) As Integer

        If estructura.Contains(index + subindex) Then
            Dim strTmp As String = ""
            Dim codiplantillacelda As Integer
            Dim width, height As Double
            width = 0
            height = 0
            Dim DS, DS2 As DataSet
            Dim dbRow, dbRow2 As DataRow
            Dim relFill As New clsRelacio
            Dim tplant, arrayEstilCSS, arrayTCont, arrayFLW, arrayCND, arrayNivell As String()
            Dim cont As Integer
            Dim strSql, descTipus As String
            Dim strsqlMETLNOD, strsqlMETLTIP, strsqlMETLREI, strsqlWHERE As String
            Dim strCSS As String
            Dim desfaseHoritzontal As Integer
            Dim arrayTMPPlantillesSec As String()
            Dim codiRelacio As Integer
            Dim fesFlow1, fesflow2 As Integer
            Dim nroMaximContinguts As Integer = 0
            Dim strCSSTamanyText As String = ""
            Dim lcw As String = ""
            codiRelacio = rel.incod
            Dim cssTmp As String = ""
            Dim plantillaOriginal As New clsPlantilla
            plantillaOriginal.copiaObj(plantilla)

            Dim arrayPltdsnum As String()
            arrayPltdsnum = plantilla.num.Split(",")

            strCSS = ""
            tplant = Split(strTplant, ",")
            Dim arrayPlantillesPerCelda As String() = {}
            arrayTCont = Split(tcont.Trim(), ",")
            arrayEstilCSS = Split(estilCSS.Trim(), "|")
            arrayNivell = Split(llistaNivellsTextos.Trim(), ",")
            arrayFLW = Split(estilFLW.Trim(), ",")
            arrayCND = Split(llistaCND.Trim(), ",")
            DS = New DataSet()
            DS2 = New DataSet()
            Dim nroFillsSenars, nroFillsParells As Integer
            Dim htmlIni, htmlNou As String
            Dim strCssSenseFons As String = ""
            htmlIni = ""
            htmlNou = ""

            Dim stil As String
            Dim stilWidth As Integer
            Dim forçarCodi As Integer = 0

            stil = ""
            stilWidth = 0
            Dim posaWidthGAIA As Integer = 1
            Dim posaFloatGAIA As Boolean = True
            Dim posaVoreres As String = ""
            Dim Hnivell As String = "0"
            Dim estatSituacio As String = "98"
            If Day(Now) & Month(Now) & Year(Now) > Day(dataSimulacio) & Month(dataSimulacio) & Year(dataSimulacio) Then
                'Si la data de simulació es diferent al dia d'avui buscaré també continguts caducats.Per ara no tindré en compte la hora. (30/11/06-MAX)
                estatSituacio = "99"
            End If


            '***************************************************************************
            '	L'ESTRUCTURA CONTÉ AQUESTA CEL·LA I L'HE DE TRACTAR
            '***************************************************************************			
            If arrayEstilCSS.Length >= estructura(index + subindex) + 1 Then ' Si hi ha un estil css el poso									
                If arrayEstilCSS(estructura(index + subindex + "")).Length > 0 Then
                    strCSS = GAIA2.trobaEstilCSS(objConn, arrayEstilCSS(estructura(index + subindex + "")), desfaseHoritzontal, heretaPropietatsWeb, strCSSTamanyText, strCssSenseFons, esEml, posaWidthGAIA, posaVoreres, posaFloatGAIA).ToString()
                    If (strCSSTamanyText).Length > 0 Then

                        strCSS = strCSSTamanyText
                    End If
                End If
            End If
            'Calculs dels tamanys i flows	
            If tMaxHor <> 0 Then
                width = CInt((thor(estructura(index + subindex + "")).Replace(".", ",") * tMaxHor) \ 100)
                If ((thor(estructura(index + subindex + "")).Replace(".", ",") * tMaxHor) / 100) - width >= 0.5 Then
                    width = width + 1
                End If
            End If
            If tMaxHor <> 0 Then
                width = width - desfaseHoritzontal
                If celdaInicial Then
                    tMaxHor = tMaxHor - desfaseHoritzontal
                End If
            End If
            height = (tver(estructura(index + subindex + "")) * tMaxVer) \ 100
            fesFlow1 = 0
            fesflow2 = 0
            If strCSS.Length = 0 Then
                If esEml = "S" Then
                    strCSS = "style="""""
                Else
                    strCSS = "class="""""
                End If
            End If

            If estilFLW <> "" And estilFLW <> "9" Then 'valor inicial que guardo per compatibilitat amb versió anterior				
                If arrayFLW(estructura(index + "1")).Trim() = "1" Then
                    fesFlow1 = 1
                End If
                If arrayFLW(estructura(index + "2")).Trim() = "1" Then
                    fesflow2 = 1
                End If
            End If
            If fesFlow1 = 0 And fesflow2 = 0 Then

                If posaWidthGAIA Then
                    If tMaxHor <> 0 Then
                        If esEml = "S" Then
                            strCSS = strCSS.Replace("style=""", "style=""width:" + width.ToString() + "px;")
                        Else
                            strCSS = strCSS.Replace("class=""", "class=""wpx" + width.ToString() + " ")
                        End If
                        'END IF
                        stil += "width: " + width.ToString() + "px;"
                        stilWidth = width.ToString()
                    End If
                End If
                If posaFloatGAIA Then 'posaWidthGAIA THEN '
                    If tMaxHor <> 0 Then
                        If esEml = "S" Then
                            strCSS = strCSS.Replace("style=""", "style=""float:left;")
                        Else
                            strCSS = strCSS.Replace("class=""", "class=""floatleft ")
                        End If

                    End If
                End If
            Else
                If fesFlow1 = 1 And fesflow2 = 1 Then 'cas de cel·la adjacent vertical
                    stil += ""
                Else
                    If fesFlow1 = 1 Then 'flow2=0
                        Select Case subindex
                            Case "1"
                                stil += ""
                            Case "2"
                                If strCSS.Length = 0 Then
                                    If esEml = "S" Then
                                        strCSS = "style=""float:right;"""
                                    Else
                                        strCSS = "class=""floatright"""
                                    End If
                                Else
                                    If esEml = "S" Then
                                        strCSS = strCSS.Replace("style=""", "style=""float:right;")
                                    Else
                                        strCSS = strCSS.Replace("class=""", "class=""floatright ")
                                    End If
                                End If
                                If tMaxHor <> 0 Then

                                    'stil += "width: " + width.ToString() + "px;"
                                    stilWidth = width
                                    If esEml = "S" Then

                                        ' strCSS = strCSS.Replace("style=""", "style=""" + stil + "")
                                    Else
                                        strCSS = strCSS.Replace("class=""", "class=""wpx" + width.ToString() + " ")
                                    End If
                                End If
                        End Select
                    Else 'flow1=0, flow2=1



                        Select Case subindex
                            Case "1"


                                If tMaxHor <> 0 Then
                                    If strCSS.Length = 0 Then
                                        If esEml = "S" Then
                                            strCSS = "style=""float:left;"""
                                        Else
                                            strCSS = "class=""floatleft"""
                                        End If
                                    Else
                                        If esEml = "S" Then
                                            strCSS = strCSS.Replace("style=""", "style=""float:left;")
                                        Else
                                            strCSS = strCSS.Replace("class=""", "class=""floatleft ")
                                        End If
                                    End If

                                    If width <> 0 Then
                                        If esEml = "S" Then
                                            ' strCSS = strCSS.Replace("style=""", "style=""width:" + width.ToString() + "px;")
                                        Else
                                            'tinc dos cel·les verticals, i estic tractant la de l'esquerra. Com que no se si la de la dreta tindrà contingut, l'intento generar i si retorna "" poso width100%. En cas contrari poso el width que li toca.								
                                            Try
                                                If formaEstructura(estructura(index + "2")) = "v" Then
                                                    Dim tmpHTML As String = ""
                                                    Dim arrayTMP As String() = Nothing
                                                    dibuixaPreviewRec(objConn, plantilla, index, "2", estructura, formaEstructura, tmpHTML, "", thor, tver, r, 100, 100, strTplant, prefix, rel, "", "", "", 0, arrayTMP, 0, idioma, estilCSS, relIni, dataSimulacio, codiUsuari, autolink, tcont, plantillesSecundaries, heretaPropietatsWeb, 0, estilFLW, titol, nivell, 0, llistaCND, "N", "N", "N", "", "", "", "", llistaNivellsTextos, False, False)
                                                    If tmpHTML.Length > 0 Then
                                                        strCSS = strCSS.Replace("class=""", "class=""wpx" + width.ToString() + " ")
                                                    Else
                                                        width = tMaxHor
                                                    End If
                                                End If
                                            Catch
                                            End Try


                                        End If
                                    End If
                                    stil += "width: " & width & "px;"
                                    stilWidth = width
                                End If
                            Case "2"
                                stil += ""
                        End Select
                    End If
                End If
            End If


            Hnivell = "0"

            'Tinc en compte el nivell de text
            If arrayNivell.Length >= estructura(index + subindex) + 1 Then ' Si hi ha un estil css el poso				
                If arrayNivell(estructura(index + subindex + "")).Length > 0 Then
                    'agafo el nivell 
                    If arrayNivell(estructura(index + subindex + "")) <> "0" Then
                        Hnivell = arrayNivell(estructura(index + subindex + ""))
                    End If
                End If
            End If


            If esEml = "N" Then
                htmlIni = IIf(Hnivell = "0", "<div ", "<h" & Hnivell) & strCSS & ">"
            Else
                If InStr(strCSS, "alignleft") Then
                    strCSS = strCSS.Replace("alignleft", "")
                    strTmp = " align=""left"" "
                End If
                If InStr(strCSS, "alignright") Then
                    strCSS = strCSS.Replace("alignright", "")
                    strTmp = " align=""right"" "
                End If
                If InStr(strCSS, "aligncenter") Then
                    strCSS = strCSS.Replace("aligncenter", "")
                    strTmp = " align=""center"" "
                End If

                strCSS = strCSS.Replace("float:left;", "")

                strCSS = strCSS.Replace("width:" & width & "px;", "")



                'els estils alignleft,alignright i aligncenter són especials per fer mailings, que són <table>. Els tracto com a especials.

                'f: posar format

                If (subindex <> "2" Or celdaInicial = 1) And Not prefix = "f" Then
                    htmlIni = "<table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""" & tMaxHor & """ " & strTmp & "><tr><td>"
                End If
            End If



            lcw = representaCodiWeb(objConn, plantilla, rel, estructura(index + subindex), idioma, relIni, width, codiUsuari, forçarCodi, css, tagsMeta)
            If forçarCodi = 1 Then
                htmlNou = lcw
                lcw = ""
            End If






            ' Si no hi ha + fills llavors represento el contingut. En cas contrari no ho faig.									
            'Contingut del quadre que es troba en algun objecte fill dintre de l'arbre (per exemple 2 fotos dins de la cel·la)
            If Not estructura.Contains(index + subindex + "1") Then
                If codiRelacio > 0 Then



                    Dim htmlTMP As String = ""

                    'Només intento materialitzar la cel·la si és una cel·la d'una pàgina web. Si no faig això passarà per aquí també per cada element de la llista
                    ' If rel.incod = relIni.incod Then
                    htmlTMP = GetHTML(objConn, "contingutCella?codirelacio=" & rel.incod & "&codiIdioma=" & idioma & "&codiRelacioInicial=" & relIni.incod & "&est=" & estructura(index + subindex) & "&width=" & width & "&node=" & rel.infil & "&us=" & codiUsuari & "&codiPlantilla=" & plantilla.innod, 0, idioma, Now, cssTmp, relIni, rel, estructura(index + subindex), codiUsuari, plantilla.innod)
                    If cssTmp.Length > 0 Then
                        css &= cssTmp
                    End If
                    'End If

                    If htmlTMP <> "" Then
                        htmlNou &= htmlTMP.Trim()


                    Else
                        descTipus = rel.tipdsdes
                        If autolink Then
                            strSql = "SELECT RELINCOD, NODINNOD, NODCDTIP, RELCDEST FROM  METLREL WITH(NOLOCK) , METLNOD WITH(NOLOCK)  WHERE RELCDRSU=" & rel.incod & " AND RELINFIL=NODINNOD AND RELCDSIT<" + estatSituacio
                            If arrayTCont.Length >= estructura(index + subindex) + 1 Then
                                If (arrayTCont(estructura(index + subindex + "")).Length > 0) Then
                                    strSql += " AND NODCDTIP=" + arrayTCont(estructura(index + subindex + ""))
                                Else
                                    strSql = ""
                                End If
                            Else
                                strSql = ""
                            End If
                        Else
                            'monto els continguts

                            strSql = "SELECT isnull(REIINCOD, 0) AS REIINCOD, isnull(REIDTCAD, '1/1/1900') AS REIDTCAD, isnull(REIDTPUB, '1/1/1900') AS REIDTPUB, metlnodFill.*, relacioFill.* FROM METLREL AS relacioFill WITH(NOLOCK) "
                            strsqlMETLNOD = " INNER JOIN METLNOD as metlnodFill WITH(NOLOCK) on metlnodFill.NODINNOD=relacioFill.RELINFIL "
                            strsqlMETLTIP = " INNER JOIN METLTIP WITH(NOLOCK) ON TIPINTIP=metlnodFill.NODCDTIP "
                            strsqlMETLREI = " LEFT OUTER JOIN METLREI WITH(NOLOCK) ON REIINCOD=relacioFill.RELINCOD AND REIINIDI=" & idioma
                            strsqlWHERE = " WHERE relacioFill.RELCDHER = '" + rel.cdher + "_" + rel.infil.ToString() + "'  AND relacioFill.RELCDSIT<" + estatSituacio + " "
                            ' Busco els continguts de la cel·la o els del mateix tipus que no s'han ubicat (valor -1). No accepto valor -2 perq són relacions que no  s'han ubicat perque hi havia + d'una casella candidata.

                            If arrayTCont.Length >= estructura(index + subindex) + 1 Then
                                If (arrayTCont(estructura(index + subindex + "")).Trim().Length > 0) Then
                                    If arrayTCont(estructura(index + subindex + "")) <> "0" Then
                                        If arrayTCont(estructura(index + subindex + "")) = 10 Then
                                            ' Si estic buscant fulles web, cerco totes les que pengin d'aquesta cel·la, així em podré saltar les cel·les intermitjes de tipus fulla web. (això ho faig per saltarme fulles web que hi ha enmig i només utilitzo per organitzar la informació (cas de siotweb)	
                                            ' strSql = "SELECT metlnodFill.*, relacioFill.* FROM METLREL AS relacioFill WITH(NOLOCK) ,  METLNOD as metlnodFill WITH(NOLOCK) , METLTIP  WITH(NOLOCK) WHERE  relacioFill.RELCDHER LIKE '" + rel.cdher + "_" + rel.infil.ToString() + "%'  and metlnodFill.NODINNOD=relacioFill.RELINFIL  AND relacioFill.RELCDSIT<" + estatSituacio + " AND TIPINTIP=metlnodFill.NODCDTIP   "

                                        End If
                                        strsqlMETLNOD &= " AND metlnodFill.NODCDTIP = " + arrayTCont(estructura(index + subindex + "")) '
                                    Else
                                        strsqlWHERE &= "AND  relacioFill.RELCDEST=" & estructura(index + subindex)
                                    End If
                                Else
                                    strsqlWHERE &= "AND relacioFill.RELCDEST=" + estructura(index + subindex)
                                End If
                            Else
                                strsqlWHERE &= "AND relacioFill.RELCDEST=" + estructura(index + subindex)
                            End If

                            If pathRelacio.Length > 0 Then 'and prefix.trim()<>"n" THEN
                                If pathRelacio <> "0" Then
                                    If arrayTCont.Length > estructura(index + subindex + "") Then
                                        If (prefix.Trim() = "n" Or prefix.Substring(0, 1) = "p" Or prefix.Substring(0, 1) = "f" Or prefix.Substring(0, 1) = "c") And arrayTCont(estructura(index + subindex + "")) <> "9" Then
                                        Else

                                            strsqlMETLNOD &= " and metlnodFill.NODINNOD in (" + pathRelacio.Substring(1).Replace("_", ",") + ")" 'OR TIPDSDES not in ('arbre web','node web'))) "' , 'fulla web')))"				
                                        End If
                                    End If
                                End If
                            End If
                            strSql &= strsqlMETLNOD & strsqlMETLTIP & strsqlMETLREI & strsqlWHERE
                            strSql &= " ORDER BY relacioFill.RELCDORD" 'ordeno per l'ordre de nodes

                        End If
                        GAIA2.bdr(objConn, strSql, DS)

                        If DS.Tables(0).Rows.Count > 0 Then 'Tinc contingut que penja d'aquesta cel·la de l'estructura		
                            cont = 0
                            nrocontingut = 0
                            dbRow = DS.Tables(0).Rows(0)
                            'EN RELCDEST TINC l'estructura a on hi ha el contingut. En cas de ser una plantilla, comprovo el nombre d'elements màxim per representar
                            If prefix.Substring(0, 1) = "p" Or prefix.Substring(0, 1) = "f" Then
                                If dbRow("RELCDEST") >= 0 And arrayPltdsnum.Length > dbRow("RELCDEST") Then
                                    nroMaximContinguts = arrayPltdsnum(dbRow("RELCDEST"))
                                End If
                            End If


                            '***************************************************************************************************+
                            '	Bucle on vaig a buscar cada element de contingut que hi ha dintre de la cel·la.
                            '	Per exemple, dos imatges (que són dos fulles de tipus document) que s'han de representar a dins
                            '***************************************************************************************************+							
                            cont = 0
                            'Aqui podria saltarme contenidos que no estan ligados por celda, pero que en otro caso no se podrían representar porque se muestran en un "detalle" donde la plantilla con la que se asignaron los contenidos no es la misma que la que se está solicitando.... MAX
                            'Habría que detectar esto e intentar recoger los contenidos como se pueda...					
                            'Busco si hi ha algun fill del tipus  arrayTCont(estructura(index+subindex+"") i que RELCDEST sigui > estructura(index+subindex) i que el seu NODCDTIP<> a 	arrayTCont(estructura(index+subindex+""). Això pot pasar quan s'intenti obrir un contingut amb una plantilla no esperada (per exemple per que fem un "detall" del contingut).					
                            Dim salta As Integer
                            salta = 0
                            'bucle pels continguts
                            For Each dbRow In DS.Tables(0).Rows
                                If nroMaximContinguts > 0 And nroMaximContinguts <= nrocontingut Then
                                    Exit For
                                End If
                                If dbRow("REIDTPUB") < Now And (dbRow("REIDTCAD") > Now() Or dbRow("REIDTCAD") = "1/1/1900") Then



                                    If arrayTCont.Length > dbRow("RELCDEST") And dbRow("RELCDEST") >= 0 Then
                                        If arrayTCont(dbRow("RELCDEST")).Length > 0 Then
                                            If dbRow("RELCDEST") = estructura(index + subindex) Then
                                                salta = 0
                                            Else
                                                If (arrayTCont(dbRow("RELCDEST")) = dbRow("NODCDTIP") Or arrayTCont(dbRow("RELCDEST")) = 54) And dbRow("RELCDEST") <> estructura(index + subindex + "") Then
                                                    salta = 1
                                                    'Tinc un contingut que no està despenjat, perque ja està assignat a una cel·la bona. El salto..
                                                End If
                                            End If
                                        End If
                                    End If



                                    If salta = 0 Then

                                        'Si la llibreria de codi té forçarCodi=0 la poso ara i només una vegada.
                                        If forçarCodi = 0 Then
                                            htmlTMP += lcw
                                            lcw = ""
                                        End If
                                        relFill.bdget(objConn, dbRow("RELINCOD"))

                                        If cont = 0 And tplant.Length > estructura(index + subindex) Then

                                            arrayPlantillesPerCelda = tplant(estructura(index + subindex)).Split("|")

                                        End If

                                        If estatCircuit(objConn, relFill, idioma, relIni, dataSimulacio, codiUsuari, publicar, estatSituacio) = 1 Then
                                            If prefix.Substring(0, 1) <> "p" And prefix.Substring(0, 1) <> "f" And (strTplant.Replace(",", "").Trim() + "").Length > 0 Then
                                                nrocontingut += 1
                                                If relFill.inpla = 9 Then ' Plantilles alternatives															
                                                    codiplantillacelda = arrayPlantillesPerCelda(cont Mod arrayPlantillesPerCelda.Length)
                                                Else
                                                    If relFill.inpla <> 0 Then
                                                        codiplantillacelda = relFill.inpla
                                                    Else
                                                        If tplant(estructura(index + subindex)).Trim().Length > 0 Then
                                                            If estructura(index + subindex) <> "" Then
                                                                If IsNumeric(tplant(estructura(index + subindex))) Then
                                                                    codiplantillacelda = Convert.ToInt32(tplant(estructura(index + subindex)))
                                                                Else
                                                                    codiplantillacelda = 0
                                                                End If
                                                            Else
                                                                codiplantillacelda = 0
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                                If codiplantillacelda <> plantilla.innod Then
                                                    plantilla.bdget(objConn, codiplantillacelda)
                                                End If

                                                If plantilla.innod > 0 Then
                                                    If relFill.inpla = 9 And plantilla.swalt = 0 Then
                                                        'salto la plantilla que no està inclosa dins de plantilles alternatives i continuo (només ho utilitzem per actes desconvocats)
                                                        cont = cont + 1
                                                        codiplantillacelda = arrayPlantillesPerCelda(cont Mod arrayPlantillesPerCelda.Length)
                                                        plantilla.bdget(objConn, codiplantillacelda)
                                                    End If
                                                End If
                                                '*********************************************************************************
                                                ' Torno a cridar a dibuixaPreview per representar el contingut de la plantilla
                                                '*********************************************************************************
                                                If plantilla.innod > 0 Then
                                                    If (relFill.tipintip = 3 Or relFill.tipintip = 4 Or relFill.tipintip = 40 Or relFill.tipintip = 45 Or relFill.tipintip = 51 Or relFill.tipintip = 49) Then

                                                        If nrocontingut = 1 Then
                                                            If esEml = "N" Then
                                                                htmlTMP &= "<div class=""llistatContinguts""><ul><li>"
                                                            Else
                                                                'no faig res, treiem llistes de continguts per correu
                                                                'htmlTMP &= "<div style=""margin:0; padding:0; width:100%; float:left; position:relative;	background:transparent;""><ul style=""margin:0; padding:0;	list-style:none; float:left;""><li style=""padding:0; margin:0; float:left;"">"

                                                            End If
                                                        Else
                                                            If esEml = "N" Then
                                                                htmlTMP &= "</li><li>"
                                                            Else
                                                                'no faig res, treiem llistes de continguts per correu
                                                            End If
                                                        End If
                                                    End If
                                                    dibuixaPreview(objConn, plantilla, plantilla.est, plantilla.arrayAtr, plantilla.hor, plantilla.ver, "", strTplant, htmlTMP, css, width, height, "p" + cont.ToString() + estructura(index + subindex), relFill, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, plantilla.css, relIni, dataSimulacio, codiUsuari, autolink, plantilla.plt, heretaPropietatsWeb, plantilla.flw, titol, nivell, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, plantilla.niv)

                                                End If
                                            End If  'prefix.Substring(0, 1) <> "p" And (strTplant.Replace(",", "").Trim() + "").Length > 0
                                            'Tinc contingut que o bé pertany a una part d'una plantilla o bé és un contingut directe (llibreria codi web) o bé és un node web o pàgina web i he de seguir baixant. 
                                            'si el tipus es node o fulla web continuo avançant																								
                                            If relFill.tipdsdes = "node web" Or relFill.tipdsdes = "fulla web" Then
                                                If relFill.tipdsdes = "node web" Or (relFill.tipdsdes = "fulla web" And relFill.incod = relIni.incod) Or (relFill.tipdsdes = "fulla web" And InStr(pathRelacio, relFill.infil) > 0) Then
                                                    'Només obro la fulla que voliem publicar perque podrien haver més d'una fulla web al mateix nivell	
                                                    GAIA2.obreFullaWeb(objConn, relFill, htmlTMP, css, 1, width, height, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, 1, idioma, relIni, dataSimulacio, codiUsuari, titol, nivell, estilbody, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, False)
                                                End If
                                            Else
                                                '******************************************************************
                                                ' no és un node o fulla web i per lo tant ja puc representar el contingut
                                                ' Si hi ha plantilla secundaria i no hi han camps del contigut  intento representarla 
                                                ' si no represento els camps		
                                                '******************************************************************
                                                htmlTMP &= GAIA2.representaCodiWeb(objConn, plantilla, relFill, estructura(index + subindex), idioma, relIni, width, codiUsuari, forçarCodi, css, tagsMeta)
                                                arrayTMPPlantillesSec = plantillesSecundaries.Split(",")
                                                If arrayTMPPlantillesSec.Length > estructura(index + subindex) Then
                                                    If arrayTMPPlantillesSec(estructura(index + subindex)).Trim.Length > 0 Then
                                                        'Tinc una plantilla secundaria amb la que he de representar el contingut que penja de la cel·la
                                                        Dim plantillaTMP As New clsPlantilla
                                                        plantillaTMP.bdget(objConn, arrayTMPPlantillesSec(estructura(index + subindex)))
                                                        If plantillaTMP.innod > 0 Then
                                                            dibuixaPreview(objConn, plantillaTMP, plantillaTMP.est, plantilla.arrayAtr, plantillaTMP.hor, plantillaTMP.ver, "", strTplant, htmlTMP, css, width, height, "p2" + cont.ToString() + estructura(index + subindex), relFill, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, plantillaTMP.css, relIni, dataSimulacio, codiUsuari, autolink, plantillaTMP.plt, heretaPropietatsWeb, plantillaTMP.flw, titol, nivell, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, plantillaTMP.niv)
                                                        End If
                                                    Else
                                                        If prefix.Substring(0, 1) <> "c" Then
                                                            Dim plantillaTMP As New clsPlantilla
                                                            plantillaTMP.copiaObj(plantilla)

                                                            If relFill.cdsit = 96 Or relFill.cdsit = 97 Then
                                                                GAIA2.pintarContingut(objConn, plantillaTMP, rel, estructura(index + subindex), rel.infil, Math.Round(width, 0).ToString(), llistaDocuments, urlDesti, idioma, relIni, 3, fDesti, publicar, heretaPropietatsWeb, codiUsuari, dataSimulacio, htmlTMP, css, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, pareTeEnllac)
                                                            Else
                                                                If relFill.cdsit <> 95 Then 'imatges amagades
                                                                    GAIA2.pintarContingut(objConn, plantillaTMP, relFill, estructura(index + subindex), relFill.infil, Math.Round(width, 0).ToString(), llistaDocuments, urlDesti, idioma, relIni, 3, fDesti, publicar, heretaPropietatsWeb, codiUsuari, dataSimulacio, htmlTMP, css, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, pareTeEnllac)
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                Else
                                                    If prefix.Substring(0, 1) <> "c" And prefix.Substring(0, 1) <> "n" Then
                                                        Dim plantillaTMP As New clsPlantilla
                                                        plantillaTMP.copiaObj(plantilla)
                                                        If (plantillaTMP.arrayCampsPlantilla.Length > estructura(index + subindex)) Then
                                                        Else


                                                        End If

                                                        If relFill.cdsit = 96 Or relFill.cdsit = 97 Then
                                                            GAIA2.pintarContingut(objConn, plantillaTMP, rel, estructura(index + subindex), rel.infil, Math.Round(width, 0).ToString(), llistaDocuments, urlDesti, idioma, relIni, 3, fDesti, publicar, heretaPropietatsWeb, codiUsuari, dataSimulacio, htmlTMP, css, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, pareTeEnllac)
                                                        Else
                                                            If relFill.cdsit <> 95 Then 'imatges amagades
                                                                GAIA2.pintarContingut(objConn, plantillaTMP, relFill, estructura(index + subindex), relFill.infil, Math.Round(width, 0).ToString(), llistaDocuments, urlDesti, idioma, relIni, 3, fDesti, publicar, heretaPropietatsWeb, codiUsuari, dataSimulacio, htmlTMP, css, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, pareTeEnllac)
                                                            End If
                                                        End If
                                                    End If
                                                    'End If
                                                End If
                                            End If

                                            cont = cont + 1
                                        End If 'estatCircuit

                                        'He acabat de representar el contingut,

                                    End If 'salta=0
                                End If
                            Next dbRow

                            'He acabat de posar elements, si hi ha més d'un contingut, faig tractament de llistes




                            If (relFill.tipintip = 3 Or relFill.tipintip = 4 Or relFill.tipintip = 40 Or relFill.tipintip = 45 Or relFill.tipintip = 51 Or relFill.tipintip = 49) Then
                                If nrocontingut <= 1 Then
                                    'només hi ha un contingut, elimino la capçalera de llista
                                    If esEml = "N" Then
                                        htmlTMP = htmlTMP.Replace("<div class=""llistatContinguts""><ul><li>", "")
                                    Else
                                        'no faig res, treiem llistes de continguts per correu
                                        'htmlTMP = htmlTMP.Replace("<div style=""margin:0; padding:0; width:100%; float:left; position:relative;	background:transparent;""><ul style=""margin:0; padding:0;	list-style:none; float:left;""><li style=""padding:0; margin:0; float:left;"">", "")
                                    End If
                                Else 'nrocontingut>1
                                    If esEml = "N" Then
                                        htmlTMP &= "</li></ul></div>"
                                    Else
                                        'no faig res, treiem llistes de continguts per correu
                                    End If

                                End If
                            End If






                        Else    'No hi ha més fills a representar, per tant la info està al propi node actual
                            If estatCircuit(objConn, rel, idioma, relIni, dataSimulacio, codiUsuari, 0, estatSituacio) = 1 Then

                                Dim plantillaTMP As New clsPlantilla
                                plantillaTMP.copiaObj(plantilla)
                                GAIA2.pintarContingut(objConn, plantillaTMP, rel, estructura(index + subindex), rel.infil, Math.Round(width, 0).ToString(), llistaDocuments, urlDesti, idioma, relIni, 3, fDesti, publicar, heretaPropietatsWeb, codiUsuari, dataSimulacio, htmlTMP, css, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, pareTeEnllac)


                            End If
                            If arrayTCont.Length >= estructura(index + subindex) + 1 And prefix.Substring(0) <> "p" And prefix.Substring(0) <> "f" Then 'and prefix.substring(0)<>"c" THEN 
                                If (arrayTCont(estructura(index + subindex + "")) + "").Length > 0 Then
                                    If rel.incod <> 82115 Then
                                        Select Case arrayTCont(estructura(index + subindex + ""))
                                            'Documents, noticies i agenda
                                            Case 45, 4, 5
                                                If arrayCND.Length > estructura(index + subindex + "") Then
                                                    If arrayCND(estructura(index + subindex + "")) = "1" Then
                                                        Dim strNoInfo As String = ""
                                                        Select Case idioma
                                                            Case 1
                                                                strNoInfo = "No hi ha informaci&oacute; disponible"
                                                            Case 2
                                                                strNoInfo = "No hay informaci&oacute;n disponible"
                                                            Case 3
                                                                strNoInfo = "There is not related information"
                                                            Case 4
                                                                strNoInfo = "Il n'y a aucune information li&eacute;e &aacute;"
                                                        End Select
                                                        htmlTMP += "<div class=""missatgeNoInfo"">" & strNoInfo & "</div>"

                                                    End If
                                                End If
                                        End Select
                                    End If
                                End If
                            End If
                        End If      'IF ds.Tables(0).Rows.count>0 		

                        htmlNou &= htmlTMP

                        'guardo el html resultant en METLCEL
                        If rel.incod = relIni.incod Then
                            Try

                                If plantilla.innod <> 0 Then
                                    Try

                                        Dim datacaducitat As DateTime
                                        'Per tots els continguts, busco el que caduca primer per donar la data màxima de validessa de la cel·la
                                        GAIA2.bdr(objConn, "select top 1 (CASE WHEN (REIDTCAD>GETDATE() AND REIDTPUB>GETDATE()) THEN CASE WHEN  REIDTCAD<REIDTPUB THEN REIDTCAD ELSE REIDTPUB END  ELSE CASE WHEN  REIDTCAD>GETDATE() THEN REIDTCAD ELSE CASE WHEN  REIDTPUB> getdate() THEN REIDTPUB ELSE GETDATE() END  END  END)  as data FROM METLREI WITH(NOLOCK), METLREL  WITH(NOLOCK) WHERE RELCDHER LIKE '%" & relIni.infil & "%' AND REIINCOD=RELINCOD AND RELCDSIT<98 AND REIDTCAD>'1/1/1900' ORDER BY data", DS)



                                        If DS.Tables(0).Rows.Count > 0 Then
                                            datacaducitat = DS.Tables(0).Rows(0)("data")
                                        Else
                                            datacaducitat = "1/1/1900"
                                        End If


                                        GAIA2.bdSR(objConn, " IF NOT EXISTS (SELECT CELCDNOD FROM METLCEL WITH(NOLOCK) WHERE CELINPAR='contingutCella?codirelacio=" & relIni.incod & "&codiIdioma=" & idioma & "&codiRelacioInicial=" & relIni.incod & "&est=" & estructura(index + subindex) & "&width=" & width & "&node=" & rel.infil & "&us=" & codiUsuari & "&codiPlantilla=" & plantillaOriginal.innod & "')   BEGIN INSERT INTO METLCEL VALUES (0," & idioma & "," & relIni.incod & "," & rel.incod & "," & estructura(index + subindex) & "," & codiUsuari & "," & rel.infil & "," & plantilla.innod & ",getdate(),'" & datacaducitat & "','" & css.Replace("<style type=""text/css""><!--", "") & "','" & htmlTMP.Replace("'", "''") & "','contingutCella?codirelacio=" & relIni.incod & "&codiIdioma=" & idioma & "&codiRelacioInicial=" & relIni.incod & "&est=" & estructura(index + subindex) & "&width=" & width & "&node=" & rel.infil & "&us=" & codiUsuari & "&codiPlantilla=" & plantillaOriginal.innod & "') END ")
                                    Catch
                                    End Try
                                End If
                            Catch
                                ' si no pot fer l'insert és per que ja s'ha creat des d'un altre procés.. i no cal fer-ho ni tractar l'error.
                            End Try
                        End If
                    End If


                Else 'codiRelacio=0
                    '***************************************************

                    ' només volem pintar l'estructura
                    '***************************************************
                    'css+=" #"+prefix+index+subindex+"-"+codiRelacio.toString()+" {	word-wrap: break-word;double: left;margin: 0px;padding: 0px;background-color: RGB("+r.NEXT(255).toString()+","+r.NEXT(255).toString()+","+r.NEXT(255).toString()+");width: "+ width.toString() +"px;height:"+ height.toString() +"px;border-width:10px;border-color:#000000;} "			
                    css += " #" + prefix + index + subindex + "-" + codiRelacio.ToString() + " {	float: left;margin: 0px;padding: 0px;background-color: RGB(" + r.Next(255).ToString() + "," + r.Next(255).ToString() + "," + r.Next(255).ToString() + ");width: " + width.ToString() + "%;height:" + height.ToString() + "%;border-width:10px;border-color:#000000;} "
                    If prefix.Substring(0) <> "p" And prefix.Substring(0) <> "f" Then    'si hi ha una plantilla associada la pinto dintre										
                        If tplant(estructura(index + subindex)).Trim().Length > 0 Then
                            codiplantillacelda = Convert.ToInt32(tplant(estructura(index + subindex)))
                        End If
                        plantilla.bdget(objConn, codiplantillacelda)
                        '*********************************************************************************
                        ' Torno a cridar a dibuixaPreview per representar el contingut de la plantilla
                        '*********************************************************************************
                        If DS2.Tables(0).Rows.Count > 0 Then
                            dbRow2 = DS2.Tables(0).Rows(0)
                            dibuixaPreview(objConn, plantilla, plantilla.est, plantilla.arrayAtr, plantilla.hor, plantilla.ver, "", strTplant, htmlNou, css, width, height, "p" + cont.ToString() + estructura(index + subindex), relFill, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, "", relIni, dataSimulacio, codiUsuari, autolink, plantilla.plt, heretaPropietatsWeb, plantilla.flw, titol, nivell, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, plantilla.niv)
                            nrocontingut += 1
                        End If
                    End If
                End If  'codiRelacio>0		


            Else
                If prefix.Substring(0) = "p" Or prefix.Substring(0) = "f" Then
                    GAIA2.pintarContingut(objConn, plantilla, rel, estructura(index + subindex), rel.infil, Math.Round(width, 0).ToString(), llistaDocuments, urlDesti, idioma, relIni, 3, fDesti, publicar, heretaPropietatsWeb, codiUsuari, dataSimulacio, htmlNou, css, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, pareTeEnllac)
                End If
            End If   'IF NOT estructura.Contains(index+subindex+"1")

            fesFlow1 = 0
            fesflow2 = 0
            If estilFLW <> "" And estilFLW <> "9" Then 'valor inicial que guardo per compatibilitat amb versió anterior				
                If arrayFLW(estructura(index + "1")).Trim() = "1" Then
                    fesFlow1 = 1
                End If
                If arrayFLW(estructura(index + "2")).Trim() = "1" Then
                    fesflow2 = 1
                End If
            End If
            'MSV9
            'css+=" #"+prefix+index+subindex+"-"+codiRelacio.tostring()+"{"+stil+"}"			
            If width <> 0 And stil <> "" Then
                If InStr(css, "wpx" + width.ToString() + "{" + stil) <= 0 Then
                    css += " .wpx" + width.ToString() + "{" + stil + "}"

                End If
            End If
            '***************************************************
            'Continuo avançant recursivament cap els fills
            '***************************************************
            If prefix <> "p" And prefix <> "f" Then 'si no estic representant una plantilla, netejo l'objecte
                plantilla = New clsPlantilla

            End If
            If estilFLW <> "9" And estilFLW.Length > 0 Then
                If arrayFLW(estructura(index + subindex + "1")).Trim() = "1" And Not arrayFLW(estructura(index + subindex + "2")).Trim() = "1" Then
                    dibuixaPreviewRec(objConn, plantilla, index + subindex, "2", estructura, formaEstructura, htmlNou, css, thor, tver, r, width, height, strTplant, prefix, rel, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, estilCSS, relIni, dataSimulacio, codiUsuari, autolink, tcont, plantillesSecundaries, heretaPropietatsWeb, 0, estilFLW, titol, nivell, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, llistaNivellsTextos, tancavoreres, pareTeEnllac)
                    dibuixaPreviewRec(objConn, plantilla, index + subindex, "1", estructura, formaEstructura, htmlNou, css, thor, tver, r, width, height, strTplant, prefix, rel, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, estilCSS, relIni, dataSimulacio, codiUsuari, autolink, tcont, plantillesSecundaries, heretaPropietatsWeb, 0, estilFLW, titol, nivell, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, llistaNivellsTextos, tancavoreres, pareTeEnllac)
                Else
                    dibuixaPreviewRec(objConn, plantilla, index + subindex, "1", estructura, formaEstructura, htmlNou, css, thor, tver, r, width, height, strTplant, prefix, rel, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, estilCSS, relIni, dataSimulacio, codiUsuari, autolink, tcont, plantillesSecundaries, heretaPropietatsWeb, 0, estilFLW, titol, nivell, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, llistaNivellsTextos, tancavoreres, pareTeEnllac)
                    dibuixaPreviewRec(objConn, plantilla, index + subindex, "2", estructura, formaEstructura, htmlNou, css, thor, tver, r, width, height, strTplant, prefix, rel, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, estilCSS, relIni, dataSimulacio, codiUsuari, autolink, tcont, plantillesSecundaries, heretaPropietatsWeb, 0, estilFLW, titol, nivell, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, llistaNivellsTextos, tancavoreres, pareTeEnllac)
                End If
            Else
                dibuixaPreviewRec(objConn, plantilla, index + subindex, "1", estructura, formaEstructura, htmlNou, css, thor, tver, r, width, height, strTplant, prefix, rel, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, estilCSS, relIni, dataSimulacio, codiUsuari, autolink, tcont, plantillesSecundaries, heretaPropietatsWeb, 0, estilFLW, titol, nivell, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, llistaNivellsTextos, tancavoreres, pareTeEnllac)
                dibuixaPreviewRec(objConn, plantilla, index + subindex, "2", estructura, formaEstructura, htmlNou, css, thor, tver, r, width, height, strTplant, prefix, rel, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, estilCSS, relIni, dataSimulacio, codiUsuari, autolink, tcont, plantillesSecundaries, heretaPropietatsWeb, 0, estilFLW, titol, nivell, estilbody, llistaCND, esForm, esEml, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, llistaNivellsTextos, tancavoreres, pareTeEnllac)
            End If

            If esEml = "N" Then

                If posaVoreres <> "" And (htmlNou.Trim.Length <> 0 Or (index + subindex = "")) Then
                    html &= "<div class=""" & posaVoreres & """><div class=""topBloc""></div><div class=""contBloc"">"
                End If
                If htmlNou.Trim().Length <> 0 Then
                    'He de fer el tractament de les vores arrodonides !			
                    html &= htmlIni & htmlNou
                    If celdaInicial = 0 Then
                        html &= IIf(Hnivell = "0", "</div>", "</h" & Hnivell & ">")
                    End If
                    If posaVoreres <> "" Then
                        If celdaInicial <> 0 Then
                            html &= "</div></div><div class=""botBloc""></div>"
                        Else
                            html &= "</div><div class=""botBloc""></div></div>"
                        End If
                    End If
                Else
                    If index + subindex = "" Then
                        html &= htmlIni
                        If posaVoreres <> "" Then
                            tancavoreres = tancavoreres + 1
                        End If
                    End If

                End If
                posaVoreres = ""
            Else

                html &= htmlIni


                Dim strTmp2 As String = ""

                If InStr(strCSS, "taulaValignBottom") Then
                    strCSS = strCSS.Replace("taulaValignBottom", "")
                    strTmp2 = " valign=""bottom"" "
                End If

                If InStr(strCSS, "taulaValignMiddle") Then
                    strCSS = strCSS.Replace("taulaValignMiddle", "")
                    strTmp2 = " valign=""middle"" "
                End If

                If InStr(strCSS, "taulaValignTop") Then
                    strCSS = strCSS.Replace("taulaValignTop", "")
                    strTmp2 = " valign=""top"" "
                End If
                If prefix <> "f" Then
                    If html.EndsWith("<td>") Then
                        html = html.Substring(0, html.Length - 1) & " width=""" & width & """  " & strCSS & strTmp2 & " >"
                    Else
                        'obro bloc
                        If subindex = "1" Then
                            html &= "<table width=""" & width & """  " & strCSS & "><tr><td" & strTmp2 & ">"
                        Else
                            html &= "<td width=""" & width & """  " & strCSS & strTmp2 & ">"
                        End If

                    End If
                End If



                If htmlNou.Trim().Length <> 0 Then
                    html &= htmlNou
                End If


                If prefix <> "f" Then
                    'tanco bloc
                    If celdaInicial = 0 Then
                        Select Case formaEstructura(estructura(index + subindex))
                            Case "iv"
                                html &= "</td>"
                            Case "v"
                                html &= "</td></tr></table>"
                            Case "ih"

                                html &= "</td></tr><tr><td>"

                            Case "h"
                                html &= "</td></tr></table>"
                        End Select
                    End If


                End If



            End If
            dibuixaPreviewRec = nroFillsSenars + nroFillsParells

            DS.Dispose()
            DS2.Dispose()
        Else
            dibuixaPreviewRec = 0
        End If       'estructura.Contains(index+subindex) 


    End Function 'dibuixaPreviewRec

    Public Shared Sub datesPublicacio(ByVal objconn As OleDbConnection, ByVal rel As clsRelacio, ByVal idioma As Integer, ByRef dataIni As DateTime, ByRef dataFi As DateTime)
        Dim sSQL As String
        Dim DS As DataSet

        Dim taula As String = ""
        Dim prefixCamp As String = ""


        Dim dataIniRel, dataIniContingut, dataFiRel, dataFiContingut As DateTime
        dataIniRel = "1/1/2050"
        dataFiRel = "1/1/2050"

        If rel.cdsit = 97 Or rel.cdsit = 96 Or rel.cdsit = 95 Then
            dataIni = CDate("01/01/1900")
            dataFi = CDate("01/01/2050")
        Else
            sSQL = "SELECT REIDTPUB, REIDTCAD FROM METLREI WITH(NOLOCK)  WHERE REIINCOD = " & rel.incod & "	AND REIINIDI = " & idioma
            DS = New DataSet()
            GAIA2.bdr(objconn, sSQL, DS)
            If DS.Tables(0).Rows.Count > 0 Then
                dataIniRel = IIf(IsDBNull(DS.Tables(0).Rows(0).Item("REIDTPUB")), CDate("01/01/1900"), DS.Tables(0).Rows(0).Item("REIDTPUB"))
                dataFiRel = IIf(IsDBNull(DS.Tables(0).Rows(0).Item("REIDTCAD")), CDate("01/01/2050"), DS.Tables(0).Rows(0).Item("REIDTCAD"))
                DS.Dispose()
            End If
            GAIA2.datesPublicacioPerNode(objconn, rel.infil, idioma, dataIniContingut, dataFiContingut)


            If dataIniRel < dataIniContingut Then
                dataIni = dataIniRel
            Else
                dataIni = dataIniContingut
            End If

            If dataFiRel < dataFiContingut Then
                dataFi = dataFiRel
            Else
                dataFi = dataFiContingut
            End If
        End If
    End Sub 'dates publicacio


    Public Shared Sub datesPublicacioPerNode(ByVal objconn As OleDbConnection, ByVal codiNode As Integer, ByVal idioma As Integer, ByRef dataIni As DateTime, ByRef dataFi As DateTime)

        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim taula As String = ""
        Dim prefixCamp As String = ""
        DS = New DataSet()
        'cerco dins de la taula on hi ha el contingut les dates de caducitat i publicació
        If idioma = 99 Or idioma = 0 Then idioma = 1
        GAIA2.bdr(objconn, "SELECT TBLDSTXT FROM METLNOD WITH(NOLOCK) , METLTBL WITH(NOLOCK)  WHERE NODINNOD=" & codiNode.ToString() & " AND  NODCDTIP=TBLINTFU AND TBLINTFU<>9  ", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            taula = dbRow("TBLDSTXT")
        End If
        dataIni = CDate("01/01/1900")
        dataFi = CDate("01/01/2100")
        If taula <> "METLNWE" And taula <> "METLFOR" Then
            If taula.Length > 0 Then

                Try
                    prefixCamp = taula.Substring(4).Trim()
                    GAIA2.bdr(objconn, "SELECT  * FROM  " + taula.ToString() + " WITH(NOLOCK)  WHERE " + prefixCamp.ToString() + "INNOD=" + codiNode.ToString() + " AND " + prefixCamp.ToString() + "INIDI=" + idioma.ToString(), DS)
                    If DS.Tables(0).Rows.Count > 0 Then
                        dbRow = DS.Tables(0).Rows(0)
                        dataIni = CDate(dbRow(prefixCamp.ToString() + "DTPUB"))
                        If IsDBNull(dbRow(prefixCamp.ToString() + "DTCAD")) Then
                            dataFi = CDate("01/01/2050")
                        Else
                            dataFi = CDate(dbRow(prefixCamp.ToString() + "DTCAD"))
                        End If

                    End If
                Catch
                    'la taula no te els camps dates de publicacio/caducitat i retorno 2100/1900
                End Try

            End If

        End If
        DS.Dispose()
    End Sub 'dates publicacio


    '************************************************************************************************************
    '	Funció: GAIA.obreFullaWeb
    '	Entrada:  
    '					codiRelacio: relació on el fill apunta al node que volem obrir.
    '					html: codi html que vaig generant
    '					css: estils que vaig generant
    '					heretaPropietatsWeb: si 1 buscaré si hi han propietats per heretar als nodes superiors (arbre web o node web)
    '					tamanyHoritzontalTotal: Espai horitzontal màxim per representar el contingut
    '					tamanyVerticalTotal: Espai Vertical màxim per representar el contingut
    '					pathRelacio: Path amb tots els nodes (en format _node1_node2_..._nodeN-1_nodeN que defineixen el que cal per obrir la fulla
    '					fDesti: Fitxer destí on es gravarà la pàgina (path sencer: \\servidor\disc\carpeta1\...\carpetaN\nomFitxer.aspx)'									
    '					URLDesti: adreça on es podrà obrir la pàgina web una vegada creat 
    '					relacioInicial: codi de la relació que ha iniciat el procés
    '					dataSimulacio : Data en la que volem veure si el contingut és publicable
    '					codiUsuari: Codi de l'usuari peticionari de l'acció
    '	Procés: 
    '					Funció que prepara la crida a les funcions de representar la pàgina web
    '					Si es desitja heretar les propietats dels nodes superiors recorrerà els nodes desde el node arbre cap avall.
    '					A les variables html i css retornarà el contingut de la pàgina.
    '************************************************************************************************************				

    Public Shared Sub obreFullaWeb(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByRef html As String, ByRef css As String, ByVal heretaPropietatsWeb As Boolean, ByVal tamanyHoritzontalTotal As Double, ByVal tamanyVerticalTotal As Double, ByRef pathRelacio As String, ByRef fDesti As String, ByRef urlDesti As String, ByRef nrocontingut As Integer, ByRef llistaDocuments As String(), ByVal publicar As Integer, ByVal idioma As Integer, ByVal relIni As clsRelacio, ByVal dataSimulacio As DateTime, ByVal codiUsuari As Integer, ByRef titol As String, ByVal nivell As Integer, ByRef estilbody As String, ByRef esForm As String, ByRef esEML As String, ByRef esSSL As String, ByRef tagsMeta As String, ByRef htmlPeu As String, ByRef strCSSPantalla As String, ByRef strCSSImpressora As String, ByVal iteracioInicial As Boolean)
        Dim strErr As String = ""
        Try
            Dim temps As Date = Now

            Dim nopublicar As Integer = 0
            Dim DS As DataSet
            Dim dbRow As DataRow
            Dim nomFitxerPublicat As String
            Dim arrayPathRelacio As String()
            Dim cont, codiRelacio, relacioInicial As Integer
            Dim descTipus, sql As String
            Dim tipusIni As String = ""
            Dim relTmp As New clsRelacio
            sql = ""
            descTipus = ""
            DS = New DataSet()
            codiRelacio = rel.incod
            relacioInicial = relIni.incod
            If esEML = "" Then
                esEML = "N"
            End If
            If esSSL = "" Then
                esSSL = "N"
            End If
            Dim plantilla As New clsPlantilla

            Dim estatSituacio As String = "98"
            If dataSimulacio <> CDate("01/01/1900") Then
                If Day(Now) & Month(Now) & Year(Now) > Day(dataSimulacio) & Month(dataSimulacio) & Year(dataSimulacio) Then
                    'Si la data de simulació es diferent al dia d'avui buscaré també continguts caducats.Per ara no tindré en compte la hora. (30/11/06-MAX)
                    estatSituacio = "99"
                End If
            End If

            '  titol=""

            If heretaPropietatsWeb Then
                strErr = "1"
                If pathRelacio.Length = 0 And iteracioInicial Then 'Encara no tinc la llista de nodes que defineixen les propietats de la pàgina i els busco										
                    pathRelacio = rel.cdher + "_" + rel.infil.ToString()
                End If
                If pathRelacio.Length > 0 Then

                    'Elimino de pathRelacio tots els nodes de tipus "fulla web" intermitges		
                    If pathRelacio = "_55785_109214_109158_109267" Then
                        pathRelacio = "_55785_109214_109267"
                    End If

                    arrayPathRelacio = Split(pathRelacio.Substring(1), "_")
                    '  Faig una neteja de la llista de nodes de la relació per obtenir només els de tipus "arbre web", 
                    ' 	node web" i finalment "fulla web"
                    '  Busco el primer node de tipus "arbre web", si no trobo cap, busco el primer node "node web", 
                    '  si no trobo cap, represento la pàgina web i prou.


                    For cont = 0 To arrayPathRelacio.Length - 2
                        GAIA2.tipusNodebyNro(objConn, arrayPathRelacio(cont), descTipus)
                        If sql = "" Then
                            Select Case descTipus
                                Case "arbre web"
                                    sql = "SELECT * FROM METLREL WITH(NOLOCK) ,METLAWE WITH(NOLOCK) , METLSER WITH(NOLOCK)  WHERE RELINFIL=RELINPAR AND RELINFIL=" + arrayPathRelacio(cont).ToString() + " AND RELINPAR = AWEINNOD AND RELCDSIT<" + estatSituacio + " AND AWEDSSER = SERINCOD"
                                    tipusIni = "arbre web"
                                Case "node web"
                                    sql = "SELECT * FROM METLREL WITH(NOLOCK) ,METLNWE  WITH(NOLOCK)  WHERE RELINCOD=" & codiRelacio & " AND  RELINFIL=" & arrayPathRelacio(cont) & " AND RELINFIL = NWEINNOD AND RELCDSIT<" & estatSituacio & " AND NWEINIDI=" & idioma

                                    tipusIni = "node web"
                                Case "fulla web"
                                    ' LES PROPIETATS NO S'HERETEN PER ARA, continuo el bucle fins la darrera pàgina del camí
                            End Select
                        End If
                    Next cont

                    If sql = "" Then

                        sql = "SELECT WEBDSATR,WEBSWSSL, WEBTPBUS,WEBDSDES,  WEBDSPCL, WEBDSTIT,WEBDSEST, WEBDSTHOR, WEBDSTVER, WEBDSTCO, WEBDSPLA, WEBDSFIT,WEBDSURL, WEBDSCSS, WEBDSCND, WEBSWFRM,  WEBSWEML, WEBDSEBO,	RELINCOD FROM METLWEB  WITH(NOLOCK) , METLREL WITH(NOLOCK)  WHERE RELINCOD=" & relacioInicial.ToString() & " AND RELINFIL=WEBINNOD AND WEBINIDI=" + idioma.ToString() + " AND RELCDSIT<" + estatSituacio
                    End If


                    GAIA2.bdr(objConn, sql, DS)
                    If DS.Tables(0).Rows.Count = 0 Then
                        If tipusIni = "node web" Then
                            sql = "SELECT * FROM METLREL  WITH(NOLOCK) ,METLNWE WITH(NOLOCK)  WHERE RELINCOD=" & codiRelacio & " AND RELINFIL = NWEINNOD AND RELCDSIT<" & estatSituacio & " AND NWEINIDI=1"
                            GAIA2.bdr(objConn, sql, DS)
                        Else
                            sql = "SELECT WEBDSATR,WEBSWSSL, WEBTPBUS,WEBDSDES,  WEBDSPCL,  WEBDSTIT,WEBDSEST, WEBDSTHOR, WEBDSTVER, WEBDSTCO, WEBDSPLA, WEBDSFIT,WEBDSURL, WEBDSCSS, WEBDSCND, WEBSWFRM,WEBSWEML, WEBDSEBO, RELINCOD FROM METLWEB WITH(NOLOCK) , METLREL WITH(NOLOCK)  WHERE RELINCOD=" & relacioInicial & " AND RELINFIL=WEBINNOD AND WEBINIDI=1 AND RELCDSIT<" & estatSituacio
                            GAIA2.bdr(objConn, sql, DS)
                            If DS.Tables(0).Rows.Count > 0 Then
                                dbRow = DS.Tables(0).Rows(0)
                                ' si trobo una versió en un altre idioma i té una adreça fixa, no la publico.
                                If dbRow("WEBDSURL") <> "" Then
                                    nopublicar = 1
                                End If
                            End If

                        End If
                    End If
                    If nopublicar = 0 Then
                        If DS.Tables(0).Rows.Count > 0 Then

                            dbRow = DS.Tables(0).Rows(0)
                            'Trec el primer element de la llista
                            If InStr(pathRelacio.Substring(1), "_") = 0 Then
                                pathRelacio = ""
                            Else
                                pathRelacio = pathRelacio.Substring(InStr(pathRelacio.Substring(1), "_"))
                            End If





                            Select Case tipusIni
                                Case "arbre web"
                                    'Continuo representant				
                                    titol = dbRow("AWEDSTIT")
                                    fDesti += dbRow("AWEDSROT").Trim()
                                    estilbody += " " + dbRow("AWEDSEBO")
                                    strCSSPantalla += " " + dbRow("AWEDSCSP")
                                    strCSSImpressora += " " + dbRow("AWEDSCSI")
                                    'tagsMeta +=  dbRow("AWEDSPEU") & dbRow("AWEDSMET")
                                    tagsMeta += dbRow("AWEDSMET")

                                    htmlPeu += dbRow("AWEDSPEU")
                                    urlDesti = dbRow("SERDSURL").Trim()
                                    If dbRow("SERDSPRT") <> "80" Then
                                        urlDesti += ":" + dbRow("SERDSPRT").ToString()
                                    End If
                                    urlDesti += dbRow("AWEDSROT").Replace("\", "/")
                                    relTmp.bdget(objConn, dbRow("RELINCOD"))

                                    GAIA2.dibuixaPreview(objConn, plantilla, dbRow("AWEDSEST"), dbRow("AWEDSATR").split(","), dbRow("AWEDSTHOR"), dbRow("AWEDSTVER"), dbRow("AWEDSTCO"), "", html, css, dbRow("AWEDSHOR"), dbRow("AWEDSVER"), "e", relTmp, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, "", relIni, dataSimulacio, codiUsuari, False, "", heretaPropietatsWeb, "", titol, nivell, estilbody, "", esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, "")
                                Case "node web"
                                    If Not TypeOf dbRow("NWEINNOD") Is DBNull Then
                                        Dim estilsCSS As String
                                        'Faig una excepció. Si la relació superior és el node "webs municipals" poso como a títol el nom del node per que és un web que no té arbre propi
                                        If rel.cdrsu = 42234 Then
                                            titol = dbRow("NWEDSTIT")
                                        End If
                                        estilbody += " " + dbRow("NWEDSEBO")
                                        If dbRow("NWEDSCSP").Trim().length > 0 Then
                                            If strCSSPantalla.Trim().Length > 0 Then
                                                strCSSPantalla += ";"
                                            End If
                                            strCSSPantalla += dbRow("NWEDSCSP").Trim()
                                        End If
                                        If dbRow("NWEDSCSI").Trim().length > 0 Then
                                            If strCSSImpressora.Trim().Length > 0 Then
                                                strCSSImpressora += ";"
                                            End If
                                            strCSSImpressora += dbRow("NWEDSCSI").Trim()
                                        End If

                                        tagsMeta &= dbRow("NWEDSMET")

                                        'Si hi ha un htmlPeu d'un node que té informació sobre google-analytics i també el htmlpeu heretat ho té, em quedo només amb la nova info.
                                        If InStr(htmlPeu, "google-analytics") > 0 And InStr(dbRow("NWEDSPEU"), "google-analytics") > 0 Then
                                            htmlPeu = dbRow("NWEDSPEU")
                                        Else
                                            htmlPeu = dbRow("NWEDSPEU") + htmlPeu
                                        End If
                                        'Continuo representant informació						
                                        If Not TypeOf dbRow("NWEDSCAR") Is DBNull Then
                                            If dbRow("NWEDSCAR").length > 0 Then
                                                fDesti &= "/" & dbRow("NWEDSCAR").Trim()
                                                urlDesti &= "/" & dbRow("NWEDSCAR").Trim().Replace("\", "/")
                                            End If
                                        End If
                                        relTmp.bdget(objConn, dbRow("RELINCOD"))
                                        If Not TypeOf dbRow("NWEDSCSS") Is DBNull Then
                                            estilsCSS = dbRow("NWEDSCSS")
                                        Else
                                            estilsCSS = ""
                                        End If

                                        GAIA2.dibuixaPreview(objConn, plantilla, dbRow("NWEDSEST"), dbRow("NWEDSATR").split(","), dbRow("NWEDSTHOR"), dbRow("NWEDSTVER"), dbRow("NWEDSTCO"), dbRow("NWEDSPLA"), html, css, tamanyHoritzontalTotal, tamanyVerticalTotal, "n", relTmp, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, estilsCSS, relIni, dataSimulacio, codiUsuari, False, "", 1, "", titol, nivell, estilbody, "", esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, "")
                                    End If

                                Case Else 'fulla web
                                    strErr &= "2"
                                    'Quan trobo una fulla web vaig directament a la relació inicial que ha provocat la publicació. Això ho faig per saltar
                                    'totes les fulles web intermitges i anar directament a l'última.
                                    'Si es poden heretar les propietats d'altres fulles web això s'haurà de refer.
                                    If InStr(titol, dbRow("WEBDSTIT")) <= 0 And InStr(titol, "|") <= 0 Then
                                        titol = dbRow("WEBDSTIT") + " | " + titol
                                    End If
                                    estilbody += " " + dbRow("WEBDSEBO")
                                    strErr &= "3"
                                    If dbRow("WEBDSFIT").ToString().Length > 0 Then
                                        Dim strTmp As String = ""
                                        'potser a fdesti ja hi ha un troç del path que es repeteix amb webdsfit. Intento corregir aquí l'error.
                                        strErr &= "4"
                                        strTmp = dbRow("WEBDSFIT")
                                        strErr &= "5"

                                        If Not String.IsNullOrEmpty(fDesti) Then
                                            strTmp = strTmp.Replace(fDesti, "")
                                        End If
                                        strErr &= "6"

                                        fDesti &= "/" & strTmp
                                        'En el cas de ser una publicació per intranet, amb nom fix, he de treure el /gdocs
                                        fDesti = fDesti.Replace("/gdocs", "").Replace("//", "/")

                                        urlDesti = dbRow("WEBDSURL").Trim()
                                        strErr &= "5"
                                    Else
                                        fDesti += "/" + relacioInicial.ToString() + "_" + idioma.ToString() + ".aspx"
                                        urlDesti += "/" + relacioInicial.ToString() + "_" + idioma.ToString() + ".aspx"
                                    End If
                                    If esForm <> "S" Then
                                        esForm = dbRow("WEBSWFRM")
                                    End If
                                    If esEML <> "S" Then
                                        esEML = dbRow("WEBSWEML")
                                    End If
                                    If esSSL <> "S" Then
                                        esSSL = dbRow("WEBSWSSL")
                                    End If

                                    If dbRow("WEBDSDES").length > 0 Then
                                        tagsMeta += "<meta name=""DC.Description"" content=""" + dbRow("WEBDSDES") + """/>"
                                        tagsMeta += "<meta name=""Description"" content=""" + dbRow("WEBDSDES") + """/>"
                                    End If

                                    If dbRow("WEBDSPCL").length > 0 Then
                                        tagsMeta += "<meta name=""keywords"" content=""" + dbRow("WEBDSPCL") + """/>"
                                        tagsMeta += "<meta name=""DC.subject"" content=""" + dbRow("WEBDSPCL") + """/>"
                                    End If
                                    If dbRow("WEBTPBUS") = "S" Then
                                        tagsMeta += "<meta name=""robots"" content=""INDEX,FOLLOW""/>"
                                    Else
                                        tagsMeta += "<meta name=""robots"" content=""NOINDEX,NOFOLLOW""/>"
                                    End If


                                    GAIA2.dibuixaPreview(objConn, plantilla, dbRow("WEBDSEST"), dbRow("WEBDSATR").split(","), dbRow("WEBDSTHOR"), dbRow("WEBDSTVER"), dbRow("WEBDSTCO"), dbRow("WEBDSPLA"), html, css, tamanyHoritzontalTotal, tamanyVerticalTotal, "c", relIni, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, dbRow("WEBDSCSS"), relIni, dataSimulacio, codiUsuari, False, "", heretaPropietatsWeb, "", titol, nivell, estilbody, dbRow("WEBDSCND"), esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, "")
                            End Select
                        End If
                    End If
                End If 'pathRelacio.length>0
            Else 'No heretem les propietats dels nodes superiors

                sql = "SELECT  WEBWNMTH,WEBDSATR,WEBSWSSL, WEBDSFIT, WEBDSURL, WEBDSEST, WEBDSTHOR, WEBDSTVER, WEBDSTCO, WEBDSPLA, WEBDSCSS, WEBDSCND,WEBSWFRM, WEBSWEML, WEBDSEBO FROM METLWEB  WITH(NOLOCK) , METLREL  WITH(NOLOCK) WHERE RELINCOD=" & codiRelacio.ToString() & " AND RELINFIL=WEBINNOD AND WEBINIDI=" + idioma.ToString() + " AND RELCDSIT<" + estatSituacio
                GAIA2.bdr(objConn, sql, DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    'IF publicar THEN	
                    estilbody = dbRow("WEBDSEBO")
                    If dbRow("WEBDSFIT").ToString().Length > 0 Then
                        fDesti = dbRow("WEBDSFIT")
                        urlDesti = dbRow("WEBDSURL")
                    Else
                        nomFitxerPublicat = rel.dsfit
                        'IF nomFitxerPublicat.length>0 THEN
                        'fDesti = nomFitxerPublicat
                        'urlDesti= nomFitxerPublicat
                        'ELSE
                        fDesti += "/" + codiRelacio.ToString() + "_" + idioma.ToString() + ".aspx"
                        urlDesti += "/" + codiRelacio.ToString() + "_" + idioma.ToString() + ".aspx"

                        'END IF
                    End If
                    'END IF					

                    If esForm <> "S" Then
                        esForm = dbRow("WEBSWFRM")
                    End If
                    If esEML <> "S" Then
                        esEML = dbRow("WEBSWEML")
                    End If
                    If esSSL <> "S" Then
                        esSSL = dbRow("WEBSWSSL")
                    End If
                    GAIA2.dibuixaPreview(objConn, plantilla, dbRow("WEBDSEST"), dbRow("WEBDSATR").split(","), dbRow("WEBDSTHOR"), dbRow("WEBDSTVER"), dbRow("WEBDSTCO"), dbRow("WEBDSPLA"), html, css, dbRow("WEBWNMTH"), tamanyVerticalTotal, "c", rel, pathRelacio, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, dbRow("WEBDSCSS"), relIni, dataSimulacio, codiUsuari, False, "", heretaPropietatsWeb, "", titol, nivell, estilbody, dbRow("WEBDSCND"), esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, "")
                End If
            End If
            DS.Dispose()


            '*********************************************
            'esborro la colecció de datasets
            '*********************************************

            css = GAIA2.netejaCss(css)
        Catch ex As Exception
            GAIA2.f_logError(objConn, "obrefullaweb", ex.Message, " Rel.incod=" & rel.incod & "_" & strErr)
        End Try
    End Sub 'obreFullaWeb							


    '************************************************************************************************************
    '	Funció: GAIA.netejaCss
    '	dins del valor css tinc un string amb class que formen els diferents widths de les cel·les. Com que les 
    '   obtinc de diferents fonts, netejo els duplicats ara. 
    '   Ex:  wpx120{width:120px}  wpx120{width:120px}  -->  wpx120{width:120px} 
    '************************************************************************************************************

    Public Shared Function netejaCss(ByVal css As String) As String
        'cerco totes les cadenes wpx, faig un replace a "" per eliminarles i les afegeixo al final.
        Dim pos As Integer = 0
        Dim cssTmp As String = ""
        Dim wp As String = ""
        Dim wpFinals As String = ""
        While True
            pos = InStr(css, ".wpx") - 1
            If pos < 0 Then
                Exit While

            Else
                cssTmp = css.Substring(pos)
                wp = cssTmp.Substring(0, InStr(cssTmp, "}"))
                css = css.Replace(wp, "")
                wpFinals &= " " & wp
            End If

        End While



        If InStr(css, "--></style>") = 0 Then
            css &= wpFinals
        Else
            css = css.Replace("--></style>", wpFinals & " --></style>")
        End If

        Return (css)
    End Function



    '************************************************************************************************************
    '	Funció: GAIA.estatcircuit
    '	Entrada:  
    '					codiRelacio: relació on el fill apunta al node que volem representar.
    '					index: zona de l'estructura on representarem el contingut
    '					plantilla: plantilla que utilitzarem per representar el contingut
    '					codiNode: node on està el contingut
    '					width: amplada que volem donar al contingut
    '					llistaDocuments: array on afegirem els documents que finalment haurem de copiar al servidor destí
    '					urlDesti: urldesti
    '					idioma: idioma que volem representar
    '					relacioInicial: Relació que apunta al node que inicialment hem volgut obrir i que ha provocat que s'hagi de
    '													representar aquest contingut
    '					publicar: si 1 llavors dessitjem publicar el contingut (generar el contingut i copiar-ho al servidor destí)
    '										en cas contrari només volem previsualitzar el contingut
    '					dataSimulacio: Data en la que volem veure com es veu, veurà o veuria el contingut.
    '					codiUsuari: Codi de l'usuari GAIA que ha iniciat l'acció

    '					fdesti: nom del fitxer que es generarà
    '		estatSituacio: si 99, faré cerques de relacions amb situació<99 i així podré veure els caducats(98). en cas contrari estatsituacio=98
    '	Procés: Miro si el contingut està dins de la finestra de temps compresa entre el temps de publicació i el de caducitat.
    '					En cas afirmatiu:
    '							Si el contingut està dins d'una pàgina web comprovo el circuit de publicació associat.
    '										Si el contingut ja estava dins d'un circuit no faig res, si no estava inicio el circuit
    '							Si no el contingut NO pertany a una pàgina web o NO té circuit associat, mostro el contingut.
    '					En cas negatiu:
    '						Si està en periode previ a la publicació i volem publicar ho canvio d'estat a "Pendent de publicar"
    '						Si està en periode de caducitat, faig el mateix que en cas de la publicació i si es pot caducar el contingut 
    '						l'esborro.
    '					
    '
    '
    '
    '

    '	 Important: Només entra en un estat de caducitat quan supera la dataCAD, no quan algú fa "esborrar" dins del contingut.
    '
    '
    '
    '
    '

    '************************************************************************************************************				



    Public Shared Function estatCircuit(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal idioma As Integer, ByVal relIni As clsRelacio, ByVal dataSimulacio As DateTime, ByVal codiUsuari As Integer, ByVal publicar As Integer, ByVal estatsituacio As String) As Integer

        Dim celdaFormateada As String
        Dim nouEstat, codiRelacio As Integer
        Dim codiCircuitPublicacio, codiCircuitCaducitat As Integer

        Dim relCircuitActual, relCircuitActualNodePare As clsRelacio
        celdaFormateada = ""

        estatCircuit = 0

        'Si és una imatge o document que penja d'un contingut, accepto sense mirar més. Això és perque el seu cicle de vida depèn del contingut superior i per tant no he de comprovar el seu.
        If rel.cdsit = 96 Or rel.cdsit = 97 Or rel.cdsit = 95 Then
            Return 1
        End If

        'comprovo si el contingut es pot publicar a la data actual
        nouEstat = GAIA2.contingutEsPublicable(objConn, rel, dataSimulacio, idioma, estatsituacio)
        ' Si estat=ctPUBLICAR o ctCADUCAR , la relació inicial apunta a una pàgina web i l'usuari dessitja publicar he de tractar el possible circuit
        '		(OR rel.cdsit=2)  he tret això perque sortia alguna informació amb continguts pendents de publicar (només sortia la reserva d'espai amb divs i el [més]info)
        If (rel.cdsit = 3 Or nouEstat = 1) And Not (nouEstat = 4) Then 'ja s'ha aprovat i per tant no comprovo els circuits o bé si nouestat=1 no faig res perque es un node de tipus "node *" que no té data de publicació o caducitat
            If nouEstat <> -1 Then
                GAIA2.canviEstatRelacio(objConn, rel, nouEstat, 0)
            End If
            estatCircuit = 1
        Else
            codiRelacio = rel.incod
            GAIA2.obtenirCodiCircuitFullaWeb(objConn, relIni.infil, idioma, codiCircuitPublicacio, codiCircuitCaducitat)
            Select Case nouEstat
                Case ctPENDENTPUBLICAR
                    If DateDiff("n", dataSimulacio, Now) <> 0 Then 'si és una simulació ensenyo el contingut caducat
                        estatCircuit = 1
                    Else
                        If codiCircuitPublicacio > 0 Then
                            relCircuitActual = GAIA2.obtenirPasDeCircuitRelacio(objConn, rel)
                            Select Case relCircuitActual.infil
                                Case 0 'no està en cap circuit						
                                    GAIA2.iniciarCircuit(objConn, rel, codiCircuitPublicacio, codiUsuari, 0, relIni)
                                    If DateDiff("n", dataSimulacio, Now) = 0 Then
                                        GAIA2.canviEstatRelacio(objConn, rel, nouEstat, 1)
                                    End If
                                Case codiCircuitPublicacio  'està dins d'un circuit de publicació
                                    GAIA2.tractarPasCircuit(objConn, rel, relCircuitActual)
                                Case codiCircuitCaducitat  'està dins d'un circuit de caducitat
                                    'Estava dins d'un circuit de caducitat i avans de que ho aprovi ningú, es canvia la data de caducitat i provoca que encara no s'hagi
                                    'superat la data de caducitat.
                                    'Trec el contingut del circuit de caducitat i entra en un de publicació.
                                    GAIA2.anularPasCircuit(objConn, rel)
                                    GAIA2.iniciarCircuit(objConn, rel, codiCircuitPublicacio, codiUsuari, 0, relIni)
                                    If DateDiff("n", dataSimulacio, Now) = 0 Then
                                        GAIA2.canviEstatRelacio(objConn, rel, nouEstat, 1)
                                    End If

                            End Select
                        Else
                            If codiCircuitCaducitat > 0 Then
                                'Estava dins d'un circuit de caducitat i avans de que ho aprovi ningú, es canvia la data de caducitat i provoca que encara no s'hagi
                                'superat la data de caducitat.
                                GAIA2.anularPasCircuit(objConn, rel)
                            End If
                            If DateDiff("n", dataSimulacio, Now) = 0 Then

                                GAIA2.canviEstatRelacio(objConn, rel, nouEstat, 0)
                            End If
                        End If
                        estatCircuit = 0 'no es mostra el contingut perque no és dins del temps de publicació.
                    End If

                Case ctPUBLICAR

                    If codiCircuitPublicacio > 0 Then
                        'Tinc un circuit associat de publicació
                        'He de buscar si el contingut ja estaba dins d'un circuit, esperant l'aprovació
                        relCircuitActual = GAIA2.obtenirPasDeCircuitRelacio(objConn, rel)

                        relCircuitActualNodePare = GAIA2.obtenirRelacioSuperior(objConn, relCircuitActual)
                        Select Case relCircuitActualNodePare.infil
                            Case 0 'no està en cap circuit									
                                GAIA2.iniciarCircuit(objConn, rel, codiCircuitPublicacio, codiUsuari, 0, relIni)
                                If DateDiff("n", dataSimulacio, Now) = 0 Then

                                    GAIA2.canviEstatRelacio(objConn, rel, nouEstat, 1)
                                End If

                                estatCircuit = 0
                            Case codiCircuitPublicacio  'està dins d'un circuit de publicació

                                GAIA2.tractarPasCircuit(objConn, rel, relCircuitActual)
                                estatCircuit = 0
                            Case codiCircuitCaducitat  'està dins d'un circuit de caducitat												
                                'Estava dins d'un circuit de caducitat i avans de que ho aprovi ningú es torna a demanar que es publiqui (potser s'han canviat les dates de caducitat). Anul·lo el circuit de caducitat i el poso en un altre de publicació.
                                GAIA2.anularPasCircuit(objConn, rel)
                                GAIA2.iniciarCircuit(objConn, rel, codiCircuitPublicacio, codiUsuari, 0, relIni)
                                If DateDiff("n", dataSimulacio, Now) = 0 Then

                                    GAIA2.canviEstatRelacio(objConn, rel, nouEstat, 1)
                                End If
                                estatCircuit = 0
                        End Select
                    Else ' no hi ha circuit associat de publicació
                        If codiCircuitCaducitat > 0 Then 'hi ha un circuit de caducitat per la pàgina web
                            relCircuitActual = GAIA2.obtenirPasDeCircuitRelacio(objConn, rel)
                            If relCircuitActual.infil = codiCircuitCaducitat Then   '¡''''''''''''''''''eeehhhh!
                                'Estava dins d'un circuit de caducitat i avans de que ho aprovi ningú es torna a demanar que es publiqui (potser s'han canviat les dates de caducitat). Anul·lo el circuit de caducitat i el poso en un altre de publicació.
                                GAIA2.anularPasCircuit(objConn, rel)
                            End If '
                        Else
                            If DateDiff("n", dataSimulacio, Now) = 0 Then

                                GAIA2.canviEstatRelacio(objConn, rel, nouEstat, 0)
                            End If

                        End If
                        'En qualsevol cas (sense circuit de publicació o amb circuit de caducitat pendent d'aprovar, de moment el publico o represento amb preview)
                        estatCircuit = 1
                    End If


                Case ctPENDENTCADUCAR

                    If codiCircuitCaducitat > 0 Then
                        'Tinc un circuit associat de caducitat

                        'He de buscar si el contingut ja estaba dins d'un circuit, esperant l'aprovació								
                        relCircuitActual = GAIA2.obtenirPasDeCircuitRelacio(objConn, rel)
                        relCircuitActualNodePare = GAIA2.obtenirRelacioSuperior(objConn, relCircuitActual)
                        Select Case relCircuitActualNodePare.infil
                            Case 0 'no està en cap circuit					
                                GAIA2.iniciarCircuit(objConn, rel, codiCircuitCaducitat, codiUsuari, 0, relIni)

                                If DateDiff("n", dataSimulacio, Now) = 0 Then

                                    GAIA2.canviEstatRelacio(objConn, rel, nouEstat, 1)
                                End If

                                estatCircuit = 1 'mostraré el contingut igual
                            Case codiCircuitCaducitat  'està dins d'un circuit de caducitat										
                                GAIA2.tractarPasCircuit(objConn, rel, relCircuitActual)
                                estatCircuit = 1 'Mostraré el contingut si toca
                            Case codiCircuitPublicacio 'està dins d'un circuit de publicació							
                                'Està dins d'un circuit de publicació i arriba el moment de  caducar el contingut
                                'No faig res, el contingut no es veurà. Quan acabi el circuit d'aprovació, el contingut no es veurà mai perque ja haurà caducat.
                                'Millor el trec del circuit de publicació i caduco el contingut
                                GAIA2.anularPasCircuit(objConn, rel)
                                GAIA2.iniciarCircuit(objConn, rel, codiCircuitCaducitat, codiUsuari, 0, relIni)
                                If DateDiff("n", dataSimulacio, Now) = 0 Then

                                    GAIA2.canviEstatRelacio(objConn, rel, nouEstat, 1)
                                End If

                                estatCircuit = 1
                                'PROBLEMA: UN contingut que passa de pendent d'aprovar la seva publicació i que entra en un circuit de caducitat. Quan s'intenti publicar de nou el contingut, aquest es publicarà mentre no es doni el ok a la caducitat!.


                        End Select
                    Else 'no hi ha circuit associat de caducitat			
                        If codiCircuitPublicacio > 0 Then 'hi ha un circuit de publicació per la pàgina web
                            relCircuitActual = GAIA2.obtenirPasDeCircuitRelacio(objConn, rel)
                            If relCircuitActual.infil = codiCircuitPublicacio Then
                                'El contingut està en un pas d'un circuit de publicació, elimino el pas i caduco el contingut
                                GAIA2.anularPasCircuit(objConn, rel)
                            End If

                        End If
                        'Ja es pot esborrar el contingut i el poso la relació en estat d'esborrat
                        nouEstat = ctESBORRATCADUCAT
                        GAIA2.caducarContinguts(objConn, rel, codiUsuari)
                        If DateDiff("n", dataSimulacio, Now) = 0 Then

                            GAIA2.canviEstatRelacio(objConn, rel, nouEstat, 0)
                        End If

                    End If
                    estatCircuit = 0

            End Select
        End If

    End Function 'estatCircuit


    Public Shared Function representaCodiWeb(ByVal objConn As OleDbConnection, ByVal plantilla As clsPlantilla, ByVal rel As clsRelacio, ByVal index As Integer, ByVal idioma As Integer, ByVal relIni As clsRelacio, ByVal width As Double, ByVal codiusuari As Integer, ByRef forçarCodi As Integer, ByRef strCss As String, ByRef tagsMeta As String) As String
        Dim celdaFormateada As String = ""
        Dim DS As DataSet
        Dim dbRow As DataRow

        Dim codiRelacio As Integer
        Dim relacioInicial As Integer
        DS = New DataSet()
        Dim codiweb As String = ""
        codiRelacio = rel.incod
        relacioInicial = relIni.incod
        GAIA2.bdr(objConn, "SELECT  METLAWE.AWEDSLCW, METLNWE.NWEDSLCW, METLWEB.WEBDSLCW FROM METLREL  WITH(NOLOCK) LEFT OUTER JOIN METLWEB  WITH(NOLOCK) ON METLREL.RELINFIL = METLWEB.WEBINNOD AND METLWEB.WEBINIDI =" + idioma.ToString() + " LEFT OUTER JOIN METLAWE WITH(NOLOCK)  ON METLREL.RELINFIL = METLAWE.AWEINNOD LEFT OUTER JOIN      METLNWE WITH(NOLOCK)  ON METLREL.RELINFIL = METLNWE.NWEINNOD WHERE(METLREL.RELCDSIT<98) AND (METLREL.RELINCOD = " + codiRelacio.ToString() + ") ", DS)


        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            If IsDBNull(dbRow("AWEDSLCW")) Then
                If IsDBNull(dbRow("NWEDSLCW")) Then
                    If IsDBNull(dbRow("WEBDSLCW")) Then
                        codiweb = ""
                    Else
                        codiweb = dbRow("WEBDSLCW")
                    End If
                Else
                    codiweb = dbRow("NWEDSLCW")
                End If
            Else
                codiweb = dbRow("AWEDSLCW")
            End If
            If Split(codiweb, ",").Length > index Then

                Dim lcw As String
                lcw = Split(codiweb, ",")(index).Trim()
                If lcw.Length > 0 Then

                    celdaFormateada += GAIA2.trobaCodiWeb(objConn, plantilla, lcw, idioma, rel, relIni, forçarCodi, index, "", width, codiusuari, strCss, tagsMeta)

                End If

            End If
        End If
        representaCodiWeb = celdaFormateada
        DS.Dispose()
    End Function 'representaCodiWeb




    '******************************************************************
    '	Funció: canviEstatRelacio
    '	Entrada: 

    '		codiRelacio: codi de la relacio que conté l'estat que volem actualitzar
    '		codiEstat: codi del nou estat
    '	Procés: 	

    '		Actualitza METLREL amb el nou estat
    '******************************************************************
    Public Shared Sub canviEstatRelacio(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiEstat As Integer, ByVal circuit As Integer)
        If rel.cdsit = 96 Or rel.cdsit = 97 Or rel.cdsit = 95 Or rel.cdsit = 98 Or rel.cdsit = 99 Then
            'No faig res perque el continguts en aquest estat continuen sempre igual
        Else
            If codiEstat > 0 Then
                If circuit = 1 Then
                    Select Case codiEstat
                        Case ctPENDENTPUBLICAR
                            codiEstat = ctPENDENTPUBLICARCIRCUIT
                        Case ctPUBLICAR
                            codiEstat = ctPUBLICARCIRCUIT
                        Case ctPENDENTCADUCAR
                            codiEstat = ctPENDENTCADUCARCIRCUIT
                    End Select
                Else 'Trec la relació d'un estat dins del circuit
                    Select Case codiEstat
                        Case ctPENDENTPUBLICARCIRCUIT
                            codiEstat = ctPENDENTPUBLICAR

                        Case ctPUBLICARCIRCUIT
                            codiEstat = ctPUBLICAR
                        Case ctPENDENTCADUCARCIRCUIT
                            codiEstat = ctPENDENTCADUCAR
                    End Select
                End If
                If codiEstat <> rel.cdsit Then
                    GAIA2.bdSR(objConn, "UPDATE METLREL SET RELCDSIT=" & codiEstat.ToString() + " WHERE RELINCOD=" + rel.incod.ToString())
                End If

            End If
        End If
    End Sub 'canviEstatRelacio



    '******************************************************************
    '	Funció: trobaCodiWeb
    '	Entrada: 
    '		codiWeb: codi del node que apunta a la informació sobre la llibreria web
    '		codiIdioma: codi del idioma de la instància de llibreria que volem cercar
    '		codiRelacio: codi de la relació que apunta al objecte que conté el codi web. 
    '	  relacioInicial: codi de la relació que ha iniciat el procés de representar la pàgina
    ' 	Index: cel·la on s'executarà el codiweb
    '		Parametres: string amb tots els parametres que té la pàgina que vol executar una llibreria de codi web (procés després de la publicació)
    '	Procés: 	
    '		Busca dins de la taula de "llibreria de codi web" (LCWDSTXT) el codiweb dins del camp METLLCW
    '		En el cas de que el codi web sigui de tipus 2:codi executable abans de la publicació (previsualització), 
    '		faré una crida a l'adreça contenida a LCWDSTXT i pasaré com a paràmetre "codiRelacio". El resultat el 
    '		retornaré a la variable trobacodiWeb
    '		Si és de tipus 1 retorno el contingut de METLLCW.LCWDSTXT
    '		Si és de tipus 3 (codi executable després de la publicació), per ara faig igual que al tipus 1. 
    '			Ho deixo preparat per retornar codi en un altre llenguatge (fent un canvi en l'extensió de la pàgina que retornaré...
    '			això cal tractar-ho a fons)
    '******************************************************************


    Public Shared Function trobaCodiWeb(ByVal objConn As OleDbConnection, ByVal plantilla As clsPlantilla, ByVal codiWeb As String, ByVal codiIdioma As Integer, ByVal rel As clsRelacio, ByVal relIni As clsRelacio, ByRef forçarCodi As Integer, ByVal index As Integer, ByVal parametres As String, ByVal width As String, ByVal codiUsuari As Integer, ByRef strCss As String, ByRef tagsMeta As String) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim csstmp As String = ""
        DS = New DataSet()

        trobaCodiWeb = ""
        Dim resultat As String = ""


        Try

            Dim element As String

            For Each element In Split(codiWeb, "|")





                GAIA2.bdr(objConn, "SELECT LCWCDTIP,  LCWDSTXT, LCWTPFOR, LCWTPFOL FROM METLLCW  WITH(NOLOCK) WHERE LCWINNOD=" & element & " AND LCWINIDI=" + codiIdioma.ToString(), DS)
                If DS.Tables(0).Rows.Count = 0 Then 'busco un altre idioma
                    GAIA2.bdr(objConn, "SELECT LCWCDTIP,  LCWDSTXT, LCWTPFOR, LCWTPFOL FROM METLLCW  WITH(NOLOCK) WHERE LCWINNOD=" & element & " ORDER BY LCWINIDI ASC", DS)
                End If
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)

                    If dbRow("LCWTPFOR") = "S" Then
                        forçarCodi = 1
                    Else
                        forçarCodi = 0
                    End If
                    Dim llibreria As String
                    Dim trobaCodiWebTMP As String = ""
                    For Each llibreria In Split(dbRow("LCWDSTXT"), "|")
                        Select Case CInt(dbRow("LCWCDTIP"))
                            Case 1  ' codi HTML 
                                resultat += llibreria.Trim()
                            Case 2 'codi executable abans de la publicació (previsualització)


                                If InStr(llibreria, "?") > 0 Then
                                    'compte, ha de ser "lhintranet". només "intranet" dona un error de permisos
                                    trobaCodiWebTMP = GAIA2.GetHTML(objConn, llibreria & "&codiRelacio=" & rel.incod & "&codiIdioma=" & codiIdioma & "&codiRelacioInicial=" & relIni.incod & "&est=" & index & "&width=" & width & "&node=" & rel.infil & "&us=" & codiUsuari & "&codiPlantilla=" & plantilla.innod, element, codiIdioma, File.GetLastWriteTime("c:\inetpub\wwwroot\GAIA\aspx\llibreriacodiweb\" & llibreria.Trim().Substring(0, InStr(llibreria, "?") - 1)), csstmp, relIni, rel, index, codiUsuari, plantilla.innod)
                                Else
                                    trobaCodiWebTMP = GAIA2.GetHTML(objConn, llibreria & "?codiRelacio=" & rel.incod & "&codiIdioma=" & codiIdioma & "&codiRelacioInicial=" & relIni.incod & "&est=" & index & "&width=" & width & "&node=" & rel.infil & "&us=" & codiUsuari & "&codiPlantilla=" & plantilla.innod, element, codiIdioma, File.GetLastWriteTime("c:\inetpub\wwwroot\GAIA\aspx\llibreriacodiweb\" + llibreria.Trim()), csstmp, relIni, rel, index, codiUsuari, plantilla.innod)
                                End If
                                If trobaCodiWebTMP.Length = 0 And dbRow("LCWTPFOL") = "N" Then
                                    resultat = ""
                                    Exit For
                                Else
                                    resultat += trobaCodiWebTMP
                                End If

                            Case 3 'codi executable després de la publicació 				
                                resultat += llibreria.Trim().Replace("Request(""stridioma"")", codiIdioma.ToString()).Replace("Request(""codiIdioma"")", codiIdioma.ToString())
                            Case 4 'fitxer executable després de la publicació 				  						
                                Dim fs As New FileStream("c:\inetpub\wwwroot\GAIA\aspx\llibreriacodiweb\" + llibreria.Trim(), FileMode.Open, FileAccess.Read)
                                Dim sr As StreamReader = New StreamReader(fs, System.Text.Encoding.Default)
                                trobaCodiWebTMP = sr.ReadToEnd()
                                trobaCodiWebTMP = trobaCodiWebTMP.Replace("""<IDIOMA>""", codiIdioma)
                                sr.Close()
                                fs.Close()

                                If trobaCodiWebTMP.Length = 0 And dbRow("LCWTPFOL") = "N" Then
                                    resultat = ""
                                    Exit For
                                Else
                                    resultat += trobaCodiWebTMP
                                End If

                        End Select
                    Next llibreria


                    strCss &= csstmp

                    '********************************************************************************************************************
                    '	Cerco els blocs <style>  i els tagsmeta <link> de la execució de la llibreria i els poso a strcss i tagsMeta per 
                    ' 	pujarlos posteriorment al head de la pàgina
                    '********************************************************************************************************************
                    If CInt(dbRow("LCWCDTIP")) = 2 Then
                        Dim posIni As Integer = InStr(resultat, "<style type=""text/css")
                        Dim posFi As Integer
                        While posIni > 0
                            posFi = InStr(resultat, "</style>")
                            '27 = per saltar <style type="text/css"><!--
                            ' - 3 abans de --></style>
                            strCss &= resultat.Substring(posIni - 1 + 27, posFi - posIni - 3 - 27)
                            resultat = resultat.Substring(0, posIni - 1) + resultat.Substring(posFi + 7)
                            posIni = InStr(resultat, "<style type=""text/css")
                        End While
                        'a strCss tinc la relació d'estils
                        posIni = InStr(resultat, "<link")
                        posFi = 0
                        While posIni > 0
                            posFi = InStr(resultat.Substring(posIni), ">")
                            tagsMeta += resultat.Substring(posIni - 1, posFi + 1)
                            resultat = resultat.Substring(0, posIni - 1) + resultat.Substring(posIni + posFi)
                            posIni = InStr(resultat, "<link")
                        End While
                        'a tagsMeta tinc la relació de  <link>
                    End If

                End If
                trobaCodiWeb &= resultat
                resultat = ""
            Next
        Catch ex As Exception

            f_logError(objConn, "G01-trobacodiweb", ex.Source, ex.Message)
            llistatErrors &= "<br/> " & ex.ToString & ". <br/> Codiweb=" & codiWeb
            trobaCodiWeb = ""

        End Try

        DS.Dispose()
    End Function 'trobaCodiWeb


    '******************************************************************
    '	Funció: contingutEsPublicable
    '	Entrada: 
    '		codiRelacio: Relacio on relinfil apunta al contingut que vull saber si es publicable
    '		dataSimulacio : Data en la que volem veure si el contingut és publicable
    '		codiIdioma: codi de l'idioma del contingut que comprovarem si és publicable
    '		estatSituacio: si 99, faré cerques de relacions amb situació<99 i així podré veure els caducats(98). en cas contrari estatsituacio=98
    '	Procés: 	

    '		Comprobo si la data actual està dintre de la finestra de temps definada per [data de publicació, data de caducitat}
    '		En cas de que hagi igualat o superat la data de caducitat, retornaré "4" (caducat) 
    '		En cas de que no hagi igualat o superat la data de caducitat i encara no hagi arribat la data de publicació , retornaré
    '			"2" (pendent de publicació)
    '		En cas de que es pugui publicar retornaré "3" (publicar)
    '******************************************************************
    Public Shared Function contingutEsPublicable(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal dataSimulacio As DateTime, ByVal codiIdioma As Integer, ByVal estatSituacio As String) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim taula, prefixCamp As String
        Dim data As DateTime
        Dim codiRelacio As Integer



        codiRelacio = rel.incod
        If dataSimulacio = CDate("01/01/1900") Then
            data = CDate(Now)
        Else
            data = dataSimulacio
        End If



        taula = ""
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT TBLDSTXT FROM METLREL WITH(NOLOCK) ,METLNOD WITH(NOLOCK) , METLTBL  WITH(NOLOCK) WHERE RELINCOD=" & codiRelacio.ToString() & " AND RELINFIL=NODINNOD AND NODCDTIP=TBLINTFU AND RELCDSIT<" + estatSituacio, DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            taula = dbRow("TBLDSTXT")
        End If
        contingutEsPublicable = 1

        If taula.Length > 0 Then
            prefixCamp = taula.Substring(4).Trim()

            GAIA2.bdr(objConn, "SELECT  " + prefixCamp.ToString() + "DTPUB," + prefixCamp.ToString() + "DTCAD FROM  METLREL WITH(NOLOCK) , " + taula.ToString() + " WITH(NOLOCK)  WHERE RELCDSIT<" + estatSituacio + " AND RELINFIL=" + prefixCamp.ToString() + "INNOD AND RELINCOD=" + codiRelacio.ToString() + " AND " + prefixCamp.ToString() + "INIDI=" + codiIdioma.ToString(), DS)
            If DS.Tables(0).Rows.Count > 0 Then
                dbRow = DS.Tables(0).Rows(0)
                If dbRow(prefixCamp.ToString() + "DTPUB").ToString.Trim().Length > 0 And dbRow(prefixCamp.ToString() + "DTCAD").ToString().Trim().Length > 0 Then
                    If dbRow(prefixCamp.ToString() + "DTCAD") <> CDate("01/01/1900") And DateDiff("n", dbRow(prefixCamp.ToString() + "DTCAD"), data) >= 0 Then
                        contingutEsPublicable = 4 'caducat
                    Else
                        If DateDiff("n", dbRow(prefixCamp.ToString() + "DTPUB"), data) >= 0 Then
                            contingutEsPublicable = 3 'publicat
                        Else
                            contingutEsPublicable = 2 'pendent de publicar
                        End If
                    End If
                End If
            Else
                'Faig un tractament per les taules de versions de tràmits
                If taula.Trim() = "METLFTR" Then
                    taula = "METLFTRV"

                    GAIA2.bdr(objConn, "SELECT  " + prefixCamp.ToString() + "DTPUB," + prefixCamp.ToString() + "DTCAD FROM  METLREL WITH(NOLOCK) , " + taula.ToString() + " WITH(NOLOCK)  WHERE RELCDSIT<" + estatSituacio + " AND RELINFIL=" + prefixCamp.ToString() + "INNOD AND RELINCOD=" + codiRelacio.ToString() + " AND " + prefixCamp.ToString() + "INIDI=" + codiIdioma.ToString() & " AND " & prefixCamp & "INVER=0", DS)
                    If DS.Tables(0).Rows.Count > 0 Then
                        dbRow = DS.Tables(0).Rows(0)
                        If dbRow(prefixCamp.ToString() + "DTPUB").ToString.Trim().Length > 0 And dbRow(prefixCamp.ToString() + "DTCAD").ToString().Trim().Length > 0 Then
                            If dbRow(prefixCamp.ToString() + "DTCAD") <> CDate("01/01/1900") And DateDiff("n", dbRow(prefixCamp.ToString() + "DTCAD"), data) >= 0 Then
                                contingutEsPublicable = 4 'caducat
                            Else
                                If DateDiff("n", dbRow(prefixCamp.ToString() + "DTPUB"), data) >= 0 Then
                                    contingutEsPublicable = 3 'publicat
                                Else
                                    contingutEsPublicable = 2 'pendent de publicar
                                End If
                            End If
                        End If
                    Else
                        contingutEsPublicable = -1
                    End If
                Else
                    contingutEsPublicable = -1
                End If
            End If  'trobo les dades
        End If
        'Si es el contingut encara està viu, comprovem si el contingut dins de l'ubicació que ocupa també és pot publicar

        If contingutEsPublicable <> 4 Then
            Dim dataIni, dataFi As DateTime
            dataIni = CDate("01/01/1900")
            dataFi = CDate("01/01/2050")
            GAIA2.datesPublicacio(objConn, rel, codiIdioma, dataIni, dataFi)

            If dataFi <> CDate("01/01/1900") Then
                'la comprovació de CDate(dataIni) > CDate(dataFi) és per casos en els que han canviat les dates de caducitat correctament però no la de publicació. 
                If CDate(dataIni) > CDate(dataFi) Or (CDate(dataIni) <= data And CDate(dataFi) > data) Then
                    'ok
                Else
                    contingutEsPublicable = 98
                End If
            End If
        End If


        DS.Dispose()
    End Function 'contingutEsPublicable


    '******************************************************************
    '	Funció: publica
    '	Entrada: 
    '		codiHtml: html per presentar la pàgina
    '		fDesti: Fitxer on guardaré la pàgina
    '		llistaDocumentsPerAfegir: array amb la llista de documents associats que he de copiar.

    '		llistaDocumentsPerEsborrar: llista de documents que he d'esborrar
    '		tipusAccio: 1: publicar--> faré la copia de la pàgina
    '							 	0: eliminar--> esborraré la pàgina

    '		relIni: relació del contingut que es vol publicar (el primer del que penjen els sub-continguts)
    '		idioma: idioma de la relació que es publicarà (s'utilitza per actualitzar la taula METLREI)
    '	Procés: 	
    '		Gravo el contingut de html en fDesti o bé esborro fDesti.
    '	Retorna 1 si ha publicat correctament
    '******************************************************************
    Public Shared Function publica(ByVal objConn As OleDbConnection, ByVal codiHtml As String, ByVal fDesti As String, ByVal llistaDocumentsPerAfegir As String(), ByVal llistaDocumentsPerEsborrar As String(), ByVal tipusAccio As Integer, ByVal relIni As clsRelacio, ByVal idioma As Integer, ByRef servidor As String, ByRef ftpSession As Session) As Integer
        Dim strErr As String = ""
        Try

            Dim objStreamWriter As StreamWriter
            Dim cont As Integer
            Dim pathOrigen As String = "e:\docs\GAIA\"
            Dim pathDocs As String
            Dim posTMP As Integer
            Dim document As String
            Dim relacio As Integer
            Dim relTmp As New clsRelacio
            Dim servidorNou, usuari, pwd, pathDestiArrel, pathDocsDesti, URL As String
            servidorNou = ""
            usuari = ""
            pwd = ""
            pathDestiArrel = ""
            pathDocsDesti = ""
            URL = ""
            Dim nomFitxer As String = ""
            Dim ds As DataSet
            ds = New DataSet
            Dim port As String = ""
            Dim codiDocument As Integer
            Dim nomFitxerPublicat As String = ""

            Dim UPD As Integer = 0
            strErr &= "1"
            publica = 1
            pathDocs = "/gdocs"
            If tipusAccio = 0 Then
                'esborro la pàgina web

                'File.Delete(pathDesti+"\"+fDesti)
                'Si no queda res a la carpeta també l'esborro
                'IF Directori.GetFiles(pathDesti).length=0 THEN
                '	Directori.delete(pathDesti)
                'END IF
                'esborro els documents de la llista
                If Not llistaDocumentsPerEsborrar Is Nothing Then
                    For cont = 0 To llistaDocumentsPerEsborrar.Length - 1
                        '				File.Delete(pathDocs+"\"+llistaDocumentsPerEsborrar(cont))
                        '				IF File.Exists(pathOrigen.tostring()+llistaDocumentsPerEsborrar(cont).SubString(0,posTMP-1)+"P"+llistaDocumentsPerEsborrar(cont).substring(posTMP-1)) THEN
                        'Si és una imatge i existeix la versió de THUMBNAIL també l'esborro
                        '					File.Delete( pathDocs+"\"+	llistaDocumentsPerEsborrar(cont).SubString(0,posTMP-1)+"P"+llistaDocumentsPerEsborrar(cont).substring(posTMP-1))					
                        '	END IF
                    Next cont
                End If

            Else 'tipusAccio=1 -> copiar
                strErr &= "2"

                GAIA2.trobaServidorDesti(objConn, relIni, servidorNou, usuari, pwd, pathDestiArrel, pathDocsDesti, port, URL)
                strErr &= "3"
                ftpSession = New Session()
                ftpSession.Server = servidorNou
                'strErr &="4- servidor=" & servidorNou & "--" & usuari & "--" & pwd & "rel=" & relTmp.incod & "--servidor=" & servidor
                ftpSession.Connect(usuari, pwd)
                strErr &= "5"


                If Not llistaDocumentsPerAfegir Is Nothing Then


                    For cont = 0 To llistaDocumentsPerAfegir.Length - 1
                        relacio = llistaDocumentsPerAfegir(cont).Substring(0, InStr(llistaDocumentsPerAfegir(cont), "_") - 1)
                        document = llistaDocumentsPerAfegir(cont).Substring(InStr(llistaDocumentsPerAfegir(cont), "_"))
                        If InStr(document, "/") >= 0 Then
                            nomFitxer = document.Substring(InStrRev(document, "/"))
                        Else
                            nomFitxer = document
                        End If
                        relTmp.bdget(objConn, relacio)

                        If servidorNou <> "" Then
                            If cont = 0 Then
                                'si no existeix la carpeta destí la creo			

                                'IF NOT directory.Exists(pathDocs) THEN
                                '	Directory.CreateDirectory(pathDocs)
                                'END IF
                            End If
                            'Comprovo que el document necessiti publicació i que alhora no s'hagi publicat
                            nomFitxerPublicat = ""
                            strErr &= "6"
                            GAIA2.bdr(objConn, "SELECT METLREI.REIDSFIT, METLDOC.DOCWNUPD, METLDOC.DOCINNOD FROM METLREL  WITH(NOLOCK) INNER JOIN METLDOC WITH(NOLOCK)  ON METLDOC.DOCINNOD = METLREL.RELINFIL AND METLDOC.DOCINIDI =" + idioma.ToString() + " LEFT OUTER JOIN METLREI  WITH(NOLOCK) ON METLREI.REIINCOD = METLREL.RELINCOD AND METLREI.REIINIDI =" + idioma.ToString() + " WHERE (METLREL.RELINCOD =" + relacio.ToString() + ")", ds)
                            If ds.Tables(0).Rows.Count > 0 Then
                                strErr &= "(6.2)"
                                If Not IsDBNull(ds.Tables(0).Rows(0)("REIDSFIT")) Then
                                    nomFitxerPublicat = ds.Tables(0).Rows(0)("REIDSFIT").Trim()
                                End If
                                UPD = ds.Tables(0).Rows(0)("DOCWNUPD")
                                codiDocument = ds.Tables(0).Rows(0)("DOCINNOD")
                            End If

                            strErr &= "(6.3)"
                            If nomFitxerPublicat = "" Or UPD = 1 Then
                                If UPD = 1 Then
                                    'Copio l'arxiu al destí
                                    pathDocsDesti = pathDocsDesti.Replace("/webs/auditoribarradas", "").Replace("/webs/teatrejoventut", "")

                                    strErr &= "7: pathDocsDesti=" & pathDocsDesti & ", document=" & document
                                    llistatErrors &= GAIA2.ftpCopiarFitxer(objConn, servidorNou, usuari, pwd, pathOrigen + document, pathDocsDesti + "/" + document, relacio, "", ftpSession, pathDestiArrel)
                                    strErr &= llistatErrors

                                    GAIA2.bdSR(objConn, "UPDATE METLREL SET RELDSFIT='" & pathDocsDesti.Replace(pathDestiArrel, "") & "/" & document.Replace("/webs/auditoribarradas", "").Replace("/webs/teatrejoventut", "") + "' WHERE RELINCOD=" + relacio.ToString())
                                    If document.Length > 0 Then
                                        GAIA2.bdr(objConn, "SELECT * FROM METLREI  WITH(NOLOCK) WHERE REIINCOD=" + relacio.ToString() + " AND REIINIDI=" + idioma.ToString(), ds)
                                        If ds.Tables(0).Rows.Count > 0 Then


                                            GAIA2.bdSR(objConn, "UPDATE METLREI SET REIDSFIT='" + (pathDocsDesti.Replace("\", "/").Replace(pathDestiArrel, "") & "/" & document).Replace("/webs/auditoribarradas", "").Replace("/webs/teatrejoventut", "").Replace("/webs/teatreJoventut", "") & "', REIDSFI2='" & nomFitxer & "' WHERE REIINCOD=" & relTmp.incod & " AND REIINIDI = " & idioma)


                                        Else
                                            Dim dataIni, dataFi As DateTime
                                            GAIA2.datesPublicacio(objConn, relTmp, idioma, dataIni, dataFi)

                                            GAIA2.bdSR(objConn, "INSERT INTO METLREI VALUES(" & relTmp.incod & "," & idioma & ",'" & dataIni.ToString() & "','" & dataFi.ToString() & "','" & (pathDocsDesti.Replace("\", "/").Replace(pathDestiArrel, "").Replace("/GAIA06", "").Replace("/GAIA", "") & "/" & document).Replace("/webs/auditoribarradas", "").Replace("/webs/teatreJoventut", "") & "','" & document & "','')")
                                        End If


                                    End If
                                    posTMP = InStrRev(document, ".")
                                    If File.Exists(pathOrigen.ToString() + document.Substring(0, posTMP - 1) + "P" + document.Substring(posTMP - 1)) Then
                                        'Si és una imatge i existeix la versió de THUMBNAIL també la copio

                                        strErr &= "8"
                                        llistatErrors &= GAIA2.ftpCopiarFitxer(objConn, servidorNou, usuari, pwd, pathOrigen.ToString() + document.Substring(0, posTMP - 1) + "P" + document.Substring(posTMP - 1), pathDocsDesti + "/" + document.Substring(0, posTMP - 1) + "P" + document.Substring(posTMP - 1), relacio, "P", ftpSession, pathDestiArrel)
                                        strErr &= "9" & llistatErrors


                                        llistatErrors &= GAIA2.ftpCopiarFitxer(objConn, servidorNou, usuari, pwd, pathOrigen.ToString() + document.Substring(0, posTMP - 1) + "P100" + document.Substring(posTMP - 1), pathDocsDesti + "/" + document.Substring(0, posTMP - 1) + "P100" + document.Substring(posTMP - 1), relacio, "P100", ftpSession, pathDestiArrel)
                                        strErr &= "10" & llistatErrors
                                    End If
                                    If codiDocument > 0 Then
                                        GAIA2.bdSR(objConn, "UPDATE METLDOC SET DOCWNUPD=0 WHERE DOCINIDI=" + idioma.ToString() + " AND DOCINNOD=" + codiDocument.ToString())
                                    End If
                                End If
                            End If
                        Else
                            ' no faig res per que no hi ha cap servidor per publicar
                        End If
                    Next cont
                End If

                'Si no existex la carpeta destí la creo...
                'If Not System.IO.Directory.Exists(pathDesti) Then
                '	System.IO.Directory.CreateDirectory(pathDesti)
                '	End If
                '***************************************************************************************
                'Copio la pàgina web		
                '***************************************************************************************			
                strErr &= "(6.4)"
                codiHtml = codiHtml.Replace("src=""/gDocs", "src=""http://" + URL + "/gDocs")
                Dim regexp As Regex
                Dim strInPattern As String
                codiHtml = codiHtml.Replace("<u>", "<span class=""subratllat"">").Replace("<i>", "<span class=""cursiva"">").Replace("<em>", "<span class=""cursiva"">")
                strInPattern = "</u>|</i>|</em>"
                regexp = New Regex(strInPattern, RegexOptions.IgnoreCase)
                codiHtml = Regex.Replace(codiHtml, strInPattern, "</span>", RegexOptions.IgnoreCase)
                regexp = Nothing


                '***************************************************************************************
                'Tinc el HTML preparat per gravar a la carpeta temporal i iniciar el procés de copia
                '1- Generaré clau de HASH
                '2- Només publicaré si és diferent al que hi ha publicat
                '3- En aquest cas, publicaré i canviaré la clau
                '***************************************************************************************
                Dim sha1Obj As New System.Security.Cryptography.SHA1CryptoServiceProvider
                Dim enc As New System.Text.UTF8Encoding()

                Dim strHash As String = Convert.ToBase64String(sha1Obj.ComputeHash(enc.GetBytes(codiHtml)))
                strErr &= "(6.5)"

                GAIA2.bdr(objConn, "SELECT isnull(REIDSHAS,'') as REIDSHAS FROM METLREI  WITH(NOLOCK) WHERE REIINCOD=" & relIni.incod & " AND REIINIDI=" & idioma, ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    If ds.Tables(0).Rows(0)("REIDSHAS") = strHash Then
                        GAIA2.llistatWarnings &= "No s'ha publicat la pàgina per que és igual a la que ja existia"
                        publica = 0
                    End If
                End If

                If publica > 0 Then
                    Dim fitxerTMP As String
                    fitxerTMP = ""

                    If InStr(fDesti, "/") Then

                        fitxerTMP = "/" & fDesti.Substring(InStrRev(fDesti, "/"))
                    Else

                        fitxerTMP = "/" & fDesti
                        fDesti = fitxerTMP
                    End If
                    fDesti = fDesti.Replace("\\", "\").Replace("\\", "\").Replace("/", "\")


                    strErr &= "(6.6)"
                    trobaServidorDesti(objConn, relIni, servidorNou, usuari, pwd, pathDestiArrel, pathDocsDesti, port, URL)
                    If servidorNou <> "" Then
                        If servidorNou <> servidor Then
                            strErr &= "11"
                            servidor = servidorNou
                            ftpSession.Close()
                            ftpSession = New Session()
                            ftpSession.Server = servidorNou
                            ftpSession.Connect(usuari, pwd)
                            strErr &= "12"
                        End If
                        'Copio al servidor Intranet on guardarem copia de tots els documents, amb excepció de proves o de una publicació a Intranet			
                        If fitxerTMP.Length > 0 Then


                            Dim destilocal As String = ""
                            If InStr(UCase(pathDestiArrel), "GAIA") <= 0 And InStr(UCase(pathDestiArrel), "SEUELEC") <= 0 Then
                                destilocal = "\webs"
                            End If

                            'destilocal = "c:\inetpub\wwwroot\websGAIA\web" + destilocal + (pathDestiArrel + fDesti).Replace("\GAIA06", "").Replace("/GAIA06", "").Replace("/gaia", "").Replace("/GAIA", "")
                            destilocal = "c:\inetpub\wwwroot\websGAIA\web" + destilocal + (pathDestiArrel & "\" & relIni.incod & "_" & idioma & ".aspx").Replace("\GAIA06", "").Replace("/GAIA06", "").Replace("/gaia", "").Replace("/GAIA", "")
                            destilocal = destilocal.Replace("/", "\")


                            Dim strPathTmp As String = destilocal.Substring(0, InStrRev(destilocal, "\") - 1)


                            If InStr(UCase(pathDestiArrel + fDesti), "PROVES") <= 0 And servidor <> "intranet" Then
                                'Si no existeix la carpeta, la creo ara
                                If (Not System.IO.Directory.Exists(strPathTmp)) Then
                                    Try
                                        System.IO.Directory.CreateDirectory(strPathTmp)
                                    Catch
                                    End Try
                                End If
                                objStreamWriter = New StreamWriter(destilocal, False, Encoding.Default)
                            Else
                                objStreamWriter = New StreamWriter("c:\windows\temp\" + fitxerTMP, False, Encoding.Default)
                            End If





                            objStreamWriter.Write(codiHtml)
                            objStreamWriter.Close()
                            'el path es /GAIA06, copio al c:\inetpub\wwwroot\websGAIA\web\...  			
                            If InStr(UCase(pathDestiArrel + fDesti), "PROVES") <= 0 And servidor <> "intranet" Then
                                strErr &= "13"


                                llistatErrors &= GAIA2.ftpCopiarFitxer(objConn, servidorNou, usuari, pwd, destilocal, pathDestiArrel + fDesti, relIni.incod, "", ftpSession, pathDestiArrel)

                                strErr &= "14"
                            Else
                                strErr &= "15"
                                llistatErrors &= GAIA2.ftpCopiarFitxer(objConn, servidorNou, usuari, pwd, "c:\windows\temp\" + fitxerTMP, pathDestiArrel + fDesti, relIni.incod, "", ftpSession, pathDestiArrel)
                                strErr &= "16"
                            End If




                            ' GAIA.bdSR(objConn, "UPDATE METLREL SET RELDSFIT='" + (pathDestiArrel.replace("\","/").Replace("/GAIA06", "").Replace("/gaia", "").Replace("/GAIA", "").Replace("/joventut", "").Replace("/lh2010", "").Replace("/museu", "").Replace("/ccbobila", "").Replace("/ccteclasala", "").Replace("/arranzbravo", "").Replace("/EMCA", "").Replace("/seuelectronica", "").Replace("/ccSantJosep", "").Replace("/promocio", "").Replace("/ccct", "").Replace("/cooperacio", "").Replace("/xarxasida", "") + fDesti).Replace("/webs/auditoribarradas", "").Replace("/webs/teatrejoventut", "").Replace("/lhon", "").replace("/TorreBarrina","").replace("/pmc","").replace("/pmlh","").replace("/pmsf","") + "' WHERE RELINCOD=" + relIni.incod.ToString())
                            GAIA2.bdSR(objConn, "UPDATE METLREL SET RELDSFIT='" & fDesti.Replace("/webs/teatrejoventut", "") & "' WHERE RELINCOD=" & relIni.incod.ToString())
                            strErr &= "17"
                            GAIA2.bdr(objConn, "SELECT * FROM METLREI  WITH(NOLOCK) WHERE REIINCOD=" & relIni.incod & " AND REIINIDI=" & idioma, ds)
                            strErr &= "18"
                            If ds.Tables(0).Rows.Count > 0 Then

                                'pathDestiArrel = pathDestiArrel.Replace("\","/")
                                'pathDestiArrel = pathDestiArrel.Replace("/GAIA06", "").Replace("/gaia", "").Replace("/GAIA", "").Replace("/joventut", "").Replace("/lh2010", "").Replace("/museu", "").Replace("/ccbobila", "").Replace("/ccteclasala", "").Replace("/arranzbravo", "").Replace("/EMCA", "").Replace("/seuelectronica", "").Replace("/ccSantJosep", "").Replace("/promocio", "")
                                'pathDestiArrel = pathDestiArrel.Replace("/ccct", "").Replace("/xarxasida", "").Replace("/cooperacio", "").replace("/pmc","").replace("/pmlh","").replace("/pmsf","") 
                                'GAIA.bdSR(objConn, "UPDATE METLREI SET REIDSFIT='" & pathDestiArrel & fDesti.replace("\","/").Replace("/webs/auditoribarradas", "").Replace("/webs/teatrejoventut", "").Replace("/lhon", "").replace("/TorreBarrina","").replace("/pmc","").replace("/pmlh","").replace("/pmsf","") & "', REIDSFI2='" & fitxerTMP & "' , REIDSHAS='" & strHash & "' WHERE REIINCOD=" + relIni.incod.ToString() + " AND REIINIDI = " & idioma)

                                GAIA2.bdSR(objConn, "UPDATE METLREI SET REIDSFIT='" & fDesti.Replace("\", "/").Replace("/webs/teatrejoventut", "") & "', REIDSFI2='" & fitxerTMP & "' , REIDSHAS='" & strHash & "' WHERE REIINCOD=" + relIni.incod.ToString() + " AND REIINIDI = " & idioma)
                                strErr &= "19"
                            Else

                                Dim dataIni, dataFi As DateTime
                                GAIA2.datesPublicacio(objConn, relIni, idioma, dataIni, dataFi)
                                ' GAIA.bdSR(objConn, "INSERT INTO METLREI VALUES(" + relIni.incod.ToString() + "," + idioma.ToString() + ",'" + dataIni.ToString() + "','" + dataFi.ToString() + "','" + (pathDestiArrel.replace("\","/").Replace("/GAIA06", "").Replace("/gaia", "").Replace("/GAIA", "").Replace("/cooperacio", "").Replace("/joventut", "").Replace("/lh2010", "").Replace("/museu", "").Replace("/ccbobila", "").Replace("/ccct", "").Replace("/ccteclasala", "").Replace("/arranzbravo", "").Replace("/EMCA", "").Replace("/seuelectronica", "").Replace("/ccSantJosep", "").Replace("/promocio", "").Replace("/xarxasida", "") + fDesti).Replace("/webs/auditoribarradas", "").Replace("/webs/teatrejoventut", "").Replace("/lhon", "").replace("/TorreBarrina","").replace("/pmc","").replace("/pmlh","").replace("/pmsf","") + "','" + fitxerTMP + "','" & strHash & "')")
                                strErr &= "20"
                                Try
                                    strErr &= "a"
                                    GAIA2.bdSR(objConn, "INSERT INTO METLREI VALUES(" & relIni.incod & "," & idioma & ",'" & dataIni.ToString() & "','" & dataFi.ToString() & "','" & fDesti.Replace("/webs/teatrejoventut", "") & "','" & fitxerTMP & "','" & strHash & "')")
                                    strErr &= "b"
                                Catch
                                    GAIA2.debug(objConn, "publica: problema al fer aquesta acció: " & "INSERT INTO METLREI VALUES(" & relIni.incod & "," & idioma & ",'" & dataIni.ToString() & "','" & dataFi.ToString() & "','" & fDesti & "','" & fitxerTMP & "','" & strHash & "')")
                                End Try
                                strErr &= "21"
                            End If
                            If InStr(UCase(pathDestiArrel), "PROVES") > 0 Or servidor = "intranet" Then
                                File.Delete("c:\windows\temp\" + fitxerTMP)
                            End If
                        Else
                            GAIA2.debug(objConn, "Hi ha error al publicar la relació: " + relIni.incod.ToString() + ". Nom de fitxer buit")
                        End If
                    End If
                End If

                'Esborro tots els manteniments de la relacio/node amb data inferior a l'actual, i que no hagin estat publicats (MANDTTEM=0)
                GAIA2.bdSR(objConn, "DELETE FROM METLMAN WHERE MANDTTEM=0 AND (MANCDREL=" & relIni.incod & " OR MANCDNOD=" & relIni.infil & ") AND MANDTDAT<getdate()  AND MANCDIDI=" & idioma)

            End If


            'Si no hem pasat cap servidor destí com a paràmetre és per que és una publicació d'una única pàgina i per tant es pot tancar la sessió
            If servidor = "" Then

                ftpSession.Close()
            Else
                'no tanco la sessió de FTP. La tancaré dins del procés de manteniment de continguts.
            End If

            ds.Dispose()
        Catch ex As Exception
            f_logError(objConn, "G01. Publica.", ex.Source, "error en publica:  " & strErr & "-" & Err.Description)
            llistatErrors &= "<br/> " & ex.ToString & ". error en publica:  " & strErr & "-" & Err.Description

        End Try
        Return 1

    End Function 'publica

    '************************************************************************************************************************
    '	Funció: generarDatesPublicacio
    ' 	Paràmetres d'entrada:
    ' 		objConn: conexió
    ' 		codiNode: codi del node del qual es buscaran les relacions al que pertany
    ' 		idioma: idioma amb el que treballem
    ' 		datapublicacio: data de publicació a assignar
    '	 	datacaducitat: data de caducitat a assignar
    '	Procés:
    '		funció que genera les entrades corresponents a METLREI per un determinat node, concretament els camps
    '		de data de publicació i de caducitat.
    '*************************************************************************************************************************
    Public Shared Sub generarDatesPublicacio(ByVal objConn As OleDbConnection, ByVal codiNode As Integer, ByVal idioma As Integer, ByVal dataPublicacio As String, ByVal dataCaducitat As String)
        Dim sSQL As String, ds As DataSet, dbRow As DataRow, dbRow2 As DataRow, ds2 As DataSet
        ds = New DataSet
        sSQL = "SELECT RELINCOD FROM METLREL  WITH(NOLOCK) WHERE RELINFIL = " & codiNode

        GAIA2.bdr(objConn, sSQL, ds)
        'recorrem totes les relacions on trobem el node donat

        For Each dbRow In ds.Tables(0).Rows
            ds2 = New DataSet
            'busquem si existeix la entrada
            sSQL = "SELECT * FROM METLREI  WITH(NOLOCK) WHERE REIINCOD = " & dbRow("RELINCOD") & " AND REIINIDI = " & idioma

            GAIA2.bdr(objConn, sSQL, ds2)

            If ds2.Tables(0).Rows.Count = 0 Then
                'si no existeix l'entrada fem l'insert amb els nous valors

                sSQL = "INSERT INTO METLREI (REIINCOD,REIINIDI,REIDTPUB,REIDTCAD,REIDSFIT, REIDSFI2,REIDSHAS) VALUES " &
                "(" & dbRow("RELINCOD") & "," & idioma & "," & dataPublicacio.ToString() & "," & dataCaducitat.ToString() & ",'','','')"
                GAIA2.bdSR(objConn, sSQL)
            Else
                For Each dbRow2 In ds2.Tables(0).Rows
                    If dbRow2("REIDTPUB").ToString() <> dataPublicacio Or dbRow2("REIDTCAD").ToString() <> dataCaducitat Then
                        GAIA2.bdSR(objConn, "UPDATE METLREI SET REIDTPUB=" + dataPublicacio + ", REIDTCAD=" + dataCaducitat + " WHERE REIINCOD=" + dbRow2("REIINCOD").ToString() + " AND REIINIDI=" + dbRow2("REIINIDI").ToString())
                    End If

                Next dbRow2
            End If
            ds2.Dispose()
        Next dbRow

        ds.Dispose()

    End Sub 'generarDatesPublicacio


    '******************************************************************
    '	Funció: trobaPosicioEstructura
    '	Entrada: 
    '		codiRelacio: codi de la relació on es buscarà a quin lloc de l'estructura s'ha d'assignar
    '									la informació

    '	Procés: 	
    '		Busca dins la relació demanada i retorna el integer que apunta a la ubicació del node
    '		dins de l'estructura de la pàgina.
    '******************************************************************
    Public Shared Function trobaPosicioEstructura(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        trobaPosicioEstructura = 0

        GAIA2.bdr(objConn, "SELECT RELCDEST FROM METLREL  WITH(NOLOCK) WHERE RELINCOD=" + codiRelacio.ToString() + " AND RELCDSIT<98", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            trobaPosicioEstructura = dbRow("RELCDEST")
        End If
        DS.Dispose()
    End Function 'trobaPosicioEstructura



    '************************************************************************************************************
    '	Funció: posaFormat
    '	Entrada:  strContingut: string amb el contingut de la cel·la
    '						strCamp: nom del camp. 
    '						css: valor de l'estil que vull aplicar. ho busco a la taula METLCSS

    '						enllaç: URL on enllaçaré el contingut
    '						esImatge: Si cert, Mostraré el contingut dins de tags <img> 
    '						width: en el cas de ser una imatge, width és l'amplada màxima en que es pot representar el contingut
    '						tamany: és el número de pixels de la imatge (resultat de multiplicar el tamany horitzonal pel vertical).
    '										Si tamany està proper al del thumbnail, obriré el thumbnail en canvi de la imatge original.
    '						estat: estat del contingut al que posaré fotmat. Així si no és pot publicar (per que està caducat o pendent de publicar, el remarcaré amb l'estil 'brdAlerta'
    '						tancaEnllaç: Si false llavors no faré el </a>
    '	Procés:   Segons el tipus de contingut, el proceso, aplico l'estil css i el retorno
    '						Si l'estil CSS = 0 llavors no retorno el contingut del camp. Sempre ha d'haver un estil associat.
    '************************************************************************************************************	
    Public Shared Function posaFormat(ByVal objConn As OleDbConnection, ByVal strContingut As String, ByVal css As String, ByVal enllaç As String, ByVal esImatge As String, ByVal width As String, ByVal tamanyH As Integer, ByVal tamanyV As Integer, ByVal estat As Integer, ByVal textalternatiu As String, ByVal tancaEnllaç As Boolean, ByVal heretaPropietatsWeb As Integer, ByVal target As Integer, ByVal rel As clsRelacio, ByVal relini As clsRelacio, ByVal idioma As Integer, ByVal esEML As String, ByVal botoMesImatge As String, ByVal titolContingut As String, ByVal plt As clsPlantilla, ByVal pareTeEnllac As Boolean, ByRef cssGenerat As String) As String


        Dim strFormat As String
        Dim strCss, strCssTamanyText As String
        Dim desfaseHoritzontal As Integer

        Dim textAlternatiuExtes As String
        Dim strCssSenseFons As String = ""
        strCssTamanyText = ""
        posaFormat = ""
        strFormat = ""
        Dim posaFormatJS As String = ""
        Dim textAlternatiuImatge As String = ""
        Dim posaWidthGAIA As Integer = 1
        Dim posaFloatGAIA As Integer = 1
        Dim posaVoreres As String = ""
        Dim estilsHref As String = ""
        Dim imatgeSenseRatio As String = ""
        strCss = ""

        If width = 0 Then
            posaWidthGAIA = 0
        End If

        If esImatge = "1" And esEML = "S" Then
            posaWidthGAIA = 0
        End If
        textAlternatiuImatge = textalternatiu
        'Si és un enllaç el title del <a href ha de tenir el títol del enllaç
        If enllaç.Trim().Length > 0 Then
            If titolContingut.Length > 0 Then
                textalternatiu = GAIA2.netejaHTML(titolContingut)
            Else
                textalternatiu = GAIA2.obtenirTitolContingut(objConn, relini, idioma)
            End If
        Else

            textalternatiu = GAIA2.netejaHTML(textalternatiu.Trim())
            textAlternatiuImatge = textalternatiu

        End If


        textAlternatiuExtes = textalternatiu



        If css <> "" Then
            strCss = GAIA2.trobaEstilCSS(objConn, css, desfaseHoritzontal, heretaPropietatsWeb, strCssTamanyText, strCssSenseFons, esEML, posaWidthGAIA, "", posaFloatGAIA, estilsHref)

            Select Case estat
                Case ctPENDENTCADUCAR
                    strCss += " fonsCaducat"
                Case ctPENDENTPUBLICAR
                    strCss += " fonsPendent"
            End Select

            'Ho faig pels autolinks de les plantilles, ja que encara no tinc versió d'idiomes segons plantilla	


            Select Case idioma
                Case 1
                    strContingut = strContingut.Replace("m&aacute;s informaci&oacute;n", "m&eacute;s informaci&oacute;").Replace("m&aacute;s [+]", "m&eacute;s [+]").Replace("Leer m&aacute;s", "Llegir-ne m&eacute;s")
                Case 2
                    strContingut = strContingut.Replace("m&eacute;s informaci&oacute;", "m&aacute;s informaci&oacute;n").Replace("m&eacute;s [+]", "m&aacute;s [+]").Replace("llegir m&eacute;s", "leer m&aacute;s").Replace("Llegir-ne m&eacute;s", "Leer m&aacute;s")
                Case 3
                    strContingut = strContingut.Replace("m&eacute;s informaci&oacute;", "more information").Replace("m&eacute;s [+]", "more [+]").Replace("llegir m&eacute;s", "read more").Replace("llegir-ne m&eacute;s", "read more")
            End Select
            '************************************************************
            ' és una imatge
            '************************************************************
            If esImatge = "1" Then


                If InStr(strContingut, ".jpg") > 0 Or InStr(strContingut, ".swf") > 0 Or InStr(strContingut, "obreFitxer") > 0 Then
                    'si hi ha enllaç poso els tags
                    If enllaç.Trim().Length > 0 Then


                        If botoMesImatge <> "" And esImatge Then
                            textAlternatiuExtes = IIf(idioma = 1, "Veure imatge de ", "Ver Imagen de ") & textalternatiu
                            'Poso target=1 per que obri sempre la imatge amb el visor a una finestra nova
                            target = 1
                        End If
                        posaFormat = "<a href=""" + enllaç + """  target=""" & IIf(target = 1, "_blank", "_self") & """ title=""" & GAIA2.netejaHTML(textAlternatiuExtes) & IIf(target = 1, IIf(idioma = 1, " (nova finestra)", " (nueva ventana) "), "") & """ class=""mesImatges"">" & posaFormat
                        tancaEnllaç = True
                        'el strcss només l'he de posar si és una imatge (no un swf)					
                    End If
                    '******************************************************************
                    'és un flash
                    '******************************************************************
                    If InStr(strContingut, "swf") > 0 Then
                        posaFormat += "<object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0""  " + strCss + " alt=""" & GAIA2.netejaHTML(textalternatiu.Replace("""", "´").Trim()) & """><param name=""scale"" value=""ExactFit""/><param name=""movie"" value=""" + strContingut + """><param name=""quality"" value=""high""><embed src=""" + strContingut + """ quality=""high"" pluginspage=""http://www.macromedia.com/go/getflashplayer"" type=""application/x-shockwave-flash"" " + strCss + "><param name=""scale"" value=""ExactFit ""/></embed></object>"
                        '******************************************************************
                        '  és una imatge														
                        '******************************************************************					
                    Else
                        textalternatiu = textAlternatiuExtes & IIf(target = 1, IIf(idioma = 1, " (nova finestra)", " (nueva ventana) "), "")



                        If InStr(strCss, "ratio") > 0 Then
                            Dim crida As String()
                            crida = strContingut.Split("?")
                            Dim oCodParam As New lhCodParam.lhCodParam
                            strContingut = crida(0) & "?" & oCodParam.encriptar(oCodParam.desencriptar(crida(1)) & "&r=" & strCss.Substring(InStr(strCss, "ratio") + 4, 3))
                            imatgeSenseRatio = crida(0) & "?" & oCodParam.encriptar(oCodParam.desencriptar(crida(1)))
                            'elimino el text de la ratio

                            strCss = strCss.Substring(0, InStr(strCss, "ratio") - 1) & strCss.Substring(InStr(strCss, "ratio") + 7)
                        Else
                            imatgeSenseRatio = strContingut
                        End If
                        If (esEML = "N") Then
                            posaFormatJS = "<a href=""" & imatgeSenseRatio & """ target=""_blank"" title=""" & GAIA2.netejaHTML(textalternatiu) & """ ><img src=""" & strContingut & """ alt=""" & textAlternatiuImatge & """ " & strCss & IIf(width = 0, " width=""" + tamanyH.ToString() + """ height=""" + tamanyV.ToString() + """", "") & "/></a>"
                        End If
                        posaFormat = posaFormat & "<img src=""" + strContingut + """ alt=""" & textAlternatiuImatge & """ " & IIf(esEML = "S", strCss, strCss.Replace("=""", "=""border0 "))
                        If width = 0 Then
                            posaFormat &= " width=""" + tamanyH.ToString() + """ height=""" + tamanyV.ToString() + """"
                        Else
                            ' si és un correu i hi ha estils width, els poso directament a les imatges per evitar que l'exchange ho elimini
                            Try
                                If (esEML = "S") Then

                                    'strcss serà del tipus :  width:100px;height:200px --> retallo i em quedo amb el valor del width
                                    'Dim tmpMida As String
                                    'tmpMida = strCss.Substring(InStr(strCss, "width:") + 5)
                                    'tmpMida = tmpMida.Substring(0, InStr(tmpMida, "px") - 1)
                                    'posaFormat &= " width=""" & tmpMida & """ "
                                    posaFormat &= " width=""" & width & """ "



                                    If InStr(strCss, "height") > 0 Then
                                        Dim tmpMida As String
                                        tmpMida = strCss.Substring(InStr(strCss, "height:") + 6)
                                        tmpMida = tmpMida.Substring(0, InStr(tmpMida, "px") - 1)
                                        posaFormat &= " height=""" & tmpMida & """ "
                                    End If
                                End If
                            Catch

                            End Try
                        End If
                        posaFormat &= "/>"


                    End If

                    If enllaç.Trim().Length > 0 And tancaEnllaç Then
                        posaFormat += botoMesImatge + "</a>"
                        tancaEnllaç = False
                    End If
                Else
                    'hi ha un document que s'espera com a imatge i no ho és. De moment no el mostro.
                End If
            Else 'No és una imatge.
                If enllaç.Trim().Length > 0 Then

                    If textalternatiu = "" Then
                        textalternatiu = GAIA2.obtenirTitolContingut(objConn, relini, idioma)
                    End If
                    If InStr(enllaç, "obreFitxer") > 0 Or InStr(enllaç, "visorImatges") > 0 Then 'si és un document l'obro en una finestra nova.                           
                        estilsHref = strCss

                        strCss = ""
                        posaFormat = "<a href=""" & enllaç & """ " & estilsHref & " target=""_blank"" title=""" & GAIA2.netejaHTML(textalternatiu) & IIf(idioma = 1, " (nova finestra) ", " (nueva ventana) ") & """>" & strContingut & "</a>"


                    Else
                        If InStr(enllaç, "<a") > 0 Then
                            'agafo el href i el tracto
                            Dim tmpstr As String = enllaç
                            tmpstr = tmpstr.Substring(InStr(tmpstr, "href=") + 5)
                            tmpstr = tmpstr.Substring(0, InStr(tmpstr, """") - 1)
                            enllaç = tmpstr
                        End If
                        estilsHref = strCssSenseFons
                        strCss = ""

                        posaFormat = "<a href=""" & enllaç & """ " & estilsHref & " target=""" & IIf(target = 1, "_blank", "_self") & """ title=""" & GAIA2.netejaHTML(textalternatiu) & IIf(target = 1, IIf(idioma = 1, " (nova finestra)", " (nueva ventana) "), "") & """>" & strContingut & "</a>"



                    End If

                Else
                    posaFormat = strContingut
                End If
            End If
        End If


        Select Case idioma
            Case 1
                posaFormat = posaFormat.Replace("_2.aspx", "_1.aspx")
            Case Else
                posaFormat = posaFormat.Replace("_1.aspx", "_" & idioma & ".aspx")
        End Select

        'Poso el nou visor d'imatges i el tractament de javascript
        If esImatge = "1" And pareTeEnllac = False And esEML = "N" Then
            'Vaig a buscar totes les imatges del mateix nivell per la galeria
            If plt.vis = 1 Then
                posaFormat = posaVisorImatges(objConn, plt, rel, idioma, width, posaFormat, posaFormatJS, cssGenerat, strCss)
            End If

        End If

    End Function 'posaFormat

    Public Shared Function posaVisorImatges(ByVal objconn As OleDbConnection, ByVal plt As clsPlantilla, ByVal rel As clsRelacio, ByVal idioma As Integer, ByVal width As Integer, ByVal strHtml As String, ByVal posaFormatJs As String, ByRef cssGenerat As String, ByVal strcss As String) As String

        Dim htmlVisorImatges As String = ""
        Dim sSql As String = ""
        Dim r As New Random()
        Dim num As Integer = r.Next(999999999)
        If cssGenerat Is Nothing Then
            cssGenerat = ""
        End If

        If plt.vis = 1 Then
            Dim itemActual As Integer = 0
            Dim i As Integer = 0
            Dim llistaImatges As String = ""
            Dim dbrow As DataRow
            Dim dt As New DataSet
            Dim strContingut As String = ""

            sSql = " SELECT distinct  relAltresFotos.RELINFIL,docFotos.DOCDSTIT,docFotos.DOCDSFIT ,docFotos.DOCWNHOR,docFotos.DOCWNVER,docOriginal.DOCWNSIZ,docFotos.DOCDSALT, relAltresFotos.RELCDORD  FROM METLTDO as tipusOriginal WITH(NOLOCK) , METLDOC as docOriginal WITH(NOLOCK)  INNER JOIN METLREL as relFoto   WITH(NOLOCK) ON  relFoto.RELINFIL=docOriginal.DOCINNOD AND  relFoto.RELCDSIT <98  INNER JOIN METLREL as relAltresFotos  WITH(NOLOCK) ON relAltresFotos.RELCDHER like relFoto.RELCDHER AND relAltresFotos.RELCDSIT<98 INNER JOIN METLDOC as docFotos WITH(NOLOCK)  ON docFotos.DOCINNOD=relAltresFotos.RELINFIL AND docFotos.DOCINIDI=1  INNER JOIN METLTDO as tipusDocumentFotos  WITH(NOLOCK) ON  docFotos.DOCINTDO=tipusDocumentFotos.TDOCDTDO  AND tipusDocumentFotos.TDODSNOM LIKE '%image%'  WHERE(docOriginal.DOCINNOD = " & rel.infil & ") AND docOriginal.DOCINIDI=1 AND docOriginal.DOCINTDO=tipusOriginal.TDOCDTDO ORDER BY relAltresFotos.RELCDORD "
            GAIA2.bdr(objconn, sSql, dt)
            Dim strtmp As String = ""
            Dim hsImatges As New Hashtable()
            Dim oCodParam As New lhCodParam.lhCodParam
            Dim mida As String = ""
            For Each dbrow In dt.Tables(0).Rows
                If Not hsImatges.Contains(dbrow("RELINFIL")) Then

                    hsImatges.Add(dbrow("RELINFIL"), "1")
                    If dbrow("RELINFIL") = rel.infil Then
                        'strContingut = "/utils/obrefitxer.ashx?" & HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbrow("RELINFIL") & "&codiIdioma=" & idioma & mida))
                        If dt.Tables(0).Rows.Count > 1 Then
                            llistaImatges = llistaImatges & "<li>" & posaFormatJs & "</li>"
                        Else
                            llistaImatges = llistaImatges & posaFormatJs

                        End If
                    Else
                        'poso les noves imatges
                        Dim fitxer As String = ""

                        '   If width <= 200 And dbrow("DOCWNHOR") <> 0 Then 'el cas de width=0 l'utilitzem només per donar-li mida màxima disponible a la imatge
                        '      mida = "&t=P100"
                        '     fitxer = dbrow("DOCDSFIT").replace(".", "P100.")
                        ' Else
                        '    If width <= 700 Then
                        mida = "&t=P"
                        fitxer = dbrow("DOCDSFIT").replace(".", "P.")
                        '  ELSE
                        '	   fitxer = dbRow("DOCDSFIT")
                        '	   mida = "&t=imatgeGran"   
                        'End If
                        ' End If

                        '  If InStr(strcss, "ratio") > 0 Then
                        '     mida &= "&w=" & dbrow("DOCWNHOR") & "r=" & strcss.Substring(InStr(strcss, "ratio") + 4, 3)
                        ' End If
                        strContingut = "/utils/obrefitxer.ashx?" & HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbrow("RELINFIL") & "&codiIdioma=" & idioma & mida & "&f=" & fitxer))


                        If dbrow("docWNHOR") > 700 And width <> 0 Then
                            llistaImatges = llistaImatges & "<li class=""amaga""><a href=""" & strContingut & """ target=""_blank""><img src=""" & strContingut & """ alt=""" & dbrow("DOCDSTIT") & """" & " width=""700""/></a></li>"
                        Else
                            llistaImatges = llistaImatges & "<li class=""amaga""><a href=""" & strContingut & """ target=""_blank""><img src=""" & strContingut & """ alt=""" & dbrow("DOCDSTIT") & """" & IIf(width = 0, " width=""" & dbrow("DOCWNHOR") & """ height=""" & dbrow("DOCWNVER") & """", " width=""700"" ") & "/></a></li>"
                        End If
                    End If
                End If
            Next dbrow

            If dt.Tables(0).Rows.Count > 1 Then
                llistaImatges = "<ul>" & llistaImatges & "</ul>"
            End If
            If dt.Tables(0).Rows.Count > 0 Then
                If InStr(cssGenerat, "gallery" & num) <= 0 Then
                    'cssGenerat &= " .gallery" & num & " {margin:0; padding:0; float:left; width:100%; position:relative;} .gallery" & num & " div {position:absolute; left:-9999px; overflow:hidden; display:none} "
                    cssGenerat &= " .gallery" & num & "  {margin:0 !important; padding:0 !important; float:left; width:100%; position:relative;} .gallery" & num & "  div {position:absolute; left:-9999px; overflow:hidden; display:none}"
                    If dt.Tables(0).Rows.Count > 1 Then
                        'cssGenerat &= " .gallery" & num & " ul {list-style: none; margin:0; padding:0} .gallery" & num & " ul li {display: inline; margin:0 !important; padding:0 !important} .gallery" & num & " ul li.amaga {position:absolute; left:-9999px; overflow:hidden; display:none }  .gallery" & num & " ul li:after {content: "".""; display: block; height: 0; clear: both; visibility: hidden;} .gallery" & num & " ul li {display: inline-block;} * html .gallery" & num & " ul li {height: 1%;}  .gallery" & num & " ul li{display: block;}  .gallery" & num & " u li { background:transparent}"
                        cssGenerat &= " .gallery" & num & " {margin:0 !important; padding:0 !important; float:left; width:100%; position:relative;} .gallery" & num & " div {position:absolute; left:-9999px; overflow:hidden; display:none} .gallery" & num & " ul {list-style: none; margin:0 !important; padding:0 !important; } .gallery" & num & " ul li {display: inline; margin:0 !important; padding:0 !important;} .gallery" & num & " ul li.amaga {position:absolute; left:-9999px; overflow:hidden; display:none;}  .gallery" & num & "  ul li:after {content: "".""; display: block; height: 0; clear: both; visibility: hidden;} .gallery" & num & " ul li {display: inline-block;} * html .gallery" & num & "  ul li {height: 1%;} .gallery" & num & "  ul li {display: block;} .gallery" & num & "  ul li { background:transparent}"
                    End If
                End If
                'cssGenerat &= "--></style> "
                strtmp &= "<div class=""gallery" & num & """ id=""gal" & num & """  style=""display:none;"">" & llistaImatges & "</div>"
                llistaImatges = strtmp & "<script type=""text/javascript"">document.getElementById('gal" & num & "').style.display = 'block'; </script>"
            End If
            dt.Dispose()



            htmlVisorImatges = "<div id=""noscript" & num & """><script type=""text/javascript""><!-- self.resizeTo(648, 750); self.toolbar = 0; self.status = false;--></script>" & strHtml & "</div>" & llistaImatges
            htmlVisorImatges &= "<script type=""text/javascript"">senseJS('noscript" & num & "'); $(function() {$('#gal" & num & " a').lightBox({ fixedNavigation: true });});</script>"
        Else
            Return (strHtml)
        End If

        Return (htmlVisorImatges)
    End Function



    '************************************************************************************************************
    '	Funció: GAIA.PlantillaPerDefecte
    '	Entrada:  codiRelacio: codi de la relació que apunta a un node fill del que volem saber quina plantilla el representarà.
    '
    '	Procés: Busco la plantilla per defecte del node apuntat pel fill de "codiRelacio"
    '************************************************************************************************************
    Public Shared Function plantillaPerDefecte(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal idioma As Integer) As String
        If rel.incod = 0 Then
            plantillaPerDefecte = 0
        Else
            Dim ordre As Integer = rel.cdord

            Dim est As Integer = rel.cdest
            Dim codiRelacioAnt As Integer
            Dim plantilles As String
            Dim DS As DataSet
            Dim dbRow As DataRow
            DS = New DataSet()
            plantillaPerDefecte = ""
            If rel.inpla > 0 Then
                plantillaPerDefecte = rel.inpla
                If rel.inpla = 9 Then

                    While rel.tipdsdes <> "fulla web" And codiRelacioAnt <> rel.incod
                        codiRelacioAnt = rel.incod
                        rel = GAIA2.obtenirRelacioSuperior(objConn, rel)
                    End While
                    If rel.tipdsdes = "fulla web" Then
                        GAIA2.bdr(objConn, "SELECT WEBDSPLA FROM METLWEB  WITH(NOLOCK) WHERE WEBINNOD LIKE '" + rel.infil.ToString() + "' UNION SELECT WEBDSPLA FROM METLWEB2  WITH(NOLOCK) WHERE WEBINNOD LIKE '" + rel.infil.ToString() + "'", DS)
                        If DS.Tables(0).Rows.Count > 0 Then
                            dbRow = DS.Tables(0).Rows(0)
                            Dim arrayPla As String()
                            arrayPla = dbRow("WEBDSPLA").split(",")
                            plantilles = arrayPla(est)
                            arrayPla = plantilles.Split("|")
                            plantillaPerDefecte = arrayPla(ordre Mod arrayPla.Length)

                        End If
                    End If

                End If
            Else


                Dim descTipusNode, descTipusNodeSUP, strPlantilla As String
                Dim posicioEstructura As Integer
                Dim codiRelacio, codiRelacioPare, codiRelacioPlantilla As Integer
                Dim plantillaPare As Integer
                Dim strTmp As String()

                plantillaPerDefecte = 0




                strPlantilla = ""

                Dim relPare As New clsRelacio
                Dim relPlantilla As New clsRelacio

                Dim relAnt As New clsRelacio
                descTipusNodeSUP = ""
                codiRelacio = rel.incod
                codiRelacioPare = rel.incod
                relPare.copiaObj(rel)
                codiRelacioAnt = relAnt.incod ' = 0 
                'Busco el node "fulla web" superior. Si no trobo cap quan arribo a l'arbre (on relinfil=relinpar) hauré d'agafar
                'la plantilla per defecte
                descTipusNode = rel.tipdsdes
                While descTipusNodeSUP <> "fulla web" And descTipusNodeSUP <> "node web" And descTipusNodeSUP <> "node estructura" And codiRelacioAnt <> codiRelacioPare And strPlantilla = "" And codiRelacioPare <> 0
                    relAnt.copiaObj(relPare)
                    codiRelacioAnt = relAnt.incod

                    relPare = GAIA2.obtenirRelacioSuperior(objConn, relPare)
                    codiRelacioPare = relPare.incod
                    If codiRelacioPare = 0 Then
                        'Miro si aquesta relació apunta a una posició de l'estructura definida per una plantilla secundària

                        If codiRelacioAnt <> codiRelacioPare Then
                            plantillaPare = GAIA2.plantillaPerDefecte(objConn, relPare, idioma)
                            If plantillaPare > 0 Then
                                GAIA2.bdr(objConn, "SELECT PLTDSPLT,RELCDEST  FROM METLPLT WITH(NOLOCK) , METLREL  WITH(NOLOCK) WHERE RELINCOD=" + codiRelacio.ToString() + " AND  PLTINNOD=" + plantillaPare.ToString(), DS)

                                If DS.Tables(0).Rows.Count > 0 Then
                                    dbRow = DS.Tables(0).Rows(0)
                                    strTmp = dbRow("PLTDSPLT").split(",")

                                    If dbRow("RELCDEST") >= 0 Then
                                        If strTmp.Length > dbRow("RELCDEST") Then
                                            If strTmp(dbRow("RELCDEST")).Trim.Length > 0 Then
                                                plantillaPerDefecte = strTmp(dbRow("RELCDEST"))
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If


                        descTipusNodeSUP = relPare.tipdsdes
                    End If
                End While
                ' He trobat que hi ha un ascendent "fulla web" i agafo la plantilla que defineix.
                If descTipusNodeSUP = "fulla web" And plantillaPerDefecte = 0 Then

                    If relPare.inpla <> 0 Then

                        plantillaPerDefecte = relPare.inpla
                    Else
                        relPlantilla.copiaObj(relPare)
                        codiRelacioPlantilla = relPlantilla.incod

                        GAIA2.bdr(objConn, "SELECT WEBDSPLA FROM METLWEB WITH(NOLOCK) , METLREL WITH(NOLOCK)  WHERE RELINCOD=" + codiRelacioPlantilla.ToString() + " AND RELINFIL=WEBINNOD AND WEBINIDI=" + idioma.ToString() + " AND RELCDSIT<98 UNION SELECT WEBDSPLA FROM METLWEB2 WITH(NOLOCK) , METLREL WITH(NOLOCK)  WHERE RELINCOD=" + codiRelacioPlantilla.ToString() + " AND RELINFIL=WEBINNOD AND WEBINIDI=" + idioma.ToString() + " AND RELCDSIT<98", DS)
                        If DS.Tables(0).Rows.Count > 0 Then
                            dbRow = DS.Tables(0).Rows(0)
                            'a dbrow("WEBDSPLA") tinc una llista amb totes les plantilles que utilitza la pàgina web. 
                            'Amb RELCDEST apuntat per codirelacioAnterior tinc la posició en que es troba la plantilla dins de l'array webdspla
                            posicioEstructura = relAnt.cdest
                            If posicioEstructura >= 0 Then 'si no hi ha una posició dins de l'estructura no agafo el valor de la plantilla
                                strTmp = Split(dbRow("WEBDSPLA"), ",")
                                If strTmp.Length >= posicioEstructura Then
                                    If strTmp(posicioEstructura).Trim().Length = 0 Then
                                        plantillaPerDefecte = 0
                                    Else
                                        plantillaPerDefecte = strTmp(posicioEstructura)
                                    End If
                                End If 'strTmp.length>=posicioEstructura
                            End If 'posicioEstructura>=0 
                        End If 'ds.Tables(0).Rows.count>0 

                    End If 'relpare.inpla<>0
                Else
                    If descTipusNodeSUP = "node web" And plantillaPerDefecte = 0 Then

                        If relPare.inpla <> 0 Then
                            plantillaPerDefecte = relPare.inpla
                        Else
                            relPlantilla.copiaObj(relPare)

                            codiRelacioPlantilla = relPlantilla.incod

                            GAIA2.bdr(objConn, "SELECT NWEDSPLA FROM METLNWE WITH(NOLOCK) , METLREL WITH(NOLOCK)  WHERE RELINCOD=" + codiRelacioPlantilla.ToString() + " AND RELINFIL=NWEINNOD AND NWEINIDI=" + idioma.ToString() + " AND RELCDSIT<98", DS)
                            If DS.Tables(0).Rows.Count > 0 Then
                                dbRow = DS.Tables(0).Rows(0)

                                'a dbrow("WEBDSPLA") tinc una llista amb totes les plantilles que utilitza la pàgina web. 

                                'Amb RELCDEST apuntat per codirelacioAnterior tinc la posició en que es troba la plantilla dins de l'array webdspla
                                posicioEstructura = relAnt.cdest
                                If posicioEstructura >= 0 Then 'si no hi ha una posició dins de l'estructura no agafo el valor de la plantilla
                                    strTmp = Split(dbRow("NWEDSPLA"), ",")
                                    If strTmp.Length >= posicioEstructura Then
                                        If strTmp(posicioEstructura).Trim().Length = 0 Then
                                            plantillaPerDefecte = 0
                                        Else
                                            plantillaPerDefecte = strTmp(posicioEstructura)
                                        End If
                                    End If 'strTmp.length>=posicioEstructura
                                End If 'posicioEstructura>=0 
                            End If 'ds.Tables(0).Rows.count>0 
                        End If 'relpare.inpla<>0
                    End If
                End If


                If InStr(plantillaPerDefecte, "|") > 0 Then
                    plantillaPerDefecte = plantillaPerDefecte.Substring(0, InStr(plantillaPerDefecte, "|") - 1)
                End If
                ' Si encara no he assignat una plantilla busco la que li toca per defecte	
                If plantillaPerDefecte = 0 Then
                    If descTipusNode = "fulla document" Then
                        GAIA2.bdr(objConn, "SELECT TDOINPLA FROM METLTDO WITH(NOLOCK) , METLREL WITH(NOLOCK) , METLDOC WITH(NOLOCK)  WHERE RELINCOD=" & codiRelacio.ToString() & " AND RELINFIL=DOCINNOD  AND DOCINTDO=TDOCDTDO AND RELCDSIT<98", DS)
                        If DS.Tables(0).Rows.Count > 0 Then
                            dbRow = DS.Tables(0).Rows(0)
                            plantillaPerDefecte = dbRow("TDOINPLA")
                        End If
                    Else
                        GAIA2.bdr(objConn, "SELECT TBLINNOD FROM METLREL WITH(NOLOCK) ,METLNOD WITH(NOLOCK) , METLTBL WITH(NOLOCK)  WHERE RELINCOD=" & codiRelacio.ToString() & " AND RELINFIL=NODINNOD AND NODCDTIP=TBLINTFU AND RELCDSIT<98", DS)
                        If DS.Tables(0).Rows.Count > 0 Then
                            dbRow = DS.Tables(0).Rows(0)
                            plantillaPerDefecte = dbRow("TBLINNOD")
                        End If
                    End If
                End If


                If InStr(plantillaPerDefecte, "|") > 0 Then
                    plantillaPerDefecte = plantillaPerDefecte.Substring(0, InStr(plantillaPerDefecte, "|") - 1)
                End If



            End If
            DS.Dispose()
        End If

        If plantillaPerDefecte = "" Then
            plantillaPerDefecte = 0
        End If
    End Function         'plantillaPerDefecte										




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
                    GAIA2.bdr(objConn, "SELECT AWEDSTCO as tipusContingut,''  as plantillaAutoLink  FROM METLAWE WITH(NOLOCK)  WHERE AWEINNOD=" + nroNodeDesti.ToString() + " AND AWEINIDI=" + idioma.ToString(), DS)
                Case "node web"
                    GAIA2.bdr(objConn, "SELECT NWEDSTCO as tipusContingut,''  as plantillaAutoLink FROM METLNWE  WITH(NOLOCK) WHERE NWEINNOD=" + nroNodeDesti.ToString() + " AND NWEINIDI=" + idioma.ToString(), DS)

                Case "fulla web"
                    GAIA2.bdr(objConn, "SELECT WEBDSTCO as tipusContingut, ''  as plantillaAutoLink FROM METLWEB WITH(NOLOCK)  WHERE WEBINNOD=" + nroNodeDesti.ToString() + " AND WEBINIDI=" + idioma.ToString(), DS)
                Case Else ' si el destí no és una fulla web serà un objecte amb una plantilla per defecte o bé donada per una pag web.			
                    GAIA2.bdr(objConn, "SELECT PLTDSTCO AS tipusContingut, PLTCDPAL as plantillaAutoLink FROM METLPLT  WITH(NOLOCK) WHERE PLTINNOD=" + plantillaPerDefecte(objConn, rel, idioma).ToString(), DS)
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
                    relPare = obtenirRelacioSuperior(objConn, rel)
                    codiRelacioPare = relPare.incod
                    descTipusNodeSup = relPare.tipdsdes

                    If descTipusNodeSup = "fulla web" Then
                        assignaAutomaticamentCellaAL = trobaCellaAutolink(objConn, tipusNodeAMoure, relPare, forcarCella, idioma)
                    Else
                        'Si el pare no es una fulla web hem de fer:
                        trobats = 0
                        cont = 0
                        Dim DS2 As DataSet
                        Dim dbRow2 As DataRow
                        DS2 = New DataSet()
                        GAIA2.bdr(objConn, "SELECT PLTDSTCO AS tipusContingut, PLTCDPAL as plantillaAutoLink FROM METLPLT  WITH(NOLOCK) WHERE PLTINNOD=" + plantillaAL.ToString(), DS2)
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
                    End If
                End If
            Else
                assignaAutomaticamentCella = -2
            End If


            cellaAutolink = assignaAutomaticamentCellaAL

        End If
        DS.Dispose()
    End Function 'assignaAutomaticamentCella


    Public Shared Function trobaCellaAutolink(ByVal objConn As OleDbConnection, ByVal tipusNodeAMoure As Integer, ByVal rel As clsRelacio, ByVal forcarCella As Integer, ByVal idioma As Integer) As Integer
        Dim codiRelacioPare, codiRelacioAnt, codiTipusNodeSUP, codiTipusNode, codiTipusFULLAWEB As Integer

        Dim tmp As Integer

        Dim relPare As New clsRelacio
        Dim relAnt As New clsRelacio
        Dim relAL As New clsRelacio
        If (rel.cdsit = 96 Or rel.cdsit = 97 Or rel.cdsit = 95) Then
            trobaCellaAutolink = -1
        Else
            relPare.copiaObj(rel)
            codiRelacioPare = relPare.incod

            codiRelacioAnt = 0
            codiTipusFULLAWEB = GAIA2.tipusNodeByTxt(objConn, "fulla web")

            codiTipusNode = rel.tipintip
            codiTipusNodeSUP = codiTipusNode
            While codiTipusNodeSUP <> codiTipusFULLAWEB And codiRelacioAnt <> codiRelacioPare
                relAnt.copiaObj(relPare)
                relPare = obtenirRelacioSuperior(objConn, relPare)
                codiTipusNodeSUP = relPare.tipintip
            End While
            If codiTipusNodeSUP = codiTipusFULLAWEB Then
                relAL = GAIA2.obtenirRelacioAL(objConn, relPare)
                trobaCellaAutolink = assignaAutomaticamentCella(objConn, tipusNodeAMoure, relAL, forcarCella, idioma, tmp, 0)
            Else
                trobaCellaAutolink = 0
            End If
        End If

    End Function 'trobaCellaAutolink


    '***************************************************************************************************
    '	Funció: GAIA.trobaEstilCSS
    '	Entrada: codiEstilCSS: integer amb el codi de l'estil a cercar
    '					cerca a la taula METLCSS el valor CSSDSTXT apuntat PER una llista de codis a CodiEstilCSS
    '					desfaseHoritzontal: per referencia. Camp CSSDSTHO. Tamany en pixels que ocupen els marges i els paddings.

    '					posaWidthGAIA: per defecte=1, segons el valor CSSWNWIG indicarà si cal posar automàticament els width i float de GAIA o no.
    '	Retorna els estils menys els de tipus tamany del text.
    '***************************************************************************************************
    Public Shared Function trobaEstilCSS(ByVal objConn As OleDbConnection, ByVal codiEstilCss As String, ByRef desfaseHoritzontal As Integer, ByVal heretaPropietatsWeb As Integer, ByRef strCSSAmbtamanyText As String, ByRef strCSSSenseFons As String, ByVal esEMl As String, ByRef PosaWidthGAIA As Integer, ByRef posaVoreres As String, ByRef posaFloatGaia As Boolean, Optional ByRef estilsHref As String = "") As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        trobaEstilCSS = ""
        strCSSAmbtamanyText = ""
        strCSSSenseFons = ""
        desfaseHoritzontal = 0
        posaVoreres = ""

        'Comprovo que codiEstilCss sigui diferent a 0,0,0,0,0,0.. si no hi ha cap estil dins de la cel·la no cal cercarlos.
        If codiEstilCss.Replace("0", "").Replace(",", "").Trim().Length <> 0 Then
            If codiEstilCss <> "undefined" Then

                GAIA2.bdr(objConn, "SELECT CSSINCOD,CSSWNWIG, CSSINTIP, CSSDSTHO,  CSSDSTHP,CSSDSTXT, CSSDSCSS, CSSWNFLT FROM METLCSS  WITH(NOLOCK) WHERE CSSINCOD IN (" + codiEstilCss + ")", DS)
                For Each dbRow In DS.Tables(0).Rows

                    desfaseHoritzontal += dbRow("CSSDSTHO")
                    PosaWidthGAIA = IIf(dbRow("CSSWNWIG") = 0, 0, PosaWidthGAIA)   'faig un AND. Si un dels estils té CSSWNWIG=0 retorno 0
                    posaFloatGaia = IIf(dbRow("CSSWNFLT") = 0, 0, posaFloatGaia)
                    If esEMl = "S" Then
                        If dbRow("CSSINTIP") = 24 Then 'css de tipus mida
                            If dbRow("CSSDSTXT").substring(0, 1) = "t" Then
                                strCSSAmbtamanyText += dbRow("CSSDSCSS").Trim() + " "
                            Else
                                trobaEstilCSS += dbRow("CSSDSCSS").Trim() + " "
                                strCSSSenseFons += dbRow("CSSDSCSS").Trim() + " "
                            End If
                        Else
                            If dbRow("CSSINTIP") <> 118 Then 'css de tipus fons
                                strCSSSenseFons += dbRow("CSSDSCSS").Trim() + " "
                            End If

                            If dbRow("CSSINTIP") = 23 Or dbRow("CSSINTIP") = 112 Then 'color o decoració
                                estilsHref &= dbRow("CSSDSCSS").Trim() + " "
                            End If

                            If dbRow("CSSINTIP") = 26 Then 'voreres arrodonides
                                posaVoreres = dbRow("CSSDSCSS").Trim() + " "
                            Else
                                trobaEstilCSS += dbRow("CSSDSCSS").Trim() + " "
                                strCSSAmbtamanyText += dbRow("CSSDSCSS").Trim() + " "
                            End If
                        End If


                    Else
                        If dbRow("CSSINTIP") = 24 Then 'css de tipus mida
                            If dbRow("CSSDSTXT").substring(0, 1) = "t" Or dbRow("CSSDSTXT").substring(0, 2) = "sc" Then
                                strCSSAmbtamanyText += dbRow("CSSDSTXT").Trim() + " "
                                '                                strCSSSenseFons += dbRow("CSSDSTXT").Trim() + " "
                            Else
                                If PosaWidthGAIA <> 0 Then
                                    trobaEstilCSS += dbRow("CSSDSTXT").Trim() + " "
                                End If
                            End If
                        Else
                            If dbRow("CSSINTIP") = 23 Or dbRow("CSSINTIP") = 112 Then 'color o decoració
                                estilsHref &= dbRow("CSSDSTXT").Trim() + " "
                            End If
                            If dbRow("CSSINTIP") <> 118 Then 'css de tipus fons
                                strCSSSenseFons += dbRow("CSSDSTXT").Trim() + " "
                            End If

                            If dbRow("CSSINTIP") = 26 Then 'voreres arrodonides
                                posaVoreres = dbRow("CSSDSTXT").Trim() + " "
                            Else
                                strCSSAmbtamanyText += dbRow("CSSDSTXT").Trim() + " "
                                trobaEstilCSS += dbRow("CSSDSTXT").Trim() + " "
                            End If
                        End If
                    End If
                Next dbRow


            End If
        End If

        If posaVoreres.Length > 0 Then
            If trobaEstilCSS.Length > 0 Then
                posaVoreres &= " " & trobaEstilCSS
                trobaEstilCSS = ""
                strCSSAmbtamanyText = ""
            End If
        End If

        ' Si es un correu poso style per que tindré totes les descripcions dels estils.
        If esEMl = "S" Then
            trobaEstilCSS = "  style=""" + trobaEstilCSS + """"

            If strCSSAmbtamanyText.Length > 0 Then
                strCSSAmbtamanyText = " style=""" + strCSSAmbtamanyText + """"
            End If

            If estilsHref.Length > 0 Then
                estilsHref = " style=""" & estilsHref & """"
            End If

            If strCSSSenseFons.Length > 0 Then
                strCSSSenseFons = " style=""" & strCSSSenseFons & """"
            End If
        Else
            trobaEstilCSS = " class=""" + trobaEstilCSS + """"

            If strCSSAmbtamanyText.Length > 0 Then
                strCSSAmbtamanyText = " class=""" + strCSSAmbtamanyText + """"
            End If

            If estilsHref.Length > 0 Then
                estilsHref = " class=""" & estilsHref & """"
            End If

            If strCSSSenseFons.Length > 0 Then
                strCSSSenseFons = " class=""" & strCSSSenseFons & """"
            End If
        End If



        DS.Dispose()
    End Function 'trobaEstilCSS


    '***************************************************************************************************
    '	Funció: GAIA.GetHTML
    '	Entrada: strPage: URL ("http://servidor/.../pag.html")
    '					Obro la pàgina web i retorno el contingut generat per la pàgina
    '***************************************************************************************************
    Public Shared Function GetHTML(ByVal objConn As OleDbConnection, ByVal strPage As String, Optional ByVal codiLCW As Integer = 0, Optional ByVal codiIdioma As Integer = 1, Optional ByVal dataLlibreria As DateTime = Nothing, Optional ByRef css As String = "", Optional ByVal relini As clsRelacio = Nothing, Optional ByVal rel As clsRelacio = Nothing, Optional ByVal est As String = "", Optional ByVal usuari As Integer = 0, Optional ByVal codiplantilla As Integer = 0, Optional ByVal width As Integer = 0) As String




        Dim strErr As String = ""

        Dim objResponse As WebResponse = Nothing
        Dim objRequest As WebRequest = Nothing
        Dim result As String = ""
        Dim sr As Object = Nothing
        Dim dataCaducitat As DateTime
        GetHTML = ""
        Dim text As System.IO.Stream
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Try
            If est = "-1" Then 'estic cercant una fulla sencera
                strErr &= "1"
                GAIA2.bdr(objConn, "SELECT * FROM METLCEL WITH(NOLOCK) WHERE CELINREL=" & rel.incod & " AND CELCDEST=" & est & " AND CELCDPLT=" & codiplantilla & " AND CELCDUSU=" & usuari & " AND  CELINIDI=" & codiIdioma, DS)
                strErr &= "2"
                If DS.Tables(0).Rows.Count > 0 Then
                    strErr &= "3"
                    GetHTML = DS.Tables(0).Rows(0)("CELDSEXE") & " " 'afegeixo un " " per que hi ha cel·les sense contingut i ho he de distinguir del cas que no s'ha trobat. Posteriorment eliminaré els espais.
                    css &= DS.Tables(0).Rows(0)("CELDSCSS")
                    Return (GetHTML)
                End If
            Else

                strErr &= "4"
                If codiLCW <> 0 Then
                    '************************************************************
                    ' Busco si la libreria de codi està materialitzada
                    '************************************************************
                    'GAIA.bdR(objConn, "select * FROM METLLCE WITH(NOLOCK) WHERE LCEINNOD=" & codiLCW & " AND LCEINIDI=" & codiIdioma & " AND LCEINLCW like '" & strPage & "'", DS)	
                    strErr &= "5"
                    GAIA2.bdr(objConn, "SELECT * FROM METLCEL WITH(NOLOCK) WHERE CELINPAR LIKE '" & strPage & "'", DS)
                    strErr &= "6"
                    If DS.Tables(0).Rows.Count > 0 Then
                        strErr &= "7"
                        dbRow = DS.Tables(0).Rows(0)
                        'comprovo la data del fitxer, si és més nou que la materialització, esborro totes les LCE i continuo generant la que necessito ara.
                        If dbRow("CELDTCAD") <> "1/1/1900" And Now > dbRow("CELDTCAD") Then
                            If codiLCW <> 0 Then
                                GAIA2.esborrarCelles(Nothing, "CELINLCW=" & codiLCW)
                                'GAIA.bdSR(objConn, "DELETE FROM METLCEL WHERE CELINLCW=" & codiLCW)
                            End If
                        Else

                            If DateDiff(DateInterval.Second, dbRow("CELDTFEC"), dataLlibreria) < 0 Then
                                GetHTML = dbRow("CELDSEXE")
                                css = dbRow("CELDSCSS")
                                Return (GetHTML)
                            Else
                                If codiLCW <> 0 Then
                                    GAIA2.esborrarCelles(Nothing, "CELINLCW=" & codiLCW)
                                    '.bdSR(objConn, "DELETE FROM METLCEL WHERE CELINLCW=" & codiLCW)
                                End If
                            End If
                        End If


                    End If
                Else
                    'si la crida no ve d'una llibreria de codi web, vindrà d'un dibuixapreviewrec per materialitzar continguts arrossegats
                    strErr &= "8"
                    If InStr(strPage, "contingutCella") > 0 Then
                        strErr &= "9"
                        GAIA2.bdr(objConn, "SELECT * FROM METLCEL WITH(NOLOCK) WHERE CELINPAR='" & strPage & "'", DS)
                        strErr &= "a"
                        If DS.Tables(0).Rows.Count > 0 Then
                            dbRow = DS.Tables(0).Rows(0)
                            If dbRow("CELDTCAD") <> "1/1/1900" And Now > dbRow("CELDTCAD") Then
                                If codiLCW <> 0 Then
                                    GAIA2.esborrarCelles(Nothing, "CELINLCW=" & codiLCW)
                                    'GAIA.bdSR(objConn, "DELETE FROM METLCEL WHERE CELINLCW=" & codiLCW)
                                End If
                            Else

                                GetHTML = dbRow("CELDSEXE") & " " 'afegeixo un " " per que hi ha cel·les sense contingut i ho he de distinguir del cas que no s'ha trobat. Posteriorment eliminaré els espais.
                                css &= dbRow("CELDSCSS")
                            End If
                        Else
                            GetHTML = ""
                        End If

                        Return (GetHTML)
                    End If
                End If
                strErr &= "j"
                If InStr(strPage, "http") > 0 Then
                    objRequest = System.Net.HttpWebRequest.Create(strPage.Replace("\", "/"))
                Else
                    objRequest = System.Net.HttpWebRequest.Create("http://lhintranet/GAIA/aspx/llibreriaCodiWeb/" & strPage.Replace("\", "/"))

                End If
                strErr &= "k"
                objRequest.Credentials = System.Net.CredentialCache.DefaultCredentials
                objRequest.Timeout = 1000000  'mil segons, per defecte eren cent segons
                Try
                    objResponse = objRequest.GetResponse()
                Catch
                    objResponse = Nothing
                End Try

                If Not objResponse Is Nothing Then
                    strErr &= "l"
                    text = objResponse.GetResponseStream()
                    sr = New StreamReader(text)
                    If sr.Peek() <> -1 Then
                        result = sr.ReadToEnd()
                        GetHTML = result 'httpUtility.htmldecode(result)
                    End If

                    strErr &= "m"
                    dataCaducitat = "1/1/1900"
                    'busco la data de caducitat més propera dels continguts afectats
                    If codiLCW <> 0 Then
                        strErr &= "b"
                        GAIA2.bdr(objConn, "select top 1 (CASE WHEN (REIDTCAD>GETDATE() AND REIDTPUB>GETDATE()) THEN CASE WHEN  REIDTCAD<REIDTPUB THEN REIDTCAD ELSE REIDTPUB END  ELSE CASE WHEN  REIDTCAD>GETDATE() THEN REIDTCAD ELSE CASE WHEN  REIDTPUB> getdate() THEN REIDTPUB ELSE GETDATE() END  END  END)  as data  FROM METLASS WITH(NOLOCK),METLREI WITH(NOLOCK),METLREL WITH(NOLOCK) WHERE ASSCDTPA=41 AND ASSCDNOD=" & codiLCW & " AND RELCDHER LIKE ('%' + cast( ASSCDNRL  as varchar(10)) + '%') AND REIINCOD=RELINCOD AND REIINIDI=1  AND REIDTCAD>'1/1/1900' AND RELCDSIT<98 order by data ", DS)
                        strErr &= "c"


                        If DS.Tables(0).Rows.Count > 0 Then
                            dataCaducitat = DS.Tables(0).Rows(0)("data")
                        End If
                        'guardo el html resultant en METLCEL

                        Dim posini As Integer
                        Dim posfi As Integer
                        'trec els <style type="text/css"><!-- ... del html
                        posini = InStr(GetHTML, "<style type=""text/css""><!--")
                        While posini > 0
                            posfi = InStr(GetHTML, "--></style>") - 1

                            If posfi > 0 Then
                                css &= GetHTML.Substring(posini + 26, posfi - (posini + 26))
                                GetHTML = GetHTML.Substring(0, posini - 1) + GetHTML.Substring(posfi + 11)
                            End If
                            posini = InStr(GetHTML, "<style type=""text/css""><!--")
                        End While
                        If codiplantilla <> 0 Or codiLCW <> 0 Then
                            Try
                                strErr &= "d"
                                GAIA2.bdSR(objConn, " IF NOT EXISTS (SELECT CELCDNOD FROM METLCEL WITH(NOLOCK) WHERE CELINPAR = '" & strPage & "')   INSERT INTO METLCEL VALUES (" & codiLCW & "," & codiIdioma & "," & relini.incod & "," & rel.incod & "," & est & "," & usuari & "," & rel.infil & "," & codiplantilla & ",getdate(),'" & dataCaducitat & "','" & css & "','" & GetHTML.Replace("'", "''") & "','" & strPage & "') ELSE 	UPDATE METLCEL SET CELDSEXE='" & GetHTML.Replace("'", "''") & "', CELDSCSS='" & css & "' WHERE  CELCDEST=" & est & " AND CELCDUSU=" & usuari & " AND CELINREI=" & relini.incod & " AND CELINREL=" & rel.incod & " AND CELINLCW=" & codiLCW & " AND CELINIDI=" & codiIdioma)
                                strErr &= "e"
                            Catch
                            End Try



                            ' Try
                            'GAIA.bdSR(objConn, "INSERT INTO METLCEL VALUES (" & codiLCW & "," & codiIdioma & "," & relini.incod & "," & rel.incod & "," & est & "," & usuari & "," & rel.infil & "," & codiplantilla & ",getdate(),'" & dataCaducitat & "','" & css & "','" & GetHTML.Replace("'", "''") & "','" & strPage & "')")
                            ' Catch
                            'ja existia
                            'GAIA.bdSR(objConn, "UPDATE METLCEL SET CELDSEXE='" & GetHTML.Replace("'", "''") & "', CELDSCSS='" & css & "' WHERE CELINPAR like '" & strPage & "'")
                            'End Try
                        End If
                    End If
                End If

            End If
            strErr &= "m"
        Catch ex As Exception
            f_logError(objConn, "G01", ex.Source, strErr & ex.Message & ".Fitxer=" & strPage.ToString())
            llistatErrors &= "<br/> " & ex.ToString & ". <br/> Fitxer=<a href=""" & strPage.ToString() & """>" & strPage & "</a>"

        Finally
            If Not (sr Is Nothing) Then
                sr.close()
            End If
            If Not (objResponse Is Nothing) Then
                sr.close()
            End If
            DS.Dispose()
        End Try
    End Function 'GetHTML


    '***************************************************************************************************
    '	Funció: GAIA.URLEncode
    '	Entrada: txt: text que volem codificar
    '					codifico en format URLEncode
    '***************************************************************************************************
    Public Shared Function aURLEncode(ByVal txt As String) As String

        Dim i As Integer

        Dim ch As String
        Dim ch_asc As Integer
        Dim result As String
        Dim m_SafeChar(255) As Boolean

        result = ""
        For i = 1 To Len(txt)
            ' Translate the next character.
            ch = Mid$(txt, i, 1)
            ch_asc = Asc(ch)
            If ch_asc = 20 Then
                ' Use a plus.
                result = result & "+"
            ElseIf ((ch_asc >= 48 And ch_asc <= 57) Or (ch_asc >= 65 And ch_asc <= 90) Or (ch_asc >= 97 And ch_asc <= 122)) Then
                ' Use the character.
                result = result & ch
            Else ' Convert the character to hex.
                result = result & "%" & Right("0" + Hex$(ch_asc).ToString(), 2)
            End If
        Next i
        aURLEncode = result
    End Function 'URLEncode


    '***************************************************************************************************
    '	Funció: GAIA.Log
    '
    '***************************************************************************************************

    Public Shared Sub log(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiUsuari As String, ByVal descripcio As String, ByVal tipusAccio As Integer)
        GAIA2.log(objConn, rel, codiUsuari, descripcio, tipusAccio, 0)
    End Sub

    Public Shared Sub log(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiUsuari As String, ByVal descripcio As String, ByVal tipusAccio As Integer, ByVal node As Integer)


        Dim descripcioObjecteLog As String
        descripcioObjecteLog = ""



        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()

        If rel.incod = 0 Then
            GAIA2.bdr(objConn, "SELECT * FROM METLNOD WITH(NOLOCK)  WHERE NODINNOD=" & node, DS)
            If DS.Tables(0).Rows.Count > 0 Then
                dbRow = DS.Tables(0).Rows(0)
                descripcioObjecteLog = dbRow("NODDSTXT")
            End If

        Else
            If rel.cdher.Length > 0 Then
                GAIA2.bdr(objConn, "SELECT * FROM METLNOD WITH(NOLOCK) ,METLREL  WITH(NOLOCK) WHERE NODINNOD IN (" + rel.cdher.Substring(1).Replace("_", ",") + ") AND RELINFIL=NODINNOD ORDER BY LEN(RELCDHER)", DS)
                For Each dbRow In DS.Tables(0).Rows
                    descripcioObjecteLog += ">" + dbRow("NODDSTXT")
                Next dbRow
            End If
            descripcioObjecteLog += ">" + rel.noddstxt
            node = rel.infil
        End If

        descripcio = GAIA2.obtenirTextCodi(objConn, 2, tipusAccio) + " sobre el node: " + descripcioObjecteLog + IIf(descripcio.Length > 0, " --> ", " ") + descripcio

        GAIA2.bdSR(objConn, "INSERT INTO METLLOG (LOGCDREL, LOGDTDAT, LOGTPTIP, LOGCDUSR, LOGDSTXT, LOGINNOD) VALUES (" & rel.incod & ",'" + Now.ToString() + "'," + tipusAccio.ToString() + ",'" + codiUsuari + "','" + descripcio.Replace("'", "''") + "'," + node.ToString() + ")")

        DS.Dispose()
    End Sub 'log




    '***************************************************************************************************
    '	Funció: GAIA.obtenirTextCodi
    '	Entrada: tipusCodi: Tipus de codi 
    '					 codi: Número de codi
    '					cerca a la taula METLCOD el valor CODDSTXT apuntat per CODINTIP=tipusCODI i CODINCOD=codi
    '***************************************************************************************************
    Public Shared Function obtenirTextCodi(ByVal objConn As OleDbConnection, ByVal tipusCodi As Integer, ByVal codi As Integer) As String
        Dim DS As DataSet
        Dim dbRow As DataRow

        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT CODDSTXT FROM METLCOD  WITH(NOLOCK) WHERE CODINTIP=" + tipusCodi.ToString() + " AND CODINCOD=" + codi.ToString(), DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirTextCodi = dbRow("CODDSTXT").Trim()
        Else
            obtenirTextCodi = ""
        End If
        DS.Dispose()
    End Function 'obtenirTextCodi


    '***************************************************************************************************
    '	Funció: GAIA.obtenirCodiCircuitFullaWeb
    '	Entrada: codiNode: node que apunta a la fulla web
    '					cerca a la taula METLWEB el valor corresponent al node que apunta als nodes circuit de publicació
    '					i caducitat.
    '***************************************************************************************************
    Public Shared Sub obtenirCodiCircuitFullaWeb(ByVal objConn As OleDbConnection, ByVal codiNode As Integer, ByVal codiIdioma As Integer, ByRef circuitPublicacio As Integer, ByRef circuitCaducitat As Integer)
        Dim DS As DataSet
        Dim dbRow As DataRow
        circuitPublicacio = 0

        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT WEBDSCIP,WEBDSCIC  FROM METLWEB  WITH(NOLOCK) WHERE WEBINNOD=" + codiNode.ToString() + " AND WEBINIDI=" + codiIdioma.ToString(), DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            If dbRow("WEBDSCIP") > 0 Then
                circuitPublicacio = dbRow("WEBDSCIP")
            End If
            If dbRow("WEBDSCIC") > 0 Then
                circuitCaducitat = dbRow("WEBDSCIC")
            End If
        End If
        DS.Dispose()
    End Sub 'obtenirCodiCircuitFullaWeb


    '***************************************************************************************************
    '	Funció: GAIA.obtenirPasDeCircuitRelacio
    '	Entrada: codiRelacio: codi de relació sobre la que cercarem si està en algun circuit
    '					 cerca a la taula PCRINCOD el valor corresponent al node circuit.
    '***************************************************************************************************
    Public Shared Function obtenirPasDeCircuitRelacio(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio) As clsRelacio
        Dim DS As DataSet
        Dim dbRow As DataRow

        obtenirPasDeCircuitRelacio = New clsRelacio
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT PCRCDFCI FROM METLPCR  WITH(NOLOCK) WHERE PCRINCOD=" + rel.incod.ToString(), DS)
        If DS.Tables(0).Rows.Count > 0 Then

            dbRow = DS.Tables(0).Rows(0)
            obtenirPasDeCircuitRelacio.bdget(objConn, dbRow("PCRCDFCI"))
        End If
        DS.Dispose()
    End Function 'obtenirPasDeCircuitRelacio




    '***************************************************************************************************
    '	Funció: GAIA.tractarPasCircuit
    '	Entrada: 	codiRelacioContingut: codi de la relació que apunta a un contingut que inicia l'acció de cridar al pas del circuit
    '						codiRelacioCircuit	: codi de la relació que apunta a la fulla del circuit
    '	Acció: Tracta un pas de circuit que ja existeix i realitza alguna acció si s'escau.
    '				 De moment aquesta acció serà la d'enviar una alerta a l'administrador dels continguts en
    '				cas que el pas hagi superat el valor FCIWNTMD(dies) + FCIWNTMH (hores) en standby
    '***************************************************************************************************
    Public Shared Sub tractarPasCircuit(ByVal objConn As OleDbConnection, ByVal relContingut As clsRelacio, ByVal relCircuit As clsRelacio)

        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim hores As Integer
        Dim codiRelacioContingut, codiRelacioCircuit As Integer

        codiRelacioCircuit = relCircuit.incod
        codiRelacioContingut = relContingut.incod

        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT FCIWNTMD, FCIWNTMH, PCRDTDAT, PCRCDRCI FROM METLFCI WITH(NOLOCK) , METLPCR WITH(NOLOCK) , METLREL WITH(NOLOCK)  WHERE RELINCOD=" + codiRelacioCircuit.ToString() + " AND PCRINCOD=" + codiRelacioContingut.ToString() + " AND FCIINNOD=RELINFIL  AND RELCDSIT<98", DS)

        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            hores = dbRow("FCIWNTMH") + dbRow("FCIWNTMD") * 24

            If DateDiff("n", CDate(dbRow("PCRDTDAT")), Now) >= hores Then 'S'ha superat el temps màxim en aquest pas
                Dim arrayDestinataris(0) As Integer
                arrayDestinataris(0) = 49730
                GAIA2.enviaMissatge(objConn, relContingut, "S'ha superat el temps màxim d'espera", "Compte: s'ha superat el temps màxim en aquest pas", codiRelacioCircuit, "N", "N", "S", "S", arrayDestinataris, dbRow("PCRCDRCI"))
            End If
        End If
        DS.Dispose()
    End Sub 'tractarPasCircuit



    '***************************************************************************************************
    '	Funció: GAIA.iniciarCircuit
    '	Entrada: 	codiRelacioContingut: codi de la relació que apunta al contingut que entra en el circuit
    '					 	codiRelacioNodeCircuit: codi de la relació que apunta al codi del node circuit que volem iniciar
    '					 	idioma: codi de l'idioma del circuit. 
    '					 	codiUsuari: codi de l'usuari que ha iniciat el circuit. Si =0 llavors s'ha iniciat automàticament
    '											 des de l'Agent GAIA.
    '						codiRelacioNodePasCircuit: code de la relacio que apunta al node amb el pas del circuit en el que
    '																			 es troba el contingut. Si =0 llavors busquem el primer, sino prenem aquest
    '						codiRelacioContingutIniciador: codi de la relacio que apunta al contingut q ha iniciat el procés per el qual
    '																				aquest contingut entra en el circuit. Per exemple, un document d'un contracte de contractació seria el "contingut", però la pàgina web on es mostra es el "contingutIniciador"
    '	Acció: 		Inicia un pas del circuit, (el primer si codiRelacioNodePasCircuit=0).
    '						i envia missatges a tots els usuaris aprovadors.
    '***************************************************************************************************
    Public Shared Sub iniciarCircuit(ByVal objConn As OleDbConnection, ByVal relContingut As clsRelacio, ByVal codiRelacioNodeCircuit As Integer, ByVal codiUsuari As Integer, ByVal codiRelacioNodePasCircuit As Integer, ByVal relContingutIniciador As clsRelacio)

        Dim codiRelacioFullaCircuit, codiFullaCircuit As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim misTitolCircuit, misAprovacioPeticionari, misSolicitudAprovacio, misCancelacioPeticionari, misModificacioPeticionari As String
        Dim codiRelacioContingut, codiRelacioContingutIniciador As Integer
        Dim relFullaCircuit As New clsRelacio

        codiRelacioContingut = relContingut.incod


        codiRelacioContingutIniciador = relContingutIniciador.incod
        misTitolCircuit = ""
        misAprovacioPeticionari = ""
        misSolicitudAprovacio = ""
        misCancelacioPeticionari = ""
        misModificacioPeticionari = ""
        DS = New DataSet()

        If codiRelacioNodePasCircuit = 0 Then
            'busco la primera fulla del circuit
            GAIA2.bdr(objConn, "SELECT METLFCI.*, relacioFullaCircuit.RELINCOD, relacioFullaCircuit.RELINFIL FROM METLREL as relacioNodeCircuit WITH(NOLOCK) , METLREL as relacioFullaCircuit WITH(NOLOCK) , METLFCI  WITH(NOLOCK) WHERE relacioNodeCircuit.RELINCOD=" + codiRelacioNodeCircuit.ToString() + " AND relacioFullaCircuit.RELCDHER like (relacioNodeCircuit.RELCDHER+ '_'+CAST (relacioNodeCircuit.RELINFIL AS VARCHAR)) AND relacioFullaCircuit.RELINFIL=FCIINNOD AND relacioFullaCircuit.RELCDSIT<98 AND relacioNodeCircuit.RELCDSIT<98 ORDER BY relacioFullaCircuit.RELCDORD ASC", DS)
        Else
            GAIA2.bdr(objConn, "SELECT METLFCI.*, relacioFullaCircuit.RELINCOD, relacioFullaCircuit.RELINFIL FROM  METLREL as relacioFullaCircuit WITH(NOLOCK) , METLFCI WITH(NOLOCK)  WHERE relacioFullaCircuit.RELINCOD=" + codiRelacioNodePasCircuit.ToString() + " AND relacioFullaCircuit.RELINFIL=FCIINNOD AND relacioFullaCircuit.RELCDSIT<98", DS)
        End If
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)

            relFullaCircuit.bdget(objConn, dbRow("RELINCOD"))
            codiRelacioFullaCircuit = relFullaCircuit.incod
            codiFullaCircuit = relFullaCircuit.infil
            'Creo el registre a METLPCR

            GAIA2.bdSR(objConn, "INSERT INTO METLPCR (PCRINCOD,PCRINORG,PCRDTDAT,PCRCDFCI, PCRCDRCI) VALUES (" + codiRelacioContingut.ToString() + "," + codiUsuari.ToString() + ",'" + Now.ToString() + "'," + relFullaCircuit.incod.ToString() + "," + codiRelacioContingutIniciador.ToString() + ")")

            GAIA2.trobarTextCircuit(objConn, relFullaCircuit, 0, misTitolCircuit, misSolicitudAprovacio, misAprovacioPeticionari, misCancelacioPeticionari, misModificacioPeticionari)

            '*********************************************
            'Envio missatges a tots els usuaris Aprovadors
            '*********************************************
            'busco fills		
            Dim relacions As Integer() = {}
            Dim arrayDestinataris As Integer() = {}

            Dim nroFills As Integer = 0

            trobaFills(objConn, relFullaCircuit, codiFullaCircuit, arrayDestinataris, nroFills, relacions, 0, 0)
            'envio el missatge				
            GAIA2.enviaMissatge(objConn, relContingut, misTitolCircuit, misSolicitudAprovacio, codiRelacioFullaCircuit, "S", "S", "N", "S", arrayDestinataris, relContingutIniciador)

        Else 'error no es pot crear el circuit perque no hi ha cap pas inicial
        End If

        DS.Dispose()
    End Sub 'iniciarCircuit



    '***************************************************************************************************
    '	Funció: GAIA.enviaMissatge
    '	Entrada: 
    '					+ codiRelacioIniciadora: codi de relació que apunta al contingut que ha provocat 
    '					 l'envio d'un missatge (per exemple per la publicació o la caducitat d'un contingut)
    '					+ titolMissatge: titol del missatge que volem enviar
    '					+ textMissatge: cos del missatge
    '					+ pasDeCircuit: codi de node que apunta al pas de circuit que ha originat el missatge. Si =0 llavors el missatge no 
    '													s'ha originat amb cap circuit
    '					+ botoAprovacio: 1 si hem de mostrar el botó d'aprovació de pas de circuit
    '					+ botoCancelacio: 1 si hem de mostrar el botó de cancelació de pas de circuit
    '					+ botoEsborrarMissatge: 1 si hem de permetre esborrar el missatge amb un botó
    '					+ botoEditarContingut: 1 si hem de permetre editar el contingut apuntat per "codiRelacioIniciadora"
    '					+ destinatari: Codi node organigrama del destinatari
    '					+ arrayDestinataris: llista de destinataris del missatge 
    '					+codiRelacioContingutIniciador: codi de la relacio que apunta al contingut que ha iniciat un circuit que provoca l'envio del missatge
    '	Acció: 
    '***************************************************************************************************
    Public Shared Sub enviaMissatge(ByVal objConn As OleDbConnection, ByVal relIniciadora As clsRelacio, ByVal titolMissatge As String, ByVal textMissatge As String, ByVal pasDeCircuit As Integer, ByVal botoAprovacio As String, ByVal botoCancelacio As String, ByVal botoEsborrarMissatge As String, ByVal botoEditarContingut As String, ByVal arrayDestinataris As Integer(), ByVal relContingutIniciador As clsRelacio)
        Dim tipusNode, codiNode, destinatari As Integer
        Dim descTipus As String = ""
        Dim codiRelacioIniciadora, codiRelacioContingutIniciador As Integer

        codiRelacioIniciadora = relIniciadora.incod
        codiRelacioContingutIniciador = relContingutIniciador.incod
        If arrayDestinataris.Length > 0 Then

            If arrayDestinataris(0) = 0 And arrayDestinataris.Length = 1 Then
                'És un missatge que ha de rebre l'agent GAIA i per lo tant no ho envio
            Else
                tipusNode = GAIA2.tipusNodeByTxt(objConn, "fulla missatge")
                'Inserto el node missatge		

                codiNode = GAIA2.insertarNode(objConn, tipusNode, titolMissatge + "." + relIniciadora.noddstxt, 0)


                GAIA2.bdSR(objConn, "INSERT INTO METLMIS(MISINNOD, MISINCOD, MISDSTIT, MISCDPCR, MISDTDAT, MISDSTXT, MISDSBAP, MISDSBCA, MISDSBES, MISDSBED, MISDSBLH, MISDSBLU,MISCDRCI) VALUES (" + codiNode.ToString() + "," + codiRelacioIniciadora.ToString() + ",'" + titolMissatge.Replace("'", "''") + "'," + pasDeCircuit.ToString() + ",'" + Now.ToString() + "','" + textMissatge.Replace("'", "''") + "','" + botoAprovacio + "','" + botoCancelacio + "','" + botoEsborrarMissatge + "','" + botoEditarContingut + "','',0," + codiRelacioContingutIniciador.ToString() + ")")

                For Each destinatari In arrayDestinataris
                    If destinatari <> 0 Then

                        tipusNode = tipusNodebyNro(objConn, destinatari, descTipus)
                        If descTipus = "fulla organigrama" Then
                            'Inserto el node fullaMissatges en l'arbre personal de l'usuari, a la carpeta missatges
                            GAIA2.insertaNodeArbrePersonal(objConn, GAIA2.tipusNodeByTxt(objConn, "fulla missatge"), codiNode, destinatari, "Missatges")
                        Else
                            If descTipus = "node organigrama" Then
                                'en principi no ho permeto
                            End If
                        End If
                    End If
                Next destinatari
            End If
        End If
    End Sub 'enviaMissatge


    '***************************************************************************************************
    '	Funció: GAIA.anularPasCircuit
    '	Entrada: rel: relació que apunta al contingut que es vol treure del circuit
    '	Acció: 
    '***************************************************************************************************
    Public Shared Sub anularPasCircuit(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio)


        GAIA2.bdSR(objConn, "DELETE FROM METLPCR WHERE PCRINCOD=" + rel.incod.ToString())
        'Busco tots els missatges associats a aquest pas del circuit i els esborro
        GAIA2.bdSR(objConn, "DELETE FROM METLREL WHERE RELINCOD IN (SELECT RELINCOD FROM METLREL,METLMIS WHERE RELINFIL=MISINNOD AND MISINCOD=" + rel.incod.ToString() + ")")
        GAIA2.bdSR(objConn, "DELETE FROM METLNOD WHERE NODINNOD IN (SELECT MISINNOD FROM METLMIS WHERE MISINCOD=" + rel.incod.ToString() + ")")
        GAIA2.bdSR(objConn, "DELETE FROM METLMIS WHERE MISINCOD=" + rel.incod.ToString())
    End Sub 'anularPasCircuit



    '***************************************************************************************************
    '	Funció: GAIA.pintarContingut
    '	Acció: 
    '			
    '***************************************************************************************************

    Public Shared Sub pintarContingut(ByVal objConn As OleDbConnection, ByRef plantilla As clsPlantilla, ByVal rel As clsRelacio, ByVal index As Integer, ByVal codiNode As Integer, ByVal width As String, ByRef llistaDocuments As String(), ByRef urlDesti As String, ByVal idioma As Integer, ByVal relIni As clsRelacio, ByVal estat As Integer, ByVal fdesti As String, ByVal publicar As Integer, ByVal heretaPropietatsWeb As Integer, ByVal codiUsuari As Integer, ByVal dataSimulacio As String, ByRef html As String, ByRef css As String, ByRef estilbody As String, ByVal llistaCND As String, ByVal esForm As String, ByVal esEML As String, ByVal esSSL As String, ByVal tagsMeta As String, ByRef htmlPeu As String, ByVal strCSSPantalla As String, ByVal strCSSImpressora As String, ByVal pareTeEnllac As Boolean)

        If rel.tipdsdes <> "node web" And rel.tipdsdes <> "fulla web" And rel.tipdsdes <> "arbre web" Then
            If plantilla.innod = 0 Then
                plantilla.bdget(objConn, plantillaPerDefecte(objConn, rel, idioma))
            End If
            If plantilla.innod > 0 Then

                Dim strvoid As String = ""
                Dim relPare As New clsRelacio
                Dim lcwANT As String = ""
                Dim lcwPOST As String = ""

                Dim camp, prefixCamp, esImatge, taula, clau, sqlTamany, strContingut, sqlTextAlternatiu, textalternatiu, strSQL, campFitxer, campTitolContingut, strSQLWHERE, strSQLWHEREIdioma As String
                Dim strSQLOrdre As String = "", enllaç As String = ""
                Dim pospunt As Integer
                Dim DS, ds2, ds3 As DataSet
                Dim dbRow, dbrow2, dbrow3 As DataRow
                Dim cont As Integer
                Dim tamanyH, tamanyV As Integer
                Dim idiomaDiferent As Integer = False
                Dim forçarCodiAnt As Integer
                Dim forçarCodiPost As Integer
                Dim celdaFormateada As String
                Dim nomFitxerPublicat As String = ""
                Dim tancaEnllaç As Integer
                Dim fitxerAutoLink As String
                Dim codiRelacio, codiRelacioInicial As Integer
                Dim dataIni, dataFi As DateTime
                Dim campUpdate As String = ""
                Dim target As Integer = 0
                Dim relarxiu As New clsRelacio
                Dim relEnllaç As New clsRelacio
                Dim codiNodeTMP As Integer = 0
                codiRelacio = rel.incod
                codiRelacioInicial = relIni.incod
                fitxerAutoLink = ""
                Dim desfaseHoritzontal As Integer = 0
                Dim botoMesImatge As String = ""
                Dim sqlTOP As String = ""
                sqlTextAlternatiu = ""
                Dim oCodParam As New lhCodParam.lhCodParam
                Dim plantillaTMP As New clsPlantilla
                campFitxer = ""
                strContingut = ""
                esImatge = ""
                Dim parametrePL As String = ""
                Dim item As String = ""

                Dim titolContingut As String = ""
                Dim altImatge As String = ""
                Dim posaWidthGAIA As Integer = 1
                Dim posafloatGAIA As Boolean = True
                Dim estilsCSSSenseFons As String = ""
                Dim strURL As String = ""
                'Dim a as new system.web.htpputility
                tancaEnllaç = False ' si true, tancarem el </a> al final de pintarContingut. Això ho utilitzaré per posar tot el contingut dins d'un ellaç (pels casos d'autolinks).
                celdaFormateada = ""
                cont = 0
                DS = New DataSet()
                ds2 = New DataSet()
                ds3 = New DataSet()
                strSQLWHERE = ""
                Dim codiRelacioArxiuPerVisorImatges As Integer = 0
                'Dim pathDocumentsXDefecte as string = "/aplics/GAIA/documents/docs/"
                Dim pathDocumentsXDefecte As String
                pathDocumentsXDefecte = "/gdocs/"


                If idioma > 1 Then
                    If plantilla.arrayAAL.Length > index Then
                        'Trec la referència a la pàgina en castellà i la poso de nou per garantir que no poso _2_2.aspx
                        plantilla.arrayAAL(index) = plantilla.arrayAAL(index).Replace("_" & idioma & ".aspx", ".aspx")
                        plantilla.arrayAAL(index) = plantilla.arrayAAL(index).Replace(".aspx", "_" & idioma & ".aspx")
                    End If
                End If

                If plantilla.arrayDSNUM.Length > index Then
                    If (plantilla.arrayDSNUM(index).Trim().Length > 0) Then
                        If plantilla.arrayDSNUM(index) <> "0" Then
                            sqlTOP = " TOP " + plantilla.arrayDSNUM(index).ToString() + " "
                        End If
                    End If
                End If

                '*******************************************************************
                ' POSO LA LLIBRERIA DE CODI WEB 
                '*******************************************************************				
                If plantilla.arrayLCW.Length > index Then
                    If (plantilla.arrayLCW(index).Trim().Length > 0) Then

                        For Each item In plantilla.arrayLCW(index).Split("|")
                            If width = "" Then
                                width = "0"
                            End If
                            lcwANT = lcwANT + GAIA2.trobaCodiWeb(objConn, plantilla, item, idioma, rel, relIni, forçarCodiAnt, index, "", width, codiUsuari, css, tagsMeta)
                            'Si el codi web s'ha de mostrar de totes formes, ja l'afegeixo 							
                            If forçarCodiAnt = 1 Then
                                'celdaFormateada+=lcwAnt
                            End If
                        Next item
                    End If
                End If

                '*******************************************************************
                'Preparo la select per trobar el contingut
                '*******************************************************************								
                If plantilla.arrayCampsPlantilla.Length > index Then
                    'Tracto el camp a reprentar si és d'alguna taula de GAIA. En cas contrari (Imatge/document) ho tracto apart
                    If (plantilla.arrayCampsPlantilla(index).IndexOf(".") >= 0) Then

                        pospunt = plantilla.arrayCampsPlantilla(index).IndexOf(".") + 1
                        camp = plantilla.arrayCampsPlantilla(index).Substring(pospunt)
                        prefixCamp = camp.Substring(0, 3)

                        enllaç = plantilla.arrayEnllaços(index).Substring(plantilla.arrayEnllaços(index).IndexOf(".") + 1)
                        textalternatiu = plantilla.arrayALT(index).Substring(plantilla.arrayALT(index).IndexOf(".") + 1).Replace("""", "´")

                        If camp = "DOCDSFIT" Or enllaç = "DOCUMENT" Or enllaç = "IMATGE" Then
                            campFitxer = "DOCDSFIT"
                        End If
                        esImatge = plantilla.arrayEsImatge(index)
                        taula = plantilla.arrayCampsPlantilla(index).Substring(0, pospunt - 1)

                        'afegeixo el titol del contingut a la select per si ho necessito (de moment només per fer un "alt" d'un enllaç

                        Select Case taula
                            Case "METLDOC"
                                If enllaç = "AUTO-ENLLAÇ" Then
                                    campFitxer = "DOCDSFIT"
                                    enllaç = "DOCDSFIT"
                                End If
                                campUpdate = ",DOCWNUPD, DOCDSLNK, TDODSNOM "
                                campTitolContingut = ",  DOCDSTIT as titolContingut, DOCINIDI as idioma,DOCWNHOR "
                                taula += " WITH(NOLOCK), METLTDO WITH(NOLOCK)"
                            Case "METLNOT"
                                campTitolContingut = ",  NOTDSTIT as titolContingut, NOTINIDI as idioma, NOTDSLNK AS link "
                            Case "METLINF"
                                campTitolContingut = ",  INFDSTIT as titolContingut, INFINIDI as idioma "

                            Case "METLAGD"
                                campTitolContingut = ",  AGDDSTIT as titolContingut, AGDINIDI as idioma "
                            Case "METLCON"
                                campTitolContingut = ",  CONDSDES as titolContingut, CONINIDI as idioma "
                            Case "METLFPR"
                                campTitolContingut = ", FPRDSNOM as titolContingut, FPRINIDI as idioma "
                            Case "METLLNK"
                                campTitolContingut = ", LNKDSTXT as titolContingut, LNKINIDI as idioma, RELINCOD "
                                taula &= " WITH(NOLOCK), METLREL WITH(NOLOCK) "
                                If enllaç = "DOCUMENT" Or enllaç = "IMATGE" Then
                                    taula &= ", METLDOC WITH(NOLOCK), METLTDO WITH(NOLOCK) "
                                    campUpdate = " ,DOCINNOD "
                                    If enllaç = "DOCUMENT" Then
                                        strSQLWHERE = "  AND  RELINPAR=LNKINNOD AND RELINFIL=DOCINNOD AND RELCDSIT=97 "
                                    Else
                                        strSQLWHERE = " AND RELINPAR=LNKINNOD AND RELINFIL=DOCINNOD AND RELCDSIT=96 "
                                    End If
                                    enllaç = "DOCDSFIT"
                                Else
                                    strSQLWHERE &= " AND RELINFIL=LNKINNOD "
                                End If
                            Case Else
                                campTitolContingut = ",  '' as titolContingut, 1  as idioma "
                        End Select

                        clau = prefixCamp + "INNOD"
                        ' Si és una imatge, llavors he de buscar el tamany vertical i l'horitzontal, per saber si he de posar la imatge en miniatura o la gran.
                        If esImatge = "1" And prefixCamp = "DOC" Then
                            sqlTamany = " , DOCWNHOR as 'tamanyH', DOCWNVER as 'tamanyV' "
                        Else
                            sqlTamany = "  "
                        End If
                        If textalternatiu.Trim().Length > 0 Then
                            sqlTextAlternatiu = " ," + textalternatiu
                        End If



                        If InStr(enllaç, "RELDSFIT") > 0 Then
                            taula += ", METLREL WITH(NOLOCK)"
                        Else
                            If sqlTOP.Length > 0 Then
                                taula += ", METLREL WITH(NOLOCK) "
                                strSQLWHERE = " AND " & clau & "= RELINFIL"
                            End If


                        End If


                        If enllaç.Trim().Length > 0 And enllaç <> "AUTO-ENLLAÇ" Then
                            strSQL = "SELECT " + sqlTOP + clau + "," + camp + campTitolContingut + campUpdate + "," + enllaç + sqlTamany + sqlTextAlternatiu + "  FROM " & taula
                            strSQLWHERE = " WHERE " & clau & "=" & codiNode & strSQLWHERE
                            strSQLWHEREIdioma = " AND " & prefixCamp & "INIDI=" & idioma
                        Else
                            strSQL = "SELECT " + sqlTOP + clau + "," + camp + campTitolContingut + campUpdate + sqlTamany + sqlTextAlternatiu + " FROM " + taula
                            strSQLWHERE = " WHERE " & clau & "=" & codiNode & strSQLWHERE
                            strSQLWHEREIdioma = " AND " & prefixCamp & "INIDI=" & idioma
                        End If

                        If InStr(enllaç, "RELDSFIT") > 0 Then
                            strSQLWHERE += " AND RELINFIL=" & prefixCamp & "INNOD AND RELINCOD=" & rel.incod
                        End If

                        'Afegeixo la comprobació de la data
                        'si és una simulació no comprobo que la data de publicació sigui la correcte
                        If CDate(Day(dataSimulacio) & "/" & Month(dataSimulacio) & "/" & Year(dataSimulacio)) < Now Then
                            strSQLWHERE += " AND (" + prefixCamp + "DTCAD>'" + dataSimulacio.ToString() + "' OR " + prefixCamp + "DTCAD='" + CDate("01/01/1900").ToString() + "')"
                        Else
                            strSQLWHERE += " AND  " + prefixCamp + "DTPUB<='" + dataSimulacio.ToString() + "' AND (" + prefixCamp + "DTCAD>'" + dataSimulacio.ToString() + "' OR " + prefixCamp + "DTCAD='" + CDate("01/01/1900").ToString() + "')"
                        End If
                        'si és un doc lligo amb la taula de tipus de documents
                        If InStr(taula, "METLDOC") > 0 Then
                            strSQLWHERE += " AND DOCINTDO=TDOCDTDO "
                        End If

                        If InStr(enllaç, "RELDSFIT") > 0 Then
                            strSQLOrdre = " ORDER BY RELINORD "
                        End If
                        If (sqlTOP.Length > 0) Then
                            strSQLOrdre = " ORDER BY RELCDORD "
                        End If






                        GAIA2.bdr(objConn, strSQL & strSQLWHERE & strSQLWHEREIdioma & strSQLOrdre, DS)

                        'Si no trobo el contingut en l'idioma dessijat ho busco en qualsevol altre idioma, ordenat per ordre d'idioma: català, castellà, anglès, i no importa la caducitat		
                        If DS.Tables(0).Rows.Count = 0 Then
                            strSQLOrdre = " ORDER BY " + prefixCamp + "INIDI"
                            If (sqlTOP.Length > 0) Then
                                strSQLOrdre += " ,RELCDORD "
                            End If
                            GAIA2.bdr(objConn, strSQL & strSQLWHERE & strSQLOrdre, DS)
                            idiomaDiferent = 1
                        End If

                        'TINC CONTINGUT														
                        If DS.Tables(0).Rows.Count > 0 Then
                            titolContingut = DS.Tables(0).Rows(0)("titolContingut")


                            If titolContingut = "" Then
                                titolContingut = GAIA2.obtenirTitolContingut(objConn, relIni, idioma)
                            End If

                            dataIni = CDate("01/01/1900")
                            dataFi = CDate("1/1/2050")
                            'Si no és un contingut codificat tinc en compte les dates de la relació
                            If Not rel.cdher.StartsWith("_57135") Then
                                ds2 = New DataSet()
                                GAIA2.bdr(objConn, "SELECT REIDTPUB,REIDTCAD FROM METLREI WITH(NOLOCK) WHERE REIINCOD=" & rel.incod, ds2)
                                If ds2.Tables(0).Rows.Count > 0 Then


                                    dbrow2 = ds2.Tables(0).Rows(0)
                                    dataIni = dbrow2("REIDTPUB")
                                    If dbrow2("REIDTCAD") = CDate("01/01/1900") Then
                                        dataFi = CDate("1/1/2050")
                                    Else
                                        dataFi = dbrow2("REIDTCAD")
                                    End If
                                End If
                                ds2.Dispose()
                            End If
                            If dataIni <= CDate(dataSimulacio) And dataFi > CDate(dataSimulacio) Then
                                dbRow = DS.Tables(0).Rows(0)


                                If rel.tipdsdes = "fulla document" And campFitxer = "DOCDSFIT" Then
                                    'Si no estaba a la llista de documents, ho coloco
                                    Dim trobat As Integer = False

                                    If Not llistaDocuments Is Nothing Then
                                        For Each item In llistaDocuments
                                            If item = dbRow(campFitxer) Then
                                                trobat = True
                                                Exit For
                                            End If
                                        Next
                                    End If
                                    If Not trobat Then
                                        'Abans d'afegir el document a la llista de documents que es publicaran, comprobo que no s'hagués publicat ja
                                        '									IF InStr(rel.dsfit,dbROW("DOCDSFIT"))>0 THEN
                                        If dbRow("DOCWNUPD") = 1 Then
                                            If llistaDocuments Is Nothing Then
                                                ReDim llistaDocuments(0)
                                            Else
                                                ReDim Preserve llistaDocuments(llistaDocuments.Length)
                                            End If
                                            llistaDocuments(llistaDocuments.Length - 1) = codiRelacio.ToString() + "_" + dbRow("DOCDSFIT")
                                        End If
                                    End If
                                End If 'rel.tipdsdes="fulla document" AND campFitxer="DOCDSFIT" 

                                If esImatge = "1" And prefixCamp = "DOC" Then
                                    tamanyH = dbRow("tamanyH")
                                    tamanyV = dbRow("tamanyV")
                                    If publicar Then
                                        If Not (urlDesti Is Nothing) Then
                                            Dim strTmp As String
                                            strTmp = urlDesti.Replace("http://", "")
                                            'IF strTMP.length=0 THEN

                                            If InStr(rel.dsfit, ".swf") > 0 Then
                                                strTmp = strTmp.Substring(0, InStr(strTmp, "/") - 1)
                                                strContingut = "/gDocs/" + dbRow(camp)
                                            Else
                                                nomFitxerPublicat = rel.dsfit

                                                'faig la selecció de la mida de la imatge a visualitzar
                                                Dim mida As String = ""
                                                Dim fitxer As String = ""
                                                If width = 0 Then  'el cas de width=0 l'utilitzem només per donar-li mida màxima disponible a la imatge mitjançant el class
                                                    mida = "&t=imatgeGran"
                                                    fitxer = dbRow("DOCDSFIT")
                                                Else

                                                    If width <= 100 Then
                                                        mida = "&t=P100"
                                                        fitxer = dbRow("DOCDSFIT").replace(".", "P100.")

                                                    Else
                                                        If width <= 700 Then
                                                            mida = "&t=P"
                                                            fitxer = dbRow("DOCDSFIT").replace(".", "P.")
                                                        Else
                                                            fitxer = dbRow("DOCDSFIT")
                                                            mida = "&t=imatgeGran"
                                                        End If
                                                    End If
                                                End If

                                                strContingut = "/utils/obreFitxer.ashx?" & HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & codiNode & "&codiIdioma=" & idioma & mida & "&f=" & fitxer))



                                            End If
                                        Else
                                            strContingut = pathDocumentsXDefecte + dbRow(camp)
                                        End If
                                    Else
                                        tamanyH = 0
                                        tamanyV = 0
                                        strContingut = pathDocumentsXDefecte + dbRow(camp)
                                    End If
                                Else 'no és imatge					

                                    If prefixCamp = "LNK" And camp.Substring(0, 3) = "LNK" Then
                                        If campFitxer = "DOCDSFIT" Then
                                            If campFitxer = "DOCDSFIT" Then
                                                Dim relDoc As New clsRelacio
                                                relDoc.bdget(objConn, -1, dbRow("DOCINNOD"))
                                                enllaç = obtenirEnllacContingut(objConn, relDoc, idioma)
                                                Select Case plantilla.arrayEnllaços(index).Substring(plantilla.arrayEnllaços(index).IndexOf(".") + 1)
                                                    Case "IMATGE"
                                                        'faig el tractament de la imatge assocciada a un enllaç que apunta a imatges amb plantilla secundària per representar-lo
                                                        If plantilla.arrayPLTDSPLT(index).Length = 0 Then
                                                            GAIA2.f_logError(objConn, "PintarContingut", "", "S'ha intentat representar una imatge o document dins d'un contingut sense plantilla secund&agrave;ria per representar-lo")
                                                        End If
                                                        codiRelacioArxiuPerVisorImatges = dbRow("RELINCOD")


                                                        enllaç = obtenirEnllacContingut(objConn, relDoc, idioma)
                                                    Case "DOCUMENT"

                                                        'ja ho he tractat
                                                End Select
                                            End If
                                        Else
                                            If enllaç.Trim.Length = 0 Then
                                                enllaç = ""
                                            Else
                                                enllaç = GAIA2.obtenirEnllaçSimple(objConn, dbRow("LNKINNOD"), dbRow("idioma"), target, urlDesti)
                                            End If
                                        End If
                                    End If

                                    If camp.Trim() <> "" Then
                                        'Només htmlencode si no estava fet anteriorment. Això es fa per que hi ha texte que ja ve codificat dels editors wysisyg
                                        'Si dsnum>0 retallo el text amb "tallatext"
                                        If plantilla.arrayDSNUM(index) > 0 Then
                                            strContingut = GAIA2.tallaText(dbRow(camp), plantilla.arrayDSNUM(index)) & "..."
                                        Else
                                            If Not IsDBNull(dbRow(camp)) Then
                                                strContingut = dbRow(camp)
                                            End If
                                        End If

                                        If strContingut.Length > 0 Then
                                            strContingut = strContingut.Replace("<p>", "").Replace("</p>", "").Replace("&nbsp;&nbsp;", " ")
                                        End If

                                    End If
                                End If

                                If textalternatiu.Trim().Length > 0 Then
                                    textalternatiu = dbRow(textalternatiu).replace("""", "´")
                                End If
                                '********************************************************************************************
                                ' formatejo el contingut. Només ho faig si està en un idioma diferent si és un document.
                                '********************************************************************************************
                                If (rel.tipdsdes = "fulla document" And idiomaDiferent) Or (Not idiomaDiferent) Then
                                    If (enllaç + "").Trim().Length > 0 Then
                                        If rel.tipdsdes <> "fulla link" Then ' Si es una fulla link ja tinc l'enllaç que he trobat més amunt
                                            If enllaç = "DOCDSFIT" Then 'L'enlla que vull representar és un fitxer
                                                If dbRow("DOCDSLNK").length > 0 Then
                                                    enllaç = dbRow("DOCDSLNK")

                                                Else
                                                    If InStr(UCase(dbRow("TDODSNOM")), "IMAGE") > 0 And rel.cdsit <> 97 Then
                                                        enllaç = "http://www.l-h.cat/utils/visorImatges.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" + codiNode.ToString() + "&codiIdioma=" + idioma.ToString()))
                                                        If esEML = "S" Then
                                                            botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" style=""float:right; position:relative; top:-18px; right:3px; border:0"" class=""noImprimible"" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"
                                                        Else
                                                            botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" class=""simbolmes noImprimible"" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"
                                                        End If
                                                    Else
                                                        enllaç = "/utils/obreFitxer.ashx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbRow("DOCINNOD") & "&codiIdioma=" + idioma.ToString() & "&f=" & dbRow("DOCDSFIT")))
                                                    End If
                                                End If
                                            Else 'l'enllaç que vull representar és un contingut de tipus noticia/tramit/directori/agenda
                                                If enllaç <> "AUTO-ENLLAÇ" Then
                                                    enllaç = dbRow(enllaç)
                                                Else



                                                    Select Case rel.tipdsdes
                                                        Case "fulla noticia"


                                                            If plantilla.arrayAAL(index).Trim.Length > 0 Then
                                                                enllaç = plantilla.arrayAAL(index).Trim
                                                            Else
                                                                enllaç = "/detallNoticia.aspx"
                                                            End If
                                                            enllaç &= "?" & idioma & HttpUtility.UrlEncode(oCodParam.encriptar("cr=" + rel.incod.ToString() + "&amp;pl=168578&amp;id=" & idioma & "&amp;us=0&amp;width=" + width.ToString()))

                                                        Case "fulla info"
                                                            If plantilla.arrayAAL(index).Trim.Length > 0 Then
                                                                enllaç = plantilla.arrayAAL(index).Trim
                                                            Else
                                                                enllaç = "/detallInformacio.aspx"
                                                            End If
                                                            '180342
                                                            enllaç &= "?" & idioma & HttpUtility.UrlEncode(oCodParam.encriptar("cr=" + rel.incod.ToString() + "&amp;pl=207935&amp;id=" & idioma & "&amp;us=0&amp;width=" + width.ToString()))
                                                        Case "fulla tramit"
                                                            If plantilla.arrayAAL(index).Trim.Length > 0 Then
                                                                enllaç = plantilla.arrayAAL(index).Trim
                                                            Else
                                                                enllaç = "tramits.aspx"
                                                            End If

                                                            enllaç &= "?" & idioma & HttpUtility.UrlEncode(oCodParam.encriptar("cr=" + rel.incod.ToString() + "&amp;pl=150861&amp;id=" & idioma & "&amp;us=0&amp;width=" + width.ToString()))
                                                        Case "fulla directori"
                                                            enllaç = "/directori.aspx"
                                                        Case "fulla agenda"
                                                            If plantilla.arrayAAL(index).Trim.Length > 0 Then
                                                                enllaç = plantilla.arrayAAL(index).Trim
                                                                enllaç &= "?" & idioma & oCodParam.encriptar("cr=" & rel.incod & "&id=" & idioma & "&pl=" & plantilla.arrayPAL(index).Trim() & "&us=0&width=" & width)
                                                            Else
                                                                enllaç = "/agenda/detallAgenda.aspx"
                                                                enllaç &= "?" & idioma & oCodParam.encriptar("cr=" & rel.incod & "&id=" & idioma & "&pl=168660&us=0&width=" & width)
                                                            End If

                                                        Case "fulla document"
                                                            If dbRow("DOCDSLNK").length > 0 Then
                                                                enllaç = dbRow("DOCDSLNK")
                                                            Else
                                                                'si és una imatge poso un enllaç al visor d'imatges, en cas contrari l'enllaç serà al visor de document.				
                                                                enllaç = "/utils/obreFitxer.ashx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbRow("DOCINNOD") & "&codiIdioma=" + idioma.ToString() & "&f=" & dbRow("DOCDSFIT")))
                                                            End If
                                                    End Select
                                                End If
                                            End If
                                        End If

                                        If rel.tipintip = "49" Then
                                            target = GAIA2.obtenirTarget(objConn, rel)
                                        Else
                                            Try
                                                target = plantilla.arrayALF(index)
                                            Catch
                                                target = 0
                                            End Try
                                        End If
                                        If codiRelacioArxiuPerVisorImatges = 0 Then


                                            celdaFormateada += GAIA2.posaFormat(objConn, strContingut, plantilla.arrayCSSPlantilla(index), enllaç, esImatge, width, tamanyH, tamanyV, estat, textalternatiu, 1, heretaPropietatsWeb, target, rel, relIni, idioma, esEML, botoMesImatge, titolContingut, plantilla, pareTeEnllac, css)
                                        End If

                                    Else
                                        'el node pare (relIni) és un enllaç i el node actual (rel) és un document/imatge. 
                                        If rel.tipintip = "49" Then
                                            target = GAIA2.obtenirTarget(objConn, rel)
                                        Else
                                            Try
                                                target = plantilla.arrayALF(index)
                                            Catch
                                                target = 0
                                            End Try

                                        End If
                                        celdaFormateada += GAIA2.posaFormat(objConn, strContingut, plantilla.arrayCSSPlantilla(index), "", esImatge, width, tamanyH, tamanyV, estat, textalternatiu, 1, heretaPropietatsWeb, target, rel, relIni, idioma, esEML, botoMesImatge, titolContingut, plantilla, pareTeEnllac, css)
                                    End If
                                End If 'comprobació de l'idioma i del tipus de node
                            End If
                        End If 'TINC CONTINGUT

                    Else '(plantilla.arrayCampsPlantilla(index).IndexOf(".")=0 --> IMATGE o DOCUMENT


                        'Tracto els casos especials "IMATGE" (codi 96) i "DOCUMENT" (codi 97)
                        If plantilla.arrayCampsPlantilla(index) = "IMATGE" Or plantilla.arrayCampsPlantilla(index) = "DOCUMENT" Then

                            If plantilla.arrayPLTDSPLT(index).Length = 0 Then
                                GAIA2.f_logError(objConn, "PintarContingut", "", "S'ha intentat representar una imatge o document dins d'un contingut sense plantilla secund&agrave;ria per representar-lo")
                            Else

                                plantillaTMP.bdget(objConn, plantilla.arrayPLTDSPLT(index))
                                If plantillaTMP.innod > 0 Then
                                    If plantilla.arrayCampsPlantilla(index) = "IMATGE" Then
                                        GAIA2.bdr(objConn, "SELECT " & sqlTOP & " RELINFIL,RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELINCOD IN (SELECT MIN(RELINCOD) FROM METLREL WHERE RELINPAR=" & rel.infil & " AND RELCDSIT=96 GROUP BY RELINFIL)  ORDER BY RELCDORD", ds3)
                                        ' GAIA.bdr(objConn, "SELECT " & sqlTOP & " RELINFIL,RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELCDRSU = " & rel.incod & " AND RELCDSIT=96", ds3)


                                    Else
                                        ' GAIA.bdr(objConn, "SELECT " & sqlTOP & " RELINFIL,RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELCDRSU = " & rel.incod & " AND RELCDSIT=97", ds3)
                                        'msv: si faig la query d'adalt no troba alguns continguts que no pengen correctament de totes les relacions. En concret es perdien al perfil del contractant
                                        GAIA2.bdr(objConn, "SELECT " & sqlTOP & " RELINFIL,RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELINCOD IN (SELECT MIN(RELINCOD) FROM METLREL WHERE RELINPAR=" & rel.infil & " AND RELCDSIT=97 GROUP BY RELINFIL)  ORDER BY RELCDORD", ds3)





                                    End If
                                    codiNodeTMP = 0
                                    For Each dbrow3 In ds3.Tables(0).Rows
                                        If codiNodeTMP <> dbrow3("RELINFIL") Then
                                            codiNodeTMP = dbrow3("RELINFIL")
                                            relarxiu.bdget(objConn, dbrow3("RELINCOD"))
                                            GAIA2.dibuixaPreview(objConn, plantillaTMP, plantillaTMP.est, plantillaTMP.arrayAtr, plantillaTMP.hor, plantillaTMP.ver, plantillaTMP.tco, "", celdaFormateada, css, width, 0, "f", relarxiu, 0, fdesti, urlDesti, 1, llistaDocuments, publicar, idioma, plantillaTMP.css, rel, dataSimulacio, codiUsuari, 0, "", 0, plantillaTMP.flw, "", 0, estilbody, llistaCND, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, plantillaTMP.niv, plantilla.arrayEnllaços(index).Trim().Length > 0)
                                        End If
                                    Next dbrow3
                                Else
                                    GAIA2.f_logError(objConn, "PintarContingut", "", "S'ha intentat representar una imatge o document dins d'un contingut sense plantilla secund&agrave;ria per representar-lo")
                                End If
                            End If

                            'Tracto l'enllaç. (pot ser que ja hagi representat la imatge d'un enllaç de tipus banner i ara he de posar l'enllaç)					
                            'En cas de tenir un enllaç a una imatge, plantillaTMP ha de ser=0 per que si <> 0 ja s'hauria representat l'enllaç en la plantilla secundària
                            'podria ser una imatge que té com a enllaç un document, llavors si que s'ha de representar
                            If ((plantilla.arrayEnllaços(index) = "IMATGE" And plantillaTMP.innod = 0) Or (plantilla.arrayEnllaços(index) = "DOCUMENT")) And plantilla.arrayEnllaços(index) <> plantilla.arrayCampsPlantilla(index) Then
                                If plantilla.arrayEnllaços(index) = "IMATGE" Then

                                    'msv 4/1/11 -> Aquestes 4 sql serien + correctes  sense el RELINPAR=  i amb un RELCDRSU=" & rel.incod , però hi ha algun cas antic que una imatge o document que penja d'un contingut  no ho fa per a totes les relacions. 
                                    '       GAIA.bdR(objConn, "SELECT " + sqlTOP + " * FROM METLREL,METLDOC,METLTDO WHERE DOCINTDO=TDOCDTDO AND RELCDRSU=" + rel.incod.ToString() + " AND RELCDSIT=96 AND DOCINNOD=RELINFIL AND DOCINIDI=" + idioma.ToString() + " ORDER BY RELCDORD", ds3)
                                    GAIA2.bdr(objConn, "SELECT " + sqlTOP + " * FROM METLREL WITH(NOLOCK),METLDOC WITH(NOLOCK),METLTDO WITH(NOLOCK)WHERE DOCINTDO=TDOCDTDO AND RELINPAR=" & rel.infil & " AND RELCDSIT=96 AND DOCINNOD=RELINFIL AND DOCINIDI=" + idioma.ToString() + " ORDER BY RELCDORD", ds3)
                                Else
                                    GAIA2.bdr(objConn, "SELECT " + sqlTOP + " * FROM METLREL WITH(NOLOCK),METLDOC WITH(NOLOCK),METLTDO WITH(NOLOCK) WHERE DOCINTDO=TDOCDTDO AND RELCDRSU=" + rel.incod.ToString() + "  AND RELCDSIT=97 AND DOCINNOD=RELINFIL AND DOCINIDI=" + idioma.ToString() + " ORDER BY RELCDORD", ds3)
                                End If
                                If ds3.Tables(0).Rows.Count = 0 Then 'si no ho trobo amb l'idioma, busco a d'altres idiomes
                                    If plantilla.arrayEnllaços(index) = "IMATGE" Then
                                        ' GAIA.bdR(objConn, "SELECT " + sqlTOP + " * FROM METLREL,METLDOC,METLTDO WHERE DOCINTDO=TDOCDTDO AND  RELCDRSU=" + rel.incod.ToString() + "  AND RELCDSIT=96 AND DOCINNOD=RELINFIL  ORDER BY RELCDORD", ds3)
                                        GAIA2.bdr(objConn, "SELECT " + sqlTOP + " * FROM METLREL WITH(NOLOCK),METLDOC WITH(NOLOCK),METLTDO WITH(NOLOCK) WHERE DOCINTDO=TDOCDTDO AND  RELINPAR=" & rel.infil & "  AND RELCDSIT=96 AND DOCINNOD=RELINFIL  ORDER BY RELCDORD", ds3)
                                    Else
                                        GAIA2.bdr(objConn, "SELECT " + sqlTOP + " * FROM METLREL WITH(NOLOCK),METLDOC WITH(NOLOCK),METLTDO WITH(NOLOCK) WHERE DOCINTDO=TDOCDTDO AND RELCDRSU=" + rel.incod.ToString() + "  AND RELCDSIT=97 AND DOCINNOD=RELINFIL  ORDER BY RELCDORD", ds3)
                                    End If
                                End If
                                If ds3.Tables(0).Rows.Count > 0 Then
                                    dbrow3 = ds3.Tables(0).Rows(0)
                                    relEnllaç.bdget(objConn, dbrow3("RELINCOD"))
                                    '********************************************************
                                    ' afegeixo a mà el document que "penja d'enllaç"
                                    '********************************************************																		
                                    If llistaDocuments Is Nothing Then
                                        ReDim llistaDocuments(0)

                                    Else

                                        ReDim Preserve llistaDocuments(llistaDocuments.Length)
                                    End If
                                    If llistaDocuments.Length <> 0 Then
                                        llistaDocuments(llistaDocuments.Length - 1) = relEnllaç.incod.ToString() + "_" + dbrow3("DOCDSFIT")
                                    End If
                                    'He de posar l'enllaç abans de la imatge que hi ha a dins de celdaFormatejada.
                                    pospunt = InStr(celdaFormateada, "<img")
                                    If pospunt > 0 Then
                                        If Not dbrow3("DOCDSALT") Is Nothing Then
                                            If dbrow3("DOCDSALT").length > 0 Then
                                                textalternatiu = dbrow3("DOCDSALT").replace("""", "'")
                                            End If
                                        End If
                                        'Si el document és de tipus imatge poso l'enllaç al visor d'Imatges
                                        If InStr(UCase(dbrow3("TDODSNOM")), "IMAGE") > 0 Then

                                            strURL = "http://www.l-h.cat/utils/visorImatges.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbrow3("DOCINNOD") & "&codiIdioma=" & idioma))
                                            If esEML = "S" Then
                                                botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" style=""float:right; position:relative; top:-18px; right:3px; border:0"" class=""noImprimible "" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"


                                            Else
                                                botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" class=""simbolmes noImprimible"" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"
                                            End If
                                        Else
                                            strURL = "/utils/obreFitxer.ashx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbrow3("DOCINNOD") & "&codiIdioma=" + idioma.ToString() & "&f=" & dbrow3("DOCDSFIT")))
                                        End If



                                        If titolContingut = "" Then
                                            titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                                        End If
                                        target = GAIA2.obtenirTarget(objConn, rel)

                                        strvoid = GAIA2.trobaEstilCSS(objConn, plantilla.arrayCSSPlantilla(index), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA)



                                        If esEML = "N" Then
                                            celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) + "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                        Else
                                            celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) + "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & celdaFormateada.Substring(pospunt - 1) & "</a>"
                                        End If




                                    End If
                                End If
                            Else
                                pospunt = InStr(celdaFormateada, "<img")
                                Select Case plantilla.arrayEnllaços(index)
                                    Case "METLLNK.LNKCDREL"

                                        If pospunt > 0 Then
                                            strURL = GAIA2.obtenirEnllaçSimple(objConn, rel.infil, idioma, target, urlDesti)
                                            If esEML = "S" Then
                                                botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" style=""float:right; position:relative; top:-18px; right:3px; border:0"" class=""noImprimible"" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"
                                            Else
                                                botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" class=""simbolmes noImprimible"" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"
                                            End If

                                            If titolContingut = "" Then
                                                titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                                            End If
                                            strvoid = GAIA2.trobaEstilCSS(objConn, plantilla.arrayCSSPlantilla(index), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA)
                                            celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"



                                            If esEML = "N" Then
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                            Else
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & celdaFormateada.Substring(pospunt - 1) & "</a>"
                                            End If


                                        End If
                                    Case "METLLNK.LNKDSLNK"

                                        If pospunt > 0 Then
                                            GAIA2.bdr(objConn, "SELECT LNKDSLNK,LNKWNTIP FROM METLLNK WITH(NOLOCK) WHERE LNKINNOD=" & rel.infil & " AND LNKINIDI=" & idioma, ds3)

                                            If ds3.Tables(0).Rows.Count = 0 Then
                                                GAIA2.bdr(objConn, "SELECT LNKDSLNK,LNKWNTIP FROM METLLNK WITH(NOLOCK) WHERE LNKINNOD=" & rel.infil & " ORDER BY LNKINIDI", ds3)

                                            End If

                                            If ds3.Tables(0).Rows.Count > 0 Then
                                                dbrow3 = ds3.Tables(0).Rows(0)
                                                If dbrow3("LNKDSLNK") <> "" Then
                                                    strURL = dbrow3("LNKDSLNK")
                                                    If titolContingut = "" Then
                                                        titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                                                    End If
                                                    strvoid = GAIA2.trobaEstilCSS(objConn, plantilla.arrayCSSPlantilla(index), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA)

                                                    If esEML = "N" Then
                                                        celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(dbrow3("LNKWNTIP") = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(dbrow3("LNKWNTIP") = 1, " target=""_blank""", " target=""_self""") & ">" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                                    Else
                                                        celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(dbrow3("LNKWNTIP") = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(dbrow3("LNKWNTIP") = 1, " target=""_blank""", " target=""_self""") & ">" & celdaFormateada.Substring(pospunt - 1) & "</a>"
                                                    End If

                                                End If
                                            End If
                                        End If
                                    Case "METLNOT.NOTDSLNK"
                                        GAIA2.bdr(objConn, "SELECT " & plantilla.arrayEnllaços(index) & " FROM METLNOT WITH(NOLOCK) WHERE NOTINNOD=" & rel.infil & " AND NOTINIDI=" & idioma, ds3)
                                        If ds3.Tables(0).Rows.Count > 0 Then
                                            dbrow3 = ds3.Tables(0).Rows(0)
                                            If titolContingut = "" Then
                                                titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                                            End If

                                            'he de netejar el html que pugui haver a NOTDSLNK
                                            Dim strTMp As String = dbrow3("NOTDSLNK")
                                            If InStr(strTMp, "<a") Then
                                                strTMp = strTMp.Substring(InStr(strTMp, "http") - 1)
                                                strTMp = strTMp.Substring(0, InStr(strTMp, """") - 1)

                                            End If

                                            strvoid = GAIA2.trobaEstilCSS(objConn, plantilla.arrayCSSPlantilla(index), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA)

                                            If strTMp.Length And celdaFormateada.Length > 0 Then
                                                If esEML = "N" Then
                                                    celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTMp) & """  " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(titolContingut) & """>" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                                Else
                                                    celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTMp) & """  " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(titolContingut) & """>" & celdaFormateada.Substring(pospunt - 1) & "</a>"
                                                End If
                                            End If

                                        End If

                                    Case "METLINF.INFDSLNK"
                                        GAIA2.bdr(objConn, "SELECT METLINF.INFDSLNK FROM METLINF WITH(NOLOCK) WHERE INFINNOD=" & rel.infil & " AND INFINIDI=" & idioma, ds3)
                                        If ds3.Tables(0).Rows.Count > 0 Then
                                            dbrow3 = ds3.Tables(0).Rows(0)
                                            'If titolContingut = "" Then
                                            '    titolContingut = GAIA.obtenirTitolContingut(objConn, rel, idioma)
                                            ' End If

                                            'he de netejar el html que pugui haver a INFDSLNK
                                            Dim str As String = dbrow3("INFDSLNK")
                                            Dim strHref As String = ""
                                            Dim strTarget As String = ""
                                            Dim strTmp As String = ""
                                            Dim strText As String = ""

                                            If InStr(str, "href") > 0 Then
                                                str = str.Replace("href", "|href")
                                                Dim k As String() = str.Split("|")
                                                For Each strItem As String In k
                                                    If InStr(strItem, "href") > 0 Then

                                                        strHref = ""
                                                        strTarget = ""
                                                        strText = ""
                                                        strTmp = ""
                                                        strItem = strItem.Replace("""", "|")
                                                        'recupero el href 
                                                        strTmp = strItem.Substring(InStr(strItem, "href=|") + 5)
                                                        strHref = strTmp.Substring(0, InStr(strTmp, "|") - 1)
                                                        'recupero target						
                                                        strTmp = strItem.Substring(InStr(strItem, "target=|") + 7)
                                                        strTarget = strTmp.Substring(0, InStr(strTmp, "|") - 1)
                                                        'recupero text
                                                        strTmp = strItem.Substring(InStr(strItem, ">"))
                                                        strText = strTmp.Substring(0, InStr(strTmp, "<") - 1)
                                                    End If
                                                Next strItem
                                            Else
                                                strHref = str
                                                strTarget = "_self"
                                                strText = str
                                            End If

                                            strvoid = GAIA2.trobaEstilCSS(objConn, plantilla.arrayCSSPlantilla(index), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA)

                                            If strTmp.Length > 0 Then
                                                If esEML = "N" Then
                                                    celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strHref) & """  " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(strText) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """>" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                                Else
                                                    celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strHref) & """  " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(strText) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """>" & celdaFormateada.Substring(pospunt - 1) & "</a>"

                                                End If
                                            End If
                                        End If
                                    Case "METLAGD.AGDDSLNK"
                                        GAIA2.bdr(objConn, "SELECT " & plantilla.arrayEnllaços(index) & " FROM METLAGD WITH(NOLOCK) WHERE AGDINNOD=" & rel.infil & " AND AGDINIDI=" & idioma, ds3)
                                        If ds3.Tables(0).Rows.Count > 0 Then
                                            dbrow3 = ds3.Tables(0).Rows(0)
                                            If titolContingut = "" Then
                                                titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                                            End If
                                            Dim strTMp As String = dbrow3("AGDDSLNK")
                                            If InStr(strTMp, "<a") Then
                                                strTMp = strTMp.Substring(InStr(strTMp, "http") - 1)
                                                strTMp = strTMp.Substring(0, InStr(strTMp, """") - 1)

                                            End If
                                            strvoid = GAIA2.trobaEstilCSS(objConn, plantilla.arrayCSSPlantilla(index), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA)


                                            If esEML = "N" Then
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTMp) & """ " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(titolContingut) & """>" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                            Else
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTMp) & """ " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(titolContingut) & """>" & celdaFormateada.Substring(pospunt - 1) & "</a>"

                                            End If
                                        End If
                                    Case "AUTO-ENLLAÇ"
                                        If pospunt > 0 Then 'si no hi ha imatge no fem l'autoenllaç
                                            Dim strTmp As String = obtenirEnllacContingut(objConn, rel, idioma)
                                            If InStr(UCase(strTmp), "OBREFITXER") > 0 Then
                                                '  strTmp = strTmp.Replace("?", "?" & idioma)
                                            End If
                                            If esEML = "N" Then
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTmp) & """ " & estilsCSSSenseFons & " title=""" & GAIA2.obtenirTitolContingut(objConn, rel, idioma) & """>" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                            Else
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTmp) & """ " & estilsCSSSenseFons & " title=""" & GAIA2.obtenirTitolContingut(objConn, rel, idioma) & """>" & celdaFormateada.Substring(pospunt - 1) & "</a>"
                                            End If
                                        End If
                                End Select
                            End If
                        End If
                    End If 'IF (plantilla.arrayCampsPlantilla(index).IndexOf(".")>=0) THEN			
                End If

                If True Then
                    'Poso l'autolink
                    If codiRelacioArxiuPerVisorImatges = 0 And plantilla.arrayPAL.Length > index Then
                        If (plantilla.arrayPAL(index).Trim.Length > 0) Then
                            'Per ara només ensenyo l'autolink si publico la pàgina.
                            fitxerAutoLink = plantilla.arrayAAL(index) & "?" & idioma & HttpUtility.UrlEncode(oCodParam.encriptar("cr=" & codiRelacio & "&id=" & idioma.ToString() & "&pl=" & plantilla.arrayPAL(index).Trim() & "&us=0"))
                            tancaEnllaç = False
                            ' Si no he trobat titol del contingut busco el títol del contingut segons l'idioma
                            If titolContingut = "" Then
                                titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                            End If
                            ' Si no he trobat titol del contingut agafo el nom del node, és un problema per que només apareix en català.
                            If titolContingut = "" Then
                                titolContingut = rel.noddstxt
                            End If
                            Select Case idioma
                                Case 2
                                    titolContingut = "Leer m&aacute;s de " & titolContingut
                                Case 3
                                    titolContingut = "Read more about " & titolContingut
                                Case Else
                                    titolContingut = "Llegir m&eacute;s de " & titolContingut
                            End Select

                            'Per accessibilitat poso el títol al text de l'enllaç però ocult amb técniques accessibles
                            If plantilla.arrayALK(index).Length > 0 Then
                                If esEML = "N" Then
                                    celdaFormateada += GAIA2.posaFormat(objConn, plantilla.arrayALK(index) & "<span class=""visibilidadoculta"">(" & titolContingut & ")</span>", plantilla.arrayCSSPlantilla(index), fitxerAutoLink, esImatge, width, tamanyH, tamanyV, estat, titolContingut, tancaEnllaç, heretaPropietatsWeb, plantilla.arrayALF(index), rel, relIni, idioma, esEML, botoMesImatge, titolContingut, plantilla, pareTeEnllac, css)
                                Else
                                    celdaFormateada += GAIA2.posaFormat(objConn, plantilla.arrayALK(index), plantilla.arrayCSSPlantilla(index), fitxerAutoLink, esImatge, width, tamanyH, tamanyV, estat, titolContingut, tancaEnllaç, heretaPropietatsWeb, plantilla.arrayALF(index), rel, relIni, idioma, esEML, botoMesImatge, titolContingut, plantilla, pareTeEnllac, css)

                                End If
                            End If
                        End If

                    End If
                End If

                If plantilla.arrayLC2.Length > index Then
                    If (plantilla.arrayLC2(index).Trim.Length > 0) And celdaFormateada.Length > 0 Then
                        For Each item In plantilla.arrayLC2(index).Split("|")
                            lcwPOST += trobaCodiWeb(objConn, plantilla, item, idioma, rel, relIni, forçarCodiPost, index, "", width, codiUsuari, css, tagsMeta)
                        Next item
                    End If
                End If




                If tancaEnllaç Then
                    celdaFormateada += "</a>"
                    tancaEnllaç = False
                End If

                If codiRelacioArxiuPerVisorImatges <> 0 Then

                    plantillaTMP.bdget(objConn, plantilla.arrayPLTDSPLT(index))
                    If plantillaTMP.innod > 0 Then

                        relarxiu.bdget(objConn, codiRelacioArxiuPerVisorImatges)
                        celdaFormateada = "<a href=""" & GAIA2.netejaHTML(enllaç) & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & GAIA2.netejaHTML(titolContingut) & "</a></div>"

                    Else
                        GAIA2.f_logError(objConn, "PintarContingut", "", "S'ha intentat representar una imatge o document dins d'un contingut sense plantilla secund&agrave;ria per representar-lo")
                    End If
                End If


                Dim tamanyContingut As Integer

                tamanyContingut = celdaFormateada.Trim.Length

                If forçarCodiAnt = 1 Or tamanyContingut > 0 Then
                    celdaFormateada = lcwANT + celdaFormateada
                End If
                If forçarCodiPost = 1 Or tamanyContingut > 0 Then
                    celdaFormateada = celdaFormateada + lcwPOST
                End If



                html += celdaFormateada
                DS.Dispose()
                ds3.Dispose()
                oCodParam = Nothing
            End If 'plantilla.innod<>0
        End If

    End Sub 'pintarContingut

    '***************************************************************************************************
    '	Funció: GAIA.trobarTextCircuit
    '	Entrada: 
    '					codiRelacioCircuit: Codi relacio on RELINFIL apunta al node fulla circuit del que volem obtenir el text
    '												Si =0 llavors busquem per codiNode
    '					codiNodeCircuit:	CodiNode que apunta al node fulla circuit del que volem obtenir el text
    '												Si =0 llavors busquem per codiRelacio
    '					misTitolCircuit: string per referència on guardarem el valor FCIDSNOM
    '					misAprovacioPeticionari: string per referència on guardarem el valor FCIDSMA1
    '					misSolicitudAprovacio: string per referència on guardarem el valor FCIDSMA2
    '					misCancelacioPeticionari: string per referència on guardarem el valor FCIDSMIC
    '					misModificacioPeticionari: string per referència on guardarem el valor FCIDSMIM
    '					
    '	Acció: 
    '***************************************************************************************************
    Public Shared Sub trobarTextCircuit(ByVal objConn As OleDbConnection, ByVal relCircuit As clsRelacio, ByVal codiNodeCircuit As Integer, ByRef misTitolCircuit As String, ByRef misSolicitudAprovador As String, ByRef misAprovacioPeticionari As String, ByRef misCancelacioPeticionari As String, ByRef misModificacioPeticionari As String)
        Dim DS As DataSet
        Dim dbRow As DataRow

        Dim strSql As String = ""
        Dim codiRelacioCircuit As Integer
        codiRelacioCircuit = relCircuit.incod
        DS = New DataSet()
        If codiRelacioCircuit > 0 Then
            strSql = "SELECT FCIDSNOM, FCIDSMA1, FCIDSMA2, FCIDSMIC, FCIDSMIM FROM METLFCI WITH(NOLOCK),METLREL WITH(NOLOCK) WHERE  RELINCOD=" + codiRelacioCircuit.ToString() + " AND FCIINNOD=RELINFIL AND RELCDSIT<98"
        Else
            If codiNodeCircuit > 0 Then
                strSql = "SELECT FCIDSNOM, FCIDSMA1, FCIDSMA2, FCIDSMIC, FCIDSMIM FROM METLFCI WITH(NOLOCK) WHERE  PCRINNOD=" + codiNodeCircuit.ToString()
            End If
        End If
        If strSql.Length > 0 Then
            GAIA2.bdr(objConn, strSql, DS)
            If DS.Tables(0).Rows.Count > 0 Then
                dbRow = DS.Tables(0).Rows(0)
                misTitolCircuit = dbRow("FCIDSNOM")
                misAprovacioPeticionari = dbRow("FCIDSMA1")
                misSolicitudAprovador = dbRow("FCIDSMA2")
                misCancelacioPeticionari = dbRow("FCIDSMIC")

                misModificacioPeticionari = dbRow("FCIDSMIM")
            End If
        End If
        DS.Dispose()


    End Sub 'trobarTextCircuit




    Public Shared Function editarContingut(ByVal objConn As OleDbConnection, ByVal nodo As Integer, ByVal idioma As Integer) As String
        editarContingut = editarContingut(objConn, nodo, idioma, 0, 0, 0)
    End Function


    Public Shared Function editarContingut(ByVal objConn As OleDbConnection, ByVal nodo As Integer, ByVal idioma As Integer, ByVal rel1 As Integer, ByVal rel2 As Integer, ByVal relActiva As Integer, Optional ByVal obrirNovaFinestra As Boolean = True) As String
        Dim descTipus As String = ""
        Dim crida As String = ""

        GAIA2.tipusNodebyNro(objConn, nodo, descTipus)
        Select Case descTipus
            Case "fulla missatge"
                crida = "/GAIA/aspx/missatges/obrirmissatge.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString() + "&rel1=" + rel1.ToString() + "&rel2=" + rel2.ToString()
            Case "fulla catalegServeis"
                crida = "/GAIA/aspx/catalegServeis/frmCSAccions.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "node catalegServeis"
                crida = "/GAIA/aspx/catalegServeis/frmNodeAgrupacioServei.aspx?id=" & nodo & "&amp;cr=" & relActiva & "&amp;idiArbre=" & idioma
            Case "fulla web"
                crida = "/GAIA/aspx/web/frmFullaWeb.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "node web"
                crida = "/GAIA/aspx/web/frmNodeWeb.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "arbre web"
                crida = "/GAIA/aspx/web/frmArbreWeb.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "fulla plantillaWeb"
                crida = "/GAIA/aspx/web/frmPlantilla.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "fulla codiWeb"
                crida = "/GAIA/aspx/fulles/frmLlibreriaCodiWeb.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "fulla document"
                crida = "/GAIA/aspx/documents/CarregaDocuments.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "fulla noticia"
                crida = "/GAIA/aspx/noticies/CarregaNoticies.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString() + "&amp;cr=" & relActiva.ToString()
            Case "fulla info"
                crida = "/GAIA/aspx/noticies/CarregaInfo.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "fulla projecte"

                crida = "/GAIA/aspx/projecte/frmProjecte.aspx	?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "fulla link"
                crida = "/GAIA/aspx/Links/CarregaLinks.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "fulla contractació"
                crida = "/GAIA/aspx/contractacio/CarregaContractes.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "node organigrama"
                crida = "/GAIA/aspx/organigrama/frmNodeOrganigrama.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "fulla organigrama"
                crida = "/GAIA/aspx/organigrama/frmFullaOrganigrama.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "fulla circuit"
                crida = "/GAIA/aspx/circuits/frmCircuits.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "node circuit"
                crida = "/GAIA/aspx/circuits/frmNodeCircuit.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "node elMeuEspai"
                crida = "/GAIA/aspx/elMeuEspai/frmNodeelMeuEspai.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "node document"
                crida = "/GAIA/aspx/documents/frmNodeDocuments.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString()
            Case "fulla agenda"
                crida = "/GAIA/aspx/agenda/frmAgenda.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString() & "&amp;cr=" & relActiva
            Case "fulla tramit"
                crida = "/GAIA/aspx/tramits/frmTramits.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString() & "&amp;cr=" & relActiva

            Case "fulla directori"

                crida = "/GAIA/aspx/directori/frmDirectori.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString() + "&rel=" + relActiva.ToString()
            Case "node codificacio"
                crida = "/GAIA/aspx/codificacio/frmNodecodificacio.aspx?id=" & nodo & "&amp;cr=" & relActiva & "&amp;idiArbre=" & idioma
            Case "fulla projecte"
                crida = "/GAIA/aspx/projecte/frmProjecte.aspx?id=" + nodo.ToString() + "&amp;idiArbre=" + idioma.ToString() + "&rel=" + relActiva.ToString()

            Case Else
                crida = "/GAIA/aspx/fulles/frmNodes.aspx?id=" + nodo.ToString() + "&amp;cr=" + relActiva.ToString() + "&amp;idiArbre=" + idioma.ToString()
        End Select
        If obrirNovaFinestra Then crida = "<script language=""JavaScript"" type=""text/javascript"">window.open(""" & crida & """)</script>"
        Return (crida)
    End Function 'editarContingut


    '***************************************************************************************************
    '	Funció: GAIA.aprovarPeticio
    '	Entrada: codiRelacioContingut: codi de relació que apunta al contingut que està dins de la petició
    '					 codiRelacioCircuit: codi de la relació que apunta al pas del circuit que volem aprovar
    '					 codirNodePasCircuit: codi de node que apunta al node circuit que volem aprovar
    '						codiUsuariPeticionari: codi de node organigrama de l'usuari que ha iniciat el circuit					
    '	Acció: 
    '				Busco si hi ha algun pas seguent
    '					si hi ha: faig un canvi d'estat, envio els missatges corresponents i elimino els que ja no facin falta
    '					si és l'últim pas d'un circuit he de realitzar l'acció. 
    '***************************************************************************************************

    Public Shared Sub aprovarPeticio(ByVal objConn As OleDbConnection, ByVal relContingut As clsRelacio, ByVal relCircuit As clsRelacio, ByVal codiNodePasCircuit As Integer, ByVal codiUsuariPeticionari As Integer, ByVal relContingutIniciador As clsRelacio)
        Dim seguentPasCircuit As Integer
        Dim textAprovacioUsuariPeticionari As String
        Dim arrayDestinataris As Integer()
        Dim width As Integer = 760
        Dim codiRelacioContingutIniciador As Integer
        codiRelacioContingutIniciador = relContingutIniciador.incod
        Dim titol As String = ""
        Dim estilbody As String = ""
        Dim strCSSPantalla As String = ""
        Dim strCSSImpressora As String = ""
        Dim tagsMeta As String = ""
        Dim esform As String = ""

        Dim esEML As String = ""
        Dim esSSL As String = ""
        Dim htmlPeu As String = ""
        '1-Esborrar Missatges pendents del pas del circuit (o bé passem al següent pas o bé acaba el circuit, els missatges pendents
        'que esperaven una aprovació o cancel·lació ja es poden esborrar).
        GAIA2.anularPasCircuit(objConn, relContingut)
        seguentPasCircuit = GAIA2.obtenirGermaSeguentRelacio(objConn, relCircuit)

        If seguentPasCircuit > 0 Then ' hi ha següent pas del circuit
            ' Faig el canvi d'estat i envio missatges als nous aprovadors
            GAIA2.iniciarCircuit(objConn, relContingut, 0, codiUsuariPeticionari, seguentPasCircuit, relContingutIniciador)
        Else    'no hi ha seguent pas del circuit		
            '2-Si cal enviarem missatges confirmant l'aprovació del pas als usuaris peticionari
            ReDim arrayDestinataris(0)
            arrayDestinataris(0) = codiUsuariPeticionari
            textAprovacioUsuariPeticionari = GAIA2.obtenirValorRegistreCircuit(objConn, codiNodePasCircuit, "FCIDSMA1")
            If textAprovacioUsuariPeticionari <> "-1" Then
                If arrayDestinataris.Length > 0 Then
                    If arrayDestinataris(0) = 0 And arrayDestinataris.Length = 1 Then
                        GAIA2.enviaMissatge(objConn, relContingut, textAprovacioUsuariPeticionari, textAprovacioUsuariPeticionari, codiNodePasCircuit, "N", "N", "S", "S", arrayDestinataris, relContingutIniciador)

                        GAIA2.log(objConn, relContingut, arrayDestinataris(0), textAprovacioUsuariPeticionari, TAAPROVAR)
                    End If
                End If
            Else
                'No hi ha text previst. NO envio res.
            End If



            Dim accAprovacio, accDenegacio As Integer

            GAIA2.obtenirAccioNodeCircuit(objConn, GAIA2.obtenirRelacioSuperior(objConn, relCircuit), accAprovacio, accDenegacio)

            'FAIG LA SELECCIÓ DE LA FUNCIÓ QUE HE D'EXECUTAR
            If accAprovacio > 0 Then
                Select Case accAprovacio
                    Case 1 'Publicar Continguts

                        Dim html, css, fdesti, urlDesti As String
                        html = ""
                        css = ""
                        fdesti = ""
                        urlDesti = ""
                        titol = ""
                        Dim llistaDocuments As String() = {}
                        GAIA2.canviEstatRelacio(objConn, relContingut, relContingut.cdsit, 0)
                        Dim heretaPropietatsWeb As Integer = GAIA2.heretarPropietatsWeb(objConn, relContingutIniciador.incod, width)
                        GAIA2.obreFullaWeb(objConn, relContingutIniciador, html, css, heretaPropietatsWeb, width, 500, "", fdesti, urlDesti, 1, llistaDocuments, 1, 1, relContingutIniciador, Now, 0, titol, 1, estilbody, esform, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, True)

                    Case 2
                        caducarContinguts(objConn, relContingut, codiUsuariPeticionari)
                End Select
            End If
        End If
    End Sub 'aprovarPeticio




    '***************************************************************************************************
    '	Funció: GAIA.denegarPeticio
    '	Entrada: relContingut:relació que apunta al contingut que està dins de la petició
    '					 relCircuit:Relació que apunta al pas del circuit que volem denegar
    '					 codirNodePasCircuit: codi de node que apunta al node circuit que volem denegar
    '						codiUsuariPeticionari: codi de node organigrama de l'usuari que ha iniciat el circuit		
    '						relContingutIniciador: contingut que conté a "Relcontingut". Normalment la plana web que té el contingut q es vol publicar.			
    '	Acció: 

    '					Cancel·lo el circuit, envio missatges de denegació de petició (si fa falta), esborro els missatges sobre el contingut
    '					i marco el contingut com a esborrat.
    '***************************************************************************************************
    Public Shared Sub denegarPeticio(ByVal objConn As OleDbConnection, ByVal relContingut As clsRelacio, ByVal relCircuit As clsRelacio, ByVal codiNodePasCircuit As Integer, ByVal codiUsuariPeticionari As Integer, ByVal relContingutIniciador As clsRelacio)

        Dim textCancelacioUsuariPeticionari As String
        Dim arrayDestinataris As Integer()
        Dim codiRelacioContingutIniciador As Integer


        codiRelacioContingutIniciador = relContingutIniciador.incod

        '1-Esborrar Missatges pendents del pas del circuit (o bé passem al següent pas o bé acaba el circuit, els missatges pendents
        'que esperaven una aprovació o cancel·lació ja es poden esborrar).
        GAIA2.anularPasCircuit(objConn, relContingut)

        'no hi ha seguent pas del circuit	perque no acceptem la petició
        '2-Si cal enviarem missatges comunicant la denegació del pas als usuaris peticionaris
        ReDim arrayDestinataris(0)
        arrayDestinataris(0) = codiUsuariPeticionari
        textCancelacioUsuariPeticionari = GAIA2.obtenirValorRegistreCircuit(objConn, codiNodePasCircuit, "FCIDSMIC")

        If textCancelacioUsuariPeticionari <> "-1" Then
            If arrayDestinataris.Length > 0 Then
                If arrayDestinataris(0) = 0 And arrayDestinataris.Length = 1 Then
                    GAIA2.enviaMissatge(objConn, relContingut, textCancelacioUsuariPeticionari, textCancelacioUsuariPeticionari, codiNodePasCircuit, "N", "N", "S", "S", arrayDestinataris, relContingutIniciador)
                    GAIA2.log(objConn, relContingut, arrayDestinataris(0), textCancelacioUsuariPeticionari, TADENEGAR)
                End If
            End If
        Else
            'No hi ha text previst. NO envio res.
        End If



        Dim accAprovacio, accDenegacio As Integer

        GAIA2.obtenirAccioNodeCircuit(objConn, GAIA2.obtenirRelacioSuperior(objConn, relCircuit), accAprovacio, accDenegacio)

        'FAIG LA SELECCIÓ DE LA FUNCIÓ QUE HE D'EXECUTAR
        If accDenegacio > 0 Then
            Select Case accDenegacio
                Case 2 'CaducarContinguts

                    GAIA2.caducarContinguts(objConn, relContingut, codiUsuariPeticionari)
                    'GAIA.canviEstatRelacio(objconn, relContingut, 99,0)
            End Select
        End If

    End Sub 'denegarPeticio



    '***************************************************************************************************
    '	Funció: GAIA.obtenirValorRegistreCircuit
    '	Entrada: codiNodePasCircuit: codi FCIINNOD de METLFCI 
    '					 camp: string amb el nom del camp que volem cercar
    '	Acció: 	Busco el valor del "camp" de la taula METLFCI apuntat per codiNodePasCircuit per l'idioma 1
    '					Retorna -1 si no trobat o en cas contrari el valor de "CAMP" 
    '***************************************************************************************************
    Public Shared Function obtenirValorRegistreCircuit(ByVal objConn As OleDbConnection, ByVal codiNodePasCircuit As Integer, ByVal camp As String) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        obtenirValorRegistreCircuit = "-1"
        GAIA2.bdr(objConn, "SELECT " + camp + " FROM METLFCI WITH(NOLOCK) WHERE  FCIINNOD=" + codiNodePasCircuit.ToString(), DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirValorRegistreCircuit = dbRow(camp)
        End If
        DS.Dispose()
    End Function 'obtenirValorRegistreCircuit



    '****************************************************************************************************************
    '	Funció: GAIA.obtenirGermaSeguentRelacio
    '	Entrada: codiNodePasCircuit: codi FCIINNOD de METLFCI 
    '					 camp: string amb el nom del camp que volem cercar
    '	Acció: 	Busco la relacio filla del mateix pare que "codiRelacio" amb RELCDORD superior i retorno el relincod
    '*****************************************************************************************************************
    Public Shared Function obtenirGermaSeguentRelacio(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio) As Integer
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        obtenirGermaSeguentRelacio = "-1"

        GAIA2.bdr(objConn, "SELECT relacioGermanaSuperior.RELINCOD,relacioGermanaSuperior.RELCDORD FROM METLREL as relacioGermanaSuperior WITH(NOLOCK), METLREL as relacio  WITH(NOLOCK) WHERE relacio.RELINCOD=" + rel.incod.ToString() + " AND relacioGermanaSuperior.RELCDHER like relacio.RELCDHER AND   relacioGermanaSuperior.RELCDORD>relacio.RELCDORD  AND relacioGermanaSuperior.RELCDSIT<98 AND relacio.RELCDSIT<98 ORDER BY relacioGermanaSuperior.RELCDORD ASC", DS)

        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            obtenirGermaSeguentRelacio = dbRow("RELINCOD")
        End If
        DS.Dispose()
    End Function 'obtenirGermaSeguentRelacio




    '****************************************************************************************************************
    '	Funció: GAIA.obtenirAccioNodeCircuit
    '	Entrada: codiNode
    '					 accAprovacio: per referencia, posarem el valor de l'acció que executarem en cas d'aprovació
    '						accDenegacio: per referencia, posarem el valor de l'acció que executarem en cas d'aprovació
    '*****************************************************************************************************************
    Public Shared Sub obtenirAccioNodeCircuit(ByVal objConn As OleDbConnection, ByVal relNodeCircuit As clsRelacio, ByRef accAprovacio As Integer, ByRef accDenegacio As Integer)
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()

        GAIA2.bdr(objConn, "SELECT * FROM METLNCI  WITH(NOLOCK) WHERE NCIINNOD=" + relNodeCircuit.infil.ToString(), DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            accAprovacio = dbRow("NCICDAAP")
            accDenegacio = dbRow("NCICDADE")
        End If
        DS.Dispose()
    End Sub 'obtenirAccioNodeCircuit


    '****************************************************************************************************************
    '	Funció: GAIA.caducarContinguts
    '	Entrada: 	relContingut: Relació que apunta al contingut que volem caducar
    '					
    '					 	codiUsuari: codi de l'usuari que ha iniciat el circuit. Si =0 llavors s'ha iniciat automàticament
    '											 des de l'Agent GAIA.
    '						codiRelacioContingutIniciador: codi de la relacio que apunta al contingut q ha iniciat el procés per el qual
    '																				aquest contingut estava al circuit. Per exemple, un document d'un contracte de contractació seria el "contingut", però la pàgina web on es mostra es el "contingutIniciador"
    '	Acció: 		
    '***************************************************************************************************
    Public Shared Sub caducarContinguts(ByVal objConn As OleDbConnection, ByVal relContingut As clsRelacio, ByVal codiUsuari As Integer)

        'Esborro contingut al que apunta la relació del servidor web destí
        'Esborro relació del contingut amb tots els seus fills (cal veure si he d'esborrar també físicament els fills, només si no els utilitza ningú més)		


        GAIA2.esborrarNode(objConn, relContingut.infil, relContingut, 1, codiUsuari, 1, ctESBORRATCADUCAT)


    End Sub 'caducarContinguts


    '***************************************************************************************************

    '	Funció: 	GAIA.f_establir permisos
    '	Entrada:	relacio: Codi de la relacio que apunta al fitxer que volem establir els permisos
    '				thumbnail: indica si es tracta d'una imatge petita, afegirem la P al final del nom del fitxer si ho és
    '	Acció: 		Dóna permisos als usuaris que tenen accés al fitxer
    '***************************************************************************************************
    Public Shared Sub f_establirPermisos(ByVal objConn As OleDbConnection, ByVal relacio As Integer, ByVal thumbnail As String, ByVal fitxerServidor As String)
        Dim ds As New DataSet, nomFitxer As String, nomFitxerFisic As String, nodeArrel As String

        Dim dbRow As DataRow, nomNode As String
        Dim permis As String
        Dim i As Integer

        nodeArrel = ""
        nomFitxerFisic = ""
        If 2 = 3 Then

            Try
                GAIA2.bdr(objConn, "SELECT RELDSFIT,RELCDHER FROM METLREL WITH(NOLOCK) WHERE RELINCOD = " & relacio, ds)
                If ds.Tables(0).Rows.Count() > 0 Then
                    'nomFitxer = mid(fitxerServidor,1,instr(fitxerServidor,".")-1) & thumbnail & mid(fitxerServidor,instr(fitxerServidor,"."))
                    nomFitxer = fitxerServidor
                    nomFitxerFisic = System.Web.HttpContext.Current.Server.MapPath(nomFitxer)
                    nodeArrel = ds.Tables(0).Rows(0)("RELCDHER")
                    nodeArrel = Mid(nodeArrel, 2, InStr(Mid(nodeArrel, 2), "_") - 1)
                End If
                nomNode = GAIA2.obtenirNomNode(objConn, nodeArrel)

                If Trim(nomNode) = "Intranet" Then
                    GAIA2.bdr(objConn, "SELECT DISTINCT PELINORG,isnull(FORDSWIN,'') AS USUARI,MIN(PERCDTIP) AS PERMIS " &
                          " FROM METLPER WITH(NOLOCK) LEFT JOIN METLPEL WITH(NOLOCK) ON PERCDREL = METLPEL.PELCDREL LEFT JOIN METLFOR  WITH(NOLOCK) " &
                          " ON METLPEL.PELINORG=METLFOR.FORINNOD WHERE PELCDREL=" & relacio &
                          " AND (isnull(FORDSWIN,'') <> '' OR PELINORG = 115969) " &
                          " GROUP BY PELINORG,isnull(FORDSWIN,'')", ds)



                    For Each dbRow In ds.Tables(0).Rows
                        i += 1
                        'RW per permisos 1,3,5,7
                        If CInt(dbRow("PERMIS")) <= 7 Then
                            permis = "RW"
                        Else 'si el permís més baix que té és major a 7, només permís de lectura.
                            permis = "R"
                        End If

                        'permis "Todos"
                        If dbRow("PELINORG") = "115969" Then
                            GAIA2.SetPermissions(nomFitxerFisic, "Todos", permis)
                        End If
                        If Trim(dbRow("USUARI")) <> "" Then
                            GAIA2.SetPermissions(nomFitxerFisic, "LH\" & Trim(dbRow("USUARI")), permis)
                            'SetPermissions("c:\\inetpub\\wwwroot\\gDocs\\d5880885.gif", "LH\" & trim(dbRow("USUARI")))
                        End If
                    Next
                End If
            Catch ex As Exception
                f_logError(objConn, "G01", ex.Source, ex.Message)
            End Try
        End If
    End Sub 'f_establirPermisos

    '***************************************************************************************************
    '	Funció: 	GAIA.f_logError
    '	Entrada:	objConn: Conexió de base de dades establerta
    '				origen: codi intern que indica on s'ha produit l'error.
    '				ex_source: propietat source de l'excepció que ha produit l'error
    '				text: propietat "message" de l'excepció que ha produit l'error
    '	Acció: 		Registra una entrada a la taula de log d'errors (METLERR)
    '***************************************************************************************************
    Public Shared Sub f_logError(ByVal objConn As OleDbConnection, ByVal origen As String, ByVal ex_source As String, ByVal text As String)
        GAIA2.bdSR(objConn, "INSERT INTO METLERR (ERRTPORG,ERRDSSRC,ERRDSDES) VALUES ('" &
        origen & "','" & Replace(ex_source, "'", "") & "','" & Replace(text, "'", "") & "')")
    End Sub


    '***************************************************************************************************
    '	Funció: 	GAIA.SetPermissions
    '	Entrada:	vPath: path complet del fitxer sobre el que apliquem els permisos
    '				UserName: nom de l'usuari al que donarem els permisos
    '				TipusPermis: tipus de permís:
    '							R : Lectura
    '							RW: Lectura i escriptura
    '	Acció: 		Registra una entrada a la taula de log d'errors (METLERR)
    '***************************************************************************************************
    Public Shared Sub SetPermissions(ByVal vPath As String, ByVal UserName As String, ByVal TipusPermis As String)


    End Sub 'setPermissions


    '****************************************************************************************************************
    '	Funció: GAIA.ftpCopiarFitxer
    '	Entrada: 	servidor: String amb el servidor destí on copiaré el fitxer
    '						usuari: usuari per fer login al ftp
    '						pwd: pwd
    '						forigen: Path+nom del fitxer origen
    '						fdesti: Path+nom del fitxer destí dins del servidor FTP
    '						relacio: número de relació
    '						thumbnail: indica si es tracta d'una imatge thumbnail (val "P" si ho és, "" en cas contrari)
    '	Acció: 		
    '***************************************************************************************************

    Public Shared Function ftpCopiarFitxer(ByVal objConn As OleDbConnection, ByVal servidor As String, ByVal usuari As String, ByVal pwd As String, ByVal fOrigen As String, ByVal fdesti As String, ByVal relacio As Integer, ByVal thumbnail As String, ByVal ftpsession As Session, Optional ByVal pathDestiArrel As String = "") As String
        ftpCopiarFitxer = ""
        Dim bCopiat As Boolean, fitxerServidor As String
        fOrigen = fOrigen.Replace("\\", "\").Replace("\\", "\").Replace("//", "/")
        Dim strErr As String = ""
        bCopiat = False

        Try
            If servidor = "" Then
                ftpCopiarFitxer &= "No he pogut copiar l'arxiu al servidor destí. Comproveu si la pàgina és dins d'una web."
                GAIA2.debug(objConn, "No he pogut copiar l'arxiu al servidor destí. Comproveu si la pàgina és dins d'una web.")
            Else
                strErr &= "a"
                Dim directori As FtpDirectory
                strErr &= "b"
                directori = ftpsession.CurrentDirectory
                strErr &= "c"
                'IF instr(LCASE(forigen),"/proves")>0 THEN				
                'no copio a la intranet (websgaia/web/proves)
                'ELSE
                Try
                    strErr &= "0"
                    directori.PutFile(fOrigen, fdesti.Replace("\\", "\"))
                    strErr &= "1"
                    bCopiat = True
                Catch ex As Exception

                    GAIA2.debug(objConn, "No he pogut copiar l'arxiu: forigen= " & fOrigen & "- fdesti=" & fdesti & " al servidor destí. Si us plau, prova de publicar de nou." & strErr & "_" & ex.Source & "--" & ex.Message)

                    Try
                        strErr &= "2"
                        directori.PutFile(fOrigen, fdesti)
                        strErr &= "3"
                        bCopiat = True
                    Catch ex2 As Exception
                        strErr &= "5"
                        ftpCopiarFitxer &= "No he pogut copiar l'arxiu: forigen= " & fOrigen & "- fdesti=" & fdesti & " al servidor destí. Si us plau, prova de publicar de nou." & strErr & "_" & ex2.Source & "--" & ex2.Message

                    End Try
                End Try
                'END IF
                If bCopiat Then
                    strErr &= "6"
                    fdesti = Replace(fdesti, "/GAIA06", "")
                    strErr &= "7"
                    fitxerServidor = Replace(fdesti, pathDestiArrel, "")
                    strErr &= "8"
                    'f_establirPermisos(objConn, relacio, thumbnail, fitxerServidor)
                End If

            End If

        Catch ex As Exception
            GAIA2.debug(objConn, "error al ftp: Forigen=" & fOrigen & "--Fdesti=" & fdesti & "-- servidor=" & servidor & "-- dades d'error: " & ex.Source & "--" & ex.Message & "strERr=" & strErr)
            Return ("error al copiar arxius ")
        End Try

    End Function 'ftpCopiarFitxer


    '***************************************************************************************************
    '	Funció: GAIA.ftpEsborrarFitxer
    '	Entrada: 	servidor: String amb el servidor destí on esborraré el fitxer
    '						usuariFTP: usuari per fer login al ftp
    '						pwdFTP: pwd'						
    '						fitxer: Path+nom del fitxer que volem esborrar
    '						codiUsuari: Usuari que vol realitzar l'acció
    '						codiRelacio: Codi de la relacio que apunta al fitxer que volem esborrar del servidor destí
    '	Acció: 		
    '***************************************************************************************************
    Public Shared Sub ftpEsborrarFitxer(ByVal objConn As OleDbConnection, ByVal servidor As String, ByVal usuariFTP As String, ByVal pwdFTP As String, ByVal fitxer As String, ByVal codiUsuari As Integer, ByVal rel As clsRelacio, ByVal borrarFitxerDefinitivament As Char)
        Dim ftpSession As Session
        Dim directori As FtpDirectory
        Dim imatgePetita As String
        ftpSession = New Session()


        Try

            GAIA2.log(objConn, rel, codiUsuari, "Fitxer:" + rel.noddstxt + "(" + fitxer + ") esborrat del servidor Intranet", TAESBORRAR)
            If borrarFitxerDefinitivament = "S" Then
                File.Delete("e:\docs\GAIA\" & fitxer.Substring(InStrRev(fitxer, "/")))
                GAIA2.bdSR(objConn, "UPDATE METLDOC SET DOCDSFIT='' WHERE DOCINNOD=" & rel.infil.ToString())
            End If

        Catch ex As Exception
            f_logError(objConn, "G01", ex.Source, ex.Message + "Fitxer no trobat. No s'ha pogut esborrar el fitxer:" + fitxer + " del servidor Intranet")
            'ex.Message
        End Try



        Try
            ftpSession.Server = servidor
            ftpSession.Connect(usuariFTP, pwdFTP)
            directori = ftpSession.CurrentDirectory

            directori.RemoveFile(fitxer)
            GAIA2.log(objConn, rel, codiUsuari, "Fitxer:" + rel.noddstxt + "(" + fitxer + ")  esborrat del servidor:" + servidor, TAESBORRAR)

            'GAIA.log(objConn, rel, codiUsuari, "Fitxer:" + rel.noddstxt + "(" + fitxer + ")  esborrat definitivament de l'úbicació mestre", TAESBORRAR)


            Try
                'Esborro la imatge petita (si existeix)            
                imatgePetita = fitxer.Substring(0, InStrRev(fitxer, ".") - 1) + "P." + fitxer.Substring(InStrRev(fitxer, "."))
                File.Delete("e:\docs\GAIA\" + imatgePetita.Substring(InStrRev(imatgePetita, "/")))
                directori.RemoveFile(imatgePetita)
            Catch
            End Try
        Catch ex As Exception
            f_logError(objConn, "G01", ex.Source, ex.Message + "Fitxer no trobat. No s'ha pogut esborrar el fitxer:" + fitxer + " del servidor:" + servidor)
        End Try
        ftpSession.Close()

    End Sub 'ftpEsborrarFitxer

    '***************************************************************************************************
    '	Funció: GAIA.nouFitxer
    '	Entrada: 	nomFitxer: contindrà el nom del fitxer al finalitzar la funció
    '				tipusDocument: contindrà el tipus del fitxer al finalitzar la funció
    '				txtThor: si el fitxer es una imatge, contindrà la seva amplada al finalitzar la funció
    '				txtTver: si el fitxer es una imatge, contindrà la seva altura al finalitzar la funció
    '				txtTamany: contindrà el tamany del fitxer al finalitzar la funció

    '				postedFile: es el fitxer recollir per post del que extraurem les dades
    '	Acció: 		
    '***************************************************************************************************



    Public Shared Sub nouFitxer(ByRef nomFitxer As String, ByRef tipusDocument As String, ByRef txtThor As String, ByRef txtTver As String, ByRef txtTamany As String, ByRef postedFile As HttpPostedFile)
        Dim thor, tver As Integer
        Dim nomFitxerImatgePetita As String
        Dim nomFitxerImatge100px As String
        Dim nomAleatori As String
        Dim savePath As String = "e:\docs\GAIA\"
        Dim r As New Random()
        Dim cont As Integer = 1
        Dim pos As Integer
        Dim sufix, sufixVell As String
        Dim objconn As OleDbConnection = Nothing
        sufix = ""
        sufixVell = ""
        If Not (postedFile Is Nothing) Then
            Dim filename As String = Path.GetFileName(postedFile.FileName)
            Dim contentLength As Integer = postedFile.ContentLength

            txtTamany = postedFile.ContentLength.ToString()

            If nomFitxer.Length = 0 Then
                tipusDocument = GAIA2.trobaTipusDocument(objconn, postedFile.ContentType)
                'Busco un nom aleatori
                nomAleatori = "d" & r.Next(9999999).ToString()

                pos = InStrRev(filename, ".")
                If pos <= 0 Then
                    pos = 0
                End If
                If pos = 0 Then
                    sufixVell = ""
                    sufix = ""
                Else
                    sufixVell = filename.Substring(pos - 1)
                    If UCase(sufixVell) = ".JPEG" Then
                        sufixVell = ".jpg"
                    End If
                    If InStr(UCase(postedFile.ContentType), "IMAGE") > 0 Then
                        sufix = ".jpg"
                    Else
                        sufix = filename.Substring(pos - 1)
                    End If
                End If
                While File.Exists(savePath.ToString() & nomAleatori.ToString() & sufix)
                    cont = cont + 1
                    nomAleatori = "d" & r.Next(9999999).ToString()
                End While
                nomFitxer = nomAleatori
                nomFitxerImatgePetita = nomAleatori + "P"
                nomFitxerImatge100px = nomAleatori + "P100"
            Else
                'He d'esborrar el fitxer actual (i l'imatge petita si hi hagués)
                File.Delete(savePath + nomFitxer)

                nomFitxer = nomFitxer.Substring(0, InStrRev(nomFitxer, ".") - 1)
                nomFitxerImatgePetita = nomFitxer + "P"
                nomFitxerImatge100px = nomFitxer + "P100"
                If File.Exists(savePath + nomFitxerImatgePetita + ".jpg") Then
                    File.Delete(savePath + nomFitxerImatgePetita + ".jpg")
                End If
                If File.Exists(savePath + nomFitxerImatge100px + ".jpg") Then
                    File.Delete(savePath + nomFitxerImatge100px + ".jpg")
                End If
                'Si el nou fitxer que substitueix a l'anterior no és una imatge em quedo la seva extensió
                If InStr(UCase(postedFile.ContentType), "IMAGE") > 0 Then
                    sufix = ".jpg"
                Else
                    sufix = filename.Substring(InStrRev(filename, ".") - 1)
                End If
            End If



            If InStr(UCase(postedFile.ContentType), "IMAGE") > 0 Then
                Dim imatge As System.Drawing.Image

                postedFile.SaveAs(savePath + nomFitxer + sufixVell)
                'Faig un canvi de format
                If UCase(sufixVell) <> ".JPG" And UCase(sufixVell) <> ".JPEG" Then
                    imatge = System.Drawing.Image.FromFile(savePath + nomFitxer + sufixVell)
                    'La imatge amb la mida original la gravo amb un factor de compressió del 95%
                    SaveJPGWithCompressionSetting(imatge, savePath + nomFitxer + sufix, 99)
                    'Si NO és un .jpg i tampoc un .gif animat o un PNG l'esborro
                    Select Case UCase(sufixVell)
                        Case ".GIF"

                            'Create a new FrameDimension object from this image
                            Dim FrameDimensions As System.Drawing.Imaging.FrameDimension = New System.Drawing.Imaging.FrameDimension(imatge.FrameDimensionsList(0))

                            'Determine the number of frames in the image
                            'Note that all images contain at least 1 frame, but an animated GIF
                            'will contain more than 1 frame.
                            Dim NumberOfFrames As Integer = imatge.GetFrameCount(FrameDimensions)
                            imatge.Dispose()
                            If NumberOfFrames < 1 Then 'NO és un gif animat
                                'Esborro la imatge en format no dessitjat
                                File.Delete(savePath + nomFitxer + sufixVell)
                            End If
                        Case ".PNG"
                            'no faig res, ja l'he gravat i no l'esborro
                        Case Else
                            imatge.Dispose()
                            'Esborro la imatge en format no dessitjat	
                            File.Delete(savePath + nomFitxer + sufixVell)
                    End Select

                End If
            Else
                postedFile.SaveAs(savePath + nomFitxer + sufix)
            End If



            tver = 0
            thor = 0
            'Si és una imatge la tracto i poso el thumbnail
            If InStr(UCase(postedFile.ContentType), "IMAGE") > 0 Then
                Dim fullSizeImg As System.Drawing.Image
                Dim tamanyHorPetit As Integer = 700
                Dim tamanyHorPetit2 As Integer = 200
                fullSizeImg = System.Drawing.Image.FromFile(savePath + nomFitxer + sufix)

                'fullSizeImg.Save(savePath & nomFitxer, ImageFormat.jpeg)			
                tver = fullSizeImg.Height
                thor = fullSizeImg.Width

                'elimino el thumbnail dintre de la imatge original (si ve d'una camara digital)
                fullSizeImg.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone)
                fullSizeImg.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone)

                Dim dummyCallBack As System.Drawing.Image.GetThumbnailImageAbort
                dummyCallBack = New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)

                Dim thumbNailImg As System.Drawing.Image
                'gravo la imatge a la mida que marca tamanyHorPetit
                thumbNailImg = fullSizeImg.GetThumbnailImage(tamanyHorPetit, (tver * 100) / ((thor * 100) / tamanyHorPetit), dummyCallBack, IntPtr.Zero)
                thumbNailImg.Save(savePath + nomFitxerImatgePetita + sufix, ImageFormat.Jpeg)


                thumbNailImg = fullSizeImg.GetThumbnailImage(tamanyHorPetit2, (tver * 100) / ((thor * 100) / tamanyHorPetit2), dummyCallBack, IntPtr.Zero)
                thumbNailImg.Save(savePath + nomFitxerImatge100px + sufix, ImageFormat.Jpeg)



                thumbNailImg.Dispose()
                fullSizeImg.Dispose()

            End If
        End If
        If UCase(sufixVell) = ".GIF" Then
            nomFitxer = nomFitxer + sufixVell
        Else
            nomFitxer = nomFitxer + sufix
        End If
        txtThor = thor.ToString()
        txtTver = tver.ToString()
    End Sub 'Nou Fitxer


    'Funció per definir el nivell de compressió amb que gravarem el jpg
    Public Shared Sub SaveJPGWithCompressionSetting(ByVal image As System.Drawing.Image, ByVal szFileName As String, ByVal lCompression As Long)
        Dim eps As EncoderParameters = New EncoderParameters(1)
        eps.Param(0) = New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, lCompression)
        Dim ici As ImageCodecInfo = GetEncoderInfo("image/jpeg")
        image.Save(szFileName, ici, eps)
    End Sub

    Public Shared Function GetEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
        Dim j As Integer
        Dim encoders As ImageCodecInfo()
        encoders = ImageCodecInfo.GetImageEncoders()
        For j = 0 To encoders.Length
            If encoders(j).MimeType = mimeType Then
                Return encoders(j)
            End If
        Next j

        Return Nothing
    End Function




    Protected Shared Function ThumbnailCallback() As Boolean
        Return False


    End Function

    '***************************************************************************************************

    '	Funció: GAIA.trobaServidorDesti
    '	Entrada: 	codiRelacio: codi de la relacio del node que volem publicar.
    '						servidor: String on guardaré el servidor FTP destí 
    '						usuari:  String on guardaré l'usuari per fer login al ftp
    '						pwd:  String on guardaré la contrasenya per fer login al ftp
    '						pathDesti: Path arrel del servidor destí
    '						pathDocsDesti: Path arrel del servidor destí per emmagatzemar documents
    '	Acció: 		Busca el node que conté la informació del arbre on està el node apuntat per codiRelacio i 
    '						retorna la informació necessaria per accedir al servidor FTP on copiarem els fitxers.
    '***************************************************************************************************
    Public Shared Sub trobaServidorDesti(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByRef servidor As String, ByRef usuari As String, ByRef pwd As String, ByRef pathDesti As String, ByRef pathDocsDesti As String, ByRef port As String, ByRef URL As String)
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        GAIA2.bdr(objConn, "SELECT SERDSDIR,  SERDSUSR, SERDSPWD, SERDSCAM, SERDSPRT, SERDSURL, AWEDSROT, AWEDSDOC FROM METLREL WITH(NOLOCK) INNER JOIN METLAWE  WITH(NOLOCK) ON SUBSTRING(METLREL.RELCDHER, 2, CHARINDEX('_',SUBSTRING(METLREL.RELCDHER,2,LEN(METLREL.RELCDHER)))-1) = METLAWE.AWEINNOD INNER JOIN METLSER WITH(NOLOCK) ON METLAWE.AWEDSSER = METLSER.SERINCOD WHERE (METLREL.RELINCOD=" + rel.incod.ToString() + ")", DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            servidor = dbRow("SERDSDIR").Trim
            pathDesti = dbRow("SERDSCAM").Trim '+dbRow("AWEDSROT")).Trim
            pathDocsDesti = (pathDesti + dbRow("AWEDSDOC")).Trim
            usuari = dbRow("SERDSUSR").Trim
            pwd = dbRow("SERDSPWD").Trim


            If servidor.Trim() = "www.l-h.es" Then
                servidor = "www.l-h.cat"
            End If
            URL = dbRow("SERDSURL")
        End If


        DS.Dispose()
    End Sub 'trobaServidorDesti


    Public Shared Function trobaURLDesti(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio) As String
        Dim DS As DataSet
        DS = New DataSet()
        Dim URL As String = ""

        GAIA2.bdr(objConn, "SELECT  RELCDHER,SERDSURL FROM METLREL WITH(NOLOCK) INNER JOIN METLAWE WITH(NOLOCK) ON SUBSTRING(METLREL.RELCDHER, 2, CHARINDEX('_',SUBSTRING(METLREL.RELCDHER,2,LEN(METLREL.RELCDHER)))-1) = METLAWE.AWEINNOD INNER JOIN METLSER WITH(NOLOCK) ON METLAWE.AWEDSSER = METLSER.SERINCOD WHERE (METLREL.RELINCOD=" + rel.incod.ToString() + ")", DS)


        If DS.Tables(0).Rows.Count > 0 Then
            'Tracto els webs de barradas i teatre joventut, que tenen domini diferent però són dins de l'arbre de l'aju
            If InStr(DS.Tables(0).Rows(0)("RELCDHER"), "_167397_174103_174959") > 0 Then
                URL = "www.auditoribarradas.cat"
            Else
                If InStr(DS.Tables(0).Rows(0)("RELCDHER"), "_167397_174103_176060") > 0 Then
                    URL = "www.teatreJoventut.cat"
                Else
                    URL = DS.Tables(0).Rows(0)("SERDSURL")
                End If
            End If



        End If
        DS.Dispose()
        Return (URL)
    End Function 'trobaURLDesti


    'Cerca l'adreça d'un contingut de tipus enllaç METLLNK i retorna l'adreça i el target
    'RelIni : Contingut GAIA que incorpora l'enllaç. Necessari només si l'enllaç no és node relacionat
    'target
    Public Shared Function obtenirEnllacSimple(ByVal objconn As OleDbConnection, ByVal codiEnllaç As Integer, ByVal idioma As Integer, ByRef target As Integer, ByVal urlDesti As String) As String
        Return obtenirEnllaçSimple(objconn, codiEnllaç, idioma, target, urlDesti)

    End Function

    Public Shared Function obtenirEnllaçSimple(ByVal objconn As OleDbConnection, ByVal codiEnllaç As Integer, ByVal idioma As Integer, ByRef target As Integer, ByVal urlDesti As String) As String
        Dim DS As DataSet
        Dim ds2 As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim enllaç As String = ""
        Dim urlDestiEnllaç As String = ""
        Dim oCodParam As New lhCodParam.lhCodParam
        Dim relEnllaç As Integer = 0

        target = 0


        GAIA2.bdr(objconn, "SELECT LNKDSLNK,ISNULL(LNKCDREL,0) as LNKCDREL,LNKWNTIP, REIDSFIT, DOCDSFIT,DOCINNOD as DOCINNOD FROM METLLNK WITH(NOLOCK) LEFT OUTER JOIN METLREL WITH(NOLOCK) ON  RELCDSIT<98 AND METLREL.RELINCOD = METLLNK.LNKCDREL LEFT OUTER JOIN METLREI WITH(NOLOCK) ON (  METLREI.REIINCOD = METLLNK.LNKCDREL) AND REIINIDI=LNKINIDI LEFT OUTER JOIN METLDOC WITH(NOLOCK) ON DOCINNOD=RELINFIL AND DOCINIDI=LNKINIDI  LEFT OUTER JOIN METLTDO WITH(NOLOCK) ON DOCINTDO=TDOCDTDO  WHERE (METLLNK.LNKINNOD =" & codiEnllaç & ") AND (METLLNK.LNKINIDI=" & idioma & ")", DS)



        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            enllaç = IIf(IsDBNull(dbRow("REIDSFIT")), dbRow("LNKDSLNK"), dbRow("REIDSFIT"))
            'és un document i selecciono el programa per obrir-ho.
            'Per ara no tinc en compte documents diferents d'imatges i fitxers, caldria incorporar vídeo i flash
            If enllaç = "" Then
                If IsDBNull(dbRow("DOCINNOD")) Then

                    If IsDBNull(dbRow("LNKCDREL")) Then
                        'El fitxer està penjant del contingut (97)
                        ds2 = New DataSet()
                        GAIA2.bdr(objconn, "SELECT RELCDRSU, RELINFIL FROM METLREL WHERE RELINPAR=" & codiEnllaç & " AND RELCDSIT=97 ORDER BY RELCDORD", ds2)
                        If ds2.Tables(0).Rows.Count > 0 Then
                            enllaç = "/utils/obrefitxer.ashx?" & HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & ds2.Tables(0).Rows(0)("RELINFIL") & "&codiIdioma=" & idioma))
                            relEnllaç = ds2.Tables(0).Rows(0)("RELCDRSU")
                        End If
                        ds2.Dispose()
                    Else

                        relEnllaç = dbRow("LNKCDREL")
                        Dim params As String = ""
                        Dim relContingut As New clsRelacio
                        relContingut.bdget(objconn, relEnllaç)
                        GAIA2.obtenirEnllacContingut(objconn, relContingut, idioma, relContingut.tipintip, "", "", 0, 0, "", 0, "", enllaç, params, "")

                        enllaç = enllaç & params
                    End If
                Else
                    enllaç = "/utils/obrefitxer.ashx?" & HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbRow("DOCINNOD") & "&codiIdioma=" & idioma & "&f=" & dbRow("DOCDSFIT")))
                End If
            Else
                If dbRow("LNKCDREL") <> 0 Then relEnllaç = 262731
            End If


            If InStr(enllaç, "http") = 0 Then
                'Si l'enllaç està a un altre servidor de gaia poso el domini
                Dim relContingut As New clsRelacio
                If Not relEnllaç = 0 Then
                    relContingut.bdget(objconn, relEnllaç)
                    urlDestiEnllaç = GAIA2.trobaURLDesti(objconn, relContingut)
                End If

                If urlDesti = "" Then
                    enllaç = "http://" & urlDestiEnllaç & enllaç
                Else
                    If InStr(urlDesti, "/") > 0 Then
                        If urlDesti.Substring(0, urlDesti.IndexOf("/")) <> urlDestiEnllaç And urlDestiEnllaç <> "" Then

                            enllaç = "http://" & urlDestiEnllaç & enllaç

                        End If
                    End If
                End If


            End If
            target = DS.Tables(0).Rows(0)("LNKWNTIP")
        End If
        DS.Dispose()
        oCodParam = Nothing
        Return (enllaç)

    End Function 'obtenirEnllaçSimple




    '****************************************************************************************************************
    '	Funció: preparaPagina
    '	Entrada: 
    '		html: string amb el codi html que he de representar
    '		css:  string amb la definició de l'estructura de la pàgina
    '		heretatPropietatsNodesSuperiors: si 1 llavors heretarà les propietats dels nodes "node web" i "arbre web" superiors.
    '																			En aquest cas només serà posar-hi <div id="cos">html</div> si s'heretan propietats
    '	Procés/sortida: 	
    '		Retorna un string que consisteix en una pàgina web ben formada i accessible per que es pugui publicar (gravar)
    '****************************************************************************************************************	
    Public Shared Function preparaPagina(ByVal objConn As OleDbConnection, ByVal html As String, ByVal css As String, ByVal heretatPropietatsNodesSuperiors As Integer, ByVal titol As String, ByVal heretaPropietats As Integer, ByVal estilBody As String, ByVal esForm As String, ByVal esEML As String, ByVal esSSL As String, ByVal tagsMeta As String, ByRef htmlPeu As String, ByVal strCSSPantalla As String, ByVal strCSSImpressora As String, ByVal idioma As Integer) As String
        Dim textCSS As String = ""
        Dim arrayCSS1 As String()
        Dim arrayCSS2 As String()
        Dim item As String
        Dim textIniciForm As String = ""
        Dim textFiForm As String = ""
        Dim strIdioma As String = ""

        Dim tagsMetaComuns As String = ""
        Select Case idioma
            Case 2
                strIdioma = "es"
            Case 3
                strIdioma = "en"
            Case Else
                strIdioma = "ca"
        End Select


        If strCSSPantalla.Trim().Length > 0 Then
            arrayCSS1 = strCSSPantalla.Split(";")
            If esSSL = "S" Then
                For Each item In arrayCSS1
                    If item.Trim().Length > 0 Then
                        textCSS += "<link rel=""stylesheet"" href=""" & item.Trim().Replace("http:", "https:").Replace(".css", "_segur.css") & """ type=""text/css"" media=""screen""/>"
                    End If
                Next item
            Else
                For Each item In arrayCSS1
                    If item.Trim().Length > 0 Then
                        textCSS += "<link rel=""stylesheet"" href=""" + item.Trim() + """ type=""text/css"" media=""screen""/>"
                    End If
                Next item
            End If
        End If
        If strCSSImpressora.Trim().Length > 0 Then
            arrayCSS2 = strCSSImpressora.Split(";")
            If esSSL = "S" Then
                For Each item In arrayCSS2
                    If item.Trim().Length > 0 Then
                        textCSS += "<link rel=""stylesheet"" href=""" & item.Trim().Replace("http:", "https:").Replace(".css", "_segur.css") & """ type=""text/css"" media=""print""/>"
                    End If
                Next item
            Else
                For Each item In arrayCSS2
                    If item.Trim().Length > 0 Then
                        textCSS += "<link rel=""stylesheet"" href=""" + item.Trim() + """ type=""text/css"" media=""print""/>"
                    End If
                Next item
            End If
        End If

        If esForm = "S" Then
            textIniciForm = "<form id=""form1"" runat=""server"">"
            textFiForm = "</form>"
        End If
        If esEML = "N" Then
            html = " <div id=""contenidor"">" + textIniciForm + html + textFiForm + "</div>"
        Else
            html = textIniciForm + html + textFiForm
        End If
        'Si és un correu poso un cap reduït
        If esEML = "S" Then
            preparaPagina = "<html><head runat=""server""><asp:Literal id=""lblhead"" runat=""server""></asp:Literal></head><body  id=""cosPagina"" style=""" + estilBody + """>" + html + "</body></html>"
        Else
            titol = GAIA2.netejaHTML(titol)
            'tagsMetaComuns +=	"<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/>"
            tagsMetaComuns += "<asp:Literal id=""lblhead"" runat=""server""></asp:Literal>"
            tagsMetaComuns += "<meta http-equiv=""Content-Type"" content=""text/html; charset=iso-8859-1""/>"
            tagsMetaComuns += "<meta http-equiv=""Content-Style-Type"" content=""text/css""/>"
            tagsMetaComuns += "<meta http-equiv=""Content-Language"" content=""" + strIdioma + """/>"
            tagsMetaComuns += "<meta http-equiv=""Content-Script-Type"" content=""text/javascript""/>"
            tagsMetaComuns += "<meta name=""Author"" content=""Ajuntament de L'Hospitalet""/><meta name=""DC.Creator"" content=""Ajuntament de L'Hospitalet""/>"
            tagsMetaComuns += "<meta name=""DC.Identifier"" content=""http://www.l-h.cat""/>"
            tagsMetaComuns += "<meta name=""title"" content=""" + titol + """ runat=""server"" id=""metatitle""/><meta name=""DC.title"" content=""" + titol + """/>"
            tagsMetaComuns += "<link rel=""schema.DC"" href=""http://purl.org/dc/elements/1.1/""/>"
            tagsMetaComuns += "<meta name=""DC.Language"" content=""" + strIdioma + """/>"
            tagsMetaComuns += "<meta name=""Copyright"" content=""http://www.l-h.cat/gdocs/d6779062.pdf""/>"
            tagsMeta = tagsMetaComuns + tagsMeta

            'preparaPagina = "<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd""><html xmlns=""http://www.w3.org/1999/xhtml"" lang=""" + strIdioma + """><head runat=""server"">" + css + "<title>" & titol & "</title>" + tagsMeta

            preparaPagina = "<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd""><!--[if lt IE 7]>      <html class=""no-js lt-ie9 lt-ie8 lt-ie7"" lang=""" & strIdioma & """> <![endif]--><!--[if IE 7]>         <html class=""no-js lt-ie9 lt-ie8"" lang=""" & strIdioma & """> <![endif]--><!--[if IE 8]>         <html class=""no-js lt-ie9"" lang=""" & strIdioma & """> <![endif]--><!--[if gt IE 8]><!--><html class=""no-js"" xmlns=""http://www.w3.org/1999/xhtml"" lang=""" & strIdioma & """><!--<![endif]--><head runat=""server"">" + css + "<title>" & titol & "</title>" + tagsMeta
            preparaPagina &= textCSS & htmlPeu & "</head><body id=""cosPagina"" " & IIf(estilBody.Trim.Length > 0, " class=""" + estilBody + """", "") & ">" + html + "</body></html>"
        End If

        'l'editor de texte enriquit que utilitzem posa <p> i <br /> i no ens interessa per l'accessibilitat.
        preparaPagina = preparaPagina.Replace("<p>", "").Replace("</p>", "<br/>") '.replace("<br />","<br>").replace("<br/>","<br>")
        If esSSL = "S" Then
            preparaPagina = preparaPagina.Replace("<img src=""http:", "<img src=""https:")
        End If
    End Function 'preparaPagina



    '****************************************************************************************************************
    '	Funció: obreFulla
    '	Entrada: 
    '		codiRelacio:  
    '		plantilla:
    '		idioma: 
    '		codiUsuari:
    '	Procés/sortida: 	
    '		Retorna un string que consisteix en una pàgina web ben formada i accessible per que es pugui publicar (gravar)
    '****************************************************************************************************************	
    Public Shared Function obreFulla(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal plantilla As clsPlantilla, ByVal idioma As Integer, ByVal codiUsuari As Integer, ByVal publicar As Integer, ByVal width As Double) As String
        obreFulla = obreFulla(objConn, rel, plantilla, idioma, codiUsuari, publicar, width, 0, Now, "-1", -1, "N")
    End Function


    Public Shared Function obreFulla(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal plantilla As clsPlantilla, ByVal idioma As Integer, ByVal codiUsuari As Integer, ByVal publicar As Integer, ByVal width As Double, ByVal dataSimulacio As DateTime) As String
        obreFulla = obreFulla(objConn, rel, plantilla, idioma, codiUsuari, publicar, width, 0, dataSimulacio, "-1", -1, "N")
    End Function

    Public Shared Function obreFulla(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal plantilla As clsPlantilla, ByVal idioma As Integer, ByVal codiUsuari As Integer, ByVal publicar As Integer, ByVal width As Double, ByVal node As Integer, ByVal dataSimulacio As DateTime) As String


        obreFulla = obreFulla(objConn, rel, plantilla, idioma, codiUsuari, publicar, width, 0, dataSimulacio, "-1", -1, "N")
    End Function



    'Si head = -1 retornaré css+html, en cas contrari, retornaré l'html i el css el posaré a la variable head
    Public Shared Function obreFulla(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal plantilla As clsPlantilla, ByVal idioma As Integer, ByVal codiUsuari As Integer, ByVal publicar As Integer, ByVal width As Double, ByVal node As Integer, ByVal dataSimulacio As DateTime, ByRef head As String, ByVal nrocontingut As Integer) As String
        obreFulla = obreFulla(objConn, rel, plantilla, idioma, codiUsuari, publicar, width, 0, dataSimulacio, head, nrocontingut, "N")
    End Function


    'Si head = -1 retornaré css+html, en cas contrari, retornaré l'html i el css el posaré a la variable head
    Public Shared Function obreFulla(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal plantilla As clsPlantilla, ByVal idioma As Integer, ByVal codiUsuari As Integer, ByVal publicar As Integer, ByVal width As Double, ByVal node As Integer, ByVal dataSimulacio As DateTime, ByRef head As String, ByVal nrocontingut As Integer, ByVal escorreu As String) As String

        obreFulla = ""
        Dim sortida As String = ""

        If GAIA2.contingutVisibleAInternet(objConn, rel.infil) = 0 Then
            If codiUsuari = 0 Then

                sortida &= "<div class=""missatgeError""><div class=""icona"">"
                If idioma = 1 Then
                    sortida &= "Contingut no disponible<br />"
                Else
                    sortida &= "Contenido no disponible<br />"
                End If
                sortida &= "</div></div>"

                Return (sortida)
            Else
                If rel.tipintip <> 51 Then 'només si no ès de tipus tràmit. En aquests posem l'avís al detall de tràmit
                    sortida &= "<div class=""missatgeAvis""><div class=""icona"">Aquest contingut no és visible per als ciutadans.<br>S'està mostrant per que has accedit des de la xarxa municipal</div></div>"

                End If
            End If




        End If


        obreFulla = GetHTML(objConn, "obreFulla", 0, idioma, dataSimulacio, head, rel, rel, -1, codiUsuari, plantilla.innod)
        If obreFulla = "" Then
            obreFulla &= sortida

            Dim estilBody As String = ""
            Dim strCSSPantalla As String = ""
            Dim strCSSImpressora As String = ""
            Dim tagsMeta As String = ""
            Dim esForm As String = "N"
            Dim esEML As String = escorreu
            Dim esSSL As String = "N"
            Dim htmlPeu As String = ""
            Dim fdesti, urldesti, nomFitxerPublicat As String
            Dim html, css As String
            Dim height As Integer
            Dim llistaDocuments As String() = {}
            Dim titol As String = ""
            'Algunes crides a obreFulla posen escorreu=0
            If escorreu = "0" Then
                escorreu = "N"
            End If

            obreFulla = ""
            html = ""
            css = ""
            fdesti = ""
            urldesti = ""
            Dim nivell As Integer = 1
            'plantilla=56676
            nomFitxerPublicat = rel.dsfit
            If nomFitxerPublicat.Length > 0 Then
                fdesti += "/" + nomFitxerPublicat
                urldesti += "/" + nomFitxerPublicat
            Else
                fdesti = ""
                urldesti = ""
            End If

            Dim DS As DataSet

            DS = New DataSet()

            If node > 0 Then
                GAIA2.bdr(objConn, "SELECT RELINCOD FROM METLREL WITH(NOLOCK) WHERE (RELINFIL = " + node.ToString() + ") AND (RELCDSIT<99)", DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    rel.bdget(objConn, DS.Tables(0).Rows(0)("RELINCOD"))
                End If
            End If

            ' Si codiPlantilla no apunta a cap plantilla,llavors busquem la plantilla per defecte per representar el objecte.
            If plantilla.innod = 0 And rel.tipintip <> 5 Then
                plantilla.bdget(objConn, GAIA2.plantillaPerDefecte(objConn, rel, idioma))
            End If

            If plantilla.innod > 0 Then
                'ara ompliré cada cel·la amb el contingut de la plantilla.			



                GAIA2.dibuixaPreview(objConn, plantilla, plantilla.est, plantilla.arrayAtr, plantilla.hor, plantilla.ver, plantilla.tco, "", html, css, width, height, "p", rel, 0, fdesti, urldesti, nrocontingut, llistaDocuments, publicar, idioma, plantilla.css, rel, dataSimulacio, codiUsuari, 0, "", 0, plantilla.flw, titol, nivell, estilBody, "", esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, plantilla.niv)
                Dim regexp As Regex
                Dim strInPattern As String


                html = html.Replace("<u>", "<span class=""subratllat"">").Replace("<em>", "<span class=""cursiva"">").Replace("<i>", "<span class=""cursiva"">").Replace("<strong>", "<span class=""bold"">")

                strInPattern = "</u>|</i>|</strong>|</em>"
                regexp = New Regex(strInPattern, RegexOptions.IgnoreCase)
                html = Regex.Replace(html, strInPattern, "</span>", RegexOptions.IgnoreCase)
                regexp = Nothing



                css = GAIA2.netejaCss(css)

                If head = "-1" Then
                    obreFulla = css + html
                Else
                    obreFulla = html
                    head = css
                End If


            Else
                If rel.tipdsdes = "fulla missatge" Then
                    obreFulla = "<script language=""JavaScript"" type=""text/javascript"">window.open("" /GAIA/aspx/missatges/obrirmissatge.aspx?id=" + rel.infil.ToString() + "&amp;idiArbre=" + idioma.ToString() + """);window.close();</script>"
                Else
                    Dim oCodParam As New lhCodParam.lhCodParam
                    obreFulla = "<script language=""JavaScript"" type=""text/javascript"">window.open(""/utils/obrefitxer.ashx?"
                    obreFulla &= HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & rel.infil & "&codiIdioma=" & idioma))
                    obreFulla &= """);window.close();</script>"


                End If
            End If

            DS.Dispose()

            'gravo fulla obtinguda per a posteriors publicacions
            Try
                GAIA2.bdSR(objConn, " INSERT INTO METLCEL VALUES (0," & idioma & "," & rel.incod & "," & rel.incod & ",-1," & codiUsuari & "," & rel.infil & "," & plantilla.innod & ",getdate(),'" & dataSimulacio & "','" & css & "','" & obreFulla.Replace("'", "''") & "','obreFulla?" & idioma & "_" & rel.incod & "_" & rel.incod & "_" & codiUsuari & "_" & plantilla.innod & "')")
            Catch
            End Try
        End If

        obreFulla = sortida & obreFulla
    End Function 'obreFulla

    '****************************************************************************************************************
    '	Funció: afegeixAccioMantenimentPDF
    '	Entrada:  objecte relació que volem afegir 
    '	Procés/sortida: 	
    '		afegeix un registre en la TAULA METLMAI amb l'objectiu de crear un pdf amb el contingut
    '****************************************************************************************************************	
    Public Shared Sub afegeixAccioMantenimentPDF(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio)
        Try
            Dim DS As DataSet
            DS = New DataSet()
            GAIA2.bdr(objConn, "SELECT * FROM METLMAI WHERE MAICDREL=" & rel.incod & " AND MAIDTTEM=0", DS)
            'si ja existia la petició de manteniement, n
            If DS.Tables(0).Rows.Count = 0 Then
                GAIA2.bdSR(objConn, "INSERT INTO METLMAI (MAICDREL, MAIDTDAT, MAICDNOD, MAIDTTEM) VALUES (" & rel.incod & ",getdate()," & rel.infil & ", 0)")
            End If
            DS.Dispose()
        Catch
            GAIA2.f_logError(objConn, "GAIA", "afegeixAccioMantenimentPDF", Err.Description)
        End Try
    End Sub


    '******************************************************************************************************************************
    '	Funció: afegeixAccioMantenimentPDF
    '	Procés/sortida: 	
    '		genera un pdf en català i un altre en castellà per cada registre que es troba a la taula METLMAI (manteniment impressió)
    '********************************************************************************************************************************	
    Public Shared Function mantenimentPDFS() As Integer
        Dim DS As DataSet
        Dim objconn As OleDbConnection
        objconn = GAIA2.bdIni()
        Dim rel As New clsRelacio()
        Dim dbRow As DataRow
        Dim dataIni As Date
        Dim cont As Integer = 0
        DS = New DataSet()

        Try
            GAIA2.bdr(objconn, "SELECT * FROM METLMAI WITH(NOLOCK) WHERE MAIDTTEM=0 ORDER BY MAIINCOD", DS)
            For Each dbRow In DS.Tables(0).Rows
                rel.bdget(objconn, dbRow("MAICDREL"))

                If rel.incod = 0 Then 'han esborrat el contingut i per tant no cal fer res
                    GAIA2.bdSR(objconn, "DELETE FROM  METLMAI WHERE MAIINCOD=" & dbRow("MAIINCOD"))
                Else
                    dataIni = Now
                    cont += GAIA2.generaPDF(objconn, rel)
                    GAIA2.bdSR(objconn, "UPDATE METLMAI SET MAIDTTEM=DATEDIFF(MS, '" & dataIni & "', GETDATE()) WHERE  MAICDREL=" & rel.incod & " AND MAIDTTEM=0 ")
                End If

            Next dbRow
        Catch
        End Try
        'Comprovo si hi ha cap tràmit que no té PDF generat. En aquest cas, el genero. Només en una execució a la nit.
        If Now.Hour() = 1 And Now.Minute() > 0 And Now.Minute < 11 Then

            GAIA2.bdr(objconn, "select DISTINCT RELINFIL FROM METLREL  WITH(NOLOCK),METLFTR WITH(NOLOCK) WHERE RELINFIL=FTRINNOD AND RELCDHER LIKE '%103017%' AND RELCDSIT<98 AND FTRDSEEX like '' AND (FTRSWVSE=1 OR FTRSWVWE=1)", DS)
            For Each dbRow In DS.Tables(0).Rows
                If Not File.Exists("e:\docs\GAIA\contingutsPDF\1_" & dbRow("RELINFIL") & ".pdf") Then
                    'response.write("falta pdf:" & dbrow("RELINFIL") & "<br />")
                    rel.bdget(objconn, -1, dbRow("RELINFIL"))

                    GAIA2.generaPDF(objconn, rel)
                End If

            Next dbRow
        End If
        DS.Dispose()

        Return (cont)
    End Function
    '****************************************************************************************************************
    '	Funció: mantenimentContinguts
    '	Entrada: 
    '	Procés/sortida: 	
    '		Fa un recorregut de tots els nodes que cal publicar i els publica
    '****************************************************************************************************************	
    Public Shared Sub mantenimentContinguts()
        Dim DS As DataSet
        Dim DS2 As DataSet
        Dim dbRow As DataRow
        Dim dbRow2 As DataRow
        Dim objconn As OleDbConnection
        Dim html, css As String
        Dim fDesti, urlDesti As String
        Dim llistaDocuments As String() = {}
        Dim rel As New clsRelacio
        Dim relOriginal As New clsRelacio
        Dim relFullaWeb As New clsRelacio
        Dim titol As String = ""
        DS = New DataSet()
        objconn = GAIA2.bdIni()
        Dim heretaPropietats As Integer
        Dim llistaDocumentsEliminar As String() = {}
        Dim ftpsession As Session
        Dim width As Integer = 760
        ftpsession = New Session()
        Dim datacad, datapub As DateTime
        Dim relbuida As New clsRelacio
        Dim tipus As String = ""

        Dim estilbody As String = ""
        Dim strCSSPantalla As String = ""

        Dim strCSSImpressora As String = ""
        Dim tagsMeta As String = ""
        Dim esForm As String = "N"
        Dim esEML As String = "N"
        Dim esSSL As String = "N"
        Dim htmlPeu As String = ""
        datacad = CDate("01/01/1900")
        datapub = CDate("01/01/1900")
        Dim usuari As Integer = 115969 'tots
        Dim strTmp As String = ""

        Dim dataIni As Date





        'Caduco tots els continguts d'agenda i notícies que hagin de caducar i que no es fa d'una forma natural per que no estàn publicats
        'AND RELCDHER LIKE '_57135_57137%'

        GAIA2.bdSR(objconn, "UPDATE METLREL SET RELCDSIT=98 WHERE RELINCOD IN (	SELECT RELINCOD FROM METLAGD WITH(NOLOCK),METLREL WITH(NOLOCK) WHERE AGDINNOD=RELINFIL  AND RELCDSIT<98 AND ((AGDDTCAD<= getdate() AND AGDDTCAD<>'01/01/1900') OR (AGDDTCAD='01/01/1900' AND (AGDDTFIN<='" + CDate(Today.AddDays(1)).ToString() + "' OR (AGDDTFIN IS NULL AND AGDDTINI<='" + CDate(Today.AddDays(1)).ToString() + "'))))) AND RELCDHER NOT  LIKE '_5286%'")
        GAIA2.bdSR(objconn, "UPDATE METLREL SET RELCDSIT=98 WHERE RELINCOD IN (SELECT RELINCOD FROM METLREL WITH(NOLOCK),METLNOT WITH(NOLOCK) WHERE NOTINNOD=RELINFIL  AND RELCDSIT<98 AND ( NOTDTCAD<=getdate()  AND NOTDTCAD<>'01/01/1900') AND RELCDHER NOT  LIKE '_5286%')")


        '4/6/2015 --> hay que borrar esto cuando esté corregido lo de informar de la libreria de codigo que consume una llamada a wsobrirfulla
        GAIA2.bdSR(objconn, "DELETE FROM METLCEL WHERE CELCDPLT=199611")


        'gaia.bdsr(objconn, "DELETE FROM METLCEL WHERE CELCDPLT=" & 

        'Esborro tots els manteniments de nodes i relacions que ja no existeixen per que s'han esborrat manualment
        'GAIA.bdSR(objconn, "DELETE FROM METLMAN WHERE MANDTTEM=0 AND MANCDNOD<>0 AND MANCDNOD NOT IN (SELECT NODINNOD FROM METLNOD)")
        'GAIA.bdSR(objconn, "DELETE FROM  METLMAN  WHERE MANDTTEM=0 AND MANCDREL<>0 AND  MANCDREL NOT IN (SELECT RELINCOD FROM METLREL)")



        'Faig el tractament de la CUA de manteniment previ: METLMPA
        Dim fin As Boolean = False


        GAIA2.bdSR(Nothing, "delete from METLMPA WHERE  MPADTTEM=0 AND MPACDREL NOT IN (SELECT RELINCOD FROM METLREL) AND MPACDREL<>0")


        While Not fin
            GAIA2.bdr(objconn, "select top 1 *  FROM METLMPA WHERE MPADTTEM=0 order by MPAINMPA", DS)
            If DS.Tables(0).Rows.Count = 0 Then
                fin = True
            Else
                dbRow = DS.Tables(0).Rows(0)
                dataIni = Now
                rel.bdget(objconn, dbRow("MPACDREL"), 0, True)
                If dbRow("MPACDREO") = 0 Then
                    relOriginal.bdget(objconn, dbRow("MPACDREL"), 0, True)
                Else
                    relOriginal.bdget(objconn, dbRow("MPACDREO"), 0, True)
                End If

                GAIA2.afegeixAccioManteniment(objconn, rel, dbRow("MPACDNOD"), dbRow("MPACDIDI"), dbRow("MPADTPUB"), dbRow("MPADTCAD"), relOriginal, dbRow("MPAWNASC"), dbRow("MPAWNEST"), False)
                GAIA2.bdSR(Nothing, "UPDATE METLMPA SET MPADTTEM=(select DATEDIFF(MS, '" & dataIni & "', GETDATE() )) WHERE MPADTTEM=0 AND MPACDREL=" & rel.incod & " AND MPACDNOD=" & dbRow("MPACDNOD") & " AND MPACDIDI=" & dbRow("MPACDIDI") & " AND MPADTPUB='" & dbRow("MPADTPUB") & "' AND MPADTCAD='" & dbRow("MPADTCAD") & "' AND MPAWNASC=" & dbRow("MPAWNASC") & " AND MPAWNEST=" & dbRow("MPAWNEST"))

            End If
        End While


        fin = False
        While Not fin
            'primer tracto les que tenen MANCDREL>0, normalment apuntaran a fulles/nodes/arbres web. Quan no quedin, tracto els nodes (MANCDNOD>0). Per cadascun hauré de revisar quines pàgines poden estar afectades i/o generar els pdfs associats.
            GAIA2.bdr(objconn, "select top 1 * FROM ((SELECT  (1) as ordre, METLMAN.* FROM METLMAN  WITH(NOLOCK) WHERE MANDTTEM=0 AND METLMAN.MANDTDAT <= getdate()  AND MANCDREL=0 ) UNION (SELECT   (2) as ordre, METLMAN.* FROM METLMAN  WITH(NOLOCK) WHERE MANDTTEM=0 AND METLMAN.MANDTDAT <= getdate()  AND MANCDNOD=0)    ) as METLMAN ORDER BY ordre, MANDTTIM", DS)
            If DS.Tables(0).Rows.Count = 0 Then
                fin = True
            Else

                dbRow = DS.Tables(0).Rows(0)


                fDesti = ""
                urlDesti = ""
                html = ""
                css = ""
                estilbody = ""
                strCSSPantalla = ""
                strCSSImpressora = ""
                htmlPeu = ""
                tagsMeta = ""
                dataIni = Now 'moment en que comença a generarse la publicació
                If dbRow("MANCDREL") <> 0 Then
                    rel.bdget(objconn, dbRow("MANCDREL"))

                    If rel.incod <> 0 Then
                        If rel.tipdsdes <> "fulla web" And rel.tipdsdes <> "node web" Then

                            GAIA2.afegeixAccioMantenimentPDF(objconn, rel)
                            ' If Not GAIA.generaPDF(objconn, rel) Then
                            'Si no és una fulla web, és un contingut
                            'Esborro el manteniment
                            GAIA2.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni, dbRow("MANCDREL"), dbRow("MANCDNOD"), dbRow("MANCDIDI"))
                            ' End If
                            'Elimino totes les cel·les on apareix aquest contingut
                            GAIA2.esborrarCelles(Nothing, "CELCDNOD=" & rel.infil)
                            'GAIA.bdSR(objconn, "DELETE FROM METLCEL WHERE CELCDNOD=" & rel.infil)


                        Else
                            Try
                                If rel.tipdsdes = "node web" Then



                                    'Faig Tractament de continguts GAIA


                                    'Si rel.tipdsdes="node web" he de fer un bucle per a tots els nodes web o fulles web que penjen						
                                    's'hauran d'esborrar tot lo que ja s'hagi tractat.
                                    DS2 = New DataSet()
                                    GAIA2.bdr(objconn, "	SELECT WEBINIDI, RELINCOD, '' AS WEBDSHTM FROM METLWEB WITH(NOLOCK) INNER JOIN METLREL WITH(NOLOCK)  ON METLWEB.WEBINNOD = METLREL.RELINFIL  INNER JOIN METLNOD ON METLREL.RELINFIL = METLNOD.NODINNOD INNER JOIN METLTIP ON METLNOD.NODCDTIP = METLTIP.TIPINTIP AND METLTIP.TIPDSDES LIKE 'fulla web' WHERE (METLREL.RELCDHER LIKE '" + rel.cdher + "_" + rel.infil.ToString() + "%') AND RELCDSIT<98 UNION SELECT WEBINIDI, RELINCOD,WEBDSHTM FROM METLWEB2 WITH(NOLOCK) INNER JOIN METLREL WITH(NOLOCK)  ON METLWEB2.WEBINNOD = METLREL.RELINFIL  INNER JOIN METLNOD ON METLREL.RELINFIL = METLNOD.NODINNOD INNER JOIN METLTIP ON METLNOD.NODCDTIP = METLTIP.TIPINTIP AND METLTIP.TIPDSDES LIKE 'fulla web' WHERE (METLREL.RELCDHER LIKE '" + rel.cdher + "_" + rel.infil.ToString() + "%') AND RELCDSIT<98", DS2)
                                    For Each dbRow2 In DS2.Tables(0).Rows
                                        dataIni = Now 'moment en que comença a generarse la publicació
                                        Try
                                            html = ""
                                            css = ""
                                            fDesti = ""
                                            urlDesti = ""
                                            titol = ""

                                            estilbody = ""
                                            strCSSPantalla = ""
                                            strCSSImpressora = ""
                                            esSSL = "N"
                                            esEML = "N"
                                            esForm = "N"
                                            tagsMeta = ""
                                            htmlPeu = ""
                                            relFullaWeb.bdget(objconn, dbRow2("RELINCOD"))


                                            'Cercar en GAIA i en GAIA2




                                            heretaPropietats = GAIA2.heretarPropietatsWeb(objconn, dbRow2("RELINCOD"), width)
                                            strTmp &= GAIA2.llistatErrors
                                            GAIA2.llistatErrors = ""


                                            GAIA2.obreFullaWeb(objconn, relFullaWeb, html, css, heretaPropietats, width, 500, "", fDesti, urlDesti, 1, llistaDocuments, 1, dbRow2("WEBINIDI"), relFullaWeb, Now, usuari, titol, 1, estilbody, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, True)
                                            If GAIA2.llistatErrors.Length > 0 Then
                                                'he trobat un error i no publico. 												
                                            Else
                                                If GAIA2.contingutEsPublicable(objconn, relFullaWeb, Now, dbRow2("WEBINIDI"), 99) = ctPUBLICAR Then
                                                    'Poso al paràmetre "servidor"= "Manteniment", només serveix per saber que s'ha de tancar al final de la publicació de tots els documents.Si el destí és un fitxer vb no poso cap format
                                                    If InStr(fDesti, ".vb") Or InStr(fDesti, ".js") > 0 Then
                                                        GAIA2.publica(objconn, html, fDesti, llistaDocuments, llistaDocumentsEliminar, 1, relFullaWeb, dbRow2("WEBINIDI"), "Manteniment", ftpsession)
                                                    Else

                                                        If GAIA2.publica(objconn, GAIA2.preparaPagina(objconn, html, css, 1, titol, heretaPropietats, estilbody, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, dbRow2("WEBINIDI")), fDesti, llistaDocuments, llistaDocumentsEliminar, 1, relFullaWeb, dbRow2("WEBINIDI"), "Manteniment", ftpsession) Then
                                                            GAIA2.log(objconn, relFullaWeb, 0, "", TAPUBLICAR)
                                                        End If

                                                    End If

                                                    'GAIA.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni)
                                                    GAIA2.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni, dbRow("MANCDREL"), dbRow("MANCDNOD"), dbRow("MANCDIDI"))
                                                End If
                                            End If
                                        Catch
                                            GAIA2.debug(objconn, "MANTENIMENT. PROBLEMES AL PUBLICAR: " + relFullaWeb.incod.ToString() + "(" + relFullaWeb.noddstxt.ToString() + ") a l'adreça " + fDesti.ToString() + "-" + Err.Description)

                                            strTmp &= "MANTENIMENT. PROBLEMES AL PUBLICAR: " + relFullaWeb.incod.ToString() + "(" + relFullaWeb.noddstxt.ToString() + ") a l'adreça " + fDesti.ToString() + "-" + Err.Description
                                            '  GAIA.esborraAccioManteniment(objconn, dbRow("MANINMAN"), rel.incod, dbRow("MANCDIDI"), dataIni, 0)
                                            ' GAIA.esborraAccioManteniment(objconn, dbRow("MANINMAN"), relFullaWeb.incod, dbRow2("WEBINIDI"), dataIni, 0)
                                            ' GAIA.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni)
                                            GAIA2.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni, dbRow("MANCDREL"), dbRow("MANCDNOD"), dbRow("MANCDIDI"))
                                        End Try

                                    Next dbRow2





                                    'GAIA.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni)
                                    GAIA2.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni, dbRow("MANCDREL"), dbRow("MANCDNOD"), dbRow("MANCDIDI"))
                                    DS2.Dispose()
                                Else    'és una fulla web						
                                    heretaPropietats = GAIA2.heretarPropietatsWeb(objconn, dbRow("MANCDREL"), width)
                                    esSSL = "N"
                                    esEML = "N"
                                    esForm = "N"
                                    strTmp &= GAIA2.llistatErrors
                                    GAIA2.llistatErrors = ""
                                    tagsMeta = ""
                                    GAIA2.obreFullaWeb(objconn, rel, html, css, heretaPropietats, width, 500, "", fDesti, urlDesti, 1, llistaDocuments, 1, dbRow("MANCDIDI"), rel, Now, usuari, titol, 1, estilbody, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, True)
                                    If GAIA2.llistatErrors.Length > 0 Then
                                        'he trobat un error i no publico. 												
                                    Else
                                        If GAIA2.contingutEsPublicable(objconn, rel, Now, dbRow("MANCDIDI"), 99) = ctPUBLICAR Then
                                            'Poso al paràmetre "servidor"= "Manteniment", només serveix per saber que s'ha de tancar al final de la publicació de tots els documents.
                                            'Si el destí és un fitxer vb no poso cap format
                                            If InStr(fDesti, ".vb") Or InStr(fDesti, ".js") > 0 Then
                                                GAIA2.publica(objconn, html, fDesti, llistaDocuments, llistaDocumentsEliminar, 1, rel, dbRow("MANCDIDI"), "Manteniment", ftpsession)
                                            Else
                                                GAIA2.publica(objconn, GAIA2.preparaPagina(objconn, html, css, 1, titol, heretaPropietats, estilbody, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, dbRow("MANCDIDI")), fDesti, llistaDocuments, llistaDocumentsEliminar, 1, rel, dbRow("MANCDIDI"), "Manteniment", ftpsession)
                                            End If
                                            GAIA2.log(objconn, rel, 0, "", TAPUBLICAR)
                                        End If
                                    End If
                                    '  GAIA.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni)
                                    GAIA2.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni, dbRow("MANCDREL"), dbRow("MANCDNOD"), dbRow("MANCDIDI"))
                                End If
                            Catch
                                strTmp &= "MANTENIMENT. PROBLEMES AL PUBLICAR: " + relFullaWeb.incod.ToString() + "(" + relFullaWeb.noddstxt.ToString() + ") a l'adreça " + fDesti.ToString() + "-" + Err.Description
                                GAIA2.debug(objconn, "MANTENIMENT. PROBLEMES AL PUBLICAR: " + rel.incod.ToString() + "(" + rel.noddstxt.ToString() + ") a l'adreça " + fDesti.ToString() + "-" + Err.Description)
                                'GAIA.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni)
                                GAIA2.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni, dbRow("MANCDREL"), dbRow("MANCDNOD"), dbRow("MANCDIDI"))
                            End Try
                        End If
                    Else
                        'la relació ja no existeix i es pot esborrar el manteniment					
                        GAIA2.bdSR(objconn, "DELETE FROM METLMAN WHERE MANCDREL=" + dbRow("MANCDREL").ToString())
                    End If
                Else 'Tinc un manteniment d'un contingut per MANCDNOD. Aquest manteniment pot ser una caducitat o un canvi del contingut que modifica METLCEL 		
                    'Elimino totes les cel·les on apareix aquest contingut

                    GAIA2.esborrarCelles(Nothing, "CELCDNOD=" & dbRow("MANCDNOD"))
                    'GAIA.bdSR(objconn, "DELETE FROM METLCEL WHERE CELCDNOD=" & dbRow("MANCDNOD"))
                    'provoco un manteniment de totes les relacions que utilitzen aquest contingut i esborro cel·les de totes les relacions afectades pel canvi




                    DS2 = New DataSet()
                    GAIA2.bdr(objconn, "SELECT RELINCOD FROM METLREL WITH(NOLOCK)  WHERE RELCDSIT<98 AND RELINFIL=" & dbRow("MANCDNOD"), DS2)
                    Dim llistaCanvis As String = ""
                    Dim codirelacio As Integer = 0
                    Dim llistaEsborrar As String = ""
                    For Each dbRow2 In DS2.Tables(0).Rows
                        rel.bdget(objconn, dbRow2("RELINCOD"))
                        llistaCanvis = GAIA2.obtenirPaginesAfectadesPerCanvi(objconn, rel, 0, 1, Now, Now, rel, 1, "", 0)
                        For Each item As String In llistaCanvis.Split(",")
                            If Not String.IsNullOrEmpty(item) Then
                                codirelacio = item.Split("|")(0)
                                If codirelacio <> 0 And item.Split("|")(3) = 1 Then  'només per la versió en català, és suficient per que esborrem totes 
                                    If llistaEsborrar.Length > 0 Then
                                        llistaEsborrar &= ","
                                    End If
                                    llistaEsborrar &= codirelacio


                                End If
                            End If
                        Next
                    Next dbRow2

                    If llistaEsborrar.Length > 0 Then
                        GAIA2.esborrarCelles(Nothing, " CELINREL IN (" & llistaEsborrar & ")")
                        ' GAIA.bdSR(objconn, "delete from METLCEL WHERE CELINREL IN (" & llistaEsborrar & ")")
                    End If



                    rel.bdget(objconn, dbRow("MANCDREL"), dbRow("MANCDNOD"))
                    If rel.incod <> 0 Then 'si es=0 és que està esborrat
                        GAIA2.datesPublicacioPerNode(objconn, dbRow("MANCDNOD"), dbRow("MANCDIDI"), datapub, datacad)
                        If rel.tipdsdes.Substring(0, 5) = "fulla" And rel.tipdsdes.Trim() <> "fulla web" Then
                            'Només esborro si és de tipus "fulla" però no de tipus "fulla web"								

                            If rel.tipdsdes = "fulla tramit" Then
                                GAIA2.afegeixAccioMantenimentPDF(objconn, rel)
                            End If


                            'Faig la comprovació de que no han canviat la data de forma que se segur que es pot esborrar
                            If CDate(datacad) <> CDate("01/01/1900") And CDate(datacad) <= CDate(Now) Then
                                Try
                                    GAIA2.esborrarNode(objconn, dbRow("MANCDNOD"), relbuida, 1, 115969, 1, GAIA2.ctESBORRATCADUCAT)

                                    'He tractat el manteniment i ja el puc esborrar				
                                    'GAIA.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni)
                                    GAIA2.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni, dbRow("MANCDREL"), dbRow("MANCDNOD"), dbRow("MANCDIDI"))
                                Catch
                                    GAIA2.debug(objconn, "Manteniment automàtic. Problemes esborrant " + dbRow("MANCDNOD").ToString())
                                End Try
                            Else ' he de mantenir el contingut de tipus fulla noticia, agenda, tràmit, etc.. De moment només tracto el contingut de tipus tràmit, imprimint el contingut en un pdf.

                                rel.bdget(objconn, 0, dbRow("MANCDNOD"))
                                If rel.tipdsdes <> "fulla web" And rel.tipdsdes <> "node web" And rel.tipdsdes <> "fulla tramit" Then
                                    'Si és un tràmit no genero el pdf en temps de manteniment, ho faré en temps d'aprovació del canvi de contingut.
                                    GAIA2.afegeixAccioMantenimentPDF(objconn, rel)
                                End If
                                GAIA2.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni, dbRow("MANCDREL"), dbRow("MANCDNOD"), dbRow("MANCDIDI"))

                            End If


                        Else
                            'Esborro el manteniment perque per ara no es pot fer manteniment d'un node de tipus "node xxxx"
                            'IF dataPub<>cdate("01/01/1900") THEN
                            'GAIA.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni)
                            GAIA2.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni, dbRow("MANCDREL"), dbRow("MANCDNOD"), dbRow("MANCDIDI"))

                            'END IF
                        End If
                    Else 'és un manteniment d'un node que està esborrat, esborro l'acció manteniment i acabo
                        GAIA2.esborraAccioManteniment(objconn, dbRow("MANINMAN"), dataIni, dbRow("MANCDREL"), dbRow("MANCDNOD"), dbRow("MANCDIDI"))

                    End If
                End If


            End If
        End While
        llistatErrors = strTmp
        DS.Dispose()

        If Not ftpsession Is Nothing Then
            ftpsession.Close()
        End If



        GAIA2.bdFi(objconn)
    End Sub 'mantenimentContinguts




    '****************************************************************************************************************
    '	Funció: afegeixAccioManteniment
    '   
    '   es crida a obtenirPaginesAfectadesPerCanvi i per la llista de pàgines obtingudes es crea el manteniment en METLMAN
    '   Es retorna un text amb la descripció del que es canviarà i la data prevista de tots els fets.   
    '****************************************************************************************************************	
    Public Shared Function afegeixAccioManteniment(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiNode As Integer, ByVal codiIdioma As Integer, ByRef dataPubOriginal As String, ByRef dataCadOriginal As String, ByVal relOriginal As clsRelacio) As String
        Return (afegeixAccioManteniment(objConn, rel, codiNode, codiIdioma, dataPubOriginal, dataCadOriginal, relOriginal, 1, 0))
    End Function
    Public Shared Function afegeixAccioManteniment(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiNode As Integer, ByVal codiIdioma As Integer, ByRef dataPubOriginal As String, ByRef dataCadOriginal As String, ByVal relOriginal As clsRelacio, ByVal mantenirAscendents As Integer, Optional ByVal estructura As Integer = 0, Optional ByVal endiferit As Boolean = False) As String
        Dim strResultat As String = ""
        strResultat = ""
        If endiferit Then
            GAIA2.bdSR(objConn, "INSERT INTO METLMPA VALUES (" & rel.incod & "," & codiIdioma & ",'" & CDate(dataPubOriginal) & "','" & CDate(dataCadOriginal) & "'," & codiNode & "," & relOriginal.incod & "," & mantenirAscendents & "," & estructura & ",0)")
        Else


            Dim strResultatAra As String = ""
            Dim strResultatDespres As String = ""
            Dim strPagines As String = ""
            Dim item As String
            obtenirPaginesAfectadesPerCanvi(objConn, rel, codiNode, codiIdioma, dataPubOriginal, dataCadOriginal, relOriginal, mantenirAscendents, strPagines, estructura)

            Dim txt As String = ""
            Dim ds As DataSet
            ds = New DataSet


            'Si és un manteniment per codi node, faig el manteniment del codi node que toca
            If rel.incod = 0 Then
                GAIA2.creaAccioManteniment(objConn, 0, codiIdioma, dataPubOriginal, codiNode)
                GAIA2.creaAccioManteniment(objConn, 0, codiIdioma, dataCadOriginal, codiNode)
            End If



            'Faig el manteniment de totes les pàgines afectades i Calculo el text on es descriuran les pàgines afectades
            Dim relTemp As New clsRelacio
            Dim pos As Integer = 0
            Dim arrValors As String()
            Dim ant As Integer = -1
            Dim antfec As Date = Nothing
            For Each item In strPagines.Split(",")
                If item.Length > 0 Then
                    arrValors = item.Split("|")
                    GAIA2.creaAccioManteniment(objConn, arrValors(0), 99, arrValors(2), arrValors(1))


                    If ant <> arrValors(0) + arrValors(1) Then
                        ant = arrValors(0) + arrValors(1)
                        antfec = arrValors(2)
                        relTemp.bdget(objConn, arrValors(0), arrValors(1))

                        If relTemp.tipintip = 8 Or relTemp.tipintip = 9 Or relTemp.tipintip = 10 Then
                            txt = "<li><img class=""valignMiddle"" src=""/img/common/iconografia/" & IIf(relTemp.tipintip = 8, "arbre.png", IIf(relTemp.tipintip = 9, "node_web.png", "ico_web.png")) & """>&nbsp;"
                            'he d'agafar el node arrel per saber l'arbre
                            Dim postmp As Integer = 0
                            If relTemp.cdher.Length > 0 Then
                                If relTemp.cdrsu <> 0 Then
                                    GAIA2.bdr(objConn, "select NODDSTXT FROM METLNOD WITH(NOLOCK), METLREL WITH(NOLOCK) WHERE RELINPAR=NODINNOD AND RELINCOD=" & relTemp.cdrsu, ds)
                                End If

                            Else
                                GAIA2.bdr(objConn, "SELECT NODDSTXT FROM METLNOD  WITH(NOLOCK) WHERE NODINNOD=" & relTemp.infil, ds)
                            End If

                            If ds.Tables(0).Rows.Count > 0 Then
                                txt &= ds.Tables(0).Rows(0)("NODDSTXT") & "&nbsp;<img src=""http://www.l-h.cat/img/fletxa_gris.gif"" class=""border0 valignMiddle""/>&nbsp;"
                            End If
                            txt &= relTemp.noddstxt
                            If arrValors(2) < Now Then
                                If InStr(strResultatAra, txt) = 0 Then
                                    strResultatAra &= txt & "</li>"
                                End If

                            Else
                                If arrValors(2) < DateAdd(DateInterval.Year, 5, Now) Then

                                    If InStr(strResultatAra, txt) = 0 Then
                                        strResultatDespres &= txt & "," & arrValors(2) & "</li>"
                                    End If
                                End If
                            End If
                        End If

                    Else
                        If antfec <> arrValors(2) Then
                            antfec = arrValors(2)
                            relTemp.bdget(objConn, arrValors(0), arrValors(1))

                            If relTemp.tipintip = 8 Or relTemp.tipintip = 9 Or relTemp.tipintip = 10 Then
                                txt = "<li><img class=""valignMiddle"" src=""/img/common/iconografia/" & IIf(relTemp.tipintip = 8, "arbre.png", IIf(relTemp.tipintip = 9, "node-web.png", "fullaweb.png")) & """>&nbsp;"
                                If relTemp.cdrsu <> 0 Then
                                    GAIA2.bdr(objConn, "select NODDSTXT FROM METLNOD WITH(NOLOCK), METLREL WITH(NOLOCK) WHERE RELINPAR=NODINNOD AND RELINCOD=" & relTemp.cdrsu, ds)
                                End If

                                If ds.Tables(0).Rows.Count > 0 Then
                                    txt &= ds.Tables(0).Rows(0)("NODDSTXT") & "&nbsp;<img src=""http://www.l-h.cat/img/fletxa_gris.gif"" class=""border0 valignMiddle""/>&nbsp;"
                                End If
                                txt &= relTemp.noddstxt
                                If arrValors(2) < Now Then
                                    If InStr(strResultatAra, txt) = 0 Then
                                        strResultatAra &= txt & "</li>"
                                    End If
                                Else
                                    If arrValors(2) < DateAdd(DateInterval.Year, 5, Now) Then
                                        If InStr(strResultatDespres, txt) = 0 Then
                                            strResultatDespres &= txt & "," & arrValors(2) & "</li>"
                                        End If
                                    End If
                                End If
                            End If
                        End If

                    End If

                End If
            Next item


            strResultat = IIf(strResultatAra.Length > 0, "<div class=""arial bold marginEsquerra10"">P&agrave;gines afectades per aquest canvi i que es publicaran automàticament en uns minuts:</div><div class=""llistaSensePunt  marginEsquerra10""><ul>" & strResultatAra & "</ul></div>", "")
            strResultat &= IIf(strResultatDespres.Length > 0, "<div class=""arial bold  marginEsquerra10"">P&agrave;gines que es publicaran properament de forma autom&agrave;tica per eliminar el contingut que haur&agrave; caducat:</div><div class=""llistaSensePunt  marginEsquerra10""><ul>" & strResultatDespres & "</ul></div>", "")






            ds.Dispose()
        End If
        Return strResultat
    End Function


    Public Shared Function tempsPendentPublicacio(ByVal objconn As OleDbConnection) As Integer
        'select  AVG(anteriors.MANDTTEM) as m FROM METLMAN as anteriors, METLMAN as pendents WHERE anteriors.MANDTDAT<GETDATE() AND anteriors.MANDTTEM>0 AND anteriors.MANCDREL<>0 AND pendents.MANDTDAT<GETDATE() AND pendents.MANDTTEM=0 AND pendents.MANCDREL=anteriors.MANCDREL GROUP BY pendents.MANCDREL

        'he de veure que passa amb les relacions pendents que no trobo
        Return 0

    End Function

    '****************************************************************************************************************
    '   Funció: obtenirPaginesAfectadesPerCanvi
    '
    '   Fa una cerca de totes les pàgines, nodes o arbres web afectats per un canvi en un contingut (rel o codinode)
    '   L'afectació pot ser per contingut arrossegat o per llibreria de codi afectada pels continguts
    '   Es retorna un string amb format  codirelacio1|idioma1|data1|node1,...,codirelacioN|idiomaN|dataN|nodeN
    '****************************************************************************************************************	

    Public Shared Function obtenirPaginesAfectadesPerCanvi(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiNode As Integer, ByVal codiIdioma As Integer, ByRef dataPubOriginal As String, ByRef dataCadOriginal As String, ByVal relOriginal As clsRelacio, ByVal mantenirAscendents As Integer, ByRef strResultat As String, Optional ByVal estructura As Integer = 0, Optional ByRef relTractades As Hashtable = Nothing, Optional ByVal esborrarCEL As Boolean = True) As String


        Dim DS, DS2 As DataSet
        Dim dbRow, dbRow2 As DataRow

        Dim codiRelacio, codiRelacioPare, codiRelacioAnt As Integer
        Dim relPare As New clsRelacio
        Dim relAnt As New clsRelacio
        Dim relPlantilla As New clsRelacio
        Dim relTmp As New clsRelacio
        Dim relIni As New clsRelacio
        Dim dataPub, dataCad As DateTime

        Dim strsql As String = ""

        dataPub = CDate("01/01/1900")
        dataCad = CDate("01/01/1900")
        DS = New DataSet()
        DS2 = New DataSet()
        codiRelacio = rel.incod
        relPare.copiaObj(rel)
        codiRelacioPare = relPare.incod
        relIni.copiaObj(rel)




        Dim strPlant As String = ""

        Dim nodesAscendents As String = ""
        Dim elements As String = ""
        If codiIdioma = 99 Then
            codiIdioma = 1
        End If

        'Si no indiquen data caducitat, poso la data mínima  i no la tractaré
        If dataCadOriginal = "" Then
            dataCad = CDate("01/01/1900")
            dataCadOriginal = CDate("01/01/1900")
        End If
        If dataPubOriginal = "" Then
            dataPub = CDate("01/01/1900")
            dataPubOriginal = CDate("01/01/1900")
        End If
        Dim herencia As String = ""
        codiRelacioAnt = 0
        Dim i As Integer
        If rel.incod = 0 Then
            If codiNode <> 0 Then 'em demanen fer el manteniment d'un node, i per tant he de cercar on apareix		
                GAIA2.bdr(objConn, "SELECT RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELCDSIT<95 AND NOT RELCDHER LIKE '_5286%' AND NOT RELCDHER LIKE '_184845%' AND NOT RELCDHER LIKE '_145043%' AND RELINFIL=" & codiNode, DS)
                For Each dbRow In DS.Tables(0).Rows
                    relTmp.bdget(objConn, dbRow("RELINCOD"))
                    GAIA2.obtenirPaginesAfectadesPerCanvi(objConn, relTmp, 0, codiIdioma, CDate(dataPubOriginal), CDate(dataCadOriginal), relTmp, 1, strResultat, 0, relTractades)


                Next dbRow
            End If

        Else  'He de fer el manteniment d'una relació concreta
            'marco la relació com a tractada
            If IsNothing(relTractades) Then
                relTractades = New Hashtable
            End If

            If Not relTractades.Contains(CInt(rel.incod)) Then
                relTractades.Add(rel.incod, "1")
                'AFegeixo el manteniment del contingut demanat per a tots els idiomes

                If dataPubOriginal = CDate("01/01/1900") Then
                    GAIA2.datesPublicacio(objConn, relOriginal, i, dataPub, dataCad)
                Else
                    dataPub = CDate(dataPubOriginal)
                    dataCad = CDate(dataCadOriginal)
                End If




                '********************************************************************************************************************
                ' faig el manteniment del contingut afectat i si és una llibreria de codi, de totes les pàgines i nodes web 
                ' que l'utilitzen directament o mitjançant plantilles
                '********************************************************************************************************************

                If rel.tipdsdes = "fulla web" Or rel.tipdsdes = "node web" Then
                    'GAIA.creaAccioManteniment(objConn, rel.incod, i, CDate(dataPub), 0)
                    If InStr(strResultat, rel.incod & "|0|" & CDate(dataPub) & "|" & i) = 0 Then
                        strResultat &= "," & rel.incod & "|0|" & CDate(dataPub) & "|" & i
                    End If
                    If InStr(strResultat, rel.incod & "|0|" & CDate(dataCad) & "|" & i) = 0 Then
                        strResultat &= "," & rel.incod & "|0|" & CDate(dataCad) & "|" & i
                    End If
                Else
                    Select Case rel.tipdsdes
                        Case "fulla codiWeb" 'llibreria de codi

                            Dim llistaPlantilles As String = ""

                            'He d'afegir accions de manteniment a totes les pàgines web o plantilles que utilitzin aquesta llibreria de codi
                            'Cerco primer dins de totes les plantilles
                            strPlant = ""
                            GAIA2.bdr(objConn, "SELECT DISTINCT PLTINNOD FROM METLPLT WITH(NOLOCK)  WHERE (PLTDSLCW LIKE '%" & relOriginal.infil & "%') OR (PLTDSLC2 LIKE '%" & relOriginal.infil & "%')", DS)

                            For Each dbRow In DS.Tables(0).Rows
                                If llistaPlantilles.Length > 0 Then
                                    llistaPlantilles &= ","
                                End If
                                llistaPlantilles &= dbRow("PLTINNOD")
                                strPlant &= " OR WEBDSPLA LIKE '%" & dbRow("PLTINNOD") & "%'"
                            Next dbRow

                            GAIA2.bdr(objConn, "(SELECT DISTINCT METLREL.RELINCOD,METLREL.RELINFIL,METLREL.RELCDHER  FROM  METLREL WITH(NOLOCK)  INNER JOIN METLWEB WITH(NOLOCK)  ON METLREL.RELINFIL = METLWEB.WEBINNOD AND METLWEB.WEBINIDI=1 WHERE     (METLREL.RELCDSIT <95) AND (WEBDSLCW LIKE '%" + relOriginal.infil.ToString() + "%'" + strPlant + ") AND NOT RELCDHER LIKE '_5286%' AND NOT RELCDHER LIKE '_184845%' AND NOT RELCDHER LIKE '_145043%') UNION (SELECT DISTINCT METLREL.RELINCOD,METLREL.RELINFIL,METLREL.RELCDHER  FROM METLREL WITH(NOLOCK)  INNER JOIN METLAWE WITH(NOLOCK)  ON METLREL.RELINFIL = METLAWE.AWEINNOD AND METLAWE.AWEINIDI=1 AND (METLAWE.AWEDSLCW LIKE '%" + relOriginal.infil.ToString() + "%') WHERE     (METLREL.RELCDSIT <95)  AND NOT RELCDHER LIKE '_5286%' AND NOT RELCDHER LIKE '_184845%' AND NOT RELCDHER LIKE '_145043%')	UNION (SELECT DISTINCT METLREL.RELINCOD,METLREL.RELINFIL,METLREL.RELCDHER  FROM METLREL WITH(NOLOCK)  INNER JOIN METLNWE WITH(NOLOCK)  ON METLREL.RELINFIL = METLNWE.NWEINNOD AND METLNWE.NWEINIDI=1  AND (METLNWE.NWEDSLCW LIKE '%" + relOriginal.infil.ToString() + "%') WHERE  (METLREL.RELCDSIT <95) AND NOT RELCDHER LIKE '_5286%' AND NOT RELCDHER LIKE '_184845%' AND NOT RELCDHER LIKE '_145043%')", DS)
                            For Each dbRow In DS.Tables(0).Rows
                                'Per cada element trobat, he de fer el manteniment d'ell i de totes les fulles web inferiors									                          
                                If InStr(strResultat, dbRow("RELINCOD") & "|0|" & CDate(dataPub) & "|" & i) = 0 Then
                                    strResultat &= "," & dbRow("RELINCOD") & "|0|" & CDate(dataPub) & "|" & i
                                End If


                                If dbRow("RELCDHER") <> "" Then
                                    elements = dbRow("RELCDHER") + "_" + dbRow("RELINFIL").ToString()
                                Else
                                    elements = "_" + dbRow("RELINFIL").ToString()
                                End If
                                GAIA2.bdr(objConn, "select RELINCOD FROM METLWEB WITH(NOLOCK) ,METLREL WITH(NOLOCK)  WHERE RELCDHER LIKE  '" & elements & "%' AND RELINFIL=WEBINNOD AND RELCDSIT<95 ", DS2)
                                For Each dbRow2 In DS2.Tables(0).Rows
                                    If InStr(strResultat, dbRow2("RELINCOD") & "|0|" & Now & "|" & i) = 0 Then
                                        strResultat &= "," & dbRow2("RELINCOD") & "|0|" & Now & "|" & i
                                    End If
                                Next dbRow2
                            Next




                            'em quedo amb totes les posicions de cel·la on la plantilla s'executa
                            Dim llistaEstPlantilla As String = ""
                            If llistaPlantilles.Length > 0 Then
                                GAIA2.bdr(objConn, "SELECT DISTINCT CELCDEST FROM METLCEL with(NOLOCK) WHERE CELINLCW=0 AND CELCDPLT IN (" & llistaPlantilles & ")", DS)
                                For Each dbRow In DS.Tables(0).Rows
                                    If llistaEstPlantilla.Length > 0 Then
                                        llistaEstPlantilla &= ","
                                    End If
                                    llistaEstPlantilla &= dbRow("CELCDEST")
                                Next
                            End If

                            '********************************************************************************************************************
                            ' Esborro totes les cel·les de les pàgines web que utilitzen la llibreria de codi. Només la cel·la on s'utilitza
                            '********************************************************************************************************************
                            If esborrarCEL Then
                                Dim item As String
                                Dim relacionsPerEsborrar As String = ""
                                For Each item In strResultat.Split(",")
                                    If relacionsPerEsborrar.Length > 0 Then
                                        relacionsPerEsborrar &= ","
                                    End If
                                    relacionsPerEsborrar &= item.Split("|")(0)
                                Next item

                                'msv 3/3/14: hauria d'esborrar només les cel·les afectades però és complex saber si són de fulla web, de plantilla etc.. de moment esborro tot
                                If relacionsPerEsborrar.Length > 0 Then
                                    'GAIA.bdSR(objConn, "DELETE FROM METLCEL WHERE CELINREL IN (" & relacionsPerEsborrar & ") ")   'AND CELCDEST IN (" & llistaEstPlantilla & ")")

                                    GAIA2.esborrarCelles(Nothing, "CELINREL IN (" & relacionsPerEsborrar & ") ")
                                End If

                                '********************************************************************************************************************
                                ' Esborro totes les cel·les amb llibreria de codi donada.
                                '********************************************************************************************************************
                                GAIA2.esborrarCelles(Nothing, "CELINLCW =" & rel.infil)
                                'GAIA.bdSR(objConn, "DELETE FROM METLCEL WHERE CELINLCW =" & rel.infil)
                            End If



                        Case "fulla tramit"
                            If InStr(strResultat, "0|" & rel.infil & "|" & Now & "|" & i) = 0 Then
                                strResultat &= "0|" & rel.infil & "|" & Now & "|" & i
                            End If
                        Case "fulla directori"
                            If CDate(dataCad) <> CDate("01/01/1900") Then
                                If InStr(strResultat, "0|" & rel.infil & "|" & CDate(dataCad) & "|" & i) = 0 Then
                                    strResultat &= "," & "0|" & rel.infil & "|" & CDate(dataCad) & "|" & i
                                End If

                            End If
                            'Cerco els tràmits que mostren el directori i els marco com a manteniment

                            GAIA2.bdr(objConn, "select DISTINCT FTRDSNOM,FTRINNOD FROM METLASS with(NOLOCK),METLFTR with(NOLOCK) WHERE  ASSCDNRL=" & rel.infil & "  AND ASSCDNOD=FTRINNOD AND FTRINIDI=1", DS)
                            For Each dbRow In DS.Tables(0).Rows
                                If InStr(strResultat, "0|" & dbRow("FTRINNOD") & "|" & Now & "|" & i) = 0 Then
                                    strResultat &= "," & "0|" & dbRow("FTRINNOD") & "|" & Now & "|" & i
                                End If
                            Next


                            'cerco els actes d'agenda que utilitzen el directori
                            GAIA2.bdr(objConn, "select DISTINCT EQPINACT FROM METLEQP WITH(NOLOCK), METLREL WITH(NOLOCK) WHERE EQPINDIR=" & rel.infil & " AND RELINFIL=EQPINACT AND RELCDSIT<98", DS)
                            For Each dbRow In DS.Tables(0).Rows
                                If InStr(strResultat, "0|" & dbRow("EQPINACT") & "|" & Now & "|" & i) = 0 Then
                                    strResultat &= "," & "0|" & dbRow("EQPINACT") & "|" & Now & "|" & i
                                End If
                            Next


                        Case "fulla document"
                            If CDate(dataCad) <> CDate("01/01/1900") Then
                                If InStr(strResultat, "0|" & rel.infil & "|" & CDate(dataCad) & "|" & i) = 0 Then
                                    strResultat &= "," & "0|" & rel.infil & "|" & CDate(dataCad) & "|" & i
                                End If
                            End If
                            'Cerco els tràmits que mostren els documents i els marco com a manteniment
                            GAIA2.bdr(objConn, "select DISTINCT FTRDSNOM,FTRINNOD FROM METLASS with(NOLOCK),METLFTR with(NOLOCK) WHERE  ASSCDNRL=" & rel.infil & "  AND ASSCDNOD=FTRINNOD AND FTRINIDI=1", DS)
                            For Each dbRow In DS.Tables(0).Rows
                                If InStr(strResultat, "0|" & dbRow("FTRINNOD") & "|" & Now & "|" & i) = 0 Then
                                    strResultat &= "," & "0|" & dbRow("FTRINNOD") & "|" & Now & "|" & i
                                End If
                            Next





                            'Case "node web","arbre web"
                            '   GAIA.bdr(objConn, "select * FROM METLREL WITH(NOLOCK) WHERE RELCDHER LIKE '%" & rel.infil & "%' AND RELCDSIT<98", DS)
                            '                         For Each dbRow In DS.Tables(0).Rows
                            '                            If InStr(strResultat, "0|" & dbRow("RELINFIL") & "|" & Now & "|" & i) = 0 Then
                            '                               strResultat &= "," & "0|" & dbRow("RELINFIL") & "|" & Now & "|" & i
                            '                          End If
                            '                     Next

                        Case Else
                            If CDate(dataCad) <> CDate("01/01/1900") Then
                                If InStr(strResultat, "0|" & rel.infil & "|" & CDate(dataCad) & "|" & i) = 0 Then
                                    strResultat &= "," & "0|" & rel.infil & "|" & CDate(dataCad) & "|" & i
                                End If
                            End If
                    End Select


                End If



                If esborrarCEL Then
                    '********************************************************************************************************************
                    'Esborro totes les entrades de la METLCEL que utilitzin aquest contingut
                    '********************************************************************************************************************
                    strsql = " CELCDNOD=" & relOriginal.infil
                    If codiNode <> 0 Then
                        strsql &= " OR CELCDNOD=" & codiNode
                    End If

                    If estructura <= 0 Then
                        'strsql &= " OR CELINREL=" & relOriginal.cdrsu
                        strsql &= " OR CELINREL=" & relOriginal.incod
                    Else
                        'strsql &= " OR (CELINREL=" & relOriginal.cdrsu & " AND CELCDEST=" & estructura & ")"
                        strsql &= " OR (CELINREL=" & relOriginal.incod & " AND CELCDEST=" & estructura & ")"
                    End If


                    GAIA2.esborrarCelles(Nothing, strsql)
                    'GAIA.bdSR(objConn, "delete FROM METLCEL WHERE " & strsql)

                    If estructura <= 0 Then
                        strsql = " CELINREL=" & relOriginal.cdrsu
                    Else
                        strsql = "  (CELINREL=" & relOriginal.cdrsu & " AND CELCDEST=" & estructura & ")"

                    End If
                    GAIA2.esborrarCelles(Nothing, strsql)
                    'GAIA.bdSR(objConn, "delete FROM METLCEL WHERE " & strsql)
                End If

                '********************************************************************************************************************
                ' Faig el manteniment de les llibreries de codi afectades pel manteniment del contingut
                ' (només ho faré si el pare de la relació que tracto és un node codificacio)
                '********************************************************************************************************************

                relPare = GAIA2.obtenirRelacioSuperior(objConn, rel)

                'Esborro totes les execucions de la llibreria afectada per que ja no són vàlides. S'hauran de refer automàticament quan s'hagi de publicar la pàgina al manteniment

                If esborrarCEL Then
                    GAIA2.esborrarCelles(Nothing, ("CELINLCW IN (select ASSCDNOD FROM METLREL WITH(NOLOCK) ,METLNOD WITH(NOLOCK) ,METLASS WITH(NOLOCK)  WHERE  RELINFIL=NODINNOD AND ASSCDTPA=41 AND ASSCDNRL=RELINFIL AND RELCDSIT<95 AND (RELINFIL=" & rel.infil & " OR RELINFIL IN (" & IIf(rel.cdher = "", rel.infil.ToString(), rel.cdher.Replace("_", ",")) & ")))").replace("(,", "("))
                    ' GAIA.bdSR(objConn, ("DELETE FROM METLCEL WHERE CELINLCW IN (select ASSCDNOD FROM METLREL WITH(NOLOCK) ,METLNOD WITH(NOLOCK) ,METLASS WITH(NOLOCK)  WHERE  RELINFIL=NODINNOD AND ASSCDTPA=41 AND ASSCDNRL=RELINFIL AND RELCDSIT<95 AND (RELINFIL=" & rel.infil & " OR RELINFIL IN (" & IIf(rel.cdher = "", rel.infil.ToString(), rel.cdher.Replace("_", ",")) & ")))").replace("(,", "("))
                    GAIA2.esborrarCelles(Nothing, "CELINLCW IN (select ASSCDNOD FROM METLREL WITH(NOLOCK) ,METLNOD WITH(NOLOCK) ,METLASS WITH(NOLOCK)  WHERE  RELINFIL=NODINNOD AND ASSCDTPA=41 AND ASSCDNRL=RELINFIL AND RELCDSIT<95  AND RELINFIL= " & rel.infil & ")")

                End If



                'Busco llibreries de codi afectades pel contingut.
                'El codi 41 de l'associació, relaciona una llibreria de codi amb un contingut(normalment de tipus "node codificacio")         
                GAIA2.bdr(objConn, ("select ASSCDNOD FROM METLREL WITH(NOLOCK) ,METLNOD WITH(NOLOCK) ,METLASS WITH(NOLOCK)  WHERE  RELINFIL=NODINNOD AND ASSCDTPA=41 AND ASSCDNRL=RELINFIL AND RELCDSIT<95 AND (RELINFIL IN (" & IIf(rel.cdher = "", rel.infil.ToString(), rel.cdher.Replace("_", ",")).replace("(,", "(") & ") OR RELINFIL=" & rel.infil & ")").Replace("(,", "("), DS)




                For Each dbRow In DS.Tables(0).Rows
                    strPlant = " OR WEBDSPLA LIKE '%" & dbRow("ASSCDNOD") & "%'"
                    'Cerco totes les pàgines web, nodes web o arbres web que utilitzen la llibreria de codi amb relació afectada o bé que utilizen una plantilla que també té la relació afectada.
                    GAIA2.bdr(objConn, " (SELECT DISTINCT METLREL.RELINCOD,METLREL.RELCDHER  FROM   METLREL  WITH(NOLOCK) INNER JOIN METLWEB  WITH(NOLOCK) ON METLREL.RELINFIL = METLWEB.WEBINNOD  AND ( (METLWEB.WEBDSLCW LIKE '%" + dbRow("ASSCDNOD").ToString() + "%') " + strPlant + " )  WHERE     (METLREL.RELCDSIT < 95) AND NOT RELCDHER LIKE '_5286%' AND NOT RELCDHER LIKE '_184845%' AND NOT RELCDHER LIKE '_145043%') UNION (SELECT DISTINCT METLREL.RELINCOD,METLREL.RELCDHER FROM METLREL WITH(NOLOCK)  INNER JOIN METLAWE  WITH(NOLOCK) ON METLREL.RELINFIL = METLAWE.AWEINNOD AND ( (METLAWE.AWEDSLCW LIKE '%" + dbRow("ASSCDNOD").ToString() + "%')  ) WHERE     (METLREL.RELCDSIT < 95)  AND NOT RELCDHER LIKE '_5286%' AND NOT RELCDHER LIKE '_184845%' AND NOT RELCDHER LIKE '_145043%')	UNION (SELECT DISTINCT METLREL.RELINCOD,METLREL.RELCDHER FROM METLREL  WITH(NOLOCK) INNER JOIN METLNWE WITH(NOLOCK)  ON METLREL.RELINFIL = METLNWE.NWEINNOD AND ( (METLNWE.NWEDSLCW LIKE '%" + dbRow("ASSCDNOD").ToString() + "%')  " + strPlant.Replace("WEB", "NWE") + ") WHERE  (METLREL.RELCDSIT <95)  AND NOT RELCDHER LIKE '_5286%' AND NOT RELCDHER LIKE '_184845%' AND NOT RELCDHER LIKE '_145043%')", DS2)

                    For Each dbRow2 In DS2.Tables(0).Rows
                        If InStr(dbRow2("RELCDHER"), "_5286") <= 0 And dbRow2("RELINCOD") <> relIni.incod And dbRow2("RELINCOD") <> rel.incod Then 'només mantindré la pàgina si no està a dins d'un arbre personal, faig al comprovació que no obtingui el mateix objecte que ha iniciat el manteniment

                            If Not relTractades.Contains(CInt(dbRow2("RELINCOD"))) Then
                                'Bucle per tots els elements que s'han de publicar 								
                                relTmp.bdget(objConn, dbRow2("RELINCOD"))
                                ' el últim paràmetre= 1 perque d'una fulla web poden penjar altres.(siot) --> ho he desactivat, no està clar
                                GAIA2.obtenirPaginesAfectadesPerCanvi(objConn, relTmp, 0, 99, CDate(dataPub), CDate(dataCad), rel, 0, strResultat, 0, relTractades)
                            End If
                        End If
                    Next dbRow2
                Next dbRow





                '   End If
                '********************************************************************************************************************
                ' faig un manteniment de totes les fulles,nodes o arbres web superiors 
                ' NO faig manteniment aquí de nodes diferents d'aquests
                '********************************************************************************************************************
                If mantenirAscendents = 1 Then
                    'Busco la fulla web, nodeweb o arbre web superior
                    While relPare.infil <> relPare.inpar And relPare.incod <> 0
                        Select Case relPare.tipdsdes
                            Case "fulla web"
                                If InStr(strResultat, relPare.incod & "|0|" & CDate(dataPub) & "|" & i) = 0 Then

                                    strResultat &= "," & relPare.incod & "|0|" & CDate(dataPub) & "|" & i
                                End If
                                'tracto si la fulla web té continguts que caduquen
                                If CDate(dataCad) <> CDate("01/01/1900") Then
                                    If InStr(strResultat, relPare.incod & "|0|" & CDate(dataCad) & "|" & i) = 0 Then
                                        strResultat &= "," & relPare.incod & "|0|" & CDate(dataCad) & "|" & i
                                    End If
                                End If
                                Exit While
                            Case "node web" 'amb aquesta acció també publicaré totes les fulles web que pengin del node web		
                                If InStr(strResultat, relPare.incod & "|0|" & CDate(dataPub) & "|" & i) = 0 Then
                                    strResultat &= "," & relPare.incod & "|0|" & CDate(dataPub) & "|" & i
                                End If
                                If CDate(dataCad) <> CDate("01/01/1900") Then
                                    If InStr(strResultat, relPare.incod & "|0|" & CDate(dataCad) & "|" & i) = 0 Then
                                        strResultat &= "," & relPare.incod & "|0|" & CDate(dataCad) & "|" & i
                                    End If
                                End If
                                Exit While
                            Case "arbre web"
                                'no ho tracto per que implica publicar tot l'arbre cada vegada
                        End Select
                        relPare = GAIA2.obtenirRelacioSuperior(objConn, relPare)
                    End While
                End If

            End If

        End If

        DS.Dispose()
        DS2.Dispose()

        Return (strResultat)
    End Function 'obtenirPaginesAfectadesPerCanvi	


    '<summary>
    ' Métode per esborrar registres de METLCEL en grups de 500
    '</summary>
    '<param name="idioma">Codi d'idioma del text de sortida</param>
    '<param name="dataInici">Data d'inici</param>
    '<param name="dataFi">Data de final</param>
    '<returns>El text formatejat segons idioma del rang de dates</returns>
    Public Shared Function esborrarCelles(ByVal objconn As OleDbConnection, ByVal strWhere As String) As Integer

        esborrarCelles = 0
        'gaia.bdsr(nothing, "DELETE FROM METLCEL WHERE " & strWhere)		
        Dim ds As New DataSet
        Dim cont As Integer
        GAIA2.bdr(Nothing, "SELECT COUNT(*) as numCelles FROM METLCEL WITH(NOLOCK) WHERE " & strWhere, ds)
        cont = ds.Tables(0).Rows(0)("numCelles")
        esborrarCelles = cont
        While cont > 0
            GAIA2.bdSR(Nothing, "delete TOP (500) FROM METLCEL WHERE " & strWhere)
            cont = cont - 500
        End While

        Return esborrarCelles
    End Function

    Public Shared Sub creaAccioManteniment(ByVal objconn As OleDbConnection, ByVal codiRelacio As Integer, ByVal codiIdioma As Integer, ByVal data As String, ByVal codiNode As Integer)


        '	If codiIdioma=99 THEN
        '		For cont=1 to 4	
        '			creaAccioManteniment(objconn, codiRelacio, cont, data, codiNode)
        '		Next cont
        '	ELSE

        Dim rel As New clsRelacio()
        rel.bdget(objconn, codiRelacio)
        If rel.tipintip <> 13 Then  ' No creo accions manteniment per fulles organigrama

            If codiIdioma = 0 Then codiIdioma = 99
            Dim valor As Integer = 0
            If data < Now Then
                Dim ds As DataSet
                ds = New DataSet
                If codiRelacio <> 0 Then
                    GAIA2.bdr(objconn, "SELECT COUNT(*) as valor  FROM METLMAN with(NOLOCK) WHERE MANDTTEM=0 AND MANCDREL=" & codiRelacio & " AND MANDTDAT<getdate() AND MANCDIDI=" & IIf(codiIdioma = 99, 1, codiIdioma), ds)
                Else
                    GAIA2.bdr(objconn, "SELECT COUNT(*) as valor  FROM METLMAN with(NOLOCK)  WHERE MANDTTEM=0 AND  MANCDNOD=" & codiNode & " AND MANDTDAT<getdate() AND MANCDIDI=" & IIf(codiIdioma = 99, 1, codiIdioma), ds)
                End If

                valor = ds.Tables(0).Rows(0)("valor")
                ds.Dispose()
            End If
            If valor = 0 Then
                If CDate(data) <> CDate("01/01/2050") Then
                    If codiIdioma = 99 Then
                        If GAIA2.existeixContingut(objconn, codiNode, codiRelacio, 1) = 1 Then
                            GAIA2.bdSR(objconn, "INSERT INTO METLMAN (MANCDREL, MANCDIDI, MANDTDAT,MANCDNOD, MANDTTIM, MANDTTEM) VALUES (" + codiRelacio.ToString() + ",1,'" + data + "'," + codiNode.ToString() + ", getdate(),0)")
                        End If
                        If GAIA2.existeixContingut(objconn, codiNode, codiRelacio, 2) = 1 Then
                            GAIA2.bdSR(objconn, "INSERT INTO METLMAN (MANCDREL, MANCDIDI, MANDTDAT,MANCDNOD, MANDTTIM, MANDTTEM) VALUES (" + codiRelacio.ToString() + ",2,'" + data + "'," + codiNode.ToString() + ", getdate(),0)")
                        End If
                        If GAIA2.existeixContingut(objconn, codiNode, codiRelacio, 3) = 1 Then
                            GAIA2.bdSR(objconn, "INSERT INTO METLMAN (MANCDREL, MANCDIDI, MANDTDAT,MANCDNOD, MANDTTIM, MANDTTEM) VALUES (" + codiRelacio.ToString() + ",3,'" + data + "'," + codiNode.ToString() + ", getdate(),0)")
                        End If
                        If GAIA2.existeixContingut(objconn, codiNode, codiRelacio, 4) = 1 Then
                            GAIA2.bdSR(objconn, "INSERT INTO METLMAN (MANCDREL, MANCDIDI, MANDTDAT,MANCDNOD, MANDTTIM, MANDTTEM) VALUES (" + codiRelacio.ToString() + ",4,'" + data + "'," + codiNode.ToString() + ", getdate(),0)")
                        End If
                    Else
                        If GAIA2.existeixContingut(objconn, codiNode, codiRelacio, codiIdioma) = 1 Then
                            GAIA2.bdSR(objconn, "INSERT INTO METLMAN (MANCDREL, MANCDIDI, MANDTDAT,MANCDNOD, MANDTTIM, MANDTTEM) VALUES (" + codiRelacio.ToString() + "," & codiIdioma & ",'" + data + "'," + codiNode.ToString() + ", getdate(),0)")
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    '****************************************************************************************************************
    '	Funció: esborraAccioManteniment
    '	Entrada: 
    '						codiNode: codi del node
    '						codiIdioma: codi de l'idioma
    '						data: data a partir de la qual faré el DELETE
    '	Procés/sortida: 		
    '		Esborra a la taula METLMAN les dades de l'acció.
    '****************************************************************************************************************	


    Public Shared Sub esborraAccioManteniment(ByVal objConn As OleDbConnection, ByVal codiManteniment As Integer, ByVal dataIniciManteniment As String, ByVal codiRelacio As Integer, ByVal codiNode As Integer, ByVal codiIdioma As Integer)
        If codiRelacio = 0 Then
            GAIA2.bdSR(objConn, "UPDATE METLMAN SET MANDTTEM=(select DATEDIFF(MS, '" & dataIniciManteniment & "', GETDATE() )) WHERE  MANCDNOD=" & codiNode & " AND MANDTTEM=0 AND MANDTDAT<=getdate() AND MANCDIDI=" & codiIdioma)

        Else
            GAIA2.bdSR(objConn, "UPDATE METLMAN SET MANDTTEM=(select DATEDIFF(MS, '" & dataIniciManteniment & "', GETDATE() )) WHERE  MANCDREL=" & codiRelacio & " AND MANDTTEM=0 AND MANDTDAT<=getdate() AND MANCDIDI=" & codiIdioma)
        End If

        'GAIA.bdSR(objConn, "DELETE FROM METLMAN WHERE MANCDREL=" & codiRelacio & " AND MANCDNOD=" & codiNode & " AND MANCDIDI=" & codiIdioma & " AND MANDTDAT<='" & data & "'")
    End Sub 'esborraAccioManteniment	

    Public Shared Sub esborraAccioManteniment(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer, ByVal codiIdioma As Integer, ByVal data As String, ByVal codiNode As Integer)

        GAIA2.bdSR(objConn, "DELETE FROM METLMAN WHERE MANCDREL=" & codiRelacio & " AND MANCDNOD=" & codiNode & " AND MANCDIDI=" & codiIdioma & " AND MANDTDAT<='" & data & "'")
    End Sub 'esborraAccioManteniment	




    'Faig una cerca del node que aparegui en alguna de les ubicacions demanades amb permis de lectura i que pertanyi a alguna de les codificacions demanada.
    ' Internet: si 1, és una cerca feta des de internet i comprovo que RELSWVIS=1
    Public Shared Function filtreCercador(ByVal objconn As OleDbConnection, ByVal selectNodes As String, ByVal strUbicacions As String, ByVal strUbicacionsFulla As String, ByVal strCodificacions As String, ByVal numNodeOrganigrama As Integer, ByVal estat As String, ByVal comprobarPermisos As Integer, ByVal internet As Integer) As Integer()
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim resultat As Integer() = {}
        Dim strSQL As String
        Dim cont As Integer = 0
        Dim relTMP As New clsRelacio
        Dim sqlUbicacionsFulla As String = "", elem As String = ""
        Dim aUbicacionsFulla As String()
        'dim heretat as integer

        aUbicacionsFulla = Split(strUbicacionsFulla, ",")

        For Each elem In aUbicacionsFulla
            If Trim(elem) <> "" Then
                sqlUbicacionsFulla &= " OR RELCDHER LIKE '%" & elem & "%'"
            End If
        Next
        DS = New DataSet()
        strSQL = "SELECT DISTINCT METLREL.RELINCOD, METLREL.RELINFIL FROM METLREL  WITH(NOLOCK) INNER JOIN (" & selectNodes.ToString() &
        ") AS T2 ON T2.CAMPCLAU = METLREL.RELINFIL "

        strSQL += " WHERE 1=1 "
        If estat <> "" Then
            strSQL += " AND RELCDSIT in (" + estat + ")"
        End If
        If internet = 1 Then
            strSQL += " AND RELSWVIS=1 "
        End If

        'GAIA.BDR(objconn, "SELECT RELINCOD FROM METLREL WHERE RELINFIL IN ("+strCodiNodes.toString() + ")"
        If Trim(strUbicacions) <> "" Or Trim(strUbicacionsFulla) <> "" Then
            If Trim(strUbicacions) = "" Then strUbicacions = "-1"
            strSQL += " AND RELINCOD IN (SELECT RELINCOD FROM METLREL WITH(NOLOCK) WHERE (RELINCOD IN (" + strUbicacions + ") " &
               sqlUbicacionsFulla & " )"

            If estat <> "" Then
                strSQL += " AND RELCDSIT in (" + estat + ")"
            End If
            strSQL += ")"
        End If
        If 2 = 1 Then
            If Trim(strCodificacions) <> "" Then
                strSQL += " AND RELINPAR IN (SELECT RELINFIL FROM METLREL  WITH(NOLOCK) WHERE RELINCOD IN (" + strCodificacions + ") "
                If estat <> "" Then
                    strSQL += " AND RELCDSIT in (" + estat + ")"
                End If
                If internet = 1 Then
                    strSQL += " AND RELSWVIS=1 "
                End If
                strSQL += ")"
            End If
        End If
        strSQL += " ORDER BY RELINFIL "
        GAIA2.bdr(objconn, strSQL, DS)

        Dim llistaPendents As String = "", llistaAmbPermisos As String = "", llistaSensePermisos As String = "", item As String = ""
        If comprobarPermisos = 2 Then
            For Each dbRow In DS.Tables(0).Rows
                If Not String.IsNullOrEmpty(llistaPendents) Then llistaPendents &= ","
                llistaPendents &= dbRow("RELINCOD")
            Next dbRow

            clsPermisos.trobaPermisLlistaRelacions(objconn, 9, llistaPendents, numNodeOrganigrama, "", "", llistaAmbPermisos, llistaSensePermisos, "")
            For Each item In llistaAmbPermisos.Split(",")
                ReDim Preserve resultat(cont)
                resultat(cont) = item
                cont += 1
            Next item
        Else

            ReDim resultat(DS.Tables(0).Rows.Count() - 1)

            For Each dbRow In DS.Tables(0).Rows


                resultat(cont) = dbRow("RELINCOD")
                cont += 1

            Next dbRow
        End If


        DS.Dispose()
        Return resultat
    End Function




    'Faig una cerca del node que aparegui en alguna de les ubicacions demanades amb permis de lectura i que pertanyi a alguna de les codificacions demanada.
    Public Shared Function filtreCercador2(ByVal objconn As OleDbConnection, ByVal selectNodes As String, ByVal strUbicacions As String, ByVal strUbicacionsFulla As String, ByVal strCodificacions As String, ByVal numNodeOrganigrama As Integer, ByVal estat As String, ByVal comprobarPermisos As Integer) As Integer()
        Dim DS As DataSet

        Dim resultat As Integer() = {}
        Dim strSQL As String
        Dim relTMP As New clsRelacio
        Dim sqlUbicacionsFulla As String = "", elem As String = ""
        Dim aUbicacionsFulla As String()
        aUbicacionsFulla = Split(strUbicacionsFulla, ",")
        For Each elem In aUbicacionsFulla
            If Trim(elem) <> "" Then
                sqlUbicacionsFulla &= " OR RELCDHER LIKE '%" & elem & "%'"
            End If
        Next
        DS = New DataSet()
        strSQL = "SELECT DISTINCT METLREL.RELINCOD, METLREL.RELINFIL FROM METLREL  WITH(NOLOCK) INNER JOIN (" & selectNodes.ToString() &
        ") AS T2 ON T2.CAMPCLAU = METLREL.RELINFIL "

        strSQL += " WHERE 1=1 "
        If estat <> "" Then
            strSQL += " AND RELCDSIT in (" + estat + ")"
        End If
        'GAIA.BDR(objconn, "SELECT RELINCOD FROM METLREL WHERE RELINFIL IN ("+strCodiNodes.toString() + ")"
        If Trim(strUbicacions) <> "" Or Trim(strUbicacionsFulla) <> "" Then
            If Trim(strUbicacions) = "" Then strUbicacions = "-1"
            strSQL += " AND RELINCOD IN (SELECT RELINCOD FROM METLREL  WITH(NOLOCK) WHERE (RELINCOD IN (" + strUbicacions + ") " &
               sqlUbicacionsFulla & " )"

            If estat <> "" Then
                strSQL += " AND RELCDSIT in (" + estat + ")"
            End If
            strSQL += ")"
        End If

        If Trim(strCodificacions) <> "" Then
            strSQL += " AND RELINPAR IN (SELECT RELINFIL FROM METLREL  WITH(NOLOCK) WHERE RELINCOD IN (" + strCodificacions + ") "
            If estat <> "" Then
                strSQL += " AND RELCDSIT in (" + estat + ")"
            End If
            strSQL += ")"
        End If
        strSQL += " ORDER BY RELINFIL "


        GAIA2.bdr(objconn, strSQL, DS)
        DS.Dispose()
        Return resultat
    End Function



    '****************************************************************************************************************
    '	Funció: obtenirEnllacContingut
    '	Entrada: 
    '						rel: relació on trobem el contingut
    '						codiIdioma: codi de l'idioma del contingut
    '						tipus:  0: l'enllaç serà el títol, en cas de ser una imatge, s'utilitzarà per retornar la imatge gran

    '								1: l'enllaç serà la descripció
    '								500: en cas de ser una imatge, es retornarà la versió de 500px (amb sufix P)
    '								100: en cas de ser una imatge es retornarà la versió de 100px (amb sufix p100)
    '						a tipus retorno el codi de tipus de contingut
    '						estilEnllaç:  llista d'estils css per representar l'enllaç
    '						target: destí de l'enllaç: 
    '										0: _self
    '										1: _blank
    '										Si és un contingut de tipus link faré el target que faci falta.
    '						icona: 0: No
    '									 1: Si
    '						estilIcona: css per representar la icona
    '						descripcio: 0: No
    '									 1: Si
    '						estilDescripcio: css per representar la descripció
    '						textEnllaç:
    '						plantilla: codi de plantilla que afegirem als paràmetres per representar l'enllaç
    '	Procés/sortida: 		
    '		retorna un enllaç ben format amb els estils necessaris.
    '****************************************************************************************************************	

    'Versió reduida que retorna l'enllaç amb els paràmetres
    Public Shared Function obtenirEnllacContingut(ByVal objconn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiIdioma As Integer) As String
        Dim tmp As String
        Dim link As String = ""
        Dim params As String = ""

        tmp = obtenirEnllacContingut(objconn, rel, codiIdioma, 0, "", "", 0, 0, "", 0, "", link, params, "")
        Return (link & params)

    End Function




    Public Shared Function obtenirEnllacContingut(ByVal objconn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiIdioma As Integer, ByRef tipus As Integer, ByVal estilenllaç As String, ByVal estiltext As String, ByVal target As Integer, ByVal icona As Integer, ByVal estilIcona As String, ByVal descripcio As Integer, ByVal estildescripcio As String, ByRef link As String, ByRef params As String) As String

        Return (obtenirEnllacContingut(objconn, rel, codiIdioma, tipus, estilenllaç, estiltext, target, icona, estilIcona, descripcio, estildescripcio, link, params, "", 0, 0))

    End Function

    Public Shared Function obtenirEnllacContingut(ByVal objconn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiIdioma As Integer, ByRef tipus As Integer, ByVal estilenllaç As String, ByVal estiltext As String, ByVal target As Integer, ByVal icona As Integer, ByVal estilIcona As String, ByVal descripcio As Integer, ByVal estildescripcio As String, ByRef link As String, ByRef params As String, ByVal textEnllaç As String) As String
        Return (obtenirEnllacContingut(objconn, rel, codiIdioma, tipus, estilenllaç, estiltext, target, icona, estilIcona, descripcio, estildescripcio, link, params, textEnllaç, 0, 0))

    End Function


    Public Shared Function obtenirEnllacContingut(ByVal objconn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiIdioma As Integer, ByRef tipus As Integer, ByVal estilenllaç As String, ByVal estiltext As String, ByVal target As Integer, ByVal icona As Integer, ByVal estilIcona As String, ByVal descripcio As Integer, ByVal estildescripcio As String, ByRef link As String, ByRef params As String, ByVal textEnllaç As String, ByVal plantilla As Integer) As String



        Return (obtenirEnllacContingut(objconn, rel, codiIdioma, tipus, estilenllaç, estiltext, target, icona, estilIcona, descripcio, estildescripcio, link, params, textEnllaç, plantilla, 0))
    End Function


    Public Shared Function obtenirEnllacContingut(ByVal objconn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiIdioma As Integer, ByRef tipus As Integer, ByVal estilenllaç As String, ByVal estiltext As String, ByVal target As Integer, ByVal icona As Integer, ByVal estilIcona As String, ByVal descripcio As Integer, ByVal estildescripcio As String, ByRef link As String, ByRef params As String, ByVal textEnllaç As String, ByVal plantilla As Integer, ByVal user As Integer) As String

        Dim oCodParam As New lhCodParam.lhCodParam
        Dim text As String = ""
        Dim resultat As String = ""
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim visorImatges As Integer = 0
        DS = New DataSet()
        Dim nomFitxer As String = ""
        Dim strDescripcio As String = ""
        link = ""
        params = ""
        Dim relLink As New clsRelacio()
        Dim tipusTMP As Integer = rel.tipintip
        Select Case rel.tipdsdes
            Case "fulla link"
                GAIA2.bdr(objconn, "SELECT    isnull(METLLNK.LNKCDREL,-1) as LNKCDREL,METLLNK.LNKDSLNK, METLLNK.LNKDSTXT, METLLNK.LNKDSDES, METLREI.REIDSFIT, METLREL.RELDSFIT, METLREL.RELINCOD FROM   METLLNK  WITH(NOLOCK) INNER JOIN METLREL  WITH(NOLOCK) ON METLLNK.LNKINNOD = METLREL.RELINFIL  AND  (METLREL.RELINCOD = " + rel.incod.ToString() + "  AND (METLREL.RELCDSIT<98)) LEFT JOIN METLREI  WITH(NOLOCK) ON METLREL.RELINCOD = METLREI.REIINCOD AND METLREI.REIINIDI = " + codiIdioma.ToString() + " WHERE    METLLNK.LNKINIDI =" + codiIdioma.ToString(), DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)

                    link = IIf(IsDBNull(dbRow("LNKDSLNK")), IIf(IsDBNull(dbRow("REIDSFIT")), dbRow("RELDSFIT"), dbRow("REIDSFIT")), dbRow("LNKDSLNK").ToString())

                    If link = "" And dbRow("LNKCDREL") <> rel.incod Then
                        relLink.bdget(objconn, dbRow("LNKCDREL"))
                        link = GAIA2.obtenirEnllacContingut(objconn, relLink, codiIdioma)
                        tipusTMP = relLink.tipintip
                    End If

                    If textEnllaç.Length = 0 Then
                        If tipus = 0 Then
                            text = dbRow("LNKDSTXT")
                        Else
                            If Not IsDBNull(dbRow("LNKDSDES")) Then
                                text = dbRow("LNKDSDES")
                            End If
                        End If
                        If Not IsDBNull(dbRow("LNKDSDES")) Then
                            strDescripcio = dbRow("LNKDSDES")
                        End If
                    Else
                        text = textEnllaç
                    End If
                End If

            Case "fulla directori"
                If plantilla = 0 Then
                    plantilla = 172661
                End If
                link = "http://www.l-h.cat/directori/detallEquipament.aspx?"
                params = "cr=" & rel.incod & "&pl=" & plantilla & "&id=" & codiIdioma & "&us=" & user & "&n=" & rel.infil

                GAIA2.bdr(objconn, "SELECT * FROM METLDIR  WITH(NOLOCK) WHERE DIRINNOD=" + rel.infil.ToString() + " AND DIRINIDI=" + codiIdioma.ToString(), DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    If textEnllaç.Length = 0 Then
                        text = dbRow("DIRDSNOM")
                    Else
                        text = textEnllaç
                    End If
                    If descripcio = 1 Then
                        strDescripcio = dbRow("DIRDSDES")
                    End If
                End If
            Case "fulla projecte"

                'Utilitzo el camp "tipus" per decidir l'enllaç de la pàgina
                Select Case tipus
                    Case 20
                        If codiIdioma = 1 Then
                            link = "http://www.l-h.cat/webs/lh2010/utils/detallProjecteOfHabitatge.aspx?"
                        Else

                            link = "http://www.l-h.cat/webs/lh2010/utils/detallProjecteOfHabitatge_2.aspx?"
                        End If
                    Case 30
                        If codiIdioma = 1 Then
                            link = "http://www.l-h.cat/webs/lh2010/utils/detallProjecteMobUrbana.aspx?"
                        Else
                            link = "http://www.l-h.cat/webs/lh2010/utils/detallProjecteMobUrbana_2.aspx?"
                        End If
                    Case Else '10
                        If codiIdioma = 1 Then
                            link = "http://www.l-h.cat/webs/lh2010/detallProjectePromocions.aspx?"
                        Else
                            link = "http://www.l-h.cat/webs/lh2010/detallProjectePromocions_2.aspx?"
                        End If
                End Select
                If plantilla = 0 Then
                    plantilla = 172661
                End If
                params = "cr=" & rel.incod & "&pl=" & plantilla & "&id=" & codiIdioma & "&us=" & user & "&n=" & rel.infil

                GAIA2.bdr(objconn, "SELECT * FROM METLFPR  WITH(NOLOCK) WHERE FPRINNOD=" & rel.infil & " AND FPRINIDI=" & codiIdioma, DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    If textEnllaç.Length = 0 Then
                        text = dbRow("FPRDSNOM")
                    Else
                        text = textEnllaç
                    End If
                    If descripcio = 1 Then
                        strDescripcio = dbRow("FPRDSDES")
                    End If
                End If
            Case "fulla tramit"
                If plantilla = 0 Then
                    plantilla = 150861
                End If

                params = codiIdioma & HttpUtility.UrlEncode(oCodParam.encriptar("cr=" & rel.incod & "&pl=" & plantilla & "&id=" & codiIdioma & "&us=" & user & "&n=" & rel.infil))
                GAIA2.bdr(objconn, "SELECT * FROM METLFTR  WITH(NOLOCK) WHERE FTRINNOD=" & rel.infil & " AND FTRINIDI=" & codiIdioma, DS)

                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    If dbRow("FTRSWVSE") = 1 Then
                        link = "https://seuelectronica.l-h.cat/tramits/detallTramit.aspx?"
                    Else
                        If dbRow("FTRSWVWE") = 1 Then
                            link = "http://www.l-h.cat/tramits/detallTramit.aspx?"
                        Else
                            If dbRow("FTRSWVIN") = 1 Then
                                link = "/tramits/detallTramit.aspx?"
                                'poso l'adreça relativa per evitar problemes amb els accesos via vpn
                            End If
                        End If
                    End If
                    dbRow = DS.Tables(0).Rows(0)
                    If textEnllaç.Length = 0 Then
                        text = dbRow("FTRDSNOM")
                    Else
                        text = textEnllaç
                    End If
                    If descripcio = 1 Then

                        strDescripcio = dbRow("FTRDSDES")
                    End If

                End If
            Case "fulla agenda"
                If plantilla = 0 Then
                    plantilla = 168660
                End If

                Select Case tipus
                    Case 50 'Museu 
                        link = "http://www.museul-h.cat/detallActe.aspx?" & codiIdioma
                    Case Else 'la resta de casos
                        link = "http://www.l-h.cat/agenda/detallAgenda.aspx?"
                End Select
                params = HttpUtility.UrlEncode(oCodParam.encriptar("cr=" & rel.incod & "&pl=" & plantilla & "&id=" & codiIdioma & "&us=" & user & "&n=" & rel.infil))

                GAIA2.bdr(objconn, "SELECT * FROM METLAGD  WITH(NOLOCK) WHERE AGDINNOD=" + rel.infil.ToString() + " AND AGDINIDI=" + codiIdioma.ToString(), DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    If textEnllaç.Length = 0 Then
                        text = dbRow("AGDDSTIT")
                    Else
                        text = textEnllaç
                    End If

                    If descripcio = 1 Then
                        strDescripcio = dbRow("AGDDSDES")
                    End If

                End If
            Case "fulla noticia"
                If plantilla = 0 Then
                    plantilla = 168578
                End If


                Select Case tipus
                    Case 50 'Museu 
                        If codiIdioma = 1 Then

                            link = "http://www.museul-h.cat/detallseducatiu.aspx?"
                        Else

                            link = "http://www.museul-h.cat/detallseducatiu_2.aspx?"
                        End If
                    Case Else 'la resta de casos
                        Select Case codiIdioma
                            Case 1
                                link = "http://www.l-h.cat/detallnoticia.aspx?"
                            Case 2
                                link = "http://www.l-h.cat/detallnoticia_2.aspx?"
                            Case 3
                                link = "http://www.l-h.cat/detallnoticia_3.aspx?"
                        End Select
                End Select
                params = codiIdioma & HttpUtility.UrlEncode(oCodParam.encriptar("cr=" & rel.incod & "&pl=" & plantilla & "&id=" & codiIdioma & "&us=" & user & "&n=" & rel.infil))

                GAIA2.bdr(objconn, "SELECT * FROM METLNOT  WITH(NOLOCK) WHERE NOTINNOD=" + rel.infil.ToString() + " AND NOTINIDI=" + codiIdioma.ToString(), DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    If textEnllaç.Length = 0 Then
                        text = dbRow("NOTDSTIT")
                    Else
                        text = textEnllaç
                    End If
                    If descripcio = 1 Then
                        If dbRow("NOTDSRES") = "" Then
                            If dbRow("NOTDSTXT").length > 100 Then
                                strDescripcio = dbRow("NOTDSTXT").substring(0, 100) + "..."
                            Else
                                strDescripcio = dbRow("NOTDSTXT")
                            End If
                        Else
                            strDescripcio = dbRow("NOTDSRES")
                        End If
                    End If


                End If

            Case "fulla info"
                If plantilla = 0 Then
                    plantilla = 180342
                End If

                link = "http://lhintranet/html/informacio.aspx"

                params = codiIdioma & HttpUtility.UrlEncode(oCodParam.encriptar("cr=" & rel.incod & "&pl=" & plantilla & "&id=" & codiIdioma & "&us=" & user & "&n=" & rel.infil))

                GAIA2.bdr(objconn, "SELECT * FROM METLINF  WITH(NOLOCK) WHERE INFINNOD=" & rel.infil & " AND INFINIDI=" & codiIdioma, DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)

                    If dbRow("INFWNBAU") > 0 Then
                        plantilla = 207395
                    End If
                    If textEnllaç.Length = 0 Then
                        text = dbRow("INFDSTIT")
                    Else
                        text = textEnllaç
                    End If
                    If descripcio = 1 Then
                        If dbRow("INFTDSRES") = "" Then
                            If dbRow("INFDSTXT").length > 100 Then
                                strDescripcio = dbRow("INFDSTXT").substring(0, 100) + "..."
                            Else
                                strDescripcio = dbRow("INFDSTXT")
                            End If
                        Else
                            strDescripcio = dbRow("INFDSRES")
                        End If
                    End If

                End If
            Case "fulla document"
                GAIA2.bdr(objconn, "SELECT TDODSNOM,METLDOC.* FROM METLDOC WITH(NOLOCK) ,METLTDO WITH(NOLOCK)  WHERE DOCINTDO=TDOCDTDO AND DOCINNOD=" + rel.infil.ToString() + " AND DOCINIDI=" + codiIdioma.ToString(), DS)
                If DS.Tables(0).Rows.Count = 0 Then
                    GAIA2.bdr(objconn, "SELECT TDODSNOM,METLDOC.* FROM METLDOC WITH(NOLOCK) ,METLTDO  WITH(NOLOCK) WHERE DOCINTDO=TDOCDTDO AND DOCINNOD=" + rel.infil.ToString() + " ORDER BY DOCINIDI ASC", DS)
                End If
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    nomFitxer = dbRow("DOCDSFIT")


                    'plantilla= -9 per no posar visor d'imatges
                    If InStr(UCase(dbRow("TDODSNOM")), "IMAGE") > 0 And plantilla <> -9 Then
                        link = "http://www.l-h.cat/utils/visorImatges.aspx?"
                        Select Case tipus
                            ' CASE 0 : no cal tractar-ho
                            Case 100
                                nomFitxer = nomFitxer.Replace("P100.", ".").Replace(".", "P100.")

                        End Select

                        visorImatges = 1
                    Else
                        link = "http://www.l-h.cat/utils/obrefitxer.ashx?"
                    End If
                    params = HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & rel.infil & "&codiIdioma=" & codiIdioma & "&f=" & nomFitxer))

                    If textEnllaç.Length = 0 Then
                        text = dbRow("DOCDSTIT")
                    Else
                        text = textEnllaç
                    End If
                    If descripcio = 1 Then
                        strDescripcio = dbRow("DOCDSDES")
                    End If
                End If

            Case "fulla web"
                link = "http://www.l-h.cat"
                GAIA2.bdr(objconn, "SELECT METLSER.SERDSURL , METLREI.REIDSFIT, WEBDSTIT FROM METLWEB  WITH(NOLOCK) INNER JOIN METLREL  WITH(NOLOCK) ON METLWEB.WEBINNOD = METLREL.RELINFIL INNER JOIN METLREI  WITH(NOLOCK) ON METLREI.REIINCOD = METLREL.RELINCOD INNER JOIN METLAWE  WITH(NOLOCK) ON SUBSTRING(METLREL.RELCDHER, 2, CHARINDEX('_', SUBSTRING(METLREL.RELCDHER, 2, LEN(METLREL.RELCDHER))) - 1) = METLAWE.AWEINNOD INNER JOIN METLSER  WITH(NOLOCK) ON METLAWE.AWEDSSER = METLSER.SERINCOD WHERE     (METLREL.RELINCOD = " + rel.incod.ToString() + ") ORDER BY METLREL.RELINCOD DESC", DS)

                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)
                    If Not IsDBNull(dbRow("REIDSFIT")) Then
                        link = "http://" + dbRow("SERDSURL") + dbRow("REIDSFIT")
                        If descripcio = 1 Then
                            strDescripcio = dbRow("WEBDSTIT")
                        End If
                    End If
                End If

        End Select

        resultat = "<a href=""#"" class=""" + estilenllaç + """"
        If target = 1 Or rel.tipdsdes = "fulla document" Then
            resultat = resultat + " target=""_blank"" "
        Else
            resultat = resultat + " target=""_self"" "
        End If
        resultat = resultat + ">"
        If icona Then
            resultat = "<img src=""http://www.l-h.cat/img/" + GAIA2.obtenirIconaTipusContingutPerRelacio(objconn, rel) + """ class=""" + estilIcona + """ hspace=""5"" align=""absmiddle""/>" + resultat
        End If
        resultat += text
        If descripcio Then
            resultat += "<br/><span class=""" + estildescripcio + """>" + strDescripcio + "</span>"
        End If
        If visorImatges Then
            'resultat+="<img src=""http://www.l-h.cat/img/common/mesimg.gif"" class=""mesImatges printhide"" alt=""" & iif(codiIdioma=2, "Ver imagen de ", "Veure imatge de ") & text & iif(codiIdioma=2, " (nueva ventana)"," (nova finestra)") & """/>"
            resultat += "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" class=""mesImatges noImprimible"" alt=""" & IIf(codiIdioma = 1, "Veure imatge de ", IIf(codiIdioma = 2, "Veure imatge de ", "Seeing image of ")) & text & IIf(codiIdioma = 1, " (nova finestra)", IIf(codiIdioma = 2, " (nueva ventana)", " (﻿new window)")) & """/>"

        End If
        tipus = tipusTMP
        resultat += "</a>"
        DS.Dispose()
        Return (resultat)

    End Function






    '****************************************************************************************************************
    '	Funció: obtenirTitolContingut
    '	Entrada: 			objconn: connexió bd
    '						rel: relació on trobem el contingut
    '						codiIdioma: codi de l'idioma del contingut
    '	Sortida: segons el tipus de contigut retorna un string amb el títol i idioma indicat. Si no existeix retorna ""
    '****************************************************************************************************************	

    Public Shared Function obtenirTitolContingut(ByVal objconn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiIdioma As Integer) As String

        Dim DS As DataSet
        obtenirTitolContingut = ""
        DS = New DataSet()
        If rel.incod = 0 Then
            'Si el contingut s'ha esborrat en la relació demanada, busco un altre que existeixi en la codificació
            GAIA2.bdr(objconn, "select TOP 1 relOk.RELINCOD FROM METLREL as relEsborrada WITH(NOLOCK) , METLREL as relOk  WITH(NOLOCK) where relEsborrada.RELINCOD=171604 and relOk.RELINFIL=relEsborrada.RELINFIL AND relOk.RELCDSIT<98 AND relOk.RELCDHER LIKE '_57135%'", DS)
            If DS.Tables(0).Rows.Count > 0 Then
                rel.bdget(objconn, DS.Tables(0).Rows(0)("RELINCOD"))
                DS.Dispose()
                obtenirTitolContingut = obtenirTitolContingut(objconn, rel, codiIdioma)
            End If
        Else
            Dim cons As String = ""

            Select Case rel.tipintip
                Case 3
                    cons = "select isnull(DIRDSNOM,'') as titol FROM METLDIR  WITH(NOLOCK) WHERE DIRINNOD=" & rel.infil & " AND DIRINIDI=" & codiIdioma
                Case 4
                    cons = "select isnull(NOTDSTIT,'') as titol FROM METLNOT  WITH(NOLOCK) WHERE NOTINNOD=" & rel.infil & " AND NOTINIDI=" & codiIdioma
                Case 5
                    cons = "select  isnull(DOCDSTIT,'') as titol FROM METLDOC  WITH(NOLOCK) WHERE DOCINNOD=" & rel.infil & " AND DOCINIDI=" & codiIdioma
                Case 10
                    cons = "select  isnull(WEBDSTIT,'') as titol FROM METLWEB  WITH(NOLOCK) WHERE WEBINNOD=" & rel.infil & " AND WEBINIDI=" & codiIdioma
                Case 40
                    cons = "select  isnull(CONDSDES,'') as titol FROM METLCON  WITH(NOLOCK) WHERE CONINNOD=" & rel.infil & " AND CONINIDI=" & codiIdioma
                Case 45
                    cons = "select  isnull(AGDDSTIT,'') as titol FROM METLAGD  WITH(NOLOCK) WHERE AGDINNOD=" & rel.infil & " AND AGDINIDI=" & codiIdioma
                Case 47
                    cons = "select  isnull(NCODSTIT,'') as titol FROM METLNCO  WITH(NOLOCK) WHERE NCOINNOD=" & rel.infil & " AND NCOINIDI=" & codiIdioma
                Case 49
                    cons = "select  isnull(LNKDSTXT,'') as titol FROM METLLNK  WITH(NOLOCK) WHERE LNKINNOD=" & rel.infil & " AND LNKINIDI=" & codiIdioma
                Case 51
                    cons = "select  isnull(FTRDSNOM,'') as titol FROM METLFTR  WITH(NOLOCK) WHERE FTRINNOD=" & rel.infil & " AND FTRINIDI=" & codiIdioma
                Case 55
                    cons = "select  isnull(FPRDSNOM,'') as titol FROM METLFPR  WITH(NOLOCK) WHERE FPRINNOD=" & rel.infil & " AND FPRINIDI=" & codiIdioma
                Case 56
                    cons = "select  isnull(INFDSTIT,'') as titol FROM METLINF  WITH(NOLOCK) WHERE INFINNOD=" & rel.infil & " AND INFINIDI=" & codiIdioma
            End Select


            GAIA2.bdr(objconn, cons, DS)
            If DS.Tables(0).Rows.Count > 0 Then
                obtenirTitolContingut = GAIA2.netejaHTML(DS.Tables(0).Rows(0)("titol"))
            Else
                obtenirTitolContingut = rel.noddstxt
            End If
            If rel.tipintip = 5 And obtenirTitolContingut + "" = "" Then
                If codiIdioma = 1 Then
                    obtenirTitolContingut = "Document"
                Else
                    obtenirTitolContingut = "Documento"
                End If
            End If
            DS.Dispose()

        End If
    End Function 'obtenirTitolContingut




    '****************************************************************************************************************
    '	Funció: obtenirDadesPaginaWeb
    '	Entrada: 			objconn: connexió bd
    '						rel: relació on trobem el contingut
    '						codiIdioma: codi de l'idioma del contingut
    '						nomWeb: per referencia, nom del web al que pertany la pàgina
    '						nomPagina: per referencia,nom de la pàgina
    '						descPagina: per referencia,descripció de la pagina
    '						
    '****************************************************************************************************************	

    Public Shared Sub obtenirNomWeb(ByVal objconn As OleDbConnection, ByVal rel As clsRelacio, ByVal codiIdioma As Integer, ByRef nomWeb As String, ByRef nomPagina As String, ByRef descPagina As String)

        Dim DS As DataSet
        Dim sql As String
        'rel.cdher= "a_b_85315_x

        Dim pos As Integer = 0
        Dim nodeWeb As String = "0"
        Dim arbreWeb As String = "0"

        nomWeb = ""
        nomPagina = ""
        descPagina = ""
        'Busco si és una web que penja de l'arbre de Web municipal		
        If rel.cdher.Length > 0 Then
            pos = InStr(rel.cdher, "_85315")

            If pos > 0 Then
                pos = pos + 7
                If rel.cdher.Length > pos Then
                    nodeWeb = rel.cdher.Substring(pos - 1)
                    pos = InStr(nodeWeb, "_")
                    If pos > 0 Then
                        nodeWeb = nodeWeb.Substring(0, pos - 1)
                    End If
                End If

            End If
            pos = InStr(rel.cdher.Substring(1), "_")
            If pos > 0 Then
                arbreWeb = rel.cdher.Substring(1, pos - 1)
            End If

        End If
        sql = "SELECT WEBDSTIT, WEBDSDES, NWEDSTIT, AWEDSTIT FROM METLWEB   WITH(NOLOCK) LEFT JOIN METLNWE  WITH(NOLOCK) ON NWEINNOD=" & nodeWeb & " LEFT JOIN METLAWE  WITH(NOLOCK) ON AWEINNOD=" & arbreWeb & " WHERE WEBINNOD = " & rel.infil & " AND WEBINIDI = " & codiIdioma

        DS = New DataSet()
        GAIA2.bdr(objconn, sql, DS)
        If DS.Tables(0).Rows.Count > 0 Then

            If Not IsDBNull(DS.Tables(0).Rows(0)("NWEDSTIT")) Then
                nomWeb = DS.Tables(0).Rows(0)("NWEDSTIT")
            Else
                If Not IsDBNull(DS.Tables(0).Rows(0)("AWEDSTIT")) Then
                    nomWeb = DS.Tables(0).Rows(0)("AWEDSTIT")
                End If
            End If
            nomPagina = DS.Tables(0).Rows(0)("WEBDSTIT")
            descPagina = DS.Tables(0).Rows(0)("WEBDSDES")
        End If
        DS.Dispose()
    End Sub










    Public Shared Sub modificaCodificacions(ByVal objconn As OleDbConnection, ByVal objArbre As RadTreeView, ByVal codiNode As Integer)
        modificaCodificacions(objconn, objArbre, codiNode, 1, 0, 0, False)
    End Sub
    Public Shared Sub modificaCodificacions(ByVal objconn As OleDbConnection, ByVal objArbre As RadTreeView, ByVal codiNode As Integer, ByVal nodeVisibleInternet As Integer)
        Dim resultat As String = ""
        resultat = modificaCodificacions(objconn, objArbre, codiNode, nodeVisibleInternet, 0, 0, False)
    End Sub

    Public Shared Function modificaCodificacions(ByVal objconn As OleDbConnection, ByVal objArbre As RadTreeView, ByVal codiNode As Integer, ByVal nodeVisibleInternet As Integer, ByVal usuari As Integer, Optional ByVal codiNodeArbre As Integer = 0, Optional ByVal tractaPermisosEnDiferit As Boolean = False) As String
        Dim strerr As String = ""
        Dim resultat As String = ""
        Dim relacionsNoEsborrades As String = ""
        Dim reltmp As New clsRelacio
        Dim tePermis As Boolean = 0
        Dim heretat As Integer = 0

        Dim llistaAmbPermisos As String = ""
        Dim llistaSensePermisos As String = ""

        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Try
            strerr &= "1"
            Dim llistarel As String = ""
            If codiNodeArbre = 0 Then
                codiNodeArbre = GAIA2.codiNodeByTxt(objconn, "arbre codificació", "arbre codificació")
            End If
            strerr &= "a"
            Dim cont As Integer = 0
            Dim treeNode As RadTreeNode
            Dim sql As String = ""
            Dim pathCodificacio As String
            For Each treeNode In objArbre.CheckedNodes
                If cont > 0 Then
                    llistarel += ","

                End If
                cont += 1
                llistarel += GAIA2.obtenirRelacioPantalla(treeNode).ToString()
            Next treeNode


            '	IF llistaRel <> "" THEN
            strerr &= "b"
            pathCodificacio = "_" & codiNodeArbre & "%"
            If objArbre.Nodes.Count > 0 Then
                If codiNodeArbre <> obtenirNodePantalla(objconn, objArbre.Nodes.Item(0)) Then
                    pathCodificacio = "_" & codiNodeArbre & "%_" & obtenirNodePantalla(objconn, objArbre.Nodes.Item(0)) & "%"
                Else
                    pathCodificacio = "_" & codiNodeArbre & "%"
                End If
            End If


            strerr &= "2"

            '***************************************************************************************
            ' Esborro totes les codificacions no marcades, només les que hi ha permís d'escriptura
            '***************************************************************************************



            sql = "SELECT RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELCDHER LIKE '" & pathCodificacio & "' AND RELINFIL=" & codiNode
            If llistarel.Length > 0 Then
                sql &= " AND RELCDRSU NOT IN (" & llistarel & ")"
            End If


            GAIA2.bdr(objconn, sql, DS)




            Dim llistarelacionsPerEsborrar As String = ""
            For Each dbRow In DS.Tables(0).Rows
                If llistarelacionsPerEsborrar.Length > 0 Then
                    llistarelacionsPerEsborrar &= ","
                End If
                llistarelacionsPerEsborrar &= dbRow("RELINCOD")
            Next dbRow
            strerr &= "3"

            If llistarelacionsPerEsborrar.Length > 0 Then

                Dim relacio As String = ""

                clsPermisos.trobaPermisLlistaRelacions(objconn, 3, llistarelacionsPerEsborrar, usuari, "", "", llistaAmbPermisos, llistaSensePermisos, "")
                'Esborro totes les codificacions on hi ha permís
                If llistaAmbPermisos.Length > 0 Then

                    GAIA2.bdSR(objconn, "delete from METLREL WHERE RELINCOD IN (" & llistaAmbPermisos & ")")
                End If
            End If



            strerr &= "4"

            For Each treeNode In objArbre.CheckedNodes
                'comprovo permisos d'edició/creació de l'usuari
                If usuari = 0 Then
                    tePermis = 1
                Else
                    reltmp.bdget(objconn, GAIA2.obtenirRelacioPantalla(treeNode))
                    tePermis = clsPermisos.tepermis(objconn, 7, usuari, usuari, reltmp, heretat, "", "")
                End If

                If tePermis Then
                    ''''''''arbol=46 es un problema a la hora de borrar contenidos.. me lo tengo que mirar, problemas con el 8
                    GAIA2.creaRelacio(objconn, 46, GAIA2.obtenirNodePantalla(objconn, treeNode), codiNode, 0, GAIA2.obtenirPathRelacioPantalla(objconn, treeNode) & "_" & GAIA2.obtenirNodePantalla(objconn, treeNode), -1, 1, -1, nodeVisibleInternet, tractaPermisosEnDiferit, usuari)

                    'si l'objecte té documents i/o notícies les poso també
                    GAIA2.bdr(objconn, "SELECT DISTINCT RELINPAR,RELINFIL,RELCDEST, RELCDORD, RELCDSIT FROM METLREL  WITH(NOLOCK) WHERE RELINPAR=" & codiNode, DS)
                    For Each dbRow In DS.Tables(0).Rows
                        GAIA2.creaRelacio(objconn, 46, dbRow("RELINPAR"), dbRow("RELINFIL"), 0, GAIA2.obtenirPathRelacioPantalla(objconn, treeNode) & "_" & GAIA2.obtenirNodePantalla(objconn, treeNode) & "_" & dbRow("RELINPAR"), dbRow("RELCDEST"), dbRow("RELCDSIT"), dbRow("RELCDORD"), nodeVisibleInternet, True, usuari)
                    Next dbRow


                Else
                    If resultat.Length > 0 Then
                        resultat = ", "
                    End If
                    resultat &= treeNode.Text
                End If
            Next treeNode
            strerr &= "5"
            If resultat <> "" Then
                resultat = "No s'ha pogut afegir el contingut, per falta de permisos, en les següents codificacions: " & resultat
            End If

            If llistaSensePermisos.Length > 0 Then
                Dim strError As String = ""
                If resultat.Length > 0 Then
                    resultat &= "<br />"
                End If
                GAIA2.bdr(objconn, "SELECT NODDSTXT FROM METLREL with(NOLOCK),METLNOD with(NOLOCK) WHERE NODINNOD=RELINPAR AND RELINCOD IN (" & llistaSensePermisos & ")", DS)
                For Each dbRow In DS.Tables(0).Rows
                    If strError.Length > 0 Then
                        strError &= ","
                    End If
                    strError &= HttpUtility.HtmlEncode(GAIA2.netejaHTML(dbRow("NODDSTXT")))
                Next dbRow

                resultat &= "No s'ha pogut eliminar el contingut de les següents codificacions per falta de permisos:" & strError
            End If
            strerr &= "6"
            DS.Dispose()
            '	End if
        Catch ex As Exception
            GAIA2.debug(objconn, "GAIA.Modifica codificacions." & strerr & ex.Message)

        End Try

        Return resultat
    End Function 'modificaCodificacions




    'La clase clsPermisos no es pot accedir des de fora i faig això per que el servei srvGaiaPermisos pugui mantenir els permisos heretats
    Public Shared Sub mantenimentPermisosHeretats()
        clsPermisos.mantenimentPermisosHeretats()
    End Sub

#Region "UTILS"


    '*******************************************************************************************************
    ' Talla el text "str" a la mida indicada. El text pot venir en html i es tenen en compte les etiquetes
    '*******************************************************************************************************
    Public Shared Function tallaText(ByVal str As String, ByVal mida As Integer) As String
        Dim strErr As String = ""
        Dim midaTmp As Integer = 0
        Try
            If mida > str.Length Then
                Return (str)
            Else
                Dim posIni As Integer = 0
                Dim posFi As Integer = -1
                Dim strTmp As String


                midaTmp = mida
                Do
                    strTmp = str.Substring(0, midaTmp)
                    posIni = InStrRev(strTmp, "<")
                    posFi = InStrRev(strTmp, ">")
                    midaTmp = midaTmp + 1
                Loop Until posIni = 0 Or posIni < posFi Or midaTmp >= str.Length - 1



                'busco el següent espai per fer el tall per evitar tallar en mig d'una paraula
                If InStr(str.Substring(mida), " ") > 0 Then
                    strTmp = strTmp & str.Substring(mida, InStr(str.Substring(mida), " ") - 1)
                End If

                'busco si he tallat un tag i el tanco. si hi ha tags anidats no funcionarà. En el cas de textos enriquits de gaia només pot passar si hi ha <table o <li> i no és probable dins del resum d'una notícia.
                posIni = InStrRev(strTmp, "<")
                posFi = InStrRev(strTmp, ">")

                If InStrRev(strTmp, "<") > 0 And posIni > posFi Then
                    'trobo un inici d'un tag

                    strTmp = strTmp & "</" & strTmp.Split(" ")(0).Substring(1) & ">" ' strTmp.Substring(posIni, (posFi - posIni))
                End If

                Return (strTmp)


            End If
        Catch
            GAIA2.debug(Nothing, strErr & "_ " & str & "_" & mida)
            Return (str)
        End Try
    End Function




    '*******************************************************************************************************
    'Convierte un punto en ED50 (UTM o Cartografia) a WGS84 (longitud/latitud) 
    '*******************************************************************************************************
    Public Shared Function DameLonLatDeCoordenada(ByVal punto As String) As String
        Dim CG As New Cartografia.CartoGEOM
        Dim wktFrom As String = "PROJCS[""ED50 / UTM zone 31N"",GEOGCS[""ED50"",DATUM[""European Datum 1950"",SPHEROID[""International 1924"", 6378388.0, 297.0,AUTHORITY[""EPSG"",""7022""]],TOWGS84[-157.89,-17.16,-78.41,2.118,2.697,-1.434,-1.1097046576093785], AUTHORITY[""EPSG"",""6230""]],PRIMEM[""Greenwich"", 0.0,AUTHORITY[""EPSG"",""8901""]],UNIT[""degree"", 0.017453292519943295],AXIS[""Geodetic longitude"", EAST],AXIS[""Geodetic latitude"", NORTH],AUTHORITY[""EPSG"",""4230""]],PROJECTION[""Transverse_Mercator""],PARAMETER[""central_meridian"", 3.0],PARAMETER[""latitude_of_origin"", 0.0],PARAMETER[""scale_factor"", 0.9996],PARAMETER[""false_easting"", 500000.0],PARAMETER[""false_northing"", 0.0],UNIT[""m"", 1.0], AXIS[""Easting"", EAST], AXIS[""Northing"", NORTH],AUTHORITY[""EPSG"",""23031""]]"
        Dim csFrom As IInfo = SharpMap.Converters.WellKnownText.CoordinateSystemWktReader.Parse(wktFrom)
        Dim csWgs84 As GeographicCoordinateSystem = GeographicCoordinateSystem.WGS84
        Dim ctFactory As CoordinateTransformationFactory = New CoordinateTransformationFactory()
        Dim trans As ICoordinateTransformation = ctFactory.CreateFromCoordinateSystems(csFrom, csWgs84)
        Dim numberFormat_EnUS As System.Globalization.NumberFormatInfo = New System.Globalization.CultureInfo("en-US", False).NumberFormat
        Dim ptoWkt As String = "POINT(" & CG.DameUTM(punto).Replace(",", " ") & ")"
        Dim pto As SharpMap.Geometries.Point = SharpMap.Converters.WellKnownText.GeometryFromWKT.Parse(ptoWkt)
        Dim ptoRes As Double() = trans.MathTransform.Transform(pto.ToDoubleArray)
        Return ptoRes(1).ToString(numberFormat_EnUS) & "," & ptoRes(0).ToString(numberFormat_EnUS)
    End Function



    Public Shared Function generaPDF(ByVal objConn As OleDbConnection, ByVal rel As clsRelacio) As Integer
        Try
            Dim cont As Integer = 0
            Dim textPeu As String = "Ajuntament de L'Hospitalet. Pl. de l'Ajuntament, 11. 08901 L'Hospitalet de Llobregat. Tel. 934.029.400"
            Dim DS As DataSet
            Dim impressio As String = ""

            If rel.tipdsdes = "fulla tramit" Then
                DS = New DataSet()
                GAIA2.bdr(objConn, "SELECT DISTINCT FTRSWVSE,FTRSWVWE, FTRSWVIN FROM METLFTR  WITH(NOLOCK) WHERE FTRINIDI=1 AND FTRINNOD=" & rel.infil, DS)
                If DS.Tables(0).Rows.Count > 0 Then
                    If DS.Tables(0).Rows(0)("FTRSWVWE") = "1" Then
                        impressio = "http://www.l-h.cat/tramits/impressioTramitWEB.aspx"
                    End If
                    If DS.Tables(0).Rows(0)("FTRSWVSE") = "1" Then
                        impressio = "http://www.l-h.cat/tramits/impressioTramitSEU.aspx"
                    End If

                End If
                DS.Dispose()
            End If
            If rel.tipdsdes = "fulla agenda" Then
                'impressio = "http://www.l-h.cat/tramits/impressioAgenda.aspx"
                ' de moment ho comento per que no es faci quan es fulla agenda, la generació dels pdfs és molt lenta i s'haurà de fer d'una altra forma
            End If


            If impressio <> "" Then

                'Guardo la versió que existia
                If File.Exists("e:\docs\GAIA\contingutsPDF\1_" & rel.infil & ".pdf") Then
                    'Busco la darrera versió
                    While File.Exists("e:\docs\GAIA\contingutsPDF\1_" & rel.infil & "_V" & cont & ".pdf")
                        cont = cont + 1
                    End While
                    File.Move("e:\docs\GAIA\contingutsPDF\1_" & rel.infil & ".pdf", "e:\docs\GAIA\contingutsPDF\1_" & rel.infil & "_V" & cont & ".pdf")
                    File.Move("e:\docs\GAIA\contingutsPDF\2_" & rel.infil & ".pdf", "e:\docs\GAIA\contingutsPDF\2_" & rel.infil & "_V" & cont & ".pdf")
                End If

                Dim process As New System.Diagnostics.Process
                process.StartInfo.FileName = "c:\Inetpub\wwwroot\GAIA\ASPX\carregaAuto\pdf\wkhtmltopdf.exe"
                process.StartInfo.Arguments = "--disable-internal-links  --print-media-type --minimum-font-size 14 --footer-font-size 8  --footer-left """ & textPeu & " - Darrera actualització: " & Now & """   --footer-right [page]/[topage]   --footer-line   " & impressio & "?cr=" & rel.incod & "&idioma=1 e:\docs\GAIA\contingutsPDF\1_" & rel.infil & ".pdf" ' --load-error-handling ignore"
                process.Start()
                process.WaitForExit()
                process.Close()

                process.StartInfo.FileName = "c:\Inetpub\wwwroot\GAIA\ASPX\carregaAuto\pdf\wkhtmltopdf.exe"
                process.StartInfo.Arguments = "--disable-internal-links   --print-media-type  --minimum-font-size 14 --footer-font-size 8  --footer-left """ & textPeu & " - Última actualitzación: " & Now & """  --footer-right [page]/[topage]   --footer-line " & impressio & "?cr=" & rel.incod & "&idioma=2 e:\docs\GAIA\contingutsPDF\2_" & rel.infil & ".pdf" ' --load-error-handling ignore"
                process.Start()
                process.WaitForExit()
                process.Close()
                cont += 1
            End If

            Return (cont)

        Catch
            GAIA2.f_logError(objConn, "GeneraPDF", "", Err.Description)
            Return (0)
        End Try
    End Function 'GeneraPDF

    Public Shared Function netejaHTML(ByVal strHtml As String) As String
        Dim regexp As Regex
        Dim strPattern As String = "<(.|\n)+?>"
        regexp = New Regex(strPattern, RegexOptions.IgnoreCase)
        strHtml = strHtml.Replace("'", "&rsquo;").Replace("""", "&quot;").Replace(Chr(13) + Chr(10), "")
        strHtml = Regex.Replace(strHtml, strPattern, "", RegexOptions.IgnoreCase)

        regexp = Nothing
        Return strHtml
    End Function


    'Donat un codi de relació i un idioma, genera un llistat <ul><li> de links que conformen un menú que penja de "relació"
    'Si cssDiv1 i cssDivN <>"" envolcaré el <ul> en un <div>.  Els estils són per quan només hi ha 1 element i per quan hi ha N respectivament


    Public Shared Function menuLinks(ByVal objConn As OleDbConnection, ByVal codiRelacio As Integer, ByVal codiIdioma As Integer, Optional ByVal cssDiv1 As String = "", Optional ByRef cssDivN As String = "", Optional ByVal cssDivFill1 As String = "", Optional ByRef cssDivFillN As String = "", Optional ByVal codiRelacioDestacada As Integer = 0, Optional ByVal cssDestacat As String = "") As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim strMenu As String = ""
        Dim Link As String
        Dim txtNovaFinestra As String = IIf(codiIdioma = 1, " (nova finestra)", " (nueva ventana)")
        Dim cont As Integer = 0
        Dim strImatgeFinestraNova As String = ""
        Dim rel As New clsRelacio()
        Dim primeraRel As Integer = 0

        GAIA2.bdr(objConn, "SELECT   DISTINCT  LNKINNOD, ISNULL(LNKCDREL,0) AS LNKCDREL,ISNULL(METLWEB.WEBSWSSL,'N') as paginaSegura,  METLLNK.LNKWNTIP, METLLNK.LNKDSLNK,METLLNK.LNKDSTXT, relContingut.RELDSFIT, METLREI.REIDSFIT, relMenu.RELINCOD,relMenu.RELCDORD, count(relFill.RELINCOD) as nroFills FROM METLREL relMenu  WITH(NOLOCK) INNER JOIN METLLNK  WITH(NOLOCK) ON relMenu.RELINFIL = METLLNK.LNKINNOD AND relMenu.RELSWVIS=1 AND  (METLLNK.LNKDTPUB<getdate() OR METLLNK.LNKDTPUB='1/1/1900') AND (METLLNK.LNKINIDI =" & codiIdioma & ") LEFT OUTER JOIN METLREI WITH(NOLOCK)  ON METLLNK.LNKCDREL = METLREI.REIINCOD AND METLREI.REIINCOD<>0 AND METLREI.REIINIDI =" & codiIdioma & " LEFT OUTER JOIN METLREL relContingut  WITH(NOLOCK) ON METLLNK.LNKCDREL = relContingut.RELINCOD AND relContingut.RELSWVIS=1  AND relContingut.RELINCOD<>0 AND relContingut.RELCDSIT<98 LEFT OUTER JOIN METLWEB ON WEBINIDI=" & codiIdioma & " AND WEBINNOD=relContingut.RELINFIL LEFT OUTER JOIN METLREL as relFill ON relFill.RELCDRSU=relMenu.RELINCOD AND relFill.RELCDSIT<98 WHERE (relMenu.RELCDRSU= " & codiRelacio & ")  AND (relMenu.RELCDSIT < 98)  GROUP BY LNKINNOD,  LNKCDREL, WEBSWSSL,  METLLNK.LNKWNTIP, METLLNK.LNKDSLNK,METLLNK.LNKDSTXT, relMenu.RELINCOD,relContingut.RELDSFIT, METLREI.REIDSFIT, relMenu.RELCDORD ORDER BY relMenu.RELCDORD", DS)

        For Each dbRow In DS.Tables(0).Rows

            rel.bdget(objConn, dbRow("LNKCDREL"))
            If cont = 0 Then primeraRel = rel.incod
            cont = cont + 1
            Link = IIf(IsDBNull(dbRow("REIDSFIT")), IIf(IsDBNull(dbRow("RELDSFIT")), dbRow("LNKDSLNK"), dbRow("RELDSFIT")), dbRow("REIDSFIT").ToString())
            If Link = "" Then
                Link = GAIA2.obtenirEnllacContingut(objConn, rel, codiIdioma)
            End If

            If InStr(Link, ".asp") Then
                If InStr(Link, "?") Then
                Else
                    If InStr(Link, "#") Then
                        Link = Link.Replace("#", "?id=" & codiIdioma & "#")
                    Else
                        If InStr(Link, "?") <= 0 Then
                            Link += IIf(InStr(Link, ".asp"), "?id=" & codiIdioma, "")
                        End If
                    End If
                End If
            End If



            If InStr(Link, "mailto") > 0 Then
                'no faig res
            Else

                Link = IIf(InStr(Link, "http") <= 0, IIf(dbRow("paginaSegura") = "S", "https://", "http://") & GAIA2.trobaURLDesti(objConn, rel), "") & Link
            End If

            If InStr(Link, "opendata") > 0 Or InStr(Link, "dadesobertes") > 0 Then
                strImatgeFinestraNova = "<img class=""finestraNova"" alt=""" & IIf(codiIdioma = 1, "Obrir enllaç dades obertes (finestra nova) ", "Abrir enlace datos abiertos (ventana nueva") & txtNovaFinestra & """ src=""" & IIf(InStr(Link, "https"), "https", "http") & "://www.l-h.cat/img/lh12/common/candau.png""/>"

            Else
                strImatgeFinestraNova = "<img class=""finestraNova"" alt=""" & IIf(codiIdioma = 1, "Obrir enllaç", "Abrir enlace (ventana nueva") & txtNovaFinestra & """ src=""" & IIf(InStr(Link, "https"), "https", "http") & "://www.l-h.cat/img/lh12/common/finestra_nova.png""/>"
            End If


            strMenu &= IIf(cont > 1, "</li><li" & IIf(codiRelacioDestacada = rel.incod, " class=""" & cssDestacat & """", "") & ">", "") & "<a href=""" & GAIA2.netejaHTML(Link) & """ target=""" & IIf(dbRow("LNKWNTIP") = 1, "_blank", "_self") & """ title=""" & IIf(codiIdioma = "1", "Anar a ", "Ir a ") & dbRow("LNKDSTXT") & IIf(dbRow("LNKWNTIP") = 1, txtNovaFinestra, "") & """>" & dbRow("LNKDSTXT") & IIf(dbRow("LNKWNTIP") = 1, strImatgeFinestraNova, "") & "</a>"

            '*********************************************
            ' Tracto els fills
            '********************************************
            If cssDivFill1.Length > 0 And dbRow("nroFills") > 0 Then
                strMenu &= GAIA2.menuLinks(objConn, dbRow("RELINCOD"), codiIdioma, cssDivFill1, cssDivFillN, "", "", codiRelacioDestacada, cssDestacat)
            End If
        Next dbRow



        If strMenu <> "" Then
            If cont > 1 Then
                strMenu = "<ul><li " & IIf(codiRelacioDestacada = primeraRel, "class=""" & cssDestacat & """", "") & ">" & strMenu & "</li></ul>"
                If cssDivN <> "" Then
                    strMenu = "<div class=""" & cssDivN & """>" & strMenu & "</div>"
                End If

            Else
                If cssDiv1 <> "" Then
                    strMenu = "<div class=""" & cssDiv1 & """>" & strMenu & "</div>"
                End If
            End If
            'strMenu = "<ul><li>" & strMenu  & "</li></ul>"
        End If




        DS.Dispose()
        Return strMenu

    End Function



    '<summary>
    'Es retorna un dataset amb la llista d'enllaços a continguts que pengen d'una relació. 
    ' El dataset incorpora els camps : URL, CODINODE, CODIRELACIO, ICONA, DATACREACIO, TITOL, VISIBLEOAC, VISIBLE010
    ' Aquests dos darrers camps són propis de les fulles informació
    '</summary>
    '<param name="codiIdioma">Codi d'idioma del destí</param>
    '<param name="codiRelacio">Codi de relació pare d'on pengen els continguts</param>
    '<param name="dataFi">Data de final</param>
    '<returns>El text formatejat segons idioma del rang de dates</returns>
    Public Shared Function obtenirEnllacosDS(ByVal objConn As OleDbConnection, ByVal codiIdioma As Integer, ByVal codiRelacio As Integer, ByVal codiUsuari As Integer) As DataSet

        Dim DS As DataSet
        DS = New DataSet()
        Dim rel As New clsRelacio
        Dim link As String = ""
        Dim params As String = ""
        Dim contingut As String = ""
        Dim cont As Integer = 0
        Dim tipus As Integer = 0
        If codiIdioma = 0 Then codiIdioma = 1
        GAIA2.bdr(objConn, "SELECT 0 as TIPUS, '' AS URL, NODINNOD AS CODINODE, relfill.RELINCOD AS CODIRELACIO, '' AS ICONA, NODDTTIM AS DATACREACIO, NODDSTXT AS TITOL,  isNUll(INFWNOAC,'') as VISIBLEOAC, isNUll(INFWN010,'') as VISIBLE010 FROM  METLREL AS relPare WITH (NOLOCK) INNER JOIN METLREL AS relfill WITH (NOLOCK) ON  relfill.RELCDSIT < 90 AND relfill.RELCDHER LIKE (relPare.RELCDHER + '_' + CAST(relPare.RELINFIL AS VARCHAR)) INNER JOIN  METLNOD WITH (NOLOCK) ON  NODCDTIP <> 10 AND relfill.RELINFIL = NODINNOD LEFT OUTER JOIN METLINF ON INFINNOD=NODINNOD AND INFINIDI=" & codiIdioma & " WHERE relPare.RELINCOD = " & codiRelacio & " AND relPare.RELCDSIT < 90 ORDER BY relfill.RELCDORD ", DS)
        For cont = 0 To DS.Tables(0).Rows.Count - 1
            rel.bdget(objConn, DS.Tables(0).Rows(cont)("CODIRELACIO"))

            contingut = GAIA2.obtenirEnllacContingut(objConn, rel, codiIdioma, tipus, "", "", 1, 1, "", 0, "", link, params, "", 0, codiUsuari)
            DS.Tables(0).Rows(cont)("ICONA") = contingut.Substring(InStr(contingut, """"))
            DS.Tables(0).Rows(cont)("ICONA") = DS.Tables(0).Rows(cont)("ICONA").substring(0, InStr(DS.Tables(0).Rows(cont)("ICONA"), """") - 1)
            DS.Tables(0).Rows(cont)("URL") = link & IIf(String.IsNullOrEmpty(params), "", "?" & params)
            DS.Tables(0).Rows(cont)("TIPUS") = tipus


        Next cont

        DS.AcceptChanges()
        Return (DS)
    End Function



    '''<summary>
    '''A partir d'una crida a detallTramit amb paràmetres encriptats, generem una altra, sense encriptar  i que apunta a la llibreria de codi de la cel·la central del tràmit.  		
    '''</summary>
    '''<param name="linkOrigen">Enllaç que haurà retornat GAIA amb paràmetres encriptats</param>
    '''<param name="width">Mida en la que es vol el contingut. Si 0, prenem mida estàndard: 890 px.</param>
    '''<param name="rol">Rol amb el que es vol obrir el contingut: 0:010 | 1:OAC | 2: altres depts</param>
    '''<returns>La crida a llibreria de codi web, només amb el detall del tràmit</returns>
    Public Shared Function csTraduccioLinkTramit(ByVal linkOrigen As String, ByVal width As Integer, ByVal rol As Integer) As String
        Dim linkTramit As String = ""
        Try
            If width = 0 Then width = 890
            Dim oCodParam As New lhCodParam.lhCodParam
            Dim params As String = HttpUtility.UrlDecode(linkOrigen.Split("?")(1).Substring(1))
            If oCodParam.queryString(params, "cr") = "" Then
                linkTramit = linkOrigen
            Else

                linkTramit = "/gaia/aspx/llibreriacodiweb/webmunicipal/selectronica/detallTramitSeu.aspx?" & oCodParam.encriptar("codiRelacio=" & oCodParam.queryString(params, "cr") & "&codiIdioma=" & oCodParam.queryString(params, "id") & "&codiRelacioInicial=" & oCodParam.queryString(params, "cr") & "&est=0&width=" & width & "&node=" & oCodParam.queryString(params, "n") & "&us=" & oCodParam.queryString(params, "us") & "&codiPlantilla=" & oCodParam.queryString(params, "pl") & "&rol=" & rol)
            End If
        Catch
            linkTramit = linkOrigen
        End Try
        Return (linkTramit)
    End Function



    '<summary>
    'Aquest métode formateja dos dates, data inici i data final, a "del diaInici del mes d'inici de l'any d'inici a .. data final"
    '</summary>
    '<param name="idioma">Codi d'idioma del text de sortida</param>
    '<param name="dataInici">Data d'inici</param>
    '<param name="dataFi">Data de final</param>
    '<returns>El text formatejat segons idioma del rang de dates</returns>

    Public Shared Function formatejaData(ByVal codiIdioma As Integer, ByVal sdataInici As String, ByVal sdataFi As String, Optional ByVal sHora As String = "", Optional ByVal mostrarDiaSetmana As Boolean = False) As String
        Dim mesCat() As String = {"de gener", "de febrer", "de mar&ccedil;", "d'abril", "de maig", "de juny", "de juliol", "d'agost", "de setembre", "d'octubre", "de novembre", "de desembre"}
        Dim mesCas() As String = {"de enero", "de febrero", "de marzo", "de abril", "de mayo", "de junio", "de julio", "de agosto", "de septiembre", "de octubre", "de noviembre", "de diciembre"}
        Dim diaCat() As String = {"Diumenge", "Dilluns", "Dimarts", "Dimecres", "Dijous", "Divendres", "Dissabte"}
        Dim diaCas() As String = {"Domingo", "Lunes", "Martes", "Mi&eacute;rcoles", "Jueves", "Viernes", "S&aacute;bado"}


        Dim sortida As String = ""
        Dim dataInici, dataFi As Date

        Dim strInpatternMes As String = "gener|febrer|mar&ccedil;|abril|maig|juny|juliol|agost|setembre|octubre|novembre|desembre|enero|marzo|mayo|junio|julio|septiembre|noviembre|diciembre"
        sHora = LCase(sHora.Trim())

        'Si han posat el mes en la hora, llavors mostraré només aquest contingut i no la resta.
        If Not Regex.IsMatch(sHora, strInpatternMes) Then
            If sdataInici <> "" Then
                If sdataFi <> "" Then
                    dataInici = CDate(sdataInici)
                    dataFi = CDate(sdataFi)

                    If dataFi = CDate("1/1/1900") Then
                        dataFi = dataInici
                    End If
                    If Year(dataFi) > Year(Now) + 1 Then  'si la data de final és 3 anys superior a l'actual, mostraré un text tipus "des del x de y de z"
                        sortida &= IIf(codiIdioma = 1, IIf(Day(dataInici) = 1, "des de l'1", "des del " & Day(dataInici)), "desde el " & Day(dataInici)) & " " & IIf(codiIdioma = 1, mesCat(Month(dataInici) - 1), mesCas(Month(dataInici) - 1))  '& " de " & Year(dataInici)

                    Else

                        If Year(dataInici) = Year(dataFi) Then
                            If Month(dataInici) = Month(dataFi) Then
                                'mateix any i mes, diferent dia
                                If Day(dataInici) <> Day(dataFi) Then
                                    sortida &= IIf(codiIdioma = 1, IIf(Day(dataInici) = 1, "de l'1", "del " & Day(dataInici)), "del " & Day(dataInici)) & " " & IIf(codiIdioma = 1, IIf(Day(dataFi) = 1, "a l'1", "al " & Day(dataFi)), "al " & Day(dataFi)) & " " & IIf(codiIdioma = 1, mesCat(Month(dataFi) - 1), mesCas(Month(dataFi) - 1)) '& " de " & Year(dataFi)
                                Else
                                    'mateix any, mes i dia

                                    If mostrarDiaSetmana Then
                                        sortida = IIf(codiIdioma = 1, diaCat(Weekday(dataInici) - 1), diaCas(Weekday(dataInici) - 1)) & ",&nbsp;"
                                    End If

                                    sortida &= Day(dataInici) & " " & IIf(codiIdioma = 1, mesCat(Month(dataInici) - 1), mesCas(Month(dataInici) - 1)) ' & " de " & Year(dataInici)
                                    If Year(dataInici) > Year(Now) Then sortida &= " " & Year(dataInici)
                                End If
                            Else
                                'mateix any, diferent mes i dia. He de mostrar: "del diaIni de mesIni fins al diaFi de mesFi de anyFi"	
                                If mostrarDiaSetmana Then
                                    sortida = IIf(codiIdioma = 1, "del " & StrConv(diaCat(Weekday(dataInici) - 1), 2) & " " & Day(dataInici) & " " & mesCat(Month(dataInici) - 1) & " al " & StrConv(diaCat(Weekday(dataFi) - 1), 2) & " " & Day(dataFi) & " " & mesCat(Month(dataFi) - 1), "del " & StrConv(diaCas(Weekday(dataInici) - 1), 2) & " " & Day(dataInici) & " " & mesCas(Month(dataInici) - 1) & " al " & StrConv(diaCas(Weekday(dataFi) - 1), 2) & " " & Day(dataFi) & " " & mesCas(Month(dataFi) - 1))
                                Else
                                    sortida = IIf(codiIdioma = 1, IIf(Day(dataInici) = 1, "de l'1", "del " & Day(dataInici)), "del " & Day(dataInici)) & " " & IIf(codiIdioma = 1, mesCat(Month(dataInici) - 1), mesCas(Month(dataInici) - 1)) & " " & IIf(codiIdioma = 1, IIf(Day(dataFi) = 1, "a l'1", "al " & Day(dataFi)), "al " & Day(dataFi)) & " " & IIf(codiIdioma = 1, mesCat(Month(dataFi) - 1), mesCas(Month(dataFi) - 1)) ' & " de " & Year(dataFi)
                                End If
                                StrConv(diaCat(Weekday(dataFi) - 1), 2)

                            End If
                        Else ' year(dataInici) <> year(dataFi)
                            'diferent any, he de mostrar:  "del disSemana diaIni de mesIni de anyIni fins al diaSemana diaFi de mesFi de anyFi"
                            If mostrarDiaSetmana Then
                                sortida = IIf(codiIdioma = 1, "del " & StrConv(diaCat(Weekday(dataInici) - 1), 2) & " " & Day(dataInici) & " " & mesCat(Month(dataInici) - 1) & " de " & Year(dataInici) & " al " & StrConv(diaCat(Weekday(dataFi) - 1), 2) & " " & Day(dataFi) & " " & mesCat(Month(dataFi) - 1) & " de " & Year(dataFi), "del " & StrConv(diaCas(Weekday(dataInici) - 1), 2) & " " & Day(dataInici) & " " & mesCas(Month(dataInici) - 1) & " de " & Year(dataInici) & " al " & StrConv(diaCas(Weekday(dataFi) - 1), 2) & " " & Day(dataFi) & " " & mesCas(Month(dataFi) - 1) & " de " & Year(dataFi))

                            Else
                                sortida = IIf(codiIdioma = 1, IIf(Day(dataInici) = 1, "de l'1", "del " & Day(dataInici)), "del " & Day(dataInici)) & " " & IIf(codiIdioma = 1, mesCat(Month(dataInici) - 1), mesCas(Month(dataInici) - 1)) & " de " & Year(dataInici) & " " & IIf(codiIdioma = 1, IIf(Day(dataFi) = 1, "a l'1", "al " & Day(dataFi)), "al " & Day(dataFi)) & " " & IIf(codiIdioma = 1, mesCat(Month(dataFi) - 1), mesCas(Month(dataFi) - 1)) & " de " & Year(dataFi)
                            End If
                        End If
                    End If
                Else
                    'no hi ha data de fi = mateix any, mes i dia
                    dataInici = CDate(sdataInici)
                    If mostrarDiaSetmana Then
                        sortida = IIf(codiIdioma = 1, diaCat(Weekday(dataInici) - 1), diaCas(Weekday(dataInici) - 1)) & ",&nbsp;"

                    End If
                    sortida &= Day(dataInici) & " " & IIf(codiIdioma = 1, mesCat(Month(dataInici) - 1), mesCas(Month(dataInici) - 1)) '& " de " & Year(dataInici)
                End If
            Else
            End If

        End If
        'Si tinc la hora com a paràmetre, la formatejo per afegir-la al text		
        If sHora <> "" Then

            Dim tmp As String = sHora


            If tmp.Length > 0 Then

                If tmp.Length > 0 Then
                    If tmp.Substring(0, 1) = ":" Then tmp = tmp.Substring(1)
                    If tmp.Substring(0, 1) = "," Then tmp = tmp.Substring(1)
                    If tmp.Substring(0, 1) = " " Then tmp = tmp.Substring(1)

                    tmp = tmp.Replace("hores", "h").Replace("horas", "h")
                    tmp = tmp.Trim()
                    If IsNumeric(tmp(tmp.Length - 1)) Then
                        If InStr(tmp, "h") = 0 Then
                            tmp = tmp & " h"
                        End If
                    End If

                    If sortida.Length > 0 Then tmp = ", " & tmp
                End If
                sortida &= tmp
            End If
        End If
        Return (sortida)
    End Function



    Public Shared Function nomTramit(ByVal nom As String, ByVal textAlerta As String, ByVal sdataInici As String, ByVal sdataFi As String, ByVal esSSL As Boolean) As String


        If DateDiff("s", Now, CDate(sdataInici)) <= 0 And DateDiff("s", Now, CDate(sdataFi)) >= 0 Then
            nom &= "&nbsp;<img src=""http" & IIf(esSSL, "s", "") & "://www.l-h.cat/img/ico_tramit_estat.gif"" class=""border0 paddingDretaEsquerra5"" alt=""Av&iacute;s""/><span class=""arial vermell "">" & textAlerta & "</span>"
        End If
        Return (nom)
    End Function 'nomTramit
#End Region






#Region "RSS"
    Public Shared Function RSSInici(ByVal writer As XmlTextWriter, ByVal sTitle As String, ByVal sDescription As String) As XmlTextWriter
        writer.WriteStartDocument()
        writer.WriteStartElement("rss")
        writer.WriteAttributeString("version", "2.0")
        writer.WriteStartElement("channel")
        writer.WriteElementString("title", sTitle)
        writer.WriteElementString("link", "http://www.l-h.cat/")
        writer.WriteElementString("description", sDescription)
        writer.WriteElementString("copyright", "Copyright 2006")
        writer.WriteElementString("generator", "XmlTextWriter")
        Return writer
    End Function

    Public Shared Function RSSItem(ByVal writer As XmlTextWriter, ByVal sItemTitle As String, ByVal sItemLink As String, ByVal sItemDescription As String, ByVal dItemPubDate As DateTime) As XmlTextWriter
        writer.WriteStartElement("item")
        writer.WriteElementString("title", sItemTitle)
        writer.WriteElementString("link", sItemLink)
        writer.WriteElementString("description", sItemDescription)
        writer.WriteElementString("pubDate", dItemPubDate.ToString("r"))
        writer.WriteEndElement()
        Return writer
    End Function

    Public Shared Function RSSFi(ByVal writer As XmlTextWriter) As XmlTextWriter
        writer.WriteEndElement()
        writer.WriteEndElement()
        writer.WriteEndDocument()
        Return writer
    End Function
#End Region











#Region "GAIA2"
    'N
    Public Shared Function maquetarHTML(ByVal objConn As OleDbConnection, ByVal estructura As String, ByVal rel As clsRelacio, ByRef fDesti As String, ByRef urlDesti As String, ByRef nrocontingut As Integer, ByRef llistaDocuments As String(), ByRef publicar As Integer, ByVal idioma As Integer, ByVal relIni As clsRelacio, ByVal dataSimulacio As DateTime, ByVal codiUsuari As Integer, ByRef tagsMeta As String) As String
        Dim html As String = ""
        Dim arrayEstructura As String()
        Dim cont As Integer = 1, idCel As Integer
        Dim strIdCel As String = "", htmlTmp As String = "", strsql As String
        Dim DS As DataSet, DS2 As DataSet
        Dim dbRow As DataRow
        Dim relInf As New clsRelacio()
        Dim plantilla As New clsPlantilla2
        DS = New DataSet()
        'Si no tinc estructura, és la primera iteració i la busco dins de la fulla/node/arbre web corresponent      
        If estructura = "" Then

            GAIA2.bdr(Nothing, "SELECT WEBDSEST FROM METLWEB2 WITH(NOLOCK) WHERE WEBINNOD=" & rel.infil & " AND WEBINIDI=" & idioma, DS)
            If DS.Tables(0).Rows.Count > 0 Then
                estructura = DS.Tables(0).Rows(0)("WEBDSEST")
            End If
        End If


        'Recorro cada cel·la i la represento
        arrayEstructura = Split(estructura, "id='d")
        'salto el primer element i per cadascun faig el tractament
        For i As Integer = 1 To arrayEstructura.Length - 1
            idCel = arrayEstructura(i).Substring(0, arrayEstructura(i).IndexOf("'"))
            clsPermisos.afegirElement(idCel, strIdCel)
        Next i


        'represento cel·les de tipus arbre web, Node WEB i Fulla web
        GAIA2.bdr(objConn, "SELECT  RELINCOD, RELCDEST, isNUll(RELINPLA,0) as RELINPLA, RELINFIL, NODCDTIP FROM METLREL WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE RELCDRSU=" & rel.incod & " AND RELCDEST IN (" & strIdCel & ") AND NODCDTIP IN (8,9,10) AND RELCDSIT<95 AND RELINFIL=NODINNOD", DS)
        For Each dbRow In DS.Tables(0).Rows
            relInf.bdget(objConn, dbRow("RELINCOD"))
            idCel = dbRow("RELCDEST")
            'Si és una fulla web o un node web, busco l'estructura
            Select Case dbRow("NODCDTIP")
                Case 8 'arbre web
                    'Tinc un node web, llegeixo l'estructura
                    DS2 = New DataSet()
                    GAIA2.bdr(objConn, "SELECT AWEDSEST FROM METLAWE2 WHERE AWEINNOD=" & dbRow("RELINFIL"), DS2)
                    If DS2.Tables(0).Rows.Count > 0 Then
                        estructura = DS2.Tables(0).Rows(0)("AWEDSEST")
                    End If
                Case 9 'node web
                    'Tinc un node web, llegeixo l'estructura
                    DS2 = New DataSet()
                    GAIA2.bdr(objConn, "SELECT NWEDSEST FROM METLNWE2 WHERE NWEINNOD=" & dbRow("RELINFIL"), DS2)
                    If DS2.Tables(0).Rows.Count > 0 Then
                        estructura = DS2.Tables(0).Rows(0)("NWEDSEST")
                    End If
                Case 10 'fulla web
                    DS2 = New DataSet()
                    GAIA2.bdr(objConn, "SELECT WEBDSEST FROM METLWEB2 WHERE WEBINNOD=" & dbRow("RELINFIL"), DS2)
                    If DS2.Tables(0).Rows.Count > 0 Then
                        estructura = DS2.Tables(0).Rows(0)("WEBDSEST")
                    End If
            End Select
            clsPermisos.eliminarElement(idCel, strIdCel)
            'En html tinc el contingut de la cel·la i ja el puc inserir 
            'LCW abans
            htmlTmp = maquetarHTML(objConn, estructura, relInf, fDesti, urlDesti, nrocontingut, llistaDocuments, publicar, idioma, rel, dataSimulacio, codiUsuari, tagsMeta)
            'LCW despres
            arrayEstructura(dbRow("RELCDEST")).Insert(arrayEstructura(dbRow("RELCDEST")).IndexOf(">"), htmlTmp)
        Next dbRow





        '*******************************************************************
        ' POSO LA LLIBRERIA DE CODI WEB 
        '*******************************************************************                
        ' If plantilla.arrayLCW.Length > idCel Then
        'If (plantilla.arrayLCW(idCel).Trim().Length > 0) Then
        'For Each item In plantilla.arrayLCW(idCel).Split("|")
        'htmlTmp &= GAIA.trobaCodiWeb2(objConn, plantilla, item, idioma, rel, relIni, 1, idCel, "", 0, codiUsuari, "", tagsMeta)
        'Si el codi web s'ha de mostrar de totes formes, ja l'afegeixo                                                         
        '   Next item
        '  End If
        '   End If


        'Represento les cel·les que tinguin contingut associat
        For Each idCel In strIdCel.Split(",")
            strsql = "select RELCDEST,isnull(REIINCOD, 0) AS REIINCOD, isnull(REIDTCAD, '1/1/1900') AS REIDTCAD, isnull(REIDTPUB, '1/1/1900') AS REIDTPUB, NODCDTIP FROM METLREL WITH(NOLOCK) INNER JOIN METLNOD WITH(NOLOCK) ON RELINFIL=NODINNOD LEFT OUTER JOIN METLREI WITH(NOLOCK) ON RELINCOD=REIINCOD AND REIINIDI=1 WHERE RELCDRSU=" & rel.incod & " AND  RELCDSIT<95 AND RELCDEST=" & idCel
            GAIA2.bdr(objConn, strsql, DS)
            If DS.Tables(0).Rows.Count > 0 Then 'Tinc contingut que penja d'aquesta cel·la de l'estructura  
                For Each dbRow In DS.Tables(0).Rows
                    htmlTmp = ""
                    'Falta: contar número de contingut màxim
                    If dbRow("REIDTPUB") < Now And (dbRow("REIDTCAD") > Now() Or dbRow("REIDTCAD") = "1/1/1900") Then



                        relInf.bdget(objConn, dbRow("REIINCOD"))
                        plantilla.bdget(Nothing, relInf.inpla)
                        htmlTmp = GAIA2.maquetarPlantilla(Nothing, plantilla, relInf, 1, rel, CDate(Now), 49730, llistaDocuments, urlDesti, fDesti, 0, "N", tagsMeta)


                        estructura = estructura.Replace("id='d" & dbRow("RELCDEST") & "'>", ">" & htmlTmp)

                        '      arrayEstructura(dbRow("RELCDEST")).Insert(arrayEstructura(dbRow("RELCDEST")).IndexOf(">"), arrayEstructura(dbRow("RELCDEST")))
                    End If
                Next dbRow

            End If

        Next idCel


        Return estructura

    End Function




    Public Shared Function maquetarPlantilla(ByVal objConn As OleDbConnection, ByVal plantilla As clsPlantilla2, ByVal rel As clsRelacio, ByVal idioma As Integer, ByVal relIni As clsRelacio, ByVal dataSimulacio As DateTime, ByVal codiUsuari As Integer, ByRef llistaDocuments As String(), ByRef urlDesti As String, ByVal fdesti As String, ByVal publicar As Integer, ByVal esEML As String, ByVal tagsMeta As String) As String
        Dim arrayEstructura As String()
        Dim cont As Integer = 1
        Dim strIdCel As String = "", htmlTmp As String = "", html As String = ""

        'Recorro cada cel·la i la represento
        arrayEstructura = Split(plantilla.est, "id='d")
        'salto el primer element i per cadascun faig el tractament
        html = arrayEstructura(0)
        For i As Integer = 1 To arrayEstructura.Length - 1
            ' idCel = arrayEstructura(i).Substring(0, arrayEstructura(i).IndexOf("'"))

            htmlTmp = GAIA2.maquetarContingut(objConn, plantilla, rel, i - 1, 0, llistaDocuments, urlDesti, idioma, relIni, 0, fdesti, publicar, codiUsuari, dataSimulacio, esEML, tagsMeta, 0)

            html &= ">" & htmlTmp & arrayEstructura(i).Substring(InStr(arrayEstructura(i), ">"))

        Next i




        Return html

    End Function


    '***************************************************************************************************
    '   Funció: GAIA.maquetarContingut
    '   Acció: 
    '           
    '***************************************************************************************************
    'N
    Public Shared Function maquetarContingut(ByVal objConn As OleDbConnection, ByRef plantilla As clsPlantilla2, ByVal rel As clsRelacio, ByVal idCel As Integer, ByVal width As String, ByRef llistaDocuments As String(), ByRef urlDesti As String, ByVal idioma As Integer, ByVal relIni As clsRelacio, ByVal estat As Integer, ByVal fdesti As String, ByVal publicar As Integer, ByVal codiUsuari As Integer, ByVal dataSimulacio As String, ByVal esEML As String, ByVal tagsMeta As String, ByVal heretaPropietatsWeb As Integer) As String
        Dim html As String = ""
        If rel.tipdsdes <> "node web" And rel.tipdsdes <> "fulla web" And rel.tipdsdes <> "arbre web" Then
            If plantilla.innod = 0 Then
                plantilla.bdget(objConn, plantillaPerDefecte(objConn, rel, idioma))
            End If
            If plantilla.innod > 0 Then

                Dim strvoid As String = ""
                Dim relPare As New clsRelacio
                Dim lcwANT As String = ""
                Dim lcwPOST As String = ""

                Dim camp, prefixCamp, esImatge, taula, clau, sqlTamany, strContingut, sqlTextAlternatiu, textalternatiu, strSQL, campFitxer, campTitolContingut, strSQLWHERE, strSQLWHEREIdioma As String
                Dim strSQLOrdre As String = "", enllaç As String = ""
                Dim pospunt As Integer
                Dim DS, ds2, ds3 As DataSet
                Dim dbRow, dbrow2, dbrow3 As DataRow
                Dim cont As Integer

                Dim idiomaDiferent As Integer = False
                Dim forçarCodiAnt As Integer
                Dim forçarCodiPost As Integer
                Dim celdaFormateada As String
                Dim nomFitxerPublicat As String = ""
                Dim tancaEnllaç As Integer
                Dim fitxerAutoLink As String
                Dim codiRelacio, codiRelacioInicial As Integer
                Dim dataIni, dataFi As DateTime
                Dim campUpdate As String = ""
                Dim target As Integer = 0
                Dim relarxiu As New clsRelacio
                Dim relEnllaç As New clsRelacio
                Dim codiNodeTMP As Integer = 0
                codiRelacio = rel.incod
                codiRelacioInicial = relIni.incod
                fitxerAutoLink = ""
                Dim desfaseHoritzontal As Integer = 0
                Dim botoMesImatge As String = ""
                Dim sqlTOP As String = ""
                sqlTextAlternatiu = ""
                Dim oCodParam As New lhCodParam.lhCodParam
                Dim plantillaTMP As New clsPlantilla2
                campFitxer = ""
                strContingut = ""
                esImatge = "0"
                Dim parametrePL As String = ""
                Dim item As String = ""

                Dim titolContingut As String = ""
                Dim altImatge As String = ""
                Dim posaWidthGAIA As Integer = 1
                Dim posafloatGAIA As Boolean = True
                Dim estilsCSSSenseFons As String = ""
                Dim strURL As String = ""
                'Dim a as new system.web.htpputility
                tancaEnllaç = False ' si true, tancarem el </a> al final de maquetarContingut. Això ho utilitzaré per posar tot el contingut dins d'un ellaç (pels casos d'autolinks).
                celdaFormateada = ""
                cont = 0
                DS = New DataSet()
                ds2 = New DataSet()
                ds3 = New DataSet()
                strSQLWHERE = ""
                Dim codiRelacioArxiuPerVisorImatges As Integer = 0
                'Dim pathDocumentsXDefecte as string = "/aplics/GAIA/documents/docs/"
                Dim pathDocumentsXDefecte As String
                pathDocumentsXDefecte = "/gdocs/"


                If idioma > 1 Then
                    If plantilla.arrayAAL.Length > idCel Then
                        'Trec la referència a la pàgina en castellà i la poso de nou per garantir que no poso _2_2.aspx
                        plantilla.arrayAAL(idCel) = plantilla.arrayAAL(idCel).Replace("_" & idioma & ".aspx", ".aspx")
                        plantilla.arrayAAL(idCel) = plantilla.arrayAAL(idCel).Replace(".aspx", "_" & idioma & ".aspx")
                    End If
                End If

                If plantilla.arrayDSNUM.Length > idCel Then
                    If (plantilla.arrayDSNUM(idCel).Trim().Length > 0) Then
                        If plantilla.arrayDSNUM(idCel) <> "0" Then
                            sqlTOP = " TOP " + plantilla.arrayDSNUM(idCel).ToString() + " "
                        End If
                    End If
                End If

                '*******************************************************************
                ' POSO LA LLIBRERIA DE CODI WEB 
                '*******************************************************************                
                If plantilla.arrayLCW.Length > idCel Then
                    If (plantilla.arrayLCW(idCel).Trim().Length > 0) Then

                        For Each item In plantilla.arrayLCW(idCel).Split("|")
                            If width = "" Then
                                width = "0"
                            End If
                            lcwANT = lcwANT + GAIA2.trobaCodiWeb2(objConn, plantilla, item, idioma, rel, relIni, forçarCodiAnt, idCel, "", width, codiUsuari, "", tagsMeta)
                        Next item
                    End If
                End If

                '*******************************************************************
                'Preparo la select per trobar el contingut
                '*******************************************************************                                
                If plantilla.arrayCampsPlantilla.Length > idCel Then
                    'Tracto el camp a reprentar si és d'alguna taula de GAIA. En cas contrari (Imatge/document) ho tracto apart
                    If (plantilla.arrayCampsPlantilla(idCel).IndexOf(".") >= 0) Then

                        pospunt = plantilla.arrayCampsPlantilla(idCel).IndexOf(".") + 1
                        camp = plantilla.arrayCampsPlantilla(idCel).Substring(pospunt)
                        prefixCamp = camp.Substring(0, 3)

                        enllaç = plantilla.arrayEnllaços(idCel).Substring(plantilla.arrayEnllaços(idCel).IndexOf(".") + 1)
                        textalternatiu = plantilla.arrayALT(idCel).Substring(plantilla.arrayALT(idCel).IndexOf(".") + 1).Replace("""", "´")

                        If camp = "DOCDSFIT" Or enllaç = "DOCUMENT" Or enllaç = "IMATGE" Then
                            campFitxer = "DOCDSFIT"
                        End If
                        esImatge = plantilla.arrayEsImatge(idCel)
                        taula = plantilla.arrayCampsPlantilla(idCel).Substring(0, pospunt - 1)

                        'afegeixo el titol del contingut a la select per si ho necessito (de moment només per fer un "alt" d'un enllaç

                        Select Case taula
                            Case "METLDOC"
                                If enllaç = "AUTO-ENLLAÇ" Then
                                    campFitxer = "DOCDSFIT"
                                    enllaç = "DOCDSFIT"
                                End If
                                campUpdate = ",DOCWNUPD, DOCDSLNK, TDODSNOM "
                                campTitolContingut = ",  DOCDSTIT as titolContingut, DOCINIDI as idioma,DOCWNHOR "
                                taula += " WITH(NOLOCK), METLTDO WITH(NOLOCK)"
                            Case "METLNOT"
                                campTitolContingut = ",  NOTDSTIT as titolContingut, NOTINIDI as idioma, NOTDSLNK AS link "
                            Case "METLINF"
                                campTitolContingut = ",  INFDSTIT as titolContingut, INFINIDI as idioma "

                            Case "METLAGD"
                                campTitolContingut = ",  AGDDSTIT as titolContingut, AGDINIDI as idioma "
                            Case "METLCON"
                                campTitolContingut = ",  CONDSDES as titolContingut, CONINIDI as idioma "
                            Case "METLFPR"
                                campTitolContingut = ", FPRDSNOM as titolContingut, FPRINIDI as idioma "
                            Case "METLLNK"
                                campTitolContingut = ", LNKDSTXT as titolContingut, LNKINIDI as idioma, RELINCOD "
                                taula &= " WITH(NOLOCK), METLREL WITH(NOLOCK) "
                                If enllaç = "DOCUMENT" Or enllaç = "IMATGE" Then
                                    taula &= ", METLDOC WITH(NOLOCK), METLTDO WITH(NOLOCK) "
                                    campUpdate = " ,DOCINNOD "
                                    If enllaç = "DOCUMENT" Then
                                        strSQLWHERE = "  AND  RELINPAR=LNKINNOD AND RELINFIL=DOCINNOD AND RELCDSIT=97 "
                                    Else
                                        strSQLWHERE = " AND RELINPAR=LNKINNOD AND RELINFIL=DOCINNOD AND RELCDSIT=96 "
                                    End If
                                    enllaç = "DOCDSFIT"
                                Else
                                    strSQLWHERE &= " AND RELINFIL=LNKINNOD "
                                End If
                            Case Else
                                campTitolContingut = ",  '' as titolContingut, 1  as idioma "
                        End Select

                        clau = prefixCamp + "INNOD"
                        ' Si és una imatge, llavors he de buscar el tamany vertical i l'horitzontal, per saber si he de posar la imatge en miniatura o la gran.
                        If esImatge = "1" And prefixCamp = "DOC" Then
                            sqlTamany = " , DOCWNHOR as 'tamanyH', DOCWNVER as 'tamanyV' "
                        Else
                            sqlTamany = "  "
                        End If
                        If textalternatiu.Trim().Length > 0 Then
                            sqlTextAlternatiu = " ," + textalternatiu
                        End If



                        If InStr(enllaç, "RELDSFIT") > 0 Then
                            taula += ", METLREL WITH(NOLOCK)"
                        Else
                            If sqlTOP.Length > 0 Then
                                taula += ", METLREL WITH(NOLOCK) "
                                strSQLWHERE = " AND " & clau & "= RELINFIL"
                            End If


                        End If


                        If enllaç.Trim().Length > 0 And enllaç <> "AUTO-ENLLAÇ" Then
                            strSQL = "SELECT " + sqlTOP + clau + "," + camp + campTitolContingut + campUpdate + "," + enllaç + sqlTamany + sqlTextAlternatiu + "  FROM " & taula
                            strSQLWHERE = " WHERE " & clau & "=" & rel.infil & strSQLWHERE
                            strSQLWHEREIdioma = " AND " & prefixCamp & "INIDI=" & idioma
                        Else
                            strSQL = "SELECT " + sqlTOP + clau + "," + camp + campTitolContingut + campUpdate + sqlTamany + sqlTextAlternatiu + " FROM " + taula
                            strSQLWHERE = " WHERE " & clau & "=" & rel.infil & strSQLWHERE
                            strSQLWHEREIdioma = " AND " & prefixCamp & "INIDI=" & idioma
                        End If

                        If InStr(enllaç, "RELDSFIT") > 0 Then
                            strSQLWHERE += " AND RELINFIL=" & prefixCamp & "INNOD AND RELINCOD=" & rel.incod
                        End If

                        'Afegeixo la comprobació de la data
                        'si és una simulació no comprobo que la data de publicació sigui la correcte
                        If CDate(Day(dataSimulacio) & "/" & Month(dataSimulacio) & "/" & Year(dataSimulacio)) < Now Then
                            strSQLWHERE += " AND (" + prefixCamp + "DTCAD>'" + dataSimulacio.ToString() + "' OR " + prefixCamp + "DTCAD='" + CDate("01/01/1900").ToString() + "')"
                        Else
                            strSQLWHERE += " AND  " + prefixCamp + "DTPUB<='" + dataSimulacio.ToString() + "' AND (" + prefixCamp + "DTCAD>'" + dataSimulacio.ToString() + "' OR " + prefixCamp + "DTCAD='" + CDate("01/01/1900").ToString() + "')"
                        End If
                        'si és un doc lligo amb la taula de tipus de documents
                        If InStr(taula, "METLDOC") > 0 Then
                            strSQLWHERE += " AND DOCINTDO=TDOCDTDO "
                        End If

                        If InStr(enllaç, "RELDSFIT") > 0 Then
                            strSQLOrdre = " ORDER BY RELINORD "
                        End If
                        If (sqlTOP.Length > 0) Then
                            strSQLOrdre = " ORDER BY RELCDORD "
                        End If


                        GAIA2.bdr(objConn, strSQL & strSQLWHERE & strSQLWHEREIdioma & strSQLOrdre, DS)

                        'Si no trobo el contingut en l'idioma dessijat ho busco en qualsevol altre idioma, ordenat per ordre d'idioma: català, castellà, anglès, i no importa la caducitat      
                        If DS.Tables(0).Rows.Count = 0 Then
                            strSQLOrdre = " ORDER BY " + prefixCamp + "INIDI"
                            If (sqlTOP.Length > 0) Then
                                strSQLOrdre += " ,RELCDORD "
                            End If
                            GAIA2.bdr(objConn, strSQL & strSQLWHERE & strSQLOrdre, DS)
                            idiomaDiferent = 1
                        End If

                        'TINC CONTINGUT                                                     
                        If DS.Tables(0).Rows.Count > 0 Then
                            titolContingut = DS.Tables(0).Rows(0)("titolContingut")


                            If titolContingut = "" Then
                                titolContingut = GAIA2.obtenirTitolContingut(objConn, relIni, idioma)
                            End If

                            dataIni = CDate("01/01/1900")
                            dataFi = CDate("1/1/2050")
                            'Si no és un contingut codificat tinc en compte les dates de la relació
                            If Not rel.cdher.StartsWith("_57135") Then
                                ds2 = New DataSet()
                                GAIA2.bdr(objConn, "SELECT REIDTPUB,REIDTCAD FROM METLREI WITH(NOLOCK) WHERE REIINCOD=" & rel.incod, ds2)
                                If ds2.Tables(0).Rows.Count > 0 Then


                                    dbrow2 = ds2.Tables(0).Rows(0)
                                    dataIni = dbrow2("REIDTPUB")
                                    If dbrow2("REIDTCAD") = CDate("01/01/1900") Then
                                        dataFi = CDate("1/1/2050")
                                    Else
                                        dataFi = dbrow2("REIDTCAD")
                                    End If
                                End If
                                ds2.Dispose()
                            End If
                            If dataIni <= CDate(dataSimulacio) And dataFi > CDate(dataSimulacio) Then
                                dbRow = DS.Tables(0).Rows(0)


                                If rel.tipdsdes = "fulla document" And campFitxer = "DOCDSFIT" Then
                                    'Si no estaba a la llista de documents, ho coloco
                                    Dim trobat As Integer = False

                                    If Not llistaDocuments Is Nothing Then
                                        For Each item In llistaDocuments
                                            If item = dbRow(campFitxer) Then
                                                trobat = True
                                                Exit For
                                            End If
                                        Next
                                    End If
                                    If Not trobat Then
                                        'Abans d'afegir el document a la llista de documents que es publicaran, comprobo que no s'hagués publicat ja
                                        '                                   IF InStr(rel.dsfit,dbROW("DOCDSFIT"))>0 THEN
                                        If dbRow("DOCWNUPD") = 1 Then
                                            If llistaDocuments Is Nothing Then
                                                ReDim llistaDocuments(0)
                                            Else
                                                ReDim Preserve llistaDocuments(llistaDocuments.Length)
                                            End If
                                            llistaDocuments(llistaDocuments.Length - 1) = codiRelacio.ToString() + "_" + dbRow("DOCDSFIT")
                                        End If
                                    End If
                                End If 'rel.tipdsdes="fulla document" AND campFitxer="DOCDSFIT" 

                                If esImatge = "1" And prefixCamp = "DOC" Then

                                    If publicar Then
                                        If Not (urlDesti Is Nothing) Then
                                            Dim strTmp As String
                                            strTmp = urlDesti.Replace("http://", "")
                                            'IF strTMP.length=0 THEN

                                            If InStr(rel.dsfit, ".swf") > 0 Then
                                                strTmp = strTmp.Substring(0, InStr(strTmp, "/") - 1)
                                                strContingut = "/gDocs/" + dbRow(camp)
                                            Else
                                                nomFitxerPublicat = rel.dsfit

                                                'faig la selecció de la mida de la imatge a visualitzar
                                                Dim mida As String = ""
                                                Dim fitxer As String = ""
                                                If width = 0 Then  'el cas de width=0 l'utilitzem només per donar-li mida màxima disponible a la imatge mitjançant el class
                                                    mida = "&t=imatgeGran"
                                                    fitxer = dbRow("DOCDSFIT")
                                                Else

                                                    If width <= 100 Then
                                                        mida = "&t=P100"
                                                        fitxer = dbRow("DOCDSFIT").replace(".", "P100.")

                                                    Else
                                                        If width <= 700 Then
                                                            mida = "&t=P"
                                                            fitxer = dbRow("DOCDSFIT").replace(".", "P.")
                                                        Else
                                                            fitxer = dbRow("DOCDSFIT")
                                                            mida = "&t=imatgeGran"
                                                        End If
                                                    End If
                                                End If

                                                strContingut = "/utils/obreFitxer.ashx?" & HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & rel.infil & "&codiIdioma=" & idioma & mida & "&f=" & fitxer))



                                            End If
                                        Else
                                            strContingut = pathDocumentsXDefecte + dbRow(camp)
                                        End If
                                    Else
                                        strContingut = "/utils/obreFitxer.ashx?" & HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & rel.infil & "&codiIdioma=" & idioma))

                                    End If
                                Else 'no és imatge                  

                                    If prefixCamp = "LNK" And camp.Substring(0, 3) = "LNK" Then
                                        If campFitxer = "DOCDSFIT" Then
                                            If campFitxer = "DOCDSFIT" Then
                                                Dim relDoc As New clsRelacio
                                                relDoc.bdget(objConn, -1, dbRow("DOCINNOD"))
                                                enllaç = obtenirEnllacContingut(objConn, relDoc, idioma)
                                                Select Case plantilla.arrayEnllaços(idCel).Substring(plantilla.arrayEnllaços(idCel).IndexOf(".") + 1)
                                                    Case "IMATGE"
                                                        'faig el tractament de la imatge assocciada a un enllaç que apunta a imatges amb plantilla secundària per representar-lo
                                                        If plantilla.arrayPLTDSPLT(idCel).Length = 0 Then
                                                            GAIA2.f_logError(objConn, "maquetarContingut", "", "S'ha intentat representar una imatge o document dins d'un contingut sense plantilla secund&agrave;ria per representar-lo")
                                                        End If
                                                        codiRelacioArxiuPerVisorImatges = dbRow("RELINCOD")


                                                        enllaç = obtenirEnllacContingut(objConn, relDoc, idioma)
                                                    Case "DOCUMENT"

                                                        'ja ho he tractat
                                                End Select
                                            End If
                                        Else
                                            If enllaç.Trim.Length = 0 Then
                                                enllaç = ""
                                            Else
                                                enllaç = GAIA2.obtenirEnllaçSimple(objConn, dbRow("LNKINNOD"), dbRow("idioma"), target, urlDesti)
                                            End If
                                        End If
                                    End If

                                    If camp.Trim() <> "" Then
                                        'Només htmlencode si no estava fet anteriorment. Això es fa per que hi ha texte que ja ve codificat dels editors wysisyg

                                        If plantilla.arrayDSNUM(idCel) > 0 Then
                                            strContingut = GAIA2.tallaText(dbRow(camp), plantilla.arrayDSNUM(idCel)) & "..."
                                        Else
                                            If Not IsDBNull(dbRow(camp)) Then
                                                strContingut = dbRow(camp)
                                            End If
                                        End If

                                        If strContingut.Length > 0 Then
                                            strContingut = strContingut.Replace("<p>", "").Replace("</p>", "").Replace("&nbsp;&nbsp;", " ")
                                        End If

                                    End If
                                End If

                                If textalternatiu.Trim().Length > 0 Then
                                    textalternatiu = dbRow(textalternatiu).replace("""", "´")
                                End If
                                '********************************************************************************************
                                ' formatejo el contingut. Només ho faig si està en un idioma diferent si és un document.
                                '********************************************************************************************
                                If (rel.tipdsdes = "fulla document" And idiomaDiferent) Or (Not idiomaDiferent) Then
                                    If (enllaç + "").Trim().Length > 0 Then
                                        If rel.tipdsdes <> "fulla link" Then ' Si es una fulla link ja tinc l'enllaç que he trobat més amunt
                                            If enllaç = "DOCDSFIT" Then 'L'enlla que vull representar és un fitxer
                                                If dbRow("DOCDSLNK").length > 0 Then
                                                    enllaç = dbRow("DOCDSLNK")

                                                Else
                                                    If InStr(UCase(dbRow("TDODSNOM")), "IMAGE") > 0 And rel.cdsit <> 97 Then
                                                        enllaç = "http://www.l-h.cat/utils/visorImatges.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & rel.infil & "&codiIdioma=" + idioma.ToString()))
                                                        If esEML = "S" Then
                                                            botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" style=""float:right; position:relative; top:-18px; right:3px; border:0"" class=""noImprimible"" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"
                                                        Else
                                                            botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" class=""simbolmes noImprimible"" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"
                                                        End If
                                                    Else
                                                        enllaç = "/utils/obreFitxer.ashx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbRow("DOCINNOD") & "&codiIdioma=" + idioma.ToString() & "&f=" & dbRow("DOCDSFIT")))
                                                    End If
                                                End If
                                            Else 'l'enllaç que vull representar és un contingut de tipus noticia/tramit/directori/agenda
                                                If enllaç <> "AUTO-ENLLAÇ" Then
                                                    enllaç = dbRow(enllaç)
                                                Else



                                                    Select Case rel.tipdsdes
                                                        Case "fulla noticia"


                                                            If plantilla.arrayAAL(idCel).Trim.Length > 0 Then
                                                                enllaç = plantilla.arrayAAL(idCel).Trim
                                                            Else
                                                                enllaç = "/detallNoticia.aspx"
                                                            End If
                                                            enllaç &= "?" & idioma & HttpUtility.UrlEncode(oCodParam.encriptar("cr=" + rel.incod.ToString() + "&amp;pl=168578&amp;id=" & idioma & "&amp;us=0&amp;width=" + width.ToString()))

                                                        Case "fulla info"
                                                            If plantilla.arrayAAL(idCel).Trim.Length > 0 Then
                                                                enllaç = plantilla.arrayAAL(idCel).Trim
                                                            Else
                                                                enllaç = "/detallInformacio.aspx"
                                                            End If
                                                            '180342
                                                            enllaç &= "?" & idioma & HttpUtility.UrlEncode(oCodParam.encriptar("cr=" + rel.incod.ToString() + "&amp;pl=207935&amp;id=" & idioma & "&amp;us=0&amp;width=" + width.ToString()))
                                                        Case "fulla tramit"
                                                            If plantilla.arrayAAL(idCel).Trim.Length > 0 Then
                                                                enllaç = plantilla.arrayAAL(idCel).Trim
                                                            Else
                                                                enllaç = "tramits.aspx"
                                                            End If

                                                            enllaç &= "?" & idioma & HttpUtility.UrlEncode(oCodParam.encriptar("cr=" + rel.incod.ToString() + "&amp;pl=150861&amp;id=" & idioma & "&amp;us=0&amp;width=" + width.ToString()))
                                                        Case "fulla directori"
                                                            enllaç = "/directori.aspx"
                                                        Case "fulla agenda"
                                                            If plantilla.arrayAAL(idCel).Trim.Length > 0 Then
                                                                enllaç = plantilla.arrayAAL(idCel).Trim
                                                                enllaç &= "?" & idioma & oCodParam.encriptar("cr=" & rel.incod & "&id=" & idioma & "&pl=" & plantilla.arrayPAL(idCel).Trim() & "&us=0&width=" & width)
                                                            Else
                                                                enllaç = "/agenda/detallAgenda.aspx"
                                                                enllaç &= "?" & idioma & oCodParam.encriptar("cr=" & rel.incod & "&id=" & idioma & "&pl=168660&us=0&width=" & width)
                                                            End If

                                                        Case "fulla document"
                                                            If dbRow("DOCDSLNK").length > 0 Then
                                                                enllaç = dbRow("DOCDSLNK")
                                                            Else
                                                                'si és una imatge poso un enllaç al visor d'imatges, en cas contrari l'enllaç serà al visor de document.                
                                                                enllaç = "/utils/obreFitxer.ashx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbRow("DOCINNOD") & "&codiIdioma=" + idioma.ToString() & "&f=" & dbRow("DOCDSFIT")))
                                                            End If
                                                    End Select
                                                End If
                                            End If
                                        End If

                                        If rel.tipintip = "49" Then
                                            target = GAIA2.obtenirTarget(objConn, rel)
                                        Else
                                            Try
                                                target = plantilla.arrayALF(idCel)
                                            Catch
                                                target = 0
                                            End Try
                                        End If
                                        If codiRelacioArxiuPerVisorImatges = 0 Then


                                            celdaFormateada += GAIA2.posaFormat2(objConn, strContingut, plantilla.arrayCSSPlantilla(idCel), enllaç, esImatge, width, 0, 0, estat, textalternatiu, 1, 0, target, rel, relIni, idioma, esEML, botoMesImatge, titolContingut, plantilla, 0, "")
                                        End If

                                    Else
                                        'el node pare (relIni) és un enllaç i el node actual (rel) és un document/imatge. 
                                        If rel.tipintip = "49" Then
                                            target = GAIA2.obtenirTarget(objConn, rel)
                                        Else
                                            Try
                                                target = plantilla.arrayALF(idCel)
                                            Catch
                                                target = 0
                                            End Try

                                        End If
                                        celdaFormateada += GAIA2.posaFormat2(objConn, strContingut, plantilla.arrayCSSPlantilla(idCel), "", esImatge, width, 0, 0, estat, textalternatiu, 1, 0, target, rel, relIni, idioma, esEML, botoMesImatge, titolContingut, plantilla, 0, "")
                                    End If
                                End If 'comprobació de l'idioma i del tipus de node
                            End If
                        End If 'TINC CONTINGUT

                    Else '(plantilla.arrayCampsPlantilla(idCel).IndexOf(".")=0 --> IMATGE o DOCUMENT


                        'Tracto els casos especials "IMATGE" (codi 96) i "DOCUMENT" (codi 97)
                        If plantilla.arrayCampsPlantilla(idCel) = "IMATGE" Or plantilla.arrayCampsPlantilla(idCel) = "DOCUMENT" Then

                            If plantilla.arrayPLTDSPLT(idCel).Length = 0 Then
                                GAIA2.f_logError(objConn, "maquetarContingut", "", "S'ha intentat representar una imatge o document dins d'un contingut sense plantilla secund&agrave;ria per representar-lo")
                            Else

                                plantillaTMP.bdget(objConn, plantilla.arrayPLTDSPLT(idCel))
                                If plantillaTMP.innod > 0 Then
                                    If plantilla.arrayCampsPlantilla(idCel) = "IMATGE" Then
                                        GAIA2.bdr(objConn, "SELECT " & sqlTOP & " RELINFIL,RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELINCOD IN (SELECT MIN(RELINCOD) FROM METLREL WHERE RELINPAR=" & rel.infil & " AND RELCDSIT=96 GROUP BY RELINFIL)  ORDER BY RELCDORD", ds3)
                                        ' GAIA.bdr(objConn, "SELECT " & sqlTOP & " RELINFIL,RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELCDRSU = " & rel.incod & " AND RELCDSIT=96", ds3)


                                    Else
                                        ' GAIA.bdr(objConn, "SELECT " & sqlTOP & " RELINFIL,RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELCDRSU = " & rel.incod & " AND RELCDSIT=97", ds3)
                                        'msv: si faig la query d'adalt no troba alguns continguts que no pengen correctament de totes les relacions. En concret es perdien al perfil del contractant
                                        GAIA2.bdr(objConn, "SELECT " & sqlTOP & " RELINFIL,RELINCOD FROM METLREL WITH(NOLOCK) WHERE RELINCOD IN (SELECT MIN(RELINCOD) FROM METLREL WHERE RELINPAR=" & rel.infil & " AND RELCDSIT=97 GROUP BY RELINFIL)  ORDER BY RELCDORD", ds3)





                                    End If
                                    codiNodeTMP = 0
                                    For Each dbrow3 In ds3.Tables(0).Rows
                                        If codiNodeTMP <> dbrow3("RELINFIL") Then
                                            codiNodeTMP = dbrow3("RELINFIL")
                                            relarxiu.bdget(objConn, dbrow3("RELINCOD"))
                                            'crido a maquetarContingut

                                            celdaFormateada = GAIA2.maquetarContingut(objConn, plantillaTMP, relarxiu, 0, 0, llistaDocuments, urlDesti, idioma, relIni, 0, fdesti, publicar, codiUsuari, dataSimulacio, esEML, tagsMeta, heretaPropietatsWeb)





                                            ' GAIA.dibuixaPreview(objConn, plantillaTMP, plantillaTMP.est, plantillaTMP.arrayAtr, plantillaTMP.hor, plantillaTMP.ver, plantillaTMP.tco, "", celdaFormateada, css, width, 0, "f", relarxiu, 0, fdesti, urlDesti, 1, llistaDocuments, publicar, idioma, plantillaTMP.css, rel, dataSimulacio, codiUsuari, 0, "", 0, plantillaTMP.flw, "", 0, estilbody, llistaCND, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, plantillaTMP.niv, plantilla.arrayEnllaços(idCel).Trim().Length > 0)
                                        End If
                                    Next dbrow3
                                Else
                                    GAIA2.f_logError(objConn, "maquetarContingut", "", "S'ha intentat representar una imatge o document dins d'un contingut sense plantilla secund&agrave;ria per representar-lo")
                                End If
                            End If

                            'Tracto l'enllaç. (pot ser que ja hagi representat la imatge d'un enllaç de tipus banner i ara he de posar l'enllaç)                    
                            'En cas de tenir un enllaç a una imatge, plantillaTMP ha de ser=0 per que si <> 0 ja s'hauria representat l'enllaç en la plantilla secundària
                            'podria ser una imatge que té com a enllaç un document, llavors si que s'ha de representar
                            If ((plantilla.arrayEnllaços(idCel) = "IMATGE" And plantillaTMP.innod = 0) Or (plantilla.arrayEnllaços(idCel) = "DOCUMENT")) And plantilla.arrayEnllaços(idCel) <> plantilla.arrayCampsPlantilla(idCel) Then
                                If plantilla.arrayEnllaços(idCel) = "IMATGE" Then

                                    'msv 4/1/11 -> Aquestes 4 sql serien + correctes  sense el RELINPAR=  i amb un RELCDRSU=" & rel.incod , però hi ha algun cas antic que una imatge o document que penja d'un contingut  no ho fa per a totes les relacions. 
                                    '       GAIA.bdR(objConn, "SELECT " + sqlTOP + " * FROM METLREL,METLDOC,METLTDO WHERE DOCINTDO=TDOCDTDO AND RELCDRSU=" + rel.incod.ToString() + " AND RELCDSIT=96 AND DOCINNOD=RELINFIL AND DOCINIDI=" + idioma.ToString() + " ORDER BY RELCDORD", ds3)
                                    GAIA2.bdr(objConn, "SELECT " + sqlTOP + " * FROM METLREL WITH(NOLOCK),METLDOC WITH(NOLOCK),METLTDO WITH(NOLOCK)WHERE DOCINTDO=TDOCDTDO AND RELINPAR=" & rel.infil & " AND RELCDSIT=96 AND DOCINNOD=RELINFIL AND DOCINIDI=" + idioma.ToString() + " ORDER BY RELCDORD", ds3)
                                Else
                                    GAIA2.bdr(objConn, "SELECT " + sqlTOP + " * FROM METLREL WITH(NOLOCK),METLDOC WITH(NOLOCK),METLTDO WITH(NOLOCK) WHERE DOCINTDO=TDOCDTDO AND RELCDRSU=" + rel.incod.ToString() + "  AND RELCDSIT=97 AND DOCINNOD=RELINFIL AND DOCINIDI=" + idioma.ToString() + " ORDER BY RELCDORD", ds3)
                                End If
                                If ds3.Tables(0).Rows.Count = 0 Then 'si no ho trobo amb l'idioma, busco a d'altres idiomes
                                    If plantilla.arrayEnllaços(idCel) = "IMATGE" Then
                                        ' GAIA.bdR(objConn, "SELECT " + sqlTOP + " * FROM METLREL,METLDOC,METLTDO WHERE DOCINTDO=TDOCDTDO AND  RELCDRSU=" + rel.incod.ToString() + "  AND RELCDSIT=96 AND DOCINNOD=RELINFIL  ORDER BY RELCDORD", ds3)
                                        GAIA2.bdr(objConn, "SELECT " + sqlTOP + " * FROM METLREL WITH(NOLOCK),METLDOC WITH(NOLOCK),METLTDO WITH(NOLOCK) WHERE DOCINTDO=TDOCDTDO AND  RELINPAR=" & rel.infil & "  AND RELCDSIT=96 AND DOCINNOD=RELINFIL  ORDER BY RELCDORD", ds3)
                                    Else
                                        GAIA2.bdr(objConn, "SELECT " + sqlTOP + " * FROM METLREL WITH(NOLOCK),METLDOC WITH(NOLOCK),METLTDO WITH(NOLOCK) WHERE DOCINTDO=TDOCDTDO AND RELCDRSU=" + rel.incod.ToString() + "  AND RELCDSIT=97 AND DOCINNOD=RELINFIL  ORDER BY RELCDORD", ds3)
                                    End If
                                End If
                                If ds3.Tables(0).Rows.Count > 0 Then
                                    dbrow3 = ds3.Tables(0).Rows(0)
                                    relEnllaç.bdget(objConn, dbrow3("RELINCOD"))
                                    '********************************************************
                                    ' afegeixo el document que "penja d'enllaç"
                                    '********************************************************                                                                       
                                    If llistaDocuments Is Nothing Then
                                        ReDim llistaDocuments(0)

                                    Else

                                        ReDim Preserve llistaDocuments(llistaDocuments.Length)
                                    End If
                                    If llistaDocuments.Length <> 0 Then
                                        llistaDocuments(llistaDocuments.Length - 1) = relEnllaç.incod.ToString() + "_" + dbrow3("DOCDSFIT")
                                    End If
                                    'He de posar l'enllaç abans de la imatge que hi ha a dins de celdaFormatejada.
                                    pospunt = InStr(celdaFormateada, "<img")
                                    If pospunt > 0 Then
                                        If Not dbrow3("DOCDSALT") Is Nothing Then
                                            If dbrow3("DOCDSALT").length > 0 Then
                                                textalternatiu = dbrow3("DOCDSALT").replace("""", "'")
                                            End If
                                        End If
                                        'Si el document és de tipus imatge poso l'enllaç al visor d'Imatges
                                        If InStr(UCase(dbrow3("TDODSNOM")), "IMAGE") > 0 Then

                                            strURL = "http://www.l-h.cat/utils/visorImatges.aspx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbrow3("DOCINNOD") & "&codiIdioma=" & idioma))
                                            If esEML = "S" Then
                                                botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" style=""float:right; position:relative; top:-18px; right:3px; border:0"" class=""noImprimible "" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"


                                            Else
                                                botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" class=""simbolmes noImprimible"" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"
                                            End If
                                        Else
                                            strURL = "/utils/obreFitxer.ashx?" + HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbrow3("DOCINNOD") & "&codiIdioma=" + idioma.ToString() & "&f=" & dbrow3("DOCDSFIT")))
                                        End If



                                        If titolContingut = "" Then
                                            titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                                        End If
                                        target = GAIA2.obtenirTarget(objConn, rel)

                                        strvoid = GAIA2.trobaEstilCSS2(objConn, plantilla.arrayCSSPlantilla(idCel), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA, "", plantilla.arrayEsImatge(idCel))



                                        If esEML = "N" Then
                                            celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) + "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                        Else
                                            celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) + "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & celdaFormateada.Substring(pospunt - 1) & "</a>"
                                        End If




                                    End If
                                End If
                            Else
                                pospunt = InStr(celdaFormateada, "<img")
                                Select Case plantilla.arrayEnllaços(idCel)
                                    Case "METLLNK.LNKCDREL"

                                        If pospunt > 0 Then
                                            strURL = GAIA2.obtenirEnllaçSimple(objConn, rel.infil, idioma, target, urlDesti)
                                            If esEML = "S" Then
                                                botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" style=""float:right; position:relative; top:-18px; right:3px; border:0"" class=""noImprimible"" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"
                                            Else
                                                botoMesImatge = "<img src=""http://www.l-h.cat/img/common/mesimg.gif"" class=""simbolmes noImprimible"" alt=""" & IIf(idioma = 2, "Ver imagen de ", IIf(idioma = 3, "﻿seeing image of ", "Veure imatge de ")) & titolContingut & IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)")) & """/>"
                                            End If

                                            If titolContingut = "" Then
                                                titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                                            End If
                                            strvoid = GAIA2.trobaEstilCSS2(objConn, plantilla.arrayCSSPlantilla(idCel), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA, "", plantilla.arrayEsImatge(idCel))



                                            'celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & strURL & """ title=""" & GAIA.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"



                                            If esEML = "N" Then
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                            Else
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & celdaFormateada.Substring(pospunt - 1) & "</a>"
                                            End If


                                        End If
                                    Case "METLLNK.LNKDSLNK"

                                        If pospunt > 0 Then
                                            GAIA2.bdr(objConn, "SELECT LNKDSLNK,LNKWNTIP FROM METLLNK WITH(NOLOCK) WHERE LNKINNOD=" & rel.infil & " AND LNKINIDI=" & idioma, ds3)

                                            If ds3.Tables(0).Rows.Count = 0 Then
                                                GAIA2.bdr(objConn, "SELECT LNKDSLNK,LNKWNTIP FROM METLLNK WITH(NOLOCK) WHERE LNKINNOD=" & rel.infil & " ORDER BY LNKINIDI", ds3)

                                            End If

                                            If ds3.Tables(0).Rows.Count > 0 Then
                                                dbrow3 = ds3.Tables(0).Rows(0)
                                                If dbrow3("LNKDSLNK") <> "" Then
                                                    strURL = dbrow3("LNKDSLNK")
                                                    If titolContingut = "" Then
                                                        titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                                                    End If
                                                    strvoid = GAIA2.trobaEstilCSS2(objConn, plantilla.arrayCSSPlantilla(idCel), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA, "", plantilla.arrayEsImatge(idCel))

                                                    If esEML = "N" Then
                                                        celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(dbrow3("LNKWNTIP") = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(dbrow3("LNKWNTIP") = 1, " target=""_blank""", " target=""_self""") & ">" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                                    Else
                                                        celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & strURL & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(dbrow3("LNKWNTIP") = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(dbrow3("LNKWNTIP") = 1, " target=""_blank""", " target=""_self""") & ">" & celdaFormateada.Substring(pospunt - 1) & "</a>"
                                                    End If

                                                End If
                                            End If
                                        End If
                                    Case "METLNOT.NOTDSLNK"
                                        GAIA2.bdr(objConn, "SELECT " & plantilla.arrayEnllaços(idCel) & " FROM METLNOT WITH(NOLOCK) WHERE NOTINNOD=" & rel.infil & " AND NOTINIDI=" & idioma, ds3)
                                        If ds3.Tables(0).Rows.Count > 0 Then
                                            dbrow3 = ds3.Tables(0).Rows(0)
                                            If titolContingut = "" Then
                                                titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                                            End If

                                            'he de netejar el html que pugui haver a NOTDSLNK
                                            Dim strTMp As String = dbrow3("NOTDSLNK")
                                            If InStr(strTMp, "<a") Then
                                                strTMp = strTMp.Substring(InStr(strTMp, "http") - 1)
                                                strTMp = strTMp.Substring(0, InStr(strTMp, """") - 1)

                                            End If

                                            strvoid = GAIA2.trobaEstilCSS(objConn, plantilla.arrayCSSPlantilla(idCel), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA, plantilla.arrayEsImatge(idCel))

                                            If strTMp.Length And celdaFormateada.Length > 0 Then
                                                If esEML = "N" Then
                                                    celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTMp) & """  " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(titolContingut) & """>" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                                Else
                                                    celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTMp) & """  " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(titolContingut) & """>" & celdaFormateada.Substring(pospunt - 1) & "</a>"
                                                End If
                                            End If

                                        End If

                                    Case "METLINF.INFDSLNK"
                                        GAIA2.bdr(objConn, "SELECT METLINF.INFDSLNK FROM METLINF WITH(NOLOCK) WHERE INFINNOD=" & rel.infil & " AND INFINIDI=" & idioma, ds3)
                                        If ds3.Tables(0).Rows.Count > 0 Then
                                            dbrow3 = ds3.Tables(0).Rows(0)
                                            'If titolContingut = "" Then
                                            '    titolContingut = GAIA.obtenirTitolContingut(objConn, rel, idioma)
                                            ' End If

                                            'he de netejar el html que pugui haver a INFDSLNK
                                            Dim str As String = dbrow3("INFDSLNK")
                                            Dim strHref As String = ""
                                            Dim strTarget As String = ""
                                            Dim strTmp As String = ""
                                            Dim strText As String = ""

                                            If InStr(str, "href") > 0 Then
                                                str = str.Replace("href", "|href")
                                                Dim k As String() = str.Split("|")
                                                For Each strItem As String In k
                                                    If InStr(strItem, "href") > 0 Then

                                                        strHref = ""
                                                        strTarget = ""
                                                        strText = ""
                                                        strTmp = ""
                                                        strItem = strItem.Replace("""", "|")
                                                        'recupero el href 
                                                        strTmp = strItem.Substring(InStr(strItem, "href=|") + 5)
                                                        strHref = strTmp.Substring(0, InStr(strTmp, "|") - 1)
                                                        'recupero target                        
                                                        strTmp = strItem.Substring(InStr(strItem, "target=|") + 7)
                                                        strTarget = strTmp.Substring(0, InStr(strTmp, "|") - 1)
                                                        'recupero text
                                                        strTmp = strItem.Substring(InStr(strItem, ">"))
                                                        strText = strTmp.Substring(0, InStr(strTmp, "<") - 1)
                                                    End If
                                                Next strItem
                                            Else
                                                strHref = str
                                                strTarget = "_self"
                                                strText = str
                                            End If

                                            strvoid = GAIA2.trobaEstilCSS(objConn, plantilla.arrayCSSPlantilla(idCel), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA, plantilla.arrayEsImatge(idCel))

                                            If strTmp.Length > 0 Then
                                                If esEML = "N" Then
                                                    celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strHref) & """  " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(strText) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """>" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                                Else
                                                    celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strHref) & """  " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(strText) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """>" & celdaFormateada.Substring(pospunt - 1) & "</a>"

                                                End If
                                            End If
                                        End If
                                    Case "METLAGD.AGDDSLNK"
                                        GAIA2.bdr(objConn, "SELECT " & plantilla.arrayEnllaços(idCel) & " FROM METLAGD WITH(NOLOCK) WHERE AGDINNOD=" & rel.infil & " AND AGDINIDI=" & idioma, ds3)
                                        If ds3.Tables(0).Rows.Count > 0 Then
                                            dbrow3 = ds3.Tables(0).Rows(0)
                                            If titolContingut = "" Then
                                                titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                                            End If
                                            Dim strTMp As String = dbrow3("AGDDSLNK")
                                            If InStr(strTMp, "<a") Then
                                                strTMp = strTMp.Substring(InStr(strTMp, "http") - 1)
                                                strTMp = strTMp.Substring(0, InStr(strTMp, """") - 1)

                                            End If
                                            strvoid = GAIA2.trobaEstilCSS(objConn, plantilla.arrayCSSPlantilla(idCel), desfaseHoritzontal, heretaPropietatsWeb, strvoid, estilsCSSSenseFons, esEML, posaWidthGAIA, "", posafloatGAIA, plantilla.arrayEsImatge(idCel))


                                            If esEML = "N" Then
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTMp) & """ " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(titolContingut) & """>" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                            Else
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTMp) & """ " & estilsCSSSenseFons & " title=""" & GAIA2.netejaHTML(titolContingut) & """>" & celdaFormateada.Substring(pospunt - 1) & "</a>"

                                            End If
                                        End If
                                    Case "AUTO-ENLLAÇ"
                                        If pospunt > 0 Then 'si no hi ha imatge no fem l'autoenllaç
                                            Dim strTmp As String = obtenirEnllacContingut(objConn, rel, idioma)
                                            If InStr(UCase(strTmp), "OBREFITXER") > 0 Then
                                                '  strTmp = strTmp.Replace("?", "?" & idioma)
                                            End If
                                            If esEML = "N" Then
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTmp) & """ " & estilsCSSSenseFons & " title=""" & GAIA2.obtenirTitolContingut(objConn, rel, idioma) & """>" & celdaFormateada.Substring(pospunt - 1, InStrRev(celdaFormateada, "</div>") - pospunt) & "</a></div>"
                                            Else
                                                celdaFormateada = celdaFormateada.Substring(0, pospunt - 1) & "<a href=""" & GAIA2.netejaHTML(strTmp) & """ " & estilsCSSSenseFons & " title=""" & GAIA2.obtenirTitolContingut(objConn, rel, idioma) & """>" & celdaFormateada.Substring(pospunt - 1) & "</a>"
                                            End If
                                        End If
                                End Select
                            End If
                        End If
                    End If 'IF (plantilla.arrayCampsPlantilla(idCel).IndexOf(".")>=0) THEN          
                End If

                If True Then
                    'Poso l'autolink
                    If codiRelacioArxiuPerVisorImatges = 0 And plantilla.arrayPAL.Length > idCel Then
                        If (plantilla.arrayPAL(idCel).Trim.Length > 0) Then
                            'Per ara només ensenyo l'autolink si publico la pàgina.
                            fitxerAutoLink = plantilla.arrayAAL(idCel) & "?" & idioma & HttpUtility.UrlEncode(oCodParam.encriptar("cr=" & codiRelacio & "&id=" & idioma.ToString() & "&pl=" & plantilla.arrayPAL(idCel).Trim() & "&us=0"))
                            tancaEnllaç = False
                            ' Si no he trobat titol del contingut busco el títol del contingut segons l'idioma
                            If titolContingut = "" Then
                                titolContingut = GAIA2.obtenirTitolContingut(objConn, rel, idioma)
                            End If
                            ' Si no he trobat titol del contingut agafo el nom del node, és un problema per que només apareix en català.
                            If titolContingut = "" Then
                                titolContingut = rel.noddstxt
                            End If
                            Select Case idioma
                                Case 2
                                    titolContingut = "Leer m&aacute;s de " & titolContingut
                                Case 3
                                    titolContingut = "Read more about " & titolContingut
                                Case Else
                                    titolContingut = "Llegir m&eacute;s de " & titolContingut
                            End Select

                            'Per accessibilitat poso el títol al text de l'enllaç però ocult amb técniques accessibles
                            If plantilla.arrayALK(idCel).Length > 0 Then
                                If esEML = "N" Then
                                    celdaFormateada += GAIA2.posaFormat2(objConn, plantilla.arrayALK(idCel) & "<span class=""visibilidadoculta"">(" & titolContingut & ")</span>", plantilla.arrayCSSPlantilla(idCel), fitxerAutoLink, esImatge, width, 0, 0, estat, titolContingut, tancaEnllaç, 0, plantilla.arrayALF(idCel), rel, relIni, idioma, esEML, botoMesImatge, titolContingut, plantilla, 0, "")
                                Else
                                    'celdaFormateada += GAIA.posaFormat(objConn, plantilla.arrayALK(idCel), plantilla.arrayCSSPlantilla(idCel), fitxerAutoLink, esImatge, width, tamanyH, tamanyV, estat, titolContingut, tancaEnllaç, heretaPropietatsWeb, plantilla.arrayALF(idCel), rel, relIni, idioma, esEML, botoMesImatge, titolContingut, plantilla, pareTeEnllac, css)

                                End If
                            End If
                        End If

                    End If
                End If

                If plantilla.arrayLC2.Length > idCel Then
                    If (plantilla.arrayLC2(idCel).Trim.Length > 0) And celdaFormateada.Length > 0 Then
                        For Each item In plantilla.arrayLC2(idCel).Split("|")
                            '  lcwPOST += trobaCodiWeb(objConn, plantilla, item, idioma, rel, relIni, forçarCodiPost, idCel, "", width, codiUsuari, css, tagsMeta)
                        Next item
                    End If
                End If




                If tancaEnllaç Then
                    celdaFormateada += "</a>"
                    tancaEnllaç = False
                End If

                If codiRelacioArxiuPerVisorImatges <> 0 Then

                    plantillaTMP.bdget(objConn, plantilla.arrayPLTDSPLT(idCel))
                    If plantillaTMP.innod > 0 Then

                        relarxiu.bdget(objConn, codiRelacioArxiuPerVisorImatges)
                        celdaFormateada = "<a href=""" & GAIA2.netejaHTML(enllaç) & """ title=""" & GAIA2.netejaHTML(titolContingut) & IIf(target = 0, "", IIf(idioma = 2, " (nueva ventana)", IIf(idioma = 3, " (new window)", " (nova finestra)"))) & """ " & estilsCSSSenseFons & IIf(target = 1, " target=""_blank"" ", " target=""_self"" ") & ">" & GAIA2.netejaHTML(titolContingut) & "</a></div>"

                    Else
                        GAIA2.f_logError(objConn, "maquetarContingut", "", "S'ha intentat representar una imatge o document dins d'un contingut sense plantilla secund&agrave;ria per representar-lo")
                    End If
                End If


                Dim tamanyContingut As Integer

                tamanyContingut = celdaFormateada.Trim.Length

                If forçarCodiAnt = 1 Or tamanyContingut > 0 Then
                    celdaFormateada = lcwANT + celdaFormateada
                End If
                If forçarCodiPost = 1 Or tamanyContingut > 0 Then
                    celdaFormateada = celdaFormateada + lcwPOST
                End If



                html += celdaFormateada
                DS.Dispose()
                ds3.Dispose()
                oCodParam = Nothing
            End If 'plantilla.innod<>0
        End If
        Return html
    End Function 'maquetarContingut



    'N
    Public Shared Function posaFormat2(ByVal objConn As OleDbConnection, ByVal strContingut As String, ByVal css As String, ByVal enllaç As String, ByVal esImatge As String, ByVal width As String, ByVal tamanyH As Integer, ByVal tamanyV As Integer, ByVal estat As Integer, ByVal textalternatiu As String, ByVal tancaEnllaç As Boolean, ByVal heretaPropietatsWeb As Integer, ByVal target As Integer, ByVal rel As clsRelacio, ByVal relini As clsRelacio, ByVal idioma As Integer, ByVal esEML As String, ByVal botoMesImatge As String, ByVal titolContingut As String, ByVal plt As clsPlantilla2, ByVal pareTeEnllac As Boolean, ByRef cssGenerat As String) As String


        Dim strFormat As String
        Dim strCss, strCssTamanyText As String
        Dim desfaseHoritzontal As Integer

        Dim textAlternatiuExtes As String
        Dim strCssSenseFons As String = ""
        strCssTamanyText = ""
        posaFormat2 = ""
        strFormat = ""
        Dim posaFormatJS As String = ""
        Dim textAlternatiuImatge As String = ""
        Dim posaWidthGAIA As Integer = 1
        Dim posaFloatGAIA As Integer = 1
        Dim posaVoreres As String = ""
        Dim estilsHref As String = ""
        Dim imatgeSenseRatio As String = ""
        strCss = ""
        If width = 0 Then
            posaWidthGAIA = 0
        End If

        If esImatge = "1" And esEML = "S" Then
            posaWidthGAIA = 0
        End If
        textAlternatiuImatge = textalternatiu
        'Si és un enllaç el title del <a href ha de tenir el títol del enllaç
        If enllaç.Trim().Length > 0 Then
            If titolContingut.Length > 0 Then
                textalternatiu = GAIA2.netejaHTML(titolContingut)
            Else
                textalternatiu = GAIA2.obtenirTitolContingut(objConn, relini, idioma)
            End If
        Else

            textalternatiu = GAIA2.netejaHTML(textalternatiu.Trim())
            textAlternatiuImatge = textalternatiu

        End If


        textAlternatiuExtes = textalternatiu

        strCss = GAIA2.trobaEstilCSS2(objConn, css, desfaseHoritzontal, heretaPropietatsWeb, strCssTamanyText, strCssSenseFons, esEML, posaWidthGAIA, "", posaFloatGAIA, estilsHref, esImatge)
        ' Public Shared Function trobaEstilCSS2(ByVal objConn As OleDbConnection, ByVal codiEstilCss As String, ByRef desfaseHoritzontal As Integer, ByVal heretaPropietatsWeb As Integer, ByRef strCSSAmbtamanyText As String, ByRef strCSSSenseFons As String, ByVal esEMl As String, ByRef PosaWidthGAIA As Integer, ByRef posaVoreres As String, ByRef posaFloatGAIA As Boolean, Optional ByRef estilsHref As String = "", Optional ByVal esImatge As Integer = 0) As String
        If css <> "" Then

            Select Case estat
                Case ctPENDENTCADUCAR
                    strCss += " fonsCaducat"
                Case ctPENDENTPUBLICAR
                    strCss += " fonsPendent"
            End Select

            'Ho faig pels autolinks de les plantilles, ja que encara no tinc versió d'idiomes segons plantilla  

        End If



        Select Case idioma
            Case 1
                strContingut = strContingut.Replace("m&agrave;s informaci&oacute;n", "m&eacute;s informaci&oacute;").Replace("m&aacute;s [+]", "m&eacute;s [+]")
            Case 2
                strContingut = strContingut.Replace("m&eacute;s informaci&oacute;", "m&aacute;s informaci&oacute;n").Replace("m&eacute;s [+]", "m&aacute;s [+]").Replace("llegir m&eacute;s", "leer m&aacute;s").Replace("llegir-ne m&eacute;s", "leer m&aacute;s")
            Case 3
                strContingut = strContingut.Replace("m&eacute;s informaci&oacute;", "more information").Replace("m&eacute;s [+]", "more [+]").Replace("llegir m&eacute;s", "read more").Replace("llegir-ne m&eacute;s", "read more")
        End Select
        '************************************************************
        ' és una imatge
        '************************************************************
        If esImatge = "1" Then


            If InStr(strContingut, ".jpg") > 0 Or InStr(strContingut, ".swf") > 0 Or InStr(strContingut, "obreFitxer") > 0 Then
                'si hi ha enllaç poso els tags
                If enllaç.Trim().Length > 0 Then
                    If botoMesImatge <> "" And esImatge Then
                        textAlternatiuExtes = IIf(idioma = 1, "Veure imatge de ", "Ver Imagen de ") & textalternatiu
                        'Poso target=1 per que obri sempre la imatge amb el visor a una finestra nova
                        target = 1
                    End If
                    posaFormat2 = "<a href=""" + enllaç + """  target=""" & IIf(target = 1, "_blank", "_self") & """ title=""" & GAIA2.netejaHTML(textAlternatiuExtes) & IIf(target = 1, IIf(idioma = 1, " (nova finestra)", " (nueva ventana) "), "") & """ class="""">" & posaFormat2
                    tancaEnllaç = True
                    'el strcss només l'he de posar si és una imatge (no un swf)                 
                End If
                '******************************************************************
                'és un flash
                '******************************************************************
                If InStr(strContingut, "swf") > 0 Then
                    posaFormat2 += "<object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0""  " + strCss + " alt=""" & GAIA2.netejaHTML(textalternatiu.Replace("""", "´").Trim()) & """><param name=""scale"" value=""ExactFit""/><param name=""movie"" value=""" + strContingut + """><param name=""quality"" value=""high""><embed src=""" + strContingut + """ quality=""high"" pluginspage=""http://www.macromedia.com/go/getflashplayer"" type=""application/x-shockwave-flash"" " + strCss + "><param name=""scale"" value=""ExactFit ""/></embed></object>"
                    '******************************************************************
                    '  és una imatge                                                        
                    '******************************************************************                 
                Else
                    textalternatiu = textAlternatiuExtes & IIf(target = 1, IIf(idioma = 1, " (nova finestra)", " (nueva ventana) "), "")

                    If InStr(strCss, "ratio") > 0 Then
                        Dim crida As String()
                        crida = strContingut.Split("?")
                        Dim oCodParam As New lhCodParam.lhCodParam
                        strContingut = crida(0) & "?" & oCodParam.encriptar(oCodParam.desencriptar(crida(1)) & "&r=" & strCss.Substring(InStr(strCss, "ratio") + 4, 3))
                        imatgeSenseRatio = crida(0) & "?" & oCodParam.encriptar(oCodParam.desencriptar(crida(1)))
                        'elimino el text de la ratio

                        strCss = strCss.Substring(0, InStr(strCss, "ratio") - 1) & strCss.Substring(InStr(strCss, "ratio") + 7)
                    Else
                        imatgeSenseRatio = strContingut
                    End If
                    If (esEML = "N") Then
                        'posaFormatJS = "<a href=""" & imatgeSenseRatio & """ target=""_blank"" title=""" & GAIA.netejaHTML(textalternatiu) & """ ><img src=""" & strContingut & """ alt=""" & textAlternatiuImatge & """ " & strCss & IIf(width = 0, " width=""" + tamanyH.ToString() + """ height=""" + tamanyV.ToString() + """", "") & "/></a>"

                        posaFormatJS = "<a href=""" & imatgeSenseRatio & """ target=""_blank"" title=""" & GAIA2.netejaHTML(textalternatiu) & """ ><img src=""" & strContingut & """ alt=""" & textAlternatiuImatge & """ " & strCss & "/></a>"


                    End If
                    posaFormat2 = posaFormat2 & "<img src=""" + strContingut + """ alt=""" & textAlternatiuImatge & """ " & IIf(esEML = "S", strCss, strCss.Replace("=""", "=""border0 "))
                    If width = 0 Then
                        '  posaFormat2 &= " width=""" + tamanyH.ToString() + """ height=""" + tamanyV.ToString() + """"
                    Else
                        ' si és un correu i hi ha estils width, els poso directament a les imatges per evitar que l'exchange ho elimini
                        Try
                            If (esEML = "S") Then

                                'strcss serà del tipus :  width:100px;height:200px --> retallo i em quedo amb el valor del width
                                'Dim tmpMida As String
                                'tmpMida = strCss.Substring(InStr(strCss, "width:") + 5)
                                'tmpMida = tmpMida.Substring(0, InStr(tmpMida, "px") - 1)
                                'posaFormat &= " width=""" & tmpMida & """ "
                                posaFormat2 &= " width=""" & width & """ "

                                If InStr(strCss, "height") > 0 Then
                                    Dim tmpMida As String
                                    tmpMida = strCss.Substring(InStr(strCss, "height:") + 6)
                                    tmpMida = tmpMida.Substring(0, InStr(tmpMida, "px") - 1)
                                    posaFormat2 &= " height=""" & tmpMida & """ "
                                End If
                            End If
                        Catch

                        End Try
                    End If
                    posaFormat2 &= "/>"


                End If

                If enllaç.Trim().Length > 0 And tancaEnllaç Then
                    posaFormat2 += botoMesImatge + "</a>"
                    tancaEnllaç = False
                End If
            Else
                'hi ha un document que s'espera com a imatge i no ho és. De moment no el mostro.
            End If
        Else 'No és una imatge.
            If enllaç.Trim().Length > 0 Then

                If textalternatiu = "" Then
                    textalternatiu = GAIA2.obtenirTitolContingut(objConn, relini, idioma)
                End If
                If InStr(enllaç, "obreFitxer") > 0 Or InStr(enllaç, "visorImatges") > 0 Then 'si és un document l'obro en una finestra nova.                           
                    estilsHref = strCss

                    strCss = ""
                    posaFormat2 = "<a href=""" & enllaç & """ " & estilsHref & " target=""_blank"" title=""" & GAIA2.netejaHTML(textalternatiu) & IIf(idioma = 1, " (nova finestra) ", " (nueva ventana) ") & """>" & strContingut & "</a>"


                Else
                    If InStr(enllaç, "<a") > 0 Then
                        'agafo el href i el tracto
                        Dim tmpstr As String = enllaç
                        tmpstr = tmpstr.Substring(InStr(tmpstr, "href=") + 5)
                        tmpstr = tmpstr.Substring(0, InStr(tmpstr, """") - 1)
                        enllaç = tmpstr
                    End If
                    estilsHref = strCssSenseFons
                    strCss = ""

                    posaFormat2 = "<a href=""" & enllaç & """ " & estilsHref & " target=""" & IIf(target = 1, "_blank", "_self") & """ title=""" & GAIA2.netejaHTML(textalternatiu) & IIf(target = 1, IIf(idioma = 1, " (nova finestra)", " (nueva ventana) "), "") & """>" & strContingut & "</a>"



                End If

            Else
                posaFormat2 = strContingut
            End If
        End If


        Select Case idioma
            Case 1
                posaFormat2 = posaFormat2.Replace("_2.aspx", "_1.aspx")
            Case Else
                posaFormat2 = posaFormat2.Replace("_1.aspx", "_" & idioma & ".aspx")
        End Select

        'Poso el nou visor d'imatges i el tractament de javascript
        If esImatge = "1" And pareTeEnllac = False And esEML = "N" Then
            'Vaig a buscar totes les imatges del mateix nivell per la galeria
            If plt.vis = 1 Then
                posaFormat2 = posaVisorImatges2(objConn, plt, rel, idioma, width, posaFormat2, posaFormatJS, cssGenerat, strCss)
            End If

        End If

    End Function 'posaFormat2


    'N
    Public Shared Function posaVisorImatges2(ByVal objconn As OleDbConnection, ByVal plt As clsPlantilla2, ByVal rel As clsRelacio, ByVal idioma As Integer, ByVal width As Integer, ByVal strHtml As String, ByVal posaFormatJs As String, ByRef cssGenerat As String, ByVal strcss As String) As String

        Dim htmlVisorImatges As String = ""
        Dim sSql As String = ""
        Dim r As New Random()
        Dim num As Integer = r.Next(999999999)
        If cssGenerat Is Nothing Then
            cssGenerat = ""
        End If

        If plt.vis = 1 Then
            Dim itemActual As Integer = 0
            Dim i As Integer = 0
            Dim llistaImatges As String = ""
            Dim dbrow As DataRow
            Dim dt As New DataSet
            Dim strContingut As String = ""

            sSql = " SELECT distinct  relAltresFotos.RELINFIL,docFotos.DOCDSTIT,docFotos.DOCDSFIT ,docFotos.DOCWNHOR,docFotos.DOCWNVER,docOriginal.DOCWNSIZ,docFotos.DOCDSALT, relAltresFotos.RELCDORD  FROM METLTDO as tipusOriginal WITH(NOLOCK) , METLDOC as docOriginal WITH(NOLOCK)  INNER JOIN METLREL as relFoto   WITH(NOLOCK) ON  relFoto.RELINFIL=docOriginal.DOCINNOD AND  relFoto.RELCDSIT <98  INNER JOIN METLREL as relAltresFotos  WITH(NOLOCK) ON relAltresFotos.RELCDHER like relFoto.RELCDHER AND relAltresFotos.RELCDSIT<98 INNER JOIN METLDOC as docFotos WITH(NOLOCK)  ON docFotos.DOCINNOD=relAltresFotos.RELINFIL AND docFotos.DOCINIDI=1  INNER JOIN METLTDO as tipusDocumentFotos  WITH(NOLOCK) ON  docFotos.DOCINTDO=tipusDocumentFotos.TDOCDTDO  AND tipusDocumentFotos.TDODSNOM LIKE '%image%'  WHERE(docOriginal.DOCINNOD = " & rel.infil & ") AND docOriginal.DOCINIDI=1 AND docOriginal.DOCINTDO=tipusOriginal.TDOCDTDO ORDER BY relAltresFotos.RELCDORD "
            GAIA2.bdr(objconn, sSql, dt)
            Dim strtmp As String = ""
            Dim hsImatges As New Hashtable()
            Dim oCodParam As New lhCodParam.lhCodParam
            Dim mida As String = ""
            For Each dbrow In dt.Tables(0).Rows
                If Not hsImatges.Contains(dbrow("RELINFIL")) Then

                    hsImatges.Add(dbrow("RELINFIL"), "1")
                    If dbrow("RELINFIL") = rel.infil Then
                        'strContingut = "/utils/obrefitxer.ashx?" & HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbrow("RELINFIL") & "&codiIdioma=" & idioma & mida))
                        If dt.Tables(0).Rows.Count > 1 Then
                            llistaImatges = llistaImatges & "<li>" & posaFormatJs & "</li>"
                        Else
                            llistaImatges = llistaImatges & posaFormatJs

                        End If
                    Else
                        'poso les noves imatges
                        Dim fitxer As String = ""

                        '   If width <= 200 And dbrow("DOCWNHOR") <> 0 Then 'el cas de width=0 l'utilitzem només per donar-li mida màxima disponible a la imatge
                        '      mida = "&t=P100"
                        '     fitxer = dbrow("DOCDSFIT").replace(".", "P100.")
                        ' Else
                        '    If width <= 700 Then
                        mida = "&t=P"
                        fitxer = dbrow("DOCDSFIT").replace(".", "P.")
                        '  ELSE
                        '      fitxer = dbRow("DOCDSFIT")
                        '      mida = "&t=imatgeGran"   
                        'End If
                        ' End If

                        '  If InStr(strcss, "ratio") > 0 Then
                        '     mida &= "&w=" & dbrow("DOCWNHOR") & "r=" & strcss.Substring(InStr(strcss, "ratio") + 4, 3)
                        ' End If
                        strContingut = "/utils/obrefitxer.ashx?" & HttpUtility.UrlEncode(oCodParam.encriptar("codiNode=" & dbrow("RELINFIL") & "&codiIdioma=" & idioma & mida & "&f=" & fitxer))


                        If dbrow("docWNHOR") > 700 And width <> 0 Then
                            llistaImatges = llistaImatges & "<li class=""amaga""><a href=""" & strContingut & """ target=""_blank""><img src=""" & strContingut & """ alt=""" & dbrow("DOCDSTIT") & """" & " width=""700""/></a></li>"
                        Else
                            llistaImatges = llistaImatges & "<li class=""amaga""><a href=""" & strContingut & """ target=""_blank""><img src=""" & strContingut & """ alt=""" & dbrow("DOCDSTIT") & """" & IIf(width = 0, " width=""" & dbrow("DOCWNHOR") & """ height=""" & dbrow("DOCWNVER") & """", " width=""700"" ") & "/></a></li>"
                        End If
                    End If
                End If
            Next dbrow

            If dt.Tables(0).Rows.Count > 1 Then
                llistaImatges = "<ul>" & llistaImatges & "</ul>"
            End If
            If dt.Tables(0).Rows.Count > 0 Then
                If InStr(cssGenerat, "gallery" & num) <= 0 Then
                    'cssGenerat &= " .gallery" & num & " {margin:0; padding:0; float:left; width:100%; position:relative;} .gallery" & num & " div {position:absolute; left:-9999px; overflow:hidden; display:none} "
                    cssGenerat &= " .gallery" & num & "  {margin:0 !important; padding:0 !important; float:left; width:100%; position:relative;} .gallery" & num & "  div {position:absolute; left:-9999px; overflow:hidden; display:none}"
                    If dt.Tables(0).Rows.Count > 1 Then
                        'cssGenerat &= " .gallery" & num & " ul {list-style: none; margin:0; padding:0} .gallery" & num & " ul li {display: inline; margin:0 !important; padding:0 !important} .gallery" & num & " ul li.amaga {position:absolute; left:-9999px; overflow:hidden; display:none }  .gallery" & num & " ul li:after {content: "".""; display: block; height: 0; clear: both; visibility: hidden;} .gallery" & num & " ul li {display: inline-block;} * html .gallery" & num & " ul li {height: 1%;}  .gallery" & num & " ul li{display: block;}  .gallery" & num & " u li { background:transparent}"
                        cssGenerat &= " .gallery" & num & " {margin:0 !important; padding:0 !important; float:left; width:100%; position:relative;} .gallery" & num & " div {position:absolute; left:-9999px; overflow:hidden; display:none} .gallery" & num & " ul {list-style: none; margin:0 !important; padding:0 !important; } .gallery" & num & " ul li {display: inline; margin:0 !important; padding:0 !important;} .gallery" & num & " ul li.amaga {position:absolute; left:-9999px; overflow:hidden; display:none;}  .gallery" & num & "  ul li:after {content: "".""; display: block; height: 0; clear: both; visibility: hidden;} .gallery" & num & " ul li {display: inline-block;} * html .gallery" & num & "  ul li {height: 1%;} .gallery" & num & "  ul li {display: block;} .gallery" & num & "  ul li { background:transparent}"
                    End If
                End If
                'cssGenerat &= "--></style> "
                strtmp &= "<div class=""gallery" & num & """ id=""gal" & num & """  style=""display:none;"">" & llistaImatges & "</div>"
                llistaImatges = strtmp & "<script type=""text/javascript"">document.getElementById('gal" & num & "').style.display = 'block'; </script>"
            End If
            dt.Dispose()



            htmlVisorImatges = "<div id=""noscript" & num & """><script type=""text/javascript""><!-- self.resizeTo(648, 750); self.toolbar = 0; self.status = false;--></script>" & strHtml & "</div>" & llistaImatges
            htmlVisorImatges &= "<script type=""text/javascript"">senseJS('noscript" & num & "'); $(function() {$('#gal" & num & " a').lightBox({ fixedNavigation: true });});</script>"
        Else
            Return (strHtml)
        End If

        Return (htmlVisorImatges)
    End Function 'posaVisorImatges2



    Public Shared Function trobaCodiWeb2(ByVal objConn As OleDbConnection, ByVal plantilla As clsPlantilla2, ByVal codiWeb As String, ByVal codiIdioma As Integer, ByVal rel As clsRelacio, ByVal relIni As clsRelacio, ByRef forçarCodi As Integer, ByVal index As Integer, ByVal parametres As String, ByVal width As String, ByVal codiUsuari As Integer, ByRef strCss As String, ByRef tagsMeta As String) As String
        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim csstmp As String = ""
        DS = New DataSet()

        trobaCodiWeb2 = ""
        Dim resultat As String = ""


        Try

            Dim element As String

            For Each element In Split(codiWeb, "|")





                GAIA2.bdr(objConn, "SELECT LCWCDTIP,  LCWDSTXT, LCWTPFOR, LCWTPFOL FROM METLLCW  WITH(NOLOCK) WHERE LCWINNOD=" & element & " AND LCWINIDI=" + codiIdioma.ToString(), DS)
                If DS.Tables(0).Rows.Count = 0 Then 'busco un altre idioma
                    GAIA2.bdr(objConn, "SELECT LCWCDTIP,  LCWDSTXT, LCWTPFOR, LCWTPFOL FROM METLLCW  WITH(NOLOCK) WHERE LCWINNOD=" & element & " ORDER BY LCWINIDI ASC", DS)
                End If
                If DS.Tables(0).Rows.Count > 0 Then
                    dbRow = DS.Tables(0).Rows(0)

                    If dbRow("LCWTPFOR") = "S" Then
                        forçarCodi = 1
                    Else
                        forçarCodi = 0
                    End If
                    Dim llibreria As String
                    Dim trobaCodiWebTMP As String = ""
                    For Each llibreria In Split(dbRow("LCWDSTXT"), "|")
                        Select Case CInt(dbRow("LCWCDTIP"))
                            Case 1  ' codi HTML 
                                resultat += llibreria.Trim()
                            Case 2 'codi executable abans de la publicació (previsualització)


                                If InStr(llibreria, "?") > 0 Then
                                    'compte, ha de ser "lhintranet". només "intranet" dona un error de permisos
                                    trobaCodiWebTMP = GAIA2.GetHTML(objConn, llibreria & "&codiRelacio=" & rel.incod & "&codiIdioma=" & codiIdioma & "&codiRelacioInicial=" & relIni.incod & "&est=" & index & "&width=" & width & "&node=" & rel.infil & "&us=" & codiUsuari & "&codiPlantilla=" & plantilla.innod, element, codiIdioma, File.GetLastWriteTime("c:\inetpub\wwwroot\GAIA\aspx\llibreriacodiweb\" & llibreria.Trim().Substring(0, InStr(llibreria, "?") - 1)), csstmp, relIni, rel, index, codiUsuari, plantilla.innod)
                                Else
                                    trobaCodiWebTMP = GAIA2.GetHTML(objConn, llibreria & "?codiRelacio=" & rel.incod & "&codiIdioma=" & codiIdioma & "&codiRelacioInicial=" & relIni.incod & "&est=" & index & "&width=" & width & "&node=" & rel.infil & "&us=" & codiUsuari & "&codiPlantilla=" & plantilla.innod, element, codiIdioma, File.GetLastWriteTime("c:\inetpub\wwwroot\GAIA\aspx\llibreriacodiweb\" + llibreria.Trim()), csstmp, relIni, rel, index, codiUsuari, plantilla.innod)
                                End If
                                If trobaCodiWebTMP.Length = 0 And dbRow("LCWTPFOL") = "N" Then
                                    resultat = ""
                                    Exit For
                                Else
                                    resultat += trobaCodiWebTMP
                                End If

                            Case 3 'codi executable després de la publicació                
                                resultat += llibreria.Trim().Replace("Request(""stridioma"")", codiIdioma.ToString()).Replace("Request(""codiIdioma"")", codiIdioma.ToString())
                            Case 4 'fitxer executable després de la publicació                                      
                                Dim fs As New FileStream("c:\inetpub\wwwroot\GAIA\aspx\llibreriacodiweb\" + llibreria.Trim(), FileMode.Open, FileAccess.Read)
                                Dim sr As StreamReader = New StreamReader(fs, System.Text.Encoding.Default)
                                trobaCodiWebTMP = sr.ReadToEnd()
                                trobaCodiWebTMP = trobaCodiWebTMP.Replace("""<IDIOMA>""", codiIdioma)
                                sr.Close()
                                fs.Close()

                                If trobaCodiWebTMP.Length = 0 And dbRow("LCWTPFOL") = "N" Then
                                    resultat = ""
                                    Exit For
                                Else
                                    resultat += trobaCodiWebTMP
                                End If

                        End Select
                    Next llibreria


                    strCss &= csstmp

                    '********************************************************************************************************************
                    '   Cerco els blocs <style>  i els tagsmeta <link> de la execució de la llibreria i els poso a strcss i tagsMeta per 
                    '   pujarlos posteriorment al head de la pàgina
                    '********************************************************************************************************************
                    If CInt(dbRow("LCWCDTIP")) = 2 Then
                        Dim posIni As Integer = InStr(resultat, "<style type=""text/css")
                        Dim posFi As Integer
                        While posIni > 0
                            posFi = InStr(resultat, "</style>")
                            '27 = per saltar <style type="text/css"><!--
                            ' - 3 abans de --></style>
                            strCss &= resultat.Substring(posIni - 1 + 27, posFi - posIni - 3 - 27)
                            resultat = resultat.Substring(0, posIni - 1) + resultat.Substring(posFi + 7)
                            posIni = InStr(resultat, "<style type=""text/css")
                        End While
                        'a strCss tinc la relació d'estils
                        posIni = InStr(resultat, "<link")
                        posFi = 0
                        While posIni > 0
                            posFi = InStr(resultat.Substring(posIni), ">")
                            tagsMeta += resultat.Substring(posIni - 1, posFi + 1)
                            resultat = resultat.Substring(0, posIni - 1) + resultat.Substring(posIni + posFi)
                            posIni = InStr(resultat, "<link")
                        End While
                        'a tagsMeta tinc la relació de  <link>
                    End If

                End If
                trobaCodiWeb2 &= resultat
                resultat = ""
            Next
        Catch ex As Exception

            f_logError(objConn, "G01-trobacodiweb", ex.Source, ex.Message)
            llistatErrors &= "<br/> " & ex.ToString & ". <br/> Codiweb=" & codiWeb
            trobaCodiWeb2 = ""

        End Try

        DS.Dispose()
    End Function 'trobaCodiWeb2


    '***************************************************************************************************
    '   Funció: GAIA2.trobaEstilCSS
    '   Entrada: codiEstilCSS: integer amb el codi de l'estil a cercar
    '                   cerca a la taula METLCSS el valor CSSDSTXT apuntat PER una llista de codis a CodiEstilCSS
    '                   desfaseHoritzontal: per referencia. Camp CSSDSTHO. Tamany en pixels que ocupen els marges i els paddings.

    '                   posaWidthGAIA2: per defecte=1, segons el valor CSSWNWIG indicarà si cal posar automàticament els width i float de GAIA2 o no.
    '   Retorna els estils menys els de tipus tamany del text.
    '***************************************************************************************************
    'N
    Public Shared Function trobaEstilCSS2(ByVal objConn As OleDbConnection, ByVal codiEstilCss As String, ByRef desfaseHoritzontal As Integer, ByVal heretaPropietatsWeb As Integer, ByRef strCSSAmbtamanyText As String, ByRef strCSSSenseFons As String, ByVal esEMl As String, ByRef PosaWidthGAIA2 As Integer, ByRef posaVoreres As String, ByRef posaFloatGAIA2 As Boolean, Optional ByRef estilsHref As String = "", Optional ByVal esImatge As Integer = 0) As String




        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        trobaEstilCSS2 = ""
        strCSSAmbtamanyText = ""
        strCSSSenseFons = ""
        desfaseHoritzontal = 0
        posaVoreres = ""

        'Comprovo que codiEstilCss sigui diferent a 0,0,0,0,0,0.. si no hi ha cap estil dins de la cel·la no cal cercarlos.
        If codiEstilCss.Replace("0", "").Replace(",", "").Trim().Length <> 0 Then
            If codiEstilCss <> "undefined" Then

                GAIA2.bdr(objConn, "SELECT CSSINCOD,CSSWNWIG, CSSINTIP, CSSDSTHO,  CSSDSTHP,CSSDSTXT, CSSDSCSS, CSSWNFLT FROM METLCSS  WITH(NOLOCK) WHERE CSSINCOD IN (" + codiEstilCss + ")", DS)
                For Each dbRow In DS.Tables(0).Rows

                    desfaseHoritzontal += dbRow("CSSDSTHO")
                    PosaWidthGAIA2 = IIf(dbRow("CSSWNWIG") = 0, 0, PosaWidthGAIA2)   'faig un AND. Si un dels estils té CSSWNWIG=0 retorno 0
                    posaFloatGAIA2 = IIf(dbRow("CSSWNFLT") = 0, 0, posaFloatGAIA2)
                    If esEMl = "S" Then
                        If dbRow("CSSINTIP") = 24 Then 'css de tipus mida
                            If dbRow("CSSDSTXT").substring(0, 1) = "t" Then
                                strCSSAmbtamanyText += dbRow("CSSDSCSS").Trim() + " "
                            Else
                                trobaEstilCSS2 += dbRow("CSSDSCSS").Trim() + " "
                                strCSSSenseFons += dbRow("CSSDSCSS").Trim() + " "
                            End If
                        Else
                            If dbRow("CSSINTIP") <> 118 Then 'css de tipus fons
                                strCSSSenseFons += dbRow("CSSDSCSS").Trim() + " "
                            End If

                            If dbRow("CSSINTIP") = 23 Or dbRow("CSSINTIP") = 112 Then 'color o decoració
                                estilsHref &= dbRow("CSSDSCSS").Trim() + " "
                            End If

                            If dbRow("CSSINTIP") = 26 Then 'voreres arrodonides
                                posaVoreres = dbRow("CSSDSCSS").Trim() + " "
                            Else
                                trobaEstilCSS2 += dbRow("CSSDSCSS").Trim() + " "
                                strCSSAmbtamanyText += dbRow("CSSDSCSS").Trim() + " "
                            End If
                        End If


                    Else
                        If dbRow("CSSINTIP") = 24 Then 'css de tipus mida
                            If dbRow("CSSDSTXT").substring(0, 1) = "t" Or dbRow("CSSDSTXT").substring(0, 2) = "sc" Then
                                strCSSAmbtamanyText += dbRow("CSSDSTXT").Trim() + " "
                                '                                strCSSSenseFons += dbRow("CSSDSTXT").Trim() + " "
                            Else
                                If PosaWidthGAIA2 <> 0 Then
                                    trobaEstilCSS2 += dbRow("CSSDSTXT").Trim() + " "
                                End If
                            End If
                        Else
                            If dbRow("CSSINTIP") = 23 Or dbRow("CSSINTIP") = 112 Then 'color o decoració
                                estilsHref &= dbRow("CSSDSTXT").Trim() + " "
                            End If
                            If dbRow("CSSINTIP") <> 118 Then 'css de tipus fons
                                strCSSSenseFons += dbRow("CSSDSTXT").Trim() + " "
                            End If

                            If dbRow("CSSINTIP") = 26 Then 'voreres arrodonides
                                posaVoreres = dbRow("CSSDSTXT").Trim() + " "
                            Else
                                strCSSAmbtamanyText += dbRow("CSSDSTXT").Trim() + " "
                                trobaEstilCSS2 += dbRow("CSSDSTXT").Trim() + " "
                            End If
                        End If
                    End If
                Next dbRow


            End If
        End If

        If posaVoreres.Length > 0 Then
            If trobaEstilCSS2.Length > 0 Then
                posaVoreres &= " " & trobaEstilCSS2
                trobaEstilCSS2 = ""
                strCSSAmbtamanyText = ""
            End If
        End If

        If esImatge Then
            trobaEstilCSS2 &= " img-fluid"
        End If

        ' Si es un correu poso style per que tindré totes les descripcions dels estils.
        If esEMl = "S" Then
            trobaEstilCSS2 = "  style=""" + trobaEstilCSS2 + """"

            If strCSSAmbtamanyText.Length > 0 Then
                strCSSAmbtamanyText = " style=""" + strCSSAmbtamanyText + """"
            End If

            If estilsHref.Length > 0 Then
                estilsHref = " style=""" & estilsHref & """"
            End If

            If strCSSSenseFons.Length > 0 Then
                strCSSSenseFons = " style=""" & strCSSSenseFons & """"
            End If
        Else
            trobaEstilCSS2 = " class=""" + trobaEstilCSS2 + """"

            If strCSSAmbtamanyText.Length > 0 Then
                strCSSAmbtamanyText = " class=""" + strCSSAmbtamanyText + """"
            End If

            If estilsHref.Length > 0 Then
                estilsHref = " class=""" & estilsHref & """"
            End If

            If strCSSSenseFons.Length > 0 Then
                strCSSSenseFons = " class=""" & strCSSSenseFons & """"
            End If
        End If



        DS.Dispose()
    End Function 'trobaEstilCSS2

#End Region




End Class 'llibreriaGAIA

Public Class clsPlantilla
    Private _innod As Integer
    Private _est As String
    Private _cmp As String
    Private _css As String
    Private _lnk As String
    Private _img As String
    Private _lcw As String
    Private _num As String
    Private _lc2 As String
    Private _alk As String
    Private _pal As String
    Private _aal As String
    Private _alf As String
    Private _niv As String
    Private _dsalt As String
    Private _swalt As Integer
    Private _tco As String
    Private _plt As String
    Private _ver As String
    Private _hor As String
    Private _flw As String
    Private _vis As Integer
    Private _atr As String
    'Bloc d'arrays
    Private _arrayEstructuraPlantilla As String()
    Private _arrayCampsPlantilla As String()
    Private _arrayCSSPlantilla As String()
    Private _arrayEnllaços As String()
    Private _arrayEsImatge As String()
    Private _arrayLCW As String()
    Private _arrayDSNUM As String()
    Private _arrayLC2 As String()
    Private _arrayALK As String()
    Private _arrayPAL As String()
    Private _arrayAAL As String()
    Private _arrayALF As String()
    Private _arrayNIV As String()
    Private _arrayALT As String()
    Private _arrayTCO As String()
    Private _arrayPLTDSPLT As String()
    Private _arrayAtr As String()

    Public Sub New()
        MyBase.New()
        _innod = 0
        _est = ""

        _cmp = ""
        _css = ""
        _lnk = ""
        _img = ""
        _lcw = ""
        _num = ""
        _lc2 = ""
        _alk = ""
        _pal = ""
        _aal = ""
        _alf = ""
        _niv = ""
        _dsalt = ""
        _swalt = 0
        _tco = ""
        _plt = ""
        _hor = ""
        _ver = ""
        _flw = ""
        _atr = ""

        _vis = 0
        _arrayEstructuraPlantilla = New String() {}



        _arrayCampsPlantilla = New String() {}
        _arrayCSSPlantilla = New String() {}
        _arrayEnllaços = New String() {}
        _arrayEsImatge = New String() {}
        _arrayLCW = New String() {}
        _arrayDSNUM = New String() {}
        _arrayLC2 = New String() {}
        _arrayALK = New String() {}
        _arrayPAL = New String() {}
        _arrayAAL = New String() {}

        _arrayALF = New String() {}
        _arrayNIV = New String() {}
        _arrayALT = New String() {}

        _arrayTCO = New String() {}
        _arrayPLTDSPLT = New String() {}
        _arrayAtr = New String() {}

    End Sub
    Public Property innod() As Integer
        Get
            Return (_innod)
        End Get
        Set(ByVal value As Integer)
            _innod = value
        End Set
    End Property
    Public Property est() As String

        Get
            Return (_est)
        End Get
        Set(ByVal value As String)
            _est = value
        End Set
    End Property
    Public Property cmp() As String
        Get
            Return (_cmp)
        End Get

        Set(ByVal value As String)
            _cmp = value
        End Set
    End Property
    Public Property css() As String
        Get
            Return (_css)
        End Get
        Set(ByVal value As String)
            _css = value
        End Set
    End Property
    Public Property lnk() As String
        Get
            Return (_lnk)
        End Get
        Set(ByVal value As String)
            _lnk = value

        End Set
    End Property
    Public Property img() As String
        Get
            Return (_img)
        End Get
        Set(ByVal value As String)
            _img = value
        End Set
    End Property
    Public Property lcw() As String
        Get
            Return (_lcw)
        End Get
        Set(ByVal value As String)
            _lcw = value
        End Set
    End Property
    Public Property num() As String
        Get
            Return (_num)
        End Get
        Set(ByVal value As String)
            _num = value
        End Set
    End Property
    Public Property lc2() As String
        Get
            Return (_lc2)
        End Get
        Set(ByVal value As String)
            _lc2 = value
        End Set
    End Property
    Public Property alk() As String
        Get
            Return (_alk)
        End Get
        Set(ByVal value As String)
            _alk = value

        End Set
    End Property
    Public Property pal() As String
        Get
            Return (_pal)
        End Get
        Set(ByVal value As String)
            _pal = value
        End Set
    End Property
    Public Property aal() As String
        Get
            Return (_aal)
        End Get
        Set(ByVal value As String)
            _est = value
        End Set
    End Property
    Public Property alf() As String
        Get
            Return (_alf)
        End Get
        Set(ByVal value As String)
            _alf = value
        End Set
    End Property
    Public Property niv() As String
        Get
            Return (_niv)
        End Get
        Set(ByVal value As String)
            _niv = value
        End Set
    End Property
    Public Property dsalt() As String
        Get
            Return (_dsalt)
        End Get
        Set(ByVal value As String)
            _dsalt = value
        End Set
    End Property

    Public Property swalt() As Integer
        Get
            Return (_swalt)
        End Get
        Set(ByVal value As Integer)
            _swalt = value
        End Set
    End Property
    Public Property tco() As String
        Get
            Return (_tco)
        End Get
        Set(ByVal value As String)
            _tco = value
        End Set
    End Property

    Public Property plt() As String
        Get
            Return (_plt)
        End Get
        Set(ByVal value As String)
            _plt = value
        End Set
    End Property

    Public Property ver() As String
        Get
            Return (_ver)
        End Get
        Set(ByVal value As String)
            _ver = value
        End Set
    End Property
    Public Property hor() As String
        Get
            Return (_hor)
        End Get
        Set(ByVal value As String)
            _hor = value
        End Set
    End Property
    Public Property flw() As String
        Get
            Return (_flw)
        End Get
        Set(ByVal value As String)
            _flw = value

        End Set

    End Property

    Public Property vis() As Integer
        Get
            Return (_vis)
        End Get
        Set(ByVal value As Integer)
            _vis = value
        End Set
    End Property

    Public Property atr() As String
        Get
            Return (_atr)
        End Get
        Set(ByVal value As String)
            _atr = value
        End Set
    End Property

    Public Property arrayEstructuraPlantilla() As String()
        Get
            Return (_arrayEstructuraPlantilla)
        End Get
        Set(ByVal value As String())
            _arrayEstructuraPlantilla = value
        End Set
    End Property
    Public Property arrayCampsPlantilla() As String()
        Get
            Return (_arrayCampsPlantilla)
        End Get
        Set(ByVal value As String())
            _arrayCampsPlantilla = value
        End Set
    End Property


    Public Property arrayCSSPlantilla() As String()

        Get
            Return (_arrayCSSPlantilla)
        End Get
        Set(ByVal value As String())
            _arrayCSSPlantilla = value
        End Set
    End Property
    Public Property arrayEnllaços() As String()
        Get
            Return (_arrayEnllaços)
        End Get
        Set(ByVal value As String())
            _arrayEnllaços = value
        End Set
    End Property
    Public Property arrayEsImatge() As String()
        Get
            Return (_arrayEsImatge)
        End Get
        Set(ByVal value As String())
            _arrayEsImatge = value
        End Set
    End Property

    Public Property arrayLCW() As String()
        Get
            Return (_arrayLCW)
        End Get
        Set(ByVal value As String())
            _arrayLCW = value
        End Set
    End Property

    Public Property arrayDSNUM() As String()
        Get
            Return (_arrayDSNUM)
        End Get
        Set(ByVal value As String())
            _arrayDSNUM = value
        End Set
    End Property

    Public Property arrayLC2() As String()
        Get
            Return (_arrayLC2)
        End Get
        Set(ByVal value As String())
            _arrayLC2 = value
        End Set
    End Property

    Public Property arrayALK() As String()
        Get
            Return (_arrayALK)
        End Get
        Set(ByVal value As String())
            _arrayALK = value
        End Set
    End Property

    Public Property arrayPAL() As String()
        Get
            Return (_arrayPAL)
        End Get
        Set(ByVal value As String())
            _arrayPAL = value
        End Set
    End Property

    Public Property arrayAAL() As String()
        Get
            Return (_arrayAAL)
        End Get
        Set(ByVal value As String())
            _arrayAAL = value
        End Set
    End Property


    Public Property arrayALF() As String()
        Get
            Return (_arrayALF)
        End Get
        Set(ByVal value As String())
            _arrayALF = value
        End Set
    End Property

    Public Property arrayNIV() As String()
        Get
            Return (_arrayNIV)
        End Get
        Set(ByVal value As String())
            _arrayNIV = value
        End Set
    End Property


    Public Property arrayALT() As String()
        Get
            Return (_arrayALT)
        End Get
        Set(ByVal value As String())
            _arrayALT = value
        End Set
    End Property
    Public Property arrayTCO() As String()
        Get
            Return (_arrayTCO)
        End Get
        Set(ByVal value As String())
            _arrayTCO = value
        End Set
    End Property

    Public Property arrayPLTDSPLT() As String()
        Get
            Return (_arrayPLTDSPLT)
        End Get
        Set(ByVal value As String())
            _arrayPLTDSPLT = value
        End Set
    End Property

    Public Property arrayAtr() As String()
        Get
            Return (_arrayAtr)
        End Get
        Set(ByVal value As String())
            _arrayAtr = value
        End Set
    End Property




    Public Sub bdget(ByVal objconn As OleDbConnection, ByVal pltinnod As Integer)

        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim trobat As Boolean


        If pltinnod <> 0 Then
            GAIA2.bdr(objconn, "SELECT PLTDSATR,PLTINNOD, PLTDSEST, PLTDSCMP,PLTDSCSS, PLTDSLNK, PLTDSIMG, PLTDSLCW, PLTDSNUM, PLTDSLC2, PLTDSALK, PLTCDPAL, PLTDSAAL, PLTDSALF, PLTDSNIV, PLTSWALT, PLTDSALT, PLTDSTCO, PLTDSPLT,PLTDSHOR, PLTDSVER,PLTDSFLW, PLTSWVIS FROM METLPLT WITH(NOLOCK) where PLTINNOD=" & pltinnod, DS)
            If DS.Tables(0).Rows.Count > 0 Then
                trobat = True
                dbRow = DS.Tables(0).Rows(0)
                _innod = dbRow("PLTINNOD")

                _est = dbRow("PLTDSEST")
                _cmp = dbRow("PLTDSCMP")
                _css = dbRow("PLTDSCSS")
                _lnk = dbRow("PLTDSLNK")
                _img = dbRow("PLTDSIMG")
                _lcw = dbRow("PLTDSLCW")
                _num = dbRow("PLTDSNUM")
                _lc2 = dbRow("PLTDSLC2")
                _alk = dbRow("PLTDSALK")
                _pal = dbRow("PLTCDPAL")
                _aal = dbRow("PLTDSAAL")
                _alf = dbRow("PLTDSALF")
                _niv = dbRow("PLTDSNIV")
                _swalt = dbRow("PLTSWALT")
                _dsalt = dbRow("PLTDSALT")
                _tco = dbRow("PLTDSTCO")
                _plt = dbRow("PLTDSPLT")
                _ver = dbRow("PLTDSVER")
                _hor = dbRow("PLTDSHOR")
                _flw = dbRow("PLTDSFLW")
                _vis = dbRow("PLTSWVIS")
                _atr = dbRow("PLTDSATR")

                _arrayEstructuraPlantilla = Split(_est, ",")
                _arrayCampsPlantilla = Split(_cmp, ",")
                _arrayEnllaços = Split(_lnk, ",")
                _arrayEsImatge = Split(_img, ",")
                _arrayCSSPlantilla = Split(_css, "|")
                _arrayLCW = Split(_lcw, ",")
                _arrayDSNUM = Split(_num, ",")
                _arrayLC2 = Split(_lc2, ",")
                _arrayALK = Split(_alk, "|")
                _arrayALF = Split(_alf, ",")
                _arrayPAL = Split(_pal, ",")
                _arrayAAL = Split(_aal, "|")
                _arrayALT = Split(_dsalt, "|")
                _arrayTCO = Split(_tco, ",")
                _arrayNIV = Split(_niv, ",")
                _arrayPLTDSPLT = Split(_plt, ",")
                _arrayAtr = Split(_atr, ",")


            End If
        End If
        If Not trobat Or pltinnod = 0 Then
            _innod = 0
            _est = ""
            _cmp = ""
            _css = ""
            _lnk = ""
            _img = ""
            _lcw = ""
            _num = ""
            _lc2 = ""
            _alk = ""
            _pal = ""
            _aal = ""
            _alf = ""
            _niv = ""
            _swalt = 0

            _dsalt = ""
            _tco = ""
            _plt = ""
            _ver = ""
            _hor = ""
            _flw = ""
            _vis = 0
            _atr = ""
            'no faig els nothing dels array per que sempre validaré que _innod<>0
        End If
        DS.Dispose()
    End Sub


    Public Sub copiaObj(ByVal pla As clsPlantilla)
        _innod = pla.innod
        _est = pla.est
        _cmp = pla.cmp
        _css = pla.css
        _lnk = pla.lnk
        _img = pla.img
        _lcw = pla.lcw
        _num = pla.num
        _lc2 = pla.lc2
        _alk = pla.alk
        _pal = pla.pal
        _aal = pla.aal
        _alf = pla.alf
        _niv = pla.niv
        _swalt = pla.swalt
        _dsalt = pla.dsalt
        _tco = pla.tco
        _plt = pla.plt
        _ver = pla.ver
        _hor = pla.hor
        _flw = pla.flw
        _vis = pla.vis
        _atr = pla.atr

        _arrayEstructuraPlantilla = Split(pla.est, ",")
        _arrayCampsPlantilla = Split(pla.cmp, ",")
        _arrayEnllaços = Split(pla.lnk, ",")
        _arrayEsImatge = Split(pla.img, ",")
        _arrayCSSPlantilla = Split(pla.css, "|")
        _arrayLCW = Split(pla.lcw, ",")
        _arrayDSNUM = Split(pla.num, ",")
        _arrayLC2 = Split(pla.lc2, ",")
        _arrayALK = Split(pla.alk, "|")

        _arrayALF = Split(pla.alf, ",")
        _arrayPAL = Split(pla.pal, ",")
        _arrayAAL = Split(pla.aal, "|")
        _arrayALT = Split(pla.dsalt, "|")
        _arrayTCO = Split(pla.tco, ",")
        _arrayNIV = Split(pla.niv, ",")
        _arrayPLTDSPLT = Split(pla.plt, ",")
        _arrayAtr = Split(pla.atr, ",")
    End Sub
End Class





Public Class clsRelacio
    Private _incod As Integer
    Private _infil As Integer
    Private _inpar As Integer
    Private _cdarb As Integer
    Private _cdsit As Integer
    Private _cdher As String
    Private _noddstxt As String
    Private _tipdsdes As String
    Private _cdOrd As Integer
    Private _dsfit As String

    Private _cdest As Integer
    Private _inpla As Integer
    Private _tipintip As Integer
    Private _pcrincod As Integer
    Private _swvis As Integer
    Private _cdrsu As Integer
    Public Sub New()
        MyBase.New()
        _incod = 0
        _infil = 0
        _inpar = 0
        _cdher = ""
        _cdarb = 0
        _cdsit = 0
        _cdOrd = 0
        _dsfit = ""
        _cdest = 0
        _inpla = 0
        _noddstxt = ""
        _tipdsdes = ""
        _tipintip = 0
        _pcrincod = 0
        _swvis = 0
        _cdrsu = 0
    End Sub

    Public Property incod() As Integer
        Get

            Return (_incod)
        End Get
        Set(ByVal value As Integer)
            _incod = value
        End Set
    End Property

    Public Property infil() As Integer
        Get
            Return (_infil)
        End Get
        Set(ByVal value As Integer)
            _infil = value
        End Set

    End Property

    Public Property inpar() As Integer
        Get
            Return (_inpar)
        End Get
        Set(ByVal value As Integer)
            _inpar = value
        End Set
    End Property

    Public Property cdher() As String
        Get
            Return (_cdher)
        End Get
        Set(ByVal value As String)
            _cdher = value
        End Set
    End Property


    Public Property cdarb() As Integer
        Get
            Return (_cdarb)
        End Get
        Set(ByVal value As Integer)
            _cdarb = value
        End Set
    End Property


    Public Property cdsit() As Integer
        Get
            Return (_cdsit)
        End Get
        Set(ByVal value As Integer)
            _cdsit = value
        End Set
    End Property


    Public Property dsfit() As String
        Get
            Return (_dsfit)
        End Get
        Set(ByVal value As String)
            _dsfit = value

        End Set
    End Property


    Public Property noddstxt() As String
        Get
            Return (_noddstxt)
        End Get
        Set(ByVal value As String)
            _noddstxt = value
        End Set
    End Property

    Public Property tipdsdes() As String
        Get
            Return (_tipdsdes)
        End Get
        Set(ByVal value As String)
            _tipdsdes = value
        End Set
    End Property

    Public Property cdord() As Integer
        Get
            Return (_cdOrd)
        End Get
        Set(ByVal value As Integer)
            _cdOrd = value
        End Set
    End Property

    Public Property cdest() As Integer
        Get
            Return (_cdest)
        End Get
        Set(ByVal value As Integer)
            _cdest = value
        End Set
    End Property

    Public Property tipintip() As Integer
        Get
            Return (_tipintip)
        End Get
        Set(ByVal value As Integer)
            _tipintip = value
        End Set
    End Property

    Public Property inpla() As Integer
        Get
            Return (_inpla)
        End Get
        Set(ByVal value As Integer)
            _inpla = value
        End Set
    End Property
    Public Property pcrincod() As Integer
        Get
            Return (_pcrincod)
        End Get
        Set(ByVal value As Integer)
            _pcrincod = value
        End Set
    End Property




    Public Property swvis() As Integer
        Get
            Return (_swvis)
        End Get
        Set(ByVal value As Integer)

            _swvis = value
        End Set
    End Property

    Public Property cdrsu() As Integer
        Get
            Return (_cdrsu)
        End Get
        Set(ByVal value As Integer)
            _cdrsu = value
        End Set
    End Property




    Public Sub bdget(ByVal objconn As OleDbConnection, ByVal relincod As Integer, Optional ByVal node As Integer = 0, Optional ByVal trobarEsborrats As Boolean = False)

        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim trobat As Boolean
        If node > 0 Then

            GAIA2.bdr(objconn, "SELECT RELSWVIS, RELCDRSU, RELINCOD,RELINFIL,RELINPAR,RELCDHER,RELCDARB,RELCDSIT,RELDSFIT,RELCDORD,RELCDEST,RELINPLA, NODDSTXT, TIPDSDES, TIPINTIP, PCRINCOD  FROM METLREL WITH(NOLOCK)  INNER JOIN METLNOD WITH(NOLOCK) ON RELINFIL=NODINNOD INNER JOIN METLTIP WITH(NOLOCK) ON NODCDTIP=TIPINTIP LEFT OUTER JOIN METLPCR WITH(NOLOCK) ON RELINCOD=PCRINCOD  WHERE RELINFIL=" & node & IIf(trobarEsborrats, "   ", " AND RELCDSIT<99") & " ORDER BY RELSWVIS DESC,RELINCOD DESC", DS)
        Else
            If relincod <> 0 Then

                GAIA2.bdr(objconn, "SELECT RELSWVIS,RELCDRSU, RELINCOD,RELINFIL,RELINPAR,RELCDHER,RELCDARB,RELCDSIT,RELDSFIT,RELCDORD,RELCDEST,RELINPLA, NODDSTXT, TIPDSDES,  TIPINTIP, PCRINCOD   FROM METLREL WITH(NOLOCK)  INNER JOIN METLNOD  WITH(NOLOCK) ON RELINFIL=NODINNOD  INNER JOIN METLTIP WITH(NOLOCK) ON NODCDTIP=TIPINTIP LEFT OUTER JOIN  METLPCR WITH(NOLOCK) ON RELINCOD=PCRINCOD   WHERE RELINCOD=" & relincod & IIf(trobarEsborrats, "  ", " AND RELCDSIT<99"), DS)
            End If
        End If


        If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
            trobat = True
            dbRow = DS.Tables(0).Rows(0)
            _incod = dbRow("RELINCOD")
            _infil = dbRow("RELINFIL")

            _inpar = dbRow("RELINPAR")
            _cdher = dbRow("RELCDHER")
            _cdarb = dbRow("RELCDARB")
            _cdsit = dbRow("RELCDSIT")
            If Not TypeOf dbRow("RELDSFIT") Is DBNull Then
                _dsfit = dbRow("RELDSFIT").Trim
            End If
            _cdOrd = dbRow("RELCDORD")
            _noddstxt = dbRow("NODDSTXT").Trim
            _tipdsdes = dbRow("TIPDSDES").Trim
            _tipintip = dbRow("TIPINTIP")
            _cdest = dbRow("RELCDEST")
            If Not TypeOf dbRow("RELINPLA") Is DBNull Then
                _inpla = dbRow("RELINPLA")
            End If
            If Not TypeOf dbRow("PCRINCOD") Is DBNull Then
                _pcrincod = dbRow("PCRINCOD")
            End If
            _swvis = dbRow("RELSWVIS")
            If Not TypeOf dbRow("RELCDRSU") Is DBNull Then
                _cdrsu = dbRow("RELCDRSU")
            Else
                _cdrsu = 0
            End If
        End If


        If Not trobat Then
            _incod = 0
            _infil = 0
            _inpar = 0
            _cdher = ""
            _cdarb = 0
            _cdsit = 0
            _cdOrd = 0
            _dsfit = ""
            _cdest = 0
            _noddstxt = ""
            _tipdsdes = ""
            _tipintip = 0
            _inpla = 0
            _pcrincod = 0
            _swvis = 0
            _cdrsu = 0
        End If
        DS.Dispose()

    End Sub
    Public Sub copiaObj(ByVal rel As clsRelacio)
        _incod = rel.incod
        _infil = rel.infil
        _inpar = rel.inpar
        _cdher = rel.cdher
        _cdarb = rel.cdarb
        _cdsit = rel.cdsit
        _dsfit = rel.dsfit
        _cdOrd = rel.cdord
        _inpla = rel.inpla
        _noddstxt = rel.noddstxt.Trim
        _tipdsdes = rel.tipdsdes.Trim
        _tipintip = rel.tipintip
        _cdest = rel.cdest
        _pcrincod = rel.pcrincod
        _swvis = rel.swvis
        _cdrsu = rel.cdrsu
    End Sub

End Class
Public Class clsPlantilla2
    Private _innod As Integer
    Private _est As String 'html amb l'estructura
    Private _cmp As String
    Private _css As String
    Private _lnk As String
    Private _img As String
    Private _lcw As String
    Private _num As String
    Private _lc2 As String
    Private _alk As String
    Private _pal As String
    Private _aal As String
    Private _alf As String
    Private _niv As String
    Private _dsalt As String
    Private _swalt As Integer
    Private _tco As String
    Private _plt As String
    Private _ver As String
    Private _hor As String
    Private _flw As String
    Private _vis As Integer
    Private _atr As String
    'Bloc d'arrays
    Private _arrayEstructuraPlantilla As String()
    Private _arrayCampsPlantilla As String()
    Private _arrayCSSPlantilla As String()
    Private _arrayEnllaços As String()
    Private _arrayEsImatge As String()
    Private _arrayLCW As String()
    Private _arrayDSNUM As String()
    Private _arrayLC2 As String()
    Private _arrayALK As String()
    Private _arrayPAL As String()
    Private _arrayAAL As String()
    Private _arrayALF As String()
    Private _arrayNIV As String()
    Private _arrayALT As String()
    Private _arrayTCO As String()
    Private _arrayPLTDSPLT As String()
    Private _arrayAtr As String()

    Public Sub New()
        MyBase.New()
        _innod = 0
        _est = ""

        _cmp = ""
        _css = ""
        _lnk = ""
        _img = ""
        _lcw = ""
        _num = ""
        _lc2 = ""
        _alk = ""
        _pal = ""
        _aal = ""
        _alf = ""
        _niv = ""
        _dsalt = ""
        _swalt = 0
        _tco = ""
        _plt = ""
        _hor = ""
        _ver = ""
        _flw = ""
        _atr = ""

        _vis = 0
        _arrayEstructuraPlantilla = New String() {}



        _arrayCampsPlantilla = New String() {}
        _arrayCSSPlantilla = New String() {}
        _arrayEnllaços = New String() {}
        _arrayEsImatge = New String() {}
        _arrayLCW = New String() {}
        _arrayDSNUM = New String() {}
        _arrayLC2 = New String() {}
        _arrayALK = New String() {}
        _arrayPAL = New String() {}
        _arrayAAL = New String() {}

        _arrayALF = New String() {}
        _arrayNIV = New String() {}
        _arrayALT = New String() {}

        _arrayTCO = New String() {}
        _arrayPLTDSPLT = New String() {}
        _arrayAtr = New String() {}

    End Sub
    Public Property innod() As Integer
        Get
            Return (_innod)
        End Get
        Set(ByVal value As Integer)
            _innod = value
        End Set
    End Property
    Public Property est() As String
        Get
            Return (_est)
        End Get
        Set(ByVal value As String)
            _est = value
        End Set
    End Property
    Public Property cmp() As String
        Get
            Return (_cmp)
        End Get

        Set(ByVal value As String)
            _cmp = value
        End Set
    End Property
    Public Property css() As String
        Get
            Return (_css)
        End Get
        Set(ByVal value As String)
            _css = value
        End Set
    End Property
    Public Property lnk() As String
        Get
            Return (_lnk)
        End Get
        Set(ByVal value As String)
            _lnk = value

        End Set
    End Property
    Public Property img() As String
        Get
            Return (_img)
        End Get
        Set(ByVal value As String)
            _img = value
        End Set
    End Property
    Public Property lcw() As String
        Get
            Return (_lcw)
        End Get
        Set(ByVal value As String)
            _lcw = value
        End Set
    End Property
    Public Property num() As String
        Get
            Return (_num)
        End Get
        Set(ByVal value As String)
            _num = value
        End Set
    End Property
    Public Property lc2() As String
        Get
            Return (_lc2)
        End Get
        Set(ByVal value As String)
            _lc2 = value
        End Set
    End Property
    Public Property alk() As String
        Get
            Return (_alk)
        End Get
        Set(ByVal value As String)
            _alk = value

        End Set
    End Property
    Public Property pal() As String
        Get
            Return (_pal)
        End Get
        Set(ByVal value As String)
            _pal = value
        End Set
    End Property
    Public Property aal() As String
        Get
            Return (_aal)
        End Get
        Set(ByVal value As String)
            _est = value
        End Set
    End Property
    Public Property alf() As String
        Get
            Return (_alf)
        End Get
        Set(ByVal value As String)
            _alf = value
        End Set
    End Property
    Public Property niv() As String
        Get
            Return (_niv)
        End Get
        Set(ByVal value As String)
            _niv = value
        End Set
    End Property
    Public Property dsalt() As String
        Get
            Return (_dsalt)
        End Get
        Set(ByVal value As String)
            _dsalt = value
        End Set
    End Property

    Public Property swalt() As Integer
        Get
            Return (_swalt)
        End Get
        Set(ByVal value As Integer)
            _swalt = value
        End Set
    End Property
    Public Property tco() As String
        Get
            Return (_tco)
        End Get
        Set(ByVal value As String)
            _tco = value
        End Set
    End Property

    Public Property plt() As String
        Get
            Return (_plt)
        End Get
        Set(ByVal value As String)
            _plt = value
        End Set
    End Property

    Public Property ver() As String
        Get
            Return (_ver)
        End Get
        Set(ByVal value As String)
            _ver = value
        End Set
    End Property
    Public Property hor() As String
        Get
            Return (_hor)
        End Get
        Set(ByVal value As String)
            _hor = value
        End Set
    End Property
    Public Property flw() As String
        Get
            Return (_flw)
        End Get
        Set(ByVal value As String)
            _flw = value

        End Set

    End Property

    Public Property vis() As Integer
        Get
            Return (_vis)
        End Get
        Set(ByVal value As Integer)
            _vis = value
        End Set
    End Property

    Public Property atr() As String
        Get
            Return (_atr)
        End Get
        Set(ByVal value As String)
            _atr = value
        End Set
    End Property

    Public Property arrayEstructuraPlantilla() As String()
        Get
            Return (_arrayEstructuraPlantilla)
        End Get
        Set(ByVal value As String())
            _arrayEstructuraPlantilla = value
        End Set
    End Property
    Public Property arrayCampsPlantilla() As String()
        Get
            Return (_arrayCampsPlantilla)
        End Get
        Set(ByVal value As String())
            _arrayCampsPlantilla = value
        End Set
    End Property


    Public Property arrayCSSPlantilla() As String()

        Get
            Return (_arrayCSSPlantilla)
        End Get
        Set(ByVal value As String())
            _arrayCSSPlantilla = value
        End Set
    End Property
    Public Property arrayEnllaços() As String()
        Get
            Return (_arrayEnllaços)
        End Get
        Set(ByVal value As String())
            _arrayEnllaços = value
        End Set
    End Property
    Public Property arrayEsImatge() As String()
        Get
            Return (_arrayEsImatge)
        End Get
        Set(ByVal value As String())
            _arrayEsImatge = value
        End Set
    End Property

    Public Property arrayLCW() As String()
        Get
            Return (_arrayLCW)
        End Get
        Set(ByVal value As String())
            _arrayLCW = value
        End Set
    End Property

    Public Property arrayDSNUM() As String()
        Get
            Return (_arrayDSNUM)
        End Get
        Set(ByVal value As String())
            _arrayDSNUM = value
        End Set
    End Property

    Public Property arrayLC2() As String()
        Get
            Return (_arrayLC2)
        End Get
        Set(ByVal value As String())
            _arrayLC2 = value
        End Set
    End Property

    Public Property arrayALK() As String()
        Get
            Return (_arrayALK)
        End Get
        Set(ByVal value As String())
            _arrayALK = value
        End Set
    End Property

    Public Property arrayPAL() As String()
        Get
            Return (_arrayPAL)
        End Get
        Set(ByVal value As String())
            _arrayPAL = value
        End Set
    End Property

    Public Property arrayAAL() As String()
        Get
            Return (_arrayAAL)
        End Get
        Set(ByVal value As String())
            _arrayAAL = value
        End Set
    End Property


    Public Property arrayALF() As String()
        Get
            Return (_arrayALF)
        End Get
        Set(ByVal value As String())
            _arrayALF = value
        End Set
    End Property

    Public Property arrayNIV() As String()
        Get
            Return (_arrayNIV)
        End Get
        Set(ByVal value As String())
            _arrayNIV = value
        End Set
    End Property


    Public Property arrayALT() As String()
        Get
            Return (_arrayALT)
        End Get
        Set(ByVal value As String())
            _arrayALT = value
        End Set
    End Property
    Public Property arrayTCO() As String()
        Get
            Return (_arrayTCO)
        End Get
        Set(ByVal value As String())
            _arrayTCO = value
        End Set
    End Property

    Public Property arrayPLTDSPLT() As String()
        Get
            Return (_arrayPLTDSPLT)
        End Get
        Set(ByVal value As String())
            _arrayPLTDSPLT = value
        End Set
    End Property

    Public Property arrayAtr() As String()
        Get
            Return (_arrayAtr)
        End Get
        Set(ByVal value As String())
            _arrayAtr = value
        End Set
    End Property




    Public Sub bdget(ByVal objconn As OleDbConnection, ByVal pltinnod As Integer)

        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim trobat As Boolean


        If pltinnod <> 0 Then
            GAIA2.bdr(objconn, "SELECT * FROM METLPLT2 WITH(NOLOCK) where PLTINNOD=" & pltinnod, DS)
            If DS.Tables(0).Rows.Count > 0 Then
                trobat = True
                dbRow = DS.Tables(0).Rows(0)
                _innod = dbRow("PLTINNOD")

                _est = dbRow("PLTDSEST")
                _cmp = dbRow("PLTDSCMP")
                _css = dbRow("PLTDSCSS")
                _lnk = dbRow("PLTDSLNK")
                _img = dbRow("PLTDSIMG")
                _lcw = dbRow("PLTDSLCW")
                _num = dbRow("PLTDSNUM")
                _lc2 = dbRow("PLTDSLC2")
                _alk = dbRow("PLTDSALK")
                _pal = dbRow("PLTCDPAL")
                _aal = dbRow("PLTDSAAL")
                _alf = dbRow("PLTDSALF")
                _niv = dbRow("PLTDSNIV")
                _swalt = dbRow("PLTSWALT")
                _dsalt = dbRow("PLTDSALT")
                _tco = dbRow("PLTDSTCO")
                _plt = dbRow("PLTDSPLT")

                _flw = dbRow("PLTDSFLW")
                _vis = dbRow("PLTSWVIS")


                _arrayEstructuraPlantilla = Split(_est, ",")
                _arrayCampsPlantilla = Split(_cmp, ",")
                _arrayEnllaços = Split(_lnk, ",")
                _arrayEsImatge = Split(_img, ",")
                _arrayCSSPlantilla = Split(_css, "|")
                _arrayLCW = Split(_lcw, ",")
                _arrayDSNUM = Split(_num, ",")
                _arrayLC2 = Split(_lc2, ",")
                _arrayALK = Split(_alk, "|")
                _arrayALF = Split(_alf, ",")
                _arrayPAL = Split(_pal, ",")
                _arrayAAL = Split(_aal, "|")
                _arrayALT = Split(_dsalt, "|")
                _arrayTCO = Split(_tco, ",")
                _arrayNIV = Split(_niv, ",")
                _arrayPLTDSPLT = Split(_plt, ",")



            End If
        End If
        If Not trobat Or pltinnod = 0 Then
            _innod = 0
            _est = ""
            _cmp = ""
            _css = ""
            _lnk = ""
            _img = ""
            _lcw = ""
            _num = ""
            _lc2 = ""
            _alk = ""
            _pal = ""
            _aal = ""
            _alf = ""
            _niv = ""
            _swalt = 0

            _dsalt = ""
            _tco = ""
            _plt = ""

            _flw = ""
            _vis = 0

            'no faig els nothing dels array per que sempre validaré que _innod<>0
        End If
        DS.Dispose()
    End Sub


    Public Sub copiaObj(ByVal pla As clsPlantilla2)
        _innod = pla.innod
        _est = pla.est
        _cmp = pla.cmp
        _css = pla.css
        _lnk = pla.lnk
        _img = pla.img
        _lcw = pla.lcw
        _num = pla.num
        _lc2 = pla.lc2
        _alk = pla.alk
        _pal = pla.pal
        _aal = pla.aal
        _alf = pla.alf
        _niv = pla.niv
        _swalt = pla.swalt
        _dsalt = pla.dsalt
        _tco = pla.tco
        _plt = pla.plt

        _flw = pla.flw
        _vis = pla.vis


        _arrayEstructuraPlantilla = Split(pla.est, ",")
        _arrayCampsPlantilla = Split(pla.cmp, ",")
        _arrayEnllaços = Split(pla.lnk, ",")
        _arrayEsImatge = Split(pla.img, ",")
        _arrayCSSPlantilla = Split(pla.css, "|")
        _arrayLCW = Split(pla.lcw, ",")
        _arrayDSNUM = Split(pla.num, ",")
        _arrayLC2 = Split(pla.lc2, ",")
        _arrayALK = Split(pla.alk, "|")

        _arrayALF = Split(pla.alf, ",")
        _arrayPAL = Split(pla.pal, ",")
        _arrayAAL = Split(pla.aal, "|")
        _arrayALT = Split(pla.dsalt, "|")
        _arrayTCO = Split(pla.tco, ",")
        _arrayNIV = Split(pla.niv, ",")
        _arrayPLTDSPLT = Split(pla.plt, ",")

    End Sub
End Class