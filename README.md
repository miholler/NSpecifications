NSpecifications
====

NSpecifications is an easy-to-use library that provides a good implementation of the specification pattern grounded on  Domain-Driven Design.
Specification pattern can be consitently applyed in order to accomplish 3 different goals:
- Validate an object by checking if it matches a set of rules
- Filter a collection of objects in-memomry or in persisted in a database
- Help in the creation of objects by describing what an object might do, without explaining the details of how the object does it, but in such a way that a candidate object might be built to fulfill the requirements.

The Specification pattern is one of the most flexible and usefull patterns but have been neglected a lot due to some big challenges related to it's understanding and the skills set needed to implement it. I'll provide some examples for these 3 scenarios and show how this library really manages handles them with elegance.




References:
- http://martinfowler.com/apsupp/spec.pdf
