<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Dialog2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TarePanel = New System.Windows.Forms.Panel()
        Me.TareOnly = New System.Windows.Forms.Label()
        Me.GrossPanel = New System.Windows.Forms.Panel()
        Me.GrossOnly = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.DonePanel = New System.Windows.Forms.Panel()
        Me.Net = New System.Windows.Forms.Label()
        Me.Tare = New System.Windows.Forms.Label()
        Me.Gross = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TarePanel.SuspendLayout()
        Me.GrossPanel.SuspendLayout()
        Me.DonePanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(212, 238)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(89, 60)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(455, 33)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Your light weight has been stored."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(99, 121)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(258, 33)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Come back loaded"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(127, 166)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(198, 33)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "To get a ticket"
        '
        'TarePanel
        '
        Me.TarePanel.Controls.Add(Me.TareOnly)
        Me.TarePanel.Controls.Add(Me.Label1)
        Me.TarePanel.Controls.Add(Me.Label3)
        Me.TarePanel.Controls.Add(Me.Label2)
        Me.TarePanel.Location = New System.Drawing.Point(29, 12)
        Me.TarePanel.Name = "TarePanel"
        Me.TarePanel.Size = New System.Drawing.Size(465, 220)
        Me.TarePanel.TabIndex = 4
        '
        'TareOnly
        '
        Me.TareOnly.AutoSize = True
        Me.TareOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TareOnly.Location = New System.Drawing.Point(178, 70)
        Me.TareOnly.Name = "TareOnly"
        Me.TareOnly.Size = New System.Drawing.Size(95, 33)
        Me.TareOnly.TabIndex = 6
        Me.TareOnly.Text = "00000"
        '
        'GrossPanel
        '
        Me.GrossPanel.Controls.Add(Me.GrossOnly)
        Me.GrossPanel.Controls.Add(Me.Label4)
        Me.GrossPanel.Controls.Add(Me.Label5)
        Me.GrossPanel.Controls.Add(Me.Label6)
        Me.GrossPanel.Location = New System.Drawing.Point(27, 9)
        Me.GrossPanel.Name = "GrossPanel"
        Me.GrossPanel.Size = New System.Drawing.Size(465, 220)
        Me.GrossPanel.TabIndex = 5
        '
        'GrossOnly
        '
        Me.GrossOnly.AutoSize = True
        Me.GrossOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrossOnly.Location = New System.Drawing.Point(178, 70)
        Me.GrossOnly.Name = "GrossOnly"
        Me.GrossOnly.Size = New System.Drawing.Size(95, 33)
        Me.GrossOnly.TabIndex = 4
        Me.GrossOnly.Text = "00000"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(473, 33)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Your gross weight has been stored."
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(127, 173)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(198, 33)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "To get a ticket"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(99, 131)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(251, 33)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Come back empty"
        '
        'DonePanel
        '
        Me.DonePanel.Controls.Add(Me.Net)
        Me.DonePanel.Controls.Add(Me.Tare)
        Me.DonePanel.Controls.Add(Me.Gross)
        Me.DonePanel.Controls.Add(Me.Label11)
        Me.DonePanel.Controls.Add(Me.Label10)
        Me.DonePanel.Controls.Add(Me.Label7)
        Me.DonePanel.Controls.Add(Me.Label8)
        Me.DonePanel.Controls.Add(Me.Label9)
        Me.DonePanel.Location = New System.Drawing.Point(32, 10)
        Me.DonePanel.Name = "DonePanel"
        Me.DonePanel.Size = New System.Drawing.Size(465, 220)
        Me.DonePanel.TabIndex = 5
        '
        'Net
        '
        Me.Net.AutoSize = True
        Me.Net.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Net.Location = New System.Drawing.Point(269, 187)
        Me.Net.Name = "Net"
        Me.Net.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Net.Size = New System.Drawing.Size(0, 33)
        Me.Net.TabIndex = 8
        '
        'Tare
        '
        Me.Tare.AutoSize = True
        Me.Tare.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Tare.Location = New System.Drawing.Point(269, 118)
        Me.Tare.Name = "Tare"
        Me.Tare.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Tare.Size = New System.Drawing.Size(0, 33)
        Me.Tare.TabIndex = 7
        '
        'Gross
        '
        Me.Gross.AutoSize = True
        Me.Gross.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Gross.Location = New System.Drawing.Point(269, 85)
        Me.Gross.Name = "Gross"
        Me.Gross.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Gross.Size = New System.Drawing.Size(0, 33)
        Me.Gross.TabIndex = 6
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(137, 176)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(60, 33)
        Me.Label11.TabIndex = 5
        Me.Label11.Text = "Net"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(137, 122)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(75, 33)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Tare"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(137, 3)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(179, 33)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "You're done!"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(137, 85)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(94, 33)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "Gross"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(77, 40)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(309, 33)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Please take your ticket"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        '
        'Dialog2
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(532, 315)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.GrossPanel)
        Me.Controls.Add(Me.DonePanel)
        Me.Controls.Add(Me.TarePanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Dialog2"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Dialog2"
        Me.TarePanel.ResumeLayout(False)
        Me.TarePanel.PerformLayout()
        Me.GrossPanel.ResumeLayout(False)
        Me.GrossPanel.PerformLayout()
        Me.DonePanel.ResumeLayout(False)
        Me.DonePanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TarePanel As System.Windows.Forms.Panel
    Friend WithEvents GrossPanel As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents DonePanel As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TareOnly As System.Windows.Forms.Label
    Friend WithEvents GrossOnly As System.Windows.Forms.Label
    Friend WithEvents Net As System.Windows.Forms.Label
    Friend WithEvents Tare As System.Windows.Forms.Label
    Friend WithEvents Gross As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
