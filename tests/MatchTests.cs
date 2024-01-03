using FluentAssertions;
using SmartResult;

namespace Tests;

public class MatchTests
{
    private record Person(string Name);

    [Fact]
    public void MatchSmartResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new Person("Amichai");
        string OnValueAction(Person person)
        {
            person.Should().BeEquivalentTo(errorOrPerson.Value);
            return "Nice";
        }

        string OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<string> action = () => errorOrPerson.Match(
            OnValueAction,
            OnErrorsAction);

        // Assert
        action.Should().NotThrow().Subject.Should().Be("Nice");
    }

    [Fact]
    public void MatchSmartResult_WhenHasError_ShouldExecuteOnErrorAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        string OnValueAction(Person _) => throw new Exception("Should not be called");

        string OnErrorsAction(IReadOnlyList<Error> errors)
        {
            errors.Should().BeEquivalentTo(errorOrPerson.Errors);
            return "Nice";
        }

        // Act
        Func<string> action = () => errorOrPerson.Match(
            OnValueAction,
            OnErrorsAction);

        // Assert
        action.Should().NotThrow().Subject.Should().Be("Nice");
    }

    [Fact]
    public void MatchFirstSmartResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new Person("Amichai");
        string OnValueAction(Person person)
        {
            person.Should().BeEquivalentTo(errorOrPerson.Value);
            return "Nice";
        }

        string OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        var action = () => errorOrPerson.MatchFirst(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        action.Should().NotThrow().Subject.Should().Be("Nice");
    }

    [Fact]
    public void MatchFirstSmartResult_WhenHasError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        string OnValueAction(Person _) => throw new Exception("Should not be called");
        string OnFirstErrorAction(Error errors)
        {
            errors.Should().BeEquivalentTo(errorOrPerson.Errors[0])
                .And.BeEquivalentTo(errorOrPerson.FirstError);

            return "Nice";
        }

        // Act
        var action = () => errorOrPerson.MatchFirst(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        action.Should().NotThrow().Subject.Should().Be("Nice");
    }
}
