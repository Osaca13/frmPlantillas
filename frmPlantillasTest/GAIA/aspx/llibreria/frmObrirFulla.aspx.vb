Imports System.Data
Imports System.Data.OleDb
Imports KCommon.Net.FTP

Public Class frmObrirFulla
    Inherits System.Web.UI.Page

    '**********************************************************************
    '**********************************************************************
    '			F R M O B R I R F U L L A  (GAIA)
    '**********************************************************************
    '**********************************************************************
    Dim nif As String
    Public Shared objconn As OleDbConnection

    Private Sub Page_UnLoad(sender As Object, e As System.EventArgs) Handles MyBase.Unload
        GAIA.bdFi(objconn)
    End Sub 'Page_UnLoad	

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        'Dim objGAIA As New GAIA
        Dim codiRelacio As String
        Dim estilbody As String = ""
        Dim strCSSPantalla As String = ""
        Dim strCSSImpressora As String = ""
        Dim tagsMeta As String = ""
        Dim htmlPeu As String = ""
        Dim dbRow As DataRow
        Dim html, css As String
        Dim fDesti, urlDesti As String
        Dim resultat As Integer
        Dim dataSimulacio As DateTime
        Dim rel As New clsRelacio

        Dim width As Integer = 760
        objconn = GAIA.bdIni()
        GAIA.llistatWarnings = ""
        fDesti = ""
        urlDesti = ""
        html = ""
        css = ""
        Dim esForm As String = ""
        Dim esEML As String = "N"
        Dim esSSL As String = "N"
        Dim ds As DataSet
        Dim titol As String = ""
        Dim plantilla As New clsPlantilla
        Dim temps As Date
        Dim strError As String = "<div class=""missatgeErrorIntranet""><span class=""topEsquerraBlanc""></span><span class=""topDretaBlanc""></span><div class=""icona"">strmiss</div><span class=""bottomEsquerraBlanc""></span><span class=""bottomDretaBlanc""></span></div>"
        Dim strAvis As String = "<div class=""missatgeAvisIntranet""><span class=""topEsquerraBlanc""></span><span class=""topDretaBlanc""></span><div class=""icona"">strmiss</div><span class=""bottomEsquerraBlanc""></span><span class=""bottomDretaBlanc""></span></div>"
        Dim strOk As String = "<div class=""missatgeOkIntranet""><span class=""topEsquerraBlanc""></span><span class=""topDretaBlanc""></span><div class=""icona"">strmiss</div><span class=""bottomEsquerraBlanc""></span><span class=""bottomDretaBlanc""></span></div>"

        'If HttpContext.Current.User.Identity.Name.Length > 0 Then
        '    If (Session("nif") Is Nothing) Then
        '        Session("nif") = GAIA.nifUsuari(objconn, HttpContext.Current.User.Identity.Name).Trim()
        '    End If
        '    If String.IsNullOrEmpty(Session("codiOrg")) Then
        '        Session("CodiOrg") = GAIA.trobaNodeUsuari(objconn, Session("nif")).ToString().Trim()

        '    End If
        'End If
        Session("codiOrg") = "346231"

        If Not (Request("data") Is Nothing) Then
            dataSimulacio = CDate(Request("data"))
        Else
            dataSimulacio = Now
        End If

        If Not Page.IsPostBack Then
            'he de rebre com a paràmetre el codi de relació
            If Request("codiRelacio") Is Nothing Then
                lblResultat.Text &= strError.Replace("strmiss", "Error. Falta el paràmetre que apunta a la relació")
            Else
                codiRelacio = Request("codiRelacio")
                rel.bdget(objconn, codiRelacio)
                If rel.incod = 0 Then
                    lblResultat.Text &= strError.Replace("strmiss", "Error. Falta el paràmetre que apunta a la relació")
                Else
                    Select Case (rel.tipdsdes.Trim())
                        Case "fulla noticia"
                            lblCodi.Text = GAIA.obreFulla(objconn, rel, plantilla, Request("idioma"), 0, 0, 0, Now)
                        Case "fulla web"
                            Dim llistaDocuments As String()
                            Dim llistaDocumentsEliminar As String()
                            Dim heretaPropietatsWeb As Integer = 0
                            Dim ftpsession As Session
                            Dim strLlegenda As String
                            ds = New DataSet()
                            ' Teresa Añado UNION con METLWEB2
                            GAIA.bdr(objconn, "	SELECT RELINCOD, WEBINIDI, 1 AS versioGAIA FROM METLWEB INNER JOIN METLREL ON METLWEB.WEBINNOD = METLREL.RELINFIL INNER JOIN METLNOD ON METLREL.RELINFIL = METLNOD.NODINNOD INNER JOIN METLTIP ON METLNOD.NODCDTIP = METLTIP.TIPINTIP AND METLTIP.TIPDSDES LIKE 'fulla web' WHERE ((METLREL.RELCDHER LIKE '" + rel.cdher + "_" + rel.infil.ToString() + "%') or METLREL.RELINCOD=" + rel.incod.ToString() + " ) AND RELCDSIT<98 UNION SELECT RELINCOD, WEBINIDI, 2 AS versioGAIA  FROM METLWEB2 INNER JOIN METLREL ON METLWEB2.WEBINNOD = METLREL.RELINFIL INNER JOIN METLNOD ON METLREL.RELINFIL = METLNOD.NODINNOD INNER JOIN METLTIP ON METLNOD.NODCDTIP = METLTIP.TIPINTIP AND METLTIP.TIPDSDES LIKE 'fulla web' WHERE ((METLREL.RELCDHER LIKE '" + rel.cdher + "_" + rel.infil.ToString() + "%') or METLREL.RELINCOD=" + rel.incod.ToString() + " ) AND RELCDSIT<98 ", ds)

                            heretaPropietatsWeb = GAIA.heretarPropietatsWeb(objconn, codiRelacio, width)

                            For Each dbRow In ds.Tables(0).Rows

                                temps = Now

                                rel.bdget(objconn, dbRow("RELINCOD"))
                                resultat = GAIA.contingutEsPublicable(objconn, rel, dataSimulacio, dbRow("WEBINIDI"), 98)

                                If Request("publicar") = 1 Then
                                    html = ""
                                    css = ""
                                    fDesti = ""
                                    urlDesti = ""
                                    titol = ""
                                    estilbody = ""
                                    strCSSPantalla = ""
                                    strCSSImpressora = ""
                                    tagsMeta = ""
                                    htmlPeu = ""

                                    If dbRow("versioGAIA") = 2 Then
                                        html = GAIA.maquetarHTML(objconn, html, rel, fDesti, urlDesti, 0, llistaDocuments, Request("publicar"), dbRow("WEBINIDI"), rel, dataSimulacio, Session("CodiOrg"), tagsMeta, titol, estilbody, esForm, esEML, esSSL, htmlPeu, strCSSPantalla, strCSSImpressora)
                                    Else

                                        GAIA.obreFullaWeb(objconn, rel, html, css, heretaPropietatsWeb, width, 500, "", fDesti, urlDesti, 1, llistaDocuments, Request("publicar"), dbRow("WEBINIDI"), rel, dataSimulacio, 0, titol, 1, estilbody, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, True)
                                    End If

                                    If GAIA.llistatErrors <> "" Then
                                        lblResultat.Text &= strError.Replace("strmiss", "<span class=""bold"">&nbsp;Pàgina en " & IIf(dbRow("WEBINIDI") = 1, "català", IIf(dbRow("WEBINIDI") = 2, "castellà", "anglès")) & " no publicada per que s'han trobat errors.</span><br /><br /><span class=""bold"">Títol</span>:&nbsp;" & titol & "&nbsp;<span class=""bold"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />Adreça:</span>&nbsp;&nbsp;<a href=""http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + """>http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + "</a>" & "<span class=""""><br />Error: <br />" & GAIA.llistatErrors & "</span>")
                                        GAIA.llistatErrors = ""
                                    Else

                                        ftpsession = New Session()

                                        If InStr(fDesti, ".") > 0 Then

                                            If InStr(fDesti, ".vb") > 0 Or InStr(fDesti, ".js") > 0 Then
                                                GAIA.publica(objconn, html.Replace("<div>", "").Replace("</div>", "").Replace("<div  class="""">", ""), fDesti, llistaDocuments, llistaDocumentsEliminar, 1, rel, dbRow("WEBINIDI"), "", ftpsession)

                                            Else
                                                If dbRow("versioGAIA") = 2 Then
                                                    GAIA.publica(objconn, GAIA.preparaPagina2(objconn, html, css, heretaPropietatsWeb, titol, heretaPropietatsWeb, estilbody, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, dbRow("WEBINIDI"), urlDesti), fDesti, llistaDocuments, llistaDocumentsEliminar, 1, rel, dbRow("WEBINIDI"), "", ftpsession)
                                                Else
                                                    GAIA.publica(objconn, GAIA.preparaPagina(objconn, html, css, heretaPropietatsWeb, titol, heretaPropietatsWeb, estilbody, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, dbRow("WEBINIDI"), urlDesti), fDesti, llistaDocuments, llistaDocumentsEliminar, 1, rel, dbRow("WEBINIDI"), "", ftpsession)
                                                End If
                                            End If

                                            If Trim(GAIA.llistatWarnings) <> "" Then
                                                lblResultat.Text &= strAvis.Replace("strmiss", "<span class=""bold"">&nbsp;Pàgina en " & IIf(dbRow("WEBINIDI") = 1, "català", IIf(dbRow("WEBINIDI") = 2, "castellà", "anglès")) & " no publicada per que ja existia.</span><br /><br /><span class=""bold"">Títol</span>:&nbsp;" & titol & "&nbsp;<span class=""bold"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />Adreça:</span>&nbsp;&nbsp;<a href=""http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + """>http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + "</a><br /><span class=""bold"">Temps:</span> " & fnTemps(temps, Now))



                                                GAIA.llistatWarnings = ""
                                            Else
                                                'GAIA.escriuResultat(objConn,lblResultat , "<span class=""bold"">&nbsp;Pàgina en " &  iif(dbrow("WEBINIDI")=1, "català" , iif(dbrow("WEBINIDI")=2, "castellà", iif(dbrow("WEBINIDI")=3, "anglès", "francès"))) & " publicada amb èxit.</span>","<br /><br /><span class=""bold"">Títol</span>:&nbsp;" & titol & "&nbsp;<span class=""bold"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />Adreça:</span>&nbsp;&nbsp;<a href=""http" & iif(esSSL="S","s","") & "://"+urlDesti.tostring()+""">http" & iif(esSSL="S","s","") & "://"+urlDesti.toString()+"</a><br /><span class=""bold"">Temps: " &   fnTemps(temps, Now) & "</span> ",0)		


                                                lblResultat.Text &= strOk.Replace("strmiss", "<span class=""bold"">&nbsp;Pàgina en " & IIf(dbRow("WEBINIDI") = 1, "català", IIf(dbRow("WEBINIDI") = 2, "castellà", IIf(dbRow("WEBINIDI") = 3, "anglès", "francès"))) & " publicada amb èxit.</span><br /><br /><span class=""bold"">Títol</span>:&nbsp;" & titol & "&nbsp;<span class=""bold"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />Adreça:</span>&nbsp;&nbsp;<a href=""http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + """>http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + "</a><br /><span class=""bold"">Temps:</span>  " & fnTemps(temps, Now))
                                                GAIA.log(objconn, rel, Session("codiOrg"), "", 8)
                                            End If
                                        End If

                                        Try
                                            ftpsession.Close()
                                        Catch
                                        End Try



                                        Select Case resultat
                                            Case 3 'publicar ok												
                                                    'Ja ho he posat abans
                                            Case 2 'pâgina pendent de publicar, però no publicable perque encara no ha arribat la data
                                                lblResultat.Text &= strError.Replace("strmiss", "La pàgina no es pot publicar encara. Proveu l'opció <em><b>obrir</b></em> per poder simular com es veurà la pàgina en una data concreta")
        'GAIA.escriuResultat(objConn,lblResultat , "<span class=""gtxt t70 vermell"">La pàgina no es pot publicar encara. Proveu l'opció <em><b>obrir</b></em> per poder simular com es veurà la pàgina en una data concreta</span><br><br>","",0)
                                            Case 4 'Pàgina caducada
                                                lblResultat.Text &= strError.Replace("strmiss", "La pàgina està caducada. Proveu l'opció <em><b>obrir</b></em> per poder simular com es veurà la pàgina en una data concreta")
                                                'GAIA.escriuResultat(objConn,lblResultat , "<span class=""gtxt t70 vermell"">La pàgina està caducada. Proveu l'opció <em><b>obrir</b></em> per poder simular com es veurà la pàgina en una data concreta</span><br><br>","",0)
                                        End Select
                                    End If
                                Else

                                    lblResultat.Text += css + "<div id=""contenidor"">" + html + "</div>"
                                    strLlegenda = "<br><table  border=""0""><tr><td width=""220"" ><div align=""right"">Contingut caducat:<div class=""vermell"">Imatge negativa o color</div></div></td><td width=""20"" ><div style=""background-color: #009900;"">&nbsp;&nbsp;<br>&nbsp;&nbsp;</td><td width=""290""><div align=""right""><div class="""">Contingut pendent de publicar:<div class="" vermell""> Imatge cap avall o color </div></div></div></td><td width=""20"" ><div style=""background-color: #CC66FF;"">&nbsp;&nbsp;<br>&nbsp;&nbsp;</div></td><td></tr></table>"
                                    Select Case resultat
                                        Case 3 'publicar ok
                                            heretaPropietatsWeb = 0
                                            heretaPropietatsWeb = GAIA.heretarPropietatsWeb(objconn, codiRelacio, width) '								
                                            GAIA.obreFullaWeb(objconn, rel, html, css, heretaPropietatsWeb, width, 500, "", fDesti, urlDesti, 1, llistaDocuments, 0, 1, rel, dataSimulacio, 0, titol, 1, estilbody, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, True)
                                            lblResultat.Text &= strOk.Replace("strmiss", "<span class=""bold""> Es mostra la pàgina a data d'avui. Els continguts que esperin una aprovació es mostraran com si s'acceptés.</span>" + strLlegenda + "<form name=""f1"" method=""get""><input type=""hidden""  class="""" name=""codirelacio"" value=""" + Request("codiRelacio").ToString() + """><input type=""hidden"" name=""idioma""  value=""" + Request("idioma").ToString() + """>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=""bold"">Data:</span>&nbsp;<input type=""text"" class="""" name=""data"" maxlength=""10"" size=""10"" value=""" + dataSimulacio + """>&nbsp;<input type=""submit"" name=""submit"" value=""Simular Pàgina en data seleccionada"" class=""""></form>")

                                            'GAIA.escriuResultat(objConn,lblResultat , "<span class=""gtxt t70 bold""> Es mostra la pàgina a data d'avui. Els continguts que esperin una aprovació es mostraran com si s'acceptés.</span>"+strLlegenda+"<form name=""f1"" method=""get""><input type=""hidden""  class=""gtxt t70"" name=""codirelacio"" value="""+ Request("codiRelacio").toString() + """><input type=""hidden"" name=""idioma""  value="""+ Request("idioma").toString() + """>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=""gtxt t70 bold"">Data:</span>&nbsp;<input type=""text"" class=""gtxt t70"" name=""data"" maxlength=""10"" size=""10"" value="""+ dataSimulacio + """>&nbsp;<input type=""submit"" name=""submit"" value=""Simular Pàgina en data seleccionada"" class=""gtxt t70""></form>","",0)
                                            lblResultat.Text += css + "<div id=""contenidor"">" + html + "</div>"
                                        Case 4 'Pàgina caducada		
                                            lblResultat.Text &= strError.Replace("strmiss", "<span class="""">La pàgina està caducada.</span><form name=""f1"" method=""get""><input type=""hidden"" name=""codirelacio"" value=""" + Request("codiRelacio").ToString() + """ class=""""><input type=""hidden"" name=""idioma""  value=""" + Request("idioma").ToString() + """>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=""bold"">Data:</span> <input type=""text"" name=""data"" maxlength=""10"" size=""10"" class="""" value=""" + dataSimulacio + """>&nbsp;<input type=""submit"" name=""submit"" class="""" value=""Simular Pàgina en data seleccionada""></form>")

                                            'GAIA.escriuResultat(objConn,lblResultat , "<span class=""gtxt t70"">La pàgina està caducada.</span><form name=""f1"" method=""get""><input type=""hidden"" name=""codirelacio"" value="""+ Request("codiRelacio").toString() + """ class=""gtxt t70""><input type=""hidden"" name=""idioma""  value="""+ Request("idioma").toString() + """>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=""gtxt t70 bold"">Data:</span> <input type=""text"" name=""data"" maxlength=""10"" size=""10"" class=""gtxt t70"" value="""+ dataSimulacio+ """>&nbsp;<input type=""submit"" name=""submit"" class=""gtxt t70"" value=""Simular Pàgina en data seleccionada""></form>","",0)
                                        Case Else 'pâgina pendent de publicar o en estat inicial										
                                            lblResultat.Text &= strAvis.Replace("strmiss", "<span class="""">Es mostra la pàgina a data d'avui.</span>" + strLlegenda + "<form name=""f1"" method=""get""><input type=""hidden"" name=""codirelacio"" value=""" + Request("codiRelacio").ToString() + """ class=""""><input type=""hidden"" name=""idioma""  value=""" + Request("idioma").ToString() + """>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=""bold"">Data:</span> <input type=""text"" name=""data"" maxlength=""10"" size=""10"" class="""" value=""" + dataSimulacio + """>&nbsp;<input type=""submit"" name=""submit"" value=""Simular Pàgina en data seleccionada"" class=""""></form>")
                                            'GAIA.escriuResultat(objConn,lblResultat , "<span class=""gtxt t70"">Es mostra la pàgina a data d'avui.</span>"+strLlegenda+"<form name=""f1"" method=""get""><input type=""hidden"" name=""codirelacio"" value="""+ Request("codiRelacio").toString() + """ class=""gtxt t70""><input type=""hidden"" name=""idioma""  value="""+ Request("idioma").toString() + """>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=""gtxt t70 bold"">Data:</span> <input type=""text"" name=""data"" maxlength=""10"" size=""10"" class=""gtxt t70"" value="""+ dataSimulacio + """>&nbsp;<input type=""submit"" name=""submit"" value=""Simular Pàgina en data seleccionada"" class=""gtxt t70""></form>","",0)			
                                            lblResultat.Text += css + "<div id=""contenidor"">" + html + "</div>"
                                    End Select
                                End If 'espublicable
                            Next dbRow
                            ds.Dispose()
                        Case "node web"
                            If Request("publicar") = 1 Then
                                Dim llistaDocuments As String()
                                Dim llistaDocumentsEliminar As String()
                                Dim heretaPropietatsWeb As Integer = 0
                                Dim ftpsession As Session

                                ds = New DataSet()
                                'Teresa Añado UNION con METLWEB2
                                GAIA.bdr(objconn, "	SELECT RELINCOD, WEBINIDI, WEBINNOD, 1 AS versioGAIA  FROM METLWEB INNER JOIN METLREL ON  METLREL.RELCDRSU=" & rel.incod & " AND RELCDSIT<98 INNER JOIN METLNOD ON METLREL.RELINFIL = METLNOD.NODINNOD INNER JOIN METLTIP ON METLNOD.NODCDTIP = METLTIP.TIPINTIP AND METLTIP.TIPDSDES LIKE 'fulla web' WHERE METLWEB.WEBINNOD = METLREL.RELINFIL UNION SELECT RELINCOD, WEBINIDI , WEBINNOD, 2 AS versioGAIA FROM METLWEB2 INNER JOIN METLREL ON  METLREL.RELCDRSU=" & rel.incod & " AND RELCDSIT<98 INNER JOIN METLNOD ON METLREL.RELINFIL = METLNOD.NODINNOD INNER JOIN METLTIP ON METLNOD.NODCDTIP = METLTIP.TIPINTIP AND METLTIP.TIPDSDES LIKE 'fulla web' WHERE METLWEB2.WEBINNOD = METLREL.RELINFIL ORDER BY WEBINNOD", ds)

                                If ds.Tables(0).Rows.Count = 0 Then
                                    GAIA.escriuResultat(objconn, lblResultat, "<span style=""margin-left: 8px;"">El node web seleccionat no conté fulles web.</span><br><br>", "", 0)
                                Else
                                    For Each dbRow In ds.Tables(0).Rows
                                        temps = Now
                                        ftpsession = New Session()
                                        rel = New clsRelacio
                                        urlDesti = ""
                                        html = ""
                                        css = ""
                                        fDesti = ""
                                        urlDesti = ""
                                        titol = ""
                                        estilbody = ""
                                        strCSSPantalla = ""
                                        strCSSImpressora = ""
                                        tagsMeta = ""
                                        htmlPeu = ""


                                        rel.bdget(objconn, dbRow("RELINCOD"))
                                        resultat = GAIA.contingutEsPublicable(objconn, rel, dataSimulacio, dbRow("WEBINIDI"), 98)


                                        heretaPropietatsWeb = GAIA.heretarPropietatsWeb(objconn, rel.incod, width)

                                        If dbRow("versioGAIA") = 2 Then
                                            html = GAIA.maquetarHTML(objconn, html, rel, fDesti, urlDesti, 0, llistaDocuments, Request("publicar"), dbRow("WEBINIDI"), rel, dataSimulacio, Session("CodiOrg"), tagsMeta, titol, estilbody, esForm, esEML, esSSL, htmlPeu, strCSSPantalla, strCSSImpressora)
                                        Else
                                            GAIA.obreFullaWeb(objconn, rel, html, css, heretaPropietatsWeb, width, 500, "", fDesti, urlDesti, 1, llistaDocuments, Request("publicar"), dbRow("WEBINIDI"), rel, dataSimulacio, 0, titol, 1, estilbody, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, True)
                                        End If

                                        If GAIA.llistatErrors <> "" Then

                                            lblResultat.Text &= strError.Replace("strmiss", "<span class=""bold"">&nbsp;Pàgina en " & IIf(dbRow("WEBINIDI") = 1, "català", IIf(dbRow("WEBINIDI") = 2, "castellà", "anglès")) & " no publicada per que s'han trobat errors. </span><span class="""">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br /><br /><span class=""bold"">Títol</span>:&nbsp;" & titol & "</span>&nbsp;<span class="""">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />Adreça:</span>&nbsp;&nbsp;<a href=""http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + """>http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + "</a><span class=""""><br />Error: <br />" & GAIA.llistatErrors & "</span>")

                                            GAIA.llistatErrors = ""
                                        Else
                                            If InStr(fDesti, ".") > 0 Then
                                                If dbRow("versioGAIA") = 2 Then
                                                    GAIA.publica(objconn, GAIA.preparaPagina2(objconn, html, css, heretaPropietatsWeb, titol, heretaPropietatsWeb, estilbody, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, dbRow("WEBINIDI"), urlDesti), fDesti, llistaDocuments, llistaDocumentsEliminar, 1, rel, dbRow("WEBINIDI"), "", ftpsession)
                                                Else
                                                    GAIA.publica(objconn, GAIA.preparaPagina(objconn, html, css, heretaPropietatsWeb, titol, heretaPropietatsWeb, estilbody, esForm, esEML, esSSL, tagsMeta, htmlPeu, strCSSPantalla, strCSSImpressora, dbRow("WEBINIDI"), urlDesti), fDesti, llistaDocuments, llistaDocumentsEliminar, 1, rel, dbRow("WEBINIDI"), "", ftpsession)
                                                End If
                                                If Trim(GAIA.llistatWarnings) <> "" Then
                                                    'GAIA.escriuResultat(objConn,lblResultat , "<span class=""gtxt t70 bold"">&nbsp;Pàgina en " & iif(dbrow("WEBINIDI")=1, "català" , iif(dbrow("WEBINIDI")=2, "castellà", "anglès")) & " no publicada per que ja existia.</span><span class=""gtxt t70 negre"">&nbsp;&nbsp;&nbsp;&nbsp;<br /><br /><span class=""bold"">Títol</span>:&nbsp;" & titol & "</span>&nbsp;<span class=""gtxt t70 bold negre"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />Adreça:</span>&nbsp;&nbsp;<a class=""gtxt yesdeco t70 grana"" href=""http" & iif(esSSL="S","s","") & "://"+urlDesti.tostring()+""">http" & iif(esSSL="S","s","") & "://"+urlDesti.toString()+"</a><br /><span class=""gtxt t70 bold negre"">Temps: " &   fnTemps(temps, Now) & "</span> ","<span class=""gtxt t70"">" & GAIA.llistatWarnings & "</span><br><br>",0)										
                                                    lblResultat.Text &= strAvis.Replace("strmiss", "<span class=""bold"">&nbsp;Pàgina en " & IIf(dbRow("WEBINIDI") = 1, "català", IIf(dbRow("WEBINIDI") = 2, "castellà", "anglès")) & " no publicada per que ja existia.</span><span class="""">&nbsp;&nbsp;&nbsp;&nbsp;<br /><br /><span class=""bold"">Títol</span>:&nbsp;" & titol & "</span>&nbsp;<span class=""bold"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />Adreça:</span>&nbsp;&nbsp;<a href=""http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + """>http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + "</a><br /><span class=""bold"">Temps:</span> " & fnTemps(temps, Now))
                                                    GAIA.llistatWarnings = ""
                                                Else
                                                    lblResultat.Text &= strOk.Replace("strmiss", "<span class=""bold"">&nbsp;Pàgina en " & IIf(dbRow("WEBINIDI") = 1, "català", IIf(dbRow("WEBINIDI") = 2, "castellà", IIf(dbRow("WEBINIDI") = 3, "anglès", "francès"))) & " publicada amb èxit.</span><span class="""">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br /><br /><span class=""bold"">Títol</span>:&nbsp;" & titol & "</span>&nbsp;<span class="""">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />Adreça:</span>&nbsp;&nbsp;<a class="""" href=""http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + """>http" & IIf(esSSL = "S", "s", "") & "://" + urlDesti.ToString() + "</a><br /><span class="""">Temps:</span>  " & fnTemps(temps, Now))


                                                    'GAIA.escriuResultat(objConn,lblResultat , "<span class=""gtxt t70 bold"">&nbsp;Pàgina en " & iif(dbrow("WEBINIDI")=1, "català" , iif(dbrow("WEBINIDI")=2, "castellà", iif(dbrow("WEBINIDI")=3, "anglès", "francès"))) & " publicada amb èxit.</span>","<span class=""gtxt t70 negre"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br /><br /><span class=""bold"">Títol</span>:&nbsp;" & titol & "</span>&nbsp;<span class=""gtxt t70 negre"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />Adreça:</span>&nbsp;&nbsp;<a class=""gtxt yesdeco t70 grana"" href=""http" & iif(esSSL="S","s","") & "://"+urlDesti.tostring()+""">http" & iif(esSSL="S","s","") & "://"+urlDesti.toString()+"</a><br /><span class=""gtxt t70 bold negre"">Temps: " &   fnTemps(temps, Now) & "</span> ",0)	
                                                    GAIA.log(objconn, rel, Session("codiOrg"), "", 8)
                                                End If
                                            End If
                                            Select Case resultat
                                                Case 3 'publicar ok												

        'GAIA.escriuResultat(objConn,lblResultat , "<span class=""gtxt t70 bold"">&nbsp;Pàgina publicada amb èxit.</span>","<span class=""gtxt t70"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Adreça:</span>&nbsp;&nbsp;<a class=""gtxt yesdeco t70 grana"" href=""http://"+urlDesti.tostring()+""">http://"+urlDesti.toString()+"</a><br><br>",0)								
                                                Case 2 'pâgina pendent de publicar, però no publicable perque encara no ha arribat la data

                                                    GAIA.escriuResultat(objconn, lblResultat, "<span class=""gtxt t70 vermell"">La pàgina no es pot publicar encara. Proveu l'opció <em><b>obrir</b></em> per poder simular com es veurà la pàgina en una data concreta</span><br><br>", "", 0)
                                                Case 4 'Pàgina caducada

                                                    GAIA.escriuResultat(objconn, lblResultat, "<span class=""gtxt t70 vermell"">La pàgina està caducada. Proveu l'opció <em><b>obrir</b></em> per poder simular com es veurà la pàgina en una data concreta</span><br><br>", "", 0)
                                            End Select
                                            llistaDocuments = Nothing
                                            llistaDocumentsEliminar = Nothing
                                            html = ""
                                            css = ""
                                            fDesti = ""
                                            urlDesti = ""
                                            titol = ""
                                            Try
                                                ftpsession.Close()
                                            Catch
                                            End Try
                                        End If
                                    Next dbRow
                                End If
                                ds.Dispose()

                            End If


                        Case "fulla document"
                            lblCodi.Text = GAIA.obreFulla(objconn, rel, plantilla, Request("idioma"), 0, 0, 0)
                        Case "node document"
                            lblCodi.Text = GAIA.obreFulla(objconn, rel, plantilla, Request("idioma"), 0, 0, 0)
                        Case "fulla catalegServeis"
                            lblCodi.Text = GAIA.obreFulla(objconn, rel, plantilla, Request("idioma"), 0, 0, 0)
                        Case "fulla contractació"
                            lblCodi.Text = GAIA.obreFulla(objconn, rel, plantilla, Request("idioma"), 0, 0, 0)
                        Case "fulla agenda"
                            lblCodi.Text = GAIA.obreFulla(objconn, rel, plantilla, Request("idioma"), 0, 0, 0)
                        Case "fulla missatge"
                            lblCodi.Text = GAIA.obreFulla(objconn, rel, plantilla, Request("idioma"), 0, 0, 0)


                        Case Else

                            GAIA.escriuResultat(objconn, lblResultat, "<span class=""bold"">Error: No es poden ni publicar ni obrir aquest tipus de nodes. No s'ha implementat com obrir aquesta fulla</span>", "", 0)
                    End Select
                End If

            End If
        Else
            'no hago nada. 
        End If
    End Sub 'Page_Load


    Public Shared Function fnTemps(ByVal tIni As Date, ByVal tfi As Date) As String
        Dim ms As Integer = tfi.Millisecond - tIni.Millisecond
        If ms < 0 Then
            ms = (1000 - tIni.Millisecond) + tfi.Millisecond
        End If
        Return (DateDiff(DateInterval.Second, tIni, tfi) & "," & ms & " segons")
    End Function
End Class