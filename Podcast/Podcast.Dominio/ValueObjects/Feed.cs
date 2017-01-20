using System.Collections.Generic;
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

        [XmlElement("Serie")]
        public List<Serie> Series { get; set; }

    }
}
