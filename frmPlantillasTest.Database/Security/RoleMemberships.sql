EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'CPDsa';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'LH\SQLSilh';


GO
EXECUTE sp_addrolemember @rolename = N'db_accessadmin', @membername = N'CPDsa';


GO
EXECUTE sp_addrolemember @rolename = N'db_securityadmin', @membername = N'CPDsa';


GO
EXECUTE sp_addrolemember @rolename = N'db_ddladmin', @membername = N'CPDsa';


GO
EXECUTE sp_addrolemember @rolename = N'db_backupoperator', @membername = N'CPDsa';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'CPDsa';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'CPDsa';

