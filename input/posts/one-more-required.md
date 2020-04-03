Title: One or more required office components failed to complete successfully - Sharepoint
Published: 7/03/2016
Tags: SharePoint
---

This happened to me installing a Sharepoint SP1 Language Pack, but I think you can encounter this error with any Sharepoint setup operation.

If you face this error, there is a big chance that Setup is waiting for STSADM operations to conclude, but this never happens.
I checked the log file of the Language Pack installation, and I saw that the last operation was failing, and it was setup trying to run 

```cmd
STSADM updateproductinfo
```

with error _failed to return after 300000 seconds_ 

So, I started a command prompt and ran stsadm.exe updateproductinfo an let the command run, it lasted like 10 minutes... and it returned the command prompt (meaning no errors).

That's the process that was failing so if you recognize this as what is happening to you, you can continue safely.