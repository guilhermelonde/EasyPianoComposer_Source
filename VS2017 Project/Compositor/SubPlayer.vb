'--- SubPlayer ---
'Autor: Guilherme Pereira Porto Londe
'Última modificação: 11 de março de 2018

Imports System.Windows.Forms
Imports System.Collections

Public Class SubPlayer

    Public Const MaxComprim = 271
    Public Const MaxParalel = 10
    Public Const MaxAltura = 61
    Private ComprimentoAtual As Integer
    Friend WithEvents Tela As System.Windows.Forms.Label
    Friend WithEvents Barra As System.Windows.Forms.ProgressBar
    Friend WithEvents Ui As TelaPrincipal
    Public orig, bm, CLR As System.Drawing.Bitmap
    Public ObjetoPlayer As New Player(Me)
    Private QuaisNotas(MaxComprim, MaxParalel) As Integer
    Private NumNotas(MaxComprim) As Integer
    Public Tabela(MaxComprim, MaxAltura) As NotaGrafica
    Private ModoEdicao, ModoCascata As Boolean
    Public dX, dY As Integer
    Private difH, difV, RaioMax As Double
    Private margemH, margemV As Integer
    Private UltimaPosicaoMouse As New System.Drawing.Point(-1, -1)
    Private UltimaPosicaoX As Integer
    Private PrimeiraPosicaoX As Integer
    Private MousePressionado As Boolean
    Private FeitoMovimento As Boolean
    Private BotaoDireito As Boolean
    Public GradeVisivel As Boolean
    Public AtivacaoNota As Boolean
    Private MarcaInicioFim As Integer
    Private ObjetoSelecaoRetangular As SelecaoRetangular
    Private RetirarDaSelecao As System.Drawing.Point
    Private ListaDesativaTeclaEscala, ListaDesativaTeclaTI As New List(Of Integer)
    Public NotasSelecionadas As New List(Of NotaSelecionada)
    Private AreaTransferencia As New List(Of Nota)
    Private PilhaDesfazer, PilhaRefazer As New Stack(Of NoPilha)



    Public Class Nota
        Public Escala, TempoInicio As Integer
    End Class

    Public Class NotaSelecionada
        Implements IComparable(Of NotaSelecionada)
        Public Escala, TempoAtual, TempoOriginal As Integer
        Public EraSelecionado, ESelecionado, FoiInserido As Boolean

        Public Function CompareTo(Outro As NotaSelecionada) As Integer _
        Implements IComparable(Of NotaSelecionada).CompareTo
            If TempoAtual < Outro.TempoAtual Then
                Return -1
            End If
            Return 1
        End Function
    End Class

    Private Class NoPilha
        Public Escala, Nseguintes, TempoInicio As Integer
        Public ParaInserir As Boolean
    End Class

    Public Class NotaGrafica
        Public Visivel As Boolean
        Public Selecionado As Boolean
        Private SubPlayerPai As SubPlayer
        Public X As Integer
        Public Y As Integer
        Private RaioMax As Double
        Private ListaV As New List(Of Integer)

        Public Sub New(ByRef Tela As System.Windows.Forms.Label,
                       ByRef SubPlayerPai As SubPlayer)
            Visivel = False
            Selecionado = False
            Me.SubPlayerPai = SubPlayerPai
        End Sub

        Public Sub SetCoordenadas(ByVal Left As Integer, ByVal Top As Integer, ByVal RaioMax As Double)
            Me.RaioMax = RaioMax
            X = Left - SubPlayerPai.dX
            Y = Top - SubPlayerPai.dY
            Dim RM_2 As Double = RaioMax * RaioMax
            ListaV.Clear()
            For i = Math.Ceiling(X - RaioMax) To Math.Floor(X + RaioMax)
                Dim u As Integer = Math.Floor(Math.Sqrt(RM_2 - ((X - i) * (X - i))))
                ListaV.Add(u)
            Next i
        End Sub

        Public Sub SetVisivel(ByVal Arg As Boolean)
            If SubPlayerPai.Tela.Width <= 0 Then
                Return
            End If
            If Arg = Visivel Then
                If Visivel = True And Selecionado = True Then
                    SetSelecionado(False)
                End If
                Return
            End If
            Selecionado = False
            If Arg = True Then
                Dim i = Math.Ceiling(X - RaioMax)
                For Each u As Integer In ListaV
                    For j = Y - u To Y + u
                        SubPlayerPai.bm.SetPixel(i, j, System.Drawing.Color.Beige)
                    Next j
                    i += 1
                Next
            Else
                Dim i = Math.Ceiling(X - RaioMax)
                For Each u As Integer In ListaV
                    For j = Y - u To Y + u
                        SubPlayerPai.bm.SetPixel(i, j, SubPlayerPai.orig.GetPixel(i, j))
                    Next j
                    i += 1
                Next
            End If
            Visivel = Arg
        End Sub

        Public Sub SetSelecionado(ByVal Arg As Boolean)
            If SubPlayerPai.Tela.Width <= 0 Then
                Return
            End If
            If Arg = Selecionado Or Visivel = False Then
                Return
            End If

            Dim RM_2 As Double = RaioMax * RaioMax
            If Arg = True Then
                Dim i = Math.Ceiling(X - RaioMax)
                For Each u As Integer In ListaV
                    For j = Y - u To Y + u
                        SubPlayerPai.bm.SetPixel(i, j, System.Drawing.Color.MediumBlue)
                    Next j
                    i += 1
                Next
            Else
                Dim i = Math.Ceiling(X - RaioMax)
                For Each u As Integer In ListaV
                    For j = Y - u To Y + u
                        SubPlayerPai.bm.SetPixel(i, j, System.Drawing.Color.Beige)
                    Next j
                    i += 1
                Next
            End If
            Selecionado = Arg
        End Sub

        Public Sub SetAtivado()
            If SubPlayerPai.Tela.Width <= 0 Then
                Return
            End If
            Dim i = Math.Ceiling(X - RaioMax)
            For Each u As Integer In ListaV
                For j = Y - u To Y + u
                    SubPlayerPai.bm.SetPixel(i, j, System.Drawing.Color.Red)
                Next j
                i += 1
            Next
        End Sub

        Public Function EstaNaRegiao(ByVal X As Integer,
                                     ByVal Y As Integer) As Boolean
            If Visivel = False Then
                Return False
            End If
            If (X - Me.X) * (X - Me.X) + (Y - Me.Y) * (Y - Me.Y) <= (RaioMax + 1) * (RaioMax + 1) Then
                Return True
            End If
            Return False
        End Function
    End Class

    Private Class SelecaoRetangular
        Public ESelecaoRetangular As Boolean
        Private Tela As System.Windows.Forms.Label
        Private Ox, Oy, Fx, Fy As Integer
        Private SegA, SegB, SegC, SegD As New List(Of System.Drawing.Color)
        Private SubPlayerPai As SubPlayer
        Public EraSelecionado(MaxComprim, MaxAltura) As Boolean

        Public Sub New(ByRef Tela As System.Windows.Forms.Label,
                       ByRef SubPlayerPai As SubPlayer)
            Me.Tela = Tela
            Me.SubPlayerPai = SubPlayerPai
        End Sub

        Public Sub IniciaSelecaoRetangular(ByRef O As System.Drawing.Point)
            Dim x As Integer = Tela.PointToClient(O).X - SubPlayerPai.dX
            Dim y As Integer = Tela.PointToClient(O).Y - SubPlayerPai.dY
            Fx = x
            Ox = x
            Fy = y
            Oy = y
            ESelecaoRetangular = True
            For i = 0 To SubPlayerPai.ComprimentoAtual - 1
                For j = 0 To MaxAltura - 1
                    If SubPlayerPai.Tabela(i, j).Selecionado And SubPlayerPai.Tabela(i, j).Visivel Then
                        EraSelecionado(i, j) = True
                    Else
                        EraSelecionado(i, j) = False
                    End If
                Next
            Next
        End Sub

        Public Sub SetSelecaoRetangular(ByRef O As System.Drawing.Point)
            Tela.Image = SubPlayerPai.CLR
            Dim x As Integer = Tela.PointToClient(O).X - SubPlayerPai.dX
            Dim y As Integer = Tela.PointToClient(O).Y - SubPlayerPai.dY
            LimpaDesenhoRetangular()
            If (x > Fx And x < Ox) Or (x < Fx And x > Ox) Then
                SelecionarNotas(New System.Drawing.Point(x, y), New System.Drawing.Point(Fx, Oy), False)
            Else
                SelecionarNotas(New System.Drawing.Point(x, y), New System.Drawing.Point(Fx, Oy), True)
            End If
            If (y > Fy And y < Oy) Or (y < Fy And y > Oy) Then
                SelecionarNotas(New System.Drawing.Point(x, y), New System.Drawing.Point(Ox, Fy), False)
            Else
                SelecionarNotas(New System.Drawing.Point(x, y), New System.Drawing.Point(Ox, Fy), True)
            End If
            GeraDesenhoRetangular(x, y)
            Fx = x
            Fy = y
            Tela.Image = SubPlayerPai.bm
        End Sub

        Private Sub LimpaDesenhoRetangular()
            Dim l As Integer
            l = Math.Max(Math.Min(Fx, Ox) + 1, 0)
            For Each it As System.Drawing.Color In SegD
                SubPlayerPai.bm.SetPixel(l, Oy, it)
                l += 1
            Next
            SegD.Clear()
            If Fy >= 0 And Fy < SubPlayerPai.bm.Height Then
                l = Math.Max(Math.Min(Fx, Ox) + 1, 0)
                For Each it As System.Drawing.Color In SegB
                    SubPlayerPai.bm.SetPixel(l, Fy, it)
                    l += 1
                Next
                SegB.Clear()
            End If
            l = Math.Max(Math.Min(Fy, Oy), 0)
            For Each it As System.Drawing.Color In SegC
                SubPlayerPai.bm.SetPixel(Ox, l, it)
                l += 1
            Next
            SegC.Clear()
            If Fx >= 0 And Fx < SubPlayerPai.bm.Width Then
                l = Math.Max(Math.Min(Fy, Oy), 0)
                For Each it As System.Drawing.Color In SegA
                    SubPlayerPai.bm.SetPixel(Fx, l, it)
                    l += 1
                Next
                SegA.Clear()
            End If
        End Sub

        Private Sub GeraDesenhoRetangular(ByVal x As Integer, ByVal y As Integer)
            Dim l, lsup, linf As Integer
            If x >= 0 And x < SubPlayerPai.bm.Width Then
                linf = Math.Max(Math.Min(y, Oy), 0)
                lsup = Math.Min(Math.Max(y, Oy), SubPlayerPai.bm.Height - 1)
                For l = linf To lsup
                    SegA.Add(SubPlayerPai.bm.GetPixel(x, l))
                    SubPlayerPai.bm.SetPixel(x, l, System.Drawing.Color.MediumBlue)
                Next
            End If
            linf = Math.Max(Math.Min(y, Oy), 0)
            lsup = Math.Min(Math.Max(y, Oy), SubPlayerPai.bm.Height - 1)
            For l = linf To lsup
                SegC.Add(SubPlayerPai.bm.GetPixel(Ox, l))
                SubPlayerPai.bm.SetPixel(Ox, l, System.Drawing.Color.MediumBlue)
            Next
            If y >= 0 And y < SubPlayerPai.bm.Height Then
                linf = Math.Max(Math.Min(x, Ox) + 1, 0)
                lsup = Math.Min(Math.Max(x, Ox) - 1, SubPlayerPai.bm.Width - 1)
                For l = linf To lsup
                    SegB.Add(SubPlayerPai.bm.GetPixel(l, y))
                    SubPlayerPai.bm.SetPixel(l, y, System.Drawing.Color.MediumBlue)
                Next
            End If
            linf = Math.Max(Math.Min(x, Ox) + 1, 0)
            lsup = Math.Min(Math.Max(x, Ox) - 1, SubPlayerPai.bm.Width - 1)
            For l = linf To lsup
                SegD.Add(SubPlayerPai.bm.GetPixel(l, Oy))
                SubPlayerPai.bm.SetPixel(l, Oy, System.Drawing.Color.MediumBlue)
            Next
        End Sub

        Public Sub FinalizaSelecaoRetangular()
            If ESelecaoRetangular Then
                ESelecaoRetangular = False
                Tela.Image = SubPlayerPai.CLR
                LimpaDesenhoRetangular()
                Dim aux As New List(Of NotaSelecionada)
                For Each it As NotaSelecionada In SubPlayerPai.NotasSelecionadas
                    If it.TempoAtual >= 0 And it.TempoAtual < SubPlayerPai.ComprimentoAtual Then
                        If SubPlayerPai.Tabela(it.TempoAtual, it.Escala).Selecionado Then
                            aux.Add(it)
                        End If
                    Else
                        aux.Add(it)
                    End If
                Next
                For i = 0 To SubPlayerPai.ComprimentoAtual - 1
                    For j = 0 To MaxAltura - 1
                        If SubPlayerPai.Tabela(i, j).Visivel And SubPlayerPai.Tabela(i, j).Selecionado Then
                            If EraSelecionado(i, j) = False Then
                                Dim P As New NotaSelecionada
                                P.Escala = j
                                P.TempoOriginal = i
                                P.TempoAtual = i
                                P.EraSelecionado = False
                                P.ESelecionado = True
                                P.FoiInserido = True
                                aux.Add(P)
                            End If
                        End If
                    Next
                Next
                SubPlayerPai.NotasSelecionadas.Clear()
                For Each it As NotaSelecionada In aux
                    SubPlayerPai.NotasSelecionadas.Add(it)
                Next
                Tela.Image = SubPlayerPai.bm
            End If
        End Sub

        Private Sub SelecionarNotas(ByVal A As System.Drawing.Point, ByVal B As System.Drawing.Point,
                                    ByVal arg As Boolean)
            Dim SupEsqX, SupEsqY, InfDirX, InfDirY As Integer
            SupEsqX = Math.Min(A.X, B.X)
            SupEsqY = Math.Min(A.Y, B.Y)
            InfDirX = Math.Max(A.X, B.X)
            InfDirY = Math.Max(A.Y, B.Y)
            A = SubPlayerPai.NotaMaisProxima(New System.Drawing.Point(SupEsqX, SupEsqY))
            B = SubPlayerPai.NotaMaisProxima(New System.Drawing.Point(InfDirX, InfDirY))
            For x = Math.Max(A.X, 0) To Math.Min(B.X, SubPlayerPai.ComprimentoAtual - 1)
                For y = Math.Max(B.Y, 0) To Math.Min(A.Y, MaxAltura - 1)
                    SubPlayerPai.Tabela(x, y).SetSelecionado(arg)
                Next
            Next
        End Sub

    End Class


    Private Function DescobreNota(ByRef O As System.Drawing.Point) As System.Drawing.Point
        Dim R As New System.Drawing.Point(-1, -1)
        Dim cX As Integer = Tela.PointToClient(O).X - dX
        Dim cY As Integer = Tela.PointToClient(O).Y - dY
        Dim p As System.Drawing.Point = NotaMaisProxima(New System.Drawing.Point(cX, cY))
        If p.X < 0 Or p.X >= ComprimentoAtual Or p.Y < 0 Or p.Y >= MaxAltura Then
            Return R
        ElseIf Tabela(p.X, p.Y).Visivel = True And Tabela(p.X, p.Y).EstaNaRegiao(cX, cY) = True Then
            R.X = p.X
            R.Y = p.Y
        End If
        UltimaPosicaoX = p.X
        Return R
    End Function

    Private Sub Tela_MouseDown(ByVal sender As System.Object,
                           ByVal e As System.Windows.Forms.MouseEventArgs) Handles Tela.MouseDown
        BotaoDireito = False
        Dim R As System.Drawing.Point = DescobreNota(Windows.Forms.Cursor.Position)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Tela.Image = CLR
            Dim aux As Boolean = R.X <> -1
            If aux Then
                aux = Tabela(R.X, R.Y).Selecionado
            End If
            If My.Computer.Keyboard.CtrlKeyDown = False And Not aux Then
                For i = 0 To ComprimentoAtual - 1
                    For j = 0 To MaxParalel - 1
                        Tabela(i, QuaisNotas(i, j)).SetSelecionado(False)
                    Next
                Next
                NotasSelecionadas.Clear()
            End If
            RetirarDaSelecao.X = -1
            RetirarDaSelecao.Y = -1
            ObjetoSelecaoRetangular.ESelecaoRetangular = False
            If R.X <> -1 And R.Y <> -1 Then
                Dim P As New NotaSelecionada
                P.Escala = R.Y
                P.TempoOriginal = R.X
                P.TempoAtual = R.X
                P.EraSelecionado = False
                P.ESelecionado = True
                P.FoiInserido = True
                If Tabela(R.X, R.Y).Selecionado = False Then
                    Tabela(R.X, R.Y).SetSelecionado(True)
                    NotasSelecionadas.Add(P)
                Else
                    RetirarDaSelecao.X = R.X
                    RetirarDaSelecao.Y = R.Y
                End If
            Else
                ObjetoSelecaoRetangular.IniciaSelecaoRetangular(Windows.Forms.Cursor.Position)
            End If
            MousePressionado = True
            FeitoMovimento = False
            Tela.Image = bm
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            ObjetoSelecaoRetangular.FinalizaSelecaoRetangular()
            FinalizaMovimento()
            BotaoDireito = True
        End If
    End Sub

    Private Sub FinalizaMovimento()
        MousePressionado = False
        If FeitoMovimento = False Or NotasSelecionadas.Count = 0 Then
            If RetirarDaSelecao.X <> -1 Then
                Tabela(RetirarDaSelecao.X, RetirarDaSelecao.Y).SetSelecionado(False)
                Dim i As Integer = 0
                For Each item As NotaSelecionada In NotasSelecionadas
                    If item.Escala = RetirarDaSelecao.Y And item.TempoOriginal = RetirarDaSelecao.X Then
                        NotasSelecionadas.RemoveAt(i)
                        Tela.Image = CLR
                        Tela.Image = bm
                        Return
                    End If
                    i += 1
                Next
            End If
            Return
        End If
        FeitoMovimento = False
        Dim NotasSelecionadasAux As New List(Of NotaSelecionada)
        Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2)
        Dim c As Integer = 0
        For Each it As NotaSelecionada In NotasSelecionadas
            Dim TA As Integer = ObjetoPlayer.Cursor - (Var - it.TempoAtual)
            If it.TempoOriginal <> -1000000000 Then
                Dim NP As New NoPilha
                NP.Escala = it.Escala
                NP.TempoInicio = ObjetoPlayer.Cursor - (Var - it.TempoOriginal)
                NP.Nseguintes = 1
                NP.ParaInserir = True
                PilhaDesfazer.Push(NP)
                c += 1
            End If
            If TA >= 0 And TA < ObjetoPlayer.CompMaximoMelodia Then
                If it.FoiInserido = False Then
                    GoTo fimlaço
                End If
                Dim NP2 As New NoPilha
                NP2.Escala = it.Escala
                NP2.TempoInicio = TA
                NP2.Nseguintes = 1
                NP2.ParaInserir = False
                PilhaDesfazer.Push(NP2)
                NotasSelecionadasAux.Add(it)
                c += 1
            End If
