Title: How to succesfully import Root CA to Trend IWSVA
Published: 12/13/2016
Tags: SSL
---

Customer reached me to get help importing Root CA certificate to InterScan Web Security Virtual Appliance,  
from Trend Micro documentation:

> "By default, IWSVA acts as a private Certificate Authority (CA) and dynamically generates digital certificates   
> that are sent to client browsers to complete a secure passage for HTTPS connections. However, the default CA 
> is not signed by a well-known (trusted) CA on the Internet.Client browsers always display a certificate 
> warning every time users access an HTTPS Web site.
> Although users can safely ignore the certificate warning, Trend Micro recommends using a signed root certificate for IWSVA."  

So, if you want your users not to receive a warning from their browsers you have to use, typical,an internal Certification Authority that all client computers trust, for example an Enterprise Windows CA like this was the case.

So, first of all, I had to export the CA Root Certificate including the Private Key,   
go on and logon the Certification Authority computer:


1. Go to 'Start' -> 'Run' -> mmc.exe
2. Click on 'File' -> 'Add / Remove Snap-in'
3. Click the 'Add...' button
4. Select 'Certificates' then click 'Add'
5. Select 'Computer Account' -> 'Next' -> 'Local Computer' -> 'Finish'
6. click 'Close' -> 'OK'
7. Expand Certificates -> and click on 'Personal' -> 'Certificates'
8. Right click the CA cert and choose 'All Tasks' -> 'Export'
9. Click 'Next' -> Select 'Yes, Export the private key' -> 'Next'
10. Uncheck all of the options here. PKCS 12 should be the only option available. Click 'Next'
11. Give the private key a password of your choice
12. Give a filename to save as and click 'Next', then 'Finish'

Now that you have a .pfx file we will need to use the excellent tool called OpenSSL, as I was using Windows, I used OpenSSL for Windows binaries that you can get here http://gnuwin32.sourceforge.net/packages/openssl.htm

Install it and copy the pfx to your computer.

start a command prompt or PowerShell and run:

```cmd
openssl pkcs12 -in -clcerts -nokeys -out certificate.cer
```

then

```cmd
openssl pkcs12 -in -nocerts -out pkey-encrypted.key 
```

You will end up with two files: certificate.cer and pkey-encrypted.key

If you import them right into IWSVA console it will fail saying "Root Certificate file is invalid. Try Again"

This drove me nuts, documentation is not so good, it only says you need to provide a PEM file format,  base54 encoded and RSA based encrypted key, and I was sure I was providing just that...  but wait... after many tries and errors, I read that some systems do not accept anything below this line in public certificate key file:

```code
-----BEGIN CERTIFICATE-----
```
and this line for the private key file

```code
-----BEGIN PRIVATE KEY-----
```

so I tried and deleted everything below that lines on both lines... and it worked! Apparently IWSVA can not handle well ignoring the information that comes there.

Hope is useful for someone else.
