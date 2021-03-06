﻿Public Class Form1
    Private Async Sub Button1_ClickAsync(sender As Object, e As EventArgs) Handles Button1.Click
        'Dim regs = pCloudSDK.Authentication.Register("user", "pass").Result
        'Console.WriteLine(regs)

        Dim interfaces As New List(Of System.Type)

        Dim AccountClient As New pCloudSDK.AccountClient
        interfaces.AddRange(AccountClient.GetType().GetInterfaces().ToList)

        Dim FilesClient As New pCloudSDK.FilesClient(New List(Of Long))
        interfaces.AddRange(FilesClient.GetType().GetInterfaces().ToList)

        Dim FileClient As New pCloudSDK.FileClient(0)
        interfaces.AddRange(FileClient.GetType().GetInterfaces().ToList)

        Dim FolderClient As New pCloudSDK.FolderClient(0)
        interfaces.AddRange(FolderClient.GetType().GetInterfaces().ToList)

        Dim FoldersClient As New pCloudSDK.FoldersClient(New List(Of Long))
        interfaces.AddRange(FoldersClient.GetType().GetInterfaces().ToList)

        Dim PublicsClient As New pCloudSDK.PublicsClient()
        interfaces.AddRange(PublicsClient.GetType().GetInterfaces().ToList)

        Dim PlaylistsClient As New pCloudSDK.PlaylistsClient(0)
        interfaces.AddRange(PlaylistsClient.GetType().GetInterfaces().ToList)

        For Each iface As Type In interfaces
            Dim methods = iface.GetMethods()

            For Each method As Reflection.MethodInfo In methods
                Dim methodName = method.Name
                DataGridView1.Rows.Add(methodName)
            Next
        Next


        Try

            Dim client As pCloudSDK.IClient = New pCloudSDK.PClient("user", "pass", Nothing)

            'Dim rslt = Await client.Items.Folder(0).Create("tzt")
            'PropertyGrid1.SelectedObject = (rslt)

            'Dim rslt = Await client.Items.Folder(0).ListSubFoldersTree
            'rslt.ForEach(Sub(f) FlowLayoutPanel1.Controls.Add(New PropertyGrid With {.SelectedObject = f, .Width = 250, .Height = 350}))


            'Dim rslt = Await client.Items.Folders(New List(Of Long) From {5597634516}).CompressAsync(0, "testComp.zip")
            'PropertyGrid1.SelectedObject = (rslt)
            'MsgBox(rslt)

            'Dim rslt = Await client.Items.Folder(5597634516).CompressAsync(0, "testComp.zip")
            'PropertyGrid1.SelectedObject = (rslt)
            'MsgBox(rslt)

            'Dim rslt = Await client.Items.Folder(5597634516).DirectZipUrl("fourhour.zip")
            'MsgBox(rslt)

            'Dim rslt = Await client.Items.Folder(5597634516).UploadLocal("C:\Users\PointNine\Downloads\1fichier.SDK.0.1.4.32.zip", pCloudSDK.Utilitiez.SentType.filepath, "1fichier.SDK.0.1.4.32.zip", False, Nothing, Nothing)
            'rslt.ForEach(Sub(f) FlowLayoutPanel1.Controls.Add(New PropertyGrid With {.SelectedObject = f, .Width = 250, .Height = 350}))

            'Dim rslt = Await client.Items.Folder(5597634516).UploadRemoteAsync("http://ipv4.download.thinkbroadband.com/5MB.zip", "5MB.zip")
            'MsgBox(rslt)

            'Dim rslt = Await client.Items.Folder(5597634516).Copy(5596868598, False)
            'PropertyGrid1.SelectedObject = (rslt)

            'Dim rslt = Await client.Items.Folder(5597634516).Public()
            'MsgBox(rslt)

            'Dim rslt = Await client.Publics.List()
            'rslt.ForEach(Sub(f) FlowLayoutPanel1.Controls.Add(New PropertyGrid With {.SelectedObject = f, .Width = 250, .Height = 350}))

            'Dim rslt = Await client.Publics.ListWithoutMetadata()
            'rslt.ForEach(Sub(f) FlowLayoutPanel1.Controls.Add(New PropertyGrid With {.SelectedObject = f, .Width = 250, .Height = 350}))


        Catch ex As pCloudSDK.pCloudException
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Async Sub ToolStripButton1_ClickAsync(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Dim client As pCloudSDK.IClient = New pCloudSDK.PClient("user", "pass", Nothing)
        Dim rslt = Await client.Items.Folder(0).List()
        PropertyGrid1.SelectedObject = rslt
    End Sub

    Private Async Sub ToolStripButton2_ClickAsync(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        Try
            Dim client As pCloudSDK.IClient = New pCloudSDK.PClient("username", "password", New pCloudSDK.ConnectionSettings With {.CloseConnection = True, .TimeOut = TimeSpan.FromMinutes(80), .Proxy = New pCloudSDK.ProxyConfig With {.SetProxy = True, .ProxyIP = "127.0.0.1", .ProxyPort = 80, .ProxyUsername = "user", .ProxyPassword = "123456"}})

            ''Account
            Await client.Account.ChangeMail("newEmail@domain.com")
            Await client.Account.DeleteToken("tokenid")
            Await client.Account.GetCurrentServer
            Await client.Account.GetRegistrationPageUrl
            Await client.Account.IP
            Await client.Account.ListInvites
            Await client.Account.ListTokens
            Await client.Account.ResendActivationMail
            Await client.Account.UserInfo

            ''file
            Await client.Items.File(1234).AudioDirectUrl(128)
            Await client.Items.File(1234).ChangesHistory
            Await client.Items.File(1234).Checksum
            Await client.Items.File(1234).CompressAsync(6789, "myArchive.zip")
            Await client.Items.File(1234).Copy(6789, False, "newname")
            Await client.Items.File(1234).CopyThumbnail(6789, "800x600", pCloudSDK.Utilitiez.ExtEnum.jpeg, True, True, Nothing)
            Await client.Items.File(1234).Delete
            Await client.Items.File(1234).DirectUrl
            Await client.Items.File(1234).Move(6789, Nothing)
            Await client.Items.File(1234).Public(50, (1 * 1024 ^ 2))
            Await client.Items.File(1234).Rename("newname")
            Await client.Items.File(1234).Thumbnail("800x600", pCloudSDK.Utilitiez.ExtEnum.png, True)
            Await client.Items.File(1234).ThumbnailUrl("800x600", pCloudSDK.Utilitiez.ExtEnum.jpeg, True)
            Await client.Items.File(1234).UnCompressAsync(6789, "abcd")
            Await client.Items.File(1234).UnCompressTaskProgress("UnCompressAsync")
            Await client.Items.File(1234).VideoDirectUrl
            Await client.Items.File(1234).VideoResolutionUrls
            Await client.Items.File(1234).VideoToMp3(128)

            ''Files
            Await client.Items.Files(New List(Of Long) From {1234}).CompressAsync(6789, "myArchive.zip")
            Await client.Items.Files(New List(Of Long) From {1234}).Copy(6789, True)
            Dim CancelToken As New Threading.CancellationTokenSource()
            Dim _ReportCls As New Progress(Of pCloudSDK.ReportStatus)(Sub(r)
                                                                          Button1.Text = String.Format("{0}/{1}", (r.BytesTransferred), (r.TotalBytes))
                                                                          Button1.Text = CInt(r.ProgressPercentage)
                                                                          Button1.Text = If(CStr(r.TextStatus) Is Nothing, "Downloading...", CStr(r.TextStatus))
                                                                      End Sub)
            Await client.Items.Files(New List(Of Long) From {1234}).DownloadAsZip("C:\Users", "myArchive.zip", _ReportCls, CancelToken.Token)
            Await client.Items.Files(New List(Of Long) From {1234}).ThumbnailUrl("800x600", pCloudSDK.Utilitiez.ExtEnum.jpeg, True)

            ''Folder
            Await client.Items.Folder(6789).CompressAsync(9876, "myArchive.zip")
            Await client.Items.Folder(6789).CompressTaskProgress("CompressAsync")
            Await client.Items.Folder(6789).Copy(9876, True)
            Await client.Items.Folder(6789).Create("newfolder")
            Await client.Items.Folder(6789).CreateIfNotExists("newfolder")
            Await client.Items.Folder(6789).Delete
            Await client.Items.Folder(6789).DirectZipUrl("myArchive.zip")
            Await client.Items.Folder(6789).List
            Await client.Items.Folder(6789).ListSubFoldersTree
            Await client.Items.Folder(6789).ListWithoutFiles
            Await client.Items.Folder(6789).ListWithoutShared
            Await client.Items.Folder(6789).Move(9876)
            Await client.Items.Folder(6789).Public
            Await client.Items.Folder(6789).Rename("newfoldername")
            Await client.Items.Folder(6789).UploadLocal("C:\Users\fle.rar", pCloudSDK.Utilitiez.SentType.filepath, "fle.rar", True, _ReportCls, CancelToken.Token)
            Await client.Items.Folder(6789).UploadRemote("https://www.doman.com/mymov.mp4", "mymov.mp4")
            Await client.Items.Folder(6789).UploadRemoteAsync("https://www.doman.com/mymov.mp4")
            Await client.Items.Folder(6789).UploadRemoteMultiple(New List(Of String) From {{"https://www.doman.com/mymov.mp4"}, {"https://www.doman.com/mymov.mp4"}})
            Await client.Items.Folder(6789).UploadRemoteMultipleAsync(New List(Of String) From {{"https://www.doman.com/mymov.mp4"}, {"https://www.doman.com/mymov.mp4"}})
            Await client.Items.Folder(6789).UploadRemoteReportProgress("UploadRemoteMultipleAsync")
            Await client.Items.Folder(6789).ZipBytesArray

            ''Folders
            Await client.Items.Folders(New List(Of Long) From {6789}).CompressAsync(9876, "myArchive.zip")

            ''all files & folders
            Await client.Items.ListAll()

            ''Playlists
            Await client.Playlists(5432).Add(New List(Of Long) From {1234, 1234})
            Await client.Playlists(5432).ChangeFilePosition(1234, 7, 9)
            Await client.Playlists(5432).Clear
            Await client.Playlists(Nothing).Create("playlistName", pCloudSDK.Utilitiez.PlaylistTypeEnum.All)
            Await client.Playlists(5432).Delete
            Await client.Playlists(Nothing).List(pCloudSDK.Utilitiez.PlaylistTypeEnum.AudioOnly, pCloudSDK.Utilitiez.OutputEnum.PlaylistsWithoutFiles, 10, 2)
            Await client.Playlists(5432).Metadata
            Await client.Playlists(5432).Public
            Await client.Playlists(5432).Remove(New List(Of Long) From {1234})
            Await client.Playlists(5432).Rename("newPlaylistName")

            ''Publics
            Await client.Publics.AudioDirectUrl(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"), 240)
            Await client.Publics.CopyThumbnail(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"), 6789, "800x600", pCloudSDK.Utilitiez.ExtEnum.jpeg, True, True, Nothing)
            Await client.Publics.CopyZip(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"), 6789, Nothing)
            Await client.Publics.DirectZipUrl(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"), "myArchive.zip")
            Await client.Publics.DownloadAsZip(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"), "C:\Users", _ReportCls, CancelToken.Token)
            Await client.Publics.FileInFolderDirectUrl(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"), 1234)
            Await client.Publics.List()
            Await client.Publics.ListWithoutMetadata
            Await client.Publics.Metadata(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"))
            Await client.Publics.SaveToMyAccount(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"), 6789)
            Await client.Publics.SingleFileDirectUrl(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"))
            Await client.Publics.Thumbnail(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"), "800x600", pCloudSDK.Utilitiez.ExtEnum.jpeg, True)
            Await client.Publics.ThumbnailUrl(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"), "800x600", pCloudSDK.Utilitiez.ExtEnum.jpeg, True)
            Await client.Publics.VideoResolutionUrls(New Uri("https://my.pcloud.com/#page=publink&code=PUBLIC_LINK_CODE"), 1234)


        Catch ex As pCloudSDK.pCloudException
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
