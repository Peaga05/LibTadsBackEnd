using System.ComponentModel.DataAnnotations;

namespace LibTads.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}