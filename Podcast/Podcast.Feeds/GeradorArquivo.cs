using Podcast.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Podcast.Feeds
{
    public static class GeradorArquivo
    {

        private static string GerarLinhaArquivoTexto(Episodio episodio)
        {
            return $"{episodio.Podcast}	{episodio.Serie}		{episodio.Titulo}	{episodio.Duracao}	{episodio.Publicacao}";
        }

        public static void GerarArquivoTexto(IEnumerable<Episodio> episodios, string caminhoArquivo)
        {
            var episodiosOrdenados = episodios.OrderBy(x => x.Publicacao).ThenBy(x => x.Titulo);

            var sb = new StringBuilder();
            foreach (var episodio in episodiosOrdenados)
            {
                sb.AppendLine(GerarLinhaArquivoTexto(episodio));
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Dados salvos no arquivo: {caminhoArquivo}");

            File.WriteAllText(caminhoArquivo, sb.ToString());
        }

    }
}
