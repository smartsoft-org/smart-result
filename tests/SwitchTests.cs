using FluentAssertions;
using SmartResult;

namespace Tests;

public class SwitchTests
{
    private record Person(string Name);

    [Fact]
    public void SwitchSmartResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new Person("Amichai");
        void OnValueAction(Person person) => person.Should().BeEquivalentTo(errorOrPerson.Value);
        void OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        var action = () => errorOrPerson.Switch(
            OnValueAction,
            OnErrorsAction);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void SwitchSmartResult_WhenHasError_ShouldExecuteOnErrorAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        void OnValueAction(Person _) => throw new Exception("Should not be called");
        void OnErrorsAction(IReadOnlyList<Error> errors) => errors.Should().BeEquivalentTo(errorOrPerson.Errors);

        // Act
        var action = () => errorOrPerson.Switch(
            OnValueAction,
            OnErrorsAction);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void SwitchFirstSmartResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new Person("Amichai");
        void OnValueAction(Person person) => person.Should().BeEquivalentTo(errorOrPerson.Value);
        void OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        var action = () => errorOrPerson.SwitchFirst(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void SwitchFirstSmartResult_WhenHasError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        void OnValueAction(Person _) => throw new Exception("Should not be called");
        void OnFirstErrorAction(Error errors)
            => errors.Should().BeEquivalentTo(errorOrPerson.Errors[0])
                .And.BeEquivalentTo(errorOrPerson.FirstError);

        // Act
        var action = () => errorOrPerson.SwitchFirst(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        action.Should().NotThrow();
    }
}