fimlaço:
        Next
        InserirNotasSelecionadasPlayer(NotasSelecionadasAux)
        PilhaDesfazer.Peek.Nseguintes = c
        PilhaRefazer.Clear()
        Atualizar()
        Tela.Image = CLR
        Dim ListaTemp As New List(Of NotaSelecionada)
        For Each it As NotaSelecionada In NotasSelecionadas
            If it.ESelecionado = True Then
                Dim P As New NotaSelecionada
                P.Escala = it.Escala
                P.TempoOriginal = it.TempoAtual
                P.TempoAtual = it.TempoAtual
                P.FoiInserido = it.FoiInserido
                Dim TA As Integer = ObjetoPlayer.Cursor - (Var - P.TempoAtual)
                If TA >= 0 And TA < ObjetoPlayer.CompMaximoMelodia And P.TempoAtual >= 0 Then
                    If P.TempoAtual < ComprimentoAtual AndAlso Tabela(P.TempoAtual, P.Escala).Visivel Then
                        Tabela(P.TempoAtual, P.Escala).SetVisivel(True)
                        Tabela(P.TempoAtual, P.Escala).Selecionado = False
                        Tabela(P.TempoAtual, P.Escala).SetSelecionado(True)
                    End If
                    ListaTemp.Add(P)
                End If
            End If
        Next
        NotasSelecionadas.Clear()
        For Each it As NotaSelecionada In ListaTemp
            Dim P As New NotaSelecionada
            P.Escala = it.Escala
            P.TempoOriginal = it.TempoAtual
            P.TempoAtual = it.TempoAtual
            P.EraSelecionado = False
            P.ESelecionado = True
            P.FoiInserido = it.FoiInserido
            NotasSelecionadas.Add(P)
        Next
        Ui.TemAlteracao = True
        Tela.Image = bm
    End Sub

    Private Sub Tela_MouseUp(ByVal sender As System.Object,
                           ByVal e As System.Windows.Forms.MouseEventArgs) Handles Tela.MouseUp
        Dim ObjetoIdioma As New Idioma
        Dim IdIdioma = Ui.IdIdioma
        If BotaoDireito Then
            Dim cms = New ContextMenuStrip
            If NotasSelecionadas.Count > 0 Then
                Dim itemCopiar = cms.Items.Add(ObjetoIdioma.Tabela(7, IdIdioma))
                itemCopiar.Tag = 1
                AddHandler itemCopiar.Click, AddressOf EscolhaMenu
                Dim itemCortar = cms.Items.Add(ObjetoIdioma.Tabela(6, IdIdioma))
                itemCortar.Tag = 2
                AddHandler itemCortar.Click, AddressOf EscolhaMenu
            End If
            Dim itemColar = cms.Items.Add(ObjetoIdioma.Tabela(8, IdIdioma))
            itemColar.Tag = 3
            AddHandler itemColar.Click, AddressOf EscolhaMenu
            If NotasSelecionadas.Count > 0 Then
                Dim itemExcluir = cms.Items.Add(ObjetoIdioma.Tabela(9, IdIdioma))
                itemExcluir.Tag = 2
                AddHandler itemExcluir.Click, AddressOf EscolhaMenu
            End If
            cms.Show(Tela, e.Location)
            UltimaPosicaoX = (Tela.PointToClient(Windows.Forms.Cursor.Position).X - dX)
            BotaoDireito = False
        Else
            ObjetoSelecaoRetangular.FinalizaSelecaoRetangular()
            FinalizaMovimento()
        End If
    End Sub

    Private Sub EscolhaMenu(ByVal sender As Object, ByVal e As EventArgs)
        Dim item = CType(sender, ToolStripMenuItem)
        Dim selection = CInt(item.Tag)
        Select Case selection
            Case 1
                Copiar()
            Case 2
                Copiar()
                Excluir()
            Case 3
                UltimaPosicaoX = Math.Round((UltimaPosicaoX - margemH) / difH)
                Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2)
                Colar(ObjetoPlayer.Cursor + (UltimaPosicaoX - Var))
            Case 4
                Excluir()
        End Select
    End Sub

    Private Sub CompletarSelecaoCascata()
        If FeitoMovimento = False And ModoCascata = True Then
            Dim mdi As Integer = -1000000
            Dim lsup As New List(Of Integer)
            For Each it As NotaSelecionada In NotasSelecionadas
                If it.TempoOriginal > mdi Then
                    mdi = it.TempoOriginal
                    lsup.Clear()
                End If
                If it.TempoOriginal = mdi Then
                    lsup.Add(it.Escala)
                End If
            Next
            Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2)
            Dim dif = mdi - (ObjetoPlayer.Cursor - (Var - mdi))
            mdi -= dif
            Dim alde As List(Of Integer) = ObjetoPlayer.PegaNotas(mdi)
            For i = mdi To ObjetoPlayer.CompMaximoMelodia - 1
                alde = ObjetoPlayer.PegaNotas(i)
                For Each it As Integer In alde
                    If i <> mdi OrElse lsup.Contains(it) = False Then
                        Dim P As New NotaSelecionada
                        P.Escala = it
                        P.TempoOriginal = i + dif
                        P.TempoAtual = i + dif
                        P.EraSelecionado = False
                        P.ESelecionado = False
                        P.FoiInserido = True
                        NotasSelecionadas.Add(P)
                    End If
                Next
            Next
        End If
    End Sub

    Private Sub ExcluirNotasSelecionadasPlayer()
        Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2)
        For Each it As NotaSelecionada In NotasSelecionadas
            Dim TA As Integer = ObjetoPlayer.Cursor - (Var - it.TempoAtual)
            ObjetoPlayer.Remover(TA, it.Escala)
        Next
    End Sub

    Private Sub InserirNotasSelecionadasPlayer(ByRef Notas As List(Of NotaSelecionada))
        Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2) - (UltimaPosicaoX - PrimeiraPosicaoX)
        For Each it As NotaSelecionada In Notas
            Dim TA As Integer = ObjetoPlayer.Cursor - (Var - it.TempoAtual)
            ObjetoPlayer.Inserir(TA, it.Escala)
        Next
    End Sub

    Private Sub Tela_MouseMove(ByVal sender As System.Object,
                           ByVal e As System.EventArgs) Handles Tela.MouseMove
        If Windows.Forms.Cursor.Position = UltimaPosicaoMouse Then
            Return
        End If
        If ObjetoSelecaoRetangular.ESelecaoRetangular Then
            UltimaPosicaoMouse = Windows.Forms.Cursor.Position
            ObjetoSelecaoRetangular.SetSelecaoRetangular(Windows.Forms.Cursor.Position)
            Return
        End If
        If MousePressionado = True And ObjetoPlayer.Tocando = False Then
            If NotasSelecionadas.Count = 0 Then
                Return
            End If
            Dim cX As Integer = Tela.PointToClient(Windows.Forms.Cursor.Position).X - dX
            Dim pX As Integer = Math.Round((cX - margemH) / difH)
            If FeitoMovimento = False Then
                CompletarSelecaoCascata()
                ExcluirNotasSelecionadasPlayer()
                NotasSelecionadas.Sort()
            End If
            MoveSelecionados(pX, UltimaPosicaoX)
            PrimeiraPosicaoX = pX
            UltimaPosicaoX = pX
            FeitoMovimento = True
        Else
            UltimaPosicaoMouse = Windows.Forms.Cursor.Position
            Dim R As System.Drawing.Point = DescobreNota(UltimaPosicaoMouse)
            If R.X <> -1 And R.Y <> -1 Then
                Tela.Cursor = System.Windows.Forms.Cursors.Hand
            Else
                Tela.Cursor = System.Windows.Forms.Cursors.Arrow
            End If
        End If
    End Sub

    Private Sub MoveSelecionados(ByVal nX As Integer, ByVal uX As Integer)
        Tela.Image = CLR
        Dim dif As Integer = (nX - uX)
        Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2)
        For Each it As NotaSelecionada In NotasSelecionadas
            Dim TA As Integer = ObjetoPlayer.Cursor - (Var - it.TempoAtual)
            If TA >= 0 And TA < ObjetoPlayer.CompMaximoMelodia Then
                If it.EraSelecionado = True Then
                    If it.TempoAtual >= 0 And it.TempoAtual < ComprimentoAtual Then
                        Tabela(it.TempoAtual, it.Escala).SetSelecionado(False)
                    End If
                Else
                    If it.TempoAtual >= 0 And it.TempoAtual < ComprimentoAtual Then
                        Tabela(it.TempoAtual, it.Escala).SetVisivel(False)
                    End If
                End If
            End If
            it.TempoAtual = it.TempoAtual + dif
        Next
        Dim ListaEscalaPorTempo As New List(Of Integer)
        Dim TempoUltimaEscala As Integer = -1
        For Each it As NotaSelecionada In NotasSelecionadas
            Dim TA As Integer = ObjetoPlayer.Cursor - (Var - it.TempoAtual)
            it.FoiInserido = False
            If TA <> TempoUltimaEscala Then
                ListaEscalaPorTempo.Clear()
                Dim temp As List(Of Integer) = ObjetoPlayer.PegaNotas(TA)
                For Each it2 As Integer In temp
                    ListaEscalaPorTempo.Add(it2)
                Next
                TempoUltimaEscala = TA
            End If
            If TA >= 0 And TA < ObjetoPlayer.CompMaximoMelodia Then
                Dim iTA As Integer = it.TempoAtual
                If iTA >= 0 And iTA < ComprimentoAtual AndAlso Tabela(iTA, it.Escala).Visivel = True Then
                    it.EraSelecionado = True
                    If it.ESelecionado = True Then
                        Tabela(it.TempoAtual, it.Escala).SetSelecionado(True)
                    End If
                Else
                    it.EraSelecionado = False
                    If ListaEscalaPorTempo.Count >= MaxParalel Then
                        Continue For
                    Else
                        Dim MesmaOitava As Integer = 0
                        Dim inf As Integer = Math.Floor(it.Escala / 12) * 12
                        Dim sup As Integer = inf + 11
                        For Each item As Integer In ListaEscalaPorTempo
                            If item >= inf And item <= sup Then
                                MesmaOitava += 1
                                If item = it.Escala Then
                                    MesmaOitava = 5
                                    it.EraSelecionado = True
                                End If
                            End If
                        Next
                        If MesmaOitava >= 5 Then
                            Continue For
                        Else
                            ListaEscalaPorTempo.Add(it.Escala)
                            If iTA >= 0 And iTA < ComprimentoAtual Then
                                Tabela(it.TempoAtual, it.Escala).SetVisivel(True)
                                If it.ESelecionado = True Then
                                    Tabela(it.TempoAtual, it.Escala).Selecionado = False
                                    Tabela(it.TempoAtual, it.Escala).SetSelecionado(True)
                                End If
                            End If
                            it.FoiInserido = True
                        End If
                    End If
                End If
            End If
        Next
        Tela.Image = bm
    End Sub

    Private Sub Barra_Click(ByVal sender As System.Object,
                           ByVal e As System.EventArgs) Handles Barra.Click
        Dim v As Double = Barra.PointToClient(Windows.Forms.Cursor.Position).X / Barra.Width
        Dim noc As Integer = Math.Round(v * ObjetoPlayer.MaiorTI)
        PassaNotas(noc - ObjetoPlayer.Cursor)
        Barra.Value = (ObjetoPlayer.Cursor * 100) / ObjetoPlayer.MaiorTI
    End Sub

    Private Sub PassaNotas(ByVal dir As Integer)
        ObjetoSelecaoRetangular.FinalizaSelecaoRetangular()
        For Each it As NotaSelecionada In NotasSelecionadas
            If it.TempoAtual >= 0 And it.TempoAtual < ComprimentoAtual And it.EraSelecionado = False Then
                Tabela(it.TempoAtual, it.Escala).SetVisivel(False)
            End If
            it.TempoAtual -= dir
            it.TempoOriginal -= dir
        Next
        ObjetoPlayer.Cursor += dir
        Atualizar()
        PrimeiraPosicaoX -= dir
        For Each it As NotaSelecionada In NotasSelecionadas
            If it.TempoAtual >= 0 And it.TempoAtual < ComprimentoAtual And it.EraSelecionado = False Then
                Tabela(it.TempoAtual, it.Escala).SetVisivel(True)
                If it.ESelecionado = True Then
                    Tabela(it.TempoAtual, it.Escala).SetSelecionado(True)
                End If
            End If
        Next
        If ObjetoPlayer.Tocando = True And AtivacaoNota = True Then
            Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2)
            For i = 0 To MaxAltura - 1
                If Tabela(Var, i).Visivel Then
                    Tabela(Var, i).SetAtivado()
                End If
            Next
        End If
    End Sub

    Private Sub DesenhaMarcaInicioFim()
        Dim X As Integer = Math.Ceiling(ComprimentoAtual / 2) - ObjetoPlayer.Cursor - 1
        If X >= 0 Then
            X = Tabela(X, 0).X
        Else
            X = Math.Ceiling(ComprimentoAtual / 2) + ObjetoPlayer.CompMaximoMelodia - ObjetoPlayer.Cursor
            If X < ComprimentoAtual Then
                X = Tabela(X, 0).X
            Else
                X = -1
            End If
        End If
        If X <> -1 Then
            Dim aux As Boolean = True
            For i = 0 To bm.Height - 1
                If aux Then
                    bm.SetPixel(X, i, System.Drawing.Color.DimGray)
                    bm.SetPixel(X - 1, i, System.Drawing.Color.DimGray)
                End If
                If i Mod 3 = 0 Then
                    aux = Not aux
                End If
            Next
            MarcaInicioFim = X
        Else
            MarcaInicioFim = -1
        End If
    End Sub

    Private Sub DesfazMarcaInicioFim()
        If MarcaInicioFim <> -1 Then
            Dim aux As Boolean = True
            For i = 0 To bm.Height - 1
                If aux Then
                    bm.SetPixel(MarcaInicioFim, i, orig.GetPixel(MarcaInicioFim, i))
                    bm.SetPixel(MarcaInicioFim - 1, i, orig.GetPixel(MarcaInicioFim, i))
                End If
                If i Mod 3 = 0 Then
                    aux = Not aux
                End If
            Next
        End If
    End Sub


    Public Sub AtualizaTamanhoTabela()
        If Ui.WindowState = FormWindowState.Minimized Then
            Return
        End If
        ObjetoSelecaoRetangular.FinalizaSelecaoRetangular()
        For i = 0 To ComprimentoAtual - 1
            For j = 0 To MaxAltura - 1
                Tabela(i, j).SetVisivel(False)
            Next j
        Next i
        bm = New System.Drawing.Bitmap(My.Resources.Screen, New System.Drawing.Size(Tela.Width, Tela.Height))
        margemH = 0.015 * orig.Width
        margemV = 0.035 * orig.Height
        Dim d1 As Integer = ComprimentoAtual
        ComprimentoAtual = Math.Min((Tela.Width - 2 * margemH) / 5, MaxComprim)
        If ComprimentoAtual Mod 2 = 0 Then
            ComprimentoAtual -= 1
        End If
        d1 = (ComprimentoAtual - d1) / 2
        For Each it As NotaSelecionada In NotasSelecionadas
            it.TempoAtual += d1
            it.TempoOriginal += d1
        Next
        difH = (bm.Width - (2 * margemH)) / ComprimentoAtual
        difV = (bm.Height - (2 * margemV)) / MaxAltura
        Dim CX As Integer = Tela.Width / 2 + 1
        Dim CY As Integer = Tela.Height / 2 - 2
        If GradeVisivel Then
            For i = margemH - 3 To (bm.Width - margemH) + 3
                For j = 1 To 61 Step 12
                    bm.SetPixel(i, margemV + Math.Floor(difV * j) - 4, System.Drawing.Color.Black)
                Next
            Next
            Dim aux As Boolean = False
            For i = 0 To bm.Height - 1
                If aux Then
                    bm.SetPixel(CX, i, System.Drawing.Color.Gray)
                    bm.SetPixel(CX - 1, i, System.Drawing.Color.Gray)
                End If
                If i Mod 3 = 0 Then
                    aux = Not aux
                End If
            Next
        End If
        orig = New System.Drawing.Bitmap(bm)
        For i = 0 To ComprimentoAtual
            NumNotas(i) = 0
        Next
        Dim dX As Integer = ((Tela.Width - orig.Width) / 2)
        Dim dY As Integer = ((Tela.Height - orig.Height) / 2)
        Dim pH As Integer = Math.Ceiling(ComprimentoAtual / 2) * -1
        Dim Raio As Double = Math.Min(difV, difH)
        Raio = 2.3
        For i = 0 To ComprimentoAtual - 1
            NumNotas(i) = 0
            Dim pY As Integer = (MaxAltura / 2)
            For j = 0 To MaxAltura - 1
                Tabela(i, j).SetCoordenadas(CX + (pH * difH), CY + (pY * difV), Raio)
                pY -= 1
            Next j
            pH += 1
        Next i
        PassaNotas(0)
    End Sub

    Public Function NotaMaisProxima(ByVal c As System.Drawing.Point) As System.Drawing.Point
        Dim pX As Integer = Math.Round((3.1 + c.X - margemH) / difH)
        Dim pY As Integer = 60 - Math.Round((c.Y - margemV) / difV)
        Return New System.Drawing.Point(pX, pY)
    End Function

    Public Sub New(ByRef TelaPai As System.Windows.Forms.Label, ByRef Barra As System.Windows.Forms.ProgressBar,
                   ByRef Ui As TelaPrincipal)
        orig = My.Resources.Screen
        bm = New System.Drawing.Bitmap(My.Resources.Screen)
        Tela = TelaPai
        CLR = My.Resources.NullImage
        Me.Barra = Barra
        Me.Ui = Ui
        Ui.KeyPreview = True
        ObjetoSelecaoRetangular = New SelecaoRetangular(Tela, Me)
        For i = 0 To MaxComprim - 1
            For j = 0 To MaxAltura - 1
                Tabela(i, j) = New NotaGrafica(TelaPai, Me)
            Next j
        Next i
        ObjetoPlayer.Cursor = 0
        GradeVisivel = True
        AtivacaoNota = True
        MarcaInicioFim = -1
        AtualizaTamanhoTabela()
    End Sub

    Public Sub ProximoTabela()
        ObjetoPlayer.Cursor -= 1
        PassaNotas(1)
    End Sub

    Public Sub Inserir(ByVal Oitava As Integer, ByVal Escala As Integer)
        If ModoEdicao = False Or ObjetoPlayer.Tocando = False Then
            ObjetoPlayer.TocarNota(Oitava, Escala)
        End If
        If ModoEdicao = False Then
            Return
        End If
        Dim Pos, MesmaOitava, Inf, Sup As Integer
        Pos = Math.Ceiling(ComprimentoAtual / 2)
        Inf = (Oitava - 1) * 12
        Sup = Inf + 11
        Escala = Inf + Escala - 1
        If NumNotas(Pos) >= MaxParalel Then
            Return
        End If
        If Tabela(Pos, Escala).Visivel = True Then
            Return
        End If
        MesmaOitava = 0
        For i = 0 To NumNotas(Pos) - 1
            If QuaisNotas(Pos, i) >= Inf And QuaisNotas(Pos, i) <= Sup Then
                MesmaOitava += 1
            End If
        Next i
        If MesmaOitava >= 5 Then
            Return
        End If
        Tabela(Pos, Escala).SetVisivel(True)
        QuaisNotas(Pos, NumNotas(Pos)) = Escala
        NumNotas(Pos) += 1
        ObjetoPlayer.Inserir(ObjetoPlayer.Cursor, Escala)

        Dim NP As New NoPilha
        NP.Escala = Escala
        NP.TempoInicio = ObjetoPlayer.Cursor
        NP.Nseguintes = 1
        NP.ParaInserir = False
        PilhaDesfazer.Push(NP)
        PilhaRefazer.Clear()

        Tela.Image = CLR
        Tela.Image = bm
    End Sub

    Public Sub Proximo()
        If ObjetoPlayer.Cursor = ObjetoPlayer.CompMaximoMelodia - 1 Then
            Return
        End If
        ObjetoPlayer.Pausar()
        PassaNotas(1)
    End Sub

    Public Sub Anterior()
        If ObjetoPlayer.Cursor = 0 Then
            Return
        End If
        ObjetoPlayer.Pausar()
        PassaNotas(-1)
    End Sub

    Public Sub Atualizar()
        If ObjetoPlayer.Cursor > ObjetoPlayer.MaiorTI Then
            Barra.Value = 100
        Else
            Barra.Value = (ObjetoPlayer.Cursor * 100) / ObjetoPlayer.MaiorTI
        End If
        Tela.Image = CLR
        For i = 0 To ComprimentoAtual - 1
            For j = 0 To NumNotas(i) - 1
                Tabela(i, QuaisNotas(i, j)).SetVisivel(False)
            Next j
        Next i
        DesfazMarcaInicioFim()
        Dim Pos As Integer = Math.Ceiling(ComprimentoAtual / 2)
        For i = 0 To ComprimentoAtual - 1
            Dim A As List(Of Integer) = ObjetoPlayer.PegaNotas(ObjetoPlayer.Cursor + i - Pos)
            NumNotas(i) = 0
            For Each Item As Integer In A
                QuaisNotas(i, NumNotas(i)) = Item
                Tabela(i, Item).SetVisivel(True)
                NumNotas(i) += 1
            Next
        Next i
        DesenhaMarcaInicioFim()
        Tela.Image = bm
    End Sub

    Public Sub Desfazer()
        LimpaSelecao()
        If PilhaDesfazer.Count = 0 Then
            Return
        End If
        Dim NumOp = PilhaDesfazer.Peek.Nseguintes
        For i = 0 To NumOp - 1
            If PilhaDesfazer.Peek.ParaInserir = True Then
                ObjetoPlayer.Inserir(PilhaDesfazer.Peek.TempoInicio, PilhaDesfazer.Peek.Escala)
                PilhaDesfazer.Peek.ParaInserir = False
                PilhaRefazer.Push(PilhaDesfazer.Peek)
            Else
                ObjetoPlayer.Remover(PilhaDesfazer.Peek.TempoInicio, PilhaDesfazer.Peek.Escala)
                PilhaDesfazer.Peek.ParaInserir = True
                PilhaRefazer.Push(PilhaDesfazer.Peek)
            End If
            PilhaDesfazer.Pop()
        Next i
        PilhaRefazer.Peek.Nseguintes = NumOp
        Atualizar()
    End Sub

    Public Sub Refazer()
        LimpaSelecao()
        If PilhaRefazer.Count = 0 Then
            Return
        End If
        Dim NumOp = PilhaRefazer.Peek.Nseguintes
        For i = 0 To NumOp - 1
            If PilhaRefazer.Peek.ParaInserir = True Then
                ObjetoPlayer.Inserir(PilhaRefazer.Peek.TempoInicio, PilhaRefazer.Peek.Escala)
                PilhaRefazer.Peek.ParaInserir = False
                PilhaDesfazer.Push(PilhaRefazer.Peek)
            Else
                ObjetoPlayer.Remover(PilhaRefazer.Peek.TempoInicio, PilhaRefazer.Peek.Escala)
                PilhaRefazer.Peek.ParaInserir = True
                PilhaDesfazer.Push(PilhaRefazer.Peek)
            End If
            PilhaRefazer.Pop()
        Next i
        PilhaDesfazer.Peek.Nseguintes = NumOp
        Atualizar()
    End Sub

    Public Sub SetModoCascata(ByVal ModoCascata As Boolean)
        FinalizaMovimento()
        Me.ModoCascata = ModoCascata
    End Sub

    Public Sub SetModoEdicao(ByVal ModoEdicao As Boolean)
        Me.ModoEdicao = ModoEdicao
    End Sub

    Public Sub SetVelocidade(ByVal Velocidade As Decimal)
        ObjetoPlayer.SetVelocidade(Velocidade)
    End Sub

    Public Sub SetGradeVisivel()
        GradeVisivel = Not GradeVisivel
        AtualizaTamanhoTabela()
    End Sub

    Public Sub SetAtivacaoNota()
        AtivacaoNota = Not AtivacaoNota
    End Sub

    Public Sub SetVolume(ByVal V As Integer)
        ObjetoPlayer.SetVolume(V)
    End Sub

    Public Sub SelecionaTudo()
        FinalizaMovimento()
        NotasSelecionadas.Clear()
        Tela.Image = CLR
        Dim alde As List(Of Integer)
        Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2)
        For i = 0 To ObjetoPlayer.CompMaximoMelodia - 1
            alde = ObjetoPlayer.PegaNotas(i)
            Dim TA As Integer = i - (ObjetoPlayer.Cursor - Var)
            Dim OK As Boolean = False
            If TA >= 0 And TA < MaxComprim Then
                OK = True
            End If
            For Each it As Integer In alde
                Dim P As New NotaSelecionada
                P.Escala = it
                P.TempoOriginal = TA
                P.TempoAtual = TA
                P.EraSelecionado = False
                P.ESelecionado = True
                P.FoiInserido = True
                NotasSelecionadas.Add(P)
                If OK Then
                    Tabela(TA, it).SetSelecionado(True)
                End If
            Next
        Next
        Tela.Image = bm
    End Sub

    Public Sub CarregarArquivo(ByVal NomeArquivo As String)
        ObjetoPlayer.Carregar(NomeArquivo)
        Parar()
        PilhaDesfazer.Clear()
        PilhaRefazer.Clear()
        Ui.TemAlteracao = False
    End Sub

    Public Sub SalvarArquivo(ByVal NomeArquivo As String)
        ObjetoPlayer.Salvar(NomeArquivo)
        PilhaDesfazer.Clear()
        PilhaRefazer.Clear()
        Ui.TemAlteracao = True
    End Sub

    Public Sub NovoArquivo()
        ObjetoPlayer.NovoArquivo()
        Parar()
    End Sub

    Public Sub Tocar()
        FinalizaMovimento()
        ObjetoPlayer.Tocar(ObjetoPlayer.Cursor)
        Tela.Image = CLR
        If AtivacaoNota = True Then
            Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2)
            For i = 0 To MaxAltura - 1
                If Tabela(Var, i).Visivel Then
                    Tabela(Var, i).SetAtivado()
                End If
            Next
        End If
        Tela.Image = bm
    End Sub

    Public Sub Pausar()
        ObjetoPlayer.Pausar()
        While ListaDesativaTeclaTI.Count > 0
            Ui.DesativaTecla(ListaDesativaTeclaEscala.Item(0))
            ListaDesativaTeclaEscala.RemoveAt(0)
            ListaDesativaTeclaTI.RemoveAt(0)
        End While
        PassaNotas(0)
    End Sub

    Public Sub Parar()
        ObjetoPlayer.Pausar()
        PassaNotas(-ObjetoPlayer.Cursor)
        While ListaDesativaTeclaTI.Count > 0
            Ui.DesativaTecla(ListaDesativaTeclaEscala.Item(0))
            ListaDesativaTeclaEscala.RemoveAt(0)
            ListaDesativaTeclaTI.RemoveAt(0)
        End While
    End Sub

    Public Sub AtivaTecla()
        Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2)
        For i = 0 To NumNotas(Var) - 1
            Ui.AtivaTecla(QuaisNotas(Var, i) + 1)
            ListaDesativaTeclaEscala.Add(QuaisNotas(Var, i) + 1)
            ListaDesativaTeclaTI.Add(ObjetoPlayer.Cursor)
        Next i
        While ListaDesativaTeclaTI.Count > 0
            Dim vit As Integer = ListaDesativaTeclaTI.Item(0)
            If vit + 2 <= ObjetoPlayer.Cursor Or vit - 2 >= ObjetoPlayer.Cursor Then
                Dim Escala As Integer = ListaDesativaTeclaEscala.Item(0)
                ListaDesativaTeclaEscala.RemoveAt(0)
                ListaDesativaTeclaTI.RemoveAt(0)
                If ListaDesativaTeclaEscala.Contains(Escala) = False Then
                    Ui.DesativaTecla(Escala)
                End If
            Else
                GoTo FimLaço
            End If
        End While
