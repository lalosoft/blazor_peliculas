using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorPeliculas.Client.Helpers
{
    public static class NavigationManagerExtensions
    {
        /// <summary>
        /// Construye los QS a partir de un arreglo de valores
        /// </summary>
        /// <param name="navigationManager"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Dictionary<string,string> ObtenerQueryStrings(this NavigationManager navigationManager, string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !url.Contains("?") || url.Substring(url.Length - 1) == "?") return null;

            //https://dominio.com?llave1=valor1&llave2=valor2&llave3=valor3
            var queryStrings = url.Split(new string[] { "?" }, System.StringSplitOptions.None)[1];
            Dictionary<string, string> dicQueryStrings = queryStrings.Split('&')
                                                            .ToDictionary(c => c.Split('=')[0],
                                                                c => Uri.UnescapeDataString(c.Split('=')[1]));
            return dicQueryStrings;
        }
    }
}
