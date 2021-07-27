Title: Task did NOT pass the parameter validation, the error message is The target site does not exist
Published: 7/27/2021
Tags: SharePoint
---

Hello,

Doing some tests migrations from SharePoint Server (on-premises) to SharePoint Online using SPMT ([SharePoint Migration Tool](https://docs.microsoft.com/en-us/sharepointmigration/new-and-improved-features-in-the-sharepoint-migration-tool)) using the SPMT PowerShell module I faced this error message:

```
Task did NOT pass the parameter validation, the error message is The target site does not exist
```

At first, I checked if, maybe I mispelled the target SPO site, but it was ok, reading the logs I found this

```
Microsoft.SharePoint.Migration.Common.Exceptions.WebTransportException ---> System.Net.WebException: The underlying connection was closed: An unexpected error occurred on a send. ---> System.IO.IOException: Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host. ---> System.Net.Sockets.SocketException: An existing connection was forcibly closed by the remote host
   at System.Net.Sockets.NetworkStream.Read(Byte[] buffer, Int32 offset, Int32 size)
   --- End of inner exception stack trace ---
   at System.Net.Sockets.NetworkStream.Read(Byte[] buffer, Int32 offset, Int32 size)
   at System.Net.FixedSizeReader.ReadPacket(Byte[] buffer, Int32 offset, Int32 count)
   at System.Net.Security.SslState.StartReceiveBlob(Byte[] buffer, AsyncProtocolRequest asyncRequest)
   at System.Net.Security.SslState.CheckCompletionBeforeNextReceive(ProtocolToken message, AsyncProtocolRequest asyncRequest)
   at System.Net.Security.SslState.ForceAuthentication(Boolean receiveFirst, Byte[] buffer, AsyncProtocolRequest asyncRequest)
   at System.Net.Security.SslState.ProcessAuthentication(LazyAsyncResult lazyResult)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Net.TlsStream.ProcessAuthentication(LazyAsyncResult result)
   at System.Net.TlsStream.Write(Byte[] buffer, Int32 offset, Int32 size)
```

Hmm... SSL... This must be related to TLS 1.2 in PowerShell, to check, tried to use ```Invoke-WebRequest``` against an SSL site and it failed with

```
Invoke-WebRequest : The request was aborted: Could not create SSL/TLS secure channel.
```

So I run in PowerShell:

```
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
```

And voilá, the error was gone and the migration finished with no issues. Keep in mind, that command will only last for the duration of the PowerShell session.

To make it permanent I ran this one-liners:

```
Set-ItemProperty -Path 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\.NetFramework\v4.0.30319' -Name 'SchUseStrongCrypto' -Value '1' -Type DWord
Set-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\.NetFramework\v4.0.30319' -Name 'SchUseStrongCrypto' -Value '1' -Type DWord
```

Hope this is useful for someone as it was for me.