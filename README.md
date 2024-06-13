Backend Setup Guide

Dependencies

This backend project uses the following dependencies:

AutoMapper
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.AspNetCore.Authentication.OpenIdConnect
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.AspNetCore.Mvc.Testing
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.Identity.Web
Microsoft.Identity.Web.DownstreamApi
Moq
Pomelo.EntityFrameworkCore.MySql
Swashbuckle.AspNetCore
Swashbuckle.AspNetCore.Swagger
Swashbuckle.AspNetCore.SwaggerUI
xunit
xunit.runner.console
xunit.runner.visualstudio

(we used the latest version for all of them)

Database

Database: MariaDB
Jwt Key: Uses HS256 web token from a secret key

Install Dependencies

dotnet restore

Generate Database Schema (Migrations)

dotnet ef migrations add InitialCreate
dotnet ef database update

Testing

Unit tests are implemented using xUnit. Run tests with:
dotnet test

Run the Application
dotnet run
