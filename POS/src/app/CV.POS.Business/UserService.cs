using System;
using System.Linq;
using CV.POS.Business.Interfaces;
using CV.POS.Entities;


namespace CV.POS.Business
{
    public interface IUserService
    {
        User User { get; set; }
        bool Authenticate();
        void CloseOpenedSessions(short userId);
        Session CreateSession(short userId, byte defaultPremiseId);
        void CashBoxIsOpened();
    }

    public class UserService : IUserService
    {
        public User User { get; set; }
        public IUow Uow { get; set; }

        public UserService(IUow uow)
        {
            Uow = uow;
            User = new User();
        }

        public bool Authenticate()
        {
            var userInDb = Uow.UserRepository
                .GetByUsernameAndPassword(User.Login, User.Password)
                .SingleOrDefault();

            if (userInDb == null)
                return false;
            else
            {
                User = userInDb;
                return true;
            }
        }

        public void CloseOpenedSessions(short userId)
        {
            Uow.SessionRepository.CloseSessionsByUserId(userId);
        }

        public Session CreateSession(short userId, byte defaultPremiseId)
        {
            var session = new Session
            {
                UserId = userId, 
                StartDate = DateTime.Now, 
                PremiseId = defaultPremiseId
            };
            Uow.SessionRepository.Insert(session);
            Uow.Commit();
            return session;
        }

        public void CashBoxIsOpened()
        {
            throw new NotImplementedException();
        }
    }
}
