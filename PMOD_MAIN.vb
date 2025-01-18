Imports LuaInterface

Public Class PMOD_MAIN
    Private Sub PMOD_ENGINE_STARTUP(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim lua As New Lua()

        ' DEFINE: Variables
        lua.DoString("_P = {}") ' Defines '_P' table, like the '_G' one, but in this case for read-only PMod things.

        lua("_P.VERSION") = "0.0.1"
        lua("_P.VERSIONSTATE") = "Concept"

        ' DEFINE: Functions
        lua.RegisterFunction("WinForms_MessageBox", Me, [GetType]().GetMethod("WinForms_MessageBox"))
        lua.RegisterFunction("WinForms_CloseMainForm", Me, [GetType]().GetMethod("WinForms_CloseMainForm"))

        lua.RegisterFunction("System_Command", Me, [GetType]().GetMethod("System_Command"))

    End Sub

    Public Function WinForms_MessageBox(msg As String)
        Return MessageBox.Show(msg)
    End Function
    Public Function WinForms_CloseMainForm()
        Application.Exit()
        Return "defined"
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

End Class