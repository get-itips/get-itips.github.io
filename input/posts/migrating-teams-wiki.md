/add header

# Introduction

I know this is a long-time request by the community, that there isn't an out-of-the-box method to do this, I was asked
if I could find a way to migrate a Wiki tab from one channel in one team to another channel in another team.

I spent several days figuring it out how Teams handles wiki tabs, from creation to updating to removal, so I decided to share
my findings with the community, this is provided as-is, with no guarantee whatsoever.

# The Wiki tab

When a Teams Wiki tab is added to a Channel, the following is created on the SharePoint site belonging to the Team:

- A SharePoint Document library named _Teams Wiki Data_

https://contoso.sharepoint.com/sites/TeamName/Teams%20Wiki%20Data/Forms/AllItems.aspx

- A SharePoint List, like this, (but it is hidden by default)

https://contoso.sharepoint.com/sites/TeamName/Lists/19pYZDaUICINpaAq7iFZpRNyuEGeXK8gqb5yUC3ja4oc1threa

(The list gets its name from the Team Channel id)

Teams will also save this information about the tab and we can query it using Graph API:

HTTP Request

```http
https://graph.microsoft.com/v1.0/teams/2c009003-bf45-47ab-ac9d-fe4f3f3967f5/channels/19:Kb8nmcctoGWrOYfiB-Cf7wVgX8Lnk0UL8BH-WB6s7hQ1@thread.tacv2/tabs
```

```json
{
    "@odata.context": "https://graph.microsoft.com/v1.0/$metadata#teams('2c009003-bf45-47ab-ac9d-fe4f3f3967f5')/channels('19%3AKb8nmcctoGWrOYfiB-Cf7wVgX8Lnk0UL8BH-WB6s7hQ1%40thread.tacv2')/tabs",
    "@odata.count": 1,
    "value": [
        {
            "id": "6dd527de-ee8e-4f65-a389-ca8920adf3e0",
            "displayName": "Wiki",
            "webUrl": "https://teams.microsoft.com/l/channel/19%3aKb8nmcctoGWrOYfiB-Cf7wVgX8Lnk0UL8BH-WB6s7hQ1%40thread.tacv2/tab%3a%3a6dd527de-ee8e-4f65-a389-ca8920adf3e0?label=Wiki&groupId=2c009003-bf45-47ab-ac9d-fe4f3f3967f5&tenantId=3da121de-c5d8-452f-b8a4-4679df1d0f76",
            "configuration": {
                "entityId": null,
                "contentUrl": null,
                "removeUrl": null,
                "websiteUrl": null,
                "hasContent": false,
                "wikiTabId@odata.type": "#Int64",
                "wikiTabId": 1,
                "dateAdded": "2022-11-25T15:58:06.789Z"
            }
        }
    ]
}
```

See the one named wikiTabId? that's an important one and it is used by Teams to identify a specific Teams Wiki tab within
the channel, as you might know, we can have more than one Wiki per channel, and if we create 3 Wiki tabs, we should have 
wikiTabId 1, 2 and 3, those numbers are assigned incrementally, this wikiTabId is important because, it is referenced in the 
list I mentioned is created in the site, but we'll return to this later.

# Requirements

Install the latest PnP PowerShell module version, you can check here https://msshells.net, we will use it for several things.
I won't go into the details on how to use PnP PowerShell, there are [good resources](https://pnp.github.io/powershell/) for that.

# The scenario

- Two Team's teams, let's call them SourceTeam and DestinationTeam
- One wiki per team

# Procedure

The DestinationTeam has to have at least an empty Wiki, so it creates the _Teams Wiki Data_ DL for us and also the SharePoint list,
let's call these placeholder DL and List. So create an empty Wiki on DestinationTeam.

Then, use PnP PowerShell to connect to the source site:

```powershell
connect-pnponline -Interactive -Url https://contoso.sharepoint.com/sites/SourceTeam
```

Get the List name by calling Get-PnpList

```powershell
Get-PnpList
```
The one we are looking for is named something like this "19:26VHuEKoG5fV1UyOFtybCpQaJilwc7efl1erwiMDhCE1@thread.tacv2_wiki"

Get the template of the list and also add the data to it, customize the $list variable

```powershell
$template = ".\sourceTeamWiki.xml"
$list = "19:26VHuEKoG5fV1UyOFtybCpQaJilwc7efl1erwiMDhCE1@thread.tacv2_wiki"

Get-PnPSiteTemplate -Out $template -ListsToExtract $list -Handlers Lists

Add-PnPDataRowsToSiteTemplate -Path $template -List $list
```

Unhide the List with [Set-PnpList](https://pnp.github.io/powershell/cmdlets/Set-PnPList.html?q=set-pnplist)

Set-PnpList -



