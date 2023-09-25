using Stash_And_Grab.Application.DtoModels;

namespace Stash_And_Grab.Application.UnitTests.ValidationTests;

public class StashItemValidationTests
{
    private readonly StashItemValidation _sut = new();

    [Fact]
    public void StashItem_ShouldPassValidation_WhenAllPropsAreValid()
    {
        //Arrange
        var itemDto = new StashCreateDtoModel("stash01", "stash", "string");

        //Act
        var result = _sut.TestValidate(itemDto);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void StashItem_ShouldFailValidation_WhenStashNameIsEmpty()
    {
        //Arrange
        var itemDto = new StashCreateDtoModel("", "stash", "string");

        //Act
        var result = _sut.TestValidate(itemDto);

        //Assert
        result.ShouldHaveValidationErrorFor(item => item.StashName);
    }

    [Fact]
    public void StashItem_ShouldFailValidation_WhenStashDataIsEmpty()
    {
        //Arrange
        var itemDto = new StashCreateDtoModel("stash01", "", "string");

        //Act
        var result = _sut.TestValidate(itemDto);

        //Assert
        result.ShouldHaveValidationErrorFor(item => item.StashData);
    }

    [Fact]
    public void StashItem_ShouldFailValidation_WhenStashTypeIsEmpty()
    {
        //Arrange
        var itemDto = new StashCreateDtoModel("stash01", "stash", "");

        //Act
        var result = _sut.TestValidate(itemDto);

        //Assert
        result.ShouldHaveValidationErrorFor(item => item.StashType);
    }
}