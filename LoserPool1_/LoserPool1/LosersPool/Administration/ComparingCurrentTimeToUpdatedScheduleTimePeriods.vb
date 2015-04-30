'Imports System.Web
'Imports System.Web.UI
Imports System.Linq
Imports System.Xml.Linq
Imports System.Globalization

Imports LoserPool1.LosersPool.Models
Imports LoserPool1.LosersPool.Administration

Public Class ComparingCurrentTimeToUpdatedScheduleTimePeriods
    Private _dbLoserPool As New LosersPoolContext

    Public Sub New()
        Try
            Using (_dbLoserPool)

                Dim scheduleTimePeriodMemory As New List(Of ScheduleTimePeriodMemory)

                Dim cDateTime As Date = Date.Now
                Dim currentDateTime = ConvertToDateAndTime.ConvertDateTime(cDateTime)
                Dim currentDate As String = ConvertToDateAndTime.FilterDate(currentDateTime)
                Dim currentTime1 = ConvertToDateAndTime.FilterTime(currentDateTime)
                Dim dateI = CInt(currentDate)
                Dim timeI = ConvertToDateAndTime.IntegerTime(currentTime1)

                Dim myUpdate = XDocument.Load("C:\Users\Larry\Documents\GitHub\RussBucks\scoringUpdateWeek1Update1.xml")

                Dim queryTime = (From score In myUpdate.Descendants("scores")
                                 Select New ScoreUpdateXML With {.filetime = score.Attribute("filetime"),
                                                                 .filedate = score.Attribute("filedate")}).ToList

                Dim fileDateI = CInt(queryTime(0).filedate)
                Dim fileTimeI = ConvertToDateAndTime.IntegerTime(queryTime(0).filetime)

                Dim queryGame = (From game In myUpdate.Descendants("scores").Descendants("game")
                                 Select New GameUpdateXML With {.hometeam = game.Attribute("hometeam").Value,
                                                               .homescore = game.Elements("homescore").Value,
                                                               .awayteam = game.Attribute("awayteam").Value,
                                                               .awayscore = game.Elements("awayscore").Value,
                                                               .gametime = game.Elements("gametime").Value}).ToList

                Dim queryTimePeriod As New List(Of ScheduleTimePeriod)
                queryTimePeriod = _dbLoserPool.ScheduleTimePeriods.ToList

                Dim WeekIsFinished As Boolean = True
                For Each game In queryGame
                    If Not (game.gametime = "final") Then
                        WeekIsFinished = False
                        Exit For
                    End If
                Next

                If WeekIsFinished = True Then

                    For Each timePeriod In queryTimePeriod
                        If timePeriod.UserEntryStartDate Is Nothing And timePeriod.UserEntryStartTime Is Nothing Then

                        Else
                            If timePeriod.GamesFinishedDate Is Nothing And timePeriod.GamesFinishedTime Is Nothing Then
                                'I think we are in time period B - user is looked out of user entry but can see scoring updates
                            Else
                                If fileDateI > CInt(timePeriod.GamesFinishedDate) And fileTimeI > CInt(timePeriod.GamesFinishedTime) Then
                                    ' I think we  need find a new time period
                                Else
                                    If timeI > CInt(timePeriod.UserEntryStartTime) And timeI < CInt(timePeriod.UserEntryFinshedGamesStartTime) Then
                                        ' I think we are in the User data entry time period
                                    End If
                                End If

                            End If

                        End If

                    Next


                Else

                    For Each timePeriod In queryTimePeriod
                        If timePeriod.UserEntryStartDate Is Nothing And timePeriod.UserEntryStartTime Is Nothing Then

                        Else
                            If timePeriod.GamesFinishedDate Is Nothing And timePeriod.GamesFinishedTime Is Nothing Then
                                'I think we are in time period B - user is looked out of user entry but can see scoring updates

                                Dim dummy1 = "dummy"
                            Else
                                If fileDateI > CInt(timePeriod.GamesFinishedDate) And fileTimeI > CInt(timePeriod.GamesFinishedTime) Then
                                    ' I think we  need find a new time period
                                    Dim dummy2 = "dummy"

                                Else
                                    If timeI > CInt(timePeriod.UserEntryStartTime) And timeI < CInt(timePeriod.UserEntryFinshedGamesStartTime) Then
                                        ' I think we are in the User data entry time period
                                        Dim dummy3 = "dummy"
                                    End If
                                End If

                            End If

                        End If

                    Next


                End If



                'Where ((fileDateI >= timePeriod.UserEntryFinshedGamesStartDate And fileTimeI >= timePeriod.UserEntryFinshedGamesStartTime) And
                '(Not (timePeriod.UserEntryStartDate Is Nothing) And Not (timePeriod.UserEntryStartTime Is Nothing)))).ToList

                Dim dummy = "dummy"

            End Using
        Catch ex As Exception

        End Try
    End Sub

End Class
