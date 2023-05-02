using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    public class MyValidation
    {
        static public bool Validated(string text)
        {
            Regex regex = new Regex(@"[\d]+");
            return (regex.IsMatch(text));            
        }
    }
}
