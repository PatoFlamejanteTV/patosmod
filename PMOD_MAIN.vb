Imports LuaInterface
Imports System.IO
Imports System.CodeDom.Compiler
Imports System.Reflection
Public Class PMOD_MAIN
    Private Sub PMOD_ENGINE_STARTUP(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim luamodsPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods", "lua")
        Dim vbmodsPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods", "vb")
        Dim modulesPath As String = Path.Combine(luamodsPath, "modules")

        ' Check if the directories exist, if not, create them
        If Not Directory.Exists(luamodsPath) Then
            Directory.CreateDirectory(luamodsPath)
            Console.WriteLine("The directory 'mods/lua' was not found and has been created.")
            Return
        End If

        If Not Directory.Exists(vbmodsPath) Then
            Directory.CreateDirectory(vbmodsPath)
            Console.WriteLine("The directory 'mods/vb' was not found and has been created.")
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
        _P.SUPPORTEDLANGS = {'lua'}
        ")

        ' DEFINE: Functions
        lua.RegisterFunction("MessageBox", Me, [GetType]().GetMethod("WinForms_MessageBox"))
        lua.RegisterFunction("CloseMainForm", Me, [GetType]().GetMethod("WinForms_CloseMainForm"))
        lua.RegisterFunction("SysCommand", Me, [GetType]().GetMethod("System_Command"))

        lua.RegisterFunction("SysBrowser_Open", Me, [GetType]().GetMethod("System_BrowserOpen"))


        Dim luaFiles As String() = Directory.EnumerateFiles(luamodsPath).
                             Where(Function(f) f.EndsWith(".lua") OrElse f.EndsWith(".txt")).
                             ToArray()

        ' Enumerate .vb or .txt files
        Dim vbFiles As String() = Directory.EnumerateFiles(vbmodsPath).
                             Where(Function(f) f.EndsWith(".vb") OrElse f.EndsWith(".txt")).
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

        For Each vbFile In vbFiles
            Try
                ' Read the content of the VB.NET script
                Dim vbCode As String = File.ReadAllText(vbFile)

                ' Compile and execute the VB.NET code
                ExecuteVbNetCode(vbCode)
                Console.WriteLine($"Executed script: {Path.GetFileName(vbFile)}")
            Catch ex As Exception
                Console.WriteLine($"Error executing script {Path.GetFileName(vbFile)}: {ex.Message}")
                MessageBox.Show($"Error executing script {Path.GetFileName(vbFile)}: {ex.Message}")
            End Try
        Next
    End Sub

    Public Sub ExecuteVbNetCodeFromFile(filePath As String)
        Try
            ' Read the VB.NET code from the file
            Dim vbCode As String = File.ReadAllText(filePath)

            ' Execute the VB.NET code
            ExecuteVbNetCode(vbCode)
        Catch ex As Exception
            MessageBox.Show("Error reading or executing VB.NET code: " & ex.Message)
        End Try
    End Sub

    Private Sub ExecuteVbNetCode(vbCode As String)
        Try
            Dim vbProvider As New VBCodeProvider()
            Dim parameters As New CompilerParameters()
            parameters.GenerateInMemory = True

            parameters.ReferencedAssemblies.Add("System.dll")
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll")
            ' Wrap the code in a simple class and method
            Dim wrappedCode As String = "
                Imports System
                " & vbCode & "
            "

            ' Compile
            Dim results As CompilerResults = vbProvider.CompileAssemblyFromSource(parameters, wrappedCode)

            If results.Errors.HasErrors Then
                Console.WriteLine("Error in VB.NET code: " & results.Errors(0).ErrorText)
                MessageBox.Show("Error in VB.NET code: " & results.Errors(0).ErrorText)
            Else
                ' Execute
                Dim assembly As Assembly = results.CompiledAssembly
                Dim scriptType As Type = assembly.GetType("Script")
                Dim scriptInstance As Object = Activator.CreateInstance(scriptType)
                scriptType.GetMethod("Execute").Invoke(scriptInstance, Nothing)
            End If
        Catch ex As Exception
            Console.WriteLine("VB Exception: " & ex.Message)
            MessageBox.Show("VB Exception: " & ex.Message)
        End Try
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

    Public Function System_BrowserOpen(url As String)
        Return Process.Start("cmd.exe", "start" & url)
    End Function

    Private Sub ButtonOpenFolder_Click(sender As Object, e As EventArgs) Handles ButtonOpenFolder.Click
        Dim modsPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods", "lua")
        Process.Start("explorer.exe", modsPath)
    End Sub
End Class