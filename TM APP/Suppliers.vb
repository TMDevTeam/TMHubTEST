Imports System.Data.SqlClient
Imports System.Data
Public Class cSuppliers
    Public dsSupplier As New DataSet
    Public dsSupplierAddress As New DataSet
    Public dsAgent As New DataSet
    Public dsHaulier As New DataSet
    Public ok As Boolean

    Sub getSupplier(TempSupplier As String)

        ok = False

        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM PM00200"
        SQL = SQL & " WHERE VNDCLSID <> 'NON TRADE'"
        If TempSupplier <> "*" Then SQL = SQL & " AND VENDORID = '" & UCase(Trim(TempSupplier)) & "'"
        SQL = SQL & " AND VNDCLSID <> 'TRADE H'"
        SQL = SQL & " AND VNDCLSID <> 'TRADE C'"
        SQL = SQL & " AND VNDCLSID <> 'TRADE A'"
        SQL = SQL & " AND VENDSTTS = 1"
        SQL = SQL & " ORDER BY VENDORID"
        Dim connection As New SqlConnection(SQLConnn.TMBConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsSupplier, "Supplier")

        If dsSupplier.Tables("Supplier").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub

    Sub getAgent(TempAgent As String)

        ok = False

        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM PM00200"
        SQL = SQL & " WHERE VNDCLSID = 'TRADE A'"
        If TempAgent <> "*" Then SQL = SQL & " AND VENDORID = '" & UCase(Trim(TempAgent)) & "'"
        SQL = SQL & " AND VENDSTTS = 1"
        SQL = SQL & " ORDER BY VENDORID"
        Dim connection As New SqlConnection(SQLConnn.TMBConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsAgent, "Agent")

        If dsAgent.Tables("Agent").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub

    Sub getHaulier(TempHaulier As String)

        ok = False

        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM PM00200"
        SQL = SQL & " WHERE (VNDCLSID = 'TRADE H' or VNDCLSID = 'TRADE B' or VNDCLSID = 'TRADE C')"
        If TempHaulier <> "*" Then SQL = SQL & " AND VENDORID = '" & UCase(Trim(TempHaulier)) & "'"
        SQL = SQL & " AND VENDSTTS = 1"
        SQL = SQL & " ORDER BY VENDORID"
        Dim connection As New SqlConnection(SQLConnn.TMBConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsHaulier, "Haulier")

        If dsHaulier.Tables("Haulier").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub

    Sub getSupplierAddress(TempSupplier As String, AddressCode As String)
        ok = False
        'MsgBox(CustomerCode)
        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM PM00300 "
        SQL = SQL & " WHERE VENDORID <> ''"
        'If AddressType = "*" Then
        SQL = SQL & " AND STATE <> 'X'"
        'Else
        '    SQL = SQL & " AND STATE = '" & AddressType & "'"
        'End If
        If TempSupplier <> "*" Then SQL = SQL & " AND VENDORID = '" & TempSupplier & "'"
        If AddressCode <> "*" Then SQL = SQL & " AND ADRSCODE = '" & AddressCode & "'"
        SQL = SQL & " ORDER BY ADRSCODE"

        Dim connection As New SqlConnection(SQLConnn.TMBConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsSupplierAddress, "SupplierAddress")

        If dsSupplierAddress.Tables("SupplierAddress").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub
End Class
