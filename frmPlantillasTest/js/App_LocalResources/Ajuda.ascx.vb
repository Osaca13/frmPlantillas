Public Class Ajuda
    Inherits UserControl

    Protected LabelText As Label

    Public Property Text As String
        Get
            Return LabelText.Text
        End Get
        Set(Value As String)
            LabelText.Text = Value
        End Set
    End Property
End Class
