using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Podcast.Dominio.ValueObjects
{
	[XmlRoot("Parametros")]
	public class Parametros
	{

		[XmlArray("Feeds")]
		[XmlArrayItem("Feed")]
		public List<Feed> Feeds { get; set; }

		[XmlElement]
		public int LimiteEpisodios { get; set; }

		[XmlElement]
		public string ArquivoSaida { get; set; }

	}
}
