using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Autores.Dto
{
    public class PagedAutoresRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
