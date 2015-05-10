NSpecifications
====

NSpecifications is an easy-to-use library that provides a great way to take advantage from the Specification Pattern in .Net grounded on the principles of the  Domain-Driven Design.

This is what you can do with NSpecifications:

 1. Filter a collection of objects in-memomry and from databases;
 2. Validate an object by checking if it matches a set of rules;
 3. Used by a factory to know how to create a candidate object built to fulfill a set of requirements.

You will understand that the Specification pattern is a very flexible and usefull pattern, that has been underestimated because a full implementation of this concept with object is cumbersome. I'll show you how you can get all these benefits using a library that handles Specifications with elegance.

Specifications are as decribed by Eric Evans as separate, combinable, rule objects, based on the concept of predicates but more specialized. "A SPECIFICATION is a predicate that determines if an object does or does not satisfy some criteria."

ISpecification(T)
--------------
The most simple Specification is a business specification that can only be used for in-memory queries. 

    public class BoringBookSpec : ISpecification<Book> {
	    public IsSatisfiedBy(Book book)
	    {
		    return book.Rating <= 3.5 && book.Pages > 450
	    }
    }

Now you can use your specification like this:

    var isBoring = new BoringBookSpec().IsSatisfiedBy(book);

Or like this:

    var boringBookSpec = new BoringBookSpec();
    var boringBooks = allBooks.Where(boringBookSpec.IsSatisfiedBy);

Because of the usage of extension methods Specifications can also be combined if they are of the same generic argument type:

    var boringBookSpec = lowRatedBookSpec.And(tooBigBookSpec);

This way of using Specifications has some cons:

 - very verbose, every new specification is defined in a class
 - it can't be converted to queries in order to retrieve data from a DB
 - *and* and *or* operators are implemented as methods and that's not the most natural way for composing predicates 

Let's now see another much more intuitive way to create and manage Specifications.

## Specification.For(T) ##

Specification is an *abstract* class, therefore it can't be directly instantiated. The Specification class expands the features of the ISpecification(T) and provides a static factory method that releases you from the burden of having to create an implementation of Specification, but you can still use it as a base class and implement your own Specification. These are the pros of Specification class:

 - because it's a class (and not just an interface) we can now use real operators for making composition;
 - it uses Linq Expressions therefore it can be easily converted into IQueryable(T), suitable for querying DBs. 

Example:

    var greatWhiskey = Specification.For<Drink>(drink => drink is Whiskey && drink.ManufacturedDate >= 11.yearsAgo);
    var fresh = Specification.For<Drink>(drink => drink.Contains<Ice>());
    var myFavouriteDrink = greatWhiskey & fresh;
    
Let me dig into the details:

 - Following the example from Eric Evans book I usually name my  specifications as objects rather then predicates. This means that I could name it like this `greatWhiskeySpecification` or `greatWhiskey` for a shortcut but not `isGreatWhiskey`. My aim is to clearly state that specifications are a class of it's own, more specialised then predicates and should not be confused. 
 - I can now compose specifications using the operators `!` (not), `&` (and), `|` (or). Please do not confuse these operators with the binary operators.





 





References:
- http://martinfowler.com/apsupp/spec.pdf
- 