FimLaço:
    End Sub

    Public Sub LimpaSelecao()
        ObjetoSelecaoRetangular.FinalizaSelecaoRetangular()
        FinalizaMovimento()
        Tela.Image = CLR
        For i = 0 To ComprimentoAtual - 1
            For j = 0 To MaxParalel - 1
                Tabela(i, QuaisNotas(i, j)).SetSelecionado(False)
            Next
        Next
        NotasSelecionadas.Clear()
        Tela.Image = bm
    End Sub

    Public Sub Copiar()
        AreaTransferencia.Clear()
        Dim min As Integer = 1000000000
        NotasSelecionadas.Sort()
        For Each it As NotaSelecionada In NotasSelecionadas
            min = Math.Min(min, it.TempoAtual)
            GoTo fimlaçocp
        Next
fimlaçocp:
        For Each it As NotaSelecionada In NotasSelecionadas
            Dim O As New Nota
            O.Escala = it.Escala
            O.TempoInicio = it.TempoAtual - min
            AreaTransferencia.Add(O)
        Next
    End Sub

    Public Sub Colar(ByVal TempoInicio As Integer)
        LimpaSelecao()
        Dim ListaEscalaPorTempo As New List(Of Integer)
        Dim TempoUltimaEscala As Integer = -1
        Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2) + (TempoInicio - ObjetoPlayer.Cursor)
        For Each it As Nota In AreaTransferencia
            Dim P As New NotaSelecionada
            Dim TA As Integer = it.TempoInicio + TempoInicio
            Dim iTA As Integer = it.TempoInicio + Var
            P.Escala = it.Escala
            P.TempoOriginal = -1000000000
            P.TempoAtual = iTA
            P.EraSelecionado = False
            P.ESelecionado = True
            P.FoiInserido = True
            If TA <> TempoUltimaEscala Then
                ListaEscalaPorTempo.Clear()
                Dim temp As List(Of Integer) = ObjetoPlayer.PegaNotas(TA)
                For Each it2 As Integer In temp
                    ListaEscalaPorTempo.Add(it2)
                Next
                TempoUltimaEscala = TA
            End If
            If TA >= 0 And TA < ObjetoPlayer.CompMaximoMelodia Then
                If iTA >= 0 And iTA < ComprimentoAtual AndAlso Tabela(iTA, it.Escala).Visivel = True Then
                    P.EraSelecionado = True
                    If P.ESelecionado = True Then
                        Tabela(iTA, P.Escala).SetSelecionado(True)
                    End If
                Else
                    P.EraSelecionado = False
                    If ListaEscalaPorTempo.Count >= MaxParalel Then
                        Continue For
                    Else
                        Dim MesmaOitava As Integer = 0
                        Dim inf As Integer = Math.Floor(it.Escala / 12) * 12
                        Dim sup As Integer = inf + 11
                        For Each item As Integer In ListaEscalaPorTempo
                            If item >= inf And item <= sup Then
                                MesmaOitava += 1
                                If item = it.Escala Then
                                    MesmaOitava = 5
                                    P.EraSelecionado = True
                                End If
                            End If
                        Next
                        If MesmaOitava >= 5 Then
                            Continue For
                        Else
                            ListaEscalaPorTempo.Add(it.Escala)
                            If iTA >= 0 And iTA < ComprimentoAtual Then
                                Tabela(iTA, P.Escala).SetVisivel(True)
                                Tabela(iTA, P.Escala).Selecionado = False
                                Tabela(iTA, P.Escala).SetSelecionado(True)
                            End If
                            NotasSelecionadas.Add(P)
                        End If
                    End If
                End If
            End If
        Next
        PrimeiraPosicaoX = UltimaPosicaoX
        FeitoMovimento = True
        PilhaRefazer.Clear()
        FinalizaMovimento()
    End Sub

    Public Sub Excluir()
        ExcluirNotasSelecionadasPlayer()
        Dim NRemocoes As Integer = 0
        Dim Var As Integer = Math.Ceiling(ComprimentoAtual / 2)
        For Each it As NotaSelecionada In NotasSelecionadas
            Dim NP As New NoPilha
            If it.TempoAtual >= 0 And it.TempoAtual < ComprimentoAtual Then
                Tabela(it.TempoAtual, it.Escala).SetVisivel(False)
            End If
            NP.Escala = it.Escala
            NP.TempoInicio = ObjetoPlayer.Cursor - (Var - it.TempoOriginal)
            NP.Nseguintes = 1
            NP.ParaInserir = True
            PilhaDesfazer.Push(NP)
            NRemocoes += 1
            Ui.TemAlteracao = True
        Next
        If NRemocoes > 0 Then
            PilhaRefazer.Clear()
            PilhaDesfazer.Peek.Nseguintes = NRemocoes
        End If
        NotasSelecionadas.Clear()
        Atualizar()
    End Sub

End Class