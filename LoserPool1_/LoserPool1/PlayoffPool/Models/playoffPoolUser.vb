Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace PlayoffPool.Models
    Public Class playoffPoolUser

        <ScaffoldColumn(False)> <Key>
        Public Property UserId As String

        <Required> <StringLength(100)>
        Public Property UserName As String

        Public Property Administrator As Boolean

    End Class
End Namespace
