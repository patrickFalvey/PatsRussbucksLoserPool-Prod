Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace LosersPool.Models
    Public Class User
        <Key>
        Public Property UserId As String

        <Required>
        Public Property UserName As String

        Public Property Administrator As Boolean

        Public Property OptionState As String

    End Class

    Public Class UserXML

        Public Property UserId As String

        Public Property UserName As String

        Public Property Administrator As Boolean

        Public Property OptionState As String

    End Class

    Public Class UserChoices

        <Key>
        Public Property ListId As Int32

        Public Property UserID As String

        Public Property UserName As String

        Public Property WeekId As String

        Public Property ConfirmationNumber As Int32

        Public Property UserPick As String

        Public Property Contender As Boolean

        Public Property SeaHawks As Boolean

        Public Property Packers As Boolean

        Public Property Falcons As Boolean

        Public Property Saints As Boolean

        Public Property Chargers As Boolean

        Public Property Cardinals As Boolean

        Public Property Lions As Boolean

        Public Property Giants As Boolean

        Public Overridable Property PossibleUserPicks As New List(Of String)

        Public Overridable Property RadioButtonsForUserPicks As List(Of String)


    End Class

    Public Class userChoicesXML

        Public Property UserID As String
        Public Property UserName As String
        Public Property WeekId As String
        Public Property ConfirmationNumber As Int32
        Public Property UserPick As String
        Public Property SeaHawks As Boolean
        Public Property Packers As Boolean
        Public Property Falcons As Boolean
        Public Property Saints As Boolean
        Public Property Chargers As Boolean
        Public Property Cardinals As Boolean
        Public Property Lions As Boolean
        Public Property Giants As Boolean
        Public Property Contender As Boolean

    End Class


    Public Class UserResult

        <Key>
        Public Property ListId As Int32

        Public Property UserID As String

        Public Property UserName As String

        Public Property WeekId As String

        Public Property SeaHawks As Boolean

        Public Property Packers As Boolean

        Public Property Falcons As Boolean

        Public Property Saints As Boolean

        Public Property Chargers As Boolean

        Public Property Cardinals As Boolean

        Public Property Lions As Boolean

        Public Property Giants As Boolean

    End Class
End Namespace
