using AutoMapper;
using Project1.Dtos;
using Project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project1.Controllers.api
{
    public class CustomersController : ApiController
    {

        private MyDBContext _context;
        public CustomersController(){
            _context = new MyDBContext();
        }

        //returning a list of objs so by convention this action will respond to GET /api/customers
        public IEnumerable<CustomerDto> GetCustomers() {

            return _context.Customers.ToList().Select(Mapper.Map<Customer, CustomerDto>); //don't inclue () at end of Mapper.Map because we're not calling the function, we just want to delegate a reference to this method 

        }

        // GET /api/customer/1
        public IHttpActionResult GetCustomer( int id ) {

            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null) return NotFound();
            if (customer == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            //b/c we're returning one customer obj, can't use Select method like up there so we use Mapper.Map and pass customer obj as a argument to method
            return Ok(Mapper.Map<Customer, CustomerDto>(customer));

        }


        //POST /api/customers - use post when creating new customer
        [HttpPost] //when creating a resource; will only be called when an Http Post request is made
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        { //anywwhere we return a customer obj, must map to DTO first and in methods where we modify customer,
          //must map the customer DTO properties back to customer obj 

            //map this dto back to domain obj
            if (!ModelState.IsValid)
            {

                return BadRequest(); //method returns badrequest result which is a class that implements ihttpactionrestult
            }


            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerDto.Id = customer.Id;

            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerDto); //second argumenent is actual obj that was created which is customerDto
        }


        //PUT /api/customers/1 --- parameter id will come from URL and Customer will come from request body
        [HttpPut]
        public void UpdateCustomer(int id, CustomerDto customerDto) {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var customerInDb = _context.Customers.SingleOrDefault( c => c.Id == id);
            if (customerInDb == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);

            }


            Mapper.Map<CustomerDto, Customer>(customerDto, customerInDb); //pass two args: source aka customerDto and target obj which is customerinDb, in previous ex, we didn't pass a second arg
                                                                           //so it created a new obj and returned it but here we want our DB to track changes in our _context
            //customerInDb.Name = customerDto.Name;
            //customerInDb.Birthdate = customerDto.Birthdate;
            //customerInDb.isSubscribedToNewsletter = customerDto.isSubscribedToNewsletter;
            //customerInDb.MembershipTypeId = customerDto.MembershipTypeId;

            _context.SaveChanges();


        }


        [HttpDelete]// /api/customers/1
    
        public void DeleteCustoner(int id) {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customerInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);

            }

            _context.Customers.Remove(customerInDb);

            _context.SaveChanges();

        }

    }
}
