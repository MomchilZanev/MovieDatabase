using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models.InputModels.User
{
    public class PromoteUserInputModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string Name { get; set; }
    }
}
