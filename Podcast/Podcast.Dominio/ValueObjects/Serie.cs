using System.Collections.Generic;
using System.Xml.Serialization;

namespace Podcast.Dominio.ValueObjects
{
    public class Serie
    {

		[XmlAttribute]
        public string Nome { get; set; }

		[XmlElement("Formato")]
        public List<string> Formatos { get; set; }

    }
}