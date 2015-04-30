Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.ModelBinding
Imports System.Data
Imports System.Threading

Imports LoserPool1.LosersPool.Models
Public Class WeeklyLoserPoolUserEntry
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("userId") Is Nothing Then
            Response.Redirect("~/Account/Login.aspx")
        End If

        Dim _dbLoserPool As New LosersPoolContext

        Dim EName = CStr(Session("userId"))
        Dim weekNumber = CStr(Session("weekNumber"))

        Dim mpl = LoserPool1.MyPickList.UserPickList(EName, weekNumber)

        Dim currentDateTime = Date.Now

        Dim minStartTime = currentDateTime.AddYears(1)

        Try

            Dim queryScheduleStartTime = (From game1 In _dbLoserPool.ScheduleEntities
                                          Where game1.WeekId = weekNumber
                                          Select game1).ToList

            ' Find Minimum start time for week
            For Each game1 In queryScheduleStartTime

                Dim gameStartDateTime = DateTime.Parse(CDate(game1.StartDate + " " + game1.StartTime))
                If gameStartDateTime < minStartTime Then
                    minStartTime = gameStartDateTime
                End If

            Next

            ' Is current time < min Start Time

            If currentDateTime > minStartTime Then
                Response.Redirect("~/LosersPool/Default.aspx")
            End If

            If mpl Is Nothing Then
                Button1.Visible = False
                Button2.Visible = True
                loser1.Visible = True
                Exit Try
            Else
                Button1.Visible = True
                Button2.Visible = False
                loser1.Visible = False
                If mpl.SeaHawks = True Or mpl.PossibleUserPicks.Contains("SeaHawks") Then
                    Dim MyPick1 As New MyPick
                    MyPick1.ListId = _dbLoserPool.MyPicks.Count + 1
                    MyPick1.PossibleTeam = "SeaHawks"
                    _dbLoserPool.MyPicks.Add(MyPick1)
                End If

                If mpl.Packers = True Or mpl.PossibleUserPicks.Contains("Packers") Then
                    Dim MyPick1 As New MyPick
                    MyPick1.ListId = _dbLoserPool.MyPicks.Count + 1
                    MyPick1.PossibleTeam = "Packers"
                    _dbLoserPool.MyPicks.Add(MyPick1)
                End If

                If mpl.Falcons = True Or mpl.PossibleUserPicks.Contains("Falcons") Then
                    Dim MyPick1 As New MyPick
                    MyPick1.ListId = _dbLoserPool.MyPicks.Count + 1
                    MyPick1.PossibleTeam = "Falcons"
                    _dbLoserPool.MyPicks.Add(MyPick1)
                End If

                If mpl.Saints = True Or mpl.PossibleUserPicks.Contains("Saints") Then
                    Dim MyPick1 As New MyPick
                    MyPick1.ListId = _dbLoserPool.MyPicks.Count + 1
                    MyPick1.PossibleTeam = "Saints"
                    _dbLoserPool.MyPicks.Add(MyPick1)
                End If

                If mpl.Chargers = True Or mpl.PossibleUserPicks.Contains("Chargers") Then
                    Dim MyPick1 As New MyPick
                    MyPick1.ListId = _dbLoserPool.MyPicks.Count + 1
                    MyPick1.PossibleTeam = "Chargers"
                    _dbLoserPool.MyPicks.Add(MyPick1)
                End If

                If mpl.Cardinals = True Or mpl.PossibleUserPicks.Contains("Cardinals") Then
                    Dim MyPick1 As New MyPick
                    MyPick1.ListId = _dbLoserPool.MyPicks.Count + 1
                    MyPick1.PossibleTeam = "Cardinals"
                    _dbLoserPool.MyPicks.Add(MyPick1)
                End If

                If mpl.Lions = True Or mpl.PossibleUserPicks.Contains("Lions") Then
                    Dim MyPick1 As New MyPick
                    MyPick1.ListId = _dbLoserPool.MyPicks.Count + 1
                    MyPick1.PossibleTeam = "Lions"
                    _dbLoserPool.MyPicks.Add(MyPick1)
                End If

                If mpl.Giants = True Or mpl.PossibleUserPicks.Contains("Giants") Then
                    Dim MyPick1 As New MyPick
                    MyPick1.ListId = _dbLoserPool.MyPicks.Count + 1
                    MyPick1.PossibleTeam = "Giants"
                    _dbLoserPool.MyPicks.Add(MyPick1)
                End If

                _dbLoserPool.SaveChanges()

            End If

        Catch ex As Exception

        End Try

    End Sub

    ' The return type can be changed to IEnumerable, however to support
    ' paging and sorting, the following parameters must be added:
    '     ByVal maximumRows as Integer
    '     ByVal startRowIndex as Integer
    '     ByRef totalRowCount as Integer
    '     ByVal sortByExpression as String
    'Public Function GridView1_GetData() As IQueryable(Of MyPick)

    'End Function

    Protected Sub Button1_Click(sender As Object, e As EventArgs)
        Dim EName = CStr(Session("userId"))
        Dim weekNumber = CStr(Session("weekNumber"))

        Dim lastWeek = "week" + CStr(CInt(Mid(weekNumber, 5)) - 1)


        Dim _DbLoserPool As New LosersPoolContext
        Using (_DbLoserPool)
            Try

                Dim lastWeekChoice As New UserChoices
                lastWeekChoice = _DbLoserPool.UserChoicesList.SingleOrDefault(Function(lWC) lWC.UserID = EName And lWC.WeekId = lastWeek)
                lastWeekChoice = SetContendersPickToFalse(lastWeekChoice)
                lastWeekChoice.WeekId = weekNumber
                lastWeekChoice.UserPick = ""

                Dim userChoice1 = New UserChoices
                userChoice1 = _DbLoserPool.UserChoicesList.SingleOrDefault(Function(uC1) uC1.UserID = EName And uC1.WeekId = weekNumber)
                _DbLoserPool.UserChoicesList.Remove(userChoice1)
                _DbLoserPool.UserChoicesList.Add(lastWeekChoice)

                _DbLoserPool.SaveChanges()

                userChoice1 = New UserChoices
                userChoice1 = _DbLoserPool.UserChoicesList.SingleOrDefault(Function(uC1) uC1.UserID = EName And uC1.WeekId = weekNumber)


                If RadioButtonList1.SelectedIndex > -1 Then
                    Dim userPick1 = RadioButtonList1.SelectedItem.Text

                    If userPick1 = "SeaHawks" Then
                        userChoice1.SeaHawks = False
                        userChoice1.UserPick = "seahawks"
                    End If

                    If userPick1 = "Packers" Then
                        userChoice1.Packers = False
                        userChoice1.UserPick = "packers"
                    End If

                    If userPick1 = "Falcons" Then
                        userChoice1.Falcons = False
                        userChoice1.UserPick = "falcons"
                    End If

                    If userPick1 = "Saints" Then
                        userChoice1.Saints = False
                        userChoice1.UserPick = "saints"
                    End If

                    If userPick1 = "Chargers" Then
                        userChoice1.Chargers = False
                        userChoice1.UserPick = "chargers"
                    End If

                    If userPick1 = "Cardinals" Then
                        userChoice1.Cardinals = False
                        userChoice1.UserPick = "cardinals"
                    End If

                    If userPick1 = "Lions" Then
                        userChoice1.Lions = False
                        userChoice1.UserPick = "lions"
                    End If

                    If userPick1 = "Giants" Then
                        userChoice1.Giants = False
                        userChoice1.UserPick = "giants"
                    End If

                    userChoice1.ConfirmationNumber = userChoice1.ListId

                    _DbLoserPool.SaveChanges()

                    Session("confirmationNumber") = userChoice1.ListId

                End If

            Catch ex As Exception

            End Try
        End Using

        Response.Redirect("~/LosersPool/ContenderConfirmation.aspx")

    End Sub

    Private Shared Function SetContendersPickToFalse(user1 As UserChoices) As UserChoices
        If user1.UserPick = "seahawks" Then
            user1.SeaHawks = False
        ElseIf user1.UserPick = "packers" Then
            user1.Packers = False
        ElseIf user1.UserPick = "falcons" Then
            user1.Falcons = False
        ElseIf user1.UserPick = "saints" Then
            user1.Saints = False
        ElseIf user1.UserPick = "chargers" Then
            user1.Chargers = False
        ElseIf user1.UserPick = "cardinals" Then
            user1.Cardinals = False
        ElseIf user1.UserPick = "lions" Then
            user1.Lions = False
        ElseIf user1.UserPick = "giants" Then
            user1.Giants = False
        End If
        Return user1
    End Function

    Protected Sub Button2_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/LosersPool/Default.aspx")
    End Sub
