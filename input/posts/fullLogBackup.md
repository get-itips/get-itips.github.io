Title: The transaction log for database 'Sharepoint_Config is full due to 'LOG_BACKUP'
Published: 8/4/2018
Tags: Sharepoint
---

Well, if you or your customer doesn't have a good maintenance plan for your Sharepoint DB (Content & Configuration) you will someday face a situation where Sharepoint_Config transaction log grows tremendously or even fills your transaction log disk, so what to do?

Note: if you can't do any operation on the RDBMS (SQL Server) because the disk is full, you need either delete something on the disk (maybe a non important testing database) or expand the disk (if you are working on a virtual machine this is relatively easy).

Once you free enough space on the disk, you must to this in order:

- Set Recovery Mode for the Sharepoint_Config database to SIMPLE
- Shrink the Transaction Log FILES of the db
- Set Recovery Mode for the Sharepoint_Config database to FULL
- Take a Full Database Backup! 
- How to do the first 3 steps using T-SQL?

```sql
USE master;
ALTER DATABASE SharePoint_Config SET RECOVERY SIMPLE;
GO

USE [SharePoint_Config]
GO
DBCC SHRINKFILE (N'SharePoint_Config_log' , 1)
GO

USE master;ALTER DATABASE SharePoint_Config SET RECOVERY FULL;
```

Once you do this, you will see that Sharepoint Config database will shrink to a better reasonable size, but, it will grow again eventually so, my advice, create a maintenance job to do this before taking a full database backup.
