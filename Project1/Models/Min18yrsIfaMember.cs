using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project1.Models
{
    public class Min18yrsIfaMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) //validation context helps w/ checking membership type
        {
            var customer = (Customer)validationContext.ObjectInstance; //gives us access to containing class, in this case customer. b/c this is an obj, we have to cast it to customer

            if (customer.MembershipTypeId == MembershipType.Unknown || customer.MembershipTypeId == MembershipType.PayAsYouGo) {
                return ValidationResult.Success;
            }


            //check to see if birthdate is null, return new validation result and specify error message. to indicate an error, instantiate new validation result
            if (customer.Birthdate == null) {
                return new ValidationResult("Please enter birthdate");
            }

            var age = DateTime.Today.Year - customer.Birthdate.Value.Year;
            return (age >= 18) ? ValidationResult.Success
                : new ValidationResult("Customer needs to be at least 18 y/o to go on membership");


        }

    }
}