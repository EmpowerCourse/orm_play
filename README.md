# orm_play
NHibernate and ActiveRecord comparison

# Setup the C#, NHibernate application
* Ensure .net core 2.0 is installed
* Ensure a version of SQL Server is installed
* Execute the NH/t-sql_atari.sql script against your local instance of SQL Server to create and populate the database
* Open the NH/NHPlay/NHPlay.csproj file in Visual Studio 2017
* Modify the CN_STR variable in NH/NHPlay/Program.cs to have a connection string valid for your local SQL Server instance

# Setup the Ruby, Rails, ActiveRecord application
* Ensure ruby is installed
* Ensure a version of PostgreSql is installed
* execute `rails new ar_play --database=postgresql`
