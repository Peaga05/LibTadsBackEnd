using Abp.Application.Services.Dto;
using LibTads.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Autores.Dto
{
    public class ReadAutorDto : EntityDto<int>
    {
        public string Nome { get; set; }
        public virtual ICollection<Livro> Livros { get; set; }


    }
}
