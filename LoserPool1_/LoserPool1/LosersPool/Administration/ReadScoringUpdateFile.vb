Imports System.Linq
Imports System.Xml.Linq

Imports LoserPool1.LosersPool.Models

Public Class ReadScoringUpdateFile

    Private _dbLoserPool As New LosersPoolContext

    Public Sub New()
        Try
            Using (_dbLoserPool)


                Dim myUpdate = XDocument.Load("C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\scoringUpdateWeek1Update1.xml")

                Dim queryTime = (From score In myUpdate.Descendants("scores")
                                 Select New ScoreUpdateXML With {.filetime = score.Attribute("filetime"),
                                                                 .filedate = score.Attribute("filedate")}).ToList

                Dim queryGame = (From game In myUpdate.Descendants("scores").Descendants("game")
                                 Select New GameUpdateXML With {.hometeam = game.Attribute("hometeam").Value,
                                                               .homescore = game.Elements("homescore").Value,
                                                               .awayteam = game.Attribute("awayteam").Value,
                                                               .awayscore = game.Elements("awayscore").Value,
                                                               .gametime = game.Elements("gametime").Value}).ToList

                Dim dummy = "dummy"
                _dbLoserPool.SaveChanges()

            End Using
        Catch ex As Exception

        End Try
    End Sub

End Class
