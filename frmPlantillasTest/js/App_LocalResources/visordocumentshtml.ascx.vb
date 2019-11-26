Imports System.Runtime.Remoting.Messaging

Public Class visordocumentshtml
    Inherits System.Web.UI.UserControl
    Protected WithEvents cntrl As Header
    Dim _codiRelacio As String
    Property codiRelacio() As String
        Get
            Return _codiRelacio
        End Get
        Set(ByVal value As String)
            _codiRelacio = value
        End Set
    End Property
End Class

