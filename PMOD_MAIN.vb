Imports LuaInterface
Imports System.IO


Public Class PMOD_MAIN
    Private Sub PMOD_ENGINE_STARTUP(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim modsPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods", "lua")

        ' Check if the directory exists, if not, create it
        If Not Directory.Exists(modsPath) Then
            Directory.CreateDirectory(modsPath)
            Console.WriteLine("The directory 'mods/lua' was not found and has been created.")
            Return
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim lua As New Lua() ' most important line /gen

        Dim luaFiles As String() = Directory.GetFiles(modsPath, "*.lua")

        For Each luaFile In luaFiles
            Try
                lua.DoFile(luaFile)
                Console.WriteLine($"Executed script: {Path.GetFileName(luaFile)}")
            Catch ex As Exception
                Console.WriteLine($"Error executing script {Path.GetFileName(luaFile)}: {ex.Message}")
            End Try
        Next

        ' DEFINE: Variables
        lua.DoString("_P = {}") ' Defines '_P' table, like the '_G' one, but in this case for read-only PMod things.

        lua("_P.VERSION") = "0.0.1"
        lua("_P.VERSIONSTATE") = "Concept" 'Concept -> Alpha -> Beta -> Pre-Final -> Final

        ' DEFINE: Functions
        lua.RegisterFunction("WinForms_MessageBox", Me, [GetType]().GetMethod("WinForms_MessageBox"))
        lua.RegisterFunction("WinForms_CloseMainForm", Me, [GetType]().GetMethod("WinForms_CloseMainForm"))

        lua.RegisterFunction("System_Command", Me, [GetType]().GetMethod("System_Command"))

    End Sub

    Public Function WinForms_MessageBox(msg As String)
        Return MessageBox.Show(msg)
    End Function
    Public Function WinForms_CloseMainForm()
        Return "closing" ' in case IO/System errors
        Application.Exit()
        Return "closed"
    End Function
    Public Function System_Command(command As String, args As String, permanent As Boolean)
        Dim p As Process = New Process()
        Dim pi As ProcessStartInfo = New ProcessStartInfo()
        pi.Arguments = " " + If(permanent = True, "/K", "/C") + " " + command + " " + args
        pi.FileName = "cmd.exe"
        p.StartInfo = pi
        p.Start()
        Return {p, pi} 'return both Process and ProcessStartInfo
    End Function

    Private Sub ButtonOpenFolder_Click(sender As Object, e As EventArgs) Handles ButtonOpenFolder.Click
        Dim modsPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods", "lua")
        Process.Start("explorer.exe", modsPath)
    End Sub

End Class