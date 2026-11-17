Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
'https://mpv.io/
Public Class MpvPlayer
    Private ctx As IntPtr = IntPtr.Zero
    Private disposed As Boolean = False

    Private Const MPV_FORMAT_INT64 As Integer = 4

    <DllImport("libmpv-2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function mpv_create() As IntPtr
    End Function

    <DllImport("libmpv-2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function mpv_initialize(ctx As IntPtr) As Integer
    End Function

    <DllImport("libmpv-2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Sub mpv_terminate_destroy(ctx As IntPtr)
    End Sub

    <DllImport("libmpv-2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Sub mpv_free(ptr As IntPtr)
    End Sub

    <DllImport("libmpv-2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function mpv_command(ctx As IntPtr, args As IntPtr) As Integer
    End Function

    <DllImport("libmpv-2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function mpv_wait_event(ctx As IntPtr, timeout As Double) As IntPtr
    End Function

    <DllImport("libmpv-2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function mpv_set_option(ctx As IntPtr, name As String, format As Integer, value As IntPtr) As Integer
    End Function

    <DllImport("libmpv-2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function mpv_set_option_string(ctx As IntPtr, name As String, value As String) As Integer
    End Function

    <DllImport("libmpv-2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function mpv_get_property(ctx As IntPtr, name As String, format As Integer, value As IntPtr) As Integer
    End Function

    <DllImport("libmpv-2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function mpv_get_property_string(ctx As IntPtr, name As String) As IntPtr
    End Function

    <DllImport("libmpv-2.dll", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function mpv_observe_property(ctx As IntPtr, userdata As ULong, name As String, format As Integer) As Integer
    End Function

    Private Enum Mpv_event_id
        MPV_EVENT_NONE = 0
        MPV_EVENT_SHUTDOWN = 1
        MPV_EVENT_LOG_MESSAGE = 2
        MPV_EVENT_GET_PROPERTY_REPLY = 3
        MPV_EVENT_SET_PROPERTY_REPLY = 4
        MPV_EVENT_COMMAND_REPLY = 5
        MPV_EVENT_START_FILE = 6
        MPV_EVENT_END_FILE = 7
        MPV_EVENT_FILE_LOADED = 8
        MPV_EVENT_TRACKS_CHANGED = 9
        MPV_EVENT_TRACK_SWITCHED = 10
        MPV_EVENT_IDLE = 11
        MPV_EVENT_PAUSE = 12
        MPV_EVENT_UNPAUSE = 13
        MPV_EVENT_TICK = 14
        MPV_EVENT_SCRIPT_INPUT_DISPATCH = 15
        MPV_EVENT_CLIENT_MESSAGE = 16
        MPV_EVENT_VIDEO_RECONFIG = 17
        MPV_EVENT_AUDIO_RECONFIG = 18
        MPV_EVENT_METADATA_UPDATE = 19
        MPV_EVENT_SEEK = 20
        MPV_EVENT_PLAYBACK_RESTART = 21
        MPV_EVENT_PROPERTY_CHANGE = 22
        MPV_EVENT_CHAPTER_CHANGE = 23
        MPV_EVENT_QUEUE_OVERFLOW = 24
    End Enum

    Private Enum Mpv_end_file_reason
        MPV_END_FILE_REASON_EOF = 0
        MPV_END_FILE_REASON_STOP = 2
        MPV_END_FILE_REASON_ERROR = 4
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Private Structure Mpv_event
        Public event_id As Mpv_event_id
        Public err As Integer
        Public reply_userdata As Long
        Public data As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure Mpv_event_end_file
        Public reason As Mpv_end_file_reason
        Public err As Integer
    End Structure

    Private eventThread As Thread
    Private running As Boolean = False

    Private Sub StartEventLoop()
        running = True
        eventThread = New Thread(AddressOf EventLoop) With {
            .IsBackground = True
        }
        eventThread.Start()
    End Sub

    Private Sub EventLoop()
        While running
            Dim ev = mpv_wait_event(ctx, 0.1)

            If ev <> IntPtr.Zero Then
                HandleEvent(ev)
            End If
        End While
    End Sub

    Public Event VideoParamsChanged()
    Public Event FileLoaded()
    Public Event FileLoadFailed()
    Public Event PlaybackEnded()
    Private Sub HandleEvent(evPtr As IntPtr)
        Dim ev = Marshal.PtrToStructure(Of Mpv_event)(evPtr)

        Select Case ev.event_id
            Case Mpv_event_id.MPV_EVENT_VIDEO_RECONFIG
                RaiseEvent VideoParamsChanged()
            Case Mpv_event_id.MPV_EVENT_FILE_LOADED
                RaiseEvent FileLoaded()
            Case Mpv_event_id.MPV_EVENT_END_FILE
                Dim data = Marshal.PtrToStructure(Of Mpv_event_end_file)(ev.data)
                If data.reason = Mpv_end_file_reason.MPV_END_FILE_REASON_ERROR Then
                    RaiseEvent FileLoadFailed()
                End If
                If data.reason = Mpv_end_file_reason.MPV_END_FILE_REASON_EOF Then
                    RaiseEvent PlaybackEnded()
                End If
        End Select
    End Sub

    Public Sub Init(ownerHandle As IntPtr)
        If ctx <> IntPtr.Zero Then Return

        ctx = mpv_create()
        If ctx = IntPtr.Zero Then
            Throw New Exception("mpv_create failed")
        End If

        Dim widPtr = Marshal.AllocHGlobal(8)
        Marshal.WriteInt64(widPtr, ownerHandle.ToInt64())
        Dim hr = mpv_set_option(ctx, "wid", MPV_FORMAT_INT64, widPtr)
        Marshal.FreeHGlobal(widPtr)

        If hr <> 0 Then
            Throw New Exception("mpv_set_option(wid) failed, hr=" & hr)
        End If

        mpv_set_option_string(ctx, "vo", "gpu")
        mpv_set_option_string(ctx, "hwdec", "auto-safe")

        hr = mpv_initialize(ctx)
        If hr <> 0 Then
            Throw New Exception("mpv_initialize failed, hr=" & hr)
        End If

        MpvCommand("set", "volume", "50")

        StartEventLoop()
    End Sub

    Public Sub ShutdownPlayer()
        running = False
        If eventThread IsNot Nothing AndAlso eventThread.IsAlive Then
            eventThread.Join()
        End If

        If ctx <> IntPtr.Zero Then
            MpvCommand(New String() {"stop", Nothing})
            mpv_terminate_destroy(ctx)
            ctx = IntPtr.Zero
        End If
    End Sub

    Public Sub OpenMedia(owner As IntPtr, path As String)
        If ctx = IntPtr.Zero Then
            Init(owner)
        End If
        MpvCommand(New String() {"loadfile", path, Nothing})
    End Sub

    Public Sub LoadFile(path As String)
        If ctx = IntPtr.Zero Then Throw New InvalidOperationException("MpvPlayer not initialized")
        MpvCommand("loadfile", path)
    End Sub

    Private Sub MpvCommand(ParamArray args As String())
        Dim ptrs(args.Length) As IntPtr

        For i = 0 To args.Length - 1
            If args(i) Is Nothing Then
                ptrs(i) = IntPtr.Zero
            Else
                Dim bytes = Encoding.UTF8.GetBytes(args(i) & vbNullChar)
                Dim p = Marshal.AllocHGlobal(bytes.Length)
                Marshal.Copy(bytes, 0, p, bytes.Length)
                ptrs(i) = p
            End If
        Next

        ptrs(args.Length) = IntPtr.Zero

        Dim size = IntPtr.Size * ptrs.Length
        Dim argv = Marshal.AllocHGlobal(size)
        Marshal.Copy(ptrs, 0, argv, ptrs.Length)

        Dim hr = mpv_command(ctx, argv)

        For i = 0 To args.Length - 1
            If ptrs(i) <> IntPtr.Zero Then Marshal.FreeHGlobal(ptrs(i))
        Next
        Marshal.FreeHGlobal(argv)

        If hr <> 0 Then
            Throw New Exception("mpv_command failed, hr=" & hr & " (" & String.Join(" ", args) & ")")
        End If
    End Sub

    Public Function IsMediaLoaded() As Boolean
        If ctx <> IntPtr.Zero Then
            Dim idle = GetStringProperty("path")
            Return Not String.IsNullOrEmpty(idle)
        Else
            Return False
        End If
    End Function

    Public Sub Pause()
        If IsMediaLoaded() Then MpvCommand("set", "pause", "yes")
    End Sub

    Public Sub [Resume]()
        If IsMediaLoaded() Then MpvCommand("set", "pause", "no")
    End Sub

    Public Sub [Stop]()
        If IsMediaLoaded() Then MpvCommand("stop")
    End Sub

    Public Sub Seek(offsetSeconds As Double)
        If IsMediaLoaded() Then MpvCommand("seek", offsetSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture), "relative")
    End Sub

    Public Sub Shutdown()
        If ctx <> IntPtr.Zero Then
            mpv_terminate_destroy(ctx)
            ctx = IntPtr.Zero
        End If
    End Sub

    Public Function GetPosition() As Double
        Return GetNumberProperty("time-pos")
    End Function

    Public Function GetDuration() As Double
        Return GetNumberProperty("duration")
    End Function

    Public Sub SeekAbsolute(seconds As Double)
        If IsMediaLoaded() Then MpvCommand(New String() {"seek", seconds.ToString(System.Globalization.CultureInfo.InvariantCulture), "absolute"})
    End Sub

    Public Function GetTrackListJson() As String
        Dim ptr = mpv_get_property_string(ctx, "track-list")
        If ptr = IntPtr.Zero Then Return Nothing

        Dim s = Marshal.PtrToStringAnsi(ptr)
        mpv_free(ptr)
        Return s
    End Function

    Public Function GetAudioTrackTitle(id As Integer) As String
        Dim json = GetTrackListJson()
        If String.IsNullOrEmpty(json) Then Return $"Audio {id}"

        Dim parts = json.Split("{"c)

        For Each p In parts
            If p.Contains("""type"":""audio""") AndAlso p.Contains($"""id"":{id}") Then

                Dim title = ExtractString(p, """title"":")
                Dim lang = ExtractString(p, """lang"":")

                If Not String.IsNullOrWhiteSpace(title) Then
                    Return title
                End If

                If Not String.IsNullOrWhiteSpace(lang) Then
                    Return lang.ToUpper()
                End If

                Exit For
            End If
        Next

        Return $"Audio {id}"
    End Function

    Private Function ExtractInt(src As String, key As String) As Integer
        Dim i = src.IndexOf(key)
        If i = -1 Then Return 0
        i += key.Length
        Dim num = ""
        While i < src.Length AndAlso Char.IsDigit(src(i))
            num &= src(i)
            i += 1
        End While
        Return If(num = "", 0, CInt(num))
    End Function

    Private Function ExtractString(src As String, key As String) As String
        Dim i = src.IndexOf(key)
        If i = -1 Then Return Nothing
        i += key.Length
        While i < src.Length AndAlso (src(i) = " "c Or src(i) = ":"c Or src(i) = """"c)
            i += 1
        End While
        Dim s = ""
        While i < src.Length AndAlso src(i) <> """"c
            s &= src(i)
            i += 1
        End While
        Return s
    End Function

    Public Function GetAudioTracks() As List(Of (Id As Integer, Name As String))
        Dim result As New List(Of (Integer, String))
        Dim json = GetTrackListJson()
        If String.IsNullOrEmpty(json) Then Return result

        Dim parts = json.Split("{"c)

        For Each p In parts
            If p.Contains("""type"":""audio""") Then
                Dim id = ExtractInt(p, """id"":")
                Dim lang = ExtractString(p, """lang"":")
                If String.IsNullOrEmpty(lang) Then lang = "Audio " & id
                result.Add((id, lang))
            End If
        Next

        Return result
    End Function

    Public Function GetCurrentAudioTrackId() As Integer
        Dim v = GetNumberProperty("aid")
        Return CInt(v)
    End Function

    Public Sub SetAudioTrack(id As Integer)
        MpvCommand(New String() {"set", "aid", id.ToString()})
    End Sub

    Public Function GetSubtitleTrackIds() As List(Of Integer)
        Dim result As New List(Of Integer)

        Dim ptr = mpv_get_property_string(ctx, "sid-list")
        If ptr = IntPtr.Zero Then Return result

        Dim raw = Marshal.PtrToStringAnsi(ptr)
        mpv_free(ptr)

        ' raw pl.: ["1","2","3"]
        raw = raw.Replace("[", "").Replace("]", "").Replace("""", "")

        For Each part In raw.Split(","c)
            Dim id As Integer
            If Integer.TryParse(part.Trim(), id) Then
                result.Add(id)
            End If
        Next

        Return result
    End Function

    Public Function GetSubtitleTrackTitle(id As Integer) As String
        Dim json = GetTrackListJson()
        If String.IsNullOrEmpty(json) Then Return $"Subtitle {id}"

        Dim parts = json.Split("{"c)

        For Each p In parts
            If p.Contains("""type"":""sub""") AndAlso p.Contains($"""id"":{id}") Then

                Dim title = ExtractString(p, """title"":")
                Dim lang = ExtractString(p, """lang"":")

                If Not String.IsNullOrWhiteSpace(title) Then
                    Return title
                End If

                If Not String.IsNullOrWhiteSpace(lang) Then
                    Return lang.ToUpper()
                End If

                Exit For
            End If
        Next

        Return $"Subtitle {id}"
    End Function

    Public Function GetSubtitleTracks() As List(Of (Id As Integer, Name As String))
        Dim result As New List(Of (Integer, String))
        Dim json = GetTrackListJson()
        If String.IsNullOrEmpty(json) Then Return result

        Dim parts = json.Split("{"c)

        For Each p In parts
            If p.Contains("""type"":""sub""") Then
                Dim id = ExtractInt(p, """id"":")
                Dim lang = ExtractString(p, """lang"":")
                If String.IsNullOrEmpty(lang) Then lang = "Subtitle " & id
                result.Add((id, lang))
            End If
        Next

        Return result
    End Function

    Public Function GetCurrentSubtitleTrackId() As Integer
        Dim v = GetNumberProperty("sid")
        Return CInt(v)
    End Function

    Public Sub SetSubtitleTrack(id As Integer)
        MpvCommand(New String() {"set", "sid", id.ToString()})
    End Sub

    Public Sub DisableSubtitles()
        MpvCommand(New String() {"set", "sid", "no"})
    End Sub

    Public Function GetAllPropertyNames() As List(Of String)
        Dim result As New List(Of String)

        Dim ptr = mpv_get_property_string(ctx, "property-list")
        If ptr = IntPtr.Zero Then Return result

        Dim raw = Marshal.PtrToStringAnsi(ptr)
        mpv_free(ptr)

        raw = raw.Replace("[", "").Replace("]", "").Replace("""", "")

        For Each part In raw.Split(","c)
            Dim name = part.Trim()
            If name <> "" Then result.Add(name)
        Next

        Return result
    End Function

    Public Function GetProperty(name As String) As String
        Return GetStringProperty(name)
    End Function

    Private Function PtrToStringUTF8(ptr As IntPtr) As String
        If ptr = IntPtr.Zero Then Return Nothing

        ' Megkeressük a null-terminátort
        Dim length As Integer = 0
        While Marshal.ReadByte(ptr, length) <> 0
            length += 1
        End While

        Dim buffer(length - 1) As Byte
        Marshal.Copy(ptr, buffer, 0, length)

        Return Encoding.UTF8.GetString(buffer)
    End Function

    Public Function GetStringProperty(name As String) As String
        Dim ptr = mpv_get_property_string(ctx, name)
        If ptr = IntPtr.Zero Then Return Nothing

        Dim s = PtrToStringUTF8(ptr)
        mpv_free(ptr)
        Return s
    End Function

    Public Sub SetPropertyString(name As String, value As String)
        MpvCommand("set", name, value)
    End Sub

    Public Function GetNumberProperty(name As String) As Double
        Dim s = GetStringProperty(name)
        If String.IsNullOrWhiteSpace(s) Then Return 0

        Dim v As Double
        If Double.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, v) Then
            Return v
        End If

        Return 0 'Double.NaN
    End Function

    Public Sub SetProperty(name As String, value As Integer)
        MpvCommand("set", name, value.ToString())
    End Sub

    Public Function GetAllProperties() As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)

        Dim names = GetAllPropertyNames()

        For Each name In names
            Try
                Dim value = GetStringProperty(name)
                If value IsNot Nothing Then
                    result(name) = value
                End If
            Catch ex As Exception

            End Try
        Next

        Return result
    End Function

    Public Class MediaInfo
        Public Property Duration As Double
        Public Property Width As Integer
        Public Property Height As Integer
        Public Property Ratio As String
        Public Property FPS As Double
        Public Property VideoCodec As String
        Public Property AudioCodec As String
        Public Property AudioChannels As String
        Public Property AudioSampleRate As Integer
        Public Property Container As String
        Public Property Title As String
        Public Property Path As String
        Public Property FileSize As Long
        Public Property AudioTracks As List(Of String)
        Public Property SubtitleTracks As List(Of String)
    End Class

    Public Function GetMediaType() As String
        Dim hasVideo = GetNumberProperty("video") > 0
        Dim hasAudio = GetNumberProperty("audio") > 0

        If hasVideo Then
            Return "video"
        End If

        If hasAudio Then
            Return "audio"
        End If

        Return "image"
    End Function

    Public Function GetMediaTitle() As String
        Dim title = GetStringProperty("media-title")
        If Not String.IsNullOrWhiteSpace(title) Then
            Return title
        End If

        Dim fileName = GetStringProperty("filename/no-ext")
        If Not String.IsNullOrWhiteSpace(fileName) Then
            Return fileName
        End If

        Dim path = GetStringProperty("file-path")
        If Not String.IsNullOrWhiteSpace(path) Then
            Return IO.Path.GetFileName(path)
        End If

        Return "Ismeretlen cím"
    End Function

    Private Shared ReadOnly FilmRatios As (Double, String, String)() = {
    (1.3333, "4:3", "Academy / SD"),
    (1.7777, "16:9", "HD / Full HD"),
    (1.85, "1.85:1", "Flat / Cinema"),
    (2.35, "2.35:1", "CinemaScope (régi)"),
    (2.39, "2.39:1", "CinemaScope / Widescreen"),
    (2.4, "2.40:1", "CinemaScope (modern)"),
    (1.6, "16:10", "Laptop / Monitor"),
    (1.5, "3:2", "Fotó / DSLR"),
    (1.25, "5:4", "Régi monitor")
}

    Public Function AspectToFilmRatio(aspect As Double) As String
        Dim bestName As String = aspect.ToString("0.00:1")
        Dim bestDiff As Double = Double.MaxValue

        For Each r In FilmRatios
            Dim diff = Math.Abs(aspect - r.Item1)
            If diff < bestDiff Then
                bestDiff = diff
                bestName = r.Item2
            End If
        Next

        Return bestName
    End Function

    Public Function GetAspectInfo(aspect As Double) As (String, String)
        Dim bestRatio As String = aspect.ToString("0.00:1")
        Dim bestName As String = "Ismeretlen képarány"
        Dim bestDiff As Double = Double.MaxValue

        For Each r In FilmRatios
            Dim diff = Math.Abs(aspect - r.Item1)
            If diff < bestDiff Then
                bestDiff = diff
                bestRatio = r.Item2
                bestName = r.Item3
            End If
        Next

        Return (bestRatio, bestName)
    End Function

    Public Function GetCleanMediaInfo() As MediaInfo
        Dim info As New MediaInfo With {
            .Duration = GetNumberProperty("duration"),
            .FPS = GetNumberProperty("container-fps").ToString("N2"),
            .Width = GetNumberProperty("width"),
            .Height = GetNumberProperty("height"),
            .VideoCodec = GetStringProperty("video-codec"),
            .AudioCodec = GetStringProperty("audio-codec"),
            .AudioChannels = GetStringProperty("audio-params/channels"),
            .AudioSampleRate = GetNumberProperty("audio-params/samplerate"),
            .Container = GetStringProperty("file-format"),
            .Title = GetMediaTitle(),
            .Path = GetStringProperty("path"),
            .FileSize = CLng(GetNumberProperty("file-size")),
            .AudioTracks = GetAudioTracks().Select(Function(t) $"{t.Id} - {GetAudioTrackTitle(t.Id)}").ToList(),
            .SubtitleTracks = GetSubtitleTracks().Select(Function(t) $"{t.Id} - {GetSubtitleTrackTitle(t.Id)}").ToList()
        }

        Dim ratioString = GetAspectInfo(GetNumberProperty("video-params/aspect"))
        info.Ratio = String.Join(" ", {ratioString.Item1, ratioString.Item2})

        If info.Duration = 0 Then info.Duration = GetNumberProperty("length")
        If info.Duration = 0 Then
            Dim pos = GetNumberProperty("time-pos")
            Dim remain = GetNumberProperty("playtime-remaining")
            If remain > 0 Then info.Duration = pos + remain
        End If

        If info.FPS = 0 Then info.FPS = GetNumberProperty("fps").ToString("N2")
        If info.FPS = 0 Then info.FPS = GetNumberProperty("demux-fps").ToString("N2")
        If info.FPS = 0 Then info.FPS = GetNumberProperty("estimated-vf-fps").ToString("N2")

        Return info
    End Function

    Public Shared Function FormatTime(seconds As Double) As String
        If seconds <= 0 Then Return "00:00:00"
        Dim ts = TimeSpan.FromSeconds(seconds)
        Return ts.ToString("hh\:mm\:ss")
    End Function

    Public Shared Function FormatTimeMs(seconds As Double) As String
        If seconds <= 0 Then Return "00:00:00.000"
        Dim ts = TimeSpan.FromSeconds(seconds)
        Return ts.ToString("hh\:mm\:ss\.fff")
    End Function

    Public Sub SetVolume(vol As Integer)
        MpvCommand("set", "volume", vol.ToString())
    End Sub

    Public Shared Function FormatSize(bytes As Long) As String
        Dim sizes = {"B", "KiB", "MiB", "GiB", "TiB"}
        Dim len = CDbl(bytes)
        Dim order = 0

        While len >= 1024 AndAlso order < sizes.Length - 1
            order += 1
            len /= 1024
        End While

        Return $"{len:0.##} {sizes(order)}"
    End Function

    Public Sub SendCommand(ParamArray args As String())
        MpvCommand(args)
    End Sub

    Protected Overrides Sub Finalize()
        Try
            If Not disposed Then
                Shutdown()
                disposed = True
            End If
        Finally
            MyBase.Finalize()
        End Try
    End Sub
End Class