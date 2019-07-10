'--- ConfigurarTela ---
'Autor: Guilherme Pereira Porto Londe
'última modificação: 10 de julho de 2019

Imports Color = System.Drawing.Color

Public Class DadosUsuario
    Public Cores As New List(Of Color)
    Public Volume, IdIdioma As Integer
    Public EspacamentoNotas, DiametroNotas As Double
    Public Acordes, Grade, Apresentacao, Menu, Ativacao, AtivacaoE As Boolean
    Public DiretorioAudio As String

    Private Function ParseProprio(ByVal str As String)
        Dim ret, av, p, i As Integer
        av = 0
        ret = 0
        i = 0
        p = 0
        While i < str.Count AndAlso ((str.Chars(i) >= "0" And str.Chars(i) <= "9") Or str.Chars(i) = ",")
            If str.Chars(i) = "," Then
                p = 1
            Else
                ret = ret * 10 + Convert.ToInt32(str.Chars(i)) - 48
                av += p
            End If
            i += 1
        End While
        Return ret / Math.Pow(10, av)
    End Function

    Public Sub New()
        IdIdioma = 1
        Volume = 100
        EspacamentoNotas = 5
        DiametroNotas = 5.6
        Acordes = 0
        Grade = 1
        Apresentacao = 0
        Menu = 1
        Ativacao = 1
        AtivacaoE = 1
        DiretorioAudio = "default"
        Cores.Add(Color.FromArgb(149, 154, 143))
        Cores.Add(Color.Beige)
        Cores.Add(Color.MediumBlue)
        Cores.Add(Color.Red)
        Cores.Add(Color.DimGray)
        Cores.Add(Color.Gray)
        Cores.Add(Color.Black)

        If System.IO.File.Exists("userdata.inf") Then
            Dim text = System.IO.File.ReadAllText("userdata.inf")
            If text.Contains("AudioDir = ") Then
                Dim pos = text.IndexOf("AudioDir = ")
                Dim posApEsq = text.IndexOf("'", pos)
                Dim posApDir = text.IndexOf("'", posApEsq + 1)
                If posApEsq <> -1 AndAlso posApDir <> -1 Then
                    DiretorioAudio = text.Substring(posApEsq + 1, posApDir - posApEsq - 1)
                End If
            End If
            If text.Contains("IdiomId = ") Then
                Dim novoIdioma As Double
                novoIdioma = ParseProprio(text.Substring(text.IndexOf("IdiomId = ") + 10))
                If novoIdioma >= 0 And novoIdioma < Idioma.ContagemIdiomas Then
                    IdIdioma = novoIdioma
                End If
            End If
            If text.Contains("Volume = ") Then
                Dim novoVolume As Double
                novoVolume = ParseProprio(text.Substring(text.IndexOf("Volume = ") + 9))
                If novoVolume >= 0 And novoVolume <= 100 Then
                    Volume = novoVolume
                End If
            End If
            If text.Contains("hSpacing = ") Then
                Dim novoEspacamento As Double
                novoEspacamento = ParseProprio(text.Substring(text.IndexOf("hSpacing = ") + 11))
                novoEspacamento = Math.Floor(novoEspacamento * 10)
                If novoEspacamento >= 50 And novoEspacamento <= 100 Then
                    EspacamentoNotas = novoEspacamento / 10
                End If
            End If
            If text.Contains("nDiameter = ") Then
                Dim novoDiametro As Double
                novoDiametro = ParseProprio(text.Substring(text.IndexOf("nDiameter = ") + 12))
                novoDiametro = Math.Floor(novoDiametro * 10)
                If novoDiametro >= 56 And novoDiametro <= 92 And (Int(novoDiametro + 1) Mod 3) = 0 Then
                    DiametroNotas = novoDiametro / 10
                End If
            End If
            If text.Contains("Chords = ") Then
                Dim novoAcorde As Integer
                novoAcorde = ParseProprio(text.Substring(text.IndexOf("Chords = ") + 9))
                If novoAcorde >= 0 And novoAcorde <= 1 Then
                    Acordes = novoAcorde
                End If
            End If
            If text.Contains("Grid = ") Then
                Dim novoGrade As Integer
                novoGrade = ParseProprio(text.Substring(text.IndexOf("Grid = ") + 7))
                If novoGrade >= 0 And novoGrade <= 1 Then
                    Grade = novoGrade
                End If
            End If
            If text.Contains("Presentation = ") Then
                Dim novoApresentacao As Integer
                novoApresentacao = ParseProprio(text.Substring(text.IndexOf("Presentation = ") + 15))
                If novoApresentacao >= 0 And novoApresentacao <= 1 Then
                    Apresentacao = novoApresentacao
                End If
            End If
            If text.Contains("Menu = ") Then
                Dim novoMenu As Integer
                novoMenu = ParseProprio(text.Substring(text.IndexOf("Menu = ") + 7))
                If novoMenu >= 0 And novoMenu <= 1 Then
                    Menu = novoMenu
                End If
            End If
            If text.Contains("nActivation = ") Then
                Dim novoAtivacao As Integer
                novoAtivacao = ParseProprio(text.Substring(text.IndexOf("nActivation = ") + 14))
                If novoAtivacao >= 0 And novoAtivacao <= 1 Then
                    Ativacao = novoAtivacao
                End If
            End If
            If text.Contains("sActivation = ") Then
                Dim novoAtivacaoE As Integer
                novoAtivacaoE = ParseProprio(text.Substring(text.IndexOf("sActivation = ") + 14))
                If novoAtivacaoE >= 0 And novoAtivacaoE <= 1 Then
                    AtivacaoE = novoAtivacaoE
                End If
            End If
            For i = 1 To 7
                Dim R, G, B As Integer
                R = -1
                G = -1
                B = -1
                If text.Contains("Color" & i & " = ") Then
                    Dim X As String = text.Substring(text.IndexOf("Color" & i & " = ") + 9)
                    R = ParseProprio(X)
                    If (X.Contains(";")) Then
                        X = X.Substring(X.IndexOf(";") + 1)
                        G = ParseProprio(X)
                        If (X.Contains(";")) Then
                            X = X.Substring(X.IndexOf(";") + 1)
                            B = ParseProprio(X)
                        End If
                    End If
                End If
                If R >= 0 And R <= 255 And G >= 0 And G <= 255 And B >= 0 And B <= 255 Then
                    Cores.Item(i - 1) = Color.FromArgb(R, G, B)
                End If
            Next
        End If
    End Sub

    Public Sub Flush()
        Dim Saida, SaidaCores As String
        SaidaCores = ""
        Saida = "IdiomId = " & IdIdioma & " " & vbCrLf & "Volume = " & Volume & " " & vbCrLf & "hSpacing = " & EspacamentoNotas & vbCrLf
        Saida = Saida & "nDiameter = " & DiametroNotas & " " & vbCrLf & "Chords = " & Int(Acordes) & vbCrLf & "Grid = " & Int(Grade) & vbCrLf
        Saida = Saida & "Presentation = " & Int(Apresentacao) & vbCrLf & "Menu = " & Int(Menu) & vbCrLf & "nActivation = " & Int(Ativacao) & vbCrLf
        Saida = Saida & "sActivation = " & Int(AtivacaoE) & vbCrLf & "AudioDir = '" & DiretorioAudio & "'" & vbCrLf
        For i = 1 To 7
            SaidaCores = SaidaCores & ("Color" & i & " = " & Cores.Item(i - 1).R & ";" & Cores.Item(i - 1).G & ";" & Cores.Item(i - 1).B & vbCrLf)
        Next
        System.IO.File.WriteAllText("userdata.inf", Saida & SaidaCores)
    End Sub
End Class
