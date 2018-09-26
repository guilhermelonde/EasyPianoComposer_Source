'--- Player ---
'Autores: Guilherme Pereira Porto Londe, Jorge Menezes dos Santos
'Última modificação: 26 de setembro de 2018

Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Windows.Forms

Public Class Player

    Public CompMaximoMelodia As Integer = 3000
    'Define o comprimento máximo da melodia. O valor 3000 representa que a melodia
    'pode durar até 5 minutos na velocidade 1x.

    Private TN As TocaNota = Nothing
    'Instanciar apenas este objeto do tipo TocaNota

    Private WithEvents Timer As New System.Windows.Threading.DispatcherTimer
    Private Intervalo As Integer
    Private ProximoFrame As Double
    Private TempoAnterior As Date
    Public Cursor As Integer
    Public MaiorTI As Integer
    Public Tocando As Boolean
    Private Notas(CompMaximoMelodia + 1) As List(Of Integer)
    Private SubPlayerPai As SubPlayer

    Public Sub New(ByRef SubPlayerPai As SubPlayer)
        Try
            TN = New TocaNota()
        Catch e As System.IO.FileNotFoundException
            MessageBox.Show("There is a problem with XNA framework. Try to install Microsoft XNA Framework Redistributable 4.0 or higher to fix the problem.", "Error!",
            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw e
        Catch e As Exception
            MessageBox.Show(e.ToString, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw e
        End Try
        Me.SubPlayerPai = SubPlayerPai
        Intervalo = 100

        For i = 0 To CompMaximoMelodia
            Notas(i) = New List(Of Integer)
        Next
        NovoArquivo()
    End Sub

    Public Sub TocarNota(ByVal Oitava As Integer, ByVal Escala As Integer)
        TN.Tocar(Oitava, Escala)
    End Sub

    Public Sub Tocar(ByVal Cursor As Integer)
        Me.Cursor = Cursor
        Timer.Interval = New TimeSpan(0, 0, 0, 0, Intervalo)
        ProximoFrame = Intervalo
        Timer.Start()
        TempoAnterior = Now
        SubPlayerPai.AtivaTecla()
        Tocando = True
    End Sub

    Public Sub Pausar()
        Timer.Stop()
        Tocando = False
    End Sub

    Public Sub Inserir(ByVal TempoInicio As Integer, ByVal Escala As Integer)
        Notas(TempoInicio).Add(Escala)
        If TempoInicio + 1 > MaiorTI Then
            MaiorTI = TempoInicio + 1
        End If
    End Sub

    Public Sub Remover(ByVal TempoInicio As Integer, ByVal Escala As Integer)
        Notas(TempoInicio).Remove(Escala)
        If TempoInicio + 1 = MaiorTI Then
            While MaiorTI > 1 And Notas(MaiorTI - 1).Count = 0
                MaiorTI -= 1
            End While
        End If
    End Sub

    Public Sub Mover(ByVal TempoInicio As Integer, ByVal Escala As Integer, ByVal TempoAdiante As Integer)
        If TempoInicio + TempoAdiante < CompMaximoMelodia AndAlso TempoInicio + TempoAdiante > -1 Then
            Inserir(TempoInicio + TempoAdiante, Escala)
        End If
        Remover(TempoInicio, Escala)
    End Sub

    Public Function PegaNotas(ByVal TempoInicio As Integer) As List(Of Integer)
        If TempoInicio >= CompMaximoMelodia Or TempoInicio < 0 Then
            Dim tmp As New List(Of Integer)
            Return tmp
        End If
        Return Notas(TempoInicio)
    End Function

    Public Sub SetVelocidade(ByVal Velocidade As Decimal)
        Intervalo = 100 / Velocidade
        Notas(CompMaximoMelodia).Item(0) = Velocidade * 100
    End Sub

    Public Sub SetVolume(ByVal V As Integer)
        TN.SetVolume(V)
    End Sub

    Public Sub Carregar(ByVal NomeArquivo As String, ByRef Velocidade As Double)
        Try
            Dim Arquivo As New FileStream(NomeArquivo, FileMode.Open)
            Dim Serializador As New BinaryFormatter
            Notas = DirectCast(Serializador.Deserialize(Arquivo), List(Of Integer)())
            Arquivo.Close()
            MaiorTI = 1
            For i = 0 To CompMaximoMelodia - 1
                If Notas(i).Count > 10 Then
                    Throw New System.Exception("")
                End If
                If Notas(i).Count > 0 Then
                    Notas(i).Sort()
                    MaiorTI = i + 1
                    Dim C() As Integer = {0, 0, 0, 0, 0, 0}
                    For j = 0 To Notas(i).Count - 1
                        If j > 0 AndAlso Notas(i).Item(j - 1) = Notas(i).Item(j) Then
                            Throw New System.Exception("")
                        End If
                        If Notas(i).Item(j) < 0 Or Notas(i).Item(j) > 60 Then
                            Throw New System.Exception("")
                        End If
                        C(Math.Floor(Notas(i).Item(j) / 12)) += 1
                        If C(Math.Floor(Notas(i).Item(j) / 12)) > 5 Then
                            Throw New System.Exception("")
                        End If
                    Next
                End If
            Next
            Velocidade = Math.Max(0.5, Notas(CompMaximoMelodia).Item(0) / 100)
            Velocidade = Math.Min(Velocidade, 2)
            If Notas(CompMaximoMelodia).Count <> 1 Then
                Throw New System.Exception("")
            End If
        Catch e As Exception
            For i = 0 To CompMaximoMelodia
                Notas(i) = New List(Of Integer)
            Next
            Throw e
        End Try
    End Sub

    Public Sub Salvar(ByVal NomeArquivo As String)
        Try
            Dim Arquivo As New FileStream(NomeArquivo, FileMode.Create)
            Dim Serializador As New BinaryFormatter
            Serializador.Serialize(Arquivo, Notas)
            Arquivo.Close()
        Catch e As Exception
            Throw e
        End Try
    End Sub

    Public Sub NovoArquivo()
        For i = 0 To CompMaximoMelodia
            Notas(i).Clear()
        Next
        Notas(CompMaximoMelodia).Add(10000 / Intervalo)
        Cursor = 0
        Tocando = False
        MaiorTI = 1
    End Sub

    Public Sub Timer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer.Tick
        Timer.Stop()
        If Cursor >= CompMaximoMelodia - 1 Then
            Tocando = False
            Return
        End If

        Dim Tmp As Integer
        For Each it As Integer In Notas(Cursor)
            Tmp = Math.Floor(it / 12) + 1
            If it = 60 Then
                Me.TN.Tocar(5, 13)
            Else
                Me.TN.Tocar(Tmp, (it Mod 12) + 1)
            End If
        Next

        Cursor += 1
        SubPlayerPai.ProximoTabela()
        SubPlayerPai.AtivaTecla()
        Dim TempoPassado As Integer = (Now - TempoAnterior).Milliseconds
        TempoAnterior = Now
        ProximoFrame = ProximoFrame + Intervalo - TempoPassado
        Timer.Interval = New TimeSpan(0, 0, 0, 0, Math.Max(ProximoFrame, 20))
        Timer.Start()
    End Sub

End Class
