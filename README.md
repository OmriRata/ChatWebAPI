# ChatWebAPI


# Welcmoe to our ChatWebApi Program!

# In this program  we will use SqlLite as the Database server. 



we will use the Pomelo Entity Framework. 
It is available as a NuGet package here: https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql
You can simply open the package manager (PM) console
(In Visual Studio 2022 -> View -> Other Windows -> Package Manager Console).

And enter the following command:
"Install-Package Pomelo.EntityFrameworkCore.MySql -Version 6.0.1"
(*without the quotation marks*)

Then, install the Microsoft Entity Framework Tools package
by entering the following command:
"Install-Package Microsoft.EntityFrameworkcore.Tools -version 6.0.1"
Then, install the Microsoft AspNetCore Authentication JwtBearer
by entering the following command:
"Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 6.0.1"
# Install SqLite:
Then, install the  Microsoft EntityFrameworkCore Sqlite 
by entering the following command:
"Install-Package Microsoft.EntityFrameworkCore.Sqlite -Version 6.0.1"
# we are using Data base file called : chat.db
(Which is already in the current folder in order to get a non-empty database )

Finally create the database:
	1) First create a migration by typing: "add-migration InitialMigration" 
	   in the PM console. 
	2) Then apply the migration using: "update-database"

Note: We are working with .NET version 6.0.1 (same as the university server).
	  Please make sure it is installed. . 
