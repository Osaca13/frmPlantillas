Imports System.Data
Imports System.Data.OleDb
Imports FredCK.FCKeditorV2.FCKeditor

Public Class frmFullaWeb
    Inherits System.Web.UI.Page


    Public nif As String
    Public objconn As OleDbConnection


    Private Sub Page_UnLoad(sender As Object, e As System.EventArgs) Handles MyBase.Unload
        GAIA.bdFi(objconn)
    End Sub 'Page_UnLoad	


    Dim llistaDesc As String = ""
    Dim estructura As String
    Dim atributs As String
    Dim arrayEstructura As String()

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load

        WEBDSDEC.Attributes.Add("onchange", "seleccionaCelda('','');")

        Dim DS As DataSet
        Dim dbRow As DataRow

        Dim strTmp As String

        estructura = ""
        atributs = ""

        objconn = GAIA.bdIni()
        'Seleccio de l'idioma actiu
        Dim codiIdioma As Integer = 1

        If Not (Request("lstIdioma") Is Nothing) Then
            codiIdioma = Request("lstIdioma")
        Else
            If Not (Request("idiarbre") Is Nothing) Then
                codiIdioma = Request("idiarbre")
            End If
        End If

        'nomÃ©s els de unitat web podem editar-ho. 
        '31/3/14 Max. Afegeixo tambÃ© a Tere Corcos(48729) pq tÃ© la funcionalitat de crear pàgines web dins de la seva intranet
        If Session("codiOrg") = 49730 Or Session("codiOrg") = 49727 Or Session("codiOrg") = 56935 Or Session("codiOrg") = 80879 Or Session("codiOrg") = 48729 Or Session("codiOrg") = 144886 Or Session("codiOrg") = 297650 Or Session("codiOrg") = 313486 Or Session("codiOrg") = 346231 Then


            'If HttpContext.Current.User.Identity.Name.Length > 0 Then
            '    If (Session("nif") Is Nothing) Then
            '        Session("nif") = GAIA.nifUsuari(objconn, HttpContext.Current.User.Identity.Name).Trim()
            '    End If
            '    If Session("codiOrg") Is Nothing Then

            '        Session("CodiOrg") = GAIA.trobaNodeUsuari(objconn, Session("nif")).ToString().Trim()

            '    End If

            'End If
            Session("codiOrg") = 346231
            ' nif = Session("nif").Trim()

            If Not Page.IsPostBack Then
                carregallistaEstilsCSS()
                If Not Request("id") Is Nothing Then
                    If Not (Request("idiArbre") Is Nothing) Then
                        codiIdioma = Request("idiArbre")
                    End If

                    txtCodiNode.Text = Request("id")
                    carregaDades(txtCodiNode.Text, codiIdioma)
                    btnInsert.Value = "Modificar pàgina web"
                Else

                    'Carrego el select d'idiomes
                    'lblIdioma.Text = "<select name=""idioma"" >" & GAIA.llistaIdiomes(objConn,0)& "</select>"			
                    WEBDTPUB.Text = New System.DateTime().Now.ToString().Substring(0, 10)
                    'WEBDTCAD.text = DateAdd("m",1,new System.DateTime().Now).toString().Substring(0,10)		
                    btnInsert.Value = "Crear pàgina web"
                    lblCodi.Text = "<script>document.getElementById('estructura').value='0000'; document.getElementById('atributs').value='ih';document.getElementById('llistaTVer').value='100';document.getElementById('llistachkimp').value='1';document.getElementById('llistachkCND').value='1';document.getElementById('llistaTHor').value='100';document.getElementById('llistaPlantilles').value=' '; document.getElementById('llistaCodis').value=' '; document.getElementById('llistaCSS').value='  ';document.getElementById('llistaTipusContinguts').value='  ';document.getElementById('llistaNodes').value=' ';document.getElementById('llistaNodes2').value=' ';document.getElementById('llistaDescripcions').value='';</script>"

                End If
            Else 'es un postback
                '	lblIdioma.Text = "<select name=""idioma"" >" &  GAIA.llistaIdiomes(objConn,Request("lstIdioma"))& "</select>"			
                estructura = Request("estructura")
                atributs = Request("atributs")

                lblCodi.Text = "<script>document.getElementById('estructura').value='" + Request("estructura") + "'; document.getElementById('atributs').value='" + Request("atributs") + "';document.getElementById('llistaTVer').value='" + Request("llistaTVer") + "';document.getElementById('llistachkimp').value='" + Request("llistachkimp") + "';document.getElementById('llistachkCND').value='" + Request("llistachkCND") + "';document.getElementById('llistaTHor').value='" + Request("llistaTHor") + "';document.getElementById('llistaPlantilles').value='" + Request("llistaPlantilles").ToString().Replace("'", "\'") + "';document.getElementById('llistaCodis').value='" + Request("llistaCodis") + "';document.getElementById('llistaTipusContinguts').value='" + Request("llistaTipusContinguts") + "';document.getElementById('llistaNodes').value='" + Request("llistaNodes") + "';document.getElementById('llistaNodes2').value='" + Request("llistaNodes2") + "';document.getElementById('llistaCSS').value='" + Request("llistaCSS") + "';document.getElementById('llistaDescripcions').value='" + Request("llistaDescripcions").ToString().Replace("'", "\'") + "';</script>"
            End If
            '	lblIdioma.Text = "<select name=""idioma"" >" + GAIA.llistaIdiomes(objConn,codiIdioma).toSTring()+ "</select>"


            lblTipusFulla.Text = ("<select id=""tipusContingut"" name=""tipusContingut"" size=""1"" ><option value=0></option>" + GAIA.llistaTipus(objconn, "node") + GAIA.llistaTipus(objconn, "fulla") + "</select>")

            arrayEstructura = Split(estructura, ",")
            Array.Sort(arrayEstructura)
            Dim arrayBuit As String()
            If Not Page.IsPostBack Then
                lblEstructura.Text = GAIA.pintaEstructura(objconn, arrayEstructura, Split(atributs, ","), Split(Request("llistaTVer"), ","), Split(Request("llistaTHor"), ","), 1, "", 1, "", Split(llistaDesc, "|"), arrayBuit, 1)
            Else
                lblEstructura.Text = GAIA.pintaEstructura(objconn, arrayEstructura, Split(atributs, ","), Split(Request("llistaTVer"), ","), Split(Request("llistaTHor"), ","), 1, "", 1, "", Split(Request("llistaDescripcions"), "|"), arrayBuit, 1)
            End If
        Else
            Server.Transfer("/gdocs/232511_1.aspx")
        End If
    End Sub 'Page_Load


    Protected Sub clickEsborrarIdioma(sender As Object, e As EventArgs)

        GAIA.bdSR(objconn, "DELETE FROM METLWEB WHERE WEBINNOD=" + txtCodiNode.Text + " AND WEBINIDI=" + Request("lstIdioma"))

        '	carregaDades(txtCodiNode.text,1)

        '			lstIdioma.SelectedItem.selected=FALSE
        '		lstIdioma.Items.FindByValue(1).Selected = True		
        '	lblResultat.text=""


    End Sub

    Protected Sub clickAfegirPaginaWeb(sender As Object, e As EventArgs)
        Dim tipusNode, codiNode As Integer
        Dim noddttim As Date
        Dim buscar As String
        Dim heretar As String
        Dim esForm, esEML, esSSL As String
        Dim strWEBDSCIP, strWEBDSCIC As String
        Dim relBuida As New clsRelacio
        Dim dataCaducitat As String

        If WEBDTCAD.Text = "" Then
            dataCaducitat = "01/01/1900"
        Else
            dataCaducitat = WEBDTCAD.Text
        End If

        If Request("gaiaCircuitPublicacioNodes").ToString().Length = 0 Then
            strWEBDSCIP = "0"
        Else
            strWEBDSCIP = Request("gaiaCircuitPublicacioNodes").ToString()
        End If

        If Request("gaiaCircuitCaducitatNodes").ToString().Length = 0 Then
            strWEBDSCIC = "0"
        Else
            strWEBDSCIC = Request("gaiaCircuitCaducitatNodes").ToString()
        End If
        If WEBTPBUS.Checked Then
            buscar = "S"
        Else
            buscar = "N"
        End If
        If WEBTPHER.Checked Then
            heretar = "S"
        Else
            heretar = "N"
        End If
        If WEBSWFRM.Checked Then
            esForm = "S"
        Else
            esForm = "N"
        End If
        If WEBSWEML.Checked Then
            esEML = "S"
        Else
            esEML = "N"
        End If
        If WEBSWSSL.Checked Then
            esSSL = "S"
        Else
            esSSL = "N"
        End If

        noddttim = New System.DateTime().Now

        If btnInsert.Value = "Modificar pàgina web" Then
            If Not Request("id") Is Nothing Then
                txtCodiNode.Text = Request("id").ToString()
            End If
            'GAIA.bdSR(objConn,"DELETE FROM METLWEB WHERE WEBINNOD="+ txtcodiNode.text + " AND WEBINIDI="+Request("lstIdioma"))

            Dim ds As DataSet
            ds = New DataSet()
            GAIA.bdr(objconn, "select WEBINNOD FROM METLWEB WHERE WEBINIDI=" & Request("lstIdioma") & " AND WEBINNOD=" & txtCodiNode.Text, ds)
            If ds.Tables(0).Rows.Count = 0 Then
                GAIA.bdSR(objconn, "INSERT INTO METLWEB (WEBINNOD, WEBINIDI,WEBDSTIT,WEBDSFIT, WEBDSURL, WEBDTPUB, WEBDTCAD, WEBDTANY, WEBDSUSR,WEBTPBUS,WEBDSTHOR, WEBDSTVER, WEBDSTCO, WEBDSPLA, WEBDSEST, WEBDSATR, WEBDSLCW, WEBDSCSS, WEBDSCIP, WEBDSCIC, WEBTPHER, WEBDSDEC, WEBCDRAL, WEBDSIMP,WEBDSCND, WEBWNMTH,WEBSWFRM, WEBSWEML,WEBDSEBO, WEBDSDES, WEBDSPCL, WEBSWSSL) VALUES (" & txtCodiNode.Text & "," & Request("lstIdioma").ToString() & ",'" & WEBDSTIT.value.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "','" & WEBDSFIT.Text.ToString().Replace("'", "''") & "','" & WEBDSURL.Text.ToString().Replace("'", "''") & "','" & WEBDTPUB.Text.ToString() & "','" & dataCaducitat.ToString() & "','" & noddttim.ToString() & "','" & Session("codiOrg") & "','" & buscar.ToString() & "','" & Request("llistaTHor").ToString() & "','" & Request("llistaTVer").ToString() & "','" & Request("llistaTipusContinguts").ToString() & "','" & Request("llistaNodes").ToString() & "','" & Request("estructura").ToString() & "','" & Request("atributs").ToString() & "','" & Request("llistaNodes2").ToString() & "','" & Request("llistaCSS").ToString().Replace("'", "''") & "'," + strWEBDSCIP + "," + strWEBDSCIC + ", '" + heretar + "','" & Request("llistaDescripcions").ToString().Replace("'", "''") & "'," & Request("gaiaAutoenllacNodes") & ",'" & Request("llistachkimp") & "','" & Request("llistachkCND") & "'," + WEBWNMTH.Text.ToString() + ",'" + esForm + "','" + esEML & "','" & WEBDSEBO.Text.Replace("'", "''") & "','" & WEBDSDES.value.Replace("'", "''") & "','" & WEBDSPCL.Text.Replace("'", "''") & "','" & esSSL & "')   ")
            Else
                GAIA.bdSR(objconn, "UPDATE METLWEB SET WEBDSTIT = '" & WEBDSTIT.value.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "', WEBDSFIT ='" & WEBDSFIT.Text.ToString().Replace("'", "''") & "', WEBDSURL='" & WEBDSURL.Text.ToString().Replace("'", "''") & "', WEBDTPUB='" & WEBDTPUB.Text.ToString() & "', WEBDTCAD='" & dataCaducitat.ToString() & "', WEBDTANY='" & noddttim.ToString() & "', WEBDSUSR='" & Session("codiOrg") & "',WEBTPBUS='" & buscar & "',WEBDSTHOR='" & Request("llistaTHor").ToString() & "', WEBDSTVER='" & Request("llistaTVer") & "', WEBDSTCO='" & Request("llistaTipusContinguts") & "', WEBDSPLA='" & Request("llistaNodes").ToString() & "', WEBDSEST='" & Request("estructura") & "', WEBDSATR='" & Request("atributs") & "', WEBDSLCW='" & Request("llistaNodes2") & "', WEBDSCSS='" & Request("llistaCSS").ToString().Replace("'", "''") & "', WEBDSCIP=" & strWEBDSCIP & ", WEBDSCIC=" & strWEBDSCIC & ", WEBTPHER='" & heretar & "', WEBDSDEC='" & Request("llistaDescripcions").ToString().Replace("'", "''") & "', WEBCDRAL=" & Request("gaiaAutoenllacNodes") & ", WEBDSIMP='" & Request("llistachkimp") & "',WEBDSCND='" & Request("llistachkCND") & "', WEBWNMTH=" & WEBWNMTH.Text & ",WEBSWFRM='" & esForm & "' , WEBSWEML='" & esEML & "',WEBDSEBO='" & WEBDSEBO.Text.Replace("'", "''") & "', WEBDSDES='" & WEBDSDES.value.Replace("'", "''") & "', WEBDSPCL='" & WEBDSPCL.Text.Replace("'", "''") & "', WEBSWSSL='" & esSSL & "'  WHERE    WEBINIDI=" & Request("lstIdioma") & " AND WEBINNOD=" & txtCodiNode.Text)
            End If
            ds.Dispose()
            If Request("lstIdioma") = 1 Then
                GAIA.bdSR(objconn, "UPDATE METLNOD SET NODDSTXT='" & WEBDSTIT.value.Replace("'", "''").replace("<p>", "").replace("</p>", "") & "' WHERE NODINNOD=" & txtCodiNode.Text)

                'Si hi ha pàgines en altres idiomes, actualitzo tots els valors comuns
                GAIA.bdSR(objconn, "UPDATE METLWEB SET  WEBDTPUB='" & WEBDTPUB.Text.ToString() & "', WEBDTCAD='" & dataCaducitat.ToString() & "', WEBDTANY='" & noddttim.ToString() & "', WEBDSUSR='" & Session("codiOrg") & "',WEBTPBUS='" & buscar & "',WEBDSTHOR='" & Request("llistaTHor").ToString() & "', WEBDSTVER='" & Request("llistaTVer") & "', WEBDSTCO='" & Request("llistaTipusContinguts") & "', WEBDSPLA='" & Request("llistaNodes").ToString() & "', WEBDSEST='" & Request("estructura") & "', WEBDSATR='" & Request("atributs") & "', WEBDSLCW='" & Request("llistaNodes2") & "', WEBDSCSS='" & Request("llistaCSS").ToString().Replace("'", "''") & "', WEBDSCIP=" & strWEBDSCIP & ", WEBDSCIC=" & strWEBDSCIC & ", WEBTPHER='" & heretar & "', WEBDSDEC='" & Request("llistaDescripcions").ToString().Replace("'", "''") & "', WEBCDRAL=" & Request("gaiaAutoenllacNodes") & ", WEBDSIMP='" & Request("llistachkimp") & "',WEBDSCND='" & Request("llistachkCND") & "', WEBWNMTH=" & WEBWNMTH.Text & ",WEBSWFRM='" & esForm & "' , WEBSWEML='" & esEML & "',WEBDSEBO='" & WEBDSEBO.Text.Replace("'", "''") & "', WEBSWSSL='" & esSSL & "' WHERE    WEBINIDI<>1 AND WEBINNOD=" & txtCodiNode.Text)




                'GAIA.bdSR(objconn, "UPDATE METLWEBSET WEBTPHER='" & heretar & "', WEBSWFRM='" & esForm  & "',WEBSWEML='" & esEML & "',WEBSWSSL='" & esSSL & "',WEBWNMTH=" & WEBWNMTH.text & ",WEBDSEBO='" &  WEBDSEBO.text.Replace("'","''") & "', WHERE    WEBINIDI<>1 AND WEBINNOD=" & txtCodiNode.tet)
            End If
            lblResultat.Text = ""
            GAIA.escriuResultat(objconn, lblResultat, "Pàgina Web modificada amb Èxit.", "<a href="".l-h.es/GAIA/aspx/web/frmFullaWeb.aspx"" class=""txtRojo12Px"">&nbsp;Nova pàgina web</a>")
            GAIA.afegeixAccioManteniment(objconn, relBuida, txtCodiNode.Text, 99, CDate(WEBDTPUB.Text.ToString()), CDate(dataCaducitat.ToString()), relBuida)
            GAIA.log(objconn, relBuida, Session("codiOrg"), WEBDSTIT.value.replace("'", "''"), GAIA.TAMODIFICAR, txtCodiNode.Text)
        Else
            tipusNode = GAIA.tipusNodeByTxt(objconn, "fulla web")
            'Inserto el node fulla web
            codiNode = GAIA.insertarNode(objconn, tipusNode, WEBDSTIT.value.replace("<p>", "").replace("</p>", ""), Session("codiOrg"))
            'Inserto el node fulla web en l'arbre personal de l'usuari		
            GAIA.insertaNodeArbrePersonal(objconn, tipusNode, codiNode, Session("codiOrg"), "")
            'Creo la fulla web
            GAIA.bdSR(objconn, "INSERT INTO METLWEB (WEBINNOD, WEBINIDI,WEBDSTIT,WEBDSFIT, WEBDSURL, WEBDTPUB, WEBDTCAD, WEBDTANY, WEBDSUSR,WEBTPBUS,WEBDSTHOR, WEBDSTVER, WEBDSTCO, WEBDSPLA, WEBDSEST, WEBDSATR, WEBDSLCW, WEBDSCSS, WEBDSCIP, WEBDSCIC, WEBTPHER, WEBDSDEC, WEBCDRAL, WEBDSIMP,WEBDSCND,WEBWNMTH,WEBSWFRM, WEBSWEML, WEBDSEBO, WEBDSDES, WEBDSPCL, WEBSWSSL) VALUES (" & codiNode.ToString() & "," & Request("lstIdioma").ToString() & ",'" & WEBDSTIT.value.tostring().Replace("'", "''").replace("<p>", "").replace("</p>", "") & "','" & WEBDSFIT.Text.ToString().Replace("'", "''") & "','" & WEBDSURL.Text.ToString().Replace("'", "''") & "','" & WEBDTPUB.Text.ToString() & "','" & dataCaducitat.ToString() & "','" & noddttim.ToString() & "','" & Session("codiOrg") & "','" & buscar.ToString() & "','" & Request("llistaTHor").ToString() & "','" & Request("llistaTVer").ToString() & "','" & Request("llistaTipusContinguts").ToString() & "','" & Request("llistaNodes").ToString() & "','" & Request("estructura").ToString() & "','" & Request("atributs").ToString() & "','" & Request("llistaNodes2").ToString() & "','" & Request("llistaCSS").ToString().Replace("'", "''") & "'," + strWEBDSCIP + "," + strWEBDSCIC + ", '" + heretar + "','" & Request("llistaDescripcions").ToString().Replace("'", "''") & "'," & Request("gaiaAutoenllacNodes") & ",'" & Request("llistachkimp") & "','" & Request("llistachkCND") & "'," + WEBWNMTH.Text.ToString() + ",'" + esForm + "','" + esEML & "','" & WEBDSEBO.Text.Replace("'", "''") & "','" & WEBDSDES.value.replace("'", "''") & "','" & WEBDSPCL.Text.Replace("'", "''") & "','" & esSSL & "')")
            lblResultat.Text = ""
            GAIA.escriuResultat(objconn, lblResultat, "Pàgina Web afegida amb Èxit.", "<a href="".l-h.es/GAIA/aspx/web/frmFullaWeb.aspx"" class=""txtRojo12Px"">&nbsp;Nova pàgina web</a>")
            txtCodiNode.Text = codiNode.ToString()
            btnInsert.Value = "Modificar pàgina web"
            GAIA.afegeixAccioManteniment(objconn, relBuida, codiNode, 99, CDate(WEBDTPUB.Text.ToString()), CDate(dataCaducitat.ToString()), relBuida)
            GAIA.log(objconn, relBuida, Session("codiOrg"), WEBDSTIT.value.replace("'", "''"), GAIA.TAINSERTAR, codiNode)
        End If



    End Sub 'ClickAfegirPaginaWeb


    Protected Sub pintaDescripcio(sender As Object, e As EventArgs)
        Dim estructura As String
        Dim atributs As String
        Dim arrayEstructura As String()
        Dim llistaDescripcions As String
        Dim arrayBuit As String()
        arrayEstructura = Split(Request("estructura"), ",")
        estructura = Request("estructura")
        atributs = Request("atributs")

        Dim llistachkimp, llistachkCND, llistaTVer, llistaTHor, llistaPlantilles, llistaCodisWeb, llistaTipusContinguts, llistaEstilsCSS, llistaNodes, llistaNodes2 As String

        llistachkimp = Request("llistachkimp")
        llistachkCND = Request("llistachkCND")
        llistaTVer = Request("llistaTVer")
        llistaTHor = Request("llistaTHor")
        llistaPlantilles = Request("llistaPlantilles")
        llistaCodisWeb = Request("llistaCodis")
        llistaTipusContinguts = Request("llistaTipusContinguts")
        llistaNodes = Request("llistaNodes")
        llistaNodes2 = Request("llistaNodes2")
        llistaEstilsCSS = Request("llistaCSS")
        llistaDescripcions = Request("llistaDescripcions")

        lblCodi.Text = "<script>document.getElementById('estructura').value='" & estructura.ToString() & "';document.getElementById('atributs').value='" & atributs.ToString() & "';document.getElementById('llistaTVer').value='" & llistaTVer.ToString() & "';document.getElementById('llistachkimp').value='" & llistachkimp.ToString() & "';document.getElementById('llistachkCND').value='" & llistachkCND.ToString() & "';document.getElementById('llistaTHor').value='" & llistaTHor.ToString() & "';document.getElementById('llistaCSS').value='" & llistaEstilsCSS.ToString() & "';document.getElementById('llistaPlantilles').value='" & llistaPlantilles.ToString().Replace("'", "\'") & "';document.getElementById('llistaCodis').value='" & llistaCodisWeb.ToString().Replace("'", "\'") & "';document.getElementById('llistaTipusContinguts').value='" & llistaTipusContinguts.ToString() & "';document.getElementById('llistaNodes').value='" & llistaNodes.ToString() & "';document.getElementById('llistaNodes2').value='" & llistaNodes2.ToString() & "';document.getElementById('llistaDescripcions').value='" & llistaDescripcions.ToString().Replace("'", "\'") & "'</script>"

        Array.Sort(arrayEstructura)
        'linia modificada por sara
        WEBDSDEC.Text = ""
        lblEstructura.Text = GAIA.pintaEstructura(objconn, arrayEstructura, Split(atributs, ","), Split(llistaTVer, ","), Split(llistaTHor, ","), 1, "", 1, "", Split(llistaDescripcions, "|"), arrayBuit, 1)

    End Sub 'pintaDescripcio



    Protected Sub dividir(ByVal atribs As String)
        Dim estructura As String
        Dim atributs As String
        Dim arrayEstructura As String()
        Dim indexDiv, ultimindex As Integer
        Dim indexDiv2, indexDiv3 As String
        Dim valor As String
        Dim llistaDescripcions As String
        Dim arrayBuit As String()
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


        Dim llistachkimp, llistachkCND, llistaTVer, llistaTHor, llistaPlantilles, llistaCodisWeb, llistaTipusContinguts, llistaEstilsCSS, llistaNodes, llistaNodes2 As String

        llistachkimp = Request("llistachkimp")
        llistachkCND = Request("llistachkCND")
        llistaTVer = Request("llistaTVer")
        llistaTHor = Request("llistaTHor")
        llistaPlantilles = Request("llistaPlantilles")
        llistaCodisWeb = Request("llistaCodis")
        llistaTipusContinguts = Request("llistaTipusContinguts")
        llistaNodes = Request("llistaNodes")
        llistaNodes2 = Request("llistaNodes2")
        llistaEstilsCSS = Request("llistaCSS")
        llistaDescripcions = Request("llistaDescripcions")

        If llistaTVer.Length > 0 Then
            llistachkimp += ","
            llistachkCND += ","
            llistaTVer += ","
            llistaTHor += ","
            llistaPlantilles += "|"
            llistaCodisWeb += "|"
            llistaTipusContinguts += ","
            llistaNodes += ","
            llistaNodes2 += ","
            llistaEstilsCSS += "|"
            llistaDescripcions += "|"
        End If
        llistachkimp += "1,1"
        llistachkCND += "1,1"
        llistaTVer += "100,100"
        llistaTHor += "100,100"
        llistaPlantilles += " | "
        llistaCodisWeb += " | "
        llistaTipusContinguts += " , "
        llistaNodes += " , "
        llistaNodes2 += " , "
        llistaEstilsCSS += "0,0,0,0,0,0,0|0,0,0,0,0,0,0"
        llistaDescripcions += "|"

        lblCodi.Text = "<script>document.getElementById('estructura').value='" & estructura.ToString() & "';document.getElementById('atributs').value='" & atributs.ToString() & "';document.getElementById('llistaTVer').value='" & llistaTVer.ToString() & "';document.getElementById('llistachkimp').value='" & llistachkimp.ToString() & "';document.getElementById('llistachkCND').value='" & llistachkCND.ToString() & "';document.getElementById('llistaTHor').value='" & llistaTHor.ToString() & "';document.getElementById('llistaCSS').value='" & llistaEstilsCSS.ToString() & "';document.getElementById('llistaPlantilles').value='" & llistaPlantilles.ToString().Replace("'", "\'") & "';document.getElementById('llistaCodis').value='" & llistaCodisWeb.ToString().Replace("'", "\'") & "';document.getElementById('llistaTipusContinguts').value='" & llistaTipusContinguts.ToString() & "';document.getElementById('llistaNodes').value='" & llistaNodes.ToString() & "';document.getElementById('llistaNodes2').value='" & llistaNodes2.ToString() & "';document.getElementById('llistaDescripcions').value='" & llistaDescripcions.ToString().Replace("'", "\'") & "'</script>"

        arrayEstructura = Split(estructura, ",")
        Array.Sort(arrayEstructura)
        lblEstructura.Text = GAIA.pintaEstructura(objconn, arrayEstructura, Split(atributs, ","), Split(llistaTVer, ","), Split(llistaTHor, ","), 1, "", 1, "", Split(llistaDescripcions, "|"), arrayBuit, 1)

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
        Dim arraychkimp, arraychkimpTMP, arraychkCND, arraychkCNDTMP, arrayTVer, arrayTVerTMP, arrayTHor, arrayTHorTMP As String()
        Dim arrayllistaEstilsCSS, arrayllistaEstilsCSSTMP, arrayLlistaTipusContinguts, arrayLlistaPlantilles, arrayLlistaCodiWeb, arrayLlistaNode, arrayLlistaNode2, arrayLlistaTipusContingutsTMP, arrayLlistaPlantillesTMP, arrayLlistaCodiWebTMP, arrayLlistaNodeTMP, arrayLlistaNode2TMP As String()
        Dim arrayllistaDescripcions, arrayllistaDescripcionsTMP As String()

        Dim estructura, atributs, chkimp, chkCND, tver, thor, llistaTipusContinguts, llistaPlantilles, llistaCodiWeb, llistaNodes, llistaNodes2, llistaEstilsCSS As String
        Dim llistaDescripcions As String
        Dim index, index2 As Integer
        Dim valor1, valor2 As String
        Dim i, cont As Integer
        Dim esborrarDiv As Integer
        Dim arrayBuit As String()
        estructura = ""
        atributs = ""

        chkimp = ""
        chkCND = ""
        tver = ""
        thor = ""
        llistaTipusContinguts = ""
        llistaPlantilles = ""
        llistaNodes = ""
        llistaCodiWeb = ""
        llistaNodes2 = ""
        llistaEstilsCSS = ""
        llistaDescripcions = ""

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
            arraychkimpTMP = Split(Request("llistachkimp"), ",")
            arraychkCNDTMP = Split(Request("llistachkCND"), ",")
            arrayTVerTMP = Split(Request("llistaTVer"), ",")
            arrayTHorTMP = Split(Request("llistaTHor"), ",")
            arrayLlistaTipusContingutsTMP = Split(Request("llistaTipusContinguts"), ",")
            arrayLlistaPlantillesTMP = Split(Request("llistaPlantilles"), "|")
            arrayLlistaNodeTMP = Split(Request("llistaNodes"), ",")
            arrayLlistaCodiWebTMP = Split(Request("llistaCodis"), "|")
            arrayLlistaNode2TMP = Split(Request("llistaNodes2"), ",")
            arrayllistaEstilsCSSTMP = Split(Request("llistaCSS"), "|")
            arrayllistaDescripcionsTMP = Split(Request("llistaDescripcions"), "|")
            i = 0
            For cont = 0 To arrayEstructuraTMP.Length - 1
                ' Trobo si he d'esborrar la divisiÃ³, perque està anidada dins de la que volem esborrar
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
                    arraychkimpTMP(i) = arraychkimpTMP(cont)
                    arraychkCNDTMP(i) = arraychkCNDTMP(cont)
                    arrayTVerTMP(i) = arrayTVerTMP(cont)
                    arrayTHorTMP(i) = arrayTHorTMP(cont)
                    arrayLlistaPlantillesTMP(i) = arrayLlistaPlantillesTMP(cont)
                    arrayLlistaNodeTMP(i) = arrayLlistaNodeTMP(cont)
                    arrayLlistaCodiWebTMP(i) = arrayLlistaCodiWebTMP(cont)
                    arrayLlistaNode2TMP(i) = arrayLlistaNode2TMP(cont)
                    arrayLlistaTipusContingutsTMP(i) = arrayLlistaTipusContingutsTMP(cont)
                    arrayllistaEstilsCSSTMP(i) = arrayllistaEstilsCSSTMP(cont)
                    arrayllistaDescripcionsTMP(i) = arrayllistaDescripcionsTMP(cont)
                    If estructura.Length > 0 Then
                        estructura += ","
                        atributs += ","
                        chkimp += ","
                        chkCND += ","
                        tver += ","
                        thor += ","
                        llistaTipusContinguts += ","
                        llistaPlantilles += "|"
                        llistaNodes += ","
                        llistaCodiWeb += "|"
                        llistaNodes2 += ","
                        llistaEstilsCSS += "|"
                        llistaDescripcions += "|"
                    End If
                    estructura += arrayEstructuraTMP(cont).Substring(0, arrayEstructuraTMP(cont).Length - 3).ToString() + formatea(i).ToString()
                    atributs += arrayAtributsTMP(cont)
                    chkimp += arraychkimpTMP(cont)
                    chkCND += arraychkCNDTMP(cont)
                    tver += arrayTVerTMP(cont)
                    thor += arrayTHorTMP(cont)
                    llistaTipusContinguts += arrayLlistaTipusContingutsTMP(cont)
                    llistaPlantilles += arrayLlistaPlantillesTMP(cont)
                    llistaNodes += arrayLlistaNodeTMP(cont)
                    llistaCodiWeb += arrayLlistaCodiWebTMP(cont)
                    llistaNodes2 += arrayLlistaNode2TMP(cont)
                    llistaEstilsCSS += arrayllistaEstilsCSSTMP(cont)
                    llistaDescripcions += arrayllistaDescripcionsTMP(cont)
                    i = i + 1
                End If
            Next cont
            ReDim Preserve arrayEstructuraTMP(i - 1)
            Array.Sort(arrayEstructuraTMP)
            lblEstructura.Text = GAIA.pintaEstructura(objconn, arrayEstructuraTMP, arrayAtributsTMP, Split(tver, ","), Split(thor, ","), 1, "", 1, "", Split(llistaDescripcions, "|"), arrayBuit, 1)
        End If
        'En el cas de que esborrar divisiÃ³ esborri completament tota la informaciÃ³ sobre les celÂ·les, 
        'crearÃ© la celÂ·la inicial
        If estructura = "" Then
            estructura = "0000"
            atributs = "ih"
            chkimp = "1"
            chkCND = "1"
            tver = "100"
            thor = "100"
            llistaDescripcions = ""
            lblEstructura.Text = GAIA.pintaEstructura(objconn, Split(estructura, ","), Split(atributs, ","), Split(tver, ","), Split(thor, ","), 1, "", 1, "", Split(llistaDescripcions, "|"), arrayBuit, 1)
        End If

        lblCodi.Text = "<script>document.getElementById('ultimaDivisio').value='';document.getElementById('estructura').value='" & estructura.ToString() & "';document.getElementById('atributs').value='" & atributs.ToString() & "';document.getElementById('llistaTVer').value='" & tver.ToString() & "';document.getElementById('llistachkimp').value='" & chkimp.ToString() & "';document.getElementById('llistachkCND').value='" & chkCND.ToString() & "';document.getElementById('llistaTHor').value='" & thor.ToString() & "';document.getElementById('llistaPlantilles').value='" & llistaPlantilles.ToString().Replace("'", "\'") & "';document.getElementById('llistaTipusContinguts').value='" & llistaTipusContinguts.ToString() & "';document.getElementById('llistaNodes').value='" & llistaNodes.ToString() & "';document.getElementById('llistaCodis').value='" & llistaCodiWeb.ToString() & "';document.getElementById('llistaNodes2').value='" & llistaNodes2.ToString() & "';document.getElementById('llistaCSS').value='" & llistaEstilsCSS.ToString() & "';document.getElementById('llistaDescripcions').value='" & llistaDescripcions.ToString().Replace("'", "\'") & "';</script>"


    End Sub 'clickEsborrarDivisio

    Protected Sub carregallistaEstilsCSS()
        Dim ds As New DataSet, qSQL As String, dv As DataView
        Dim element As ListItem
        qSQL = "select isnull(CSSDSCSS,'1') as CSSDSCSS,CSSINTIP,CSSINCOD,CSSDSTXT from METLCSS WITH(NOLOCK) ORDER BY CSSDSTXT"
        GAIA.bdr(objconn, qSQL, ds)



        'colors

        dv = ds.Tables(0).DefaultView

        ddlb_23.Items.Insert(0, New ListItem("", 0))
        For Each item In dv.Table.Rows
            If item("CSSINTIP") = 23 Then
                element = New ListItem(item("CSSDSTXT"), item("CSSINCOD"))
                If item("CSSDSCSS") <> "1" And item("CSSDSTXT") <> "blanc" Then
                    element.Attributes.Add("style", item("CSSDSCSS"))
                End If
                ddlb_23.Items.Add(element)
            End If
        Next item

        'mida lletra
        dv = ds.Tables(0).DefaultView
        ddlb_24.Items.Insert(0, New ListItem("", 0))
        For Each item In dv.Table.Rows
            If item("CSSINTIP") = 24 Then
                element = New ListItem(item("CSSDSTXT"), item("CSSINCOD"))
                'if item("CSSDSCSS")<>"1" AND instr(item("CSSDSCSS"),"FFFFFF")=0 THEN
                element.Attributes.Add("style", item("CSSDSCSS"))
                'END IF
                ddlb_24.Items.Add(element)
            End If
        Next item



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

        dv.RowFilter = "CSSINTIP=109"
        ddlb_109.DataSource = dv
        ddlb_109.DataTextField = "CSSDSTXT"
        ddlb_109.DataValueField = "CSSINCOD"
        ddlb_109.DataBind()
        ddlb_109.Items.Insert(0, New ListItem("", 0))

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

        dv.RowFilter = "CSSINTIP=117"
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



        ds.Dispose()
    End Sub 'carregallistaEstilsCSS




    Protected Sub canviIdioma(sender As Object, e As EventArgs)
        Dim codiIdioma As Integer
        codiIdioma = lstCanviIdioma.SelectedItem.Value
        If txtCodiNode.Text.Length > 0 Then
            carregaDades(txtCodiNode.Text, codiIdioma)
        Else
            carregaDades(0, codiIdioma)
        End If
        lstIdioma.SelectedItem.Selected = False
        lstIdioma.Items.FindByValue(codiIdioma).Selected = True


    End Sub 'canviIdioma




    Protected Sub carregaDades(ByVal codiNode As Integer, ByVal codiIdioma As Integer)
        Dim DS As DataSet
        Dim dbRow As DataRow

        Dim strTmp As String = ""

        estructura = ""
        atributs = ""
        Dim idiomaNoTrobat As Boolean = False
        DS = New DataSet()
        GAIA.bdr(objconn, "SELECT * FROM METLWEB WHERE WEBINNOD=" + codiNode.ToString() + " AND WEBINIDI=" + codiIdioma.ToString(), DS)

        'No he trobat l'idioma demanat i busco un altre
        If DS.Tables(0).Rows.Count = 0 Then
            GAIA.bdr(objconn, "SELECT * FROM METLWEB WHERE WEBINNOD=" + codiNode.ToString() + " ORDER BY WEBINIDI ASC ", DS)
            If DS.Tables(0).Rows.Count > 0 Then
                lblResultat.Text = ""
                GAIA.escriuResultat(objconn, lblResultat, "No s'ha trobat l'idioma demanat.", " <a href=""/GAIA/aspx/web/frmFullaWeb.aspx"" class=""txtneg12px"">Nova fulla web</a>")

                idiomaNoTrobat = True
            Else
                Response.Redirect("/GAIA2/aspx/web/frmplantilla.aspx?id=" & codiNode.ToString() & "&tipus=W")

                'Response.Redirect("http://lhintranet/gaia2/aspx/web/frmplantilla.aspx?id=" & codiNode.ToString() & "&tipus=W")
            End If
        End If
        If DS.Tables(0).Rows.Count > 0 Then
            dbRow = DS.Tables(0).Rows(0)
            WEBWNMTH.Text = dbRow("WEBWNMTH")
            WEBDTPUB.Text = dbRow("WEBDTPUB")
            If dbRow("WEBDTCAD") = CDate("01/01/1900") Then
                WEBDTCAD.Text = ""
            Else
                WEBDTCAD.Text = dbRow("WEBDTCAD")
            End If
            WEBDSTIT.value = dbRow("WEBDSTIT").replace("<p>", "").replace("</p>", "")
            WEBDSDES.value = dbRow("WEBDSDES")
            WEBDSPCL.Text = dbRow("WEBDSPCL")

            WEBDSEBO.Text = dbRow("WEBDSEBO")
            WEBDSFIT.Text = dbRow("WEBDSFIT").ToString()
            WEBDSURL.Text = dbRow("WEBDSURL").ToString()

            If ("S".Equals(dbRow("WEBTPHER").ToString())) Then
                WEBTPHER.Checked = True
            Else
                WEBTPHER.Checked = False
            End If
            If ("S".Equals(dbRow("WEBSWFRM").ToString())) Then
                WEBSWFRM.Checked = True
            Else
                WEBSWFRM.Checked = False
            End If
            If ("S".Equals(dbRow("WEBSWEML").ToString())) Then
                WEBSWEML.Checked = True
            Else
                WEBSWEML.Checked = False
            End If
            If ("S".Equals(dbRow("WEBSWSSL").ToString())) Then
                WEBSWSSL.Checked = True
            Else
                WEBSWSSL.Checked = False
            End If
            llistaDesc = dbRow("WEBDSDEC").ToString()

            'Carrego el select d'idiomes					
            If dbRow("WEBTPBUS") = "S" Then
                WEBTPBUS.Checked = True
            End If

            lblCodi.Text += "<script>document.getElementById('estructura').value='" & dbRow("WEBDSEST").ToString() & "'; document.getElementById('atributs').value='" & dbRow("WEBDSATR").ToString() & "';document.getElementById('llistaTVer').value='" & dbRow("WEBDSTVER").ToString() & "';document.getElementById('llistachkimp').value='" & dbRow("WEBDSIMP").ToString() & "';document.getElementById('llistachkCND').value='" & dbRow("WEBDSCND").ToString() & "';document.getElementById('llistaTHor').value='" & dbRow("WEBDSTHOR").ToString() & "';document.getElementById('llistaTipusContinguts').value='" & dbRow("WEBDSTCO").ToString() & "';document.getElementById('llistaCSS').value='" & dbRow("WEBDSCSS").ToString() & "';document.getElementById('llistaNodes').value='" & dbRow("WEBDSPLA").ToString() & "';document.getElementById('llistaNodes2').value='" & dbRow("WEBDSLCW").ToString() & "';document.getElementById('llistaDescripcions').value='" & llistaDesc.ToString().Replace("'", "\'") & "';"
            estructura = dbRow("WEBDSEST")
            atributs = dbRow("WEBDSATR")

            Dim llistaNodes, llistaNodes2 As String()
            Dim llistaPlantilles, llistaCodiWeb As String

            llistaNodes = Split(dbRow("WEBDSPLA"), ",")
            llistaNodes2 = Split(dbRow("WEBDSLCW"), ",")
            llistaPlantilles = ""
            llistaCodiWeb = ""
            Dim item, itemTMP As String
            Dim arrayTMP
            Dim cont As Integer = 0
            For Each item In llistaNodes
                If llistaPlantilles.Length > 0 Then
                    llistaPlantilles += "|"
                End If
                If (item.Trim().Length > 0) Then
                    arrayTMP = item.Split("|")
                    cont = 0
                    For Each itemTMP In arrayTMP
                        If itemTMP.Length > 0 Then
                            strTmp = GAIA.descNode(objconn, itemTMP)
                            If strTmp.Length > 0 Then
                                If cont > 0 Then
                                    llistaPlantilles += "  "
                                End If
                                llistaPlantilles += strTmp.Replace("'", "Â´").Replace("""", "Â´")
                                cont = +1
                            Else
                                llistaPlantilles += " "
                            End If
                        Else
                            llistaPlantilles += " "
                        End If
                    Next itemTMP
                Else
                    llistaPlantilles += " "
                End If
            Next item

            'llistaPlantilles+=strTMP.Replace("'","Â´").Replace("""","Â´")							
            For Each item In llistaNodes2
                If llistaCodiWeb.Length > 0 Then
                    llistaCodiWeb += "|"
                End If
                If (item.Trim().Length > 0) Then
                    For Each itemTMP In Split(item, "|")
                        strTmp = GAIA.descNode(objconn, itemTMP)
                        If strTmp.Length > 0 Then
                            llistaCodiWeb += strTmp.Replace("'", "Â´")
                            If InStr(item, "|") > 0 Then
                                llistaCodiWeb &= ","
                            End If
                        Else
                            llistaCodiWeb += " "
                        End If
                    Next itemTMP
                Else
                    llistaCodiWeb += " "
                End If
            Next item
            If dbRow("WEBDSCIP").ToString().Length > 0 Then
                gaiaCircuitPublicacioTxt.Text = GAIA.descNode(objconn, GAIA.obtenirFillRelacio(objconn, dbRow("WEBDSCIP")))
                gaiaCircuitPublicacioNodes.Text = dbRow("WEBDSCIP")
            End If
            If dbRow("WEBDSCIC").ToString().Length > 0 Then
                gaiaCircuitCaducitatTxt.Text = GAIA.descNode(objconn, GAIA.obtenirFillRelacio(objconn, dbRow("WEBDSCIC")))
                gaiaCircuitCaducitatNodes.Text = dbRow("WEBDSCIC")
            End If
            lblCodi.Text += "document.getElementById('llistaPlantilles').value='" + llistaPlantilles.ToString().Replace("'", "\'") + "';document.getElementById('llistaCodis').value='" + llistaCodiWeb + "';"
        End If

        If lblCodi.Text.Length > 0 Then
            lblCodi.Text += "</script>"
        End If

        DS.Dispose()
    End Sub 'carregaDades	

End Class