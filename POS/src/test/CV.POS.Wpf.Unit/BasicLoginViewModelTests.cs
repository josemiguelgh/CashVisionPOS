using CV.POS.Business;
using CV.POS.Business.Helpers;
using CV.POS.Entities;
using CV.POS.Wpf.Common;
using CV.POS.Wpf.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;

namespace CV.POS.Wpf.Unit
{
    [TestFixture]
    public class BasicLoginViewModelTests
    {
        private OperationResult operationResult;
        
        [TestCase("admin2","",Result = false)]
        [TestCase("admin2", "admin2", Result = true)]//valid arguments
        public bool AuthenticateUser_ArgumentsFromTestCase_AuthenticationSameResult(string username, string password)
        {
            Messenger.Default.Register<OperationResult>(this,
                Constants.Token1, UpdateView);
            Messenger.Default.Register<OperationResult>(this,
                Constants.Token2, UpdateView);

            var userServiceStub = new Mock<IUserService>();
            userServiceStub.Setup(x => x.Authenticate()).Returns(true);
            userServiceStub.SetupGet(x => x.User).Returns(new User());

            //Arrange
            var basicLoginViewModel = new BasicLoginViewModel(userServiceStub.Object)
            {
                Username = username,
                Password = password
            };

            //Act
            basicLoginViewModel.AuthenticateUser();

            //Assert	
            return operationResult.Succeed;
        }

        private void UpdateView(OperationResult operationResultParam)
        {
            operationResult = operationResultParam;
        }
    }
}
