using Abp.Application.Services.Dto;
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

    }
}
