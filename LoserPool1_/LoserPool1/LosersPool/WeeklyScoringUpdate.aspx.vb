Imports System.Data

Imports System
Imports System.Collections.Specialized
Imports System.Collections
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls


Imports LoserPool1
Imports LoserPool1.LosersPool.Models
Imports LoserPool1.JoinPools.Models

Public Class WeeklyScoringUpdate
    Inherits System.Web.UI.Page

    Private GameUpdateCollection As New Dictionary(Of String, GameUpdate)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("userId") Is Nothing Then
            Response.Redirect("~/Account/Login.aspx")
        End If


        Dim thisWeek = CStr(Session("weekNumber"))

        Dim _DbLoserPool As New LosersPoolContext
        Dim _DbPools2 As New PoolDbContext

        Try
            Using (_DbLoserPool)
                Using (_DbPools2)

                    Dim myUpdate = XDocument.Load("C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\scoringUpdate.xml")

                    Dim teams1 As New List(Of Team)

                    Dim weeklyGames As New List(Of ScheduleEntity)

                    teams1 = (From teams2 In _DbPools2.Teams).ToList

                    weeklyGames = (From scheduleEntity1 In _DbLoserPool.ScheduleEntities
                           Where scheduleEntity1.WeekId = thisWeek
                           Select scheduleEntity1).ToList

                    Dim cnt1 = 0
                    For Each game1 In weeklyGames

                        cnt1 = cnt1 + 1
                        Dim gameupdate1 As New GameUpdate

                        gameupdate1.GameId = "game" + CStr(cnt1)
                        gameupdate1.HomeTeam = game1.HomeTeam
                        gameupdate1.AwayTeam = game1.AwayTeam

                        Dim queryUpdateGame1 = (From game In myUpdate.Descendants("scores").Descendants("game")
                                                Where game.Attribute("hometeam").Value = game1.HomeTeam Or game.Attribute("awayteam").Value = game1.AwayTeam)

                        If queryUpdateGame1.Count = 0 Then
                            Continue For
                        End If

                        Dim queryUpdateGame As New List(Of GameUpdateXML)

                        queryUpdateGame = (From game In myUpdate.Descendants("scores").Descendants("game")
                                              Where game.Attribute("hometeam").Value = game1.HomeTeam And game.Attribute("awayteam").Value = game1.AwayTeam
                                              Select New GameUpdateXML With {.hometeam = game.Attribute("hometeam").Value,
                                                                       .homescore = game.Elements("homescore").Value,
                                                                       .awayteam = game.Attribute("awayteam").Value,
                                                                       .awayscore = game.Elements("awayscore").Value,
                                                                       .gametime = game.Elements("gametime").Value}).ToList


                        gameupdate1.HomeScore = queryUpdateGame(0).homescore
                        gameupdate1.AwayScore = queryUpdateGame(0).awayscore
                        gameupdate1.GameTime = queryUpdateGame(0).gametime

                        Dim queryContenders As New List(Of UserChoices)

                        queryContenders = (From contender1 In _DbLoserPool.UserChoicesList
                                           Where contender1.WeekId = thisWeek And contender1.Contender = True
                                           Select contender1).ToList

                        For Each user1 In queryContenders


                            gameupdate1.UserHandles.Add(user1.UserName)

                            Dim teamAvailability As String

                            For Each team1 In teams1

                                If team1.TeamName = queryUpdateGame(0).hometeam Then
                                    If team1.TeamName = "seahawks" Then
                                        teamAvailability = user1.SeaHawks
                                        gameupdate1 = SetHomeTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "packers" Then
                                        teamAvailability = user1.Packers
                                        gameupdate1 = SetHomeTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "falcons" Then
                                        teamAvailability = user1.Falcons
                                        gameupdate1 = SetHomeTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "saints" Then
                                        teamAvailability = user1.Saints
                                        gameupdate1 = SetHomeTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "chargers" Then
                                        teamAvailability = user1.Chargers
                                        gameupdate1 = SetHomeTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "cardinals" Then
                                        teamAvailability = user1.Cardinals
                                        gameupdate1 = SetHomeTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "lions" Then
                                        teamAvailability = user1.Lions
                                        gameupdate1 = SetHomeTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    Else
                                        teamAvailability = user1.Giants
                                        gameupdate1 = SetHomeTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    End If

                                ElseIf team1.TeamName = queryUpdateGame(0).awayteam Then

                                    If team1.TeamName = "seahawks" Then
                                        teamAvailability = user1.SeaHawks
                                        gameupdate1 = SetAwayTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "packers" Then
                                        teamAvailability = user1.Packers
                                        gameupdate1 = SetAwayTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "falcons" Then
                                        teamAvailability = user1.Falcons
                                        gameupdate1 = SetAwayTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "saints" Then
                                        teamAvailability = user1.Saints
                                        gameupdate1 = SetAwayTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "chargers" Then
                                        teamAvailability = user1.Chargers
                                        gameupdate1 = SetAwayTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "cardinals" Then
                                        teamAvailability = user1.Cardinals
                                        gameupdate1 = SetAwayTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    ElseIf team1.TeamName = "lions" Then
                                        teamAvailability = user1.Lions
                                        gameupdate1 = SetAwayTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    Else
                                        teamAvailability = user1.Giants
                                        gameupdate1 = SetAwayTeamAvailabilityState(gameupdate1, queryUpdateGame, team1.TeamName, teamAvailability, user1)
                                    End If
                                End If
                            Next
                        Next
                        GameUpdateCollection.Add(gameupdate1.GameId, gameupdate1)
                    Next



                    For Each game In GameUpdateCollection

                        If game.Key = "game1" Then
                            GameNumber1.Text = game.Key
                            HomeTeam1.Text = GameUpdateCollection(game.Key).HomeTeam
                            AwayTeam1.Text = GameUpdateCollection(game.Key).AwayTeam
                            HomeScore1.Text = GameUpdateCollection(game.Key).HomeScore
                            AwayScore1.Text = GameUpdateCollection(game.Key).AwayScore
                            GameNumber1Status.Text = GameUpdateCollection(game.Key).GameTime

                            If CInt(HomeScore1.Text) > CInt(AwayScore1.Text) Then
                                HomeTeam1.ForeColor = Drawing.Color.DarkGreen
                                HomeScore1.ForeColor = Drawing.Color.DarkGreen
                                AwayTeam1.ForeColor = Drawing.Color.DarkRed
                                AwayScore1.ForeColor = Drawing.Color.DarkRed
                            ElseIf CInt(AwayScore1.Text) > CInt(HomeScore1.Text) Then
                                HomeTeam1.ForeColor = Drawing.Color.DarkRed
                                HomeScore1.ForeColor = Drawing.Color.DarkRed
                                AwayTeam1.ForeColor = Drawing.Color.DarkGreen
                                AwayScore1.ForeColor = Drawing.Color.DarkGreen
                            End If

                        ElseIf game.Key = "game2" Then
                            GameNumber2.Text = game.Key
                            HomeTeam2.Text = GameUpdateCollection(game.Key).HomeTeam
                            AwayTeam2.Text = GameUpdateCollection(game.Key).AwayTeam
                            HomeScore2.Text = GameUpdateCollection(game.Key).HomeScore
                            AwayScore2.Text = GameUpdateCollection(game.Key).AwayScore
                            GameNumber2Status.Text = GameUpdateCollection(game.Key).GameTime

                            If CInt(HomeScore2.Text) > CInt(AwayScore2.Text) Then
                                HomeTeam2.ForeColor = Drawing.Color.DarkGreen
                                HomeScore2.ForeColor = Drawing.Color.DarkGreen
                                AwayTeam2.ForeColor = Drawing.Color.DarkRed
                                AwayScore2.ForeColor = Drawing.Color.DarkRed
                            ElseIf CInt(AwayScore2.Text) > CInt(HomeScore2.Text) Then
                                HomeTeam2.ForeColor = Drawing.Color.DarkRed
                                HomeScore2.ForeColor = Drawing.Color.DarkRed
                                AwayTeam2.ForeColor = Drawing.Color.DarkGreen
                                AwayScore2.ForeColor = Drawing.Color.DarkGreen
                            End If


                        ElseIf game.Key = "game3" Then
                            GameNumber3.Text = game.Key
                            HomeTeam3.Text = GameUpdateCollection(game.Key).HomeTeam
                            AwayTeam3.Text = GameUpdateCollection(game.Key).AwayTeam
                            HomeScore3.Text = GameUpdateCollection(game.Key).HomeScore
                            AwayScore3.Text = GameUpdateCollection(game.Key).AwayScore
                            GameNumber3Status.Text = GameUpdateCollection(game.Key).GameTime

                            If CInt(HomeScore3.Text) > CInt(AwayScore3.Text) Then
                                HomeTeam3.ForeColor = Drawing.Color.DarkGreen
                                HomeScore3.ForeColor = Drawing.Color.DarkGreen
                                AwayTeam3.ForeColor = Drawing.Color.DarkRed
                                AwayScore3.ForeColor = Drawing.Color.DarkRed
                            ElseIf CInt(AwayScore3.Text) > CInt(HomeScore3.Text) Then
                                HomeTeam3.ForeColor = Drawing.Color.DarkRed
                                HomeScore3.ForeColor = Drawing.Color.DarkRed
                                AwayTeam3.ForeColor = Drawing.Color.DarkGreen
                                AwayScore3.ForeColor = Drawing.Color.DarkGreen
                            End If


                        ElseIf game.Key = "game4" Then
                            GameNumber4.Text = game.Key
                            HomeTeam4.Text = GameUpdateCollection(game.Key).HomeTeam
                            AwayTeam4.Text = GameUpdateCollection(game.Key).AwayTeam
                            HomeScore4.Text = GameUpdateCollection(game.Key).HomeScore
                            AwayScore4.Text = GameUpdateCollection(game.Key).AwayScore
                            GameNumber4Status.Text = GameUpdateCollection(game.Key).GameTime

                            If CInt(HomeScore4.Text) > CInt(AwayScore4.Text) Then
                                HomeTeam4.ForeColor = Drawing.Color.DarkGreen
                                HomeScore4.ForeColor = Drawing.Color.DarkGreen
                                AwayTeam4.ForeColor = Drawing.Color.DarkRed
                                AwayScore4.ForeColor = Drawing.Color.DarkRed
                            ElseIf CInt(AwayScore4.Text) > CInt(HomeScore4.Text) Then
                                HomeTeam4.ForeColor = Drawing.Color.DarkRed
                                HomeScore4.ForeColor = Drawing.Color.DarkRed
                                AwayTeam4.ForeColor = Drawing.Color.DarkGreen
                                AwayScore4.ForeColor = Drawing.Color.DarkGreen
                            End If
                        End If
                    Next

                    Dim cnt = 0
                    For Each user1 In GameUpdateCollection("game1").UserHandles

                        Dim userColor As New System.Drawing.Color

                        For Each game In GameUpdateCollection
                            If GameUpdateCollection.Count >= 1 Then
                                If GameUpdateCollection("game1").HomeTeamAvailability(user1) = "L" Then
                                    If HomeScore1.ForeColor = Drawing.Color.DarkGreen Then
                                        userColor = Drawing.Color.DarkRed
                                        Exit For
                                    ElseIf HomeScore1.ForeColor = Drawing.Color.DarkRed Then
                                        userColor = Drawing.Color.DarkGreen
                                        Exit For
                                    End If
                                End If

                                If GameUpdateCollection("game1").AwayTeamAvailability(user1) = "L" Then
                                    If AwayScore1.ForeColor = Drawing.Color.DarkGreen Then
                                        userColor = Drawing.Color.DarkRed
                                        Exit For
                                    ElseIf AwayScore1.ForeColor = Drawing.Color.DarkRed Then
                                        userColor = Drawing.Color.DarkGreen
                                        Exit For
                                    End If
                                End If

                            End If

                            If GameUpdateCollection.Count >= 2 Then
                                If GameUpdateCollection("game2").HomeTeamAvailability(user1) = "L" Then
                                    If HomeScore2.ForeColor = Drawing.Color.DarkGreen Then
                                        userColor = Drawing.Color.DarkRed
                                        Exit For
                                    ElseIf HomeScore2.ForeColor = Drawing.Color.DarkRed Then
                                        userColor = Drawing.Color.DarkGreen
                                        Exit For
                                    End If
                                End If

                                If GameUpdateCollection("game2").AwayTeamAvailability(user1) = "L" Then
                                    If AwayScore2.ForeColor = Drawing.Color.DarkGreen Then
                                        userColor = Drawing.Color.DarkRed
                                        Exit For
                                    ElseIf AwayScore2.ForeColor = Drawing.Color.DarkRed Then
                                        userColor = Drawing.Color.DarkGreen
                                        Exit For
                                    End If
                                End If
                            End If

                            If GameUpdateCollection.Count >= 3 Then
                                If GameUpdateCollection("game3").HomeTeamAvailability(user1) = "L" Then
                                    If HomeScore3.ForeColor = Drawing.Color.DarkGreen Then
                                        userColor = Drawing.Color.DarkRed
                                        Exit For
                                    ElseIf HomeScore3.ForeColor = Drawing.Color.DarkRed Then
                                        userColor = Drawing.Color.DarkGreen
                                        Exit For
                                    End If
                                End If

                                If GameUpdateCollection("game3").AwayTeamAvailability(user1) = "L" Then
                                    If AwayScore3.ForeColor = Drawing.Color.DarkGreen Then
                                        userColor = Drawing.Color.DarkRed
                                        Exit For
                                    ElseIf AwayScore3.ForeColor = Drawing.Color.DarkRed Then
                                        userColor = Drawing.Color.DarkGreen
                                        Exit For
                                    End If
                                End If
                            End If

                            If GameUpdateCollection.Count >= 4 Then
                                If GameUpdateCollection("game4").HomeTeamAvailability(user1) = "L" Then
                                    If HomeScore4.ForeColor = Drawing.Color.DarkGreen Then
                                        userColor = Drawing.Color.DarkRed
                                        Exit For
                                    ElseIf HomeScore4.ForeColor = Drawing.Color.DarkRed Then
                                        userColor = Drawing.Color.DarkGreen
                                        Exit For
                                    End If
                                End If

                                If GameUpdateCollection("game4").AwayTeamAvailability(user1) = "L" Then
                                    If AwayScore4.ForeColor = Drawing.Color.DarkGreen Then
                                        userColor = Drawing.Color.DarkRed
                                        Exit For
                                    ElseIf AwayScore4.ForeColor = Drawing.Color.DarkRed Then
                                        userColor = Drawing.Color.DarkGreen
                                        Exit For
                                    End If

                                End If
                            End If
                        Next


                        Dim dRow As New TableRow

                        If GameUpdateCollection.Count >= 1 Then

                            Dim dCell1 As New TableCell
                            dCell1.Text = GameUpdateCollection("game1").UserHandles(cnt)
                            dCell1.Width = "80"
                            dCell1.ForeColor = userColor
                            dRow.Cells.Add(dCell1)


                            Dim dCell2 As New TableCell
                            dCell2.Text = GameUpdateCollection("game1").HomeTeamAvailability(user1)
                            dCell2.Width = "80"

                            If dCell2.Text = "L" Then
                                dCell2.ForeColor = userColor
                                dCell2.Font.Bold = True
                            End If

                            dRow.Cells.Add(dCell2)

                            Dim dCell3 As New TableCell
                            dCell3.Text = GameUpdateCollection("game1").AwayTeamAvailability(user1)
                            dCell3.Width = "80"

                            If dCell3.Text = "L" Then
                                dCell3.ForeColor = userColor
                                dCell3.Font.Bold = True
                            End If

                            dRow.Cells.Add(dCell3)

                        End If

                        If GameUpdateCollection.Count >= 2 Then
                            Dim dCell4 As New TableCell
                            dCell4.Text = GameUpdateCollection("game2").HomeTeamAvailability(user1)

                            If dCell4.Text = "L" Then
                                dCell4.ForeColor = userColor
                                dCell4.Font.Bold = True
                            End If


                            dCell4.Width = "80"
                            dRow.Cells.Add(dCell4)

                            Dim dCell5 As New TableCell
                            dCell5.Text = GameUpdateCollection("game2").AwayTeamAvailability(user1)
                            dCell5.Width = "80"

                            If dCell5.Text = "L" Then
                                dCell5.ForeColor = userColor
                                dCell5.Font.Bold = True
                            End If

                            dRow.Cells.Add(dCell5)

                        End If

                        If GameUpdateCollection.Count >= 3 Then

                            Dim dCell6 As New TableCell
                            dCell6.Text = GameUpdateCollection("game3").HomeTeamAvailability(user1)
                            dCell6.Width = "80"

                            If dCell6.Text = "L" Then
                                dCell6.ForeColor = userColor
                                dCell6.Font.Bold = True
                            End If

                            dRow.Cells.Add(dCell6)

                            Dim dCell7 As New TableCell
                            dCell7.Text = GameUpdateCollection("game3").AwayTeamAvailability(user1)
                            dCell7.Width = "80"

                            If dCell7.Text = "L" Then
                                dCell7.ForeColor = userColor
                                dCell7.Font.Bold = True
                            End If

                            dRow.Cells.Add(dCell7)

                        End If

                        If GameUpdateCollection.Count >= 4 Then
                            Dim dCell8 As New TableCell
                            dCell8.Text = GameUpdateCollection("game4").HomeTeamAvailability(user1)
                            dCell8.Width = "80"

                            If dCell8.Text = "L" Then
                                dCell8.ForeColor = userColor
                                dCell8.Font.Bold = True
                            End If

                            dRow.Cells.Add(dCell8)

                            Dim dCell9 As New TableCell
                            dCell9.Text = GameUpdateCollection("game4").AwayTeamAvailability(user1)
                            dCell9.Width = "80"

                            If dCell9.Text = "L" Then
                                dCell9.ForeColor = userColor
                                dCell9.Font.Bold = True
                            End If

                            dRow.Cells.Add(dCell9)

                        End If

                        If GameUpdateCollection.Count >= 1 Then
                            TeamsByGameTable1.Rows.Add(dRow)
                        End If

                        cnt = cnt + 1
                    Next

                    If GameUpdateCollection.Count >= 1 Then
                        TeamsByGameTable1.DataBind()
                    End If

                End Using
            End Using

        Catch ex As Exception

        End Try


    End Sub

    Private Shared Function SetHomeTeamAvailabilityState(gameUpdate1 As GameUpdate, queryUpdateGame As List(Of GameUpdateXML), team As String, teamAvailability As Boolean, user1 As UserChoices) As GameUpdate
        If queryUpdateGame(0).hometeam = team Then
            If user1.UserPick = team Then
                gameUpdate1.HomeTeamAvailability.Add(user1.UserName, "L")
            ElseIf teamAvailability = True Then
                gameUpdate1.HomeTeamAvailability.Add(user1.UserName, "A")
            Else
                gameUpdate1.HomeTeamAvailability.Add(user1.UserName, "NA")
            End If
        End If

        Return gameUpdate1
    End Function

    Private Shared Function SetAwayTeamAvailabilityState(gameUpdate1 As GameUpdate, queryUpdateGame As List(Of GameUpdateXML), team As String, teamAvailability As Boolean, user1 As UserChoices) As GameUpdate
        If queryUpdateGame(0).awayteam = team Then
            If user1.UserPick = team Then
                gameUpdate1.AwayTeamAvailability.Add(user1.UserName, "L")
            ElseIf teamAvailability = True Then
                gameUpdate1.AwayTeamAvailability.Add(user1.UserName, "A")
            Else
                gameUpdate1.AwayTeamAvailability.Add(user1.UserName, "NA")
            End If
        End If

        Return gameUpdate1
    End Function


    ' The return type can be changed to IEnumerable, however to support
    ' paging and sorting, the following parameters must be added:
    '     ByVal maximumRows as Integer
    '     ByVal startRowIndex as Integer
    '     ByRef totalRowCount as Integer
    '     ByVal sortByExpression as String

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Response.Redirect("~/LosersPool/Default.aspx")
    End Sub
End Class