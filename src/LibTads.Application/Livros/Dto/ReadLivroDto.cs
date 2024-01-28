﻿using LibTads.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace LibTads.Livros.Dto
{
    public class ReadLivroDto : EntityDto<int>
    {
        [Required(ErrorMessage = "O campo título é obrigatório.")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "O campo ISBN é obrigatório."), StringLength(11, ErrorMessage = "O campo ISBN só pode ter 11 caracteres")]
        public string Isbn { get; set; }
        [Required(ErrorMessage = "O campo quantidade é obrigatório.")]
        public int Quantidade { get; set; }
        public string QrCode { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public string NomeAutor {  get; set; }
        public int AutorId { get; set; }
        public int GeneroId { get; set; }
        public string DescricaoGenero { get; set; }
    }
}
