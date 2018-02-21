Title: Handy SMTP Handshake for testing with Telnet
Published: 2/8/2017
Tags: ExchangeServer
---

Hi,

It's always useful to test SMTP service, using Telnet, so these are the commands needed:

First of all, connect through Command Prompt or Powershell

```cmd
Telnet x.x.x.x tcpPort
```

For Example `Telnet 10.0.2.1 25`

You will get a welcome message from SMTP Server, like this one:

`220 exc02.contoso.com Microsoft ESMTP MAIL Service ready at Wed`

then write sequentially hitting enter at each line what is coloured in green here (no color lines are server responses)

```code
EHLO mailtesting
AUTH LOGIN
334 VXNlcm5hbWU6
Y29udG9zb1xhY2NvdW50
334 UGFzc3dvcmQ6
dGhlUGFzc3dvcmQ=
```

```code
MAIL FROM:
RCPT TO:
DATA
Subject: here comes the test 
This is the email content.
.
250 Queued mail for delivery 
```
---
**Important notes:** 
You have to hit enter twice after subject input.
account name and password are entered Base64 encoded, so first encode that information using for example [this site.](https://www.base64encode.org/)
