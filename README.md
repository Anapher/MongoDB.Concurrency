# MongoDB.Concurrency
MongoDB utilities to apply optimistic concurrency when updating and deleting entities. A version property is used to identify the snapshot of an entity and when executing operations, the version of the local copy and the version of the current entity of the database is compared. The operation will only be executed if the versions match.

## Example
First of all, you need an entity class. This class may implement `IVersionedEntity` from this library or just specifc a property of type `int` (typically called `Version`). It is recommended to not use the interface if you have a layerd architecture (like Clean Architecture) as the Domain Layer should not have a reference to database specific things like MongoDB.

If you want to update an entity, just use the `.Optimistic()` extension method and supply a reference to the version property. Then you may use one of the operations that automatically take care of comparing and incrementing the version. If the update failed because of a version mismatch, a `MongoConcurrencyUpdatedException` exception is thrown.

```cs
var entity = await Collection.Find(x => x.Name == 34).FirstOrDefaultAsync();
entity.RandomProp = "Hello world";

try
{
    await Collection.Optimistic(x => x.Version).UpdateAsync(entity);
}
catch (MongoConcurrencyUpdatedException e)
{
    // retry
}
```

## License
MongoDB.Concurrency is licensed under the MIT License. For more information see [LICENSE](./LICENSE).