using AutoMapper;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.DTO.Account;

namespace RecordManagementSystem.Map
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountDTO,AddStudentAccountDTO>();
        }
    }
}
