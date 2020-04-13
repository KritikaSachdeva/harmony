using AutoMapper;
using harmony.Dtos;
using harmony.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace harmony.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private MyDBContext _context;

        public MoviesController()
        {
            _context = new MyDBContext();
        }

        //GET /api/movies
        [HttpGet]
        public IHttpActionResult GetMovies()
        {
            var movieDtos = _context.Movies.ToList().Select(Mapper.Map<Movie, MoviesDto>);
           
            return Ok(movieDtos);
        }

        //GET /api/movies/id
        [HttpGet]
        public IHttpActionResult GetMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return Ok(Mapper.Map<Movie, MoviesDto>(movie));
        }

        //POST /api/movies
        [HttpPost]
        public IHttpActionResult CreateMovie(MoviesDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = Mapper.Map<MoviesDto, Movie>(movieDto);

            _context.Movies.Add(movie);
            _context.SaveChanges();

            movieDto.Id = movie.Id;

            return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieDto);
        }

        //PUT /api/movies/id
        [HttpPut]
        public IHttpActionResult UpdateMovie(int id, MoviesDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var movieInDb = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movieInDb == null)
                return NotFound();
            Mapper.Map(movieDto, movieInDb);

            _context.SaveChanges();
            return Ok();
        }
        
        //DELETE /api/movies/id
        [HttpDelete]
        public IHttpActionResult DeleteMovie(int id)
        {
            var movieInDb = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movieInDb == null)
                return NotFound();

            _context.Movies.Remove(movieInDb);
            _context.SaveChanges();
            return Ok();
        }
    }
}