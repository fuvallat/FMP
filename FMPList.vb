Public Class FMPList
    Private Sub RefreshPlaylistUI()
        updating = True

        PlayerList.Nodes.Clear()

        For Each path In MainForm.playlist
            AddPathToTree(path)
        Next

        PlayerList.ExpandAll()

        If MainForm.currentIndex >= 0 AndAlso MainForm.currentIndex < MainForm.playlist.Count Then
            SelectNodeByPath(MainForm.playlist(MainForm.currentIndex))
        End If

        updating = False
    End Sub

    Private Sub FMPList_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        AddHandler MainForm.PlaylistChanged, AddressOf OnPlaylistChanged

        RefreshPlaylistUI()
    End Sub

    Private Sub AddPathToTree(fullPath As String)
        Dim parts = fullPath.Split(IO.Path.DirectorySeparatorChar)
        Dim currentNodes = PlayerList.Nodes
        Dim currentNode As TreeNode = Nothing

        For i = 0 To parts.Length - 2
            Dim folder = parts(i)
            Dim found = If(currentNodes.Cast(Of TreeNode)().FirstOrDefault(Function(n) n.Text = folder), currentNodes.Add(folder))

            currentNode = found
            currentNodes = found.Nodes
        Next

        ' fájl
        Dim fileName = IO.Path.GetFileName(fullPath)
        Dim fileNode = currentNodes.Add(fileName)
        fileNode.Tag = fullPath
    End Sub

    Private Sub SelectNodeByPath(path As String)
        Dim nodes = PlayerList.Nodes
        Dim parts = path.Split(IO.Path.DirectorySeparatorChar)
        Dim currentNode As TreeNode = Nothing

        For i = 0 To parts.Length - 1
            Dim part = parts(i)
            Dim found = nodes.Cast(Of TreeNode)().FirstOrDefault(Function(n) n.Text = part)

            If found Is Nothing Then Exit Sub

            currentNode = found
            nodes = found.Nodes
        Next

        PlayerList.SelectedNode = currentNode
        currentNode.EnsureVisible()
    End Sub

    Private Sub PlayerList_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles PlayerList.AfterSelect
        If updating Then Exit Sub

        Dim path = TryCast(e.Node.Tag, String)
        If path Is Nothing Then Exit Sub

        Dim index = MainForm.playlist.IndexOf(path)
        If index >= 0 Then
            MainForm.PlayFromPlaylist(index)
        End If
    End Sub

    Private Sub PlayerList_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles PlayerList.NodeMouseDoubleClick
        Dim path = TryCast(e.Node.Tag, String)
        If path Is Nothing Then Exit Sub

        Dim index = MainForm.playlist.IndexOf(path)
        If index >= 0 Then
            MainForm.PlayFromPlaylist(index)
        End If
    End Sub

    Private Sub PlayerList_NodeMouseHover(sender As Object, e As TreeNodeMouseHoverEventArgs) Handles PlayerList.NodeMouseHover
        Dim path = TryCast(e.Node.Tag, String)
        If path Is Nothing Then Exit Sub

        ShowMetadataTooltip(path)
    End Sub

    Private Function BuildTooltipText(meta As MediaMetadata, path As String) As String
        Dim sb As New Text.StringBuilder()

        sb.AppendLine(IO.Path.GetFileName(path))
        sb.AppendLine("Méret: " & FormatFileSize(meta.FileSize))

        If meta.Width > 0 Then
            sb.AppendLine("Felbontás: " & meta.Width & "x" & meta.Height)
        End If

        If meta.Duration > 0 Then
            sb.AppendLine("Hossz: " & MpvPlayer.FormatTime(meta.Duration))
        End If

        If Not String.IsNullOrEmpty(meta.VideoCodec) Then
            sb.AppendLine("Videó codec: " & meta.VideoCodec)
        End If

        '        sb.AppendLine("Hangsávok: " & meta.AudioTracks)
        '        sb.AppendLine("Feliratok: " & meta.SubtitleTracks)

        Return sb.ToString()
    End Function

    Private Function FormatFileSize(bytes As Long) As String
        If bytes < 1024 Then Return bytes & " B"
        If bytes < 1024 ^ 2 Then Return Math.Round(bytes / 1024, 1) & " KB"
        If bytes < 1024 ^ 3 Then Return Math.Round(bytes / 1024 ^ 2, 1) & " MB"
        Return Math.Round(bytes / 1024 ^ 3, 1) & " GB"
    End Function

    Private Sub OnPlaylistChanged()
        If Me.Visible Then
            RefreshPlaylistUI()
        End If
    End Sub

    Private updating As Boolean = False

    Private metadataCache As New Dictionary(Of String, MediaMetadata)
    Public Class MediaMetadata
        Public Property FileSize As Long
        Public Property Duration As Double
        Public Property Width As Integer
        Public Property Height As Integer
        Public Property VideoCodec As String
        Public Property AudioTracks As Integer
        Public Property SubtitleTracks As Integer
    End Class

    Private Sub ShowMetadataTooltip(path As String)
        If metadataCache.ContainsKey(path) Then
            PlayerToolTip.SetToolTip(PlayerList, BuildTooltipText(metadataCache(path), path))
            Return
        End If

        Task.Run(Sub()
                     Dim meta = LoadMetadata(path)
                     If meta IsNot Nothing Then
                         metadataCache(path) = meta
                         Me.Invoke(Sub()
                                       PlayerToolTip.SetToolTip(PlayerList, BuildTooltipText(meta, path))
                                   End Sub)
                     End If
                 End Sub)
    End Sub

    Private Function LoadMetadata(path As String) As MediaMetadata
        Try
            Dim meta As New MediaMetadata()

            Dim fi As New IO.FileInfo(path)
            meta.FileSize = fi.Length

            Using probe As New MpvProbe()
                probe.LoadFile(path)

                meta.Duration = probe.GetNumber("duration")
                meta.Width = CInt(probe.GetNumber("width"))
                meta.Height = CInt(probe.GetNumber("height"))
                meta.VideoCodec = probe.GetString("video-codec")
                '                meta.AudioTracks = CInt(probe.GetNumber("audio-tracks"))
                '                meta.SubtitleTracks = CInt(probe.GetNumber("subtitle-tracks"))
            End Using

            Return meta

        Catch
            Return Nothing
        End Try
    End Function

End Class
