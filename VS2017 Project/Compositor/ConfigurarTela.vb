'--- ConfigurarTela ---
'Autor: Guilherme Pereira Porto Londe
'última modificação: 18 de maio de 2019

Imports Color = System.Drawing.Color

Public Class ConfigurarTela

    Private ObjetoIdioma As Idioma
    Public EspacamentoNotasDouble As Double
    Public EspacamentoNotasDoubleBackup As Double
    Public DiametroNotaDouble As Double
    Public DiametroNotaDoubleBackup As Double
    Public Cores As New List(Of Color)
    Public CoresBackup As New List(Of Color)
    Private Const NumTemas = 5
    Private SP As SubPlayer
    Private CD As New System.Windows.Forms.ColorDialog
    Private houveSimulacao

    Public Sub New(ByRef NovoObjetoIdioma As Idioma, ByVal NovoEspacamento As Double, ByVal NovoDiametro As Double, ByRef NovoCores As List(Of Color))
        InitializeComponent()
        ObjetoIdioma = NovoObjetoIdioma
        EspacamentoNotasDouble = NovoEspacamento
        EspacamentoNotasDoubleBackup = NovoEspacamento
        DiametroNotaDouble = NovoDiametro
        DiametroNotaDoubleBackup = NovoDiametro
        SelecionaPadrao.Items.Clear()
        For i = 1 To NumTemas
            SelecionaPadrao.Items.Add(i.ToString())
        Next
        SelecionaPadrao.Items.Add("")
        For i = 0 To NovoCores.Count - 1
            Cores.Add(NovoCores.Item(i))
            CoresBackup.Add(NovoCores.Item(i))
        Next
        houveSimulacao = False
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
        Simular.Text = ObjetoIdioma.Entrada(37)
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
        Simular.PerformClick()
        houveSimulacao = False
        CoresBackup.Clear()
        For i = 0 To Cores.Count - 1
            CoresBackup.Add(Cores.Item(i))
        Next
        DiametroNotaDoubleBackup = DiametroNotaDouble
        EspacamentoNotasDoubleBackup = EspacamentoNotasDouble
        Me.Close()
    End Sub

    Public Sub AtualizarTela()
        SP.AtualizaTamanhoTabela(EspacamentoNotasDouble, DiametroNotaDouble / 2, Cores)
    End Sub

    Private Sub Simular_Click(sender As Object, e As EventArgs) Handles Simular.Click
        EspacamentoNotasDouble = EspacamentoNotas.Value
        DiametroNotaDouble = DiametroNota.Value
        For i = 0 To Cores.Count - 1
            Cores.Item(i) = Me.Controls("Cor" & CStr(i + 1)).BackColor
        Next
        houveSimulacao = True
        AtualizarTela()
    End Sub

    Private Sub Cancelar_Click(sender As Object, e As EventArgs) Handles Cancelar.Click
        If (houveSimulacao) Then
            Cores.Clear()
            For i = 0 To CoresBackup.Count - 1
                Cores.Add(CoresBackup.Item(i))
            Next
            EspacamentoNotasDouble = EspacamentoNotasDoubleBackup
            DiametroNotaDouble = DiametroNotaDoubleBackup
            SP.AtualizaTamanhoTabela(EspacamentoNotasDouble, DiametroNotaDouble / 2, Cores)
            houveSimulacao = False
        End If
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
        If SelecionaPadrao.SelectedIndex >= 0 And SelecionaPadrao.SelectedIndex < NumTemas Then
            PadraoCor(SelecionaPadrao.SelectedIndex)
        End If
    End Sub

    Private Sub SelecionaPadrao_Click(sender As Object, e As EventArgs) Handles SelecionaPadrao.Click
        SelecionaPadrao.SelectedIndex = NumTemas
    End Sub

    Private Sub PadraoCor(ByVal i As Integer)
        Dim l As New List(Of Color)
        Select Case i
            Case 0
                l.Add(Color.FromArgb(149, 154, 143))
                l.Add(Color.Beige)
                l.Add(Color.MediumBlue)
                l.Add(Color.Red)
                l.Add(Color.DimGray)
                l.Add(Color.Gray)
                l.Add(Color.Black)
            Case 1
                l.Add(Color.FromArgb(10, 5, 9))
                l.Add(Color.FromArgb(94, 141, 151))
                l.Add(Color.FromArgb(255, 83, 108))
                l.Add(Color.FromArgb(255, 83, 108))
                l.Add(Color.FromArgb(46, 31, 86))
                l.Add(Color.FromArgb(46, 31, 86))
                l.Add(Color.FromArgb(46, 31, 86))
            Case 2
                l.Add(Color.FromArgb(128, 128, 128))
                l.Add(Color.FromArgb(64, 0, 128))
                l.Add(Color.FromArgb(221, 217, 255))
                l.Add(Color.FromArgb(0, 255, 255))
                l.Add(Color.FromArgb(85, 82, 122))
                l.Add(Color.FromArgb(43, 45, 66))
                l.Add(Color.FromArgb(0, 0, 0))
            Case 3
                l.Add(Color.FromArgb(22, 22, 29))
                l.Add(Color.FromArgb(240, 213, 162))
                l.Add(Color.FromArgb(82, 163, 111))
                l.Add(Color.FromArgb(191, 84, 53))
                l.Add(Color.FromArgb(103, 81, 67))
                l.Add(Color.FromArgb(85, 82, 72))
                l.Add(Color.FromArgb(14, 10, 10))
            Case 4
                l.Add(Color.FromArgb(18, 18, 16))
                l.Add(Color.FromArgb(196, 0, 39))
                l.Add(Color.FromArgb(210, 177, 79))
                l.Add(Color.FromArgb(255, 255, 226))
                l.Add(Color.FromArgb(94, 49, 34))
                l.Add(Color.FromArgb(97, 48, 69))
                l.Add(Color.FromArgb(97, 48, 69))
        End Select
        For i = 0 To Cores.Count - 1
            Me.Controls("Cor" & CStr(i + 1)).BackColor = l.Item(i)
        Next
    End Sub

End Class