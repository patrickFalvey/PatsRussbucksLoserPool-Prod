Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace LosersPool.Models
    Public Class Loser

        <Key>
        Public Property ListId As Int32

        Public Property UserId As String

        Public Property UserName As String

        Public Property WeekId As String

        Public Property WeekIdInt As Int32

        Public Property LosingPick As String

    End Class
End Namespace