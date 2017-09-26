Imports System.Data
Class wpfOrders
    Dim isLoading As Boolean
    Private Sub lblShowOrderLines_Click(sender As Object, e As RoutedEventArgs) Handles btnOrderFirst.Click
        Dim ShowWindow = New wpfOrderLine()
        ShowWindow.Show()
    End Sub

    Private Sub txtOrderNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtOrderNo.KeyDown
        If e.Key = Key.Enter Then
            txtOrderNo.Text = UCase(txtOrderNo.Text)
            Call DisplayOrder(txtOrderNo.Text)
        End If
    End Sub
    Private Sub DisplayOrder(OrderNo As String)
        Call DisplayOrderHeader(OrderNo)
        Call DisplayOrderLines(OrderNo)
    End Sub
    Sub DisplayOrderLines(OrderNo As String)
        Dim DisplayOrderLines As New cOrders
        DisplayOrderLines.getOrderLines(OrderNo)
        'grdOrderLines.ItemsSource = DisplayOrderLines.dsOrderLines.Tables(0).DefaultView

        Dim OrderLines As New List(Of cOrderLines)

        With DisplayOrderLines.dsOrderLines.Tables(0)
            For i = 0 To .Rows.Count - 1

                'Analysis Code
                Dim AnalysisCode As String = .Rows(i)("analysiscode").ToString.Trim
                If AnalysisCode = "B" Then
                    AnalysisCode = .Rows(i)("prodcode").ToString.Trim
                End If

                'Line Number
                Dim LineNo As String
                If .Rows(i)("CommentType").ToString.Trim <> "P" Then
                    LineNo = Format(.Rows(i)("line"), "0000") + "/" + .Rows(i)("comment_no").ToString.Trim
                    'Loop list of comment types
                    For j = 0 To CommentTypes.Count - 1
                        If CommentTypes(j).Substring(0, 1) = Trim(.Rows(i)("CommentType").ToString) Then
                            AnalysisCode = CommentTypes(j).Substring(InStr(CommentTypes(j), "-") + 1)
                            Exit For
                        End If
                    Next
                Else
                    LineNo = Format(.Rows(i)("line"), "0000")
                End If

                'Description
                Dim Desc As String = .Rows(i)("description1").ToString.Trim
                If .Rows(i)("description2").ToString.Trim <> "" Then
                    Desc += Environment.NewLine + .Rows(i)("description2").ToString.Trim
                End If
                If .Rows(i)("description3").ToString.Trim <> "" Then
                    Desc += Environment.NewLine + .Rows(i)("description3").ToString.Trim
                End If

                'Check Currency for BP, Haulage & SP and display correct figures
                Dim GoodsVal As String = 0
                Dim BandVal As String = 0
                Dim HaulVal As String = 0
                Dim SPVal As String = 0

                'BP & Banding Prices
                Select Case txtSupplierCurr.Text.Trim
                    Case "GBP"
                        GoodsVal = Format(.Rows(i)("GoodsGBP"), "#0.00")
                        BandVal = Format(.Rows(i)("BandGBP"), "#0.00")
                    Case Else
                        GoodsVal = Format(.Rows(i)("goodsordercurncyid"), "#0.00")
                        BandVal = Format(.Rows(i)("bandordercurncyid"), "#0.00")
                End Select
                GoodsVal += Environment.NewLine + .Rows(i)("bpper").ToString.Trim

                'Haulage Price
                Select Case txtHaulierCurr.Text.Trim
                    Case "GBP"
                        HaulVal = Format(.Rows(i)("HaulGBP"), "#0.00")
                    Case Else
                        HaulVal = Format(.Rows(i)("haulordercurncyid"), "#0.00")
                End Select
                HaulVal += Environment.NewLine + .Rows(i)("haulper").ToString.Trim

                'SP Price
                Select Case txtCustomerCurr.Text.Trim
                    Case "GBP"
                        SPVal = Format(.Rows(i)("SPGBP"), "#0.00")
                    Case Else
                        SPVal = Format(.Rows(i)("spordercurncyid"), "#0.00")
                End Select
                SPVal += Environment.NewLine + .Rows(i)("spper").ToString.Trim

                Dim AgentVal As String = Format(.Rows(i)("AgentGBP"), "#0.00")
                AgentVal += Environment.NewLine + .Rows(i)("agentper").ToString.Trim

                Dim Margin As String = Format(.Rows(i)("margin"), "#0.00")

                'Check if this is a comment line or a product line then add to class
                If .Rows(i)("CommentType").ToString.Trim = "P" Then
                    'Product Line
                    OrderLines.Add(New cOrderLines(LineNo, Format(.Rows(i)("qty"), "#"), "", "", "", AnalysisCode, Desc, .Rows(i)("d_or_e"),
                                               GoodsVal, BandVal, HaulVal, AgentVal, SPVal, Margin, "BON", .Rows(i)("rev"), .Rows(i)("description1").ToString.Trim,
                                               .Rows(i)("description2").ToString.Trim, .Rows(i)("description3").ToString.Trim))
                Else
                    'Comment Line
                    OrderLines.Add(New cOrderLines(LineNo, "", "", "", "", AnalysisCode, Desc, "", "", "", "", "", "", "", "", "", "", "", ""))
                End If

            Next
        End With

        'Display order lines from the class
        grdOrderLines.ItemsSource = OrderLines
    End Sub
    Sub DisplayOrderHeader(OrderNo As String)
        Dim DisplayOrders As New cOrders
        Dim i As Integer

        'Clear the order header
        ClearOrderHeader()

        DisplayOrders.getOrderHeader(OrderNo)
        If DisplayOrders.ok = False Then
            MsgBox("Order Doesn't Exist")
            Exit Sub
        End If


        With DisplayOrders.dsOrderHeader.Tables(0)
            txtOrderDate.Text = Format(.Rows(0)("date_order"), "dd/MM/yyyy")
            txtBPOrder.Text = Llun(.Rows(0)("BPOrder"))

            'Salesperson 1
            For i = 0 To cboSalesperson1.Items.Count - 1
                cboSalesperson1.SelectedIndex = i
                If cboSalesperson1.Text.Substring(0, 3) = Trim(.Rows(0)("Salesperson1").ToString) Then
                    Exit For
                End If
            Next
            txtSalesSplit1.Text = .Rows(0)("salesSplit")
            'Salesperson 2
            For i = 0 To cboSalesperson2.Items.Count - 1
                cboSalesperson2.SelectedIndex = i
                If cboSalesperson2.Text.Substring(0, 3) = Trim(.Rows(0)("Salesperson2").ToString) Then
                    Exit For
                End If
            Next
            txtSalesSplit2.Text = .Rows(0)("salesSplit2")
            'Salesperson 3
            For i = 0 To cboSalesperson3.Items.Count - 1
                cboSalesperson3.SelectedIndex = i
                If cboSalesperson3.Text.Substring(0, 3) = Trim(.Rows(0)("Salesperson3").ToString) Then
                    Exit For
                End If
            Next
            txtSalesSplit3.Text = ""

            'Specifier
            For i = 0 To cboSpecifier.Items.Count - 1
                cboSpecifier.SelectedIndex = i
                If cboSpecifier.Text.Substring(0, 3) = Trim(.Rows(0)("tmSpecifierID").ToString) Then
                    Exit For
                End If
                cboSpecifier.SelectedIndex = -1
            Next

            'Supplier
            For i = 0 To cboSupplier.Items.Count - 1
                cboSupplier.SelectedIndex = i
                If cboSupplier.Text.Substring(cboSupplier.Text.Length - 10, 10) = Trim(.Rows(0)("Supplier").ToString) Then
                    Exit For
                End If
            Next
            If cboSupplier.SelectedIndex < 0 Then
                MessageBox.Show("SUPPLIER ERROR!" & Chr(10) & Chr(10) & "The supplier previously used on this order (" & Trim(.Rows(0)("Supplier").ToString) & ") is no longer valid, please select a different one from the list." & Chr(10) & Chr(10) & "Once you have updated and saved the order you will have to view the notes/QCR's manually.", "Please note....")
            End If

            txtSupplierCurr.Text = Llun(.Rows(0)("BPCurnCyID"))

            txtSupplierContact.Text = Llun(.Rows(0)("SuppliersRep"))
            txtAck.Text = Llun(.Rows(0)("supp_ack"))

            'Agent
            For i = 0 To cboAgent.Items.Count - 1
                cboAgent.SelectedIndex = i
                If Trim(.Rows(0)("Agent").ToString) = "NONE" Then
                    Exit For
                ElseIf i = 0 Then
                    GoTo NextAgent
                End If
                If cboAgent.Text.Substring(cboAgent.Text.Length - 10, 10) = Trim(.Rows(0)("Agent").ToString) Then
                    Exit For
                End If
