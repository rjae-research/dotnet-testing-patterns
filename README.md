# dotnet-testing-patterns
![](https://github.com/rjae-testing/dotnet-testing-patterns/workflows/Integration/badge.svg)

# Contents

* [Unit Testing](#Unit-Testing)
  * [Patterns and Practices](#Patterns-and-Practices)
    * [Use Single Responsibility Principle](#Use-Single-Responsibility-Principle)
    * [Create Test-Oriented Code](#Create-Test-Oriented-Code)
    * [Optimize Test Execution](#Optimize-Test-Execution)
    * [Create Reader-Oriented Code](#Create-Reader-Oriented-Code)
    * [Optimize Your Testing Practice](#Optimize-Your-Testing-Practice)
  * [FAQ](#FAQ)
    * [Is 100% code coverage really necessary?](#Is-100-code-coverage-really-necessary)
    * [This is a lot of testing, where do I start!?](#This-is-a-lot-of-testing-where-do-I-start)
    * [How do I know what to assert?](#How-do-I-know-what-to-assert)
    * [Can a test have multiple asserts?](#Can-a-test-have-multiple-asserts)
    * [Are there any assertion "best practices"?](#Are-there-any-assertion-best-practices)
    * [What are test doubles?](#What-are-test-doubles)
    * [Should my tests use test doubles?](#Should-my-tests-use-test-doubles)
    * [Should I use a mocking framework?](#Should-I-use-a-mocking-framework)
    * [How fast do my tests need to be?](#How-fast-do-my-tests-need-to-be)

# Unit Testing

* To do: introduction and overview

## Patterns and Practices

### Use Single Responsibility Principle

* Use single responsibility principle in testing just as we do in production code.
* Test projects, files, classes, facts, and theories must have one single responsibility.
* Tests must assert the expected behavior of a single responsibility.
* Tests may contain multiple asserts but must not contain multiple responsibilities.
* Remember that a responsibility represents a vector of change. Multiple vectors of change can be brittle and more difficult to understand.

See [Multiple Responsibility Project](SingleResponsibilityPrinciple/Anti-pattern/MultipleResponsibility/README.md) for more details and code examples.

### Create Test-Oriented Code

* Avoid use of static methods. They cannot be overridden. They are cumbersome to mock. They may leak side effects when used in a suite of tests.
* Prefer virtual methods as they make for more testable code.
  * Test doubles may be used to override orthogonal behavior.
  * Easier to independently test, e.g., `InvokeMyVirtualMethod() => MyVirtualMethod()`.
  * Much faster test execution as resource hungry implementations are overridden.
  * Symbiotic with single responsibility principle, leading to better tests.
* Refactor helpful testing patterns to common test libraries. Promotes good patterns and demotes poor patterns.
* Common testing patterns are great. Not-so-common testing patterns are horrible. Prefer private patterns until you have derived a truly common test pattern. There is no harm in having multiple MockProducer classes if their behavior is unique. There is harm in forcing a single MockProducer to juggle multiple testing responsibilities: brittle test code.

### Optimize Test Execution

* Avoid use of mocking frameworks where possible. It takes next to no time to compose test doubles - which have no overhead. Mocking frameworks on the other hand incur hundreds of milliseconds of overhead. Multiply that by 1000, 5000, 10000 tests and the additional overhead becomes significant. This has two serious impacts. Your build times will get longer and developers will stop running all the tests as frequently as they should due to the long test durations.
* Identify and minimize other runtime overheads. For example, in-memory databases must generate a model for every context the first time a context is used. Similar to mocking frameworks, this can introduce hundreds of milliseconds of overhead.
* Test methods must take 100 milliseconds or less. If tests are taking longer than that, the method under test may need to be optimized and/or the testing strategy may need to be altered.

### Create Reader-Oriented Code

* Test methods must have fully descriptive names. They must specify the following: member, behavior, and preconditions. For example, `AuthorizeTokenMustThrowExceptionWhenTokenIsNullOrWhitespace`.
* Consistency should be viewed from the perspective of current and future team members. Inconsistency in naming, layout, etc. produces a less understood system.
* Do not allow logging and console actions to be sent to standard out. Without this practice, important build messages will be easy to miss. Carefully mock standard out and console logging providers.
* Generously assert logging output. If unit-tests protect from unwanted released behavior, then logging aids in improving already released behavior. Your tests must assert what you expect from your logging. Think of it from the perspective of: what do I want my system to log when I am troubleshooting production at 3 am? Write tests for that!

### Optimize Your Testing Practice

* Run all tests before pushing. Ideally you should use a shell script or git hook that requires all tests passing before allowing commits to be pushed.
* Never ignore failing tests, especially intermittently failing tests. If you absolutely cannot fix the test now, create a ticket and place it in ready-for-development (or similar) status.
* Null is your friend. Do not be afraid to pass null to a method under test. If the parameter is not depended upon in your specific test, there is no need to spend time on it. A side benefit of this is that you implicitly assert that the parameter can accept null. If that changes in the future, your test will alert you by failing.
* Commit to very high percentage of code coverage. The higher the test coverage, the less risk is involved in making changes; it's easy to tell when new changes break existing behavior.


## FAQ

### Is 100% code coverage really necessary?

The question behind this question is whether there is sufficient value derived from the effort of covering 100% of our code. The answer is that it depends on how we go about measuring what code it is we're covering. In particular, it means covering 100% of the code that we deem meaningful to cover, in a way that tells us meaningful things about the code.

If all we did was create tests that orchestrated our code through all lines and branches without meaningful asserts that tested the behavior, then we would not have much to show for all the effort - other than pretty charts. Likewise, If we include generated code and state-only types (e.g., data transfer objects) in our code coverage, then again we would not have much to show for all the effort. In general, what we're talking about covering 100% is code that has behavior (even if it's as simple as object creation via a constructor) and that also has meaningful asserts on that behavior.

The sweet spot definition of 100% code coverage is unique for each team, but the following rules will get you started.

* Exclude generated code
* Exclude test code
* Exclude state-only types
* Exclude third party libraries
* Include all other code
* Assert on all "meaningful" behavior, no matter how simple

### This is a lot of testing, where do I start!?

Let's face it: any of the daunting tasks before us feel like this before we get started. The best thing we can do is start smart. Begin in a way that is strategic and will yield exactly what we need right now. Here's a script to get us started:

1. Generate baseline code coverage reports. This tells us where we are starting from and may reveal some surprises in our code.
1. Add code coverage analysis to build pipeline. Doing this early in the process helps you stay committed to closing the gap over time.
1. To write tests for existing code:
   * Write tests for simple, well-refactored code. This builds confidence in test writing and generates testing patterns to be reused.
   * Write tests for complex and/or error prone code. This puts much of the code under test without creating vectors of change. There is an entire set of patterns dealing with legacy code that's hard to test. See [Working Effectively with Legacy Code](https://www.amazon.com/dp/0131177052/) for example.
   * Refactor code as necessary to reach 100% coverage. By writing tests along with the refactoring we are more likely to create the results we are after.
1. To write tests for new code:
    * Try to start writing the tests first. This will drive you to create simple well-factored code, primarily by forcing you to inject dependencies and to only write code that is required by the test.
    * As you add tests, you'll add behavior and refactor the code to support the new test.

### How do I know what to assert?

We want our assertions to test system state before and after the behavior we are testing executes. If we picture what we are testing as a state machine then our canonical test is: _given_ \<pre-conditions\> _when_ \<behavior\> _then_ \<post-conditions\>. Therefore we want to write assertions for pre-conditions and post-conditions.

Post-conditions may vary depending on unique paths through the behavior under test. For example the post-condition of passing a null argument may be an ArgumentNullException. Use your coverage reporting as a guide to which post-conditions are possible for a given behavior.

### Can a test have multiple asserts?

A test can and should have multiple asserts if multiple post-condition changes exist after the behavior under test has executed. Don't Repeat Yourself (DRY) applies to unit testing as well: if two tests have identical test code but exist to assert different post-conditions then refactor them into one test with multiple assertions.

The point is that whatever post-condition changes exist, we must have assertions to test them.

### Are there any assertion "best practices"?

* Assertions should be as restrictive as possible (e.g., assert specific value vs. not-null).
* Assertions should not affect the system under test.
* Prefer specific assertions vs. `Assert.True` (e.g., `Assert.Empty` vs. `Count == 0`).
* Include description of assertion with assert (tested documentation!).
* Assert pre-conditions to show nothing up our sleeves.

### What are test doubles?

"Test Doubles" is the abstract term for fakes, stubs, mocks, dummies, nulls, and other objects that stand in for an object from the system under test.

* _Fake objects_ have working implementations that differ from _real objects_ by taking some shortcuts.
* _Stub objects_ respond with test data instead of getting real data from services (e..g, list of test customers).
* _Mock objects_ expose methods and properties to examine and assert post-conditions (e.g., number of times a method was called).
* _Dummy objects_ are passed as parameter arguments but are not used.
* _Null objects_ have "no-op" implementations that are used to bypass real implementations (e.g., NullLogger does nothing with log events).

### Should my tests use test doubles?

Yes. Test Doubles are extremely useful in establishing seams so that specific behavior is asserted.

An argument is often made against using test doubles because their use can alter the behavior under test. It is true that we must write tests without use of test doubles - but those tests are narrow-scoped _integration tests_, not unit tests. Both forms of testing are critical pieces of the full testing suite.

### Should I use a mocking framework?

No. Mocking frameworks greatly increase the amount of time it takes to run tests. You can do your own verification of this, but research has already been published comparing popular mocking frameworks.

In practice it does not take long to compose test doubles, and if done in a well-patterned way then they can be reused by other tests and even other test modules. An advantage to creating our own test doubles is that they increase our knowledge (tested documentation!) of the system under test.

### How fast do my tests need to be?

The real answer is: as fast as possible. Our tests need to be fast because we must run the tests every single time we push changes to the system. If the full test suite takes "too long" then the feedback loop is too long. We need to know about test breaks as well as code coverage as quickly as possible. In practice we should strive for single-digit milliseconds for each test.
