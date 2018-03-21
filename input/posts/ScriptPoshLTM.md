Title: Script to call POSH-LTM-Rest cmdlets to enable or disable Pool Members on a F5 LTM Load Balancer
Published: 3/21/2018
Tags: TeamFoundationServer
---

Hi,

Had to do this PS Script for a customer, they use TFS and wanted to control F5 LTM (Load Balancer) to disable or enable nodes in a release definition,
so they can disable a node, update the binaries on it, and enable it again.
So, I found this excellent [Powershell modules](https://github.com/joel74/POSH-LTM-Rest) from [Joel74](https://github.com/joel74) that include the needed functions.
First of all, you have to install the POSH-LTM modules as the creator says on his github page on the computer that will be used to run the PS Script,
then, save this PS Script on the same computer.

 
```powershell
 param (
    [Parameter(Mandatory=$true)][string]$LTMName,
    [Parameter(Mandatory=$true)][string]$Partition,
    [Parameter(Mandatory=$true)][string]$PoolName,
    [Parameter(Mandatory=$true)][string]$MemberName,
    [switch]$disable = $false,
    [Parameter(Mandatory=$true)][string]$F5Username,
    [Parameter(Mandatory=$true)][string]$F5Password
)

$passwd = $F5Password
$user=$F5Username

$secpasswd = ConvertTo-SecureString $passwd -AsPlainText -Force
$ltmCreds = New-Object System.Management.Automation.PSCredential ($user, $secpasswd)


#Se solicita una sesion al F5 BIG-IP
$session = $null
$session = New-F5Session -LTMCredentials $ltmCreds -LTMName $LTMName -PassThrough

if($session -ne $null){
    if($Disable){
    Write-Verbose "Trying to disable $LTMName|$Partition|$PoolName|$MemberName"
    Disable-PoolMember -F5Session $session -PoolName $PoolName -Partition $Partition -Name $MemberName

    }else{
    Write-Verbose "Trying to enable $LTMName|$Partition|$PoolName|$MemberName"
    Enable-PoolMember -F5Session $session -PoolName $PoolName -Partition $Partition -Name $MemberName
    }
}
```                   

Go to the release definition of your project of choice, and add a "Powershell on target Machines" task, fill the fields as follows:

- Machines: fill with the hostname or IP of the computer with the PS Module and PS Script saved in the last step.
- Admin Login / Password: Credentials to connect to this computer
- Protocol: HTTP or HTTPS
- Powershell Script: Full path to the PS Script saved in the last step, for example C:\Scripts\F5Manage-TeamMember.ps1
- Script Arguments: Arguments needed by my script, for example, using release variables:

-LTMName $(LTMName) -Partition $(Partition) -PoolName $(PoolName) -MemberName $(MemberName) -F5Username $(Username) -F5Password $(Password)


Remember to create this variables in the release definition.

**Test it. always test it on your environment.**
