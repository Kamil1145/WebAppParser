using System.Collections.Generic;
using System.Net;
using System.Web;
using HtmlAgilityPack;
using WebAppParser.Models;
using System.Drawing;
using WebAppParser.Helper;

namespace WebAppParser.Controllers
{
    public class OtoDomParser
    {
        public Dictionary<string, string> Scrape(string url)
        {
            Property property = new Property();

            List<string> headersList = new List<string>();
            List<string> contentsList = new List<string>();
            List<string> addressesList = new List<string>();
            List<string> pricesList = new List<string>();
            List<string> contentList = new List<string>();
            List<string> additionalContentList = new List<string>();
            List<string> imgsList = new List<string>();
            

            var web = new HtmlWeb();
            web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36";
            var doc = web.Load(url);


            string header, text, loc, price, table;
            string path = @"C:\Users\Kamil\source\repos\WebAppParser\Content\images\";

            var Title = doc.DocumentNode.SelectNodes("//*[@class = 'css-d2oo9m']");
            var Content = doc.DocumentNode.SelectNodes("//*[@class='section-description']");
            var Address = doc.DocumentNode.SelectNodes("//*[@class='css-d2oo9m']");
            var ContentArray = doc.DocumentNode.SelectNodes("//*[@class='css-1ci0qpi']");
            var Price = doc.DocumentNode.SelectNodes("//*[@class='css-s3teq']");


            //zdjecia
            var Photos = doc.DocumentNode.SelectNodes("//*[@class='offerdescription clr']");
            Utils.FilesDeleter(path);


            foreach (var title in Title)
            {
                header = HttpUtility.HtmlDecode(title.SelectSingleNode(".//h1").InnerText);
                headersList.Add(header);
            }

            foreach (var content in Content)
            {
                text = HttpUtility.HtmlDecode(content.SelectSingleNode(".//div").InnerText);
                contentsList.Add(text);
            }

            foreach (var address in Address)
            {
                loc = HttpUtility.HtmlDecode(address.SelectSingleNode(".//a").InnerText);
                addressesList.Add(loc);
            }

            foreach (var prize in Price)
            {
                price = HttpUtility.HtmlDecode(prize.SelectSingleNode(".//div").InnerText);
                pricesList.Add(price);
            }

            foreach (HtmlNode line in doc.DocumentNode.SelectNodes("//*[@class = 'css-1ci0qpi']"))
            {
                contentList.Add(line.SelectSingleNode(".//ul").InnerHtml);   
            }
            


            //zdjecia

            //foreach (HtmlNode photo in doc.DocumentNode.SelectNodes("//img"))
            //{
            //    string value = photo.GetAttributeValue("src", string.Empty);
            //    string link = "https://apollo-ireland.akamaized.net";

            ////    if (string.Compare(value, 0, link, 0, 35) == 0)
            ////        if (value.IndexOf("644x461") > 0)
            ////            imgsList.Add(value);
            ////}

            //for (int i = 0; i < imgsList.Count; i++)
            //{
            //    {
            //        WebRequest requestPic = WebRequest.Create(imgsList[i]);
            //        WebResponse responsePic = requestPic.GetResponse();
            //        Image webImage = Image.FromStream(responsePic.GetResponseStream());
            //        string imagePath = path + "image_" + i + ".jpg";
            //        webImage.Save(imagePath);
            //        Property.propertyD.Add("Zdjecie_" + i, imagePath);
            //    }
            //}

            headersList.RemoveAll(string.IsNullOrEmpty);
            contentsList.RemoveAll(string.IsNullOrEmpty);
            addressesList.RemoveAll(string.IsNullOrEmpty);
            pricesList.RemoveAll(string.IsNullOrEmpty);
            contentList.RemoveAll(string.IsNullOrEmpty);

            header = headersList[0];
            header = header.Replace("\n", "");
            header = header.Trim();
            property.Title = header;

            text = contentsList[0];
            //text = text.Replace("\n", "");
            text = text.Trim();
            property.Content = text;

            loc = addressesList[0];
            loc = loc.Trim();
            property.Address = loc;

            price = pricesList[0];
            property.Prize = price;


            table = contentList[0];

            table = table.Replace("<strong>", "");
            table = table.Replace("</strong>", "");
            table = table.Replace("<ul>", "");
            table = table.Replace("<li>", "");
            table = table.Replace("/li>", "");
            table = table.Replace(">", "> ");
            table = table.Replace("Piętro", "Poziom");



            while (table.Length > 0)
            {
                string key = table.Substring(0, table.IndexOf(":"));
                table = table.Replace(key, "");
                table = table.Remove(0, 2);
                string value = table.Substring(0, table.IndexOf("<"));
                table = table.Replace(value, "");
                Property.propertyD.Add(key, value);
                table = table.Remove(0, 1);
            }


            Property.propertyD.Add("Tytul", header);
            Property.propertyD.Add("Opis", text);
            Property.propertyD.Add("Adres", loc);
            Property.propertyD.Add("Cena", price);


            return Property.propertyD;
        }
    }
}
