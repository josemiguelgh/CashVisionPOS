using CV.POS.Entities;

namespace CV.POS.Business.Interfaces
{

    public interface ISessionRepository : IBaseRepository<Session>
    {
        void CloseSessionsByUserId(short userId);
    }
}
