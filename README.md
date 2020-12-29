# xe-ideas
An app to keep track of ideas


## First time set up
1. Set environment to development
```
$Env:ASPNETCORE_ENVIRONMENT = "Development"
```
1. Create a database in SQL Server name `xe_ideas`
1. Create a new user `xe_ideas_app` with the password `Password2` (or choose your own)
1. Update the database connection string
    1. Update `./src/xe-ideas/appsettings.Development.json`
    1. Change "DefaultConnection" to point to your local database
1. Build the project
```
cd ./src/xe-ideas/
donet build
```

1. Update the database schema
```
cd ./src/xe-ideas/
dotnet ef database update
```

## Running locally
1. Start React
```
cd ./src/xe-ideas/ClientApp/
npm start
```
1. Start backend server
    1. Press F5 in Visual Studio Code
    1. Or run the following command
```
dotnet run
```

## Running Unit test locally
```
cd tests/xe-ideas.Tests/
dotnet test
```