'$Id$
Public Class LanguageStrings
    Private Strings As New System.Collections.Generic.Dictionary(Of String, String)
    Public Sub New(ByVal DefaultStrings As String, ByVal LanguageFile As String)

        Dim xml As Xml.XmlDocument = New Xml.XmlDocument

        xml.LoadXml(DefaultStrings)
        For Each s As System.Xml.XmlNode In xml.DocumentElement.ChildNodes
            Strings.Add(s.Name, s.InnerText)
        Next

        If System.IO.File.Exists(LanguageFile) Then
            xml.Load(LanguageFile)
            SettingsLog.Add("LanguageFile: " & LanguageFile) 'global
            For Each s As System.Xml.XmlNode In xml.DocumentElement.ChildNodes
                If Strings.ContainsKey(s.Name) Then
                    Strings.Item(s.Name) = s.InnerText
                Else
                    Strings.Add(s.Name, s.InnerText)
                End If

            Next
        End If
    End Sub
    Function s(ByVal StringName As String) As String
        If Strings.ContainsKey(StringName) Then
            Return Strings.Item(StringName).Replace("\r\n", vbCrLf)
        Else
            Return StringName
        End If
    End Function
End Class
