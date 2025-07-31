using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Application.Features.Account.DTO;
using RecordManagementSystem.Domain.Entities.Account;


namespace RecordManagementSystem.Application.Common.Mappings
{
    public class MappingProfileApp : Profile
    {
        public MappingProfileApp()
        {
            CreateMap<AddStudentAccountDTO,StudentUserAccount>();
            CreateMap<StudentUserAccount, AddStudentAccountDTO>();
        }
    }
}
