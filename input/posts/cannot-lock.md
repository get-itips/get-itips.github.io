Title: Error cannot lock ref Git fetch failed with exit code 1 unable to update local ref
Published: 19/3/2018
Tags: TeamFoundationServer
---

Hi,

Had to troubleshoot this error on one of my customers and wanted to share so if anyone encounter this can use this solution.
Basically, when executing a Build using TFS Build Agents, we were seeing this error

```
2018-09-17T19:10:10.9599867Z  * [new branch]        ABZ-Branch -> origin/ABZ-Branch
2018-09-17T19:10:10.9599867Z error: cannot lock ref 'refs/remotes/origin/Abz-Branch': is at 551a179d8451f1154e0b38b85993a439d8c39af2 but expected e887cf2f4e449bd480560b8aee827bf140faf65a
2018-09-17T19:10:10.9599867Z  ! e887cf2f..087dcca9  Abz-Branch -> origin/Abz-Branch  (unable to update local ref)
2018-09-17T19:10:11.0381160Z ##[error]Git fetch failed with exit code: 1
```

Strange huh? if you do a search you won't find exactly this error, on similar situations we can recommendations to do a git prune, but I did this with no luck.

So, I saw something, as you can see in the extracted log, there is ABZ-Branch and Abz-Branch, they might seem the same branch but they are different in case!!

TFS, at least 2017 update 3 is not preventing you from creating branches with the same name but different case.

The Git install where the Build agents are, is not making difference of this, so it is not case sensitive, (as digging into Agent's work folder at for example
E:\Agents\Agent01\_work\2\s\.git\config file you can see there that ignorecase = true

so it is processing the git fetch as if ABZ-Branch and Abz-Branch were the same! leading  to the error saying that it is expecting other digest.

So if you encounter exactly this, check if you have two branches with the same name but different case. 
