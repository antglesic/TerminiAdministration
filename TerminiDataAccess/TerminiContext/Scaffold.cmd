@echo off

cd ..\..

dotnet build .\TerminiAPI\TerminiAPI.csproj

rmdir /Q /S .\TerminiDataAccess\TerminiContext\Models
del .\TerminiDataAccess\TerminiContext\TerminiContext.cs

dotnet ef dbcontext scaffold Name=Default Microsoft.EntityFrameworkCore.SqlServer ^
-s .\TerminiAPI ^
-p .\TerminiDataAccess ^
--schema Termini ^
--context TerminiContext ^
--context-dir .\TerminiContext ^
--output-dir .\TerminiContext\Models ^
--no-pluralize ^
--force ^
--no-build ^
--no-onconfiguring


rmdir /Q /S .\TerminiDataAccess\TerminiContext\Compiled
dotnet build .\TerminiAPI\TerminiAPI.csproj

dotnet ef dbcontext optimize ^
-s .\TerminiAPI ^
-p .\TerminiDataAccess ^
--context TerminiContext ^
--output-dir .\TerminiContext\Compiled ^
--namespace TerminiDataAccess.TerminiContext ^
--no-build

pause
