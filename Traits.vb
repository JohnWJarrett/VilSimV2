Public Class Traits

    Public Property TraitsList As List(Of Trilean)

    Public Sub New()
        Dim nTrait As Trilean
        TraitsList = New List(Of Trilean)
        For Each l As String In IO.File.ReadAllLines("traits.txt")
            nTrait = New Trilean(l.Split(".")(0), l.Split(".")(1))
            TraitsList.Add(nTrait)
        Next
    End Sub

    Public Function CalculateScore(Vil2 As Villager) As Double
        Dim Result As Double

        For i As Integer = 0 To TraitsList.Count - 1
            Result += TraitsList(i).Scale - Vil2.Traits.TraitsList(i).Scale
        Next

        Return Result
    End Function

    Public Sub RandomizeTraits()
        For Each t As Trilean In TraitsList
            t.Scale = RNGenD()
        Next
    End Sub

End Class