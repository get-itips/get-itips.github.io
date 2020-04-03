Title: Connect to SQL using Powershell
Published: 6/28/2016
Tags: Powershell
---

#QuickTip

Often, when I request a database restore on customer, I do not have RDP to SQL Server... and I want to check if they mounted the database and gave me the desired permissions before I try to use that database on, for example, Lync or Sharepoint, so how can I do that without installing Management Studio or sql command line tools? Powershell to the rescue!

```Powershell
$conn = New-Object System.Data.SqlClient.SqlConnection
$conn.ConnectionString = "Data Source=SQL_Hostname;Initial Catalog=DATABASE_NAME;Integrated Security=SSPI;"
$conn.open()
```

If $conn.Open() throws a error, bingo, there is no database or permission to access it available.
But, if no error is thrown, you are connected! remember to disconnect with:

```Powershell
$conn.Close()
```