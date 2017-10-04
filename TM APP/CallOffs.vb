Imports System.Data.SqlClient
Imports System.Data
Public Class cCallOffs
    Public ok As Boolean
    Public dsCOFFHeader As New DataSet
    Public dsCOFFSched As New DataSet
    Public dsCOFFWorks As New DataSet

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


    Sub getCOFFByOrderNo(OrderNo As String)
        ok = False
        Dim SQL As String = ""

        Dim SQLConnn As New cADOConnections

        SQL = "SELECT DISTINCT coff_line.coff_no, coff_line.branch, coff_header.date_tm_rev"
        SQL = SQL & " FROM coff_line INNER JOIN coff_header ON coff_line.branch = coff_header.branch AND coff_line.coff_no = coff_header.coff_no"
        SQL = SQL & " WHERE orderno = '" & OrderNo & "'"
        SQL = SQL & " ORDER BY coff_header.date_tm_rev"

        'MessageBox.Show(SQL)
        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsCOFFHeader, "COFFByON")

        If dsCOFFHeader.Tables("COFFByON").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub


    Sub getCOFFDeliverySchedule(COFFNo As String, Branch As String)
        ok = False
        dsCOFFSched.Clear()
        Dim SQL As String = ""

        Dim SQLConnn As New cADOConnections
        SQL = "SELECT coff_line.branch, coff_line.line, coff_line.invoiced, coff_line.qty_invoiced, coff_line.qty_current, coff_line.id_tm_cancel, coff_line.id_cust_cancel, coff_line.d_or_e,
          coff_line.coff_no, coff_line.works, coff_line.date_current, coff_line.xdate_current, coff_line.coff_suffix, coff_line.qty_prom_words, coff_line.del_date, coff_line.first_invoice,
          coff_line.advice, coff_header.date_tm_rev,  coff_header.xdate_tm_rev, coff_header.CustCoff_No, coff_line.qty_current, coff_line.CommentLog, coff_line.orderno"
        SQL = SQL & " FROM coff_line INNER JOIN coff_header ON coff_line.branch = coff_header.branch AND coff_line.coff_no = coff_header.coff_no"
        SQL = SQL & " WHERE coff_line.branch = '" & Branch & "'"
        SQL = SQL & " AND coff_line.coff_no = '" & COFFNo & "'"
        'SQL = SQL & " AND coff_line.line = " & this_order_line
        SQL = SQL & " ORDER BY coff_header.date_tm_rev"

        'MessageBox.Show(SQL)
        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsCOFFSched, "DeliverySchedule")

        If dsCOFFSched.Tables("DeliverySchedule").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub

    Sub getCOFFWorks(Branch As String, CoffNo As String, Works As String, d_or_e As String)
        ok = False
        Dim SQL As String = ""

        Dim SQLConnn As New cADOConnections
        SQL = "SELECT * from coff_works"
        SQL = SQL & " WHERE branch = '" & Branch & "'"
        SQL = SQL & " AND coff_no = " & CoffNo
        SQL = SQL & " AND works = '" & Works & "'"
        SQL = SQL & " AND d_or_e = '" & d_or_e & "'"

        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsCOFFWorks, "COFFWorks")

        If dsCOFFWorks.Tables("COFFWorks").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub

End Class

Public Class cCOFFSchedule
    Private _COFFNo As String
    Private _COFFLineNo As String
    Private _OrderNo As String
    Private _OrderLineNo As String
    Private _RequestDateType As String
    Private _DateRequested As String
    Private _Description As String
    Private _QtyRequested As String
    Private _QtyDelivered As String
    Private _DeliveryDate As String
    Private _InvoiceNo As String
    Private _Status As String

    Public Sub New(COFFNo As String, COFFLineNo As String, OrderNo As String, OrderLineNo As String, RequestDateType As String, DateRequested As String, Description As String,
                   QtyRequested As String, QtyDelivered As String, DeliveryDate As String, InvoiceNo As String, Status As String)
        Me.COFFNo = COFFNo
        Me.COFFLineNo = COFFLineNo
        Me.OrderNo = OrderNo
        Me.OrderLineNo = OrderLineNo
        Me.RequestDateType = RequestDateType
        Me.DateRequested = DateRequested
        Me.Description = Description
        Me.QtyRequested = QtyRequested
        Me.QtyDelivered = QtyDelivered
        Me.DeliveryDate = DeliveryDate
        Me.InvoiceNo = InvoiceNo
        Me._Status = Status
    End Sub

    Public Property Status
        Get
            Return _Status
        End Get
        Set(value)
            _Status = value
        End Set
    End Property

    Public Property COFFNo
        Get
            Return _COFFNo
        End Get
        Set(value)
            _COFFNo = value
        End Set
    End Property

    Public Property COFFLineNo
        Get
            Return _COFFLineNo
        End Get
        Set(value)
            _COFFLineNo = value
        End Set
    End Property

    Public Property OrderNo
        Get
            Return _OrderNo
        End Get
        Set(value)
            _OrderNo = value
        End Set
    End Property

    Public Property OrderLineNo
        Get
            Return _OrderLineNo
        End Get
        Set(value)
            _OrderLineNo = value
        End Set
    End Property

    Public Property RequestDateType
        Get
            Return _RequestDateType
        End Get
        Set(value)
            _RequestDateType = value
        End Set
    End Property

    Public Property DateRequested
        Get
            Return _DateRequested
        End Get
        Set(value)
            _DateRequested = value
        End Set
    End Property

    Public Property Description
        Get
            Return _Description
        End Get
        Set(value)
            _Description = value
        End Set
    End Property

    Public Property QtyRequested
        Get
            Return _QtyRequested
        End Get
        Set(value)
            _QtyRequested = value
        End Set
    End Property

    Public Property QtyDelivered
        Get
            Return _QtyDelivered
        End Get
        Set(value)
            _QtyDelivered = value
        End Set
    End Property

    Public Property DeliveryDate
        Get
            Return _DeliveryDate
        End Get
        Set(value)
            _DeliveryDate = value
        End Set
    End Property

    Public Property InvoiceNo
        Get
            Return _InvoiceNo
        End Get
        Set(value)
            _InvoiceNo = value
        End Set
    End Property
End Class
