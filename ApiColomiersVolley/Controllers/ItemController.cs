using ApiColomiersVolley.BLL.DMItem.Business;
using ApiColomiersVolley.BLL.DMItem.Business.Interfaces;
using ApiColomiersVolley.BLL.DMItem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiColomiersVolley.Controllers
{
    [ApiController]
    [Route("api")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;
        private readonly IBSItem _bsItem;

        public ItemController(ILogger<ItemController> logger, IBSItem bsItem)
        {
            _logger = logger;
            _bsItem = bsItem;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<DtoItem>> Get()
        {
            return await _bsItem.GetListe();
        }

        [HttpGet]
        [Route("tree")]
        public async Task<IEnumerable<DtoItem>> GetTree()
        {
            return await _bsItem.GetTree();
        }

        [HttpPost]
        [Route("add")]
        public async Task<DtoItem> AddOrUpdate([FromBody] DtoItem item)
        {
            return await _bsItem.AddOrUpdate(item);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<bool> Delete([FromBody] int id)
        {
            return await _bsItem.Remove(id);
        }
    }
}