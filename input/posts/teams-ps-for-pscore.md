Title: Teams PowerShell module is now compatible with PowerShell Core!
Published: 6/25/2020
Tags: Teams
---

Is it?, well, let's see.

Support for PowerShell Core (AKA PowerShell 7) was apparently added silently to Microsoft Teams PowerShell module version since 1.0.6 (at the time of this writing the current stable module version is 1.0.7).

This image shows what the PowerShell gallery site says currently 

![PsGallery_Now](/images/teams_pscore_7.png)


and this is what it said for older versions.

![PsGallery_Older](/images/teams_pscore_6.png)

No big announcements AFAIK.

# Does it work?

## Connect-MicrosoftTeams

I usually use a PSCredential object to connect to every PowerShell module against Microsoft services, so I tried and this is what I got

![connectMsTeamsError](/images/teams_pscore_1.png)

Essentially, the error is:

```
Connect-MicrosoftTeams: password_required_for_managed_user: Password is required for managed user
```

It does not work when passing a PSCredential object as parameter for -Credential, apparently, this has something to do with Azure AD authentication libraries, more information [here](https://github.com/AzureAD/azure-activedirectory-library-for-dotnet/issues/1478).

I could report this to the Product Team and now they are aware of this behaviour and working on it, they told me to fallback to Device Auth:

![connectMsTeamsDeviceAuth](/images/teams_pscore_2.png)

a lot of logging outputted to the console window

![connectMsTeamsDeviceAuth](/images/teams_pscore_3.png)

but eventually connects

## Other cmdlets after connection

![connectMsTeamsDeviceAuth](/images/teams_pscore_4.png)

again, a lot of logging in the console, but once connected, everything seems to work the same as with Windows Powershell

![connectMsTeamsDeviceAuth](/images/teams_pscore_5.png)

This is a big step into modernizing Microsoft Teams PowerShell module, it will surely help to integrate the module to newer methods of automatization, from any Operating System, I salute the Product Team for all the hard work done, hopefully, the Credential error will be fixed soon.