Title: Enabling Teams Shared Channels 101
Published: 4/27/2022
Tags: Teams
---

#Background

Teams Shared Channels is a great new feature that is still in Public Preview, the goal of this post is to create a concise guide to follow in order to enable Shared Channels in your Tenant 

# Azure AD Cross Tenant Access Policies 

These are the basis of Teams Shared Channels and we must begin configuring them in order to enable Teams Shared Channels later. 

1. Browse to https://aad.portal.azure.com/  
2. Click on **Azure Active Directory** 
3. Click on **External Identities**
4. Click on **Cross-Tenant access settings (preview)** 
5. Click **Add Organization**, this would be the information of the other organization we would like to interact with. 
6. Enter the **Tenant ID or domain name**, if you use the latter, it is in the form of tenantname.onmicrosoft.com 
7. It should resolve the Name and Tenant ID, click **Add**
8. A new entry should be added on the list and a default of "**Inherited from default**" in **Inbound Access** and **Outbound access** columns should be displayed. 
9. We are going to customize per organization instead of using the defaults, but you can also customize the defaults. Click on the **Inherited from default** in the **inbound access** column 
10. Click **B2B Direct connect** 
11. Click **Customize settings** 
12. Verify **Allow Access** and **All External users and groups** are selected (Although it can be customized to scope to certain users and groups), and Click **Save**
13. Click **Applications** and verify **Allow Access** and **all applications** are selected (although it can be customized to certain applications), click **Save** if you had to modify any setting here.
14. Go back to the **External identities** screen clicking on **External Identities**
15. Inbound access is configured when the direction of the collaboration is ToMyTenancy, in other words when we want to invite someone from the other organization to a Shared channel created on our tenant. 
16. If you want your users to be able to participate in the other organization’s shared channels, configure the same settings under inherited from default in Outbound access column. 

**Note:** This configuration can be viewed with the Graph PowerShell cmdlet **Get-MgPolicyCrossTenantAccessPolicy** (Microsoft.Graph.Identity.SignIns) and the Exchange Online PowerShell cmdlet **Get-CrossTenantAccessPolicy**, although not yet so user-friendly.

Check this great resources in case you want to understand more about these policies:

https://www.michev.info/Blog/Post/3681/cross-tenant-access-policy-xtap-and-the-graph-api
https://practical365.com/cross-tenant-access-policies/
https://docs.microsoft.com/en-us/azure/active-directory/external-identities/b2b-direct-connect-overview
https://docs.microsoft.com/en-us/azure/active-directory/external-identities/cross-tenant-access-settings-b2b-direct-connect

# Teams Admin Center 

## Teams policies 

1. Browse to https://admin.teams.microsoft.com/ 
2. Click on **Teams – Teams policies**
3. Click on the **Global (org-wide default)**
4. Verify that **Create shared channels** and **Join external shared channels** is **enabled** 
5. You can also create a new policy if you don’t want to use the Global one 


## Teams update policies 

1. Click on **Teams – Teams update policies** 
2. Click on the **Global (org-wide default)**
3. Verify that under the **Show preview features** combo, **Enabled** is selected 
4. Click **Apply** if necessary. 

## Teams client 

1. Click the **…** in the Teams desktop client 
2. Select **About – Public Preview version** 
3. Accept the agreement 
4. You should now be able to create a Shared Channel 

**Note:** Is not necessary to load Teams using the Public Preview version to later interact with the Channel once it is created. 
