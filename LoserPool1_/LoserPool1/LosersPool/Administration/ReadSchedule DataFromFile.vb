Imports System.Linq
Imports System.Xml.Linq

Imports LoserPool1.LosersPool.Models

Public Class ReadScheduleFile


    Public Sub New(filecontrol As String)

        Dim _dbLoserPool As New LosersPoolContext

        Using (_dbLoserPool)
            If _dbLoserPool.ScheduleEntities.Count >= 1 Then
                Exit Sub
            End If
        End Using

        If filecontrol = "onefile" Then

            Dim pathname = "C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\scheduleDataWeek3.xml"
            ReadScheduleXMLFileAndWriteToScheduleEntities(pathname)

        ElseIf filecontrol = "manyfiles" Then

            Dim schedulefile = XDocument.Load("C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\scheduleDataFileList.xml")

            Dim weekFileList = (From weekFile In schedulefile.Descendants("schedulefiles").Descendants("file")
                                Select New weeklyFileXML With {.weekFilePath = weekFile.Elements("filepath").Value}).ToList

            For Each week In weekFileList
                Dim pathname = week.weekFilePath
                ReadScheduleXMLFileAndWriteToScheduleEntities(pathname)
            Next

        End If



        Dim dummy = "dummy"

    End Sub

    Private Shared Sub ReadScheduleXMLFileAndWriteToScheduleEntities(pathname As String)

        Dim _dbLoserPool = New LosersPoolContext

        Try
            Using (_dbLoserPool)

                Dim myschedule = XDocument.Load(pathname)

                Dim queryWeeK = (From weekElement In myschedule.Descendants("week")
                                Select New weekXML With {.weekNumber = weekElement.Attribute("weekNumber")}).ToList

                Dim queryGame = (From gameElement In myschedule.Descendants("week").Descendants("game")
                Select New gameXML With {.startDate = gameElement.Elements("startDate").Value,
                    .startTime = gameElement.Elements("startTime").Value,
                    .homeTeam = gameElement.Elements("homeTeam").Value,
                    .awayTeam = gameElement.Elements("awayTeam").Value}).ToList

                For gameNum = 1 To queryGame.Count

                    Dim gameNumber = New ScheduleEntity

                    gameNumber.GameId = CreateGameId(_dbLoserPool)
                    gameNumber.WeekId = queryWeeK(0).weekNumber
                    gameNumber.StartDate = queryGame(gameNum - 1).startDate
                    gameNumber.StartTime = queryGame(gameNum - 1).startTime
                    gameNumber.HomeTeam = queryGame(gameNum - 1).homeTeam
                    gameNumber.AwayTeam = queryGame(gameNum - 1).awayTeam

                    _dbLoserPool.ScheduleEntities.Add(gameNumber)
                    _dbLoserPool.SaveChanges()

                Next


            End Using
        Catch ex As Exception

        End Try

        Dim dummy = "dummy"

    End Sub

    Private Shared Function CreateGameId(_dbLoserPool As LosersPoolContext) As String

        Dim gameId1 As String

        If _dbLoserPool.ScheduleEntities Is Nothing Then
            gameId1 = "game" + Convert.ToString(1)
        Else
            gameId1 = "game" + Convert.ToString(_dbLoserPool.ScheduleEntities.Count + 1)
        End If

        Return gameId1

    End Function

    Private Shared Function ClearExistingSchedule(_dbLoserPool As LosersPoolContext) As LosersPoolContext

        Try
            Using (_dbLoserPool)

                Dim querySchedule = (From game In _dbLoserPool.ScheduleEntities).ToList

                For Each game In querySchedule
                    _dbLoserPool.ScheduleEntities.Remove(game)
                Next

                _dbLoserPool.SaveChanges()
                Return _dbLoserPool

            End Using
        Catch ex As Exception

        End Try

        Return Nothing

    End Function

End Class

