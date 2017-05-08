using Podcast.Dominio.Entidades;
using Podcast.Feeds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Podcast.Testes
{
    class Program
    {
        static void Main(string[] args)
        {

            var feeds = LeitorFeeds.Parametros.Feeds;

            var tasks = new List<Task<IEnumerable<Episodio>>>();
            //var tasks = new List<Task>();

            var feedsFiltrados = from f in feeds
                                 where
                                    // Não aplica filtro se não forem passados parâmetros.
                                    !args.Any()
                                    // Verifica se o nome do feed está entre os parâmetros de execução do programa.
                                    || args.Contains(f.Nome, StringComparer.OrdinalIgnoreCase)
                                 select f;

            var ignorarLimiteEpisodios = args.Any() || LeitorFeeds.Parametros.ArquivoSaidaPorPodcast;

            foreach (var feed in feedsFiltrados)
            {
                //não funciona dessa forma: var task = new Task(async () => {
                var task = Task.Run(async () =>
                {
                    var episodios = await LeitorFeeds.ListarEpisodiosAsync(feed, ignorarLimiteEpisodios);

                    // Gera um arquivo por podcast.
                    if (LeitorFeeds.Parametros.ArquivoSaidaPorPodcast)
                    {
                        var ultimaPublicacao = episodios.Max(e => e.Publicacao);

                        var caminhoBase = new FileInfo(LeitorFeeds.Parametros.ArquivoSaida).DirectoryName;
                        var caminhoArquivo = Path.Combine(caminhoBase, $"{ultimaPublicacao.ToString("yyyy-MM-dd HH-mm")} - {feed.Nome}{(!string.IsNullOrWhiteSpace(feed.Serie) && feed.Serie != feed.Nome ? $" - {feed.Serie}" : "")}.txt");

                        GeradorArquivo.GerarArquivoTexto(episodios, caminhoArquivo);
                    }

                    if (!ignorarLimiteEpisodios && LeitorFeeds.Parametros.LimiteEpisodios > 0 && episodios.Count() > LeitorFeeds.Parametros.LimiteEpisodios)
                    {
                        return episodios.Take(LeitorFeeds.Parametros.LimiteEpisodios);
                    }
                    else
                    {
                        return episodios;
                    }
                });

                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            // Gera um arquivo unificado.
            Console.WriteLine();

            var episodiosGeral = new List<Episodio>();
            foreach (var task in tasks)
            {
                episodiosGeral.AddRange(task.Result);
            }

            GeradorArquivo.GerarArquivoTexto(episodiosGeral, LeitorFeeds.Parametros.ArquivoSaida);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Pressione qualquer tecla para continuar...");

            Console.ReadKey();
        }

    }
}
