Module General
    Function Llun(Lun As Object)

        If IsDBNull(Lun) Then
            Llun = ""
            GoTo FinishItOffNicely
        End If

        If Trim(Lun) = "" Then
            Llun = ""
        Else
            Llun = Trim(Lun)
        End If

FinishItOffNicely:

        Exit Function
    End Function


End Module
