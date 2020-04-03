Title: Copy IP Block List from one Server to Another
Published: 6/14/2016
Tags: ExchangeServer
---

If you have a large list of blocked IP address on Exchange Server 2010 (when you use Antispam Agents) you may know that this lists are particular of each Hub Transport Server, so when you for some reason change the default HT used as incoming connections you will lose this configuration, so how can we copy the IPs from one server to another?

I made this script just for that, is very simple and also very specific, it is only to copy single addresses from one server to another, but if you need IP ranges you'll figure that out and customize it.

Here we go:

```Powershell
$source = sourceserver.domain.tld
$dest = destinationserver.domain.tld

$entries  = get-ipblocklistentry -Server $source

foreach($e in $entries){
 $ip = $e.IPRange.LowerBound.ToString();
 Add-IPBlockListEntry -IPAddress $ip -Server $dest
}
```