Public Class scionReader

    Public Shared Function Read(path As String) As scionData

        Dim sc As New scionData

        If IO.File.Exists(path) Then

            ' Read all lines to a list
            Dim lines As String() = IO.File.ReadAllLines(path)

            For Each line In lines

                ' Trim line
                line = line.Trim()

                ' Skip line if empty
                If line.Length = 0 Then Continue For

                ' Skip line if comment
                If line.StartsWith("#") Then Continue For

                If line.StartsWith("@") Then

                    Dim includeFile As String = line.Split({""""}, StringSplitOptions.RemoveEmptyEntries)(1)

                    If IO.File.Exists(includeFile) Then
                        Dim inc As scionData = scionReader.Read(includeFile)

                        For Each item In inc
                            sc.Add(item.Key, item.Value)
                        Next
                    End If

                    Continue For
                End If

                ' Add to notes if starts with >
                If line.StartsWith(">") Then
                    sc.Notes.Add(line)
                End If

                ' Split using spaces with remove empty option
                Dim data As String() = line.Split({" ", vbTab}, StringSplitOptions.RemoveEmptyEntries)

                ' sample data notations: 
                ' displacement 	: 2530	[tons]                      > simple variable
                ' range			: 2500  @       speed_eco	[nm]    > variable connected to another data
                ' speed         : 7     -       12          [kn]    > variable with limits

                If data.Count > 2 & data.Count < 10 Then
                    Dim sf As New scionField

                    If data(1) <> ":" Then Continue For ' TODO : find a fix for those cases.

                    If IsNumeric(data(2)) Then

                        sf.Key = data(0)

                        If data.Count > 2 Then

                            If data(3).StartsWith("@") Then
                                ' range			: 2500  @       speed_eco	[nm]

                                sf.Value = data(2)
                                sf.Unit = data(5)

                                sf.HasCondition = True
                                sf.ConditionKey = data(4)

                            ElseIf data(3).StartsWith("-") Then
                                ' speed         : 7     -       12          [kn]    > variable with limits

                                sf.Value = data(2)
                                sf.Unit = data(5)

                                sf.IsRange = True
                                sf.UpperLimit = data(4)

                            Else
                                ' displacement 	: 2530	[tons]  

                                sf.Value = data(2)
                                sf.Unit = data(3)

                            End If
                        Else
                            sf.Value = data(2)
                        End If

                    Else

                        ' String value can contain spaces etc.
                        sf.Key = data(0)
                        sf.Value = line.Split({":"}, StringSplitOptions.RemoveEmptyEntries)(1).Trim()
                        sf.IsText = True

                    End If

                    If sc.ContainsKey(sf.Key) Then
                        sc(sf.Key).HasCondition = True

                        If sc(sf.Key).Value <> "" Then
                            sc(sf.Key).ConditionTable.Add("_default", sc(sf.Key).Value)
                        End If

                        sc(sf.Key).ConditionTable.Add(sf.ConditionKey, sf.Value)
                    Else
                        sc.Add(sf.Key, sf)

                        If sf.HasCondition Then
                            sc(sf.Key).ConditionTable.Add(sf.ConditionKey, sf.Value)
                        End If
                    End If

                End If
            Next

        Else
            Throw New Exception("File " & path & " doesn't exist.")
        End If

        Return sc
    End Function


End Class
