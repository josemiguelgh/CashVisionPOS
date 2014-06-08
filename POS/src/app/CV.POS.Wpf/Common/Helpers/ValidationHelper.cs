using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV.POS.Wpf.Common.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsNullOrWhitespace(string s)
        {
            return String.IsNullOrEmpty(s) || s.Trim().Length == 0;
        }
    }
}
