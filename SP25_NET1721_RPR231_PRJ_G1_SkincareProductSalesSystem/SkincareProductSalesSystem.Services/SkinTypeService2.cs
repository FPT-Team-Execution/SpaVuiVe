using Google.Protobuf.Collections;
using Grpc.Core;
using Protos.SkinTypesService;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkincareProductSalesSystem.Services
{
    public class SkinTypeService2 : SkinTypesService.SkinTypesServiceBase
    {
        private readonly UnitOfWork _unitOfWork;

        public SkinTypeService2()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public override async Task<ServiceResulSingleProto> Create(CreateSkinTypeRequestProto request, ServerCallContext context)
        {
            var newSkinType = new SkinType
            {
                SkinTypeId = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                Characteristics = request.Characteristics,
                AvoidIngredients = request.AvoidIngredients,
                CareInstructions = request.CareInstructions,
                IsActive = false, //default is not active
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.SkinTypeRepository.CreateAsync(newSkinType);
            await _unitOfWork.SkinTypeRepository.SaveAsync();
            return new ServiceResulSingleProto
            {
                Status = 200,
                Message = "Thành công",
                Data = new SkinTypeProto
                {
                    SkinTypeId = newSkinType.SkinTypeId ?? "",
                    Name = newSkinType.Name ?? "",
                    Description = newSkinType.Description ?? "",
                    Characteristics = newSkinType.Characteristics ?? "",
                    AvoidIngredients = newSkinType.AvoidIngredients ?? "",
                    CareInstructions = newSkinType.CareInstructions ?? ""
                }
            };
        }

        public override async Task<ServiceResulSingleProto> Delete(GetByIdRequestProto request, ServerCallContext context)
        {
            var skinType = await _unitOfWork.SkinTypeRepository.GetByIdAsync(request.Id);
            if (skinType == null)
                return new ServiceResulSingleProto
                {
                    Status = 404,
                    Message = "Không tìm thấy",
                    Data = null
                };
            await _unitOfWork.SkinTypeRepository.RemoveAsync(skinType);
            await _unitOfWork.SkinTypeRepository.SaveAsync();
            return new ServiceResulSingleProto
            {
                Status = 200,
                Message = "Thành công",
                Data = new SkinTypeProto
                {
                    SkinTypeId = skinType.SkinTypeId ?? "",
                    Name = skinType.Name ?? "",
                    Description = skinType.Description ?? "",
                    Characteristics = skinType.Characteristics ?? "",
                    AvoidIngredients = skinType.AvoidIngredients ?? "",
                    CareInstructions = skinType.CareInstructions ?? ""
                }
            };
        }

        public override async Task<ServiceResulSingleProto> Get(GetByIdRequestProto request, ServerCallContext context)
        {
            var skinType = await _unitOfWork.SkinTypeRepository.GetByIdAsync(request.Id);
            if (skinType == null)
                return new ServiceResulSingleProto
                {
                    Status = 404,
                    Message = "Không tìm thấy",
                    Data = null
                };
            return new ServiceResulSingleProto
            {
                Status = 200,
                Message = "Thành công",
                Data = new SkinTypeProto
                {
                    SkinTypeId = skinType.SkinTypeId ?? "",
                    Name = skinType.Name ?? "",
                    Description = skinType.Description ?? "",
                    Characteristics = skinType.Characteristics ?? "",
                    AvoidIngredients = skinType.AvoidIngredients ?? "",
                    CareInstructions = skinType.CareInstructions ?? ""
                }
            };
        }

        public override async Task<ServiceResulListProto> GetAll(EmptyRequestProto _, ServerCallContext context)
        {
            var skinTypes = await _unitOfWork.SkinTypeRepository.GetAllAsync();
            RepeatedField<SkinTypeProto> list = [];
            skinTypes.ForEach(st => list.Add(new SkinTypeProto
            {
                SkinTypeId = st.SkinTypeId ?? "",
                Name = st.Name ?? "",
                Description = st.Description ?? "",
                Characteristics = st.Characteristics ?? "",
                AvoidIngredients = st.AvoidIngredients ?? "",
                CareInstructions = st.CareInstructions ?? ""
            }));
            ServiceResulListProto result = new();
            result.Status = 200;
            result.Message = "Thành công";
            result.Data.AddRange(list);
            return result;
        }

        public override async Task<ServiceResulSingleProto> Update(UpdateSkinTypeRequestProto request, ServerCallContext context)
        {
            var skinType = await _unitOfWork.SkinTypeRepository.GetByIdAsync(request.Id);
            if (skinType == null)
                return new ServiceResulSingleProto
                {
                    Status = 404,
                    Message = "Không tìm thấy",
                    Data = null
                };

            //update skin type
            skinType.Name = request.Name;
            skinType.Description = request.Description;
            skinType.Characteristics = request.Characteristics;
            skinType.AvoidIngredients = request.AvoidIngredients;
            skinType.CareInstructions = request.CareInstructions;
            //* update update time
            skinType.UpdatedAt = DateTime.Now;


            await _unitOfWork.SkinTypeRepository.UpdateAsync(skinType);
            await _unitOfWork.SkinTypeRepository.SaveAsync();
            return new ServiceResulSingleProto
            {
                Status = 200,
                Message = "Thành công",
                Data = new SkinTypeProto
                {
                    SkinTypeId = skinType.SkinTypeId,
                    Name = skinType.Name,
                    Description = skinType.Description,
                    Characteristics = skinType.Characteristics,
                    AvoidIngredients = skinType.AvoidIngredients,
                    CareInstructions = skinType.CareInstructions
                }
            };

        }

    }


}

