﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Dieser Code wurde von einem Tool generiert.
'     Laufzeitversion:2.0.50727.3603
'
'     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
'     der Code erneut generiert wird.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'Diese Klasse wurde von der StronglyTypedResourceBuilder automatisch generiert
    '-Klasse über ein Tool wie ResGen oder Visual Studio automatisch generiert.
    'Um einen Member hinzuzufügen oder zu entfernen, bearbeiten Sie die .ResX-Datei und führen dann ResGen
    'mit der /str-Option erneut aus, oder Sie erstellen Ihr VS-Projekt neu.
    '''<summary>
    '''  Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen usw.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Gibt die zwischengespeicherte ResourceManager-Instanz zurück, die von dieser Klasse verwendet wird.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("PrintButtons.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Überschreibt die CurrentUICulture-Eigenschaft des aktuellen Threads für alle
        '''  Ressourcenzuordnungen, die diese stark typisierte Ressourcenklasse verwenden.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Sucht eine lokalisierte Zeichenfolge, die &lt;languages&gt;
        '''	&lt;ErrorText&gt;Error in Addin-In {AddinName}:\r\n&lt;/ErrorText&gt;
        '''	&lt;ErrorWriteFile&gt;Error writing to\r\n&lt;/ErrorWriteFile&gt;
        '''	&lt;ErrorNoIDW&gt;no IDW-File&lt;/ErrorNoIDW&gt;
        '''	&lt;ErrorNoFileName&gt;no filename found; save it first?&lt;/ErrorNoFileName&gt;
        '''	&lt;ErrorFileExistsDelete&gt;file {filename} already exists. overwrite it?&lt;/ErrorFileExistsDelete&gt;
        '''	&lt;FileSaved&gt;{filename} saved.\r\nopen it?&lt;/FileSaved&gt;
        '''	&lt;NoPrinterConfig&gt;No printer config found. need own here:\r\n&lt;/NoPrinterConfig&gt;
        '''	&lt;TabName&gt;Printbuttons&lt;/TabName&gt;
        '''	&lt;PanelN [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        '''</summary>
        Friend ReadOnly Property defaultLanguageStrings() As String
            Get
                Return ResourceManager.GetString("defaultLanguageStrings", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property dwg2007() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("dwg2007", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property dxf() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("dxf", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property ico_large_question() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("ico_large_question", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property pdf() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("pdf", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property pdf_black() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("pdf_black", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
    End Module
End Namespace
