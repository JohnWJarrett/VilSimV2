Public Module VillagerActions
    Public Sub AgeUp(CurrentYear As Integer)
        For Each v As Villager In GetVillagerList()
            If Not v.AgeUp() Then
                RemoveVillager(v, CurrentYear)
            End If
        Next
    End Sub

    Public Sub AddNewVillagers(c As Integer)
        Dim x1 As Villager
        For i = 0 To c
            x1 = New Villager(RNGen(18, 40))
            MyVillage.VillagerCount += 1
            AllVillagers.Add(x1.UID, x1)
        Next
    End Sub

    ' When reaching the age of [MinAgeForJob] get an occupation, most likely the same as their father, but it can be different...
    ' So some probability calculation, like 60% chance of doing the same thing as your father, maybe that goes down with however many boys are in the family
    ' Yes, yes, I know, gender equallity, but this IS a medieval simulator...
    Public Sub GetJob(Vlgr As Villager, Vlg As Village)
        If Vlgr.Age >= MinAgeForJob AndAlso Vlgr.Occupation Is Nothing Then
            Dim availableOccupations As List(Of String) = Vlg.AvailableOccupations()
            Dim father As Villager = GetVillagerByUID(Vlgr.FatherUID)

            If father IsNot Nothing AndAlso father.Occupation IsNot Nothing Then
                ' 60% chance of following the father's occupation
                If RNGen(1, 100) <= 60 AndAlso availableOccupations.Contains(father.Occupation) Then
                    Vlgr.Occupation = father.Occupation
                Else
                    ' Choose a random occupation from the available ones
                    Vlgr.Occupation = availableOccupations(RNGen(0, availableOccupations.Count - 1))
                End If
            Else
                ' Choose a random occupation from the available ones if the father's occupation is unknown or not available
                Vlgr.Occupation = availableOccupations(RNGen(0, availableOccupations.Count - 1))
            End If
        End If
    End Sub


    ' Any one over the age of [MinAgeForRelation] can start dating anyone else that is in the acceptable age range
    ' [MinAgeForRelation] +- [RangeForRelations] (So if the Range is 3, then someone who is 20 can date anyone from the age of 17 to 23
    ' Relationships are always for "Love" and not "Old Timey Married for money ways". So the CheckMatch must be true for it to happen.
    ' this will nned to create a new "Relationship" object, you can get the score by calling 
    ' "CheckMatch(VilA As Villager, VilB As Villager) As (WillMatch As Boolean, FScore As Double, PScore As Double, CScore As Integer, AScore As Integer)"
    ' Where FScore is the ultimate calculated score, WillMatch says if they will match or not based on several factors.
    Public Sub StartRelationship(Vlgr As Villager, currentYear As Integer)
        If Vlgr.GenderChar = "m" AndAlso Vlgr.Age >= MinAgeForRelation AndAlso Not Vlgr.InRelationship Then
            Dim potentialPartners As New List(Of Villager)

            For Each potentialVillager As Villager In GetVillagerList()
                If potentialVillager.GenderChar = "f" AndAlso Not potentialVillager.InRelationship _
                AndAlso Math.Abs(Vlgr.Age - potentialVillager.Age) <= RangeForRelations Then
                    potentialPartners.Add(potentialVillager)
                End If
            Next

            For Each potentialPartner As Villager In potentialPartners
                Dim matchResult = CheckMatch(Vlgr, potentialPartner)
                If matchResult.WillMatch Then
                    Dim newRelationship As New Relationship(Vlgr.UID, potentialPartner.UID, currentYear, matchResult.FScore)
                    Vlgr.InRelationship = True
                    potentialPartner.InRelationship = True

                    Vlgr.AddRelationship(newRelationship.GetRelationshipUID.Full)
                    potentialPartner.AddRelationship(newRelationship.GetRelationshipUID.Full)
                    newRelationship.EventList.Add($"{currentYear}: Started Dating.")
                    AllRelationships.Add(newRelationship.GetRelationshipUID.Full, newRelationship)
                    Exit For
                End If
            Next
        End If
    End Sub



    ' Anyone in a relationship that the youngest is => the [MinMarriageAge] can get married if they want
    ' Basically, a 10% chance for each year they have been together starting from their MatchScore (Not derectly, as the score at max is usually a low number, 
    '  also it's not set in stone as it depends on how many traits a villager can possibly have which can be changed by the user.)
    ' so if they started dating at the age of 15, by the time they are 20, they will have a 50/50 chance
    ' Even if they only started dating at the age of 20, they still have a 10% chance... which means it will still happen.
    ' Also, anyone of a upper or higher social class has a chance of being married for money, or power, or... well, you know how this works...
    ' I am thinking there needs to be a chance for two people to be forced into marriage, upper and higher is only 3 social classes, Upper, Rich and Noble
    ' So With Upper to Upper I think it would be a low chance that they will be forced into marriage, For upper to Rich, it will be lower, Upper to Noble even more so
    ' And then with Rich to rich, it will be higher, then Rich to Noble will be lower than Rich to Rich but higher than Upper to Upper... And obvioulsy, Noble to Noble will be the highest chance.
    ' Does any of this make sense? To me... sort of...

    Public Sub MarryVillagers(Vlg As Village, CurrentYear As Integer)
        Dim relationships = AllRelationshipsAsList()

        For Each rel In relationships
            Dim maleVillager = GetVillagerByUID(rel.GetRelationshipUID().Male)
            Dim femaleVillager = GetVillagerByUID(rel.GetRelationshipUID().Female)

            Dim youngestAge = Math.Min(maleVillager.Age, femaleVillager.Age)

            If youngestAge >= MinMarriageAge Then
                Dim relationshipDuration = CurrentYear - rel.RStartYear
                Dim marriageChance = 0.1 * relationshipDuration + rel.MatchScore / 10

                If marriageChance > Rnd() Then
                    Dim buildingType = "House_" & maleVillager.SClass.ToString()
                    If Vlg.BuildBuilding(buildingType).Result Then
                        rel.EventList.Add($"{CurrentYear}: Got married.")
                        rel.IsMarried = True
                    End If
                End If
            End If
        Next

        For Each maleVillager In GetVillagerList.Where(Function(v) v.GenderChar = "m" AndAlso v.Age >= MinMarriageAge AndAlso Not v.InRelationship)

            Dim forcedMarriageChance As Double

            Select Case maleVillager.SClass
                Case 3 ' Upper class
                    forcedMarriageChance = 0.1
                Case 4 ' Rich class
                    forcedMarriageChance = 0.4
                Case 5 ' Noble class
                    forcedMarriageChance = 0.8
                Case Else
                    Continue For
            End Select

            If forcedMarriageChance > Rnd() Then
                Dim targetFemaleClass As Integer
                Dim rng As Double = Rnd()

                Select Case True
                    Case maleVillager.SClass = 3 And rng < 0.05
                        targetFemaleClass = 5
                    Case maleVillager.SClass = 3 And rng < 0.25
                        targetFemaleClass = 4
                    Case maleVillager.SClass = 3 And rng < 0.5
                        targetFemaleClass = 3
                    Case maleVillager.SClass = 3 And rng > 0.8
                        targetFemaleClass = 2
                    Case maleVillager.SClass = 4 And rng < 0.1
                        targetFemaleClass = 5
                    Case maleVillager.SClass = 4 And rng < 0.5
                        targetFemaleClass = 4
                    Case maleVillager.SClass = 4 And rng > 0.8
                        targetFemaleClass = 3
                    Case maleVillager.SClass = 5 And rng < 0.5
                        targetFemaleClass = 5
                    Case maleVillager.SClass = 5 And rng > 0.8
                        targetFemaleClass = 4
                    Case maleVillager.SClass = 5 And rng > 0.95
                        targetFemaleClass = 3
                    Case Else
                        Continue For
                End Select

                Dim availableFemales = GetVillagerList.Where(Function(v) v.GenderChar = "f" AndAlso v.SClass = targetFemaleClass AndAlso Not v.InRelationship).ToList()

                If availableFemales.Count > 0 Then
                    Dim selectedFemale = availableFemales(Rnd() * (availableFemales.Count - 1))
                    Dim buildingType = "House_" & maleVillager.SClass.ToString()

                    If Vlg.BuildBuilding(buildingType).Result Then
                        Dim newRelationship = New Relationship(maleVillager.UID, selectedFemale.UID, CurrentYear, 0)
                        newRelationship.EventList.Add($"{CurrentYear}: Forced to marry for social/political reasons.")
                        newRelationship.IsMarried = True
                        maleVillager.AddRelationship(newRelationship.GetRelationshipUID.Full)
                        selectedFemale.AddRelationship(newRelationship.GetRelationshipUID.Full)
                        AllRelationships.Add(newRelationship.GetRelationshipUID.Full, newRelationship)
                    End If
                End If
            End If
        Next
    End Sub

    ' Anyone female over the age of [MinChildbirthAge] and under the age of [MaxChildbirthAge] has a chance of getting pregnant
    ' Though it will be a low chance, say, 2% chance, then that goes up to 10% if they are over the age of [MinSACBAge], 
    ' And then up to 45% chance if they are in a relationship And up to 90% chance if they are married
    ' If they are in a relationship, give them a chance based on their relationship score
    Public Sub HaveChildren()
        Dim villagers As List(Of Villager) = GetVillagerList()
        Dim femaleVillagers As List(Of Villager) = villagers.FindAll(Function(v) v.Gender.AsChar = "f" AndAlso v.Age >= MinChildbirthAge AndAlso v.Age <= MaxChildbirthAge)

        For Each femaleVillager As Villager In femaleVillagers
            Dim relationshipChance As Double = 0.02 ' Default 2% chance

            If femaleVillager.Age >= MinSACBAge Then
                relationshipChance = 0.1 ' 10% chance
            End If

            Dim relationships As List(Of Relationship) = GetAllRelationshipsBy(RelInfo.FemaleUID, femaleVillager.UID)

            If relationships.Count > 0 Then
                Dim relationship As Relationship = relationships(0)
                Dim relationshipScore As Double = relationship.MatchScore

                If relationship.IsMarried Then
                    relationshipChance = 0.9 ' 90% chance
                ElseIf relationshipScore >= 5 Then
                    relationshipChance = 0.45 ' 45% chance
                End If
            End If

            ' Check if the female villager will get pregnant based on the chance calculated
            If Rnd() <= relationshipChance Then
                ' TODO: Handle the pregnancy and childbirth process
            End If
        Next
    End Sub


    ' A chance that is based on the relationships Score, the higher the score, the less of a chance.
    ' Though certain events should auto kill a relationship, such as infidelity and Death...
    Public Sub EndRelationship(CurrentYear As Integer)
        ' Iterate through all the relationships
        For Each rel In AllRelationshipsAsList()

            ' Check if relationship is ongoing
            If rel.REndYear = -9999 Then

                ' Check if any partner is dead
                Dim maleUID As String = rel.GetRelationshipUID().Male
                Dim femaleUID As String = rel.GetRelationshipUID().Female
                Dim maleVillager As Villager = GetVillagerByUID(maleUID)
                Dim femaleVillager As Villager = GetVillagerByUID(femaleUID)

                If maleVillager Is Nothing Or femaleVillager Is Nothing Then
                    ' One of the partners is dead, end the relationship
                    rel.REndYear = CurrentYear
                    rel.EventList.Add($"{CurrentYear}: Relationship ended due to death.")
                    Continue For
                End If

                ' Check if the relationship score is below 90%
                Dim relationshipScore As Double = rel.MatchScore
                Dim yearsTogether As Integer = CurrentYear - rel.RStartYear
                Dim endRelationshipChance As Double
                Dim HClass As Integer

                If maleVillager.SClass > femaleVillager.SClass Then
                    HClass = maleVillager.SClass
                Else
                    HClass = femaleVillager.SClass
                End If

                If HClass = 5 Then
                    relationshipScore += 2
                ElseIf HClass = 6 Then
                    relationshipScore += 4
                End If

                Select Case True
                    Case relationshipScore >= 9
                        Continue For
                    Case relationshipScore >= 8 AndAlso yearsTogether >= 20
                        endRelationshipChance = 0.05
                    Case relationshipScore >= 7 AndAlso yearsTogether >= 15
                        endRelationshipChance = 0.05
                    Case relationshipScore >= 6 AndAlso yearsTogether >= 10
                        endRelationshipChance = 0.05
                    Case relationshipScore >= 5 AndAlso yearsTogether >= 5
                        endRelationshipChance = 0.05
                    Case relationshipScore >= 4
                        endRelationshipChance = 0.1
                    Case relationshipScore >= 3
                        endRelationshipChance = 0.25
                    Case relationshipScore >= 2
                        endRelationshipChance = 0.5
                    Case relationshipScore >= 1
                        endRelationshipChance = 0.75
                    Case Else
                        endRelationshipChance = 1
                End Select

                ' End relationship with the calculated chance
                If Rnd() < endRelationshipChance Then
                    rel.REndYear = CurrentYear
                    rel.EventList.Add($"{CurrentYear}: Relationship ended due to unhappiness.")
                    Continue For
                End If
            End If
        Next
    End Sub

    ' A chance based on how many years a villager has been [IsHungry] or if they are homeless and under a certain age...
    Public Sub MoveOut()
        Dim villagersToMoveOut As New List(Of Villager)

        For Each v As Villager In GetVillagerList()
            If v.Age >= MinAgeForJob AndAlso v.Age <= MaxAgeToLeaveVillage Then
                If v.SClass = 0 OrElse v.IsHungry Then
                    Dim moveOutChance As Double = CalculateMoveOutChance(v.YearsHomeless, v.YearsHungry)

                    If moveOutChance > Rnd() Then
                        villagersToMoveOut.Add(v)
                    End If
                End If
            End If
        Next

        For Each villagerToMoveOut As Villager In villagersToMoveOut
            RemoveVillager(villagerToMoveOut, False)
        Next
    End Sub

    Private Function CalculateMoveOutChance(yearsHomeless As Integer, yearsHungry As Integer) As Double
        Const HomelessWeight As Double = 0.05
        Const HungryWeight As Double = 0.03

        Dim moveOutChance As Double = (yearsHomeless * HomelessWeight) + (yearsHungry * HungryWeight)

        ' Clamp the move-out chance between 0 and 1
        moveOutChance = Math.Max(0, Math.Min(1, moveOutChance))

        Return moveOutChance
    End Function

    ' Random chance thing
    Public Sub GetInjured(vlgr As Villager)
        ' Calculate the injury chance based on age and competency
        Dim injuryChance As Double
        Dim ageFactor As Double = NGaus(vlgr.Age, 10)
        Dim competencyFactor As Double = vlgr.Competency / 4
        injuryChance = (1 - competencyFactor) * ageFactor

        ' Roll for injury
        If Rnd() < injuryChance Then
            ' Set the injured flag and assign a random number of years for injury duration
            vlgr.IsSickOrInjured = True
            vlgr.YearsInjured = CInt(Math.Ceiling(3 * BiasedRandomTowardsOne()))
        End If
    End Sub

    Private Function BiasedRandomTowardsOne() As Double
        Dim randomNumber As Double = Rnd()
        Return Math.Sqrt(randomNumber)
    End Function

End Module
