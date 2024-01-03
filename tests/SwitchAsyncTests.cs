using FluentAssertions;
using SmartResult;

namespace Tests;

public class SwitchAsyncTests
{
    private record Person(string Name);

    [Fact]
    public async Task SwitchAsyncSmartResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new Person("Amichai");
        Task OnValueAction(Person person) => Task.FromResult(person.Should().BeEquivalentTo(errorOrPerson.Value));
        Task OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        var action = async () => await errorOrPerson.SwitchAsync(
            OnValueAction,
            OnErrorsAction);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SwitchAsyncSmartResult_WhenHasError_ShouldExecuteOnErrorAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task OnValueAction(Person _) => throw new Exception("Should not be called");
        Task OnErrorsAction(IReadOnlyList<Error> errors) => Task.FromResult(errors.Should().BeEquivalentTo(errorOrPerson.Errors));

        // Act
        var action = async () => await errorOrPerson.SwitchAsync(
            OnValueAction,
            OnErrorsAction);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SwitchAsyncFirstSmartResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new Person("Amichai");
        Task OnValueAction(Person person) => Task.FromResult(person.Should().BeEquivalentTo(errorOrPerson.Value));
        Task OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        var action = async () => await errorOrPerson.SwitchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SwitchFirstAsyncSmartResult_WhenHasError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task OnValueAction(Person _) => throw new Exception("Should not be called");
        Task OnFirstErrorAction(Error errors)
            => Task.FromResult(errors.Should().BeEquivalentTo(errorOrPerson.Errors[0])
                .And.BeEquivalentTo(errorOrPerson.FirstError));

        // Act
        var action = async () => await errorOrPerson.SwitchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        await action.Should().NotThrowAsync();
    }
}
