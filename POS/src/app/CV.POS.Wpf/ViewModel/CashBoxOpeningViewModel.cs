using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using CV.POS.Business;
using CV.POS.Wpf.Common;
using GalaSoft.MvvmLight.Command;

namespace CV.POS.Wpf.ViewModel
{
    public sealed class CashBoxOpeningViewModel
        : ViewModelCustomBase<CashBoxOpeningViewModel>
    {

        #region Fields and Properties

        private readonly ICashBoxService cashBoxService;
        private readonly ICommand openCashboxCommand;
        
        private decimal cashBoxAmount;
        private string verifiedAmount;

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.Error6)]
        [RegularExpression(Constants.MoneyRegEx, ErrorMessage = Constants.Error7)]
        public string VerifiedAmount
        {
            get { return verifiedAmount; }
            set
            {
                if (verifiedAmount == value)
                    return;
                ValidateProperty("VerifiedAmount", value);
                verifiedAmount = value;
                RaisePropertyChanged("VerifiedAmount");
            }
        }

        public decimal CashBoxAmount
        {
            get { return cashBoxAmount; }
        }

        #endregion

        public ICommand OpenCashboxCommand
        {
            get { return openCashboxCommand; }
        }

        public CashBoxOpeningViewModel(ICashBoxService cashBoxService)
        {
            this.cashBoxService = cashBoxService;
            openCashboxCommand = new RelayCommand(OpenCashbox);

            cashBoxAmount = cashBoxService.GetAmountForCashBox();
        }

        private void OpenCashbox()
        {
            if (!ValidateObject())
                return;
            try
            {
                if (cashBoxService.IsOpened())
                    NotifyStatusAndClose(Constants.Error8);
                else
                {
                    var operationResult = cashBoxService.OpenCashBox(GlobalAppValues.UserId, GlobalAppValues.SessionId, Convert.ToDecimal(verifiedAmount));
                    if (operationResult.Succeed)
                        NotifyStatusAndClose(Constants.Msg1);
                    else
                        NotifyStatusAndClose(Constants.Error9);
                }
            }
            catch (Exception ex)
            {
                PropagateException(BuildCompositeErrorMessage(ex.Message, ex.StackTrace), Constants.Token6);
            }
        }

        private void NotifyStatusAndClose(string errorMessage)
        {
            PropagateMessage(errorMessage, Constants.Token6);
        }
    }
}