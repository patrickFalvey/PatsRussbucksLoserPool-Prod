Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema


Public Class ScheduleEntity

    <Key>
    Public Property GameId As String

    Public Property WeekId As String

    Public Property HomeTeam As String

    Public Property AwayTeam As String

    Public Property StartTime As String

    Public Property StartDate As String

    Public Property HomeScore As String

    Public Property AwayScore As String

    Public Property GameTime As String

End Class

Public Class GameUpdate

    Public Property GameId As String

    Public Property HomeTeam As String

    Public Property AwayTeam As String

    Public Property HomeScore As String

    Public Property AwayScore As String

    Public Property GameTime As String

    Public Property UserHandles As New List(Of String)

    Public Property HomeTeamAvailability As New Dictionary(Of String, String)

    Public Property AwayTeamAvailability As New Dictionary(Of String, String)

End Class