using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppParser.Models;

namespace WebAppParser.Controllers
{
    public class DictionaryPrint
    {

        public static void Print(Dictionary<string, string> property)
        {

            foreach (KeyValuePair<string, string> item in property)
            {
                Console.WriteLine(item.Key + item.Value);

            }
        }

}
}
