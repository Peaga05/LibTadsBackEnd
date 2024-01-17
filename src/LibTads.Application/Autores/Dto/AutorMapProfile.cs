using Abp.Authorization;
using AutoMapper;
using LibTads.Authorization.Roles;
using LibTads.Domain;
using LibTads.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Autores.Dto
{
    public class AutorMapProfile : Profile
    {
     
        public AutorMapProfile()
        {
            CreateMap<CreateAutorDto, Autor>();
            CreateMap<AutorDto, Autor>();
            CreateMap<Autor, ReadAutorDto>();
            CreateMap<UpdateAutorDto, Autor>();
        }
    }
}
