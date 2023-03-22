using ApiColomiersVolley.BLL.Core.Exceptions;
using ApiColomiersVolley.BLL.DMAuthentication.Business.Interfaces;
using ApiColomiersVolley.BLL.DMAuthentication.Exceptions;
using ApiColomiersVolley.BLL.DMAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace ApiColomiersVolley.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ColomiersVolleyController
    {
        private readonly IBSLogin _bsLogin;
        private readonly IBSAuthentication _bsAuth;
        private readonly IConfiguration _config;

        public AuthenticationController(IBSLogin bsLogin, IBSAuthentication bsAuth, IConfiguration config)
        {
            _bsLogin = bsLogin;
            _bsAuth = bsAuth;
            _config = config;
        }

        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <param name="login">The email and password for this attempt</param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        /// <returns>A Token if the connexion was successfull</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> LogIn([FromBody] Login login)
        {
            try
            {
                StringValues origin, referer;
                Request.Headers.TryGetValue("Origin", out origin);
                Request.Headers.TryGetValue("Referer", out referer);
                var ip = Request.HttpContext.Connection?.RemoteIpAddress?.ToString();
                var token = await _bsAuth.LogInUser(login, ip);
                if (token == null)
                {
                    return BadRequest();
                }

                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (MaximumLoginAttemptException ex)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests, ex.Message);
            }
            catch (AccessForbiddenException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Refresh the current user token
        /// </summary>
        /// <param name="refresh">The refresh informations to create a new token</param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        /// <returns>A Token if the refresh was successfull</returns>
        [HttpPost("token")]
        public async Task<ActionResult<UserToken>> Token([FromBody] Refresh refresh)
        {
            try
            {
                StringValues origin, referer;
                Request.Headers.TryGetValue("Origin", out origin);
                Request.Headers.TryGetValue("Referer", out referer);
                var ip = Request.HttpContext.Connection?.RemoteIpAddress?.ToString();
                var token = await _bsAuth.RefreshUser(refresh, ip);
                if (token == null)
                {
                    return Unauthorized();
                }

                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        [HttpDelete("logout")]
        public async Task<ActionResult> LogOut()
        {
            var tokenName = _config.GetSection("Token").GetValue<string>("TokenName");
            var refreshId = Request.Cookies.FirstOrDefault(c => c.Key == tokenName).Value;
            await _bsAuth.LogOutUser(refreshId);

            var cookie = new CookieBuilder
            {
                Expiration = TimeSpan.Zero,
                Path = "/",
            };

            Response.Cookies.Append(tokenName, "disconnected", cookie.Build(HttpContext));
            return new EmptyResult();
        }

        /// <summary>
        /// Identify or create a user with email
        /// </summary>
        /// <param name="userEmail">The email and the ClientID</param>
        /// <response code="200">Success / Succès de la requête</response>
        /// <response code="204">No content / Aucune donnée</response>
        /// <response code="400">Bad request / La syntaxe de la requête est erronée</response>
        /// <response code="403">Forbidden / Accès refusé:  les droits d'accès ne permettent pas au client d'accéder à la ressource</response>
        /// <response code="500">Internal Server Error / Erreur interne du serveur</response>
        /// <returns>A Token if the identification was successfull</returns>
        [HttpPost("email")]
        public async Task<ActionResult<bool>> Email([FromBody] UserEmail userEmail)
        {
            try
            {
                StringValues origin, referer;
                Request.Headers.TryGetValue("Origin", out origin);
                Request.Headers.TryGetValue("Referer", out referer);
                var ip = Request.HttpContext.Connection?.RemoteIpAddress?.ToString();
                return Ok(await _bsAuth.CheckUserEmail(userEmail));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }
        }
    }
}
