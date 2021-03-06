BEGIN TRANSACTION;
--
-- Extension: Platformus.Configurations
-- Version: alpha-21
--
INSERT INTO public."Configurations" ("Id","Code","Name") VALUES (1,'Email','Email');
INSERT INTO public."Configurations" ("Id","Code","Name") VALUES (2,'Globalization','Globalization');
INSERT INTO public."Variables" ("Id","ConfigurationId","Code","Name","Value","Position") VALUES (1,1,'SmtpServer','SMTP server','test',1);
INSERT INTO public."Variables" ("Id","ConfigurationId","Code","Name","Value","Position") VALUES (2,1,'SmtpPort','SMTP port','25',2);
INSERT INTO public."Variables" ("Id","ConfigurationId","Code","Name","Value","Position") VALUES (3,1,'SmtpLogi','SMTP logi','test',3);
INSERT INTO public."Variables" ("Id","ConfigurationId","Code","Name","Value","Position") VALUES (4,1,'SmtpPassword','SMTP password','test',4);
INSERT INTO public."Variables" ("Id","ConfigurationId","Code","Name","Value","Position") VALUES (5,1,'SmtpSenderEmail','SMTP sender email','test',5);
INSERT INTO public."Variables" ("Id","ConfigurationId","Code","Name","Value","Position") VALUES (6,1,'SmtpSenderName','SMTP sender name','test',6);
INSERT INTO public."Variables" ("Id","ConfigurationId","Code","Name","Value","Position") VALUES (7,2,'SpecifyCultureInUrl','Specify culture in URL','yes',1);

--
-- Extension: Platformus.Security
-- Version: alpha-21
--
INSERT INTO public."Users" ("Id","Name","Created") VALUES (1,'Administrator','2017-01-01 00:00:00.000000');
INSERT INTO public."CredentialTypes" ("Id","Code","Name","Position") VALUES (1,'Email','Email',1);
INSERT INTO public."Credentials" ("Id","CredentialTypeId","Identifier","Secret","UserId") VALUES (1,1,'admin@platformus.net','21-23-2F-29-7A-57-A5-A7-43-89-4A-0E-4A-80-1F-C3',1);
INSERT INTO public."Roles" ("Id","Code","Name","Position") VALUES (1,'Administrator','Administrator',100);
INSERT INTO public."Roles" ("Id","Code","Name","Position") VALUES (2,'ApplicationOwner','Application owner',200);
INSERT INTO public."Roles" ("Id","Code","Name","Position") VALUES (3,'ContentManager','Content manager',300);
INSERT INTO public."UserRoles" ("UserId","RoleId") VALUES (1,1);
INSERT INTO public."UserRoles" ("UserId","RoleId") VALUES (1,2);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (1,'BrowseBackend','Browse backend',1);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (2,'DoEverything','Do everything',100);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (3,'BrowseConfigurations','Browse configurations',200);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (4,'BrowsePermissions','Browse permissions',300);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (5,'BrowseRoles','Browse roles',310);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (6,'BrowseUsers','Browse users',320);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (7,'BrowseFileManager','Browse file manager',400);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (8,'BrowseCultures','Browse cultures',500);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (9,'BrowseObjects','Browse objects',600);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (10,'BrowseDataTypes','Browse data types',610);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (11,'BrowseClasses','Browse classes',620);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (12,'BrowseEndpoints','Browse endpoints',630);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (13,'BrowseMenus','Browse menus',700);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (14,'BrowseForms','Browse forms',800);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (15,'BrowseViews','Browse views',900);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (16,'BrowseStyles','Browse styles',910);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (17,'BrowseScripts','Browse scripts',920);
INSERT INTO public."Permissions" ("Id","Code","Name","Position") VALUES (18,'BrowseBundles','Browse bundles',930);
INSERT INTO public."RolePermissions" ("RoleId","PermissionId") VALUES (1,1);
INSERT INTO public."RolePermissions" ("RoleId","PermissionId") VALUES (2,2);
INSERT INTO public."RolePermissions" ("RoleId","PermissionId") VALUES (3,7);
INSERT INTO public."RolePermissions" ("RoleId","PermissionId") VALUES (3,9);
INSERT INTO public."RolePermissions" ("RoleId","PermissionId") VALUES (3,13);
INSERT INTO public."RolePermissions" ("RoleId","PermissionId") VALUES (3,14);

--
-- Extension: Platformus.Globalization
-- Version: alpha-21
--
INSERT INTO public."Cultures" ("Id","Code","Name","IsNeutral","IsDefault") VALUES (1,'__','Neutral',TRUE,FALSE);
INSERT INTO public."Cultures" ("Id","Code","Name","IsNeutral","IsDefault") VALUES (2,'en','English',FALSE,TRUE);

--
-- Extension: Platformus.Domain
-- Version: alpha-21
--
INSERT INTO public."DataTypes" ("Id","StorageDataType","JavaScriptEditorClassName","Name","Position") VALUES (1,'string','singleLinePlainText','Single line plain text',1);
INSERT INTO public."DataTypes" ("Id","StorageDataType","JavaScriptEditorClassName","Name","Position") VALUES (2,'string','multilinePlainText','Multiline plain text',2);
INSERT INTO public."DataTypes" ("Id","StorageDataType","JavaScriptEditorClassName","Name","Position") VALUES (3,'string','html','Html',3);
INSERT INTO public."DataTypes" ("Id","StorageDataType","JavaScriptEditorClassName","Name","Position") VALUES (4,'string','image','Image',4);
INSERT INTO public."DataTypes" ("Id","StorageDataType","JavaScriptEditorClassName","Name","Position") VALUES (5,'datetime','date','Date',5);

--
-- Extension: Platformus.Forms
-- Version: alpha-21
--
INSERT INTO public."FieldTypes" ("Id","Code","Name","Position") VALUES (1,'TextBox','Text box',1);
INSERT INTO public."FieldTypes" ("Id","Code","Name","Position") VALUES (2,'TextArea','Text area',2);
INSERT INTO public."FieldTypes" ("Id","Code","Name","Position") VALUES (3,'DropDownList','Drop down list',3);
INSERT INTO public."FieldTypes" ("Id","Code","Name","Position") VALUES (4,'FileUpload','File upload',4);
INSERT INTO public."DataTypeParameters" ("Id","DataTypeId","JavaScriptEditorClassName","Code","Name") VALUES (1,1,'checkbox','IsRequired','Is required');
INSERT INTO public."DataTypeParameters" ("Id","DataTypeId","JavaScriptEditorClassName","Code","Name") VALUES (2,1,'numericTextBox','MaxLength','Max length');
INSERT INTO public."DataTypeParameters" ("Id","DataTypeId","JavaScriptEditorClassName","Code","Name") VALUES (3,2,'checkbox','IsRequired','Is required');
INSERT INTO public."DataTypeParameters" ("Id","DataTypeId","JavaScriptEditorClassName","Code","Name") VALUES (4,2,'numericTextBox','MaxLength','Max length');
INSERT INTO public."DataTypeParameters" ("Id","DataTypeId","JavaScriptEditorClassName","Code","Name") VALUES (5,4,'numericTextBox','Width','Width');
INSERT INTO public."DataTypeParameters" ("Id","DataTypeId","JavaScriptEditorClassName","Code","Name") VALUES (6,4,'numericTextBox','Height','Height');
INSERT INTO public."DataTypeParameters" ("Id","DataTypeId","JavaScriptEditorClassName","Code","Name") VALUES (7,5,'checkbox','IsRequired','Is required');

COMMIT;
