Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace JoinPools.Models
    Public Class MyPool


        <Key>
        Public Property UserId As String

        Public Property EName As String

        Public Property Loser As String

        Public Property Playoff As String

        Public Overridable Property Pools As New List(Of String)

        Public Overridable Property LoserPoolOptions As New List(Of String)


    End Class

    Public Class MyPoolXML

        Public Property UserId As String

        Public Property EName As String

        Public Property Loser As String

        Public Property Playoff As String

    End Class
End Namespace