Public Class Villager

    ' Personal Information
    Public ReadOnly Property UID As String
    Public ReadOnly Property Name_F As String
    Public ReadOnly Property Name_L As String
    Public ReadOnly Property Age As Integer

    ' Occupation
    Public Property Occupation As String
    Public Property Competency As Integer ' Between 0 and 4

    ' Family Information
    Public ReadOnly Property FatherUID As String
    Public ReadOnly Property MotherUID As String

    Public ReadOnly Property RelationshipList As List(Of String)

    ' Pysical Description - Public
    Public ReadOnly Property EyeColour As String
    Public ReadOnly Property HairColour As String
    Public ReadOnly Property HairLength As String
    Public ReadOnly Property HairStyle As String
    Public ReadOnly Property FaceShape As String
    Public ReadOnly Property NoseShape As String
    Public ReadOnly Property LipShape As String
    Public ReadOnly Property SkinTone As String

    ' Pysical Description - Private
    Private _Gender As Char
    Private _BustPure As Integer
    Private _weightInKg As Double
    Private _heightInCm As Double

    ' Relationship Stats
    Public ReadOnly Property SClass As SClasses 'homeless,poor,lower,middle,upper,rich,noble
    Public ReadOnly Property Attrac As Integer 'between 1 and 10
    Public ReadOnly Property SDrive As Integer 'between 0 and 100
    Public ReadOnly Property Traits As New Traits

    ' Health
    Public Property Health As Double ' Between 0 and 100
    Public Property IsSickOrInjured As Boolean
    Public Property HasChronicIllness As Boolean
    Public Property IsHungry As Boolean

    ' Properties with proc
    Public Property GenderChar As Char
        Get
            Return _Gender
        End Get
        Set(value As Char)
            _Gender = value
        End Set
    End Property

    Public Property BustPure As Integer
        Get
            Return _BustPure
        End Get
        Set(value As Integer)
            _BustPure = value
        End Set
    End Property

    Public Property WeightInKg As Double
        Get
            Return _weightInKg
        End Get
        Set(value As Double)
            _weightInKg = value
        End Set
    End Property

    Public Property HeightInCm As Double
        Get
            Return _heightInCm
        End Get
        Set(value As Double)
            _heightInCm = value
        End Set
    End Property

    <LiteDB.BsonIgnore>
    Public ReadOnly Property Gender As (AsChar As Char, AsAAString As String, AsANString As String, AsPNouA As String, AsPNouB As String, AsPNouC As String)
        Get
            Dim Result, gString, PNounA, PNounB, PNounC As String

            If _Gender = "m" Then
                Result = "male"
                PNounA = "his"
                PNounB = "him"
                PNounC = "he"
            Else
                Result = "female"
                PNounA = "her"
                PNounB = "her"
                PNounC = "she"
            End If

            If Age <= AGroup.AgeGroup("Adolescent").MaxAge Then
                gString = If(_Gender = "m", "boy", "girl")
            Else
                gString = If(_Gender = "m", "man", "woman")
            End If

            Return (_Gender, Result, gString, PNounA, PNounB, PNounC)
        End Get
    End Property

    <LiteDB.BsonIgnore>
    Public ReadOnly Property Bust As (pure As Integer, SDTag As String, CSize As String)
        Get
            Dim sdTag As String
            Dim CupSize As String

            sdTag = AGroup.GetBust(_BustPure)

            Select Case _BustPure
                Case 1
                    CupSize = "A"
                Case 2
                    CupSize = "B"
                Case 3
                    CupSize = "C"
                Case 4
                    CupSize = "D"
                Case Else
                    CupSize = ""
            End Select

            Return (_BustPure, sdTag, CupSize)
        End Get
    End Property

    <LiteDB.BsonIgnore>
    Public ReadOnly Property Weight As (SDTag As String, Metric As Double, Imperial As Double)
        Get
            Dim sdTag As String = AGroup.GetWeightRange(Age, _weightInKg)
            Dim weightInPounds As Double = _weightInKg * 2.20462
            Return (sdTag, _weightInKg, weightInPounds)
        End Get
    End Property

    <LiteDB.BsonIgnore>
    Public ReadOnly Property Height As (SDTag As String, Metric As Double, Imperial As Double)
        Get
            Dim sdTag As String = AGroup.GetHeightRange(Age, _heightInCm)
            Dim heightInInches As Double = _heightInCm / 2.54
            Return (sdTag, _heightInCm, heightInInches)
        End Get
    End Property

    ' Creating new Villagers

    ' For use with when a child is born in the village
    Public Sub New(NewAge As Integer, NewGender As Char, NewFatherUID As String, NewMotherUID As String, NewSocialClass As Integer)

        Name_L = GetRandomLastName()

        UID = Guid.NewGuid().ToString("N").Substring(0, 8)

        FatherUID = NewFatherUID
        MotherUID = NewMotherUID

        Age = NewAge
        _Gender = NewGender

        Name_F = GetRandomName(_Gender)

        If Not _Gender = "m" Then
            _BustPure = AGroup.GenerateBust(Age)
        Else
            _BustPure = -1
        End If

        _weightInKg = AGroup.GenerateWeight(Age)
        _heightInCm = AGroup.GenerateHeight(Age)

        EyeColour = GetRandomFeature(0)

        If _Gender = "m" Then
            HairLength = GetRandomFeature(1)
        Else
            HairLength = GetRandomFeature(2)
        End If

        HairColour = GetRandomFeature(3)
        HairStyle = GetRandomFeature(4)
        FaceShape = GetRandomFeature(5)
        NoseShape = GetRandomFeature(6)
        LipShape = GetRandomFeature(7)
        SkinTone = GetRandomFeature(8)

        Traits.RandomizeTraits()

        SClass = NewSocialClass
        Attrac = Math.Round(TruncNorm(AttractivenessMean, AttractivenessStdDev, AttractivenessMin, AttractivenessMax))

        SDrive = RNGen(0, 100)
    End Sub

    ' For use when a Villager moves in
    Public Sub New(NewAge As Integer, NewGender As Char)
        Me.New(NewAge, NewGender, "NONGIVEN", "NONGIVEN", -1)
        SClass = Math.Round(TruncNorm(SocialClassMean, SocialClassStdDev, SocialClassMin, SocialClassMax))
    End Sub

    Public Sub New(NewAge As Integer)
        Me.New(NewAge, "U")
        _Gender = If(RNGen(1000) < 500, "m", "f")
    End Sub

    Public Sub New(NewGender As Char)
        Me.New(-1, NewGender)
        Age = RNGen(90)
    End Sub

    Public Sub New()
        Me.New(-1, "U")
        Age = RNGen(90)
        _Gender = If(RNGen(1000) < 500, "m", "f")
    End Sub

    ' Functions

    Public Function GetCompetency() As CompetencyLevels
        Dim Result As CompetencyLevels = Competency

        If IsSickOrInjured Then
            Result -= 1
        End If

        If IsHungry Then
            Result -= 1
        End If

        Select Case Health
            Case < StartingBaseHealth * 0.1
                Result -= 2
            Case < StartingBaseHealth * 0.5
                Result -= 1
        End Select

        Return Result
    End Function
End Class