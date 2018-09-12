using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project1.Models;
using System.Data.Entity;
using Project1.ViewModels;
using System.Data.Entity.Validation;
using Project1.Migrations;

namespace Project1.Controllers
{
    public class MoviesController : Controller
    {


        private MyDBContext _context;

        public MoviesController()
        {

            _context = new MyDBContext(); //intialize private variable; this DB context is a disposable opbject

        }

        //overrwrite dispose method from base controller class
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer> {
                //new Customer { Name = "Customer 1"},
                //new Customer { Name = "Customer 2"}
            };



            return View(movie);
            //return Content("Hello World!");
            //return RedirectToAction("Index", "Home", new{ page = 1, sortBy = "name"}); redirect user from one page to another, somtimes must pass argument to action 
        }


        public ActionResult MovieForm()
        {
            //need to get list of genres from db; can't just pass the list b/c we need the move info too, so we need to create view model
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel { Genres = genres
            //Movie = new Movie()
            };
            return View("MovieForm",viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie) //form posted to this Save action
        {
            //if (!ModelState.IsValid)
            //{

            //    var viewModel = new MovieFormViewModel
            //    {
            //        Movie = movie,
            //        //Id = movie.Id,
            //        //Name = movie.Name,
            //        //ReleaseDate = movie.ReleaseDate,
            //        //NumberInStock = movie.NumberInStock,
            //        //GenreId = movie.GenreId,
            //        //Title = "New Movie",
            //        Genres = _context.Genres.ToList(),
            //        Title = "New Movie"

            //    };

            //    return View("MovieForm", viewModel);


            //}



            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                var genre = _context.Genres.SingleOrDefault(m => m.Id == movie.GenreId);
                movie.Genre = genre;
                _context.Movies.Add(movie);
            }
            else //existing movie
            {
                var movieInDb = _context.Movies.Single(m => m.Id == movie.Id);
                movieInDb.Name = movie.Name;
                movieInDb.Id = movie.Id;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.Genre = movie.Genre;
                movieInDb.NumberInStock = movie.NumberInStock;
                movieInDb.DateAdded = movie.DateAdded;

            }

            try
            {
   
                _context.SaveChanges();
            }//have to manually add try catch block to find the validation error and use shift and f5 to stop debugger, f5 to start debugger
            //had a validation error b/c in the movie class, we had the genre entity as a required attrib. but when we get the movie obj from the form, its genre attrib is null b/c we post the genre ID which is the key for drop downn list
            catch (DbEntityValidationException e) {
                Console.WriteLine(e);
            }
            return RedirectToAction("Index", "Movies"); //after saving, redirect user to list of movie

        }


            public ActionResult Redirect() {

            return RedirectToAction("MovieForm", "Movies");
        }



        public ActionResult Edit(int id) {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);
            var viewModel = new MovieFormViewModel
            {
                Movie = movie,
            //    Id = movie.Id,
            //Name = movie.Name,
            //ReleaseDate = movie.ReleaseDate,
            //NumberInStock = movie.NumberInStock,
            //GenreId = movie.GenreId,
            Title = "Edit Movie",
            Genres = _context.Genres.ToList(),


            };
            return View("MovieForm", viewModel);
        }

        public ActionResult DeleteMovie(int id) {
            var movie = _context.Movies.Find(id);
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }



        //movies
        //public ActionResult Index(int? pageIndex, string sortBy) {
        //    if (!pageIndex.HasValue) {
        //        pageIndex = 1;
        //    }
        //    if (String.IsNullOrWhiteSpace(sortBy)) {
        //        sortBy = "Name";
        //    }

        //    return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));

        //}


        public ActionResult Index()
        {
            var movies = _context.Movies.Include(m => m.Genre);

            return View(movies);
        }

        private IEnumerable<Movie> GetMovies()
        {
            return new List<Movie>
            {
                new Movie { Id = 1, Name = "Wall-e" },
                new Movie { Id = 2, Name = "Crazy Rich Asians" }
            };
        }


        [Route("movies/released/{year}/{month:regex(\\d{2}):range(1, 12)}")] //set 2 different attributes constraints for month parameter
        public ActionResult ByReleaseDate(int year, int month) {
            return Content(year + "/" + month);
        }


        public ActionResult MovieDetails(int id)
        {

            var movie = _context.Movies.Include(m  => m.Genre).SingleOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return new HttpNotFoundResult();

            }

            return View(movie);



        }



    }
}