using Abp.Application.Services.Dto;

namespace LibTads.Autores.Dto
{
    public class PagedAutorResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

