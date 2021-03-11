Title: HAFNIUM Exchange Server exploits - Resources
Published: 3/10/2021
Tags: ExchangeServer
---

Some days had passed since the public Microsoft announcement about this HAFNIUM exploit and as I have been asked to step to help several customers I would like to list all the resources I used to work on this matter, I will try to update this list with new content as it is being generated.

3/10/2021: Added two more resources.

# Microsoft resources

- [Microsoft Security Blog entry](https://www.microsoft.com/security/blog/2021/03/02/hafnium-targeting-exchange-servers/)

Probably the official **entry point** to understand the exploit and should be the initial procedure to take against this vulnerability.

- [Script to check if your Exchange has been compromised](https://github.com/microsoft/CSS-Exchange/tree/main/Security)

Not only contains a script that you must execute to understand if your Exchange logs show suspicious entries (Suspicious activity found in % log!), **EVEN IF YOU APPLIED THE PATCHES**, but also a mitigation script to apply if for some reason you are unable to install the Security Patches.

- [MSERT Safety Scanner tool](https://docs.microsoft.com/en-us/windows/security/threat-protection/intelligence/safety-scanner-download)

This tool was updated to detect web shells that could have been left by attackers, not enough, but a must-run tool.

# Other resources

- An excellent [MSxFAQ.de blog post](https://www.msxfaq.de/exchange/update/hafnium-exploit.htm). (In German)

Frank Carius always produces great content, this time is sharing a very complete review of the vulnerability, also with a recommended workflow to follow.

- A very [interesting analysis of attacking behavior based on log entries and W3WP memory dumps](https://www.crowdstrike.com/blog/falcon-complete-stops-microsoft-exchange-server-zero-day-exploits/). 

(I am not affiliated, associated, authorized, endorsed by, or in any way officially connected with this solution)

- A [Basic Timeline by Krebs on Security](https://krebsonsecurity.com/2021/03/a-basic-timeline-of-the-exchange-mass-hack/)

An excellent compilation of events since the discovery of this vulnerability.

- [Practical 365 - Attack on Exchange Servers Gives Impetus to Move Email to the Cloud](https://practical365.com/blog/attack-exchange-impetus-move-cloud/)

- [How To Rebuild your Exchange Server (AFTER HAFNIUM INFECTION)](https://jaapwesselius.com/2021/03/09/rebuild-your-exchange-server-after-hafnium-infection/)




