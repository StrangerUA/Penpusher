﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Penpusher.Models;

namespace Penpusher.Services.ContentService
{
    public class RssParser : IParser
    {
        public IEnumerable<Article> GetParsedArticles(RssChannelModel rssModel)
        {
            var parsedArticles = new List<Article>();
            IEnumerable<XElement> rssArticles = rssModel.RssFile.Descendants("item");

            foreach (XElement post in rssArticles)parsedArticles
                    .Add(ParseArticle(post, rssModel.ProviderId));
            return parsedArticles;
        }

        private Article ParseArticle(XElement post, int idNewsProvider)
        {
            DateTime date;
            if (!DateTime.TryParse(GetDescedantValue(post, "pubDate"), out date))
                date = DateTime.Now;

            return new Article()
            {
                Description = GetDescedantValue(post, "description"),
                Title = GetDescedantValue(post, "title"),
                Date = date,
                Link = GetDescedantValue(post, "link"),
                Id = 0,
                Image = GetImage(post),
                IdNewsProvider = idNewsProvider
            };
        }

        private string GetDescedantValue(XElement post, string descedantName)
        {
            return post.Element(descedantName)?.Value;
        }

        private string GetImage(XElement post)
        {
            XElement image = post.Element("enclosure");
            if (image != null && image.HasAttributes)
            {
                foreach (XAttribute attribute in image.Attributes())
                {
                    string value = attribute.Value.ToLower();
                    if (value.StartsWith("http://") && (value.EndsWith(".jpg") || value.EndsWith(".png") || value.EndsWith(".gif")))
                    {
                        return value; // Add here the image link to some array
                    }
                }
            }
            return null;
        }
    }
}