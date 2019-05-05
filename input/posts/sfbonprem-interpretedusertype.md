Title: SfB On-Prem decommission and InterpretedUserType
Published: 5/5/2019
Tags: Skype4B
---
![msRTCSip-DeploymentLocator](/images/DeploymentLocator.png)

Hi!
I was involved in a project with my colleage [Dario Woitasen](https://twitter.com/dariomws) to remove Skype for Business from an hybrid deployment with SfB Online and experienced some odd things that we had to deal with.
I want to share them if anyone experiences the same as we did.

As a start, we followed this excellent guide [Decommissioning Skype/Lync on-premises](https://blogs.technet.microsoft.com/fasttracktips/2017/06/27/decommissioning-skypelync-on-premises/) but, as usual, as we progressed we saw that there were more things to take into account.


One of the steps includes running this cmdlet for all users

**Disable-CsUser**

Well, this cmdlet should:

- Clear all msRTCSip- attributes of every enabled user.

But actually:

- Did not fully clear the msRTCSip- attributes, specially msRTCSip-DeploymentLocator, more info on this attrib [here](https://docs.microsoft.com/en-us/skypeforbusiness/schema-reference/active-directory-schema-extensions-classes-and-attributes/schema-attributes-and-descriptions)
- Removed sip:user@contoso.com from proxyAddresses attribute!

This lead to two things:

- **Single sign-on stopped working for the SfB client**, later, we identified that this was caused by the sip:user@contoso.com entry not being present in proxyAddresses (remember that Disable-CsUser deleted this?), so we created a script to fix this in local AD domain:

```powershell
$csv=import-csv "D:\Users.csv"

foreach($user in $csv){
        $userUpn=$user.SipAddress.Replace("sip:","")
        #Do not add proxyAddress if it is present already
        $proxies=Get-ADUser -Filter "UserPrincipalName -eq '$userupn'" -properties * | Select-Object @{Name=“proxyAddresses”;Expression={$_.proxyAddresses}}


        if(($null -eq $proxies) -or ($proxies.proxyAddresses -join ' ' -notmatch "sip:")) #empty list of ProxyAddresses
        {
            write-host "Adding sip"
            Get-ADUser -Filter "UserPrincipalName -eq '$userupn'" | Set-ADUser -Add @{"proxyAddresses"=$user.SipAddress}
        }
        else
        {
            write-host "No need to Add sip"
        }
}
```

Look that this is a portion of the real script, that is reading a csv file with the output of users that need to be treated, csv has a column for the User's Sip Address exported from SfB Online Powershell, like this:


```c
"SipAddress"
"sip:debrab@contoso.com"
"sip:johnd@contoso.com"
```

Once we added this attribute to the local AD, *single sign-on worked again*

Where were we? oh yes... the presence of values in msRTCSip-DeploymentLocator attribute in the local Active Directory lead to the SfB Online deployment (after Azure AD Sync) to "think" or "interpret" that the users were still hybrid or OnPrem and this prevented (or at least we feared) to remove the last on-prem servers.
We concluded this looking at the InterpretedUserType of the users and in the process, we identified all of these possible values for  an SfB Online user account:

```c
DirSyncDisabledDisabledUser
DirSyncDisabledNoService
DirSyncDisabledSfBUser
DirSyncDisabledUser
DirSyncNoService
DirSyncSfBUser
DirSyncSfBUserNeedsProvisioning
HybridOnlineDisabledUser
HybridOnlineDisabledUserNeedsProvisioning
HybridOnlineDisabledUserWithDeletedLicenses
HybridOnlineSfBUser
HybridOnlineSfBUserWithDeletedLicenses
HybridOnpremDisabledUser
HybridOnpremSfBUser
PureOnlineNoService
PureOnlineNoServiceWithDeletedLicenses
PureOnlineSfBUser
```

We are not sure of the complete enumeration of this type of data, but as you may see, the list is big, this excellent post tries to explain some of them [InterpretedUserType to the rescue for Hybrid Deployments](http://www.be-com.eu/?p=3286)

## InterpretedUserType

**We could not find official documentation about the exact meaning of every value, so I will try to match them to other values I observed, so please use this at your own risk, if I found out that some of this values is incorrect or get any official documentation I will update this**

- **DirSyncDisabledDisabledUser**: User account disabled in SfB online and disabled in local AD, has a sipAddress attribute online, RegistrarPool has a value
- **DirSyncDisabledNoService**: User account disabled in SfB online and does not have a sipAddress attribute online, RegistrarPool empty, OnPremHostingProvider shows sipfed.online.lync.com
- **DirSyncDisabledSfBUser**: User account disabled in SfB online and has a sipAddress attribute online, also, has a RegistrarPool value and OnPremHostingProvider is empty
- **DirSyncDisabledUser**: User account disabled in SfB online and disabled in local AD, does not have a RegistrarPool value.
- **DirSyncNoService**: To this day, I still do not know exactly what that means.
- **DirSyncSfBUser**: Enabled in SfB Online, has a SipAddress and has a RegistrarPool, this is the best value we can get.
- **DirSyncSfBUserNeedsProvisioning**: This appears to be a temporal state before DirSyncSfBUser
- **HybridOnlineDisabledUser**: User account disabled in SfB online, has a SipAddress attribute online and OnPremHostingProvider is not empty.
- **HybridOnlineDisabledUserNeedsProvisioning**: User account disabled in SfB online, hasn't a SipAddress attribute online and OnPremHostingProvider is not empty.
- **HybridOnlineDisabledUserWithDeletedLicenses**: It's like HybridOnlineDisabledUser but, it seems that license is not assigned.
- **HybridOnlineSfBUser**: User account enabled and DirSynched, has a SipAddress and a OnPremHostingProvider and RegistrarPool.
- **HybridOnlineSfBUserWithDeletedLicenses**: Same as HybridOnlineSfBUser but judging by the name, it seems that license is not present.
- **HybridOnpremDisabledUser**: User account disabled in SfB OnPrem, OnPremHostingProvider has "SRV:" value and RegistrarPool is empty.
- **HybridOnpremSfBUser**: According to Johan Delimon's Blog, the account is created at Customer AD, DirSynched and uses SfB OnPrem, we can judge this by the presence of OnPremHostingProvider
- **PureOnlineNoService**: I still do not know exactly what that means.
- **PureOnlineNoServiceWithDeletedLicenses**: User account created in Office 365, has no local AD account, so is not DirSynched, has no SfB license.
- **PureOnlineSfBUser**: User account created in Office 365, has no local AD account, so is not DirSynched, and uses SfB Online.

So, as I said, *Disable-CsUser* did not clear the *msRTCSIP-DeploymentLocator* value, this lead to having most of the users in an undesired state. OnPremHostingProvider is an Online attribute that is synced with it, for more information, see [Skype Online user is disabled, although Skype Online license assigned!](http://toregj.blogspot.com/2017/03/skype-online-user-is-disabled-although.html) so we first cleared all the msRTCSIP- attributes in local AD for a user and waited for sync... when synced happened, we observe the attribute InterpretedUserType changing for example from HybridOnlineSfbUser to DirSyncSfBUserNeedsProvisioning until it went to the final state DirSyncSfBUser, but for this, we had to wait hours.

So, we created an script to clear all msRTCSIP- for every user in local Active Directory domain:

```powershell
#ok lets clear msRTCSIP-* Attributes
        write-host "Clearing msRTCSIP-* Attributes"
        Get-ADUser -Filter "UserPrincipalName -eq '$userupn'" | Set-ADUser -clear msRTCSIP-ApplicationOptions,msRTCSIP-DeploymentLocator,msRTCSIP-Line,msRTCSIP-OwnerUrn,msRTCSIP-PrimaryUserAddress,msRTCSIP-UserEnabled,msRTCSIP-OptionFlags
```

(I recommend running this cmdlet against the same Domain Controller using the -Server parameter)

That's it, we waited some hours and started to see the expected value for the users (DirSyncSfBUser), then, we followed the guide and removed the last servers and ran Disable-CsAdDomain and Disable-CsAdForest.

Hope this post helps anyone that stumble upon the same scenario as Dario and me.get
