Title: The specified module 'Microsoft.SharePoint.MigrationTool.PowerShell' was not loaded because no valid module file was found in any module directory
Published: 7/17/2019
Tags: Sharepoint
---

Hi,

Hopefully, this is only useful temporarily until it is fixed in the installation package, but currently, if you install the [Sharepoint Migration Tool](https://docs.microsoft.com/en-us/sharepointmigration/introducing-the-sharepoint-migration-tool)
module files will be copied to `$env:UserProfile\Documents\WindowsPowerShell\Modules\Microsoft.SharePoint.MigrationTool.PowerShell` as the [documentation](https://docs.microsoft.com/en-us/sharepointmigration/overview-spmt-ps-cmdlets) says.

But if you try to import the module, you will receive an error:

```powershell
PS C:\Users\user\Documents\WindowsPowerShell\Modules\Microsoft.SharePoint.MigrationTool.PowerShell> Import-Module Mi
crosoft.SharePoint.MigrationTool.PowerShell
Import-Module : The specified module 'Microsoft.SharePoint.MigrationTool.PowerShell' was not loaded because no
valid module file was found in any module directory.
At line:1 char:1
+ Import-Module Microsoft.SharePoint.MigrationTool.PowerShell
+ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : ResourceUnavailable: (Microsoft.Share...Tool.PowerShell:String) [Import-Module], File
   NotFoundException
    + FullyQualifiedErrorId : Modules_ModuleNotFound,Microsoft.PowerShell.Commands.ImportModuleCommand
```

Simply copy the `Microsoft.SharePoint.MigrationTool.PowerShell` folder in `C:\Program Files\WindowsPowerShell\Modules` and you will be able to import it.

