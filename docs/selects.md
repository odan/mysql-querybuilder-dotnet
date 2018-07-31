## Selects

Create a select query object with the connection object.

```csharp

```
Creating a SelectQuery object manually:

```csharp

```

## Inspecting The Query

Getting the generated SQL string:

```csharp

```
Output:

```sql
SELECT * FROM `users` as `u`;
```

### Retrieving Results

#### Retrieving All Rows From A Table

You may use the `select()` method of the `Connection` object to begin a query. 
The table method returns a fluent query builder instance for 
the given table, allowing you to chain more constraints onto 
the query and then finally get the results using the get method:

```csharp

```

The `fetch()` method returns an row containing the results 
where each result is an instance of the Array or PHP StdClass object. 
You may access each column's value by accessing the column as a property of the object:

```csharp

```

#### Retrieving A Single Row From A Table

```csharp

```

#### Retrieving A Single Column From A Table

```csharp

```

#### Distinct

The distinct method allows you to force the query to return distinct results:

```csharp

```

#### Columns

Select columns by name:

```csharp

```

```sql
SELECT `id`,`username`,`first_name` AS `firstName` FROM `users`;
```

Select columns by array:

```csharp

```

Select columns with alias:

```csharp

```

Add fields one after another:

```csharp

```

```sql
SELECT `first_name`,`last_name`,`email` FROM `users`;
```

#### Raw Expressions

Sometimes you may need to use a raw expression in a query. 
These expressions will be injected into the query as strings, 
so be careful not to create any SQL injection points! 

To create a raw expression, you may use the RawExp value object:

```csharp

```

```sql
SELECT count(*) AS user_count, `status` FROM `payments` WHERE `status` <> 1 GROUP BY `status`;
```

Creating raw expressions with the function builder:

```csharp

```

```sql
SELECT count(*) AS user_count,`status` FROM `payments`;
```

#### Aggregates

The query builder also provides a RawExp for aggregate methods 
such as count, max, min, avg, and sum. 

You may call any of these methods after constructing your query:

```csharp

```

```sql
SELECT MAX(amount), MIN(amount) FROM `payments`;
```

#### Sub Selects

If you want to SELECT FROM a subselect, do so by passing a callback
function and define an alias for the subselect:

```csharp
```

```sql
SELECT `id`,(SELECT MAX(payments.amount) FROM `payments`) AS `max_amount` FROM `test`;
```

### Joins

#### Inner Join Clause

The query builder may also be used to write join statements. 
To perform a basic "inner join", you may use the join method 
on a query builder instance. The first argument passed to 
the join method is the name of the table you need to join to, 
while the remaining arguments specify the column constraints 
for the join. Of course, as you can see, you can join to 
multiple tables in a single query:

```csharp

```

```sql
SELECT `users`.*, `contacts`.`phone`, `orders`.`price` 
FROM `users`
INNER JOIN `contacts` ON `users`.`id` = `contacts`.`user_id`
INNER JOIN `orders` ON `users`.`id` = `orders`.`user_id`;
```

#### Left Join Clause

If you would like to perform a "left join" instead of an "inner join", 
use the leftJoin method. The  leftJoin method has the same signature as the join method:

```csharp

```

```sql
SELECT *
FROM `users`
LEFT JOIN `posts` ON `users`.`id` = `posts`.`user_id`;
```

#### Cross Join Clause

