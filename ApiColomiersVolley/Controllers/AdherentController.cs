using ApiColomiersVolley.BLL.Core.Models.Generic;
using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMItem.Business.Interfaces;
using ApiColomiersVolley.BLL.DMItem.Models;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

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
    public class AdherentController : ColomiersVolleyController
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
        /// Gets a paged adherents list
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="sortColumn"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpPost]
        [Route("paged")]
        public async Task<PagedList<DtoAdherent>> GetPaged(
            [FromBody] AdherentFilter filter,
            [FromQuery] string? sort = null, 
            [FromQuery] string? sortColumn = null,
            [FromQuery] int page = 0, 
            [FromQuery] int size = 10)
        {
            var pagination = new Pagination { Page = page, Size = size };
            var sorting = new Sorting { Field = sortColumn, OrderAsc = sort == "asc" };
            return await _bsAdherent.GetPagedListe(filter, sorting, pagination);
        }

        /// <summary>
        /// Search for an adherent
        /// </summary>
        /// <param name="search"></param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpPost]
        [Route("search")]
        public async Task<DtoAdherent> Search([FromBody] AdherentSearch search)
        {
            return await _bsAdherent.Search(search);
        }

        /// <summary>
        /// Gets a zip file containing all documents of a type 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="type"></param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpPost]
        [Route("docs")]
        public async Task<ActionResult> GetDocuments([FromBody] AdherentFilter filter,[FromQuery] string type)
        {
            var file = await _bsAdherent.GetDocuments(filter, type);
            if (file != null)
            {
                return File(file.Content, file.Type, file.Name);
            }

            return null;
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
        /// Gets stats on all adherents
        /// </summary>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpGet]
        [Route("stats")]
        public async Task<IEnumerable<DtoStat>> GetStats()
        {
            return await _bsAdherent.GetStats();
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
        [Route("orders")]
        public async Task<IEnumerable<DtoOrderFull>> GetOrders([FromBody] OrderFilter search)
        {
            return await _bsAdherent.GetOrders(search);
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
        [Route("searchByName")]
        public async Task<IEnumerable<DtoAdherent>> GetByName([FromBody] SearchAdherent demand)
        {
            return await _bsAdherent.SearchAdherents(demand.Name, demand.PostalCode);
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
        public async Task<DtoAdherent> AddOrUpdate([FromBody] DtoAdherent adherent)
        {
            StringValues origin, referer;
            Request.Headers.TryGetValue("Origin", out origin);
            Request.Headers.TryGetValue("Referer", out referer);
            if (string.IsNullOrEmpty(origin) && string.IsNullOrEmpty(referer))
            {

            }
            return await _bsAdherent.AddOrUpdate(adherent);
        }

        /// <summary>
        /// Permet d'obtenir le fichier Excel de la liste des comptes clients
        /// </summary>
        /// <param name="filter">Les paramètres de filtrage</param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        /// <returns>Une réponse HTTP 200 contenant les comptes</returns>
        [HttpPost]
        [Route("export")]
        public async Task<ActionResult> GetExcelFile([FromBody] AdherentFilter filter)
        {
            var file = await _bsAdherent.GetExcelFile(filter);
            return File(file.Content, file.Type, file.Name);
        }

        /// <summary>
        /// Permet d'obtenir la liste des emails des adhérents sélectionnés
        /// </summary>
        /// <param name="filter">Les paramètres de filtrage</param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        /// <returns>Une réponse HTTP 200 contenant les comptes</returns>
        [HttpPost]
        [Route("export/email")]
        public async Task<ActionResult> GetEmailFile([FromBody] AdherentFilter filter)
        {
            var file = await _bsAdherent.GetEmailFile(filter);
            return File(file.Content, file.Type, file.Name);
        }

        /// <summary>
        /// Permet d'obtenir le fichier Excel de la liste des comptes clients
        /// </summary>
        /// <param name="filter">Les paramètres de filtrage</param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        /// <returns>Une réponse HTTP 200 contenant les comptes</returns>
        [HttpPost]
        [Route("export/order")]
        public async Task<ActionResult> GetOrderFile([FromBody] AdherentFilter filter)
        {
            var file = await _bsAdherent.GetOrderFile(filter);
            return File(file.Content, file.Type, file.Name);
        }
    }
}
