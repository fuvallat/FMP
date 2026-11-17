Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Public Class MainForm
    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function LoadLibrary(ByVal lpFileName As String) As IntPtr
    End Function

    Private Const WM_NCLBUTTONDOWN As Integer = &HA1
    Private Const HTCAPTION As Integer = 2

    <DllImport("user32.dll")>
    Private Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function ReleaseCapture() As Boolean
    End Function

    Private clickTimer As New Timer() With {.Interval = SystemInformation.DoubleClickTime}
    Private pendingClick As Boolean = False

    Private Sub LoadCodec(codec As String)
        Dim fullPath = Path.Combine(Application.StartupPath, "Codecs", codec)

        If File.Exists(fullPath) Then
            Dim result = LoadLibrary(fullPath)

            If result = IntPtr.Zero Then
                Dim err = Marshal.GetLastWin32Error()
                Console.WriteLine($"HIBA: {codec} betöltése sikertelen. Win32 error: {err}")
            Else
                Console.WriteLine($"Betöltve: {codec}")
            End If
        End If
    End Sub

    Public Sub LoadAllCodecs()
        For Each c As String In Directory.EnumerateFiles(Path.Combine(Application.StartupPath, "Codecs"))
            LoadCodec(c)
        Next

        For Each p As ProcessModule In Process.GetCurrentProcess().Modules
            Console.WriteLine(p.ModuleName)
        Next
    End Sub

    Private player As MpvPlayer

    Private Sub FMP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PlayerControls.Parent = Me
        PlayerControls.BackColor = Color.FromArgb(120, 0, 0, 0)
        PlayerControls.BringToFront()

        MenuSendTo.Checked = File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\Windows\SendTo", My.Application.Info.Title & ".lnk"))

        PlayerVideo.AllowDrop = True

        AddHandler clickTimer.Tick, AddressOf HandleSingleClick

        player = New MpvPlayer()
        AddHandler player.VideoParamsChanged, AddressOf OnVideoParamsChanged
        AddHandler player.FileLoaded, AddressOf OnFileLoaded
        AddHandler player.FileLoadFailed, AddressOf OnFileLoadFailed
        AddHandler player.PlaybackEnded, AddressOf OnPlaybackEnded

        Dim args = Environment.GetCommandLineArgs()
        If args.Length > 1 Then
            For i = 1 To args.Length - 1
                Console.WriteLine(args(i))
                OpenMedia(args(i))
            Next

            If playlist.Count > 0 Then
                PlayFromPlaylist(0)
            End If
        End If
    End Sub

    <DllImport("PowrProf.dll", SetLastError:=True)>
    Private Shared Function SetSuspendState(hibernate As Boolean, force As Boolean, disableWakeEvent As Boolean) As Boolean
    End Function
    Public Sub PutComputerToSleep()
        SetSuspendState(False, True, True)
    End Sub

    Private videoSized As Boolean = False
    Private Sub OnVideoParamsChanged()
        If videoSized Then Exit Sub
        videoSized = True

        Me.Invoke(Sub()
                      ResizeFormToVideo()

                      'Dim info = player.GetCleanMediaInfo()
                      'Dim vol = player.GetNumberProperty("volume")

                      'Debug.WriteLine("---- Video Info ----")
                      'Debug.WriteLine("File: " & info.Path)
                      'Debug.WriteLine("Duration: " & player.FormatTime(info.Duration))
                      'Debug.WriteLine("Resolution: " & info.Width & "x" & info.Height)
                      'Debug.WriteLine("Volume: " & vol & "%")
                      'Debug.WriteLine("--------------------")
                  End Sub)
    End Sub

    Private Sub OnFileLoaded()
        Me.Invoke(Sub()
                      ResizeFormToVideo()

                      If player.IsMediaLoaded Then SetupAfterLoad()
                  End Sub)
    End Sub

    Private Sub OnFileLoadFailed()
        Me.Invoke(Sub()
                      If currentIndex + 1 < playlist.Count Then
                          PlayFromPlaylist(currentIndex + 1)
                      Else
                          ResetPlayerUI()

                          If MenuSuspend.Checked Then
                              PutComputerToSleep()
                          End If
                      End If
                  End Sub)
        '        MessageBox.Show("A videó betöltése sikertelen.", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub OnPlaybackEnded()
        Me.Invoke(Sub()
                      If currentIndex + 1 < playlist.Count Then
                          PlayFromPlaylist(currentIndex + 1)
                      Else
                          ResetPlayerUI()

                          If MenuSuspend.Checked Then
                              PutComputerToSleep()
                          End If
                      End If
                  End Sub)
    End Sub

    Private Sub ResetPlayerUI()
        PlayerControls.Visible = False
        PlayerTrackBar.Value = 0
        PlayerPosition.Text = ""
        PlayerDuration.Text = ""
        Me.Text = My.Application.Info.Title
        Me.Size = New Size(300, 200)
        PlayerBase.Visible = True
        player.Stop()
    End Sub

    Public playlist As New List(Of String)
    Public currentIndex As Integer = -1
    Public Sub AddToPlaylist(path As String)
        playlist.Add(path)
    End Sub

    Public Event PlaylistChanged()
    Public Sub PlayFromPlaylist(index As Integer)
        If index < 0 OrElse index >= playlist.Count Then Exit Sub

        currentIndex = index
        videoSized = False

        player.OpenMedia(PlayerVideo.Handle, playlist(index))

        RaiseEvent PlaylistChanged()
    End Sub

    Public Sub PlayNext()
        If currentIndex + 1 < playlist.Count Then
            PlayFromPlaylist(currentIndex + 1)
        End If
    End Sub

    Public Sub PlayPrevious()
        If currentIndex - 1 >= 0 Then
            PlayFromPlaylist(currentIndex - 1)
        End If
    End Sub

    Public Sub ResetPlaylist()
        playlist.Clear()
        currentIndex = -1
    End Sub

    Public Sub OpenFolderAsPlaylist(folder As String)
        Dim files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)

        ResetPlaylist()

        For Each f In files
            playlist.Add(f)
        Next

        If playlist.Count > 0 Then
            PlayFromPlaylist(0)
        End If
    End Sub

    Public Sub OpenMedia(input As String)
        If String.IsNullOrWhiteSpace(input) Then Exit Sub

        If input.StartsWith("http://") OrElse input.StartsWith("https://") Then
            AddToPlaylist(input)
            Exit Sub
        End If

        If Directory.Exists(input) Then
            OpenFolderAsPlaylist(input)
            Exit Sub
        End If

        If File.Exists(input) Then
            AddToPlaylist(input)
            Exit Sub
        End If

        If Uri.IsWellFormedUriString(input, UriKind.Absolute) Then
            AddToPlaylist(input)
            Exit Sub
        End If

        MessageBox.Show("A megadott tartalom nem nyitható meg: " & input)
    End Sub

    Private Sub SetupAfterLoad()
        PlayerBase.Visible = False
        PlayerTimer.Enabled = True
        LoadAudioTrackMenu()
        LoadSubtitleMenu()
        Me.Text = player.GetMediaTitle()

        Dim mediaType = player.GetMediaType()

        Select Case mediaType
            Case "video"
                PlayerControls.Visible = True
            Case "audio"
                PlayerControls.Visible = True
            Case "image"
                PlayerControls.Visible = False
        End Select
    End Sub

    Private isVolumeDragging As Boolean = False
    Private Sub PlayerTimer_Tick(sender As Object, e As EventArgs) Handles PlayerTimer.Tick
        Dim dur = player.GetDuration()
        Dim pos = player.GetPosition()

        If dur > 0 Then
            PlayerTrackBar.Maximum = CInt(dur)
            If PlayerTrackBar.Value <> Math.Min(CInt(pos), PlayerTrackBar.Maximum) Then
                PlayerTrackBar.Value = Math.Min(CInt(pos), PlayerTrackBar.Maximum)
            End If

            PlayerPosition.Text = MpvPlayer.FormatTime(pos)
            PlayerDuration.Text = MpvPlayer.FormatTime(dur)
            PlayerRemaining.Text = MpvPlayer.FormatTime(dur - pos)

            If Not isVolumeDragging Then
                Dim vol = player.GetNumberProperty("volume")
                If vol >= 0 AndAlso vol <= PlayerVolumeBar.Maximum Then
                    PlayerVolumeBar.Value = CInt(vol)
                End If
                PlayerVolume.Text = PlayerVolumeBar.Value & "%"
            End If

            If Not MenuHideControls.Checked And Not MenuHideControls.Enabled Then
                PlayerControls.Visible = True
                MenuHideControls.Enabled = True
                MenuInfo.Enabled = True
                MenuLanguage.Enabled = MenuLanguage.HasDropDownItems
                MenuSubtitle.Enabled = MenuSubtitle.DropDownItems.Count > 1
                MenuFullscreen.Enabled = True
            End If
        End If
    End Sub

    Private Sub HandleSingleClick(sender As Object, e As EventArgs)
        clickTimer.Stop()

        If pendingClick Then
            pendingClick = False

            If isPaused Then
                player.Resume()
            Else
                player.Pause()
            End If

            isPaused = Not isPaused
        End If
    End Sub

    Private Sub PlayerMenu_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles PlayerMenu.ItemClicked
        Select Case e.ClickedItem.Name
            Case MenuOpenFile.Name
                PlayerMenu.Close()
                If PlayerFileDialog.ShowDialog() = DialogResult.OK Then
                    ResetPlaylist()
                    For Each f In PlayerFileDialog.FileNames
                        OpenMedia(f)
                    Next

                    If playlist.Count > 0 Then
                        PlayFromPlaylist(0)
                    End If
                End If
            Case MenuOpenFolder.Name
                PlayerMenu.Close()
                If PlayerFolderDialog.ShowDialog() = DialogResult.OK Then
                    ResetPlaylist()
                    OpenMedia(PlayerFolderDialog.SelectedPath)
                End If
            Case MenuOpenClipboard.Name
                PlayerMenu.Close()
                ResetPlaylist()
            Case MenuPlaylist.Name
                FMPList.Show()
            Case MenuOpenURL.Name
                PlayerMenu.Close()
                If MenuURL.Text.Length > 0 Then
                    ResetPlaylist()
                    OpenMedia(MenuURL.Text)
                End If

                If playlist.Count > 0 Then
                    PlayFromPlaylist(0)
                End If
            Case MenuHideControls.Name
                PlayerControls.Visible = Not PlayerControls.Visible
                MenuHideControls.Checked = PlayerControls.Visible
            Case MenuMinimal.Name
                If FormBorderStyle = FormBorderStyle.Sizable Then
                    FormBorderStyle = FormBorderStyle.None
                    MenuMinimal.Checked = True
                Else
                    FormBorderStyle = FormBorderStyle.Sizable
                    MenuMinimal.Checked = False
                End If
            Case MenuInfo.Name
                PlayerMenu.Close()

                Dim info = player.GetCleanMediaInfo()

                Dim sb As New StringBuilder()
                sb.AppendLine("Videó:")
                sb.AppendLine($"  {info.Width}x{info.Height} @ {info.FPS} FPS")
                sb.AppendLine($"  Képarány: {info.Ratio}")
                sb.AppendLine($"  Codec: {info.VideoCodec}")
                sb.AppendLine()
                sb.AppendLine("Hang:")
                sb.AppendLine($"  Codec: {info.AudioCodec}")
                sb.AppendLine($"  Csatornák: {info.AudioChannels}")
                sb.AppendLine($"  Ráta: {info.AudioSampleRate}")
                sb.AppendLine("  Sávok:")
                For Each a In info.AudioTracks
                    sb.AppendLine("    " & a)
                Next
                sb.AppendLine()
                sb.AppendLine("Feliratok:")
                For Each s In info.SubtitleTracks
                    sb.AppendLine("    " & s)
                Next
                sb.AppendLine()
                sb.AppendLine("Konténer:")
                sb.AppendLine($"  {info.Container.ToUpper()}")
                sb.AppendLine()
                sb.AppendLine("Idő:")
                sb.AppendLine($"  Hossz: {MpvPlayer.FormatTime(info.Duration)}")
                sb.AppendLine()
                sb.AppendLine("Fájl:")
                sb.AppendLine($"  Útvonal: {info.Path}")
                sb.AppendLine($"  Méret: {MpvPlayer.FormatSize(info.FileSize)}")

                MessageBox.Show(sb.ToString(), "Media Info")
            Case MenuTopMost.Name
                Me.TopMost = Not Me.TopMost
                MenuTopMost.Checked = Me.TopMost
            Case MenuFullscreen.Name
                ToggleFullscreen()
            Case MenuSendTo.Name
                If File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\Windows\SendTo", My.Application.Info.Title & ".lnk")) Then
                    RemoveSendToShortcut()
                    MenuSendTo.Checked = False
                Else
                    CreateSendToShortcut()
                    MenuSendTo.Checked = True
                End If
            Case MenuSuspend.Name
                MenuSuspend.Checked = Not MenuSuspend.Checked
        End Select
    End Sub

    Private Sub MenuLanguage_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuLanguage.DropDownItemClicked
        Dim id As Integer = CInt(e.ClickedItem.Tag)

        If id = -1 Then
            player.SendCommand({"set", "aid", "no"})
        Else
            player.SetAudioTrack(id)
        End If
    End Sub

    Private Sub MenuSubtitle_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuSubtitle.DropDownItemClicked
        Dim id As Integer = CInt(e.ClickedItem.Tag)

        If id = -1 Then
            player.DisableSubtitles()
        Else
            player.SetSubtitleTrack(id)
        End If
    End Sub

    Private isPaused As Boolean = False
    Private isFullscreen As Boolean = False
    Private normalBounds As Rectangle

    Private Sub ToggleFullscreen()
        If Not isFullscreen Then
            normalBounds = Me.Bounds
            Me.FormBorderStyle = FormBorderStyle.None
            Me.WindowState = FormWindowState.Maximized
        Else
            If Not MenuMinimal.Checked Then
                Me.FormBorderStyle = FormBorderStyle.Sizable
            End If
            Me.WindowState = FormWindowState.Normal
            Me.Bounds = normalBounds
        End If

        isFullscreen = Not isFullscreen
        MenuFullscreen.Checked = isFullscreen
    End Sub

    Private Sub LoadAudioTrackMenu()
        MenuLanguage.DropDownItems.Clear()

        Dim offItem As New ToolStripMenuItem("Hangsáv kikapcsolása") With {
            .Tag = -1
        }
        MenuLanguage.DropDownItems.Add(offItem)

        Dim tracks = player.GetAudioTracks()
        Dim current = player.GetCurrentAudioTrackId()

        For Each t In tracks
            Dim item As New ToolStripMenuItem(player.GetAudioTrackTitle(t.Id)) With {
                .Tag = t.Id,
                .Checked = (t.Id = current)
            }
            MenuLanguage.DropDownItems.Add(item)
        Next
        MenuLanguage.Enabled = True
    End Sub

    Private Sub LoadSubtitleMenu()
        MenuSubtitle.DropDownItems.Clear()

        Dim offItem As New ToolStripMenuItem("Felirat kikapcsolása") With {
            .Tag = -1
        }
        MenuSubtitle.DropDownItems.Add(offItem)

        Dim tracks = player.GetSubtitleTracks()
        Dim current = player.GetCurrentSubtitleTrackId()

        For Each t In tracks
            Dim item As New ToolStripMenuItem(player.GetSubtitleTrackTitle(t.Id)) With {
                .Tag = t.Id,
                .Checked = (t.Id = current)
            }
            MenuSubtitle.DropDownItems.Add(item)
        Next
        MenuSubtitle.Enabled = True
    End Sub

    Private Sub FMP_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Space
                If isPaused Then player.Resume() Else player.Pause()
                isPaused = Not isPaused
            Case Keys.Left
                player.Seek(-10)
            Case Keys.Right
                player.Seek(10)
            Case Keys.Up
                Dim current = player.GetNumberProperty("volume")
                Dim newVol = Math.Max(0, Math.Min(PlayerVolumeBar.Maximum, current + 5))
                player.SetVolume(newVol)
            Case Keys.Down
                Dim current = player.GetNumberProperty("volume")
                Dim newVol = Math.Max(0, Math.Min(PlayerVolumeBar.Maximum, current - 5))
                player.SetVolume(newVol)
            Case Keys.Escape
                ToggleFullscreen()
        End Select

        If player.IsMediaLoaded() Then
            ShowControls()
        End If
    End Sub

    Private Sub ResizeFormToVideo()
        Dim videoW = CInt(player.GetNumberProperty("width"))
        Dim videoH = CInt(player.GetNumberProperty("height"))

        If videoW <= 0 OrElse videoH <= 0 Then Exit Sub

        Dim borderW = Me.Width - Me.ClientSize.Width
        Dim borderH = Me.Height - Me.ClientSize.Height
        Dim trackH = IIf(PlayerTrackBar.Visible, PlayerTrackBar.Height, 0)

        Me.Width = videoW + borderW
        Me.Height = videoH + borderH ' + trackH
    End Sub

    Private Sub ResizeVideo()
        Dim videoW = player.GetNumberProperty("width")
        Dim videoH = player.GetNumberProperty("height")

        If videoW <= 0 OrElse videoH <= 0 Then Exit Sub

        Dim panelW = PlayerVideo.ClientSize.Width
        Dim panelH = PlayerVideo.ClientSize.Height

        If panelW <= 0 OrElse panelH <= 0 Then Exit Sub

        Dim videoAR = videoW / videoH
        Dim panelAR = panelW / panelH

        Dim newW As Integer
        Dim newH As Integer

        If panelAR > videoAR Then
            newH = panelH
            newW = CInt(newH * videoAR)
        Else
            newW = panelW
            newH = CInt(newW / videoAR)
        End If

        Dim x = (panelW - newW) \ 2
        Dim y = (panelH - newH) \ 2

        Me.SetBounds(x, y, newW, newH)
    End Sub

    Private Sub PlayerTrackBar_MouseDown(sender As Object, e As MouseEventArgs) Handles PlayerTrackBar.MouseDown
        Dim tb = DirectCast(sender, ProgressBar)
        Dim ratio As Double = e.X / tb.Width
        Dim newValue As Integer = CInt(tb.Minimum + (tb.Maximum - tb.Minimum) * ratio)
        tb.Value = Math.Max(tb.Minimum, Math.Min(tb.Maximum, newValue))
        player.SeekAbsolute(tb.Value)
    End Sub

    Private Sub PlayerVideo_DragEnter(sender As Object, e As DragEventArgs) Handles PlayerVideo.DragEnter, PlayerBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) OrElse
       e.Data.GetDataPresent(DataFormats.Text) Then

            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub PlayerVideo_DragDrop(sender As Object, e As DragEventArgs) Handles PlayerVideo.DragDrop, PlayerBase.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            ResetPlaylist()
            Dim paths = CType(e.Data.GetData(DataFormats.FileDrop), String())
            For Each p In paths
                OpenMedia(p)
            Next

            If playlist.Count > 0 Then
                PlayFromPlaylist(0)
            End If
            Exit Sub
        End If

        If e.Data.GetDataPresent(DataFormats.Text) Then
            ResetPlaylist()
            Dim text = CStr(e.Data.GetData(DataFormats.Text)).Trim()
            OpenMedia(text)

            If playlist.Count > 0 Then
                PlayFromPlaylist(0)
            End If
            Exit Sub
        End If
    End Sub

    Private Sub PlayerVideo_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles PlayerVideo.MouseDoubleClick
        pendingClick = False
        clickTimer.Stop()
        ToggleFullscreen()
    End Sub

    Private Sub PlayerVideo_MouseClick(sender As Object, e As MouseEventArgs) Handles PlayerVideo.MouseClick
        If e.Button = MouseButtons.Left Then
            pendingClick = True
            clickTimer.Start()
        End If
    End Sub

    Private isDragging As Boolean = False
    Private dragStart As Point

    Private Sub PlayerVideo_MouseDown(sender As Object, e As MouseEventArgs) Handles PlayerVideo.MouseDown, PlayerBase.MouseDown
        If e.Button = MouseButtons.Left Then
            If e.Clicks = 2 Then
                Return
            End If

            isDragging = True
            dragStart = e.Location
        End If
    End Sub

    Private Sub PlayerVideo_MouseMove(sender As Object, e As MouseEventArgs) Handles PlayerVideo.MouseMove, PlayerBase.MouseMove
        If isDragging AndAlso e.Button = MouseButtons.Left Then
            Dim dx = Math.Abs(e.X - dragStart.X)
            Dim dy = Math.Abs(e.Y - dragStart.Y)

            If dx > 5 OrElse dy > 5 Then
                isDragging = False
                ReleaseCapture()
                SendMessage(Me.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0)
            End If
        End If

        If player.IsMediaLoaded Then
            If e.Location <> lastMousePos Then
                lastMousePos = e.Location
                If Not controlsVisible Then ShowControls()
                ControlHideTimer.Stop()
                ControlHideTimer.Start()
            End If
        End If
    End Sub

    Private Sub PlayerVideo_MouseUp(sender As Object, e As MouseEventArgs) Handles PlayerVideo.MouseUp, PlayerBase.MouseUp
        isDragging = False
    End Sub

    Private Sub PlayerVideo_MouseWheel(sender As Object, e As MouseEventArgs) Handles PlayerVideo.MouseWheel
        If e.Delta > 0 Then
            Dim current = player.GetNumberProperty("volume")
            Dim newVol = Math.Max(0, Math.Min(PlayerVolumeBar.Maximum, current + 5))
            player.SetVolume(newVol)
        Else
            Dim current = player.GetNumberProperty("volume")
            Dim newVol = Math.Max(0, Math.Min(PlayerVolumeBar.Maximum, current - 5))
            player.SetVolume(newVol)
        End If
    End Sub

    Private Sub PlayerVideo_MouseHover(sender As Object, e As EventArgs) Handles PlayerVideo.MouseHover
        PlayerVideo.Focus()
    End Sub

    Private Sub SetVolumeFromClick(mouseX As Integer)
        Dim pb = PlayerVolumeBar
        Dim ratio = mouseX / pb.Width
        Dim vol = CInt(Math.Max(0, Math.Min(PlayerVolumeBar.Maximum, ratio * PlayerVolumeBar.Maximum)))

        player.SetVolume(vol)
        pb.Value = vol
        PlayerVolume.Text = PlayerVolumeBar.Value & "%"
    End Sub

    Private Sub PlayerVolumeBar_MouseDown(sender As Object, e As MouseEventArgs) Handles PlayerVolumeBar.MouseDown
        isVolumeDragging = True
        SetVolumeFromClick(e.X)
    End Sub

    Private Sub PlayerVolumeBar_MouseMove(sender As Object, e As MouseEventArgs) Handles PlayerVolumeBar.MouseMove
        If isVolumeDragging Then
            SetVolumeFromClick(e.X)
        End If
    End Sub

    Private Sub PlayerVolumeBar_MouseUp(sender As Object, e As MouseEventArgs) Handles PlayerVolumeBar.MouseUp
        isVolumeDragging = False
    End Sub

    Public Sub CreateSendToShortcut()
        Dim sendToPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\Windows\SendTo")
        Dim shortcutPath As String = Path.Combine(sendToPath, My.Application.Info.Title & ".lnk")

        Dim shell = CreateObject("WScript.Shell")
        Dim shortcut = shell.CreateShortcut(shortcutPath)

        shortcut.TargetPath = Application.ExecutablePath
        shortcut.WorkingDirectory = Path.GetDirectoryName(Application.ExecutablePath)
        shortcut.IconLocation = Application.ExecutablePath & ",0"

        shortcut.Save()
    End Sub

    Public Sub RemoveSendToShortcut()
        Dim sendToPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\Windows\SendTo")
        Dim shortcutPath As String = Path.Combine(sendToPath, My.Application.Info.Title & ".lnk")

        If File.Exists(shortcutPath) Then
            File.Delete(shortcutPath)
        End If
    End Sub

    Private controlsVisible As Boolean = True
    Private lastMousePos As Point
    Private Sub ControlHideTimer_Tick(sender As Object, e As EventArgs) Handles ControlHideTimer.Tick
        If player.IsMediaLoaded() AndAlso Not isPaused Then
            HideControls()
        End If
        ControlHideTimer.Stop()
    End Sub

    Private Sub ShowControls()
        PlayerControls.Visible = True
        controlsVisible = True
        ControlHideTimer.Stop()
        ControlHideTimer.Start()
    End Sub

    Private Sub HideControls()
        PlayerControls.Visible = False
        controlsVisible = False
    End Sub

    Private Sub BaseOpen_Click(sender As Object, e As EventArgs) Handles BaseOpen.Click
        ResetPlaylist()
        If BasePath.Text.Length > 0 Then
            OpenMedia(BasePath.Text)
        Else
            If PlayerFileDialog.ShowDialog() = DialogResult.OK Then
                For Each f In PlayerFileDialog.FileNames
                    OpenMedia(f)
                Next
            End If
        End If

        If playlist.Count > 0 Then
            PlayFromPlaylist(0)
        End If
    End Sub

    Private Sub BasePath_KeyUp(sender As Object, e As KeyEventArgs) Handles BasePath.KeyUp
        If BasePath.Text.Length > 0 AndAlso e.KeyCode = Keys.Enter Then
            BaseOpen.PerformClick()
        End If
    End Sub

    Private Sub PlayerRemaining_Click(sender As Object, e As EventArgs) Handles PlayerRemaining.Click
        PlayerDuration.BringToFront()
    End Sub

    Private Sub PlayerDuration_Click(sender As Object, e As EventArgs) Handles PlayerDuration.Click
        PlayerRemaining.BringToFront()
    End Sub

    Private Sub SetVideoScale(scale As Double)
        Dim w = player.GetNumberProperty("dwidth")
        Dim h = player.GetNumberProperty("dheight")

        If w <= 0 OrElse h <= 0 Then Exit Sub

        Dim videoPanel = PlayerVideo

        Dim borderW = Me.Width - videoPanel.Width
        Dim borderH = Me.Height - videoPanel.Height

        Dim newW = CInt(w * scale) + borderW
        Dim newH = CInt(h * scale) + borderH

        Me.Size = New Size(newW, newH)
    End Sub

    Private Sub MenuScale_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuScale.DropDownItemClicked
        Dim d As Double = Double.Parse(e.ClickedItem.Tag.ToString, Globalization.CultureInfo.InvariantCulture)
        SetVideoScale(d)
    End Sub
End Class
