using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LibTads.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Emprestimos.Dto
{
    [AutoMapFrom(typeof(Emprestimo))]
    public class UpdateEmprestimoDto : EntityDto<int>
    {
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public int LivroId { get; set; }
        public long UserId { get; set; }
    }
}
