using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Domain
{
    public class Livro : Entity<int>
    {
        [Required(ErrorMessage = "O campo título é obrigatório.")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "O campo ISBN é obrigatório."), StringLength(13, ErrorMessage = "O campo ISBN só pode ter 13 caracteres")]
        public string Isbn { get; set; }
        [Required(ErrorMessage = "O campo quantidade é obrigatório.")]
        public int Quantidade { get; set; }
        public string QrCode { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        [ForeignKey(nameof(AutorId))]
        public int AutorId { get; set; }
        public virtual Autor Autor { get; set; }
        [Required]
        [ForeignKey(nameof(GeneroId))]
        public int GeneroId { get; set; }
        public virtual Genero Genero { get; set; }
    }
}
