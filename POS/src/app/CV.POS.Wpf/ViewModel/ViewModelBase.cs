using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.Messaging;

namespace CV.POS.Wpf.ViewModel
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase, INotifyDataErrorInfo
    {
        private readonly IDictionary<string, IList<string>> errors = new Dictionary<string, IList<string>>();

        protected ViewModelBase()
        {
        }

        protected ViewModelBase(IMessenger messenger)
            : base(messenger)
        {
        }

        #region INotifyDataErrorInfo Members

        public IEnumerable GetErrors(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                IList<string> propertyErrors = errors[propertyName];
                foreach (string propertyError in propertyErrors)
                {
                    yield return propertyError;
                }
            }
            yield break;
        }

        public bool HasErrors
        {
            get { return errors.Count > 0; }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion

        protected void NotifyErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void ValidateProperty(string propertyName, object value)
        {
            ViewModelBase objectToValidate = this;
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateProperty(
                value,
                new ValidationContext(objectToValidate, null, null)
                {
                    MemberName = propertyName
                },
                results);

            if (isValid)
                RemoveErrorsForProperty(propertyName);
            else
                AddErrorsForProperty(propertyName, results);

            NotifyErrorsChanged(propertyName);
        }

        private void AddErrorsForProperty(string propertyName, IEnumerable<ValidationResult> validationResults)
        {
            RemoveErrorsForProperty(propertyName);
            errors.Add(propertyName, validationResults.Select(vr => vr.ErrorMessage).ToList());
        }

        private void RemoveErrorsForProperty(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
                errors.Remove(propertyName);
        }

        public bool ValidateObject()
        {
            ViewModelBase objectToValidate = this;
            errors.Clear();
            Type objectType = objectToValidate.GetType();
            PropertyInfo[] properties = objectType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.GetCustomAttributes(typeof(ValidationAttribute), true).Any())
                {
                    object value = property.GetValue(objectToValidate, null);
                    ValidateProperty(property.Name, value);
                }
            }

            return !HasErrors;
        }
    }
}
