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

			var episodios = new List<Episodio>();

			var tasks = new List<Task<IEnumerable<Episodio>>>();

			foreach(var feed in feeds)
			{
				tasks.Add(LeitorFeeds.ListarEpisodios(feed));
				//foreach(var episodio in LeitorFeeds.ListarEpisodios(feed))
				//{
				//	//Console.WriteLine("{0} | {1} | {2}", episodio.Titulo, episodio.Id, episodio.Publicacao);
				//	episodios.Add(episodio);
				//}

				//Console.WriteLine();
			}

			Task.WhenAll(tasks);

			foreach(var task in tasks)
			{
				episodios.AddRange(task.Result);
			}
			
			Console.WriteLine();

			var episodiosOrdenados = episodios.OrderByDescending(x => x.Publicacao).ThenBy(x => x.Titulo);

			var sb = new StringBuilder();
			foreach(var episodio in episodiosOrdenados)
			{
				sb.AppendLine(String.Format("{0}	{1}		{2}	{3:d}", episodio.Podcast, episodio.Serie, episodio.Titulo, episodio.Publicacao));
			}

			Console.ForegroundColor = ConsoleColor.White;
			//Console.WriteLine(sb);
			
			var caminhoArquivo = @"C:\Temp\podcasts.txt";
			Console.WriteLine("Salvando dados no arquivo: {0}", caminhoArquivo);

			File.WriteAllText(caminhoArquivo, sb.ToString());

			Console.ReadKey();
		}
	}
}
