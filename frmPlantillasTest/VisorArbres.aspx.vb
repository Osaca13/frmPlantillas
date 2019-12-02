Imports System.Data
Imports Telerik.WebControls
Imports System.Data.OleDb

Public Class VisorArbres
    Inherits System.Web.UI.Page

    '**********************************************************************
    '**********************************************************************
    '			F R M V I S O R A R B R E S
    '**********************************************************************
    '**********************************************************************
    'Paràmetres d'entrada:
    ' PerCodi : si valor=1 mostrarà l'arbre a partir de la relació apuntada pel paràmetre arbre1
    ' arbre1: si percodi=1 relacionat amb l'anterior, si no s'informa percodi, s'obrirà l'arbre que tingui nom=arbre1
    ' c: nom del camp de nodes on es retornarà la llista de nodes seleccionats i amb sufix "txt" el camp on s'escriurà els noms dels camps
    ' nodesSeleccionats: llista de nodes, separats per comes, que es seleccionaran
    'separador= cadena amb el que es separarà la llista de nodes
    'res:  si valor="literal" es mostrarà el nom dels nodes seleccionats a "c"&"txt", amb salts de linea
    'nU: si valor=1 només es permetrà seleccionar un valor

    Dim novisibles As Integer = 0
    Public nif As String
    Public idUsuari As Integer
    Public objconn As OleDbConnection

    Protected WithEvents arbrePantalla1 As Telerik.WebControls.RadTreeView
    Protected WithEvents arbrePantalla2 As Telerik.WebControls.RadTreeView

    Public usuari As String = ""

    Private Sub Page_UnLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        GAIA.bdFi(objconn)
    End Sub 'Page_UnLoad


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim nodesSeleccionats As String = ""
        Dim nodesSeleccionats2 As String = ""

        'Faig aquest "invent" (per no dir chapuza) perque necessito una variable declarada amb "WithEvents" dins de visorArbreslite.aspx, però no pot ser radtree1 perque ja
        ' està declarada a visoarbres.aspx. 

        Dim rel As New clsRelacio
        Dim arbre1 As String = ""
        Dim arbre2 As String = ""
        Dim contingut As String = ""
        Dim nodosPerObrir(0) As String
        Dim numNodo As Integer
        arbrePantalla1 = RadTree1
        arbrePantalla2 = RadTree2


        objconn = GAIA.bdIni()

        'és necessari!!!
        Session("nif") = ""
        Session("codiOrg") = ""

        If HttpContext.Current.User.Identity.Name.Length > 0 Then
            Session("nif") = GAIA.nifUsuari(objconn, HttpContext.Current.User.Identity.Name).Trim()
            Session("CodiOrg") = GAIA.trobaNodeUsuari(objconn, Session("nif")).ToString().Trim()
            idUsuari = Session("CodiOrg")
        Else
            'idUsuari = "99999999"
            'Session("CodiOrg") = "99999999"
            idUsuari = "346231"
            Session("codiOrg") = "346231"
        End If

        If Session("codiOrg") = "0" Then 'reenvio a la portada de GAIA. No tinc codi d'usuari i no el puc deixar veure els arbres. Cal mostrar missatge!
            Response.Redirect("http://lhintranet/gdocs/232511_1.aspx")
        End If

        'idusuari = 49337
        'idusuari = 50319


        'session("codiOrg")=50319
        'idusuari=50319
        'session("nif")="44175980H".text = ""
        '    gaia.debug(nothing, "1_" & lblCodi.text)
        lblCodi.Text = ""
        'gaia.debug(nothing, "2_" &  lblCodi.text)

        If Not (Request("nv") Is Nothing) Then
            novisibles = 1
        Else
            novisibles = 0
        End If


        If Not Page.IsPostBack Then
            ' Carrego la llista de tipus de nodes        
            Dim element As ListItem
            If Not tipusNode Is Nothing Then
                carregaLlistaTipusNode("node", tipusNode, idUsuari)
                carregaLlistaTipusNode("fulla info", tipusNode, idUsuari)  'Teresa. 18-1-19
                carregaLlistaTipusNode("fulla noticia", tipusNode, idUsuari)  'Teresa. 18-1-19
                carregaLlistaTipusNode("fulla link", tipusNode, idUsuari)  'Teresa. 18-1-19
            End If


            If Not llistaArbres Is Nothing Then
                carregaLlistaTipusNode("arbre", llistaArbres, idUsuari)
            End If

            If Not llistaArbres2_1 Is Nothing Then
                'copio de l'arbre 2_1								
                For Each element In llistaArbres.Items
                    llistaArbres2_1.Items.Add(element)
                Next element
            End If
            If Not llistaArbres2_2 Is Nothing Then
                For Each element In llistaArbres2_1.Items
                    llistaArbres2_2.Items.Add(element)
                Next element
                'carregaLlistaTipusNode("arbre", llistaArbres2_2, idUsuari)
            End If

            If (Not Request("arbre1") Is Nothing) And (Request("arbre1") <> "permisos") Then
                'Si hi ha un node seleccionat, he d'obrir l'arbre on és
                arbre1 = Request("arbre1")

                If Request("perCodi") = "1" Then
                    rel.bdget(objconn, Request("arbre1"))
                Else
                    If llistaArbres2_1.SelectedItem.Value.Substring(0, 1) <> "#" Then
                        rel.bdget(objconn, llistaArbres2_1.SelectedItem.Value)
                    End If
                End If

                GAIA.generaArbre_NP(objconn, RadTree1, 1, idUsuari, rel, Request("arbre1"), 0, 0, 2, novisibles)

            End If

            'Continguts que provocan la publicació, pueden estar en cualquier arbol. Teresa 9-4-2019
            If (Not Request("contingut") Is Nothing) And (Request("contingut") <> "") Then
                If InStr(Request("contingut").ToString(), ",") > 1 Then
                    contingut = Request("contingut").Substring(0, InStr(Request("contingut").ToString(), ",") - 1)
                Else
                    contingut = Request("contingut").ToString()
                End If

                numNodo = Convert.ToInt32(contingut)
                rel.bdget(objconn, 0, numNodo)
                If rel.incod <> 0 Then
                    GAIA.buscaParesRel(objconn, rel.incod, nodosPerObrir)
                    rel.bdget(objconn, 0, Convert.ToInt32(nodosPerObrir(1)))
                    GAIA.generaArbre_NP(objconn, RadTree1, 1, idUsuari, rel, Request("arbre1"), 0, 0, 2, novisibles)
                Else
                    Response.Write("No existe el nodo " + contingut)
                End If
            End If

            If (Not Request("ca") Is Nothing) Then
                rel.bdget(objconn, Request("ca"))
                GAIA.generaArbre_NP(objconn, RadTree1, 1, idUsuari, rel, Request("arbre1"), 0, 0, 2, novisibles)
            End If

            If Not Request("arbre2") Is Nothing Then
                arbre2 = Request("arbre2")

                If Request("perCodi") = "1" Then
                    rel.bdget(objconn, Request("arbre2"))
                Else
                    rel.bdget(objconn, llistaArbres2_2.SelectedItem.Value)
                End If

                GAIA.generaArbre_NP(objconn, RadTree2, 2, idUsuari, rel, Request("arbre2"), 0, 0, 2, novisibles)

            End If

            If Not Request("nodesSeleccionats") Is Nothing Then

                nodesSeleccionats = Request("nodesSeleccionats").ToString()
                If nodesSeleccionats.Length > 1 Then
                    If nodesSeleccionats.Substring(nodesSeleccionats.Length - 1, 1) = "|" Then
                        nodesSeleccionats = nodesSeleccionats.Substring(0, nodesSeleccionats.Length - 1)
                    End If
                End If
                If nodesSeleccionats.Trim().Length > 0 Then
                    Try
                        If Request("trobaRelacio") = 1 Then
                            GAIA.cercarNodesPantallaByCodi(objconn, RadTree1, nodesSeleccionats, GAIA.llegirCodiArbrePantalla(objconn, RadTree1.Nodes.Item(0)), idUsuari, 1, 1)
                        Else
                            GAIA.cercarNodesPantallaByCodi(objconn, RadTree1, nodesSeleccionats, GAIA.llegirCodiArbrePantalla(objconn, RadTree1.Nodes.Item(0)), idUsuari, 1, 0)
                        End If
                    Catch
                    End Try
                End If
            End If
            If Not Request("nodesSeleccionats2") Is Nothing Then
                nodesSeleccionats2 = Request("nodesSeleccionats2").ToString()
                If nodesSeleccionats2.Length > 1 Then
                    If nodesSeleccionats2.Substring(nodesSeleccionats2.Length - 1, 1) = "|" Then
                        nodesSeleccionats2 = nodesSeleccionats2.Substring(0, nodesSeleccionats2.Length - 1)
                    End If
                End If
                If nodesSeleccionats2.Trim().Length > 0 Then
                    Try
                        If Request("trobaRelacio") = 1 Then
                            GAIA.cercarNodesPantallaByCodi(objconn, RadTree2, nodesSeleccionats2, GAIA.llegirCodiArbrePantalla(objconn, RadTree2.Nodes.Item(0)), idUsuari, 1, 1)
                        Else
                            GAIA.cercarNodesPantallaByCodi(objconn, RadTree2, nodesSeleccionats2, GAIA.llegirCodiArbrePantalla(objconn, RadTree2.Nodes.Item(0)), idUsuari, 1, 0)
                        End If
                    Catch
                    End Try
                End If
            End If

        Else
            'si puc faig una actualització dels arbres oberts
            If actualitzaNode.Text.Length > 0 Then
                clickActualitzar(arbrePantalla1, e, False)
                actualitzaNode.Text = ""
            End If

            If actualitzaNodeArbre2.Text.Length > 0 Then
                clickActualitzar(arbrePantalla2, e, False)
                actualitzaNodeArbre2.Text = ""
            End If


            'Per cada listbox poso els attributs de color
            Dim tmpLB As New ListBox
            If Not llistaArbres Is Nothing Then
                For Each element In llistaArbres.Items
                    If element.value.substring(0, 1) = "#" And element.value.length > 1 Then
                        element.Attributes.Add("style", "background-color:" & element.value & ";color:white;text-transform:uppercase")
                    End If
                    tmpLB.Items.Add(element)
                Next element
                llistaArbres.Items.Clear()
                For Each element In tmpLB.Items
                    llistaArbres.Items.Add(element)
                Next element
            End If
            tmpLB.Items.Clear()
            If Not llistaArbres2_1 Is Nothing Then
                'copio de l'arbre 2_1						

                For Each element In llistaArbres2_1.Items
                    If element.value.substring(0, 1) = "#" And element.value.length > 1 Then
                        element.Attributes.Add("style", "background-color:" & element.value & ";color:white;text-transform:uppercase")
                    End If
                    tmpLB.Items.Add(element)
                Next element
                llistaArbres2_1.Items.Clear()
                For Each element In tmpLB.Items
                    llistaArbres2_1.Items.Add(element)
                Next element
            End If
            tmpLB.Items.Clear()
            If Not llistaArbres2_2 Is Nothing Then
                For Each element In llistaArbres2_2.Items
                    If element.value.substring(0, 1) = "#" And element.value.length > 1 Then
                        element.Attributes.Add("style", "background-color:" & element.value & ";color:white;text-transform:uppercase")
                    End If
                    tmpLB.Items.Add(element)
                Next element
                llistaArbres2_2.Items.Clear()
                For Each element In tmpLB.Items
                    llistaArbres2_2.Items.Add(element)
                Next element
            End If
        End If

    End Sub 'Page_Load






    Protected Sub ContextClicked(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs)
        Dim nodo, arbre As String
        Dim codiRelacio As Integer

        nodo = e.NodeClicked.Value.Substring(0, e.NodeClicked.Value.IndexOf("-"))
        arbre = GAIA.llegirCodiArbrePantalla(objconn, e.NodeClicked)
        Dim permisHeretat As Integer

        Dim rel As New clsRelacio


        codiRelacio = 0
        If e.NodeClicked.Value.IndexOf("_") > 0 Then
            codiRelacio = e.NodeClicked.Value.Substring(e.NodeClicked.Value.IndexOf("_") + 1)
        End If

        'codiRelacio = GAIA.obtenirRelacioPantalla(selNode)
        rel.bdget(objconn, codiRelacio)

        Response.Write("<meta http-equiv=""Page-Enter"" content=""revealTrans(Duration=0,Transition=5)"">")
        Select Case e.ContextMenuItemText
            Case "Actualitzar"
                clickActualitzar(o, e, False)

            Case "Duplicar"

                GAIA.ferCopiaContingut(objconn, rel, idUsuari)
                clickActualitzar(o, e, False, rel.cdrsu)

            Case "Veure Tots"
                clickActualitzar(o, e, True)
            Case "Veure caducats"
                clickActualitzar(o, e, True, 0, True)
            Case "Editar"


                If clsPermisos.tepermis2(objconn, 3, idUsuari, idUsuari, rel, permisHeretat, "", "", 0) = 1 Or idUsuari = 297650 Then
                    Dim val1, val2 As Integer
                    val1 = 0
                    val2 = 0
                    If Not (RadTree1.SelectedNode() Is Nothing) Then
                        val1 = GAIA.obtenirRelacioPantalla(RadTree1.SelectedNode())
                    End If
                    If Not (RadTree2.SelectedNode() Is Nothing) Then
                        val2 = GAIA.obtenirRelacioPantalla(RadTree2.SelectedNode())
                    End If
                    Response.Write(GAIA.editarContingut(objconn, nodo, 1, val1, val2, codiRelacio))

                Else
                    Response.Write("<script language='javascript'>alert('L\'usuari " + idUsuari.ToString + " no té permisos per realitzar l\'acció seleccionada.');</script>")
                End If
            Case "Obrir"
                If clsPermisos.tepermis2(objconn, 7, idUsuari, idUsuari, rel, permisHeretat, "", "", 0) = 1 Or idUsuari = 297650 Then
                    Dim descTipus As String = String.Empty
                    GAIA.tipusNodebyNro(objconn, nodo, descTipus)
                    Select Case descTipus
                        Case "node missatge"
                            Response.Write("<script language=""javascript"">window.open(""/GAIA/aspx/missatges/visorNodeMissatges.aspx?codiRelacio=" + codiRelacio.ToString() + "& idioma=1"")</script>")
                        Case "node elMeuEspai"
                            Response.Write("<script language=""javascript"">window.open(""/GAIA/aspx/missatges/visorNodeMissatges.aspx?codiRelacio=" + codiRelacio.ToString() + "& idioma=1"")</script>")
                        Case "fulla web"
                            Response.Write("<script language=""javascript"">window.open(""" & GAIA.obtenirEnllacContingut(objconn, rel, 1) & """)</script>")
                        Case Else
                            Response.Write("<script language=""javascript"">window.open(""/GAIA/aspx/fulles/frmObrirFulla.aspx?codiRelacio=" + codiRelacio.ToString() + "& idioma=1"")</script>")
                    End Select
                Else
                    Response.Write("<script language='javascript'>alert('L\'usuari \'" + idUsuari.ToString() + "\' no té permisos per realitzar l\'acció seleccionada.');</script>")
                End If
            Case "Permisos"
                If clsPermisos.tepermis2(objconn, 1, idUsuari, idUsuari, rel, permisHeretat, "", "", 0) = 1 Or idUsuari = 297650 Then
                    Response.Write("<script language=""javascript"">window.open(""/GAIA/aspx/permisos/permisos.aspx?arbres=" & arbre.ToString() & "&nodes=" & nodo.ToString() & "&relincod=" & codiRelacio.ToString() & "&arbre1=organigrama usuarisGAIA"",""_blank"", ""location=0,height=500,width=426,scrollbars=yes,resizable=yes"");</script>")
                Else
                    Response.Write("<script language='javascript'>alert('L\'usuari \'" + idUsuari.ToString() + "\' no té permisos per realitzar l\'acció seleccionada.');</script>")
                End If
            Case "Esborrar"
                If clsPermisos.tepermis2(objconn, 5, idUsuari, idUsuari, rel, permisHeretat, "", "", 0) = 1 Then
                    clickEsborrarNode(o, e)
                Else

                    Response.Write("<script language='javascript'>alert('L\'usuari \'" + idUsuari.ToString() + "\' no té permisos per realitzar l\'acció seleccionada.');</script>")
                End If

            Case "Esborrar tots"
                If clsPermisos.tepermis2(objconn, 5, idUsuari, idUsuari, rel, permisHeretat, "", "", 0) = 1 Then
                    clickEsborrarNodeTots(o, e)
                Else

                    Response.Write("<script language='javascript'>alert('L\'usuari \'" + idUsuari.ToString() + "\' no té permisos per realitzar l\'acció seleccionada.');</script>")
                End If



            Case "Caducar"
                If clsPermisos.tepermis2(objconn, 5, idUsuari, idUsuari, rel, permisHeretat, "", "", 0) = 1 Then
                    clickCaducarNodeTots(o, e)
                Else

                    Response.Write("<script language='javascript'>alert('L\'usuari \'" + idUsuari.ToString() + "\' no té permisos per realitzar l\'acció seleccionada.');</script>")
                End If

            Case "Propietats"
                Response.Write("<script language=""javascript"">window.open(""/GAIA/aspx/propietats/propietats.aspx?relincod=" & codiRelacio.ToString() & """,""_blank"", ""location=0,height=410,width=820,scrollbars=yes,resizable=yes"");</script>")
            Case "Publicar"
                If clsPermisos.tepermis2(objconn, 7, idUsuari, idUsuari, rel, permisHeretat, "", "", 0) = 1 Or idUsuari = 297650 Then
                    Response.Write("<script language=""javascript"">window.open(""/GAIA/aspx/fulles/frmObrirFulla.aspx?codiRelacio=" & codiRelacio.ToString() & "&publicar=1&idioma=1"")</script>")
                Else
                    Response.Write("<script language='javascript'>alert('L\'usuari \'" + idUsuari.ToString() + "\' no té permisos per realitzar l\'acció seleccionada.');</script>")
                End If

        End Select
    End Sub




    Private Sub RadTree1_NodeExpand(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles arbrePantalla1.NodeExpand
        Dim nodo, arbre, txt
        Dim rel As New clsRelacio

        nodo = e.NodeClicked.Value.Substring(0, e.NodeClicked.Value.IndexOf("-"))
        arbre = GAIA.llegirCodiArbrePantalla(objconn, e.NodeClicked)

        If (e.NodeClicked.Nodes.Count = 0) Then
            GAIA.afegeixNodesFillsPantallaLlista_NP(objconn, nodo, e.NodeClicked, arbre, idUsuari, 1, 0, 0, 1, 0, novisibles)
        End If

    End Sub

    Private Sub RadTree2_NodeExpand(ByVal o As Object, ByVal e As Telerik.WebControls.RadTreeNodeEventArgs) Handles arbrePantalla2.NodeExpand
        Dim nodo, arbre, txt
        nodo = e.NodeClicked.Value.Substring(0, e.NodeClicked.Value.IndexOf("-"))
        arbre = GAIA.llegirCodiArbrePantalla(objconn, e.NodeClicked)
        If (e.NodeClicked.Nodes.Count = 0) Then
            GAIA.afegeixNodesFillsPantallaLlista_NP(objconn, nodo, e.NodeClicked, arbre, idUsuari, 2, 0, 0, 1, 0, novisibles)

        End If

    End Sub


    Protected Sub tractaDragDrop(ByVal sender As Object, ByVal NodeEvent As RadTreeNodeEventArgs)

        Dim sourceNode As RadTreeNode = NodeEvent.SourceDragNode
        Dim destNode As RadTreeNode = NodeEvent.DestDragNode
        Dim nodeFill As RadTreeNode
        Dim nroArbreOrigen, nroArbreDesti, nroNodeOrigen, nroNodeDesti As Integer
        Dim nodePathVell, nodePathNou As String
        Dim item As RadTreeNode
        Dim recursiu As Integer
        Dim nroNodePareAnterior, nroNodePareNou As Integer
        Dim strTmp As String = ""
        Dim codiRelacioOrigen, codiRelacioDesti As Integer
        Dim crida As String
        Dim rel As New clsRelacio

        crida = ""
        lblCodi.Text = ""
        Dim direccio As String


        If sourceNode.TreeViewParent Is RadTree1 Then
            If destNode.TreeViewParent Is RadTree1 Then
                'Dragrop dins del mateix arbre ->1
                direccio = "1"
            Else
                'Drag drop de l'arbre 1 a arbre 2
                direccio = ">"
            End If
        Else
            If destNode.TreeViewParent Is RadTree1 Then
                'Dragrop dins del mateix arbre -> 2
                direccio = "<"
            Else
                'Drag drop de l'arbre 2 a l'arbre 1
                direccio = "2"
            End If
        End If


        If (sourceNode.Parent Is Nothing) Then
            'No faig res perque el root no es pot moure				
        Else
            nroNodeOrigen = GAIA.obtenirNodePantalla(objconn, sourceNode)
            nroArbreOrigen = GAIA.llegirCodiArbrePantalla(objconn, sourceNode)
            nroNodeDesti = GAIA.obtenirNodePantalla(objconn, destNode)
            nroArbreDesti = GAIA.llegirCodiArbrePantalla(objconn, destNode)
            nroNodePareAnterior = sourceNode.Parent.Value.Substring(0, sourceNode.Parent.Value.IndexOf("-"))
            nodePathVell = GAIA.obtenirPathRelacioPantalla(objconn, sourceNode)

            strTmp = GAIA.obtenirPathRelacioPantalla(objconn, destNode)
            If strTmp = "" Then 'és el primer node de l'arbre
                nroNodePareNou = nroNodeDesti
            Else
                nroNodePareNou = strTmp.Substring(InStrRev(strTmp, "_"))
            End If

            nodePathNou = strTmp + "_" + nroNodeDesti.ToString()
            codiRelacioOrigen = GAIA.obtenirRelacioPantalla(sourceNode)

            codiRelacioDesti = GAIA.obtenirRelacioPantalla(destNode)
            rel.bdget(Nothing, codiRelacioDesti)
            'si el moviment es fa a un node "expandit" també haurà de poder decidir si vol insertar-ho al començament de la llista (moure i ordenar) o bé al final (insertar)	

            crida = "nroArbreOrigen=" + nroArbreOrigen.ToString() + "&nroArbreDesti=" + nroArbreDesti.ToString() + "&nroNodeOrigen=" + nroNodeOrigen.ToString() + "&nroNodeDesti=" + nroNodeDesti.ToString() + "&nodePathVell=" + nodePathVell.ToString() + "&nodePathNou=" + nodePathNou.ToString() + "&nroNodePareAnterior=" + nroNodePareAnterior.ToString() + "&codiRelacioOrigen=" + codiRelacioOrigen.ToString() + "&codiRelacioDesti=" + codiRelacioDesti.ToString() + "&moureFills=" + (Not sourceNode.Expanded).ToString() + "&dragDrop=1&direccio=" + direccio
            Dim esGAIACompro As Boolean = esGAIA2(objconn, nroNodeDesti)

            If esGAIACompro And rel.tipintip <> 8 And rel.tipintip <> 9 Then
                'lblCodi.Text += "<script>window.open(""http://lhintranet/GAIA/aspx/web/estructura.aspx?" + crida + """,""_blank"", ""location=0,height=650,width=500,scrollbars=yes,resizable=yes"");</script>"
                lblCodi.Text += "<script>window.open(""estructura.aspx?" + crida + """,""_blank"", ""location=0,height=650,width=500,scrollbars=yes,resizable=yes"");</script>"

            Else
                'lblCodi.Text += "<script>window.open(""web/frmEstructura.aspx?" + crida + """,""_blank"", ""location=0,height=650,width=500,scrollbars=yes,resizable=yes"");</script>"
                lblCodi.Text += "<script>window.open(""frmestructura.aspx?" + crida + """,""_blank"", ""location=0,height=650,width=500,scrollbars=yes,resizable=yes"");</script>"

            End If
            'Elimino el nodeOrigen de l'arbre origen
            sourceNode.Parent.Nodes.Remove(sourceNode)
        End If
        RadTree1.ClearSelectedNodes()

    End Sub 'tractaDragDrop

    Protected Sub clickEsborrarNode(ByVal sender As Object, ByVal e As EventArgs)
        Dim selNode As RadTreeNode
        Dim nodeFill As RadTreeNode
        Dim resultat As String
        Dim nroNode As Integer
        Dim nroNodePare As Integer
        Dim pathHerencia As String
        Dim codiRelacio As String
        Dim arbre As RadTreeView
        Dim objconn As OleDbConnection
        Dim rel As New clsRelacio
        Dim permisHeretat As Integer


        objconn = GAIA.bdIni()
        arbre = sender

        For Each selNode In arbre.SelectedNodes()
            If Not selNode.Parent Is Nothing Then 'comprovo que no esborrin un arbre
                nroNode = selNode.Value.Substring(0, selNode.Value.IndexOf("-"))
                nroNodePare = selNode.Parent.Value.Substring(0, selNode.Parent.Value.IndexOf("-"))
                pathHerencia = GAIA.obtenirPathRelacioPantalla(objconn, selNode)
                codiRelacio = GAIA.obtenirRelacioPantalla(selNode)
                rel.bdget(objconn, codiRelacio)
                'Si el node està expandit esborro només el node seleccionat, si no està expandit, he d'esborrar tots els fills!
                If selNode.Expanded = True And selNode.Nodes.Count > 0 Then
                    'Poso els fills al seu nivell a la pantalla
                    For Each nodeFill In selNode.Nodes
                        selNode.Parent.AddNode(nodeFill)
                    Next nodeFill

                    'No cal fer log, es fa en el GAIA.esborrarNode
                    '  GAIA.log(objConn, rel, session("codiOrg"), "", GAIA.TAESBORRAR)

                    resultat = GAIA.esborrarNode(objconn, 0, rel, 0, Session("codiOrg"), 1, GAIA.ctESBORRATMANUAL)
                Else

                    resultat = GAIA.esborrarNode(objconn, 0, rel, 0, Session("codiOrg"), 1, GAIA.ctESBORRATMANUAL)
                End If
                ' Esborro el node de la pantalla		
                arbre.SelectedNode().Remove()
            End If
        Next selNode

    End Sub 'clickEsborrarNode


    Protected Sub clickCaducarNodeTots(ByVal sender As Object, ByVal e As EventArgs)
        Dim selNode As RadTreeNode
        Dim nodeFill As RadTreeNode
        Dim resultat As String
        Dim nroNode As Integer
        Dim nroNodePare As Integer
        Dim pathHerencia As String
        Dim codiRelacio As String
        Dim arbre As RadTreeView
        Dim objconn As OleDbConnection
        Dim relVoid As New clsRelacio
        Dim permisHeretat As Integer


        objconn = GAIA.bdIni()
        arbre = sender

        For Each selNode In arbre.SelectedNodes()
            If Not selNode.Parent Is Nothing Then 'comprovo que no esborrin un arbre
                nroNode = selNode.Value.Substring(0, selNode.Value.IndexOf("-"))
                nroNodePare = selNode.Parent.Value.Substring(0, selNode.Parent.Value.IndexOf("-"))
                pathHerencia = GAIA.obtenirPathRelacioPantalla(objconn, selNode)

                'Si el node està expandit esborro només el node seleccionat, si no està expandit, he d'esborrar tots els fills!
                If selNode.Expanded = True And selNode.Nodes.Count > 0 Then
                    'Poso els fills al seu nivell a la pantalla
                    For Each nodeFill In selNode.Nodes
                        selNode.Parent.AddNode(nodeFill)
                    Next nodeFill

                    'No cal fer log, es fa en el GAIA.esborrarNode
                    '  GAIA.log(objConn, rel, session("codiOrg"), "", GAIA.TAESBORRAR)

                    resultat = GAIA.esborrarNode(objconn, nroNode, relVoid, 0, Session("codiOrg"), 1, GAIA.ctESBORRATCADUCAT)
                Else

                    resultat = GAIA.esborrarNode(objconn, nroNode, relVoid, 0, Session("codiOrg"), 1, GAIA.ctESBORRATCADUCAT)
                End If
                ' Esborro el node de la pantalla		
                arbre.SelectedNode().Remove()
            End If
        Next selNode

    End Sub 'clickCaducarNodeTots




    Protected Sub clickEsborrarNodeTots(ByVal sender As Object, ByVal e As EventArgs)
        Dim selNode As RadTreeNode
        Dim nodeFill As RadTreeNode
        Dim resultat As String
        Dim nroNode As Integer
        Dim nroNodePare As Integer
        Dim pathHerencia As String
        Dim codiRelacio As String
        Dim arbre As RadTreeView
        Dim objconn As OleDbConnection
        Dim relVoid As New clsRelacio
        Dim permisHeretat As Integer


        objconn = GAIA.bdIni()
        arbre = sender

        For Each selNode In arbre.SelectedNodes()
            If Not selNode.Parent Is Nothing Then 'comprovo que no esborrin un arbre
                nroNode = selNode.Value.Substring(0, selNode.Value.IndexOf("-"))
                nroNodePare = selNode.Parent.Value.Substring(0, selNode.Parent.Value.IndexOf("-"))
                pathHerencia = GAIA.obtenirPathRelacioPantalla(objconn, selNode)
                codiRelacio = GAIA.obtenirRelacioPantalla(selNode)

                'Si el node està expandit esborro només el node seleccionat, si no està expandit, he d'esborrar tots els fills!
                If selNode.Expanded = True And selNode.Nodes.Count > 0 Then
                    'Poso els fills al seu nivell a la pantalla
                    For Each nodeFill In selNode.Nodes
                        selNode.Parent.AddNode(nodeFill)
                    Next nodeFill
                    resultat = GAIA.esborrarNode(objconn, nroNode, relVoid, 0, Session("codiOrg"), 1, GAIA.ctESBORRATMANUAL)
                Else
                    resultat = GAIA.esborrarNode(objconn, nroNode, relVoid, 0, Session("codiOrg"), 1, GAIA.ctESBORRATMANUAL)
                End If
                ' Esborro el node de la pantalla		
                arbre.SelectedNode().Remove()
            End If
        Next selNode

    End Sub 'clickEsborrarNodeTots



    Protected Sub clickActualitzar(ByVal sender As Object, ByVal e As EventArgs, ByVal veureTots As Integer, Optional ByRef codiRelacio As Integer = 0, Optional ByRef veureCaducats As Boolean = False)
        Dim selNode As RadTreeNode
        Dim node, nodeNou As RadTreeNode
        Dim arbrePantalla As RadTreeView
        Dim index As Integer
        Dim nroFills As Integer = 0
        Dim rel As New clsRelacio
        objconn = GAIA.bdIni()
        arbrePantalla = sender

        If codiRelacio <> 0 Then
            selNode = GAIA.buscaRelacioArbrePantalla(objconn, arbrePantalla, codiRelacio)
        Else
            ' Si estem actualitzant la pantalla perque s'ha demanat des d'una altra pantalla 
            If actualitzaNodeArbre2.Text.Length > 0 Then
                selNode = GAIA.buscaRelacioArbrePantalla(objconn, arbrePantalla, actualitzaNodeArbre2.Text)

            Else
                If actualitzaNode.Text.Length > 0 Then
                    selNode = GAIA.buscaRelacioArbrePantalla(objconn, arbrePantalla, actualitzaNode.Text)

                End If
            End If
        End If
        'És una petició d'actualització manual
        If selNode Is Nothing Then
            selNode = arbrePantalla.SelectedNode()
        End If
        'Busco el nro. de fills
        Dim DS As DataSet
        DS = New DataSet()

        GAIA.bdr(objconn, "SELECT NODDSTXT FROM METLREL, METLNOD WHERE RELINCOD<>" & GAIA.obtenirRelacioPantalla(selNode).ToString() & " AND (" & " RELCDHER LIKE '" & GAIA.obtenirPathRelacioPantalla(objconn, selNode).ToString() & "_" & GAIA.obtenirNodePantalla(objconn, selNode).ToString() & "'" & ") AND RELINFIL=NODINNOD AND RELCDSIT<>99 ORDER BY RELCDORD asc", DS)


        If DS.Tables(0).Rows.Count > 0 Then
            nroFills = DS.Tables(0).Rows.Count.ToString()
        Else
            nroFills = 0
        End If
        DS.Dispose()


        If InStrRev(selNode.ToolTip, "(") > 0 And InStrRev(selNode.ToolTip, "(") > selNode.ToolTip.Length - 5 Then
            selNode.ToolTip = selNode.ToolTip.Substring(0, InStrRev(selNode.ToolTip, "(") - 1)
        End If
        If nroFills > 0 Then
            selNode.ToolTip += "(" + nroFills.ToString() + ")"
        End If


        rel.bdget(objconn, GAIA.obtenirRelacioPantalla(selNode))
        nodeNou = GAIA.creaNodePantalla(objconn, rel.noddstxt.Replace("<br />", ""), selNode.Value, rel.tipdsdes, "", 0, idUsuari, rel, rel.pcrincod, 0)

        index = arbrePantalla.GetAllNodes().IndexOf(selNode)
        Dim peers As RadTreeNodeCollection
        If (selNode.Parent Is Nothing) Then
            peers = arbrePantalla.Nodes
        Else
            peers = selNode.Parent.Nodes
        End If
        index = peers.IndexOf(selNode)



        'GAIA.afegeixNodesFillsPantallaLlista(objConn, nodeNou.Value.SubString(0, nodeNou.Value.IndexOf("-")), nodeNou, GAIA.llegirCodiArbrePantalla(objConn, nodeNou), idUsuari, 1, 0, 0, 1, veureTots, novisibles)
        GAIA.afegeixNodesFillsPantallaLlista_NP(objconn, nodeNou.Value.Substring(0, nodeNou.Value.IndexOf("-")), nodeNou, GAIA.llegirCodiArbrePantalla(objconn, nodeNou), idUsuari, 1, 0, 0, 1, veureTots, novisibles, veureCaducats)

        nodeNou.Expanded = True
        nodeNou.Selected = True
        selNode.Remove()
        peers.Insert(index, nodeNou)
        actualitzaNodeArbre2.Text = ""
        actualitzaNode.Text = ""

    End Sub 'clickActualitzar



    Protected Sub clickCercarNode(ByVal sender As Object, ByVal e As EventArgs)
        Dim arbre As RadTreeView
        Dim objconn As OleDbConnection
        Dim selNode As RadTreeNode
        Dim resultat As String = ""

        objconn = GAIA.bdIni()

        If UCase(arbreTreeView.Text) = "RADTREE2" Then
            arbre = RadTree2
        Else
            arbre = RadTree1
        End If

        selNode = arbre.SelectedNode()
        resultat = GAIA.cercarNodeByTxt(objconn, arbre, cerca.Text, GAIA.llegirCodiArbrePantalla(objconn, arbre.Nodes.Item(0)), idUsuari, 1, GAIA.obtenirPathRelacioPantalla(objconn, selNode) + "_" + GAIA.obtenirNodePantalla(objconn, selNode))
        If resultat.Length > 0 Then

            Response.Write("<script language=""javascript"">alert(""" + resultat + """)</script>")
        End If

        'Public Shared FUNCTION  cercarNodeByTxt(byVal objConn as OleDbConnection,radtree1 As RadTreeView ,text as string, codiArbre as integer, idusuari as integer, byval nroArbre as integer) 	
    End Sub 'clickCercarNode


    Protected Sub clickRetornarNodes(ByVal sender As Object, ByVal e As EventArgs)

        Dim nodes As String
        Dim nroNodes As String
        Dim treenode As RadTreeNode
        Dim nouNroNode As String
        Dim index As Integer
        Dim arbre As RadTreeView
        Dim separador As String
        Dim cont As Integer = 0
        Dim pos As Integer = 0
        Dim strNouNode As String = ""
        If Request("separador") <> "" Then
            separador = Request("separador")
        Else
            separador = ","
        End If
        arbre = RadTree1
        nodes = ""
        nroNodes = ""
        For Each treenode In arbre.CheckedNodes
            If Request("trobaRelacio") = 1 Then

                nouNroNode = GAIA.obtenirRelacioPantalla(treenode).ToString()
            Else

                nouNroNode = GAIA.obtenirNodePantalla(objconn, treenode).ToString()

            End If

            index = nroNodes.IndexOf(separador + nouNroNode.ToString() + separador)
            If index < 0 Then 'No el tenia a la llista
                If nodes.Length > 0 Then


                    If Request("res") = "literal" Then
                        nodes = nodes & "\n"
                    Else
                        nodes = nodes & ","
                    End If

                End If
                strNouNode = GAIA.netejaHTML(HttpUtility.HtmlDecode(treenode.Text.Trim()))
                'Elimino els números entre parèntesi que es trobin al final, donat pel nombre de nodes que pengen
                pos = InStr(strNouNode, "(")


                If pos > 5 And strNouNode.Length < pos + 5 Then
                    strNouNode = strNouNode.Substring(0, pos - 1)
                End If
                nodes &= strNouNode

                If nroNodes.Length > 0 Then nroNodes &= separador
                If Request("trobaRelacio") = 1 Then

                    nroNodes &= GAIA.obtenirRelacioPantalla(treenode).ToString().Trim()
                Else
                    nroNodes &= GAIA.obtenirNodePantalla(objconn, treenode).ToString().Trim()
                End If

            End If
            cont = cont + 1
        Next treenode

        If nroNodes.Length > 0 Then

            nroNodes = nroNodes.Replace(separador + separador, separador) 'canvio les ,, per ,
        End If


        Response.Write("<script language=""javascript"">opener.document.getElementById(""" + Request("c") + "Txt"").value =""" & nodes.Replace("""", "\""") + """; opener.document.getElementById(""" + Request("c") + "Txt"").style.height ='" & (cont * 30) & "px';opener.document.getElementById(""" & Request("c") & "Nodes"").value =""" & nroNodes.ToString() & """;window.close();</script>")


    End Sub 'clickRetornarNodes




    Protected Sub clickInsertarNode(ByVal sender As Object, ByVal e As EventArgs)
        Dim selNode As RadTreeNode
        Dim nodeFill As RadTreeNode
        Dim resultat As String
        Dim nroArbre As Integer
        Dim nroNodePare As Integer
        Dim tipus As Integer
        Dim descTipus As String = String.Empty

        Dim nodePath As String
        Dim codiRelacio, codiRelacioPare As Integer
        Dim arbre As RadTreeView
        Dim relPare As New clsRelacio
        Dim rel As New clsRelacio

        If UCase(arbreTreeView.Text) = "RADTREE1" Then
            arbre = RadTree1
        Else
            arbre = RadTree2
        End If

        selNode = arbre.SelectedNode()
        nroNodePare = selNode.Value.Substring(0, selNode.Value.IndexOf("-"))
        nroArbre = GAIA.llegirCodiArbrePantalla(objconn, selNode)

        codiRelacioPare = GAIA.obtenirRelacioPantalla(selNode)
        relPare.bdget(objconn, codiRelacioPare)
        Dim node As String
        node = GAIA.insertarNode(objconn, tipusNode.SelectedItem.Value, noddstxt.Text.Replace("<br />", ""), Session("codiOrg"))
        tipus = GAIA.tipusNodebyNro(objconn, node, descTipus)


        nodePath = GAIA.obtenirPathRelacioPantalla(objconn, selNode)
        If node > 0 Then

            Dim cellaAutolink As Integer 'No l'utilitzo per res, però la funció assiganAutomaticamentCella la necessita per referència. 

            rel = GAIA.creaRelacio(objconn, nroArbre, nroNodePare, node, 0, nodePath + "_" + nroNodePare.ToString(), GAIA.assignaAutomaticamentCella(objconn, tipus, relPare, 1, 1, cellaAutolink, 0), 1, -1, 1, False, Session("codiOrg"))
            codiRelacio = rel.incod
            GAIA.log(objconn, rel, Session("codiOrg"), "", GAIA.TAINSERTAR)
            clsPermisos.gravaPermis(objconn, 1, Session("codiOrg"), 0, 0, rel)
            '        clsPermisos.assignaPermisLlistarNode(objconn, 0, rel)
        Else
            'escriuError("No s'ha pogut crear el node")
        End If
        'Si es un node que té una taula associada he de fer un "insert" del registre en blanc
        Select Case descTipus
            Case "node web"
                'Si es GAIA2 que inserte en otra tabla: 
                If esGAIA2(objconn, nroNodePare) Then
                    GAIA.bdSR(objconn, "INSERT INTO METLNWE2 (NWEINNOD, NWEINIDI,NWEDSTIT, NWEDSUSR, NWEDSTCO, NWEDSLCW, NWEDSHTM) VALUES (" + node.ToString() + ",1,'" + noddstxt.Text.ToString().Replace("'", "''").Replace("<br />", "") + "','" + Session("codiOrg") + "','','', '<div class=""contenidor border border-secondary p-2 pr-4 pl-4""><span class=""contenidorAtributs"" style=""display:none"">###########################|</span><div class=""row border border-secondary p-2""><span class=""rowAtributs"" style=""display:none"">###########################|</span><div class=""col cel border border-secondary p-2"" id=""d0""><span class=""divId"" style=""display:none"">0</span><span class=""divImg""></span><span class=""text"">Cel&middot;la inicial</span><span class=""atributs"" style=""display:none"">0#Cel&middot;la inicial##########################|</span></div></div></div> ')")
                Else
                    GAIA.bdSR(objconn, "INSERT INTO METLNWE (NWEINNOD, NWEINIDI,NWEDSTIT, NWEDSUSR,NWEDSTHOR, NWEDSTVER, NWEDSTCO, NWEDSEST, NWEDSATR, NWEDSLCW, NWEDSCAR) VALUES (" + node.ToString() + ",1,'" + noddstxt.Text.ToString().Replace("'", "''").Replace("<br />", "") + "','" + Session("codiOrg") + "','100','100','','0000','ih','','')")
                End If
            Case "node catalegServeis"
                GAIA.bdSR(objconn, "INSERT INTO CSTLNCS  (NCSINCOD,NCSINIDI,NCSDSNOM) VALUES (" + node.ToString() + ",1,'" + noddstxt.Text.ToString().Replace("'", "''").Replace("<br />", "") + "')")
            Case "node document"
                GAIA.bdSR(objconn, "INSERT INTO METLNDO  (NDOINNOD ,NDOINIDI ,NDODSTIT,NDODSUSR) VALUES (" + node.ToString() + ",1,'" + noddstxt.Text.ToString().Replace("'", "''").Replace("<br />", "") + "','" + Session("codiOrg") + "')")
            Case "node codificacio"
                GAIA.bdSR(objconn, "INSERT INTO METLNCO (NCOINNOD, NCOINIDI,NCODSTIT,NCODSUSR) VALUES (" + node.ToString() + ",1,'" + noddstxt.Text.ToString().Replace("'", "''").Replace("<br />", "") + "','" + Session("codiOrg") + "')")

            Case "fulla info"
                GAIA.bdSR(objconn, "INSERT INTO METLINF VALUES (" & node.ToString() & ",1, '" & noddstxt.Text.ToString().Replace("'", "''").Replace("<br />", "") & "','','','', getdate(), '01/01/1900', '01/01/1900', '" & Session("codiOrg") & "','','','','',0,0,'','','01/01/1900','',0,'','','01/01/1900','' )")

            Case "fulla noticia"
                GAIA.bdSR(objconn, "INSERT INTO METLNOT VALUES (" & node.ToString() & ",1, '" & noddstxt.Text.ToString().Replace("'", "''").Replace("<br />", "") & "','','','', getdate(), '01/01/1900', '01/01/1900', '" & Session("codiOrg") & "','','','0','','')")

            Case "fulla link"
                GAIA.bdSR(objconn, "INSERT INTO METLLNK VALUES (" & node.ToString() & ",1, '" & noddstxt.Text.ToString().Replace("'", "''").Replace("<br />", "") & "','',0,'01/01/1900', '01/01/1900', '" & Session("codiOrg") & "',null,0,'','')")

        End Select

        'Inserto el node a l'arbre que hi ha a la pantalla
        ' Si el node ja està expandit poso el nou node
        If selNode.Expanded = True Then
            Dim nodePantalla As RadTreeNode = GAIA.creaNodePantalla(objconn, noddstxt.Text.Replace("<br />", ""), node.ToString() & "-" & nroArbre.ToString() & "_" & codiRelacio.ToString(), descTipus.Trim, "", 0, idUsuari, rel, 0, 0)
            selNode.AddNode(nodePantalla)
        End If

    End Sub 'ClickInsertarNode

    Private Function esGAIA2(ByVal objConn As OleDbConnection, ByVal nroNode As Integer) As Boolean
        Dim resultado As Boolean = False
        Dim ds As New DataSet
        Dim query As String

        query = "SELECT AWEINNOD FROM METLAWE2 WHERE AWEINNOD=" & nroNode & " UNION SELECT NWEINNOD FROM METLNWE2 WHERE NWEINNOD=" & nroNode & " UNION SELECT WEBINNOD FROM METLWEB2 WHERE WEBINNOD=" & nroNode & ""
        GAIA.bdr(objConn, query, ds)

        If ds.Tables(0).Rows.Count > 0 Then
            resultado = True
        End If

        Return resultado
    End Function

    Protected Function plantillaGAIA2(ByVal objConn As OleDbConnection, ByVal nroNode As Integer) As Boolean
        Dim resultado As Boolean = False
        Dim ds As New DataSet
        Dim query As String

        query = "SELECT RELINCOD FROM METLREL WHERE RELINFIL=" & nroNode & " AND RELCDHER LIKE '%298072%'"
        GAIA.bdr(objConn, query, ds)

        If ds.Tables(0).Rows.Count > 0 Then
            resultado = True
        End If

        Return resultado
    End Function

    Protected Sub llistaArbres_canviArbre(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim objconn As OleDbConnection
        Dim rel As New clsRelacio
        objconn = GAIA.bdIni()
        If llistaArbres.SelectedItem.Value.Substring(0, 1) <> "#" Then
            rel.bdget(objconn, llistaArbres.SelectedItem.Value)
            GAIA.generaArbre_NP(objconn, RadTree1, 1, idUsuari, rel, 0, 0, 0, 2, novisibles)

        End If
    End Sub
    Protected Sub llistaArbres2_1_canviArbre(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim rel As New clsRelacio
        If llistaArbres2_1.SelectedItem.Value.Substring(0, 1) <> "#" Then
            rel.bdget(objconn, llistaArbres2_1.SelectedItem.Value)
            GAIA.generaArbre_NP(objconn, RadTree2, 2, idUsuari, rel, 0, 0, 0, 2, novisibles)
        End If
    End Sub
    Protected Sub llistaArbres2_2_canviArbre(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim rel As New clsRelacio
        If llistaArbres2_2.SelectedItem.Value.Substring(0, 1) <> "#" Then
            rel.bdget(objconn, llistaArbres2_2.SelectedItem.Value)
            GAIA.generaArbre_NP(objconn, RadTree2, 2, idUsuari, rel, 0, 0, 0, 2, novisibles)
        End If
    End Sub

    Protected Sub clickCopiarNode(ByVal sender As Object, ByVal e As EventArgs)

        Dim sourceNode As RadTreeNode
        Dim destNode As RadTreeNode

        Dim node, posicioEstructura As Integer
        Dim codiRelacioOrigen, codiRelacioDesti As String
        Dim nroArbre As Integer

        Dim nroNodeOrigen, nroNodeDesti, nroNodePareAnterior As Integer
        Dim llistaNodesPares(0) As String
        Dim recursiu As Integer
        Dim nodePath, nodePathOrigen, nodePathDesti As String
        Dim item As RadTreeNode

        Dim nodesOrg(), permisos() As Integer
        Dim nroNodesOrg, cont, cont2 As Integer

        sourceNode = RadTree1.SelectedNode()
        destNode = RadTree2.SelectedNode()
        If sourceNode Is Nothing Or destNode Is Nothing Then
            'Error
            If sourceNode Is Nothing Then
                missatgeError("ERROR: Cal seleccionar el node origen")
            End If
            If destNode Is Nothing Then
                missatgeError("ERROR: Cal seleccionar el node destí")
            End If
        Else
            nodePathOrigen = GAIA.obtenirPathRelacioPantalla(objconn, sourceNode)

            nroNodePareAnterior = sourceNode.Parent.Value.Substring(0, sourceNode.Parent.Value.IndexOf("-"))
            nroNodeOrigen = GAIA.obtenirNodePantalla(objconn, sourceNode)
            nroNodeDesti = GAIA.obtenirNodePantalla(objconn, destNode)
            nodePathDesti = GAIA.obtenirPathRelacioPantalla(objconn, destNode) + "_" + nroNodeDesti.ToString()
            nroArbre = GAIA.llegirCodiArbrePantalla(objconn, destNode)
            codiRelacioOrigen = GAIA.obtenirRelacioPantalla(sourceNode)
            codiRelacioDesti = GAIA.obtenirRelacioPantalla(destNode)

            Dim rel As New clsRelacio
            rel.bdget(Nothing, codiRelacioOrigen)
            'Faig una cerca per veure si la copia del node provoca recursivitat
            recursiu = False
            If nroNodeOrigen = nroNodeDesti Then
                recursiu = True
            Else
                GAIA.buscaParesRel(objconn, codiRelacioDesti, llistaNodesPares)
                For Each node In llistaNodesPares
                    If node = nroNodeOrigen Then
                        recursiu = True
                    End If
                Next node
            End If
            If recursiu = False Then
                If (sourceNode.Parent Is Nothing) Then
                    missatgeError("ERROR: Els nodes de tipus arbre\nno es poden moure")
                Else 'Faig la copia
                    Dim crida As String

                    'Preparo la crida a frmEstructura.aspx perque faci la copia i demani a l'usuari la informació
                    'que cal per fer la copia
                    crida = "nroArbreOrigen=" + GAIA.llegirCodiArbrePantalla(objconn, sourceNode).ToString() + "&nroArbreDesti=" + GAIA.llegirCodiArbrePantalla(objconn, destNode).ToString() + "&nroNodeOrigen=" + nroNodeOrigen.ToString() + "&nroNodeDesti=" + nroNodeDesti.ToString() + "&nodePathVell=" + nodePathOrigen.ToString() + "&nodePathNou=" + nodePathDesti.ToString() + "&nroNodePareAnterior=" + nroNodePareAnterior.ToString() + "&codiRelacioOrigen=" + codiRelacioOrigen.ToString() + "&codiRelacioDesti=" + codiRelacioDesti.ToString() + "&moureFills=" + (Not sourceNode.Expanded).ToString() + "&dragDrop=0&direccio=2"
                    'Teresa: incluyo la posibilidad de GAIA2

                    If esGAIA2(objconn, nroNodeDesti) And rel.tipintip <> 8 And rel.tipintip <> 9 Then
                        lblCodi.Text += "<script>window.open(""web/estructura.aspx?" + crida + """,""_blank"", ""location=0,height=650,width=500,scrollbars=yes,resizable=yes"");</script>"
                    Else
                        lblCodi.Text += "<script>window.open(""web/frmEstructura.aspx?" + crida + """,""_blank"", ""location=0,height=650,width=500,scrollbars=yes,resizable=yes"");</script>"
                    End If
                End If
            Else 'recursiu				
                missatgeError("ERROR: relació recursiva\nEl node no s'ha copiat")
            End If
        End If
    End Sub 'clickCopiarNode

    '************************************************************************************************************
    '	Funció: carregaLlistaTipusNode
    '					nodeOrg: codi organigrama de l'usuari al que hem de mostrar la info.
    '	Sortida:  carrega les dades del dropdownlist
    '************************************************************************************************************
    Protected Function carregaLlistaTipusNode(ByVal desc As String, ByVal camp As DropDownList, ByVal nodeOrg As Integer)

        Dim DS As DataSet
        Dim dbRow As DataRow
        DS = New DataSet()
        Dim element As ListItem
        If desc = "arbre" Then
            Dim valorAnt, contMateixTipus As Integer
            Dim codiTMP As Integer
            Dim textTMP As String
            Dim tipusTmp As String
            Dim permisHeretat As Integer
            valorAnt = 0
            contMateixTipus = 0
            codiTMP = 0
            tipusTmp = ""
            textTMP = ""
            Dim rel As New clsRelacio

            element = New ListItem("selecciona arbre", "#")
            camp.Items.Add(element)


            Dim tipusPermis As Integer = 9
            Dim llistaPendents As String = ""
            Dim llistaAmbPermisos As String = ""
            Dim llistaSensePermisos As String = ""
            GAIA.bdr(Nothing, "SELECT TIPINTIP, TIPDSDES,SUBSTRING(NODDSTXT, 1, 25) AS NODDSTXT,RELINCOD FROM METLREL WITH(NOLOCK), METLTIP WITH(NOLOCK), METLNOD WITH(NOLOCK) WHERE RELCDSIT<98 AND RELINFIL = RELINPAR AND TIPINTIP = RELCDARB AND NODINNOD = RELINPAR  ORDER BY CASE TIPINTIP when 14 THEN 1 when 11 then 2 WHEN 46 then 3 WHEN 8 then 5 else  TIPINTIP END, NODDSTXT", DS)
            For Each dbRow In DS.Tables(0).Rows

                clsPermisos.afegirElement(dbRow("RELINCOD"), llistaPendents)
            Next dbRow

            clsPermisos.trobaPermisLlistaRelacions(Nothing, tipusPermis, llistaPendents, idUsuari, "", "", llistaAmbPermisos, llistaSensePermisos, "")

            Dim codiColorFort As String = ""
            Dim colorFort As String = ""
            Dim colorFluix As String = ""

            Dim strLlista As String = ""
            Dim cont As Integer = 0
            For Each dbRow In DS.Tables(0).Rows
                If clsPermisos.existeixElement(dbRow("RELINCOD"), llistaAmbPermisos) Then
                    If valorAnt <> dbRow("TIPINTIP") And tipusTmp <> "" Then



                        'tracto llista
                        tractaLlistaArbres(camp, strLlista, valorAnt, tipusTmp)
                        strLlista = ""
                    End If
                    clsPermisos.afegirElement(dbRow("RELINCOD") & "|" & dbRow("NODDSTXT"), strLlista)

                    tipusTmp = dbRow("TIPDSDES")
                    valorAnt = dbRow("TIPINTIP")
                End If
            Next dbRow

            'si hi ha elements tracto darrer
            tractaLlistaArbres(camp, strLlista, valorAnt, tipusTmp)
        Else
            GAIA.bdr(Nothing, "SELECT TIPCDVER, isnull(RELINPAR,0) as RELINPAR, TIPINTIP, REPLACE(TIPDSDES,'node','carpeta') AS TIPDSDES, RELINPAR FROM METLTIP WITH(NOLOCK)  LEFT OUTER JOIN METLREL WITH(NOLOCK) ON  RELINFIL=" & idUsuari & " AND (RELINPAR=125337 OR RELINPAR=57821 OR RELINPAR=127716 ) WHERE TIPDSDES LIKE '" & desc & "%' AND TIPCDVER>-3 ", DS)
            For Each dbRow In DS.Tables(0).Rows
                If dbRow("RELINPAR") = 125337 Then 'unitat web
                    camp.Items.Add(New ListItem(dbRow("TIPDSDES"), dbRow("TIPINTIP")))
                Else
                    If (dbRow("RELINPAR") = 57821 Or dbRow("RELINPAR") = 127716) And dbRow("TIPCDVER") > -2 Then  'OAC i organització
                        camp.Items.Add(New ListItem(dbRow("TIPDSDES"), dbRow("TIPINTIP")))
                    Else
                        If dbRow("RELINPAR") = 0 And dbRow("TIPCDVER") > -1 Then 'resta de grups						
                            camp.Items.Add(New ListItem(dbRow("TIPDSDES"), dbRow("TIPINTIP")))
                        End If
                    End If
                End If
            Next dbRow
        End If
        DS.Dispose()
        Return DS

    End Function

    Public Shared Function tractaLlistaArbres(ByVal camp As DropDownList, ByVal strllista As String, ByVal tipus As Integer, ByVal nomTipus As String)
        Dim element As ListItem

        Dim codiColorFort As String = ""
        Dim colorFort As String = ""
        Dim colorFluix As String = ""
        Dim nomArbre As String = ""
        Dim codiArbre As String = ""
        Dim cont As Integer = 0
        Dim arrLlista As String()

        arrLlista = strllista.Split(",")

        seleccionaEstil(tipus, colorFluix, colorFort, codiColorFort)

        For Each item As String In arrLlista
            If item.Length > 0 Then
                codiArbre = item.Split("|")(0)
                nomArbre = item.Split("|")(1)
                If cont = 0 And arrLlista.Length > 1 Then
                    'poso titol
                    element = New ListItem((nomTipus.Replace("arbre", "")).Trim(), codiColorFort)
                    element.Attributes.Add("style", colorFort)
                    camp.Items.Add(element)
                End If
                'poso element
                'element = New listItem(iif(arrLlista.length>1,(Server.HtmlDecode("&nbsp;") & Server.HtmlDecode("&nbsp;"))","")  & (  nomArbre.replace("arbre", "").trim()), codiArbre)

                element = New ListItem(IIf(arrLlista.Length > 1, System.Web.HttpContext.Current.Server.HtmlDecode("&nbsp;") & System.Web.HttpContext.Current.Server.HtmlDecode("&nbsp;"), "") & nomArbre.Replace("arbre", "").Trim(), codiArbre)
                element.Attributes.Add("style", colorFluix)
                camp.Items.Add(element)
                cont = cont + 1
            End If
        Next item

        Return cont

    End Function

    Public Shared Sub seleccionaEstil(ByVal tipus As Integer, ByRef colorFluix As String, ByRef colorFort As String, ByRef codiColorFort As String)
        Select Case tipus
            Case 14 'el meu espai
                colorFort = "background-color:#000000;color:white;text-transform:uppercase"
                codiColorFort = "#000000"
            Case 11 'intranet
                colorFort = "background-color:#3d678d; color:white;text-transform:uppercase"
                codiColorFort = "#3d678d"
            'colorFluix="background-color:#E49AB0 "
            Case 46 'codificació
                colorFort = "background-color:#a87f01; color:white;text-transform:uppercase"
                codiColorFort = "#a87f01"
            'colorFluix="background-color:#FFC971 "
            Case 8 'webs
                colorFort = "background-color:#0194a8; color:white;text-transform:uppercase"
                codiColorFort = "#0194a8"
                'colorFluix="background-color:#ABABAB "
            Case Else 'admin
                colorFort = "background-color:#be4307; color:white;text-transform:uppercase"
                codiColorFort = "#be4307"
                'colorFluix="background-color:#FFC971 "
        End Select
    End Sub

    '******************************************************************
    '	Sub: missatgeError
    '	Entrada: 
    '		text : descripció de l'error
    '	Procés: 	
    '		Escriu un javascript amb un alert(text);
    '******************************************************************
    Protected Sub missatgeError(ByRef text)
        Response.Write("<script>alert('" & text.ToString().Replace("'", "´") & "');</script>")
    End Sub 'MissatgeError


#Region "Web Form Designer generated code"
    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        InitializeComponent()
        MyBase.OnInit(e)
    End Sub 'OnInit
    Private Sub InitializeComponent()
    End Sub 'InitializeComponent   



    Private Sub Page_PreRender(sender As Object, e As EventArgs)

        RadTree1.Attributes.Add("OnClick", "client_OnTreeNodeChecked(event)")
    End Sub
#End Region

End Class