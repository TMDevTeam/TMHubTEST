Imports System.Data.SqlClient
Imports System.Data
Public Class cProjects
    Public dsProjectHeader As New DataSet
    Public ok As Boolean
    Sub getProjectsBy(CustomerCode As String, SupplierCode As String, HaulierCode As String, DelAddress As String,
                      AckAddress As String, BuyingOffice As String, WorksAdd As String, HaulDepot As String, ProjectStatus As String)
        ok = False
        Dim SQL As String = ""

        Dim SQLConnn As New cADOConnections
        If SupplierCode <> "*" Or HaulierCode <> "*" Then
            SQL = "SELECT QuoteHeaders.QuoteNo FROM QuoteHeaders LEFT JOIN QuoteLines on QuoteHeaders.QuoteNo = QuoteLines.QuoteNo  AND  QuoteHeaders.Version = QuoteLines.Version"
        Else
            SQL = "SELECT QuoteHeaders.QuoteNo FROM QuoteHeaders "
        End If

        SQL = SQL & " WHERE substring(QuoteHeaders.QuoteNo,1,1) = 'Q'"
        If CustomerCode <> "*" Then SQL = SQL & " AND CustomerCode = '" & CustomerCode & "'"
        If SupplierCode <> "*" Then SQL = SQL & " AND SupplierCode = '" & SupplierCode & "'"
        If HaulierCode <> "*" Then SQL = SQL & " AND HaulierCode = '" & HaulierCode & "'"
        If DelAddress <> "*" Then SQL = SQL & " AND SiteAddressCode = '" & DelAddress & "'"
        If AckAddress <> "*" Then SQL = SQL & " AND QuoteAddressCode = '" & AckAddress & "'"
        If BuyingOffice <> "*" Then SQL = SQL & " AND (CustomerSpecOffice = '" & BuyingOffice & "' OR MainContractorSpecOffice = '" & BuyingOffice & "')"
        If WorksAdd <> "*" Then SQL = SQL & " AND WorksCode = '" & WorksAdd & "'"
        If HaulDepot <> "*" Then SQL = SQL & " AND DepotCode = '" & HaulDepot & "'"
        Select Case ProjectStatus
            Case "Open"
                SQL = SQL & " AND QuoteHeaders.Status = 0"
            Case "Closed"
                SQL = SQL & " AND QuoteHeaders.Status <> 0"
        End Select

        SQL = SQL & " GROUP BY QuoteHeaders.QuoteNo"
        SQL = SQL & " ORDER BY QuoteHeaders.QuoteNo"

        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        'MessageBox.Show(SQL)

        SQLAdap.Fill(dsProjectHeader, "ProjectHeader")

        If dsProjectHeader.Tables("ProjectHeader").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub
End Class
