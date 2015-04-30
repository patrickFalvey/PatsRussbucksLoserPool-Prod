Public Class _Default3
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("userId") Is Nothing Then
            Response.Redirect("~/Account/Login.aspx")
        End If
    End Sub

End Class