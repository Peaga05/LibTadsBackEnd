using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LibTads.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Livros.Dto
{
    [AutoMapFrom(typeof(Livro))]
    public class LivroDto : EntityDto<int>
    {
        [Required(ErrorMessage = "O campo título é obrigatório.")]
        public string Titulo { get; set; }
        [StringLength(13, ErrorMessage = "O campo ISBN só pode ter 13 caracteres")]
        public string Isbn { get; set; }
        [Required(ErrorMessage = "O campo quantidade é obrigatório.")]
        public int Quantidade { get; set; }
        public int QuantidadeDisponivel { get; set; }
        public string QrCode { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public string NomeAutor { get; set; }
        public int AutorId { get; set; }
        public int GeneroId { get; set; }
        public string DescricaoGenero { get; set; }
    }
}
