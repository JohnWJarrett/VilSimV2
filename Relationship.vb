Public Class Relationship

    Public ReadOnly RelationshipUID As String
    Public ReadOnly RStartYear As Integer
    Public ReadOnly EventList As List(Of String)
    Public REndYear As Integer
    Public ReadOnly ChildrenUIDs As List(Of String)
    Public MatchScore As Double

    Public Property IsMarried As Boolean

    Public Sub New(maleUID As String, femaleUID As String, startYear As Integer, relMatchScore As Double)
        RelationshipUID = maleUID & "-" & femaleUID
        RStartYear = startYear
        EventList = New List(Of String)
        REndYear = -9999 ' -9999 indicates the relationship is ongoing
        ChildrenUIDs = New List(Of String)
        MatchScore = relMatchScore
    End Sub

    Public Enum RelationshipEvent
        GoOnDate
        ShareSecret
        ExchangeGifts
        HavePicnic
        AttendPartyTogether
        SupportPartnerInCrisis
        ResolveConflict
        TravelTogether
        ApologizeAfterArgument
        CareForSickPartner
        SurpriseVisit
        ExpressGratitude
        EncouragePartner
        ComfortPartner
        ShareHobby
        HelpWithTask
        LearnSomethingNewTogether
        LaughTogether
        DiscussFuturePlans
        MeetEachOthersFamilies
        Disagree
        Argue
        IgnoreEachOther
        BreakPromise
        BeJealous
        MissImportantEvent
        HaveMiscommunication
        CriticizePartner
        Dishonest
        FailToSupport
        HumiliatePartner
        BetrayTrust
        LieAboutImportantMatter
        DisrespectFamily
        AbandonInTimeOfNeed
        FightPhysically
        RefuseToApologize
        BreakConfidences
        UnderminePartner
        ControlPartner
    End Enum

    Public Function GetRelationshipUID() As (Full As String, Male As String, Female As String)
        Return (RelationshipUID, RelationshipUID.Split(":")(0), RelationshipUID.Split(":")(1))
    End Function

    Private Sub IntAddRelationshipEvent(Message As String, ScoreChange As Integer, CurDate As Integer)
        EventList.Add(Message)
        If MatchScore + ScoreChange > 10 Then
            MatchScore = 10
        Else
            If MatchScore + ScoreChange < 0 Then
                MatchScore = 0
            Else
                MatchScore += ScoreChange
            End If
        End If

        If MatchScore = 0 Then
            If RNGen(500) < 250 Then
                EndRelationship(CurDate)
            End If
        End If
    End Sub

    Public Sub AddRandomRelationshipEvent(CurDate As Integer)
        AddRelationshipEvent(RNGen(0, 39), CurDate)
    End Sub

    Public Sub AddRelationshipEvent(EvType As RelationshipEvent, CurDate As Integer)
        Dim Msg As String
        Dim SChange As Double

        Select Case EvType
            Case RelationshipEvent.GoOnDate
                Msg = $"{CurDate}: Enjoyed a romantic date in the village together."
                SChange = 0.5
            Case RelationshipEvent.ShareSecret
                Msg = $"{CurDate}: Shared deep secrets with each other, strengthening their bond."
                SChange = 0.4
            Case RelationshipEvent.ExchangeGifts
                Msg = $"{CurDate}: Exchanged meaningful gifts with each other."
                SChange = 0.3
            Case RelationshipEvent.HavePicnic
                Msg = $"{CurDate}: Relished a picnic by the river together."
                SChange = 0.3
            Case RelationshipEvent.AttendPartyTogether
                Msg = $"{CurDate}: Attended a local celebration together."
                SChange = 0.2
            Case RelationshipEvent.SupportPartnerInCrisis
                Msg = $"{CurDate}: Offered support to each other during a difficult time."
                SChange = 0.5
            Case RelationshipEvent.ResolveConflict
                Msg = $"{CurDate}: Resolved a disagreement together, improving their relationship."
                SChange = 0.4
            Case RelationshipEvent.TravelTogether
                Msg = $"{CurDate}: Embarked on a journey together."
                SChange = 0.3
            Case RelationshipEvent.ApologizeAfterArgument
                Msg = $"{CurDate}: Apologized and made amends with each other after an argument."
                SChange = 0.2
            Case RelationshipEvent.CareForSickPartner
                Msg = $"{CurDate}: Nursed each other back to health."
                SChange = 0.5
            Case RelationshipEvent.SurpriseVisit
                Msg = $"{CurDate}: Surprised each other with a visit."
                SChange = 0.3
            Case RelationshipEvent.ExpressGratitude
                Msg = $"{CurDate}: Expressed gratitude for each other's support."
                SChange = 0.2
            Case RelationshipEvent.EncouragePartner
                Msg = $"{CurDate}: Encouraged each other during a challenging time."
                SChange = 0.3
            Case RelationshipEvent.ComfortPartner
                Msg = $"{CurDate}: Comforted each other when feeling down."
                SChange = 0.4
            Case RelationshipEvent.ShareHobby
                Msg = $"{CurDate}: Bonded together over a shared hobby."
                SChange = 0.3
            Case RelationshipEvent.HelpWithTask
                Msg = $"{CurDate}: Helped each other with an important task."
                SChange = 0.2
            Case RelationshipEvent.LearnSomethingNewTogether
                Msg = $"{CurDate}: Learned a new skill together."
                SChange = 0.3
            Case RelationshipEvent.LaughTogether
                Msg = $"{CurDate}: Shared a moment of laughter and joy together."
                SChange = 0.3
            Case RelationshipEvent.DiscussFuturePlans
                Msg = $"{CurDate}: Discussed future plans and aspirations together."
                SChange = 0.2
            Case RelationshipEvent.MeetEachOthersFamilies
                Msg = $"{CurDate}: Introduced each other to their families."
                SChange = 0.4
            Case RelationshipEvent.Disagree
                Msg = $"{CurDate}: Disagreed on an issue together, causing tension."
                SChange = -0.3
            Case RelationshipEvent.Argue
                Msg = $"{CurDate}: Got into an argument, testing their bond."
                SChange = -0.4
            Case RelationshipEvent.IgnoreEachOther
                Msg = $"{CurDate}: Ignored each other's feelings and concerns."
                SChange = -0.5
            Case RelationshipEvent.BreakPromise
                Msg = $"{CurDate}: Broke promises to each other, causing distrust."
                SChange = -0.5
            Case RelationshipEvent.BeJealous
                Msg = $"{CurDate}: Felt jealousy, leading to negative emotions in the relationship."
                SChange = -0.3
            Case RelationshipEvent.MissImportantEvent
                Msg = $"{CurDate}: Missed an important event, causing disappointment for both."
                SChange = -0.4
            Case RelationshipEvent.HaveMiscommunication
                Msg = $"{CurDate}: Had a miscommunication, causing confusion and frustration for both."
                SChange = -0.2
            Case RelationshipEvent.CriticizePartner
                Msg = $"{CurDate}: Criticized each other, hurting feelings."
                SChange = -0.3
            Case RelationshipEvent.Dishonest
                Msg = $"{CurDate}: Were dishonest, damaging trust in the relationship."
                SChange = -0.5
            Case RelationshipEvent.FailToSupport
                Msg = $"{CurDate}: Failed to support each other in a time of need."
                SChange = -0.4
            Case RelationshipEvent.HumiliatePartner
                Msg = $"{CurDate}: Humiliated each other in public, damaging self-esteem."
                SChange = -0.6
            Case RelationshipEvent.BetrayTrust
                Msg = $"{CurDate}: Betrayed each other's trust, causing a major rift."
                SChange = -0.7
            Case RelationshipEvent.LieAboutImportantMatter
                Msg = $"{CurDate}: Lied to each other about an important matter, further harming trust."
                SChange = -0.6
            Case RelationshipEvent.DisrespectFamily
                Msg = $"{CurDate}: Disrespected each other's families, causing resentment."
                SChange = -0.5
            Case RelationshipEvent.AbandonInTimeOfNeed
                Msg = $"{CurDate}: Abandoned each other in a time of need, leaving them feeling hurt and alone."
                SChange = -0.8
            Case RelationshipEvent.FightPhysically
                Msg = $"{CurDate}: Engaged in a physical fight, escalating conflict to a dangerous level."
                SChange = -0.9
            Case RelationshipEvent.RefuseToApologize
                Msg = $"{CurDate}: Refused to apologize for wrongdoings, demonstrating a lack of remorse."
                SChange = -0.4
            Case RelationshipEvent.BreakConfidences
                Msg = $"{CurDate}: Shared private information about each other with others, breaching trust."
                SChange = -0.5
            Case RelationshipEvent.UnderminePartner
                Msg = $"{CurDate}: Undermined each other's authority or achievements, causing frustration."
                SChange = -0.4
            Case RelationshipEvent.ControlPartner
                Msg = $"{CurDate}: Attempted to control each other's actions or decisions, causing friction."
                SChange = -0.5
            Case Else
                Msg = $"{CurDate}: Error, this shouldn't have happened"
                SChange = 0
        End Select

        IntAddRelationshipEvent(Msg, SChange, CurDate) ' Don't worry, this function deals with if the score goes below or above the max
    End Sub


End Class