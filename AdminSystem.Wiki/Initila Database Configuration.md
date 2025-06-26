# #Todo after SQL Server database restore

- DB permission as db owner
- Run below sql script to enable broker for sql table dependency:

```
USE [master]
GO
ALTER DATABASE <DB_Name> SET ENABLE_BROKER with rollback immediate
GO

```
