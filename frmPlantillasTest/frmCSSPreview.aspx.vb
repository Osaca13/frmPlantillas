Imports System.Data
Imports System.Data.OleDb


Public Class frmFullaWeb
    Inherits System.Web.UI.Page

    Public Shared nif As String
    Public Shared objconn As OleDbConnection



    Private Sub Page_UnLoad(sender As Object, e As System.EventArgs) Handles MyBase.UnLoad
        GAIA.bdFi(objConn)
    End Sub 'Page_UnLoad

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        objConn = GAIA.bdIni()
        If HttpContext.Current.User.Identity.Name.length > 0 Then
            If (Session("nif") Is Nothing) Then
                Session("nif") = GAIA.nifUsuari(objConn, HttpContext.Current.User.Identity.Name).Trim()
            End If
            If Session("codiOrg") Is Nothing Then
                session("CodiOrg") = GAIA.trobaNodeUsuari(objConn, Session("nif")).tostring().Trim()
            End If
        End If
        nif = Session("nif").Trim()

        Dim estructura, tver, thor, tcontinguts, tplant As String

        Dim css, html As String
        Dim tMaxHor As Integer = 770
        Dim tMaxVer As Integer = 450

        css = ""
        html = ""

        estructura = Request("estructura")
        thor = Request("thor")
        tver = Request("tver")
        tcontinguts = Request("tcontinguts")
        tplant = Request("llistaPlantilles")

        lblEstructura.Text = GAIA.dibuixaPreview(objConn, estructura, thor, tver, tcontinguts, tplant, html, css, tMaxHor, tMaxVer, "e", 0, 0, "", "")
    End Sub 'Page_Load
End Class



