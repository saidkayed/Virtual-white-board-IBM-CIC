# Virtual-white-board-IBM-CIC

I have chosen to focus on the backend part of the case

Backend Framework: i have chosen to use ASP.NET Core Web API as the backend framework with EF6.


Database: MSSQL (relational database) is the database chosen, to store the Data

not only C# is my main language, other reasions is:
* Quick and Easy Setup with the framework
* EF6 easy Code-First approach (add-migration, update-database)
* built-in Swagger ui (for quick endpoint testing and visual documentation)
* backed by microsoft
* huge community

cons with it
* endpoints hard to unit-test if you dont do the repository design pattern approach.

# Setup Guide
* make sure microsoft sql server is installed i got: Microsoft SQL Server Management Studio 18
* Connection string matching your server name in appsetting.json. example: (localdb)\MSSQLLocalDB






