using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using newMoviesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace newMoviesApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _dbContext;

        public MoviesController(MovieContext dbContext)
        {
            _dbContext= dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> All()
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }

            return await _dbContext.Movies.ToListAsync();
        }

        [HttpGet("id")]
        public async Task<ActionResult<Movie>> Show(int id)
        {
            if (_dbContext.Movies!= null)
            {
                var movie = await _dbContext.Movies.FindAsync(id);

                if (movie != null)
                {
                    return movie;
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> Store(Movie movie)
        {
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Show), new { id = movie.Id }, movie);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            } 
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                } 
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Destroy(int id)
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }

            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }


        private bool MovieExists(int id)
        {
            return (_dbContext.Movies?.Any(movie => movie.Id == id)).GetValueOrDefault();
        }
    }
}
