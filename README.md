# Virtual-white-board-IBM-CIC

I have chosen to focus on the backend part of the case

Backend Framework: i have chosen to use ASP.NET Core Web API as the backend framework with EF6.


Database: MSSQL (relational database) is the database chosen, to store the Data

not only C# is my main language, other reasions is:
* Quick and Easy Setup with the framwork
* EF6 easy Code-First approach (add-migration, update-database)
* built-in Swagger ui (for quick endpoint testing)
* backed by microsoft
* huge community

cons with it
* endpoints hard to unit-test if you dont do the repository design pattern approach.

# Setup Guide (tomorrow sunday i will see i can make it easyer for u guys by having it in a container on docker instead)
* make sure microsoft sql server is installed i got: Microsoft SQL Server Management Studio 18
* Connection string matching your server name in appsetting.json. example: (localdb)\MSSQLLocalDB





