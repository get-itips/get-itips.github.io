Title: How to export Azure AD Connect metaverse using sqlcmd
Published: 9/6/2022
Tags: Azure
---

# Introduction

The Synchronization Service Manager app is good to review the status and check the configuration of synchronization to and from an Azure AD tenant,
and it also includes a Metaverse Search option where you can query the SQL database that Azure AD Connect uses.
One of the things that is recommended in a swing migration is to compare the number of objects between installations, however this UI does not handle correctly 
a big number objects, I found that if you click _Search_ on the _Metaverse Search_ option, with 50,000 items or more, and then you try to copy all the results (I do this to 
understand which accounts will not be part of the new installation, hence, deleted from Azure AD), the UI will hang.

# Analysis

So I needed a way to query the objects but using the command line, I didn't want to install SQL Management Studio on the Azure AD Connect Server.

First of all, we need to get the named pipes connection string, (these instructions are if using the LocalDB version of SQL Server's Azure AD Connect, otherwise, if using an standalone SQL
Server you would already know the server name, instance name and protocol).

# The Solution?
SQL Server's Azure AD Connect uses named pipes as protocol, so we need to take that into account, open the error.log file that is present in 

```
C:\Users\[Account Running Microsoft Azure AD Sync Service]\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\ADSync2019
```

also found on this path

```
C:\Windows\ServiceProfiles\ADSync\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\ADSync2019
```

find a line similar to:

```
Server local connection provider is ready to accept connection on [ \\.\pipe\LOCALDB#SHEA4A65\tsql\query ].
```

take note of the whole pipe \\.\pipe\LOCALDB#SHEA4A65\tsql\query (it differs from installation to installation, yours will be different)

then, open a command prompt and run this command, making the required adjustments

```cmd
sqlcmd -S np:\\.\pipe\LOCALDB#SHEA4A65\tsql\query -d "ADSync" -E -Q "select displayName FROM [ADSync].[dbo].[mms_metaverse]" -o "ExportAADC.txt" -h-1 -w 200
```
This command connects to the specified named pipe (-S np:\\.\pipe\LOCALDB#SHEA4A65\tsql\query), against the specified database (-d "ADSync"), runs the specified T-SQL
(-Q "select displayName FROM [ADSync].[dbo].[mms_metaverse]") and produces the specified output text file (-o "ExportAADC.txt")
This command will produce a text file with the output of the query, unfortunately, a lot of white lines will be present on the file, you can remove them easily using

```
Edit -> Line operations -> Remove Empty Lines (Containing Blank characters)
```

option from Notepad++.

# Conclusion

This method is useful if you do not want to add any other piece of software in the server, which it is usually not recommended or easy in customer's installations, sqlcmd comes with the Azure AD Connect installation and is right there.


