Title: How to limit download rate on Ignite Videos
Published: 10/6/2016
Tags: Ignite
---

If you are using [THE powershell](http://tomtalks.uk/2015/05/download-all-the-skype-for-business-ignite-sessions/) script to download Ignite 2016 Videos and Slide Decks you probably noticed that it will try to use all your bandwidth available and maybe kill all your connections, so, if this happened to you, you will find this useful

Hack into the ps file, go to line 156:

```powershell
Start-Process -FilePath .\youtube-dl.exe -ArgumentList "-o ""$vidFullFile""", $url -NoNewWindow -Wait
```
and replace with this
```powershell
Start-Process -FilePath .\youtube-dl.exe -ArgumentList "-o ""$vidFullFile""","--limit-rate 30K" , $url -NoNewWindow -Wait
```
customize the --limit-rate argument to suit your preferences and voila!
