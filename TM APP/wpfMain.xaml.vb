
Imports System.Data

Class wpfMain
    Public Arrays As New cArrays
    Protected isLoading As Boolean
    Public Sub New()
        isLoading = True
        ' This call is required by the designer.
        InitializeComponent()

        Call Arrays.setupArrays()

        'This setups for customers on initial load as this is most likely first search
        Call SetupCombos("Customers")

        isLoading = False
    End Sub

    Private Sub SetupCombos(TempType As String)
        'Dim SetupCombos As New cArrays

        With Arrays
            cboCustomer.Items.Clear()
            Select Case TempType
                Case "Customers"
                    For i As Integer = 0 To AllCustomers.Count - 1
                        cboCustomer.Items.Add(AllCustomers(i))
                    Next
                Case "Hauliers"
                    For i As Integer = 0 To AllHauliers.Count - 1
                        cboCustomer.Items.Add(AllHauliers(i))
                    Next
                Case "Suppliers"
                    For i As Integer = 0 To AllSuppliers.Count - 1
                        cboCustomer.Items.Add(AllSuppliers(i))
                    Next
            End Select

            cboCustomer.SelectedIndex = -1
        End With

    End Sub

    Private Sub DisplayHeader(TempType As String)

        'Check a customer has been picked from the customer combo
        If cboCustomer.SelectedIndex <> -1 Then
            Dim CustomerCode As String = cboCustomer.Text.Substring(cboCustomer.Text.Length - 10, 10)

            Call clearHeader()

            Select Case TempType
                Case "Customers"
                    Call DisplayCustomer(CustomerCode)
                Case "Hauliers"
                    Call DisplayHaulier(CustomerCode)
                Case "Suppliers"
                    Call DisplaySupplier(CustomerCode)
            End Select
        End If

    End Sub

    Private Sub DisplayHaulier(HaulierCode As String)
        Dim DisplayHeader As New cSuppliers

        DisplayHeader.getHaulier(HaulierCode)
        If DisplayHeader.ok = False Then Exit Sub

        With DisplayHeader.dsHaulier.Tables("Haulier")
            txtAddress1.Text = .Rows(0)("VNDCNTCT")
            txtAddress2.Text = .Rows(0)("ADDRESS1")
            txtAddress3.Text = .Rows(0)("ADDRESS2")
            txtCity.Text = .Rows(0)("CITY")
            txtPostCode.Text = .Rows(0)("ZIPCODE")
            txtTelephone.Text = .Rows(0)("PHNUMBR1")
            'txtEmail.Text = DisplayHeader.getCustomerEmail(SupplierCode, 2)
        End With

        'Get all addresses for this haulier
        DisplayHeader.getSupplierAddress(HaulierCode, "*")
        With DisplayHeader.dsSupplierAddress.Tables("SupplierAddress")
            cboCustomerAddr.Items.Clear()
            For i As Integer = 0 To .Rows.Count - 1
                cboCustomerAddr.Items.Add(.Rows(i)("ADRSCODE"))
            Next
            cboCustomerAddr.SelectedIndex = -1
        End With

        'Display Orders Grid
        Call DisplayOrdersGrid("*", "*", HaulierCode, "*", "*", "*", "*", "*", "*", "Open")

        'Display Projects Grid
        Call DisplayProjectsGrid("*", "*", HaulierCode, "*", "*", "*", "*", "*", "Open")

        'Display Call Off Grid
        Call DisplayCOFFGrid("*", "*", HaulierCode, "*", "*", "*", "*", "*", "*", "Open")

        'Display QCR Grid
        'Call DisplayQCRGrid("*", "*", "*", "Open")

    End Sub
    Private Sub DisplaySupplier(SupplierCode As String)
        Dim DisplayHeader As New cSuppliers

        DisplayHeader.getSupplier(SupplierCode)
        If DisplayHeader.ok = False Then Exit Sub

        With DisplayHeader.dsSupplier.Tables("Supplier")
            txtAddress1.Text = .Rows(0)("VNDCNTCT")
            txtAddress2.Text = .Rows(0)("ADDRESS1")
            txtAddress3.Text = .Rows(0)("ADDRESS2")
            txtCity.Text = .Rows(0)("CITY")
            txtPostCode.Text = .Rows(0)("ZIPCODE")
            txtTelephone.Text = .Rows(0)("PHNUMBR1")
            'txtEmail.Text = DisplayHeader.getCustomerEmail(SupplierCode, 2)
        End With

        'Get all addresses for this customer
        DisplayHeader.getSupplierAddress(SupplierCode, "*")
        With DisplayHeader.dsSupplierAddress.Tables("SupplierAddress")
            cboCustomerAddr.Items.Clear()
            For i As Integer = 0 To .Rows.Count - 1
                If Trim(.Rows(i)("ADRSCODE")) <> "REMIT" Then cboCustomerAddr.Items.Add(.Rows(i)("ADRSCODE"))
            Next
            cboCustomerAddr.SelectedIndex = -1
        End With

        'Display Orders Grid
        Call DisplayOrdersGrid("*", SupplierCode, "*", "*", "*", "*", "*", "*", "*", "Open")

        'Display Projects Grid
        Call DisplayProjectsGrid("*", SupplierCode, "*", "*", "*", "*", "*", "*", "Open")

        'Display Call Off Grid
        Call DisplayCOFFGrid("*", SupplierCode, "*", "*", "*", "*", "*", "*", "*", "Open")

        'Display QCR Grid
        Call DisplayQCRGrid("*", SupplierCode, "*", "*", "*", "*", "*", "*", "*", "Open")

    End Sub

    Private Sub DisplayCustomer(CustomerCode As String)
        Dim DisplayHeader As New cCustomers

        Dim AddrType As String = ""

        DisplayHeader.getCustomer(CustomerCode)
            If DisplayHeader.ok = False Then Exit Sub

            With DisplayHeader.dsCustomer.Tables("Customer")
                txtAddress1.Text = .Rows(0)("CNTCPRSN")
                txtAddress2.Text = .Rows(0)("ADDRESS1")
                txtAddress3.Text = .Rows(0)("ADDRESS2")
                txtCity.Text = .Rows(0)("CITY")
                txtPostCode.Text = .Rows(0)("ZIP")
                txtTelephone.Text = .Rows(0)("PHONE1")
                txtEmail.Text = DisplayHeader.getCustomerEmail(CustomerCode, 2)
            End With

            'Get all addresses for this customer
            DisplayHeader.getCustomerAddress(CustomerCode, "*", "*")
            With DisplayHeader.dsCustomerAddress.Tables("CustomerAddress")
                cboCustomerAddr.Items.Clear()
                For i As Integer = 0 To .Rows.Count - 1
                    If AddrType <> Trim(.Rows(i)("STATE")) Then
                        Select Case Trim(.Rows(i)("STATE"))
                            Case "A"
                                cboCustomerAddr.Items.Add("")
                                cboCustomerAddr.Items.Add("* * * * * * * * * * * * ACK ADDRESSES * * * * * * * * * * * *")
                                AddrType = "A"
                            Case "B"
                                cboCustomerAddr.Items.Add("")
                                cboCustomerAddr.Items.Add("* * * * * * * * * * * * BUYING OFFICES * * * * * * * * * * * *")
                                AddrType = "B"
                            Case "I"
                                cboCustomerAddr.Items.Add("")
                                cboCustomerAddr.Items.Add("* * * * * * * * * * * INVOICE ADDRESSES * * * * * * * * * * *")
                                AddrType = "I"
                            Case "S"
                                cboCustomerAddr.Items.Add("")
                                cboCustomerAddr.Items.Add("* * * * * * * * * * * * SITE ADDRESSES * * * * * * * * * * *")
                                AddrType = "S"
                        End Select
                    End If
                    If Trim(.Rows(i)("ADRSCODE")) <> "STATEMENT" Then cboCustomerAddr.Items.Add(.Rows(i)("ADRSCODE"))
                Next
                cboCustomerAddr.SelectedIndex = -1
            End With

        'Display Orders Grid
        Call DisplayOrdersGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "*", "Open")

        'Display Projects Grid
        Call DisplayProjectsGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "Open")

        'Display Call Off Grid
        Call DisplayCOFFGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "*", "Open")

        'Display QCR Grid
        Call DisplayQCRGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "*", "Open")

        'Display Credit Info
        Call DisplayCreditInfo(CustomerCode)

    End Sub

    Private Sub DisplayOrdersGrid(CustomerCode As String, SupplierCode As String, HaulierCode As String, SiteAddress As String,
                                  AckAddress As String, InvAddress As String, BuyingOffice As String, WorksAdd As String, HaulDepot As String, OrderStatus As String)
        Dim DisplayOrders As New cOrders
        DisplayOrders.getOrdersBy(CustomerCode, SupplierCode, HaulierCode, SiteAddress, AckAddress, InvAddress, BuyingOffice, WorksAdd, HaulDepot, OrderStatus)
        grdOrders.ItemsSource = DisplayOrders.dsOrderHeader.Tables(0).DefaultView
        'txtTEST.Text = DisplayOrders.dsOrderHeader.Tables(0).Rows(3)("orderno").ToString
    End Sub
    Private Sub DisplayProjectsGrid(CustomerCode As String, SupplierCode As String, HaulierCode As String, SiteAddress As String,
                                  AckAddress As String, BuyingOffice As String, WorksAdd As String, HaulDepot As String, ProjectStatus As String)
        Dim DisplayProjects As New cProjects
        DisplayProjects.getProjectsBy(CustomerCode, SupplierCode, HaulierCode, SiteAddress, AckAddress, BuyingOffice, WorksAdd, HaulDepot, ProjectStatus)
        grdProjects.ItemsSource = DisplayProjects.dsProjectHeader.Tables(0).DefaultView
    End Sub
    Private Sub DisplayCOFFGrid(CustomerCode As String, SupplierCode As String, HaulierCode As String, SiteAddress As String,
                                AckAddress As String, InvAddress As String, BuyingOffice As String, WorksAdd As String, HaulDepot As String, COFFStatus As String)
        Dim DisplayCOFF As New cCallOffs
        DisplayCOFF.getCOFFBy(CustomerCode, SupplierCode, HaulierCode, SiteAddress, AckAddress, InvAddress, BuyingOffice, WorksAdd, HaulDepot, COFFStatus)
        grdCOFF.ItemsSource = DisplayCOFF.dsCOFFHeader.Tables(0).DefaultView
    End Sub
    Private Sub DisplayQCRGrid(CustomerCode As String, SupplierCode As String, HaulierCode As String, SiteAddress As String,
                                AckAddress As String, InvAddress As String, BuyingOffice As String, WorksAdd As String, HaulDepot As String, QCRStatus As String)
        Dim DisplayQCR As New cQCR
        DisplayQCR.getQCRBy(CustomerCode, SupplierCode, HaulierCode, SiteAddress, AckAddress, InvAddress, BuyingOffice, WorksAdd, HaulDepot, QCRStatus)
        grdQCR.ItemsSource = DisplayQCR.dsQCR.Tables(0).DefaultView
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub mnuOrders_Click(sender As Object, e As RoutedEventArgs)
        Call ShowOrdersScreen()
    End Sub

    Private Sub ShowOrdersScreen()
        Dim ShowWindow = New wpfOrders()
        ShowWindow.Show()
    End Sub

    Private Sub cboCustomer_DropDownClosed(sender As Object, e As EventArgs) Handles cboCustomer.DropDownClosed
        If isLoading = False Then
            Call DisplayHeader(cboChoice.Text.Trim)
        End If
    End Sub


    Private Sub clearHeader()
        'Main Address
        clearMainAddress()
        'Delivery Address
        clearDelAddress()
    End Sub

    Private Sub clearMainAddress()
        txtAddress1.Clear()
        txtAddress2.Clear()
        txtAddress3.Clear()
        txtCity.Clear()
        txtPostCode.Clear()
        txtTelephone.Clear()
        txtEmail.Clear()
    End Sub

    Private Sub clearDelAddress()
        txtAddress1Addr.Clear()
        txtAddress2Addr.Clear()
        txtAddress3Addr.Clear()
        txtCityAddr.Clear()
        txtPostCodeAddr.Clear()
        txtTelephoneAddr.Clear()
        txtEmailAddr.Clear()
    End Sub

    Private Sub cboCustomerAddr_DropDownClosed(sender As Object, e As EventArgs) Handles cboCustomerAddr.DropDownClosed

        clearDelAddress()
        If cboCustomerAddr.SelectedIndex <> -1 Then
            Dim CustomerCode As String = cboCustomer.Text.Substring(cboCustomer.Text.Length - 10, 10)
            Select Case cboChoice.Text.Trim
                Case "Customers"
                    Call DisplayCustomerAddresses(CustomerCode)
                Case "Hauliers"
                    Call DisplayHaulierAddresses(CustomerCode)
                Case "Suppliers"
                    Call DisplaySupplierAddresses(CustomerCode)
            End Select

        End If
    End Sub

    Private Sub DisplayHaulierAddresses(HaulierCode As String)
        Dim displaySuppAddr As New cSuppliers

        displaySuppAddr.getSupplierAddress(HaulierCode, cboCustomerAddr.Text.Trim)
        If displaySuppAddr.ok = False Then
            'If no records redisplay all grids for customer
            Call DisplayOrdersGrid("*", "*", HaulierCode, "*", "*", "*", "*", "*", "*", "Open")
            Call DisplayCOFFGrid("*", "*", HaulierCode, "*", "*", "*", "*", "*", "*", "Open")
            Call DisplayProjectsGrid("*", "*", HaulierCode, "*", "*", "*", "*", "*", "Open")
            Call DisplayQCRGrid("*", "*", HaulierCode, "*", "*", "*", "*", "*", "*", "Open")
            Exit Sub
        End If

        With displaySuppAddr.dsSupplierAddress.Tables("SupplierAddress")
            txtAddress1Addr.Text = .Rows(0)("VNDCNTCT")
            txtAddress2Addr.Text = .Rows(0)("ADDRESS1")
            txtAddress3Addr.Text = .Rows(0)("ADDRESS2")
            txtCityAddr.Text = .Rows(0)("CITY")
            txtPostCodeAddr.Text = .Rows(0)("ZIPCODE")
            txtTelephoneAddr.Text = .Rows(0)("PHNUMBR1")

            'txtEmail.Text = displayCustAddr.getCustomerEmail(cboCustomer.Text.Substring(cboCustomer.Text.Length - 10, 10), 2)

            'Display Orders Grid
            Call DisplayOrdersGrid("*", "*", HaulierCode, "*", "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "Open")
            Call DisplayCOFFGrid("*", "*", HaulierCode, "*", "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "Open")
            Call DisplayProjectsGrid("*", "*", HaulierCode, "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "Open")
            Call DisplayQCRGrid("*", "*", HaulierCode, "*", "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "Open")
        End With
    End Sub

    Private Sub DisplaySupplierAddresses(SupplierCode As String)
        Dim displaySuppAddr As New cSuppliers

        displaySuppAddr.getSupplierAddress(SupplierCode, cboCustomerAddr.Text.Trim)
        If displaySuppAddr.ok = False Then
            'If no records redisplay all grids for customer
            Call DisplayOrdersGrid("*", SupplierCode, "*", "*", "*", "*", "*", "*", "*", "Open")
            Call DisplayCOFFGrid("*", SupplierCode, "*", "*", "*", "*", "*", "*", "*", "Open")
            Call DisplayProjectsGrid("*", SupplierCode, "*", "*", "*", "*", "*", "*", "Open")
            Call DisplayQCRGrid("*", SupplierCode, "*", "*", "*", "*", "*", "*", "*", "Open")
            Exit Sub
        End If

        With displaySuppAddr.dsSupplierAddress.Tables("SupplierAddress")
            txtAddress1Addr.Text = .Rows(0)("VNDCNTCT")
            txtAddress2Addr.Text = .Rows(0)("ADDRESS1")
            txtAddress3Addr.Text = .Rows(0)("ADDRESS2")
            txtCityAddr.Text = .Rows(0)("CITY")
            txtPostCodeAddr.Text = .Rows(0)("ZIPCODE")
            txtTelephoneAddr.Text = .Rows(0)("PHNUMBR1")

            'txtEmail.Text = displayCustAddr.getCustomerEmail(cboCustomer.Text.Substring(cboCustomer.Text.Length - 10, 10), 2)

            'Display Orders Grid
            Call DisplayOrdersGrid("*", SupplierCode, "*", "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "Open")
            Call DisplayCOFFGrid("*", SupplierCode, "*", "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "Open")
            Call DisplayProjectsGrid("*", SupplierCode, "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "Open")
            Call DisplayQCRGrid("*", SupplierCode, "*", "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "Open")

        End With
    End Sub

    Private Sub DisplayCustomerAddresses(CustomerCode As String)
        Dim displayCustAddr As New cCustomers

        displayCustAddr.getCustomerAddress(CustomerCode, cboCustomerAddr.Text.Trim, "*")
        If displayCustAddr.ok = False Then
            'If no records redisplay all grids for customer
            Call DisplayOrdersGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "*", "Open")
            Call DisplayProjectsGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "Open")
            Call DisplayCOFFGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "*", "Open")
            Call DisplayQCRGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "*", "Open")
            Exit Sub
        End If

        With displayCustAddr.dsCustomerAddress.Tables("CustomerAddress")
            txtAddress1Addr.Text = .Rows(0)("CNTCPRSN")
            txtAddress2Addr.Text = .Rows(0)("ADDRESS1")
            txtAddress3Addr.Text = .Rows(0)("ADDRESS2")
            txtCityAddr.Text = .Rows(0)("CITY")
            txtPostCodeAddr.Text = .Rows(0)("ZIP")
            txtTelephoneAddr.Text = .Rows(0)("PHONE1")
            'txtEmail.Text = displayCustAddr.getCustomerEmail(cboCustomer.Text.Substring(cboCustomer.Text.Length - 10, 10), 2)

            'Display Orders Grid
            Select Case .Rows(0)("STATE").ToString.Trim
                Case "A"
                    Call DisplayOrdersGrid(CustomerCode, "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "*", "*", "Open")
                    Call DisplayProjectsGrid(CustomerCode, "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "*", "Open")
                    Call DisplayCOFFGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "*", "Open")
                    Call DisplayQCRGrid(CustomerCode, "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "*", "*", "Open")
                Case "B"
                    Call DisplayOrdersGrid(CustomerCode, "*", "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "Open")
                    Call DisplayProjectsGrid(CustomerCode, "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "Open")
                    Call DisplayCOFFGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "*", "Open")
                    Call DisplayQCRGrid(CustomerCode, "*", "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "Open")
                Case "I"
                    Call DisplayOrdersGrid(CustomerCode, "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "*", "Open")
                    Call DisplayProjectsGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "Open")
                    Call DisplayCOFFGrid(CustomerCode, "*", "*", "*", "*", "*", "*", "*", "*", "Open")
                    Call DisplayQCRGrid(CustomerCode, "*", "*", "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "*", "Open")
                Case "S"
                    Call DisplayOrdersGrid(CustomerCode, "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "*", "*", "*", "Open")
                    Call DisplayProjectsGrid(CustomerCode, "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "*", "*", "Open")
                    Call DisplayCOFFGrid(CustomerCode, "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "*", "*", "*", "Open")
                    Call DisplayQCRGrid(CustomerCode, "*", "*", cboCustomerAddr.Text.Trim, "*", "*", "*", "*", "*", "Open")
            End Select

        End With
    End Sub

    Private Sub grdOrders_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles grdOrders.MouseDoubleClick
        'Dim test As String
        Dim rowview As DataRowView = grdOrders.SelectedItem

        'Sets global field for order number
        TMOrderNo = rowview.Row(0).ToString
        Call ShowOrdersScreen()
    End Sub


    Private Sub cboChoice_DropDownClosed(sender As Object, e As EventArgs) Handles cboChoice.DropDownClosed
        Call SetupCombos(cboChoice.Text.Trim)
    End Sub

    Private Sub DisplayCreditInfo(CustomerCode As String)
        Dim DisplayHeader As New cCustomers

        'Current Balances & Limits
        txtCreditLimit.Text = 0
        txtCurrentBalance.Text = 0
        txtCurrent.Text = 0
        txtNotDue.Text = 0
        txtOverdue.Text = 0
        txt30Days.Text = 0
        txt60Days.Text = 0

        lblCurrent.Content = Format(DateAdd("m", 0, Now), "MMMM") & vbCrLf & "Current (£)"
        lblNotDue.Content = Format(DateAdd("m", -1, Now), "MMMM") & vbCrLf & "Due (£)"
        lblOverdue.Content = Format(DateAdd("m", -2, Now), "MMMM") & vbCrLf & "Overdue (£)"
        lbl30Days.Content = Format(DateAdd("m", -3, Now), "MMMM") & vbCrLf & "30 Days (£)"
        lbl60Days.Content = Format(DateAdd("m", -4, Now), "MMMM") & vbCrLf & "60+ Days (£)"

        'Only display credit limit for customers
        If cboChoice.Text <> "Customers" Then Exit Sub

        DisplayHeader.getCustomer(CustomerCode)
        If DisplayHeader.ok = False Then Exit Sub

        With DisplayHeader.dsCustomer.Tables("Customer")
            txtCreditLimit.Text = Format(.Rows(0)("CRLMTAMT"), "#,##0")
        End With

        DisplayHeader.GetCurrentBal(CustomerCode)
        With DisplayHeader.dsCurrentBalance.Tables("CurrentBalance")
            txtCurrentBalance.Text = Format(DisplayHeader.GetCurrentBal(CustomerCode), "#,##0")

        End With


        Call DisplayHeader.GetCurrentBals(CustomerCode)
        If DisplayHeader.ok = True Then
            'txtcreditlimit = Format(rsCustomers!CRLMTAMT, "#,##0")
            'txtCurrentBalance = Format(rsCCNotes!CUSTBLNC, "#,##0")
            txtCurrent.Text = Format(DisplayHeader.Current, "#,##0")
            txtNotDue.Text = Format(DisplayHeader.CurrentDue, "#,##0")
            txtOverdue.Text = Format(DisplayHeader.Overdue, "#,##0")
            txt30Days.Text = Format(DisplayHeader.Overdue30, "#,##0")
            txt60Days.Text = Format(DisplayHeader.OverduePlus, "#,##0")
        End If


        'Credit Control Notes
        txtCCNotes.Clear
        Call DisplayHeader.GetCCNotes(CustomerCode)

        With DisplayHeader.dsCCNotes.Tables("CCNotes")
            For i As Integer = 0 To .Rows.Count - 1
                txtCCNotes.Text = txtCCNotes.Text & .Rows(i)("CRUSRID").ToString.Trim & " (" & .Rows(i)("NC_Created_Date").ToString.Trim & ") - " & .Rows(i)("TXTFIELD").ToString.Trim & vbCrLf & vbCrLf

            Next
        End With
    End Sub

End Class
