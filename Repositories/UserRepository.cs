using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MinimalJwtProject.Models;

namespace MinimalJwtProject.Repositories
{
    public class UserRepository
    {
        //[Authorize(Roles="Administrator")]
        public static List<User> Users = new()
        {
            new()
            {
                Username = "big_admin", EmailAddress="bigadmin@gmail.com", Password="Pass_0rdu", 
                GivenName="Dany", Surname="lovato", Role="Administrator"
            },
             new()
            {
                Username = "small_user", EmailAddress="smalluser@gmail.com", Password="User_0rdu",
                GivenName="Steve", Surname="song", Role="Standart"
            },

        };
    }
}
