Title: M365 Enterprise Demo Customizer
Published: 7/26/2021
Tags: 365
---

Hello,

I am a heavy Microsoft 365 Enterprise Demos user (https://demos.microsoft.com), from Customer Digital Experiences (CDX - https://cdx.transform.microsoft.com/) you can recognize them as Microsoft assigns to them tenant domain names like this M365x######.OnMicrosoft.com.

There are plenty of tenant templates, EDU-content tenant names receive domain names like this M365EDU###### and so on, however, probably the most popular is the Microsoft 365 Enterprise Demo Content.

These demo tenants have two durations, 90 days or one year, I usually create a one-year duration demo tenant and two 90-day tenants.

The thing is, they expire, and with that, you have to start over customizing it with your needs, the thing that I do most is assigning administrative roles to different users, this helps me in my https://Docs.Microsoft.com collaborator/Pull Request reviewer job to test different permissions with different Azure AD built-in roles and also to test different scenarios before executing on customers.

So I decided to create a simple script, named it Customize-M365EnterpriseDemo.ps1, to assign different roles to the different users provided in the tenant template, decided to share the script to the community, maybe you find it useful, [the script is hosted in a GitHub repository](https://github.com/get-itips/M365EnterpriseDemoCustomizer), that way, anyone can easily file a bug or collaborate by adding some feature to it.

The script takes a .csv file as input, this file can be customized if you want or does not fit your demo tenant or needs, however, if you run it as-is, you will end up with:

- Allan Deyoung as Exchange and SharePoint admin
- Isaiah Langer as Teams Administrator
- Irvin Sayers as Teams Communication Support Specialist
- Lee Gu as Global Reader
- And so on...

I wanted all administrative users to have a license, with the provided licenses of the demo tenant, this means assigning more than one role to some users, so I tried to assign totally not-related roles when assigning more than one role to a user.

I hope you find it useful, this is still version 0.1, it surely needs more work and adjustments, contributions are very well welcomed!

Andrés