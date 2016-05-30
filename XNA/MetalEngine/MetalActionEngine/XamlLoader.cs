using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MetalActionEngine
{
    internal class XamlLoader
    {
        private static string _assemblyName;

        private static string AssemblyName
        {
            get
            {
                if ( String.IsNullOrWhiteSpace(_assemblyName) )
                {
                    _assemblyName = System.Reflection.Assembly.GetEntryAssembly().FullName;
                }

                return _assemblyName;
            }
        }


        internal static T LoadComponent<T>(string relativeFilePath)
        {
            if (relativeFilePath == null)
                throw new ArgumentNullException("relativeFilePath");

            relativeFilePath = relativeFilePath.Replace("\\", "/");

            if (relativeFilePath.Substring(0, 1) == "/")
                relativeFilePath = relativeFilePath.Substring(1, relativeFilePath.Length - 1);

            if ( relativeFilePath.Length < 6 || relativeFilePath.Substring(relativeFilePath.Length - 6, 5).ToLower() != ".xaml")
                relativeFilePath +=".xaml";

            var path = String.Format("/{0};component/{1}", AssemblyName, relativeFilePath);

            var uri = new Uri(path, UriKind.Relative);

            return (T)Application.LoadComponent(uri);
        }

    }


}
