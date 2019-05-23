Title: Export TFS Release Definitions to a CSV file
Published: 5/23/2019
Tags: TeamFoundationServer
---

Hi,

I created a simple small script in Powershell to query Team Foundation Server to get a list of all Release Definitions per project, the
output of the script is a csv file with this header

```
"Project;ReleaseDefinition"
```

It uses the REST API to query for this information and if the project does not have release definition it will appear with a NULL value on
the ReleaseDefinition column, only one time of course.

### Input parameters
It asks for FQDN, CollectionName and Path for the CSV file.

### Current limitations
It was only tested against Team Foundation Server 2017, I expect to improve it including more versions.
Have to change the way the FQDN is asked, as http is currently hard-coded.

Full script can be found here [Export-TFSReleaseDefinitions.ps1](https://github.com/get-itips/AzureDevopsTools/blob/master/azuredevopstools-ps/Export-TFSReleaseDefinitions.ps1)
