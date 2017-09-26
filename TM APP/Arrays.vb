Public Class cArrays
    Sub setupArrays()

        'Customer
        Dim Customers As New cCustomers
        Customers.getCustomer("*")

        With Customers.dsCustomer.Tables("Customer")
            'MsgBox(.Rows.Count - 1)
            ReDim AllCustomers(.Rows.Count - 1)
            For i = 0 To .Rows.Count - 1
                AllCustomers(i) = Trim((.Rows(i)("CUSTNAME"))) & " - " & Trim(.Rows(i)("CUSTNMBR"))
            Next
        End With

        'Salespeople
        Dim Salespeople As New cSalesperson
        Salespeople.getSalesperson("*")

        With Salespeople.dsSalespeople.Tables("Salespeople")
            ReDim AllSalespeople(.Rows.Count - 1)
            For i = 0 To .Rows.Count - 1
                AllSalespeople(i) = Trim((.Rows(i)("SLPRSNID"))) & " " & Trim(.Rows(i)("SLPRSNFN")) & " " & Trim(.Rows(i)("SPRSNSLN"))
            Next
        End With

        'Specifiers
        Dim Specifier As New cSalesperson
        Specifier.getSpecifier("*")

        With Specifier.dsSpecifier.Tables("Specifier")
            ReDim AllSpecifiers(.Rows.Count - 1)
            For i = 0 To .Rows.Count - 1
                AllSpecifiers(i) = Trim((.Rows(i)("SLPRSNID"))) & " " & Trim(.Rows(i)("SLPRSNFN")) & " " & Trim(.Rows(i)("SPRSNSLN"))
            Next
        End With

        'Suppliers
        Dim Supplier As New cSuppliers
        Supplier.getSupplier("*")

        With Supplier.dsSupplier.Tables("Supplier")
            ReDim AllSuppliers(.Rows.Count - 1)
            For i = 0 To .Rows.Count - 1
                AllSuppliers(i) = Trim((.Rows(i)("VENDNAME"))) & " - " & Trim(.Rows(i)("VENDORID"))
            Next
        End With

        'Agents
        Dim Agent As New cSuppliers
        Agent.getAgent("*")

        With Agent.dsAgent.Tables("Agent")
            ReDim AllAgents(.Rows.Count - 1)
            For i = 0 To .Rows.Count - 1
                AllAgents(i) = Trim((.Rows(i)("VENDNAME"))) & " - " & Trim(.Rows(i)("VENDORID"))
            Next
        End With

        'Haulier
        Dim Haulier As New cSuppliers
        Haulier.getHaulier("*")

        With Haulier.dsHaulier.Tables("Haulier")
            ReDim AllHauliers(.Rows.Count - 1)
            For i = 0 To .Rows.Count - 1
                AllHauliers(i) = Trim((.Rows(i)("VENDNAME"))) & " - " & Trim(.Rows(i)("VENDORID"))
            Next
        End With

        'Contractors
        Dim Contractor As New cCustomers
        Contractor.getContractor("*")

        With Contractor.dsContractor.Tables("Contractor")
            ReDim AllContractors(.Rows.Count - 1)
            For i = 0 To .Rows.Count - 1
                AllContractors(i) = Trim((.Rows(i)("CUSTNAME"))) & " - " & Trim(.Rows(i)("CUSTNMBR"))
            Next
        End With

        'Architects
        Dim Architect As New cCustomers
        Architect.GetArchitect("*", 0, "*")

        With Architect.dsArchitect.Tables("Architect")
            ReDim AllArchitects(.Rows.Count - 1)
            For i = 0 To .Rows.Count - 1
                AllArchitects(i) = Trim((.Rows(i)("Name"))) & " - " & Trim(.Rows(i)("Code"))
            Next
        End With

        'Vehicle Types
        ReDim VehicleTypes(20)
        VehicleTypes(0) = "TAIL LIFT"
        VehicleTypes(1) = "ARTIC"
        VehicleTypes(2) = "ARTIC+CRNE"
        VehicleTypes(3) = "ARTIC OPEN"
        VehicleTypes(4) = "RIGID"
        VehicleTypes(5) = "RIGID+CRNE"
        VehicleTypes(6) = "RIGID+MOFF"
        VehicleTypes(7) = "DRAWBAR"
        VehicleTypes(8) = "DRAWB+CRNE"
        VehicleTypes(9) = "MOF MOUNTY"
        VehicleTypes(10) = "FLATBED"
        VehicleTypes(11) = "CRANE"
        VehicleTypes(12) = "CURTAIN"
        VehicleTypes(13) = "DROPANDGO"
        VehicleTypes(14) = "TIPPER"
        VehicleTypes(15) = "ANY"
        VehicleTypes(16) = "COURIER"
        VehicleTypes(17) = "MANITOU"
        VehicleTypes(18) = "URBANARTIC"
        VehicleTypes(19) = "PALLETNET"

        'Comment Types
        ReDim CommentTypes(7)
        CommentTypes(0) = "A - All"
        CommentTypes(1) = "B - Both"
        CommentTypes(2) = "C - Customer"
        CommentTypes(3) = "H - Haulier"
        CommentTypes(4) = "I - Iternal"
        CommentTypes(5) = "O - Call Off"
        CommentTypes(6) = "S - Supplier"

    End Sub
End Class
