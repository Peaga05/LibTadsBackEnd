using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Domain
{
    public class Genero : Entity<int>
    {

        [Required(ErrorMessage = "A descrição do gênero é obrigatório.")]
        public string Descricao { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
