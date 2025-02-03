using AutoMapper;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Models.AuthModels;
using SkincareProductSalesSystem.Services.Models.PromotionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services.Configs
{
	public class AutoMapperConfig : Profile
	{
        public AutoMapperConfig()
        {
			CreateMap<RegisterModel, User>()
				.ForMember(b => b.Email, opt => opt.MapFrom(a => a.Email))
				.ForMember(b => b.Username, opt => opt.MapFrom(a => a.Username))
				.ForMember(b => b.PhoneNumber, opt => opt.MapFrom(a => a.PhoneNumber))
				.ForMember(b => b.FullName, opt => opt.MapFrom(a => a.FullName));

			CreateMap<CreatePromotionRequestModel, Promotion>();

			CreateMap<UpdatePromotionRequestModel, Promotion>()
				.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
		}
    }
}
