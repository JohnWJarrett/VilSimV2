Public Class Form1
    Private ReadOnly SDOP As String
    Private ReadOnly DEOP As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim VTypes As List(Of String) = IO.File.ReadAllLines("vtypes.txt").ToList

        MyVillage = New Village(VTypes(RNGen(VTypes.Count - 1)))

        MyVillage.BuildBuilding("House_BunkHouse", True)

        'Vill(MyVillage.OrigVillagerCount)

        For i = 0 To 100
            AgeUp(i)
            'Vill(i)
            MyVillage.BuildResourceDrivenAllocation()
        Next

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        My.Computer.Clipboard.SetText(SDOP)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        My.Computer.Clipboard.SetText(DEOP)
    End Sub


End Class



'Dim OutPut As String
'Dim vil As Villager
'Dim G As Char
'Dim Out1 As String = "([Age_] year old [AGTa] [GenA]:1.5), ([SknT] complexion:1.5), [BusT], [WeiT], [EyeC] eyes, [HaiL] [HaiC] [HaiS] Hair, [FacS] face, [NosS] nose, [LipS] lips"

'If ComboBox1.SelectedItem = "Male" Then
'    G = "m"
'ElseIf ComboBox1.SelectedItem = "Female" Then
'    G = "f"
'Else
'    G = "r"
'End If

'If NumericUpDown1.Value = -1 Then
'    If G = "r" Then
'        vil = New Villager()
'    Else
'        vil = New Villager(G)
'    End If
'Else
'    If G = "r" Then
'        vil = New Villager(NumericUpDown1.Value)
'    Else
'        vil = New Villager(NumericUpDown1.Value, G)
'    End If
'End If

'SDOP = TagParse(Out1, vil).ToLower
'DEOP = TagParse(RandomLines.GetLine, vil)

'OutPut = SDOP & vbNewLine & vbNewLine & DEOP

'TextBox1.Text = OutPut

'Dim x1, x2 As Villager
'x1 = New Villager(16, "M")
'x2 = New Villager(16, "F")
'Dim matchResult = CheckMatch(x1, x2)
'Dim FScore = matchResult.FScore
'Dim PScore = matchResult.PScore
'Dim CScore = matchResult.CScore
'Dim AScore = matchResult.AScore
'Dim Rns As Integer = 5000

'Dim y As New List(Of Double)

'Dim ccount(7) As Integer
'Dim acount(10) As Integer

'For i = 0 To Rns - 1
'    x1 = New Villager(16, "M")
'    x2 = New Villager(16, "F")
'    CountClassStats(x1, ccount)
'    CountClassStats(x2, ccount)
'    CountAttracStats(x1, acount)
'    CountAttracStats(x2, acount)
'    matchResult = CheckMatch(x1, x2)
'    y.Add(matchResult.FScore)
'Next

'ldb.Commit()
'ldb.Dispose()

'Dim scr = AnalyzeScores(y.ToArray)

'TextBox1.Text = scr.Summary _
'                & vbNewLine & " H: " & ccount(0) & " P: " & ccount(1) & " L: " & ccount(2) & " M: " & ccount(3) & " U: " & ccount(4) & " R: " & ccount(5) & " N: " & ccount(6) _
'                & vbNewLine & " 1: " & acount(0) & " 2: " & acount(1) & " 3: " & acount(2) & " 4: " & acount(3) & " 5: " & acount(4) _
'                            & " 6: " & acount(5) & " 7: " & acount(6) & " 8: " & acount(7) & " 9: " & acount(8) & " X: " & acount(9)
'IO.File.WriteAllText("out.txt", scr.Summary)