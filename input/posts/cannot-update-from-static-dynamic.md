Title: Public IP address is in use by ipconfig and cannot be updated from static to dynamic.
Published: 5/5/2021
Tags: Azure
---

Hello, maybe my first blog post specifically about Azure, do not know if this is the best way to fix this error however I couldn't find many results on Internet.
Somehow, do not know how, an IP Address associated to one of my Virtual Machines in Azure was configured as static (I do not remember doing that, maybe when I was playing with the DNS name associated I made the mistake), so when I tried to change it back to dynamic I received:

> Public IP address /subscriptions/bd0d2034-4212-436e-9516-xxxxxx/resourceGroups/onpremlab/providers/Microsoft.Network/publicIPAddresses/onpremlabvm01-ip is in use by ipconfig /subscriptions/bd0d2034-4212-436e-9516-xxxxxx/resourceGroups/onpremlab/providers/Microsoft.Network/networkInterfaces/onpremlabvm01189/ipConfigurations/ipconfig1 and cannot be updated from static to dynamic.

As everything is an object in Azure, "ipconfig1" was the IP Configuration object belonging to the Network Interface of that VM.

So what I did was to open the Network interface by going to the Network Interfaces blade in Azure, located the one of that VM and clicked on IP Configurations and obviously saw the static IP Address of the external interface listed there under "Public IP Address", clicked on it and you can either click "Disassociate", save the changes or create a new public IP address from this panel.

If you click disassociate, you need to then go to Public IP addresses blade, locate it and change it to dynamic, now you should be able to do it. Remember to then, go back to the IP Configuration and re-associate the Public IP address object to the IP Configuration.

Please let me know if you have a better solution! :)