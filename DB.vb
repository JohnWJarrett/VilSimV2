Imports LiteDB

Public Module DB
    Public ldb As New LiteDatabase("Village2.db")
    Public villagers As LiteCollection(Of Villager) = ldb.GetCollection(Of Villager)("villagers")

    Public MyVillage As Village

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

    Public Function GetVillagerList() As List(Of Villager)
        Return AllVillagers.Values.ToList
    End Function

    Public Sub RemoveVillager(V As Villager, CurrentYear As Integer, Optional KillVillager As Boolean = True)
        ' Remove the villager from AllVillagers
        If AllVillagers.ContainsKey(V.UID) Then
            AllVillagers.Remove(V.UID)
            If KillVillager Then
                V.DOD = CurrentYear
            Else
                V.MOY = CurrentYear
                V.DOD = RNGen(CurrentYear, CurrentYear + (V.Age - 100))
            End If
        End If

        ' Move the dead villager to the database
        villagers.Insert(V)

        '' Check if both parties in the relationship are dead, and if so, remove the relationship
        'Dim relationshipsToRemove As New List(Of String)
        'For Each kvp As KeyValuePair(Of String, Relationship) In AllRelationships
        '    Dim relationship As Relationship = kvp.Value
        '    Dim UIDs As String() = kvp.Key.Split(":")

        '    ' Check if both parties in the relationship are dead
        '    If Not AllVillagers.ContainsKey(UIDs(0)) AndAlso Not AllVillagers.ContainsKey(UIDs(1)) Then
        '        relationshipsToRemove.Add(kvp.Key)
        '    End If
        'Next

        '' Remove dead relationships
        'For Each ruid As String In relationshipsToRemove
        '    AllRelationships.Remove(ruid)
        'Next
    End Sub

    Public Function AllRelationshipsAsList() As List(Of Relationship)
        Dim Result As New List(Of Relationship)

        For Each r As Relationship In AllRelationships.Values
            Result.Add(r)
        Next

        Return Result
    End Function

    Public Enum RelInfo
        MaleUID ' By the Males UID
        FemaleUID ' By the Females UID
        AllMarried ' all married couples
        AllUnMarried ' All non-married couples
        AllActive ' active relationships
        AllInactive ' ended relationships
    End Enum

    Public Function GetAllRelationshipsBy(RI As RelInfo, Optional UID As String = "NA") As List(Of Relationship)
        Dim result As New List(Of Relationship)

        Select Case RI
            Case 0
                If Not UID = "NA" Then
                    For Each f As KeyValuePair(Of String, Relationship) In AllRelationships
                        If f.Key.Split(":")(0) = UID Then
                            result.Add(f.Value)
                        End If
                    Next
                End If
            Case 1
                If Not UID = "NA" Then
                    For Each f As KeyValuePair(Of String, Relationship) In AllRelationships
                        If f.Key.Split(":")(1) = UID Then
                            result.Add(f.Value)
                        End If
                    Next
                End If
            Case 2
                For Each f As KeyValuePair(Of String, Relationship) In AllRelationships
                    If f.Value.IsMarried Then
                        result.Add(f.Value)
                    End If
                Next
            Case 3
                For Each f As KeyValuePair(Of String, Relationship) In AllRelationships
                    If Not f.Value.IsMarried Then
                        result.Add(f.Value)
                    End If
                Next
            Case 4
                For Each f As KeyValuePair(Of String, Relationship) In AllRelationships
                    If f.Value.REndYear = -9999 Then
                        result.Add(f.Value)
                    End If
                Next
            Case 5
                For Each f As KeyValuePair(Of String, Relationship) In AllRelationships
                    If Not f.Value.REndYear = -9999 Then
                        result.Add(f.Value)
                    End If
                Next
        End Select

        Return result
    End Function


End Module
