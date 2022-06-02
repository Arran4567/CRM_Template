using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bicks.Areas.Settings.ViewModels
{
    public class EditUserAccountViewModel
    {
        public EditUserAccountViewModel()
        {
            UserRoles = new List<string>();
        }

        [Required]
        [Display(Name = "ID")]
        public string ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Account Disabled")]
        public bool AccountDisabled { get; set; }

        [Display(Name = "Roles")]
        public IList<string> UserRoles { get; set; }
    }
}
