Title: New-RetentionCompliancePolicy and Teams
Published: 8/21/2019
Tags: Teams
---
![teamsSoHot](/images/mugatu.PNG)

There is a nice post [here](https://techcommunity.microsoft.com/t5/Microsoft-Teams-Blog/Retention-policies-for-Microsoft-Teams/ba-p/178011)
explaining how Retention policies for Microsoft Teams work, the intention of this blog post is not to talk about that but is kinda a 
quick cheat sheet if you happen to use the New-RetentionCompliancePolicy cmdlet for Teams.

New-RetentionCompliancePolicy includes four parameters related to Teams:

`TeamsChannelLocation`


_The TeamsChannelLocation parameter is used to target specific Teams to include in the policy_

`TeamsChannelLocationException`


_This parameter specifies the Teams to exclude when you use the value All for the TeamsChannelLocation parameter_

`TeamsChatLocation`


_The TeamsChatLocation parameter is used to target specific Teams users to include in the policy_

`TeamsChatLocationException`


_This parameter specifies the Teams users to exclude when you use the value All for the TeamsChatLocation parameter_


## Now, some examples:

### Example 1

```powershell
New-RetentionCompliancePolicy -Name "Example1" -TeamsChannelLocation "All" -TeamsChannelLocationException "32996b0b-67f6-4159-b4bf-f738d12a5f86"
```


In this example the Retention applies to all Teams except the Team with GUID 32996b0b-67f6-4159-b4bf-f738d12a5f86, we can only specify an
Exception when "All" is used for the location parameter.

### Example 2
```powershell
New-RetentionCompliancePolicy -Name "Example2" -TeamsChannelLocation "operations@M365x71714.onmicrosoft.com","systems@M365x71714.onmicrosoft.com"
```

In this example the Retention applies to the Teams operations and systems, as you can see, we are specifying the Teams with email address.


### Example 3
```powershell
New-RetentionCompliancePolicy -Name "Example3" -TeamsChatLocation "All" -TeamsChatLocationException
 "Allan Deyoung","Cameron White"
```


In this example the Retention applies to all users except Allan and Cameron, see that we are specifying them with their Name.


### Example 4
```powershell
New-RetentionCompliancePolicy -Name "Example4" -TeamsChatLocation "41f18a1c-2e21-4c6c-bca9-3b51a8d18a80"
```


In this example the Retention applies only to user with id 41f18a1c-2e21-4c6c-bca9-3b51a8d18a80, see that we are specifying them with their id
we can get this id thru Azure AD.

We can specify Teams with their name, email, guid and users with their name, email, DN and guid.

## One more thing...
One thing I noticed, is that, policies created using Powershell can't be edited later using the Office 365 Security & Compliance portal, hope
Microsoft fix this soon.
Also, if you use [Get-RetentionCompliancePolicy](https://docs.microsoft.com/en-us/powershell/module/exchange/policy-and-compliance-retention/get-retentioncompliancepolicy?view=exchange-ps) 
to retrieve the newly created policies, you will notice that these attributes will be empty:


```powershell
TeamsChatLocation             : {}

TeamsChatLocationException    : {}

TeamsChannelLocation          : {}

TeamsChannelLocationException : {}
```


Reason for that is that you need to use the -DistributionDetail parameter with it:


```powershell
TeamsChatLocation             : {Debra Berger, Allan Deyoung}
TeamsChatLocationException    : {}
TeamsChannelLocation          : {}
TeamsChannelLocationException : {}
```


Hope this is useful for somebody.