End Class

Public Class MyPickList

    Public Property MyPicks As UserChoices

    Public Sub New(Ename As String, weekNumber As String)

        Dim user1 As UserChoices = UserPickList(Ename, weekNumber)
        Me.MyPicks = user1

    End Sub

    Public Shared Function UserPickList(EName As String, weekNumber As String) As UserChoices
        Dim _DbLoserPool = New LosersPoolContext

        Try
            Using (_DbLoserPool)

                ' Clear Previous Picks
                Dim users1 = New List(Of MyPick)
                users1 = _DbLoserPool.MyPicks.ToList

                For Each user2 In users1
                    _DbLoserPool.MyPicks.Remove(user2)
                Next

                _DbLoserPool.SaveChanges()

                ' Get New User

                Dim user1 = New UserChoices
                user1 = _DbLoserPool.UserChoicesList.SingleOrDefault(Function(u1) u1.UserID = EName And u1.WeekId = weekNumber)

                If user1 Is Nothing Then
                    Return Nothing
                Else
                    If user1.SeaHawks = True Or user1.UserPick = "seahawks" Then
                        user1.PossibleUserPicks.Add("SeaHawks")
                    End If

                    If user1.Packers = True Or user1.UserPick = "packers" Then
                        user1.PossibleUserPicks.Add("Packers")
                    End If

                    If user1.Falcons = True Or user1.UserPick = "falcons" Then
                        user1.PossibleUserPicks.Add("Falcons")
                    End If

                    If user1.Saints = True Or user1.UserPick = "saints" Then
                        user1.PossibleUserPicks.Add("Saints")
                    End If

                    If user1.Chargers = True Or user1.UserPick = "chargers" Then
                        user1.PossibleUserPicks.Add("Chargers")
                    End If

                    If user1.Cardinals = True Or user1.UserPick = "cardinals" Then
                        user1.PossibleUserPicks.Add("Cardinals")
                    End If

                    If user1.Lions = True Or user1.UserPick = "lions" Then
                        user1.PossibleUserPicks.Add("Lions")
                    End If

                    If user1.Giants = True Or user1.UserPick = "giants" Then
                        user1.PossibleUserPicks.Add("Giants")
                    End If

                    Return user1

                End If

            End Using
        Catch ex As Exception

        End Try
        Return Nothing
    End Function
End Class