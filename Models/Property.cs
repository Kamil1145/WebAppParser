using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebAppParser.Models
{
    public class Property
    {
        public static Dictionary<string, string> propertyD = new Dictionary<string, string>();

        public string Id { get; set; }
        public string Url { get; set; }
        public string Title{ get; set; }
        public string Content { get; set; }
        public string Prize { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string Rooms { get; set; }
        public string Floor { get; set; }
        public string Furnitured { get; set; }
        public string Development { get; set; }
        public string PrizePerM { get; set; }
        public string OfferFrom { get; set; }
        public string Trade { get; set; }
        public string Fee { get; set; }
        public string PhoneNumber { get; set; }
        public string Parking { get; set; }
        public string Bathrooms { get; set; }
    
    }
}
