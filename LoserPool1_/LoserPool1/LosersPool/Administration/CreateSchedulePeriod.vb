Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Linq
Imports System.Data

Imports LoserPool1.LosersPool.Models

Imports LoserPool1.JoinPools
Imports LoserPool1.JoinPools.Models

Public Class CreateSchedulePeriod

    Private _dbLoserPool As New LosersPoolContext

    Public Sub New()

        If _dbLoserPool.ScheduleTimePeriods.Count >= 1 Then
            Exit Sub
        End If


        Try
            Using (_dbLoserPool)

                Dim games = New List(Of ScheduleEntity)
                games = _dbLoserPool.ScheduleEntities.ToList

                Dim weeks = New Dictionary(Of String, String)
                For Each game In games

                    Dim week1 = game.WeekId
                    If Not (weeks.ContainsKey(week1)) Then
                        weeks.Add(week1, week1)
                    End If
                Next

                For Each week2 In weeks

                    Dim weeklygames = New List(Of ScheduleEntity)
                    weeklygames = _dbLoserPool.ScheduleEntities.Where(Function(wg) wg.WeekId = week2.Key).ToList

                    Dim minStartDate As String
                    minStartDate = weeklygames(0).StartDate
                    For gamenum = 1 To weeklygames.Count - 1
                        If weeklygames(gamenum).StartDate < minStartDate Then
                            minStartDate = weeklygames(gamenum).StartDate
                        End If
                    Next

                    Dim weeklygames1 = New List(Of ScheduleEntity)
                    weeklygames1 = _dbLoserPool.ScheduleEntities.Where(Function(wg) wg.WeekId = week2.Key And wg.StartDate = minStartDate).ToList

                    Dim minStartTime As String
                    minStartTime = weeklygames1(0).StartTime
                    For gamenum = 1 To weeklygames1.Count - 1
                        If weeklygames1(gamenum).StartTime < minStartTime Then
                            minStartTime = weeklygames1(gamenum).StartTime
                        End If
                    Next

                    Dim scheduleTimePeriod = New ScheduleTimePeriod
                    scheduleTimePeriod.weekID = week2.Key

                    If week2.Key = "week1" Then
                        scheduleTimePeriod.UserEntryStartDate = "3/15/15"
                        scheduleTimePeriod.UserEntryStartTime = "12:01 AM"
                    End If

                    scheduleTimePeriod.UserEntryFinshedGamesStartTime = minStartTime
                    scheduleTimePeriod.UserEntryFinshedGamesStartDate = minStartDate

                    _dbLoserPool.ScheduleTimePeriods.Add(scheduleTimePeriod)
                    _dbLoserPool.SaveChanges()

                    Dim dummy = "dummy"

                Next




            End Using
        Catch ex As Exception

        End Try


    End Sub

    Private Shared Function ClearExistingScheduleTimePeriodList(_dbLoserPool As LosersPoolContext) As LosersPoolContext

        Try

            Dim queryScheduleTimePeriod = (From game In _dbLoserPool.ScheduleTimePeriods).ToList

            For Each timePeriod In queryScheduleTimePeriod
                _dbLoserPool.ScheduleTimePeriods.Remove(timePeriod)
            Next

            _dbLoserPool.SaveChanges()
            Return _dbLoserPool


        Catch ex As Exception

        End Try

        Return Nothing

    End Function

End Class
