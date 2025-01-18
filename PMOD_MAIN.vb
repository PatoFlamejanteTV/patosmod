Imports LuaInterface
Imports System.IO

Public Class PMOD_MAIN
    Private Sub PMOD_ENGINE_STARTUP(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim modsPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods", "lua")
        Dim modulesPath As String = Path.Combine(modsPath, "modules")

        ' Check if the directories exist, if not, create them
        If Not Directory.Exists(modsPath) Then
            Directory.CreateDirectory(modsPath)
            Console.WriteLine("The directory 'mods/lua' was not found and has been created.")
            Return
        End If

        If Not Directory.Exists(modulesPath) Then
            Directory.CreateDirectory(modulesPath)
            Console.WriteLine("The directory 'mods/lua/modules' was not found and has been created.")
        End If

        Dim lua As New Lua() ' most important line /gen

        ' Update Lua path to include the modules directory
        lua.DoString(String.Format("package.path = package.path .. ';{0}{1}?.lua'", modulesPath, Path.DirectorySeparatorChar))

        Console.WriteLine(String.Format("package.path = package.path .. ';{0}{1}?.lua'", modulesPath, Path.DirectorySeparatorChar))

        ' DEFINE: Variables
        lua.DoString("
        _P = {}
        _P.URL = 'https://github.com/PatoFlamejanteTV/patosmod/tree/main'
        _P.BRANCH = 'main'
        _P.GITHUB = 'PatoFlamejanteTV/patosmod'
        _P.VERSION = '0.0.1'
        _P.VERSIONSTATE = 'Concept'
        ")

        ' DEFINE: Functions
        lua.RegisterFunction("MessageBox", Me, [GetType]().GetMethod("WinForms_MessageBox"))
        lua.RegisterFunction("CloseMainForm", Me, [GetType]().GetMethod("WinForms_CloseMainForm"))
        lua.RegisterFunction("SysCommand", Me, [GetType]().GetMethod("System_Command"))

        Dim luaFiles As String() = Directory.EnumerateFiles(modsPath).
                             Where(Function(f) f.EndsWith(".lua") OrElse f.EndsWith(".txt")).
                             ToArray()

        For Each luaFile In luaFiles
            Try
                ' Read the first line of the Lua script to check for --!strict
                Dim firstLine As String = File.ReadLines(luaFile).FirstOrDefault()

                If firstLine IsNot Nothing AndAlso firstLine.Trim() = "--!strict" Then
                    lua.DoString("require('mods\\lua\\modules\\strict')")
                End If

                lua.DoFile(luaFile)
                Console.WriteLine($"Executed script: {Path.GetFileName(luaFile)}")
            Catch ex As Exception
                Console.WriteLine($"Error executing script {Path.GetFileName(luaFile)}: {ex.Message}")
                MessageBox.Show($"Error executing script {Path.GetFileName(luaFile)}: {ex.Message}")
            End Try
        Next
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