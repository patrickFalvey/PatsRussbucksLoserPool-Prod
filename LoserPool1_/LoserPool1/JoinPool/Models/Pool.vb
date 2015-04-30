Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace JoinPools.Models
    Public Class Pool

        <Key>
        Public Property PoolId As String

        <Required>
        Public Property PoolName As String

    End Class

    Public Class PoolXML
        Public Property PoolId As String

        Public Property PoolName As String

    End Class

    Public Class Team

        <Key>
        Public Property TeamId As String

        <Required>
        Public Property TeamName As String

    End Class

    Public Class TeamXML
        Public Property TeamId As String

        Public Property TeamName As String

    End Class
End Namespace
