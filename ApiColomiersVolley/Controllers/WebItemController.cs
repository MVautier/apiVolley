using ApiColomiersVolley.BLL.DMItem.Business;
using ApiColomiersVolley.BLL.DMItem.Business.Interfaces;
using ApiColomiersVolley.BLL.DMItem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiColomiersVolley.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebItemController : ControllerBase
    {
        private readonly ILogger<WebItemController> _logger;
        private readonly IBSItem _bsItem;

        public WebItemController(ILogger<WebItemController> logger, IBSItem bsItem)
        {
            _logger = logger;
            _bsItem = bsItem;
        }

        /// <summary>
        /// Gets all configured webitems
        /// </summary>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<WebItem>> Get()
        {
            return await _bsItem.GetListe();
        }

        /// <summary>
        /// Gets all configured pages
        /// </summary>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpGet]
        [Route("tree")]
        public async Task<Tree> GetTree()
        {
            return await _bsItem.GetTree();
        }

        /// <summary>
        /// Adds or update a WebItem
        /// </summary>
        /// <param name="item">The item to add or update</param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpPost]
        [Route("add")]
        public async Task<WebItem> AddOrUpdate([FromBody] WebItem item)
        {
            return await _bsItem.AddOrUpdate(item);
        }

        /// <summary>
        /// Removes a WebItem
        /// </summary>
        /// <param name="id">The item identifier</param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpDelete]
        [Route("delete")]
        public async Task<bool> Delete([FromBody] int id)
        {
            return await _bsItem.Remove(id);
        }
    }
}