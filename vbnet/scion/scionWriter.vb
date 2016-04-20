Imports System.Reflection

Public Class scionWriter

    Public Shared Sub WriteScon(path As String, data As scionData)

        Dim lines As New List(Of String)

        ' Dim info() As PropertyInfo = data.GetType().GetProperties()

        For Each kv As KeyValuePair(Of String, scionField) In data

            Dim sc As scionField = kv.Value

            lines.Add(kv.Value.ToString)


        Next

        IO.File.WriteAllLines(path, lines)

    End Sub


End Class
