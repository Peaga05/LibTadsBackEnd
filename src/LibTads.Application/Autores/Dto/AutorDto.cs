using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LibTads.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Autores.Dto
{
    [AutoMapFrom(typeof(Autor))]
    public class AutorDto : EntityDto<int>
    {

        [Required(ErrorMessage = "O nome do autor é obrigatório.")]
        public string Nome { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
