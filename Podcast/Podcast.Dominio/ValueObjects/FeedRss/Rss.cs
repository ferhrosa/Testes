using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Podcast.Dominio.ValueObjects.FeedRss
{
	[XmlRoot("rss")]
	public class Rss
	{

        public const string NamespaceItunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";

        [XmlElement("channel")]
		public Channel Channel { get; set; }

	}
}
