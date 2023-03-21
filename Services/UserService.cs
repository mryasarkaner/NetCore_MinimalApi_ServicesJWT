using MinimalJwtProject.Models;
using MinimalJwtProject.Repositories;

namespace MinimalJwtProject.Services
{
    public class UserService : IUserService
    {
        public User Get(UserLogin userLogin)
        {

            //tr- Burda servisimiz UserLogin interfaceden gelen name ve pass bilgileri eşitse user geriye döndürebilirsin işlemini gerçekleştiriyor.7

            //ENG- Here, our service performs the operation that you can return the user if the name and pass information from the UserLogin interface are equal.

            User user  = UserRepository.Users.FirstOrDefault(o => 
            o.Username.Equals(userLogin.Username, StringComparison.OrdinalIgnoreCase)&& 
            o.Password.Equals(userLogin.Password )
            );
            return user;

        }
    }
}
