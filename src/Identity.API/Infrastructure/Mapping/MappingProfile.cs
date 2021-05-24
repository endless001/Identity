using Account.API.Grpc;
using AutoMapper;
using Identity.API.Models;

namespace Identity.API.Infrastructure.Mapping
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
            CreateMap<AccountModel, AccountRequest>().ReverseMap();
    }
  }
}
