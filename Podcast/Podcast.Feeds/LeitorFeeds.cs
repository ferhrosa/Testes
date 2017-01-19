using Podcast.Dominio.Entidades;
using Podcast.Dominio.ValueObjects;
using Podcast.Dominio.ValueObjects.FeedRss;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Podcast.Feeds
{
	public class LeitorFeeds
	{

		/// <summary>
		/// Nome do arquivo que possui os parâmetros para execução da aplicação.
		/// </summary>
		private const string NomeArquivoParametros = "Parametros.xml";

		private static readonly string CaminhoExecutavel = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


		private static Parametros parametros;

		public static Parametros Parametros
		{
			get
			{
				if(parametros == null)
				{
					// Abre o arquivo de parâmetros para realizar a leitura.
					var stream = new FileStream(Path.Combine(CaminhoExecutavel, NomeArquivoParametros), FileMode.Open);

					// Cria objeto que serializa e desserializa o objeto Parametros.
					var serializer = new XmlSerializer(typeof(Parametros));

					// Cria o objeto Parametros de acordo com o arquivo de parâmetros.
					parametros = (Parametros)serializer.Deserialize(stream);

					// Fecha o arquivo de parâmetros.
					stream.Close();
				}

				return parametros;
			}
		}

		public static async Task<IEnumerable<Episodio>> ListarEpisodiosAsync(Feed feed, bool ignorarLimiteEpisodios = false)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Iniciando: {0}", feed.Nome);

			var episodios = new List<Episodio>();

			try
			{
				var httpClient = new WebClient();
				var stream = await httpClient.OpenReadTaskAsync(feed.Url);

				// Cria objeto que serializa e desserializa o objeto Parametros.
				var serializer = new XmlSerializer(typeof(Rss));

				// Cria o objeto Parametros de acordo com o arquivo de parâmetros.
				var rss = (Rss)serializer.Deserialize(stream);

				foreach(var item in rss.Channel.Items)
				{
					episodios.Add(new Episodio
					{
						Podcast = feed.Nome,
						Serie = feed.Serie,
						Id = item.Id,
						Titulo = item.Title,
						Publicacao = ConverterData(item.Publicacao),
                        Duracao = ConverterTempo(item.Duration)
                    });
				}

				stream.Close();

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Finalizando: {0}", feed.Nome);
			}
			catch(Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Falhou: {0} | {1}", feed.Nome, ex.Message);
			}

            episodios = episodios.OrderByDescending(x => x.Publicacao).ThenBy(x => x.Titulo).ToList();

            if (!ignorarLimiteEpisodios && Parametros.LimiteEpisodios > 0 && episodios.Count() > Parametros.LimiteEpisodios)
            {
                return episodios.Take(Parametros.LimiteEpisodios);
            }
            else
            {
                return episodios;
            }

        }

        private static DateTime ConverterData(string dataTexto)
		{
			var data = new DateTime();
			var dataTextoOriginal = dataTexto;

			//if(Regex.IsMatch(dataTexto.Substring(dataTexto.Length - 5), @"^([-+]{1}[0-9]{4})$"))
			//{
			//	dataTexto = dataTexto.Substring(0, dataTexto.Length - 5) + dataTexto.Substring(dataTexto.Length - 5, 3) + ":" + dataTexto.Substring(dataTexto.Length - 2);
			//}

			//if(!DateTime.TryParseExact(dataTexto, "ddd, dd MMM yyyy HH:mm:ss zzz", CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out data))
			//{
			//	if(!DateTime.TryParseExact(dataTexto, "ddd, dd MMM yyyy HH:mm:ss zzz", CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out data))
			//	{
			//		if(!DateTime.TryParseExact(dataTexto, "ddd, d MMM yyyy HH:mm:ss zzz", CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out data))
			//		{
			//			//dataTexto = dataTexto.Replace("GMT", "+00:00");

			//			if(Regex.Replace(dataTexto.Substring(dataTexto.Length - 3), @"[0-9:]", "") != "")
			//			{
			//				dataTexto = dataTexto.Substring(0, dataTexto.Length - 3).Trim();
			//			}

			//			if(!DateTime.TryParseExact(dataTexto, "ddd, dd MMM yyyy HH:mm:ss", CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out data))
			//			{
			//				if(!DateTime.TryParseExact(dataTexto, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out data))
			//				{
			//					Console.ForegroundColor = ConsoleColor.Red;
			//					Console.WriteLine("Falha ao converter data: {0}", dataTextoOriginal);
			//				}
			//			}
			//		}
			//	}
			//}

			dataTexto = dataTexto.Trim();

			// Se a data possuir o dia da semana no início (exemplo: "Sat,"), esse é removido,
			// porque se não estiver de acordo com a data ocorre erro na conversão.
			if(Regex.IsMatch(dataTexto.Substring(0, 4), @"^(\D{3},{1})$"))
			{
				dataTexto = dataTexto.Substring(4).Trim();
			}

			// Tenta realizar uma conversão sem mais nenhuma alteração na data original.
			if(!DateTime.TryParse(dataTexto, out data))
			{
				// Se ocorrer algum erro de conversão, verifica se no final da data tem
				// o fuso horário escrito em forma de sigla (exemplos: PDT, UTC).
				// Esse formato não é suportado, então se estiver assim esse texto é removido do final da data original.
				if(Regex.Replace(dataTexto.Substring(dataTexto.Length - 3), @"[0-9:]", "") != "")
				{
					dataTexto = dataTexto.Substring(0, dataTexto.Length - 3).Trim();
				}

				if(!DateTime.TryParse(dataTexto, out data))
				{
					// Se ocorrer erro mesmo assim, apenas gera log de falha na conversão da data.
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Falha ao converter data: {0}", dataTextoOriginal);
				}
			}

			return data;
		}

        private static TimeSpan? ConverterTempo(string tempoTexto)
        {
            if (String.IsNullOrWhiteSpace(tempoTexto) || !tempoTexto.Contains(":"))
            {
                return null;
            }

            var partes = tempoTexto.Split(':');

            if (partes.Length > 3)
            {
                // Se tiver mais que 3 partes, não está nos formatos válidos (HH:MM:SS, H:MM:SS, MM:SS ou M:SS).
                // Definição: https://lists.apple.com/archives/syndication-dev/2005/Nov/msg00002.html#_Toc526931683
                return null;
            }

            int h = 0, m = 0, s = 0;

            try
            {
                // Última parte.
                s = Convert.ToInt32(partes[partes.Length - 1]);
                // Penúltima parte.
                m = Convert.ToInt32(partes[partes.Length - 2]);

                if (partes.Length == 3)
                {
                    h = Convert.ToInt32(partes[0]);
                }

                var tempo = new TimeSpan(h, m, s);

                if (tempo == TimeSpan.Zero)
                {
                    return null;
                }

                return tempo;
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao converter o o texto \"{tempoTexto}\" para TimeSpan.\r\nErro: {ex.Message}");
            }
        }

	}
}
