Imports System.Data.SqlClient
Imports System.Data
Public Class cCallOffs
    Public ok As Boolean
    Public dsCOFFHeader As New DataSet

    Sub getCOFFBy(CustomerCode As String, SupplierCode As String, HaulierCode As String,
                  SiteAddress As String, AckAddress As String, InvAddress As String,
                  BuyingOffice As String, WorksAddress As String, HaulDepot As String, COFFStatus As String)
        ok = False
        Dim SQL As String = ""

        'MsgBox(OrderNo)
        Dim SQLConnn As New cADOConnections
        If SupplierCode <> "*" Then
            SQL = "SELECT coff_header.coff_no FROM coff_header LEFT JOIN coff_works on  coff_header.coff_no = coff_works.coff_no"
            SQL = SQL & " WHERE Supplier = '" & SupplierCode & "'"
        ElseIf haulierCode <> "*" Then
            SQL = "SELECT coff_header.coff_no FROM coff_header LEFT JOIN coff_works on  coff_header.coff_no = coff_works.coff_no"
            SQL = SQL & " WHERE haulier = '" & HaulierCode & "'"
        Else
            SQL = "SELECT coff_no FROM coff_header "
            SQL = SQL & " WHERE branch IS NOT NULL"
        End If

        
        If CustomerCode <> "*" Then SQL = SQL & " AND cust_code = '" & CustomerCode & "'"
        If SiteAddress <> "*" Then SQL = SQL & " AND DelAddrCode = '" & SiteAddress & "'"
        If WorksAddress <> "*" Then SQL = SQL & " AND works = '" & WorksAddress & "'"
        If HaulDepot <> "*" Then SQL = SQL & " AND Depot = '" & HaulDepot & "'"
        'Order Status
        Select Case COFFStatus
            Case "Open"
                SQL = SQL & " AND complete = 0"
            Case "Closed"
                SQL = SQL & " AND complete = -1"
        End Select
        SQL = SQL & " GROUP BY coff_header.coff_no"
        SQL = SQL & " ORDER BY coff_header.coff_no"
        'MessageBox.Show(SQL)
        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsCOFFHeader, "COFFHeader")

        If dsCOFFHeader.Tables("COFFHeader").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub
End Class
