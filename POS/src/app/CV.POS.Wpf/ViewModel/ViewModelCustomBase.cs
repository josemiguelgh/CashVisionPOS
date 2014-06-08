using System;
using System.Linq.Expressions;
using CV.POS.Business.Helpers;
using GalaSoft.MvvmLight.Messaging;

namespace CV.POS.Wpf.ViewModel
{
    public class ViewModelCustomBase<TViewModel> : ViewModelBase
    {
        protected string BuildCompositeErrorMessage(string errorMessage, string stackTrace)
        {
            return string.Format("Error = {0} // StackTrace = {1}", errorMessage, stackTrace);
        }

        protected void PropagateException(string errorMessage, string token)
        {
            var operationResult = new OperationResult { Succeed = false, Message = errorMessage };
            Messenger.Default.Send(operationResult, token);
        }

        protected void PropagateMessage(string message, string token)
        {
            Messenger.Default.Send(message, token);
        }

        protected void PropagateMessage(SimpleSaleLineViewModel message, string token)
        {
            Messenger.Default.Send(message, token);
        }

        public void RaisePropertyChanged(Expression<Func<TViewModel, object>> propertyGetter)
        {
            RaisePropertyChanged(propertyGetter.GetPropertyNameString());
        }
        
    }

    public static class ExpressionExtensions
    {
        public static string GetPropertyNameString<TOwner, TProperty>(
                 this Expression<Func<TOwner, TProperty>> propertyGetter)
        {
            var expression =
              propertyGetter.Body as MemberExpression
                   ?? (MemberExpression)((UnaryExpression)propertyGetter.Body).Operand;
            return expression.Member.Name;
        }

    }
}
