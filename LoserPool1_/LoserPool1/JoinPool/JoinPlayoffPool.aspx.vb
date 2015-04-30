Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Linq

Imports LoserPool1.PlayoffPool
Imports LoserPool1.PlayoffPool.Models

Imports LoserPool1.JoinPools
Imports LoserPool1.JoinPools.Models


Public Class JoinPlayoffPool
    Inherits System.Web.UI.Page

    Private _dbPlayoffPool As New PlayoffPoolContext
    Private _dbMyPool As New PoolDbContext


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("userId") Is Nothing Then
            Response.Redirect("~/Account/Login.aspx")
        End If

        UserNameTextBox.Focus()

    End Sub

    Public Sub JoinLoserPoolBtn_Click(sender As Object, e As EventArgs)

        Dim UserName1 As String = CStr(UserNameTextBox.Text)

        If Not (UserName1 = "") Then

            Using (_dbPlayoffPool)

                Dim notValid As New playoffPoolUser
                notValid = _dbPlayoffPool.playoffPoolUsers.SingleOrDefault(Function(u1) u1.UserName = UserName1)

                If (notValid Is Nothing) Then

                    Dim newUser As New playoffPoolUser
                    newUser.UserId = CreateUserId()

                    newUser.UserName = UserName1
                    newUser.Administrator = False

                    _dbPlayoffPool.playoffPoolUsers.Add(newUser)
                    _dbPlayoffPool.SaveChanges()

                    Try
                        Using (_dbMyPool)

                            Dim Ename1 As String = CStr(Session("userId"))

                            Dim newuser1 As New MyPool
                            newuser1 = _dbMyPool.MyPools.SingleOrDefault(Function(mp) mp.EName = Ename1)
                            newuser1.Playoff = "PlayoffPool"

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
        Else
            UserNameTextBox.Text = ""
            UserNameTextBox.Focus()
            JoinError.Text = "ERROR:  Invalid user name"
            'Response.Redirect("~/LosersPool/JoinLoserPool.aspx")
        End If


    End Sub

    Private Function CreateUserId() As String

        Dim userId1 As String

        If _dbPlayoffPool.playoffPoolUsers Is Nothing Then
            userId1 = "user" + Convert.ToString(1)
        Else
            userId1 = "user" + Convert.ToString(_dbPlayoffPool.playoffPoolUsers.Count + 1)
        End If

        Return userId1

    End Function

End Class