NextAgent:
            Next
            'If Trim(.Rows(0)("Agent").ToString) > 0 And Trim(.Rows(0)("Agent").ToString) <> "NONE" And cboAgent.SelectedIndex <= 0 Then
            '    MessageBox.Show("AGENT ERROR!" & Chr(10) & Chr(10) & "The agent previously used on this order (" & Trim(.Rows(0)("Agent").ToString) & ") is no longer valid, please select a different one from the list." & Chr(10) & Chr(10) & "Once you have updated and saved the order you will have to view the notes/QCR's manually.", vbExclamation, "Please note....")
            'End If
            txtAgentCurr.Text = Llun(.Rows(0)("AgentCurncyID"))

            'Haulier
            Dim StartPoint As Integer
            Select Case Trim(.Rows(0)("Haulier").ToString)
                Case "COLLECTED"
                    StartPoint = 9
                Case "WORKS"
                    StartPoint = 5
                Case Else
                    StartPoint = 10
            End Select
            For i = 0 To cboHaulier.Items.Count - 1
                cboHaulier.SelectedIndex = i
                If cboHaulier.Text.Substring(cboHaulier.Text.Length - StartPoint, StartPoint) = Trim(.Rows(0)("Haulier").ToString) Then
                    Exit For
                End If
                cboHaulier.SelectedIndex = -1
            Next
            If cboHaulier.SelectedIndex < 0 Then
                MessageBox.Show("HAULIER ERROR!" & Chr(10) & Chr(10) & "The haulier previously used on this order (" & Trim(.Rows(0)("Haulier").ToString) & ") is no longer valid, please select a different one from the list." & Chr(10) & Chr(10) & "Once you have updated and saved the order you will have to view the notes/QCR's manually.", "Please note....")
            End If
            txtHaulierCurr.Text = Llun(.Rows(0)("HaulCurnCyID"))


            txtGoodsDisc1.Text = .Rows(0)("goodsdisc1").ToString
            txtGoodsDisc2.Text = .Rows(0)("goodsdisc2").ToString
            txtGoodsDisc3.Text = .Rows(0)("goodsdisc3").ToString

            txtTermsInWords.Text = Llun(.Rows(0)("bpterms").ToString)

            If .Rows(0)("architectorder") Then
                chkArchitect.IsChecked = True
            Else
                chkArchitect.IsChecked = False
            End If

            If .Rows(0)("plotdatareqd") Then
                chkPlotDataReqd.IsChecked = True
            Else
                chkPlotDataReqd.IsChecked = False
            End If

            If .Rows(0)("nopriceincreases") Then
                chkNoPriceIncreases.IsChecked = True
            Else
                chkNoPriceIncreases.IsChecked = False
            End If

            If .Rows(0)("suppress") Then
                chkSuppress.IsChecked = True
            Else
                chkSuppress.IsChecked = False
            End If

            'Customer
            For i = 0 To cboCustomer.Items.Count - 1
                cboCustomer.SelectedIndex = i
                If cboCustomer.Text.Substring(cboCustomer.Text.Length - 10, 10) = Trim(.Rows(0)("Customer").ToString) Then
                    Exit For
                End If
                cboCustomer.SelectedIndex = -1
            Next
            If cboCustomer.SelectedIndex < 0 Then
                MessageBox.Show("CUSTOMER ERROR!" & Chr(10) & Chr(10) & "The customer previously used on this order (" & Trim(.Rows(0)("Customer").ToString) & ") is no longer valid, please select a different one from the list." & Chr(10) & Chr(10) & "Once you have updated and saved the order you will have to view the notes/QCR's manually.", "Please note....")
            End If
            txtcustcode.text = Trim(.Rows(0)("Customer").ToString)
            txtCustomerCurr.Text = Llun(.Rows(0)("SPCurnCyID"))

            'txtSettlementDisc.Text = Llun(.Rows(0)("spdisc").ToString)

            'Main Contractor
            If Trim(.Rows(0)("Contractor").ToString) <> "" Then
                For i = 0 To cboMainContractor.Items.Count - 1
                    cboMainContractor.SelectedIndex = i
                    If cboMainContractor.Text.Substring(cboMainContractor.Text.Length - 10, 10) = Trim(.Rows(0)("Contractor").ToString) Then
                        Exit For
                    End If
                    cboMainContractor.SelectedIndex = -1
                Next
                If cboCustomer.SelectedIndex < 0 Then
                    MessageBox.Show("CUSTOMER ERROR!" & Chr(10) & Chr(10) & "The main contractor previously used on this order (" & Trim(.Rows(0)("Customer").ToString) & ") is no longer valid, please select a different one from the list." & Chr(10) & Chr(10) & "Once you have updated and saved the order you will have to view the notes/QCR's manually.", "Please note....")
                End If
            Else
                cboMainContractor.SelectedIndex = -1
            End If

            SetupCustomerRelatedCombos(Trim(.Rows(0)("Customer").ToString), Trim(.Rows(0)("CustomerBuyingOffice").ToString), Trim(.Rows(0)("AckAddrCode").ToString), Trim(.Rows(0)("InvAddrCode").ToString), Trim(.Rows(0)("DelAddrCode").ToString))

            txtAckContact.Text = Llun(.Rows(0)("ackcontact"))

            'Architect
            For i = 0 To cboArchitect.Items.Count - 1
                cboArchitect.SelectedIndex = i
                If Trim(.Rows(0)("ArchitectCode").ToString) = "NONE" Then

                    Exit For
                ElseIf i = 0 Then
                    GoTo NextArch
                End If
                If cboArchitect.Text.Substring(cboArchitect.Text.Length - 10, 10) = Trim(.Rows(0)("ArchitectCode").ToString) Then
                    Exit For
                End If
