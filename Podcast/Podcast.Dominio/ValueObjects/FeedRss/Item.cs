using System.Xml.Serialization;

namespace Podcast.Dominio.ValueObjects.FeedRss
{
    public class Item
    {
        [XmlElement("guid")]
        public string Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("pubDate")]
        public string Publicacao { get; set; }

        [XmlElement("duration", Namespace = Rss.NamespaceItunes)]
        public string Duration { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        private Enclosure enclosure;

        [XmlElement("enclosure")]
        public Enclosure Enclosure
        {
            //get;set;
            get { return this.enclosure = this.enclosure ?? new Enclosure(); }
            set { this.enclosure = value; }
        }
    }
}
