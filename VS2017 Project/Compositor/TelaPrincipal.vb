'--- TelaPrincipal ---
'Autores: Rafael Gomes da Silva, Guilherme Pereira Porto Londe
'última modificação: 09 de março de 2018

Imports System.Drawing
Imports System.Windows.Forms
Imports Compositor.My.Resources.Resources

Public Class TelaPrincipal

    Private SP As SubPlayer
    Private Pausado As Boolean = True
    Public TemAlteracao As Boolean = False
    Private Inicializado As Boolean = False
    Private LarguraTelaAnterior As Integer
    Private DiretorioArquivo As String
    Private CoordYTabela As Integer
    Public IdIdioma As Integer
    Private ObjetoIdioma As New Idioma
    Private VelocidadeAtual As Integer
    Friend WithEvents MenuVelocidade As New ContextMenuStrip
    Private LocalMenuIdioma As Point

    Public Sub New()
        ' Esta chamada é requerida pelo designer.
        InitializeComponent()
        Inicializado = True
        LarguraTelaAnterior = Me.Width
        SP = New SubPlayer(TelaPai, Barra, Me)
        CoordYTabela = 62
        IdIdioma = 1
        DiretorioArquivo = New String("")
        CarregaIdIdioma()
        AtualizaIdioma()
        Me.Width -= 1
        VelocidadeAtual = 100
        TabIndexVolume.Value = 100
        ControleVolume.Hide()
        ' Adicione qualquer inicialização após a chamada InitializeComponent().
    End Sub

    Public Sub CarregaIdIdioma()
        If System.IO.File.Exists("userdata.inf") Then
            Dim text = System.IO.File.ReadAllText("userdata.inf")
            If text.IndexOf("IdiomId = ") = 0 And text.Count > 10 Then
                IdIdioma = Convert.ToInt32(text.Chars(10)) - 48
                If IdIdioma < 0 Or IdIdioma > 1 Then
                    GoTo CasoFalso
                End If
            Else
                GoTo CasoFalso
            End If
        Else
