Public Module Enums
    Public Enum RFeatures
        EyeCol = 0
        HaiLenM = 1
        HaiLenF = 2
        HaiCol = 3
        HaiSty = 4
        FacShp = 5
        NosShp = 6
        LipTyp = 7
        SknTon = 8
    End Enum

    Public Enum SClasses
        Homeless = 0
        Poor = 1
        Lower = 2
        Middle = 3
        Upper = 4
        Rich = 5
        Noble = 6
    End Enum

    Public Enum CompetencyLevels
        Incompetent
        Poor
        Average
        Good
        Master
    End Enum

    Public Enum BCond
        Weak
        Poor
        Average
        Good
        Strong
        Fortified
    End Enum

    Public Enum BuildingType
        BTResidential
        BTCommercial
        BTIndustrial
        BTAgricultural
        BTPublic
        BTMilitary
        BTReligious
        BTUtility
        BTPort
    End Enum

    Public Enum BTResidential
        ResNone
        ResPoor
        ResLower
        ResMiddle
        ResUpper
        ResRich
        ResNoble
        ResBunk
    End Enum

    Public Enum BTCommercial
        ComBlacksmith
        ComBaker
        ComInn
        ComTavern
        ComGeneralStore
        ComTailor
        ComCobbler
        ComJeweler
        ComAlchemist
        ComCarpenter
        ComButcher
        ComFishmonger
        ComMarket
        ComStables
    End Enum

    Public Enum BTIndustrial
        IndSawmill
        IndMine
        IndQuarry
        IndFoundry
        IndLumberyard
        IndDock
        IndShipyard
    End Enum

    Public Enum BTAgricultural
        FarWheat
        FarVegetables
        FarFruit
        FarHorse
        FarCow
        FarSheep
        FarChicken
        FarPig
        FarBeekeeping
        FarGranary
        FarMill
        FarHunter
    End Enum

    Public Enum BTPublic
        PubTownHall
        PubJail
        PubPark
        PubMonument
        PubSquare
    End Enum

    Public Enum BTMilitary
        MilBarracks
        MilWatchtower
        MilFort
        MilArmory
        MilTrainingGround
    End Enum

    Public Enum BTReligious
        RelChurch
        RelShrine
        RelGraveyard
        RelMystical
    End Enum

End Module
