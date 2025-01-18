<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PMOD_MAIN
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
        Me.ButtonOpenFolder = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ButtonOpenFolder
        '
        Me.ButtonOpenFolder.Location = New System.Drawing.Point(13, 13)
        Me.ButtonOpenFolder.Name = "ButtonOpenFolder"
        Me.ButtonOpenFolder.Size = New System.Drawing.Size(272, 23)
        Me.ButtonOpenFolder.TabIndex = 0
        Me.ButtonOpenFolder.Text = "Open mods/lua"
        Me.ButtonOpenFolder.UseVisualStyleBackColor = True
        '
        'PMOD_MAIN
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(297, 297)
        Me.Controls.Add(Me.ButtonOpenFolder)
        Me.Name = "PMOD_MAIN"
        Me.Text = "PatosMod: Main Window"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ButtonOpenFolder As Button
End Class
