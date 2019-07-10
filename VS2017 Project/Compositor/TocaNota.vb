'--- TocaNota ---
'Autor: Guilherme Pereira Porto Londe
'Última modificação: 10 de julho de 2019

Imports SoundEffect = Microsoft.Xna.Framework.Audio.SoundEffect
Imports Content = Microsoft.Xna.Framework.Content.ContentManager
Imports Game = Microsoft.Xna.Framework.Game
'É necessário instalar o Microsoft XNA Redistributable 4.0 ou superior para importar
'corretamente as bibliotecas

Public Class TocaNota
    'Classe que carrega um diretório com 5 arquivos de áudio no formato .wav e que 
    'executa qualquer um deles na frequência de notas musicais conhecidas.

    'No diretório de áudio contém 5 arquivos .wav, onde cada um deles representa 
    'uma nota média entre todas As notas de sua oitava.

    Private SE(5) As SoundEffect
    'Player atribuido à cada arquivo de áudio

    Private Volume As Double

    Public Sub New()
        Volume = 0.8
    End Sub

    Public Sub Carregar(ByVal Diretorio As String)
        Dim G As New Game
        Dim X As Content = New Content(G.Services)
        For i = 0 To 4
            SE(i) = Nothing
        Next
        For i = 0 To 4
            Dim myFileStream As System.IO.FileStream
            Dim DiretorioCompleto = "audio/" & Diretorio & "/" & (i + 1) & ".wav"
            If System.IO.File.Exists(DiretorioCompleto) = False Then
                For j = 0 To 4
                    SE(j) = Nothing
                Next
                Throw New System.Exception(DiretorioCompleto)
            End If
            myFileStream = New System.IO.FileStream(DiretorioCompleto, System.IO.FileMode.Open, System.IO.FileAccess.Read)
            Dim Arquivo = New System.IO.MemoryStream
            Arquivo.SetLength(myFileStream.Length)
            myFileStream.Read(Arquivo.GetBuffer(), 0, myFileStream.Length)
            SE(i) = SoundEffect.FromStream(Arquivo)
            'Carrega 5 vezes o mesmo áudio na i-ésima oitava.
        Next i
    End Sub

    Public Sub SetVolume(ByVal V As Integer)
        Volume = V * 0.008
    End Sub

    Public Sub Tocar(ByVal Oitava As Integer, ByVal Escala As Integer)
        If SE(0) Is Nothing Then
            Return
        End If
        'Oitava: aceita valor entre 1 - 5; Escala: aceita valor entre 1 - 12, 
        'exceto a Oitava 5 que aceita a Escala 13.
        Oitava = Oitava - 1
        If Oitava < 0 Or Oitava > 4 Then
            Return
        End If

        If Escala < 1 Or (Escala = 13 And Oitava <> 4) Or Escala > 13 Then
            Return
        End If

        Dim Pitch As Double = 0
        If Oitava = 5 Then
            Oitava -= 1
        End If
        Select Case Escala
            Case 1
                Pitch = -0.5
            Case 2
                Pitch = -0.415
            Case 3
                Pitch = -0.335
            Case 4
                Pitch = -0.25
            Case 5
                Pitch = -0.165
            Case 6
                Pitch = -0.085
            Case 7
                Pitch = 0
            Case 8
                Pitch = 0.085
            Case 9
                Pitch = 0.165
            Case 10
                Pitch = 0.25
            Case 11
                Pitch = 0.335
            Case 12
                Pitch = 0.415
            Case 13
                Pitch = 0.5
        End Select

        SE(Oitava).Play(Volume, Pitch, 0)

    End Sub
End Class