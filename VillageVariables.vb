Public Module VillageVariables

    ' =======================================================================================================================================================

    ' Set the mean for the social class value, lower values will result in a poorer village
    Public Const SocialClassMean As Double = 3

    ' Set the standard deviation for the social class value, higher values will result in greater variance in social class
    Public Const SocialClassStdDev As Double = 1

    ' Set the minimum social class value, all values lower than this will be clamped to this value
    Public Const SocialClassMin As Double = 0

    ' Set the maximum social class value, all values higher than this will be clamped to this value
    Public Const SocialClassMax As Double = 6

    ' Set the mean for the attractiveness value, lower values will result in less attractive villagers
    Public Const AttractivenessMean As Double = 5

    ' Set the standard deviation for the attractiveness value, higher values will result in greater variance in attractiveness
    Public Const AttractivenessStdDev As Double = 2

    ' Set the minimum attractiveness value, all values lower than this will be clamped to this value
    Public Const AttractivenessMin As Double = 1

    ' Set the maximum attractiveness value, all values higher than this will be clamped to this value
    Public Const AttractivenessMax As Double = 10

    ' Upper nth percentile - the score above which two villagers are considered highly compatible, expressed as a percentage (0-100)
    ' If UNP is set to 85, any two villagers whose compatibility score falls in the top 15% will be considered highly compatible.
    Public Const UNP As Double = 85

    ' Lower nth percentile - the score below which two villagers are considered highly incompatible, expressed as a percentage (0-100)
    ' If LNP is set to 10, any two villagers whose compatibility score falls in the bottom 10% will be considered so poorly matched that they will be unfaithful to each other if forced into marriage.
    Public Const LNP As Double = 10

    ' =======================================================================================================================================================

    Public Const StartingBaseHealth As Double = 100

    ' =======================================================================================================================================================

    Public Property EyeColors As String() = {"blue", "brown", "green", "hazel", "gray", "amber", "black", "dark brown", "light brown", "blue-green", "dark hazel", "light blue", "dark green", "gray-blue"}
    Public Property HairLengthsM As String() = {"short", "medium-length", "shoulder-length", "buzzed", "shaved", "bald"}
    Public Property HairLengthsF As String() = {"short", "medium-length", "shoulder-length", "long", "very long"}
    Public Property HairColors As String() = {"blonde", "black", "brown", "red", "auburn", "chestnut", "golden", "strawberry blonde", "platinum blonde", "dark brown", "light brown", "dirty blonde", "jet black"}
    Public Property HairStyles As String() = {"straight", "wavy", "curly", "coiled", "kinky", "frizzy", "thick", "thin", "fine", "lustrous"}
    Public Property FaceShapes As String() = {"oval-shaped", "round", "square-jawed", "heart-shaped", "angular", "long and narrow", "chiseled"}
    Public Property NoseShapes As String() = {"roman", "button", "snub", "crooked", "aquiline", "upturned", "flat", "wide", "narrow", "long", "short", "bulbous", "fleshy", "hawk-like", "pointed", "straight", "high bridge", "low bridge"}
    Public Property LipTypes As String() = {"full", "thin", "small", "tightly pursed", "heart-shaped", "thin upper and full lower"}
    Public Property SkinTypes As String() = {"pale", "light", "fair", "medium", "olive", "tan", "deep", "dark", "ebony"}

End Module
