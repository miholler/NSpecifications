NSpecifications
====

NSpecifications is an easy-to-use library that provides a great way to take advantage from the Specification Pattern in .Net grounded on the principles of the  Domain-Driven Design.

This is what you can do with NSpecifications:

 1. Filter a collection of objects in-memomry and from databases;
 2. Validate an object by checking if it matches a set of rules;
 3. Used by a factory to know how to create a candidate object built to fulfill a set of requirements.

You will understand that the Specification pattern is a very flexible and usefull pattern, that has been underestimated because a full implementation of this concept with object is cumbersome. I'll show you how you can get all these benefits using a library that handles Specifications with elegance.

Specifications are as decribed by Eric Evans as separate, combinable, rule objects, based on the concept of predicates but more specialized. "A SPECIFICATION is a predicate that determines if an object does or does not satisfy some criteria."

Simple example
--------------

    var boringBook = Specification.For<Book>(book => book.Rating <= 3.5 && book.Pages > 450);





References:
- http://martinfowler.com/apsupp/spec.pdf
- 
