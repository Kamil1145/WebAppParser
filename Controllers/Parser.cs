using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using WebAppParser.Models;
using System.Drawing;

namespace WebAppParser.Controllers
{
    public class Parser
    {

        private List<Property> properties = new List<Property>();

        public List<Property> Properties
        {
            get { return properties; }
            set { properties = value; }
        }

        public static void ScrapeData(string page)
        {
            List<string> headersL = new List<string>();
            List<string> contentsL = new List<string>();
            List<string> addressesL = new List<string>();
            List<string> pricesL = new List<string>();
            List<string> tabsL = new List<string>();
            List<string> tabs2L = new List<string>();
            List<string> imgsL = new List<string>();

            var web = new HtmlWeb();
            var doc = web.Load(page);

            int ind, s;
            string header, text, loc, price;

            var Title = doc.DocumentNode.SelectNodes("//*[@class = 'offer-titlebox']");
            var Content = doc.DocumentNode.SelectNodes("//*[@class='clr descriptioncontent marginbott20']");
            var Address = doc.DocumentNode.SelectNodes("//*[@class='offer-user__address']");
            var Tab = doc.DocumentNode.SelectNodes("//table[@class='details fixed marginbott20 margintop5 full']/tr/td");
            var Price = doc.DocumentNode.SelectNodes("//*[@class='offer-sidebar__inner offeractions']");
            var Photos = doc.DocumentNode.SelectNodes("//*[@class='offerdescription clr']");

            foreach (var title in Title)
            {
                header = HttpUtility.HtmlDecode(title.SelectSingleNode(".//h1").InnerText);
                headersL.Add(header);
            }


            foreach (var content in Content)
            {
                text = HttpUtility.HtmlDecode(content.SelectSingleNode(".//div[@id= 'textContent']").InnerText);
                contentsL.Add(text);
            }


            foreach (var address in Address)
            {
                loc = HttpUtility.HtmlDecode(address.SelectSingleNode(".//p").InnerText);
                addressesL.Add(loc);
            }


            foreach (var cell in Tab)
            {
                tabsL.Add(cell.InnerText);
            }


            foreach (var prize in Price)
            {
                price = HttpUtility.HtmlDecode(prize.SelectSingleNode("//*[@id='offeractions']/div[1]/strong").InnerText);
                pricesL.Add(price);
            }

            foreach (HtmlNode photo in doc.DocumentNode.SelectNodes("//img"))
            {

                string value = photo.GetAttributeValue("src", string.Empty);
                string link = "https://apollo-ireland.akamaized.net";

                if (string.Compare(value, 0, link, 0, 35) == 0)
                    if (value.IndexOf("644x461") > 0)
                        imgsL.Add(value);
            }

            for (int i = 0; i < imgsL.Count; i++)
            {
                {
                    WebRequest requestPic = WebRequest.Create(imgsL[i]);
                    WebResponse responsePic = requestPic.GetResponse();
                    Image webImage = Image.FromStream(responsePic.GetResponseStream());
                    webImage.Save(@"c:\\temporary\\" + i + ".jpg");
                }
            }

            headersL.RemoveAll(string.IsNullOrEmpty);
            contentsL.RemoveAll(string.IsNullOrEmpty);
            addressesL.RemoveAll(string.IsNullOrEmpty);
            pricesL.RemoveAll(string.IsNullOrEmpty);

            header = headersL[0];
            header = header.Replace("\n", "");
            header = header.Trim();

            text = contentsL[0];
            text = text.Replace("\n", "");
            text = text.Trim();

            loc = addressesL[0];
            loc = loc.Trim();

            price = pricesL[0];

            for (int i = 0; i < tabsL.Count; i++)
            {
                tabsL[i] = tabsL[i].Replace("\t", "");
                tabsL[i] = tabsL[i].Replace("&nbsp;", "");
                tabsL[i] = tabsL[i].Replace("\n", " ");
                tabsL[i] = tabsL[i].Trim();
                tabsL[i] = tabsL[i].Replace("\t", "");
                tabsL[i] = tabsL[i].Replace("\n", "");

                ind = tabsL[i].IndexOf("   ");
                if (ind == -1)
                {
                    continue;
                }
                tabs2L.Add(tabsL[i].Substring(ind));
                tabsL[i] = tabsL[i].Remove(ind);
            }

            for (int i = 0; i < tabs2L.Count; i++)
            {
                tabs2L[i] = tabs2L[i].Trim();
            }

            tabsL.RemoveAll(string.IsNullOrEmpty);
            tabs2L.RemoveAll(string.IsNullOrEmpty);


            if (tabsL.Count > tabs2L.Count)
            {
                s = tabsL.Count;
            }
            else
            {
                s = tabs2L.Count;
            }

            Property.propertyD.Add("Tytul", header);
            Property.propertyD.Add("Opis", text);
            Property.propertyD.Add("Adres", loc);
            Property.propertyD.Add("Cena", price);


            for (int i = 0; i < s; i++)
            {
                switch (tabsL[i])
                {
                    case "Cena za m²":
                        Property.propertyD.Add("Cena za m²", tabs2L[i]);
                        break;

                    case "Umeblowane":
                        Property.propertyD.Add("Umeblowane", tabs2L[i]);
                        break;

                    case "Oferta od":
                        Property.propertyD.Add("Oferta od", tabs2L[i]);
                        break;

                    case "Poziom":
                        Property.propertyD.Add("Poziom", tabs2L[i]);
                        break;

                    case "Rynek":
                        Property.propertyD.Add("Rynek", tabs2L[i]);
                        break;

                    case "Rodzaj zabudowy":
                        Property.propertyD.Add("Rodzaj zabudowy", tabs2L[i]);
                        break;

                    case "Powierzchnia":
                        Property.propertyD.Add("Powierzchnia", tabs2L[i]);
                        break;

                    case "Liczba pokoi":
                        Property.propertyD.Add("Liczba pokoi", tabs2L[i]);
                        break;

                    case "Czynsz (dodatkowo)":
                        Property.propertyD.Add("Czynsz(dodatkowo)", tabs2L[i]);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
