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



Public Class cADOConnections

    Private Const _TMBConnectionString = "Server=192.168.10.2;Database=TMB;User Id=sa;Password=bowie;"
    Private Const _TMBInvConnectionString = "Server=192.168.10.2;Database=TMBInv;User Id=sa;Password=bowie;"
    Private Const _DynamicsConnectionString = "Server=192.168.10.2;Database=DYNAMICS;User Id=sa;Password=bowie;"
    Public TMBConnection As SqlConnection
    Public TMBInvConnection As SqlConnection
    Public DynamicsConnection As SqlConnection
    Public TMBConnectionString As String = "Server=192.168.10.2;Database=TMB;User Id=sa;Password=bowie;"
    Public TMBInvConnectionString As String = "Server=192.168.10.2;Database=TMBInv;User Id=sa;Password=bowie;"
    Public DynamicsConnectionString As String = "Server=192.168.10.2;Database=DYNAMICS;User Id=sa;Password=bowie;"

End Class

