'This module's imports and settings.
Option Compare Binary
Option Explicit On
Option Infer Off
Option Strict On

Imports System
Imports System.Environment
Imports System.IO
Imports System.Linq
Imports System.Text

'This module contains this program's core procedures.
Public Module CoreModule

   'This program is executed when this program is started.
   Public Sub Main()
      Try
         Dim Block As New StringBuilder
         Dim Count As Integer = 0
         Dim InputFile As String = Nothing
         Dim OutputFile As String = $"{InputFile}.tmp"
         Dim OutputText As New StringBuilder
         Dim PreviousBlock As String = Nothing

         If GetCommandLineArgs().Count < 2 Then
            Console.WriteLine("No input file specified.")
         Else
            InputFile = GetCommandLineArgs().Last()

            For Each Line As String In File.ReadAllLines(InputFile)
               If Line = Nothing Then
                  If PreviousBlock IsNot Nothing AndAlso Not Block.ToString() = Nothing AndAlso Block.ToString() = PreviousBlock Then
                     Count += 1
                  ElseIf Not Block.ToString().Contains("[0x???] = ???") Then
                     OutputText.Append($"{Block}{NewLine}")
                  End If
                  PreviousBlock = Block.ToString()
                  Block.Clear()
               Else
                  Block.Append($"{Line}{NewLine}")
               End If
            Next Line

            If Block.ToString() IsNot Nothing Then
               OutputText.Append(Block)
            End If

            If Not OutputText.ToString().StartsWith(NewLine) Then
               OutputText.Insert(0, NewLine)
            End If

            File.WriteAllText(OutputFile, OutputText.ToString())
            File.Delete(InputFile)
            File.Move(OutputFile, InputFile)
            Console.WriteLine($"{Count} duplicate trace events removed.")
         End If
      Catch [Exception] As Exception
         Console.WriteLine($"ERROR: {[Exception]}")
      End Try

      Console.ReadLine()
   End Sub

End Module