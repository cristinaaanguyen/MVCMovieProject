using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Project1.Models;

namespace Project1.ViewModels
{
    public class MovieFormViewModel
    {
        public IEnumerable<Genre> Genres { get; set; }
        public Movie Movie { get; set; }
        public string Title { get; set; }
        //[Required] //make all attributes nullable 
        //public int? Id { get; set; }
        //[Required]
        //[StringLength(255)]
        //public string Name { get; set; }

        //[Required]
        //public byte? GenreId { get; set; }

        //[Required]
        //public DateTime? ReleaseDate { get; set; }

        //[Required]
        //public byte? NumberInStock { get; set; }
        //public string Title { get; set; }


        //define constructor that takes a movie obj
        //public MovieFormViewModel(Movie movie)
        //{
        //    Id = movie.Id;
        //    Name = movie.Name;
        //    ReleaseDate = movie.ReleaseDate;
        //    NumberInStock = movie.NumberInStock;
        //    GenreId = movie.GenreId;
        //    Title = "Edit Movie";


        //}

        ////default constructor for creating a new movie
        //public MovieFormViewModel()
        //{
        //    //make sure in our form, the hidden field is populated
        //    Id = 0;
        //}

    }




}