using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Podcast.Dominio.ValueObjects.FeedRss
{
	public class Channel
	{
		[XmlElement("item")]
		public List<Item> Items { get; set; }
	}
}
