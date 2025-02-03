using SkincareProductSalesSystem.Services.Models.PromotionModels;

namespace SkincareProductSalesSystem.Services.Services.Interfaces
{
	public interface IPromotionService
	{
		Task<CreatePromotionResponseModel> Create(CreatePromotionRequestModel dto);
		Task<bool> Delete(DeletePromotionRequestModel dto);
		Task<GetPromotionResponseModel> GetCodes();
		Task<bool> Update(UpdatePromotionRequestModel dto);
	}
}