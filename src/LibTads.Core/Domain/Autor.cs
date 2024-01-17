using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LibTads.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Domain
{
    public class Autor : Entity<int>
    {
        [Required(ErrorMessage = "O nome do autor é obrigatório.")]
        public string Nome {  get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
