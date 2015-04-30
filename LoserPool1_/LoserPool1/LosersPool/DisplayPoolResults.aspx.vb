Imports System
Imports System.Linq
Imports System.Xml.Linq
Imports System.Globalization
Imports System.Threading

Imports LoserPool1.JoinPools.Models
Imports LoserPool1.LosersPool.Models


Public Class DisplayPoolResults
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("userId") Is Nothing Then
            Response.Redirect("~/Account/Login.aspx")
        End If

    End Sub

    ' The return type can be changed to IEnumerable, however to support
    ' paging and sorting, the following parameters must be added:
    '     ByVal maximumRows as Integer
    '     ByVal startRowIndex as Integer
    '     ByRef totalRowCount as Integer
    '     ByVal sortByExpression as String

    Public Function ContendersTable_GetData() As List(Of UserResult)

        Dim _dbLoserPool As New LosersPoolContext

        Try
            Using (_dbLoserPool)
                Dim UserResults = _dbLoserPool.UserResultList.ToList

                For Each userResult1 In UserResults
                    _dbLoserPool.UserResultList.Remove(userResult1)
                Next

                _dbLoserPool.SaveChanges()

                Dim weekNumber = CStr(Session("weekNumber"))
                Dim weekId = "week" + CStr(CInt(Mid(weekNumber, 5)) - 1)

                If CStr(Session("optionState")) = "SeasonEnd" Then
                    weekId = "week" + CStr(CInt(Mid(weekNumber, 5)))
                End If

                Dim WeeklyUserChoices As New List(Of UserChoices)

                WeeklyUserChoices = (From user1 In _dbLoserPool.UserChoicesList
                                        Where user1.Contender = True And user1.WeekId = weekId
                                        Select user1).ToList()

                For Each user1 In WeeklyUserChoices

                    Dim user2 = New UserResult
                    user2.ListId = user1.ListId
                    user2.UserID = user1.UserID
                    user2.UserName = user1.UserName
                    user2.WeekId = weekId

                    If user1.UserPick = "seahawks" Then
                        user2.SeaHawks = False
                    Else
                        user2.SeaHawks = user1.SeaHawks
                    End If

                    If user1.UserPick = "packers" Then
                        user2.Packers = False
                    Else
                        user2.Packers = user1.Packers

                    End If

                    If user1.UserPick = "falcons" Then
                        user2.Falcons = False
                    Else
                        user2.Falcons = user1.Falcons

                    End If

                    If user1.UserPick = "saints" Then
                        user2.Saints = False
                    Else
                        user2.Saints = user1.Saints

                    End If

                    If user1.UserPick = "chargers" Then
                        user2.Chargers = False
                    Else
                        user2.Chargers = user1.Chargers

                    End If

                    If user1.UserPick = "cardinals" Then
                        user2.Cardinals = False
                    Else
                        user2.Cardinals = user1.Cardinals

                    End If

                    If user1.UserPick = "lions" Then
                        user2.Lions = False
                    Else
                        user2.Lions = user1.Lions

                    End If

                    If user1.UserPick = "giants" Then
                        user2.Giants = False
                    Else
                        user2.Giants = user1.Giants

                    End If


                    _dbLoserPool.UserResultList.Add(user2)
                    _dbLoserPool.SaveChanges()
                Next


                Dim WeeklyUserResults = (From user1 In _dbLoserPool.UserResultList
                                        Where user1.WeekId = weekId
                                        Select user1).ToList()

                ViewState("weekNumber") = weekId
                Return WeeklyUserResults
            End Using
        Catch ex As Exception

        End Try
        Return Nothing
    End Function

    ' The return type can be changed to IEnumerable, however to support
    ' paging and sorting, the following parameters must be added:
    '     ByVal maximumRows as Integer
    '     ByVal startRowIndex as Integer
    '     ByRef totalRowCount as Integer
    '     ByVal sortByExpression as String
    Public Function LosersTable_GetData() As List(Of Loser)
        Dim _dbLoserPool As New LosersPoolContext
        Try
            Using (_dbLoserPool)
                Dim losers1 = _dbLoserPool.LoserList.ToList
                Return losers1
            End Using
        Catch ex As Exception

        End Try
        Return Nothing
    End Function

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("~/LosersPool/Default.aspx")
    End Sub
End Class