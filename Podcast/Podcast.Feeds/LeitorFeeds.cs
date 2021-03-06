﻿using Podcast.Dominio.Entidades;
using Podcast.Dominio.ValueObjects;
using Podcast.Dominio.ValueObjects.FeedRss;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
                if (parametros == null)
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Iniciando: {0}", feed.Nome);

            var episodios = new List<Episodio>();

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var httpClient = new WebClientCustom();
                httpClient.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";

                var stream = await httpClient.OpenReadTaskAsync(feed.Url);

                // Cria objeto que serializa e desserializa o objeto Parametros.
                var serializer = new XmlSerializer(typeof(Rss));

                // Cria o objeto Parametros de acordo com o arquivo de parâmetros.
                var rss = (Rss)serializer.Deserialize(stream);

                foreach (var item in rss.Channel.Items)
                {
                    var episodio = new Episodio
                    {
                        Podcast = feed.Nome,
                        Serie = feed.Serie,
                        Id = (feed.UsarEnclosureUrlComoId ? (item.Enclosure.Url) : item.Id),
                        Titulo = item.Title,
                        Publicacao = ConverterData(item.Publicacao, feed, item),
                        Duracao = ConverterTempo(item.Duration),
                        Link = item.Link,
                        EnclosureUrl = item.Enclosure.Url
                    };

                    if (feed.Series != null && feed.Series.Any())
                    {
                        var encontrou = false;

                        foreach (var serie in feed.Series)
                        {
                            foreach (var formato in serie.Formatos.Where(f => !String.IsNullOrWhiteSpace(f)))
                            {
                                if (Regex.IsMatch(episodio.Titulo, formato, RegexOptions.IgnoreCase))
                                {
                                    episodio.Serie = serie.Nome;
                                    encontrou = true;
                                    break;
                                }
                            }

                            if (encontrou) { break; }
                        }
                    }

                    episodios.Add(episodio);
                }

                stream.Close();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Finalizado: {feed.Nome}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Falhou: {feed.Nome} | {ex.Message}");
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

        private static DateTime ConverterData(string dataTexto, Feed feed, Item item)
        {
            var data = new DateTime();
            var dataTextoOriginal = dataTexto;

            dataTexto = dataTexto.Trim();

            // Se a data possuir o dia da semana no início (exemplo: "Sat,"), esse é removido,
            // porque se não estiver de acordo com a data ocorre erro na conversão.
            if (Regex.IsMatch(dataTexto.Substring(0, 4), @"^(\D{3},{1})$"))
            {
                dataTexto = dataTexto.Substring(4).Trim();
            }

            // Tenta realizar uma conversão sem mais nenhuma alteração na data original.
            if (!DateTime.TryParse(dataTexto, out data))
            {
                // Se ocorrer algum erro de conversão, verifica se no final da data tem
                // o fuso horário escrito em forma de sigla (exemplos: PDT, UTC).
                // Esse formato não é suportado, então se estiver assim esse texto é removido do final da data original.
                if (Regex.Replace(dataTexto.Substring(dataTexto.Length - 3), @"[0-9:]", "") != "")
                {
                    dataTexto = dataTexto.Substring(0, dataTexto.Length - 3).Trim();
                }

                if (!DateTime.TryParse(dataTexto, out data))
                {
                    // Se ocorrer erro mesmo assim, apenas gera log de falha na conversão da data.
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Falha ao converter data. Feed: {feed.Nome} | Item: {item.Title} | {dataTextoOriginal}");

                    //throw new Exception($"Falha ao converter data: {dataTextoOriginal}");
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
            catch (Exception ex)
            {
                throw new Exception($"Erro ao converter o texto \"{tempoTexto}\" para TimeSpan.\r\nErro: {ex.Message}");
            }
        }

    }
}
