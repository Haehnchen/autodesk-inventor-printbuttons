'$Id$
Imports Inventor
Imports System.Collections.Generic
Public Class XMLButtonDef
    Dim _Buttons As New List(Of SingleButtonDefintion)
    ''' <summary>
    ''' Need a XML Config File
    ''' </summary>
    ''' <param name="ConfFile">FileName to read</param>
    ''' <remarks>XMLObject with Parameters needed; look example XML-File for Syntax!</remarks>
    Public Sub New(ByVal ConfFile As String, Optional ByVal InternalNamePrefix As String = "my")
        Dim xml As Xml.XmlDocument = New Xml.XmlDocument
        xml.Load(ConfFile)

        For Each s As System.Xml.XmlNode In xml.DocumentElement.ChildNodes
            Dim sButton As New SingleButtonDefintion

            For Each attrib As System.Xml.XmlAttribute In s.Attributes
                Select Case attrib.Name
                    Case "Tooltip" : sButton.Tooltip = attrib.Value
                    Case "AllColorsAsBlack" : sButton.AllColorsAsBlack = attrib.Value
                    Case "PrintScaleMode" : sButton.PrintScaleMode = attrib.Value
                    Case "UpdatePlotstyles" : sButton.UpdatePlotstyles = attrib.Value
                    Case "Rotate" : sButton.Rotate = attrib.Value
                    Case "WindowsPrinterName" : sButton.WindowsPrinterName = attrib.Value
                    Case "UpdateIProperties" : sButton.UpdateIProperties = attrib.Value
                    Case "FixedPaperSize" : sButton.FixedPaperSize = attrib.Value
                End Select
            Next
            sButton.Name = s.Name
            sButton.InternalName = InternalNamePrefix & "XMLButton" & s.Name
            _Buttons.Add(sButton)

        Next
    End Sub
    ''' <summary>
    ''' this should be used to check if the button config was changed or reordered
    ''' it compares the internalnames
    ''' </summary>
    ''' <param name="CurrentInternalNames">String of Buttons</param>
    ''' <returns>boolean</returns>
    Function ButtonsDefinitionChanged(ByVal CurrentInternalNames As String) As Boolean
        Dim back As String = ""
        For Each Item As SingleButtonDefintion In _Buttons
            back = Item.InternalName
        Next
        Return (back = CurrentInternalNames) = False
    End Function

    ''' <summary>
    ''' Get ButtonDefintion on button name
    ''' </summary>
    ''' <param name="ItemString">name of button</param>
    ''' <returns>SingleButtonDefintion or nothing on not found</returns>
    ''' <remarks></remarks>
    Function ItemByName(ByVal ItemString As String) As SingleButtonDefintion
        For Each Item As SingleButtonDefintion In _Buttons
            If Item.Name = ItemString Then Return Item
        Next
        Return Nothing
    End Function
    ''' <summary>
    ''' Get ButtonDefintion on internal button mae
    ''' </summary>
    ''' <param name="ItemString">internal of button</param>
    ''' <returns>SingleButtonDefintion or nothing on not found</returns>
    ''' <remarks></remarks>
    Function ItemByInternalName(ByVal ItemString As String) As SingleButtonDefintion
        For Each Item As SingleButtonDefintion In _Buttons
            If Item.InternalName = ItemString Then Return Item
        Next
        Return Nothing
    End Function
    ''' <summary>
    ''' Get position on internal button name
    ''' </summary>
    ''' <param name="ItemString">internal button name</param>
    ''' <returns>Integer or nothing on not found</returns>
    ''' <remarks></remarks>
    Function IndexByInternalName(ByVal ItemString As String) As Integer
        Dim i As Integer = 0
        For Each Item As SingleButtonDefintion In _Buttons
            If Item.InternalName = ItemString Then Return i
            i += 1
        Next
        Return Nothing
    End Function
    ''' <summary>
    ''' Get position on button name
    ''' </summary>
    ''' <param name="ItemString">button name</param>
    ''' <returns>Integer or nothing on not found</returns>
    ''' <remarks></remarks>
    Function IndexByName(ByVal ItemString As String) As Integer
        Dim i As Integer = 0
        For Each Item As SingleButtonDefintion In _Buttons
            If Item.Name = ItemString Then Return i
            i += 1
        Next
        Return Nothing
    End Function
    ''' <summary>
    ''' Get ButtonDefintion on Index
    ''' </summary>
    ''' <param name="index">position as integer</param>
    ''' <returns>SingleButtonDefintion or nothing on not found</returns>
    ''' <remarks></remarks>
    Function Item(ByVal index As Integer) As SingleButtonDefintion
        If Not index > _Buttons.Count - 1 Then
            Return _Buttons.Item(index)
        Else
            Return Nothing
        End If
    End Function
    ''' <summary>
    ''' All ButtonItems as Listobject
    ''' </summary>
    ''' <value></value>
    ''' <returns>List of all ButtonDefintion</returns>
    ''' <remarks></remarks>
    ReadOnly Property Buttons() As List(Of SingleButtonDefintion)
        Get
            Return _Buttons
        End Get
    End Property
    Public Class SingleButtonDefintion
        Private _Tooltip As String = ""
        Private _AllColorsAsBlack As String = ""
        Private _UpdatePlotstyles As String = ""
        Private _UpdateIProperties As String = ""
        Private _PrintScaleMode As String = ""
        Private _Rotate As String = ""
        Private _WindowsPrinterName As String = ""
        Private _Name As String = ""
        Private _InternalName As String = ""
        Private _FixedPaperSize As String = ""
        Public Property Tooltip() As String
            Get
                Return _Tooltip
            End Get
            Set(ByVal value As String)
                _Tooltip = value
            End Set
        End Property
        Public Property AllColorsAsBlack() As String
            Get
                Return _AllColorsAsBlack
            End Get
            Set(ByVal value As String)
                _AllColorsAsBlack = value
            End Set
        End Property
        Public Property UpdatePlotstyles() As String
            Get
                Return _UpdatePlotstyles
            End Get
            Set(ByVal value As String)
                _UpdatePlotstyles = value
            End Set
        End Property
        Public Property PrintScaleMode() As String
            Get
                Return _PrintScaleMode
            End Get
            Set(ByVal value As String)
                _PrintScaleMode = value
            End Set
        End Property
        Public Property Rotate() As String
            Get
                Return _Rotate
            End Get
            Set(ByVal value As String)
                _Rotate = value
            End Set
        End Property

        Public Property WindowsPrinterName() As String
            Get
                Return _WindowsPrinterName
            End Get
            Set(ByVal value As String)
                _WindowsPrinterName = value
            End Set
        End Property
        Public Property FixedPaperSize() As String
            Get
                Return _FixedPaperSize
            End Get
            Set(ByVal value As String)
                _FixedPaperSize = value
            End Set
        End Property

        Public Property UpdateIProperties() As String
            Get
                Return _UpdateIProperties
            End Get
            Set(ByVal value As String)
                _UpdateIProperties = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property
        Public Property InternalName() As String
            Get
                Return _InternalName
            End Get
            Set(ByVal value As String)
                _InternalName = value
            End Set
        End Property
    End Class
End Class

