Imports System.Data.Entity

Namespace JoinPools.Models

    Public Class PoolDbContext
        Inherits DbContext

        Public Property Pools As DbSet(Of Pool)
        Public Property MyPools As DbSet(Of MyPool)
        Public Property Teams As DbSet(Of Team)

        Public Sub New()

            MyBase.New("Pools2-Test")

        End Sub

    End Class
End Namespace

