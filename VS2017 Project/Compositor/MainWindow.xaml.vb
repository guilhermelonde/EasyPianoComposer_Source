Class MainWindow

    'Não implementar código nesta classe

    Public Sub New()
        Me.Hide()
        Dim TP As New TelaPrincipal
        TP.ShowDialog()
        Me.Close()
    End Sub

End Class
