<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FMPList
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
        Me.PlayerList = New System.Windows.Forms.TreeView()
        Me.PlayerToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'PlayerList
        '
        Me.PlayerList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PlayerList.FullRowSelect = True
        Me.PlayerList.Location = New System.Drawing.Point(0, 0)
        Me.PlayerList.Name = "PlayerList"
        Me.PlayerList.Size = New System.Drawing.Size(404, 161)
        Me.PlayerList.TabIndex = 0
        '
        'FMPList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(404, 161)
        Me.Controls.Add(Me.PlayerList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FMPList"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Lejátszólista"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PlayerList As TreeView
    Friend WithEvents PlayerToolTip As ToolTip
End Class
