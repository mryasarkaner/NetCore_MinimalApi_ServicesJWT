using MinimalJwtProject.Models;

namespace MinimalJwtProject.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);



    }
}
