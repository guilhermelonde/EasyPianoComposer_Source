'--- TocaNota ---
'Autor: Guilherme Pereira Porto Londe
'Última modificação: 14 de março de 2018

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

    Public Const TocaEmParalelo = 5
    'Valor que representa quantas notas podem ser tocadas em paralelo para cada
    'oitava. Quanto menor o valor de TocaEmParalelo mais memória é otimizada.

    Private Arquivos(5, TocaEmParalelo) As System.IO.MemoryStream
    'Aqui são carregados os arquivos de áudio. 
    'Assim está organizado: (Oitava | Slot) - Para cada oitava, nos seus 5 slots 
    'são carregados o mesmo audio para poder ser tocado 5 vezes em paralelo.
    Private SE(5, TocaEmParalelo) As SoundEffect
    'Player atribuido à cada arquivo de áudio

    Private SlotAtual(5) As Integer
    'Representa qual slot de BufferSecundario será substituído. Os valores dos 
    'slots iteram de forma circular com valores entre 0 e TocaEmParalelo-1.

    Private Volume As Double

    Public Sub New()
        Dim G As New Game
        Dim X As Content = New Content(G.Services)
        Volume = 1
        For i = 0 To 4
            SlotAtual(i) = 0
            For j = 0 To TocaEmParalelo - 1
                Dim myFileStream As System.IO.FileStream
                If System.IO.File.Exists("audio/" & (i + 1) & ".wav") = False Then
                    Throw New System.Exception("Missing file: audio/" & (i + 1) & ".wav ")
                End If
                myFileStream = New System.IO.FileStream("audio/" & (i + 1) & ".wav", System.IO.FileMode.Open, System.IO.FileAccess.Read)
                Arquivos(i, j) = New System.IO.MemoryStream
                Arquivos(i, j).SetLength(myFileStream.Length)
                myFileStream.Read(Arquivos(i, j).GetBuffer(), 0, myFileStream.Length)
                SE(i, j) = SoundEffect.FromStream(Arquivos(i, j))
                'Carrega 5 vezes o mesmo áudio na i-ésima oitava.
            Next j
        Next i
    End Sub

    Public Sub SetVolume(ByVal V As Integer)
        Volume = V * 0.01
    End Sub

    Public Sub Tocar(ByVal Oitava As Integer, ByVal Escala As Integer)
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

        SE(Oitava, SlotAtual(Oitava)).Play(Volume, Pitch, 0)

        SlotAtual(Oitava) = (SlotAtual(Oitava) + 1) Mod TocaEmParalelo
        'Iteração circular
    End Sub
End Class