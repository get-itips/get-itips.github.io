Title: The Data Collector Set or one of its dependencies is already in use
Published: 8/3/2021
Tags: Windows
---

I was troubleshooting a high CPU usage of lsass.exe process in some Domain Controllers and I found this article [How to troubleshoot high Lsass.exe CPU utilization on Active Directory Domain Controllers](https://docs.microsoft.com/troubleshoot/windows-server/identity/troubleshoot-high-lsass.exe-cpu-utilization).

In that article, it recommends to run the Active Directory Data Collector, when I tried to do it I got

> The Data Collector Set or one of its dependencies is already in use.

I checked if it was already running, any Data Collector, but they were all stopped, I also found this event in **Microsoft** > **Windows** > **Diagnosis-PLA** > **Operational**:

> Data collector set system\Active Directory Diagnostics failed to start as DOMAIN\DCACCOUNT$ with error code 0x803000AA.

On an internet search I found multiple things, restart the server, look for running data collectors, I tried everything I found as a possible fix with no luck, until I noticed a pattern, all DCs where I couldn't run the said Data Collector were running PaloAlto Cortex, I also found some Data Collectors that seemed to belong to the product in Startup Event Trace Sessions under System Data Collector sets in Performance Monitor, and in particular, XdrAgentLog was enabled.

## Resolution
I asked the customer to disable the Cortex agent and I could succesfully run the Active Directory Data Collector, we later re-enabled it and will probably open a support ticket to find a way not to do that while we need to run the data collector.

Publishing this in case anyone faces the same, hope anyone find this useful. 