NextArch:
            Next
            If cboArchitect.SelectedIndex < 0 Then
                MessageBox.Show("ARCHITECT ERROR!" & Chr(10) & Chr(10) & "The architect previously used on this order (" & UCase(Trim(.Rows(0)("ArchitectCode").ToString)) & ") is no longer valid, please select a different one from the list." & Chr(10) & Chr(10) & "Once you have updated and saved the order you will have to view the notes/QCR's manually.", "Please note....")
            End If

            'Architect Contacts
            SetupArchitectContacts(Trim(.Rows(0)("ArchitectCode").ToString))
            For i = 0 To cboArchitectContact.Items.Count - 1
                cboArchitectContact.SelectedIndex = i
                If Trim(.Rows(0)("ArchitectID").ToString) = "NONE" Then
                    Exit For
                ElseIf i = 0 Then
                    GoTo NextArchContact
                End If
                If cboArchitectContact.Text.Substring(InStr(cboArchitectContact.Text, "-") + 1) = Trim(.Rows(0)("ArchitectID").ToString) Then
                    Exit For
                End If
NextArchContact:
            Next
            If cboArchitect.SelectedIndex < 0 Then
                MessageBox.Show("ARCHITECT ERROR!" & Chr(10) & Chr(10) & "The architect previously used on this order (" & UCase(Trim(.Rows(0)("ArchitectID").ToString)) & ") is no longer valid, please select a different one from the list." & Chr(10) & Chr(10) & "Once you have updated and saved the order you will have to view the notes/QCR's manually.", "Please note....")
            End If

            'Mileage

            txtCustomerCurr.Text = Trim(.Rows(0)("spcurncyid").ToString)
            If Trim(.Rows(0)("spcurncyid").ToString).Length = 0 Then txtCustomerCurr.Text = "GBP"
            txtCustomerCurrRate.Text = Trim(.Rows(0)("spcurncyrate").ToString)
            If Trim(txtCustomerCurr.Text) = "GBP" Then
                txtCustomerCurrRate.Visibility = Visibility.Hidden
            Else
                txtCustomerCurrRate.Visibility = Visibility.Visible
                txtCustomerCurr.Visibility = Visibility.Visible
            End If

            'txtSettlementDisc.Text = Format(Trim(.Rows(0)("SPDISC").ToString), "0.00")

            'Main Contractor
            Dim Customers As New cCustomers
            Customers.getCustomer(.Rows(0)("Contractor").ToString)

            If custBuyingOffice Then
                SetupContractorSpecOfficeCombo(.Rows(0)("Contractor").ToString, .Rows(0)("ContractorBuyingOffice").ToString)
                With Customers.dsCustomer.Tables(0)

                End With
                If cboMainContractorSpecOffice.Items.Count = 0 Then
                    cboMainContractorSpecOffice.IsEnabled = False
                    MessageBox.Show("SPECIFICATION OFFICE ERROR!" & Chr(10) & Chr(10) & "This main contractor that a specification office is selected but none have been created at Head Office." & Chr(10) & Chr(10) & "Please contact IT immediately as the buying will need to be created before you can save this order.", "Please note....")
                Else
                    cboMainContractorSpecOffice.IsEnabled = True
                    For i = 0 To cboMainContractorSpecOffice.Items.Count - 1
                        cboMainContractorSpecOffice.SelectedIndex = i
                        If Trim(cboMainContractorSpecOffice.Text) = Trim(.Rows(0)("ContractorBuyingOffice").ToString) Then
                            Exit For
                        End If
                        cboMainContractorSpecOffice.SelectedIndex = -1
                        If IsDBNull(.Rows(0)("ContractorBuyingOffice")) Then
                            cboMainContractorSpecOffice.SelectedIndex = -1
                            MessageBox.Show("SPECIFICATION OFFICE ERROR!" & Chr(10) & Chr(10) & "This main contractor requires a specification office to be selected.", "Please note....")
                        End If
                    Next
                End If
            Else
                cboMainContractorSpecOffice.Items.Clear()
                cboMainContractorSpecOffice.IsEnabled = False
            End If

            txtBuyersOrderNo.Text = UCase(Trim(.Rows(0)("buyers_order")))

            'Vehicle Types
            If IsDBNull(.Rows(0)("VehicleType")) Or Len(Trim(.Rows(0)("VehicleType"))) = 0 Then
                'If this_branchno = "47" Then
                '    cboVehicleType.SelectedIndex = 2
                'Else
                cboVehicleType.SelectedIndex = 1
                'End If
                GoTo vehicle_done
            End If

            For i = 0 To cboVehicleType.Items.Count - 1
                cboVehicleType.SelectedIndex = i
                If cboVehicleType.Text = Trim(.Rows(0)("VehicleType").ToString) Then
                    Exit For
                End If
                cboVehicleType.SelectedIndex = -1
            Next
