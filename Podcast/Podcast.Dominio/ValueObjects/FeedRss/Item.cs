using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	}
}
