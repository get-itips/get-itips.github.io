Title: Renewing Web Management Service certificate from SHA1 to SHA256 in Exchange Server
Published: 7/7/2022
Tags: ExchangeServer
---

# Background

The [Exchange Health Checker](https://microsoft.github.io/CSS-Exchange/Diagnostics/HealthChecker/) checks for the presence of SHA1 signed certificates in the output of Get-ExchangeCertificate, upon checking on a customer where I ran the script, a warning was raised about SHA1 certificates being present, the only SHA1-signed certificate was the one that Exchange creates for the Web Management service of IIS (WMSvc) when installing Exchange. This certificate can be SHA1-signed if, at the time of the setup, Exchange did not create certificates with a signature algorithm of SHA256.

It seems that [starting from CU13](https://blog.rmilne.ca/2016/07/20/exchange-self-signed-sha2-certificates/) of Exchange 2013 and Exchange 2016 CU2, self-signed certificates created using New-ExchangeCertificate started to be created using SHA256.

# Checking if using SHA1 for WMSvc service

If you are not sure, confirm that you are indeed using a SHA1-signed certificate for WMSvc

1)	Open _IIS Manager_
2)	Click on the name of the server in the left pane
3)	2-Click on _Management Service_
4)	Take note of the name of the SSL certificate (this will be the friendly name in the _Certificates_ snap-in)
5)	Open _mmc.exe_ and add the _certificates Snap-In_ for _local computer_
6)	Search for the certificate, it will be something like “WMSvc - ServerName” in the personal store, open the properties of the certificate
7)	Click the _Details_ tab
8)	Check if _signature algorithm_ field is SHA1

# Getting a new self-signed certificate SHA256-signed

__Prerequisite:__ You must be running Exchange 2013 CU13 or later or Exchange 2016 CU2, Exchange 2019 creates SHA2 certificates from RTM

Open Exchange Management Shell and run this cmdlet to create a new self-signed certificate

```powershell
New-ExchangeCertificate -SubjectName "cn=WMSvc-SHA2-SERVERNAME" -FriendlyName "WMSVC-SHA2"
```
__Note:__ Replace SERVERNAME in the cmdlet with the actual name of your Exchange Server

#  Verify the new certificate was generated and trust it

If you still have the certificates snap-in open, you will now have a _Issued to_ certificate with the subject name we used in the step above.

1)	Right-click & copy this certificate
2)	Select _Trusted Root Certification Authorities_
3)	Right-click & paste this certificate

Swapping the certificate used by WMSvc service
1)	Open _IIS Manager_
2)	Click on the name of the server in the left pane
3)	2-Click on _Management Service_
4)	In the _Actions_ pane, click _Stop_, accept any warning
5)	Now you can select different certificate in the _SSL Certificate_ drop-down, choose the newly generated cert
6)	In the _Actions_ pane, click _Start_

That should be it, you will now have a Web Management Service running a SHA2 signed certificate and the health checker should not raise you this warning.
