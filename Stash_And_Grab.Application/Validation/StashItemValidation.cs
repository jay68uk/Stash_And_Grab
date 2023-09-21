using FluentValidation;

namespace Stash_And_Grab.Application.Validation;

internal class StashItemValidation : AbstractValidator<StashCreateDtoModel>
{
    public StashItemValidation()
    {
        RuleFor(item => item.StashName).NotEmpty();

        RuleFor(item => item.StashData).NotEmpty();

        RuleFor(item => item.StashType).NotEmpty();
    }
}