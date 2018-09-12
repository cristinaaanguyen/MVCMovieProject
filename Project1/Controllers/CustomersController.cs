
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Project1.Models;
using Project1.ViewModels;

namespace Project1.Controllers
{
    public class CustomersController : Controller
    {

        private MyDBContext _context; //need the DB context to access database
        public CustomersController() {

            _context = new MyDBContext(); //intialize private variable; this DB context is a disposable opbject

        }

        //overrwrite dispose method from base controller class
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }



        //form for creating customer, need action that returns a view including a form
        public ActionResult New() {

            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new CustomerFormViewModel
            {
                MembershipTypes = membershipTypes,
                Customer = new Customer() //new customer will be initialized with default values and ID will be 0 until hidden field is rendered after submit
            };
            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//apply HttpPost to this action to make sure it can be called only using Http post and not http get
        //model behind our view is of type CustomerFormViewModel, we can use this type in the parameter and MVC framework will bind this model to the request data. but we can just make the parameter
        // of the type Customer b/c MVC framework smart enough to bind this obj to form data b/c all the keys in form data were prefixed w/ Customer
        public ActionResult Save(Customer customer) { //when a parameter is passed to an action, MVC framework validates that obj using data annotes applied to their properties
            //Customer as parameter, when .NET populates the customer obj using request data, it checks to see if this obj is valid based on data annotations applied on diff. properites of customer class
            //now we can use ModelState property to get access to validation data;

            if (!ModelState.IsValid) { //if not valid, then return the same view w the customer form otherwise add/update customer and redirect user to list of customers
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList() //this is required so we can repopulate the form with the input the user already filled
                };

                return View("CustomerForm", viewModel);
            }

            if (customer.Id == 0)  //means we have a new customer //relying on customer.Id, but this Id is not currently in our form; we only have input fields for name, DOB and etc. 
                _context.Customers.Add(customer);
            else //existing customer
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.Id = customer.Id;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.isSubscribedToNewsletter = customer.isSubscribedToNewsletter;
                

            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Customers"); //after saving, redirect user to list of customers

        }



        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);
            if (customer == null) return HttpNotFound();
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };

            return View("CustomerForm", viewModel); //specify view name otherwise MVC will look for a view called edit, but we want the view called new
    

        }

            public ViewResult Index()
        {
            //var customers = GetCustomers();


            //var c = new List<Customer> return this list in the view, since list is also IEnumerable type
            //{
            //    new Customer { Id = 1, Name = "John Smith" },
            //    new Customer { Id = 2, Name = "Mary Williams" }
            //};




            var customers = _context.Customers.Include(c => c.MembershipType).ToList(); //customers property is a DBset we defined in our DB context so we can get all customers in the DB
                                                                                        //load customers and their membership types together -> eager loading. b/c  EF only loads customer obj, not their related objs
                                                          //use Include method and pass the expression to determine target property, in this case MembershipType 
            return View(customers);
        }


        public ActionResult Details(int id) {
            if (id == null) {
                return new HttpNotFoundResult();
            }


            //var customers = GetCustomers();
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id); //executes query right away
            if (customer == null) {
                return new HttpNotFoundResult();
            }
            //foreach (var customer in customers) {
            //    if (customer.Id == id) {
            //        return View(customer);
            //    }
            //}

            return View(customer);



        }
      

        //private IEnumerable<Customer> GetCustomers() don't need this anymore because we will be getting customers from db. 
        //{
        //    return new List<Customer>
        //    {
        //        new Customer { Id = 1, Name = "John Smith" },
        //        new Customer { Id = 2, Name = "Mary Williams" }
        //    };
        //}
    }
}