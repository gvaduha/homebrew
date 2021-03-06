Option Strict Off
Option Explicit Off
Imports System
Imports EnvDTE
Imports EnvDTE80
Imports EnvDTE90
Imports EnvDTE90a
Imports EnvDTE100
Imports System.Diagnostics

Public Module Module1
    Dim files As System.Collections.ObjectModel.ReadOnlyCollection(Of String)

    Sub Mac1()
        Dim sp As New SolutionParser
        Dim slnFiles As System.Collections.Generic.SortedSet(Of String)
        Dim dirFiles As System.Collections.Generic.IEnumerable(Of String)

        slnName = "x:\MySln.sln"

        dirFiles = System.IO.Directory.EnumerateFiles(System.IO.Path.GetDirectoryName(slnName))
        slnFiles = sp.GetSlnFiles(slnName)

        For Each fileName As String In dirFiles
            If Not slnFiles.Contains(fileName) Then
                Debug.Print("<Missing>" & fileName)
            End If
        Next
    End Sub
End Module

REM ==============================================================

Imports System
Imports EnvDTE
Imports EnvDTE80
Imports EnvDTE90
Imports EnvDTE90a
Imports EnvDTE100
Imports System.Diagnostics
Imports System.Collections.Generic

Public Class SolutionParser
    Function GetSlnFiles(ByVal slnName As String)
        Dim sln As Solution
        Dim slnFiles As New SortedSet(Of String)

        sln = DTE.Solution
        sln.Open(slnName)

        For Each prj As Project In sln.Projects
            |slnFiles.Add(prj.FullName)
            AddProjectFiles(slnFiles, prj.ProjectItems)
        Next
        Return slnFiles
    End Function

    Private Function AddProjectFiles(ByRef fileList As SortedSet(Of String), ByRef catalog As ProjectItems)
        For Each item As ProjectItem In catalog
            If (String.Compare(item.Kind, "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}") = 0) Then 'Physical File
                Debug.Assert(item.FileCount = 1)
                If (Not fileList.Contains(item.FileNames(0))) Then
                    Debug.Print(item.FileNames(0))
                    fileList.Add(item.FileNames(0))
                End If
            Else
                'Beware of Subprojects if they could be included in project itself
                AddProjectFiles(fileList, item.ProjectItems)
            End If
        Next
    End Function
End Class

