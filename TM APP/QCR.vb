Imports System.Data.SqlClient
Imports System.Data
Public Class cQCR
    Public ok As Boolean
    Public dsQCR As New DataSet

    Sub getQCRBy(CustomerCode As String, SupplierCode As String, HaulierCode As String, SiteAddress As String,
                                AckAddress As String, InvAddress As String, BuyingOffice As String, WorksAdd As String, HaulDepot As String, QCRStatus As String)
        ok = False

        'MsgBox(OrderNo)
        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT QCRNo FROM QCRForms INNER JOIN sop_header ON QCRForms.OrderNo = sop_header.orderno"
        SQL = SQL & " WHERE QCRForms.OrderNo IS NOT NULL"
        If CustomerCode <> "*" Then SQL = SQL & " AND CustCode = '" & CustomerCode & "'"
        If SupplierCode <> "*" Then SQL = SQL & " AND ManufCode = '" & SupplierCode & "'"
        If HaulierCode <> "*" Then SQL = SQL & " AND haulier = '" & HaulierCode & "'"
        'Order Status
        Select Case QCRStatus
            Case "Open"
                SQL = SQL & " AND Stage = '0'"
            Case "Closed"
                SQL = SQL & " AND Stage = -1"
        End Select
        If SiteAddress <> "*" Then SQL = SQL & " AND DelAddrCode = '" & SiteAddress & "'"
        If AckAddress <> "*" Then SQL = SQL & " AND AckAddrCode = '" & AckAddress & "'"
        If InvAddress <> "*" Then SQL = SQL & " AND InvAddrCode = '" & InvAddress & "'"
        If BuyingOffice <> "*" Then SQL = SQL & " AND (CustomerBuyingOffice = '" & BuyingOffice & "' OR ContractorBuyingOffice = '" & BuyingOffice & "')"
        If WorksAdd <> "*" Then SQL = SQL & " AND works = '" & WorksAdd & "'"
        If HaulDepot <> "*" Then SQL = SQL & " AND depot = '" & HaulDepot & "'"
        SQL = SQL & " ORDER BY QCRForms.OrderNo"

        'MessageBox.Show(SQL)
        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsQCR, "QCR")

        If dsQCR.Tables("QCR").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub

End Class
