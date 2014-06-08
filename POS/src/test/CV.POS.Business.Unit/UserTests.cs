using System.Linq;
using CV.POS.Business.Interfaces;
using CV.POS.Entities;
using Moq;
using NUnit.Framework;

namespace CV.POS.Business.Unit
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Authenticate_WithoutPassword_ReturnsFailedState()
        {
            //Arrange
            var queryableExpectedResult = Enumerable.Empty<User>().AsQueryable();
            var userRepositoryStub = new Mock<IUserRepository>();
            userRepositoryStub.Setup(x => x.GetByUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(queryableExpectedResult);
            var uowStub = new Mock<IUow>();
            uowStub.SetupGet(x => x.UserRepository).Returns(userRepositoryStub.Object);

            var userService = new UserService(uowStub.Object) { User = new User{Login = "admin2", Password = ""}};
            //Act, Assert	
            Assert.IsFalse(userService.Authenticate());
        }
        
        [Test]
        public void CreateSession_StandardCall_ReturnsValidSession()
        {
            //Arrange
            short userId = 1;
            byte premiseId = 1;

            var sessionRepositoryStub = new Mock<ISessionRepository>();

            var uowStub = new Mock<IUow>();
            uowStub.SetupGet(x => x.SessionRepository).Returns(sessionRepositoryStub.Object);

            var userService = new UserService(uowStub.Object) { User = new User { UserId = 1 } };

            //Act
            var currentSession = userService.CreateSession(userId,premiseId);

            //Assert
            Assert.IsNotNull(currentSession.StartDate);
            Assert.IsNull(currentSession.EndDate);
        }
    }
}
