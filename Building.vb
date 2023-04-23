' The Building class represents various types of buildings in the village simulator.
Public Class Building
    ' Building name
    Public ReadOnly Property BuildingName As String

    ' Building condition ranges from 0 to 500 (0 = Destroyed, 500 = New)
    Public Property BuildingCondition As Integer = 500
    '       This is the initial condition
    Public Property BuildingCondInt As Integer

    ' Building Importance ranges from 0 To 5 (0 = Most Important, 5 = Least Important) - This decides who gets resources first
    Public Property BuildingImportance As Integer

    ' Deterioration rate per year
    Public Property DeteriorationEachYear As Integer

    ' Maximum occupancy of the building
    Public Property MaxOccupancy As Integer

    ' Social Class to live in the building
    Public Property SClass As Integer

    ' Required occupation for managing the building (e.g., blacksmith, mason)
    Public Property RequiredOccupation As String

    ' List of required resources for the building
    Public Property RequiredResources As List(Of String)

    ' List of current resources for the building
    Private Property CurrentResources As Dictionary(Of Integer, Integer)

    ' List of resources produced by the building
    Public Property ProducedResources As List(Of String)

    ' The requirements to build this building
    Public Property BuildingRequirements As List(Of String)

    ' Occupant is the head of a household or the person with the required occupation
    Public Property MainOccupantUID As String

    ' The rest of the Villagers living in the building
    Public Property OccupantList As List(Of String)

    ' Constructor to initialize a Building object from a data string
    '
    ' Format: "BuildingName:BuildingImportance:Condition:MaxOccupancy:SClass:RequiredOccupation:RequiredResources:ProducedResources:BuildRequirements" - NONE OF THIS IS OPTIONAL, AND IT MUST BE DONE LIKE THIS.
    ' Example: House_Poor:5:0:6:1:NA:15=1:-1:26=1,16=5,17=3,18=2
    ' Example: Blacksmith:3:2:4:3:blacksmith:15=1,5=5,1=2:5.1=16.10,5.2=13.5,5.2=14.6:26=1,16=10,17=7,18=4,3=2
    ' 
    ' BuildingName: Just a String
    ' BuildingImportance: Enum with values from 0 to 5 - Set's who gets resources first
    '   0 = most important to 5 = Least important
    ' Condition: Set using SetCondition method using the following Enum (Number only)
    '   CondEnums (Use Number): weak (Lowest Class house), Poor (Lower class to middle houses), Average (Middle to Upper class houses, most businesses), Good (Rich And Noble Houses)
    '   CondEnums (Cont)      : Strong (Things that aren't buildings, like parks and monuments), Fortified (Castles, Forts, and Military stuff)
    ' MaxOccupancy: Integer value
    '   Is not worker count, is people who live in the building.
    '   Even businesses can have people living in them, this is Fantasy Medieval, A blacksmith would live above his forge, farmers live on farms, and so on and so forth.
    ' SClass: What level of Social class is required to live in this building, if the building doesn't house people, put -1 (For example, a park or a quarry)
    '  SClasses (Use Number): Homeless, Poor, Lower, Middle, Upper, Rich, Noble
    ' RequiredOccupation: String, whatever this string is, is what the program will look for when looking for an appropriate person, Ex. "blacksmith", "butcher", "hunter". Set to "NA" if there isn't a required occupation.
    ' RequiredResources: Comma-separated list of resources the building consumes
    '   Format: "ResEnum=ResAmntNeeded" (See Resource Enums below)
    '   ResEnum is an index for a list of resources
    '   Every building must consume "Maintenance" (ResEnum 15)
    '   Example: "5=10,15=5"
    '   Other than Maintainance, homes do not need resources
    ' ProducedResources: Comma-separated list of resources the building produces
    '   Format: "ReqResEnum.ReqAmnt=ProdResEnum.ProdAmnt" (See Resource Enums below)
    '   ReqResEnum is the enum number for the required Resource, ReqAmnt is how much, ProdResEnum is the Produced resource enum number, ProdAmnt is how much product is produced
    '   This is a one item makes one item deal, 1 raw wood makes 2 wood (Or whatever), NOT 1 raw wood and 1 tool makes wood or 1 raw wood makes 2 wood and 2 food.
    '   Use "-1" if the building does not produce any resources
    '   Example: "0.1=1.4"
    '   Homes do not produce resources
    '   If the building doesn't consume resources (Such as a farm, a lumberjack, a mine or the like) when producing items, put a -1 for the first part
    '   Example: "-1=0.10"
    ' BuildRequirements: The required resources to build this building, MUST INCLUDE A LAND TYPE THAT IS APPROPRIATE, Mostly just use stable land, the rest are for special buildings like a quarry would need stone.
    '   Format: "ResEnum=ResAmntNeeded" (See Resource Enums below)
    '   Other than the Land Type Only uses the BuildMat's
    '   Misc is stuff like hinges and braces, small is like bricks, medium is like doors and planks, large is like frames
    ' Resource Enums (Use number): RawWood, Wood, RawStone, Stone, RawMetal, Metal, RawFabric, Fabric, RawFood, Food, RawValubles, Valubles, Medical, Tools, Weapons
    ' Resource Enums (Cont)      : Maintenance, BuildMatMisc, BuildMatSmall, BuildMatMedium, BuildMatLarge, AlcoQuaLow, AlcoQuaHigh
    ' Resource Enums (Cont*)     : LandRiver, LandOcean, LandFertile, LandGrass, LandStable, LandStone, LandGold, LandDiamond, LandIron, 
    '  * These are only used for BuildingRequirements, they are still apart of the Resource Enum, but are not produced by any building, nor are they consumed outside of the original building stage.
    '  * ONE IS REQUIRED FOR EVERY BUILDING (even houses) (And JUST ONE UNIT OF) as it is what type of terrain the building is built on, LandStable is just regular land, not good for much other than building on
    '  * LandGold however would be a place that a gold mine would be built on, LandOcean would be good for a dock, LandRiver would be good for a fishing hut and so on and so forth
    Public Sub New(Data As String)
        Dim D() As String = Data.Split(":")
        BuildingName = D(0)
        BuildingImportance = D(1)
        SetCondition(D(2))
        BuildingCondInt = D(2)
        MaxOccupancy = D(3)
        SClass = D(4)
        RequiredOccupation = D(5)

        RequiredResources = D(6).Split(",").ToList
        If Not D(7) = "-1" Then
            ProducedResources = D(7).Split(",").ToList
        Else
            ProducedResources = New List(Of String)
        End If
        BuildingRequirements = D(8).Split(",").ToList

        ' ToDo: An event for when the building is built, such as a clerics hut increasing life expectancy - Requires thought
    End Sub

    ' This method populates the building with the main occupant and a list of other occupants.
    ' MainUID: UID of the main occupant (head of the household or person with the occupation for the building)
    ' ListOfOccupants: List of UIDs of other occupants living in the building
    Public Sub PopulateBuilding(MainUID As String, ListOfOccupants As List(Of String))
        MainOccupantUID = MainUID
        OccupantList = ListOfOccupants
    End Sub

    ' This method depopulates the building, removing all occupants (main and others).
    Public Sub DepopulateBuilding()
        MainOccupantUID = Nothing
        OccupantList = Nothing
    End Sub

    ' This method removes a specific occupant from the building, using their UID.
    ' UID: The UID of the occupant to be removed
    Public Sub RemoveOccupant(UID As String)
        If MainOccupantUID = UID Then
            MainOccupantUID = Nothing
        Else
            OccupantList.Remove(UID)
        End If
    End Sub

    ' DamageBuilding reduces the BuildingCondition by the specified damage amount
    Public Sub DamageBuilding(Dmg As Integer)
        BuildingCondition -= Dmg
    End Sub

    ' RepairBuilding increases the BuildingCondition by the specified repair amount
    Public Sub RepairBuilding(Rep As Integer)
        BuildingCondition += Rep
    End Sub

    ' SetCondition sets the deterioration rate based on the building condition category
    Public Sub SetCondition(Str As BCond)
        Select Case Str
            Case BCond.Weak ' Building should last for ~10 years
                DeteriorationEachYear = 50
            Case BCond.Poor ' Building should last for ~15 years
                DeteriorationEachYear = 30
            Case BCond.Average ' Building should last for ~25 years
                DeteriorationEachYear = 20
            Case BCond.Good ' Building should last for ~40 years
                DeteriorationEachYear = 12
            Case BCond.Strong ' Building should last for ~80 years
                DeteriorationEachYear = 7
            Case BCond.Fortified ' Building should last for ~100 years
                DeteriorationEachYear = 5
        End Select
    End Sub

    ' TakeResource consumes required resources for the building, reducing its condition if resources are insufficient
    Public Sub TakeResource()
        Dim aamnt As Integer ' Available Amount
        Dim ramnt As Integer ' Required Amount
        Dim samnt As Integer ' Stored Amount
        Dim Res As Integer   ' The resource
        CurrentResources = New Dictionary(Of Integer, Integer)
        ' Format: "ResEnum=ResAmntNeeded"
        For Each l As String In RequiredResources
            Res = Integer.Parse(l.Split("=")(0))
            ramnt = Integer.Parse(l.Split("=")(1))

            aamnt = VilResources.Resources(Res)

            If aamnt > ramnt Then
                CurrentResources.Add(Res, ramnt)
                VilResources.Resources(Res) -= ramnt
            Else
                samnt = ramnt - aamnt
                DamageBuilding((DeteriorationEachYear * 0.1) * samnt)
                CurrentResources.Add(Res, ramnt - samnt)
                VilResources.Resources(Res) = 0
            End If
        Next
    End Sub

    ' Calculate adjusted competency based on consumed resources
    Private Function AdjustedCompetency(ExpectedAmount As Integer) As Integer
        Dim Result As Integer

        If Not IsNothing(GetVillagerByUID(MainOccupantUID)) Then
            Select Case GetVillagerByUID(MainOccupantUID).Competency
                Case 0
                    Result = ExpectedAmount * 0.1
                Case 1
                    Result = ExpectedAmount * 0.5
                Case 2
                    Result = ExpectedAmount * 1
                Case 3
                    Result = ExpectedAmount * 1.5
                Case 4
                    Result = ExpectedAmount * 2
                Case Else
                    Result = 0
            End Select
        Else
            Result = 0
        End If

        Return Result
    End Function

    ' Make Resources
    Public Sub MakeResource()
        Dim ResInP As Integer  ' The resource consumed
        Dim ramntI As Integer ' Required Amount In
        Dim ResOut As Integer ' The resource produced
        Dim ramntO As Integer ' Required Amount Out

        Dim maxProduced As Integer
        ' Format: "ReqResEnum=ReqAmnt.ProdResEnum=ProdAmnt"
        For Each l As String In ProducedResources
            If Not ResInP = -1 Then

                'in
                ResInP = Integer.Parse(l.Split(".")(0).Split("=")(0))
                ramntI = Integer.Parse(l.Split(".")(0).Split("=")(1))
                'out
                ResOut = Integer.Parse(l.Split(".")(1).Split("=")(0))
                ramntO = Integer.Parse(l.Split(".")(1).Split("=")(1))

                If CurrentResources(ResInP) >= ramntI Then
                    CurrentResources(ResInP) -= ramntI
                    VilResources.Resources(ResOut) += AdjustedCompetency(ramntO)
                Else
                    maxProduced = CurrentResources(ResInP) \ ramntI
                    CurrentResources(ResInP) = 0
                    VilResources.Resources(ResOut) += AdjustedCompetency(maxProduced * ramntO)
                End If
            Else
                'in
                ResInP = 31
                ramntI = 0
                'out
                ResOut = Integer.Parse(l.Split(".")(1).Split("=")(0))
                ramntO = Integer.Parse(l.Split(".")(1).Split("=")(1))

                VilResources.Resources(ResOut) += AdjustedCompetency(ramntO)
                End If
        Next
    End Sub

    ' IsOk checks if the building is still functional (BuildingCondition > 0)
    Public Function IsOk() As Boolean
        Dim result As Boolean = True

        If BuildingCondition <= 0 Then
            result = False
        End If

        Return result
    End Function

    '   For Each r As String In BuildingRequirements
    '       VilResources.Resources(r.Split("=")(0)) -= r.Split("=")(1)
    '   Next

    ' Checks to see if a building can be built
    Public Function CheckBuildRequirements() As Boolean
        Return VilResources.CheckRequirements(RequiredResources)
    End Function
End Class
