using AutoMapper;
using LibTads.Autores.Dto;
using LibTads.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Generos.Dto
{
    public class GeneroMapProfile : Profile
    {
        public GeneroMapProfile()
        {
            CreateMap<CreateGeneroDto, Genero>();
            CreateMap<GeneroDto, Genero>();
            CreateMap<Genero, ReadGeneroDto>();
            CreateMap<UpdateGeneroDto, Genero>();
        }
    }
}
