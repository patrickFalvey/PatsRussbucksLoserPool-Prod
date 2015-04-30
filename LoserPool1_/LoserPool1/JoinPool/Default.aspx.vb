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


Public Class _Default1
    Inherits System.Web.UI.Page

    Private _db As New PoolDbContext


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("userId") Is Nothing Then
            Response.Redirect("~/Account/Login.aspx")
        End If

        'read PoolXML and save to database if table is empty
        'Dim poolList1 = New PoolList("C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\PoolList.xml")

        'seed Pools table
        Dim poolList2 = New SeedPools

        'read TeamXML and save to database if table is empty
        'Dim teamList1 = New TeamList("C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\TeamList.xml")

        'seed Teams table
        Dim teamList2 = New SeedTeams

        'read MyPoolXML and save to database if table is empty
        'Dim myPool1 = New MyPoolListAlso("C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\MyPoolList.xml")

        PoolNameTextBox.Focus()

    End Sub

    Protected Sub FindPool_Click(sender As Object, e As EventArgs)


        Dim PoolName1 As String = CStr(PoolNameTextBox.Text)

        If Not (PoolName1 = "") Then

            Using (_db)

                Dim validPool As New Pool
                validPool = _db.Pools.SingleOrDefault(Function(p1) p1.PoolName = PoolName1)   '

                If Not (validPool Is Nothing) Then

                    If PoolName1 = "LoserPool" Then
                        PoolNameTextBox.Focus()
                        PoolNameTextBox.Text = ""
                        Response.Redirect("~/JoinPool/JoinLoserPool.aspx")
                    ElseIf PoolName1 = "PlayoffPool" Then
                        PoolNameTextBox.Focus()
                        PoolNameTextBox.Text = ""
                        Response.Redirect("~/JoinPool/JoinPlayoffPool.aspx")

                    End If

                End If
            End Using
        End If

    End Sub
End Class