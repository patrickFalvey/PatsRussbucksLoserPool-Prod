Imports System.Web
Imports System.Web.UI
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin

Imports LoserPool1.JoinPools.Models
Imports LoserPool1.PlayoffPool.Models
Imports LoserPool1.PlayoffPool


Partial Public Class Login
    Inherits Page

    Private _db As New PoolDbContext



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        RegisterHyperLink.NavigateUrl = "Register"
        ' Enable this once you have account confirmation enabled for password reset functionality
        ' ForgotPasswordHyperLink.NavigateUrl = "Forgot"
        OpenAuthLogin.ReturnUrl = Request.QueryString("ReturnUrl")
        Dim returnUrl = HttpUtility.UrlEncode(Request.QueryString("ReturnUrl"))
        If Not [String].IsNullOrEmpty(returnUrl) Then
            RegisterHyperLink.NavigateUrl += "?ReturnUrl=" & returnUrl
        End If
    End Sub

    Protected Sub LogIn(sender As Object, e As EventArgs)
        If IsValid Then
            ' Validate the user password
            Dim manager = Context.GetOwinContext().GetUserManager(Of ApplicationUserManager)()
            Dim signinManager = Context.GetOwinContext().GetUserManager(Of ApplicationSignInManager)()

            ' This doen't count login failures towards account lockout
            ' To enable password failures to trigger lockout, change to shouldLockout := True
            Dim result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout := False)

            Select Case result
                Case SignInStatus.Success

                    Dim userName As String
                    Dim user1 As String = CStr(Email.Text)

                    Dim Ai As Int16 = InStr(user1, "@")
                    Dim Pi As Int16 = InStr(user1, ".")

                    userName = Left(user1, Ai - 1) + Mid(user1, (Ai + 1), Pi - Ai - 1) + Right(user1, Len(user1) - Pi)
                    Session("userId") = userName

                    Dim Ename1 As String = CStr(Session("userId"))


                    ' uncomment the code below to create the MyPools Table
                    'Dim _dbMyPools As New PoolDbContext
                    'Dim user11 As New MyPool

                    'Try
                    'Using (_dbMyPools)

                    'user11 = _dbMyPools.MyPools.SingleOrDefault(Function(mpl) mpl.EName = Ename1)

                    'Dim _dbPlayoffPool As New PlayoffPoolContext
                    'Dim user2 As New playoffPoolUser

                    'Try

                    'Using (_dbPlayoffPool)
                    'user2.UserId = user11.UserId
                    'user2.UserName = user11.EName

                    '_dbPlayoffPool.playoffPoolUsers.Add(user2)
                    '_dbPlayoffPool.SaveChanges()
                    'End Using

                    'Catch ex As Exception

                    'End Try

                    'End Using

                    'Catch ex As Exception

                    'End Try


                    'Using (_db)


                    'Dim mypool As New MyPool
                    'mypool = _db.MyPools.SingleOrDefault(Function(mp) mp.EName = Ename1)
                    'Session("userId") = mypool.UserId

                    'End Using

                    IdentityHelper.RedirectToReturnUrl(Request.QueryString("ReturnUrl"), Response)
                    Exit Select
                Case SignInStatus.LockedOut
                    Response.Redirect("/Account/Lockout")
                    Exit Select
                Case SignInStatus.RequiresVerification
                    Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                    Request.QueryString("ReturnUrl"),
                                                    RememberMe.Checked),
                                      True)
                    Exit Select
                Case Else
                    FailureText.Text = "Invalid login attempt"
                    ErrorMessage.Visible = True
                    Exit Select
            End Select
        End If
    End Sub
End Class
