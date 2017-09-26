Imports System.Data.SqlClient
Imports System.Data
Public Class cSalesperson
    Public dsSalespeople As New DataSet
    Public dsSpecifier As New DataSet
    Sub getSalesperson(TempSalesperson As String)

        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM RM00301"
        SQL = SQL & " WHERE INACTIVE = 0 "
        If TempSalesperson <> "*" Then SQL = SQL & " AND SLPRSNID = '" & UCase(Trim(TempSalesperson)) & "'"
        SQL = SQL & " ORDER BY SLPRSNID"
        Dim connection As New SqlConnection(SQLConnn.TMBConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsSalespeople, "Salespeople")
    End Sub
    Sub getSpecifier(TempSpecifier As String)

        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM RM00301"
        SQL = SQL & " WHERE INACTIVE = 0 "
        SQL = SQL & " AND PHONE2 = 'Y' "
        If TempSpecifier <> "*" Then SQL = SQL & " AND SLPRSNID = '" & UCase(Trim(TempSpecifier)) & "'"
        SQL = SQL & " ORDER BY SLPRSNID"
        Dim connection As New SqlConnection(SQLConnn.TMBConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsSpecifier, "Specifier")
    End Sub
End Class
