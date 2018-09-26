<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigurarTela
    Inherits System.Windows.Forms.Form

    'Descartar substituições de formulário para limpar a lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Exigido pelo Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'OBSERVAÇÃO: o procedimento a seguir é exigido pelo Windows Form Designer
    'Pode ser modificado usando o Windows Form Designer.  
    'Não o modifique usando o editor de códigos.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.DiametroNota = New System.Windows.Forms.NumericUpDown()
        Me.EspacamentoNotas = New System.Windows.Forms.NumericUpDown()
        Me.LabelDiametro = New System.Windows.Forms.Label()
        Me.LabelEspacamento = New System.Windows.Forms.Label()
        Me.Confirmar = New System.Windows.Forms.Button()
        Me.Aplicar = New System.Windows.Forms.Button()
        Me.Cancelar = New System.Windows.Forms.Button()
        Me.Separador2 = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PadraoLimite = New System.Windows.Forms.Button()
        Me.Separador3 = New System.Windows.Forms.Panel()
        Me.PadraoCor = New System.Windows.Forms.Button()
        Me.LabelFundo = New System.Windows.Forms.Label()
        Me.LabelNota = New System.Windows.Forms.Label()
        Me.LabelDestacada = New System.Windows.Forms.Label()
        Me.LabelSelecionada = New System.Windows.Forms.Label()
        Me.Cor1 = New System.Windows.Forms.ComboBox()
        Me.Cor2 = New System.Windows.Forms.ComboBox()
        Me.Cor4 = New System.Windows.Forms.ComboBox()
        Me.Cor3 = New System.Windows.Forms.ComboBox()
        Me.LabelInicioFim = New System.Windows.Forms.Label()
        Me.LabelCores = New System.Windows.Forms.Label()
        Me.Cor5 = New System.Windows.Forms.ComboBox()
        Me.LabelCursor = New System.Windows.Forms.Label()
        Me.Cor6 = New System.Windows.Forms.ComboBox()
        Me.LabelSeparadorVertical = New System.Windows.Forms.Label()
        Me.Cor7 = New System.Windows.Forms.ComboBox()
        Me.LabelLimites = New System.Windows.Forms.Label()
        Me.Separador1 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        CType(Me.DiametroNota, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EspacamentoNotas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Separador2.SuspendLayout()
        Me.Separador1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DiametroNota
        '
        Me.DiametroNota.Cursor = System.Windows.Forms.Cursors.Hand
        Me.DiametroNota.DecimalPlaces = 1
        Me.DiametroNota.Increment = New Decimal(New Integer() {3, 0, 0, 65536})
        Me.DiametroNota.Location = New System.Drawing.Point(169, 57)
        Me.DiametroNota.Maximum = New Decimal(New Integer() {92, 0, 0, 65536})
        Me.DiametroNota.Minimum = New Decimal(New Integer() {56, 0, 0, 65536})
        Me.DiametroNota.Name = "DiametroNota"
        Me.DiametroNota.ReadOnly = True
        Me.DiametroNota.Size = New System.Drawing.Size(42, 20)
        Me.DiametroNota.TabIndex = 0
        Me.DiametroNota.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        'EspacamentoNotas
        '
        Me.EspacamentoNotas.Cursor = System.Windows.Forms.Cursors.Hand
        Me.EspacamentoNotas.DecimalPlaces = 1
        Me.EspacamentoNotas.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.EspacamentoNotas.Location = New System.Drawing.Point(169, 28)
        Me.EspacamentoNotas.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.EspacamentoNotas.Minimum = New Decimal(New Integer() {35, 0, 0, 65536})
        Me.EspacamentoNotas.Name = "EspacamentoNotas"
        Me.EspacamentoNotas.ReadOnly = True
        Me.EspacamentoNotas.Size = New System.Drawing.Size(42, 20)
        Me.EspacamentoNotas.TabIndex = 1
        Me.EspacamentoNotas.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'LabelDiametro
        '
        Me.LabelDiametro.AutoSize = True
        Me.LabelDiametro.Location = New System.Drawing.Point(22, 58)
        Me.LabelDiametro.Name = "LabelDiametro"
        Me.LabelDiametro.Size = New System.Drawing.Size(91, 13)
        Me.LabelDiametro.TabIndex = 2
        Me.LabelDiametro.Text = "Diâmetro da nota:"
        '
        'LabelEspacamento
        '
        Me.LabelEspacamento.AutoSize = True
        Me.LabelEspacamento.Location = New System.Drawing.Point(22, 30)
        Me.LabelEspacamento.Name = "LabelEspacamento"
        Me.LabelEspacamento.Size = New System.Drawing.Size(123, 13)
        Me.LabelEspacamento.TabIndex = 3
        Me.LabelEspacamento.Text = "Espaçamento horizontal:"
        '
        'Confirmar
        '
        Me.Confirmar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Confirmar.Location = New System.Drawing.Point(299, 256)
        Me.Confirmar.Name = "Confirmar"
        Me.Confirmar.Size = New System.Drawing.Size(53, 23)
        Me.Confirmar.TabIndex = 4
        Me.Confirmar.Text = "Ok"
        Me.Confirmar.UseVisualStyleBackColor = True
        '
        'Aplicar
        '
        Me.Aplicar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Aplicar.Location = New System.Drawing.Point(13, 256)
        Me.Aplicar.Name = "Aplicar"
        Me.Aplicar.Size = New System.Drawing.Size(75, 23)
        Me.Aplicar.TabIndex = 5
        Me.Aplicar.Text = "Aplicar"
        Me.Aplicar.UseVisualStyleBackColor = True
        '
        'Cancelar
        '
        Me.Cancelar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Cancelar.Location = New System.Drawing.Point(150, 256)
        Me.Cancelar.Name = "Cancelar"
        Me.Cancelar.Size = New System.Drawing.Size(75, 23)
        Me.Cancelar.TabIndex = 6
        Me.Cancelar.Text = "Cancelar"
        Me.Cancelar.UseVisualStyleBackColor = True
        '
        'Separador2
        '
        Me.Separador2.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Separador2.Controls.Add(Me.Panel1)
        Me.Separador2.Location = New System.Drawing.Point(8, 89)
        Me.Separador2.Name = "Separador2"
        Me.Separador2.Size = New System.Drawing.Size(350, 1)
        Me.Separador2.TabIndex = 8
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Panel1.Location = New System.Drawing.Point(0, -82)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(350, 1)
        Me.Panel1.TabIndex = 9
        '
        'PadraoLimite
        '
        Me.PadraoLimite.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PadraoLimite.Location = New System.Drawing.Point(289, 56)
        Me.PadraoLimite.Name = "PadraoLimite"
        Me.PadraoLimite.Size = New System.Drawing.Size(64, 23)
        Me.PadraoLimite.TabIndex = 9
        Me.PadraoLimite.Text = "Padrão"
        Me.PadraoLimite.UseVisualStyleBackColor = True
        '
        'Separador3
        '
        Me.Separador3.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Separador3.Location = New System.Drawing.Point(8, 227)
        Me.Separador3.Name = "Separador3"
        Me.Separador3.Size = New System.Drawing.Size(350, 1)
        Me.Separador3.TabIndex = 9
        '
        'PadraoCor
        '
        Me.PadraoCor.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PadraoCor.Location = New System.Drawing.Point(289, 194)
        Me.PadraoCor.Name = "PadraoCor"
        Me.PadraoCor.Size = New System.Drawing.Size(63, 23)
        Me.PadraoCor.TabIndex = 10
        Me.PadraoCor.Text = "Padrão"
        Me.PadraoCor.UseVisualStyleBackColor = True
        '
        'LabelFundo
        '
        Me.LabelFundo.AutoSize = True
        Me.LabelFundo.Location = New System.Drawing.Point(22, 113)
        Me.LabelFundo.Name = "LabelFundo"
        Me.LabelFundo.Size = New System.Drawing.Size(40, 13)
        Me.LabelFundo.TabIndex = 11
        Me.LabelFundo.Text = "Fundo:"
        '
        'LabelNota
        '
        Me.LabelNota.AutoSize = True
        Me.LabelNota.Location = New System.Drawing.Point(22, 141)
        Me.LabelNota.Name = "LabelNota"
        Me.LabelNota.Size = New System.Drawing.Size(33, 13)
        Me.LabelNota.TabIndex = 12
        Me.LabelNota.Text = "Nota:"
        '
        'LabelDestacada
        '
        Me.LabelDestacada.AutoSize = True
        Me.LabelDestacada.Location = New System.Drawing.Point(199, 114)
        Me.LabelDestacada.Name = "LabelDestacada"
        Me.LabelDestacada.Size = New System.Drawing.Size(86, 13)
        Me.LabelDestacada.TabIndex = 13
        Me.LabelDestacada.Text = "Nota destacada:"
        '
        'LabelSelecionada
        '
        Me.LabelSelecionada.AutoSize = True
        Me.LabelSelecionada.Location = New System.Drawing.Point(199, 141)
        Me.LabelSelecionada.Name = "LabelSelecionada"
        Me.LabelSelecionada.Size = New System.Drawing.Size(93, 13)
        Me.LabelSelecionada.TabIndex = 14
        Me.LabelSelecionada.Text = "Nota selecionada:"
        '
        'Cor1
        '
        Me.Cor1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Cor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.Cor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cor1.FormattingEnabled = True
        Me.Cor1.Location = New System.Drawing.Point(146, 110)
        Me.Cor1.Name = "Cor1"
        Me.Cor1.Size = New System.Drawing.Size(42, 21)
        Me.Cor1.TabIndex = 15
        '
        'Cor2
        '
        Me.Cor2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Cor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.Cor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cor2.FormattingEnabled = True
        Me.Cor2.Location = New System.Drawing.Point(146, 138)
        Me.Cor2.Name = "Cor2"
        Me.Cor2.Size = New System.Drawing.Size(42, 21)
        Me.Cor2.TabIndex = 16
        '
        'Cor4
        '
        Me.Cor4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Cor4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.Cor4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cor4.FormattingEnabled = True
        Me.Cor4.Location = New System.Drawing.Point(309, 111)
        Me.Cor4.Name = "Cor4"
        Me.Cor4.Size = New System.Drawing.Size(42, 21)
        Me.Cor4.TabIndex = 17
        '
        'Cor3
        '
        Me.Cor3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Cor3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.Cor3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cor3.FormattingEnabled = True
        Me.Cor3.Location = New System.Drawing.Point(309, 138)
        Me.Cor3.Name = "Cor3"
        Me.Cor3.Size = New System.Drawing.Size(42, 21)
        Me.Cor3.TabIndex = 18
        '
        'LabelInicioFim
        '
        Me.LabelInicioFim.AutoSize = True
        Me.LabelInicioFim.Location = New System.Drawing.Point(22, 169)
        Me.LabelInicioFim.Name = "LabelInicioFim"
        Me.LabelInicioFim.Size = New System.Drawing.Size(94, 13)
        Me.LabelInicioFim.TabIndex = 20
        Me.LabelInicioFim.Text = "Marca início e fim:"
        '
        'LabelCores
        '
        Me.LabelCores.AutoSize = True
        Me.LabelCores.Location = New System.Drawing.Point(9, 94)
        Me.LabelCores.Name = "LabelCores"
        Me.LabelCores.Size = New System.Drawing.Size(37, 13)
        Me.LabelCores.TabIndex = 21
        Me.LabelCores.Text = "Cores:"
        '
        'Cor5
        '
        Me.Cor5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Cor5.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.Cor5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cor5.FormattingEnabled = True
        Me.Cor5.Location = New System.Drawing.Point(146, 166)
        Me.Cor5.Name = "Cor5"
        Me.Cor5.Size = New System.Drawing.Size(42, 21)
        Me.Cor5.TabIndex = 22
        '
        'LabelCursor
        '
        Me.LabelCursor.AutoSize = True
        Me.LabelCursor.Location = New System.Drawing.Point(199, 169)
        Me.LabelCursor.Name = "LabelCursor"
        Me.LabelCursor.Size = New System.Drawing.Size(87, 13)
        Me.LabelCursor.TabIndex = 23
        Me.LabelCursor.Text = "Marca do cursor:"
        '
        'Cor6
        '
        Me.Cor6.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Cor6.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.Cor6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cor6.FormattingEnabled = True
        Me.Cor6.Location = New System.Drawing.Point(309, 166)
        Me.Cor6.Name = "Cor6"
        Me.Cor6.Size = New System.Drawing.Size(42, 21)
        Me.Cor6.TabIndex = 24
        '
        'LabelSeparadorVertical
        '
        Me.LabelSeparadorVertical.AutoSize = True
        Me.LabelSeparadorVertical.Location = New System.Drawing.Point(22, 199)
        Me.LabelSeparadorVertical.Name = "LabelSeparadorVertical"
        Me.LabelSeparadorVertical.Size = New System.Drawing.Size(96, 13)
        Me.LabelSeparadorVertical.TabIndex = 25
        Me.LabelSeparadorVertical.Text = "Separador vertical:"
        '
        'Cor7
        '
        Me.Cor7.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Cor7.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.Cor7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cor7.FormattingEnabled = True
        Me.Cor7.Location = New System.Drawing.Point(146, 195)
        Me.Cor7.Name = "Cor7"
        Me.Cor7.Size = New System.Drawing.Size(42, 21)
        Me.Cor7.TabIndex = 26
        '
        'LabelLimites
        '
        Me.LabelLimites.AutoSize = True
        Me.LabelLimites.Location = New System.Drawing.Point(10, 11)
        Me.LabelLimites.Name = "LabelLimites"
        Me.LabelLimites.Size = New System.Drawing.Size(42, 13)
        Me.LabelLimites.TabIndex = 27
        Me.LabelLimites.Text = "Limites:"
        '
        'Separador1
        '
        Me.Separador1.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Separador1.Controls.Add(Me.Panel3)
        Me.Separador1.Location = New System.Drawing.Point(8, 7)
        Me.Separador1.Name = "Separador1"
        Me.Separador1.Size = New System.Drawing.Size(350, 1)
        Me.Separador1.TabIndex = 10
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Panel3.Location = New System.Drawing.Point(0, -82)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(350, 1)
        Me.Panel3.TabIndex = 9
        '
        'ConfigurarTela
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(364, 292)
        Me.Controls.Add(Me.Separador1)
        Me.Controls.Add(Me.LabelLimites)
        Me.Controls.Add(Me.Cor7)
        Me.Controls.Add(Me.LabelSeparadorVertical)
        Me.Controls.Add(Me.Cor6)
        Me.Controls.Add(Me.LabelCursor)
        Me.Controls.Add(Me.Cor5)
        Me.Controls.Add(Me.LabelCores)
        Me.Controls.Add(Me.LabelInicioFim)
        Me.Controls.Add(Me.Cor3)
        Me.Controls.Add(Me.Cor4)
        Me.Controls.Add(Me.Cor2)
        Me.Controls.Add(Me.Cor1)
        Me.Controls.Add(Me.LabelSelecionada)
        Me.Controls.Add(Me.LabelDestacada)
        Me.Controls.Add(Me.LabelNota)
        Me.Controls.Add(Me.LabelFundo)
        Me.Controls.Add(Me.PadraoCor)
        Me.Controls.Add(Me.Separador3)
        Me.Controls.Add(Me.PadraoLimite)
        Me.Controls.Add(Me.Separador2)
        Me.Controls.Add(Me.Cancelar)
        Me.Controls.Add(Me.Aplicar)
        Me.Controls.Add(Me.Confirmar)
        Me.Controls.Add(Me.LabelEspacamento)
        Me.Controls.Add(Me.LabelDiametro)
        Me.Controls.Add(Me.EspacamentoNotas)
        Me.Controls.Add(Me.DiametroNota)
        Me.MaximumSize = New System.Drawing.Size(380, 330)
        Me.MinimumSize = New System.Drawing.Size(380, 330)
        Me.Name = "ConfigurarTela"
        Me.Text = "ConfigurarTela"
        CType(Me.DiametroNota, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EspacamentoNotas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Separador2.ResumeLayout(False)
        Me.Separador1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DiametroNota As Forms.NumericUpDown
    Friend WithEvents EspacamentoNotas As Forms.NumericUpDown
    Friend WithEvents LabelDiametro As Forms.Label
    Friend WithEvents LabelEspacamento As Forms.Label
    Friend WithEvents Confirmar As Forms.Button
    Friend WithEvents Aplicar As Forms.Button
    Friend WithEvents Cancelar As Forms.Button
    Friend WithEvents Separador2 As Forms.Panel
    Friend WithEvents PadraoLimite As Forms.Button
    Friend WithEvents Separador3 As Forms.Panel
    Friend WithEvents PadraoCor As Forms.Button
    Friend WithEvents LabelFundo As Forms.Label
    Friend WithEvents LabelNota As Forms.Label
    Friend WithEvents LabelDestacada As Forms.Label
    Friend WithEvents LabelSelecionada As Forms.Label
    Friend WithEvents Cor1 As Forms.ComboBox
    Friend WithEvents Cor2 As Forms.ComboBox
    Friend WithEvents Cor4 As Forms.ComboBox
    Friend WithEvents Cor3 As Forms.ComboBox
    Friend WithEvents LabelInicioFim As Forms.Label
    Friend WithEvents LabelCores As Forms.Label
    Friend WithEvents Cor5 As Forms.ComboBox
    Friend WithEvents LabelCursor As Forms.Label
    Friend WithEvents Cor6 As Forms.ComboBox
    Friend WithEvents LabelSeparadorVertical As Forms.Label
    Friend WithEvents Cor7 As Forms.ComboBox
    Friend WithEvents Panel1 As Forms.Panel
    Friend WithEvents LabelLimites As Forms.Label
    Friend WithEvents Separador1 As Forms.Panel
    Friend WithEvents Panel3 As Forms.Panel
End Class
