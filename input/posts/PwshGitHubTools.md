Title: It's like SysInternals, but for Git-Hub!
Published: 3/2/2019
Tags: GitHub
---

Hi!
(Well... If the great Mark Russinovich allows me... obviously) 
I am back! It's been a busy year, hopefully, I'll write more often.
Last year I started contributing in GitHub, more specifically as a Collaborator for docs.microsoft.com creating Pull Requests to improve the documentation based on user feedback.

Let me tell you that is an activity I really enjoy to do! I think Microsoft has made the right choice open-sourcing the documentation of their solutions.

So as I am a PowerShell guy (now a PowerShell Core guy) I created a small script to check the status of Issues.

The name of this Script is Get-GitHubIssuesState.ps1

Example Use

```powershell
PS > .\Get-GitHubIssuesState.ps1 -Owner MicrosoftDocs -Repo officedocs-SkypeForBusiness -Issues 834,817,803,779,777,772,771,774,663 -OAuthtoken x02ab2e5xxx34ef25fea707cfb02542e9d7xxx
```

**The reason behind needing OAuthToken is because of a limit in requests per hour that an anonymous user can make.**

The repo is located [here](https://github.com/get-itips/PwshGitHubTools) and I expect to create other small tools to do things with GitHub through PowerShell.

Remember, PowerShell makes you PowerFULL

