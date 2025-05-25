using MediatR;
using Mapster;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Data.Enums;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdatePropertyInfoDisplay.Queries;

namespace Uni_Mate.Features.ApartmentManagment.UpdateApartment.UpdateApartmentInfoDisplay.Queries
{
	public class UpdateApartmentInfoDisplayQuery : IRequest<RequestResult<UpdateApartmetInfoDisplayDTO>>
	{
		public int ApartmentId { get; set; }

		public UpdateApartmentInfoDisplayQuery(int apartmentId)
		{
			ApartmentId = apartmentId;
		}
	}

	public class UpdateApartmentInfoDisplayQueryHandler : IRequestHandler<UpdateApartmentInfoDisplayQuery, RequestResult<UpdateApartmetInfoDisplayDTO>>
	{
		private readonly IRepository<Apartment> _repository;
		private readonly UserInfo _userInfo;

		public UpdateApartmentInfoDisplayQueryHandler(BaseRequestHandlerParameter<Apartment> parameters)
		{
			_repository = parameters.Repository;
			_userInfo = parameters.UserInfo;
		}

		public async Task<RequestResult<UpdateApartmetInfoDisplayDTO>> Handle(UpdateApartmentInfoDisplayQuery request, CancellationToken cancellationToken)
		{
			var apartment = await _repository.GetByIDAsync(request.ApartmentId);
			if (apartment == null)
			{
				return RequestResult<UpdateApartmetInfoDisplayDTO>.Failure(ErrorCode.NotFound, "Apartment not found");
			}

			if (apartment.OwnerID != _userInfo.ID)
			{
				return RequestResult<UpdateApartmetInfoDisplayDTO>.Failure(ErrorCode.Forbidden, "You are not authorized to access this apartment");
			}

			var dto = apartment.Adapt<UpdateApartmetInfoDisplayDTO>();
			return RequestResult<UpdateApartmetInfoDisplayDTO>.Success(dto);
		}
	}
}