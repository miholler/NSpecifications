NSpecifications
====

NSpecifications is an easy-to-use library, grounded on the principles of the  Domain-Driven Design. It provides a great way to take advantage of the Specification Pattern on .Net.

This is what you can do with NSpecifications:

 1. **To validate an object to see if it fulfils a set of rules or is ready for some purpose;**
 2. **To filter an in-memory collection of objects or query a DB;**
 3. **Use it on a factory for specifying some rules that need to be fulfilled wen building a new object.**

Although **Specification pattern is very flexible and useful**, it's been underestimated due to the lack of awareness about it, and it's also been often avoided due of the pains of implementing it in OOP languages that don't support lambda expressions, fortunately this is no longer the case with C#. 

Specification patter will allow you to:

 - Use the same exact **same code for querying a database or an in-memory collection of objects**: 
	 - Query a database: `repository.Find(cheapPlacesToEat & open)`
	 - Filter in-memory list: `list.Where(cheapPlacesToEat & open)`
 - Use it **in place of lambda expressions** that work with the `IQueryable` interface
 - Encapsulate **predicates that can be reused over and over again** (the predicate is just like a query but only for a specific class),
 - **Compose** predicates **just like if they were binary expressions**  (&, |, !, ==, !=)
 - Create bigger predicates from the composition of smaller predicates
 - Combine a predicate with a boolean expression.
   
Example combining predicate with a boolean expression:

    // If isCheap is not set it will simply return all places
    // If isCheap is true it will return only Cheap places
    // If isCheap is false it will return all places that are not Cheap
    public Places[] FindPlaces(bool? isCheap = null) {
        // AllPlaces and Cheap are specifications
        var spec = AllPlaces;
        if (isCheap != null)
	        spec += isCheap == Cheap;
        repository.Find(spec):
    }

It can also helps because you can:
 - Use *ReSharper* to easily trace **all existing queries** in the source code 
 - Write more readable, manageable and elegant code.

Specifications are described by Eric Evans as separate, combinable, rule objects, based on the concept of predicates but more specialized. **"A SPECIFICATION is a predicate that determines if an object does or does not satisfy some criteria."**

The Old Way: ISpecification(T)
--------------
The most basic form of Specification used to be implemented via a simple interface, the probem was that they could only be used for in-memory queries. 

    public class BoringBookSpecification : ISpecification<Book> {
	    public IsSatisfiedBy(Book book)
	    {
		    return book.Rating < 3 && book.Pages > 450;
	    }
    }

And then we could use it like this:

    var isBoring = new BoringBookSpec().IsSatisfiedBy(book);

Or like this:

    var boringBookSpec = new BoringBookSpec();
    var boringBooks = allBooks.Where(boringBookSpec.IsSatisfiedBy);

`ISpecification<T>` could also be composed:

    var boringBookSpec = lowRatedBookSpec.And(bigBookSpec);

This way of using Specifications had some cons:

 - it was **very verbose**, because every new specification had to be defined in a new class
 - it **couldn't be converted to database queries**
 - *and*, *or* and *not* "operators" were implemented as methods, **not as real operators**. 

Let's see now a more intuitive way to create and manage Specifications.

## The New Way: Spec.For(T) ##

`ISpecification<T>` is now extended by the `Specification<T>` *abstract* class. This abstract class enables a set of new features such as: 

 - real operators (&, |, !, ==, !=)
 - implicit operators that make any Specification to be interchangeable with `Expression<Func<T, bool>>` and `Func<T, bool>` 

`Specification<T>` is an *abstract* class therefore it can't be directly instantiated. We now have a factory called `Spec` that free us completely from the need to create a new implementation for each single Specification. It instantiates an internal generic implementation that can be used for all specs. (And if for any reason a generic implementation if not good enough, we can always fallback on making our own custom implementation of `Specification<T>`) 

**Pros of using factory `Spec.For<T>(lambda_express...)`:**

 - only one line needed to define a new specification
 - it returns `Specification<T>` class (and not just an interface) we can now use real operators for making composition;
 - it stores a Linq Expression in the created instance, therefore it can be easily converted and used in any IQueryable(T), suitable for querying DBs. 

Example:

    var greatWhiskey = Spec.For<Drink>(drink => drink is Whiskey && drink.ManufacturedDate >= 11.yearsAgo);
    var fresh = Spec.For<Drink>(drink => drink.Contains<Ice>());
    var myFavouriteDrink = repository.Find(greatWhiskey & fresh);
    
Let me dig into the details:

 - Following the example from Eric Evans book I usually name my  specifications as objects rather then predicates. This means that I could name it  `greatWhiskeySpec` or `greatWhiskey` for a shortcut but not `isGreatWhiskey`. My aim is to clearly state that specifications are a class of it's own, more specialised then predicates and should not be confused. 
 - As you may have noticed by now this is much less verbose.
 - I can now compose specifications using friendly operators: `!` (not), `&` (and), `|` (or), == (equal). (Please do not confuse these operators with the binary operators).
 - I'm passing my specifications directly as a parameter to the Find method of the repository that expects a Linq Expression, but it receives a specification instead and it is converted to Expression automatically.

## Use Case ##

Let's say that I need to search for users by name (if specified) and by the locked-out state. This is how I'd do it.

First I'd have to find a meaningful place to put my specifications: 

 - It could be in a statics class called `Specifications` or `Specs` for short. I could invoke it like this: `Specs.LockedOutUser`.   
 - Or it could be in static classes, one per Entity type `UserSpecs`. Ex: `UserSpecs.LockedOut` .
 - It could be in a static member inside the `User` entity. Ex: `User.LockedOut`. This is my favourite, because specifications are tightly coupled to entities. 

The only thing I'd like to note here is that hosting specifications in static members do not present any problem for Unit Testing. Specifications have no need need to be mocked.

Let's blend the specifications into the User class.

    public class User 
    {
	    public string Name { get; }
	    public bool IsLockedOut { get; }
	    
	    // Spec for LockedOut
	    public static readonly Specification<User> LockedOut = Spec.For<User>(user => user.IsLockedOut);  
	    
	    // Spec for NamedLike
	    public static Specification<User> NamedLike(string text) 
	    {
		    return Specific.For<User>(user => name.Contains(text));
	    }
		
		// Spec for all Users
		public static readonly Specification<User> All = Spec.ForAll<User>();  
    }

While in the first member `LockedOut` is instantiated once (it's a readonly static field), the second member `NamedLike` need to instantiated for every given text parameter (it's a static factory method). That's Ok and that's just the way that specifications are meant to work when they need to incorporate parameters.

When I need to execute the query I do it like this:

    public User[] Find(string searchText = null, bool? isLockedOut = null) {
	    var spec = User.All;
	    if (string.IsNullOrEmpty(searchText))
		    spec += User.NamedLike(searchText);
		if (isLockedOut != null)
			spec += isLockedOut == User.LockedOut;
		var repository = new UserRepository();
	    var users = repository.Find(spec);
    }




 





References:
http://martinfowler.com/apsupp/spec.pdf