vehicle_done:

            'Fixed Prices
            If Trim(Llun(.Rows(0)("SuppFixedPriceUntil"))) = "" Then
                txtBPFixed.Text = ""
            Else
                If Trim(Llun(.Rows(0)("SuppFixedPriceUntil"))) = "01/01/1900" Then
                    txtBPFixed.Text = ""
                Else
                    txtBPFixed.Text = Format(.Rows(0)("SuppFixedPriceUntil"), "dd/MM/yyyy")
                End If
            End If

            If Trim(Llun(.Rows(0)("CustFixedPriceUntil"))) = "" Then
                txtSPFixed.Text = ""
            Else
                If Trim(Llun(.Rows(0)("CustFixedPriceUntil"))) = "01/01/1900" Then
                    txtSPFixed.Text = ""
                Else
                    txtSPFixed.Text = Format(.Rows(0)("CustFixedPriceUntil"), "dd/MM/yyyy")
                End If
            End If
        End With


    End Sub

    Sub ClearOrderHeader()
        'txtOrderNo.Clear()
        txtOrderDate.Clear()
        txtBPOrder.Clear()
        cboSalesperson1.SelectedIndex = -1
        txtSalesSplit1.Clear()
        cboSalesperson2.SelectedIndex = -1
        txtSalesSplit2.Clear()
        cboSalesperson3.SelectedIndex = -1
        txtSalesSplit3.Clear()
        cboSpecifier.SelectedIndex = -1
        cboSupplier.SelectedIndex = -1
        txtSupplierCurr.Clear()
        cboWorks.SelectedIndex = -1
        txtSupplierContact.Clear()
        txtAck.Clear()
        cboAgent.SelectedIndex = -1
        txtAgentCurr.Clear()
        cboHaulier.SelectedIndex = -1
        txtHaulierCurr.Clear()
        cboDepot.SelectedIndex = -1
        txtGoodsDisc1.Clear()
        txtGoodsDisc2.Clear()
        txtGoodsDisc3.Clear()
        txtTermsInWords.Clear()
        cboCustomer.SelectedIndex = -1
        txtCustName.Clear()
        txtCustomerCurr.Clear()
        cboCustomerSpecOffice.SelectedIndex = -1
        txtCreditLimit.Clear()
        txtCurrentBalance.Clear()
        'txtSettlementDisc.Clear()
        cboMainContractor.SelectedIndex = -1
        txtMainContractorCurr.Clear()
        cboMainContractorSpecOffice.SelectedIndex = -1
        cboArchitect.SelectedIndex = -1
        cboArchitectContact.SelectedIndex = -1
        txtBuyersOrderNo.Clear()
        cboInvoiceAddress.SelectedIndex = -1
        cboAckAddress.SelectedIndex = -1
        txtAckContact.Clear()
        cboAckEmail.SelectedIndex = -1

        ClearSiteAddress()
        cboVehicleType.SelectedIndex = -1
        txtBPFixed.Clear()
        txtSPFixed.Clear()

    End Sub

    Sub ClearSiteAddress()
        cboSiteCode.SelectedIndex = -1
        txtSiteAddress1.Clear()
        txtSiteAddress2.Clear()
        txtSiteAddress3.Clear()
        txtSiteAddress4.Clear()
        txtSitePostCode.Clear()
        txtSiteTelephone.Clear()
        txtSiteFax.Clear()
        txtSiteContact.Clear()
        cboSiteEmail.SelectedIndex = -1
    End Sub

    Sub SetupCombos()

        'Customers
        cboCustomer.Items.Clear()
        For i As Integer = 0 To AllCustomers.Count - 1
            cboCustomer.Items.Add(AllCustomers(i))
        Next
        cboCustomer.SelectedIndex = -1

        'Salespeople
        cboSalesperson1.Items.Clear()
        cboSalesperson2.Items.Clear()
        cboSalesperson3.Items.Clear()
        For i As Integer = 0 To AllSalespeople.Count - 1
            cboSalesperson1.Items.Add(AllSalespeople(i))
            cboSalesperson2.Items.Add(AllSalespeople(i))
            cboSalesperson3.Items.Add(AllSalespeople(i))
        Next
        cboSalesperson1.SelectedIndex = -1
        cboSalesperson2.SelectedIndex = -1
        cboSalesperson3.SelectedIndex = -1

        'Specifiers
        cboSpecifier.Items.Clear()
        For i As Integer = 0 To AllSpecifiers.Count - 1
            cboSpecifier.Items.Add(AllSpecifiers(i))
        Next
        cboSpecifier.SelectedIndex = -1

        'Suppliers
        cboSupplier.Items.Clear()
        For i As Integer = 0 To AllSuppliers.Count - 1
            cboSupplier.Items.Add(AllSuppliers(i))
        Next
        cboSupplier.SelectedIndex = -1

        'Agents
        cboAgent.Items.Clear()
        cboAgent.Items.Add("NONE")
        For i As Integer = 0 To AllAgents.Count - 1
            cboAgent.Items.Add(AllAgents(i))
        Next
        cboAgent.SelectedIndex = -1

        'Hauliers
        cboHaulier.Items.Clear()
        For i As Integer = 0 To AllHauliers.Count - 1
            cboHaulier.Items.Add(AllHauliers(i))
        Next
        cboHaulier.SelectedIndex = -1

        'Suppliers
        cboCustomer.Items.Clear()
        For i As Integer = 0 To AllCustomers.Count - 1
            cboCustomer.Items.Add(AllCustomers(i))
        Next
        cboCustomer.SelectedIndex = -1

        'Main Contractor
        cboMainContractor.Items.Clear()
        For i As Integer = 0 To AllContractors.Count - 1
            cboMainContractor.Items.Add(AllContractors(i))
        Next
        cboMainContractor.SelectedIndex = -1

        'Architects
        cboArchitect.Items.Clear()
        cboArchitect.Items.Add("NONE")
        For i As Integer = 0 To AllArchitects.Count - 1
            cboArchitect.Items.Add(AllArchitects(i))
        Next
        cboArchitect.SelectedIndex = -1

        'Vehicle Types
        cboVehicleType.Items.Clear()
        For i As Integer = 0 To VehicleTypes.Count - 1
            cboVehicleType.Items.Add(VehicleTypes(i))
        Next
        cboVehicleType.SelectedIndex = -1

    End Sub


    Public Sub New()
        isLoading = True
        ' This call is required by the designer.
        InitializeComponent()
        Call SetupCombos()
        'expGridLines.Visibility = Visibility.Hidden
        If Llun(TMOrderNo).Length > 0 Then
            txtOrderNo.Text = TMOrderNo
            Call DisplayOrder(txtOrderNo.Text)
            TMOrderNo = ""
        End If
        isLoading = False
    End Sub

    Private Sub btnOrderRight_Click(sender As Object, e As RoutedEventArgs) Handles btnOrderRight.Click
        Dim wrkOrderNo As Double
        Dim wrkOrderPrefix As String
        If txtOrderNo.Text.Length > 0 Then
            wrkOrderPrefix = txtOrderNo.Text.Substring(0, 2)
            wrkOrderNo = txtOrderNo.Text.Substring(2)
            txtOrderNo.Text = wrkOrderPrefix & wrkOrderNo + 1
            DisplayOrder(txtOrderNo.Text)
        End If
    End Sub

    Private Sub btnOrderLeft_Click(sender As Object, e As RoutedEventArgs) Handles btnOrderLeft.Click
        Dim wrkOrderNo As Double
        Dim wrkOrderPrefix As String
        If txtOrderNo.Text.Length > 0 Then
            wrkOrderPrefix = txtOrderNo.Text.Substring(0, 2)
            wrkOrderNo = txtOrderNo.Text.Substring(2)
            txtOrderNo.Text = wrkOrderPrefix & wrkOrderNo - 1
            DisplayOrder(txtOrderNo.Text)
        End If

    End Sub

    Sub SetupArchitectContacts(Architect As String)
        cboArchitectContact.Items.Clear()
        Dim ArchitectContact As New cCustomers
        ArchitectContact.GetArchitectContact(Architect, 0,"*", "*")
        If ArchitectContact.ok = False Then Exit Sub

        cboArchitectContact.Items.Add("")
        With ArchitectContact.dsArchitect.Tables(0)
            For i As Integer = 0 To .Rows.Count - 1
                cboArchitectContact.Items.Add(Trim((.Rows(i)("Name"))) & " - " & Trim(.Rows(i)("ID")))
            Next
        End With

        cboArchitectContact.SelectedIndex = -1
    End Sub

    Sub SetupCustomerRelatedCombos(Customer As String, CustSpecOffice As String, AckAddressCode As String, InvAddressCode As String, SiteAddressCode As String)

        'Call CustContUnderline(frmBrick, frmBrick.cboCustomer, frmBrick.cboMainContractor)
        Dim Customers As New cCustomers
        Customers.getCustomer(Customer)
        If Customers.ok = False Then Exit Sub
        txtCustName.Text = Trim(Customers.dsCustomer.Tables("Customer").Rows(0)("CUSTNAME").ToString)

        With Customers.dsCustomer.Tables(0)

            txtCustomerCurr.Text = Customers.dsCustomer.Tables(0).Rows(0)("curncyID")
            If Trim(txtCustomerCurr.Text) = "GBP" Then
                txtCustomerCurrRate.Visibility = Visibility.Hidden
                txtCustomerCurrRate.Text = "1"
            Else
                txtCustomerCurrRate.Visibility = Visibility.Visible
                txtCustomerCurrRate.Text = "1"
            End If
            If .Rows(0)("CUSTCLAS") = "TBE" Then
                txtCreditLimit.Foreground = New SolidColorBrush(Colors.Green) 'Green
            Else
                txtCreditLimit.Foreground = New SolidColorBrush(Colors.Blue) 'Blue
            End If
            If .Rows(0)("CUSTCLAS") = "PBD" Then
                txtCreditLimit.Text = UCase(.Rows(0)("CUSTCLAS"))
            Else
                txtCreditLimit.Text = FormatCurrency(.Rows(0)("crlmtamt"), 0,,, TriState.True)
            End If

            txtCurrentBalance.Text = FormatCurrency(Customers.GetCurrentBal(Customer), 0,,, TriState.True)


            'txtSettlementDisc.Text = "0.00"
            'txtStoreSettDisc.text = "0.00"
            'If IsNumeric(.Rows(0)("CUSTDISC")) Then
            '    txtSettlementDisc.Text = Format(.Rows(0)("CUSTDISC") / 100, "0.00")
            '    'txtStoreSettDisc = Format(!CUSTDISC / 100, "0.00")
            'End If
        End With

        If custBuyingOffice Then
            SetupCustomerSpecOfficeCombo(Customer, CustSpecOffice)
            If cboCustomerSpecOffice.Items.Count = 0 Then
                cboCustomerSpecOffice.IsEnabled = False
                MsgBox("SPECIFICATION OFFICE ERROR!" & Chr(10) & Chr(10) & "This customer requires that a specification office Is selected but none have been created at Head Office." & Chr(10) & Chr(10) & "Please contact IT immediately As the buying will need To be created before you can save this order.", "Please note....")
            Else
                cboCustomerSpecOffice.IsEnabled = True
                For i = 0 To cboCustomerSpecOffice.Items.Count - 1
                    cboCustomerSpecOffice.SelectedIndex = i
                    If Trim(cboCustomerSpecOffice.Text) = Trim(CustSpecOffice) Then
                        Exit For
                    End If
                    cboCustomerSpecOffice.SelectedIndex = -1
                Next
            End If
        Else
            cboCustomerSpecOffice.Items.Clear()
            cboCustomerSpecOffice.IsEnabled = False
        End If

        If custOnHold <> 0 Then MessageBox.Show("CUSTOMER Is On STOP!" & Chr(10) & Chr(10) & "This customer Is currently On Stop so NO New orders can be created, existing orders can still be amended.", "Please note....")

        '        Call customer_display(UsingSQL, cboCustomer)



        ackaddress_populate(Customer, AckAddressCode)
        invaddress_populate(Customer, InvAddressCode)
        SiteAddress_populate(Customer, SiteAddressCode)
        DisplaySiteAddress(Customer, SiteAddressCode)

        'customer_done:

        If custNoNewBusiness Then
            If custNL Then
                MessageBox.Show("CUSTOMER ERROR!" & Chr(10) & Chr(10) & "This customer has had it's credit limit withdrawn and therefore orders cannot be created or modified.", "Please note....")
            Else
                MessageBox.Show("NO NEW BUSINESS ALLOWED!" & Chr(10) & Chr(10) & "We cannot currently transact new business with this customer." & Chr(10) & Chr(10) & "Existing orders will be fullfilled but you cannot create a new one.", "Please note....")
            End If
        End If


        '        Call MileageVerify(Me, cboCustomer.Text, cboMainContractor.Text, cbositeAddr.Text, cboSupplier.Text, cboworkscode.Text, False, False)


        If Mid(txtCustName.Text, 1, 17) = "John Graham Const" Then
            MessageBox.Show("JOHN GRAHAM CONSTRUCTION:" & Chr(10) & Chr(10) & Chr(10) & "NO VERBAL ORDERS" & Chr(10) & Chr(10) & "EMAILED/FAXED/PHYSICAL ORDERS ONLY" & Chr(10) & Chr(10) & "NO GOODS TO BE ADDED TO ORDERS UNLESS NEW/ADDITIONAL ORDER ISSUED" & Chr(10) & Chr(10) & "NO ORDER = NO GOODS" & Chr(10) & Chr(10) & "CORRECT ORDER NUMBERS MUST BE STATED ON INVOICES", "Please note....")
        End If

