
Namespace LosersPool.Models
    Public Class gameXML
        Public Property startTime As String
        Public Property startDate As String
        Public Property homeTeam As String
        Public Property awayTeam As String
    End Class

    Public Class weekXML
        Public Property weekNumber As String
    End Class

    Public Class weeklyFileXML
        Public Property weekFilePath As String
    End Class

    Public Class scheduleTimePeriodXML

        Public Property weekId As String
        Public Property startWeekTime As String
        Public Property startWeekDate As String
        Public Property startGameTime As String
        Public Property startGameDate As String
        Public Property endWeekTime As String
        Public Property endWeekDate As String

    End Class


End Namespace
