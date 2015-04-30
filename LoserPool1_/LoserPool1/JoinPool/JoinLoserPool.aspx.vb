Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Linq

Imports LoserPool1.LosersPool.Models

Imports LoserPool1.JoinPools
Imports LoserPool1.JoinPools.Models

Public Class JoinLoserPool
    Inherits System.Web.UI.Page

    Private _dbLoserPool As New LosersPoolContext
    Private _dbMyPool As New PoolDbContext

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("userId") Is Nothing Then
            Response.Redirect("~/Account/Login.aspx")
        End If

        UserNameTextBox.Focus()
        'JoinError.Text = ""
    End Sub

    Public Sub JoinLoserPoolBtn_Click(sender As Object, e As EventArgs)

        Dim UserName1 As String = CStr(UserNameTextBox.Text)
        Dim EName As String = CStr(Session("userId"))

        If Not (UserName1 = "") Then

            Try
                Using (_dbLoserPool)

                    Dim notValid As New User
                    notValid = _dbLoserPool.Users.SingleOrDefault(Function(u1) u1.UserName = UserName1)

                    If (notValid Is Nothing) Then

                        Dim newUser As New User
                        newUser.UserId = EName
                        newUser.UserName = UserName1
                        newUser.Administrator = False

                        Dim newUser2 As New UserChoices
                        newUser2.UserID = EName
                        newUser2.UserName = UserName1
                        newUser2.WeekId = "week0"
                        newUser2.Contender = True
                        newUser2.SeaHawks = True
                        newUser2.Packers = True
                        newUser2.Falcons = True
                        newUser2.Saints = True
                        newUser2.Chargers = True
                        newUser2.Cardinals = True
                        newUser2.Lions = True
                        newUser2.Giants = True

                        If _dbLoserPool.UserChoicesList.Count = 0 Then
                            newUser2.ListId = 1
                        Else
                            newUser2.ListId = _dbLoserPool.UserChoicesList.Count + 1
                        End If

                        _dbLoserPool.Users.Add(newUser)
                        _dbLoserPool.UserChoicesList.Add(newUser2)

                        Dim newUser3 As New UserChoices
                        newUser3.UserID = EName
                        newUser3.UserName = UserName1
                        newUser3.WeekId = "week1"
                        newUser3.Contender = True
                        newUser3.SeaHawks = True
                        newUser3.Packers = True
                        newUser3.Falcons = True
                        newUser3.Saints = True
                        newUser3.Chargers = True
                        newUser3.Cardinals = True
                        newUser3.Lions = True
                        newUser3.Giants = True

                        _dbLoserPool.UserChoicesList.Add(newUser3)
                        _dbLoserPool.SaveChanges()

                        Try
                            Using (_dbMyPool)

                                Dim Ename1 As String = CStr(Session("userId"))

                                Dim newuser1 As New MyPool
                                newuser1 = _dbMyPool.MyPools.SingleOrDefault(Function(mp) mp.EName = Ename1)
                                newuser1.Loser = "LoserPool"

                                _dbMyPool.SaveChanges()

                            End Using

                        Catch ex As Exception

                        End Try

                        Response.Redirect("~/Default.aspx")
                    Else
                        UserNameTextBox.Text = ""
                        UserNameTextBox.Focus()
                        JoinError.Text = "ERROR: User name is already in use"
                        'Response.Redirect("~/LosersPool/JoinLoserPool.aspx")
                    End If

                End Using

            Catch ex As Exception

            End Try

        Else
            UserNameTextBox.Text = ""
            UserNameTextBox.Focus()
            JoinError.Text = "ERROR:  Invalid user name"
            'Response.Redirect("~/LosersPool/JoinLoserPool.aspx")
        End If


    End Sub

    Private Function CreateUserId() As String

        Dim userId1 As String

        If _dbLoserPool.Users Is Nothing Then
            userId1 = "user" + Convert.ToString(1)
        Else
            userId1 = "user" + Convert.ToString(_dbLoserPool.Users.Count + 1)
        End If

        Return userId1

    End Function


End Class