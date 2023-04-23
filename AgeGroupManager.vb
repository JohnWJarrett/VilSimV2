Public Class AgeGroupManager
    Private ReadOnly _ageGroups As New Dictionary(Of String, (Name As String, Tag As String, MaxAge As Integer, Height As (TMin As Double, AMin As Double, AMax As Double, TMax As Double), Weight As (TMin As Double, AMin As Double, AMax As Double, TMax As Double), Bust As (Min As Double, Max As Double)))

    Public Sub New(ageGroupsFilePath As String)
        LoadAgeGroups(ageGroupsFilePath)
    End Sub

    Private Sub LoadAgeGroups(ageGroupsFilePath As String)
        ' Read the age group data from the text file and add it to the _ageGroups dictionary
        Dim ageGroupsLines = IO.File.ReadAllLines(ageGroupsFilePath)

        For Each ageGroupLine In ageGroupsLines
            Dim ageGroupFields = ageGroupLine.Split(","c)
            Dim ageGroupName = ageGroupFields(0).Trim
            Dim ageGroupTag = ageGroupFields(1).Trim
            Dim ageGroupMaxAge = Integer.Parse(ageGroupFields(2))

            ' Group the height properties together in a tuple
            Dim heightProperties = (TMin:=Double.Parse(ageGroupFields(3)),
                                    AMin:=Double.Parse(ageGroupFields(4)),
                                    AMax:=Double.Parse(ageGroupFields(5)),
                                    TMax:=Double.Parse(ageGroupFields(6)))

            ' Group the weight properties together in a tuple
            Dim weightProperties = (TMin:=Double.Parse(ageGroupFields(7)),
                                    AMin:=Double.Parse(ageGroupFields(8)),
                                    AMax:=Double.Parse(ageGroupFields(9)),
                                    TMax:=Double.Parse(ageGroupFields(10)))

            ' Group the bust properties together in a tuple
            Dim bustProperties = (Min:=Double.Parse(ageGroupFields(11)),
                                  Max:=Double.Parse(ageGroupFields(12)))

            _ageGroups.Add(ageGroupName, (ageGroupName, ageGroupTag, ageGroupMaxAge, heightProperties, weightProperties, bustProperties))
        Next
    End Sub

    Public ReadOnly Property AgeGroup(name As String) As (Name As String, Tag As String, MaxAge As Integer, HeightTMin As Double, HeightAMin As Double, HeightAMax As Double, HeightTMax As Double, WeightTMin As Double, WeightAMin As Double, WeightAMax As Double, WeightTMax As Double, BustMin As Double, BustMax As Double)
        Get
            If _ageGroups.ContainsKey(name) Then
                Dim group = _ageGroups(name)
                Return (group.Name, group.Tag, group.MaxAge, group.Height.TMin, group.Height.AMin, group.Height.AMax, group.Height.TMax, group.Weight.TMin, group.Weight.AMin, group.Weight.AMax, group.Weight.TMax, group.Bust.Min, group.Bust.Max)
            Else
                Throw New ArgumentException($"Invalid age group: {name}")
            End If
        End Get
    End Property

    Public Function GetAgeGroupByAge(age As Integer) As (Name As String, Tag As String, MaxAge As Integer, HeightTMin As Double, HeightAMin As Double, HeightAMax As Double, HeightTMax As Double, WeightTMin As Double, WeightAMin As Double, WeightAMax As Double, WeightTMax As Double, BustMin As Double, BustMax As Double)
        Dim ageGroupName As String = Nothing

        ' Find the age group that matches the given age
        For Each kvp In _ageGroups
            If age <= kvp.Value.MaxAge Then
                ageGroupName = kvp.Key
                Exit For
            End If
        Next

        If ageGroupName IsNot Nothing Then
            ' Return the age group's properties
            Dim group = _ageGroups(ageGroupName)
            Return (group.Name, group.Tag, group.MaxAge, group.Height.TMin, group.Height.AMin, group.Height.AMax, group.Height.TMax, group.Weight.TMin, group.Weight.AMin, group.Weight.AMax, group.Weight.TMax, group.Bust.Min, group.Bust.Max)
        Else
            Throw New ArgumentException($"No age group found for age {age}")
        End If
    End Function

    Public Function GetBust(Bust As Integer) As String
        Dim Result As String
        Select Case Bust
            Case 0
                Result = "(breasts:0)"
            Case 1
                Result = "(breasts:0.25)"
            Case 2
                Result = "(breasts:0.75)"
            Case 3
                Result = "(breasts:1)"
            Case 4
                Result = "(breasts:1.5)"
            Case Else
                Result = ""
        End Select
        Return Result
    End Function

    Function GenerateBust(age As Integer) As Integer
        Dim ageGroup = GetAgeGroupByAge(age)
        Return RNGen(ageGroup.BustMin, ageGroup.BustMax)
    End Function

    Public Function GetWeightRange(age As Integer, weight As Double) As String
        Dim tMin As Double = GetAgeGroupByAge(age).WeightTMin
        Dim tMax As Double = GetAgeGroupByAge(age).WeightTMax
        Dim aMin As Double = GetAgeGroupByAge(age).WeightAMin
        Dim aMax As Double = GetAgeGroupByAge(age).WeightAMax

        Dim UW As Double = tMin + ((aMin - tMin) / 2)
        Dim AvgA As Double = aMin + ((aMax - aMin) / 4)
        Dim AvgB As Double = aMax - ((aMax - aMin) / 4)
        Dim OW As Double = tMax - ((tMax - aMax) / 2)

        If weight < UW Then
            Return "(skinny:1.5)"
        ElseIf weight >= UW And weight <= aMin Then
            Return "(skinny:1)"
        ElseIf weight > aMin And weight < AvgA Then
            Return "(skinny:0.5)"
        ElseIf weight >= AvgA And weight <= AvgB Then
            Return "(skinny:0)"
        ElseIf weight > AvgB And weight < aMax Then
            Return "(fat:0.5)"
        ElseIf weight >= aMax And weight <= OW Then
            Return "(fat:1)"
        ElseIf weight > OW Then
            Return "(fat:1.5)"
        Else
            Return "Unknown weight range"
        End If
    End Function

    Function GenerateWeight(age As Integer) As Double
        Dim ageGroup = GetAgeGroupByAge(age)
        Dim meanWeight = (ageGroup.WeightAMax + ageGroup.WeightAMin) / 2
        Dim stdDev = (ageGroup.WeightAMax - ageGroup.WeightAMin) / 2 ' adjust this factor to control how spread out the weights are
        Dim weight As Double
        Do
            weight = nRan.NextGaussian(meanWeight, stdDev * nRan.NextDouble() * 1.0 + 0.5)
        Loop Until weight >= ageGroup.WeightTMin AndAlso weight <= ageGroup.WeightTMax
        Return weight
    End Function

    Public Function GetHeightRange(age As Integer, Height As Double) As String
        Dim tMin As Double = GetAgeGroupByAge(age).HeightTMin
        Dim tMax As Double = GetAgeGroupByAge(age).HeightTMax
        Dim aMin As Double = GetAgeGroupByAge(age).HeightAMin
        Dim aMax As Double = GetAgeGroupByAge(age).HeightAMax

        Dim SHRT As Double = tMin + ((aMin - tMin) / 2)
        Dim AvgA As Double = aMin + ((aMax - aMin) / 4)
        Dim AvgB As Double = aMax - ((aMax - aMin) / 4)
        Dim TALL As Double = tMax - ((tMax - aMax) / 2)

        If Height < SHRT Then
            Return "(short:1.5)"
        ElseIf Height >= SHRT And Height <= aMin Then
            Return "(short:1)"
        ElseIf Height > aMin And Height < AvgA Then
            Return "(short:0.5)"
        ElseIf Height >= AvgA And Height <= AvgB Then
            Return "(tall:0)"
        ElseIf Height > AvgB And Height < aMax Then
            Return "(tall:0.5)"
        ElseIf Height >= aMax And Height <= TALL Then
            Return "(tall:1)"
        ElseIf Height > TALL Then
            Return "(tall:1.5)"
        Else
            Return "Unknown Height range"
        End If
    End Function

    Function GenerateHeight(age As Integer) As Double
        Dim ageGroup = GetAgeGroupByAge(age)
        Dim meanHeight = (ageGroup.HeightAMax + ageGroup.HeightAMin) / 2
        Dim stdDev = (ageGroup.HeightAMax - ageGroup.HeightAMin) / 2 ' adjust this factor to control how spread out the Heights are
        Dim Height As Double
        Dim rnd As New Random()
        Do
            Height = rnd.NextGaussian(meanHeight, stdDev * rnd.NextDouble() * 1.0 + 0.5)
        Loop Until Height >= ageGroup.HeightTMin AndAlso Height <= ageGroup.HeightTMax
        Return Height
    End Function
End Class