Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Linq
Imports System.Data

Imports LoserPool1.LosersPool.Models

Imports LoserPool1.JoinPools
Imports LoserPool1.JoinPools.Models


Public Class ReadScheduleTimePeriodXMLFile

    Private _dbLoserPool As New LosersPoolContext

    Public Sub New(filepath As String)

        Try
            Using (_dbLoserPool)

                Dim scheduleTimePeriodList = XDocument.Load(filepath)

                Dim queryScheduleTimePeriod = (From weekElement In scheduleTimePeriodList.Descendants("scheduleTimePeriod").Descendants("week")
                                               Select New scheduleTimePeriodXML With {.weekId = weekElement.Attribute("weekNumber").Value,
                                                                                      .startWeekTime = weekElement.Elements("startWeekTime").Value,
                                                                                      .startWeekDate = weekElement.Elements("startWeekDate").Value,
                                                                                      .startGameTime = weekElement.Elements("startGameTime").Value,
                                                                                      .startGameDate = weekElement.Elements("startGameDate").Value,
                                                                                      .endWeekTime = weekElement.Elements("endGameTime").Value,
                                                                                      .endWeekDate = weekElement.Elements("endGameDate").Value}).ToList

                For Each timePeriod In queryScheduleTimePeriod

                    Dim timePeriodEntity = New ScheduleTimePeriod
                    timePeriodEntity.weekID = timePeriod.weekId

                    If timePeriod.startWeekDate = "" Then
                        timePeriodEntity.UserEntryFinshedGamesStartDate = Nothing
                    Else
                        timePeriodEntity.UserEntryStartDate = timePeriod.startWeekDate
                    End If

                    If (timePeriod.startWeekTime = "") Then
                        timePeriodEntity.UserEntryStartTime = Nothing
                    Else
                        timePeriodEntity.UserEntryStartTime = timePeriod.startWeekTime
                    End If

                    timePeriodEntity.UserEntryFinshedGamesStartDate = timePeriod.startGameDate
                    timePeriodEntity.UserEntryFinshedGamesStartTime = timePeriod.startGameTime

                    If (timePeriod.endWeekDate = "") Then
                        timePeriodEntity.GamesFinishedDate = Nothing
                    Else
                        timePeriodEntity.GamesFinishedDate = timePeriod.endWeekDate
                    End If

                    If (timePeriod.endWeekDate = "") Then
                        timePeriodEntity.GamesFinishedTime = Nothing
                    Else
                        timePeriodEntity.GamesFinishedTime = timePeriod.endWeekTime
                    End If

                    _dbLoserPool.ScheduleTimePeriods.Add(timePeriodEntity)
                    _dbLoserPool.SaveChanges()

                Next

                Dim dummy = "dummy"

            End Using
        Catch ex As Exception

        End Try

    End Sub

End Class

Public Class UserChoiceList

    Private _dbLoserPool As New LosersPoolContext

    Public Sub New(filepath As String, wNumber As String)
        Try
            Using (_dbLoserPool)

                Dim queryUser1 = (From users1 In _dbLoserPool.UserChoicesList).ToList

                If _dbLoserPool.UserChoicesList.Count >= 1 Then
                    Exit Sub
                End If

                _dbLoserPool.SaveChanges()


                Dim userChoicesXDocument = XDocument.Load(filepath)
                'Dim user1 As New userChoicesXML
                Dim WeeklyPossibleChoicesForAllUsers = (From weekElement In userChoicesXDocument.Descendants("UserChoicesList").Descendants("Week").Descendants("User")
                                                        Select New userChoicesXML With {.UserID = weekElement.Attribute("UserId").Value,
                                                                                        .WeekId = weekElement.Attribute("WeekId").Value,
                                                                                        .UserName = weekElement.Elements("UserName").Value,
                                                                                        .ConfirmationNumber = CInt(weekElement.Elements("ConfirmationNumber").Value),
                                                                                        .UserPick = weekElement.Elements("UserPick").Value,
                                                                                        .SeaHawks = CBool(weekElement.Elements("SeaHawks").Value),
                                                                                        .Packers = CBool(weekElement.Elements("Packers").Value),
                                                                                        .Falcons = CBool(weekElement.Elements("Falcons").Value),
                                                                                        .Chargers = CBool(weekElement.Elements("Chargers").Value),
                                                                                        .Cardinals = CBool(weekElement.Elements("Cardinals").Value),
                                                                                        .Saints = CBool(weekElement.Elements("Saints").Value),
                                                                                        .Lions = CBool(weekElement.Elements("Lions").Value),
                                                                                        .Giants = CBool(weekElement.Elements("Giants").Value),
                                                                                        .Contender = CBool(weekElement.Elements("Contender").Value)}).ToList

                For Each user1 In WeeklyPossibleChoicesForAllUsers
                    Dim user2 = New UserChoices
                    user2.ListId = _dbLoserPool.UserChoicesList.Count + 1
                    user2.UserID = user1.UserID
                    user2.UserName = user1.UserName
                    user2.WeekId = user1.WeekId
                    user2.ConfirmationNumber = user1.ConfirmationNumber
                    user2.Contender = user1.Contender
                    user2.UserPick = user1.UserPick
                    user2.SeaHawks = user1.SeaHawks
                    user2.Packers = user1.Packers
                    user2.Falcons = user1.Falcons
                    user2.Saints = user1.Saints
                    user2.Chargers = user1.Chargers
                    user2.Cardinals = user1.Cardinals
                    user2.Lions = user1.Lions
                    user2.Giants = user1.Giants

                    _dbLoserPool.UserChoicesList.Add(user2)
                Next

                _dbLoserPool.SaveChanges()

                Dim dummy = "dummy"
            End Using
        Catch ex As Exception

        End Try

    End Sub
End Class

Public Class UserList

    Public Sub New(filepath As String)
        Dim _dbLoserPool As New LosersPoolContext
        Try
            Using (_dbLoserPool)

                Dim queryUser1 = (From users1 In _dbLoserPool.Users).ToList

                If _dbLoserPool.Users.Count >= 1 Then
                    Exit Sub
                End If

                _dbLoserPool.SaveChanges()

                Dim userListXDocument = XDocument.Load(filepath)
                'Dim user1 As New UserXML
                Dim AllUsers = (From user2 In userListXDocument.Descendants("UserList").Descendants("User")
                               Select New UserXML With {.UserId = user2.Attribute("UserId").Value,
                               .UserName = user2.Elements("UserName").Value,
                               .Administrator = CBool(user2.Elements("Adminstrator").Value),
                               .OptionState = user2.Elements("OptionState").Value}).ToList

                For Each user1 In AllUsers
                    Dim user2 As New User
                    user2.UserId = user1.UserId
                    user2.UserName = user1.UserName
                    user2.Administrator = user1.Administrator
                    If user1.OptionState = "" Then
                        user2.OptionState = Nothing
                    Else
                        user2.OptionState = user1.OptionState

                    End If
                    _dbLoserPool.Users.Add(user2)
                Next

                _dbLoserPool.SaveChanges()
            End Using
        Catch ex As Exception

        End Try
    End Sub
End Class