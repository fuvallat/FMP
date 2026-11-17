<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.PlayerVideo = New System.Windows.Forms.Panel()
        Me.PlayerMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuOpenFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOpenFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOpenClipboard = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPlaylist = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuURLLabel = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuURL = New System.Windows.Forms.ToolStripTextBox()
        Me.MenuOpenURL = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuLanguage = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSubtitle = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuHideControls = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMinimal = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuTopMost = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuScale = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuFullscreen = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuSendTo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuSuspend = New System.Windows.Forms.ToolStripMenuItem()
        Me.PlayerFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.PlayerTimer = New System.Windows.Forms.Timer(Me.components)
        Me.PlayerTrackBar = New System.Windows.Forms.ProgressBar()
        Me.PlayerVolumeBar = New System.Windows.Forms.ProgressBar()
        Me.PlayerControls = New System.Windows.Forms.Panel()
        Me.PlayerVolume = New FMP.OutlineLabel()
        Me.PlayerRemaining = New FMP.OutlineLabel()
        Me.PlayerDuration = New FMP.OutlineLabel()
        Me.PlayerPosition = New FMP.OutlineLabel()
        Me.PlayerFolderDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.ControlHideTimer = New System.Windows.Forms.Timer(Me.components)
        Me.PlayerBase = New System.Windows.Forms.Panel()
        Me.BaseOpen = New System.Windows.Forms.Button()
        Me.BasePath = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PlayerMenu.SuspendLayout()
        Me.PlayerControls.SuspendLayout()
        Me.PlayerBase.SuspendLayout()
        Me.SuspendLayout()
        '
        'PlayerVideo
        '
        Me.PlayerVideo.ContextMenuStrip = Me.PlayerMenu
        Me.PlayerVideo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PlayerVideo.Location = New System.Drawing.Point(0, 0)
        Me.PlayerVideo.Name = "PlayerVideo"
        Me.PlayerVideo.Size = New System.Drawing.Size(284, 161)
        Me.PlayerVideo.TabIndex = 0
        '
        'PlayerMenu
        '
        Me.PlayerMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuOpenFile, Me.MenuOpenFolder, Me.MenuOpenClipboard, Me.MenuPlaylist, Me.ToolStripSeparator1, Me.MenuURLLabel, Me.MenuURL, Me.MenuOpenURL, Me.ToolStripSeparator2, Me.MenuLanguage, Me.MenuSubtitle, Me.MenuInfo, Me.ToolStripSeparator3, Me.MenuHideControls, Me.MenuMinimal, Me.ToolStripSeparator4, Me.MenuTopMost, Me.MenuScale, Me.MenuFullscreen, Me.ToolStripSeparator5, Me.MenuSendTo, Me.ToolStripSeparator6, Me.MenuSuspend})
        Me.PlayerMenu.Name = "MenuVideo"
        Me.PlayerMenu.Size = New System.Drawing.Size(203, 417)
        '
        'MenuOpenFile
        '
        Me.MenuOpenFile.Name = "MenuOpenFile"
        Me.MenuOpenFile.Size = New System.Drawing.Size(202, 22)
        Me.MenuOpenFile.Text = "Fájl megnyitása"
        '
        'MenuOpenFolder
        '
        Me.MenuOpenFolder.Name = "MenuOpenFolder"
        Me.MenuOpenFolder.Size = New System.Drawing.Size(202, 22)
        Me.MenuOpenFolder.Text = "Mappa betöltése"
        '
        'MenuOpenClipboard
        '
        Me.MenuOpenClipboard.Name = "MenuOpenClipboard"
        Me.MenuOpenClipboard.Size = New System.Drawing.Size(202, 22)
        Me.MenuOpenClipboard.Text = "Megnyitás vágólapról"
        Me.MenuOpenClipboard.Visible = False
        '
        'MenuPlaylist
        '
        Me.MenuPlaylist.Name = "MenuPlaylist"
        Me.MenuPlaylist.Size = New System.Drawing.Size(202, 22)
        Me.MenuPlaylist.Text = "Lejátszólista"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(199, 6)
        '
        'MenuURLLabel
        '
        Me.MenuURLLabel.Enabled = False
        Me.MenuURLLabel.Name = "MenuURLLabel"
        Me.MenuURLLabel.Size = New System.Drawing.Size(202, 22)
        Me.MenuURLLabel.Text = "URL:"
        '
        'MenuURL
        '
        Me.MenuURL.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.MenuURL.Name = "MenuURL"
        Me.MenuURL.Size = New System.Drawing.Size(100, 23)
        '
        'MenuOpenURL
        '
        Me.MenuOpenURL.Name = "MenuOpenURL"
        Me.MenuOpenURL.Size = New System.Drawing.Size(202, 22)
        Me.MenuOpenURL.Text = "Megnyitás"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(199, 6)
        '
        'MenuLanguage
        '
        Me.MenuLanguage.Enabled = False
        Me.MenuLanguage.Name = "MenuLanguage"
        Me.MenuLanguage.Size = New System.Drawing.Size(202, 22)
        Me.MenuLanguage.Text = "Hangsáv"
        '
        'MenuSubtitle
        '
        Me.MenuSubtitle.Enabled = False
        Me.MenuSubtitle.Name = "MenuSubtitle"
        Me.MenuSubtitle.Size = New System.Drawing.Size(202, 22)
        Me.MenuSubtitle.Text = "Felirat"
        '
        'MenuInfo
        '
        Me.MenuInfo.Enabled = False
        Me.MenuInfo.Name = "MenuInfo"
        Me.MenuInfo.Size = New System.Drawing.Size(202, 22)
        Me.MenuInfo.Text = "Információk"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(199, 6)
        '
        'MenuHideControls
        '
        Me.MenuHideControls.Enabled = False
        Me.MenuHideControls.Name = "MenuHideControls"
        Me.MenuHideControls.Size = New System.Drawing.Size(202, 22)
        Me.MenuHideControls.Text = "Vezérlők elrejtése"
        Me.MenuHideControls.Visible = False
        '
        'MenuMinimal
        '
        Me.MenuMinimal.Name = "MenuMinimal"
        Me.MenuMinimal.Size = New System.Drawing.Size(202, 22)
        Me.MenuMinimal.Text = "Minimális felület"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(199, 6)
        Me.ToolStripSeparator4.Visible = False
        '
        'MenuTopMost
        '
        Me.MenuTopMost.Name = "MenuTopMost"
        Me.MenuTopMost.Size = New System.Drawing.Size(202, 22)
        Me.MenuTopMost.Text = "Mindig felül"
        '
        'MenuScale
        '
        Me.MenuScale.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripMenuItem2, Me.ToolStripMenuItem3, Me.ToolStripMenuItem4, Me.ToolStripMenuItem5})
        Me.MenuScale.Name = "MenuScale"
        Me.MenuScale.Size = New System.Drawing.Size(202, 22)
        Me.MenuScale.Text = "Méretezés"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(102, 22)
        Me.ToolStripMenuItem1.Tag = "0.25"
        Me.ToolStripMenuItem1.Text = "25%"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(102, 22)
        Me.ToolStripMenuItem2.Tag = "0.5"
        Me.ToolStripMenuItem2.Text = "50%"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(102, 22)
        Me.ToolStripMenuItem3.Tag = "1"
        Me.ToolStripMenuItem3.Text = "100%"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(102, 22)
        Me.ToolStripMenuItem4.Tag = "1.5"
        Me.ToolStripMenuItem4.Text = "150%"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(102, 22)
        Me.ToolStripMenuItem5.Tag = "2.0"
        Me.ToolStripMenuItem5.Text = "200%"
        '
        'MenuFullscreen
        '
        Me.MenuFullscreen.Enabled = False
        Me.MenuFullscreen.Name = "MenuFullscreen"
        Me.MenuFullscreen.Size = New System.Drawing.Size(202, 22)
        Me.MenuFullscreen.Text = "Teljesképernyő"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(199, 6)
        '
        'MenuSendTo
        '
        Me.MenuSendTo.Name = "MenuSendTo"
        Me.MenuSendTo.Size = New System.Drawing.Size(202, 22)
        Me.MenuSendTo.Text = "Küldés menü"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(199, 6)
        '
        'MenuSuspend
        '
        Me.MenuSuspend.Name = "MenuSuspend"
        Me.MenuSuspend.Size = New System.Drawing.Size(202, 22)
        Me.MenuSuspend.Text = "Alvó mód a videó végén"
        '
        'PlayerFileDialog
        '
        Me.PlayerFileDialog.Multiselect = True
        '
        'PlayerTimer
        '
        Me.PlayerTimer.Interval = 500
        '
        'PlayerTrackBar
        '
        Me.PlayerTrackBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PlayerTrackBar.Location = New System.Drawing.Point(4, 4)
        Me.PlayerTrackBar.Name = "PlayerTrackBar"
        Me.PlayerTrackBar.Size = New System.Drawing.Size(276, 10)
        Me.PlayerTrackBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.PlayerTrackBar.TabIndex = 0
        '
        'PlayerVolumeBar
        '
        Me.PlayerVolumeBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.PlayerVolumeBar.Location = New System.Drawing.Point(92, 29)
        Me.PlayerVolumeBar.Maximum = 200
        Me.PlayerVolumeBar.Name = "PlayerVolumeBar"
        Me.PlayerVolumeBar.Size = New System.Drawing.Size(100, 10)
        Me.PlayerVolumeBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.PlayerVolumeBar.TabIndex = 2
        '
        'PlayerControls
        '
        Me.PlayerControls.BackColor = System.Drawing.Color.Transparent
        Me.PlayerControls.Controls.Add(Me.PlayerVolume)
        Me.PlayerControls.Controls.Add(Me.PlayerVolumeBar)
        Me.PlayerControls.Controls.Add(Me.PlayerRemaining)
        Me.PlayerControls.Controls.Add(Me.PlayerDuration)
        Me.PlayerControls.Controls.Add(Me.PlayerPosition)
        Me.PlayerControls.Controls.Add(Me.PlayerTrackBar)
        Me.PlayerControls.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PlayerControls.Location = New System.Drawing.Point(0, 118)
        Me.PlayerControls.Name = "PlayerControls"
        Me.PlayerControls.Padding = New System.Windows.Forms.Padding(4)
        Me.PlayerControls.Size = New System.Drawing.Size(284, 43)
        Me.PlayerControls.TabIndex = 4
        Me.PlayerControls.Visible = False
        '
        'PlayerVolume
        '
        Me.PlayerVolume.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.PlayerVolume.ForeColor = System.Drawing.Color.White
        Me.PlayerVolume.Location = New System.Drawing.Point(92, 14)
        Me.PlayerVolume.Name = "PlayerVolume"
        Me.PlayerVolume.OutlineColor = System.Drawing.Color.Gray
        Me.PlayerVolume.OutlineWidth = 2
        Me.PlayerVolume.Size = New System.Drawing.Size(100, 13)
        Me.PlayerVolume.TabIndex = 3
        Me.PlayerVolume.Text = "0%"
        Me.PlayerVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PlayerRemaining
        '
        Me.PlayerRemaining.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PlayerRemaining.ForeColor = System.Drawing.Color.White
        Me.PlayerRemaining.Location = New System.Drawing.Point(198, 18)
        Me.PlayerRemaining.Name = "PlayerRemaining"
        Me.PlayerRemaining.OutlineColor = System.Drawing.Color.Gray
        Me.PlayerRemaining.OutlineWidth = 2
        Me.PlayerRemaining.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.PlayerRemaining.Size = New System.Drawing.Size(80, 20)
        Me.PlayerRemaining.TabIndex = 4
        Me.PlayerRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PlayerDuration
        '
        Me.PlayerDuration.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PlayerDuration.ForeColor = System.Drawing.Color.White
        Me.PlayerDuration.Location = New System.Drawing.Point(198, 18)
        Me.PlayerDuration.Name = "PlayerDuration"
        Me.PlayerDuration.OutlineColor = System.Drawing.Color.Gray
        Me.PlayerDuration.OutlineWidth = 2
        Me.PlayerDuration.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.PlayerDuration.Size = New System.Drawing.Size(80, 20)
        Me.PlayerDuration.TabIndex = 1
        Me.PlayerDuration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PlayerPosition
        '
        Me.PlayerPosition.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PlayerPosition.ForeColor = System.Drawing.Color.White
        Me.PlayerPosition.Location = New System.Drawing.Point(4, 18)
        Me.PlayerPosition.Name = "PlayerPosition"
        Me.PlayerPosition.OutlineColor = System.Drawing.Color.Gray
        Me.PlayerPosition.OutlineWidth = 2
        Me.PlayerPosition.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.PlayerPosition.Size = New System.Drawing.Size(80, 20)
        Me.PlayerPosition.TabIndex = 0
        Me.PlayerPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PlayerFolderDialog
        '
        Me.PlayerFolderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer
        '
        'ControlHideTimer
        '
        Me.ControlHideTimer.Interval = 3000
        '
        'PlayerBase
        '
        Me.PlayerBase.ContextMenuStrip = Me.PlayerMenu
        Me.PlayerBase.Controls.Add(Me.BaseOpen)
        Me.PlayerBase.Controls.Add(Me.BasePath)
        Me.PlayerBase.Controls.Add(Me.Label1)
        Me.PlayerBase.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PlayerBase.Location = New System.Drawing.Point(0, 0)
        Me.PlayerBase.Name = "PlayerBase"
        Me.PlayerBase.Size = New System.Drawing.Size(284, 118)
        Me.PlayerBase.TabIndex = 5
        '
        'BaseOpen
        '
        Me.BaseOpen.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.BaseOpen.Location = New System.Drawing.Point(105, 97)
        Me.BaseOpen.Name = "BaseOpen"
        Me.BaseOpen.Size = New System.Drawing.Size(75, 23)
        Me.BaseOpen.TabIndex = 2
        Me.BaseOpen.Text = "Megnyitás"
        Me.BaseOpen.UseVisualStyleBackColor = True
        '
        'BasePath
        '
        Me.BasePath.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.BasePath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.BasePath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl
        Me.BasePath.Location = New System.Drawing.Point(14, 71)
        Me.BasePath.Name = "BasePath"
        Me.BasePath.Size = New System.Drawing.Size(257, 20)
        Me.BasePath.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label1.Location = New System.Drawing.Point(12, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(260, 58)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Megnyitáshoz add meg a videó címét, útvonalát, vagy nyiss meg egyet a gépről, ese" &
    "tleg húzd az ablakba."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 161)
        Me.Controls.Add(Me.PlayerBase)
        Me.Controls.Add(Me.PlayerControls)
        Me.Controls.Add(Me.PlayerVideo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(300, 200)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Fuvallat Media Player"
        Me.PlayerMenu.ResumeLayout(False)
        Me.PlayerMenu.PerformLayout()
        Me.PlayerControls.ResumeLayout(False)
        Me.PlayerBase.ResumeLayout(False)
        Me.PlayerBase.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PlayerVideo As Panel
    Friend WithEvents PlayerMenu As ContextMenuStrip
    Friend WithEvents MenuOpenFile As ToolStripMenuItem
    Friend WithEvents PlayerFileDialog As OpenFileDialog
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents MenuLanguage As ToolStripMenuItem
    Friend WithEvents MenuSubtitle As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents MenuInfo As ToolStripMenuItem
    Friend WithEvents PlayerTimer As Timer
    Friend WithEvents PlayerTrackBar As ProgressBar
    Friend WithEvents PlayerDuration As OutlineLabel
    Friend WithEvents PlayerPosition As OutlineLabel
    Friend WithEvents PlayerVolumeBar As ProgressBar
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents MenuHideControls As ToolStripMenuItem
    Friend WithEvents MenuMinimal As ToolStripMenuItem
    Friend WithEvents PlayerVolume As OutlineLabel
    Friend WithEvents MenuFullscreen As ToolStripMenuItem
    Friend WithEvents MenuTopMost As ToolStripMenuItem
    Friend WithEvents PlayerControls As Panel
    Friend WithEvents MenuURLLabel As ToolStripMenuItem
    Friend WithEvents MenuURL As ToolStripTextBox
    Friend WithEvents MenuOpenURL As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents MenuOpenFolder As ToolStripMenuItem
    Friend WithEvents MenuOpenClipboard As ToolStripMenuItem
    Friend WithEvents PlayerFolderDialog As FolderBrowserDialog
    Friend WithEvents MenuPlaylist As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents MenuSendTo As ToolStripMenuItem
    Friend WithEvents ControlHideTimer As Timer
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents MenuSuspend As ToolStripMenuItem
    Friend WithEvents PlayerBase As Panel
    Friend WithEvents BaseOpen As Button
    Friend WithEvents BasePath As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents PlayerRemaining As OutlineLabel
    Friend WithEvents MenuScale As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As ToolStripMenuItem
End Class
