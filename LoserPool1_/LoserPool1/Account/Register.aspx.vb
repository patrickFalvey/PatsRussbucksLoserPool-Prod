Imports System
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Owin

Imports LoserPool1.JoinPools.Models

Partial Public Class Register
    Inherits Page

    Private _db As New PoolDbContext

    Protected Sub CreateUser_Click(sender As Object, e As EventArgs)
        Dim userName As String = Email.Text
        Dim manager = Context.GetOwinContext().GetUserManager(Of ApplicationUserManager)()
        Dim signInManager = Context.GetOwinContext().Get(Of ApplicationSignInManager)()
        Dim user = New ApplicationUser() With {.UserName = userName, .Email = userName}
        Dim result = manager.Create(user, Password.Text)
        If result.Succeeded Then
            ' For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            ' Dim code = manager.GenerateEmailConfirmationToken(user.Id)
            ' Dim callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request)
            ' manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=""" & callbackUrl & """>here</a>.")

            signInManager.SignIn(user, isPersistent := False, rememberBrowser := False)

            ' Register user is added to is issued friendly userId and emailname

            Using (_db)
                Dim userName1 As String
                Dim user1 As String = CStr(user.UserName)

                Dim Ai As Int16 = InStr(user1, "@")
                Dim Pi As Int16 = InStr(user1, ".")

                userName1 = Left(user1, Ai - 1) + Mid(user1, (Ai + 1), Pi - Ai - 1) + Right(user1, Len(user1) - Pi)
                Session("userId") = userName1

                Dim mypool = New MyPool

                mypool.UserId = userName1
                mypool.EName = userName1


                _db.MyPools.Add(mypool)
                _db.SaveChanges()
            End Using

            IdentityHelper.RedirectToReturnUrl(Request.QueryString("ReturnUrl"), Response)

        Else
            ErrorMessage.Text = result.Errors.FirstOrDefault()
        End If
    End Sub

    Private Function CreateUserId() As String

        Dim userId1 As String

        If _db.MyPools Is Nothing Then
            userId1 = "user" + Convert.ToString(1)
        Else
            userId1 = "user" + Convert.ToString(_db.MyPools.Count + 1)
        End If

        Return userId1

    End Function

End Class

