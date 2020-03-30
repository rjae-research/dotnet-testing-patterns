# MultipleResponsibility Project

MultipleResponsibility project demonstrates several anti-patterns which violate the Single Responsiblity Principle.

## Project contains multiple responsibilities (domains)

* User should be in identity-related domain project.
* Item and Order should be in order-related domain project.

As a result we may experience one or more of the following:

* Release delays due to impacting multiple domains
* Higher bug risk due to multiple vectors of change
* Don't-Repeat-Yourself violations due avoiding changing code. This in turn exposes us to higher risk of bugs because there are multiple "copies" of code in use.
* Slower grokking of code due to overly complex project.

## Files contain multiple responsibilities (domain classes)

* [User.cs](User.cs) contains User, EmailAddress, NonEmptyString classes.
* [UserTests.cs](../MultipleResponsibility.Tests/UserTests.cs) contains tests for multiple classes.

As a result we may experience one or more of the following:

* Slower grokking of code due to overly complex file.
* Makes finding classes time consuming.
* Code review (code reading in general) is hampered by the complexity.
* Encourages coupling of classes in same file due to [locality principle](https://en.wikipedia.org/wiki/Principle_of_locality).

## Classes contains multiple responsibilities (domain behavior)

* [User constructor](User.cs#L24) validates non-empty names.
* [Item constructor](Item.cs#L15) validates non-empty names and non-negative prices.

As a result we may experience one or more of the following:

* DRY violations make it difficult to find and fix all "copies" of domain behavior. An example is demonstrated - where FirstName initialization was fixed to validate non-whitespace names but LastName was not.
* Release delays due to changing (and testing and packaging and deploying) multiple "copies" of domain behavior.