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

    Public Function CheckRequirements(List As List(Of String)) As Boolean
        Dim Result As Boolean = True

        For Each rr As String In List
            If Not Resources(rr.Split("=")(0)) >= rr.Split("=")(1) Then
                Result = False
                Exit For
            End If
        Next

        Return Result
    End Function

End Class
