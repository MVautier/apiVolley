using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMItem.Business.Interfaces;
using ApiColomiersVolley.BLL.DMItem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiColomiersVolley.Controllers
{/// <summary>
 /// Traitement de tous adhérents
 /// </summary>
 /// <response code="200">Success / Succès de la requête</response>
 /// <response code="204">No content / Aucune donnée</response>
 /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
 /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
 /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
    [ApiController]
    [Route("api/[controller]")]
    public class AdherentController : ControllerBase
    {
        private readonly IBSAdherent _bsAdherent;
        private readonly IBSCategory _bsCategory;

        /// <summary>
        /// The class Constructor
        public AdherentController(IBSAdherent bsAdherent, IBSCategory bsCategory)
        {
            _bsAdherent = bsAdherent;
            _bsCategory = bsCategory;
        }

        /// <summary>
        /// Gets all adherents
        /// </summary>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpGet]
        public async Task<IEnumerable<DtoAdherent>> Get()
        {
            return await _bsAdherent.GetListe();
        }

        /// <summary>
        /// Gets all adherents
        /// </summary>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpGet]
        [Route("category")]
        public async Task<IEnumerable<DtoCategory>> GetCategories()
        {
            return await _bsCategory.GetListe();
        }

        /// <summary>
        /// Gets all adherents
        /// </summary>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpPost]
        public async Task<IEnumerable<DtoAdherent>> GetByName([FromBody] SearchAdherent demand)
        {
            return await _bsAdherent.SearchAdherents(demand.Name, demand.PostalCode);
        }
    }
}
