Public Class scion

    Inherits Dictionary(Of String, scionField)

    Public Notes As List(Of String)

    Public Sub New()
        Notes = New List(Of String)
    End Sub

    Public Overloads Sub Add(sf As scionField)
        MyBase.Add(sf.Key, sf)
    End Sub

    Public Function checkRequiredInputs(fieldKeys() As String) As Boolean
        For Each key In fieldKeys
            If Not Me.ContainsKey(key) Then
                Throw New Exception("Field " & key & " is required.")
                Return False
            End If
        Next
        Return True
    End Function

    Public Overloads Function GetString(key As String) As String
        If Me.ContainsKey(key) Then
            ' TODO : Unit check
            Return Me.Item(key).Value
        Else
            Throw New Exception("Field " & key & " is required.")
        End If
    End Function

    Public Overloads Function GetNumber(key As String) As Single
        If Me.ContainsKey(key) Then
            If IsNumeric(Me.Item(key).Value) Then
                ' TODO : Unit check
                Return Me.Item(key).Value
            Else
                Throw New Exception("Field " & key & " is not numeric.")
            End If
        Else
            Throw New Exception("Field " & key & " is required.")
        End If
    End Function

    Public Overloads Function GetNumber(key As String, unit As String) As Single
        If Me.ContainsKey(key) Then
            If IsNumeric(Me.Item(key).Value) Then
                ' TODO : Unit check
                Return Me.Item(key).Value
            Else
                Throw New Exception("Field " & key & " is not numeric.")
            End If
        Else
            Throw New Exception("Field " & key & " is required.")
        End If
    End Function
End Class
