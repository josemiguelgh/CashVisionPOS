using System.Linq;
using CV.POS.Entities;

namespace CV.POS.Business.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetByUsernameAndPassword(string username, string password);
    }
}
