'$Id$
Public Class Settings
    Private Strings As New System.Collections.Generic.Dictionary(Of String, String)
    Public Sub New(ByVal SettingsFile As String)
        Dim xml As Xml.XmlDocument = New Xml.XmlDocument
        If System.IO.File.Exists(SettingsFile) Then
            xml.Load(SettingsFile)
            SettingsLog.Add("Settings: " & SettingsFile) 'global
            For Each s As System.Xml.XmlNode In xml.DocumentElement.ChildNodes
                Strings.Add(s.Name, s.InnerText)
            Next
        End If
    End Sub
    Function s(ByVal StringName As String, Optional ByVal DefaultValue As String = "") As String
        If Strings.ContainsKey(StringName) Then
            Return Strings.Item(StringName)
        Else
            Return DefaultValue
        End If
    End Function
End Class
