using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactAPI.Models
{
    public class ContactViewModel
    {
        [DisplayName("Id")]
        public int ContactId { get; set; }

        
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First Name should be letters only.")]
        [DisplayName("First Name")]
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last Name should be letters only.")]
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress(ErrorMessage = "Email address must be in a@b.com format.")]
        [MaxLength(100)]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        [Required(ErrorMessage = "You must provide a phone number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone number must be numeric and 10 digits long.")]
        public string PhoneNo { get; set; }

        public bool? Status { get; set; }
    }
}