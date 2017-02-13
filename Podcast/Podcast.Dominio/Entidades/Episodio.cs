using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Podcast.Dominio.Entidades
{
	public class Episodio
	{
		public string Podcast { get; set; }

		public string Serie { get; set; }

		public string Id { get; set; }

		public string Titulo { get; set; }

		public DateTime Publicacao { get; set; }

        public TimeSpan? Duracao { get; set; }

        public string Link { get; set; }

        public string EnclosureUrl { get; set; }
    }
}
