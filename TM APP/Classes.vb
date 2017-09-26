Imports System.Data.SqlClient
Public Class cAddresses
    Private _AddressName As String
    Private _AddressLine1 As String
    Private _AddressLine2 As String
    Private _AddressLine3 As String
    Private _City As String
    Private _PostCode As String
    Private _TelNumber As String
    Private _Email As String

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

Public Class cADOConnections

    Private Const _TMBConnectionString = "Server=192.168.10.2;Database=TMB;User Id=sa;Password=bowie;"
    Private Const _TMBInvConnectionString = "Server=192.168.10.2;Database=TMBInv;User Id=sa;Password=bowie;"
    Private Const _DynamicsConnectionString = "Server=192.168.10.2;Database=DYNAMICS;User Id=sa;Password=bowie;"
    Public TMBConnection As SqlConnection
    Public TMBInvConnection As SqlConnection
    Public DynamicsConnection As SqlConnection
    Public TMBConnectionString As String = "Server=192.168.10.2;Database=TMB;User Id=sa;Password=bowie;"
    Public TMBInvConnectionString As String = "Server=192.168.10.2;Database=TMBInv;User Id=sa;Password=bowie;"


    Function OpenADOConnections(TMB As Boolean, TMBInv As Boolean, Dynamics As Boolean) As Boolean
        TMBConnection = New SqlConnection(cADOConnections._TMBInvConnectionString)
        TMBInvConnection = New SqlConnection(cADOConnections._TMBInvConnectionString)
        DynamicsConnection = New SqlConnection(cADOConnections._TMBInvConnectionString)
        Try
            If TMB Then TMBConnection.Open()
            If TMBInv Then TMBInvConnection.Open()
            If Dynamics Then DynamicsConnection.Open()
            Return True
        Catch ex As Exception
            'MsgBox("Can not open connection ! ")
            Return False
        End Try
    End Function


    Function CloseADOConnections() As Boolean
        Try
            TMBConnection.Close()
            TMBInvConnection.Close()
            DynamicsConnection.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class

