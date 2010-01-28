'$Id$
Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Namespace PrintButtons
    <ProgIdAttribute("PrintButtons.StandardAddInServer"), _
    GuidAttribute("e8b2615b-11aa-4b1b-9d7c-654026c99fac")> _
    Public Class StandardAddInServer
        Implements Inventor.ApplicationAddInServer

        ' Inventor application object.
        Private m_inventorApplication As Inventor.Application

        'use custom buttons
        Dim WithEvents m_buttonPDFColor As ButtonDefinition
        Dim WithEvents m_buttonPDFBlack As ButtonDefinition
        Dim WithEvents m_buttonDWF As ButtonDefinition
        Dim WithEvents m_buttonDWG As ButtonDefinition
        Dim WithEvents m_buttonHelp As ButtonDefinition

        Dim AllButtons As XMLButtonDef

#Region "ApplicationAddInServer Members"

        Public Sub Activate(ByVal addInSiteObject As Inventor.ApplicationAddInSite, ByVal firstTime As Boolean) Implements Inventor.ApplicationAddInServer.Activate

            ' This method is called by Inventor when it loads the AddIn.
            ' The AddInSiteObject provides access to the Inventor Application object.
            ' The FirstTime flag indicates if the AddIn is loaded for the first time.

            ' Initialize AddIn members.
            m_inventorApplication = addInSiteObject.Application
            Try
                Dim m_inventorApp As Inventor.Application = m_inventorApplication
                SettingsLog.Add("DebugLog: " & DebugLog("", True))

                'Dim m_userInterfaceManager As Inventor.UserInterfaceManager = m_inventorApplication.UserInterfaceManager
                'MsgBox(m_inventorApp.LanguageName)

                Dim m_cultureCurrent As System.Globalization.CultureInfo = Threading.Thread.CurrentThread.CurrentCulture
                LanguageStr = New LanguageStrings(My.Resources.defaultLanguageStrings, HelperFunctions.readDllPath & "\languages\" & Threading.Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName & ".xml")
                SettingsLog.Add("CurrentISOLanguageName: " & Threading.Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName)

                Dim AddInCLSID As GuidAttribute = CType(System.Attribute.GetCustomAttribute(GetType(StandardAddInServer), GetType(GuidAttribute)), GuidAttribute)
                Dim AddInCLSIDString As String = "{" & AddInCLSID.Value & "}"

                Dim iNames As String = My.Application.Info.AssemblyName

                ' TODO:  Add ApplicationAddInServer.Activate implementation.
                ' e.g. event initialization, command creation etc.
                ' Create the control definitions.  This is the same for ribbon and classic.

                'Read XML Configfile
                Dim PrinterFile As String = GetConfigFiles()
                If Not PrinterFile = Nothing Then
                    AllButtons = New XMLButtonDef(PrinterFile, iNames)
                    SettingsLog.Add("PrinterConfigFile: " & PrinterFile)
                Else
                    Dim ConfigError As String = GetConfigFiles(True)
                    DebugLog(LanguageStr.s("NoPrinterConfig") & ConfigError)
                    MsgBox(LanguageStr.s("NoPrinterConfig") & ConfigError)
                    Exit Sub
                End If
                Debug.WriteLine(PrinterFile)

                'since inventor 2010 firstTime is true everytime, is this correct?
                'we dont need crazy firstTime workarounds

                Dim controlDefs As ControlDefinitions = m_inventorApp.CommandManager.ControlDefinitions
                m_buttonPDFColor = controlDefs.AddButtonDefinition("PDF", iNames & "intButtonPDF", CommandTypesEnum.kFilePropertyEditCmdType, Nothing, Nothing, Nothing, BitmapToIPicture(My.Resources.pdf), BitmapToIPicture(My.Resources.pdf), ButtonDisplayEnum.kNoTextWithIcon)
                m_buttonPDFBlack = controlDefs.AddButtonDefinition("PDF black", iNames & "intButtonPDFBlack", CommandTypesEnum.kFilePropertyEditCmdType, Nothing, Nothing, Nothing, BitmapToIPicture(My.Resources.pdf_black), BitmapToIPicture(My.Resources.pdf_black), ButtonDisplayEnum.kNoTextWithIcon)
                m_buttonDWF = controlDefs.AddButtonDefinition("DWF", iNames & "intButtonDWF", CommandTypesEnum.kFilePropertyEditCmdType, Nothing, Nothing, Nothing, BitmapToIPicture(My.Resources.dxf), BitmapToIPicture(My.Resources.dxf), ButtonDisplayEnum.kNoTextWithIcon)
                m_buttonDWG = controlDefs.AddButtonDefinition("DWG", iNames & "intButtonDWG", CommandTypesEnum.kFilePropertyEditCmdType, Nothing, Nothing, Nothing, BitmapToIPicture(My.Resources.dwg2007), BitmapToIPicture(My.Resources.dwg2007), ButtonDisplayEnum.kNoTextWithIcon)
                m_buttonHelp = controlDefs.AddButtonDefinition("Info", iNames & "intButtonHelp", CommandTypesEnum.kFilePropertyEditCmdType, Nothing, Nothing, Nothing, BitmapToIPicture(My.Resources.dwg2007), BitmapToIPicture(My.Resources.ico_large_question), ButtonDisplayEnum.kNoTextWithIcon)

                Dim uiManager As Inventor.UserInterfaceManager = m_inventorApp.UserInterfaceManager
                If m_inventorApp.UserInterfaceManager.InterfaceStyle = Inventor.InterfaceStyleEnum.kRibbonInterface Then
                    ' Get the Drawing ribbon and the addin tab.
                    Dim assemblyRibbon As Inventor.Ribbon = uiManager.Ribbons.Item("Drawing")
                    Dim assemblyTab As Inventor.RibbonTab = AddRibbonTab(assemblyRibbon, LanguageStr.s("TabName"), iNames & "Ribbon", AddInCLSIDString)

                    ' Create a new panel on the Assemble tab.
                    Dim PrinterPanel As Inventor.RibbonPanel = AddRibbonPanel(assemblyTab, LanguageStr.s("PanelNamePrinter"), iNames & "RibbonPanelPrinter", AddInCLSIDString)
                    Dim OtherPanel As Inventor.RibbonPanel = AddRibbonPanel(assemblyTab, LanguageStr.s("PanelNameTranslator"), iNames & "RibbonPanelOther", AddInCLSIDString)

                    'add all buttondefinitions to inventor and create handler for buttons
                    For Each ButtonItem As XMLButtonDef.SingleButtonDefintion In AllButtons.Buttons
                        Dim k As New RibbonButton(controlDefs.AddButtonDefinition(ButtonItem.Name, ButtonItem.InternalName, CommandTypesEnum.kFilePropertyEditCmdType, Nothing, ButtonItem.Tooltip, ButtonItem.Tooltip, IPictureDisp(ButtonItem.Name), IPictureDisp(ButtonItem.Name), ButtonDisplayEnum.kNoTextWithIcon))
                        PrinterPanel.CommandControls.AddButton(k.ButtonDefinition)
                        AddHandler k.pressed, AddressOf Me.ButtonEvent
                    Next

                    OtherPanel.CommandControls.AddButton(m_buttonPDFColor, True)
                    OtherPanel.CommandControls.AddButton(m_buttonPDFBlack, True)
                    OtherPanel.CommandControls.AddSeparator()
                    OtherPanel.CommandControls.AddButton(m_buttonDWF, True)
                    OtherPanel.CommandControls.AddButton(m_buttonDWG, True)
                    OtherPanel.CommandControls.AddSeparator()
                    OtherPanel.CommandControls.AddButton(m_buttonHelp, True)

                Else
                    'do we need classic ui interface support?
                    Dim SlotCommandBar1 As CommandBar
                    Dim UserInterfaceManager1 As UserInterfaceManager = m_inventorApplication.UserInterfaceManager

                    If firstTime = True Then
                        DebugLog("firstrun")
                        SlotCommandBar1 = UserInterfaceManager1.CommandBars.Add(LanguageStr.s("PanelNamePrinter"), LanguageStr.s("PanelNamePrinter") & "ToolbarIntName", , AddInCLSIDString)
                    Else
                        SlotCommandBar1 = UserInterfaceManager1.CommandBars.Item(LanguageStr.s("PanelNamePrinter") & "ToolbarIntName")
                        For Each CurrentInventorButton As CommandBarControl In SlotCommandBar1.Controls
                            CurrentInventorButton.Delete()
                        Next
                    End If

                    For Each ButtonItem As XMLButtonDef.SingleButtonDefintion In AllButtons.Buttons
                        Dim k As New RibbonButton(controlDefs.AddButtonDefinition(ButtonItem.Name, ButtonItem.InternalName, CommandTypesEnum.kFilePropertyEditCmdType, Nothing, ButtonItem.Tooltip, ButtonItem.Tooltip, IPictureDisp(ButtonItem.Name), IPictureDisp(ButtonItem.Name), ButtonDisplayEnum.kNoTextWithIcon))
                        SlotCommandBar1.Controls.AddButton(k.ButtonDefinition)
                        AddHandler k.pressed, AddressOf Me.ButtonEvent
                    Next

                    SlotCommandBar1.Controls.AddButton(m_buttonPDFColor)
                    SlotCommandBar1.Controls.AddButton(m_buttonPDFBlack)
                    SlotCommandBar1.Controls.AddButton(m_buttonDWF)
                    SlotCommandBar1.Controls.AddButton(m_buttonDWG)
                    SlotCommandBar1.Controls.AddButton(m_buttonHelp)

                    SlotCommandBar1.Visible = True
                End If

            Catch ex As Exception
                MsgBox(LanguageStr.s("ErrorText").Replace("{AddinName}", My.Application.Info.AssemblyName) & ex.Message)
            End Try

        End Sub

        ''' <summary>
        ''' Example EventHandler for dynamic Inventor Buttons
        ''' Generate it with Class RibbonButton
        ''' </summary>
        ''' <param name="pressedButton">ButtonDefinition of the pressed Button</param>
        ''' <remarks></remarks>
        Sub ButtonEvent(ByVal pressedButton As ButtonDefinition)
            'do only on IDW-File
            If m_inventorApplication.ActiveDocumentType = DocumentTypeEnum.kDrawingDocumentObject Then
                'get the name of the pressed button and get the info about it
                Dim PrinterButtonDef As XMLButtonDef.SingleButtonDefintion = AllButtons.ItemByName(pressedButton.DisplayName)
                Dim Plotter As inventorplotclass = HelperFunctions.GeneratePlotterClass(m_inventorApplication.ActiveDocument, PrinterButtonDef)
                Plotter.plot()
            Else
                MsgBox(LanguageStr.s("ErrorNoIDW"), MsgBoxStyle.Critical)
            End If

        End Sub
        Enum TranslatorAddinEnum
            dwg = 1 : pdf = 2 : dwf = 3
        End Enum
        ''' <summary>
        ''' simple sub for dynamic translator addin usage
        ''' </summary>
        ''' <param name="type">TranslatorAddinEnum</param>
        ''' <param name="AllColorASBlack">only for PDF translator</param>
        ''' <remarks></remarks>
        Private Sub TranslatorAddinHelper(ByVal type As TranslatorAddinEnum, Optional ByVal AllColorASBlack As Boolean = False)
            'generate output filename depend on TranslatorAddinEnum
            Dim OutPutFile As String = m_inventorApplication.ActiveDocument.FullFileName & "." & [Enum].GetName(GetType(TranslatorAddinEnum), type)

            'check if we get a fullname of current dacument
            If m_inventorApplication.ActiveDocument.FullFileName.Length = 0 Then
                MsgBox(LanguageStr.s("ErrorNoFileName"), MsgBoxStyle.Critical)
                Exit Sub
            End If

            'check if we want to overwrite the file if it exists
            If HelperFileExistsOverwrite(OutPutFile) = False Then Exit Sub

            'use translator function depend TranslatorAddinEnum
            Select Case type
                Case TranslatorAddinEnum.dwg
                    'TODO: find translator id for dwg
                    Dim oDoc As Document = m_inventorApplication.ActiveDocument
                    Call oDoc.SaveAs(OutPutFile, True)
                Case TranslatorAddinEnum.pdf
                    InventorPlotClass.SaveAsPDF(m_inventorApplication, OutPutFile, AllColorASBlack)
                Case TranslatorAddinEnum.dwf
                    InventorPlotClass.SaveAsDWF(m_inventorApplication, OutPutFile)
            End Select

            'open saved file
            If MsgBox(LanguageStr.s("FileSaved").Replace("{filename}", OutPutFile), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                ExecuteFile(OutPutFile)
            End If

        End Sub
#Region "InternalButtons"
        Private Sub m_buttonDWG_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_buttonDWG.OnExecute
            TranslatorAddinHelper(TranslatorAddinEnum.dwg)
        End Sub
        Private Sub m_buttonDWF_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_buttonDWF.OnExecute
            TranslatorAddinHelper(TranslatorAddinEnum.dwf)
        End Sub
        Private Sub m_buttonPDFColor_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_buttonPDFColor.OnExecute
            TranslatorAddinHelper(TranslatorAddinEnum.pdf, False)
        End Sub
        Private Sub m_buttonPDFBlack_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_buttonPDFBlack.OnExecute
            TranslatorAddinHelper(TranslatorAddinEnum.pdf, True)
        End Sub
        Private Sub m_buttonHelp_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_buttonHelp.OnExecute
            Dim b As String = ""
            For Each strline As String In SettingsLog
                b &= strline & vbCrLf
            Next
            MsgBox(b, MsgBoxStyle.Information)
        End Sub
#End Region

        Public Sub Deactivate() Implements Inventor.ApplicationAddInServer.Deactivate

            ' This method is called by Inventor when the AddIn is unloaded.
            ' The AddIn will be unloaded either manually by the user or
            ' when the Inventor session is terminated.

            ' TODO:  Add ApplicationAddInServer.Deactivate implementation

            ' Release objects.
            Marshal.ReleaseComObject(m_inventorApplication)
            m_inventorApplication = Nothing

            System.GC.WaitForPendingFinalizers()
            System.GC.Collect()

        End Sub

        Public ReadOnly Property Automation() As Object Implements Inventor.ApplicationAddInServer.Automation

            ' This property is provided to allow the AddIn to expose an API 
            ' of its own to other programs. Typically, this  would be done by
            ' implementing the AddIn's API interface in a class and returning 
            ' that class object through this property.

            Get
                Return Nothing
            End Get

        End Property

        Public Sub ExecuteCommand(ByVal commandID As Integer) Implements Inventor.ApplicationAddInServer.ExecuteCommand

            ' Note:this method is now obsolete, you should use the 
            ' ControlDefinition functionality for implementing commands.

        End Sub

#End Region

#Region "COM Registration"

        ' Registers this class as an AddIn for Inventor.
        ' This function is called when the assembly is registered for COM.
        <ComRegisterFunctionAttribute()> _
        Public Shared Sub Register(ByVal t As Type)

            Dim clssRoot As RegistryKey = Registry.ClassesRoot
            Dim clsid As RegistryKey = Nothing
            Dim subKey As RegistryKey = Nothing

            Try
                clsid = clssRoot.CreateSubKey("CLSID\" + AddInGuid(t))
                clsid.SetValue(Nothing, "PrintButtons")
                subKey = clsid.CreateSubKey("Implemented Categories\{39AD2B5C-7A29-11D6-8E0A-0010B541CAA8}")
                subKey.Close()

                subKey = clsid.CreateSubKey("Settings")
                subKey.SetValue("AddInType", "Standard")
                subKey.SetValue("LoadOnStartUp", "1")

                'subKey.SetValue("SupportedSoftwareVersionLessThan", "")
                subKey.SetValue("SupportedSoftwareVersionGreaterThan", "13..")
                'subKey.SetValue("SupportedSoftwareVersionEqualTo", "")
                'subKey.SetValue("SupportedSoftwareVersionNotEqualTo", "")
                'subKey.SetValue("Hidden", "0")
                'subKey.SetValue("UserUnloadable", "1")
                subKey.SetValue("Version", 0)
                subKey.Close()

                subKey = clsid.CreateSubKey("Description")
                subKey.SetValue(Nothing, "PrintButtons")

            Catch ex As Exception
                System.Diagnostics.Trace.Assert(False)
            Finally
                If Not subKey Is Nothing Then subKey.Close()
                If Not clsid Is Nothing Then clsid.Close()
                If Not clssRoot Is Nothing Then clssRoot.Close()
            End Try

        End Sub

        ' Unregisters this class as an AddIn for Inventor.
        ' This function is called when the assembly is unregistered.
        <ComUnregisterFunctionAttribute()> _
        Public Shared Sub Unregister(ByVal t As Type)

            Dim clssRoot As RegistryKey = Registry.ClassesRoot
            Dim clsid As RegistryKey = Nothing

            Try
                clssRoot = Microsoft.Win32.Registry.ClassesRoot
                clsid = clssRoot.OpenSubKey("CLSID\" + AddInGuid(t), True)
                clsid.SetValue(Nothing, "")
                clsid.DeleteSubKeyTree("Implemented Categories\{39AD2B5C-7A29-11D6-8E0A-0010B541CAA8}")
                clsid.DeleteSubKeyTree("Settings")
                clsid.DeleteSubKeyTree("Description")
            Catch
            Finally
                If Not clsid Is Nothing Then clsid.Close()
                If Not clssRoot Is Nothing Then clssRoot.Close()
            End Try

        End Sub

        ' This property uses reflection to get the value for the GuidAttribute attached to the class.
        Public Shared ReadOnly Property AddInGuid(ByVal t As Type) As String
            Get
                Dim guid As String = ""
                Try
                    Dim customAttributes() As Object = t.GetCustomAttributes(GetType(GuidAttribute), False)
                    Dim guidAttribute As GuidAttribute = CType(customAttributes(0), GuidAttribute)
                    guid = "{" + guidAttribute.Value.ToString() + "}"
                Finally
                    AddInGuid = guid
                End Try
            End Get
        End Property

#End Region

    End Class

End Namespace

