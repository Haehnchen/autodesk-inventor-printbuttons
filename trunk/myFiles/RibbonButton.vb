'$Id$
Imports Inventor

Public Class RibbonButton
    Dim WithEvents m_ButtonDefinition As ButtonDefinition
    Public Event pressed(ByVal erg As ButtonDefinition)
    Public Sub New(ByVal AddButtonDefinition As ButtonDefinition)
        m_ButtonDefinition = AddButtonDefinition
    End Sub
    ReadOnly Property ButtonDefinition() As ButtonDefinition
        Get
            Return m_ButtonDefinition
        End Get
    End Property
    Private Sub m_ButtonDefinition_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_ButtonDefinition.OnExecute
        RaiseEvent pressed(m_ButtonDefinition)
    End Sub
End Class
