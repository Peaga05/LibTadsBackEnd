using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LibTads.Authorization.Users;
using LibTads.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Emprestimos.Dto
{
    [AutoMapFrom(typeof(Emprestimo))]
    public class EmprestimoDto : EntityDto<int>
    {
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public int LivroId { get; set; }
        public long UserId { get; set; }
        public string TituloLivro { get; set; }
        public string NomeUsuario { get; set; }
    }
}
