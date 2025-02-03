using AutoMapper;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Models.PromotionModels;
using SkincareProductSalesSystem.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Services
{
	public class PromotionService : IPromotionService
	{
		private UnitOfWork _uOW;
		private IMapper _mapper;

		public PromotionService(UnitOfWork uOW, IMapper mapper)
		{
			_uOW = uOW;
			_mapper = mapper;
		}

		public async Task<CreatePromotionResponseModel> Create(CreatePromotionRequestModel dto)
		{
			try
			{
				if (dto.StartDate > dto.EndDate)
					return new CreatePromotionResponseModel
					{
						IsSuccess = false,
						Message = "End Date must be after Start Date"
					};

				if (dto.EndDate < DateTime.UtcNow.AddHours(7))
					return new CreatePromotionResponseModel
					{
						IsSuccess = false,
						Message = "End Date must be after current date"
					};

				if (dto.Code.IsNullOrEmpty())
				{
					dto.Code = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8)).Substring(0, 16);
				};

				Promotion entity = _mapper.Map<Promotion>(dto);
				await _uOW.PromotionRepository.CreateAsync(entity);

				return new CreatePromotionResponseModel
				{
					Code = entity.Code,
					IsSuccess = true,
				};
			}
			catch (Exception ex)
			{
				return new CreatePromotionResponseModel
				{
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}

		public async Task<GetPromotionResponseModel> GetCodes()
		{
			try
			{
				var result = await _uOW.PromotionRepository.GetAllAsync();

				return new GetPromotionResponseModel
				{
					IsSuccess = true,
					promotions = result
				};
			}
			catch (Exception ex)
			{
				return new GetPromotionResponseModel
				{
					IsSuccess = false,
					Message = ex.Message
				};
			}
		}


		public async Task<bool> Delete(DeletePromotionRequestModel dto)
		{
			try
			{
				if (!dto.PromotionIds.Any())
					return false;

				foreach (var id in dto.PromotionIds)
				{
					var result = await _uOW.PromotionRepository.GetByIdAsync(id);
					if (result != null) await _uOW.PromotionRepository.RemoveAsync(result);
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public async Task<bool> Update(UpdatePromotionRequestModel dto)
		{
			try
			{
				var result = await _uOW.PromotionRepository.GetByIdAsync(dto.PromotionId);
				if (result == null)
					return false;

				result = _mapper.Map(dto, result);
				await _uOW.PromotionRepository.UpdateAsync(result);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}
	}
}
