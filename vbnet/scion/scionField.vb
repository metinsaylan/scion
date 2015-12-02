Public Class scionField

    Public Order As Integer
    Public Key As String
    Public Value As String
    Public Unit As String
    Public IsRange As Boolean
    Public UpperLimit As String
    Public HasCondition As Boolean
    Public ConditionKey As String
    Public IsText As String

    Public ConditionTable As Dictionary(Of String, String)

    Public Sub New()

        Order = -1
        Unit = "[-]"
        Key = ""
        Value = ""
        IsRange = False
        HasCondition = False
        UpperLimit = ""
        ConditionKey = ""
        IsText = False
        ConditionTable = New Dictionary(Of String, String)

    End Sub

    Public Sub New(key As String, value As String, unit As String)

        Me.New
        Me.Key = key
        Me.Value = value

        If Not unit.StartsWith("[") Then
            Me.Unit = "[" & unit & "]"
        Else
            Me.Unit = unit
        End If

    End Sub

    Public Overloads Function GetConditionalValue(key As String) As String
        If HasCondition Then
            If ConditionTable.Count > 0 Then
                If ConditionTable.Keys.Contains(key) Then
                    Return ConditionTable.Item(key)
                Else
                    Throw New Exception("Item " & Me.Key & " doesn't have value for condition : " & key)
                End If
            Else
                Throw New Exception("Item " & Me.Key & " doesn't have any conditions defined. Requested condition was : " & key)
            End If
        Else
            Return Value
        End If
    End Function

    Public Overrides Function ToString() As String
        Dim sb As String

        sb = Me.Key & vbTab & " : " & vbTab

        ' displacement 	: 2530	[tons]                      > simple variable
        ' range			: 2500  @       speed_eco	[nm]    > variable connected to another data
        ' speed         : 7     -       12          [kn]    > variable with limits

        If Me.HasCondition Then
            Dim base As String = sb
            sb = ""
            For Each cond As KeyValuePair(Of String, String) In ConditionTable
                sb = sb & base & cond.Value & " @ " & cond.Key & " " & Me.Unit & vbCrLf
            Next

            ' remove last line break
            sb = sb.Trim()

        ElseIf Me.IsRange Then
            sb = sb & Me.Value & " - " & Me.UpperLimit & " " & Me.Unit
        Else
            sb = sb & Me.Value & " " & Me.Unit
        End If

        Return sb.ToString
    End Function

End Class
