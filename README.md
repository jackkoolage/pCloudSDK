## pCloudSDK

`Download:`[https://github.com/jackkoolage/pCloudSDK/releases](https://github.com/jackkoolage/pCloudSDK/releases)<br>
`Help:`[https://github.com/jackkoolage/pCloudSDK/wiki](https://github.com/jackkoolage/pCloudSDK/wiki)<br>
`NuGet:`
[![NuGet](https://img.shields.io/nuget/v/DeQmaTech.pCloudSDK.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/DeQmaTech.pCloudSDK)<br>


# Features:
* Assemblies for .NET 4.5.2 and .NET Standard 2.0 and .NET Core 2.1
* Just one external reference (Newtonsoft.Json)
* Easy installation using NuGet
* Upload/Download tracking support
* Proxy Support
* Upload/Download cancellation support


# List of functions:
**Token**
1. Register
1. GetAuthToken

**Account**
1. IP
1. ListInvites
1. GetRegistrationPageUrl
1. GetCurrentServer
1. DeleteToken
1. ListTokens
1. ChangeMail
1. UserInfo
1. ResendActivationMail

**Files**
1. ThumbnailUrl
1. CompressAsync
1. Copy
1. DownloadAsZip

**File**
1. Public
1. Thumbnail
1. ThumbnailUrl
1. CopyThumbnail
1. VideoToMp3
1. AudioDirectUrl
1. ChangesHistory
1. CompressAsync
1. UnCompressTaskProgress
1. UnCompressAsync
1. DirectUrl
1. VideoDirectUrl
1. VideoResolutionUrls
1. Checksum
1. Move
1. Rename
1. Delete
1. Copy
1. ZipBytesArray

**Folder**
1. Public
1. DirectZipUrl
1. CompressAsync
1. CompressTaskProgress
1. ListSubFoldersTree
1. List
1. ListWithoutFiles
1. ListWithoutShared
1. Create
1. CreateIfNotExists
1. Rename
1. Copy
1. Delete
1. Move
1. UploadRemoteAsync
1. UploadRemote
1. UploadRemoteMultipleAsync
1. UploadRemoteMultiple
1. UploadRemoteReportProgress
1. UploadLocal

**Folders**
1. CompressAsync

**Publics**
1. Metadata
1. SingleFileDirectUrl
1. FileInFolderDirectUrl
1. SaveToMyAccount
1. SaveToMyAccount
1. List
1. ListWithoutMetadata
1. DownloadAsZip
1. DirectZipUrl
1. Thumbnail
1. ThumbnailUrl
1. CopyThumbnail
1. CopyZip
1. VideoResolutionUrls
1. VideoResolutionUrls
1. AudioDirectUrl
1. AudioDirectUrl

**Playlists**
1. ChangeFilePosition
1. Rename
1. List
1. Clear
1. Remove
1. Add
1. Metadata
1. Delete
1. Create
1. Public

# CodeMap:
![codemap](https://i.postimg.cc/bJ6Y9FW3/pc-codemap.png)

# Code simple:
```vb.net
Await pCloudSDK.Authentication.Register("name@domian.com", "123465#")
Await pCloudSDK.Authentication.GetAuthToken("name@domian.com", "123465#")

Try
    Dim client As pCloudSDK.IClient = New pCloudSDK.PClient("username", "password", New pCloudSDK.ConnectionSettings With {.CloseConnection = True, .TimeOut = TimeSpan.FromMinutes(80), .Proxy = New ProxyConfig With {.SetProxy = True, .ProxyIP = "127.0.0.1", .ProxyPort = 80, .ProxyUsername = "user", .ProxyPassword = "123456"}})

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
```
