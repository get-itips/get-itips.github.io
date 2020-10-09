Title: PnP Teams Cmdlets Review
Published: 10/8/2020
Tags: Teams
---
![introimage](/images/TeamsPNP.png)

# Introduction

On July 2020, the great Community of [SharePoint Patterns and Practices PowerShell](https://docs.microsoft.com/powershell/sharepoint/sharepoint-pnp/sharepoint-pnp-cmdlets?view=sharepoint-ps) released cmdlets to manage Microsoft Teams aspects, I have been using PnP cmdlets for SharePoint for a while and they are really powerfull and best of all, they are open sourced and community-maintained, so I was curious to try these Teams-related cmdlets.

# Requirements

## PnP Module Version

Be sure to be running the latest version of the SharePoint PnP PowerShell Online module, to this date, that is 3.25.2009.1.

![versionimage](/images/pnpteams_versionimage.png)

also, we can check, which commands are currently available for Teams related tasks running

```powershell
Get-Command -Module *PnP* -Name "*Teams*"
```
and those are

```powershell
CommandType     Name                                               Version    Source
-----------     ----                                               -------    ------
Cmdlet          Add-PnPTeamsChannel                                3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Add-PnPTeamsTab                                    3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Add-PnPTeamsTeam                                   3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Add-PnPTeamsUser                                   3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Get-PnPTeamsApp                                    3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Get-PnPTeamsChannel                                3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Get-PnPTeamsChannelMessage                         3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Get-PnPTeamsTab                                    3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Get-PnPTeamsTeam                                   3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Get-PnPTeamsUser                                   3.25.20... SharePointPnPPowerShellOnline
Cmdlet          New-PnPTeamsApp                                    3.25.20... SharePointPnPPowerShellOnline
Cmdlet          New-PnPTeamsTeam                                   3.25.20... SharePointPnPPowerShellOnline
Cmdlet          New-PnPTenantSequenceTeamSite                      3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Remove-PnPTeamsApp                                 3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Remove-PnPTeamsChannel                             3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Remove-PnPTeamsTab                                 3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Remove-PnPTeamsTeam                                3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Remove-PnPTeamsUser                                3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Set-PnPTeamsChannel                                3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Set-PnPTeamsTab                                    3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Set-PnPTeamsTeam                                   3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Set-PnPTeamsTeamArchivedState                      3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Set-PnPTeamsTeamPicture                            3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Submit-PnPTeamsChannelMessage                      3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Sync-PnPAppToTeams                                 3.25.20... SharePointPnPPowerShellOnline
Cmdlet          Update-PnPTeamsApp                                 3.25.20... SharePointPnPPowerShellOnline
```

## Graph API Permissions

To run any of those cmdlets, we will need special Graph API Permissions, thankfully, those are documented at Microsoft Docs, for example for the `Get-PnPTeamsTeam` cmdlet:

![graphapipermission](/images/pnpteams_graphperm.png)

We will need either Group.Read.All or Group.ReadWrite.All.

## Connecting to PnP Online

If we try to use any *PnPTeams* cmdlet without the required permissions we will receive an error like this:

```powershell
Get-PnPTeamsTeam : Unable to retrieve a token for MicrosoftGraph. Ensure you connect using one of the
Connect-PnPOnline commands which uses the -ClientId argument or use Connect-PnPOnline -Scopes to connect.
At line:1 char:1
+ Get-PnPTeamsTeam
+ ~~~~~~~~~~~~~~~~
    + CategoryInfo          : ConnectionError: (:) [Get-PnPTeamsTeam], InvalidOperationException
    + FullyQualifiedErrorId : NO_OAUTH_TOKEN,PnP.PowerShell.Commands.Graph.GetTeamsTeam
```
The error message is pretty clear, we must connect using the `-ClientId` or  `-Scopes`, this time, I will use the `-Scopes` parameter, like this:

```powershell
Connect-PnPOnline -Scopes Group.ReadWrite.All
```

The first time that you run that cmdlet, you will be presented with the typical app consent window, to approve the use of the PnP Management Shell on your tenant, that is totally expected if you are used to work with the Graph API.

Note that I used only one scope but you can specify more than one, for more information refer to the [official documentation of the cmdlet](https://docs.microsoft.com/powershell/module/sharepoint-pnp/connect-pnponline?view=sharepoint-ps).

Now that we are connected, let's move on and test the Teams cmdlets, for a more detailed guide on how to connect there is an excellent [blog post](https://www.erwinmcm.com/pnp-teams-cmdlets/) from one Erwin van Hunen, one of the Maintainers of PnP PowerShell.

# The Teams PnP cmdlets

Let's start with, what seems to be the most simple one `Get-PnPTeamsTeam`

![getpnpteamsteam](/images/pnpteams_getteam.png)

Hey, wait a minute, that looks familiar, yes, it is pretty similar output as if we would run `Get-Team` using the [Official Microsoft Teams PowerShell](https://docs.microsoft.com/microsoftteams/teams-powershell-overview).

Only when we ask for the members of the output object we identify differences in the output:

For the PnP Module version

```powershell
PS C:\> $teams[0] | Get-Member
   TypeName: PnP.PowerShell.Commands.Model.Teams.Team

Name              MemberType Definition
----              ---------- ----------
Equals            Method     bool Equals(System.Object obj)
GetHashCode       Method     int GetHashCode()
GetType           Method     type GetType()
ToString          Method     string ToString()
Apps              Property   System.Collections.Generic.List[PnP.PowerShell.Commands.Model.Teams.TeamAppInstance] Apps {get;}
Classification    Property   string Classification {get;set;}
CloneFrom         Property   string CloneFrom {get;set;}
Description       Property   string Description {get;set;}
DiscoverySettings Property   PnP.PowerShell.Commands.Model.Teams.TeamDiscoverySettings DiscoverySettings {get;set;}
DisplayName       Property   string DisplayName {get;set;}
FunSettings       Property   PnP.PowerShell.Commands.Model.Teams.TeamFunSettings FunSettings {get;set;}
GroupId           Property   string GroupId {get;set;}
GuestSettings     Property   PnP.PowerShell.Commands.Model.Teams.TeamGuestSettings GuestSettings {get;set;}
IsArchived        Property   System.Nullable[bool] IsArchived {get;set;}
MailNickname      Property   string MailNickname {get;set;}
MemberSettings    Property   PnP.PowerShell.Commands.Model.Teams.TeamMemberSettings MemberSettings {get;set;}
MessagingSettings Property   PnP.PowerShell.Commands.Model.Teams.TeamMessagingSettings MessagingSettings {get;set;}
Security          Property   PnP.PowerShell.Commands.Model.Teams.TeamSecurity Security {get;set;}
Specialization    Property   System.Nullable[PnP.PowerShell.Commands.Model.Teams.TeamSpecialization] Specialization {get;set;}
Visibility        Property   System.Nullable[PnP.PowerShell.Commands.Model.Teams.GroupVisibility] Visibility {get;set;}
```
For the Microsoft Teams PowerShell module

```powershell
PS C:\> $teams[0] | Get-Member
   TypeName: Microsoft.TeamsCmdlets.PowerShell.Custom.Model.TeamSettings

Name                              MemberType Definition
----                              ---------- ----------
Equals                            Method     bool Equals(System.Object obj)
GetHashCode                       Method     int GetHashCode()
GetType                           Method     type GetType()
ToJsonString                      Method     string ToJsonString()
ToString                          Method     string ToString()
ToTeam                            Method     Microsoft.TeamsCmdlets.PowerShell.Custom.Model.Team ToTeam()
AllowAddRemoveApps                Property   System.Nullable[bool] AllowAddRemoveApps {get;set;}
AllowChannelMentions              Property   System.Nullable[bool] AllowChannelMentions {get;set;}
AllowCreateUpdateChannels         Property   System.Nullable[bool] AllowCreateUpdateChannels {get;set;}
AllowCreateUpdateRemoveConnectors Property   System.Nullable[bool] AllowCreateUpdateRemoveConnectors {get;set;}
AllowCreateUpdateRemoveTabs       Property   System.Nullable[bool] AllowCreateUpdateRemoveTabs {get;set;}
AllowCustomMemes                  Property   System.Nullable[bool] AllowCustomMemes {get;set;}
AllowDeleteChannels               Property   System.Nullable[bool] AllowDeleteChannels {get;set;}
AllowGiphy                        Property   System.Nullable[bool] AllowGiphy {get;set;}
AllowGuestCreateUpdateChannels    Property   System.Nullable[bool] AllowGuestCreateUpdateChannels {get;set;}
AllowGuestDeleteChannels          Property   System.Nullable[bool] AllowGuestDeleteChannels {get;set;}
AllowOwnerDeleteMessages          Property   System.Nullable[bool] AllowOwnerDeleteMessages {get;set;}
AllowStickersAndMemes             Property   System.Nullable[bool] AllowStickersAndMemes {get;set;}
AllowTeamMentions                 Property   System.Nullable[bool] AllowTeamMentions {get;set;}
AllowUserDeleteMessages           Property   System.Nullable[bool] AllowUserDeleteMessages {get;set;}
AllowUserEditMessages             Property   System.Nullable[bool] AllowUserEditMessages {get;set;}
Archived                          Property   System.Nullable[bool] Archived {get;set;}
Classification                    Property   string Classification {get;set;}
Description                       Property   string Description {get;set;}
DisplayName                       Property   string DisplayName {get;set;}
GiphyContentRating                Property   System.Nullable[Microsoft.TeamsCmdlets.PowerShell.Custom.Model.GiphyRatingType] GiphyCon...
GroupId                           Property   string GroupId {get;set;}
MailNickName                      Property   string MailNickName {get;set;}
ShowInTeamsSearchAndSuggestions   Property   System.Nullable[bool] ShowInTeamsSearchAndSuggestions {get;set;}
Visibility                        Property   string Visibility {get;set;}
```

So, under the hood, a lot of differences, and that is welcomed, why? because we will be able to have an alternative to the official Microsoft Teams PowerShell module in case this one runs short on something.

Let's try now some cmdlets that aren't available on the official Teams Module like `Get-PnPTeamsChannelMessage`


![getpnpteamschannelmessage](/images/pnpteams_getmessages.png)

Pretty neat, huh?

Hey, look at this cmdlet, `Submit-PnPTeamsChannelMessage`, can I post a message from PowerShell? Nice! Let's try it!

```powershell
Submit-PnPTeamsChannelMessage -Team "Sales and Marketing" -Channel General -Message "Developers, Developers, Developers!"
```
![getpnpteamschannelmessage](/images/pnpteams_postmessage.png)

Loved it!

That's all for part 1 of these series, in part 2, I'll keep exploring what these cmdlets can provide to Teams Administrators.