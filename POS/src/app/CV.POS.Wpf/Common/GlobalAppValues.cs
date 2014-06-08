using System;
using System.Windows;

namespace CV.POS.Wpf.Common
{
    public static class GlobalAppValues
    {
        public static short UserId
        {
            get { return Convert.ToInt16(Application.Current.Properties["UserId"]); }
        }

        public static int SessionId
        {
            get { return Convert.ToInt32(Application.Current.Properties["SessionId"]); }
        }

        public static byte PremiseId
        {
            get { return Convert.ToByte(Application.Current.Properties["PremiseId"]); }
        }
    }
}
