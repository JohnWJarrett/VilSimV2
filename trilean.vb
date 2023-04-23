Public Class Trilean
    Private _scale As Double
    Private Const DEFAULT_SCALE As Double = 0.0

    Public Property Scale As Double
        Get
            Return _scale
        End Get
        Set(value As Double)
            If Not value < -1 And Not value > 1 Then
                _scale = value
            End If
        End Set
    End Property

    Public Property Negative As String
    Public Property Positive As String

    Public Sub New(newNegative As String, newPositive As String, Optional newScale As Double = DEFAULT_SCALE)
        Negative = newNegative
        Positive = newPositive
        Scale = newScale
    End Sub

    Public Overrides Function ToString() As String
        Select Case Scale
            Case Is > 0
                Return Positive
            Case Is < 0
                Return Negative
            Case Else
                Return "Neutral"
        End Select
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not [GetType]() Is obj.GetType() Then
            Return False
        End If
        Dim other As Trilean = CType(obj, Trilean)
        Return Me.Scale = other.Scale AndAlso Me.Negative = other.Negative AndAlso Me.Positive = other.Positive
    End Function
End Class
