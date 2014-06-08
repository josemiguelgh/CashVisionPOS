using System.Collections.Generic;
using System.Windows;
using CV.POS.Wpf.Common;

namespace CV.POS.Wpf.View
{

    public abstract class SingleInstanceWindow : Window
    {
        private static List<string> windowsCreated = new List<string>();

        private string windowName;
        private string creationStatus;

        public string CreationStatus
        {
            get { return creationStatus; }
        }

        protected SingleInstanceWindow()
        {
            windowName = GetType().Name;
            if (windowsCreated.Contains(windowName))
                creationStatus = Constants.ShouldBeClosed;
            else
            {
                creationStatus = Constants.IsFirstInstance;
                windowsCreated.Add(windowName);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (CreationStatus == Constants.IsFirstInstance)
                windowsCreated.Remove(windowName);
        }

        protected void NotifyStatusAndClose(string message)
        {
            MessageBox.Show(message);
            Close();
        }

        protected void NotifyStatus(string message)
        {
            MessageBox.Show(message);
        }
        //protected SingleInstanceWindow(Func<SingleInstanceWindow, string> classNameProvider)
        //this.windowName = classNameProvider(this);
        //protected static string FindClassName(SingleInstanceWindow @this)
        //{
        //    return @this.GetType().Name;
        //}

        
        
    }
}
