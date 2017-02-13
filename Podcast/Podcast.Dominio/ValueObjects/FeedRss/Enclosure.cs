using System;
using System.Xml.Serialization;

namespace Podcast.Dominio.ValueObjects.FeedRss
{
    public class Enclosure
    {
        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("length")]
        public string Length { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}
