Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema


Public Class ScheduleTimePeriod

    <Key>
    Public Property weekID As String

    Public Property UserEntryStartTime As String

    Public Property UserEntryStartDate As String

    Public Property UserEntryFinshedGamesStartTime As String

    Public Property UserEntryFinshedGamesStartDate As String

    Public Property GamesFinishedTime As String

    Public Property GamesFinishedDate As String

End Class

Public Class ScheduleTimePeriodMemory

    Public Property weekID As String

    Public Property UserEntryStartTime As Int32

    Public Property UserEntryStartDate As Int32

    Public Property UserEntryFinshedGamesStartTime As Int32

    Public Property UserEntryFinshedGamesStartDate As Int32

    Public Property GamesFinishedTime As Int32

    Public Property GamesFinishedDate As Int32


End Class
