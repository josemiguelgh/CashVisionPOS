using System;
using System.Linq;
using System.Windows;
using CV.POS.Business;
using CV.POS.Business.Helpers;
using CV.POS.Entities;
using CV.POS.Wpf.Common;
using CV.POS.Wpf.View.Login;
using CV.POS.Wpf.View.Main;
using GalaSoft.MvvmLight.Messaging;

namespace CV.POS.Wpf.ViewModel
{
    public sealed class MainViewModel : ViewModelCustomBase<MainViewModel>
    {

        private readonly IUserService userService;
        private readonly ICashBoxService cashBoxService;

        private object currentView;

        public object CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }

        public MainViewModel(IUserService userService, ICashBoxService cashboxService)
        {
            this.userService = userService;
            this.cashBoxService = cashboxService;

            CurrentView = new MainLogin();
            Messenger.Default.Register<OperationResult>(this,
                Constants.Token2, EnterPOS);
        }

        private void EnterPOS(OperationResult operationResult)
        {
            try
            {
                if (!operationResult.Succeed) 
                    return;
                if (cashBoxService.IsOpenedForOtherUsers(operationResult.User.UserId))
                {
                    PropagateMessage(Constants.Error5, Constants.Token5);
                    return;
                }
                userService.CloseOpenedSessions(operationResult.User.UserId);
                    
                var currentSession = userService.CreateSession(operationResult.User.UserId, operationResult.User.DefaultPremise);
                SaveSessionInfoInMemory(currentSession, operationResult.User);
                RedirectToMenuPage();
            }
            catch (Exception ex)
            {
                PropagateException(BuildCompositeErrorMessage(ex.Message, ex.StackTrace), Constants.Token3);
            }
        }

        private void SaveSessionInfoInMemory(Session currentSession, User currentUser)
        {
            Application.Current.Properties["UserId"] = currentSession.UserId;
            Application.Current.Properties["FirstName"] = currentUser.Employee.Person.FirstName;
            Application.Current.Properties["LastName"] = currentUser.Employee.Person.LastName;
            Application.Current.Properties["Login"] = currentUser.Login;
            Application.Current.Properties["RoleId"] = currentUser.SystemRole.Single().RoleId;
            Application.Current.Properties["RoleName"] = currentUser.SystemRole.Single().RoleName;
            Application.Current.Properties["SessionId"] = currentSession.SessionId;
            Application.Current.Properties["PremiseId"] = currentSession.PremiseId;
        }

        private void RedirectToMenuPage()
        {
            CurrentView = new MainMenu();
        }
    }
}