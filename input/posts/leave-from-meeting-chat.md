Title: How to leave a Teams meeting chat, even if you are the organizer 
Published: 2/10/2022
Tags: Teams
---

#Introduction

I noticed that, if you schedule a Teams Meeting, (and you are the organizer), these are the options available for the meeting chat/conversation: 

![Teams Meetings No Leave Option](/images/NoLeaveOption.png)
 
See? No **Leave** option. 

_Did you know? Leave option will show up to attendes only if the meeting has > 2 participants_ 

![Teams Meetings Leave Option](/images/LeaveOption.png)

# The problem?

What about if, for some reason, you are not anymore in the topic, you switched roles in your company, or you just want to leave the conversation for good? 

Sure, you can mute or hide the conversation, but it will show up again if someone writes into the meeting chat. 

So, I investigated a little bit with Developer tools, as I did in [Getting some extra information about Teams federated users using PowerShell](https://get-itips.capazero.net/posts/extra-information-federated-teams) and [Mark Teams notifications as read](https://get-itips.capazero.net/posts/clear-teams-notifications) to see what happens when a non-organizer leaves a Meeting chat/conversation and found out that a REST API is called to do that, so decided to give it a try and force an organizer to Leave a Teams meeting chat. 

# PreRequisites

The tricky part is that we need to specify two things in the URL 

- Some sort of thread ID created for the meeting 

- Our Teams User ID  

_Sample URL_ 

```powershell
https://amer.ng.msg.teams.microsoft.com/v1/threads/19:meeting_MjRkYTA1ZWItZjVhYi00MDVjWJiZDQtMDQwZGU3OTkwZTIz@thread.v2/members/8:orgid:cb13db92-6e3a-4f30-a4a1-3be0d94d4ede 
```

So how do we get these values? Currently, I am trying to figure out a scripted way to do this, but for now, we can extract the thread ID related to the meeting by editing the meeting itself from Teams web, look in the address bar and copy and paste the whole URL to your favorite text editor. 

Then, grab everything between the **19:meeting** and **@thread.v2**, (including those strings), you will end up with something similar to this 

```powershell
19:meeting_NTNjZGZmODItYjc0NS00yLTkyOWMtZmRhOWEwNDc2ZWEy@thread.v2 
```

Now, let’s get our user id, the id of the organizer and for this, I used Teams PowerShell, but there are probably other options. 

I run  

```powershell
Get-TeamUser -GroupId 13854e5-baf1-403e-ad3f-b26687383541 
```

Using a `GroupId` of a team where the organizer belongs, and in the results, the column on the left will show you the `UserId` that we need, something like 

`cb13db92-6e3a-4f30-a4a1-3be0d94d4ede` 

Ok, now we have everything we need, this is the script, go and replace the thread ID and userId in the lines that begin with `Invoke-WebRequest` and `path`, you have to provide the credentials of the organizer when asked 

 
```powershell
Import-Module AADInternals  

#This will prompt for credentials so it supports MFA login  

$token = Get-AADIntAccessTokenForTeams  

$skypeToken = Get-AADIntSkypeToken -AccessToken $token 

Invoke-WebRequest -UseBasicParsing -Uri "https://amer.ng.msg.teams.microsoft.com/v1/threads/19:meeting_MjRkYTA1ZWItZjVhYi00MDVjLWJiZDQtMDQwZGU3OTkwZTIz@thread.v2/members/8:orgid:cb13db92-6e3a-4f30-a4a1-3be0d94d4ede" ` 

-Method "DELETE" ` 

-Headers @{ 

"method"="DELETE" 

  "authority"="amer.ng.msg.teams.microsoft.com" 

  "scheme"="https" 

  "path"="/v1/threads/19:meeting_MjRkYTA1ZWItZjVhYi00MDVjLWJiZDQtMDQwZGU3OTkwZTIz@thread.v2/members/8:orgid:cb13db92-6e3a-4f30-a4a1-3be0d94d4ede" 

  "sec-ch-ua"="`" Not A;Brand`";v=`"99`", `"Chromium`";v=`"98`", `"Microsoft Edge`";v=`"98`"" 

  "x-ms-session-id"="edba78e0-8e34-767a-2aae-fcb1e3d8bb68" 

  "behavioroverride"="redirectAs404" 

  "x-ms-scenario-id"="738" 

  "x-ms-client-env"="pds-prod-comm-usce-01" 

  "x-ms-client-type"="web" 

  "sec-ch-ua-mobile"="?0" 

  "clientinfo"="os=windows; osVer=10; proc=x86; lcid=es-es; deviceType=1; country=es; clientName=skypeteams; clientVer=1415/1.0.0.2022020411; utcOffset=-06:00; timezone=America/Costa_Rica" 

  "x-ms-client-version"="1415/1.0.0.2022020411" 

  "x-ms-user-type"="null" 

  "authentication"="skypetoken=$skypeToken" 

  "sec-ch-ua-platform"="`"Windows`"" 

  "origin"="https://teams.microsoft.com" 

  "sec-fetch-site"="same-site" 

  "sec-fetch-mode"="cors" 

  "sec-fetch-dest"="empty" 

  "referer"="https://teams.microsoft.com/" 

  "accept-encoding"="gzip, deflate, br" 

  "accept-language"="es" 

} 
```
(This script is a very first version, that means I would like to improve it, and it is provided without any guarantee.)  

Once run, you will receive a 200 status code HTTP output, and this will appear now in the Meeting chat/conversation 

![Removed from Teams Meetings Chat](/images/Removed.png)

# Conclusion

This might be useful for very specific cases, and, until Microsoft provides a UI for an organizer to leave a Teams meeting chat for good, we can use this little script.
If, for some reason, you want to return to the Meeting chat, you can ask a member of the Meeting chat (roster) to add you again

