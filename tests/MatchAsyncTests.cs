using FluentAssertions;
using SmartResult;

namespace Tests;

public class MatchAsyncTests
{
    private record Person(string Name);

    [Fact]
    public async Task MatchAsyncSmartResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new Person("Amichai");
        Task<string> OnValueAction(Person person)
        {
            person.Should().BeEquivalentTo(errorOrPerson.Value);
            return Task.FromResult("Nice");
        }

        Task<string> OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        // Act
        var action = async () => await errorOrPerson.MatchAsync(
            OnValueAction,
            OnErrorsAction);

        // Assert
        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }

    [Fact]
    public async Task MatchAsyncSmartResult_WhenHasError_ShouldExecuteOnErrorAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> OnValueAction(Person _) => throw new Exception("Should not be called");

        Task<string> OnErrorsAction(IReadOnlyList<Error> errors)
        {
            errors.Should().BeEquivalentTo(errorOrPerson.Errors);
            return Task.FromResult("Nice");
        }

        // Act
        var action = async () => await errorOrPerson.MatchAsync(
            OnValueAction,
            OnErrorsAction);

        // Assert
        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }

    [Fact]
    public async Task MatchFirstAsyncSmartResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new Person("Amichai");
        Task<string> OnValueAction(Person person)
        {
            person.Should().BeEquivalentTo(errorOrPerson.Value);
            return Task.FromResult("Nice");
        }

        Task<string> OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        var action = async () => await errorOrPerson.MatchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }

    [Fact]
    public async Task MatchFirstAsyncSmartResult_WhenHasError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        SmartResult<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> OnValueAction(Person _) => throw new Exception("Should not be called");
        Task<string> OnFirstErrorAction(Error errors)
        {
            errors.Should().BeEquivalentTo(errorOrPerson.Errors[0])
                .And.BeEquivalentTo(errorOrPerson.FirstError);

            return Task.FromResult("Nice");
        }

        // Act
        var action = async () => await errorOrPerson.MatchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        // Assert
        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }
}
