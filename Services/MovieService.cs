using MinimalJwtProject.Models;
using MinimalJwtProject.Repositories;

namespace MinimalJwtProject.Services
{
    public class MovieService : IMovieService
    {
        public Movie Create(Movie movie)
        {
            movie.id = MovieRepository.Movies.Count + 1;
            MovieRepository.Movies.Add(movie);
            return movie;

        }
        public Movie Get(int id)
        {
            var movie = MovieRepository.Movies.FirstOrDefault(x => x.id == id);
            if (movie == null) return null;
            return movie;

        }

        public List<Movie> List()
        {
            var movies = MovieRepository.Movies;

            return movies;


        }

        public Movie Update(Movie putMovie)
        {

            var oldMovie = MovieRepository.Movies.FirstOrDefault(o=> o.id == putMovie.id);
            if (oldMovie is null) return null;

            oldMovie.Title = putMovie.Title;
            oldMovie.Description=putMovie.Description;
            oldMovie.Rating=putMovie.Rating;

            return putMovie;



        }

        public bool Delete(int id)
        {

            var oldMovie = MovieRepository.Movies.FirstOrDefault(o => o.id == id);

            if (oldMovie is null) return false;
            
            MovieRepository.Movies.Remove(oldMovie);

            return true;


        }
    }
}
