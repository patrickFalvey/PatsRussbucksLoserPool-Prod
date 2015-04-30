Public Class UpdateNotReady
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("userId") Is Nothing Then
            Response.Redirect("~/Account/Login.aspx")
        End If
    End Sub

    Protected Sub ReturnToLoserPool1_Click(sender As Object, e As EventArgs) Handles ReturnToLoserPool1.Click
        Response.Redirect("~/LosersPool/Default.aspx")
    End Sub
End Class