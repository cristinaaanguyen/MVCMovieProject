using Project1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project1.Dtos
{
    public class CustomerDto //need Dtos, plain data structure used to transfer data from the client to server and vice versa, will reduce chances of our project breaking
    {
        public int Id { get; set; }
        //data annotations are useful bc ASP net uses them for both server and client side validation
        [Required(ErrorMessage = "Please enter customer's name.")]
        [StringLength(255)] //data annotations to overrwrite default conventions; every time we make changes to domain models, must add migration in PM
        public string Name { get; set; }
        public bool isSubscribedToNewsletter { get; set; }
        //had to remove property MembershipType b/c MembershipType is a domain class and its creating a dependency from our DTO to the domain model , we should decouple it from its
        public byte MembershipTypeId { get; set; } //EF treats this as foreign key
        //[Min18yrsIfaMember] have to comment out b/c in the validation rule file, we're casting the obj instance to Customer
        public DateTime? Birthdate { get; set; }

    }
}