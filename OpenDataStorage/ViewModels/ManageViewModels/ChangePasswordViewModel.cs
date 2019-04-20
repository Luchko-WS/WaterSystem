using OpenDataStorage.Resources;
using System.ComponentModel.DataAnnotations;

namespace OpenDataStorage.ViewModels.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(Lexicon))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Lexicon), Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(Lexicon))]
        [StringLength(100, ErrorMessageResourceName = "PasswordLengthErrorMessage", ErrorMessageResourceType = typeof(Lexicon), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Lexicon), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Lexicon), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordCompareErrorMessage", ErrorMessageResourceType = typeof(Lexicon))]
        public string ConfirmPassword { get; set; }
    }
}