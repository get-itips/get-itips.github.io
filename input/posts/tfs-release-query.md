Title: Team Foundation Server SQL Query to get Releases and approvals
Published: 9/25/2019
Tags: TeamFoundationServer
---

![tfssql](/images/sqltfs.PNG)

Hi,

I created a SQL query to be run against Team Foundation Server collection database to get a list of all releases per project.
TFS Collection database is not documented at all, so I had to hack into it to understand the relationship of the tables.

Essentially when running this Query you will get this columns:

| Column | Meaning  |
|---|---|
|  [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].[Id] | Id of the Step inside the environment   |
| [Tfs_Collection].[dbo].[tbl_Projects].project_name | Project that this release belongs to  |
| [Tfs_Collection].[Release].[tbl_Release].Name  | Release version name  |
| [Tfs_Collection].[Release].[tbl_Release].[Description]  | Description of the Release  |
| [Tfs_Collection].[dbo].[ADObjects].DisplayName  | Account that created the release  |
|  [Tfs_Collection].[Release].[tbl_Release].CreatedOn | Creation Date  |
|  [Tfs_Collection].[dbo].[ADObjects].DisplayName (A) | Group Approver resultant from a join  |
|  [Tfs_Collection].[dbo].[ADObjects].DisplayName (B) | Real Approver resultant from a join  |
| [Tfs_Collection].[Release].[tbl_ReleaseEnvironment].Name  | Environment Name, for example DEV  |
|  [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].[ApproverComment] | Approver comment for the release  |
| [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].[CreatedOn]  | Date that the step was started  |
| [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].[ModifiedOn]  | Date that the step was last modified  |

## The query itself

```sql
	       SELECT [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].[Id]
             [Tfs_Collection].[dbo].[tbl_Projects].project_name AS 'Project'
         ,[Tfs_Collection].[Release].[tbl_Release].Name as 'Version'
         ,replace(replace([Tfs_Collection].[Release].[tbl_Release].[Description],char(10),''),char(13),'') AS 'Description'
         ,C.DisplayName AS 'Creator'
         ,[Tfs_Collection].[Release].[tbl_Release].CreatedOn as 'CreationDate'
      ,A.DisplayName AS 'ApproverGroup'
      ,B.DisplayName AS 'Approver'
         ,[Tfs_Collection].[Release].[tbl_ReleaseEnvironment].Name AS 'Environment'
      ,[ApproverComment] as 'ApproverComment'
      ,[Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].[CreatedOn]  as 'StepCreationDate'
      ,[Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].[ModifiedOn] as 'StepModDate'
  FROM [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep]
  INNER JOIN [Tfs_Collection].[dbo].[ADObjects] A
  ON A.TeamFoundationId = [ApproverId]
  INNER JOIN [Tfs_Collection].[dbo].[ADObjects] B
  ON B.TeamFoundationId = [ActualApproverId]

  INNER JOIN [Tfs_Collection].[Release].[tbl_Release]
  ON [Tfs_Collection].[Release].[tbl_Release].[ID] = [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].ReleaseID
  AND [Tfs_Collection].[Release].[tbl_Release].[PartitionID] = [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].PartitionID
AND [Tfs_Collection].[Release].[tbl_Release].[DataspaceId] = [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].DataspaceId

  INNER JOIN [Tfs_Collection].[dbo].[ADObjects] C
  ON C.TeamFoundationId = [Tfs_Collection].[Release].[tbl_Release].[CreatedBy]
    
    INNER JOIN [Tfs_Collection].[Release].[tbl_ReleaseEnvironment]
  ON [Tfs_Collection].[Release].[tbl_ReleaseEnvironment].Id = [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].ReleaseEnvironmentId
AND [Tfs_Collection].[Release].[tbl_ReleaseEnvironment].DefinitionEnvironmentId = [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].DefinitionEnvironmentId
   AND [Tfs_Collection].[Release].[tbl_ReleaseEnvironment].[PartitionID] = [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].PartitionID
     AND [Tfs_Collection].[Release].[tbl_ReleaseEnvironment].[DataspaceId] = [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].DataspaceId
      AND  [Tfs_Collection].[Release].[tbl_ReleaseEnvironment].[ReleaseId] = [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].ReleaseID

   INNER JOIN [Tfs_Collection].[dbo].[tbl_Dataspace]
  on [Tfs_Collection].[dbo].[tbl_Dataspace].DataspaceId  = [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].DataspaceId

   INNER JOIN [Tfs_Collection].[dbo].[tbl_Projects] ON
    [Tfs_Collection].[dbo].[tbl_Projects].Project_Id = [Tfs_Collection].[dbo].[tbl_Dataspace].DataspaceIdentifier

  
 and [Tfs_Collection].[Release].[tbl_ReleaseEnvironment].Name  IN ('PROD', 'TEST','DEV')

   where 
   [Tfs_Collection].[Release].[tbl_Release].CreatedOn > = @FromDate 
   and
   [Tfs_Collection].[Release].[tbl_Release].CreatedOn < = @ToDate


   order by [Tfs_Collection].[Release].[tbl_ReleaseEnvironmentStep].[CreatedOn] 
```

You will have to pass as parameters the @fromDate and @toDate to tailor the result and please take into account the enviroment names which are 
hard-coded in this query.

Hope you find it useful!
