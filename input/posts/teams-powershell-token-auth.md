Title: Microsoft Teams PowerShell and token-based auth
Published: 6/9/2021
Tags: Teams
---

**October 2022 Update**
The Teams PowerShell module has been updated a lot since I made this post, 4.7.1-preview version of the module finally adds support for Token-Based Authentication.
The [Connect-MicrosoftTeams](https://learn.microsoft.com/en-us/powershell/module/teams/connect-microsoftteams?view=teams-ps?WT.mc_id=M365-MVP-5004663) article has been updated to list the required permissions, but still lists the delegated ones, so a new article has been introduced for [Application-based authentication](https://learn.microsoft.com/en-us/MicrosoftTeams/teams-powershell-application-authentication?view=teams-ps?WT.mc_id=M365-MVP-5004663).

**You should follow those guidelines now to setup Certificate-based Authentication as this post is now outdated.**

**Early in May 2022**, a new entry was added to the [Official Docs article](https://docs.microsoft.com/microsoftteams/teams-powershell-release-notes) for the Microsoft Teams PowerShell module Release notes, we could read:

>- Updates for AccessToken login with Connect-MicrosoftTeams
>- Unified token array instead of resource specific access token parameters
>- Removed all AADGraph references as AADGraph is nearing End of Life

The entry was mentioning a 3.0.0 version number however was later changed to 2.3.2-preview, is this version finally fixing the issues trying to use other methods of connection other than interactive login? Let's remember that, even though in <2.3.2 we could connect using access tokens and Certificate-based authentication, if we tried to use any *-Cs* cmdlet they failed with a `GetSteppablePipeline` error.

But wait! not only that, `-AccountId` parameter wasn't usable too, if we try to use it we will get: _connect-microsoftteams : Integrated Windows Auth is not supported for managed users._

## Connect-MicrosoftTeams

Let's focus on this cmdlet, `-AadAccessToken` is gone, now we have `-AccessTokens`, and according to documentation, this parameter is an string array consisting of `@($graphtoken, $teamstoken)`, but how do we request them?

I am not a Graph expert, I leave that to my good friend [Alex Holmeset](https://twitter.com/AlexHolmeset), what follows is what I did based on what I could find in official documentation and Tech Community forums.

First, I created an application in the Azure AD Portal, for more information, see [Register an application with the Microsoft identity platform](https://docs.microsoft.com/graph/auth-register-app-v2), assigned permissions (more on that later), created a client secret and [granted consent](https://docs.microsoft.com/azure/active-directory/develop/v2-permissions-and-consent).

From that step, we will need to collect:

- The Application (client) ID (a GUID from the Overview page in the Application menu blade)
- The Directory (tenant) name (something.onmicrosoft.com)
- The client secret (you should have copied it when you created it)

AzureAD V2 PowerShell alternative to get the Application (client) ID and Directory (tenant) name:

```Powershell
$application="NameThatYouGaveToTheApp"
Connect-AzureAD
Get-AzureADApplication -Filter "DisplayName eq '$application'"  | ft AppId
Get-AzureADTenantDetail
```
### API Permissions

According to documentation you only need these:

- For Microsoft Graph API - Delegated permissions
    * "AppCatalog.ReadWrite.All", "Group.ReadWrite.All", "User.Read.All".
- For Skype and Teams Tenant Admin API
    * "Add all listed"

However, only setting those permissions will not be enough as the connect cmdlet will not warn you, but you will get a _Authorization_RequestDenied_ when running any cmdlet, so I ended up also adding these (trial and error and reviewing the permissions documented in [Use the Microsoft Graph API to work with Microsoft Teams](https://docs.microsoft.com/graph/api/resources/teams-api-overview?view=graph-rest-1.0):

- For Microsoft Graph API - Application permissions
    * "Directory.ReadWrite.All", "Directory.Read.All"

### Getting the Tokens

Once you get all of that I described earlier, it's time to get the tokens, as I said, I am not a Graph expert, so it was a lot of trial and error, the official documentation for [Connect-MicrosoftTeams](https://docs.microsoft.com/powershell/module/teams/connect-microsoftteams?view=teams-ps) said

```Powershell
$graphtoken = #Get MSGraph Token for following for resource  "https://graph.microsoft.com" and scopes "AppCatalog.ReadWrite.All", "Group.ReadWrite.All", "User.Read.All";
$teamstoken = #Get Teams resource token for resource id "48ac35b8-9aa8-4d74-927d-1f4a14a0b239" and scope "user_impersonation";
```

And this [Tech Community thread](https://techcommunity.microsoft.com/t5/teams-developer/authenticating-with-an-access-token-connect-microsoftteams/m-p/2233794/page/2
) also helped.

For Graph token:

```Powershell
$clientId = "6a7b5598-clientid-a1be-503ee9eda982"  
$clientSecret = "olbc9secretfmWRMC55-.Qet"  
$tenantName = "tenantname.onmicrosoft.com"  
$resource = "https://graph.microsoft.com/"  
$tokenBody = @{  
   Grant_Type    = "client_credentials"  
   Scope         = "https://graph.microsoft.com/.default"  
   Client_Id     = $clientId  
   Client_Secret = $clientSecret  
}   
$graphTokenResponse = Invoke-RestMethod -Uri "https://login.microsoftonline.com/$tenantName/oauth2/v2.0/token" -Method POST -Body $tokenBody
```

For Teams token:

```Powershell
$clientId = "6a7b5598-clientid-a1be-503ee9eda982"  
$clientSecret = "olbc9secretfmWRMC55-.Qet" 
$tenantName = "tenantname.onmicrosoft.com"   
$resource = "https://api.interfaces.records.teams.microsoft.com"  
$tokenBody = @{  
   Grant_Type    = "client_credentials"  
   Scope         = "48ac35b8-9aa8-4d74-927d-1f4a14a0b239/.default"  
   Client_Id     = $clientId  
   Client_Secret = $clientSecret
  
}   
$TeamsTokenResponse = Invoke-RestMethod -Uri "https://login.microsoftonline.com/$tenantName/oauth2/v2.0/token" -Method POST -Body $tokenBody
```

So now we have the tokens in two variables, `$graphTokenResponse.access_token` and `$TeamsTokenResponse.access_token`, if you parse the tokens in https://jwt.ms/ you should see something like this for Graph token:

```json
  "roles": [
    "Directory.ReadWrite.All",
    "Directory.Read.All"
  ],
```

and this for the Teams token:

```json
  "roles": [
    "application_access_custom_sba_appliance",
    "application_access"
  ],
```

### Ok, let's connect now

The cmdlet should be used like this

```Powershell
Connect-MicrosoftTeams -AccessTokens @($graphTokenResponse.access_token,$TeamsTokenResponse.access_token) -AccountId meganb@tenantname.onmicrosoft.com
```

If everything went well, you should be able to run any native Teams cmdlet.

### -Cs* cmdlets

Unfortunately, any -Cs* cmdlets still result in an error:

```Powershell
Get-CsOnlineSession : Connecting to remote server api.interfaces.records.teams.microsoft.com failed with the following
error message : The WinRM client cannot process the request. The authentication mechanism requested by the client is
not supported by the server or unencrypted traffic is disabled in the service configuration. Verify the unencrypted
traffic setting in the service configuration or specify one of the authentication mechanisms supported by the server.
To use Kerberos, specify the computer name as the remote destination. Also verify that the client computer and the
destination computer are joined to a domain. To use Basic, specify the computer name as the remote destination,
specify Basic authentication and provide user name and password. Possible authentication mechanisms reported by
server: For more information, see the about_Remote_Troubleshooting Help topic.
At C:\Program Files\WindowsPowerShell\Modules\MicrosoftTeams\2.3.2\net472\SfBORemotePowershellModule.psm1:63 char:22
+     $remoteSession = & (Get-CsOnlineSessionCommand)
+                      ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : NotSpecified: (:) [Get-CsOnlineSession], PSRemotingTransportException
    + FullyQualifiedErrorId : PSRemotingTransportException,Microsoft.Teams.ConfigApi.Cmdlets.GetCsOnlineSession

Invoke-Command : Cannot validate argument on parameter 'Session'. The argument is null or empty. Provide an argument
that is not null or empty, and then try the command again.
At C:\Program Files\WindowsPowerShell\Modules\MicrosoftTeams\2.3.2\net472\SfBORemotePowershellModule.psm1:9490 char:38
+ ...    -Session (Get-PSImplicitRemotingSession -CommandName 'Get-CsOnline ...
+                 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : InvalidData: (:) [Invoke-Command], ParentContainsErrorRecordException
    + FullyQualifiedErrorId : ParameterArgumentValidationError,Microsoft.PowerShell.Commands.InvokeCommandCommand
```    

## Conclussion
The changes are welcomed, however I really hope the issue with -Cs* cmdlets is fixed once this release gets General Availability condition, any non-interactive method is very useful for unattended scripting scenarios and it has been long requested by the community.
If you have any other method, other than the described in this blog post to obtain the tokens or if you could make -Cs* cmdlets work please leave me a comment!

For instructions on how to install this specific version, you can visit [>_☁ MSShells.net](https://msshells.net/).
