CashVisionPOS
=============

Sample POS application made with WPF and MVVMLight to help developers who want to learn these technologies.

Application made with VS2012 and using SQL Server 2012.

To start using the app just need to do the following:
- Navigate to POS/sqlscripts and run the scripts:
  - PosDb01052014A (database and tables creation)
  - PosDbInitialization1 (Initial database configuration with some general data, could be changed according your needs)
  - PosDbDevInitProcedures (Stored procedure which creates some demo products)
- Change the PosDbContext connection string for the app.config file (in the CV.POS.Wpf and CV.POS.Data projects) including your SQLServer instance, the database name should be the same as the default one.
<add name="PosDbContext" connectionString="metadata=res://*/PosModel.csdl|res://*/PosModel.ssdl|res://*/PosModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=YOURSERVER;initial catalog=PosDb;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />

And that's all, if you face any issue with the application running please feel free to contact me.
