using Microsoft.EntityFrameworkCore;
using Movies.API.Models;

namespace Movies.API.Data
{
    public class MoviesContext : DbContext
    {
        public MoviesContext(DbContextOptions<MoviesContext> options) : base(options) { }
        public DbSet<Movie> Movie { get; set; }
    }
}