CasoFalso:
            System.IO.File.WriteAllText("userdata.inf", "IdiomId = 1")
            IdIdioma = 1
        End If
    End Sub

    Public Sub AtivaTecla(ByVal NumeroTecla As Integer)
        Dim MyBrush As Brush
        Dim MyGraphics As Graphics
        Dim MyPen As Pen
        Dim Tecla As Control
        Dim X, Y As Integer
        Tecla = Teclas.Controls("Tecla_" & CStr(NumeroTecla))
        MyGraphics = Tecla.CreateGraphics()
        MyGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        MyPen = New Pen(Color.Red, 1)
        MyBrush = New SolidBrush(Color.Red)
        X = (Tecla.Width - 10) / 2
        Y = Tecla.Height - 25
        MyGraphics.DrawEllipse(MyPen, X, Y, 10, 10)
        MyGraphics.FillEllipse(MyBrush, X, Y, 10, 10)
        MyBrush.Dispose()
        MyPen.Dispose()
        MyGraphics.Dispose()
    End Sub

    Public Sub DesativaTecla(ByVal NumeroTecla As Integer)
        Teclas.Controls("Tecla_" & CStr(NumeroTecla)).Refresh()
    End Sub

    Private Function Ajustar(ByRef Tecla As System.Windows.Forms.Button, ByRef x As Integer, ByVal w As Double) As Double
        Tecla.Left = x
        Tecla.Width = Math.Floor(w)
        x += (Tecla.Width + 1)
        Return w - Tecla.Width
    End Function

    Private Sub TelaPrincipal_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If Inicializado Then
            MenuPrincipal.Width = Me.Width
            LarguraTelaAnterior = Me.Width
            Divisor1.Width = Me.Width
            Dim w As Double = ((Me.Width - 40) / 36) - 2
            Teclas.Width = Me.Width - 40
            Teclas.Top = Me.Height - 190
            Controles.Top = Me.Height - 225
            Controles.Left = (Me.Width / 2) - (Controles.Width / 2)
            TelaPai.Top = CoordYTabela
            TelaPai.Width = Me.Width - 40
            TelaPai.Height = (62 - CoordYTabela) + Me.Height - 306
            Barra.Top = Me.Height - 243
            Barra.Width = Me.Width - 40
            Dim l As Integer = 20
            For I As Integer = 1 To 5
                Dim p As Integer = 1 + (I - 1) * 12
                Dim l2 As Integer
                Dim rm As Double = 0
                rm = Ajustar(Teclas.Controls("Tecla_" & CStr(p)), l, w)
                l2 = l - w / 3
                Ajustar(Teclas.Controls("Tecla_" & CStr(p + 1)), l2, w / 1.5)
                rm = Ajustar(Teclas.Controls("Tecla_" & CStr(p + 2)), l, w + rm)
                l2 = l - w / 3
                Ajustar(Teclas.Controls("Tecla_" & CStr(p + 3)), l2, w / 1.5)
                rm = Ajustar(Teclas.Controls("Tecla_" & CStr(p + 4)), l, w + rm)
                rm = Ajustar(Teclas.Controls("Tecla_" & CStr(p + 5)), l, w + rm)
                l2 = l - w / 3
                Ajustar(Teclas.Controls("Tecla_" & CStr(p + 6)), l2, w / 1.5)
                rm = Ajustar(Teclas.Controls("Tecla_" & CStr(p + 7)), l, w + rm)
                l2 = l - w / 3
                Ajustar(Teclas.Controls("Tecla_" & CStr(p + 8)), l2, w / 1.5)
                rm = Ajustar(Teclas.Controls("Tecla_" & CStr(p + 9)), l, w + rm)
                l2 = l - w / 3
                Ajustar(Teclas.Controls("Tecla_" & CStr(p + 10)), l2, w / 1.5)
                rm = Ajustar(Teclas.Controls("Tecla_" & CStr(p + 11)), l, w + rm)
                l += 1
            Next
            Ajustar(Teclas.Controls("Tecla_61"), l, w)
            SP.AtualizaTamanhoTabela()
        End If
    End Sub

    Private Sub Me_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If SolicitacaoFechar() = False Then
            e.Cancel = True
        End If
    End Sub

    Private Sub F1pressed()
        For I As Integer = 0 To 4
            Teclas.Controls("Tecla_" & CStr(I * 12 + 1)).Text = "C"
            Teclas.Controls("Tecla_" & CStr(I * 12 + 2)).Text = "#"
            Teclas.Controls("Tecla_" & CStr(I * 12 + 2)).ForeColor = System.Drawing.Color.DarkGray
            Teclas.Controls("Tecla_" & CStr(I * 12 + 3)).Text = "D"
            Teclas.Controls("Tecla_" & CStr(I * 12 + 4)).Text = "#"
            Teclas.Controls("Tecla_" & CStr(I * 12 + 4)).ForeColor = System.Drawing.Color.DarkGray
            Teclas.Controls("Tecla_" & CStr(I * 12 + 5)).Text = "E"
            Teclas.Controls("Tecla_" & CStr(I * 12 + 6)).Text = "F"
            Teclas.Controls("Tecla_" & CStr(I * 12 + 7)).Text = "#"
            Teclas.Controls("Tecla_" & CStr(I * 12 + 7)).ForeColor = System.Drawing.Color.DarkGray
            Teclas.Controls("Tecla_" & CStr(I * 12 + 8)).Text = "G"
            Teclas.Controls("Tecla_" & CStr(I * 12 + 9)).Text = "#"
            Teclas.Controls("Tecla_" & CStr(I * 12 + 9)).ForeColor = System.Drawing.Color.DarkGray
            Teclas.Controls("Tecla_" & CStr(I * 12 + 10)).Text = "A"
            Teclas.Controls("Tecla_" & CStr(I * 12 + 11)).Text = "#"
            Teclas.Controls("Tecla_" & CStr(I * 12 + 11)).ForeColor = System.Drawing.Color.DarkGray
            Teclas.Controls("Tecla_" & CStr(I * 12 + 12)).Text = "B"
        Next
        Teclas.Controls("Tecla_61").Text = "C"
    End Sub

    Private Sub F2pressed()
        For I As Integer = 1 To 61
            Teclas.Controls("Tecla_" & CStr(I)).Text = ""
        Next
    End Sub

    Private Sub AtualizaNomeJanela()
        If DiretorioArquivo.Count = 0 Then
            Me.Text = ObjetoIdioma.Tabela(28, IdIdioma)
            Return
        End If
        Dim nome As String
        For i = DiretorioArquivo.Count - 1 To 0 Step -1
            If DiretorioArquivo.Chars(i) = "/" Or DiretorioArquivo.Chars(i) = "\" Then
                nome = DiretorioArquivo.Substring(i + 1)
                GoTo fimlaço
            End If
        Next
        nome = DiretorioArquivo
