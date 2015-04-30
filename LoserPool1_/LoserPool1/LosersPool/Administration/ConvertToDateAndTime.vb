Namespace LosersPool.Administration
    Public Class ConvertToDateAndTime

        Public Shared Function ConvertDateTime(cDateTime As Date) As String
            Dim currentDateTime As String = cDateTime.ToString("g")

            currentDateTime = Replace(currentDateTime, "/20", "")
            currentDateTime = Replace(currentDateTime, "/", "")
            Return Replace(currentDateTime, ":", "")

        End Function

        Public Shared Function FilterTime(currentDateTime As String) As String
            Dim sp = InStr(currentDateTime, " ")
            Return Mid(currentDateTime, sp + 1)

        End Function

        Public Shared Function FilterDate(currentDateTime As String) As String
            Dim sp = InStr(currentDateTime, " ")
            Return Left(currentDateTime, sp - 1)

        End Function

        Public Shared Function IntegerTime(currentTime As String) As Int32
            Dim sp = InStr(currentTime, " ")
            Dim timeS = Left(currentTime, sp - 1)
            Dim timeM = Mid(currentTime, sp + 1)
            Dim timeI = CInt(timeS)

            If timeM = "PM" Then
                timeI = timeI + 1200
                If timeI >= 2400 Then
                    timeI = timeI - 2400
                End If
            End If

            Return timeI

        End Function

    End Class
End Namespace

