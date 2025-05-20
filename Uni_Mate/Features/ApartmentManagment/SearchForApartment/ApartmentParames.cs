using FluentValidation;
using Uni_Mate.Common.Helper;
using Uni_Mate.Features.ApartmentManagment.SearchForApartment.Queries;
using Uni_Mate.Models.GeneralEnum;

namespace Uni_Mate.Features.ApartmentManagment.SearchForApartment
{
    public class ApartmentParames :QueryStringParamater   
    {
      public  string? Keyword    {get ;set ;}
      public  decimal? FromPrice {get ;set ;}
      public  decimal? ToPrice   {get ;set ;}
      public  int? Capacity      {get ;set ;}
      public  string? Location   {get ;set ;}
      public  Gender? Gender     {get ;set ;}
      public SortOption? SortBy { get; set; } = SortOption.None;
    }
    public class ApartmentParamesValidator : AbstractValidator<ApartmentParames>
    {
        public ApartmentParamesValidator()
        {
            // validation rules here
        }
    }
}