
Imports System
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Owin

Imports LoserPool1.JoinPools
Imports LoserPool1.JoinPools.Models


Public Class JoinPool
    Inherits System.Web.UI.Page

    Private _db As New PoolDbContext

    Public PoolName1 As String
    Public UserEmailAddress As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PoolNameTextBox.Focus()

    End Sub

    Protected Sub PoolNameBtn_Click(sender As Object, e As EventArgs)

        Dim Ename = CStr(Session("userId"))

        PoolName1 = PoolNameTextBox.Text

        If Not (PoolName1 = "") Then

            Try
                Using (_db)
                    Dim valid = _db.Pools.SingleOrDefault(Function(p1) p1.PoolName = PoolName1)

                    Dim validPoolForUser = _db.MyPools.SingleOrDefault(Function(vPU) vPU.EName = Ename And Not (vPU.Loser = valid.PoolName))

                    If Not (valid Is Nothing) And Not (validPoolForUser Is Nothing) Then

                        If PoolName1 = "LoserPool" Then
                            PoolNameTextBox.Focus()
                            PoolNameTextBox.Text = ""
                            Response.Redirect("~/JoinPool/JoinLoserPool.aspx")
                        ElseIf PoolName1 = "PlayoffPool" Then
                            PoolNameTextBox.Focus()
                            PoolNameTextBox.Text = ""
                            Response.Redirect("~/JoinPool/JoinPlayoffPool.aspx")

                        End If
                    Else
                        PoolNameTextBox.Text = ""
                        PoolNameTextBox.Focus()
                        JoinError.Text = "ERROR: User has already joined pool"

                    End If
                End Using
            Catch ex As Exception

            End Try

        End If

    End Sub
End Class