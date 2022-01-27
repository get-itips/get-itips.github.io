Title: Getting some extra information about Teams federated users using PowerShell
Published: 12/20/2021
Tags: Teams
---

**Last update:** 27th of January - 2022 Added TFL support.

# Introduction

Inspired by my friend's Alexander Holmeset excellent blog post: [Microsoft Teams Speed Dial Contacts Provisioning](https://alexholmeset.blog/2021/12/13/microsoft-teams-speed-dial-contacts-provisioning/), I decided to investigate some other API calls made by the Teams client (as Alexander explains, easier to see using Teams web and activating browser's developer tools) and I was interested in one in particular, a request that is made when starting a new chat with external contacts, other tenants or Skype consumer.

I said, let's see how it discovers those contacts, just for fun or learning purposes, but I discovered more information is outputted than what the end user actually sees.

# Useful information?

The URL that we will be querying is:

https://teams.microsoft.com/api/mt/part/amer-03/beta/users/[sip_uri]/externalsearchv3

If I use it to query another Teams tenant user information:

```powershell
tenantId          : ceffd5d3-edcb-482e-a7ed-3ecedfe5519c
isShortProfile    : False
accountEnabled    : True
featureSettings   : @{coExistenceMode=TeamsOnly}
userPrincipalName : freddiem@snoopcore.com
givenName         : freddiem@snoopcore.com
surname           :
email             : freddiem@snoopcore.com
displayName       : Freddie Mercury
type              : Federated
mri               : 8:orgid:87da1b0b-e2a5-4964-9fcc-70bf0a186fe0
objectId          : 87da1b0b-e2a5-4964-9fcc-70bf0a186fe0
```

And yes, we can even know if the other party is in TeamsOnly or in Islands mode:

```powershell
featureSettings   : @{coExistenceMode=Islands}
```

This is another example, a company that does not have tenancy but Skype For Business on-premises:

```powershell
tenantId          : ac23bebc-10b0-4f0f-bae2-48b14c65af41
isShortProfile    : False
accountEnabled    : True
featureSettings   : @{coExistenceMode=Unknown}
userPrincipalName : john.doe@contoso.com
givenName         : john.doe@contoso.com
surname           :
email             : john.doe@contoso.com
displayName       : john.doe@contoso.com
type              : Federated
mri               : 8:sfb:e1c75ab3-bc2d-4733-ba24-c4186f48d1d0
objectId          : e1c75ab3-bc2d-4733-ba24-c4186f48d1d0
```

See how the mri now says sfb?


also a Skype (consumer) user:

```powershell
skypeId           : megan.bowen
city              : Ciudad Autonoma de Buenos Aires
state             : Buenos Aires
country           : Argentina
avatarUrl         : https://api.skype.com/users/megan.bowen/profile/avatar
isShortProfile    : False
accountEnabled    : True
userPrincipalName : megan.bowen@hotmail.com
email             : megan.bowen@hotmail.com
displayName       : megan bowen
type              : SkypeConsumer
mri               : 8:megan.bowen
```

That is more information that I was expecting... :)

Is this useful? I do not exactly know, but I decided to share, maybe an administrator receives a call saying that the CEO is unable to communicate with another external user, you get the SIP URI of this user and you can, using this script understand if at least at federation level everything is how it supposed to be.

Also, if you are a heavy Teams user/Administrator/Consultant you already received the "Due to organization policy changes..." message, and I think this could help troubleshoot that and other federation scenarios.

# The Script

Here is the script, I basically modified the great output of "Copy as PowerShell" feature of Edge's developer tool, as I see this more as learning purposes, I did not put much effort into it, there is a lot of room to improve it. Credits also to the great module [AADInternals](https://o365blog.com/aadinternals/) to get the required tokens (Teams token and x-skypetoken)

```powershell

$urlPart1="https://teams.microsoft.com/api/mt/part/amer-03/beta/users/"
$userToSearch="meganb@contoso.com" #Replace this with the other party's SIP URI you want to search more information for
$urlPart3="/externalsearchv3?includeTFLUsers=true"

$FinalUrl=$urlPart1+$userToSearch+$urlPart3

Import-Module AADInternals

$user = "admin@M365BV2XXX.onmicrosoft.com" #Replace this with your tenant's username
$password = "P@ssw0rd" #And your password
$secpwd = ConvertTo-SecureString $password -AsPlainText -Force
$Cred = New-Object System.Management.Automation.PSCredential ($user,$secpwd)

$token = Get-AADIntAccessTokenForTeams -Credentials $cred

$skypeToken = Get-AADIntSkypeToken -AccessToken $token


$Result=Invoke-WebRequest -UseBasicParsing -Uri $finalUrl `
-Headers @{
"method"="GET"
  "authority"="teams.microsoft.com"
  "scheme"="https"
  "path"="/api/mt/part/amer-03/beta/users/$userToSearch/externalsearchv3?includeTFLUsers=true"
  "sec-ch-ua"="`" Not A;Brand`";v=`"99`", `"Chromium`";v=`"96`", `"Microsoft Edge`";v=`"96`""
  "x-ms-user-type"="null"
  "x-ms-client-type"="web"
  "authorization"="Bearer $token"
  "x-skypetoken"=$skypeToken
  "x-ms-client-version"="1415/1.0.0.2021120724"
  "sec-ch-ua-platform"="`"Windows`""
  "x-ms-session-id"="8f622a27-5f89-bf7f-ad89-9bb9550300c8"
  "x-ms-scenario-id"="309"
  "x-anchormailbox"=$user
  "x-ms-client-env"="pds-prod-azsc-usea-01"
  "sec-ch-ua-mobile"="?0"
  "accept"="application/json, text/plain, */*"
  "x-ms-client-caller"="search_externally_people_resolver"
  "x-ringoverride"="general"
  "sec-fetch-site"="same-origin"
  "sec-fetch-mode"="cors"
  "sec-fetch-dest"="empty"
  "referer"="https://teams.microsoft.com/_"
  "accept-encoding"="gzip, deflate, br"
  "accept-language"="es,es-ES;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6"
  }
  
  ConvertFrom-Json -InputObject $result.Content

  ```
