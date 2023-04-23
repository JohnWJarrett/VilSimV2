Public Module NameTools
    Private ReadOnly MaleNamesList As List(Of String) = LoadNames("male")
    Private ReadOnly FemaleNamesList As List(Of String) = LoadNames("female")
    Private ReadOnly LastNamesList As List(Of String) = LoadNames("last")

    Private Function LoadNames(nameType As String) As List(Of String)
        Dim result As New List(Of String)

        If nameType = "male" Then
            If IO.File.Exists("data\fnm.txt") Then
                ' Read in file from directory
                result = IO.File.ReadAllLines("data\fnm.txt").ToList()
            Else
                ' Read text from embedded resource called "males"
                Try
                    result = New List(Of String)(My.Resources.males.Split({vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries))
                Catch ex As Exception
                    Console.WriteLine("Error reading names from embedded resource 'males': {0}", ex.Message)
                    ' Return a hardcoded list of names
                    result = {"John", "Michael", "David", "James", "William", "Christopher", "Joseph", "Matthew", "Daniel", "Robert"}.ToList()
                End Try
            End If
        ElseIf nameType = "female" Then
            If IO.File.Exists("data\fnf.txt") Then
                ' Read in file from directory
                result = IO.File.ReadAllLines("data\fnf.txt").ToList()
            Else
                ' Read text from embedded resource called "females"
                Try
                    result = New List(Of String)(My.Resources.females.Split({vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries))
                Catch ex As Exception
                    Console.WriteLine("Error reading names from embedded resource 'females': {0}", ex.Message)
                    ' Return a hardcoded list of names
                    result = {"Emily", "Emma", "Madison", "Olivia", "Abigail", "Isabella", "Samantha", "Elizabeth", "Ashley", "Mia"}.ToList()
                End Try
            End If
        ElseIf nameType = "last" Then
            If IO.File.Exists("data\ln.txt") Then
                ' Read in file from directory
                result = IO.File.ReadAllLines("data\ln.txt").ToList()
            Else
                ' Read text from embedded resource called "lastnames"
                Try
                    result = New List(Of String)(My.Resources.last.Split({vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries))
                Catch ex As Exception
                    Console.WriteLine("Error reading names from embedded resource 'lastnames': {0}", ex.Message)
                    ' Return a hardcoded list of names
                    result = {"Smith", "Johnson", "Williams", "Jones", "Brown", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez"}.ToList()
                End Try
            End If
        End If

        Return result
    End Function

    Public Function GetRandomName(gender As Char) As String
        Dim NameList As List(Of String) = If(gender = "m"c, MaleNamesList, FemaleNamesList)
        ' Generate a random index between 0 and the number of available names
        Dim randomIndex As Integer = RNGen(0, NameList.Count - 1)

        ' Get the name at the random index
        Dim result As String = NameList(randomIndex)

        Return result
    End Function

    Public Function GetRandomLastName() As String
        Dim NameList As List(Of String) = LastNamesList
        ' Generate a random index between 0 and the number of available names
        Dim randomIndex As Integer = RNGen(0, NameList.Count - 1)

        ' Get the name at the random index
        Dim result As String = NameList(randomIndex)

        Return result
    End Function

End Module