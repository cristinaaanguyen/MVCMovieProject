using AutoMapper;
using Project1.Dtos;
using Project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project1.App_Start
{
    public class MappingProfile : Profile //derive class from Profile
    {
        public MappingProfile() {

            Mapper.CreateMap<Customer, CustomerDto>(); //takes two parameters source and target, so we want to map a customer to a customer DTO
            Mapper.CreateMap<CustomerDto, Customer>().ForMember(c => c.Id, opt => opt.Ignore()); //call this when we're making modifications and updates to a customer, automapper uses reflection to scan the types and find the properties and map them based on names
            //once done making mapping profile, need to load this when application is started and we do that in global.asax 
            Mapper.CreateMap<Movie, MovieDto>();
            
            Mapper.CreateMap<MovieDto, Movie>().ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}