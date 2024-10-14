using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Filters;
using WebApi.Data.Entities;

namespace WebApi.Features.FavoriteGadgets;

[ApiController]
[JwtValidation]
[RolesFilter(Role.Customer)]
[RequestValidation<Request>]
public class CustomerCreateFavoriteGadget : ControllerBase
{
    public new class Request
    {
        public Guid CustomerId { get; set; }
        public Guid GadgetId { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(fg => fg.CustomerId)
                .NotEmpty()
                .WithMessage("CustomerId không được để trống");
            RuleFor(fg => fg.GadgetId)
                .NotEmpty()
                .WithMessage("GadgetId không được để trống");
        }
    }
}