FinishItOffNicely:

        'If IgnoreClick = False Then
        '    Call VerifyFixedPricePeriod(Trim(cboSupplier), Trim(txtCustGroup), "")
        'End If

        Exit Sub
    End Sub

    Sub SetupCustomerSpecOfficeCombo(Customer As String, TempSpecOffice As String)
        cboCustomerSpecOffice.Items.Clear()

        If TempSpecOffice = "" Then
            GoTo FinishItOffNicely
        Else
            Dim CustAddress As New cCustomers
            CustAddress.getCustomerAddress(Customer, "*", "B")
            If CustAddress.ok = False Then GoTo FinishItOffNicely

            With CustAddress.dsCustomerAddress.Tables(0)
                For i As Integer = 0 To .Rows.Count - 1
                    cboCustomerSpecOffice.Items.Add(.Rows(i)("AdrsCode"))
                Next
            End With

            cboCustomerSpecOffice.SelectedIndex = -1

        End If

FinishItOffNicely:
    End Sub

    Sub SetupContractorSpecOfficeCombo(MainContractor As String, TempSpecOffice As String)
        cboMainContractorSpecOffice.Items.Clear()

        If TempSpecOffice = "" Then
            GoTo FinishItOffNicely
        Else
            Dim CustAddress As New cCustomers
            CustAddress.getCustomerAddress(MainContractor, "*", "B")
            If CustAddress.ok = False Then GoTo FinishItOffNicely

            With CustAddress.dsCustomerAddress.Tables(0)
                For i As Integer = 0 To .Rows.Count - 1
                    cboMainContractorSpecOffice.Items.Add(.Rows(i)("AdrsCode"))
                Next
            End With

            cboCustomerSpecOffice.SelectedIndex = -1

        End If

