using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace WebAppParser.Helper
{
    public class Validator
    {
        
        public static bool ValidateUrl(string url)
        {

            if (url.Contains("olx"))
                return true;
            else
                return false;
        }
    }
}
