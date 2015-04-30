Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.ModelBinding

Imports LoserPool1.JoinPools.Models


Public Class MyPools
    Inherits System.Web.UI.Page

    'Private _db As PoolDbContext

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

    Public Function GetMyPools() As MyPoolList

        Dim EName As String = CStr(Session("userId"))

        Dim mpl As New MyPoolList(EName)

        If mpl.queryResult.Pools(0) = "dummy" Then
            myLoserPool.Visible = False
        End If

        If mpl.queryResult.Pools(1) = "dummy" Then
            myPlayoffPool.Visible = False
        End If

        Return mpl

    End Function
End Class

Public Class MyPoolList

    Public Property queryResult As MyPool

    Public Sub New(Ename)
        Me.queryResult = UserPoolList(Me, ename)
    End Sub

    Public Function UserPoolList(mpl1 As MyPoolList, Ename1 As String) As MyPool


        Dim _db As New PoolDbContext

        Try
            Using (_db)

                Dim poolUserQuery

                poolUserQuery = _db.MyPools.SingleOrDefault(Function(mpl) mpl.EName = Ename1)

                If Not (poolUserQuery.Loser Is Nothing) Then
                    poolUserQuery.Pools.Add("LoserPool")
                Else
                    poolUserQuery.Pools.Add("dummy")
                End If

                If Not (poolUserQuery.Playoff Is Nothing) Then
                    poolUserQuery.Pools.Add("PlayoffPool")
                Else
                    poolUserQuery.Pools.Add("dummy")
                End If



                Return poolUserQuery

            End Using
        Catch ex As Exception

            Return mpl1.queryResult

        End Try

        Return mpl1.queryResult

    End Function
End Class

