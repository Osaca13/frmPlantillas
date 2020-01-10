Imports System.Data
Imports System.Data.OleDb

Public Class frmArbreWeb
    Inherits System.Web.UI.Page

    '**********************************************************************
    '**********************************************************************
    '			F R M A R B R E W E B
    '**********************************************************************
    '**********************************************************************

    Public Shared nif As String
    Public Shared objconn As OleDbConnection

    Private Sub Page_UnLoad(sender As Object, e As System.EventArgs) Handles MyBase.Unload
        GAIA.bdFi(objconn)
    End Sub 'Page_UnLoad

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load



        Dim DS As DataSet
        Dim dbRow As DataRow
        Dim estructura As String
        Dim atributs As String
        Dim arrayEstructura As String()
        Dim arrayBuit As String()
        estructura = ""
        atributs = ""

        objconn = GAIA.bdIni()

        'If HttpContext.Current.User.Identity.Name.Length > 0 Then
        '    If (Session("nif") Is Nothing) Then
        '        Session("nif") = GAIA.nifUsuari(objconn, HttpContext.Current.User.Identity.Name).Trim()
        '    End If
        '    If Session("codiOrg") Is Nothing Then
        '        Session("CodiOrg") = GAIA.trobaNodeUsuari(objconn, Session("nif")).ToString().Trim()
        '    End If
        'End If
        ' idUsuari = "346231"
        Session("codiOrg") = "346231"

        If Session("codiOrg") = 49730 Or Session("codiOrg") = 49727 Or Session("codiOrg") = 56935 Or Session("codiOrg") = 80879 Or Session("codiOrg") = 297650 Or Session("codiOrg") = 313486 Or Session("codiOrg") = 346231 Then

            If Not Page.IsPostBack Then
                inicialitzaLListaServidors()
                If Not Request("id") Is Nothing Then
                    DS = New DataSet()
                    GAIA.bdr(objconn, "SELECT * FROM METLAWE WHERE AWEINNOD=" & Request("id").ToString(), DS)
                    If DS.Tables(0).Rows.Count = 0 Then
                        ' Response.Redirect("http://lhintranet/gaia2/aspx/web/frmplantilla.aspx?id=" & Request("id") & "&tipus=A")
                        Response.Redirect("~/GAIA2/aspx/web/frmplantilla.aspx?id=" & Request("id") & "&tipus=A")
                    Else
                        dbRow = DS.Tables(0).Rows(0)
                        AWEDSTIT.Text = dbRow("AWEDSTIT")
                        AWEDSNOM.Text = dbRow("AWEDSNOM")
                        AWEDSROT.Text = dbRow("AWEDSROT")
                        AWEDSHOR.Text = dbRow("AWEDSHOR")
                        AWEDSVER.Text = dbRow("AWEDSVER")
                        AWEDSDOC.Text = dbRow("AWEDSDOC")
                        AWEDSEBO.Text = dbRow("AWEDSEBO")
                        AWEDSMET.Text = dbRow("AWEDSMET")
                        AWEDSPEU.Text = dbRow("AWEDSPEU")
                        AWEDSCSP.Text = dbRow("AWEDSCSP")
                        AWEDSCSI.Text = dbRow("AWEDSCSI")


                        lstAWEDSSER.Items.FindByValue(dbRow("AWEDSSER")).Selected = True

                        lblIdioma.Text = "<select name=""idioma"" >" & GAIA.llistaIdiomes(objconn, dbRow("AWEINIDI")) & "</select>"
                        lblCodi.Text = "<script>document.getElementById('estructura').value='" & dbRow("AWEDSEST").ToString() & "'; document.getElementById('atributs').value='" & dbRow("AWEDSATR").ToString() & "';document.getElementById('llistaTVer').value='" & dbRow("AWEDSTVER").ToString() & "';document.getElementById('llistaTHor').value='" & dbRow("AWEDSTHOR").ToString() & "';document.getElementById('llistaTipusContinguts').value='" & dbRow("AWEDSTCO").ToString() & "';document.getElementById('llistaNodes2').value='" & dbRow("AWEDSLCW").ToString() & "';"
                        btnInsert.Value = "Modificar arbre web"

                        estructura = dbRow("AWEDSEST")
                        atributs = dbRow("AWEDSATR")

                        Dim llistaNodes2 As String()
                        Dim llistaCodiWeb As String
                        llistaNodes2 = Split(dbRow("AWEDSLCW"), ",")
                        llistaCodiWeb = ""
                        Dim item As String

                        For Each item In llistaNodes2
                            If llistaCodiWeb.Length > 0 Then
                                llistaCodiWeb += "|"
                            End If
                            If (item.Trim().Length > 0) Then
                                GAIA.bdr(objconn, "SELECT NODDSTXT FROM METLNOD WHERE NODINNOD=" & item.ToString(), DS)
                                If DS.Tables(0).Rows.Count > 0 Then
                                    dbRow = DS.Tables(0).Rows(0)
                                    llistaCodiWeb += dbRow("NODDSTXT").Replace("'", "´")
                                Else
                                    llistaCodiWeb += " "
                                End If
                            Else
                                llistaCodiWeb += " "
                            End If
                        Next item
                        lblCodi.Text += "document.getElementById('llistaCodis').value='" + llistaCodiWeb + "';</script>"
                    End If

                    DS.Dispose()
                Else    'és un arbre web nou.					
                    'Carrego el select d'idiomes
                    lblIdioma.Text = "<select name=""idioma"" >" & GAIA.llistaIdiomes(objconn, 0) & "</select>"
                    btnInsert.Value = "Crear arbre web"
                    lblCodi.Text = "<script>document.getElementById('estructura').value='0000'; document.getElementById('atributs').value='ih';document.getElementById('llistaTVer').value='100';document.getElementById('llistaTHor').value='100';document.getElementById('llistaCodis').value=' ';document.getElementById('llistaTipusContinguts').value='  ';document.getElementById('llistaNodes2').value=' ';</script>"
                    AWEDSHOR.Text = "950"
                    AWEDSVER.Text = "660"
                End If
            Else 'es un postback
                lblIdioma.Text = "<select name=""idioma"" >" & GAIA.llistaIdiomes(objconn, Request("idioma")) & "</select>"
                estructura = Request("estructura")
                atributs = Request("atributs")
                lblCodi.Text = "<script>document.getElementById('estructura').value='" + Request("estructura") + "'; document.getElementById('atributs').value='" + Request("atributs") + "';document.getElementById('llistaTVer').value='" + Request("llistaTVer") + "';document.getElementById('llistaTHor').value='" + Request("llistaTHor") + "';document.getElementById('llistaCodis').value='" + Request("llistaCodis") + "';document.getElementById('llistaTipusContinguts').value='" + Request("llistaTipusContinguts") + "';document.getElementById('llistaNodes2').value='" + Request("llistaNodes2") + "';</script>"
            End If

            lblTipusFulla.Text = ("<select name=""tipusContingut"" id=""tipusContingut"" size=""1"" disabled><option value=0></option>" + GAIA.llistaTipus(objconn, "node web") + GAIA.llistaTipus(objconn, "fulla web") + "</select>")

            arrayEstructura = Split(estructura, ",")
            Array.Sort(arrayEstructura)
            Dim arrayDescripcions As String()
            lblEstructura.Text = GAIA.pintaEstructura(objconn, arrayEstructura, Split(atributs, ","), Split(Request("llistaTVer"), ","), Split(Request("llistaTHor"), ","), 1, "", 1, "", arrayDescripcions, arrayBuit, 1)

        Else
            Server.Transfer("/gdocs/232511_1.aspx")
        End If

    End Sub 'Page_Load


    Protected Sub clickModificarArbreWeb(sender As Object, e As EventArgs)
        Dim rel As New clsRelacio
        If btnInsert.Value = "Modificar arbre web" Then
            GAIA.bdSR(objconn, "UPDATE METLAWE SET AWEINIDI=" + Request("idioma").ToString() + ", AWEDSTIT= '" + AWEDSTIT.Text.Replace("'", "''") + "', AWEDSSER=" + lstAWEDSSER.SelectedItem.Value.ToString() + ", AWEDSHOR=" + AWEDSHOR.Text + ", AWEDSVER=" + AWEDSVER.Text + ",AWEDSROT='" + AWEDSROT.Text.Replace("'", "''") + "' , AWEDSTHOR='" & Request("llistaTHor").ToString() & "', AWEDSTVER='" & Request("llistaTVer").ToString() & "', AWEDSTCO='" & Request("llistaTipusContinguts").ToString() & "',AWEDSEST='" & Request("estructura").ToString() & "',AWEDSATR='" & Request("atributs").ToString() & "',AWEDSLCW='" & Request("llistaNodes2").ToString() & "', AWEDSDOC = '" + AWEDSDOC.Text + "', AWEDSEBO = '" + AWEDSEBO.Text + "', AWEDSMET='" + AWEDSMET.Text.Replace("'", "''") + "', AWEDSPEU='" + AWEDSPEU.Text.Replace("'", "''") + "', AWEDSCSP='" + AWEDSCSP.Text.Replace("'", "''") + "', AWEDSCSI='" & AWEDSCSI.Text.Replace("'", "''") & "', AWEDSNOM='" & AWEDSNOM.Text.Replace("'", "''") & "' WHERE AWEINNOD=" & Request("id").ToString())




            GAIA.bdSR(objconn, "UPDATE METLNOD SET NODDSTXT='" & AWEDSNOM.Text.Replace("'", "''") & "' WHERE NODINNOD=" & Request("id").ToString())
            lblResultat.Text = ""
            lblResultat.Text = ""
            GAIA.escriuResultat(objconn, lblResultat, "Arbre web modificat amb èxit.", "")
            Dim relbuida As New clsRelacio


            GAIA.afegeixAccioManteniment(objconn, relbuida, Request("txtCodiNode"), 99, Now, Now, relbuida)
        Else
            Dim tipusNode, codiNode, nodeOrg, codiRelacio As Integer

            tipusNode = GAIA.tipusNodeByTxt(objconn, "arbre web")
            'Inserto el node arbre web
            codiNode = GAIA.insertarNode(objconn, tipusNode, AWEDSNOM.Text, Session("codiOrg"))
            'Creo una relació del node amb si mateix perque és el primer
            rel = GAIA.creaRelacio(objconn, tipusNode, codiNode, codiNode, 0, "", -1, 1, -1, 1, False, Session("codiOrg"))

            'Assigno el permís d'administrador per l'usuari creador
            nodeOrg = Session("codiOrg")
            clsPermisos.gravaPermis(objconn, 1, nodeOrg, 0, 0, rel)
            'Creo la fulla web
            GAIA.bdSR(objconn, "INSERT INTO METLAWE (AWEINNOD, AWEINIDI,AWEDSTIT,AWEDSHOR, AWEDSVER, AWEDSUSR,AWEDSTHOR, AWEDSTVER, AWEDSTCO, AWEDSEST, AWEDSATR, AWEDSLCW, AWEDSSER, AWEDSROT,AWEDSDOC,AWEDSEBO, AWEDSMET, AWEDSPEU,AWEDSCSP,AWEDSCSI,AWEDSNOM) VALUES (" + codiNode.ToString() + "," + Request("idioma").ToString() + ",'" + AWEDSTIT.Text.ToString().Replace("'", "''") + "'," + AWEDSHOR.Text + "," + AWEDSVER.Text + ",'" + Session("codiOrg") + "','" + Request("llistaTHor").ToString() + "','" + Request("llistaTVer").ToString() + "','" + Request("llistaTipusContinguts").ToString() + "','" + Request("estructura").ToString() + "','" + Request("atributs").ToString() + "','" + Request("llistaNodes2").ToString() + "'," + lstAWEDSSER.SelectedItem.Value.ToString() + ",'" + AWEDSROT.Text.ToString() + "','" + AWEDSDOC.Text + "','" + AWEDSEBO.Text + "','" + AWEDSMET.Text.Replace("'", "''") + "','" + AWEDSPEU.Text.Replace("'", "''") + "','" + AWEDSCSP.Text.Replace("'", "''") + "','" + AWEDSCSI.Text.Replace("'", "''") & "','" & AWEDSNOM.Text.Replace("'", "''") & "')")
            lblResultat.Text = ""
            GAIA.escriuResultat(objconn, lblResultat, "Arbre web afegit amb èxit.", "")
            btnInsert.Value = "Modificar arbre web"
        End If
    End Sub

    Protected Sub dividir(ByVal atribs As String)
        Dim estructura As String
        Dim atributs As String
        Dim arrayEstructura As String()
        Dim indexDiv, ultimindex As Integer
        Dim indexDiv2, indexDiv3 As String
        Dim valor As String

        If Request("ultimaDivisio").Length > 0 Then 'AND Request("ultimaDivisio")<>"t0" THEN
            indexDiv = Request("ultimaDivisio").Substring(1)
            ultimindex = Request("estructura").ToString().Substring(Request("estructura").ToString().Length - 3)
            indexDiv2 = formatea(ultimindex + 1)
            indexDiv3 = formatea(ultimindex + 2)
        Else
            indexDiv = 0
            valor = "0"
        End If

        'busco l'index on vull colocar 
        arrayEstructura = Split(Request("estructura"), ",")
        valor = arrayEstructura(indexDiv).Substring(0, arrayEstructura(indexDiv).Length - 3)
        estructura = Request("estructura").ToString() & "," & valor.ToString() & "1" & indexDiv2.ToString() & "," & valor.ToString() & "2" & indexDiv3.ToString()
        atributs = Request("atributs").ToString() & "," & atribs


        Dim llistaTVer, llistaTHor, llistaCodisWeb, llistaTipusContinguts, llistaNodes2 As String

        llistaTVer = Request("llistaTVer")
        llistaTHor = Request("llistaTHor")
        llistaCodisWeb = Request("llistaCodis")
        llistaTipusContinguts = Request("llistaTipusContinguts")
        llistaNodes2 = Request("llistaNodes2")

        If llistaTVer.Length > 0 Then
            llistaTVer += ","
            llistaTHor += ","
            llistaCodisWeb += "|"
            llistaTipusContinguts += ","
            llistaNodes2 += ","
        End If
        llistaTVer += "100,100"
        llistaTHor += "100,100"
        llistaCodisWeb += " | "
        llistaTipusContinguts += " , "
        llistaNodes2 += " , "

        lblCodi.Text = "<script>document.getElementById('estructura').value='" & estructura.ToString() & "';document.getElementById('atributs').value='" & atributs.ToString() & "';document.getElementById('llistaTVer').value='" & llistaTVer.ToString() & "';document.getElementById('llistaTHor').value='" & llistaTHor.ToString() & "';document.getElementById('llistaCodis').value='" & llistaCodisWeb.ToString() & "';document.getElementById('llistaTipusContinguts').value='" & llistaTipusContinguts.ToString() & "';document.getElementById('llistaNodes2').value='" & llistaNodes2.ToString() & "'</script>"

        arrayEstructura = Split(estructura, ",")
        Array.Sort(arrayEstructura)
        Dim arrayDescripcions As String()
        Dim arrayBuit As String()

        lblEstructura.Text = GAIA.pintaEstructura(objconn, arrayEstructura, Split(atributs, ","), Split(llistaTVer, ","), Split(llistaTHor, ","), 1, "", 1, "", arrayDescripcions, arrayBuit, 1)

    End Sub 'dividir

    Protected Function formatea(ByVal a As Integer)
        If a < 10 Then
            formatea = "00" & a.ToString()
        Else
            If a < 100 Then
                formatea = "0" & a.ToString()
            Else
                formatea = a.ToString()
            End If
        End If
    End Function

    Protected Sub clickDividirHoritzontalment(sender As Object, e As EventArgs)
        dividir("ih,h")
    End Sub 'clickDividirHoritzontalment

    Protected Sub clickDividirVerticalment(sender As Object, e As EventArgs)
        dividir("iv,v")
    End Sub 'clickDividirVerticalment

    Protected Sub clickEsborrarDivisio(sender As Object, e As EventArgs)
        Dim arrayEstructura, arrayEstructuraTMP As String()
        Dim arrayAtributs, arrayAtributsTMP As String()
        Dim arrayTVer, arrayTVerTMP, arrayTHor, arrayTHorTMP As String()
        Dim arrayLlistaTipusContinguts, arrayLlistaCodiWeb, arrayLlistaNode2, arrayLlistaTipusContingutsTMP, arrayLlistaCodiWebTMP, arrayLlistaNode2TMP As String()
        Dim arrayBuit As String()
        Dim estructura, atributs, tver, thor, llistaTipusContinguts, llistaCodiWeb, llistaNodes2 As String
        Dim index, index2 As Integer
        Dim valor1, valor2 As String
        Dim i, cont As Integer
        Dim esborrarDiv As Integer
        estructura = ""
        atributs = ""

        tver = ""
        thor = ""
        llistaTipusContinguts = ""
        llistaCodiWeb = ""
        llistaNodes2 = ""

        If Request("ultimaDivisio").Length > 0 And Request("ultimaDivisio") <> "t0" Then
            arrayEstructuraTMP = Split(Request("estructura"), ",")
            index = Request("ultimaDivisio").Substring(1)
            If index Mod 2 = 1 Then
                index2 = index + 1
            Else
                index2 = index - 1
            End If

            valor1 = arrayEstructuraTMP(index).Substring(0, arrayEstructuraTMP(index).Length - 3)
            valor2 = arrayEstructuraTMP(index2).Substring(0, arrayEstructuraTMP(index2).Length - 3)
            arrayAtributsTMP = Split(Request("atributs"), ",")
            arrayTVerTMP = Split(Request("llistaTVer"), ",")
            arrayTHorTMP = Split(Request("llistaTHor"), ",")
            arrayLlistaTipusContingutsTMP = Split(Request("llistaTipusContinguts"), ",")
            arrayLlistaCodiWebTMP = Split(Request("llistaCodis"), "|")
            arrayLlistaNode2TMP = Split(Request("llistaNodes2"), ",")

            i = 0
            For cont = 0 To arrayEstructuraTMP.Length - 1
                ' Trobo si he d'esborrar la divisió, perque està anidada dins de la que volem esborrar
                esborrarDiv = False
                If arrayEstructuraTMP(cont).Length - 3 >= valor1.ToString().Length Then
                    If arrayEstructuraTMP(cont).Substring(0, arrayEstructuraTMP(cont).ToString().Length - 3).Substring(0, valor1.ToString().Length) = valor1 Then
                        esborrarDiv = True
                    End If
                End If
                If arrayEstructuraTMP(cont).Length - 3 >= valor2.ToString().Length Then
                    If arrayEstructuraTMP(cont).Substring(0, arrayEstructuraTMP(cont).ToString().Length - 3).Substring(0, valor2.ToString().Length) = valor2 Then
                        esborrarDiv = True
                    End If
                End If

                ' NO l'esborro i per tant l'afegeixo a la nova estructura
                If esborrarDiv = False Then
                    arrayEstructuraTMP(i) = arrayEstructuraTMP(cont).Substring(0, arrayEstructuraTMP(cont).Length - 3).ToString() & formatea(i).ToString()
                    arrayAtributsTMP(i) = arrayAtributsTMP(cont)
                    arrayTVerTMP(i) = arrayTVerTMP(cont)
                    arrayTHorTMP(i) = arrayTHorTMP(cont)
                    arrayLlistaCodiWebTMP(i) = arrayLlistaCodiWebTMP(cont)
                    arrayLlistaNode2TMP(i) = arrayLlistaNode2TMP(cont)
                    arrayLlistaTipusContingutsTMP(i) = arrayLlistaTipusContingutsTMP(cont)
                    If estructura.Length > 0 Then
                        estructura += ","
                        atributs += ","
                        tver += ","
                        thor += ","
                        llistaTipusContinguts += ","
                        llistaCodiWeb += "|"
                        llistaNodes2 += ","
                    End If
                    estructura += arrayEstructuraTMP(cont).Substring(0, arrayEstructuraTMP(cont).Length - 3).ToString() + formatea(i).ToString()
                    atributs += arrayAtributsTMP(cont)
                    tver += arrayTVerTMP(cont)
                    thor += arrayTHorTMP(cont)
                    llistaTipusContinguts += arrayLlistaTipusContingutsTMP(cont)
                    llistaCodiWeb += arrayLlistaCodiWebTMP(cont)
                    llistaNodes2 += arrayLlistaNode2TMP(cont)
                    i = i + 1
                End If
            Next cont
            ReDim Preserve arrayEstructuraTMP(i - 1)
            Array.Sort(arrayEstructuraTMP)
            Dim arrayDescripcions As String()
            lblEstructura.Text = GAIA.pintaEstructura(objconn, arrayEstructuraTMP, arrayAtributsTMP, Split(tver, ","), Split(thor, ","), 1, "", 1, "", arrayDescripcions, arrayBuit, 1)
        End If
        'En el cas de que esborrar divisió esborri completament tota la informació sobre les cel·les, 
        'crearé la cel·la inicial
        If estructura = "" Then
            estructura = "0000"
            atributs = "ih"
            tver = "100"
            thor = "100"
        End If

        lblCodi.Text = "<script>document.getElementById('ultimaDivisio').value='';document.getElementById('estructura').value='" & estructura.ToString() & "';document.getElementById('atributs').value='" & atributs.ToString() & "';document.getElementById('llistaTVer').value='" & tver.ToString() & "';document.getElementById('llistaTHor').value='" & thor.ToString() & "';document.getElementById('llistaTipusContinguts').value='" & llistaTipusContinguts.ToString() & "';document.getElementById('llistaCodis').value='" & llistaCodiWeb.ToString() & "';document.getElementById('llistaNodes2').value='" & llistaNodes2.ToString() & "';</script>"


    End Sub 'clickEsborrarDivisio


    Protected Sub inicialitzaLListaServidors()
        Dim DS As DataSet
        DS = New DataSet()
        GAIA.bdr(objconn, "SELECT SERINCOD, SERDSURL FROM METLSER", DS)
        lstAWEDSSER.DataSource = DS.Tables(0).DefaultView
        lstAWEDSSER.DataTextField = "SERDSURL"
        lstAWEDSSER.DataValueField = "SERINCOD"
        lstAWEDSSER.DataBind()
        lstAWEDSSER.Items.Insert(0, "Selecciona un servidor")
        'lstAWEDSSER.Items(0).Selected = True
        DS.Dispose()
    End Sub 'carregaCodis





End Class