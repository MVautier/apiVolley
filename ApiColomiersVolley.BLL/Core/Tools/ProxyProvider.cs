using Microsoft.Extensions.Configuration;
using System.Net;

namespace ApiColomiersVolley.BLL.Core.Tools
{
    /// <summary>
    /// Proxy de sortie vers Internet, lu depuis la configuration (clé "Proxy:Url").
    ///
    /// L'hébergement de production (IONOS mutualisé) n'autorise pas les appels sortants
    /// directs : ils passent par le proxy de l'hébergeur. Son adresse était codée en dur,
    /// si bien qu'un changement de nom d'hôte ou de port chez IONOS ne pouvait être suivi
    /// qu'au prix d'une recompilation et d'un redéploiement complets -- avec, pendant tout
    /// ce temps, l'impossibilité de lancer le moindre paiement. La lire en configuration
    /// permet de la corriger par FTP.
    ///
    /// Une URL vide ou absente signifie sortie directe, sans proxy.
    /// </summary>
    public static class ProxyProvider
    {
        public const string ConfigurationKey = "Proxy:Url";

        public static IWebProxy? GetProxy(IConfiguration config)
        {
            var url = config?.GetValue<string>(ConfigurationKey);
            return string.IsNullOrWhiteSpace(url) ? null : new WebProxy(url, true);
        }
    }
}
