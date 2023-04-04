using ApiColomiersVolley.BLL.DMArticle.Business;
using ApiColomiersVolley.BLL.DMArticle.Business.Interfaces;
using ApiColomiersVolley.BLL.DMArticle.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiColomiersVolley.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly IBSArticle _bsArticle;

        public ArticleController(ILogger<ArticleController> logger, IBSArticle bsArticle)
        {
            _logger = logger;
            _bsArticle = bsArticle;
        }

        [HttpGet]
        public async Task<IEnumerable<DtoArticle>> Get()
        {
            return await _bsArticle.GetArticles();
        }
    }
}