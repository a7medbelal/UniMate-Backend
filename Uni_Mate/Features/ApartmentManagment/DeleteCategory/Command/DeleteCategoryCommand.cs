using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.DeleteCategory.Command
{
    public record DeleteCategoryCommand(int Id) : IRequest<RequestResult<bool>>;

    public class DeleteCategoryHandler : BaseRequestHandler<DeleteCategoryCommand,RequestResult<bool>,Category>
    {
        public DeleteCategoryHandler(BaseRequestHandlerParameter<Category> parameters) : base(parameters)
        {
        }

        public override async Task<RequestResult<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "Invalid category ID.");
            }

            var category = await _repository.GetByIDAsync(request.Id);
            if (category == null)
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.NotFound, "Category not found.");
            }
            await _repository.DeleteAsync(category);
            await _repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Category deleted successfully.");
        }
    }
}
