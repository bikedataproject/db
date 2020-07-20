# HOW TO UPDATE

```
dotnet ef --startup-project ../console-project/ database update
```

# HOW TO ADD A NEW MIGRATION

```
dotnet ef --startup-project ../console-project/ migrations add YourMigrationName
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
