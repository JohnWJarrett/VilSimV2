Public Module analysis
    Public Function CountClassStats(V1 As Villager, ccount() As Integer) As Integer()
        Select Case V1.SClass
            Case SClasses.Homeless
                ccount(0) += 1
            Case SClasses.Poor
                ccount(1) += 1
            Case SClasses.Lower
                ccount(2) += 1
            Case SClasses.Middle
                ccount(3) += 1
            Case SClasses.Upper
                ccount(4) += 1
            Case SClasses.Rich
                ccount(5) += 1
            Case SClasses.Noble
                ccount(6) += 1
            Case Else
                ccount(7) += 1
        End Select

        Return ccount
    End Function

    Public Function CountAttracStats(V1 As Villager, ccount() As Integer) As Integer()
        Select Case V1.Attrac
            Case < 2
                ccount(0) += 1
            Case < 3
                ccount(1) += 1
            Case < 4
                ccount(2) += 1
            Case < 5
                ccount(3) += 1
            Case < 6
                ccount(4) += 1
            Case < 7
                ccount(5) += 1
            Case < 8
                ccount(6) += 1
            Case < 9
                ccount(7) += 1
            Case < 10
                ccount(8) += 1
            Case Else
                ccount(9) += 1
        End Select

        Return ccount
    End Function

    ''' <summary>
    ''' Analyzes an array of scores and returns Summary, Total Average, Median, Lowest Value, Highest Value, Lower 10% Average, Upper 10% Average, Standard Deviation, Lower 10th Percentile, Upper 10th Percentile, Married Count, Unfaithful Count, Married Percent of total, unfaithful Percent of total.
    ''' </summary>
    ''' <param name="scores">An array of scores to analyze.</param>
    ''' <returns></returns>
    Public Function AnalyzeScores(scores() As Double) As (Summary As String, Avg As Double, Median As Double, Low As Double, High As Double, AvgLow As Double, AvgHigh As Double,
                                  Stdev As Double, Low10Perc As Double, High10Perc As Double, Married As Integer, Unfaithful As Integer, MPerc As Double, UPerc As Double)

        ' Calculate average
        Dim avg As Double = scores.Average()

        ' Calculate median
        Array.Sort(scores)
        Dim median As Double
        If scores.Length Mod 2 = 0 Then
            median = (scores(scores.Length \ 2 - 1) + scores(scores.Length \ 2)) / 2
        Else
            median = scores(scores.Length \ 2)
        End If

        ' Calculate low and high scores
        Dim low As Double = scores.Min()
        Dim high As Double = scores.Max()

        ' Calculate average of top 10% and bottom 10%
        Dim numTop As Integer = scores.Length \ 10
        Dim numLow As Integer = scores.Length \ 10
        Dim avgHigh As Double = scores.OrderByDescending(Function(x) x).Take(numTop).Average()
        Dim avgLow As Double = scores.OrderBy(Function(x) x).Take(numLow).Average()

        ' Calculate standard deviation
        Dim stdev As Double = Math.Sqrt(scores.Average(Function(x) Math.Pow(x - avg, 2)))

        ' Calculate 10th percentiles
        Dim low10Perc As Double = scores.OrderBy(Function(x) x).ElementAt(scores.Length * 0.1)
        Dim high10Perc As Double = scores.OrderByDescending(Function(x) x).ElementAt(scores.Length * 0.1)

        ' Calculate the index corresponding to the Yth percentile (lowest Y% of scores)
        Dim yIndex As Integer = Math.Floor((scores.Length * LNP / 100)) - 1

        ' Calculate the index corresponding to the (100-X)th percentile (top X% of scores)
        Dim xIndex As Integer = Math.Floor((scores.Length * (100 - UNP) / 100)) - 1

        ' Retrieve the scores at the corresponding indices
        Dim lowestYPercentScore As Double = scores(yIndex)
        Dim topXPercentScore As Double = scores(xIndex)

        ' Calculate percentage of married and unfaithful
        Dim marriedCount As Integer = scores.Where(Function(x) x >= topXPercentScore).Count()
        Dim unfaithfulCount As Integer = scores.Where(Function(x) x <= lowestYPercentScore).Count()
        Dim mPerc As Double = (marriedCount / scores.Length) * 100
        Dim uPerc As Double = (unfaithfulCount / scores.Length) * 100

        Dim sum As String = "Statistics for " & scores.Length & " Runs" & vbNewLine & vbNewLine
        sum &= "Married: " & marriedCount & " (" & mPerc & "%) for (S > " & topXPercentScore & ")" & vbNewLine
        sum &= "Unfaith: " & unfaithfulCount & " (" & uPerc & "%) for (S < " & lowestYPercentScore & ")" & vbNewLine & vbNewLine
        sum &= "AvgL10%: " & avgLow & vbNewLine
        sum &= "LowestS: " & low & vbNewLine
        sum &= "Lo10th%: " & low10Perc & vbNewLine
        sum &= "MedianS: " & median & vbNewLine
        sum &= "Average: " & avg & vbNewLine
        sum &= "Std Dev: " & stdev & vbNewLine
        sum &= "Up10th%: " & high10Perc & vbNewLine
        sum &= "HigestS: " & high & vbNewLine
        sum &= "AvgU10%: " & avgHigh

        ' Return results
        Return (sum, avg, median, low, high, avgLow, avgHigh, stdev, low10Perc, high10Perc, marriedCount, unfaithfulCount, mPerc, uPerc)
    End Function



End Module
