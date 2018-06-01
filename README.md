# NSpecifications -- Specifications on .net
## What is the Specification Pattern?
When selecting a subset of objects, it allows to separate the statement of **what kind of objects can be selected** from the **object that does the selection**. 

Ex: 

> A cargo has a separate storage specification to describe what kind of
> container can contain it. The specification object has a clear and
> limited responsibility, which can be separated and decoupled from the
> domain object that uses it.

## What can you do with Specification Pattern?
1. **Validate an object** or check that only suitable objects are used for a certain role
2. **Select a subset of objects** based a specified criteria, and refresh the selection at various times
3. Describe what an object might do, without explaining the details of how the object does it, but in such a way that **a candidate might be built to fulfill the requirement**

## What are the advantages of this library over others
1. One-liner specifications
2. Query an **in-memory collection** and **databases** using same code
4. Easy **composition** of specifications
6. Use a Specification in place of any `Expression<Func<T, bool>>` and `Func<T, bool>`
7. Combine a `boolean` with a Specification in order to negate it when value is `false`
8. Is / Are extension methods

### One-liner specifications
```csharp
var greatWhiskey = new Spec<Drink>(drink => drink.Type == "Whiskey" && drink.Age >= 11);
```

### Query an in-memory collection and databases using same code
|                |                          |
|----------------|-------------------------------|
|Database query| `dataContext.Places.Where(cheapPlacesToEat & open)`
|Filter in-memory collection| `list.Where(cheapPlacesToEat & open)`   
|                |                          |
### Easy **composition** of specifications
```csharp
var greatWhiskey = new Spec<Drink>(drink => drink.Type == "Whiskey" && drink.Age >= 11);
var fresh = new Spec<Drink>(drink => drink.Extras.Contains("Ice"));
var myFavourite = greatWhiskey & fresh;
var yourFavourite = greatWhiskey & !fresh;
```
### Use a Specification in place of any `Expression<Func<T, bool>>` and `Func<T, bool>`
```csharp
var books = new List<Book>();
(...) 
var boringBookSpec = new Spec<Book>(book => book.NumberOfPages > 300);
var boringBooks = books.Where(boringBookSpec);
```
### Combine a `boolean` with a Specification in order to negate it when value is `false`
```csharp
static ASpec<Place> CheapPlace = new Spec<Place>(p => p.Price < 10);

// If isCheap is not set it will simply return all places
// If isCheap is true it will return only Cheap places
// If isCheap is false it will return all places that are not Cheap
public Places[] FindPlaces(bool? isCheap = null) {
    
    // Initialize spec with an all-inclusive specification
    var spec = Spec.Any<Place>();
    
    // Apply filter only if a filter was specified
    if (isCheap.HasValue)
        spec = spec & (CheapPlace == isCheap.Value);
    
    // Let the repository search it in the DB
    return repository.Find(spec);
}
```
### Is / Are extension methods
```csharp
var cold = new Spec<Drink>(d => d.TemperatureCelsius < 2);

if (candidateDrink.Is(cold))
    Console.Write("Candidate drink is cold.");
    
if (new[] { blackberryJuice, appleJuice, orangeJuice }.Are(cold))
    Console.Write("All candidate drinks are cold.");
```
## Other possible reasons
 - Easily finding **all existing queries** in the source code with a simple search for usages of `Spec`
 - Write more readable, manageable and elegant code

## This Library was based on Eric Evans book
Specifications are described by Eric Evans as separate, combinable, rule objects, based on the concept of predicates but more specialized. **"A SPECIFICATION is a predicate that determines if an object does or does not satisfy some criteria."**

## Technical Documentation
### ISpecification(T) -- The Old Way of doing things

The most basic form of Specification used to be implemented via a simple interface, the probem was that they could only be used for in-memory queries. 
```csharp
public class BoringBookSpec : ISpecification<Book> {
    public IsSatisfiedBy(Book book)
    {
        return book.Rating < 3 && book.Pages > 450;
    }
}
```
And then we could use it like this:
```csharp
var isBoring = new BoringBookSpec().IsSatisfiedBy(book);
```
Or like this:
```csharp
var boringBookSpec = new BoringBookSpec();
var boringBooks = allBooks.Where(boringBookSpec.IsSatisfiedBy);
```
`ISpecification<T>` could also be composed:
```csharp
var boringBookSpec = lowRatedBookSpec.And(bigBookSpec);
```
This way of using Specifications had some cons:

 - it was **very verbose**, because every new specification had to be defined in a new class
 - it **couldn't be converted to database queries**
 - *and*, *or* and *not* "operators" were implemented as methods, **not as real operators**. 