FinishItOffNicely:
    End Sub

    Sub invaddress_populate(Customer As String, TempAddress As String)

        cboInvoiceAddress.Items.Clear()
        Dim InvListIndex As Integer = 0
        cboInvoiceAddress.Items.Add("STATEMENT")
        Dim CustAddress As New cCustomers
        CustAddress.getCustomerAddress(Customer, "*", "I")
        If CustAddress.ok = False Then GoTo DisplayInvAddress

        With CustAddress.dsCustomerAddress.Tables(0)
            For i As Integer = 0 To .Rows.Count - 1
                cboInvoiceAddress.Items.Add(.Rows(i)("AdrsCode"))
                cboInvoiceAddress.SelectedIndex = i
                If Trim(cboInvoiceAddress.Text) = UCase(Trim(TempAddress)) Then
                    InvListIndex = cboInvoiceAddress.SelectedIndex
                End If
            Next
        End With

        'Check the last record added
        cboInvoiceAddress.SelectedIndex = cboInvoiceAddress.SelectedIndex + 1
        If Trim(cboInvoiceAddress.Text) = UCase(Trim(TempAddress)) Then
            InvListIndex = cboInvoiceAddress.SelectedIndex
        End If

DisplayInvAddress:
        If cboInvoiceAddress.Items.Count = 1 Then
            cboInvoiceAddress.SelectedIndex = 0
        Else
            cboInvoiceAddress.SelectedIndex = InvListIndex
        End If

    End Sub

    Sub ackaddress_populate(Customer As String, TempAddress As String)

        cboAckAddress.Items.Clear()
        Dim AckListIndex As Integer = 0
        cboAckAddress.Items.Add("STATEMENT")
        Dim CustAddress As New cCustomers
        CustAddress.getCustomerAddress(Customer, "*", "A")
        If CustAddress.ok = False Then GoTo DisplayAckAddress

        With CustAddress.dsCustomerAddress.Tables(0)
            For i As Integer = 0 To .Rows.Count - 1
                cboAckAddress.Items.Add(.Rows(i)("AdrsCode"))
                cboAckAddress.SelectedIndex = i
                If Trim(cboAckAddress.Text) = UCase(Trim(TempAddress)) Then
                    AckListIndex = cboAckAddress.SelectedIndex
                End If
            Next
        End With

        'Check the last record added
        cboAckAddress.SelectedIndex = cboAckAddress.SelectedIndex + 1
        If Trim(cboAckAddress.Text) = UCase(Trim(TempAddress)) Then
            AckListIndex = cboAckAddress.SelectedIndex
        End If

