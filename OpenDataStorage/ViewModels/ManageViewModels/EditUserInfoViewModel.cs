using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using OpenDataStorage.Resources;

namespace OpenDataStorage.ViewModels.ManageViewModels
{
    public class EditUserInfoViewModel
    {
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        [EmailAddress]
        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(Lexicon))]
        [Display(ResourceType = typeof(Lexicon), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(Lexicon))]
        [Display(ResourceType = typeof(Lexicon), Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredErrorMessage", ErrorMessageResourceType = typeof(Lexicon))]
        [Display(ResourceType = typeof(Lexicon), Name = "LastName")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(Lexicon), Name = "Language")]
        public string Language { get; set; }
    }
}