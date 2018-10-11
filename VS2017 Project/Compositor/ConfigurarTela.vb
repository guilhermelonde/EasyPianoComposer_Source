'--- ConfigurarTela ---
'Autor: Guilherme Pereira Porto Londe
'última modificação: 11 de outubro de 2018

Imports Color = System.Drawing.Color

Public Class ConfigurarTela

    Private ObjetoIdioma As Idioma
    Public EspacamentoNotasDouble As Double
    Public DiametroNotaDouble As Double
    Public Cores As New List(Of Color)
    Private SP As SubPlayer
    Private CD As New System.Windows.Forms.ColorDialog

    Public Sub New(ByRef NovoObjetoIdioma As Idioma, ByVal NovoEspacamento As Double, ByVal NovoDiametro As Double, ByRef NovoCores As List(Of Color))
        InitializeComponent()
        ObjetoIdioma = NovoObjetoIdioma
        EspacamentoNotasDouble = NovoEspacamento
        DiametroNotaDouble = NovoDiametro
        SelecionaPadrao.Items.Clear()
        SelecionaPadrao.Items.Add("1")
        SelecionaPadrao.Items.Add("2")
        SelecionaPadrao.Items.Add("")
        For i = 0 To NovoCores.Count - 1
            Cores.Add(NovoCores.Item(i))
        Next
    End Sub

    Public Sub setSubPlayer(ByRef novoSubPlayer As SubPlayer)
        SP = novoSubPlayer
    End Sub

    Public Sub mostrar()
        EspacamentoNotas.Value = EspacamentoNotasDouble
        DiametroNota.Value = DiametroNotaDouble
        LabelDiametro.Text = ObjetoIdioma.Entrada(35)
        LabelEspacamento.Text = ObjetoIdioma.Entrada(36)
        Me.Text = ObjetoIdioma.Entrada(34)
        Aplicar.Text = ObjetoIdioma.Entrada(37)
        Cancelar.Text = ObjetoIdioma.Entrada(38)
        LabelPadrao.Text = ObjetoIdioma.Entrada(39)
        PadraoLimite.Text = ObjetoIdioma.Entrada(39)
        LabelCores.Text = ObjetoIdioma.Entrada(41)
        LabelLimites.Text = ObjetoIdioma.Entrada(40)
        LabelEspacamento.Text = ObjetoIdioma.Entrada(42)
        LabelDiametro.Text = ObjetoIdioma.Entrada(43)
        LabelFundo.Text = ObjetoIdioma.Entrada(44)
        LabelNota.Text = ObjetoIdioma.Entrada(45)
        LabelInicioFim.Text = ObjetoIdioma.Entrada(46)
        LabelSeparadorVertical.Text = ObjetoIdioma.Entrada(47)
        LabelDestacada.Text = ObjetoIdioma.Entrada(48)
        LabelSelecionada.Text = ObjetoIdioma.Entrada(49)
        LabelCursor.Text = ObjetoIdioma.Entrada(50)
        LabelPadrao.Text = ObjetoIdioma.Entrada(55)
        For i = 0 To Cores.Count - 1
            Me.Controls("Cor" & CStr(i + 1)).BackColor = Cores.Item(i)
        Next
        Me.Show()
    End Sub

    Private Sub Confirmar_Click(sender As Object, e As EventArgs) Handles Confirmar.Click
        Aplicar.PerformClick()
        Me.Close()
    End Sub

    Public Sub AtualizarTela()
        SP.AtualizaTamanhoTabela(EspacamentoNotasDouble, DiametroNotaDouble / 2, Cores)
    End Sub

    Private Sub Aplicar_Click(sender As Object, e As EventArgs) Handles Aplicar.Click
        EspacamentoNotasDouble = EspacamentoNotas.Value
        DiametroNotaDouble = DiametroNota.Value
        For i = 0 To Cores.Count - 1
            Cores.Item(i) = Me.Controls("Cor" & CStr(i + 1)).BackColor
        Next
        AtualizarTela()
    End Sub

    Private Sub Cancelar_Click(sender As Object, e As EventArgs) Handles Cancelar.Click
        Me.Close()
    End Sub

    Private Sub Me_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Me.Hide()
    End Sub

    Private Sub PadraoLimite_Click(sender As Object, e As EventArgs) Handles PadraoLimite.Click
        EspacamentoNotasDouble = 5
        DiametroNotaDouble = 5.6
        EspacamentoNotas.Value = EspacamentoNotasDouble
        DiametroNota.Value = DiametroNotaDouble
    End Sub

    Private Sub Cor_Click(sender As Object, e As EventArgs) Handles Cor1.Click, Cor2.Click, Cor3.Click, Cor4.Click, Cor5.Click, Cor6.Click, Cor7.Click
        If CD.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            Me.Controls(sender.Name).backColor = CD.Color
        End If
    End Sub

    Private Sub SelecionaPadrao_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SelecionaPadrao.SelectedIndexChanged
        If SelecionaPadrao.SelectedIndex >= 0 And SelecionaPadrao.SelectedIndex <= 1 Then
            PadraoCor(SelecionaPadrao.SelectedIndex)
            For i = 0 To Cores.Count - 1
                Me.Controls("Cor" & CStr(i + 1)).BackColor = Cores.Item(i)
            Next
        End If
    End Sub

    Private Sub SelecionaPadrao_Click(sender As Object, e As EventArgs) Handles SelecionaPadrao.Click
        SelecionaPadrao.SelectedIndex = 2
    End Sub

    Private Sub PadraoCor(ByVal i As Integer)
        Cores.Clear()
        Select Case i
            Case 0
                Cores.Add(Color.FromArgb(149, 154, 143))
                Cores.Add(Color.Beige)
                Cores.Add(Color.MediumBlue)
                Cores.Add(Color.Red)
                Cores.Add(Color.DimGray)
                Cores.Add(Color.Gray)
                Cores.Add(Color.Black)
            Case 1
                Cores.Add(Color.FromArgb(0, 0, 0))
                Cores.Add(Color.FromArgb(113, 45, 255))
                Cores.Add(Color.FromArgb(128, 255, 255))
                Cores.Add(Color.FromArgb(125, 242, 255))
                Cores.Add(Color.FromArgb(64, 0, 64))
                Cores.Add(Color.FromArgb(64, 0, 64))
                Cores.Add(Color.FromArgb(64, 0, 64))
        End Select
    End Sub

End Class