NSpecifications
====

NSpecifications is an easy-to-use library, grounded on the principles of the  Domain-Driven Design, that provides a great way to take advantage from the Specification Pattern in .Net.

This is what you can do with NSpecifications:

 1. **To validate an object to see if it fulfils a set of rules or is ready for some purpose;**
 2. **To filter an in-memomry collection of objects or query a DB;**
 3. **Used by a factory to know how to create a candidate object built to fulfil a set of requirements.**

You will understand that the **Specification pattern is a very flexible and useful pattern**, that has been underestimated because a full implementation of this concept in OOP used to be cumbersome. Your code will have **less redundancy** and **your repositories will be much more readable when you start using meaningful specifications instead of composing complex predicates**. I'll show you how to finally get all these benefits using a library that handles Specifications with elegance.

Specifications are described by Eric Evans as separate, combinable, rule objects, based on the concept of predicates but more specialized. **"A SPECIFICATION is a predicate that determines if an object does or does not satisfy some criteria."**

ISpecification(T)
--------------
The most simple Specification is a business specification that can only be used for in-memory queries. 

    public class BoringBookSpecification : ISpecification<Book> {
	    public IsSatisfiedBy(Book book)
	    {
		    return book.Rating < 3 && book.Pages > 450
	    }
    }

Now you can use your specification like this:

    var isBoring = new BoringBookSpecification().IsSatisfiedBy(book);

Or like this:

    var boringBookSpecification = new BoringBookSpecification();
    var boringBooks = allBooks.Where(boringBookSpecification.IsSatisfiedBy);

ISpecifications can also be combined if they are of the same generic argument type:

    var boringBookSpecification = lowRatedBookSpecification.And(bigBookSpecification);

This way of using Specifications has some cons:

 - **very verbose**, every new specification is defined in a class
 - it **can't be converted to queries** in order to retrieve data from a DB
 - *and*, *or* and *not* operators are implemented as methods and that's **not the most natural way for composing predicates** 

Let's see now a more intuitive way to create and manage Specifications.

## Specification.For(T) ##

Specification is an *abstract* class, therefore it can't be directly instantiated. The Specification(T) class expands the features of the ISpecification(T) and provides a static factory method that releases you from the burden of having to create an implementation of Specification, but if you want you can still use it as a base class and implement your own Specification. These are the pros of Specification class:

 - only one line needed to define a new specification
 - because it's a class (and not just an interface) we can now use real operators for making composition;
 - it uses Linq Expressions therefore it can be easily converted into IQueryable(T), suitable for querying DBs. 

Example:

    var greatWhiskey = Specification.For<Drink>(drink => drink is Whiskey && drink.ManufacturedDate >= 11.yearsAgo);
    var fresh = Specification.For<Drink>(drink => drink.Contains<Ice>());
    var myFavouriteDrink = repository.Get(greatWhiskey & fresh);
    
Let me dig into the details:

 - Following the example from Eric Evans book I usually name my  specifications as objects rather then predicates. This means that I could name it like this `greatWhiskeySpecification` or `greatWhiskey` for a shortcut but not `isGreatWhiskey`. My aim is to clearly state that specifications are a class of it's own, more specialised then predicates and should not be confused. 
 - As you may have noticed now I didn't need to create a new class for every specification.
 - I can now compose specifications using friendly operators: `!` (not), `&` (and), `|` (or). (Please do not confuse these operators with the binary operators).
 - I'm passing my specifications as a parameter to the Get method of the repository that knows how to use it to query the DB.

## Use Case ##

Let's say that I need to search for users by name (if specified) and by locked out status. This is how I'd do it.

First I'd have to find a place to put my specifications: 

 - It could be in a statics class called `Specifications`. I would call it like this: `Specifications.LockedOutUser`.   
 - It could be in a statics class called `UserSpecification`. I would call it like this: `UserSpecification.LockedOut`.  
 - It could be in a static method inside the `User` entity. I would call it like this `User.LockedOut`. This is my favourite because  specifications are tightly coupled to entities. 
 - The only thing I'd like to note here is that hosting specifications in static members do not present any problem for Unit Testing and I've never seen any specification that needed to be mocked.

Let's blend the specifications into the class.

    public class User 
    {
	    public string Name { get; }
	    public bool IsLockedOut { get; }
	    
	    public static readonly Specification<User> LockedOut = Specification.For<User>(user => user.IsLockedOut);  
	    
	    public static Specification<User> NamedLike(string text) 
	    {
		    return Specification.For<User>(user => name.Contains(text));
	    }
		
		public static readonly Specification<User> All = Specification.ForAll<User>();  
    }

While in the first member `LockedOut` specification is only instantiated once (it's a readonly static field), for the second member `NamedLike` the specification have to be instantiated for every given text parameter (it's a static factory method). That's OK and that's just the way that specifications are meant to work.

When I need to call my domain and execute the query I do it like this:

    var specification = User.All();
    if (string.IsNullOrEmpty(searchText))
	    specification = specification & User.NamedLike(searchText);
	if (isLocked != null)
	    specification = specification & User.LockedOut.Value
	var repository = new UserRepository();
    var users = repository.Find(specification);




 





References:
http://martinfowler.com/apsupp/spec.pdf

