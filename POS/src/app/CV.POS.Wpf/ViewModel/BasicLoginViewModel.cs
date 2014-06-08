using System;
using System.Windows.Input;
using CV.POS.Business;
using CV.POS.Business.Helpers;
using CV.POS.Wpf.Common;
using CV.POS.Wpf.Common.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CV.POS.Wpf.ViewModel
{
	public sealed class BasicLoginViewModel : ViewModelCustomBase<BasicLoginViewModel>
	{
		#region Fields and Properties

		private readonly ICommand loginCommand;
		private string username;
		private string password;

		private readonly IUserService userService;

		public string Username
		{
			get { return username; }
			set
			{
				if (username == value)
					return;
				username = value;
				RaisePropertyChanged(Username);
			}
		}
		public string Password
		{
			get { return password; }
			set
			{
				if (password == value)
					return;
				password = value;
				RaisePropertyChanged(Password);
			}
		}

		public ICommand LoginCommand
		{
			get { return loginCommand; }
		}

		#endregion

		public BasicLoginViewModel(IUserService userService)
		{
			loginCommand = new RelayCommand(AuthenticateUser);
			this.userService = userService;
			/*TODO: To be deleted, automatic login*/
			Username = "jgutierrez";
			Password = "jgutierrez";
		}

		#region Public Methods
		
		
		public void AuthenticateUser()
		{
			try
			{
				if (!ValidationHelper.IsNullOrWhitespace(Username) && !ValidationHelper.IsNullOrWhitespace(Password))
				{
					userService.User.Login = Username;
					userService.User.Password = Password;

					if (userService.Authenticate())
						SendSuccessfulAuthenticationResult();
					else
						SendFailedAuthenticationResults(Constants.Error2);
				}
				else
					SendFailedAuthenticationResults(Constants.Error1);
			}
			catch (Exception ex)
			{
				PropagateException(BuildCompositeErrorMessage(ex.Message, ex.StackTrace), Constants.Token3);
			}
		}

		#endregion

		#region Private Methods

		private void SendFailedAuthenticationResults(string error)
		{
			Username = string.Empty;
			Password = string.Empty;
			var operationResult = new OperationResult { Succeed = false, Message = error };
			Messenger.Default.Send(operationResult, Constants.Token1);
		}

		private void SendSuccessfulAuthenticationResult()
		{
			var operationResult = new OperationResult { Succeed = true, User = userService.User };
			Messenger.Default.Send(operationResult, Constants.Token2);
		}

		#endregion
	}
}