Let's see now a more intuitive way to create and manage Specifications.

### The New Way: `new Spec<T>(expression)` ##

`ISpecification<T>` is now extended by the `ASpec<T>` *abstract* class. This abstract class enables a set of new features such as: 

 - real operators (&, |, !, ==, !=)
 - implicit operators that make any Specification to be interchangeable with `Expression<Func<T, bool>>` and `Func<T, bool>` 

`ASpec<T>` is an *abstract* class therefore it can't be directly instantiated. Instead we use `Spec<T>` to create a new instance. This generic class should be good for 99.9% of your specifications but if you need to make your own implementation of Specification you can always extend it from `ASpec<T>`.

```csharp
ASpec<Car> fastCar = new Spec<Car>(c => c.VelocityKmH > 200);
```

**Pros of using this generic implementation**

 - only one line needed to define a new specification
 - it returns `ASpec<T>` class (and not just an interface) so that we can now use real operators for making composition;
 - it stores a Linq Expression in the created instance, therefore it can be easily used by any `IQueryable<T>`, suitable for querying DBs. 

Example:
```csharp
var greatWhiskey = new Spec<Drink>(drink => drink.Type == "Whiskey" && drink.Age >= 11);
var fresh = new Spec<Drink>(drink => drink.Extras.Contains("Ice"));
var myFavouriteDrink = repository.FindOne(greatWhiskey & fresh);
```    
Let me dig into the details:

 - Following the example from Eric Evans book I usually name my specifications as objects rather than predicates. I could name it  `greatWhiskeySpec` or `greatWhiskey` for short but not `isGreatWhiskey`. My aim is to make it clear that a specification is be a bit more than just a simple boolean expression. 
 - As you may have noticed by now this code now is much less verbose than when we were using `ISpecification<T>`.
 - I can now compose specifications using friendly operators: `!` (not), `&` (and), `|` (or), == (compare spec with a boolean).
 - I'm passing my specifications directly as a parameter to a repository method that expects a Linq Expression, but it receives a specification instead and that's converted automatically.

## Real Use Cases ##

Let's say that I need to search for users by name (if specified) and by their locked-out state. This is how I'd do it.

First I'd have to find a meaningful place to store my specifications: 

 - It could be in a static class called `Specifications` or `Specs` for short. I could invoke it like this: `Specs.LockedOutUser`.   
 - Or it could be in a static classe per entity type like (`UserSpecs`, `UserGroupSpecs`, ...). Ex: `UserSpecs.LockedOut` .
 - It could be in a static members inside the `User` entity. Ex: `User.LockedOut`. This is my favourite, because specifications are always tightly coupled to entities and this can make maintenance easier. 

The only thing I'd like to note here is that hosting specifications in static members do not present any problem for Unit Testing. Specifications usually don't need to be mocked.

Let's blend the specifications into the User class.
```csharp
public class User 
{
    public string Name { get; }
    public bool IsLockedOut { get; }
	
    // Spec for LockedOut
    public static readonly ASpec<User> LockedOut = new Spec<User>(user => user.IsLockedOut);  
    	
    // Spec for NamedLike
    public static ASpec<User> NamedLike(string text) 
    {
    	return new Spec<User>(user => name.Contains(text));
    }
}
```
While in the first member `LockedOut` is instantiated once (it's a readonly static field), the second member `NamedLike` need to be instantiated for every given text parameter (it's a static factory method). That's the way that specifications need to be done when they need to receive parameters.

When I need to make my query I can do it like this:
```csharp
public User[] FindByNameAndLockedStatus(string name = null, bool? isLockedOut = null) {
    
    // Initialize the spec with an all inclusive spec
    var spec = Spec.Any<User>;
    
    // Apply Name filter
    if (!string.IsNullOrEmpty(name))
    	spec = spec & User.NamedLike(name);
    
    // Apply LockeOut filter
    if (isLockedOut.HasValue)
    	spec = spec & (User.LockedOut == isLockedOut.Value);
    
    var users = _repository.Find(spec);
}
```

## Install it from NuGet Gallery ##
```
Install-Package NSpecifications -Version 1.1.0
```

## References:
http://martinfowler.com/apsupp/spec.pdf
https://domainlanguage.com/ddd/
