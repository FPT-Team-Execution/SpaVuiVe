using AutoMapper;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;
using Protos.PromotionService;
using Protos.SkinTypesService;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using Solara.Main.Payload;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Metadata;

namespace SkincareProductSalesSystem.Services
{

	public class PromotionService2 : PromotionServiceGRPC.PromotionServiceGRPCBase
	{
		private UnitOfWork _uOW;
		private IMapper _mapper;

		public PromotionService2(UnitOfWork uOW, IMapper mapper)
		{
			_uOW = uOW;
			_mapper = mapper;
		}


		public override async Task<ServiceResultSingleProto> Create(CreatePromotionRequestProto request, ServerCallContext context)
		{
			try
			{
				if (request.StartDate > request.EndDate)
					return new ServiceResultSingleProto()
					{
						Status = 400,
						Message = "End Date must be after Start Date"
					};

				if (request.EndDate.ToDateTime() < DateTime.UtcNow)
					return new ServiceResultSingleProto()
					{
						Status = 400,
						Message = "End Date must be after current date"
					};

				if (string.IsNullOrEmpty(request.Code))
				{
					request.Code = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8)).Substring(0, 16);
				};

				Promotion entity = new()
				{
					Name = request.Name,
					Code = request.Code.ToUpper(),
					CreatedAt = DateTime.UtcNow,
					DiscountAmount = request.DiscountAmount,
					MinimumPurchase = Convert.ToDecimal(request.MinimumPurchase),
					StartDate = DateTime.SpecifyKind(request.StartDate.ToDateTime(), DateTimeKind.Utc),
					EndDate = DateTime.SpecifyKind(request.EndDate.ToDateTime(), DateTimeKind.Utc),
					UsageLimit = request.UsageLimit	
				};
				entity.CreatedAt = DateTime.UtcNow;
				await _uOW.PromotionRepository.CreateAsync(entity);

				return new ServiceResultSingleProto()
				{
					Status = 200,
					Message = "Thành công",
					Data = new PromotionProto()
					{
						Name = entity.Name,
						Code = entity.Code,
						CreatedAt =  Timestamp.FromDateTime(entity.CreatedAt ?? DateTime.UtcNow),
						DiscountAmount = Convert.ToInt32( entity.DiscountAmount),
						MinimumPurchase = Convert.ToInt32( entity.MinimumPurchase),
						StartDate = Timestamp.FromDateTime(entity.StartDate.ToUniversalTime()),
						EndDate= Timestamp.FromDateTime(entity.EndDate.ToUniversalTime()),
						UsageLimit = Convert.ToInt32(entity.UsageLimit),
						IsActive = Convert.ToBoolean(entity.IsActive)
					}
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return new ServiceResultSingleProto()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public override async Task<ServiceResultListProto> GetAll(Protos.PromotionService.EmptyPromotionRequestProto request, ServerCallContext context)
		{
			try
			{
				var result = await _uOW.PromotionRepository.GetAllAsync();

				RepeatedField<PromotionProto> list = [];
				foreach (var item in result)
				{
					list.Add(new PromotionProto
					{
						PromotionId = item.PromotionId,
						Name = item.Name,
						Code = item.Code,
						CreatedAt = Timestamp.FromDateTime(Convert.ToDateTime(item.CreatedAt).ToUniversalTime()),
						DiscountAmount = Convert.ToInt32(item.DiscountAmount),
						MinimumPurchase = Convert.ToInt32(item.MinimumPurchase),
						StartDate = Timestamp.FromDateTime(item.StartDate.ToUniversalTime()),
						EndDate = Timestamp.FromDateTime(item.EndDate.ToUniversalTime()),
						UsageLimit = Convert.ToInt32(item.UsageLimit),
						IsActive = Convert.ToBoolean(item.IsActive)
					});
				}

				var result2 = new ServiceResultListProto()
				{
					Status = 200,
					Message = "Thành công",
				};
				result2.Data.Add(list);
				return result2;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return new ServiceResultListProto()
				{
					Status = 500,
					Message = ex.Message
										
				};
			}
		}

		public override async Task<ServiceResultSingleProto> Get(Protos.PromotionService.GetByIdRequestProto request, ServerCallContext context)
		{
			try
			{
				var entity = await _uOW.PromotionRepository.GetByIdAsync(request.Id);

				return new ServiceResultSingleProto
				{
					Status = 200,
					Message = "Thành công",
					Data = new PromotionProto()
					{
						PromotionId = entity.PromotionId,
						Name = entity.Name,
						Code = entity.Code,
						CreatedAt = Timestamp.FromDateTime(Convert.ToDateTime(entity.CreatedAt).ToUniversalTime()),
						DiscountAmount = Convert.ToInt32(entity.DiscountAmount),
						MinimumPurchase = Convert.ToInt32(entity.MinimumPurchase),
						StartDate = Timestamp.FromDateTime(entity.StartDate.ToUniversalTime()),
						EndDate = Timestamp.FromDateTime(entity.EndDate.ToUniversalTime()),
						UsageLimit = Convert.ToInt32(entity.UsageLimit),
						IsActive = Convert.ToBoolean(entity.IsActive)
					},
				};
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return new ServiceResultSingleProto()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public override async Task<ServiceResultProto> Delete(DeletePromotionRequestProto request, ServerCallContext context)
		{
			try
			{
				if (request.Id.IsNullOrEmpty())
					return new ServiceResultProto()
					{
						Status = 400,
						Message = "ID không được để trống"
					};

				var entity = await _uOW.PromotionRepository.GetByIdAsync(request.Id);

				if (entity == null)
					return new ServiceResultProto()
					{
						Status = 404,
						Message = "Không tìm thấy ID"
					};

				await _uOW.PromotionRepository.RemoveAsync(entity);
				return new ServiceResultProto()
				{
					Status = 200,
					Message = "Thành công",
				};
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
				return new ServiceResultProto()
				{
					Status = 500,
					Message = ex.Message
				};
			}
		}

		public override async Task<ServiceResultProto> Update(UpdatePromotionRequestProto request, ServerCallContext context)
		{
			try
			{
				var result = await _uOW.PromotionRepository.GetByIdAsync(request.PromotionId);
				if (result == null)
					return new ServiceResultProto()
					{
						Status = 404,
						Message = "Không tìm thấy"
					};

				//result = _mapper.Map(request, result);

				result.Code = request.Code.ToUpper();
				result.Name = request.Name;
				result.DiscountAmount = request.DiscountAmount;
				result.MinimumPurchase = request.MinimumPurchase;
				result.StartDate = request.StartDate.ToDateTime();
				result.EndDate = request.EndDate.ToDateTime();
				result.UsageLimit = request.UsageLimit;
 

				await _uOW.PromotionRepository.UpdateAsync(result);
				return new ServiceResultProto()
				{
					Status = 200,
					Message = "Thành công",
				};
			}

			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return new ServiceResultProto()
				{
					Status = 500,
					Message = ex.Message,
				};
			}
		}
	}
}
