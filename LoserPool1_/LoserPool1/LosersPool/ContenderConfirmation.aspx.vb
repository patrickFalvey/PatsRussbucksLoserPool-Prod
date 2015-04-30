Public Class ContenderConfirmation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Session("userId") Is Nothing Then
            Response.Redirect("~/Account/Login.aspx")
        End If

        Response.Redirect("~/LosersPool/Default.aspx")

    End Sub
End Class