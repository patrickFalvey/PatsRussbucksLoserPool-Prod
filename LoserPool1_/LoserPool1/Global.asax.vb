Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Optimization
Imports System.Web.Routing
Imports System.Web.Security
Imports System.Web.SessionState
Imports System.Data.Entity

Imports LoserPool1.JoinPools.Models
Imports LoserPool1.PlayoffPool.Models
Imports LoserPool1.PlayoffPool
Imports LoserPool1.LosersPool.Models

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)



        'InitalizePoolDatabase

        'Dim _db As New LosersPoolContext

        'Dim userChoices1 As New UserChoices

        'userChoices1.UserID = "lh4uhotmailcom"
        '_db.UserChoicesList.Add(userChoices1)

        '_db.SaveChanges()

        Dim dummy = "dummy"

        'Dim teams As New List(Of Team)
        'teams = GetTeams()
        'For Each team1 In teams
        '_db.Teams.Add(team1)
        'Next

        '_db.SaveChanges()


        'Database.SetInitializer(New PoolDbInitializer)


    End Sub




End Class