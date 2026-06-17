---
name: clean-architecture-net
description: >
  Bootstrap a new .NET (C#) solution following Clean Architecture with 4 layers:
  Domain, Application, Infrastructure, Api. Creates the solution, projects, project
  references in the correct dependency direction, and base folders/files.
  Use when the user says "nuevo proyecto clean architecture", "crear solución clean
  architecture", "scaffold clean architecture", "start a clean architecture project",
  or asks to set up a layered .NET backend.
---

# Clean Architecture .NET — bootstrap

Create a .NET solution with 4 layers. Dependency rule: dependencies point **inward**.
`Domain` depends on nothing. `Api` is the composition root.

```
Api ──► Application ──► Domain
 │                        ▲
 └──► Infrastructure ─────┘
```

## Before you start

Ask the user (skip any already given):
1. **Solution name** (e.g. `Rifa`). Used for folder + `.sln`.
2. **Target framework** — default `net10.0` unless they say otherwise. Verify with
   `dotnet --version` if unsure.
3. **DB / ORM** — EF Core? provider (SqlServer/Postgres/Sqlite)? Or none yet.

Confirm name + framework before running `dotnet new`.

## Layer responsibilities

| Layer | SDK | Depends on | Holds |
|-------|-----|-----------|-------|
| `Domain` | `Microsoft.NET.Sdk` | — (nothing) | Entities, value objects, domain events, enums, domain exceptions, repository **interfaces** |
| `Application` | `Microsoft.NET.Sdk` | `Domain` | Use cases (commands/queries), DTOs, interfaces for infra (e.g. `IEmailSender`), validation, mapping |
| `Infrastructure` | `Microsoft.NET.Sdk` | `Application`, `Domain` | EF Core `DbContext`, repository **implementations**, external services, persistence config |
| `Api` | `Microsoft.NET.Sdk.Web` | `Application`, `Infrastructure` | Endpoints/controllers, DI wiring (composition root), middleware, `Program.cs` |

Rule: `Domain` never references another project. `Application` never references
`Infrastructure` (only its interfaces, which live in Application/Domain).

## Steps (commands)

Run from the directory where the solution folder should live. Replace `<Name>` and
`<tfm>`.

```bash
# 1. solution + layer projects
dotnet new sln -n <Name>
dotnet new classlib -n Domain         -f <tfm> -o Domain
dotnet new classlib -n Application    -f <tfm> -o Application
dotnet new classlib -n Infrastructure -f <tfm> -o Infrastructure
dotnet new webapi    -n Api           -f <tfm> -o Api

# 2. add to solution
dotnet sln add Domain Application Infrastructure Api

# 3. references — respect the dependency direction
dotnet add Application    reference Domain
dotnet add Infrastructure reference Application
dotnet add Infrastructure reference Domain
dotnet add Api            reference Application
dotnet add Api            reference Infrastructure

# 4. remove default Class1.cs from classlibs
rm Domain/Class1.cs Application/Class1.cs Infrastructure/Class1.cs
```

Each `.csproj` should have `<Nullable>enable</Nullable>` and
`<ImplicitUsings>enable</ImplicitUsings>` (the webapi/classlib templates add these on
recent SDKs — verify).

## Base folders

Create starting folders so the structure is obvious:

```
Domain/         Entities/  ValueObjects/  Enums/  Exceptions/  Interfaces/  Events/
Application/    Common/  Interfaces/  + a folder per feature (e.g. Rifas/ with Commands/ Queries/ Dtos/)
Infrastructure/ Persistence/  Persistence/Configurations/  Persistence/Repositories/  Services/  DependencyInjection.cs
Api/            Endpoints/ (or Controllers/)  Middleware/  Program.cs
```

## DI wiring

Give each non-Domain layer an extension method, called from `Program.cs`:

- `Application/DependencyInjection.cs` → `AddApplication(this IServiceCollection)`
- `Infrastructure/DependencyInjection.cs` → `AddInfrastructure(this IServiceCollection, IConfiguration)`

`Program.cs`:
```csharp
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
```

## If EF Core requested

Add to `Infrastructure`:
```bash
dotnet add Infrastructure package Microsoft.EntityFrameworkCore
dotnet add Infrastructure package Microsoft.EntityFrameworkCore.<Provider>   # SqlServer | Npgsql.EntityFrameworkCore.PostgreSQL | Sqlite
dotnet add Infrastructure package Microsoft.EntityFrameworkCore.Design
```
Put `AppDbContext` in `Infrastructure/Persistence/`. Register it in
`AddInfrastructure` reading the connection string from `IConfiguration`.
Migrations run with `--project Infrastructure --startup-project Api`.

## Verify

```bash
dotnet build
```
Build must pass with 0 errors before declaring done. Report the tree and any warnings.

## Common mistakes — do not

- Reference `Infrastructure` from `Application`. (Inverts the dependency rule.)
- Put `DbContext` or EF packages in `Domain` or `Application`.
- Reference any project from `Domain`.
- Skip `dotnet sln add` (projects build but aren't in the solution).
