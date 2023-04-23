Public Class LineManager
    Private ReadOnly Property LineList As List(Of String)

    Public Sub New(Linespath As String)
        LineList = New List(Of String)
        For Each l As String In IO.File.ReadAllLines(Linespath)
            If Not l = "" Then
                LineList.Add(l)
            End If
        Next
    End Sub

    Public Function GetLine() As String
        Return LineList(RNGen(LineList.Count - 1))
    End Function
End Class
