﻿using FluentValidation;
using Uni_Mate.Common.Helper;
using Uni_Mate.Features.Common.ApartmentManagement.ApartmerntDTO;

namespace Uni_Mate.Features.ApartmentManagment.GetApartment;
public class GetApartmentViewModel : QueryStringParamater
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetApartmentValidator : AbstractValidator<GetApartmentViewModel>
{
    public GetApartmentValidator()
    {
        RuleFor(x => x.PageNumber)
            .NotEmpty()
            .WithMessage("Page number is required.")
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");
    }
}

public class GetApartmentResponseViewModel
{
    public List<GetApartmentDTO> Apartments { get; set; } = new List<GetApartmentDTO>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
