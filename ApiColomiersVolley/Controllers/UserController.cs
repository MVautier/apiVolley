using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Business;
using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAdherent.Models;
using ApiColomiersVolley.BLL.DMUser.Business.Interfaces;
using ApiColomiersVolley.BLL.DMUser.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiColomiersVolley.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ColomiersVolleyController
    {
        private readonly IBSUser _bsUser;
        private readonly IBSRole _bsRole;
        private readonly IServiceSendMail _mailManager;

        /// <summary>
        /// The class Constructor
        public UserController(IBSUser bsUser, IBSRole bsRole, IServiceSendMail mailManager)
        {
            _bsUser = bsUser;
            _bsRole = bsRole;
            _mailManager = mailManager;
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
        public async Task<IEnumerable<DtoUserRole>> Get()
        {
            try
            {
                await _mailManager.SendMailSimple("Admin logged", "Call made to get Users List");
            }
            catch (Exception)
            {

            }
            
            return await _bsUser.GetListe();
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
        [Route("role")]
        public async Task<IEnumerable<DtoRole>> GetRoles()
        {
            return await _bsRole.GetRoles();
        }
    }
}
