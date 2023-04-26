Public Class Village

    Public Property Resources As Resources
    Public ReadOnly Property VillageName As String
    Public Property VillagerCount As Integer
    Public Property Buildings As List(Of Building) = New List(Of Building)
    Public Property BuildingTemplates As New Dictionary(Of String, BuildingTemplate)
    Public Property VilString As String
    Public Property OrigVillagerCount As Integer

    Public ReadOnly ResourceToBuildings As New Dictionary(Of Integer, List(Of String))

    Public ReadOnly Property AvailableOccupations As New List(Of String)

    Public Sub New(VillType As String)
        'Name:IntitialVillagers:InitialResources
        VilString = VillType
        Resources = New Resources
        VillageName = VilString.Split(":")(0)
        OrigVillagerCount = VilString.Split(":")(1)

        For Each r As String In VilString.Split(":")(2).Split(",")
            Resources.Resources(r.Split("=")(0)) = r.Split("=")(1)
        Next

        BuildTemplateList()
    End Sub

    Private Sub BuildTemplateList()
        ' Load the building templates
        For Each dataString As String In IO.File.ReadAllLines("buildings.txt")
            Dim buildingType As String = dataString.Split(":")(0)
            Dim buildingImprt As String = dataString.Split(":")(1)
            Dim SC As String = dataString.Split(":")(4)
            Dim template As New BuildingTemplate(buildingType, dataString, buildingImprt, SC)

            If Resources.CheckLandType(dataString.Split(":")(8).Split(",")(0).Split("=")(0)) Then
                BuildingTemplates.Add(buildingType, template)
                _AvailableOccupations.Add(dataString.Split(":")(5))
            End If

        Next
        CreateMapResourceToBuildings()
    End Sub

    Private Function CreateMapResourceToBuildings() As Dictionary(Of Integer, List(Of String))
        ' Initialize the dictionary with all available resource types (excluding land types)
        For i As Integer = 0 To Resources.ResType.LandResource
            ResourceToBuildings(i) = New List(Of String)()
        Next

        ' Populate the dictionary with building types that produce each resource
        For Each buildingType As String In BuildingTemplates.Keys
            Dim building As BuildingTemplate = BuildingTemplates(buildingType)
            Dim producedResources() As String = building.DataString.Split(":")(7).Split(",")

            For Each producedResource As String In producedResources
                If Not producedResource.Equals("-1") Then ' Filter out buildings that don't return a resource
                    Dim tokens() As String = producedResource.Split(".")
                    Dim resType As Integer = Integer.Parse(tokens(1).Split("=")(0))

                    If Not ResourceToBuildings(resType).Contains(buildingType) Then
                        ResourceToBuildings(resType).Add(buildingType)
                    End If
                End If
            Next
        Next

        Return ResourceToBuildings
    End Function

    Private Function GetBuildingByResource(RsT As Resources.ResType) As List(Of String)
        Dim result As New List(Of String)

        For Each v As String In ResourceToBuildings(RsT)
            result.Add(v)
        Next

        Return result
    End Function

    Public Function GetConsumptionRates() As List(Of Resources.ResType)
        Dim ScarceResources As New List(Of Resources.ResType)
        Dim ProductionRates As New Dictionary(Of Resources.ResType, Integer)
        Dim ConsumptionRates As New Dictionary(Of Resources.ResType, Integer)

        ' Initialize consumption and production dictionaries with all resource types and set their initial values to 0
        For i As Integer = 0 To Resources.ResType.LandResource
            ProductionRates(i) = 0
            ConsumptionRates(i) = 0
        Next

        ConsumptionRates(Resources.ResType.Food) = GetFoodCount()

        ' Calculate production and consumption rates based on the buildings in the village
        For Each bt As Building In Buildings
            For Each producedResource As String In bt.ProducedResources
                Dim resType As Integer = Integer.Parse(producedResource.Split(".")(1).Split("=")(0))
                Dim productionAmount As Integer = Integer.Parse(producedResource.Split(".")(1).Split("=")(1))

                ProductionRates(resType) += productionAmount
            Next

            For Each consumedResource As String In bt.RequiredResources
                Dim resType As Integer = Integer.Parse(consumedResource.Split("=")(0))
                Dim consumptionAmount As Integer = Integer.Parse(consumedResource.Split("=")(1))

                ConsumptionRates(resType) += consumptionAmount
            Next
        Next

        ' Identify scarce resources
        For resType As Integer = 0 To Resources.ResType.LandResource
            If ConsumptionRates(resType) > ProductionRates(resType) Then
                ScarceResources.Add(resType)
            End If
        Next

        Return ScarceResources
    End Function

    ' Skip Resource is for testing only, it will build the building even if the requirments are not met
    Public Function BuildBuilding(BuildingName As String, Optional SkipResource As Boolean = False) As (Result As Boolean, Message As String)
        Dim Res As Boolean
        Dim Mes As String

        Dim Build As Building = New Building(BuildingTemplates(BuildingName).DataString)
        Dim Chk = Resources.CheckRequirements(BuildingTemplates(BuildingName).DataString.Split(":")(8).Split(",").ToList)

        Res = Chk.Result

        If Res Then
            For Each rsrc As String In Build.BuildingRequirements
                Resources.Resources(rsrc.Split("=")(0)) -= (rsrc.Split("=")(1))
            Next
            Mes = $"Built {BuildingName}"
        Else
            Mes = Chk.Message
        End If

        If SkipResource Then
            Res = True
        End If

        If Res Then
            Buildings.Add(Build)
        End If

        Return (Res, Mes)
    End Function

    Public Sub BuildResourceDrivenAllocation()
        ' 1. Get scarce resources
        Dim scarceResources As List(Of Resources.ResType) = GetConsumptionRates()

        ' 2. For each scarce resource, find the buildings that produce it
        For Each scarceResource As Resources.ResType In scarceResources
            Dim buildingTypes As List(Of String) = GetBuildingByResource(scarceResource)

            ' 3. Check if the village has enough resources to build the building
            For Each buildingType As String In buildingTypes
                Dim buildResult = BuildBuilding(buildingType)

                ' 4. If the building was successfully built, update resources and break out of the loop
                If buildResult.Result Then
                    ' Update resources
                    ' Break out of the loop to avoid building multiple buildings of the same type
                    Exit For
                Else
                    Console.Write(buildResult.Message)
                End If
            Next
        Next
    End Sub

    Private Function GetFoodCount() As Integer
        Dim Result As Integer
        Dim Classes As Integer() = {0, 0, 0, 0, 0, 0, 0}

        For Each v As Villager In AllVillagers.Values
            Classes(v.SClass) += 1
        Next

        ' We don't calculate Homeless as they would be fed by scraps and the kindness of others and wouldn't use food resources
        Result = CInt(Math.Ceiling(Classes(1) * 0.5)) ' Force the result to be rounded up
        Result += Classes(2) * 1
        Result += Classes(3) * 2
        Result += Classes(4) * 3
        Result += Classes(5) * 5
        Result += Classes(6) * 7

        Return Result
    End Function

End Class
