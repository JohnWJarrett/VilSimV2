Imports LiteDB

Public Module DB
    Public ldb As New LiteDatabase("Village2.db")
    Public villagers As LiteCollection(Of Villager) = ldb.GetCollection(Of Villager)("villagers")

    Public AllVillagers As New Dictionary(Of String, Villager)
    Public Property AllRelationships As New Dictionary(Of String, Relationship)
    Public Property VilResources As New Resources

    Public Function GetVillagerByUID(UID As String) As Villager
        Dim villager As Villager = Nothing

        If AllVillagers.TryGetValue(UID, villager) Then
            Return villager
        Else
            Return Nothing ' Or handle the case where the villager is not found
        End If
    End Function

    Public Function GetRelationByRUID(RUID As String) As Relationship
        Dim relationship As Relationship = Nothing

        If AllRelationships.TryGetValue(RUID, relationship) Then
            Return relationship
        Else
            Return Nothing ' Or handle the case where the villager is not found
        End If
    End Function

End Module
