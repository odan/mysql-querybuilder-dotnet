# Documentation

* [Connection](#connection)
* [Select](selects.md)
* [Insert](inserts.md)
* [Update](updates.md)
* [Delete](deletes.md)
* [Schema](schema.md)

## Introduction

The database query builder provides a convenient, fluent interface to creating and running database queries. 
It can be used to perform most database operations in your application and works on all supported database systems.

The query builder uses MySql.Data parameter binding to protect your application against SQL injection attacks. 
There is no need to clean strings being passed as bindings.

## Connection

Create a new database Connection:

```csharp
using MySql.Data.MySqlClient;
using MySqlQueryBuilder;

string dsn = "Database=test;Data Source=localhost;User Id=root;Password=;SslMode=none";
MySqlConnection connection = new MySqlConnection(dsn);

try
{
    connection.Open();
}
catch (Exception e)
{
    System.Diagnostics.Trace.TraceError(e.Message);
}

// Create the query object
Query query = new Query(connection);

```

**Next page:** [Select](selects.md)
