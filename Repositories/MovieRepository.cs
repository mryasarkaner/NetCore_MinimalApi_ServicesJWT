using MinimalJwtProject.Models;

namespace MinimalJwtProject.Repositories
{
    public class MovieRepository
    {

        public static List<Movie> Movies = new()
        {
            new(){id=1, Title="Gladiator",
                Description="North Roma, Ceaser Marcus oarolius power team General Maximus,life road example.",
            Rating =8.7 },

            new(){id=2, Title="Rocky",
                Description="A never give up Boxer.",
            Rating =8.5 },

            new(){id=3, Title="Elysium",
                Description="3050 years next, New world policy and one man change a world policy movie action.",
            Rating =7.7 },

            new(){id=4, Title="Fast Furios",
                Description="Fast cars team, Illegaly organized protected events.",
            Rating =8.3 },

            new(){id=5, Title="Troy",
                Description="3050 years before, Warrios a Ashil and very sized soldier Greece attack but against Assasian Hector ",
            Rating =7.5 },


        };
    }
}
