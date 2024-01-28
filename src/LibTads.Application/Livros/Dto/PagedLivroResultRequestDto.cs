using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Livros.Dto
{
    public class PagedLivroResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
