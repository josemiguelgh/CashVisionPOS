using System.Linq;
using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly PosDbContext db;

        public UserRepository(PosDbContext dbContext)
        {
            db = dbContext;
        }

        public IQueryable<User> GetByUsernameAndPassword(string username, string password)
        {
            return db.User.Where(x => x.Login == username
                                    && x.Password == password);
        }
    }
}
