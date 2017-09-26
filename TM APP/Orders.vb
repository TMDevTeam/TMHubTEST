Imports System.Data.SqlClient
Imports System.Data
Public Class cOrders
    Public dsOrderHeader As New DataSet
    Public dsOrderLines As New DataSet
    Public ok As Boolean


    Sub getOrderHeader(OrderNo As String)
        ok = False

        'MsgBox(OrderNo)
        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM sop_header WHERE orderno = '" & OrderNo & "'"
        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsOrderHeader, "OrderHeader")

        If dsOrderHeader.Tables("OrderHeader").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub

    Sub getOrderLines(OrderNo As String)
        ok = False

        'MsgBox(OrderNo)
        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM sop_lines WHERE orderno = '" & OrderNo & "'"
        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsOrderLines, "OrderLines")

        If dsOrderLines.Tables("OrderLines").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub

    Sub getOrdersBy(CustomerCode As String, SupplierCode As String, HaulierCode As String,
                    SiteAddress As String, AckAddress As String, InvAddress As String,
                    BuyingOffice As String, WorksAddress As String, HaulDepot As String, OrderStatus As String)
        ok = False

        'MsgBox(OrderNo)
        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT orderno, Supplier FROM sop_header "
        SQL = SQL & " WHERE branch IS NOT NULL"
        If CustomerCode <> "*" Then SQL = SQL & " AND customer = '" & CustomerCode & "'"
        If SupplierCode <> "*" Then SQL = SQL & " AND Supplier = '" & SupplierCode & "'"
        If HaulierCode <> "*" Then SQL = SQL & " AND haulier = '" & HaulierCode & "'"
        'Order Status
        Select Case OrderStatus
            Case "Open"
                SQL = SQL & " AND status1 <> 'D'"
            Case "Closed"
                SQL = SQL & " AND status1 = 'D'"
        End Select
        If SiteAddress <> "*" Then SQL = SQL & " AND DelAddrCode = '" & SiteAddress & "'"
        If AckAddress <> "*" Then SQL = SQL & " AND AckAddrCode = '" & AckAddress & "'"
        If InvAddress <> "*" Then SQL = SQL & " AND InvAddrCode = '" & InvAddress & "'"
        If BuyingOffice <> "*" Then SQL = SQL & " AND (CustomerBuyingOffice = '" & BuyingOffice & "' OR ContractorBuyingOffice = '" & BuyingOffice & "')"
        If WorksAddress <> "*" Then SQL = SQL & " AND works = '" & WorksAddress & "'"
        If HaulDepot <> "*" Then SQL = SQL & " AND depot = '" & HaulDepot & "'"
        SQL = SQL & " ORDER BY orderno"
        'MessageBox.Show(SQL)
        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsOrderHeader, "OrderHeader")

        If dsOrderHeader.Tables("OrderHeader").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub
End Class
