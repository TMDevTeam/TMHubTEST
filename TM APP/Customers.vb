Imports System.Data.SqlClient
Imports System.Data
Public Class cCustomers
    Public dsCustomer As New DataSet
    Public dsContractor As New DataSet
    Public dsCustomerEmail As New DataSet
    Public dsCustomerAddress As New DataSet
    Public dsCurrentBalance As New DataSet
    Public dsArchitect As New DataSet
    Public ok As Boolean
    Sub getCustomer(TempCustomer As String)

        ok = False

        custBuyingOffice = False
        custOnHold = False
        custNoNewBusiness = False
        custNL = False

        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM RM00101 "
        SQL = SQL & " WHERE INACTIVE = 0"
        If TempCustomer <> "*" Then SQL = SQL & " AND CUSTNMBR = '" & TempCustomer & "'"
        SQL = SQL & " ORDER BY CUSTNAME"
        'MsgBox(SQL)
        Dim connection As New SqlConnection(SQLConnn.TMBConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsCustomer, "Customer")

        If dsCustomer.Tables("Customer").Rows.Count = 0 Then Exit Sub

        If Mid(dsCustomer.Tables("Customer").Rows(0)("govindid"), 1, 1) = "1" Then custBuyingOffice = True
        If dsCustomer.Tables("Customer").Rows(0)("HOLD") = "1" Then custOnHold = True
        If Mid(dsCustomer.Tables("Customer").Rows(0)("govindid"), 10, 1) = "1" Or UCase(Trim(dsCustomer.Tables("Customer").Rows(0)("CUSTCLAS"))) = "NL" Then custNoNewBusiness = True
        If UCase(Trim(dsCustomer.Tables("Customer").Rows(0)("CUSTCLAS"))) = "NL" Then custNL = True


        ok = True

    End Sub

    Function getCustomerEmail(CustomerCode As String, EmailType As Integer) As String

        ok = False
        'MsgBox(CustomerCode)
        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT Email_Recipient FROM RM00106"
        SQL = SQL & " WHERE CUSTNMBR = '" & CustomerCode & "'"
        SQL = SQL & " AND Email_Type = " & EmailType
        'MsgBox(SQL)
        Dim connection As New SqlConnection(SQLConnn.TMBConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsCustomerEmail, "CustomerEmail")

        If dsCustomerEmail.Tables("CustomerEmail").Rows.Count > 0 Then
            Return Trim(dsCustomerEmail.Tables("CustomerEmail").Rows(0)("Email_Recipient"))
        Else
            Return ""
        End If

        ok = True

    End Function

    Sub getCustomerAddress(TempCustomer As String, AddressCode As String, AddressType As String)
        ok = False
        'MsgBox(CustomerCode)
        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM RM00102 "
        SQL = SQL & " WHERE CUSTNMBR <> ''"
        If AddressType = "*" Then
            SQL = SQL & " AND STATE <> 'X'"
        Else
            SQL = SQL & " AND STATE = '" & AddressType & "'"
        End If
        If TempCustomer <> "*" Then SQL = SQL & " AND CUSTNMBR = '" & TempCustomer & "'"
        If AddressCode <> "*" Then SQL = SQL & " AND ADRSCODE = '" & AddressCode & "'"
        SQL = SQL & " ORDER BY STATE, ADRSCODE"

        Dim connection As New SqlConnection(SQLConnn.TMBConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsCustomerAddress, "CustomerAddress")

        If dsCustomerAddress.Tables("CustomerAddress").Rows.Count = 0 Then Exit Sub

        ok = True
    End Sub

    Sub getContractor(TempContractor As String)

        ok = False
        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM RM00101 "
        SQL = SQL & " WHERE INACTIVE = 0"
        SQL = SQL & " AND SUBSTRING(GOVINDID, 7, 1) = '1'"
        If TempContractor <> "*" Then SQL = SQL & " AND CUSTNMBR = '" & TempContractor & "'"
        SQL = SQL & " ORDER BY CUSTNAME"
        'MsgBox(SQL)
        Dim connection As New SqlConnection(SQLConnn.TMBConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsContractor, "Contractor")

        If dsContractor.Tables("Contractor").Rows.Count = 0 Then Exit Sub

        ok = True

    End Sub

    Sub GetArchitect(TempArchitect As String, TempInactive As Integer, TempBranch As String)

        ok = False

        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM architects "
        SQL = SQL & " WHERE Code <> '9999999999'"
        SQL = SQL & " AND Name <> 'Enter NEW architect Practice here....'"
        SQL = SQL & " AND Name <> 'Enter NEW architect practice here....'"
        If Trim(TempBranch) <> "*" Then SQL = SQL & " AND Branch = '" & TempBranch & "'"
        If TempInactive.ToString <> "*" Then SQL = SQL & " AND Inactive = " & TempInactive
        If UCase(Trim(TempArchitect)) <> "*" Then
            If Len(TempArchitect) = 6 Then
                SQL = SQL & " AND Code LIKE '" & TempArchitect & "%'"
                SQL = SQL & " ORDER BY Code DESC"
            Else
                SQL = SQL & " AND Code = '" & TempArchitect & "'"
                SQL = SQL & " ORDER BY Name, City"
            End If
        Else
            SQL = SQL & " ORDER BY Name, City"
        End If
        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsArchitect, "Architect")

        If dsArchitect.Tables("Architect").Rows.Count = 0 Then Exit Sub

        ok = True

    End Sub

    Sub GetArchitectContact(TempArchitect As String, TempInactive As Integer, TempBranch As String, TempContact As String)

        ok = False

        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM architectcontacts "
        SQL = SQL & " WHERE Code <> '9999999999'"
        SQL = SQL & " AND Name <> 'Enter NEW architect contact here....'"
        If Trim(TempBranch) <> "*" Then SQL = SQL & " AND Branch = '" & TempBranch & "'"
        If TempInactive.ToString <> "*" Then SQL = SQL & " AND Inactive = " & TempInactive
        If TempContact.ToString <> "*" Then SQL = SQL & " AND ID = " & TempContact
        If UCase(Trim(TempArchitect)) <> "*" Then
            If Len(TempArchitect) = 6 Then
                SQL = SQL & " AND Code LIKE '" & TempArchitect & "%'"
            Else
                SQL = SQL & " AND Code = '" & TempArchitect & "'"
            End If
        End If
        SQL = SQL & " ORDER BY Name"
        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsArchitect, "ArchitectContact")

        If dsArchitect.Tables("ArchitectContact").Rows.Count = 0 Then Exit Sub

        ok = True

    End Sub



    Sub GetCustomerAddressEmail(TempCustomer As String, TempAddress As String, TempType As String)

        ok = False

        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT * FROM custaddressemail "
        SQL = SQL & " WHERE BRANCH <> ''"
        If TempCustomer <> "*" Then SQL = SQL & " AND CUSTNMBR = '" & UCase(Trim(TempCustomer)) & "'"
        If TempType <> "*" Then SQL = SQL & " AND STATE = '" & TempType & "'"
        If TempAddress <> "*" Then SQL = SQL & " AND ADRSCODE = '" & UCase(Trim(TempAddress)) & "'"
        SQL = SQL & " ORDER BY CUSTNMBR"
        Dim connection As New SqlConnection(SQLConnn.TMBInvConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(SQL, connection)
        connection.Close()

        SQLAdap.Fill(dsCustomerEmail, "AddressEmails")

        If dsCustomerEmail.Tables("AddressEmails").Rows.Count = 0 Then Exit Sub

        ok = True

    End Sub

    Function GetCurrentBal(TempCustomer As String) As Decimal

        Dim SQLConnn As New cADOConnections
        Dim SQL As String = "SELECT CUSTBLNC FROM RM00103"
        SQL = Sql & " WHERE CUSTNMBR = '" & TempCustomer & "'"
        Dim connection As New SqlConnection(SQLConnn.TMBConnectionString)
        connection.Open()
        Dim SQLAdap As New SqlDataAdapter(Sql, connection)
        connection.Close()

        SQLAdap.Fill(dsCurrentBalance, "CurrentBalance")

        If dsCurrentBalance.Tables("CurrentBalance").Rows.Count = 0 Then
            Return 0
        Else
            Return dsCurrentBalance.Tables("CurrentBalance").Rows(0)("CUSTBLNC")
        End If



    End Function

End Class
' ***** Possible Reader Option *****
'Dim SQLConnn As New cADOConnections
'Dim SQL As String = "SELECT CUSTNMBR FROM RM00101 WHERE CUSTNMBR = '" & CustomerCode & "'"
'Using connection As New SqlConnection(SQLConnn.TMBConnectionString)
'    Dim command As New SqlCommand(SQL, connection)
'    Try
'        connection.Open()
'        rCustomer = command.ExecuteReader()
'        'Do While rCustomer.Read()
'        '    MsgBox(rCustomer("CUSTNMBR"))
'        'Loop
'    Catch ex As Exception
'        MsgBox(ex.Message)
'    End Try
'End Using