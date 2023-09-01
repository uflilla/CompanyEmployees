using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees
{
  public class MapperProfile: Profile
  {
    public MapperProfile()
    {
      CreateMap<Company, CompanyDto>().ForMember(c=>c.FullAddress,
        opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
      CreateMap<Employee, EmployeeDto>();
      CreateMap<CompanyForCreationDto, Company>();
      CreateMap<EmployeeForCreationDto, Employee>();
    }
  }
}