using ApiColomiersVolley.BLL.Core.Helloasso.Interfaces;
using ApiColomiersVolley.BLL.Core.Helloasso.Models;
using ApiColomiersVolley.BLL.Core.Models.Generic;
using ApiColomiersVolley.BLL.Core.Tools.Models;
using ApiColomiersVolley.BLL.DMAdherent.Business;
using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ApiColomiersVolley.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloassoController : ColomiersVolleyController
    {
        private readonly IServiceHelloasso _serviceHello;

        public HelloassoController(IServiceHelloasso serviceHello)
        {
            _serviceHello = serviceHello;
        }

        /// <summary>
        /// Gets a paged adherents list
        /// </summary>
        /// <param name="cart"></param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpPost]
        [Route("initiate")]
        public async Task<PostIntentResult> InitiateIntent([FromBody] Cart cart)
        {
            try
            {
                return await _serviceHello.SendCheckout(cart);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Gets a paged adherents list
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpGet]
        [Route("receipt/{id}")]
        public async Task<GetIntentResult> GetReceiptUrl(string id)
        {
            try
            {
                return await _serviceHello.GetReceiptUrl(id);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Gets a paged adherents list
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpGet]
        [Route("token")]
        public async Task<InMemoryToken> GetToken()
        {
            try
            {
                return await _serviceHello.GetTokenByApi();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Reçoit les notifications serveur-à-serveur (webhook) d'Helloasso pour finaliser
        /// un paiement indépendamment du retour navigateur de l'utilisateur
        /// </summary>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="401">Unauthorized / Signature invalide</response>
        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> Webhook()
        {
            string rawBody;
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                rawBody = await reader.ReadToEndAsync();
            }

            Request.Headers.TryGetValue("x-ha-signature", out var signatureHeader);
            var remoteIp = HttpContext.GetRemoteIPAddress()?.ToString();

            // HandleWebhook gère elle-même les erreurs de traitement (réponse 200 quand même,
            // signature/IP déjà validées à ce stade) pour ne pas déclencher de retries Helloasso
            // sur des erreurs applicatives non transitoires ; seule une signature/IP invalide
            // renvoie false ici.
            var isValid = await _serviceHello.HandleWebhook(rawBody, signatureHeader, remoteIp);
            if (!isValid)
            {
                return Unauthorized();
            }

            return Ok();
        }
    }
}
