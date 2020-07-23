# HOW TO UPDATE

```
dotnet ef --startup-project ../tools/BikeDataProject.DB.Tools.Setup/ database update
```

# HOW TO ADD A NEW MIGRATION

```
dotnet ef --startup-project ../tools/BikeDataProject.DB.Tools.Setup/ migrations add YourMigrationName
```

# DEPLOY PACKAGE

## Make a release of the package
```
dotnet pack -c release
```

## Deploy it to GitHub Packages
```
dotnet nuget push "bin/Release/bikedataproject.database.models.package.{VERSION_NUMBER}.nupkg" --source "github"
```
