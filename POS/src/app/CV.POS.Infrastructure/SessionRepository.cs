using System;
using CV.POS.Business.Interfaces;
using CV.POS.Data;
using CV.POS.Entities;

namespace CV.POS.Infrastructure
{
    public class SessionRepository 
        : BaseRepository<Session>, ISessionRepository
    {
        public SessionRepository(PosDbContext dbContext) 
            : base(dbContext) { }

        public void CloseSessionsByUserId(short userId)
        {
            Db.Database.ExecuteSqlCommand(
                "UPDATE Session SET EndDate = {0} " +
                "WHERE UserId = {1} " +
                "and EndDate IS NULL",
                DateTime.Now, userId);
        }
    }
}
