Imports System.IO
Imports System.Reflection
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf ResolveAssembly
            LoadEmbeddedDll("libmpv-2.dll")
        End Sub

        Private Function ResolveAssembly(ByVal sender As Object, ByVal e As ResolveEventArgs) As Assembly
            Dim assemblyName As New AssemblyName(e.Name)
            Dim resourceName As String = $"{GetType(MyApplication).Namespace}.{assemblyName.Name}.dll".Replace("My.", "")

            Using stream As Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
                If stream IsNot Nothing Then
                    Dim assemblyData As Byte() = New Byte(stream.Length - 1) {}
                    stream.Read(assemblyData, 0, stream.Length)
                    Return Assembly.Load(assemblyData)
                Else
                    '                    Console.WriteLine($"MISSING EMBEDDED RESOURCE: {resourceName}")
                End If
            End Using

            Return Nothing
        End Function

        Private Sub LoadEmbeddedDll(dllName As String)
            Dim resourceName As String = $"{GetType(MyApplication).Namespace}.{dllName}".Replace("My.", "")

            Using stream As Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
                If stream IsNot Nothing Then
                    Dim assemblyData As Byte() = New Byte(stream.Length - 1) {}
                    stream.Read(assemblyData, 0, stream.Length)
                    Dim tempFile As String = Path.Combine(Path.GetTempPath(), dllName)
                    File.WriteAllBytes(tempFile, assemblyData)
                    LoadLibrary(tempFile)
                Else
                    '                    Console.WriteLine($"MISSING EMBEDDED RESOURCE: {resourceName}")
                End If
            End Using
        End Sub

        <Runtime.InteropServices.DllImport("kernel32.dll", SetLastError:=True)>
        Private Shared Function LoadLibrary(ByVal lpFileName As String) As IntPtr
        End Function
    End Class
End Namespace
