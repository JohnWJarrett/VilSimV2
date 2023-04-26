Public Class Resources

    Public Enum ResType
        RawWood
        Wood
        RawStone
        Stone
        RawMetal
        Metal
        RawFabric
        Fabric
        RawFood
        Food
        RawValubles
        Valubles
        Medical
        Tools
        Weapons
        Maintenance
        BuildMatMisc
        BuildMatSmall
        BuildMatMedium
        BuildMatLarge
        AlcoQuaLow
        AlcoQuaHigh
        LandRiver
        LandOcean
        LandFertile
        LandGrass
        LandStable
        LandStone
        LandGold
        LandDiamond
        LandIron
        LandResource
    End Enum

    Private ReadOnly _resources As Integer()

    Private ReadOnly _AvailableLandTypes As List(Of Integer)

    Public Sub New()
        ReDim _resources([Enum].GetValues(GetType(ResType)).Length - 1)
    End Sub

    Public Property Resources(res As ResType) As Integer
        Get
            Return _resources(res)
        End Get
        Set(value As Integer)
            _resources(res) = value
        End Set
    End Property

    Public Function CheckRequirements(List As List(Of String)) As (Result As Boolean, Message As String)
        Dim MyResult As Boolean = True
        Dim Mes As String = Nothing
        For Each rr As String In List
            If Resources(rr.Split("=")(0)) < rr.Split("=")(1) Then
                Dim keyValue() As String = rr.Split("=")
                Dim key As String = keyValue(0)
                Dim requiredValue As Integer = Integer.Parse(keyValue(1))
                Mes = $"Resource: {key}, Required: {requiredValue}, Available: {Resources(key)}"
                MyResult = False
                Exit For
            End If
        Next

        Return (MyResult, Mes)
    End Function

    Public Function GetAvailableLandTypes() As List(Of Integer)
        For i = 22 To 30
            If _resources(i) > 0 Then
                _AvailableLandTypes.Add(i)
            End If
        Next

        Return _AvailableLandTypes
    End Function

    Public Function CheckLandType(ReqString As Integer) As Boolean
        Return _AvailableLandTypes.Contains(ReqString)
    End Function

End Class
