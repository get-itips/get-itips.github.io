Title: Mark Teams notifications as read
Published: 1/4/2022
Tags: Teams
---

Hello!

# Introduction

The idea behind this blog post comes from the same idea behind this other blog post [Getting some extra information about Teams federated users using PowerShell](https://get-itips.capazero.net/posts/extra-information-federated-teams), so you can read more details about how this is done and what it requires there.

Essentially, I continued exploring client-side API calls made by the Teams client, and I decided to play around with the one that is called to mark the notifications that appear under Activity in the Teams app bar as read.

# Why

Maybe you are returning from vacations/holidays and you have a lot of notifications to mark as read, there isn't currently an easy way to do this using the Teams client, you have to mark as read every single one, so you might want to clear the clutter more easily.

# The Script

This script is a very first version, that means I would like to improve it a lot, and it is provided without any guarantee.

```powershell
# Clear all Microsoft Teams notifications

Import-Module AADInternals

#This will prompt for credentials so it supports MFA login
$token = Get-AADIntAccessTokenForTeams

$skypeToken = Get-AADIntSkypeToken -AccessToken $token

$result=Invoke-WebRequest -UseBasicParsing -Uri "https://amer.ng.msg.teams.microsoft.com/v1/users/ME/conversations/48%3Anotifications/messages?view=msnp24Equivalent|supportsMessageProperties&pageSize=200" `
-Headers @{
"method"="GET"
  "authority"="amer.ng.msg.teams.microsoft.com"
  "scheme"="https"
  "path"="/v1/users/ME/conversations/48%3Anotifications/messages?view=msnp24Equivalent|supportsMessageProperties&pageSize=200"
  "sec-ch-ua"="`" Not A;Brand`";v=`"99`", `"Chromium`";v=`"96`", `"Microsoft Edge`";v=`"96`""
  "x-ms-session-id"="ae16f178-a088-a7bb-603b-27aad38b6c88"
  "behavioroverride"="redirectAs404"
  "x-ms-scenario-id"="130"
  "x-ms-client-cpm"="ApplicationLaunch"
  "x-ms-client-env"="pds-prod-azsc-usce-01"
  "x-ms-client-type"="web"
  "sec-ch-ua-mobile"="?0"
  "clientinfo"="os=windows; osVer=10; proc=x86; lcid=en-us; deviceType=1; country=us; clientName=skypeteams; clientVer=1415/1.0.0.2021120940; utcOffset=-06:00; timezone=America/Costa_Rica"
  "x-ms-client-version"="1415/1.0.0.2021120940"
  "x-ms-user-type"="null"
  "authentication"="skypetoken=$skypeToken"
  "sec-ch-ua-platform"="`"Windows`""
  "origin"="https://teams.microsoft.com"
  "sec-fetch-site"="same-site"
  "sec-fetch-mode"="cors"
  "sec-fetch-dest"="empty"
  "referer"="https://teams.microsoft.com/"
  "accept-encoding"="gzip, deflate, br"
  "accept-language"="es,es-ES;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6"
}

$messages=ConvertFrom-Json -InputObject $result.Content


$NotificationsToClear=$messages.messages | where-object {$_.properties.isread -ne "True"}


foreach($notifications in $notificationsToClear)
{

$urlFQDN="https://amer.ng.msg.teams.microsoft.com"
$urlPart1="/v1/users/ME/conversations/48%3Anotifications/messages/"
$urlPart3="/properties?name=isread"
$finalUrl=$urlFQDN+$urlPart1+$notifications.Id+$urlPart3

Write-Host "Clearing notification" $notifications.Id

$result=Invoke-WebRequest -UseBasicParsing -Uri $finalUrl `
-Method "PUT" `
-Headers @{
"method"="PUT"
  "authority"="amer.ng.msg.teams.microsoft.com"
  "scheme"="https"
  "path"=$urlPart1+$notifications.Id+$urlPart3
  "sec-ch-ua"="`" Not A;Brand`";v=`"99`", `"Chromium`";v=`"96`", `"Microsoft Edge`";v=`"96`""
  "x-ms-user-type"="null"
  "x-ms-client-type"="web"
  "x-ms-client-version"="1415/1.0.0.2021120940"
  "authentication"="skypetoken=$skypeToken"
  "sec-ch-ua-platform"="`"Windows`""
  "x-ms-scenario-id"="716"
  "x-ms-client-env"="pckgsvc-prod-c1-usea-01"
  "sec-ch-ua-mobile"="?0"
  "clientinfo"="os=windows; osVer=10; proc=x86; lcid=en-us; deviceType=1; country=us; clientName=skypeteams; clientVer=1415/1.0.0.2021120940; utcOffset=-06:00; timezone=America/Costa_Rica"
  "behavioroverride"="redirectAs404"
  "x-ms-client-caller"="markReadStatus"
  "origin"="https://teams.microsoft.com"
  "sec-fetch-site"="same-site"
  "sec-fetch-mode"="cors"
  "sec-fetch-dest"="empty"
  "referer"="https://teams.microsoft.com/"
  "accept-encoding"="gzip, deflate, br"
  "accept-language"="es,es-ES;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6"
} `
-ContentType "application/json" `
-Body "{`"isread`":true}"

}
```

# Next version

There are different types of notifications, like mentions, reply, etc, this very first and raw version of the script clears all notifications however I would like to work on it to selectively clear different types of notifications (for example everything but mentions).
I invite everyone that wants to contribute to this script to do so here in this [GitHub repo](https://github.com/get-itips/MiscScripts/blob/main/Teams/Set-NotificationsAsRead.ps1).
