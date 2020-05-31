# Katoa Queries

Katoa Queries is a simple NuGet package for quickly loading SQL queries from embedded resources.

Why load queries from embedded resources? This allows you to avoid embedding all your sql as strings in your dotnet code.  Also many IDEs also make it realtively easy to connect .sql files to databases and allowing auto complete and the ability to test queries interactively.  

## Installing

Install package ```Katoa.Queries``` from Nuget


## Adding Queries

Create a folder called "Queries" 

Add .sql files into the Queries directory

Change the properties of the SQL files so they will be compiled as an embedded resource.

## Loading Queries

Queries are loaded by just the name of the file(excluding the ```.sql``` extension).

For example, if you have a query with the path :-

```./Queries/MyQuery.sql```

You load it with the following :-

```C#
var sql = Query.For("MyQuery");
```

If subdirectories are used to organize queries under the query folder then they will be seperated by a dot.

For example if you make a query with the following path :-

```./Queries/Product/All.sql```

You can load it with the following :-
```C#
var sql = Query.For("Product.All");
```

## Ignore First Section of SQL files

To interactively test parametized queries in your IDE, all the parameters need to be declared as varibles.  When loading the query these declarations are not needed, so Katoa Queries has a simple mechanisim to ignore everything before a "Magic" comment if present :-

```-- QUERY```

As an example, consider the following

```sql
DECLARE @Param INT = 10
-- QUERY
SELECT * from Table WHERE X > @Param
```

When loading this query only the SELECT statement will bee loaded.  

## Dapper Example

In the the folder ```DapperDemo``` there is an exmple of using Katoa Queries to load sql for use with Dapper.
