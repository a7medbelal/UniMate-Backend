using MediatR;
using Uni_Mate.Common.BaseHandlers;
using Uni_Mate.Common.Views;
using Uni_Mate.Models.ApartmentManagement;

namespace Uni_Mate.Features.ApartmentManagment.AddCategory.Command
{
    public record AddCategoryCommand(string Name) : IRequest<RequestResult<bool>>;

    public class AddCategoryHandler : BaseRequestHandler<AddCategoryCommand, RequestResult<bool>, Category>
    {
        public AddCategoryHandler(BaseRequestHandlerParameter<Category> parameters) : base(parameters)
        {
        }
        public async override Task<RequestResult<bool>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.Name))
            {
                return RequestResult<bool>.Failure(Uni_Mate.Common.Data.Enums.ErrorCode.InvalidData, "Category name cannot be empty.");
            }

            var category = new Category
            {
                Name = request.Name
            };
            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Category added successfully.");
        }
    }
}
