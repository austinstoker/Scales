Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.ComponentModel
Imports System.Threading

Public Structure Prefs
    Dim RetainTare As Boolean
    Dim RequireFrom As Boolean
    Dim StartFrom As String
    Dim RequireTo As Boolean
    Dim StartTo As String
    Dim RequireHowMany As Boolean
    Dim StartHowMany As Integer
    Dim StartCropNum As Integer
    Dim RequireCropNum As Boolean
End Structure


Public Class Form2
    Dim ready As Boolean = True
    Dim ScaleFilePath As String = "C:\ScaleFilePath.txt"
    Dim MyExtension As String = ".txt"
    Dim TreePath As String
    Dim TruckingPath As String
    Dim WhatPath As String
    Dim FromPath As String
    Dim ToPath As String
    Dim Weight As Long
    Dim Buffer() As String
    Dim Current() As String
    Dim pd As New PrintDocument()
    Dim printstring As String
    Public keeptop As Boolean
    Dim lbdn As ListBox
    Dim ScrollCount As Integer
    Dim lbup As ListBox
    Dim ChangingTruck As Boolean = False
    Dim ChangingWhat As Boolean = False
    Dim ChangingFrom As Boolean = False
    Dim ChangingTo As Boolean = False
    Dim TruckPrefs As Prefs
    Dim WhatPrefs As Prefs
    Dim askfromandto As Boolean = False
    Dim Showaddfolderbuttons As Boolean = False
    Dim IsZero As Boolean = False

    Private Sub FindSourceFiles()
        If File.Exists("C:\ScaleFilePath.txt") Then
            TreePath = File.ReadAllText("C:\ScaleFilePath.txt")
        Else
            TreePath = "C:\DropBox\Scales\Scales 2"
        End If
        TruckingPath = TreePath & "\Trucking"
        WhatPath = TreePath & "\What"
        FromPath = TreePath & "\Location"
        ToPath = FromPath
    End Sub

    Private Sub GetDirectoriesWhat(ByVal subDirs() As DirectoryInfo, _
        ByVal nodeToAddTo As TreeNode)

        Dim aNode As TreeNode
        Dim bNode As TreeNode
        Dim filelist() As FileInfo
        Dim aFile As FileInfo
        Dim subSubDirs() As DirectoryInfo
        Dim subDir As DirectoryInfo
        For Each subDir In subDirs
            aNode = New TreeNode(subDir.Name, 0, 0)
            aNode.Tag = subDir
            aNode.ImageKey = "folder"
            subSubDirs = subDir.GetDirectories()
            If subSubDirs.Length <> 0 Then
                GetDirectories(subSubDirs, aNode)
            End If
            nodeToAddTo.Nodes.Add(aNode)
            filelist = subDir.GetFiles()
            For Each aFile In filelist
                'If aFile.Name.Contains(".ID") = True Then Continue For
                bNode = New TreeNode(aFile.Name, 0, 0)
                bNode.Tag = aFile.Name
                bNode.ImageKey = "file"
                nodeToAddTo.Nodes.Add(bNode)
            Next aFile

        Next subDir

    End Sub


    Private Sub TreeViewWhat_NodeMouseClick(ByVal sender As Object, _
    ByVal e As TreeNodeMouseClickEventArgs) Handles TreeViewWhat.NodeMouseClick


        UpdateList(e.Node, TreeViewWhat, ListViewWhat, ListBoxWhat)

    End Sub
    Private Sub UpdateList(ByVal selectedNode As TreeNode, ByRef tree As TreeView, ByRef LView As ListView, ByRef LBox As ListBox)

        Dim newSelected As TreeNode = selectedNode
        If newSelected.ImageKey = "file" Then Exit Sub ' this prevents an error when seeking directories in a file

        LView.Items.Clear()
        LBox.Items.Clear()
        Dim nodeDirInfo As DirectoryInfo = _
        CType(newSelected.Tag, DirectoryInfo)
        Dim subItems() As ListViewItem.ListViewSubItem
        Dim item As ListViewItem = Nothing
        'Dim item2 As String = Nothing

        Dim dir As DirectoryInfo
        For Each dir In nodeDirInfo.GetDirectories()
            item = New ListViewItem(dir.Name, 0)
            subItems = New ListViewItem.ListViewSubItem() _
                {New ListViewItem.ListViewSubItem(item, "Directory"), _
                New ListViewItem.ListViewSubItem(item, _
                dir.LastAccessTime.ToShortDateString())}

            item.SubItems.AddRange(subItems)
            LView.Items.Add(item)
            LBox.Items.Add(item.Text)
        Next dir
        Dim file As FileInfo
        For Each file In nodeDirInfo.GetFiles()
            item = New ListViewItem((file.Name), 1)
            subItems = New ListViewItem.ListViewSubItem() _
                {New ListViewItem.ListViewSubItem(item, "File"), _
                New ListViewItem.ListViewSubItem(item, _
                file.LastAccessTime.ToShortDateString())}
            If item.Text.Contains(".ID") Then Continue For
            If item.Text.Contains(MyExtension) Then item.Text = LSet(item.Text, item.Text.Length - 4)
            item.SubItems.AddRange(subItems)
            LView.Items.Add(item)
            LBox.Items.Add(item.Text)
        Next file
    End Sub

    Private Sub PopulateTreeView(ByRef tree As TreeView, ByVal myPath As String)
        tree.Nodes.Clear()
        Dim rootNode As TreeNode

        Dim info As New DirectoryInfo(myPath)
        If info.Exists Then
            rootNode = New TreeNode(info.Name)
            rootNode.Tag = info
            GetDirectories(info.GetDirectories(), rootNode)
            tree.Nodes.Add(rootNode)
        End If

    End Sub


    Private Sub GetDirectories(ByVal subDirs() As DirectoryInfo, _
        ByVal nodeToAddTo As TreeNode)

        Dim aNode As TreeNode
        Dim bNode As TreeNode
        Dim filelist() As FileInfo
        Dim aFile As FileInfo
        Dim subSubDirs() As DirectoryInfo
        Dim subDir As DirectoryInfo
        For Each subDir In subDirs
            aNode = New TreeNode(subDir.Name, 0, 0)
            aNode.Tag = subDir
            aNode.ImageKey = "folder"
            subSubDirs = subDir.GetDirectories()
            If subSubDirs.Length <> 0 Then
                GetDirectories(subSubDirs, aNode)
            End If
            nodeToAddTo.Nodes.Add(aNode)
            filelist = subDir.GetFiles()
            For Each aFile In filelist
                'If aFile.Name.Contains(".ID") = True Then Continue For
                bNode = New TreeNode(aFile.Name, 0, 0)
                bNode.Tag = aFile.Name
                bNode.ImageKey = "file"
                nodeToAddTo.Nodes.Add(bNode)
            Next aFile

        Next subDir

    End Sub


    Private Sub TreeView2_NodeMouseClick(ByVal sender As Object, _
    ByVal e As TreeNodeMouseClickEventArgs) Handles TreeView2.NodeMouseClick


        UpdateList(e.Node, TreeView2, ListView1, ListBox1)

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RefreshTrucking()
        RefreshWhat()
        RefreshFrom()
        RefreshTo()
    End Sub
    Private Sub RefreshTrucking()
        PopulateTreeView(TreeView2, TruckingPath)
        TreeView2.SelectedNode = TreeView2.TopNode
        UpdateList(TreeView2.TopNode, TreeView2, ListView1, ListBox1)
        txtTruck.Text = ""

    End Sub
    Public Sub RefreshWhat()
        PopulateTreeView(TreeViewWhat, WhatPath)
        TreeViewWhat.SelectedNode = TreeViewWhat.TopNode
        UpdateList(TreeViewWhat.TopNode, TreeViewWhat, ListViewWhat, ListBoxWhat)
        txtWhat.Text = ""
    End Sub
    Private Sub RefreshFrom()
        PopulateTreeView(TreeViewFrom, FromPath)
        TreeViewFrom.SelectedNode = TreeViewFrom.TopNode
        UpdateList(TreeViewFrom.TopNode, TreeViewFrom, ListViewFrom, ListBoxFrom)
        txtFrom.Text = ""

    End Sub
    Private Sub RefreshTo()
        PopulateTreeView(TreeViewTo, ToPath)
        TreeViewTo.SelectedNode = TreeViewTo.TopNode
        UpdateList(TreeViewTo.TopNode, TreeViewTo, ListViewTo, ListBoxTo)
        TxtTo.Text = ""

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TruckingBack.Click
        ClickBack(TreeView2, ListView1, ListBox1, txtTruck)
        GuideMe()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TruckingOK.Click
        TruckingOkSub()
    End Sub
    Private Sub TruckingOkSub()
        ClickOK(TreeView2, ListView1, ListBox1, txtTruck)
        If txtTruck.Text.Length > 0 And Microsoft.VisualBasic.Right(txtTruck.Text, 1) <> "\" Then
            GBoxWhat.Visible = True
            WeightEmpty.Enabled = True
            ChangingTruck = False
            PanLastTrans.Visible = False
            Dim desiredpath = TruckingPath & "\" & txtTruck.Text & ".txt"
            Loadprefs(desiredpath, TruckPrefs)

            Dim j As Integer
            Dim temp() As String
            Buffer = File.ReadAllLines(TreePath & "\Transactions.txt")
            For i = 1 To Buffer.Length - 1
                j = Buffer.Length - i

                If Buffer(j).Contains(txtTruck.Text) Then
                    temp = Split(Buffer(j), ",")
                    If temp(0) = txtTruck.Text Then
                        If temp(1) = "Full" Or temp(1) = "Empty" Then
                            findstuff(temp(2), ListBoxWhat, WhatOK)

                            LblFullEmpty.Text = temp(1)
                            lblLastDateTime.Text = temp(3)
                            If temp(1) = "Full" Then
                                lblLastWeight.Text = temp(4)
                            Else
                                lblLastWeight.Text = temp(5)
                            End If
                            PanLastTrans.Visible = True
                            If temp.Length > 9 Then
                                If temp(9) <> "" And txtHowMany.Visible = True Then
                                    txtHowMany.Text = temp(9)
                                End If
                            End If
                            If temp.Length > 10 Then
                                If temp(10) <> "" And txtCropNum.Visible = True Then
                                    txtCropNum.Text = temp(10)
                                End If
                            End If
                            'Put something here about from and to auto filling

                            Exit For
                        Else
                            'WhatPan.Visible = False
                            ' RefreshWhat()
                            PanLastTrans.Visible = False
                            Exit For
                        End If
                    End If
                End If
            Next
        End If
        GuideMe()
    End Sub

    Private Sub ClickOK(ByRef myTree As TreeView, ByRef myLview As ListView, ByRef myLBox As ListBox, ByRef mytbox As TextBox)
        'If ready = False Then Exit Sub
        Dim cNode As New TreeNode
        Dim temp() As String
        Dim temppath As String = ""
        If myTree.SelectedNode.Nodes.Count < 1 Then
            temp = Split(myTree.SelectedNode.FullPath, "\")
            For i = 1 To temp.Length - 1
                temppath = temppath & temp(i) & "\"
            Next
            mytbox.Text = temppath & myLBox.SelectedItem
            Exit Sub
        End If


        For Each cNode In myTree.SelectedNode.Nodes
            If cNode.Text = myLBox.SelectedItem Then
                myTree.SelectedNode = cNode
                'ready = False
                UpdateList(cNode, myTree, myLview, myLBox)
            End If
        Next cNode
        temp = Split(myTree.SelectedNode.FullPath, "\")
        For i = 1 To temp.Length - 1
            temppath = temppath & temp(i) & "\"
        Next
        mytbox.Text = temppath & myLBox.SelectedItem

        'If mytbox.Text.Length > 0 And Microsoft.VisualBasic.Right(mytbox.Text, 1) <> "\" Then
        '    GBoxWhat.Visible = True
        '    WeightEmpty.Enabled = True
        '    ChangingTruck = False
        'End If

    End Sub

    Private Sub ClickBack(ByRef mytree As TreeView, ByRef myLview As ListView, ByRef myLbox As ListBox, ByRef myTbox As TextBox)
        Dim pNode As TreeNode = mytree.SelectedNode.Parent
        Dim temp() As String
        Dim temppath As String = ""
        If mytree.SelectedNode.Level > 0 Then
            mytree.SelectedNode = pNode
            UpdateList(pNode, mytree, myLview, myLbox)
        End If
        temp = Split(mytree.SelectedNode.FullPath, "\")
        For i = 1 To temp.Length - 1
            temppath = temppath & temp(i) & "\"
        Next
        myTbox.Text = temppath
    End Sub

    Function ReceiveSerialData() As String
        ' Receive strings from a serial port.
        Dim returnStr As String = ""
        If IO.Ports.SerialPort.GetPortNames.Contains("com1") = False Then
            Return returnStr
            Exit Function
        End If

        Dim com1 As IO.Ports.SerialPort = Nothing
        Try
            com1 = My.Computer.Ports.OpenSerialPort("COM1")
            com1.ReadTimeout = 10000
            com1.BaudRate = 9600
            com1.DataBits = 8
            com1.Parity = Ports.Parity.None
            com1.StopBits = 1

            Do
                Dim Incoming As String = com1.ReadLine()
                If Incoming Is Nothing Then
                    Exit Do
                Else
                    returnStr &= Incoming & vbCrLf
                End If
            Loop
        Catch ex As TimeoutException
            returnStr = "Error: Serial Port read timed out."
        Finally
            If com1 IsNot Nothing Then com1.Close()
        End Try

        Return returnStr
    End Function

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeightTimer.Tick
            Dim filetext() As String
            Dim filetext1() As String
        Dim tempWeight As Long
        Dim tempDate As Date
        Try
            If File.Exists(TreePath & "\Weight.txt") = False Then
                PanStatus.BackColor = Color.Black
                Exit Sub
            Else
                filetext1 = File.ReadAllLines(TreePath & "\Weight.txt")
                tempWeight = 0
                tempDate = Split(filetext1(0), " ", 2)(1)
                tempWeight = Split(filetext1(0), " ", 2)(0)
                TextBox1.Text = tempWeight
                LblDelay.Text = tempDate
                If tempWeight < 500 Then
                    LblWeightVis.Text = tempWeight
                Else
                    LblWeightVis.Text = "*****"
                End If

            End If
          
        Catch ex As Exception
            Dim t1 As String
            t1 = ex.Message

        Finally
        End Try


    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        GuideMe()

    End Sub

    Private Sub SetPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim FBD As New FolderBrowserDialog

        If FBD.ShowDialog = DialogResult.OK Then
            TreePath = FBD.SelectedPath
        End If
    End Sub
    Public Sub ListBoxUp(ByVal Box As ListBox)
        If Box Is Nothing Then Exit Sub
        If Box.SelectedIndex > 0 Then
            Box.SelectedIndex = Box.SelectedIndex - 1
            If Box.SelectedIndex < Box.Items.Count - 3 Then Box.TopIndex = Box.TopIndex - 1
        End If

    End Sub

    Public Sub ListBoxDown(ByVal Box As ListBox)
        If Box.SelectedIndex < Box.Items.Count - 1 Then
            Box.SelectedIndex = Box.SelectedIndex + 1
            If Box.SelectedIndex > 2 Then Box.TopIndex = Box.TopIndex + 1
        End If
    End Sub



    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCompany.Click
        Dim Created As String
        Created = AddFolder("Add a New Company Name", TruckingPath, TreeView2)
        If Created <> "" Then
            RefreshTrucking()
            findstuff(Created, ListBox1, TruckingOK)
        End If
    End Sub

    Private Function AddFolder(ByVal message As String, ByVal mypath As String, ByVal myTree As TreeView)
        Dim CreatePath As String = ""
        Dialog1.TextBox1.Text = ""
        Dialog1.Label1.Text = message
        Dialog1.ShowDialog()
        If Dialog1.DialogResult = DialogResult.OK Then
            Dim Temp() As String = Split(mypath, "\")

            Dim tempname As String

            For i = 0 To Temp.Length - 2
                CreatePath = CreatePath & Temp(i) & "\"
            Next
            tempname = StrConv(Dialog1.TextBox1.Text, vbProperCase)
            CreatePath = CreatePath & myTree.SelectedNode.FullPath & "\" & tempname
            My.Computer.FileSystem.CreateDirectory(CreatePath)
        End If
        Return CreatePath
        Dialog1.Label1.Text = ""
    End Function

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddTruck.Click
        Dim Created As String
        Created = addfile("Add a new Truck", TruckingPath, "\Default Files\defaultTruck.txt", TreeView2)
        If Created <> "" Then
            RefreshTrucking()
            findstuff(Created, ListBox1, TruckingOK)
        End If
    End Sub
    Private Function addfile(ByVal message As String, ByVal mypath As String, ByVal typepath As String, ByVal mytree As TreeView)
        Dialog1.TextBox1.Text = ""
        Dialog1.Label1.Text = message
        Dialog1.ShowDialog()
        If Dialog1.DialogResult = DialogResult.OK Then
            Dim Temp() As String = Split(mypath, "\")
            Dim CreatePath As String = ""
            Dim tempname As String
            Dim sourcefile As String
            Dim ReturnPath As String
            For i = 0 To Temp.Length - 2
                CreatePath = CreatePath & Temp(i) & "\"
            Next

            tempname = StrConv(Dialog1.TextBox1.Text, vbProperCase)
            CreatePath = CreatePath & mytree.SelectedNode.FullPath & "\" & tempname & ".txt"
            ReturnPath = CreatePath & mytree.SelectedNode.FullPath & "\" & tempname
            sourcefile = TreePath & typepath '"\DefaultTruck.txt"
            My.Computer.FileSystem.CopyFile(sourcefile, CreatePath)
            Return ReturnPath
        Else
            Return ""
        End If
        Dialog1.Label1.Text = ""
    End Function

    Private Sub NextWhat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WhatOK.Click
        WhatOKSub()
    End Sub
    Private Sub WhatOKSub()
        ClickOK(TreeViewWhat, ListViewWhat, ListBoxWhat, txtWhat)
        If txtWhat.Text.Length > 0 And Microsoft.VisualBasic.Right(txtWhat.Text, 1) <> "\" Then
            WeighFull.Enabled = True
            ChangingWhat = False
        End If
        GuideMe()
    End Sub


    Private Sub UpWhat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpWhat.Click
        ListBoxUp(ListBoxWhat)
        GuideMe()
    End Sub

    Private Sub DownWhat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownWhat.Click
        GuideMe()
    End Sub

    Private Sub BackWhat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WhatBack.Click
        ClickBack(TreeViewWhat, ListViewWhat, ListBoxWhat, txtWhat)
        GuideMe()
    End Sub
    Private Sub BackClick(ByRef Tree As TreeView, ByRef box As TextBox)
        Dim pNode As TreeNode = Tree.SelectedNode.Parent
        Dim temp() As String
        Dim temppath As String = ""
        If Tree.SelectedNode.Level > 0 Then
            Tree.SelectedNode = pNode
            UpdateList(pNode, TreeViewWhat, ListViewWhat, ListBoxWhat)
        End If
        temp = Split(Tree.SelectedNode.FullPath, "\")
        For i = 1 To temp.Length - 1
            temppath = temppath & temp(i)
        Next
        box.Text = temppath
    End Sub
    Private Sub AddWhatCategory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddWhatCategory.Click
        Dim Created As String
        Created = AddFolder("Add a New Commodity Category", WhatPath, TreeViewWhat)
        If Created <> "" Then
            RefreshWhat()
            findstuff(Created, ListBoxWhat, WhatOK)
        End If
    End Sub

    Private Sub AddCommodity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCommodity.Click
        Dim Created As String
        Created = addfile("Add a new Commodity", WhatPath, "\Default Files\defaultWhat.txt", TreeViewWhat)
        If Created <> "" Then
            RefreshWhat()
            findstuff(Created, ListBoxWhat, WhatOK)
        End If
    End Sub

    Private Sub WeighFull_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeighFull.Click
        If Weight = 0 Then
            MessageBox.Show("The scale is reading zero, you cannot weigh", "Nothing on scale")
            Exit Sub
        End If
        If Microsoft.VisualBasic.Right(txtTruck.Text, 1) = "\" Or Microsoft.VisualBasic.Right(txtWhat.Text, 1) = "\" Or txtTruck.Text = "" Or txtWhat.Text = "" Then
            MessageBox.Show("You must finish answering Who you are and What you are hauling")
            Exit Sub
        End If
        Dim j As Integer
        Dim temp() As String
        Buffer = File.ReadAllLines(TreePath & "\Transactions.txt")
        For i = 1 To Buffer.Length - 1
            j = Buffer.Length - i

            If Buffer(j).Contains(txtTruck.Text) Then
                temp = Split(Buffer(j), ",")
                If temp(0) = txtTruck.Text Then
                    If temp(1) = "Empty" Then
                        Current = temp
                        completefull(Current)
                        Exit Sub
                    Else
                        If WhatPrefs.RetainTare = False Then
                            partialfull()
                            Exit Sub
                        Else
                            If j > 10000 Then
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If
        Next
        partialfull()
    End Sub
    Private Sub completefull(ByVal cur() As String)
        Dim BigString As String
        Dim gross As Long = 0
        Dim net As Long = 0
        Dim tare As Long = 0
        Dim printFont As Font
        Dim streamToPrint As StreamReader
        Dim TransDate As String
        Dim WhatStr As String = txtWhat.Text
        Dim TruckingStr As String = txtTruck.Text
        Dim FromStr As String = txtFrom.Text
        Dim ToStr As String = TxtTo.Text
        Dim HowManyStr As String = ""
        Dim CropNumStr As String = ""

        If cur.Length > 9 Then
            HowManyStr = cur(9)
        End If

        If cur.Length > 10 Then
            CropNumStr = cur(10)
        End If

        If txtHowMany.Visible = True Then
            HowManyStr = txtHowMany.Text
        End If

        If txtCropNum.Visible = True Then
            CropNumStr = txtCropNum.Text
        End If

        gross = Weight
        tare = Val(cur(5))

        If gross < tare Then
            tare = gross
            gross = Val(cur(5))
        End If

        net = gross - tare

        TransDate = Date.Now.ToString
        BigString = vbNewLine & cur(0) & "," & "DoneFull" & "," & WhatStr & "," & TransDate & "," & Str(gross) & "," & Str(tare) & "," & Str(net) & "," & FromStr & "," & ToStr & "," & HowManyStr & "," & CropNumStr
        'FileOpen(1, TreePath & "\Transactions.txt", Microsoft.VisualBasic.OpenMode.Append)
        My.Computer.FileSystem.WriteAllText(TreePath & "\Transactions.txt", BigString, True)
        If File.Exists(TreePath & "\Transactions.csv") Then
            My.Computer.FileSystem.WriteAllText(TreePath & "\Transactions.csv", BigString, True)
        End If
        printstring = "Thank you!" & vbNewLine _
            & "" & vbNewLine _
            & TransDate & vbNewLine _
            & "" & vbNewLine _
            & TruckingStr & vbNewLine _
            & "" & vbNewLine _
            & WhatStr & vbNewLine _
            & HowManyStr & vbNewLine _
            & CropNumStr & vbNewLine _
            & "FROM: " & FromStr & vbNewLine _
            & "" & vbNewLine _
            & "TO:   " & ToStr & vbNewLine _
            & vbNewLine _
            & "Gross:  " & gross & vbNewLine _
            & "Tare:   " & tare & vbNewLine _
            & "------------------" & vbNewLine _
            & "Net:    " & net & vbNewLine


        If File.Exists(TreePath & "\Config Files\AskPrinter.txt") Then
            keeptop = False
            Dim PrintDialog1 As New PrintDialog
            PrintDialog1.ShowDialog()
            pd.PrinterSettings.PrinterName = PrintDialog1.PrinterSettings.PrinterName
            keeptop = True
        End If

        Try
            streamToPrint = New StreamReader(TreePath & "\Config Files\Print.txt")

            Try
                printFont = New Font("Arial", 12)
                'pd.PrinterSettings.PaperSizes.
                'Dim pd As New PrintDocument()
                AddHandler pd.PrintPage, AddressOf Me.printtext
                pd.Print()
            Finally
                streamToPrint.Close()
            End Try
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        pd.PrinterSettings.PrinterName = pd.PrinterSettings.PrinterName
        With Dialog2
            .Tare.Text = tare
            .Gross.Text = gross
            .Net.Text = net
            .DonePanel.Visible = True
            .TarePanel.Visible = False
            .GrossPanel.Visible = False
            .ShowDialog()
        End With

        RefreshAll()

    End Sub
    Private Sub printtext(ByVal sender As Object, ByVal ev As PrintPageEventArgs)
        ev.Graphics.DrawString(printstring, New Font("arial", 11, FontStyle.Regular), Brushes.Black, 120, 120)
        ev.HasMorePages = False
    End Sub

    Private Sub partialfull()
        Dim BigString As String
        Dim gross As Long = 0
        Dim net As Long = 0
        Dim tare As Long = 0
        Dim TransDate As String
        Dim WhatStr As String = txtWhat.Text
        Dim TruckingStr As String = txtTruck.Text
        Dim FromStr As String = txtFrom.Text
        Dim ToStr As String = TxtTo.Text
        Dim Howmanystr As String = ""
        Dim CropNumStr As String = ""

        If txtHowMany.Visible = True Then
            Howmanystr = txtHowMany.Text
        End If

        If txtCropNum.Visible = True Then
            CropNumStr = txtCropNum.Text
        End If

        gross = Weight

        TransDate = Date.Now.ToString

        BigString = vbNewLine & txtTruck.Text & "," & "Full" & "," & txtWhat.Text & "," & TransDate & "," & Str(gross) & "," & Str(tare) & "," & Str(net) & "," & FromStr & "," & ToStr & "," & Howmanystr & "," & CropNumStr
        'FileOpen(1, TreePath & "\Transactions.txt", Microsoft.VisualBasic.OpenMode.Append)
        My.Computer.FileSystem.WriteAllText(TreePath & "\Transactions.txt", BigString, True)
        If File.Exists(TreePath & "\Transactions.csv") Then
            My.Computer.FileSystem.WriteAllText(TreePath & "\Transactions.csv", BigString, True)
        End If
        With Dialog2
            .GrossOnly.Text = gross
            .GrossPanel.Visible = True
            .TarePanel.Visible = False
            .DonePanel.Visible = False
            .ShowDialog()
        End With
        RefreshAll()

    End Sub
    Private Sub RefreshAll()
        RefreshTo()
        RefreshTrucking()
        RefreshFrom()
        RefreshWhat()
        RefreshPrefs(WhatPrefs)
        RefreshPrefs(TruckPrefs)
        GBoxWhat.Visible = False
        GBoxFrom.Visible = False
        GBoxTo.Visible = False
        WeighFull.Enabled = False
        WeightEmpty.Enabled = False
        txtHowMany.Text = "0"
        txtCropNum.Text = "0"
        If Dialog1.Visible = True Then
            Dialog1.DialogResult = vbCancel
            Dialog1.Close()
        End If
        If Dialog2.Visible = True Then
            Dialog2.DialogResult = vbCancel
            Dialog2.Close()
        End If

    End Sub

    Private Sub completeEmpty(ByVal cur() As String)
        Dim BigString As String
        Dim gross As Long = 0
        Dim net As Long = 0
        Dim tare As Long = 0
        Dim printFont As Font
        Dim streamToPrint As StreamReader
        Dim TransDate As String
        Dim WhatStr As String = txtWhat.Text
        Dim TruckingStr As String = txtTruck.Text
        Dim FromStr As String = txtFrom.Text
        Dim ToStr As String = TxtTo.Text
        Dim Howmanystr As String = ""
        Dim CropNumStr As String = ""

        If cur.Length > 9 Then
            Howmanystr = cur(9)
        End If

        If cur.Length > 10 Then
            CropNumStr = cur(10)
        End If

        If txtHowMany.Visible = True Then
            Howmanystr = txtHowMany.Text
        End If

        If txtCropNum.Visible = True Then
            CropNumStr = txtCropNum.Text
        End If

        tare = Weight
        gross = (cur(4))
        If gross < tare Then
            tare = gross
            gross = Val(cur(4))
        End If
        net = gross - tare

        If WhatStr.Count > 0 And Microsoft.VisualBasic.Right(WhatStr, 1) <> "\" Then
            WhatStr = WhatStr
        Else
            WhatStr = cur(2)
        End If

        TransDate = Date.Now.ToString
        BigString = vbNewLine & cur(0) & "," & "DoneEmpty" & "," & WhatStr & "," & TransDate & "," & Str(gross) & "," & Str(tare) & "," & Str(net) & "," & FromStr & "," & ToStr & "," & Howmanystr & "," & CropNumStr
        'FileOpen(1, TreePath & "\Transactions.txt", Microsoft.VisualBasic.OpenMode.Append)
        My.Computer.FileSystem.WriteAllText(TreePath & "\Transactions.txt", BigString, True)
        If File.Exists(TreePath & "\Transactions.csv") Then
            My.Computer.FileSystem.WriteAllText(TreePath & "\Transactions.csv", BigString, True)
        End If

        printstring = "Thank you!" & vbNewLine _
              & "" & vbNewLine _
              & TransDate & vbNewLine _
              & "" & vbNewLine _
              & TruckingStr & vbNewLine _
              & "" & vbNewLine _
              & WhatStr & vbNewLine _
              & Howmanystr & vbNewLine _
              & CropNumStr & vbNewLine _
              & "FROM: " & FromStr & vbNewLine _
              & "" & vbNewLine _
              & "TO:   " & ToStr & vbNewLine _
              & vbNewLine _
              & "Gross:  " & gross & vbNewLine _
              & "Tare:   " & tare & vbNewLine _
              & "--------------------------" & vbNewLine _
              & "Net:    " & net & vbNewLine

        If File.Exists(TreePath & "\Config Files\AskPrinter.txt") Then
            keeptop = False
            Dim PrintDialog1 As New PrintDialog
            PrintDialog1.ShowDialog()
            pd.PrinterSettings.PrinterName = PrintDialog1.PrinterSettings.PrinterName
            keeptop = True
        End If

        Try
            streamToPrint = New StreamReader(TreePath & "\Config Files\Print.txt")

            Try
                printFont = New Font("Arial", 12)
                pd.DefaultPageSettings.Margins.Left = 0.5
                pd.DefaultPageSettings.Margins.Top = 0.5
                'Dim pd As New PrintDocument()
                AddHandler pd.PrintPage, AddressOf Me.printtext
                pd.Print()
            Finally
                streamToPrint.Close()
            End Try
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        With Dialog2
            .Tare.Text = tare
            .Gross.Text = gross
            .Net.Text = net
            .DonePanel.Visible = True
            .TarePanel.Visible = False
            .GrossPanel.Visible = False
            .ShowDialog()
        End With


        RefreshAll()

    End Sub
    Private Sub partialEmpty()
        Dim BigString As String
        Dim gross As Long = 0
        Dim net As Long = 0
        Dim tare As Long = 0
        Dim TransDate As String
        Dim WhatStr As String = txtWhat.Text
        Dim TruckingStr As String = txtTruck.Text
        Dim FromStr As String = txtFrom.Text
        Dim ToStr As String = TxtTo.Text
        Dim HowManyStr As String = ""
        Dim CropNumStr As String = ""

        If GboxHowMany.Visible = True Then
            HowManyStr = txtHowMany.Text
        End If

        If GboxCropNum.Visible = True Then
            CropNumStr = txtCropNum.Text
        End If

        tare = Weight
        TransDate = Date.Now.ToString
        BigString = vbNewLine & txtTruck.Text & "," & "Empty" & "," & txtWhat.Text & "," & TransDate & "," & Str(gross) & "," & Str(tare) & "," & Str(net) & "," & FromStr & "," & ToStr & "," & HowManyStr & "," & CropNumStr
        'FileOpen(1, TreePath & "\Transactions.txt", Microsoft.VisualBasic.OpenMode.Append)
        My.Computer.FileSystem.WriteAllText(TreePath & "\Transactions.txt", BigString, True)
        If File.Exists(TreePath & "\Transactions.csv") Then
            My.Computer.FileSystem.WriteAllText(TreePath & "\Transactions.csv", BigString, True)
        End If
        With Dialog2
            .TareOnly.Text = tare
            .TarePanel.Visible = True
            .DonePanel.Visible = False
            .GrossPanel.Visible = False
            .ShowDialog()
        End With
        RefreshAll()

    End Sub

    Private Sub WeightEmpty_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles WeightEmpty.Click
        If Weight = 0 Then
            MessageBox.Show("The scale is reading zero, you cannot weigh", "Nothing on scale")
            Exit Sub
        End If
        If Microsoft.VisualBasic.Right(txtTruck.Text, 1) = "\" Or txtTruck.Text = "" Then
            MessageBox.Show("You must at least enter your trucking information to weigh empty")
            Exit Sub
        End If
        Dim j As Integer
        Dim temp() As String
        Buffer = File.ReadAllLines(TreePath & "\Transactions.txt")
        For i = 1 To Buffer.Length - 1
            j = Buffer.Length - i

            If Buffer(j).Contains(txtTruck.Text) Then
                temp = Split(Buffer(j), ",")
                If temp(0) = txtTruck.Text Then
                    If temp(1) = "Full" Then
                        Current = temp
                        completeEmpty(Current)
                        Exit Sub
                    Else
                        partialEmpty()
                        Exit Sub
                    End If
                End If
            End If
        Next
        partialEmpty()
    End Sub

    Private Sub txtTruck_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTruck.TextChanged
        If txtTruck.Text.Length > 0 Then
            AddTruck.Visible = True
            If Microsoft.VisualBasic.Right(txtTruck.Text, 1) <> "\" Then
                txtTruck.BackColor = Color.LightGreen
                GBoxWhat.Visible = True
                WeightEmpty.Enabled = True

            Else
                txtTruck.BackColor = Color.Red
            End If
        Else
            AddTruck.Visible = False
        End If
        adjustfont(txtTruck)
    End Sub
    Private Sub adjustfont(ByRef box As TextBox)
        Dim temp As Integer
        If Len(box.Text) > 10 Then
            temp = Int(340 / (Len(box.Text) + 1))
        Else
            temp = 30
        End If
        Dim NF As Font = New Font(Me.Font.FontFamily, temp)
        box.Font = NF
    End Sub
    Private Sub txtWhat_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWhat.TextChanged
        If txtWhat.Text.Length > 0 And Microsoft.VisualBasic.Right(txtWhat.Text, 1) <> "\" Then
            'txtWhat.BackColor = Color.LightGreen
            WeighFull.Enabled = True
        Else
            'txtWhat.BackColor = Color.Red
        End If
        adjustfont(txtWhat)
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            If TextBox2.Text = "" Then
                Weight = 0
            Else
                Weight = TextBox2.Text
            End If

        Else
            If TextBox1.Text = "" Then
                Weight = 0
            Else
                Weight = TextBox1.Text
            End If
        End If


    End Sub

    Private Sub TextBox2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox2.Click
        Dialog1.TextBox1.Text = ""
        Dialog1.Label1.Text = "Enter the Weight you wish to simulate"
        Dialog1.ShowDialog()
        If Dialog1.DialogResult = DialogResult.OK Then
            TextBox2.Text = Val(Dialog1.TextBox1.Text)
        End If
        Dialog1.Label1.Text = ""
        Dialog1.TextBox1.Text = ""
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If CheckBox1.Checked = True Then Weight = TextBox2.Text
    End Sub


    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        'connectserial()
    End Sub
    'Private Sub connectserial()
    '    If My.Computer.Ports.SerialPortNames.Count = 0 Then Exit Sub
    '    SerialPort1.PortName = "COM1"        'Set SerialPort1 to the selected COM port at startup
    '    SerialPort1.BaudRate = 9600
    '    SerialPort1.Parity = IO.Ports.Parity.None
    '    SerialPort1.StopBits = IO.Ports.StopBits.One
    '    SerialPort1.DataBits = 8            'Open our serial port
    '    SerialPort1.Open()
    'End Sub
    'Private Sub SerialPort1_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs)
    '    Dim temp As String
    '    temp = Val(SerialPort1.ReadExisting)
    '    'MsgBox(temp)

    '    If Weight > 1000 Then
    '        If Microsoft.VisualBasic.Right(Weight, temp.Length) = temp Then Exit Sub
    '    End If

    '    TextBox1.Text = temp
    '    If CheckBox1.Checked = False Then
    '        If TextBox1.Text = "Error: Serial Port read timed out." Then
    '            Weight = 0
    '        Else
    '            If Val(temp) > 40 Then
    '                Weight = Val(temp)
    '                ZeroTimer.Enabled = False
    '                'lblzerotimer.Visible = False
    '            Else
    '                lblzerotimer.Visible = True
    '                If ZeroTimer.Enabled = False Then
    '                    ZeroTimer.Enabled = True
    '                End If
    '            End If
    '        End If

    '    Else
    '        Weight = Val(TextBox2.Text)
    '    End If

    '    If Weight < 80 Then
    '        TransTimeOut.Enabled = True
    '    Else
    '        TransTimeOut.Enabled = False
    '    End If


    '    'If temp = 0 Then
    '    '    If Weight < 1000 Then
    '    '        Weight = temp
    '    '    End If
    '    'Else
    '    '    Weight = temp
    '    'End If
    'End Sub
    Private Sub ListBoxWhat_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListBoxWhat.MouseUp
        WhatOKSub()
    End Sub

    Private Sub ListBox1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.Click

    End Sub

    Private Sub ListBox1_ClientSizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.ClientSizeChanged

    End Sub

    Private Sub ListBox1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListBox1.MouseUp
        TruckingOkSub()
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        If Button8.Text = "Hide Options" Then
            Panel1.Visible = True
            GBComOpts.Visible = False
            Button8.Text = "Show Options"
        Else
            Dialog1.TextBox1.Text = ""
            Dialog1.Label1.Text = "Enter the password"
            Dialog1.ShowDialog()
            If Dialog1.DialogResult = DialogResult.OK Then
                If Dialog1.TextBox1.Text = "1234" Then
                    Panel1.Visible = False
                    GBComOpts.Visible = True
                End If
            End If
            Dialog1.Label1.Text = ""
            Button8.Text = "Hide Options"
        End If

    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        End
    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GBoxTrucking.Enter

    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Panel1.BringToFront()
        FindSourceFiles()
        RefreshAll()
        'connectserial()
    End Sub

    Private Sub ListBoxTo_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListBoxTo.MouseUp
        ToOKSub()
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpFrom.Click
        'ListBoxUp(ListBoxFrom)
        GuideMe()
    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpTo.Click
        'ListBoxUp(ListBoxTo)
        GuideMe()
    End Sub

    Private Sub Button24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownTo.Click
        'ListBoxDown(ListBoxTo)
        GuideMe()
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownFrom.Click
        'ListBoxDown(ListBoxFrom)
        GuideMe()
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FromOK.Click
        FromOKSub()
    End Sub
    Private Sub FromOKSub()
        ClickOK(TreeViewFrom, ListViewFrom, ListBoxFrom, txtFrom)
        If txtFrom.Text.Length > 0 And Microsoft.VisualBasic.Right(txtFrom.Text, 1) <> "\" Then
            ChangingFrom = False
        End If
        GuideMe()
    End Sub
    Private Sub Button27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToOK.Click
        ToOKSub()
    End Sub
    Private Sub ToOKSub()
        ClickOK(TreeViewTo, ListViewTo, ListBoxTo, TxtTo)
        If TxtTo.Text.Length > 0 And Microsoft.VisualBasic.Right(TxtTo.Text, 1) <> "\" Then
            ChangingTo = False
        End If
        GuideMe()
    End Sub
    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        ClickBack(TreeViewFrom, ListViewFrom, ListBoxFrom, txtFrom)
    End Sub

    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button25.Click
        ClickBack(TreeViewTo, ListViewTo, ListBoxTo, TxtTo)
        GuideMe()
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        Dim Created As String
        Created = AddFolder("Add a new Group of Places", FromPath, TreeViewFrom)
        If Created <> "" Then
            RefreshFrom()
            findstuff(Created, ListBoxFrom, FromOK)
        End If

    End Sub

    Private Sub Button28_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button28.Click
        Dim Created As String
        Created = AddFolder("Add a new Group of Places", ToPath, TreeViewTo)
        If Created <> "" Then
            RefreshTo()
            findstuff(Created, ListBoxTo, ToOK)
        End If
    End Sub

    Private Sub txtFrom_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFrom.TextChanged
        If txtFrom.Text.Length > 0 And Microsoft.VisualBasic.Right(txtFrom.Text, 1) <> "\" Then
            'txtFrom.BackColor = Color.LightGreen
        Else
            'txtFrom.BackColor = Color.Orange
        End If
        If txtFrom.Text = "" Then txtFrom.BackColor = Color.White
        adjustfont(txtFrom)
    End Sub

    Private Sub TxtTo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtTo.TextChanged
        If TxtTo.Text.Length > 0 And Microsoft.VisualBasic.Right(TxtTo.Text, 1) <> "\" Then
            'TxtTo.BackColor = Color.LightGreen
        Else
            'TxtTo.BackColor = Color.Orange
        End If
        If TxtTo.Text = "" Then TxtTo.BackColor = Color.White
        adjustfont(TxtTo)
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddFromPlace.Click
        Dim Created As String
        Created = addfile("Add a New Place to the list", FromPath, "\Default Files\defaultFrom.txt", TreeViewFrom)
        If Created <> "" Then
            RefreshFrom()
            findstuff(Created, ListBoxFrom, FromOK)
        End If
    End Sub

    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddToPlace.Click
        Dim Created As String
        Created = addfile("Add a New Place to the list", ToPath, "\Default Files\defaultTo.txt", TreeViewTo)
        If Created <> "" Then
            RefreshTo()
            findstuff(Created, ListBoxTo, ToOK)
        End If
    End Sub


    Private Sub ListBoxFrom_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListBoxFrom.MouseUp
        FromOKSub()
    End Sub


    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KeepOnTop.Tick
        If KeepTopChk.Checked = True Then
            Me.TopMost = True
        Else
            Me.TopMost = False
        End If

    End Sub


    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TransTimeOut.Tick
        RefreshAll()
        ScreenSaverOn("Touch the Screen to Start")
    End Sub

    Private Sub Button4_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TruckingDown.MouseDown
        ScrollCount = 1
        ScrollDnTimer.Enabled = True
        ScrollDnTimer.Interval = 1
        lbdn = ListBox1
    End Sub

    Private Sub ScrollTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScrollDnTimer.Tick
        If ScrollCount = 1 Then
            ListBoxDown(lbdn)
            ScrollDnTimer.Interval = 400
        ElseIf ScrollCount < 4 Then
            ListBoxDown(lbdn)
            ScrollDnTimer.Interval = ScrollDnTimer.Interval - 100
        ElseIf ScrollCount > 3 Then
            ListBoxDown(lbdn)
            ScrollDnTimer.Interval = 100
        End If
        ScrollCount = ScrollCount + 1
    End Sub

    Private Sub Button4_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TruckingDown.MouseLeave
        lbdn = New ListBox
        ScrollDnTimer.Enabled = False
    End Sub

    Private Sub Button4_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TruckingDown.MouseUp
        lbdn = New ListBox
        ScrollDnTimer.Enabled = False
    End Sub

    Private Sub ScrollUpTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScrollUpTimer.Tick
        If ScrollCount = 1 Then
            ListBoxUp(lbup)
            ScrollUpTimer.Interval = 400
        ElseIf ScrollCount < 4 Then
            ListBoxUp(lbup)
            ScrollUpTimer.Interval = ScrollUpTimer.Interval - 100
        ElseIf ScrollCount > 3 Then
            ListBoxUp(lbup)
            ScrollUpTimer.Interval = 100
        End If
        ScrollCount = ScrollCount + 1
    End Sub

    Private Sub DownWhat_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DownWhat.MouseDown
        ScrollCount = 1
        ScrollDnTimer.Enabled = True
        ScrollDnTimer.Interval = 1
        lbdn = ListBoxWhat
    End Sub

    Private Sub DownWhat_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles DownWhat.MouseLeave
        ScrollDnTimer.Enabled = False
    End Sub

    Private Sub DownWhat_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DownWhat.MouseUp
        ScrollDnTimer.Enabled = False
        lbdn = New ListBox
    End Sub

    Private Sub Button3_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button3.MouseDown
        ScrollCount = 1
        ScrollUpTimer.Enabled = True
        ScrollUpTimer.Interval = 1
        lbup = ListBox1
    End Sub

    Private Sub Button3_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.MouseLeave

        ScrollUpTimer.Enabled = False

    End Sub

    Private Sub Button3_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button3.MouseUp
        ScrollUpTimer.Enabled = False
    End Sub

    Private Sub TruckingDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TruckingDown.Click
        GuideMe()
    End Sub

    Private Sub GuideMe()
        Dim ButtonList(11) As Button

        ButtonList(0) = TruckingDown
        ButtonList(1) = TruckingOK
        ButtonList(2) = AddTruck
        ButtonList(3) = DownWhat
        ButtonList(4) = WhatOK
        ButtonList(5) = AddCommodity
        ButtonList(6) = DownFrom
        ButtonList(7) = FromOK
        ButtonList(8) = AddFromPlace
        ButtonList(9) = DownTo
        ButtonList(10) = ToOK
        ButtonList(11) = AddToPlace

        Showaddfolderbuttons = chkShowAddButtons.Checked

        AddCompany.Visible = Showaddfolderbuttons
        AddWhatCategory.Visible = Showaddfolderbuttons
        AddFromPlace.Visible = Showaddfolderbuttons
        AddToPlace.Visible = Showaddfolderbuttons

        Loadprefs(WhatPath & "\" & txtWhat.Text & ".txt", WhatPrefs)
        Loadprefs(TruckingPath & "\" & txtTruck.Text & ".txt", TruckPrefs)

        If WhatPrefs.RequireTo = False Then
            GBoxTo.Visible = False
        End If

        If WhatPrefs.RequireFrom = False Then
            GBoxFrom.Visible = False
        End If

        For i = 0 To ButtonList.Length - 1
            ButtonList(i).BackColor = TruckingBack.BackColor
        Next

        If txtTruck.Text = "" Then
            txtTruck.BackColor = Color.Red
            TruckPan.Visible = False
            AddTruck.Visible = False
            WhatPan.Visible = False
            GBoxWhat.Visible = False
            GBoxFrom.Visible = False
            GBoxTo.Visible = False
            If ListBox1.SelectedIndex = -1 Then
                TruckingDown.BackColor = Color.LimeGreen
            Else
                TruckingOK.BackColor = Color.LimeGreen
            End If
        ElseIf Microsoft.VisualBasic.Right(txtTruck.Text, 1) = "\" Then
            txtTruck.BackColor = Color.Red
            ' GBoxWhat.Visible = False
            AddTruck.Visible = True
            TruckPan.Visible = False
            If ListBox1.SelectedIndex = -1 Then
                If ListBox1.Items.Count = 0 Then
                    AddTruck.BackColor = Color.LimeGreen
                Else
                    TruckingDown.BackColor = Color.LimeGreen
                End If
            Else
                TruckingOK.BackColor = Color.LimeGreen
            End If
        ElseIf Microsoft.VisualBasic.Right(txtTruck.Text, 1) <> "\" Then
            txtTruck.BackColor = Color.LightGreen
            AddTruck.Visible = True
            If Microsoft.VisualBasic.Right(txtTruck.Text, ListBox1.SelectedItem.ToString.Count) = ListBox1.SelectedItem.ToString Then
                GBoxWhat.Visible = True
                If ChangingTruck = False Then
                    TruckPan.Visible = True
                    TruckPan.BringToFront()
                Else
                    TruckPan.Visible = False
                End If
            Else
                'GBoxWhat.Visible = False
                TruckingOK.BackColor = Color.LimeGreen
            End If
        End If

        If txtWhat.Text = "" Then
            GboxHowMany.Visible = False
            GboxCropNum.Visible = False
            txtWhat.BackColor = Color.Red
            AddCommodity.Visible = False
            If ListBoxWhat.SelectedIndex = -1 Then
                DownWhat.BackColor = Color.LimeGreen
            Else
                WhatOK.BackColor = Color.LimeGreen
            End If
        ElseIf Microsoft.VisualBasic.Right(txtWhat.Text, 1) = "\" Then
            GboxHowMany.Visible = False
            GboxCropNum.Visible = False
            txtWhat.BackColor = Color.Red
            AddCommodity.Visible = True
            If ListBoxWhat.SelectedIndex = -1 Then
                If ListBoxWhat.Items.Count = 0 Then
                    AddCommodity.BackColor = Color.LimeGreen
                Else
                    DownWhat.BackColor = Color.LimeGreen
                End If

            Else
                WhatOK.BackColor = Color.LimeGreen
            End If
        ElseIf Microsoft.VisualBasic.Right(txtWhat.Text, 1) <> "\" Then
            Loadprefs(WhatPath & "\" & txtWhat.Text & ".txt", WhatPrefs)
            GboxHowMany.Visible = WhatPrefs.RequireHowMany
            GboxCropNum.Visible = WhatPrefs.RequireCropNum

            ChkReqFrom.Checked = WhatPrefs.RequireFrom
            ChkReqTo.Checked = WhatPrefs.RequireTo
            ChkReqHowMany.Checked = WhatPrefs.RequireHowMany
            ChkReqCropNum.Checked = WhatPrefs.RequireCropNum

            If txtHowMany.Text = "0" Then txtHowMany.Text = WhatPrefs.StartHowMany
            If txtCropNum.Text = "0" Then txtCropNum.Text = WhatPrefs.StartCropNum
            txtWhat.BackColor = Color.LightGreen
            AddCommodity.Visible = True
            If Microsoft.VisualBasic.Right(txtWhat.Text, ListBoxWhat.SelectedItem.ToString.Count) = ListBoxWhat.SelectedItem.ToString Then
                If ChangingWhat = False Then
                    'If askfromandto = True Then
                    '    GBoxFrom.Visible = True
                    'End If
                    WhatPan.Visible = True
                    WhatPan.BringToFront()
                Else
                    WhatPan.Visible = False
                End If
            Else
                WhatOK.BackColor = Color.LimeGreen
            End If
        End If


        If WhatPrefs.RequireFrom = True Then
            GBoxFrom.Visible = True
            If txtFrom.Text = "" Then
                txtFrom.BackColor = Color.Orange
                AddFromPlace.Visible = False
                FromPan.Visible = False
                If ListBoxFrom.SelectedIndex = -1 Then
                    DownFrom.BackColor = Color.LimeGreen
                Else
                    FromOK.BackColor = Color.LimeGreen
                End If
            ElseIf Microsoft.VisualBasic.Right(txtFrom.Text, 1) = "\" Then
                txtFrom.BackColor = Color.Orange
                FromPan.Visible = False
                AddFromPlace.Visible = True
                If ListBoxFrom.SelectedIndex = -1 Then
                    If ListBoxFrom.Items.Count = 0 Then
                        AddFromPlace.BackColor = Color.LimeGreen
                    Else
                        DownFrom.BackColor = Color.LimeGreen
                    End If

                Else
                    FromOK.BackColor = Color.LimeGreen
                End If
            ElseIf Microsoft.VisualBasic.Right(txtFrom.Text, 1) <> "\" Then
                txtFrom.BackColor = Color.LightGreen
                AddFromPlace.Visible = True
                If Microsoft.VisualBasic.Right(txtFrom.Text, ListBoxFrom.SelectedItem.ToString.Count) = ListBoxFrom.SelectedItem.ToString Then
                    If ChangingFrom = False Then
                        GBoxFrom.Visible = True
                        FromPan.Visible = True
                        FromPan.BringToFront()
                    Else
                        FromPan.Visible = False
                    End If
                Else
                    FromOK.BackColor = Color.LimeGreen
                End If
            End If
        Else
            GBoxFrom.Visible = False
        End If

        If WhatPrefs.RequireTo = True Then
            GBoxTo.Visible = True
            If TxtTo.Text = "" Then
                TxtTo.BackColor = Color.Orange
                ToPan.Visible = False
                AddToPlace.Visible = False
                If ListBoxTo.SelectedIndex = -1 Then
                    DownTo.BackColor = Color.LimeGreen
                Else
                    ToOK.BackColor = Color.LimeGreen
                End If
            ElseIf Microsoft.VisualBasic.Right(TxtTo.Text, 1) = "\" Then
                TxtTo.BackColor = Color.Orange
                ToPan.Visible = False
                AddToPlace.Visible = True
                If ListBoxTo.SelectedIndex = -1 Then
                    If ListBoxTo.Items.Count = 0 Then
                        AddToPlace.BackColor = Color.LimeGreen
                    Else
                        DownTo.BackColor = Color.LimeGreen
                    End If
                Else
                    ToOK.BackColor = Color.LimeGreen
                End If
            ElseIf Microsoft.VisualBasic.Right(TxtTo.Text, 1) <> "\" Then
                TxtTo.BackColor = Color.LightGreen
                AddToPlace.Visible = True
                If Microsoft.VisualBasic.Right(TxtTo.Text, ListBoxTo.SelectedItem.ToString.Count) = ListBoxTo.SelectedItem.ToString Then
                    If ChangingTo = False Then
                        GBoxTo.Visible = True
                        ToPan.Visible = True
                        ToPan.BringToFront()
                    Else
                        ToPan.Visible = False
                    End If
                Else
                    ToOK.BackColor = Color.LimeGreen
                End If
            End If
        End If
    End Sub

    Private Sub GuideTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GuideTimer.Tick
        GuideMe()
    End Sub
    Private Sub SeekNode(ByRef theTree As TreeView, ByVal seekpath As String)


    End Sub

    Private Sub TruckChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TruckChange.Click
        ChangingTruck = True
    End Sub

    Private Sub ChangeWhat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeWhat.Click
        ChangingWhat = True
    End Sub

    Private Sub ChangeFrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeFrom.Click
        ChangingFrom = True
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        ChangingTo = True
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

        GuideMe()
    End Sub

    Private Sub ListBoxWhat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBoxWhat.SelectedIndexChanged
        GuideMe()
    End Sub

    Private Sub Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RefreshAll()
    End Sub

    Private Sub GBoxWhat_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GBoxWhat.Enter

    End Sub
    Private Sub Loadprefs(ByVal fpath As String, ByRef myprefs As Prefs)
        Dim myfile() As String
        Dim prefline() As String
        myprefs.RequireFrom = False
        myprefs.RequireHowMany = False
        myprefs.RequireCropNum = False
        myprefs.RequireTo = False
        myprefs.RetainTare = False
        myprefs.StartFrom = ""
        myprefs.StartTo = ""
        myprefs.StartHowMany = 0
        myprefs.StartCropNum = 0

        If File.Exists(fpath) = False Then Exit Sub

        myfile = File.ReadAllLines(fpath)

        For i = 0 To myfile.Count - 1
            prefline = Split(myfile(i), " ")


            Select Case StrConv(prefline(0), vbProperCase)
                Case "Retaintare"
                    If prefline.Contains("true") Then
                        myprefs.RetainTare = True
                    End If
                Case "Requirefrom"
                    If prefline.Contains("true") Then
                        myprefs.RequireFrom = True
                    End If
                Case "Startfrom"
                    myprefs.StartFrom = prefline(1)
                Case "Requireto"
                    If prefline.Contains("true") Then
                        myprefs.RequireTo = True
                    End If
                Case "Startto"
                    myprefs.StartTo = prefline(1)
                Case "Requirehowmany"
                    If prefline.Contains("true") Then
                        myprefs.RequireHowMany = True
                    End If
                Case "Starthowmany"
                    myprefs.StartHowMany = Val(prefline(1))
                Case "Requirecropnum"
                    If prefline.Contains("true") Then
                        myprefs.RequireCropNum = True
                    End If
                Case "Startcropnum"
                    myprefs.StartCropNum = Val(prefline(1))
            End Select
        Next
    End Sub

    Private Sub WritePrefs(ByVal fpath As String, ByVal PrefName As String, ByVal PrefVal As String)
        Dim myfile() As String
        Dim prefline() As String
        Dim found As Boolean = False
        If File.Exists(fpath) = False Then Exit Sub

        myfile = File.ReadAllLines(fpath)


        For i = 0 To myfile.Count - 1
            prefline = Split(myfile(i), " ")
            If StrConv(prefline(0), vbProperCase) = StrConv(PrefName, vbProperCase) Then
                Dim mynewfile(myfile.Count - 1) As String
                For j = 0 To myfile.Count - 1
                    mynewfile(j) = myfile(j)
                Next
                mynewfile(i) = PrefName & " " & PrefVal
                File.WriteAllLines(fpath, mynewfile)
                found = True
            End If

        Next
        If found = False Then
            Dim mynewfile(myfile.Count) As String
            For i = 0 To myfile.Count - 1
                mynewfile(i) = myfile(i)
            Next
            mynewfile(myfile.Count) = PrefName & " " & PrefVal
            File.WriteAllLines(fpath, mynewfile)
            found = True
        End If
    End Sub

    Private Sub KeepTopChk_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KeepTopChk.CheckedChanged

    End Sub

    Private Sub StillZero_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StillZero.Tick
        If Weight < 1000 Then
            TransTimeOut.Enabled = True
            CheckVersion()
        Else
            ScreenSaverOff()
            TransTimeOut.Enabled = False
        End If
    End Sub



    Private Sub UpHowMany_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpHowMany.Click
        txtHowMany.Text = Val(txtHowMany.Text) + 1
    End Sub

    Private Sub DownHowMany_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownHowMany.Click
        txtHowMany.Text = Val(txtHowMany.Text) - 1
    End Sub
    Private Sub ScreenSaverOn(ByVal message As String)
        txtScreenSaver.BringToFront()
        ScreenSaver.Enabled = True
        Cursor.Position = Me.Location
        txtScreenSaver.Visible = True
        txtScreenSaver.Left = 0
        txtScreenSaver.Top = 0
        txtScreenSaver.Width = Me.Width
        txtScreenSaver.Height = Me.Height
        txtScreenSaver.Tag = message

    End Sub
    Private Sub ScreenSaverOff()
        TransTimeOut.Enabled = False
        TransTimeOut.Enabled = True
        ScreenSaver.Enabled = False
        txtScreenSaver.Visible = False
    End Sub

    Private Sub ScreenSaver_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScreenSaver.Tick
        txtScreenSaver.Text = vbNewLine & txtScreenSaver.Text
        Dim y = txtScreenSaver.Text.Length - txtScreenSaver.Tag.ToString.Length
        Dim x = txtScreenSaver.Height / txtScreenSaver.Font.Size

        Dim pos = Int(Rnd() * 4)



        If y - 2 > x Then
            Dim fnt As Font
            Dim size As Integer = Int(Rnd() * 40) + 30
            fnt = txtScreenSaver.Font
            txtScreenSaver.Font = New Font(fnt.Name, size, FontStyle.Regular)
            ScreenSaver.Interval = Int(Rnd() * 1000) + 3000
            txtScreenSaver.Text = txtScreenSaver.Tag

            Select Case pos
                Case 1
                    txtScreenSaver.TextAlign = HorizontalAlignment.Left
                Case 2
                    txtScreenSaver.TextAlign = HorizontalAlignment.Center
                Case 3
                    txtScreenSaver.TextAlign = HorizontalAlignment.Right
            End Select

            If txtScreenSaver.BackColor = Color.White Then
                txtScreenSaver.BackColor = Color.Black
                txtScreenSaver.ForeColor = Color.White
            Else
                txtScreenSaver.BackColor = Color.White
                txtScreenSaver.ForeColor = Color.Black
            End If
        End If

    End Sub

    Private Sub txtScreenSaver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScreenSaver.Click
        ScreenSaverOff()
    End Sub

    Private Sub CheckVersion()
        Dim wantvers() As String
        Dim havevers As String
        CheckScaleSource()
        If File.Exists(TreePath & "\Config Files\CurrentVersionPath.txt") = False Then
            Exit Sub
        End If

        wantvers = File.ReadAllLines(TreePath & "\Config Files\CurrentVersionPath.txt")
        For i = 0 To wantvers.Length - 1
            wantvers(i) = StrConv(wantvers(i), vbProperCase)
        Next

        havevers = StrConv(System.Windows.Forms.Application.ExecutablePath.ToString(), vbProperCase)
        If wantvers(0) = havevers Then
        Else
            If File.Exists(wantvers(0)) = True Then
                'close the connection to serial port

                'Kill Scale Source

                'If SerialPort1.IsOpen Then
                '    SerialPort1.Close()
                'End If
                'launch the version we want and close myself
                Shell(wantvers(0))
                End
            End If
        End If

    End Sub

    Private Sub CheckScaleSource()
        Dim p() As Process
        Dim Desired As String
        'Find desired version
        Dim wantvers() As String
        Dim havevers As String

        If File.Exists(TreePath & "\Config Files\CurrentScaleSourcePath.txt") = False Then
            Exit Sub
        End If

        wantvers = File.ReadAllLines(TreePath & "\Config Files\CurrentScaleSourcePath.txt")
        For i = 0 To wantvers.Length - 1
            wantvers(i) = StrConv(wantvers(i), vbProperCase)
        Next
        Desired = Split(wantvers.Last, "\").Last
        Desired = Split(Desired, ".").First

        p = Process.GetProcesses

        For i = 0 To p.Count - 1
            If p(i).ProcessName.Contains("ScaleSource") Then
                If StrConv(p(i).ProcessName, vbProperCase) <> StrConv(Desired, vbProperCase) Then
                    p(i).Kill()
                End If
            End If
        Next

        p = Process.GetProcessesByName(Desired)
        If p.Count = 1 Then Exit Sub
        If File.Exists(wantvers.Last) = False Then Exit Sub

        If p.Count > 1 Then
            For i = 0 To p.Count - 1
                p(i).Kill()
            Next
                Shell(wantvers.Last)
            End If

            If p.Count < 1 Then
                Shell(wantvers.Last)
            End If



            'havevers = StrConv(System.Windows.Forms.Application.ExecutablePath.ToString(), vbProperCase)
            'If wantvers(0) = havevers Then
            'Else
            '    If File.Exists(wantvers(0)) = True Then
            '        'close the connection to serial port

            '        'Kill Scale Source

            '        'If SerialPort1.IsOpen Then
            '        '    SerialPort1.Close()
            '        'End If
            '        'launch the version we want and close myself
            '        Shell(wantvers(0))
            '        End
            '    End If
            'End If

    End Sub
    Private Sub findstuff(ByVal destination As String, ByRef mylist As ListBox, ByRef mybut As Button)
        Dim path() As String = Split(destination, "\")

        For i = 0 To path.Length - 1
            For j = 0 To mylist.Items.Count - 1
                If StrConv(mylist.Items(j), vbProperCase) = StrConv(path(i), vbProperCase) Then
                    mylist.SetSelected(j, True)
                    mybut.PerformClick()
                    Exit For
                End If
            Next
        Next
    End Sub

    Private Sub DownFrom_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DownFrom.MouseDown
        ScrollCount = 1
        ScrollDnTimer.Enabled = True
        ScrollDnTimer.Interval = 1
        lbdn = ListBoxFrom
    End Sub

    Private Sub DownTo_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DownTo.MouseDown
        ScrollCount = 1
        ScrollDnTimer.Enabled = True
        ScrollDnTimer.Interval = 1
        lbdn = ListBoxTo
    End Sub

    Private Sub UpWhat_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UpWhat.MouseDown
        ScrollCount = 1
        ScrollUpTimer.Enabled = True
        ScrollUpTimer.Interval = 1
        lbup = ListBoxWhat
    End Sub

    Private Sub UpFrom_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UpFrom.MouseDown
        ScrollCount = 1
        ScrollUpTimer.Enabled = True
        ScrollUpTimer.Interval = 1
        lbup = ListBoxFrom
    End Sub

    Private Sub UpTo_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UpTo.MouseDown
        ScrollCount = 1
        ScrollUpTimer.Enabled = True
        ScrollUpTimer.Interval = 1
        lbup = ListBoxTo
    End Sub

    Private Sub DownFrom_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles DownFrom.MouseLeave
        ScrollDnTimer.Enabled = False
        'lbdn = New ListBox
    End Sub

    Private Sub DownFrom_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DownFrom.MouseUp
        ScrollDnTimer.Enabled = False
        'lbdn = New ListBox
    End Sub

    Private Sub DownTo_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles DownTo.MouseLeave
        ScrollDnTimer.Enabled = False
        'lbdn = New ListBox
    End Sub

    Private Sub DownTo_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DownTo.MouseUp
        ScrollDnTimer.Enabled = False
        'lbdn = New ListBox
    End Sub

    Private Sub UpWhat_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpWhat.MouseLeave
        ScrollUpTimer.Enabled = False
    End Sub

    Private Sub UpWhat_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UpWhat.MouseUp
        ScrollUpTimer.Enabled = False
    End Sub

    Private Sub UpFrom_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpFrom.MouseLeave
        ScrollUpTimer.Enabled = False
    End Sub

    Private Sub UpFrom_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UpFrom.MouseUp
        ScrollUpTimer.Enabled = False
    End Sub

    Private Sub UpTo_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpTo.MouseLeave
        ScrollUpTimer.Enabled = False
    End Sub

    Private Sub UpTo_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UpTo.MouseUp
        ScrollUpTimer.Enabled = False
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PanInstructions.Paint

    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        PanInstructions.Visible = True
        GBComOpts.Visible = False
    End Sub

    Private Sub ZeroTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZeroTimer.Tick
        Weight = 0
        lblzerotimer.visible = True
    End Sub

    Private Sub Timer1_Tick_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If CheckBox1.Checked = True Then Weight = Val(TextBox2.Text)

        LblWeight.Text = Weight
    End Sub
    Private Sub RefreshPrefs(ByRef mypref As Prefs)
        With mypref
            .RequireFrom = False
            .RequireHowMany = False
            .RequireTo = False
            .RetainTare = False
        End With
    End Sub

    Private Sub ChkReqFrom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkReqFrom.CheckedChanged

    End Sub


    Private Sub ChkReqTo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkReqTo.CheckedChanged

    End Sub

    Private Sub ChkReqFrom_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChkReqFrom.MouseUp
        GuideTimer.Enabled = False
        WritePrefs(WhatPath & "\" & txtWhat.Text & ".txt", "RequireFrom", StrConv(ChkReqFrom.Checked.ToString, vbLowerCase))
        GuideTimer.Enabled = True
    End Sub

    Private Sub ChkReqTo_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChkReqTo.MouseUp
        GuideTimer.Enabled = False
        WritePrefs(WhatPath & "\" & txtWhat.Text & ".txt", "RequireTo", StrConv(ChkReqTo.Checked.ToString, vbLowerCase))
        GuideTimer.Enabled = True
    End Sub

    Private Sub ChkReqHowMany_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkReqHowMany.CheckedChanged

    End Sub

    Private Sub ChkReqHowMany_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChkReqHowMany.MouseUp
        GuideTimer.Enabled = False
        WritePrefs(WhatPath & "\" & txtWhat.Text & ".txt", "RequireHowMany", StrConv(ChkReqHowMany.Checked.ToString, vbLowerCase))
        GuideTimer.Enabled = True
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownCropNum.Click
        txtCropNum.Text = Val(txtCropNum.Text) - 1
    End Sub

    Private Sub UpCropNum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpCropNum.Click
        txtCropNum.Text = Val(txtCropNum.Text) + 1
    End Sub

    Private Sub ChkReqCropNum_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkReqCropNum.CheckedChanged

    End Sub

    Private Sub ChkReqCropNum_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChkReqCropNum.MouseUp
        GuideTimer.Enabled = False
        WritePrefs(WhatPath & "\" & txtWhat.Text & ".txt", "RequireCropNum", StrConv(ChkReqCropNum.Checked.ToString, vbLowerCase))
        GuideTimer.Enabled = True
    End Sub

    Private Sub ChkRetainTare_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkRetainTare.CheckedChanged

    End Sub

    Private Sub ChkRetainTare_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChkRetainTare.MouseUp
        GuideTimer.Enabled = False
        WritePrefs(WhatPath & "\" & txtWhat.Text & ".txt", "RetainTare", StrConv(ChkRetainTare.Checked.ToString, vbLowerCase))
        GuideTimer.Enabled = True
    End Sub

    Private Sub ButStartOver_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButStartOver.Click
        RefreshAll()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If CheckBox1.Checked = False Then Weight = TextBox1.Text
    End Sub
End Class