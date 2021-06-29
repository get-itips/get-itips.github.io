Title: PnP Teams Cmdlets Review - Part II
Published: 06/29/2021
Tags: Teams
---

# Introduction
Back in October 2020 I made a review of PnP Teams cmdlets, you can find that review [here](https://get-itips.capazero.net/posts/teams-pnp).
However, that review is now outdated as PnP project had a major change in the architecture, now being built in .NET Core 3.1 / .NET Framework 4.6.1 and, hence, supporting PowerShell core, so because of that fact, let's make another review to see what has changed.

# Requirements

## PnP Module Version

Be sure to be running the latest stable version of the PnP PowerShell module, to this date, that is 1.6.0.

also, we can check, which commands are currently available for Teams related tasks running

```powershell
Get-Command -Module *PnP* -Name "*Teams*"
```

```powershell
CommandType     Name                                               Version    Source
-----------     ----                                               -------    ------
Cmdlet          Add-PnPTeamsChannel                                1.6.0      PnP.PowerShell
Cmdlet          Add-PnPTeamsTab                                    1.6.0      PnP.PowerShell
Cmdlet          Add-PnPTeamsTeam                                   1.6.0      PnP.PowerShell
Cmdlet          Add-PnPTeamsUser                                   1.6.0      PnP.PowerShell
Cmdlet          Get-PnPTeamsApp                                    1.6.0      PnP.PowerShell
Cmdlet          Get-PnPTeamsChannel                                1.6.0      PnP.PowerShell
Cmdlet          Get-PnPTeamsChannelMessage                         1.6.0      PnP.PowerShell
Cmdlet          Get-PnPTeamsTab                                    1.6.0      PnP.PowerShell
Cmdlet          Get-PnPTeamsTeam                                   1.6.0      PnP.PowerShell
Cmdlet          Get-PnPTeamsUser                                   1.6.0      PnP.PowerShell
Cmdlet          New-PnPTeamsApp                                    1.6.0      PnP.PowerShell
Cmdlet          New-PnPTeamsTeam                                   1.6.0      PnP.PowerShell
Cmdlet          New-PnPTenantSequenceTeamSite                      1.6.0      PnP.PowerShell
Cmdlet          Remove-PnPTeamsApp                                 1.6.0      PnP.PowerShell
Cmdlet          Remove-PnPTeamsChannel                             1.6.0      PnP.PowerShell
Cmdlet          Remove-PnPTeamsTab                                 1.6.0      PnP.PowerShell
Cmdlet          Remove-PnPTeamsTeam                                1.6.0      PnP.PowerShell
Cmdlet          Remove-PnPTeamsUser                                1.6.0      PnP.PowerShell
Cmdlet          Set-PnPTeamsChannel                                1.6.0      PnP.PowerShell
Cmdlet          Set-PnPTeamsTab                                    1.6.0      PnP.PowerShell
Cmdlet          Set-PnPTeamsTeam                                   1.6.0      PnP.PowerShell
Cmdlet          Set-PnPTeamsTeamArchivedState                      1.6.0      PnP.PowerShell
Cmdlet          Set-PnPTeamsTeamPicture                            1.6.0      PnP.PowerShell
Cmdlet          Submit-PnPTeamsChannelMessage                      1.6.0      PnP.PowerShell
Cmdlet          Sync-PnPAppToTeams                                 1.6.0      PnP.PowerShell
Cmdlet          Update-PnPTeamsApp                                 1.6.0      PnP.PowerShell
```

Seems the same list of cmdlets that were available at the time I made the first part of my review, no new cmdlets.

## Graph API Permissions

To run any of those cmdlets, we will need special Graph API Permissions, thankfully, those are documented at Microsoft Docs, for example for the `Get-PnPTeamsTeam` cmdlet, we will need either Group.Read.All or Group.ReadWrite.All.

## Connecting to PnP Online

If we try to use any *PnPTeams* cmdlet without the required permissions we will receive an error like this:

```PowerShell
Connect-PnPOnline: AADSTS65001: The user or administrator has not consented to use the application with ID '31359c7f-bd7e-475c-86db-fdb8c937548e' named 'PnP Management Shell'. Send an interactive authorization request for this user and resource.
Trace ID: 185aa11a-cb90-4434-8aee-6c5538a3d800
Correlation ID: d72ec6f1-75a5-4ccb-9d97-c537a4c59f1a
Timestamp: 2021-06-29 17:38:00Z
```

To understand how to connect it is better to read the source information from the PnP team [here](https://pnp.github.io/powershell/articles/connecting.html) as the process changed, for example `-Scopes` parameter is not there anymore in `Connect-PnpOnline`.

# The Teams PnP cmdlets

Back in the Part I, I showed the output of `Get-PnPTeamsTeam` and `Get-PnPTeamsChannelMessage` and an example use of `Submit-PnPTeamsChannelMessage`, they haven't changed much so let's try other cmdlets

## Set-PnPTeamsChannel

The `IsFavoriteByDefault` boolean parameter described in the [documentation](https://docs.microsoft.com/powershell/module/sharepoint-pnp/set-pnpteamschannel?view=sharepoint-ps) got my attention, however, I receive an error trying to use it:

```PowerShell
Set-PnPTeamsChannel: Failed to execute backend request.
```

An [issue](https://github.com/pnp/powershell/issues/563) is already created in the PnP GitHub repository, so eventually it will be fixed.

If you try to change any property of a built-in Channel, like General, it will fail with:

```PowerShell
Set-PnPTeamsChannel: General channel cannot be patched.
```

Besides that, I was able to run:

```PowerShell
set-PnPTeamsChannel -Team "Contoso" -Identity "CustomChannel" -DisplayName "Custom Channel"
```

On a custom made channel.


## Add-PnPTeamsTab

Let's test this interesting cmdlet to add a custom tab of type "WebSite" to a Teams Channel

```PowerShell
Add-PnPTeamsTab -Team "Contoso" -Channel "Custom Channel" -DisplayName "MSShells" -Type WebSite -ContentUrl "https://msshells.net"
```

This worked great, adding a web site to a Teams channel.

# Conclussions

PnP Teams cmdlets didn't change so much since Part I of these series, it would be interesting to see more cmdlets and more parameters in next releases however it is fantastic to have a community alternative to the official Teams module, the same way we have an alternative to the official SharePoint Online module.
