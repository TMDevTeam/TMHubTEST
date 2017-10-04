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

    Sub getOrderLines(OrderNo As String, LineNo As String)
        ok = False

        'MsgBox(OrderNo)
        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM sop_lines "
        SQL = SQL & " WHERE orderno = '" & OrderNo & "'"
        If LineNo <> "*" Then SQL = SQL & " AND line = " & LineNo
        SQL = SQL & " ORDER BY orderno"
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

Public Class cOrderLines
    Private _LineNo As String
    Private _OrderQty As String
    Private _COFFQty As String
    Private _DelQty As String
    Private _RemainQty As String
    Private _Code As String
    Private _Desc As String
    Private _Desc1 As String
    Private _Desc2 As String
    Private _Desc3 As String
    Private _DorE As String
    Private _GoodsVal As String
    Private _PackingVal As String
    Private _HaulVal As String
    Private _CommVal As String
    Private _SPVal As String
    Private _Margin As String
    Private _BuyersON As String
    Private _Rev As String
    Private _EffectiveDate As String
    Private _Plots As String
    Private _BPLine As String
    Private _CollectionNo As String
    Private _GoodsDisc1 As String
    Private _GoodsDisc2 As String
    Private _GoodsDisc3 As String


    Public Sub New(LineNo As String, OrderQty As String, COFFQty As String, DelQty As String, RemainQty As String, Code As String, Desc As String, DorE As String, GoodsVal As String,
                   PackingVal As String, HaulVal As String, CommVal As String, SPVal As String, Margin As String, BuyersON As String, Rev As String, Desc1 As String, Desc2 As String,
                   Desc3 As String, EffectiveDate As String, Plots As String, BPLine As String, CollectionNo As String, GoodsDisc1 As String, GoodsDisc2 As String, GoodsDisc3 As String)
        Me.LineNo = LineNo
        Me.OrderQty = OrderQty
        Me.COFFQty = COFFQty
        Me.DelQty = DelQty
        Me.RemainQty = RemainQty
        Me.Code = Code
        Me.Desc = Desc
        Me.Desc1 = Desc1
        Me.Desc2 = Desc2
        Me.Desc3 = Desc3
        Me.DorE = DorE
        Me.GoodsVal = GoodsVal
        Me.PackingVal = PackingVal
        Me.HaulVal = HaulVal
        Me.CommVal = CommVal
        Me.SPVal = SPVal
        Me.Margin = Margin
        Me.BuyersON = BuyersON
        Me.Rev = Rev
        Me.EffectiveDate = EffectiveDate
        Me.Plots = Plots
        Me.BPLine = BPLine
        Me.CollectionNo = CollectionNo
        Me.GoodsDisc1 = GoodsDisc1
        Me.GoodsDisc2 = GoodsDisc2
        Me.GoodsDisc3 = GoodsDisc3

    End Sub

    Public Property GoodsDisc3 As String
        Get
            Return _GoodsDisc3
        End Get
        Set(value As String)
            _GoodsDisc3 = value
        End Set
    End Property
    Public Property GoodsDisc2 As String
        Get
            Return _GoodsDisc2
        End Get
        Set(value As String)
            _GoodsDisc2 = value
        End Set
    End Property
    Public Property GoodsDisc1 As String
        Get
            Return _GoodsDisc1
        End Get
        Set(value As String)
            _GoodsDisc1 = value
        End Set
    End Property
    Public Property CollectionNo As String
        Get
            Return _CollectionNo
        End Get
        Set(value As String)
            _CollectionNo = value
        End Set
    End Property
    Public Property BPLine As String
        Get
            Return _BPLine
        End Get
        Set(value As String)
            _BPLine = value
        End Set
    End Property
    Public Property Plots As String
        Get
            Return _Plots
        End Get
        Set(value As String)
            _Plots = value
        End Set
    End Property
    Public Property EffectiveDate As String
        Get
            Return _EffectiveDate
        End Get
        Set(value As String)
            _EffectiveDate = value
        End Set
    End Property

    Public Property Desc3 As String
        Get
            Return _Desc3
        End Get
        Set(value As String)
            _Desc3 = value
        End Set
    End Property
    Public Property Desc2 As String
        Get
            Return _Desc2
        End Get
        Set(value As String)
            _Desc2 = value
        End Set
    End Property
    Public Property Desc1 As String
        Get
            Return _Desc1
        End Get
        Set(value As String)
            _Desc1 = value
        End Set
    End Property
    Public Property Rev As String
        Get
            Return _Rev
        End Get
        Set(value As String)
            _Rev = value
        End Set
    End Property
    Public Property BuyersON As String
        Get
            Return _BuyersON
        End Get
        Set(value As String)
            _BuyersON = value
        End Set
    End Property
    Public Property Margin As String
        Get
            Return _Margin
        End Get
        Set(value As String)
            _Margin = value
        End Set
    End Property
    Public Property SPVal As String
        Get
            Return _SPVal
        End Get
        Set(value As String)
            _SPVal = value
        End Set
    End Property
    Public Property CommVal As String
        Get
            Return _CommVal
        End Get
        Set(value As String)
            _CommVal = value
        End Set
    End Property
    Public Property HaulVal As String
        Get
            Return _HaulVal
        End Get
        Set(value As String)
            _HaulVal = value
        End Set
    End Property

    Public Property PackingVal As String
        Get
            Return _PackingVal
        End Get
        Set(value As String)
            _PackingVal = value
        End Set
    End Property
    Public Property GoodsVal As String
        Get
            Return _GoodsVal
        End Get
        Set(value As String)
            _GoodsVal = value
        End Set
    End Property
    Public Property DorE As String
        Get
            Return _DorE
        End Get
        Set(value As String)
            _DorE = value
        End Set
    End Property

    Public Property Desc As String
        Get
            Return _Desc
        End Get
        Set(value As String)
            _Desc = value
        End Set
    End Property
    Public Property Code As String
        Get
            Return _Code
        End Get
        Set(value As String)
            _Code = value
        End Set
    End Property
    Public Property RemainQty As String
        Get
            Return _RemainQty
        End Get
        Set(value As String)
            _RemainQty = value
        End Set
    End Property
    Public Property DelQty As String
        Get
            Return _DelQty
        End Get
        Set(value As String)
            _DelQty = value
        End Set
    End Property
    Public Property COFFQty As String
        Get
            Return _COFFQty
        End Get
        Set(value As String)
            _COFFQty = value
        End Set
    End Property
    Public Property OrderQty As String
        Get
            Return _OrderQty
        End Get
        Set(value As String)
            _OrderQty = value
        End Set
    End Property

    Public Property LineNo() As String
        Get
            Return _LineNo
        End Get
        Set(value As String)
            _LineNo = value
        End Set
    End Property
End Class