From the [MySQL JOIN](https://dev.mysql.com/doc/refman/5.7/en/nested-join-optimization.html) docs:

> In MySQL, CROSS JOIN is syntactically equivalent to INNER JOIN; they can replace each other. 
> In standard SQL, they are not equivalent. INNER JOIN is used with an ON clause; CROSS JOIN is used otherwise.

In MySQL Inner Join and Cross Join yielding the same result.

Please use the [join](#inner-join-clause) method.

#### Advanced Join Clauses

You may also specify more advanced join clauses. 
To get started, pass a (raw) string as the second argument into 
the `joinRaw` and `leftJoinRaw` method.

```csharp

```

```sql
SELECT `id` FROM `users` AS `u` 
INNER JOIN `posts` AS `p` ON (p.user_id=u.id AND u.enabled=1 OR p.published IS NULL);
```

### Unions

The query builder also provides a quick way to "union" two queries together. 
For example, you may create an initial query and use the 
`union()`, `unionAll()` and `unionDistinct() `method to union it with a second query:

```csharp

```

```sql
SELECT `id` FROM `table1` UNION SELECT `id` FROM `table2`;
```

#### Where Clauses

Simple Where Clauses

You may use the where method on a query builder instance 
to add where clauses to the query. The most basic call 
to where requires three arguments. The first argument is 
the name of the column. The second argument is an operator, 
which can be any of the database's supported operators. 

Finally, the third argument is the value to evaluate against the column.

For example, here is a query that verifies the value 
of the "votes" column is equal to 100:

```csharp

```

```sql
SELECT * FROM `users` WHERE `votes` = 100;
```

Of course, you may use a variety of other operators when writing a where clause:

```csharp

```

You may also pass multiple AND conditions:

```csharp

```

```sql
SELECT * FROM `users` WHERE `status` = '1' AND `subscribed` <> '1';
```

#### Or Statements

ou may chain where constraints together as well as add OR clauses to the query. 
The orWhere method accepts the same arguments as the where method:

```csharp

```

```sql
SELECT * FROM `users` WHERE `votes` > '100' OR `name` = 'John';
```

#### Additional Where Clauses

##### Between and not between

```csharp

```

```sql
SELECT * FROM `users` WHERE `votes` BETWEEN '1' AND '100';
```
 

```csharp

```

```sql
SELECT * FROM `users` WHERE `votes` NOT BETWEEN '1' AND '100';
```

##### In and not in

```csharp

```

```sql
SELECT * FROM `users` WHERE `id` IN ('1', '2', '3');
```


```csharp

```

```sql
SELECT * FROM `users` WHERE `id` NOT IN ('1', '2', '3');
```

##### Is null and is not null

```csharp

```

```sql
SELECT * FROM `users` WHERE `updated_at` IS NULL;
```

```csharp

```

```sql
SELECT * FROM `users` WHERE `updated_at` IS NOT NULL;
```

If you use the '=' or '<>' for comparison and pass a null value you get the same result.

```csharp

```

```sql
SELECT * FROM `users` WHERE `updated_at` IS NULL;
```


#### Where Column

The whereColumn method may be used to verify that two columns are equal:

```csharp

```

```sql
SELECT * FROM `users` WHERE `users`.`id` = `posts`.`user_id`;
```

The whereColumn method can also be called multiple times to add multiple conditions. 
These conditions will be joined using the and operator:

```csharp

```

```sql
SELECT * 
FROM `users` 
WHERE `first_name` = `last_name`
AND `updated_at` = `created_at`;
```

#### Complex Where Conditions

```csharp

```

#### Where Raw

Using whereRaw:

```csharp

```

```sql
SELECT `id`, `username` FROM `users` WHERE status <> 1;
```

Using RawExp:

```csharp

```

```sql
SELECT * FROM `users` WHERE users.id = posts.user_id;
```

#### Order By

```csharp

```

```sql
SELECT * FROM `users` ORDER BY `updated_at` ASC;
```

#### Group By

```csharp

```

```sql
SELECT * FROM `users` GROUP BY `role`;
```

#### Limit and Offset

```csharp

```

```sql
SELECT * FROM `users` LIMIT 10;
```


```csharp

```

```sql
SELECT * FROM `users` LIMIT 25, 10;
```

#### Having

```csharp

```

```sql
SELECT * 
FROM `users` 
GROUP BY `id`, `username` ASC
HAVING `username` = 'admin';
```

Complex having conditions:

```csharp

```

### Using SQL Functions

A number of commonly used functions can be created with the Func() method:

* sum() Calculate a sum. The arguments will be treated as literal values.
* avg() Calculate an average. The arguments will be treated as literal values.
* min() Calculate the min of a column. The arguments will be treated as literal values.
* max() Calculate the max of a column. The arguments will be treated as literal values.
* count() Calculate the count. The arguments will be treated as literal values.
* now() Returns a Expression representing a call that will return the current date and time (ISO).

Example:

```csharp

```

```sql
SELECT SUM(`amount`) AS `sum_amount` FROM `payments`;
```

**Next page:** [Inserts](inserts.md)
