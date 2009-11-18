'$Id$
Imports Inventor
Imports System.xml
Imports System.Reflection
Imports System.Drawing
Imports System.IO

Module HelperFunctions
    Public LanguageStr As LanguageStrings
    Public SettingsLog As New ArrayList
    Public myGlobalSettings As New Settings(readDllPath() & "\settings.xml")
    Public PrinterConfigFile As String = myGlobalSettings.s("PrinterConfigFile", "printer.xml")

    Function GeneratePlotterClass(ByVal ActiveDocument As Inventor.Document, ByVal printer As XMLButtonDef.SingleButtonDefintion) As InventorPlotClass
        Try
            Dim test As New InventorPlotClass(ActiveDocument)
            test.SetSystemPrinter = printer.WindowsPrinterName

            If Not printer.AllColorsAsBlack = "" Then test.AllColorsAsBlack = Convert.ToBoolean(printer.AllColorsAsBlack)
            If Not printer.UpdatePlotstyles = "" Then test.UpdatePlotstyles = Convert.ToBoolean(printer.UpdatePlotstyles)
            If Not printer.UpdateIProperties = "" Then test.UpdateIProperties = Convert.ToBoolean(printer.UpdateIProperties)

            If Not printer.Rotate = "" Then
                If printer.Rotate.Contains("A0") Then test.RotatePlot.A0 = True
                If printer.Rotate.Contains("A1") Then test.RotatePlot.A1 = True
                If printer.Rotate.Contains("A2") Then test.RotatePlot.A2 = True
                If printer.Rotate.Contains("A3") Then test.RotatePlot.A3 = True
                If printer.Rotate.Contains("A4") Then test.RotatePlot.A4 = True
            End If

            If Not printer.FixedPaperSize = "" Then
                Dim strPaper As String = CType(printer.FixedPaperSize, String)
                Dim enumValue As PaperSizeEnum = CType([Enum].Parse(GetType(PaperSizeEnum), strPaper), PaperSizeEnum)
                If enumValue > 0 Then test.FixedPaperSize = enumValue
            End If

            If Not printer.PrintScaleMode = "" Then
                Dim strPaper As String = CType(printer.PrintScaleMode, String)
                Dim enumValue As PrintScaleModeEnum = CType([Enum].Parse(GetType(PrintScaleModeEnum), strPaper), PrintScaleModeEnum)
                If enumValue > 0 Then test.PrintScaleMode = enumValue
            End If

            Return test
        Catch ex As Exception
            MsgBox(LanguageStr.s("ErrorText") & ex.Message)
            Return Nothing
        End Try
    End Function
    Function DebugLog(ByVal str As String, Optional ByVal ReturnLogPathOnly As Boolean = False) As String
        Dim sFilePath As String = System.IO.Path.GetTempPath & "inv-" & My.Application.Info.AssemblyName.ToLower & ".log"
        If ReturnLogPathOnly = True Then Return sFilePath

        Dim dateTimeInfo As DateTime = DateTime.Now
        Try
            Dim streami As FileStream = New FileStream(sFilePath, FileMode.Append)
            Dim SWriter As StreamWriter = New StreamWriter(streami, System.Text.Encoding.Default)
            SWriter.WriteLine(dateTimeInfo.ToString & " - " & str)
            SWriter.Close()
        Catch ex As Exception
            MsgBox(LanguageStr.s("ErrorWriteFile") & sFilePath & ex.Message & vbCrLf & str)
        End Try
        Return ""
    End Function
    Function readDllPath() As String
        Dim myAssy As [Assembly]
        myAssy = [Assembly].GetExecutingAssembly
        Dim filePath As String = myAssy.Location
        filePath = filePath.Substring(0, filePath.LastIndexOf("\"))
        Return filePath
    End Function
    Function IPictureDisp(ByVal name As String) As Object
        Dim Pfad As String = HelperFunctions.readDllPath()
        If System.IO.File.Exists(Pfad & "\icos\" & name & ".gif") Then
            Dim ico As New Bitmap(Pfad & "\icos\" & name & ".gif")
            Return Microsoft.VisualBasic.Compatibility.VB6.Support.IconToIPicture(System.Drawing.Icon.FromHandle(ico.GetHicon()))
        Else
            Return Nothing
        End If
    End Function
    Function BitmapToIPicture(ByVal iBitmap As Bitmap) As Object
        Return Microsoft.VisualBasic.Compatibility.VB6.Support.IconToIPicture(System.Drawing.Icon.FromHandle(iBitmap.GetHicon()))
    End Function
    ''' <summary>
    ''' Add a RibbonTab to Inventor Ribbon interface and return the ribbon
    ''' if exists it return only the ribbon tab
    ''' 
    ''' http://discussion.autodesk.com/forums/thread.jspa?messageID=6225731
    ''' </summary>
    ''' <param name="objEnvironment"></param>
    ''' <param name="strDisplayName">DisplayName of the Ribbon</param>
    ''' <param name="strInternalName">InternalName must be unique</param>
    ''' <param name="strCLSID">a Guid String; should be Addin Guid</param>
    ''' <returns>RibbonTag object</returns>
    ''' <remarks></remarks>
    Function AddRibbonTab(ByVal objEnvironment As Inventor.Ribbon, ByVal strDisplayName As String, ByVal strInternalName As String, ByVal strCLSID As String) As RibbonTab
        Dim objRibbonTab As RibbonTab = Nothing
        Try
            Try
                objRibbonTab = objEnvironment.RibbonTabs.Add(strDisplayName, strInternalName, strCLSID)
            Catch ex As Exception
                objRibbonTab = objEnvironment.RibbonTabs.Item(strInternalName)
            End Try
        Catch ex As Exception
            'Debug.WriteLine(ex)
            MsgBox(ex.Message)
        End Try
        Return objRibbonTab
    End Function
    ''' <summary>
    ''' Add a RibbonPanel to a InventorRibbonTab and return the tab
    ''' if exists it return only the tab
    ''' 
    ''' http://discussion.autodesk.com/forums/thread.jspa?messageID=6225731
    ''' </summary>
    ''' <param name="objRibbonTab">RibbonTab the Panel should be shown on</param>
    ''' <param name="strDisplayName">DisplayName of the Tab</param>
    ''' <param name="strInternalName">InternalName must be unique</param>
    ''' <param name="strCLSID">a Guid String; should be Addin Guid</param>
    ''' <returns>RibbonPanel object</returns>
    ''' <remarks></remarks>
    Function AddRibbonPanel(ByVal objRibbonTab As RibbonTab, ByVal strDisplayName As String, ByVal strInternalName As String, ByVal strCLSID As String) As RibbonPanel
        Dim objRibbonPanel As RibbonPanel = Nothing
        Try
            Try
                objRibbonPanel = objRibbonTab.RibbonPanels.Add(strDisplayName, strInternalName, strCLSID)
            Catch ex As Exception
                objRibbonPanel = objRibbonTab.RibbonPanels.Item(strInternalName)
            End Try
        Catch ex As Exception
            'Debug.WriteLine(ex)
            MsgBox(ex.Message)
        End Try
        Return objRibbonPanel
    End Function
    ''' <summary>
    ''' Checks if the file exits and ask if it should overwrite
    ''' </summary>
    ''' <param name="OutPutFile"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function HelperFileExistsOverwrite(ByVal OutPutFile As String) As Boolean
        If System.IO.File.Exists(OutPutFile) Then
            If MsgBox(LanguageStr.s("ErrorFileExistsDelete").Replace("{filename}", OutPutFile), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                System.IO.File.Delete(OutPutFile)
                Return True
            Else
                Return False
            End If
        End If
        Return True
    End Function
    ''' <summary>
    ''' Reads Printer Config
    ''' </summary>
    ''' <param name="returnPaths">for debugging only; prints all search paths</param>
    ''' <returns>FullName of Configfile</returns>
    ''' <remarks></remarks>
    Function GetConfigFiles(Optional ByVal returnPaths As Boolean = False) As String
        Dim cfgFile As New ArrayList
        Dim retString As String = Nothing

        'search a ConfigFile in this order: MyDocuments, Path in SettingsFile, File in DLL Path
        cfgFile.Add(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\" & PrinterConfigFile)
        If Not myGlobalSettings.s("PrinterConfigNetworkPath") = "" Then cfgFile.Add(myGlobalSettings.s("PrinterConfigNetworkPath") & "\" & PrinterConfigFile)
        cfgFile.Add(HelperFunctions.readDllPath & "\" & PrinterConfigFile)

        For Each file As String In cfgFile
            If returnPaths = True Then
                retString &= file & vbNewLine
            Else
                If System.IO.File.Exists(file) Then Return file
            End If

        Next
        Return retString

    End Function
    Sub ExecuteFile(ByVal OutPutFile As String)
        Dim FileExe As New System.Diagnostics.Process
        FileExe.StartInfo.FileName = OutPutFile
        FileExe.Start()

    End Sub
 
End Module