fimlaço:
        If nome.LastIndexOf(".") <> -1 Then
            nome = nome.Substring(0, nome.LastIndexOf("."))
        End If
        Me.Text = nome.Insert(nome.Count, " - " & ObjetoIdioma.Tabela(28, IdIdioma))
    End Sub

    Private Sub Abrir_Click(sender As Object, e As EventArgs) Handles Abrir.Click
        F_Abrir(sender, e)
    End Sub

    Private Sub F_Abrir(sender As Object, e As EventArgs)
        SP.LimpaSelecao()

        If SolicitacaoFechar() = False Then
            Return
        End If

        Dim FD As New OpenFileDialog()
        FD.Filter = ObjetoIdioma.Tabela(30, IdIdioma) & " | *.mld"
        FD.Title = ObjetoIdioma.Tabela(29, IdIdioma)

        If FD.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            DiretorioArquivo = String.Copy(FD.FileName)
            SP.CarregarArquivo(DiretorioArquivo)
            If Not Pausado Then
                Tocar.BackgroundImage = playIcon
                Pausado = True
            End If
            AtualizaNomeJanela()
        End If
    End Sub

    Private Sub Salvar_Click(sender As Object, e As EventArgs) Handles Salvar.Click
        F_Salvar(sender, e)
    End Sub

    Private Sub F_Salvar(sender As Object, e As EventArgs)
        If DiretorioArquivo = "" Then
            F_SalvarComo(sender, e)
            Return
        End If
        SP.LimpaSelecao()
        SP.SalvarArquivo(DiretorioArquivo)
        TemAlteracao = False
    End Sub

    Private Sub SalvarComo_Click(sender As Object, e As EventArgs) Handles SalvarComo.Click
        F_SalvarComo(sender, e)
    End Sub

    Private Sub F_SalvarComo(sender As Object, e As EventArgs)
        SP.LimpaSelecao()

        Dim FD As New SaveFileDialog()
        FD.Filter = ObjetoIdioma.Tabela(30, IdIdioma) & " | *.mld"
        FD.Title = ObjetoIdioma.Tabela(31, IdIdioma)

        FD.ShowDialog()
        If FD.FileName <> "" Then
            DiretorioArquivo = String.Copy(FD.FileName)
            SP.SalvarArquivo(FD.FileName)
            TemAlteracao = False
            AtualizaNomeJanela()
        End If
    End Sub

    Private Sub Novo_Click(sender As Object, e As EventArgs) Handles Novo.Click
        F_Novo(sender, e)
    End Sub

    Private Sub F_Novo(sender As Object, e As EventArgs)
        SP.LimpaSelecao()

        If SolicitacaoFechar() = False Then
            Return
        End If
        DiretorioArquivo = ""
        Me.Text = ObjetoIdioma.Tabela(28, IdIdioma)
        SP.NovoArquivo()
    End Sub

    Private Function SolicitacaoFechar() As Boolean
        If TemAlteracao = False Then
            Return True
        End If
        Dim n As String = MsgBox(ObjetoIdioma.Tabela(32, IdIdioma), MsgBoxStyle.YesNoCancel, ObjetoIdioma.Tabela(28, IdIdioma))
        If n = vbYes Then
            If DiretorioArquivo = "" Then
                Dim FD As New SaveFileDialog()
                FD.Filter = ObjetoIdioma.Tabela(30, IdIdioma) & " | *.mld"
                FD.Title = ObjetoIdioma.Tabela(31, IdIdioma)
                FD.ShowDialog()
                If FD.FileName <> "" Then
                    SP.SalvarArquivo(FD.FileName)
                    TemAlteracao = False
                    Return True
                End If
                Return False
            End If
            SP.LimpaSelecao()
            SP.SalvarArquivo(DiretorioArquivo)
            TemAlteracao = False
            Return True
        ElseIf n = vbNo Then
            TemAlteracao = False
            Return True
        End If
        Return False
    End Function

    Private Sub Desfazer_Click(sender As Object, e As EventArgs) Handles Desfazer.Click
        F_Desfazer(sender, e)
    End Sub

    Private Sub F_Desfazer(sender As Object, e As EventArgs)
        SP.Desfazer()
        Me.TemAlteracao = True
    End Sub

    Private Sub Refazer_Click(sender As Object, e As EventArgs) Handles Refazer.Click
        F_Refazer(sender, e)
    End Sub

    Private Sub F_Refazer(sender As Object, e As EventArgs)
        SP.Refazer()
        Me.TemAlteracao = True
    End Sub

    Private Sub ModoEdição_CheckedChanged(sender As Object, e As EventArgs) Handles ModoEdição.CheckedChanged
        SP.SetModoEdicao(ModoEdição.Checked)
    End Sub

    Private Sub F_ModoEdicao(sender As Object, e As EventArgs)
        ModoEdição.Checked = Not ModoEdição.Checked
        SP.SetModoEdicao(ModoEdição.Checked)
    End Sub

    Private Sub ModoCascata_CheckedChanged(sender As Object, e As EventArgs) Handles ModoCascata.CheckedChanged
        SP.SetModoCascata(ModoCascata.Checked)
    End Sub

    Private Sub F_ModoCascata(sender As Object, e As EventArgs)
        ModoCascata.Checked = Not ModoCascata.Checked
        SP.SetModoCascata(ModoCascata.Checked)
    End Sub

    Private Sub Anterior_Click(sender As Object, e As EventArgs) Handles Anterior.Click
        SP.Anterior()
        If Pausado = False Then
            Tocar.BackgroundImage = playIcon
            Pausado = True
        End If
    End Sub

    Private Sub Tocar_Click(sender As Object, e As EventArgs) Handles Tocar.Click
        If Pausado Then
            SP.Tocar()
            Tocar.BackgroundImage = pauseIcon
        Else
            SP.Pausar()
            Tocar.BackgroundImage = playIcon
        End If
        Pausado = Not Pausado
    End Sub

    Private Sub Parar_Click(sender As Object, e As EventArgs) Handles Parar.Click
        SP.Parar()
        If Not Pausado Then
            Tocar.BackgroundImage = playIcon
            Pausado = True
        End If
    End Sub

    Private Sub Próximo_Click(sender As Object, e As EventArgs) Handles Próximo.Click
        SP.Proximo()
        If Pausado = False Then
            Tocar.BackgroundImage = playIcon
            Pausado = True
        End If
    End Sub

    Private Sub Tecla_Click(sender As Object, e As EventArgs) Handles Tecla_1.Click, Tecla_2.Click, Tecla_3.Click, Tecla_4.Click, Tecla_5.Click, Tecla_6.Click, Tecla_7.Click, Tecla_8.Click, Tecla_9.Click, Tecla_10.Click, Tecla_11.Click, Tecla_12.Click, Tecla_13.Click, Tecla_14.Click, Tecla_15.Click, Tecla_16.Click, Tecla_17.Click, Tecla_18.Click, Tecla_19.Click, Tecla_20.Click, Tecla_21.Click, Tecla_22.Click, Tecla_23.Click, Tecla_24.Click, Tecla_25.Click, Tecla_26.Click, Tecla_27.Click, Tecla_28.Click, Tecla_29.Click, Tecla_30.Click, Tecla_31.Click, Tecla_32.Click, Tecla_33.Click, Tecla_34.Click, Tecla_35.Click, Tecla_36.Click, Tecla_37.Click, Tecla_38.Click, Tecla_39.Click, Tecla_40.Click, Tecla_41.Click, Tecla_42.Click, Tecla_43.Click, Tecla_44.Click, Tecla_45.Click, Tecla_46.Click, Tecla_47.Click, Tecla_48.Click, Tecla_49.Click, Tecla_50.Click, Tecla_51.Click, Tecla_52.Click, Tecla_53.Click, Tecla_54.Click, Tecla_55.Click, Tecla_56.Click, Tecla_57.Click, Tecla_58.Click, Tecla_59.Click, Tecla_60.Click, Tecla_61.Click
        Dim Escala, NumeroTecla, Oitava As Integer
        NumeroTecla = Integer.Parse(sender.Name.split("_")(1))
        If NumeroTecla = 61 Then
            Oitava = 5
            Escala = 13
        Else
            Oitava = Math.Ceiling(NumeroTecla / 12)
            Escala = NumeroTecla Mod 12
            If Escala = 0 Then
                Escala = 12
            End If
        End If
        AtivaTecla(NumeroTecla)
        SP.Inserir(Oitava, Escala)
        If ModoEdição.Checked = True Then
            TemAlteracao = True
        End If
    End Sub

    Private Sub TelaPrincipal_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        Select Case e.KeyCode
            Case Keys.D1
                Tecla_2.Refresh()
                Tecla_1.Refresh()
            Case Keys.D2
                Tecla_4.Refresh()
                Tecla_3.Refresh()
            Case Keys.D3
                Tecla_5.Refresh()
            Case Keys.D4
                Tecla_7.Refresh()
                Tecla_6.Refresh()
            Case Keys.D5
                Tecla_9.Refresh()
                Tecla_8.Refresh()
            Case Keys.D6
                Tecla_11.Refresh()
                Tecla_10.Refresh()
            Case Keys.D7
                Tecla_12.Refresh()
            Case Keys.D8
                Tecla_14.Refresh()
                Tecla_13.Refresh()
            Case Keys.D9
                Tecla_16.Refresh()
                Tecla_15.Refresh()
            Case Keys.D0
                Tecla_17.Refresh()
            Case Keys.Q
                Tecla_19.Refresh()
                Tecla_18.Refresh()
            Case Keys.W
                Tecla_21.Refresh()
                Tecla_20.Refresh()
            Case Keys.E
                Tecla_23.Refresh()
                Tecla_22.Refresh()
            Case Keys.R
                Tecla_24.Refresh()
            Case Keys.T
                Tecla_26.Refresh()
                Tecla_25.Refresh()
            Case Keys.Y
                Tecla_28.Refresh()
                Tecla_27.Refresh()
            Case Keys.U
                Tecla_29.Refresh()
            Case Keys.I
                Tecla_31.Refresh()
                Tecla_30.Refresh()
            Case Keys.O
                Tecla_33.Refresh()
                Tecla_32.Refresh()
            Case Keys.P
                Tecla_35.Refresh()
                Tecla_34.Refresh()
            Case Keys.A
                Tecla_36.Refresh()
            Case Keys.S
                Tecla_38.Refresh()
                Tecla_37.Refresh()
            Case Keys.D
                Tecla_40.Refresh()
                Tecla_39.Refresh()
            Case Keys.F
                Tecla_41.Refresh()
            Case Keys.G
                Tecla_43.Refresh()
                Tecla_42.Refresh()
            Case Keys.H
                Tecla_45.Refresh()
                Tecla_44.Refresh()
            Case Keys.J
                Tecla_47.Refresh()
                Tecla_46.Refresh()
            Case Keys.K
                Tecla_48.Refresh()
            Case Keys.L
                Tecla_50.Refresh()
                Tecla_49.Refresh()
            Case Keys.Z
                Tecla_52.Refresh()
                Tecla_51.Refresh()
            Case Keys.X
                Tecla_53.Refresh()
            Case Keys.C
                Tecla_55.Refresh()
                Tecla_54.Refresh()
            Case Keys.V
                Tecla_57.Refresh()
                Tecla_56.Refresh()
            Case Keys.B
                Tecla_59.Refresh()
                Tecla_58.Refresh()
            Case Keys.N
                Tecla_60.Refresh()
            Case Keys.M
                Tecla_61.Refresh()

        End Select
    End Sub

    Private Sub TelaPrincipal_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Delete
                F_Excluir(sender, e)
            Case Keys.Z
                If e.Control Then
                    F_Desfazer(sender, e)
                ElseIf e.Shift Then
                    Tecla_52.PerformClick()
                Else
                    Tecla_51.PerformClick()
                End If
            Case Keys.Y
                If e.Control Then
                    F_Refazer(sender, e)
                ElseIf e.Shift Then
                    Tecla_28.PerformClick()
                Else
                    Tecla_27.PerformClick()
                End If
            Case Keys.I
                If e.Control Then
                    F_ModoEdicao(sender, e)
                ElseIf e.Shift Then
                    Tecla_31.PerformClick()
                Else
                    Tecla_30.PerformClick()
                End If
            Case Keys.N
                If e.Control Then
                    F_Novo(sender, e)
                Else
                    Tecla_60.PerformClick()
                End If
            Case Keys.S
                If e.Control Then
                    F_Salvar(sender, e)
                ElseIf e.Shift Then
                    Tecla_38.PerformClick()
                Else
                    Tecla_37.PerformClick()
                End If
            Case Keys.O
                If e.Control Then
                    F_Abrir(sender, e)
                ElseIf e.Shift Then
                    Tecla_33.PerformClick()
                Else
                    Tecla_32.PerformClick()
                End If
            Case Keys.D
                If e.Shift Then
                    Tecla_40.PerformClick()
                ElseIf e.Control Then
                    F_ModoCascata(sender, e)
                Else
                    Tecla_39.PerformClick()
                End If
            Case Keys.Space
                Tocar.PerformClick()
            Case Keys.Back
                Parar.PerformClick()
            Case Keys.F1
                F1pressed()
            Case Keys.F2
                F2pressed()
            Case Keys.C
                If e.Control Then
                    F_Copiar(sender, e)
                ElseIf e.Shift Then
                    Tecla_55.PerformClick()
                Else
                    Tecla_54.PerformClick()
                End If
            Case Keys.X
                If e.Control Then
                    F_Cortar(sender, e)
                Else
                    Tecla_53.PerformClick()
                End If
            Case Keys.V
                If e.Control Then
                    F_Colar(sender, e)
                ElseIf e.Shift Then
                    Tecla_57.PerformClick()
                Else
                    Tecla_56.PerformClick()
                End If
            Case Keys.A
                If e.Control Then
                    SP.SelecionaTudo()
                Else
                    Tecla_36.PerformClick()
                End If


            'Teclas do piano
            Case Keys.D1
                If e.Shift Then
                    Tecla_2.PerformClick()
                Else
                    Tecla_1.PerformClick()
                End If
            Case Keys.D2
                If e.Shift Then
                    Tecla_4.PerformClick()
                Else
                    Tecla_3.PerformClick()
                End If
            Case Keys.D3
                Tecla_5.PerformClick()
            Case Keys.D4
                If e.Shift Then
                    Tecla_7.PerformClick()
                Else
                    Tecla_6.PerformClick()
                End If
            Case Keys.D5
                If e.Shift Then
                    Tecla_9.PerformClick()
                Else
                    Tecla_8.PerformClick()
                End If
            Case Keys.D6
                If e.Shift Then
                    Tecla_11.PerformClick()
                Else
                    Tecla_10.PerformClick()
                End If
            Case Keys.D7
                Tecla_12.PerformClick()
            Case Keys.D8
                If e.Shift Then
                    Tecla_14.PerformClick()
                Else
                    Tecla_13.PerformClick()
                End If
            Case Keys.D9
                If e.Shift Then
                    Tecla_16.PerformClick()
                Else
                    Tecla_15.PerformClick()
                End If
            Case Keys.D0
                Tecla_17.PerformClick()
            Case Keys.Q
                If e.Shift Then
                    Tecla_19.PerformClick()
                Else
                    Tecla_18.PerformClick()
                End If
            Case Keys.W
                If e.Shift Then
                    Tecla_21.PerformClick()
                Else
                    Tecla_20.PerformClick()
                End If
            Case Keys.R
                Tecla_24.PerformClick()
            Case Keys.T
                If e.Shift Then
                    Tecla_26.PerformClick()
                Else
                    Tecla_25.PerformClick()
                End If
            Case Keys.U
                Tecla_29.PerformClick()
            Case Keys.P
                If e.Shift Then
                    Tecla_35.PerformClick()
                Else
                    Tecla_34.PerformClick()
                End If
            Case Keys.E
                If e.Shift Then
                    Tecla_23.PerformClick()
                Else
                    Tecla_22.PerformClick()
                End If
            Case Keys.F
                Tecla_41.PerformClick()
            Case Keys.G
                If e.Shift Then
                    Tecla_43.PerformClick()
                Else
                    Tecla_42.PerformClick()
                End If
            Case Keys.H
                If e.Shift Then
                    Tecla_45.PerformClick()
                Else
                    Tecla_44.PerformClick()
                End If
            Case Keys.J
                If e.Shift Then
                    Tecla_47.PerformClick()
                Else
                    Tecla_46.PerformClick()
                End If
            Case Keys.K
                Tecla_48.PerformClick()
            Case Keys.L
                If e.Shift Then
                    Tecla_50.PerformClick()
                Else
                    Tecla_49.PerformClick()
                End If
            Case Keys.B
                If e.Shift Then
                    Tecla_59.PerformClick()
                Else
                    Tecla_58.PerformClick()
                End If
            Case Keys.M
                Tecla_61.PerformClick()

            Case Else
        End Select
        e.Handled = True
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        Select Case keyData
            Case Keys.Left
                Anterior.PerformClick()
                Return True
            Case Keys.Right
                Próximo.PerformClick()
                Return True
            Case Else
                Return MyBase.ProcessCmdKey(msg, keyData)
        End Select
    End Function

    Private Sub Copiar_Click(sender As Object, e As EventArgs) Handles Copiar.Click
        F_Copiar(sender, e)
    End Sub

    Private Sub F_Copiar(sender As Object, e As EventArgs)
        SP.Copiar()
    End Sub

    Private Sub Excluir_Click(sender As Object, e As EventArgs) Handles Excluir.Click
        F_Excluir(sender, e)
    End Sub

    Private Sub F_Excluir(sender As Object, e As EventArgs)
        SP.Excluir()
    End Sub

    Private Sub Colar_Click(sender As Object, e As EventArgs) Handles Colar.Click
        F_Colar(sender, e)
    End Sub

    Private Sub F_Colar(sender As Object, e As EventArgs)
        SP.Colar(SP.ObjetoPlayer.Cursor)
    End Sub

    Private Sub Cortar_Click(sender As Object, e As EventArgs) Handles Cortar.Click
        F_Cortar(sender, e)
    End Sub

    Private Sub F_Cortar(sender As Object, e As EventArgs)
        SP.Copiar()
        SP.Excluir()
    End Sub

    Private Sub SelecionaTudo(sender As Object, e As EventArgs)
        SP.SelecionaTudo()
    End Sub

    Private Sub GradeVisivel(sender As Object, e As EventArgs)
        SP.SetGradeVisivel()
    End Sub

    Private Sub BarraMenuVisivel(sender As Object, e As EventArgs)
        Me.MenuPrincipal.Visible = Not Me.MenuPrincipal.Visible
        If Me.MenuPrincipal.Visible = True Then
            CoordYTabela = 62
        Else
            CoordYTabela = 32
        End If
        TelaPai.Top = CoordYTabela
        TelaPai.Height = (62 - CoordYTabela) + Me.Height - 306
        SP.AtualizaTamanhoTabela()
    End Sub

    Private Sub SetAtivacaoNota(sender As Object, e As EventArgs)
        SP.SetAtivacaoNota()
    End Sub

    Private Sub SetIdioma(sender As Object, e As EventArgs)
        Dim item = CType(sender, ToolStripMenuItem)
        IdIdioma = CInt(item.Tag)
        AtualizaIdioma()
    End Sub

    Private Sub AtualizaIdioma()
        System.IO.File.WriteAllText("userdata.inf", "IdiomId = " & IdIdioma)
        Arquivo.Text = ObjetoIdioma.Tabela(17, IdIdioma)
        Editar.Text = ObjetoIdioma.Tabela(18, IdIdioma)
        Exibir.Text = ObjetoIdioma.Tabela(19, IdIdioma)
        Configuracoes.Text = ObjetoIdioma.Tabela(20, IdIdioma)
        ToolTip.SetToolTip(SalvarComo, ObjetoIdioma.Tabela(3, IdIdioma))
        ToolTip.SetToolTip(Excluir, ObjetoIdioma.Tabela(9, IdIdioma))
        ToolTip.SetToolTip(Salvar, ObjetoIdioma.Tabela(2, IdIdioma))
        ToolTip.SetToolTip(Novo, ObjetoIdioma.Tabela(0, IdIdioma))
        ToolTip.SetToolTip(Abrir, ObjetoIdioma.Tabela(1, IdIdioma))
        ToolTip.SetToolTip(Desfazer, ObjetoIdioma.Tabela(4, IdIdioma))
        ToolTip.SetToolTip(Refazer, ObjetoIdioma.Tabela(5, IdIdioma))
        ToolTip.SetToolTip(Cortar, ObjetoIdioma.Tabela(6, IdIdioma))
        ToolTip.SetToolTip(Copiar, ObjetoIdioma.Tabela(7, IdIdioma))
        ToolTip.SetToolTip(Colar, ObjetoIdioma.Tabela(8, IdIdioma))
        ToolTip.SetToolTip(Excluir, ObjetoIdioma.Tabela(9, IdIdioma))
        ToolTip.SetToolTip(ModoEdição, ObjetoIdioma.Tabela(14, IdIdioma))
        ToolTip.SetToolTip(ModoCascata, ObjetoIdioma.Tabela(15, IdIdioma))
        ToolTip.SetToolTip(Anterior, ObjetoIdioma.Tabela(21, IdIdioma))
        ToolTip.SetToolTip(Tocar, ObjetoIdioma.Tabela(22, IdIdioma))
        ToolTip.SetToolTip(Próximo, ObjetoIdioma.Tabela(24, IdIdioma))
        ToolTip.SetToolTip(Parar, ObjetoIdioma.Tabela(23, IdIdioma))
        ToolTip.SetToolTip(Velocidade, ObjetoIdioma.Tabela(25, IdIdioma))
        ToolTip.SetToolTip(Volume, ObjetoIdioma.Tabela(27, IdIdioma))
        AtualizaNomeJanela()
    End Sub

    Private Sub Arquivo_Click(sender As Object, e As EventArgs) Handles Arquivo.Click
        Dim cms = New ContextMenuStrip
        cms.BackColor = System.Drawing.Color.DarkGray
        cms.ShowImageMargin = False
        Dim itemNovo = cms.Items.Add("   " & ObjetoIdioma.Tabela(0, IdIdioma))
        AddHandler itemNovo.Click, AddressOf F_Novo
        Dim itemAbrir = cms.Items.Add("   " & ObjetoIdioma.Tabela(1, IdIdioma))
        AddHandler itemAbrir.Click, AddressOf F_Abrir
        cms.Items.Add(New ToolStripSeparator())
        Dim itemSalvar = cms.Items.Add("   " & ObjetoIdioma.Tabela(2, IdIdioma))
        AddHandler itemSalvar.Click, AddressOf F_Salvar
        Dim itemSalvarComo = cms.Items.Add("   " & ObjetoIdioma.Tabela(3, IdIdioma))
        AddHandler itemSalvarComo.Click, AddressOf F_SalvarComo
        cms.Show(Me, New Point(Arquivo.Left, Arquivo.Top + Arquivo.Height))
    End Sub

    Private Sub Editar_Click(sender As Object, e As EventArgs) Handles Editar.Click
        Dim cms = New ContextMenuStrip
        cms.BackColor = System.Drawing.Color.DarkGray
        cms.ShowImageMargin = False
        Dim itemDesfazer = cms.Items.Add("   " & ObjetoIdioma.Tabela(4, IdIdioma))
        AddHandler itemDesfazer.Click, AddressOf F_Desfazer
        Dim itemRefazer = cms.Items.Add("   " & ObjetoIdioma.Tabela(5, IdIdioma))
        AddHandler itemRefazer.Click, AddressOf F_Refazer
        cms.Items.Add(New ToolStripSeparator())
        Dim itemRecortar = cms.Items.Add("   " & ObjetoIdioma.Tabela(6, IdIdioma))
        AddHandler itemRecortar.Click, AddressOf F_Cortar
        Dim itemCopiar = cms.Items.Add("   " & ObjetoIdioma.Tabela(7, IdIdioma))
        AddHandler itemCopiar.Click, AddressOf F_Copiar
        Dim itemColar = cms.Items.Add("   " & ObjetoIdioma.Tabela(8, IdIdioma))
        AddHandler itemColar.Click, AddressOf F_Colar
        Dim itemExcluir = cms.Items.Add("   " & ObjetoIdioma.Tabela(9, IdIdioma))
        AddHandler itemExcluir.Click, AddressOf F_Excluir
        cms.Items.Add(New ToolStripSeparator())
        Dim itemSelecionaTudo = cms.Items.Add("   " & ObjetoIdioma.Tabela(10, IdIdioma))
        AddHandler itemSelecionaTudo.Click, AddressOf SelecionaTudo
        cms.Show(Me, New Point(Editar.Left, Editar.Top + Editar.Height))
    End Sub

    Private Sub Exibir_Click(sender As Object, e As EventArgs) Handles Exibir.Click
        Dim cms = New ContextMenuStrip
        cms.BackColor = System.Drawing.Color.DarkGray
        cms.ShowImageMargin = False
        Dim itemBM = cms.Items.Add("   " & ObjetoIdioma.Tabela(11, IdIdioma))
        If MenuPrincipal.Visible Then
            itemBM.BackColor = System.Drawing.Color.Gray
        End If
        AddHandler itemBM.Click, AddressOf BarraMenuVisivel
        Dim itemGrade = cms.Items.Add("   " & ObjetoIdioma.Tabela(12, IdIdioma))
        If SP.GradeVisivel = True Then
            itemGrade.BackColor = System.Drawing.Color.Gray
        End If
        AddHandler itemGrade.Click, AddressOf GradeVisivel
        Dim itemAtivacao = cms.Items.Add("   " & ObjetoIdioma.Tabela(13, IdIdioma))
        If SP.AtivacaoNota = True Then
            itemAtivacao.BackColor = System.Drawing.Color.Gray
        End If
        AddHandler itemAtivacao.Click, AddressOf SetAtivacaoNota
        cms.Show(Me, New Point(Exibir.Left, Exibir.Top + Exibir.Height))
    End Sub

    Private Sub Configuracoes_Click(sender As Object, e As EventArgs) Handles Configuracoes.Click
        Dim cms = New ContextMenuStrip
        cms.BackColor = System.Drawing.Color.DarkGray
        cms.Stretch = True
        cms.ShowImageMargin = False
        Dim itemEdicao = cms.Items.Add("   " & ObjetoIdioma.Tabela(14, IdIdioma))
        If ModoEdição.Checked Then
            itemEdicao.BackColor = System.Drawing.Color.Gray
        End If
        AddHandler itemEdicao.Click, AddressOf F_ModoEdicao
        Dim itemCascata = cms.Items.Add("   " & ObjetoIdioma.Tabela(15, IdIdioma))
        If ModoCascata.Checked Then
            itemCascata.BackColor = System.Drawing.Color.Gray
        End If
        AddHandler itemCascata.Click, AddressOf F_ModoCascata
        cms.Items.Add(New ToolStripSeparator())
        Dim itemIdioma = cms.Items.Add("   " & ObjetoIdioma.Tabela(16, IdIdioma))
        AddHandler itemIdioma.Click, AddressOf MenuIdioma
        cms.Show(Me, New Point(Configuracoes.Left, Configuracoes.Top + Configuracoes.Height))
        LocalMenuIdioma = New Point(Configuracoes.Left + cms.Width, Configuracoes.Top + cms.Height)
    End Sub

    Private Sub MenuIdioma(sender As Object, e As EventArgs)
        Dim cms2 As New ContextMenuStrip
        Dim itemIdioma0 = cms2.Items.Add(ObjetoIdioma.Tabela(26, 0))
        AddHandler itemIdioma0.Click, AddressOf SetIdioma
        itemIdioma0.Tag = 0
        Dim itemIdioma1 = cms2.Items.Add(ObjetoIdioma.Tabela(26, 1))
        AddHandler itemIdioma1.Click, AddressOf SetIdioma
        itemIdioma1.Tag = 1
        cms2.Show(Me, LocalMenuIdioma)
    End Sub

    Private Sub SetVelocidadePlayer(sender As Object, e As EventArgs)
        Dim item = CType(sender, ToolStripMenuItem)
        VelocidadeAtual = CInt(item.Tag)
        SP.SetVelocidade(VelocidadeAtual / 100)
    End Sub

    Private Sub Velocidade_Click(sender As Object, e As EventArgs) Handles Velocidade.Click
        MenuVelocidade = New ContextMenuStrip
        MenuVelocidade.BackColor = System.Drawing.Color.DarkGray
        MenuVelocidade.ShowImageMargin = False
        Dim L As New List(Of Integer)(New Integer() {200, 175, 150, 135, 125, 115, 100, 85, 75, 50})
        For Each item As Integer In L
            Dim itemMenu = MenuVelocidade.Items.Add((item / 100) & " x")
            itemMenu.Tag = item
            If item = VelocidadeAtual Then
                itemMenu.BackColor = System.Drawing.Color.Gray
            End If
            AddHandler itemMenu.Click, AddressOf SetVelocidadePlayer
        Next
        AddHandler MenuVelocidade.MouseLeave, AddressOf ControleVelocidadeSumir
        MenuVelocidade.Show(Me, New Point(Controles.Left + 262, Controles.Top - 214))
    End Sub

    Private Sub Volume_Click(sender As Object, e As EventArgs) Handles Volume.Click
        ControleVolume.Left = Controles.Left + 39
        ControleVolume.Top = Controles.Top - 116
        ControleVolume.Show()
    End Sub

    Private Sub ControleVolumeSumir(sender As Object, e As EventArgs) Handles ControleVolume.MouseLeave, Volume.MouseLeave, TabIndexVolume.MouseLeave
        Dim O = Windows.Forms.Cursor.Position
        Dim X As Integer = Me.PointToClient(O).X
        Dim Y As Integer = Me.PointToClient(O).Y
        Dim a1 As Boolean = False
        If X >= Volume.Left And X < Volume.Left + Volume.Width Then
            If Y >= Volume.Top And Y < Volume.Top + Volume.Height Then
                a1 = True
            End If
        End If
        Dim a2 As Boolean = False
        If X >= ControleVolume.Left And X < ControleVolume.Left + ControleVolume.Width Then
            If Y >= ControleVolume.Top And Y < ControleVolume.Top + ControleVolume.Height Then
                a2 = True
            End If
        End If
        If a1 = False And a2 = False Then
            ControleVolume.Hide()
        End If
    End Sub

    Private Sub ControleVelocidadeSumir(sender As Object, e As EventArgs) Handles Velocidade.MouseLeave
        Dim O = Windows.Forms.Cursor.Position
        Dim X As Integer = Me.PointToClient(O).X
        Dim Y As Integer = Me.PointToClient(O).Y
        Dim a1 As Boolean = False
        If X >= Velocidade.Left And X < Velocidade.Left + Velocidade.Width Then
            If Y >= Velocidade.Top And Y < Velocidade.Top + Velocidade.Height Then
                a1 = True
            End If
        End If
        Dim a2 As Boolean = False
        If O.X >= MenuVelocidade.Left And O.X < MenuVelocidade.Left + MenuVelocidade.Width Then
            If O.Y >= MenuVelocidade.Top And O.Y < MenuVelocidade.Top + MenuVelocidade.Height Then
                a2 = True
            End If
        End If
        If a1 = False And a2 = False Then
            MenuVelocidade.Hide()
        End If
    End Sub

    Private Sub ControleVolume_Scroll(sender As Object, e As EventArgs) Handles TabIndexVolume.Scroll
        SP.SetVolume(TabIndexVolume.Value)
    End Sub
End Class