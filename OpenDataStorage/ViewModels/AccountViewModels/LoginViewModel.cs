using OpenDataStorage.Resources;
using System.ComponentModel.DataAnnotations;

namespace OpenDataStorage.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(Lexicon))]
        [Display(ResourceType = typeof(Lexicon), Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(Lexicon))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Lexicon), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Lexicon), Name = "RememberMeQuestion")]
        public bool RememberMe { get; set; }
    }
}
