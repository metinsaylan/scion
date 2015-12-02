Imports scion

Module Module1

    Public Sub Main()

        Dim asp As String = Application.StartupPath
        Dim dataFile As String = IO.Path.Combine(asp, "data", "sample.md")

        Dim sample As New scion.scion
        Dim scr As New scionReader

        Console.WriteLine("> Reading sample.md file..")
        sample = scionReader.Read(dataFile)

        Dim len As Single = sample.GetNumber("length")
        Console.WriteLine("  Length field in file: ")
        Console.WriteLine(sample.Item("length"))

        Dim wid As Single = sample.GetNumber("width")
        Console.WriteLine("  Width field in file: ")
        Console.WriteLine(sample.Item("width"))

        Console.WriteLine("> Calculating area..")
        Dim Area As Single = len * wid

        Console.WriteLine("> Adding result to scion data..")
        sample.Add(New scionField("area", Area, "m2"))

        Dim outFile As String = IO.Path.Combine(asp, "data", "output.md")

        Console.WriteLine("> Writing resulting data to output.md..")
        scionWriter.WriteScon(outFile, sample)

        Console.Write("Finished. Press any key to exit.")
        Console.ReadKey()

    End Sub

End Module
