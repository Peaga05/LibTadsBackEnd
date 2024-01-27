﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LibTads.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Generos.Dto
{
    [AutoMapFrom(typeof(Genero))]
    public class UpdateGeneroDto : EntityDto<int>
    {
        [Required(ErrorMessage = "A descrição do gênero é obrigatório.")]
        public string Descricao { get; set; }
    }
}
