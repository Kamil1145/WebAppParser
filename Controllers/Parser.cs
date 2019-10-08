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
using WebAppParser.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppParser.Controllers
{
    public class Parser 
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
            var doc = web.Load(url);

            int index, s;
            string header, text, loc, price;

            var Title = doc.DocumentNode.SelectNodes("//*[@class = 'offer-titlebox']");
            var Content = doc.DocumentNode.SelectNodes("//*[@class='clr descriptioncontent marginbott20']");
            var Address = doc.DocumentNode.SelectNodes("//*[@class='offer-user__address']");
            var ContentArray = doc.DocumentNode.SelectNodes("//table[@class='details fixed marginbott20 margintop5 full']/tr/td");
            var Price = doc.DocumentNode.SelectNodes("//*[@class='offer-sidebar__inner offeractions']");
            var Photos = doc.DocumentNode.SelectNodes("//*[@class='offerdescription clr']");

            foreach (var title in Title)
            {
                header = HttpUtility.HtmlDecode(title.SelectSingleNode(".//h1").InnerText);
                headersList.Add(header);
            }


            foreach (var content in Content)
            {
                text = HttpUtility.HtmlDecode(content.SelectSingleNode(".//div[@id= 'textContent']").InnerText);
                contentsList.Add(text);
            }


            foreach (var address in Address)
            {
                loc = HttpUtility.HtmlDecode(address.SelectSingleNode(".//p").InnerText);
                addressesList.Add(loc);
            }


            foreach (var cell in ContentArray)
            {
                contentList.Add(cell.InnerText);
            }


            foreach (var prize in Price)
            {
                price = HttpUtility.HtmlDecode(prize.SelectSingleNode("//*[@id='offeractions']/div[1]/strong").InnerText);
                pricesList.Add(price);
            }

            foreach (HtmlNode photo in doc.DocumentNode.SelectNodes("//img"))
            {
                string value = photo.GetAttributeValue("src", string.Empty);
                string link = "https://apollo-ireland.akamaized.net";

                if (string.Compare(value, 0, link, 0, 35) == 0)
                    if (value.IndexOf("644x461") > 0)
                        imgsList.Add(value);
            }

            for (int i = 0; i < imgsList.Count; i++)
            {
                {
                    WebRequest requestPic = WebRequest.Create(imgsList[i]);
                    WebResponse responsePic = requestPic.GetResponse();
                    Image webImage = Image.FromStream(responsePic.GetResponseStream());
                    webImage.Save(@"c:\\temporary\\" + i + ".jpg");
                }
            }

            headersList.RemoveAll(string.IsNullOrEmpty);
            contentsList.RemoveAll(string.IsNullOrEmpty);
            addressesList.RemoveAll(string.IsNullOrEmpty);
            pricesList.RemoveAll(string.IsNullOrEmpty);

            header = headersList[0];
            header = header.Replace("\n", "");
            header = header.Trim();
            property.Title = header;

            text = contentsList[0];
            text = text.Replace("\n", "");
            text = text.Trim();
            property.Content = text;

            loc = addressesList[0];
            loc = loc.Trim();
            property.Address = loc;

            price = pricesList[0];
            property.Prize = price;

            for (int i = 0; i < contentList.Count; i++)
            {
                contentList[i] = contentList[i].Replace("\t", "");
                contentList[i] = contentList[i].Replace("&nbsp;", "");
                contentList[i] = contentList[i].Replace("\n", " ");
                contentList[i] = contentList[i].Trim();
                contentList[i] = contentList[i].Replace("\t", "");
                contentList[i] = contentList[i].Replace("\n", "");

                index = contentList[i].IndexOf("   ");
                if (index == -1)
                {
                    continue;
                }
                additionalContentList.Add(contentList[i].Substring(index));
                contentList[i] = contentList[i].Remove(index);
            }

            for (int i = 0; i < additionalContentList.Count; i++)
            {
                additionalContentList[i] = additionalContentList[i].Trim();
            }

            contentList.RemoveAll(string.IsNullOrEmpty);
            additionalContentList.RemoveAll(string.IsNullOrEmpty);


            if (contentList.Count > additionalContentList.Count)
            {
                s = contentList.Count;
            }
            else
            {
                s = additionalContentList.Count;
            }

            Property.propertyD.Add("Tytul", header);
            Property.propertyD.Add("Opis", text);
            Property.propertyD.Add("Adres", loc);
            Property.propertyD.Add("Cena", price);




            for (int i = 0; i < s; i++)
            {
                switch (contentList[i])
                {
                    case "Cena za m²":
                        Property.propertyD.Add("Cena za m²", additionalContentList[i]);
                        break;

                    case "Umeblowane":
                        Property.propertyD.Add("Umeblowane", additionalContentList[i]);
                        break;

                    case "Oferta od":
                        Property.propertyD.Add("Oferta od", additionalContentList[i]);
                        break;

                    case "Poziom":
                        Property.propertyD.Add("Poziom", additionalContentList[i]);
                        break;

                    case "Rynek":
                        Property.propertyD.Add("Rynek", additionalContentList[i]);
                        break;

                    case "Rodzaj zabudowy":
                        Property.propertyD.Add("Rodzaj zabudowy", additionalContentList[i]);
                        break;

                    case "Powierzchnia":
                        Property.propertyD.Add("Powierzchnia", additionalContentList[i]);
                        break;

                    case "Liczba pokoi":
                        Property.propertyD.Add("Liczba pokoi", additionalContentList[i]);
                        break;

                    case "Czynsz (dodatkowo)":
                        Property.propertyD.Add("Czynsz(dodatkowo)", additionalContentList[i]);
                        break;

                    default:
                        break;
                }
            }

            return Property.propertyD;
        }
    }
}
