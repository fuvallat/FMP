Imports System.Runtime.InteropServices

Public Class MpvProbe
    Implements IDisposable

    Private ctx As IntPtr

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

    Public Sub New()
        ctx = mpv_create()
        mpv_set_option_string(ctx, "vo", "null")
        mpv_set_option_string(ctx, "ao", "null")
        mpv_initialize(ctx)
    End Sub

    Private Function BuildArgs(args As String()) As IntPtr
        Dim ptrs(args.Length) As IntPtr

        For i = 0 To args.Length - 1
            ptrs(i) = Marshal.StringToHGlobalAnsi(args(i))
        Next

        ptrs(args.Length) = IntPtr.Zero ' null terminator

        Dim size = IntPtr.Size * ptrs.Length
        Dim dest = Marshal.AllocHGlobal(size)
        Marshal.Copy(ptrs, 0, dest, ptrs.Length)

        Return dest
    End Function

    Private Sub FreeArgs(ptr As IntPtr, count As Integer)
        Dim arr(count) As IntPtr
        Marshal.Copy(ptr, arr, 0, count + 1)

        For i = 0 To count - 1
            If arr(i) <> IntPtr.Zero Then Marshal.FreeHGlobal(arr(i))
        Next

        Marshal.FreeHGlobal(ptr)
    End Sub

    Public Sub LoadFile(path As String)
        mpv_command(ctx, BuildArgs({"loadfile", path}))
        Threading.Thread.Sleep(100)
    End Sub

    Public Function GetString(name As String) As String
        Dim ptr = mpv_get_property_string(ctx, name)
        If ptr = IntPtr.Zero Then Return Nothing
        Dim s = Marshal.PtrToStringAnsi(ptr)
        mpv_free(ptr)
        Return s
    End Function

    Public Function GetNumber(name As String) As Double
        Dim s = GetString(name)
        If Double.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, Nothing) Then
            Return Double.Parse(s, Globalization.CultureInfo.InvariantCulture)
        End If
        Return 0
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        If ctx <> IntPtr.Zero Then
            mpv_terminate_destroy(ctx)
            ctx = IntPtr.Zero
        End If
    End Sub
End Class