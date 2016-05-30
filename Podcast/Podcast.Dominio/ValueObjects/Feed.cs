using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Podcast.Dominio.ValueObjects
{
	public class Feed
	{

		[XmlAttribute]
		public string Nome { get; set; }

		[XmlAttribute]
		public string Serie { get; set; }

		[XmlAttribute]
		public string Url { get; set; }

	}
}
