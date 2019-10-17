using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppParser.Models;


namespace WebAppParser.Controllers
{
    public class ViewDataSort 
    {
        public static string Sort(string key)
        {
            return Property.propertyD[key];
        }


    }
}
