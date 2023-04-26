Public Class BuildingTemplate
    Public Property BuildingType As String
    Public Property DataString As String
    Public Property BuildingImportance As String
    Public Property SClass As String

    Public Sub New(NewBuildingType As String, NewDataString As String, NewBuildingImportance As String, NewSClass As String)
        BuildingType = NewBuildingType
        DataString = NewDataString
        BuildingImportance = NewBuildingImportance
        SClass = NewSClass
    End Sub
End Class
