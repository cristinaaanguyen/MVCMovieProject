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
    public class MoviesController : ApiController
    {
        //need _context for db
        private MyDBContext _context;
        public MoviesController() {

            _context = new MyDBContext();

        }


        //getallmovies 

        //GET /api/movies

        public IEnumerable<MovieDto> GetMovies() {

            return _context.Movies.ToList().Select(Mapper.Map<Movie, MovieDto>);


        }

        //getmovie with id
        // GET /api/movie/1
        public IHttpActionResult GetMovie(int id) {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();
            if (movie == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Ok(Mapper.Map<Movie, MovieDto>(movie));

        }


        //creating
        //POST /api/movies
        [HttpPost]
        public IHttpActionResult CreateMovie(MovieDto movieDto) {
            if (!ModelState.IsValid) //validates input, had exception earlier bc Genre was required so had to remove that annotation from domain model 
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                return BadRequest();
            }

            movieDto.DateAdded = DateTime.Now;
            var genre = _context.Genres.SingleOrDefault(g => movieDto.GenreId == g.Id);
            movieDto.Genre = genre;
            var movie = Mapper.Map<MovieDto, Movie>(movieDto);



            _context.Movies.Add(movie);

            _context.SaveChanges();
            movieDto.Id = movie.Id;


            return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieDto);
        }


        //updating
        //PUT /api/movies/1
        [HttpPut]
        public void UpdateMovie(int id, MovieDto movieDto) {
            if (!ModelState.IsValid) //validates input
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var movieInDb = _context.Movies.SingleOrDefault(m => m.Id == id);
            if (movieInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            //movieInDb.Id = movie.Id; cant include this line b/c exception will raise since ID cant be changed 
            //movieInDb.Name = movie.Name;
            //movieInDb.NumberInStock = movie.NumberInStock;
            //movieInDb.GenreId = movie.GenreId;
            //movieInDb.Genre = movie.Genre;
            //movieInDb.ReleaseDate = movie.ReleaseDate;
            //movieInDb.DateAdded = movie.DateAdded;

            Mapper.Map<MovieDto, Movie>(movieDto, movieInDb);

            _context.SaveChanges();

        }


        //deleting /api/movies/1
        [HttpDelete]
        public void DeleteMovie(int id) {
            var movieInDb = _context.Movies.SingleOrDefault(m => m.Id == id);
            if (movieInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            _context.Movies.Remove(movieInDb);
            _context.SaveChanges();

        }
    
    }
}
