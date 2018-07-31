## Updates

Create a update object:

```csharp

```

Of course, in addition to inserting records into the database, 
the query builder can also update existing records using the update method. 

The update method, like the insert method, accepts an array of column 
and value pairs containing the columns to be updated. 

You may constrain the update query using where clauses:

```csharp

```

```csharp

```

### Get number of affected rows:

```csharp

```

### Increment & Decrement

The query builder also provides convenient methods for incrementing or 
decrementing the value of a given column. This is simply a shortcut, 
providing a more expressive and terse interface compared to manually 
writing the update statement.

Both of these methods accept at least one argument: the column to modify. 
A second argument may optionally be passed to control the amount by 
which the column should be incremented or decremented:

```csharp

```

```csharp

```

Incrementing without the convenient methods:

```csharp

```

### Update Limit

The `limit` clause places a limit on the number of rows that can be updated.

```csharp

```

### Update Low Priority

With the `LOW_PRIORITY ` modifier, execution of the UPDATE is delayed until no 
other clients are reading from the table. This affects only storage engines 
that use only table-level locking (such as MyISAM, MEMORY, and MERGE).

```csharp

```

### Update and ignore errors

With the `IGNORE` modifier, the update statement does not abort 
even if errors occur during the update. Rows for which duplicate-key 
conflicts occur on a unique key value are not updated. 

```csharp

```

### Update with order by

If an UPDATE statement includes an ORDER BY clause, 
the rows are updated in the order specified by the clause. 

```csharp

```

**Next page:** [Deletes](deletes.md)
