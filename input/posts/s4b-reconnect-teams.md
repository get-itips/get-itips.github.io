Title: Enable-CsOnlineSessionForReconnection... on Teams PowerShell 1.1.6
Published: 9/17/2020
Tags: Teams
---
![introimage](/images/TeamsS4BReconnect.png)

Microsoft has just released [Teams PowerShell module version 1.1.6](https://www.powershellgallery.com/packages/MicrosoftTeams/1.1.6), a version that includes the Skype for Business Online Connector built-in, so, it is a requirement to uninstall it, if you had it installed on your computer.

Thanks to a great [Alexander Holmeset](https://twitter.com/AlexHolmeset)'s heads-up, that identified that a very handy PowerShell cmdlet called  **Enable-CsOnlineSessionForReconnection**, was not anymore present, I decided to investigate if we could get the functionality back, more information on this cmdlet on this [excellent blog post](https://ucstatus.com/2019/11/25/skypeonlineconnector-session-reconnection/) by Randy Chapman, but essentially, it allows you to reconnect the Skype For Business session automatically when it time-outs after an hour.

So I downloaded the Skype for Business Online Connector bits on another computer and tried to find where was it, finally found it inside C:\Program Files\Common Files\Skype for Business Online\Modules\SkypeOnlineConnector\SkypeOnlineConnectorStartup.psm1 and decided to give it a try "as is", but it threw some errors and did not worked, as expected, so I realized I had to make some adjustments to it, specifically the way it recognizes if the module is already loaded.

Here is the script, based on the original function, save it as Enable-CsOnlineSessionForReconnection.ps1

```powershell
$global:CsOnlineSessionInputParams=$UserCredential
$modules = Get-Module tmp_*
$csModuleUrl = "/OcsPowershellOAuth"
$isSfbPsModuleFound = $false;

    foreach ($module in $modules)
    {
        [string] $moduleUrl = $module.Description
        [int] $queryStringIndex = $moduleUrl.IndexOf("?")

        if ($queryStringIndex -gt 0)
        {
            $moduleUrl = $moduleUrl.SubString(0,$queryStringIndex)
        }

        if ($moduleUrl.Contains($csModuleUrl))
        {
            $isSfbPsModuleFound = $true
            & $module { ${function:Get-PSImplicitRemotingSession} = `
            {
                param(
                    [Parameter(Mandatory = $true, Position = 0)]
                    [string]
                    $commandName
                )

                if (($script:PSSession -eq $null) -or ($script:PSSession.Runspace.RunspaceStateInfo.State -ne 'Opened'))
                {
                    Set-PSImplicitRemotingSession `
                        (& $script:GetPSSession `
                            -InstanceId $script:PSSession.InstanceId.Guid `
                            -ErrorAction SilentlyContinue )
                }
                if (($script:PSSession -ne $null) -and ($script:PSSession.Runspace.RunspaceStateInfo.State -eq 'Disconnected'))
                {
                    # If we are handed a disconnected session, try re-connecting it before creating a new session.
                    Set-PSImplicitRemotingSession `
                        (& $script:ConnectPSSession `
                            -Session $script:PSSession `
                            -ErrorAction SilentlyContinue)
                }
                if (($script:PSSession -eq $null) -or ($script:PSSession.Runspace.RunspaceStateInfo.State -ne 'Opened'))
                {
                    Write-PSImplicitRemotingMessage ('Recreating a new remote powershell session (implicit) for command: "{0}" ...' -f $commandName)

                    if ((Test-Path variable:global:CsOnlineSessionInputParams) -ne $true)
                    {
                        throw 'Unable find input parameters from global scope, will not be able to recreate session'
                    }

                    if ((Test-Path variable:global:CsOnlineSessionRetryAttempt) -ne $true)
                    {
                        $global:CsOnlineSessionRetryAttempt = 1
                    }
                    else
                    {
                        $global:CsOnlineSessionRetryAttempt = $global:CsOnlineSessionRetryAttempt + 1
                    }

                    $session = New-CsOnlineSession -Credential @global:CsOnlineSessionInputParams

                    if ($session -ne $null)
                    {
                        Set-PSImplicitRemotingSession -CreatedByModule $true -PSSession $session
                    }

                    #note - this magic string has to be same as above, search for this string above, it will become clear
                    #because this will be in callback handler, I am not putting this into const variable
                    $sfbPsSessionPrefix = "SfBPowerShellSession_"
                    #sessions originally created will have the below one
                    $sfbPsSessionRegEx1 = $sfbPsSessionPrefix + "*"
                    #sessions created later will get their name changed by powershell during Set-PSImplicitRemotingSession
                    #and so it will have names like "Session for implicit remoting module at C:\Users\<user>\AppData\Local\Temp\tmp_tsdvdhga.20e\tmp_tsdvdhga.20e.psm1"
                    #tmp_tsdvdhga.20e being the module name
                    $sfbPsSessionRegEx2 = Get-PSImplicitRemotingModuleName
                    $sfbPsSessionRegEx2 = "*" + $sfbPsSessionRegEx2 + "*"
                    #delete broken sessions - begin
                    $psBroken = Get-PSSession | where-object {($_.Name -like $sfbPsSessionRegEx1 -or $_.Name -like $sfbPsSessionRegEx2) -and $_.State -like "*Broken*"}
                    $psClosed = Get-PSSession | where-object {($_.Name -like $sfbPsSessionRegEx1 -or $_.Name -like $sfbPsSessionRegEx2) -and $_.State -like "*Closed*"}

                    $psBroken | Remove-PSSession;
                    $psClosed | Remove-PSSession;
                    #delete broken sessions - end
                }
                if (($script:PSSession -eq $null) -or ($script:PSSession.Runspace.RunspaceStateInfo.State -ne 'Opened'))
                {
                    throw 'No session has been associated with this implicit remoting module'
                }

                return [Management.Automation.Runspaces.PSSession]$script:PSSession
            }}
        }
    }
```

and use it like this

```powershell
$userCredential = Get-Credential
#Connect to Microsoft Teams
Connect-MicrosoftTeams -Credential $credential

$sfbSession = New-CsOnlineSession -Credential $userCredential
Import-PsSession $sfbSession

.\Enable-CsOnlineSessionForReconnection.ps1
```

That should be enough for it to make it work as before, I tested it and it worked for me, please be aware that this is provided as is without any guarantee, let me know if it works four you!