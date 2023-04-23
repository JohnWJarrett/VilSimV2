Public Class Relationship

    Private ReadOnly RelationshipUID As String
    Private ReadOnly RStartYear As Integer
    Private ReadOnly EventList As List(Of String)
    Private REndYear As Integer
    Private ReadOnly ChildrenUIDs As List(Of String)

    Public Sub New(maleUID As String, femaleUID As String, startYear As Integer)
        RelationshipUID = maleUID & "-" & femaleUID
        RStartYear = startYear
        EventList = New List(Of String)
        REndYear = -9999 ' -9999 indicates the relationship is ongoing
        ChildrenUIDs = New List(Of String)
    End Sub

    Public Sub AddEvent(eventName As String)
        EventList.Add(eventName)
    End Sub

    Public Sub EndRelationship(endYear As Integer)
        REndYear = endYear
    End Sub

    Public Sub AddChild(childUID As String)
        ChildrenUIDs.Add(childUID)
    End Sub

    Public Function GetRelationshipUID() As String
        Return RelationshipUID
    End Function

    Public Function GetStartYear() As Integer
        Return RStartYear
    End Function

    Public Function GetEventList() As List(Of String)
        Return EventList
    End Function

    Public Function GetEndYear() As Integer
        Return REndYear
    End Function

    Public Function GetChildrenUIDs() As List(Of String)
        Return ChildrenUIDs
    End Function

End Class