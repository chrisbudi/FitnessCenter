using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataObjects.Entities;


namespace ViewModel.Master
{
    public class CreateVmBackOffice
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public tPerson Person { get; set; }
        public tUserBackOffice UserBackOffice { get; set; }
        public IEnumerable<strLocBO> StrLocBo { get; set; }
    }

    public class EditVmBackOffice
    {
        public tPerson Person { get; set; }
        public tUserBackOffice UserBackOffice { get; set; }
        public IEnumerable<strLocBO> StrLocBo { get; set; }
    }
}
