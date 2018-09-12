using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project1.Models;

namespace Project1.ViewModels
{
    public class CustomerFormViewModel
    {
        public IEnumerable<MembershipType> MembershipTypes { get; set; }
        public Customer Customer { get; set; }
    }
}