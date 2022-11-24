Title: Using Conditional Access Policies with Microsoft Teams Rooms devices - Part 1 
Published: 11/24/2022
Tags: Teams
---

#Introduction

Upon customer request, I needed to investigate how to restrict the resource account used by a Microsoft Teams Rooms device, the goal was to prevent this account from logging in from other devices than the MTR device, I think that is not so crazy request, right?

In this blog post, that I hope it becomes a blog series, I want to share my findings about this, and also thank [Randy Chapman](https://twitter.com/randychapman), [Linus Cansby](https://twitter.com/LCansby), [Graham Walsh](https://twitter.com/thegrahamwalsh) for providing some more information. 

According to Microsoft, _Filter for devices_, which is a _Conditional Access Policy_ condition, is **supported** in Microsoft Teams Rooms on Android and on Windows. See here [Supported Conditional Access and Intune device compliance policies for Microsoft Teams Rooms and Teams Android Devices](https://learn.microsoft.com/microsoftteams/rooms/supported-ca-and-compliance-policies?tabs=mtr-w) 

However, we also found this statement 

> You can't use a resource account to apply device-level conditional access policies in Azure Active Directory and Endpoint Manager as device info is not passed when using this grant type. 

[Authentication in Microsoft Teams Rooms on Windows - Microsoft Teams | Microsoft Learn](https://learn.microsoft.com/microsoftteams/rooms/rooms-authentication)

So if we read both those pages, it is a little contradictory, right? Is it or is it not supported to use _filter for devices_? 

So, upon testing, I found out that Microsoft Teams Rooms devices based on windows **do not send the device information on the sign-in event** of the Microsoft Resource Account (Room Mailbox), see here an example: 

![TMR Windows Sign In event](/images/MTR_Windows_SignIn.png) 

Decided to give it a try [Azure AD Registering the device](https://support.microsoft.com/en-us/account-billing/register-your-personal-device-on-your-work-or-school-network-8803dd61-a613-45e3-ae6c-bd1ab25bf8a8), also with no luck. Then, I received confirmation from Microsoft that the device needs to be AADJ (Azure AD Joined/Enrolled) to send this information, I still could verify this but I hope I can do it for my 2nd blog post of these series. 

So, that leaves me with the MTRoA devices, the idea, was to create a Conditional Access Policy like this: 

- **Users**: The specific resource account 
- **Cloud Apps**: All cloud apps (or at least, Exchange Online, Microsoft Teams, and SharePoint Online as explained here [Conditional Access and compliance best practices for Microsoft Teams Rooms - Microsoft Teams | Microsoft Learn](https://learn.microsoft.com/en-us/microsoftteams/rooms/conditional-access-and-compliance-for-devices)) 
- **Conditions** : **Filter for Devices**: _Exclude filtered devices_, Configure: Yes, Exclude filtered devices from policy: deviceId Equals e592fe64-fd2b-4ced-ae96-91657183cdb8 
- **Grant**: Block access 

The policy should be read like this: _Block resource account from logging in from any device but the ones that match the condition of deviceId_ 

As said, this unfortunately din’t work with a MTRoW, as-is or Azure AD Registered, but what about Microsoft Teams Rooms on Android? 

These devices showed as Azure AD Registered by default and upon testing, they send the device information upon sign-in.

![TMR Android Sign In event](/images/MTR_Android_SignIn.png) 

So, we could use CAPs with Android-based MTR devices without any other intervention (like AADJ/Enrolling, see https://techcommunity.microsoft.com/t5/intune-customer-success/enrolling-microsoft-teams-rooms-on-windows-devices-with/ba-p/3246986 ) 

Hope these findings are useful and saves time for someone, as this took me a good couple of days to find out and test.

I expect to continue sharing my findings with the community in a 2nd part of this blog post.
