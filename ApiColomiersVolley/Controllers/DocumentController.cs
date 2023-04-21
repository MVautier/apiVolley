using ApiColomiersVolley.BLL.DMAdherent.Business;
using ApiColomiersVolley.BLL.DMAdherent.Business.Interfaces;
using ApiColomiersVolley.BLL.DMFile.Business;
using ApiColomiersVolley.BLL.DMFile.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiColomiersVolley.Controllers
{
    /// <summary>
     /// Traitement des documents
     /// </summary>
     /// <response code="200">Success / Succès de la requête</response>
     /// <response code="204">No content / Aucune donnée</response>
     /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
     /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
     /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IBSDocument _bsDocument;

        /// <summary>
        /// The class Constructor
        public DocumentController(IBSDocument bsDocument)
        {
            _bsDocument = bsDocument;
        }

        [HttpPost("SavePDF")]
        public async Task<ActionResult<string>> SavePDF([FromForm] string filename, [FromForm] string id)
        {
            if (Request.Form.Files.Count < 1 || string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            try
            {
                return Ok(await _bsDocument.SavePdf(filename, id, Request.Form.Files.First()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("SaveDocuments")]
        public async Task<ActionResult<bool>> SaveDocuments([FromForm] string id)
        {
            if (Request.Form.Files.Count < 1 || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            try
            {
                return Ok(await _bsDocument.SaveDocuments(id, Request.Form.Files));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
