NSpecifications - Specification Pattern for .Net
====

NSpecifications is an easy-to-use library, grounded on the principles of the Domain-Driven Design. It provides a great way to take advantage of the Specification Pattern on .Net.

This is what you can do with Specifications:

 1. **Validate an object to see if it fulfils a set of rules or is ready for some purpose;**
 2. **Filter an in-memory collection of objects or query a DB with the same code;**
 3. **Use it on a factory for specifying some rules or properties that the new instance must fulfil.**

Although **Specification pattern is very flexible and useful**, it's been underestimated due to the lack of awareness about it, and it's also been often avoided due of the pains of implementing it in OOP languages that don't support lambda expressions, fortunately this is no longer the case with C#. 

Specification pattern will allow you to:

 - Use the **same code for querying a database or an in-memory collection of objects**: 
	 - Query a database: `repository.Find(cheapPlacesToEat & open)`
	 - Filter in-memory list: `list.Where(cheapPlacesToEat & open)`
 - Use it **in place of lambda expressions** that work with the `IQueryable<T>` interface
 - Encapsulate **predicates that can be reused over and over again** (a predicate is just like a query but only for a specific entity),
 - **Compose** specifications **just like if they were boolean expressions**  (&, |, !, ==, !=)
 - Create bigger specifications from the composition of smaller specifications
 - Combine a specifications with a boolean expression.
   
Example combining specifications with a boolean expression:
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
Specifications can also be useful if you want to:
 - Use *ReSharper* or reflection for easily tracing **all existing queries** in the source code 
 - Write more readable, manageable and elegant code.

Specifications are described by Eric Evans as separate, combinable, rule objects, based on the concept of predicates but more specialized. **"A SPECIFICATION is a predicate that determines if an object does or does not satisfy some criteria."**

The Old Way: ISpecification(T)
--------------
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

## The New Way: `new Spec<T>(expression)` ##

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

## Use Case ##

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

## Is / Are ##

Usually candidates are checked against Specifications, but another possible use case is to do it the other way around, to check if a given specification matches the attributes of a candidate or set of candidates.

```csharp
var cold = new Spec<Drink>(d => d.TemperatureCelsius < 2);

if (candidateDrink.Is(cold))
    Console.Write("Candidate drink is cold.");
    
if (new[] { blackberryJuice, appleJuice, orangeJuice }.Are(cold))
    Console.Write("All candidate drinks are cold.");
```

## Install it from NuGet Gallery ##
```
Install-Package NSpecifications -Version 1.0.1
```


References:
http://martinfowler.com/apsupp/spec.pdf



