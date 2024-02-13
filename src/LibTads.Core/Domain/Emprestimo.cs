using Abp.Domain.Entities;
using LibTads.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Domain
{
    public class Emprestimo : Entity<int>
    {
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey(nameof(LivroId))]
        public int LivroId { get; set; }
        public virtual Livro Livro { get; set; }
        [ForeignKey(nameof(UserId))]
        public long UserId { get; set; }
        public virtual User Usuario { get; set; }
    }
}
