using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project1.Models
{
    public class Customer
    {
        public int Id { get; set; }
        //data annotations are useful bc ASP net uses them for both server and client side validation
        [Required(ErrorMessage ="Please enter customer's name.")]
        [StringLength(255)] //data annotations to overrwrite default conventions; every time we make changes to domain models, must add migration in PM
        public string Name { get; set; }
        public bool isSubscribedToNewsletter { get; set; }
        public MembershipType MembershipType { get; set; } //navigation property, b/c we can navi from one type to another (navi from customer to its membership type)
        public byte MembershipTypeId { get; set; } //EF treats this as foreign key
        [Min18yrsIfaMember]
        [Display(Name = "Date of Birth")]
        public DateTime? Birthdate { get; set; }

    }
}