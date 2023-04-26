Imports System.Runtime.CompilerServices

Public Module tools

    Public ReadOnly nRan As New Random
    Private Const rScale As Integer = 10000
    Private Const MaxRandValue As Integer = Integer.MaxValue \ rScale

    ReadOnly Property RandomLines As New LineManager("strings.txt")
    ReadOnly Property AGroup As New AgeGroupManager("agroups.txt")

    ' extension method for the Random class to generate a random number from a Gaussian distribution
    <Extension()>
    Public Function NextGaussian(rnd As Random, mean As Double, stdDev As Double) As Double
        Dim u1 = rnd.NextDouble()
        Dim u2 = rnd.NextDouble()
        Dim z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2)
        Return mean + stdDev * z
    End Function

    Public Function NGaus(mean As Double, stdDev As Double) As Double
        Return nRan.NextGaussian(mean, stdDev)
    End Function

    Public Function TruncNorm(mean As Double, stdDev As Double, minVal As Double, maxVal As Double) As Double
        Dim x As Double
        Do
            x = NGaus(mean, stdDev)
        Loop Until x >= minVal AndAlso x <= maxVal
        Return x
    End Function

    Public Function GetRandomFeature(Feature As RFeatures) As String
        Dim Result As String

        Select Case Feature
            Case 0
                Result = eyeColors(RNGen(eyeColors.Count - 1))
            Case 1
                Result = hairLengthsM(RNGen(hairLengthsM.Count - 1))
            Case 2
                Result = hairLengthsF(RNGen(hairLengthsF.Count - 1))
            Case 3
                Result = hairColors(RNGen(hairColors.Count - 1))
            Case 4
                Result = hairStyles(RNGen(hairStyles.Count - 1))
            Case 5
                Result = faceShapes(RNGen(faceShapes.Count - 1))
            Case 6
                Result = noseShapes(RNGen(noseShapes.Count - 1))
            Case 7
                Result = lipTypes(RNGen(lipTypes.Count - 1))
            Case 8
                Result = skinTypes(RNGen(skinTypes.Count - 1))
            Case Else
                Result = "ERROR"
        End Select

        Return Result
    End Function

    Public Function RNGen(Min As Integer, Max As Integer) As Integer
        Dim result As Integer
        Try
            result = nRan.Next((Min * rScale), ((Max + 1) * rScale)) \ rScale
        Catch ex As Exception
            result = 10
        End Try
        Return result
    End Function

    Public Function RNGenD() As Double
        Dim result As Double
        Try
            result = nRan.Next(-10, 10) / 10
        Catch ex As Exception
            result = 10
        End Try
        Return result
    End Function

    Public Function RNGen(Max As Integer) As Integer
        Return RNGen(0, Max)
    End Function

    Public Function RNGen() As Integer
        Return RNGen(0, MaxRandValue)
    End Function

    '[NamF] = String - First Name (Joe, Jill, Shannon, ect, ect.)
    '[NamL] = String - Last Name (Redwood, Greenleaf, Bluebell, ect, ect.)
    '[EyeC] = String - Eye Colour (Red, Green, Blue, ect, ect.)
    '[HaiL] = String - Hair Length (Long, short, shoulder length, ect, ect.)
    '[HaiC] = String - Hair Colour (Red, Green, Blue, ect, ect.)
    '[HaiS] = String - Hair Style (Frizzy, curly, braided, ect, ect.)
    '[FacS] = String - Face Shape (Round, square, heart-shaped, ect, ect.)
    '[NosS] = String - Nose Shape (Ridged, Hawk-like, button, ect, ect.)
    '[LipS] = String - Lip Shape (Puckered, Heart-shaped, broad, ect, ect.)
    '[SknT] = String - Skin Tone (Pale, tan, ebony, ect, ect.)
    '[Age_] = Integer - Age in numbers (1, 23, 4761, ect, ect.)
    '[GenA] = String - Gender (Boy, Girl, Man, Woman)
    '[GenN] = String - Gender (Male, Female)
    '[GNPA] = String - Gender (his, her)
    '[GNPB] = String - Gender (him, her)
    '[GNPC] = String - Gender (he, she)
    '[AGNa] = String - Age Group name (Infant, Adolecent, Middle-Aged, ect, ect.)
    '[AGTa] = String - Age Group name (Infant, Teenage, Adult, ect, ect.)
    '[HeiT] = String - Height as a tag ("(short:1.5)", "(short:0.5)", "(Tall:1)", ect, ect.) !!FOR SD DO NOT USE FOR READABLE DESCRIPTION!!
    '[HeiM] = Double - Height in metric (40, 80, 185, ect, ect)
    '[HeiI] = Double - Height in Imperial (15.748, 31, 72.83, ect, ect)
    '[WeiT] = String - Weight as a tag ("(skinny:1.5)", "(skinny:0.5)", "(fat:1)", ect, ect.) !!FOR SD DO NOT USE FOR READABLE DESCRIPTION!!
    '[WeiM] = Double - Weight in metric (45.5, 65.2, 108, ect, ect)
    '[WeiI] = Double - Weight in Imperial (99.2, 143.3, 238.9, ect, ect)
    '[BusT] = String - Bust size as tag ("(breasts:1.5)", "(breasts:1)", "(breasts:0)", ect, ect.) !!FOR SD DO NOT USE FOR READABLE DESCRIPTION!!
    '[BusW] = String - Bust size as Cup ("D", "B", "", ect, ect.)

    Public Function TagParse(Input As String, vil As Villager) As String
        Dim Result As String = Input

        '[NamF] = String - First Name (Joe, Jill, Shannon, ect, ect.)
        Result = Result.Replace("[NamF]", vil.Name_F)

        '[NamL] = String - Last Name (Redwood, Greenleaf, Bluebell, ect, ect.)
        Result = Result.Replace("[NamL]", vil.Name_L)

        '[EyeC] = vil.EyeColour - string - Eye Colour (Red, Green, Blue, ect, ect.)
        Result = Result.Replace("[EyeC]", vil.EyeColour)

        '[HaiL] = vil.HairLength - string - Hair Length (Long, short, shoulder length, ect, ect.)
        Result = Result.Replace("[HaiL]", vil.HairLength)

        '[HaiC] = vil.HairColour - string - Hair Colour (Red, Green, Blue, ect, ect.)
        Result = Result.Replace("[HaiC]", vil.HairColour)

        '[HaiS] = vil.HairStyle - string - Hair Style (Frizzy, curly, braided, ect, ect.)
        Result = Result.Replace("[HaiS]", vil.HairStyle)

        '[FacS] = vil.FaceShape - string - Face Shape (Round, square, heart-shaped, ect, ect.)
        Result = Result.Replace("[FacS]", vil.FaceShape)

        '[NosS] = vil.NoseShape - string - Nose Shape (Ridged, Hawk-like, button, ect, ect.)
        Result = Result.Replace("[NosS]", vil.NoseShape)

        '[LipS] = vil.LipShape - string - Lip Shape (Puckered, Heart-shaped, broad, ect, ect.)
        Result = Result.Replace("[LipS]", vil.LipShape)

        '[SknT] = String - Skin Tone (Pale, tan, ebony, ect, ect.)
        Result = Result.Replace("[SknT]", vil.SkinTone)

        '[Age_] = vil.Age - integer - Age in numbers (1, 23, 4761, ect, ect.)
        Result = Result.Replace("[Age_]", vil.Age)

        '[GenA] = vil.Gender.AsAAString - string - Gender (Boy, Girl, Man, Woman)
        Result = Result.Replace("[GenA]", vil.Gender.AsAAString)

        '[GenN] = vil.Gender.AsANString - string - Gender (Male, Female)
        Result = Result.Replace("[GenN]", vil.Gender.AsANString)

        '[GNPA] = String - Gender (his, her)
        Result = Result.Replace("[GNPA]", vil.Gender.AsPNouA)

        '[GNPB] = String - Gender (him, her)
        Result = Result.Replace("[GNPB]", vil.Gender.AsPNouB)

        '[GNPC] = String - Gender (he, she)
        Result = Result.Replace("[GNPC]", vil.Gender.AsPNouC)

        '[AGNa] = AGroup.GetAgeGroupByAge(vil.age).Name - string - Age Group name (Infant, Adolecent, Middle-Aged, ect, ect.)
        Result = Result.Replace("[AGNa]", AGroup.GetAgeGroupByAge(vil.Age).Name)

        '[AGTa] = AGroup.GetAgeGroupByAge(vil.age).Tag - string - Age Group tag (Infant, Teenage, Adult, ect, ect.)
        Result = Result.Replace("[AGTa]", AGroup.GetAgeGroupByAge(vil.Age).Tag)

        '[HeiT] = vil.Height.SDTag - string - Height as a tag ("(short:1.5)", "(short:0.5)", "(Tall:1)", ect, ect.) !!FOR SD DO NOT USE FOR READABLE DESCRIPTION!!
        Result = Result.Replace("[HeiT]", vil.Height.SDTag)

        '[HeiM] = vil.Height.Metric - Double - Height in metric (40, 80, 185, ect, ect)
        Result = Result.Replace("[HeiM]", Math.Round(vil.Height.Metric))

        '[HeiI] = vil.Height.Imperial - Double - Height in Imperial (15.748, 31.4961, 72.8346, ect, ect)
        Result = Result.Replace("[HeiI]", Math.Round(vil.Height.Imperial, 2))

        '[WeiT] = vil.Weight.SDTag - string - Weight as a tag ("(skinny:1.5)", "(skinny:0.5)", "(fat:1)", ect, ect.) !!FOR SD DO NOT USE FOR READABLE DESCRIPTION!!
        Result = Result.Replace("[WeiT]", vil.Weight.SDTag)

        '[WeiM] = vil.Weight.Metric - Double - Weight in metric (45, 65, 108, ect, ect)
        Result = Result.Replace("[WeiM]", Math.Round(vil.Weight.Metric, 1))

        '[WeiI] = vil.Weight.Imperial - Double - Weight in Imperial (99.208, 143.3, 238.099, ect, ect)
        Result = Result.Replace("[WeiI]", Math.Round(vil.Weight.Imperial, 1))

        '[BusT] = vil.Bust.SDTag - string - Bust size as tag ("(breasts:1.5)", "(breasts:1)", "(breasts:0)", ect, ect.) !!FOR SD DO NOT USE FOR READABLE DESCRIPTION!!
        Result = Result.Replace("[BusT]", vil.Bust.SDTag)

        '[BusW] = vil.Bust.CSize - string - Bust size as Cup ("D", "B", "", ect, ect.)
        Result = Result.Replace("[BusW]", vil.Bust.CSize)

        Return Result
    End Function

    Public Function GetMatch(VilA As Villager, VilB As Villager) As Double
        Dim Score As Double = VilA.Traits.CalculateScore(VilB)
        Return (Score)
    End Function

    Public Function GetSClassMatchScore(VilA As Villager, VilB As Villager) As Integer
        ' Assign numerical values to SClass
        Dim SClassValues As New Dictionary(Of SClasses, Integer) From {
            {SClasses.Homeless, 0},
            {SClasses.Poor, 1},
            {SClasses.Lower, 2},
            {SClasses.Middle, 3},
            {SClasses.Upper, 4},
            {SClasses.Rich, 5},
            {SClasses.Noble, 6}
        }

        ' Calculate the SClass match score
        Dim AValue As Integer = SClassValues(VilA.SClass)
        Dim BValue As Integer = SClassValues(VilB.SClass)
        Dim Score As Integer = 10 - 2 * Math.Abs(AValue - BValue)
        Return Score
    End Function

    Public Function GetAttractionScore(VilA As Villager, VilB As Villager) As Integer
        Dim attDiff As Integer = Math.Abs(VilA.Attrac - VilB.Attrac)
        Return attDiff
    End Function

    Public Function CheckMatch(VilA As Villager, VilB As Villager) As (WillMatch As Boolean, FScore As Double, PScore As Double, CScore As Integer, AScore As Integer)
        Dim Result As Boolean
        Dim MScore As Double = GetMatch(VilA, VilB) ' Personality (Traits) score between -x and x where x is the amount of traits that exist
        Dim SScore As Integer = GetSClassMatchScore(VilA, VilB) ' Social class score between 0 and 10
        Dim BScore As Integer = GetAttractionScore(VilA, VilB) ' Attractivness score between 0 and 10
        Dim x As Integer = VilA.Traits.TraitsSpread

        ' Normalize MScore to a range between 0 and 1
        Dim NormalizedMScore As Double = (MScore + x) / (2 * x)

        ' Set a fixed value for the maximum score
        Dim MaxScore As Double = 10.0

        ' Scale the normalized MScore to the maximum score
        Dim WeightedMScore As Double = NormalizedMScore * MaxScore * 0.2
        Dim WeightedSScore As Double = SScore * 0.5
        Dim WeightedBScore As Double = BScore * 0.3

        Dim Score As Double = WeightedMScore + WeightedSScore + WeightedBScore

        Result = If(Score > 0.5, True, False)

        Return (Result, Score, MScore, SScore, BScore)
    End Function

End Module
