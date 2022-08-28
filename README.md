# Accounts Balance Viewer API

## Prerequisites

* Install .NET Core SDK 6.0.400.
* Install PostgreSQL 14.5

## Migrations script

Run `dotnet-ef migrations script --project AccountsBalanceViewer.API` to generate schema migrations script.

## Run on local environment

1. Create a PostgreSQL database with the name `AccountsDB`.
2. Restore the database dump in `./dump` folder into the created database.
3. Copy the connection string and set it into the `ConnectionStrings:AccountsDB` section in `appsettings.json` file. 
4. Run `dotnet run --project AccountsBalanceViewer.API` to spin up the dev server. Navigate to `https://localhost:7029` or `http://localhost:5072`.

## Build

Run `dotnet build` to build the projects. The build artifacts of each project will be stored in their respective directories as follows.
* `AccountsBalanceViewer.API` -> `AccountsBalanceViewer.API/bin/Debug/net6.0/AccountsBalanceViewer.API.dll`
* `AccountsBalanceViewer.UnitTests` -> `AccountsBalanceViewer.UnitTests/bin/Debug/net6.0/AccountsBalanceViewer.UnitTests.dll`

## Running unit tests

* Run `dotnet test` to execute the unit tests in local environment.
* Commit / Place a Pull Request to the `main` branch. GitHub Actions will trigger and execute the unit
  tests [![Build and Test](https://github.com/yousuf-git/accounts-balance-viewer-api/actions/workflows/build-test.yml/badge.svg)](https://github.com/yousuf-git/accounts-balance-viewer-api/actions/workflows/build-test.yml).

## Deployment

Run the workflow in GitHub Actions [![Build, Test and Deploy](https://github.com/yousuf-git/accounts-balance-viewer-api/actions/workflows/build-test-deploy.yml/badge.svg)](https://github.com/yousuf-git/accounts-balance-viewer-api/actions/workflows/build-test-deploy.yml).

* Production host: `https://accounts-balance-viewer-api-core.azurewebsites.net`
* Health check: `https://accounts-balance-viewer-api-core.azurewebsites.net/debug/ping`