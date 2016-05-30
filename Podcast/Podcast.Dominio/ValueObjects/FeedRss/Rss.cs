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
		[XmlElement("channel")]
		public Channel Channel { get; set; }
	}
}
