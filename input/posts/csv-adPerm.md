Title: Powershell Script to CSV all AD Permissions for a particular Receive Connector
Published: 2/8/2017
Tags: ExchangeServer
---

Hi,

Had to do this PS Script for me to troubleshoot a receive connector permissions problem.
This will output to a CSV all permissions assigned to a receive connector, please change the $path and $connector variables to suit your needs  

 
```powershell
$path = "E:\path\dumpRights.csv"
$csv = "user,identity,deny,accessrights,extendedrights`r`n"
$connector="connectorName"
$perms=Get-adpermission $connector

foreach($perm in $perms){
    $perm
    $csv+=$perm.user.RawIdentity+ "," + $perm.identity.ToString() + "," + $perm.deny.IsPresent + "," + $perm.accessrights + "," + $perm.extendedrights
    $csv +="`n"
}

$fso = new-object -comobject scripting.filesystemobject
$file = $fso.CreateTextFile($path,$true)
$file.write($csv)
$file.close()
```                        
