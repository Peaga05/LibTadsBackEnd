using AutoMapper;
using LibTads.Domain;
using LibTads.Generos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Livros.Dto
{
    public class LivroMapProfile : Profile
    {
        public LivroMapProfile()
        {
            CreateMap<CreateLivroDto, Livro>();
            CreateMap<LivroDto, Livro>();
            CreateMap<Livro, ReadLivroDto>();
            CreateMap<UpdateLivroDto, Livro>();
        }
    }
}