DisplayAckAddress:
        If cboAckAddress.Items.Count = 1 Then
            cboAckAddress.SelectedIndex = 0
        Else
            cboAckAddress.SelectedIndex = AckListIndex
        End If

    End Sub

    Sub SiteAddress_populate(Customer As String, TempAddress As String)

        cboSiteCode.Items.Clear()
        Dim SiteListIndex As Integer = -1
        Dim CustAddress As New cCustomers
        CustAddress.getCustomerAddress(Customer, "*", "S")
        If CustAddress.ok = False Then GoTo DisplaySiteAddress

        With CustAddress.dsCustomerAddress.Tables(0)
            For i As Integer = 0 To .Rows.Count - 1
                cboSiteCode.Items.Add(.Rows(i)("AdrsCode"))
                cboSiteCode.SelectedIndex = i

                If Trim(cboSiteCode.Text) = UCase(Trim(TempAddress)) Then
                    SiteListIndex = cboSiteCode.SelectedIndex
                End If
            Next
        End With

DisplaySiteAddress:
        If cboSiteCode.Items.Count = 1 Then
            cboSiteCode.SelectedIndex = 0
        Else
            cboSiteCode.SelectedIndex = SiteListIndex
        End If

    End Sub

    Sub DisplaySiteAddress(Customer As String, SiteAddress As String)
        Dim CustAddress As New cCustomers
        CustAddress.getCustomerAddress(Customer, SiteAddress, "S")
        If CustAddress.ok = False Then Exit Sub

        With CustAddress.dsCustomerAddress.Tables(0)
            txtSiteAddress1.Text = Llun(txtCustName.Text)
            txtSiteAddress2.Text = Llun(Trim(.Rows(0)("CNTCPRSN")))
            txtSiteAddress3.Text = Llun(Trim(.Rows(0)("ADDRESS1")))
            txtSiteAddress4.Text = Llun(Trim(.Rows(0)("ADDRESS2"))) & " " & Llun(Trim(.Rows(0)("CITY")))
            txtSitePostCode.Text = Llun(Trim(.Rows(0)("ZIP")))
            txtSiteTelephone.Text = Llun(Trim(.Rows(0)("PHONE1")))
            txtSiteFax.Text = Llun(Trim(.Rows(0)("FAX")))
            txtSiteContact.Text = Llun(Trim(.Rows(0)("ADDRESS3")))
        End With

        'Site Address Notes

        'Site Address Emails
        cboSiteEmail.Items.Clear()
        CustAddress.GetCustomerAddressEmail(Customer, SiteAddress, "S")
        If CustAddress.ok Then
            With CustAddress.dsCustomerEmail.Tables(0)
                If Trim(Llun(.Rows(0)("EMAIL1"))) <> "" Then
                    cboSiteEmail.Items.Add(Trim(.Rows(0)("EMAIL1")))
                    cboSiteEmail.SelectedIndex = 0
                End If
                If Trim(Llun(.Rows(0)("EMAIL2"))) <> "" Then cboSiteEmail.Items.Add(Trim(.Rows(0)("EMAIL2")))
                If Trim(Llun(.Rows(0)("EMAIL3"))) <> "" Then cboSiteEmail.Items.Add(Trim(.Rows(0)("EMAIL3")))
                If Trim(Llun(.Rows(0)("EMAIL4"))) <> "" Then cboSiteEmail.Items.Add(Trim(.Rows(0)("EMAIL4")))
            End With
        End If
    End Sub

    Private Sub cboSiteCode_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSiteCode.SelectionChanged

    End Sub

    Private Sub cboSiteCode_DropDownClosed(sender As Object, e As EventArgs) Handles cboSiteCode.DropDownClosed
        Call DisplaySiteAddress(Trim(txtCustCode.Text), Trim(cboSiteCode.Text))
    End Sub


    Private Sub grdOrderLines_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles grdOrderLines.MouseDoubleClick
        'Dim test As cOrderLines = grdOrderLines.SelectedItem
        'MessageBox.Show(test.LineNo)
    End Sub



    Private Sub grdOrderLines_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles grdOrderLines.MouseDown
        'Dim test As cOrderLines = grdOrderLines.SelectedItem
        'MessageBox.Show(test.LineNo)
    End Sub

    Private Sub grdOrderLines_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles grdOrderLines.MouseLeftButtonDown
        Call DisplayOrderLineDetails()
    End Sub

    Private Sub DisplayOrderLineDetails()
        Dim StringPosition As Integer

        Call ClearOrderLineDetails()

        Dim OrderLine As cOrderLines = grdOrderLines.SelectedItem
        txtLineNo.Text = OrderLine.LineNo
        txtQty.Text = OrderLine.OrderQty
        'Products here
        txtDescription1.Text = OrderLine.Desc1
        txtDescription2.Text = OrderLine.Desc2
        txtDescription3.Text = OrderLine.Desc3
        txtDE.Text = OrderLine.DorE
        'Goods & Per
        StringPosition = InStr(OrderLine.GoodsVal, vbCrLf)
        txtGoodsBP.Text = OrderLine.GoodsVal.Substring(0, StringPosition)
        txtGoodsPer.Text = OrderLine.GoodsVal.Substring(StringPosition + 1)
        txtPackingBP.Text = OrderLine.PackingVal
        'Haulage & Per
        StringPosition = InStr(OrderLine.HaulVal, vbCrLf)
        txtHaulageBP.Text = OrderLine.HaulVal.Substring(0, StringPosition)
        txtHaulagePer.Text = OrderLine.HaulVal.Substring(StringPosition + 1)
        'Commision & Per
        StringPosition = InStr(OrderLine.CommVal, vbCrLf)
        txtCommissionBP.Text = OrderLine.CommVal.Substring(0, StringPosition)
        txtCommissionPer.Text = OrderLine.CommVal.Substring(StringPosition + 1)
        'SP & Per
        StringPosition = InStr(OrderLine.SPVal, vbCrLf)
        txtSP.Text = OrderLine.SPVal.Substring(0, StringPosition)
        txtSPPer.Text = OrderLine.SPVal.Substring(StringPosition + 1)
        'Margin
        txtMargin.Text = OrderLine.Margin
        'Effective Date
        expGridLines.IsExpanded = True
        expGridLines.Visibility = Visibility.Visible
    End Sub

    Private Sub ClearOrderLineDetails()
        txtLineNo.Clear()

        txtQty.Clear()
        cboProductGroup.SelectedIndex = -1
        txtDescription1.Clear()
        txtDE.Clear()
        txtGoodsBP.Clear()
        txtPackingBP.Clear()
        txtHaulageBP.Clear()
        txtCommissionBP.Clear()
        txtSP.Clear()
        txtEffectiveDate.Clear()
        cboProductGroup2.SelectedIndex = -1
        txtDescription2.Clear()
        txtGoodsPer.Clear()
        txtHaulagePer.Clear()

        txtCommissionPer.Clear()
        txtSPPer.Clear()
        cboProductGroup3.SelectedIndex = -1
        txtDescription3.Clear()
        txtPlots.Clear()
        txtBuyersOrder.Clear()
        txtLineGoodsDisc1.Clear()
        txtHaulDisc1.Clear()
        cboBPLine.SelectedIndex = -1
        txtLineGoodsDisc2.Clear()
        txtHaulDisc2.Clear()
        txtCollectionNo.Clear()
        txtLineGoodsDisc3.Clear()
        txtHaulDisc3.Clear()
    End Sub

End Class
