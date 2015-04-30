Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure

Namespace LosersPool.Models
    Public Class LosersPoolContext
        Inherits DbContext

        Public Property Users As DbSet(Of User)
        Public Property ScheduleEntities As DbSet(Of ScheduleEntity)
        Public Property ScheduleTimePeriods As DbSet(Of ScheduleTimePeriod)
        Public Property UserChoicesList As DbSet(Of UserChoices)
        Public Property LoserList As DbSet(Of Loser)
        Public Property MyPicks As DbSet(Of MyPick)
        Public Property UserResultList As DbSet(Of UserResult)

        Public Sub New()
            MyBase.New("LosersPool2-Test")
        End Sub


    End Class
End Namespace
