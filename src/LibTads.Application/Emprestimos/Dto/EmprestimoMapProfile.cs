using AutoMapper;
using LibTads.Domain;
using LibTads.Livros.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Emprestimos.Dto
{
    public class EmprestimoMapProfile : Profile
    {
        public EmprestimoMapProfile()
        {
            CreateMap<CreateEmprestimoDto, Emprestimo>();
            CreateMap<EmprestimoDto, Emprestimo>();
            CreateMap<Emprestimo, ReadEmprestimoDto>();
            CreateMap<UpdateEmprestimoDto, Emprestimo>();
        }
    }
}
